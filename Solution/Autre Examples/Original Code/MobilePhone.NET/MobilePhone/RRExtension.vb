Imports ATSMS
Imports System.IO

<ComClass(RRExtension.ClassId, RRExtension.InterfaceId, RRExtension.EventsId)> _
Public Class RRExtension


#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "6a437c15-adb9-457b-8ebe-0b8df6eb5084"
    Public Const InterfaceId As String = "069f5e0f-6962-4144-9efb-9d860e49fe59"
    Public Const EventsId As String = "dd27abab-d667-46f3-808a-5050acc754fb"
#End Region

    Private sMobilePhoneInfoText As String
    Private RR_SDK As Object
    Private sPhonebookpath As String
    Private sPhonebooktype As String
    Private sPhotoPath As String
    Private sPhotoExtension As String
    Private nBatteryImageCount As Integer
    Private nBatteryWarningLevel As Integer
    Private sBatteryImageExtension As String
    Private nSignalImageCount As Integer
    Private sSignalImageExtension As String
    Private bAutoConnect As Boolean = False
    Private sImagePath As String
    Private sSignalStrength As String
    Private sBatteryStrength As String
    Private bBatteryCharging As Boolean

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
        Dim sPath = "./MobilePhone.ini"
        nMobilePhoneSyncDelay = INIFile.Read(sPath, "PHONEBOOK", "SyncDelay", "100")
        sPhonebookpath = INIFile.Read(sPath, "PHONEBOOK", "Path", "")
        sPhotoPath = INIFile.Read(sPath, "PHONEBOOK", "PhotoPath", "")
        sPhotoExtension = INIFile.Read(sPath, "PHONEBOOK", "PhotoExt", "jpg")
        nBatteryWarningLevel = Integer.Parse(INIFile.Read(sPath, "PHONESTATUS", "BatteryWarningLevel", "10"))
        nBatteryImageCount = Integer.Parse(INIFile.Read(sPath, "PHONESTATUS", "BatteryImageCount", "5"))
        sBatteryImageExtension = INIFile.Read(sPath, "PHONESTATUS", "BatteryImageExt", "jpg")
        nSignalImageCount = Integer.Parse(INIFile.Read(sPath, "PHONESTATUS", "SignalImageCount", "5"))
        sSignalImageExtension = INIFile.Read(sPath, "PHONESTATUS", "SignalImageExt", "jpg")
        sImagePath = INIFile.Read(sPath, "PHONESTATUS", "ImagePath", "")
        bAutoConnect = Convert.ToBoolean(INIFile.Read(sPath, "CONNECTION", "AutoConnect", "False"))
        RR_SDK = CreateObject("RoadRunner.SDK")
        If bAutoConnect Then
            Try
                oMobilePhone.Connect()
            Catch ex As Exception
            End Try
        End If

        nUpdateDelay = INIFile.Read(sPath, "PHONESTATUS", "Update", "5000")
        updateTimer = New System.Timers.Timer()
        updateTimer.Interval = nUpdateDelay
        AddHandler updateTimer.Elapsed, AddressOf Me.OnTimerEvent
        updateTimer.Enabled = True

    End Sub


