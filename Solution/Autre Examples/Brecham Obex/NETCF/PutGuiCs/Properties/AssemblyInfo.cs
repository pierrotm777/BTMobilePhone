using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PutGuiCs")]
//[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("PutGuiCs")]
[assembly: AssemblyCopyright("Copyright © 2006 Andy Hume")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("bde6e536-0255-4728-aa62-79a083dcd0df")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.0")]

// Report the built type
[assembly: AssemblyDescription(""
#if NETCF
    + "NETCF "
#endif
#if FX1_1
    + "FX1_1 "
#endif
#if DEBUG
    + "DEBUG "
#else // RELEASE
    + "RELEASE "
#endif
#if CODE_ANALYSIS
    + "CODE_ANALYSIS "
#endif
)]

