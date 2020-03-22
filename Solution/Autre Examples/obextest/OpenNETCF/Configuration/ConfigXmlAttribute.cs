//==========================================================================================
//
//		OpenNETCF.Configuration.ConfigXmlAttribute
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
using System.Xml;

namespace OpenNETCF.Configuration
{
	sealed class ConfigXmlAttribute : XmlAttribute, IConfigXmlNode
	{
		private int _line;
		private string _filename;

		public ConfigXmlAttribute(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc) : base(prefix, localName, namespaceUri, doc)
		{
			_line = line;
			_filename = filename;
		}

		int IConfigXmlNode.LineNumber
		{
			get { return _line; }
		}

		string IConfigXmlNode.Filename
		{
			get { return _filename; }
		}

		public override XmlNode CloneNode(bool deep)
		{
			XmlNode cloneNode = base.CloneNode(deep);
			ConfigXmlAttribute clone = cloneNode as ConfigXmlAttribute;
			if (clone != null)
			{
				clone._line = _line;
				clone._filename = _filename;
			}
			return clone;
		}
	}
}
