using System;
using System.Collections;
using OpenNETCF.Win32;
using OpenNETCF.IO;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Class that represents a collection of the SSID values
	/// that a given network adapter can hear over the 
	/// airwaves.  For each SSID, you can get the signal
	/// strength and random other information.
	/// </summary>
	public class AccessPointCollection : CollectionBase
	{
		internal Adapter	adapter = null;

		/// <summary>
		/// The Adapter instance with which the SSID instance
		/// is associated.
		/// </summary>
		public Adapter AssociatedAdapter
		{
			get { return adapter; }
		}

		internal AccessPointCollection( Adapter a )
		{
			adapter = a;

			this.RefreshList( true );
		}

		internal unsafe void ClearCache()
		{
			// Tell the driver to search for any new SSIDs
			// and return them on the next OID_802_11_BSSID_LIST
			// message.
			uint		dwBytesReturned = 0;
			NDISUIO_QUERY_OID	queryOID = new NDISUIO_QUERY_OID( 0 );
			IntPtr		ndisAccess;
			bool		retval;

			// Attach to NDISUIO.
			ndisAccess = FileEx.CreateFile( 
				NDISUIOPInvokes.NDISUIO_DEVICE_NAME, 
				FileAccess.All, 
				FileShare.None,
				FileCreateDisposition.OpenExisting,
				NDISUIOPInvokes.FILE_ATTRIBUTE_NORMAL | NDISUIOPInvokes.FILE_FLAG_OVERLAPPED );
			if ( (int)ndisAccess == FileEx.InvalidHandle )
			{
				// The operation failed.  Leave us empty.
				return;
			}

			// Send message to driver.
			byte[]	namestr = System.Text.Encoding.Unicode.GetBytes(adapter.Name+'\0');
			fixed (byte *name = &namestr[ 0 ])
			{
				// Get Signal strength
				queryOID.ptcDeviceName = name;
				queryOID.Oid = NDISUIOPInvokes.OID_802_11_BSSID_LIST_SCAN; // 0x0D01011A

				retval = NDISUIOPInvokes.DeviceIoControl( ndisAccess,
					NDISUIOPInvokes.IOCTL_NDISUIO_SET_OID_VALUE,	// 0x120814
					queryOID, 
					queryOID.Size,
					queryOID, 
					queryOID.Size,
					ref dwBytesReturned,
					IntPtr.Zero);
			}

			if( retval )
			{
				// The call went fine.  There is no return
				// data.
			}
			else
			{
				// There was an error.
				UInt32	err = NDISUIOPInvokes.GetLastError();

				// ToDo: Additional error processing.
			}

			queryOID = null;

			FileEx.CloseHandle( ndisAccess );
		}

		internal unsafe void RefreshList( Boolean clearCache )
		{
			// If we are to clear the driver's cache of SSID
			// values, call the appropriate method.
			if ( clearCache )
			{
				this.ClearCache();

				// This seems to be needed to avoid having
				// a list of zero elements returned.
				System.Threading.Thread.Sleep( 1000 );
			}

			// Retrieve a list of NDIS_802_11_BSSID_LIST 
			// structures from the driver.  We'll parse that
			// list and populate ourselves based on the data
			// that we find there.
			uint		dwBytesReturned = 0;
			NDISUIO_QUERY_OID	queryOID = new NDISUIO_QUERY_OID( 2000 );
			IntPtr		ndisAccess;
			bool		retval;

			// Attach to NDISUIO.
			ndisAccess = FileEx.CreateFile( 
				NDISUIOPInvokes.NDISUIO_DEVICE_NAME, 
				FileAccess.All, 
				FileShare.None,
				FileCreateDisposition.OpenExisting,
				NDISUIOPInvokes.FILE_ATTRIBUTE_NORMAL | NDISUIOPInvokes.FILE_FLAG_OVERLAPPED );
			if ( (int)ndisAccess == FileEx.InvalidHandle )
			{
				// The operation failed.  Leave us empty.
				return;
			}

			// Get Signal strength
			byte[]	namestr = System.Text.Encoding.Unicode.GetBytes(adapter.Name+'\0');
			fixed (byte *name = &namestr[ 0 ])
			{
				// Get Signal strength
				queryOID.ptcDeviceName = name;
				queryOID.Oid = NDISUIOPInvokes.OID_802_11_BSSID_LIST; // 0x0D010217

				retval = NDISUIOPInvokes.DeviceIoControl( ndisAccess,
					NDISUIOPInvokes.IOCTL_NDISUIO_QUERY_OID_VALUE,	// 0x00120804
					queryOID, 
					queryOID.Size,
					queryOID, 
					queryOID.Size,
					ref dwBytesReturned,
					IntPtr.Zero);
			}

			if( retval )
			{
				// Now we need to parse the incoming data into
				// suitable representations of the SSIDs.

				// Figure out how many SSIDs there are.
				NDIS_802_11_BSSID_LIST	rawlist = new NDIS_802_11_BSSID_LIST( queryOID.Data );

				for ( int i = 0; i < rawlist.NumberOfItems; i++ )
				{
					// Get the next raw item from the list.
					NDIS_WLAN_BSSID	bssid = rawlist.Item( i );

					// Using the raw item, create a cooked 
					// SSID item.
					AccessPoint	ssid = new AccessPoint( bssid );

					// Add the new item to this.
					this.List.Add( ssid );
				}
			}
			else
			{
				// We might just need more room.
				// For now, we just leave the list empty.
				// ToDo: Additional error processing.
			}

			FileEx.CloseHandle( ndisAccess );
		}

		/// <summary>
		/// Retrieve the indicated item from the collection.
		/// </summary>
		/// <param name="index">
		/// Zero-based index of SSID item to retrieve.
		/// </param>
		/// <returns>
		/// SSID found at index position in collection
		/// </returns>
		public AccessPoint Item( int index )
		{
			return (AccessPoint)List[ index ];
		}

		/// <summary>
		/// Refresh the list of SSID values, asking the 
		/// adapter to scan for new ones, also.
		/// </summary>
		public void Refresh()
		{
			this.RefreshList( true );
		}
	}

}