#Region "RRExtension"

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

        ProcessCommand = 0

        Try
            If LCase(frm.tag).Equals("mobilephone.skin") Then
                Dim control As Object = frm.CurrControl
                Dim name As String = control.tag
                Select Case LCase(CMD)
                    Case "mobilephone_connect"
                        If oMobilePhone.IsConnected Then
                            oMobilePhone.Disconnect()
                        Else
                            oMobilePhone.Connect()
                        End If

                        'TEXT INPUT COMMANDS
                    Case "mobilephone_0" '0 pressed
                        appendInfo("0")
                    Case "mobilephone_1" '1 pressed
                        appendInfo("1")
                    Case "mobilephone_2" '2 pressed
                        appendInfo("2")
                    Case "mobilephone_3" '3 pressed
                        appendInfo("3")
                    Case "mobilephone_4" '4 pressed
                        appendInfo("4")
                    Case "mobilephone_5" '5 pressed
                        appendInfo("5")
                    Case "mobilephone_6" '6 pressed
                        appendInfo("6")
                    Case "mobilephone_7" '7 pressed
                        appendInfo("7")
                    Case "mobilephone_8" '8 pressed
                        appendInfo("8")
                    Case "mobilephone_9" '9 pressed
                        appendInfo("9")
                    Case "mobilephone_#" '# pressed
                        appendInfo("#")
                    Case "mobilephone_*" '* pressed
                        appendInfo("*")
                    Case "mobilephone_del"
                        trimLastInfo()

                        'DIALING COMMANDS
                    Case "mobilephone_dial"
                        If oMobilePhone.IsConnected Then
                            oMobilePhone.Dial(sMobilePhoneInfoText)
                            bIncommingCall = True
                        End If
                    Case "mobilephone_pickup"
                        If oMobilePhone.IsConnected Then
                            If bIncommingCall Then
                                oMobilePhone.Answer()
                            Else
                                oMobilePhone.Dial(sMobilePhoneInfoText)
                                bIncommingCall = True
                            End If
                        End If
                    Case "mobilephone_hangup"
                        If oMobilePhone.IsConnected Then
                            bIncommingCall = False
                            sMobilePhoneInfoText = ""
                            oMobilePhone.HangUp()

                            ' We just made a call, so update the received, missed or dialed phonebooks
                            Dim workerThread As Threading.Thread
                            workerThread = New Threading.Thread(AddressOf MobilePhoneSyncProcess)
                            workerThread.Start()
                        End If
                        'SMS COMMANDS
                    Case "mobilephone_smssend"
                        If oMobilePhone.IsConnected Then
                            Phone_SendSMS()
                        End If
                    Case "mobilephone_smsread"
                        If oMobilePhone.IsConnected Then
                            Phone_ReadSMS()
                        End If

                        'PHONEBOOK NAVIGATION COMMANDS
                    Case "mobilephone_pb_first_entry"
                        RR_Execute("GOTOFIRST")
                        RR_Execute("ONCLCLICK")
                    Case "mobilephone_pb_previous_page"
                    Case "mobilephone_pb_previous_entry"
                        RR_Execute("CTRLUP")
                        RR_Execute("ONCLCLICK")
                    Case "mobilephone_pb_next_entry"
                        RR_Execute("CTRLDOWN")
                        RR_Execute("ONCLCLICK")
                    Case "mobilephone_pb_next_page"
                    Case "mobilephone_pb_last_entry"
                        RR_Execute("GOTOLAST")
                        RR_Execute("ONCLCLICK")

                        'PHONEBOOK MANAGEMENT COMMANDS
                    Case "mobilephone_add_entry"
                        RR_SDK.Execute("CLADD;" & sMobilePhoneInfoText & ";" & sMobilePhoneInfoText)
                        RR_Execute("ONCLCLICK")
                        PC_SaveCurrentPhoneBook()
                    Case "mobilephone_delete_entry"
                        RR_SDK.Execute("CLDEL;" & CLPOS)
                        PC_SaveCurrentPhoneBook()
                    Case "mobilephone_edit_entry"
                        Dim currentname As String = RR_GetInfo("CLDESC")
                        If currentname.Length <= 0 Then
                            currentname = sMobilePhoneInfoText
                        End If
                        RR_SDK.Execute("CLSET;" & CLPOS & ";" & sMobilePhoneInfoText & ";" & currentname)
                        PC_SaveCurrentPhoneBook()

                    Case "mobilephone_sync" ' UPDATE PC FILES WITH PHONE (OVERWRITES THE CURRENT PC FILES!!)
                        If oMobilePhone.IsConnected Then
                            bSynchingInProgress = True
                            Phone_LoadPhoneBook("ME")
                            Phone_LoadPhoneBook("SM")
                            Phone_LoadPhoneBook("RC")
                            Phone_LoadPhoneBook("DC")
                            Phone_LoadPhoneBook("MC")
                            bSynchingInProgress = False
                        End If
                    Case "mobilephone_pc" 'PC PHONE BOOK FROM PC
                        PC_LoadPhoneBook("PC")
                    Case "mobilephone_me" 'MEMORY PHONE BOOK FROM PC
                        PC_LoadPhoneBook("ME")
                    Case "mobilephone_sm" 'SIM Card PHONE BOOK FROM PC
                        PC_LoadPhoneBook("SM")
                    Case "mobilephone_rc" 'RECEIVED PHONE BOOK
                        PC_LoadPhoneBook("RC")
                    Case "mobilephone_dc" 'DIALED PHONE BOOK
                        PC_LoadPhoneBook("DC")
                    Case "mobilephone_mc" 'MISSED PHONE BOOK
                        PC_LoadPhoneBook("MC")
                    Case "onclselclick" 'WHEN PHONE LIST IS CLICKED SET INFO LABEL
                    Case "onclclick"
                        updateListInfo()
                End Select
            End If
        Catch
            ' oops
        End Try

    End Function
    Public Function ReturnIndicator(ByRef IND As String) As String
        Dim ReturnCode As String
        'Default (No Action)
        ReturnCode = "False"
        Select Case LCase(IND)
            Case "mobilephone_connected"
                ReturnCode = oMobilePhone.IsConnected
            Case "mobilephone_batterycharging"
                ReturnCode = bBatteryCharging 'no direct call to phone, takes to long
            Case "mobilephone_incommingcall"
                ReturnCode = bIncommingCall
            Case "mobilephone_messagereceived"
                ReturnCode = bNewMessageReceived
            Case "mobilephone_synchronizing"
                ReturnCode = bSynchingInProgress
            Case "mobilephone_communicating"
                ReturnCode = oMobilePhone.bCommunicating
        End Select

        ReturnIndicator = ReturnCode
    End Function
    Public Function ReturnLabel(ByVal LBL As String, ByVal FMT As String) As String
        Select Case LCase(LBL)
            Case "mobilephone_dialbox"
                ReturnLabel = sMobilePhoneInfoText
            Case "mobilephone_manufacturer"
                ReturnLabel = oMobilePhone.Manufacturer
            Case "mobilephone_model"
                ReturnLabel = oMobilePhone.PhoneModel
            Case "mobilephone_revision"
                ReturnLabel = oMobilePhone.Revision
            Case "mobilephone_batterycharging"
                ReturnLabel = bBatteryCharging 'no direct call to phone, takes to long
            Case "mobilephone_communicating"
                ReturnLabel = oMobilePhone.bCommunicating
            Case "mobilephone_batterystrength"
                ReturnLabel = sBatteryStrength 'no direct call to phone, takes to long
            Case "mobilephone_signalstrength"
                ReturnLabel = sSignalStrength 'no direct call to phone, takes to long
            Case "mobilephone_smsc"
                ReturnLabel = oMobilePhone.SMSC
            Case "mobilephone_connected"
                ReturnLabel = oMobilePhone.IsConnected
            Case "mobilephone_imsi"
                ReturnLabel = MobilePhone.CleanPhoneNumber(oMobilePhone.IMSI)
            Case "mobilephone_network"
                ReturnLabel = oMobilePhone.Network
            Case "mobilephone_ownnumber"
                ReturnLabel = MobilePhone.CleanPhoneNumber(oMobilePhone.OwnNumber)
        End Select
    End Function
