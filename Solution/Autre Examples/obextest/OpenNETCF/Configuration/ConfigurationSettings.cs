//==========================================================================================
//
//		OpenNETCF.Configuration.ConfigurationSettings
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
using System.Collections.Specialized;
using System.Threading;
using System.Globalization;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Provides access to configuration settings in a specified configuration section. This class cannot be inherited.
	/// <para><b>New in v1.1</b></para>
	/// </summary>
	public sealed class ConfigurationSettings
	{
		// The Configuration System
		private static IConfigurationSystem configSystem = null;
		private static bool configurationInitialized = false;
		private static Exception initError = null;
		private static string appBase = null;

		private ConfigurationSettings() {}

		internal static bool SetConfigurationSystemInProgress
		{
			get { return ( (configSystem != null) && (configurationInitialized == false) ); }
		}

		/// <summary>
		/// Gets configuration settings in the configuration section.
		/// </summary>
		public static NameValueCollection AppSettings
		{
			get
			{
				appBase = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;
				ReadOnlyNameValueCollection appSettings = (ReadOnlyNameValueCollection)GetConfig("appSettings");
				
				if (appSettings == null)
				{
					appSettings = new ReadOnlyNameValueCollection(new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
					appSettings.SetReadOnly();
				}

				return appSettings;
			}
		}

		/// <summary>
		/// Returns configuration settings for a user-defined configuration section.  
		/// </summary>
		/// <param name="sectionName">The configuration section to read.</param>
		/// <returns>The configuration settings for sectionName.</returns>
		public static object GetConfig(string sectionName)
		{
			if (!configurationInitialized)
			{
				lock(typeof(ConfigurationSettings))
				{

					if(appBase == null)
						appBase = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;

					if (configSystem == null && !SetConfigurationSystemInProgress)
					{
						SetConfigurationSystem(new DefaultConfigurationSystem(appBase));
					}

					appBase = null;
				}
			}
			if (initError != null)
			{
				throw initError;
			}
			else
			{
				return configSystem.GetConfig(sectionName);
			}
		}

		public static object GetConfig(string sectionName, string appPath)
		{
			if (!configurationInitialized)
			{
				lock(typeof(ConfigurationSettings))
				{

					if(appBase == null)
						appBase = appPath;

					if (configSystem == null && !SetConfigurationSystemInProgress)
					{
						SetConfigurationSystem(new DefaultConfigurationSystem(appBase));
					}

					appBase = null;
				}
			}
			if (initError != null)
			{
				throw initError;
			}
			else
			{
				return configSystem.GetConfig(sectionName);
			}
		}

		internal static void SetConfigurationSystem(IConfigurationSystem ConfigSystem)
		{
			lock(typeof(ConfigurationSettings))
			{
				if (configSystem != null)
				{
					throw new InvalidOperationException("Config system already set");
				}
			
				try
				{
					configSystem = ConfigSystem;
					configSystem.Init();
				}
				catch (Exception e)
				{
					initError = e;
					throw;
				}
				
				configurationInitialized = true;
			}
		}
	}
}
