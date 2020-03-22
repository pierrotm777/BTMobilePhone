//==========================================================================================
//
//		OpenNETCF.Win32.Core
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
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;

namespace OpenNETCF.Win32
{
	
	#region Disabled public delegates
	#if!NDOC
	/* See OpenNETCF.Windows.Forms.NotifyIcon
	/// <summary>
	/// Handles notifications from Tray Icon
	/// </summary>
	public delegate bool BeforeIconMessageEventHandler(ref Message msg);
	/// <summary>
	/// Handles notifications from Tray Icon
	/// </summary>
	public delegate void AfterIconMessageEventHandler(ref Message msg);*/
	#endif
	#endregion

	/// <summary>
	/// OpenNETCF Win32 API Wrapper Class for CoreDLL.dll
	/// </summary>
	public sealed class Core
	{
		#region --------------- Library Constants and enums ---------------

		internal const uint	CLR_INVALID				= 0xFFFFFFFF;
		//public const uint	WAIT_OBJECT_0   		= 0x00000000;
		//public const uint	WAIT_ABANDONED  		= 0x00000080;
		//public const uint	WAIT_ABANDONED_0		= 0x00000080;
		//public const uint	WAIT_FAILED				= 0xffffffff;
		internal const uint	INFINITE				= 0xffffffff;
		/// <summary>
		/// The returned handle is not valid.
		/// </summary>
		public const int	INVALID_HANDLE_VALUE	= -1;
		
		#region Wait Enumeration
		/// <summary>
		/// Responses from <see cref="WaitForSingleObject"/> function.
		/// </summary>
		[CLSCompliant(false)]
		public enum Wait : uint
		{
			/// <summary>
			/// The state of the specified object is signaled.
			/// </summary>
			Object		= 0x00000000,
			/// <summary>
			/// Wait abandoned.
			/// </summary>
			Abandoned	= 0x00000080,
			/// <summary>
			/// Wait failed.
			/// </summary>
			Failed		= 0xffffffff,
		}
		#endregion

		#region Format Message Flags Enumeration
		/// <summary>
		/// Specifies aspects of the formatting process and how to interpret the lpSource parameter.
		/// </summary>
		/// <remarks>The low-order byte of dwFlags specifies how the function handles line breaks in the output buffer.
		/// The low-order byte can also specify the maximum width of a formatted output line.</remarks>
		[Flags]
		public enum FormatMessageFlags : int
		{
			/// <summary>
			/// The function allocates a buffer large enough to hold the formatted message, and places a pointer to the allocated buffer at the address specified by lpBuffer.
			/// </summary>
			AllocateBuffer = 0x00000100,
			/// <summary>
			/// Insert sequences in the message definition are to be ignored and passed through to the output buffer unchanged.
			/// </summary>
			IgnoreInserts  = 0x00000200,
			/// <summary>
			/// Specifies that lpSource is a pointer to a null-terminated message definition.
			/// </summary>
			FromString     = 0x00000400,
			/// <summary>
			/// Specifies that lpSource is a module handle containing the message-table resource(s) to search.
			/// </summary>
			FromHModule    = 0x00000800,
			/// <summary>
			/// Specifies that the function should search the system message-table resource(s) for the requested message.
			/// </summary>
			FromSystem     = 0x00001000,
			/// <summary>
			/// Specifies that the Arguments parameter is not a va_list structure, but instead is just a pointer to an array of 32-bit values that represent the arguments.
			/// </summary>
			ArgumentArray  = 0x00002000,
			/// <summary>
			/// Use the <b>MaxWidthMask</b> constant and bitwise Boolean operations to set and retrieve this maximum width value.
			/// </summary>
			MaxWidthMask  = 0x000000FF,
		}
		#endregion


		//internal const uint CF_UNICODETEXT          = 13;

		/*internal const uint NIF_MESSAGE = 0x1;
		internal const uint NIF_ICON	= 0x2;
		internal const uint NIF_TIP		= 0x4;
		internal const uint NIM_ADD		= 0;
		internal const uint NIM_DELETE  = 2;
		internal const uint NIM_MODIFY  = 1;*/
		
		/// <summary>   
		/// This message is posted when the user presses the touch-screen in the client area of a window.   
		/// </summary>      
		internal const int WM_LBUTTONDOWN	= 0x0201;
		/// <summary>   
		/// This message is used by applications to help define private messages.   
		/// </summary> 
		internal const int WM_USER			= 0x0400;

		/// <summary>
		/// 
		/// </summary>
		public const int	MK_LBUTTON		= 0x0001;
		/// <summary>
		/// 
		/// </summary>
		public const int	MK_RBUTTON		= 0x0002;
		/// <summary>
		/// 
		/// </summary>
		public const int	MK_SHIFT		= 0x0004;
		/// <summary>
		/// 
		/// </summary>
		public const int	MK_CONTROL		= 0x0008;

		internal static Int32 METHOD_BUFFERED = 0;
		internal static Int32 FILE_ANY_ACCESS = 0;
		internal static Int32 FILE_DEVICE_HAL = 0x00000101;
		internal static Int32 IOCTL_HAL_GET_DEVICEID = ((FILE_DEVICE_HAL) << 16) |
			((FILE_ANY_ACCESS) << 14) | ((21) << 2) | (METHOD_BUFFERED);

		private enum KeyEvents
		{
			ExtendedKey = 0x0001,
			KeyUp       = 0x0002,
			Silent      = 0x0004
		}

		private enum EventFlags
		{
			Pulse     = 1,
			Reset     = 2,
			Set       = 3
		}

		/// <summary>
		/// Region type
		/// </summary>
		public enum GDIRegion : int
		{
			/// <summary>
			/// 
			/// </summary>
			NULLREGION = 1,
			/// <summary>
			/// 
			/// </summary>
			SIMPLEREGION = 2,
			/// <summary>
			/// 
			/// </summary>
			COMPLEXREGION = 3
		}

		/// <summary>
		/// Background mode
		/// </summary>
		public enum BackMode : int
		{
			/// <summary>
			/// Background is Transparent.
			/// </summary>
			TRANSPARENT = 1,
			/// <summary>
			/// Background is Opaque.
			/// </summary>
			OPAQUE= 2
		}
    
		/// <summary>
		/// LocalAlloc flags
		/// </summary>
		public enum MemoryAllocFlags : int
		{
			/// <summary>   
			/// Allocates fixed memory.   
			/// The return value is a pointer to the memory object.   
			/// </summary>   
			Fixed		= 0x00,
			/// <summary>   
			/// Initializes memory contents to zero.   
			/// </summary>      
			ZeroInit	= 0x40,
			/// <summary>   
			/// Combines the Fixed and ZeroInit flags.   
			/// </summary>     
			LPtr			= ZeroInit | Fixed
		}		

		#region KeyStateFlags
		/// <summary>
		/// KeyStateFlags for Keyboard methods
		/// </summary>
		[Flags()]
		public enum KeyStateFlags : int
		{
			/// <summary>
			/// Key is toggled.
			/// </summary>
			Toggled		= 0x0001,
			/// <summary>
			/// 
			/// </summary>
			AsyncDown	= 0x0002,		//	 went down since last GetAsync call.
			/// <summary>
			/// Key was previously down.
			/// </summary>
			PrevDown	= 0x0040,
			/// <summary>
			/// Key is currently down.
			/// </summary>
			Down		= 0x0080,
			/// <summary>
			/// Left or right CTRL key is down.
			/// </summary>
			AnyCtrl		= 0x40000000,
			/// <summary>
			/// Left or right SHIFT key is down.
			/// </summary>
			AnyShift	= 0x20000000,
			/// <summary>
			/// Left or right ALT key is down.
			/// </summary>
			AnyAlt		= 0x10000000,
			/// <summary>
			/// VK_CAPITAL is toggled.
			/// </summary>
			Capital		= 0x08000000,
			/// <summary>
			/// Left CTRL key is down.
			/// </summary>
			LeftCtrl	= 0x04000000,
			/// <summary>
			/// Left SHIFT key is down.
			/// </summary>
			LeftShift	= 0x02000000,
			/// <summary>
			/// Left ALT key is down.
			/// </summary>
			LeftAlt		= 0x01000000,
			/// <summary>
			/// Left Windows logo key is down.
			/// </summary>
			LeftWin		= 0x00800000,
			/// <summary>
			/// Right CTRL key is down.
			/// </summary>
			RightCtrl	= 0x00400000,
			/// <summary>
			/// Right SHIFT key is down
			/// </summary>
			RightShift	= 0x00200000,
			/// <summary>
			/// Right ALT key is down
			/// </summary>
			RightAlt	= 0x00100000,
			/// <summary>
			/// Right Windows logo key is down.
			/// </summary>
			RightWin	= 0x00080000,
			/// <summary>
			/// Corresponding character is dead character.
			/// </summary>
			Dead		= 0x00020000,
			/// <summary>
			/// No characters in pCharacterBuffer to translate.
			/// </summary>
			NoCharacter	= 0x00010000,
			/// <summary>
			/// Use for language specific shifts.
			/// </summary>
			Language1	= 0x00008000,
			/// <summary>
			/// NumLock toggled state.
			/// </summary>
			NumLock		= 0x00001000,
		}
		#endregion

		
		

		#region WaveInCaps
		/// <summary>
		/// Class for getting audio device capabilities
		/// </summary>
		public class WaveInCaps
		{
			private const int MAXPNAMELEN = 32;
			
			private const int wMIDOffset			= 0;
			private const int wPIDOffset			= wMIDOffset + 2;
			private const int vDriverVersionOffset	= wPIDOffset + 2;
			private const int szPnameOffset			= vDriverVersionOffset + 4;
			private const int dwFormatsOffset		= szPnameOffset + MAXPNAMELEN * 2;
			private const int wChannelsOffset		= dwFormatsOffset + 4;
			private const int wReserved1Offset		= wChannelsOffset + 2;

			private byte[] flatStruct = new byte[2 + 2 + 4 + MAXPNAMELEN * 2 + 4 + 2 + 2];

			public byte[] ToByteArray()
			{
				return flatStruct;
			}

			public static implicit operator byte[]( WaveInCaps wic )
			{
				return wic.flatStruct;
			}

			public WaveInCaps()
			{
				Array.Clear(flatStruct, 0, flatStruct.Length);
			}

			public WaveInCaps( byte[] bytes ) : this( bytes, 0 )
			{
			}

			public WaveInCaps( byte[] bytes, int offset )
			{
				Buffer.BlockCopy( bytes, offset, flatStruct, 0, flatStruct.Length );
			}
			
