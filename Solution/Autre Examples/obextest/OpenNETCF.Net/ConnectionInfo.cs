/*=======================================================================================

	OpenNETCF.Net.ConnectionInfo
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
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Summary description for ConnectionInfo.
	/// </summary>
	internal struct ConnectionInfo
	{
		public int cbSize;
		public int dwParams;
		public int dwFlags;
		public int dwPriority;
		public bool bExclusive;
		public bool bDisabled;
		public Guid guidDestNet;
		public IntPtr hWnd;
		public int uMsg;
		public int lParam;
		int ulMaxCost;
		int ulMinRcvBw;
		int ulMaxConnLatency;
		internal IntPtr handle;

		/// <summary>
		/// Writes the ConnectionInfo data to unmanaged memory.
		/// </summary>
		/// <returns>A pointer to the unmanaged memory block storing the ConnectionInfo data</returns>		
		public IntPtr ToPtr()
		{			
//			ulMaxCost = 0;
//			ulMinRcvBw = 0;
//			ulMaxConnLatency = 0;

			IntPtr p = MarshalEx.AllocHLocal((uint)Marshal.SizeOf(typeof(ConnectionInfo)));

			MarshalEx.WriteInt32(p, 0, this.cbSize);
			MarshalEx.WriteInt32(p, 4, this.dwParams);
			MarshalEx.WriteInt32(p, 8, this.dwFlags);
			MarshalEx.WriteInt32(p, 12, this.dwPriority);
			MarshalEx.WriteBool(p, 16, this.bExclusive);
			MarshalEx.WriteBool(p, 20, this.bDisabled);
			MarshalEx.WriteByteArray(p, 24, this.guidDestNet.ToByteArray());
			MarshalEx.WriteIntPtr(p, 40, this.hWnd);
			MarshalEx.WriteInt32(p, 44, this.uMsg);
			MarshalEx.WriteInt32(p, 48, this.lParam);
			MarshalEx.WriteInt32(p, 52, this.ulMaxCost);
			MarshalEx.WriteInt32(p, 56, this.ulMinRcvBw);
			MarshalEx.WriteInt32(p, 60, this.ulMaxConnLatency);

			handle = p;
			return p;
		}

		/// <summary>
		/// Disposes of the ConnectionInfo object.
		/// </summary>
		public void Dispose()
		{
			this.hWnd = IntPtr.Zero;

			if(handle != IntPtr.Zero)
				MarshalEx.FreeHLocal(handle);
		}
	};
}
