using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using Brecham.Obex.Pdus;
using Brecham.Obex;
using System.Net; // eg ProtocolViolationException
using System.Diagnostics;
using System.Net.Sockets; // eg Debug


namespace ObexServer
{

    /// <summary>
    /// Handles the I/O component of the server, reading and writing PDUs, 
    /// the component handling the PDUs is <see cref="T:ObexServer.IObexServer"/>
    /// </summary>
    public abstract class ObexServerHost  // TODO ObexServerHost : IDisposable
    {
        //--------------------------------------------------------------
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        protected Stream m_peer;
        private EndPoint m_remoteEndPoint;
        LocalObexPduFactory m_pf;
        //
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        protected IObexServer m_pduHandler;
        //
        // The connection is now (to be) closed.  This is acted upon after the write
        // completes, and thus will not start a new read.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        protected bool m_closed;
        //
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        bool m_killing;
        //
        int m_LastReadTime = Environment.TickCount;
        int m_LastWroteTime = Environment.TickCount;
        //
        ServerExitReason m_exitReason;
        volatile bool m_complete;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        protected Exception m_error;
        System.Threading.ManualResetEvent m_completeEvent
            = new System.Threading.ManualResetEvent(false);

        //--------------------------------------------------------------
#if ! NETCF
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        protected static System.Diagnostics.TraceSource s_trace
            = new System.Diagnostics.TraceSource("Brecham.ObexServer.ServerHost"
#if DEBUG
                    , System.Diagnostics.SourceLevels.Information // .Verbose for packet dumps
#endif // DEBUG
                    );
#endif // ! PocketPC

        //--------------------------------------------------------------

        protected ObexServerHost(Stream peer, int bufferSize)
        {
            m_peer = peer;
            m_pf = new LocalObexPduFactory(bufferSize);
        }

        //--------------------------------------------------------------
        public void AddHandler(IObexServer pduHandler)
        {
            if (m_pduHandler != null) {
                throw new InvalidOperationException("IObexServer Handler is already set, only one is supported currently.");
            }
            m_pduHandler = pduHandler;
            m_pduHandler.Host = this;
        }

        //--------------------------------------------------------------

        public bool IsCompleted
        {
            get { return m_complete; }
        }

