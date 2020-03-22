Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Diagnostics

''' <summary>
''' Summary description for Form1.
''' </summary>
Public Class DemoForm
    Inherits System.Windows.Forms.Form
    Private tabControl1 As System.Windows.Forms.TabControl
    Private tabPage1 As System.Windows.Forms.TabPage
    Private tabPage2 As System.Windows.Forms.TabPage
    Private tabPage3 As System.Windows.Forms.TabPage
    Private cbShowColorName As System.Windows.Forms.CheckBox
    Private label8 As System.Windows.Forms.Label
    Private chkContinuous As System.Windows.Forms.CheckBox
    Private groupBox1 As System.Windows.Forms.GroupBox
    Private colorPicker As PJLControls.ColorPicker
    Private colorPickerWeb As PJLControls.ColorPicker
    Private colorPickerSystem As PJLControls.ColorPicker
    Private groupBox2 As System.Windows.Forms.GroupBox
    Private label9 As System.Windows.Forms.Label
    Private cbShowPickColor As System.Windows.Forms.CheckBox
    Private labelWeb As System.Windows.Forms.Label
    Private labelSystem As System.Windows.Forms.Label
    Private labelPanel As System.Windows.Forms.Label
    Private colorPanel As PJLControls.ColorPanel
    Private labelCustomName As System.Windows.Forms.Label
    Private labelCustomColor As System.Windows.Forms.Label
    Private customColorPicker As PJLControls.CustomColorPicker
    Private cbEnableSystemColors As System.Windows.Forms.CheckBox
    Private cbEnableCustom As System.Windows.Forms.CheckBox
    Private cbEnablePanel As System.Windows.Forms.CheckBox
    Private tabPage4 As System.Windows.Forms.TabPage
    Private customColorPanel1 As PJLControls.CustomColorPanel
    Private label1 As System.Windows.Forms.Label
    Private dudSortColorsWeb As System.Windows.Forms.DomainUpDown
    Private dudSortColorsPanel As System.Windows.Forms.DomainUpDown
    Private dudColorSetPanel As System.Windows.Forms.DomainUpDown
    Private label2 As System.Windows.Forms.Label
    Private dudColorSetOther As System.Windows.Forms.DomainUpDown
    Private dudZ As System.Windows.Forms.DomainUpDown
    Private components As System.ComponentModel.IContainer

    Public Sub New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        labelPanel.BackColor = colorPanel.Color
        labelPanel.Text = colorPanel.Color.Name

        cbEnablePanel.Checked = colorPanel.Enabled
        cbEnableCustom.Checked = customColorPicker.Enabled
    End Sub

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"
    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.dudSortColorsWeb = New System.Windows.Forms.DomainUpDown()
        Me.cbEnableCustom = New System.Windows.Forms.CheckBox()
        Me.dudSortColorsPanel = New System.Windows.Forms.DomainUpDown()
        Me.labelCustomColor = New System.Windows.Forms.Label()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.cbEnableSystemColors = New System.Windows.Forms.CheckBox()
        Me.groupBox2 = New System.Windows.Forms.GroupBox()
        Me.labelWeb = New System.Windows.Forms.Label()
        Me.cbShowPickColor = New System.Windows.Forms.CheckBox()
        Me.label9 = New System.Windows.Forms.Label()
        Me.dudZ = New System.Windows.Forms.DomainUpDown()
        Me.colorPicker = New PJLControls.ColorPicker()
        Me.customColorPanel1 = New PJLControls.CustomColorPanel()
        Me.tabPage2 = New System.Windows.Forms.TabPage()
        Me.label2 = New System.Windows.Forms.Label()
        Me.dudColorSetPanel = New System.Windows.Forms.DomainUpDown()
        Me.cbEnablePanel = New System.Windows.Forms.CheckBox()
        Me.label8 = New System.Windows.Forms.Label()
        Me.labelPanel = New System.Windows.Forms.Label()
        Me.colorPanel = New PJLControls.ColorPanel()
        Me.tabPage3 = New System.Windows.Forms.TabPage()
        Me.chkContinuous = New System.Windows.Forms.CheckBox()
        Me.labelCustomName = New System.Windows.Forms.Label()
        Me.customColorPicker = New PJLControls.CustomColorPicker()
        Me.tabPage1 = New System.Windows.Forms.TabPage()
        Me.dudColorSetOther = New System.Windows.Forms.DomainUpDown()
        Me.labelSystem = New System.Windows.Forms.Label()
        Me.colorPickerWeb = New PJLControls.ColorPicker()
        Me.colorPickerSystem = New PJLControls.ColorPicker()
        Me.cbShowColorName = New System.Windows.Forms.CheckBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.tabPage4 = New System.Windows.Forms.TabPage()
        Me.tabControl1 = New System.Windows.Forms.TabControl()
        Me.groupBox1.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.tabPage2.SuspendLayout()
        Me.tabPage3.SuspendLayout()
        Me.tabPage1.SuspendLayout()
        Me.tabPage4.SuspendLayout()
        Me.tabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dudSortColorsWeb
        '
        Me.dudSortColorsWeb.Location = New System.Drawing.Point(200, 80)
        Me.dudSortColorsWeb.Name = "dudSortColorsWeb"
        Me.dudSortColorsWeb.ReadOnly = True
        Me.dudSortColorsWeb.Size = New System.Drawing.Size(120, 20)
        Me.dudSortColorsWeb.TabIndex = 7
        AddHandler Me.dudSortColorsWeb.SelectedItemChanged, AddressOf Me.dudSortColorsWeb_SelectedItemChanged
        '
        'cbEnableCustom
        '
        Me.cbEnableCustom.Checked = True
        Me.cbEnableCustom.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbEnableCustom.Location = New System.Drawing.Point(296, 312)
        Me.cbEnableCustom.Name = "cbEnableCustom"
        Me.cbEnableCustom.Size = New System.Drawing.Size(104, 24)
        Me.cbEnableCustom.TabIndex = 4
        Me.cbEnableCustom.Text = "Enable"
        AddHandler Me.cbEnableCustom.CheckedChanged, AddressOf Me.cbEnableCustom_CheckedChanged
        '
        'dudSortColorsPanel
        '
        Me.dudSortColorsPanel.Location = New System.Drawing.Point(272, 56)
        Me.dudSortColorsPanel.Name = "dudSortColorsPanel"
        Me.dudSortColorsPanel.ReadOnly = True
        Me.dudSortColorsPanel.Size = New System.Drawing.Size(120, 20)
        Me.dudSortColorsPanel.TabIndex = 19
        AddHandler Me.dudSortColorsPanel.SelectedItemChanged, AddressOf Me.dudSortColorsPanel_SelectedItemChanged
        '
        'labelCustomColor
        '
        Me.labelCustomColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.labelCustomColor.Location = New System.Drawing.Point(16, 288)
        Me.labelCustomColor.Name = "labelCustomColor"
        Me.labelCustomColor.Size = New System.Drawing.Size(256, 32)
        Me.labelCustomColor.TabIndex = 1
        Me.labelCustomColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.cbEnableSystemColors)
        Me.groupBox1.Location = New System.Drawing.Point(8, 136)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(336, 104)
        Me.groupBox1.TabIndex = 8
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "System Colors"
        '
        'cbEnableSystemColors
        '
        Me.cbEnableSystemColors.Location = New System.Drawing.Point(256, 24)
        Me.cbEnableSystemColors.Name = "cbEnableSystemColors"
        Me.cbEnableSystemColors.Size = New System.Drawing.Size(72, 24)
        Me.cbEnableSystemColors.TabIndex = 10
        Me.cbEnableSystemColors.Text = "Enable"
        AddHandler Me.cbEnableSystemColors.CheckedChanged, AddressOf Me.cbEnableSystemColors_CheckedChanged
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.dudSortColorsWeb)
        Me.groupBox2.Controls.Add(Me.labelWeb)
        Me.groupBox2.Controls.Add(Me.cbShowPickColor)
        Me.groupBox2.Controls.Add(Me.label9)
        Me.groupBox2.Location = New System.Drawing.Point(8, 16)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(336, 112)
        Me.groupBox2.TabIndex = 7
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Web Colors"
        '
        'labelWeb
        '
        Me.labelWeb.Location = New System.Drawing.Point(8, 48)
        Me.labelWeb.Name = "labelWeb"
        Me.labelWeb.Size = New System.Drawing.Size(184, 56)
        Me.labelWeb.TabIndex = 6
        Me.labelWeb.Text = "labelWeb"
        Me.labelWeb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbShowPickColor
        '
        Me.cbShowPickColor.Checked = True
        Me.cbShowPickColor.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbShowPickColor.Location = New System.Drawing.Point(200, 16)
        Me.cbShowPickColor.Name = "cbShowPickColor"
        Me.cbShowPickColor.Size = New System.Drawing.Size(128, 24)
        Me.cbShowPickColor.TabIndex = 2
        Me.cbShowPickColor.Text = "Show Pick Color"
        AddHandler Me.cbShowPickColor.CheckedChanged, AddressOf Me.cbShowPickColor_CheckedChanged
        '
        'label9
        '
        Me.label9.Location = New System.Drawing.Point(200, 64)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(96, 16)
        Me.label9.TabIndex = 4
        Me.label9.Text = "Sort Colors By"
        '
        'dudZ
        '
        Me.dudZ.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dudZ.Location = New System.Drawing.Point(312, 32)
        Me.dudZ.Name = "dudZ"
        Me.dudZ.ReadOnly = True
        Me.dudZ.Size = New System.Drawing.Size(120, 20)
        Me.dudZ.TabIndex = 2
        AddHandler Me.dudZ.SelectedItemChanged, AddressOf Me.dudZ_SelectedItemChanged
        '
        'colorPicker
        '
        Me.colorPicker._Text = "Pick another color"
        Me.colorPicker.AutoSize = False
        Me.colorPicker.ColorSortOrder = PJLControls.ColorSortOrder.Brightness
        Me.colorPicker.ColorWellSize = New System.Drawing.Size(24, 12)
        Me.colorPicker.CustomColors = New System.Drawing.Color() {System.Drawing.Color.Tomato, System.Drawing.Color.Firebrick, System.Drawing.Color.LightCoral, System.Drawing.Color.Maroon, System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.DarkRed, System.Drawing.Color.DarkSalmon, System.Drawing.Color.Coral, System.Drawing.Color.OrangeRed}
        Me.colorPicker.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.colorPicker.ForeColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.colorPicker.Location = New System.Drawing.Point(8, 248)
        Me.colorPicker.Name = "colorPicker"
        Me.colorPicker.Size = New System.Drawing.Size(232, 40)
        Me.colorPicker.TabIndex = 12
        '
        'customColorPanel1
        '
        Me.customColorPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.customColorPanel1.AutoSize = True
        Me.customColorPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.customColorPanel1.Color = System.Drawing.Color.Empty
        Me.customColorPanel1.Isotropic = True
        Me.customColorPanel1.Location = New System.Drawing.Point(16, 32)
        Me.customColorPanel1.Name = "customColorPanel1"
        Me.customColorPanel1.Size = New System.Drawing.Size(280, 226)
        Me.customColorPanel1.TabIndex = 0
        Me.customColorPanel1.ZAxis = PJLControls.ZAxis.red
        '
        'tabPage2
        '
        Me.tabPage2.Controls.Add(Me.label2)
        Me.tabPage2.Controls.Add(Me.dudColorSetPanel)
        Me.tabPage2.Controls.Add(Me.dudSortColorsPanel)
        Me.tabPage2.Controls.Add(Me.cbEnablePanel)
        Me.tabPage2.Controls.Add(Me.label8)
        Me.tabPage2.Controls.Add(Me.labelPanel)
        Me.tabPage2.Controls.Add(Me.colorPanel)
        Me.tabPage2.Location = New System.Drawing.Point(4, 22)
        Me.tabPage2.Name = "tabPage2"
        Me.tabPage2.Size = New System.Drawing.Size(464, 339)
        Me.tabPage2.TabIndex = 1
        Me.tabPage2.Text = "Panel"
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(176, 88)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(72, 23)
        Me.label2.TabIndex = 21
        Me.label2.Text = "Color Set"
        '
        'dudColorSetPanel
        '
        Me.dudColorSetPanel.Location = New System.Drawing.Point(272, 88)
        Me.dudColorSetPanel.Name = "dudColorSetPanel"
        Me.dudColorSetPanel.ReadOnly = True
        Me.dudColorSetPanel.Size = New System.Drawing.Size(120, 20)
        Me.dudColorSetPanel.TabIndex = 20
        AddHandler Me.dudColorSetPanel.SelectedItemChanged, AddressOf Me.dudColorSetPanel_SelectedItemChanged
        '
        'cbEnablePanel
        '
        Me.cbEnablePanel.Location = New System.Drawing.Point(176, 16)
        Me.cbEnablePanel.Name = "cbEnablePanel"
        Me.cbEnablePanel.Size = New System.Drawing.Size(104, 24)
        Me.cbEnablePanel.TabIndex = 15
        Me.cbEnablePanel.Text = "Enable Panel"
        AddHandler Me.cbEnablePanel.CheckedChanged, AddressOf Me.cbEnablePanel_CheckedChanged
        '
        'label8
        '
        Me.label8.Location = New System.Drawing.Point(176, 56)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(96, 16)
        Me.label8.TabIndex = 16
        Me.label8.Text = "Sort Colors By"
        '
        'labelPanel
        '
        Me.labelPanel.Location = New System.Drawing.Point(176, 144)
        Me.labelPanel.Name = "labelPanel"
        Me.labelPanel.Size = New System.Drawing.Size(216, 88)
        Me.labelPanel.TabIndex = 18
        Me.labelPanel.Text = "label1"
        Me.labelPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'colorPanel
        '
        Me.colorPanel.ColorSortOrder = PJLControls.ColorSortOrder.Brightness
        Me.colorPanel.CustomColors = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer)), System.Drawing.Color.Red, System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer)), System.Drawing.Color.Maroon, System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.Aqua, System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.Teal, System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.Magenta, System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.Purple, System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer)), System.Drawing.Color.Yellow, System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer)), System.Drawing.Color.Olive}
        Me.colorPanel.Location = New System.Drawing.Point(0, 8)
        Me.colorPanel.Name = "colorPanel"
        Me.colorPanel.Size = New System.Drawing.Size(148, 260)
        Me.colorPanel.TabIndex = 13
        AddHandler Me.colorPanel.ColorChanged, AddressOf Me.colorPanel_ColorChanged
        '
        'tabPage3
        '
        Me.tabPage3.Controls.Add(Me.cbEnableCustom)
        Me.tabPage3.Controls.Add(Me.chkContinuous)
        Me.tabPage3.Controls.Add(Me.labelCustomName)
        Me.tabPage3.Controls.Add(Me.labelCustomColor)
        Me.tabPage3.Controls.Add(Me.customColorPicker)
        Me.tabPage3.Location = New System.Drawing.Point(4, 22)
        Me.tabPage3.Name = "tabPage3"
        Me.tabPage3.Size = New System.Drawing.Size(464, 339)
        Me.tabPage3.TabIndex = 2
        Me.tabPage3.Text = "Custom"
        '
        'chkContinuous
        '
        Me.chkContinuous.Checked = True
        Me.chkContinuous.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkContinuous.Location = New System.Drawing.Point(296, 288)
        Me.chkContinuous.Name = "chkContinuous"
        Me.chkContinuous.Size = New System.Drawing.Size(144, 24)
        Me.chkContinuous.TabIndex = 3
        Me.chkContinuous.Text = "Continous Scroll Z Axis"
        AddHandler Me.chkContinuous.CheckedChanged, AddressOf Me.chkContinuous_CheckedChanged
        '
        'labelCustomName
        '
        Me.labelCustomName.BackColor = System.Drawing.Color.White
        Me.labelCustomName.Location = New System.Drawing.Point(108, 296)
        Me.labelCustomName.Name = "labelCustomName"
        Me.labelCustomName.Size = New System.Drawing.Size(72, 16)
        Me.labelCustomName.TabIndex = 2
        Me.labelCustomName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'customColorPicker
        '
        Me.customColorPicker.Color = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.customColorPicker.Location = New System.Drawing.Point(8, 8)
        Me.customColorPicker.Name = "customColorPicker"
        Me.customColorPicker.Size = New System.Drawing.Size(448, 280)
        Me.customColorPicker.TabIndex = 19
        AddHandler Me.customColorPicker.ColorChanged, AddressOf Me.customColorPicker_ColorChanged
        '
        'tabPage1
        '
        Me.tabPage1.Controls.Add(Me.dudColorSetOther)
        Me.tabPage1.Controls.Add(Me.colorPicker)
        Me.tabPage1.Controls.Add(Me.labelSystem)
        Me.tabPage1.Controls.Add(Me.colorPickerWeb)
        Me.tabPage1.Controls.Add(Me.colorPickerSystem)
        Me.tabPage1.Controls.Add(Me.cbShowColorName)
        Me.tabPage1.Controls.Add(Me.groupBox1)
        Me.tabPage1.Controls.Add(Me.groupBox2)
        Me.tabPage1.Location = New System.Drawing.Point(4, 22)
        Me.tabPage1.Name = "tabPage1"
        Me.tabPage1.Size = New System.Drawing.Size(464, 339)
        Me.tabPage1.TabIndex = 0
        Me.tabPage1.Text = "Dropdown"
        '
        'dudColorSetOther
        '
        Me.dudColorSetOther.Location = New System.Drawing.Point(248, 248)
        Me.dudColorSetOther.Name = "dudColorSetOther"
        Me.dudColorSetOther.ReadOnly = True
        Me.dudColorSetOther.Size = New System.Drawing.Size(96, 20)
        Me.dudColorSetOther.TabIndex = 13
        AddHandler Me.dudColorSetOther.SelectedItemChanged, AddressOf Me.dudColorSetOther_SelectedItemChanged
        '
        'labelSystem
        '
        Me.labelSystem.Location = New System.Drawing.Point(16, 192)
        Me.labelSystem.Name = "labelSystem"
        Me.labelSystem.Size = New System.Drawing.Size(320, 40)
        Me.labelSystem.TabIndex = 11
        Me.labelSystem.Text = "label5"
        Me.labelSystem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'colorPickerWeb
        '
        Me.colorPickerWeb._Text = "Pick a web color."
        Me.colorPickerWeb.BackColor = System.Drawing.SystemColors.Control
        Me.colorPickerWeb.Color = System.Drawing.Color.Yellow
        Me.colorPickerWeb.ColorSortOrder = PJLControls.ColorSortOrder.Saturation
        Me.colorPickerWeb.Columns = 14
        Me.colorPickerWeb.Location = New System.Drawing.Point(16, 32)
        Me.colorPickerWeb.Name = "colorPickerWeb"
        Me.colorPickerWeb.Size = New System.Drawing.Size(176, 19)
        Me.colorPickerWeb.TabIndex = 1
        AddHandler Me.colorPickerWeb.ColorChanged, AddressOf Me.colorPickerWeb_ColorChanged
        '
        'colorPickerSystem
        '
        Me.colorPickerSystem._Text = "Pick a system color"
        Me.colorPickerSystem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.colorPickerSystem.Color = System.Drawing.SystemColors.Desktop
        Me.colorPickerSystem.ColorSet = PJLControls.ColorSet.System
        Me.colorPickerSystem.ColorSortOrder = PJLControls.ColorSortOrder.Brightness
        Me.colorPickerSystem.ColorWellSize = New System.Drawing.Size(36, 36)
        Me.colorPickerSystem.CustomColors = New System.Drawing.Color() {System.Drawing.Color.White}
        Me.colorPickerSystem.Enabled = False
        Me.colorPickerSystem.Location = New System.Drawing.Point(16, 160)
        Me.colorPickerSystem.Name = "colorPickerSystem"
        Me.colorPickerSystem.PanelBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.colorPickerSystem.Size = New System.Drawing.Size(240, 21)
        Me.colorPickerSystem.TabIndex = 9
        AddHandler Me.colorPickerSystem.ColorChanged, AddressOf Me.colorPickerSystem_ColorChanged
        '
        'cbShowColorName
        '
        Me.cbShowColorName.Location = New System.Drawing.Point(208, 56)
        Me.cbShowColorName.Name = "cbShowColorName"
        Me.cbShowColorName.Size = New System.Drawing.Size(128, 24)
        Me.cbShowColorName.TabIndex = 3
        Me.cbShowColorName.Text = "Show Color Name"
        AddHandler Me.cbShowColorName.CheckedChanged, AddressOf Me.cbShowColorName_CheckedChanged
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(16, 8)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(288, 23)
        Me.label1.TabIndex = 1
        Me.label1.Text = "This will demo the custom color panel when it's done."
        '
        'tabPage4
        '
        Me.tabPage4.Controls.Add(Me.dudZ)
        Me.tabPage4.Controls.Add(Me.label1)
        Me.tabPage4.Controls.Add(Me.customColorPanel1)
        Me.tabPage4.Location = New System.Drawing.Point(4, 22)
        Me.tabPage4.Name = "tabPage4"
        Me.tabPage4.Size = New System.Drawing.Size(464, 339)
        Me.tabPage4.TabIndex = 3
        Me.tabPage4.Text = "Custom Panel"
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.tabPage1)
        Me.tabControl1.Controls.Add(Me.tabPage2)
        Me.tabControl1.Controls.Add(Me.tabPage3)
        Me.tabControl1.Controls.Add(Me.tabPage4)
        Me.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControl1.Location = New System.Drawing.Point(0, 0)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(472, 365)
        Me.tabControl1.TabIndex = 8
        '
        'DemoForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(472, 365)
        Me.Controls.Add(Me.tabControl1)
        Me.Name = "DemoForm"
        Me.Text = "Color Picker"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox2.ResumeLayout(False)
        Me.tabPage2.ResumeLayout(False)
        Me.tabPage3.ResumeLayout(False)
        Me.tabPage1.ResumeLayout(False)
        Me.tabPage4.ResumeLayout(False)
        Me.tabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region

    ''' <summary>
    ''' The main entry point for the application.
    ''' </summary>
    <STAThread()> _
    Private Shared Sub Main()
        Application.Run(New DemoForm())
    End Sub

    Private Sub Form1_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        SetControlColor(labelPanel, colorPanel.Color)
        SetControlColor(labelWeb, colorPickerWeb.Color)
        SetControlColor(labelSystem, colorPickerSystem.Color)

        dudSortColorsWeb.Items.AddRange([Enum].GetValues(GetType(PJLControls.ColorSortOrder)))
        dudSortColorsWeb.SelectedIndex = 0

        dudSortColorsPanel.Items.AddRange([Enum].GetValues(GetType(PJLControls.ColorSortOrder)))
        dudSortColorsPanel.SelectedIndex = 0

        dudColorSetPanel.Items.AddRange([Enum].GetValues(GetType(PJLControls.ColorSet)))
        dudColorSetPanel.SelectedIndex = 0

        dudColorSetOther.Items.AddRange([Enum].GetValues(GetType(PJLControls.ColorSet)))
        dudColorSetOther.SelectedIndex = 0

        dudZ.Items.AddRange([Enum].GetValues(GetType(PJLControls.ZAxis)))
        dudZ.SelectedIndex = 0

        ShowCustomColorPickerColor(customColorPicker.Color)
    End Sub

    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            'cancel the close
            e.Cancel = True
            Me.Hide()
        End If
    End Sub

    Private Sub colorPanel_ColorChanged(sender As Object, e As PJLControls.ColorChangedEventArgs)
        SetControlColor(labelPanel, e.Color)
    End Sub

    Private Sub SetControlColor(ctrl As Control, c As Color)
        ctrl.BackColor = c
        Dim s As String = String.Format("{0}, {1:X}", c.Name, c.ToArgb())
        ctrl.Text = s
        ctrl.ForeColor = If((c.GetBrightness() < 0.3), (Color.White), (Color.Black))
    End Sub

    Private Sub colorPickerWeb_ColorChanged(sender As Object, e As PJLControls.ColorChangedEventArgs)
        SetControlColor(labelWeb, e.Color)
    End Sub

    Private Sub colorPickerSystem_ColorChanged(sender As Object, e As PJLControls.ColorChangedEventArgs)
        SetControlColor(labelSystem, e.Color)
    End Sub

    Private Sub ShowCustomColorPickerColor(c As Color)
        labelCustomColor.BackColor = c
        labelCustomName.Text = ColorTranslator.ToHtml(c)
    End Sub

    Private Sub customColorPicker_ColorChanged(sender As Object, e As PJLControls.ColorChangedEventArgs)
        ShowCustomColorPickerColor(e.Color)
    End Sub

    Private Sub cbShowPickColor_CheckedChanged(sender As Object, e As System.EventArgs)
        colorPickerWeb.DisplayColor = cbShowPickColor.Checked
    End Sub

    Private Sub cbShowColorName_CheckedChanged(sender As Object, e As System.EventArgs)
        colorPickerWeb.DisplayColorName = cbShowColorName.Checked
    End Sub

    Private Sub chkContinuous_CheckedChanged(sender As Object, e As System.EventArgs)
        customColorPicker.EnableContinuousScrollZ = chkContinuous.Checked
    End Sub

    Private Sub cbEnableSystemColors_CheckedChanged(sender As Object, e As System.EventArgs)
        colorPickerSystem.Enabled = cbEnableSystemColors.Checked
    End Sub

    Private Sub cbEnableCustom_CheckedChanged(sender As Object, e As System.EventArgs)
        customColorPicker.Enabled = cbEnableCustom.Checked
    End Sub

    Private Sub cbEnablePanel_CheckedChanged(sender As Object, e As System.EventArgs)
        colorPanel.Enabled = cbEnablePanel.Checked
    End Sub

    Private Sub dudSortColorsWeb_SelectedItemChanged(sender As Object, e As System.EventArgs)
        colorPickerWeb.ColorSortOrder = DirectCast(dudSortColorsWeb.SelectedItem, PJLControls.ColorSortOrder)
    End Sub

    Private Sub dudSortColorsPanel_SelectedItemChanged(sender As Object, e As System.EventArgs)
        colorPanel.ColorSortOrder = DirectCast(dudSortColorsPanel.SelectedItem, PJLControls.ColorSortOrder)
    End Sub

    Private Sub dudColorSetPanel_SelectedItemChanged(sender As Object, e As System.EventArgs)
        colorPanel.ColorSet = DirectCast(dudColorSetPanel.SelectedItem, PJLControls.ColorSet)
    End Sub

    Private Sub dudColorSetOther_SelectedItemChanged(sender As Object, e As System.EventArgs)
        colorPicker.ColorSet = DirectCast(dudColorSetOther.SelectedItem, PJLControls.ColorSet)
    End Sub

    Private Sub dudZ_SelectedItemChanged(sender As Object, e As System.EventArgs)
        customColorPanel1.ZAxis = DirectCast(dudZ.SelectedItem, PJLControls.ZAxis)
    End Sub
End Class
