Option Strict Off
Option Explicit On

Imports System.Speech
Imports System.Speech.Synthesis
Imports System.Globalization
Imports System.Reflection
Imports System.Text
Imports MobilePhone.PhoneNumberFormatter
Imports System.ComponentModel
Imports System.Threading
Imports System.Collections.Generic

'Imports PhoneNumbers
'Imports PhoneNumbers.PhoneNumberUtil

'*****************************************************************
' Every Plugin MUST have its OWN set of GUIDs. use Tools->Create GUID
'*****************************************************************

<ComClass("E99D1ADB-E87B-4921-BA7F-86A8869E9BEB", "1D06FA29-0866-491B-97CD-7DCF15BB9C42")> _
Public Class RRExtension
    Dim RunOnce As Boolean ' set to prevent a double execution of code
    Dim SDK As RRSDK ' set type of var to the subclass

    Dim WithEvents speechrecognition As ClassSpeechRecognition


    'Dim IconPath() As String
    Dim sPhonebookpath As String
    Dim sPhotoPath As String
    Dim vcarIdPh As Integer = 0
    Dim vcarIdPhOther As Integer = 0
    Dim vcarIdAdd As Integer = 0
    Dim vcarIdAddOther As Integer = 0
    Dim NewVcarId As Integer = 0, NewVcarIdOther As Integer = 0
    Dim msgId As Integer = 0
    Dim vcardInPBList As Boolean = True
    Dim vcardInPBListString As String = ""
    Dim favoriteSkinindicatorsPath As String = ""
    Dim ThemeIconsPath As String = ""
    Dim sPhotoExtension As String
    Dim favoriteVcardID As Integer = 0
    Dim newPhones(0 To 2) As String, newAddress(0 To 2) As String, newLabels(0 To 2) As String, newPhonesCount As Integer = 0

    Private Const SRCActiveScreen As String = "MOBILEPHONE_PLAYER.skin"
    'Private Const MainScreen As String = "MOBILEPHONE_PLAYER.skin"

    'Private threadRunBluesoleil As Thread = New Thread(New ThreadStart(AddressOf InitHfpAndMapService))
    Private WithEvents bgw As BackgroundWorker
    Private tbProgress_Text As String = ""

    Dim OldMainVolume As String = ""
    Dim OldPlayerStatus As String = ""


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

    Private Sub BlinkIndicatorOn() 'Handles bluetooth.SetIndBlinkOn
        '"SetIndBlink;MUTE;true"             - adds ":b" from indicator MUTE on current screen.
        '"SetIndBlink;MUTE;false"            - del ":b" from indicator MUTE on current screen.
        SDK.Execute("SetIndBlink;mobilephone_synchronizing;true", True)
    End Sub
    Private Sub BlinkIndicatorOff() 'Handles bluetooth.SetIndBlinkOff
        '"SetIndBlink;MUTE;true"             - adds ":b" from indicator MUTE on current screen.
        '"SetIndBlink;MUTE;false"            - del ":b" from indicator MUTE on current screen.
        SDK.Execute("SetIndBlink;mobilephone_synchronizing;false", True)
    End Sub
    Private Sub bsHandler_HFP_NetworkAvailable() 'phoneNetworkAvail() 'Handles bluetooth.PhoneNetAvail
        hfpNetworkAvailable = True
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONENETAVAIL", True)
        Else
            SDK.Execute("ONMOBILEPHONENETAVAIL", True)
        End If
    End Sub
    Private Sub bsHandler_HFP_NetworkUnavailable() 'phoneNetworkUnAvail() 'Handles bluetooth.PhoneNetUnAvail
        hfpNetworkAvailable = False
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONENETUNAVAIL", True)
        Else
            SDK.Execute("ONMOBILEPHONENETUNAVAIL", True)
        End If
    End Sub

    Private Sub bsHandler_HFP_CallerID(ByVal phoneNo As String, ByVal phoneName As String)
        hfpCallerIDno = phoneNo
        hfpCallerIDname = phoneName
        ToLog("bsHandler_HFP_CallerIDreturn : hfpCallerIDno = " & hfpCallerIDno & " and hfpCallerIDname = " & hfpCallerIDname)
    End Sub
    Private Sub bsHandler_HFP_Ringing() 'phoneRinging() 
        hfpStatusStr = "Ringing"
        hfpCallerIDnoFormatted = New PhoneNumberFormatter(TempPluginSettings.PhoneCountryCodes(0), TempPluginSettings.PhoneCountryCodes(1), TempPluginSettings.PhoneCountryCodes(2))
        hfpCallerIDno = hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(hfpCallerIDno, OutputFormats.National)
        'MessageBox.Show("search number: " & hfpCallerIDno)

        Try
            'OldPlayerStatus = SDK.GetInfo("STATUS")
            OldMainVolume = SDK.GetInfo("VOLUME").Replace("%", "")

            'load contact from the pb.vcf file
            Bluesoleil_LoadContactsFile()
            Thread.Sleep(500)

            'if the contact id exist into the black list, the plugin auto hungup
            If TempPluginSettings.AutoNotAnswerCallIn = True Then
                If PluginRunForDS = False Then
                    If CheckContactInBlackList(hfpCallerIDno) = True Then
                        SDK.Execute("MOBILEPHONE_HANGUP||*ONMOBILEPHONEAUTOHANGUP||SETVARFROMVAR;POPUP;l_set_BTM_IsInBlackList||MENU;POPUP.SKIN", True) 'reject automatically the caller
                    End If
                    ToLog("RR return the number " & hfpCallerIDno & " is in blacklist ????")
                    Exit Sub
                Else 'erreur avec iCarDS et CheckContactInBlackList ????
                    If CheckContactInBlackList(hfpCallerIDno) = True Then
                        SDK.Execute("MOBILEPHONE_HANGUP||ONMOBILEPHONEAUTOHANGUP||SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_IsInBlackList||POPUP;ReadingPhoneBook.SKIN;5", True) 'reject automatically the caller
                    End If
                    ToLog("iCarDS return the number " & hfpCallerIDno & " is in blacklist ????")
                    Exit Sub
                End If
            End If

            If PluginRunForDS = False Then
                ToLog("RR return number " & hfpCallerIDno & " isn't in blacklist")
            Else
                ToLog("iCarDS return number " & hfpCallerIDno & " isn't in blacklist")
            End If

            SDK.Execute("SETVAR;NEWCONTACT;" & hfpCallerIDname, True)

            If File.Exists(MainPath & "PhoneBook\pb.vcf") = True Then
                hfpCallerIDname = BlueSoleil_PBAP_GetNameFromNumber(hfpCallerIDno)

                Dim hfpCallerIDImage As String = "" 'pour laisser le temps à la récupération du nom de l'image
                hfpCallerIDImage = MainPath & "Photo\" & hfpCallerIDno & ".jpg" 'BlueSoleil_BS_PBAP_GetImageFromNumber(hfpCallerIDno)

                'MessageBox.Show(hfpCallerIDImage)
                'Dim newGetVcardId As Integer = BlueSoleil_PBAP_GetVcardId(hfpCallerIDno)
                If File.Exists(hfpCallerIDImage) Then
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & hfpCallerIDImage & "||WAIT;0.5||MENU;MOBILECALL.skin||*ONMOBILEPHONERINGING||PAUSE||RELOADSCREEN", True)
                    Else
                        SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & hfpCallerIDImage & "||WAIT;0.5||MENU;MOBILECALL.skin||ONMOBILEPHONERINGING||PAUSE||RELOADSCREEN", True)
                    End If
                    ToLog("pb.vcf file exist and picture found for the number " & hfpCallerIDno)
                Else
                    SDK.Execute("*ONMOBILEPHONERINGING||PAUSE||MENU;MOBILECALL.skin", True)
                End If

            Else
                hfpCallerIDname = SDK.GetInfo("=$l_set_BTM_Unknown$") '"Unknow"
                If PluginRunForDS = False Then
                    SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\unknow.gif||WAIT;0.5||MENU;MOBILECALL.skin||*ONMOBILEPHONERINGING||PAUSE||RELOADSCREEN", True)
                Else
                    SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\unknow.gif||WAIT;0.5||MENU;MOBILECALL.skin||ONMOBILEPHONERINGING||PAUSE||RELOADSCREEN", True)
                End If
                If File.Exists(MainPath & "PhoneBook\pb.vcf") = True Then ToLog("pb.vcf file not exist")
                If BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDno) = True Then ToLog("no vcard for this number " & hfpCallerIDno)
            End If

            'launch the ringin loop
            timer3.Enabled = True

            ToLog("bsHandler_HFP_Ringing : hfpCallerIDno = " & hfpCallerIDno & " and hfpCallerIDname = " & hfpCallerIDname)

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Error bsHandler_HFP_Ringing")
            ToLog("Error in bsHandler_HFP_Ringing")
        End Try
    End Sub
    Private Sub bsHandler_HFP_OngoingCall() 'réception d'un appel
        hfpStatusStr = "Ongoing Call"
        'OldPlayerStatus = SDK.GetInfo("STATUS")

        pbapReturnFromCall = "CallIn" 'return what phone book will be updated
        pbapListNeedUpdate = True

        'Pause Audio
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("SETVARBYCODE;OLDVOLUME;VOLUME||*ONMOBILEPHONEINCALL", True)
        Else
            SDK.Execute("SETVARBYCODE;OLDVOLUME;VOLUME||ONMOBILEPHONEINCALL", True)
        End If

    End Sub
    Private Sub bsHandler_HFP_OutgoingCall() 'création d'un appel
        'Remove Call Screen
        hfpStatusStr = "Outgoing Call"
        'OldPlayerStatus = SDK.GetInfo("STATUS")

        pbapReturnFromCall = "CallOut" 'return what phone book will be updated
        pbapListNeedUpdate = True

        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("SETVARBYCODE;OLDVOLUME;VOLUME||*ONMOBILEPHONEOUTCALL", True)
        Else
            SDK.Execute("SETVARBYCODE;OLDVOLUME;VOLUME||ONMOBILEPHONEOUTCALL", True)
        End If

    End Sub
    Private Sub bsHandler_HFP_MissedCall() 'appel refusé
        pbapReturnFromCall = "Missed" 'return what phone book will be updated
        pbapListNeedUpdate = True
    End Sub

    Private Sub bsHandler_HFP_StandBy() 'mise en attente
        hfpStatusStr = "StandBy"

        'stop the ringin loop
        timer3.Enabled = False

        'If LCase(SDK.GetInfo("RRSCREEN")) = "mobilecall.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilecall" Then
        '    SDK.Execute("ESC", True)
        'End If

        IncomingCall = False

        Thread.Sleep(500)

        'if last contact isn't in contact's list we can save or not this new contact into the black list
        If hfpCallerIDno <> "" Then
            If File.Exists(MainPath & "PhoneBook\pb.vcf") = True Then 'And commenté pour pb + deviennant 0
                '(BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDno) = False Or
                'BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(hfpCallerIDno, OutputFormats.National))) = False Then
                'ToLog("Number'" & hfpCallerIDno & " or '" & hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(hfpCallerIDno, OutputFormats.National) & "' isn't found ! (Add contact code)")
                ToLog("Number'" & hfpCallerIDno & " or '" & hfpCallerIDno & "' isn't found ! (Add contact code)")
                If TempPluginSettings.PhoneNoAddContact = False Then
                    If PluginRunForDS = False Then
                        SDK.Execute("MENU;MOBILEPHONE_ADDCONTACT.SKIN", True)
                    Else
                        SDK.Execute("POPUP;MOBILEPHONE_ADDCONTACT.SKIN;5", True)
                    End If
                    ToLog("'PhoneNoAddContact' option is off")
                End If
            End If
        End If

        If TempPluginSettings.PhoneBookAutoUpdate = True AndAlso ServicePBAP_Usable = True AndAlso pbapListNeedUpdate = True Then
            Select Case pbapReturnFromCall
                Case "CallIn" 'when then phone return from incall, ich.vcf vcard file is updated
                    Dim t As New Threading.Thread(Sub() RRVcardUpdateProcess(False), "ICH") 'no event is send to RR after the update
                    t.Start()
                Case "CallOut" 'when then phone return from outcall, och.vcf vcard file is updated
                    Dim u As New Threading.Thread(Sub() RRVcardUpdateProcess(False), "OCH") 'no event is send to RR after the update
                    u.Start()
                Case "Missed" 'when then phone return from outcall, och.vcf vcard file is updated
                    Dim v As New Threading.Thread(Sub() RRVcardUpdateProcess(False), "MCH") 'no event is send to RR after the update
                    v.Start()
            End Select
            'compiled and missed are always updated
            Dim w As New Threading.Thread(Sub() RRVcardUpdateProcess(False), "CCH") 'no event is send to RR after the update
            w.Start()
            pbapReturnFromCall = ""
        End If

        'Unpause Audio
        If OldMainVolume <> "" Then 'évite la mise à zéro du volume au démarrage du plugin!
            If PluginRunForDS = False Then
                If OldPlayerStatus = "PLAY" Then
                    SDK.Execute("SETVOL;MASTER;" & OldMainVolume & "||RESUME||*ONMOBILEPHONEHUNGUP", True)
                Else
                    SDK.Execute("SETVOL;MASTER;" & OldMainVolume & "||*ONMOBILEPHONEHUNGUP||*ONMOBILEPHONEALLRESUME", True)
                End If
            Else
                If OldPlayerStatus = "PLAY" Then
                    SDK.Execute("RESUME||ONMOBILEPHONEHUNGUP", True)
                Else
                    SDK.Execute("ONMOBILEPHONEHUNGUP||ONMOBILEPHONEALLRESUME", True)
                End If
            End If
        End If

    End Sub

    Dim CurrentCallInfo As String = "", CurrentCallName As String = "", CurrentCallNumber As String = ""
    Private Sub bsHandler_HFP_CurrentCallInfo(ByVal phoneNo As String, ByVal callIsIncoming As Boolean, ByVal callIsFax As Boolean, ByVal callIsData As Boolean, ByVal callIsMultiParty As Boolean)
        CurrentCallInfo = phoneNo & ", " & callIsIncoming.ToString & ", " & callIsFax.ToString & ", " & callIsData.ToString & ", " & callIsMultiParty.ToString

        CurrentCallNumber = phoneNo
        If File.Exists(MainPath & "PhoneBook\pb.vcf") = True AndAlso BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDno) = True Then
            CurrentCallName = BlueSoleil_PBAP_GetNameFromNumber(hfpCallerIDno)
        Else
            CurrentCallName = SDK.GetInfo("=$l_set_BTM_Unknown$") '"Unknow"
        End If
        ToLog("CurrentCallInfo return hfpCallerIDname=" & CurrentCallName)
        ToLog("CurrentCallInfo return CurrentCallInfo=" & CurrentCallInfo)
    End Sub


    Private Sub bsHandler_HFP_NetworkName(ByVal networkName As String)
        Dim p1 As Integer = InStr(1, networkName, Chr(0))
        If p1 > 0 Then Exit Sub
        hfpNetworkName = networkName
    End Sub
    Private Sub bsHandler_HFP_ModelName(ByVal modelName As String)
        hfpModelName = modelName
    End Sub
    Private Sub bsHandler_HFP_ManufacturerName(ByVal manuName As String)
        manuName = manuName.Trim
        If manuName.Contains("+CGMI: ") Then
            manuName = manuName.Replace("+CGMI: ", "")
            manuName = manuName.ToLower
        End If
        hfpManufacturerNameParsed = UCase(Left(manuName, 1)) & Mid(manuName, 2) 'manuName
    End Sub
    Private Sub bsHandler_HFP_ConnectionReleased()
        '!
        ToLog("Connection HFP is released !")
    End Sub
    Private Sub bsHandler_HFP_SubscriberNo(ByVal phoneNo As String, ByVal phoneName As String)
        hfpSubscriberNo = phoneNo
        hfpSubscriberName = phoneName
    End Sub
    'Private Sub bsHandler_HFP_BatteryCharge(ByVal batteryPct As Double)
    '    hfpBatteryPct = batteryPct
    '    If hfpBatteryPctOld = 0 Then hfpBatteryPctOld = batteryPct
    'End Sub
    'Private Sub bsHandler_HFP_SignalQuality(ByVal signalPct As Double)
    '    hfpSignalPct = signalPct
    'End Sub
    Private Sub bsHandler_HFP_MicVolume(ByVal volumePct As Double)
        hfpMicVolumePct = volumePct
    End Sub
    Private Sub bsHandler_HFP_SpeakerVolume(ByVal volumePct As Double)
        hfpSpeakerVolumePct = volumePct
        'Dim tempVolInt As Integer = CInt(TrackBar1.Maximum * hfpSpeakerVolumePct / 100)
        'If tempVolInt < 0 Then tempVolInt = 0
        'If tempVolInt > TrackBar1.Maximum Then tempVolInt = TrackBar1.Maximum
        'TrackBar1.Value = tempVolInt

    End Sub

    Private Sub bsHandler_HFP_StartRoaming()
        If PluginRunForDS = False Then
            If hfpIsRoaming = False Then SDK.Execute("*ONMOBILEPHONESTARTROAMING", True)
        Else
            If hfpIsRoaming = False Then SDK.Execute("ONMOBILEPHONESTARTROAMING", True)
        End If
        hfpIsRoaming = True
    End Sub

    Private Sub bsHandler_HFP_EndRoaming()
        If PluginRunForDS = False Then
            If hfpIsRoaming = True Then SDK.Execute("*ONMOBILEPHONEENDROAMING", True)
        Else
            If hfpIsRoaming = True Then SDK.Execute("ONMOBILEPHONEENDROAMING", True)
        End If
        hfpIsRoaming = False
    End Sub

    Private Sub voiceActivation()
        Thread.Sleep(100)
        SDK.Execute("PAUSE||*ONMOBILEPHONEVOICEON", True)
    End Sub
    Private Sub voiceDeactivation()
        Thread.Sleep(100)
        SDK.Execute("RESUME||*ONMOBILEPHONEVOICEOFF", True)
    End Sub

    'external power alarm events
    Private Sub battisFullCharge()
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONEBATTERYFULLCHARGE", True)
        Else
            SDK.Execute("ONMOBILEPHONEBATTERYFULLCHARGE", True)
        End If
    End Sub
    Private Sub alarmExternalPowerOn()
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONEEXTPOWERON", True)
        Else
            SDK.Execute("ONMOBILEPHONEEXTPOWERON", True)
        End If
    End Sub
    Private Sub alarmExternalPowerOff()
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONEEXTPOWEROFF", True)
        Else
            SDK.Execute("ONMOBILEPHONEEXTPOWEROFF", True)
        End If
    End Sub


    Private Sub bsHandler_HFP_SecondPhoneConnected(ByVal phoneName As String)
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("SETVAR;POPUP;Second Phone '" & phoneName & "' is Connected !||MENU;POPUP.SKIN", True)
        Else
            SDK.Execute("SETVAR;l_ReadingPhoneBook;Second Phone '" & phoneName & "' is Connected !||POPUP;ReadingPhoneBook.SKIN;5", True)
        End If
    End Sub
    'first or second selected phones events
    Private Sub firstPhoneNotFound()
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONE1FOUND", True)
        Else
            SDK.Execute("ONMOBILEPHONE1FOUND", True)
        End If
    End Sub
    Public Sub secondPhoneNotFound()
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONE2FOUND", True)
        Else
            SDK.Execute("ONMOBILEPHONE2FOUND", True)
        End If
    End Sub

    'PBAPIsReady (phone books)
    Private Sub bsHandler_PBAP_ContactsListUsable()
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("SETVARFROMVAR;POPUP;l_set_BTM_ContactsListReady||MENU;POPUP.SKIN||MOBILEPHONE_PB||*ONMOBILEPHONEPBAPISREADY", True)
        Else
            SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_ContactsListReady||POPUP;ReadingPhoneBook.SKIN;5||MOBILEPHONE_PB||ONMOBILEPHONEPBAPISREADY", True)
        End If
    End Sub

    Private Sub bsHandler_PBAP_IsBirthDay(ByVal count As Integer)
        Thread.Sleep(100)
        'moved into RequestInfo()

    End Sub

    'SMS
    Private Sub bsHandler_MAP_MsgIsSend(ByVal name As String, ByVal number As String)
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONESMSISSEND||SETVAR;MOBILEPHONE_SMSINFO;Message send to '" & name & "' !!!", True)
        Else
            SDK.Execute("ONMOBILEPHONESMSISSEND||SETVAR;MOBILEPHONE_SMSINFO;Message send to '" & name & "' !!!", True)
        End If
    End Sub
    'Private Sub bsHandler_MAP_MsgNotification()
    '    'MessageBox.Show("New Msg Received!")

    '    mapMsgPopUpAllreadySend = False ' reset the SMS popup
    '    btnGetMessages(True)
    '    ToLog("!!! New message NOTIFICATION !!!")
    'End Sub
    Private Sub bsHandler_MAP_MsgIsReceived(ByVal name As String, ByVal number As Integer, ByVal subject As String, ByVal attach As Boolean)
        'MessageBox.Show("name: " & name & vbCrLf &
        '                "number: " & number & vbCrLf &
        '                "subject: " & subject & vbCrLf &
        '                "attach: " & attach.ToString)
        OldMainVolume = SDK.GetInfo("VOLUME").Replace("%", "")
        OldPlayerStatus = SDK.GetInfo("STATUS")

        If number > 0 Then mapMsgReceived = True
        If number <> MsgNumberOfNewUnread Then
            MsgNumberOfNewUnread = number
            mapMsgPopUpAllreadySend = False

        End If

        If mapMsgReceived = True Then
            msgId = 0 'raz msg id entre chaque check
            If PluginRunForDS = False Then
                If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smsread.skin" Then SDK.Execute("CLCLEAR;ALL||MOBILEPHONE_SMSVIEW||SETVAR;MOBILEPHONE_SMSINFO;")
            Else
                If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smsread" Then SDK.Execute("CLCLEAR;ALL||MOBILEPHONE_SMSVIEW||SETVAR;MOBILEPHONE_SMSINFO;")
            End If
        End If

        'stop MAP popup
        If IncomingCall = True Or TempPluginSettings.PhoneNoSMSPopupInfo = True Or SmsUnreadOnly = False Or mapMsgPopUpAllreadySend = True Then
            If IncomingCall = True Then ToLog("IncomingCall stop SMS popup")
            If TempPluginSettings.PhoneNoSMSPopupInfo = True Then ToLog("PhoneNoSMSPopupInfo stop SMS popup")
            If SmsUnreadOnly = False Then ToLog("SmsUnreadOnly stop SMS popup") 'stop sms popup if all sms selcted
            If mapMsgPopUpAllreadySend = True Then ToLog("mapMsgPopUpAllreadySend stop SMS popup") 'stop sms popup if number of sms is same
            If PluginRunForDS = False Then
                SDK.Execute("*ONMOBILEPHONESMSISRECEIVED")
            Else
                SDK.Execute("ONMOBILEPHONESMSISRECEIVED")
            End If
            Exit Sub
        End If

        'If mapMsgPopUpAllreadySend = False Then
        If name = "****" Then
            name = SDK.GetInfo("=$l_set_BTM_Unknown$")
        End If
        'msgAttachExist = attach
        If PluginRunForDS = False Then
            Dim addIMG As String = IIf(attach = True, SkinPath & "Indicators\ATTACH.png", "")
            If number = 1 Then
                Dim info As String = SDK.GetInfo("=$l_set_BTM_NewSMSFrom$") & " " & name & " !!!"
                SDK.Execute("*ONMOBILEPHONESMSISRECEIVED||PAUSE||SETVAR;POPUP;" & info &
                            "||MENU;POPUP.SKIN||SAY;$l_set_BTM_NewMessageSentence$||WAIT;2||SETVOL;MASTER;" & OldMainVolume, True)
            ElseIf number > 1 Then
                SDK.Execute("*ONMOBILEPHONESMSISRECEIVED||PAUSE||SETVAR;POPUP;$l_set_BTM_NewSMS$ !!!" &
                            "||MENU;POPUP.SKIN||SAY;$l_set_BTM_NewMessageSentence$||WAIT;2||SETVOL;MASTER;" & OldMainVolume, True)
            End If
        Else
            If number = 1 Then
                Dim info As String = SDK.GetInfo("=$l_set_BTM_NewSMSFrom$") & " " & name & " !!!"
                SDK.Execute("ONMOBILEPHONESMSISRECEIVED||PAUSE||SETVAR;l_ReadingPhoneBook;" & info &
                            "||POPUP;ReadingPhoneBook.SKIN;5||SAY;$l_set_BTM_NewMessageSentence$||WAIT;2||SETVOL;MASTER;" & OldMainVolume, True)
            ElseIf number > 1 Then
                SDK.Execute("ONMOBILEPHONESMSISRECEIVED||PAUSE||SETVAR;l_ReadingPhoneBook;$l_set_BTM_NewSMS$ !!!" &
                            "||POPUP;ReadingPhoneBook.SKIN;5||SAY;$l_set_BTM_NewMessageSentence$||WAIT;2||SETVOL;MASTER;" & OldMainVolume, True)
            End If

        End If

        If OldMainVolume <> "" Then 'évite la mise à zéro du volume au démarrage du plugin!
            If OldPlayerStatus = "PLAY" Then
                SDK.Execute("RESUME", True)
            Else
                If PluginRunForDS = False Then
                    SDK.Execute("*ONMOBILEPHONEALLRESUME", True)
                Else
                    SDK.Execute("ONMOBILEPHONEALLRESUME", True)
                End If
            End If
        End If
        mapMsgPopUpAllreadySend = True ' reset the SMS popup
        'End If

    End Sub
    'SMS

    'AVRCP
    Private Sub avrcpConnect()
        Thread.Sleep(100)
        OldMainVolume = SDK.GetInfo("VOLUME").Replace("%", "")
        If PluginRunForDS = False Then
            SDK.Execute("PAUSE||*ONMOBILEPHONEAVRCPON||SETVOL;MASTER;100||SETVAR;POPUP;Player is Ready||MENU;POPUP.SKIN", True)
        Else
            SDK.Execute("PAUSE||ONMOBILEPHONEAVRCPON||SETVAR;l_ReadingPhoneBook;Player is Ready||POPUP;ReadingPhoneBook.SKIN;5", True)
        End If
    End Sub
    Private Sub avrcpDisConnect()
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("RESUME||SETVOL;MASTER;" & OldMainVolume & "||*ONMOBILEPHONEAVRCPOFF", True)
        Else
            SDK.Execute("RESUME||SETVOL;MASTER;" & OldMainVolume & "||ONMOBILEPHONEAVRCPOFF", True)
        End If
    End Sub
    Private Sub bsHandler_AVRCP_TrackAlbum(ByVal newAlbum As String)
        avrcpTrackAlbum = newAlbum
    End Sub
    Private Sub bsHandler_AVRCP_TrackArtist(ByVal newArtist As String)
        avrcpTrackArtist = newArtist
    End Sub
    Private Sub bsHandler_AVRCP_TrackTitle(ByVal newTitle As String)
        avrcpTrackTitle = newTitle
    End Sub
    Private Sub bsHandler_AVRCP_TrackChanged()
        avrcpTrackPosUpdateTicks = 0
        avrcpNeedsTrackInfo = True
        avrcpNeedsPlayStatus = True
    End Sub
    Private Sub bsHandler_AVRCP_PlayStatusInfo(ByVal trackLenSec As Double, ByVal trackPosSec As Double, ByVal isPlaying As Boolean)
        Dim prevTrackLen As Double = avrcpTrackLen
        Dim prevPlaying As Boolean = avrcpIsPlaying

        If trackLenSec <> -1 And trackPosSec <> -1 Then
            avrcpTrackLen = trackLenSec
            avrcpTrackPos = trackPosSec
            avrcpTrackPosUpdateTicks = (DateTime.UtcNow.Ticks \ TimeSpan.TicksPerMillisecond)
        End If
        avrcpIsPlaying = isPlaying
        ToLog("avrcpTrackPos = " & avrcpTrackPos.ToString)
        ToLog("avrcpIsPlaying = " & avrcpIsPlaying.ToString)
        'Private Const BTSDK_AVRCP_PLAYSTATUS_STOPPED As Byte = &H0
        'Private Const BTSDK_AVRCP_PLAYSTATUS_PLAYING As Byte = &H1
        'Private Const BTSDK_AVRCP_PLAYSTATUS_PAUSED As Byte = &H2
        If avrcpIsPlaying = True Then
            AVRCP_status = "play"
        Else
            AVRCP_status = "pause"
        End If

        If avrcpTrackLen <> prevTrackLen Or avrcpIsPlaying <> prevPlaying Then
            avrcpNeedsPlayStatus = True
            avrcpNeedsTrackInfo = True
        End If
    End Sub
    Private Sub bsHandler_AVRCP_AbsoluteVolume(ByVal volPct As Double)
        avrcpAbsVolPct = volPct

        'AVRCP_AbsVolPct = CInt(15 * avrcpAbsVolPct / 100) 'Dim tempVolInt As Integer = CInt(15 * avrcpAbsVolPct / 100)
        Dim tempVolInt As Integer = CInt(100 * avrcpAbsVolPct / 100)
        If tempVolInt < 0 Then tempVolInt = 0
        If tempVolInt > 100 Then tempVolInt = 0
        'TrackBar2.Value = tempVolInt

    End Sub

    Private Sub bsHandler_AVRCP_BatteryStatus(ByVal isCritical As Boolean, ByVal isLow As Boolean, ByVal isNormal As Boolean, ByVal isCharging As Boolean, ByVal isFullyCharged As Boolean)

        Dim retStr As String = "Unknown"
        If isCritical = True Then retStr = "Critical"
        If isLow = True Then retStr = "Low"
        If isNormal = True Then retStr = "Normal"
        If isCharging = True Then retStr = "Charging"
        If isFullyCharged = True Then retStr = "Fully Charged"

        'avrcpBatteryStatus = retStr

    End Sub
    'AVRCP

    'Speech Recognition events
    Private Sub speechLoad() 'Handles speechrecognition.MobilePhoneSeepchRecognitionIsON
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("PAUSE||*ONMOBILEPHONESPEECHLOAD", True)
        Else
            SDK.Execute("PAUSE||ONMOBILEPHONESPEECHLOAD", True)
        End If
    End Sub
    Private Sub speechUnload() 'Handles speechrecognition.MobilePhoneSeepchRecognitionIsOFF
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("RESUME||*ONMOBILEPHONESPEECHUNLOAD", True)
        Else
            SDK.Execute("RESUME||ONMOBILEPHONESPEECHUNLOAD", True)
        End If
    End Sub
    Private Sub speechDial() 'Handles speechrecognition.MobilePhoneSeepchDial
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("SETVAR;DIALNUMBER;" & dialNumber & "||SETVARBYCODE;MOBILEPHONE_CALLERID;DIALNUMBER||MOBILEPHONE_DIAL||*ONMOBILEPHONESPEECHDIAL", True)
        Else
            SDK.Execute("SETVAR;DIALNUMBER;" & dialNumber & "||SETVARBYCODE;MOBILEPHONE_CALLERID;DIALNUMBER||MOBILEPHONE_DIAL||ONMOBILEPHONESPEECHDIAL", True)
        End If
    End Sub
    Private Sub speechHangup() 'Handles speechrecognition.MobilePhoneSeepchHangup
        'CallerName = ""
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("RESUME||*ONMOBILEPHONEHUNGUP", True)
        Else
            SDK.Execute("RESUME||ONMOBILEPHONEHUNGUP", True)
        End If
    End Sub
    Private Sub speechHelp() 'Handles speechrecognition.MobilePhoneSeepchHelp
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONESPEECHHELP", True)
        Else
            SDK.Execute("ONMOBILEPHONESPEECHHELP", True)
        End If
    End Sub
    Private Sub speechDialByName() 'Handles speechrecognition.MobilePhoneSeepchDialByName
        Thread.Sleep(100)
        Dim tmp As String = speechrecognition.speechCallByNameResult
        tmp = BlueSoleil_PBAP_GetNumberFromName(Replace(tmp, SpeechArray(18), ""))
        'MsgBox(tmp)
        If PluginRunForDS = False Then
            SDK.Execute("SETVAR;DIALNUMBER;" & dialNumber & "||SETVARBYCODE;MOBILEPHONE_CALLERID;DIALNUMBER||MOBILEPHONE_DIAL||*ONMOBILEPHONESPEECHDIAL", True)
        Else
            SDK.Execute("SETVAR;DIALNUMBER;" & dialNumber & "||SETVARBYCODE;MOBILEPHONE_CALLERID;DIALNUMBER||MOBILEPHONE_DIAL||ONMOBILEPHONESPEECHDIAL", True)
        End If
    End Sub
    Private Sub speechSupp1()
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONESPEECHSUPP1", True)
        Else
            SDK.Execute("ONMOBILEPHONESPEECHSUPP1", True)
        End If

    End Sub
    Private Sub speechSupp2()
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONESPEECHSUPP2", True)
        Else
            SDK.Execute("ONMOBILEPHONESPEECHSUPP2", True)
        End If
    End Sub
    Private Sub speechSupp3()
        Thread.Sleep(100)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONESPEECHSUPP3", True)
        Else
            SDK.Execute("ONMOBILEPHONESPEECHSUPP3", True)
        End If
    End Sub

    Private Sub bsHandler_PAN_IPchange(ByVal newIP As String)
        If newIP <> "0.0.0.0" Then
            If PluginRunForDS = False Then
                SDK.Execute("SETVAR;POPUP;$l_set_BTM_StartPANText$ IP: " & newIP & "||MENU;POPUP.SKIN", True)
            Else
                SDK.Execute("SETVAR;l_ReadingPhoneBook;$l_set_BTM_StartPANText$ IP: " & newIP & "||POPUP;ReadingPhoneBook.SKIN;5", True)
            End If
        End If

    End Sub
    Private Sub bsHandler_PAN_IsOn()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONEPANISON", True)
        Else
            SDK.Execute("ONMOBILEPHONEPANISON", True)
        End If
    End Sub
    Private Sub bsHandler_PAN_IsOff()
        panIPaddress = "0.0.0.0"
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONEPANISOFF||SETVARFROMVAR;POPUP;l_set_BTM_StopPANText||MENU;POPUP.SKIN", True)
        Else
            SDK.Execute("ONMOBILEPHONEPANISOFF||SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_StopPANText||POPUP;ReadingPhoneBook.SKIN;5", True)
        End If
    End Sub

    Private Sub bsHandler_WIFI_IsOn()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONEWIFIISON", True)
        Else
            SDK.Execute("ONMOBILEPHONEWIFIISON", True)
        End If
    End Sub
    Private Sub bsHandler_WIFI_IsOff()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONEWIFIISOFF||SETVARFROMVAR;POPUP;l_set_BTM_WifiIsLost||MENU;POPUP.SKIN", True)
        Else
            SDK.Execute("ONMOBILEPHONEWIFIISOFF||SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_WifiIsLost||POPUP;ReadingPhoneBook.SKIN;5", True)
        End If

    End Sub

    Private Sub bsHandler_LAN_IsOn()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONELANISON", True)
        Else
            SDK.Execute("ONMOBILEPHONELANISON", True)
        End If
    End Sub
    Private Sub bsHandler_LAN_IsOff()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONELANISOFF||SETVAR;MOBILEPHONE_INFO;Net Info||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||CLADD;||CLADD;||CLADD;Lan Is Lost !", True)
        Else
            SDK.Execute("ONMOBILEPHONELANISOFF||SETVAR;MOBILEPHONE_INFO;Net Info||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||CLADD;||CLADD;||CLADD;Lan Is Lost !", True)
        End If
    End Sub

    Private Sub bsHandler_HFP_ExtCmdInd(ByVal atCmdResult As String)
        ''MessageBox.Show(result)
        ''http://lgmorand.developpez.com/dotnet/regex/
        'Dim atRegex As New Regex(".*?\+(?<cmd>[A-Z]+):\s*(?<result>.*)", RegexOptions.IgnoreCase)
        'Dim atMatch As Match = atRegex.Match(atCmdResult.Trim())
        'If debugFrm.Visible Then debugFrm.debugTextBox.AppendText(atCmdResult)
        ''If debugFrm.Visible Then debugFrm.debugTextBox.AppendText(atMatch.Groups("cmd").Value)

        'If atMatch.Success Then
        '    Select Case atMatch.Groups("cmd").Value
        '        'replaced by hfpSignalPct
        '        Case "CSQ"
        '            'Signal Quality
        '            Dim resultMatch As Match = Regex.Match(atMatch.Groups("result").Value, "([0-9]+),([0-9]+)")
        '            hfpSignalPct = Byte.Parse(resultMatch.Groups(1).Value)
        '            ToLog("AT+CSQ return: " & hfpSignalPct.ToString)

        '            'Case "CBC"
        '            '    'External power check
        '            '    Dim battinfo() As String, ExternalPowerIsON As String
        '            '    ExternalPowerIsON = Replace(atMatch.Value, Chr(0), "")
        '            '    If Left(ExternalPowerIsON, 4) = "+CBC" Then
        '            '        ExternalPowerIsON = Mid(ExternalPowerIsON, 6).Replace(Chr(34), "").Trim
        '            '    End If
        '            '    battinfo = ExternalPowerIsON.Split(",")
        '            '    If battinfo(0) = 1 And ExternalPowerIsConnected = False Then
        '            '        ExternalPowerIsConnected = True
        '            '        RaiseEvent BlueSoleil_Event_HFP_ExtPowerBattON()
        '            '    ElseIf battinfo(0) = 0 And ExternalPowerIsConnected = True Then
        '            '        ExternalPowerIsConnected = False
        '            '        RaiseEvent BlueSoleil_Event_HFP_ExtPowerBattOFF()
        '            '    End If
        '            '    'replaced by hfpBatteryPct
        '            '    'battery = battinfo(1)

        '            'Case "CGSN" 'lecture N°IMEI
        '            '    ImeiNumber = atMatch.Value
        '            '    'MsgBox(ImeiNumber)
        '            'Case "CIMI" 'lecture N°ISMI
        '            '    IsmiNumber = atMatch.Value
        '            'Case "CGMR" 'lecture revision number
        '            '    RevisionNumber = atMatch.Value

        '            'Case "CCLK" 'return date  and time from phone 
        '            '    '+CCLK: "15/07/17,18:44:29+04"
        '            '    Dim PhoneDateTime() As String = atMatch.Groups("result").Value.Replace(Chr(34), "").Split(",")
        '            '    PhoneDate = PhoneDateTime(0)
        '            '    PhoneTime = Left(PhoneDateTime(1), 8)

        '        Case "CIND" 'accepté par Galaxy A5 (2016)
        '            'AT+CIND=?
        '            '+CIND: ("CALL",(0,1)),("CALLSETUP",(0-3)),("SERVICE",(0-1)),("SIGNAL",(0-5)),("ROAM",(0,1)),("BATTCHG",(0-5)),("CALLHELD",(0-2))
        '            '+CIND: 0,0,1,1,0,3,0

        '            'MessageBox.Show(atMatch.Value)
        '            ToLog("CIND return: " & atMatch.Value)
        '            'Dim PhoneEvent() As String = atMatch.Value.Trim.Split(":")
        '            'Dim CINDbits() As String = PhoneEvent(1).Split(",")
        '            'hfpBatteryPct = Convert.ToDouble(CINDbits(6))
        '            'ToLog("AT+CIND return: " & hfpBatteryPct.ToString)
        '            'Dim PhoneEventList As List(Of String) = New List(Of String)(CINDbits)
        '            'MessageBox.Show(PhoneEvent(1).ToString)
        '            'For bitCIND As Integer = 0 To PhoneEventList.Count - 1
        '            '    Select Case bitCIND
        '            '        Case 6
        '            '            hfpBatteryPct=
        '            '            'Case 9
        '            '            '    If PhoneEventList.Item(9) = "1" Then ExternalPowerIsConnected = True Else ExternalPowerIsConnected = False
        '            '    End Select
        '            'Next

        '    End Select

        'End If

    End Sub

    Private Sub bsHandler_Status_TurnOn()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONE_TURNON", True)
        Else
            SDK.Execute("ONMOBILEPHONE_TURNON", True)
        End If
    End Sub
    Private Sub bsHandler_Status_TurnOff()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONE_TURNOFF", True)
        Else
            SDK.Execute("ONMOBILEPHONE_TURNOFF", True)
        End If
    End Sub

    Private Sub bsHandler_Status_Plugged()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONE_PLUGGEDON", True)
        Else
            SDK.Execute("ONMOBILEPHONE_PLUGGEDON", True)
        End If
    End Sub
    Private Sub bsHandler_Status_Unplugged()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONE_PLUGGEDOFF", True)
        Else
            SDK.Execute("ONMOBILEPHONE_PLUGGEDOFF", True)
        End If
    End Sub

    Private Sub bsHandler_Status_DevicePaired()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONE_DEVICEPAIRED", True)
        Else
            SDK.Execute("ONMOBILEPHONE_DEVICEPAIRED", True)
        End If
    End Sub
    Private Sub bsHandler_Status_DeviceUnpaired()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONE_DEVICEUNPAIRED", True)
        Else
            SDK.Execute("ONMOBILEPHONE_DEVICEUNPAIRED", True)
        End If
    End Sub
    Private Sub bsHandler_Status_DeviceDeleted()
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONE_DEVICEDELETED", True)
        Else
            SDK.Execute("ONMOBILEPHONE_DEVICEDELETED", True)
        End If
    End Sub
    Private Sub bsHandler_Status_DeviceFound(ByVal dvcHandle As UInt32)
        If PluginRunForDS = False Then
            SDK.Execute("*ONMOBILEPHONE_DEVICEFOUND", True)
        Else
            SDK.Execute("ONMOBILEPHONE_DEVICEFOUND", True)
        End If
    End Sub


    Private Sub bsHandler_FTP_FoundFolder(ByVal foundFolder As String)
        'If lvwFTPbrowser.InvokeRequired = True Then
        '    'thread-safe UI update.
        '    Dim args(0 To 0) As Object
        '    args(0) = foundFolder
        '    lvwFTPbrowser.BeginInvoke(New DelegateFTPfoundFolder(AddressOf bsHandler_FTP_FoundFolder), args)
        'Else
        '    SyncLock lvwFTPbrowser
        '        Dim tempItem As ListViewItem = lvwFTPbrowser.Items.Add("[" & foundFolder & "]")
        '    End SyncLock
        'End If
    End Sub

    Private Sub bsHandler_FTP_FoundFile(ByVal foundFile As String, ByVal fileSize As UInt64)
        'If lvwFTPbrowser.InvokeRequired = True Then
        '    'thread-safe UI update.
        '    Dim args(0 To 1) As Object
        '    args(0) = foundFile
        '    args(1) = fileSize
        '    lvwFTPbrowser.BeginInvoke(New DelegateFTPfoundFile(AddressOf bsHandler_FTP_FoundFile), args)
        'Else
        '    SyncLock lvwFTPbrowser
        '        Dim tempItem As ListViewItem = lvwFTPbrowser.Items.Add(foundFile)
        '        tempItem.SubItems.Add(Format(fileSize, "###,###,###,##0"))

        '        lvwFTPbrowser.Sort()
        '    End SyncLock
        'End If
    End Sub

    Private Sub bsHandler_Status_ServiceConnectedInbound(ByVal dvcHandle As UInt32, ByVal svcHandle As UInt32, ByVal svcClass As UInt16)
    End Sub
    Private Sub bsHandler_Status_ServiceDisconnectedInbound(ByVal dvcHandle As UInt32, ByVal svcHandle As UInt32, ByVal svcClass As UInt16)
    End Sub
    Private Sub bsHandler_Status_ServiceConnectedOutbound(ByVal dvcHandle As UInt32, ByVal svcHandle As UInt32, ByVal svcClass As UInt16)
    End Sub
    Private Sub bsHandler_Status_ServiceDisconnectedOutbound(ByVal dvcHandle As UInt32, ByVal svcHandle As UInt32, ByVal svcClass As UInt16)
    End Sub


    Private Sub bsHandler_ERROR_Return(ByVal newerror As String, ByVal usepopup As Boolean, useasrrcommand As Boolean)
        If usepopup = True Then
            If PluginRunForDS = False Then
                SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||CLADD;" & newerror, True)
            Else
                SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||CLADD;" & newerror, True)
            End If
        Else
            If useasrrcommand = True Then
                SDK.Execute(newerror, True)
            Else
                SDK.Execute("SETVAR;MOBILEPHONE_INFO;" & newerror, True)
            End If
        End If

    End Sub

    Private Sub hfpDisconnectAfterChange()
        Try
            If hfpIsActive = True Then
                btnHandsFreeDisconnect()
                'Terminate()
                If PluginRunForDS = False Then
                    SDK.Execute("*ONMOBILEPHONE_UNCONNECT_AFERDEVICECHANGED", True)
                Else
                    SDK.Execute("ONMOBILEPHONE_UNCONNECT_AFERDEVICECHANGED", True)
                End If

            End If
            'btnHandsFreeConnect(PhoneCheckedIs)

            If PluginRunForDS = False Then
                SDK.Execute("mobilephone_settings_setvarphone||SetErrorScr;MobilePhone Info;" & SDK.GetInfo("=$l_set_BTM_FirstPhone$") & " '" & TempPluginSettings.PhoneDeviceName & "';" & _
                            IIf(TempPluginSettings.PhoneDeviceName2 <> "NONAME", SDK.GetInfo("=$l_set_BTM_SecondPhone$") & " '" & TempPluginSettings.PhoneDeviceName2 & "'", SDK.GetInfo("=$l_set_BTM_NoSecondPhone$")) & ";3" &
                            "||WAIT;1||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||" & _
                            "SETVAR;MOBILEPHONE_INFO;Message Error||CLADD;||CLADD;" & SDK.GetInfo("=$l_set_BTM_QuitAfterApply$") & " RideRunner !!!")
            Else
                SDK.Execute("mobilephone_settings_setvarphone||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||SETVAR;MOBILEPHONE_INFO;Settings Info||" & _
                            "CLADD;" & SDK.GetInfo("=$l_set_BTM_FirstPhone$") & " '" & TempPluginSettings.PhoneDeviceName & "'||" & _
                            "CLADD;" & IIf(TempPluginSettings.PhoneDeviceName2 <> "NONAME",
                                SDK.GetInfo("=$l_set_BTM_SecondPhone$") & " '" & TempPluginSettings.PhoneDeviceName2 & "'",
                                SDK.GetInfo("=$l_set_BTM_NoSecondPhone$")) & _
                            "||SETVAR;MOBILEPHONE_SETTINGSINFO;$l_set_BTM_QuitAfterApply$ iCarDS !!!")
            End If

        Catch ex As Exception
            SDK.Execute("ONMOBILEPHONE_DEVICECHANGED_ERROR --> " & ex.Message, True)
        End Try

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
               "ClockWorK for HFP, PBAP, MAP, PAN, SPP and AVRCP features" & vbCrLf & _
               "Pierre Montet", "About (Bluesoleil RR Plugin)", _
                MessageBoxButtons.OK, _
                MessageBoxIcon.Asterisk)
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

        'If Directory.Exists(pluginDataPath) = False Then Directory.CreateDirectory(pluginDataPath)

        '**********************
        PluginRunForDS = True
        '**********************
        If PluginRunForDS = False Then  'RideRunner path
            MainPath = pluginDataPath
            SDK.Execute("SETVARBYCODE;PLUGINPATH;PLUGINSPATH")
        Else 'iCarDS path
            MainPath = Application.StartupPath & "\Extentions\MobilePhone\"
            SDK.Execute("SETVAR;PLUGINPATH;" & Application.StartupPath & "\Extentions\")
        End If
        'MessageBox.Show(pluginDataPath)
        SkinPath = SDK.GetInfo("=$SKINPATH$")
        If PluginRunForDS = True Then
            ThemeIconsPath = SDK.GetInfo("=$ThemeIcons$")
            favoriteSkinindicatorsPath = SkinPath & "Theme\" & ThemeIconsPath & "\MenuIcons\" '"$SKINPATH$Theme\$ThemeIcons$\MenuIcons\"
            'DefineLanguages()
        End If
        sPhotoPath = MainPath & "Photo\"
        sPhotoExtension = "jpg"
        sPhonebookpath = MainPath & "Phonebook\"
        dialNumber = ""
        DebuglogPath = MainPath & "MobilePhone.log"


        If File.Exists(DebuglogPath) = True Then
            File.Delete(DebuglogPath)
        End If


        SDK.RRlog("MOBILEPHONE is started")

        'ckeck version for delete the MobilePhone.xml file
        If My.Settings.LastVersion <> Assembly.GetExecutingAssembly().GetName().Version.ToString() Then
            ToLog("Old version was : " & My.Settings.LastVersion)
            ToLog("New version is : " & Assembly.GetExecutingAssembly().GetName().Version.ToString())
            If File.Exists(MainPath & "MobilePhone.xml") = True Then
                File.Delete(MainPath & "MobilePhone.xml")
                ToLog("MobilePhone.xml file is updated")
            End If
            My.Settings.LastVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString()
            My.Settings.Save()
        End If

        If File.Exists(MainPath & "Languages.txt") = False Then
            ListeDirectoriesIntoDirectory(MainPath & "Languages")
        End If

        '''''''''''''''''
        ''' Settings  '''
        '''''''''''''''''
        If PluginRunForDS = False Then  'RideRunner path
            PluginSettings = New cMySettings(Path.Combine(pluginDataPath, "MobilePhone"))
        Else 'iCarDS path
            PluginSettings = New cMySettings(Path.Combine(MainPath, "MobilePhone"))
            'PluginSettings = New cMySettings(MainPath & "MobilePhone")
        End If
        ' read in defaults
        cMySettings.DeseralizeFromXML(PluginSettings)
        ' copy to temp
        TempPluginSettings = New cMySettings(PluginSettings)

        ' check if PhoneCountryCodes as the good format, if no, the file is updated  with the default values ("0","33","00") 
        If TempPluginSettings.PhoneCountryCodes.Count = 0 Then
            cMySettings.SetToDefault(PluginSettings, True)
            ' copy to temp (skin views temp)
            cMySettings.Copy(PluginSettings, TempPluginSettings)
            cMySettings.SerializeToXML(PluginSettings)
        End If

        'SDK.Execute("PRELOAD;MOBILECALL.skin")
        'SDK.Execute("PRELOAD;MOBILEPHONE.skin")
        If Not Directory.Exists(MainPath & "PhoneBook") Then
            Directory.CreateDirectory(MainPath & "PhoneBook")
        End If
        If Not Directory.Exists(MainPath & "Obex") Then
            Directory.CreateDirectory(MainPath & "Obex")
        End If
        If Not Directory.Exists(MainPath & "Messages") Then
            Directory.CreateDirectory(MainPath & "Messages")
        End If
        If Not Directory.Exists(MainPath & "Photo") Then
            Directory.CreateDirectory(MainPath & "Photo")
        End If

        'read(languages)
        ReadLanguageVars()

        'force list format not simplified!
        pbapHistoryListIsSimplified = True

        If PluginRunForDS = False Then 'replace BYIND;GTRANSLATOR;=RRTRANSLATOR is OFF<<=RRTRANSLATOR is ON by BYVAR;GTRANSLATOR;=RRTRANSLATOR is OFF<<=RRTRANSLATOR is ON into settings2
            If CBool(SDK.GetInd("GTRANSLATOR")) = True Then
                SDK.Execute("SETVAR;GTRANSLATOR;1")
            Else
                SDK.Execute("SETVAR;GTRANSLATOR;0")
            End If
        End If

        MyCultureInfo = CultureInfo.InstalledUICulture.DisplayName 'CultureInfo.CurrentCulture.EnglishName
        MyPhoneCultureInfo = CountryToPhoneIndicatif(MyCultureInfo)

        hfpContactNumberInBlackList = CountContactsInBlacklist()

        'hfpCallerIDnoFormatted = New PhoneNumberFormatter(TempPluginSettings.PhoneCountryCodes(0), TempPluginSettings.PhoneCountryCodes(1), TempPluginSettings.PhoneCountryCodes(2))

        CreateAllHandlers()


        If TempPluginSettings.AutoSwapPhone = True Then
            If TempPluginSettings.PhoneDeviceName <> "NONAME" AndAlso TempPluginSettings.PhoneDeviceName2 <> "NONAME" Then
                SDK.RRlog("Phone SWAP mode is ready to use")
                ToLog("Phone SWAP mode is ready to use")
            ElseIf TempPluginSettings.PhoneDeviceName <> "NONAME" AndAlso TempPluginSettings.PhoneDeviceName2 = "NONAME" Then
                SDK.RRlog("Phone SWAP mode isn't ready to use (second phone isn't defined in settings)")
                ToLog("Phone SWAP mode isn't ready to use (second phone isn't defined in settings)")
            End If
        End If

        'define Ringin loop
        TimerRinggingBuild()

        'define if MAP will be loaded by variable
        If SDK.GetInfo("=$MOBILEPHONE_MAPSERVICE$") = "" Or SDK.GetInfo("=$MOBILEPHONE_MAPSERVICE$") = "1" Then
            LoadMAPService = True
        ElseIf SDK.GetInfo("=$MOBILEPHONE_MAPSERVICE$") = "0" Then
            LoadMAPService = False
        End If

        If TempPluginSettings.RunOnStart = True Then
            If PluginRunForDS = False Then  'RideRunner path
                ToLog("MOBILEPHONE is ready for RideRunner")
            Else 'iCarDS path
                ToLog("MOBILEPHONE is ready for iCarDS")
            End If

            RunUSBWatcher()

            If TempPluginSettings.PhoneDeviceName <> "NONAME" Then
                bgw = New BackgroundWorker
                ' le BackGroundWorker doit indiquer sa progression et accepter une possible annulation
                bgw.WorkerReportsProgress = True
                bgw.WorkerSupportsCancellation = True

                AddHandler bgw.DoWork, AddressOf bgw_DoWork
                AddHandler bgw.ProgressChanged, AddressOf bgw_ProgressChanged
                AddHandler bgw.RunWorkerCompleted, AddressOf bgw_RunWorkerCompleted

                If Not bgw.IsBusy = True Then
                    bgw.RunWorkerAsync()
                    SDK.RRlog("MOBILEPHONE run a BackroundWorker")
                    ToLog("MOBILEPHONE run a BackroundWorker")
                End If
                ' on utilise l'objet bgwArgument pour passer des paramètres au Thread
                'bgw.RunWorkerAsync(New bgwArguments(CInt(Me.tbCompteur.Text), CInt(Me.tbPause.Text)))

                ' Lancement du thread
                'threadRunBluesoleil.Start()
            Else
                ToLog("RunOnStart isn't possible (Device name 1 = NONAME")
            End If
        Else
            If PluginRunForDS = False Then
                SDK.Execute("WAIT;5||SetErrorScr;MobilePhone Info;" & SDK.GetInfo("=$l_set_BTM_StartAutoIsOffInfo$") & ";" & SDK.GetInfo("=$l_set_BTM_StartAutoIsOffText$") & ";5")
            Else
                SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_StartAutoIsOffText||POPUP;ReadingPhoneBook.SKIN;5")
            End If
        End If

    End Sub

    Private Sub bgw_Cancel()
        If bgw.WorkerSupportsCancellation = True Then
            bgw.CancelAsync()
            SDK.RRlog("BackroundWorker is stopped")
            ToLog("BackroundWorker is stopped")
        End If
    End Sub

    Private Sub bgw_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        If hfpIsActive = False Then
            SDK.RRlog("BackroundWorker run InitHfpAndMapService()")
            ToLog("BackroundWorker run InitHfpAndMapService()")
            InitHfpAndMapService()
        End If

    End Sub
    Private Sub bgw_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        'If hfpIsActive = False Then
        'If Not bgw.IsBusy = True Then
        '    bgw.RunWorkerAsync()
        'End If
        If e.Cancelled = True Then
            SDK.RRlog("MOBILEPHONE BackgroundWorker Canceled")
            ToLog("MOBILEPHONE BackgroundWorker Canceled")
        ElseIf e.Error IsNot Nothing Then
            SDK.RRlog("MOBILEPHONE BackgroundWorker Error " & e.Error.Message)
            ToLog("MOBILEPHONE BackgroundWorker Error " & e.Error.Message)
            'If Not bgw.IsBusy = True Then
            '    bgw = New BackgroundWorker
            '    bgw.RunWorkerAsync()
            '    SDK.RRlog("MOBILEPHONE start after a BackroundWorker error")
            '    ToLog("MOBILEPHONE start after a BackroundWorker error")
            'End If
            'Dim st As New Threading.Thread(Sub() InitHfpAndMapService()) 'check new unread msg
            'st.Start()
        Else
            SDK.RRlog("MOBILEPHONE is Ready :-)")
            ToLog("MOBILEPHONE is Ready :-)")
            tbProgress_Text = "MOBILEPHONE is Ready :-)"
        End If

        'bgw_Cancel()
        'End If

    End Sub
    Private Sub bgw_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
        tbProgress_Text = e.ProgressPercentage.ToString() & "%"
    End Sub
    '*****************************************************************
    '* This sub is called on unload of plugin by RR
    '*
    '*****************************************************************
    Public Sub Terminate()

        If bgw.IsBusy = True Then
            If bgw.WorkerSupportsCancellation = True Then
                bgw.CancelAsync()
                bgw.Dispose()
                ToLog("BackgroundWorker is stopped")
            End If
        End If
        'If threadRunBluesoleil.IsAlive = True Then
        '    threadRunBluesoleil.Abort()
        '    ToLog("threadRunBluesoleil is stopped")
        'End If

        btnHandsFreeDisconnect() 'disconnect hfp service
        If avrcpIsActive = True Then
            btnMediaStop()
            btnMediaDisconnect() 'disconnect avrcp service
        End If
        If mapIsActive = True Then
            btnMsgDisconnect() 'disconnect map service
        End If
        btnFTPdisconnect() 'disconnect ftp service
        btnSppDisConnect() 'disconnect spp service
        btnA2DPDisconnect() 'disconnect a2dp service (headset)

        btnDeInitBlueSoleil()
        BlueSoleil_StopBlueTooth()
        If speechrecognition.SpeechIsActive = True Then
            speechrecognition.unload()
        End If
        If debugFrm.Visible = True Then
            debugFrm.Hide()
            debugFrm.Dispose()
        End If
        RemoveAllHandlers()
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
                Properties = "Asela Fernando (Fox_Mulder) / Modified by pierrotm777"
            Case "category"
                Properties = "Phone" '"Source"
            Case "description"
                Properties = "BTMobilephone"
                'define as a RR source
            Case "src_guid"
                Properties = "{2A459B0B-6EBD-4977-B933-3EEF99668187}"
            Case "src_activesource"
                Properties = "AVRCP"
            Case "src_skin"
                Properties = SRCActiveScreen
                'define as a RR source
            Case "supporturl"
                If PluginRunForDS = False Then
                    'RideRunner
                    Properties = "http://www.mp3car.com/forum/mp3car-technical-software/front-ends/road-runner/rr-plugins/rr-plugins-in-progress/2579831-new-bt-mobilephone-plugin"
                Else
                    'iCarDS
                    Properties = "http://pccar.ru/showthread.php?t=24142"
                End If
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
            Case "drivelist"
                SDK.Execute("LOAD;FTPCOPY.SKIN")
                DrivesList()
                ProcessCommand = 2
                'Case "pbexist"
                '    Try
                '        ' do we have a CL on the screen?
                '        Dim isCLShowing As Boolean = CBool(frm.GetType().InvokeMember("ISShowPB", BindingFlags.GetProperty, Nothing, frm, Nothing))
                '        MsgBox(isCLShowing.ToString)
                '    Catch ex As Exception
                '        MessageBox.Show(ex.Message)
                '    End Try
                '    ProcessCommand = 2

                'Case "pb_test"
                '    Try
                '        'For x As Integer = 0 To 10
                '        frm.PBAddItem("Bonjour")
                '        'Next
                '    Catch ex As Exception
                '        MessageBox.Show(ex.Message)
                '    End Try
                '    ProcessCommand = 2

                'Case "pl_test"
                '    Try
                '        'For x As Integer = 0 To 10
                '        frm.PLAddItem("Bonjour")
                '        'Next
                '    Catch ex As Exception
                '        MessageBox.Show(ex.Message)
                '    End Try
                '    ProcessCommand = 2

                'RR events
                'Case "onsourcechange"
                '    If SDK.GetInfo("ACTIVESOURCE") = "AVRCP Player" Then 'var of ACTIVESOURCEID = 10
                '        If CInt(SDK.GetInfo("VOL;PLAYER")) = 0 Then SDK.Execute("setvol;winamp;255")
                '        If SDK.GetInd("MUTE") = "True" Then SDK.Execute("MUTE")
                '        'DabStart()
                '        'probably the start of a crashed rr, ignore
                '    ElseIf SDK.GetInfo("ACTIVESOURCE").Length = 0 Then
                '        'player.Stop();
                '        'MyWinAmp.StopWinamp();
                '        ' set last resume to false
                '    Else
                '        'DabStop()
                '    End If
                '    ProcessCommand = 2

            Case "onidle"
                If LCase(frm.tag) = "mobilephone_info.skin" Then
                    SDK.Execute("CLCLEAR;ALL||CLLOAD;" & MainPath & "MobilePhone_DevicesList.lst||SETVAR;MOBILEPHONE_INFO;Device's list !!!")
                End If
                ProcessCommand = 2
            Case "onsuspend"
                SDK.RRlog("MobilePhone is running HIBERNATE mode")
                Terminate()
                ProcessCommand = 2
            Case "onresume"
                If PluginSettings.PhoneDeviceName <> "NONAME" Then 'And YouMustRestartRR = False Then
                    SDK.RRlog("MobilePhone restart after HIBERNATE")
                    ToLog("Connection to the device '" & TempPluginSettings.PhoneDeviceName & "' after RESUME")
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVARFROMVAR;POPUP;l_set_BTM_PleaseResumeWait||POPUP;POPUP.SKIN;5")
                    Else
                        SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_PleaseResumeWait||POPUP;ReadingPhoneBook.SKIN;5")
                    End If
                    pbapUnknowName = SDK.GetInfo("=$l_set_BTM_Unknown$")
                    bgw = New BackgroundWorker
                    ' le BackGroundWorker indique sa progression et accepte une possible annulation
                    bgw.WorkerReportsProgress = True
                    bgw.WorkerSupportsCancellation = True
                    AddHandler bgw.DoWork, AddressOf bgw_DoWork
                    AddHandler bgw.ProgressChanged, AddressOf bgw_ProgressChanged
                    AddHandler bgw.RunWorkerCompleted, AddressOf bgw_RunWorkerCompleted
                    If Not bgw.IsBusy = True Then
                        bgw.RunWorkerAsync()
                        SDK.RRlog("MobilePhone RESUME after HIBERNATE is done")
                    End If
                    'threadRunBluesoleil.Start()
                Else
                    ToLog("Connection after RESUME isn't possible (Device name 1 = NONAME")
                    SDK.Execute("SETVAR;l_ReadingPhoneBook;Connection fater RESUME isn't possible (Device name 1 = NONAME). Please update your phone's settings !||POPUP;ReadingPhoneBook.SKIN;5")
                End If
                ProcessCommand = 2

            Case "onexit"
                'Terminate()
                ProcessCommand = 2

            Case "onclposchange" 'onclposchange used by RR
                If PluginRunForDS = False Then
                    If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_contacts.skin" Then
                        If vcardInPBList = True Then
                            vcarIdPh = BlueSoleil_PBAP_GetVcardId(SDK.GetInfo("CLTEXT")) 'CInt(SDK.GetInfo("CLPOS")) - 1 'read the contact position in the vcard list for use it into vcard viewer and editor.
                        Else
                            vcardInPBListString = SDK.GetInfo("CLTEXT")
                            If File.Exists(MainPath & "PhoneBook\pb.vcf") = True AndAlso BlueSoleil_PBAP_NumberExistAsVcard(vcardInPBListString) Then
                                vcarIdPh = BlueSoleil_PBAP_GetVcardId(vcardInPBListString)
                            Else
                                'vcarIdPh = -1
                                vcarIdPh = BlueSoleil_PBAP_GetVcardId(vcardInPBListString)
                                SDK.Execute("SETVARFROMVAR;MOBILEPHONE_CARDNAME;l_set_BTM_Unknown||SETVAR;MOBILEPHONE_CARDPHONENUMBER;" & vcardInPBListString)
                            End If
                        End If
                    End If
                End If
                ProcessCommand = 2
                'Case "onclselitem" 'onclselitem used by iCarDS only
                '    If vcardInPBList = True Then
                '        vcarIdPh = BlueSoleil_PBAP_GetVcardId(SDK.GetInfo("CLTEXT")) 'CInt(SDK.GetInfo("CLPOS")) - 1 'read the contact position in the vcard list for use it into vcard viewer and editor.
                '    Else
                '        vcardInPBListString = SDK.GetInfo("CLTEXT")
                '        If File.Exists(MainPath & "PhoneBook\pb.vcf") = True AndAlso BlueSoleil_PBAP_NumberExistAsVcard(vcardInPBListString) Then
                '            vcarIdPh = BlueSoleil_PBAP_GetVcardId(vcardInPBListString)
                '        Else
                '            'vcarIdPh = -1
                '            vcarIdPh = BlueSoleil_PBAP_GetVcardId(vcardInPBListString)
                '            SDK.Execute("SETVARFROMVAR;MOBILEPHONE_CARDFULLNAME;l_set_BTM_Unknown||SETVAR;MOBILEPHONE_CARDPHONENUMBER;" & vcardInPBListString) ' & "||MOBILEPHONE_NEWENTRY")
                '        End If
                '    End If
                '    If PluginRunForDS = True Then
                '        dialNumber = SDK.GetInfo("CLTEXT")
                '    End If
                '    ProcessCommand = 2

            Case "mobilephone"
                If hfpIsActive = False Then
                    If PluginSettings.PhoneDeviceName <> "NONAME" Then 'And YouMustRestartRR = False Then
                        pbapUnknowName = SDK.GetInfo("=$l_set_BTM_Unknown$")
                        bgw = New BackgroundWorker
                        ' le BackGroundWorker doit indiquer sa progression et accepter une possible annulation
                        bgw.WorkerReportsProgress = True
                        bgw.WorkerSupportsCancellation = True
                        AddHandler bgw.DoWork, AddressOf bgw_DoWork
                        AddHandler bgw.ProgressChanged, AddressOf bgw_ProgressChanged
                        AddHandler bgw.RunWorkerCompleted, AddressOf bgw_RunWorkerCompleted
                        If Not bgw.IsBusy = True Then
                            bgw.RunWorkerAsync()
                        End If
                        'threadRunBluesoleil.Start()

                        ToLog("Manual connection to the device '" & TempPluginSettings.PhoneDeviceName & "'")
                        If PluginRunForDS = False Then
                            SDK.Execute("SETVARFROMVAR;POPUP;l_set_BTM_PleaseWait||MENU;POPUP.skin")
                        Else
                            SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_PleaseWait||POPUP;ReadingPhoneBook.SKIN;5")
                        End If
                    Else
                        ToLog("Manual connection isn't possible (Device name 1 = NONAME")
                        SDK.Execute("MOBILEPHONE_SETTINGS")
                    End If
                Else
                    If PluginRunForDS = False Then
                        SDK.Execute("LOAD;MOBILEPHONE.SKIN")
                    Else
                        SDK.Execute("LOAD;MOBILEPHONE.SKIN||WAIT;0.5||SETVAR;OLDENTRY;||MOBILEPHONE_PB")
                    End If
                End If
                ProcessCommand = 2

            Case "mobilephone_connect"
                If hfpIsActive = False Then
                    btnHandsFreeConnect(PhoneCheckedIs)
                Else
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVARFROMVAR;POPUP;l_set_BTM_AllReadyConnected||MENU;POPUP.skin")
                    Else
                        SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_AllReadyConnected||POPUP;ReadingPhoneBook.skin;5")
                    End If
                End If
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Connected ..." & vbCrLf)
                ProcessCommand = 2
            Case "mobilephone_disconnect"
                If hfpIsActive = True Then
                    btnHandsFreeDisconnect()
                Else
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVARFROMVAR;POPUP;l_set_BTM_AllReadyDisConnected||MENU;POPUP.skin")
                    Else
                        SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_AllReadyDisConnected||POPUP;ReadingPhoneBook.skin;5")
                    End If
                End If
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Disconnected ..." & vbCrLf)
                ProcessCommand = 2

            Case "mobilephone_btstart"
                BlueSoleil_StartBlueTooth()
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Phone is ON ..." & vbCrLf)
                ProcessCommand = 2
            Case "mobilephone_btstop"
                BlueSoleil_DisconnectServiceConns_ByName("MAP")
                BlueSoleil_DisconnectServiceConns_ByName("PAN")
                BlueSoleil_DisconnectServiceConns_ByName("PBAP")
                BlueSoleil_DisconnectServiceConns_ByName("SPP")
                BlueSoleil_DisconnectServiceConns_ByName("AVRCP")
                BlueSoleil_DisconnectServiceConns_ByName("HFP")
                BlueSoleil_DisconnectServiceConns_ByName("HSP")
                BlueSoleil_DisconnectServiceConns_ByName("A2DP")
                BlueSoleil_StopBlueTooth()
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText("Phone is OFF ..." & vbCrLf)
                ProcessCommand = 2

            Case "mobilephone_transaudio"
                Dim BA As Boolean = btnHandsFreeTransfer()
                If debugFrm.Visible And BA Then debugFrm.debugTextBox.AppendText("Audio Trans is ON ..." & vbCrLf)
                If debugFrm.Visible And Not BA Then debugFrm.debugTextBox.AppendText("Audio Trans is OFF ..." & vbCrLf)
                ProcessCommand = 2

                'mute micro & speaker
            Case "mobilephone_mutemic"
                BlueSoleil_HFP_SetMicVol(hfpHandleConnHFAG, 0)
                ProcessCommand = 2
            Case "mobilephone_mutespkr"
                BlueSoleil_HFP_SetSpeakerVol(hfpHandleConnHFAG, 0)
                ProcessCommand = 2
            Case "mobilephone_unmutemic"
                BlueSoleil_HFP_SetMicVol(hfpHandleConnHFAG, 15)
                ProcessCommand = 2
            Case "mobilephone_unmutespkr"
                BlueSoleil_HFP_SetSpeakerVol(hfpHandleConnHFAG, 15)
                ProcessCommand = 2

                'TEXT INPUT COMMANDS
            Case "mobilephone_1"
                dialNumber += "1"
                dtmf("1")
                ProcessCommand = 2
            Case "mobilephone_2"
                dialNumber += "2"
                dtmf("2")
                ProcessCommand = 2
            Case "mobilephone_3"
                dialNumber += "3"
                dtmf("3")
                ProcessCommand = 2
            Case "mobilephone_4"
                dialNumber += "4"
                dtmf("4")
                ProcessCommand = 2
            Case "mobilephone_5"
                dialNumber += "5"
                dtmf("5")
                ProcessCommand = 2
            Case "mobilephone_6"
                dialNumber += "6"
                dtmf("6")
                ProcessCommand = 2
            Case "mobilephone_7"
                dialNumber += "7"
                dtmf("7")
                ProcessCommand = 2
            Case "mobilephone_8"
                dialNumber += "8"
                dtmf("8")
                ProcessCommand = 2
            Case "mobilephone_9"
                dialNumber += "9"
                dtmf("9")
                ProcessCommand = 2
            Case "mobilephone_0"
                dialNumber += "0"
                dtmf("0")
                ProcessCommand = 2
            Case "mobilephone_*"
                dialNumber += "*"
                dtmf("*")
                ProcessCommand = 2
            Case "mobilephone_+"
                dialNumber += "+"
                ProcessCommand = 2
            Case "mobilephone_#"
                dialNumber += "#"
                dtmf("#")
                ProcessCommand = 2
            Case "mobilephone_del"
                If PluginRunForDS = False Then
                    dialNumber = dialNumber.Remove(dialNumber.Length - 1)
                Else
                    dialNumber = dialNumber.Remove(dialNumber.Length - 1)
                    If dialNumber.Length = 0 Then
                        SDK.Execute("RELOADSCREEN")
                    End If
                End If
                ProcessCommand = 2
            Case "mobilephone_clear"
                dialNumber = ""
                If PluginRunForDS = True Then
                    If dialNumber.Length = 0 Then
                        SDK.Execute("RELOADSCREEN")
                    End If
                End If
                ProcessCommand = 2

            Case "mobilephone_addcountryprefix"
                dialNumber = ""
                Dim splitdialNumber() As String = SDK.GetInfo("CLTEXT").Replace(" ", "").Replace("-->", ",").Split(",")
                dialNumber += "+" & splitdialNumber(1)
                SDK.Execute("ESC")
                ProcessCommand = 2

                'DIALING COMMANDS
            Case "mobilephone_dial"
                If PluginRunForDS = False Then 'RR only
                    Try
                        hfpCallerIDno = dialNumber
                        OldMainVolume = SDK.GetInfo("VOLUME").Replace("%", "")
                        OldPlayerStatus = SDK.GetInfo("STATUS")

                        If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_entry.skin" Then
                            dialNumber = SDK.GetInfo("MOBILEPHONE_CARDPHONENUMBER")
                            SDK.Execute("PAUSE||SETVOL;MASTER;100||LOAD;MOBILEPHONE.skin||WAIT;.5||MENU;MOBILECALL.skin")
                        Else
                            SDK.Execute("PAUSE||SETVOL;MASTER;100||MENU;MOBILECALL.skin")
                        End If

                        If File.Exists(MainPath & "PhoneBook\pb.vcf") = True AndAlso BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDno) = True Then
                            hfpCallerIDname = BlueSoleil_PBAP_GetNameFromNumber(hfpCallerIDno)
                            If File.Exists(MainPath & "Photo\" & hfpCallerIDno & ".jpg") = True Then 'If File.Exists(BlueSoleil_BS_PBAP_GetImageFromNumber(hfpCallerIDno)) = True Then
                                'SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & BlueSoleil_BS_PBAP_GetImageFromNumber(hfpCallerIDno))
                                SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\" & hfpCallerIDno & ".jpg")
                            Else
                                SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\unknow.gif")
                            End If
                        ElseIf hfpCallerIDno = TempPluginSettings.VocalMessageryNumber Then
                            hfpCallerIDname = SDK.GetInfo("=$l_set_BTM_VoiceMail$")
                            SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\voicemail.png")
                        ElseIf hfpCallerIDno = TempPluginSettings.EmergencyNumber Then
                            hfpCallerIDname = SDK.GetInfo("=$l_set_BTM_Emergency$")
                            SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\emergency.png||*ONMOBILEPHONEEMERGENCY")
                        ElseIf hfpCallerIDname <> SDK.GetInfo("=$l_set_BTM_Unknown$") AndAlso SDK.GetInfo("=$PLACENAMECLIP$") <> "" Then
                            'Used by RRGMT plugin
                            hfpCallerIDname = SDK.GetInfo("=$PLACENAMECLIP$")
                        Else
                            hfpCallerIDname = SDK.GetInfo("=$l_set_BTM_Unknown$")
                            SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\unknow.gif")
                        End If
                        btnHandsFreeDial(dialNumber)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try

                Else 'iCarDS only
                    Try
                        hfpCallerIDno = dialNumber
                        OldMainVolume = SDK.GetInfo("VOLUME").Replace("%", "")
                        OldPlayerStatus = SDK.GetInfo("STATUS")

                        If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_entry" Then
                            hfpCallerIDno = SDK.GetInfo("MOBILEPHONE_CARDPHONENUMBER")
                            SDK.Execute("PAUSE||MENU;MOBILECALL.skin")
                        Else
                            SDK.Execute("PAUSE||MENU;MOBILECALL.skin")
                        End If

                        If File.Exists(MainPath & "PhoneBook\pb.vcf") = True AndAlso BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDno) = True Then
                            hfpCallerIDname = BlueSoleil_PBAP_GetNameFromNumber(hfpCallerIDno)
                            If File.Exists(MainPath & "Photo\" & hfpCallerIDno & ".jpg") = True Then 'If File.Exists(BlueSoleil_BS_PBAP_GetImageFromNumber(hfpCallerIDno)) = True Then
                                'SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & BlueSoleil_BS_PBAP_GetImageFromNumber(hfpCallerIDno))
                                SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\" & hfpCallerIDno & ".jpg")
                            Else
                                SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\unknow.gif")
                            End If
                        ElseIf hfpCallerIDno = TempPluginSettings.VocalMessageryNumber Then
                            hfpCallerIDname = SDK.GetInfo("=$l_set_BTM_VoiceMail$")
                            SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\voicemail.png")
                        ElseIf hfpCallerIDno = TempPluginSettings.EmergencyNumber Then
                            hfpCallerIDname = SDK.GetInfo("=$l_set_BTM_Emergency$")
                            SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\emergency.png||ONMOBILEPHONEEMERGENCY")
                        ElseIf hfpCallerIDname <> SDK.GetInfo("=$l_set_BTM_Unknown$") AndAlso SDK.GetInfo("=$PLACENAMECLIP$") <> "" Then
                            'Used by RRGMT plugin
                            hfpCallerIDname = SDK.GetInfo("=$PLACENAMECLIP$")
                        Else
                            hfpCallerIDname = SDK.GetInfo("=$l_set_BTM_Unknown$")
                            SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\unknow.gif")
                        End If

                        btnHandsFreeDial(dialNumber)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try

                End If
                ProcessCommand = 2

            Case "mobilephone_emergency"
                btnHandsFreeDial(TempPluginSettings.EmergencyNumber)
                ProcessCommand = 2
            Case "mobilephone_pickup"
                timer3.Enabled = False 'stop the ringin loop
                btnHandsFreeAnswer()
                IncomingCall = True
                ProcessCommand = 2
            Case "mobilephone_hangup"
                timer3.Enabled = False 'stop the ringin loop
                btnHandsFreeHangUp()
                If OldPlayerStatus = "PLAY" Then
                    If PluginRunForDS = False Then
                        SDK.Execute("RESUME||SETVOL;MASTER;" & OldMainVolume) 'resume the master volume to the old value
                    Else
                        SDK.Execute("RESUME||SETVOL;MASTER;" & OldMainVolume) 'resume the master volume to the old value
                    End If
                End If
                IncomingCall = False
                timerMapCounter = 0 'reinitiate check MAP counter
                timerNetworkCounter = 0 'reinitiate check Network counter
                If pbapReturnFromCall = "CallIn" Then

                End If
                ProcessCommand = 2
            Case "mobilephone_redial"
                'redial last call
                BlueSoleil_HandsFree_Redial()
                ProcessCommand = 2


                'PHONEBOOK NAVIGATION COMMANDS
            Case "mobilephone_pb_first_entry"
                SDK.Execute("PGUP||ONCLCLICK")
                ProcessCommand = 2
                'Case "mobilephone_pb_previous_page"
            Case "mobilephone_pb_previous_entry"
                SDK.Execute("UP||ONCLCLICK")
                ProcessCommand = 2
            Case "mobilephone_pb_next_entry"
                SDK.Execute("DOWN||ONCLCLICK")
                ProcessCommand = 2
                'Case "mobilephone_pb_next_page"
            Case "mobilephone_pb_last_entry"
                SDK.Execute("PGDOWN||ONCLCLICK")
                ProcessCommand = 2

                'PHONEBOOK MANAGEMENT COMMANDS
            Case "mobilephone_updatecl"
                If PluginRunForDS = False Then
                    'SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & SDK.GetInd("mobilephone_phonebookpicture")) '& "||SETVARBYCODE;FAVORITE;CLDESC")
                    If BlueSoleil_BS_PBAP_GetImageFromNumber(dialNumber) <> "" Then
                        SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & BlueSoleil_BS_PBAP_GetImageFromNumber(dialNumber))
                    Else
                        SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\unknow.gif")
                    End If
                    dialNumber = SDK.GetInfo("CLTEXT")
                Else
                    If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone" Then
                        'SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & BlueSoleil_BS_PBAP_GetImageFromNumber(dialNumber))
                        dialNumber = SDK.GetInfo("CLTEXT")
                    End If
                End If
                vcarIdPh = BlueSoleil_PBAP_GetVcardId(dialNumber)
                ProcessCommand = 2

            Case "mobilephone_viewentry"
                Try
                    Load_PhoneNumberFromVcard(vcarIdPh, 0)
                    Load_PhoneAddressFromVcard(vcarIdPh, 0)

                Catch ex As Exception
                    'MessageBox.Show(ex.Message, "Error viewentry")
                    ToLog("Error viewentry: " & ex.Message)
                End Try
                ProcessCommand = 2
            Case "mobilephone_viewentrymore"
                vcarIdPhOther = 0
                vcarIdAddOther = 0
                If vcarIdPh < PhoneBookEntryCount - 1 Then
                    vcarIdPh += 1
                    vcarIdAdd += 1
                ElseIf vcarIdAdd = PhoneBookEntryCount - 1 Then
                    vcarIdPh = PhoneBookEntryCount - 1
                    vcarIdAdd = PhoneBookEntryCount - 1
                End If
                Load_PhoneNumberFromVcard(vcarIdPh, 0)
                Load_PhoneAddressFromVcard(vcarIdAdd, 0)
                ProcessCommand = 2
            Case "mobilephone_viewentryless"
                vcarIdPhOther = 0
                vcarIdAddOther = 0
                If vcarIdPh > 0 Then
                    vcarIdPh -= 1
                    vcarIdAdd -= 1
                ElseIf vcarIdAdd = 0 Then
                    vcarIdPh = 0
                    vcarIdAdd = 0
                End If
                Load_PhoneNumberFromVcard(vcarIdPh, 0)
                Load_PhoneAddressFromVcard(vcarIdAdd, 0)
                ProcessCommand = 2
            Case "mobilephone_viewentryfirst"
                vcarIdPh = 0
                vcarIdAdd = 0
                vcarIdPhOther = 0
                vcarIdAddOther = 0
                Load_PhoneNumberFromVcard(0, 0)
                Load_PhoneAddressFromVcard(0, 0)
                ProcessCommand = 2
            Case "mobilephone_viewentrylast"
                vcarIdPh = PhoneBookEntryCount - 1
                vcarIdAdd = PhoneBookEntryCount - 1
                vcarIdPhOther = 0
                vcarIdAddOther = 0
                Load_PhoneNumberFromVcard(PhoneBookEntryCount - 1, 0)
                Load_PhoneAddressFromVcard(PhoneBookEntryCount - 1, 0)
                ProcessCommand = 2
            Case "mobilephone_cardothernumber"
                Try
                    If PhoneBookEntries(vcarIdPh).EntryPhoneNumberCount > 1 Then
                        If vcarIdPhOther = PhoneBookEntries(vcarIdPh).EntryPhoneNumberCount - 1 Then
                            vcarIdPhOther = 0
                        Else
                            vcarIdPhOther += 1
                        End If
                        Load_PhoneNumberFromVcard(vcarIdPh, vcarIdPhOther)
                    End If

                    If PhoneBookEntries(vcarIdAdd).EntryPhoneAddressCount > 0 Then
                        If vcarIdAddOther = PhoneBookEntries(vcarIdAdd).EntryPhoneAddressCount - 1 Then
                            vcarIdAddOther = 0
                        Else
                            vcarIdAddOther += 1
                        End If
                        Load_PhoneAddressFromVcard(vcarIdAdd, vcarIdAddOther)
                    Else
                        Load_PhoneAddressFromVcard(vcarIdAdd, 0)
                    End If

                Catch ex As Exception
                    'MessageBox.Show(ex.Message)
                    ToLog("Error View entry other: " & ex.Message)
                End Try
                ProcessCommand = 2

            Case "mobilephone_addcontactinphone"
                If PluginRunForDS = False Then
                    'SDK.Execute("SetErrorScr;MobilePhone Info;!!! This feature isn't ready !!!;****************  Sorry  ***************;5")
                    If BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDno) Or BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(hfpCallerIDno, OutputFormats.National)) = False Then
                        SDK.Execute("LOAD;MOBILEPHONE_CONTACT_EDIT.SKIN||RUN;" & MainPath & "SlideShow.exe" & ";SlideShow||SETVAR;MOBILEPHONE_CARDNAME;" & hfpCallerIDname & "||SETVAR;MOBILEPHONE_CARDPHONENUMBER;" & hfpCallerIDno, True)
                    End If
                Else
                    'SDK.Execute("MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||SETVAR;MOBILEPHONE_INFO;!!! This feature isn't ready !!!")
                    If BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDno) Or BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(hfpCallerIDno, OutputFormats.National)) = False Then
                        SDK.Execute("LOAD;MOBILEPHONE_CONTACT_EDIT.SKIN||RUN;" & MainPath & "SlideShow.exe" & ";SlideShow||SETVAR;MOBILEPHONE_CARDNAME;" & hfpCallerIDname & "||SETVAR;MOBILEPHONE_CARDPHONENUMBER;" & hfpCallerIDno, True)
                    End If
                End If
                ProcessCommand = 2
            Case "mobilephone_addcontactinblacklist"
                'blackListCountNumberInList = 0
                If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_addcontact.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_addcontact" Then
                    Dim addNewContactId As String = ""
                    Dim addNewContactName As String = ""
                    addNewContactId = hfpCallerIDno
                    addNewContactName = hfpCallerIDname
                    If CheckContactInBlackList(addNewContactId) = False Then
                        AddContactInBlackList(addNewContactId)
                        If PluginRunForDS = False Then
                            SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||" & _
                                    "CLADD;The number '" & addNewContactId & "' is black listed !!!||*ONMOBILEPHONEADDINBLACKLIST||" & _
                                    "SETVAR;MOBILEPHONE_BLKLINFO;This phone number is now Black Listed !!!", True)
                        Else
                            SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||" & _
                                    "CLADD;The number '" & addNewContactId & "' is black listed !!!||*ONMOBILEPHONEADDINBLACKLIST||" & _
                                    "SETVAR;MOBILEPHONE_BLKLINFO;This phone number is now Black Listed !!!", True)
                        End If

                    End If
                ElseIf LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_contact_edit.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_contact_edit" Then
                    Dim newcardphonenumber As String = SDK.GetInfo("MOBILEPHONE_CARDPHONENUMBER")
                    newcardphonenumber = newcardphonenumber.Trim
                    If CheckContactInBlackList(newcardphonenumber) = False Then
                        If newcardphonenumber <> "-" Then
                            AddContactInBlackList(newcardphonenumber)
                            If PluginRunForDS = False Then
                                SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||" & _
                                 "CLADD;The number '" & newcardphonenumber & "' is black listed !!!||*ONMOBILEPHONEADDINBLACKLIST||" & _
                                 "SETVAR;MOBILEPHONE_BLKLINFO;This phone number is now Black Listed !!!", True)
                            Else
                                SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||" & _
                                 "CLADD;The number '" & newcardphonenumber & "' is black listed !!!||*ONMOBILEPHONEADDINBLACKLIST||" & _
                                 "SETVAR;MOBILEPHONE_BLKLINFO;This phone number is now Black Listed !!!", True)
                            End If
                        Else
                            SDK.Execute("SETVAR;MOBILEPHONE_BLKLINFO;No phone found to add !!!")
                        End If
                    Else
                        SDK.Execute("SETVAR;MOBILEPHONE_BLKLINFO;This number allready exist in the Black List !!!")
                    End If
                End If
                ProcessCommand = 2
            Case "mobilephone_resetblacklist"
                ResetContactsInBlacklist()
                If PluginRunForDS = False Then
                    SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||" & _
                            "CLADD;The Black list is empty !!!||SETVAR;MOBILEPHONE_BLKLINFO;-", True)
                Else
                    SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||" & _
                            "CLADD;The Black list is empty !!!||SETVAR;MOBILEPHONE_BLKLINFO;-", True)
                End If
                ProcessCommand = 2
            Case "mobilephone_deleteonecontactinblacklist"
                If PluginRunForDS = False Then
                    SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||WAIT;0.5||MOBILEPHONE_DELETEOK")
                Else
                    SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||WAIT;0.5||MOBILEPHONE_DELETEOK")
                End If
                ProcessCommand = 2
            Case "mobilephone_deleteok" 'internal only (lancé en deux fois sinon la list n'est pas visible assez vite, d'où WAIT;0.5)
                Try
                    ReadLanguageVars()
                    Dim allLines As String() = IO.File.ReadAllLines(MainPath & "MobilePhone_BlackList.lst", Encoding.Unicode)
                    For ph As Integer = 0 To allLines.Length - 1
                        If File.Exists(BlueSoleil_BS_PBAP_GetImageFromNumber(allLines(ph))) Then
                            'frm.CLAddItemWithImage(allLines(ph) & vbCrLf & allLines(ph) & " --> " & BlueSoleil_PBAP_GetNameFromNumber(allLines(ph)), BlueSoleil_BS_PBAP_GetImageFromNumber(allLines(ph)))
                            SDK.Execute("CLADD;" & allLines(ph) & ";" & allLines(ph) & " --> " & BlueSoleil_PBAP_GetNameFromNumber(allLines(ph)) & _
                                        "||CLSETIMG;" & ph + 1 & ";" & BlueSoleil_BS_PBAP_GetImageFromNumber(allLines(ph)))
                        Else
                            'frm.CLAddItemWithImage(allLines(ph) & vbCrLf & allLines(ph) & " --> Unknow", MainPath & "Photo\unknow.gif")
                            SDK.Execute("CLADD;" & allLines(ph) & ";" & allLines(ph) & " --> Unknow" & _
                                        "||CLSETIMG;" & ph + 1 & ";" & MainPath & "Photo\unknow.gif")
                        End If
                    Next
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error deleteok")
                End Try
                ProcessCommand = 2
            Case "mobilephone_delcontactinblacklist" 'internal command only
                If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_messagebox.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_messagebox" Then
                    DeleteContactInBlacklist(CInt(SDK.GetInfo("CLPOS")) - 1)
                    'SDK.Execute("mobilephone_deleteonecontactinblacklist")
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL" & _
                                    "||CLADD;Phone " & SDK.GetInfo("CLTEXT") & " isn't in the Black List !!!" & _
                                    "||SETVAR;MOBILEPHONE_BLKLINFO;Phone " & SDK.GetInfo("CLTEXT") & " isn't in the Black List !!!", True)
                    Else
                        SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL" & _
                                    "||CLADD;Phone " & SDK.GetInfo("CLTEXT") & " isn't in the Black List !!!" & _
                                    "||SETVAR;MOBILEPHONE_BLKLINFO;Phone " & SDK.GetInfo("CLTEXT") & " isn't in the Black List !!!", True)
                    End If

                End If
                ProcessCommand = 2

            Case "mobilephone_newentry" 'load the screen contact editor
                Try
                    NewVcarIdOther = 1
                    NewVcarId = 1
                    'newPhonesCount = NewVcarIdOther + 1
                    SDK.Execute("MOBILEPHONE_CARDRESET||LOAD;MOBILEPHONE_CONTACT_EDIT.SKIN||RUN;" & MainPath & "SlideShow.exe" & ";SlideShow")
                Catch ex As Exception
                    'MessageBox.Show(ex.Message, "Error newentry")
                    ToLog("Error newentry: " & ex.Message)
                End Try
                ProcessCommand = 2

            Case "mobilephone_cardaddothernumber"
                Try
                    'check if a name and number and phone type exist in minimum !!!
                    If SDK.GetInfo("=$PhoneNumberNType$") <> "" And SDK.GetInfo("=$mobilephone_cardphonenumber$") <> "" And SDK.GetInfo("=$PhoneNumberNType$") <> "" Then

                        'home,cell,work ==> PhoneBookEntryCount+1
                        Select Case Convert.ToInt16(SDK.GetInfo("=$PhoneNumberNType$"))
                            Case 1 'cell
                                SDK.Execute("SETVAR;mobilephone_typephone;mobile")
                                SDK.Execute("SETVAR;mobilephone_cardphonelabel;CELL")
                            Case 2 'home
                                SDK.Execute("SETVAR;mobilephone_typephone;home")
                                SDK.Execute("SETVAR;mobilephone_cardphonelabel;HOME")
                            Case 3 'work
                                SDK.Execute("SETVAR;mobilephone_typephone;work")
                                SDK.Execute("SETVAR;mobilephone_cardphonelabel;WORK")
                            Case 4 'fax
                                SDK.Execute("SETVAR;mobilephone_typephone;FAX")
                                SDK.Execute("SETVAR;mobilephone_cardphonelabel;FAX")
                            Case 5 'other
                                SDK.Execute("SETVAR;mobilephone_typephone;OTHER")
                                SDK.Execute("SETVAR;mobilephone_cardphonelabel;OTHER")
                        End Select

                        newPhones(NewVcarIdOther) = SDK.GetInfo("=$mobilephone_cardphonenumber$")
                        newLabels(NewVcarIdOther) = SDK.GetInfo("=$mobilephone_cardphonelabel$")

                        'Champs textes séparés par des points-virgules : Boîte postale, Adresse étendue, Nom de rue, Ville, Région (ou état/province), Code postal et Pays
                        newAddress(NewVcarIdOther) = ";" & SDK.GetInfo("=$mobilephone_cardaddrext$") & ";" & _
                                    SDK.GetInfo("=$mobilephone_cardaddrstreet$") & ";" & SDK.GetInfo("=$mobilephone_cardaddrcity$") & ";" & _
                                    SDK.GetInfo("=$mobilephone_cardaddrstate$") & ";" & SDK.GetInfo("=$mobilephone_cardaddrstate$") & ";" & _
                                    SDK.GetInfo("=$mobilephone_cardaddrpostalcode$") & ";" & SDK.GetInfo("=$mobilephone_cardaddrcountry$")
                        'newAddress(NewVcarIdOther) = "Test"

                        If NewVcarIdOther < 3 Then
                            NewVcarIdOther += 1
                        Else
                            NewVcarIdOther = 0
                        End If

                        If newPhonesCount < 3 Then
                            newPhonesCount += 1
                        End If

                        'we must to preserve name, prefix, suffix, photo and birthday
                        If newPhones(NewVcarIdOther) = "" Then
                            SDK.Execute("SETVAR;mobilephone_cardphonenumber;||SETVAR;mobilephone_cardphonelabel;||SETVAR;mobilephone_cardphonecount;||" & _
                                        "SETVAR;mobilephone_cardemail;||SETVAR;mobilephone_cardnote;||SETVAR;mobilephone_cardorganisation;||" & _
                                        "SETVAR;mobilephone_cardgeoposition;||" & _
                                        "SETVAR;mobilephone_cardaddrstreet;||SETVAR;mobilephone_cardaddrcity;||SETVAR;mobilephone_cardaddrpostalcode;||" & _
                                        "SETVAR;mobilephone_cardaddrext;||SETVAR;mobilephone_cardaddrstate;||SETVAR;mobilephone_cardaddrcountry;||" & _
                                        "SETVAR;MOBILEPHONE_NBOFPHONEINCONTACT;" & NewVcarIdOther & "/" & newPhonesCount & "||" & _
                                        "SETVAR;PhoneNumberNType;||RELOADSCREEN", True)
                        Else
                            Dim add() As String = newAddress(NewVcarIdOther).Split(";")
                            SDK.Execute("SETVAR;mobilephone_cardphonenumber;" & newPhones(NewVcarIdOther) &
                                        "||SETVAR;mobilephone_cardphonelabel;" & newLabels(NewVcarIdOther) &
                                        "||SETVAR;mobilephone_cardphonecount;" & NewVcarIdOther + 1 & _
                                        "||SETVAR;mobilephone_cardaddrstreet;" & add(2) & _
                                        "||SETVAR;mobilephone_cardaddrcity;" & add(3) & _
                                        "||SETVAR;mobilephone_cardaddrpostalcode;" & add(5) & _
                                        "||SETVAR;mobilephone_cardaddrext;" & add(1) & _
                                        "||SETVAR;mobilephone_cardaddrstate;" & add(4) & _
                                        "||SETVAR;mobilephone_cardaddrcountry;" & add(6) & _
                                        "||SETVAR;mobilephone_cardemail;||SETVAR;mobilephone_cardnote;||SETVAR;mobilephone_cardorganisation;" & _
                                        "||SETVAR;mobilephone_cardgeoposition;||SETVAR;mobilephone_cardnameprefix;||SETVAR;mobilephone_cardnamesuffix;" & _
                                        "||SETVAR;MOBILEPHONE_NBOFPHONEINCONTACT;" & NewVcarIdOther & "/" & newPhonesCount & "||RELOADSCREEN", True)
                        End If

                        If PluginRunForDS = False Then
                            SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL" & _
                                        "||CLADD;Phone '" & SDK.GetInfo("=$mobilephone_cardphonenumber$") & "' is added as '" & SDK.GetInfo("=$mobilephone_typephone$") & "' !!!", True)
                        Else
                            SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL" & _
                                        "||CLADD;Phone '" & SDK.GetInfo("=$mobilephone_cardphonenumber$") & "' is added as '" & SDK.GetInfo("=$mobilephone_typephone$") & "' !!!", True)
                        End If
                    Else

                        If PluginRunForDS = False Then
                            SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL" & _
                                        "||CLADD;Please, enter a name, a phone and a phone type !!!", True)
                        Else
                            SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL" & _
                                        "||CLADD;Please, enter a name, a phone and a phone type !!!", True)
                        End If
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

                ProcessCommand = 2

            Case "mobilephone_cardreset"
                NewVcarIdOther = 0
                SDK.Execute("SETVAR;mobilephone_cardname;||SETVAR;mobilephone_cardphonenumber;||SETVAR;mobilephone_cardphonelabel;||SETVAR;mobilephone_cardphonecount;||" & _
                            "SETVAR;mobilephone_cardimage;||SETVAR;mobilephone_cardemail;||SETVAR;mobilephone_cardnote;||SETVAR;mobilephone_cardorganisation;||" & _
                            "SETVAR;PhoneNumberNType;||SETVAR;mobilephone_cardgeoposition;||SETVAR;mobilephone_cardbirthday;||SETVAR;mobilephone_cardnameprefix;||" & _
                            "SETVAR;MOBILEPHONE_CARDBDAY_DAY;||SETVAR;MOBILEPHONE_CARDBDAY_MONTH;||SETVAR;MOBILEPHONE_CARDBDAY_YEAR;||SETVAR;mobilephone_cardnamesuffix;||" & _
                            "SETVAR;mobilephone_cardaddrstreet;||SETVAR;mobilephone_cardaddrcity;||SETVAR;mobilephone_cardaddrpostalcode;||" & _
                            "SETVAR;mobilephone_cardaddrext;||SETVAR;mobilephone_cardaddrstate;||SETVAR;mobilephone_cardaddrcountry;||SETVAR;MOBILEPHONE_NBOFPHONEINCONTACT;1/1||RELOADSCREEN")
                ProcessCommand = 2
            Case "mobilephone_cardbuild" 'build a contact vcard file
                Try
                    Dim en As CultureInfo = CultureInfo.InvariantCulture
                    Dim newBday As String = ""
                    Dim newName As String = ""

                    newName = SDK.GetInfo("=$mobilephone_cardnameprefix$") & " " & SDK.GetInfo("=$mobilephone_cardname$") & ", " & SDK.GetInfo("=$mobilephone_cardnamesuffix$")

                    newBday = SDK.GetInfo("=$mobilephone_cardbday_year$") & SDK.GetInfo("=$mobilephone_cardbday_month$") & SDK.GetInfo("=$mobilephone_cardbday_day$")
                    If SDK.GetInfo("=$mobilephone_cardbday_year$") <> "" AndAlso SDK.GetInfo("=$mobilephone_cardbday_month$") <> "" AndAlso SDK.GetInfo("=$mobilephone_cardbday_day$") <> "" Then
                        SDK.Execute("SETVAR;mobilephone_cardage;" & Convert.ToString(GetCurrentAge(DateTime.ParseExact(newBday, "yyyyMMdd", en))))
                    End If

                    BlueSoleil_PBAP_CreateNewContactAsVcard(newName,
                                                            newPhones, newLabels, newPhonesCount,
                                                            newAddress, newLabels, newPhonesCount,
                                                            SDK.GetInfo("=$mobilephone_cardimage$"), SDK.GetInfo("=$mobilephone_cardemail$"),
                                                            SDK.GetInfo("=$mobilephone_cardorganisation$"), SDK.GetInfo("=$mobilephone_cardnote$"),
                                                            SDK.GetInfo("=$mobilephone_cardgeoposition$"), newBday)

                    SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||" & _
                                "CLADD;$l_set_BTM_TheContact$ '$mobilephone_cardname$'" & _
                                "||CLADD;$l_set_BTM_VcardIsBuild$ !!!||*ONMOBILEPHONEADDINPHONE", True)
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error Build Contact")
                End Try
                ProcessCommand = 2
            Case "mobilephone_cardsave" 'save a contact into the phone
                btnOPPSaveToPhone()
                SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_MessageInfo||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL" & _
                            "CLADD;The number '" & SDK.GetInfo("=$mobilephone_cardphonenumber$") & "'" & _
                            "||CLADD;is build as vcard !!!||*ONMOBILEPHONEADDINPHONE", True)
                ProcessCommand = 2

            Case "mobilephone_sync", "mobilephone_vcardextract" ' UPDATE Vcard and list FILES from PHONE (OVERWRITES THE CURRENT PC FILES!!)
                Dim t As New Threading.Thread(Sub() RRVcardUpdateProcess(True)) 'event is send to RR after the update
                t.Start()
                ProcessCommand = 2

            Case "mobilephone_phonebooksort"
                Dim t As New Threading.Thread(Sub() BlueSoleil_PBAP_SortvCardbyName()) 'sort vcard and rebuild RR list
                t.Start()
                ProcessCommand = 2

            Case "mobilephone_favorite_add" '1=sms , 0=call
                SDK.Execute("SETVARBYCODE;FAVORITE;CLTEXT||SETVAR;FAVORITETO;0||WAIT;1||LOAD;MOBILEPHONE_FAVORITE.skin")
                ProcessCommand = 2
            Case "mobilephone_dialorselect" 'dial if one number is found or load screen selector if several number are found
                'from favorite's screen
                Try
                    If dialnumberFromFavorite.Contains(",", StringComparison.OrdinalIgnoreCase) = True Then
                        Dim numbersList() As String = dialnumberFromFavorite.Split(",")
                        If BlueSoleil_PBAP_GetVcardId(numbersList(0)) <> -1 Then
                            vcarIdPh = BlueSoleil_PBAP_GetVcardId(numbersList(0))
                        End If
                    Else
                        dialnumberFromFavorite = SDK.GetInfo("CLTEXT")
                        vcarIdPh = BlueSoleil_PBAP_GetVcardId(dialnumberFromFavorite)
                    End If
                    'MessageBox.Show("vcarIdPh --> " & vcarIdPh)
                Catch ex As Exception
                    'MessageBox.Show("1 --> " & ex.Message)
                End Try

                Try

                    'MessageBox.Show(PhoneBookEntries(vcarIdPh).EntryPhoneNumberCount.ToString)
                    If BlueSoleil_PBAP_NumberExistAsVcard(SDK.GetInfo("CLTEXT")) = True Then
                        If PhoneBookEntries(vcarIdPh).EntryPhoneNumberCount > 1 Then
                            For n As Integer = 0 To PhoneBookEntries(vcarIdPh).EntryPhoneNumberCount - 1
                                SDK.Execute("SETVAR;MOBILEPHONE_NUMBER" & n & ";" & PhoneBookEntries(vcarIdPh).EntryPhoneNumbers(n).EntryPhoneNumber & "||" &
                                        "SETVAR;MOBILEPHONE_NAME;" & PhoneBookEntries(vcarIdPh).EntryFullName & "||" &
                                        "SETVAR;CONTACTIMGTOSEND;" & MainPath & "Photo\" & PhoneBookEntries(vcarIdPh).EntryPhoneNumbers(0).EntryPhoneNumber & ".jpg")

                                'MessageBox.Show(BlueSoleil_BS_PBAP_GetImageFromNumber(PhoneBookEntries(vcarIdPh).EntryPhoneNumbers(0).EntryPhoneNumber))

                                If PluginRunForDS = False Then
                                    Select Case UCase(PhoneBookEntries(vcarIdPh).EntryPhoneNumbers(n).Location.Replace(";PREF", ""))
                                        Case "CELL"
                                            SDK.Execute("SETVAR;MOBILEPHONE_NUMBER" & n + 1 & "TYPE;$SKINPATH$Indicators\phone_mobile.png")
                                        Case "HOME"
                                            SDK.Execute("SETVAR;MOBILEPHONE_NUMBER" & n + 1 & "TYPE;$SKINPATH$Indicators\phone_home.png")
                                        Case "WORK"
                                            SDK.Execute("SETVAR;MOBILEPHONE_NUMBER" & n + 1 & "TYPE;$SKINPATH$Indicators\phone_work.png")
                                        Case "FAX"
                                            SDK.Execute("SETVAR;MOBILEPHONE_NUMBER" & n + 1 & "TYPE;$SKINPATH$Indicators\phone_fax.png")
                                        Case Else
                                            SDK.Execute("SETVAR;MOBILEPHONE_NUMBER" & n + 1 & "TYPE;$SKINPATH$Indicators\phone_other.png")
                                    End Select
                                Else
                                    Select Case UCase(PhoneBookEntries(vcarIdPh).EntryPhoneNumbers(n).Location.Replace(";PREF", ""))
                                        Case "CELL"
                                            SDK.Execute("SETVAR;PhoneNumber" & n + 1 & "Type;$SKINPATH$Theme\$ThemeIcons$\MenuIcons\phone_mobile.png")
                                        Case "HOME"
                                            SDK.Execute("SETVAR;PhoneNumber" & n + 1 & "Type;$SKINPATH$Theme\$ThemeIcons$\MenuIcons\phone_home.png")
                                        Case "WORK"
                                            SDK.Execute("SETVAR;PhoneNumber" & n + 1 & "Type;$SKINPATH$Theme\$ThemeIcons$\MenuIcons\phone_work.png")
                                        Case "FAX"
                                            SDK.Execute("SETVAR;PhoneNumber" & n + 1 & "Type;$SKINPATH$Theme\$ThemeIcons$\MenuIcons\phone_fax.png")
                                        Case Else
                                            SDK.Execute("SETVAR;PhoneNumber" & n + 1 & "Type;$SKINPATH$Theme\$ThemeIcons$\MenuIcons\phone_other.png")
                                    End Select

                                End If
                            Next
                            SDK.Execute("MENU;MOBILEPHONE_CHOOSENUMBER.skin")
                        Else
                            SDK.Execute("MOBILEPHONE_DIAL||WAIT;1||MOBILEPHONE_PICKUP")
                        End If
                    Else
                        If BlueSoleil_BS_PBAP_GetImageFromName(PhoneBookEntries(vcarIdPh).EntryFullName) <> "" Then
                            SDK.Execute("SETVAR;CONTACTIMGTOSEND;" & BlueSoleil_BS_PBAP_GetImageFromName(PhoneBookEntries(vcarIdPh).EntryFullName))
                            ToLog("The number '" & dialnumberFromFavorite & "' exist in pb list with another format's number")
                        Else
                            ToLog("The number '" & dialnumberFromFavorite & "' not exist in pb list")
                        End If
                        SDK.Execute("MOBILEPHONE_DIAL||WAIT;1||MOBILEPHONE_PICKUP")
                    End If
                    dialnumberFromFavorite = ""
                Catch ex As Exception
                    MessageBox.Show("2 --> " & ex.Message)
                End Try

                ProcessCommand = 2
            Case "mobilephone_dialselectorreset"
                'reset multi phone selector variables:
                For c As Integer = 0 To 4
                    SDK.Execute("SETVAR;MOBILEPHONE_NUMBER" & c & ";||SETVAR;MOBILEPHONE_NAME;||" &
                                "SETVAR;MOBILEPHONE_NUMBER" & c + 1 & "TYPE;||" &
                                "SETVAR;PhoneNumber" & c + 1 & "Type;")
                Next
                ProcessCommand = 2
            Case "mobilephone_resetphonebook"
                DeleteAllPhoneBook()
                ProcessCommand = 2

            Case "mobilephone_phonebooktype"
                pbapHistoryListIsSimplified = Not pbapHistoryListIsSimplified
                ProcessCommand = 2
                'PHONEBOOK MANAGEMENT COMMANDS

                'New Phone's Contacts
            Case "mobilephone_cch" 'HISTORY PHONE BOOK
                PC_LoadPhoneBook("CCH")
                ProcessCommand = 2
            Case "mobilephone_ich" 'RECEIVED PHONE BOOK
                PC_LoadPhoneBook("ICH")
                ProcessCommand = 2
            Case "mobilephone_och" 'DIALED PHONE BOOK
                PC_LoadPhoneBook("OCH")
                ProcessCommand = 2
            Case "mobilephone_mch" 'MISSED PHONE BOOK
                PC_LoadPhoneBook("MCH")
                ProcessCommand = 2
            Case "mobilephone_pb" 'MT MASTER PHONE BOOK FROM PC
                PC_LoadPhoneBook("PB")
                ProcessCommand = 2
                'New Phone's Contacts

            Case "mobilephone_siri"
                'bluetooth.Siri()
                If hfpVoiceCmdStateEnabled = True Then
                    btnHandsFreeVoiceRecognition(True)
                ElseIf hfpVoiceCmdStateEnabled = False Then
                    btnHandsFreeVoiceRecognition(False)
                End If
                ProcessCommand = 2

            Case "mobilephone_requestphoneinfo"
                RequestInfos()
                ProcessCommand = 2

                'debug form
            Case "mobilephone_debugon"
                debugFrm.Show()
                debugFrm.debugTextBox.AppendText("The debug window is ready ..." & vbCrLf)
                SDK.Execute("SETTOPMOST;MobilePhone Debug Box;TRUE") 'put the debug form on top
                ProcessCommand = 2
            Case "mobilephone_debugoff"
                debugFrm.Hide()
                ProcessCommand = 2
            Case "mobilephone_debugclear"
                debugFrm.debugTextBox.Clear()
                debugFrm.debugTextBox.AppendText("Clear is done ..." & vbCrLf)
                ProcessCommand = 2
                'debug form

                'SMS COMMANDS
            Case "mobilephone_smsmodels"
                If File.Exists(MainPath & "Messages\Models\Models.lst") = False Then
                    Dim models As New ArrayList
                    models.Add("Adore")
                    models.Add("Cool")
                    models.Add("Cry")
                    models.Add("Furious")
                    models.Add("Laugh")
                    models.Add("Pudently")
                    models.Add("Struggle")
                    models.Add("Study")
                    models.Add("Sweet-angel")
                    Dim monStreamWriter As New StreamWriter(MainPath & "Messages\Models\Models.lst", True, Encoding.Unicode)
                    monStreamWriter.WriteLine(" 0")
                    For Each model As String In models
                        monStreamWriter.WriteLine("LST" & model & "||" & model)
                        If PluginRunForDS = False Then
                            monStreamWriter.WriteLine("ICO" & MainPath & "Messages\Models\" & model & ".png")
                        End If
                    Next
                    monStreamWriter.Close()
                    models.Clear()
                    If PluginRunForDS = False Then
                        SDK.Execute("LOAD;MOBILEPHONE_SMSMODELS.skin||CLCLEAR;ALL||CLLOAD;" & MainPath & "Messages\Models\Models.lst||CLFIND;Adore||ONCLPOSCHANGE")
                    Else
                        SDK.Execute("POPUP;MOBILEPHONE_SMSMODELS.skin;5||CLCLEAR;ALL||CLLOAD;" & MainPath & "Messages\Models\Models.lst||SKIPTO;Adore||ONCLPOSCHANGE")
                    End If
                Else
                    If PluginRunForDS = False Then
                        SDK.Execute("LOAD;MOBILEPHONE_SMSMODELS.skin||CLCLEAR;ALL||CLLOAD;" & MainPath & "Messages\Models\Models.lst||CLFIND;Adore||ONCLPOSCHANGE")
                    Else
                        SDK.Execute("POPUP;MOBILEPHONE_SMSMODELS.skin;5||CLCLEAR;ALL||CLLOAD;" & MainPath & "Messages\Models\Models.lst||SKIPTO;Adore||ONCLPOSCHANGE")
                    End If

                End If
                ProcessCommand = 2
            Case "mobilephone_addmodel"
                SDK.Execute("SETVARBYCODE;TEXTTOSEND;CLTEXT||ESC||LOAD;MOBILEPHONE_SMSWRITE.skin")
                ProcessCommand = 2
            Case "mobilephone_smswrite"
                'Check Lock Motion used or not
                If TempPluginSettings.LockInMotion = True Then
                    If PluginRunForDS = False Then
                        If Convert.ToUInt16(SDK.GetInfo("GPSSPDN")) > 0 Then
                            SDK.ErrScrn("MobilePhone Info", "!!! " & SDK.GetInfo("=$l_set_BTM_StopYourVehicule$") & " !!!", "****************  Sorry  ***************", 5)
                        Else
                            SDK.Execute("SETVAR;FAVORITETO;1||SETVARBYCODE;CONTACTNAMETOSEND;CLDESC||SETVARBYCODE;CONTACTNUMBERTOSEND;CLTEXT" & _
                                "||SETVARBYCODE;CONTACTIMGTOSEND;CLIMG||SETVAR;MOBILEPHONE_SMSINFO;...||LOAD;MOBILEPHONE_SMSWRITE.skin")
                        End If

                    Else
                        If Convert.ToUInt16(SDK.GetInfo("GPSSPDN")) > 0 Then
                            SDK.Execute("POPUP;MOBILEPHONE_LOCKINMOTION.SKIN;5")
                        Else
                            SDK.Execute("SETVAR;FAVORITETO;1||SETVARBYCODE;CONTACTNAMETOSEND;CLDESC||SETVARBYCODE;CONTACTNUMBERTOSEND;CLTEXT" & _
                                      "||SETVAR;MOBILEPHONE_SMSINFO;...||LOAD;MOBILEPHONE_SMSWRITE.skin")
                        End If
                    End If
                Else
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVAR;FAVORITETO;1||SETVARBYCODE;CONTACTNAMETOSEND;CLDESC||SETVARBYCODE;CONTACTNUMBERTOSEND;CLTEXT" & _
                                "||SETVARBYCODE;CONTACTIMGTOSEND;CLIMG||SETVAR;MOBILEPHONE_SMSINFO;...||LOAD;MOBILEPHONE_SMSWRITE.skin")
                    Else
                        SDK.Execute("SETVAR;FAVORITETO;1||SETVARBYCODE;CONTACTNAMETOSEND;CLDESC||SETVARBYCODE;CONTACTNUMBERTOSEND;CLTEXT" & _
                                    "||SETVAR;MOBILEPHONE_SMSINFO;...||LOAD;MOBILEPHONE_SMSWRITE.skin")
                    End If
                End If
                ProcessCommand = 2

            Case "onfileclick"
                If PluginRunForDS = False AndAlso LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_attachfile.skin" Then
                    SDK.Execute("SETVARBYCODE;imagefilename;LISTTEXT")
                End If
                ProcessCommand = 2
            Case "mobilephone_addattachfile"
                SDK.Execute("SETVAR;MOBILEPHONE_PATHATTCHFILE;" & MainPath & "Photo\||MENU;MOBILEPHONE_ATTACHFILE.SKIN")
                ProcessCommand = 2
            Case "mobilephone_sendsms" 'SMS/MMS write by MAP feature
                Try
                    Dim pathAttachFile As String = SDK.GetInfo("=$MOBILEPHONE_PATHATTCHFILE$$IMAGEFILENAME$")
                    If File.Exists(pathAttachFile) = True Then
                        btnSendMMS("TEST", SDK.GetInfo("=$CONTACTNUMBERTOSEND$"), SDK.GetInfo("=$TEXTTOSEND$"), pathAttachFile)
                        If PluginRunForDS = False Then
                            SDK.Execute("SETVAR;POPUP;MMS is send !||MENU;POPUP.SKIN", True)
                        Else
                            SDK.Execute("SETVAR;MOBILEPHONE_PATHATTCHFILE;||SETVAR;IMAGEFILENAME;||SETVAR;l_ReadingPhoneBook;MMS is send !||POPUP;ReadingPhoneBook.SKIN;5")
                        End If
                    Else
                        btnSendText(SDK.GetInfo("=$CONTACTNUMBERTOSEND$"), SDK.GetInfo("=$TEXTTOSEND$"))
                        If PluginRunForDS = False Then
                            SDK.Execute("SETVAR;POPUP;SMS is send !||MENU;POPUP.SKIN", True)
                        Else
                            SDK.Execute("SETVAR;MOBILEPHONE_PATHATTCHFILE;||SETVAR;IMAGEFILENAME;||SETVAR;l_ReadingPhoneBook;SMS is send !||POPUP;ReadingPhoneBook.SKIN;5")
                        End If
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
                ProcessCommand = 2

            Case "mobilephone_sendposition" 'http://maps.google.fr/maps?f=q&hl=fr&q=" & LatLonValues
                Dim LatLonValues As String = Replace(SDK.GetInfo("GPSLAT"), ",", ".") & "," & Replace(SDK.GetInfo("GPSLON"), ",", ".")
                'btnSendText(num, "http://maps.google.fr/maps?f=q&hl=fr&q=" & LatLonValues)
                'https://yandex.com/maps/123471/merignac/?ll=-0.813048%2C44.841571&z=11
                'https://www.bing.com/mapspreview?cp=47.677797~-122.122013
                SDK.Execute("SETVAR;TEXTTOSEND;" & SDK.GetInfo("=$l_set_BTM_ItsMyPosition$") & vbCrLf & vbCrLf & "Google Maps:" & vbCrLf & "http://maps.google.fr/maps?f=q&hl=fr&q=" & LatLonValues & vbCrLf & _
                                            "Bing maps:" & vbCrLf & "https://www.bing.com/mapspreview?cp=" & LatLonValues.Replace(",", "~") & vbCrLf & _
                                            "Yandex Maps:" & vbCrLf & "https://yandex.com/maps/123471/merignac/?ll=" & LatLonValues.Replace(",", "%2C") & "&z=11")
                ToLog("position send to contact")
                ProcessCommand = 2


            Case "mobilephone_mmsread"
                SDK.Execute("LOAD;MOBILEPHONE_SMSREAD.skin")
                Dim t As New Threading.Thread(Sub() btnGetMessages(SmsUnreadOnly, True)) 'extract MMS msg
                t.Start()
                timerMapCounter = 0 'reset check counter
                ProcessCommand = 2
            Case "mobilephone_smsread" 'build map_MsgHistoryItems
                'Check Lock Motion used or not
                If TempPluginSettings.LockInMotion = True Then
                    If PluginRunForDS = False Then
                        If Convert.ToUInt16(SDK.GetInfo("GPSSPDN")) > 0 Then
                            SDK.ErrScrn("MobilePhone Info", "!!! " & SDK.GetInfo("=$l_set_BTM_StopYourVehicule$") & " !!!", "****************  Sorry  ***************", 5)
                        Else
                            SDK.Execute("LOAD;MOBILEPHONE_SMSREAD.skin")
                            Dim t As New Threading.Thread(Sub() btnGetMessages(SmsUnreadOnly)) 'extract msg
                            t.Start()
                            timerMapCounter = 0 'reset check counter
                        End If
                    Else
                        If Convert.ToUInt16(SDK.GetInfo("GPSSPDN")) > 0 Then
                            SDK.Execute("POPUP;MOBILEPHONE_LOCKINMOTION.SKIN;5")
                        Else
                            SDK.Execute("LOAD;MOBILEPHONE_SMSREAD.skin")
                            Dim t As New Threading.Thread(Sub() btnGetMessages(SmsUnreadOnly)) 'extract msg
                            t.Start()
                            timerMapCounter = 0 'reset check counter
                        End If
                    End If
                Else
                    SDK.Execute("LOAD;MOBILEPHONE_SMSREAD.skin")
                    Dim t As New Threading.Thread(Sub() btnGetMessages(SmsUnreadOnly)) 'extract msg
                    t.Start()
                    timerMapCounter = 0 'reset check counter
                End If
                ProcessCommand = 2

            Case "mobilephone_smsview" 'view msg (unred or all) into map_MsgHistoryItems
                If map_MsgHistoryItems(msgId).msgFromName = "****" Then
                    map_MsgHistoryItems(msgId).msgFromName = SDK.GetInfo("=$l_set_BTM_Unknown$")
                End If
                SDK.Execute("SETVAR;mobilephone_msgfullname;" & map_MsgHistoryItems(msgId).msgFromName & "||" & _
                            "SETVAR;mobilephone_msgphonenumber;" & map_MsgHistoryItems(msgId).msgFromNumber & "||" & _
                            "SETVAR;mobilephone_msghandle;" & map_MsgHistoryItems(msgId).msgHandle & "||" & _
                            "SETVAR;mobilephone_msgreadstate;" & map_MsgHistoryItems(msgId).msgReadState & "||" & _
                            "SETVAR;mobilephone_msgnumberofmsg;" & map_MsgHistoryCount.ToString & "||" & _
                            "SETVAR;mobilephone_msgdatetime;" & map_MsgHistoryItems(msgId).msgDateTime.ToString & "||" & _
                            "SETVAR;mobilephone_msgimage;" & map_MsgHistoryItems(msgId).msgImage & "||" & _
                            "SETVAR;mobilephone_msgid;" & (msgId + 1).ToString & "||" & _
                            "SETVAR;mobilephone_msgattachsize;" & map_MsgHistoryItems(msgId).msgAttachmentSizes.ToString & "||" & _
                            "SETVAR;mobilephone_msgattachname;" & map_MsgHistoryItems(msgId).msgAttachmentName & "||" & _
                            "SETVAR;mobilephone_msgtype;" & map_MsgHistoryItems(msgId).msgType & "||" & _
                            "LOAD;MOBILEPHONE_SMSREAD.skin")

                If map_MsgHistoryItems(msgId).msgAttachmentSizes <> 0 Then
                    msgAttachExist = True
                Else
                    msgAttachExist = False
                End If

                If map_MsgHistoryItems(msgId).msgText.IndexOf(vbLf) <> -1 Then
                    Dim spText() As String = map_MsgHistoryItems(msgId).msgText.Split(vbLf)
                    For w As Integer = spText.Length - 1 To 0 Step -1 ' inversé pour compenser l'inversion à la lecture.
                        SDK.Execute("CLADD;" & spText(w))
                    Next
                Else
                    SDK.Execute("CLADD;" & map_MsgHistoryItems(msgId).msgText)
                End If
                ProcessCommand = 2

            Case "msgnames"
                ToLog("MessagesNames:")
                For l As Integer = 0 To map_MsgHistoryCount - 1
                    ToLog(map_MsgHistoryItems(l).msgFromName)
                Next
                ProcessCommand = 2
            Case "booknames"
                ToLog("BookNames:")
                For m As Integer = 0 To PhoneBookEntryCount - 1
                    ToLog(PhoneBookEntries(m).EntryName)
                Next
                ProcessCommand = 2
            Case "test"
                ToLog("test: " & BlueSoleil_BS_PBAP_GetImageFromName("Kristie"))
                ProcessCommand = 2

            Case "mobilephone_viewmsgmore"
                If msgId < map_MsgHistoryCount - 1 Then
                    msgId += 1
                End If
                SDK.Execute("CLCLEAR;ALL||mobilephone_smsview")
                ProcessCommand = 2
            Case "mobilephone_viewmsgless"
                If msgId > 0 Then
                    msgId -= 1
                End If
                SDK.Execute("CLCLEAR;ALL||mobilephone_smsview")
                ProcessCommand = 2

            Case "mobilephone_saymessage"
                If map_MsgHistoryItems(msgId).msgText.IndexOf(vbLf) <> -1 Then
                    Dim spText() As String = map_MsgHistoryItems(msgId).msgText.Split(vbLf)
                    For w As Integer = spText.Length - 1 To 0 Step -1 ' inversé pour compenser l'inversion à la lecture.
                        If spText(w).Contains("Subject:") = True Then
                            SDK.Execute("SAY;" & spText(w))
                        End If
                    Next
                Else
                    SDK.Execute("SAY;" & map_MsgHistoryItems(msgId).msgText)
                End If
                ProcessCommand = 2

            Case "mobilephone_resetpopupmap" 'for tests only
                mapMsgPopUpAllreadySend = False
                ProcessCommand = 2
                'SMS COMMANDS

                'A2DP COMMANDS
            Case "mobilephone_a2dponoff"
                If a2dpIsActive = False Then
                    btnA2DPDConnect()
                ElseIf a2dpIsActive = True Then
                    btnA2DPDisconnect()
                End If
                ProcessCommand = 2
                'A2DP COMMANDS

                'PAN COMMANDS
            Case "mobilephone_panonoff"
                If panIsActive = False Then
                    btnTether()
                ElseIf panIsActive = True Then
                    btnUnTether()
                End If
                ProcessCommand = 2
                'PAN COMMANDS

                'SPP COMMANDS
            Case "mobilephone_spponoff"
                If sppIsActive = False Then
                    btnSppConnect()
                Else
                    btnSppDisConnect()
                End If
                ProcessCommand = 2
                'SPP COMMANDS

                'AVRCP Commands
            Case "mobilephone_avrcponoff"
                If avrcpIsActive = False Then
                    btnMediaConnect()
                ElseIf avrcpIsActive = True Then
                    btnMediaStop()
                    btnMediaDisconnect()
                End If
                ProcessCommand = 2
            Case "mobilephone_avrplay"
                If avrcpIsPlaying = False Then
                    btnMediaPlay()
                Else
                    btnMediaStop()
                End If
                ProcessCommand = 2
            Case "mobilephone_avrpause"
                btnMediaPause()
                ProcessCommand = 2
            Case "mobilephone_avrstop"
                btnMediaStop()
                ProcessCommand = 2
            Case "mobilephone_avrprev"
                btnMediaPrev()
                ProcessCommand = 2
            Case "mobilephone_avrnext"
                btnMediaNext()
                ProcessCommand = 2
            Case "mobilephone_avrmute"
                btnMediaMute()
                ProcessCommand = 2

            Case "mobilephone_avrrepeat"
                SDK.Execute("ClickInd;mobilephone_avrrepeat")
                btnMediaSetPlayerSetting(AVRCP_repeat, AVRCP_shuffle) 'cbAVrepeat.Checked, cbAVshuffle.Checked
                ProcessCommand = 2
            Case "mobilephone_avrshuffle"
                SDK.Execute("ClickInd;mobilephone_avrshuffle")
                btnMediaSetPlayerSetting(AVRCP_repeat, AVRCP_shuffle) 'cbAVrepeat.Checked, cbAVshuffle.Checked
                ProcessCommand = 2

            Case "mobilephone_avrvolup"
                btnMediaVolUp()
                ProcessCommand = 2
            Case "mobilephone_avrvoldown"
                btnMediaVolDown()
                ProcessCommand = 2
            Case "mobilephone_avrbrowsingok"
                btnMediaEnableBrowsing()
                ProcessCommand = 2
            Case "mobilephone_avrgetlist"
                btnMediaGetLists()
                ProcessCommand = 2
            Case "mobilephone_avrgetfilesystem"
                btnMediaGetFileSystem()
                ProcessCommand = 2
                'AVRCP Commands

                '###############################  Interface RRTranslator  #############################
            Case "mobilephone_translate" 'MOBILEPHONE_MESSAGE
                If SDK.GetInd("GTRANSLATOR") = "True" Then
                    If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smswrite.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smswrite" Then
                        SDK.Execute("SETVARFROMVAR;GTRANSLATOR_FROMTEXT;TEXTTOSEND||GTRANS_TRANSLATE||" & _
                                    "WAIT;2||SETVARFROMVAR;TEXTTOSEND;GTRANSLATOR_TOTEXT||SETVARFROMVAR;TEXTTOSEND;GTRANSLATOR_TOTEXT")
                    ElseIf LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smsread.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smsread" Then
                        SDK.Execute("SETVARBYCODE;GTRANSLATOR_FROMTEXT;CLTEXT||GTRANS_TRANSLATE||" & _
                                    "CLCLEAR;ALL||CLADD;" + SDK.GetInfo("=$GTRANSLATOR_TOTEXT$"))
                    End If
                Else
                    If PluginRunForDS = False Then
                        SDK.ErrScrn("Translator Error !!!", "If you want to use this function,", " you need to install the plugin RRTranslator ...", 5)
                    Else
                        SDK.Execute("POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||SETVAR;MOBILEPHONE_INFO;Settings Info||" & _
                                    "CLADD;Please, Install the plugin RRTranslator")
                    End If
                End If
                ProcessCommand = 2
            Case "mobilephone_fromlanguage"
                If SDK.GetInd("GTRANSLATOR") = "True" Then
                    'If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_settings2.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_settings2" Then
                    SDK.Execute("GTRANS_FROMLANG")
                    'End If
                Else
                    If PluginRunForDS = False Then
                        SDK.ErrScrn("Translator Error !!!", "If you want to use this function,", " you need to install the plugin RRTranslator ...", 5)
                    Else
                        SDK.Execute("POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||SETVAR;MOBILEPHONE_INFO;Settings Info||" & _
                                    "CLADD;Please, Install the plugin RRTranslator")
                    End If
                End If
                ProcessCommand = 2
            Case "mobilephone_tolanguage"
                If SDK.GetInd("GTRANSLATOR") = "True" Then
                    'If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_settings2.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_settings2" Then
                    SDK.Execute("GTRANS_TOLANG")
                    'End If
                Else
                    If PluginRunForDS = False Then
                        SDK.ErrScrn("Translator Error !!!", "If you want to use this function,", " you need to install the plugin RRTranslator ...", 5)
                    Else
                        SDK.Execute("POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||SETVAR;MOBILEPHONE_INFO;Settings Info||" & _
                                    "CLADD;Please, Install the plugin RRTranslator")
                    End If
                End If
                ProcessCommand = 2
            Case "ongtrans_changelang" 'RR event returned by RRTranslator each time the TO language is modified ! 
                Dim oldLanguage As String = TempPluginSettings.Language
                If LCase(SDK.GetInfo("=$GTRANSLATOR_TOLANGUAGE$")) <> oldLanguage Then
                    TempPluginSettings.Language = LCase(SDK.GetInfo("=$GTRANSLATOR_TOLANGUAGE$"))
                    If File.Exists(MainPath & "Languages\" & TempPluginSettings.Language & "\" & TempPluginSettings.Language & ".txt") = True Then
                        If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                            cMySettings.Copy(TempPluginSettings, PluginSettings)
                            cMySettings.SerializeToXML(PluginSettings)
                            ReadLanguageVars()
                        End If
                        SDK.Execute("MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||SETVAR;MOBILEPHONE_INFO;New Language||CLADD;" & _
                                    SDK.GetInfo("=$l_set_BTM_NewLanguage$") & "||CLADD;||CLADD;" & TempPluginSettings.Language)
                    Else
                        TempPluginSettings.Language = oldLanguage
                        SDK.Execute("MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||SETVAR;MOBILEPHONE_INFO;Only Language Usable" & _
                                    "||CLLOAD;" & MainPath & "Languages.txt")

                    End If
                End If

                ProcessCommand = 2
                '###############################  Interface RRTranslator  #############################

                'SETTINGS
            Case "mobilephone_settings"
                If File.Exists(MainPath & "MobilePhone_DevicesList.lst") = False Then
                    SDK.Execute("LOAD;MOBILEPHONE_SETTINGS.skin||CLCLEAR;ALL||SetVarFromVar;MOBILEPHONE_SETTINGSINFO;l_set_BTM_DeviceListIsUpdated")
                    Search_Device()
                Else
                    PhoneCheckedIs = 1
                    SDK.Execute("mobilephone_settings_setvarphone||SetVarFromVar;MOBILEPHONE_SETTINGSINFO;l_set_BTM_ClickOnApply||LOAD;MOBILEPHONE_SETTINGS.skin" & _
                            "||CLCLEAR;ALL||CLLOAD;" & MainPath & "MobilePhone_DevicesList.lst||SEARCHLIST;$SEARCHPHONE$||MOBILEPHONE_SETTINGS_UPDATEINFODEVICE")

                End If
                ProcessCommand = 2
            Case "mobilephone_settings_default"
                cMySettings.SetToDefault(PluginSettings, True)
                ' copy to temp (skin views temp)
                cMySettings.Copy(PluginSettings, TempPluginSettings)
                cMySettings.SerializeToXML(PluginSettings)
                ProcessCommand = 2
            Case "mobilephone_settings_apply"
                Try
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                    Dim t As New Threading.Thread(Sub() hfpDisconnectAfterChange())
                    t.Start()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error in setting apply")
                End Try
                ProcessCommand = 2
            Case "mobilephone_settings_setvarphone"
                If PhoneCheckedIs = 1 Then
                    SDK.Execute("SETVAR;SEARCHPHONE;" & PluginSettings.PhoneDeviceName)
                ElseIf PhoneCheckedIs = 2 Then
                    SDK.Execute("SETVAR;SEARCHPHONE;" & PluginSettings.PhoneDeviceName2)
                End If
                ProcessCommand = 2

            Case "mobilephone_settings_cancel"
                cMySettings.Copy(PluginSettings, TempPluginSettings)
                ProcessCommand = 2
            Case "mobilephone_settings_updatedevicelist"
                Search_Device()
                ProcessCommand = 2
            Case "mobilephone_settings_updateinfodevice" ' "onclposchange"
                If Not File.Exists(MainPath & "MobilePhone_DevicesList.lst") Then
                    Search_Device()
                Else
                    Dim descriptionDevice() As String
                    If PhoneCheckedIs = 1 Then
                        If TempPluginSettings.PhoneDeviceName = PluginSettings.PhoneDeviceName And hfpIsActive = True Then
                            PhoneStatus = SDK.GetInfo("=$l_set_BTM_PhoneStatusInfo$") & ": " & SDK.GetInfo("=$l_set_BTM_PhoneStatusIsConnected$")
                        Else
                            PhoneStatus = SDK.GetInfo("=$l_set_BTM_PhoneStatusInfo$") & ": " & SDK.GetInfo("=$l_set_BTM_PhoneStatusIsNotConnected$")
                        End If

                    ElseIf PhoneCheckedIs = 2 Then
                        If TempPluginSettings.PhoneDeviceName2 = PluginSettings.PhoneDeviceName2 And hfpIsActive = True Then
                            PhoneStatus = "Connected"
                        Else
                            PhoneStatus = "Not Connected"
                        End If
                    End If
                    SDK.Execute("SETVAR;MOBILEPHONE_CLDESC;" & SDK.GetInfo("CLDESC"))
                    descriptionDevice = SDK.GetInfo("CLDESC").Replace(" --> ", ",").Split(",")
                    If PhoneCheckedIs = 1 Then TempPluginSettings.PhoneDeviceName = descriptionDevice(0)
                    If PhoneCheckedIs = 2 Then TempPluginSettings.PhoneDeviceName2 = descriptionDevice(0)
                    hfpdevicemacaddress = SDK.GetInfo("CLTEXT")
                    hfpdevicetype = descriptionDevice(1)
                End If
                ProcessCommand = 2

            Case "mobilephone_settings_updateremotelist"
                Search_RemoteServices()
                ProcessCommand = 2
            Case "mobilephone_settings_updatelocallist"
                Search_LocalServices()
                ProcessCommand = 2
            Case "mobilephone_remoteservicesusable"
                CheckPhone_bluetooth_Services()
                ProcessCommand = 2

            Case "mobilephone_update_emergencynumber"
                TempPluginSettings.EmergencyNumber = SDK.GetInfo("=$MOBILEPHONE_EMERGENCYNUMBER$")
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                ProcessCommand = 2
            Case "mobilephone_update_vocalmessagerynumber"
                TempPluginSettings.VocalMessageryNumber = SDK.GetInfo("=$MOBILEPHONE_VOCALMESSAGERYNUMBER$")
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                ProcessCommand = 2

            Case "mobilephone_update_phoneculture"
                TempPluginSettings.PhoneCountryCodes(1) = SDK.GetInfo("=$MOBILEPHONE_PHONECULTURE$")
                'MessageBox.Show(PluginSettings.PhoneCountryCodes & "  " & TempPluginSettings.PhoneCountryCodes)
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                ProcessCommand = 2

            Case "mobilephone_pairedpincode"
                btnPairing_PairByPincode("Galaxy A5 (2016)", "")
            Case "mobilephone_paired"
                btnPairing_Pair(DeviceNameFromCLDesc(SDK.GetInfo("CLDESC")))
                ProcessCommand = 2
            Case "mobilephone_unpaired"
                btnPairing_UnPair(DeviceNameFromCLDesc(SDK.GetInfo("CLDESC")))
                ProcessCommand = 2
            Case "mobilephone_deletepaired"
                btnDeleteDvc(DeviceNameFromCLDesc(SDK.GetInfo("CLDESC")))
                ProcessCommand = 2

            Case "mobilephone_phoneprefixbycountry"
                SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_BTM_PhoneCountryPrefix||MENU;MOBILEPHONE_INFO.SKIN||CLCLEAR;ALL||WAIT;.5||mobilephone_prefixbycountry")
                ProcessCommand = 2
            Case "mobilephone_prefixbycountry" 'internal only (for compatibility cladd in icards)
                CountryToPhoneIndicatifList()
                ProcessCommand = 2

            Case "mobilephone_speech_recognitionhelp"
                If TempPluginSettings.PhoneSpeechRecognition = True Then
                    SDK.Execute("MENU;MOBILEPHONE_SPEECHHELP.skin||CLCLEAR;ALL")
                    Dim allLines() As String = Split(SDK.GetInfo("=$l_set_BTM_PhoneSpeechNumbers$"), ",")
                    For ph As Integer = 0 To allLines.Length - 1
                        'frm.CLAddItem(allLines(ph) & vbCrLf & allLines(ph) & " --> Nothing")
                        SDK.Execute("CLADD;" & allLines(ph) & ";" & allLines(ph)) ' & " --> Nothing")
                    Next
                Else
                    If PluginRunForDS = False Then
                        SDK.ErrScrn("MobilePhone Error", "Speech Recognition is OFF!!!", "Set 'PhoneSpeechRecognition' to 'True'", 5)
                    Else
                        SDK.Execute("SETVAR;MOBILEPHONE_INFO;Speech Recognition is OFF!!!" & "||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||" & _
                                    "CLCLEAR;ALL||CLADD;Set 'PhoneSpeechRecognition' to 'True'")
                    End If
                End If
                ProcessCommand = 2
                'SETTINGS

                '##############################  Interface GoogleMapsTools ############################
            Case "mobilephone_unformatednumber" 'envoyé par un plugin (par exemple RRGoogleMapsTools)
                dialNumber = BlueSoleil_BS_PBAP_CleanPhoneNumber(SDK.GetInfo("=$MOBILEPHONE_UNFORMATEDNUMBER$"))
                ProcessCommand = 2

                'BYVARX;WhatdoYouWant;(OSKTOVAR;GOADDRESS<<gmaps_placetypetosearch<<OSKTOVAR;GOADDRESS<<OSKTOVAR;GOADDRESS)
            Case "mobilephone_viewgeoposition"
                'If SDK.GetInd("rrgooglemapstools") = "True" Then 'à ajouter dans RRGoogleMapsTools
                '    'SDK.Execute("MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||SETVAR;MOBILEPHONE_INFO;MobilePhone Info||CLADD;Plugin RRGoogleMapsTools found !", True)
                '    'gmaps_goaddress;
                '    'gmaps_golatlon;
                'Else
                If PluginRunForDS = False Then
                    SDK.ErrScrn("MobilePhone Plugin Info !!!", "Plugin RRGoogleMapsTools not found !", "Please, install this plugin for use this feature !", 5)
                Else
                    SDK.Execute("SETVAR;MOBILEPHONE_INFO;Plugin RRGoogleMapsTools not found !||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||CLADD;Please, install this plugin!")
                End If
                'End If
                ProcessCommand = 2
                '##############################  Interface GoogleMapsTools ############################

                '##############################  Envoie au GPS par defaut  ############################
            Case "mobilephone_sendtogps"
                If PluginRunForDS = False Then
                    SDK.ErrScrn("MobilePhone Plugin Info !!!", "This feature isn't ready !!!", "... Sorry ...", 5)
                Else
                    SDK.Execute("SETVAR;MOBILEPHONE_INFO;This feature isn't ready !!!||POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||CLADD;... Sorry ...")
                End If
                ProcessCommand = 2
                '######################################################################################

            Case "mobilephone_phoneswap"
                SwapDevicePhone()
                ProcessCommand = 2

            Case "mobilephone_sysinfo"
                Dim t As New Threading.Thread(AddressOf GetVersionFromRegistry)
                t.Start()
                ProcessCommand = 2

            Case "mobilephone_timer1onoff"
                If timer1.Enabled = False Then
                    timer1.Enabled = True
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVAR;POPUP;Main Timer is ON|||MENU;POPUP.SKIN", True)
                    Else
                        SDK.Execute("SETVAR;l_ReadingPhoneBook;Main Timer is ON||POPUP;ReadingPhoneBook.SKIN;5", True)
                    End If
                Else
                    timer1.Enabled = False
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVAR;POPUP;Main Timer is OFF|||MENU;POPUP.SKIN", True)
                    Else
                        SDK.Execute("SETVAR;l_ReadingPhoneBook;Main Timer is OFF||POPUP;ReadingPhoneBook.SKIN;5", True)
                    End If
                End If
                ProcessCommand = 2
            Case "mobilephone_timer2onoff"
                If timer2.Enabled = False Then
                    timer2.Enabled = True
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVAR;POPUP;Check Phones Timer is ON|||MENU;POPUP.SKIN", True)
                    Else
                        SDK.Execute("SETVAR;l_ReadingPhoneBook;Check Phones Timer is ON||POPUP;ReadingPhoneBook.SKIN;5", True)
                    End If
                Else
                    timer2.Enabled = False
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVAR;POPUP;Check Phones Timer is OFF|||MENU;POPUP.SKIN", True)
                    Else
                        SDK.Execute("SETVAR;l_ReadingPhoneBook;Check Phones Timer is OFF||POPUP;ReadingPhoneBook.SKIN;5", True)
                    End If
                End If
                ProcessCommand = 2

            Case "mobilephone_atcmdonoff"
                TempPluginSettings.PhoneExecATCmd = Not TempPluginSettings.PhoneExecATCmd
                ProcessCommand = 2

            Case "mobilephone_mapoff"
                btnMsgDisconnect()
                ProcessCommand = 2

            Case "mobilephone_network"
                Net_CheckBetterNetwork()
                ProcessCommand = 2

            Case "mobilephone_stopallpopup"
                TempPluginSettings.PhoneNoSMSPopupInfo = Not TempPluginSettings.PhoneNoSMSPopupInfo
                cMySettings.Copy(TempPluginSettings, PluginSettings)
                cMySettings.SerializeToXML(PluginSettings)
                ProcessCommand = 2

            Case "mobilephone_checkifsmsread" 'internal only
                If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smsread" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smsread.skin" Then
                    SDK.Execute("SETVARFROMVAR;MOBILEPHONE_SMSINFO;l_set_BTM_UnreadSmSInfo")
                    'MessageBox.Show("SETVARFROMVAR;MOBILEPHONE_SMSINFO;l_set_BTM_UnreadSmSInfo")
                Else
                    'MessageBox.Show("NOTHING")
                    SDK.Execute("SETVAR;MOBILEPHONE_SMSINFO;-")
                End If
                ProcessCommand = 2

        End Select

        'mobilephone_searchinlist;texttosearch
        If LCase(Left$(CMD, 25)) = "mobilephone_searchinlist;" Then
            If Mid(CMD, 26).StartsWith("$") And Mid(CMD, 26).EndsWith("$") Then
                SerListInPhoneBook(SDK.GetInfo("=" & Mid(CMD, 26)))
            Else
                SerListInPhoneBook(Mid(CMD, 26))
            End If

            ''If List already visible, search that one
            'If frm.ShowPL Then s = frm.Tag

            ''Searches Current List (start from current item)
            'With Menus(IsLoaded(s))
            '    .CurrControl.Value = .CurrControl.Value + 1
            '    For t = .CurrControl.Value To .CurrControl.Max
            '        If InStr(1, .CurrControl.GetItem(t), frm.txt(0).Text, vbTextCompare) > 0 Then .CurrControl.Value = t : Exit For
            '    Next t
            'End With

            ''If Playlist is already on screen, don't exit
            'If frm.ShowPL Then GoTo DoneExec
            ProcessCommand = 2



        End If

        'mobilephone_changlang;419
        If LCase(Left$(CMD, 22)) = "mobilephone_changlang;" Then
            'If LCase(SDK.GetInfo("LANGNAME")) = "ru" Then
            '    SDK.Execute("SETVAR;KBLAYOUT;1")
            'Else
            '    SDK.Execute("SETVAR;KBLAYOUT;0")
            'End If
            If LCase(Mid(CMD, 23)) = "ru" Then
                Changelangue(&H419) 'russian
                'SDK.Execute("SETVAR;KBLAYOUT;1")
            ElseIf LCase(Mid(CMD, 23)) = "fr" Then
                Changelangue(&H40C) 'french
                'SDK.Execute("SETVAR;KBLAYOUT;0")
            Else
                Changelangue(Mid(CMD, 23))
            End If
            ProcessCommand = 2
        End If

        'stop or restart MAP and Network check
        'mobilephone_stopallcheck;0 or 1'
        If LCase(Left$(CMD, 25)) = "mobilephone_stopallcheck;" Then
            If Mid(CMD, 26) = "0" Then
                TempPluginSettings.PhoneNoSMSPopupInfo = False
            ElseIf Mid(CMD, 26) = "1" Then
                TempPluginSettings.PhoneNoSMSPopupInfo = True
            End If
            cMySettings.Copy(TempPluginSettings, PluginSettings)
            cMySettings.SerializeToXML(PluginSettings)
            ProcessCommand = 2
        End If

        'mobilephone_smsviewnumber;0 to x
        'If LCase(Left$(CMD, 26)) = "mobilephone_smsviewnumber;" Then
        '    Try
        '        btnDownloadMessage(Mid(CMD, 27))
        '    Catch ex As Exception
        '        MessageBox.Show(ex.Message)
        '    End Try
        '    ProcessCommand = 2
        'End If

        'mobilephone_pn;number;N or I
        If LCase(Left$(CMD, 15)) = "mobilephone_pn;" Then
            Try
                'hfpCallerIDnoFormatted = New PhoneNumberFormatter(TempPluginSettings.PhoneCountryCodes(0), TempPluginSettings.PhoneCountryCodes(1), TempPluginSettings.PhoneCountryCodes(2))
                'Dim hfpCallerIDnoFormatted As PhoneNumberFormatter = New PhoneNumberFormatter(TempPluginSettings.PhoneCountryCodes(0), TempPluginSettings.PhoneCountryCodes(1), TempPluginSettings.PhoneCountryCodes(2))
                'MessageBox.Show(hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber("0603363212", OutputFormats.International))
                'MessageBox.Show(hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber("+33603363212", OutputFormats.National))
                'MessageBox.Show(hfpCallerIDnoFormatted.ConvertToUnformatedPhoneNumber("+33603363212"))
                'Dim stringValue As String = Mid(CMD, 16)
                'MessageBox.Show(String.Format(New NumericStringFormatter(), "{0} (formatted: {0:###-##-####})", stringValue))

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
            ProcessCommand = 2
        End If

        'mobilephone_pn2;number;format
        '    If LCase(Left$(CMD, 16)) = "mobilephone_pn2;" Then
        '        Try
        '            'Dim stringValue As String = Mid(CMD, 17)
        '            ''This outputs: +91 (123) 456-7890
        '            ''Dim phoneNumber As String = "+91 1234567890"
        '            'MessageBox.Show(String.Format(New PhoneFormatter(), "{0:(###) ###-####}", stringValue))
        '            ''MessageBox.Show(String.Format(New PhoneFormatter(), s(1), s(0)))
        '            ''Similarly, following code produces (123) 456-7890
        '            'Dim phoneNumber2 As String = "1234567890"
        '            'MessageBox.Show(String.Format(New PhoneFormatter(), "{0:(###) ###-####}", phoneNumber2))

        '            ''Other ne marche pas
        '            'Dim phoneNumber3 As String = "+33102030405"
        '            'MessageBox.Show(String.Format(New PhoneFormatter(), "{0:##.##.##.##.##}", phoneNumber3))
        '            'Dim v_phone As New PhoneNumber(33, 6, 3, 36, 32, 12)
        '            'MessageBox.Show(v_phone.StationCode)
        '            'MessageBox.Show(String.Format("Is NANP? {0}", v_phone.IsNanpValid))

        ''https://github.com/googlei18n/libphonenumber
        '            'Dim phoneUtil As PhoneNumberUtil = PhoneNumberUtil.GetInstance()
        '            'Dim formatter As AsYouTypeFormatter = phoneUtil.GetAsYouTypeFormatter("US")
        '            'Dim x As String
        '            ''x = formatter.InputDigit("1"c)
        '            'x = formatter.InputDigit("8"c)
        '            'x = formatter.InputDigit("0"c)
        '            'x = formatter.InputDigit("5"c)
        '            'x = formatter.InputDigit("5"c)
        '            'x = formatter.InputDigit("5"c)
        '            'x = formatter.InputDigit("5"c)
        '            'x = formatter.InputDigit("1"c)
        '            'x = formatter.InputDigit("2"c)
        '            'x = formatter.InputDigit("3"c)
        '            'x = formatter.InputDigit("4"c)

        '            Dim swissNumberStr As String = "+33603363212"
        '            Dim phoneUtil As PhoneNumberUtil = PhoneNumberUtil.GetInstance()
        '            'Dim swissNumberProto As PhoneNumber = phoneUtil.Parse(swissNumberStr, RegionCode.DE)
        '            Dim swissNumberProto As PhoneNumber = phoneUtil.Parse(swissNumberStr, "FR")
        '            Try
        '                Console.WriteLine(swissNumberProto.CountryCode)
        '            Catch e As NumberParseException
        '                Console.WriteLine("NumberParseException was thrown: " + e.ToString())
        '            End Try

        '            Dim geocoder As PhoneNumberOfflineGeocoder = PhoneNumberOfflineGeocoder.GetInstance()
        '            ' Outputs "Zurich"
        '            MessageBox.Show(geocoder.GetDescriptionForNumber(swissNumberProto, Locale.ENGLISH))
        '            ' Outputs "Zürich"
        '            MessageBox.Show(geocoder.GetDescriptionForNumber(swissNumberProto, Locale.GERMAN))
        '            ' Outputs "Zurigo"
        '            MessageBox.Show(geocoder.GetDescriptionForNumber(swissNumberProto, Locale.ITALIAN))
        '        Catch ex As Exception
        '            MessageBox.Show(ex.Message)
        '        End Try
        '        ProcessCommand = 2
        '    End If

        'mobilephone_favorite_add;n;number
        If LCase(Left$(CMD, 25)) = "mobilephone_favorite_add;" Then
            Dim favsplit() As String = CMD.Split(";"), numberList As String = ""
            numberList = favsplit(2)
            For n As Integer = 0 To PhoneBookEntryCount - 1
                If PhoneBookEntries(n).EntryPhoneNumbers(0).EntryPhoneNumber = numberList Then
                    If PhoneBookEntries(n).EntryPhoneNumberCount > 1 Then
                        For o As Integer = 1 To PhoneBookEntries(n).EntryPhoneNumberCount - 1
                            numberList &= "," & PhoneBookEntries(n).EntryPhoneNumbers(o).EntryPhoneNumber
                            favoriteVcardID = n
                        Next
                    End If
                End If
            Next

            'MessageBox.Show("number = " & favsplit(2))
            SDK.Execute("SAVETOSKIN;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "NUMBER;" & numberList & _
                        "||SETVAR;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "NUMBER;" & numberList) 'favsplit(2)= number
            If File.Exists(MainPath & "PhoneBook\pb.vcf") = True Then 'AndAlso BlueSoleil_PBAP_NumberExistAsVcard(favsplit(2)) = True Then
                SDK.Execute("SAVETOSKIN;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "NAME;" & BlueSoleil_PBAP_GetNameFromNumber(favsplit(2)) & _
                            "||SETVAR;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "NAME;" & BlueSoleil_PBAP_GetNameFromNumber(favsplit(2)))
                SDK.Execute("SAVETOSKIN;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "IMG;" & MainPath & "Photo\" & favsplit(2) & ".jpg" &
                            "||SETVAR;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "IMG;" & MainPath & "Photo\" & favsplit(2) & ".jpg" & _
                            "||RELOADSCREEN")
            End If
            ProcessCommand = 2
        End If
        'mobilephone_favorite_del;n;number
        If LCase(Left$(CMD, 25)) = "mobilephone_favorite_del;" Then
            Dim favsplit() As String = CMD.Split(";")
            SDK.Execute("SAVETOSKIN;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "NUMBER;||SETVAR;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "NUMBER;")
            SDK.Execute("SAVETOSKIN;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "NAME;||SETVAR;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "NAME;")
            SDK.Execute("SAVETOSKIN;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "IMG;||SETVAR;MOBILEPHONE_FAVORITE" & UCase(favsplit(1)) & "IMG;||RELOADSCREEN")
            ProcessCommand = 2
        End If

        'mobilephone_rrlog;text
        If LCase(Left$(CMD, 18)) = "mobilephone_rrlog;" Then
            SDK.RRlog(Mid(CMD, 19))
            ProcessCommand = 2
        End If
        'setsource;avrcp
        If LCase(Left$(CMD, 27)) = "mobilephone_setsource;avrcp" Then
            SDK.Execute("setsource;none||load;mobilephone_player.skin||ESC")
            If avrcpIsActive = True Then
                btnMediaPlay()
            Else
                btnMediaConnect()
                Thread.Sleep(1000)
                btnMediaPlay()
            End If
            ProcessCommand = 2
        End If
        'setsource;none
        If LCase(Left$(CMD, 26)) = "mobilephone_setsource;none" Then
            If avrcpIsActive = True Then
                btnMediaStop()
            Else
                SDK.Execute("setsource;none")
            End If
            ProcessCommand = 2
        End If

        'for check callername only
        If LCase(Left(CMD, 29)) = "mobilephone_checkcontactname;" Then
            hfpStatusStr = "Ringing"
            hfpCallerIDno = Mid(CMD, 30)

            hfpCallerIDnoFormatted = New PhoneNumberFormatter(TempPluginSettings.PhoneCountryCodes(0), TempPluginSettings.PhoneCountryCodes(1), TempPluginSettings.PhoneCountryCodes(2))
            hfpCallerIDno = hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(hfpCallerIDno, OutputFormats.National)
            'MessageBox.Show("search number: " & hfpCallerIDno)

            Try
                'OldPlayerStatus = SDK.GetInfo("STATUS")
                OldMainVolume = SDK.GetInfo("VOLUME").Replace("%", "")

                'load contact from the pb.vcf file
                Bluesoleil_LoadContactsFile()
                Thread.Sleep(500)

                'if the contact id exist into the black list, the plugin auto hungup
                If TempPluginSettings.AutoNotAnswerCallIn = True Then
                    If PluginRunForDS = False Then
                        If CheckContactInBlackList(hfpCallerIDno) = True Then
                            SDK.Execute("MOBILEPHONE_HANGUP||*ONMOBILEPHONEAUTOHANGUP||SETVARFROMVAR;POPUP;l_set_BTM_IsInBlackList||MENU;POPUP.SKIN", True) 'reject automatically the caller
                        End If
                        ToLog("RR return the number " & hfpCallerIDno & " is in blacklist ????")
                        Exit Function
                    Else 'erreur avec iCarDS et CheckContactInBlackList ????
                        If CheckContactInBlackList(hfpCallerIDno) = True Then
                            SDK.Execute("MOBILEPHONE_HANGUP||ONMOBILEPHONEAUTOHANGUP||SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_IsInBlackList||POPUP;ReadingPhoneBook.SKIN;5", True) 'reject automatically the caller
                        End If
                        ToLog("iCarDS return the number " & hfpCallerIDno & " is in blacklist ????")
                        Exit Function
                    End If
                End If

                If PluginRunForDS = False Then
                    ToLog("RR return number " & hfpCallerIDno & " isn't in blacklist")
                Else
                    ToLog("iCarDS return number " & hfpCallerIDno & " isn't in blacklist")
                End If

                SDK.Execute("SETVAR;NEWCONTACT;" & hfpCallerIDname, True)

                If File.Exists(MainPath & "PhoneBook\pb.vcf") = True Then
                    hfpCallerIDname = BlueSoleil_PBAP_GetNameFromNumber(hfpCallerIDno)

                    Dim hfpCallerIDImage As String = "" 'pour laisser le temps à la récupération du nom de l'image
                    hfpCallerIDImage = MainPath & "Photo\" & hfpCallerIDno & ".jpg" 'BlueSoleil_BS_PBAP_GetImageFromNumber(hfpCallerIDno)

                    'MessageBox.Show(hfpCallerIDImage)
                    'Dim newGetVcardId As Integer = BlueSoleil_PBAP_GetVcardId(hfpCallerIDno)
                    If File.Exists(hfpCallerIDImage) Then
                        If PluginRunForDS = False Then
                            SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & hfpCallerIDImage & "||WAIT;0.5||MENU;MOBILECALL.skin||*ONMOBILEPHONERINGING||PAUSE||RELOADSCREEN", True)
                        Else
                            SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & hfpCallerIDImage & "||WAIT;0.5||MENU;MOBILECALL.skin||ONMOBILEPHONERINGING||PAUSE||RELOADSCREEN", True)
                        End If
                        ToLog("pb.vcf file exist and picture found for the number " & hfpCallerIDno)
                    Else
                        SDK.Execute("*ONMOBILEPHONERINGING||PAUSE||MENU;MOBILECALL.skin", True)
                    End If

                Else
                    hfpCallerIDname = SDK.GetInfo("=$l_set_BTM_Unknown$") '"Unknow"
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\unknow.gif||WAIT;0.5||MENU;MOBILECALL.skin||*ONMOBILEPHONERINGING||PAUSE||RELOADSCREEN", True)
                    Else
                        SDK.Execute("SETVAR;MOBILEPHONE_NEWPHOTOPATH;" & MainPath & "Photo\unknow.gif||WAIT;0.5||MENU;MOBILECALL.skin||ONMOBILEPHONERINGING||PAUSE||RELOADSCREEN", True)
                    End If
                    If File.Exists(MainPath & "PhoneBook\pb.vcf") = True Then ToLog("pb.vcf file not exist")
                    If BlueSoleil_PBAP_NumberExistAsVcard(hfpCallerIDno) = True Then ToLog("no vcard for this number " & hfpCallerIDno)
                End If

                'launch the ringin loop
                timer3.Enabled = True

                ToLog("bsHandler_HFP_Ringing : hfpCallerIDno = " & hfpCallerIDno & " and hfpCallerIDname = " & hfpCallerIDname)

            Catch ex As Exception
                'MessageBox.Show(ex.Message, "Error bsHandler_HFP_Ringing")
                ToLog("Error in bsHandler_HFP_Ringing")
            End Try
            ProcessCommand = 2
        End If
        'for check callername only


        'mobilephone_sendsms;0102030405;text
        If LCase(Left$(CMD, 20)) = "mobilephone_sendsms;" Then
            Dim cmdDial() As String = Mid$(CMD, 21).Split(";")
            If cmdDial(0).StartsWith("$") And cmdDial(0).EndsWith("$") Then cmdDial(0) = SDK.GetInfo("=" & cmdDial(0))
            If cmdDial(1).StartsWith("$") And cmdDial(1).EndsWith("$") Then cmdDial(1) = SDK.GetInfo("=" & cmdDial(1))
            btnSendText(cmdDial(0), cmdDial(1))
            ToLog("sms send to number '" & cmdDial(0) & "' is : " & cmdDial(1))
            ProcessCommand = 2
        End If

        'mobilephone_dial;112
        If LCase(Left(CMD, 17)) = "mobilephone_dial;" Then
            Dim cmdDial() As String = CMD.Split(";") ', numbersList() As String
            'MessageBox.Show(cmdDial.Length)
            If UCase(cmdDial(1)) = "EMERGENCY" Then
                dialNumber = TempPluginSettings.EmergencyNumber
                ToLog("'" & cmdDial(1) & " is replaced by '" & dialNumber & "'")
                SDK.Execute("MOBILEPHONE_DIAL", True)
            ElseIf UCase(cmdDial(1)) = "MSGVOCAL" Then
                dialNumber = TempPluginSettings.VocalMessageryNumber
                ToLog("'" & cmdDial(1) & " is replaced by '" & dialNumber & "'")
                'ElseIf Left(cmdDial(1), 1) = "$" AndAlso Right(cmdDial(1), 1) = "$" Then
            ElseIf cmdDial(1) = "" Then 'lorsque var est vide

                If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_favorite.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_favorite" Then
                    SDK.Execute("SETVAR;l_ReadingPhoneBook;!!! This FAVORITE is EMPTY !!!||POPUP;ReadingPhoneBook.SKIN;2||WAIT;5||LOAD;MOBILEPHONE_FAVORITE.skin", True)
                    ToLog("favorite variable is empty !!!!")
                ElseIf LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_contacts.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone" Then
                    SDK.Execute("SETVAR;l_ReadingPhoneBook;!!! This Selection is EMPTY !!!||POPUP;ReadingPhoneBook.SKIN;2", True)
                    ToLog("selection variable is empty !!!!")
                End If
            Else
                If cmdDial(1).Contains(",", StringComparison.OrdinalIgnoreCase) = True Then
                    'numbersList = cmdDial(1).Split(",")
                    dialnumberFromFavorite = cmdDial(1)
                    'MessageBox.Show(dialNumber)
                    ToLog("Several numbers exist for this contact !!!")
                    SDK.Execute("MOBILEPHONE_DIALORSELECT", True)
                Else
                    dialNumber = cmdDial(1)
                    ToLog("'" & dialNumber & "' is called !")
                    SDK.Execute("MOBILEPHONE_DIAL", True)
                End If
            End If
            ProcessCommand = 2
        End If
        'mobilephone_dialbyname;pierre
        If LCase(Left$(CMD, 23)) = "mobilephone_dialbyname;" Then
            dialNumber = BlueSoleil_PBAP_GetNumberFromName(Mid$(CMD, 24))
            ToLog("name '" & Mid$(CMD, 18) & "' is replaced by '" & dialNumber & "'")
            SDK.Execute("MOBILEPHONE_DIAL")
            ProcessCommand = 2
        End If

        'mobilephone_phonebookload;sp
        If LCase(Left$(CMD, 26)) = "mobilephone_phonebookload;" Then
            PC_LoadPhoneBook(UCase(Mid$(CMD, 27)))
            ProcessCommand = 2
        End If
        'mobilephone_phonebookget;pb
        If LCase(Left$(CMD, 25)) = "mobilephone_phonebookget;" Then
            btnGetPhonebook(UCase(Mid$(CMD, 26)))
            ProcessCommand = 2
        End If

        'mobilephone_addpicturetoitem;+33557590878.gif USED by SlideShow.exe for select a picture.
        If LCase(Left$(CMD, 29)) = "mobilephone_addpicturetoitem;" Then
            Dim PictureContact As String = Mid$(CMD, 30)
            SDK.Execute("SETVAR;MOBILEPHONE_CARDIMAGE;" & MainPath & "Photo\" & PictureContact)
            ToLog("The picture's contact " & "'" & PictureContact & "'" & " is added")
            ProcessCommand = 2
        End If
        'mobilephone_additem;MYLIST,0102030405,name
        If LCase(Left$(CMD, 20)) = "mobilephone_additem;" Then
            Dim StringItem As String = Mid$(CMD, 21), splitStringItem() As String = StringItem.Split(",")
            AddCustomList(splitStringItem(0), splitStringItem(1), splitStringItem(2), MainPath & "Photo\" & splitStringItem(1) & ".jpg")
            ToLog("The picture's contact " & "'" & splitStringItem(2) & "'" & " is added into the list " & splitStringItem(0))
            ProcessCommand = 2
        End If

        'mobilephone_atcmd;at+cgmm
        If LCase(Left$(CMD, 18)) = "mobilephone_atcmd;" Then
            If LCase(Mid$(CMD, 19)) = "ctrlz" Then
                ATExecute(Chr(26) & vbCrLf, 5000)
                If debugFrm.Visible Then debugFrm.debugTextBox.Focus()
                SendKeys.Send("^z")
            Else
                ATExecute(Mid$(CMD, 19) & vbCrLf, 5000)
                If debugFrm.Visible Then debugFrm.debugTextBox.AppendText(Mid$(CMD, 19) & vbCrLf)
            End If
            ProcessCommand = 2
        End If

        'mobilephone_microvolume;15
        If LCase(Left$(CMD, 24)) = "mobilephone_microvolume;" Then
            BlueSoleil_HFP_SetMicVol(hfpHandleConnHFAG, Mid$(CMD, 25))
            ProcessCommand = 2
        End If
        'mobilephone_speakervolume;15
        If LCase(Left$(CMD, 26)) = "mobilephone_speakervolume;" Then
            BlueSoleil_HFP_SetSpeakerVol(hfpHandleConnHFAG, Mid$(CMD, 27))
            ProcessCommand = 2
        End If

        '## Phone Manager by pierrotm777 ###################################################

        'iCarDS only
        If PluginRunForDS = True Then
            Select Case LCase(CMD)
                Case "mobilephone_settings_phone2"
                    If PhoneCheckedIs = 1 Then
                        PhoneCheckedIs = 2
                        SDK.Execute("SKIPTO;" & PluginSettings.PhoneDeviceName2)
                    ElseIf PhoneCheckedIs = 2 Then
                        PhoneCheckedIs = 1
                        SDK.Execute("SKIPTO;" & PluginSettings.PhoneDeviceName)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_speechrecognition" 'load or stop speech recognition
                    If TempPluginSettings.PhoneSpeechRecognition = True Then
                        If SpeechRecognitionIsActive = False Then
                            InitSpeechReco()
                        ElseIf SpeechRecognitionIsActive = True Then
                            StopSpeechReco()
                        End If
                    Else
                        If PluginRunForDS = False Then
                            SDK.ErrScrn("MobilePhone Error", "Speech Recognition is OFF!!!", "Set 'PhoneSpeechRecognition' to 'True'", 5)
                        Else
                            SDK.Execute("SETVAR;l_ReadingPhoneBook;Speech Recognition is OFF !!!, Set 'PhoneSpeechRecognition' to 'True'||POPUP;ReadingPhoneBook.SKIN;5", True)
                        End If
                    End If
                    ProcessCommand = 2
                    'SETTINGS SCREEN 2
                Case "mobilephone_runonstart"
                    TempPluginSettings.RunOnStart = Not TempPluginSettings.RunOnStart
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_phonebookupdate"
                    TempPluginSettings.PhoneBookAutoUpdate = Not TempPluginSettings.PhoneBookAutoUpdate
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_vcardsupplement"
                    TempPluginSettings.PhoneBookUseVcardSupplement = Not TempPluginSettings.PhoneBookUseVcardSupplement
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_autoswapphone"
                    TempPluginSettings.AutoSwapPhone = Not TempPluginSettings.AutoSwapPhone
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                    'SDK.ErrScrn("MobilePhone Info", "This feature isn't active !!! Sorry", "*********************************************", 5)

                Case "mobilephone_speech_recognition"
                    TempPluginSettings.PhoneSpeechRecognition = Not TempPluginSettings.PhoneSpeechRecognition
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2

                Case "mobilephone_debuglog"
                    TempPluginSettings.PhoneDebugLog = Not TempPluginSettings.PhoneDebugLog
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_lockinmotion"
                    TempPluginSettings.LockInMotion = Not TempPluginSettings.LockInMotion
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_lockinmotioncmd"
                    TempPluginSettings.LockInMotionForCMD = UCase(SDK.GetInfo("=$MOBILEPHONE_LOCKINMOTIONCMD$"))
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_phoneexecatcmd"
                    TempPluginSettings.PhoneExecATCmd = Not TempPluginSettings.PhoneExecATCmd
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_autonotanswercallin"
                    TempPluginSettings.AutoNotAnswerCallIn = Not TempPluginSettings.AutoNotAnswerCallIn
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_panautoconnect"
                    TempPluginSettings.PhonePANAutoRun = Not TempPluginSettings.PhonePANAutoRun
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_noaddcontact"
                    TempPluginSettings.PhoneNoAddContact = Not TempPluginSettings.PhoneNoAddContact
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                Case "mobilephone_nosmspopup"
                    TempPluginSettings.PhoneNoSMSPopupInfo = Not TempPluginSettings.PhoneNoSMSPopupInfo
                    If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                        cMySettings.Copy(TempPluginSettings, PluginSettings)
                        cMySettings.SerializeToXML(PluginSettings)
                    End If
                    ProcessCommand = 2
                    'SETTINGS SCREEN 2

                Case "mobilephone_msgunreadonly" 'replace indicatorclick that don't run with icards
                    SmsUnreadOnly = Not SmsUnreadOnly
                    If SmsUnreadOnly = False Then
                        SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_CheckUnreadSMSInPause||POPUP;ReadingPhoneBook.SKIN;5", True)
                    End If

                Case "mobilephone_avrrepeat"
                    AVRCP_repeat = Not AVRCP_repeat
                    btnMediaSetPlayerSetting(AVRCP_repeat, AVRCP_shuffle)
                    ProcessCommand = 2
                Case "mobilephone_avrshuffle"
                    AVRCP_shuffle = Not AVRCP_shuffle
                    btnMediaSetPlayerSetting(AVRCP_repeat, AVRCP_shuffle)
                    ProcessCommand = 2
                Case "mobilephone_avrmute"
                    AVRCP_mute = Not AVRCP_mute
                    btnMediaMute()
                    ProcessCommand = 2

                Case "mobilephone_isbirthday"
                    If cardBirthday.Count > 0 Then
                        Dim cardSplit() As String = Nothing
                        Dim j As Integer = 0
                        If PluginRunForDS = False Then
                            SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_BTM_BirthdayIsFound||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL", True)
                        Else
                            SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_BirthdayIsFound||POPUP;ReadingPhoneBook.SKIN;5", True)
                        End If
                        For j = 0 To cardBirthday.Count - 1
                            cardSplit = cardBirthday.Item(j).Split("|")
                            If PluginRunForDS = False Then
                                SDK.Execute("CLADD; '" & cardSplit(0) & "' (" & cardSplit(1) & ") '" & cardSplit(2) & "' " & SDK.GetInfo("=$l_set_BTM_BirthdayYear$") & "!||CLSETIMG;" & j + 1 & ";" & SkinPath & "Indicators\BIRTHDAY_01.png", True)
                            Else
                                SDK.Execute("CLADD; '" & cardSplit(0) & "' (" & cardSplit(1) & ") '" & cardSplit(2) & "' " & SDK.GetInfo("=$l_set_BTM_BirthdayYear$") & "!", True)
                            End If

                        Next
                    Else
                        If PluginRunForDS = False Then
                            SDK.Execute("SETVARFROMVAR;MOBILEPHONE_INFO;l_set_BTM_BirthdayNotFound||MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL", True)
                        Else
                            SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_BirthdayNotFound||POPUP;ReadingPhoneBook.SKIN;5", True)
                        End If

                    End If
                    ProcessCommand = 2

            End Select
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
            Case "mobilephone_language"
                ReturnLabel = TempPluginSettings.Language
            Case "mobilephone_pluginver"
                ReturnLabel = Assembly.GetExecutingAssembly().GetName().Version.ToString()
            Case "mobilephone_sdkversion"
                ReturnLabel = GetVersionSdkDll()

            Case "mobilephone_dialbox"
                ReturnLabel = dialNumber
            Case "mobilephone_dialboxlenght"
                ReturnLabel = dialNumber.Length.ToString

            Case "mobilephone_manufacturer"
                ReturnLabel = hfpManufacturerNameParsed 'trim is needed (for remove extra hidden characters)
            Case "mobilephone_model"
                ReturnLabel = hfpModelName
            Case "mobilephone_networkname"
                ReturnLabel = hfpNetworkName
            Case "mobilephone_ownnumber"
                ReturnLabel = hfpSubscriberNo
            Case "mobilephone_ownname"
                ReturnLabel = hfpSubscriberName
            Case "mobilephone_callerid"
                ReturnLabel = hfpCallerIDno
            Case "mobilephone_callername"
                ReturnLabel = hfpCallerIDname

            Case "mobilephone_phonedescription"
                ReturnLabel = IIf(PluginRunForDS = True, SDK.GetInfo("CLDESC"), "")
            Case "mobilephone_phonedevicename"
                ReturnLabel = IIf(hfpModelName <> "-", IIf(PhoneCheckedIs = 1, TempPluginSettings.PhoneDeviceName, TempPluginSettings.PhoneDeviceName2), "-")
            Case "mobilephone_phonedevicetype"
                ReturnLabel = hfpdevicetype
            Case "mobilephone_phonedevicemacaddress"
                ReturnLabel = hfpdevicemacaddress

            Case "mobilephone_networkavail"
                ReturnLabel = hfpNetworkAvailable.ToString
            Case "mobilephone_connected", "mobilephone_phoneconnection"
                ReturnLabel = hfpIsActive.ToString

            Case "mobilephone_hfpstatusstr"
                ReturnLabel = hfpStatusStr
            Case "mobilephone_phonestatus"
                ReturnLabel = PhoneStatus
            Case "mobilephone_oldplayerstatus"
                ReturnLabel = OldPlayerStatus

            Case "mobilephone_lockinmotioncmd"
                ReturnLabel = TempPluginSettings.LockInMotionForCMD

            Case "mobilephone_isroaming"
                ReturnLabel = IIf(hfpIsRoaming = True, "R", "")

            Case "mobilephone_timerinit"
                ReturnLabel = hfpTimerInit.ToString & " s"

                'Case "mobilephone_operatornames"
                '    ReturnLabel = OperatorNames
            Case "mobilephone_imei"
                ReturnLabel = ImeiNumber
            Case "mobilephone_imsi"
                ReturnLabel = IsmiNumber
                'Case "mobilephone_revision"
                '    ReturnLabel = RevisionNumber

            Case "mobilephone_info"
                ReturnLabel = SDK.GetInfo("=$MOBILEPHONE_INFO$")
            Case "mobilephone_smsinfo"
                ReturnLabel = SDK.GetInfo("=$MOBILEPHONE_SMSINFO$")
            Case "mobilephone_settingsinfo"
                ReturnLabel = SDK.GetInfo("=$MOBILEPHONE_SETTINGSINFO$")

            Case "mobilephone_communicating"
                ReturnLabel = hfpCallIsActive.ToString

            Case "mobilephone_numberofphonefound"
                ReturnLabel = NumberOfDeviceFound.ToString
            Case "mobilephone_autoswapphone"
                ReturnLabel = TempPluginSettings.AutoSwapPhone.ToString
            Case "mobilephone_phoneselected"
                ReturnLabel = IIf(PhoneCheckedIs = 1, "1", "2")
                'Case "mobilephone_settings_setphone2"
                '    ReturnLabel = SDK.GetInfo("CLDESC" & Phone2.ToString)


            Case "mobilephone_signalstrength"
                ReturnLabel = Format(hfpSignalPct * 20, "#0") & "%" 'Math.Truncate(hfpSignalPct / 31 * 100).ToString 'Math.Truncate(bluetooth.signalStrength * 65535 / 31).ToString()
            Case "mobilephone_batterystrength"
                ReturnLabel = Format(hfpBatteryPct * 20, "#0") & "%" 'Math.Truncate(hfpBatteryPct / 5 * 100).ToString 'Math.Truncate(bluetooth.batteryLevel * 65535 / 5).ToString()

            Case "mobilephone_batterystrengthold"
                ReturnLabel = hfpBatteryPctOld.ToString
            Case "mobilephone_batterycharging"
                ReturnLabel = IIf(ExternalPowerIsConnected = True, "True", "False")
            Case "mobilephone_batteryisfull"
                ReturnLabel = IIf(BatteryFullCharge = True, "True", "False")

            Case "mobilephone_phonetime"
                ReturnLabel = PhoneTime
            Case "mobilephone_phonedate"
                ReturnLabel = PhoneDate

            Case "mobilephone_smsfromnumber"
                ReturnLabel = SDK.GetInfo("=$MOBILEPHONE_SMSFROMNUMBER$")
            Case "mobilephone_smsfromdatetime"
                ReturnLabel = SDK.GetInfo("=$MOBILEPHONE_SMSFROMDATETIME$")

            Case "mobilephone_numberofsmsfound"
                ReturnLabel = MsgNumberOfNewUnread.ToString

            Case "mobilephone_emergencynumber"
                ReturnLabel = TempPluginSettings.EmergencyNumber
            Case "mobilephone_vocalmessagerynumber"
                ReturnLabel = TempPluginSettings.VocalMessageryNumber

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
                ReturnLabel = TempPluginSettings.PhoneCountryCodes(1)

                'Case "mobilephone_osmajor"
                '    ReturnLabel = osVer.Version.Major
                'Case "mobilephone_osminor"
                '    ReturnLabel = osVer.Version.Minor

            Case "mobilephone_blacklistcount"
                ReturnLabel = hfpContactNumberInBlackList.ToString

                'vcard screen
            Case "vcardid"
                ReturnLabel = vcarIdPh.ToString
            Case "mobilephone_phonebooktype"
                ReturnLabel = If(pbapHistoryListIsSimplified = True, SDK.GetInfo("=$l_set_BTM_SimplifiedList$"), SDK.GetInfo("=$l_set_BTM_NotSimplifiedList$"))
            Case "mobilephone_contactsfromphone" 'number of contacts into the PB vcard file
                ReturnLabel = PhoneBookEntryCount.ToString
            Case "mobilephone_cardname" 'contact's full name
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardname$")
            Case "mobilephone_cardnameprefix" 'contact's full name
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardnameprefix$")
            Case "mobilephone_cardnamesuffix" 'contact's full name
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardnamesuffix$")
            Case "mobilephone_cardemail" 'contact's email
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardemail$")

            Case "mobilephone_cardphonenumber" 'contact's phone number
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardphonenumber$")
            Case "mobilephone_cardphonelabel" 'contact's phone type (home, work, cell,fax and other)
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardphonelabel$")


            Case "mobilephone_cardaddrpobox" 'contact's address pobox
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardaddrpobox$")
            Case "mobilephone_cardaddrext" 'contact's address extend
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardaddrext$")
            Case "mobilephone_cardaddrstreet" 'contact's address street
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardaddrstreet$")
            Case "mobilephone_cardaddrcity" 'contact's address city
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardaddrcity$")
            Case "mobilephone_cardaddrstate" 'contact's address state
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardaddrstate$")
            Case "mobilephone_cardaddrpostalcode" 'contact's address postal code
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardaddrpostalcode$")
            Case "mobilephone_cardaddrcountry" 'contact's address country
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardaddrcountry$")


            Case "mobilephone_cardothernumber" 'contact's other phone number and address
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardothernumber$")

            Case "mobilephone_cardphonecount" 'contact's number of phone
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardphonecount$")
            Case "mobilephone_cardimage" 'contact's image
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardimage$")
            Case "mobilephone_cardbirthday" 'contact's birthday
                ReturnLabel = IIf(SDK.GetInfo("=$mobilephone_cardbirthday$") <> "", SDK.GetInfo("=$mobilephone_cardbirthday$") & " (" & SDK.GetInfo("=$mobilephone_cardage$") & ")", "")
                'Case "mobilephone_cardage" 'contact's age (in years) 
                '    ReturnLabel = SDK.GetInfo("=$mobilephone_cardage$")
            Case "mobilephone_cardnote" 'contact's note
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardnote$")
            Case "mobilephone_cardgeoposition" 'contact's geolocalisation
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardgeoposition$")
            Case "mobilephone_cardorganisation"
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardorganisation$")
            Case "mobilephone_cardid"
                ReturnLabel = SDK.GetInfo("=$mobilephone_cardid$")
            Case "mobilephone_isbirthday"
                ReturnLabel = IIf(cardBirthday.Count > 0, "Contact Birthday", "No Birthday")
                'vcard screen


                'messages screen
            Case "mobilephone_msgfullname"
                ReturnLabel = SDK.GetInfo("=$mobilephone_msgfullname$")
            Case "mobilephone_msgphonenumber"
                ReturnLabel = SDK.GetInfo("=$mobilephone_msgphonenumber$")
            Case "mobilephone_msgtext"
                ReturnLabel = SDK.GetInfo("=$mobilephone_msgtext$")
            Case "mobilephone_msghandle"
                ReturnLabel = SDK.GetInfo("=$mobilephone_msghandle$")
            Case "mobilephone_msgreadstate"
                ReturnLabel = SDK.GetInfo("=$mobilephone_msgreadstate$")
            Case "mobilephone_msgnumberofmsg"
                ReturnLabel = SDK.GetInfo("=$mobilephone_msgnumberofmsg$")
            Case "mobilephone_msgdatetime"
                ReturnLabel = SDK.GetInfo("=$mobilephone_msgdatetime$")
            Case "mobilephone_msgid"
                ReturnLabel = SDK.GetInfo("=$mobilephone_msgid$")
            Case "mobilephone_msgunreadonly"
                ReturnLabel = IIf(SmsUnreadOnly = True, SDK.GetInfo("=$l_set_BTM_UnReadOnly$"), SDK.GetInfo("=$l_set_BTM_AllSMS$"))
                'messages screen

            Case "mobilephone_avrcp_album", "tagalbum"
                ReturnLabel = AVRCP_lblAValbum
            Case "mobilephone_avrcp_artist", "tagartist"
                ReturnLabel = AVRCP_lblAVartist
            Case "mobilephone_avrcp_title", "tagtitle", "trackname"
                ReturnLabel = AVRCP_lblAVtitle
            Case "mobilephone_avrcp_trackpos", "currenttracktime"
                ReturnLabel = AVRCP_lblAVtrackpos
            Case "mobilephone_avrcp_tracklen", "tracktime"
                ReturnLabel = AVRCP_lblAVtracklen
            Case "mobilephone_avrcp_status" ', "status"
                ReturnLabel = IIf(AVRCP_status = "play", "PLAY", "PAUSE")

            Case "mobilephone_panipaddress"
                ReturnLabel = panIPaddress
            Case "mobilephone_pansentbytes"
                ReturnLabel = panGetUpDownStatsBytesSent.ToString
            Case "mobilephone_panreceivedbytes"
                ReturnLabel = panGetUpDownStatsBytesReceived.ToString

            Case "mobilephone_sppcomaddress"
                ReturnLabel = sppCOMMportNum.ToString

            Case "mobilephone_atcmdresult"
                ReturnLabel = hfpATCmdResult

            Case "mobilephone_timer1" 'used for check if the timer1 is running
                ReturnLabel = timerBatteryCounter.ToString
                'Case "mobilephone_timer2" 'used for check if the timer2 is running
                '    ReturnLabel = timer2Counter.ToString
            Case "mobilephone_hfpdeviceisactive" 'used for check presence of the device
                ReturnLabel = hfpDeviceIsActive.ToString

            Case "mobilephone_devicelink"
                ReturnLabel = lblDeviceLink
            Case "mobilephone_devicercvd"
                ReturnLabel = lblDeviceRcvd
            Case "mobilephone_devicesent"
                ReturnLabel = lblDeviceSent
            Case "mobilephone_devicerssi"
                ReturnLabel = lblDeviceRSSI
            Case "mobilephone_deviceaddress"
                ReturnLabel = lblDeviceAddress '(idem phone mac address)

            Case "mobilephone_currentcallinfo"
                ReturnLabel = CurrentCallInfo

            Case "mobilephone_checkheading"
                ReturnLabel = Convert.ToString(ChechkGPSHeading())

            Case "mobilephone_phonebookpath"
                ReturnLabel = sPhonebookpath
            Case "mobilephone_photopath"
                ReturnLabel = sPhotoPath

            Case "mobilephone_timeinuse"
                ReturnLabel = startPluginTimeStr


            Case "mobilephone_speakervol"
                ReturnLabel = hfpSpeakerVolumePct.ToString
            Case "mobilephone_microvol"
                ReturnLabel = hfpMicVolumePct.ToString
            Case "mobilephone_avrcpvol"
                ReturnLabel = avrcpAbsVolPct.ToString

            Case "mobilephone_msgvocalnumber"
                ReturnLabel = TempPluginSettings.VocalMessageryNumber

            Case "mobilephone_islanconnected"
                ReturnLabel = Net_IsLANconnected.ToString

            Case "mobilephone_iswificonnected"
                ReturnLabel = Net_IsWiFiConnected.ToString

            Case "mobilephone_isbtpanconnected"
                ReturnLabel = Net_IsBlueToothPANconnected.ToString

            Case "mobilephone_netdescription"
                ReturnLabel = Net_NetworkDescription()
            Case "mobilephone_netlanype"
                ReturnLabel = Net_LanType()

            Case "mobilephone_panisinlist"
                ReturnLabel = BluetoothPANNetworkAdapter.ToString

            Case "mobilephone_oldvolume"
                ReturnLabel = OldMainVolume
        End Select

    End Function
#End Region

#Region "IndicatorsEx"
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
            Case "btbackroundworker_status"
                ReturnIndicatorEx = bgw.IsBusy.ToString 'threadRunBluesoleil.IsAlive.ToString
            Case "gpsvalid2"
                ReturnIndicatorEx = IIf(SDK.GetInfo("GPSSAT") <> "0", "True", "False")

            Case "mobilephone_settings_changed" 'teste si les réglages sont changés par rapport au fichier .xml
                ReturnIndicatorEx = IIf(cMySettings.Compare(PluginSettings, TempPluginSettings) = False, "True", "False")
            Case "mobilephone_networkavail"
                ReturnIndicatorEx = hfpNetworkAvailable.ToString

            Case "mobilephone_connected", "phone_connected_phoco"
                ReturnIndicatorEx = IIf(hfpIsActive = True, "True", "False")
            Case "mobilephone_avrcpisactive"
                ReturnIndicatorEx = IIf(avrcpIsActive = True, "True", "False")
            Case "mobilephone_sppisactive"
                ReturnIndicatorEx = IIf(sppIsActive = True, "True", "False")
            Case "mobilephone_panisactive"
                ReturnIndicatorEx = IIf(panIsActive = True, "True", "False")
            Case "mobilephone_a2dpisactive"
                ReturnIndicatorEx = IIf(a2dpIsActive = True, "True", "False")
            Case "mobilephone_ftpisactive"
                ReturnIndicatorEx = IIf(ftpIsActive = True, "True", "False")

            Case "mobilephone_incall"
                ReturnIndicatorEx = hfpCallIsActive.ToString

            Case "mobilephone_sdkfound", "bt_connected"
                ReturnIndicatorEx = BlueSoleil_IsInstalled().ToString

                'Phone Books
            Case "mobilephone_phonecch"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_CCH.txt") = True, "True", "False")
            Case "mobilephone_phonemch"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_MCH.txt") = True, "True", "False")
            Case "mobilephone_phoneoch"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_OCH.txt") = True, "True", "False")
            Case "mobilephone_phoneich"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_ICH.txt") = True, "True", "False")
            Case "mobilephone_phonepb"
                ReturnIndicatorEx = IIf(File.Exists(MainPath & "PhoneBook\MobilePhone_PB.txt") = True, "True", "False")

            Case "mobilephone_isbirthday"
                ReturnIndicatorEx = IIf(cardBirthday.Count > 0, "True", "False")

            Case "mobilephone_batterystrength"
                'ReturnIndicatorEx = SkinPath & "Sliders\MOBILEPHONE_SLIDER_" & Convert.ToString(Math.Truncate(hfpBatteryPct / 20)) & ".png" 'used by iCarDS
                ReturnIndicatorEx = SkinPath & "Sliders\MOBILEPHONE_SLIDER_" & Convert.ToString(hfpBatteryPct) & ".png" 'used by iCarDS
            Case "mobilephone_signalstrength"
                'ReturnIndicatorEx = SkinPath & "Sliders\MOBILEPHONE_SLIDER_" & Convert.ToString(Math.Truncate(hfpSignalPct / 20)) & ".png" 'used by iCarDS
                ReturnIndicatorEx = SkinPath & "Sliders\MOBILEPHONE_SLIDER_" & Convert.ToString(hfpSignalPct) & ".png" 'used by iCarDS

            Case "mobilephone_isroaming"
                ReturnIndicatorEx = IIf(hfpIsRoaming = True, "True", "False")
            Case "mobilephone_batterycharging"
                ReturnIndicatorEx = IIf(ExternalPowerIsConnected = True, "True", "False")
            Case "mobilephone_batteryisfull"
                ReturnIndicatorEx = IIf(BatteryFullCharge = True, "True", "False")
            Case "mobilephone_incommingcall"
                ReturnIndicatorEx = IncomingCall.ToString
            Case "mobilephone_messagereceived"
                ReturnIndicatorEx = mapMsgReceived.ToString
            Case "mobilephone_messagereceivedtype"
                ReturnIndicatorEx = MsgReceivedType

            Case "mobilephone_synchronizing"
                ReturnIndicatorEx = IIf(PhoneBookSyncInProgress = True, "True", "False")
                '+ NEW Skin Command; "SetIndBlink"
                'usage: SetIndBlink;xxxx;yyyy
                '	where xxxx is indicator name
                '	where yyyy is "true/false" or "1/0" or not specified for toggle
                'ex. "SetIndBlink;MUTE;0"                - removes ":b" from indicator MUTE on current screen.
                'ex. "SetIndBlink;MUTE;true"             - adds ":b" from indicator MUTE on current screen.
                'ex. "SetIndBlink;MUTE"             		- toggles ":b" from indicator MUTE on current screen.

                'Case "mobilephone_phonebooksort"
                '    ReturnIndicatorEx = PhoneBookSortInProgress.ToString

            Case "mobilephone_phonebookpicture"
                'le test IF ajoute flicker sur l'image
                '    ReturnIndicatorEx = IIf((LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_contacts.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_addpicture.skin") AndAlso SDK.GetInfo("CLIMG") <> "", SDK.GetInfo("CLIMG"), MainPath & "Photo\unknow.gif")
                '    'ReturnIndicatorEx = IIf(BlueSoleil_BS_PBAP_GetImageFromNumber(SDK.GetInfo("CLTEXT")) <> "", BlueSoleil_BS_PBAP_GetImageFromNumber(SDK.GetInfo("CLTEXT")), MainPath & "Photo\unknow.gif") 
                If PluginRunForDS = False Then
                    'ReturnIndicatorEx = IIf(BlueSoleil_BS_PBAP_GetImageFromNumber(dialNumber) <> "", BlueSoleil_BS_PBAP_GetImageFromNumber(dialNumber), MainPath & "Photo\unknow.gif")
                    ReturnIndicatorEx = SDK.GetInfo("CLIMG")
                Else
                    'ReturnIndicatorEx = IIf(BlueSoleil_BS_PBAP_GetImageFromNumber(dialNumber) <> "", BlueSoleil_BS_PBAP_GetImageFromNumber(dialNumber), MainPath & "Photo\unknow.gif")
                    ReturnIndicatorEx = MainPath & "Photo\" & dialNumber & ".jpg" 'BlueSoleil_BS_PBAP_GetImageFromNumber(dialNumber)
                End If

            Case "mobilephone_newphotopath"
                ReturnIndicatorEx = SDK.GetInfo("=$MOBILEPHONE_NEWPHOTOPATH$")
            Case "mobilephone_cardimage"
                ReturnIndicatorEx = SDK.GetInfo("=$MOBILEPHONE_CARDIMAGE$")
            Case "mobilephone_msgimage"
                ReturnIndicatorEx = SDK.GetInfo("=$MOBILEPHONE_MSGIMAGE$")

            Case "mobilephone_communicating"
                ReturnIndicatorEx = hfpCallIsActive.ToString

            Case "mobilephone_speechrecognition"
                ReturnIndicatorEx = IIf(SpeechRecognitionIsActive = True, "True", "False")

                'SETTINGS SCREEN 2
            Case "mobilephone_runonstart"
                ReturnIndicatorEx = IIf(TempPluginSettings.RunOnStart = True, "True", "False")
            Case "mobilephone_phonebookupdate"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneBookAutoUpdate = True, "True", "False")
            Case "mobilephone_vcardsupplement"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneBookUseVcardSupplement = True, "True", "False")
            Case "mobilephone_autoswapphone"
                ReturnIndicatorEx = IIf(TempPluginSettings.AutoSwapPhone = True, "True", "False")
            Case "mobilephone_speech_recognition"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneSpeechRecognition = True, "True", "False")
            Case "mobilephone_debuglog"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneDebugLog = True, "True", "False")
            Case "mobilephone_lockinmotion"
                ReturnIndicatorEx = IIf(TempPluginSettings.LockInMotion = True, "True", "False")
            Case "mobilephone_phoneexecatcmd"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneExecATCmd = True, "True", "False")
            Case "mobilephone_autonotanswercallin"
                ReturnIndicatorEx = IIf(TempPluginSettings.AutoNotAnswerCallIn = True, "True", "False")
            Case "mobilephone_panautoconnect"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhonePANAutoRun = True, "True", "False")
            Case "mobilephone_noaddcontact"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneNoAddContact = True, "True", "False")
            Case "mobilephone_nosmspopup"
                ReturnIndicatorEx = IIf(TempPluginSettings.PhoneNoSMSPopupInfo = True, "True", "False")

                'IF RRTRANSLATOR is ON
            Case "mobilephone_translatorison"
                ReturnIndicatorEx = IIf(SDK.GetInfo("GTRANSLATOR") = "Is ready", "True", "False")
            Case "mobilephone_translatorfromlang"
                ReturnIndicatorEx = IIf(SDK.GetInfo("GTRANSLATOR") = "Is ready", SDK.GetInd("GTRANSLATOR_ind_from"), MainPath & "Languages\error.gif")
            Case "mobilephone_translatortolang"
                ReturnIndicatorEx = IIf(SDK.GetInfo("GTRANSLATOR") = "Is ready", SDK.GetInd("GTRANSLATOR_ind_to"), MainPath & "Languages\error.gif")
                'IF RRTRANSLATOR is ON
                'SETTINGS SCREEN 2

            Case "mobilephone_oppok"
                ReturnIndicatorEx = IIf(ServiceOPP_Usable = True, "True", "False")
            Case "mobilephone_ftpok"
                ReturnIndicatorEx = IIf(ServiceFTP_Usable = True, "True", "False")
            Case "mobilephone_pbapok"
                ReturnIndicatorEx = IIf(ServicePBAP_Usable = True, "True", "False")
            Case "mobilephone_mapok"
                ReturnIndicatorEx = IIf(ServiceMAP_Usable = True, "True", "False")
            Case "mobilephone_panok"
                ReturnIndicatorEx = IIf(ServicePAN_Usable = True, "True", "False")
            Case "mobilephone_avrcpok"
                ReturnIndicatorEx = IIf(ServiceAVRCP_Usable = True, "True", "False")
            Case "mobilephone_a2dpok"
                ReturnIndicatorEx = IIf(ServiceA2DP_Usable = True, "True", "False")
            Case "mobilephone_sppok"
                ReturnIndicatorEx = IIf(ServiceSPP_Usable = True, "True", "False")

            Case "mobilephone_avrstatus"
                'If PluginRunForDS = False Then
                '    ReturnIndicatorEx = IIf(AVRCP_status = "play", SkinPath & "PLAY.png", SkinPath & "STOP.png")
                'Else
                ReturnIndicatorEx = IIf(avrcpIsPlaying = True, "True", "False")
                'End If

            Case "mobilephone_avrmute"
                ReturnIndicatorEx = IIf(AVRCP_mute = True, "True", "False")
            Case "mobilephone_avrrepeat"
                ReturnIndicatorEx = IIf(AVRCP_repeat = True, "True", "False")
            Case "mobilephone_avrshuffle"
                ReturnIndicatorEx = IIf(AVRCP_shuffle = True, "True", "False")
            Case "mobilephone_avrbrowsingok"
                ReturnIndicatorEx = IIf(avrcpBrowsingOk = True, "True", "False")

            Case "mobilephone_timerrequestinfos"
                ReturnIndicatorEx = IIf(timer1.Enabled = True, "True", "False")
            Case "mobilephone_timercheckdevicepresence"
                ReturnIndicatorEx = IIf(timer2.Enabled = True, "True", "False")

            Case "mobilephone_msgunreadonly"
                ReturnIndicatorEx = IIf(SmsUnreadOnly = True, "True", "False")
            Case "mobilephone_msgattachexist"
                ReturnIndicatorEx = IIf(msgAttachExist = True, "True", "False")
            Case "mobilephone_addmsgattachexist"
                ReturnIndicatorEx = IIf(SDK.GetInfo("=$MOBILEPHONE_PATHATTCHFILE$$IMAGEFILENAME$") <> "", "True", "False")

            Case "mobilephone_vcardinlist"
                ReturnIndicatorEx = IIf(vcardInPBList = True, "True", "False")

            Case "mobilephone_favoriteto" '1=sms , 0=call
                If PluginRunForDS = False Then
                    'ReturnIndicatorEx = IIf(SDK.GetInfo("FAVORITETO") = "1", "File;$SKINPATH$Theme\$ThemeIcons$\MenuIcons\sms_skin.png:a", "File;$SKINPATH$Theme\$ThemeIcons$\MenuIcons\ind_PHONE_CONNECTED_phoco_on.png:a")
                    ReturnIndicatorEx = IIf(SDK.GetInfo("FAVORITETO") = "1", "True", "False")
                Else
                    ReturnIndicatorEx = IIf(SDK.GetInfo("=$FAVORITETO$") = "1", favoriteSkinindicatorsPath & "ind_sms_on.png", favoriteSkinindicatorsPath & "ind_PHONE_CONNECTED_phoco_on.png")
                End If

            Case "mobilephone_stopallcheck"
                ReturnIndicatorEx = TempPluginSettings.PhoneNoSMSPopupInfo.ToString
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
                If PhoneCheckedIs = 1 Then
                    PhoneCheckedIs = 2
                    SDK.Execute("SKIPTO;" & PluginSettings.PhoneDeviceName2)
                ElseIf PhoneCheckedIs = 2 Then
                    PhoneCheckedIs = 1
                    SDK.Execute("SKIPTO;" & PluginSettings.PhoneDeviceName)
                End If

            Case "mobilephone_speechrecognition" 'load or stop speech recognition
                If TempPluginSettings.PhoneSpeechRecognition = True Then
                    If SpeechRecognitionIsActive = False Then
                        InitSpeechReco()
                    ElseIf SpeechRecognitionIsActive = True Then
                        StopSpeechReco()
                    End If
                Else
                    If PluginRunForDS = False Then
                        SDK.ErrScrn("MobilePhone Error", "Speech Recognition is OFF!!!", "Set 'PhoneSpeechRecognition' to 'True'", 5)
                    Else
                        SDK.Execute("POPUP;MOBILEPHONE_MESSAGEBOX.SKIN;5||CLCLEAR;ALL||" & _
                                    "SETVAR;MOBILEPHONE_INFO;Speech Recognition is OFF!!!" & "||CLADD;Set 'PhoneSpeechRecognition' to 'True'")
                    End If
                End If

                'SETTINGS SCREEN 2
            Case "mobilephone_runonstart"
                TempPluginSettings.RunOnStart = Not TempPluginSettings.RunOnStart
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_phonebookupdate"
                TempPluginSettings.PhoneBookAutoUpdate = Not TempPluginSettings.PhoneBookAutoUpdate
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_vcardsupplement"
                TempPluginSettings.PhoneBookUseVcardSupplement = Not TempPluginSettings.PhoneBookUseVcardSupplement
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
                'SDK.ErrScrn("MobilePhone Info", "This feature isn't active !!! Sorry", "*********************************************", 5)

            Case "mobilephone_speech_recognition"
                TempPluginSettings.PhoneSpeechRecognition = Not TempPluginSettings.PhoneSpeechRecognition
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If

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
            Case "mobilephone_lockinmotioncmd"
                TempPluginSettings.LockInMotionForCMD = SDK.GetInfo("=$MOBILEPHONE_LOCKINMOTIONCMD$")
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_phoneexecatcmd"
                TempPluginSettings.PhoneExecATCmd = Not TempPluginSettings.PhoneExecATCmd
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_autonotanswercallin"
                TempPluginSettings.AutoNotAnswerCallIn = Not TempPluginSettings.AutoNotAnswerCallIn
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_panautoconnect"
                TempPluginSettings.PhonePANAutoRun = Not TempPluginSettings.PhonePANAutoRun
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_noaddcontact"
                TempPluginSettings.PhoneNoAddContact = Not TempPluginSettings.PhoneNoAddContact
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            Case "mobilephone_nosmspopup"
                TempPluginSettings.PhoneNoSMSPopupInfo = Not TempPluginSettings.PhoneNoSMSPopupInfo
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                'SETTINGS SCREEN 2

            Case "mobilephone_msgunreadonly" 'RR only
                SmsUnreadOnly = Not SmsUnreadOnly
                If SmsUnreadOnly = False Then
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVARFROMVAR;POPUP;l_set_BTM_CheckUnreadSMSInPause||MENU;POPUP.SKIN", True)
                    Else
                        SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_CheckUnreadSMSInPause||POPUP;ReadingPhoneBook.SKIN;5", True)
                    End If
                End If

            Case "mobilephone_avrrepeat"
                AVRCP_repeat = Not AVRCP_repeat
                btnMediaSetPlayerSetting(AVRCP_repeat, AVRCP_shuffle)
            Case "mobilephone_avrshuffle"
                AVRCP_shuffle = Not AVRCP_shuffle
                btnMediaSetPlayerSetting(AVRCP_repeat, AVRCP_shuffle)
            Case "mobilephone_avrmute"
                AVRCP_mute = Not AVRCP_mute
                btnMediaMute()

            Case "mobilephone_isbirthday"
                If cardBirthday.Count > 0 Then
                    Dim cardSplit() As String = Nothing
                    Dim j As Integer = 0
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVARFROMVAR;POPUP;l_set_BTM_BirthdayIsFound||MENU;POPUP.SKIN", True)
                    Else
                        SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_BirthdayIsFound||POPUP;ReadingPhoneBook.SKIN;5", True)
                    End If
                    For j = 0 To cardBirthday.Count - 1
                        cardSplit = cardBirthday.Item(j).Split("|")
                        If PluginRunForDS = False Then
                            SDK.Execute("CLADD; '" & cardSplit(0) & "' (" & cardSplit(1) & ") '" & cardSplit(2) & "' " & SDK.GetInfo("=$l_set_BTM_BirthdayYear$") & "!||CLSETIMG;" & j + 1 & ";" & SkinPath & "Indicators\BIRTHDAY_01.png", True)
                        Else
                            SDK.Execute("CLADD; '" & cardSplit(0) & "' (" & cardSplit(1) & ") '" & cardSplit(2) & "' " & SDK.GetInfo("=$l_set_BTM_BirthdayYear$") & "!", True)
                        End If

                    Next
                Else
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVARFROMVAR;POPUP;l_set_BTM_BirthdayNotFound||MENU;POPUP.SKIN", True)
                    Else
                        SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_BirthdayNotFound||POPUP;ReadingPhoneBook.SKIN;5", True)
                    End If

                End If

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

        Select Case LCase(SLD) 'SDK.GetInfo("SLD;GPSSIGNAL")
            Case "mobilephone_batterystrength"
                ReturnSlider = Math.Truncate(hfpBatteryPct * 65535 / 100 * 20)
            Case "mobilephone_signalstrength"
                ReturnSlider = Math.Truncate(hfpSignalPct * 65535 / 100 * 20)

            Case "mobilephone_speakervol"
                ReturnSlider = Math.Truncate((hfpSpeakerVolumePct / 15) * 65535)
            Case "mobilephone_microvol"
                ReturnSlider = Math.Truncate((hfpMicVolumePct / 15) * 65535)
            Case "mobilephone_avrcpvol"
                ReturnSlider = Math.Truncate((avrcpAbsVolPct / 100) * 65535)

                'Case "songpos"
            Case "mobilephone_avrcp_trackpos"
                ReturnSlider = Math.Truncate((avrcpTrackPos / avrcpTrackLen) * 65535)

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
            Case "mobilephone_speakervol"
                If Value > 65535 Then Value = 65535
                If Value < 0 Then Value = 0
                hfpSpeakerVolumePct = Math.Truncate(Value * 15 / 65535)
                BlueSoleil_HFP_SetSpeakerVol(hfpHandleConnHFAG, hfpSpeakerVolumePct)
            Case "mobilephone_microvol"
                If Value > 65535 Then Value = 65535
                If Value < 0 Then Value = 0
                hfpMicVolumePct = Math.Truncate(Value * 15 / 65535)
                BlueSoleil_HFP_SetMicVol(hfpHandleConnHFAG, hfpMicVolumePct)

            Case "mobilephone_avrcpvol"
                avrcpAbsVolPct = Math.Truncate(Value * 100 / 65535)
                BlueSoleil_AVRCP_SendReq_SetAbsoluteVolumePct(avrcpHandleDvc, avrcpAbsVolPct)
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

