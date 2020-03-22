//==========================================================================================
//
//		OpenNETCF.Configuration.DictionarySectionHandler
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
using System.Globalization;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Reads key-value pair configuration information for a configuration section.
	/// <para><b>New in v1.1</b></para>
	/// </summary>
	/// <example>
	/// <code>
	/// &lt;add key="name" value="text"> - sets key=text
	/// &lt;remove key="name"> - removes the definition of key
	/// &lt;clear/> - removes all definitions
	/// </code>
	/// </example>
	public class DictionarySectionHandler: IConfigurationSectionHandler
	{
		/// <summary>
		/// Make the name of the key attribute configurable by derived classes.
		/// </summary>
		protected virtual string KeyAttributeName
		{
			get { return "key"; }
		}

		/// <summary>
		/// Make the name of the value attribute configurable by derived classes.
		/// </summary>
		protected virtual string ValueAttributeName
		{
			get { return "value"; }
		}

		internal virtual bool ValueRequired
		{
			get { return false; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="context"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable result;

			// Create a shallow clone of the parent
			if (parent == null)
			{
				result = new Hashtable(new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
			}
			else
			{
				result = (Hashtable)((Hashtable)parent).Clone();
			}
			// Process XML
			HandlerBase.CheckForUnrecognizedAttributes(section);

			foreach(XmlNode child in section.ChildNodes)
			{
				// Skip whitespace and comments; throw exception if non-element
				if(HandlerBase.IsIgnorableAlsoCheckForNonElement(child))
				{
					continue;
				}

				// Handle <add>, <remove>, and <clear> tags
				if(child.Name == "add")
				{
					string key = HandlerBase.RemoveRequiredAttribute(child, KeyAttributeName);
					string value = HandlerBase.RemoveAttribute(child, ValueAttributeName);
					HandlerBase.CheckForUnrecognizedAttributes(child);

					if(value == null)
					{
						value = string.Empty;
					}

					result[key] = value;
				}
				else if(child.Name == "remove")
				{
					string key = HandlerBase.RemoveRequiredAttribute(child, KeyAttributeName);
					HandlerBase.CheckForUnrecognizedAttributes(child);

					result.Remove(key);
				}
				else if(child.Name == "clear")
				{
					HandlerBase.CheckForUnrecognizedAttributes(child);
					result.Clear();
				}
				else
				{
					HandlerBase.ThrowUnrecognizedElement(child);
				}
			}
			return result;
		}
	}
}

