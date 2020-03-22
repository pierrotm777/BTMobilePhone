using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Brecham.Obex.Pdus;

namespace ObexServer.Tests
{
    [TestFixture]
    public class Test_SetPath
    {
        // SetPath documentation is a wee bit unclear.  Implying a difference between 
        // an "empty" name header, and an "ommited" header, and implying that an 
        // "empty" header causes a Reset, but not specifing what ommitted+NoFlags 
        // means and thus it can only mean Reset too...
        //
        // Brecham.Obex client library sends:
        // * Reset as: empty Name header,   flags={ DoNotCreate }.
        // * Up as   : omitted Name header, flags={ Up + DoNotCreate }.
        //
        // In the server we'll consider the empty and omitted Name header cases 
        // the same, and use them respectively as:
        // * Reset: if flags does NOT include Up.
        // * Up   : if flags DOES include Up.


        //--------
        private static void DoTest(byte[] pdu, string expectedName, FolderChangeType expectedType, bool expectedMayCreate)
        {
            DoTest(pdu, null, Tests_Values.PduSuccess, expectedName, expectedType, expectedMayCreate);

        }
        private static void DoTest(byte[] pdu, ObexCreatedPdu errorResponsePdu, byte[] expectedResponsePdu, string expectedName, FolderChangeType expectedType, bool expectedMayCreate)
        {
            SetPathTester tester = new SetPathTester();
            tester.SetRequestPdus(pdu);
            tester.SetExpectedPdus(expectedResponsePdu);
            tester.m_setPathResponsePdu = errorResponsePdu;
            tester.ExpectedExitReason = ServerExitReason.ConnectionGracefulClose;
            tester.DoAssert();
            Assert.AreEqual(1, tester.m_setPathEventArgsList.Count, "ea_count");
            Assert.AreEqual(expectedName, tester.m_setPathEventArgsList[0].ChildFolderName, "expectedName");
            Assert.AreEqual(expectedType, tester.m_setPathEventArgsList[0].FolderChangeType, "expectedType");
            Assert.AreEqual(expectedMayCreate, tester.m_setPathEventArgsList[0].MayCreateIfNotExist, "expectedMayCreate");
        }

        //--------
        [Test]
        public void ReportError()
        {
            ObexPduFactory f = new ObexPduFactory(10000);
            ObexCreatedPdu rspPdu = f.CreateResponse(ObexResponseCode.Conflict, null, null);
            DoTest(SetPathNameFlags0, rspPdu, Tests_Values.PduConflict, FolderName, FolderChangeType.Down, true);
        }
        //--------
        [Test]
        public void Name0()
        {
            DoTest(SetPathNameFlags0, FolderName, FolderChangeType.Down, true);
        }
        [Test]
        public void Name1_UpAndDown()
        {
            DoTest(SetPathNameFlags1_UpAndDown, FolderName, FolderChangeType.UpAndDown, true);
        }
        [Test]
        public void Name2()
        {
            DoTest(SetPathNameFlags2, FolderName, FolderChangeType.Down, false);
        }
        [Test]
        public void Name3_UpAndDown()
        {
            DoTest(SetPathNameFlags3_UpAndDown, FolderName, FolderChangeType.UpAndDown, false);
        }
        [Test]
        public void NameBad0()
        {
            DoTest(SetPathNameFlagsBadBits0, FolderName, FolderChangeType.Down, true);
        }

        //--------
        const bool UpAndResetMayCreateValueIsIgnored = true;
        // Note: true && !UpAndResetMayCreateValueIsIgnored == false

        [Test]
        public void NoHeader0()
        {
            DoTest(SetPathNoHeadersFlags0, null, FolderChangeType.Reset, true && !UpAndResetMayCreateValueIsIgnored);
        }
        [Test]
        public void NoHeader1_Up()
        {
            DoTest(SetPathNoHeadersFlags1_Up, null, FolderChangeType.Up, true && !UpAndResetMayCreateValueIsIgnored);
        }
        [Test]
        public void NoHeader2()
        {
            DoTest(SetPathNoHeadersFlags2, null, FolderChangeType.Reset, false);
        }
        [Test]
        public void NoHeader3_Up()
        {
            DoTest(SetPathNoHeadersFlags3_Up, null, FolderChangeType.Up, false);
        }
        [Test]
        public void NoHeaderBad0()
        {
            DoTest(SetPathNoHeadersFlagsBadBits0, null, FolderChangeType.Reset, true && !UpAndResetMayCreateValueIsIgnored);
        }

