using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using OpenNETCF.Net.Bluetooth.Internal;
using OpenNETCF.Net.Bluetooth.Sdp;

namespace OpenNETCF.Net.Sockets
{
	/// <summary>
	/// Summary description for BluetoothSocket.
	/// </summary>
	public class BluetoothSocket : Socket
	{
		private const int AddressFamilyBluetooth = 32;
		private const int ProtocolTypeRFCOMM  = 0x0003;
		private PlatformID m_platform;

		/// <summary>
		/// 
		/// </summary>
		public BluetoothSocket() : base((AddressFamily)32, SocketType.Stream, (ProtocolType)0x0003)
		{
			m_platform = System.Environment.OSVersion.Platform;			
		}
		
		
		
	}
}
