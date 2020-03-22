/*=======================================================================================

	OpenNETCF.Net.ConnMgr
	Copyright © 2003, OpenNETCF.org

	This library is free software; you can redistribute it and/or modify it under 
	the terms of the OpenNETCF.org Shared Source License.

	This library is distributed in the hope that it will be useful, but 
	WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
	FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License 
	for more details.

	You should have received a copy of the OpenNETCF.org Shared Source License 
	along with this library; if not, email licensing@opennetcf.org to request a copy.

	If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please 
	email licensing@opennetcf.org.

	For general enquiries, email enquiries@opennetcf.org or visit our website at:
	http://www.opennetcf.org

=======================================================================================*/
using System;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net
{
	#region -------------- Enums --------------

	/// <summary>
	/// Specifies the network connection state.
	/// </summary>
	[Obsolete("ConnectionManagerState is obsolete. Use ConnectionStatus instead.",true)]
	public enum ConnectionManagerState : int
	{
//		Connected = 0, 
//		Disconnected, 
//		WaitingForConnection, 
//		WaitingForDisconnection,
//		Failed
	};

	public enum ConnectionPriority : int
	{
		Voice					= 0x20000,
		UserInteractive			= 0x08000,
		UserBackground			= 0x02000,
		UserIdle				= 0x0800,
		HighPriorityBackground	= 0x0200,
		IdleBackground			= 0x0080,
		ExternalInteractive		= 0x0020,
		LowBackground			= 0x0008,
		Cached					= 0x0002,
	};

	/// <summary>
	/// Describes the current status of the connection
	/// </summary>
	public enum ConnectionStatus : int
	{
		/// <summary>
		/// Unknown status
		/// </summary>
		Unknown					= 0x00, 
		/// <summary>
		/// Connection is up
		/// </summary>
		Connected				= 0x10,
		/// <summary>
		/// Connection is disconnected
		/// </summary>
		Disconnected			= 0x20,
		/// <summary>
		/// Connection failed and cannot not be re-established
		/// </summary>
		ConnectionFailed		= 0x21,
		/// <summary>
		/// User aborted connection
		/// </summary>
		ConnectionCancelled		= 0x22, 
		/// <summary>
		/// Connection is ready to connect but disabled
		/// </summary>
		ConnectionDisabled		= 0x23, 
		/// <summary>
		/// No path could be found to destination
		/// </summary>
		NoPathToDestination		= 0x24, 
		/// <summary>
		/// Waiting for a path to the destination
		/// </summary>
		WaitingForPath			= 0x25,
		/// <summary>
		/// Voice call is in progress
		/// </summary>
		WaitingForPhone			= 0x26,
		/// <summary>
		/// Attempting to connect
		/// </summary>
		WaitingConnection		= 0x40,
		/// <summary>
		/// Resource is in use by another connection
		/// </summary>
		WaitingForResource		= 0x41, 
		/// <summary>
		/// No path could be found to destination
		/// </summary>
		WaitingForNetwork		= 0x42, 
		/// <summary>
		/// Connection is being brought down
		/// </summary>
		WaitingDisconnection	= 0x80, 
		/// <summary>
		/// Aborting connection attempt
		/// </summary>
		WaitingConnectionAbort	= 0x81,
	};

	public enum ConnectionMode : int
	{
		Synchronous = 0,
		Asynchronous = 1,
	}
	#endregion

	/// <summary>
	/// A class that handles opening and closing of network connections. 
	/// <para>
	/// It is important to remember that when connecting asynchronous, 
	/// you must poll the connection status in your code.
	/// </para>
	/// <example>
	/// <code>
	/// bool connected = false;
	/// ConnectionManger connMgr = new ConnectionManager();
	/// connMgr.Connect(true, ConnectionMode.Asynchronous);
	/// while(!connected)
	/// {
	///     if(connMgr.Status != ConnectionStatus.Connected)
	///     {
	///		    Thread.Sleep(1000);
	///		    continue;
	///     }
	///     connected = true;
	/// }
	/// // Now we are connected and can do something... 
	/// </code>
	/// </example>
	/// <para>
	/// It is also important to note that creating a non-exclusive 
	/// connection will result in your application being unable to 
	/// programmatically disconnect.
	/// </para>
	/// </summary>
	sealed public class ConnectionManager
	{
		#region ---------- Constructors ----------

		/// <summary>
		/// Creates a new instance of the Connection Manager object.
		/// </summary>
		public ConnectionManager()
		{
#if !NDOC
			mwnd = new MsgWnd(this);
			hwnd = mwnd.Hwnd;
#endif
		}

		#endregion

		#region ------------- Fields -------------

		private IntPtr hConnection = IntPtr.Zero;
		private IntPtr hwnd = IntPtr.Zero;
		private int	dwTimeout = 5000; // Default the timeout to 5 seconds
#if !NDOC
		private MsgWnd mwnd = null;
#endif
		private ConnectionInfo connInfo;

		private const int CONNMGR_PARAM_GUIDDESTNET = (0x1);
		private const int WM_APP_CONNMGR = 0x400+0;

		private readonly System.Guid IID_DestNetInternet = new System.Guid("436ef144-b4fb-4863-a041-8f905a62c572");
		private readonly System.Guid IID_DestNetGprs = new Guid("adb0b001-10b5-3f39-27c6-9742e785fcd4");
		
		// For thread safety
		private object syncRoot = new object();

		private EventHandler connectedEvent;
		private EventHandler disconnectEvent;
		private EventHandler connectionStateChangedEvent;
		private	EventHandler connectionFailedEvent;

		#endregion

		#region ----------- Properties -----------

		/// <summary>
		/// The current connection state of Connection Manager
		/// </summary>
		[Obsolete("ConnectionManager.State has been deprecated. Please use ConnectionManager.Status.",true)]
		public ConnectionManagerState State
		{			
			get { return 0; }
		}
		// ----------------------------------------------------------------
		/// <summary>
		/// Returns a handle to the current connection.
		/// </summary>
		public IntPtr Handle
		{
			get  
			{ 
				IntPtr handle = IntPtr.Zero;
				lock(syncRoot) 
				{ 
					handle = hConnection; 
				} 
				return handle;
			}
		}
		// ---------------------------------------------------------------
		/// <summary>
		/// Describes the current state of the connection
		/// </summary>
		public ConnectionStatus Status
		{
			get 
			{ 
				ConnectionStatus status = ConnectionStatus.Unknown;
				
				lock(syncRoot)
				{
					int dwStatus = new int();
					int result = ConnMgrConnectionStatus(this.Handle, out dwStatus);
					if(result == 0)
					{
						status = (ConnectionStatus)dwStatus;
					}
				}
				return status;
			}
		}

		/// <summary>
		/// Specifies the timeout for a synchronous connection attempt
		/// </summary>
		public int Timeout
		{
			get 
			{ 
				int i = 0;
				lock(syncRoot)
				{
					i = dwTimeout;
				}
				return i;
			}
			set 
			{
				lock(syncRoot)
				{
					dwTimeout = value;
				}
			}
		}

		#endregion

		#region ------------- Events -------------

		[Obsolete("The Connected event is deprecated. Please use the OnConnect event.", true)]
		public event EventHandler Connected;

		/// <summary>
		/// Occurs when a connection is opened.
		/// </summary>
		public event EventHandler OnConnect
		{
			add { lock(syncRoot) { connectedEvent += value; } }
			remove { lock(syncRoot) { connectedEvent -= value; } }
		}

		[Obsolete("The Disconnected event is deprecated. Please use the OnDisconnect event.", true)]
		public event EventHandler Disconnected;

		/// <summary>
		/// Occurs when a connection is closed.
		/// </summary>
		public event EventHandler OnDisconnect
		{
			add { lock(syncRoot) { disconnectEvent += value; } }
			remove { lock(syncRoot) { disconnectEvent -= value; } }
		}

		[Obsolete("The ConnectionStateChanged event is deprecated. Please use the OnConnectionStateChanged event.", true)]
		public event EventHandler ConnectionStateChanged;

		/// <summary>
		/// Occurs when the connection state is changed.
		/// </summary>
		public event EventHandler OnConnectionStateChanged
		{
			add { lock(syncRoot) { connectionStateChangedEvent += value; } }
			remove { lock(syncRoot) { connectionStateChangedEvent -= value; } }
		}

		[Obsolete("The ConnectionFailed event is deprecated. Please use the OnConnectionFailed event.", true)]
		public event EventHandler ConnectionFailed;

		/// <summary>
		/// Occurs when a connection fails.
		/// </summary>
		public event EventHandler OnConnectionFailed
		{
			add { lock(syncRoot) { connectionFailedEvent += value; } } 
			remove { lock(syncRoot) { connectionFailedEvent -= value; } }
		}

		/// <summary>
		/// Raises the OpenNETCF.Net.ConnectionManager.OnConnect event.
		/// </summary>
		/// <param name="e">A System.EventArgs object that contains the event data.</param>
		internal void ConnectEvent(EventArgs e)
		{
			EventHandler handler;
			lock(syncRoot)
			{
				handler = connectedEvent;
			}
			if(handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Raises the OpenNETCF.Net.ConnectionManager.Disconnected event.
		/// </summary>
		/// <param name="e">A System.EventArgs object that contains the event data.</param>
		internal void DisconnectEvent(EventArgs e)
		{
			EventHandler handler;
			lock(syncRoot) 
			{
				handler = disconnectEvent;
			}
			if(handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Raises the OpenNETCF.Net.ConnectionManager.ConnectionStateChanged event.
		/// </summary>
		/// <param name="e">A System.EventArgs object that contains the event data.</param>
		internal void ConnectionStateChangedEvent(EventArgs e)
		{
			EventHandler handler;
			lock(syncRoot)
			{
				handler = connectionStateChangedEvent;
			}
			if(handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Raises the OpenNETCF.Net.ConnectionManager.ConnectionFailed event.
		/// </summary>
		/// <param name="e">A System.EventArgs object that contains the event data.</param>
		internal void ConnectionFailedEvent(EventArgs e)
		{
			EventHandler handler;
			lock(syncRoot)
			{
				handler = connectionFailedEvent;
			}
			if(handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region --------- Public Members ---------

		/// <summary>
		/// Makes an exclusive, asynchronous connection with Connection Manager using the system default destination. 
		/// </summary>
		public void Connect()
		{
			this.Connect(IID_DestNetGprs, true, ConnectionMode.Asynchronous);
		}

		/// <summary>
		/// Makes an asynchronous connection with Connection Manager using the system default destination.
		/// </summary>
		/// <param name="exclusive">True creates an exclusive connection; false creates a non-exclusive connection.</param>
		public void Connect(bool exclusive)
		{
			this.Connect(IID_DestNetGprs, exclusive, ConnectionMode.Asynchronous);
		}

		/// <summary>
		/// Makes an exclusive connection with Connection Manager using the system default destination.
		/// </summary>
		/// <param name="mode">States how the connection is to be made: either Synchronous or Asynchronous</param>
		public void Connect(ConnectionMode mode)
		{
			this.Connect(IID_DestNetGprs, true, mode);
		}
		
		/// <summary>
		/// Makes a connection with Connection Manager using the system default destination.
		/// </summary>
		/// <param name="exclusive">True creates an exclusive connection; false creates a non-exclusive connection.</param>
		/// <param name="mode">States how the connection is to be made: either Synchronous or Asynchronous</param>
		public void Connect(bool exclusive, ConnectionMode mode)
		{
			this.Connect(IID_DestNetGprs, exclusive, mode);
		}

		/// <summary>
		/// Makes a connection with Connection Manager using the specified destination.
		/// </summary>
		/// <param name="destGuid">The destination to connect to.</param>
		/// <param name="exclusive">Determines whether the connection should be exclusive or not.</param>
		/// <param name="mode">Determines how the connection is to be made: either Synchronous or Asynchronous</param>
		public void Connect(Guid destGuid, bool exclusive, ConnectionMode mode)
		{
			lock(syncRoot)
			{
				connInfo.cbSize = Marshal.SizeOf(connInfo);
				connInfo.dwParams = CONNMGR_PARAM_GUIDDESTNET;
				connInfo.dwPriority = (int)ConnectionPriority.UserBackground; 
				connInfo.dwFlags = 0;
				connInfo.bExclusive = exclusive;
				connInfo.bDisabled = false;
				connInfo.guidDestNet = destGuid; 
				connInfo.hWnd = hwnd;
				connInfo.uMsg = WM_APP_CONNMGR;
				connInfo.lParam = 0;
			
				IntPtr hConnInfo = connInfo.ToPtr(); 

				int result = -1, dwStatus = 0;
				if(mode == ConnectionMode.Synchronous)
				{
					result = ConnMgrEstablishConnectionSync(hConnInfo, out hConnection, dwTimeout, out dwStatus);
					if(result != 0)
					{
						throw new InvalidOperationException("Failed to make a connection. ConnMgr returned status " + ((ConnectionStatus)dwStatus).ToString());
					}
				}
				else
				{
					ConnMgrEstablishConnection(hConnInfo, out hConnection);
				}
			}
		}

		/// <summary>
		/// Maps a URL to a destination GUID.
		/// </summary>
		/// <param name="Url">The URL to map.</param>
		/// <returns>The GUID the URL is mapped to.</returns>
		public Guid MapUrl(string Url)
		{
			Guid urlGuid = Guid.Empty;
				
			lock(syncRoot)
			{
				int idx = 0;
				ConnMgrMapURL(Url, ref urlGuid, ref idx);
			}
			return urlGuid;
		}

		/// <summary>
		/// Disconnection the current connection.
		/// </summary>
		public void Disconnect()
		{
			if(this.Handle != IntPtr.Zero)
				this.Disconnect(this.Handle);

		}

		/// <summary>
		/// Disconnection the connection whose handle is hConnection.
		/// </summary>
		/// <param name="hConnection">The handle of the connection to close.</param>
		public void Disconnect(IntPtr hConnection)
		{
			lock(syncRoot)
			{
				int result = ConnMgrReleaseConnection(hConnection, Convert.ToInt32(false));	
				if(result == 0)
				{
					// The connection handle has been freed, so zero it to prevent handle leaks
					hConnection = IntPtr.Zero;
								
					// Raise the OnDisconnect event since the WndProc can't/won't do it for us
					DisconnectEvent(EventArgs.Empty);
				}
				
			}
		}

		/// <summary>
		/// Returns a collection of destinations specified within the system.
		/// </summary>
		/// <returns>A DestinationInfo collection with details of all the destinations in the system.</returns>
		public DestinationInfoCollection EnumDestinations()
		{
			DestinationInfo destInfo = new DestinationInfo();
			IntPtr hDestInfo = IntPtr.Zero;

			DestinationInfoCollection dests = new DestinationInfoCollection();

			bool loop = true;

			int i = 0;
			int ret = 0;

			lock(syncRoot)
			{
				do  
				{
					hDestInfo = MarshalEx.AllocHLocal((uint)DestinationInfo.NativeSize);

					ret = ConnMgrEnumDestinations(i++, hDestInfo);
					if(ret == -2147467259)
					{
						loop = false;
						break;
					}
					DestinationInfo cm = new DestinationInfo(hDestInfo);
					dests.Add(cm);

					MarshalEx.FreeHLocal(hDestInfo);
				}
				while(loop);
			}

			return dests;
		}

		#endregion

		#region --------- API Prototypes ---------

		[DllImport("cellcore.dll",EntryPoint="ConnMgrReleaseConnection",SetLastError=true)]
		internal static extern int ConnMgrReleaseConnection(IntPtr hConnection, int bCache);

		[DllImport("cellcore.dll",EntryPoint="ConnMgrEstablishConnection",SetLastError=true)]
		internal static extern int ConnMgrEstablishConnection(IntPtr pConnInfo, out IntPtr phConnection);

		[DllImport("cellcore.dll",EntryPoint="ConnMgrEstablishConnectionSync",SetLastError=true)]
		internal static extern int ConnMgrEstablishConnectionSync(IntPtr pConnInfo, out IntPtr phConnection, int dwTimeout, out int dwStatus);

		[DllImport("cellcore.dll",EntryPoint="ConnMgrEnumDestinations",SetLastError=true)]
		internal static extern int ConnMgrEnumDestinations(int nIndex, IntPtr pDestinationInfo);

		[DllImport("cellcore.dll",EntryPoint="ConnMgrMapURL",SetLastError=true)]
		internal static extern int ConnMgrMapURL(string pwszUrl, ref Guid pguid, ref int pdwIndex);

		[DllImport("cellcore.dll",EntryPoint="ConnMgrConnectionStatus",SetLastError=true)]
		internal static extern int ConnMgrConnectionStatus(IntPtr hConnection, out int pdwStatus);

		#endregion

		#region ----- Window Message Handler -----

#if !NDOC
		internal class MsgWnd : MessageWindow
		{
			private const int WM_APP_CONNMGR = 0x400+0;
			private const int CONNMGR_STATUS_CONNECTED = 0x10;
			private const int CONNMGR_STATUS_DISCONNECTED = 0x20;
			private const int CONNMGR_STATUS_WAITINGCONNECTION = 0x40;
			private const int CONNMGR_STATUS_WAITINGDISCONNECTION = 0x80;
			private const int CONNMGR_STATUS_CONNECTIONFAILED = 0x21;

			private ConnectionManager connmgr;

			public MsgWnd(ConnectionManager obj)
			{
				this.connmgr = obj;
				this.connmgr.hwnd = this.Hwnd;
			}

			protected override void WndProc(ref Message m)
			{
				switch(m.Msg)
				{
					case WM_APP_CONNMGR: 
					{
						switch((int)m.WParam)
						{
							case CONNMGR_STATUS_CONNECTED:
								this.connmgr.ConnectionStateChangedEvent(EventArgs.Empty);
								this.connmgr.ConnectEvent(EventArgs.Empty);
								break;
							case CONNMGR_STATUS_WAITINGCONNECTION:
								this.connmgr.ConnectionStateChangedEvent(EventArgs.Empty);
								break;
// -----------------------------------------------------------------------
// This WndProc never seems to receive CONNMGR_STATUS_DISCONNECTED, so let's remove it
//							case CONNMGR_STATUS_DISCONNECTED:
//								this.connmgr.SetState(ConnectionManagerState.Disconnected);
//								this.connmgr.DisconnectEvent(EventArgs.Empty);
//								break;
// -----------------------------------------------------------------------
							case CONNMGR_STATUS_WAITINGDISCONNECTION:
								this.connmgr.ConnectionStateChangedEvent(EventArgs.Empty);
								break;
							case CONNMGR_STATUS_CONNECTIONFAILED:
								this.connmgr.ConnectionStateChangedEvent(EventArgs.Empty);
								break;
						} 
					} 
					break;
				}

				base.WndProc(ref m);
			}

		}
#endif
		#endregion

	}
}

