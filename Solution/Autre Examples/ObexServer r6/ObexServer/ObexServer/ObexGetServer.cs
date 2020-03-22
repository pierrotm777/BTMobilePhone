#define GET_RESPONSE_INCLUDES_LENGTH
using System;
using System.Collections.Generic;
using System.Text;
using Brecham.Obex; //eg ObexHeaderCollection
using Brecham.Obex.Pdus; //eg ObexCreatedPdu
using System.IO;
using Brecham.Obex.Objects;
using System.Diagnostics;

namespace ObexServer
{
    public class ObexGetServer : ObexInboxServer
    {

        //--------------------------------------------------------------
        //
        /// <summary>
        /// The <c>Stream</c> from which the GET content is being read.
        /// </summary>
        Stream m_getSource;

        /// <summary>
        /// The aggregate set of metadata headers received in the current GET operation.
        /// </summary>
        ObexHeaderCollection m_getClientHdrs;

        //--------------------------------------------------------------
        public event EventHandler<CreateGetStreamEventArgs> CreateGetStream;

        private Stream DoCreateGetStream(ObexHeaderCollection getMetadata, ref ObexCreatedPdu errorResponsePdu)
        {
            CreateGetStreamEventArgs ea = new CreateGetStreamEventArgs(getMetadata, m_folder);
            OnCreateGetStream(ea);
            if (ea.GetStream == null) {
                // TODO OnCreateGetStream--Do we want to accept and code and description instead of a PDU?
                errorResponsePdu = ea.ErrorResponsePdu;
                return null;
            } else {
                // Verify its usefulness
                if (!ea.GetStream.CanRead) {
                    throw new ArgumentException("GetStream is not readable.");
                }
                return ea.GetStream;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ObexServer.ObexInboxServer.CreateGetStream"/> event.
        /// </summary>
        /// <param name="ea">
        /// A <see cref="T:ObexServer.CreateGetStreamEventArgs"/> instance that contains the event data.
        /// </param>
        protected virtual void OnCreateGetStream(CreateGetStreamEventArgs ea)
        {
            EventHandler<CreateGetStreamEventArgs> handler = CreateGetStream;
            if (handler != null) {
                Object sender = this;
                handler(sender, ea);
                // Called; now use the results.
            } else {
                // No handler!
                throw new ObexClientFatalException("No GET handler.");
            }
        }


        //--------------------------------------------------------------
        private void GetReset()
        {
            if (m_getSource != null) {
                m_getSource.Close();
            }
            m_getSource = null;
            m_getClientHdrs = null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Catching all exception when adding Length header, justing in case some Streams don't support it.")]
        protected override Brecham.Obex.Pdus.ObexCreatedPdu HandleGetPdu(Brecham.Obex.Pdus.ObexParsedRequestPdu requestPdu)
        {
            ObexCreatedPdu rspPdu;
            bool close = false;
            bool success = false; //For finally-based exception handler clean-up code.
            ObexHeaderCollection extraRspHdrs = null;
            //
            try {
                //------------------------------------------------------
                // Gather Metadata
                //------------------------------------------------------
                if (m_getSource != null) {
                    //TODO what about headers when already open?
                } else {
                    if (m_getClientHdrs == null) {
                        // The first PDU in this operation create a new collection.
                        m_getClientHdrs = new ObexHeaderCollection();
                        // Add two dummy headers to ensure that the Body headers are never added.
                        // Though in a GET such headers would be illegal...
                        m_getClientHdrs.Add(ObexHeaderId.Body, new byte[0]);
                        m_getClientHdrs.Add(ObexHeaderId.EndOfBody, new byte[0]);
                    }
                    // Combine with any previously received headers.
                    m_getClientHdrs.AppendSkippingAnyDuplicates(requestPdu.Headers);
                }

                //------------------------------------------------------
                // Check PDU
                //------------------------------------------------------
                System.Diagnostics.Debug.Assert(requestPdu.Opcode == ObexOpcode.Get || requestPdu.Opcode == ObexOpcode.GetNonFinal,
                    "Can only handle GET PDUs.");
                Host.CheckRequestPduDoesNotContain(ObexHeaderId.EndOfBody, requestPdu);
                Host.CheckRequestPduDoesNotContain(ObexHeaderId.Body, requestPdu);
                if (m_getSource != null) {
                    if (requestPdu.Opcode != ObexOpcode.Get) {
                        throw Host.CreateObexClientOperationException("After a GET Final Request every request should be Final.");
                    }
                }

                //------------------------------------------------------
                // Open (if not already), if we've received a 'Final' request.
                //------------------------------------------------------
                if (m_getSource == null && requestPdu.Opcode == ObexOpcode.Get/*-Final*/) {
                    // Ask the local consumer where to find this GET content.
                    rspPdu = null;
                    Stream openedStream = DoCreateGetStream(m_getClientHdrs, ref rspPdu);
                    if (openedStream != null) {
                        // Successful open, it returned the stream
                        m_getSource = openedStream;
#if GET_RESPONSE_INCLUDES_LENGTH
                        // Add Length header
                        try {
                            long fileLen = m_getSource.Length;
                            if (fileLen <= UInt32.MaxValue) {
                                UInt32 len = checked((UInt32)fileLen);
                                if (extraRspHdrs == null)
                                    extraRspHdrs = new ObexHeaderCollection();
                                extraRspHdrs.Add(ObexHeaderId.Length, len);
                            }
                        } catch (NotSupportedException) {
                            // ("A class derived from Stream does not support seeking.")
                        } catch (Exception ex) {
                            //System.Diagnostics.Debug.Fail("Exception in adding file length header.");
                            string msg = "Exception in adding file length header: "
                                + ex.GetType().FullName + ": " + ex.Message;
                            Console.WriteLine(msg);
#if ! NETCF
                            Trace.TraceWarning(msg);
#endif
                        }
#endif
                    } else {
                        // Error, he didn't know what to with/didn't like this Request.
                        // If it didn't return a response pdu create a generic one.
                        if (rspPdu == null) {
                            rspPdu = Host.CreateResponseWithDescription(ObexResponseCode.BadRequest,
                                "Bad Request, could not open local source.");
                        }
                        throw new ObexClientOperationException(rspPdu);
                    }//openedStream==null
                }

                //------------------------------------------------------
                // Create the response PDU
                //------------------------------------------------------
                if (m_getSource == null) {
                    rspPdu = PduFactory.CreateResponse(ObexResponseCode.Continue, null, null);
                } else {
                    //if (extraRspHdrs != null) {
                    //    Console.Out.Write("Sending GET response with: ");
                    //    extraRspHdrs.Dump(Console.Out);
                    //}
                    rspPdu = PduFactory.CreateResponse(ObexResponseCode.Continue, extraRspHdrs, m_getSource);
                    //
                    if (rspPdu.EndOfBodyStream) {
                        ObexHeaderCollection headers = new ObexHeaderCollection();
                        headers.Add(ObexHeaderId.EndOfBody, new byte[0]);
                        rspPdu = PduFactory.CreateResponse(ObexResponseCode.Success, headers, null);
                        close = true;
                    }
                }

                //------------------------------------------------------
                // Close if
                //------------------------------------------------------
                if (close) {
                    System.Diagnostics.Debug.Assert(m_getSource != null, "m_getSource != null");
                    System.Diagnostics.Debug.Assert(m_getSource.CanRead, "m_getSource.CanRead");
                    // TODO Call an overridable method or an event at GET operation completion?
                    if (m_getSource != null) {
                        m_getSource.Close();
                    }
                    GetReset();
                }//if--close

                success = true;
            } catch (IOException ioex) {
                GetReset();
                rspPdu = Host.CreateResponseWithDescription(ObexResponseCode.InternalServerError, "Read from content store failed with: " + ioex.Message);
            } catch (ObexClientOperationException opex) {
                GetReset();
                rspPdu = opex.ResponsePdu;
            } finally {
                if (!success) {
                    GetReset();
                }
            }
            //----------------------------------------------------------
            // Respond
            //----------------------------------------------------------
            return rspPdu;
        }

        protected override ObexCreatedPdu HandleAbortPdu(ObexParsedRequestPdu requestPdu)
        {
            Console.WriteLine("ABORT received, cancelling GET (was " + (m_getSource != null ? "open" : "closed") + ").");
            GetReset();
            return base.HandleAbortPdu(requestPdu);
        }

        protected override ObexCreatedPdu HandleConnectPdu(ObexParsedRequestPdu requestPdu)
        {
            ObexCreatedPdu rspPdu = base.HandleConnectPdu(requestPdu);
            // Confirm that we support Folder-Browsing, if the client asks for that service.
            if (requestPdu.Headers.Contains(ObexHeaderId.Target)) {
                byte[] target = requestPdu.Headers.GetByteSeq(ObexHeaderId.Target);
                byte[] fbTarget = ObexConstant.Target.FolderBrowsing;
                if (Arrays_Equal(fbTarget, target)) {
                    ObexResponseCode rspCode = (ObexResponseCode)rspPdu.Buffer[0];
                    if (rspCode == ObexResponseCode.Success) {
                        ObexHeaderCollection headers = new ObexHeaderCollection();
                        headers.Add(ObexHeaderId.Who, fbTarget);
                        rspPdu = PduFactory.CreateResponse(true, ObexResponseCode.Success, headers, null);
                    }
                }
            }
            return rspPdu;
        }

        private static bool Arrays_Equal<T>(T[] x, T[] y)
        {
            if (x.Length != y.Length)
                return false;
            for (int i = 0; i < x.Length; ++i) {
                if (!x[i].Equals(y[i]))
                    return false;
            }
            return true;
        }

        //----------------------------------------------------------------------
        protected override ObexCreatedPdu HandleSetPathPdu(ObexParsedRequestPdu requestPdu)
        {
            System.Diagnostics.Debug.Assert(requestPdu.Headers.Contains(ObexHeaderId.ProxyPreHeadersBytes),
                "All SetPath request PDUs should contain an extra (two) bytes before any headers.");
            byte[] preHeaders = requestPdu.Headers.GetByteSeq(ObexHeaderId.ProxyPreHeadersBytes);
            System.Diagnostics.Debug.Assert(preHeaders.Length == 2,
                "All SetPath request PDUs should contain extra TWO bytes before any headers.");
            SetPathFlags flags = (SetPathFlags)preHeaders[0];
            NameHeaderForSetPath nameStatus;
            if (!requestPdu.Headers.Contains(ObexHeaderId.Name)) {
                nameStatus = NameHeaderForSetPath.Omitted;
            } else {
                String name = requestPdu.Headers.GetString(ObexHeaderId.Name);
                if (String.IsNullOrEmpty(name)) {
                    nameStatus = NameHeaderForSetPath.Empty;
                } else {
                    nameStatus = NameHeaderForSetPath.Exists;
                }
            }
            // Do it now!
            FolderChangeEventArgs e = new FolderChangeEventArgs(m_folder);
            if (nameStatus == NameHeaderForSetPath.Empty) {
                e.FolderChangeType = FolderChangeType.Reset;
            } else {
                if (0 != (flags & SetPathFlags.Backup)) {
                    e.FolderChangeType = FolderChangeType.Up;
                }
                if (nameStatus == NameHeaderForSetPath.Exists) {
                    System.Diagnostics.Debug.Assert(requestPdu.Headers.Contains(ObexHeaderId.Name),
                        "Internal error, check for name header exists and non-blank above!");
                    bool mayCreateIfNotExist = 0 == (flags & SetPathFlags.DoNotCreateIfDoesNotExist);
                    e.ChildFolderName = requestPdu.Headers.GetString(ObexHeaderId.Name);
                    e.MayCreateIfNotExist = mayCreateIfNotExist;
                    if (e.FolderChangeType == FolderChangeType.Up) {
                        e.FolderChangeType = FolderChangeType.UpAndDown;
                    } else {
                        e.FolderChangeType = FolderChangeType.Down;
                    }
                }
            }
            OnFolderChange(e);
            if (e.ErrorResponsePdu != null) {
                return e.ErrorResponsePdu;
            } else {
                if (e.NewFolder == null) // This will cause the server to abort
                    throw new ArgumentException("The NewFolder property must be set on successful folder change.");
                m_folder = e.NewFolder;
                return Host.PduFactory.CreateResponse(false, ObexResponseCode.Success, null, null);
            }
        }

        enum NameHeaderForSetPath
        {
            Exists,
            Empty,
            Omitted
        }

        [Flags]
        private enum SetPathFlags : byte
        {
            None = 0,
            Backup = 1,
            DoNotCreateIfDoesNotExist = 2,
        }

        //--------
        protected virtual void OnFolderChange(FolderChangeEventArgs e)
        {
            EventHandler<FolderChangeEventArgs> h = FolderChange;
            if (h != null)
                h(this, e);
        }

        public event EventHandler<FolderChangeEventArgs> FolderChange;


        //----------------------------------------------------------------------

        public static Stream CreateFolderListing(bool hasParent, DirectoryInfo path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            DirectoryInfo[] dirs = path.GetDirectories();
            FileInfo[] files = path.GetFiles();
            return CreateFolderListing(hasParent, dirs, files);
        }

        public static Stream CreateFolderListing(bool hasParent, String path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            DirectoryInfo di = new DirectoryInfo(path);
            return CreateFolderListing(hasParent, di);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes",
            Justification = "We set xmlWriterSettings.CloseOutput = false thus only one Dispose done.")]
        public static Stream CreateFolderListing(bool hasParent, DirectoryInfo[] directories, FileInfo[] files)
        {
            if (directories == null) { directories = new DirectoryInfo[0]; }
            if (files == null) { files = new FileInfo[0]; }
            //
            MemoryStream src;
            int count = 0;
            using (MemoryStream dest = new MemoryStream()) {
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Indent = true;
                settings.CloseOutput = false;
                System.Xml.XmlWriter wtr = System.Xml.XmlWriter.Create(dest, settings);
                wtr.WriteStartDocument();
                try {
                    // <!DOCTYPE folder-listing SYSTEM "obex-folder-listing.dtd">
                    wtr.WriteDocType(ObexFolderListingParser.DocTypeName, null,
                        ObexFolderListingParser.SystemIdentifier, null);
                } catch (NotSupportedException) {
#if ! NETCF
                    // Suppress not supported on CF; but allow it to work in a later version.
                    throw;
#endif
                }
                // <folder-listing version="1.0">
                wtr.WriteStartElement("folder-listing");
                wtr.WriteAttributeString("version", "1.0");
                // <parent-folder />
                if (hasParent) {
                    ++count;
                    wtr.WriteElementString("parent-folder", null);
                }
                // <folder name="..." ... />
                foreach (DirectoryInfo cur in directories) {
                    ++count;
                    wtr.WriteStartElement("folder");
                    wtr.WriteAttributeString("name", cur.Name);
                    String dateString;
                    dateString = ObexHeaderConverter.DateTimeAsIso8601String(cur.CreationTime);
                    wtr.WriteAttributeString("created", dateString);
                    dateString = ObexHeaderConverter.DateTimeAsIso8601String(cur.LastWriteTime);
                    wtr.WriteAttributeString("modified", dateString);
                    dateString = ObexHeaderConverter.DateTimeAsIso8601String(cur.LastAccessTime);
                    wtr.WriteAttributeString("accessed", dateString);
                    wtr.WriteEndElement();
                }
                // <file name="..." ... />
                foreach (FileInfo cur in files) {
                    ++count;
                    wtr.WriteStartElement("file");
                    wtr.WriteAttributeString("name", cur.Name);
                    //
                    wtr.WriteStartAttribute("size");
                    wtr.WriteValue(cur.Length);
                    wtr.WriteEndAttribute();
                    //
                    String dateString;
                    dateString = ObexHeaderConverter.DateTimeAsIso8601String(cur.CreationTime);
                    wtr.WriteAttributeString("created", dateString);
                    dateString = ObexHeaderConverter.DateTimeAsIso8601String(cur.LastWriteTime);
                    wtr.WriteAttributeString("modified", dateString);
                    dateString = ObexHeaderConverter.DateTimeAsIso8601String(cur.LastAccessTime);
                    wtr.WriteAttributeString("accessed", dateString);
                    wtr.WriteEndElement();
                }
                //
                wtr.WriteEndElement();
                wtr.WriteEndDocument();
                wtr.Close();
                src = GetMemoryStreamContentAsReadOnly(dest);
                DebugVerifyFolderListing(dest);
                dest.Close();
            }
            Console.WriteLine("folder-listing contains {0} items", count);
            return src;
        }

        private static MemoryStream GetMemoryStreamContentAsReadOnly(MemoryStream source)
        {
            int length = checked((int)source.Length);
            MemoryStream copy = new MemoryStream(source.GetBuffer(), 0, length, false);
            return copy;
        }

        [Conditional("DEBUG")]
        private static void DebugVerifyFolderListing(MemoryStream listing)
        {
            VerifyFolderListing(GetMemoryStreamContentAsReadOnly(listing));
        }

        private static void VerifyFolderListing(Stream listing)
        {
            ObexFolderListingParser p = new ObexFolderListingParser(listing);
            ObexFolderListing fl = p.GetAllItems();
        }

    }//class


}