			public short MID
			{
				get
				{
					return BitConverter.ToInt16(flatStruct, wMIDOffset);
				}
				set
				{
					byte[]	bytes = BitConverter.GetBytes( value );
					Buffer.BlockCopy( bytes, 0, flatStruct, wMIDOffset, Marshal.SizeOf(value));
				}
			}

			public short PID
			{
				get
				{
					return BitConverter.ToInt16(flatStruct, wPIDOffset);
				}
				set
				{
					byte[]	bytes = BitConverter.GetBytes( value );
					Buffer.BlockCopy( bytes, 0, flatStruct, wPIDOffset, Marshal.SizeOf(value));
				}
			}

			public int DriverVersion
			{
				get
				{
					return BitConverter.ToInt32(flatStruct, vDriverVersionOffset);
				}
				set
				{
					byte[]	bytes = BitConverter.GetBytes( value );
					Buffer.BlockCopy( bytes, 0, flatStruct, vDriverVersionOffset, Marshal.SizeOf(value));
				}
			}

			public string szPname
			{
				get
				{
					return Encoding.Unicode.GetString(flatStruct, szPnameOffset, MAXPNAMELEN * 2).Trim('\0');
				}
			}
		
			public int Formats
			{
				get
				{
					return BitConverter.ToInt32(flatStruct, dwFormatsOffset);
				}
				set
				{
					byte[]	bytes = BitConverter.GetBytes( value );
					Buffer.BlockCopy( bytes, 0, flatStruct, dwFormatsOffset, Marshal.SizeOf(value));
				}
			}

			public short Channels
			{
				get
				{
					return BitConverter.ToInt16(flatStruct, wChannelsOffset);
				}
				set
				{
					byte[]	bytes = BitConverter.GetBytes( value );
					Buffer.BlockCopy( bytes, 0, flatStruct, wChannelsOffset, Marshal.SizeOf(value));
				}
			}

			public short wReserved1
			{
				get
				{
					return BitConverter.ToInt16(flatStruct, wReserved1Offset);
				}
				set
				{
					byte[]	bytes = BitConverter.GetBytes( value );
					Buffer.BlockCopy( bytes, 0, flatStruct, wReserved1Offset, Marshal.SizeOf(value));
				}
			}
		}
		#endregion

		#region ProcessorArchitecture
		/// <summary>
		/// Processor Architecture values (GetSystemInfo)
		/// </summary>
		/// <seealso cref="M:OpenNETCF.WinAPI.Core.GetSystemInfo(OpenNETCF.WinAPI.Core.SYSTEM_INFO)"/>
		[CLSCompliant(false)]
		public enum ProcessorArchitecture : ushort
		{
			/// <summary>
			/// Processor is Intel x86 based.
			/// </summary>
			Intel	= 0,
			/// <summary>
			/// Processor is MIPS based.
			/// </summary>
			MIPS	= 1,
			/// <summary>
			/// Processor is Alpha based.
			/// </summary>
			Alpha	= 2,
			/// <summary>
			/// Processor is Power PC based.
			/// </summary>
			PPC		= 3,
			/// <summary>
			/// Processor is SH3, SH4 etc.
			/// </summary>
			SHX		= 4,
			/// <summary>
			/// Processor is ARM based.
			/// </summary>
			ARM		= 5,
			/// <summary>
			/// Processor is Intel 64bit.
			/// </summary>
			IA64	= 6,
			/// <summary>
			/// Processor is Alpha 64bit.
			/// </summary>
			Alpha64 = 7,
			/// <summary>
			/// Unknown processor architecture.
			/// </summary>
			Unknown = 0xFFFF
		}
		#endregion

		#region Processor Type
		/// <summary>
		/// Processor type values (GetSystemInfo)
		/// </summary>
		/// <seealso cref="M:OpenNETCF.Win32.Core.GetSystemInfo(OpenNETCF.Win32.Core.SYSTEM_INFO)"/>
		public enum ProcessorType : int
		{
			/// <summary>
			/// Processor is Intel 80386.
			/// </summary>
			Intel_386			= 386,
			/// <summary>
			/// Processor is Intel 80486.
			/// </summary>
			Intel_486			= 486,
			/// <summary>
			/// Processor is Intel Pentium (80586).
			/// </summary>
			Intel_Pentium		= 586,
			/// <summary>
			/// Processor is Intel Pentium II (80686).
			/// </summary>
			Intel_PentiumII		= 686,
			/// <summary>
			/// Processor is Intel 64bit (IA64).
			/// </summary>
			Intel_IA64		= 2200,
			/// <summary>
			/// Processor is MIPS R4000.
			/// </summary>
			MIPS_R4000        = 4000,
			/// <summary>
			/// Processor is Alpha 21064.
			/// </summary>
			Alpha_21064       = 21064,
			/// <summary>
			/// Processor is Power PC 403.
			/// </summary>
			PPC_403           = 403,
			/// <summary>
			/// Processor is Power PC 601.
			/// </summary>
			PPC_601           = 601,
			/// <summary>
			/// Processor is Power PC 603.
			/// </summary>
			PPC_603           = 603,
			/// <summary>
			/// Processor is Power PC 604.
			/// </summary>
			PPC_604           = 604,
			/// <summary>
			/// Processor is Power PC 620.
			/// </summary>
			PPC_620           = 620,
			/// <summary>
			/// Processor is Hitachi SH3.
			/// </summary>
			Hitachi_SH3       = 10003,
			/// <summary>
			/// Processor is Hitachi SH3E.
			/// </summary>
			Hitachi_SH3E      = 10004,
			/// <summary>
			/// Processor is Hitachi SH4.
			/// </summary>
			Hitachi_SH4       = 10005,
			/// <summary>
			/// Processor is Motorola 821.
			/// </summary>
			Motorola_821      = 821,
			/// <summary>
			/// Processor is SH3.
			/// </summary>
			SHx_SH3           = 103,
			/// <summary>
			/// Processor is SH4.
			/// </summary>
			SHx_SH4           = 104,
			/// <summary>
			/// Processor is StrongARM.
			/// </summary>
			StrongARM         = 2577,
			/// <summary>
			/// Processor is ARM 720.
			/// </summary>
			ARM720            = 1824,
			/// <summary>
			/// Processor is ARM 820.
			/// </summary>
			ARM820            = 2080,
			/// <summary>
			/// Processor is ARM 920.
			/// </summary>
			ARM920            = 2336,
			/// <summary>
			/// Processor is ARM 7 TDMI.
			/// </summary>
			ARM_7TDMI         = 70001
		}
		#endregion

		#region SystemInfo
		/// <summary>
		/// This structure contains information about the current computer system. This includes the processor type, page size, memory addresses, and OEM identifier.
		/// </summary>
		/// <seealso cref="GetSystemInfo"/>
		public struct SystemInfo
		{
			/// <summary>
			/// The system's processor architecture.
			/// </summary>
			[CLSCompliant(false)]
			public ProcessorArchitecture wProcessorArchitecture;

			internal ushort wReserved;
			/// <summary>
			/// The page size and the granularity of page protection and commitment.
			/// </summary>
			public int PageSize;
			/// <summary>
			/// Pointer to the lowest memory address accessible to applications and dynamic-link libraries (DLLs). 
			/// </summary>
			public int MinimumApplicationAddress;
			/// <summary>
			/// Pointer to the highest memory address accessible to applications and DLLs.
			/// </summary>
			public int MaximumApplicationAddress;
			/// <summary>
			/// Specifies a mask representing the set of processors configured into the system. Bit 0 is processor 0; bit 31 is processor 31. 
			/// </summary>
			public int ActiveProcessorMask;
			/// <summary>
			/// Specifies the number of processors in the system.
			/// </summary>
			public int NumberOfProcessors;
			/// <summary>
			/// Specifies the type of processor in the system.
			/// </summary>
			public ProcessorType ProcessorType;
			/// <summary>
			/// Specifies the granularity with which virtual memory is allocated.
			/// </summary>
			public int AllocationGranularity;
			/// <summary>
			/// Specifies the system’s architecture-dependent processor level.
			/// </summary>
			public short ProcessorLevel;
			/// <summary>
			/// Specifies an architecture-dependent processor revision.
			/// </summary>
			public short ProcessorRevision;
		}
		#endregion

		#region SystemParametersInfoAction Enumeration
		/// <summary>
		/// Specifies the system-wide parameter to query or set.
		/// </summary>
		public enum SystemParametersInfoAction : int
		{
			/// <summary>
			/// Retrieves the two mouse threshold values and the mouse speed.
			/// </summary>
			GetMouse = 3,
			/// <summary>
			/// Sets the two mouse threshold values and the mouse speed.
			/// </summary>
			SetMouse = 4,

			/// <summary>
			/// For Windows CE 2.12 and later, sets the desktop wallpaper.
			/// </summary>
			SetDeskWallpaper = 20,
			/// <summary>
			/// 
			/// </summary>
			SetDeskPattern = 21,

			/// <summary>
			/// Sets the size of the work area — the portion of the screen not obscured by the system taskbar or by toolbars displayed on the desktop by applications.
			/// </summary>
			SetWorkArea = 47,
			/// <summary>
			/// Retrieves the size of the work area on the primary screen.
			/// </summary>
			GetWorkArea = 48,

			/// <summary>
			/// Retrieves whether the show sounds option is on or off.
			/// </summary>
			GetShowSounds = 56,
			/// <summary>
			/// Turns the show sounds accessibility option on or off.
			/// </summary>
			SetShowSounds = 57,

			/// <summary>
			/// Gets the number of lines to scroll when the mouse wheel is rotated.
			/// </summary>
			GetWheelScrollLines = 104,
			/// <summary>
			/// Sets the number of lines to scroll when the mouse wheel is rotated.
			/// </summary>
			SetWheelScrollLines = 105,

			/// <summary>
			/// Retrieves a contrast value that is used in smoothing text displayed using Microsoft® ClearType®.
			/// </summary>
			GetFontSmoothingContrast = 0x200C,
			/// <summary>
			/// Sets the contrast value used when displaying text in a ClearType font.
			/// </summary>
			SetFontSmoothingContrast = 0x200D,

			/// <summary>
			/// Retrieves the screen saver time-out value, in seconds.
			/// </summary>
			GetScreenSaveTimeout = 14,
			/// <summary>
			/// Sets the screen saver time-out value to the value of the uiParam parameter.
			/// </summary>
			SetScreenSaveTimeout = 15,

