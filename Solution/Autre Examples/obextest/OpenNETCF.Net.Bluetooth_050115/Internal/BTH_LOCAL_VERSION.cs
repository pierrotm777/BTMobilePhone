using System;

namespace OpenNETCF.Net.Bluetooth.Internal
{
	/// <summary>
	/// Summary description for BTH_LOCAL_VERSION.
	/// </summary>
	internal class BTH_LOCAL_VERSION
	{
		byte	hci_version;
		ushort	hci_revision;
		byte	lmp_version;
		ushort	lmp_subversion;
		ushort	manufacturer;
		long	lmp_features;

		public BluetoothVersion ToVersion()
		{
			return new BluetoothVersion(hci_version, hci_revision, lmp_version, lmp_subversion, manufacturer, lmp_features);
		}
	}
}
