//==========================================================================================
//
//		OpenNETCF.Configuration.ConfigXmlWhitespace
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
	sealed class ConfigXmlWhitespace : XmlWhitespace, IConfigXmlNode
	{
		private int _line;
		private string _filename;

		public ConfigXmlWhitespace(string filename, int line, string comment, XmlDocument doc) : base(comment, doc)
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
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlWhitespace configXmlWhitespace = xmlNode as ConfigXmlWhitespace;
			if (configXmlWhitespace != null)
			{
				configXmlWhitespace._line = _line;
				configXmlWhitespace._filename = _filename;
			}
			return xmlNode;
		}
	}
}
