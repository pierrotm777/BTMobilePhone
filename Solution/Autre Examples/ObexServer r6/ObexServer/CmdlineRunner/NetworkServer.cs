// %%% #define LOCAL_AUTHENTICATION
// %%% #define MAXIMUM_ACTIVE_SERVERS__
using System;
using System.IO;
using ObexServer;
using Brecham.Obex; // eg ObexHeaderCollection, ObexConstant
using System.Net.Sockets; // eg ProtocolFamily
using System.Net; // eg IPAddress
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;


namespace CmdlineRunner
{

    /// <summary>
    /// The infrastructure that handles listening on the specified network protocol 
    /// and creates a new instance of the ObexServer class passing it the NetworkStream.
    /// </summary>
    /// <remarks>
    /// Supports only one connection, waiting for its completion, and has a 'Killer' thread 
    /// that kills the server if it has ben idle for three minutes.
    /// </remarks>
    public class NetworkServer
    {
        //----------------------------------------
        static readonly bool UseSync = false;
        static readonly bool MultiConnection = false;
        // UseSync=true and MultiConnection=true are incompatible.
        const int TcpListenBacklog = 20;

        //----------------------------------------
        private const int BufferSize
            //= 1 * 1024;
            // 16KB seems a good compromise, see http://32feet.net/files/folders/objectexchange/entry2575.aspx
            = 16 * 1024;
            //= 64 * 1024 - 1;

        //----------------------------------------
        private ListenProtocol m_protocolToListenOn;
        private bool m_bluetoothFtp;
        bool m_useSync; //HACK m_useSync
        //
        ObexServerHost m_host;
        Dictionary<ObexServerHost, ActiveObexServer> m_activeHosts
             = new Dictionary<ObexServerHost,ActiveObexServer>();
        object m_lockActiveHosts = new object();
        //
        SocketListener m_hackListenSocket;
        // Set if any other object needs to be closed.  For instance in Bluetooth
        // we need to call BluetoothListener.Stop to remove the SDP record.
        IDisposable m_hackListenDisposable;
        //
        volatile bool m_shuttingDown;
        System.Threading.ManualResetEvent m_shuttingDownEvent;
        volatile bool m_active;
        //
        DirectoryInfo m_rootFolder;
        //
        public TextWriter m_console = Console.Out;

        //----------------------------------------
        public NetworkServer()
        {
            HackUseSync = UseSync;
        }

        //----------------------------------------
        public bool HackUseSync
        {
            get { return m_useSync; }
            set { m_useSync = value; }
        }

        public ListenProtocol NetworkProtocol
        {
            get { return m_protocolToListenOn; }
            set { m_protocolToListenOn = value; }
        }

        public bool BluetoothFtpProfile
        {
            get { return m_bluetoothFtp; }
            set { m_bluetoothFtp = value; }
        }

        //----------------------------------------
        public void Start()
        {
            try {
                m_active = true;

                String rootFolder;
#if NETCF
                String cb = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                // Running on full FX, then the return from AssemblyName.CodeBase is
                // in URI format, that is as specified in MSDN.  Support that form too.
                String cbPath = new Uri(cb).LocalPath;
                rootFolder = Path.GetDirectoryName(cbPath);
#else
                rootFolder = Environment.CurrentDirectory;
#endif
                // Normalize the path for security checking later, and create if not exists.
                rootFolder = Path.Combine(rootFolder, "PutToDiskServer uploads");
                DirectoryInfo di = new DirectoryInfo(rootFolder);
                if (!di.Exists) {
                    di.Create();
                }
                m_rootFolder = di;
                //
                SocketListener svr;
                IDisposable listeningSocketOwnerIDisposable;
                svr = NetworkStart(out listeningSocketOwnerIDisposable);
                m_hackListenSocket = svr;
                m_hackListenDisposable = listeningSocketOwnerIDisposable;
                ListenAndHandleForever(svr);
            } finally {
                m_active = false;
            }
        }

        public bool IsActive { get { return m_active; } }

