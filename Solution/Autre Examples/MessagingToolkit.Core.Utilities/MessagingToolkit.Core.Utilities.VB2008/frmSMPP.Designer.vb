<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSMPP
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtServer = New System.Windows.Forms.TextBox
        Me.label9 = New System.Windows.Forms.Label
        Me.chkPayload = New System.Windows.Forms.CheckBox
        Me.btnSendSms = New System.Windows.Forms.Button
        Me.cboDataCoding = New System.Windows.Forms.ComboBox
        Me.label21 = New System.Windows.Forms.Label
        Me.txtMessage = New System.Windows.Forms.TextBox
        Me.groupBox4 = New System.Windows.Forms.GroupBox
        Me.txtSourceAddress = New System.Windows.Forms.TextBox
        Me.label20 = New System.Windows.Forms.Label
        Me.label17 = New System.Windows.Forms.Label
        Me.cboDestinationNpi = New System.Windows.Forms.ComboBox
        Me.cboSourceTon = New System.Windows.Forms.ComboBox
        Me.label19 = New System.Windows.Forms.Label
        Me.label5 = New System.Windows.Forms.Label
        Me.cboDestinationTon = New System.Windows.Forms.ComboBox
        Me.cboSourceNpi = New System.Windows.Forms.ComboBox
        Me.label18 = New System.Windows.Forms.Label
        Me.txtRecipients = New System.Windows.Forms.TextBox
        Me.groupBox5 = New System.Windows.Forms.GroupBox
        Me.label15 = New System.Windows.Forms.Label
        Me.label7 = New System.Windows.Forms.Label
        Me.tabAbout = New System.Windows.Forms.TabPage
        Me.groupBox8 = New System.Windows.Forms.GroupBox
        Me.lblLicense = New System.Windows.Forms.Label
        Me.lblAbout = New System.Windows.Forms.Label
        Me.linkLabel1 = New System.Windows.Forms.LinkLabel
        Me.cboNpi = New System.Windows.Forms.ComboBox
        Me.label14 = New System.Windows.Forms.Label
        Me.cboTon = New System.Windows.Forms.ComboBox
        Me.label16 = New System.Windows.Forms.Label
        Me.txtAddressRange = New System.Windows.Forms.TextBox
        Me.txtSystemType = New System.Windows.Forms.TextBox
        Me.label8 = New System.Windows.Forms.Label
        Me.label13 = New System.Windows.Forms.Label
        Me.txtReceivedMessage = New System.Windows.Forms.TextBox
        Me.txtServerKeepAlive = New System.Windows.Forms.TextBox
        Me.label12 = New System.Windows.Forms.Label
        Me.label11 = New System.Windows.Forms.Label
        Me.label10 = New System.Windows.Forms.Label
        Me.label4 = New System.Windows.Forms.Label
        Me.groupBox6 = New System.Windows.Forms.GroupBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabSettings = New System.Windows.Forms.TabPage
        Me.groupBox44 = New System.Windows.Forms.GroupBox
        Me.txtLogFile = New System.Windows.Forms.TextBox
        Me.groupBox3 = New System.Windows.Forms.GroupBox
        Me.btnDisconnect = New System.Windows.Forms.Button
        Me.btnConnect = New System.Windows.Forms.Button
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.label6 = New System.Windows.Forms.Label
        Me.txtSleepAfterSocketFailure = New System.Windows.Forms.TextBox
        Me.cboInterfaceVersion = New System.Windows.Forms.ComboBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.txtSystemId = New System.Windows.Forms.TextBox
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.label1 = New System.Windows.Forms.Label
        Me.label26 = New System.Windows.Forms.Label
        Me.cboSystemMode = New System.Windows.Forms.ComboBox
        Me.tabSendReceiveSms = New System.Windows.Forms.TabPage
        Me.groupBox7 = New System.Windows.Forms.GroupBox
        Me.chkDeliveryReport = New System.Windows.Forms.CheckBox
        Me.groupBox4.SuspendLayout()
        Me.groupBox5.SuspendLayout()
        Me.tabAbout.SuspendLayout()
        Me.groupBox8.SuspendLayout()
        Me.groupBox6.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabSettings.SuspendLayout()
        Me.groupBox44.SuspendLayout()
        Me.groupBox3.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.tabSendReceiveSms.SuspendLayout()
        Me.groupBox7.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtServer
        '
        Me.txtServer.Location = New System.Drawing.Point(82, 19)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(204, 20)
        Me.txtServer.TabIndex = 25
        Me.txtServer.Text = "localhost"
        '
        'label9
        '
        Me.label9.AutoSize = True
        Me.label9.Location = New System.Drawing.Point(308, 78)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(66, 13)
        Me.label9.TabIndex = 71
        Me.label9.Text = "Data Coding"
        '
        'chkPayload
        '
        Me.chkPayload.Location = New System.Drawing.Point(114, 111)
        Me.chkPayload.Name = "chkPayload"
        Me.chkPayload.Size = New System.Drawing.Size(207, 24)
        Me.chkPayload.TabIndex = 62
        Me.chkPayload.Text = "Payload "
        '
        'btnSendSms
        '
        Me.btnSendSms.Location = New System.Drawing.Point(216, 141)
        Me.btnSendSms.Name = "btnSendSms"
        Me.btnSendSms.Size = New System.Drawing.Size(137, 29)
        Me.btnSendSms.TabIndex = 61
        Me.btnSendSms.Text = "Send"
        Me.btnSendSms.UseVisualStyleBackColor = True
        '
        'cboDataCoding
        '
        Me.cboDataCoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDataCoding.FormattingEnabled = True
        Me.cboDataCoding.Location = New System.Drawing.Point(401, 72)
        Me.cboDataCoding.Name = "cboDataCoding"
        Me.cboDataCoding.Size = New System.Drawing.Size(148, 21)
        Me.cboDataCoding.TabIndex = 70
        '
        'label21
        '
        Me.label21.AutoSize = True
        Me.label21.Location = New System.Drawing.Point(24, 48)
        Me.label21.Name = "label21"
        Me.label21.Size = New System.Drawing.Size(49, 13)
        Me.label21.TabIndex = 59
        Me.label21.Text = "Message"
        '
        'txtMessage
        '
        Me.txtMessage.Location = New System.Drawing.Point(117, 45)
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(338, 66)
        Me.txtMessage.TabIndex = 58
        '
        'groupBox4
        '
        Me.groupBox4.Controls.Add(Me.label9)
        Me.groupBox4.Controls.Add(Me.cboDataCoding)
        Me.groupBox4.Controls.Add(Me.txtSourceAddress)
        Me.groupBox4.Controls.Add(Me.label20)
        Me.groupBox4.Controls.Add(Me.label17)
        Me.groupBox4.Controls.Add(Me.cboDestinationNpi)
        Me.groupBox4.Controls.Add(Me.cboSourceTon)
        Me.groupBox4.Controls.Add(Me.label19)
        Me.groupBox4.Controls.Add(Me.label5)
        Me.groupBox4.Controls.Add(Me.cboDestinationTon)
        Me.groupBox4.Controls.Add(Me.cboSourceNpi)
        Me.groupBox4.Controls.Add(Me.label18)
        Me.groupBox4.Location = New System.Drawing.Point(5, 3)
        Me.groupBox4.Name = "groupBox4"
        Me.groupBox4.Size = New System.Drawing.Size(661, 99)
        Me.groupBox4.TabIndex = 70
        Me.groupBox4.TabStop = False
        Me.groupBox4.Text = "Configuration Settings"
        '
        'txtSourceAddress
        '
        Me.txtSourceAddress.Location = New System.Drawing.Point(115, 19)
        Me.txtSourceAddress.Name = "txtSourceAddress"
        Me.txtSourceAddress.Size = New System.Drawing.Size(148, 20)
        Me.txtSourceAddress.TabIndex = 54
        Me.txtSourceAddress.Text = "4477665544"
        '
        'label20
        '
        Me.label20.AutoSize = True
        Me.label20.Location = New System.Drawing.Point(21, 76)
        Me.label20.Name = "label20"
        Me.label20.Size = New System.Drawing.Size(81, 13)
        Me.label20.TabIndex = 69
        Me.label20.Text = "Destination NPI"
        '
        'label17
        '
        Me.label17.AutoSize = True
        Me.label17.Location = New System.Drawing.Point(22, 22)
        Me.label17.Name = "label17"
        Me.label17.Size = New System.Drawing.Size(82, 13)
        Me.label17.TabIndex = 55
        Me.label17.Text = "Source Address"
        '
        'cboDestinationNpi
        '
        Me.cboDestinationNpi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDestinationNpi.FormattingEnabled = True
        Me.cboDestinationNpi.Location = New System.Drawing.Point(115, 72)
        Me.cboDestinationNpi.Name = "cboDestinationNpi"
        Me.cboDestinationNpi.Size = New System.Drawing.Size(148, 21)
        Me.cboDestinationNpi.TabIndex = 68
        '
        'cboSourceTon
        '
        Me.cboSourceTon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSourceTon.FormattingEnabled = True
        Me.cboSourceTon.Location = New System.Drawing.Point(401, 18)
        Me.cboSourceTon.Name = "cboSourceTon"
        Me.cboSourceTon.Size = New System.Drawing.Size(148, 21)
        Me.cboSourceTon.TabIndex = 62
        '
        'label19
        '
        Me.label19.AutoSize = True
        Me.label19.Location = New System.Drawing.Point(21, 49)
        Me.label19.Name = "label19"
        Me.label19.Size = New System.Drawing.Size(85, 13)
        Me.label19.TabIndex = 67
        Me.label19.Text = "Destination TON"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(307, 22)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(64, 13)
        Me.label5.TabIndex = 63
        Me.label5.Text = "Source TON"
        '
        'cboDestinationTon
        '
        Me.cboDestinationTon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDestinationTon.FormattingEnabled = True
        Me.cboDestinationTon.Location = New System.Drawing.Point(115, 45)
        Me.cboDestinationTon.Name = "cboDestinationTon"
        Me.cboDestinationTon.Size = New System.Drawing.Size(148, 21)
        Me.cboDestinationTon.TabIndex = 66
        '
        'cboSourceNpi
        '
        Me.cboSourceNpi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSourceNpi.FormattingEnabled = True
        Me.cboSourceNpi.Location = New System.Drawing.Point(401, 46)
        Me.cboSourceNpi.Name = "cboSourceNpi"
        Me.cboSourceNpi.Size = New System.Drawing.Size(148, 21)
        Me.cboSourceNpi.TabIndex = 64
        '
        'label18
        '
        Me.label18.AutoSize = True
        Me.label18.Location = New System.Drawing.Point(307, 49)
        Me.label18.Name = "label18"
        Me.label18.Size = New System.Drawing.Size(60, 13)
        Me.label18.TabIndex = 65
        Me.label18.Text = "Source NPI"
        '
        'txtRecipients
        '
        Me.txtRecipients.Location = New System.Drawing.Point(117, 19)
        Me.txtRecipients.Name = "txtRecipients"
        Me.txtRecipients.Size = New System.Drawing.Size(338, 20)
        Me.txtRecipients.TabIndex = 56
        Me.txtRecipients.Text = "337788665522"
        '
        'groupBox5
        '
        Me.groupBox5.Controls.Add(Me.chkPayload)
        Me.groupBox5.Controls.Add(Me.btnSendSms)
        Me.groupBox5.Controls.Add(Me.label21)
        Me.groupBox5.Controls.Add(Me.txtMessage)
        Me.groupBox5.Controls.Add(Me.txtRecipients)
        Me.groupBox5.Controls.Add(Me.label15)
        Me.groupBox5.Location = New System.Drawing.Point(3, 108)
        Me.groupBox5.Name = "groupBox5"
        Me.groupBox5.Size = New System.Drawing.Size(472, 177)
        Me.groupBox5.TabIndex = 71
        Me.groupBox5.TabStop = False
        Me.groupBox5.Text = "Send SMS"
        '
        'label15
        '
        Me.label15.AutoSize = True
        Me.label15.Location = New System.Drawing.Point(24, 22)
        Me.label15.Name = "label15"
        Me.label15.Size = New System.Drawing.Size(56, 13)
        Me.label15.TabIndex = 57
        Me.label15.Text = "Recipients"
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(368, 77)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(24, 13)
        Me.label7.TabIndex = 69
        Me.label7.Text = "NPI"
        '
        'tabAbout
        '
        Me.tabAbout.Controls.Add(Me.groupBox8)
        Me.tabAbout.Location = New System.Drawing.Point(4, 22)
        Me.tabAbout.Name = "tabAbout"
        Me.tabAbout.Size = New System.Drawing.Size(674, 429)
        Me.tabAbout.TabIndex = 3
        Me.tabAbout.Text = "About"
        Me.tabAbout.UseVisualStyleBackColor = True
        '
        'groupBox8
        '
        Me.groupBox8.Controls.Add(Me.lblLicense)
        Me.groupBox8.Controls.Add(Me.lblAbout)
        Me.groupBox8.Controls.Add(Me.linkLabel1)
        Me.groupBox8.Location = New System.Drawing.Point(3, 0)
        Me.groupBox8.Name = "groupBox8"
        Me.groupBox8.Size = New System.Drawing.Size(665, 426)
        Me.groupBox8.TabIndex = 1
        Me.groupBox8.TabStop = False
        '
        'lblLicense
        '
        Me.lblLicense.AutoSize = True
        Me.lblLicense.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLicense.Location = New System.Drawing.Point(271, 104)
        Me.lblLicense.Name = "lblLicense"
        Me.lblLicense.Size = New System.Drawing.Size(0, 20)
        Me.lblLicense.TabIndex = 2
        Me.lblLicense.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblAbout
        '
        Me.lblAbout.AutoSize = True
        Me.lblAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAbout.Location = New System.Drawing.Point(225, 139)
        Me.lblAbout.Name = "lblAbout"
        Me.lblAbout.Size = New System.Drawing.Size(60, 20)
        Me.lblAbout.TabIndex = 1
        Me.lblAbout.Text = "label12"
        Me.lblAbout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'linkLabel1
        '
        Me.linkLabel1.AutoSize = True
        Me.linkLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.linkLabel1.Location = New System.Drawing.Point(252, 187)
        Me.linkLabel1.Name = "linkLabel1"
        Me.linkLabel1.Size = New System.Drawing.Size(162, 20)
        Me.linkLabel1.TabIndex = 0
        Me.linkLabel1.TabStop = True
        Me.linkLabel1.Text = "http://www.twit88.com"
        '
        'cboNpi
        '
        Me.cboNpi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboNpi.FormattingEnabled = True
        Me.cboNpi.Location = New System.Drawing.Point(462, 73)
        Me.cboNpi.Name = "cboNpi"
        Me.cboNpi.Size = New System.Drawing.Size(148, 21)
        Me.cboNpi.TabIndex = 68
        '
        'label14
        '
        Me.label14.AutoSize = True
        Me.label14.Location = New System.Drawing.Point(368, 50)
        Me.label14.Name = "label14"
        Me.label14.Size = New System.Drawing.Size(28, 13)
        Me.label14.TabIndex = 67
        Me.label14.Text = "TON"
        '
        'cboTon
        '
        Me.cboTon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTon.FormattingEnabled = True
        Me.cboTon.Location = New System.Drawing.Point(462, 46)
        Me.cboTon.Name = "cboTon"
        Me.cboTon.Size = New System.Drawing.Size(148, 21)
        Me.cboTon.TabIndex = 66
        '
        'label16
        '
        Me.label16.AutoSize = True
        Me.label16.Location = New System.Drawing.Point(369, 22)
        Me.label16.Name = "label16"
        Me.label16.Size = New System.Drawing.Size(80, 13)
        Me.label16.TabIndex = 51
        Me.label16.Text = "Address Range"
        '
        'txtAddressRange
        '
        Me.txtAddressRange.Location = New System.Drawing.Point(462, 19)
        Me.txtAddressRange.Name = "txtAddressRange"
        Me.txtAddressRange.Size = New System.Drawing.Size(148, 20)
        Me.txtAddressRange.TabIndex = 50
        '
        'txtSystemType
        '
        Me.txtSystemType.Location = New System.Drawing.Point(144, 94)
        Me.txtSystemType.Name = "txtSystemType"
        Me.txtSystemType.Size = New System.Drawing.Size(75, 20)
        Me.txtSystemType.TabIndex = 45
        Me.txtSystemType.Text = "SMPP"
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(8, 101)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(72, 13)
        Me.label8.TabIndex = 44
        Me.label8.Text = "System  Type"
        '
        'label13
        '
        Me.label13.AutoSize = True
        Me.label13.Location = New System.Drawing.Point(226, 70)
        Me.label13.Name = "label13"
        Me.label13.Size = New System.Drawing.Size(46, 13)
        Me.label13.TabIndex = 40
        Me.label13.Text = "seconds"
        '
        'txtReceivedMessage
        '
        Me.txtReceivedMessage.Location = New System.Drawing.Point(6, 19)
        Me.txtReceivedMessage.Multiline = True
        Me.txtReceivedMessage.Name = "txtReceivedMessage"
        Me.txtReceivedMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtReceivedMessage.Size = New System.Drawing.Size(649, 107)
        Me.txtReceivedMessage.TabIndex = 59
        '
        'txtServerKeepAlive
        '
        Me.txtServerKeepAlive.Location = New System.Drawing.Point(144, 67)
        Me.txtServerKeepAlive.Name = "txtServerKeepAlive"
        Me.txtServerKeepAlive.Size = New System.Drawing.Size(75, 20)
        Me.txtServerKeepAlive.TabIndex = 39
        Me.txtServerKeepAlive.Text = "0"
        '
        'label12
        '
        Me.label12.AutoSize = True
        Me.label12.Location = New System.Drawing.Point(8, 74)
        Me.label12.Name = "label12"
        Me.label12.Size = New System.Drawing.Size(92, 13)
        Me.label12.TabIndex = 38
        Me.label12.Text = "Server Keep Alive"
        '
        'label11
        '
        Me.label11.AutoSize = True
        Me.label11.Location = New System.Drawing.Point(225, 49)
        Me.label11.Name = "label11"
        Me.label11.Size = New System.Drawing.Size(46, 13)
        Me.label11.TabIndex = 37
        Me.label11.Text = "seconds"
        '
        'label10
        '
        Me.label10.AutoSize = True
        Me.label10.Location = New System.Drawing.Point(8, 49)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(131, 13)
        Me.label10.TabIndex = 36
        Me.label10.Text = "Sleep After Socket Failure"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(6, 69)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(53, 13)
        Me.label4.TabIndex = 31
        Me.label4.Text = "Password"
        '
        'groupBox6
        '
        Me.groupBox6.Controls.Add(Me.txtReceivedMessage)
        Me.groupBox6.Location = New System.Drawing.Point(3, 291)
        Me.groupBox6.Name = "groupBox6"
        Me.groupBox6.Size = New System.Drawing.Size(661, 132)
        Me.groupBox6.TabIndex = 72
        Me.groupBox6.TabStop = False
        Me.groupBox6.Text = "Received SMS"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(82, 66)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(204, 20)
        Me.txtPassword.TabIndex = 32
        Me.txtPassword.Text = "password"
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabSettings)
        Me.tabMain.Controls.Add(Me.tabSendReceiveSms)
        Me.tabMain.Controls.Add(Me.tabAbout)
        Me.tabMain.Location = New System.Drawing.Point(7, 7)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(682, 455)
        Me.tabMain.TabIndex = 28
        '
        'tabSettings
        '
        Me.tabSettings.Controls.Add(Me.groupBox44)
        Me.tabSettings.Controls.Add(Me.groupBox3)
        Me.tabSettings.Controls.Add(Me.groupBox2)
        Me.tabSettings.Controls.Add(Me.groupBox1)
        Me.tabSettings.Location = New System.Drawing.Point(4, 22)
        Me.tabSettings.Name = "tabSettings"
        Me.tabSettings.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSettings.Size = New System.Drawing.Size(674, 429)
        Me.tabSettings.TabIndex = 0
        Me.tabSettings.Text = "Settings"
        Me.tabSettings.UseVisualStyleBackColor = True
        '
        'groupBox44
        '
        Me.groupBox44.Controls.Add(Me.txtLogFile)
        Me.groupBox44.Location = New System.Drawing.Point(7, 322)
        Me.groupBox44.Name = "groupBox44"
        Me.groupBox44.Size = New System.Drawing.Size(649, 44)
        Me.groupBox44.TabIndex = 32
        Me.groupBox44.TabStop = False
        Me.groupBox44.Text = "Logging"
        '
        'txtLogFile
        '
        Me.txtLogFile.Location = New System.Drawing.Point(59, 14)
        Me.txtLogFile.Name = "txtLogFile"
        Me.txtLogFile.ReadOnly = True
        Me.txtLogFile.Size = New System.Drawing.Size(552, 20)
        Me.txtLogFile.TabIndex = 14
        '
        'groupBox3
        '
        Me.groupBox3.Controls.Add(Me.btnDisconnect)
        Me.groupBox3.Controls.Add(Me.btnConnect)
        Me.groupBox3.Location = New System.Drawing.Point(8, 366)
        Me.groupBox3.Name = "groupBox3"
        Me.groupBox3.Size = New System.Drawing.Size(647, 60)
        Me.groupBox3.TabIndex = 28
        Me.groupBox3.TabStop = False
        '
        'btnDisconnect
        '
        Me.btnDisconnect.Location = New System.Drawing.Point(310, 18)
        Me.btnDisconnect.Name = "btnDisconnect"
        Me.btnDisconnect.Size = New System.Drawing.Size(98, 29)
        Me.btnDisconnect.TabIndex = 37
        Me.btnDisconnect.Text = "Disconnect"
        Me.btnDisconnect.UseVisualStyleBackColor = True
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(194, 18)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(98, 29)
        Me.btnConnect.TabIndex = 36
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.label7)
        Me.groupBox2.Controls.Add(Me.cboNpi)
        Me.groupBox2.Controls.Add(Me.label14)
        Me.groupBox2.Controls.Add(Me.cboTon)
        Me.groupBox2.Controls.Add(Me.label16)
        Me.groupBox2.Controls.Add(Me.txtAddressRange)
        Me.groupBox2.Controls.Add(Me.txtSystemType)
        Me.groupBox2.Controls.Add(Me.label8)
        Me.groupBox2.Controls.Add(Me.label13)
        Me.groupBox2.Controls.Add(Me.txtServerKeepAlive)
        Me.groupBox2.Controls.Add(Me.label12)
        Me.groupBox2.Controls.Add(Me.label11)
        Me.groupBox2.Controls.Add(Me.label10)
        Me.groupBox2.Controls.Add(Me.label6)
        Me.groupBox2.Controls.Add(Me.txtSleepAfterSocketFailure)
        Me.groupBox2.Controls.Add(Me.cboInterfaceVersion)
        Me.groupBox2.Location = New System.Drawing.Point(8, 111)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(648, 205)
        Me.groupBox2.TabIndex = 27
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Advanced Settings"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(8, 23)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(90, 13)
        Me.label6.TabIndex = 30
        Me.label6.Text = "Interface Version"
        '
        'txtSleepAfterSocketFailure
        '
        Me.txtSleepAfterSocketFailure.Location = New System.Drawing.Point(144, 45)
        Me.txtSleepAfterSocketFailure.Name = "txtSleepAfterSocketFailure"
        Me.txtSleepAfterSocketFailure.Size = New System.Drawing.Size(75, 20)
        Me.txtSleepAfterSocketFailure.TabIndex = 29
        Me.txtSleepAfterSocketFailure.Text = "10"
        '
        'cboInterfaceVersion
        '
        Me.cboInterfaceVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboInterfaceVersion.FormattingEnabled = True
        Me.cboInterfaceVersion.Location = New System.Drawing.Point(144, 19)
        Me.cboInterfaceVersion.Name = "cboInterfaceVersion"
        Me.cboInterfaceVersion.Size = New System.Drawing.Size(148, 21)
        Me.cboInterfaceVersion.TabIndex = 23
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.txtPassword)
        Me.groupBox1.Controls.Add(Me.label4)
        Me.groupBox1.Controls.Add(Me.label3)
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.txtSystemId)
        Me.groupBox1.Controls.Add(Me.txtPort)
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Controls.Add(Me.label26)
        Me.groupBox1.Controls.Add(Me.txtServer)
        Me.groupBox1.Controls.Add(Me.cboSystemMode)
        Me.groupBox1.Location = New System.Drawing.Point(7, 6)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(648, 99)
        Me.groupBox1.TabIndex = 26
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Connection"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(319, 44)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(71, 13)
        Me.label3.TabIndex = 30
        Me.label3.Text = "System Mode"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(6, 48)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(56, 13)
        Me.label2.TabIndex = 28
        Me.label2.Text = "System ID"
        '
        'txtSystemId
        '
        Me.txtSystemId.Location = New System.Drawing.Point(82, 42)
        Me.txtSystemId.Name = "txtSystemId"
        Me.txtSystemId.Size = New System.Drawing.Size(204, 20)
        Me.txtSystemId.TabIndex = 29
        Me.txtSystemId.Text = "smppclient1"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(423, 19)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(148, 20)
        Me.txtPort.TabIndex = 27
        Me.txtPort.Text = "2775"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(320, 22)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(27, 13)
        Me.label1.TabIndex = 26
        Me.label1.Text = "Port"
        '
        'label26
        '
        Me.label26.AutoSize = True
        Me.label26.Location = New System.Drawing.Point(6, 22)
        Me.label26.Name = "label26"
        Me.label26.Size = New System.Drawing.Size(39, 13)
        Me.label26.TabIndex = 24
        Me.label26.Text = "Server"
        '
        'cboSystemMode
        '
        Me.cboSystemMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSystemMode.FormattingEnabled = True
        Me.cboSystemMode.Location = New System.Drawing.Point(423, 42)
        Me.cboSystemMode.Name = "cboSystemMode"
        Me.cboSystemMode.Size = New System.Drawing.Size(148, 21)
        Me.cboSystemMode.TabIndex = 23
        '
        'tabSendReceiveSms
        '
        Me.tabSendReceiveSms.Controls.Add(Me.groupBox7)
        Me.tabSendReceiveSms.Controls.Add(Me.groupBox6)
        Me.tabSendReceiveSms.Controls.Add(Me.groupBox5)
        Me.tabSendReceiveSms.Controls.Add(Me.groupBox4)
        Me.tabSendReceiveSms.Location = New System.Drawing.Point(4, 22)
        Me.tabSendReceiveSms.Name = "tabSendReceiveSms"
        Me.tabSendReceiveSms.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSendReceiveSms.Size = New System.Drawing.Size(674, 429)
        Me.tabSendReceiveSms.TabIndex = 1
        Me.tabSendReceiveSms.Text = "Send & Receive SMS"
        Me.tabSendReceiveSms.UseVisualStyleBackColor = True
        '
        'groupBox7
        '
        Me.groupBox7.Controls.Add(Me.chkDeliveryReport)
        Me.groupBox7.Location = New System.Drawing.Point(481, 108)
        Me.groupBox7.Name = "groupBox7"
        Me.groupBox7.Size = New System.Drawing.Size(183, 177)
        Me.groupBox7.TabIndex = 61
        Me.groupBox7.TabStop = False
        Me.groupBox7.Text = "SMS Send Options"
        '
        'chkDeliveryReport
        '
        Me.chkDeliveryReport.Location = New System.Drawing.Point(6, 22)
        Me.chkDeliveryReport.Name = "chkDeliveryReport"
        Me.chkDeliveryReport.Size = New System.Drawing.Size(151, 24)
        Me.chkDeliveryReport.TabIndex = 3
        Me.chkDeliveryReport.Text = "&Request delivery report"
        '
        'frmSMPP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(696, 469)
        Me.Controls.Add(Me.tabMain)
        Me.Name = "frmSMPP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmSMPP"
        Me.groupBox4.ResumeLayout(False)
        Me.groupBox4.PerformLayout()
        Me.groupBox5.ResumeLayout(False)
        Me.groupBox5.PerformLayout()
        Me.tabAbout.ResumeLayout(False)
        Me.groupBox8.ResumeLayout(False)
        Me.groupBox8.PerformLayout()
        Me.groupBox6.ResumeLayout(False)
        Me.groupBox6.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.tabSettings.ResumeLayout(False)
        Me.groupBox44.ResumeLayout(False)
        Me.groupBox44.PerformLayout()
        Me.groupBox3.ResumeLayout(False)
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.tabSendReceiveSms.ResumeLayout(False)
        Me.groupBox7.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents txtServer As System.Windows.Forms.TextBox
    Private WithEvents label9 As System.Windows.Forms.Label
    Friend WithEvents chkPayload As System.Windows.Forms.CheckBox
    Private WithEvents btnSendSms As System.Windows.Forms.Button
    Private WithEvents cboDataCoding As System.Windows.Forms.ComboBox
    Private WithEvents label21 As System.Windows.Forms.Label
    Private WithEvents txtMessage As System.Windows.Forms.TextBox
    Private WithEvents groupBox4 As System.Windows.Forms.GroupBox
    Private WithEvents txtSourceAddress As System.Windows.Forms.TextBox
    Private WithEvents label20 As System.Windows.Forms.Label
    Private WithEvents label17 As System.Windows.Forms.Label
    Private WithEvents cboDestinationNpi As System.Windows.Forms.ComboBox
    Private WithEvents cboSourceTon As System.Windows.Forms.ComboBox
    Private WithEvents label19 As System.Windows.Forms.Label
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents cboDestinationTon As System.Windows.Forms.ComboBox
    Private WithEvents cboSourceNpi As System.Windows.Forms.ComboBox
    Private WithEvents label18 As System.Windows.Forms.Label
    Private WithEvents txtRecipients As System.Windows.Forms.TextBox
    Private WithEvents groupBox5 As System.Windows.Forms.GroupBox
    Private WithEvents label15 As System.Windows.Forms.Label
    Private WithEvents label7 As System.Windows.Forms.Label
    Private WithEvents tabAbout As System.Windows.Forms.TabPage
    Private WithEvents groupBox8 As System.Windows.Forms.GroupBox
    Private WithEvents lblLicense As System.Windows.Forms.Label
    Private WithEvents lblAbout As System.Windows.Forms.Label
    Private WithEvents linkLabel1 As System.Windows.Forms.LinkLabel
    Private WithEvents cboNpi As System.Windows.Forms.ComboBox
    Private WithEvents label14 As System.Windows.Forms.Label
    Private WithEvents cboTon As System.Windows.Forms.ComboBox
    Private WithEvents label16 As System.Windows.Forms.Label
    Private WithEvents txtAddressRange As System.Windows.Forms.TextBox
    Private WithEvents txtSystemType As System.Windows.Forms.TextBox
    Private WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents label13 As System.Windows.Forms.Label
    Private WithEvents txtReceivedMessage As System.Windows.Forms.TextBox
    Private WithEvents txtServerKeepAlive As System.Windows.Forms.TextBox
    Private WithEvents label12 As System.Windows.Forms.Label
    Private WithEvents label11 As System.Windows.Forms.Label
    Private WithEvents label10 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents groupBox6 As System.Windows.Forms.GroupBox
    Private WithEvents txtPassword As System.Windows.Forms.TextBox
    Private WithEvents tabMain As System.Windows.Forms.TabControl
    Private WithEvents tabSettings As System.Windows.Forms.TabPage
    Private WithEvents groupBox44 As System.Windows.Forms.GroupBox
    Private WithEvents txtLogFile As System.Windows.Forms.TextBox
    Private WithEvents groupBox3 As System.Windows.Forms.GroupBox
    Private WithEvents btnDisconnect As System.Windows.Forms.Button
    Private WithEvents btnConnect As System.Windows.Forms.Button
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents txtSleepAfterSocketFailure As System.Windows.Forms.TextBox
    Private WithEvents cboInterfaceVersion As System.Windows.Forms.ComboBox
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents txtSystemId As System.Windows.Forms.TextBox
    Private WithEvents txtPort As System.Windows.Forms.TextBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents label26 As System.Windows.Forms.Label
    Private WithEvents cboSystemMode As System.Windows.Forms.ComboBox
    Private WithEvents tabSendReceiveSms As System.Windows.Forms.TabPage
    Friend WithEvents groupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents chkDeliveryReport As System.Windows.Forms.CheckBox
End Class
