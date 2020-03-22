// %%% #define LOCAL_AUTHENTICATION
using System;
//
using System.IO;
//
using Brecham.Obex.Pdus;
using Brecham.Obex;


namespace ObexServer
{

    /// <summary>
    /// An implementation of an OBEX server that handles the PUT operation.  The 
    /// implementer needs to supply an event handler to open a <see cref="T:System.IO.Stream "/>
    /// when a PUT operation is received.
    /// </summary>
    public class ObexInboxServer : ObexServerBase
    {
        //----------------------------------------------------------
        bool m_doneConnect;
        protected DirectoryInfo m_folder;
        //
        /// <summary>
        /// The <c>Stream</c> to which the PUT content is being written.
        /// </summary>
        Stream m_putDest;

        /// <summary>
        /// The aggregate set of metadata headers received in the current PUT operation.
        /// </summary>
        ObexHeaderCollection m_putClientHdrs;

        bool DumpAllReceivePdus = false;

#if LOCAL_AUTHENTICATION
        AuthenticationState m_authState;
        byte[] m_lastChallengeNonceSent;
        string m_passphrase = null;
#endif

        //----------------------------------------------------------
        public ObexInboxServer()
        {
#if LOCAL_AUTHENTICATION
            m_authState = AuthenticationState.NotRequired;
#endif
        }

        //--------------------------------------------------------------
        public void SetFolder(DirectoryInfo initialFolder)
        {
            m_folder = initialFolder;
        }

        public void SetLocalPassphrase(string passphrase)
        {
            if (passphrase == null)
                throw new ArgumentNullException("passphrase", "Authentication is off by default, set a non-null passphrase to enable it.");
#if LOCAL_AUTHENTICATION
            // Verify that the passphrase is pure ASCII!
            byte[] tmp = ObexHeaderConverter.StringAsAsciiByteSeq(passphrase, false);
            m_authState = AuthenticationState.Required;
            m_passphrase = passphrase;
#else
            throw new NotSupportedException("Authentication isn't supported in this configuration.");
#endif
        }

        //--------------------------------------------------------------
        public override ObexCreatedPdu HandlePdu(ObexParsedRequestPdu requestPdu)
        {
            if (requestPdu == null) {
                throw new ArgumentNullException("requestPdu");
            }
            //
            if (DumpAllReceivePdus) {
                Console.Write("Received PDU: {0}=0x{0:x}, ", requestPdu.Opcode);
                requestPdu.Headers.Dump(Console.Out);
            }
            //
#if LOCAL_AUTHENTICATION
            ObexCreatedPdu authRequiredPdu = DoLocalAuth(requestPdu);
            // If an authentication PDU is to be sent rather than SUCCESS etc as normal.
            if (authRequiredPdu != null)
                return authRequiredPdu;
#endif
            //
            // TODO Handle the case where a different request pdu occurs in the middle of a PUT operation. 
            ObexCreatedPdu rspPdu;
            if (requestPdu.Opcode == ObexOpcode.Connect) {
                rspPdu = HandleConnectPdu(requestPdu);
            } else if (requestPdu.Opcode == ObexOpcode.Disconnect) {
                rspPdu = HandleDisconnectPdu(requestPdu);
            } else if (requestPdu.Opcode == ObexOpcode.Put
                    || requestPdu.Opcode == ObexOpcode.PutNonFinal) {
                rspPdu = HandlePutPdu(requestPdu);
            } else if (requestPdu.Opcode == ObexOpcode.Get
                    || requestPdu.Opcode == ObexOpcode.GetNonFinal) {
                rspPdu = HandleGetPdu(requestPdu);
            } else if (requestPdu.Opcode == ObexOpcode.SetPath) {
                rspPdu = HandleSetPathPdu(requestPdu);
            } else if (requestPdu.Opcode == ObexOpcode.Session) {
                rspPdu = HandleSessionPdu(requestPdu);
            } else if (requestPdu.Opcode == ObexOpcode.Abort) {
                rspPdu = HandleAbortPdu(requestPdu);
            } else {
                //TODO What error code to return when garbage opcode?
                rspPdu = PduFactory.CreateResponse(ObexResponseCode.NotImplemented, null, null);
            }
            System.Diagnostics.Debug.Assert(rspPdu != null);
            return rspPdu;
        }

        //--------------------------------------------------------------
        protected virtual ObexCreatedPdu HandleGetPdu(ObexParsedRequestPdu requestPdu)
        {
            return PduFactory.CreateResponse(ObexResponseCode.NotImplemented, null, null);
        }

        protected virtual ObexCreatedPdu HandleSetPathPdu(ObexParsedRequestPdu requestPdu)
        {
            return PduFactory.CreateResponse(ObexResponseCode.NotImplemented, null, null);
        }