			/// <summary>
			/// Sets the amount of time that Windows CE will stay on with battery power before it suspends due to user inaction.
			/// </summary>
			SetBatteryIdleTimeout = 251,
			/// <summary>
			/// Retrieves the amount of time that Windows CE will stay on with battery power before it suspends due to user inaction.
			/// </summary>
			GetBatteryIdleTimeout = 252,

			/// <summary>
			/// Sets the amount of time that Windows CE will stay on with AC power before it suspends due to user inaction.
			/// </summary>
			SetExternalIdleTimeout = 253,
			/// <summary>
			/// Retrieves the amount of time that Windows CE will stay on with AC power before it suspends due to user inaction.
			/// </summary>
			GetExternalIdleTimeout = 254,

			/// <summary>
			/// Sets the amount of time that Windows CE will stay on after a user notification that reactivates the suspended device.
			/// </summary>
			SetWakeupIdleTimeout = 255,
			/// <summary>
			/// Retrieves the amount of time that Windows CE will stay on after a user notification that reactivates a suspended device.
			/// </summary>
			GetWakeupIdleTimeout = 256,

			// @CESYSGEN ENDIF

			//The following flags also used with WM_SETTINGCHANGE
			// so don't use the values for future SPI_*

			//#define SETTINGCHANGE_START  		0x3001
			//#define SETTINGCHANGE_RESET  		0x3002
			//#define SETTINGCHANGE_END    		0x3003

			/// <summary>
			/// Get the platform name e.g. PocketPC, Smartphone etc.
			/// </summary>
			GetPlatformType = 257,
			/// <summary>
			/// Get OEM specific information.
			/// </summary>
			GetOemInfo = 258,

		}
		#endregion

		#region SystemParametersInfoFlags Enumeration
		/// <summary>
		/// Specifies whether the user profile is to be updated, and if so, whether the WM_SETTINGCHANGE message is to be broadcast to all top-level windows to notify them of the change.
		/// </summary>
		public enum SystemParametersInfoFlags : int
		{
			/// <summary>
			/// No notifications are sent on settings changed.
			/// </summary>
			None = 0,
			/// <summary>
			/// Writes the new system-wide parameter setting to the user profile.
			/// </summary>
			UpdateIniFile = 0x0001,
			/// <summary>
			/// Broadcasts the WM_SETTINGCHANGE message after updating the user profile.
			/// </summary>
			SendChange = 0x0002,
		}
		#endregion

		#region System Power Status

		/// <summary>
		/// AC power status.
		/// </summary>
		/// <remarks>Used by <see cref="T:OpenNETCF.Win32.Core.SystemPowerStatus"/> Structure.</remarks>
		public enum ACLineStatus : byte
		{
			/// <summary>
			/// AC power is offline.
			/// </summary>
			Offline         = 0x00,
			/// <summary>
			/// AC power is online.
			/// </summary>
			Online			= 0x01,
			/// <summary>
			/// Unit is on backup power.
			/// </summary>
			BackupPower		= 0x02,
			/// <summary>
			/// AC line status is unknown.
			/// </summary>
			Unknown			= 0xFF,
		}

		/// <summary>
		/// Battery charge status.
		/// </summary>
		[Flags()]
		public enum BatteryFlag : byte
		{
			/// <summary>
			/// Battery is high.
			/// </summary>
			High		= 0x01,
			/// <summary>
			/// Battery is low.
			/// </summary>
			Low			= 0x02,
			/// <summary>
			/// Battery is critically low.
			/// </summary>
			Critical	= 0x04,
			/// <summary>
			/// Battery is currently charging.
			/// </summary>
			Charging	= 0x08,
			/// <summary>
			/// No battery is attached.
			/// </summary>
			NoBattery	= 0x80,
			/// <summary>
			/// Battery charge status is unknown.
			/// </summary>
			Unknown		= 0xFF,

		}

		/// <summary>
		/// The remaining battery power is unknown.
		/// </summary>
		public const byte BatteryPercentageUnknown = 0xFF;

		internal const uint BatteryLifeUnknown = 0xFFFFFFFF;

		
		/// <summary>
		/// This structure contains information about the power status of the system.
		/// </summary>
		public struct SystemPowerStatus 
		{
			/// <summary>
			/// AC power status.
			/// </summary>
			public ACLineStatus ACLineStatus;
			/// <summary>
			/// Battery charge status.
			/// </summary>
			public BatteryFlag BatteryFlag;
			/// <summary>
			/// Percentage of full battery charge remaining.
			/// This member can be a value in the range 0 to 100, or 255 if status is unknown.
			/// All other values are reserved.
			/// </summary>
			public byte BatteryLifePercent;
			internal byte Reserved1;
			/// <summary>
			/// Number of seconds of battery life remaining, or 0xFFFFFFFF if remaining seconds are unknown.
			/// </summary>
			public int BatteryLifeTime;
			/// <summary>
			/// Number of seconds of battery life when at full charge, or 0xFFFFFFFF if full lifetime is unknown.
			/// </summary>
			private uint dwBatteryFullLifeTime;
			/// <summary>
			/// Number of seconds of battery life when at full charge, or 0 if full lifetime is unknown.
			/// </summary>
			public int BatteryFullLifeTime
			{
				get
				{
					if(dwBatteryFullLifeTime==BatteryLifeUnknown)
					{
						return 0;
					}
					else
					{
						return Convert.ToInt32(dwBatteryFullLifeTime);
					}
				}
			}
			internal byte Reserved2;
			/// <summary>
			/// Battery charge status.
			/// </summary>
			public BatteryFlag BackupBatteryFlag;
			/// <summary>
			/// Percentage of full backup battery charge remaining. Must be in the range 0 to 100.
			/// </summary>
			public byte BackupBatteryLifePercent;
			internal byte Reserved3;
			/// <summary>
			/// Number of seconds of backup battery life remaining.
			/// </summary>
			private uint dwBackupBatteryLifeTime;
			/// <summary>
			/// Number of seconds of backup battery life remaining.
			/// </summary>
			public int BackupBatteryLifeTime
			{
				get
				{
					if(dwBackupBatteryLifeTime==BatteryLifeUnknown)
					{
						return 0;
					}
					else
					{
						return Convert.ToInt32(dwBackupBatteryLifeTime);
					}
				}
			}
			/// <summary>
			/// Number of seconds of backup battery life when at full charge.
			/// </summary>
			private uint dwBackupBatteryFullLifeTime;
			/// <summary>
			/// Number of seconds of backup battery life when at full charge.
			/// </summary>
			public int BackupBatteryFullLifeTime
			{
				get
				{
					if(dwBackupBatteryFullLifeTime==BatteryLifeUnknown)
					{
						return 0;
					}
					else
					{
						return Convert.ToInt32(dwBackupBatteryFullLifeTime);
					}
				}
			}
		}
		#endregion

		#region Disabled PlatformType
		// <summary>
		// OSVERSIONINFO platform type
		// </summary>
		/*public enum PlatformType : uint
		{
			/// <summary>
			/// Win32s on Windows 3.1.
			/// </summary>
			VER_PLATFORM_WIN32s			= 0,
			/// <summary>
			/// Win32 on Windows 95 or Windows 98.
			/// For Windows 95, dwMinorVersion is zero. 
			/// For Windows 98, dwMinorVersion is greater than zero.
			/// </summary>
			VER_PLATFORM_WIN32_WINDOWS	= 1,	
			/// <summary>
			/// //Win32 on Windows NT.
			/// </summary>
			VER_PLATFORM_WIN32_NT		= 2,	
			/// <summary>
			/// Win32 on Windows CE.
			/// </summary>
			VER_PLATFORM_WIN32_CE		= 3
		}*/
		#endregion

		#region Disabled Registry
		/*/// <summary>
		/// Root key enum for RegCreateKey(Ex)
		/// </summary>
		public enum RootKey : uint 
		{
			HKEY_CLASSES_ROOT = 0x80000000, 
			HKEY_CURRENT_USER = 0x80000001, 
			HKEY_LOCAL_MACHINE = 0x80000002,

			HKEY_USERS = 0x80000003 
		} 

		/// <summary>
		/// Key disposition for RegCreateKey(Ex)
		/// </summary>
		public enum KeyDisposition : int 
		{
			REG_CREATED_NEW_KEY = 1, 
			REG_OPENED_EXISTING_KEY = 2 
		} 

		/// <summary>
		/// Key type for RegCreateKey(Ex)
		/// </summary>
		public enum KeyType : int
		{
			REG_SZ = 1,
			REG_EXPAND_SZ = 2,
			REG_BINARY = 3,
			REG_DWORD = 4,
		}*/
		#endregion

		#region Disabled OsVersion
		/*public struct OSVERSIONINFO
		{
			internal int dwOSVersionInfoSize;
			/// <summary>
			/// Identifies the major version number of the OS.
			/// </summary>
			/// <remarks>For example, for Windows CE 2.10, the major version number is 2.</remarks>
			public int dwMajorVersion;
			/// <summary>
			/// Identifies the minor version number of the OS.
			/// </summary>
			/// <remarks>For example, for Windows CE 2.10, the minor version number is 1.</remarks>
			public int dwMinorVersion;
			/// <summary>
			/// This identifies the build number of the OS or is set to 0.
			/// </summary>
			public int dwBuildNumber;
			/// <summary>
			/// This identifies the OS.
			/// </summary>
			public PlatformType dwPlatformId;

			public OSVERSIONINFO()
			{
				//set the size member to the size of this structure
				dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
			}
		}*/
		#endregion

		#region Memory Status
		/// <summary>
		/// This structure contains information about current memory availability. The GlobalMemoryStatus function uses this structure.
		/// </summary>
		public struct MemoryStatus
		{
			internal uint dwLength;
			/// <summary>
			/// Specifies a number between 0 and 100 that gives a general idea of current memory utilization, in which 0 indicates no memory use and 100 indicates full memory use.
			/// </summary>
			public int MemoryLoad;
			/// <summary>
			/// Indicates the total number of bytes of physical memory.
			/// </summary>
			public int TotalPhysical;
			/// <summary>
			/// Indicates the number of bytes of physical memory available.
			/// </summary>
			public int AvailablePhysical;
			/// <summary>
			/// Indicates the total number of bytes that can be stored in the paging file. Note that this number does not represent the actual physical size of the paging file on disk.
			/// </summary>
			public int TotalPageFile; 
			/// <summary>
			/// Indicates the number of bytes available in the paging file.
			/// </summary>
			public int AvailablePageFile; 
			/// <summary>
			/// Indicates the total number of bytes that can be described in the user mode portion of the virtual address space of the calling process.
			/// </summary>
			public int TotalVirtual; 
			/// <summary>
			/// Indicates the number of bytes of unreserved and uncommitted memory in the user mode portion of the virtual address space of the calling process.
			/// </summary>
			public int AvailableVirtual;
		}
		#endregion

