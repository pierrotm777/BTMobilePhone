using System;
using System.Text;
using Brecham.CrossPlatform;

#if NUNIT
using NUnit.Framework;
#endif // NUNIT

using System.Net.Sockets; //eg ProtocolFamily
using System.Net;   //eg EndPoint
using InTheHand.Net.Sockets;
using InTheHand.Net;

using System.IO;


namespace Brecham.Obex.Net
{

    /// <summary>
    /// The base class of a set of classes that handle connection to a service on a peer
    /// machine.
    /// </summary>
    /// <remarks>
    /// The base class of a set of classes that handle connection to a service on a peer
    /// machine. As well as connecting, they also handle disconnect and clean-up using 
    /// the IDisposable pattern i.e. with the Dispose method.
    /// </remarks>
    public abstract class TransportConnection : IDisposable
    {
        //--------------------------------------------------------------
        //--------------------------------------------------------------
        /// <summary>
        /// A <see cref="T:System.Net.Sockets.ProtocolFamily"/> member for Bluetooth.
        /// </summary>
        public const ProtocolFamily ProtocolFamily_Bluetooth = (ProtocolFamily)0x20;

        //--------------------------------------------------------------
        //--------------------------------------------------------------

        // <summary>
        // The Service to connect to eg "obex-push", "obex-ftp".
        // </summary>
        // <remarks>
        // The Protocol, Peer, and Service values are used at transport connect time, 
        // but the Service value is also used at OBEX Connect time, so we store it
        // for use then.
        // </remarks>
        /// <exclude/>
        protected String m_service;

        // Is only set at Connecting..., for use for say:
        // try{ conn.Connect() } catch(...) { WriteLine("Failed to connect to: " + conn.RequestedRemoteAddress); }
        private String m_addrString;
        //
        bool m_connectedTransport;
        bool m_didConnect;  // Whether the complete connect operation succeeded.
        Socket m_socket;
        IDisposable m_disposableClient;
        NetworkStream m_peerStream;

        //--------------------------------------------------------------
        //--------------------------------------------------------------

        //bool m_forceTcp;
        //[Obsolete]
        //public bool ForceTcp
        //{
        //    get { return m_forceTcp; }
        //    set { m_forceTcp = value; }
        //}


        //--------------------------------------------------------------
        /// <summary>
        /// Gets whether the connection has been made.
        /// </summary>
        /// <remarks>
        /// That is, whether <see cref="M:Connect"/> has been called and was successful.
        /// </remarks>
        public bool Connected { get { return m_didConnect; } }

        //--------------------------------------------------------------

        /// <summary>
        /// Returns the <see cref="T:System.Net.Sockets.NetworkStream"/> used to send and receive data.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <c>GetStream</c> returns a <c>NetworkStream</c> that you can use to send 
        /// and receive data. The <c>NetworkStream</c> class inherits from the 
        /// <see cref="T:System.IO.Stream"/> class, which provides a rich collection
        /// of methods and properties used to facilitate network communications.
        /// </para><para>
        /// You must call the <see cref="M:Connect"/> method first, or the <c>GetStream</c>
        /// method will throw an <c>InvalidOperationException</c>. After you have 
        /// obtained the NetworkStream, call the <see cref="M:System.Net.Sockets.NetworkStream.Write"/>
        /// method to send data to the remote host. Call the <see cref="M:System.Net.Sockets.NetworkStream.Read"/>
        /// method to receive data arriving from the remote host. Both of these
        /// methods block until the specified operation is performed. You can avoid
        /// blocking on a read operation by checking the <see cref="P:System.Net.Sockets.NetworkStream.DataAvailable"/>
        /// property. A <c>true</c> value means that data has arrived from the remote
        /// host and is available for reading. In this case, <c>Read</c> is guaranteed
        /// to complete immediately. If the remote host has shutdown its connection,
        /// <c>Read</c> will immediately return with zero bytes.
        /// </para>
        /// </remarks>
        /// --
        /// <exception cref="T:System.InvalidOperationException">
        /// The <c>TransportConnection</c> instance is not connected to a remote host.
        /// </exception>
        public Stream PeerStream
        {
            get
            {
                if (!m_connectedTransport)
                    throw new InvalidOperationException("Not connected.");
                if (m_peerStream == null) {
                    System.Diagnostics.Debug.Assert(m_socket != null);
                    System.Diagnostics.Debug.Assert(m_disposableClient == null);
                    m_peerStream = new NetworkStream(m_socket, true);
                }
                return m_peerStream;
            }
        }

        /// <summary>
        /// Gets the underlying <see cref="T:System.Net.Sockets.Socket"/>.
        /// </summary>
        /// <remarks>
        /// In many cases the instance will represent a upper-layer connection, 
        /// most often OBEX, in that case the upper-layer connection feature should
        /// be used instead of the <c>Socket</c> directly.
        /// </remarks>
        public Socket Client
        {
            get { return m_socket; }
        }

        /// <summary>
        /// Gets the remote address to which that the user requested a connection.
        /// </summary>
        public String RequestedRemoteAddress
        {
            get { return m_addrString; }
        }

        //--------------------------------------------------------------
        //--------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:Brecham.Obex.Net.TransportConnection"/> class. 
        /// </summary>
        protected TransportConnection()
        { }

        //--------------------------------------------------------------
        //--------------------------------------------------------------
        /// <summary>
        /// Returns one of "obex-push", "obex-ftp", etc.
        /// </summary>
        /// <returns></returns>
        protected abstract String ChooseService();

        /// <summary>
        /// Which L3 protocol
        /// </summary>
        /// <returns></returns>
        protected abstract ProtocolFamily ChooseProtocol();

        /// <summary>
        /// Returns the address of the peer device to connect to.  Is returned as a
        /// <c>String</c> since there&#x2019;s no <c>NetworkAddress</c> base class.
        /// </summary>
        /// <returns></returns>
        protected abstract String ChoosePeer(ProtocolFamily proto);

        /// <summary>
        /// Called to display status text to the user.  For a GUI application this
        /// would likely be overridden to set the status bar text, for a console
        /// application it would likely be overridden to simply Console.WriteLine
        /// the message.
        /// </summary>
        /// <param name="message">The status message to be displayed to the user.</param>
        protected abstract void ShowStatus(String message);

        //--------------------------------------------------------------
        //--------------------------------------------------------------

        /// <summary>
        /// Gets a <see cref="T:System.String"/> holding the transport connection identifier
        /// for the service
        /// being used for this connection.  Used with IrDA for instance.
        /// </summary>
        /// <param name="proto">
        /// The <see cref="T:System.Net.Sockets.ProtocolFamily"/> 
        /// being used for this connection.
        /// </param>
        /// <param name="service">
        /// A <see cref="T:System.String"/> identifying the service
        /// being used for this connection.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.String"/> holding the transport connection identifier
        /// for the service
        /// being used for this connection.
        /// </returns>
        protected abstract String GetServiceString(ProtocolFamily proto, String service);

        /// <summary>
        /// Gets a <see cref="T:System.Guid"/> holding the transport connection identifier
        /// for the service
        /// being used for this connection.  Used by Bluetooth for instance.
        /// </summary>
        /// <param name="proto">
        /// The <see cref="System.Net.Sockets.ProtocolFamily"/> 
        /// being used for this connection.
        /// </param>
        /// <param name="service">
        /// A <see cref="T:System.String"/> identifying the service
        /// being used for this connection.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Guid"/> holding the transport connection identifier
        /// for the service
        /// being used for this connection.
        /// </returns>
        protected abstract Guid GetServiceGuid(ProtocolFamily proto, String service);

        /// <summary>
        /// Gets a <see cref="T:System.Int32"/> holding the transport connection identifier
        /// for the service
        /// being used for this connection. Used by TCP/IP for instance.
        /// </summary>
        /// <param name="proto">
        /// The <see cref="System.Net.Sockets.ProtocolFamily"/> 
        /// being used for this connection.
        /// </param>
        /// <param name="service">
        /// A <see cref="T:System.String"/> identifying the service
        /// being used for this connection.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Int32"/> holding the transport connection identifier
        /// for the service
        /// being used for this connection.
        /// </returns>
        protected abstract int GetServiceInt32(ProtocolFamily proto, String service);

        //--------------------------------------------------------------
        //--------------------------------------------------------------

        /// <summary>
        /// Create an <see cref="System.Net.EndPoint"/> from the specified values.
        /// </summary>
        /// <remarks>
        /// Override if support for a new protocol is being added.
        /// </remarks>
        // <param name="protocol"></param>
        // <param name="addressString"></param>
        // <param name="service"></param>
        // <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "EndPoint")]
        protected virtual EndPoint CreateEndPoint(ProtocolFamily protocol, String addressAsText, String service)
        {
            EndPoint ep;
            switch (protocol) {
                case ProtocolFamily.Irda:
                    IrDAAddress addrIrda = IrDAAddress.Parse(addressAsText);
                    ep = new IrDAEndPoint(addrIrda, GetServiceString(protocol, service));
                    break;
                case ProtocolFamily_Bluetooth:
                    BluetoothAddress addrBt = BluetoothAddress.Parse(addressAsText);
                    ep = new BluetoothEndPoint(addrBt, GetServiceGuid(protocol, service));
                    break;
                case ProtocolFamily.InterNetwork:
                    IPAddress addrIp = IPAddress.Parse(addressAsText);
                    ep = new IPEndPoint(addrIp, GetServiceInt32(protocol, service));
                    break;
                default:
                    throw new ArgumentException("Unhandled protocol.");
            }//switch
            return ep;
        }