        protected virtual ObexCreatedPdu HandleSessionPdu(ObexParsedRequestPdu requestPdu)
        {
            return PduFactory.CreateResponse(ObexResponseCode.NotImplemented, null, null);
        }


        //--------------------------------------------------------------
        private ObexCreatedPdu HandleDisconnectPdu(ObexParsedRequestPdu requestPdu)
        {
            ObexCreatedPdu rspPdu = PduFactory.CreateResponse(ObexResponseCode.Success, null, null);
            ObexOpcode code = requestPdu.Opcode;
            System.Diagnostics.Debug.Assert(code == ObexOpcode.Disconnect);
            //TODO Signal the connection to close after the response has been written?
            return rspPdu;
        }


        protected virtual ObexCreatedPdu HandleConnectPdu(ObexParsedRequestPdu requestPdu)
        {
            if (requestPdu == null) {
                throw new ArgumentNullException("requestPdu");
            }
            ObexOpcode code = requestPdu.Opcode;
            System.Diagnostics.Debug.Assert(code == ObexOpcode.Connect);
            if (m_doneConnect) {
                throw new ObexClientFatalException("Connect PDU sent when already connected.");
            }
            // Use the connect bytes, to set the MRU etc.
            byte[] xtra = requestPdu.Headers.GetByteSeq(ObexHeaderId.ProxyPreHeadersBytes);
            ((LocalObexPduFactory)PduFactory).ServerUseConnectBytes(xtra);//HACK cast LocalObexPduFactory
            m_doneConnect = true;
            //
            DisplayConnectionInfo(PduFactory);
            // We only support the default Inbox service, so just return a PDU with
            // no headers (no ConnectionId, no Who header).
            ObexCreatedPdu rspPdu = PduFactory.CreateResponse(true, ObexResponseCode.Success, null, null);
            return rspPdu;
        }//fn

        private static void DisplayConnectionInfo(ObexPduFactory pduFcty)
        {
            Console.WriteLine("Local MRU: {0}, MTU: {1}, Peer MRU: {2}",
                pduFcty.LocalMru, pduFcty.Mtu, pduFcty.PeerMru);
        }//fn--cmdReader

        //--------------------------------------------------------------

        public event EventHandler<CreatePutStreamEventArgs> CreatePutStream;

