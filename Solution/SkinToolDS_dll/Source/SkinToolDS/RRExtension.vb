Option Strict Off
Option Explicit On

Imports System
Imports System.Text
Imports System.IO
Imports System.IO.File
Imports System.IO.Ports
Imports System.Threading
Imports System.Reflection
Imports System.Runtime.InteropServices

'*****************************************************************
' Every Plugin MUST have its OWN set of GUIDs. use Tools->Create GUID
'*****************************************************************
<ComClass("E68EB322-4A13-46C3-90FB-C394CD84E830", "26332CE1-1381-4E92-98B9-CB8A26FF8043")> _
Public Class RRExtension

    Dim RunOnce As Boolean ' set to prevent a double execution of code
    Friend SDK As RRSDK ' set type of var to the subclass

    Dim MainPath As String
    Public timer1 As System.Timers.Timer 'update all info each (3s)

    Dim DebugLogOnPause As Boolean = False
    '*******************************************************
    '* This is an interface to add commands/labels/indicators/sliders
    '* to RideRunner without needing a whole new application for such.
    '*
    '* You can monitor commands executed in RR by checking the CMD
    '* paramter of ProcessCommand and similarly monitor labels and
    '* indicators of the current screen. The idea is so you can create
    '* new commands, labels, indicators and sliders without having
    '* to re-compile or understand the code in RideRunner.
    '*
    '* Furthermore, it should be possible to intercept commands and
    '* modify them to your interst, say "AUDIO" to "LOAD;MYAUDIO.SKIN"
    '* for this all you need to do is modify CMD and return 3 on the
    '* processcommand call so that RR executes the command you return.
    '*
    '* You're free to use this code in any way you see fit.
    '*
    '*****************************************************************

    '*****************************************************************
    '* This sub is called when pluginmgr;about command is used
    '*
    '*****************************************************************
    Public Sub About(ByRef frm As Object)
        MsgBox("RideRunner SkinToolDS Plugin", vbOKOnly, "By pierrotm777")
    End Sub

    '*****************************************************************
    '* This sub is called when pluginmgr;settings command is used
    '*
    '*****************************************************************
    Public Sub Settings(ByRef frm As Object)

    End Sub

    '*****************************************************************
    '* This function is called immediatly after plugin is loaded and
    '* when ever RR is changing the plugin status (enabled/disabled)
    '* when True its enabled, False its disabled
    '* calls to the SDK methods should not be made when the plugin is
    '* disabled. When plugin is DISABLED no calls into the plugin will
    '* be made. This is all handled by the Sub-Class I have created
    '* (RRSDK.cls)
    '*
    '*****************************************************************
    Public Sub Enabled(ByRef state As Boolean)

        ' set sub class state, which will handle all processing to the
        ' real RR SDK
        SDK.Enabled = state

    End Sub

    '*****************************************************************
    '* This sub is called immediatly after plugin is loaded and
    '* enabled, its only called once.
    '* pluginDataPath = contains where the plugin should store any of
    '* its WRITEABLE\SETTINGS data to.
    '*
    '* NOTE: The plugin is required to create this directory if needed.
    '*
    '*****************************************************************
    Public Sub Initialize(ByRef pluginDataPath As String)

        On Error Resume Next

        ' pluginDataPath will contain a USER Profile (my documents) folder path
        ' suitable for storing WRITEABLE settings to
        ' this would make your plugin OS compliant (VISTA and onward)
        ' not to mention, its proper programming, user data should NOT be stored in "Program Files"
        '
        ' example (typical vista): "C:\Users\Username\Documents\RideRunner\Plugins\MyPlugin\"
        '
        ' App.path will be the path of the ACTUALL LOADED .dll (not recomend for any writes)
        '
        ' uncomment code below if u need the directory
        '

        If Directory.Exists(pluginDataPath) = False Then Directory.CreateDirectory(pluginDataPath)
        MainPath = pluginDataPath

        'create a timer with a three seconds interval.
        If IsNothing(timer1) = True Then
            timer1 = New System.Timers.Timer()
            timer1.Interval = 3000
            ' Hook up the Elapsed event for the timer.
            AddHandler timer1.Elapsed, AddressOf UpdateInfos
            ' Have the timer fire repeated events (true is the default)
            timer1.AutoReset = True
            timer1.Enabled = False
        End If
    End Sub

    '*****************************************************************
    '* This sub is called on unload of plugin by RR
    '*
    '*****************************************************************
    Public Sub Terminate()

    End Sub

    '*****************************************************************
    '* This function provides the metadata
    '*
    '* a string containing a "item" is passed into the function
    '*
    '*
    '*****************************************************************
    Public Function Properties(ByRef item As String) As String

        Properties = ""
        Select Case item
            Case "version"
                Properties = Assembly.GetExecutingAssembly().GetName().Version.ToString()
            Case "author"
                Properties = "pierrotm777"
            Case "category"
                Properties = "Interface for DS"
            Case "description"
                Properties = "SkinTool for DS"
            Case "supporturl"
                Properties = "pierrotm777@gmail.com"
            Case "menuitem"
                Properties = Chr(34) + "SKINTOOLDS" + Chr(34) + ",SKINTOOL,Icons\SkinTool.png,SkinTool,SkinTool is selected"
        End Select

    End Function

