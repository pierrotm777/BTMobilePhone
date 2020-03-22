//==========================================================================================
//
//		OpenNETCF.Configuration.IConfiguratinSectionHandler
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
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Defines the contract that all configuration section handlers must implement in order to participate in the resolution of configuration settings.
	/// Reads key-value pair configuration information for a configuration section.
	/// </summary>
	public interface IConfigurationSectionHandler
	{
		/// <summary>
		/// Implemented by all configuration section handlers to parse the XML of the configuration section. The 
		/// returned object is added to the configuration collection and is accessed by 
		/// System.Configuration.ConfigurationSettings.GetConfig(System.String).
		/// </summary>
		/// <param name="parent">The configuration settings in a corresponding parent configuration section.</param>
		/// <param name="configContext">An System.Web.Configuration.HttpConfigurationContext when 
		/// System.Configuration.IConfigurationSectionHandler.Create(System.Object,System.Object,System.Xml.XmlNode) 
		/// is called from the ASP.NET configuration system. Otherwise, this parameter is reserved and is null.</param>
		/// <param name="section">The System.Xml.XmlNode that contains the configuration information from the 
		/// configuration file. Provides direct access to the XML contents of the configuration section.</param>
		/// <returns>A configuration object.</returns>
		object Create(object parent, object configContext, XmlNode section);
	}
}