#End Region

#Region "RR.SDK"
    Private Sub RR_Execute(ByVal CMD As String)
        RR_SDK.Execute(CMD)
    End Sub
    Private Function RR_GetInfo(ByVal CMD As String) As String
        RR_GetInfo = RR_SDK.GetInfo(CMD)
    End Function
#End Region


    Private Sub appendInfo(ByVal key As String)
        sMobilePhoneInfoText = sMobilePhoneInfoText & key
    End Sub
    Private Sub trimLastInfo()
        If sMobilePhoneInfoText.Length > 0 Then
            sMobilePhoneInfoText = sMobilePhoneInfoText.Substring(0, sMobilePhoneInfoText.Length - 1)
        End If
    End Sub

    Private CLPOS As Integer
    Private CLMAX As Integer
    Private CLTOP As Integer
    Private CLLINES As Integer
    Private CLPAGE As Integer
    Private CLPAGES As Integer

    Private Sub updateListInfo()
        CLPOS = Integer.Parse(RR_GetInfo("CLPOS"))
        CLMAX = Integer.Parse(RR_GetInfo("CLMAX"))
        CLTOP = Integer.Parse(RR_GetInfo("CLTOP"))
        CLLINES = Integer.Parse(RR_GetInfo("CLLINES"))
        Dim value As String = RR_GetInfo("CLTEXT")
        sMobilePhoneInfoText = value

        RR_Execute("SETVAR;mobilephone_photopath;" & sPhotoPath & sMobilePhoneInfoText & "." & sPhotoExtension)
    End Sub

    Private Sub PC_SaveCurrentPhoneBook()
        RR_SDK.Execute("CLSAVE;" & sPhonebookpath & "MobilePhone_" & sPhonebooktype & ".txt")
    End Sub
    Private Sub PC_LoadPhoneBook(ByVal type As String)
        sPhonebooktype = type
        RR_Execute("CLLOAD;" & sPhonebookpath & "MobilePhone_" & sPhonebooktype & ".txt")
    End Sub

