//==========================================================================================
//
//		OpenNETCF.Win32.DateTimeEx
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

namespace OpenNETCF.Win32
{
	#region DateTimeEx Class
	/// <summary>
	/// Provides additional <see cref="T:System.DateTime"/> functions
	/// </summary>
	public class DateTimeEx
	{
		private DateTimeEx(){}

		#region Time Zone Functions
		/// <summary>
		/// This function sets the current time-zone parameters. These parameters control translations from Coordinated Universal Time (UTC) to local time
		/// </summary>
		/// <param name="tzi"></param>
		public static void SetTimeZoneInformation( TimeZoneInformation tzi )
		{
			// Call CE function (implicit conversion occurs to
			// byte[]).
			if(! SetTimeZoneInformationCE( tzi ))
			{
				throw new WinAPIException("Cannot Set Time Zone", Marshal.GetLastWin32Error());
			}
		}

		/// <summary>
		/// This function gets the time-zone parameters for the active
		/// time-zone. These parameters control translations from Coordinated 
		/// Universal Time (UTC) to local time.
		/// </summary>
		/// <param name="tzi"></param>
		public static TimeZoneState GetTimeZoneInformation( ref TimeZoneInformation tzi )
		{
			// Call CE function (implicit conversion occurs to
			// byte[]).
			TimeZoneState	stat;
			if( ( stat = GetTimeZoneInformationCE( tzi ) ) == 
				TimeZoneState.Unknown )
			{
				throw new WinAPIException("Cannot Get Time Zone");
			}

			return stat;
		}
		#endregion

		#region Local Time
		/*
		/// <summary>
		/// This function sets the current local time and date
		/// </summary>
		/// <param name="year">Current Year</param>
		/// <param name="month">Current Month</param>
		/// <param name="day">Current Day</param>
		/// <param name="hour">Current Hour</param>
		/// <param name="minute">Current Minute</param>
		/// <param name="second">Current Second</param>
		public static void SetLocalTime(int year, int month, int day, int hour, int minute, int second)
		{
			SystemTime st = new SystemTime(Convert.ToUInt16(year), Convert.ToUInt16(month), Convert.ToUInt16(day), Convert.ToUInt16(hour), Convert.ToUInt16(minute), Convert.ToUInt16(second));
			if(! SetLocalTime(st.ToByteArray()))
			{
				throw new WinAPIException("Cannot Set Time");
			}
		}*/

		/// <summary>
		/// This function sets the current local time and date
		/// </summary>
		/// <param name="dt">DateTime representing required Local Time</param>
		public static void SetLocalTime(System.DateTime dt)
		{
			SystemTime st = SystemTime.FromDateTime(dt);
			if(!SetLocalTime(st.ToByteArray()))
			{
				throw new WinAPIException("Cannot set Local Time");
			}
		}
		#endregion

		#region System Time
		/*
		/// <summary>
		/// This function sets the current system time and date. The system time is expressed in Coordinated Universal Time (UTC).
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <param name="hour"></param>
		/// <param name="minute"></param>
		/// <param name="second"></param>
		public static void SetSystemTime(int year, int month, int day, int hour, int minute, int second)
		{
			SystemTime st = new SystemTime(Convert.ToUInt16(year), Convert.ToUInt16(month), Convert.ToUInt16(day), Convert.ToUInt16(hour), Convert.ToUInt16(minute), Convert.ToInt16(second));
			if(! SetSystemTime(st.ToByteArray()))
			{
				throw new WinAPIException("Cannot Set Time");
			}
		}*/

		/// <summary>
		/// This function sets the current system time and date. The system time is expressed in Coordinated Universal Time (UTC).
		/// </summary>
		/// <param name="dt">Required DateTime</param>
		public static void SetSystemTime(System.DateTime dt)
		{
			SystemTime st = SystemTime.FromDateTime(dt);

			if(!SetSystemTime(st.ToByteArray()))
			{
				throw new WinAPIException("Cannot set System Time");
			}
		}
		#endregion

		
		#region Date/Time P/Invokes

		[DllImport("coredll", EntryPoint="GetTimeZoneInformation", SetLastError=true)]
		internal static extern TimeZoneState GetTimeZoneInformationCE( byte[] tzice ); 

		[DllImport("coredll", EntryPoint="SetTimeZoneInformation", SetLastError=true)]
		internal static extern bool SetTimeZoneInformationCE( byte[] tzice ); 

		[DllImport("coredll", SetLastError=true)]
		internal static extern bool SetLocalTime(byte[] st);

		[DllImport("coredll", SetLastError=true)]
		internal static extern bool SetSystemTime(byte[] st);

		#endregion
	}
	#endregion


	#region TZReg
	/// <summary>
	/// Time-Zone
	/// </summary>
	public class TZREG
	{
		// Declare array of bytes to represent the structure
		// contents in a form which can be marshalled.
		private byte[]	flatStruct = new byte[ 4 + 4 + 4 + 16 + 16 ];

