Imports ATSMS
Imports ATSMS.SMS
Imports ATSMS.Common


Public Class TestATSMS

    Private WithEvents oPhone As GSMModem

    Private Sub TestATSMS_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        oPhone = New GSMModem
        CheckForIllegalCrossThreadCalls = False
    End Sub

    Private Sub btnAutoDetect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutoDetect.Click
        Try
            If oPhone.IsConnected Then oPhone.Disconnect()
            If oPhone.AutoDetect() Then
                oPhone.Connect()
                MsgBox("Connected to " + oPhone.Port, MsgBoxStyle.Information)
                oPhone.Echo = False
                EnableButtons()
            Else
                MsgBox("Failed auto detect", MsgBoxStyle.Information)
                DisableButtons()
            End If
        Catch ex As GeneralException
            MsgBox("Error connecting: " + ex.Message)
        End Try
    End Sub

    Private Sub btnIMSI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIMSI.Click
        Try
            MsgBox(oPhone.IMSI, MsgBoxStyle.Information)
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnIMEI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIMEI.Click
        Try
            MsgBox(oPhone.IMEI, MsgBoxStyle.Information)
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisconnect.Click
        oPhone.Disconnect()
        DisableButtons()
        btnAutoDetect.Enabled = True
        btnManualConnect.Enabled = True
    End Sub

    Private Sub btnPhoneModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPhoneModel.Click
        MsgBox(oPhone.PhoneModel)
    End Sub

    Private Sub btnDial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDial.Click
        If Len(txtMSISDN.Text) = 0 Then
            MsgBox("Please enter a phone number", MsgBoxStyle.Information)
            Exit Sub
        End If

        If oPhone.Dial(txtMSISDN.Text) Then
            MsgBox("Dialed " + txtMSISDN.Text)
        End If
    End Sub

    Private Sub btnAnswer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnswer.Click
        If oPhone.Answer Then
            MsgBox("Answered", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnHangup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHangup.Click
        If oPhone.HangUp Then
            MsgBox("Hang up", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnDtmfDigits_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDtmfDigits.Click
        MsgBox(oPhone.DtmfDigits, MsgBoxStyle.Information)
    End Sub

    Private Sub btnSendDtmf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendDtmf.Click
        If oPhone.SendDtmf("0-9, #, *, A-D") Then
            MsgBox("DTMF supported", MsgBoxStyle.Information)
        Else
            MsgBox("Not supported", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnSendTextSms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendTextSms.Click
        If Len(txtMSISDN.Text) = 0 Then
            MsgBox("Please enter a phone number", MsgBoxStyle.Information)
            Exit Sub
        End If
        oPhone.Encoding = EnumEncoding.GSM_Default_7Bit
        Dim msgId As String = oPhone.SendSMS(txtMSISDN.Text, txtMsg.Text)
        MsgBox("Message sent. Message id is " + msgId)
    End Sub

    Private Sub btnSMSC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSMSC.Click
        MsgBox(oPhone.SMSC, MsgBoxStyle.Information)
    End Sub

    Private Sub btnEnableCLIP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnableCLIP.Click
        If oPhone.EnableCLIP Then
            MsgBox("CLIP is enabled", MsgBoxStyle.Information)
        Else
            MsgBox("CLIP is not enabled", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnDisableCLIP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisableCLIP.Click
        If oPhone.DisableCLIP Then
            MsgBox("CLIP is disabled", MsgBoxStyle.Information)
        Else
            MsgBox("CLIP is not disabled", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnMSISDN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMSISDN.Click
        Try
            MsgBox(oPhone.MSISDN)
        Catch ex As GeneralException
            MsgBox("Cannot read MSISDN")
        End Try
    End Sub

    Private Sub btnManufacturer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManufacturer.Click
        MsgBox(oPhone.Manufacturer)
    End Sub

    Private Sub btnRevision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevision.Click
        MsgBox(oPhone.Revision)
    End Sub

    Private Sub btnSendChineseSms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendUnicodeSms.Click
        If Len(txtMSISDN.Text) = 0 Then
            MsgBox("Please enter a phone number", MsgBoxStyle.Information)
            Exit Sub
        End If
        oPhone.Encoding = EnumEncoding.Unicode_16Bit
        Dim msgId As String = oPhone.SendSMS(txtMSISDN.Text, txtMsg.Text)
        MsgBox("Message sent. Message id is " + msgId)
    End Sub

    Private Sub btnPDUEncoder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPDUEncoder.Click
        Dim f As New frmPDUEncoder
        f.Show()
    End Sub

    Private Sub btnPDUDecoder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPDUDecoder.Click
        Dim f As New frmPDUDecoder
        f.Show()
    End Sub

    Private Sub btnInitMsgIndication_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInitMsgIndication.Click
        Try
            MsgBox("Error code : " & oPhone.InitMsgIndication, MsgBoxStyle.Information)
        Catch ex As System.Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnQueryMsgIndication_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQueryMsgIndication.Click
        Try
            MsgBox("CNMI : " & oPhone.GetMsgIndication, MsgBoxStyle.Information)
        Catch ex As System.Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnQueryStorageSupported_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQueryStorageSupported.Click
        Dim response As String() = oPhone.GetStorageSupported
        If Not response Is Nothing Then
            Dim i As Integer
            For i = 0 To response.Length - 1
                MsgBox("Storage: " + response(i))
            Next
        End If
    End Sub

    Private Sub btnQueryStorageSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQueryStorageSettings.Click
        Try
            Dim storage() As Storage = oPhone.GetStorageSetting()
            If Not storage Is Nothing Then
                Dim i As Integer
                For i = 0 To storage.Length - 1
                    MsgBox(storage(i).Name & ": Used " & storage(i).Used & " Total " & storage(i).Total, MsgBoxStyle.Information)
                Next
            End If
        Catch ex As System.Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnEnableCLIR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnableCLIR.Click
        If oPhone.EnableCLIR Then
            MsgBox("CLIR is enabled", MsgBoxStyle.Information)
        Else
            MsgBox("CLIR is not enabled", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnDisableCLIR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisableCLIR.Click
        If oPhone.DisableCLIR Then
            MsgBox("CLIR is disabled", MsgBoxStyle.Information)
        Else
            MsgBox("CLIR is not disabled", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnQueryLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQueryLocation.Click
        Dim loc As Location = oPhone.GetLocation
        If Not loc Is Nothing Then
            MsgBox("MCC: " & loc.MCC, MsgBoxStyle.Information)
            MsgBox("MNC: " & loc.MNC, MsgBoxStyle.Information)
            MsgBox("LAI: " & loc.LAI, MsgBoxStyle.Information)
            MsgBox("CELL ID: " & loc.CellID, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnSendHexSms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendHexSms.Click
        If Len(txtMSISDN.Text) = 0 Then
            MsgBox("Please enter a phone number", MsgBoxStyle.Information)
            Exit Sub
        End If
        oPhone.Encoding = EnumEncoding.Hex_Message
        Dim msgId As String = oPhone.SendSMS(txtMSISDN.Text, txtMsg.Text)
        MsgBox("Message sent. Message id is " + msgId)
    End Sub

    Private Sub btnQueryBatteryLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQueryBatteryLevel.Click
        Dim b As Battery = oPhone.GetBatteryLevel
        If Not b Is Nothing Then
            MsgBox("Min: " & b.MinimumLevel, MsgBoxStyle.Information)
            MsgBox("Max: " & b.MaximumLevel, MsgBoxStyle.Information)
            MsgBox("Level: " & b.BatteryLevel, MsgBoxStyle.Information)
            MsgBox("Charged: " & b.BatteryCharged, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnQueryRSSI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQueryRSSI.Click
        Dim r As Rssi = oPhone.GetRssi
        If Not r Is Nothing Then
            MsgBox("Min: " & r.Minimum, MsgBoxStyle.Information)
            MsgBox("Max: " & r.Maximum, MsgBoxStyle.Information)
            MsgBox("Level: " & r.Current, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnATDiagnostics_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnATDiagnostics.Click
        Try
            MsgBox("AT command file should be c:\temp\AT.txt, output file is c:\temp\ATout.txt", MsgBoxStyle.Information)
            If oPhone.Diagnose("c:\temp\at.txt", "c:\temp\atout.txt") Then
                MsgBox("Diagnostics done")
            End If
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub oPhone_NewIncomingCall(ByVal e As ATSMS.NewIncomingCallEventArgs) Handles _
                oPhone.NewIncomingCall
        txtMsg.Text = txtMsg.Text & "Incoming call from " & e.CallerID & ControlChars.CrLf
    End Sub


    Private Sub btnClass2SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClass2SMS.Click
        If Len(txtMSISDN.Text) = 0 Then
            MsgBox("Please enter a phone number", MsgBoxStyle.Information)
            Exit Sub
        End If
        oPhone.Encoding = EnumEncoding.Class2_7_Bit
        Dim msgId As String = oPhone.SendSMS(txtMSISDN.Text, txtMsg.Text)
        MsgBox("Message sent. Message id is " + msgId)
    End Sub

    Private Sub oPhone_NewMessageReceived(ByVal e As ATSMS.NewMessageReceivedEventArgs) Handles oPhone.NewMessageReceived
        txtMsg.Text = "Received msg from " & e.MSISDN & ControlChars.CrLf & e.TextMessage & ControlChars.CrLf
    End Sub

    Private Sub btnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        MsgBox(oPhone.About)
    End Sub


    Private Sub btnSMS8Bit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSMS8Bit.Click
        If Len(txtMSISDN.Text) = 0 Then
            MsgBox("Please enter a phone number", MsgBoxStyle.Information)
            Exit Sub
        End If
        oPhone.Encoding = EnumEncoding.Class2_8_Bit
        Dim msgId As String = oPhone.SendSMS(txtMSISDN.Text, txtMsg.Text)
        MsgBox("Message sent. Message id is " + msgId)
    End Sub

    Private Sub btnOutbox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutbox.Click
        If Len(txtMSISDN.Text) = 0 Then
            MsgBox("Please enter a phone number", MsgBoxStyle.Information)
            Exit Sub
        End If
        oPhone.Encoding = EnumEncoding.GSM_Default_7Bit
        Dim i As Integer
        For i = 1 To 3
            Dim msgId As String = oPhone.SendSMSToOutbox(txtMSISDN.Text, txtMsg.Text)
            MsgBox("Message sent to Outbox . Queue message id is " + msgId)
        Next
    End Sub

    Private Sub oPhone_OutboxSMSSent(ByVal e As ATSMS.OutboxSMSSentEventArgs) Handles oPhone.OutboxSMSSent
        MsgBox("Queue msg no " + e.QueueMessageKey + " Msg Ref No: " + e.MessageReference)
    End Sub

    Private Sub btnAnalyzePhone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnalyzePhone.Click
        If oPhone.IsConnected Then
            oPhone.CheckATCommands()
        End If
    End Sub

    Private Sub btnInbox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMsgStore.Click
        If oPhone.IsConnected Then
            Dim msgStore As MessageStore = oPhone.MessageStore
            oPhone.MessageMemory = EnumMessageMemory.PHONE
            msgStore.Refresh()
            Dim i As Integer
            For i = 0 To msgStore.Count - 1
                Dim sms As SMSMessage = msgStore.Message(i)
                MsgBox(sms.Text)
                sms.Delete()
            Next
        End If
    End Sub

    Private Sub btnManualConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManualConnect.Click
        Dim strComPort As String = InputBox("Enter COM Port", "COM Port Setting")
        Try
            If oPhone.IsConnected Then oPhone.Disconnect()
            oPhone.Port = strComPort
            oPhone.BaudRate = 9600
            oPhone.DataBits = 8
            oPhone.Parity = EnumParity.None
            oPhone.StopBits = EnumStopBits.One
            oPhone.FlowControl = EnumFlowControl.RTS_CTS
            oPhone.Connect()
            MsgBox("Connected to " + oPhone.Port, MsgBoxStyle.Information)
            oPhone.Echo = False
            EnableButtons()
        Catch ex As GeneralException
            DisableButtons()
            MsgBox("Error connecting: " + ex.Message)
        End Try
    End Sub

    Private Sub EnableButtons()
        btnAutoDetect.Enabled = False
        btnManualConnect.Enabled = False
        btnDisconnect.Enabled = True
        btnIMSI.Enabled = True
        btnIMEI.Enabled = True
        btnPhoneModel.Enabled = True
        btnDial.Enabled = True
        btnAnswer.Enabled = True
        btnHangup.Enabled = True
        btnDtmfDigits.Enabled = True
        btnSendDtmf.Enabled = True
        btnSendTextSms.Enabled = True
        btnSendHexSms.Enabled = True
        btnSendUnicodeSms.Enabled = True
        btnSMSC.Enabled = True
        btnEnableCLIP.Enabled = True
        btnDisableCLIP.Enabled = True
        btnMSISDN.Enabled = True
        btnManufacturer.Enabled = True
        btnRevision.Enabled = True
        btnInitMsgIndication.Enabled = True
        btnQueryMsgIndication.Enabled = True
        btnQueryStorageSupported.Enabled = True
        btnQueryStorageSettings.Enabled = True
        btnEnableCLIR.Enabled = True
        btnDisableCLIR.Enabled = True
        btnQueryLocation.Enabled = True
        btnQueryBatteryLevel.Enabled = True
        btnQueryRSSI.Enabled = True
        btnATDiagnostics.Enabled = True
        btnClass2SMS.Enabled = True
        btnSMS8Bit.Enabled = True
        btnOutbox.Enabled = True
        btnAnalyzePhone.Enabled = True
        btnMsgStore.Enabled = True
        btnVCard.Enabled = True
        btnVCalendar.Enabled = True
        btnWapPush.Enabled = True
        btnDeleteMsg.Enabled = True

        oPhone.NewMessageIndication = True
        oPhone.IncomingCallIndication = True

    End Sub

    Private Sub DisableButtons()
        btnDisconnect.Enabled = False
        btnIMSI.Enabled = False
        btnIMEI.Enabled = False
        btnPhoneModel.Enabled = False
        btnDial.Enabled = False
        btnAnswer.Enabled = False
        btnHangup.Enabled = False
        btnDtmfDigits.Enabled = False
        btnSendDtmf.Enabled = False
        btnSendTextSms.Enabled = False
        btnSendHexSms.Enabled = False
        btnSendUnicodeSms.Enabled = False
        btnSMSC.Enabled = False
        btnEnableCLIP.Enabled = False
        btnDisableCLIP.Enabled = False
        btnMSISDN.Enabled = False
        btnManufacturer.Enabled = False
        btnRevision.Enabled = False
        btnInitMsgIndication.Enabled = False
        btnQueryMsgIndication.Enabled = False
        btnQueryStorageSupported.Enabled = False
        btnQueryStorageSettings.Enabled = False
        btnEnableCLIR.Enabled = False
        btnDisableCLIR.Enabled = False
        btnQueryLocation.Enabled = False
        btnQueryBatteryLevel.Enabled = False
        btnQueryRSSI.Enabled = False
        btnATDiagnostics.Enabled = False
        btnClass2SMS.Enabled = False
        btnSMS8Bit.Enabled = False
        btnOutbox.Enabled = False
        btnAnalyzePhone.Enabled = False
        btnMsgStore.Enabled = False
        btnVCard.Enabled = False
        btnVCalendar.Enabled = False
        btnWapPush.Enabled = False
        btnDeleteMsg.Enabled = False
    End Sub

    Private Sub btnVCard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVCard.Click
        Dim v As New vCard
        v.LastName = "Meng Wang"
        'v.Emails = New vCard.vEmails()
        'v.Emails.Add(New vCard.vEmail("mengwangk@gmail.com"))
        v.Telephones = New vCard.vTelephones
        v.Telephones.Add(New vCard.vTelephone("0192292309", vCard.vLocations.CELL, vCard.vPhoneTypes.VOICE, True))
        If Len(txtMSISDN.Text) = 0 Then
            MsgBox("Please enter a phone number", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim msgId As String = oPhone.SendSMS(txtMSISDN.Text, v.ToString)
        MsgBox("Message sent. Message id is " + msgId)
    End Sub

    Private Sub btnVCalendar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVCalendar.Click
        Dim v As New vCalendar
        Dim vE As New vCalendar.vEvent
        vE.DTStart = New Date
        vE.DTEnd = New Date
        vE.Organizer = "Test Org"
        vE.Summary = "Test Summary"
        v.Events.Add(vE)
        If Len(txtMSISDN.Text) = 0 Then
            MsgBox("Please enter a phone number", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim msgId As String = oPhone.SendSMS(txtMSISDN.Text, v.ToString)
        MsgBox("Message sent. Message id is " + msgId)
    End Sub

    Private Sub btnWapPush_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWapPush.Click

        Dim href As String = "http://www.sharemymobile.com"
        Dim text As String = "Share your mobile today"
        Dim body As String = New WapPushMessage(href, text).ToString
        MsgBox(body)
        If Len(txtMSISDN.Text) = 0 Then
            MsgBox("Please enter a phone number", MsgBoxStyle.Information)
            Exit Sub
        End If
        oPhone.Encoding = EnumEncoding.Hex_Message
        Dim msgId As String = oPhone.SendSMS(txtMSISDN.Text, body)
        MsgBox("Message sent. Message id is " + msgId)
    End Sub

    Private Sub btnDeleteMsg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteMsg.Click
        If oPhone.IsConnected Then
            Dim msgStore As MessageStore = oPhone.MessageStore
            oPhone.MessageMemory = EnumMessageMemory.PHONE
            msgStore.Refresh()
            Dim i As Integer
            For i = 0 To msgStore.Count - 1
                Dim sms As SMSMessage = msgStore.Message(i)
                sms.Delete()
            Next
        End If
    End Sub
End Class