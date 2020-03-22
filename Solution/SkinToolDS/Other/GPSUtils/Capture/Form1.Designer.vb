<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TabControlCapture = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblLastLine = New System.Windows.Forms.Label()
        Me.btnSerialConnect = New System.Windows.Forms.Button()
        Me.lblSerialStatus = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.boxDataBits = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.boxSpeed = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.boxSerialPort = New System.Windows.Forms.ComboBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.lblNTRIPConnectedAt = New System.Windows.Forms.Label()
        Me.pbNTRIP = New System.Windows.Forms.ProgressBar()
        Me.boxMountpoint = New System.Windows.Forms.ComboBox()
        Me.tbNTRIPPassword = New System.Windows.Forms.TextBox()
        Me.tbNTRIPUsername = New System.Windows.Forms.TextBox()
        Me.tbNTRIPCasterPort = New System.Windows.Forms.TextBox()
        Me.tbNTRIPCasterIP = New System.Windows.Forms.TextBox()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.lblNTRIPStatus = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.btnNTRIPConnect = New System.Windows.Forms.Button()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.TabNMEADataStream = New System.Windows.Forms.TabPage()
        Me.rtbDataStream = New System.Windows.Forms.RichTextBox()
        Me.TabNMEAEvents = New System.Windows.Forms.TabPage()
        Me.btnEventsToFile = New System.Windows.Forms.Button()
        Me.rtbEvents = New System.Windows.Forms.RichTextBox()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.lblStatPDOP = New System.Windows.Forms.Label()
        Me.lblStatVDOP = New System.Windows.Forms.Label()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.lblStatCorrStationID = New System.Windows.Forms.Label()
        Me.lblStatCorrAge = New System.Windows.Forms.Label()
        Me.lblStatHDOP = New System.Windows.Forms.Label()
        Me.lblStatSatCount = New System.Windows.Forms.Label()
        Me.lblStatGPSType = New System.Windows.Forms.Label()
        Me.lblStatAltitude = New System.Windows.Forms.Label()
        Me.lblStatSpeed = New System.Windows.Forms.Label()
        Me.lblStatHeading = New System.Windows.Forms.Label()
        Me.lblStatLongitude = New System.Windows.Forms.Label()
        Me.lblStatLatitude = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabSatMap = New System.Windows.Forms.TabPage()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.lblPrnSignal = New System.Windows.Forms.Label()
        Me.btnSatMapRefresh = New System.Windows.Forms.Button()
        Me.TabCapt = New System.Windows.Forms.TabPage()
        Me.lblEndTime = New System.Windows.Forms.Label()
        Me.lblStartTime = New System.Windows.Forms.Label()
        Me.lblCurrentFile = New System.Windows.Forms.Label()
        Me.boxDataToLog = New System.Windows.Forms.ComboBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.boxUponCompletion = New System.Windows.Forms.ComboBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.boxLogForMinutes = New System.Windows.Forms.ComboBox()
        Me.btnStopRecording = New System.Windows.Forms.Button()
        Me.btnStartRecording = New System.Windows.Forms.Button()
        Me.tbSessionLabel = New System.Windows.Forms.TextBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblStatusBar = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TabControlCapture.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabNMEADataStream.SuspendLayout()
        Me.TabNMEAEvents.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.TabSatMap.SuspendLayout()
        Me.TabCapt.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControlCapture
        '
        Me.TabControlCapture.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControlCapture.Controls.Add(Me.TabPage1)
        Me.TabControlCapture.Controls.Add(Me.TabPage2)
        Me.TabControlCapture.Controls.Add(Me.TabNMEADataStream)
        Me.TabControlCapture.Controls.Add(Me.TabNMEAEvents)
        Me.TabControlCapture.Controls.Add(Me.TabPage7)
        Me.TabControlCapture.Controls.Add(Me.TabSatMap)
        Me.TabControlCapture.Controls.Add(Me.TabCapt)
        Me.TabControlCapture.Location = New System.Drawing.Point(12, 12)
        Me.TabControlCapture.Name = "TabControlCapture"
        Me.TabControlCapture.SelectedIndex = 0
        Me.TabControlCapture.Size = New System.Drawing.Size(749, 471)
        Me.TabControlCapture.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Window
        Me.TabPage1.Controls.Add(Me.lblLastLine)
        Me.TabPage1.Controls.Add(Me.btnSerialConnect)
        Me.TabPage1.Controls.Add(Me.lblSerialStatus)
        Me.TabPage1.Controls.Add(Me.Label19)
        Me.TabPage1.Controls.Add(Me.Label11)
        Me.TabPage1.Controls.Add(Me.Label12)
        Me.TabPage1.Controls.Add(Me.Label13)
        Me.TabPage1.Controls.Add(Me.Label14)
        Me.TabPage1.Controls.Add(Me.Label15)
        Me.TabPage1.Controls.Add(Me.boxDataBits)
        Me.TabPage1.Controls.Add(Me.Label16)
        Me.TabPage1.Controls.Add(Me.boxSpeed)
        Me.TabPage1.Controls.Add(Me.Label17)
        Me.TabPage1.Controls.Add(Me.Label18)
        Me.TabPage1.Controls.Add(Me.boxSerialPort)
        Me.TabPage1.Controls.Add(Me.Label32)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(741, 445)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Serial Port"
        '
        'lblLastLine
        '
        Me.lblLastLine.AutoSize = True
        Me.lblLastLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastLine.Location = New System.Drawing.Point(16, 306)
        Me.lblLastLine.Name = "lblLastLine"
        Me.lblLastLine.Size = New System.Drawing.Size(74, 20)
        Me.lblLastLine.TabIndex = 64
        Me.lblLastLine.Text = "Last Line"
        Me.lblLastLine.Visible = False
        '
        'btnSerialConnect
        '
        Me.btnSerialConnect.BackColor = System.Drawing.Color.LightGray
        Me.btnSerialConnect.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSerialConnect.Location = New System.Drawing.Point(112, 222)
        Me.btnSerialConnect.Name = "btnSerialConnect"
        Me.btnSerialConnect.Size = New System.Drawing.Size(136, 33)
        Me.btnSerialConnect.TabIndex = 63
        Me.btnSerialConnect.Text = "Connect"
        Me.btnSerialConnect.UseVisualStyleBackColor = False
        '
        'lblSerialStatus
        '
        Me.lblSerialStatus.AutoSize = True
        Me.lblSerialStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSerialStatus.ForeColor = System.Drawing.Color.Black
        Me.lblSerialStatus.Location = New System.Drawing.Point(114, 269)
        Me.lblSerialStatus.Name = "lblSerialStatus"
        Me.lblSerialStatus.Size = New System.Drawing.Size(126, 24)
        Me.lblSerialStatus.TabIndex = 62
        Me.lblSerialStatus.Text = "Disconnected"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.Black
        Me.Label19.Location = New System.Drawing.Point(16, 269)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(100, 24)
        Me.Label19.TabIndex = 61
        Me.Label19.Text = "Serial Port:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(20, 49)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(86, 20)
        Me.Label11.TabIndex = 49
        Me.Label11.Text = "Serial Port:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(112, 185)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(18, 20)
        Me.Label12.TabIndex = 59
        Me.Label12.Text = "1"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(16, 83)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(90, 20)
        Me.Label13.TabIndex = 50
        Me.Label13.Text = "Baud Rate:"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(112, 151)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(47, 20)
        Me.Label14.TabIndex = 58
        Me.Label14.Text = "None"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(27, 117)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(79, 20)
        Me.Label15.TabIndex = 51
        Me.Label15.Text = "Data Bits:"
        '
        'boxDataBits
        '
        Me.boxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxDataBits.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxDataBits.FormattingEnabled = True
        Me.boxDataBits.Items.AddRange(New Object() {"7", "8"})
        Me.boxDataBits.Location = New System.Drawing.Point(112, 114)
        Me.boxDataBits.Name = "boxDataBits"
        Me.boxDataBits.Size = New System.Drawing.Size(77, 28)
        Me.boxDataBits.TabIndex = 57
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(54, 151)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(52, 20)
        Me.Label16.TabIndex = 52
        Me.Label16.Text = "Parity:"
        '
        'boxSpeed
        '
        Me.boxSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxSpeed.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxSpeed.FormattingEnabled = True
        Me.boxSpeed.Items.AddRange(New Object() {"2400", "4800", "9600", "14400", "19200", "38400", "57600", "115200"})
        Me.boxSpeed.Location = New System.Drawing.Point(112, 80)
        Me.boxSpeed.Name = "boxSpeed"
        Me.boxSpeed.Size = New System.Drawing.Size(150, 28)
        Me.boxSpeed.TabIndex = 56
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(28, 185)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(78, 20)
        Me.Label17.TabIndex = 53
        Me.Label17.Text = "Stop Bits:"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(268, 83)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(103, 20)
        Me.Label18.TabIndex = 55
        Me.Label18.Text = "bytes/second"
        '
        'boxSerialPort
        '
        Me.boxSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxSerialPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxSerialPort.FormattingEnabled = True
        Me.boxSerialPort.Location = New System.Drawing.Point(112, 46)
        Me.boxSerialPort.Name = "boxSerialPort"
        Me.boxSerialPort.Size = New System.Drawing.Size(303, 28)
        Me.boxSerialPort.TabIndex = 54
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(16, 12)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(306, 20)
        Me.Label32.TabIndex = 48
        Me.Label32.Text = "GPS Receiver Serial Port Connection"
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.Window
        Me.TabPage2.Controls.Add(Me.lblNTRIPConnectedAt)
        Me.TabPage2.Controls.Add(Me.pbNTRIP)
        Me.TabPage2.Controls.Add(Me.boxMountpoint)
        Me.TabPage2.Controls.Add(Me.tbNTRIPPassword)
        Me.TabPage2.Controls.Add(Me.tbNTRIPUsername)
        Me.TabPage2.Controls.Add(Me.tbNTRIPCasterPort)
        Me.TabPage2.Controls.Add(Me.tbNTRIPCasterIP)
        Me.TabPage2.Controls.Add(Me.Label43)
        Me.TabPage2.Controls.Add(Me.Label42)
        Me.TabPage2.Controls.Add(Me.Label37)
        Me.TabPage2.Controls.Add(Me.lblNTRIPStatus)
        Me.TabPage2.Controls.Add(Me.Label35)
        Me.TabPage2.Controls.Add(Me.Label34)
        Me.TabPage2.Controls.Add(Me.btnNTRIPConnect)
        Me.TabPage2.Controls.Add(Me.Label33)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(741, 445)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "NTRIP Client"
        '
        'lblNTRIPConnectedAt
        '
        Me.lblNTRIPConnectedAt.AutoSize = True
        Me.lblNTRIPConnectedAt.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNTRIPConnectedAt.Location = New System.Drawing.Point(77, 280)
        Me.lblNTRIPConnectedAt.Name = "lblNTRIPConnectedAt"
        Me.lblNTRIPConnectedAt.Size = New System.Drawing.Size(97, 17)
        Me.lblNTRIPConnectedAt.TabIndex = 56
        Me.lblNTRIPConnectedAt.Text = "Connected At:"
        Me.lblNTRIPConnectedAt.Visible = False
        '
        'pbNTRIP
        '
        Me.pbNTRIP.Location = New System.Drawing.Point(126, 244)
        Me.pbNTRIP.Name = "pbNTRIP"
        Me.pbNTRIP.Size = New System.Drawing.Size(235, 23)
        Me.pbNTRIP.TabIndex = 55
        Me.pbNTRIP.Visible = False
        '
        'boxMountpoint
        '
        Me.boxMountpoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxMountpoint.FormattingEnabled = True
        Me.boxMountpoint.Location = New System.Drawing.Point(126, 144)
        Me.boxMountpoint.Name = "boxMountpoint"
        Me.boxMountpoint.Size = New System.Drawing.Size(235, 21)
        Me.boxMountpoint.TabIndex = 54
        '
        'tbNTRIPPassword
        '
        Me.tbNTRIPPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNTRIPPassword.Location = New System.Drawing.Point(126, 112)
        Me.tbNTRIPPassword.Name = "tbNTRIPPassword"
        Me.tbNTRIPPassword.Size = New System.Drawing.Size(235, 23)
        Me.tbNTRIPPassword.TabIndex = 53
        '
        'tbNTRIPUsername
        '
        Me.tbNTRIPUsername.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNTRIPUsername.Location = New System.Drawing.Point(126, 82)
        Me.tbNTRIPUsername.Name = "tbNTRIPUsername"
        Me.tbNTRIPUsername.Size = New System.Drawing.Size(235, 23)
        Me.tbNTRIPUsername.TabIndex = 52
        '
        'tbNTRIPCasterPort
        '
        Me.tbNTRIPCasterPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNTRIPCasterPort.Location = New System.Drawing.Point(282, 52)
        Me.tbNTRIPCasterPort.Name = "tbNTRIPCasterPort"
        Me.tbNTRIPCasterPort.Size = New System.Drawing.Size(79, 23)
        Me.tbNTRIPCasterPort.TabIndex = 51
        '
        'tbNTRIPCasterIP
        '
        Me.tbNTRIPCasterIP.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNTRIPCasterIP.Location = New System.Drawing.Point(126, 52)
        Me.tbNTRIPCasterIP.Name = "tbNTRIPCasterIP"
        Me.tbNTRIPCasterIP.Size = New System.Drawing.Size(97, 23)
        Me.tbNTRIPCasterIP.TabIndex = 49
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(247, 55)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(38, 17)
        Me.Label43.TabIndex = 50
        Me.Label43.Text = "Port:"
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(56, 115)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(73, 17)
        Me.Label42.TabIndex = 48
        Me.Label42.Text = "Password:"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(52, 85)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(77, 17)
        Me.Label37.TabIndex = 47
        Me.Label37.Text = "Username:"
        '
        'lblNTRIPStatus
        '
        Me.lblNTRIPStatus.AutoSize = True
        Me.lblNTRIPStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNTRIPStatus.Location = New System.Drawing.Point(77, 212)
        Me.lblNTRIPStatus.Name = "lblNTRIPStatus"
        Me.lblNTRIPStatus.Size = New System.Drawing.Size(150, 17)
        Me.lblNTRIPStatus.TabIndex = 46
        Me.lblNTRIPStatus.Text = "Status: Not Connected"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(42, 145)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(87, 17)
        Me.Label35.TabIndex = 45
        Me.Label35.Text = "Mount Point:"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.Location = New System.Drawing.Point(31, 55)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(98, 17)
        Me.Label34.TabIndex = 44
        Me.Label34.Text = "NTRIP Caster:"
        '
        'btnNTRIPConnect
        '
        Me.btnNTRIPConnect.Location = New System.Drawing.Point(286, 171)
        Me.btnNTRIPConnect.Name = "btnNTRIPConnect"
        Me.btnNTRIPConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnNTRIPConnect.TabIndex = 42
        Me.btnNTRIPConnect.Text = "Connect"
        Me.btnNTRIPConnect.UseVisualStyleBackColor = True
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(6, 16)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(241, 20)
        Me.Label33.TabIndex = 41
        Me.Label33.Text = "NTRIP Connection (Optional)"
        '
        'TabNMEADataStream
        '
        Me.TabNMEADataStream.BackColor = System.Drawing.SystemColors.Window
        Me.TabNMEADataStream.Controls.Add(Me.rtbDataStream)
        Me.TabNMEADataStream.Location = New System.Drawing.Point(4, 22)
        Me.TabNMEADataStream.Name = "TabNMEADataStream"
        Me.TabNMEADataStream.Padding = New System.Windows.Forms.Padding(3)
        Me.TabNMEADataStream.Size = New System.Drawing.Size(741, 445)
        Me.TabNMEADataStream.TabIndex = 2
        Me.TabNMEADataStream.Text = "Data Stream"
        '
        'rtbDataStream
        '
        Me.rtbDataStream.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbDataStream.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbDataStream.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbDataStream.Location = New System.Drawing.Point(0, 0)
        Me.rtbDataStream.Name = "rtbDataStream"
        Me.rtbDataStream.Size = New System.Drawing.Size(741, 445)
        Me.rtbDataStream.TabIndex = 4
        Me.rtbDataStream.Text = "NMEA Data Stream will show up here."
        '
        'TabNMEAEvents
        '
        Me.TabNMEAEvents.BackColor = System.Drawing.SystemColors.Window
        Me.TabNMEAEvents.Controls.Add(Me.btnEventsToFile)
        Me.TabNMEAEvents.Controls.Add(Me.rtbEvents)
        Me.TabNMEAEvents.Location = New System.Drawing.Point(4, 22)
        Me.TabNMEAEvents.Name = "TabNMEAEvents"
        Me.TabNMEAEvents.Padding = New System.Windows.Forms.Padding(3)
        Me.TabNMEAEvents.Size = New System.Drawing.Size(741, 445)
        Me.TabNMEAEvents.TabIndex = 3
        Me.TabNMEAEvents.Text = "Events"
        '
        'btnEventsToFile
        '
        Me.btnEventsToFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEventsToFile.Location = New System.Drawing.Point(618, 3)
        Me.btnEventsToFile.Name = "btnEventsToFile"
        Me.btnEventsToFile.Size = New System.Drawing.Size(120, 23)
        Me.btnEventsToFile.TabIndex = 43
        Me.btnEventsToFile.Text = "Save Events to File"
        Me.btnEventsToFile.UseVisualStyleBackColor = True
        '
        'rtbEvents
        '
        Me.rtbEvents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbEvents.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbEvents.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbEvents.Location = New System.Drawing.Point(0, 0)
        Me.rtbEvents.Name = "rtbEvents"
        Me.rtbEvents.Size = New System.Drawing.Size(741, 445)
        Me.rtbEvents.TabIndex = 3
        Me.rtbEvents.Text = "NMEA Events will show up here."
        '
        'TabPage7
        '
        Me.TabPage7.BackColor = System.Drawing.SystemColors.Window
        Me.TabPage7.Controls.Add(Me.lblStatPDOP)
        Me.TabPage7.Controls.Add(Me.lblStatVDOP)
        Me.TabPage7.Controls.Add(Me.Label44)
        Me.TabPage7.Controls.Add(Me.Label36)
        Me.TabPage7.Controls.Add(Me.lblStatCorrStationID)
        Me.TabPage7.Controls.Add(Me.lblStatCorrAge)
        Me.TabPage7.Controls.Add(Me.lblStatHDOP)
        Me.TabPage7.Controls.Add(Me.lblStatSatCount)
        Me.TabPage7.Controls.Add(Me.lblStatGPSType)
        Me.TabPage7.Controls.Add(Me.lblStatAltitude)
        Me.TabPage7.Controls.Add(Me.lblStatSpeed)
        Me.TabPage7.Controls.Add(Me.lblStatHeading)
        Me.TabPage7.Controls.Add(Me.lblStatLongitude)
        Me.TabPage7.Controls.Add(Me.lblStatLatitude)
        Me.TabPage7.Controls.Add(Me.Label10)
        Me.TabPage7.Controls.Add(Me.Label9)
        Me.TabPage7.Controls.Add(Me.Label8)
        Me.TabPage7.Controls.Add(Me.Label7)
        Me.TabPage7.Controls.Add(Me.Label6)
        Me.TabPage7.Controls.Add(Me.Label5)
        Me.TabPage7.Controls.Add(Me.Label4)
        Me.TabPage7.Controls.Add(Me.Label3)
        Me.TabPage7.Controls.Add(Me.Label2)
        Me.TabPage7.Controls.Add(Me.Label1)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage7.Size = New System.Drawing.Size(741, 445)
        Me.TabPage7.TabIndex = 4
        Me.TabPage7.Text = "Information"
        '
        'lblStatPDOP
        '
        Me.lblStatPDOP.AutoSize = True
        Me.lblStatPDOP.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatPDOP.Location = New System.Drawing.Point(183, 280)
        Me.lblStatPDOP.Name = "lblStatPDOP"
        Me.lblStatPDOP.Size = New System.Drawing.Size(13, 17)
        Me.lblStatPDOP.TabIndex = 43
        Me.lblStatPDOP.Text = "-"
        '
        'lblStatVDOP
        '
        Me.lblStatVDOP.AutoSize = True
        Me.lblStatVDOP.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatVDOP.Location = New System.Drawing.Point(183, 255)
        Me.lblStatVDOP.Name = "lblStatVDOP"
        Me.lblStatVDOP.Size = New System.Drawing.Size(13, 17)
        Me.lblStatVDOP.TabIndex = 42
        Me.lblStatVDOP.Text = "-"
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(130, 280)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(47, 17)
        Me.Label44.TabIndex = 41
        Me.Label44.Text = "PDOP"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.Location = New System.Drawing.Point(130, 255)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(47, 17)
        Me.Label36.TabIndex = 40
        Me.Label36.Text = "VDOP"
        '
        'lblStatCorrStationID
        '
        Me.lblStatCorrStationID.AutoSize = True
        Me.lblStatCorrStationID.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatCorrStationID.Location = New System.Drawing.Point(183, 350)
        Me.lblStatCorrStationID.Name = "lblStatCorrStationID"
        Me.lblStatCorrStationID.Size = New System.Drawing.Size(13, 17)
        Me.lblStatCorrStationID.TabIndex = 39
        Me.lblStatCorrStationID.Text = "-"
        '
        'lblStatCorrAge
        '
        Me.lblStatCorrAge.AutoSize = True
        Me.lblStatCorrAge.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatCorrAge.Location = New System.Drawing.Point(183, 325)
        Me.lblStatCorrAge.Name = "lblStatCorrAge"
        Me.lblStatCorrAge.Size = New System.Drawing.Size(13, 17)
        Me.lblStatCorrAge.TabIndex = 38
        Me.lblStatCorrAge.Text = "-"
        '
        'lblStatHDOP
        '
        Me.lblStatHDOP.AutoSize = True
        Me.lblStatHDOP.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatHDOP.Location = New System.Drawing.Point(183, 230)
        Me.lblStatHDOP.Name = "lblStatHDOP"
        Me.lblStatHDOP.Size = New System.Drawing.Size(13, 17)
        Me.lblStatHDOP.TabIndex = 37
        Me.lblStatHDOP.Text = "-"
        '
        'lblStatSatCount
        '
        Me.lblStatSatCount.AutoSize = True
        Me.lblStatSatCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatSatCount.Location = New System.Drawing.Point(183, 205)
        Me.lblStatSatCount.Name = "lblStatSatCount"
        Me.lblStatSatCount.Size = New System.Drawing.Size(13, 17)
        Me.lblStatSatCount.TabIndex = 36
        Me.lblStatSatCount.Text = "-"
        '
        'lblStatGPSType
        '
        Me.lblStatGPSType.AutoSize = True
        Me.lblStatGPSType.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatGPSType.Location = New System.Drawing.Point(183, 180)
        Me.lblStatGPSType.Name = "lblStatGPSType"
        Me.lblStatGPSType.Size = New System.Drawing.Size(13, 17)
        Me.lblStatGPSType.TabIndex = 35
        Me.lblStatGPSType.Text = "-"
        '
        'lblStatAltitude
        '
        Me.lblStatAltitude.AutoSize = True
        Me.lblStatAltitude.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatAltitude.Location = New System.Drawing.Point(183, 135)
        Me.lblStatAltitude.Name = "lblStatAltitude"
        Me.lblStatAltitude.Size = New System.Drawing.Size(13, 17)
        Me.lblStatAltitude.TabIndex = 34
        Me.lblStatAltitude.Text = "-"
        '
        'lblStatSpeed
        '
        Me.lblStatSpeed.AutoSize = True
        Me.lblStatSpeed.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatSpeed.Location = New System.Drawing.Point(183, 110)
        Me.lblStatSpeed.Name = "lblStatSpeed"
        Me.lblStatSpeed.Size = New System.Drawing.Size(13, 17)
        Me.lblStatSpeed.TabIndex = 33
        Me.lblStatSpeed.Text = "-"
        '
        'lblStatHeading
        '
        Me.lblStatHeading.AutoSize = True
        Me.lblStatHeading.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatHeading.Location = New System.Drawing.Point(183, 85)
        Me.lblStatHeading.Name = "lblStatHeading"
        Me.lblStatHeading.Size = New System.Drawing.Size(13, 17)
        Me.lblStatHeading.TabIndex = 32
        Me.lblStatHeading.Text = "-"
        '
        'lblStatLongitude
        '
        Me.lblStatLongitude.AutoSize = True
        Me.lblStatLongitude.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatLongitude.Location = New System.Drawing.Point(183, 40)
        Me.lblStatLongitude.Name = "lblStatLongitude"
        Me.lblStatLongitude.Size = New System.Drawing.Size(13, 17)
        Me.lblStatLongitude.TabIndex = 31
        Me.lblStatLongitude.Text = "-"
        '
        'lblStatLatitude
        '
        Me.lblStatLatitude.AutoSize = True
        Me.lblStatLatitude.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatLatitude.Location = New System.Drawing.Point(183, 15)
        Me.lblStatLatitude.Name = "lblStatLatitude"
        Me.lblStatLatitude.Size = New System.Drawing.Size(13, 17)
        Me.lblStatLatitude.TabIndex = 30
        Me.lblStatLatitude.Text = "-"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(35, 350)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(142, 17)
        Me.Label10.TabIndex = 29
        Me.Label10.Text = "Correction Station ID:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(71, 325)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(106, 17)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "Correction Age:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(129, 230)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(48, 17)
        Me.Label8.TabIndex = 27
        Me.Label8.Text = "HDOP"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(52, 205)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(125, 17)
        Me.Label7.TabIndex = 26
        Me.Label7.Text = "Satellites Tracked:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(100, 180)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(77, 17)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "GPS Type:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(118, 135)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 17)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "Altitude:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(124, 110)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 17)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Speed:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(112, 85)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 17)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "Heading:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(102, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 17)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Longitude:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(114, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 17)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Latitude:"
        '
        'TabSatMap
        '
        Me.TabSatMap.BackColor = System.Drawing.SystemColors.Window
        Me.TabSatMap.Controls.Add(Me.Label31)
        Me.TabSatMap.Controls.Add(Me.Label30)
        Me.TabSatMap.Controls.Add(Me.lblPrnSignal)
        Me.TabSatMap.Controls.Add(Me.btnSatMapRefresh)
        Me.TabSatMap.Location = New System.Drawing.Point(4, 22)
        Me.TabSatMap.Name = "TabSatMap"
        Me.TabSatMap.Padding = New System.Windows.Forms.Padding(3)
        Me.TabSatMap.Size = New System.Drawing.Size(741, 445)
        Me.TabSatMap.TabIndex = 5
        Me.TabSatMap.Text = "Satellite Map"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(395, 29)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(132, 13)
        Me.Label31.TabIndex = 9
        Me.Label31.Text = "Outer Circle = The Horizon"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(395, 13)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(142, 13)
        Me.Label30.TabIndex = 8
        Me.Label30.Text = "Center = Straight Above You"
        '
        'lblPrnSignal
        '
        Me.lblPrnSignal.AutoSize = True
        Me.lblPrnSignal.Location = New System.Drawing.Point(445, 52)
        Me.lblPrnSignal.Name = "lblPrnSignal"
        Me.lblPrnSignal.Size = New System.Drawing.Size(65, 13)
        Me.lblPrnSignal.TabIndex = 7
        Me.lblPrnSignal.Text = "PRN: Signal"
        '
        'btnSatMapRefresh
        '
        Me.btnSatMapRefresh.Location = New System.Drawing.Point(3, 3)
        Me.btnSatMapRefresh.Name = "btnSatMapRefresh"
        Me.btnSatMapRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnSatMapRefresh.TabIndex = 6
        Me.btnSatMapRefresh.Text = "Refresh"
        Me.btnSatMapRefresh.UseVisualStyleBackColor = True
        '
        'TabCapt
        '
        Me.TabCapt.BackColor = System.Drawing.SystemColors.Window
        Me.TabCapt.Controls.Add(Me.lblEndTime)
        Me.TabCapt.Controls.Add(Me.lblStartTime)
        Me.TabCapt.Controls.Add(Me.lblCurrentFile)
        Me.TabCapt.Controls.Add(Me.boxDataToLog)
        Me.TabCapt.Controls.Add(Me.Label24)
        Me.TabCapt.Controls.Add(Me.boxUponCompletion)
        Me.TabCapt.Controls.Add(Me.Label20)
        Me.TabCapt.Controls.Add(Me.Label23)
        Me.TabCapt.Controls.Add(Me.Label22)
        Me.TabCapt.Controls.Add(Me.Label21)
        Me.TabCapt.Controls.Add(Me.Label29)
        Me.TabCapt.Controls.Add(Me.boxLogForMinutes)
        Me.TabCapt.Controls.Add(Me.btnStopRecording)
        Me.TabCapt.Controls.Add(Me.btnStartRecording)
        Me.TabCapt.Controls.Add(Me.tbSessionLabel)
        Me.TabCapt.Location = New System.Drawing.Point(4, 22)
        Me.TabCapt.Name = "TabCapt"
        Me.TabCapt.Padding = New System.Windows.Forms.Padding(3)
        Me.TabCapt.Size = New System.Drawing.Size(741, 445)
        Me.TabCapt.TabIndex = 6
        Me.TabCapt.Text = "Capture"
        '
        'lblEndTime
        '
        Me.lblEndTime.AutoSize = True
        Me.lblEndTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndTime.Location = New System.Drawing.Point(75, 345)
        Me.lblEndTime.Name = "lblEndTime"
        Me.lblEndTime.Size = New System.Drawing.Size(72, 17)
        Me.lblEndTime.TabIndex = 72
        Me.lblEndTime.Text = "End Time:"
        Me.lblEndTime.Visible = False
        '
        'lblStartTime
        '
        Me.lblStartTime.AutoSize = True
        Me.lblStartTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartTime.Location = New System.Drawing.Point(70, 315)
        Me.lblStartTime.Name = "lblStartTime"
        Me.lblStartTime.Size = New System.Drawing.Size(77, 17)
        Me.lblStartTime.TabIndex = 71
        Me.lblStartTime.Text = "Start Time:"
        Me.lblStartTime.Visible = False
        '
        'lblCurrentFile
        '
        Me.lblCurrentFile.AutoSize = True
        Me.lblCurrentFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentFile.Location = New System.Drawing.Point(62, 285)
        Me.lblCurrentFile.Name = "lblCurrentFile"
        Me.lblCurrentFile.Size = New System.Drawing.Size(85, 17)
        Me.lblCurrentFile.TabIndex = 70
        Me.lblCurrentFile.Text = "Current File:"
        Me.lblCurrentFile.Visible = False
        '
        'boxDataToLog
        '
        Me.boxDataToLog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxDataToLog.FormattingEnabled = True
        Me.boxDataToLog.Items.AddRange(New Object() {"GGA", "GGA, RMC", "Everything"})
        Me.boxDataToLog.Location = New System.Drawing.Point(153, 56)
        Me.boxDataToLog.Name = "boxDataToLog"
        Me.boxDataToLog.Size = New System.Drawing.Size(156, 21)
        Me.boxDataToLog.TabIndex = 69
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(66, 57)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(81, 17)
        Me.Label24.TabIndex = 68
        Me.Label24.Text = "Data to log:"
        '
        'boxUponCompletion
        '
        Me.boxUponCompletion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxUponCompletion.FormattingEnabled = True
        Me.boxUponCompletion.Items.AddRange(New Object() {"Start Another Log File", "Do Nothing", "Close Program"})
        Me.boxUponCompletion.Location = New System.Drawing.Point(153, 174)
        Me.boxUponCompletion.Name = "boxUponCompletion"
        Me.boxUponCompletion.Size = New System.Drawing.Size(156, 21)
        Me.boxUponCompletion.TabIndex = 67
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(29, 175)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(118, 17)
        Me.Label20.TabIndex = 66
        Me.Label20.Text = "Upon completion:"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(605, 20)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(35, 17)
        Me.Label23.TabIndex = 65
        Me.Label23.Text = ".gps"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(150, 20)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(309, 17)
        Me.Label22.TabIndex = 64
        Me.Label22.Text = "[Program location]\Data\YYYYMMDD-HHMMSS-"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(55, 23)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(92, 17)
        Me.Label21.TabIndex = 63
        Me.Label21.Text = "Save data to:"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(20, 134)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(127, 17)
        Me.Label29.TabIndex = 59
        Me.Label29.Text = "Quit Logging After:"
        '
        'boxLogForMinutes
        '
        Me.boxLogForMinutes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxLogForMinutes.FormattingEnabled = True
        Me.boxLogForMinutes.Location = New System.Drawing.Point(153, 133)
        Me.boxLogForMinutes.Name = "boxLogForMinutes"
        Me.boxLogForMinutes.Size = New System.Drawing.Size(156, 21)
        Me.boxLogForMinutes.TabIndex = 58
        '
        'btnStopRecording
        '
        Me.btnStopRecording.Enabled = False
        Me.btnStopRecording.Location = New System.Drawing.Point(234, 223)
        Me.btnStopRecording.Name = "btnStopRecording"
        Me.btnStopRecording.Size = New System.Drawing.Size(75, 23)
        Me.btnStopRecording.TabIndex = 61
        Me.btnStopRecording.Text = "Stop"
        Me.btnStopRecording.UseVisualStyleBackColor = True
        '
        'btnStartRecording
        '
        Me.btnStartRecording.Location = New System.Drawing.Point(153, 223)
        Me.btnStartRecording.Name = "btnStartRecording"
        Me.btnStartRecording.Size = New System.Drawing.Size(75, 23)
        Me.btnStartRecording.TabIndex = 60
        Me.btnStartRecording.Text = "Start"
        Me.btnStartRecording.UseVisualStyleBackColor = True
        '
        'tbSessionLabel
        '
        Me.tbSessionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbSessionLabel.Location = New System.Drawing.Point(460, 17)
        Me.tbSessionLabel.Name = "tbSessionLabel"
        Me.tbSessionLabel.Size = New System.Drawing.Size(139, 23)
        Me.tbSessionLabel.TabIndex = 56
        Me.tbSessionLabel.Text = "mytest"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatusBar})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 486)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(773, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblStatusBar
        '
        Me.lblStatusBar.Name = "lblStatusBar"
        Me.lblStatusBar.Size = New System.Drawing.Size(69, 17)
        Me.lblStatusBar.Text = "lblStatusBar"
        '
        'Timer1
        '
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(773, 508)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.TabControlCapture)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "Lefebure GPS Capture"
        Me.TabControlCapture.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabNMEADataStream.ResumeLayout(False)
        Me.TabNMEAEvents.ResumeLayout(False)
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage7.PerformLayout()
        Me.TabSatMap.ResumeLayout(False)
        Me.TabSatMap.PerformLayout()
        Me.TabCapt.ResumeLayout(False)
        Me.TabCapt.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TabControlCapture As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents pbNTRIP As System.Windows.Forms.ProgressBar
    Friend WithEvents boxMountpoint As System.Windows.Forms.ComboBox
    Friend WithEvents tbNTRIPPassword As System.Windows.Forms.TextBox
    Friend WithEvents tbNTRIPUsername As System.Windows.Forms.TextBox
    Friend WithEvents tbNTRIPCasterPort As System.Windows.Forms.TextBox
    Friend WithEvents tbNTRIPCasterIP As System.Windows.Forms.TextBox
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents lblNTRIPStatus As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents btnNTRIPConnect As System.Windows.Forms.Button
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents TabNMEADataStream As System.Windows.Forms.TabPage
    Friend WithEvents TabNMEAEvents As System.Windows.Forms.TabPage
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents lblStatPDOP As System.Windows.Forms.Label
    Friend WithEvents lblStatVDOP As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents lblStatCorrStationID As System.Windows.Forms.Label
    Friend WithEvents lblStatCorrAge As System.Windows.Forms.Label
    Friend WithEvents lblStatHDOP As System.Windows.Forms.Label
    Friend WithEvents lblStatSatCount As System.Windows.Forms.Label
    Friend WithEvents lblStatGPSType As System.Windows.Forms.Label
    Friend WithEvents lblStatAltitude As System.Windows.Forms.Label
    Friend WithEvents lblStatSpeed As System.Windows.Forms.Label
    Friend WithEvents lblStatHeading As System.Windows.Forms.Label
    Friend WithEvents lblStatLongitude As System.Windows.Forms.Label
    Friend WithEvents lblStatLatitude As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TabSatMap As System.Windows.Forms.TabPage
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents lblPrnSignal As System.Windows.Forms.Label
    Friend WithEvents btnSatMapRefresh As System.Windows.Forms.Button
    Friend WithEvents TabCapt As System.Windows.Forms.TabPage
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents boxLogForMinutes As System.Windows.Forms.ComboBox
    Friend WithEvents btnStopRecording As System.Windows.Forms.Button
    Friend WithEvents btnStartRecording As System.Windows.Forms.Button
    Friend WithEvents tbSessionLabel As System.Windows.Forms.TextBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatusBar As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents boxDataBits As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents boxSpeed As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents boxSerialPort As System.Windows.Forms.ComboBox
    Friend WithEvents lblSerialStatus As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btnSerialConnect As System.Windows.Forms.Button
    Friend WithEvents lblLastLine As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents boxUponCompletion As System.Windows.Forms.ComboBox
    Friend WithEvents boxDataToLog As System.Windows.Forms.ComboBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents lblCurrentFile As System.Windows.Forms.Label
    Friend WithEvents lblEndTime As System.Windows.Forms.Label
    Friend WithEvents lblStartTime As System.Windows.Forms.Label
    Friend WithEvents rtbEvents As System.Windows.Forms.RichTextBox
    Friend WithEvents rtbDataStream As System.Windows.Forms.RichTextBox
    Friend WithEvents lblNTRIPConnectedAt As System.Windows.Forms.Label
    Friend WithEvents btnEventsToFile As System.Windows.Forms.Button

End Class
