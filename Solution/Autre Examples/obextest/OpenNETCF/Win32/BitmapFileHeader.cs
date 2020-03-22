//==========================================================================================
//
//		OpenNETCF.Win32.BitmapFileHeader
//		Copyright (c) 2003, OpenNETCF.org
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

namespace OpenNETCF.Win32
{
	/// <summary>
	/// This structure contains information about the type, size, and layout of a file that containing a device-independent bitmap (DIB).
	/// </summary>
	/// <remarks>Wraps the native <b>BITMAPFILEHEADER</b> structure</remarks>
	public class BitmapFileHeader 
	{ 
		private byte[] data;

		public BitmapFileHeader()
		{
			data = new byte[14];
		}
		/// <summary>
		/// Specifies the file type. It must be BM.
		/// </summary>
		public short  Type
		{
			get { return BitConverter.ToInt16(data, 0); }
			set { BitConverter.GetBytes(value).CopyTo(data, 0); }
		}
		/// <summary>
		/// Specifies the size, in bytes, of the bitmap file.
		/// </summary>
		public int    Size
		{
			get { return BitConverter.ToInt32(data, 2); }
			set { BitConverter.GetBytes(value).CopyTo(data, 2); }
		}

		//public ushort  bfReserved1;
		//public ushort  bfReserved2; 
		/// <summary>
		/// Specifies the offset, in bytes, from the <b>BitmapFileHeader</b> structure to the bitmap bits.
		/// </summary>
		public int    OffBits
		{
			get { return BitConverter.ToInt32(data, 10); }
			set { BitConverter.GetBytes(value).CopyTo(data, 10); }
		}

		public byte[] Data { get { return data; } }
	}
}