		#endregion ---------------------------------------------
    
		

		#region --------------- Multimedia API Calls ---------------
		

		/// <summary>
		/// Set the volume for the default waveOut device (device ID = 0)
		/// </summary>
		/// <param name="Volume"></param>
		public void SetVolume(int Volume)
		{
			WaveFormatEx	format		= new WaveFormatEx();
			IntPtr			hWaveOut	= IntPtr.Zero;

			waveOutOpen(out hWaveOut, 0, format, IntPtr.Zero, 0, 0);
			waveOutSetVolume(hWaveOut, Volume);
			waveOutClose(hWaveOut);
		}

		/// <summary>
		/// Set the volume for an already-open waveOut device
		/// </summary>
		/// <param name="hWaveOut"></param>
		/// <param name="Volume"></param>
		public void SetVolume(IntPtr hWaveOut, int Volume)
		{
			waveOutSetVolume(hWaveOut, Volume);
		}

		/// <summary>
		/// Get the current volume setting for the default waveOut device (device ID = 0)
		/// </summary>
		/// <returns></returns>
		public int GetVolume()
		{
			WaveFormatEx	format		= new WaveFormatEx();
			IntPtr			hWaveOut	= IntPtr.Zero;
			int			volume		= 0;

			waveOutOpen(out hWaveOut, 0, format, IntPtr.Zero, 0, 0);
			waveOutGetVolume(hWaveOut, ref volume);
			waveOutClose(hWaveOut);

			return volume;
		}

		/// <summary>
		/// Set the current volume setting for an already-open waveOut device
		/// </summary>
		/// <param name="hWaveOut"></param>
		/// <returns></returns>
		public int GetVolume(IntPtr hWaveOut)
		{
			int			volume		= 0;

			waveOutGetVolume(hWaveOut, ref volume);

			return volume;
		}
		#endregion ---------------------------------------------

		#region --------------- Memory API Calls ---------------
		/// <summary>
		/// Obsolete. Allocate unmanaged memory.  Memory allocated must be released with LocalFree
		/// </summary>
		/// <param name="uFlags"></param>
		/// <param name="uBytes"></param>
		/// <returns></returns>
		/// <seealso cref="OpenNETCF.Runtime.InteropServices.MarshalEx.AllocHGlobal"/>
		[Obsolete("Use OpenNETCF.Runtime.InteropServices.MarshalEx.AllocHGlobal instead", false)]
		public static IntPtr LocalAlloc(MemoryAllocFlags uFlags, int uBytes)
		{
			return OpenNETCF.Runtime.InteropServices.MarshalEx.AllocHGlobal(uBytes);

			/*IntPtr ptr = IntPtr.Zero;

			ptr = LocalAllocCE((int)uFlags,uBytes);

			if(ptr == IntPtr.Zero)
			{
				throw new WinAPIException("Allocation Failed");
			}

			return ptr;*/
		}

		/// <summary>
		/// Obsolete. This function changes the size or the attributes of a specified local memory object. The size can increase or decrease.
		/// </summary>
		/// <param name="hMemory"></param>
		/// <param name="uBytes"></param>
		/// <returns></returns>
		/// <remarks>Use <see cref="OpenNETCF.Runtime.InteropServices.MarshalEx.ReAllocHGlobal"/> instead.</remarks>
		[Obsolete("Use OpenNETCF.Runtime.InteropServices.MarshalEx.ReAllocHGlobal instead")]
		public static IntPtr LocalReAlloc(IntPtr hMemory, int uBytes)
		{
			return OpenNETCF.Runtime.InteropServices.MarshalEx.ReAllocHGlobal(hMemory, new IntPtr(uBytes));
			/*
			IntPtr ptr = IntPtr.Zero;

			CheckHandle(hMemory);

			LocalReAllocCE(hMemory, uBytes, (int)MemoryAllocFlags.ZeroInit);

			if(ptr == IntPtr.Zero)
			{
				throw new WinAPIException("Allocation Failed");
			}

			return ptr;*/
		}

		/// <summary>
		/// Obsolete. Frees memory allocated with LocalAlloc
		/// </summary>
		/// <param name="hMem"></param>
		/// <remarks>Instead of using this function use <see cref="OpenNETCF.Runtime.InteropServices.MarshalEx.FreeHGlobal"/> which follows the model of the full .NET framework.</remarks>
		/// <seealso cref="OpenNETCF.Runtime.InteropServices.MarshalEx.FreeHGlobal"/>
		[Obsolete("Use OpenNETCF.Runtime.InteropServices.MarshalEx.FreeHGlobal instead", false)]
		public static void LocalFree(IntPtr hMem)
		{
			OpenNETCF.Runtime.InteropServices.MarshalEx.FreeHGlobal(hMem);

			/*IntPtr ptr = IntPtr.Zero;

			CheckHandle(hMem);

			ptr = LocalFreeCE(hMem);

			if(ptr != IntPtr.Zero)
			{
				throw new WinAPIException("Invalid Handle Value");
			}*/
		}

		/// <summary>
		/// Retrieves the memory status of the device
		/// </summary>
		public static MemoryStatus GlobalMemoryStatus()
		{
			MemoryStatus ms = new MemoryStatus();
			
			GlobalMemoryStatusCE( out ms );
			
			return ms;
		}

		#endregion ---------------------------------------------

		#region --------------- GDI API Calls ---------------

		/// <summary>
		/// Find a Window
		/// </summary>
		/// <param name="lpClassName">Can be empty</param>
		/// <param name="lpWindowName">Caption or text of Window to find</param>
		/// <returns>Handle to specified window.</returns>
		public static IntPtr FindWindow(string lpClassName, string lpWindowName)
		{
			IntPtr ptr = IntPtr.Zero;

			ptr = FindWindowCE(lpClassName, lpWindowName);

			if(ptr == IntPtr.Zero)
			{
				throw new WinAPIException("Failed to find Window");
			}

			return ptr;
		}

		/// <summary>
		/// Set the forecolor of text in the selected DC
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="crColor"></param>
		/// <returns></returns>
		public static int SetTextColor(IntPtr hdc, int crColor)
		{
			uint prevColor;
			
			CheckHandle(hdc);

			prevColor = SetTextColorCE(hdc, crColor);

			if(prevColor == CLR_INVALID)
			{
				throw new WinAPIException("Failed to set color");
			}

			return Convert.ToInt32(prevColor);
		}

		/// <summary>
		/// Get the forecolor of text in the selected DC
		/// </summary>
		/// <param name="hdc"></param>
		/// <returns></returns>
		public static int GetTextColor(IntPtr hdc)
		{
			CheckHandle(hdc);

			return GetTextColorCE(hdc);
		}

		/// <summary>
		/// Set the backcolor in the selected DC
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="crColor"></param>
		/// <returns></returns>
		public static int SetBkColor(IntPtr hdc, int crColor)
		{
			uint prevColor;
			
			CheckHandle(hdc);

			prevColor = SetBkColorCE(hdc, crColor);

			if(prevColor == CLR_INVALID)
			{
				throw new WinAPIException("Failed to set color");
			}

			return Convert.ToInt32(prevColor);
		}
		
		/// <summary>
		/// Set the backmode in the selected DC
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="iBkMode"></param>
		/// <returns></returns>
		public static BackMode SetBkMode(IntPtr hdc, BackMode iBkMode)
		{
			int prevMode;
			
			CheckHandle(hdc);

			prevMode = SetBkModeCE(hdc, (int)iBkMode);

			if(prevMode == 0)
			{
				throw new WinAPIException("Failed to set BkMode");
			}

			return (BackMode)prevMode;
		}

		/// <summary>
		/// Select a system object (FONT, DC, etc.)
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="hgdiobj"></param>
		/// <returns></returns>
		public static IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj)
		{
			IntPtr ptr = IntPtr.Zero;

			CheckHandle(hdc);
			CheckHandle(hgdiobj);

			ptr = SelectObjectCE(hdc, hgdiobj);

			if(ptr == IntPtr.Zero)
			{
				throw new WinAPIException("Failed. Selected object is not a region.");
			}

			return ptr;
		}

		/// <summary>
		/// Release a Device Context
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="hDC"></param>
		public static void ReleaseDC(IntPtr hWnd, IntPtr hDC)
		{
			CheckHandle(hDC);

			if(ReleaseDCCE(hWnd, hDC) == 0)
			{
				throw new WinAPIException("Failed to release DC.");
			}
		}

		/// <summary>
		/// Get the DC for the specified window
		/// </summary>
		/// <param name="hWnd">Native window handle of the window.</param>
		/// <returns>Device Context Handle for specified window.</returns>
		public static IntPtr GetWindowDC(IntPtr hWnd)
		{
			IntPtr ptr = IntPtr.Zero;

			ptr = GetWindowDCCE(hWnd);

			if(ptr == IntPtr.Zero)
			{
				throw new WinAPIException("Failed to get DC.");
			}

			return ptr;
		}

		/// <summary>
		/// Get the DC for the specified window handle
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		public static IntPtr GetDC(IntPtr hWnd)
		{
			IntPtr ptr = IntPtr.Zero;

			ptr = GetDCCE(hWnd);

			if(ptr == IntPtr.Zero)
			{
				throw new WinAPIException("Failed to get DC.");
			}

			return ptr;
		}

		/// <summary>
		/// Draw a rectangle in a DC
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="nLeftRect"></param>
		/// <param name="nTopRect"></param>
		/// <param name="nRightRect"></param>
		/// <param name="nBottomRect"></param>
		public static void Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
		{
			CheckHandle(hdc);

			if(Convert.ToBoolean(RectangleCE(hdc, nLeftRect, nTopRect, nRightRect, nBottomRect)) == false)
			{
				throw new WinAPIException("Failed to draw rectangle.");
			}
		}
		#endregion ---------------------------------------------

		#region --------------- Synchronization API Calls ---------------

		/// <summary>
		/// Create a system event (*not* a managed event)
		/// </summary>
		/// <param name="bManualReset"></param>
		/// <param name="bInitialState"></param>
		/// <param name="lpName"></param>
		/// <returns></returns>
		public static IntPtr CreateEvent(bool bManualReset, bool bInitialState, string lpName)
		{
			return CECreateEvent(IntPtr.Zero, Convert.ToInt32(bManualReset), Convert.ToInt32(bInitialState), lpName);
		}