		#region Flat structure offset constants
		private const int	biasOffset = 0;
		private const int	standardBiasOffset = 4;
		private const int	daylightBiasOffset = 4 + 4;
		private const int	standardDateOffset = 4 + 4 + 4;
		private const int	daylightDateOffset = 4 + 4 + 4 + 16 /* sizeof( SYSTEMTIME ) */;
		#endregion

		public TZREG()
		{
		}

		public TZREG( byte[] bytes ) : 
			this( bytes, 0 )
		{
		}

		public TZREG( byte[] bytes, int offset )
		{
			// Dump the byte array into our array.
			Buffer.BlockCopy( bytes, offset, flatStruct, 0, flatStruct.Length );
		}

		public byte[] ToByteArray()
		{
			return flatStruct;
		}

		public static implicit operator byte[]( TZREG tzr )
		{
			return tzr.ToByteArray();
		}

		public int Bias
		{
			get
			{
				return BitConverter.ToInt32( flatStruct, biasOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, biasOffset, 4 );
			}
		}
		public int StandardBias
		{
			get
			{
				return BitConverter.ToInt32( flatStruct, standardBiasOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, standardBiasOffset, 4 );
			}
		}
		public int DaylightBias
		{
			get
			{
				return BitConverter.ToInt32( flatStruct, daylightBiasOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, daylightBiasOffset, 4 );
			}
		}
		public SystemTime StandardDate
		{
			get
			{
				return new SystemTime( flatStruct, standardDateOffset );
			}
			set
			{
				byte[]	bytes = value.ToByteArray();
				Buffer.BlockCopy( bytes, 0, flatStruct, standardDateOffset, 16 );
			}
		}
		public SystemTime DaylightDate
		{
			get
			{
				return new SystemTime( flatStruct, daylightDateOffset );
			}
			set
			{
				byte[]	bytes = value.ToByteArray();
				Buffer.BlockCopy( bytes, 0, flatStruct, daylightDateOffset, 16 );
			}
		}
	}
	#endregion


	#region Time Zone Information
	/// <summary>
	/// Time Zone information
	/// </summary>
	/// <remarks>Wraps the native <b>TIME_ZONE_INFORMATION</b> structure.</remarks>
	public class TimeZoneInformation
	{
		// Declare simple string to be used as the display name
		// for the timezone.  This would be used when creating
		// a collection of all known timezones.
		private string	displayName;

		// Declare array of bytes to represent the structure
		// contents in a form which can be marshalled.
		private byte[]	flatStruct = new byte[ 4 + 64 + 16 + 4 + 64 + 16 + 4 ];

		// Now, declare the offsets of the various fields within
		// that array of bytes.
		#region Flat structure offset constants
		private const int	biasOffset = 0;
		private const int	standardNameOffset = 4;
		private const int	standardNameLengthBytes = 64;
		private const int	standardDateOffset = 4 + 64 /* sizeof( WCHAR ) * 32 */;
		private const int	standardBiasOffset = 4 + 64 + 16 /* sizeof( SYSTEMTIME ) */;
		private const int	daylightNameOffset = 4 + 64 + 16 + 4;
		private const int	daylightNameLengthBytes = 64;
		private const int	daylightDateOffset = 4 + 64 + 16 + 4 + 64;
		private const int	daylightBiasOffset = 4 + 64 + 16 + 4 + 64 + 16;
		#endregion

		#region Constructor
		/// <summary>
		/// Create a new instance of the <b>TimeZoneInformation</b> class.
		/// </summary>
		public TimeZoneInformation()
		{
		}
		#endregion

		#region Constructor (Byte[])
		/// <summary>
		/// Create a new instance of the TimeZoneInformation class based on data in the supplied Byte Array.
		/// </summary>
		/// <param name="bytes">Byte Array containing <b>TIME_ZONE_INFORMATION</b> data.</param>
		public TimeZoneInformation( byte[] bytes ) : 
			this( bytes, 0 )
		{
		}
		#endregion

		public TimeZoneInformation( byte[] bytes, int offset )
		{
			// Dump the byte array into our array.
			Buffer.BlockCopy( bytes, offset, flatStruct, 0, flatStruct.Length );
		}

		// ???? need constructors with various elements for 
		// creating 'custom' timezones.

		#region To String
		/// <summary>
		/// Returns a String representing this instance of <b>TimeZoneInformation</b>.
		/// </summary>
		/// <returns>A string containing the name of the Time Zone.</returns>
		public override string ToString()
		{
			// Set the string to the default description of the
			// timezone.
			return this.DisplayName;
		}
		#endregion

		/// <summary>
		/// Returns the raw structure in a <see cref="T:System.Byte[]"/>.
		/// </summary>
		/// <returns>Byte array containing the <b>SYSTEMTIME</b> data.</returns>
		public byte[] ToByteArray()
		{
			return flatStruct;
		}

		public static implicit operator byte[]( TimeZoneInformation tzi )
		{
			return tzi.ToByteArray();
		}

		#region Display Name
		/// <summary>
		/// Name used to describe the Time Zone.
		/// </summary>
		public string DisplayName
		{
			get
			{
				return displayName;
			}
			set
			{
				displayName = value;
			}
		}
		#endregion

