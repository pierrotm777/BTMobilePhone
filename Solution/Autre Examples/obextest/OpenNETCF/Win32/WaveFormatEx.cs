//==========================================================================================
//
//		OpenNETCF.Win32.WaveFormatEx
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
	/// This class defines the format of waveform-audio data. 
	/// Only format information common to all waveform-audio data formats is included in this class. 
	/// For formats that require additional information, this class is included 
	/// as the first member in another class, along with the additional information
	/// </summary>
	/// <remarks>Equivalent to native <b>WAVEFORMATEX</b> structure.</remarks>
	public sealed class WaveFormatEx
	{
		public Int16 FormatTag;      
		public Int16 Channels;       
		public Int32 SamplesPerSec;  
		public Int32 AvgBytesPerSec; 
		public Int16 BlockAlign;     
		public Int16 BitsPerSample;  
		public Int16 Size;          
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public WaveFormatEx()
		{
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public WaveFormatEx(byte[] data)
		{
			if ( data.Length < 18 )
				throw new InvalidOperationException("Not enough data");
			FormatTag = BitConverter.ToInt16(data, 0);
			Channels =  BitConverter.ToInt16(data, 2);
			SamplesPerSec = BitConverter.ToInt32(data, 4);
			AvgBytesPerSec = BitConverter.ToInt32(data, 8);
			BlockAlign = BitConverter.ToInt16(data, 12);
			BitsPerSample = BitConverter.ToInt16(data, 14);
			Size = BitConverter.ToInt16(data, 16);
		}

		/// <summary>
		/// Get bytes
		/// </summary>
		/// <returns>byte array representation of this instance</returns>
		public byte[] GetBytes()
		{
			byte[] data = new byte[18];

			BitConverter.GetBytes(FormatTag).CopyTo(data, 0);
			BitConverter.GetBytes(Channels).CopyTo(data, 2);
			BitConverter.GetBytes(SamplesPerSec).CopyTo(data, 4);
			BitConverter.GetBytes(AvgBytesPerSec).CopyTo(data, 8);
			BitConverter.GetBytes(BlockAlign).CopyTo(data, 12);
			BitConverter.GetBytes(BitsPerSample).CopyTo(data, 14);
			BitConverter.GetBytes(Size).CopyTo(data, 16);

			return data;
		}
	}
}