        //--------
        [Test]
        public void Null0()
        {
            DoTest(SetPathNullNameFlags0, null, FolderChangeType.Reset, true && !UpAndResetMayCreateValueIsIgnored);
        }
        [Test]
        public void Null1_Up()
        {
            DoTest(SetPathNullNameFlags1_Up, null, FolderChangeType.Up, true && !UpAndResetMayCreateValueIsIgnored);
        }
        [Test]
        public void Null2()
        {
            DoTest(SetPathNullNameFlags2, null, FolderChangeType.Reset, false);
        }
        [Test]
        public void Null3_Up_IgnoreMayCreate()
        {
            DoTest(SetPathNullNameFlags3_Up, null, FolderChangeType.Up, false);
        }
        [Test]
        public void NullBad0()
        {
            DoTest(SetPathNullNameFlagsBadBits0, null, FolderChangeType.Reset, true && !UpAndResetMayCreateValueIsIgnored);
        }

        //--------
        [Test]
        public void Empty0()
        {
            DoTest(SetPathEmptyNameFlags0, null, FolderChangeType.Reset, true && !UpAndResetMayCreateValueIsIgnored);
        }
        [Test]
        public void Empty1_Up()
        {
            DoTest(SetPathEmptyNameFlags1_Up, null, FolderChangeType.Up, true && !UpAndResetMayCreateValueIsIgnored);
        }
        [Test]
        public void Empty2()
        {
            DoTest(SetPathEmptyNameFlags2, null, FolderChangeType.Reset, false);
        }
        [Test]
        public void Empty3_Up_IgnoreMayCreate()
        {
            DoTest(SetPathEmptyNameFlags3_Up, null, FolderChangeType.Up, false);
        }
        [Test]
        public void EmptyBad0()
        {
            DoTest(SetPathEmptyNameFlagsBadBits0, null, FolderChangeType.Reset, true && !UpAndResetMayCreateValueIsIgnored);
        }

        //--------
        [Test]
        public void ObexLibTests_setPathReset()
        {
            DoTest(ObexLibTests_setPathReset_Pdu, null, FolderChangeType.Reset, false);
        }
        [Test]
        public void ObexLibTests_setPathUp()
        {
            DoTest(ObexLibTests_setPathUp_Pdu, null, FolderChangeType.Up, false);
        }
        [Test]
        public void ObexLibTests_setPathDown_simple()
        {
            DoTest(ObexLibTests_setPathDown_simple_Pdu, "F1", FolderChangeType.Down, false);
        }
        [Test]
        public void ObexLibTests_setPathUpAndDown()
        {
            DoTest(ObexLibTests_setPathUpAndDown_Pdu, "F3", FolderChangeType.UpAndDown, true);
        }

