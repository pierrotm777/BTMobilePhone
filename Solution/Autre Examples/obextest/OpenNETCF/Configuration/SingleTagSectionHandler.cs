//==========================================================================================
//
//		OpenNETCF.Configuration.SingleTagSectionHandler
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
using System.Collections;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// <para><b>New in v1.1</b></para>
	/// </summary>
	public class SingleTagSectionHandler: IConfigurationSectionHandler
	{
		/// <summary>
		/// Returns a collection of configuration section values.
		/// </summary>
		/// <param name="parent">The configuration settings in a corresponding parent configuration section.</param>
		/// <param name="context">This parameter is reserved and is null.</param>
		/// <param name="section">An <see cref="System.Xml.XmlNode"/> that contains configuration information from the configuration file.
		/// Provides direct access to the XML contents of the configuration section.</param>
		/// <returns>A <see cref="Hashtable"/> containing configuration section directives.</returns>
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable result;

			// start result off as a shallow clone of the parent
			if (parent == null)
			{
				result = new Hashtable();
			}
			else
			{
				result = new Hashtable((Hashtable)parent);
			}

			// Check for child nodes
			HandlerBase.CheckForChildNodes(section);
			
			foreach(XmlNode attribute in section.Attributes)
			{
				result[attribute.Name] = attribute.Value;
			}

			return result;
		}
	}
}
