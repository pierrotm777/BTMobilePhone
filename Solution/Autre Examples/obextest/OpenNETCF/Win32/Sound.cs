//==========================================================================================
//
//		OpenNETCF.Win32.Sound
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
using System.Runtime.InteropServices;

// This file is now obsolete. All sound functionality has been refactored to follow the desktop .NET 2.0 framework
// OpenNETCF.Windows.Forms.SoundPlayer supports playing wave audio (PlaySound)
// OpenNETCF.Windows.Forms.SystemSounds allows playing predefined system sounds (MessageBeep)
// Peter Foot 12 July 2004

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Contains Sound-related P/Invokes
	/// </summary>
	[Obsolete("Use OpenNETCF.Windows.Forms.SoundPlayer for playing sounds", true)]
	public class Sound
	{
		private Sound(){}

		#region Play Sound
		/// <summary>
		/// Plays a sound specified by the specified file name, resource, or system event.
		/// </summary>
		/// <param name="name">String that specifies the sound to play. If this parameter is NULL, any currently playing waveform sound is stopped.</param>
		[Obsolete("Use OpenNETCF.Windows.Forms.SoundPlayer for playing sounds", true)]
		public static void PlaySound(string name)
		{
			PlaySound(name, IntPtr.Zero, 0);
		}

		/// <summary>
		/// Plays a sound specified by the specified file name, resource, or system event.
		/// </summary>
		/// <param name="name">String that specifies the sound to play. If this parameter is NULL, any currently playing waveform sound is stopped.</param>
		/// <param name="flags">Flags for playing the sound.</param>
		/// <seealso cref="T:OpenNETCF.Win32.Sound.SoundFlags"/>
		[Obsolete("Use OpenNETCF.Windows.Forms.SoundPlayer for playing sounds", true)]
		public static void PlaySound(string name, SoundFlags flags)
		{
			PlaySound(name, IntPtr.Zero, flags);
		}

		/// <summary>   
		/// Plays a sound specified by the specified file name, resource, or system event.   
		/// </summary>   
		/// <param name="name">String that specifies the sound to play. If this parameter is NULL, any currently playing waveform sound is stopped.</param>   
		/// <param name="hModule">Handle to the executable file that contains the resource to be loaded. This parameter must be NULL unless SoundFlags.Resource is specified in <paramref>dwFlags</paramref>.</param>   
		/// <param name="flags">Flags for playing the sound.</param>   
		/// <seealso cref="T:OpenNETCF.Win32.Sound.SoundFlags"/> 
		[Obsolete("Use OpenNETCF.Windows.Forms.SoundPlayer for playing sounds", true)]
		public static void PlaySound(string name, IntPtr hModule, SoundFlags flags)
		{
			if(!PlaySoundW(name, hModule, (uint)flags))
			{
				throw new WinAPIException("Cannot play sound");
			}
		}

		[DllImport("coredll", EntryPoint="PlaySoundW", SetLastError=true) ]
		private extern static bool PlaySoundW(String lpszName, IntPtr hModule, uint dwFlags);
		
		#endregion

		#region Sound Flags
		/// <summary>
		/// PlaySound flags
		/// </summary>
		[Flags(), Obsolete("Use OpenNETCF.Windows.Forms.SoundPlayer for playing sounds", true)]
		public enum SoundFlags : int
		{
			/// <summary>
			/// <b>name</b> is a WIN.INI [sounds] entry
			/// </summary>
			Alias      = 0x00010000,
			/// <summary>
			/// <b>name</b> is a file name
			/// </summary>
			FileName   = 0x00020000,
			/// <summary>
			/// <b>name</b> is a resource name or atom
			/// </summary>   
			Resource   = 0x00040004,   
			/// <summary>   
			/// Play synchronously (default)   
			/// </summary>   
			Sync       = 0x00000000,   
			/// <summary>   
			///  Play asynchronously   
			/// </summary>   
			Async      = 0x00000001,   
			/// <summary>   
			/// Silence not default, if sound not found   
			/// </summary>   
			NoDefault  = 0x00000002,   
			/// <summary>   
			/// <b>name</b> points to a memory file   
			/// </summary>   
			Memory     = 0x00000004,   
			/// <summary>   
			/// Loop the sound until next sndPlaySound   
			/// </summary>   
			Loop       = 0x00000008,   
			/// <summary>   
			/// Don't stop any currently playing sound   
			/// </summary>   
			NoStop     = 0x00000010,   
			/// <summary>   
			/// Don't wait if the driver is busy   
			/// </summary>   
			NoWait     = 0x00002000   
		}                  
		#endregion

		#region Beep
		/// <summary>
		/// This function plays a default system waveform sound.
		/// </summary>
		/// <remarks>Uses native MessageBeep function.</remarks>
		[Obsolete("Use OpenNETCF.Windows.Forms.SystemSounds for predefined system sounds", true)]
		public static void Beep()
		{
			Beep(BeepType.Default);
		}
		/// <summary>
		/// This function plays a system waveform sound.
		/// </summary>
		/// <param name="type">Specifies the sound type.</param>
		/// <remarks>Uses native MessageBeep function.</remarks>
		/// <seealso cref="T:OpenNETCF.Win32.Sound.BeepFlags"/>
		[Obsolete("Use OpenNETCF.Windows.Forms.SystemSounds for predefined system sounds", true)]
		public static void Beep(BeepType type)
		{
			uint beeptype = 0;
			if(type==BeepType.Default)
			{
				beeptype = 0xffffffff;
			}
			else
			{
				beeptype = Convert.ToUInt32(type);
			}
			MessageBeep(beeptype);
		}
		[DllImport("coredll", EntryPoint="MessageBeep", SetLastError=true)]
		private static extern void MessageBeep(uint type);
		#endregion

		#region Beep Type
		/// <summary>
		/// Specifies the sound type for the Beep function.
		/// </summary>
		/// <seealso cref="T:System.Windows.Forms.MessageBoxIcon"/>
		[Obsolete("Use OpenNETCF.Windows.Forms.SystemSounds for predefined system sounds", true)]
		public enum BeepType : int
		{
			/// <summary>
			/// Default beep sound.
			/// </summary>
			Default		= 0,//0xFFFFFFFF,
			/// <summary>
			/// Sound played during an Asterisk MessageBox.
			/// </summary>
			Asterisk	= 0x00000040,
			/// <summary>
			/// Sound played during an Exclamation MessageBox.
			/// </summary>
			Exclamation = 0x00000030,
			/// <summary>
			/// Sound played during a Question MessageBox.
			/// </summary>
			Question	= 0x00000020,
			/// <summary>
			/// Sound played during a Hand MessageBox.
			/// </summary>
			Hand		= 0x00000010,
		}
		#endregion
		
	}
}
