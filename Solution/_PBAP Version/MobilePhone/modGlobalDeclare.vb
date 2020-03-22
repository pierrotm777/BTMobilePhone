Imports System.Collections.Generic
Imports System.Globalization
Imports System.Threading
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml
Imports MobilePhone.PhoneNumberFormatter
Imports System.Runtime.InteropServices
Imports System.Runtime.CompilerServices
Imports System.Management

'Imports PhoneNumbers
'Imports NUnit.Framework
'Imports PhoneNumbers.Test

Module modGlobalDeclare


#Region "Variables"
    Public PluginSettings As cMySettings
    Public TempPluginSettings As cMySettings
    Public MainPath As String = ""
    Public SkinPath As String = ""
    Public DebuglogPath As String = ""
    Public dialNumber As String = ""
    Public dialnumberFromFavorite As String = ""
    Public PhoneStatus As String = ""
    Public RRScreen As String
    Public PhoneDate As String = ""
    Public PhoneTime As String = ""

    Public MsgIsReceived As String
    Public MsgReceivedType As String
    Public MsgLastUnreadCheckTime As DateTime = Nothing
    Public MsgNumberOfNewUnread As Integer = 0
    Public MsgNumberOfNewUnreadOld As Integer = 0

    Public SmsUnreadOnly As Boolean = True
    Public IncomingCall As Boolean = False
    Public ImeiNumber As String
    Public IsmiNumber As String
    Public RevisionNumber As String
    Public PhoneBookSyncInProgress As Boolean = False
    Public hfpCallIsActive As Boolean = False
    Public ExternalPowerIsConnected As Boolean = False
    Public ExternalPowerStatus As Boolean = False
    'Public ExternalPowerInfo As String = ""
    Public BatteryFullCharge As Boolean = False
    Public debugFrm As New frmDebugBox
    Public ATValueResult As String
    Public IconDeviceType As String
    Public ServiceList As New Dictionary(Of String, String)
    Public CountryList As New Dictionary(Of String, Integer)

    Public PhoneCheckedIs As Integer = 1
    Public NumberOfDeviceFound As Integer = 0
    Public PhoneDeviceNameList As New List(Of String)

    Public SpeechRecognitionIsActive As Boolean = False
    Public SpeechRecognizedIs As String = ""
    Public SpeechToNumberError As String = ""
    Public SpeechToNumberList As New Dictionary(Of String, Integer)
    Public SpeechArray() As String
    Public MyCultureInfo As String = ""
    Public MyPhoneCultureInfo As String = ""

    'Public SearchThread As System.Threading.Thread
    'Public workerThread As Threading.Thread

    'Alarm variables
    Public AlarmIndicator_Monday As Boolean = False
    Public AlarmIndicator_Tuesday As Boolean = False
    Public AlarmIndicator_Wednesday As Boolean = False
    Public AlarmIndicator_Thirsday As Boolean = False
    Public AlarmIndicator_Friday As Boolean = False
    Public AlarmIndicator_Saturday As Boolean = False
    Public AlarmIndicator_Sunday As Boolean = False
    Public AlarmIndicator_Week As Boolean = False
    Public Alarm1IsOn As Boolean = False
    Public Alarm2IsOn As Boolean = False
    Public Alarm3IsOn As Boolean = False
    Public Alarm4IsOn As Boolean = False
    Public Alarm5IsOn As Boolean = False
    Public AlarmAttributes As String

    Public CellularPhoneValue As String
    Public DepartmentValue As String
    Public EmailAddressValue As String
    Public FormattedNameValue As String
    Public HomePhoneValue As String
    Public JobTitleValue As String
    Public OfficeValue As String
    Public OrganizationValue As String
    Public PagerValue As String
    Public PersonalWebSiteValue As String
    Public WorkFaxValue As String
    Public WorkPhoneValue As String
    Public WorkWebSiteValue As String

    Public AVRCP_lblAValbum As String
    Public AVRCP_lblAVartist As String
    Public AVRCP_lblAVtitle As String
    Public AVRCP_lblAVtrackpos As String
    Public AVRCP_lblAVtracklen As String
    Public AVRCP_repeat As Boolean = False
    Public AVRCP_shuffle As Boolean = False
    Public AVRCP_mute As Boolean = False
    Public AVRCP_status As String = ""

    Public ServiceSPP_Usable As Boolean = False
    Public ServiceOPP_Usable As Boolean = False
    Public ServiceFTP_Usable As Boolean = False
    Public ServicePBAP_Usable As Boolean = False
    Public ServiceMAP_Usable As Boolean = False
    Public ServicePAN_Usable As Boolean = False
    Public ServiceAVRCP_Usable As Boolean = False
    Public ServiceA2DP_Usable As Boolean = False

    Public SDKErrorList As New Dictionary(Of UInteger, String)
#End Region

#Region "New Variables"
    Private Const BTSDK_TRUE As UInt32 = 1

    Public handlersMainDone As Boolean = False
    Public handlersSpeechDone As Boolean = False

    Public dvcNameArray(0 To 0) As String
    Public dvcHandleArray(0 To 0) As UInt32
    Public dvcArrayCount As Integer = 0
    Public dvcCurrHandle As UInt32 = 0
    Public dvcCurrAddress As String = ""

    Public lblDeviceLink As String = ""
    Public lblDeviceRcvd As String = ""
    Public lblDeviceSent As String = ""
    Public lblDeviceRSSI As String = ""
    Public lblDeviceAddress As String = ""

    Public panHandleDvc As UInt32 = 0
    Public panHandleConn As UInt32 = 0
    Public panHandleSvc As UInt32 = 0
    Public panIPaddress As String = "0.0.0.0"
    Public panGetUpDownStatsBytesSent As Long = 0
    Public panGetUpDownStatsBytesReceived As Long = 0

    Public sppHandleDvc As UInt32 = 0
    Public sppHandleConn As UInt32 = 0
    Public sppHandleSvc As UInt32 = 0
    Public sppCOMMportNum As Integer = 0

    Public avrcpHandleDvc As UInt32 = 0
    Public avrcpHandleConn As UInt32 = 0
    Public avrcpHandleSvc As UInt32 = 0
    Public avrcpTrackTitle As String = ""
    Public avrcpTrackArtist As String = ""
    Public avrcpTrackAlbum As String = ""
    Public avrcpTrackPos As Double = 0
    Public avrcpTrackLen As Double = 0
    Public avrcpAbsVolPct As Double = 100
    Public avrcpIsPlaying As Boolean = False
    Public avrcpBrowsingOk As Boolean = False

    Public avrcpTrackPosUpdateTicks As Long = 0         'we are gonna use this to calculate the track position in between notifications.
    Public avrcpNeedsTrackInfo As Boolean = True        'these are used for knowing when to request track/status, instead of querying it every few seconds..
    Public avrcpNeedsPlayStatus As Boolean = True       ''
    Public avrcpNeedsSupportedEvents As Boolean = True

    Public avrcpEventSupported_PlaybackStatusChanged As Boolean = False
    Public avrcpEventSupported_TrackChanged As Boolean = False
    Public avrcpEventSupported_TrackEnded As Boolean = False
    Public avrcpEventSupported_TrackStarted As Boolean = False
    Public avrcpEventSupported_TrackPosChanged As Boolean = False
    Public avrcpEventSupported_BatteryStatusChanged As Boolean = False
    Public avrcpEventSupported_SystemStatusChanged As Boolean = False
    Public avrcpEventSupported_PlayerSettingChanged As Boolean = False
    Public avrcpEventSupported_NowPlayingContentChanged As Boolean = False
    Public avrcpEventSupported_NumPlayersChanged As Boolean = False
    Public avrcpEventSupported_CurrPlayerChanged As Boolean = False
    Public avrcpEventSupported_UIDsChanged As Boolean = False
    Public avrcpEventSupported_VolumeChanged As Boolean = False


    Public hfpHandle_LocalSvc_HFAG As UInt32 = 0
    Public hfpHandle_LocalSvc_HFunit As UInt32 = 0
    Public hfpHandle_LocalSvc_HSAG As UInt32 = 0
    Public hfpHandle_LocalSvc_HSunit As UInt32 = 0

    Public hfpHandleConnHSAG As UInt32 = 0

    Public hfpHandleDvc As UInt32 = 0
    Public hfpHandleConnHFAG As UInt32 = 0
    Public hfpHandleSvc As UInt32 = 0
    Public hfpStatusStr As String = "Not Connected"
    Public hfpSignalPct As Double = 0
    Public hfpBatteryPct As Double = 0
    Public hfpBatteryPctOld As Double = 0
    Public hfpLastBatteryCheckTime As DateTime = Nothing
    Public hfpSpeakerVolumePct As Double = 15
    Public hfpMicVolumePct As Double = 15
    Public hfpNetworkAvailable As Boolean = False
    Public hfpNetworkName As String = ""
    Public hfpCallerIDno As String = ""
    Public hfpCallerIDname As String = ""
    Public hfpSubscriberNo As String = ""
    Public hfpSubscriberName As String = ""
    Public hfpModelName As String = ""
    Public hfpManufacturerName As String = ""
    Public hfpManufacturerNameParsed As String = ""
    Public hfpVoiceCmdStateEnabled As Boolean = False
    Public hfpIsRoaming As Boolean = False
    Public hfpRequestCounter As Integer = 0
    Public hfpATCmdResult As String = ""
    Public hfpContactNumberInBlackList As Integer = 0
    Public hfpDeviceIsActive As Boolean = False
    Public hfpTimerInit As Double = 0

    Public pbapListNeedUpdate As Boolean = True
    Public pbapHistoryListIsSimplified As Boolean = True
    Public pbapReturnFromCall As String = ""
    Public pbapUnknowName As String = ""
    Public pbapHandleDvc As UInt32 = 0
    Public pbapHandleConn As UInt32 = 0
    Public pbapHandleSvc As UInt32 = 0
    Public PhoneBookEntries(0 To 0) As DLcontact
    Public PhoneBookEntryCount As Integer = 0
    Public PhoneBookNumContactsRetrieved As Integer = -1
    Public Structure DLcontact
        Dim EntryFullName As String
        Dim EntryName As String
        Dim EntryNamePrefix As String
        Dim EntryNameSuffix As String
        Dim EntryPhoneNumberCount As Integer
        Dim EntryPhoneAddressCount As Integer
        Dim EntryPhoneNumbers() As DLphoneNumber
        Dim EntryAddress() As DLphoneNumber
        Dim EntryImage As Bitmap
        'Dim EntryImage As String
        Dim EntryEmail As String
        Dim EntryBirthDay As String
        Dim EntryGeoPosition As String
        Dim EntryNote As String
        Dim EntryOrganisation As String
    End Structure
    Public Structure DLphoneNumber
        Dim Location As String
        Dim EntryPhoneNumber As String
        Dim EntryPOBox As String
        Dim EntryExtAddress As String
        Dim EntryStreetAddress As String
        Dim EntryLocalityCity As String
        Dim EntryRegionState As String
        Dim EntryPostalCode As String
        Dim EntryCountry As String
        Dim EntryFullAddress As String
    End Structure
    Public cardBirthday As New List(Of String)
    Public cardBirthdayEntAllReadySend As Boolean = False

    Public msgAttachExist As Boolean = False
    Public LoadMAPService As Boolean = True
    Public mapHandleDvc As UInt32 = 0
    Public mapHandleConn As UInt32 = 0
    Public mapHandleSvc As UInt32 = 0
    Public mapHandleMNSsvc As UInt32 = 0
    'Public mapHandleMASsvc As UInt32 = 0
    Public mapMsgReceived As Boolean = False
    Public mapMsgPopUpAllreadySend As Boolean = False

    Public map_LastSMS_Handle As String = ""
    Public map_LastSMS_FromNumber As String = ""
    Public map_LastSMS_FromName As String = ""
    Public map_LastSMS_Text As String = ""
    Public map_LastSMS_FromImage As String = ""
    Public map_IsRefreshing As Boolean = False
    Public map_DeviceName As String = ""

    Public map_MsgHistoryItems(0 To 0) As DLmessage
    Public map_MsgHistoryCount As Integer = 0
    Public map_LastRefreshTicks As Long = 0
    Public Structure DLmessage
        Dim msgFromNumber As String
        Dim msgFromName As String
        Dim msgText As String
        Dim msgType As String
        Dim msgHandle As String
        Dim msgReadState As String
        Dim msgDateTime As DateTime
        Dim msgImage As String
        Dim msgAttachmentSizes As Integer
        Dim msgAttachmentName As String
    End Structure
    Public fileHandles As New List(Of String)

    Public dvcInfos(0 To 0) As DeviceInfo
    Public dvcInfosCount As Integer = 0
    Public Structure DeviceInfo
        Dim dvcName As String
        Dim dvcHandle As UInt32
        Dim dvcMac As String
        Dim dvcIsPaired As boolean
    End Structure

    Public hfpIsActive As Boolean = False
    Public mapIsActive As Boolean = False
    Public avrcpIsActive As Boolean = False
    Public panIsActive As Boolean = False
    Public sppIsActive As Boolean = False
    Public a2dpIsActive As Boolean = False
    Public oppIsActive As Boolean = False
    Public ftpIsActive As Boolean = False

    'hfp variables
    Public hfpdevicename As String = ""
    Public hfpdevicetype As String = ""
    Public hfpdevicemacaddress As String = ""
    Public hfpStateStr As String = ""
    Public hfpCallerIDnoFormatted As PhoneNumberFormatter '= New PhoneNumberFormatter(TempPluginSettings.PhoneCountryCodes(0), TempPluginSettings.PhoneCountryCodes(1), TempPluginSettings.PhoneCountryCodes(2))

    Public ftpHandleDvc As UInt32 = 0
    Public ftpHandleConn As UInt32 = 0
    Public ftpHandleSvc As UInt32 = 0
    Public ftpRemotePath As String = ""

    Public oppHandleDvc As UInt32 = 0
    Public oppHandleConn As UInt32 = 0
    Public oppHandleSvc As UInt32 = 0

    Private Delegate Sub DelegateFTPfoundFolder(ByVal foundFolderName As String)    'these are used for handling some FTP events on the UI thread.
    Private Delegate Sub DelegateFTPfoundFile(ByVal foundFileName As String, ByVal foundFileSize As UInt64)


    'a2dp variables
    Public a2dpHandleDvc As UInt32 = 0
    Public a2dpHandleConn As UInt32 = 0
    Public a2dpHandleSvc As UInt32 = 0
    Public a2dpHandleSNKsvc As UInt32 = 0
    Public a2dpHandleSRCsvc As UInt32 = 0

    'gatt variables
    Public gattHandleDvc As UInt32 = 0
    Public gattHandleConn As UInt32 = 0
    Public gattHandleSvc As UInt32 = 0

    'Possible phone states
    Public Calls_IsRinging As Boolean = False
    Public Calls_IsOnCall As Boolean = False
    Public Calls_IsReady As Boolean = False

    'Events variables
    Public Event SetIndBlinkOn()                                'Raise an event for run blink of synch indicator
    Public Event SetIndBlinkOff()                               'Raise an event for stop blink of synch indicator
    Public Event BlueSoleil_Event_HFP_BatteryIsFull()           'Raises an event when the battery is 100%

    Public Event BlueSoleil_Event_HFP_Missedcall()          'Raise an evnt if a call is missed

    Public Event BlueSoleil_Event_HFP_ExtPowerBattON()      'Raises an event when an external power is connected to the phone.
    Public Event BlueSoleil_Event_HFP_ExtPowerBattOFF()     'Raises an event when an external power is unconnected to the phone.

    Public Event BlueSoleil_Event_HFP_OtherPhoneConnected(ByVal phoneName As String)

    Public Event BlueSoleil_Event_PBAP_ContactsListUsable()
    Public Event BlueSoleil_Event_PBAP_IsBirthDay(ByVal count As Integer)

    Public Event BlueSoleil_Event_MAP_MsgIsReceived(ByVal name As String, ByVal number As Integer, ByVal subject As String, ByVal attach As Boolean)
    Public Event BlueSoleil_Event_MAP_MsgIsSend(ByVal name As String, ByVal number As String)

    Public Event BlueSoleil_Event_PAN_IsOn()
    Public Event BlueSoleil_Event_PAN_IsOff()
    Public Event BlueSoleil_Event_WIFI_IsOn()
    Public Event BlueSoleil_Event_WIFI_IsOff()
    Public Event BlueSoleil_Event_LAN_IsOn()
    Public Event BlueSoleil_Event_LAN_IsOff()

    Public Event BlueSoleil_Event_ERROR_Return(ByVal newerror As String, ByVal usepopup As Boolean, useasrrcommand As Boolean)

    Public timer1 As System.Timers.Timer 'update all info each (3s)
    Public timer2 As System.Timers.Timer 'check presence phone device (30s)
    Public timer3 As System.Timers.Timer 'ringing timer (delay = .wav duration + 2s)
    Public timer1IsRunning As Boolean = False
    Public timer2IsRunning As Boolean = False

    Public startPluginTime As DateTime 'check time that plugin is running
    Public startPluginTimeStr As String = ""

    Public timerBatteryCounter As Integer = 0
    Public timerMapCounter As Integer = 0
    Public timerNetworkCounter As Integer = 0
    Public timerNetworkSpeed As Integer = 0
    Public OldDownload As UInteger = 0
    Public timerPluginTimeStr As Integer = 0

    Private DetectedDevice As Boolean = False

    Public OldPhoneDebugLog As String = ""
    Public OldDeviceName As String = ""
    Public OldDeviceName2 As String = ""
    Public OldLanguage As String = ""
    Public OldEmergencyNumber As String = ""
    Public OldPhoneCountryCodes As String = ""

    Public PluginRunForDS As Boolean

    Public CMEErrorList As New Dictionary(Of UInteger, String)
    Public LanguageList As New Dictionary(Of String, String)

    Public NetWorkNameList As New List(Of String)
    Public BluetoothPANNetworkAdapter As Boolean = False

    Public KeyBoardIsRussian As Boolean = False

    Public BirthdayAlreadyChecked As Boolean = False

#End Region

#Region "BS Init/DeInit"
    Public Function btnInitBlueSoleil() As Boolean

        BlueSoleil_Init()

        Dim retBool As Boolean
        Dim startTime As DateTime = Now
        Do
            'My.Application.DoEvents()
            Threading.Thread.Sleep(100)
            If BlueSoleil_IsSDKinitialized() = True Then
                RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;BlueSoleil is On", False, True)
                ToLog("BlueSoleil is On")
                Exit Do
            End If
            If Now.Subtract(startTime).TotalSeconds > 5 Then 'ne démarre plus si < 5
                Exit Do
            Else
                'My.Application.DoEvents()
            End If

        Loop

        ToLog("BlueSoleil try BlueSoleil_Status_RegisterCallbacks()")
        retBool = BlueSoleil_Status_RegisterCallbacks()

        If TempPluginSettings.RestartBTBeforeStart = True Then
            ToLog("BlueSoleil try BlueSoleil_StopBlueTooth()")
            retBool = BlueSoleil_StopBlueTooth()
        End If
        If BlueSoleil_IsBluetoothReady() = False Then
            ToLog("BlueSoleil try BlueSoleil_StartBlueTooth()")
            retBool = BlueSoleil_StartBlueTooth()
        End If

        ToLog("BlueSoleil try BlueSoleil_SetLocalDeviceServiceClass()")
        retBool = BlueSoleil_SetLocalDeviceServiceClass(True, True, True)
        'MsgBox("Done.")

        Return retBool
    End Function

    Public Sub btnDeInitBlueSoleil()
        BlueSoleil_Status_UnregisterCallbacks()
        BlueSoleil_Done()

        'MsgBox("Done.")

    End Sub

    Public Sub btnGetOpenConnections()


        Dim retConnHandles(0 To 0) As UInt32, retConnDvcHandles(0 To 0) As UInt32, retConnSvcHandles(0 To 0) As UInt32, retConnSvcClasses(0 To 0) As UShort
        Dim retConnCount As Integer = 0

        BlueSoleil_GetLocalDeviceConnections(retConnHandles, retConnDvcHandles, retConnSvcHandles, retConnSvcClasses, retConnCount)

        Dim openConnsStr As String = ""

        Dim i As Integer, retSvcName As String = ""
        For i = 0 To retConnCount - 1
            BlueSoleil_GetRemoteServiceAttributes(retConnSvcHandles(i), retSvcName, 0)
            If retSvcName = "" Then

            End If
            openConnsStr = openConnsStr & retConnHandles(i) & "   " & retSvcName & "    " & retConnSvcClasses(i) & vbNewLine
        Next i

        'MsgBox("Open Service Connections:   " & retConnCount & vbNewLine & openConnsStr)
        RaiseEvent BlueSoleil_Event_ERROR_Return("Open Service Connections:   " & retConnCount & " " & openConnsStr, True, False)
        retConnCount = retConnCount

    End Sub

#End Region

    '***********************************************
    'https://github.com/CompulsiveCode/BlueSol_NET
    '***********************************************

#Region "GATT service"
    Private Sub btnGATTgetSvcs()
        If dvcCurrHandle = 0 Then Exit Sub

        'get service handles for device.
        Dim svcHandleArray(0 To 0) As UInt32, svcHandleCount As Integer = 0

        Dim dvcHandleLE As UInt32 = BlueSoleil_GetRemoteDeviceHandle_LE(dvcCurrAddress)

        Dim TorF As Boolean
        Dim gattSvcUUIDs(0 To 0) As String, gattSvcAttribHandles(0 To 0) As UShort, gattSvcCount As Integer = 0
        TorF = BlueSoleil_GATT_GetDeviceServices(dvcHandleLE, gattSvcUUIDs, gattSvcAttribHandles, gattSvcCount)

        gattSvcCount = gattSvcCount

    End Sub

    Private Sub btnGATTconnect()

        Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text

        'Dim masBTpath As String = My.Application.Info.DirectoryPath
        'If Strings.Right(masBTpath, 1) <> "\" Then masBTpath = masBTpath & "\"
        'masBTpath = masBTpath & "MASserver"
        'BlueSoleil_MAP_RegisterServers(mapHandleMNSsvc, mapHandleMASsvc, masBTpath)

        ' mapHandleMNSSvc = BlueSoleil_MAP_RegisterNotificationService()

        Dim connTorF As Boolean = BlueSoleil_ConnectService_ByName("GATT", phoneDvcName, gattHandleDvc, gattHandleConn, gattHandleSvc)

        If gattHandleConn <> 0 Then
            MsgBox("Done.  Return = " & (gattHandleConn <> 0))
        Else
            MsgBox("Done.  Return = " & (gattHandleConn <> 0))
        End If

    End Sub

    Private Sub btnGATTdisconnect()

        BlueSoleil_DisconnectServiceConn(gattHandleConn)
        gattHandleConn = 0

        MsgBox("Done.")

    End Sub
#End Region

