//==========================================================================================
//
//		OpenNETCF.Configuration.NameValueFileSectionHandler
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
using System.IO;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// <para><b>New in v1.1</b></para>
	/// </summary>
	public class NameValueFileSectionHandler: IConfigurationSectionHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			object result = parent;
			XmlNode fileAttribute = section.Attributes.RemoveNamedItem("file");
			result = NameValueSectionHandler.CreateStatic(result, section);
			if (fileAttribute != null && fileAttribute.Value.Length != 0)
			{
				string sectionName = fileAttribute.Value;
				IConfigXmlNode configXmlNode = fileAttribute as IConfigXmlNode;
				if (configXmlNode == null)
				{
					return null;
				}
				string sourceFileFullPath = Path.Combine(Path.GetDirectoryName(configXmlNode.Filename), sectionName);
				if (File.Exists(sourceFileFullPath))
				{
					ConfigXmlDocument configXmlDocument = new ConfigXmlDocument();
					try
					{
						configXmlDocument.Load(sourceFileFullPath);
					}
					catch (XmlException e)
					{
						throw new ConfigurationException(e.Message, e, sourceFileFullPath, e.LineNumber);
					}
					if (section.Name != configXmlDocument.DocumentElement.Name)
					{
						throw new ConfigurationException("Config NameValueFile Section: Invalid root", configXmlDocument.DocumentElement);
					}
					result = NameValueSectionHandler.CreateStatic(result, configXmlDocument.DocumentElement);
				}
			}
			return result;
		}
	}

}
