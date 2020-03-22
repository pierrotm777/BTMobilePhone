#define GET_RESPONSE_INCLUDES_LENGTH
using System;

namespace ObexServer.Tests
{

    class Tests_Values_Get
    {
        static byte LengthOfLengthHeader
#if GET_RESPONSE_INCLUDES_LENGTH
            = 5;
#else
            = 0;
#endif

        // Miscellaneous
        public static readonly byte[] PduReqGetFinalNoHeaders = {
                Tests_Values.OpcodeGetFinal, 0,3,
            };
        public static readonly byte[] PduRspContinueNoHeaders = {
            Tests_Values.RespcodeContinue, 0, 3
            };
        //
        // One
        public static readonly byte[] PduReqOne = {
                Tests_Values.OpcodeGetFinal, 0,24,
                Tests_Values.HeaderName, 0,21, 0,0x64, 0,0x64, 0,0x64, 0,0x64, 0,0x2e, 0,0x74, 0,0x78, 0,0x74, 0,0,
            };
        public static readonly byte[] PduRspOne1 = {
            Tests_Values.RespcodeContinue, 0, (byte)(3+3+11 + LengthOfLengthHeader),
#if GET_RESPONSE_INCLUDES_LENGTH
                Tests_Values.HeaderLength, 0,0,0,11,
#endif
                Tests_Values.HeaderBody, 0, 3+11, //"Hello world"
                    (byte)'H', (byte)'e', (byte)'l', (byte)'l', (byte)'o', 
                    (byte)' ', (byte)'w', (byte)'o', (byte)'r', (byte)'l', 
                    (byte)'d', 
            };
        public static readonly byte[] PduRspOne2End = {
            Tests_Values.RespcodeSuccess, 0, 3+3,
                Tests_Values.HeaderEob, 0, 3,
            };
        //
        // One in smaller Chunks
        public static readonly byte[] PduRspOneSmallerChunks1 = {
            Tests_Values.RespcodeContinue, 0, (byte)(3+3+4 + LengthOfLengthHeader),
#if GET_RESPONSE_INCLUDES_LENGTH
                Tests_Values.HeaderLength, 0,0,0,11,
#endif
                Tests_Values.HeaderBody, 0, 3+4, //"Hell|o wo|rld"
                    (byte)'H', (byte)'e', (byte)'l', (byte)'l',
            };
        public static readonly byte[] PduRspOneSmallerChunks2 = {
            Tests_Values.RespcodeContinue, 0, 3+3+4,
                Tests_Values.HeaderBody, 0, 3+4, //"Hell|o wo|rld"
                    (byte)'o', (byte)' ', (byte)'w', (byte)'o',
            };
        public static readonly byte[] PduRspOneSmallerChunks3 = {
            Tests_Values.RespcodeContinue, 0, 3+3+3,
                Tests_Values.HeaderBody, 0, 3+3, //"Hell|o wo|rld"
                    (byte)'r', (byte)'l', (byte)'d', 
            };
        public static readonly byte[] PduRspOneSmallerChunks4End = {
            Tests_Values.RespcodeSuccess, 0, 3+3,
                Tests_Values.HeaderEob, 0, 3,
            };
        //
        // One in smaller Chunks, local io error on second
        public static readonly byte[] PduRspOneSmallerChunksExceptionOnSecond1 = {
            Tests_Values.RespcodeContinue, 0, 3+3+4,
                Tests_Values.HeaderBody, 0, 3+4, //"Hell|o wo|rld"
                    (byte)'H', (byte)'e', (byte)'l', (byte)'l',
            };
        public static readonly byte[] PduRspOneSmallerChunksExceptionOnSecond2Error = {
            Tests_Values.RespcodeInternalServerError, 0, 3+0x81,
                Tests_Values.HeaderDescription, 0, 0x81, //0x81-3 = 0x7E
            // "Read from content store failed with: Testing break connection."
            0, (byte)'R', 0, (byte)'e', 0, (byte)'a', 0, (byte)'d', 0, (byte)' ', 0, (byte)'f', 0, (byte)'r', 0, (byte)'o', 
            0, (byte)'m', 0, (byte)' ', 0, (byte)'c', 0, (byte)'o', 0, (byte)'n', 0, (byte)'t', 0, (byte)'e', 0, (byte)'n', 
            0, (byte)'t', 0, (byte)' ', 0, (byte)'s', 0, (byte)'t', 0, (byte)'o', 0, (byte)'r', 0, (byte)'e', 0, (byte)' ', 
            0, (byte)'f', 0, (byte)'a', 0, (byte)'i', 0, (byte)'l', 0, (byte)'e', 0, (byte)'d', 0, (byte)' ', 0, (byte)'w', 
            0, (byte)'i', 0, (byte)'t', 0, (byte)'h', 0, (byte)':', 0, (byte)' ', 0, (byte)'T', 0, (byte)'e', 0, (byte)'s', 
            0, (byte)'t', 0, (byte)'i', 0, (byte)'n', 0, (byte)'g', 0, (byte)' ', 0, (byte)'b', 0, (byte)'r', 0, (byte)'e', 
            0, (byte)'a', 0, (byte)'k', 0, (byte)' ', 0, (byte)'c', 0, (byte)'o', 0, (byte)'n', 0, (byte)'n', 0, (byte)'e', 
            0, (byte)'c', 0, (byte)'t', 0, (byte)'i', 0, (byte)'o', 0, (byte)'n', 0, (byte)'.', 0,        0,
            };
        //
        // One with two request packets
        public static readonly byte[] PduReqOneWithTwoRequestPackets1 = {
                Tests_Values.OpcodeGetNonFinal, 0,24,
                Tests_Values.HeaderName, 0,21, 0,0x64, 0,0x64, 0,0x64, 0,0x64, 0,0x2e, 0,0x74, 0,0x78, 0,0x74, 0,0,
            };
        public static readonly byte[] PduReqOneWithTwoRequestPackets2 = {
                Tests_Values.OpcodeGetFinal, 0,3,
            };
        //
        //PduRspOneButGetNonFinalAfterFinalError
        public static readonly byte[] PduReqOneButGetNonFinalAfterFinalError1 = {
                Tests_Values.OpcodeGetFinal, 0,24,
                Tests_Values.HeaderName, 0,21, 0,0x64, 0,0x64, 0,0x64, 0,0x64, 0,0x2e, 0,0x74, 0,0x78, 0,0x74, 0,0,
            };
        public static readonly byte[] PduReqOneButGetNonFinalAfterFinalError2 = {
                Tests_Values.OpcodeGetNonFinal, 0,3,
            };
        public static readonly byte[] PduRspOneButGetNonFinalAfterFinalError = {
            Tests_Values.RespcodeBadRequest, 0, 3+3+0x72,
                Tests_Values.HeaderDescription, 0, 3+0x72,
                // "After a GET Final Request every request should be Final."
                0, (byte)'A', 0, (byte)'f', 0, (byte)'t', 0, (byte)'e', 0, (byte)'r', 0, (byte)' ', 0, (byte)'a', 0, (byte)' ',
                0, (byte)'G', 0, (byte)'E', 0, (byte)'T', 0, (byte)' ', 0, (byte)'F', 0, (byte)'i', 0, (byte)'n', 0, (byte)'a', 
                0, (byte)'l', 0, (byte)' ', 0, (byte)'R', 0, (byte)'e', 0, (byte)'q', 0, (byte)'u', 0, (byte)'e', 0, (byte)'s', 
                0, (byte)'t', 0, (byte)' ', 0, (byte)'e', 0, (byte)'v', 0, (byte)'e', 0, (byte)'r', 0, (byte)'y', 0, (byte)' ', 
                0, (byte)'r', 0, (byte)'e', 0, (byte)'q', 0, (byte)'u', 0, (byte)'e', 0, (byte)'s', 0, (byte)'t', 0, (byte)' ', 
                0, (byte)'s', 0, (byte)'h', 0, (byte)'o', 0, (byte)'u', 0, (byte)'l', 0, (byte)'d', 0, (byte)' ', 0, (byte)'b', 
                0, (byte)'e', 0, (byte)' ', 0, (byte)'F', 0, (byte)'i', 0, (byte)'n', 0, (byte)'a', 0, (byte)'l', 0, (byte)'.',
                0, 0,
            };


    }//class

}