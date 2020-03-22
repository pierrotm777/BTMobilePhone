<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Process
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Process))
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.lblStatusBar = New System.Windows.Forms.ToolStripStatusLabel
        Me.btnProcess = New System.Windows.Forms.Button
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.tbFile = New System.Windows.Forms.TextBox
        Me.Label37 = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.boxFixType = New System.Windows.Forms.ComboBox
        Me.boxSatCount = New System.Windows.Forms.ComboBox
        Me.tbMaxHDOP = New System.Windows.Forms.TextBox
        Me.tbMaxCorrAge = New System.Windows.Forms.TextBox
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatusBar})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 535)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(648, 22)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblStatusBar
        '
        Me.lblStatusBar.Name = "lblStatusBar"
        Me.lblStatusBar.Size = New System.Drawing.Size(69, 17)
        Me.lblStatusBar.Text = "lblStatusBar"
        '
        'btnProcess
        '
        Me.btnProcess.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnProcess.Location = New System.Drawing.Point(561, 215)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(75, 23)
        Me.btnProcess.TabIndex = 43
        Me.btnProcess.Text = "Process"
        Me.btnProcess.UseVisualStyleBackColor = True
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.Location = New System.Drawing.Point(561, 12)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 44
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'tbFile
        '
        Me.tbFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbFile.Location = New System.Drawing.Point(52, 12)
        Me.tbFile.Name = "tbFile"
        Me.tbFile.Size = New System.Drawing.Size(503, 23)
        Me.tbFile.TabIndex = 53
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(12, 15)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(34, 17)
        Me.Label37.TabIndex = 54
        Me.Label37.Text = "File:"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(12, 218)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(52, 17)
        Me.lblStatus.TabIndex = 55
        Me.lblStatus.Text = "Status:"
        Me.lblStatus.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 17)
        Me.Label1.TabIndex = 56
        Me.Label1.Text = "Limits:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(49, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 17)
        Me.Label2.TabIndex = 57
        Me.Label2.Text = "Fix Type:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(49, 131)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(114, 17)
        Me.Label3.TabIndex = 58
        Me.Label3.Text = "Maximum HDOP:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(49, 161)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(168, 17)
        Me.Label4.TabIndex = 59
        Me.Label4.Text = "Maximum Correction Age:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(49, 101)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(162, 17)
        Me.Label5.TabIndex = 60
        Me.Label5.Text = "Minimum Satellite Count:"
        '
        'boxFixType
        '
        Me.boxFixType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxFixType.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxFixType.FormattingEnabled = True
        Me.boxFixType.Items.AddRange(New Object() {"Any", "(2) DGPS", "(4) RTK"})
        Me.boxFixType.Location = New System.Drawing.Point(120, 68)
        Me.boxFixType.Name = "boxFixType"
        Me.boxFixType.Size = New System.Drawing.Size(123, 24)
        Me.boxFixType.TabIndex = 61
        '
        'boxSatCount
        '
        Me.boxSatCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxSatCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxSatCount.FormattingEnabled = True
        Me.boxSatCount.Items.AddRange(New Object() {"Any", "4", "5", "6", "7", "8", "9", "10"})
        Me.boxSatCount.Location = New System.Drawing.Point(217, 98)
        Me.boxSatCount.Name = "boxSatCount"
        Me.boxSatCount.Size = New System.Drawing.Size(123, 24)
        Me.boxSatCount.TabIndex = 62
        '
        'tbMaxHDOP
        '
        Me.tbMaxHDOP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbMaxHDOP.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbMaxHDOP.Location = New System.Drawing.Point(169, 128)
        Me.tbMaxHDOP.Name = "tbMaxHDOP"
        Me.tbMaxHDOP.Size = New System.Drawing.Size(60, 23)
        Me.tbMaxHDOP.TabIndex = 65
        Me.tbMaxHDOP.Text = "99"
        '
        'tbMaxCorrAge
        '
        Me.tbMaxCorrAge.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbMaxCorrAge.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbMaxCorrAge.Location = New System.Drawing.Point(223, 158)
        Me.tbMaxCorrAge.Name = "tbMaxCorrAge"
        Me.tbMaxCorrAge.Size = New System.Drawing.Size(60, 23)
        Me.tbMaxCorrAge.TabIndex = 66
        Me.tbMaxCorrAge.Text = "999"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(648, 557)
        Me.Controls.Add(Me.tbMaxCorrAge)
        Me.Controls.Add(Me.tbMaxHDOP)
        Me.Controls.Add(Me.boxSatCount)
        Me.Controls.Add(Me.boxFixType)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.Label37)
        Me.Controls.Add(Me.tbFile)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.btnProcess)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "Lefebure Process GPS"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatusBar As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnProcess As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents tbFile As System.Windows.Forms.TextBox
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents boxFixType As System.Windows.Forms.ComboBox
    Friend WithEvents boxSatCount As System.Windows.Forms.ComboBox
    Friend WithEvents tbMaxHDOP As System.Windows.Forms.TextBox
    Friend WithEvents tbMaxCorrAge As System.Windows.Forms.TextBox

End Class
