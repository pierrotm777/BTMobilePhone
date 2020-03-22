//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.Internal.BTHNS_INQUIRYBLOB
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
//using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net.Bluetooth.Internal
{
	/// <summary>
	/// This structure contains additional parameters for device inquiries.
	/// </summary>
	internal class BTHNS_INQUIRYBLOB : BTHNS_BLOB
	{

		public BTHNS_INQUIRYBLOB(short length, short responses)
		{
			m_data = new byte[8];

			Lap = 0x9e8b33;
			Length = length;
			Responses = responses;
		}

		/// <summary>
		/// LAP from which the inquiry access code is derived when the inquiry procedure is made.
		/// </summary>
		public uint Lap
		{
			get
			{
				return BitConverter.ToUInt32(m_data,0);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 0);
			}
		}
		/// <summary>
		/// Amount of time allowed to perform the query.
		/// This value is measured in units of 1.28 seconds (time to query=length*1.28 seconds).
		/// The default value is 16.
		/// </summary>
		public short Length
		{
			get
			{
				return BitConverter.ToInt16(m_data,4);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 4);
			}
		}
		/// <summary>
		/// Maximum number of devices to retrieve information about before stopping the inquiry. The default value is 16.
		/// </summary>
		public short Responses
		{
			get
			{
				return BitConverter.ToInt16(m_data,6);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 6);
			}
		}


		
	}
}
