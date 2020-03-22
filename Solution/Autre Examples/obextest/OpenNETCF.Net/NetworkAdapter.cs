//==========================================================================================
//
//		OpenNETCF.Net.NetworkAdapter
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
using System.Runtime.InteropServices;
using System.Text;
using OpenNETCF.Win32;
using System.Collections;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Exposes methods for network adapter identification
	/// </summary>
	[Obsolete("",true)]
	public class NetworkAdapter
	{
		[DllImport("iphlpapi", EntryPoint="GetAdaptersInfo", SetLastError=true)]
		extern private static int GetAdaptersInfoCE(IntPtr p, ref int cb);

		/// <summary>
		/// Get an array of available network adapters
		/// </summary>
		/// <returns>aArray of AdapterInfo classes</returns>
		public static AdapterInfo[] GetAdaptersInfo()
		{
			ArrayList adapters = new ArrayList();

			int cb = 0;
			int ret = GetAdaptersInfoCE(IntPtr.Zero, ref cb);
			
			IntPtr pInfo = Core.LocalAlloc(Core.MemoryAllocFlags.LPtr, cb); //LPTR

			ret = GetAdaptersInfoCE(pInfo, ref cb);
			if ( ret == 0 )
			{
				AdapterInfo info = new AdapterInfo(pInfo, 0);
				while ( info != null )
				{
					adapters.Add(info);
					info = info.Next;
				}
			}
			Core.LocalFree(pInfo);

			return (AdapterInfo[])adapters.ToArray(Type.GetType("OpenNETCF.Net.AdapterInfo"));
		}
	}

	/// <summary>
	/// Class that provides information about a specific network adapter
	/// </summary>
	[Obsolete("OpenNETCF.Net.AdapterInfo is deprecated. Please use OpenNETCF.Net.Adapter",true)]
	public class AdapterInfo
	{
		#region Constructors

		private IntPtr ptr = IntPtr.Zero;
		private byte[] data = new byte[640];

		internal AdapterInfo(IntPtr pData, int offset)
		{
			this.ptr = new IntPtr(pData.ToInt32() + offset);
			MarshalEx.Copy(this.ptr, data, 0, 640);
		}
		#endregion

		#region Properties
		internal AdapterInfo Next
		{
			get 
			{ 
				if ( MarshalEx.ReadInt32(ptr, 0) == 0 ) 
					return null;
				return new AdapterInfo(new IntPtr(MarshalEx.ReadInt32(ptr, 0)), 0);
			}
		}
		
		internal uint ComboIndex
		{
			get { return MarshalEx.ReadUInt32(ptr, 4); }
		}

		/// <summary>
		/// The adapter name
		/// </summary>
		public string AdapterName
		{
			get { return MarshalEx.PtrToStringAnsi(this.ptr,8,268-8); }
		}

		/// <summary>
		/// Adapter description
		/// </summary>
		public string Description
		{
			get { return MarshalEx.PtrToStringAnsi(this.ptr,268,400-268); }
		}

		internal uint AddressLength
		{
			get { return MarshalEx.ReadUInt32(this.ptr, 400); }
		}
		
		/// <summary>
		/// Adapter's MAC address
		/// </summary>
		public byte[] MACAddress
		{
			get { return MarshalEx.ReadByteArray(this.ptr, 404, (int)AddressLength); }
		}
		
		/// <summary>
		/// Index of the adapter in the list of available adapters
		/// </summary>
		[CLSCompliant(false)]
		public uint Index
		{
			get { return MarshalEx.ReadUInt32(this.ptr, 412); }
		}

		/// <summary>
		/// Adapter type
		/// </summary>
		[CLSCompliant(false)]
		public uint Type
		{
			get { return MarshalEx.ReadUInt32(this.ptr, 416); }
		}


		/// <summary>
		/// Is DHCP enabled for this adapter?
		/// </summary>
		public bool DhcpEnabled
		{
			get { return (MarshalEx.ReadBool(this.ptr, 420)); }
		}

		/// <summary>
		/// Current IP address of the adapter
		/// </summary>
		public string CurrentIpAddress
		{
			get 
			{ 
				IntPtr p = new IntPtr( MarshalEx.ReadInt32(this.ptr, 424) );
				if ( p == IntPtr.Zero )
					return null;

				return new IP_ADDR_STRING( p, 0 ).IpAddress.String;
			}
		}

		internal IP_ADDR_STRING IpAddressList
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 428);
			}
		}

		/// <summary>
		/// Gateway assigned to the adapter
		/// </summary>
		public string Gateway
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 468).IpAddress.String;
			}
		}

		/// <summary>
		/// IP address of the DHCP server
		/// </summary>
		public string DhcpServer
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 508).IpAddress.String;
			}
		}

		/// <summary>
		/// Does this adapter use WINS?
		/// </summary>
		public bool HasWins
		{
			get { return (MarshalEx.ReadBool(this.ptr,548)); }
		}

		/// <summary>
		/// IP address of the primary WINS server for this adapter
		/// </summary>
		public string PrimaryWinsServer
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 552).IpAddress.String;
			}
		}

		/// <summary>
		/// IP address of the primary WINS server for this adapter
		/// </summary>
		public string SecondaryWinsServer
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 592).IpAddress.String;
			}
		}

		/// <summary>
		/// Time when current DHCP lease was obtained
		/// </summary>
		public DateTime LeaseObtained
		{
			get { return MarshalEx.Time_tToDateTime(MarshalEx.ReadUInt32(this.ptr, 632)); }
		}

		/// <summary>
		/// Time when current DHCP lease expires
		/// </summary>
		public DateTime LeaseExpires
		{
			get 
			{ return MarshalEx.Time_tToDateTime(MarshalEx.ReadUInt32(this.ptr, 636)); }
		}
		#endregion
	}

	/// <summary>
	/// Class IP_ADDR_STRING 
	/// Description:
	/// Implementation of custom marshaller for IPHLPAPI IP_ADDR_STRING
	/// </summary>
	internal class IP_ADDR_STRING
	{
		#region Contructors

		private IntPtr ptr = IntPtr.Zero;
		private byte[] data = new byte[40];
		private int baseOffset = 0;

		public IP_ADDR_STRING()
		{
		}
		public IP_ADDR_STRING(byte[] data, int offset)
		{
			this.data = data;
			this.ptr = new IntPtr(ptr.ToInt32() + offset);
		}
		public IP_ADDR_STRING(IntPtr pData, int offset)
		{
			this.ptr = new IntPtr( pData.ToInt32() + offset );
			Marshal.Copy(this.ptr, data, 0, 40);
		}
		#endregion

		#region Properties
		public IP_ADDR_STRING Next
		{
			get 
			{ 
				if ( MarshalEx.ReadInt32(this.ptr, 0) == 0 ) 
					return null;
				return new IP_ADDR_STRING(new IntPtr( MarshalEx.ReadInt32(this.ptr, 0) ), 0); 
			}
		}
		public IP_ADDRESS_STRING IpAddress
		{
			get { return new IP_ADDRESS_STRING(data, baseOffset + 4); }

		}
		public IP_ADDRESS_STRING IpMask
		{
			get { return new IP_ADDRESS_STRING(data, baseOffset + 20); }
		}
		public uint Context
		{
			get { return MarshalEx.ReadUInt32(this.ptr,36); }
			set { MarshalEx.WriteUInt32(this.ptr,36, value); }
		}
		#endregion
	}

	/// <summary>
	/// Class IP_ADDRESS_STRING 
	/// Description:
	/// Implementation of custom marshaller for IPHLPAPI IP_ADDRESS_STRING
	/// </summary>
	internal class IP_ADDRESS_STRING
	{
		#region Contructors

		private IntPtr ptr = IntPtr.Zero;
		private byte[] data = new byte[16];
		private int baseOffset = 0;

		public IP_ADDRESS_STRING()
		{
		}
		public IP_ADDRESS_STRING(byte[] data, int offset)
		{
			this.data = data;
			this.baseOffset = offset;
		}
		public IP_ADDRESS_STRING(IntPtr pData, int offset)
		{
			this.ptr = new IntPtr( pData.ToInt32() + offset );
			Marshal.Copy(this.ptr, data, 0, 16);
		}
		#endregion

		#region Properties
		public string String
		{
			get { return MarshalEx.PtrToStringAnsi(this.ptr, 0, 15 ); }
			set { System.Text.Encoding.Unicode.GetBytes(value, 0, value.Length, data, this.baseOffset); }
		}
		#endregion
	}
}
