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

	internal class NDIS_802_11_CONFIGURATION_FH
	{
		internal byte[]	data;
		internal int	offset;

		protected const int		LengthOffset = 0;
		protected const int		HopPatternOffset = 4;
		protected const int		HopSetOffset = 8;
		protected const int		DwellTimeOffset = 12;

		public NDIS_802_11_CONFIGURATION_FH( byte[] d, int o )
		{
			data = d;
			offset = o;
		}

		public uint Length
		{
			get
			{
				return BitConverter.ToUInt32( data, offset + LengthOffset );
			}
		}

		public uint HopPattern
		{
			get
			{
				return BitConverter.ToUInt32( data, offset + HopPatternOffset );
			}
		}

		public uint HopSet
		{
			get
			{
				return BitConverter.ToUInt32( data, offset + HopSetOffset );
			}
		}

		/// <summary>
		/// Amount of time dwelling in each frequency (in kusec).
		/// </summary>
		public uint DwellTime
		{
			get
			{
				return BitConverter.ToUInt32( data, offset + DwellTimeOffset );
			}
		}
	}

	internal class NDIS_802_11_CONFIGURATION
	{
		internal byte[]	data;
		internal int	offset;

		protected const int	    LengthOffset = 0;
		protected const int		BeaconPeriodOffset = 4;
		protected const int		ATIMWindowOffset = 8;
		protected const int		DSConfigOffset = 12;
		protected const int		FHConfigOffset = 16;

		public NDIS_802_11_CONFIGURATION( byte[] d, int o )
		{
			data = d;
			offset = o;
		}

		public uint Length
		{
			get
			{
				return BitConverter.ToUInt32( data, offset + LengthOffset );
			}
		}

		public uint BeaconPeriod
		{
			get
			{
				return BitConverter.ToUInt32( data, offset + BeaconPeriodOffset );
			}
		}

		public uint ATIMWindow
		{
			get
			{
				return BitConverter.ToUInt32( data, offset + ATIMWindowOffset );
			}
		}

		public uint DSConfig
		{
			get
			{
				return BitConverter.ToUInt32( data, offset + DSConfigOffset );
			}
		}

		public NDIS_802_11_CONFIGURATION_FH FHConfig
		{
			get
			{
				return new NDIS_802_11_CONFIGURATION_FH( data, offset + FHConfigOffset );
			}
		}
	}

	internal class NDIS_WLAN_BSSID
	{
		internal byte[]	data;
		internal int	offset;

		protected const int	    LengthOffset = 0;
		protected const int	    MacAddressOffset = LengthOffset + 4;
		protected const int	    ReservedOffset = MacAddressOffset + 6;
		protected const int	    SsidOffset = ReservedOffset + 2;
		protected const int	    PrivacyOffset = SsidOffset + 36;	// The ssid length plus 32 character array.
		protected const int	    RssiOffset = PrivacyOffset + 4;
		protected const int	    NetworkTypeInUseOffset = RssiOffset + 4;
		protected const int	    ConfigurationOffset = NetworkTypeInUseOffset + 4;	// It's an enum.  I hope it's four bytes.
		protected const int	    InfrastructureModeOffset = ConfigurationOffset + 32;	// It's a structure, with another structure inside it.
		protected const int	    SupportedRatesOffset = InfrastructureModeOffset + 4;	// Another enum.

		public NDIS_WLAN_BSSID( byte[] d, int o )
		{
			data = d;
			offset = o;
		}

		public uint Length
		{
			get { return BitConverter.ToUInt32( data, offset + LengthOffset ); }
		}

		public byte[] MacAddress
		{
			get 
			{ 
				byte[]	b = new byte[ 6 ];
				Array.Copy( data, offset + MacAddressOffset, b, 0, 6 ); 
				return b;
			}
		}

		public String SSID
		{
			get 
			{ 
				// Get the string length from the first four bytes.
				int	c = BitConverter.ToInt32( data, offset + SsidOffset );

				// There are some adapters which cause the base
				// GetString() call below to fail because of the
				// value returned by them as the length of the 
				// SSID string.  We try to guess at what values
				// might make sense and so something that might
				// make sense also in those cases.

				// If, by chance, the length is negative, then do
				// something reasonable.
				if ( c == -1 )
				{
					c = 32;	// 32 is the maximum length, so pick
					// that value.
				}
				else if ( c < 0 )
				{
					// At a guess, perhaps some drivers are returning
					// the negative of the actual length, for some
					// reason.
					c = -c;
				}

				// Final range check.
				if ( c > 32 )
				{
					// There is no valid value of length which
					// is greater than 32.
					return null;
				}

				// Convert the rest of the SSID stuff to a string.
				return System.Text.Encoding.ASCII.GetString( data, offset + SsidOffset + 4, c );
			}
		}

		public int Privacy
		{
			get { return BitConverter.ToInt32( data, offset + PrivacyOffset ); }
		}

		public int Rssi
		{
			get 
			{ 
				// There seems to be some confusion in the 
				// drivers about whether this should be a
				// negative number or not.  If it's greater
				// than zero, we negate it.
				int	db = BitConverter.ToInt32( data, offset + RssiOffset );
				if( db > 0 )
					return -db;
				else
					return db;
			}
		}

		public NetworkType NetworkTypeInUse
		{
			get 
			{ 
				return (NetworkType)BitConverter.ToInt32( data, offset + NetworkTypeInUseOffset );
			}
		}

		public NDIS_802_11_CONFIGURATION Configuration
		{
			get
			{
				return new NDIS_802_11_CONFIGURATION( data, offset + ConfigurationOffset );
			}
		}

		public InfrastructureMode InfrastructureMode
		{
			get
			{
				return (InfrastructureMode)BitConverter.ToInt32( data, offset + InfrastructureModeOffset );
			}
		}

		public byte[] SupportedRates
		{
			get
			{
				byte[]	b = new byte[ 8 ];
				Array.Copy( data, offset + SupportedRatesOffset, b, 0, 8 ); 
				return b;
			}
		}
	}

	/// <summary>
	/// This class represents the data returned by the 
	/// OID_802_11_BSSID_LIST query to an RF Ethernet adapter.
	/// It is just used during parsing of the returned data
	/// and really should not persist.
	/// </summary>
	internal class NDIS_802_11_BSSID_LIST
	{
		internal byte[]	data;

		protected const int NumberOfItemsOffset = 0;
		protected const int BssidOffset = 4;

		public NDIS_802_11_BSSID_LIST( byte [] d )
		{
			data = d;
		}

		public uint NumberOfItems
		{
			get { return BitConverter.ToUInt32( data, NumberOfItemsOffset ); }
		}

		public NDIS_WLAN_BSSID Item( int index )
		{
			// You can't just do this!  Some adapters seem
			// to use different sizes for *each element* of
			// the array!  You have to step from one to the
			// next to the next to get to the indicated item
			// index.
			int	offset = 0;
			for ( int i = 0; i < index; i++ )
			{
				// Get the length of the item we are presently
				// pointing to.
				int	len = BitConverter.ToInt32( data, BssidOffset + offset );
				offset += len;
			}

			// Return the current offset.  This is the start
			// of the data for the indicated item in the list.
			return new NDIS_WLAN_BSSID( data, BssidOffset + offset );
		}
	}

	#endregion

	/// <summary>
	/// The SSID class represents a single SSID value which
	/// an adapter might be receiving data from.  It can be
	/// queried for SSID-specific information for the
	/// associated adapter such as signal strength.
	/// </summary>
	public class AccessPoint
	{
		internal AccessPoint( NDIS_WLAN_BSSID bssid )
		{
			name = bssid.SSID;
			macaddr = bssid.MacAddress;
			privacy = bssid.Privacy;
			rssi = bssid.Rssi;

			supportedrates = bssid.SupportedRates;
			networktypeinuse = bssid.NetworkTypeInUse;
			infrastructuremode = bssid.InfrastructureMode;

			// ToDo: For now, the configuration is not returned.
//			configuration = bssid.Configuration;
		}

		internal String name;
		/// <summary>
		/// The SSID name string.
		/// </summary>
		public String Name
		{
			get { return name; }
		}

		internal byte[] macaddr;
		/// <summary>
		/// The hardware address for the network adapter.
		/// </summary>
		public byte[] MacAddress
		{
			get { return macaddr; }
		}

		internal int privacy;
		/// <summary>
		/// The privacy mask for the adapter.
		/// </summary>
		public int Privacy
		{
			get { return privacy; }
		}

		internal int rssi;

		/// <summary>
		/// Returns the strength of the RF Ethernet signal
		/// being received by the adapter for the SSID, in dB.
		/// </summary>
		/// <returns>
		/// integer strength in dB; zero, if adapter is not
		/// an RF adapter or an error occurred
		/// </returns>
		public int SignalStrengthInDecibels
		{
			get
			{
				return rssi;
			}
		}

		/// <summary>
		/// Returns the strength of the RF Ethernet signal
		/// being received by the adapter for the SSID, in dB.
		/// </summary>
		/// <returns>
		/// SignalStrength instance containing the strength
		/// </returns>
		public SignalStrength SignalStrength
		{
			get
			{
				return new SignalStrength( rssi );
			}
		}

		internal byte[] supportedrates;
		/// <summary>
		/// Returns the list of supported signaling rates for
		/// the adapter.  Each value indicates a single rate.
		/// </summary>
		/// <returns>
		/// array of bytes, each of which represents a rate.
		/// The units are 0.5Mbps.  Rates that belong to the
		/// 'basic rate set' have their high bits set to 1
		/// (they are OR-ed with 0x80).  Rates which are not
		/// in the basic rate set, have this bit clear.
		/// So, a value of 0x96, after clearing the
		/// high bit, is 0x16 or 22d.  Multiplying by 0.5Mbps 
		/// gives a rate of 11Mbps.  Since the high bit was
		/// set, this rate is in the basic rate set.
		/// </returns>
		public byte[] SupportedRates
		{
			get
			{
				return supportedrates;
			}
		}

		internal NetworkType networktypeinuse;
		/// <summary>
		/// Returns the current type of network in use in
		/// the form of an element of the 
		/// Ndis80211NetworkType enumeration.
		/// </summary>
		/// <returns>
		/// Ndis80211NetworkType network type
		/// </returns>
		public NetworkType NetworkTypeInUse
		{
			get
			{
				return networktypeinuse;
			}
		}

		internal InfrastructureMode infrastructuremode;
		/// <summary>
		/// Returns the current infrastructure in use by the
		/// adapter.
		/// </summary>
		/// <returns>
		/// Ndis80211NetworkInfrastructure type
		/// </returns>
		public InfrastructureMode InfrastructureMode
		{
			get
			{
				return infrastructuremode;
			}
		}
	}


	#region -------------- Add-ons --------------
	
	/// <summary>
	/// Abstract class representing a 'different' method of
	/// finding the signal strength for a network adapter. 
	/// You might need to provide a subclass of this class
	/// when your code runs on Windows CE 3.0 or earlier or
	/// when the signal strength is not retrievable via the
	/// Windows CE.NET NDISUIO driver in the standard 802.11
	/// manner.
	/// </summary>
	internal abstract class StrengthAddon
	{
		/// <summary>
		/// Abstract method.  Your subclass of StrengthAddon
		/// must implement this method and do whatever is 
		/// needed to get the RF signal strength in dB from
		/// the indicated adapter.  Return 0 if the adapter
		/// is not an RF adapter or there is an error.
		/// </summary>
		/// <param name="a">
		/// Instance of Adapter class for which signal strength is to be returned
		/// </param>
		/// <returns>
		/// int signal strength in dB or zero for error
		/// </returns>
		public abstract int RFSignalStrengthDB( Adapter a );

		/// <summary>
		/// Abstract method.  Your subclass of StrengthAddon
		/// must implement this method and do whatever is 
		/// needed to get the current SSID from the indicated 
		/// adapter.  Return null if the adapter is not an RF 
		/// adapter, if it is not presently associated, or 
		/// there is an error.
		/// </summary>
		/// <param name="a">
		/// Instance of Adapter class for which SSID is to be returned
		/// </param>
		/// <returns>
		/// String SSID string or null for error or unassociated
		/// </returns>
		public abstract String RFSSID( Adapter a );
	}

	#endregion

}
