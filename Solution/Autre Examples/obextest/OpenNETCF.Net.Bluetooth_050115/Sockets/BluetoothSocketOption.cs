//==========================================================================================
//
//		OpenNETCF.Net.Sockets.BluetoothSocketOptionName
//		Copyright (C) 2003-2005, OpenNETCF.org
//
//		This library is free software; you can redistribute it and/or modify it under 
//		the terms of the OpenNETCF.org Shared Source License.
//
//		This library is distributed in the hope that it will be useful, but 
//		WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
//		FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License 
//		for more details.
//
//		You should have received a copy of the OpenNETCF.org Shared Source License 
//		along with this library; if not, email licensing@opennetcf.org to request a copy.
//
//		If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please 
//		email licensing@opennetcf.org.
//
//		For general enquiries, email enquiries@opennetcf.org or visit our website at:
//		http://www.opennetcf.org
//
//==========================================================================================
using System;

namespace OpenNETCF.Net.Sockets
{
	/// <summary>
	/// Defines <see cref="System.Net.Sockets.Socket"/> configuration option names for the <see cref="System.Net.Sockets.Socket"/> class when used with Bluetooth.
	/// </summary>
	public enum BluetoothSocketOptionName : int
	{
		/// <summary>
		/// On connected socket, triggers authentication.
		/// On not connected socket, forces authentication on connection.
		/// For incoming connection this means that connection is rejected if authentication cannot be performed.
		/// </summary>
		/// <remarks>The optval and optlen parameters are ignored; however, Winsock implementation on Windows CE requires optlen to be at least 4 and optval to point to at least an integer datum.</remarks>
		Authenticate	= 0x00000001,
		/// <summary>
		/// On a connected socket, this command turns encryption on or off.
		/// On an unconnected socket, this forces encryption to be on or off on connection.
		/// For an incoming connection, this means that the connection is rejected if the encryption cannot be turned on.
		/// </summary>
		Encrypt			= 0x00000002,	// optlen=sizeof(unsigned int), optval = &(unsigned int)TRUE/FALSE
		/// <summary>
		/// This sets or revokes PIN code to use with a connection or socket.
		/// </summary>
		SetPin			= 0x00000003,	// bound only! survives socket! optlen=sizeof(BTH_SOCKOPT_SECURITY), optval=&BTH_SOCKOPT_SECURITY
		/// <summary>
		/// This sets or revokes link key to use with a connection or peer device.
		/// </summary>
		SetLink			= 0x00000004,	// bound only! survives socket! optlen=sizeof(BTH_SOCKOPT_SECURITY), optval=&BTH_SOCKOPT_SECURITY
		/// <summary>
		/// Returns link key associated with peer Bluetooth device.
		/// </summary>
		GetLink			= 0x00000005,	// bound only! optlen=sizeof(BTH_SOCKOPT_SECURITY), optval=&BTH_SOCKOPT_SECURITY
		/// <summary>
		/// This sets default MTU (maximum transmission unit) for connection negotiation.
		/// While allowed for connected socket, it has no effect if the negotiation has already completed.
		/// Setting it on listening socket will propagate the value for all incoming connections.
		/// </summary>
		SetMtu			= 0x00000006,	// unconnected only! optlen=sizeof(unsigned int), optval = &mtu
		/// <summary>
		/// Returns MTU (maximum transmission unit).
		/// For connected socket, this is negotiated value, for server (accepting) socket it is MTU proposed for negotiation on connection request.
		/// </summary>
		GetMtu			= 0x00000007,	// optlen=sizeof(unsigned int), optval = &mtu
		/// <summary>
		/// This sets maximum MTU for connection negotiation.
		/// While allowed for connected socket, it has no effect if the negotiation has already completed.
		/// Setting it on listening socket will propagate the value for all incoming connections.
		/// </summary>
		SetMtuMaximum	= 0x00000008,	// unconnected only! optlen=sizeof(unsigned int), optval = &max. mtu
		/// <summary>
		/// Returns maximum MTU acceptable MTU value for a connection on this socket.
		/// Because negotiation has already happened, has little meaning for connected socket.
		/// </summary>
		GetMtuMaximum	= 0x00000009,	// bound only! optlen=sizeof(unsigned int), optval = &max. mtu
		/// <summary>
		/// This sets minimum MTU for connection negotiation.
		/// While allowed for connected socket, it has no effect if the negotiation has already completed.
		/// Setting it on listening socket will propagate the value for all incoming connections.
		/// </summary>
		SetMtuMinimum	= 0x0000000a,	// unconnected only! optlen=sizeof(unsigned int), optval = &min. mtu
		/// <summary>
		/// Returns minimum MTU acceptable MTU value for a connection on this socket.
		/// Because negotiation has already happened, has little meaning for connected socket. 
		/// </summary>
		GetMtuMinimum	= 0x0000000b,	// bound only! optlen=sizeof(unsigned int), optval = &min. mtu
		/// <summary>
		/// This sets XON limit.
		/// Setting it on listening socket will propagate the value for all incoming connections.
		/// </summary>
		SetXOnLimit		= 0x0000000c,	// optlen=sizeof(unsigned int), optval = &xon limit (set flow off)
		/// <summary>
		/// Returns XON limit for a connection.
		/// XON limit is only used for peers that do not support credit-based flow control (mandatory in the Bluetooth Core Specification version 1.1).
		/// When amount of incoming data received, but not read by an application for a given connection grows past this limit, a flow control command is sent to the peer requiring suspension of transmission.
		/// </summary>
		GetXOnLimit		= 0x0000000d,	// optlen=sizeof(unsigned int), optval = &xon
		/// <summary>
		/// This sets XOFF limit.
		/// Setting it on listening socket will propagate the value for all incoming connections.
		/// </summary>
		SetXOffLimit	= 0x0000000e,	// optlen=sizeof(unsigned int), optval = &xoff limit (set flow on)
		/// <summary>
		/// Returns XOFF limit for a connection.
		/// XOFF limit is only used for peers that do not support credit-based flow control (mandatory in the Bluetooth Core Specification 1.1).
		/// If flow has been suspended because of buffer run-up, when amount of incoming data received, but not read by an application for a given connection falls below this limit, a flow control command is sent to the peer allowing continuation of transmission.
		/// </summary>
		GetXOffLimit	= 0x0000000f,	// optlen=sizeof(unsigned int), optval = &xoff
		/// <summary>
		/// Specifies maximum amount of data that can be buffered inside RFCOMM (this is amount of data before call to send blocks).
		/// </summary>
		SetSendBuffer	= 0x00000010,	// optlen=sizeof(unsigned int), optval = &max buffered size for send
		/// <summary>
		///  Returns maximum amount of data that can be buffered inside RFCOMM (this is amount of data before call to send blocks).
		/// </summary>
		GetSendBuffer	= 0x00000011,	// optlen=sizeof(unsigned int), optval = &max buffered size for send
		/// <summary>
		/// Specifies maximum amount of data that can be buffered for a connection.
		/// This buffer size is used to compute number of credits granted to peer device when credit-based flow control is implemented.
		/// This specifies the maximum amount of data that can be buffered.
		/// </summary>
		SetReceiveBuffer	= 0x00000012,	// optlen=sizeof(unsigned int), optval = &max buffered size for recv
		/// <summary>
		/// Returns maximum amount of data that can be buffered for a connection.
		/// This buffer size is used to compute number of credits granted to peer device when credit-based flow control is implemented.
		/// This specifies the maximum amount of data that can be buffered.
		/// </summary>
		GetReceiveBuffer	= 0x00000013,	// optlen=sizeof(unsigned int), optval = &max buffered size for recv
		/// <summary>
		/// Retrieves last v24 and break signals set through MSC command from peer device.
		/// </summary>
		GetV24Break			= 0x00000014,	// connected only! optlen=2*sizeof(unsigned int), optval = &{v24 , br}
		/// <summary>
		/// Retrieves last line status signals set through RLS command from peer device.
		/// </summary>
		GetRls				= 0x00000015,	// connected only! optlen=sizeof(unsigned int), optval = &rls
		/// <summary>
		/// Sends MSC command. V24 and breaks are as specified in RFCOMM Specification.
		/// Only modem signals and breaks can be controlled, RFCOMM reserved fields such as flow control are ignored and should be set to 0.
		/// </summary>
		SendMsc				= 0x00000016,	// connected only! optlen=2*sizeof(unsigned int), optval = &{v24, br}
		/// <summary>
		/// Sends RLS command.
		/// Argument is as specified in RFCOMM Specification.
		/// </summary>
		SendRls				= 0x00000017,	// connected only! optlen=sizeof(unsigned int), optval = &rls
		/// <summary>
		/// Gets flow control type on the connected socket.
		/// </summary>
		GetFlowType			= 0x00000018,	// connected only! optlen=sizeof(unsigned int), optval=&1=credit-based, 0=legacy
		/// <summary>
		/// Sets the page timeout for the card.
		/// The socket does not have to be connected.
		/// </summary>
		SetPageTimeout		= 0x00000019,	// no restrictions. optlen=sizeof(unsigned int), optval = &page timeout
		/// <summary>
		/// Gets the current page timeout.
		/// The socket does not have to be connected.
		/// </summary>
		GetPageTimeout		= 0x0000001a,	// no restrictions. optlen=sizeof(unsigned int), optval = &page timeout
		/// <summary>
		/// Sets the scan mode for the card.
		/// The socket does not have to be connected.
		/// </summary>
		SetScan				= 0x0000001b,	// no restrictions. optlen=sizeof(unsigned int), optval = &scan mode
		/// <summary>
		/// Gets the current scan mode.
		/// The socket does not have to be connected.
		/// </summary>
		GetScan				= 0x0000001c,	// no restrictions. optlen=sizeof(unsigned int), optval = &scan mode

