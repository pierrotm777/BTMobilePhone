using System;
using System.Diagnostics;

namespace OpenNETCF.Diagnostics
{
	public sealed class TraceEx
	{
		private static string appBase = string.Empty;
		private static bool initialized = false;

		public static bool AutoFlush 
		{ 
			get 
			{
				if(!initialized) 
					  InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
				return TraceInternalEx.AutoFlush; 
			}
			set 
			{
				if(!initialized) 
					InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
				TraceInternalEx.AutoFlush = value; 
			}
		}

		[ConditionalAttribute("TRACE")]
		public static void Assert(bool condition)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.Assert(condition);
		}

		[ConditionalAttribute("TRACE")]
		public static void Assert(bool condition, string message)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.Assert(condition, message);
		}

		[ConditionalAttribute("TRACE")]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.Assert(condition, message, detailMessage);
		}

		[ConditionalAttribute("TRACE")]
		public static void Write(object value)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.Write(value);
		}
		
		[ConditionalAttribute("TRACE")]
		public static void Write(string message)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.Write(message);
		}

		[ConditionalAttribute("TRACE")]
		public static void Write(string message, string category)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.Write(message, category);
		}

		[ConditionalAttribute("TRACE")]
		public static void Write(object value, string category)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.Write(value, category);
		}

		[ConditionalAttribute("TRACE")]
		public static void WriteLine(object value)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.WriteLine(value);
		}
		
		[ConditionalAttribute("TRACE")]
		public static void WriteLine(string message)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.WriteLine(message);
		}

		[ConditionalAttribute("TRACE")]
		public static void WriteLine(string message, string category)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.WriteLine(message, category);
		}

		[ConditionalAttribute("TRACE")]
		public static void WriteLine(object value, string category)
		{
			if(!initialized) 
				InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
			TraceInternalEx.WriteLine(value, category);
		}
		
		public static TraceListenerCollection Listeners
		{
			get
			{
				if(!initialized) 
					InitializeSettings(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase); 
				return TraceInternalEx.Listeners;
			}
		}

		
		private static void InitializeSettings(string path)
		{
			TraceInternalEx.appBase = appBase = path;
			initialized = true;
		}
	}
}
