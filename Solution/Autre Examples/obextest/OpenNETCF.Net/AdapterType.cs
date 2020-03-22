using System;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Different NIC adapter types
	/// </summary>
	public enum AdapterType
	{
		Other = 1,
		Ethernet = 6,
		TokenRing = 9,
		FDDI = 15,
		PPP = 23,
		Loopback = 24,
		SLIP = 28,
	}
}
