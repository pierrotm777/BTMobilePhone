//==========================================================================================
//
//		OpenNETCF.Win32.WinAPIException
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
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Exception class for OpenNETCF WinAPI classes
	/// </summary>
	public class WinAPIException : Exception
	{
		private int win32Error;

		public WinAPIException(string Message) : base(Message + " " + GetErrorMessage(Marshal.GetLastWin32Error()))
		{
			this.win32Error = Marshal.GetLastWin32Error();
		}

		public WinAPIException(Exception ex) : base(ex.Message)
		{			
			this.win32Error = 0;
		}

		public WinAPIException(string Message, int ErrorCode) : base(Message + " " + GetErrorMessage(ErrorCode))
		{
			this.win32Error = ErrorCode;
		}
	
		public int Win32Error
		{
			get
			{
				return win32Error;
			}
		}

		internal static string GetErrorMessage(int ErrNo)
		{
			IntPtr pBuffer;
			int nLen = Win32.Core.FormatMessage(Core.FormatMessageFlags.FromSystem | Core.FormatMessageFlags.AllocateBuffer, 0, ErrNo, 0, out pBuffer, 0, null);
			if ( nLen == 0 )//Failed
			{
				return string.Format("Error {0} (0x{0:X})", ErrNo);
			}
			string sMsg = Marshal.PtrToStringUni(pBuffer, nLen);
			Core.LocalFreeCE(pBuffer);
			return sMsg;
		}
	}
}

