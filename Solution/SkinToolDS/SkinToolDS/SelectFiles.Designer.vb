<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectFiles
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
        Me.btnRRIni = New System.Windows.Forms.Button()
        Me.btnRRDebug = New System.Windows.Forms.Button()
        Me.btnRRTBL = New System.Windows.Forms.Button()
        Me.btnRRDocs = New System.Windows.Forms.Button()
        Me.btnSkinIni = New System.Windows.Forms.Button()
        Me.btnSkinTBL = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnRRIni
        '
        Me.btnRRIni.Location = New System.Drawing.Point(0, 1)
        Me.btnRRIni.Name = "btnRRIni"
        Me.btnRRIni.Size = New System.Drawing.Size(120, 23)
        Me.btnRRIni.TabIndex = 0
        Me.btnRRIni.Text = "RR Ini"
        Me.btnRRIni.UseVisualStyleBackColor = True
        '
        'btnRRDebug
        '
        Me.btnRRDebug.Location = New System.Drawing.Point(0, 23)
        Me.btnRRDebug.Name = "btnRRDebug"
        Me.btnRRDebug.Size = New System.Drawing.Size(120, 23)
        Me.btnRRDebug.TabIndex = 1
        Me.btnRRDebug.Text = "RR Debug"
        Me.btnRRDebug.UseVisualStyleBackColor = True
        '
        'btnRRTBL
        '
        Me.btnRRTBL.Location = New System.Drawing.Point(0, 45)
        Me.btnRRTBL.Name = "btnRRTBL"
        Me.btnRRTBL.Size = New System.Drawing.Size(120, 23)
        Me.btnRRTBL.TabIndex = 2
        Me.btnRRTBL.Text = "RR ExecTBL"
        Me.btnRRTBL.UseVisualStyleBackColor = True
        '
        'btnRRDocs
        '
        Me.btnRRDocs.Location = New System.Drawing.Point(0, 111)
        Me.btnRRDocs.Name = "btnRRDocs"
        Me.btnRRDocs.Size = New System.Drawing.Size(120, 23)
        Me.btnRRDocs.TabIndex = 5
        Me.btnRRDocs.Text = "< RR Doc's"
        Me.btnRRDocs.UseVisualStyleBackColor = True
        '
        'btnSkinIni
        '
        Me.btnSkinIni.Location = New System.Drawing.Point(0, 89)
        Me.btnSkinIni.Name = "btnSkinIni"
        Me.btnSkinIni.Size = New System.Drawing.Size(120, 23)
        Me.btnSkinIni.TabIndex = 4
        Me.btnSkinIni.Text = "Skin Ini"
        Me.btnSkinIni.UseVisualStyleBackColor = True
        '
        'btnSkinTBL
        '
        Me.btnSkinTBL.Location = New System.Drawing.Point(0, 67)
        Me.btnSkinTBL.Name = "btnSkinTBL"
        Me.btnSkinTBL.Size = New System.Drawing.Size(120, 23)
        Me.btnSkinTBL.TabIndex = 3
        Me.btnSkinTBL.Text = "Skin ExecTBL"
        Me.btnSkinTBL.UseVisualStyleBackColor = True
        '
        'SelectFiles
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(120, 134)
        Me.Controls.Add(Me.btnRRDocs)
        Me.Controls.Add(Me.btnSkinIni)
        Me.Controls.Add(Me.btnSkinTBL)
        Me.Controls.Add(Me.btnRRTBL)
        Me.Controls.Add(Me.btnRRDebug)
        Me.Controls.Add(Me.btnRRIni)
        Me.Name = "SelectFiles"
        Me.Text = "SelectFiles"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnRRIni As System.Windows.Forms.Button
    Friend WithEvents btnRRDebug As System.Windows.Forms.Button
    Friend WithEvents btnRRTBL As System.Windows.Forms.Button
    Friend WithEvents btnRRDocs As System.Windows.Forms.Button
    Friend WithEvents btnSkinIni As System.Windows.Forms.Button
    Friend WithEvents btnSkinTBL As System.Windows.Forms.Button
End Class
