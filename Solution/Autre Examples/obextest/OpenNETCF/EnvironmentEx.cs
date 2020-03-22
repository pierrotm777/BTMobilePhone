//==========================================================================================
//
//		OpenNETCF.EnvironmentEx
//		Copyright (c) 2003-2004, OpenNETCF.org
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
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using OpenNETCF.Win32;

namespace OpenNETCF
{
	/// <summary>
	/// Extends the functionality of <see cref="T:System.Environment"/>
	/// </summary>
	/// <seealso cref="T:System.Environment">System.Environment Class</seealso>
	public class EnvironmentEx
	{
		//maximum supported length of a file path
		private const int MaxPath = 260;
		
		private EnvironmentEx(){}

		#region New Line
		/// <summary>
		/// Gets the newline string defined for this environment.
		/// </summary>
		/// <value>A string containing "\r\n".</value>
		/// <remarks>The property value is a constant customized specifically for the current platform.
		/// This value is automatically appended to text when using WriteLine methods, such as <see cref="M:T:System.Console.WriteLine(System.String)">Console.WriteLine</see>.</remarks>
		/// <seealso cref="P:System.Environment.NewLine">System.Environment.NewLine Property</seealso>
		public static string NewLine
		{
			get
			{
				return "\r\n";
			}
		}
		#endregion

		#region System Directory
		/// <summary>
		/// Gets the fully qualified path of the system directory.
		/// </summary>
		/// <value>A string containing a directory path.</value>
		/// <remarks>An example of the value returned is the string "\Windows".</remarks>
		/// <seealso cref="P:System.Environment.SystemDirectory">System.Environment.SystemDirectory Property</seealso>
		public static string SystemDirectory
		{
			get
			{
				try
				{
					return GetFolderPath(SpecialFolder.Windows);
				}
				catch
				{
					return "\\Windows";
				}
			}
		}
		#endregion

		#region Get Folder Path
		/// <summary>
		/// Gets the path to the system special folder identified by the specified enumeration.
		/// </summary>
		/// <param name="folder">An enumerated constant that identifies a system special folder.</param>
		/// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, the empty string ("").
		/// A folder will not physically exist if the operating system did not create it, the existing folder was deleted, or the folder is a virtual directory, such as My Computer, which does not correspond to a physical path.</returns>
		/// <seealso cref="M:System.Environment.GetFolderPath(System.Environment.SpecialFolder)">System.Environment.GetFolderPath Method</seealso>
		public static string GetFolderPath(SpecialFolder folder)
		{
			StringBuilder path = new StringBuilder(MaxPath + 2);

			if(!SHGetSpecialFolderPath(IntPtr.Zero, path, (int)folder, 0))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(),"Error retrieving folder path");
			}

