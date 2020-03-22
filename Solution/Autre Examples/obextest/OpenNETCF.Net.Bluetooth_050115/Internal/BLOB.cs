//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.Internal.BLOB
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
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.Bluetooth.Internal
{
	/// <summary>
	/// This structure is derived from a binary large object (BLOB) and contains information about a block of data.
	/// </summary>
	internal class BLOB : IDisposable
	{
		private byte[] m_data;

		private GCHandle m_blobhandle;
		private BTHNS_BLOB m_blob;

		public BLOB(BTHNS_BLOB blob)
		{
			m_data = new byte[8];

			m_blob = blob;
			m_blobhandle = GCHandle.Alloc(blob.ToByteArray(), GCHandleType.Pinned);
			
			//write to the byte array
			BitConverter.GetBytes(m_blob.Length).CopyTo(m_data, 0);
			int memloc = 0;
			if(System.Environment.OSVersion.Platform==PlatformID.WinCE)
			{
				memloc = m_blobhandle.AddrOfPinnedObject().ToInt32() + 4;
			}
			else
			{
				memloc = m_blobhandle.AddrOfPinnedObject().ToInt32();
			}

			BitConverter.GetBytes(memloc).CopyTo(m_data, 4);
		}

		public int Size
		{
			get
			{
				return BitConverter.ToInt32(m_data, 0);
			}
		}

		public BTHNS_BLOB BlobData
		{
			get
			{
				return m_blob;
			}
		}

		public byte[] ToByteArray()
		{
			return m_data;
		}

		#region IDisposable Members

		protected void Dispose(bool disposing)
		{
			if(m_blobhandle.IsAllocated)
			{
				m_blobhandle.Free();
			}

			if(disposing)
			{
				m_blob = null;
				m_data = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~BLOB()
		{
			Dispose(false);
		}

		#endregion
	}
}