		/// <summary>
		/// Sets the class of the device.
		/// The socket does not have to be connected.
		/// </summary>
		SetCod =  0x0000001d,	// no restrictions. 
		/// <summary>
		/// Retrieve the Class of Device.
		/// </summary>
		GetCod =  0x0000001e,	// no restrictions. optlen=sizeof(unsigned int), optval = &cod
		/// <summary>
		/// Get the version information from the Bluetooth adapter.
		/// </summary>
		GetLocalVersion		= 0x0000001f, // no restrictions. 

		/// <summary>
		/// Get the version of the remote adapter.
		/// </summary>
		GetRemoteVersion	= 0x00000020,	// connected only! optlen=sizeof(BTH_REMOTE_VERSION), optval = &BTH_REMOTE_VERSION
		
		/// <summary>
		/// Retrieves the authentication settings.
		/// The socket does not have to be connected.
		/// </summary>
		GetAuthenticationEnabled = 0x00000021,	// no restrictions. optlen=sizeof(unsigned int), optval = &authentication enable
		/// <summary>
		/// Sets the authentication policy of the device.
		/// </summary>
		SetAuthenticationEnabled = 0x00000022,	// no restrictions. optlen=sizeof(unsigned int), optval = &authentication enable

		/// <summary>
		/// Reads the remote name of the device.
		/// The socket does not have to be connected.
		/// </summary>
		ReadRemoteName		= 0x00000023,	// no restrictions.

