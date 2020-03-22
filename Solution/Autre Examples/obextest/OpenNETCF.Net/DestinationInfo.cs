/*=======================================================================================

	OpenNETCF.Net.DestinationInfo
	Copyright © 2003, OpenNETCF.org

	This library is free software; you can redistribute it and/or modify it under 
	the terms of the OpenNETCF.org Shared Source License.

	This library is distributed in the hope that it will be useful, but 
	WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
	FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License 
	for more details.

	You should have received a copy of the OpenNETCF.org Shared Source License 
	along with this library; if not, email licensing@opennetcf.org to request a copy.

	If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please 
	email licensing@opennetcf.org.

	For general enquiries, email enquiries@opennetcf.org or visit our website at:
	http://www.opennetcf.org

=======================================================================================*/
using System;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Contains information about a specific network.
	/// </summary>
	public class DestinationInfo
	{
		private object syncRoot = new object();

		/// <summary>
		/// Size of the DestinationInfo structure in unmanaged memory.
		/// </summary>
		public static int NativeSize = 272;

		/// <summary>
		/// The destination's GUID identifier.
		/// </summary>
		public Guid guid;

		/// <summary>
		/// The destination's description.
		/// </summary>
		public string description = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DestinationInfo()
		{
		}

		/// <summary>
		/// Creates a new instance of DestinationInfo at the specific memory address.
		/// </summary>
		/// <param name="baseAddr">Memory address where the DestinationInfo object should be created.</param>
		public DestinationInfo(IntPtr baseAddr)
		{
			lock(syncRoot)
			{
				guid = new Guid(MarshalEx.ReadByteArray(baseAddr, 0, 16));
				description = MarshalEx.PtrToStringUni(baseAddr, 16, 256);
			}
		}
	}
}