		#region Bias
		/// <summary>
		/// Specifies the current bias, in minutes, for local time translation on this computer.
		/// </summary>
		/// <remarks>The bias is the difference, in minutes, between Coordinated Universal Time (UTC) and local time.
		/// All translations between UTC and local time are based on the following formula:
		/// <para>UTC = local time + bias</para></remarks>
		public int Bias
		{
			get
			{
				return BitConverter.ToInt32( flatStruct, biasOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, biasOffset, 4 );
			}
		}
		#endregion

		#region Standard Name
		/// <summary>
		/// Name associated with standard time on this device.
		/// </summary>
		/// <remarks>For example, this member could contain “EST” to indicate Eastern Standard Time.</remarks>
		public string StandardName
		{
			get
			{
				String sReturn = System.Text.Encoding.Unicode.GetString( flatStruct, standardNameOffset, standardNameLengthBytes /* in BYTES */ );
				return sReturn;
			}
			set
			{
				byte[] bytes = System.Text.Encoding.Unicode.GetBytes( value );
				Buffer.BlockCopy(bytes, 0, flatStruct, standardNameOffset, Math.Min( bytes.Length, standardNameLengthBytes ) );
			}
		}
		#endregion

		#region Standard Date
		/// <summary>
		/// The date and local time when the transition from Daylight time to Standard time occurs.
		/// </summary>
		/// <remarks>This member supports two date formats.
		/// Absolute format specifies an exact date and time when standard time begins.
		/// In this form, the wYear, wMonth, wDay, wHour, wMinute, wSecond, and wMilliseconds members of the SYSTEMTIME structure are used to specify an exact date.
		/// <para>Day-in-month format is specified by setting the wYear member to zero, setting the wDayOfWeek member to an appropriate weekday, and using a wDay value in the range 1 through 5 to select the correct day in the month.
		/// Using this notation, the first Sunday in April can be specified, as can the last Thursday in October (5 is equal to “the last”).</para></remarks>
		public SystemTime StandardDate
		{
			get
			{
				return new SystemTime( flatStruct, standardDateOffset );
			}
			set
			{
				byte[]	bytes = value.ToByteArray();
				Buffer.BlockCopy( bytes, 0, flatStruct, standardDateOffset, 16 );
			}
		}
		#endregion

		#region Standard Bias
		/// <summary>
		/// Specifies a bias value to be used during local time translations that occur during standard time.
		/// </summary>
		/// <remarks>This member is ignored if a value for the StandardDate member is not supplied.
		/// This value is added to the value of the Bias member to form the bias used during standard time.
		/// In most time zones, the value of this member is zero.</remarks>
		public int StandardBias
		{
			get
			{
				return BitConverter.ToInt32( flatStruct, standardBiasOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, standardBiasOffset, 4 );
			}
		}
		#endregion

		#region Daylight Name
		/// <summary>
		/// Specifies a string associated with daylight time.
		/// </summary>
		public string DaylightName
		{
			get
			{
				String sReturn = System.Text.Encoding.Unicode.GetString( flatStruct, daylightNameOffset, daylightNameLengthBytes /* in BYTES */ );
				return sReturn;
			}
			set
			{
				value=value.TrimEnd(new char[]{(char)0});
				byte[] bytes = System.Text.Encoding.Unicode.GetBytes( value );
				Buffer.BlockCopy(bytes, 0, flatStruct, daylightNameOffset, Math.Min( bytes.Length, daylightNameLengthBytes ) );
			}
		}
		#endregion

		#region Daylight Date
		/// <summary>
		/// Specifies a date and local time when the transition from standard time to daylight time occurs. 
		/// </summary>
		public SystemTime DaylightDate
		{
			get
			{
				return new SystemTime( flatStruct, daylightDateOffset );
			}
			set
			{
				byte[]	bytes = value.ToByteArray();
				Buffer.BlockCopy( bytes, 0, flatStruct, daylightDateOffset, 16 );
			}
		}
		#endregion

		#region Daylight Bias
		/// <summary>
		/// Specifies a bias value to be used during local time translations that occur during daylight time.
		/// </summary>
		public int DaylightBias
		{
			get
			{
				return BitConverter.ToInt32( flatStruct, daylightBiasOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, daylightBiasOffset, 4 );
			}
		}
		#endregion
	}
	#endregion


	#region Time Zone State enum
	/// <summary>
	/// Return values from <see cref="M:OpenNETCF.Win32.DateTimeEx.GetTimeZoneInformation"/>.
	/// </summary>
	public enum TimeZoneState : int
	{
		/// <summary>
		/// The system cannot determine the current time zone.
		/// This value is returned if daylight savings time is not used in the current time zone, because there are no transition dates.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// The system is operating in the range covered by the StandardDate member of the <see cref="OpenNETCF.Win32.TimeZoneInformation"/> structure.
		/// </summary>
		Standard = 1,
		/// <summary>
		/// The system is operating in the range covered by the DaylightDate member of the <see cref="OpenNETCF.Win32.TimeZoneInformation"/> structure.
		/// </summary>
		Daylight = 2
	}
	#endregion
}
