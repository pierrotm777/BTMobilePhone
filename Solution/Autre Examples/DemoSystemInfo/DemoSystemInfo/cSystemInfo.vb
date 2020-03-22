Option Strict On

Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Management
Imports Microsoft.Win32

Public Class cSystemInfo

#Region "Declarations"

    'Possible Office applications
    Enum MSOfficeApp
        Access_Application
        Excel_Application
        Outlook_Application
        PowerPoint_Application
        Word_Application
    End Enum

    'Possible Office versions    
    Enum Version
        Version95 = 7
        Version97 = 8
        Version2000 = 9
        Version2002 = 10
        Version2003 = 11
        Version2007 = 12
        Version2010 = 14
    End Enum

#End Region

#Region " - Battery - "

    Public Shared ReadOnly Property IsLaptop() As Boolean
        Get
            Static mgmtProc As ManagementClass
            If (mgmtProc Is Nothing) Then
                Try
                    mgmtProc = New ManagementClass("Win32_Battery")
                Catch ex As Exception
                    mgmtProc = Nothing
                End Try
            End If
            If (mgmtProc Is Nothing) Then Return False

            For Each objInstance As ManagementObject In mgmtProc.GetInstances
                Return True
            Next

            Return False
        End Get
    End Property

#End Region

#Region " --- Disks --- "

    Public Shared ReadOnly Property DiskSpace() As List(Of String)
        Get
            Dim arrInfo As New List(Of String)
            Dim Searcher As ManagementObjectSearcher
            Searcher = New ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk")
            For Each mgmtDisk As ManagementObject In Searcher.Get
                If Convert.ToString(mgmtDisk("MediaType")) = "12" Then
                    arrInfo.Add(
                            mgmtDisk.Properties("DeviceID").Value.ToString +
                            " Size=" + readableLength(Convert.ToInt64(mgmtDisk.Properties("Size").Value)) +
                            " Free=" + readableLength(Convert.ToInt64(mgmtDisk.Properties("FreeSpace").Value)))
                End If
            Next

            Return arrInfo
        End Get
    End Property

    'READABLE LENGTH
    Public Shared Function readableLength(ByVal length As Long) As String
        Dim suffix() As String = New String(3) {"b", "Kb", "Mb", "Gb"}
        Dim i As Integer = suffix.GetLowerBound(0)
        Do While (length > 1024) AndAlso (i < suffix.GetUpperBound(0))
            length \= 1024
            i += 1
        Loop
        Return length.ToString & suffix(i)
    End Function

#End Region ' --- Disks ---

