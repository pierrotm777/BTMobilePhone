//==========================================================================================
//
//		OpenNETCF.Configuration.DefaultConfigurationSystem
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
using System.IO;
using System.Reflection;
using System.Threading;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Summary description for DefaultConfigurationSystem.
	/// </summary>
	class DefaultConfigurationSystem : IConfigurationSystem 
	{
		private const string ConfigExtension = ".config";
		private const string MachineConfigFilename = "machine.config";
		private const string MachineConfigSubdirectory = "config";
		private static string _codeBase = null;
		private ConfigurationRecord _application;
		
		internal DefaultConfigurationSystem()
		{
		}

		public DefaultConfigurationSystem(string CodeBase)
		{
			_codeBase = CodeBase;
		}

		object IConfigurationSystem.GetConfig(string configKey) 
		{
			if (_application != null) 
			{
				return _application.GetConfig(configKey);
			}
			else 
			{
				throw new InvalidOperationException("Client config init error");
			}
		}

		void IConfigurationSystem.Init() 
		{
			lock(this) 
			{
				if(_application == null) 
				{
					ConfigurationRecord machineConfig = null;
					string machineConfigFilename = MachineConfigurationFilePath;
					Uri appConfigFilename = AppConfigPath;
					_application = machineConfig = new ConfigurationRecord();
					bool machineConfigExists = machineConfig.Load(machineConfigFilename);

					// Only load the app.config is machine.config exists
					if(machineConfigExists && appConfigFilename != null) 
					{
						_application = new ConfigurationRecord(machineConfig);
						_application.Load(appConfigFilename.ToString());
					}
				}
			}            
		}

		internal static string MsCorLibDirectory 
		{
			get 
			{ 
				string corCodeBase = typeof(object).Assembly.GetName().CodeBase;
				int separatorIndex = corCodeBase.IndexOf("\\", 2) + 1;
				
				string filename = corCodeBase.Substring(0, separatorIndex);//.Replace('/','\\');
				filename = filename.Replace("/","\\");
				return Path.GetDirectoryName(filename);
			}
		}

		internal static string MachineConfigurationFilePath 
		{
			get 
			{
				return Path.Combine(Path.Combine(MsCorLibDirectory, MachineConfigSubdirectory), MachineConfigFilename);
			}
		}

		internal static Uri AppConfigPath 
		{
			get 
			{                
				try 
				{
					string appBase = ApplicationBase();
                
					// we need to ensure AppBase ends in an '/'.                                  
					if (appBase.Length > 0) 
					{
						char lastChar = appBase[appBase.Length - 1];
						if (lastChar != '/' && lastChar != '\\') 
						{
							appBase += '\\';
						}
					}
					Uri uri = new Uri(appBase);
					string config = ConfigurationFile();
					if (config != null && config.Length > 0) 
					{
						uri = new Uri(uri, config);
						return uri;
					}
				}
				finally 
				{
                    
				}
				return null;
			}
		}

		private static string GetCodeBase() 
		{
			return _codeBase; //Assembly.GetCallingAssembly().GetName().CodeBase;
		}

		private static string ApplicationBase() 
		{
			string codeBase = GetCodeBase();
			return codeBase.Substring(0,codeBase.LastIndexOf("\\"));
		}

		private static string ConfigurationFile() 
		{
			return GetCodeBase() + ConfigExtension;
		}
	}
}
