//==========================================================================================
//
//		OpenNETCF.Net.BluetoothAddress
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

namespace OpenNETCF.Net
{
	/// <summary>
	/// Represents a Bluetooth device address.
	/// </summary>
	public class BluetoothAddress
	{
		private byte[] m_data;

		/// <summary>
		/// Initializes a new instance of the <see cref="BluetoothAddress"/>.
		/// </summary>
		public BluetoothAddress()
		{
			m_data = new byte[8];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BluetoothAddress"/> class with the specified address.
		/// </summary>
		/// <param name="address"><see cref="Int64"/> representation of the address.</param>
		public BluetoothAddress(long address) : this()
		{
			//copy value to array
			BitConverter.GetBytes(address).CopyTo(m_data,0);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BluetoothAddress"/> class with the specified address.
		/// </summary>
		/// <param name="address">Address as 6 byte array.</param>
		public BluetoothAddress(byte[] address) : this()
		{
			if(address.Length == 6)
			{
				Buffer.BlockCopy(address, 0, m_data, 0, 6);
			}
			else
			{
				throw new ArgumentException();
			}
		}

		/// <summary>
		/// Converts the string representation of a Bluetooth address to a new <see cref="BluetoothAddress"/> instance.
		/// </summary>
		/// <param name="address">A string containing an address to convert.</param>
		/// <returns>New BluetoothAddress instance.</returns>
		/// <remarks>Address must be specified in hex format separated by the colon character e.g. 00:00:00:00:00:00, or as a long hex formatted integer.</remarks>
		public static BluetoothAddress Parse(string address)
		{
			BluetoothAddress ba;

			if(address.IndexOf(":") > -1)
			{
				//assume address in standard hex format 00:00:00:00:00:00
				ba = new BluetoothAddress();
				byte[] babytes = ba.ToByteArray();
				//split on colons
				string[] sbytes = address.Split(':');
				for(int ibyte = 0; ibyte < 6; ibyte++)
				{
					//parse hex byte in reverse order
					babytes[ibyte] = byte.Parse(sbytes[5 - ibyte],System.Globalization.NumberStyles.HexNumber);
				}
			}
			else
			{
				//assume specified as long integer
				ba = new BluetoothAddress(long.Parse(address, System.Globalization.NumberStyles.HexNumber));
			}

			return ba;
		}


		/// <summary>
		/// Returns the internal byte array
		/// </summary>
		/// <returns></returns>
		public byte[] ToByteArray()
		{
			return m_data;
		}

		/// <summary>
		/// Returns the Bluetooth address as a long integer.
		/// </summary>
		/// <returns></returns>
		public long ToInt64()
		{
			return BitConverter.ToInt64(m_data, 0);
		}

		/// <summary>
		/// Converts the address to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of this instance.</returns>
		public override string ToString()
		{
			string result = "";

			result += m_data[5].ToString("X2") + ":";
			result += m_data[4].ToString("X2") + ":";
			result += m_data[3].ToString("X2") + ":";
			result += m_data[2].ToString("X2") + ":";
			result += m_data[1].ToString("X2") + ":";
			result += m_data[0].ToString("X2");

			return result;
		}

		/// <summary>
		/// Provides a null Bluetooth address.
		/// This field is read-only.
		/// </summary>
		public static readonly BluetoothAddress None = new BluetoothAddress();

	}
}
