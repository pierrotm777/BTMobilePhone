Option Strict Off
Option Explicit On

Imports System.Threading

Imports System.Speech
Imports System.Speech.Synthesis
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Reflection

Imports System.Text
Imports MobilePhone.PhoneNumberFormatter

'Imports PhoneNumbers
'Imports NUnit.Framework
'*****************************************************************
' Every Plugin MUST have its OWN set of GUIDs. use Tools->Create GUID
'*****************************************************************

<ComClass("E99D1ADB-E87B-4921-BA7F-86A8869E9BEB", "1D06FA29-0866-491B-97CD-7DCF15BB9C42")> _
Public Class RRExtension
    Dim RunOnce As Boolean ' set to prevent a double execution of code
    Dim SDK As RRSDK ' set type of var to the subclass
    Dim WithEvents bluetooth As BT
    Dim WithEvents speechrecognition As ClassSpeechRecognition

    'Dim myphonebook As phoneBook
    'Dim sysFolderPath As String
    'Dim PhoneConnectedStatus As Boolean = False

    Dim IconPath() As String
    Dim ReturnedLineNumberForString As Integer
    Dim DebuglogPath As String

    Private Event MicrophoneRecordingStart()       'Raises an event when the record start
    Private Event MicrophoneRecordingStopSave()       'Raises an event when the record start
    Dim recording As Boolean = False

    Dim WithEvents SearchPhoneTimer1 As System.Timers.Timer

    Dim GPS = CreateObject("RideRunner.GPS")
    'LOCK = GPS.Valid -- Is the information on the GPS Valid ? True/False (Has a lock)
    'LAT = GPS.Lat -- Parsed GPS Latitude (double)
    'LON = GPS.Lon -- Parsed GPS Longitude (double)
    'HDG = GPS.Hdg -- Parsed GPS Heading (double)
    'ALT = GPS.Alt -- Parsed GPS Altitude (double)
    'SPD = GPS.Speed -- Parsed GPS Speed (double)
    'SATS = GPS.Sats -- Parsed GPS number of sattelites in use (integer)
    'N = GPS.Clients -- Number of Applications connected to GPS, including caller (integer)
    'GPRMC = GPS.NMEA("$GPRMC") -- Most recent $GPRMC nmea message (works with all $XXXXX messages)
    'GPGGA = GPS.NMEA("$GPGGA") -- Most recent $GPGGA nmea message (works with all $XXXXX messages)
    'It is also possible to close/open the GPS port for internal use, though you should be mindful of other application using the GPS (including RR) -- thus why the number of clients connected to the GPS.
    'GPS.ClosePort -- Closes GPS Port (so it is available for other applications outside RR)
    'GPS.OpenPort -- Re-Opens GPS Port (after being closed by ClosePort)


    'Windows 10 check
    Dim osVer As System.OperatingSystem = System.Environment.OSVersion
    'If osVer.Version.Major = 6 AndAlso osVer.Version.Minor = 1 Then
    '    'running Windows 7 or Windows 2008 R2
    'End If

    Dim en As New CultureInfo("en-US")

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

#Region "RR Events"

    Public Sub BlinkIndicatorOn() Handles bluetooth.SetIndBlinkOn
        '"SetIndBlink;MUTE;true"             - adds ":b" from indicator MUTE on current screen.
        '"SetIndBlink;MUTE;false"            - del ":b" from indicator MUTE on current screen.
        SDK.Execute("SetIndBlink;mobilephone_synchronizing;true", True)
    End Sub
    Public Sub BlinkIndicatorOff() Handles bluetooth.SetIndBlinkOff
        '"SetIndBlink;MUTE;true"             - adds ":b" from indicator MUTE on current screen.
        '"SetIndBlink;MUTE;false"            - del ":b" from indicator MUTE on current screen.
        SDK.Execute("SetIndBlink;mobilephone_synchronizing;false", True)
    End Sub
    Public Sub phoneNetworkAvail() Handles bluetooth.PhoneNetAvail
        SDK.Execute("*ONMOBILEPHONENETAVAIL", True)
    End Sub
    Public Sub phoneNetworkUnAvail() Handles bluetooth.PhoneNetUnAvail
        SDK.Execute("*ONMOBILEPHONENETUNAVAIL", True)
    End Sub
    Public Sub phoneIsConnected() Handles bluetooth.PhoneIsConnected
        SDK.Execute("*ONMOBILEPHONECONNECTED", True)
    End Sub
    Public Sub phoneIsDisConnected() Handles bluetooth.PhoneIsDisConnected
        SDK.Execute("*ONMOBILEPHONEDISCONNECTED", True)
    End Sub
    Public Sub phoneInCall() Handles bluetooth.PhoneInCall
        SDK.Execute("*ONMOBILEPHONEINCALL", True)
    End Sub

    Public Sub phoneRinging() Handles bluetooth.Ringing
        'Show Call Screen
        'MsgBox(bluetooth.callerID)
        CallerID = bluetooth.callerID
        SDK.SetUserVar("MOBILEPHONE_PHOTOPATH", "")
        CallerID = IIf(CallerID.StartsWith("+"), CallerID.Replace("+", ""), CallerID)
        CallerName = "Unknow"
        SDK.SetUserVar("NEWCONTACT", CallerName)

        If File.Exists(MainPath & "PhoneBook\MobilePhone_MT.txt") AndAlso bluetooth.CheckIfContactIsInList(MainPath & "PhoneBook\MobilePhone_MT.txt", bluetooth.callerID) Then
            CallerName = ContactNameFromNumber(MainPath & "PhoneBook\MobilePhone_MT.txt", CallerID)
            For Each Extension In allowedExtensions
                If File.Exists(sPhotoPath & bluetooth.callerID & "." & Extension) Then
                    SDK.Execute("MENU;MOBILECALL.skin||SETVAR;MOBILEPHONE_PHOTOPATH;" & sPhotoPath & bluetooth.callerID & "." & Extension)
                Else
                    SDK.Execute("MENU;MOBILECALL.skin")
                End If
            Next
        Else
            SDK.Execute("MENU;MOBILECALL.skin||SETVAR;MOBILEPHONE_PHOTOPATH;")
            CallerName = "Unknow"
        End If
        'Pause Audio
        SDK.Execute("PAUSE||DVBMUTE;1||*ONMOBILEPHONERINGING", True)
    End Sub
    Public Sub phoneHungup() Handles bluetooth.Hungup
        'Remove Call Screen
        If LCase(SDK.GetInfo("RRSCREEN")) = "mobilecall.skin" Then
            SDK.Execute("ESC", True)
        End If

        'For test only
        'CallerID = "0671xxyyzz" 
        'CallerName = "Unknow"
        'SDK.SetUserVar("NEWCONTACT", CallerName) 
        'For test only
        Thread.Sleep(500)
        'if last contact isn't in contact's list we can save or not this new contact into the phone
        If CallerID <> "" Then
            If Not bluetooth.CheckIfContactIsInList(MainPath & "PhoneBook\MobilePhone_MT.txt", CallerID) Then
                SDK.Execute("MENU;MOBILEPHONE_ADDCONTACTTOPHONE.SKIN", True)
                'Else 'For test only
                '    MessageBox.Show("Contact already exist !")
            End If
        End If

        'Unpause Audio
        Thread.Sleep(100)
        SDK.Execute("RESUME||DVBMUTE;0||*ONMOBILEPHONEHUNGUP", True)
    End Sub

    Public Sub voiceActivation() Handles bluetooth.VoiceActivation
        SDK.Execute("PAUSE||*ONMOBILEPHONEVOICEON", True)
    End Sub
    Public Sub voiceDeactivation() Handles bluetooth.VoiceDeactivation
        Thread.Sleep(100)
        SDK.Execute("RESUME||*ONMOBILEPHONEVOICEOFF", True)
    End Sub

    Public Sub PhoneInCharge() Handles bluetooth.PhoneInCharge
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONEBATTERYONCHARGE", True)
    End Sub
    Public Sub battisFullCharge() Handles bluetooth.BatteryIsFull
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONEBATTERYFULLCHARGE", True)
    End Sub

    'emergency call number event
    Public Sub alarmEmergency() Handles bluetooth.EmergencyAlarm
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONEEMERGENCY", True)
    End Sub

    'external power alarm events
    Public Sub alarmExternalPowerOn() Handles bluetooth.ExtPowerBattON
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONEEXTPOWERON", True)
    End Sub
    Public Sub alarmExternalPowerOff() Handles bluetooth.ExtPowerBattOFF
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONEEXTPOWEROFF", True)
    End Sub

    'first or second selected phones events
    Public Sub firstPhoneNotFound() Handles bluetooth.FirstPhoneNotFound
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONE1NOTFOUND", True)
    End Sub
    Public Sub secondPhoneNotFound() Handles bluetooth.SecondPhoneNotFound
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONE2NOTFOUND", True)
    End Sub

    'PBAPIsReady (phone books)
    Public Sub pbapIsReady() Handles bluetooth.PBAPIsReady
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONEPBAPISREADY", True)
    End Sub

    '(SMS)
    Public Sub mapIsReady() Handles bluetooth.MAPIsReady
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONEMAPISREADY", True)
    End Sub
    Public Sub smsIsSend() Handles bluetooth.SMSIsSend
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONESMSISSEND", True)
    End Sub
    Public Sub smsIsReceived() Handles bluetooth.SMSIsReceived
        Thread.Sleep(100)
        If SmsIsReceivedStatus = True Then
            SDK.Execute("MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||SETVAR;MOBILEPHONE_INFO;NEW SMS||CLADD;A new SMS message||CLADD;is on the memory '" & NewSmsIsReceivedNumber & "'||*ONMOBILEPHONESMSISSEND", True)
        End If
    End Sub
    Public Sub smsIsDecoded() Handles bluetooth.SMSPDUIsDecoded
        Thread.Sleep(100)
        Dim SmsSplit() As String, FromDateTime() As String, NewImgPath As String, SmsName As String = ""
        SDK.Execute("CLCLEAR;ALL")
        SmsSplit = SmsToRead.Split(vbCrLf)
        For Each s In SmsSplit
            If s.Contains("From") Then
                s = s.Replace("From:", "").Replace("Time:", " ")
                FromDateTime = s.Split(" ")
                SDK.SetUserVar("MOBILEPHONE_SMSFROMNUMBER", "+" & FromDateTime(0).Trim)
                SDK.SetUserVar("MOBILEPHONE_SMSFROMDATETIME", FromDateTime(1).Trim)
            ElseIf s.Contains("SMS_RECEIVED") Then
                s = s.Replace("SMS_RECEIVED", "")
            ElseIf s.Contains("SMS_SUBMIT") Then
                s = s.Replace("SMS_SUBMIT", "")
            ElseIf s.Contains("EMS_RECEIVED") Then
                s = s.Replace("EMS_RECEIVED", "")
            ElseIf s.Contains("EMS_SUBMIT") Then
                s = s.Replace("EMS_SUBMIT", "")
                'ElseIf s.Contains(vbCrLf) Then
                '    s = s.Replace(vbCrLf, "|")
                '    SDK.SetUserVar("MOBILEPHONE_MESSAGE", Trim(s))
            Else
                If s <> "" Then SDK.Execute("CLADD;" & Trim(s))
                If s <> "" Then SDK.SetUserVar("MOBILEPHONE_MESSAGE", Trim(s))
            End If
        Next
        'check picture's path
        NewImgPath = sPhotoPath & SDK.GetUserVar("MOBILEPHONE_SMSFROMNUMBER") & "." & sPhotoExtension
        If File.Exists(NewImgPath) Then
            SDK.Execute("SETVAR;MOBILEPHONE_PHOTOPATH;" & NewImgPath)
        Else
            SDK.Execute("SETVAR;MOBILEPHONE_PHOTOPATH;" & MainPath & "Photo\unknow.jpg")
        End If

        'check if name of the sms number exist into phonebook
        'If MyPhoneCultureInfo = "33" Then 'MyPhoneCultureInfo '33
        '    SmsName = "0" & Mid(SDK.GetUserVar("MOBILEPHONE_SMSFROMNUMBER"), 4) 'efface le +33
        'End If

        If ContactNameFromNumber(MainPath & "PhoneBook\MobilePhone_MT.txt", SmsName) <> "" Then
            SDK.Execute("SETVAR;MOBILEPHONE_SMSNAME;" & ContactNameFromNumber(MainPath & "PhoneBook\MobilePhone_MT.txt", SmsName))
        Else
            SDK.Execute("SETVAR;MOBILEPHONE_SMSNAME;Unknow")
        End If
    End Sub

    'Speech Recognition events
    Public Sub speechLoad() Handles speechrecognition.MobilePhoneSeepchRecognitionIsON
        Thread.Sleep(100)
        SDK.Execute("PAUSE||*ONMOBILEPHONESPEECHLOAD", True)
    End Sub
    Public Sub speechUnload() Handles speechrecognition.MobilePhoneSeepchRecognitionIsOFF
        Thread.Sleep(100)
        SDK.Execute("RESUME||ONMOBILEPHONESPEECHUNLOAD", True)
    End Sub

    Public Sub speechDial() Handles speechrecognition.MobilePhoneSeepchDial
        Thread.Sleep(100)
        SDK.Execute("SETVAR;DIALNUMBER;" & dialNumber & "||SETVARBYCODE;MOBILEPHONE_CALLERID;DIALNUMBER||MOBILEPHONE_DIAL||*ONMOBILEPHONESPEECHDIAL", True)
    End Sub
    Public Sub speechHangup() Handles speechrecognition.MobilePhoneSeepchHangup
        'CallerName = ""
        Thread.Sleep(100)
        SDK.Execute("RESUME||MOBILEPHONE_HANGUP", True)
    End Sub

    Public Sub speechHelp() Handles speechrecognition.MobilePhoneSeepchHelp
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONESPEECHHELP", True)
    End Sub

    Public Sub speechDialByName() Handles speechrecognition.MobilePhoneSeepchDialByName
        Thread.Sleep(100)
        Dim tmp As String = speechrecognition.speechCallByNameResult
        tmp = NumberFromContactName(MainPath & "PhoneBook\MobilePhone_MT.txt", Replace(tmp, SpeechArray(18), ""))
        'MsgBox(tmp)
        SDK.Execute("SETVAR;DIALNUMBER;" & dialNumber & "||SETVARBYCODE;MOBILEPHONE_CALLERID;DIALNUMBER||MOBILEPHONE_DIAL||*ONMOBILEPHONESPEECHDIAL", True)
    End Sub

    Public Sub speechSupp1() Handles speechrecognition.MobilePhoneSeepchSupp1
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONESPEECHSUPP1", True)
    End Sub
    Public Sub speechSupp2() Handles speechrecognition.MobilePhoneSeepchSupp2
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONESPEECHSUPP2", True)
    End Sub
    Public Sub speechSupp3() Handles speechrecognition.MobilePhoneSeepchSupp3
        Thread.Sleep(100)
        SDK.Execute("*ONMOBILEPHONESPEECHSUPP3", True)
    End Sub
