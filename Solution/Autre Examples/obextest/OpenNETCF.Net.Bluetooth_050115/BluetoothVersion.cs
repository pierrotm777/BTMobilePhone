//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.BluetoothVersion
//		Copyright (C) 2003-2004, OpenNETCF.org
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

namespace OpenNETCF.Net.Bluetooth
{
	/// <summary>
	/// Summary description for BluetoothVersion.
	/// </summary>
	public class BluetoothVersion
	{
		private Version mHciVersion;
		private Version mLmpVersion;
		private int mManufacturer;
		private long mFeatures;

		internal BluetoothVersion(byte[] data)
		{
			byte hv = data[0];
			ushort hr = BitConverter.ToUInt16(data, 1);
			mHciVersion = new Version(hv, hr);
			byte lv = data[3];
			ushort ls = BitConverter.ToUInt16(data, 4);
			mLmpVersion = new Version(lv, ls);

			mManufacturer = BitConverter.ToUInt16(data, 6);
			mFeatures = BitConverter.ToInt64(data, 8);
		}
		internal BluetoothVersion(byte hciVersion, ushort hciRevision, byte lmpVersion, ushort lmpSubversion, ushort manufacturer, long features )
		{
			mHciVersion = new Version(hciVersion, hciRevision);
			mLmpVersion = new Version(lmpVersion, lmpSubversion);
			mManufacturer = manufacturer;
			mFeatures = features;
		}

		/// <summary>
		/// Version of the current Host Controller Interface (HCI) in the Bluetooth hardware.
		/// </summary>
		/// <remarks>This value changes only when new versions of the Bluetooth hardware are created for the new Bluetooth Special Interest Group (SIG) specifications.</remarks>
		public Version HciVersion
		{
			get
			{
				return mHciVersion;
			}
		}

		/// <summary>
		/// Version of the current Link Manager Protocol (LMP) in the Bluetooth hardware.
		/// </summary>
		public Version LmpVersion
		{
			get
			{
				return mLmpVersion;
			}
		}

		/// <summary>
		/// Name of the Bluetooth hardware manufacturer.
		/// </summary>
		public Manufacturer Manufacturer
		{
			get
			{
				return (Manufacturer)mManufacturer;
			}
		}

		public long Features
		{
			get
			{
				return mFeatures;
			}
		}

		public override string ToString()
		{
			return "Hci: " + HciVersion.ToString() + " Lmp: " + LmpVersion.ToString() + " Manufacturer: " + Manufacturer.ToString();
		}

	}
}