#Region "HFP service"

    ''' <summary>
    ''' Init BlueSoleil
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitHfpAndMapService()


        '*************************************************************
        'Its very important to separate the HFP init in two times.
        '-run btnInitBlueSoleil and BlueSoleil_IsSDKinitialized in first.
        '-wait than BlueSoleil_IsSDKinitialized() return a True value
        '-run only after that btnHandsFreeConnect(PhoneCheckedIs).
        '*************************************************************

        Dim retBool As Boolean = False
        If BlueSoleil_IsSDKinitialized() = False Then
            retBool = btnInitBlueSoleil()
            'check if BlueSoleilCS.exe process is ready!
            If retBool = True Then
                '
            Else
                If BlueSoleilCS_IsRunning() = True Then
                    retBool = BlueSoleilCS_KillBSprocess()
                    Thread.Sleep(500) 'Thread.Sleep(1000)
                    If retBool = True Then
                        BlueSoleilCS_Restart()
                        Thread.Sleep(500) 'Thread.Sleep(1000)
                        retBool = btnInitBlueSoleil()
                    End If
                End If
                Thread.Sleep(500) 'Thread.Sleep(3000) 'Exit Sub
            End If
        End If

        Dim startTime As DateTime = Now

        Do ' initiate only the SDK
            Threading.Thread.Sleep(500) 'Threading.Thread.Sleep(1000)
            ToLog("BlueSoleil SDK not ready, i wait .5 second")
            If Now.Subtract(startTime).TotalSeconds > TempPluginSettings.PhoneStartupDelay Then '5 Then  'With this timer, RR has time for load all plugins
                If BlueSoleil_IsSDKinitialized() = True Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;BlueSoleil SDK Ready", False, True)
                    ToLog("BlueSoleil SDK Ready")
                    Exit Do
                End If
            End If
        Loop

        startTime = Now
        Do 'initiate now the HFP service
            Threading.Thread.Sleep(500) 'Threading.Thread.Sleep(1000)
            ToLog("BlueSoleil HFP service not Ready, i wait .5 second")
            If Now.Subtract(startTime).TotalSeconds > TempPluginSettings.PhoneStartupDelay Then '5 Then
                If btnHandsFreeConnect(PhoneCheckedIs) = True Then 'connect the phone 1 by default
                    BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CGSN", 500) 'lecture N°IMEI
                    BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CIMI", 500) 'lecture N°ISMI
                    'BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CGMR", 500) 'lecture revision number
                    Exit Do
                End If
            End If
        Loop

        If LoadMAPService = True Then
            startTime = Now
            btnMsgConnect() 'initiate now the MAP service
            Do
                Threading.Thread.Sleep(500) 'Threading.Thread.Sleep(1000)
                If (mapHandleConn <> 0) = True Then
                    mapIsActive = True
                    If TempPluginSettings.PhoneNoSMSPopupInfo = True Then
                        RaiseEvent BlueSoleil_Event_ERROR_Return("MAP is ready", False, False)
                    End If
                    ToLog("MAP is ready, quit do loop")
                    Exit Do
                End If
                If Now.Subtract(startTime).TotalSeconds > TempPluginSettings.PhoneStartupDelay Then '5 Then
                    If mapHandleConn = 0 Then
                        RaiseEvent BlueSoleil_Event_ERROR_Return("MAP isn't ready", False, False)
                        ToLog("MAP isn't ready, quit do loop")
                        Exit Do
                    End If
                End If
            Loop
        Else
            ToLog("MAP service is set OFF by variable MOBILEPHONE_NOMAPSERVICE")
        End If

        If TempPluginSettings.PhoneBookAutoUpdate = True Then 'update all phone book
            ToLog("All phone book are updated in start")
            Dim t As New Threading.Thread(Sub() RRVcardUpdateProcess(False), "") 'no event is send to RR after the update
            t.Start()
        End If
    End Sub

    Private Sub PhoneList_SelectedIndexChanged(PhoneDeviceName As String)
        ToLog("Update index is on !")
        'get device handle, store it.
        RaiseEvent BlueSoleil_Event_ERROR_Return("Search device " & PhoneDeviceName, False, False)
        If dvcArrayCount = 0 Then
            ToLog("No device found is ready!")
        End If
        Dim i As Integer
        For i = 0 To dvcArrayCount - 1
            If dvcNameArray(i) = PhoneDeviceName Then
                'MessageBox.Show(dvcNameArray(i))
                dvcCurrHandle = dvcHandleArray(i)
                Dim dvcAddress As String = BlueSoleil_GetRemoteDeviceAddress(dvcCurrHandle)
                lblDeviceAddress = "Add.: " & dvcAddress 'Device Address (idem phone mac address)
                dvcCurrAddress = dvcAddress
                RaiseEvent BlueSoleil_Event_ERROR_Return("Search device " & PhoneDeviceName & " found return mac address: " & dvcAddress, False, False)
                ToLog("Search device " & PhoneDeviceName & " found return mac address: " & dvcAddress)

                If TempPluginSettings.RunOnStart = False Then 'used only in manual connection mode.
                    RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE", False, True)
                End If
                Exit For
            End If
        Next i
    End Sub

    Public Function btnHandsFreeConnect(Optional secondPhone As Integer = 1) As Boolean

        Dim phoneDvcName As String = ""

        If secondPhone = 2 Then
            phoneDvcName = TempPluginSettings.PhoneDeviceName2
        ElseIf secondPhone = 1 Then
            phoneDvcName = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text
        End If

        'check if a phone is paired and saved in settings
        If phoneDvcName = "NONAME" Then
            ToLog(phoneDvcName & " isn't a phone name allready paired !")
            ToLog("Change NONAME in settings by your phone name !")
            If PluginRunForDS = False Then
                RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONE_NOPHONEFOUND", False, False)
            Else
                RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONE_NOPHONEFOUND", False, False)
            End If
            Return False
            Exit Function
        End If

        'I *think* the right way to do this is to connect A2DP first.  Then AVRCP, and then HFP.  Then disconnect in reverse order.
        'BlueSoleil_ConnectService_ByName("A2DP", phoneDvcName, a2dpHandleDvc, a2dpHandleConn, a2dpHandleSvc)

        hfpHandle_LocalSvc_HFAG = BlueSoleil_HFP_RegisterService_HandsFreeAudioGateway("TestBS_HFAG")
        hfpHandle_LocalSvc_HFunit = BlueSoleil_HFP_RegisterService_HandsFreeUnit("TestBS_HFunit")

        hfpHandle_LocalSvc_HSunit = BlueSoleil_HFP_RegisterService_HeadSetUnit("TestBS_HSunit")
        hfpHandle_LocalSvc_HSAG = BlueSoleil_HFP_RegisterService_HeadSetAudioGateway("TestBS_HSAG")

        BlueSoleil_HFP_RegisterCallbacks()

        Dim retBool As Boolean = False
        retBool = BlueSoleil_ConnectService_ByName("HFP", phoneDvcName, hfpHandleDvc, hfpHandleConnHFAG, hfpHandleSvc)

        startPluginTime = Now

        'create the timer2 for check the phone presence
        If IsNothing(timer2) = True Then
            timer2 = New System.Timers.Timer()
            timer2.Interval = 30000
            'timer2.AutoReset = True
            AddHandler timer2.Elapsed, AddressOf CkeckDevicePresence
            timer2.Enabled = False 'start the timer.
        End If

        'create a timer with a three seconds interval.
        If IsNothing(timer1) = True Then
            timer1 = New System.Timers.Timer()
            timer1.Interval = 3000
            ' Hook up the Elapsed event for the timer.
            AddHandler timer1.Elapsed, AddressOf RequestInfos
            ' Have the timer fire repeated events (true is the default)
            timer1.AutoReset = True
            timer1.Enabled = False
        End If

        If hfpHandleConnHFAG <> 0 Then
            RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;BlueSoleil HFP service is Ready", False, True)
            ToLog("BlueSoleil HFP service is Ready")
            Threading.Thread.Sleep(150)
            BlueSoleil_HFP_SendRequest_GetSubscriberNumber(hfpHandleConnHFAG)
            Threading.Thread.Sleep(150)
            BlueSoleil_HFP_SendRequest_GetNetworkOperator(hfpHandleConnHFAG)

            BlueSoleil_HFP_SetSpeakerVol(hfpHandleConnHFAG, 15)
            BlueSoleil_HFP_SetMicVol(hfpHandleConnHFAG, 15)
            BlueSoleil_HFP_GetManufacturer(hfpHandleConnHFAG, hfpManufacturerName)
            BlueSoleil_HFP_GetModel(hfpHandleConnHFAG, hfpModelName)

            If hfpModelName = "" Then
                hfpModelName = BlueSoleil_GetRemoteDeviceName(hfpHandleDvc)
            End If

            hfpStatusStr = "Connected"
            hfpIsActive = True
            ToLog(phoneDvcName & " is connected !")

            If timer1.Enabled = False Then
                ' Start the timer
                timer1.Enabled = True
                timer1.Start()
                timer1IsRunning = True
                RaiseEvent BlueSoleil_Event_ERROR_Return("Main timer is started (3s)", False, False)
                ToLog("Main timer is started")
            End If


            RaiseEvent BlueSoleil_Event_ERROR_Return("SMS timer will started each 60s", False, False)
            ToLog("SMS timer will started each 60s")

            'Local list kill access to remote list ?
            ToLog("Local Services list is off !")
            'Dim u As New Threading.Thread(Sub() GetLocalServicesList(False)) 'check the type of local services accepted
            'u.Start()

            'Thread.Sleep(1000)
            'ToLog("Remote Services list is off !")
            Dim t As New Threading.Thread(Sub() GetRemoteServicesList(False)) 'check the type of remote services accepted
            t.Start()

            'ToLog("Devices list is off !")
            Dim u As New Threading.Thread(Sub() btnRefreshDevices(False)) 'check the number of device ready and paired
            u.Start()

            'ToLog("Update index is off !")
            PhoneList_SelectedIndexChanged(phoneDvcName)

            'ToLog("THIS PLUGIN VERSION IS FOR TEST and TIMER1 IS OFF")
            'RaiseEvent BlueSoleil_Event_ERROR_Return("THIS PLUGIN VERSION IS FOR TEST and TIMER1 IS OFF", False, False)

            Return True
        Else
            If PluginRunForDS = False Then
                RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONE_PHONENOTFOUND", False, True)
            Else
                RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONE_PHONENOTFOUND", False, True)
            End If
            ToLog("Phone not found")
            BlueSoleil_HFP_UnregisterCallbacks()

            'enter in WAIT mode
            If timer2.Enabled = False Then 'create timer IF NOT ALREADY CREATED
                ClearHFPvariables()
                RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE enter in the WAIT mode (30s)", False, False)
                ToLog("MOBILEPHONE enter in the WAIT mode (30s)")
                timer2.Enabled = True 'start the timer.
                timer2.Start()
                timer2IsRunning = True
            End If
            Return False
        End If

        'MsgBox("Done.  Return = " & (hfpHandleConnHFAG <> 0))

        Return retBool

    End Function


    Public Sub btnHandsFreeDisconnect()
        Try
            hfpIsActive = False

            If IsNothing(timer1) = False Then
                timer1.Stop()
                timer1.Enabled = False 'stop update info
                timer1IsRunning = False
            End If
            If IsNothing(timer2) = False Then
                timer2.Stop()
                timer2.Enabled = False 'stop check presence device
                timer2IsRunning = False
            End If

            BlueSoleil_DisconnectServiceConn(hfpHandleConnHFAG)
            BlueSoleil_DisconnectServiceConn(hfpHandleConnHSAG)
            hfpHandleConnHFAG = 0
            hfpHandleConnHSAG = 0

            hfpHandleDvc = 0
            hfpStatusStr = "Not Connected"

            BlueSoleil_HFP_UnregisterService(hfpHandle_LocalSvc_HFunit)
            BlueSoleil_HFP_UnregisterService(hfpHandle_LocalSvc_HFAG)

            BlueSoleil_HFP_UnregisterService(hfpHandle_LocalSvc_HSunit)
            BlueSoleil_HFP_UnregisterService(hfpHandle_LocalSvc_HSAG)

            hfpHandle_LocalSvc_HFAG = 0
            hfpHandle_LocalSvc_HSAG = 0
            hfpHandle_LocalSvc_HFunit = 0
            hfpHandle_LocalSvc_HSunit = 0

            BlueSoleil_HFP_UnregisterCallbacks()
            RaiseEvent BlueSoleil_Event_ERROR_Return("BlueSoleil HFP is disconnected", False, False)
            ToLog("BlueSoleil HFP is disconnected")
        Catch ex As Exception
            ToLog("BlueSoleil HFP is disconnect error")
            'MessageBox.Show(ex.Message)
        End Try

        'MsgBox("Done.")
    End Sub

    Public Sub btnHandsFreeDial(ByVal tbxHandsFreePhoneNo As String)
        Dim hfpBool As Boolean = BlueSoleil_HFP_Dial(hfpHandleConnHFAG, tbxHandsFreePhoneNo)
    End Sub

    Public Sub btnHandsFreeHangUp()
        Dim hfpBool As Boolean = BlueSoleil_HFP_HangUp(hfpHandleConnHFAG)
        If hfpStatusStr = "Ringing" Then
            RaiseEvent BlueSoleil_Event_HFP_Missedcall()
        End If
    End Sub

    Public Sub btnHandsFreeAnswer()
        Dim hfpBool As Boolean = BlueSoleil_HFP_AnswerCall(hfpHandleConnHFAG)

    End Sub

    Public Function btnHandsFreeTransfer() As Boolean
        Dim retBool As Boolean = False
        retBool = BlueSoleil_HFP_TransferAudioConnection(hfpHandleConnHFAG)
        If retBool = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function btnHandsFreeVoiceRecognition(ByVal state As Boolean) As Boolean
        hfpVoiceCmdStateEnabled = state
        Dim retBool As Boolean = BlueSoleil_HFP_SetVoiceRecognitionState(hfpHandleConnHFAG, hfpVoiceCmdStateEnabled)
        Return retBool
    End Function

    Private Sub ClearHFPvariables()
        hfpSignalPct = 0
        hfpBatteryPct = 0
        avrcpTrackPos = 0
        hfpIsRoaming = False
        hfpNetworkAvailable = False
        ExternalPowerIsConnected = False
        BatteryFullCharge = False
        dialNumber = ""
        hfpManufacturerName = "-"
        hfpModelName = "-"
        hfpNetworkName = "-"
        hfpSubscriberNo = "-"
        hfpSubscriberName = "-"
        lblDeviceLink = "Link: -"
        lblDeviceRSSI = "RSSI: -"
        lblDeviceRcvd = "Rcvd: -"
        lblDeviceSent = "Sent: -"
    End Sub


#End Region

