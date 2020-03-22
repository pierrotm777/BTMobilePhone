<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.groupBox4 = New System.Windows.Forms.GroupBox
        Me.label4 = New System.Windows.Forms.Label
        Me.button2 = New System.Windows.Forms.Button
        Me.groupBox5 = New System.Windows.Forms.GroupBox
        Me.label5 = New System.Windows.Forms.Label
        Me.button3 = New System.Windows.Forms.Button
        Me.label3 = New System.Windows.Forms.Label
        Me.groupBox3 = New System.Windows.Forms.GroupBox
        Me.btnSmpp = New System.Windows.Forms.Button
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.label2 = New System.Windows.Forms.Label
        Me.btnMMSMM1 = New System.Windows.Forms.Button
        Me.label1 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Button4 = New System.Windows.Forms.Button
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Button5 = New System.Windows.Forms.Button
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Button6 = New System.Windows.Forms.Button
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Button7 = New System.Windows.Forms.Button
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Button8 = New System.Windows.Forms.Button
        Me.GroupBox11 = New System.Windows.Forms.GroupBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Button9 = New System.Windows.Forms.Button
        Me.GroupBox12 = New System.Windows.Forms.GroupBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Button10 = New System.Windows.Forms.Button
        Me.groupBox4.SuspendLayout()
        Me.groupBox5.SuspendLayout()
        Me.groupBox3.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBox4
        '
        Me.groupBox4.Controls.Add(Me.label4)
        Me.groupBox4.Controls.Add(Me.button2)
        Me.groupBox4.Location = New System.Drawing.Point(12, 310)
        Me.groupBox4.Name = "groupBox4"
        Me.groupBox4.Size = New System.Drawing.Size(415, 79)
        Me.groupBox4.TabIndex = 8
        Me.groupBox4.TabStop = False
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(177, 33)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(209, 26)
        Me.label4.TabIndex = 1
        Me.label4.Text = "Send and receive SMS through Windows " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Mobile Phone connected to your computer"
        '
        'button2
        '
        Me.button2.Enabled = False
        Me.button2.Location = New System.Drawing.Point(19, 19)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(152, 47)
        Me.button2.TabIndex = 0
        Me.button2.Text = "SMS via Windows Mobile"
        Me.button2.UseVisualStyleBackColor = True
        '
        'groupBox5
        '
        Me.groupBox5.Controls.Add(Me.label5)
        Me.groupBox5.Controls.Add(Me.button3)
        Me.groupBox5.Location = New System.Drawing.Point(12, 212)
        Me.groupBox5.Name = "groupBox5"
        Me.groupBox5.Size = New System.Drawing.Size(415, 79)
        Me.groupBox5.TabIndex = 9
        Me.groupBox5.TabStop = False
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(177, 33)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(164, 13)
        Me.label5.TabIndex = 1
        Me.label5.Text = "Send MMS through MM7 protocol"
        '
        'button3
        '
        Me.button3.Location = New System.Drawing.Point(19, 19)
        Me.button3.Name = "button3"
        Me.button3.Size = New System.Drawing.Size(152, 47)
        Me.button3.TabIndex = 0
        Me.button3.Text = "MMS via MM7"
        Me.button3.UseVisualStyleBackColor = True
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(177, 36)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(225, 13)
        Me.label3.TabIndex = 1
        Me.label3.Text = "Send and receive SMS through SMPP protocol"
        '
        'groupBox3
        '
        Me.groupBox3.Controls.Add(Me.label3)
        Me.groupBox3.Controls.Add(Me.btnSmpp)
        Me.groupBox3.Location = New System.Drawing.Point(12, 130)
        Me.groupBox3.Name = "groupBox3"
        Me.groupBox3.Size = New System.Drawing.Size(415, 79)
        Me.groupBox3.TabIndex = 7
        Me.groupBox3.TabStop = False
        '
        'btnSmpp
        '
        Me.btnSmpp.Location = New System.Drawing.Point(19, 19)
        Me.btnSmpp.Name = "btnSmpp"
        Me.btnSmpp.Size = New System.Drawing.Size(152, 47)
        Me.btnSmpp.TabIndex = 0
        Me.btnSmpp.Text = "SMS via SMPP"
        Me.btnSmpp.UseVisualStyleBackColor = True
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.GroupBox12)
        Me.groupBox2.Controls.Add(Me.label1)
        Me.groupBox2.Controls.Add(Me.label2)
        Me.groupBox2.Controls.Add(Me.GroupBox11)
        Me.groupBox2.Controls.Add(Me.btnMMSMM1)
        Me.groupBox2.Controls.Add(Me.GroupBox9)
        Me.groupBox2.Controls.Add(Me.GroupBox10)
        Me.groupBox2.Location = New System.Drawing.Point(12, 12)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(415, 108)
        Me.groupBox2.TabIndex = 6
        Me.groupBox2.TabStop = False
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(177, 57)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(223, 39)
        Me.label2.TabIndex = 1
        Me.label2.Text = "Send and receive MMS through MM1 protocol" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "using GPRS/3G modem connected to your " & _
            "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "computer"
        '
        'btnMMSMM1
        '
        Me.btnMMSMM1.Location = New System.Drawing.Point(6, 16)
        Me.btnMMSMM1.Name = "btnMMSMM1"
        Me.btnMMSMM1.Size = New System.Drawing.Size(152, 80)
        Me.btnMMSMM1.TabIndex = 0
        Me.btnMMSMM1.Text = "SMS via GSM Modem" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "MMS via MM1"
        Me.btnMMSMM1.UseVisualStyleBackColor = True
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(177, 16)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(215, 26)
        Me.label1.TabIndex = 2
        Me.label1.Text = "Send and receive SMS through GSM modem" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "connected to your computer"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(415, 108)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(177, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(215, 26)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Send and receive SMS through GSM modem" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "connected to your computer"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(177, 57)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(223, 39)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Send and receive MMS through MM1 protocol" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "using GPRS/3G modem connected to your " & _
            "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "computer"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(6, 16)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(152, 80)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "SMS via GSM Modem" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "MMS via MM1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.Label8)
        Me.GroupBox6.Controls.Add(Me.Button4)
        Me.GroupBox6.Location = New System.Drawing.Point(12, 130)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(415, 79)
        Me.GroupBox6.TabIndex = 7
        Me.GroupBox6.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(177, 36)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(225, 13)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Send and receive SMS through SMPP protocol"
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(19, 19)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(152, 47)
        Me.Button4.TabIndex = 0
        Me.Button4.Text = "SMS via SMPP"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.Label9)
        Me.GroupBox7.Controls.Add(Me.Button5)
        Me.GroupBox7.Location = New System.Drawing.Point(12, 212)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(415, 79)
        Me.GroupBox7.TabIndex = 9
        Me.GroupBox7.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(177, 33)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(164, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Send MMS through MM7 protocol"
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(19, 19)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(152, 47)
        Me.Button5.TabIndex = 0
        Me.Button5.Text = "MMS via MM7"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.Label10)
        Me.GroupBox8.Controls.Add(Me.Button6)
        Me.GroupBox8.Location = New System.Drawing.Point(12, 310)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(415, 79)
        Me.GroupBox8.TabIndex = 8
        Me.GroupBox8.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(177, 33)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(209, 26)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Send and receive SMS through Windows " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Mobile Phone connected to your computer"
        '
        'Button6
        '
        Me.Button6.Enabled = False
        Me.Button6.Location = New System.Drawing.Point(19, 19)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(152, 47)
        Me.Button6.TabIndex = 0
        Me.Button6.Text = "SMS via Windows Mobile"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.Label11)
        Me.GroupBox9.Controls.Add(Me.Label12)
        Me.GroupBox9.Controls.Add(Me.Button7)
        Me.GroupBox9.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(415, 108)
        Me.GroupBox9.TabIndex = 6
        Me.GroupBox9.TabStop = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(177, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(215, 26)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Send and receive SMS through GSM modem" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "connected to your computer"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(177, 57)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(223, 39)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Send and receive MMS through MM1 protocol" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "using GPRS/3G modem connected to your " & _
            "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "computer"
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(6, 16)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(152, 80)
        Me.Button7.TabIndex = 0
        Me.Button7.Text = "SMS via GSM Modem" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "MMS via MM1"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.Label13)
        Me.GroupBox10.Controls.Add(Me.Button8)
        Me.GroupBox10.Location = New System.Drawing.Point(0, 165)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(415, 79)
        Me.GroupBox10.TabIndex = 7
        Me.GroupBox10.TabStop = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(177, 36)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(225, 13)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "Send and receive SMS through SMPP protocol"
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(19, 19)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(152, 47)
        Me.Button8.TabIndex = 0
        Me.Button8.Text = "SMS via SMPP"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.Label14)
        Me.GroupBox11.Controls.Add(Me.Button9)
        Me.GroupBox11.Location = New System.Drawing.Point(0, 247)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(415, 79)
        Me.GroupBox11.TabIndex = 9
        Me.GroupBox11.TabStop = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(177, 33)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(164, 13)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "Send MMS through MM7 protocol"
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(19, 19)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(152, 47)
        Me.Button9.TabIndex = 0
        Me.Button9.Text = "MMS via MM7"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.Label15)
        Me.GroupBox12.Controls.Add(Me.Button10)
        Me.GroupBox12.Location = New System.Drawing.Point(0, 345)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(415, 79)
        Me.GroupBox12.TabIndex = 8
        Me.GroupBox12.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(177, 33)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(209, 26)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "Send and receive SMS through Windows " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Mobile Phone connected to your computer"
        '
        'Button10
        '
        Me.Button10.Enabled = False
        Me.Button10.Location = New System.Drawing.Point(19, 19)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(152, 47)
        Me.Button10.TabIndex = 0
        Me.Button10.Text = "SMS via Windows Mobile"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(441, 214)
        Me.Controls.Add(Me.GroupBox8)
        Me.Controls.Add(Me.groupBox4)
        Me.Controls.Add(Me.GroupBox7)
        Me.Controls.Add(Me.groupBox5)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.groupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.groupBox2)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.groupBox4.ResumeLayout(False)
        Me.groupBox4.PerformLayout()
        Me.groupBox5.ResumeLayout(False)
        Me.groupBox5.PerformLayout()
        Me.groupBox3.ResumeLayout(False)
        Me.groupBox3.PerformLayout()
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout()
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox12.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents groupBox4 As System.Windows.Forms.GroupBox
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents button2 As System.Windows.Forms.Button
    Private WithEvents groupBox5 As System.Windows.Forms.GroupBox
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents button3 As System.Windows.Forms.Button
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents groupBox3 As System.Windows.Forms.GroupBox
    Private WithEvents btnSmpp As System.Windows.Forms.Button
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents btnMMSMM1 As System.Windows.Forms.Button
    Private WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Private WithEvents Label15 As System.Windows.Forms.Label
    Private WithEvents Button10 As System.Windows.Forms.Button
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Private WithEvents Label14 As System.Windows.Forms.Label
    Private WithEvents Button9 As System.Windows.Forms.Button
    Private WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Private WithEvents Label11 As System.Windows.Forms.Label
    Private WithEvents Label12 As System.Windows.Forms.Label
    Private WithEvents Button7 As System.Windows.Forms.Button
    Private WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Private WithEvents Label13 As System.Windows.Forms.Label
    Private WithEvents Button8 As System.Windows.Forms.Button
    Private WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents Label6 As System.Windows.Forms.Label
    Private WithEvents Label7 As System.Windows.Forms.Label
    Private WithEvents Button1 As System.Windows.Forms.Button
    Private WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Private WithEvents Label8 As System.Windows.Forms.Label
    Private WithEvents Button4 As System.Windows.Forms.Button
    Private WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Private WithEvents Label9 As System.Windows.Forms.Label
    Private WithEvents Button5 As System.Windows.Forms.Button
    Private WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Private WithEvents Label10 As System.Windows.Forms.Label
    Private WithEvents Button6 As System.Windows.Forms.Button

End Class