#End Region

    '*****************************************************************
    '* This sub is called when pluginmgr;about command is used
    '*
    '*****************************************************************
    Public Sub About(ByRef frm As Object)
        Dim result = MessageBox.Show("Created By Asela Fernando (Fox_Mulder)" & vbCrLf & vbCrLf & _
               "Modify by:" & vbCrLf & _
               "Cormac Champion for phonebook feature" & vbCrLf & _
               "Pierre Montet", "About (Bluesoleil RR Plugin)", _
                MessageBoxButtons.OK, _
                MessageBoxIcon.Asterisk)
        'MsgBox(, vbOKOnly, "About")
    End Sub

    '*****************************************************************
    '* This sub is called when pluginmgr;settings command is used
    '*
    '*****************************************************************
    Public Sub Settings(ByRef frm As Object)
        SDK.Execute("MOBILEPHONE_SETTINGS")
    End Sub

    '*****************************************************************
    '* This function is called immediatly after plugin is loaded and
    '* when ever RR is changing the plugin status (enabled/disabled)
    '* when True its enabled, False its disabled
    '* calls to the SDK methods should not be made when the plugin is
    '* disabled. When plugin is DISABLED no calls into the plugin will
    '* be made. This is all handled by the Sub-Class I have created
    '* (SDK.cls)
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

        If Directory.Exists(pluginDataPath) = False Then Directory.CreateDirectory(pluginDataPath)
        MainPath = pluginDataPath

        sPhotoPath = MainPath & "Photo\"
        sPhotoExtension = "jpg"
        sPhonebookpath = MainPath & "Phonebook\"
        dialNumber = ""
        DebuglogPath = MainPath & "mobilephone.log"

        '''''''''''''''''
        ''' Settings  '''
        '''''''''''''''''
        PluginSettings = New cMySettings(Path.Combine(pluginDataPath, "MobilePhone"))
        ' read in defaults
        cMySettings.DeseralizeFromXML(PluginSettings)

        ' copy to temp
        TempPluginSettings = New cMySettings(PluginSettings)


        SDK.Execute("PRELOAD;MOBILECALL.skin")
        SDK.Execute("PRELOAD;MOBILEPHONE.skin")
        If Not Directory.Exists(MainPath & "PhoneBook") Then
            Directory.CreateDirectory(MainPath & "PhoneBook")
        End If
        If Not Directory.Exists(MainPath & "Photo") Then
            Directory.CreateDirectory(MainPath & "Photo")
        End If

        MyCultureInfo = CultureInfo.CurrentCulture.EnglishName
        MyPhoneCultureInfo = CountryToPhoneIndicatif(MyCultureInfo)
        'check Culture phone number
        'If TempPluginSettings.PhoneCountryCodes(1) <> MyPhoneCultureInfo Then
        '    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
        '        cMySettings.Copy(TempPluginSettings, PluginSettings)
        '        cMySettings.SerializeToXML(PluginSettings)
        '    End If
        'End If

        ''Init timer that search phone each 5s and stop when a phone is found
        'SearchPhoneTimer1 = New System.Timers.Timer(5000)
        'AddHandler SearchPhoneTimer1.Elapsed, AddressOf OnTimedEvent
        'SearchPhoneTimer1.AutoReset = True
        'SearchPhoneTimer1.Enabled = False

        'Init BlueSoleil
        'InitAndSearchPhone()
        SDK.SetUserVar("PHONE", "TRY LOAD PHONE1")
        If BlueSoleil_IsInstalled() = True Then
            PhoneCheckedIs = 1 'recherche le premier mobile s'il est défini dans le .xml
            If TempPluginSettings.RunOnStart = True Then
                bluetooth = New BT
            End If


            'If TempPluginSettings.AutoSwapPhone = True Then
            '    If CBool(bluetooth.phoneConnection) = False Then
            '        Thread.Sleep(500)
            '        bluetooth.unload()
            '        SDK.SetUserVar("PHONE", "TRY LOAD PHONE2")
            '        PhoneCheckedIs = 2 'recherche le deuxième mobile s'il est défini dans le .xml
            '        bluetooth = New BT
            '    End If
            '    Thread.Sleep(10000)
            '    If CBool(bluetooth.phoneConnection) = False Then
            '        SearchPhoneTimer1.Enabled = True
            '    Else
            '        SearchPhoneTimer1.Enabled = False
            '    End If
            'End If

        Else
            SDK.Execute("WAIT;5||SetErrorScr;MobilePhone Error;Bluesoleil isn't Installed !!!;*****************************************;5")
        End If
    End Sub

    '*****************************************************************
    '* This sub is called on unload of plugin by RR
    '*
    '*****************************************************************
    Public Sub Terminate()
        bluetooth.unload()
        speechrecognition.unload()
        If debugFrm.Visible = True Then
            debugFrm.Hide()
            debugFrm.Dispose()
        End If
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
                Properties = Assembly.GetExecutingAssembly().GetName().Version.ToString() '"v1.1.5"
            Case "author"
                Properties = "Asela Fernando (Fox_Mulder) / Modified by pierrotm777"
            Case "category"
                Properties = "Phone"
            Case "description"
                Properties = "Mobilephone Bluetooth Plugin"
            Case "supporturl"
                Properties = "http://www.mp3car.com/rr-plugins/154943-bt-mobilephone-plugin.html#post1487622"
            Case "menuitem"
                Properties = Chr(34) + "MOBILEPHONE" + Chr(34) + ",PHONE,Icons\Phone.png,MobilePhone,MobilePhone is selected"
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

        Select Case LCase(CMD)
            'RR events
            Case "onidle"
                If LCase(frm.tag) = "mobilephone_info.skin" Then
                    SDK.Execute("CLCLEAR;ALL||CLLOAD;" & MainPath & "MobilePhone_DevicesList.lst")
                    SDK.SetUserVar("MOBILEPHONE_INFO", "Device's list !!!")
                End If
                ProcessCommand = 2
            Case "onsuspend"
                Terminate()
                ProcessCommand = 2
            Case "onresume"
                If CBool(bluetooth.phoneConnection) = True Then
                    Terminate()
                    bluetooth = New BT()
                Else
                    bluetooth = New BT()
                End If
                ProcessCommand = 2
            Case "onexit"
                Terminate()
                ProcessCommand = 2
                'Case "onclselclick" 'WHEN PHONE LIST IS CLICKED SET INFO LABEL
                '    ProcessCommand = 2
            Case "onclclick"
                updateListInfo()
                dialNumber = SDK.GetUserVar("LISTTEXT")
                ProcessCommand = 2
                'RR events

            Case "mobilephone"
                If SDK.GetInfo("mobilephone_phonestatus") = "Plugin is Off" Then
                    bluetooth = New BT()
                    SDK.Execute("LOAD;MOBILEPHONE.SKIN")
                Else
                    If BlueSoleil_IsInstalled() = True Then
                        If CBool(bluetooth.phoneConnection) = True Then
                            SDK.Execute("LOAD;MOBILEPHONE.SKIN")
                            If TempPluginSettings.LockInMotion = True AndAlso SDK.GetInd("pluginmgr;status;GpsLockout") = "True" Then
                                SDK.ErrScrn("MobilePhone Info", "!!! LOCK IN MOTION Is ACTIVE !!!", "Read or Write SMS will not be possible !", 5)
                            End If
                        Else
                            SDK.Execute("MOBILEPHONE_SETTINGS")
                        End If
                    Else
                        SDK.ErrScrn("MobilePhone Error", "Bluesoleil isn't found !!!", "*****************************************", 5)
                    End If
                End If

                ProcessCommand = 2

            Case "mobilephone_restart"
                bluetooth.unload()

            Case "mobilephone_connect"
                If CBool(bluetooth.phoneConnection) = False Then
                    bluetooth = New BT()
                Else
                    SDK.ErrScrn("MobilePhone Error", "Your phone is allready connected !", "*****************************************", 5)
                End If
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Connected ..." & vbCrLf)
                ProcessCommand = 2
            Case "mobilephone_disconnect"
                bluetooth.unload()
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Disconnected ..." & vbCrLf)
                ProcessCommand = 2

            Case "mobilephone_btstart"
                bluetooth.BlueSoleil_Btsdk_StartBluetooth()
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Phone is ON ..." & vbCrLf)
                ProcessCommand = 2
            Case "mobilephone_btstop"
                bluetooth.BlueSoleil_Btsdk_StopBluetooth()
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Phone is OFF ..." & vbCrLf)
                ProcessCommand = 2

            Case "mobilephone_transaudio"
                Dim BA As Boolean = bluetooth.AudioConnTrans()
                If debugFrm.Visible And BA Then debugFrm.debugTextBox.AppendText("Audio Trans is ON ..." & vbCrLf)
                If debugFrm.Visible And Not BA Then debugFrm.debugTextBox.AppendText("Audio Trans is OFF ..." & vbCrLf)
                ProcessCommand = 2

                'mute micro & speaker
            Case "mobilephone_mutemic"
                bluetooth.phonemicrovolMax(0)
                ProcessCommand = 2
            Case "mobilephone_mutespkr"
                bluetooth.phonespeakervolMax(0)
                ProcessCommand = 2
            Case "mobilephone_unmutemic"
                bluetooth.phonemicrovolMax(15)
                ProcessCommand = 2
            Case "mobilephone_unmutespkr"
                bluetooth.phonespeakervolMax(15)
                ProcessCommand = 2

                'TEXT INPUT COMMANDS
            Case "mobilephone_1"
                dialNumber += "1"
                bluetooth.dtmf("1")
                ProcessCommand = 2
            Case "mobilephone_2"
                dialNumber += "2"
                bluetooth.dtmf("2")
                ProcessCommand = 2
            Case "mobilephone_3"
                dialNumber += "3"
                bluetooth.dtmf("3")
                ProcessCommand = 2
            Case "mobilephone_4"
                dialNumber += "4"
                bluetooth.dtmf("4")
                ProcessCommand = 2
            Case "mobilephone_5"
                dialNumber += "5"
                bluetooth.dtmf("5")
                ProcessCommand = 2
            Case "mobilephone_6"
                dialNumber += "6"
                bluetooth.dtmf("6")
                ProcessCommand = 2
            Case "mobilephone_7"
                dialNumber += "7"
                bluetooth.dtmf("7")
                ProcessCommand = 2
            Case "mobilephone_8"
                dialNumber += "8"
                bluetooth.dtmf("8")
                ProcessCommand = 2
            Case "mobilephone_9"
                dialNumber += "9"
                bluetooth.dtmf("9")
                ProcessCommand = 2
            Case "mobilephone_0"
                dialNumber += "0"
                bluetooth.dtmf("0")
                ProcessCommand = 2
            Case "mobilephone_*"
                dialNumber += "*"
                bluetooth.dtmf("*")
                ProcessCommand = 2
            Case "mobilephone_+"
                dialNumber += "+"
                ProcessCommand = 2
            Case "mobilephone_#"
                dialNumber += "#"
                bluetooth.dtmf("#")
                ProcessCommand = 2
            Case "mobilephone_del"
                dialNumber = dialNumber.Remove(dialNumber.Length - 1)
                ProcessCommand = 2
            Case "mobilephone_clear"
                dialNumber = ""
                ProcessCommand = 2

            Case "mobilephone_addcountryprefix"
                dialNumber = ""
                Dim splitdialNumber() As String = SDK.GetUserVar("LISTTEXT").Replace(" ", "").Replace("-->", ",").Split(",")
                dialNumber += "+" & splitdialNumber(1)
                SDK.Execute("ESC")
                ProcessCommand = 2

                'DIALING COMMANDS
            Case "mobilephone_dial"
                bluetooth.dial(dialNumber)
                ProcessCommand = 2
            Case "mobilephone_emergency"
                bluetooth.dial(TempPluginSettings.EmergencyNumber)
                ProcessCommand = 2
            Case "mobilephone_pickup"
                bluetooth.answerCall()
                IncomingCall = False
                ProcessCommand = 2
            Case "mobilephone_hangup"
                bluetooth.hangupCall()
                ' We just made a call, so update the received, missed or dialed phonebooks
                'A MODIFIER POUR PB ICH OCH MCH et CCH
                'workerThread = New Threading.Thread(AddressOf bluetooth.MobilePhoneSyncProcess)
                'workerThread.Start()
                ProcessCommand = 2
            Case "mobilephone_redial"
                'redial last call
                bluetooth.BlueSoleil_HandsFree_Redial()
                ProcessCommand = 2

                'record, stop/save, play
            Case "mobilephone_record"
                If recording = False Then
                    RaiseEvent MicrophoneRecordingStart()
                Else
                    RaiseEvent MicrophoneRecordingStopSave()
                End If
                ProcessCommand = 2

                'PHONEBOOK NAVIGATION COMMANDS
            Case "MobilePhone_MT_first_entry"
                SDK.Execute("GOTOFIRST||ONCLCLICK")
                ProcessCommand = 2
                'Case "MobilePhone_MT_previous_page"
            Case "MobilePhone_MT_previous_entry"
                SDK.Execute("CTRLUP||ONCLCLICK")
                ProcessCommand = 2
            Case "MobilePhone_MT_next_entry"
                SDK.Execute("CTRLDOWN||ONCLCLICK")
                ProcessCommand = 2
                'Case "MobilePhone_MT_next_page"
            Case "MobilePhone_MT_last_entry"
                SDK.Execute("GOTOLAST||ONCLCLICK")
                ProcessCommand = 2



                'PHONEBOOK MANAGEMENT COMMANDS
            Case "mobilephone_updatecl"
                If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_contact_edit.skin" Then
                    If SDK.GetInfo("CLIMG") <> "" Then
                        SDK.Execute("SETVARFROMVAR;NEWNAME;LISTDESC||SETVARFROMVAR;NEWNUMBER;LISTTEXT||SETVARBYCODE;NEWIMG;CLIMG")
                    Else
                        SDK.Execute("SETVARFROMVAR;NEWNAME;LISTDESC||SETVARFROMVAR;NEWNUMBER;LISTTEXT||SETVAR;NEWIMG;" & MainPath & "Photo\unknow.gif")
                    End If
                ElseIf LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_contacts.skin" Then
                    dialNumber = SDK.GetInfo("CLTEXT")
                    'SDK.Execute("SETVARFROMVAR;MOBILEPHONE_DIALBOX;LISTTEXT")
                End If
                ProcessCommand = 2
            Case "mobilephone_add_entry"
                'SDK.Execute("LOAD;MOBILEPHONE_CONTACT_EDIT.SKIN")
                If SDK.GetUserVar("NEWNUMBER") <> "" Then
                    If File.Exists(MainPath & "PhoneBook\MobilePhone_MT.txt") AndAlso bluetooth.CheckIfContactIsInList(MainPath & "PhoneBook\MobilePhone_MT.txt", SDK.GetUserVar("NEWNAME")) Then
                        SDK.Execute("CLADD;" & SDK.GetUserVar("NEWNUMBER") & ";" & SDK.GetUserVar("NEWNAME") & "||ONCLCLICK")
                        PC_SaveCurrentPhoneBook()
                    End If
                Else
                    SDK.Execute("SetErrorScr;MobilePhone Info;!!! You have no new contact !!!;****************  Sorry  ***************")
                End If
                ProcessCommand = 2
            Case "mobilephone_delete_entry" 'OK
                SDK.Execute("CLDEL;" & CLPOS)
                PC_SaveCurrentPhoneBook()
                ProcessCommand = 2
                'see also 'mobilephone_addpicturetoitem;......'
            Case "mobilephone_edit_entry"
                If LCase(CheckRRSkinName()) = "reborn" Then
                    SDK.SetUserVar("USEDX", "True") 'issue with Reborn skin if USEDX is False SAVE cmd that follow don't run
                End If
                SDK.Execute("SETVARFROMVAR;NEWNAME;LISTDESC||SETVARFROMVAR;NEWNUMBER;LISTTEXT||SETVARBYCODE;NEWIMG;CLIMG||" & _
                            "LOAD;MOBILEPHONE_CONTACT_EDIT.skin||CLCLEAR;ALL||CLLOAD;" & sPhonebookpath & "MobilePhone_" & _
                            sPhonebooktype & ".txt||RUN;" & MainPath & "SlideShow.exe" & ";SlideShow")
                ProcessCommand = 2
            Case "mobilephone_save_entry" 'modif item in list and save list after edit
                Dim PictureContact As String = SDK.GetUserVar("NEWIMG")
                SDK.Execute("CLSET;" & SDK.GetInfo("CLPOS") & ";$NEWNUMBER$;$NEWNAME$||CLSETIMG;" & SDK.GetInfo("CLPOS") & ";" & PictureContact)
                SDK.Execute("CLSAVE;" & sPhonebookpath & "MobilePhone_" & sPhonebooktype & ".txt")
                If LCase(CheckRRSkinName()) = "reborn" Then
                    SDK.SetUserVar("USEDX", "False")
                End If
                ProcessCommand = 2
                'PHONEBOOK MANAGEMENT COMMANDS


            Case "mobilephone_sync" 'A MODIFIER POUR PB ICH OCH MCH et CCH ' UPDATE PC FILES WITH PHONE (OVERWRITES THE CURRENT PC FILES!!)
                'workerThread = New Threading.Thread(AddressOf bluetooth.RRListSyncProcess)
                'workerThread.Start()
                ProcessCommand = 2

            Case "mobilephone_phonebooksort"
                workerThread = New Threading.Thread(AddressOf bluetooth.PhoneBookSort)
                workerThread.Start()
                PC_LoadPhoneBook("PB")
                ProcessCommand = 2


                'Old Phone's Contacts
                'Case "mobilephone_build_pc"
                '    Dim myarray() = phoneBookData.List.ToArray
                '    For n As Integer = 0 To myarray.Length - 1
                '        bluetooth.AddCustomList(MainPath & "PhoneBook\MobilePhone_PC.txt", phoneBookData.List.Item(n).Number,
                '                      phoneBookData.List.Item(n).Name.Replace("/M", " Mobile").Replace("/H", " Home").Replace("/W", " Work"), "")
                '    Next
                '    ProcessCommand = 2
            Case "mobilephone_mt" 'PC PHONE BOOK FROM MT
                PC_LoadPhoneBook("MT")
                ProcessCommand = 2
            Case "mobilephone_me" 'MEMORY PHONE BOOK FROM ME
                PC_LoadPhoneBook("ME")
                ProcessCommand = 2
            Case "mobilephone_sm" 'SIM Card PHONE BOOK FROM SM
                PC_LoadPhoneBook("SM")
                ProcessCommand = 2
            Case "mobilephone_dc" 'SIM Card PHONE BOOK FROM PC
                PC_LoadPhoneBook("DC")
                ProcessCommand = 2
            Case "mobilephone_rc" 'SIM Card PHONE BOOK FROM PC
                PC_LoadPhoneBook("RC")
                ProcessCommand = 2
                'Old Phone's Contacts



            Case "mobilephone_siri"
                bluetooth.Siri()
                ProcessCommand = 2

            Case "mobilephone_requestphoneinfo"
                bluetooth.requestPhoneInfo()
                ProcessCommand = 2

                'debug form
            Case "mobilephone_debugon"
                debugFrm.Show()
                debugFrm.debugTextBox.AppendText("The debug window is ready ..." & vbCrLf)
                ProcessCommand = 2
            Case "mobilephone_debugappendtext"
                debugFrm.debugTextBox.AppendText("Test" & vbCrLf)
                ProcessCommand = 2
            Case "mobilephone_debugoff"
                debugFrm.Hide()
                ProcessCommand = 2
            Case "mobilephone_debugclear"
                debugFrm.debugTextBox.Clear()
                debugFrm.debugTextBox.AppendText("Clear is done ..." & vbCrLf)
                ProcessCommand = 2

            Case "mobilephone_phonebooktypelist" 'OK
                bluetooth.getPhoneBookTypeList()
                ProcessCommand = 2

                'SMS COMMANDS
            Case "mobilephone_smsmodels"
                SDK.Execute("LOAD;MOBILEPHONE_SMSMODELS.skin||CLCLEAR;ALL||CLLOAD;" & MainPath & "Models\Models.lst||CLPOS;1||ONCLPOSCHANGE")
                ProcessCommand = 2
            Case "mobilephone_addmodel"
                SDK.Execute("SETVARFROMVAR;TEXTTOSEND;LISTTEXT||ESC||LOAD;MOBILEPHONE_SMSWRITE.skin")
                ProcessCommand = 2
            Case "mobilephone_smswrite"
                If GPS.Valid AndAlso GPS.Speed > 0 AndAlso SDK.GetInd("pluginmgr;status;GpsLockout") = "False" Then
                    SDK.Execute("SetErrorScr;MobilePhone Info;!!! Please, STOP your vehicule !!!;****************  Sorry  ***************" & _
                                "||SETVARFROMVAR;CONTACTNAMETOSEND;LISTDESC||SETVARFROMVAR;CONTACTNUMBERTOSEND;LISTTEXT" & _
                                "||SETVARBYCODE;CONTACTIMGTOSEND;CLIMG||SETVAR;MOBILEPHONE_INFO;...||LOAD;MOBILEPHONE_SMSWRITE.skin")
                Else
                    SDK.Execute("SETVARFROMVAR;CONTACTNAMETOSEND;LISTDESC||SETVARFROMVAR;CONTACTNUMBERTOSEND;LISTTEXT" & _
                                "||SETVARBYCODE;CONTACTIMGTOSEND;CLIMG||SETVAR;MOBILEPHONE_INFO;...||LOAD;MOBILEPHONE_SMSWRITE.skin")
                End If
                ProcessCommand = 2
            Case "mobilephone_sendsms"
                bluetooth.SendSMS(SDK.GetUserVar("CONTACTNUMBERTOSEND"), SDK.GetUserVar("TEXTTOSEND"))
                SDK.Execute("SETVAR;MOBILEPHONE_INFO;Message is Send to " & SDK.GetUserVar("CONTACTNAMETOSEND"))
                ProcessCommand = 2

            Case "mobilephone_smsread"
                If GPS.Valid AndAlso GPS.Speed > 0 AndAlso SDK.GetInd("pluginmgr;status;GpsLockout") = "False" Then
                    SDK.Execute("SetErrorScr;MobilePhone Info;!!! Please, STOP your vehicule !!!;****************  Sorry  ***************") ' & _
                    '"LOAD;MOBILEPHONE_SMSREAD.skin||wait;1||SetErrorScr;MobilePhone Error;This feature isn't active !;****************  Sorry  ***************;5")
                Else
                    SDK.Execute("LOAD;MOBILEPHONE_SMSREAD.skin||SETVAR;MOBILEPHONE_SMSNUMBER;1")
                End If
                ProcessCommand = 2
                'SMS COMMANDS

                'PHONE ALARME
            Case "mobilephone_phonealarm"
                SDK.Execute("SETVAR;MOBILEPHONE_TIMEHOURS;0||SETVAR;MOBILEPHONE_TIMEMINUTES;0||LOAD;MOBILEPHONE_ADDALARM.skin")
                ProcessCommand = 2
            Case "mobilephone_hoursup"
                SDK.Execute("EVAL;MOBILEPHONE_TIMEHOURS;$MOBILEPHONE_TIMEHOURS$+1||IFVAR;MOBILEPHONE_TIMEHOURS=25;SETVAR;MOBILEPHONE_TIMEHOURS;24<<")
                ProcessCommand = 2
            Case "mobilephone_hoursdown"
                SDK.Execute("EVAL;MOBILEPHONE_TIMEHOURS;$MOBILEPHONE_TIMEHOURS$-1||IFVAR;MOBILEPHONE_TIMEHOURS=-1;SETVAR;MOBILEPHONE_TIMEHOURS;0<<")
                ProcessCommand = 2
            Case "mobilephone_minutesup"
                SDK.Execute("EVAL;MOBILEPHONE_TIMEMINUTES;$MOBILEPHONE_TIMEMINUTES$+1||IFVAR;MOBILEPHONE_TIMEMINUTES=60;SETVAR;MOBILEPHONE_TIMEMINUTES;59<<")
                ProcessCommand = 2
            Case "mobilephone_minutesdown"
                SDK.Execute("EVAL;MOBILEPHONE_TIMEMINUTES;$MOBILEPHONE_TIMEMINUTES$-1||IFVAR;MOBILEPHONE_TIMEMINUTES=-1;SETVAR;MOBILEPHONE_TIMEMINUTES;0<<")
                ProcessCommand = 2
                'PHONE ALARME

                'RRTRANSLATOR
            Case "mobilephone_translate" 'MOBILEPHONE_MESSAGE
                If SDK.GetInd("pluginmgr;status;rrtranslator") = "True" Then
                    If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smswrite.skin" Then
                        SDK.Execute("SETVARFROMVAR;RR_TRANSLATOR_FROMTEXT;TEXTTOSEND||RR_TRANSLATOR_TRANSLATE||" & _
                                    "SETVARFROMVAR;TEXTTOSEND;RR_TRANSLATOR_TOTEXT")
                    ElseIf LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smsread.skin" Then
                        SDK.Execute("SETVARFROMVAR;RR_TRANSLATOR_FROMTEXT;MOBILEPHONE_MESSAGE||RR_TRANSLATOR_TRANSLATE||" & _
                                    "SETVARFROMVAR;MOBILEPHONE_MESSAGE;RR_TRANSLATOR_TOTEXT")
                    End If
                Else
                    SDK.ErrScrn("Translator Error !!!", "If you want to use this function,", " you need to install the plugin RRTranslator ...", 5)
                End If
                ProcessCommand = 2
                'RRTRANSLATOR

            Case "mobilephone_unformatednumber" 'envoyé par un plugin (par exemple RRGoogleMapsTools)
                dialNumber = SDK.GetUserVar("MOBILEPHONE_UNFORMATEDNUMBER").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace(".", "")
                ProcessCommand = 2

                'SETTINGS
            Case "mobilephone_settings"
                If Not File.Exists(MainPath & "MobilePhone_DevicesList.lst") Then
                    SDK.Execute("LOAD;MOBILEPHONE_SETTINGS.skin||CLCLEAR;ALL")
                    SDK.SetUserVar("MOBILEPHONE_INFO", "The device's list is updated, please wait !!!")
                    Search_Device()
                    ProcessCommand = 2
                Else
                    SDK.Execute("LOAD;MOBILEPHONE_SETTINGS.skin||CLCLEAR;ALL||CLLOAD;" & MainPath & "MobilePhone_DevicesList.lst||CLPOS;" & PhoneCheckedIs.ToString & "||ONCLPOSCHANGE")
                    SDK.SetUserVar("MOBILEPHONE_INFO", "The device's list is updated !!!")
                    ProcessCommand = 2
                End If
            Case "mobilephone_settings_default"
                cMySettings.SetToDefault(PluginSettings)
                ' copy to temp (skin views temp)
                cMySettings.Copy(PluginSettings, TempPluginSettings)
                cMySettings.SerializeToXML(PluginSettings)
                ProcessCommand = 2
            Case "mobilephone_settings_apply"
                Dim SecondPhone As String = ""
                If CInt(SDK.GetInfo("CLMAX")) > 1 Then
                    TempPluginSettings.PhoneMacAddress2 = SDK.GetInfo("CLTEXT" & Phone2.ToString)
                    SecondPhone = "Second Phone '" & SDK.GetInfo("CLDESC" & Phone2.ToString) & "' --> " & TempPluginSettings.PhoneMacAddress2
                End If
                Try
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                    bluetooth.unload()
                    bluetooth = New BT()

                    SDK.ErrScrn("MobilePhone Info", "First Phone '" & SDK.GetInfo("CLDESC") & "' --> " & TempPluginSettings.PhoneMacAddress, SecondPhone, 10)
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
                ProcessCommand = 2


                'Case "testlist"
                '    Try
                '        Dim ListeSettings As List(Of cMySettings)
                '        MsgBox(ListeSettings(0).PhoneCountryCodes(0))
                '    Catch ex As Exception
                '        MessageBox.Show(ex.Message)
                '    End Try
                '    ProcessCommand = 2


            Case "mobilephone_settings_cancel"
                cMySettings.Copy(PluginSettings, TempPluginSettings)
                ProcessCommand = 2
            Case "mobilephone_settings_updatedevicelist"
                Search_Device()
                ProcessCommand = 2
            Case "mobilephone_settings_updateinfodevice" ' "onclposchange"
                If Not File.Exists(MainPath & "MobilePhone_DevicesList.lst") Then
                    Search_Device()
                    ProcessCommand = 2
                Else
                    If TempPluginSettings.PhoneMacAddress = PluginSettings.PhoneMacAddress And CBool(bluetooth.phoneConnection) = True Then
                        PhoneSatus = "Connected"
                        TempPluginSettings.PhoneMacAddress = SDK.GetInfo("CLTEXT")
                    Else
                        PhoneSatus = "Not Connected"
                        TempPluginSettings.PhoneMacAddress = SDK.GetInfo("CLTEXT")
                    End If
                    ProcessCommand = 2
                End If
            Case "mobilephone_settings_updateserviceslist"
                Search_Services()
                ProcessCommand = 2

            Case "mobilephone_update_emergencynumber"
                TempPluginSettings.EmergencyNumber = SDK.GetUserVar("MOBILEPHONE_EMERGENCYNUMBER")
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                ProcessCommand = 2
            Case "mobilephone_update_smscenternumber"
                TempPluginSettings.SmsServiceCentreAddress = SDK.GetUserVar("MOBILEPHONE_SMSCENTERSERVICENUMBER")
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                ProcessCommand = 2
            Case "mobilephone_update_phoneculture"
                TempPluginSettings.PhoneCountryCodes = SDK.GetUserVar("MOBILEPHONE_PHONECULTURE")
                'MessageBox.Show(PluginSettings.PhoneCountryCodes & "  " & TempPluginSettings.PhoneCountryCodes)
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                ProcessCommand = 2

            Case "mobilephone_paired"
                Paired_Device()
                ProcessCommand = 2
            Case "mobilephone_unpaired"
                bluetooth.BlueSoleil_PairedUnpairedDelete(CInt(SDK.GetInfo("CLPOS")) - 1, 2, "")
                ProcessCommand = 2
            Case "mobilephone_deletepaired"
                bluetooth.BlueSoleil_PairedUnpairedDelete(CInt(SDK.GetInfo("CLPOS")) - 1, 3, "")
                ProcessCommand = 2

            Case "mobilephone_phoneprefixbycountry"
                CountryToPhoneIndicatifList()
                ProcessCommand = 2
                'SETTINGS


                'Case "mobilephone_a2dp"
                '    bluetooth.RunA2DP()
                '    ProcessCommand = 2


                'Case "mobilephone_writecontacttophone"
                '    If LCase(SDK.GetUserVar("NEWCONTACT")) <> LCase(CallerName) Then
                '        CallerName = SDK.GetUserVar("NEWCONTACT")
                '        'MessageBox.Show("Contact is renamed !")
                '    Else
                '        'MessageBox.Show("Contact is unknow !")
                '    End If
                '    bluetooth.Phone_WriteContact(CallerName, CallerID, "129")
                '    ProcessCommand = 2
        End Select

        'for check callername only
        If LCase(Left(CMD, 29)) = "mobilephone_checkcontactname;" Then
            Try
                'Show Call Screen
                'MsgBox(bluetooth.callerID)
                SDK.SetUserVar("MOBILEPHONE_PHOTOPATH", "")
                CallerID = Mid(CMD, 30) 'IIf(Mid(CMD, 30).StartsWith("+"), Mid(CMD, 30).Replace("+", ""), Mid(CMD, 30))  'bluetooth.callerID
                CallerName = "Unknow"
                'MessageBox.Show("CallerName :" & CallerName & vbCrLf & "CallerID :" & CallerID)
                SDK.SetUserVar("NEWCONTACT", CallerName)
                If File.Exists(MainPath & "PhoneBook\MobilePhone_MT.txt") AndAlso bluetooth.CheckIfContactIsInList(MainPath & "PhoneBook\MobilePhone_MT.txt", CallerID) Then
                    CallerName = ContactNameFromNumber(MainPath & "PhoneBook\MobilePhone_MT.txt", CallerID)
                    'MessageBox.Show("CallerName :" & CallerName & vbCrLf & "CallerID :" & CallerID)
                    If File.Exists(sPhotoPath & CallerID & "." & sPhotoExtension) Then
                        SDK.Execute("MENU;MOBILECALL.skin||SETVAR;MOBILEPHONE_PHOTOPATH;" & sPhotoPath & CallerID & "." & sPhotoExtension)
                    Else
                        SDK.Execute("MENU;MOBILECALL.skin")
                    End If
                Else
                    SDK.Execute("MENU;MOBILECALL.skin||SETVAR;MOBILEPHONE_PHOTOPATH;")
                    CallerName = "Unknow"
                End If
                'Pause Audio
                'SDK.Execute("PAUSE||DVBMUTE;1||*ONMOBILEPHONERINGING", True)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
            ProcessCommand = 2
        End If

        'mobilephone_dial;112
        If LCase(Left$(CMD, 17)) = "mobilephone_dial;" Then
            SDK.Execute("LOAD;MOBILEPHONE.SKIN")
            dialNumber = Mid$(CMD, 18)
            If dialNumber = "EMERGENCY" Then dialNumber = TempPluginSettings.EmergencyNumber
            ToLog("'" & Mid$(CMD, 18) & " is replaced by 'EMERGENCY'")
            bluetooth.dial(dialNumber)
            ProcessCommand = 2
        End If
        'mobilephone_dialbyname;pierre
        If LCase(Left$(CMD, 17)) = "mobilephone_dialbyname;" Then
            SDK.Execute("LOAD;MOBILEPHONE.SKIN")
            dialNumber = Mid$(CMD, 18)
            bluetooth.dial(dialNumber)
            ProcessCommand = 2
        End If

        'mobilephone;addcontact;$SKINPATH$phone_list.txt;080030001000|Kai|$SKINPATH$include\contacts\userpics\kai.jpg
        If LCase(Left$(CMD, 23)) = "mobilephone;addcontact;" Then
            Dim dataArray() As String = {0}
            IconPath = Split(dataArray(3), "|")
            AddCustomList(dataArray(2), IconPath(0), IconPath(1), IconPath(2))
            ToLog("The contact " & "'" & IconPath(1) & "'" & " is added")
            ProcessCommand = 2
        End If

        'mobilephone_addpicturetoitem;+33557590878.gif
        If LCase(Left$(CMD, 29)) = "mobilephone_addpicturetoitem;" Then
            Dim PictureContact As String = Mid$(CMD, 30)
            SDK.Execute("CLSETIMG;" & SDK.GetInfo("CLPOS") & ";" & MainPath & "Photo\" & PictureContact & "||SETVARBYCODE;NEWIMG;CLIMG||" & _
                        "SETVAR;MOBILEPHONE_NEWPICONTACT;" & PictureContact)
            ToLog("The picture's contact " & "'" & PictureContact & "'" & " is added")
            ProcessCommand = 2
        End If

        'mobilephone;deletecontact;$SKINPATH$phone_list.txt;$LISTDESC$
        If LCase(Left$(CMD, 26)) = "mobilephone;deletecontact;" Then
            Dim dataArray() As String = {0}
            ToLog("The contact " & "'" & dataArray(3) & "'" & " is deleted")
            ReturnedLineNumberForString = (Val(SDK.GetUserVar("PBCLPOS")) * 2) - 1
            DeleteCustomList(dataArray(2), "", ReturnedLineNumberForString, 0)
            DeleteCustomList(dataArray(2), "", ReturnedLineNumberForString, 0)
            ProcessCommand = 2
        End If

        'mobilephone;modifycontact;$PLUGINSPATH$PhoneBook\MobilePhone_MT.txt;$NEWNUMBER$|$NEWNAME$|$NEWIMG$
        If LCase(Left$(CMD, 26)) = "mobilephone;modifycontact;" Then
            Dim dataArray() As String = {0}
            '"BMWPHONEMAN","SETVARFROMVAR;MODNAME;listdesc||SETVARFROMVAR;MODNUMBER;LISTTEXT||SETVARFROMVAR;MODIMG;CLIMG||SETVARFROMVAR;MODPOS;PBCLPOS||LOAD;bmw_phone_man.skin||MODCONTACT"
            IconPath = Split(dataArray(3), "|")
            ToLog("The contact " & SDK.GetUserVar("MODNAME") & " is modified")
            ToLog("New number " & IconPath(0))
            ToLog("New name " & IconPath(1))
            ToLog("New icon " & IconPath(2))
            ModifyCustomList(dataArray(2), SDK.GetUserVar("MODNAME"), IconPath(0), IconPath(1), IconPath(2)) 'Number,Name,Icon
            ProcessCommand = 2
        End If


        ''mobilephone_smsread;all
        'If LCase(Left$(CMD, 20)) = "mobilephone_smsread;" Then
        '    'SDK.Execute("MENU;MOBILEPHONE_READSMS.SKIN")
        '    bluetooth.ReadAllSMS(Mid$(CMD, 21))
        '    ProcessCommand = 2
        'End If
        'mobilephone_smsselectone;+ or -
        If LCase(Left$(CMD, 25)) = "mobilephone_smsselectone;" Then
            If Mid$(CMD, 26) = "+" Then
                If CInt(SDK.GetUserVar("MOBILEPHONE_SMSNUMBER")) < NumberOfSMSInMemoryNew Then
                    SDK.Execute("EVAL;MOBILEPHONE_SMSNUMBER;$MOBILEPHONE_SMSNUMBER$+1")
                ElseIf CInt(SDK.GetUserVar("MOBILEPHONE_SMSNUMBER")) = NumberOfSMSInMemoryNew Then
                    SDK.Execute("SETVAR;MOBILEPHONE_SMSNUMBER;1")
                End If
            ElseIf Mid$(CMD, 26) = "-" Then
                If CInt(SDK.GetUserVar("MOBILEPHONE_SMSNUMBER")) > 1 Then
                    SDK.Execute("EVAL;MOBILEPHONE_SMSNUMBER;$MOBILEPHONE_SMSNUMBER$-1")
                ElseIf CInt(SDK.GetUserVar("MOBILEPHONE_SMSNUMBER")) = 1 Then
                    SDK.Execute("SETVAR;MOBILEPHONE_SMSNUMBER;" & NumberOfSMSInMemoryNew)
                End If
            End If
            ProcessCommand = 2
        End If
        'mobilephone_smsreadone;n
        If LCase(Left$(CMD, 23)) = "mobilephone_smsreadone;" Then
            If CInt(NumberOfSMSInMemoryNew) > 0 Then
                Dim SmsNb As String = ""
                If CMD.Contains("$") Then 'check if after ; is a var or number
                    CMD = CMD.Replace("$", "")
                    SmsNb = SDK.GetUserVar(Mid$(CMD, 24))
                Else
                    SmsNb = Mid$(CMD, 24)
                End If

                Try
                    'bluetooth.ATExecute("AT+CSCS=""UCS2""" & vbCrLf, 500)
                    SDK.Execute("SETVAR;MOBILEPHONE_SMSFROMNUMBER;-||SETVAR;MOBILEPHONE_SMSFROMDATETIME;-||mobilephone_atcmd;AT+CMGR=" & SmsNb) 'MessageBox.Show(SmsNb)  
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            ElseIf CInt(NumberOfSMSInMemoryNew) = 0 Then
                SDK.Execute("SETVAR;MOBILEPHONE_INFO;... You have no unread message ...")
            Else
                SDK.Execute("SETVAR;MOBILEPHONE_INFO;... Please wait ...")
            End If
            ProcessCommand = 2
        End If


        'mobilephone_atcmd;at+cgmm
        If LCase(Left$(CMD, 18)) = "mobilephone_atcmd;" Then
            If LCase(Mid$(CMD, 19)) = "ctrlz" Then
                bluetooth.ATExecute(Chr(26) & vbCrLf, 5000)
                'debugFrm.debugTextBox.AppendText(Chr(26) & vbCrLf)
                If debugFrm.Visible Then debugFrm.debugTextBox.Focus()
                SendKeys.Send("^z")
            Else
                bluetooth.ATExecute(Mid$(CMD, 19) & vbCrLf, 5000)
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText(Mid$(CMD, 19) & vbCrLf)
            End If
            'debugFrm.debugTextBox.AppendText(bluetooth.ATResultValue & vbCrLf)
            ProcessCommand = 2
        End If
        'mobilephone_microvolume;15
        If LCase(Left$(CMD, 24)) = "mobilephone_microvolume;" Then
            bluetooth.phonemicrovolMax(Mid$(CMD, 25))
            ProcessCommand = 2
        End If
        'mobilephone_speakervolume;15
        If LCase(Left$(CMD, 26)) = "mobilephone_speakervolume;" Then
            bluetooth.phonemicrovolMax(Mid$(CMD, 27))
            ProcessCommand = 2
        End If
        'mobilephone_phonebooksort;me
        'If LCase(Left$(CMD, 26)) = "mobilephone_phonebooksort;" Then
        '    If Not SynchroningInProgress Then
        '        SynchroningInProgress = True
        '        bluetooth.Phone_SortPhoneBook(LCase(Mid$(CMD, 27)))
        '        SynchroningInProgress = False
        '    Else
        '        bluetooth.Phone_SortPhoneBook(LCase(Mid$(CMD, 27)))
        '    End If
        '    ProcessCommand = 2
        'End If
        '## Phone Manager by pierrotm777 ###################################################


        If LCase(Left$(CMD, 22)) = "mobilephone_alarmssave" Then
            Try
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Save Alarm error")
            End Try
            ProcessCommand = 2
        End If
        'mobilephone_alarmset;1
        If LCase(Left$(CMD, 21)) = "mobilephone_alarmset;" Then
            Try
                AlarmAttributes = ""
                AlarmAttributes &= Chr(34)
                AlarmAttributes &= SDK.GetUserVar("MOBILEPHONE_TIMEHOURS") 'time
                AlarmAttributes &= ":"
                AlarmAttributes &= SDK.GetUserVar("MOBILEPHONE_TIMEMINUTES") 'time
                AlarmAttributes &= Chr(34)
                AlarmAttributes &= "|"
                AlarmAttributes &= Mid$(CMD, 22) 'position alarm
                AlarmAttributes &= "|"
                AlarmAttributes &= IIf(SDK.GetUserVar("MOBILEPHONE_ALARMTYPE") = "", 1, SDK.GetUserVar("MOBILEPHONE_ALARMTYPE")) 'alarm type
                AlarmAttributes &= "|"
                AlarmAttributes &= Chr(34)
                AlarmAttributes &= IIf(SDK.GetUserVar("MOBILEPHONE_ALARMTEXT") = "", "..", SDK.GetUserVar("MOBILEPHONE_ALARMTEXT")) 'text alarm
                AlarmAttributes &= Chr(34)
                AlarmAttributes &= "|"
                AlarmAttributes &= Chr(34)
                Dim AlarmDays As String = ""
                If AlarmIndicator_Monday = True Then AlarmDays &= "1"
                If AlarmIndicator_Tuesday = True Then AlarmDays &= "2"
                If AlarmIndicator_Wednesday = True Then AlarmDays &= "3"
                If AlarmIndicator_Thirsday = True Then AlarmDays &= "4"
                If AlarmIndicator_Friday = True Then AlarmDays &= "5"
                If AlarmIndicator_Saturday = True Then AlarmDays &= "6"
                If AlarmIndicator_Sunday = True Then AlarmDays &= "7"
                Dim charArray() As Char = AlarmDays.ToCharArray
                AlarmDays = ""
                For i = 0 To charArray.Length - 1
                    If i < charArray.Length - 1 Then
                        AlarmDays &= charArray(i) & ","
                    Else
                        AlarmDays &= charArray(i)
                    End If
                Next
                AlarmAttributes &= AlarmDays
                AlarmAttributes &= Chr(34)

                Select Case Val(Mid$(CMD, 22))
                    Case 1 : TempPluginSettings.PhoneAlarm1 = AlarmAttributes
                    Case 2 : TempPluginSettings.PhoneAlarm2 = AlarmAttributes
                    Case 3 : TempPluginSettings.PhoneAlarm3 = AlarmAttributes
                    Case 4 : TempPluginSettings.PhoneAlarm4 = AlarmAttributes
                    Case 5 : TempPluginSettings.PhoneAlarm5 = AlarmAttributes
                End Select
                'MessageBox.Show(AlarmAttributes)
                'Final format:
                'mobilephone_phonealarm;"11:15",3             ,1                       ,"Alarm Text","1,2,3,4,5"
                '                       "time ",position alarm,0=recurrent alarm 1=time,"Alarm Text","Monday (1), Sunday (7) Weekend(0)"
                If SDK.GetInd("mobilephone_alarm1_on") = "True" Then
                    AlarmAttributes = AlarmAttributes.Replace("|", ",")
                    bluetooth.ATExecute("AT+CALA=" & AlarmAttributes & vbCrLf, 500)
                End If
                'If att.Length = 5 Then
                'Else
                '    SDK.ErrScrn("MobilePhone Error", "Alarm format error !!!", "mobilephone_phonealarm;" & Chr(34) & "11:15" & Chr(34) & "|3|1|" & Chr(34) & "Alarm Text" & Chr(34) & "|" & Chr(34) & "1,2,3,4,5" & Chr(34), 5)
                'End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Set Alarm error")
            End Try
            ProcessCommand = 2
        End If

        'mobilephone_alarmget;1
        If LCase(Left$(CMD, 21)) = "mobilephone_alarmget;" Then
            Try
                Select Case Val(Mid$(CMD, 22))
                    Case 1 : AlarmAttributes = TempPluginSettings.PhoneAlarm1
                    Case 2 : AlarmAttributes = TempPluginSettings.PhoneAlarm2
                    Case 3 : AlarmAttributes = TempPluginSettings.PhoneAlarm3
                    Case 4 : AlarmAttributes = TempPluginSettings.PhoneAlarm4
                    Case 5 : AlarmAttributes = TempPluginSettings.PhoneAlarm5
                End Select

                Dim getAlarm() As String = AlarmAttributes.Replace(Chr(34), "").Split("|")
                Dim getTime() As String = getAlarm(0).Split(":")
                SDK.SetUserVar("MOBILEPHONE_TIMEHOURS", getTime(0)) 'time
                SDK.SetUserVar("MOBILEPHONE_TIMEMINUTES", getTime(1)) 'time
                SDK.SetUserVar("MOBILEPHONE_ALARMTYPE", getAlarm(2))
                SDK.SetUserVar("MOBILEPHONE_ALARMTEXT", getAlarm(3))
                AlarmIndicator_Monday = False
                AlarmIndicator_Tuesday = False
                AlarmIndicator_Wednesday = False
                AlarmIndicator_Thirsday = False
                AlarmIndicator_Friday = False
                AlarmIndicator_Saturday = False
                AlarmIndicator_Sunday = False
                AlarmIndicator_Week = False
                If getAlarm(4).Contains(",") Then
                    Dim getDays() As String = getAlarm(4).Split(",")
                    For i = 0 To getDays.Length - 1
                        Select Case getDays(i)
                            Case "1" : AlarmIndicator_Monday = True
                            Case "2" : AlarmIndicator_Tuesday = True
                            Case "3" : AlarmIndicator_Wednesday = True
                            Case "4" : AlarmIndicator_Thirsday = True
                            Case "5" : AlarmIndicator_Friday = True
                            Case "6" : AlarmIndicator_Saturday = True
                            Case "7" : AlarmIndicator_Sunday = True
                            Case "0"
                                AlarmIndicator_Saturday = True
                                AlarmIndicator_Sunday = True
                        End Select
                    Next
                Else
                    Select Case getAlarm(4)
                        Case "1" : AlarmIndicator_Monday = True
                        Case "2" : AlarmIndicator_Tuesday = True
                        Case "3" : AlarmIndicator_Wednesday = True
                        Case "4" : AlarmIndicator_Thirsday = True
                        Case "5" : AlarmIndicator_Friday = True
                        Case "6" : AlarmIndicator_Saturday = True
                        Case "7" : AlarmIndicator_Sunday = True
                        Case "0"
                            AlarmIndicator_Saturday = True
                            AlarmIndicator_Sunday = True
                    End Select
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
            ProcessCommand = 2
        End If

        If LCase(Left$(CMD, 21)) = "mobilephone_alarmdel;" Then
            bluetooth.ATExecute("AT+CALD=" & Mid$(CMD, 22) & vbCrLf, 500)
            ProcessCommand = 2
        End If
    End Function
