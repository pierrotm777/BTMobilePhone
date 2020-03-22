<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PhoneInterface
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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPagePhone = New System.Windows.Forms.TabPage()
        Me.TabPagePhoneBook = New System.Windows.Forms.TabPage()
        Me.btnCallWork = New System.Windows.Forms.Button()
        Me.btnCallHome = New System.Windows.Forms.Button()
        Me.btnCallMobile = New System.Windows.Forms.Button()
        Me.txtWorkPhone = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtHomePhone = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtMobilePhone = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lstContacts = New System.Windows.Forms.ListView()
        Me.TabPageRecentCalls = New System.Windows.Forms.TabPage()
        Me.TabPageDial = New System.Windows.Forms.TabPage()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.TabControl1.SuspendLayout()
        Me.TabPagePhone.SuspendLayout()
        Me.TabPagePhoneBook.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPagePhone)
        Me.TabControl1.Controls.Add(Me.TabPagePhoneBook)
        Me.TabControl1.Controls.Add(Me.TabPageRecentCalls)
        Me.TabControl1.Controls.Add(Me.TabPageDial)
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.Location = New System.Drawing.Point(2, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(750, 408)
        Me.TabControl1.TabIndex = 6
        '
        'TabPagePhone
        '
        Me.TabPagePhone.Controls.Add(Me.DataGridView1)
        Me.TabPagePhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPagePhone.Location = New System.Drawing.Point(4, 29)
        Me.TabPagePhone.Name = "TabPagePhone"
        Me.TabPagePhone.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPagePhone.Size = New System.Drawing.Size(742, 375)
        Me.TabPagePhone.TabIndex = 0
        Me.TabPagePhone.Text = "Phone"
        Me.TabPagePhone.UseVisualStyleBackColor = True
        '
        'TabPagePhoneBook
        '
        Me.TabPagePhoneBook.Controls.Add(Me.btnCallWork)
        Me.TabPagePhoneBook.Controls.Add(Me.btnCallHome)
        Me.TabPagePhoneBook.Controls.Add(Me.btnCallMobile)
        Me.TabPagePhoneBook.Controls.Add(Me.txtWorkPhone)
        Me.TabPagePhoneBook.Controls.Add(Me.Label3)
        Me.TabPagePhoneBook.Controls.Add(Me.txtHomePhone)
        Me.TabPagePhoneBook.Controls.Add(Me.Label2)
        Me.TabPagePhoneBook.Controls.Add(Me.txtMobilePhone)
        Me.TabPagePhoneBook.Controls.Add(Me.Label1)
        Me.TabPagePhoneBook.Controls.Add(Me.lstContacts)
        Me.TabPagePhoneBook.Location = New System.Drawing.Point(4, 29)
        Me.TabPagePhoneBook.Name = "TabPagePhoneBook"
        Me.TabPagePhoneBook.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPagePhoneBook.Size = New System.Drawing.Size(742, 375)
        Me.TabPagePhoneBook.TabIndex = 1
        Me.TabPagePhoneBook.Text = "PhoneBook"
        Me.TabPagePhoneBook.UseVisualStyleBackColor = True
        '
        'btnCallWork
        '
        Me.btnCallWork.Location = New System.Drawing.Point(661, 196)
        Me.btnCallWork.Name = "btnCallWork"
        Me.btnCallWork.Size = New System.Drawing.Size(75, 42)
        Me.btnCallWork.TabIndex = 9
        Me.btnCallWork.Text = "Work"
        Me.btnCallWork.UseVisualStyleBackColor = True
        '
        'btnCallHome
        '
        Me.btnCallHome.Location = New System.Drawing.Point(661, 123)
        Me.btnCallHome.Name = "btnCallHome"
        Me.btnCallHome.Size = New System.Drawing.Size(75, 42)
        Me.btnCallHome.TabIndex = 8
        Me.btnCallHome.Text = "Home"
        Me.btnCallHome.UseVisualStyleBackColor = True
        '
        'btnCallMobile
        '
        Me.btnCallMobile.Location = New System.Drawing.Point(661, 51)
        Me.btnCallMobile.Name = "btnCallMobile"
        Me.btnCallMobile.Size = New System.Drawing.Size(75, 42)
        Me.btnCallMobile.TabIndex = 7
        Me.btnCallMobile.Text = "Mobile"
        Me.btnCallMobile.UseVisualStyleBackColor = True
        '
        'txtWorkPhone
        '
        Me.txtWorkPhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWorkPhone.Location = New System.Drawing.Point(460, 204)
        Me.txtWorkPhone.Name = "txtWorkPhone"
        Me.txtWorkPhone.Size = New System.Drawing.Size(179, 26)
        Me.txtWorkPhone.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(333, 207)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 20)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Work Phone:"
        '
        'txtHomePhone
        '
        Me.txtHomePhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHomePhone.Location = New System.Drawing.Point(460, 131)
        Me.txtHomePhone.Name = "txtHomePhone"
        Me.txtHomePhone.Size = New System.Drawing.Size(179, 26)
        Me.txtHomePhone.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(333, 134)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(106, 20)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Home Phone:"
        '
        'txtMobilePhone
        '
        Me.txtMobilePhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMobilePhone.Location = New System.Drawing.Point(460, 59)
        Me.txtMobilePhone.Name = "txtMobilePhone"
        Me.txtMobilePhone.Size = New System.Drawing.Size(179, 26)
        Me.txtMobilePhone.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(333, 62)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Mobile Phone:"
        '
        'lstContacts
        '
        Me.lstContacts.Location = New System.Drawing.Point(7, 28)
        Me.lstContacts.Name = "lstContacts"
        Me.lstContacts.Size = New System.Drawing.Size(311, 331)
        Me.lstContacts.TabIndex = 0
        Me.lstContacts.UseCompatibleStateImageBehavior = False
        '
        'TabPageRecentCalls
        '
        Me.TabPageRecentCalls.Location = New System.Drawing.Point(4, 29)
        Me.TabPageRecentCalls.Name = "TabPageRecentCalls"
        Me.TabPageRecentCalls.Size = New System.Drawing.Size(742, 375)
        Me.TabPageRecentCalls.TabIndex = 2
        Me.TabPageRecentCalls.Text = "Recent Calls"
        Me.TabPageRecentCalls.UseVisualStyleBackColor = True
        '
        'TabPageDial
        '
        Me.TabPageDial.Location = New System.Drawing.Point(4, 29)
        Me.TabPageDial.Name = "TabPageDial"
        Me.TabPageDial.Size = New System.Drawing.Size(742, 375)
        Me.TabPageDial.TabIndex = 3
        Me.TabPageDial.Text = "Dial"
        Me.TabPageDial.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(667, 426)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 42)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "Quit"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(3, 3)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(733, 366)
        Me.DataGridView1.TabIndex = 2
        '
        'PhoneInterface
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(757, 480)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "PhoneInterface"
        Me.Text = "PhoneForm"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPagePhone.ResumeLayout(False)
        Me.TabPagePhoneBook.ResumeLayout(False)
        Me.TabPagePhoneBook.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPagePhone As System.Windows.Forms.TabPage
    Friend WithEvents TabPagePhoneBook As System.Windows.Forms.TabPage
    Friend WithEvents txtWorkPhone As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtHomePhone As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMobilePhone As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lstContacts As System.Windows.Forms.ListView
    Friend WithEvents TabPageRecentCalls As System.Windows.Forms.TabPage
    Friend WithEvents TabPageDial As System.Windows.Forms.TabPage
    Friend WithEvents btnCallWork As System.Windows.Forms.Button
    Friend WithEvents btnCallHome As System.Windows.Forms.Button
    Friend WithEvents btnCallMobile As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
End Class