#Region "View Entry"
    Private Sub Load_PhoneNumberFromVcard(Id As Integer, Other As Integer)

        SDK.Execute("SETVAR;mobilephone_cardid;" & (Id + 1).ToString & "||" & _
                    "SETVAR;mobilephone_cardphonecount;" & PhoneBookEntries(Id).EntryPhoneNumberCount.ToString)

        Try
            If PhoneBookEntries(Id).EntryPhoneNumberCount > 0 Then

                If File.Exists(MainPath & "Photo\" & PhoneBookEntries(Id).EntryPhoneNumbers(0).EntryPhoneNumber & ".jpg") = True Then
                    SDK.Execute("SETVAR;mobilephone_cardimage;" & MainPath & "Photo\" & PhoneBookEntries(Id).EntryPhoneNumbers(0).EntryPhoneNumber & ".jpg")
                Else
                    If PluginRunForDS = False Then
                        SDK.Execute("SETVAR;mobilephone_cardimage;" & sPhotoPath & "unknow.gif")
                    Else
                        SDK.Execute("SETVAR;mobilephone_cardimage;")
                    End If
                End If

                'If File.Exists(PhoneBookEntries(Id).EntryImage) = True Then
                '    SDK.Execute("SETVAR;mobilephone_cardimage;" & PhoneBookEntries(Id).EntryImage)
                'Else
                '    If PluginRunForDS = False Then
                '        SDK.Execute("SETVAR;mobilephone_cardimage;" & sPhotoPath & "unknow.gif")
                '    Else
                '        SDK.Execute("SETVAR;mobilephone_cardimage;")
                '    End If
                'End If

                SDK.Execute("SETVAR;mobilephone_cardname;" & PhoneBookEntries(Id).EntryName &
                            "||SETVAR;mobilephone_cardnameprefix;" & PhoneBookEntries(Id).EntryNamePrefix &
                            "||SETVAR;mobilephone_cardnamesuffix;" & PhoneBookEntries(Id).EntryNameSuffix &
                            "||SETVAR;mobilephone_cardphonenumber;" & PhoneBookEntries(Id).EntryPhoneNumbers(Other).EntryPhoneNumber &
                            "||SETVAR;mobilephone_cardphonelabel;" & PhoneBookEntries(Id).EntryPhoneNumbers(Other).Location.Replace(";PREF", "") &
                            "||SETVAR;mobilephone_cardfullname;" & PhoneBookEntries(Id).EntryFullName)

                If CheckContactInBlackList(PhoneBookEntries(Id).EntryPhoneNumbers(Other).EntryPhoneNumber) = True Then
                    SDK.Execute("SETVAR;MOBILEPHONE_BLKLINFO;This phone number is in the Black List !!!")
                Else
                    SDK.Execute("SETVAR;MOBILEPHONE_BLKLINFO;-")
                End If

                'check if a contact use more than 1 phone number
                If PhoneBookEntries(Id).EntryPhoneNumberCount > 1 Then
                    SDK.Execute("SETVAR;mobilephone_cardothernumber;+")
                Else
                    SDK.Execute("SETVAR;mobilephone_cardothernumber;||RELOADSCREEN")
                End If

                If PhoneBookEntries(Id).EntryEmail <> "" Then
                    SDK.Execute("SETVAR;mobilephone_cardemail;" & PhoneBookEntries(Id).EntryEmail)
                Else
                    SDK.Execute("SETVAR;mobilephone_cardemail;")
                End If
                If PhoneBookEntries(Id).EntryBirthDay <> "" Then
                    SDK.Execute("SETVAR;mobilephone_cardbirthday;" & PhoneBookEntries(Id).EntryBirthDay & "||" & _
                                "SETVAR;mobilephone_cardage;" & Convert.ToString(GetCurrentAge(PhoneBookEntries(Id).EntryBirthDay)))
                Else
                    SDK.Execute("SETVAR;mobilephone_cardbirthday;||SETVAR;mobilephone_cardage;||RELOADSCREEN")
                End If
                If PhoneBookEntries(Id).EntryNote <> "" Then
                    SDK.Execute("SETVAR;mobilephone_cardnote;" & PhoneBookEntries(Id).EntryNote)
                Else
                    SDK.Execute("SETVAR;mobilephone_cardnote;")
                End If
                If PhoneBookEntries(Id).EntryGeoPosition <> "" Then
                    SDK.Execute("SETVAR;mobilephone_cardgeoposition;" & PhoneBookEntries(Id).EntryGeoPosition)
                Else
                    SDK.Execute("SETVAR;mobilephone_cardgeoposition;")
                End If
                If PhoneBookEntries(Id).EntryOrganisation <> "" Then
                    SDK.Execute("SETVAR;mobilephone_cardorganisation;" & PhoneBookEntries(Id).EntryOrganisation)
                Else
                    SDK.Execute("SETVAR;mobilephone_cardorganisation;")
                End If

                If PluginRunForDS = False Then
                    Select Case UCase(PhoneBookEntries(Id).EntryPhoneNumbers(Other).Location.Replace(";PREF", ""))
                        Case "CELL"
                            SDK.Execute("SETVAR;MOBILEPHONE_CARDPHONETYPE;$SKINPATH$Indicators\phone_mobile.png")
                        Case "HOME"
                            SDK.Execute("SETVAR;MOBILEPHONE_CARDPHONETYPE;$SKINPATH$Indicators\phone_home.png")
                        Case "WORK"
                            SDK.Execute("SETVAR;MOBILEPHONE_CARDPHONETYPE;$SKINPATH$Indicators\phone_work.png")
                        Case "FAX"
                            SDK.Execute("SETVAR;MOBILEPHONE_CARDPHONETYPE;$SKINPATH$Indicators\phone_fax.png")
                        Case Else
                            SDK.Execute("SETVAR;MOBILEPHONE_CARDPHONETYPE;$SKINPATH$Indicators\phone_other.png")
                    End Select
                Else
                    Select Case UCase(PhoneBookEntries(Id).EntryPhoneNumbers(Other).Location.Replace(";PREF", ""))
                        Case "CELL"
                            SDK.Execute("SETVAR;MOBILEPHONE_CARDPHONETYPE;phone_mobile")
                        Case "HOME"
                            SDK.Execute("SETVAR;MOBILEPHONE_CARDPHONETYPE;phone_home")
                        Case "WORK"
                            SDK.Execute("SETVAR;MOBILEPHONE_CARDPHONETYPE;phone_work")
                        Case "FAX"
                            SDK.Execute("SETVAR;MOBILEPHONE_CARDPHONETYPE;phone_fax")
                        Case Else
                            SDK.Execute("SETVAR;MOBILEPHONE_CARDPHONETYPE;phone_other")
                    End Select
                End If


            Else
                'lblName.Text = "Name: -"
                'lblPhone.Text = "Phone: -"
                SDK.Execute("SETVAR;mobilephone_cardname;-" &
                            "||SETVAR;mobilephone_cardphonenumber;-" &
                            "||SETVAR;mobilephone_cardphonelabel;-" &
                            "||SETVAR;mobilephone_cardnameprefix;" &
                            "||SETVAR;mobilephone_cardnamesuffix;" &
                            "||SETVAR;mobilephone_cardfullname;" &
                            "||SETVAR;mobilephone_cardimage;")

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Phone")
        End Try

        SDK.Execute("LOAD;MOBILEPHONE_ENTRY.SKIN")
    End Sub

    Private Sub Load_PhoneAddressFromVcard(Id As Integer, Other As Integer)
        Try
            If PhoneBookEntries(Id).EntryPhoneAddressCount > 0 Then
                If PhoneBookEntries(Id).EntryAddress(Other).EntryFullAddress <> "" Then
                    If PhoneBookEntries(Id).EntryAddress(Other).EntryPOBox <> "" Then
                        SDK.Execute("SETVAR;mobilephone_cardaddrpobox;" & PhoneBookEntries(Id).EntryAddress(Other).EntryPOBox)
                    Else
                        SDK.Execute("SETVAR;mobilephone_cardaddrpobox;")
                    End If
                    If PhoneBookEntries(Id).EntryAddress(Other).EntryExtAddress <> "" Then
                        SDK.Execute("SETVAR;mobilephone_cardaddrext;" & PhoneBookEntries(Id).EntryAddress(Other).EntryExtAddress)
                    Else
                        SDK.Execute("SETVAR;mobilephone_cardaddrext;")
                    End If
                    If PhoneBookEntries(Id).EntryAddress(Other).EntryStreetAddress <> "" Then
                        SDK.Execute("SETVAR;mobilephone_cardaddrstreet;" & PhoneBookEntries(Id).EntryAddress(Other).EntryStreetAddress)
                    Else
                        SDK.Execute("SETVAR;mobilephone_cardaddrstreet;")
                    End If
                    If PhoneBookEntries(Id).EntryAddress(Other).EntryLocalityCity <> "" Then
                        SDK.Execute("SETVAR;mobilephone_cardaddrcity;" & PhoneBookEntries(Id).EntryAddress(Other).EntryLocalityCity)
                    Else
                        SDK.Execute("SETVAR;mobilephone_cardaddrcity;")
                    End If
                    If PhoneBookEntries(Id).EntryAddress(Other).EntryRegionState <> "" Then
                        SDK.Execute("SETVAR;mobilephone_cardaddrstate;" & PhoneBookEntries(Id).EntryAddress(Other).EntryRegionState)
                    Else
                        SDK.Execute("SETVAR;mobilephone_cardaddrstate;")
                    End If
                    If PhoneBookEntries(Id).EntryAddress(Other).EntryPostalCode <> "" Then
                        SDK.Execute("SETVAR;mobilephone_cardaddrpostalcode;" & PhoneBookEntries(Id).EntryAddress(Other).EntryPostalCode)
                    Else
                        SDK.Execute("SETVAR;mobilephone_cardaddrpostalcode;")
                    End If
                    If PhoneBookEntries(Id).EntryAddress(Other).EntryCountry <> "" Then
                        SDK.Execute("SETVAR;mobilephone_cardaddrcountry;" & PhoneBookEntries(Id).EntryAddress(Other).EntryCountry)
                    Else
                        SDK.Execute("SETVAR;mobilephone_cardaddrcountry;")
                    End If

                    If PhoneBookEntries(Id).EntryAddress(Other).EntryFullAddress <> "" Then
                        SDK.Execute("SETVAR;mobilephone_cardaddrfull;" & PhoneBookEntries(Id).EntryAddress(Other).EntryFullAddress.Replace("|", " ").Replace("\,", ", ")) '.Replace("**", " "))
                    Else
                        SDK.Execute("SETVAR;mobilephone_cardaddrfull;")
                    End If

                    If PluginRunForDS = False Then
                        Select Case UCase(PhoneBookEntries(Id).EntryAddress(Other).Location)
                            Case "CELL"
                                SDK.Execute("SETVAR;MOBILEPHONE_CARDADDTYPE;$SKINPATH$Indicators\phone_mobile.png")
                            Case "HOME"
                                SDK.Execute("SETVAR;MOBILEPHONE_CARDADDTYPE;$SKINPATH$Indicators\phone_home.png")
                            Case "WORK"
                                SDK.Execute("SETVAR;MOBILEPHONE_CARDADDTYPE;$SKINPATH$Indicators\phone_work.png")
                            Case "FAX"
                                SDK.Execute("SETVAR;MOBILEPHONE_CARDADDTYPE;$SKINPATH$Indicators\phone_fax.png")
                            Case Else
                                SDK.Execute("SETVAR;MOBILEPHONE_CARDADDTYPE;$SKINPATH$Indicators\phone_other.png")
                        End Select
                    Else
                        Select Case UCase(PhoneBookEntries(Id).EntryAddress(Other).Location)
                            Case "CELL"
                                SDK.Execute("SETVAR;MOBILEPHONE_CARDADDTYPE;phone_mobile")
                            Case "HOME"
                                SDK.Execute("SETVAR;MOBILEPHONE_CARDADDTYPE;phone_home")
                            Case "WORK"
                                SDK.Execute("SETVAR;MOBILEPHONE_CARDADDTYPE;phone_work")
                            Case "FAX"
                                SDK.Execute("SETVAR;MOBILEPHONE_CARDADDTYPE;phone_fax")
                            Case Else
                                SDK.Execute("SETVAR;MOBILEPHONE_CARDADDTYPE;phone_other")
                        End Select
                    End If

                Else

                    SDK.Execute("SETVAR;mobilephone_cardaddrpobox;||SETVAR;mobilephone_cardaddrext;||SETVAR;mobilephone_cardaddrstreet;" & _
                                "||SETVAR;mobilephone_cardaddrcity;||SETVAR;mobilephone_cardaddrstate;||SETVAR;mobilephone_cardaddrpostalcode;" & _
                                "||SETVAR;mobilephone_cardaddrcountry;||SETVAR;mobilephone_cardaddrfull;||SETVAR;mobilephone_cardaddtype;||RELOADSCREEN")
                End If
            Else

                SDK.Execute("SETVAR;mobilephone_cardaddrpobox;||SETVAR;mobilephone_cardaddrext;||SETVAR;mobilephone_cardaddrstreet;" & _
                            "||SETVAR;mobilephone_cardaddrcity;||SETVAR;mobilephone_cardaddrstate;||SETVAR;mobilephone_cardaddrpostalcode;" & _
                            "||SETVAR;mobilephone_cardaddrcountry;||SETVAR;mobilephone_cardaddrfull;||SETVAR;mobilephone_cardaddtype;||RELOADSCREEN")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Address")
        End Try

        SDK.Execute("LOAD;MOBILEPHONE_ENTRY.SKIN")
    End Sub
#End Region

#Region "Manage Custom List with ICO line"
    Private Sub PC_LoadPhoneBook(ByVal type As String)
        If UCase(type) = "PB" Then
            vcardInPBList = True
        Else
            vcardInPBList = False
        End If
        Dim counter = My.Computer.FileSystem.GetFiles(MainPath & "PhoneBook", FileIO.SearchOption.SearchTopLevelOnly, "*.vcf")
        Dim counter2 = My.Computer.FileSystem.GetFiles(MainPath & "PhoneBook", FileIO.SearchOption.SearchTopLevelOnly, "MobilePhone_*.txt")
        'MessageBox.Show("vcf = " & counter2.Count.ToString & vbCrLf & "txt = " & counter.Count.ToString)
        If counter.Count = 0 Or counter2.Count = 0 Then 'counter.Count < 5 Or counter2.Count < 5 Then
            If PluginRunForDS = False Then
                SDK.Execute("SETVARFROMVAR;POPUP;l_set_BTM_ReadingPhoneBook||MENU;POPUP.skin")
            Else
                SDK.Execute("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_ReadingPhoneBook||POPUP;ReadingPhoneBook.skin;5")
            End If
            Dim t As New Threading.Thread(Sub() RRVcardUpdateProcess(True)) 'no event is send to RR after the update
            t.Start()
        End If

        If pbapHistoryListIsSimplified = False Or UCase(type) = "PB" Then
            SDK.Execute("CLCLEAR;ALL||CLLOAD;" & sPhonebookpath & "MobilePhone_" & UCase(type) & ".txt||CLPOS;" & vcarIdPh)
        Else
            SDK.Execute("CLCLEAR;ALL||CLLOAD;" & sPhonebookpath & "MobilePhone_" & UCase(type) & "2.txt||CLPOS;" & vcarIdPh)
        End If


    End Sub

    Private Sub DeleteAllPhoneBook()
        Try
            Dim counter = My.Computer.FileSystem.GetFiles(MainPath & "PhoneBook", FileIO.SearchOption.SearchTopLevelOnly, "*.vcf")
            Dim counter2 = My.Computer.FileSystem.GetFiles(MainPath & "PhoneBook", FileIO.SearchOption.SearchTopLevelOnly, "MobilePhone_*.txt")
            'MessageBox.Show("vcf = " & counter2.Count.ToString & vbCrLf & "txt = " & counter.Count.ToString)
            Dim f As Integer = 0
            For f = 0 To counter.Count - 1
                'MessageBox.Show(counter.Item(f))
                File.Delete(counter.Item(f))
            Next
            For f = 0 To counter2.Count - 1
                'MessageBox.Show(counter.Item(f))
                File.Delete(counter.Item(f))
            Next
            MessageBox.Show("All Phone Book are deleted !", "Phone Book Reset", MessageBoxButtons.OK, MessageBoxIcon.Warning)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub


    ''' <summary>
    ''' Search text in list
    ''' </summary>
    ''' <param name="searchline"></param>
    ''' <remarks></remarks>
    Public Sub SerListInPhoneBook(searchline As String)

        Try
            Dim allLines As String() = IO.File.ReadAllLines(MainPath & "PhoneBook\MobilePhone_PB.txt", Encoding.Default)
            'Dim MaxContactsInList As Integer = Convert.ToInt16(SDK.GetInfo("CLMAX"))
            Dim line As Integer = 1

            Dim comp As StringComparison = StringComparison.OrdinalIgnoreCase 'StringComparison.Ordinal

            For line = 1 To allLines.Length - 1 Step 2
                'spLine = allLines(line).Replace("||", ",").Split(",")
                'PhoneBookArray.Add(spLine(1) & "," & spLine(0) & "," & allLines(line + 1))
                'If allLines(line).Contains(searchline) Then
                '    SDK.Execute("CLPOS;" & (line / 2) & "||ONCLCLICK")
                'End If
                If allLines(line).Contains(searchline, comp) = True Then
                    SDK.Execute("CLPOS;" & Math.Floor(line / 2) & "||CTRLDOWN")
                End If
            Next

        Catch ex As Exception
            MsgBox("Sub PhoneBook Sort - " & ex.Message)
        End Try

    End Sub

    Private Function DeviceNameFromCLDesc(CLDESC As String) As String
        Dim CL() As String
        Dim retCLDESC As String
        CL = Split(CLDESC.Replace(" --> ", ","), ",")
        retCLDESC = CL(0)
        Return retCLDESC
    End Function

    Private Function CheckRRSkinName() As String
        Dim RRSkin() As String = Split(Path.GetDirectoryName(SDK.GetInfo("RRSKIN")), "\")
        'MessageBox.Show(RRSkin.Length)
        CheckRRSkinName = RRSkin(RRSkin.Length - 1)
        Return CheckRRSkinName
    End Function



#End Region

#Region "Speech recognition (number or dictation)"
    Private Sub CheckLanguageSentence()
        Dim PhoneSpeechNumbersByLanguage As String = ""
        PhoneSpeechNumbersByLanguage = SDK.GetInfo("=$l_set_BTM_PhoneSpeechNumbers$")
        If PhoneSpeechNumbersByLanguage <> "" Then
            If TempPluginSettings.PhoneSpeechNumbers <> PhoneSpeechNumbersByLanguage Then
                TempPluginSettings.PhoneSpeechNumbers = PhoneSpeechNumbersByLanguage
                If cMySettings.Compare(PluginSettings, TempPluginSettings) = False Then
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
            End If
        End If
    End Sub
    Private Sub InitSpeechReco()
        'Init Speech Recognition
        ReadLanguageVars()

        'check language sentence
        CheckLanguageSentence()

        Select Case GetOSName()
            Case OsNames.Windows8, OsNames.Windows7, OsNames.WindowsVista, OsNames.Windows10
                Try
                    ' j’active ici mes contrôles pour windows 10, Windows 8 , 8.1 , 7 et Vista (add of Window10 to test)
                    If TempPluginSettings.PhoneSpeechRecognition = True Then
                        If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone" Then
                            speechrecognition = New ClassSpeechRecognition("number")
                            speechrecognition.SpeechProcessing_Load()
                        ElseIf LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smswrite.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smswrite" Then
                            speechrecognition = New ClassSpeechRecognition("dictation")
                            speechrecognition.SpeechDictationProcessing_Load()
                        End If
                    End If
                    CreateSpeechHandlers()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Speech Recognition Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
        RemoveSpeechHandlers()
        If LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone" Then
            speechrecognition = New ClassSpeechRecognition("number")
            speechrecognition.SpeechProcessing_Stop()
        ElseIf LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smswrite.skin" Or LCase(SDK.GetInfo("RRSCREEN")) = "mobilephone_smswrite" Then
            speechrecognition = New ClassSpeechRecognition("dictation")
            speechrecognition.DictationProcessing_Stop()
        End If
    End Sub
#End Region

#Region "Search Devices"
    Private Sub Search_Device() 'Creation d'une list de téléphones appairés
        If File.Exists(MainPath & "MobilePhone_DevicesList.lst") Then File.Delete(MainPath & "MobilePhone_DevicesList.lst")
        'SearchThread = New System.Threading.Thread(AddressOf Search_bluetooth_Device)
        'SearchThread.Priority = Threading.ThreadPriority.Normal
        'SearchThread.Start()
        Dim t As New Threading.Thread(AddressOf Search_bluetooth_Devices)
        t.Start()
    End Sub
    Private Sub Search_bluetooth_Devices() 'bluetooth_Device()
        If hfpIsActive = True Then
            btnHandsFreeDisconnect()
        End If
        If BlueSoleil_IsSDKinitialized() = False Then
            btnInitBlueSoleil()
        End If
        Dim startTime As DateTime = Now
        Do
            Threading.Thread.Sleep(100)
            If BlueSoleil_IsSDKinitialized() = True Then Exit Do
            If Now.Subtract(startTime).TotalSeconds > 5 Then
                Exit Do
            End If
        Loop

        btnRefreshDevices(True)

        SDK.Execute("CLLOAD;" & MainPath & "MobilePhone_DevicesList.lst||CLPOS;1||SETVARFROMVAR;MOBILEPHONE_SETTINGSINFO;l_set_BTM_DeviceListIsUpdated !!!")

    End Sub
#End Region

#Region "Search Services accepted by the phone"
    ''' <summary>
    ''' Extract Services accepted by a phone
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Search_RemoteServices() 'Creation d'une liste de téléphones appairés
        If File.Exists(MainPath & "MobilePhone_RemoteServices.lst") Then File.Delete(MainPath & "MobilePhone_RemoteServices.lst")
        SDK.Execute("SETVAR;MOBILEPHONE_SETTINGSINFO;The Remote Services's List for the phone --> " & TempPluginSettings.PhoneDeviceName & " is updated, please wait !!!")
        Dim t As New Threading.Thread(Sub() GetRemoteServicesList(True))
        t.Start()
    End Sub
    Private Sub Search_LocalServices() 'Creation d'une liste de téléphones appairés
        If File.Exists(MainPath & "MobilePhone_LocalServices.lst") Then File.Delete(MainPath & "MobilePhone_LocalServices.lst")
        SDK.Execute("SETVAR;MOBILEPHONE_SETTINGSINFO;The Local Services's List for the phone --> " & TempPluginSettings.PhoneDeviceName & "  is updated, please wait !!!")
        Dim t As New Threading.Thread(Sub() GetLocalServicesList(True))
        t.Start()
    End Sub
    Private Sub CheckPhone_bluetooth_Services() 'bluetooth_Services()
        Dim t As New Threading.Thread(Sub() GetRemoteServicesList(False))
        t.Start()
        If PluginRunForDS = False Then
            SDK.Execute("MENU;MOBILEPHONE_CHECK.skin")
        Else
            SDK.Execute("POPUP;MOBILEPHONE_CHECK.skin;5")
        End If

    End Sub
#End Region

#Region "List Country Indicatifs"

    ''' <summary>
    ''' Read the dictionnary CountryList
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CountryToPhoneIndicatifList()
        CountryList.Clear()
        CreateCountryList()
        Dim CountryToSelect() As String, test As String = "", c As Integer = 0
        CountryToSelect = MyCultureInfo.Split(" ")
        Dim ff As New List(Of String)
        If PluginRunForDS = False Then
            For Each pair In CountryList
                SDK.Execute("CLADD;" & pair.Key & " -- > " & pair.Value.ToString & "||CLFIND;" & CountryToSelect(1).Replace("(", "").Replace(")", ""))
            Next
        Else
            For Each pair In CountryList
                ff.Add("CLADD;" & pair.Key & " -- > " & pair.Value.ToString & "||CLFIND;" & CountryToSelect(1).Replace("(", "").Replace(")", ""))
            Next
            ff.Reverse()
            For Each pair In ff
                SDK.Execute(pair)
            Next
        End If


        'MessageBox.Show(test)
        CountryList.Clear()
    End Sub
#End Region

#Region "Languages"
    Private Sub ReadLanguageVars()
        Try
            If PluginRunForDS = True Then
                If LCase(Left(TempPluginSettings.Language, 2)) <> LCase(SDK.GetInfo("=$language$")) Then
                    TempPluginSettings.Language = DSToRRLanguage(LCase(SDK.GetInfo("=$language$")))
                    cMySettings.Copy(TempPluginSettings, PluginSettings)
                    cMySettings.SerializeToXML(PluginSettings)
                End If
                LanguageList.Clear()
            End If
            Dim arrayLang() As String, arrayLine() As String
            'MessageBox.Show(MainPath & "Languages\" & TempPluginSettings.Language & "\" & TempPluginSettings.Language & ".txt")
            If File.Exists(MainPath & "Languages\" & TempPluginSettings.Language & "\" & TempPluginSettings.Language & ".txt") = True Then 'ajout test présence du fichier fichier langue défini dans le fichier .ini
                arrayLang = File.ReadAllLines(MainPath & "Languages\" & TempPluginSettings.Language & "\" & TempPluginSettings.Language & ".txt")
                For i As Integer = 0 To arrayLang.Length - 1
                    If arrayLang(i).StartsWith("l_set_BTM_") AndAlso arrayLang(i) <> "" Then
                        arrayLine = Split(arrayLang(i), "=")
                        SDK.Execute("SETVAR;" & arrayLine(0) & ";" & arrayLine(1)) 'lecture des variables de langue
                    End If
                Next
                SDK.Execute("SETVAR;MOBILEPHONE_INFO;***")
            Else
                SDK.Execute("MENU;MOBILEPHONE_MESSAGEBOX.SKIN||CLCLEAR;ALL||SETVAR;MOBILEPHONE_INFO;Language Info||" & _
                            "CLADD;" & TempPluginSettings.Language & ".txt file not exist !!!" & _
                            "CLADD;   " & _
                            "CLADD;You can create your own language file !!!")
            End If
            ToLog("Read Variables for the " & TempPluginSettings.Language & " language is done")
            SDK.RRlog("Read Variables for the " & TempPluginSettings.Language & " language is done")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "Error in Language VAR load")
            ToLog("Read Variables for the " & TempPluginSettings.Language & " return error " & ex.Message)
            SDK.RRlog("Read Variables for the " & TempPluginSettings.Language & " return error " & ex.Message)
        End Try

    End Sub
#End Region

#Region "CheckGPS Gps Status"
    Private Function ChechkGPSHeading() As Boolean
        If PluginRunForDS = False Then
            If UCase(Path.GetFileNameWithoutExtension(SDK.GetInd("RRGPSHeading"))) <> "NA" Then
                Return True
            Else
                Return False
            End If
        Else
            If Convert.ToUInt16(SDK.GetInfo("GPSSPDN")) > 0 Then
                Return True
            Else
                Return False
            End If
        End If

    End Function
#End Region

#Region "Handlers"
    Private Sub CreateSpeechHandlers()
        If handlersSpeechDone = True Then Exit Sub
        handlersSpeechDone = True
        AddHandler speechrecognition.MobilePhoneSeepchRecognitionIsOFF, AddressOf speechUnload
        AddHandler speechrecognition.MobilePhoneSeepchDial, AddressOf speechDial
        AddHandler speechrecognition.MobilePhoneSeepchHangup, AddressOf speechHangup
        AddHandler speechrecognition.MobilePhoneSeepchHelp, AddressOf speechHelp
        AddHandler speechrecognition.MobilePhoneSeepchDialByName, AddressOf speechDialByName
        AddHandler speechrecognition.MobilePhoneSeepchSupp1, AddressOf speechSupp1
        AddHandler speechrecognition.MobilePhoneSeepchSupp2, AddressOf speechSupp2
        AddHandler speechrecognition.MobilePhoneSeepchSupp3, AddressOf speechSupp3
    End Sub
    Private Sub CreateAllHandlers() 'Handles Me.Shown

        If handlersMainDone = True Then Exit Sub
        handlersMainDone = True


        AddHandler SetIndBlinkOn, AddressOf BlinkIndicatorOn
        AddHandler SetIndBlinkOff, AddressOf BlinkIndicatorOff

        AddHandler BlueSoleil_Status_TurnOn, AddressOf bsHandler_Status_TurnOn
        AddHandler BlueSoleil_Status_TurnOff, AddressOf bsHandler_Status_TurnOff
        AddHandler BlueSoleil_Status_Plugged, AddressOf bsHandler_Status_Plugged
        AddHandler BlueSoleil_Status_Unplugged, AddressOf bsHandler_Status_Unplugged
        AddHandler BlueSoleil_Status_DevicePaired, AddressOf bsHandler_Status_DevicePaired
        AddHandler BlueSoleil_Status_DeviceUnpaired, AddressOf bsHandler_Status_DeviceUnpaired
        AddHandler BlueSoleil_Status_DeviceDeleted, AddressOf bsHandler_Status_DeviceDeleted
        AddHandler BlueSoleil_Status_DeviceFound, AddressOf bsHandler_Status_DeviceFound

        AddHandler BlueSoleil_Status_ServiceConnectedInbound, AddressOf bsHandler_Status_ServiceConnectedInbound
        AddHandler BlueSoleil_Status_ServiceDisconnectedInbound, AddressOf bsHandler_Status_ServiceDisconnectedInbound
        AddHandler BlueSoleil_Status_ServiceConnectedOutbound, AddressOf bsHandler_Status_ServiceConnectedOutbound
        AddHandler BlueSoleil_Status_ServiceDisconnectedOutbound, AddressOf bsHandler_Status_ServiceDisconnectedOutbound

        AddHandler BlueSoleil_Event_FTP_FoundFolder, AddressOf bsHandler_FTP_FoundFolder
        AddHandler BlueSoleil_Event_FTP_FoundFile, AddressOf bsHandler_FTP_FoundFile

        AddHandler BlueSoleil_Event_MAP_MsgIsReceived, AddressOf bsHandler_MAP_MsgIsReceived
        'AddHandler BlueSoleil_Event_MAP_MsgNotification, AddressOf bsHandler_MAP_MsgNotification
        AddHandler BlueSoleil_Event_MAP_MsgIsSend, AddressOf bsHandler_MAP_MsgIsSend

        AddHandler BlueSoleil_Event_PAN_IPchanged, AddressOf bsHandler_PAN_IPchange
        AddHandler BlueSoleil_Event_PAN_IsOn, AddressOf bsHandler_PAN_IsOn
        AddHandler BlueSoleil_Event_PAN_IsOff, AddressOf bsHandler_PAN_IsOff

        AddHandler BlueSoleil_Event_WIFI_IsOn, AddressOf bsHandler_WIFI_IsOn
        AddHandler BlueSoleil_Event_WIFI_IsOff, AddressOf bsHandler_WIFI_IsOff
        AddHandler BlueSoleil_Event_LAN_IsOn, AddressOf bsHandler_LAN_IsOn
        AddHandler BlueSoleil_Event_LAN_IsOff, AddressOf bsHandler_LAN_IsOff

        AddHandler BlueSoleil_Event_AVRCP_PlayStatus, AddressOf bsHandler_AVRCP_PlayStatusInfo
        AddHandler BlueSoleil_Event_AVRCP_TrackAlbum, AddressOf bsHandler_AVRCP_TrackAlbum
        AddHandler BlueSoleil_Event_AVRCP_TrackArtist, AddressOf bsHandler_AVRCP_TrackArtist
        AddHandler BlueSoleil_Event_AVRCP_TrackTitle, AddressOf bsHandler_AVRCP_TrackTitle
        AddHandler BlueSoleil_Event_AVRCP_TrackChanged, AddressOf bsHandler_AVRCP_TrackChanged
        AddHandler BlueSoleil_Event_AVRCP_AbsoluteVolume, AddressOf bsHandler_AVRCP_AbsoluteVolume
        AddHandler BlueSoleil_Event_AVRCP_BatteryStatusChanged, AddressOf bsHandler_AVRCP_BatteryStatus

        AddHandler BlueSoleil_Event_HFP_Ringing, AddressOf bsHandler_HFP_Ringing
        AddHandler BlueSoleil_Event_HFP_OngoingCall, AddressOf bsHandler_HFP_OngoingCall
        AddHandler BlueSoleil_Event_HFP_OutgoingCall, AddressOf bsHandler_HFP_OutgoingCall
        AddHandler BlueSoleil_Event_HFP_Missedcall, AddressOf bsHandler_HFP_MissedCall
        AddHandler BlueSoleil_Event_HFP_Standby, AddressOf bsHandler_HFP_StandBy
        'AddHandler BlueSoleil_Event_HFP_SignalQuality, AddressOf bsHandler_HFP_SignalQuality
        'AddHandler BlueSoleil_Event_HFP_BatteryCharge, AddressOf bsHandler_HFP_BatteryCharge
        AddHandler BlueSoleil_Event_HFP_SpeakerVolume, AddressOf bsHandler_HFP_SpeakerVolume
        AddHandler BlueSoleil_Event_HFP_MicVolume, AddressOf bsHandler_HFP_MicVolume
        AddHandler BlueSoleil_Event_HFP_ExtCmdInd, AddressOf bsHandler_HFP_ExtCmdInd

        AddHandler BlueSoleil_Event_HFP_NetworkAvailable, AddressOf bsHandler_HFP_NetworkAvailable
        AddHandler BlueSoleil_Event_HFP_NetworkUnavailable, AddressOf bsHandler_HFP_NetworkUnavailable
        AddHandler BlueSoleil_Event_HFP_NetworkOperatorName, AddressOf bsHandler_HFP_NetworkName
        AddHandler BlueSoleil_Event_HFP_ConnectionReleased, AddressOf bsHandler_HFP_ConnectionReleased
        AddHandler Bluesoleil_Event_HFP_CallerID, AddressOf bsHandler_HFP_CallerID
        AddHandler Bluesoleil_Event_HFP_SubscriberPhoneNo, AddressOf bsHandler_HFP_SubscriberNo
        AddHandler BlueSoleil_Event_HFP_ModelName, AddressOf bsHandler_HFP_ModelName
        AddHandler BlueSoleil_Event_HFP_ManufacturerName, AddressOf bsHandler_HFP_ManufacturerName
        AddHandler BlueSoleil_Event_HFP_CurrentCallInfo, AddressOf bsHandler_HFP_CurrentCallInfo
        AddHandler BlueSoleil_Event_HFP_OtherPhoneConnected, AddressOf bsHandler_HFP_SecondPhoneConnected

        AddHandler BlueSoleil_Event_HFP_StartRoaming, AddressOf bsHandler_HFP_StartRoaming
        AddHandler BlueSoleil_Event_HFP_StopRoaming, AddressOf bsHandler_HFP_EndRoaming

        AddHandler BlueSoleil_Event_HFP_VoiceCmdActivated, AddressOf voiceActivation
        AddHandler BlueSoleil_Event_HFP_VoiceCmdDeactivated, AddressOf voiceDeactivation

        AddHandler BlueSoleil_Event_PBAP_ContactsListUsable, AddressOf bsHandler_PBAP_ContactsListUsable
        AddHandler BlueSoleil_Event_PBAP_IsBirthDay, AddressOf bsHandler_PBAP_IsBirthDay

        AddHandler BlueSoleil_Event_HFP_ExtPowerBattON, AddressOf alarmExternalPowerOn
        AddHandler BlueSoleil_Event_HFP_ExtPowerBattOFF, AddressOf alarmExternalPowerOff
        AddHandler BlueSoleil_Event_HFP_BatteryIsFull, AddressOf battisFullCharge

        AddHandler BlueSoleil_Event_ERROR_Return, AddressOf bsHandler_ERROR_Return
    End Sub



    Private Sub RemoveSpeechHandlers()
        RemoveHandler speechrecognition.MobilePhoneSeepchRecognitionIsOFF, AddressOf speechUnload
        RemoveHandler speechrecognition.MobilePhoneSeepchDial, AddressOf speechDial
        RemoveHandler speechrecognition.MobilePhoneSeepchHangup, AddressOf speechHangup
        RemoveHandler speechrecognition.MobilePhoneSeepchHelp, AddressOf speechHelp
        RemoveHandler speechrecognition.MobilePhoneSeepchDialByName, AddressOf speechDialByName
        RemoveHandler speechrecognition.MobilePhoneSeepchSupp1, AddressOf speechSupp1
        RemoveHandler speechrecognition.MobilePhoneSeepchSupp2, AddressOf speechSupp2
        RemoveHandler speechrecognition.MobilePhoneSeepchSupp3, AddressOf speechSupp3
    End Sub
    Private Sub RemoveAllHandlers()
        If handlersMainDone = False Then Exit Sub
        handlersMainDone = False


        RemoveHandler SetIndBlinkOn, AddressOf BlinkIndicatorOn
        RemoveHandler SetIndBlinkOff, AddressOf BlinkIndicatorOff

        RemoveHandler BlueSoleil_Status_TurnOn, AddressOf bsHandler_Status_TurnOn
        RemoveHandler BlueSoleil_Status_TurnOff, AddressOf bsHandler_Status_TurnOff
        RemoveHandler BlueSoleil_Status_Plugged, AddressOf bsHandler_Status_Plugged
        RemoveHandler BlueSoleil_Status_Unplugged, AddressOf bsHandler_Status_Unplugged
        RemoveHandler BlueSoleil_Status_DevicePaired, AddressOf bsHandler_Status_DevicePaired
        RemoveHandler BlueSoleil_Status_DeviceUnpaired, AddressOf bsHandler_Status_DeviceUnpaired
        RemoveHandler BlueSoleil_Status_DeviceDeleted, AddressOf bsHandler_Status_DeviceDeleted
        RemoveHandler BlueSoleil_Status_DeviceFound, AddressOf bsHandler_Status_DeviceFound

        RemoveHandler BlueSoleil_Status_ServiceConnectedInbound, AddressOf bsHandler_Status_ServiceConnectedInbound
        RemoveHandler BlueSoleil_Status_ServiceDisconnectedInbound, AddressOf bsHandler_Status_ServiceDisconnectedInbound
        RemoveHandler BlueSoleil_Status_ServiceConnectedOutbound, AddressOf bsHandler_Status_ServiceConnectedOutbound
        RemoveHandler BlueSoleil_Status_ServiceDisconnectedOutbound, AddressOf bsHandler_Status_ServiceDisconnectedOutbound

        RemoveHandler BlueSoleil_Event_FTP_FoundFolder, AddressOf bsHandler_FTP_FoundFolder
        RemoveHandler BlueSoleil_Event_FTP_FoundFile, AddressOf bsHandler_FTP_FoundFile

        RemoveHandler BlueSoleil_Event_MAP_MsgIsReceived, AddressOf bsHandler_MAP_MsgIsReceived
        'RemoveHandler BlueSoleil_Event_MAP_MsgNotification, AddressOf bsHandler_MAP_MsgNotification
        RemoveHandler BlueSoleil_Event_MAP_MsgIsSend, AddressOf bsHandler_MAP_MsgIsSend

        RemoveHandler BlueSoleil_Event_PAN_IPchanged, AddressOf bsHandler_PAN_IPchange
        RemoveHandler BlueSoleil_Event_PAN_IsOn, AddressOf bsHandler_PAN_IsOn
        RemoveHandler BlueSoleil_Event_PAN_IsOff, AddressOf bsHandler_PAN_IsOff

        RemoveHandler BlueSoleil_Event_WIFI_IsOn, AddressOf bsHandler_WIFI_IsOn
        RemoveHandler BlueSoleil_Event_WIFI_IsOff, AddressOf bsHandler_WIFI_IsOff
        RemoveHandler BlueSoleil_Event_LAN_IsOn, AddressOf bsHandler_LAN_IsOn
        RemoveHandler BlueSoleil_Event_LAN_IsOff, AddressOf bsHandler_LAN_IsOff

        RemoveHandler BlueSoleil_Event_AVRCP_PlayStatus, AddressOf bsHandler_AVRCP_PlayStatusInfo
        RemoveHandler BlueSoleil_Event_AVRCP_TrackAlbum, AddressOf bsHandler_AVRCP_TrackAlbum
        RemoveHandler BlueSoleil_Event_AVRCP_TrackArtist, AddressOf bsHandler_AVRCP_TrackArtist
        RemoveHandler BlueSoleil_Event_AVRCP_TrackTitle, AddressOf bsHandler_AVRCP_TrackTitle
        RemoveHandler BlueSoleil_Event_AVRCP_TrackChanged, AddressOf bsHandler_AVRCP_TrackChanged
        RemoveHandler BlueSoleil_Event_AVRCP_AbsoluteVolume, AddressOf bsHandler_AVRCP_AbsoluteVolume
        RemoveHandler BlueSoleil_Event_AVRCP_BatteryStatusChanged, AddressOf bsHandler_AVRCP_BatteryStatus

        RemoveHandler BlueSoleil_Event_HFP_Ringing, AddressOf bsHandler_HFP_Ringing
        RemoveHandler BlueSoleil_Event_HFP_OngoingCall, AddressOf bsHandler_HFP_OngoingCall
        RemoveHandler BlueSoleil_Event_HFP_OutgoingCall, AddressOf bsHandler_HFP_OutgoingCall
        AddHandler BlueSoleil_Event_HFP_Missedcall, AddressOf bsHandler_HFP_MissedCall
        RemoveHandler BlueSoleil_Event_HFP_Standby, AddressOf bsHandler_HFP_StandBy
        'RemoveHandler BlueSoleil_Event_HFP_SignalQuality, AddressOf bsHandler_HFP_SignalQuality
        'RemoveHandler BlueSoleil_Event_HFP_BatteryCharge, AddressOf bsHandler_HFP_BatteryCharge
        RemoveHandler BlueSoleil_Event_HFP_SpeakerVolume, AddressOf bsHandler_HFP_SpeakerVolume
        RemoveHandler BlueSoleil_Event_HFP_MicVolume, AddressOf bsHandler_HFP_MicVolume
        RemoveHandler BlueSoleil_Event_HFP_ExtCmdInd, AddressOf bsHandler_HFP_ExtCmdInd

        RemoveHandler BlueSoleil_Event_HFP_NetworkAvailable, AddressOf bsHandler_HFP_NetworkAvailable
        RemoveHandler BlueSoleil_Event_HFP_NetworkUnavailable, AddressOf bsHandler_HFP_NetworkUnavailable
        RemoveHandler BlueSoleil_Event_HFP_NetworkOperatorName, AddressOf bsHandler_HFP_NetworkName
        RemoveHandler BlueSoleil_Event_HFP_ConnectionReleased, AddressOf bsHandler_HFP_ConnectionReleased
        RemoveHandler Bluesoleil_Event_HFP_CallerID, AddressOf bsHandler_HFP_CallerID
        RemoveHandler Bluesoleil_Event_HFP_SubscriberPhoneNo, AddressOf bsHandler_HFP_SubscriberNo
        RemoveHandler BlueSoleil_Event_HFP_ModelName, AddressOf bsHandler_HFP_ModelName
        RemoveHandler BlueSoleil_Event_HFP_ManufacturerName, AddressOf bsHandler_HFP_ManufacturerName
        RemoveHandler BlueSoleil_Event_HFP_CurrentCallInfo, AddressOf bsHandler_HFP_CurrentCallInfo
        RemoveHandler BlueSoleil_Event_HFP_OtherPhoneConnected, AddressOf bsHandler_HFP_SecondPhoneConnected

        RemoveHandler BlueSoleil_Event_HFP_StartRoaming, AddressOf bsHandler_HFP_StartRoaming
        RemoveHandler BlueSoleil_Event_HFP_StopRoaming, AddressOf bsHandler_HFP_EndRoaming
        RemoveHandler BlueSoleil_Event_HFP_VoiceCmdActivated, AddressOf voiceActivation
        RemoveHandler BlueSoleil_Event_HFP_VoiceCmdDeactivated, AddressOf voiceDeactivation

        RemoveHandler BlueSoleil_Event_PBAP_ContactsListUsable, AddressOf bsHandler_PBAP_ContactsListUsable
        RemoveHandler BlueSoleil_Event_PBAP_IsBirthDay, AddressOf bsHandler_PBAP_IsBirthDay

        RemoveHandler BlueSoleil_Event_HFP_ExtPowerBattON, AddressOf alarmExternalPowerOn
        RemoveHandler BlueSoleil_Event_HFP_ExtPowerBattOFF, AddressOf alarmExternalPowerOff
        RemoveHandler BlueSoleil_Event_HFP_BatteryIsFull, AddressOf battisFullCharge

        RemoveHandler BlueSoleil_Event_ERROR_Return, AddressOf bsHandler_ERROR_Return

        'RemoveHandler bgw.DoWork, AddressOf bgw_DoWork
        'RemoveHandler bgw.ProgressChanged, AddressOf bgw_ProgressChanged
        'RemoveHandler bgw.RunWorkerCompleted, AddressOf bgw_RunWorkerCompleted
    End Sub
#End Region


#Region " - Framework - "
    Public Function GetOSArchitecture() As String
        Dim pa As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")
        Return (If(([String].IsNullOrEmpty(pa) OrElse [String].Compare(pa, 0, "x86", 0, 3, True) = 0), "32 Bits", "64 Bits"))
    End Function
    Public Sub GetVersionFromRegistry()
        SDK.Execute("SETVAR;MOBILEPHONE_SETTINGSINFO;Please, wait !!!")

        If File.Exists(MainPath & "SysInfo.lst") Then File.Delete(MainPath & "SysInfo.lst")

        AddCustomList(MainPath & "SysInfo.lst", "Computer", "Computer", "")
        AddCustomList(MainPath & "SysInfo.lst", "    Name = " + My.Computer.Name, "    Name = " + My.Computer.Name, "")
        AddCustomList(MainPath & "SysInfo.lst", "    Is a Laptop = " + cSystemInfo.IsLaptop.ToString, "    Is a Laptop = " + cSystemInfo.IsLaptop.ToString, "")
        AddCustomList(MainPath & "SysInfo.lst", "    UI Culture = " + My.Computer.Info.InstalledUICulture.ToString, "    UI Culture = " + My.Computer.Info.InstalledUICulture.ToString, "")
        AddCustomList(MainPath & "SysInfo.lst", "    OS FullName = " + My.Computer.Info.OSFullName.ToString, "    OS FullName = " + My.Computer.Info.OSFullName.ToString, "")
        'AddCustomList(MainPath & "SysInfo.lst", "    OS Platform = " + My.Computer.Info.OSPlatform.ToString, "    OS Platform = " + My.Computer.Info.OSPlatform.ToString, "")
        AddCustomList(MainPath & "SysInfo.lst", "    OS Platform = " + GetOSArchitecture(), "    OS Platform = " + GetOSArchitecture(), "")
        AddCustomList(MainPath & "SysInfo.lst", "    OS Version = " + My.Computer.Info.OSVersion.ToString, "    OS Version = " + My.Computer.Info.OSVersion.ToString, "")

        AddCustomList(MainPath & "SysInfo.lst", "Processor", "Processor", "")
        AddCustomList(MainPath & "SysInfo.lst", "    Number Physical = " + cSystemInfo.NumberOfCPUsPhysical, "    Number Physical = " + cSystemInfo.NumberOfCPUsPhysical, "")
        AddCustomList(MainPath & "SysInfo.lst", "    Number Logical = " + cSystemInfo.NumberOfCPUsLogical, "    Number Logical = " + cSystemInfo.NumberOfCPUsLogical, "")
        AddCustomList(MainPath & "SysInfo.lst", "    Manufacturer = " + cSystemInfo.CPUManufacturer, "    Manufacturer = " + cSystemInfo.CPUManufacturer, "")
        AddCustomList(MainPath & "SysInfo.lst", "    Name = " + cSystemInfo.CPUName, "    Name = " + cSystemInfo.CPUName, "")
        AddCustomList(MainPath & "SysInfo.lst", "    Description = " + cSystemInfo.CPUDescription, "    Description = " + cSystemInfo.CPUDescription, "")
        AddCustomList(MainPath & "SysInfo.lst", "    Address Width = " + cSystemInfo.AddressWidth, "    Address Width = " + cSystemInfo.AddressWidth, "")
        AddCustomList(MainPath & "SysInfo.lst", "    Clock Speed Max = " + cSystemInfo.ClockSpeedMax, "    Clock Speed Max = " + cSystemInfo.ClockSpeedMax, "")
        AddCustomList(MainPath & "SysInfo.lst", "    Clock Speed Current = " + cSystemInfo.ClockSpeedCurrent, "    Clock Speed Current = " + cSystemInfo.ClockSpeedCurrent, "")

        AddCustomList(MainPath & "SysInfo.lst", "Memory", "Memory", "")
        AddCustomList(MainPath & "SysInfo.lst", "    Total Physical = " + My.Computer.Info.TotalPhysicalMemory.ToString("#,##0"), "    Total Physical = " + My.Computer.Info.TotalPhysicalMemory.ToString("#,##0"), "")
        AddCustomList(MainPath & "SysInfo.lst", "    Available Physical = " + My.Computer.Info.AvailablePhysicalMemory.ToString("#,##0"), "    Available Physical = " + My.Computer.Info.AvailablePhysicalMemory.ToString("#,##0"), "")
        AddCustomList(MainPath & "SysInfo.lst", "    Total Virtual = " + My.Computer.Info.TotalVirtualMemory.ToString("#,##0"), "    Total Virtual = " + My.Computer.Info.TotalVirtualMemory.ToString("#,##0"), "")
        AddCustomList(MainPath & "SysInfo.lst", "    Available Virtual = " + My.Computer.Info.AvailableVirtualMemory.ToString("#,##0"), "    Available Virtual = " + My.Computer.Info.AvailableVirtualMemory.ToString("#,##0"), "")

        AddCustomList(MainPath & "SysInfo.lst", "Hard Drives", "Hard Drives", "")
        For Each strX In cSystemInfo.DiskSpace
            AddCustomList(MainPath & "SysInfo.lst", "    " + strX, "    " + strX, "")
        Next

        AddCustomList(MainPath & "SysInfo.lst", "Screens", "Screens", "")
        For Each strX In cSystemInfo.Screens
            AddCustomList(MainPath & "SysInfo.lst", "    " + strX, "    " + strX, "")
        Next


        AddCustomList(MainPath & "SysInfo.lst", ".Net Framework installed", ".Net Framework installed", "")
        AddCustomList(MainPath & "SysInfo.lst", "    Highest = " + cSystemInfo.HighestFrameworkVersion, "    Highest = " + cSystemInfo.HighestFrameworkVersion, "")
        AddCustomList(MainPath & "SysInfo.lst", "    All", "    All", "")
        For Each strX In cSystemInfo.ListFrameworkVersions
            AddCustomList(MainPath & "SysInfo.lst", "        " + strX, "        " + strX, "")
        Next

        SDK.Execute("SETVAR;MOBILEPHONE_SETTINGSINFO;-||SETVAR;MOBILEPHONE_INFO;*** System Informations ***||MENU;MOBILEPHONE_INFO.skin||CLCLEAR;ALL||CLLOAD;" & MainPath & "SysInfo.lst")
    End Sub

    Public Function CheckFramework35Ready() As Boolean
        Dim retBool35 As Boolean = False
        For Each strX In cSystemInfo.ListFrameworkVersions
            If strX = "3.5" Then
                retBool35 = True
                Exit For
            End If
        Next
        Return retBool35
    End Function


#End Region ' - Framework -

#Region "Ringin Feature"
    Public Sub TimerRinggingBuild()
        Try
            Dim duration As Double = 0
            If File.Exists(MainPath & "RingTone\ringin.wav") = True Then
                duration = GetDuration(MainPath & "RingTone\ringin.wav")
            Else
                Exit Sub
            End If

            'create a timer with a three seconds interval.
            If IsNothing(timer3) = True Then
                timer3 = New System.Timers.Timer()
                timer3.Interval = Math.Round(duration) + 2000
                ' Hook up the Elapsed event for the timer.
                AddHandler timer3.Elapsed, AddressOf TimerRingin
                ' Have the timer fire repeated events (true is the default)
                timer3.AutoReset = True
                timer3.Enabled = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub TimerRingin()
        If PluginRunForDS = False Then
        Else
            SDK.Execute("PLAYSOUND;" & MainPath & "RingTone\ringin.wav")
        End If
    End Sub
#End Region

#Region "Drives Liste"
    Public Sub DrivesList()
        Dim allDrives() As DriveInfo
        allDrives = DriveInfo.GetDrives()
        Dim disknumber As Integer = 1
        'convert to Megabytes
        For i = 1 To 8
            SDK.SetUserVar("DISKTYPE" & i, "")
            'INI.Write("FTPCOPY", "DISK" & i, "#")
        Next
        Try
            For Each disk As DriveInfo In allDrives
                'SDK.Execute("SAVETOSKIN;DISK" & disknumber & ";" & UCase(disk.Name) & "||SETVAR;DISK" & disknumber & ";" & UCase(disk.Name))
                If disk.DriveType = 3 Or disk.DriveType = 2 Or disk.DriveType = 6 Then
                    'INI.Write("FTPCOPY", "DISK" & disknumber, UCase(disk.Name))
                    If disk.IsReady = True Then
                        SDK.SetUserVar("DISK" & disknumber, UCase(disk.Name))
                        SDK.SetUserVar("DISKTYPE" & disknumber, disk.DriveType)
                        SDK.SetUserVar("DISKLABEL" & disknumber, disk.VolumeLabel)
                        SDK.SetUserVar("DISKAVAILABLEFREESPACE" & disknumber, Format(disk.AvailableFreeSpace \ 1024 \ 1024 \ 1024, "###,###" & " GB"))
                        SDK.SetUserVar("DISKTOTALFREESPACE" & disknumber, disk.TotalFreeSpace)
                        SDK.SetUserVar("DISKTOTALSIZE" & disknumber, disk.TotalSize)
                    Else
                        Continue For
                    End If
                Else
                    'INI.Write("FTPCOPY", "DISK" & disknumber, UCase(disk.Name))
                    SDK.SetUserVar("DISK" & disknumber, UCase(disk.Name))
                    SDK.SetUserVar("DISKTYPE" & disknumber, disk.DriveType)
                End If

                disknumber = disknumber + 1

                'Console.WriteLine("Drive {0}", disk.Name)
                'Console.WriteLine("  File type: {0}", disk.DriveType)
                'If disk.IsReady = True Then
                '    Console.WriteLine("  Volume label: {0}", disk.VolumeLabel)
                '    Console.WriteLine("  File system: {0}", disk.DriveFormat)
                '    Console.WriteLine("  Available space to current user:{0, 15} bytes", disk.AvailableFreeSpace)
                '    Console.WriteLine("  Total available space:          {0, 15} bytes", disk.TotalFreeSpace)
                '    Console.WriteLine("  Total size of drive:            {0, 15} bytes ", disk.TotalSize)
                'End If
            Next
        Catch ex As Exception
            MessageBox.Show("FTPCopy error: " & ex.Message) ' & vbCrLf & ex.InnerException.ToString)
        End Try
        If disknumber > allDrives.Length Then
            For i = disknumber To 8
                'SDK.Execute("SAVETOSKIN;DISK" & i & ";#||SETVAR;DISK" & i & ";#")
                'INI.Write("FTPCOPY", "DISK" & i, "#")
                SDK.SetUserVar("DISK" & i, "#")
                'i = i + 1
            Next
        End If
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class


'Public Class bgwArguments
'    Private _nbr As Integer
'    Private _pause As Integer

'    Public Sub New(ByVal nbrCompteur As Integer, ByVal pauseCompteur As Integer)
'        _nbr = nbrCompteur
'        _pause = pauseCompteur
'    End Sub

'    Public Property nbr() As Integer
'        Get
'            Return _nbr
'        End Get
'        Set(ByVal value As Integer)
'            _nbr = value
'        End Set
'    End Property

'    Public Property pause() As Integer
'        Get
'            Return _pause
'        End Get
'        Set(ByVal value As Integer)
'            _pause = value
'        End Set
'    End Property
'End Class

'Public Class bgwProgress
'    Private _text As String

'    Public Sub New(ByVal txt As String)
'        _text = txt
'    End Sub

'    Public Property text() As String
'        Get
'            Return _text
'        End Get
'        Set(ByVal value As String)
'            _text = value
'        End Set
'    End Property

'End Class

