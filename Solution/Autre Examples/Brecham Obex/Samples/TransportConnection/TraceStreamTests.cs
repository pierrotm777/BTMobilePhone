#if NUNIT
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using Brecham.Obex.Net;


namespace TraceStreamTests
{

    [TestFixture]
    public class TraceStreamTests
    {
        public const String NewLine = "\r\n";

        public readonly byte[] WriteDataA = { 1, 4, 249, 0, 32 };
        public readonly byte[] ReadDataA = { 2, 3, 4, 5, 6, 7, 255, 8 };
        public const String ExpectedLogA
            = "Write()" + NewLine
            + "Data from Write" + NewLine
            + "01 04 F9 00 20                                    .... " + NewLine
            + "Exiting Write()" + NewLine
            //
            + "Read()" + NewLine
            + "Data from Read" + NewLine
            + "02 03 04                                          ..." + NewLine
            + "Exiting Read()" + NewLine
            //
            + "Read()" + NewLine
            + "Data from Read" + NewLine
            + "05 06 07 FF 08                                    ....." + NewLine
            + "Exiting Read()" + NewLine
            //
            + "Read()" + NewLine
            + "Data from Read" + NewLine
            //? + "" + NewLine
            + "Exiting Read()" + NewLine
            ;

        public readonly byte[] WriteDataB1 = {
            1, 4, 249, 0, 32, 1,1,1,1,1,1,1,1,1,1,1,
            0, 1, };
        public readonly byte[] WriteDataB2 = {
            2, 4, 249, 0, 32, 1,1,1,1,1,1,1,1,1,1,2, 
            1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16
        };
        public readonly byte[] WriteDataB3 = {
            3, 255, 249, };
        //public readonly byte[] ReadDataA = { 2, 3, 4, 5, 6, 7, 255, 8 };
        public const String ExpectedLogB
            = "Write()" + NewLine
            + "Data from Write" + NewLine
            + "01 04 F9 00 20 01 01 01  01 01 01 01 01 01 01 01  .... ... ........" + NewLine
            + "00 01                                             .." + NewLine
            + "Exiting Write()" + NewLine
            //
            + "Write()" + NewLine
            + "Data from Write" + NewLine
            + "02 04 F9 00 20 01 01 01  01 01 01 01 01 01 01 02  .... ... ........" + NewLine
            + "01 02 03 04 05 06 07 08  09 0A 0B 0C 0D 0E 0F 10  ........ ........" + NewLine
            + "Exiting Write()" + NewLine
            //
            + "Write()" + NewLine
            + "Data from Write" + NewLine
            //"00 11 22 33 44 55 66 77  88 99 aa bb cc dd ee ff  ........ ........" + NewLine
            + "03 FF F9                                          ..." + NewLine
            + "Exiting Write()" + NewLine
            //
            + "Read()" + NewLine
            + "Data from Read" + NewLine
            + "02 03 04                                          ..." + NewLine
            + "Exiting Read()" + NewLine
            //
            + "Read()" + NewLine
            + "Data from Read" + NewLine
            + "05 06 07 FF 08                                    ....." + NewLine
            + "Exiting Read()" + NewLine
            //
            + "Read()" + NewLine
            + "Data from Read" + NewLine
            //? + "" + NewLine
            + "Exiting Read()" + NewLine
            ;

        [Test]
        public void TestA()
        {
            MemoryStream childSrc = new MemoryStream(ReadDataA);
            MemoryStream childDest = new MemoryStream();
            PairOfStream child = new PairOfStream(childSrc, childDest);
            //
            MemoryStream logX = new MemoryStream();
            StreamWriter log = new StreamWriter(logX);
            TraceStream strm = new TraceStream(child, log);
            //
            byte[] data1 = WriteDataA;
            strm.Write(data1, 0, data1.Length);
            Assert.AreEqual(WriteDataA, childDest.ToArray(), "written data");
            //
            byte[] buf = new byte[100];
            int readLen;
            int offset = 0;
            readLen = strm.Read(buf, offset, 3);
            Assert.AreEqual(3, readLen, "3, readLen");
            offset += readLen;
            readLen = strm.Read(buf, offset, 10);
            Assert.AreEqual(5, readLen, "10->5, readLen");
            offset += readLen;
            readLen = strm.Read(buf, offset, 1);
            Assert.AreEqual(0, readLen, "0, readLen");
            //--
            strm.Dispose();
            byte[] logBytes = logX.ToArray();
            String output = Encoding.ASCII.GetString(logBytes);
            Assert.AreEqual(ExpectedLogA, output, "log output");
            //
            Assert_IsClosed(childDest, "childDest");
            Assert_IsClosed(childSrc, "childSrc");
            Assert_IsClosed(child, "child");
            Assert_IsClosed(logX, "logX");
        }

