//==========================================================================================
//
//		OpenNETCF.Net.Bluetooth.LinkPolicy
//		Copyright (C) 2005, OpenNETCF.org
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

namespace OpenNETCF.Net.Bluetooth
{
	/// <summary>
	/// Flags to describe Link Policy.
	/// </summary>
	[Flags()]
	public enum LinkPolicy : short
	{
		/// <summary>
		/// Disables all LAN Manager (LM) modes. 
		/// </summary>
		Disabled = 0x0000, 
		/// <summary>
		/// Enables the master slave switch.
		/// </summary>
		MasterSlave = 0x0001,
		/// <summary>
		/// Enables Hold mode.
		/// </summary>
		Hold = 0x0002,  
		/// <summary>
		/// Enables Sniff Mode.
		/// </summary>
		Sniff = 0x0004,
		/// <summary>
		/// Enables Park Mode.
		/// </summary>
		Park = 0x0008,  

	}
}
