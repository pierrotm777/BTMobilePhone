using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using Brecham.Obex.Pdus;
using Brecham.Obex;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;



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
    public class Async2ObexServerHost : ObexServerHost
    {
        //--------------------------------------------------------------
        Socket m_socket;
        byte[] m_rcvBuf;
        int m_pduLength;
        //
        SocketAsyncEventArgs m_rcvEa, m_sndEa;
        //
        int m_completedSyncCount, m_completedSyncReadCount, m_completedSyncWriteCount;

        //--------------------------------------------------------------
        // Construction and initiation
        //--------------------------------------------------------------
        public Async2ObexServerHost(Socket peerSocket, Stream peer, int bufferSize)
            : base(peer, bufferSize)
        {
            m_socket = peerSocket;
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
            if (m_rcvEa == null) {
                m_rcvEa = new SocketAsyncEventArgs();
                m_rcvEa.Completed += ReadPeerCallback;
                m_rcvEa.SetBuffer(m_rcvBuf, 0, 0);
            }
            try {
#if ! NETCF
                s_trace.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                    "BeginRead m_pduLength {0}, count {1}", m_pduLength, count);
#endif // ! PocketPC
                m_rcvEa.SetBuffer(m_pduLength, count);
                m_rcvEa.UserToken = pduHandler; //state = the PDU handler fn
                CheckSocketReceiveAsyncArgs(m_rcvEa);
                bool pending = m_socket.ReceiveAsync(m_rcvEa);
                UpdateCompletedSyncReadCount(pending);
                if (!pending)
                    ReadPeerCallback(this, m_rcvEa);
            } catch (SocketException sex) {
                CloseConnectionLost(ServerExitReason.ConnectionErrorOnRead, sex);
            }
        }

        /// <summary>
        /// Have read the first or another chunk of the PDU
        /// </summary>
        /// <param name="ar">The standard <c>IAsyncResult</c> parameter.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void ReadPeerCallback(object sender, SocketAsyncEventArgs e)
        {
            try {
                int count;
                //--------------------------------------------------
                // Complete the Read
                //--------------------------------------------------
                if (e.SocketError == SocketError.Success) {
                    count = e.BytesTransferred;
                    Debug.Assert(e.BytesTransferred <= e.Count, e.BytesTransferred + " <= " + e.Count);
#if ! NETCF
                    s_trace.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                        "EndRead byte[0]: 0x{0:X2}, count: {1}", m_rcvBuf[0], count);
#endif // ! PocketPC
                } else {
                    SocketError errorCode = e.SocketError;
                    if (errorCode == SocketError.ConnectionReset) {
                        if (m_pduLength == 0) {
                            // Normal close appears as reset from some peer devices over some protocols,
                            // so allow for that behaviour.  One such device is PalmOS over IrDA.
                            CloseConnectionLost(ServerExitReason.ConnectionHardCloseOutwithAPdu, e);
                            return;
                        }
                    }
                    //else...
#if ! NETCF
                    s_trace.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0,
                        "Read peer failed with {0}.", e.SocketError);
#endif // ! PocketPC
                    CloseConnectionLost(ServerExitReason.ConnectionErrorOnRead, e);
                    return;
                }

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
                CompletePduHandler handler = (CompletePduHandler)e.UserToken;
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

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void InitiateWrite(ObexCreatedPdu rspPdu
            )
        {
            //AsyncCallback afterPduWrittenHandler = new AsyncCallback(WrittenPeerCallback);
            EventHandler<SocketAsyncEventArgs> afterPduWrittenHandler
                = new EventHandler<SocketAsyncEventArgs>(WrittenPeerCallback);
            try {
                if (m_sndEa == null) {
                    m_sndEa = new SocketAsyncEventArgs();
                    m_sndEa.Completed += afterPduWrittenHandler;
                }
                m_sndEa.SetBuffer(rspPdu.Buffer, 0, rspPdu.Length);
                CheckSocketSendAsyncArgs(m_sndEa);
                bool pending = m_socket.SendAsync(m_sndEa);
                UpdateCompletedSyncWriteCount(pending);
                if (!pending)
                    afterPduWrittenHandler(this, m_sndEa);
            } catch (Exception ex) {
                m_error = ex;
                CloseConnectionLost(ServerExitReason.ConnectionErrorOnWrite, ex);
            }
        }

        // The response PDU has been Written.
        // Start Reading the next 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void WrittenPeerCallback(object sender, SocketAsyncEventArgs e)
        {
            try {
                if (e.SocketError == SocketError.Success) {
                    Debug.Assert(e.BytesTransferred == e.Count, e.BytesTransferred + " == " + e.Count);
                    if (e.BytesTransferred != e.Count)
                        throw new InvalidOperationException("BytesTransferred: " + e.BytesTransferred + " == " + e.Count);
                } else {
#if ! NETCF
                    s_trace.TraceEvent(System.Diagnostics.TraceEventType.Error, 0,
                        "Write to peer failed with: " + e.SocketError);
#endif // ! PocketPC
                    CloseConnectionLost(ServerExitReason.ConnectionErrorOnWrite, e);
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
        protected void CloseConnectionLost(ServerExitReason localExitReason, SocketAsyncEventArgs e)
        {
            SocketException ex;
            if (e == null)
                ex = null;
            else
                ex = new SocketException((int)e.SocketError);
            CloseConnectionLost(localExitReason, ex);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        private void CheckSocketSendAsyncArgs(SocketAsyncEventArgs e)
        {
            CheckSocketAsyncEventArgs_(e);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        private void CheckSocketReceiveAsyncArgs(SocketAsyncEventArgs e)
        {
            CheckSocketAsyncEventArgs_(e);
        }

        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        private void CheckSocketAsyncEventArgs_(SocketAsyncEventArgs e)
        {
            if (e.Buffer == null)
                throw new ArgumentNullException("e.Buffer");
            System.Reflection.FieldInfo fldCompleted
                = typeof(SocketAsyncEventArgs).GetField("m_Completed",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            object completed = fldCompleted.GetValue(e);
            if (completed == null)
                throw new ArgumentNullException("e.Completed");
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
                Console.WriteLine("INFO: m_completedSyncCount is {0}", m_completedSyncCount);
#if ! NETCF
                s_trace.TraceInformation("INFO: m_completedSyncCount is {0}", m_completedSyncCount);
#endif
            }
        }

        private void UpdateCompletedSyncReadCount(bool pending)
        {
            if (!pending) {
                ++m_completedSyncCount;
                ++m_completedSyncReadCount;
            } else {
                m_completedSyncCount = m_completedSyncReadCount = m_completedSyncWriteCount = 0;
            }
            CheckCompletedSyncCount();
        }

        private void UpdateCompletedSyncWriteCount(bool pending)
        {
            if (!pending) {
                ++m_completedSyncCount;
                ++m_completedSyncWriteCount;
            } else {
                m_completedSyncCount = m_completedSyncReadCount = m_completedSyncWriteCount = 0;
            }
            CheckCompletedSyncCount();
        }

    }//class--AsyncObexServerHost

}
