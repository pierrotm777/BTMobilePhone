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

    public static class Tests_Values
    {
        //----------------------------------------------------------
        //
        public const byte OpcodePutFinal = 0x82;
        public const byte OpcodePutNonFinal = 0x02;
        public const byte OpcodeGetFinal = 0x83;
        public const byte OpcodeGetNonFinal = 0x03;
        public const byte OpcodeSetpath = 0x85;
        //
        public const byte RespcodeContinue = 0x90;
        public const byte RespcodeSuccess = 0xA0;
        public const byte RespcodeBadRequest = 0xC0;
        public const byte RespcodeConflict= 0xC9;
        public const byte RespcodeInternalServerError = 0xD0;
        //
        public const byte HeaderName = 0x01;
        public const byte HeaderDescription = 0x05;
        public const byte HeaderBody = 0x48;
        public const byte HeaderEob = 0x49;
        public const byte HeaderConnId = 0xCB;
        public const byte HeaderLength = 0xC3;
        //
        public static readonly byte[] PduSuccess = { RespcodeSuccess, 0, 3 };
        public static readonly byte[] PduContinue = { RespcodeContinue, 0, 3 };
        public static readonly byte[] PduConflict = { RespcodeConflict, 0, 3 };
        public static readonly byte[] PduBadRequest = { RespcodeBadRequest, 0, 3 };
        public static readonly byte[] PduBadRequestNoName = { 
            RespcodeBadRequest, 0,68,
            HeaderDescription,0,65,0,(byte)'N',0,(byte)'o',0,(byte)' ',0,(byte)'N',
            0,(byte)'a',0,(byte)'m',0,(byte)'e',0,(byte)' ',0,(byte)'g',0,(byte)'i',
            0,(byte)'v',0,(byte)'e',0,(byte)' ',0,(byte)'i',0,(byte)'n',0,(byte)' ',
            0,(byte)'P',0,(byte)'U',0,(byte)'T',0,(byte)' ',0,(byte)'o',0,(byte)'p',
            0,(byte)'e',0,(byte)'r',0,(byte)'a',0,(byte)'t',0,(byte)'i',0,(byte)'o',
            0,(byte)'n',0,(byte)'.',
            0,0,
            };
        public static readonly byte[] PduBadRequestShouldntEob = { 
                RespcodeBadRequest, 0,102,
                HeaderDescription, 0,99,/**/0,(byte)'R',0,(byte)'e',0,(byte)'q',0,(byte)'u',
                0,(byte)'e',0,(byte)'s',0,(byte)'t',0,(byte)' ',0,(byte)'P',0,(byte)'D',
                0,(byte)'U',0,(byte)' ',0,(byte)'s',0,(byte)'h',0,(byte)'o',0,(byte)'u',
                0,(byte)'l',0,(byte)'d',0,(byte)'n',0,(byte)'\'',0,(byte)'t',0,(byte)' ',
                0,(byte)'c',0,(byte)'o',0,(byte)'n',0,(byte)'t',0,(byte)'a',0,(byte)'i',
                0,(byte)'n',0,(byte)' ',0,(byte)'E',0,(byte)'n',0,(byte)'d',0,(byte)'O',
                0,(byte)'f',0,(byte)'B',0,(byte)'o',0,(byte)'d',0,(byte)'y',0,(byte)' ',
                0,(byte)'h',0,(byte)'e',0,(byte)'a',0,(byte)'d',0,(byte)'e',0,(byte)'r',
                0,(byte)'.', 0,0
            };
        public static readonly byte[] PduBadRequestShouldEob = { 
                RespcodeBadRequest, 0,96,
                HeaderDescription, 0,93,/**/0,(byte)'R',0,(byte)'e',0,(byte)'q',0,(byte)'u',
                0,(byte)'e',0,(byte)'s',0,(byte)'t',0,(byte)' ',0,(byte)'P',0,(byte)'D',
                0,(byte)'U',0,(byte)' ',0,(byte)'s',0,(byte)'h',0,(byte)'o',0,(byte)'u',
                0,(byte)'l',0,(byte)'d',/*0,(byte)'n',0,(byte)'\'',0,(byte)'t',*/0,(byte)' ',
                0,(byte)'c',0,(byte)'o',0,(byte)'n',0,(byte)'t',0,(byte)'a',0,(byte)'i',
                0,(byte)'n',0,(byte)' ',0,(byte)'E',0,(byte)'n',0,(byte)'d',0,(byte)'O',
                0,(byte)'f',0,(byte)'B',0,(byte)'o',0,(byte)'d',0,(byte)'y',0,(byte)' ',
                0,(byte)'h',0,(byte)'e',0,(byte)'a',0,(byte)'d',0,(byte)'e',0,(byte)'r',
                0,(byte)'.', 0,0
            };
        public static readonly byte[] PduInternalServerErrorContentIoBreakTesting = { 
                RespcodeInternalServerError, 0,130,
                HeaderDescription, 0,127,/**/0,(byte)'W',0,(byte)'r',0,(byte)'i',
            0,(byte)'t',0,(byte)'e',0,(byte)' ',0,(byte)'t',0,(byte)'o',0,(byte)' ',
            0,(byte)'c',0,(byte)'o',0,(byte)'n',0,(byte)'t',0,(byte)'e',0,(byte)'n',
            0,(byte)'t',0,(byte)' ',0,(byte)'s',0,(byte)'t',0,(byte)'o',0,(byte)'r',
            0,(byte)'e',0,(byte)' ',0,(byte)'f',0,(byte)'a',0,(byte)'i',0,(byte)'l',
            0,(byte)'e',0,(byte)'d',0,(byte)' ',0,(byte)'w',0,(byte)'i',0,(byte)'t',
            0,(byte)'h',0,(byte)':',0,(byte)' ',0,(byte)'T',0,(byte)'e',0,(byte)'s',
            0,(byte)'t',0,(byte)'i',0,(byte)'n',0,(byte)'g',0,(byte)' ',0,(byte)'b',
            0,(byte)'r',0,(byte)'e',0,(byte)'a',0,(byte)'k',0,(byte)' ',0,(byte)'c',
            0,(byte)'o',0,(byte)'n',0,(byte)'n',0,(byte)'e',0,(byte)'c',0,(byte)'t',
            0,(byte)'i',0,(byte)'o',0,(byte)'n',0,(byte)'.',
            0,0
            };

        //----------------------------------------------------------------------
        public static readonly byte[] PduPutShortLessThan3 = {
            OpcodePutFinal, 0, /* incomplete! */
        };

        public static readonly byte[] PduPutShort = {
                OpcodePutFinal, 0,37,
                HeaderName, 0,21, 0,0x61, 0,0x61, 0,0x61, 0,0x61, 0,0x2e, 0,0x78, 0,0x74, 0,0x78, 0,0,
                //incomplete HeaderEob,  0,13, 1,2,3,4,0x99,6,7,8,9,0xFF
        };

        //----------------------------------------------------------------------
        public const String NameOfSinglePduWithName = "dddd.txt";
        public static readonly byte[] PduPutSinglePduWithName = {
                OpcodePutFinal, 0,37,
                HeaderName, 0,21, 0,0x64, 0,0x64, 0,0x64, 0,0x64, 0,0x2e, 0,0x74, 0,0x78, 0,0x74, 0,0,
                HeaderEob,  0,13, 1,2,3,4,0x99,6,7,8,9,0xFF
            };
        public static readonly byte[] ContentPutSinglePduWithName = { 1, 2, 3, 4, 0x99, 6, 7, 8, 9, 0xFF };
        //
        public static readonly byte[] PduPutZeroSinglePduWithName = {
                OpcodePutFinal, 0,27,
                HeaderName, 0,21, 0,0x61, 0,0x61, 0,0x61, 0,0x61, 0,0x2e, 0,0x78, 0,0x74, 0,0x78, 0,0,
                HeaderEob,  0,3,
            };
        public static readonly byte[] ContentZeroLength ={ };
        //
        public static readonly byte[] PduPutInvalidNameHeader = {
                OpcodePutFinal, 0,37,
                HeaderName, 1,255, 0,0x61, 0,0x61, 0,0x61, 0,0x61, 0,0x2e, 0,0x78, 0,0x74, 0,0x78, 0,0,
                HeaderEob,  0,13, 1,2,3,4,0x99,6,7,8,9,0xFF
            };
        //
        public const String Name1 = "aaaa.txt";
        public static readonly byte[] PduPutPduWithName1
            = { OpcodePutNonFinal, 0,24,
                HeaderName, 0,21, 0,0x61, 0,0x61, 0,0x61, 0,0x61, 0,0x2e, 0,0x74, 0,0x78, 0,0x74, 0,0,
            };
        public const String Name2 = "bbbb.txt";
        public static readonly byte[] PduPutPduWithName2
            = { OpcodePutNonFinal, 0,24,
                HeaderName, 0,21, 0,0x62, 0,0x62, 0,0x62, 0,0x62, 0,0x2e, 0,0x74, 0,0x78, 0,0x74, 0,0,
            };
        public const String NameOfNameAlone = "cccc.txt";
        public static readonly byte[] PduPutZeroPduWithNameAlone = {
                OpcodePutNonFinal, 0,24,
                HeaderName, 0,21, 0,0x63, 0,0x63, 0,0x63, 0,0x63, 0,0x2e, 0,0x74, 0,0x78, 0,0x74, 0,0,
            };
        public static readonly byte[] PduPutJustConnectionId = {
                OpcodePutNonFinal, 0,8,
                HeaderConnId, 0,0x2e,0xFF,0x78,
            };
        //----
        public static readonly byte[] PduPutEmptyBody
            = { OpcodePutNonFinal, 0,6,
                HeaderBody,  0,3,
            };
        public static readonly byte[] PduPutBody1
            = { OpcodePutNonFinal, 0,16,
                HeaderBody,  0,13, 1,2,3,4,0x99,6,7,8,9,0xFF
            };
        public static readonly byte[] ContentBody1 = { 1, 2, 3, 4, 0x99, 6, 7, 8, 9, 0xFF };
        //
        public static readonly byte[] PduPutBody2
            = { OpcodePutNonFinal, 0,16,
                HeaderBody,  0,13, 111,12,30,41,0x9,246,75,80,19,0xFF
            };
        public static readonly byte[] ContentBody2 = { 111, 12, 30, 41, 0x9, 246, 75, 80, 19, 0xFF };
        //----
        public static readonly byte[] PduPutFinalNonEmptyEob
            = { OpcodePutFinal, 0,16,
                HeaderEob,  0,13, 99,88,255,1,2,43,5,0,254,255
            };
        public static readonly byte[] ContentPutFinalNonEmptyEob = { 99, 88, 255, 1, 2, 43, 5, 0, 254, 255 };
        //
        public static readonly byte[] PduPutFinalEmptyEob
            = { OpcodePutFinal, 0,6,
                HeaderEob,  0,3,
            };
        //----
        public static readonly byte[] PduPutEobInNonFinalPdu
            = { OpcodePutNonFinal, 0,16,
                HeaderEob,  0,13, 1,2,3,4,0x99,6,7,8,9,0xFF
            };
        public static readonly byte[] PduPutBodyInFinalPdu
            = { OpcodePutFinal, 0,16,
                HeaderBody,  0,13, 1,2,3,4,0x99,6,7,8,9,0xFF
            };

        //----------------------------------------------------------------------
        //public static void Assert_Expect_Pdu(byte[] expected, Stream peer)
        //{
        //    byte[] buffer = new byte[expected.Length];
        //    int count = peer.Read(buffer, 0, expected.Length);
        //    if (count == 0) {
        //        Assert.Fail("Stream is closed.");
        //    }
        //    if (count != expected.Length) {
        //        Assert.Fail("Currently only can handle a stream where all the PDU arrives in one Read.");
        //    }
        //    Assert.AreEqual(expected, buffer);
        //}
    }//class--Tests_Values

}
