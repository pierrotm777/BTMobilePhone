/*=======================================================================================

    OpenNETCF.Net
    Copyright 2003, OpenNETCF.org

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
using System.Data;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using OpenNETCF.Win32;
using OpenNETCF.IO;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net
{
	#region -------------- Internal-only Classes --------------

	internal class NDISUIO_QUERY_OID	
	{
		protected byte[]	data;
		protected int		ourSize;
		public int Size
		{
			get { return ourSize; }
		}

		protected const int NDISUIO_QUERY_OID_SIZE = 12;
		public NDISUIO_QUERY_OID( int extrasize )
		{
			// Most of the cases we'll use will have a size
			// of just sizeof( DWORD ), but you never know.
			ourSize = NDISUIO_QUERY_OID_SIZE + extrasize;
			data = new byte[ ourSize ];
		}

		protected const int OidOffset = 0;
		public uint Oid
		{
			get { return BitConverter.ToUInt32( data, OidOffset ); }
			set 
			{ 
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, data, OidOffset, 4 );
			}
		}

		protected const int ptcDeviceNameOffset = OidOffset + 4;
		public unsafe byte *ptcDeviceName
		{
			get 
			{ 
				return (byte*)BitConverter.ToUInt32( data, ptcDeviceNameOffset ); 
			}
			set 
			{ 
				byte[]	bytes = BitConverter.GetBytes( (UInt32)value );
				Buffer.BlockCopy( bytes, 0, data, ptcDeviceNameOffset, 4 );
			}
		}

		protected const int DataOffset = ptcDeviceNameOffset + 4;
		public byte[] Data
		{
			get
			{
				byte[]	b = new byte[ ourSize - DataOffset ];
				Array.Copy( data, DataOffset, b, 0, ourSize - DataOffset );
				return b;
			}
		}

		public byte[] getBytes()
		{
			return data;
		}

		public static implicit operator byte[](NDISUIO_QUERY_OID qoid)
		{
			return qoid.data;
		}
	}

	internal class IP_ADAPTER_INFO
	{
		protected byte[]	data;
		protected uint		firstnextIndex = 0;
		protected uint		firstnextOffset = 0;
		protected uint		ourSize = 0;
		protected uint		ourBase = 0;

		// Main constructor.  This figures out how much space
		// is needed to hold the list of adapters, allocates
		// a byte array for that space, and gets the list
		// from GetAdaptersInfo().
		unsafe public IP_ADAPTER_INFO()
		{
			// Find out how much space we need to store the
			// adapter list.
			int	size = 0;
			int	err = AdapterPInvokes.GetAdaptersInfo( null, ref size ); 
			if ( err != 0 )
			{
				// This is what we'd expect: there is not enough room in the
				// buffer, so the size is set and an error is returned.
				if ( err == 111 )
				{
					// ToDo: Handle buffer-too-small.
				}
			}

			ourSize = (uint)size;
			data = new byte[ size ];

			// We need to lock this in memory until we can
			// get its address.  Since GetAdaptersInfo will
			// be storing Next pointers from adapter information
			// to adapter information, we need to know what
			// the base for those pointers is.  We can then
			// calculate the offset into the byte array of
			// IP_ADAPTER_INFO from that.
			// Fix the data array in memory.  We need to do
			// this to store the base address of the array.
			// The GetAdaptersInfo call will put various Next
			// pointers in the structure and we need to know
			// what the base address against which those are
			// measured is.  With that, we can figure out what
			// offset in the data array they reference.
			fixed( byte *b = &data[ 0 ] )
			{
				// Save the base address.
				ourBase = (uint)b;

				// Actually call GetAdaptersInfo.
				int	siz = (int)ourSize;
				err = AdapterPInvokes.GetAdaptersInfo( data, ref siz );
			}

			if ( err != 0 )
				data = null;
		}

		protected const int IP_ADAPTER_INFO_SIZE = 640;
		protected IP_ADAPTER_INFO( byte[] datain, uint offset )
		{
			// Create an internal-only copy of this structure,
			// making it easy to get to the fields of one
			// of the items in the linked list, based on its
			// offset within the byte[] of the main
			// instance of an IP_ADAPTER_INFO.
			ourSize = IP_ADAPTER_INFO_SIZE;
			data = new byte[ IP_ADAPTER_INFO_SIZE ];
			Array.Copy( datain, (int)offset, data, 0, IP_ADAPTER_INFO_SIZE );
		}

		public Adapter FirstAdapter()
		{
			if ( data == null )
				return null;

			// Reset the indexing.
			firstnextIndex = 0;
			firstnextOffset = this.Next - ourBase;

			// Since we are creating this adapter based on
			// the first entry in our table, we can just pass
			// 'this' to do it.
			return new Adapter( this );
		}
		public Adapter NextAdapter()
		{
			// Starting at the current offset in our 'data'
			// member, get the Next field, subtrace its pointer
			// from the initial pointer value to find the
			// new offset, and create an adapter starting
			// at that point in the 'data' member.

			if ( data == null )
				return null;

			// Handle no more case.
			if ( firstnextOffset == 0 )
				return null;

			IP_ADAPTER_INFO	newinfo = new IP_ADAPTER_INFO( data, 
				firstnextOffset );

			firstnextIndex++;

			// Now, use the Next field of the new info 
			// structure to update where we find the next
			// one after that in our list.
			if ( newinfo.Next == 0 )
				firstnextOffset = 0;
			else
				firstnextOffset += newinfo.Next - ourBase;

			return new Adapter( newinfo );
		}

		// Accessors for fields of the item.
		protected const int NextOffset = 0;
		public uint Next
		{
			get { return BitConverter.ToUInt32( data, NextOffset ); }
		}

		protected const int ComboIndexOffset = NextOffset + 4;
		public int ComboIndex
		{
			get { return BitConverter.ToInt32( data, ComboIndexOffset ); }
		}

		protected const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;
		protected const int MAX_ADAPTER_NAME_LENGTH = 256;
		protected const int MAX_ADAPTER_ADDRESS_LENGTH = 8;

		protected const int AdapterNameOffset = ComboIndexOffset + 4;
		public String AdapterName
		{
			get { return Encoding.ASCII.GetString(data, AdapterNameOffset, MAX_ADAPTER_NAME_LENGTH).TrimEnd('\0'); }
		}

		protected const int DescriptionOffset = AdapterNameOffset + MAX_ADAPTER_NAME_LENGTH + 4;
		public String Description
		{
			get { return Encoding.ASCII.GetString(data, DescriptionOffset, MAX_ADAPTER_DESCRIPTION_LENGTH).TrimEnd('\0'); }
		}

		protected const int PhysAddressLengthOffset = DescriptionOffset + MAX_ADAPTER_DESCRIPTION_LENGTH + 4;
		public int PhysAddressLength
		{
			get { return BitConverter.ToInt32( data, PhysAddressLengthOffset ); }
		}

		protected const int PhysAddressOffset = PhysAddressLengthOffset + 4;
		public byte[] PhysAddress
		{
			get 
			{ 
				byte[]	b = new byte[ MAX_ADAPTER_ADDRESS_LENGTH ];
				Array.Copy( data, PhysAddressOffset, b, 0, MAX_ADAPTER_ADDRESS_LENGTH ); 
				return b;
			}
		}

		protected const int IndexOffset = PhysAddressOffset + MAX_ADAPTER_ADDRESS_LENGTH;
		public int Index
		{
			get { return BitConverter.ToInt32( data, IndexOffset ); }
		}

		protected const int TypeOffset = IndexOffset + 4;
		public int Type
		{
			get { return BitConverter.ToInt32( data, TypeOffset ); }
		}

		protected const int DHCPEnabledOffset = TypeOffset + 4;
		public bool DHCPEnabled
		{
			get { return BitConverter.ToBoolean( data, DHCPEnabledOffset ); }
		}

		protected const int CurrentIpAddressOffset = DHCPEnabledOffset + 4;
		public String CurrentIpAddress
		{
			// The CurrentIpAddress field is a pointer to an 
			// IP_ADDRESS_STRING structure, not a string itself,
			// so we have to do some magic to make this work.
			get 
			{ 
				IntPtr p = new IntPtr( BitConverter.ToInt32( data, CurrentIpAddressOffset) );
				if ( p == IntPtr.Zero )
					return null;

				// Here, I'm going to extract the 16 bytes of
				// the IP address string from the data pointed
				// to by the CurrentIpAddress pointer.  The
				// IP address part of what's pointed to starts
				// at offset 4 from the pointer value (skip the
				// Next field).  From there, we just copy 16
				// bytes, the length of the IP address string,
				// to a local byte array, which can easily be
				// converted to a managed string below.
				byte[]	b = new byte[ 16 ];
				IntPtr	p4 = new IntPtr( p.ToInt32()+4 );
				Marshal.Copy( p4, b, 0, 16 );

				// The string itself is stored after the Next
				// field in the IP_ADDRESS_STRING structure
				// (offset 4).
				return Encoding.ASCII.GetString(b, 0, 16).TrimEnd('\0'); 
			}
		}

		// The current subnet mask is part of the same
		// IP_ADDRESS_STRING that contains the CurrentIpAddress.
		// We simply extract a different piece of that 
		// structure to get it.s
		public String CurrentSubnetMask
		{
			// The CurrentIpAddress field is a pointer to an 
			// IP_ADDRESS_STRING structure, not a string itself,
			// so we have to do some magic to make this work.
			get 
			{ 
				IntPtr p = new IntPtr( BitConverter.ToInt32( data, CurrentIpAddressOffset) );
				if ( p == IntPtr.Zero )
					return null;

				// Here, I'm going to extract the 16 bytes of
				// the subnet address string from the data pointed
				// to by the CurrentIpAddress pointer.  The
				// mask address part of what's pointed to starts
				// at offset 4+16 from the pointer value (skip 
				// the Next field and the IP address field, 
				// which has length 16).  From there, we just 
				// copy 16 bytes, the length of the mask 
				// string, to a local byte array, which can 
				// easily be converted to a managed string 
				// below.
				byte[]	b = new byte[ 16 ];
				IntPtr	p4 = new IntPtr( p.ToInt32()+4+16 );
				Marshal.Copy( p4, b, 0, 16 );

				// The string itself is stored after the Next
				// and IpAddresss fields in the 
				// IP_ADDRESS_STRING structure (offset 4 + 16).
				return Encoding.ASCII.GetString(b, 0, 16).TrimEnd('\0'); 
			}
		}

		protected const int IpAddressListOffset = CurrentIpAddressOffset + 4;
#if notdefined
		public string IpAddressList
		{
			get
			{
				return null;	// ????
			}
		}
#endif

		// The offset is the start of the address list plus the
		// size of the IP_ADDRESS_STRING structure which includes
		// the Next, IpAddress, IpMask, and Context fields.
		protected const int GatewayListOffset = IpAddressListOffset + 4 + 16 + 16 + 4;
		public String Gateway
		{
			// The GatewayList field is a structure of type
			// IP_ADDRESS_STRING.  We have to extract the bits
			// we want.
			get 
			{ 
				// The string itself is stored after the Next
				// field in the IP_ADDRESS_STRING structure
				// (offset 4).
				return Encoding.ASCII.GetString(data, GatewayListOffset + 4, 16).TrimEnd('\0'); 
			}
		}

		protected const int DHCPServerOffset = GatewayListOffset + 4 + 16 + 16 + 4;
		public String DHCPServer
		{
			// The DhcpServer field is a structure of type
			// IP_ADDRESS_STRING.  We have to extract the bits
			// we want.
			get 
			{ 
				// The string itself is stored after the Next
				// field in the IP_ADDRESS_STRING structure
				// (offset 4).
				return Encoding.ASCII.GetString(data, DHCPServerOffset + 4, 16).TrimEnd('\0'); 
			}
		}

		protected const int HaveWINSOffset = DHCPServerOffset + 4 + 16 + 16 + 4;
		public bool HaveWINS
		{
			get { return BitConverter.ToBoolean( data, HaveWINSOffset ); }
		}

		protected const int PrimaryWINSServerOffset = HaveWINSOffset + 4;
		public String PrimaryWINSServer
		{
			get { return Encoding.ASCII.GetString(data, PrimaryWINSServerOffset+4, 16).TrimEnd('\0'); }
		}

		protected const int SecondaryWINSServerOffset = PrimaryWINSServerOffset + 4 + 16 + 16 + 4;
		public String SecondaryWINSServer
		{
			get { return Encoding.ASCII.GetString(data, SecondaryWINSServerOffset+4, 16).TrimEnd('\0'); }
		}

		protected const int LeaseObtainedOffset = SecondaryWINSServerOffset + 4 + 16 + 16 + 4;
		public DateTime LeaseObtained
		{
			get { return MarshalEx.Time_tToDateTime(BitConverter.ToUInt32( data, LeaseObtainedOffset)); }
		}

		protected const int LeaseExpiresOffset = LeaseObtainedOffset + 4;
		public DateTime LeaseExpires
		{
			get { return MarshalEx.Time_tToDateTime(BitConverter.ToUInt32( data, LeaseExpiresOffset)); }
		}

		/*
		IP_ADDR_STRING IpAddressList;
		*/

		public static implicit operator byte[](IP_ADAPTER_INFO ipinfo)
		{
			return ipinfo.data;
		}
	}

	#endregion

	/// <summary>
	/// Class representing a single instance of a network
	/// adapter, which might include PCMCIA cards, USB network
	/// cards, built-in Ethernet chips, etc.
	/// </summary>
	public class Adapter
	{
		internal String	name;
		/// <summary>
		/// The NDIS/driver assigned adapter name.
		/// </summary>
		public String	Name
		{
			get { return name; }
		}
		internal String	description;
		/// <summary>
		/// The descriptive name of the adapter.
		/// </summary>
		public String	Description
		{
			get { return description; }
		}
		internal int	index;
		/// <summary>
		/// The index in NDIS' list of adapters where this
		/// adapter is found.
		/// </summary>
		public int	Index
		{
			get { return index; }
		}
		internal int	type;
		/// <summary>
		/// The adapter type.  Adapters can be standard
		/// Ethernet, RF Ethernet, loopback, dial-up, etc.
		/// </summary>
		public AdapterType	Type
		{
			get { return (AdapterType)type; }
		}
		internal byte[]	hwaddress;
		/// <summary>
		/// The hardware address associated with the adapter.
		/// For Ethernet-based adapters, including RF Ethernet
		/// adapters, this is the Ethernet address.
		/// </summary>
		public byte[]	MacAddress
		{
			get { return hwaddress; }
		}

		internal bool	dhcpenabled;
		/// <summary>
		/// Indicator of whether DHCP (for IP address 
		/// assignment from a server), is enabled for the
		/// adapter.
		/// </summary>
		public bool	DhcpEnabled
		{
			get { return dhcpenabled; }
		}

		internal string	currentIp;
		/// <summary>
		/// The currently active IP address of the adapter.
		/// </summary>
		public string	CurrentIpAddress
		{
			get { return currentIp; }
		}

		internal string	currentsubnet;
		/// <summary>
		/// The currently active subnet mask address of the 
		/// adapter.
		/// </summary>
		public string	CurrentSubnetMask
		{
			get { return currentsubnet; }
		}

		internal string	gateway;
		/// <summary>
		/// The active gateway address.
		/// </summary>
		public string	Gateway
		{
			get { return gateway; }
		}

		internal string	dhcpserver;
		/// <summary>
		/// The DHCP server from which the IP address was
		/// last assigned.
		/// </summary>
		public string	DhcpServer
		{
			get { return dhcpserver; }
		}

		internal bool	havewins;
		/// <summary>
		/// Indicates the presence of WINS server addresses
		/// for the adapter.
		/// </summary>
		public bool	HasWins
		{
			get { return havewins; }
		}

		internal string	primarywinsserver;
		/// <summary>
		/// The IP address of the primary WINS server for the
		/// adapter.
		/// </summary>
		public string	PrimaryWinsServer
		{
			get { return primarywinsserver; }
		}
		internal string	secondarywinsserver;
		/// <summary>
		/// The IP address of the secondary WINS server for the
		/// adapter.
		/// </summary>
		public string	SecondaryWinsServer
		{
			get { return secondarywinsserver; }
		}

		internal DateTime	leaseobtained;
		/// <summary>
		/// The date/time at which the IP address lease was
		/// obtained from the DHCP server.
		/// </summary>
		public DateTime	LeaseObtained
		{
			get { return leaseobtained; }
		}
		internal DateTime	leaseexpires;
		/// <summary>
		/// The date/time at which the IP address lease from
		/// the DHCP server will expire (at which time the
		/// adapter will have to contact the server to renew
		/// the lease or stop using the IP address).
		/// </summary>
		public DateTime	LeaseExpires
		{
			get { return leaseexpires; }
		}

		/// <summary>
		/// Field, if set, is used, if the NDISUIO method
		/// fails, to get the RF signal strength.  You might 
		/// use this on an OS earlier than 4.0, when NDISUIO
		/// became available.  You'd usually create your own
		/// subclass of StrengthAddon, then assign an instance
		/// of that subclass to this property, then ask for
		/// the signal strength.
		/// </summary>
		internal StrengthAddon StrengthFetcher = null;

		internal Adapter( IP_ADAPTER_INFO info )
		{
			// Copy the name, description, index, etc.
			name = info.AdapterName;
			description = info.Description;
			index = info.Index;

			// The adapter type should not change, so we
			// can store that.
			type = info.Type;

			// The hardware address should not change, so
			// we can store that, too.
			hwaddress = info.PhysAddress;

			// Get the flag concerning whether DHCP is enabled
			// or not.
			dhcpenabled = info.DHCPEnabled;

			// Get the current IP address and subnet mask.
			currentIp = info.CurrentIpAddress;
			currentsubnet = info.CurrentSubnetMask;

			// Get the gateway address and the DHCP server.
			gateway = info.Gateway;
			dhcpserver = info.DHCPServer;

			// Get the WINS information.
			havewins = info.HaveWINS;
			primarywinsserver = info.PrimaryWINSServer;
			secondarywinsserver = info.SecondaryWINSServer;

			// DHCP lease information.
			leaseobtained = info.LeaseObtained;
			leaseexpires = info.LeaseExpires;
		}
//
//		internal const int MIB_IF_TYPE_OTHER = 1;
//		internal const int MIB_IF_TYPE_ETHERNET = 6;
//		internal const int MIB_IF_TYPE_TOKENRING = 9;
//		internal const int MIB_IF_TYPE_FDDI = 15;
//		internal const int MIB_IF_TYPE_PPP = 23;
//		internal const int MIB_IF_TYPE_LOOPBACK = 24;
//		internal const int MIB_IF_TYPE_SLIP = 28;

		/// <summary>
		/// Returns a Boolean indicating if the adapter is
		/// an RF Ethernet adapter.
		/// </summary>
		/// <returns>
		/// true if adapter is RF Ethernet; false otherwise
		/// </returns>
		public bool IsWireless
		{
			// Deciding if the adapter is RF Ethernet is a
			// little more complicated than just looking at
			// a bit somewhere.
			get {return ( (Type == AdapterType.Ethernet) && (SignalStrengthInDecibels != 0) ); }
		}

		// ???? To really replace NetworkAdapter, we need to
		// implement the properties below.
#if notdefined
		internal IP_ADDR_STRING IpAddressList
		{
			get 
			{ 
				return new IP_ADDR_STRING(data, 428);
			}
		}
#endif

		/// <summary>
		/// Returns the currently-attached SSID for RF
		/// Ethernet adapters.
		/// </summary>
		/// <returns>
		/// Instance of SSID class (or null if not associated).
		/// </returns>
		public unsafe String AssociatedAccessPoint
		{
			get 
			{
				// Are we wireless?
				if(!IsWireless)
					throw new AdapterException("Wired NICs are not associated with Access Points");
				
				String	ssid = null;

				// If we are running on an OS version of 4.0 or
				// higher (Windows CE.NET), then attempt to use
				// NDISUIO to get the SSID.  If we are running on 
				// an earlier version of the OS, we call a virtual 
				// method to get it.  If you have a PPC or other 
				// 3.0-based device, you can override this method 
				// to get the SSID in some other way.
				if ( System.Environment.OSVersion.Version.Major >= 4 )
				{
					NDISUIO_QUERY_OID	queryOID;
					uint				dwBytesReturned = 0;
					IntPtr				ndisAccess;
					bool				retval;

					// Attach to NDISUIO.
					ndisAccess = FileEx.CreateFile( 
						NDISUIOPInvokes.NDISUIO_DEVICE_NAME, 
						FileAccess.All, 
						FileShare.None,
						FileCreateDisposition.OpenExisting,
						NDISUIOPInvokes.FILE_ATTRIBUTE_NORMAL | NDISUIOPInvokes.FILE_FLAG_OVERLAPPED );
					if ( (int)ndisAccess == FileEx.InvalidHandle )
						return null;

					// Pin unsafely-accessed items in memory.
					byte[]	namestr = System.Text.Encoding.Unicode.GetBytes(this.Name+'\0');
					fixed (byte *name = &namestr[ 0 ])
					{
						// Get Signal strength
						queryOID = new NDISUIO_QUERY_OID( 36 );	// The data is a four-byte length plus 32-byte ASCII string
						queryOID.ptcDeviceName = name;
						queryOID.Oid = NDISUIOPInvokes.OID_802_11_SSID; // 0x0D010102

						retval = NDISUIOPInvokes.DeviceIoControl( ndisAccess,
							NDISUIOPInvokes.IOCTL_NDISUIO_QUERY_OID_VALUE,	// 0x00120804
							queryOID, 
							queryOID.Size,
							queryOID, 
							queryOID.Size,
							ref dwBytesReturned,
							IntPtr.Zero);
					}

					if ( retval )
					{
						// Convert the data to a string.
						byte[]	ssdata = queryOID.Data;
						int		len	= BitConverter.ToInt32( ssdata, 0 );
						if ( len > 0 )
						{
							// Convert the string from ASCII to
							// Unicode.
							ssid = System.Text.Encoding.ASCII.GetString( ssdata, 4, len );
						}
					}
					// else, there is an error; return 0.

					FileEx.CloseHandle( ndisAccess );
				}

				// If there is still no signal indication,
				// give the add-on method a chance.
				if ( ( ssid == null ) && ( this.StrengthFetcher != null ) )
					ssid = this.StrengthFetcher.RFSSID( this );

				return ssid;
			}
		}

		/// <summary>
		/// Returns the strength of the RF Ethernet signal
		/// being received by the adapter, in dB.
		/// </summary>
		/// <returns>
		/// integer strength in dB; zero, if adapter is not
		/// an RF adapter or an error occurred
		/// </returns>
		public unsafe int SignalStrengthInDecibels
		{
			get 
			{
				int	db = 0;	// 0 indicates not an RF adapter or error.

				// If we are running on an OS version of 4.0 or
				// higher (Windows CE.NET), then attempt to use
				// NDISUIO to get the RF signal strenth.  If we
				// are running on an earlier version of the OS, 
				// we call a virtual method to get it.  If you
				// have a PPC or other 3.0-based device, you can
				// override this method to get the signal
				// strength in some other way.
				if ( System.Environment.OSVersion.Version.Major >= 4 )
				{
					NDISUIO_QUERY_OID	queryOID;
					uint				dwBytesReturned = 0;
					IntPtr				ndisAccess;
					bool				retval;

					// Attach to NDISUIO.
					ndisAccess = FileEx.CreateFile( 
						NDISUIOPInvokes.NDISUIO_DEVICE_NAME, 
						FileAccess.All, 
						FileShare.None,
						FileCreateDisposition.OpenExisting,
						NDISUIOPInvokes.FILE_ATTRIBUTE_NORMAL | NDISUIOPInvokes.FILE_FLAG_OVERLAPPED );
					if ( (int)ndisAccess == FileEx.InvalidHandle )
						return 0;

					// Pin unsafely-accessed items in memory.
					byte[]	namestr = System.Text.Encoding.Unicode.GetBytes(this.Name+'\0');
					fixed (byte *name = &namestr[ 0 ])
					{
						// Get Signal strength
						queryOID = new NDISUIO_QUERY_OID( 4 );
						queryOID.ptcDeviceName = name;
						queryOID.Oid = NDISUIOPInvokes.OID_802_11_RSSI; // 0x0d010206

						retval = NDISUIOPInvokes.DeviceIoControl( ndisAccess,
							NDISUIOPInvokes.IOCTL_NDISUIO_QUERY_OID_VALUE,	// 0x00120804
							queryOID, 
							queryOID.Size,
							queryOID, 
							queryOID.Size,	// 0x10
							ref dwBytesReturned,
							IntPtr.Zero);
					}

					if ( retval )
					{
						byte[]	ssdata = queryOID.Data;
						db = BitConverter.ToInt32( ssdata, 0 );
					}
					// else, there is an error; return 0.

					FileEx.CloseHandle( ndisAccess );
				}

				// If there is still no signal indication,
				// give the add-on method a chance.
				if ( ( db == 0 ) && ( this.StrengthFetcher != null ) )
					db = this.StrengthFetcher.RFSignalStrengthDB( this );

				return db;
			}
		}

		/// <summary>
		/// Returns a SignalStrength class representing the current strength
		/// of the signal.
		/// </summary>
		/// <returns>
		///	SignalStrength
		/// </returns>
		public SignalStrength SignalStrength
		{
			get 
			{
				// Check if its a 802.11 adapter first...
				if(!IsWireless)
					throw new AdapterException("Signal strength is not a property of a wired NIC adapter");

				// Get the signal strength code and just convert
				// it to a string.
				return ( new SignalStrength(this.SignalStrengthInDecibels) );
			}
		}

		/// <summary>
		/// Returns a list of the SSID values which the 
		/// adapter can currently 'hear'.
		/// </summary>
		/// <returns>
		/// SSIDList instance containing the SSIDs.
		/// </returns>
		public AccessPointCollection NearbyAccessPoints
		{
			get { return ( new AccessPointCollection( this ) ); }
		}
	}

	#region ---------- P/Invokes ----------

	// P/Invoke declarations.
	internal class AdapterPInvokes
	{
		[DllImport ("iphlpapi.dll", SetLastError=true)]
		public static extern int GetAdaptersInfo( byte[] ip, ref int size );
	}

	internal class NDISUIOPInvokes
	{
		public const String NDISUIO_DEVICE_NAME = "UIO1:";

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern bool DeviceIoControl(
			IntPtr hDevice, UInt32 dwIoControlCode,
			byte[] lpInBuffer, Int32 nInBufferSize,
			byte[] lpOutBuffer, Int32 nOutBufferSize,
			ref UInt32 lpBytesReturned,
			IntPtr lpOverlapped);

		[DllImport("coredll.dll")]
		public static extern uint GetLastError();

//		public const Int32 INVALID_HANDLE_VALUE = -1;
//		public const UInt32 OPEN_EXISTING = 3;
//		public const UInt32 GENERIC_READ = 0x80000000;
//		public const UInt32 GENERIC_WRITE = 0x40000000;

		public const Int32 FILE_ATTRIBUTE_NORMAL = 0x00000080;
		public const Int32	FILE_FLAG_OVERLAPPED = 0x40000000;

		public const UInt32 ERROR_SUCCESS = 0x0;
		public const UInt32 E_FAIL = 0x80004005;

		public const uint OID_802_11_BSSID                        = 0x0D010101;
		public const uint OID_802_11_SSID                         = 0x0D010102;
		public const uint OID_802_11_NETWORK_TYPES_SUPPORTED      = 0x0D010203;
		public const uint OID_802_11_NETWORK_TYPE_IN_USE          = 0x0D010204;
		public const uint OID_802_11_TX_POWER_LEVEL               = 0x0D010205;
		public const uint OID_802_11_RSSI                         = 0x0D010206;
		public const uint OID_802_11_RSSI_TRIGGER                 = 0x0D010207;
		public const uint OID_802_11_INFRASTRUCTURE_MODE          = 0x0D010108;
		public const uint OID_802_11_FRAGMENTATION_THRESHOLD      = 0x0D010209;
		public const uint OID_802_11_RTS_THRESHOLD                = 0x0D01020A;
		public const uint OID_802_11_NUMBER_OF_ANTENNAS           = 0x0D01020B;
		public const uint OID_802_11_RX_ANTENNA_SELECTED          = 0x0D01020C;
		public const uint OID_802_11_TX_ANTENNA_SELECTED          = 0x0D01020D;
		public const uint OID_802_11_SUPPORTED_RATES              = 0x0D01020E;
		public const uint OID_802_11_DESIRED_RATES                = 0x0D010210;
		public const uint OID_802_11_CONFIGURATION                = 0x0D010211;
		public const uint OID_802_11_STATISTICS                   = 0x0D020212;
		public const uint OID_802_11_ADD_WEP                      = 0x0D010113;
		public const uint OID_802_11_REMOVE_WEP                   = 0x0D010114;
		public const uint OID_802_11_DISASSOCIATE                 = 0x0D010115;
		public const uint OID_802_11_POWER_MODE                   = 0x0D010216;
		public const uint OID_802_11_BSSID_LIST                   = 0x0D010217;
		public const uint OID_802_11_AUTHENTICATION_MODE          = 0x0D010118;
		public const uint OID_802_11_PRIVACY_FILTER               = 0x0D010119;
		public const uint OID_802_11_BSSID_LIST_SCAN              = 0x0D01011A;
		public const uint OID_802_11_WEP_STATUS                   = 0x0D01011B;
		// Renamed to support more than just WEP encryption
		public const uint OID_802_11_ENCRYPTION_STATUS            = OID_802_11_WEP_STATUS;
		public const uint OID_802_11_RELOAD_DEFAULTS              = 0x0D01011C;

		public const uint IOCTL_NDISUIO_QUERY_OID_VALUE = 0x120804;
		public const uint IOCTL_NDISUIO_SET_OID_VALUE = 0x120814;
	}

	#endregion

}
