using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using Brecham.TestsInfrastructure;

namespace ObexServer.Tests
{

    [TestFixture]
    public class Get
    {
        [Test]
        public void One()
        {
            GetTester tester = new GetTester();
            tester.SetRequestPdus(Tests_Values_Get.PduReqOne, Tests_Values_Get.PduReqGetFinalNoHeaders);
            tester.SetExpectedPdus(Tests_Values_Get.PduRspOne1, Tests_Values_Get.PduRspOne2End);
            tester.AddExpectedContent(Tests_Values.NameOfSinglePduWithName);
            tester.SetGetContent(Encoding.ASCII.GetBytes("Hello world"));
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void OneSmallerChunks()
        {
            GetTester tester = new GetTester();
            tester.SetRequestPdus(Tests_Values_Get.PduReqOne, Tests_Values_Get.PduReqGetFinalNoHeaders,
                Tests_Values_Get.PduReqGetFinalNoHeaders, Tests_Values_Get.PduReqGetFinalNoHeaders);
            tester.SetExpectedPdus(Tests_Values_Get.PduRspOneSmallerChunks1, Tests_Values_Get.PduRspOneSmallerChunks2,
                Tests_Values_Get.PduRspOneSmallerChunks3, Tests_Values_Get.PduRspOneSmallerChunks4End);
            tester.AddExpectedContent(Tests_Values.NameOfSinglePduWithName);
            Stream content = new MemoryStream(Encoding.ASCII.GetBytes("Hello world"));
            content = new SmallChunkStream(content, 4);
            tester.SetGetContent(content);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void OneSmallerChunksReadErrorOnSecond()
        {
            GetTester tester = new GetTester();
            tester.SetRequestPdus(Tests_Values_Get.PduReqOne, Tests_Values_Get.PduReqGetFinalNoHeaders);
            tester.SetExpectedPdus(Tests_Values_Get.PduRspOneSmallerChunksExceptionOnSecond1,
                Tests_Values_Get.PduRspOneSmallerChunksExceptionOnSecond2Error);
            tester.AddExpectedContent(Tests_Values.NameOfSinglePduWithName);
            Stream content = new MemoryStream(Encoding.ASCII.GetBytes("Hello world"));
            content = new SmallChunkStream(content, 4);
            content = new ExceptionOnReadStream(content, 2);
            tester.SetGetContent(content);
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void OneWithTwoRequestPackets()
        {
            GetTester tester = new GetTester();
            tester.SetRequestPdus(Tests_Values_Get.PduReqOneWithTwoRequestPackets1,
                Tests_Values_Get.PduReqOneWithTwoRequestPackets2,
                Tests_Values_Get.PduReqGetFinalNoHeaders);
            tester.SetExpectedPdus(Tests_Values_Get.PduRspContinueNoHeaders, Tests_Values_Get.PduRspOne1, Tests_Values_Get.PduRspOne2End);
            tester.AddExpectedContent(Tests_Values.NameOfSinglePduWithName);
            tester.SetGetContent(Encoding.ASCII.GetBytes("Hello world"));
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

        [Test]
        public void OneButGetNonFinalAfterFinal()
        {
            GetTester tester = new GetTester();
            tester.SetRequestPdus(Tests_Values_Get.PduReqOneButGetNonFinalAfterFinalError1,
                Tests_Values_Get.PduReqOneButGetNonFinalAfterFinalError2);
            tester.SetExpectedPdus(Tests_Values_Get.PduRspOne1, Tests_Values_Get.PduRspOneButGetNonFinalAfterFinalError);
            tester.AddExpectedContent(Tests_Values.NameOfSinglePduWithName);
            tester.SetGetContent(Encoding.ASCII.GetBytes("Hello world"));
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
        }

    }//class

}
