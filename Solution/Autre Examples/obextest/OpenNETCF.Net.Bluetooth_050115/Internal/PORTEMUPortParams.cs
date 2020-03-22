//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.PORTEMUPortParams
//		Copyright (C) 2004, OpenNETCF.org
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
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.Bluetooth.Internal
{
	/// <summary>
	/// Used when creating a virtual serial port.
	/// </summary>
	internal class PORTEMUPortParams
	{
		/*int channel;
		int flocal;
		BD_ADDR device;10
		int imtu;
		int iminmtu;
		int imaxmtu;
		int isendquota;
		int irecvquota;
		GUID uuidService;16
		unsigned int uiportflags;*/

		private byte[] m_data;
		
		/// <summary>
		/// 
		/// </summary>
		public PORTEMUPortParams()
		{
			m_data = new byte[60];

			Flags = RFCommPortFlags.RemoteDCB;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		internal byte[] ToByteArray()
		{
			return m_data;
		}

		/// <summary>
		/// Set to either an explicit server channel, or, for a server application that wants the server channel to be autobound, to RFCOMM_CHANNEL_MULTIPLE.
		/// </summary>
		public int Channel
		{
			get
			{
				return BitConverter.ToInt32(m_data, 0);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 0);
			}
		}
		/// <summary>
		/// Set to TRUE for a server port that accepts connections, or to FALSE for a client port that is used to creating outgoing connections.
		/// </summary>
		public bool Local
		{
			get
			{
				return Convert.ToBoolean(BitConverter.ToInt32(m_data, 4));
			}
			set
			{
				BitConverter.GetBytes((int)(value ? 0 : 1)).CopyTo(m_data, 4);
			}
		}

		/// <summary>
		/// The address of a target device on a client port.
		/// </summary>
		public BluetoothAddress Address
		{
			get
			{
				BluetoothAddress ba = new BluetoothAddress();
				Buffer.BlockCopy(m_data, 8, ba.ToByteArray(), 0, 6);
				return ba;
			}
			set
			{
				byte[] addrbytes = value.ToByteArray();
				Buffer.BlockCopy(addrbytes, 0, m_data, 8, 3);
				Buffer.BlockCopy(addrbytes, 4, m_data, 12, 2);
				Buffer.BlockCopy(addrbytes, 6, m_data, 16, 1);

				//Buffer.BlockCopy(value.ToByteArray(), 0, m_data, 8, 6);

			}
		}

		public int Mtu
		{
			get
			{
				return BitConverter.ToInt32(m_data, 20);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 20);
			}
		}

		public int MinMtu
		{
			get
			{
				return BitConverter.ToInt32(m_data, 24);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 24);
			}
		}

		public int MaxMtu
		{
			get
			{
				return BitConverter.ToInt32(m_data, 28);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 28);
			}
		}

		public int SendQuota
		{
			get
			{
				return BitConverter.ToInt32(m_data, 32);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 32);
			}
		}

		public int ReceiveQuota
		{
			get
			{
				return BitConverter.ToInt32(m_data, 36);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 36);
			}
		}
		//etc


		/// <summary>
		/// Specifies the UUID for the target RFCOMM service.
		/// If channel == 0 for the client port, an SDP query is performed to determine the target channel id before the connection is made.
		/// </summary>
		public Guid Service
		{
			get
			{
				byte[] guidbytes = new byte[16];
				Buffer.BlockCopy(m_data, 40, guidbytes, 0, 16);
				return new Guid(guidbytes);
			}
			set
			{
				Buffer.BlockCopy(value.ToByteArray(), 0, m_data, 40, 16);

				/*if(p_uuid == IntPtr.Zero)
				{
					p_uuid = MarshalEx.AllocHGlobal(16);
					BitConverter.GetBytes(p_uuid.ToInt32()).CopyTo(m_data, 32);
				}

				Marshal.Copy(value.ToByteArray(), 0, p_uuid, 16);	*/
			}
		}

		/// <summary>
		/// Port Flags.
		/// </summary>
		public RFCommPortFlags Flags
		{
			get
			{
				return (RFCommPortFlags)BitConverter.ToInt32(m_data, 56);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 56);
			}
		}
	}

	[Flags()]
	internal enum RFCommPortFlags : int
	{
		RemoteDCB = 0x00000001,
		KeepDCD = 0x00000002, 
		Authenticate = 0x00000004, 
		Encrypt = 0x00000008, 
	}
}