#End Region

#Region "Labels"
    '*****************************************************************
    '* This Function will be called with a requested label code and
    '* format specified at the skin file. Simply return any text to
    '* be displayed for the specified format.
    '*****************************************************************
    Public Function ReturnLabel(ByRef LBL As String, ByRef FMT As String) As String

        ReturnLabel = ""
        Select Case LCase(LBL)
            Case "mobilephone_pluginver"
                ReturnLabel = Assembly.GetExecutingAssembly().GetName().Version.ToString()
            Case "mobilephone_sdkversion"
                ReturnLabel = GetVersionSdkDll()
            Case "mobilephone_dialbox"
                ReturnLabel = dialNumber
            Case "mobilephone_manufacturer"
                ReturnLabel = bluetooth.phoneManufacturerName
            Case "mobilephone_model"
                ReturnLabel = Trim(bluetooth.phoneModelName) 'bluetooth.MyPhoneName() 
            Case "mobilephone_info"
                ReturnLabel = SDK.GetUserVar("MOBILEPHONE_INFO")
            Case "mobilephone_imei"
                ReturnLabel = ImeiNumber
            Case "mobilephone_revision"
                ReturnLabel = RevisionNumber
            Case "mobilephone_communicating"
                ReturnLabel = CallIsActif.ToString
            Case "mobilephone_atresultcode"
                ReturnLabel = bluetooth.ATResultValue

            Case "mobilephone_autoswapphone"
                ReturnLabel = TempPluginSettings.AutoSwapPhone
            Case "mobilephone_phoneselected"
                ReturnLabel = IIf(PhoneCheckedIs = 1, "1", "2")
            Case "mobilephone_settings_setphone2"
                ReturnLabel = SDK.GetInfo("CLDESC" & Phone2.ToString)

            Case "mobilephone_batterystrength"
                ReturnLabel = Math.Truncate(bluetooth.batteryLevel / 5 * 100).ToString 'Math.Truncate(bluetooth.batteryLevel * 65535 / 5).ToString()
            Case "battery"
                ReturnLabel = bluetooth.batteryLevel.ToString
            Case "mobilephone_batterycharging"
                ReturnLabel = bluetooth.batteryincharge 'IIf(ExternalPowerIsConnected = True, "True", "False")
            Case "mobilephone_batteryisfull"
                ReturnLabel = bluetooth.batteryStatus 'IIf(BatteryFullCharge = True, "True", "False")

            Case "mobilephone_signalstrength"
                ReturnLabel = Math.Truncate(bluetooth.signalStrength / 31 * 100).ToString 'Math.Truncate(bluetooth.signalStrength * 65535 / 31).ToString()

                'Case "mobilephone_plinkquality"
                '    ReturnLabel = bluetooth.plinkQuality.ToString
            Case "mobilephone_phonetime"
                ReturnLabel = PhoneTime
            Case "mobilephone_phonedate"
                ReturnLabel = PhoneDate

                'Case "mobilephone_smsc"
                '    'ReturnLabel = oMobilePhone.SMSC
            Case "mobilephone_connected"
                ReturnLabel = bluetooth.phoneConnection.ToString
            Case "mobilephone_imsi"
                ReturnLabel = IsmiNumber
            Case "mobilephone_network"
                ReturnLabel = bluetooth.networkOperator
            Case "mobilephone_ownnumber"
                ReturnLabel = bluetooth.phoneSubscriberNumber

                'Case "mobilephone_getcomnumber"
                '    ReturnLabel = bluetooth.BlueSoleil_Btsdk_GetClientPort

            Case "mobilephone_callerid"
                ReturnLabel = bluetooth.callerID
            Case "mobilephone_callername"
                ReturnLabel = CallerName
            Case "mobilephone_phonemacaddress"
                ReturnLabel = PhoneMacAddress
            Case "mobilephone_phonedevicename"
                ReturnLabel = SDK.GetUserVar("MOBILEPHONE_PHONEDEVICENAME")
            Case "mobilephone_phonestatus"
                Try
                    If CBool(bluetooth.phoneConnection) = True Then
                        ReturnLabel = "Connected"
                    Else
                        ReturnLabel = "Not Connected"
                    End If
                Catch
                    ReturnLabel = "Plugin is Off"
                End Try
            Case "mobilephone_networkavail"
                ReturnLabel = bluetooth.networkAvailable
            Case "mobilephone_phonebook"
                ReturnLabel = bluetooth.phonebook
            Case "mobilephone_phonebookupdate"
                ReturnLabel = TempPluginSettings.PhoneBookListUpdate
            Case "mobilephone_phoneconnection"
                ReturnLabel = bluetooth.phoneConnection

            Case "mobilephone_contactsfromphone"
                ReturnLabel = PhoneNumberOfContactsFoundFinal.ToString
            Case "mobilephone_phonebooktypelist"
                ReturnLabel = TempPluginSettings.PhoneBookList
            Case "mobilephone_phonebooklistinuse" 'OK
                ReturnLabel = PhoneListType

            Case "MobilePhone_MTapserviceisready"
                ReturnLabel = PBAPServiceIsReady.ToString
            Case "mobilephone_mapserviceisready"
                ReturnLabel = MAPServiceIsReady.ToString

            Case "mobilephone_smsfromnumber"
                ReturnLabel = SDK.GetUserVar("MOBILEPHONE_SMSFROMNUMBER")
            Case "mobilephone_smsfromdatetime"
                ReturnLabel = SDK.GetUserVar("MOBILEPHONE_SMSFROMDATETIME")

            Case "mobilephone_smsreceivedmemory"
                ReturnLabel = NewSmsIsReceivedMemory
            Case "mobilephone_smsreceivednumber"
                ReturnLabel = NewSmsIsReceivedNumber
            Case "mobilephone_smscenterservicenumber"
                ReturnLabel = TempPluginSettings.SmsServiceCentreAddress

            Case "mobilephone_emergencynumber"
                ReturnLabel = TempPluginSettings.EmergencyNumber

            Case "mobilephone_numberofsmsfound"
                ReturnLabel = NumberOfSMSInMemoryNew
            Case "mobilephone_pdutextmodesusable"
                If PDUTextModesUsable = 0 Then
                    ReturnLabel = "PDU Mode"
                ElseIf PDUTextModesUsable = 1 Or PDUTextModesUsable = 2 Then
                    ReturnLabel = "Text Mode"
                End If

            Case "mobilephone_operatornames"
                ReturnLabel = OperatorNames

                'Case "mobilephone_missedcall"
                '    'ReturnLabel = bluetooth.NumberOfMissedCall.ToString

                'Case "mobilephone_mapinfo"
                '    ReturnLabel = MAPInfo


            Case "mobilephone_sendbytes"
                ReturnLabel = SendBytes
            Case "mobilephone_receivedbytes"
                ReturnLabel = ReceivedBytes


            Case "mobilephone_speechrecognized"
                ReturnLabel = SpeechRecognizedIs
            Case "mobilephone_speecherror"
                ReturnLabel = SpeechToNumberError
            Case "mobilephone_speechrecognition"
                ReturnLabel = TempPluginSettings.PhoneSpeechRecognition
            Case "mobilephone_speechnumbers"
                ReturnLabel = TempPluginSettings.PhoneSpeechNumbers

            Case "mobilephone_speechculture"
                ReturnLabel = MyCultureInfo
            Case "mobilephone_cultureinfo"
                ReturnLabel = MyPhoneCultureInfo
            Case "mobilephone_phoneculture"
                ReturnLabel = TempPluginSettings.PhoneCountryCodes

                'PHONE ALARM
            Case "mobilephone_alarm1"
                ReturnLabel = TempPluginSettings.PhoneAlarm1.Replace("|", ",")
            Case "mobilephone_alarm2"
                ReturnLabel = TempPluginSettings.PhoneAlarm2.Replace("|", ",")
            Case "mobilephone_alarm3"
                ReturnLabel = TempPluginSettings.PhoneAlarm3.Replace("|", ",")
            Case "mobilephone_alarm4"
                ReturnLabel = TempPluginSettings.PhoneAlarm4.Replace("|", ",")
            Case "mobilephone_alarm5"
                ReturnLabel = TempPluginSettings.PhoneAlarm5.Replace("|", ",")
                'PHONE ALARM

            Case "mobilephone_osmajor"
                ReturnLabel = osVer.Version.Major

            Case "mobilephone_osminor"
                ReturnLabel = osVer.Version.Minor


        End Select

    End Function
