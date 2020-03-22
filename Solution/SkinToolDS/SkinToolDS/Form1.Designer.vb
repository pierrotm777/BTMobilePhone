
Namespace Temperature
	Partial Class MainForm
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
			Me.label1 = New System.Windows.Forms.Label()
			Me.timer1 = New System.Windows.Forms.Timer(Me.components)
			Me.SuspendLayout()
			' 
			' label1
			' 
			Me.label1.AutoSize = True
			Me.label1.BackColor = System.Drawing.SystemColors.ControlText
			Me.label1.Font = New System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
			Me.label1.ForeColor = System.Drawing.Color.Lime
			Me.label1.Location = New System.Drawing.Point(60, 26)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(53, 24)
			Me.label1.TabIndex = 0
			Me.label1.Text = "CPU"
			Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
			' 
			' timer1
			' 
			Me.timer1.Tick += New System.EventHandler(Me.timer1_Tick)
			' 
			' MainForm
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.BackColor = System.Drawing.SystemColors.ControlText
			Me.ClientSize = New System.Drawing.Size(207, 75)
			Me.Controls.Add(Me.label1)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
			Me.Icon = DirectCast(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MaximizeBox = False
			Me.Name = "MainForm"
			Me.Text = "CPU Temperature"
			Me.Load += New System.EventHandler(Me.MainForm_Load)
			Me.Click += New System.EventHandler(Me.MainForm_Click)
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private label1 As System.Windows.Forms.Label
		Private timer1 As System.Windows.Forms.Timer

	End Class
End Namespace


