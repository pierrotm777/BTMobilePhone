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
        Me.components = New System.ComponentModel.Container
        Me.Button1 = New System.Windows.Forms.Button
        Me.NetAvail = New System.Windows.Forms.Label
        Me.Connected = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.CallerId = New System.Windows.Forms.Label
        Me.Ringing = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.NetOperator = New System.Windows.Forms.Label
        Me.Subscriber = New System.Windows.Forms.Label
        Me.SignalStrength = New System.Windows.Forms.Label
        Me.BatteryCharge = New System.Windows.Forms.Label
        Me.ManufacturerId = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.ModelId = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.OutgoingCall = New System.Windows.Forms.Label
        Me.Answer = New System.Windows.Forms.Button
        Me.Hangup = New System.Windows.Forms.Button
        Me.Dial = New System.Windows.Forms.Button
        Me.GetNetwork = New System.Windows.Forms.Button
        Me.SubscriberBut = New System.Windows.Forms.Button
        Me.PhoneNumber = New System.Windows.Forms.TextBox
        Me.deviceAddress = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(282, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 48)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Connect"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'NetAvail
        '
        Me.NetAvail.AutoSize = True
        Me.NetAvail.Location = New System.Drawing.Point(101, 27)
        Me.NetAvail.Name = "NetAvail"
        Me.NetAvail.Size = New System.Drawing.Size(47, 13)
        Me.NetAvail.TabIndex = 1
        Me.NetAvail.Text = "NetAvail"
        '
        'Connected
        '
        Me.Connected.AutoSize = True
        Me.Connected.Location = New System.Drawing.Point(101, 9)
        Me.Connected.Name = "Connected"
        Me.Connected.Size = New System.Drawing.Size(32, 13)
        Me.Connected.TabIndex = 2
        Me.Connected.Text = "False"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(381, 15)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 45)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Disconnect"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 10000
        '
        'CallerId
        '
        Me.CallerId.AutoSize = True
        Me.CallerId.Location = New System.Drawing.Point(101, 46)
        Me.CallerId.Name = "CallerId"
        Me.CallerId.Size = New System.Drawing.Size(42, 13)
        Me.CallerId.TabIndex = 4
        Me.CallerId.Text = "CallerId"
        '
        'Ringing
        '
        Me.Ringing.AutoSize = True
        Me.Ringing.Location = New System.Drawing.Point(101, 64)
        Me.Ringing.Name = "Ringing"
        Me.Ringing.Size = New System.Drawing.Size(43, 13)
        Me.Ringing.TabIndex = 5
        Me.Ringing.Text = "Ringing"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "NetAvail"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Caller id"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Ringing"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Connected"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "NetOperator"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 97)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(57, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Subscriber"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 114)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(76, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "SignalStrength"
        '
        'NetOperator
        '
        Me.NetOperator.AutoSize = True
        Me.NetOperator.Location = New System.Drawing.Point(102, 81)
        Me.NetOperator.Name = "NetOperator"
        Me.NetOperator.Size = New System.Drawing.Size(65, 13)
        Me.NetOperator.TabIndex = 13
        Me.NetOperator.Text = "NetOperator"
        '
        'Subscriber
        '
        Me.Subscriber.AutoSize = True
        Me.Subscriber.Location = New System.Drawing.Point(102, 97)
        Me.Subscriber.Name = "Subscriber"
        Me.Subscriber.Size = New System.Drawing.Size(57, 13)
        Me.Subscriber.TabIndex = 14
        Me.Subscriber.Text = "Subscriber"
        '
        'SignalStrength
        '
        Me.SignalStrength.AutoSize = True
        Me.SignalStrength.Location = New System.Drawing.Point(103, 114)
        Me.SignalStrength.Name = "SignalStrength"
        Me.SignalStrength.Size = New System.Drawing.Size(76, 13)
        Me.SignalStrength.TabIndex = 15
        Me.SignalStrength.Text = "SignalStrength"
        '
        'BatteryCharge
        '
        Me.BatteryCharge.AutoSize = True
        Me.BatteryCharge.Location = New System.Drawing.Point(104, 130)
        Me.BatteryCharge.Name = "BatteryCharge"
        Me.BatteryCharge.Size = New System.Drawing.Size(74, 13)
        Me.BatteryCharge.TabIndex = 16
        Me.BatteryCharge.Text = "BatteryCharge"
        '
        'ManufacturerId
        '
        Me.ManufacturerId.AutoSize = True
        Me.ManufacturerId.Location = New System.Drawing.Point(104, 146)
        Me.ManufacturerId.Name = "ManufacturerId"
        Me.ManufacturerId.Size = New System.Drawing.Size(79, 13)
        Me.ManufacturerId.TabIndex = 17
        Me.ManufacturerId.Text = "ManufacturerId"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 130)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(74, 13)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "BatteryCharge"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(12, 146)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(79, 13)
        Me.Label11.TabIndex = 19
        Me.Label11.Text = "ManufacturerId"
        '
        'ModelId
        '
        Me.ModelId.AutoSize = True
        Me.ModelId.Location = New System.Drawing.Point(105, 161)
        Me.ModelId.Name = "ModelId"
        Me.ModelId.Size = New System.Drawing.Size(45, 13)
        Me.ModelId.TabIndex = 20
        Me.ModelId.Text = "ModelId"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 161)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(45, 13)
        Me.Label9.TabIndex = 21
        Me.Label9.Text = "ModelId"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 176)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(67, 13)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "OutgoingCall"
        '
        'OutgoingCall
        '
        Me.OutgoingCall.AutoSize = True
        Me.OutgoingCall.Location = New System.Drawing.Point(105, 176)
        Me.OutgoingCall.Name = "OutgoingCall"
        Me.OutgoingCall.Size = New System.Drawing.Size(67, 13)
        Me.OutgoingCall.TabIndex = 22
        Me.OutgoingCall.Text = "OutgoingCall"
        '
        'Answer
        '
        Me.Answer.Location = New System.Drawing.Point(282, 66)
        Me.Answer.Name = "Answer"
        Me.Answer.Size = New System.Drawing.Size(80, 48)
        Me.Answer.TabIndex = 24
        Me.Answer.Text = "Answer"
        Me.Answer.UseVisualStyleBackColor = True
        '
        'Hangup
        '
        Me.Hangup.Location = New System.Drawing.Point(282, 120)
        Me.Hangup.Name = "Hangup"
        Me.Hangup.Size = New System.Drawing.Size(80, 48)
        Me.Hangup.TabIndex = 25
        Me.Hangup.Text = "Hangup"
        Me.Hangup.UseVisualStyleBackColor = True
        '
        'Dial
        '
        Me.Dial.Location = New System.Drawing.Point(282, 174)
        Me.Dial.Name = "Dial"
        Me.Dial.Size = New System.Drawing.Size(80, 48)
        Me.Dial.TabIndex = 26
        Me.Dial.Text = "Dial"
        Me.Dial.UseVisualStyleBackColor = True
        '
        'GetNetwork
        '
        Me.GetNetwork.Location = New System.Drawing.Point(381, 66)
        Me.GetNetwork.Name = "GetNetwork"
        Me.GetNetwork.Size = New System.Drawing.Size(80, 48)
        Me.GetNetwork.TabIndex = 27
        Me.GetNetwork.Text = "GetNetwork"
        Me.GetNetwork.UseVisualStyleBackColor = True
        '
        'SubscriberBut
        '
        Me.SubscriberBut.Location = New System.Drawing.Point(381, 120)
        Me.SubscriberBut.Name = "SubscriberBut"
        Me.SubscriberBut.Size = New System.Drawing.Size(80, 48)
        Me.SubscriberBut.TabIndex = 28
        Me.SubscriberBut.Text = "Subscriber"
        Me.SubscriberBut.UseVisualStyleBackColor = True
        '
        'PhoneNumber
        '
        Me.PhoneNumber.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PhoneNumber.Location = New System.Drawing.Point(381, 176)
        Me.PhoneNumber.Name = "PhoneNumber"
        Me.PhoneNumber.Size = New System.Drawing.Size(195, 29)
        Me.PhoneNumber.TabIndex = 29
        Me.PhoneNumber.Text = "08450928081"
        '
        'deviceAddress
        '
        Me.deviceAddress.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.deviceAddress.Location = New System.Drawing.Point(15, 209)
        Me.deviceAddress.Name = "deviceAddress"
        Me.deviceAddress.Size = New System.Drawing.Size(195, 29)
        Me.deviceAddress.TabIndex = 30
        Me.deviceAddress.Text = "00:12:D2:A6:30:3E"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 266)
        Me.Controls.Add(Me.deviceAddress)
        Me.Controls.Add(Me.PhoneNumber)
        Me.Controls.Add(Me.SubscriberBut)
        Me.Controls.Add(Me.GetNetwork)
        Me.Controls.Add(Me.Dial)
        Me.Controls.Add(Me.Hangup)
        Me.Controls.Add(Me.Answer)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.OutgoingCall)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.ModelId)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.ManufacturerId)
        Me.Controls.Add(Me.BatteryCharge)
        Me.Controls.Add(Me.SignalStrength)
        Me.Controls.Add(Me.Subscriber)
        Me.Controls.Add(Me.NetOperator)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Ringing)
        Me.Controls.Add(Me.CallerId)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Connected)
        Me.Controls.Add(Me.NetAvail)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents NetAvail As System.Windows.Forms.Label
    Friend WithEvents Connected As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents CallerId As System.Windows.Forms.Label
    Friend WithEvents Ringing As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents NetOperator As System.Windows.Forms.Label
    Friend WithEvents Subscriber As System.Windows.Forms.Label
    Friend WithEvents SignalStrength As System.Windows.Forms.Label
    Friend WithEvents BatteryCharge As System.Windows.Forms.Label
    Friend WithEvents ManufacturerId As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ModelId As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents OutgoingCall As System.Windows.Forms.Label
    Friend WithEvents Answer As System.Windows.Forms.Button
    Friend WithEvents Hangup As System.Windows.Forms.Button
    Friend WithEvents Dial As System.Windows.Forms.Button
    Friend WithEvents GetNetwork As System.Windows.Forms.Button
    Friend WithEvents SubscriberBut As System.Windows.Forms.Button
    Friend WithEvents PhoneNumber As System.Windows.Forms.TextBox
    Friend WithEvents deviceAddress As System.Windows.Forms.TextBox

End Class