        private SocketListener NetworkStart(out IDisposable listeningSocketOwnerIDisposable)
        {
            SocketListener svr;
            listeningSocketOwnerIDisposable = null;
            if (m_protocolToListenOn == ListenProtocol.Irda) {
                svr = ListenOnIrda();
            } else if (m_protocolToListenOn == ListenProtocol.Bluetooth) {
                if (m_bluetoothFtp) {
                    svr = ListenOnBluetoothFtp();
                } else {
                    svr = ListenOnBluetooth(out listeningSocketOwnerIDisposable);
                }
            } else if (m_protocolToListenOn == ListenProtocol.Ipv4
                        || m_protocolToListenOn == ListenProtocol.Ipv6
                        || m_protocolToListenOn == ListenProtocol.Ipv6And4Dual) {
                bool localConnectionsOnly = false;
                svr = ListenOnTcpip(m_protocolToListenOn, localConnectionsOnly);
            } else {
                throw new ArgumentException("Not a supported protocol.");
            }
            return svr;
        }

        private static SocketListener ListenOnIrda()
        {
            // good
            InTheHand.Net.Sockets.IrDAListener lsnr = new InTheHand.Net.Sockets.IrDAListener("OBEX");
            lsnr.Start();
            return new SocketListener(lsnr);
        }

