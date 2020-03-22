<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectTools
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnGpsSimu = New System.Windows.Forms.Button()
        Me.btnCaptureGps = New System.Windows.Forms.Button()
        Me.btnProcessGps = New System.Windows.Forms.Button()
        Me.btnColorPicker = New System.Windows.Forms.Button()
        Me.btnKeyCode = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnGpsSimu
        '
        Me.btnGpsSimu.Location = New System.Drawing.Point(12, -1)
        Me.btnGpsSimu.Name = "btnGpsSimu"
        Me.btnGpsSimu.Size = New System.Drawing.Size(53, 43)
        Me.btnGpsSimu.TabIndex = 0
        Me.btnGpsSimu.Text = "Simu Gps"
        Me.btnGpsSimu.UseVisualStyleBackColor = True
        '
        'btnCaptureGps
        '
        Me.btnCaptureGps.Location = New System.Drawing.Point(71, -1)
        Me.btnCaptureGps.Name = "btnCaptureGps"
        Me.btnCaptureGps.Size = New System.Drawing.Size(53, 43)
        Me.btnCaptureGps.TabIndex = 1
        Me.btnCaptureGps.Text = "Capture Gps"
        Me.btnCaptureGps.UseVisualStyleBackColor = True
        '
        'btnProcessGps
        '
        Me.btnProcessGps.Location = New System.Drawing.Point(130, -1)
        Me.btnProcessGps.Name = "btnProcessGps"
        Me.btnProcessGps.Size = New System.Drawing.Size(53, 43)
        Me.btnProcessGps.TabIndex = 2
        Me.btnProcessGps.Text = "Process Gps"
        Me.btnProcessGps.UseVisualStyleBackColor = True
        '
        'btnColorPicker
        '
        Me.btnColorPicker.Location = New System.Drawing.Point(189, -1)
        Me.btnColorPicker.Name = "btnColorPicker"
        Me.btnColorPicker.Size = New System.Drawing.Size(53, 43)
        Me.btnColorPicker.TabIndex = 3
        Me.btnColorPicker.Text = "Color Picker"
        Me.btnColorPicker.UseVisualStyleBackColor = True
        '
        'btnKeyCode
        '
        Me.btnKeyCode.Location = New System.Drawing.Point(248, -1)
        Me.btnKeyCode.Name = "btnKeyCode"
        Me.btnKeyCode.Size = New System.Drawing.Size(53, 43)
        Me.btnKeyCode.TabIndex = 4
        Me.btnKeyCode.Text = "Key Code"
        Me.btnKeyCode.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 44)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(53, 43)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Mouse Pos."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'SelectTools
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(312, 86)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnKeyCode)
        Me.Controls.Add(Me.btnColorPicker)
        Me.Controls.Add(Me.btnProcessGps)
        Me.Controls.Add(Me.btnCaptureGps)
        Me.Controls.Add(Me.btnGpsSimu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "SelectTools"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "SelectFiles"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnGpsSimu As System.Windows.Forms.Button
    Friend WithEvents btnCaptureGps As System.Windows.Forms.Button
    Friend WithEvents btnProcessGps As System.Windows.Forms.Button
    Friend WithEvents btnColorPicker As System.Windows.Forms.Button
    Friend WithEvents btnKeyCode As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
