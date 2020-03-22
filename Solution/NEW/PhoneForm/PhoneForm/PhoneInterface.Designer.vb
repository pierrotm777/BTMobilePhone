
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PhoneInterface
    Inherits Telerik.WinControls.UI.RadForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PhoneInterface))
        Me.HighContrastBlackTheme1 = New Telerik.WinControls.Themes.HighContrastBlackTheme()
        Me.TelerikMetroTouchTheme1 = New Telerik.WinControls.Themes.TelerikMetroTouchTheme()
        Me.Windows7Theme1 = New Telerik.WinControls.Themes.Windows7Theme()
        Me.VisualStudio2012DarkTheme1 = New Telerik.WinControls.Themes.VisualStudio2012DarkTheme()
        Me.RadPageView1 = New Telerik.WinControls.UI.RadPageView()
        Me.RadPageViewPage4 = New Telerik.WinControls.UI.RadPageViewPage()
        Me.RadPageViewPage1 = New Telerik.WinControls.UI.RadPageViewPage()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.btnCallWork = New System.Windows.Forms.Button()
        Me.btnCallHome = New System.Windows.Forms.Button()
        Me.btnCallMobile = New System.Windows.Forms.Button()
        Me.lblWork = New Telerik.WinControls.UI.RadLabel()
        Me.lblHome = New Telerik.WinControls.UI.RadLabel()
        Me.lblMobile = New Telerik.WinControls.UI.RadLabel()
        Me.txtWorkPhone = New Telerik.WinControls.UI.RadTextBox()
        Me.txtHomePhone = New Telerik.WinControls.UI.RadTextBox()
        Me.txtMobilePhone = New Telerik.WinControls.UI.RadTextBox()
        Me.lstContacts = New Telerik.WinControls.UI.RadListView()
        Me.txtSearch = New Telerik.WinControls.UI.RadTextBox()
        Me.RadPageViewPage2 = New Telerik.WinControls.UI.RadPageViewPage()
        Me.RadPageViewPage3 = New Telerik.WinControls.UI.RadPageViewPage()
        Me.RadThemeManager1 = New Telerik.WinControls.RadThemeManager()
        CType(Me.RadPageView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPageView1.SuspendLayout()
        Me.RadPageViewPage1.SuspendLayout()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.lblWork, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblHome, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblMobile, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtWorkPhone, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHomePhone, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMobilePhone, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lstContacts, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPageView1
        '
        Me.RadPageView1.Controls.Add(Me.RadPageViewPage4)
        Me.RadPageView1.Controls.Add(Me.RadPageViewPage1)
        Me.RadPageView1.Controls.Add(Me.RadPageViewPage2)
        Me.RadPageView1.Controls.Add(Me.RadPageViewPage3)
        Me.RadPageView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPageView1.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadPageView1.ForeColor = System.Drawing.Color.Gainsboro
        Me.RadPageView1.ItemSizeMode = CType((Telerik.WinControls.UI.PageViewItemSizeMode.EqualWidth Or Telerik.WinControls.UI.PageViewItemSizeMode.EqualHeight), Telerik.WinControls.UI.PageViewItemSizeMode)
        Me.RadPageView1.Location = New System.Drawing.Point(0, 0)
        Me.RadPageView1.Name = "RadPageView1"
        Me.RadPageView1.SelectedPage = Me.RadPageViewPage3
        Me.RadPageView1.Size = New System.Drawing.Size(1358, 523)
        Me.RadPageView1.TabIndex = 0
        Me.RadPageView1.Text = "Recent Calls"
        Me.RadPageView1.ThemeName = "VisualStudio2012Dark"
        Me.RadPageView1.ViewMode = Telerik.WinControls.UI.PageViewMode.Stack
        CType(Me.RadPageView1.GetChildAt(0), Telerik.WinControls.UI.RadPageViewStackElement).StackPosition = Telerik.WinControls.UI.StackViewPosition.Bottom
        CType(Me.RadPageView1.GetChildAt(0), Telerik.WinControls.UI.RadPageViewStackElement).ItemSizeMode = CType((Telerik.WinControls.UI.PageViewItemSizeMode.EqualWidth Or Telerik.WinControls.UI.PageViewItemSizeMode.EqualHeight), Telerik.WinControls.UI.PageViewItemSizeMode)
        CType(Me.RadPageView1.GetChildAt(0), Telerik.WinControls.UI.RadPageViewStackElement).ItemContentOrientation = Telerik.WinControls.UI.PageViewContentOrientation.Horizontal
        CType(Me.RadPageView1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.RadPageViewLabelElement).Text = "Dial"
        CType(Me.RadPageView1.GetChildAt(0).GetChildAt(2), Telerik.WinControls.UI.RadPageViewLabelElement).Text = "Dial"
        '
        'RadPageViewPage4
        '
        Me.RadPageViewPage4.ItemSize = New System.Drawing.SizeF(1358.0!, 36.0!)
        Me.RadPageViewPage4.Location = New System.Drawing.Point(4, 28)
        Me.RadPageViewPage4.Name = "RadPageViewPage4"
        Me.RadPageViewPage4.Size = New System.Drawing.Size(1350, 346)
        Me.RadPageViewPage4.Text = "Phone"
        '
        'RadPageViewPage1
        '
        Me.RadPageViewPage1.Controls.Add(Me.RadPanel1)
        Me.RadPageViewPage1.Controls.Add(Me.lstContacts)
        Me.RadPageViewPage1.Controls.Add(Me.txtSearch)
        Me.RadPageViewPage1.ItemSize = New System.Drawing.SizeF(1358.0!, 36.0!)
        Me.RadPageViewPage1.Location = New System.Drawing.Point(4, 28)
        Me.RadPageViewPage1.Name = "RadPageViewPage1"
        Me.RadPageViewPage1.Size = New System.Drawing.Size(1350, 346)
        Me.RadPageViewPage1.Text = "Phone Book"
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.btnCallWork)
        Me.RadPanel1.Controls.Add(Me.btnCallHome)
        Me.RadPanel1.Controls.Add(Me.btnCallMobile)
        Me.RadPanel1.Controls.Add(Me.lblWork)
        Me.RadPanel1.Controls.Add(Me.lblHome)
        Me.RadPanel1.Controls.Add(Me.lblMobile)
        Me.RadPanel1.Controls.Add(Me.txtWorkPhone)
        Me.RadPanel1.Controls.Add(Me.txtHomePhone)
        Me.RadPanel1.Controls.Add(Me.txtMobilePhone)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel1.Location = New System.Drawing.Point(481, 0)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(869, 346)
        Me.RadPanel1.TabIndex = 3
        Me.RadPanel1.ThemeName = "Windows7"
        '
        'btnCallWork
        '
        Me.btnCallWork.FlatAppearance.BorderSize = 0
        Me.btnCallWork.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCallWork.Image = CType(resources.GetObject("btnCallWork.Image"), System.Drawing.Image)
        Me.btnCallWork.Location = New System.Drawing.Point(667, 225)
        Me.btnCallWork.Margin = New System.Windows.Forms.Padding(8)
        Me.btnCallWork.Name = "btnCallWork"
        Me.btnCallWork.Size = New System.Drawing.Size(70, 70)
        Me.btnCallWork.TabIndex = 6
        Me.btnCallWork.UseVisualStyleBackColor = True
        '
        'btnCallHome
        '
        Me.btnCallHome.FlatAppearance.BorderSize = 0
        Me.btnCallHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCallHome.Image = CType(resources.GetObject("btnCallHome.Image"), System.Drawing.Image)
        Me.btnCallHome.Location = New System.Drawing.Point(667, 125)
        Me.btnCallHome.Margin = New System.Windows.Forms.Padding(8)
        Me.btnCallHome.Name = "btnCallHome"
        Me.btnCallHome.Size = New System.Drawing.Size(70, 70)
        Me.btnCallHome.TabIndex = 5
        Me.btnCallHome.UseVisualStyleBackColor = True
        '
        'btnCallMobile
        '
        Me.btnCallMobile.FlatAppearance.BorderSize = 0
        Me.btnCallMobile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCallMobile.Image = CType(resources.GetObject("btnCallMobile.Image"), System.Drawing.Image)
        Me.btnCallMobile.Location = New System.Drawing.Point(667, 28)
        Me.btnCallMobile.Margin = New System.Windows.Forms.Padding(8)
        Me.btnCallMobile.Name = "btnCallMobile"
        Me.btnCallMobile.Size = New System.Drawing.Size(70, 70)
        Me.btnCallMobile.TabIndex = 4
        Me.btnCallMobile.UseVisualStyleBackColor = True
        '
        'lblWork
        '
        Me.lblWork.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblWork.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWork.Location = New System.Drawing.Point(76, 242)
        Me.lblWork.Name = "lblWork"
        Me.lblWork.Size = New System.Drawing.Size(176, 41)
        Me.lblWork.TabIndex = 3
        Me.lblWork.Text = "Work Phone :"
        Me.lblWork.ThemeName = "VisualStudio2012Dark"
        '
        'lblHome
        '
        Me.lblHome.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblHome.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHome.Location = New System.Drawing.Point(76, 144)
        Me.lblHome.Name = "lblHome"
        Me.lblHome.Size = New System.Drawing.Size(185, 41)
        Me.lblHome.TabIndex = 3
        Me.lblHome.Text = "Home Phone :"
        Me.lblHome.ThemeName = "VisualStudio2012Dark"
        '
        'lblMobile
        '
        Me.lblMobile.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblMobile.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMobile.Location = New System.Drawing.Point(76, 48)
        Me.lblMobile.Name = "lblMobile"
        Me.lblMobile.Size = New System.Drawing.Size(196, 41)
        Me.lblMobile.TabIndex = 2
        Me.lblMobile.Text = "Mobile Phone :"
        Me.lblMobile.ThemeName = "VisualStudio2012Dark"
        '
        'txtWorkPhone
        '
        Me.txtWorkPhone.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtWorkPhone.Font = New System.Drawing.Font("Segoe UI Semibold", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWorkPhone.Location = New System.Drawing.Point(288, 242)
        Me.txtWorkPhone.Margin = New System.Windows.Forms.Padding(8)
        Me.txtWorkPhone.Name = "txtWorkPhone"
        Me.txtWorkPhone.Size = New System.Drawing.Size(300, 43)
        Me.txtWorkPhone.TabIndex = 1
        Me.txtWorkPhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtWorkPhone.ThemeName = "Windows7"
        '
        'txtHomePhone
        '
        Me.txtHomePhone.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtHomePhone.Font = New System.Drawing.Font("Segoe UI Semibold", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHomePhone.Location = New System.Drawing.Point(288, 142)
        Me.txtHomePhone.Margin = New System.Windows.Forms.Padding(8)
        Me.txtHomePhone.Name = "txtHomePhone"
        Me.txtHomePhone.ReadOnly = True
        Me.txtHomePhone.Size = New System.Drawing.Size(300, 43)
        Me.txtHomePhone.TabIndex = 1
        Me.txtHomePhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtHomePhone.ThemeName = "Windows7"
        '
        'txtMobilePhone
        '
        Me.txtMobilePhone.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtMobilePhone.Font = New System.Drawing.Font("Segoe UI Semibold", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMobilePhone.Location = New System.Drawing.Point(288, 45)
        Me.txtMobilePhone.Margin = New System.Windows.Forms.Padding(8)
        Me.txtMobilePhone.Name = "txtMobilePhone"
        Me.txtMobilePhone.ReadOnly = True
        Me.txtMobilePhone.Size = New System.Drawing.Size(300, 43)
        Me.txtMobilePhone.TabIndex = 0
        Me.txtMobilePhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtMobilePhone.ThemeName = "Windows7"
        '
        'lstContacts
        '
        Me.lstContacts.Dock = System.Windows.Forms.DockStyle.Left
        Me.lstContacts.Font = New System.Drawing.Font("Segoe UI Semibold", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstContacts.Location = New System.Drawing.Point(0, 0)
        Me.lstContacts.Name = "lstContacts"
        Me.lstContacts.Size = New System.Drawing.Size(481, 346)
        Me.lstContacts.TabIndex = 0
        Me.lstContacts.ThemeName = "HighContrastBlack"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Location = New System.Drawing.Point(804, 308)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(200, 20)
        Me.txtSearch.TabIndex = 2
        '
        'RadPageViewPage2
        '
        Me.RadPageViewPage2.ItemSize = New System.Drawing.SizeF(1358.0!, 36.0!)
        Me.RadPageViewPage2.Location = New System.Drawing.Point(4, 28)
        Me.RadPageViewPage2.Name = "RadPageViewPage2"
        Me.RadPageViewPage2.Size = New System.Drawing.Size(1350, 346)
        Me.RadPageViewPage2.Text = "Recent Calls"
        '
        'RadPageViewPage3
        '
        Me.RadPageViewPage3.ItemSize = New System.Drawing.SizeF(1358.0!, 36.0!)
        Me.RadPageViewPage3.Location = New System.Drawing.Point(4, 28)
        Me.RadPageViewPage3.Name = "RadPageViewPage3"
        Me.RadPageViewPage3.Size = New System.Drawing.Size(1350, 346)
        Me.RadPageViewPage3.Text = "Dial"
        '
        'PhoneInterface
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1358, 523)
        Me.ControlBox = False
        Me.Controls.Add(Me.RadPageView1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PhoneInterface"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.Text = ""
        Me.ThemeName = "VisualStudio2012Dark"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.RadPageView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPageView1.ResumeLayout(False)
        Me.RadPageViewPage1.ResumeLayout(False)
        Me.RadPageViewPage1.PerformLayout()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me.lblWork, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblHome, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblMobile, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtWorkPhone, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHomePhone, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMobilePhone, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lstContacts, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HighContrastBlackTheme1 As Telerik.WinControls.Themes.HighContrastBlackTheme
    Friend WithEvents TelerikMetroTouchTheme1 As Telerik.WinControls.Themes.TelerikMetroTouchTheme
    Friend WithEvents Windows7Theme1 As Telerik.WinControls.Themes.Windows7Theme
    Friend WithEvents VisualStudio2012DarkTheme1 As Telerik.WinControls.Themes.VisualStudio2012DarkTheme
    Friend WithEvents RadPageView1 As Telerik.WinControls.UI.RadPageView
    Friend WithEvents RadPageViewPage1 As Telerik.WinControls.UI.RadPageViewPage
    Friend WithEvents RadPageViewPage2 As Telerik.WinControls.UI.RadPageViewPage
    Friend WithEvents RadPageViewPage3 As Telerik.WinControls.UI.RadPageViewPage
    Friend WithEvents RadPageViewPage4 As Telerik.WinControls.UI.RadPageViewPage
    Friend WithEvents lstContacts As Telerik.WinControls.UI.RadListView
    Friend WithEvents RadThemeManager1 As Telerik.WinControls.RadThemeManager
    Friend WithEvents txtSearch As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents txtMobilePhone As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents txtWorkPhone As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents txtHomePhone As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents lblWork As Telerik.WinControls.UI.RadLabel
    Friend WithEvents lblHome As Telerik.WinControls.UI.RadLabel
    Friend WithEvents lblMobile As Telerik.WinControls.UI.RadLabel
    Friend WithEvents btnCallMobile As System.Windows.Forms.Button
    Friend WithEvents btnCallWork As System.Windows.Forms.Button
    Friend WithEvents btnCallHome As System.Windows.Forms.Button
End Class

