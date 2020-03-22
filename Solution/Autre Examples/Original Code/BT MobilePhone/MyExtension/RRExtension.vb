Option Strict Off
Option Explicit On
Imports System.Text
Imports System.Threading

'*****************************************************************
' Every Plugin MUST have its OWN set of GUIDs. use Tools->Create GUID
'*****************************************************************

<ComClass("A78A338C-9C9A-4726-990F-DAF3112D9101", "34C59AAD-59AE-4E50-9C23-A74051547252")> _
Public Class RRExtension
    Dim RunOnce As Boolean ' set to prevent a double execution of code
    Dim SDK As RRSDK ' set type of var to the subclass
    Private WithEvents bluetooth As BT
    Private dialNumber As String

    '*****************************************************************
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

    Public Sub phoneRinging() Handles bluetooth.Ringing
        'Show Call Screen
        SDK.Execute("MENU;MOBILECALL.skin")
        'SDK.Execute(("Playwav;C:\Program Files (x86)\Road Runner\button2.wav"))
        'Pause Audio
        SDK.Execute("PAUSE", True)
    End Sub

    Public Sub phoneHungup() Handles bluetooth.Hungup
        'Remove Call Screen
        SDK.Execute("EXIT", True)
        'Unpause Audio
        Thread.Sleep(100)
        SDK.Execute("RESUME", True)
    End Sub

    Public Sub voiceActivation() Handles bluetooth.VoiceActivation
        SDK.Execute("PAUSE", True)
    End Sub

    Public Sub voiceDeactivation() Handles bluetooth.VoiceDeactivation
        Thread.Sleep(100)
        SDK.Execute("RESUME", True)
    End Sub

    '*****************************************************************
    '* This sub is called when pluginmgr;about command is used
    '*
    '*****************************************************************
    Public Sub About(ByRef frm As Object)

        MsgBox("Created By Asela Fernando (Fox_Mulder)", vbOKOnly, "About")

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
        dialNumber = ""
        bluetooth = New BT()
        SDK.Execute("PRELOAD;MOBILECALL.skin")
        '
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

        'If Directory.Exists(pluginDataPath) = False Then Directory.CreateDirectory(pluginDataPath)

    End Sub

    '*****************************************************************
    '* This sub is called on unload of plugin by RR
    '*
    '*****************************************************************
    Public Sub Terminate()
        bluetooth.unload()
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
                Properties = "v1.0.0"
            Case "author"
                Properties = "Asela Fernando (Fox_Mulder)"
            Case "category"
                Properties = "Phone"
            Case "description"
                Properties = "Mobilephone Bluetooth Plugin"
            Case "supporturl"
                Properties = "http://www.mp3car.com/vbulletin"
        End Select

    End Function

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

        Select Case LCase(CMD)
            Case "mobilephone_dial"
                bluetooth.dial(dialNumber)
                ProcessCommand = 2
            Case "mobilephone_pickup"
                bluetooth.answerCall()
                ProcessCommand = 2
            Case "mobilephone_hangup"
                bluetooth.hangupCall()
                ProcessCommand = 2
            Case "mobilephone_transaudio"
                ProcessCommand = 2
            Case "mobilephone_1"
                dialNumber += "1"
                ProcessCommand = 2
            Case "mobilephone_2"
                dialNumber += "2"
                ProcessCommand = 2
            Case "mobilephone_3"
                dialNumber += "3"
                ProcessCommand = 2
            Case "mobilephone_4"
                dialNumber += "4"
                ProcessCommand = 2
            Case "mobilephone_5"
                dialNumber += "5"
                ProcessCommand = 2
            Case "mobilephone_6"
                dialNumber += "6"
                ProcessCommand = 2
            Case "mobilephone_7"
                dialNumber += "7"
                ProcessCommand = 2
            Case "mobilephone_8"
                dialNumber += "8"
                ProcessCommand = 2
            Case "mobilephone_9"
                dialNumber += "9"
                ProcessCommand = 2
            Case "mobilephone_0"
                dialNumber += "0"
                ProcessCommand = 2
            Case "mobilephone_*"
                dialNumber += "+"
                ProcessCommand = 2
            Case "mobilephone_#"
                dialNumber += "#"
                ProcessCommand = 2
            Case "mobilephone_del"
                dialNumber = dialNumber.Remove(dialNumber.Length - 1)
            Case "mobilephone_pc"
                'Contact Select
                ProcessCommand = 2
            Case "mobilephone_siri"
                bluetooth.Siri()
                ProcessCommand = 2

                'Case "mycommand"
                'This example shows a message on screen (never recommended)
                'MsgBox("MYCOMMAND was executed from " & frm.Tag + " !", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Extension Plugin")
                'ProcessCommand = 2 'Command completed

                'Specify whatever and whichever commands you wish to create
                'You can add as many as you'd like, and you can process complex commands as long
                'as you parse them yourself (i.e. "mycomplexcommand;parameter")

                'Case "mycommand2"
                'Insert Command code here

        End Select

    End Function

    '*****************************************************************
    '* This Function will be called with a requested label code and
    '* format specified at the skin file. Simply return any text to
    '* be displayed for the specified format.
    '*****************************************************************
    Public Function ReturnLabel(ByRef LBL As String, ByRef FMT As String) As String

        ReturnLabel = ""
        Select Case LCase(LBL)
            Case "mobilephone_dialbox"
                ReturnLabel = dialNumber
            Case "mobilephone_networkname"
                ReturnLabel = bluetooth.networkOperator
            Case "mobilephone_model"
                ReturnLabel = bluetooth.phoneModelName
            Case "mobilephone_manufacturer"
                ReturnLabel = bluetooth.phoneManufacturerName
            Case "mobilephone_callerid"
                ReturnLabel = bluetooth.callerID
            Case "mobilephone_batterystrength"
                ReturnLabel = Math.Truncate(bluetooth.batteryLevel * 65535 / 5).ToString()
            Case "mobilephone_signalstrength"
                ReturnLabel = Math.Truncate(bluetooth.signalStrength * 65535 / 31).ToString()
        End Select

    End Function

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
            Case "mobilephone_networkavailable"
                ReturnIndicatorEx = bluetooth.networkAvailable
            Case "mobilephone_connected"
                ReturnIndicatorEx = bluetooth.phoneConnection

                'Specify whatever and whichever indicators you wish to create
                'You can add as many as you'd like, and you can process complex indicators as long
                'as you parse them yourself (i.e. "mycomplexindicator;parameter")

                'Case "myindicator2"
                'Insert Code here to return "True", "False" or Path name

        End Select

    End Function

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
            'Case "myindicator"
            'Displays message box
            '    MsgBox("MYINDICATOR has been clicked!")

        End Select

    End Sub

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
            Case "mobilephone_batterystrength"
                ReturnSlider = Math.Truncate(bluetooth.batteryLevel * 65535 / 5)
            Case "mobilephone_signalstrength"
                ReturnSlider = Math.Truncate(bluetooth.signalStrength * 65535 / 31)

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
            'Case "myslider"
            '   MsgBox("Myslider Clicked to set value to:" & CStr(Value) & " Direction: " + IIf(Direction, "UP", "DOWN"))

            'Case "myslider2"
            'Insert code to process/set slider value here

        End Select

    End Sub

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
End Class