#End Region

#Region "Indicators"
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
            Case "mobilephone_settings_changed" 'teste si les réglages sont changés par rapport au fichier .xml
                ReturnIndicatorEx = IIf(cMySettings.Compare(PluginSettings, TempPluginSettings) = False, "True", "False")
            Case "mobilephone_networkavail"
                ReturnIndicatorEx = bluetooth.networkAvailable
            Case "mobilephone_connected", "phone_connected_phoco"
                ReturnIndicatorEx = bluetooth.phoneConnection
            Case "mobilephone_incall"
                ReturnIndicatorEx = CallIsActif.ToString

            Case "mobilephone_sdkfound", "bt_connected"
                ReturnIndicatorEx = BlueSoleil_IsInstalled().ToString

                'Phone Books
            Case "mobilephone_phonebook"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_ME.txt") = True, "True", "False")
            Case "mobilephone_simbook"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_SM.txt") = True, "True", "False")
            Case "mobilephone_phonemc"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_MC.txt"), "True", "False")
            Case "mobilephone_phonedc"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_DC.txt"), "True", "False")
            Case "mobilephone_phonerc"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_RC.txt"), "True", "False")
            Case "mobilephone_phonepc"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_PC.txt"), "True", "False")

            Case "mobilephone_batterystrength"
                ReturnIndicatorEx = Math.Truncate(bluetooth.batteryLevel * 65535 / 5)
            Case "mobilephone_signalstrength"
                ReturnIndicatorEx = Math.Truncate(bluetooth.signalStrength * 65535 / 5)
            Case "mobilephone_batterycharging"
                ReturnIndicatorEx = bluetooth.batteryincharge
            Case "mobilephone_batteryisfull"
                ReturnIndicatorEx = bluetooth.batteryStatus
            Case "mobilephone_incommingcall"
                ReturnIndicatorEx = IncomingCall.ToString
            Case "mobilephone_messagereceived"
                ReturnIndicatorEx = SmsIsReceivedStatus.ToString
            Case "mobilephone_messagereceivedtype"
                ReturnIndicatorEx = SmsIsReceivedType.ToString

            Case "mobilephone_synchronizing"
                ReturnIndicatorEx = IIf(SynchroningInProgress = True Or PhoneBookSyncInProgress = True, "True", "False")
                '+ NEW Skin Command; "SetIndBlink"
                'usage: SetIndBlink;xxxx;yyyy
                '	where xxxx is indicator name
                '	where yyyy is "true/false" or "1/0" or not specified for toggle
                'ex. "SetIndBlink;MUTE;0"                - removes ":b" from indicator MUTE on current screen.
                'ex. "SetIndBlink;MUTE;true"             - adds ":b" from indicator MUTE on current screen.
                'ex. "SetIndBlink;MUTE"             		- toggles ":b" from indicator MUTE on current screen.

            Case "mobilephone_phonebooksort"
                ReturnIndicatorEx = PhoneBookSortInProgress.ToString

            Case "mobilephone_phonebookpicture"
                ReturnIndicatorEx = IIf((LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_contacts.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_addpicture.skin") AndAlso SDK.GetInfo("CLIMG") <> "", SDK.GetInfo("CLIMG"), MainPath & "Photo\unknow.jpg")
            Case "mobilephone_communicating"
                ReturnIndicatorEx = CallIsActif.ToString

            Case "MobilePhone_MTapserviceisready"
                ReturnIndicatorEx = PBAPServiceIsReady.ToString
            Case "mobilephone_mapserviceisready"
                ReturnIndicatorEx = MAPServiceIsReady.ToString

            Case "mobilephone_settings_phone2"
                ReturnIndicatorEx = IIf(SDK.GetInfo("CLTEXT2") <> "07:08:09:10:11:12", "True", "False")

            Case "mobilephone_speechrecognition"
                ReturnIndicatorEx = IIf(SpeechRecognitionIsActive = True, "True", "False")

                'PHONE ALARM
            Case "mobilephone_alarm_monday"
                ReturnIndicatorEx = IIf(AlarmIndicator_Monday = True, "True", "False")
            Case "mobilephone_alarm_tuesday"
                ReturnIndicatorEx = IIf(AlarmIndicator_Tuesday = True, "True", "False")
            Case "mobilephone_alarm_wednesday"
                ReturnIndicatorEx = IIf(AlarmIndicator_Wednesday = True, "True", "False")
            Case "mobilephone_alarm_thirsday"
                ReturnIndicatorEx = IIf(AlarmIndicator_Thirsday = True, "True", "False")
            Case "mobilephone_alarm_friday"
                ReturnIndicatorEx = IIf(AlarmIndicator_Friday = True, "True", "False")
            Case "mobilephone_alarm_saturday"
                ReturnIndicatorEx = IIf(AlarmIndicator_Saturday = True, "True", "False")
            Case "mobilephone_alarm_sunday"
                ReturnIndicatorEx = IIf(AlarmIndicator_Sunday = True, "True", "False")
            Case "mobilephone_alarm_weekday"
                ReturnIndicatorEx = IIf(AlarmIndicator_Week = True, "True", "False")
                If AlarmIndicator_Week = True Then
                    AlarmIndicator_Saturday = True
                    AlarmIndicator_Sunday = True
                End If
            Case "mobilephone_alarm1_on"
                ReturnIndicatorEx = IIf(Alarm1IsOn = True, "True", "False")
            Case "mobilephone_alarm2_on"
                ReturnIndicatorEx = IIf(Alarm2IsOn = True, "True", "False")
            Case "mobilephone_alarm3_on"
                ReturnIndicatorEx = IIf(Alarm3IsOn = True, "True", "False")
            Case "mobilephone_alarm4_on"
                ReturnIndicatorEx = IIf(Alarm4IsOn = True, "True", "False")
            Case "mobilephone_alarm5_on"
                ReturnIndicatorEx = IIf(Alarm5IsOn = True, "True", "False")
                'PHONE ALARM

                'SETTINGS SCREEN 2
            Case "mobilephone_runonstart"
                ReturnIndicatorEx = IIf(TempPluginSettings.RunOnStart = True, "True", "False")
            Case "mobilephone_phonebookupdate"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneBookUpdate = True, "True", "False")
            Case "mobilephone_phonebooklistupdate"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneBookListUpdate = True, "True", "False")
            Case "mobilephone_autoswapphone"
                ReturnIndicatorEx = IIf(TempPluginSettings.AutoSwapPhone = True, "True", "False")
            Case "mobilephone_speech_recognition"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneSpeechRecognition = True, "True", "False")
            Case "mobilephone_debuglog"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneDebugLog = True, "True", "False")
            Case "mobilephone_lockinmotion"
                ReturnIndicatorEx = IIf(TempPluginSettings.LockInMotion = True, "True", "False")
                'IF RRTRANSLATOR is ON
            Case "mobilephone_translatorison"
                ReturnIndicatorEx = IIf(SDK.GetInd("rr_translator") = "True", "True", "False")
            Case "mobilephone_translatorfromlang"
                ReturnIndicatorEx = IIf(SDK.GetInd("rr_translator") = "True", SDK.GetInd("rr_translator_ind_from"), MainPath & "Photo\unknow.gif")
            Case "mobilephone_translatortolang"
                ReturnIndicatorEx = IIf(SDK.GetInd("rr_translator") = "True", SDK.GetInd("rr_translator_ind_to"), MainPath & "Photo\unknow.gif")
                'IF RRTRANSLATOR is ON
                'SETTINGS SCREEN 2


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
            Case "mobilephone_settings_phone2"
                If Phone2 < SDK.GetInfo("CLMAX") Then
                    Phone2 = Phone2 + 1
                Else
                    Phone2 = 1
                End If
                SDK.SetUserVar("MOBILEPHONE_INFO", "The second phone is : " & SDK.GetInfo("CLDESC" & Phone2.ToString))
                TempPluginSettings.PhoneMacAddress2 = SDK.GetInfo("CLTEXT" & Phone2.ToString)

            Case "mobilephone_speechrecognition"
                If TempPluginSettings.PhoneSpeechRecognition = True Then
                    If SpeechRecognitionIsActive = False Then
                        InitSpeechReco()
                    ElseIf SpeechRecognitionIsActive = True Then
                        StopSpeechReco()
                    End If
                Else
                    SDK.ErrScrn("MobilePhone Error", "Speech Recognition is OFF!!!", "Set 'PhoneSpeechRecognition' to 'True'", 5)
                End If

                'PHONE ALARM
            Case "mobilephone_alarm_monday"
                AlarmIndicator_Monday = Not AlarmIndicator_Monday
            Case "mobilephone_alarm_tuesday"
                AlarmIndicator_Tuesday = Not AlarmIndicator_Tuesday
            Case "mobilephone_alarm_wednesday"
                AlarmIndicator_Wednesday = Not AlarmIndicator_Wednesday
            Case "mobilephone_alarm_thirsday"
                AlarmIndicator_Thirsday = Not AlarmIndicator_Thirsday
            Case "mobilephone_alarm_friday"
                AlarmIndicator_Friday = Not AlarmIndicator_Friday
            Case "mobilephone_alarm_saturday"
                AlarmIndicator_Saturday = Not AlarmIndicator_Saturday
            Case "mobilephone_alarm_sunday"
                AlarmIndicator_Sunday = Not AlarmIndicator_Sunday
            Case "mobilephone_alarm_weekday"
                AlarmIndicator_Week = Not AlarmIndicator_Week
            Case "mobilephone_alarm1_on"
                Alarm1IsOn = Not Alarm1IsOn
            Case "mobilephone_alarm2_on"
                Alarm2IsOn = Not Alarm2IsOn
            Case "mobilephone_alarm3_on"
                Alarm3IsOn = Not Alarm3IsOn
            Case "mobilephone_alarm4_on"
                Alarm4IsOn = Not Alarm4IsOn
            Case "mobilephone_alarm5_on"
                Alarm5IsOn = Not Alarm5IsOn
                'PHONE ALARM

                'SETTINGS SCREEN 2
            Case "mobilephone_runonstart"
                TempPluginSettings.RunOnStart = Not TempPluginSettings.RunOnStart
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_phonebookupdate"
                If TempPluginSettings.PhoneBookUpdate = False Then SDK.Execute("mobilephone_sync")
                TempPluginSettings.PhoneBookUpdate = Not TempPluginSettings.PhoneBookUpdate
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_phonebooklistupdate"
                If TempPluginSettings.PhoneBookListUpdate = False Then bluetooth.getPhoneBookTypeList()
                TempPluginSettings.PhoneBookListUpdate = Not TempPluginSettings.PhoneBookListUpdate
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_autoswapphone"
                TempPluginSettings.AutoSwapPhone = Not TempPluginSettings.AutoSwapPhone
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                SDK.ErrScrn("MobilePhone Info", "This feature isn't active !!! Sorry", "*********************************************", 5)
            Case "mobilephone_speech_recognition"
                TempPluginSettings.PhoneSpeechRecognition = Not TempPluginSettings.PhoneSpeechRecognition
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_speech_recognitionreset"
                TempPluginSettings.PhoneSpeechNumbers = "zero,one,two,three,four,five,six,seven,eight,nine,more,star,sharp,delete,cancel,call,stop call,help call,by name,-,-,-"
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                SDK.ErrScrn("MobilePhone Info", "English isn't your language ?", "You must edit the xml file into 'PhoneSpeechNumbers' part!", 5)

            Case "mobilephone_debuglog"
                TempPluginSettings.PhoneDebugLog = Not TempPluginSettings.PhoneDebugLog
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_lockinmotion"
                TempPluginSettings.LockInMotion = Not TempPluginSettings.LockInMotion
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                'SETTINGS SCREEN 2
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

