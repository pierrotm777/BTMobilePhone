using System;
using System.Net;
using System.Net.Sockets;
using OpenNETCF.Net.Bluetooth;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Establishes connections to a peer device and provides Bluetooth port information.
	/// </summary>
	public class BluetoothEndPoint : EndPoint
	{
		private BluetoothAddress m_id;
		private Guid m_service;
		private int m_port;
		private static int m_addressoffset;
		private static int m_serviceoffset;
		private static int m_portoffset;
		private static int m_salength;
		private static int m_defaultport;

		static BluetoothEndPoint()
		{
			//setup platform specific offsets
			if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
			{
				m_addressoffset = 8;
				m_serviceoffset = 16;
				m_portoffset = 32;
				m_salength = 40;
				m_defaultport = 0;
			}
			else
			{
				m_addressoffset = 2;
				m_serviceoffset = 10;
				m_portoffset = 26;
				m_salength = 30;
				m_defaultport = -1;
			}
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="BluetoothEndPoint"/> class with the specified address and service.
		/// </summary>
		/// <param name="address">The Bluetooth address of the device. A six byte array.</param>
		/// <param name="service">Guid representing the Bluetooth profile to use.</param>
		public BluetoothEndPoint(BluetoothAddress address, Guid service) : this(address, service, m_defaultport)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BluetoothEndPoint"/> class with the specified address, service and port number.
		/// </summary>
		/// <param name="address">The Bluetooth address of the device. A six byte array.</param>
		/// <param name="service">Guid representing the Bluetooth profile to use.</param>
		/// <param name="port">Radio channel to use, -1 for any.</param>
		public BluetoothEndPoint(BluetoothAddress address, Guid service, int port)
		{
			
			m_id = address;
			m_service = service;
			m_port = port;
		}

		/// <summary>
		/// Serializes endpoint information into a <see cref="SocketAddress"/> instance.
		/// </summary>
		/// <returns>A <see cref="SocketAddress"/> instance containing the socket address for the endpoint.</returns>
		public override SocketAddress Serialize()
		{
			SocketAddress btsa = new SocketAddress(AddressFamily.Unspecified, m_salength);
			//copy address type
			btsa[0] = 32;
			
			//copy device id
			if(m_id != null)
			{
				byte[] deviceidbytes = m_id.ToByteArray();

				for(int idbyte =0; idbyte < 6; idbyte++)
				{
					btsa[idbyte+m_addressoffset] = deviceidbytes[idbyte];
				}
			}

			//copy service clsid
			byte[] servicebytes = m_service.ToByteArray();
			for(int servicebyte = 0; servicebyte < 16; servicebyte++)
			{
				btsa[servicebyte+m_serviceoffset] = servicebytes[servicebyte];
			}
			
			//copy port
			byte[] portbytes = BitConverter.GetBytes(m_port);
			for(int portbyte = 0; portbyte < 4; portbyte++)
			{
				btsa[portbyte+m_portoffset] = portbytes[portbyte];
			}
			//BitConverter.GetBytes((int)m_port).CopyTo(btsa.ToByteArray(), 32);

			return btsa;
		}

		/// <summary>
		/// Creates an endpoint from a socket address.
		/// </summary>
		/// <param name="socketAddress">The <see cref="SocketAddress"/> to use for the endpoint.</param>
		/// <returns>An <see cref="EndPoint"/> instance using the specified socket address.</returns>
		public override EndPoint Create(SocketAddress socketAddress)
		{
			//if a Bluetooth SocketAddress
			if(socketAddress[0] == 32)
			{
				int ibyte;

				byte[] addrbytes = new byte[6];
				for(ibyte = 0; ibyte < 6; ibyte++)
				{
					addrbytes[ibyte] = socketAddress[m_addressoffset+ibyte];
				}
				
				byte[] servicebytes = new byte[16];
				for(ibyte = 0; ibyte < 16; ibyte++)
				{
					servicebytes[ibyte] = socketAddress[m_serviceoffset+ibyte];
				}

				byte[] portbytes = new byte[4];
				for(ibyte = 0; ibyte < 4; ibyte++)
				{
					portbytes[ibyte] = socketAddress[m_portoffset+ibyte];
				}
				
				return new BluetoothEndPoint(new BluetoothAddress(addrbytes), new Guid(servicebytes), BitConverter.ToInt32(portbytes, 0));
				
			}
			else
			{
				//use generic method
				return base.Create(socketAddress);
			}
		}

		/// <summary>
		/// Gets the address family of the Bluetooth address. 
		/// </summary>
		public override AddressFamily AddressFamily
		{
			get
			{
				return (AddressFamily)32;
			}
		}



		/// <summary>
		/// Gets or sets the Bluetooth address of the endpoint.
		/// </summary>
		public BluetoothAddress Address
		{
			get
			{
				return m_id;
			}
			set
			{
				m_id = value;
			}
		}

		/// <summary>
		/// Gets or sets the Bluetooth service to use for the connection.
		/// </summary>
		public Guid Service
		{
			get
			{
				return m_service;
			}
			set
			{
				m_service = value;
			}
		}

		/// <summary>
		/// Gets or sets the service channel number of the endpoint.
		/// </summary>
		public int Port
		{
			get
			{
				return m_port;
			}
			set
			{
				m_port = value;
			}
		}

		/// <summary>
		/// Specifies the minimum value that can be assigned to the Port property.
		/// </summary>
		public const int MinPort = 1;

		/// <summary>
		/// Specifies the maximum value that can be assigned to the Port property.
		/// </summary>
		public const int MaxPort = 0xffff;
	}
}
