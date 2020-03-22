<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Simulator
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Simulator))
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.boxRMCHertz = New System.Windows.Forms.ComboBox()
        Me.btnDisconnect = New System.Windows.Forms.Button()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnClearLog = New System.Windows.Forms.Button()
        Me.lblLog = New System.Windows.Forms.Label()
        Me.boxSerialSpeed = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.boxSerialPorts = New System.Windows.Forms.ComboBox()
        Me.boxDataSource = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblBytesRecLabel = New System.Windows.Forms.Label()
        Me.lblBytesReceived = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(48, 99)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(94, 13)
        Me.Label16.TabIndex = 88
        Me.Label16.Text = "NMEA Frequency:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(81, 47)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(61, 13)
        Me.Label15.TabIndex = 87
        Me.Label15.Text = "Baud Rate:"
        '
        'boxRMCHertz
        '
        Me.boxRMCHertz.FormattingEnabled = True
        Me.boxRMCHertz.Items.AddRange(New Object() {"1 Hz", "2 Hz", "5 Hz", "10 Hz"})
        Me.boxRMCHertz.Location = New System.Drawing.Point(148, 96)
        Me.boxRMCHertz.Name = "boxRMCHertz"
        Me.boxRMCHertz.Size = New System.Drawing.Size(107, 21)
        Me.boxRMCHertz.TabIndex = 86
        '
        'btnDisconnect
        '
        Me.btnDisconnect.Enabled = False
        Me.btnDisconnect.Location = New System.Drawing.Point(440, 42)
        Me.btnDisconnect.Name = "btnDisconnect"
        Me.btnDisconnect.Size = New System.Drawing.Size(75, 23)
        Me.btnDisconnect.TabIndex = 85
        Me.btnDisconnect.Text = "Disconnect"
        Me.btnDisconnect.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.Black
        Me.Label14.Location = New System.Drawing.Point(0, 348)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(527, 2)
        Me.Label14.TabIndex = 84
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(0, 123)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(527, 2)
        Me.Label11.TabIndex = 83
        '
        'btnClearLog
        '
        Me.btnClearLog.Location = New System.Drawing.Point(453, 368)
        Me.btnClearLog.Name = "btnClearLog"
        Me.btnClearLog.Size = New System.Drawing.Size(61, 23)
        Me.btnClearLog.TabIndex = 82
        Me.btnClearLog.Text = "Clear Log"
        Me.btnClearLog.UseVisualStyleBackColor = True
        '
        'lblLog
        '
        Me.lblLog.AutoSize = True
        Me.lblLog.Location = New System.Drawing.Point(12, 373)
        Me.lblLog.Name = "lblLog"
        Me.lblLog.Size = New System.Drawing.Size(171, 13)
        Me.lblLog.TabIndex = 81
        Me.lblLog.Text = "Generated NMEA data will go here"
        '
        'boxSerialSpeed
        '
        Me.boxSerialSpeed.FormattingEnabled = True
        Me.boxSerialSpeed.Items.AddRange(New Object() {"4800", "9600", "14400", "19200", "38400", "57600", "115200"})
        Me.boxSerialSpeed.Location = New System.Drawing.Point(148, 44)
        Me.boxSerialSpeed.Name = "boxSerialSpeed"
        Me.boxSerialSpeed.Size = New System.Drawing.Size(107, 21)
        Me.boxSerialSpeed.TabIndex = 77
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(411, 521)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(103, 13)
        Me.Label5.TabIndex = 71
        Me.Label5.Text = "http://Lefebure.com"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(12, 521)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(42, 13)
        Me.lblVersion.TabIndex = 70
        Me.lblVersion.Text = "Version"
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(359, 42)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnConnect.TabIndex = 56
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(130, 13)
        Me.Label1.TabIndex = 55
        Me.Label1.Text = "COM Port to send data to:"
        '
        'boxSerialPorts
        '
        Me.boxSerialPorts.FormattingEnabled = True
        Me.boxSerialPorts.Location = New System.Drawing.Point(148, 18)
        Me.boxSerialPorts.Name = "boxSerialPorts"
        Me.boxSerialPorts.Size = New System.Drawing.Size(107, 21)
        Me.boxSerialPorts.TabIndex = 54
        '
        'boxDataSource
        '
        Me.boxDataSource.FormattingEnabled = True
        Me.boxDataSource.Items.AddRange(New Object() {"Generated", "From File"})
        Me.boxDataSource.Location = New System.Drawing.Point(148, 70)
        Me.boxDataSource.Name = "boxDataSource"
        Me.boxDataSource.Size = New System.Drawing.Size(107, 21)
        Me.boxDataSource.TabIndex = 99
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(72, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 13)
        Me.Label4.TabIndex = 100
        Me.Label4.Text = "Data Source:"
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(3, 128)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(511, 217)
        Me.Panel1.TabIndex = 101
        '
        'lblBytesRecLabel
        '
        Me.lblBytesRecLabel.AutoSize = True
        Me.lblBytesRecLabel.Location = New System.Drawing.Point(324, 99)
        Me.lblBytesRecLabel.Name = "lblBytesRecLabel"
        Me.lblBytesRecLabel.Size = New System.Drawing.Size(85, 13)
        Me.lblBytesRecLabel.TabIndex = 102
        Me.lblBytesRecLabel.Text = "Bytes Received:"
        '
        'lblBytesReceived
        '
        Me.lblBytesReceived.AutoSize = True
        Me.lblBytesReceived.Location = New System.Drawing.Point(412, 99)
        Me.lblBytesReceived.Name = "lblBytesReceived"
        Me.lblBytesReceived.Size = New System.Drawing.Size(13, 13)
        Me.lblBytesReceived.TabIndex = 103
        Me.lblBytesReceived.Text = "0"
        '
        'Simulator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(519, 541)
        Me.Controls.Add(Me.lblBytesReceived)
        Me.Controls.Add(Me.lblBytesRecLabel)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.boxDataSource)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.boxRMCHertz)
        Me.Controls.Add(Me.btnDisconnect)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.btnClearLog)
        Me.Controls.Add(Me.lblLog)
        Me.Controls.Add(Me.boxSerialSpeed)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnConnect)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.boxSerialPorts)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(535, 577)
        Me.MinimumSize = New System.Drawing.Size(535, 577)
        Me.Name = "Simulator"
        Me.Text = "Lefebure GPS Receiver Simulator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents boxRMCHertz As System.Windows.Forms.ComboBox
    Friend WithEvents btnDisconnect As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnClearLog As System.Windows.Forms.Button
    Friend WithEvents lblLog As System.Windows.Forms.Label
    Friend WithEvents boxSerialSpeed As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents boxSerialPorts As System.Windows.Forms.ComboBox
    Friend WithEvents boxDataSource As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblBytesRecLabel As System.Windows.Forms.Label
    Friend WithEvents lblBytesReceived As System.Windows.Forms.Label

End Class
