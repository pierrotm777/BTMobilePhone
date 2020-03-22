/*=====================================================================
  File:      ProtocolCommand.cs

  Summary:   Protocol command properties for GSMComm Demo application.

---------------------------------------------------------------------

This source code is intended only as a supplement to the GSMComm
development package or on-line documentation and may not be distributed
separately.
=====================================================================*/

using System;

namespace GSMCommDemo
{
	class ProtocolCommand
	{
		private string name;
		private bool needsData;
		private bool needsPattern;
		private bool needsError;

		public ProtocolCommand(string name)
		{
			this.name = name;
			this.needsData = false;
			this.needsPattern = false;
			this.needsError = false;
		}

		public ProtocolCommand(string name, bool needsData, bool needsPattern, bool needsError)
		{
			this.name = name;
			this.needsData = needsData;
			this.needsPattern = needsPattern;
			this.needsError = needsError;
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public bool NeedsData
		{
			get { return needsData; }
			set { needsData = value; }
		}

		public bool NeedsPattern
		{
			get { return needsPattern; }
			set { needsPattern = value; }
		}

		public bool NeedsError
		{
			get { return needsError; }
			set { needsError = value; }
		}
	}

}
