//==========================================================================================
//
//		OpenNETCF.Multimedia.Audio.WaveHeader
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
using OpenNETCF.Win32;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Multimedia.Audio
{
	/// <summary>
	/// Internal wrapper around WAVEHDR
	/// Facilitates asynchronous operations
	/// </summary>
	internal class WaveHeader: IDisposable
	{
		private WaveHdr m_hdr;
		private IntPtr m_lpData;
		private int m_cbdata;
		private int m_cbHeader;
		private IntPtr m_lpHeader;


		public WaveHeader(byte[] data)
		{
			InitFromData(data, data.Length);
		}

		/// <summary>
		/// Creates WaveHeader and fills it with wave data
		/// <see cref="WaveHdr"/>
		/// </summary>
		/// <param name="data">wave data bytes</param>
		/// <param name="datalength">length of Wave data</param>
		public WaveHeader(byte[] data, int datalength)
		{
			InitFromData(data, datalength);
		}
		
		/// <summary>
		/// Constructor for WaveHeader class
		/// Allocates a buffer of required size
		/// </summary>
		/// <param name="BufferSize"></param>
		public WaveHeader(int BufferSize)
		{
			InitFromData(null, BufferSize);
		}

		internal void InitFromData(byte[] data, int datalength)
		{
			m_cbdata = datalength;
			m_lpData = MarshalEx.AllocHGlobal(m_cbdata);
			if ( data != null )
				Marshal.Copy(data, 0, m_lpData, m_cbdata);
			m_hdr = new WaveHdr((int)m_lpData.ToInt32(), m_cbdata);
			m_cbHeader = m_hdr.ToByteArray().Length;
			m_lpHeader = MarshalEx.AllocHGlobal(m_cbHeader);
			byte[] hdrbits = m_hdr.ToByteArray();
			Marshal.Copy(hdrbits, 0, m_lpHeader, m_cbHeader);
		}

		///<summary>Ptr to WAVEHDR in the unmanaged memory</summary>
		public IntPtr Header { get { return m_lpHeader; } }
		///<summary>Ptr to wave data in the unmanaged memory</summary>
		public IntPtr Data { get { return m_lpData; } }
		///<summary>Wave data size</summary>
		public int DataLength { get { return m_cbdata; } }
		public int HeaderLength { get { return m_cbHeader; } }
		public WaveHdr waveHdr { get { return m_hdr; } }
		public byte[] GetData()
		{
			byte [] data = new byte[m_cbdata];
			Marshal.Copy(m_lpData, data, 0, m_cbdata);
			return data;
		}
		public void RetrieveHeader()
		{
			byte[] headerBits = new byte[m_cbHeader];
			Marshal.Copy(m_lpHeader, headerBits, 0, m_cbHeader);
			m_hdr = new WaveHdr(headerBits);
		}
		#region IDisposable Members

		public void Dispose()
		{
			MarshalEx.FreeHGlobal(m_lpData);
			MarshalEx.FreeHGlobal(m_lpHeader);
		}

		#endregion
	}
}