        /// <summary>
        /// Create an <see cref="System.Net.Sockets.Socket"/> from the specified values.
        /// </summary>
        /// <remarks>
        /// Override if support for a new protocol is being added.
        /// </remarks>
        // <param name="protocol">
        // The <see cref="T:System.Net.Socket.ProtocolFamily"/> being used.
        // </param>
        // <returns>
        // </returns>
        protected  virtual Socket CreateSocket(ProtocolFamily protocol)
        {
            Socket sock;
            AddressFamily af = (AddressFamily)protocol;
            switch (protocol) {
                case ProtocolFamily.Irda:
                    sock = new Socket(af, SocketType.Stream, ProtocolType.Unspecified);
                    break;
                case ProtocolFamily_Bluetooth:
                    sock = new Socket(af, SocketType.Stream, (ProtocolType)3);
                    break;
                case ProtocolFamily.InterNetwork:
                    sock = new Socket(af, SocketType.Stream, ProtocolType.Unspecified);
                    break;
                default:
                    throw new ArgumentException("Unhandled protocol.");
            }//switch
            return sock;
        }

        /// <summary>
        /// Connects to the configured transport layer endpoint.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the connection was carried out, false if not.  Exceptions
        /// are thrown if the connection failed.
        /// </returns>
        protected virtual bool ConnectTransport()
        {
            ProtocolFamily proto = ChooseProtocol();
            if (proto == ProtocolFamily.Unspecified) {
                return false;
            }
            String addrString = ChoosePeer(proto);
            if (StringUtilities.IsNullOrEmpty(addrString)) {
                return false;
            }
            if (addrString == null) {
                ShowStatus("No peer device chosen.");
                return false;
            }
            m_addrString = addrString;
            m_service = ChooseService();
            if (StringUtilities.IsNullOrEmpty(m_service)) {
                return false;
            }
            //
            if (proto == ProtocolFamily.InterNetwork) {
                // We want to be able to use hostnames in TCP/IP so we will use TcpClient
                // there, as doing our own hostname lookup, address selection etc would
                // be crazy.
                int port = GetServiceInt32(proto, m_service);
                ShowStatus("Connecting...");
                TcpClient cli;
                try {
                    // NETCFv1's TcpClient seems not to like the hostname string
                    // to contain an IP Address.  So try a direct parse.  In general,
                    // also saves a possible(?) DNS lookup if it is an address.
                    IPAddress ipAddr = IPAddress.Parse(addrString);
                    cli = new TcpClient(
#if ! (NETCF && FX1_1)
                        // To support IPv6 etc, we must tell TcpClient as initialisation
                        // what address family it's going to be used for.
                        ipAddr.AddressFamily
#endif
                        );
                    cli.Connect(ipAddr, port);
                } catch (FormatException) {
                    // Apparently not a literal IP Address so do it the old way (incl DNS).
                    cli = new TcpClient(addrString, port);
                }
                m_disposableClient = cli;
                // Ideally we'd set the m_socket value here, but in FX1_1 the TcpClient
                // has NO Client property. Thus no:
                // m_socket = cli.Client;
                // Have to create NetworkStream here as we have no access to the 
                // TcpClient by type later.
                m_peerStream = cli.GetStream();
            } else {
                m_socket = CreateSocket(proto);
                EndPoint ep = CreateEndPoint(proto, addrString, m_service);
                ShowStatus("Connecting...");
                m_socket.Connect(ep);
            }
            System.Diagnostics.Debug.Assert(m_socket != null || m_disposableClient != null);
            System.Diagnostics.Debug.Assert(m_disposableClient == null || m_peerStream != null);
            m_connectedTransport = true;
            ShowStatus("Connected.");
            return true;
        }

        /// <summary>
        /// Connects to the configured transport layer endpoint, and optionally service.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the connection was carried out, false if not.  Exceptions
        /// are thrown if the connection failed.
        /// </returns>
        public bool Connect()
        {
            bool didConnect = ConnectCore();
            m_didConnect = didConnect;
            return didConnect;
        }

        /// <summary>
        /// Connects to the configured transport layer endpoint.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the connection was carried out, false if not.  Exceptions
        /// are thrown if the connection failed.
        /// </returns>
        protected virtual bool ConnectCore()
        {
            return ConnectTransport();
        }

        //--------------------------------------------------------------
        //--------------------------------------------------------------

        /// <summary>
        /// Closes any connections and releases all resources used by the 
        /// <see cref="T:Brecham.Obex.Net.TransportConnection"/>.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        #region IDisposable Members

        /// <summary>
        /// Closes any connections and releases all resources used by the 
        /// <see cref="T:Brecham.Obex.Net.TransportConnection"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <exclude/>
        protected virtual void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.Assert(disposing);
            m_didConnect = false;
            m_connectedTransport = false;
            if (disposing) {
                if (m_peerStream != null) { m_peerStream.Close(); }
                if (m_socket != null) { m_socket.Close(); }
                if (m_disposableClient != null) { m_disposableClient.Dispose(); }
                m_peerStream = null;
            }//if
        }

