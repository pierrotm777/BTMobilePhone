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
    [TestFixture]
    public class InvalidPdus
    {
        //----------------------------------------------------------------------
        [Test]
        public void PutInvalidNameHeader()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutInvalidNameHeader);
            tester.SetExpectedPdus(new byte[0]);
            tester.ExpectedExitReason = ServerExitReason.InvalidClientPdu;
            tester.DoAssert();
        }

        //----------------------------------------------------------------------
        [Test]
        public void PutShortLessThan3Pdu()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdusNoLengthChecking(Tests_Values.PduPutShortLessThan3);
            tester.SetExpectedPdus(new byte[0]);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulCloseWithinAPdu;
            tester.DoAssert();
        }

        [Test]
        public void PutShortPdu()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdusNoLengthChecking(Tests_Values.PduPutShort);
            tester.SetExpectedPdus(new byte[0]);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulCloseWithinAPdu;
            tester.DoAssert();
        }

        //----------------------------------------------------------------------
        [Test]
        public void PutNoBytesSent()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(new byte[0]);
            tester.SetExpectedPdus(new byte[0]);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

    }

    [TestFixture]
    public class Put_ValidPdus
    {
        //----------------------------------------------------------------------
        [Test]
        public void PutSinglePduWithName()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutSinglePduWithName);
            tester.SetExpectedPdus(Tests_Values.PduSuccess);
            tester.AddExpectedContent(Tests_Values.NameOfSinglePduWithName, Tests_Values.ContentPutSinglePduWithName);
            //tester.SetExpectedFilename();
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void PutSinglePduWithoutName()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutFinalNonEmptyEob);
            tester.SetExpectedPdus(Tests_Values.PduBadRequestNoName);
            tester.AddExpectedContent(null, null);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void PutThreePdusNameEmptyEof()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutPduWithName1, Tests_Values.PduPutBody1, Tests_Values.PduPutFinalEmptyEob);
            tester.SetExpectedPdus(Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduSuccess);
            tester.AddExpectedContent(Tests_Values.Name1, Tests_Values.ContentZeroLength, Tests_Values.ContentBody1, Tests_Values.ContentZeroLength);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void PutThreePdusNameNonEmptyEof()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutPduWithName1, Tests_Values.PduPutBody1, Tests_Values.PduPutFinalNonEmptyEob);
            tester.SetExpectedPdus(Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduSuccess);
            tester.AddExpectedContent(Tests_Values.Name1, Tests_Values.ContentZeroLength, Tests_Values.ContentBody1, Tests_Values.ContentPutFinalNonEmptyEob);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        //--------
        [Test]
        public void PutZeroSinglePdu()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutZeroSinglePduWithName);
            tester.SetExpectedPdus(Tests_Values.PduSuccess);
            tester.AddExpectedContent("aaaa.xtx", Tests_Values.ContentZeroLength);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void PutZeroTwoPdus()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutZeroPduWithNameAlone, Tests_Values.PduPutFinalEmptyEob);
            tester.SetExpectedPdus(Tests_Values.PduContinue, Tests_Values.PduSuccess);
            tester.AddExpectedContent(Tests_Values.NameOfNameAlone, Tests_Values.ContentZeroLength);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void PutZeroOneEmptyPdu()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutZeroPduWithNameAlone, Tests_Values.PduPutEmptyBody, Tests_Values.PduPutFinalEmptyEob);
            tester.SetExpectedPdus(Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduSuccess);
            tester.AddExpectedContent(Tests_Values.NameOfNameAlone, Tests_Values.ContentZeroLength);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        //--------
        [Test]
        public void PutEobInNonFinalPdu()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutPduWithName1, Tests_Values.PduPutEobInNonFinalPdu);
            tester.SetExpectedPdus(Tests_Values.PduContinue, Tests_Values.PduBadRequestShouldntEob);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void PutBodyInFinalPdu()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutPduWithName1, Tests_Values.PduPutBodyInFinalPdu);
            tester.SetExpectedPdus(Tests_Values.PduContinue, Tests_Values.PduBadRequestShouldEob);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        //----
        [Test]
        public void PutNameIsInSecondPdu()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(Tests_Values.PduPutJustConnectionId, Tests_Values.PduPutPduWithName1, Tests_Values.PduPutBody1, Tests_Values.PduPutFinalEmptyEob);
            tester.SetExpectedPdus(Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduSuccess);
            tester.AddExpectedContent(Tests_Values.Name1, Tests_Values.ContentZeroLength, Tests_Values.ContentBody1, Tests_Values.ContentZeroLength);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        //----
        [Test]
        public void PutThreeOperations()
        {
            PutTester tester = new PutTester();
            tester.SetRequestPdus(
                /*#1*/Tests_Values.PduPutPduWithName1, Tests_Values.PduPutBody1, Tests_Values.PduPutFinalEmptyEob,
                /*#2*/Tests_Values.PduPutPduWithName2, Tests_Values.PduPutBody2, Tests_Values.PduPutBody1, Tests_Values.PduPutFinalEmptyEob,
                /*#3*/Tests_Values.PduPutSinglePduWithName);
            tester.SetExpectedPdus(
                /*#1*/Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduSuccess,
                /*#2*/Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduSuccess,
                /*#3*/Tests_Values.PduSuccess);
            tester.AddExpectedContent(/*#1*/Tests_Values.Name1, Tests_Values.ContentZeroLength, Tests_Values.ContentBody1, Tests_Values.ContentZeroLength);
            tester.AddExpectedContent(/*#2*/Tests_Values.Name2, Tests_Values.ContentBody2, Tests_Values.ContentBody1, Tests_Values.ContentZeroLength);
            tester.AddExpectedContent(/*#3*/Tests_Values.NameOfSinglePduWithName, Tests_Values.ContentPutSinglePduWithName);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }


    }//class


    //TODO PutClientCancels [TestFixture]
    public class PutClientCancels
    {
        // TODO Test--Client sends an Abort command; at various places.
        // TODO Test--Client sends a non PUT packet whilst in the middle of an operation, ie abandons it.
    }


    [TestFixture]
    public class ContentStreamErrors
    {
        public class ExceptionOnWriteRememberPutMetadataAndContentPutStreamObexServerFactory
            : PutTester.ObexServerFactory
        {
            internal int? m_failStreamOnlyOnNthOperation;

            public override IObexServer GetNewObexServer(Stream peer)
            {
                ExceptionOnWriteRememberPutMetadataAndContentPutStreamObexServer svr
                    = new ExceptionOnWriteRememberPutMetadataAndContentPutStreamObexServer(peer);
                svr.m_failStreamOnlyOnNthOperation = m_failStreamOnlyOnNthOperation;
                return svr;
            }
        }

        public class ExceptionOnWriteRememberPutMetadataAndContentPutStreamObexServer
            : PutTester.RememberPutMetadataAndContentPutStreamObexServer
        {
            internal int? m_failStreamOnlyOnNthOperation;
            int m_count = 0;

            public ExceptionOnWriteRememberPutMetadataAndContentPutStreamObexServer(Stream peer)
                : base(peer, 2048)
            {
            }

            protected override void SetServerOnHost()
            {
                this.CreatePutStream += this.EowRpmacCreateStream;
            }

            protected void EowRpmacCreateStream(Object sender, CreatePutStreamEventArgs e)
            {
                bool creatingFailingStream;
                if (!m_failStreamOnlyOnNthOperation.HasValue) {
                    // Always to create a failing stream
                    creatingFailingStream = true;
                } else {
                    if (m_count == m_failStreamOnlyOnNthOperation.Value) {
                        creatingFailingStream = true;
                    } else {
                        creatingFailingStream = false;
                    }
                    ++m_count;
                }//if--m_failStreamOnlyOnNthOperation.HasValue
                //
                CreatePutStreamEventArgs eChild = new CreatePutStreamEventArgs(e.PutMetadata, new DirectoryInfo("."));
                Stream strm;
                base.RpmacCreateStream(sender, eChild);
                strm = eChild.PutStream;
                Stream strm2;
                if (!creatingFailingStream) {
                    strm2 = strm;
                } else {
                    // Do create a failing stream
                    if (strm != null) {
                        strm2 = new ExceptionOnWriteStream(strm, 2);
                    } else {
                        strm2 = null;
                    }
                }//if--creatingFailingStream
                e.PutStream = strm2;
            }
        }//class--ExceptionOnWriteRememberPutMetadataAndContentPutStreamObexServer

        //----------------------------------------------------------------------
        [Test]
        public void PutFailOnContentStream()
        {
            PutTester tester = new PutTester(new ExceptionOnWriteRememberPutMetadataAndContentPutStreamObexServerFactory());
            tester.SetRequestPdus(Tests_Values.PduPutPduWithName2, Tests_Values.PduPutBody1, Tests_Values.PduPutBody1);
            tester.SetExpectedPdus(Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduInternalServerErrorContentIoBreakTesting);
            tester.AddExpectedContent(Tests_Values.Name2, Tests_Values.ContentZeroLength, Tests_Values.ContentBody1);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void PutThreeOperationsMiddleOneFails()
        {
            ExceptionOnWriteRememberPutMetadataAndContentPutStreamObexServerFactory factory
               = new ExceptionOnWriteRememberPutMetadataAndContentPutStreamObexServerFactory();
            factory.m_failStreamOnlyOnNthOperation = 1; //2nd operation only
            PutTester tester = new PutTester(factory);
            tester.SetRequestPdus(
                /*#1*/Tests_Values.PduPutPduWithName1, Tests_Values.PduPutBody1, Tests_Values.PduPutFinalEmptyEob,
                /*#2*/Tests_Values.PduPutPduWithName2, Tests_Values.PduPutBody2, Tests_Values.PduPutBody1,
                /*#3*/Tests_Values.PduPutSinglePduWithName);
            tester.SetExpectedPdus(
                /*#1*/Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduSuccess,
                /*#2*/Tests_Values.PduContinue, Tests_Values.PduContinue, Tests_Values.PduInternalServerErrorContentIoBreakTesting,
                /*#3*/Tests_Values.PduSuccess);
            tester.AddExpectedContent(/*#1*/Tests_Values.Name1, Tests_Values.ContentZeroLength, Tests_Values.ContentBody1, Tests_Values.ContentZeroLength);
            tester.AddExpectedContent(/*#2*/Tests_Values.Name2, Tests_Values.ContentBody2);
            tester.AddExpectedContent(/*#3*/Tests_Values.NameOfSinglePduWithName, Tests_Values.ContentPutSinglePduWithName);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }
    }//class


    //TODO ConnectionErrors [TestFixture]
    class ConnectionErrors
    {
        //TODO Test--Connection close, gracefully and not; at various points.
    }//class

}//namespace