			return path.ToString();
		}

		[DllImport("coredll.dll", EntryPoint="SHGetSpecialFolderPath", SetLastError=false)]
		internal static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, StringBuilder lpszPath, int nFolder, int fCreate);

		#endregion

		#region Special Folder
		/// <summary>
		/// Specifies enumerated constants used to retrieve directory paths to system special folders.
		/// </summary>
		/// <remarks>Not all platforms support all of these constants.</remarks>
		/// <seealso cref="T:System.Environment.SpecialFolder">System.Environment.SpecialFolder Enumeration</seealso>
		public enum SpecialFolder
		{
			// <summary>
			// Windows CE.NET 4.2 and above
			// </summary>
			//VirtualRoot		= 0x00,
			/// <summary>
			/// The directory that contains the user's program groups.
			/// </summary>
			Programs		= 0x02,       
			// <summary>
			// control panel icons
			// </summary>
			//Controls		= 0x03,
			// <summary>
			// printers folder
			// </summary>
			//Printers		= 0x04,
			/// <summary>
			/// The directory that serves as a common repository for documents. (Not supported in Pocket PC and Pocket PC 2002 - "\My Documents").
			/// </summary>
			Personal		= 0x05,
			/// <summary>
			/// The directory that serves as a common repository for the user's favorite items.
			/// </summary>
			Favorites		= 0x06,	
			/// <summary>
			/// The directory that corresponds to the user's Startup program group.   The system starts these programs whenever a user starts Windows CE.
			/// </summary>
			Startup			= 0x07,
			/// <summary>
			/// The directory that contains the user's most recently used documents.
			/// Not supported on Windows Mobile.
			/// </summary>
			Recent			= 0x08,	
			// <summary>
			// The directory that contains the Send To menu items.
			// </summary>
			//SendTo			= 0x09,
			// <summary>
			// Recycle bin.
			// </summary>
			//RecycleBin		= 0x0A,
			/// <summary>
			/// The directory that contains the Start menu items.
			/// </summary>
			StartMenu		= 0x0B,     
			/// <summary>
			/// The directory used to physically store file objects on the desktop.   Do not confuse this directory with the desktop folder itself, which is a virtual folder.
			/// Not supported on Windows Mobile.
			/// </summary>
			DesktopDirectory = 0x10,
			// <summary>
			// The "My Computer" folder.
			// </summary>
			//MyComputer		= 0x11,
			// <summary>
			// Network Neighbourhood
			// </summary>
			//NetNeighborhood = 0x12,
			/// <summary>
			/// The Fonts folder.
			/// </summary>
			Fonts			= 0x14,
			/// <summary>
			/// The directory that serves as a common repository for application-specific data for the current user.
			/// Not supported on Windows Mobile.
			/// </summary>
			ApplicationData	= 0x1a,
			/// <summary>
			/// The Windows folder.
			/// Not supported by Pocket PC and Pocket PC 2002.
			/// </summary>
			Windows			= 0x24,
			/// <summary>
			/// The program files directory.
			/// </summary>
			ProgramFiles	= 0x26,

		}
		#endregion

		#region Platform Name
		/// <summary>
		/// Returns a string which identifies the device platform
		/// </summary>
		/// <remarks>Valid values include:-
		/// <list type="bullet">
		/// <item><term>PocketPC</term><description>Pocket PC device or Emulator</description></item>
		/// <item><term>SmartPhone</term><description>Smartphone 2003 Device or Emulator</description></item>
		/// <item><term>CEPC platform</term><description>Windows CE.NET Emulator</description></item></list>
		/// Additional platform types will have other names.
		/// Useful when writing library code targetted at multiple platforms.</remarks>
		public static string PlatformName
		{
			get
			{
				//allocate buffer to receive value
				byte[] buffer = new byte[32];

				//call native function
				if(!OpenNETCF.Win32.Core.SystemParametersInfo(Core.SystemParametersInfoAction.GetPlatformType, buffer.Length, buffer, Core.SystemParametersInfoFlags.None))
				{
					throw new Win32Exception(Marshal.GetLastWin32Error(), "Retrieving platform name failed");
				}

				//get string from buffer contents
				string platformname = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length);

				//trim any trailing null characters
				return platformname.Substring(0, platformname.IndexOf("\0"));
			}
		}
		#endregion


		//v1.2 Features

		#region Get Logical Drives
		/// <summary>
		/// Returns an array of string containing the names of the logical drives on the current computer.
		/// <para><b>New in v1.2</b></para></summary>
		/// <returns>An array of string where each element contains the name of a logical drive.</returns>
		public static string[] GetLogicalDrives()
		{
			//storage cards are directories with the temporary attribute
			System.IO.FileAttributes attrStorageCard = System.IO.FileAttributes.Directory | System.IO.FileAttributes.Temporary;

			ArrayList drives = new ArrayList();

			drives.Add("\\");

			DirectoryInfo rootDir = new DirectoryInfo(@"\");
			
			foreach(DirectoryInfo di in rootDir.GetDirectories() )
			{
				//if directory and temporary
				if ( (di.Attributes & attrStorageCard) == attrStorageCard )
				{
					//add to collection of storage cards
					drives.Add(di.Name);
				}
			}
			return (string[])drives.ToArray(typeof(string));

		}
		#endregion

		#region Machine Name
		/// <summary>
		/// Gets the name of this local device.
		/// <para><b>New in v1.2</b></para></summary>
		public static string MachineName
		{
			get
			{
				string machineName = "";

				try
				{
					RegistryKey ident = Registry.LocalMachine.OpenSubKey("Ident");
					machineName = ident.GetValue("Name").ToString();
					ident.Close();
				}
				catch
				{
					throw new PlatformNotSupportedException();
				}
				
				return machineName;
			}
		}
		#endregion

		#region User Name
		/// <summary>
		/// Gets the user name of the person who started the current thread.
		/// <para><b>New in v1.2</b></para></summary>
		/// <remarks>Supported only on Pocket PC and Smartphone platforms.</remarks>
		public static string UserName
		{
			get
			{
				string userName = "";

				try
				{
					RegistryKey ownerKey = Registry.CurrentUser.OpenSubKey("ControlPanel\\Owner");
					byte[] ownerData = (byte[])ownerKey.GetValue("Owner");
					userName = System.Text.Encoding.Unicode.GetString(ownerData, 0, 72);
					userName = userName.Substring(0,userName.IndexOf("\0"));
				}
				catch
				{
				}

				return userName;
			}
		}
		#endregion


		#region Wrappers to System.Environment

		/// <summary>
		/// Gets an <see cref="System.OperatingSystem"/> object that contains the current platform identifier and version number. 
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		public static OperatingSystem OSVersion
		{
			get
			{
				return System.Environment.OSVersion;
			}
		}

		/// <summary>
		/// Gets the number of milliseconds elapsed since the system started.
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		public static int TickCount
		{
			get
			{
				return System.Environment.TickCount;
			}
		}

		/// <summary>
		/// Gets a <see cref="System.Version"/> object that describes the major, minor, build, and revision numbers of the common language runtime.
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		public static Version Version
		{
			get
			{
				return System.Environment.Version;
			}
		}
		#endregion
	}

	/// <summary>
	/// Obsolete. Use <see cref="EnvironmentEx"/> instead.
	/// </summary>
	/// <seealso cref="EnvironmentEx"/>
	[Obsolete("Use OpenNETCF.EnvironmentEx instead", true)]
	public class Environment
	{
		//maximum supported length of a file path
		private const int MaxPath = 260;
		
		private Environment(){}

		#region New Line
		/// <summary>
		/// Gets the newline string defined for this environment.
		/// </summary>
		/// <value>A string containing "\r\n".</value>
		/// <remarks>The property value is a constant customized specifically for the current platform.
		/// This value is automatically appended to text when using WriteLine methods, such as <see cref="M:T:System.Console.WriteLine(System.String)">Console.WriteLine</see>.</remarks>
		/// <seealso cref="P:System.Environment.NewLine">System.Environment.NewLine Property</seealso>
		public static string NewLine
		{
			get
			{
				return "\r\n";
			}
		}
		#endregion

		#region System Directory
		/// <summary>
		/// Gets the fully qualified path of the system directory.
		/// </summary>
		/// <value>A string containing a directory path.</value>
		/// <remarks>An example of the value returned is the string "\Windows".</remarks>
		/// <seealso cref="P:System.Environment.SystemDirectory">System.Environment.SystemDirectory Property</seealso>
		public static string SystemDirectory
		{
			get
			{
				try
				{
					return GetFolderPath(SpecialFolder.Windows);
				}
				catch
				{
					return "\\Windows";
				}
			}
		}
		#endregion

		#region Get Folder Path
		/// <summary>
		/// Gets the path to the system special folder identified by the specified enumeration.
		/// </summary>
		/// <param name="folder">An enumerated constant that identifies a system special folder.</param>
		/// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, the empty string ("").  A folder will not physically exist if the operating system did not create it, the existing folder was deleted, or the folder is a virtual directory, such as My Computer, which does not correspond to a physical path.</returns>
		/// <seealso cref="M:System.Environment.GetFolderPath(System.Environment.SpecialFolder)">System.Environment.GetFolderPath Method</seealso>
		public static string GetFolderPath(SpecialFolder folder)
		{
			StringBuilder path = new StringBuilder(MaxPath + 2);

			if(!SHGetSpecialFolderPath(IntPtr.Zero, path, (int)folder, 0))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(),"Error retrieving folder path");
			}

			return path.ToString();
		}

		[DllImport("coredll.dll", EntryPoint="SHGetSpecialFolderPath", SetLastError=false)]
		internal static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, StringBuilder lpszPath, int nFolder, int fCreate);

		#endregion

		#region Special Folder
		/// <summary>
		/// Specifies enumerated constants used to retrieve directory paths to system special folders.
		/// </summary>
		/// <remarks>Not all platforms support all of these constants.</remarks>
		/// <seealso cref="T:System.Environment.SpecialFolder">System.Environment.SpecialFolder Enumeration</seealso>
		public enum SpecialFolder
		{
			// <summary>
			// Windows CE.NET 4.2 and above
			// </summary>
			//VirtualRoot		= 0x00,
			/// <summary>
			/// The directory that contains the user's program groups.
			/// </summary>
			Programs		= 0x02,       
			// <summary>
			// control panel icons
			// </summary>
			//Controls		= 0x03,
			// <summary>
			// printers folder
			// </summary>
			//Printers		= 0x04,
			/// <summary>
			/// The directory that serves as a common repository for documents. (Not supported in Pocket PC and Pocket PC 2002 - "\My Documents").
			/// </summary>
			Personal		= 0x05,
			/// <summary>
			/// The directory that serves as a common repository for the user's favorite items.
			/// </summary>
			Favorites		= 0x06,	
			/// <summary>
			/// The directory that corresponds to the user's Startup program group.   The system starts these programs whenever a user starts Windows CE.
			/// </summary>
			Startup			= 0x07,
			/// <summary>
			/// The directory that contains the user's most recently used documents.
			/// Not supported on Windows Mobile.
			/// </summary>
			Recent			= 0x08,	
			// <summary>
			// The directory that contains the Send To menu items.
			// </summary>
			//SendTo			= 0x09,
			// <summary>
			// Recycle bin.
			// </summary>
			//RecycleBin		= 0x0A,
			/// <summary>
			/// The directory that contains the Start menu items.
			/// </summary>
			StartMenu		= 0x0B,     
			/// <summary>
			/// The directory used to physically store file objects on the desktop.   Do not confuse this directory with the desktop folder itself, which is a virtual folder.
			/// Not supported on Windows Mobile.
			/// </summary>
			DesktopDirectory = 0x10,
			// <summary>
			// The "My Computer" folder.
			// </summary>
			//MyComputer		= 0x11,
			// <summary>
			// Network Neighbourhood
			// </summary>
			//NetNeighborhood = 0x12,
			/// <summary>
			/// The Fonts folder.
			/// </summary>
			Fonts			= 0x14,
			/// <summary>
			/// The directory that serves as a common repository for application-specific data for the current user.
			/// Not supported on Windows Mobile.
			/// </summary>
			ApplicationData	= 0x1a,
			/// <summary>
			/// The Windows folder.
			/// Not supported by Pocket PC and Pocket PC 2002.
			/// </summary>
			Windows			= 0x24,
			/// <summary>
			/// The program files directory.
			/// </summary>
			ProgramFiles	= 0x26,

		}
		#endregion

		#region Platform Name
		/// <summary>
		/// Returns a string which identifies the device platform
		/// </summary>
		/// <remarks>Valid values include:-
		/// <list type="bullet">
		/// <item><term>PocketPC</term><description>Pocket PC device or Emulator</description></item>
		/// <item><term>SmartPhone</term><description>Smartphone 2003 Device or Emulator</description></item>
		/// <item><term>CEPC platform</term><description>Windows CE.NET Emulator</description></item></list>
		/// Additional platform types will have other names.
		/// Useful when writing library code targetted at multiple platforms.</remarks>
		public static string PlatformName
		{
			get
			{
				//allocate buffer to receive value
				byte[] buffer = new byte[32];

				//call native function
				if(!OpenNETCF.Win32.Core.SystemParametersInfo(Core.SystemParametersInfoAction.GetPlatformType, buffer.Length, buffer, Core.SystemParametersInfoFlags.None))
				{
					throw new Win32Exception(Marshal.GetLastWin32Error(), "Retrieving platform name failed");
				}

				//get string from buffer contents
				string platformname = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length);

				//trim any trailing null characters
				return platformname.Substring(0, platformname.IndexOf("\0"));
			}
		}
		#endregion


		#region Wrappers to System.Environment

		/// <summary>
		/// Gets an <see cref="System.OperatingSystem"/> object that contains the current platform identifier and version number. 
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		public static OperatingSystem OSVersion
		{
			get
			{
				return System.Environment.OSVersion;
			}
		}

		/// <summary>
		/// Gets the number of milliseconds elapsed since the system started.
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		public static int TickCount
		{
			get
			{
				return System.Environment.TickCount;
			}
		}

		/// <summary>
		/// Gets a <see cref="System.Version"/> object that describes the major, minor, build, and revision numbers of the common language runtime.
		/// <para><b>New in v1.1</b></para>
		/// </summary>
		public static Version Version
		{
			get
			{
				return System.Environment.Version;
			}
		}
		#endregion
	}
}