#Region "ProcessCommand"
    '*****************************************************************
    '* This Function will be called with the current command string
    '* The return parameter of this function determines the handling
    '* To be taken upon returning to RR:
    '*
    '* 0 = Command not processed here
    '* 1 = Command completed + return to previous screen
    '* 2 = Command completed, stay on current screen
    '* 3 = Command has been changed/modified, execute returned one
    '*
    '* frm is the form object which generated the current command. Be
    '* VERY VERY careful when using it.
    '*
    '* frm.tag contains the screen name for the same screen.
    '* you can poll other propperties/methods from the screen but you
    '* will need to look at RR's frmskin to know what you can use/do.
    '*****************************************************************
    Public Function ProcessCommand(ByRef CMD As String, ByRef frm As Object) As Short
        If CMD.Length = 0 Then Exit Function


        Select Case LCase(CMD)
            Case "runlog"
                DebugLogOnPause = True
                ProcessCommand = 2

            Case "pauselog"
                DebugLogOnPause = False
                ProcessCommand = 2

            Case Else
                If CheckIfRunning("skintoolds") = True Then
                    'If CMD <> "" Then
                    Dim sendCmd As String = CMD
                    Dim t As New Threading.Thread(Sub() SendToSkinToolDS(sendCmd))
                    t.Start()
                    ProcessCommand = 2
                    'End If
                End If
        End Select


    End Function

    Private Sub SendToSkinToolDS(cmdRR As String)

        ' Find the window with the name of the main form
        Dim ptrWnd As IntPtr = NativeMethods.FindWindow(Nothing, "SkinToolDS")
        Dim ptrWnd2 As IntPtr = NativeMethods.FindWindow(Nothing, "DebugForm")
        If ptrWnd = IntPtr.Zero And ptrWnd2 = IntPtr.Zero Then
            'MsgBox(String.Format("No window found with the title {0}.", "SkinToolDS or DebugForm"))
        ElseIf ptrWnd <> IntPtr.Zero Then
            Dim ptrCopyData As IntPtr = IntPtr.Zero
            Try
                ' Create the data structure and fill with data
                Dim copyData As New NativeMethods.COPYDATASTRUCT()
                copyData.dwData = New IntPtr(2)
                ' Just a number to identify the data type
                copyData.cbData = cmdRR.Length + 1
                ' One extra byte for the \0 character
                copyData.lpData = Marshal.StringToHGlobalAnsi(cmdRR)

                ' Allocate memory for the data and copy
                ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData))
                Marshal.StructureToPtr(copyData, ptrCopyData, False)

                ' Send the message
                NativeMethods.SendMessage(ptrWnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData)
            Catch ex As Exception
                'MsgBox(ex.ToString())
            Finally
                ' Free the allocated memory after the control has been returned
                If ptrCopyData <> IntPtr.Zero Then
                    Marshal.FreeCoTaskMem(ptrCopyData)
                End If
            End Try

        ElseIf ptrWnd2 <> IntPtr.Zero Then
            Dim ptrCopyData As IntPtr = IntPtr.Zero
            Try
                ' Create the data structure and fill with data
                Dim copyData As New NativeMethods.COPYDATASTRUCT()
                copyData.dwData = New IntPtr(2)
                ' Just a number to identify the data type
                copyData.cbData = cmdRR.Length + 1
                ' One extra byte for the \0 character
                copyData.lpData = Marshal.StringToHGlobalAnsi(cmdRR)

                ' Allocate memory for the data and copy
                ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData))
                Marshal.StructureToPtr(copyData, ptrCopyData, False)

                ' Send the message
                NativeMethods.SendMessage(ptrWnd2, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData)
            Catch ex As Exception
                'MsgBox(ex.ToString())
            Finally
                ' Free the allocated memory after the control has been returned
                If ptrCopyData <> IntPtr.Zero Then
                    Marshal.FreeCoTaskMem(ptrCopyData)
                End If
            End Try
        End If

    End Sub
