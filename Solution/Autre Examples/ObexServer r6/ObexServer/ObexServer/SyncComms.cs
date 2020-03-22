using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using Brecham.Obex.Pdus;
using Brecham.Obex;
using System.Net.Sockets;
using System.Net;



namespace ObexServer
{

    /// <summary>
    /// A base implementation of an OBEX server.  This is an abstract class and supplies
    /// only the infrastructure for the server, implementation will need to override this
    /// class providing the actual PUT request handling etc.
    /// </summary>
    /// <remarks>
    /// This class supplies the PDU reading and writing infrastructure, and other 
    /// infrastructure services such as close and disposing etc.  The class handles one
    /// connection, and handling of multiple simulaneous connections will need a wrapper
    /// to accept each one connection and pass it to a new instance of this.
    /// </remarks>
    public class SyncObexServerHost : ObexServerHost
    {
        public SyncObexServerHost(Stream peer, int bufferSize)
            :base(peer, bufferSize)
        {
        }

        /// <summary>
        /// This <see cref="T:ObexServer.ObexServerHost"/> uses synchronous Socket 
        /// calls therefore <see cref="M:ObexServer.SyncObexServerHost.Start"/>
        /// blocks until the client exits or the session is otherwise closed.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>See <see cref="T:ObexServer.AsyncObexServerHost"/> for a host 
        /// that does not block in <c>Start</c> and is much more scalable.
        /// </para>
        /// </remarks>
        public override bool StartBlocks
        {
            get { return true; }
        }

        public override void Start()
        {
            Loop();
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void Loop()
        {
            //Console.WriteLine("SyncObexServerHost.Loop();");
            byte[] rcvBuf = PduFactory.GetReceiveBuffer();
            int pduLength;
            try {
                while (true) {
                    // Read PDU
                    pduLength = ReadPdu(rcvBuf);
                    if (pduLength == 0) { break; }

                    // Handle PDU
                    ObexCreatedPdu rspPdu = HandlePduAndResponse(rcvBuf, pduLength);
                    if (rspPdu == null) { break; }

                    // Write PDU
                    try {
                        m_peer.Write(rspPdu.Buffer, 0, rspPdu.Length);
                    } catch (IOException ioex) {
#if ! NETCF
                        s_trace.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                            "Write to peer failed with: " + ioex);
#endif // ! PocketPC
                        CloseConnectionLost(ServerExitReason.ConnectionErrorOnWrite, ioex);
                        return;
                    }

                    //
                    if (m_closed) {
#if ! NETCF
                        s_trace.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                            "m_closed = true");
#endif // ! PocketPC
                        return;
                    }
                    SetLastWriteTime();
                }//while
            } catch (Exception unhandledEx) {
                CloseUnhandledError(ServerExitReason.InternalServerError, unhandledEx);
            }
        }

        int ReadPdu(byte[] rcvBuf)
        {
            int extraLengthRequired;
            int pduLength_cur;
            int pduLength_final;
            int count;
            //
            extraLengthRequired = ObexPduFactory.MinimumPduLength;
            pduLength_cur = 0;
            //
            while (true) {
                try {
                    count = m_peer.Read(rcvBuf, pduLength_cur, extraLengthRequired);
                } catch (IOException ioex) {
                    Exception innerException = ioex.InnerException;
                    System.Net.Sockets.SocketException sex = innerException as System.Net.Sockets.SocketException;
                    if (sex != null) {
#if ! NETCF  // Could probably use the Int32 NativeErrorCode property.
                        SocketError errorCode = sex.SocketErrorCode;
                        if (errorCode == SocketError.ConnectionReset) {
                            if (pduLength_cur == 0) {
                                // Normal close appears as reset from some peer devices over some protocols,
                                // so allow for that behaviour.  One such device is PalmOS over IrDA.
                                CloseConnectionLost(ServerExitReason.ConnectionHardCloseOutwithAPdu, ioex);
                                return 0;
                            }
                        }
#endif // ! PocketPC
                    }
                    //else...
#if ! NETCF
                    s_trace.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0,
                        "Read peer failed with {0}.", ioex);
#endif // ! PocketPC
                    CloseConnectionLost(ServerExitReason.ConnectionErrorOnRead, ioex);
                    return 0;
                }//catch

                //--------------------------------------------------
                // Graceful close.
                //--------------------------------------------------
                if (count == 0) {
                    if (pduLength_cur != 0) {
                        // But are we in the middle of a PDU!
                        CloseConnectionLost(ServerExitReason.ConnectionGracefulCloseWithinAPdu, null);
                    } else {
                        CloseConnectionLost(ServerExitReason.ConnectionGracefulClose, null);
                    }
                    return 0;
                }

                //--------------------------------------------------
                // See if we've got the whole PDU, if so pass to the handler.  If not,
                // then read some more...
                //--------------------------------------------------
                //int extraLengthRequired;
                //int pduLength;
                pduLength_cur += count;
                PduFactory.ParseForLength(true, rcvBuf, pduLength_cur, out extraLengthRequired, out pduLength_final);
                //CompletePduHandler handler = (CompletePduHandler)ar.AsyncState;
                if (extraLengthRequired == 0) {
                    //--------------------------------------------------
                    // Complete PDU, handle it.
                    //--------------------------------------------------
                    SetLastReadTime();
#if ! NETCF
                    s_trace.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                        "Complete pdu byte[0] 0x{0:x}, length: {1}", rcvBuf[0], pduLength_final);
                    TraceByteArray(System.Diagnostics.TraceEventType.Verbose, 0, rcvBuf, pduLength_final);
#endif // ! PocketPC
                    return pduLength_final;
                } else {
                    // Fall through to
                    //--------------------------------------------------
                    // Initiate another async read for the rest of the PDU.
                    //--------------------------------------------------
                    //InitiateRead(handler, extraLengthRequired);
                }//if
            }
        }

    }//class

}
