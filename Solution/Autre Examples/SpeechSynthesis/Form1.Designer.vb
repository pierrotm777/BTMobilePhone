Namespace SoundsVB

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class speakerForm
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
            Me.speakTextBox = New System.Windows.Forms.TextBox()
            Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
            Me.volumeLabel = New System.Windows.Forms.Label()
            Me.volumeUpDown = New System.Windows.Forms.NumericUpDown()
            Me.speedLabel = New System.Windows.Forms.Label()
            Me.speedUpDown = New System.Windows.Forms.NumericUpDown()
            Me.fileButton = New System.Windows.Forms.Button()
            Me.fileTextBox = New System.Windows.Forms.TextBox()
            Me.exitButton = New System.Windows.Forms.Button()
            Me.exportButton = New System.Windows.Forms.Button()
            Me.speakButton = New System.Windows.Forms.Button()
            CType(Me.volumeUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.speedUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'speakTextBox
            '
            Me.speakTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.speakTextBox.Location = New System.Drawing.Point(12, 12)
            Me.speakTextBox.Multiline = True
            Me.speakTextBox.Name = "speakTextBox"
            Me.speakTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.speakTextBox.Size = New System.Drawing.Size(240, 140)
            Me.speakTextBox.TabIndex = 1
            Me.speakTextBox.Text = "Type here to speak."
            '
            'volumeLabel
            '
            Me.volumeLabel.AutoSize = True
            Me.volumeLabel.Location = New System.Drawing.Point(11, 160)
            Me.volumeLabel.Name = "volumeLabel"
            Me.volumeLabel.Size = New System.Drawing.Size(45, 13)
            Me.volumeLabel.TabIndex = 18
            Me.volumeLabel.Text = "Volume:"
            '
            'volumeUpDown
            '
            Me.volumeUpDown.Location = New System.Drawing.Point(62, 158)
            Me.volumeUpDown.Name = "volumeUpDown"
            Me.volumeUpDown.Size = New System.Drawing.Size(63, 20)
            Me.volumeUpDown.TabIndex = 10
            Me.volumeUpDown.Value = New Decimal(New Integer() {100, 0, 0, 0})
            '
            'speedLabel
            '
            Me.speedLabel.AutoSize = True
            Me.speedLabel.Location = New System.Drawing.Point(139, 160)
            Me.speedLabel.Name = "speedLabel"
            Me.speedLabel.Size = New System.Drawing.Size(41, 13)
            Me.speedLabel.TabIndex = 17
            Me.speedLabel.Text = "Speed:"
            '
            'speedUpDown
            '
            Me.speedUpDown.Location = New System.Drawing.Point(186, 158)
            Me.speedUpDown.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
            Me.speedUpDown.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
            Me.speedUpDown.Name = "speedUpDown"
            Me.speedUpDown.Size = New System.Drawing.Size(65, 20)
            Me.speedUpDown.TabIndex = 11
            Me.speedUpDown.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'fileButton
            '
            Me.fileButton.Location = New System.Drawing.Point(223, 182)
            Me.fileButton.Name = "fileButton"
            Me.fileButton.Size = New System.Drawing.Size(28, 23)
            Me.fileButton.TabIndex = 13
            Me.fileButton.Text = "..."
            Me.fileButton.UseVisualStyleBackColor = True
            '
            'fileTextBox
            '
            Me.fileTextBox.Location = New System.Drawing.Point(14, 184)
            Me.fileTextBox.Name = "fileTextBox"
            Me.fileTextBox.ReadOnly = True
            Me.fileTextBox.Size = New System.Drawing.Size(203, 20)
            Me.fileTextBox.TabIndex = 12
            '
            'exitButton
            '
            Me.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.exitButton.Location = New System.Drawing.Point(176, 210)
            Me.exitButton.Name = "exitButton"
            Me.exitButton.Size = New System.Drawing.Size(75, 23)
            Me.exitButton.TabIndex = 16
            Me.exitButton.Text = "E&xit"
            Me.exitButton.UseVisualStyleBackColor = True
            '
            'exportButton
            '
            Me.exportButton.Location = New System.Drawing.Point(95, 210)
            Me.exportButton.Name = "exportButton"
            Me.exportButton.Size = New System.Drawing.Size(75, 23)
            Me.exportButton.TabIndex = 15
            Me.exportButton.Text = "&Export"
            Me.exportButton.UseVisualStyleBackColor = True
            '
            'speakButton
            '
            Me.speakButton.Location = New System.Drawing.Point(14, 210)
            Me.speakButton.Name = "speakButton"
            Me.speakButton.Size = New System.Drawing.Size(75, 23)
            Me.speakButton.TabIndex = 14
            Me.speakButton.Text = "&Speak"
            Me.speakButton.UseVisualStyleBackColor = True
            '
            'speakerForm
            '
            Me.AcceptButton = Me.speakButton
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.exitButton
            Me.ClientSize = New System.Drawing.Size(262, 242)
            Me.Controls.Add(Me.volumeLabel)
            Me.Controls.Add(Me.volumeUpDown)
            Me.Controls.Add(Me.speedLabel)
            Me.Controls.Add(Me.speedUpDown)
            Me.Controls.Add(Me.fileButton)
            Me.Controls.Add(Me.fileTextBox)
            Me.Controls.Add(Me.exitButton)
            Me.Controls.Add(Me.exportButton)
            Me.Controls.Add(Me.speakButton)
            Me.Controls.Add(Me.speakTextBox)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.Name = "speakerForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Speaker (VB)"
            CType(Me.volumeUpDown, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.speedUpDown, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Private WithEvents speakTextBox As System.Windows.Forms.TextBox
        Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
        Private WithEvents volumeLabel As System.Windows.Forms.Label
        Private WithEvents volumeUpDown As System.Windows.Forms.NumericUpDown
        Private WithEvents speedLabel As System.Windows.Forms.Label
        Private WithEvents speedUpDown As System.Windows.Forms.NumericUpDown
        Private WithEvents fileButton As System.Windows.Forms.Button
        Private WithEvents fileTextBox As System.Windows.Forms.TextBox
        Private WithEvents exitButton As System.Windows.Forms.Button
        Private WithEvents exportButton As System.Windows.Forms.Button
        Private WithEvents speakButton As System.Windows.Forms.Button

    End Class
End Namespace