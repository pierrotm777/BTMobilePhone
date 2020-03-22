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
    public class AsyncObexServerHost : ObexServerHost
    {
        //--------------------------------------------------------------
        byte[] m_rcvBuf;
        int m_pduLength;
        //
        int m_completedSyncCount, m_completedSyncReadCount, m_completedSyncWriteCount;

        //--------------------------------------------------------------
        // Construction and initiation
        //--------------------------------------------------------------
        public AsyncObexServerHost(Stream peer, int bufferSize)
            : base(peer, bufferSize)
        {
            m_rcvBuf = PduFactory.GetReceiveBuffer();
        }

        public override void Start()
        {
            ReadNewPduAsync();
        }

        /// <summary>
        /// This <see cref="T:ObexServer.ObexServerHost"/> uses asynchronous Socket 
        /// calls and therefore <see cref="M:ObexServer.SyncObexServerHost.Start"/>
        /// return once the first asynchronous call is initiated.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>To know when the host has exited use property 
        /// <see cref="P:ObexServer.ObexServerHost.IsCompleted"/>
        ///  or event <see cref="P:ObexServer.ObexServerHost.ExitWaitHandle"/>.
        /// </para>
        /// <para>The other host, <see cref="T:ObexServer.SyncObexServerHost"/>, uses
        /// synchronous Socket calls.
        /// </para>
        /// </remarks>
        public override bool StartBlocks
        {
            get { return false; }
        }


        //--------------------------------------------------------------
        /// <summary>
        /// Who to call when a complete PDU has been received.
        /// </summary>
        /// <param name="pduBuffer">
        /// A <see cref="T:System.Byte"/> array containing the received PDU at offset zero.
        /// </param>
        /// <param name="length">
        /// The length of the PDU.
        /// </param>
        private delegate void CompletePduHandler(byte[] pduBuffer, int length);

        /// <summary>
        /// Start reading the next PDU asynchronously.
        /// </summary>
        void ReadNewPduAsync()
        {
            //if (m_manuallyAddLatency) {
            //    System.Threading.Thread.Sleep(3000);
            //}
            CompletePduHandler pduHandler = new CompletePduHandler(HandlePduAndResponse);
            m_pduLength = 0;
            //This (currently?) returns a new buffer each time so don't call it each time!
            //m_rcvBuf = PduFactory.GetReceiveBuffer();
            //
            InitiateRead(pduHandler, ObexPduFactory.MinimumPduLength);
        }

        /// <summary>
        /// Set the asynchronous reading of the next (or first) chunk of the PDU.
        /// </summary>
        /// <param name="pduHandler">The method which handle complete PDUs.</param>
        /// <param name="count">How many more bytes the PDU contains.</param>
        private void InitiateRead(CompletePduHandler pduHandler, int count)
        {
            AsyncCallback readCb = new AsyncCallback(ReadPeerCallback);
            try {
#if ! NETCF
                s_trace.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                    "BeginRead m_pduLength {0}, count {1}", m_pduLength, count);
#endif // ! PocketPC
                m_peer.BeginRead(m_rcvBuf, m_pduLength, count, readCb,
                    /* state = the PDU handler fn */ pduHandler);
            } catch (IOException ioex) {
                CloseConnectionLost(ServerExitReason.ConnectionErrorOnRead, ioex);
            }
        }

        /// <summary>
        /// Have read the first or another chunk of the PDU
        /// </summary>
        /// <param name="ar">The standard <c>IAsyncResult</c> parameter.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void ReadPeerCallback(IAsyncResult ar)
        {
            try {
                int count;
                try {
                    //--------------------------------------------------
                    // Complete the Read
                    //--------------------------------------------------
                    count = m_peer.EndRead(ar);
                    UpdateCompletedSyncReadCount(ar);
#if ! NETCF
                    s_trace.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                        "EndRead byte[0]: 0x{0:X2}, count: {1}", m_rcvBuf[0], count);
#endif // ! PocketPC
                } catch (IOException ioex) {
                    Exception innerException = ioex.InnerException;
                    System.Net.Sockets.SocketException sex = innerException as System.Net.Sockets.SocketException;
                    if (sex != null) {
#if ! NETCF  // Could probably use the Int32 NativeErrorCode property.
                        SocketError errorCode = sex.SocketErrorCode;
                        if (errorCode == SocketError.ConnectionReset) {
                            if (m_pduLength == 0) {
                                // Normal close appears as reset from some peer devices over some protocols,
                                // so allow for that behaviour.  One such device is PalmOS over IrDA.
                                CloseConnectionLost(ServerExitReason.ConnectionHardCloseOutwithAPdu, ioex);
                                return;
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
                    return;
                }//catch

                //--------------------------------------------------
                // Graceful close.
                //--------------------------------------------------
                if (count == 0) {
                    if (m_pduLength != 0) {
                        // But are we in the middle of a PDU!
                        CloseConnectionLost(ServerExitReason.ConnectionGracefulCloseWithinAPdu, null);
                    } else {
                        CloseConnectionLost(ServerExitReason.ConnectionGracefulClose, null);
                    }
                    return;
                }

                //--------------------------------------------------
                // See if we've got the whole PDU, if so pass to the handler.  If not,
                // then read some more...
                //--------------------------------------------------
                int extraLengthRequired;
                int pduLength;
                m_pduLength += count;
                PduFactory.ParseForLength(true, m_rcvBuf, m_pduLength, out extraLengthRequired, out pduLength);
                CompletePduHandler handler = (CompletePduHandler)ar.AsyncState;
                if (extraLengthRequired == 0) {
                    //--------------------------------------------------
                    // Complete PDU, handle it.
                    //--------------------------------------------------
                    SetLastReadTime();
#if ! NETCF
                    s_trace.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                        "Complete pdu byte[0]: 0x{0:X2}, length: {1}", m_rcvBuf[0], m_pduLength);
                    TraceByteArray(System.Diagnostics.TraceEventType.Verbose, 0, m_rcvBuf, m_pduLength);
#endif // ! PocketPC
                    handler(m_rcvBuf, pduLength);
                    return;
                } else {
                    //--------------------------------------------------
                    // Initiate another async read for the rest of the PDU.
                    //--------------------------------------------------
                    InitiateRead(handler, extraLengthRequired);
                }//if
            } catch (Exception unhandledEx) {
                CloseUnhandledError(ServerExitReason.InternalServerError, unhandledEx);
            }
        }

        private void InitiateWrite(ObexCreatedPdu rspPdu
            )
        {
            AsyncCallback afterPduWrittenHandler = new AsyncCallback(WrittenPeerCallback);
            try {
                m_peer.BeginWrite(rspPdu.Buffer, 0, rspPdu.Length, afterPduWrittenHandler, null);
            } catch (IOException ioex) {
                m_error = ioex;
                CloseConnectionLost(ServerExitReason.ConnectionErrorOnWrite, ioex);
            }
        }


        // The response PDU has been Written.
        // Start Reading the next 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void WrittenPeerCallback(IAsyncResult ar)
        {
            try {
                try {
                    m_peer.EndWrite(ar);
                    UpdateCompletedSyncWriteCount(ar);
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
                //
                ReadNewPduAsync();
            } catch (Exception unhandledEx) {
                CloseUnhandledError(ServerExitReason.InternalServerError, unhandledEx);
            }
        }

        /// <summary>
        /// Have received a complete PDU.  So parse it, and then hand it off the the 
        /// third-party method which knows how to deal with PDUs.  It returns a PDU to be
        /// sent in response, so then initiate the writing of it asynchronously.
        /// </summary>
        /// <param name="receiveBuffer">
        /// A <see cref="System.Byte"/> array containing the received PDU at offset zero.
        /// </param>
        /// <param name="receivePduLength">
        /// The length of the PDU.
        /// </param>
        protected new void HandlePduAndResponse(byte[] receiveBuffer, int receivePduLength)
        {
            ObexCreatedPdu rspPdu = base.HandlePduAndResponse(receiveBuffer, receivePduLength);
            if (rspPdu == null) {
                return;
            }

            // Send the response
            InitiateWrite(rspPdu);
        }

        //--------------------------------------------------------------
        /// <summary>
        /// Check whether lots of asynchronous Socket calls are completed synchronously 
        /// (immediately on same thread).
        /// </summary>
        /// -
        /// <remarks>
        /// <para>As seen on newsgroup postings people sometimes see their asynchronous-sockets 
        /// applications deadlocking because they expect BeginRead/BeginWrite to always 
        /// complete on a thread-pool thread, but as signalled by boolean property 
        /// IAsyncResult.CompletedSynchronously it can complete syncronously within 
        /// the initiating BeginRead/-Write call.
        /// </para>
        /// <para>So we add this logging mostly out of curiosity to see if that 
        /// ever occurs for us.  We shouldn't have any problem in that case as far 
        /// as I know, as we don't lock or similar, but it would be interesting to 
        /// see that scenario to occur.  I haven't manages to see it occur at all 
        /// in testing.
        /// </para>
        /// </remarks>
        private void CheckCompletedSyncCount()
        {
            System.Diagnostics.Debug.Assert(m_completedSyncCount >= m_completedSyncReadCount, "cSC >= cSReadC");
            System.Diagnostics.Debug.Assert(m_completedSyncCount >= m_completedSyncWriteCount, "cSC >= cSWriteC");
            System.Diagnostics.Debug.Assert(m_completedSyncCount == m_completedSyncReadCount + m_completedSyncWriteCount, "cSC == cSReadC + cSWriteC");
            //
            //Console.WriteLine("DIAGNOSTIC: m_completedSyncCount is {0}", m_completedSyncCount);
            const int CompletedSyncCountLimit = 10; // For pre-implementation
            if (m_completedSyncCount > CompletedSyncCountLimit) {
                Console.WriteLine("WARNING: m_completedSyncCount is {0}", m_completedSyncCount);
#if ! NETCF
                s_trace.TraceInformation("WARNING: m_completedSyncCount is {0}", m_completedSyncCount);
#endif
            }
        }

        private void UpdateCompletedSyncReadCount(IAsyncResult ar)
        {
            if (ar.CompletedSynchronously) {
                ++m_completedSyncCount;
                ++m_completedSyncReadCount;
            } else {
                m_completedSyncCount = m_completedSyncReadCount = m_completedSyncWriteCount = 0;
            }
            CheckCompletedSyncCount();
        }

        private void UpdateCompletedSyncWriteCount(IAsyncResult ar)
        {
            if (ar.CompletedSynchronously) {
                ++m_completedSyncCount;
                ++m_completedSyncWriteCount;
            } else {
                m_completedSyncCount = m_completedSyncReadCount = m_completedSyncWriteCount = 0;
            }
            CheckCompletedSyncCount();
        }


    }//class--AsyncObexServerHost

}
