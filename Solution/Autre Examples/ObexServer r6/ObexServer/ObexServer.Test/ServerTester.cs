using System;
using System.Collections.Generic;
using System.Text;
//
using System.IO;
//
using NUnit.Framework;
using Brecham.TestsInfrastructure;

namespace ObexServer.Tests
{

    public abstract class ServerTester
    {
        //----------------------------------------------------------
        protected ObexServerFactory m_factory;
        //
        protected byte[][] m_requests;
        protected bool m_lengthCheckRequests;
        protected byte[][] m_expectedPdus;
        public struct ExpectedContents
        {
            //public readonly 
            byte[][] m_expectedContents;
            readonly String m_expectedName;

            public ExpectedContents(String name, byte[][] contents)
            {
                m_expectedContents = contents;
                m_expectedName = name;
            }

            public String ExpectedName { get { return m_expectedName; } }
            public byte[] ExpectedContent
            {
                get
                {
                    byte[] expectedContent = CombineContentBuffers(m_expectedContents);
                    return expectedContent;
                }
            }
        }//struct
        protected List<ExpectedContents> m_expectedObjects = new List<ExpectedContents>();
        protected ServerExitReason m_expectedExitReason;
        protected PairOfStream m_strmPair;
        protected IObexServer m_svr;
        protected ObexServerHost m_svrHost;

        //----------------------------------------------------------
        public ServerTester(ObexServerFactory factory)
        {
            m_factory = factory;
        }

        //----------------------------------------------------------
        public void SetRequestPdus(params byte[][] pdus)
        {
            if (m_requests != null)
                throw new InvalidOperationException("Request PDUs already set.");
            m_requests = pdus;
            m_lengthCheckRequests = true;
        }
        public void SetRequestPdusNoLengthChecking(params byte[][] pdus)
        {
            if (m_requests != null)
                throw new InvalidOperationException("Request PDUs already set.");
            m_requests = pdus;
            m_lengthCheckRequests = false;
        }

        public void SetExpectedPdus(params byte[][] pdus) { m_expectedPdus = pdus; }

        public void AddExpectedContent(String name, params byte[][] content)
        {
            ExpectedContents item = new ExpectedContents(name, content);
            m_expectedObjects.Add(item);
        }

        public ServerExitReason ExpectedExitReason { set { m_expectedExitReason = value; } }

        //----------------------------------------------------------------------
        protected virtual IObexServer GetServer(Stream peer)
        {
            //RememberPutMetadataAndContentPutStreamObexServer svr = new RememberPutMetadataAndContentPutStreamObexServer(peer);
            IObexServer svr = m_factory.GetNewObexServer(peer);
            return svr;
        }

        protected virtual ObexServerHost GetServerHost(Stream peer, int bufferSize)
        {
            ObexServerHost host;
            //HACK host = new AsyncObexServerHost(peer, bufferSize, pduHandler);//HACK
            host = new SyncObexServerHost(peer, bufferSize);//HACK
            return host;
        }

        protected byte[] GetServerResponses(byte[] requests)
        {
            MemoryStream src = new MemoryStream(requests, false);
            MemoryStream write = new MemoryStream();
            m_strmPair = new PairOfStream(src, write);
            m_svr = GetServer(m_strmPair);
            m_svrHost = GetServerHost(m_strmPair, 2048);
            m_svrHost.AddHandler(m_svr);
            InitServer();
            m_svrHost.Start();
            m_svrHost.ExitWaitHandle.WaitOne();
            m_svrHost.RethrowAnyErrorInTIEx();
            //
            return write.ToArray();
        }

        protected abstract void InitServer();