        #endregion
    }//class--TransportConnection


    /// <summary>
    /// Adds OBEX connection features to the class, i.e. knows the IrDA 
    /// &#x201C;Service Name&#x201D;, Bluetooth UUID etc for the OBEX service being used.
    /// </summary>
    public abstract class ObexTransportConnection : TransportConnection
    {
        //--------------------------------------------------------------

        /// <summary>
        /// The Service identifier for the OBEX default Inbox service.
        /// </summary>
        public const String ObexDefaultService = "obex";

        /// <summary>
        /// The Service identifier for the OBEX 'push' service.
        /// </summary>
        public const String ObexPushService = "obex-push";

        /// <summary>
        /// The Service identifier for the OBEX Folder-Browsing service.
        /// </summary>
        public const String ObexFtpService = "obex-ftp";

        // The list of support scheme/service strings.
        /// <exclude/>
        protected static readonly String[] s_schemes = { ObexDefaultService, ObexPushService, ObexFtpService  /*, "obex-sync"*/ };

        //--------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:Brecham.Obex.Net.ObexTransportConnection"/> class. 
        /// </summary>
        protected ObexTransportConnection() 
            : base() 
        { }

        //--------------------------------------------------------------

        /// <summary>
        /// Gets a <see cref="T:System.String"/> holding the transport connection identifier
        /// for the service
        /// being used for this connection.  Used with IrDA for instance.
        /// </summary>
        /// <remarks>
        /// Supports IrDA and returns Service Name "OBEX" for all OBEX services.
        /// </remarks>
        protected override string GetServiceString(ProtocolFamily proto, String service)
        {
            if (proto == ProtocolFamily.Irda) {
                return "OBEX";  // For every standard OBEX service/application.
            } else {
                throw new ArgumentException("Service Names only returned for IrDA.");
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Guid"/> holding the transport connection identifier
        /// for the service
        /// being used for this connection.  Used by Bluetooth for instance.
        /// </summary>
        /// <remarks>
        /// Supports Bluetooth, and currently returns
        /// <see cref="F:InTheHand.Net.Bluetooth.BluetoothService.ObexObjectPush"/>
        /// for &#x2018;OBEX Push&#x2019;, and
        /// <see cref="F:InTheHand.Net.Bluetooth.BluetoothService.ObexFileTransfer"/>
        /// for &#x2018;OBEX Ftp&#x2019;.
        /// </remarks>
        protected override Guid GetServiceGuid(ProtocolFamily proto, String service)
        {
            if (proto == TransportConnection.ProtocolFamily_Bluetooth) {
                switch (service) {
                    case ObexDefaultService:
                    case ObexPushService:
                        return InTheHand.Net.Bluetooth.BluetoothService.ObexObjectPush;
                    case ObexFtpService: return InTheHand.Net.Bluetooth.BluetoothService.ObexFileTransfer;
                    default: throw new ArgumentException("Unknown service type.", "service");
                }//switch

            } else {
                    throw new ArgumentException("Service GUIDs only returned for Bluetooth.");
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Int32"/> holding the transport connection identifier
        /// for the service
        /// being used for this connection. Used by TCP/IP for instance.
        /// </summary>
        /// <remarks>
        /// Supports TCP/IP and returns port number 650, the default for all OBEX services. 
        /// </remarks>
        protected override int GetServiceInt32(ProtocolFamily proto, String service)
        {
            if (proto == ProtocolFamily.InterNetwork) {
                return ObexConstant.TcpPortNumber;
            } else {
                throw new ArgumentException("Server port numbers only returned for TCP/IP.");
            }
        }
    }//class--ObexTransportConnection 


    /// <summary>
    /// Adds OBEX server connection features to the class, i.e. knows the Target header
    /// for the service being used, and provides access to the connected 
    /// <see cref="T:Brecham.Obex.ObexClientSession"/>.
    /// </summary>
    public abstract class ObexSessionConnection : ObexTransportConnection
    {
        //--------------------------------------------------------------
        private int m_mru = UInt16.MaxValue;
        bool m_connectedSession;
        ObexClientSession m_sess;
        int m_mtuToSet;

        //--------------------------------------------------------------

        /// <summary>
        /// Gets or set the size of buffer that the ObexClientSession will be instantiated with.
        /// That is, as passed to its constructor&#x2019;s <c>bufferSize</c> parameter.
        /// </summary>
        public int ObexBufferSize
        {
            get { return m_mru; }
            set { m_mru = value; }
        }

        /// <summary>
        /// Set the size to restrict our sends to.
        /// </summary>
        /// <remarks>
        /// If we are for instance doing a PUT to a peer with a very large receive
        /// buffer size then we could be creating 64KB sized packets. Setting this
        /// values restricts the maximum size send packets we will create.
        /// This sets the <see cref="P:Brecham.Obex.Pdus.ObexPduFactory.Mtu"/> 
        /// property on <c>Brecham.Obex.Pdus.ObexPduFactory</c>.
        /// </remarks>
        public int MaxSendSize
        {
            set
            {
                if (m_sess != null) {
                    m_sess.PduFactory.Mtu = value;
                } else {
                    // Not connected yet, so store the value to be used on connected.
                    m_mtuToSet = value;
                }
            }
        }

        /// <summary>
        /// Gets the connected <see cref="T:Brecham.Obex.ObexClientSession"/>.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        /// The <see cref="T:Brecham.Obex.Net.ObexSessionConnection"/> is not connected.
        /// </exception>
        public ObexClientSession ObexClientSession
        {
            get
            {
                if (!m_connectedSession)
                    throw new InvalidOperationException("Not connected.");
                return m_sess;
            }
        }

        //--------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:Brecham.Obex.Net.ObexSessionConnection"/> class. 
        /// </summary>
        protected ObexSessionConnection()
            : base()
        { }

        //--------------------------------------------------------------

        /// <summary>
        /// Connects to the OBEX server after forming the transport layer connection.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the connection was carried out, false if not.  Exceptions
        /// are thrown if the connection failed.
        /// </returns>
        protected override bool ConnectCore()
        {
            if (!base.ConnectCore()) {
                return false;
            }
            //
            ShowStatus("OBEX Connecting...");
            try {
                m_sess = new ObexClientSession(PeerStream, m_mru);
                if (m_mtuToSet != 0) {
                    m_sess.PduFactory.Mtu = m_mtuToSet;
                }
                byte[] target = GetObexTarget(m_service);
                if (target == null) {
                    m_sess.Connect();
                } else {
                    m_sess.Connect(target);
                }
                ShowStatus("OBEX Connected.");
                m_connectedSession = true;
            } catch (Exception ex) {
                try {
                    ShowStatus("OBEX Connect failed: " + ex.Message + ".");
                } finally {
                    this.Dispose();
                }
                throw;
            }
            return true;
        }

        /// <summary>
        /// Gets the OBEX Target value for the OBEX Service/Application being used.
        /// </summary>
        /// <param name="service">
        /// A <see cref="T:System.String"/> representing the OBEX Service/Application being used.
        /// </param>
        /// <returns>
        /// A byte array containing the value to be used in the Target header.
        /// </returns>
        protected virtual byte[] GetObexTarget(String service)
        {
            byte[]target;
            switch (service) {
                case ObexDefaultService:
                case ObexPushService:
                    target = null;
                    break;
                case ObexFtpService:
                    target = Brecham.Obex.ObexConstant.Target.FolderBrowsing;
                    break;
                default:
                    throw new ArgumentException("Unhandled service type >" + service + "<.");
            }
            return target;
        }
    }//class

    /// <summary>
    /// Creates a connection to an OBEX Server prompting the user for its details
    /// on the GUI, for instance using BluetoothSelectDialog.
    /// </summary>
    public class GuiObexSessionConnection : ObexSessionConnection
    {
        //--------------------------------------------------------------
        ProtocolFamily m_protocolFamily;    //bool m_toBluetooth;
        bool m_fbService;
        System.Windows.Forms.Control m_statusControl;
#if ! NETCF
        System.Windows.Forms.ToolStripItem m_toolStripItem;
#endif // ! NETCF

        //--------------------------------------------------------------
        /// <exclude/>
        [Obsolete("Use one of the overloads that takes ProtocolFamily at the first parameter.")]
        public GuiObexSessionConnection(bool toBluetooth, bool fbService, 
            System.Windows.Forms.Control statusControl)
            :this(toBluetooth, fbService)
        {
            m_statusControl = statusControl;
        }

#if ! NETCF
        /// <exclude/>
        [Obsolete("Use one of the overloads that takes ProtocolFamily at the first parameter.")]
        public GuiObexSessionConnection(bool toBluetooth, bool fbService,
            System.Windows.Forms.ToolStripItem toolStripItem)
            : this(toBluetooth, fbService)
        {
            m_toolStripItem = toolStripItem;
        }
#endif // ! NETCF

        /// <exclude/>
        [Obsolete("Use one of the overloads that takes ProtocolFamily at the first parameter.")]
        public GuiObexSessionConnection(bool toBluetooth, bool fbService)
            : this(toBluetooth ? ProtocolFamily_Bluetooth : ProtocolFamily.Irda, fbService)
        { }

        //--------

        /// <summary>
        /// Initializes a new instance of <see cref="T:Brecham.Obex.Net.GuiObexSessionConnection"/>
        /// to connect using the specified protocol, and optionally to the 
        /// Folder-Browsing service.
        /// Displays status messages on the specified Forms <see cref="T:System.Windows.Forms.Control"/>.
        /// </summary>
        /// <param name="protocol">
        /// A <see cref="T:System.Net.Sockets.ProtocolFamily"/> specifying the protocol
        /// to use to connect, for instance <see cref="F:System.Net.Sockets.ProtocolFamily.Irda"/>.
        /// </param>
        /// <param name="fbService">
        /// Whether to connect to the OBEX Folder-Browsing service, if <c>false</c>
        /// the connection will be to the default Inbox service.
        /// </param>
        /// <param name="statusControl">
        /// The Forms <see cref="T:System.Windows.Forms.Control"/> where status messages
        /// are to be displayed.
        /// </param>
        /// <example>
        /// <code lang="Visual Basic">
        /// Class Form1
        ///    Friend WithEvents Label1 As System.Windows.Forms.Label
        ///    Friend WithEvents ProtocolComboBox1 As Brecham.Obex.Net.Forms.ProtocolComboBox
        ///    ...
        /// 
        ///    Private Sub ButtonConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles connectButton.Click
        ///       ' Connect
        ///       Dim pf As System.Net.Sockets.ProtocolFamily = Me.ProtocolComboBox1.SelectedProtocol
        ///       MyBase.UseWaitCursor = True
        ///       Me.m_conn = New GuiObexSessionConnection(pf, True, Me.Label1)
        ///       Try
        ///          Dim connected As Boolean = Me.m_conn.Connect
        ///       ...
        ///    End Sub
        /// End Class
        /// </code></example>
        public GuiObexSessionConnection(ProtocolFamily protocol, bool fbService,
            System.Windows.Forms.Control statusControl)
            : this(protocol, fbService)
        {
            m_statusControl = statusControl;
        }

#if ! NETCF
        /// <summary>
        /// Initializes a new instance of <see cref="T:Brecham.Obex.Net.GuiObexSessionConnection"/>
        /// to connect using the specified protocol, and optionally to the 
        /// Folder-Browsing service.
        /// Displays status messages on the specified Forms <see cref="T:System.Windows.Forms.ToolStripItem"/>.
        /// </summary>
        /// <param name="protocol">
        /// A <see cref="T:System.Net.Sockets.ProtocolFamily"/> specifying the protocol
        /// to use to connect, for instance <see cref="F:System.Net.Sockets.ProtocolFamily.Irda"/>.
        /// </param>
        /// <param name="fbService">
        /// Whether to connect to the OBEX Folder-Browsing service, if <c>false</c>
        /// the connection will be to the default Inbox service.
        /// </param>
        /// <param name="toolStripItem">
        /// The Forms <see cref="T:System.Windows.Forms.ToolStripItem"/> where status messages
        /// are to be displayed.
        /// </param>
        /// <example>
        /// <code lang="Visual Basic">
        /// Class Form1
        ///    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
        ///    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
        ///    Friend WithEvents ProtocolComboBox1 As Brecham.Obex.Net.Forms.ProtocolComboBox
        ///    ...
        /// 
        ///    Private Sub ButtonConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles connectButton.Click
        ///       ' Connect
        ///       Dim pf As System.Net.Sockets.ProtocolFamily = Me.ProtocolComboBox1.SelectedProtocol
        ///       MyBase.UseWaitCursor = True
        ///       Me.m_conn = New GuiObexSessionConnection(pf, True, Me.ToolStripStatusLabel1)
        ///       Try
        ///          Dim connected As Boolean = Me.m_conn.Connect
        ///       ...
        ///    End Sub
        /// End Class
        /// </code></example>
        public GuiObexSessionConnection(ProtocolFamily protocol, bool fbService,
            System.Windows.Forms.ToolStripItem toolStripItem)
            : this(protocol, fbService)
        {
            m_toolStripItem = toolStripItem;
        }
#endif // ! NETCF

        /// <overloads>
        /// Initializes a new instance of <see cref="T:Brecham.Obex.Net.GuiObexSessionConnection"/>.
        /// </overloads>
        /// ----
        /// <summary>
        /// Initializes a new instance of <see cref="T:Brecham.Obex.Net.GuiObexSessionConnection"/>
        /// to connect using the specified protocol, and optionally to the 
        /// Folder-Browsing service.
        /// </summary>
        /// <param name="protocol">
        /// A <see cref="T:System.Net.Sockets.ProtocolFamily"/> specifying the protocol
        /// to use to connect, for instance <see cref="F:System.Net.Sockets.ProtocolFamily.Irda"/>.
        /// </param>
        /// <param name="fbService">
        /// Whether to connect to the OBEX Folder-Browsing service, if <c>false</c>
        /// the connection will be to the default Inbox service.
        /// </param>
        public GuiObexSessionConnection(ProtocolFamily protocol, bool fbService)
        {
            if (protocol != ProtocolFamily_Bluetooth && protocol != ProtocolFamily.Irda
                && protocol != ProtocolFamily.InterNetwork) {
                throw ExceptionFactory.ArgumentOutOfRangeException("protocol", "Only support Bluetooth, IrDA, and TCP/IP.");
            }
            m_protocolFamily = protocol;
            m_fbService = fbService;
        }

        //--------------------------------------------------------------

        /// <exclude/>
        protected override void ShowStatus(string message)
        {
            if (m_statusControl != null) {
                m_statusControl.Text = message;
            }
#if ! NETCF
            if (m_toolStripItem != null) {
                m_toolStripItem.Text = message;
            }
#endif // ! NETCF
        }

        /// <exclude/>
        protected override string ChooseService()
        {
            if (m_fbService) {
                return ObexFtpService;
            } else {
                return ObexPushService;
            }
        }

        /// <exclude/>
        protected override ProtocolFamily ChooseProtocol()
        {
            //if (ForceTcp) {
            //    return ProtocolFamily.InterNetwork;
            //}
            return m_protocolFamily;
        }

        /// <exclude/>
        protected override string ChoosePeer(ProtocolFamily proto)
        {
            if (proto == ProtocolFamily_Bluetooth) {
                return ChoosePeerBluetooth();
            } else if (proto == ProtocolFamily.Irda) {
                return ChoosePeerIrda();
            } else if (proto == ProtocolFamily.InterNetwork) {
                return ChoosePeerTcpip();
            } else {
                throw new ArgumentException("Unhandled protocol.");
            }
        }

        private static string ChoosePeerTcpip()
        {
            String addr = "192.168.2.3";
            //
            Brecham.Obex.Net.Forms.TcpipAddressDialog form
                = new Brecham.Obex.Net.Forms.TcpipAddressDialog();
            form.AddressOrHostName = addr;
            // Per MSDN: "the currently active window is made the owner",
            // so no need to do so explicitly.  Which make life easier
            // as we support NETCF with this one call too.
            System.Windows.Forms.DialogResult rslt = form.ShowDialog();
            if (rslt == System.Windows.Forms.DialogResult.OK) {
                addr = form.AddressOrHostName;
                return addr;
            } else {
                return null;
            }
        }

        private string ChoosePeerIrda()
        {
            IrDAClient cli = new IrDAClient();
            IrDADeviceInfo[] devs = cli.DiscoverDevices(1);
            if (devs.Length >= 1) {
                String addrAsString = devs[0].DeviceAddress.ToString();
                return addrAsString;
            } else {
                ShowStatus("No peer IrDA devices found.");
                return null;
            }
        }

        private string ChoosePeerBluetooth()
        {
            // Bluetooth
            InTheHand.Windows.Forms.SelectBluetoothDeviceDialog dialog
                = new InTheHand.Windows.Forms.SelectBluetoothDeviceDialog();
            // TODO (((PutGUI--SelectBluetoothDeviceDialog() filtering...?)))
            dialog.ShowUnknown = true;//HACK showNewDevices;
            //dialog.ShowUnknown = false;
            //dialog.ShowAuthenticated = false;
            System.Windows.Forms.DialogResult rslt = dialog.ShowDialog();
            if (rslt != System.Windows.Forms.DialogResult.OK) {
                ShowStatus("Select Bluetooth device cancelled [" + rslt + "].");
                return null;
            }
            BluetoothDeviceInfo device = dialog.SelectedDevice;
            if (device == null) {
                //Does happen is none selected, 
                ShowStatus("No Bluetooth device selected.");
                return null;
            }
            String addr = device.DeviceAddress.ToString();
            return addr;
        }

    }//class GuiObexSessionConnection 


#if ! (NETCF && FX1_1)
    /// <summary>
    /// Connects to an Obex Server prompting the user for the details of the peer on
    /// the console, using a simple menu-based format.
    /// </summary>
    /// <remarks>
    /// A sample use would be as the following:
    /// <code>
    /// Connect to a Bluetooth, IrDA, or TCP peer [BIT]>b
    /// Bluetooth discovery...
    ///  0: Andy Hume, last seen: 08/10/2006 21:41:08 [InTheHand.Net.Sockets.BluetoothDeviceInfo]
    ///  1: AndyHumeE2, last seen: 08/10/2006 21:41:08 [InTheHand.Net.Sockets.BluetoothDeviceInfo]
    ///  2: ÁSBJÖRG, last seen: 08/10/2006 21:41:08 [InTheHand.Net.Sockets.BluetoothDeviceInfo]
    /// Choose which device by number>2
    /// Connect to the Folder-Browsing server, select No for the default inbox [Y/n]>
    /// Will connect to the Folder-Browsing Target.
    /// Connecting...
    /// Connected.
    /// OBEX Connecting...
    /// OBEX Connected.
    /// </code>
    /// Similar prompts are made if one of the other protocols is selected.
    /// <para>
    /// Further to the exceptions that can occur from the base classes on calling
    /// <see cref="M:Brecham.Obex.Net.TransportConnection.Connect"/>, this class can
    /// also throw <see cref="T:System.IO.EndOfStreamException"/> if the console
    /// input is closed whilst waiting for a response to one of the menu prompts.
    /// </para>
    /// </remarks>
    public class ConsoleMenuObexSessionConnection : ObexSessionConnection
    {
        //--------------------------------------------------------------
        /// <summary>
        /// Initialize a new instance of ConsoleMenuObexSessionConnection
        /// </summary>
        public ConsoleMenuObexSessionConnection()
            : base()
        { }


        //--------------------------------------------------------------

        // Should accept a list of which services to offer in the constructor and build
        // a menu here based on that list.  Here we're assuming the list has 
        /// <exclude/>
        protected override string ChooseService()
        {
            // Check what application to connect to, we assume Folder Browsing.
            Console.Write("Connect to the Folder-Browsing server, select No for the default inbox");
            bool fbService = MenuKeyboardInput.GetCmdLineIsYesAssumeYes();
            //
            if (fbService) {
                Console.WriteLine("Will connect to the Folder-Browsing Target.");
                return ObexFtpService;
            } else {
                Console.WriteLine("Will connect to the default Inbox Target.");
                return ObexPushService;
            }
        }

        /// <exclude/>
        protected override ProtocolFamily ChooseProtocol()
        {
            // Check what protocol to connect over.
            Console.Write("Connect to a Bluetooth, IrDA, or TCP peer");
#if ! ( FX1_1 || NETCF )
            Console.Beep();
#endif
            char protocol = MenuKeyboardInput.GetCharacterInSetUppercase("BIT");
            //
            if (protocol == 'B') {
                return ProtocolFamily_Bluetooth;
            } else if (protocol == 'I') {
                return ProtocolFamily.Irda;
            } else if (protocol == 'T') {
                return ProtocolFamily.InterNetwork;
            } else {
                System.Diagnostics.Debug.Fail("Illegal character.");
                return ProtocolFamily.Unspecified;
            }
        }

        /// <exclude/>
        protected override string ChoosePeer(ProtocolFamily proto)
        {
            if (proto == ProtocolFamily_Bluetooth) {
                return ChoosePeerBluetooth();
            } else if (proto == ProtocolFamily.Irda) {
                return ChoosePeerIrda();
            } else if (proto == ProtocolFamily.InterNetwork) {
                return ChoosePeerTcpip();
            } else {
                throw new ArgumentException("Unhandled protocol.");
            }
        }

        private string ChoosePeerIrda()
        {
            IrDAClient irdaCli = null;
            try {
                irdaCli = new IrDAClient();
                IrDADeviceInfo[] devices;
                while (true) {
                    Console.WriteLine("IrDA discovery...");
                    devices = irdaCli.DiscoverDevices();
                    for (int i = 0; i < devices.Length; ++i) {
                        IrDADeviceInfo curDev = devices[i];
                        Console.WriteLine("{0,2}: {1} [{3}]",//, last seen: {2}",
                            i, curDev.DeviceName, null//curDev.LastSeen
                            , curDev
                            );
                    }//for
#if ! ( FX1_1 || NETCF )
                    Console.Beep();
#endif
                    if (devices.Length != 0) break;
                    Console.Write("No devices discovered, hit return to try again" + MenuKeyboardInput.Prompt);
                    MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.Allow);
                }//while
                Console.Write("Choose which device by number");
                int selected = MenuKeyboardInput.GetIndexOfRange(devices.Length);
                IrDAAddress addr = devices[selected].DeviceAddress;
                String addrAsString = addr.ToString();
                return addrAsString;
            } finally {
                if (irdaCli != null) { irdaCli.Close(); }
            }
        }

        private string ChoosePeerBluetooth()
        {
            BluetoothClient btCli = new BluetoothClient();
            BluetoothDeviceInfo[] devices;
            while (true) {
                Console.WriteLine("Bluetooth discovery...");
                devices = btCli.DiscoverDevices();
                for (int i = 0; i < devices.Length; ++i) {
                    BluetoothDeviceInfo curDev = devices[i];
                    Console.WriteLine("{0,2}: {1}, last seen: {2} [{3}]",
                        i, curDev.DeviceName, curDev.LastSeen, curDev);
                }//for
#if ! ( FX1_1 || NETCF )
                Console.Beep();
#endif
                if (devices.Length != 0) break;
                Console.Write("No devices discovered, hit return to try again" + MenuKeyboardInput.Prompt);
                MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.Allow);
            }//while
            Console.Write("Choose which device by number");
            int selected = MenuKeyboardInput.GetIndexOfRange(devices.Length);
            BluetoothAddress addr = devices[selected].DeviceAddress;
            return addr.ToString();
        }

        private string ChoosePeerTcpip()
        {
            String result;
            Console.Write("Please enter the hostname or IP Address of the server" + MenuKeyboardInput.Prompt);
            while (true) {
                String line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.DoNotAllow);
                // Success
                result = line;
                break;
            }
#if DEBUG
            Console.WriteLine("[[Will connect to: " + result + "]]");
#endif
            return result;
        }

        //--------------------------------------------------------------
        /// <exclude/>
        protected override void ShowStatus(string message)
        {
            Console.WriteLine(message);
        }
    }//class--ConsoleMenuObexConnection 
#endif


    /// <summary>
    /// Connects to an Obex Server using the details of the peer as supplied in the
    /// specified URL.
    /// </summary>
    public class UriObexSessionConnection : ObexSessionConnection
    {
        //--------------------------------------------------------------
        Uri m_uri;
        ProtocolFamily m_protoFromUri;
        String m_addressFromUri;

        //--------------------------------------------------------------

        /// <overloads>
        /// Initialize a new <see cref="T:UriObexSessionConnection"/> to connect to
        /// the specified OBEX Server.
        /// </overloads>
        /// -
        /// <summary>
        /// Initialize a new <see cref="T:UriObexSessionConnection"/> to connect to
        /// the OBEX Server as specified in the given URL.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>The scheme part of the <see cref="T:System.Uri"/> should be one
        /// of the following: <c>obex</c>, <c>obex-push</c>, or <c>obex-ftp</c>.
        /// </para>
        /// <para>The host part should be the network address of the OBEX server, 
        /// for Bluetooth it MAC Address, and for IrDA the four byte address.  They
        /// should formatted to suit the respective <c>Parse</c> method (on 
        /// <c>BluetoothAddress</c> and <c>IrDAAddress</c> respectively).
        /// </para>
        /// <para>For example, with Bluetooth and IrDA addresses respectively:
        /// <code>
        /// obex-ftp://010203040506/
        /// obex-push://01020304/
        /// </code>
        /// </para>
        /// </remarks>
        /// -
        /// <param name="uri">
        /// An <see cref="T:System.Uri"/> configured with the details of the server
        /// to connect to.
        /// </param>
        public UriObexSessionConnection(Uri uri)
        {
            m_uri = uri;
            VerifyAndDisectUri();
        }//.ctor

        /// <summary>
        /// Initialize a new <see cref="T:UriObexSessionConnection"/> to connect to
        /// the OBEX Server as specified in the device address string.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>For example, with Bluetooth and IrDA addresses respectively:
        /// <code>
        /// new UriObexSessionConnection("obex-ftp", "010203040506");
        /// new UriObexSessionConnection("obex-push", "01020304");
        /// </code>
        /// </para>
        /// </remarks>
        /// -
        /// <param name="scheme">One
        /// of the following: <c>obex</c>, <c>obex-push</c>, or <c>obex-ftp</c>.
        /// </param>
        /// <param name="deviceAddress">The network address of the OBEX server, 
        /// for Bluetooth it MAC Address, and for IrDA the four byte address.  They
        /// should formatted to suit the respective <c>Parse</c> method (on 
        /// <c>BluetoothAddress</c> and <c>IrDAAddress</c> respectively).
        /// </param>
        public UriObexSessionConnection(String scheme, String deviceAddress)
            : this(new Uri(scheme + "://" + deviceAddress))
        { }

        /// <summary>
        /// Initialize a new <see cref="T:UriObexSessionConnection"/> to connect to
        /// the OBEX Server as specified in the Bluetooth device address.
        /// </summary>
        /// -
        /// <param name="scheme">One
        /// of the following: <c>obex</c>, <c>obex-push</c>, or <c>obex-ftp</c>.
        /// </param>
        /// <param name="deviceAddress">The network address of the OBEX server, 
        /// as a <c>BluetoothAddress</c>.
        /// </param>
        public UriObexSessionConnection(String scheme, BluetoothAddress deviceAddress)
            : this(scheme, deviceAddress.ToString())
        { }

        /// <summary>
        /// Initialize a new <see cref="T:UriObexSessionConnection"/> to connect to
        /// the OBEX Server as specified in the IrDA device address.
        /// </summary>
        /// -
        /// <param name="scheme">One
        /// of the following: <c>obex</c>, <c>obex-push</c>, or <c>obex-ftp</c>.
        /// </param>
        /// <param name="deviceAddress">The network address of the OBEX server, 
        /// as a <c>IrDAAddress</c>.
        /// </param>
        public UriObexSessionConnection(String scheme, IrDAAddress deviceAddress)
            : this(scheme, deviceAddress.ToString())
        { }

        //--------------------------------------------------------------
        void VerifyUri(Uri peer)
        {
            //-------------
            // Scheme
            //-------------
            String scheme = peer.Scheme;
            foreach (String curScheme in s_schemes) {
                if (curScheme.Equals(scheme))
                    goto GoodScheme;
            }
            throw new ArgumentException(ExMsgUriBadScheme);
        GoodScheme:

            //-------------
            // Path and Query
            //-------------
            // A possible user mistake is to do >new Uri("obex:/" + addr +"/")<
            // so creating a uri of obex:/11223344/  Oops, no host part!  So check
            // this first to give a better error message.
            if (StringUtilities.IsNullOrEmpty(peer.Host))
                throw new ArgumentException(ExMsgUriHostIsEmpty);
            if (!peer.PathAndQuery.Equals("/"))
                throw new ArgumentException(ExMsgUriBadPathQuery);
            if (!StringUtilities.IsNullOrEmpty(peer.Fragment))
                throw new ArgumentException(ExMsgUriBadFragment);

            //-------------
            // Host
            //-------------
            // We won't do any *more* checking of the host part here, there are various formats
            // allowed and we'll be strict at connection time.
        }


        void VerifyAndDisectUri()
        {
            VerifyUri(m_uri);
            m_service = m_uri.Scheme;
            //
            IrDAAddress irdaAddr; BluetoothAddress btAddr; IPAddress ipAddr;
            GetAddressFromUri(m_uri, out irdaAddr, out btAddr, out ipAddr);
            if (btAddr != null) {
                m_protoFromUri = ProtocolFamily_Bluetooth;
                m_addressFromUri = btAddr.ToString();
            } else if (irdaAddr != null) {
                m_protoFromUri = ProtocolFamily.Irda;
                m_addressFromUri = irdaAddr.ToString();
            } else if (ipAddr != null) {
                m_protoFromUri = ProtocolFamily.InterNetwork; //even if it's actually InterNetworkV6
                m_addressFromUri = ipAddr.ToString();
            } else {
                System.Diagnostics.Debug.Fail( "just to show that we never get here, GetAddressFromUri will have thrown.");
            }
        }


        /// <summary>
        /// Parse and convert the host portion of the request URI in to a device 
        /// address, either an IrDA address, a Bluetooth address, or an IP address based on the 
        /// format.
        /// </summary>
        /// -
        /// <param name="peer">
        /// An instance of <see cref="T:System.Uri"/> containing the URL to extract
        /// the host address part from.
        /// </param>
        /// <param name="irdaAddr">
        /// If <paramref name="peer"/> represent an IrDA address, an instance of 
        /// <see cref="T:InTheHand.Net.IrDAAddress"/> containing the respective value..
        /// </param>
        /// <param name="bluetoothAddr">
        /// If <paramref name="peer"/> represent an Bluetooth address, an instance of 
        /// <see cref="T:InTheHand.Net.BluetoothAddress"/> containing the respective value..
        /// </param>
        /// <param name="ipAddr">
        /// If <paramref name="peer"/> represent an IP address, an instance of 
        /// <see cref="T:InTheHand.Net.IPAddress"/> containing the respective value..
        /// </param>
        /// -
        /// <exception cref="T:System.ArgumentException">If the format of the host 
        /// portion of the given Uri is not recognized.
        /// </exception>
        void GetAddressFromUri(Uri peer, out IrDAAddress irdaAddr, out BluetoothAddress bluetoothAddr,
            out IPAddress ipAddr)
        {
            //
            // IrDA: 
            //   xxxxxxxx, length=8
            //   xx-xx-xx-xx, length=11
            //
            // Bluetooth: 
            //   xxxxxxxxxxxx, length=12
            //   xx-xx-xx-xx-xx-xx, length=17
            //
            String hostname = peer.Host;
            if (hostname.Length == 8
                || hostname.Length == (8 + 3)) {
                // IrDA
                irdaAddr = IrDAAddress.Parse(hostname);
                bluetoothAddr = null;
                ipAddr = null;
                return;
            }
            //
            if (hostname.Length == 12
                || hostname.Length == (12 + 5)) {
                //Bluetooth
                bluetoothAddr = BluetoothAddress.Parse(hostname);
                irdaAddr = null;
                ipAddr = null;
                return;
            }
            //
            try {
                ipAddr = IPAddress.Parse(hostname); // No TryParse on NETCF.
                // IP
                irdaAddr = null;
                bluetoothAddr = null;
                return;
            } catch (FormatException) {
            }
            //
            throw new ArgumentException("Unrecognized hostname format."
                + " E.g. for an IrDA peer use format xx-xx-xx-xx, as returned by IrDAAddress.ToString.");
        }//fn


        //--------------------------------------------------------------
        /// <exclude/>
        protected override string ChooseService() { return m_service; }

        /// <exclude/>
        protected override ProtocolFamily ChooseProtocol() { return m_protoFromUri; }

        /// <exclude/>
        protected override string ChoosePeer(ProtocolFamily proto) { return m_addressFromUri; }

        /// <exclude/>
        protected override void ShowStatus(string message) { /*NOOP*/; }

        //--------------------------------------------------------------
        /// <exclude/>
        public const String ExMsgUriBadScheme = "Uri Scheme invalid, not a known value.";
        /// <exclude/>
        public const String ExMsgUriHostIsEmpty = "Uri Host part is empty.";
        /// <exclude/>
        public const String ExMsgUriBadPathQuery = "Uri Path and Query must be empty";
        /// <exclude/>
        public const String ExMsgUriBadFragment = "Uri Fragment must be empty";

    }//class UriObexSessionConnection


#if NUNIT
    [TestFixture]
    public class Test_UriObexConnection_VerifyUri
    {
        internal class TestSeesInternalsUriObexSessionConnection : UriObexSessionConnection
        {
            public TestSeesInternalsUriObexSessionConnection(Uri uri)
                : base(uri)
            { }

            public ProtocolFamily ParsedProtocol { get { return base.ChooseProtocol(); } }
            public String ParsedPeer { get { return base.ChoosePeer(ProtocolFamily.Unknown); } }
            public String ParsedService { get { return base.ChooseService(); } }
            //
            internal byte[] TestsGetObexTarget() { return base.GetObexTarget(m_service); }
            internal static String[] TestsGetDefinedSchemes() { return s_schemes; }
        }//class


        internal static TestSeesInternalsUriObexSessionConnection New_UriObexConnection(Uri uri)
        {
            //UriObexSessionConnection conn = new UriObexSessionConnection(uri);
            TestSeesInternalsUriObexSessionConnection conn = new TestSeesInternalsUriObexSessionConnection(uri);
            return conn;
        }

        //--------
        void doTest(Uri uri, ProtocolFamily expectedProtocol, String expectedPeer, String expectedService)
        {
            TestSeesInternalsUriObexSessionConnection conn
                = New_UriObexConnection(uri);
            Assert.AreEqual(conn.ParsedProtocol, expectedProtocol);
            Assert.AreEqual(conn.ParsedPeer, expectedPeer);
            Assert.AreEqual(conn.ParsedService, expectedService);
        }

        //--------
        public const String UrlIrdaGood = "obex://12345678";
        public const String UrlIrdaGoodPush = "obex-push://12345678/";

        //--------
        [Test]
        [ExpectedException(typeof(ArgumentException), UriObexSessionConnection.ExMsgUriBadScheme)]
        public void badScheme()
        {
            New_UriObexConnection(new Uri("zzobex://12345678"));
        }

        //--------
        [Test]
        public void goodIrda()
        {
            doTest(new Uri(UrlIrdaGood), ProtocolFamily.Irda,
                "12345678", ObexTransportConnection.ObexDefaultService);
        }

        //[Test]        [Ignore]
        public void goodIrdaHyphens()
        {
            doTest(new Uri("obex://12-34-56-78"), ProtocolFamily.Irda,
                "12345678", ObexTransportConnection.ObexDefaultService);
        }

        [Test]
        public void goodIrdaDots()
        {
            doTest(new Uri("obex://12.34.56.78"), ProtocolFamily.Irda,
                "12345678", ObexTransportConnection.ObexDefaultService);
        }

        //[Test]        [Ignore]
        public void goodIrdaColonsA()
        {
            doTest(new Uri("obex://12:34:56:78"), ProtocolFamily.Irda,
                "12345678", ObexTransportConnection.ObexDefaultService);
        }

        // [Test]        [Ignore]
        //public void goodIrdaColonsBldr()
        //{
        //    UriBuilder bldr = new UriBuilder();
        //    bldr.Host = "12:34:56:78";
        //    bldr.Scheme = "obex";
        //    new UriObexConnection(bldr.Uri);
        //}

        [Test]
        public void goodIrdaPush()
        {
            doTest(new Uri(UrlIrdaGoodPush), ProtocolFamily.Irda,
                "12345678", ObexTransportConnection.ObexPushService);
        }

        [Test]
        public void goodBluetooth()
        {
            doTest(new Uri("obex://123456789012"), TransportConnection.ProtocolFamily_Bluetooth,
                "123456789012", ObexTransportConnection.ObexDefaultService);
        }

        //[Test]        [Ignore]
        public void goodBluetoothHyphens()
        {
            doTest(new Uri("obex://12-34-56-78-90-12"), TransportConnection.ProtocolFamily_Bluetooth,
                "123456789012", ObexTransportConnection.ObexDefaultService);
        }

        [Test]
        public void goodBluetoothDots()
        {
            doTest(new Uri("obex://12.34.56.78.90.12"), TransportConnection.ProtocolFamily_Bluetooth,
                "123456789012", ObexTransportConnection.ObexDefaultService);
        }

        //[Test]        [Ignore]
        public void goodBluetoothColons()
        {
            doTest(new Uri("obex://12:34:56:78:90:12"), TransportConnection.ProtocolFamily_Bluetooth,
                "123456789012", ObexTransportConnection.ObexDefaultService);
        }

        [Test]
        public void goodBluetoothFtp()
        {
            doTest(new Uri("obex-ftp://123456789012/"), TransportConnection.ProtocolFamily_Bluetooth,
                "123456789012", ObexTransportConnection.ObexFtpService);
        }

        //Later!
        //[Test]
        //public void goodBluetoothSync()
        //{
        //    New_UriObexConnection(new Uri("obex-sync://123456789012/"));
        //}

        //--------
        [Test]
        [ExpectedException(typeof(ArgumentException), UriObexSessionConnection.ExMsgUriBadPathQuery)]
        public void badPath()
        {
            New_UriObexConnection(new Uri("obex://12345678/p"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), UriObexSessionConnection.ExMsgUriBadPathQuery)]
        public void badQuery()
        {

            New_UriObexConnection(new Uri("obex://12345678/?q"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), UriObexSessionConnection.ExMsgUriBadFragment)]
        public void badFragment()
        {

            New_UriObexConnection(new Uri("obex://12345678#f"));
        }

        //--------
        [Test]
        [ExpectedException(typeof(ArgumentException), UriObexSessionConnection.ExMsgUriHostIsEmpty)]
        public void AddressByMistakeInPathPath()
        {
            New_UriObexConnection(new Uri("obex:/12345678/"));
        }

        //--------
    }


    [TestFixture]
    public class Test_UriObexConnection_VerifySchemeConversion
    {
        [Test]
        public void obexDefault(){
            Test_UriObexConnection_VerifyUri.TestSeesInternalsUriObexSessionConnection conn
                = Test_UriObexConnection_VerifyUri.New_UriObexConnection(new Uri(Test_UriObexConnection_VerifyUri.UrlIrdaGood));
            byte[] target = conn.TestsGetObexTarget();
        }

        [Test]
        public void obexPush()
        {
            Test_UriObexConnection_VerifyUri.TestSeesInternalsUriObexSessionConnection conn
                = Test_UriObexConnection_VerifyUri.New_UriObexConnection(new Uri(Test_UriObexConnection_VerifyUri.UrlIrdaGoodPush));
            byte[] target = conn.TestsGetObexTarget();
        }

        [Test]
        public void allInKnownList()
        {
            foreach (String curScheme in 
                        Test_UriObexConnection_VerifyUri.TestSeesInternalsUriObexSessionConnection.TestsGetDefinedSchemes()) {
                Uri uri = new UriBuilder(curScheme, "11223344").Uri;
                Test_UriObexConnection_VerifyUri.TestSeesInternalsUriObexSessionConnection conn
                    = Test_UriObexConnection_VerifyUri.New_UriObexConnection(uri);
                byte[] target = conn.TestsGetObexTarget();
            }
        }
    }
#endif



    //==========================================================================

    /// <summary>
    /// Specifies whether a blank line is valid input from the 
    /// <see cref="M:Brecham.Obex.Net.MenuKeyboardInput.ReadLine(Brecham.Obex.Net.ReadLineInputIsEmpty)"/>
    /// methods.
    /// </summary>
    public enum ReadLineInputIsEmpty
    {
        /// <summary>
        /// Do not accept an empty input line.
        /// </summary>
        DoNotAllow,

        /// <summary>
        /// Do accept an empty input line.
        /// </summary>
        Allow
    }


#if ! (NETCF && FX1_1)
    /// <summary>
    /// Provides methods to read user input as a console menu-based system.
    /// </summary>
    public static class MenuKeyboardInput
    {
        /// <summary>
        /// The <see cref="System.String"/> that contains the characters that form
        /// the user prompt, by default this is ">".
        /// </summary>
        public const String Prompt = ">";

        //----------------------------------------------------------------------

        /// <summary>
        /// Read a line from the console, checking for end of input and blank input.
        /// </summary>
        /// <param name="allowBlank">
        /// Whether empty input is acceptable.  
        /// That is that the user hits return without any other key being pressed
        /// for instance.
        /// </param>
        /// <returns>
        /// A <c>String</c> containing the input line.  Is always non-null, 
        /// but may be 'empty' depending on <paramref name="allowBlank"/>.
        /// </returns>
        /// <exception cref="System.IO.EndOfStreamException">
        /// The end of the input was reached.
        /// </exception>
        public static String ReadLine(ReadLineInputIsEmpty allowBlank)
        {
#if ! FX1_1
            return ReadLine(allowBlank, Console.In, Console.Out);
#else
            throw new RankException();
#endif
        }

        /// <summary>
        /// Read a line from the given <see cref="T:System.IO.TextReader"/>,
        /// checking for end of input and blank input.
        /// </summary>
        /// <param name="allowBlank">
        /// Whether empty input is acceptable.  
        /// That is that the user hits return without any other key being pressed
        /// for instance.
        /// </param>
        /// <param name="input">
        /// The <see cref="T:System.IO.TextReader"/> to read from.
        /// </param>
        /// <param name="output">
        /// The <see cref="T:System.IO.TextWriter"/> to write any prompts or messages to.
        /// </param>
        /// <returns>
        /// A <c>String</c> containing the input line.  Is always non-null, 
        /// but may be 'empty' depending on <paramref name="allowBlank"/>.
        /// </returns>
        /// <exception cref="System.IO.EndOfStreamException">
        /// The end of the input was reached.
        /// </exception>
        public static String ReadLine(ReadLineInputIsEmpty allowBlank, 
            TextReader input, TextWriter output)
        {
            if (input == null) {
                throw new ArgumentNullException("input");
            }
            if (output == null) {
                throw new ArgumentNullException("output");
            }
            //
            if (allowBlank != ReadLineInputIsEmpty.Allow &&
                allowBlank != ReadLineInputIsEmpty.DoNotAllow) {
                throw new ArgumentOutOfRangeException("allowBlank");
            }
            //
            String line;
            while (true) {
                line = input.ReadLine();
                if (line == null) {
                    // End of input
                    // Write an error, supressing any IO errors then.
                    try {
                        output.WriteLine(ExMsgReaderClosed);
                    } catch (IOException) {
                    } catch (ObjectDisposedException) {
                    }
                    throw new System.IO.EndOfStreamException(ExMsgReaderClosed);

                }
                if (line.Length != 0 || allowBlank == ReadLineInputIsEmpty.Allow) {
                    break;
                }
                // Retry since (Length==0 && allowBlank != ReadLineInputIsEmpty.Allow)
                output.Write("Input was blank" + Prompt);
            }//while
            return line;
        }

        /// <exclude/>
        public const String ExMsgReaderClosed = "Console input was closed.";

        //----------------------------------------------------------------------

        // Note NO internationalisation testing done!!!!!
        /// <summary>
        /// Gets one of a list of characters from the user as console input.
        /// </summary>
        /// <param name="validCharacters">
        /// A <see cref="T:System.String"/> containing the list of valid characters.
        /// </param>
        /// <returns>
        /// The character that the user entered as a <see cref="T:System.Char"/>.
        /// </returns>
        public static char GetCharacterInSetUppercase(String validCharacters)
        {
            if (validCharacters == null) { throw new ArgumentNullException("validCharacters"); }
            //
            char value;
            while (true) {
                Console.Write(" [" + validCharacters + "]" + Prompt);
                String line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.DoNotAllow);
                value = GetCharacterInSetUppercase(validCharacters, line);
                if (value != 0) {
                    break;
                }
                Console.Write("Not an allowed character, please try again");
            }//while
            System.Diagnostics.Debug.Assert(value != 0);
            return value;
        }

        // Note NO internationalisation testing done!!!!!
        /// <summary>
        /// Gets one of a list of characters from the specified input <see cref="T:System.String"/>.
        /// </summary>
        /// <param name="validCharacters">
        /// A <see cref="T:System.String"/> containing the list of valid characters.
        /// </param>
        /// <param name="inputLine">
        /// The input as a <see cref="T:System.String"/>.
        /// </param>
        /// <returns>
        /// The character that the user entered as a <see cref="T:System.Char"/>.
        /// </returns>
        public static char GetCharacterInSetUppercase(String validCharacters, String inputLine)
        {
            if (validCharacters == null) { throw new ArgumentNullException("validCharacters"); }
            if (inputLine == null) { throw new ArgumentNullException("inputLine"); }
            if (inputLine.Length == 0) { throw new ArgumentException("Is empty.", "inputLine"); }
            //
            for (int i = 0; i < validCharacters.Length; ++i) {
                if (Char.IsLower(validCharacters, i)) {
                    throw ExceptionFactory.ArgumentOutOfRangeException("validCharacters",
                        "Char " + i.ToString() + " is lower case.");
                }
            }
            //
            char value = Char.ToUpper(inputLine[0]);
            if (-1 != validCharacters.IndexOf(value)) {
                return value;
            } else {
                return (char)0;
            }
        }

        //----------------------------------------------------------------------
        /// <summary>
        /// Get input from the user as an integer in the given range.
        /// </summary>
        /// <param name="range">
        /// The size of the range to allow, value values being 0 through (range-1).
        /// </param>
        /// <returns>As integer in the given range.</returns>
        /// --
        /// <exception cref="System.IO.EndOfStreamException">
        /// The end of the input was reached.
        /// </exception>
        public static int GetIndexOfRange(int range)
        {
            Int32 value = -99;
            while (true) {
                Console.Write(Prompt);
                String line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.DoNotAllow);
                // No Int32.TryParse in CF20/FX1_1.
                // bool success = Int32.TryParse(line, out value);
                bool success = false;
                try {
                    //success = false;
                    value = Int32.Parse(line);
                    success = true;
                } catch (FormatException) { } catch (OverflowException) { }
                if (!success) {
                    Console.Write("Not a number, please try again");
                } else if (value >= range || value < 0) {
                    Console.Write("The number should be in the range, 0 through {0}", range - 1);
                } else { break; }
            }//while
            return value;
        }

        //----------------------------------------------------------------------

        /// <summary>
        /// Gets a Yes/No input from the user as console input.
        /// Assumes No if no, or invalid, input is provided.
        /// </summary>
        /// <returns>
        /// <c>true</c> for Yes, <c>false</c> for No.
        /// </returns>
        public static bool GetCmdLineIsYesAssumeNo()
        {
            Console.Write(" [y/N]" + Prompt);
            String input = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.Allow);
            return GetCmdLineIsYesAssumeNo(input);
        }

        /// <summary>
        /// Gets a Yes/No input from the user as console input.
        /// Assumes Yes if no, or invalid, input is provided.
        /// </summary>
        /// <returns>
        /// <c>true</c> for Yes, <c>false</c> for No.
        /// </returns>
        public static bool GetCmdLineIsYesAssumeYes()
        {
            Console.Write(" [Y/n]" + Prompt);
            String input = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.Allow);
            return GetCmdLineIsYesAssumeYes(input);
        }

        /// <summary>
        /// Gets a Yes/No input from the user as console input.
        /// If no, or invalid, input is provided, returns <paramref name="assume"/>.
        /// </summary>
        /// <param name="assume">
        /// What value to return on empty, or invalid, input.
        /// </param>
        /// <returns>
        /// <c>true</c> for yes, <c>false</c> for no.
        /// </returns>
        public static bool GetCmdLineIsYesBlankIs(bool assume)
        {
            if (assume) {
                Console.Write(" [Y/n]" + Prompt);
            } else {
                Console.Write(" [y/N]" + Prompt);
            }
            String input = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.Allow);
            return GetCmdLineIsYesBlankIs(assume, input);
        }

        //----

        /// <summary>
        /// Gets a Yes/No input from the specified input <see cref="T:System.String"/>.
        /// Assumes No if no, or invalid, input is provided.
        /// </summary>
        /// <param name="input">
        /// The input as a <see cref="T:System.String"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> for Yes, <c>false</c> for No.
        /// </returns>
        public static bool GetCmdLineIsYesAssumeNo(String input)
        {
            return GetCmdLineIsYesBlankIs(false, input);
        }

        /// <summary>
        /// Gets a Yes/No input from the specified input <see cref="T:System.String"/>.
        /// Assumes Yes if no, or invalid, input is provided.
        /// </summary>
        /// <param name="input">
        /// The input as a <see cref="T:System.String"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> for Yes, <c>false</c> for No.
        /// </returns>
        public static bool GetCmdLineIsYesAssumeYes(String input)
        {
            return GetCmdLineIsYesBlankIs(true, input);
        }

        /// <summary>
        /// Gets a Yes/No input from the specified input <see cref="T:System.String"/>.
        /// If no, or invalid, input is provided, returns <paramref name="assume"/>.
        /// </summary>
        /// <param name="assume">
        /// What value to return on empty, or invalid, input.
        /// </param>
        /// <param name="input">
        /// The input as a <see cref="T:System.String"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> for yes, <c>false</c> for no.
        /// </returns>
        public static bool GetCmdLineIsYesBlankIs(bool assume, String input)
        {
            if (input == null
                || input.Length == 0) {
                return assume;
            }
            //
            char firstChar = FirstCharAsUppercase(input);
            if (firstChar == 'Y') {
                return true;
            } else if (firstChar == 'N') {
                return false;
            } else {
                return assume;
            }
        }

        //----------------------------------------------------------------------

        /// <summary>
        /// Returns the first character in the <see cref="System.String"/>, as upper
        /// case if valid.
        /// </summary>
        /// <param name="input">The <see cref="System.String"/> to return the first character of.</param>
        /// <returns>
        /// The first character in the <see cref="System.String"/>, as upper
        /// case if valid.
        /// </returns>
        public static char FirstCharAsUppercase(String input)
        {
            if (input == null) {
                throw new ArgumentNullException("input");
            }
            if (input.Length < 1) {
                throw new ArgumentException("String is empty.", "input");
            }
#if ! FX1_1 && ! NETCF
            return input.Substring(0, 1).ToUpperInvariant()[0];
#else
            return input.Substring(0, 1).ToUpper(System.Globalization.CultureInfo.InvariantCulture)[0];
#endif
        }//fn

        //----------------------------------------------------------------------
    }//class