		/// <summary>
		/// This function sets the state of the specified event object to signaled
		/// </summary>
		/// <param name="hEvent"></param>
		/// <returns></returns>
		public static bool SetEvent(IntPtr hEvent)
		{
			return Convert.ToBoolean(CEEventModify(hEvent, (uint)EventFlags.Set));
		}

		/// <summary>
		/// This function sets the state of the specified event object to nonsignaled
		/// </summary>
		/// <param name="hEvent"></param>
		/// <returns></returns>
		public static bool ResetEvent(IntPtr hEvent)
		{
			return Convert.ToBoolean(CEEventModify(hEvent, (uint)EventFlags.Reset));
		}

		/// <summary>
		/// This function provides a single operation that sets (to signaled) the state of the specified event object and then resets it (to nonsignaled) after releasing the appropriate number of waiting threads
		/// </summary>
		/// <param name="hEvent"></param>
		/// <returns></returns>
		public static bool PulseEvent(IntPtr hEvent)
		{
			return Convert.ToBoolean(CEEventModify(hEvent, (uint)EventFlags.Pulse));
		}

		/// <summary>
		/// This function returns when the specified object is in the signaled state or when the time-out interval elapses
		/// </summary>
		/// <param name="hHandle"></param>
		/// <param name="dwMilliseconds"></param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public static Wait WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds)
		{
			return (Wait)CEWaitForSingleObject(hHandle, dwMilliseconds);
		}

		/// <summary>
		/// This function returns when one of the following occurs:
		/// <list type=""><item>Either any one of the specified objects are in the signaled state.</item><item>The time-out interval elapses.</item></list>
		/// <i>New in SDF version 1.1</i>
		/// </summary>
		/// <param name="Handles">An array of handles to wait on</param>
		/// <param name="WaitForAll">Wait for all handles before returning</param>
		/// <param name="Timeout">Timeout period in milliseconds</param>
		/// <returns>WAIT_FAILED indicates failure.</returns>
		[CLSCompliant(false)]
		public static Wait WaitForMultipleObjects(IntPtr[] Handles, bool WaitForAll, int Timeout)
		{
			return (Wait)CEWaitForMultipleObjects((uint)Handles.Length, Handles, WaitForAll, (uint)Timeout);
		}

