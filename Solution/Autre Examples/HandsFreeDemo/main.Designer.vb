<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fmMain
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
        Me.btCallNumber = New System.Windows.Forms.Button
        Me.edNumber = New System.Windows.Forms.TextBox
        Me.btAnswerCall = New System.Windows.Forms.Button
        Me.btCancelCall = New System.Windows.Forms.Button
        Me.btDisconnect = New System.Windows.Forms.Button
        Me.btConnect = New System.Windows.Forms.Button
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.lvDevices = New System.Windows.Forms.ListView
        Me.chAddress = New System.Windows.Forms.ColumnHeader
        Me.chName = New System.Windows.Forms.ColumnHeader
        Me.btDiscover = New System.Windows.Forms.Button
        Me.btGetManu = New System.Windows.Forms.Button
        Me.btGetModel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btCallNumber
        '
        Me.btCallNumber.Location = New System.Drawing.Point(174, 173)
        Me.btCallNumber.Name = "btCallNumber"
        Me.btCallNumber.Size = New System.Drawing.Size(75, 23)
        Me.btCallNumber.TabIndex = 16
        Me.btCallNumber.Text = "Call number"
        Me.btCallNumber.UseVisualStyleBackColor = True
        '
        'edNumber
        '
        Me.edNumber.Location = New System.Drawing.Point(12, 175)
        Me.edNumber.Name = "edNumber"
        Me.edNumber.Size = New System.Drawing.Size(156, 20)
        Me.edNumber.TabIndex = 15
        Me.edNumber.Text = "+375299460092"
        '
        'btAnswerCall
        '
        Me.btAnswerCall.Location = New System.Drawing.Point(255, 144)
        Me.btAnswerCall.Name = "btAnswerCall"
        Me.btAnswerCall.Size = New System.Drawing.Size(75, 23)
        Me.btAnswerCall.TabIndex = 14
        Me.btAnswerCall.Text = "Answer call"
        Me.btAnswerCall.UseVisualStyleBackColor = True
        '
        'btCancelCall
        '
        Me.btCancelCall.Location = New System.Drawing.Point(174, 144)
        Me.btCancelCall.Name = "btCancelCall"
        Me.btCancelCall.Size = New System.Drawing.Size(75, 23)
        Me.btCancelCall.TabIndex = 13
        Me.btCancelCall.Text = "Cancel call"
        Me.btCancelCall.UseVisualStyleBackColor = True
        '
        'btDisconnect
        '
        Me.btDisconnect.Location = New System.Drawing.Point(93, 144)
        Me.btDisconnect.Name = "btDisconnect"
        Me.btDisconnect.Size = New System.Drawing.Size(75, 23)
        Me.btDisconnect.TabIndex = 12
        Me.btDisconnect.Text = "Disconnect"
        Me.btDisconnect.UseVisualStyleBackColor = True
        '
        'btConnect
        '
        Me.btConnect.Location = New System.Drawing.Point(12, 144)
        Me.btConnect.Name = "btConnect"
        Me.btConnect.Size = New System.Drawing.Size(75, 23)
        Me.btConnect.TabIndex = 11
        Me.btConnect.Text = "Connect"
        Me.btConnect.UseVisualStyleBackColor = True
        '
        'lbLog
        '
        Me.lbLog.FormattingEnabled = True
        Me.lbLog.Location = New System.Drawing.Point(12, 201)
        Me.lbLog.Name = "lbLog"
        Me.lbLog.Size = New System.Drawing.Size(511, 212)
        Me.lbLog.TabIndex = 17
        '
        'lvDevices
        '
        Me.lvDevices.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chAddress, Me.chName})
        Me.lvDevices.FullRowSelect = True
        Me.lvDevices.GridLines = True
        Me.lvDevices.HideSelection = False
        Me.lvDevices.Location = New System.Drawing.Point(12, 41)
        Me.lvDevices.MultiSelect = False
        Me.lvDevices.Name = "lvDevices"
        Me.lvDevices.Size = New System.Drawing.Size(511, 97)
        Me.lvDevices.TabIndex = 10
        Me.lvDevices.UseCompatibleStateImageBehavior = False
        Me.lvDevices.View = System.Windows.Forms.View.Details
        '
        'chAddress
        '
        Me.chAddress.Text = "Address"
        Me.chAddress.Width = 220
        '
        'chName
        '
        Me.chName.Text = "Name"
        Me.chName.Width = 220
        '
        'btDiscover
        '
        Me.btDiscover.Location = New System.Drawing.Point(12, 12)
        Me.btDiscover.Name = "btDiscover"
        Me.btDiscover.Size = New System.Drawing.Size(75, 23)
        Me.btDiscover.TabIndex = 9
        Me.btDiscover.Text = "Discover"
        Me.btDiscover.UseVisualStyleBackColor = True
        '
        'btGetManu
        '
        Me.btGetManu.Location = New System.Drawing.Point(336, 144)
        Me.btGetManu.Name = "btGetManu"
        Me.btGetManu.Size = New System.Drawing.Size(75, 23)
        Me.btGetManu.TabIndex = 18
        Me.btGetManu.Text = "Get Manu"
        Me.btGetManu.UseVisualStyleBackColor = True
        '
        'btGetModel
        '
        Me.btGetModel.Location = New System.Drawing.Point(417, 144)
        Me.btGetModel.Name = "btGetModel"
        Me.btGetModel.Size = New System.Drawing.Size(75, 23)
        Me.btGetModel.TabIndex = 19
        Me.btGetModel.Text = "Get Model"
        Me.btGetModel.UseVisualStyleBackColor = True
        '
        'fmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(536, 422)
        Me.Controls.Add(Me.btGetModel)
        Me.Controls.Add(Me.btGetManu)
        Me.Controls.Add(Me.btCallNumber)
        Me.Controls.Add(Me.edNumber)
        Me.Controls.Add(Me.btAnswerCall)
        Me.Controls.Add(Me.btCancelCall)
        Me.Controls.Add(Me.btDisconnect)
        Me.Controls.Add(Me.btConnect)
        Me.Controls.Add(Me.lbLog)
        Me.Controls.Add(Me.lvDevices)
        Me.Controls.Add(Me.btDiscover)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "fmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Hands Free profile Demo"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents btCallNumber As System.Windows.Forms.Button
    Private WithEvents edNumber As System.Windows.Forms.TextBox
    Private WithEvents btAnswerCall As System.Windows.Forms.Button
    Private WithEvents btCancelCall As System.Windows.Forms.Button
    Private WithEvents btDisconnect As System.Windows.Forms.Button
    Private WithEvents btConnect As System.Windows.Forms.Button
    Private WithEvents lbLog As System.Windows.Forms.ListBox
    Private WithEvents lvDevices As System.Windows.Forms.ListView
    Private WithEvents chAddress As System.Windows.Forms.ColumnHeader
    Private WithEvents chName As System.Windows.Forms.ColumnHeader
    Private WithEvents btDiscover As System.Windows.Forms.Button
    Friend WithEvents btGetManu As System.Windows.Forms.Button
    Friend WithEvents btGetModel As System.Windows.Forms.Button

End Class
