using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Brecham TransportConnection")]

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

// Report the built type
[assembly: AssemblyConfiguration(""
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

[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("Copyright © 2006 Andy Hume")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f8bcf670-99ac-482a-90b4-4339b87420fb")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.7.1012.0")] // "1.7.*")]

//----------------------------------------------------------------------
#if ! FX1_1
#if ! NETCF
// Security Transparent doesn't exist in v1, so, being doubly cautious,
// don't set either.
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Security.SecurityTransparent]
// Is this right?  FxCop recommends it...
[assembly: System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.RequestMinimum, Name = "Execution")]
#endif
#endif //! FX1_1


//----------------------------------------------------------------------
[assembly: System.CLSCompliant(true)]


//----------------------------------------------------------------------
// EOF