#Region "Init BlueSoleil and Search Phone 1 or 2"
    'Private Sub InitAndSearchPhone()
    '    'Init BlueSoleil
    '    SDK.SetUserVar("PHONE", "TRY LOAD PHONE1")
    '    If BlueSoleil_IsInstalled() = True Then
    '        PhoneCheckedIs = 1 'recherche le premier mobile s'il est défini dans le .xml
    '        bluetooth = New BT

    '        If TempPluginSettings.AutoSwapPhone = True Then
    '            If CBool(bluetooth.phoneConnection) = False Then
    '                Thread.Sleep(2000)
    '                bluetooth.unload()
    '                SDK.SetUserVar("PHONE", "TRY LOAD PHONE2")
    '                PhoneCheckedIs = 2 'recherche le deuxième mobile s'il est défini dans le .xml
    '                bluetooth = New BT
    '            End If
    '            Thread.Sleep(10000)
    '            If CBool(bluetooth.phoneConnection) = False Then
    '                SearchPhoneTimer1.Enabled = True
    '            Else
    '                SearchPhoneTimer1.Enabled = False
    '            End If
    '        End If

    '    Else
    '        SDK.Execute("WAIT;5||SetErrorScr;MobilePhone Error;Bluesoleil isn't Installed !!!;*****************************************;5")
    '    End If
    'End Sub
    'Private Sub OnTimedEvent(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
    '    InitAndSearchPhone()
    'End Sub