#End Region

#Region "ReturnLabel"
    '*****************************************************************
    '* This Function will be called with a requested label code and
    '* format specified at the skin file. Simply return any text to
    '* be displayed for the specified format.
    '*****************************************************************
    Public Function ReturnLabel(ByRef LBL As String, ByRef FMT As String) As String
        ReturnLabel = ""
        Select Case LCase(LBL)
            Case "skintoolds_plugindesc"
                ReturnLabel = "SkinToolDS Interface"
            Case "skintoolds_pluginver"
                ReturnLabel = Assembly.GetExecutingAssembly().GetName().Version.ToString()

        End Select

    End Function
#End Region

#Region "ReturnIndicatorEx"
    '*****************************************************************
    '* This Function will be called with requested indicator code
    '* specified at the skin file. Simply return "True" or "False" to
    '* displayed the respective ON or OFF layer of the skin images.
    '* alternatively you can specify a path to a file to be displayed
    '* as the indicator specified. Return "False" to erase the image.
    '* ONLY return something else IF AND ONLY IF you process that code
    '*****************************************************************
    Public Function ReturnIndicatorEx(ByRef IND As String) As String
        'Default (No Action)
        'ONLY return something else IF AND ONLY IF you process that code
        ReturnIndicatorEx = ""

        Select Case LCase(IND)
            'Case "netinfo_maximumreceived" 'clignote si les 2/3 de TempPluginSettings.MaximumDataValue est atteint


        End Select


    End Function
#End Region

#Region "IndicatorClick"
    '*****************************************************************
    '* This Sub will be called with an indicator code "CLICKED"
    '* specified at the skin file. This "event" so to speak can be used
    '* to toggle indicators or execute any code you desire when clicking
    '* on a specifig indicator in the skin. You can also modify IND and
    '* monitor the IND parameter as to detect/alter the behaviour of
    '* how RR will process the indicator code being clicked.
    '*****************************************************************
    Public Sub IndicatorClick(ByRef IND As String)
        'If one of our indicators
        Select Case LCase(IND)
            'Timer Designer


        End Select

    End Sub
#End Region

#Region "Sliders"
    '*****************************************************************
    '* This Function will be called with requested slider code
    '* specified at the skin file. Simply return the value of the
    '* slider to be displayed. Values should range from 0 to 65536.
    '* It is also possible to intercept/change the slider code before
    '* it is processed in RideRunner (to overwrite existing codes).
    '*****************************************************************
    Public Function ReturnSlider(ByRef SLD As String) As Integer

        'This tells RR that the Slider was not processed in this plugin
        ReturnSlider = -1

        Select Case LCase(SLD)

        End Select




    End Function


    '*****************************************************************
    '* This Function will be called with requested slider code
    '* specified at the skin file. Simply return the value of the
    '* slider to be displayed. Values should range from 0 to 65536.
    '* It is also possible to intercept/change the slider code before
    '* it is processed in RideRunner (to overwrite existing codes).
    '*****************************************************************
    Public Sub SetSlider(ByRef SLD As String, ByRef Value As Integer, ByRef Direction As Boolean)

        Select Case LCase(SLD)

        End Select




    End Sub

#End Region

    Public Sub New()
        MyBase.New()

        If RunOnce = False Then ' only want to do once
            RunOnce = True
            'Code here is executed when loading the Extension plugin
            ' set RRSDK (this is the sub class)
            SDK = New RRSDK

            ' run any one time code here

        End If

    End Sub

    Private Sub UpdateInfos()

    End Sub

    Private Function CheckIfRunning(sProcessName As String) As Boolean
        Dim bRet As Boolean = False
        Try
            Dim listProc() As System.Diagnostics.Process
            listProc = System.Diagnostics.Process.GetProcessesByName(sProcessName)
            If listProc.Length > 0 Then
                ' Process is running
                bRet = True
            Else
                ' Process is not running
                bRet = False
            End If
        Catch ex As Exception

        End Try
        Return bRet
    End Function


    '#########################################################################################

End Class