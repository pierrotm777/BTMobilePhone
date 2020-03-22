//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.Internal.BTHNS_RESTRICTIONBLOB
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

namespace OpenNETCF.Net.Bluetooth.Internal
{
	/// <summary>
	/// This structure contains details about a query restriction.
	/// </summary>
	internal class BTHNS_RESTRICTIONBLOB : BTHNS_BLOB
	{
		private const int length = 20;

		public BTHNS_RESTRICTIONBLOB()
		{
			m_data = new byte[length];
		}

		/// <summary>
		/// Type of search to perform.
		/// </summary>
		public SearchType Type
		{
			get
			{
				return (SearchType)BitConverter.ToUInt32(m_data, 0);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 0);
			}
		}

		/// <summary>
		/// Service handle on which to query the attributes in the pRange member.
		/// The serviceHandle member is used for attribute searches. 
		/// </summary>
		public uint ServiceHandle
		{
			get
			{
				return BitConverter.ToUInt32(m_data, 4);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 4);
			}
		}
		/// <summary>
		/// Used for service and service attribute searches.
		/// Specifies the UUIDs that a record must contain to match the search.
		/// If less than 12 UUIDs are to be queried, set the SdpQueryUuid element, immediately following the last valid UUID, to all zeros (Guid.Empty).
		/// </summary>
		public IntPtr Uuids
		{
			get
			{
				return (IntPtr)BitConverter.ToInt32(m_data, 8);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 8);
			}
		}

		/// <summary>
		/// Used for attribute and service attribute searches.
		/// Specifies the number of elements in pRange.
		/// </summary>
		public uint NumRange
		{
			get
			{
				return BitConverter.ToUInt32(m_data, 12);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 12);
			}
		}
		/// <summary>
		/// Used for attribute and service attribute searches.
		/// Specifies the attribute values to retrieve for any matching records.
		/// </summary>
		public IntPtr pRange
		{
			get
			{
				return (IntPtr)BitConverter.ToInt32(m_data, 16);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 16);
			}
		}

	}

	internal enum SearchType :uint
	{
		SDP_SERVICE_SEARCH_REQUEST           = 1,
		SDP_SERVICE_ATTRIBUTE_REQUEST        = 2,
		SDP_SERVICE_SEARCH_ATTRIBUTE_REQUEST = 3,
	}
}