        private SocketListener ListenOnBluetooth(out IDisposable svrDisposable)
        {
            UInt16 serviceClassUuid16 = 0x1105;
            Guid serviceClassUuid = InTheHand.Net.Bluetooth.BluetoothService.ObexObjectPush;
            //
            ServiceElement pdl = ServiceRecordHelper.CreateGoepProtocolDescriptorList();
            ServiceElement classList = new ServiceElement(ElementType.ElementSequence,
               new ServiceElement(ElementType.Uuid16, serviceClassUuid16));
            const Byte SupportedFormatsValue = 0xFF;
            ServiceElement supdFormats = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.UInt8, SupportedFormatsValue));
            ServiceElement svcName = new ServiceElement(ElementType.TextString, "OBEX Object Push");
            ServiceElement provName = new ServiceElement(ElementType.TextString, "Brecham.Obex");
            ServiceElement langBaseList = CreateEnglishUtf8PrimaryLanguageServiceElement();
            //
            ServiceRecord record = new ServiceRecord(
               new ServiceAttribute(UniversalAttributeId.ServiceClassIdList, classList),
               new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, pdl),
               new ServiceAttribute(ObexAttributeId.SupportedFormatsList, supdFormats),
               new ServiceAttribute(UniversalAttributeId.LanguageBaseAttributeIdList, langBaseList),
               new ServiceAttribute(ServiceRecord.CreateLanguageBasedAttributeId(
                        UniversalAttributeId.ServiceName, LanguageBaseItem.PrimaryLanguageBaseAttributeId),
                    svcName),
               new ServiceAttribute(ServiceRecord.CreateLanguageBasedAttributeId(
                        UniversalAttributeId.ProviderName, LanguageBaseItem.PrimaryLanguageBaseAttributeId),
                    provName)
               );
            //
            m_console.WriteLine("Gonna listen on: " + serviceClassUuid);
            InTheHand.Net.Sockets.BluetoothListener lsnr = Create_BluetoothListener(
                serviceClassUuid,
                record);
            lsnr.ServiceClass = InTheHand.Net.Bluetooth.ServiceClass.ObjectTransfer;
            lsnr.Start();
            svrDisposable = new BluetoothListenerDisposer(lsnr);
            return new SocketListener(lsnr);
        }

        private SocketListener ListenOnBluetoothFtp()
        {
            UInt16 serviceClassUuid16 = 0x1106;
            Guid serviceClassUuid = InTheHand.Net.Bluetooth.BluetoothService.ObexFileTransfer;
            //
            ServiceElement pdl = ServiceRecordHelper.CreateRfcommProtocolDescriptorList();
            ServiceElement classList = new ServiceElement(ElementType.ElementSequence,
               new ServiceElement(ElementType.Uuid16, serviceClassUuid16));
            ServiceElement svcName = new ServiceElement(ElementType.TextString, "OBEX File Transfer");
            ServiceElement provName = new ServiceElement(ElementType.TextString, "Brecham.Obex");
            ServiceElement langBaseList = CreateEnglishUtf8PrimaryLanguageServiceElement();
            //
            ServiceRecord record = new ServiceRecord(
               new ServiceAttribute(UniversalAttributeId.ServiceClassIdList, classList),
               new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, pdl),
               new ServiceAttribute(UniversalAttributeId.LanguageBaseAttributeIdList, langBaseList),
               new ServiceAttribute(ServiceRecord.CreateLanguageBasedAttributeId(
                        UniversalAttributeId.ServiceName, LanguageBaseItem.PrimaryLanguageBaseAttributeId),
                    svcName),
               new ServiceAttribute(ServiceRecord.CreateLanguageBasedAttributeId(
                        UniversalAttributeId.ProviderName, LanguageBaseItem.PrimaryLanguageBaseAttributeId),
                    provName)
               );
            //
            m_console.WriteLine("Gonna listen on: " + serviceClassUuid);
            InTheHand.Net.Sockets.BluetoothListener lsnr = Create_BluetoothListener(
                serviceClassUuid, record);
            lsnr.ServiceClass = InTheHand.Net.Bluetooth.ServiceClass.ObjectTransfer;
            lsnr.Start();
            return new SocketListener(lsnr);
        }

        private InTheHand.Net.Sockets.BluetoothListener Create_BluetoothListener(Guid serviceClassUuid, ServiceRecord sdpRecord)
        {
            bool pickStack = false;
            InTheHand.Net.Sockets.BluetoothListener lsnr;
            if (!pickStack) {
                lsnr = new InTheHand.Net.Sockets.BluetoothListener(
                    serviceClassUuid, sdpRecord);
            } else {
                BluetoothRadio[] radios = BluetoothRadio.AllRadios;
                BluetoothRadio selected = radios[radios.Length - 1];
                lsnr = selected.StackFactory.CreateBluetoothListener(serviceClassUuid, sdpRecord);
            }
            return lsnr;
        }

        private static ServiceElement CreateEnglishUtf8PrimaryLanguageServiceElement()
        {
            ServiceElement englishUtf8PrimaryLanguage
               = LanguageBaseItem.CreateElementSequenceFromList(
                  new LanguageBaseItem[] {
            new LanguageBaseItem("en", LanguageBaseItem.Utf8EncodingId,
               LanguageBaseItem.PrimaryLanguageBaseAttributeId)});
            return englishUtf8PrimaryLanguage;
        }

        internal class BluetoothListenerDisposer : IDisposable
        {
            InTheHand.Net.Sockets.BluetoothListener m_lsnr;

            public BluetoothListenerDisposer(InTheHand.Net.Sockets.BluetoothListener lsnr)
            {
                m_lsnr = lsnr;
            }

            #region IDisposable Members

            void IDisposable.Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "disposing")]
            protected void Dispose(bool disposing)
            {
                m_lsnr.Stop();
            }

            #endregion
        }//class

        private static SocketListener ListenOnTcpip(ListenProtocol proto, bool localConnectionsOnly)
        {
            // good
            IPAddress serverAddress;
            if (proto == ListenProtocol.Ipv4) {
                serverAddress = localConnectionsOnly
                                ? IPAddress.Loopback   // Allow local clients only.
                                : IPAddress.Any;
            } else if (proto == ListenProtocol.Ipv6 || proto == ListenProtocol.Ipv6And4Dual) {
                serverAddress = localConnectionsOnly
                                ? IPAddress.IPv6Loopback   // Allow local clients only.
                                : IPAddress.IPv6Any;
            } else {
                throw new ArgumentException("Unsupported IP server type.");
            }
            //
            //
            // Stupid TcpListener on NETCF Finalizes the socket!!!!
            //TcpListener lsnr = new TcpListener(serverAddress, ObexConstant.TcpPortNumber);
            Socket sock = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Unspecified);
            if (proto == ListenProtocol.Ipv6And4Dual) {
                // This is supported in Windows Vista.
                // 27 is IPV6_V6ONLY
                SocketOptionName Ipv6v6Only = (SocketOptionName)27;
                //lsnr.Server.SetSocketOption(SocketOptionLevel.IPv6, Ipv6v6Only, 0);
                sock.SetSocketOption(SocketOptionLevel.IPv6, Ipv6v6Only, 0);
            }
            //lsnr.Start();
            sock.Bind(new IPEndPoint(serverAddress, ObexConstant.TcpPortNumber));
            sock.Listen(TcpListenBacklog);
            return new SocketListener(sock);
        }

        private void Killer()
        {
            while (true) {
                // Every minute.
                bool signalled = m_shuttingDownEvent.WaitOne(60 * 1000, false);
                if (signalled) {
                    // Shutting down.
                    break;
                }
                if (!MultiConnection) {
                    if (m_host == null) {
                        continue;
                    }
                    int timeIdle = Environment.TickCount - m_host.LastWriteTickCount;
                    // Kill any servers that have been idle for three minutes.
                    if (timeIdle > 3 * 60 * 1000) {
#if ! NETCF
                        Console.WriteLine("Killing idle server.");
                        System.Diagnostics.Trace.WriteLine("Killing idle server.");
#endif // ! PocketPC
                        m_host.Kill();
                    }
                } else {
                    Dictionary<ObexServerHost, ActiveObexServer> hosts = GetActiveHostsThreadSafe();
                    // A host could exit in this gap, but that should be ok, as it'll
                    // not be GC'd, and Kill can be called multiple times.
                    foreach (ActiveObexServer curHost in hosts.Values) {
                        int timeIdle = Environment.TickCount - curHost.Host.LastWriteTickCount;
                        // Kill any servers that have been idle for three minutes.
                        if (timeIdle > 3 * 60 * 1000) {
#if ! NETCF
                            Console.WriteLine("Killing idle server.");
                            System.Diagnostics.Trace.WriteLine("Killing idle server.");
#endif // ! PocketPC
                            // This shouldn't throw, so no need to try/catch hopefully!
                            curHost.Host.Kill();
                            // Remember that the exit event removes the host from the list.
                        }
                    }//foreach
                }
            }//while
        }

        private Dictionary<ObexServerHost, ActiveObexServer> GetActiveHostsThreadSafe()
        {
            Dictionary<ObexServerHost, ActiveObexServer> hosts;
            lock (m_lockActiveHosts) {
                hosts = new Dictionary<ObexServerHost, ActiveObexServer>(m_activeHosts);
            }
            return hosts;
        }

        private void ListenAndHandleForever(SocketListener serverSock)
        {
            m_shuttingDownEvent = new System.Threading.ManualResetEvent(false);
            System.Threading.Thread thrd = new System.Threading.Thread(Killer);
            thrd.Start();
            //
            m_console.WriteLine(DateTime.Now.ToShortTimeString() + ", server started, listening on: " + serverSock.LocalEndPoint.ToString());
            while (true) {
                m_console.WriteLine("Waiting for connection...");
                SocketClient peerSock;
                try {
                    peerSock = serverSock.Accept();
                } catch (SocketException sex) {
                    if (m_shuttingDown) {
#if NETCF
                        const int SocketError_Interrupted = 10004;
                        System.Diagnostics.Debug.Assert(SocketError_Interrupted == sex.ErrorCode,
                            "At shutdown expected error 10004, not " + sex.ErrorCode);
#else
                        m_console.WriteLine("At shutdown server socket error was " + sex.ErrorCode);
#endif
                        break;
                    } else {
                        throw;
                    }
                } catch (Exception) {
                    // e.g. ObjectDisposedException, TypeLoadException on NETCF really!!!
                    if (m_shuttingDown) {
                        break;
                    } else {
                        throw;
                    }
                }
                DateTime start = DateTime.UtcNow;
                m_console.WriteLine(DateTime.Now.ToShortTimeString() + ", new connection from: " + peerSock.RemoteEndPoint.ToString());
                NetworkStream peer = peerSock.GetStream(); //new NetworkStream(peerSock, true);
                // CreatePutStream is now a Event on ObexInboxServer so we don't
                // have to subclass ObexInboxServer to SavePutToCurrentDirectoryObexServer.
                // Instead the contents of that class are now method SavePutToCurrentDirectory,
                // which we set on the event.
                //HACK RESTORE!!!!ObexInboxServer handler = new ObexInboxServer();
                ObexGetServer handler = new ObexGetServer();
                handler.SetFolder(m_rootFolder);
                // Handle the events
                handler.CreatePutStream
                    += SavePutToCurrentDirectory;
                //Or- += WritePutToNull;
                handler.CreateGetStream += LoadGetFromCurrentDirectory;
                handler.FolderChange += ChangeFolder;
#if LOCAL_AUTHENTICATION
                handler.SetLocalPassphrase("andy");
#endif
                // Create the host and start
                ObexServerHost host = CreateHost(peerSock, peer, handler);
                if (!MultiConnection) {
                    RunSingleConnectionBlocking(host, start);
                } else {
                    RunNewMultiConnectionNonBlocking(host, start);
                }
            }//while
        }

        private void RunSingleConnectionBlocking(ObexServerHost host, DateTime start)
        {
            m_host = host;
            //m_host.Exit += delegate { Console.WriteLine("Exit event!"); }; //just to test and set a breakpoint on
            m_host.Start();  // Go!
            m_host.ExitWaitHandle.WaitOne(); // Block until the server completes.
            m_console.WriteLine("Connection lasted {0}", DateTime.UtcNow - start);
            m_console.WriteLine(DateTime.Now.ToShortTimeString() + ", server instance exited due to: " + m_host.ExitReason);
            //?m_host.RethrowAnyError();//HACK NEW BAD????
            m_host = null;
        }

        private void RunNewMultiConnectionNonBlocking(ObexServerHost host, DateTime start)
        {
            if (host.StartBlocks)
                throw new ArgumentException("In multi-threaded network server mode the server hosts must run non-blocking.");
            ActiveObexServer record = new ActiveObexServer(host, start);
            host.Exit += MultiServerHostExited;
            lock (m_lockActiveHosts) {
                m_activeHosts.Add(host, record);
            }
            host.Start();  // Go!
            //----
            // TODO Allow only N-hundred active servers?  Wait here for one to exit...
#if MAXIMUM_ACTIVE_SERVERS__ 
            int MaximumActiveServers = 100;
            TimeSpan TooManyActiveServersWaitTimeout = new TimeSpan(0, 10, 0);
            lock (m_lockActiveHosts) {
                while (m_activeHosts.Count > MaximumActiveServers) {
#if !PocketPC
                    bool signalled = System.Threading.Monitor.Wait(m_lockActiveHosts, TooManyActiveServersWaitTimeout);
                    Debug.Assert(signalled, "Just very long running connections, so none has exited recently?");
#else
                    System.Threading.Thread.Sleep(10 * 1000);
#endif
                }
            }
#endif
        }

        void MultiServerHostExited(object sender, EventArgs e)
        {
            ObexServerHost host = (ObexServerHost)sender;
            ActiveObexServer record;
            lock (m_lockActiveHosts) {
                record = m_activeHosts[host];
                bool removed = m_activeHosts.Remove(host);
                Debug.Assert(removed, "Did find the host to remove!");
#if MAXIMUM_ACTIVE_SERVERS__
#if !PocketPC
                // In case there were the maximum number of hosts already running, 
                // wake the start code so that it can now start another server.
                System.Threading.Monitor.Pulse(m_lockActiveHosts);
#endif
#endif
            }
            m_console.WriteLine("Connection lasted {0}", DateTime.UtcNow - record.StartTime);
            m_console.WriteLine(DateTime.Now.ToShortTimeString() + ", server instance exited due to: " + record.Host.ExitReason);
            //?m_host.RethrowAnyError();//HACK NEW BAD????
        }



        class ActiveObexServer : IEquatable<ActiveObexServer >
        {
            ObexServerHost m_host;
            DateTime m_start;

            //--------
            public ActiveObexServer(ObexServerHost host, DateTime start)
            {
                m_host = host;
                m_start = start;
            }

            //--------
            public ObexServerHost Host { get { return m_host; } }
            public DateTime StartTime { get { return m_start; } }

            //--------
            #region IEquatable<ActiveObexServer> Members
            public bool Equals(ActiveObexServer other)
            {
                if (other == null)
                    return false;
                return other.m_host == this.m_host;
            }
            #endregion

            public override bool Equals(object obj)
            {
                ActiveObexServer other = obj as ActiveObexServer;
                return this.Equals(other);
            }

            public override int GetHashCode()
            {
                return m_host.GetHashCode();
            }
        }

        private ObexServerHost CreateHost(SocketClient peerSocket, NetworkStream peer, IObexServer handler)
        {
            ObexServerHost h = CreateHost(m_useSync, peerSocket, peer, handler);
            if (peerSocket != null) {
                h.SetRemoteEndPoint(peerSocket.RemoteEndPoint);
            }
            return h;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "peerSocket",
            Justification = "Is used by Async2ObexServerHost for instance")]
        public ObexServerHost CreateHost(bool useSync, SocketClient peerSocket, Stream peer, IObexServer handler)
        {
            ObexServerHost host;
            if (useSync) {
                host = new SyncObexServerHost(peer, BufferSize);
            } else {
                host = new AsyncObexServerHost(peer, BufferSize);
                //host = new Async2ObexServerHost(peerSocket.GetSocketIfAvailable(), peer, BufferSize);
            }
            host.AddHandler(handler);
            m_console.WriteLine("IObexServerHost is type: " + host.GetType().FullName);
            return host;
        }


        //--------------------------------------------------------------------------
        public void StopHack()
        {
            m_shuttingDown = true;
            m_shuttingDownEvent.Set();
            try {
                try {
                    if (m_hackListenSocket != null) {
                        m_hackListenSocket.Close();
                    }
                } finally {
                    if (m_hackListenDisposable != null) {
                        m_hackListenDisposable.Dispose();
                    }
                }
            } finally {
                if (!MultiConnection) {
                    if (m_host != null) {
                        m_host.Kill();
                    }
                } else {
                    Dictionary<ObexServerHost, ActiveObexServer> hosts = GetActiveHostsThreadSafe();
                    foreach (ActiveObexServer cur in hosts.Values) {
                        cur.Host.Kill();
                        m_console.WriteLine();
                    }
                }
            }//try/finally
        }


        //--------------------------------------------------------------------------
        // Provide a event implementation that writes PUTs to disk by Name.
        //--------------------------------------------------------------------------
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
           Justification = "Need to catch all error on opening the specified file.")]
        void SavePutToCurrentDirectory(object sender, CreatePutStreamEventArgs e)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            if (e == null)
                throw new ArgumentNullException("e");
            IObexServer svr = (IObexServer)sender;
            ObexServerHost host = svr.Host;

            if (e.PutMetadata.Contains(ObexHeaderId.Name)) {
                Stream dest;
                String name = e.PutMetadata.GetString(ObexHeaderId.Name);
                // TODO !!! check for sneeky hacker paths !!!
                String destPath = CreateFilePathSafely(svr, e.CurrentFolder, name);
                try {
                    dest = File.Create(destPath);
                    m_console.Write("Opened PUT content stream for: " + name + ", with ");
                    e.PutMetadata.Dump(m_console);
                    e.PutStream = dest;
                } catch (Exception ioex) { // Have to catch IOEx*, ArgEx*, and UnauthorizedAccessException etc
                    Console.WriteLine("Failed to open file for PUT with: " + ioex.Message);
                    // Don't tell hackers the details!!!
                    e.ErrorResponsePdu = host.CreateResponseWithDescription("Could not open specified file.");
                    e.PutStream = null;
                }
            } else {
                // Not opened stream, as didn't known Name, so complain.
                e.ErrorResponsePdu = host.CreateResponseWithDescription("No PUT Name given.");
                e.PutStream = null;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Need to catch all error on opening the specified file.")]
        void LoadGetFromCurrentDirectory(object sender, CreateGetStreamEventArgs e)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            if (e == null)
                throw new ArgumentNullException("e");
            IObexServer svr = (IObexServer)sender;
            ObexServerHost host = svr.Host;

            //HACK folder-listing
            if (e.GetMetadata.Contains(ObexHeaderId.Type)) {
                byte[] typeBuf = e.GetMetadata.GetByteSeq(ObexHeaderId.Type);
                String type = ObexHeaderConverter.StringFromAsciiByteSeq(typeBuf);
                if (type == ObexConstant.Type.FolderListing) {
                    if (e.GetMetadata.Contains(ObexHeaderId.Name)
                            && !String.IsNullOrEmpty(e.GetMetadata.GetString(ObexHeaderId.Name))) {
                        e.ErrorResponsePdu = host.CreateResponseWithDescription(
                            "Don't current handle getting a folder-listing for a different folder.");
                        e.GetStream = null;
                        return;
                    }
                    bool hasParent = e.CurrentFolder.FullName.Length > m_rootFolder.FullName.Length;
                    Stream strm = ObexGetServer.CreateFolderListing(hasParent, e.CurrentFolder);
                    e.GetStream = strm;
                    m_console.WriteLine("Folder-listing.");
                    return;
                }
            }
            if (e.GetMetadata.Contains(ObexHeaderId.Name)) {
                Stream dest;
                String name = e.GetMetadata.GetString(ObexHeaderId.Name);
                // TODO !!! check for sneeky hacker paths !!!
                String destPath = CreateFilePathSafely(svr, e.CurrentFolder, name);
                try {
                    dest = File.OpenRead(destPath);
                    m_console.Write("Opened GET content stream for: " + name + ", with ");
                    e.GetMetadata.Dump(m_console);
                    e.GetStream = dest;
                } catch (Exception ioex) { // Have to catch IOEx*, ArgEx*, and UnauthorizedAccessException etc
                    m_console.WriteLine("Failed to open file for GET with: " + ioex.Message);
                    // Don't tell hackers the details!!!
                    e.ErrorResponsePdu = host.CreateResponseWithDescription("Could not open specified file.");
                    e.GetStream = null;
                }
            } else {
                // Not opened stream, as didn't known Name, so complain.
                e.ErrorResponsePdu = host.CreateResponseWithDescription("No GET Name given.");
                e.GetStream = null;
            }
        }

        void ChangeFolder(object sender, FolderChangeEventArgs e)
        {
            DirectoryInfo newFolder;
            switch (e.FolderChangeType) {
                case FolderChangeType.Up:
                    newFolder = e.CurrentFolder.Parent;
                    break;
                case FolderChangeType.Down:
                    newFolder = new DirectoryInfo(Path.Combine(e.CurrentFolder.FullName, e.ChildFolderName));
                    break;
                case FolderChangeType.UpAndDown:
                    newFolder = e.CurrentFolder.Parent;
                    newFolder = new DirectoryInfo(Path.Combine(newFolder.FullName, e.ChildFolderName));
                    break;
                default: // Reset
                    System.Diagnostics.Debug.Assert(e.FolderChangeType == FolderChangeType.Reset,
                        "Unknown FolderChangeType: " + e.FolderChangeType);
                    newFolder = m_rootFolder;
                    break;
            }
            // Normalize and security check (e.g. "..")!
            IObexServer server = (IObexServer)sender;
            DirectoryInfo di = FolderNormalizeAndSecurityCheck(newFolder, m_rootFolder);
            if (di == null) {
                m_console.WriteLine("Bad folder change.");
                e.ErrorResponsePdu = server.Host
                    .CreateResponseWithDescription("Bad folder change.");
                return;
            }
            string actionMsgFmt = "Changed to folder: '{0}'";
            // Handle non-existant cases.
            if (e.MayCreateIfNotExist
                    && (e.FolderChangeType == FolderChangeType.Down
                        || e.FolderChangeType == FolderChangeType.UpAndDown
                        && !di.Exists)) {
                di.Create();
                actionMsgFmt = "Created and changed to folder: '{0}'";
            }
            if (!di.Exists) {
                e.ErrorResponsePdu = server.Host
                    .CreateResponseWithDescription("No such folder.");
                return;
            }
            // Success
            e.NewFolder = di;
            m_console.WriteLine(actionMsgFmt, e.NewFolder);
        }

        private DirectoryInfo FolderNormalizeAndSecurityCheck(DirectoryInfo newFolderTmp, DirectoryInfo rootFolder)
        {
            String normalizedPath = newFolderTmp.FullName;
#if ! NETCF
            System.Diagnostics.Debug.Assert(
                -1 == normalizedPath.IndexOf(Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar),
                "Normalized path should have removed any change-up .. parts!");
#endif
            if (normalizedPath.StartsWith(rootFolder.FullName, StringComparison.Ordinal)) {
                // Good, a child folder.
                return newFolderTmp;
            } else {
                // Bad, a parent or unrelated folder!!!!!!!
                return null;
            }
        }

        public static string FolderNormalizeAndSecurityCheck(string newFolder, string rootFolder, out DirectoryInfo folder)
        {
            folder = new DirectoryInfo(newFolder);
            // TODO normalizedPath = di.FullName -- This seems not to work on the NETCF beware!!!!!
            String normalizedPath = folder.FullName;
#if ! NETCF
            System.Diagnostics.Debug.Assert(
                -1 == normalizedPath.IndexOf(Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar),
                "Normalized path should have removed any change-up .. parts!");
#endif
            if (normalizedPath.StartsWith(rootFolder, StringComparison.Ordinal)) {
                // Good, a child folder.
                return normalizedPath;
            } else {
                // Bad, a parent or unrelated folder!!!!!!!
                folder = null;
                return null;
            }
        }

        /// <exclude/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Using DirectoryInfo as we don't want a file.")]
        public static String CreateFilePathSafely(IObexServer server, DirectoryInfo folder, String name)
        {
            if (folder == null)
                throw new ArgumentNullException("folder");
            // We're going to use 'name' as a pathname, so do so!
            // In a platform specific manner removing any directory changes etc...
            String filename = Path.GetFileName(name); // Removes any directory parts including ".." (hopefully!)
            String path = Path.Combine(folder.FullName, filename);
            System.Diagnostics.Debug.Assert(path.StartsWith(folder.FullName, StringComparison.Ordinal),
                "Path.GetFileName should have made the path combine safe!");
            return path;
        }

        //--------------------------------------------------------------------------
        // xxxxxxProvide a event implementation that writes PUTs to disk by Name.
        //--------------------------------------------------------------------------
        void WritePutToNull(object sender, CreatePutStreamEventArgs e)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            if (e == null)
                throw new ArgumentNullException("e");
            IObexServer svr = (IObexServer)sender;
            ObexServerHost host = svr.Host;

            if (e.PutMetadata.Contains(ObexHeaderId.Name)) {
                Stream dest;
                String name = e.PutMetadata.GetString(ObexHeaderId.Name);
                dest = new NullStream();
                m_console.WriteLine("Opened 'null' stream for: " + name);
                e.PutStream = dest;
            } else {
                // Not opened stream, as didn't known Name, so complain.
                e.ErrorResponsePdu = host.CreateResponseWithDescription("No PUT Name given.");
                e.PutStream = null;
            }
        }


    }//class

}//namespace