		/// <summary>
		/// This function creates a named or unnamed mutex object
		/// </summary>
		/// <param name="lpMutexAttributes">Ignored. Must be null</param>
		/// <param name="bInitialOwner">Boolean that specifies the initial owner 
		/// of the mutex object. If this value is true and the caller created 
		/// the mutex, the calling thread obtains ownership of the mutex object. 
		/// Otherwise, the calling thread does not obtain ownership of the mutex.</param>
		/// <param name="lpName">String specifying the name of the mutex object.</param>
		/// <returns>A handle to the mutex object indicates success. If the 
		/// named mutex object existed before the function call, the function 
		/// returns a handle to the existing object. A return value of null 
		/// indicates failure. </returns>
		public static IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName)
		{
			IntPtr h;
			const int ERROR_ALREADY_EXISTS = 183;

			h = CreateMutexCE(lpMutexAttributes, bInitialOwner, lpName);

			if(h != IntPtr.Zero)
			{
				if(Marshal.GetLastWin32Error() == ERROR_ALREADY_EXISTS)
				{
					throw new WinAPIException("Mutex already exists", (int)h);
				}
			}
			else
			{
				throw new WinAPIException("CreateMutex failed.",(int)h);
			}
			return h;
		}

		/// <summary>
		/// This function releases ownership of the specified mutex object
		/// </summary>
		/// <param name="hMutex"></param>
		/// <returns></returns>
		public static bool ReleaseMutex(IntPtr hMutex)
		{
			bool b;

			b = ReleaseMutexCE(hMutex);

			if(!b)
			{
				throw new WinAPIException("Error releasing Mutex.", Marshal.GetLastWin32Error());
			}
			return b;
		}
		#endregion ---------------------------------------------

		#region --------------- Performance Monitoring functions ---------------

		/// <summary>
		/// This function retrieves the frequency of the high-resolution performance counter if one is provided by the OEM.
		/// </summary>
		/// <returns>The current performance-counter frequency. If the installed hardware does not support a high-resolution performance counter, this parameter can be to zero.</returns>
		public static long QueryPerformanceFrequency()
		{
			long lpFrequency = 0;

			int i;

			i = QueryPerformanceFrequencyCE(ref lpFrequency);

			if(i == 0)
			{
				throw new WinAPIException("The hardware does not support a high frequency counter.");
			}

			return lpFrequency;
		}

		/// <summary>
		/// This function retrieves the current value of the high-resolution performance counter if one is provided by the OEM
		/// </summary>
		/// <returns>The current performance-counter value. If the installed hardware does not support a high-resolution performance counter, this parameter can be set to zero.</returns>
		public static long QueryPerformanceCounter()
		{
			long lpPerformanceCount = 0;

			int i;

			i = QueryPerformanceCounterCE(ref lpPerformanceCount);

			if(i == 0)
			{
				throw new WinAPIException("The hardware does not support a high frequency counter.");
			}

			return lpPerformanceCount;
		}

		#endregion ---------------------------------------------

		#region --------------- System Info functions ---------------

		#region Get System Info
		/// <summary>
		/// This function returns information about the current system
		/// </summary>
		public static SystemInfo GetSystemInfo()
		{
			SystemInfo pSI = new SystemInfo();

			try 
			{
				GetSystemInfoCE(out pSI);
			}
			catch(Exception)
			{
				throw new WinAPIException("Error retrieving system info.");
			}

			return pSI;
		}
		#endregion

		#region GetSystemPowerStatus
		/// <summary>
		/// This function retrieves the power status of the system.
		/// </summary>
		/// <returns>A SystemPowerStatus structure containing power state information.</returns>
		/// <remarks>The status indicates whether the system is running on AC or DC power, whether or not the batteries are currently charging, and the remaining life of main and backup batteries.</remarks>
		/// <param name="update">If True retrieves latest state, otherwise retrieves cached information which may be out of date.</param>
		public static SystemPowerStatus GetSystemPowerStatusEx(bool update)
		{
			SystemPowerStatus pStatus = new SystemPowerStatus();

			try
			{
				GetSystemPowerStatusExCE(out pStatus, update);
			}
			catch(Exception)
			{
				throw new WinAPIException("Error retrieving system power status.");
			}

			return pStatus;
		}
		#endregion

		#region OsVersion (System.Environment)
		// <summary>
		// This function obtains extended information about the version of the operating system that is currently running.
		// </summary>
		// <param name="osvi"></param>
		/*public static void GetVersionEx(out OSVERSIONINFO osvi)
		{
			bool b;

			osvi.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFO));

			b = GetVersionExCE(out osvi);

			if(!b)
			{
				throw new WinAPIException("Error retrieving version information.", Marshal.GetLastWin32Error());
			}
			
		}*/
		#endregion

		#region Get Device ID
		/// <summary>
		/// Returns a string containing a unique identifier for the device.
		/// </summary>
		/// <returns>Devices unique ID.</returns>
		public static string GetDeviceID()
		{
			int			len		= 256;	
			int			cb		= 0;
			byte[]			buffer	= new byte[256];	
			//uint			ret;	
			StringBuilder	sb		= new StringBuilder();	

			buffer[0] = 0;	
			buffer[1] = 1;	

			KernelIoControl(IOCTL_HAL_GET_DEVICEID, IntPtr.Zero, 0, buffer, len, ref cb);
			
			Int32 dwPresetIDOffsset = BitConverter.ToInt32(buffer, 4);	
			Int32 dwPlatformIDOffset = BitConverter.ToInt32(buffer, 0xc);	
			Int32 dwPlatformIDSize = BitConverter.ToInt32(buffer, 0x10);	

			sb.Append(String.Format("{0:X8}-{1:X4}-{2:X4}-{3:X4}-",	
				BitConverter.ToInt32(buffer, dwPresetIDOffsset),
				BitConverter.ToInt16(buffer, dwPresetIDOffsset+4),
				BitConverter.ToInt16(buffer, dwPresetIDOffsset+6),
				BitConverter.ToInt16(buffer, dwPresetIDOffsset+8)));	
			
			for(int i = dwPlatformIDOffset ; i < dwPlatformIDOffset + dwPlatformIDSize ; i ++)
			{
				sb.Append( String.Format("{0:X2}", buffer[i] ));
			}

			return sb.ToString();
		}
		#endregion

		#endregion ---------------------------------------------

		#region --------------- Obsolete Process functions ---------------
		// Now housed in OpenNETCF.Diagnostics.Process class

		/// <summary>
		/// This function performs an action on a file. The file can be an executable file or a document
		/// </summary>
		/// <param name="FileName">Filename of executable or document to run.</param>
		/// <param name="Parameters">Optional command line parameters.</param>
		/// <returns>Handle to created process</returns>
		[ObsoleteAttribute("Use the OpenNETCF.Diagnostics.Process class to start and stop processes", true)]
		public static IntPtr ShellExecute(string FileName, string Parameters)
		{
			OpenNETCF.Diagnostics.ProcessStartInfo psi = new OpenNETCF.Diagnostics.ProcessStartInfo();
			psi.UseShellExecute = true;
			psi.FileName = FileName;
			psi.Arguments = Parameters;
			return OpenNETCF.Diagnostics.Process.Start(psi).Handle;

			/*int nSize;
			SHELLEXECUTEEX see;
			IntPtr pFile;
			IntPtr pParams;

			see = new SHELLEXECUTEEX();

			nSize = FileName.Length * 2 + 2;
			pFile = LocalAlloc(MemoryAllocFlags.LPtr, nSize);
			Marshal.Copy(Encoding.Unicode.GetBytes(FileName), 0, pFile, nSize - 2);

			nSize = Parameters.Length * 2 + 2;
			pParams = LocalAlloc(MemoryAllocFlags.LPtr, nSize);
			Marshal.Copy(Encoding.Unicode.GetBytes(Parameters), 0, pParams, nSize - 2);
			
			see.lpFile = pFile;
			see.lpParameters = pParams;
			
			if(ShellExecuteExCE(see) == 0)
			{
				throw new WinAPIException("Cannot Execute File");
			}

			LocalFreeCE(pFile);
			LocalFreeCE(pParams);
		
			return see.hProcess;*/
		}

		/// <summary>
		/// This function is used to run a new program. It creates a new process and its primary thread. The new process executes the specified executable file
		/// </summary>
		/// <param name="FileName">Filename of executable to run.</param>
		/// <param name="CommandLine">Optional command line attributes.</param>
		/// <returns>Handle to created process</returns>
		[ObsoleteAttribute("Use the OpenNETCF.Diagnostics.Process class to start and stop processes", true)]
		public static IntPtr CreateProcess(string FileName, string CommandLine)
		{
			OpenNETCF.Diagnostics.ProcessStartInfo psi = new OpenNETCF.Diagnostics.ProcessStartInfo();
			psi.UseShellExecute = false;
			psi.FileName = FileName;
			psi.Arguments = CommandLine;
			return OpenNETCF.Diagnostics.Process.Start(psi).Handle;
			
			/*PROCESS_INFORMATION pi = new PROCESS_INFORMATION();

			if(CreateProcessCE(FileName, CommandLine, IntPtr.Zero, IntPtr.Zero, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, pi) == 0)
			{
				throw new WinAPIException("Cannot Create Process");
			}
			
			return pi.hProcess;*/
		}

		/// <summary>
		/// This function terminates the specified process and all of its threads
		/// </summary>
		/// <param name="hProcess">Handle of Process to be terminated.</param>
		/// <param name="ExitCode">Exit code to pass to process.</param>
		[ObsoleteAttribute("Use the OpenNETCF.Diagnostics.Process class to start and stop processes", true)]
		public static void TerminateProcess(IntPtr hProcess, int ExitCode)
		{
			if(TerminateProcessCE(hProcess, ExitCode) == 0)
			{
				throw new WinAPIException("Cannot Terminate Process");
			}
		}
		#endregion

		#region --------------- Disabled Tray Icon functions ---------------
		//Now housed in OpenNETCF.Windows.Forms.NotifyIcon

		/*private IntPtr hAppIcon = IntPtr.Zero;
		private TrayIconMessageWindow iconWnd;
		public event BeforeIconMessageEventHandler BeforeIconMessage;
		public event AfterIconMessageEventHandler AfterIconMessage;
		private static bool bRegistered = false;

		/// <summary>
		/// Remove app icon from the system tray
		/// </summary>
		public void UnregisterTrayIcon()
		{
			if(! bRegistered)
			{
				throw new WinAPIException("Application has no registered icon");
			}

			NOTIFYICONDATA nid = new NOTIFYICONDATA();
			int cbSize = 24 + 64 * Marshal.SystemDefaultCharSize;
			IntPtr pData = LocalAlloc(MemoryAllocFlags.LPTR, (uint)cbSize);
			nid.cbSize = cbSize;
			nid.hWnd = iconWnd.Hwnd;
			nid.uFlags = 0;
			Marshal.StructureToPtr(nid, pData, false);
			if(! Shell_NotifyIconCE(NIM_DELETE, pData))
			{
				throw new WinAPIException("Failed to delete registered icon");
			}
			
			// Also we need to destroy the icon to recover resources
			DestroyIconCE(hAppIcon);
			hAppIcon = IntPtr.Zero;
			bRegistered = false;
		}

		/// <summary>
		/// Set app icon in the system tray
		/// </summary>
		/// <param name="ToolTip"></param>
		public void RegisterTrayIcon(string ToolTip)
		{
			IntPtr pData;

			// only allow one registration
			if(bRegistered)
			{
				throw new WinAPIException("Application already has registered icon");
			}

			// Extract icon from the application's exe
			// This requires that app icon was set in the project properties
			string szPath = Assembly.GetCallingAssembly().GetModules()[0].FullyQualifiedName;
			if(ExtractIconExCE(szPath, 0, 0, ref hAppIcon, 1) == IntPtr.Zero)
			{
				throw new WinAPIException("Cannot extract app icon");
			}

			try
			{
				// Fill NOTIFYICONDATA with data
				NOTIFYICONDATA nid = new NOTIFYICONDATA();

				// In our struct definition we have omitted emebedded wchar array size 64 - we need to account for it
				int cbSize = Marshal.SizeOf(nid) + 64 * Marshal.SystemDefaultCharSize;

				// Allocate memory
				pData = LocalAlloc(MemoryAllocFlags.LPTR, (uint)cbSize);
			
				// Set fields of the NOTIFYICONDATA (see SDK documentation)
				iconWnd = new TrayIconMessageWindow();
				iconWnd.AfterIconMessageEvent += new AfterIconMessageEventHandler(iconWnd_AfterIconMessageEvent);
				iconWnd.BeforeIconMessageEvent += new BeforeIconMessageEventHandler(iconWnd_BeforeIconMessageEvent);
				nid.cbSize = cbSize;
				nid.hIcon = hAppIcon;
				nid.hWnd = iconWnd.Hwnd;
				nid.uCallbackMessage = WM_USER + 100;
				nid.uID = 1;
				nid.uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP;
				Marshal.StructureToPtr(nid, pData, false);

				// Add tooltip
				char[] arrTooltip = ToolTip.ToCharArray();
				Marshal.Copy(arrTooltip, 0, (IntPtr)(pData.ToInt32() + Marshal.SizeOf(nid)), arrTooltip.Length);
			
				// Register with shell
				if(! Shell_NotifyIconCE(NIM_ADD, pData))
				{
					bRegistered = false;
					LocalFree(pData);
					throw new WinAPIException("Cannot register icon");
				}

				//Free memory
				LocalFree(pData);
			}
			catch(WinAPIException ex)
			{
				bRegistered = false;
				throw ex;
			}
			catch(Exception ex)
			{
				bRegistered = false;
				throw new WinAPIException(ex);
			}

			bRegistered = true;
		}

		private void iconWnd_AfterIconMessageEvent(ref Message msg)
		{
			if(AfterIconMessage != null)
			{
				AfterIconMessage(ref msg);
			}
		}

		private bool iconWnd_BeforeIconMessageEvent(ref Message msg)
		{
			if(BeforeIconMessage != null)
			{
				return BeforeIconMessage(ref msg);
			}

			return false;
		}*/
		#endregion

		#region --------------- System Functions ----------------			
		/// <summary>
		/// Wrapper around GetProcAddress Win32 function
		/// </summary>
		/// <param name="hModule">Module handle (returned by GetModuleHandle or LoadLibrary</param>
		/// <param name="proc">Function name</param>
		/// <returns>The address of the DLL's exported function indicates success.
		/// Null indicates failure.</returns>
		public static IntPtr GetProcAddress(IntPtr hModule, string proc)
		{
			return GetProcAddressCE(hModule, proc);
		}

		/// <summary>
		/// Wrapper around GetModuleHandle Win32 function
		/// </summary>
		/// <param name="module">Module name</param>
		/// <returns>Module handle or IntPtr.Zero, if not in memory </returns>
		public static IntPtr GetModuleHandle(string module)
		{
			return GetModuleHandleCE(module);
		}

		/// <summary>
		/// Wrapper around LoadLibrary Win32 function
		/// </summary>
		/// <param name="library">Library module name</param>
		/// <returns>hModule or error code</returns>
		public static IntPtr LoadLibrary(string library)
		{
			IntPtr ret = LoadLibraryCE(library);
			if ( ret.ToInt32() <= 31 & ret.ToInt32() >= 0)
				throw new WinAPIException("Failed to load library " + library, Marshal.GetLastWin32Error());
			return ret;
		}
		
		/// <summary>
		/// This function determines whether the calling process has read access to the memory at the specified address.
		/// </summary>
		/// <param name="pfnCode">Pointer to an address in memory.</param>
		/// <returns>Zero indicates that the calling process has read access to the specified memory.
		/// Nonzero indicates that the calling process does not have read access to the specified memory.</returns>
		public static bool IsBadCodePtr(IntPtr pfnCode)
		{
			return IsBadCodePtrCE(pfnCode);
		}

		#endregion

		#region --------------- Internal helper functions ----------------
		
		internal static void CheckHandle(IntPtr hPtr)
		{
			if((hPtr == IntPtr.Zero) || ((int)hPtr == INVALID_HANDLE_VALUE))
			{
				throw new WinAPIException("Invalid Handle Value", 0);
			}
		}

		/// <summary>
		/// Obsolete - Allocates native memory and marshals the string.
		/// </summary>
		/// <param name="val">String value.</param>
		/// <returns>Pointer to native memory - must be freed with <see cref="LocalFree"/>.</returns>
		[Obsolete("Use OpenNETCF.Runtime.MarshalEx.StringToHGlobalUni instead", true)]
		public static IntPtr StringToPointer(string val)
		{   
			if(val == null)
				return IntPtr.Zero;

			IntPtr retVal = Core.LocalAlloc(Core.MemoryAllocFlags.LPtr, (val.Length + 1) * 2);
            if(retVal == IntPtr.Zero)
				throw new OutOfMemoryException();

			Marshal.Copy(val.ToCharArray(), 0, retVal, val.Length);
			return retVal;
		}

		#endregion ---------------------------------------------

		#region ----------------- Keyboard functions ------------------
		/// <summary>
		/// Send a string to the keyboard
		/// </summary>
		/// <param name="Keys"></param>
		public static void SendKeyboardString(string Keys)
		{
			SendKeyboardString(Keys, KeyStateFlags.Down, IntPtr.Zero);
		}

		/// <summary>
		/// Send a string to the keyboard
		/// </summary>
		/// <param name="Keys"></param>
		/// <param name="Flags"></param>
		public static void SendKeyboardString(string Keys, KeyStateFlags Flags)
		{
			SendKeyboardString(Keys, Flags, IntPtr.Zero);
		}

		/// <summary>
		/// Send a string to the keyboard
		/// </summary>
		/// <param name="Keys"></param>
		/// <param name="Flags"></param>
		/// <param name="hWnd"></param>
		public static void SendKeyboardString(string Keys, KeyStateFlags Flags, IntPtr hWnd)
		{
			uint[]			keys	= new uint[Keys.Length];
			KeyStateFlags[]	states	= new KeyStateFlags[Keys.Length];
			KeyStateFlags[]	dead	= {KeyStateFlags.Dead};

			for(int k = 0 ; k < Keys.Length ; k++)
			{
				states[k] = Flags;
				keys[k] = Convert.ToUInt32(Keys[k]);
			}

			PostKeybdMessage(hWnd, 0, Flags, (uint)keys.Length, states, keys);
			PostKeybdMessage(hWnd, 0, dead[0], 1, dead, keys); 
		}

		/// <summary>
		/// Send a key to the keyboard
		/// </summary>
		/// <param name="VirtualKey"></param>
		public static void SendKeyboardKey(byte VirtualKey)
		{
			SendKeyboardKey(VirtualKey, true);
		}

		/// <summary>
		/// Send a key to the keyboard
		/// </summary>
		/// <param name="VirtualKey"></param>
		/// <param name="Silent"></param>
		public static void SendKeyboardKey(byte VirtualKey, bool Silent)
		{
			int silent = Silent ? (int)KeyEvents.Silent : 0;

			keybd_event(VirtualKey, 0, 0, silent);
			keybd_event(VirtualKey, 0, (int)KeyEvents.KeyUp, silent);
		}
		#endregion

		#region --------------- P/Invoke declarations ---------------

		

		#region Keyboard P/Invokes
		[DllImport("coredll.dll", EntryPoint="keybd_event", SetLastError=true)]
		private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

		[DllImport("coredll.dll", EntryPoint="PostKeybdMessage", SetLastError=true)]
		private static extern bool PostKeybdMessage(IntPtr hwnd, uint vKey, KeyStateFlags flags, uint cCharacters, KeyStateFlags[] pShiftStateBuffer, uint[] pCharacterBuffer);
		#endregion

		#region multimedia P/Invokes
		
		[DllImport ("coredll.dll", EntryPoint="waveOutGetNumDevs", SetLastError=true)]
		public static extern int waveOutGetNumDevs();

		[DllImport ("coredll.dll", EntryPoint="waveInGetNumDevs", SetLastError=true)]
		public static extern int waveInGetNumDevs();

		[DllImport ("coredll.dll", EntryPoint="waveOutOpen", SetLastError=true)]
		public static extern int waveOutOpen(out IntPtr t, int id, WaveFormatEx pwfx, IntPtr dwCallback, int dwInstance, int fdwOpen);

		[DllImport ("coredll.dll", EntryPoint="waveOutGetVolume", SetLastError=true)]
		public static extern int waveOutGetVolume(IntPtr hwo, ref int pdwVolume);

		[DllImport ("coredll.dll", EntryPoint="waveOutSetVolume", SetLastError=true)]
		public static extern int waveOutSetVolume(IntPtr hwo, int dwVolume);

		[DllImport ("coredll.dll", EntryPoint="waveOutPrepareHeader", SetLastError=true)]
		public static extern int waveOutPrepareHeader(IntPtr hwo, byte[] pwh, int cbwh);
		[DllImport ("coredll.dll", EntryPoint="waveOutPrepareHeader", SetLastError=true)]
		public static extern int waveOutPrepareHeader(IntPtr hwo, IntPtr lpHdr, int cbwh);

		[DllImport ("coredll.dll", EntryPoint="waveOutWrite", SetLastError=true)]
		public static extern int waveOutWrite(IntPtr hwo, byte[] pwh, int cbwh);
		[DllImport ("coredll.dll", EntryPoint="waveOutWrite", SetLastError=true)]
		public static extern int waveOutWrite(IntPtr hwo, IntPtr lpHdr, int cbwh);

		[DllImport ("coredll.dll", EntryPoint="waveOutUnprepareHeader", SetLastError=true)]
		public static extern int waveOutUnprepareHeader(IntPtr hwo, byte[] pwh, int cbwh);
		[DllImport ("coredll.dll", EntryPoint="waveOutUnprepareHeader", SetLastError=true)]
		public static extern int waveOutUnprepareHeader(IntPtr hwo, IntPtr lpHdr, int cbwh);

		[DllImport ("coredll.dll", EntryPoint="waveOutClose", SetLastError=true)]
		public static extern int waveOutClose(IntPtr hwo);

		[DllImport ("coredll.dll", EntryPoint="waveOutPause", SetLastError=true)]
		public static extern int waveOutPause(IntPtr hwo);

		[DllImport ("coredll.dll", EntryPoint="waveOutRestart", SetLastError=true)]
		public static extern int waveOutRestart(IntPtr hwo);

		[DllImport ("coredll.dll", EntryPoint="waveOutReset", SetLastError=true)]
		public static extern int waveOutReset(IntPtr hwo);

		[DllImport ("coredll.dll", EntryPoint="waveInOpen", SetLastError=true)]
		internal static extern int waveInOpen(out IntPtr t, uint id, WaveFormatEx pwfx, IntPtr dwCallback, int dwInstance, int fdwOpen);

		[DllImport ("coredll.dll", EntryPoint="waveInClose", SetLastError=true)]
		public static extern int waveInClose(IntPtr hDev);

		[DllImport ("coredll.dll", EntryPoint="waveInPrepareHeader", SetLastError=true)]
		public static extern int waveInPrepareHeader(IntPtr hwi, byte[] pwh, int cbwh);
		[DllImport ("coredll.dll", EntryPoint="waveInPrepareHeader", SetLastError=true)]
		public static extern int waveInPrepareHeader(IntPtr hwi, IntPtr lpHdr, int cbwh);

		[DllImport ("coredll.dll", EntryPoint="waveInStart", SetLastError=true)]
		public static extern int waveInStart(IntPtr hwi);

		[DllImport ("coredll.dll", EntryPoint="waveInUnprepareHeader", SetLastError=true)]
		public static extern int waveInUnprepareHeader(IntPtr hwi, byte[] pwh, int cbwh);
		[DllImport ("coredll.dll", EntryPoint="waveInUnprepareHeader", SetLastError=true)]
		public static extern int waveInUnprepareHeader(IntPtr hwi, IntPtr lpHdr, int cbwh);

		[DllImport ("coredll.dll", EntryPoint="waveInStop", SetLastError=true)]
		public static extern int waveInStop(IntPtr hwi);

		[DllImport ("coredll.dll", EntryPoint="waveInReset", SetLastError=true)]
		public static extern int waveInReset(IntPtr hwi);

		[DllImport ("coredll.dll", EntryPoint="waveInGetDevCaps", SetLastError=true)]
		public static extern int waveInGetDevCaps(int uDeviceID, byte[] pwic, int cbwic);

		[DllImport ("coredll.dll", EntryPoint="waveInAddBuffer", SetLastError=true)]
		public static extern int waveInAddBuffer(IntPtr hwi, byte[] pwh, int cbwh);
		[DllImport ("coredll.dll", EntryPoint="waveInAddBuffer", SetLastError=true)]
		public static extern int waveInAddBuffer(IntPtr hwi, IntPtr lpHdr, int cbwh);

		#endregion
		
		#region memory P/Invokes
		//[DllImport("coredll", EntryPoint="LocalAlloc", SetLastError=true)]
		//internal static extern IntPtr LocalAllocCE(int uFlags, int uBytes);

		//[DllImport("coredll", EntryPoint="LocalReAlloc", SetLastError=true)]
		//internal static extern IntPtr LocalReAllocCE(IntPtr hMem, int uBytes, int fuFlags);

		[DllImport("coredll", EntryPoint="LocalFree", SetLastError=true)]
		internal static extern IntPtr LocalFreeCE(IntPtr hMem);

		[DllImport("coredll", EntryPoint="GlobalMemoryStatus", SetLastError=false)]
		internal static extern void GlobalMemoryStatusCE(out MemoryStatus msce);
		#endregion

		#region GDI P/Invokes

		[DllImport("coredll.dll", EntryPoint="FindWindowW", SetLastError=true)]
		private static extern IntPtr FindWindowCE(string lpClassName, string lpWindowName);
		
		[DllImport ("coredll.dll", EntryPoint="SetTextColor", SetLastError=true)]
		private static extern uint SetTextColorCE(IntPtr hdc, int crColor);

		[DllImport ("coredll.dll", EntryPoint="GetTextColor", SetLastError=true)]
		private static extern int GetTextColorCE(IntPtr hdc);

		[DllImport ("coredll.dll", EntryPoint="SetBkColor", SetLastError=true)]
		private static extern uint SetBkColorCE(IntPtr hdc, int crColor);

		[DllImport ("coredll.dll", EntryPoint="SetBkMode", SetLastError=true)]
		private static extern int SetBkModeCE(IntPtr hdc, int iBkMode);

		[DllImport ("coredll.dll", EntryPoint="SelectObject", SetLastError=true)]
		private static extern IntPtr SelectObjectCE(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("coredll.dll", EntryPoint="ReleaseDC", SetLastError=true)]
		private static extern int ReleaseDCCE(IntPtr hWnd, IntPtr hDC);

		[DllImport("coredll.dll", EntryPoint="GetWindowDC", SetLastError=true)]
		private static extern IntPtr GetWindowDCCE(IntPtr hWnd);

		[DllImport("coredll", EntryPoint="GetDC", SetLastError=true)]
		private static extern IntPtr GetDCCE(IntPtr hWnd);

		[DllImport("coredll", EntryPoint="Rectangle", SetLastError=true)]
		private static extern uint RectangleCE(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

		/// <summary>
		/// This function obtains information about a specified graphics object.
		/// </summary>
		/// <param name="hObj">Handle to the graphics object of interest.</param>
		/// <param name="cb">Specifies the number of bytes of information to be written to the buffer.</param>
		/// <param name="objdata">a buffer that is to receive the information about the specified graphics object.</param>
		/// <returns>If the function succeeds, and lpvObject is a valid pointer, the return value is the number of bytes stored into the buffer.</returns>
		[DllImport("coredll", EntryPoint="GetObject", SetLastError=true)]
		extern static public int GetObject(IntPtr hObj, int cb, byte[] objdata);
		/// <summary>
		/// This function obtains information about a specified graphics object.
		/// </summary>
		/// <param name="hObj">Handle to the graphics object of interest.</param>
		/// <param name="cb">Specifies the number of bytes of information to be written to the buffer.</param>
		/// <param name="objdata">a buffer that is to receive the information about the specified graphics object.</param>
		/// <returns>If the function succeeds, and lpvObject is a valid pointer, the return value is the number of bytes stored into the buffer.</returns>
		[DllImport("coredll", EntryPoint="GetObject", SetLastError=true)]
		extern static public int GetObject(IntPtr hObj, int cb, DibSection objdata);

		/// <summary>
		/// This function creates a logical brush that has the specified solid color.
		/// </summary>
		/// <param name="crColor">Specifies the color of the brush.</param>
		/// <returns>A handle that identifies a logical brush indicates success.</returns>
		[DllImport("coredll", EntryPoint="CreateSolidBrush", SetLastError=true)]
		public static extern IntPtr CreateSolidBrush( int crColor );

		#endregion

		#region Synchronization P/Invokes
		[DllImport("coredll.dll", EntryPoint="WaitForSingleObject", SetLastError = true)]
		private static extern uint CEWaitForSingleObject(IntPtr hHandle, uint dwMilliseconds); 

		[DllImport("coredll.dll", EntryPoint="WaitForMultipleObjects", SetLastError = true)]
		private static extern int CEWaitForMultipleObjects(uint nCount, IntPtr[] lpHandles, bool fWaitAll, uint dwMilliseconds); 

		[DllImport("coredll.dll", EntryPoint="EventModify", SetLastError = true)]
		private static extern int CEEventModify(IntPtr hEvent, uint function); 

		[DllImport("coredll.dll", EntryPoint="CreateEvent", SetLastError = true)]
		private static extern IntPtr CECreateEvent(IntPtr lpEventAttributes, int bManualReset, int bInitialState, string lpName); 

		[DllImport("coredll.dll", EntryPoint="CreateMutex", SetLastError=true)]		
		internal static extern IntPtr CreateMutexCE(
			IntPtr lpMutexAttributes, 
			bool InitialOwner, 
			string MutexName);		
		
		[DllImport("coredll",EntryPoint="ReleaseMutex",SetLastError=true)]
		internal static extern bool ReleaseMutexCE(IntPtr hMutex);

		#endregion

		#region Performance Monitoring P/Invokes

		[DllImport("coredll", EntryPoint="QueryPerformanceFrequency", SetLastError=true)]
		private static extern int QueryPerformanceFrequencyCE(ref Int64 lpFrequency);

		[DllImport("coredll", EntryPoint="QueryPerformanceCounter", SetLastError=true)]
		private static extern int QueryPerformanceCounterCE(ref Int64 lpPerformanceCount);

		#endregion

		#region System Info P/Invokes

		[DllImport("coredll", EntryPoint="GetSystemInfo",SetLastError=true)]
		internal static extern void GetSystemInfoCE(out SystemInfo pSI); 

		//[DllImport("coredll", EntryPoint="GetVersionEx", SetLastError=true)]
		//internal static extern bool GetVersionExCE(out OSVERSIONINFO lpVersionInformation);

		[DllImport("coredll", EntryPoint="FormatMessageW", SetLastError=false)]
		internal static extern int FormatMessage(FormatMessageFlags dwFlags, int lpSource, int dwMessageId, int dwLanguageId, out IntPtr lpBuffer, int nSize, int[] Arguments );

		
		[DllImport("coredll.dll", SetLastError=true)]
		private static extern bool KernelIoControl(Int32 dwIoControlCode, IntPtr
			lpInBuf, Int32 nInBufSize, byte[] lpOutBuf, Int32 nOutBufSize, ref Int32
			lpBytesReturned);

		[DllImport("coredll", EntryPoint="GetSystemPowerStatusEx", SetLastError=true)]
		internal static extern bool GetSystemPowerStatusExCE(out SystemPowerStatus pStatus, bool fUpdate);

		[DllImport("coredll", EntryPoint="SystemParametersInfo")]
		internal static extern bool SystemParametersInfo(SystemParametersInfoAction action, int size, byte[] buffer, SystemParametersInfoFlags winini);

		#endregion

		

		#region User P/Invokes
		
		[DllImport("coredll", SetLastError=true)]
		internal static extern uint RegisterWindowMessage( string lpString );

		#endregion

		#region Disabled Registry P/Invokes
		//see OpenNETCF.Win32.Registry
		/*
		[DllImport("coredll.dll", EntryPoint="RegOpenKeyEx", SetLastError=true)] 
		internal static extern int RegOpenKeyExCE(uint hKey,
			string lpSubKey, int ulOptions, int samDesired, ref uint phkResult); 

		[DllImport("coredll", EntryPoint="RegCreateKeyEx", SetLastError=true)] 
		internal static extern int RegCreateKeyExCE(uint
			hKey, string lpSubKey, int lpReserved, string lpClass, int dwOptions,
			int samDesired, ref int lpSecurityAttributes, ref uint phkResult, 
			out KeyDisposition lpdwDisposition); 

		[DllImport("coredll.dll", EntryPoint="RegEnumKeyEx", SetLastError=true)]
		internal static extern int RegEnumKeyExCE(uint hKey,
			int iIndex, 
			byte[] sKeyName, ref int iKeyNameLen, 
			int iReservedZero,
			byte[] sClassName, ref int iClassNameLen, 
			int iFiletimeZero);

		[DllImport("coredll.dll", EntryPoint="RegEnumValue", SetLastError=true)]
		internal static extern int RegEnumValueCE(uint hKey,
			int iIndex, byte[] sValueName, 
			ref int iValueNameLen, int
			iReservedZero, ref KeyType iType, 
			byte[] byData, ref int iDataLen);

		[DllImport("coredll.dll", EntryPoint="RegQueryInfoKey", SetLastError=true)]
		internal static extern int RegQueryInfoKeyCE(uint
			hKey, char[] lpClass, ref int lpcbClass, 
			int reservedZero, out int cSubkey, 
			out int iMaxSubkeyLen, out int lpcbMaxSubkeyClassLen,
			out int cValueNames, out int iMaxValueNameLen, 
			out int iMaxValueLen, int securityDescriptorZero, int
			lastWriteTimeZero);

		[DllImport("coredll.dll", EntryPoint="RegQueryValueEx", SetLastError=true)] 
		internal static extern int RegQueryValueExCE(uint
			hKey, string lpValueName, int lpReserved, 
			ref KeyType lpType, byte[] lpData,
			ref int lpcbData); 

		[DllImport("coredll.dll", EntryPoint="RegSetValueExW", SetLastError=true)] 
		internal static extern int RegSetValueExCE(uint
			hKey, string lpValueName, int lpReserved, 
			KeyType lpType, byte[] lpData, int lpcbData); 

		[DllImport("coredll.dll", EntryPoint="RegCloseKey", SetLastError=true)] 
		internal static extern int RegCloseKeyCE(uint hKey);

		[DllImport("coredll.dll", EntryPoint="RegDeleteKey", SetLastError=true)]
		internal static extern int RegDeleteKeyCE(uint hKey,
			string keyName);

		[DllImport("coredll.dll", EntryPoint="RegDeleteValue", SetLastError=true)]
		internal static extern int RegDeleteValueCE(uint
			hKey, string valueName);
		*/
		#endregion

		#region Obsolete Process P/Invokes
		//See OpenNETCF.Diagnostics.Process

		//[DllImport("coredll", EntryPoint="ShellExecuteEx", SetLastError=true)]
		//internal extern static int ShellExecuteExCE( SHELLEXECUTEEX ex );

		//[DllImport("coredll", EntryPoint="CreateProcess", SetLastError=true)]
		//internal extern static int CreateProcessCE(string pszImageName, string pszCmdLine, IntPtr psaProcess, IntPtr psaThread, int fInheritHandles, int fdwCreate, IntPtr pvEnvironment, IntPtr pszCurDir, IntPtr psiStartInfo, PROCESS_INFORMATION pi);

		[DllImport("coredll", EntryPoint="TerminateProcess", SetLastError=true)]
		internal extern static int TerminateProcessCE(IntPtr hProcess, int uExitCode);
		
		#endregion

		#region Shell P/Invokes

		[DllImport("coredll", EntryPoint="DestroyIcon", SetLastError=true)]
		internal extern static IntPtr DestroyIconCE(IntPtr hIcon);
	
		[DllImport("coredll", EntryPoint="ExtractIconEx", SetLastError=true)]
		internal extern static IntPtr ExtractIconExCE(
			string lpszFile, 
			int nIconIndex, 
			int hiconLarge, 
			ref IntPtr hIcon, 
			uint nIcons); 

		[DllImport("coredll", EntryPoint="Shell_NotifyIcon", SetLastError=true)]
		internal extern static bool Shell_NotifyIconCE(uint dwMessage, IntPtr lpData);
		
		#endregion

		#region System P/Invokes
		[DllImport("coredll", EntryPoint="GetProcAddressW", SetLastError=true)]
		internal static extern IntPtr GetProcAddressCE(IntPtr hModule, string lpszProc);
		[DllImport("coredll", EntryPoint="GetModuleHandleW", SetLastError=true)]
		internal static extern IntPtr GetModuleHandleCE(string lpszModule);
		[DllImport("coredll", EntryPoint="IsBadCodePtr", SetLastError=true)]
		internal static extern bool IsBadCodePtrCE( IntPtr lpfn );
		[DllImport("coredll", EntryPoint="LoadLibraryW", SetLastError=true)]
		internal static extern IntPtr LoadLibraryCE( string lpszLib );
		
		#endregion

		#endregion ---------------------------------------------
	}
	#region ---------- Disabled Internal Helper classes ----------
	internal sealed class PROCESS_INFORMATION
	{
		public IntPtr hProcess	= IntPtr.Zero;
		public IntPtr hThread	= IntPtr.Zero;
		public int dwProcessID	= 0;
		public int dwThreadID	= 0;
	}

	internal sealed class SHELLEXECUTEEX
	{

		public UInt32 cbSize		= 60; 
		public UInt32 fMask			= 0; 
		public IntPtr hwnd			= IntPtr.Zero; 
		public IntPtr lpVerb		= IntPtr.Zero; 
		public IntPtr lpFile		= IntPtr.Zero; 
		public IntPtr lpParameters	= IntPtr.Zero; 
		public IntPtr lpDirectory	= IntPtr.Zero; 
		public int nShow			= 0; 
		public IntPtr hInstApp		= IntPtr.Zero; 
		public IntPtr lpIDList		= IntPtr.Zero; 
		public IntPtr lpClass		= IntPtr.Zero; 
		public IntPtr hkeyClass		= IntPtr.Zero; 
		public UInt32 dwHotKey		= 0; 
		public IntPtr hIcon			= IntPtr.Zero; 
		public IntPtr hProcess		= IntPtr.Zero; 
	}

	/*internal struct NOTIFYICONDATA
	{
		public int cbSize;
		public IntPtr hWnd;
		public uint uID;
		public uint uFlags;
		public uint uCallbackMessage;
		public IntPtr hIcon;
		//char  szTip[64];
	}*/

	//Helper class to receive messages from tray icon
	#if!NDOC
	/*internal sealed class TrayIconMessageWindow : MessageWindow
	{		
		public event BeforeIconMessageEventHandler BeforeIconMessageEvent;
		public event AfterIconMessageEventHandler AfterIconMessageEvent;

		public TrayIconMessageWindow()
		{
		}

		protected override void WndProc(ref Message m)
		{
			if ((BeforeIconMessageEvent != null) && (BeforeIconMessageEvent(ref m)))
			{
				return;
			}
			
			base.WndProc (ref m);

			AfterIconMessageEvent(ref m);

			base.WndProc (ref m);
		}
	}*/
	#endif
	#endregion

}

/* ============================== Change Log ==============================
 * see Changelog.txt
 * ======================================================================== */