        //----------------------------------------------------------
        protected static byte[] CombineRequestPduBuffers(params byte[][] buffers)
        {
            return CombineRequestPduBuffers(true, buffers);

        }
        protected static byte[] CombineRequestPduBuffers(bool doLengthCheck, params byte[][] buffers)
        {
            return CombineBuffers_("Request", doLengthCheck, buffers);

        }
        protected static byte[] CombineResponsePduBuffers(params byte[][] buffers)
        {
            return CombineBuffers_("Response", true, buffers);

        }
        private static byte[] CombineContentBuffers(params byte[][] buffers)
        {
            return CombineBuffers_("Content", false, buffers);

        }
        private static byte[] CombineBuffers_(string bufferName, bool checkPduLengths, params byte[][] buffers)
        {
            if (buffers == null) { return null; }
            //
            int totalLength = 0;
            int i = 0;
            foreach (byte[] curBuf in buffers) {
                totalLength += curBuf.Length;
                if (checkPduLengths && curBuf.Length != 0) {
                    // Can give warning if PDU obviously invalid.
                    Int16 field = BitConverter.ToInt16(curBuf, 1); //Int16 to suit next NTHO
                    int pduGivenLength = System.Net.IPAddress.NetworkToHostOrder(field);
                    if (pduGivenLength != curBuf.Length) {
                        //System.Diagnostics.StackTrace sf = new System.Diagnostics.StackTrace();
                        //Console.WriteLine(
                        throw new ArgumentOutOfRangeException(null,
                            "PduLength " + pduGivenLength + " != ArrayLength " + curBuf.Length
                            + " in buffer '" + bufferName + "' #" + i);
                    }
                }
                ++i;
            }
            byte[] combinedBuf = new byte[totalLength];
            int offset = 0;
            foreach (byte[] curBuf in buffers) {
                curBuf.CopyTo(combinedBuf, offset);
                offset += curBuf.Length;
            }
            System.Diagnostics.Debug.Assert(offset == totalLength);
            return combinedBuf;
        }

        //----------------------------------------------------------
        public void DoAssert()
        {
            BeforeAssert();
            //
            byte[] buf;
            buf = CombineRequestPduBuffers(m_lengthCheckRequests, m_requests);
            // Do it!
            byte[] responses = GetServerResponses(buf);
            //
            //----------------------------------------------------------
            // Done, now check its behaviour.
            //----------------------------------------------------------
            Assert.IsFalse(m_strmPair.CanWrite, "Server should always close the connection(write).");
            Assert.IsFalse(m_strmPair.CanRead, "Server should always close the connection(read).");
            //
            // Response PDUs.
            Assert_ObexPackets(m_expectedPdus, responses, "Responses");
            buf = CombineResponsePduBuffers(m_expectedPdus);
            Assert.AreEqual(buf, responses, "Responses");
            //
            // Exit reason.
            Assert.AreEqual(m_expectedExitReason, m_svrHost.ExitReason, "ExitReason");
            //
            AfterAssert();
        }

        protected abstract void BeforeAssert();
        protected abstract void AfterAssert();

        //----------------------------------------------------------
        static void Assert_ObexPackets(byte[][] expectedPdus, byte[] actualPdus, string message)
        {
            List<byte[]> actualPdusList = new List<byte[]>(
                expectedPdus.Length); //guess final length
            int i;
            for (i = 0; i < actualPdus.Length; ) {
                int curPduLength = ReadPduLength(actualPdus, i);
                byte[] buf = new byte[curPduLength];
                Array.Copy(actualPdus, i, buf, 0, buf.Length);
                actualPdusList.Add(buf);
                i += curPduLength;
            }//while
            // Check an exact number of PDU in the buffer
            if (i != actualPdus.Length) {
                // What to throw here, AssertException or something else?
                throw new ArgumentException("Assert_ObexPackets did not find compete PDUs in the buffer.");
            }
            //
            Assert_ObexPackets(expectedPdus, actualPdusList.ToArray(), message);
        }

        private static int ReadPduLength(byte[] actualPdus, int i)
        {
            int curPduLength = actualPdus[i + 1] * 256 + actualPdus[i + 2];
            return curPduLength;
        }

        static void Assert_ObexPackets(byte[][] expectedPdus, byte[][] actualPdus, string message)
        {
            //HACK Assert_ObexPackets
            if (actualPdus.Length == 0 && expectedPdus.Length == 1 && expectedPdus[0].Length == 0) {
                // The tests where no responses are expected are configured with a 
                // single empty element, so detect that case here, checking the actual
                // result matches too.
                return;
            }
            // Real checking now...
            //
            Assert.AreEqual(expectedPdus.Length, actualPdus.Length, message + " -- Number of PDUs");
            //
            const int PduCodeOffset = 0;
            for (int i = 0; i < expectedPdus.Length; ++i) {
                //TODO check the PDU content before checking the length
                Assert.AreEqual(expectedPdus[i][PduCodeOffset], actualPdus[i][PduCodeOffset], message + " -- PDU Code at PDUs #" + i);
                //......
                // The old way, just do a raw check on the array contents.
                Assert.AreEqual(expectedPdus[i], actualPdus[i], message + " -- at PDUs #" + i);
            }//for
        }

        //----------------------------------------------------------
        public abstract class ObexServerFactory
        {
            public abstract IObexServer GetNewObexServer(Stream peer);
        }

        //----------------------------------------------------------
    }//class

}
