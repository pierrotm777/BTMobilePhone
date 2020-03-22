<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestATSMS
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TestATSMS))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtMSISDN = New System.Windows.Forms.TextBox
        Me.txtMsg = New System.Windows.Forms.TextBox
        Me.btnAutoDetect = New System.Windows.Forms.Button
        Me.btnIMSI = New System.Windows.Forms.Button
        Me.btnIMEI = New System.Windows.Forms.Button
        Me.btnDisconnect = New System.Windows.Forms.Button
        Me.btnPhoneModel = New System.Windows.Forms.Button
        Me.btnDial = New System.Windows.Forms.Button
        Me.btnAnswer = New System.Windows.Forms.Button
        Me.btnHangup = New System.Windows.Forms.Button
        Me.btnDtmfDigits = New System.Windows.Forms.Button
        Me.btnSendDtmf = New System.Windows.Forms.Button
        Me.btnSMSC = New System.Windows.Forms.Button
        Me.btnSendTextSms = New System.Windows.Forms.Button
        Me.btnSendHexSms = New System.Windows.Forms.Button
        Me.btnSendUnicodeSms = New System.Windows.Forms.Button
        Me.btnEnableCLIP = New System.Windows.Forms.Button
        Me.btnDisableCLIP = New System.Windows.Forms.Button
        Me.btnMSISDN = New System.Windows.Forms.Button
        Me.btnManufacturer = New System.Windows.Forms.Button
        Me.btnRevision = New System.Windows.Forms.Button
        Me.btnPDUEncoder = New System.Windows.Forms.Button
        Me.btnPDUDecoder = New System.Windows.Forms.Button
        Me.btnInitMsgIndication = New System.Windows.Forms.Button
        Me.btnQueryMsgIndication = New System.Windows.Forms.Button
        Me.btnQueryStorageSupported = New System.Windows.Forms.Button
        Me.btnQueryStorageSettings = New System.Windows.Forms.Button
        Me.btnEnableCLIR = New System.Windows.Forms.Button
        Me.btnDisableCLIR = New System.Windows.Forms.Button
        Me.btnQueryLocation = New System.Windows.Forms.Button
        Me.btnQueryBatteryLevel = New System.Windows.Forms.Button
        Me.btnQueryRSSI = New System.Windows.Forms.Button
        Me.btnATDiagnostics = New System.Windows.Forms.Button
        Me.btnClass2SMS = New System.Windows.Forms.Button
        Me.btnAbout = New System.Windows.Forms.Button
        Me.btnSMS8Bit = New System.Windows.Forms.Button
        Me.btnOutbox = New System.Windows.Forms.Button
        Me.btnAnalyzePhone = New System.Windows.Forms.Button
        Me.btnMsgStore = New System.Windows.Forms.Button
        Me.btnManualConnect = New System.Windows.Forms.Button
        Me.btnVCard = New System.Windows.Forms.Button
        Me.btnVCalendar = New System.Windows.Forms.Button
        Me.btnWapPush = New System.Windows.Forms.Button
        Me.btnDeleteMsg = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "MSISDN"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Message"
        '
        'txtMSISDN
        '
        Me.txtMSISDN.Location = New System.Drawing.Point(80, 13)
        Me.txtMSISDN.Name = "txtMSISDN"
        Me.txtMSISDN.Size = New System.Drawing.Size(138, 20)
        Me.txtMSISDN.TabIndex = 2
        '
        'txtMsg
        '
        Me.txtMsg.Location = New System.Drawing.Point(81, 49)
        Me.txtMsg.Multiline = True
        Me.txtMsg.Name = "txtMsg"
        Me.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtMsg.Size = New System.Drawing.Size(269, 60)
        Me.txtMsg.TabIndex = 3
        '
        'btnAutoDetect
        '
        Me.btnAutoDetect.Location = New System.Drawing.Point(377, 5)
        Me.btnAutoDetect.Name = "btnAutoDetect"
        Me.btnAutoDetect.Size = New System.Drawing.Size(117, 28)
        Me.btnAutoDetect.TabIndex = 4
        Me.btnAutoDetect.Text = "AutoDetect"
        Me.btnAutoDetect.UseVisualStyleBackColor = True
        '
        'btnIMSI
        '
        Me.btnIMSI.Enabled = False
        Me.btnIMSI.Location = New System.Drawing.Point(13, 116)
        Me.btnIMSI.Name = "btnIMSI"
        Me.btnIMSI.Size = New System.Drawing.Size(79, 30)
        Me.btnIMSI.TabIndex = 5
        Me.btnIMSI.Text = "IMSI"
        Me.btnIMSI.UseVisualStyleBackColor = True
        '
        'btnIMEI
        '
        Me.btnIMEI.Enabled = False
        Me.btnIMEI.Location = New System.Drawing.Point(13, 152)
        Me.btnIMEI.Name = "btnIMEI"
        Me.btnIMEI.Size = New System.Drawing.Size(79, 30)
        Me.btnIMEI.TabIndex = 6
        Me.btnIMEI.Text = "IMEI"
        Me.btnIMEI.UseVisualStyleBackColor = True
        '
        'btnDisconnect
        '
        Me.btnDisconnect.Enabled = False
        Me.btnDisconnect.Location = New System.Drawing.Point(377, 76)
        Me.btnDisconnect.Name = "btnDisconnect"
        Me.btnDisconnect.Size = New System.Drawing.Size(117, 30)
        Me.btnDisconnect.TabIndex = 7
        Me.btnDisconnect.Text = "Disconnect"
        Me.btnDisconnect.UseVisualStyleBackColor = True
        '
        'btnPhoneModel
        '
        Me.btnPhoneModel.Enabled = False
        Me.btnPhoneModel.Location = New System.Drawing.Point(13, 188)
        Me.btnPhoneModel.Name = "btnPhoneModel"
        Me.btnPhoneModel.Size = New System.Drawing.Size(79, 32)
        Me.btnPhoneModel.TabIndex = 8
        Me.btnPhoneModel.Text = "Phone Model"
        Me.btnPhoneModel.UseVisualStyleBackColor = True
        '
        'btnDial
        '
        Me.btnDial.Enabled = False
        Me.btnDial.Location = New System.Drawing.Point(13, 226)
        Me.btnDial.Name = "btnDial"
        Me.btnDial.Size = New System.Drawing.Size(79, 32)
        Me.btnDial.TabIndex = 9
        Me.btnDial.Text = "Dial"
        Me.btnDial.UseVisualStyleBackColor = True
        '
        'btnAnswer
        '
        Me.btnAnswer.Enabled = False
        Me.btnAnswer.Location = New System.Drawing.Point(13, 264)
        Me.btnAnswer.Name = "btnAnswer"
        Me.btnAnswer.Size = New System.Drawing.Size(79, 32)
        Me.btnAnswer.TabIndex = 10
        Me.btnAnswer.Text = "Answer"
        Me.btnAnswer.UseVisualStyleBackColor = True
        '
        'btnHangup
        '
        Me.btnHangup.Enabled = False
        Me.btnHangup.Location = New System.Drawing.Point(13, 303)
        Me.btnHangup.Name = "btnHangup"
        Me.btnHangup.Size = New System.Drawing.Size(79, 32)
        Me.btnHangup.TabIndex = 11
        Me.btnHangup.Text = "Hangup"
        Me.btnHangup.UseVisualStyleBackColor = True
        '
        'btnDtmfDigits
        '
        Me.btnDtmfDigits.Enabled = False
        Me.btnDtmfDigits.Location = New System.Drawing.Point(98, 116)
        Me.btnDtmfDigits.Name = "btnDtmfDigits"
        Me.btnDtmfDigits.Size = New System.Drawing.Size(79, 32)
        Me.btnDtmfDigits.TabIndex = 12
        Me.btnDtmfDigits.Text = "DTMF Digits"
        Me.btnDtmfDigits.UseVisualStyleBackColor = True
        '
        'btnSendDtmf
        '
        Me.btnSendDtmf.Enabled = False
        Me.btnSendDtmf.Location = New System.Drawing.Point(97, 154)
        Me.btnSendDtmf.Name = "btnSendDtmf"
        Me.btnSendDtmf.Size = New System.Drawing.Size(80, 30)
        Me.btnSendDtmf.TabIndex = 13
        Me.btnSendDtmf.Text = "SendDtmf"
        Me.btnSendDtmf.UseVisualStyleBackColor = True
        '
        'btnSMSC
        '
        Me.btnSMSC.Enabled = False
        Me.btnSMSC.Location = New System.Drawing.Point(98, 192)
        Me.btnSMSC.Name = "btnSMSC"
        Me.btnSMSC.Size = New System.Drawing.Size(80, 30)
        Me.btnSMSC.TabIndex = 14
        Me.btnSMSC.Text = "SMSC"
        Me.btnSMSC.UseVisualStyleBackColor = True
        '
        'btnSendTextSms
        '
        Me.btnSendTextSms.Enabled = False
        Me.btnSendTextSms.Location = New System.Drawing.Point(98, 230)
        Me.btnSendTextSms.Name = "btnSendTextSms"
        Me.btnSendTextSms.Size = New System.Drawing.Size(80, 30)
        Me.btnSendTextSms.TabIndex = 15
        Me.btnSendTextSms.Text = "Text SMS"
        Me.btnSendTextSms.UseVisualStyleBackColor = True
        '
        'btnSendHexSms
        '
        Me.btnSendHexSms.Enabled = False
        Me.btnSendHexSms.Location = New System.Drawing.Point(98, 268)
        Me.btnSendHexSms.Name = "btnSendHexSms"
        Me.btnSendHexSms.Size = New System.Drawing.Size(80, 30)
        Me.btnSendHexSms.TabIndex = 16
        Me.btnSendHexSms.Text = "Hex SMS"
        Me.btnSendHexSms.UseVisualStyleBackColor = True
        '
        'btnSendUnicodeSms
        '
        Me.btnSendUnicodeSms.Enabled = False
        Me.btnSendUnicodeSms.Location = New System.Drawing.Point(97, 307)
        Me.btnSendUnicodeSms.Name = "btnSendUnicodeSms"
        Me.btnSendUnicodeSms.Size = New System.Drawing.Size(81, 30)
        Me.btnSendUnicodeSms.TabIndex = 17
        Me.btnSendUnicodeSms.Text = "Unicode SMS"
        Me.btnSendUnicodeSms.UseVisualStyleBackColor = True
        '
        'btnEnableCLIP
        '
        Me.btnEnableCLIP.Enabled = False
        Me.btnEnableCLIP.Location = New System.Drawing.Point(183, 115)
        Me.btnEnableCLIP.Name = "btnEnableCLIP"
        Me.btnEnableCLIP.Size = New System.Drawing.Size(80, 30)
        Me.btnEnableCLIP.TabIndex = 18
        Me.btnEnableCLIP.Text = "Enable CLIP"
        Me.btnEnableCLIP.UseVisualStyleBackColor = True
        '
        'btnDisableCLIP
        '
        Me.btnDisableCLIP.Enabled = False
        Me.btnDisableCLIP.Location = New System.Drawing.Point(183, 151)
        Me.btnDisableCLIP.Name = "btnDisableCLIP"
        Me.btnDisableCLIP.Size = New System.Drawing.Size(80, 30)
        Me.btnDisableCLIP.TabIndex = 19
        Me.btnDisableCLIP.Text = "Disable CLIP"
        Me.btnDisableCLIP.UseVisualStyleBackColor = True
        '
        'btnMSISDN
        '
        Me.btnMSISDN.Enabled = False
        Me.btnMSISDN.Location = New System.Drawing.Point(186, 187)
        Me.btnMSISDN.Name = "btnMSISDN"
        Me.btnMSISDN.Size = New System.Drawing.Size(76, 27)
        Me.btnMSISDN.TabIndex = 20
        Me.btnMSISDN.Text = "MSISDN"
        Me.btnMSISDN.UseVisualStyleBackColor = True
        '
        'btnManufacturer
        '
        Me.btnManufacturer.Enabled = False
        Me.btnManufacturer.Location = New System.Drawing.Point(184, 225)
        Me.btnManufacturer.Name = "btnManufacturer"
        Me.btnManufacturer.Size = New System.Drawing.Size(79, 27)
        Me.btnManufacturer.TabIndex = 21
        Me.btnManufacturer.Text = "Manufacturer"
        Me.btnManufacturer.UseVisualStyleBackColor = True
        '
        'btnRevision
        '
        Me.btnRevision.Enabled = False
        Me.btnRevision.Location = New System.Drawing.Point(186, 263)
        Me.btnRevision.Name = "btnRevision"
        Me.btnRevision.Size = New System.Drawing.Size(79, 27)
        Me.btnRevision.TabIndex = 22
        Me.btnRevision.Text = "Revision"
        Me.btnRevision.UseVisualStyleBackColor = True
        '
        'btnPDUEncoder
        '
        Me.btnPDUEncoder.Location = New System.Drawing.Point(544, 7)
        Me.btnPDUEncoder.Name = "btnPDUEncoder"
        Me.btnPDUEncoder.Size = New System.Drawing.Size(103, 31)
        Me.btnPDUEncoder.TabIndex = 23
        Me.btnPDUEncoder.Text = "PDU Encoder"
        Me.btnPDUEncoder.UseVisualStyleBackColor = True
        '
        'btnPDUDecoder
        '
        Me.btnPDUDecoder.Location = New System.Drawing.Point(544, 42)
        Me.btnPDUDecoder.Name = "btnPDUDecoder"
        Me.btnPDUDecoder.Size = New System.Drawing.Size(103, 31)
        Me.btnPDUDecoder.TabIndex = 24
        Me.btnPDUDecoder.Text = "PDU Decoder"
        Me.btnPDUDecoder.UseVisualStyleBackColor = True
        '
        'btnInitMsgIndication
        '
        Me.btnInitMsgIndication.Enabled = False
        Me.btnInitMsgIndication.Location = New System.Drawing.Point(269, 115)
        Me.btnInitMsgIndication.Name = "btnInitMsgIndication"
        Me.btnInitMsgIndication.Size = New System.Drawing.Size(149, 32)
        Me.btnInitMsgIndication.TabIndex = 25
        Me.btnInitMsgIndication.Text = "Init Msg Indication"
        Me.btnInitMsgIndication.UseVisualStyleBackColor = True
        '
        'btnQueryMsgIndication
        '
        Me.btnQueryMsgIndication.Enabled = False
        Me.btnQueryMsgIndication.Location = New System.Drawing.Point(269, 151)
        Me.btnQueryMsgIndication.Name = "btnQueryMsgIndication"
        Me.btnQueryMsgIndication.Size = New System.Drawing.Size(149, 30)
        Me.btnQueryMsgIndication.TabIndex = 26
        Me.btnQueryMsgIndication.Text = "Query Msg Indication"
        Me.btnQueryMsgIndication.UseVisualStyleBackColor = True
        '
        'btnQueryStorageSupported
        '
        Me.btnQueryStorageSupported.Enabled = False
        Me.btnQueryStorageSupported.Location = New System.Drawing.Point(269, 187)
        Me.btnQueryStorageSupported.Name = "btnQueryStorageSupported"
        Me.btnQueryStorageSupported.Size = New System.Drawing.Size(149, 29)
        Me.btnQueryStorageSupported.TabIndex = 27
        Me.btnQueryStorageSupported.Text = "Query Storage Supported"
        Me.btnQueryStorageSupported.UseVisualStyleBackColor = True
        '
        'btnQueryStorageSettings
        '
        Me.btnQueryStorageSettings.Enabled = False
        Me.btnQueryStorageSettings.Location = New System.Drawing.Point(269, 225)
        Me.btnQueryStorageSettings.Name = "btnQueryStorageSettings"
        Me.btnQueryStorageSettings.Size = New System.Drawing.Size(149, 29)
        Me.btnQueryStorageSettings.TabIndex = 28
        Me.btnQueryStorageSettings.Text = "Query Storage Settings"
        Me.btnQueryStorageSettings.UseVisualStyleBackColor = True
        '
        'btnEnableCLIR
        '
        Me.btnEnableCLIR.Enabled = False
        Me.btnEnableCLIR.Location = New System.Drawing.Point(186, 299)
        Me.btnEnableCLIR.Name = "btnEnableCLIR"
        Me.btnEnableCLIR.Size = New System.Drawing.Size(80, 30)
        Me.btnEnableCLIR.TabIndex = 29
        Me.btnEnableCLIR.Text = "Enable CLIR"
        Me.btnEnableCLIR.UseVisualStyleBackColor = True
        '
        'btnDisableCLIR
        '
        Me.btnDisableCLIR.Enabled = False
        Me.btnDisableCLIR.Location = New System.Drawing.Point(186, 338)
        Me.btnDisableCLIR.Name = "btnDisableCLIR"
        Me.btnDisableCLIR.Size = New System.Drawing.Size(80, 30)
        Me.btnDisableCLIR.TabIndex = 30
        Me.btnDisableCLIR.Text = "Disable CLIR"
        Me.btnDisableCLIR.UseVisualStyleBackColor = True
        '
        'btnQueryLocation
        '
        Me.btnQueryLocation.Enabled = False
        Me.btnQueryLocation.Location = New System.Drawing.Point(271, 262)
        Me.btnQueryLocation.Name = "btnQueryLocation"
        Me.btnQueryLocation.Size = New System.Drawing.Size(149, 29)
        Me.btnQueryLocation.TabIndex = 31
        Me.btnQueryLocation.Text = "Query Location"
        Me.btnQueryLocation.UseVisualStyleBackColor = True
        '
        'btnQueryBatteryLevel
        '
        Me.btnQueryBatteryLevel.Enabled = False
        Me.btnQueryBatteryLevel.Location = New System.Drawing.Point(272, 300)
        Me.btnQueryBatteryLevel.Name = "btnQueryBatteryLevel"
        Me.btnQueryBatteryLevel.Size = New System.Drawing.Size(149, 29)
        Me.btnQueryBatteryLevel.TabIndex = 32
        Me.btnQueryBatteryLevel.Text = "Query Battery Level"
        Me.btnQueryBatteryLevel.UseVisualStyleBackColor = True
        '
        'btnQueryRSSI
        '
        Me.btnQueryRSSI.Enabled = False
        Me.btnQueryRSSI.Location = New System.Drawing.Point(272, 339)
        Me.btnQueryRSSI.Name = "btnQueryRSSI"
        Me.btnQueryRSSI.Size = New System.Drawing.Size(149, 29)
        Me.btnQueryRSSI.TabIndex = 33
        Me.btnQueryRSSI.Text = "Query RSSI"
        Me.btnQueryRSSI.UseVisualStyleBackColor = True
        '
        'btnATDiagnostics
        '
        Me.btnATDiagnostics.Enabled = False
        Me.btnATDiagnostics.Location = New System.Drawing.Point(433, 114)
        Me.btnATDiagnostics.Name = "btnATDiagnostics"
        Me.btnATDiagnostics.Size = New System.Drawing.Size(113, 29)
        Me.btnATDiagnostics.TabIndex = 34
        Me.btnATDiagnostics.Text = "AT Diagnostics"
        Me.btnATDiagnostics.UseVisualStyleBackColor = True
        '
        'btnClass2SMS
        '
        Me.btnClass2SMS.Enabled = False
        Me.btnClass2SMS.Location = New System.Drawing.Point(433, 152)
        Me.btnClass2SMS.Name = "btnClass2SMS"
        Me.btnClass2SMS.Size = New System.Drawing.Size(113, 29)
        Me.btnClass2SMS.TabIndex = 35
        Me.btnClass2SMS.Text = "Class 2 7 Bit SMS "
        Me.btnClass2SMS.UseVisualStyleBackColor = True
        '
        'btnAbout
        '
        Me.btnAbout.Location = New System.Drawing.Point(544, 79)
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Size = New System.Drawing.Size(103, 25)
        Me.btnAbout.TabIndex = 36
        Me.btnAbout.Text = "About"
        Me.btnAbout.UseVisualStyleBackColor = True
        '
        'btnSMS8Bit
        '
        Me.btnSMS8Bit.Enabled = False
        Me.btnSMS8Bit.Location = New System.Drawing.Point(433, 187)
        Me.btnSMS8Bit.Name = "btnSMS8Bit"
        Me.btnSMS8Bit.Size = New System.Drawing.Size(113, 29)
        Me.btnSMS8Bit.TabIndex = 37
        Me.btnSMS8Bit.Text = "Class 2 8 Bit SMS"
        Me.btnSMS8Bit.UseVisualStyleBackColor = True
        '
        'btnOutbox
        '
        Me.btnOutbox.Enabled = False
        Me.btnOutbox.Location = New System.Drawing.Point(433, 225)
        Me.btnOutbox.Name = "btnOutbox"
        Me.btnOutbox.Size = New System.Drawing.Size(113, 35)
        Me.btnOutbox.TabIndex = 38
        Me.btnOutbox.Text = "Send to Outbox 3 Times"
        Me.btnOutbox.UseVisualStyleBackColor = True
        '
        'btnAnalyzePhone
        '
        Me.btnAnalyzePhone.Enabled = False
        Me.btnAnalyzePhone.Location = New System.Drawing.Point(433, 268)
        Me.btnAnalyzePhone.Name = "btnAnalyzePhone"
        Me.btnAnalyzePhone.Size = New System.Drawing.Size(113, 32)
        Me.btnAnalyzePhone.TabIndex = 39
        Me.btnAnalyzePhone.Text = "Analyze Phone"
        Me.btnAnalyzePhone.UseVisualStyleBackColor = True
        '
        'btnMsgStore
        '
        Me.btnMsgStore.Enabled = False
        Me.btnMsgStore.Location = New System.Drawing.Point(433, 308)
        Me.btnMsgStore.Name = "btnMsgStore"
        Me.btnMsgStore.Size = New System.Drawing.Size(113, 29)
        Me.btnMsgStore.TabIndex = 40
        Me.btnMsgStore.Text = "Message Store"
        Me.btnMsgStore.UseVisualStyleBackColor = True
        '
        'btnManualConnect
        '
        Me.btnManualConnect.Location = New System.Drawing.Point(377, 42)
        Me.btnManualConnect.Name = "btnManualConnect"
        Me.btnManualConnect.Size = New System.Drawing.Size(117, 28)
        Me.btnManualConnect.TabIndex = 41
        Me.btnManualConnect.Text = "Manual Connect"
        Me.btnManualConnect.UseVisualStyleBackColor = True
        '
        'btnVCard
        '
        Me.btnVCard.Enabled = False
        Me.btnVCard.Location = New System.Drawing.Point(564, 116)
        Me.btnVCard.Name = "btnVCard"
        Me.btnVCard.Size = New System.Drawing.Size(113, 29)
        Me.btnVCard.TabIndex = 42
        Me.btnVCard.Text = "vCard"
        Me.btnVCard.UseVisualStyleBackColor = True
        '
        'btnVCalendar
        '
        Me.btnVCalendar.Enabled = False
        Me.btnVCalendar.Location = New System.Drawing.Point(564, 151)
        Me.btnVCalendar.Name = "btnVCalendar"
        Me.btnVCalendar.Size = New System.Drawing.Size(113, 29)
        Me.btnVCalendar.TabIndex = 43
        Me.btnVCalendar.Text = "vCalendar"
        Me.btnVCalendar.UseVisualStyleBackColor = True
        '
        'btnWapPush
        '
        Me.btnWapPush.Enabled = False
        Me.btnWapPush.Location = New System.Drawing.Point(564, 186)
        Me.btnWapPush.Name = "btnWapPush"
        Me.btnWapPush.Size = New System.Drawing.Size(113, 29)
        Me.btnWapPush.TabIndex = 44
        Me.btnWapPush.Text = "Wap Push"
        Me.btnWapPush.UseVisualStyleBackColor = True
        '
        'btnDeleteMsg
        '
        Me.btnDeleteMsg.Enabled = False
        Me.btnDeleteMsg.Location = New System.Drawing.Point(433, 343)
        Me.btnDeleteMsg.Name = "btnDeleteMsg"
        Me.btnDeleteMsg.Size = New System.Drawing.Size(153, 29)
        Me.btnDeleteMsg.TabIndex = 45
        Me.btnDeleteMsg.Text = "Delete SIM Messages"
        Me.btnDeleteMsg.UseVisualStyleBackColor = True
        '
        'TestATSMS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(703, 395)
        Me.Controls.Add(Me.btnDeleteMsg)
        Me.Controls.Add(Me.btnWapPush)
        Me.Controls.Add(Me.btnVCalendar)
        Me.Controls.Add(Me.btnVCard)
        Me.Controls.Add(Me.btnManualConnect)
        Me.Controls.Add(Me.btnMsgStore)
        Me.Controls.Add(Me.btnAnalyzePhone)
        Me.Controls.Add(Me.btnOutbox)
        Me.Controls.Add(Me.btnSMS8Bit)
        Me.Controls.Add(Me.btnAbout)
        Me.Controls.Add(Me.btnClass2SMS)
        Me.Controls.Add(Me.btnATDiagnostics)
        Me.Controls.Add(Me.btnQueryRSSI)
        Me.Controls.Add(Me.btnQueryBatteryLevel)
        Me.Controls.Add(Me.btnQueryLocation)
        Me.Controls.Add(Me.btnDisableCLIR)
        Me.Controls.Add(Me.btnEnableCLIR)
        Me.Controls.Add(Me.btnQueryStorageSettings)
        Me.Controls.Add(Me.btnQueryStorageSupported)
        Me.Controls.Add(Me.btnQueryMsgIndication)
        Me.Controls.Add(Me.btnInitMsgIndication)
        Me.Controls.Add(Me.btnPDUDecoder)
        Me.Controls.Add(Me.btnPDUEncoder)
        Me.Controls.Add(Me.btnRevision)
        Me.Controls.Add(Me.btnManufacturer)
        Me.Controls.Add(Me.btnMSISDN)
        Me.Controls.Add(Me.btnDisableCLIP)
        Me.Controls.Add(Me.btnEnableCLIP)
        Me.Controls.Add(Me.btnSendUnicodeSms)
        Me.Controls.Add(Me.btnSendHexSms)
        Me.Controls.Add(Me.btnSendTextSms)
        Me.Controls.Add(Me.btnSMSC)
        Me.Controls.Add(Me.btnSendDtmf)
        Me.Controls.Add(Me.btnDtmfDigits)
        Me.Controls.Add(Me.btnHangup)
        Me.Controls.Add(Me.btnAnswer)
        Me.Controls.Add(Me.btnDial)
        Me.Controls.Add(Me.btnPhoneModel)
        Me.Controls.Add(Me.btnDisconnect)
        Me.Controls.Add(Me.btnIMEI)
        Me.Controls.Add(Me.btnIMSI)
        Me.Controls.Add(Me.btnAutoDetect)
        Me.Controls.Add(Me.txtMsg)
        Me.Controls.Add(Me.txtMSISDN)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "TestATSMS"
        Me.Text = "ATSMS Test"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMSISDN As System.Windows.Forms.TextBox
    Friend WithEvents txtMsg As System.Windows.Forms.TextBox
    Friend WithEvents btnAutoDetect As System.Windows.Forms.Button
    Friend WithEvents btnIMSI As System.Windows.Forms.Button
    Friend WithEvents btnIMEI As System.Windows.Forms.Button
    Friend WithEvents btnDisconnect As System.Windows.Forms.Button
    Friend WithEvents btnPhoneModel As System.Windows.Forms.Button
    Friend WithEvents btnDial As System.Windows.Forms.Button
    Friend WithEvents btnAnswer As System.Windows.Forms.Button
    Friend WithEvents btnHangup As System.Windows.Forms.Button
    Friend WithEvents btnDtmfDigits As System.Windows.Forms.Button
    Friend WithEvents btnSendDtmf As System.Windows.Forms.Button
    Friend WithEvents btnSMSC As System.Windows.Forms.Button
    Friend WithEvents btnSendTextSms As System.Windows.Forms.Button
    Friend WithEvents btnSendHexSms As System.Windows.Forms.Button
    Friend WithEvents btnSendUnicodeSms As System.Windows.Forms.Button
    Friend WithEvents btnEnableCLIP As System.Windows.Forms.Button
    Friend WithEvents btnDisableCLIP As System.Windows.Forms.Button
    Friend WithEvents btnMSISDN As System.Windows.Forms.Button
    Friend WithEvents btnManufacturer As System.Windows.Forms.Button
    Friend WithEvents btnRevision As System.Windows.Forms.Button
    Friend WithEvents btnPDUEncoder As System.Windows.Forms.Button
    Friend WithEvents btnPDUDecoder As System.Windows.Forms.Button
    Friend WithEvents btnInitMsgIndication As System.Windows.Forms.Button
    Friend WithEvents btnQueryMsgIndication As System.Windows.Forms.Button
    Friend WithEvents btnQueryStorageSupported As System.Windows.Forms.Button
    Friend WithEvents btnQueryStorageSettings As System.Windows.Forms.Button
    Friend WithEvents btnEnableCLIR As System.Windows.Forms.Button
    Friend WithEvents btnDisableCLIR As System.Windows.Forms.Button
    Friend WithEvents btnQueryLocation As System.Windows.Forms.Button
    Friend WithEvents btnQueryBatteryLevel As System.Windows.Forms.Button
    Friend WithEvents btnQueryRSSI As System.Windows.Forms.Button
    Friend WithEvents btnATDiagnostics As System.Windows.Forms.Button
    Friend WithEvents btnClass2SMS As System.Windows.Forms.Button
    Friend WithEvents btnAbout As System.Windows.Forms.Button
    Friend WithEvents btnSMS8Bit As System.Windows.Forms.Button
    Friend WithEvents btnOutbox As System.Windows.Forms.Button
    Friend WithEvents btnAnalyzePhone As System.Windows.Forms.Button
    Friend WithEvents btnMsgStore As System.Windows.Forms.Button
    Friend WithEvents btnManualConnect As System.Windows.Forms.Button
    Friend WithEvents btnVCard As System.Windows.Forms.Button
    Friend WithEvents btnVCalendar As System.Windows.Forms.Button
    Friend WithEvents btnWapPush As System.Windows.Forms.Button
    Friend WithEvents btnDeleteMsg As System.Windows.Forms.Button
End Class