        private void SetComplete()
        {
            m_complete = true;
            m_completeEvent.Set();
            EventHandler handler = Exit;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        //--------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------

        /// <summary>
        /// Get the <see cref="T:System.Net.EndPoint"/> for the remote device
        /// -- if we are connected over the network.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>Currently returns <c>null</c> if the connection is not over a
        /// network, e.g is by serial cable, or in a test harness, should we throw
        /// an exception instead...?
        /// </para>
        /// </remarks>
        public EndPoint RemoteEndPoint
        {
            get { return m_remoteEndPoint; }
        }

        public void SetRemoteEndPoint(EndPoint rep)
        {
            m_remoteEndPoint = rep;
        }

        /// <summary>
        /// Gets the <see cref="T:Brecham.Obex.Pdus.ObexPduFactory"/> to use to create
        /// PDUs.
        /// </summary>
        public ObexPduFactory PduFactory {
            [System.Diagnostics.DebuggerStepThrough]
            get { return m_pf; }
        }

        /// <summary>
        /// Gets an event that signals that the server has exited.
        /// </summary>
        public System.Threading.WaitHandle ExitWaitHandle { get { return m_completeEvent; } }

        /// <summary>
        /// Occurs when the server host has exited.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>If running a server that supports more that one concurrent connection
        /// then this can be used to find when the connection is finished and the 
        /// server host can be removed from the list of active servers.
        /// </para>
        /// </remarks>
        public event EventHandler Exit;

        /// <summary>
        /// Gets the reason that the server exited.
        /// </summary>
        public ServerExitReason ExitReason { get { return m_exitReason; } }

        /// <summary>
        /// The time at which the last write of a PDU completed.
        /// </summary>
        /// <remarks>
        /// This could be used for instance to do purging of connections when they are
        /// found to be idle.  For instance a separate thread could periodically check
        /// every current connection, and close each one that was found to have done
        /// no work in two minutes (say).
        /// </remarks>
        /// <value>
        /// The value of <see cref="P:System.Environment.TickCount"/> at that point.
        /// </value>
        public int LastWriteTickCount { get { return m_LastWroteTime; } }

        protected void SetLastWriteTime()
        {
            m_LastWroteTime = Environment.TickCount;
        }

        /// <summary>
        /// The time at which the last read of a PDU completed.
        /// </summary>
        /// <value>
        /// The value of <see cref="P:System.Environment.TickCount"/> at that point.
        /// </value>
        public int LastReadTickCount { get { return m_LastReadTime; } }

        protected void SetLastReadTime()
        {
            m_LastReadTime = Environment.TickCount;
        }

        public void RethrowAnyError()
        {
            if (m_error != null) {
                throw m_error;
                //throw new Exception("Rethrown.", m_error);
            }
        }

        public void RethrowAnyErrorInTIEx()
        {
            if (m_error != null) {
                throw new System.Reflection.TargetInvocationException(m_error);
                //throw new Exception("Rethrown.", m_error);
            }
        }

        //--------------------------------------------------------------
        // 
        //--------------------------------------------------------------
        /// <summary>
        /// Start this <see cref="T:ObexServer.ObexServerHost"/> instance, can be 
        /// either synchronous or asynchronous.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>Note, that implementations of the server host can either block 
        /// till completion, or return once it has started.  Check the 
        /// <see cref="P:ObexServer.ObexServerHost.StartBlocks"/> property to see 
        /// which applies.
        /// </para>
        /// <para>If the network server wants to support multiple concurrent 
        /// connections and the server host is blocking, then the network server 
        /// should create a new thread for each connection and run the server host
        /// on that thread.
        /// </para>
        /// <para>In either case to know when the host has exited one can use property 
        /// <see cref="P:ObexServer.ObexServerHost.IsCompleted"/>
        /// or event <see cref="P:ObexServer.ObexServerHost.ExitWaitHandle"/>.
        /// </para>
        /// <para>Two existing server host implementations are
        /// <see cref="T:ObexServer.AsyncObexServerHost"/> and
        /// <see cref="T:ObexServer.SyncObexServerHost"/>.
        /// </para>
        /// </remarks>
        public abstract void Start();

        /// <summary>
        /// Get whether this implementation of the server host will block or run 
        /// in the asyncronously.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>See <see cref="M:ObexServer.ObexServerHost.Start"/> for more information.
        /// </para>
        /// </remarks>
        public abstract bool StartBlocks { get; }

        //--------------------------------------------------------------
        /// <summary>
        /// Kill the server host and server premptively.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>Closes the server quickly.  Does not do a graceful disconnect to 
        /// the OBEX client at the OBEX nor network layers.
        /// This method is used when the server has been idle for a lengthy period, 
        /// and when the network server is shutting down for instance.
        /// </para>
        /// </remarks>
        public void Kill()
        {
            m_killing = true;
#if ! NETCF
            // m_killing will be flushed at the end of the method, but the server 
            // could react to the connection close before that...
            System.Threading.Thread.MemoryBarrier();
#endif
            Debug.Assert(m_peer != null, "Expect this to be always set, for shutdown safety we check for null just in case.");
            if (m_peer != null)
                m_peer.Close();
        }

        //--------------------------------------------------------------
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected ObexCreatedPdu HandlePduAndResponse(byte[] receiveBuffer, int receivePduLength)
        {
            ObexParsedRequestPdu reqPdu;
            // Parse the received PDU
            try {
                reqPdu = (ObexParsedRequestPdu)PduFactory.Parse(Brecham.Obex.Pdus.ObexPduType.Request, receiveBuffer, receivePduLength);
            } catch (ProtocolViolationException pvex) {
                // Just close the connection.
                CloseClientFatalError(ServerExitReason.InvalidClientPdu, pvex);
                return null;
            }
            // Catch errors created only by the third-party overidden handler.
            ObexCreatedPdu rspPdu;
            try {
                rspPdu = HandlePdu(reqPdu);
                System.Diagnostics.Debug.Assert(rspPdu != null);
            } catch (Exception ex) {
                CloseUnhandledError(ServerExitReason.InternalServerErrorInHandler, ex);
                return null;
            }

            return rspPdu;
        }


        //--------------------------------------------------------------

        protected ObexCreatedPdu HandlePdu(ObexParsedRequestPdu requestPdu)
        {
            // TODO A _list_ of handlers to call...?
            ObexCreatedPdu rspPdu = m_pduHandler.HandlePdu(requestPdu);
            System.Diagnostics.Debug.Assert(rspPdu != null);
            return rspPdu;
        }

        //--------------------------------------------------------------
        // Closing
        //--------------------------------------------------------------
        protected void CloseUnhandledError(ServerExitReason localExitReason, Exception error)
        {
            if (m_killing) {
#if ! NETCF
                s_trace.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                    "(CloseUnhandledError at Kill: code: {0}, exception: {1})", localExitReason, error);
#endif
                Debug.Assert(error is ObjectDisposedException, "Expect error after Kill to be NetworkStream closed.");
                localExitReason = ServerExitReason.Disposed;
            } else {
#if ! NETCF
                s_trace.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                    "CloseUnhandledError: code: {0}, exception: {1}", localExitReason, error);
#endif
                m_error = error;
            }
            CloseCore(localExitReason);
        }

