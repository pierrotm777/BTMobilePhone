//==========================================================================================
//
//		OpenNETCF.Win32.DibSection
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
	/// Wrapper for WIN32 <b>DIBSECTION</b> structure
	/// </summary>
	public class DibSection 
	{
		/// <summary>
		/// Specifies the bitmap type; set to zero.
		/// </summary>
		public int       bmType;
		/// <summary>
		/// Specifies the width, in pixels, of the bitmap.
		/// The width must be greater than zero.
		/// </summary>
		public int       bmWidth;
		/// <summary>
		/// Specifies the height, in pixels, of the bitmap.
		/// The height must be greater than zero.
		/// </summary>
		public int       bmHeight;
		/// <summary>
		/// Specifies the number of bytes in each scan line.
		/// This value must be divisible by 2, because the system assumes that the bit values of a bitmap form an array that is word aligned.
		/// </summary>
		public int       bmWidthBytes;
		public int      bmPlanesAndbmBitsPixel;
		/// <summary>
		/// Pointer to the location of the bit values for the bitmap.
		/// The bmBits member must be a long pointer to an array of character (1-byte) values.
		/// </summary>
		public IntPtr    bmBits;
		public int      biSize;
		public int       biWidth;
		public int       biHeight;
		public int      biPlanesAndBitCount;
		public int      biCompression;
		public int       biSizeImage;
		public int       biXPelsPerMeter;
		public int       biYPelsPerMeter;
		public int      biClrUsed;
		public int      biClrImportant;
		public int      dsBitfields0;
		public int      dsBitfields1;
		public int      dsBitfields2;
		public IntPtr    dshSection;
		public int      dsOffset;
	}
}
