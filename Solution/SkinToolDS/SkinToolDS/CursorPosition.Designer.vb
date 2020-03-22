<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CursorPosition
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
        Me.components = New System.ComponentModel.Container()
        Me.TimerMouse = New System.Windows.Forms.Timer(Me.components)
        Me.lblCursorPosition = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'TimerMouse
        '
        Me.TimerMouse.Enabled = True
        '
        'lblCursorPosition
        '
        Me.lblCursorPosition.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCursorPosition.AutoSize = True
        Me.lblCursorPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCursorPosition.Location = New System.Drawing.Point(12, 28)
        Me.lblCursorPosition.Name = "lblCursorPosition"
        Me.lblCursorPosition.Size = New System.Drawing.Size(55, 16)
        Me.lblCursorPosition.TabIndex = 0
        Me.lblCursorPosition.Text = "Label1"
        Me.lblCursorPosition.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CursorPosition
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(251, 78)
        Me.Controls.Add(Me.lblCursorPosition)
        Me.Name = "CursorPosition"
        Me.Text = "CursorPosition"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TimerMouse As System.Windows.Forms.Timer
    Friend WithEvents lblCursorPosition As System.Windows.Forms.Label
End Class