#End Region



#Region "Gestion Custom List avec ligne ICO"
    'Private sMobilePhoneInfoText As String
    Private sPhonebookpath As String
    Private sPhonebooktype As String
    Private sPhotoPath As String
    Private sPhotoExtension As String
    Private allowedExtensions() As String = {"gif", "jpg", "bmp", "png"} ', ".tiff"}

    Private CLPOS As Integer
    Private CLMAX As Integer
    Private CLTOP As Integer
    Private CLLINES As Integer
    Private CLPAGE As Integer
    Private CLPAGES As Integer
    Private CLIMG As Integer
    Private Sub updateListInfo()
        CLPOS = Integer.Parse(SDK.GetInfo("CLPOS"))
        CLMAX = Integer.Parse(SDK.GetInfo("CLMAX"))
        CLTOP = Integer.Parse(SDK.GetInfo("CLTOP"))
        CLLINES = Integer.Parse(SDK.GetInfo("CLLINES"))
        CLIMG = Integer.Parse(SDK.GetInfo("CLIMG"))
        'dialNumber = SDK.GetInfo("CLTEXT")

        SDK.Execute("SETVAR;MOBILEPHONE_PHOTOPATH;" & sPhotoPath & dialNumber & "." & sPhotoExtension)
    End Sub
    Private Sub PC_SaveCurrentPhoneBook()
        SDK.Execute("CLSAVE;" & sPhonebookpath & "MobilePhone_" & sPhonebooktype & ".txt")
    End Sub
    Private Sub PC_LoadPhoneBook(ByVal type As String)
        sPhonebooktype = type
        SDK.Execute("CLCLEAR;ALL||CLLOAD;" & sPhonebookpath & "MobilePhone_" & sPhonebooktype & ".txt")
    End Sub

    Private Function CheckRRSkinName() As String
        Dim RRSkin() As String = Split(Path.GetDirectoryName(SDK.GetInfo("RRSKIN")), "\")
        'MessageBox.Show(RRSkin.Length)
        CheckRRSkinName = RRSkin(RRSkin.Length - 1)
        Return CheckRRSkinName
    End Function

    Private Sub AddCustomList(ByVal CustomList As String, ByVal Number As String, ByVal Name As String, ByVal Icon As String)
        Dim strPath() As String
        Dim lngIndex As Long
        strPath = Split(Icon, "\")  'Put the Parts of our path into an array
        lngIndex = UBound(strPath)
        Icon = MainPath & sPhotoPath & strPath(lngIndex) 'Get the File Name from our array
        Dim objFSO, objFile
        objFSO = CreateObject("Scripting.FileSystemObject")
        'Opening the file in UNICODE
        objFile = objFSO.OpenTextFile(CustomList, 8, True, -1)
        'Writing some data into the file
        objFile.WriteLine("LST" & Number & "||" & Name)
        objFile.WriteLine("ICO" & Icon)
        'Closing the file
        objFile.Close()
    End Sub

    'DeleteLine "c:\test.txt", "", 3, 0
    Private Sub DeleteCustomList(strFile, strkey, LineNumber, CheckCase)
        'DeleteLine Function by TomRiddle 2008
        'Remove line(s) containing text (strKey) from text file (strFile)
        'or
        'Remove line number from text file (strFile)
        'or
        'Remove line number if containing text (strKey) from text file (strFile)
        'Use strFile = "c:\file.txt"  (Full path to text file)
        'Use strKey = "John Doe"      (Lines containing this text string to be deleted)
        'Use strKey = ""              (To not use keyword search)
        'Use LineNumber = "1"         (Enter specific line number to delete)
        'Use LineNumber = "0"         (To ignore line numbers)
        'Use CheckCase = "1"          (For case sensitive search )
        'Use CheckCase = "0"          (To ignore upper/lower case characters)
        Const ForReading = 1 : Const ForWriting = 2
        Dim objFSO, objFile
        Dim strLineCase = "", strNewFile = "", strLine
        objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(strFile, ForReading, True, -1) 'Set objFile = objFSO.OpenTextFile(CustomList, 8, True, -1)
        Do Until objFile.AtEndOfStream
            strLine = objFile.Readline
            If CheckCase = 0 Then strLineCase = UCase(strLine) : strkey = UCase(strkey)
            If LineNumber = objFile.Line - 1 Or LineNumber = 0 Then
                If InStr(strLine, strkey) Or InStr(strLineCase, strkey) Or strkey = "" Then
                    strNewFile = strNewFile
                Else
                    strNewFile = strNewFile & strLine & vbCrLf
                End If
            Else
                strNewFile = strNewFile & strLine & vbCrLf
            End If
        Loop
        objFile.Close()
        objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(strFile, ForWriting, True, -1)
        objFile.Write(strNewFile)
        objFile.Close()
    End Sub

    Private Sub ModifyCustomList(ByVal CustomList As String, ByVal StringToMofify As String, ByVal CLText As String, CLDescription As String, CLImg As String)
        Dim strPath() As String
        Dim lngIndex As Long
        strPath = Split(CLImg, "\")
        lngIndex = UBound(strPath)
        CLImg = MainPath & "Photo\" & strPath(lngIndex)

        Const ForReading = 1 : Const ForWriting = 2
        Dim objFSO, objFile, allLines, arrLines, ReturnedLineNumberForString
        ReturnedLineNumberForString = (Val(SDK.GetUserVar("PBCLPOS")) * 2) - 1
        SDK.SetUserVar("StringToMofify", StringToMofify & " --> " & ReturnedLineNumberForString)
        objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(CustomList, ForReading, True, -1)
        allLines = objFile.ReadAll
        'MsgBox allLines.Line 'number of lines
        arrLines = Split(allLines, vbCrLf)
        arrLines(ReturnedLineNumberForString - 1) = "LST" & CLText & "||" & CLDescription
        arrLines(ReturnedLineNumberForString) = "ICO" & CLImg
        objFile.Close()
        objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(CustomList, ForWriting, True, -1)

        Dim MyLine
        For Each MyLine In arrLines
            objFile.Write(MyLine & vbCrLf)
        Next

        objFile.Close()

    End Sub

#End Region

#Region "Speech recognition (number or doctation)"
    Private Sub InitSpeechReco()
        'Init Speech Recognition
        Select Case GetOSName()
            Case OsNames.Windows8, OsNames.Windows7, OsNames.WindowsVista, OsNames.Windows10
                Try
                    ' j’active ici mes contrôles pour Windows 8 , 8.1 , 7 et Vista (add of Window10 to test)
                    If TempPluginSettings.PhoneSpeechRecognition = True Then
                        If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone.skin" Then
                            speechrecognition = New ClassSpeechRecognition("number")
                            speechrecognition.SpeechProcessing_Load()
                        ElseIf LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smswrite.skin" Then
                            speechrecognition = New ClassSpeechRecognition("dictation")
                            speechrecognition.SpeechDictationProcessing_Load()
                        End If
                    End If
                Catch ex As Exception
                    MessageBox.Show("Speech Recognition Error !!!" & vbCrLf & vbCrLf & ex.Message)
                End Try

            Case OsNames.WindowsXP
                ' j’active ici mes contrôles pour Windows XP
                'Try
                '    speechrecognition = New ClassSpeechRecognition()
                'Catch ex As Exception
                '    MsgBox(ex.Message)
                'End Try
        End Select
    End Sub

    Private Sub StopSpeechReco()
        If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone.skin" Then
            speechrecognition = New ClassSpeechRecognition("number")
            speechrecognition.SpeechProcessing_Stop()
        ElseIf LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smswrite.skin" Then
            speechrecognition = New ClassSpeechRecognition("dictation")
            speechrecognition.DictationProcessing_Stop()
        End If
    End Sub
#End Region

#Region "Debug Log"
    Public Sub ToLog(ByVal TheMessage As String)
        If TempPluginSettings.PhoneDebugLog = True Then
            Dim DebugLogFile As StreamWriter
            If Not File.Exists(DebuglogPath) Then
                DebugLogFile = New StreamWriter(DebuglogPath)
            Else
                DebugLogFile = File.AppendText(DebuglogPath)
            End If
            ' Write to the file:
            DebugLogFile.WriteLine(DateTime.Now + "-->" & TheMessage)
            ' Close the stream:
            DebugLogFile.Close()
        End If
    End Sub

#End Region

#Region "Recherche Devices avec BlueSoleil"
    Private Sub Search_Device() 'Creation d'une list de téléphones appairés
        If File.Exists(MainPath & "MobilePhone_DevicesList.lst") Then File.Delete(MainPath & "MobilePhone_DevicesList.lst")

        SearchThread = New System.Threading.Thread(AddressOf Search_Bluetooth_Device)
        SearchThread.Priority = Threading.ThreadPriority.Normal
        SearchThread.Start()
    End Sub
    Private Sub Search_Bluetooth_Device()
        bluetooth.BlueSoleil_HandsFree_GetDevicesList()
        SDK.Execute("CLLOAD;" & MainPath & "MobilePhone_DevicesList.lst||CLPOS;1")
        SDK.SetUserVar("MOBILEPHONE_INFO", "The device's list is ready !!!")
    End Sub
#End Region

#Region "Paired Unpaired Device"
    Private Sub Paired_Device() 'Creation d'une list de téléphones appairés
        SearchThread = New System.Threading.Thread(AddressOf Paired_Bluetooth_Device)
        SearchThread.Priority = Threading.ThreadPriority.Normal
        SearchThread.Start()
    End Sub
    Private Sub Paired_Bluetooth_Device()
        bluetooth.BlueSoleil_PairedUnpairedDelete(CInt(SDK.GetInfo("CLPOS")) - 1, 1, SDK.GetUserVar("MOBILEPHONE_PINCODE"))
        'bluetooth.BlueSoleil_PairedUnpairedDelete(CInt(SDK.GetInfo("CLPOS")) - 1, 1, "1234")
    End Sub
#End Region

#Region "Recherche Services accepté par le Tel"
    ''' <summary>
    ''' Extract Services accepted by a phone
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Search_Services() 'Creation d'une liste de téléphones appairés
        If File.Exists(MainPath & "MobilePhone_Services.lst") Then File.Delete(MainPath & "MobilePhone_Services.lst")
        SDK.SetUserVar("MOBILEPHONE_INFO", "The Services's List for the phone --> " & SDK.GetInfo("MOBILEPHONE_MODEL") & " services  is updated, please wait !!!")

        SearchThread = New System.Threading.Thread(AddressOf Search_Bluetooth_Services)
        SearchThread.Priority = Threading.ThreadPriority.Normal
        SearchThread.Start()
    End Sub
    Private Sub Search_Bluetooth_Services()
        bluetooth.GetServicesList()
        SDK.Execute("MENU;MOBILEPHONE_INFO.skin||CLCLEAR;ALL||CLLOAD;" & MainPath & "MobilePhone_Services.lst")
    End Sub

#End Region

#Region "Liste Indicatifs"

    ''' <summary>
    ''' Read the dictionnary CountryList
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CountryToPhoneIndicatifList()
        SDK.Execute("SETVAR;MOBILEPHONE_INFO;Phone Country Prefix||MENU;MOBILEPHONE_INFO.SKIN||CLCLEAR;ALL")
        CountryList.Clear()
        CreateCountryList()
        For Each pair In CountryList
            SDK.Execute("CLADD;" & pair.Key & " -- > " & pair.Value.ToString)
        Next
        CountryList.Clear()
    End Sub
#End Region


End Class