#Region "Phone"

    Private bIncommingCall As Boolean
    Private bNewMessageReceived As Boolean
    Private WithEvents oMobilePhone As New MobilePhone

    Private Sub Phone_NewIncomingCall(ByVal e As ATSMS.NewIncomingCallEventArgs) Handles oMobilePhone.NewIncomingCall
        bIncommingCall = True
        sMobilePhoneInfoText = e.CallerID
    End Sub
    Private Sub Phone_NewMessageReceived(ByVal e As ATSMS.NewMessageReceivedEventArgs) Handles oMobilePhone.NewMessageReceived
        bNewMessageReceived = True
    End Sub
    Private Sub Phone_ReadSMS()
        bNewMessageReceived = False
    End Sub
    Private Sub Phone_SendSMS()

    End Sub

    Private Sub Phone_LoadPhoneBook(ByVal type As String)
        ' LOAD stuff from phone and save it into files on pc
        Dim entries() As MobilePhone.Entry = oMobilePhone.GetPhoneBookEntries(type)
        If entries IsNot Nothing Then
            Dim result As String = " 0 " & vbCrLf
            For Each entry As MobilePhone.Entry In entries
                result = result & "LST" & entry.Number & "||" & entry.Name & vbCrLf
            Next
            SaveTextToFile(result, sPhonebookpath & "MobilePhone_" & type & ".txt")
        End If
    End Sub

#End Region

    'Helper methods
    Dim bSynchingInProgress As Boolean = False
    Dim nMobilePhoneSyncDelay As Integer = 1000
    Public Sub MobilePhoneSyncProcess()
        If Not bSynchingInProgress Then
            bSynchingInProgress = True
            Threading.Thread.Sleep(nMobilePhoneSyncDelay) 'wait some time before engaging this process
            Phone_LoadPhoneBook("RC")
            Phone_LoadPhoneBook("DC")
            Phone_LoadPhoneBook("MC")
            bSynchingInProgress = False
        End If
    End Sub

    Dim nUpdateDelay As Integer
    Public updateTimer As System.Timers.Timer

    Public Sub OnTimerEvent(ByVal source As Object, ByVal e As System.Timers.ElapsedEventArgs)
        If oMobilePhone.IsConnected Then
            sSignalStrength = oMobilePhone.SignalStrength
            sBatteryStrength = oMobilePhone.BatteryStrength
            bBatteryCharging = oMobilePhone.BatteryCharging

            ' BATTERY
            Dim nImage As Integer = 0
            Try
                nImage = Integer.Parse(oMobilePhone.BatteryStrength)
                If nImage > 0 Then
                    nImage = (Double.Parse(oMobilePhone.BatteryStrength) / 100.0 * nBatteryImageCount)
                End If
            Catch
            End Try
            RR_Execute("SETVAR;mobilephone_batterystrength;" & sImagePath & "MOBILEPHONE_BATTERY_" & nImage & "." & sBatteryImageExtension)

            ' SIGNAL
            nImage = 0
            Try
                nImage = Integer.Parse(oMobilePhone.SignalStrength)
                If nImage > 0 Then
                    nImage = (Double.Parse(oMobilePhone.SignalStrength) / 100.0 * nSignalImageCount)
                End If
            Catch
            End Try
            RR_Execute("SETVAR;mobilephone_signalstrength;" & sImagePath & "MOBILEPHONE_SIGNAL_" & nImage & "." & sSignalImageExtension)

        End If
    End Sub 'OnTimerEvent


    Public Function SaveTextToFile(ByVal strData As String, _
      ByVal FullPath As String, _
        Optional ByVal ErrInfo As String = "") As Boolean

        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        Try
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return bAns
    End Function
End Class