#Region " - Framework - "

    Private Const FRAMEWORK_PATH As String = "\Microsoft.NET\Framework"
    Private Const WINDIR1 As String = "windir"
    Private Const WINDIR2 As String = "SystemRoot"

    Public Shared ReadOnly Property HighestFrameworkVersion() As String
        Get
            Try
                Return GetHighestVersion(NetFrameworkInstallationPath)
            Catch generatedExceptionName As Security.SecurityException
                Return "Unknown"
            End Try
        End Get
    End Property

    Public Shared ReadOnly Property ListFrameworkVersions() As List(Of String)
        Get
            Dim arrVersions As New List(Of String)
            Dim strVersion As String = "Unknown"

            For Each strX As String In Directory.GetDirectories(NetFrameworkInstallationPath, "v*")
                strVersion = ExtractVersion(strX)
                If PatternIsVersion(strVersion) Then
                    arrVersions.Add(strVersion)
                End If
            Next

            Return arrVersions
        End Get
    End Property

    Private Shared Function GetHighestVersion(ByVal pInstallationPath As String) As String
        Dim arrVersions As String() = Directory.GetDirectories(pInstallationPath, "v*")
        Dim strVersion As String = "Unknown"

        For i As Integer = arrVersions.Length - 1 To 0 Step -1
            strVersion = ExtractVersion(arrVersions(i))
            If PatternIsVersion(strVersion) Then Return strVersion
        Next

        Return strVersion
    End Function

    Private Shared Function ExtractVersion(ByVal pdirectory As String) As String
        Dim intStartIndex As Integer = pdirectory.LastIndexOf("\") + 2
        Return pdirectory.Substring(intStartIndex, pdirectory.Length - intStartIndex)
    End Function

    Private Shared Function PatternIsVersion(ByVal pVersion As String) As Boolean
        Return New Regex("[0-9](.[0-9]){0,3}").IsMatch(pVersion)
    End Function

    Public Shared ReadOnly Property NetFrameworkInstallationPath() As String
        Get
            Return WindowsPath + FRAMEWORK_PATH
        End Get
    End Property

    Public Shared ReadOnly Property WindowsPath() As String
        Get
            Dim strWinDir As String = Environment.GetEnvironmentVariable(WINDIR1)
            If String.IsNullOrEmpty(strWinDir) Then
                strWinDir = Environment.GetEnvironmentVariable(WINDIR2)
            End If

            Return strWinDir
        End Get
    End Property

#End Region ' - Framework - 

#Region " - Screens - "

    Public Shared ReadOnly Property Screens() As List(Of String)
        Get
            Dim arrInfo As New List(Of String)

            Dim intI As Integer = 0
            For Each objX As Screen In Screen.AllScreens
                intI += 1

                arrInfo.Add(String.Format("Screen {0} - Primary {1} - Bounds {2} - BitsPerPixel {3}", intI, objX.Primary, objX.Bounds.ToString, objX.BitsPerPixel.ToString))
            Next

            Return arrInfo
        End Get
    End Property

#End Region ' - Screens - 

#Region " - Office - "

    'ALL OFFICE VERSIONS
    Public Shared ReadOnly Property AllOfficeVersions() As List(Of String)
        Get
            Dim arrInfo As New List(Of String)
            For Each s As String In [Enum].GetNames(GetType(MSOfficeApp))
                arrInfo.Add(s.Replace("_Application", "") + "=" + GetVersionsString(CType([Enum].Parse(GetType(MSOfficeApp), s), MSOfficeApp)))
            Next

            Return arrInfo
        End Get
    End Property

    'GET VERSIONS STRING
    Public Shared Function GetVersionsString(ByVal app As MSOfficeApp) As String
        Try
            Dim strProgID As String = [Enum].GetName(GetType(MSOfficeApp), app)
            strProgID = strProgID.Replace("_", ".")
            Dim regKey As RegistryKey
            regKey = Registry.LocalMachine.OpenSubKey("Software\Classes\" & strProgID & "\CurVer", False)
            If regKey Is Nothing Then Return "No version detected."
            Dim strV As String = regKey.GetValue("", Nothing, RegistryValueOptions.None).ToString
            regKey.Close()
            strV = strV.Replace(strProgID, "").Replace(".", "")
            Return [Enum].GetName(GetType(Version), CInt(strV))
        Catch ex As Exception
            If Debugger.IsAttached Then Debugger.Break()
            Return String.Empty
        End Try
    End Function

#End Region ' - Office - 

#Region " --- WMI Properties --- "

    Public Shared ReadOnly Property NumberOfCPUsPhysical() As String
        Get
            Return "Physical = " + WMIComputerSystemProperties("NumberOfProcessors")
        End Get
    End Property

    Public Shared ReadOnly Property NumberOfCPUsLogical() As String
        Get
            Return "Logical = " + WMIComputerSystemProperties("NumberOfLogicalProcessors")
        End Get
    End Property

    Public Shared ReadOnly Property ClockSpeedMax() As String
        Get
            Return "Max = " + WMIProcessorProperties("MaxClockSpeed")
        End Get
    End Property

    Public Shared ReadOnly Property ClockSpeedCurrent() As String
        Get
            Return "Current = " + WMIProcessorProperties("MaxClockSpeed")
        End Get
    End Property

    Public Shared ReadOnly Property AddressWidth() As String
        Get
            Return WMIProcessorProperties("AddressWidth")
        End Get
    End Property

    Public Shared ReadOnly Property CPUManufacturer() As String
        Get
            Return WMIProcessorProperties("Manufacturer")
        End Get
    End Property

    Public Shared ReadOnly Property CPUName() As String
        Get
            Return WMIProcessorProperties("Name")
        End Get
    End Property

    Public Shared ReadOnly Property CPUDescription() As String
        Get
            Return WMIProcessorProperties("Description")
        End Get
    End Property

    Public Shared ReadOnly Property WMIProcessorProperties(ByVal pProperties As String) As String
        Get
            Dim strInfo As String = String.Empty

            Static mgmtProc As ManagementClass
            If (mgmtProc Is Nothing) Then
                Try
                    mgmtProc = New ManagementClass("Win32_Processor")
                Catch ex As Exception
                    mgmtProc = Nothing
                End Try
            End If
            If (mgmtProc Is Nothing) Then Return String.Empty

            For Each objInstance As ManagementObject In mgmtProc.GetInstances
                Dim strValue As String = String.Empty
                Try
                    strValue = objInstance.Properties(pProperties).Value.ToString()
                Catch ex As Exception
                    strValue = String.Empty
                End Try

                If (Not String.IsNullOrEmpty(strValue)) AndAlso (Not strInfo.Contains(strValue)) Then
                    If Not String.IsNullOrEmpty(strInfo) Then strInfo += ", "
                    strInfo += strValue
                End If
            Next

            Return strInfo
        End Get
    End Property

    Public Shared ReadOnly Property WMIComputerSystemProperties(ByVal pProperties As String) As String
        Get
            Dim strInfo As String = String.Empty

            Static mgmtCS As ManagementClass
            If (mgmtCS Is Nothing) Then
                Try
                    mgmtCS = New ManagementClass("Win32_ComputerSystem")
                Catch ex As Exception
                    mgmtCS = Nothing
                End Try
            End If
            If (mgmtCS Is Nothing) Then Return String.Empty

            For Each objInstance As ManagementObject In mgmtCS.GetInstances
                Dim strValue As String = String.Empty
                Try
                    strValue = objInstance.Properties(pProperties).Value.ToString()
                Catch ex As Exception
                    strValue = String.Empty
                End Try

                If (Not String.IsNullOrEmpty(strValue)) AndAlso (Not strInfo.Contains(strValue)) Then
                    If Not String.IsNullOrEmpty(strInfo) Then strInfo += ", "
                    strInfo += strValue
                End If
            Next

            Return strInfo
        End Get
    End Property

#End Region ' --- WMI Properties ---

End Class
