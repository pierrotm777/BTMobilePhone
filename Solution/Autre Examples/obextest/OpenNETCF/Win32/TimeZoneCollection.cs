//==========================================================================================
//
//		OpenNETCF.Win32.TimeZoneCollection
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

using System.Text.RegularExpressions;
using OpenNETCF.Win32;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Define a TimeZoneCollection which can return a list of all
	/// of the TimeZones known to the OS.  This should help make
	/// time zone picker controls, etc.
	/// </summary>
	public class TimeZoneCollection : System.Collections.ArrayList 
	{
		public const int ALL_TIMEZONES_LIST = 999;

		/// <summary>
		/// Creates a new instance of TimeZoneCollection
		/// </summary>
		public TimeZoneCollection()
		{
		}

		/// <summary>
		/// Refreshes the contents of the TimeZoneCollection
		/// </summary>
		public void Refresh()
		{
			// Clear the collection and reinitialize.
			this.Clear();

			this.Initialize();
		}

		/// <summary>
		/// The Initialize() method is equivalent to calling
		/// Initialize( ALL_TIMEZONES_LIST ).  It populates the
		/// collection with all OS-known timezones.
		/// </summary>
		public void Initialize()
		{
			this.Initialize( ALL_TIMEZONES_LIST );
		}

		/// <summary>
		/// Use the Initialize method with the gmtOffset parameter
		/// to create a list of timezones which have a specific
		/// offset value.  For example, for all timezones at 
		/// GMT-5:00, pass -300 (minutes).  To get all of the time
		/// zones known to the OS, pass ALL_TIMEZONES_LIST or call
		/// the method with no parameters.
		/// </summary>
		/// <param name="gmtOffset">Offset of the target timezones
		/// from GMT (in minutes).</param>
		public void Initialize( int gmtOffset )
		{
			// On Windows CE 3.0 other than Pocket PC, we can scan
			// the registry for a list of timezones.  

			// HKLM\Time Zones is the root.  Under this root are all
			// of the timezone entries that the system knows about.
			// The name of each key for a timezone entry is the 
			// 'standard time' description for the timezone (Eastern
			// Standard Time, for example).  Under this key are various
			// values:
			// Display - The name of the timezone for display purposes.
			// This value includes the offset from GMT, as well as the
			// name, in some form, of the timezone.
			// Dlt - The name of the timezone for the 'daylight time'
			// part of the year (Eastern Daylight Time, for example).
			// TZI - A binary structure, like TIME_ZONE_INFORMATION,
			// describing the characteristics of the timezone.

			// Scan through the registry, looking for the indicated
			// timezone offset from GMT in the display string.  When 
			// we find it, we add it to a list, since there might
			// be several matches (Arizona and normal MST would both
			// match GMT-7, for example).
			RegistryKey	baseKey;

			baseKey = Registry.LocalMachine.OpenSubKey("Time Zones", false);
			if ( baseKey != null )
			{

				// Enumerate all keys under the base timezone key.
					//string	cls;
					string[] subkeynames = baseKey.GetSubKeyNames();
					foreach(string thiskeyname in subkeynames)
					{

						// Open the enumerated key.
						RegistryKey	tzKey = baseKey.OpenSubKey(thiskeyname, false);
						if ( tzKey  != null )
						{
							// Get the display name value for the timezone.
							string			dispName;
							
							dispName = tzKey.GetValue("Display").ToString();

							if(dispName != null)
							{
								// Extract the offset from GMT from the display string.
								// The basic format of the string is:
								// (GMT-%02d:%02d) <Name>
								// The problem is when you are *at* GMT, where the sign 
								// and the numbers aren't present.
								int	hoursOffset = 999, minutesOffset = 999;
//								_stscanf( dispName, "(GMT%d:%d)",
//									&hoursOffset, &minutesOffset );
								string	hours;
								string	minutes;
								hours = dispName.Substring( 4, 3 );
								minutes = dispName.Substring( 8, 2 );

								// For the case of GMT itself, there is no
								// offset in the display name, so we
								// set the offset to zero, in that case.
								if ( ( hours[ 0 ] == '-' ) ||
									( hours[ 0 ] == '+' ) )
								{
									hoursOffset = System.Int32.Parse( hours );
									minutesOffset = System.Int32.Parse( minutes );
								}
								else
								{
									hoursOffset = 0;
									minutesOffset = 0;
								}

								// Convert the hours and minutes offset to a decimal 
								// (30 minutes equals 0.5 hours).
								double	offset = hoursOffset;

								if ( hoursOffset < 0 )
								{
									offset -= minutesOffset / 60.0;
								}
								else
								{
									offset += minutesOffset / 60.0;
								}

								// We now have a value which we can compare to the 
								// value that the user gave us.
								if ( ( gmtOffset == offset ) || 
									( gmtOffset == ALL_TIMEZONES_LIST ) )
								{
									// Match.  Add timezone to list.  So far, we have
									// the standard time name and the display name.  
									// We still need to get the DST name and the 
									// TIME_ZONE_INFORMATION.
									string	dstName;
									dstName = tzKey.GetValue("Dlt").ToString();

									// Read the time zone information from the 
									// registry.  Unfortunately, this is something
									// that only the Control Panel code actually 
									// knows about, so, if they change it, we break.
									TZREG					tzr = new TZREG();
									int						tzrSize = tzr.ToByteArray().Length;
									TimeZoneInformation	tzi = new TimeZoneInformation();
									byte[]					btzr;

									btzr = (byte[])tzKey.GetValue("TZI");

									tzr = new TZREG( btzr );

									tzi.Bias = tzr.Bias;
									tzi.StandardBias = tzr.StandardBias;
									tzi.DaylightBias = tzr.DaylightBias;
									tzi.StandardDate = tzr.StandardDate;
									tzi.DaylightDate = tzr.DaylightDate;

									// Don't forget to copy the standard name and
									// daylight name to the structure.
									tzi.DaylightName = dstName;
									tzi.StandardName = thiskeyname;

									// Copy the display name from the registry to
									// the class.
									tzi.DisplayName = dispName;

									this.Add( tzi );
								}
							}//endif dispname null

							// close the timezone key.
							tzKey.Close();

						}//endif tzkey null

					}//end foreach subkeynames

				// Close the key.
				baseKey.Close();
			}
		}
	}
}
 

 /* ============================== Change Log ==============================
 * ---------------------
 * August 21, 2003, Peter Foot
 * ---------------------
 * Replaced registry calls with new model registry (currently links to existing external assembly)
 *
 * ---------------------
 * May 13, 2003, Alex Feinman
 * ---------------------
 * - Added changelog
 * - Updated call to RegKey.EnumSubkeys to reflect recent changes
 *
 * ======================================================================== */
