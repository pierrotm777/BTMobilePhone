//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.Internal.BTHNS_SETBLOB
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
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.Bluetooth.Internal
{
	/// <summary>
	/// This structure is passed to the BthNsSetService function through the lpqsRegInfo->lpBlob member and contains information on the new service.
	/// </summary>
	internal class BTHNS_SETBLOB : BTHNS_BLOB
	{

		private const int BTH_SDP_VERSION = 1;

		private GCHandle pVersionHandle;
		private GCHandle pRecordHandle;

		/* ULONG __RPC_FAR *pSdpVersion;
	ULONG __RPC_FAR *pRecordHandle;
	ULONG Reserved[ 4 ];
	ULONG fSecurity;
	ULONG fOptions;
	ULONG ulRecordLength;
	UCHAR pRecord[ 1 ];
	
		public IntPtr pRecordHandle;
		private uint fSecurity;
		private uint fOptions;
		public uint ulRecordLength;
		public byte pRecord;*/

		public BTHNS_SETBLOB(byte[] record)
		{
			//create data buffer
			m_data = new byte[36 + record.Length];
			pVersionHandle = GCHandle.Alloc(BTH_SDP_VERSION, GCHandleType.Pinned);
			pRecordHandle = GCHandle.Alloc((int)0, GCHandleType.Pinned);
			IntPtr vaddr = pVersionHandle.AddrOfPinnedObject();
			IntPtr haddr = pRecordHandle.AddrOfPinnedObject(); //MarshalEx.AllocHGlobal(4);//
			BitConverter.GetBytes((int)vaddr).CopyTo(m_data, 0);
			BitConverter.GetBytes((int)haddr).CopyTo(m_data, 4);
			BitConverter.GetBytes(record.Length).CopyTo(m_data, 32);

			//copy sdp record
			Buffer.BlockCopy(record, 0, m_data, 36, record.Length);
		}

		public int Handle
		{
			get
			{
				IntPtr pHandle = (IntPtr)BitConverter.ToInt32(m_data, 4);
				return Marshal.ReadInt32(pHandle);
				//return m_handle;	
			}
			set
			{
				IntPtr pHandle = (IntPtr)BitConverter.ToInt32(m_data, 4);
				Marshal.WriteInt32(pHandle, value);
			}
		}

		#region IDisposable Members

		protected override void Dispose(bool disposing)
		{
			if(pVersionHandle.IsAllocated)
			{
				pVersionHandle.Free();
			}

			if(pRecordHandle.IsAllocated)
			{
				pRecordHandle.Free();
			}

			base.Dispose(disposing);
		}
		#endregion
	}
}