#endif



#if NUNIT
    [TestFixture]
    public class Test_ReadLine
    {
        //----------------------------------------
        [Test]
        [ExpectedException(typeof(EndOfStreamException), MenuKeyboardInput.ExMsgReaderClosed)]
        public void ImmediateEndOfInput()
        {
            StringReader rdr = new StringReader(String.Empty);
            String line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.Allow, rdr, TextWriter.Null);
        }

        [Test]
        public void EmptyAllow()
        {
            StringReader rdr = new StringReader(String.Empty + Environment.NewLine);
            String line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.Allow, rdr, TextWriter.Null);
            Assert.AreEqual(String.Empty, line);
        }

        [Test]
        public void EmptyAllowThenMoreLines()
        {
            StringReader rdr = new StringReader(String.Empty + Environment.NewLine
                + "abcd" + Environment.NewLine + "mnop" + Environment.NewLine);
            String line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.Allow, rdr, TextWriter.Null);
            Assert.AreEqual(String.Empty, line);
        }

        [Test]
        [ExpectedException(typeof(EndOfStreamException), MenuKeyboardInput.ExMsgReaderClosed)]
        public void EmptyNotAllowedThenEnd()
        {
            StringReader rdr = new StringReader(String.Empty + Environment.NewLine);
            String line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.DoNotAllow, rdr, TextWriter.Null);
        }

        [Test]
        public void EmptyNotAllowedThenValidNoNewLine()
        {
            const String value = "abcd";
            StringReader rdr = new StringReader(String.Empty + Environment.NewLine + value);
            String line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.DoNotAllow, rdr, TextWriter.Null);
            Assert.AreEqual(value, line);
        }

        [Test]
        public void EmptyNotAllowedThenValid()
        {
            const String value = "abcd";
            StringReader rdr = new StringReader(String.Empty + Environment.NewLine + value + Environment.NewLine);
            String line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.DoNotAllow, rdr, TextWriter.Null);
            Assert.AreEqual(value, line);
        }

        [Test]
        public void TwoLines()
        {
            const String value1 = "abcd";
            const String value2 = "mnop";
            StringReader rdr = new StringReader(value1 + Environment.NewLine + value2 + Environment.NewLine);
            String line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.DoNotAllow, rdr, TextWriter.Null);
            Assert.AreEqual(value1, line);
            line = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.DoNotAllow, rdr, TextWriter.Null);
            Assert.AreEqual(value2, line);
        }

    }


    [TestFixture]
    public class Test_GetCmdLineIsYesEtc
    {
        //----------------------------------------
        [Test]
        public void noneAssumeFalse()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesAssumeNo("");
            Assert.IsFalse(result);
        }

        [Test]
        public void noneAssumeTrue()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesAssumeYes("");
            Assert.IsTrue(result);
        }

        //----------------------------------------
        [Test]
        public void noneBlankIsFalse()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesBlankIs(false, "");
            Assert.IsFalse(result);
        }

        [Test]
        public void noneBlankIsTrue()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesBlankIs(true, "");
            Assert.IsTrue(result);
        }

        //----------------------------------------
        [Test]
        public void nLower()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesBlankIs(false, "n");
            Assert.IsFalse(result);
        }

        [Test]
        public void nUpper()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesBlankIs(false, "N");
            Assert.IsFalse(result);
        }

        [Test]
        public void yLower()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesBlankIs(false, "y");
            Assert.IsTrue(result);
        }

        [Test]
        public void yUpper()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesBlankIs(false, "Y");
            Assert.IsTrue(result);
        }

        [Test]
        public void nWithMore()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesBlankIs(false, "nKJSBDkasksaknakdhjd");
            Assert.IsFalse(result);
        }

        [Test]
        public void yWithMore()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesBlankIs(false, "yjksai8ywqkhe12kj");
            Assert.IsTrue(result);
        }

        //----------------------------------------
        [Test]
        public void otherAssumeFalse()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesBlankIs(false, "z");
            Assert.IsFalse(result);
        }

        [Test]
        public void otherAssumeTrue()
        {
            bool result = MenuKeyboardInput.GetCmdLineIsYesBlankIs(true, "z");
            Assert.IsTrue(result);
        }

    }//class

    [TestFixture]
    public class Test_GetCharacterInSet
    {
        //--------
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Char 3 is lower case.\r\nParameter name: validCharacters")]
        public void SetLowerCase()
        {
            MenuKeyboardInput.GetCharacterInSetUppercase("ABCd", "A");
        }

        //--------
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullInput()
        {
            Assert.AreEqual('A', MenuKeyboardInput.GetCharacterInSetUppercase("ABCD", null));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void BlankInput()
        {
            Assert.AreEqual('A', MenuKeyboardInput.GetCharacterInSetUppercase("ABCD", String.Empty));
        }

        //--------
        [Test]
        public void FirstOfFourLettersAllUpperCase()
        {
            Assert.AreEqual('A', MenuKeyboardInput.GetCharacterInSetUppercase("ABCD", "A"));
        }

        [Test]
        public void FourthOfFourLettersAllUpperCase()
        {
            Assert.AreEqual('D', MenuKeyboardInput.GetCharacterInSetUppercase("ABCD", "D"));
        }

        [Test]
        public void NoneOfFourLettersAllUpperCase()
        {
            Assert.AreEqual(0, MenuKeyboardInput.GetCharacterInSetUppercase("ABCD", "Z"));
        }

        //--------
        [Test]
        public void FirstOfFourLettersAllUpperCaseGivenLowerCase()
        {
            Assert.AreEqual('A', MenuKeyboardInput.GetCharacterInSetUppercase("ABCD", "a"));
        }

        [Test]
        public void FourthOfFourLettersAllUpperCaseGivenLowerCase()
        {
            Assert.AreEqual('D', MenuKeyboardInput.GetCharacterInSetUppercase("ABCD", "d"));
        }

        [Test]
        public void NoneOfFourLettersAllUpperCaseGivenLowerCase()
        {
            Assert.AreEqual(0, MenuKeyboardInput.GetCharacterInSetUppercase("ABCD", "z"));
        }

        //--------
        [Test]
        public void Dot()
        {
            Assert.AreEqual('.', MenuKeyboardInput.GetCharacterInSetUppercase("ABCD.", "."));
        }

    }
#endif


}

