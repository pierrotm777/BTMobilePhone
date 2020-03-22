using System;
using System.Reflection;
using System.Collections;
using OpenNETCF.Configuration;

namespace OpenNETCF.Diagnostics 
{
	internal enum InitState 
	{
		NotInitialized,
		Initializing,
		Initialized
	}

	internal class DiagnosticsConfiguration 
	{
		private static Hashtable configTable;
		private static InitState initState = InitState.NotInitialized;   
		private static string appBase = string.Empty;

		// Setting for TraceInternal.AutoFlush
		internal static bool AutoFlush 
		{
			get 
			{ 
				Initialize();
				if (configTable != null && configTable.ContainsKey("autoflush"))
					return (bool)configTable["autoflush"];
				else
					return false; // the default
			}
		}

		// Setting for TraceInternal.IndentSize
		internal static int IndentSize 
		{
			get 
			{ 
				Initialize();
				if (configTable != null && configTable.ContainsKey("indentsize"))
					return (int)configTable["indentsize"];
				else
					return 4; // the default
			}
		}

        
		private static Hashtable GetConfigTable() 
		{
			//string appBase = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;
			Hashtable configTable = (Hashtable) ConfigurationSettings.GetConfig("system.diagnostics", appBase);
			return configTable;
		}

		internal static bool IsInitializing() 
		{
			return initState == InitState.Initializing;
		}

		internal static bool CanInitialize() 
		{
			return (initState != InitState.Initializing) && !(ConfigurationSettings.SetConfigurationSystemInProgress);
		}
        
		internal static void Initialize() 
		{
			lock (typeof(DiagnosticsConfiguration)) 
			{
				if (initState != InitState.NotInitialized || ConfigurationSettings.SetConfigurationSystemInProgress)
					return;

				// Prevent recursion
				initState = InitState.Initializing; 
				try 
				{
					//appBase = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;
					configTable = GetConfigTable();
				}
				finally 
				{
					initState = InitState.Initialized;
				}
			}
		}

		internal static string AppBase
		{
			get { return appBase; }
			set { appBase = value; }
		}
	}
}