		/// <summary>
		/// Retrieves the link policy of the device.
		/// </summary>
		GetLinkPolicy		= 0x00000024,	// connected only! optlen=sizeof(unsigned int), optval = &link policy
		/// <summary>
		/// Sets the link policy for an existing baseband connection.
		/// The socket must be connected.
		/// </summary>
		SetLinkPolicy		= 0x00000025,	// connected only! optlen=sizeof(unsigned int), optval = &link policy
		
		/// <summary>
		/// 
		/// </summary>
		EnterHoldMode		= 0x00000026,  // connected only! optlen=sizeof(BTH_HOLD_MODE), optval = &BTH_HOLD_MODE
		/// <summary>
		/// 
		/// </summary>
		EnterSniffMode		= 0x00000027,  // connected only! optlen=sizeof(BTH_SNIFF_MODE), optval = &BTH_SNIFF_MODE
		/// <summary>
		/// 
		/// </summary>
		ExitSniffMode		= 0x00000028,  // connected only! optlen=0, optval - ignored
		/// <summary>
		/// 
		/// </summary>
		EnterParkMode		= 0x00000029,  // connected only! optlen=sizeof(BTH_PARK_MODE), optval = &BTH_PARK_MODE
		/// <summary>
		/// 
		/// </summary>
		ExitParkMode		= 0x0000002a,  // connected only! optlen=0, optval - ignored
		/// <summary>
		/// Gets the current mode of the connection.
		/// The mode can either be sniff, park, or hold. The socket must be connected.
		/// </summary>
		GetMode				= 0x0000002b,	// connected only! optlen=sizeof(int), optval = &mode
	}

	/// <summary>
	/// Defines Bluetooth specific socket option levels for the <see cref="System.Net.Sockets.Socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel,System.Net.Sockets.SocketOptionName,System.Int32)"/> and <see cref="System.Net.Sockets.Socket.GetSocketOption(System.Net.Sockets.SocketOptionLevel,System.Net.Sockets.SocketOptionName)"/> methods.   
	/// </summary>
	public enum BluetoothSocketOptionLevel :int
	{
		/// <summary>
		/// 
		/// </summary>
		RFComm  = 0x03,
		/// <summary>
		/// 
		/// </summary>
		BthTdi  = 0x100,
		/// <summary>
		/// 
		/// </summary>
		Sdp     = 0x0101,
	}

	/// <summary>
	/// Specifies the Bluetooth specific protocols that the <see cref="System.Net.Sockets.Socket"/> class supports.
	/// </summary>
	public enum BluetoothProtocolType : int
	{
		Sdp = 0x0001,
		Udp = 0x0002,

		/// <summary>
		/// 
		/// </summary>
		RFComm  = 0x0003,

		Tcp = 0x0004,
		TcsBin = 0x0005,
		TcsAt = 0x0006,
		Obex = 0x0008,

		/// <summary>
		/// 
		/// </summary>
		L2Cap   = 0x0100,
	}

	/// <summary>
	/// Specifies the Bluetooth specific addressing scheme that an instance of the <see cref="System.Net.Sockets.Socket"/> class can use.
	/// </summary>
	public enum BluetoothAddressFamily : int
	{
		/// <summary>
		/// Bluetooth address.
		/// </summary>
		/// <value>32</value>
		Bluetooth = 32,
	}
}