#Region "PBAP service"
    'Extract 1 by 1
    'Use BlueSoleil_PBAP_PullCardList to get the Card List file.  All it contains is a list of Names and Handles.
    'Then use BlueSoleil_PBAP_XML_GetCardListInfo to get an array of all the Handles and Names.
    'Loop through the array, using BlueSoleil_PBAP_PullCard to pull each card from the phone to a separate file on the PC.  
    'Use VCard_GetContactInfo to get the info from that VCard file, just like before.  You can specify the offset as zero, since these VCards only have one contact in them.
    Public Sub btnGetPhonebook(Optional book As String = "", Optional sendEventToRR As Boolean = True)
        If BlueSoleil_IsInstalled() = False Then Exit Sub
        If BlueSoleil_IsBluetoothReady() = False Then Exit Sub


        Dim writeToPath As String = MainPath & "PhoneBook" 'My.Application.Info.DirectoryPath
        'If Strings.Right(writeToPath, 1) <> "\" Then writeToPath = writeToPath & "\"
        'writeToPath = writeToPath & "Contacts"

        If IO.Directory.Exists(writeToPath) = False Then
            Try
                IO.Directory.CreateDirectory(writeToPath)
            Catch ex As Exception

            End Try
        End If

        Dim connBool As Boolean = False
        Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text

        connBool = BlueSoleil_ConnectService_ByName("PBAP", phoneDvcName, pbapHandleDvc, pbapHandleConn, pbapHandleSvc)

        If connBool = True Then
            PhoneBookSyncInProgress = True
            RaiseEvent SetIndBlinkOn()
        Else
            Exit Sub
        End If

        Dim retBool As Boolean = False

        If book = "" Then
            retBool = BlueSoleil_PBAP_PullPhoneBook(pbapHandleConn, MainPath & "PhoneBook\pb.vcf", "CONTACTS")
            Thread.Sleep(500)
            retBool = BlueSoleil_PBAP_PullPhoneBook(pbapHandleConn, MainPath & "PhoneBook\ich.vcf", "INCOMING")
            Thread.Sleep(500)
            retBool = BlueSoleil_PBAP_PullPhoneBook(pbapHandleConn, MainPath & "PhoneBook\och.vcf", "OUTGOING")
            Thread.Sleep(500)
            retBool = BlueSoleil_PBAP_PullPhoneBook(pbapHandleConn, MainPath & "PhoneBook\mch.vcf", "MISSED")
            Thread.Sleep(500)
            retBool = BlueSoleil_PBAP_PullPhoneBook(pbapHandleConn, MainPath & "PhoneBook\cch.vcf", "HISTORY")
            Thread.Sleep(500)

            BlueSoleil_DisconnectServiceConn(pbapHandleConn)
            Thread.Sleep(500)

            'Build the RR Lists
            GetInfoFromVcfFile(MainPath & "PhoneBook\pb.vcf")
            GetInfoFromVcfFile(MainPath & "PhoneBook\ich.vcf")
            GetInfoFromVcfFile(MainPath & "PhoneBook\och.vcf")
            GetInfoFromVcfFile(MainPath & "PhoneBook\mch.vcf")
            GetInfoFromVcfFile(MainPath & "PhoneBook\cch.vcf")

        Else
            Dim vcfFilteType As String = ""
            Select Case book
                Case "PB"
                    vcfFilteType = "CONTACTS"
                Case "ICH"
                    vcfFilteType = "INCOMING"
                Case "OCH"
                    vcfFilteType = "OUTGOING"
                Case "MCH"
                    vcfFilteType = "MISSED"
                Case "CCH"
                    vcfFilteType = "HISTORY"
            End Select
            retBool = BlueSoleil_PBAP_PullPhoneBook(pbapHandleConn, MainPath & "PhoneBook\" & LCase(book) & ".vcf", vcfFilteType)
            Thread.Sleep(500)
            BlueSoleil_DisconnectServiceConn(pbapHandleConn)
            Thread.Sleep(500)
            GetInfoFromVcfFile(MainPath & "PhoneBook\" & LCase(book) & ".vcf")
        End If

        If retBool = True And sendEventToRR = True Then
            ServicePBAP_Usable = True
            RaiseEvent BlueSoleil_Event_PBAP_ContactsListUsable()
        Else
            ServicePBAP_Usable = False
        End If

        'MsgBox("Done.  Connect = " & (pbapHandleConn <> 0) & "  Pull = " & pullTorF & ".  # of contacts = " & contactOffsets.Length)

        pbapHandleConn = 0

        RaiseEvent SetIndBlinkOff()

        PhoneBookSyncInProgress = False

        If File.Exists(MainPath & "PhoneBook\pb.vcf") = False Then
            RaiseEvent BlueSoleil_Event_ERROR_Return("SETVARFROMVAR;l_ReadingPhoneBook;!!! 'pb.vcf' file not exist !!!||POPUP;ReadingPhoneBook.SKIN;5", False, True)
            ToLog("pb.vcf file not found !!!")
        End If
        If File.Exists(MainPath & "PhoneBook\MobilePhone_PB.txt") Then
            RaiseEvent BlueSoleil_Event_ERROR_Return("SETVARFROMVAR;l_ReadingPhoneBook;!!! 'MobilePhone_PB.txt' file not exist !!!||POPUP;ReadingPhoneBook.SKIN;5", False, True)
            ToLog("MobilePhone_PB.txt file not found !!!")
        End If

    End Sub


    Public Sub GetInfoFromVcfFile(vcfFileName As String)

        Dim fileBook As String = UCase(Path.GetFileNameWithoutExtension(vcfFileName))
        Dim numberInNationalFormat As String = ""
        Dim vcfCardOffsets(0 To 0) As Long
        Dim vcfCardCount As Integer = 0

        vcfCardCount = VCard_GetContactOffsets(vcfFileName, vcfCardOffsets, "") ' This gets the number of entries, and the offset to each entry.

        Dim i As Integer
        Dim cardName As String = "", cardPhoneNumbers(0 To 0) As String, cardPhoneCount As Integer = 0, cardPhoneAddress(0 To 0) As String, cardPhoneLabels(0 To 0) As String, cardAddressLabels(0 To 0) As String, cardPhoneAddressCount As Integer, cardOrganisation As String = ""
        Dim cardEmail As String = ""
        Dim cardImage As Bitmap = Nothing
        Dim tempImage As Bitmap = Nothing
        Dim phoneBookList As String = ""
        Dim cardDateTime As String = ""
        Dim cardCallType As String = ""
        Dim cardBirthDay As String = ""
        Dim cardGeoPosition As String = ""
        Dim cardNote As String = ""

        hfpCallerIDnoFormatted = New PhoneNumberFormatter(TempPluginSettings.PhoneCountryCodes(0), TempPluginSettings.PhoneCountryCodes(1), TempPluginSettings.PhoneCountryCodes(2))

        'MessageBox.Show(fileBook)
        If fileBook = "PB" Then
            phoneBookList = MainPath & "PhoneBook\MobilePhone_" & fileBook & ".txt" 'Main Phone book
        ElseIf fileBook = "ICH" Then
            phoneBookList = MainPath & "PhoneBook\MobilePhone_" & fileBook & ".txt" 'Incoming Calls History
        ElseIf fileBook = "OCH" Then
            phoneBookList = MainPath & "PhoneBook\MobilePhone_" & fileBook & ".txt" 'Outgoing Calls History
        ElseIf fileBook = "MCH" Then
            phoneBookList = MainPath & "PhoneBook\MobilePhone_" & fileBook & ".txt" 'Missed Calls History
        ElseIf fileBook = "CCH" Then
            phoneBookList = MainPath & "PhoneBook\MobilePhone_" & fileBook & ".txt" 'Combined Calls History
        End If

        If File.Exists(phoneBookList) Then
            File.Delete(phoneBookList)
        End If

        Try
            For i = 0 To vcfCardCount - 1
                ' This gets the info for the contact in the VCF file at the offset specified.
                VCard_GetContactInfo(vcfFileName, vcfCardOffsets(i), cardName, cardEmail, cardPhoneNumbers, cardPhoneLabels, cardPhoneCount, cardImage, cardDateTime, cardCallType, cardPhoneAddress, cardAddressLabels, cardPhoneAddressCount, cardBirthDay, cardGeoPosition, cardNote, cardOrganisation)
                ' Do something with cardName, cardPhoneNumbers, cardPhoneTypes, and cardImage.
                ' Could do cardImage.save to save the image to a separate file.
                ' Could write out info to a text file.
                If cardPhoneCount > 0 Then 'If IsNothing(cardPhoneNumbers(0)) = False Then
                    If IsNothing(cardDateTime) = False And fileBook <> "PB" Then 'carDateTime format is 20151230T160624
                        'Dim en As New CultureInfo("en-US")
                        Dim en As CultureInfo = CultureInfo.InvariantCulture
                        Dim cDTSplit() As String = cardDateTime.Split(",")
                        Dim cDTSplit2() As String = cDTSplit(1).Split("T")
                        Dim cardDate As String = DateTime.ParseExact(cDTSplit2(0), "yyyyMMdd", en)
                        Dim cardTime As String = Regex.Replace(cDTSplit2(1), "\d{2}", "${0}:").TrimEnd(":")
                        cardDateTime = cDTSplit(0) & " at " & cardDate & " " & cardTime
                    Else
                        cardDateTime = ""
                    End If

                    'extract image if exist
                    If IsNothing(cardImage) = False And fileBook = "PB" Then
                        'If File.Exists(MainPath & "Photo\" & cardPhoneNumbers(0) & ".jpg") = False Or File.Exists(MainPath & "Photo\" & numberInNationalFormat & ".jpg") Then 'evite la sauvegarde si l'image existe déjà
                        tempImage = New Bitmap(cardImage)
                        'tempImage.Save(MainPath & "Photo\" & cardPhoneNumbers(0) & ".jpg", Imaging.ImageFormat.Jpeg)

                        For pic As Integer = 0 To cardPhoneCount - 1
                            'cardPhoneNumbers(pic) = BlueSoleil_BS_PBAP_CleanPhoneNumber(cardPhoneNumbers(pic)) 'remove characters: (,),-,. and space (now, the number has no extra characters and photo also)
                            'numberInNationalFormat = hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(cardPhoneNumbers(pic), OutputFormats.National)
                            'ToLog("phone original format is : " & cardPhoneNumbers(pic))
                            'ToLog("phone in national format is : " & numberInNationalFormat)
                            If File.Exists(MainPath & "Photo\" & cardPhoneNumbers(pic) & ".jpg") = False Then
                                tempImage.Save(MainPath & "Photo\" & cardPhoneNumbers(pic) & ".jpg", Imaging.ImageFormat.Jpeg)
                                ToLog("The photo '" & cardPhoneNumbers(pic) & ".jpg' is saved !")
                            End If
                            If File.Exists(MainPath & "Photo\" & hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(cardPhoneNumbers(pic), OutputFormats.National) & ".jpg") = False Then
                                tempImage.Save(MainPath & "Photo\" & hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(cardPhoneNumbers(pic), OutputFormats.National) & ".jpg", Imaging.ImageFormat.Jpeg)
                                ToLog("The photo '" & hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(cardPhoneNumbers(pic), OutputFormats.National) & ".jpg' is saved !")
                            End If

                            Dim TwoFirstNumbers As String = hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(cardPhoneNumbers(pic), OutputFormats.International)
                            'return international number with 00
                            If File.Exists(MainPath & "Photo\" & TwoFirstNumbers & ".jpg") = False Then
                                tempImage.Save(MainPath & "Photo\" & TwoFirstNumbers & ".jpg", Imaging.ImageFormat.Jpeg)
                                ToLog("The photo '" & TwoFirstNumbers & ".jpg' is saved !")
                            End If
                            'return international number with +
                            If TwoFirstNumbers.StartsWith("00") Then TwoFirstNumbers = "+" & TwoFirstNumbers.Substring(2)
                            If File.Exists(MainPath & "Photo\" & TwoFirstNumbers & ".jpg") = False Then
                                tempImage.Save(MainPath & "Photo\" & TwoFirstNumbers & ".jpg", Imaging.ImageFormat.Jpeg)
                                ToLog("The photo '" & TwoFirstNumbers & ".jpg' is saved !")
                            End If
                        Next
                        cardImage.Dispose()
                        'End If
                    End If

                    'the photo isn't added an iCarDS list
                    If fileBook = "PB" Then
                        If File.Exists(MainPath & "Photo\" & cardPhoneNumbers(0) & ".jpg") = True Then
                            AddCustomList(phoneBookList, cardPhoneNumbers(0), cardName, MainPath & "Photo\" & cardPhoneNumbers(0) & ".jpg")
                        Else
                            AddCustomList(phoneBookList, cardPhoneNumbers(0), cardName, MainPath & "Photo\unknow.gif")
                        End If
                    Else
                        'added in case no name and no datetime
                        If cardName = "" Then cardName = cardPhoneNumbers(0)
                        'MessageBox.Show(cardName)
                        If cardDateTime = "" Then
                            Select Case fileBook
                                Case "ICH"
                                    cardDateTime = "RECEIVED at unknow date time"
                                Case "OCH"
                                    cardDateTime = "DIALED at unknow date time"
                                Case "MCH"
                                    cardDateTime = "MISSED at unknow date time"
                                Case "CCH"
                                    cardDateTime = cardCallType & " at unknow date time"
                            End Select
                        End If

                        If File.Exists(MainPath & "Photo\" & cardPhoneNumbers(0) & ".jpg") = True Then
                            AddCustomList(phoneBookList, cardPhoneNumbers(0), cardName & " " & cardDateTime, MainPath & "Photo\" & cardPhoneNumbers(0) & ".jpg")
                        Else
                            AddCustomList(phoneBookList, cardPhoneNumbers(0), cardName & " " & cardDateTime, MainPath & "Photo\unknow.gif")
                        End If

                    End If

                Else
                    Continue For
                End If

            Next i

            GetInfoFromVcfFile2(fileBook)

        Catch ex As Exception
            ' Throws a new exception.
            'Throw New System.Exception("An exception has occurred.")
            'MessageBox.Show(ex.Message)
            ToLog("GetInfoFromVcfFile error: " & ex.Message)
        End Try

    End Sub

    Public Sub GetInfoFromVcfFile2(phonebookType As String)

        Try
            Dim vcfCardCount As Integer = 0
            Dim spLine() As String
            Dim phonebookFilter As String = ""
            Dim cardName As String = ""
            Dim cardPhone As String = ""
            Dim cardDateTime As String = ""
            Dim cardImage As String = ""
            Dim sPath As String = MainPath & "PhoneBook\MobilePhone_" & phonebookType & ".txt"
            Dim allLines As String() = IO.File.ReadAllLines(sPath, Encoding.Default)

            Dim PhoneBookArray As New ArrayList()

            PhoneBookArray.Clear()

            If PluginRunForDS = False Then
                For line As Integer = 1 To allLines.Length - 1 Step 2 'contact on two lines
                    If allLines(line).Contains("RECEIVED") Then
                        phonebookFilter = "RECEIVED"
                    ElseIf allLines(line).Contains("DIALED") Then
                        phonebookFilter = "DIALED"
                    ElseIf allLines(line).Contains("MISSED") Then
                        phonebookFilter = "MISSED"
                    End If
                    allLines(line) = allLines(line).Replace("LST", "")
                    allLines(line + 1) = allLines(line + 1).Replace("ICO", "##")
                    PhoneBookArray.Add(allLines(line).Replace("||", "##").Replace(" " & phonebookFilter & " ", "##" & phonebookFilter & "##") & allLines(line + 1))
                    ToLog("RR --> " & allLines(line).Replace("||", "##").Replace(" " & phonebookFilter & " ", "##" & phonebookFilter & "##") & allLines(line + 1))
                Next
            Else
                For line As Integer = 1 To allLines.Length - 1 'contact on one line
                    If allLines(line).Contains("RECEIVED") Then
                        phonebookFilter = "RECEIVED"
                    ElseIf allLines(line).Contains("DIALED") Then
                        phonebookFilter = "DIALED"
                    ElseIf allLines(line).Contains("MISSED") Then
                        phonebookFilter = "MISSED"
                    End If
                    allLines(line) = allLines(line).Replace("LST", "")
                    PhoneBookArray.Add(allLines(line).Replace("||", "##").Replace(" " & phonebookFilter & " ", "##" & phonebookFilter & "##"))
                    ToLog("DS --> " & allLines(line).Replace("||", "##").Replace(" " & phonebookFilter & " ", "##" & phonebookFilter & "##"))
                Next
            End If

            ToLog(PhoneBookArray.Count.ToString & " found for the list " & phonebookType)
            vcfCardCount = PhoneBookArray.Count

            If File.Exists(MainPath & "PhoneBook\MobilePhone_" & phonebookType & "2.txt") = True Then
                File.Delete(MainPath & "PhoneBook\MobilePhone_" & phonebookType & "2.txt")
            End If

            'make the container
            Dim counts As New Dictionary(Of String, Integer), retName() As String

            For p As Integer = 0 To PhoneBookArray.Count - 1
                retName = Split(PhoneBookArray.Item(p), "##")
                'ToLog("in PhoneBookArray: " & retName(1))
                If Not counts.ContainsKey(retName(1)) Then
                    counts.Add(retName(1), 1)
                Else
                    counts.Item(retName(1)) = counts.Item(retName(1)) + 1
                End If
            Next

            For Each kvp As KeyValuePair(Of String, Integer) In counts
                'ToLog("Name: " & kvp.Key & ", Count: " & kvp.Value)
                For p As Integer = 0 To PhoneBookArray.Count - 1
                    spLine = Split(PhoneBookArray.Item(p), "##")
                    If spLine(1) = kvp.Key Then
                        cardName = spLine(1)
                        cardPhone = spLine(0)
                        cardDateTime = spLine(3).Replace("at", "at Last")
                        ToLog("Name: " & kvp.Key & ", Count: " & kvp.Value & ", Number: " & spLine(0))
                        If spLine.Length = 5 Then cardImage = spLine(4) Else cardImage = ""
                        If File.Exists(cardImage) = True Then
                            AddCustomList(MainPath & "PhoneBook\MobilePhone_" & phonebookType & "2.txt", cardPhone, cardName & " (" & kvp.Value & ") " & cardDateTime, cardImage)
                        Else
                            AddCustomList(MainPath & "PhoneBook\MobilePhone_" & phonebookType & "2.txt", cardPhone, cardName & " (" & kvp.Value & ") " & cardDateTime, MainPath & "Photo\unknow.gif")
                        End If
                        Exit For
                    End If
                Next
            Next

            PhoneBookArray.Clear()
            counts.Clear()

            ToLog("MobilePhone_" & phonebookType & "2.txt file is build")
        Catch ex As Exception
            ToLog("GetInfoFromVcfFile2 error: " & ex.Message)
        End Try

    End Sub
    Public Function GetStringBetween(ByVal Haystack As String, ByVal StartSearch As String, ByVal EndSearch As String) As String
        GetStringBetween = ""
        If InStr(Haystack, StartSearch) < 1 Then Return False
        Dim rx As New Regex(StartSearch & "(.+?)" & EndSearch)
        Dim m As Match = rx.Match(Haystack)
        If m.Success Then
            Return m.Groups(1).ToString()
        End If
    End Function



    Public Sub Bluesoleil_LoadContactsFile()

        'On Error Resume Next 'solve the issue if a contact has more than one phone and no address
        If File.Exists(MainPath & "PhoneBook\pb.vcf") = True Then
            Dim vcfFileName As String = MainPath & "PhoneBook\pb.vcf"
            If File.Exists(vcfFileName) = False Then Exit Sub

            Dim vcfCardOffsets(0 To 0) As Long
            Dim vcfCardCount As Integer = 0


            vcfCardCount = VCard_GetContactOffsets(vcfFileName, vcfCardOffsets, "")

            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim k As Integer = 0

            If PhoneBookEntryCount <> vcfCardCount Then
                PhoneBookEntryCount = vcfCardCount
            End If

            For i = 0 To vcfCardCount - 1

                Dim cardFullName As String = "", cardPhoneNumbers(0 To 0) As String, cardPhoneLabels(0 To 0) As String, cardPhoneCount As Integer = 0, cardPhoneAddress(0 To 0) As String, cardAddressLabels(0 To 0) As String, cardPhoneAddressCount As Integer, cardOrganisation As String = ""
                Dim cardEmail As String = ""
                Dim cardImage As Bitmap = Nothing
                Dim tempImage As Bitmap = Nothing
                Dim cardDateTime As String = ""
                'Dim cardLastCallType As String = ""
                Dim cardBirthDay As String = ""
                Dim cardGeoPosition As String = ""
                Dim cardNote As String = ""
                Dim tmpcardPhoneaddress() As String

                Dim retName As String = ""
                Dim retNamePrefix As String = ""
                Dim retNameSuffix As String = ""

                Dim en As CultureInfo = CultureInfo.InvariantCulture

                VCard_GetContactInfo(vcfFileName, vcfCardOffsets(i), cardFullName, cardEmail, cardPhoneNumbers, cardPhoneLabels, cardPhoneCount, tempImage, cardDateTime, "", cardPhoneAddress, cardAddressLabels, cardPhoneAddressCount, cardBirthDay, cardGeoPosition, cardNote, cardOrganisation)

                If cardPhoneCount > 0 Then 'check only contact with phone number
                    If IsNothing(tempImage) = False Then
                        Try
                            cardImage = New Bitmap(tempImage)
                        Catch ex As Exception

                        End Try
                        tempImage.Dispose()
                        tempImage = Nothing
                    Else
                        cardImage = Nothing
                    End If

                    ReDim Preserve PhoneBookEntries(0 To i)
                    If cardFullName <> "" Then
                        VCard_NameParser(cardFullName, retName, retNamePrefix, retNameSuffix)
                        PhoneBookEntries(i).EntryFullName = cardFullName
                        PhoneBookEntries(i).EntryName = retName
                        If retNamePrefix <> "" Then
                            PhoneBookEntries(i).EntryNamePrefix = retNamePrefix
                        End If
                        If retNameSuffix <> "" Then
                            PhoneBookEntries(i).EntryNameSuffix = retNameSuffix
                        End If
                    End If
                    If cardImage IsNot Nothing Then
                        PhoneBookEntries(i).EntryImage = cardImage
                        'PhoneBookEntries(i).EntryImage = MainPath & "Photo\" & PhoneBookEntries(i).EntryPhoneNumbers(0).EntryPhoneNumber & ".jpg"
                    End If
                    PhoneBookEntries(i).EntryPhoneNumberCount = cardPhoneCount
                    If cardEmail <> "" Then
                        PhoneBookEntries(i).EntryEmail = cardEmail
                    End If
                    If cardBirthDay <> "" Then
                        If cardBirthDay.Contains("-") Then cardBirthDay = cardBirthDay.Replace("-", "")
                        PhoneBookEntries(i).EntryBirthDay = DateTime.ParseExact(cardBirthDay, "yyyyMMdd", en)
                    End If
                    If cardGeoPosition <> "" Then
                        PhoneBookEntries(i).EntryGeoPosition = cardGeoPosition
                    End If
                    If cardNote <> "" Then
                        PhoneBookEntries(i).EntryNote = cardNote
                    End If
                    If cardOrganisation <> "" Then
                        PhoneBookEntries(i).EntryOrganisation = cardOrganisation
                    End If

                    ReDim PhoneBookEntries(i).EntryPhoneNumbers(0 To cardPhoneCount - 1)
                    For j = 0 To cardPhoneCount - 1
                        PhoneBookEntries(i).EntryPhoneNumbers(j).EntryPhoneNumber = BlueSoleil_BS_PBAP_CleanPhoneNumber(cardPhoneNumbers(j))
                        PhoneBookEntries(i).EntryPhoneNumbers(j).Location = cardPhoneLabels(j)
                    Next


                    ReDim PhoneBookEntries(i).EntryAddress(0 To cardPhoneAddressCount - 1)
                    PhoneBookEntries(i).EntryPhoneAddressCount = cardPhoneAddressCount
                    For k = 0 To cardPhoneAddressCount - 1
                        If cardPhoneAddress(k) <> "" Then
                            'ToLog(cardPhoneAddress(k))
                            'retPObox & "|" & retExtAddr & "|" & retStreetAddr & "|" & retLocalityCity & "|" & retRegionState & "|" & retPostalCode & "|" & retCountry
                            tmpcardPhoneaddress = cardPhoneAddress(k).Split("|")
                            PhoneBookEntries(i).EntryAddress(k).EntryPOBox = tmpcardPhoneaddress(0)
                            PhoneBookEntries(i).EntryAddress(k).EntryExtAddress = tmpcardPhoneaddress(1)
                            PhoneBookEntries(i).EntryAddress(k).EntryStreetAddress = tmpcardPhoneaddress(2)
                            PhoneBookEntries(i).EntryAddress(k).EntryLocalityCity = tmpcardPhoneaddress(3)
                            PhoneBookEntries(i).EntryAddress(k).EntryRegionState = tmpcardPhoneaddress(4)
                            PhoneBookEntries(i).EntryAddress(k).EntryPostalCode = tmpcardPhoneaddress(5)
                            PhoneBookEntries(i).EntryAddress(k).EntryCountry = tmpcardPhoneaddress(6)
                            PhoneBookEntries(i).EntryAddress(k).EntryFullAddress = cardPhoneAddress(k)
                            PhoneBookEntries(i).EntryAddress(k).Location = cardAddressLabels(k)
                        Else
                            PhoneBookEntries(i).EntryAddress(k).EntryPOBox = ""
                            PhoneBookEntries(i).EntryAddress(k).EntryExtAddress = ""
                            PhoneBookEntries(i).EntryAddress(k).EntryStreetAddress = ""
                            PhoneBookEntries(i).EntryAddress(k).EntryLocalityCity = ""
                            PhoneBookEntries(i).EntryAddress(k).EntryRegionState = ""
                            PhoneBookEntries(i).EntryAddress(k).EntryPostalCode = ""
                            PhoneBookEntries(i).EntryAddress(k).EntryCountry = ""
                            PhoneBookEntries(i).EntryAddress(k).EntryFullAddress = ""
                            PhoneBookEntries(i).EntryAddress(k).Location = ""
                        End If
                    Next
                    'PhoneBookEntryCount = PhoneBookEntryCount + 1
                End If
            Next i

        Else
            ToLog("Phone book PB isn't found !")
            RaiseEvent BlueSoleil_Event_ERROR_Return("Main phone book isn't found", False, False)
        End If

    End Sub

    Private Sub VCard_NameParser(ByVal inpFullName As String, ByRef retFullName As String, ByRef retNamePrefix As String, ByRef retNameSuffix As String)
        Try
            'this subroutine takes the full name, "Mr. Robert Downey, Jr." for example, and parses out the first, last, additional, prefix, and suffix.
            retFullName = ""
            retNamePrefix = ""
            retNameSuffix = ""
            Dim prefixArray() As String = {"MR", "MR.", "MS", "MS.", "MRS", "MRS.", "DR", "DR.", "MME", "MME.", "MLLE", "MLLE."} 'duke, earl, lord, sir, etc.
            Dim suffixArray() As String = {"JR", "JR.", "SR", "SR.", "II", "III", "IV"}

            'get prefix and suffix if available, and remove them from the words.
            For Each p In prefixArray
                If p = UCase(Left(inpFullName, p.Length)) Then
                    'MessageBox.Show(p)
                    retNamePrefix = Left(inpFullName, p.Length)
                    retFullName = LTrim(inpFullName.Replace(retNamePrefix, ""))
                    Exit For
                End If
            Next

            For Each q In suffixArray
                If q = UCase(Right(inpFullName, q.Length)) Then
                    'MessageBox.Show(q)
                    retNameSuffix = Right(inpFullName, q.Length)
                    retFullName = RTrim(retFullName.Replace(retNameSuffix, "").Replace(",", ""))
                    Exit For
                End If
            Next

            If retFullName = "" Then retFullName = inpFullName
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Public Sub BlueSoleil_PBAP_SortvCardbyName()
        Try
            timer1.Enabled = False
            If File.Exists(MainPath & "PhoneBook\pb.vcf") = True Then
                PhoneBookSyncInProgress = True
                Thread.Sleep(1000)
                VCard_ResortFile_ByName(MainPath & "PhoneBook\pb.vcf", MainPath & "PhoneBook\pb_sorted.vcf")
                Thread.Sleep(1000)
                File.Replace(MainPath & "PhoneBook\pb_sorted.vcf", MainPath & "PhoneBook\pb.vcf", Nothing)
                Thread.Sleep(1000)
                GetInfoFromVcfFile(MainPath & "PhoneBook\pb.vcf")
                Thread.Sleep(1000)
                PhoneBookSyncInProgress = True
                ToLog("Phone book is now in good order !")
                If PluginRunForDS = False Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONE_PHONEBOOKISSORTED||mobilephone_phonebookload;pb", False, False)
                Else
                    RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONE_PHONEBOOKISSORTED||mobilephone_phonebookload;pb", False, False)
                End If
            Else
                ToLog("Phone book PB isn't found !")
                RaiseEvent BlueSoleil_Event_ERROR_Return("Main phone book isn't found", True, False)
            End If
            timer1.Enabled = True
        Catch ex As Exception
            MsgBox("Sub PhoneBook Sort - " & ex.Message)
        End Try
    End Sub



    Public Function BlueSoleil_PBAP_NumberExistAsVcard(ByVal inpPhoneNo As String) As Boolean
        'inpPhoneNo = Replace(inpPhoneNo, "-", "")
        'inpPhoneNo = Replace(inpPhoneNo, ".", "")
        'inpPhoneNo = Replace(inpPhoneNo, " ", "")
        'inpPhoneNo = Replace(inpPhoneNo, "(", "")
        'inpPhoneNo = Replace(inpPhoneNo, ")", "")
        'inpPhoneNo = Replace(inpPhoneNo, "+", "")
        Dim retStr As Boolean = False
        Dim testNo As String = ""
        Dim i As Integer, j As Integer
        For i = 0 To PhoneBookEntryCount - 1
            For j = 0 To PhoneBookEntries(i).EntryPhoneNumberCount - 1
                testNo = PhoneBookEntries(i).EntryPhoneNumbers(j).EntryPhoneNumber
                'testNo = Replace(testNo, "-", "")
                'testNo = Replace(testNo, ".", "")
                'testNo = Replace(testNo, " ", "")
                'testNo = Replace(testNo, "(", "")
                'testNo = Replace(testNo, ")", "")
                'testNo = Replace(testNo, "+", "")
                If testNo = inpPhoneNo Then 'Or
                    '    testNo = hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(inpPhoneNo, OutputFormats.National) Or
                    '    testNo = "+" & hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(inpPhoneNo, OutputFormats.International).Substring(2) Then
                    retStr = True
                    Exit For
                End If
            Next j
            If retStr <> False Then Exit For
        Next i
        Return retStr
    End Function
    Public Function BlueSoleil_PBAP_GetVcardId(ByVal inpPhoneNo As String) As Integer
        'inpPhoneNo = Replace(inpPhoneNo, "-", "")
        'inpPhoneNo = Replace(inpPhoneNo, ".", "")
        'inpPhoneNo = Replace(inpPhoneNo, " ", "")
        'inpPhoneNo = Replace(inpPhoneNo, "(", "")
        'inpPhoneNo = Replace(inpPhoneNo, ")", "")
        'inpPhoneNo = Replace(inpPhoneNo, "+", "")
        Dim retStr As Integer = -2
        Dim testNo As String = ""
        Dim i As Integer, j As Integer
        For i = 0 To PhoneBookEntryCount - 1
            For j = 0 To PhoneBookEntries(i).EntryPhoneNumberCount - 1
                testNo = PhoneBookEntries(i).EntryPhoneNumbers(j).EntryPhoneNumber
                'testNo = Replace(testNo, "-", "")
                'testNo = Replace(testNo, ".", "")
                'testNo = Replace(testNo, " ", "")
                'testNo = Replace(testNo, "(", "")
                'testNo = Replace(testNo, ")", "")
                'testNo = Replace(testNo, "+", "")
                'If testNo = inpPhoneNo Then
                If testNo = inpPhoneNo Then 'Or
                    'testNo = hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(inpPhoneNo, OutputFormats.National) Or
                    'testNo = "+" & hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(inpPhoneNo, OutputFormats.International).Substring(2) Then
                    retStr = i
                    Exit For
                End If
            Next j
        Next i
        Return retStr
    End Function
    Public Function BlueSoleil_PBAP_GetNameFromNumber(ByVal inpPhoneNo As String) As String
        'inpPhoneNo = Replace(inpPhoneNo, "-", "")
        'inpPhoneNo = Replace(inpPhoneNo, ".", "")
        'inpPhoneNo = Replace(inpPhoneNo, " ", "")
        'inpPhoneNo = Replace(inpPhoneNo, "(", "")
        'inpPhoneNo = Replace(inpPhoneNo, ")", "")
        'inpPhoneNo = Replace(inpPhoneNo, "+", "")
        Dim retStr As String = ""
        Dim testNo As String = ""
        Dim i As Integer, j As Integer
        For i = 0 To PhoneBookEntryCount - 1
            For j = 0 To PhoneBookEntries(i).EntryPhoneNumberCount - 1
                testNo = PhoneBookEntries(i).EntryPhoneNumbers(j).EntryPhoneNumber
                'testNo = Replace(testNo, "-", "")
                'testNo = Replace(testNo, ".", "")
                'testNo = Replace(testNo, " ", "")
                'testNo = Replace(testNo, "(", "")
                'testNo = Replace(testNo, ")", "")
                'testNo = Replace(testNo, "+", "")
                If testNo = inpPhoneNo Then 'Or
                    'testNo = hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(inpPhoneNo, OutputFormats.National) Or
                    'testNo = "+" & hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber(inpPhoneNo, OutputFormats.International).Substring(2) Then
                    retStr = PhoneBookEntries(i).EntryName
                    Exit For
                End If
            Next j
            If retStr <> "" Then Exit For
        Next i

        Return retStr

    End Function

    Public Function BlueSoleil_PBAP_GetNumberFromName(ByVal inpName As String) As String

        'inpName = Replace(inpName, "-", "")
        'inpName = Replace(inpName, ".", "")
        'inpName = Replace(inpName, " ", "")
        'inpName = Replace(inpName, "(", "")
        'inpName = Replace(inpName, ")", "")
        'inpName = Replace(inpName, "+", "")

        Dim retStr As String = ""

        Dim testName As String = ""

        Dim i As Integer
        For i = 0 To PhoneBookEntryCount - 1

            testName = PhoneBookEntries(i).EntryName
            'testName = Replace(testName, "-", "")
            'testName = Replace(testName, ".", "")
            'testName = Replace(testName, " ", "")
            'testName = Replace(testName, "(", "")
            'testName = Replace(testName, ")", "")
            'testName = Replace(testName, "+", "")

            If testName = inpName Then
                retStr = PhoneBookEntries(i).EntryPhoneNumbers(0).EntryPhoneNumber
                Exit For
            End If

            If retStr <> "" Then Exit For
        Next i

        If retStr = "" Then
            Dim p1 As Integer = InStr(1, inpName, "<")
            If p1 > 1 Then
                inpName = Trim(Strings.Left(inpName, p1 - 1))
                retStr = BlueSoleil_PBAP_GetNumberFromName(inpName)
            End If
        End If

        Return retStr

    End Function

    Public Function BlueSoleil_BS_PBAP_GetImageFromName(ByVal inpName As String) As String 'As Bitmap

        'inpName = Replace(inpName, "-", "")
        'inpName = Replace(inpName, ".", "")
        'inpName = Replace(inpName, " ", "")
        'inpName = Replace(inpName, "(", "")
        'inpName = Replace(inpName, ")", "")
        'inpName = Replace(inpName, "+", "")

        'Dim retImg As Bitmap = Nothing
        Dim retImg As String = ""
        Dim testName As String = ""

        Dim i As Integer
        For i = 0 To PhoneBookEntryCount - 1

            testName = PhoneBookEntries(i).EntryName
            'testName = Replace(testName, "-", "")
            'testName = Replace(testName, ".", "")
            'testName = Replace(testName, " ", "")
            'testName = Replace(testName, "(", "")
            'testName = Replace(testName, ")", "")
            'testName = Replace(testName, "+", "")

            If testName = inpName Then
                'retImg = PhoneBookEntries(i).EntryImage
                retImg = PhoneBookEntries(i).EntryPhoneNumbers(0).EntryPhoneNumber & ".jpg"
                Exit For
            Else
                Continue For 'continue loop if nothing found
            End If

            If IsNothing(retImg) = False Then Exit For
        Next i

        'If IsNothing(retImg) = True Then
        '    Dim p1 As Integer = InStr(1, inpName, "<")
        '    If p1 > 1 Then
        '        inpName = Trim(Strings.Left(inpName, p1 - 1))
        '        retImg = BlueSoleil_PBAP_GetImageFromName(inpName)
        '    End If
        'End If

        Return retImg

    End Function

    Public Function BlueSoleil_BS_PBAP_GetImageFromNumber(ByVal inpPhoneNo As String) As String
        'A TESTER (conversion format national <--> international
        'hfpCallerIDnoFormatted = New PhoneNumberFormatter(TempPluginSettings.PhoneCountryCodes(0), TempPluginSettings.PhoneCountryCodes(1), TempPluginSettings.PhoneCountryCodes(2))
        'hfpCallerIDnoFormatted = New PhoneNumberFormatter("0", "33", "00")
        'MessageBox.Show(hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber("0603363212", OutputFormats.International))
        'MessageBox.Show(hfpCallerIDnoFormatted.ConvertToCallablePhoneNumber("+33603363212", OutputFormats.National))
        'MessageBox.Show(hfpCallerIDnoFormatted.ConvertToUnformatedPhoneNumber("+33603363212"))
        'A TESTER

        'inpPhoneNo = Replace(inpPhoneNo, "-", "")
        'inpPhoneNo = Replace(inpPhoneNo, ".", "")
        'inpPhoneNo = Replace(inpPhoneNo, " ", "")
        'inpPhoneNo = Replace(inpPhoneNo, "(", "")
        'inpPhoneNo = Replace(inpPhoneNo, ")", "")
        'inpPhoneNo = Replace(inpPhoneNo, "+", "") ' keep + for +33102030405.jpg file type
        Dim retImg As String = ""
        Dim testNo As String = ""
        Dim i As Integer, j As Integer
        For i = 0 To PhoneBookEntryCount - 1

            For j = 0 To PhoneBookEntries(i).EntryPhoneNumberCount - 1
                testNo = PhoneBookEntries(i).EntryPhoneNumbers(j).EntryPhoneNumber
                'testNo = Replace(testNo, "-", "")
                'testNo = Replace(testNo, ".", "")
                'testNo = Replace(testNo, " ", "")
                'testNo = Replace(testNo, "(", "")
                'testNo = Replace(testNo, ")", "")
                'testNo = Replace(testNo, "+", "") ' keep + for +33102030405.jpg file type

                If testNo = inpPhoneNo Then
                    retImg = MainPath & "Photo\" & inpPhoneNo & ".jpg"
                    'retImg = MainPath & PhoneBookEntries(i).EntryPhoneNumbers(0).EntryPhoneNumber & ".jpg"
                    Exit For
                Else
                    Continue For 'continue loop if nothing found
                End If
            Next j
            If IsNothing(retImg) = False Then Exit For
        Next i

        Return retImg

    End Function

    Public Function BlueSoleil_BS_PBAP_CleanPhoneNumber(ByVal inpPhoneNo As String) As String
        Dim retnewNumber As String = ""
        inpPhoneNo = Replace(inpPhoneNo, "-", "")
        inpPhoneNo = Replace(inpPhoneNo, ".", "")
        inpPhoneNo = Replace(inpPhoneNo, " ", "")
        inpPhoneNo = Replace(inpPhoneNo, "(", "")
        inpPhoneNo = Replace(inpPhoneNo, ")", "")
        'inpPhoneNo = Replace(inpPhoneNo, "+", "") ' keep + for +33102030405.jpg file type

        retnewNumber = inpPhoneNo
        Return retnewNumber
    End Function

#End Region

#Region "MAP service"

    Public Sub btnMsgConnect()
        Try
            If BlueSoleil_IsInstalled() = False Then Exit Sub
            If BlueSoleil_IsBluetoothReady() = False Then Exit Sub

            Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text

            'mapHandleMNSsvc = BlueSoleil_MAP_RegisterNotificationService() 'create an Unknown service BS message box

            Dim connTorF As Boolean = False
            connTorF = BlueSoleil_ConnectService_ByName("MAP", phoneDvcName, mapHandleDvc, mapHandleConn, mapHandleSvc)


            If (mapHandleConn <> 0) = True Then
                ServiceMAP_Usable = True
                'BlueSoleil_MAP_EnableNotifications(mapHandleConn, True) 'create an Unknown service BS popup
                mapIsActive = True
                RaiseEvent BlueSoleil_Event_ERROR_Return("MAP connection is OK", False, False)
                If PluginRunForDS = False Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONEMAPISREADY", False, True)
                Else
                    RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONEMAPISREADY", False, True)
                End If
                ToLog("MAP connection is OK")
            Else
                mapIsActive = False
                'BlueSoleil_MAP_UnregisterServers(mapHandleMNSsvc, 0)
                RaiseEvent BlueSoleil_Event_ERROR_Return("MAP connection isn't possible !", False, False)
                ToLog("MAP connection isn't possible !")
            End If
        Catch ex As Exception
            mapIsActive = False
            'BlueSoleil_MAP_UnregisterServers(mapHandleMNSsvc, 0)
            RaiseEvent BlueSoleil_Event_ERROR_Return("MAP connection isn't ready !", False, False)
            ToLog("MAP connection isn't ready !")
        End Try
        'MsgBox("Done.  Return = " & (mapHandleConn <> 0) & ".  If you receive a text message, a notification *should* pop up.")
    End Sub

    Public Sub btnMsgDisconnect()
        Try
            'If mapHandleConn = 0 Then
            '    Exit Sub
            'End If
            If BlueSoleil_IsInstalled() = False Then Exit Sub
            If BlueSoleil_IsSDKinitialized() = False Then Exit Sub
            If BlueSoleil_IsBluetoothReady() = False Then Exit Sub

            'If mapHandleConn <> 0 Then
            'If BlueSoleil_IsServerConnected() = True Then
            BlueSoleil_DisconnectServiceConn(mapHandleConn)
            'BlueSoleil_DisconnectServiceConn(mapHandleSvc)
            'End If
            ' End If

            'BlueSoleil_MAP_EnableNotifications(mapHandleConn, False)
            BlueSoleil_DisconnectServiceConn(mapHandleConn)
            mapHandleConn = 0
            BlueSoleil_MAP_UnregisterServers(mapHandleMNSsvc, 0)


            mapHandleSvc = 0
            mapHandleDvc = 0

            RaiseEvent BlueSoleil_Event_ERROR_Return("MAP connection is disconnected !", False, False)
            ToLog("MAP connection is disconnected !")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        'MsgBox("Done.")
    End Sub

    Public Sub btnMsgReConnect()
        btnMsgDisconnect()
        btnMsgConnect()
    End Sub

    ''' <summary>
    ''' Extract Messages
    ''' </summary>
    ''' <param name="unread"></param>
    ''' <remarks></remarks>
    Public Sub btnGetMessages(ByRef unreadOnly As Boolean, Optional ByRef downloadAttachment As Boolean = False)

        Dim writeToPath As String = MainPath ' My.Application.Info.DirectoryPath
        If Strings.Right(writeToPath, 1) <> "\" Then writeToPath = writeToPath & "\"
        writeToPath = writeToPath & "Messages\Inbox"

        If IO.Directory.Exists(writeToPath) = False Then
            Try
                IO.Directory.CreateDirectory(writeToPath)
            Catch ex As Exception

            End Try
        End If

        Dim flTorF As Boolean = False, arrayFolders(0 To 0) As String, folderCount As Integer = 0
        flTorF = BlueSoleil_MAP_PullFolderList(mapHandleConn, writeToPath & "\FolderList.xml") 'define folder's position
        If flTorF = True Then
            folderCount = BlueSoleil_MAP_XML_GetFolderListInfo(writeToPath & "\FolderList.xml", arrayFolders) 'folder's list
        End If

        If unreadOnly = False Then
            'MsgBox("The program will be unresponsive for a minute or more.  Click OK to begin.")
            RaiseEvent BlueSoleil_Event_ERROR_Return("SETVARFROMVAR;MOBILEPHONE_SMSINFO;l_set_BTM_AllSmSInfo", False, True)
            ToLog("Extract All SMS is running !")
        Else

            RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_CHECKIFSMSREAD", False, True)

            ToLog("Extract Unread SMS is running !")
        End If

        Dim mlTorF As Boolean = False
        mlTorF = BlueSoleil_MAP_PullMessageList(mapHandleConn, "inbox", writeToPath & "\MessageList.xml", unreadOnly)

        Dim i As Integer = 0 ', NumberOfMsg As Integer = 0

        Dim msgInfo As String = ""
        Dim msgTorF As Boolean = False
        Dim msgFileName As String = ""
        Dim msgText As String = "", msgFromName As String = "", msgFromNo As String = "", msgType As String = "", msgStatus As String = "", msgFolder As String = "", msgAttachOriginalFN As String = ""
        Dim msgHandles(0 To 0) As String, msgSubjects(0 To 0) As String, msgDateTimes(0 To 0) As DateTime, msgSenderNames(0 To 0) As String, msgSenderAddresses(0 To 0) As String, msgRecipAddresses(0 To 0) As String, msgTypes(0 To 0) As String, msgSizes(0 To 0) As Integer, msgAttachmentSizes(0 To 0) As Integer, msgReadStates(0 To 0) As Boolean
        Dim msgAttachBytes(0 To 0) As Byte, msgAttachType As String = "", msgAttachSize As Long = 0


        If mlTorF = True Then
            MsgNumberOfNewUnreadOld = BlueSoleil_MAP_XML_GetMessageListInfo(writeToPath & "\MessageList.xml", msgHandles, msgSubjects, msgDateTimes, msgSenderNames, msgSenderAddresses, msgRecipAddresses, msgTypes, msgSizes, msgAttachmentSizes, msgReadStates)

            If MsgNumberOfNewUnreadOld > 0 Then

                map_MsgHistoryCount = MsgNumberOfNewUnreadOld 'fileHandles.Count
                'ReDim Preserve map_MsgHistoryItems(0 To map_MsgHistoryCount)
                ReDim map_MsgHistoryItems(0 To map_MsgHistoryCount)

                'fill the sctructure map_MsgHistoryItems with the arrays from the MessageList.
                For i = 0 To MsgNumberOfNewUnreadOld - 1

                    map_MsgHistoryItems(i).msgReadState = msgReadStates(i).ToString
                    map_MsgHistoryItems(i).msgHandle = msgHandles(i)
                    map_MsgHistoryItems(i).msgDateTime = msgDateTimes(i)
                    map_MsgHistoryItems(i).msgAttachmentSizes = msgAttachmentSizes(i)

                    'get some info from all .BMSG files and not .xml file, 
                    msgFileName = writeToPath & "\" & msgHandles(i) & ".BMSG"
                    Try
                        If IO.File.Exists(msgFileName) = True Then
                            IO.File.Delete(msgFileName)
                        End If
                    Catch ex As Exception
                        ToLog("Map error on delete BMSG file: " & ex.Message)
                    End Try

                    Try
                        'by default, attachment will don't extracted (downloadAttachment = False) !
                        'msgTorF = BlueSoleil_MAP_PullMessage(mapHandleConn, msgHandles(i), msgFileName, downloadAttachment)
                        'MULTIPART is really only supposed to be for messages where reception_status (from msg list) does not equal COMPLETE.  But we are testing it.
                        msgTorF = BlueSoleil_MAP_PullMessage_MultiPart(mapHandleConn, msgHandles(i), msgFileName, downloadAttachment)

                        If msgTorF = True Then
                            BMSG_GetMessageInfo(msgFileName, msgText, msgFromName, msgFromNo, msgType, msgStatus, msgFolder, msgAttachBytes, msgAttachType, msgAttachOriginalFN)
                            msgAttachSize = msgAttachBytes.Length
                            If msgAttachType = "" Then msgAttachSize = 0

                            'ajout 11-12-2016
                            If msgAttachSize > 0 And msgAttachType <> "" Then

                                Dim slashPos As Integer = InStrRev(msgAttachType, "/")
                                Dim testXtn As String = Strings.Mid(msgAttachType, slashPos + 1)

                                Dim saveAttachmentDLG As New SaveFileDialog
                                saveAttachmentDLG.FileName = "attachment." & testXtn

                                If msgAttachOriginalFN <> "" Then
                                    saveAttachmentDLG.FileName = msgAttachOriginalFN
                                    map_MsgHistoryItems(i).msgAttachmentName = saveAttachmentDLG.FileName
                                End If

                                'MessageBox.Show(msgAttachOriginalFN)
                                saveAttachmentDLG.Filter = "All Files (*.*)|*.*"
                                saveAttachmentDLG.DefaultExt = testXtn
                                saveAttachmentDLG.Title = "Save Attachment?  File is of type " & msgAttachType
                                Dim dlgResult As DialogResult = saveAttachmentDLG.ShowDialog()
                                If dlgResult = DialogResult.OK Or dlgResult = DialogResult.Yes Then
                                    Try
                                        'delete existing file.
                                        IO.File.Delete(saveAttachmentDLG.FileName)
                                    Catch ex As Exception

                                    End Try

                                    Try
                                        'write new file.
                                        IO.File.WriteAllBytes(saveAttachmentDLG.FileName, msgAttachBytes)
                                    Catch ex As Exception

                                    End Try

                                End If

                            End If
                            'ajout 11-12-2016
                            msgInfo = "Message =" & vbNewLine & "From: " & msgFromNo & "  " & msgFromName & vbNewLine & "Msg Subj: " & msgSubjects(0) & vbNewLine & "Msg Text: " & msgText & vbNewLine & "Type: " & msgType & vbNewLine & "Status: " & msgStatus & vbNewLine & "Folder: " & msgFolder & vbNewLine & "Attachment Size: " & msgAttachSize & " bytes"
                            map_MsgHistoryItems(i).msgText = msgText

                            'added 28-10-2016
                            If msgFromName <> "" Then
                                map_MsgHistoryItems(i).msgFromName = msgFromName
                            Else
                                If BlueSoleil_PBAP_NumberExistAsVcard(msgFromNo) = True Then
                                    If BlueSoleil_PBAP_GetNameFromNumber(msgFromNo) <> "" Then
                                        map_MsgHistoryItems(i).msgFromName = BlueSoleil_PBAP_GetNameFromNumber(msgFromNo)
                                    Else
                                        map_MsgHistoryItems(i).msgFromName = "****"
                                    End If
                                Else
                                    map_MsgHistoryItems(i).msgFromName = "****"
                                End If
                            End If

                            'ajout 25-12-2016
                            map_MsgHistoryItems(i).msgType = msgType

                            map_MsgHistoryItems(i).msgFromNumber = msgFromNo
                            ToLog("GetImageFromNumber for: '" & msgFromNo & "' --> " & BlueSoleil_BS_PBAP_GetImageFromNumber(msgFromNo))
                            ToLog("GetImageFromName for: '" & msgFromName & "' --> " & MainPath & "Photo\" & BlueSoleil_BS_PBAP_GetImageFromName(msgFromName))
                            If File.Exists(BlueSoleil_BS_PBAP_GetImageFromNumber(msgFromNo)) = True Then
                                map_MsgHistoryItems(i).msgImage = BlueSoleil_BS_PBAP_GetImageFromNumber(msgFromNo)
                                'ToLog("GetImageFromNumber" & BlueSoleil_BS_PBAP_GetImageFromNumber(msgFromNo))
                            ElseIf File.Exists(MainPath & "Photo\" & BlueSoleil_BS_PBAP_GetImageFromName(msgFromName)) = True Then
                                map_MsgHistoryItems(i).msgImage = MainPath & "Photo\" & BlueSoleil_BS_PBAP_GetImageFromName(msgFromName)
                                'ToLog("GetImageFromName" & MainPath & "Photo\" & BlueSoleil_BS_PBAP_GetImageFromName(msgFromName))
                            Else
                                ToLog("Image not found for number '" & msgFromNo & "'")
                                If PluginRunForDS = False Then
                                    map_MsgHistoryItems(i).msgImage = MainPath & "Photo\unknow.gif"
                                Else
                                    map_MsgHistoryItems(i).msgImage = ""
                                End If
                            End If
                            'added 28-10-2016

                            ToLog("GetMessage '" & i.ToString & "' --> " & msgText)
                            If msgAttachOriginalFN <> "" Then ToLog("GetAttachment '" & i.ToString & "' --> " & msgAttachOriginalFN)
                        End If
                    Catch ex As Exception
                        ToLog("Map error on extract BMSG file: '" & i.ToString & "' --> " & ex.Message)
                    End Try

                Next i

            End If

        End If


        If mapMsgPopUpAllreadySend = False Then
            mapMsgPopUpAllreadySend = True
        End If
        msgAttachSize = map_MsgHistoryItems(0).msgAttachmentSizes
        msgFromName = map_MsgHistoryItems(0).msgFromName
        RaiseEvent BlueSoleil_Event_MAP_MsgIsReceived(msgFromName, MsgNumberOfNewUnreadOld, map_MsgHistoryItems(0).msgText, IIf(msgAttachSize <> 0, True, False))
        ToLog(IIf(MsgNumberOfNewUnreadOld > 1, MsgNumberOfNewUnreadOld.ToString & " messages found !", MsgNumberOfNewUnreadOld.ToString & " message from " & msgFromName))

        'MsgBox("GetMessage = " & msgTorF & vbNewLine & msgInfo)
    End Sub


    ''' <summary>
    ''' Send text message (SMS)
    ''' </summary>
    ''' <param name="number"></param>
    ''' <param name="text"></param>
    ''' <remarks></remarks>
    Public Sub btnSendText(ByRef number As String, ByRef text As String)
        If mapHandleConn = 0 Then
            btnMsgConnect()

        End If

        Dim writeToPath As String = MainPath & "Messages\Outbox" 'My.Application.Info.DirectoryPath
        'If Strings.Right(writeToPath, 1) <> "\" Then writeToPath = writeToPath & "\"
        'writeToPath = writeToPath & "Outbox"

        If IO.Directory.Exists(writeToPath) = False Then
            Try
                IO.Directory.CreateDirectory(writeToPath)
            Catch ex As Exception

            End Try
        End If

        If IO.File.Exists(writeToPath & "\ToSend.BMSG") = True Then
            Try
                IO.File.Delete(writeToPath & "\ToSend.BMSG")
            Catch ex As Exception

            End Try
        End If

        Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text

        BMSG_WriteMessageFile_Text(writeToPath & "\ToSend.BMSG", number, text)

        Dim sendTorF As Boolean = False
        sendTorF = BlueSoleil_MAP_PushMessage_BMSG(mapHandleConn, writeToPath & "\ToSend.BMSG")

        'MsgBox("Done.  Send = " & sendTorF)

        ToLog("Done.  SMS Send = " & sendTorF)
        RaiseEvent BlueSoleil_Event_MAP_MsgIsSend(number, text)
    End Sub


    ''' <summary>
    ''' Send text with attachement file option (MMS)
    ''' </summary>
    ''' <param name="subject"></param>
    ''' <param name="number"></param>
    ''' <param name="text"></param>
    ''' <param name="tbMMSattachment"></param>
    ''' <remarks></remarks>
    Public Sub btnSendMMS(ByRef subject As String, ByRef number As String, ByRef text As String, ByRef tbMMSattachment As String)

        Dim writeToPath As String = MainPath & "Messages\Outbox" 'My.Application.Info.DirectoryPath
        'If Strings.Right(writeToPath, 1) <> "\" Then writeToPath = writeToPath & "\"
        'writeToPath = writeToPath & "Outbox"

        If IO.Directory.Exists(writeToPath) = False Then
            Try
                IO.Directory.CreateDirectory(writeToPath)
            Catch ex As Exception

            End Try
        End If

        If IO.File.Exists(writeToPath & "\ToSend.BMSG") = True Then
            Try
                IO.File.Delete(writeToPath & "\ToSend.BMSG")
            Catch ex As Exception

            End Try
        End If

        Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text

        Dim attachFN As String = tbMMSattachment 'tbMMSattachment.Text
        If IO.File.Exists(attachFN) = False Then attachFN = ""

        Dim sendTorF As Boolean = False
        If attachFN <> "" AndAlso UCase(Strings.Right(attachFN, 5)) = ".BMSG" Then
            'if the attachment is a .BMSG file, this will push it directly, instead of attaching it as a file.
            sendTorF = BlueSoleil_MAP_PushMessage_BMSG(mapHandleConn, attachFN)
        Else


            BMSG_WriteMessageFile_MMS_WithFile(writeToPath & "\ToSend.BMSG", number, text, attachFN, subject)
            sendTorF = BlueSoleil_MAP_PushMessage_BMSG(mapHandleConn, writeToPath & "\ToSend.BMSG")

        End If


        'MsgBox("Done.  Send = " & sendTorF)
        ToLog("Done.  MMS Send = " & sendTorF)
        RaiseEvent BlueSoleil_Event_MAP_MsgIsSend(number, text)

    End Sub

#End Region

#Region "PAN service"
    Public Function btnTether() As Boolean
        If BlueSoleil_IsInstalled() = False Then Return False
        If BlueSoleil_IsBluetoothReady() = False Then Return False
        Dim retBool As Boolean
        Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text
        BlueSoleil_PAN_RegisterCallbackForIPaddress()
        retBool = BlueSoleil_ConnectService_ByName("PAN", phoneDvcName, panHandleDvc, panHandleConn, panHandleSvc)

        If retBool = False Then
            BlueSoleil_PAN_UnregisterCallbackForIPaddress()
        End If

        If (panHandleConn <> 0) = True Then
            panIsActive = True
            RaiseEvent BlueSoleil_Event_PAN_IsOn()
            ToLog("PAN service is OK")
        Else
            panIsActive = False
            If ServicePAN_Usable = True Then
                If PluginRunForDS = False Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVARFROMVAR;POPUP;l_set_BTM_PANNotReady||MENU;POPUP.SKIN", False, True)
                Else
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_PANNotReady||POPUP;ReadingPhoneBook.SKIN;5", False, True)
                End If
                ToLog("Set your phone as Modem before !")
            Else
                If PluginRunForDS = False Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVARFROMVAR;POPUP;l_set_BTM_PANNotAccepted||MENU;POPUP.SKIN", False, True)
                Else
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVARFROMVAR;l_ReadingPhoneBook;l_set_BTM_PANNotAccepted||POPUP;ReadingPhoneBook.SKIN;5", False, True)
                End If
                ToLog("PAN connection isn't OK")
            End If
        End If

        'MsgBox("Done.  Return = " & (panHandleConn <> 0))
        Return retBool

    End Function

    Public Sub btnUnTether()
        If BlueSoleil_IsInstalled() = False Then Exit Sub
        If BlueSoleil_IsBluetoothReady() = False Then Exit Sub

        If panHandleConn <> 0 Then
            If BlueSoleil_IsServerConnected() = True Then
                BlueSoleil_DisconnectServiceConn(panHandleConn)
                'BlueSoleil_DisconnectServiceConn(panHandleSvc)
                'BlueSoleil_DisconnectServiceConn(panHandleDvc)
                'BlueSoleil_StopService(panHandleSvc)  'this seems to make the PAN disconnect correctly.
            End If

            BlueSoleil_PAN_UnregisterCallbackForIPaddress()
        End If

        panHandleConn = 0
        panHandleDvc = 0
        panHandleSvc = 0
        panIPaddress = ""
        panIsActive = False

        RaiseEvent BlueSoleil_Event_PAN_IsOff()
        ToLog("PAN connection is Off")

        'If DriveLine_Resume_IsSuspending = False Then
        '    'update internet connection status
        '    DriveLine_Internet_IsWiFiConnected = Net_IsWiFiConnected()
        '    DriveLine_Internet_IsCellularConnected = Net_IsCellularConnected()
        '    DriveLine_Internet_IsConnected = INet_GetOnlineStatus()
        '    DriveLine_Internet_IsPANconnected = Net_IsBlueToothPANconnected()
        '    'DriveLine_Internet_IsPANconnected = False

        '    If DriveLine_Internet_IsConnected <> tempPrevOnline Or DriveLine_Internet_IsWiFiConnected <> tempPrevWiFi Or DriveLine_Internet_IsCellularConnected <> tempPrevCellular Or DriveLine_Internet_IsPANconnected <> tempPrevPAN Then
        '        DriveLine_Skin_Handlers_AppEvent_OnlineChanged()
        '    End If
        'End If

        'MsgBox("Done.")
    End Sub
#End Region

#Region "AVRCP service"

    Public Sub btnMediaConnect()
        Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text
        BlueSoleil_ConnectService_ByName("AVRCP", phoneDvcName, avrcpHandleDvc, avrcpHandleConn, avrcpHandleSvc)

        If avrcpHandleDvc <> 0 Then
            BlueSoleil_AVRCP_RegisterCallbacks(avrcpHandleDvc)
            avrcpNeedsTrackInfo = True
            avrcpNeedsPlayStatus = True
            avrcpIsActive = True
            'MsgBox("Done.  Return = " & (avrcpHandleDvc <> 0) & ".  Anything playing (including Navigation) on your phone should play through the PC now.")

            ToLog("AVRCP connection is OK")
            If PluginRunForDS = False Then
                RaiseEvent BlueSoleil_Event_ERROR_Return("load;mobilephone_player.skin||WAIT;2||SETVAR;POPUP;Anything playing (including Navigation) on your phone should play through the PC now.||MENU;POPUP.SKIN", False, True)
            Else
                RaiseEvent BlueSoleil_Event_ERROR_Return("load;mobilephone_player.skin||WAIT;2||SETVAR;l_ReadingPhoneBook;Anything playing (including Navigation) on your phone should play through the PC now.||POPUP;ReadingPhoneBook.SKIN;5", False, True)
            End If

        Else
            avrcpIsActive = False
            'MsgBox("Done.  Return = " & (avrcpHandleDvc <> 0))
            RaiseEvent BlueSoleil_Event_ERROR_Return("AVRCP connection isn't OK", True, False)
            ToLog("AVRCP connection isn't OK")

        End If

    End Sub

    Public Sub btnMediaDisconnect()
        BlueSoleil_AVRCP_UnregisterCallbacks(avrcpHandleDvc)
        BlueSoleil_DisconnectServiceConn(avrcpHandleConn)
        avrcpHandleConn = 0
        avrcpHandleDvc = 0
        'MsgBox("Done.")
        avrcpIsActive = False
    End Sub

    Public Sub btnMediaPlay()
        BlueSoleil_AVRCP_SendCmd_Play(avrcpHandleDvc)
        AVRCP_status = "play"
        avrcpIsPlaying = True
        avrcpNeedsPlayStatus = True
    End Sub

    Public Sub btnMediaPause()
        BlueSoleil_AVRCP_SendCmd_Pause(avrcpHandleDvc)
        AVRCP_status = "pause"
        avrcpIsPlaying = False
        avrcpNeedsPlayStatus = True
    End Sub

    Public Sub btnMediaStop()
        BlueSoleil_AVRCP_SendCmd_Stop(avrcpHandleDvc)
        avrcpNeedsTrackInfo = True
        avrcpNeedsPlayStatus = True
    End Sub

    Public Sub btnMediaPrev()
        BlueSoleil_AVRCP_SendCmd_Prev(avrcpHandleDvc)
        avrcpNeedsTrackInfo = True
        avrcpNeedsPlayStatus = True
    End Sub

    Public Sub btnMediaNext()
        BlueSoleil_AVRCP_SendCmd_Next(avrcpHandleDvc)
        avrcpNeedsTrackInfo = True
        avrcpNeedsPlayStatus = True
    End Sub

    Public Sub btnMediaPower()
        BlueSoleil_AVRCP_SendCmd_Power(avrcpHandleDvc)
        avrcpNeedsTrackInfo = True
        avrcpNeedsPlayStatus = True
    End Sub

    Public Sub btnMediaMute()
        BlueSoleil_AVRCP_SendCmd_Mute(avrcpHandleDvc)
        avrcpNeedsTrackInfo = True
        avrcpNeedsPlayStatus = True
    End Sub

    Public Sub btnMediaVolUp()
        BlueSoleil_AVRCP_SendCmd_VolumeUp(avrcpHandleDvc)
        'BlueSoleil_AVRCP_SendReq_SetAbsoluteVolumePct(avrcpHandleDvc, 100)
    End Sub

    Public Sub btnMediaVolDown()
        BlueSoleil_AVRCP_SendCmd_VolumeDown(avrcpHandleDvc)
        'BlueSoleil_AVRCP_SendReq_SetAbsoluteVolumePct(avrcpHandleDvc, 100)
    End Sub
    Public Sub btnMediaSetPlayerSetting(repeat As Boolean, shuffle As Boolean)
        BlueSoleil_AVRCP_SendReq_SetPlayerSettings(avrcpHandleDvc, repeat, shuffle)
    End Sub


    Public Sub btnMediaEnableBrowsing()
        Dim retBool As Boolean = BlueSoleil_AVRCP_EnableBrowsing(avrcpHandleDvc)
        If retBool = True Then
            avrcpBrowsingOk = True
            RaiseEvent BlueSoleil_Event_ERROR_Return("AVRCP Set Browsing is OK", False, False)
        End If
        'MsgBox("Done.  Return = " & retBool)
    End Sub
    Public Sub btnMediaGetLists() 'Handles btnMediaGetNowPlaying.Click
        BlueSoleil_AVRCP_SendReq_GetFolderItems_NowPlayingList(avrcpHandleDvc)
    End Sub
    Public Sub btnMediaGetFileSystem() 'Handles btnMediaGetFileSystem.Click
        BlueSoleil_AVRCP_SendReq_GetFolderItems_FileSystem(avrcpHandleDvc)
    End Sub




#End Region

#Region "A2DP service"
    Public Function btnA2DPDConnect() As Boolean

        If BlueSoleil_IsInstalled() = False Then Return False
        If BlueSoleil_IsBluetoothReady() = False Then Return False

        Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text
        Dim connTorF As Boolean = False
        btnA2DPDisconnect()

        connTorF = BlueSoleil_ConnectService_ByName("A2DP", phoneDvcName, a2dpHandleDvc, a2dpHandleConn, a2dpHandleSvc)

        If connTorF = False Then
            ToLog("Failed to connect to A2DP Profile.")
            a2dpIsActive = False
            If PluginRunForDS = False Then
                RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;POPUP;Failed to connect to A2DP Profile.||MENU;POPUP.SKIN", False, True)
            Else
                RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;l_ReadingPhoneBook;Failed to connect to A2DP Profile.||POPUP;ReadingPhoneBook.SKIN;5", False, True)
            End If
        Else
            ToLog("Connected to A2DP Profile.")
            a2dpIsActive = True
            If avrcpIsActive = False Then
                If PluginRunForDS = False Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;POPUP;Headset need AVRCP service||MENU;POPUP.SKIN", False, True)
                Else
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;l_ReadingPhoneBook;Headset need AVRCP service||POPUP;ReadingPhoneBook.SKIN;5", False, True)
                End If
            End If
        End If

        Return connTorF

    End Function

    Public Sub btnA2DPDisconnect()

        'If DLsvc_BS_UseBlueSoleil = False Then Exit Sub

        If BlueSoleil_IsInstalled() = False Then Exit Sub
        If BlueSoleil_IsBluetoothReady() = False Then Exit Sub

        'If DriveLine_Resume_IsResuming = True Then Exit Sub

        If a2dpHandleConn <> 0 Then
            If BlueSoleil_IsServerConnected() = True Then
                If DriveLine_BS_A2DP_IsConnected() = True Then
                    BlueSoleil_DisconnectServiceConn(a2dpHandleConn)
                    ToLog("A2DP service is disconnected")
                    If PluginRunForDS = False Then
                        RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;POPUP;A2DP service is disconnected||MENU;POPUP.SKIN", False, True)
                    Else
                        RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;l_ReadingPhoneBook;A2DP service is disconnected||POPUP;ReadingPhoneBook.SKIN;5", False, True)
                    End If
                End If
            End If
        End If

        a2dpHandleConn = 0
        a2dpHandleDvc = 0
        a2dpHandleSvc = 0

        a2dpIsActive = False
    End Sub


    Public Function DriveLine_BS_A2DP_IsConnected() As Boolean

        If BlueSoleil_IsInstalled() = False Then Return False

        'If a2dpHandleConn <> 0 Then Return True

        Return BlueSoleil_GetConnectionProperties(a2dpHandleConn)

        Return False

    End Function

#End Region

#Region "OPP service"
    'btnOPPpush
    Public Sub BlueSoleil_PBAP_CreateNewContactAsVcard(name As String,
                                                        numbers() As String, label() As String, count As Integer,
                                                        addresss() As String, addresslabel() As String, addresscount As Integer,
                                                        Optional picture As String = "", Optional mail As String = "",
                                                        Optional organisation As String = "", Optional note As String = "",
                                                        Optional geoposition As String = "", Optional birthday As String = "", Optional eraseFile As Boolean = True)

        Try

            'prepare variables.
            Dim vcfFullName As String = Trim(name)
            Dim vcfEMail As String = Trim(mail)
            Dim vcfImageFN As String = Trim(picture)
            Dim vcfNotes As String = Trim(note)
            Dim vcfImageObj As Bitmap = Nothing
            Dim vcfCompany As String = Trim(organisation)
            Dim vcfBirthDay As String = Trim(birthday)

            Dim vcfPhoneCell As String = Trim(numbers(0))
            Dim vcfPhoneWork As String = Trim(numbers(2))
            Dim vcfPhoneHome As String = Trim(numbers(1))
            'fax --> numbers(3)
            Dim vcfPhoneNumberCount As Integer = 0

            Dim vcfAddressCell As String = Trim(addresss(0))
            Dim vcfAddressWork As String = Trim(addresss(2))
            Dim vcfAddressHome As String = Trim(addresss(1))
            Dim vcfAddressCount As Integer = 0

            'prepare array of phone numbers and labels
            Dim vcfPhoneNumberArray(0 To 0) As String, vcfPhoneLabelArray(0 To 0) As String
            If vcfPhoneCell <> "" Then
                ReDim Preserve vcfPhoneNumberArray(0 To vcfPhoneNumberCount)
                ReDim Preserve vcfPhoneLabelArray(0 To vcfPhoneNumberCount)
                vcfPhoneNumberArray(vcfPhoneNumberCount) = vcfPhoneCell
                vcfPhoneLabelArray(vcfPhoneNumberCount) = "CELL"
                vcfPhoneNumberCount = vcfPhoneNumberCount + 1
            End If
            If vcfPhoneWork <> "" Then
                ReDim Preserve vcfPhoneNumberArray(0 To vcfPhoneNumberCount)
                ReDim Preserve vcfPhoneLabelArray(0 To vcfPhoneNumberCount)
                vcfPhoneNumberArray(vcfPhoneNumberCount) = vcfPhoneWork
                vcfPhoneLabelArray(vcfPhoneNumberCount) = "WORK"
                vcfPhoneNumberCount = vcfPhoneNumberCount + 1
            End If
            If vcfPhoneHome <> "" Then
                ReDim Preserve vcfPhoneNumberArray(0 To vcfPhoneNumberCount)
                ReDim Preserve vcfPhoneLabelArray(0 To vcfPhoneNumberCount)
                vcfPhoneNumberArray(vcfPhoneNumberCount) = vcfPhoneHome
                vcfPhoneLabelArray(vcfPhoneNumberCount) = "HOME"
                vcfPhoneNumberCount = vcfPhoneNumberCount + 1
            End If

            'prepare array of addresses and labels.
            Dim vcfAddressArray(0 To 0) As String, vcfAddressLabelArray(0 To 0) As String
            If vcfAddressCell <> "" Then
                ReDim Preserve vcfAddressArray(0 To vcfAddressCount)
                ReDim Preserve vcfAddressLabelArray(0 To vcfAddressCount)
                vcfAddressArray(vcfAddressCount) = vcfAddressCell
                vcfAddressLabelArray(vcfAddressCount) = "CELL"
                vcfAddressCount = vcfAddressCount + 1
            End If
            If vcfAddressWork <> "" Then
                ReDim Preserve vcfAddressArray(0 To vcfAddressCount)
                ReDim Preserve vcfAddressLabelArray(0 To vcfAddressCount)
                vcfAddressArray(vcfAddressCount) = vcfAddressWork
                vcfAddressLabelArray(vcfAddressCount) = "WORK"
                vcfAddressCount = vcfAddressCount + 1
            End If
            If vcfAddressHome <> "" Then
                ReDim Preserve vcfAddressArray(0 To vcfAddressCount)
                ReDim Preserve vcfAddressLabelArray(0 To vcfAddressCount)
                vcfAddressArray(vcfAddressCount) = vcfAddressHome
                vcfAddressLabelArray(vcfAddressCount) = "HOME"
                vcfAddressCount = vcfAddressCount + 1
            End If

            'prepare the image object.
            If vcfImageFN <> "" Then
                Try
                    If IO.File.Exists(vcfImageFN) = True Then
                        vcfImageObj = CType(Image.FromFile(vcfImageFN), Bitmap)
                    End If

                Catch ex As Exception

                End Try
            End If

            If eraseFile = True Then
                If File.Exists(MainPath & "Obex\NewCard.vcf") Then File.Delete(MainPath & "Obex\NewCard.vcf")
            End If

            'write the vcard file.
            Dim writeTorF As Boolean = VCard_WriteContactInfo_V3(MainPath & "Obex\NewCard.vcf", vcfFullName, vcfPhoneNumberArray, vcfPhoneLabelArray, vcfPhoneNumberCount, vcfAddressArray, vcfAddressLabelArray, vcfAddressCount, vcfImageObj, vcfEMail, vcfCompany, vcfNotes, vcfBirthDay)

            If IsNothing(vcfImageObj) = False Then
                vcfImageObj.Dispose()
            End If

            If writeTorF = True Then
                If PluginRunForDS = False Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;POPUP;" & MainPath & "Obex\NewCard.vcf||MENU;POPUP.SKIN", False, True)
                Else
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;l_ReadingPhoneBook;" & MainPath & "Obex\NewCard.vcf||POPUP;ReadingPhoneBook.SKIN;5", False, True)
                End If
            Else
                If PluginRunForDS = False Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;POPUP;NewCard Build Error||MENU;POPUP.SKIN", False, True)
                Else
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;l_ReadingPhoneBook;NewCard Build Error||POPUP;ReadingPhoneBook.SKIN;5", False, True)
                End If
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Public Sub btnOPPSaveToPhone()
        Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text
        'connect to OPP.
        Dim connTorF As Boolean = BlueSoleil_ConnectService_ByName("OPP", phoneDvcName, oppHandleDvc, oppHandleConn, oppHandleSvc)
        'register OPP status callback?  meh.  don't really need it for simple connect-push-disconnect.
        'push the vcard file.
        Dim pushTorF As Boolean
        If connTorF = True Then
            'the callback doesn't do anything special for us.
            BlueSoleil_OPP_RegisterStatusCallback(oppHandleConn)
            pushTorF = BlueSoleil_OPP_PushVCard(oppHandleConn, MainPath & "Obex\NewCard.vcf")
            'unregister status callback?  meh.
            BlueSoleil_OPP_UnregisterStatusCallback(oppHandleConn)
            'disconnect.
            BlueSoleil_DisconnectServiceConn(oppHandleConn)

            If pushTorF = True Then
                If PluginRunForDS = False Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;POPUP;NewCard is saved in your phone||MENU;POPUP.SKIN", False, True)
                Else
                    RaiseEvent BlueSoleil_Event_ERROR_Return("SETVAR;l_ReadingPhoneBook;NewCard is saved in your phone||POPUP;ReadingPhoneBook.SKIN;5", False, True)
                End If
            End If
        End If
        'MsgBox("Connect = " & connTorF & "   PushCard = " & pushTorF)
    End Sub

    'btnOPPpull
    Private Sub PullVcardFromPhone()

        'create path to write temporary vcard file to.
        Dim writeToPath As String = MainPath 'My.Application.Info.DirectoryPath
        'If Strings.Right(writeToPath, 1) <> "\" Then writeToPath = writeToPath & "\"
        writeToPath = writeToPath & "Obex"

        If IO.Directory.Exists(writeToPath) = False Then
            Try
                IO.Directory.CreateDirectory(writeToPath)
            Catch ex As Exception

            End Try
        End If

        Dim vcfFileName As String = writeToPath     'file always gets saved as REMOTE.VCF

        Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName

        'connect to OPP.
        Dim connTorF As Boolean = BlueSoleil_ConnectService_ByName("OPP", phoneDvcName, oppHandleDvc, oppHandleConn, oppHandleSvc)


        'push the vcard file.
        Dim pullTorF As Boolean = False
        Dim pullAccessDenied As Boolean = False
        If connTorF = True Then

            'register OPP status callback?  meh.  don't really need it for simple connect-pull-disconnect.
            BlueSoleil_OPP_RegisterStatusCallback(oppHandleConn)

            pullTorF = BlueSoleil_OPP_PullVCard(oppHandleConn, vcfFileName, pullAccessDenied)

            'unregister status callback?  meh.
            BlueSoleil_OPP_UnregisterStatusCallback(oppHandleConn)

            'disconnect.
            BlueSoleil_DisconnectServiceConn(oppHandleConn)
        End If

        If pullTorF = True Then

            Dim cardName As String = "", cardPhoneNumbers(0 To 0) As String, cardPhoneLabels(0 To 0) As String, cardPhoneCount As Integer = 0
            Dim cardImage As Bitmap = Nothing
            Dim tempImage As Bitmap = Nothing
            Dim cardEMail As String = "", cardLastCallDateTime As DateTime = Nothing, cardAddresses(0 To 0) As String, cardAddressLabels(0 To 0) As String, cardAddressCount As Integer, cardBirthday As String = "", cardGeoPos As String = "", cardNotes As String = "", cardOrganization As String = ""
            'Dim cardLastCallType As String = ""
            'we know the first contact is at offset 0.  However, we could call VCard_GetContactOffsets if we really wanted to, and use the offset from that.
            'VCard_GetContactInfo(vcfFileName, 0, cardName, cardEMail, cardPhoneNumbers, cardPhoneLabels, cardPhoneCount, tempImage, cardLastCallDateTime, cardLastCallType, cardAddresses, cardAddressLabels, cardAddressCount, cardBirthday, cardGeoPos, cardNotes, cardOrganization)
            VCard_GetContactInfo(vcfFileName, 0, cardName, cardEMail, cardPhoneNumbers, cardPhoneLabels, cardPhoneCount, tempImage, cardLastCallDateTime, "", cardAddresses, cardAddressLabels, cardAddressCount, cardBirthday, cardGeoPos, cardNotes, cardOrganization)

            Dim i As Integer
            Dim cardInfoStr As String = "Name:    " & cardName & vbNewLine
            If cardEMail <> "" Then cardInfoStr = cardInfoStr & "E-Mail:   " & cardEMail & vbNewLine
            If cardOrganization <> "" Then cardInfoStr = cardInfoStr & "Company:   " & cardOrganization & vbNewLine
            For i = 0 To cardPhoneCount - 1
                cardInfoStr = cardInfoStr & cardPhoneLabels(i) & " Phone: " & cardPhoneNumbers(i) & vbNewLine
            Next i
            For i = 0 To cardPhoneCount - 1
                cardInfoStr = cardInfoStr & cardAddressLabels(i) & " Addr: " & cardAddresses(i) & vbNewLine
            Next i

            cardInfoStr = cardInfoStr & vbNewLine & "The VCard is saved as " & vcfFileName

            MsgBox("Connect = " & connTorF & "   PullCard = " & pullTorF & vbNewLine & vbNewLine & cardInfoStr)

        Else

            If pullAccessDenied = True Then
                MsgBox("Connect = " & connTorF & "   PullCard = " & pullTorF & "  - Access Denied")
            Else
                MsgBox("Connect = " & connTorF & "   PullCard = " & pullTorF)
            End If

        End If


    End Sub


#End Region

#Region "SPP service"
    Public Sub btnSppConnect()
        Dim retBool As Boolean
        Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName 'cboPhoneList.Text
        retBool = BlueSoleil_ConnectService_ByName("SPP", phoneDvcName, sppHandleDvc, sppHandleConn, sppHandleSvc)

        sppCOMMportNum = BlueSoleil_SPP_GetCOMMportNum(sppHandleConn)
        If retBool = True Then
            sppIsActive = True
        End If
        MsgBox("Done.  Return = " & (sppHandleConn <> 0) & vbNewLine & vbNewLine & "COMM Port = " & sppCOMMportNum)
    End Sub

    Public Sub btnSppDisConnect()
        Dim retBool As Boolean
        retBool = BlueSoleil_DisconnectServiceConn(sppHandleConn)
        If retBool = True Then
            sppIsActive = False
        End If
        'MsgBox("Done.")
    End Sub
    'Public Function BlueSoleil_SPP_GetCOMMportNumber() As String
    '    Return BlueSoleil_SPP_GetCOMMportNum(sppHandleConn)
    'End Function
#End Region

#Region "FTP"
    Public Sub btnFTPconnect()
        Try
            Dim phoneDvcName As String = TempPluginSettings.PhoneDeviceName

            Dim connTorF As Boolean = BlueSoleil_ConnectService_ByName("FTP", phoneDvcName, ftpHandleDvc, ftpHandleConn, ftpHandleSvc)

            If connTorF = True Then
                BlueSoleil_FTP_RegisterStatusCallback(ftpHandleConn)
                BlueSoleil_FTP_GetRemotePath(ftpHandleConn, ftpRemotePath)
                BlueSoleil_FTP_BrowseFolder(ftpHandleConn, ftpRemotePath)
            End If

            'MsgBox("Done.  Return = " & (ftpHandleConn <> 0))
            RaiseEvent BlueSoleil_Event_ERROR_Return("FTP service is ready !", True, False)
        Catch ex As Exception
            RaiseEvent BlueSoleil_Event_ERROR_Return(ex.Message, True, False)
        End Try


    End Sub

    Public Sub btnFTPdisconnect()

        BlueSoleil_FTP_UnregisterStatusCallback(ftpHandleConn)
        BlueSoleil_DisconnectServiceConn(ftpHandleConn)

        'MsgBox("Done.")

    End Sub

    Private Sub btnFTPcreateFolder() ' Handles btnFTPcreateFolder.Click

        Dim fdrName As String = InputBox("Enter name for new folder:")

        If fdrName = "" Then Exit Sub

        Dim retBool As Boolean = BlueSoleil_FTP_CreateDirectory(ftpHandleConn, fdrName)


        'refresh current folder.
        Dim ftpPath As String = ""
        BlueSoleil_FTP_GetRemotePath(ftpHandleConn, ftpPath)
        ftpPath = Replace(ftpPath, Chr(0), "")
        If ftpPath <> "\" And ftpPath <> "" Then
            'lvwFTPbrowser.Items.Add("[..]")
            RaiseEvent BlueSoleil_Event_ERROR_Return("CLEAR;ALL||CLADD;[..]", False, True)
        End If
        BlueSoleil_FTP_BrowseFolder(ftpHandleConn, Strings.Mid(ftpPath, 2))

        ToLog("Return = " & retBool)

    End Sub

    Private Sub btnFTPupload(inpFN As String)

        'Dim inpFN As String = Browse_ShowOpenFile(Me, "All files (*.*)|*.*", "Select a file to upload...")

        If inpFN = "" Then Exit Sub

        Dim retBool As Boolean
        retBool = BlueSoleil_FTP_PutFile(ftpHandleConn, inpFN, IO.Path.GetFileName(inpFN))


        'refresh current folder.
        Dim ftpPath As String = ""
        BlueSoleil_FTP_GetRemotePath(ftpHandleConn, ftpPath)
        ftpPath = Replace(ftpPath, Chr(0), "")
        If ftpPath <> "\" And ftpPath <> "" Then
            'lvwFTPbrowser.Items.Add("[..]")
        End If
        BlueSoleil_FTP_BrowseFolder(ftpHandleConn, Strings.Mid(ftpPath, 2))


        MsgBox("Return = " & retBool)

    End Sub

    Private Sub btnFTPdownload(saveToDir As String)

        Dim selItem As ListViewItem = Nothing

        'If lvwFTPbrowser.SelectedItems.Count < 1 Then Exit Sub

        'selItem = lvwFTPbrowser.SelectedItems(0)

        If selItem.Text = "[..]" Then
            Exit Sub
        End If

        If selItem.SubItems.Count < 2 Or Strings.Left(selItem.Text, 1) = "[" Then
            Dim getDir As String = Replace(selItem.Text, "[", "")
            getDir = Replace(getDir, "]", "")

            'Dim saveToDir As String = Browse_ShowOpenFolder(Me, "Select folder to save to...")
            If saveToDir = "" Then Exit Sub
            Dim retBool_GetFolder As Boolean
            retBool_GetFolder = BlueSoleil_FTP_GetFolder(ftpHandleConn, getDir, saveToDir)

            MsgBox("GetFolder Return = " & retBool_GetFolder)

        Else
            Dim saveFN As String = "" 'Browse_ShowSaveFile(Me, "All files (*.*)|*.*", "Save As...", selItem.Text)
            If saveFN = "" Then Exit Sub
            Dim retBool_GetFile As Boolean
            retBool_GetFile = BlueSoleil_FTP_GetFile(ftpHandleConn, IO.Path.GetFileName(saveFN), saveFN)

            MsgBox("GetFile Return = " & retBool_GetFile)

        End If

    End Sub

    Private Sub btnFTPcancelXfer()

        If ftpHandleConn = 0 Then
            Exit Sub
        End If

        Dim retBool As Boolean = BlueSoleil_FTP_CancelTransfer(ftpHandleConn)

        MsgBox("Return = " & retBool)

    End Sub

    Private Sub btnFTPdelete()

        Dim selItem As ListViewItem = Nothing

        'If lvwFTPbrowser.SelectedItems.Count < 1 Then Exit Sub

        'selItem = lvwFTPbrowser.SelectedItems(0)

        If selItem.Text = "[..]" Then Exit Sub


        'confirm delete.
        Dim YorN As MsgBoxResult = MsgBox("Are you sure you want to delete " & selItem.Text & " ?", MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, "Confirm delete")
        If YorN <> MsgBoxResult.Yes Then Exit Sub


        Dim retBool As Boolean = False
        If selItem.SubItems.Count < 2 Or Strings.Left(selItem.Text, 1) = "[" Then
            'folder.  remove brackets.
            Dim delDir As String = Replace(selItem.Text, "[", "")
            delDir = Replace(delDir, "]", "")

            retBool = BlueSoleil_FTP_DeleteDirectory(ftpHandleConn, delDir)

        Else
            'file
            retBool = BlueSoleil_FTP_DeleteFile(ftpHandleConn, selItem.Text)
        End If



        'refresh current folder.
        Dim ftpPath As String = ""
        BlueSoleil_FTP_GetRemotePath(ftpHandleConn, ftpPath)
        ftpPath = Replace(ftpPath, Chr(0), "")
        If ftpPath <> "\" And ftpPath <> "" Then
            'lvwFTPbrowser.Items.Add("[..]")
        End If
        BlueSoleil_FTP_BrowseFolder(ftpHandleConn, Strings.Mid(ftpPath, 2))


        MsgBox("Return = " & retBool)

    End Sub

    Private Sub lvwFTPbrowser_DoubleClick()

        Dim selItem As ListViewItem = Nothing

        'If lvwFTPbrowser.SelectedItems.Count < 1 Then Exit Sub

        'selItem = lvwFTPbrowser.SelectedItems(0)

        Dim ftpPath As String = ""

        Dim cdTorF As Boolean = False

        'up one folder.
        If selItem.Text = "[..]" Then

            cdTorF = BlueSoleil_FTP_UpOneFolder(ftpHandleConn)

            If cdTorF = False Then
                'failed to change dir
                MsgBox("Failed to move up one level.")
                Exit Sub
            End If

            'lvwFTPbrowser.Items.Clear()
            BlueSoleil_FTP_GetRemotePath(ftpHandleConn, ftpPath)

            'If ftpPath <> "\" And ftpPath <> "" Then
            '    lvwFTPbrowser.Items.Add("[..]")
            'End If

            If ftpPath = "\" Then ftpPath = "\\"

            BlueSoleil_FTP_BrowseFolder(ftpHandleConn, Strings.Mid(ftpPath, 2))
            Exit Sub
        End If

        'go to folder.
        If selItem.SubItems.Count < 2 Or Strings.Left(selItem.Text, 1) = "[" Then

            Dim newDir As String = Replace(selItem.Text, "[", "")
            newDir = Replace(newDir, "]", "")

            Dim dirAccessDeniedError As Boolean = False

            cdTorF = BlueSoleil_FTP_SetRemotePath(ftpHandleConn, "\" & newDir, dirAccessDeniedError)
            If cdTorF = False Then
                'failed to change dir

                If dirAccessDeniedError = True Then
                    MsgBox("Failed to change to directory " & newDir & " - Access Denied.")
                Else
                    MsgBox("Failed to change to directory " & newDir)
                End If


                Exit Sub
            End If


            'lvwFTPbrowser.Items.Clear()
            'If newDir <> "\" And newDir <> "" Then
            '    lvwFTPbrowser.Items.Add("[..]")
            'End If

            BlueSoleil_FTP_BrowseFolder(ftpHandleConn, newDir)
            Exit Sub
        End If

        'double-clicked on file.  download?


    End Sub
#End Region

#Region "BirthDay Search"
    Public Sub BlueSoleil_CheckBirthDay()
        Dim j As Integer = 0, cardSplit() As String = Nothing

        Try
            'Bluesoleil_LoadContactsFile() ' reload the contact's info from the pb.vcf file
            cardBirthday.Clear()
            For j = 0 To PhoneBookEntryCount - 1
                'MessageBox.Show("Birthday of the contact " & PhoneBookEntries(j).EntryBirthDay)
                If IsNothing(PhoneBookEntries(j).EntryBirthDay) = False AndAlso IsBirthDay(Convert.ToDateTime(PhoneBookEntries(j).EntryBirthDay)) = True Then
                    cardBirthday.Add(PhoneBookEntries(j).EntryName & "|" & PhoneBookEntries(j).EntryPhoneNumbers(0).EntryPhoneNumber & "|" & Convert.ToString(GetCurrentAge(PhoneBookEntries(j).EntryBirthDay)))
                    ToLog("Birthday for " & PhoneBookEntries(j).EntryName & ", " & PhoneBookEntries(j).EntryPhoneNumbers(0).EntryPhoneNumber & ", " & Convert.ToString(GetCurrentAge(PhoneBookEntries(j).EntryBirthDay)))
                Else
                    Continue For
                End If
            Next j

            If cardBirthday.Count > 0 And cardBirthdayEntAllReadySend = False Then
                RaiseEvent BlueSoleil_Event_PBAP_IsBirthDay(cardBirthday.Count)
                cardBirthdayEntAllReadySend = True 'stop multi event
            End If

        Catch ex As Exception
            ToLog(ex.Message)
        End Try


    End Sub
    Public Function IsBirthDay(ByRef DOB As DateTime) As Boolean
        Dim today As DateTime = DateTime.Now
        If today.Day = DOB.Day AndAlso today.Month = DOB.Month Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function DaysToNextBirthday(ByRef DOB As DateTime) As Integer
        Dim today As DateTime = DateTime.Now
        Dim Birthday As New DateTime(today.Year, DOB.Month, DOB.Day)
        Dim diff As TimeSpan = Birthday - today
        If diff.Days < 0 Then
            ' Had his birthday this year!
            diff = Birthday.AddYears(1) - today
        End If
        Return diff.Days
    End Function
    Public Function GetCurrentAge(ByVal dob As DateTime) As Integer
        Dim age As Integer
        age = Today.Year - dob.Year
        If (dob > Today.AddYears(-age)) Then age -= 1
        Return age
    End Function

#End Region

#Region "Search Services List"
    Public Sub GetRemoteServicesList(Optional buildList As Boolean = False) 'Remote List
        Try
            ToLog("Remote Services list is on !")
            'get service handles for device.
            Dim svcHandleArray(0 To 0) As UInt32, svcHandleCount As Integer = 0

            Dim TorF As Boolean
            TorF = BlueSoleil_GetRemoteDeviceServiceHandles_Refresh(hfpHandleDvc, svcHandleArray, svcHandleCount)
            'TorF = BlueSoleil_GetRemoteDeviceServiceHandles(hfpHandleConnHFAG, svcHandleArray, svcHandleCount)
            CreateServicesList()
            'Dim ServiceList As New Dictionary(Of String, String)
            Dim ServicesArray As New ArrayList, servicedetail(0 To 0) As String, Tvalue As String = ""

            Dim i As Integer, svcName As String = "", svcClass As UShort = 0
            If TorF = True Then
                For i = 0 To svcHandleCount - 1
                    BlueSoleil_GetRemoteServiceAttributes(svcHandleArray(i), svcName, svcClass)
                    If svcClass <> 0 And TorF = True Then
                        If ServiceList.ContainsKey(Hex(svcClass)) = True Then
                            ServicesArray.Add(Hex(svcClass) & "," & ServiceList(Hex(svcClass)))
                        Else
                            ServicesArray.Add(Hex(svcClass) & ", " & Hex(svcClass) & " is an unknow service")
                        End If

                    End If
                Next i
                ToLog("Found " & svcHandleCount & " remote services.")
                RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;" & "Found " & svcHandleCount & " remote services.", False, True)
            Else
                ToLog("Unable to retrieve remote service list from device " & TempPluginSettings.PhoneDeviceName & " !")
                RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;Unable to retrieve remote service list from device " & TempPluginSettings.PhoneDeviceName & " !", False, True)
                'MsgBox("Unable to retrieve service list from device.")
            End If

            'Sort the array
            ServicesArray.Sort()

            For Each line As String In ServicesArray
                servicedetail = line.Split(",")
                If buildList = True Then
                    AddCustomList(MainPath & "MobilePhone_RemoteServices.lst", servicedetail(0), "&H" & servicedetail(0) & " --> " & servicedetail(1), SkinPath & "Indicators\BT_01.png")
                Else
                    If servicedetail(0) = "1101" Then ServiceSPP_Usable = True
                    If servicedetail(0) = "1105" Then ServiceOPP_Usable = True
                    If servicedetail(0) = "1106" Then ServiceFTP_Usable = True
                    If servicedetail(0) = "110A" Then ServiceA2DP_Usable = True
                    If servicedetail(0) = "112F" Then ServicePBAP_Usable = True
                    If servicedetail(0) = "1132" Then ServiceMAP_Usable = True
                    If servicedetail(0) = "1116" Then ServicePAN_Usable = True
                    If servicedetail(0) = "110C" Then ServiceAVRCP_Usable = True

                End If
            Next

            ToLog(ServicesArray.Count & " Remote services accepted by the device " & TempPluginSettings.PhoneDeviceName & " !")
            If ServicePBAP_Usable = True Then ToLog("PBAP Usable") Else ToLog("PBAP Not Usable")
            If ServiceOPP_Usable = True Then ToLog("OPP Usable") Else ToLog("OPP Not Usable")
            If ServiceMAP_Usable = True Then ToLog("MAP Usable") Else ToLog("MAP Not Usable")
            If ServiceAVRCP_Usable = True Then ToLog("AVRCP Usable") Else ToLog("AVRCP Not Usable")
            If ServiceA2DP_Usable = True Then ToLog("A2DP Usable") Else ToLog("A2DP Not Usable")
            If ServiceSPP_Usable = True Then ToLog("SPP Usable") Else ToLog("SPP Not Usable")
            If ServiceFTP_Usable = True Then ToLog("FTP Usable") Else ToLog("FTP Not Usable")
            If ServicePAN_Usable = True Then ToLog("PAN Usable") Else ToLog("PAN Not Usable")
            ServiceList.Clear()

            If ServicePBAP_Usable = True Then RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;PBAP Usable", False, True) Else RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;PBAP Not Usable", False, True)
            If ServiceOPP_Usable = True Then RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;OPP Usable", False, True) Else RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;OPP Not Usable", False, True)
            If ServiceMAP_Usable = True Then RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;MAP Usable", False, True) Else RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;MAP Not Usable", False, True)
            If ServiceAVRCP_Usable = True Then RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;AVRCP Usable", False, True) Else RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;AVRCP Not Usable", False, True)
            If ServiceA2DP_Usable = True Then RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;A2DP Usable", False, True) Else RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;A2DP Not Usable", False, True)
            If ServiceSPP_Usable = True Then RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;SPP Usable", False, True) Else RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;SPP Not Usable", False, True)
            If ServiceFTP_Usable = True Then RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;FTP Usable", False, True) Else RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;FTP Not Usable", False, True)
            If ServicePAN_Usable = True Then RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;PAN Usable", False, True) Else RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;PAN Not Usable", False, True)

            If buildList = True Then
                RaiseEvent BlueSoleil_Event_ERROR_Return("MENU;MOBILEPHONE_INFO.skin||CLCLEAR;ALL||CLLOAD;" & MainPath & "MobilePhone_RemoteServices.lst", False, True)
                ToLog(MainPath & "MobilePhone_RemoteServices.lst is loaded !")
            End If
            ServiceList.Clear()
        Catch ex As Exception
            ToLog("Remote Services Error " & ex.Message)
        End Try

    End Sub
    Public Sub GetLocalServicesList(Optional buildList As Boolean = False) 'Loacl List
        Try
            ToLog("Local Services list is on !")
            'get service handles for device.
            Dim svcHandleArray(0 To 0) As UInt32, svcClassArray(0 To 0) As UInt16, svcNameArray(0 To 0) As String, svcHandleCount As Integer = 0
            Dim TorF As Boolean
            TorF = BlueSoleil_GetLocalDeviceServices(svcClassArray, svcHandleArray, svcNameArray, svcHandleCount)
            CreateServicesList()
            'Dim ServiceList As New Dictionary(Of String, String)
            Dim ServicesArray As New ArrayList, servicedetail(0 To 0) As String

            Dim i As Integer, svcName As String = "", svcClass As UShort = 0
            If TorF = True Then
                For i = 0 To svcHandleCount - 1
                    svcName = svcNameArray(i)
                    svcClass = svcClassArray(i)
                    If svcClass <> 0 Then
                        ServicesArray.Add(Hex(svcClass) & "," & ServiceList(Hex(svcClass)))
                    End If
                Next i
                ToLog("Found " & svcHandleCount & " local services.")
                RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;" & "Found " & svcHandleCount & " local services.", False, True)
            Else
                ToLog("Unable to retrieve local service list from device " & TempPluginSettings.PhoneDeviceName & " !")
                RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;Unable to retrieve local service list from device " & TempPluginSettings.PhoneDeviceName & " !", False, True)
                'MsgBox("Unable to retrieve service list from device.")
            End If

            'Sort the array
            ServicesArray.Sort()

            For Each line As String In ServicesArray
                servicedetail = line.Split(",")
                If buildList = True Then
                    AddCustomList(MainPath & "MobilePhone_LocalServices.lst", servicedetail(0), "&H" & servicedetail(0) & " --> " & servicedetail(1), SkinPath & "Indicators\BT_01.png")
                End If
            Next

            ToLog(ServicesArray.Count & " Local services accepted by the device " & TempPluginSettings.PhoneDeviceName & " !")

            ServicesArray.Clear()

            If buildList = True Then
                RaiseEvent BlueSoleil_Event_ERROR_Return("MENU;MOBILEPHONE_INFO.skin||CLCLEAR;ALL||CLLOAD;" & MainPath & "MobilePhone_LocalServices.lst", False, True)
                ToLog(MainPath & "MobilePhone_LocalServices.lst is loaded !")
            End If
        Catch ex As Exception
            ToLog("Local Services Error " & ex.Message)
        End Try

    End Sub

#End Region

#Region "Search Devices"
    'Other method for check devices
    ''' <summary>
    ''' Build a RR list with the paired device
    ''' </summary>
    ''' <param name="buildList"></param>
    ''' <remarks></remarks>
    Public Sub btnRefreshDevices(buildList As Boolean)
        ToLog("Paired Devices list is on !")
        BlueSoleil_GetPairedDevices_NamesAndHandles(dvcNameArray, dvcHandleArray, dvcArrayCount)
        Dim retdevClass(0 To 0) As UInteger
        Dim i As Integer
        PhoneDeviceNameList.Clear()
        If dvcArrayCount > 0 Then
            ReDim dvcNameArray(0 To dvcArrayCount - 1)
            ReDim retdevClass(0 To dvcArrayCount - 1)
            Dim DeviceClass As String
            For i = 0 To dvcArrayCount - 1
                dvcNameArray(i) = BlueSoleil_GetRemoteDeviceName(dvcHandleArray(i))
                BlueSoleil_GetRemoteDeviceClass(dvcHandleArray(i), retdevClass(i)) 'Btsdk_GetRemoteDeviceClass(dvcHandleArray(i), retdevClass(i))
                DeviceClass = ClassVal2String(retdevClass(i))
                'MsgBox("Name: " & retDvcNames(i) & vbCr & "Type : 0x" & Hex(retdevClass(i)))
                ToLog("The device " & dvcNameArray(i) & " --> " & DeviceClass & " is found !")
                RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;The device " & dvcNameArray(i) & " --> " & DeviceClass & " is found !", False, True)
                Select Case BlueSoleil_DevicePairing_IsDevicePaired(dvcHandleArray(i))
                    Case True
                        IconDeviceType = SkinPath & "Indicators\BT_Paired.png"
                    Case False
                        IconDeviceType = SkinPath & "Indicators\BT_UnPaired.png"
                End Select
                If buildList = True Then
                    If DeviceClass <> "Unknow" Then
                        AddCustomList(MainPath & "MobilePhone_DevicesList.lst", BlueSoleil_GetRemoteDeviceAddress(dvcHandleArray(i)), dvcNameArray(i) & " --> " & DeviceClass, IconDeviceType)
                    Else
                        AddCustomList(MainPath & "MobilePhone_DevicesList.lst", BlueSoleil_GetRemoteDeviceAddress(dvcHandleArray(i)), dvcNameArray(i) & " --> " & "&H" & Right(Hex(dvcHandleArray(i).ToString), 3), IconDeviceType)
                    End If
                Else
                    PhoneDeviceNameList.Add(dvcNameArray(i))
                    'MsgBox(dvcNameArray(i))
                End If

            Next i

            NumberOfDeviceFound = PhoneDeviceNameList.Count
        End If

        'MsgBox("Done")
    End Sub

    Private Sub btnRefreshAllDevices(buildList As Boolean)
        ToLog("Paired or not Devices list is on !")

        BlueSoleil_GetAllDevices_NamesAndHandles(dvcNameArray, dvcHandleArray, dvcArrayCount)
        If dvcArrayCount = 0 Then
            BlueSoleil_GetInquiredDevices_NamesAndHandles(dvcNameArray, dvcHandleArray, dvcArrayCount)
        End If

        Dim retdevClass(0 To 0) As UInteger
        Dim i As Integer
        PhoneDeviceNameList.Clear()
        If dvcArrayCount > 0 Then
            ReDim dvcNameArray(0 To dvcArrayCount - 1)
            ReDim retdevClass(0 To dvcArrayCount - 1)
            Dim DeviceClass As String
            Dim DeviceMacAdd As String
            ReDim Preserve dvcInfos(0 To dvcArrayCount - 1)
            dvcInfosCount = dvcArrayCount
            For i = 0 To dvcArrayCount - 1
                dvcNameArray(i) = BlueSoleil_GetRemoteDeviceName(dvcHandleArray(i))
                BlueSoleil_GetRemoteDeviceClass(dvcHandleArray(i), retdevClass(i)) 'Btsdk_GetRemoteDeviceClass(dvcHandleArray(i), retdevClass(i))
                DeviceClass = ClassVal2String(retdevClass(i))
                DeviceMacAdd = BlueSoleil_GetRemoteDeviceAddress(dvcHandleArray(i))
                'MsgBox("Name: " & retDvcNames(i) & vbCr & "Type : 0x" & Hex(retdevClass(i)))
                ToLog("The device " & dvcNameArray(i) & " --> " & DeviceClass & " is found !")
                RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;The device " & dvcNameArray(i) & " --> " & DeviceClass & " is found !", False, True)

                dvcInfos(i).dvcHandle = dvcHandleArray(i)
                dvcInfos(i).dvcMac = DeviceMacAdd
                dvcInfos(i).dvcName = dvcNameArray(i)
                dvcInfos(i).dvcIsPaired = BlueSoleil_DevicePairing_IsDevicePaired(dvcHandleArray(i))
                Select Case BlueSoleil_DevicePairing_IsDevicePaired(dvcHandleArray(i))
                    Case True
                        IconDeviceType = SkinPath & "Indicators\BT_Paired.png"
                    Case False
                        IconDeviceType = SkinPath & "Indicators\BT_UnPaired.png"
                End Select

                If buildList = True Then
                    If DeviceClass <> "Unknow" Then
                        AddCustomList(MainPath & "MobilePhone_DevicesList.lst", DeviceMacAdd, dvcNameArray(i) & " --> " & DeviceClass, IconDeviceType)
                    Else
                        AddCustomList(MainPath & "MobilePhone_DevicesList.lst", DeviceMacAdd, dvcNameArray(i) & " --> " & "&H" & Right(Hex(dvcHandleArray(i).ToString), 3), IconDeviceType)
                    End If
                Else
                    PhoneDeviceNameList.Add(dvcNameArray(i))
                    'MsgBox(dvcNameArray(i))
                End If

            Next i

            NumberOfDeviceFound = PhoneDeviceNameList.Count
        End If

        BlueSoleil_DiscoverDevices_Start(dvcHandleArray, dvcArrayCount)
        'MsgBox("Done")

    End Sub

    Public Sub PinCodeAdd(hdl As UInt32, pincode As String)
        Btsdk_PinCodeReply(hdl, pincode, pincode.Length)
    End Sub

    Public Sub btnPairing_Pair(CLDESC As String)
        btnRefreshAllDevices(True) 'btnPairing_GetDevices.PerformClick()
        Try
            If CLDESC = "" Then 'lvwPairing_DeviceList.SelectedItems.Count = 0 Then
                MsgBox("Select a device in the list.")
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Select a device in the list.")
            Exit Sub

        End Try

        Dim dvcHandle As UInt32

        For d As Integer = 0 To dvcInfosCount - 1
            If dvcInfos(d).dvcName = CLDESC Then
                dvcHandle = dvcInfos(d).dvcHandle
                MessageBox.Show("Name: " & dvcInfos(d).dvcName & vbCrLf & "Handle: " & dvcInfos(d).dvcHandle)
                Exit For
            End If
        Next

        Dim TorF As Boolean = BlueSoleil_DevicePairing_PairDevice(dvcHandle) 'BlueSoleil_DevicePairing_PairDevicebyPinCode(dvcHandle, TempPluginSettings.PhonePinCode) 

        MsgBox("Pair Device '" & CLDESC & "'- Return value = " & TorF & ".  Click OK to refresh the list.")

        btnRefreshAllDevices(True) 'btnPairing_GetDevices.PerformClick()

    End Sub

    Public Sub btnPairing_PairByPincode(CLDESC As String, Optional pincode As String = "")
        btnRefreshAllDevices(True) 'btnPairing_GetDevices.PerformClick()
        Try
            If CLDESC = "" Then 'lvwPairing_DeviceList.SelectedItems.Count = 0 Then
                MsgBox("Select a device in the list.")
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Select a device in the list.")
            Exit Sub

        End Try

        Dim dvcHandle As UInt32

        For d As Integer = 0 To dvcInfosCount - 1
            If dvcInfos(d).dvcName = CLDESC Then
                dvcHandle = dvcInfos(d).dvcHandle
                MessageBox.Show("Name: " & dvcInfos(d).dvcName & vbCrLf & "Handle: " & dvcInfos(d).dvcHandle)
                Exit For
            End If
        Next


        Dim TorF As Boolean = BlueSoleil_DevicePairing_PairDevicebyPinCode(dvcHandle, If(pincode <> "", pincode, TempPluginSettings.PhonePinCode))

        MsgBox("Pair Device '" & CLDESC & "'- Return value = " & TorF & ".  Click OK to refresh the list.")

        btnRefreshAllDevices(True) 'btnPairing_GetDevices.PerformClick()

    End Sub

    Public Sub btnPairing_UnPair(CLDESC As String)
        btnRefreshAllDevices(True) 'btnPairing_GetDevices.PerformClick()
        Try
            If CLDESC = "" Then 'lvwPairing_DeviceList.SelectedItems.Count = 0 Then
                MsgBox("Select a device in the list.")
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Select a device in the list.")
            Exit Sub

        End Try

        Dim dvcHandle As UInt32

        For d As Integer = 0 To dvcInfosCount - 1
            If dvcInfos(d).dvcName = CLDESC Then
                dvcHandle = dvcInfos(d).dvcHandle
                'MessageBox.Show("Name: " & dvcInfos(d).dvcName & vbCrLf & "Handle: " & dvcInfos(d).dvcHandle)
                If BlueSoleil_IsDeviceConnected(dvcInfos(d).dvcHandle) = True Then
                    'disconnect all services
                    'BlueSoleil_DisconnectServiceConns_ByName("HFP")
                    BlueSoleil_DisconnectServiceConns_ByName("MAP")
                    BlueSoleil_DisconnectServiceConns_ByName("PAN")
                    BlueSoleil_DisconnectServiceConns_ByName("PBAP")
                    BlueSoleil_DisconnectServiceConns_ByName("SPP")
                    BlueSoleil_DisconnectServiceConns_ByName("AVRCP")
                    BlueSoleil_DisconnectServiceConns_ByName("HSP")
                    BlueSoleil_DisconnectServiceConns_ByName("A2DP")
                    btnHandsFreeDisconnect()
                End If
                Exit For
            End If
        Next

        Dim TorF As Boolean = BlueSoleil_DevicePairing_UnpairDevice(dvcHandle)

        MsgBox("Un-Pair Device '" & CLDESC & "'- Return value = " & TorF & ".  Click OK to refresh the list.")

        btnRefreshAllDevices(True) 'btnPairing_GetDevices.PerformClick()

    End Sub

    Public Sub btnDeleteDvc(CLDESC As String)
        btnRefreshAllDevices(True) 'btnPairing_GetDevices.PerformClick()
        Try
            If CLDESC = "" Then 'lvwPairing_DeviceList.SelectedItems.Count = 0 Then
                MsgBox("Select a device in the list.")
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Select a device in the list.")
            Exit Sub

        End Try

        Dim dvcHandle As UInt32

        For d As Integer = 0 To dvcInfosCount - 1
            If dvcInfos(d).dvcName = CLDESC Then
                dvcHandle = dvcInfos(d).dvcHandle
                MessageBox.Show("Name: " & dvcInfos(d).dvcName & vbCrLf & "Handle: " & dvcInfos(d).dvcHandle)
                Exit For
            End If
        Next

        Dim TorF As Boolean = BlueSoleil_DevicePairing_DeleteDevice(dvcHandle)

        MsgBox("Delete Device - Return value = " & TorF & ".  Click OK to refresh the list.")

        btnRefreshAllDevices(True) 'btnPairing_GetDevices.PerformClick()

    End Sub

#End Region

#Region "Timers"
    Public Sub RequestInfos() 'read each 3s by timer1
        'load contact from the pb.vcf file
        If pbapListNeedUpdate = True Then
            Bluesoleil_LoadContactsFile()
            pbapListNeedUpdate = False
        End If

        'check birthday contacts
        If PhoneBookEntryCount > 0 Then
            If BirthdayAlreadyChecked = False Then
                BlueSoleil_CheckBirthDay()
                BirthdayAlreadyChecked = True
            Else
                If Now.Day <> startPluginTime.Day Then 'if date change, a new bd check is done
                    startPluginTime = Now
                    BirthdayAlreadyChecked = False
                    ToLog("Date is different, new birthday check is done")
                End If
            End If
        Else
            ToLog("Birthday not passed (PhoneBookEntryCount = 0)")
        End If


        '********** SMS Check *****************
        If LoadMAPService = True Then
            If IncomingCall = False Then 'check map is stop if during call in 
                If SmsUnreadOnly = True Then 'doesn't check unread if ALL read SMS is used !
                    If hfpCallIsActive = False Then 'not check sms during a call
                        If timerMapCounter > 20 Then '(checked each 20x3s=60s)
                            If (mapHandleConn <> 0) = True Then
                                Dim t As New Threading.Thread(Sub() btnGetMessages(True)) 'check new unread msg
                                t.Start()
                            End If
                            timerMapCounter = 0
                        End If
                        timerMapCounter += 1 'check if the timerMapCounter is active
                    End If
                End If
            Else
                If IncomingCall = True Then ToLog("IncomingCall stop SMS check")
            End If
        End If
        '********** SMS Check *****************

        '********** Wifi/Lan Check ****************
        If IncomingCall = False Then 'check network is stop if during call in 
            If TempPluginSettings.PhonePANAutoRun = True And ServicePAN_Usable = True Then
                If timerNetworkCounter > 3 Then '(checked each 3x3s=9s)
                    'Dim startTime As DateTime = Now
                    Dim retInet As Boolean = False
                    retInet = INet_GetOnlineStatus()
                    Thread.Sleep(500)
                    If retInet = True And panIsActive = True Then
                        Net_GetUpDownStats(panGetUpDownStatsBytesSent, panGetUpDownStatsBytesReceived)
                        ToLog("Sent bytes: " & panGetUpDownStatsBytesSent)
                        ToLog("Received bytes: " & panGetUpDownStatsBytesReceived)

                        'add check if wifi or lan is available (if yes, PAN is stopped)
                        Net_CheckBetterNetwork()
                    End If
                    If retInet = False And panIsActive = False Then
                        Dim t As New Threading.Thread(Sub() btnTether()) 'run PAN service
                        t.Start()
                    End If
                    timerNetworkCounter = 0

                End If
                timerNetworkCounter += 1
            End If
        Else
            If IncomingCall = True Then ToLog("IncomingCall stop Wifi check")
        End If
        '********** Wifi/Lan Check ****************

        If hfpVoiceCmdStateEnabled = True Then Exit Sub

        If dvcCurrHandle <> 0 Then 'fonctionne que si btnRefreshDevices() a été lancé
            Dim currDvcLinkPct As Double = BlueSoleil_GetRemoteLinkQualityPct(dvcCurrHandle)
            lblDeviceLink = "Link: " & Format(currDvcLinkPct, "#0") & "%"

            Dim currDvcRSSIdb As Double = BlueSoleil_GetRemoteRSSI_Decibles(dvcCurrHandle)
            lblDeviceRSSI = "RSSI: " & currDvcRSSIdb & " db"

            'using ZERO for the device handle here to get the statistics of the local device, since the remote device stats were unreliable.
            Dim currDvcBytesRcvd As UInt32, currDvcBytesSent As UInt32
            BlueSoleil_GetRemoteLinkDataStatistics(0, currDvcBytesRcvd, currDvcBytesSent)
            lblDeviceRcvd = "Rcvd: " & Format(currDvcBytesRcvd, "###,###,###,##0")
            lblDeviceSent = "Sent: " & Format(currDvcBytesSent, "###,###,###,##0")

        End If

        'lblSerialCOMMport.Text = "Port:  COM " & sppCOMaddress

        'AVRCP update infos
        If avrcpHandleDvc <> 0 And avrcpIsActive = True Then
            AVRCP_lblAValbum = avrcpTrackAlbum
            AVRCP_lblAVartist = avrcpTrackArtist
            AVRCP_lblAVtitle = avrcpTrackTitle
            If avrcpTrackPos > 0 And avrcpTrackLen > 0 Then
                'calculate the current position of the track, based on last known trackpos, and time since update.
                Dim currTicks As Long = (DateTime.UtcNow.Ticks \ TimeSpan.TicksPerMillisecond)
                Dim secondsSincePosUpdate As Double = (currTicks - avrcpTrackPosUpdateTicks) / 1000
                If avrcpTrackPosUpdateTicks = 0 Or avrcpIsPlaying = False Then secondsSincePosUpdate = 0

                Dim posSpan As New TimeSpan(0, 0, CInt(avrcpTrackPos + secondsSincePosUpdate))
                Dim lenSpan As New TimeSpan(0, 0, CInt(avrcpTrackLen))
                'lblMediaPos.Text = "Pos: " & posSpan.Minutes & ":" & Format(posSpan.Seconds, "00") & " / " & lenSpan.Minutes & ":" & Format(lenSpan.Seconds, "00")
                AVRCP_lblAVtrackpos = posSpan.Minutes & ":" & Format(posSpan.Seconds, "00")
                AVRCP_lblAVtracklen = lenSpan.Minutes & ":" & Format(lenSpan.Seconds, "00")
            Else
                If avrcpIsPlaying = True Then
                    AVRCP_lblAValbum = "?"
                    AVRCP_lblAVartist = "?"
                    AVRCP_lblAVtitle = "?"
                End If
            End If
            If avrcpNeedsTrackInfo = True Then
                avrcpNeedsTrackInfo = False
                BlueSoleil_AVRCP_SendReq_GetElementInfo(avrcpHandleDvc)
            End If
            If avrcpNeedsPlayStatus = True Then
                avrcpNeedsPlayStatus = False
                BlueSoleil_AVRCP_SendReq_GetPlayStatus(avrcpHandleDvc)
            End If

            If avrcpNeedsSupportedEvents = True Then
                avrcpNeedsSupportedEvents = False
                BlueSoleil_AVRCP_SendReq_GetCapabilities_SupportedEvents(avrcpHandleDvc)
                Application.DoEvents()
                Exit Sub
            End If

            'BlueSoleil_AVRCP_SendReq_GetPlayerSettings(avrcpHandleDvc)

        End If


        'check if the phone connected is found in car and answer
        hfpDeviceIsActive = BlueSoleil_GetConnectionProperties(hfpHandleConnHFAG)
        If hfpDeviceIsActive = False And hfpHandleConnHFAG <> 0 Then 'if active is false but connection handle not zero
            btnHandsFreeDisconnect() 'zero the connection handle and hfpIsActive = False
            If timer2.Enabled = False Then
                ClearHFPvariables()
                RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE enter in the WAIT mode (30s)", False, False)
                ToLog("MOBILEPHONE enter in the WAIT mode (30s)")
                timer2.Enabled = True 'start the timer.
                timer2.Start()
            End If
        End If


        'HFP update infos
        If hfpHandleConnHFAG <> 0 And hfpVoiceCmdStateEnabled = False Then

            '********** Check if external power is connected *****************
            If timerBatteryCounter > 5 Then '(3x5s)
                RunUSBWatcher()
                If ExternalPowerIsConnected = True Then
                    If ExternalPowerStatus = False Then RaiseEvent BlueSoleil_Event_HFP_ExtPowerBattON()
                    ExternalPowerIsConnected = True
                Else
                    If ExternalPowerStatus = True Then RaiseEvent BlueSoleil_Event_HFP_ExtPowerBattOFF()
                    ExternalPowerIsConnected = False
                End If
                timerBatteryCounter = 0
            End If
            timerBatteryCounter += 1 'check if the timerBatteryCounter is active
            '********** Check if external power is connected *****************

            'hfpBatteryPct = hfpBatteryPct 'Format(hfpBatteryPct, "#0") & "%"
            If hfpBatteryPct >= 98 Then 'If hfpBatteryPct / 5 * 100 >= 100 Then
                If BatteryFullCharge = False Then RaiseEvent BlueSoleil_Event_HFP_BatteryIsFull()
                BatteryFullCharge = True
            End If

            'lblHandsFreeStatus.Text = "Status:  " & hfpStatusStr
            'lblHandsFreeBattery.Text = "Battery:  " & Format(hfpBatteryPct, "#0") & "%"
            'lblHandsFreeSignal.Text = "Signal:  " & Format(hfpSignalPct, "#0") & "%"

            'lblHandsFreeIncomingNo.Text = "Caller ID:  " & hfpCallerIDno & " " & hfpCallerIDname
            'lblHandsFreeYourNo.Text = "Your No:  " & hfpSubscriberNo & " " & hfpSubscriberName
            'lblHandsFreeNetwork.Text = "Network:  " & hfpNetworkName
            'lblHandsFreePhoneType.Text = "Phone:  " & hfpManufacturerName & " " & tempModelName
            'lblHandsFreeRoaming.Text = "Roaming:  " & hfpIsRoaming

            'If TempPluginSettings.PhoneExecATCmd = True Then
            'non accepté par Galaxy A5 (2016)
            'BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CBC", 500) 'check if external power is connected and battery charge (replaced by hfpBatteryPct)
            'BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CMER?", 500)
            'BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CIMI", 500) 'lecture N°ISMI
            'BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CCLK?", 500) 'return date  and time from phone

            'accepté par Galaxy A5 (2016)
            'BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CGSN", 500) 'lecture N°IMEI
            'BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CSQ", 500) 'return signal value
            BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CIND?", 500) 'return signal and battery charging mode ( and other phone status) D: 0,0,1,0,0,3,0
            '+CIND: ("CALL",(0,1)),("CALLSETUP",(0-3)),("SERVICE",(0-1)),("SIGNAL",(0-5)),("ROAM",(0,1)),("BATTCHG",(0-5)),("CALLHELD",(0-2))
            '+CIND: 0,0,1,1,0,3,0
            'End If

            Dim TorF As Boolean
            Dim retStateInt As UInt16, PhoneStatusString As String
            TorF = BlueSoleil_HFP_GetState(hfpHandleConnHFAG, retStateInt)
            If TorF = True Then
                PhoneStatusString = BlueSoleil_HFP_GetHFPstateDesc(retStateInt)
            Else
                PhoneStatusString = "Unknown"
            End If

            'only one request per timer tick.
            Select Case hfpRequestCounter Mod 10
                Case 0
                    'BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CSQ", 1000)     'probably not required
                    'Application.DoEvents()
                Case 1
                    'BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CBC", 1000)     'probably not required
                    'Application.DoEvents()
                Case 2
                    If hfpSubscriberNo = "" Then
                        BlueSoleil_HFP_SendRequest_GetSubscriberNumber(hfpHandleConnHFAG)
                        'Application.DoEvents()
                    End If
                Case 3
                    If hfpNetworkName = "" Then
                        BlueSoleil_HFP_SendRequest_GetNetworkOperator(hfpHandleConnHFAG)
                        'Application.DoEvents()
                    End If
                Case 4
                    If hfpManufacturerName = "" Then
                        BlueSoleil_HFP_GetManufacturer(hfpHandleConnHFAG, hfpManufacturerName)
                        'Application.DoEvents()
                    End If
                Case 5
                    If hfpModelName = "" Then
                        BlueSoleil_HFP_GetModel(hfpHandleConnHFAG, hfpManufacturerName)
                        'Application.DoEvents()
                    End If
                Case 6
                    If hfpManufacturerName = "" Then
                        BlueSoleil_HFP_SendATcmd(hfpHandleConnHFAG, "AT+CGMI", 1000)
                    End If
                    hfpRequestCounter = -1
            End Select

            hfpRequestCounter = hfpRequestCounter + 1

        End If

        If timerPluginTimeStr > 20 Then 'check each 1 minute time elapsed from start
            startPluginTimeStr = Now.Subtract(startPluginTime).Hours.ToString & "h:" & Now.Subtract(startPluginTime).Minutes.ToString & "m:" & Now.Subtract(startPluginTime).Seconds.ToString & "s"
            ToLog("The plugin operating for " & startPluginTimeStr)
            timerPluginTimeStr = 0
        End If
        timerPluginTimeStr += 1
    End Sub

    Public Sub SwapDevicePhone()
        If TempPluginSettings.PhoneDeviceName <> "NONAME" AndAlso TempPluginSettings.PhoneDeviceName2 <> "NONAME" AndAlso TempPluginSettings.AutoSwapPhone = True Then
            RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;MOBILEPHONE try swap phone", False, True)
            ToLog("MOBILEPHONE try swap phone")
            btnHandsFreeDisconnect()
            Thread.Sleep(1000)
            If PhoneCheckedIs = 1 Then
                PhoneCheckedIs = 2
            ElseIf PhoneCheckedIs = 2 Then
                PhoneCheckedIs = 1
            End If

            Dim startTime As DateTime = Now
            Do 'initiate now the HFP service
                Threading.Thread.Sleep(1000)
                ToLog("MOBILEPHONE HFP service not Ready, i wait 1 second")
                If Now.Subtract(startTime).TotalSeconds > 5 Then 'TempPluginSettings.PhoneStartupTimer Then
                    If btnHandsFreeConnect(PhoneCheckedIs) = True Then 'accept only 1 (phone 1) or 2 (phone 2)
                        RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;MOBILEPHONE HFP service is Ready", False, True)
                        ToLog("MOBILEPHONE HFP service is Ready")
                        Exit Do
                    End If
                End If
            Loop

            If hfpIsActive = True Then
                'MessageBox.Show("OK")
                If PluginRunForDS = False Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONE_PHONE" & PhoneCheckedIs & "FOUND", False, True)
                Else
                    RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONE_PHONE" & PhoneCheckedIs & "FOUND", False, True)
                End If
                ToLog("Phone" & PhoneCheckedIs & "found")
                'DetectedDevice = True
                If timer2.Enabled = True Then
                    timer2.Stop()
                    timer2.Enabled = False 'stop check presence device
                End If
                If timer1.Enabled = False Then
                    timer1.Enabled = True 'stop update info
                    timer1.Start()
                    RaiseEvent BlueSoleil_Event_ERROR_Return("Main timer is restarted (3s)", False, False)
                    ToLog("Main timer is restarted")
                End If
                Exit Sub
            Else
                'MessageBox.Show("No Phone Found !")
                If PluginRunForDS = False Then
                    RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONE_PHONE" & PhoneCheckedIs & "NOTFOUND", False, True)
                Else
                    RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONE_PHONE" & PhoneCheckedIs & "NOTFOUND", False, True)
                End If
                ToLog("Phone" & PhoneCheckedIs & "not found")
            End If
        Else
            RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;MOBILEPHONE isn't ready for SWAP (check your settings)", False, True)
            ToLog("MOBILEPHONE isn't ready for SWAP (check your settings)")
        End If

    End Sub
    Private Sub CkeckDevicePresence()
        If PluginRunForDS = False Then
            RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONE_CHECKPHONEPRESENCE", False, True)
        Else
            RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONE_CHECKPHONEPRESENCE", False, True)
        End If
        ToLog("CHECKPHONEPRESENCE is active")
        If hfpIsActive = False Then

            'Dim retBool As Boolean = False
            btnRefreshDevices(False)
            Dim p As Integer = 0

            ' try to reconnect any of two phones in settings
            If TempPluginSettings.PhoneDeviceName <> "NONAME" AndAlso TempPluginSettings.PhoneDeviceName2 <> "NONAME" AndAlso TempPluginSettings.AutoSwapPhone = True Then
                For p = 0 To PhoneDeviceNameList.Count - 1
                    'MessageBox.Show(PhoneDeviceNameList.Item(p))
                    Dim startTime As DateTime = Now
                    Do 'initiate now the HFP service
                        Threading.Thread.Sleep(1000)
                        ToLog("BlueSoleil HFP service not Ready, i wait 1 second")
                        If Now.Subtract(startTime).TotalSeconds > 5 Then 'TempPluginSettings.PhoneStartupTimer Then
                            If btnHandsFreeConnect(p + 1) = True Then 'accept only 1 (phone 1) or 2 (phone 2)
                                RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;BlueSoleil HFP service is Ready", False, True)
                                ToLog("BlueSoleil HFP service is Ready")
                                PhoneCheckedIs = p + 1
                                Exit Do
                            End If
                        End If
                    Loop

                    'MessageBox.Show(retBool)
                    If hfpIsActive = True Then
                        'MessageBox.Show("OK")
                        If PluginRunForDS = False Then
                            RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONE_PHONE" & p + 1 & "FOUND", False, True)
                        Else
                            RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONE_PHONE" & p + 1 & "FOUND", False, True)
                        End If
                        ToLog("Phone" & p + 1 & "found")
                        'DetectedDevice = True
                        If timer2.Enabled = True Then
                            timer2.Stop()
                            timer2.Enabled = False 'stop check presence device
                        End If
                        If timer1.Enabled = False Then
                            timer1.Enabled = True 'stop update info
                            timer1.Start()
                            RaiseEvent BlueSoleil_Event_ERROR_Return("Main timer is restarted", False, False)
                            ToLog("Main timer is restarted")
                        End If
                        Exit Sub
                    Else
                        'MessageBox.Show("No Phone Found !")
                        If PluginRunForDS = False Then
                            RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONE_PHONE" & p + 1 & "NOTFOUND", False, True)
                        Else
                            RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONE_PHONE" & p + 1 & "NOTFOUND", False, True)
                        End If
                        ToLog("Phone" & p + 1 & "not found")
                    End If
                Next

            Else ' try to reconnect always same phone
                Dim startTime As DateTime = Now
                Do 'initiate now the HFP service
                    Threading.Thread.Sleep(1000)
                    ToLog("BlueSoleil HFP service not Ready, i wait 1 second")
                    If Now.Subtract(startTime).TotalSeconds > 5 Then 'TempPluginSettings.PhoneStartupTimer Then
                        If btnHandsFreeConnect(PhoneCheckedIs) = True Then 'accept only 1 (phone 1) or 2 (phone 2)
                            RaiseEvent BlueSoleil_Event_ERROR_Return("MOBILEPHONE_RRLOG;BlueSoleil HFP service is Ready", False, True)
                            ToLog("BlueSoleil HFP service is Ready")
                            Exit Do
                        End If
                    End If
                Loop

                'MessageBox.Show(retBool)
                If hfpIsActive = True Then
                    'MessageBox.Show("OK")
                    If PluginRunForDS = False Then
                        RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONE_PHONE" & PhoneCheckedIs & "FOUND", False, True)
                    Else
                        RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONE_PHONE" & PhoneCheckedIs & "FOUND", False, True)
                    End If
                    ToLog("Phone" & PhoneCheckedIs & "found")
                    'DetectedDevice = True
                    If timer2.Enabled = True Then
                        timer2.Stop()
                        timer2.Enabled = False 'stop check presence device
                    End If
                    If timer1.Enabled = False Then
                        timer1.Enabled = True 'stop update info
                        timer1.Start()
                        RaiseEvent BlueSoleil_Event_ERROR_Return("Main timer is restarted", False, False)
                        ToLog("Main timer is restarted")
                    End If
                    Exit Sub
                Else
                    'MessageBox.Show("No Phone Found !")
                    If PluginRunForDS = False Then
                        RaiseEvent BlueSoleil_Event_ERROR_Return("*ONMOBILEPHONE_PHONE" & PhoneCheckedIs & "NOTFOUND", False, True)
                    Else
                        RaiseEvent BlueSoleil_Event_ERROR_Return("ONMOBILEPHONE_PHONE" & PhoneCheckedIs & "NOTFOUND", False, True)
                    End If
                    ToLog("Phone" & PhoneCheckedIs & "not found")
                End If
            End If

        End If


    End Sub


#End Region

#Region "Dictionnaries"
    Public Sub CreateServicesList()
        ServiceList.Add("1101", "Serial Port service.")
        ServiceList.Add("1102", "LAN Access service.")
        ServiceList.Add("1103", "Dial-up Networking service.")
        ServiceList.Add("1104", "Synchronization service.")
        ServiceList.Add("1105", "Object Push service.")
        ServiceList.Add("1106", "File Transfer service.")
        ServiceList.Add("1107", "IrMC Sync Command service.")
        ServiceList.Add("1108", "Headset service.")
        ServiceList.Add("1109", "Cordless Telephony service.")
        ServiceList.Add("110A", "Audio Source service.")
        ServiceList.Add("110B", "Audio Sink service.")
        ServiceList.Add("110C", "A/V Remote Control Target service.")
        ServiceList.Add("110D", "Advanced Audio Distribution service.")
        ServiceList.Add("110E", "A/V Remote Control service.")
        ServiceList.Add("110F", "Video conference service.")
        ServiceList.Add("1110", "Intercom service.")
        ServiceList.Add("1111", "Fax service.")
        ServiceList.Add("1112", "Headset Audio Gateway service.")
        ServiceList.Add("1113", "WAP service.")
        ServiceList.Add("1114", "WAP client service.")
        ServiceList.Add("1115", "PANU service.")
        ServiceList.Add("1116", "NAP service.")
        ServiceList.Add("1117", "GN service.")
        ServiceList.Add("1118", "Direct Print service.")
        ServiceList.Add("1119", "Referenced Print service.")
        ServiceList.Add("111A", "Imaging service.")
        ServiceList.Add("111B", "Imaging Responder service.")
        ServiceList.Add("111C", "Imaging Automatic Archive service.")
        ServiceList.Add("111D", "Imaging Referenced Objects service.")
        ServiceList.Add("111E", "Hands-free service.")
        ServiceList.Add("111F", "Hands-free Audio Gateway service.")
        ServiceList.Add("1120", "DPS Referenced Objects service.")
        ServiceList.Add("1121", "Reflected UI service.")
        ServiceList.Add("1122", "Basic Print service.")
        ServiceList.Add("1123", "Print Status service.")
        ServiceList.Add("1124", "Human Interface Device service.")
        ServiceList.Add("1125", "Hardcopy Cable Replacement service.")
        ServiceList.Add("1126", "HCRP Print service.")
        ServiceList.Add("1127", "HCRP Scan service.")
        ServiceList.Add("112D", "SIM Card Access service.")
        ServiceList.Add("112E", "PBAP Phonebook Client Equipment service.")
        ServiceList.Add("112F", "PBAP Phonebook Server Equipment service.")
        ServiceList.Add("1130", "Phonebook Access service.")
        ServiceList.Add("1132", "MAP service")
        ServiceList.Add("1200", "Bluetooth Device Identification.")

        'http://bluetooth-pentest.narod.ru/doc/assigned_numbers_-_service_discovery.html
        ServiceList.Add("1201", "GenericNetworking")
        ServiceList.Add("1202", "GenericFileTransfer")
        ServiceList.Add("1203", "GenericAudio")
        ServiceList.Add("1204", "GenericTelephony")
        ServiceList.Add("1205", "UPNP_Service [ESDP] and possible future profiles")
        ServiceList.Add("1206", "UPNP_IP_Service [ESDP] and possible future profiles")
        ServiceList.Add("1300", "ESDP_UPNP_IP_PAN	[ESDP]")
        ServiceList.Add("1301", "ESDP_UPNP_IP_LAP [ESDP]")
        ServiceList.Add("1302", "ESDP_UPNP_L2CAP [ESDP]")
        ServiceList.Add("1303", "VideoSource See Video Distribution Profile (VDP), Bluetooth SIG")
        ServiceList.Add("1304", "VideoSink See Video Distribution Profile (VDP), Bluetooth SIG")
        ServiceList.Add("1305", "VideoDistribution 	Video Distribution Profile (VDP), Bluetooth SIG")

        ServiceList.Add("1800", "GAP (Generic Access Profile)")
        ServiceList.Add("1801", "GATT (Generic Attributes)")

    End Sub


    Public Sub CreateCountryList()
        CountryList.Add("Afghanistan", 93)
        CountryList.Add("Albania", 355)
        CountryList.Add("Algeria", 213)
        CountryList.Add("American Samoa", 1684)
        CountryList.Add("Andorra", 376)
        CountryList.Add("Angola", 244)
        CountryList.Add("Anguilla", 1264)
        CountryList.Add("Antarctica", 672)
        CountryList.Add("Antigua and Barbuda", 1268)
        CountryList.Add("Argentina", 54)
        CountryList.Add("Armenia", 374)
        CountryList.Add("Aruba", 297)
        CountryList.Add("Australia", 61)
        CountryList.Add("Austria", 43)
        CountryList.Add("Azerbaijan", 994)
        CountryList.Add("Bahamas", 1242)
        CountryList.Add("Bahrain", 973)
        CountryList.Add("Bangladesh", 880)
        CountryList.Add("Barbados", 1246)
        CountryList.Add("Belarus", 375)
        CountryList.Add("Belgium", 32)
        CountryList.Add("Belize", 501)
        CountryList.Add("Benin", 229)
        CountryList.Add("Bermuda", 1441)
        CountryList.Add("Bhutan", 975)
        CountryList.Add("Bolivia", 591)
        CountryList.Add("Bosnia and Herzegovina", 387)
        CountryList.Add("Botswana", 267)
        CountryList.Add("Brazil", 55)
        'CountryList.Add("British Indian Ocean Territory", )
        CountryList.Add("British Virgin Islands", 1284)
        CountryList.Add("Brunei", 673)
        CountryList.Add("Bulgaria", 359)
        CountryList.Add("Burkina Faso", 226)
        CountryList.Add("Burma (Myanmar)", 95)
        CountryList.Add("Burundi", 257)
        CountryList.Add("Cambodia", 855)
        CountryList.Add("Cameroon", 237)
        CountryList.Add("Canada", 1)
        CountryList.Add("Cape Verde", 238)
        CountryList.Add("Cayman Islands", 1345)
        CountryList.Add("Central African Republic", 236)
        CountryList.Add("Chad", 235)
        CountryList.Add("Chile", 56)
        CountryList.Add("China", 86)
        CountryList.Add("Christmas Island", 61)
        CountryList.Add("Cocos (Keeling) Islands", 61)
        CountryList.Add("Colombia", 57)
        CountryList.Add("Comoros", 269)
        CountryList.Add("Cook Islands", 682)
        CountryList.Add("Costa Rica", 506)
        CountryList.Add("Croatia", 385)
        CountryList.Add("Cuba", 53)
        CountryList.Add("Cyprus", 357)
        CountryList.Add("Czech Republic", 420)
        CountryList.Add("Democratic Republic of the Congo", 243)
        CountryList.Add("Denmark", 45)
        CountryList.Add("Djibouti", 253)
        CountryList.Add("Dominica", 1767)
        CountryList.Add("Dominican Republic", 1809)
        CountryList.Add("Ecuador", 593)
        CountryList.Add("Egypt", 20)
        CountryList.Add("El Salvador", 503)
        CountryList.Add("Equatorial Guinea", 240)
        CountryList.Add("Eritrea", 291)
        CountryList.Add("Estonia", 372)
        CountryList.Add("Ethiopia", 251)
        CountryList.Add("Falkland Islands", 500)
        CountryList.Add("Faroe Islands", 298)
        CountryList.Add("Fiji", 679)
        CountryList.Add("Finland", 358)
        CountryList.Add("France", 33)
        CountryList.Add("French Polynesia", 689)
        CountryList.Add("Gabon", 241)
        CountryList.Add("Gambia", 220)
        CountryList.Add("Gaza Strip", 970)
        CountryList.Add("Georgia", 995)
        CountryList.Add("Germany", 49)
        CountryList.Add("Ghana", 233)
        CountryList.Add("Gibraltar", 350)
        CountryList.Add("Greece", 30)
        CountryList.Add("Greenland", 299)
        CountryList.Add("Grenada", 1473)
        CountryList.Add("Guam", 1671)
        CountryList.Add("Guatemala", 502)
        CountryList.Add("Guinea", 224)
        CountryList.Add("Guinea-Bissau", 245)
        CountryList.Add("Guyana", 592)
        CountryList.Add("Haiti", 509)
        CountryList.Add("Holy See (Vatican City)", 39)
        CountryList.Add("Honduras", 504)
        CountryList.Add("Hong Kong", 852)
        CountryList.Add("Hungary", 36)
        CountryList.Add("Iceland", 354)
        CountryList.Add("India", 91)
        CountryList.Add("Indonesia", 62)
        CountryList.Add("Iran", 98)
        CountryList.Add("Iraq", 964)
        CountryList.Add("Ireland", 353)
        CountryList.Add("Isle of Man", 44)
        CountryList.Add("Israel", 972)
        CountryList.Add("Italy", 39)
        CountryList.Add("Ivory Coast", 225)
        CountryList.Add("Jamaica", 1876)
        CountryList.Add("Japan", 81)
        'CountryList.Add("Jersey", )
        CountryList.Add("Jordan", 962)
        CountryList.Add("Kazakhstan", 7)
        CountryList.Add("Kenya", 254)
        CountryList.Add("Kiribati", 686)
        CountryList.Add("Kosovo", 381)
        CountryList.Add("Kuwait", 965)
        CountryList.Add("Kyrgyzstan", 996)
        CountryList.Add("Laos", 856)
        CountryList.Add("Latvia", 371)
        CountryList.Add("Lebanon", 961)
        CountryList.Add("Lesotho", 266)
        CountryList.Add("Liberia", 231)
        CountryList.Add("Libya", 218)
        CountryList.Add("Liechtenstein", 423)
        CountryList.Add("Lithuania", 370)
        CountryList.Add("Luxembourg", 352)
        CountryList.Add("Macau", 853)
        CountryList.Add("Macedonia", 389)
        CountryList.Add("Madagascar", 261)
        CountryList.Add("Malawi", 265)
        CountryList.Add("Malaysia", 60)
        CountryList.Add("Maldives", 960)
        CountryList.Add("Mali", 223)
        CountryList.Add("Malta", 356)
        CountryList.Add("Marshall Islands", 692)
        CountryList.Add("Mauritania", 222)
        CountryList.Add("Mauritius", 230)
        CountryList.Add("Mayotte", 262)
        CountryList.Add("Mexico", 52)
        CountryList.Add("Micronesia", 691)
        CountryList.Add("Moldova", 373)
        CountryList.Add("Monaco", 377)
        CountryList.Add("Mongolia", 976)
        CountryList.Add("Montenegro", 382)
        CountryList.Add("Montserrat", 1664)
        CountryList.Add("Morocco", 212)
        CountryList.Add("Mozambique", 258)
        CountryList.Add("Namibia", 264)
        CountryList.Add("Nauru", 674)
        CountryList.Add("Nepal", 977)
        CountryList.Add("Netherlands", 31)
        CountryList.Add("Netherlands Antilles", 599)
        CountryList.Add("New Caledonia", 687)
        CountryList.Add("New Zealand", 64)
        CountryList.Add("Nicaragua", 505)
        CountryList.Add("Niger", 227)
        CountryList.Add("Nigeria", 234)
        CountryList.Add("Niue", 683)
        CountryList.Add("Norfolk Island", 672)
        CountryList.Add("North Korea", 850)
        CountryList.Add("Northern Mariana Islands", 1670)
        CountryList.Add("Norway", 47)
        CountryList.Add("Oman", 968)
        CountryList.Add("Pakistan", 92)
        CountryList.Add("Palau", 680)
        CountryList.Add("Panama", 507)
        CountryList.Add("Papua New Guinea", 675)
        CountryList.Add("Paraguay", 595)
        CountryList.Add("Peru", 51)
        CountryList.Add("Philippines", 63)
        CountryList.Add("Pitcairn Islands", 870)
        CountryList.Add("Poland", 48)
        CountryList.Add("Portugal", 351)
        CountryList.Add("Puerto Rico", 1)
        CountryList.Add("Qatar", 974)
        CountryList.Add("Republic of the Congo", 242)
        CountryList.Add("Romania", 40)
        CountryList.Add("Russia", 7)
        CountryList.Add("Rwanda", 250)
        CountryList.Add("Saint Barthelemy", 590)
        CountryList.Add("Saint Helena", 290)
        CountryList.Add("Saint Kitts and Nevis", 1869)
        CountryList.Add("Saint Lucia", 1758)
        CountryList.Add("Saint Martin", 1599)
        CountryList.Add("Saint Pierre and Miquelon", 508)
        CountryList.Add("Saint Vincent and the Grenadines", 1784)
        CountryList.Add("Samoa", 685)
        CountryList.Add("San Marino", 378)
        CountryList.Add("Sao Tome and Principe", 239)
        CountryList.Add("Saudi Arabia", 966)
        CountryList.Add("Senegal", 221)
        CountryList.Add("Serbia", 381)
        CountryList.Add("Seychelles", 248)
        CountryList.Add("Sierra Leone", 232)
        CountryList.Add("Singapore", 65)
        CountryList.Add("Slovakia", 421)
        CountryList.Add("Slovenia", 386)
        CountryList.Add("Solomon Islands", 677)
        CountryList.Add("Somalia", 252)
        CountryList.Add("South Africa", 27)
        CountryList.Add("South Korea", 82)
        CountryList.Add("Spain", 34)
        CountryList.Add("Sri Lanka", 94)
        CountryList.Add("Sudan", 249)
        CountryList.Add("Suriname", 597)
        'CountryList.Add("Svalbard", )
        CountryList.Add("Swaziland", 268)
        CountryList.Add("Sweden", 46)
        CountryList.Add("Switzerland", 41)
        CountryList.Add("Syria", 963)
        CountryList.Add("Taiwan", 886)
        CountryList.Add("Tajikistan", 992)
        CountryList.Add("Tanzania", 255)
        CountryList.Add("Thailand", 66)
        CountryList.Add("Timor-Leste", 670)
        CountryList.Add("Togo", 228)
        CountryList.Add("Tokelau", 690)
        CountryList.Add("Tonga", 676)
        CountryList.Add("Trinidad and Tobago", 1868)
        CountryList.Add("Tunisia", 216)
        CountryList.Add("Turkey", 90)
        CountryList.Add("Turkmenistan", 993)
        CountryList.Add("Turks and Caicos Islands", 1649)
        CountryList.Add("Tuvalu", 688)
        CountryList.Add("Uganda", 256)
        CountryList.Add("Ukraine", 380)
        CountryList.Add("United Arab Emirates", 971)
        CountryList.Add("United Kingdom", 44)
        CountryList.Add("United States", 1)
        CountryList.Add("Uruguay", 598)
        CountryList.Add("US Virgin Islands", 1340)
        CountryList.Add("Uzbekistan", 998)
        CountryList.Add("Vanuatu", 678)
        CountryList.Add("Venezuela", 58)
        CountryList.Add("Vietnam", 84)
        CountryList.Add("Wallis and Futuna", 681)
        CountryList.Add("West Bank", 970)
        'CountryList.Add("Western Sahara", )
        CountryList.Add("Yemen", 967)
        CountryList.Add("Zambia", 260)
        CountryList.Add("Zimbabwe", 263)

    End Sub

    Public Function CountryToPhoneIndicatif(ByVal country As String) As String
        CountryToPhoneIndicatif = ""
        Dim finalstring As String = country.Substring(country.IndexOf("("), (country.LastIndexOf(")") - country.IndexOf("(")) + 1)
        finalstring = finalstring.Replace("(", "").Replace(")", "")
        CountryList.Clear()
        CreateCountryList()
        For Each pair In CountryList
            If pair.Key = finalstring Then CountryToPhoneIndicatif = pair.Value.ToString
        Next
        CountryList.Clear()
        Return CountryToPhoneIndicatif
    End Function

    Public Sub CreateListSDKError()
        SDKErrorList.Add(&H0, "The operation completed successfully.")
        SDKErrorList.Add(&HC0, "Local service is still active. When the application tries to remove or activate an active service, this error code is returned.")
        SDKErrorList.Add(&HC1, "No service record with the specified search pattern is found on the remote device.")
        SDKErrorList.Add(&HC2, "The specified service record does not exist on the remote device.")
        SDKErrorList.Add(&H301, "The object specified by the handle does not exist in local BlueSoleil SDK database.")
        SDKErrorList.Add(&H302, "The operation fails for an undefined reason.")
        SDKErrorList.Add(&H303, "BlueSoleil SDK has not been initialized.")
        SDKErrorList.Add(&H304, "The parameter value is invalid.")
        SDKErrorList.Add(&H305, "The pointer value is NULL.")
        SDKErrorList.Add(&H306, "Not enough storage is available to process this function.")
        SDKErrorList.Add(&H307, "The specified buffer size is too small to hold the required information.")
        SDKErrorList.Add(&H308, "The specified function is not supported by the BlueSoleil.")
        SDKErrorList.Add(&H309, "No fixed PIN code is available.")
        SDKErrorList.Add(&H30A, "The specified service has been connected already.")
        SDKErrorList.Add(&H30B, "The request can.t be processed since a same request is being processed.")
        SDKErrorList.Add(&H30C, "The limit of connection number is reached.")
        SDKErrorList.Add(&H30D, "An object with the specified attribute exists.")
        SDKErrorList.Add(&H30E, "The specified object is accessed by other process. It can‟t be removed or modified.")
        SDKErrorList.Add(&H30F, "The specified remote device is not paired.")
        SDKErrorList.Add(&H401, "HCI error 'Unknown ,HCI Command (&H01)' is received.")
        SDKErrorList.Add(&H402, "HCI error 'Unknown Connection Identifier (&H02)' is received.")
        SDKErrorList.Add(&H403, "HCI error 'Hardware Failure (&H03)' is received.")
        SDKErrorList.Add(&H404, "HCI error 'Page Timeout (&H04)' is received.")
        SDKErrorList.Add(&H405, "HCI error 'Authentication Failure (&H05)' is received.")
        SDKErrorList.Add(&H406, "HCI error 'PIN or Key Missing (&H06)' is received.")
        SDKErrorList.Add(&H407, "HCI error 'Memory Capacity Exceeded (&H07)' is received.")
        SDKErrorList.Add(&H408, "HCI error 'Connection Timeout (&H08)' is received.")
        SDKErrorList.Add(&H409, "HCI error 'Connection Limit Exceeded (&H09)' is received.")
        SDKErrorList.Add(&H40A, "HCI error 'Synchronous Connection Limit to a Device Exceeded (&H0A)' is received.")
        SDKErrorList.Add(&H40B, "HCI error 'ACL Connection Already Exists (&H0B)' is received.")
        SDKErrorList.Add(&H40C, "HCI error 'Command Disallowed (&H0C)' is received.")
        SDKErrorList.Add(&H40D, "HCI error 'Connection Rejected due to Limited Resources (&H0D)' is received.")
        SDKErrorList.Add(&H40E, "HCI error 'Connection Rejected due to Security Reasons (&H0E)' is received.")
        SDKErrorList.Add(&H40F, "HCI error 'Connection Rejected due to Unacceptable BD_ADDR (&H0F)' is received.")
        SDKErrorList.Add(&H410, "HCI error 'Connection Accept Timeout Exceeded (0X10)' is received.")
        SDKErrorList.Add(&H411, "HCI error 'Unsupported Feature or Parameter Value (0X11)' is received.")
        SDKErrorList.Add(&H412, "HCI error 'Invalid ,HCI Command parameters (0X12)' is received.")
        SDKErrorList.Add(&H413, "HCI error 'Remote User Terminated Connection (0X13)' is received.")
        SDKErrorList.Add(&H414, "HCI error 'Remote Device Terminated Connection due to Low Resources (0X14)' is received.")
        SDKErrorList.Add(&H415, "HCI error 'Remote Device Terminated Connection due to Power Off (0X15)' is received.")
        SDKErrorList.Add(&H416, "HCI error 'Connection Terminated by Local Host (0X16)' is received.")
        SDKErrorList.Add(&H417, "HCI error 'Repeated Attempts (0X17)' is received.")
        SDKErrorList.Add(&H418, "HCI error 'Pairing Not Allowed (0X18)' is received.")
        SDKErrorList.Add(&H419, "HCI error 'Unknown LMP PDU (0X19)' is received.")
        SDKErrorList.Add(&H41A, "HCI error 'Unsupported Remote Feature / Unsupported LMP Feature (0X1A)' is received.")
        SDKErrorList.Add(&H41B, "HCI error 'SCO Offset Rejected (0X1B)' is received.")
        SDKErrorList.Add(&H41C, "HCI error 'SCO Interval Rejected (0X1C)' is received.")
        SDKErrorList.Add(&H41D, "HCI error 'SCO Air Mode Rejected (0X1D)' is received.")
        SDKErrorList.Add(&H41E, "HCI error 'Invalid LMP Parameters (0X1E)' is received.")
        SDKErrorList.Add(&H41F, "HCI error 'Unspecified Error (0X1F)' is received.")
        SDKErrorList.Add(&H420, "HCI error 'Unsupported LMP Parameter Value (0X20)' is received.")
        SDKErrorList.Add(&H421, "HCI error 'Role Change Not Allowed (0X21)' is received.")
        SDKErrorList.Add(&H422, "HCI error 'LMP Response Timeout (0X22)' is received.")
        SDKErrorList.Add(&H423, "HCI error 'LMP Error Transaction Collision (0X23)' is received.")
        SDKErrorList.Add(&H424, "HCI error 'LMP PDU Not Allowed (0X24)' is received.")
        SDKErrorList.Add(&H425, "HCI error 'Encryption Mode Not Acceptable (0X25)' is received.")
        SDKErrorList.Add(&H426, "HCI error 'Link Key Can not be Changed (0X26)' is received.")
        SDKErrorList.Add(&H427, "HCI error 'Requested QOS Not Supported (0X27)' is received.")
        SDKErrorList.Add(&H428, "HCI error 'Instant Passed (0X28)' is received.")
        SDKErrorList.Add(&H429, "HCI error 'Pairing with Unit Key Not Supported (0X29)' is received.")
        SDKErrorList.Add(&H42A, "HCI error 'Different Transaction Collision (0X2A)' is received.")
        SDKErrorList.Add(&H42C, "HCI error 'QOS Unacceptable Parameter (0X2C)' is received.")
        SDKErrorList.Add(&H42D, "HCI error 'QOS Rejected (0X2D)' is received.")
        SDKErrorList.Add(&H42E, "HCI error 'Channel Classification Not Supported (0X2E)' is received.")
        SDKErrorList.Add(&H42F, "HCI error 'Insufficient Security (0X2F)' is received.")
        SDKErrorList.Add(&H430, "HCI error 'Parameter Out of Mandatory Range (0X30)' is received.")
        SDKErrorList.Add(&H432, "HCI error 'Role Switch Pending (0X32)' is received.")
        SDKErrorList.Add(&H434, "HCI error 'Reserved Slot Violation (0X34)' is received.")
        SDKErrorList.Add(&H435, "HCI error 'Role Switch Failed (0X35)' is received.")
        'see supplement err codes: 
        '6.1.1 Error Codes
        '6.1.2 AVRCP Error Codes
    End Sub
    Public Function SDKErrorToString(SDKError As UInt32) As String
        SDKErrorList.Clear()
        CreateListSDKError()
        Dim stringfinal As String = ""
        For Each pair In SDKErrorList
            If pair.Key = "&H0" & SDKError Then stringfinal = pair.Value.ToString
        Next
        SDKErrorList.Clear()
        Return stringfinal
    End Function

    Public Sub CreateListCMEError()
        CMEErrorList.Add(0, "Phone failure")
        CMEErrorList.Add(1, "No connection to phone")
        CMEErrorList.Add(2, "Phone adapter link reserved")
        CMEErrorList.Add(3, "Operation not allowed")
        CMEErrorList.Add(4, "Operation not supported")
        CMEErrorList.Add(5, "PH_SIM PIN required")
        CMEErrorList.Add(6, "PH_FSIM PIN required")
        CMEErrorList.Add(7, "PH_FSIM PUK required")
        CMEErrorList.Add(10, "SIM not inserted")
        CMEErrorList.Add(11, "SIM PIN required")
        CMEErrorList.Add(12, "SIM PUK required")
        CMEErrorList.Add(13, "SIM failure")
        CMEErrorList.Add(14, "SIM busy")
        CMEErrorList.Add(15, "SIM wrong")
        CMEErrorList.Add(16, "Incorrect password")
        CMEErrorList.Add(17, "SIM PIN2 required")
        CMEErrorList.Add(18, "SIM PUK2 required")
        CMEErrorList.Add(20, "Memory full")
        CMEErrorList.Add(21, "Invalid index")
        CMEErrorList.Add(22, "Not found")
        CMEErrorList.Add(23, "Memory failure")
        CMEErrorList.Add(24, "Text string too long")
        CMEErrorList.Add(25, "Invalid characters in text string")
        CMEErrorList.Add(26, "Dial string too long")
        CMEErrorList.Add(27, "Invalid characters in dial string")
        CMEErrorList.Add(30, "No network service")
        CMEErrorList.Add(31, "Network timeout")
        CMEErrorList.Add(32, "Network not allowed, emergency calls only")
        CMEErrorList.Add(40, "Network personalization PIN required")
        CMEErrorList.Add(41, "Network personalization PUK required")
        CMEErrorList.Add(42, "Network subset personalization PIN required")
        CMEErrorList.Add(43, "Network subset personalization PUK required")
        CMEErrorList.Add(44, "Service provider personalization PIN required")
        CMEErrorList.Add(45, "Service provider personalization PUK required")
        CMEErrorList.Add(46, "Corporate personalization PIN required")
        CMEErrorList.Add(47, "Corporate personalization PUK required")
        CMEErrorList.Add(48, "PH-SIM PUK required")
        CMEErrorList.Add(100, "Unknown error")
        CMEErrorList.Add(103, "Illegal MS")
        CMEErrorList.Add(106, "Illegal ME")
        CMEErrorList.Add(107, "GPRS services not allowed")
        CMEErrorList.Add(111, "PLMN not allowed")
        CMEErrorList.Add(112, "Location area not allowed")
        CMEErrorList.Add(113, "Roaming not allowed in this location area")
        CMEErrorList.Add(126, "Operation temporary not allowed")
        CMEErrorList.Add(132, "Service operation not supported")
        CMEErrorList.Add(133, "Requested service option not subscribed")
        CMEErrorList.Add(134, "Service option temporary out of order")
        CMEErrorList.Add(148, "Unspecified GPRS error")
        CMEErrorList.Add(149, "PDP authentication failure")
        CMEErrorList.Add(150, "Invalid mobile class")
        CMEErrorList.Add(256, "Operation temporarily not allowed")
        CMEErrorList.Add(257, "Call barred")
        CMEErrorList.Add(258, "Phone is busy")
        CMEErrorList.Add(259, "User abort")
        CMEErrorList.Add(260, "Invalid dial string")
        CMEErrorList.Add(261, "SS not executed")
        CMEErrorList.Add(262, "SIM Blocked")
        CMEErrorList.Add(263, "Invalid block")
        CMEErrorList.Add(772, "SIM powered down")

    End Sub
    Public Function CMEErrorToString(CMEError As String) As String
        Dim stringfinal As String = ""
        If CMEError.StartsWith("+CME ERROR: ") Then
            CMEError = CMEError.Replace("+CME ERROR: ", "")
        Else
            stringfinal = "???Bad CME error???"
        End If
        CMEErrorList.Clear()
        CreateListCMEError()
        For Each pair In SDKErrorList
            If pair.Key = "&H0" & CInt(CMEError) Then stringfinal = pair.Value.ToString
        Next
        SDKErrorList.Clear()
        Return stringfinal
    End Function

    Public Function ClassVal2String(ByVal classofdevice As UInteger) As String
        ClassVal2String = ""
        Select Case Right(Hex(classofdevice), 3)
            Case "1FFC"
                ClassVal2String = "Mask of device class"
            Case "100"
                ClassVal2String = "Computer major device class"
            Case "100"
                ClassVal2String = "Uncategorized computer, code for device not assigned"
            Case "104"
                ClassVal2String = "Desktop workstation"
            Case "108"
                ClassVal2String = "Server-class computer"
            Case "10C"
                ClassVal2String = "Laptop computer"
            Case "110"
                ClassVal2String = "Handheld PC/PDA (clam shell)"
            Case "114"
                ClassVal2String = "Palm sized PC/PDA"
            Case "118"
                ClassVal2String = "Wearable computer (Watch sized)"
            Case "200"
                ClassVal2String = "Phone major device class"
            Case "200"
                ClassVal2String = "Uncategorized phone, code for device not assigned"
            Case "204"
                ClassVal2String = "Cellular phone"
            Case "208"
                ClassVal2String = "Cordless phone"
            Case "20C"
                ClassVal2String = "Smart phone"
            Case "210"
                ClassVal2String = "Wired modem or voice gateway"
            Case "214"
                ClassVal2String = "Common ISDN Access"
            Case "218"
                ClassVal2String = "SIM card reader"
            Case "300"
                ClassVal2String = "LAN / Network Access Point major"
            Case "300"
                ClassVal2String = "Fully available"
            Case "320"
                ClassVal2String = "1 - 17% utilized"
            Case "340"
                ClassVal2String = "17- 33% utilized"
            Case "360"
                ClassVal2String = "33 - 50% utilized"
            Case "380"
                ClassVal2String = "50 - 67% utilized"
            Case "3A0"
                ClassVal2String = "67 - 83% utilized"
            Case "3C0"
                ClassVal2String = "83 – 99% utilized"
            Case "3E0"
                ClassVal2String = "No service available"
            Case "400"
                ClassVal2String = "Audio/Video major device class"
            Case "400"
                ClassVal2String = "Uncategorized A/V device, code for device not assigned"
            Case "404"
                ClassVal2String = "Wearable headset device"
            Case "408"
                ClassVal2String = "Hands-free device"
            Case "410"
                ClassVal2String = "Microphone"
            Case "414"
                ClassVal2String = "Loudspeaker"
            Case "418"
                ClassVal2String = "Headphones"
            Case "41C"
                ClassVal2String = "Portable Audio"
            Case "420"
                ClassVal2String = "Car Audio"
            Case "424"
                ClassVal2String = "Set-top box"
            Case "428"
                ClassVal2String = "HiFi Audio device"
            Case "42C"
                ClassVal2String = "Videocassette recorder"
            Case "430"
                ClassVal2String = "Video camera"
            Case "434"
                ClassVal2String = "Camcorder"
            Case "438"
                ClassVal2String = "Video monitor"
            Case "43C"
                ClassVal2String = "Video display and loudspeaker"
            Case "440"
                ClassVal2String = "Video conferencing"
            Case "448"
                ClassVal2String = "Gaming/Toy"
            Case "500"
                ClassVal2String = "Peripheral major device class"
            Case "500"
                ClassVal2String = "Uncategorized peripheral device, code for device not assigned"
            Case "540"
                ClassVal2String = "Keyboard"
            Case "580"
                ClassVal2String = "Pointing device"
            Case "5C0"
                ClassVal2String = "Combo keyboard/pointing device"
            Case "600"
                ClassVal2String = "Imaging major device class"
            Case "610"
                ClassVal2String = "Display"
            Case "620"
                ClassVal2String = "Camera"
            Case "640"
                ClassVal2String = "Scanner"
            Case "680"
                ClassVal2String = "Printer"
            Case "700"
                ClassVal2String = "Wearable major device class"
            Case "704"
                ClassVal2String = "Wristwatch"
            Case "708"
                ClassVal2String = "Pager"
            Case "70C"
                ClassVal2String = "Jacket"
            Case "710"
                ClassVal2String = "Helmet"
            Case "714"
                ClassVal2String = "Glasses"
            Case Else
                ClassVal2String = "Unknow"
        End Select
        Return ClassVal2String
    End Function


    Public Function DSToRRLanguage(lang As String) As String
        Dim RRlang As String = ""
        RRToDSDictionnary()
        If LanguageList.ContainsValue(lang) Then
            Dim pair As KeyValuePair(Of String, String)
            For Each pair In LanguageList
                If pair.Value = lang Then RRlang = pair.Key
            Next
        End If
        Return RRlang
    End Function

    ''' <summary>
    ''' Converts a language to its identifier.
    ''' </summary>
    ''' <param name="language">The language."</param>
    ''' <returns>The identifier or <see cref="string.Empty"/> if none.</returns>
    Public Sub RRToDSDictionnary()
        LanguageList.Clear()

        LanguageList.Add("afrikaans", "af")
        LanguageList.Add("albanian", "sq")
        LanguageList.Add("arabic", "ar")
        LanguageList.Add("azerbaijani", "az")
        LanguageList.Add("basque", "eu")
        LanguageList.Add("bengali", "bn")
        LanguageList.Add("belarusian", "be")
        LanguageList.Add("bulgarian", "bg")
        LanguageList.Add("catalan", "ca")
        LanguageList.Add("chinese", "zh-cn")
        'languagelist.add("chinese s", "zh-cn")
        'languagelist.add("chinese t", "zh-tw")
        LanguageList.Add("croatian", "hr")
        LanguageList.Add("czech", "cs")
        LanguageList.Add("danish", "da")
        LanguageList.Add("dutch", "nl")
        LanguageList.Add("english", "en")
        LanguageList.Add("esperanto", "eo")
        LanguageList.Add("estonian", "et")
        LanguageList.Add("filipino", "tl")
        LanguageList.Add("finnish", "fi")
        LanguageList.Add("french", "fr")
        LanguageList.Add("galician", "gl")
        LanguageList.Add("georgian", "ka")
        LanguageList.Add("german", "de")
        LanguageList.Add("greek", "el")
        LanguageList.Add("gujarati", "gu")
        LanguageList.Add("haitian creole", "ht")
        LanguageList.Add("hebrew", "iw")
        LanguageList.Add("hindi", "hi")
        LanguageList.Add("hungarian", "hu")
        LanguageList.Add("icelandic", "is")
        LanguageList.Add("indonesian", "id")
        LanguageList.Add("irish", "ga")
        LanguageList.Add("italian", "it")
        LanguageList.Add("japanese", "ja")
        LanguageList.Add("korean", "ko")
        LanguageList.Add("latin", "la")
        LanguageList.Add("latvian", "lv")
        LanguageList.Add("lithuanian", "lt")
        LanguageList.Add("macedonian", "mk")
        LanguageList.Add("malay", "ms")
        LanguageList.Add("maltese", "mt")
        LanguageList.Add("norwegian", "no")
        LanguageList.Add("persian", "fa")
        LanguageList.Add("polish", "pl")
        LanguageList.Add("portuguese", "pt")
        LanguageList.Add("romanian", "ro")
        LanguageList.Add("russian", "ru")
        LanguageList.Add("serbian", "sr")
        LanguageList.Add("slovak", "sk")
        LanguageList.Add("slovenian", "sl")
        LanguageList.Add("spanish", "es")
        LanguageList.Add("swahili", "sw")
        LanguageList.Add("swedish", "sv")
        LanguageList.Add("tamil", "ta")
        LanguageList.Add("telugu", "te")
        LanguageList.Add("thai", "th")
        LanguageList.Add("turkish", "tr")
        LanguageList.Add("ukrainian", "uk")
        LanguageList.Add("urdu", "ur")
        LanguageList.Add("vietnamese", "vi")
        LanguageList.Add("welsh", "cy")
        LanguageList.Add("yiddish", "yi")

    End Sub
#End Region

#Region "Extra functions"
    ''' <summary>
    ''' Get the Bluesoleil SDK version
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVersionSdkDll() As String
        ' Get the file version for the bsSDK.dll. 
        Dim tempStr As String = BlueSoleil_GetSDKDLLfilename()
        FileVersionInfo.GetVersionInfo(tempStr)
        Dim myFileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(tempStr)
        ' Print the file name and version number.
        'Console.WriteLine("File: " + myFileVersionInfo.FileDescription + vbLf + "Version number: " + myFileVersionInfo.FileVersion)
        tempStr = myFileVersionInfo.FileVersion
        Return tempStr
    End Function

    ''' <summary>
    ''' Convert Hexa value to Decimal
    ''' </summary>
    ''' <param name="ValHex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HexaToDec(ValHex As String) As Integer
        Return Val("&H" & ValHex) '& "&")
    End Function


    ''' <summary>
    ''' Build a RR custom list file with ICO option
    ''' </summary>
    ''' <param name="CustomList"></param>
    ''' <param name="Number"></param>
    ''' <param name="Name"></param>
    ''' <param name="IconType"></param>
    ''' <remarks></remarks>
    Public Sub AddCustomList(ByVal CustomList As String, ByVal Number As String, ByVal Name As String, ByVal IconType As String)
        If File.Exists(CustomList) = False Then
            Using objStreamWriter As StreamWriter = New StreamWriter(CustomList, True, Encoding.Unicode)
                objStreamWriter.Write(" 0" & vbCrLf)
                objStreamWriter.Close()
            End Using
        End If
        File.AppendAllText(CustomList, "LST" & Number & "||" & Name & vbCrLf, Encoding.Unicode)

        If PluginRunForDS = False Then 'line ICON not accepted by iCarDS
            If IconType <> "" Then File.AppendAllText(CustomList, "ICO" & IconType & vbCrLf, Encoding.Unicode)
        End If

    End Sub


    ''' <summary>
    ''' Run the vcard update process with or without popup at the end !
    ''' </summary>
    ''' <param name="WithEvent"></param>
    ''' <remarks></remarks>
    Public Sub RRVcardUpdateProcess(ByVal WithEvent As Boolean, Optional bookType As String = "")
        If Not PhoneBookSyncInProgress Then
            PhoneBookSyncInProgress = True
            If WithEvent = False Then
                btnGetPhonebook(bookType, False)
            Else
                btnGetPhonebook(bookType, True)
            End If
            PhoneBookSyncInProgress = False
        End If
    End Sub

    ''' <summary>
    ''' Build a RR List with languages directories
    ''' </summary>
    ''' <param name="MyDirectory"></param>
    ''' <remarks></remarks>
    Public Sub ListeDirectoriesIntoDirectory(ByVal MyDirectory As String)
        Try
            Dim monStreamWriter As New StreamWriter(MyDirectory & ".txt", True, Encoding.Unicode)
            monStreamWriter.WriteLine(" 0")
            For Each ligneF In Directory.GetDirectories(MyDirectory, "*.*", SearchOption.AllDirectories)
                monStreamWriter.WriteLine("LST" & Path.GetFileName(ligneF) & "||" & Path.GetFileName(ligneF))
                If PluginRunForDS = False Then 'line ICON not accepted by iCarDS
                    monStreamWriter.WriteLine("ICO" & MainPath & "Languages\" & Path.GetFileName(ligneF) & "\" & Path.GetFileName(ligneF) & ".gif")
                End If
            Next
            monStreamWriter.Close()
        Catch ex As Exception
            'TextBox1.Text = ex.Message
        End Try
    End Sub

    Public Sub ChangeEncodingFormatFile(myfile As String)
        File.WriteAllText(myfile, File.ReadAllText(myfile), Encoding.Unicode)
    End Sub
    Public Function DetectEncodingFromFile(myfile As String) As String
        Dim data() As Byte = File.ReadAllBytes(myfile)
        Dim detectedEncoding As Encoding = DetectEncodingFromBom(data)
        If detectedEncoding Is Nothing Then
            DetectEncodingFromFile = "Unable to detect encoding"
        Else
            DetectEncodingFromFile = detectedEncoding.EncodingName
        End If
        Return DetectEncodingFromFile
    End Function
    Public Function DetectEncodingFromBom(data() As Byte) As Encoding
        Dim detectedEncoding As Encoding = Nothing
        For Each info As EncodingInfo In Encoding.GetEncodings()
            Dim currentEncoding As Encoding = info.GetEncoding()
            Dim preamble() As Byte = currentEncoding.GetPreamble()
            Dim match As Boolean = True
            If (preamble.Length > 0) And (preamble.Length <= data.Length) Then
                For i As Integer = 0 To preamble.Length - 1
                    If preamble(i) <> data(i) Then
                        match = False
                        Exit For
                    End If
                Next
            Else
                match = False
            End If
            If match Then
                detectedEncoding = currentEncoding
                Exit For
            End If
        Next
        Return detectedEncoding
    End Function


    ''' <summary>
    ''' Check if BlueSoleilCS.exe is running
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BlueSoleilCS_IsRunning() As Boolean
        'find process BlueSoleilCS
        Dim isProcHere As Boolean = False
        Dim proc() As Process
        Try
            proc = Process.GetProcessesByName("BlueSoleilCS")
            If proc.Length > 0 Then
                isProcHere = True
            End If
        Catch ex As Exception

        End Try
        Return isProcHere
    End Function

    ''' <summary>
    ''' Kill the process BlueSoleilCS.exe
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BlueSoleilCS_KillBSprocess() As Boolean
        'find process BlueSoleilCS
        'kill it.
        Dim isProcHere As Boolean = False
        Dim isProcKilled As Boolean = False
        Dim proc() As Process
        Try
            proc = Process.GetProcessesByName("BlueSoleilCS")
            If proc.Length > 0 Then
                isProcHere = True
            End If
            For i As Integer = 0 To proc.Length - 1
                Try
                    proc(i).CloseMainWindow()
                Catch ex As Exception

                End Try
                Try
                    proc(i).Kill()
                    isProcKilled = proc(i).WaitForExit(5000)
                Catch ex As Exception

                End Try
            Next i
        Catch ex As Exception

        End Try
        If isProcHere = False Then
            Return True                 'not running.
        Else
            If isProcKilled = True Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    ' Uses the ProcessStartInfo class to start new processes,
    ' both in a minimized mode.
    Public Sub BlueSoleilCS_Restart()
        Dim p As New ProcessStartInfo
        p.FileName = "BlueSoleilCS.exe"
        p.WindowStyle = ProcessWindowStyle.Normal

        Process.Start(p)

    End Sub 'OpenWithStartInfo

#End Region

#Region "Contain in string with sensitive case option"
    <Extension()>
    Public Function Contains(str As String, substring As String, comp As StringComparison) As Boolean
        If substring Is Nothing Then
            Throw New ArgumentNullException("substring", "substring cannot be null.")
        ElseIf Not [Enum].IsDefined(GetType(StringComparison), comp) Then
            Throw New ArgumentException("comp is not a member of StringComparison", "comp")
        End If
        Return str.IndexOf(substring, comp) >= 0
    End Function
#End Region

#Region "Black List"
    Public Sub AddContactInBlackList(ByVal contactNumber As String)
        If contactNumber <> "" Then
            If File.Exists(MainPath & "MobilePhone_BlackList.lst") = False Then
                Using objStr As StreamWriter = New StreamWriter(MainPath & "MobilePhone_BlackList.lst", True, Encoding.Unicode)
                    objStr.Close()
                End Using
            End If
            File.AppendAllText(MainPath & "MobilePhone_BlackList.lst", contactNumber.Trim & vbCrLf, Encoding.Unicode)
        End If
        hfpContactNumberInBlackList = CountContactsInBlacklist()
    End Sub
    Public Function CheckContactInBlackList(ByVal contactNumber As String) As Boolean
        If File.Exists(MainPath & "MobilePhone_BlackList.lst") = True Then
            Dim allLines As String() = IO.File.ReadAllLines(MainPath & "MobilePhone_BlackList.lst", Encoding.Unicode)
            For Each line In allLines
                If line = contactNumber Then
                    Return True
                    Exit Function
                End If

            Next
            Return False
        End If
    End Function
    Public Function CountContactsInBlacklist() As Integer
        If File.Exists(MainPath & "MobilePhone_BlackList.lst") = True Then
            Dim allLines As Integer = IO.File.ReadAllLines(MainPath & "MobilePhone_BlackList.lst", Encoding.Unicode).Length
            Return allLines
        End If
    End Function
    Public Function ResetContactsInBlacklist() As Boolean
        If File.Exists(MainPath & "MobilePhone_BlackList.lst") = True Then
            Dim allLines As New List(Of String)
            allLines.AddRange(System.IO.File.ReadAllLines(MainPath & "MobilePhone_BlackList.lst"))
            If allLines.Count = 0 Then Exit Function
            For i As Integer = 0 To allLines.Count - 1
                allLines.RemoveAt(i)
            Next
            System.IO.File.WriteAllLines(MainPath & "MobilePhone_BlackList.lst", allLines.ToArray, Encoding.Unicode)
            allLines.AddRange(System.IO.File.ReadAllLines(MainPath & "MobilePhone_BlackList.lst"))
            If allLines.Count = 0 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Sub DeleteContactInBlacklist(ByRef line As Integer)
        If File.Exists(MainPath & "MobilePhone_BlackList.lst") = True Then
            Dim allLines As New List(Of String)
            allLines.AddRange(System.IO.File.ReadAllLines(MainPath & "MobilePhone_BlackList.lst"))
            ' if line is beyond end of list the exit sub
            If line >= allLines.Count Then Exit Sub
            allLines.RemoveAt(line)
            System.IO.File.WriteAllLines(MainPath & "MobilePhone_BlackList.lst", allLines.ToArray, Encoding.Unicode)
        End If
    End Sub

#End Region

#Region "Debug Log"
    'Public Sub ToLog(ByVal TheMessage As String) 'si pb voir ici https://msdn.microsoft.com/en-us/library/3zc0w663(v=vs.110).aspx
    '    If TempPluginSettings.PhoneDebugLog = True Then
    '        Try
    '            If File.Exists(DebuglogPath) = False Then
    '                Using objStreamWriter As StreamWriter = New StreamWriter(DebuglogPath, True, Encoding.Unicode)
    '                    objStreamWriter.WriteLine(DateTime.Now + "-->MOBILEPHONE is started :-)")
    '                    objStreamWriter.Close()
    '                End Using
    '            End If
    '            File.AppendAllText(DebuglogPath, DateTime.Now + "-->" & TheMessage.Replace(vbCrLf, "") & vbCrLf, Encoding.Unicode)
    '        Catch ex As Exception
    '            'do nothing
    '        End Try

    '    End If
    'End Sub
    Public Sub ToLog(ByVal TheMessage As String)
        If TempPluginSettings.PhoneDebugLog = True Then
            Dim failCounter As Integer = 0
            Do
                Try
                    If File.Exists(DebuglogPath) = False Then
                        Using objStreamWriter As StreamWriter = New StreamWriter(DebuglogPath, True, Encoding.Unicode)
                            objStreamWriter.WriteLine(DateTime.Now + "-->MOBILEPHONE is started :-)")
                            objStreamWriter.Close()
                        End Using
                    End If
                    File.AppendAllText(DebuglogPath, DateTime.Now + "-->" & TheMessage.Replace(vbCrLf, "") & vbCrLf, Encoding.Unicode)
                    Exit Do 'exit the loop since there was no error if execution reaches this line.
                Catch ex As Exception
                    Application.DoEvents() 'maybe give other processes a moment to do their things.
                    failCounter = failCounter + 1 'increment number of failed attempts.
                    If failCounter > 20 Then Exit Do 'if we've failed 20 times, give up.
                    'do nothing, because loop will re-try.
                End Try
            Loop
        End If
    End Sub
#End Region

#Region "Change Keyboard"
    ''http://stackoverflow.com/questions/34973921/keyboard-input-language-changing-program
    'Public _russianInput As InputLanguage
    'Public _englishInput As InputLanguage

    'Public Sub DefineLanguages()
    '    _russianInput = GetInputLanguageByName("russian")
    '    _englishInput = GetInputLanguageByName("english")
    'End Sub

    'Public Sub SetKeyboardLayout(ByVal layout As InputLanguage)
    '    InputLanguage.CurrentInputLanguage = layout
    'End Sub

    'Public Function GetInputLanguageByName(ByVal inputName As String) As InputLanguage
    '    Dim lang As InputLanguage
    '    For Each lang In InputLanguage.InstalledInputLanguages
    '        If lang.Culture.EnglishName.ToLower().StartsWith(inputName) Then
    '            Return lang
    '        End If
    '    Next
    '    Return Nothing
    'End Function

    'Public Sub LoadRussianKeyboardLayout()
    '    If Not _russianInput Is Nothing Then
    '        InputLanguage.CurrentInputLanguage = _russianInput
    '    Else
    '        InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage
    '    End If
    'End Sub

    'Public Sub LoadEnglishKeyboardLayout()
    '    If Not _englishInput Is Nothing Then
    '        InputLanguage.CurrentInputLanguage = _englishInput
    '    Else
    '        InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage
    '    End If
    'End Sub

    'Private Sub TextBox1_GotFocus(sender As Object, e As System.EventArgs) Handles TextBox1.GotFocus
    '    InputLanguage.CurrentInputLanguage = _russianInput
    'End Sub

    'Private Sub TextBox2_GotFocus(sender As Object, e As System.EventArgs) Handles TextBox2.GotFocus
    '    InputLanguage.CurrentInputLanguage = _englishInput
    'End Sub

    'declaration des contantes
    Public Const KLF_ACTIVATE = &H1
    Public Const LANG_FR As UInt32 = &H40C
    Public Const LANG_AR As UInt32 = &H1801
    Public Const LANG_EN As UInt32 = &H409
    'declaration d'une fonction qui modifié la langue a l'aide de "user32"
    Public Declare Function ActivateKeyboardLayout Lib "user32" (ByVal HKL As UInt32, ByVal flags As UInt32) As UInt32
    ' puis on fait l'appel a notre fonction ActivateKeyboardLayout dans une autre fonction pour facilite utilisation
    Public Sub Changelangue(ByVal Language As UInt32)
        ActivateKeyboardLayout(Language, KLF_ACTIVATE)
    End Sub
    'et enfin on fait l'appel a cet derniere fontion dans n'import evenement et choisi la langue qui tu veut comme ça (ex: l'evenement GotFocus):
    'Private Sub textboxarabe_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles nomar.GotFocus
    '    ChangeKeyboardLanguage(LANG_AR)
    'End Sub


    Private Declare Function GetKeyboardLayoutList Lib "user32" (ByVal size As Long, ByRef layouts As Long) As Long
    Public Sub GetKb()
        Dim numLayouts As Long
        Dim i As Long
        Dim layouts() As Long

        numLayouts = GetKeyboardLayoutList(0, 0&)
        ReDim layouts(numLayouts - 1)
        GetKeyboardLayoutList(numLayouts, layouts(0))

        Dim msg As String
        msg = "Loaded keyboard layouts: " & vbCrLf & vbCrLf

        For i = 0 To numLayouts - 1
            msg = msg & Hex(layouts(i)) & vbCrLf
        Next

        MsgBox(msg)
    End Sub


#End Region

#Region "Song duration"
    Public Function GetDuration(ByVal filename As String) As Double

        Dim rifftag(4) As Char
        Dim Filesize As Long
        Dim WAVtag(4) As Char
        Dim FMTtag(4) As Char
        Dim FMTsize As Long
        Dim compresstype As Integer
        Dim channels As Integer
        Dim samplerate As Long
        Dim bytespersec As Long
        Dim bytespersample As Integer
        Dim bitspersample As Integer
        Dim wavlength As Double

        Dim reader = New BinaryReader(File.Open(filename, FileMode.Open))

        rifftag = reader.ReadChars(4)
        Filesize = reader.ReadInt32
        WAVtag = reader.ReadChars(4)
        FMTtag = reader.ReadChars(4)
        FMTsize = reader.ReadInt32
        compresstype = reader.ReadInt16
        channels = reader.ReadInt16
        samplerate = reader.ReadInt32
        bytespersec = reader.ReadInt32
        bytespersample = reader.ReadInt16
        bitspersample = reader.ReadInt16

        wavlength = (Filesize / bytespersec) * 1000 ' return in milli seconds

        reader.Close()

        Return wavlength

    End Function

#End Region

#Region "USBWatcher"
    Public Sub RunUSBWatcher() 'check usb connection and return if phone device is connected to an external power
        Dim strComputer As String
        Dim objWMIService As Object
        Dim colDevices As Object
        Dim strDeviceName As String
        Dim strQuotes As String
        Dim arrDeviceNames As Array
        Dim colUSBDevices As Object
        'Dim objUSBDevice As Object
        'Dim item2 As String
        Dim deviceNubrFound As Integer = 0

        strComputer = "."

        objWMIService = GetObject("winmgmts:\\" & strComputer & "\root\cimv2")
        colDevices = objWMIService.ExecQuery("Select * From Win32_USBControllerDevice")

        For Each objDevice In colDevices
            strDeviceName = objDevice.Dependent
            strQuotes = Chr(34)
            strDeviceName = Replace(strDeviceName, strQuotes, "")
            arrDeviceNames = Split(strDeviceName, "=")
            strDeviceName = arrDeviceNames(1)
            'MessageBox.Show(strDeviceName)
            colUSBDevices = objWMIService.ExecQuery("Select * From Win32_PnPEntity Where DeviceID = '" & strDeviceName & "'")
            For Each objUSBDevice In colUSBDevices
                'ExternalPowerInfo = ""
                'ExternalPowerInfo &= "Availability: " & objUSBDevice.Availability & Chr(13)
                'ExternalPowerInfo &= "Caption: " & objUSBDevice.Caption & Chr(13)
                'ExternalPowerInfo &= "ClassGuid: " & objUSBDevice.ClassGuid & Chr(13)
                'ExternalPowerInfo &= "ConfigManagerErrorCode: " & objUSBDevice.ConfigManagerErrorCode & Chr(13)
                'ExternalPowerInfo &= "ConfigManagerUserConfig: " & objUSBDevice.ConfigManagerUserConfig & Chr(13)
                'ExternalPowerInfo &= "CreationClassName: " & objUSBDevice.CreationClassName & Chr(13)
                'ExternalPowerInfo &= "Description: " & objUSBDevice.Description & Chr(13)
                'ExternalPowerInfo &= "DeviceID: " & objUSBDevice.DeviceID & Chr(13)
                'ExternalPowerInfo &= "ErrorCleared: " & objUSBDevice.ErrorCleared & Chr(13)
                'ExternalPowerInfo &= "ErrorDescription: " & objUSBDevice.ErrorDescription & Chr(13)
                'ExternalPowerInfo &= "InstallDate: " & objUSBDevice.InstallDate & Chr(13)
                'ExternalPowerInfo &= "LastErrorCode: " & objUSBDevice.LastErrorCode & Chr(13)
                'ExternalPowerInfo &= "Manufacturer: " & objUSBDevice.Manufacturer & Chr(13)
                'ExternalPowerInfo &= "Name: " & objUSBDevice.Name & Chr(13)
                'ExternalPowerInfo &= "PNPDeviceID: " & objUSBDevice.PNPDeviceID & Chr(13)
                'ExternalPowerInfo &= "PowerManagementCapabilities: " & objUSBDevice.PowerManagementCapabilities & Chr(13)
                'ExternalPowerInfo &= "PowerManagementSupported: " & objUSBDevice.PowerManagementSupported & Chr(13)
                'ExternalPowerInfo &= "Service: " & objUSBDevice.Service & Chr(13)
                'ExternalPowerInfo &= "Status: " & objUSBDevice.Status & Chr(13)
                'ExternalPowerInfo &= "StatusInfo: " & objUSBDevice.StatusInfo & Chr(13)
                'ExternalPowerInfo &= "SystemCreationClassName: " & objUSBDevice.SystemCreationClassName & Chr(13)
                'ExternalPowerInfo &= "SystemName: " & objUSBDevice.SystemName & Chr(13)
                'For Each item2 In objUSBDevice.HardwareID
                '    ExternalPowerInfo &= "HardwareID: " & item2 & Chr(13)
                'Next
                'ExternalPowerInfo &= objUSBDevice.Description() & Chr(13)
                'ExternalPowerInfo &= Chr(13) & Chr(13)
                If objUSBDevice.Caption = TempPluginSettings.PhoneDeviceName Then
                    deviceNubrFound += 1
                    Exit For
                End If
            Next
            'MessageBox.Show(ExternalPowerInfo)
            If deviceNubrFound > 0 Then Exit For
        Next
        If deviceNubrFound > 0 Then
            ExternalPowerIsConnected = True
        Else
            ExternalPowerIsConnected = False
        End If

    End Sub
#End Region

#Region "Phoco Xml  save test"
    'PhoCo's main settings
    Public ExePath_PHOCO As String          'Path to PhoneControl.NET
    Public NoDesign_PHOCO As Boolean        'No Design? (Faster for 1.4 registered and above)
    Public ListsPath_PHOCO As String        'Path where to save phonebooks, calls and SMS
    Public PhonebookStd_PHOCO As Integer    'Default phonebook to be used
    Public MaxCalls_PHOCO As Integer        'Max calls to store
    Public Ringtone_PHOCO As String         'Ringtone sound
    Public SMSTone_PHOCO As String          'SMS notify sound
    Public ContactsPictures_PHOCO As String 'Path to contacts' pictures
    Public NoInCallEvent_PHOCO As Boolean   'For the phones that doesn't suport it
    Public ConnectHSP_PHOCO As Boolean      'Connect BT headset while in call
    Public PhoneName_PHOCO As String        'Bluetooth phone's name
    Public MotoPhone_PHOCO As Boolean       'For Motorola and other phones that allows only one BT connection - Added by Sonicxtacy02
    Public PBEncoding_PHOCO As String       'Phonebooks encoding


    'Status variables
    Public IsConnected_PHOCO As Boolean     'Connection to PhoCo
    Public PhoneIsConnected_PHOCO As Boolean 'Connection to phone
    Public HSPIsConnected_PHOCO As Boolean  'Connection to headset
    Public SignalLevel_PHOCO As Integer     'Signal quality in %
    Public BatteryLevel_PHOCO As Integer    'Battery level in %
    Public PhoneCharging_PHOCO As Boolean   'Battery in charging
    Public MissedCall_PHOCO As Boolean      'Missed call
    Public SMSArrived_PHOCO As Boolean      'New SMS is arrived
    Public SmsStatus_PHOCO As Integer       '0=None, 1=Sending, 2=Sent, 3=Not sent
    Public IncomingCall_PHOCO As Boolean    'Incoming Call notification- added by sonicxtacy02

    'Phonebooks
    Private Structure PBEntry
        Dim name As String      'Name
        Dim privat As String   'Number home
        Dim mobile As String    'Number mobile
        Dim work As String      'Number work
        Dim fax As String       'Number fax
        Dim other As String     'Number other
        Dim email As String     'Email of contact
        Dim Street As String    'Street of contact
        Dim City As String      'City of contact
        Dim Country As String   'Country of contact
        Dim Zip As String       'Zip of contact
    End Structure

    Private InternalPhonebook() As PBEntry
    Private PhonePhonebook() As PBEntry
    Private TEMP_PhonePhonebook() As PBEntry
    Private SimPhonebook() As PBEntry
    Private TEMP_SimPhonebook() As PBEntry
    Private isLoaded_InternalPB As Boolean
    Private isLoaded_PhonePB As Boolean
    Private isLoaded_SimPB As Boolean

    Private InternalPBFile As String = MainPath & "IntPB.xml"
    Private PhonePBFile As String = MainPath & "PB.xml"

    '********************************************************
    'Function for writing Phonebooks - Calls - SMS to file
    '********************************************************

    Public Sub PBXmlSaveToFile()

        'Init vars
        Dim m_oXML As New Xml.XmlDocument
        Dim oRoot As Xml.XmlElement
        Dim oElem As Xml.XmlElement
        Dim oElemData As Xml.XmlElement
        Dim oInstruct As Xml.XmlProcessingInstruction

        Dim x As Long
        Dim item As Long

        On Error Resume Next

        ReDim InternalPhonebook(0)
        ReDim PhonePhonebook(0)
        ReDim TEMP_PhonePhonebook(0)
        ReDim SimPhonebook(0)
        ReDim TEMP_SimPhonebook(0)
        'ReDim AllCalls(0)
        'ReDim TEMP_AllCalls(0)
        'ReDim SmsIn(0)
        'ReDim TEMP_SmsIn(0)
        'ReDim SmsOut(0)
        'ReDim SmsSent(0)
        'ReDim SmsArchive(0)

        'Save Internal Phonebook to file
        If Not isLoaded_InternalPB Then GoTo SavePhonePhonebook
        If File.Exists(InternalPBFile) Then File.Delete(InternalPBFile)
        'Save to xml-DOM
        oInstruct = m_oXML.CreateProcessingInstruction("xml", "version=""1.0""" & " encoding=""" & PBEncoding_PHOCO & """")
        m_oXML.InsertBefore(oInstruct, m_oXML.ChildNodes.Item(0))
        oRoot = m_oXML.CreateElement("AddressBook")
        'Add the root of element to the document
        m_oXML.AppendChild(oRoot)
        item = UBound(InternalPhonebook)
        If item = 0 Then 'Check if entry
            If Not (InternalPhonebook(0).City <> "" _
                    Or InternalPhonebook(0).Country <> "" _
                    Or InternalPhonebook(0).email <> "" _
                    Or InternalPhonebook(0).fax <> "" _
                    Or InternalPhonebook(0).mobile <> "" _
                    Or InternalPhonebook(0).name <> "" _
                    Or InternalPhonebook(0).other <> "" _
                    Or InternalPhonebook(0).privat <> "" _
                    Or InternalPhonebook(0).Street <> "" _
                    Or InternalPhonebook(0).work <> "" _
                    Or InternalPhonebook(0).Zip <> "" _
                    ) Then item = -1
        End If
        For x = 0 To item
            oElemData = m_oXML.CreateElement("AddressBookEntry")
            oRoot.AppendChild(oElemData)
            oElem = m_oXML.CreateElement("Name")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).name
            oElem = m_oXML.CreateElement("PhoneHome")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).privat
            oElem = m_oXML.CreateElement("PhoneWork")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).work
            oElem = m_oXML.CreateElement("PhoneMobil")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).mobile
            oElem = m_oXML.CreateElement("Fax")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).fax
            oElem = m_oXML.CreateElement("PhoneOther")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).other
            oElem = m_oXML.CreateElement("E-Mail")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).email
            oElem = m_oXML.CreateElement("Street")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).Street
            oElem = m_oXML.CreateElement("City")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).City
            oElem = m_oXML.CreateElement("ZIP")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).Zip
            oElem = m_oXML.CreateElement("Country")
            oElemData.AppendChild(oElem)
            oElem.InnerText = InternalPhonebook(x).Country
        Next
        'Save to file
        m_oXML.Save(InternalPBFile)
        'Reset vars
        m_oXML = Nothing
        oElemData = Nothing
        oElem = Nothing
        oRoot = Nothing
        'End Internal Phonebook

SavePhonePhonebook:
        'Save Phone Phonebook to file
        If File.Exists(PhonePBFile) Then File.Delete(PhonePBFile)
        'Save to xml-DOM
        oInstruct = m_oXML.CreateProcessingInstruction("xml", "version=""1.0""" & " encoding=""" & PBEncoding_PHOCO & """")
        m_oXML.InsertBefore(oInstruct, m_oXML.ChildNodes.Item(0))
        oRoot = m_oXML.CreateElement("AddressBook")
        'Add the root of element to the document
        m_oXML.AppendChild(oRoot)
        item = UBound(PhonePhonebook)
        If item = 0 Then 'Check if entry
            If Not (PhonePhonebook(0).City <> "" _
                    Or PhonePhonebook(0).Country <> "" _
                    Or PhonePhonebook(0).email <> "" _
                    Or PhonePhonebook(0).fax <> "" _
                    Or PhonePhonebook(0).mobile <> "" _
                    Or PhonePhonebook(0).name <> "" _
                    Or PhonePhonebook(0).other <> "" _
                    Or PhonePhonebook(0).privat <> "" _
                    Or PhonePhonebook(0).Street <> "" _
                    Or PhonePhonebook(0).work <> "" _
                    Or PhonePhonebook(0).Zip <> "" _
                    ) Then item = -1
        End If
        For x = 0 To item
            oElemData = m_oXML.CreateElement("AddressBookEntry")
            oRoot.AppendChild(oElemData)
            oElem = m_oXML.CreateElement("Name")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).name
            oElem = m_oXML.CreateElement("PhoneHome")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).privat
            oElem = m_oXML.CreateElement("PhoneWork")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).work
            oElem = m_oXML.CreateElement("PhoneMobil")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).mobile
            oElem = m_oXML.CreateElement("Fax")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).fax
            oElem = m_oXML.CreateElement("PhoneOther")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).other
            oElem = m_oXML.CreateElement("E-Mail")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).email
            oElem = m_oXML.CreateElement("Street")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).Street
            oElem = m_oXML.CreateElement("City")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).City
            oElem = m_oXML.CreateElement("ZIP")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).Zip
            oElem = m_oXML.CreateElement("Country")
            oElemData.AppendChild(oElem)
            oElem.InnerText = PhonePhonebook(x).Country
        Next
        'Save to File
        m_oXML.Save(PhonePBFile)
        'Reset vars
        m_oXML = Nothing
        oElemData = Nothing
        oElem = Nothing
        oRoot = Nothing
        'End Phone Phonebook


    End Sub
#End Region



End Module

