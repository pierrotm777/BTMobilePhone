//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.BluetoothSocketAddress
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
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace OpenNETCF.Net.Bluetooth
{
	/// <summary>
	/// Summary description for BluetoothSocketAddress.
	/// </summary>
	public class BluetoothSocketAddress : SocketAddress
	{
		//USHORT addressFamily; //2
		//bt_addr btAddr; //6 b8-b13
		//GUID serviceClassId; //16
		//ULONG port; //4

		//private byte[] m_data;
		//public const int Length = 40;
		private const ushort AF_BTH = 32;

		/// <summary>
		/// Creates a new empty <see cref="BluetoothSocketAddress"/>.
		/// </summary>
		public BluetoothSocketAddress() : base(AddressFamily.Unknown, 40)
		{
			byte[] af = BitConverter.GetBytes(AF_BTH);
			//copy to underlying data
			this[0] = af[0];
			this[1] = af[1];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		internal BluetoothSocketAddress(byte[] data) : base(AddressFamily.Unknown, 40)
		{
			if(data.Length==this.Size)
			{
				for(int ibyte = 0; ibyte<this.Size; ibyte++)
				{
					this[ibyte] = data[ibyte];
				}
			}
			else
			{
				throw new ArgumentOutOfRangeException("Data length not expected");
			}
		}

		/*public byte[] 
		{
			return m_data;
		}*/

		internal byte[] ToByteArray()
		{
			return (byte[])this.GetType().GetField("m_Buffer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string result = "";

			result += this[13].ToString("X") + ":";
			result += this[12].ToString("X") + ":";
			result += this[11].ToString("X") + ":";
			result += this[10].ToString("X") + ":";
			result += this[9].ToString("X") + ":";
			result += this[8].ToString("X");

			return result;
		}


		/// <summary>
		/// Target device address.
		/// </summary>
		public BluetoothAddress Address
		{
			get
			{
				byte[] addrbuffer = new byte[6];
				for(int addrbyte = 0; addrbyte < 6; addrbyte++)
				{
					addrbuffer[addrbyte] = this[addrbyte+2];
				}
				return new BluetoothAddress(addrbuffer);
			}
		}

		/// <summary>
		/// The GUID for the RFCOMM service.
		/// </summary>
		public Guid Service
		{
			get
			{
				byte[] service = new byte[16];
				Buffer.BlockCopy(this.ToByteArray(),16, service, 0, 16);
				return new Guid(service);
			}
		}

		/// <summary>
		/// Service channel number or zero.
		/// </summary>
		public int Channel
		{
			get
			{
				return BitConverter.ToInt32(this.ToByteArray(), 32);
			}
		}
	}
}