        [Test]
        public void TestLongData()
        {
            MemoryStream childSrc = new MemoryStream(ReadDataA);
            MemoryStream childDest = new MemoryStream();
            PairOfStream child = new PairOfStream(childSrc, childDest);
            //
            MemoryStream logX = new MemoryStream();
            StreamWriter log = new StreamWriter(logX);
            TraceStream strm = new TraceStream(child, log);
            //
            strm.Write(WriteDataB1, 0, WriteDataB1.Length);
            strm.Write(WriteDataB2, 0, WriteDataB2.Length);
            strm.Write(WriteDataB3, 0, WriteDataB3.Length);
            byte[] expectedWrittenData;
            ConcatentateArrays(out expectedWrittenData, WriteDataB1, WriteDataB2, WriteDataB3);
            Assert.AreEqual(expectedWrittenData, childDest.ToArray(), "written data");
            //
            byte[] buf = new byte[100];
            int readLen;
            int offset = 0;
            readLen = strm.Read(buf, offset, 3);
            Assert.AreEqual(3, readLen, "3, readLen");
            offset += readLen;
            readLen = strm.Read(buf, offset, 10);
            Assert.AreEqual(5, readLen, "10->5, readLen");
            offset += readLen;
            readLen = strm.Read(buf, offset, 1);
            Assert.AreEqual(0, readLen, "0, readLen");
            //--
            strm.Dispose();
            byte[] logBytes = logX.ToArray();
            String output = Encoding.ASCII.GetString(logBytes);
            Assert.AreEqual(ExpectedLogB, output, "log output");
            //
            Assert_IsClosed(childDest, "childDest");
            Assert_IsClosed(childSrc, "childSrc");
            Assert_IsClosed(child, "child");
            Assert_IsClosed(logX, "logX");
        }

        private void ConcatentateArrays<T>(out T[] concatenation, params T[][] arrays)
        {
            int totalLength=0;
            foreach(T[] cur in arrays){
                totalLength += cur.Length;
            }
            concatenation = new T[totalLength];
            int offset = 0;
            foreach (T[] cur in arrays) {
                cur.CopyTo(concatenation, offset);
                offset += cur.Length;
            }
            System.Diagnostics.Debug.Assert(offset == concatenation.Length, "offset == concatenation.Length");
        }

        private void Assert_IsClosed(Stream stream, String message)
        {
            try {
                stream.ReadByte();
                Assert.Fail(message + "-- not closed");
            } catch (ObjectDisposedException) { }
        }

    }//class


    public class PairOfStream : Brecham.Obex.Net.ForV1CloseDisposeLikeV2Stream
    {
        Stream m_readStrm;
        Stream m_writeStrm;

        protected override void Dispose(bool disposing)
        {
            try {
                m_readStrm.Close();
            } finally {
                try {
                    m_writeStrm.Close();
                } finally {
                    base.Dispose(disposing);
                }
            }
        }


        public PairOfStream(Stream readStrm, Stream writeStrm)
        {
            m_readStrm = readStrm;
            m_writeStrm = writeStrm;
        }



        public override bool CanRead { get { return m_readStrm.CanRead; } }

        public override bool CanWrite { get { return m_writeStrm.CanWrite; } }

        public override bool CanSeek
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override void Flush()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override long Position
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_readStrm.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            m_writeStrm.Write(buffer, offset, count);
        }
    }//class

}
#endif