        private Stream DoCreatePutStream(ObexHeaderCollection putMetadata, ref ObexCreatedPdu errorResponsePdu)
        {
            Console.WriteLine("PUT creation for peer: '{0}'", Host.RemoteEndPoint);
            CreatePutStreamEventArgs ea = new CreatePutStreamEventArgs(putMetadata, m_folder);
            OnCreatePutStream(ea);
            if (ea.PutStream == null) {
                // TODO OnCreatePutStream--Do we want to accept a code and description instead of a PDU?
                errorResponsePdu = ea.ErrorResponsePdu;
                return null;
            } else {
                if (!ea.PutStream.CanWrite) {
                    throw new ArgumentException("PutStream is not writeable.");
                }
                return ea.PutStream;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ObexServer.ObexInboxServer.CreatePutStream"/> event.
        /// </summary>
        /// <param name="ea">
        /// A <see cref="T:ObexServer.CreatePutStreamEventArgs"/> instance that contains the event data.
        /// </param>
        protected virtual void OnCreatePutStream(CreatePutStreamEventArgs ea)
        {
            EventHandler<CreatePutStreamEventArgs> handler = CreatePutStream;
            if (handler != null) {
                Object sender = this;
                handler(sender, ea);
                // Called; now use the results.
            } else {
                // No handler!
                throw new ObexClientFatalException("No PUT handler.");
            }
        }


        //--------------------------------------------------------------
        private void PutReset()
        {
            // Also want to cancel the delete the file if we're using a FileStream?
            // Probably the overrider's job anyway...
            if (m_putDest != null) {
                m_putDest.Close();
            }
            m_putDest = null;
            m_putClientHdrs = null;
        }

        /// <summary>
        /// This is the meat of the PUT server
        /// </summary>
        /// <param name="requestPdu">The received PDU.</param>
        /// <returns>The PDU to send in response.</returns>
        protected virtual ObexCreatedPdu HandlePutPdu(ObexParsedRequestPdu requestPdu)
        {
            ObexCreatedPdu rspPdu;
            bool close = false;
            byte[] body;
            bool success = false; //For finally-based exception handler clean-up code.
            //
            try {
                //------------------------------------------------------
                // Gather Metadata
                //------------------------------------------------------
                if (m_putClientHdrs == null) {
                    // The first PDU in this operation create a new collection.
                    m_putClientHdrs = new ObexHeaderCollection();
                    // Add two dummy headers to ensure that the Body headers are never added.
                    m_putClientHdrs.Add(ObexHeaderId.Body, new byte[0]);
                    m_putClientHdrs.Add(ObexHeaderId.EndOfBody, new byte[0]);
                }
                // Combine with any previously received headers.
                m_putClientHdrs.AppendSkippingAnyDuplicates(requestPdu.Headers);

                //------------------------------------------------------
                // Check PDU
                //------------------------------------------------------
                if (requestPdu.Opcode == ObexOpcode.Put) {
                    // Final PDU.  Must EoB, must not Body.
                    // (OBEX13.pdf section 3.3.3.6 suggests using a Put PDU with no body
                    // header as a delete operation; we don't support that here...)
                    Host.CheckRequestPduContains(ObexHeaderId.EndOfBody, requestPdu);
                    Host.CheckRequestPduDoesNotContain(ObexHeaderId.Body, requestPdu);
                    body = requestPdu.Headers.GetByteSeq(ObexHeaderId.EndOfBody);
                    requestPdu.Headers.Remove(ObexHeaderId.EndOfBody);
                    rspPdu = PduFactory.CreateResponse(ObexResponseCode.Success, null, null);
                    close = true;
                } else { //if (requestPdu.Opcode == ObexOpcode.PutNonFinal) {
                    System.Diagnostics.Debug.Assert(requestPdu.Opcode == ObexOpcode.PutNonFinal, "Can only handle PUT PDUs.");
                    // Non-final PDU.  Must not EoB, may Body.
                    Host.CheckRequestPduDoesNotContain(ObexHeaderId.EndOfBody, requestPdu);
                    if (requestPdu.Headers.Contains(ObexHeaderId.Body)) {
                        body = requestPdu.Headers.GetByteSeq(ObexHeaderId.Body);
                        requestPdu.Headers.Remove(ObexHeaderId.Body);
                    } else {
                        body = null;
                    }
                    rspPdu = PduFactory.CreateResponse(ObexResponseCode.Continue, null, null);
                }

                //------------------------------------------------------
                // Open (if not already), as we've received a body header.
                //------------------------------------------------------
                if (m_putDest == null && body != null) {
                    // Remove the dummy body headers that we added above.
                    m_putClientHdrs.Remove(ObexHeaderId.Body);
                    m_putClientHdrs.Remove(ObexHeaderId.EndOfBody);
                    // Ask the local consumer where to store this PUT content.
                    Stream openedStream = DoCreatePutStream(m_putClientHdrs, ref rspPdu);
                    if (openedStream != null) {
                        // Successful open, it returned the stream
                        m_putDest = openedStream;
                    } else {
                        // Error, he didn't know what to with/didn't like this Request.
                        // If it didn't return a response pdu create a generic one.
                        if (rspPdu == null) {
                            rspPdu = Host.CreateResponseWithDescription(ObexResponseCode.BadRequest,
                                "Bad Request, could not open local destination.");
                        }
                        throw new ObexClientOperationException(rspPdu);
                    }//openedStream==null
                    // Re-add the dummy body headers that we removed just above.
                    m_putClientHdrs.Add(ObexHeaderId.Body, new byte[0]);
                    m_putClientHdrs.Add(ObexHeaderId.EndOfBody, new byte[0]);
                }//if--m_putDest==null, and now got the first body chunk.
                // else: Local destination stream is already open.

                //------------------------------------------------------
                // Write if
                //------------------------------------------------------
                if (body != null) {
                    System.Diagnostics.Debug.Assert(m_putDest != null, "m_putDest != null");
                    System.Diagnostics.Debug.Assert(m_putDest.CanWrite, "m_putDest.CanWrite");
                    m_putDest.Write(body, 0, body.Length);
                }//if--body

                //------------------------------------------------------
                // Close if
                //------------------------------------------------------
                if (close) {
                    System.Diagnostics.Debug.Assert(m_putDest != null, "m_putDest != null");
                    System.Diagnostics.Debug.Assert(m_putDest.CanWrite, "m_putDest.CanWrite");
                    // TODO Call an overridable method or an event at PUT operation completion?
                    if (m_putDest != null) {
                        m_putDest.Close();
                    }
                    PutReset();
                }//if--close

                success = true;
            } catch (IOException ioex) {
                PutReset();
                rspPdu = Host.CreateResponseWithDescription(ObexResponseCode.InternalServerError, "Write to content store failed with: " + ioex.Message);
            } catch (ObexClientOperationException opex) {
                PutReset();
                rspPdu = opex.ResponsePdu;
            } finally {
                if (!success) {
                    PutReset();
                }
            }
            //----------------------------------------------------------
            // Respond
            //----------------------------------------------------------
            return rspPdu;
        }

        //--------------------------------------------------------------
        protected virtual ObexCreatedPdu HandleAbortPdu(ObexParsedRequestPdu requestPdu)
        {
            Console.WriteLine("ABORT received, cancelling PUT (was " + (m_putDest != null ? "open" : "closed") + ").");
            PutReset();
            ObexCreatedPdu rspPdu = PduFactory.CreateResponse(ObexResponseCode.Success, null, null);
            return rspPdu;
        }

        private ObexCreatedPdu HandlePutAbandon()
        {
            PutReset();
            ObexCreatedPdu rspPdu = Host.CreateResponseWithDescription(ObexResponseCode.BadRequest,
                "Other request occurred during a Put operation.");
            return rspPdu;
        }

        //--------------------------------------------------------------
#if LOCAL_AUTHENTICATION
        private ObexCreatedPdu DoLocalAuth(ObexParsedRequestPdu requestPdu)
        {
            ObexCreatedPdu pduToSendInsteadOfSuccess;
            if (m_authState != AuthenticationState.Required) {
                return null;
            } else {
                if (requestPdu.Headers.Contains(ObexHeaderId.AuthResponse)) {
                    byte[] requestDigest, userId, nonce;
                    Authentication.ParseDigestResponseHeader(requestPdu.Headers.GetByteSeq(ObexHeaderId.AuthResponse),
                        out requestDigest, out userId, out nonce);
                    if (nonce != null && !ArraysEqual(m_lastChallengeNonceSent, nonce)) {
                        Console.WriteLine("Client tried to use a response nonce that we hadn't sent.");
                        throw new ObexClientFatalException("Client tried to use a response nonce that we hadn't sent.");
                    } // else...
                    byte[] expectedResult = Authentication.CreateDigestResponse(m_lastChallengeNonceSent, m_passphrase);
                    if (!ArraysEqual(expectedResult, requestDigest)) {
                        Console.WriteLine("Client sent the wrong passphrase.");
                        return CreateAuthChallengePdu(out pduToSendInsteadOfSuccess);
                    } // else ...
                    // Auth complete! :-)
                    m_authState = AuthenticationState.Completed;
                    return null; // Send the normal SUCCESS response.
                } else {
                    return CreateAuthChallengePdu(out pduToSendInsteadOfSuccess);
                }
            }
        }

        private ObexCreatedPdu CreateAuthChallengePdu(out ObexCreatedPdu pduToSendInsteadOfSuccess)
        {
            ObexHeaderCollection hdrs = new ObexHeaderCollection();
            byte[] nonce = CreateNonce();
            string realm = null;
            hdrs.Add(ObexHeaderId.AuthChallenge, Authentication.CreateDigestChallengeHeader(
                nonce, DigestChallengeOptions.None, realm));
            pduToSendInsteadOfSuccess = PduFactory.CreateResponse(true, ObexResponseCode.Unauthorized, hdrs, null);
            m_lastChallengeNonceSent = nonce;
            return pduToSendInsteadOfSuccess;
        }

        private bool ArraysEqual(byte[] arr1, byte[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;
            for (int i = 0; i < arr1.Length; ++i)
                if (arr1[i] != arr2[i])
                    return false;
            return true;
        }

        private byte[] CreateNonce()
        {
            byte[] x = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
            byte[] SomeKey = { 0x73, 0x00, 0x50, 0x30, 0x47, 0x38, 0xCF, 0xC2 };
            byte[] value = Array_Concat(x, SomeKey);
            byte[] nonce = System.Security.Cryptography.MD5.Create().ComputeHash(value);
            return nonce;
        }

        static T[] Array_Concat<T>(T[] arr1, T[] arr2)
        {
            T[] all = new T[arr1.Length + arr2.Length];
            int index = 0;
            Array.Copy(arr1, 0, all, index, arr1.Length);
            index += arr1.Length;
            Array.Copy(arr2, 0, all, index, arr2.Length);
            index += arr2.Length;
            System.Diagnostics.Debug.Assert(index == all.Length, "Copy finished at end of all array");
            return all;
        }
        static T[] Array_Concat<T>(params T[][] arrays)
        {
            int totalLen = 0;
            foreach (T[] a in arrays)
                totalLen += a.Length;
            T[] all = new T[totalLen];
            int index = 0;
            foreach (T[] a in arrays) {
                Array.Copy(a, 0, all, index, a.Length);
                index += a.Length;
            }
            System.Diagnostics.Debug.Assert(index == all.Length, "Copy finished at end of all array");
            return all;
        }
#endif

    }//class--ObexInboxServer

}
