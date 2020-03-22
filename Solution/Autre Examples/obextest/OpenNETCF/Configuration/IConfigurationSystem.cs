//==========================================================================================
//
//		OpenNETCF.Configuration.IConfigurationSystem
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

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// <para><b>New in v1.1</b></para>
	/// </summary>
	public interface IConfigurationSystem
	{
		/// <summary>
		/// Returns the config object for the specified key.  
		/// </summary>
		/// <param name="configKey">Section name of config object to retrieve. </param>
		/// <returns></returns>
		object GetConfig(string configKey);

		/// <summary>
		/// Initializes the configuration system. 
		/// </summary>
		void Init();
	}
}
