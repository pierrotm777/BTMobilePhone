# Microsoft Developer Studio Project File - Name="sdksample" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Console Application" 0x0103

CFG=sdksample - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "sdksample.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "sdksample.mak" CFG="sdksample - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "sdksample - Win32 Release" (based on "Win32 (x86) Console Application")
!MESSAGE "sdksample - Win32 Debug" (based on "Win32 (x86) Console Application")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""$/006_BlueSoleil5/sdksample", GEUBAAAA"
# PROP Scc_LocalPath "."
CPP=cl.exe
RSC=rc.exe

!IF  "$(CFG)" == "sdksample - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_CONSOLE" /D "_MBCS" /YX /FD /c
# ADD CPP /nologo /MDd /W3 /GX /O2 /I "..\SDKheaders\include" /D "WIN32" /D "NDEBUG" /D "_CONSOLE" /D "_MBCS" /YX /FD /c
# ADD BASE RSC /l 0x804 /d "NDEBUG"
# ADD RSC /l 0x804 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:console /machine:I386
# ADD LINK32 winmm.lib Setupapi.lib Advapi32.lib ws2_32.lib Shlwapi.lib Winspool.lib Msacm32.lib BSSDK.lib kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib BSSDK.lib bmsg_parser.lib /nologo /subsystem:console /machine:I386 /out:"../bin/sdksample_release.exe" /libpath:"..\BIN" /libpath:"$(MSSDK)\Lib"
# SUBTRACT LINK32 /pdb:none /nodefaultlib

!ELSEIF  "$(CFG)" == "sdksample - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_CONSOLE" /D "_MBCS" /YX /FD /GZ /c
# ADD CPP /nologo /MDd /W3 /Gm /GX /ZI /Od /I "..\SDKheaders\include" /D "WIN32" /D "_DEBUG" /D "_CONSOLE" /D "_MBCS" /FR /YX /FD /GZ /c
# ADD BASE RSC /l 0x804 /d "_DEBUG"
# ADD RSC /l 0x804 /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:console /debug /machine:I386 /pdbtype:sept
# ADD LINK32 msvcrtd.lib winmm.lib Setupapi.lib Advapi32.lib ws2_32.lib Shlwapi.lib Winspool.lib Msacm32.lib BSSDK.lib kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib BSSDK.lib bmsg_parserd.lib /nologo /subsystem:console /map /debug /machine:I386 /nodefaultlib:"libcmtd.lib" /nodefaultlib:"libcd.lib" /out:"../bin/sdksample_debug.exe" /pdbtype:sept /libpath:"..\BIN" /libpath:"$(MSSDK)\Lib"
# SUBTRACT LINK32 /pdb:none /nodefaultlib

!ENDIF 

# Begin Target

# Name "sdksample - Win32 Release"
# Name "sdksample - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=.\av_tst.c
# End Source File
# Begin Source File

SOURCE=.\ftpopp_tst.c
# End Source File
# Begin Source File

SOURCE=.\gatt_test.c
# End Source File
# Begin Source File

SOURCE=.\hfp_tst.c
# End Source File
# Begin Source File

SOURCE=.\hid_tst.c
# End Source File
# Begin Source File

SOURCE=.\loc_dev_tst.c
# End Source File
# Begin Source File

SOURCE=.\map_tst.c
# End Source File
# Begin Source File

SOURCE=.\pan_tst.c
# End Source File
# Begin Source File

SOURCE=.\pbap_tst.c
# End Source File
# Begin Source File

SOURCE=.\profiles_tst.c
# End Source File
# Begin Source File

SOURCE=.\report.c
# End Source File
# Begin Source File

SOURCE=.\rmt_dev_tst.c
# End Source File
# Begin Source File

SOURCE=.\rmt_svc_tst.c
# End Source File
# Begin Source File

SOURCE=.\sdk_tst.c
# End Source File
# Begin Source File

SOURCE=.\spp_tst.c
# End Source File
# Begin Source File

SOURCE=.\vcard_parser\vcc.c
# End Source File
# Begin Source File

SOURCE=.\vcard_parser\vobject.c
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=.\map_tst.h
# End Source File
# Begin Source File

SOURCE=.\profiles_tst.h
# End Source File
# Begin Source File

SOURCE=.\sdk_gatt.h
# End Source File
# Begin Source File

SOURCE=.\sdk_tst.h
# End Source File
# Begin Source File

SOURCE=.\vcard_parser\vcc.h
# End Source File
# Begin Source File

SOURCE=.\vcard_parser\vobject.h
# End Source File
# End Group
# Begin Group "Resource Files"

# PROP Default_Filter "ico;cur;bmp;dlg;rc2;rct;bin;rgs;gif;jpg;jpeg;jpe"
# End Group
# End Target
# End Project
