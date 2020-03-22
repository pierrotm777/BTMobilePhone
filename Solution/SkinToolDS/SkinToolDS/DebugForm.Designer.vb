<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DebugForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DebugForm))
        Me.txtBoxDebuLog = New System.Windows.Forms.TextBox()
        Me.btnDebugClear = New System.Windows.Forms.Button()
        Me.btnDebugSave = New System.Windows.Forms.Button()
        Me.btnDebugPause = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnDebugHide = New System.Windows.Forms.Button()
        Me.chkDebugOnTop = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtBoxDebuLog
        '
        Me.txtBoxDebuLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.txtBoxDebuLog.Location = New System.Drawing.Point(0, 55)
        Me.txtBoxDebuLog.Multiline = True
        Me.txtBoxDebuLog.Name = "txtBoxDebuLog"
        Me.txtBoxDebuLog.Size = New System.Drawing.Size(473, 284)
        Me.txtBoxDebuLog.TabIndex = 31
        '
        'btnDebugClear
        '
        Me.btnDebugClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnDebugClear.Location = New System.Drawing.Point(0, 345)
        Me.btnDebugClear.Name = "btnDebugClear"
        Me.btnDebugClear.Size = New System.Drawing.Size(70, 25)
        Me.btnDebugClear.TabIndex = 32
        Me.btnDebugClear.Text = "Clear"
        Me.btnDebugClear.UseVisualStyleBackColor = True
        '
        'btnDebugSave
        '
        Me.btnDebugSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnDebugSave.Location = New System.Drawing.Point(403, 345)
        Me.btnDebugSave.Name = "btnDebugSave"
        Me.btnDebugSave.Size = New System.Drawing.Size(70, 25)
        Me.btnDebugSave.TabIndex = 33
        Me.btnDebugSave.Text = "Save"
        Me.btnDebugSave.UseVisualStyleBackColor = True
        '
        'btnDebugPause
        '
        Me.btnDebugPause.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnDebugPause.Location = New System.Drawing.Point(76, 345)
        Me.btnDebugPause.Name = "btnDebugPause"
        Me.btnDebugPause.Size = New System.Drawing.Size(321, 25)
        Me.btnDebugPause.TabIndex = 34
        Me.btnDebugPause.Text = "Pause Debugger"
        Me.btnDebugPause.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.btnDebugHide)
        Me.Panel1.Controls.Add(Me.chkDebugOnTop)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblInfo)
        Me.Panel1.Controls.Add(Me.txtBoxDebuLog)
        Me.Panel1.Controls.Add(Me.btnDebugPause)
        Me.Panel1.Controls.Add(Me.btnDebugSave)
        Me.Panel1.Controls.Add(Me.btnDebugClear)
        Me.Panel1.Location = New System.Drawing.Point(6, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(473, 373)
        Me.Panel1.TabIndex = 35
        '
        'btnDebugHide
        '
        Me.btnDebugHide.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnDebugHide.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnDebugHide.Location = New System.Drawing.Point(397, 5)
        Me.btnDebugHide.Name = "btnDebugHide"
        Me.btnDebugHide.Size = New System.Drawing.Size(70, 25)
        Me.btnDebugHide.TabIndex = 39
        Me.btnDebugHide.Text = "Hide"
        Me.btnDebugHide.UseVisualStyleBackColor = True
        '
        'chkDebugOnTop
        '
        Me.chkDebugOnTop.AutoSize = True
        Me.chkDebugOnTop.Location = New System.Drawing.Point(399, 33)
        Me.chkDebugOnTop.Name = "chkDebugOnTop"
        Me.chkDebugOnTop.Size = New System.Drawing.Size(68, 17)
        Me.chkDebugOnTop.TabIndex = 38
        Me.chkDebugOnTop.Text = "On Top?"
        Me.chkDebugOnTop.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(153, 13)
        Me.Label1.TabIndex = 37
        Me.Label1.Text = "To copy a line to the clipboard:"
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo.Location = New System.Drawing.Point(6, 34)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(195, 13)
        Me.lblInfo.TabIndex = 36
        Me.lblInfo.Text = "Select Line > Right Click > To Clipboard"
        '
        'DebugForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.CancelButton = Me.btnDebugHide
        Me.ClientSize = New System.Drawing.Size(485, 374)
        Me.Controls.Add(Me.Panel1)
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(501, 410)
        Me.Name = "DebugForm"
        Me.Text = "DebugForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtBoxDebuLog As System.Windows.Forms.TextBox
    Friend WithEvents btnDebugClear As System.Windows.Forms.Button
    Friend WithEvents btnDebugSave As System.Windows.Forms.Button
    Friend WithEvents btnDebugPause As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnDebugHide As System.Windows.Forms.Button
    Friend WithEvents chkDebugOnTop As System.Windows.Forms.CheckBox
End Class
