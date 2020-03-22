<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GenSet
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.btnAutoTurn = New System.Windows.Forms.Button
        Me.Label19 = New System.Windows.Forms.Label
        Me.btnElevM5cm = New System.Windows.Forms.Button
        Me.btnElevP5cm = New System.Windows.Forms.Button
        Me.btnElevM1m = New System.Windows.Forms.Button
        Me.btnElevP1m = New System.Windows.Forms.Button
        Me.tbElevation = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.boxPositionType = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbLon = New System.Windows.Forms.TextBox
        Me.tbLat = New System.Windows.Forms.TextBox
        Me.tbCHeading = New System.Windows.Forms.TextBox
        Me.tbTHeading = New System.Windows.Forms.TextBox
        Me.tbSpeed = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.btnP1MPH = New System.Windows.Forms.Button
        Me.btnM1MPH = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.lblIsMoving = New System.Windows.Forms.Label
        Me.btnTurnRight90 = New System.Windows.Forms.Button
        Me.btnTurnLeft90 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnStop = New System.Windows.Forms.Button
        Me.btnStart = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnAutoTurn
        '
        Me.btnAutoTurn.Location = New System.Drawing.Point(413, 89)
        Me.btnAutoTurn.Name = "btnAutoTurn"
        Me.btnAutoTurn.Size = New System.Drawing.Size(54, 23)
        Me.btnAutoTurn.TabIndex = 129
        Me.btnAutoTurn.Text = "Auto"
        Me.btnAutoTurn.UseVisualStyleBackColor = True
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(173, 198)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(45, 13)
        Me.Label19.TabIndex = 128
        Me.Label19.Text = "(Meters)"
        '
        'btnElevM5cm
        '
        Me.btnElevM5cm.Location = New System.Drawing.Point(389, 193)
        Me.btnElevM5cm.Name = "btnElevM5cm"
        Me.btnElevM5cm.Size = New System.Drawing.Size(49, 23)
        Me.btnElevM5cm.TabIndex = 127
        Me.btnElevM5cm.Text = "-5 cm"
        Me.btnElevM5cm.UseVisualStyleBackColor = True
        '
        'btnElevP5cm
        '
        Me.btnElevP5cm.Location = New System.Drawing.Point(224, 193)
        Me.btnElevP5cm.Name = "btnElevP5cm"
        Me.btnElevP5cm.Size = New System.Drawing.Size(49, 23)
        Me.btnElevP5cm.TabIndex = 126
        Me.btnElevP5cm.Text = "+5 cm"
        Me.btnElevP5cm.UseVisualStyleBackColor = True
        '
        'btnElevM1m
        '
        Me.btnElevM1m.Location = New System.Drawing.Point(334, 193)
        Me.btnElevM1m.Name = "btnElevM1m"
        Me.btnElevM1m.Size = New System.Drawing.Size(49, 23)
        Me.btnElevM1m.TabIndex = 125
        Me.btnElevM1m.Text = "-1 M"
        Me.btnElevM1m.UseVisualStyleBackColor = True
        '
        'btnElevP1m
        '
        Me.btnElevP1m.Location = New System.Drawing.Point(279, 193)
        Me.btnElevP1m.Name = "btnElevP1m"
        Me.btnElevP1m.Size = New System.Drawing.Size(49, 23)
        Me.btnElevP1m.TabIndex = 124
        Me.btnElevP1m.Text = "+1 M"
        Me.btnElevP1m.UseVisualStyleBackColor = True
        '
        'tbElevation
        '
        Me.tbElevation.Location = New System.Drawing.Point(100, 195)
        Me.tbElevation.Name = "tbElevation"
        Me.tbElevation.Size = New System.Drawing.Size(67, 20)
        Me.tbElevation.TabIndex = 123
        Me.tbElevation.Text = "200"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(40, 198)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(54, 13)
        Me.Label17.TabIndex = 122
        Me.Label17.Text = "Elevation:"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(20, 9)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(74, 13)
        Me.Label18.TabIndex = 121
        Me.Label18.Text = "Position Type:"
        '
        'boxPositionType
        '
        Me.boxPositionType.FormattingEnabled = True
        Me.boxPositionType.Items.AddRange(New Object() {"0 - Invalid Position", "1 - Plain GPS (No Differential)", "2 - Differential GPS", "4 - RTK", "5 - Float RTK", "8 - Simulation"})
        Me.boxPositionType.Location = New System.Drawing.Point(100, 6)
        Me.boxPositionType.Name = "boxPositionType"
        Me.boxPositionType.Size = New System.Drawing.Size(267, 21)
        Me.boxPositionType.TabIndex = 120
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(222, 172)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(81, 13)
        Me.Label10.TabIndex = 119
        Me.Label10.Text = "(ie: -91.671454)"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(222, 146)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(78, 13)
        Me.Label9.TabIndex = 118
        Me.Label9.Text = "(ie: 41.977296)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(159, 120)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(145, 13)
        Me.Label3.TabIndex = 117
        Me.Label3.Text = "0=N,  90=E,  180=S,  270=W"
        '
        'tbLon
        '
        Me.tbLon.Location = New System.Drawing.Point(100, 169)
        Me.tbLon.Name = "tbLon"
        Me.tbLon.Size = New System.Drawing.Size(107, 20)
        Me.tbLon.TabIndex = 116
        Me.tbLon.Text = "-91.671454"
        '
        'tbLat
        '
        Me.tbLat.Location = New System.Drawing.Point(100, 143)
        Me.tbLat.Name = "tbLat"
        Me.tbLat.Size = New System.Drawing.Size(107, 20)
        Me.tbLat.TabIndex = 115
        Me.tbLat.Text = "41.977296"
        '
        'tbCHeading
        '
        Me.tbCHeading.Enabled = False
        Me.tbCHeading.Location = New System.Drawing.Point(100, 117)
        Me.tbCHeading.Name = "tbCHeading"
        Me.tbCHeading.Size = New System.Drawing.Size(39, 20)
        Me.tbCHeading.TabIndex = 114
        Me.tbCHeading.Text = "0"
        '
        'tbTHeading
        '
        Me.tbTHeading.Location = New System.Drawing.Point(100, 91)
        Me.tbTHeading.Name = "tbTHeading"
        Me.tbTHeading.Size = New System.Drawing.Size(39, 20)
        Me.tbTHeading.TabIndex = 113
        Me.tbTHeading.Text = "0"
        '
        'tbSpeed
        '
        Me.tbSpeed.Location = New System.Drawing.Point(100, 65)
        Me.tbSpeed.Name = "tbSpeed"
        Me.tbSpeed.Size = New System.Drawing.Size(39, 20)
        Me.tbSpeed.TabIndex = 112
        Me.tbSpeed.Text = "5"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(37, 172)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(57, 13)
        Me.Label13.TabIndex = 111
        Me.Label13.Text = "Longitude:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(46, 146)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(48, 13)
        Me.Label12.TabIndex = 110
        Me.Label12.Text = "Latitude:"
        '
        'btnP1MPH
        '
        Me.btnP1MPH.Location = New System.Drawing.Point(213, 63)
        Me.btnP1MPH.Name = "btnP1MPH"
        Me.btnP1MPH.Size = New System.Drawing.Size(61, 23)
        Me.btnP1MPH.TabIndex = 109
        Me.btnP1MPH.Text = "+1 MPH"
        Me.btnP1MPH.UseVisualStyleBackColor = True
        '
        'btnM1MPH
        '
        Me.btnM1MPH.Location = New System.Drawing.Point(146, 63)
        Me.btnM1MPH.Name = "btnM1MPH"
        Me.btnM1MPH.Size = New System.Drawing.Size(61, 23)
        Me.btnM1MPH.TabIndex = 108
        Me.btnM1MPH.Text = "-1 MPH"
        Me.btnM1MPH.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(53, 68)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(41, 13)
        Me.Label8.TabIndex = 107
        Me.Label8.Text = "Speed:"
        '
        'lblIsMoving
        '
        Me.lblIsMoving.AutoSize = True
        Me.lblIsMoving.Location = New System.Drawing.Point(100, 42)
        Me.lblIsMoving.Name = "lblIsMoving"
        Me.lblIsMoving.Size = New System.Drawing.Size(21, 13)
        Me.lblIsMoving.TabIndex = 102
        Me.lblIsMoving.Text = "No"
        '
        'btnTurnRight90
        '
        Me.btnTurnRight90.Location = New System.Drawing.Point(279, 89)
        Me.btnTurnRight90.Name = "btnTurnRight90"
        Me.btnTurnRight90.Size = New System.Drawing.Size(128, 23)
        Me.btnTurnRight90.TabIndex = 101
        Me.btnTurnRight90.Text = "Turn Right 90 Degrees"
        Me.btnTurnRight90.UseVisualStyleBackColor = True
        '
        'btnTurnLeft90
        '
        Me.btnTurnLeft90.Location = New System.Drawing.Point(145, 89)
        Me.btnTurnLeft90.Name = "btnTurnLeft90"
        Me.btnTurnLeft90.Size = New System.Drawing.Size(128, 23)
        Me.btnTurnLeft90.TabIndex = 100
        Me.btnTurnLeft90.Text = "Turn Left 90 Degrees"
        Me.btnTurnLeft90.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(35, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 99
        Me.Label2.Text = "Is Moving?"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(10, 120)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 13)
        Me.Label7.TabIndex = 106
        Me.Label7.Text = "Current Heading:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 94)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 13)
        Me.Label6.TabIndex = 105
        Me.Label6.Text = "Target Heading:"
        '
        'btnStop
        '
        Me.btnStop.Location = New System.Drawing.Point(213, 37)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(61, 23)
        Me.btnStop.TabIndex = 104
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(146, 37)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(61, 23)
        Me.btnStart.TabIndex = 103
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'GenSet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnAutoTurn)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.btnElevM5cm)
        Me.Controls.Add(Me.btnElevP5cm)
        Me.Controls.Add(Me.btnElevM1m)
        Me.Controls.Add(Me.btnElevP1m)
        Me.Controls.Add(Me.tbElevation)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.boxPositionType)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbLon)
        Me.Controls.Add(Me.tbLat)
        Me.Controls.Add(Me.tbCHeading)
        Me.Controls.Add(Me.tbTHeading)
        Me.Controls.Add(Me.tbSpeed)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.btnP1MPH)
        Me.Controls.Add(Me.btnM1MPH)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.lblIsMoving)
        Me.Controls.Add(Me.btnTurnRight90)
        Me.Controls.Add(Me.btnTurnLeft90)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.btnStart)
        Me.Name = "GenSet"
        Me.Size = New System.Drawing.Size(511, 217)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAutoTurn As System.Windows.Forms.Button
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btnElevM5cm As System.Windows.Forms.Button
    Friend WithEvents btnElevP5cm As System.Windows.Forms.Button
    Friend WithEvents btnElevM1m As System.Windows.Forms.Button
    Friend WithEvents btnElevP1m As System.Windows.Forms.Button
    Friend WithEvents tbElevation As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents boxPositionType As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbLon As System.Windows.Forms.TextBox
    Friend WithEvents tbLat As System.Windows.Forms.TextBox
    Friend WithEvents tbCHeading As System.Windows.Forms.TextBox
    Friend WithEvents tbTHeading As System.Windows.Forms.TextBox
    Friend WithEvents tbSpeed As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnP1MPH As System.Windows.Forms.Button
    Friend WithEvents btnM1MPH As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblIsMoving As System.Windows.Forms.Label
    Friend WithEvents btnTurnRight90 As System.Windows.Forms.Button
    Friend WithEvents btnTurnLeft90 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents btnStart As System.Windows.Forms.Button

End Class
