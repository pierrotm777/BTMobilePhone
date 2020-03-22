using System;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net.Bluetooth.Internal
{
	/// <summary>
	/// Summary description for WSAQuerySet.
	/// </summary>
	internal class WSAQuerySet : IDisposable
	{
		public const int Length = 60;

		private byte[] m_data;
		private BLOB m_blob;

		private GCHandle m_blobHandle;

		/*public int dwSize;
		public IntPtr lpszServiceInstanceName;
		public IntPtr lpServiceClassId;
		public IntPtr lpVersion;
		public IntPtr lpszComment;
		public int dwNameSpace; 
		public IntPtr lpNSProviderId;
		public IntPtr lpszContext;
		public int dwNumberOfProtocols;
		public IntPtr lpafpProtocols;
		public IntPtr lpszQueryString;
		public int dwNumberOfCsAddrs;
		public IntPtr lpcsaBuffer;
		public int dwOutputFlags;
		public IntPtr lpBlob;*/

		public WSAQuerySet()
		{
			m_data = new byte[60];
			//write size
			BitConverter.GetBytes((int)60).CopyTo(m_data, 0);
			//set namespace
			BitConverter.GetBytes((int)16).CopyTo(m_data, 20);
		}

		public byte[] ToByteArray()
		{
			return m_data;
		}

		public void Dispose()
		{
			if(m_blobHandle.IsAllocated)
			{
				m_blobHandle.Free();
			}

			//free name
			IntPtr pName = (IntPtr)BitConverter.ToInt32(m_data, 4);
			if(pName != IntPtr.Zero)
			{
				MarshalEx.FreeHGlobal(pName);
			}
		}

		/// <summary>
		/// Returns name of device if specified in query.
		/// </summary>
		public string ServiceInstanceName
		{
			get
			{
				return Marshal.PtrToStringUni((IntPtr)BitConverter.ToInt32(m_data, 4));
			}
		}

		/// <summary>
		/// Identifies the services class in the form of a GUID.
		/// </summary>
		public Guid ServiceClassId
		{
			get
			{
				IntPtr pGuid = (IntPtr)BitConverter.ToInt32(m_data, 8);
				byte[] guidbytes = new byte[16];
				Marshal.Copy(pGuid, guidbytes, 0, 16);
				return new Guid(guidbytes);
			}
		}

		/// <summary>
		/// This context varies depending upon the function.
		/// </summary>
		public BTHNS_BLOB Blob
		{
			get
			{

				if(m_blob != null)
				{
					return m_blob.BlobData;
				}
				return null;
			}
			set
			{
				IntPtr addr = IntPtr.Zero;

				if(m_blob != null)
				{
					m_blob.Dispose();
					m_blob = null;
				}

				m_blob = new BLOB(value);

				//free old handle
				if(m_blobHandle.IsAllocated)
				{
					m_blobHandle.Free();
				}

				if(value != null)
				{
					//alloc new handle
					m_blobHandle = GCHandle.Alloc(m_blob.ToByteArray(), GCHandleType.Pinned);
					addr = m_blobHandle.AddrOfPinnedObject();
				}

				//write to structure
				if(System.Environment.OSVersion.Platform==PlatformID.WinCE)
				{
					BitConverter.GetBytes(addr.ToInt32() + 4).CopyTo(m_data, 56);
				}
				else
				{
					BitConverter.GetBytes(addr.ToInt32()).CopyTo(m_data, 56);
				}
				
			}
		}
	}
}