        protected void CloseConnectionLost(ServerExitReason localExitReason, Exception error)
        {
            if (m_killing) {
                // Mostly the ObjectDisposedException comes through the CloseUnhandledError
                // code path, but sometimes instead through e.g. CloseConnectionLost
                // as a IOException then wrapping the ObjectDisposedException 
#if ! NETCF
                s_trace.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                    "(CloseCore at Kill: code: {0})", localExitReason);
#endif
                ObjectDisposedException odex = error != null ? error.InnerException as ObjectDisposedException : null;
                SocketException sex = error != null ? error.InnerException as SocketException : null;
                const int SocketError_Interrupted = 10004; // (No SocketError on NETCF).
                Debug.Assert(error is ObjectDisposedException
                    || odex != null
                    || (sex != null && sex.ErrorCode == SocketError_Interrupted),
                    "Expect error after Kill to be NetworkStream closed.");
                localExitReason = ServerExitReason.Disposed;
            }
            //m_error = error;
            CloseCore(localExitReason);
        }

        protected void CloseClientFatalError(ServerExitReason localExitReason, Exception error)
        {
            //m_error = error;
            CloseCore(localExitReason);
        }

        protected void CloseClientFatalError(ServerExitReason localExitReason, String reason)
        {
            //TODO CloseClientFatalError do something with the description
            //m_error = error;
            CloseCore(localExitReason);
        }

        private void CloseCore(ServerExitReason localExitReason)
        {
            try {
                ServerExitReason localExitReasonOrig = localExitReason;
                if (m_killing && localExitReason != ServerExitReason.Disposed) {
                    // Mostly the ObjectDisposedException comes through the CloseUnhandledError
                    // code path, but sometimes instead through e.g. CloseConnectionLost
                    // (as a IOException then wrapping the ObjectDisposedException).
                    // So make the diagostic output correct then!
#if ! NETCF
                    s_trace.TraceEvent(System.Diagnostics.TraceEventType.Information, 0, 
                        "(CloseCore at Kill: code: {0})", localExitReason);
#endif
                    //Debug.Assert(error is ObjectDisposedException, "Expect error after Kill to be NetworkStream closed.");
                    localExitReason = ServerExitReason.Disposed;
                }
#if ! NETCF
                s_trace.TraceEvent(System.Diagnostics.TraceEventType.Information, 0,
                    "Close reason: {0}->{1}.", localExitReasonOrig, localExitReason);
#endif // ! PocketPC
                //
                m_exitReason = localExitReason;
                m_closed = true;
                System.Net.Sockets.NetworkStream ns = m_peer as System.Net.Sockets.NetworkStream;
                if (ns != null) {
#if ! NETCF
                    ns.Close(10 * 1000);
#else
                    m_peer.Close();
#endif // ! PocketPC
                } else {
                    m_peer.Close();
                }
            } finally {
                SetComplete();
            }
        }


        //--------------------------------------------------------------
        /// <summary>
        /// Create a response PDU with code BadRequest and the specified reason text.
        /// </summary>
        /// -
        /// <param name="reason">Descriptive text to include in a Description header.
        /// </param>
        /// -
        /// <returns>A <see cref="T:Brecham.Obex.Pdus.ObexCreatedPdu"/>.
        /// </returns>
        public ObexCreatedPdu CreateResponseWithDescription(String reason)
        {
            return CreateResponseWithDescription(ObexResponseCode.BadRequest, reason);
        }
        /// <summary>
        /// Create a response PDU with the specified response code and reason text.
        /// </summary>
        /// -
        /// <param name="code">The response code to use in the PDU.
        /// </param>
        /// <param name="reason">Descriptive text to include in a Description header.
        /// </param>
        /// -
        /// <returns>A <see cref="T:Brecham.Obex.Pdus.ObexCreatedPdu"/>.
        /// </returns>
        public ObexCreatedPdu CreateResponseWithDescription(ObexResponseCode code, String reason)
        {
            ObexHeaderCollection headers = new ObexHeaderCollection();
            headers.Add(ObexHeaderId.Description, reason);
            ObexCreatedPdu pdu;
            int debugHeaderCount = headers.Count;
            try {
                pdu = PduFactory.CreateResponse(code, headers, null);
            } catch (ObexCreateTooLongException) {
                // Only the one header, so from one, to zero.
                System.Diagnostics.Debug.Assert(headers.Count == debugHeaderCount - 1);
                headers.Remove(ObexHeaderId.Description);
                pdu = PduFactory.CreateResponse(code, headers, null);
            }
            return pdu;
        }

        public ObexClientOperationException CreateObexClientOperationException(String reason)
        {
            ObexCreatedPdu pdu = CreateResponseWithDescription(ObexResponseCode.BadRequest, reason);
            ObexClientOperationException ex = new ObexClientOperationException(pdu);
            return ex;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
           Justification = "More understandable as we only deal with request PDUs here.")]
        public void CheckRequestPduDoesNotContain(ObexHeaderId headerId, ObexParsedRequestPdu pdu)
        {
            if (pdu == null) {
                throw new ArgumentNullException("pdu");
            }
            if (pdu.Headers.Contains(headerId)) {
                throw CreateObexClientOperationException("Request PDU shouldn't contain " + headerId.ToString() + " header.");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
           Justification = "More understandable as we only deal with request PDUs here.")]
        public void CheckRequestPduContains(ObexHeaderId headerId, ObexParsedRequestPdu pdu)
        {
            if (pdu == null) {
                throw new ArgumentNullException("pdu");
            }
            if (!pdu.Headers.Contains(headerId)) {
                throw CreateObexClientOperationException("Request PDU should contain " + headerId.ToString() + " header.");
            }
        }

        //--------------------------------------------------------------
        //#region IDisposable Members
        //
        //TODO public void Dispose()
        //{
        //    Dispose(true);
        //}
        //
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposing) {
        //        m_completeEvent.Close();
        //        const bool LeaveInnerStreamOpen = false;
        //        if (!LeaveInnerStreamOpen) {
        //            m_peer.Dispose();
        //        }
        //    }
        //}
        //
        //#endregion

        //--------------------------------------------------------------
#if ! NETCF
        protected static void TraceByteArray(System.Diagnostics.TraceEventType eventType, int id, byte[] buffer, int length)
        {
            const int BytesPerLine = 16;
            const int TruncateLength = 64;
            int offset = 0;
            while (true) {
                // Number of bytes remaining.
                int curCount = length - offset;
                // How many allowed on a line, at this point.
                int allowedCount = Math.Min(BytesPerLine, TruncateLength - offset);
                // Now get at most a lines worth.
                curCount = Math.Min(curCount, allowedCount);
                System.Diagnostics.Debug.Assert(curCount >= 0);
                if (curCount <= 0) {
                    break;
                }
                String line = BitConverter.ToString(buffer, offset, curCount);
                s_trace.TraceData(eventType, id, line);
                offset += curCount;
            }//for
        }
#endif // ! PocketPC

    }//class--ObexServerHostBase

}