        //--------
        //
        const string FolderName = "ab";
        static readonly byte[] SetPathNameFlags0 = { Tests_Values.OpcodeSetpath, 0, 14, 0, 0, Tests_Values.HeaderName, 0, 9, 0, (byte)'a', 0, (byte)'b', 0, 0, };
        static readonly byte[] SetPathNameFlags1_UpAndDown = { Tests_Values.OpcodeSetpath, 0, 14, 1, 0, Tests_Values.HeaderName, 0, 9, 0, (byte)'a', 0, (byte)'b', 0, 0, };
        static readonly byte[] SetPathNameFlags2 = { Tests_Values.OpcodeSetpath, 0, 14, 2, 0, Tests_Values.HeaderName, 0, 9, 0, (byte)'a', 0, (byte)'b', 0, 0, };
        static readonly byte[] SetPathNameFlags3_UpAndDown = { Tests_Values.OpcodeSetpath, 0, 14, 3, 0, Tests_Values.HeaderName, 0, 9, 0, (byte)'a', 0, (byte)'b', 0, 0, };
        static readonly byte[] SetPathNameFlagsBadBits0 = { Tests_Values.OpcodeSetpath, 0, 14, 0xFC, 0, Tests_Values.HeaderName, 0, 9, 0, (byte)'a', 0, (byte)'b', 0, 0, };
        //
        static readonly byte[] SetPathNoHeadersFlags0 = { Tests_Values.OpcodeSetpath, 0, 5, 0, 0, };
        // send a SETPATH (generally with no Name header) and the “back up a level” flag set.
        static readonly byte[] SetPathNoHeadersFlags1_Up = { Tests_Values.OpcodeSetpath, 0, 5, 1, 0, };
        static readonly byte[] SetPathNoHeadersFlags2 = { Tests_Values.OpcodeSetpath, 0, 5, 2, 0, };
        static readonly byte[] SetPathNoHeadersFlags3_Up = { Tests_Values.OpcodeSetpath, 0, 5, 3, 0, };
        static readonly byte[] SetPathNoHeadersFlagsBadBits0 = { Tests_Values.OpcodeSetpath, 0, 5, 0xFC, 0, };
        //
        static readonly byte[] SetPathNullNameFlags0 = { Tests_Values.OpcodeSetpath, 0, 8, 0, 0, Tests_Values.HeaderName, 0, 3, };
        static readonly byte[] SetPathNullNameFlags1_Up = { Tests_Values.OpcodeSetpath, 0, 8, 1, 0, Tests_Values.HeaderName, 0, 3, };
        static readonly byte[] SetPathNullNameFlags2 = { Tests_Values.OpcodeSetpath, 0, 8, 2, 0, Tests_Values.HeaderName, 0, 3, };
        static readonly byte[] SetPathNullNameFlags3_Up = { Tests_Values.OpcodeSetpath, 0, 8, 3, 0, Tests_Values.HeaderName, 0, 3, };
        static readonly byte[] SetPathNullNameFlagsBadBits0 = { Tests_Values.OpcodeSetpath, 0, 8, 0xFC, 0, Tests_Values.HeaderName, 0, 3, };
        //
        static readonly byte[] SetPathEmptyNameFlags0 = { Tests_Values.OpcodeSetpath, 0, 10, 0, 0, Tests_Values.HeaderName, 0, 5, 0, 0, };
        static readonly byte[] SetPathEmptyNameFlags1_Up = { Tests_Values.OpcodeSetpath, 0, 10, 1, 0, Tests_Values.HeaderName, 0, 5, 0, 0, };
        static readonly byte[] SetPathEmptyNameFlags2 = { Tests_Values.OpcodeSetpath, 0, 10, 2, 0, Tests_Values.HeaderName, 0, 5, 0, 0, };
        static readonly byte[] SetPathEmptyNameFlags3_Up = { Tests_Values.OpcodeSetpath, 0, 10, 3, 0, Tests_Values.HeaderName, 0, 5, 0, 0, };
        static readonly byte[] SetPathEmptyNameFlagsBadBits0 = { Tests_Values.OpcodeSetpath, 0, 10, 0xFC, 0, Tests_Values.HeaderName, 0, 5, 0, 0, };
        //
        byte[] ObexLibTests_setPathReset_Pdu ={ 0x85, 0, 8, /*SP bytes*/0x02, 0, /*Name*/0x01, 0, 3 };
        byte[] ObexLibTests_setPathUp_Pdu ={ 0x85, 0, 5, /*SP bytes*/0x03, 0, };
        byte[] ObexLibTests_setPathDown_simple_Pdu = { 0x85, 0, 14, /*SP bytes*/0x02, 0, 
                /**/0x01, 0, 9, 0,(byte)'F', 0,(byte)'1', 0,0 };
        byte[] ObexLibTests_setPathUpAndDown_Pdu ={ 0x85, 0,14, /*SP bytes*/0x01, 0, 
            /*Name*/0x01, 0,9, 0,(byte)'F', 0,(byte)'3', 0,0
        };

    }//class
}
