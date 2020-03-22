using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace ColorPickerDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class DemoForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.CheckBox cbShowColorName;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.CheckBox chkContinuous;
		private System.Windows.Forms.GroupBox groupBox1;
		private PJLControls.ColorPicker colorPicker;
		private PJLControls.ColorPicker colorPickerWeb;
		private PJLControls.ColorPicker colorPickerSystem;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox cbShowPickColor;
		private System.Windows.Forms.Label labelWeb;
		private System.Windows.Forms.Label labelSystem;
		private System.Windows.Forms.Label labelPanel;
		private PJLControls.ColorPanel colorPanel;
		private System.Windows.Forms.Label labelCustomName;
		private System.Windows.Forms.Label labelCustomColor;
		private PJLControls.CustomColorPicker customColorPicker;
		private System.Windows.Forms.CheckBox cbEnableSystemColors;
		private System.Windows.Forms.CheckBox cbEnableCustom;
		private System.Windows.Forms.CheckBox cbEnablePanel;
		private System.Windows.Forms.TabPage tabPage4;
		private PJLControls.CustomColorPanel customColorPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DomainUpDown dudSortColorsWeb;
		private System.Windows.Forms.DomainUpDown dudSortColorsPanel;
		private System.Windows.Forms.DomainUpDown dudColorSetPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DomainUpDown dudColorSetOther;
		private System.Windows.Forms.DomainUpDown dudZ;
		private System.ComponentModel.IContainer components;

		public DemoForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			labelPanel.BackColor   = colorPanel.Color;
			labelPanel.Text        = colorPanel.Color.Name;

			cbEnablePanel.Checked  = colorPanel.Enabled;
			cbEnableCustom.Checked = customColorPicker.Enabled;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dudSortColorsWeb = new System.Windows.Forms.DomainUpDown();
			this.cbEnableCustom = new System.Windows.Forms.CheckBox();
			this.dudSortColorsPanel = new System.Windows.Forms.DomainUpDown();
			this.labelCustomColor = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbEnableSystemColors = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.labelWeb = new System.Windows.Forms.Label();
			this.cbShowPickColor = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.dudZ = new System.Windows.Forms.DomainUpDown();
			this.colorPicker = new PJLControls.ColorPicker();
			this.customColorPanel1 = new PJLControls.CustomColorPanel();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.dudColorSetPanel = new System.Windows.Forms.DomainUpDown();
			this.cbEnablePanel = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.labelPanel = new System.Windows.Forms.Label();
			this.colorPanel = new PJLControls.ColorPanel();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.chkContinuous = new System.Windows.Forms.CheckBox();
			this.labelCustomName = new System.Windows.Forms.Label();
			this.customColorPicker = new PJLControls.CustomColorPicker();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.dudColorSetOther = new System.Windows.Forms.DomainUpDown();
			this.labelSystem = new System.Windows.Forms.Label();
			this.colorPickerWeb = new PJLControls.ColorPicker();
			this.colorPickerSystem = new PJLControls.ColorPicker();
			this.cbShowColorName = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dudSortColorsWeb
			// 
			this.dudSortColorsWeb.Location = new System.Drawing.Point(200, 80);
			this.dudSortColorsWeb.Name = "dudSortColorsWeb";
			this.dudSortColorsWeb.ReadOnly = true;
			this.dudSortColorsWeb.TabIndex = 7;
			this.dudSortColorsWeb.SelectedItemChanged += new System.EventHandler(this.dudSortColorsWeb_SelectedItemChanged);
			// 
			// cbEnableCustom
			// 
			this.cbEnableCustom.Checked = true;
			this.cbEnableCustom.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableCustom.Location = new System.Drawing.Point(296, 312);
			this.cbEnableCustom.Name = "cbEnableCustom";
			this.cbEnableCustom.TabIndex = 4;
			this.cbEnableCustom.Text = "Enable";
			this.cbEnableCustom.CheckedChanged += new System.EventHandler(this.cbEnableCustom_CheckedChanged);
			// 
			// dudSortColorsPanel
			// 
			this.dudSortColorsPanel.Location = new System.Drawing.Point(272, 56);
			this.dudSortColorsPanel.Name = "dudSortColorsPanel";
			this.dudSortColorsPanel.ReadOnly = true;
			this.dudSortColorsPanel.TabIndex = 19;
			this.dudSortColorsPanel.SelectedItemChanged += new System.EventHandler(this.dudSortColorsPanel_SelectedItemChanged);
			// 
			// labelCustomColor
			// 
			this.labelCustomColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelCustomColor.Location = new System.Drawing.Point(16, 288);
			this.labelCustomColor.Name = "labelCustomColor";
			this.labelCustomColor.Size = new System.Drawing.Size(256, 32);
			this.labelCustomColor.TabIndex = 1;
			this.labelCustomColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.cbEnableSystemColors});
			this.groupBox1.Location = new System.Drawing.Point(8, 136);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(336, 104);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "System Colors";
			// 
			// cbEnableSystemColors
			// 
			this.cbEnableSystemColors.Location = new System.Drawing.Point(256, 24);
			this.cbEnableSystemColors.Name = "cbEnableSystemColors";
			this.cbEnableSystemColors.Size = new System.Drawing.Size(72, 24);
			this.cbEnableSystemColors.TabIndex = 10;
			this.cbEnableSystemColors.Text = "Enable";
			this.cbEnableSystemColors.CheckedChanged += new System.EventHandler(this.cbEnableSystemColors_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.dudSortColorsWeb,
																					this.labelWeb,
																					this.cbShowPickColor,
																					this.label9});
			this.groupBox2.Location = new System.Drawing.Point(8, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(336, 112);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Web Colors";
			// 
			// labelWeb
			// 
			this.labelWeb.Location = new System.Drawing.Point(8, 48);
			this.labelWeb.Name = "labelWeb";
			this.labelWeb.Size = new System.Drawing.Size(184, 56);
			this.labelWeb.TabIndex = 6;
			this.labelWeb.Text = "labelWeb";
			this.labelWeb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cbShowPickColor
			// 
			this.cbShowPickColor.Checked = true;
			this.cbShowPickColor.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbShowPickColor.Location = new System.Drawing.Point(200, 16);
			this.cbShowPickColor.Name = "cbShowPickColor";
			this.cbShowPickColor.Size = new System.Drawing.Size(128, 24);
			this.cbShowPickColor.TabIndex = 2;
			this.cbShowPickColor.Text = "Show Pick Color";
			this.cbShowPickColor.CheckedChanged += new System.EventHandler(this.cbShowPickColor_CheckedChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(200, 64);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(96, 16);
			this.label9.TabIndex = 4;
			this.label9.Text = "Sort Colors By";
			// 
			// dudZ
			// 
			this.dudZ.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.dudZ.Location = new System.Drawing.Point(312, 32);
			this.dudZ.Name = "dudZ";
			this.dudZ.ReadOnly = true;
			this.dudZ.TabIndex = 2;
			this.dudZ.SelectedItemChanged += new System.EventHandler(this.dudZ_SelectedItemChanged);
			// 
			// colorPicker
			// 
			this.colorPicker._Text = "Pick another color";
			this.colorPicker.AutoSize = false;
			this.colorPicker.ColorSortOrder = PJLControls.ColorSortOrder.Brightness;
			this.colorPicker.ColorWellSize = new System.Drawing.Size(24, 12);
			this.colorPicker.CustomColors = new System.Drawing.Color[] {
																		   System.Drawing.Color.Tomato,
																		   System.Drawing.Color.Firebrick,
																		   System.Drawing.Color.LightCoral,
																		   System.Drawing.Color.Maroon,
																		   System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(64)), ((System.Byte)(0))),
																		   System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0))),
																		   System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(192)), ((System.Byte)(128))),
																		   System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(224)), ((System.Byte)(192))),
																		   System.Drawing.Color.DarkRed,
																		   System.Drawing.Color.DarkSalmon,
																		   System.Drawing.Color.Coral,
																		   System.Drawing.Color.OrangeRed};
			this.colorPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.colorPicker.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(128)), ((System.Byte)(128)), ((System.Byte)(255)));
			this.colorPicker.Location = new System.Drawing.Point(8, 248);
			this.colorPicker.Name = "colorPicker";
			this.colorPicker.Size = new System.Drawing.Size(232, 40);
			this.colorPicker.TabIndex = 12;
			// 
			// customColorPanel1
			// 
			this.customColorPanel1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.customColorPanel1.AutoSize = true;
			this.customColorPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.customColorPanel1.Color = System.Drawing.Color.Empty;
			this.customColorPanel1.Isotropic = true;
			this.customColorPanel1.Location = new System.Drawing.Point(16, 32);
			this.customColorPanel1.Name = "customColorPanel1";
			this.customColorPanel1.Size = new System.Drawing.Size(280, 244);
			this.customColorPanel1.TabIndex = 0;
			this.customColorPanel1.ZAxis = PJLControls.ZAxis.red;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.label2,
																				   this.dudColorSetPanel,
																				   this.dudSortColorsPanel,
																				   this.cbEnablePanel,
																				   this.label8,
																				   this.labelPanel,
																				   this.colorPanel});
			this.tabPage2.Location = new System.Drawing.Point(4, 4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(464, 357);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Panel";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(176, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 21;
			this.label2.Text = "Color Set";
			// 
			// dudColorSetPanel
			// 
			this.dudColorSetPanel.Location = new System.Drawing.Point(272, 88);
			this.dudColorSetPanel.Name = "dudColorSetPanel";
			this.dudColorSetPanel.ReadOnly = true;
			this.dudColorSetPanel.TabIndex = 20;
			this.dudColorSetPanel.SelectedItemChanged += new System.EventHandler(this.dudColorSetPanel_SelectedItemChanged);
			// 
			// cbEnablePanel
			// 
			this.cbEnablePanel.Location = new System.Drawing.Point(176, 16);
			this.cbEnablePanel.Name = "cbEnablePanel";
			this.cbEnablePanel.TabIndex = 15;
			this.cbEnablePanel.Text = "Enable Panel";
			this.cbEnablePanel.CheckedChanged += new System.EventHandler(this.cbEnablePanel_CheckedChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(176, 56);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(96, 16);
			this.label8.TabIndex = 16;
			this.label8.Text = "Sort Colors By";
			// 
			// labelPanel
			// 
			this.labelPanel.Location = new System.Drawing.Point(176, 144);
			this.labelPanel.Name = "labelPanel";
			this.labelPanel.Size = new System.Drawing.Size(216, 88);
			this.labelPanel.TabIndex = 18;
			this.labelPanel.Text = "label1";
			this.labelPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// colorPanel
			// 
			this.colorPanel.ColorSortOrder = PJLControls.ColorSortOrder.Brightness;
			this.colorPanel.CustomColors = new System.Drawing.Color[] {
																		  System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(192)), ((System.Byte)(192))),
																		  System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(128))),
																		  System.Drawing.Color.Red,
																		  System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(0)), ((System.Byte)(0))),
																		  System.Drawing.Color.Maroon,
																		  System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(255)), ((System.Byte)(255))),
																		  System.Drawing.Color.FromArgb(((System.Byte)(128)), ((System.Byte)(255)), ((System.Byte)(255))),
																		  System.Drawing.Color.Aqua,
																		  System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(192))),
																		  System.Drawing.Color.Teal,
																		  System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(192)), ((System.Byte)(255))),
																		  System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(255))),
																		  System.Drawing.Color.Magenta,
																		  System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(0)), ((System.Byte)(192))),
																		  System.Drawing.Color.Purple,
																		  System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192))),
																		  System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(128))),
																		  System.Drawing.Color.Yellow,
																		  System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(0))),
																		  System.Drawing.Color.Olive};
			this.colorPanel.Location = new System.Drawing.Point(0, 8);
			this.colorPanel.Name = "colorPanel";
			this.colorPanel.Size = new System.Drawing.Size(148, 260);
			this.colorPanel.TabIndex = 13;
			this.colorPanel.ColorChanged += new PJLControls.ColorChangedEventHandler(this.colorPanel_ColorChanged);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.cbEnableCustom,
																				   this.chkContinuous,
																				   this.labelCustomName,
																				   this.labelCustomColor,
																				   this.customColorPicker});
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(464, 339);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Custom";
			// 
			// chkContinuous
			// 
			this.chkContinuous.Checked = true;
			this.chkContinuous.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkContinuous.Location = new System.Drawing.Point(296, 288);
			this.chkContinuous.Name = "chkContinuous";
			this.chkContinuous.Size = new System.Drawing.Size(144, 24);
			this.chkContinuous.TabIndex = 3;
			this.chkContinuous.Text = "Continous Scroll Z Axis";
			this.chkContinuous.CheckedChanged += new System.EventHandler(this.chkContinuous_CheckedChanged);
			// 
			// labelCustomName
			// 
			this.labelCustomName.BackColor = System.Drawing.Color.White;
			this.labelCustomName.Location = new System.Drawing.Point(108, 296);
			this.labelCustomName.Name = "labelCustomName";
			this.labelCustomName.Size = new System.Drawing.Size(72, 16);
			this.labelCustomName.TabIndex = 2;
			this.labelCustomName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// customColorPicker
			// 
			this.customColorPicker.Color = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(192)), ((System.Byte)(128)));
			this.customColorPicker.Location = new System.Drawing.Point(8, 8);
			this.customColorPicker.Name = "customColorPicker";
			this.customColorPicker.Size = new System.Drawing.Size(448, 280);
			this.customColorPicker.TabIndex = 19;
			this.customColorPicker.ColorChanged += new PJLControls.ColorChangedEventHandler(this.customColorPicker_ColorChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.dudColorSetOther,
																				   this.colorPicker,
																				   this.labelSystem,
																				   this.colorPickerWeb,
																				   this.colorPickerSystem,
																				   this.cbShowColorName,
																				   this.groupBox1,
																				   this.groupBox2});
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(464, 339);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Dropdown";
			// 
			// dudColorSetOther
			// 
			this.dudColorSetOther.Location = new System.Drawing.Point(248, 248);
			this.dudColorSetOther.Name = "dudColorSetOther";
			this.dudColorSetOther.ReadOnly = true;
			this.dudColorSetOther.Size = new System.Drawing.Size(96, 20);
			this.dudColorSetOther.TabIndex = 13;
			this.dudColorSetOther.SelectedItemChanged += new System.EventHandler(this.dudColorSetOther_SelectedItemChanged);
			// 
			// labelSystem
			// 
			this.labelSystem.Location = new System.Drawing.Point(16, 192);
			this.labelSystem.Name = "labelSystem";
			this.labelSystem.Size = new System.Drawing.Size(320, 40);
			this.labelSystem.TabIndex = 11;
			this.labelSystem.Text = "label5";
			this.labelSystem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// colorPickerWeb
			// 
			this.colorPickerWeb._Text = "Pick a web color.";
			this.colorPickerWeb.BackColor = System.Drawing.SystemColors.Control;
			this.colorPickerWeb.Color = System.Drawing.Color.Yellow;
			this.colorPickerWeb.ColorSortOrder = PJLControls.ColorSortOrder.Saturation;
			this.colorPickerWeb.Columns = 14;
			this.colorPickerWeb.Location = new System.Drawing.Point(16, 32);
			this.colorPickerWeb.Name = "colorPickerWeb";
			this.colorPickerWeb.Size = new System.Drawing.Size(176, 19);
			this.colorPickerWeb.TabIndex = 1;
			this.colorPickerWeb.ColorChanged += new PJLControls.ColorChangedEventHandler(this.colorPickerWeb_ColorChanged);
			// 
			// colorPickerSystem
			// 
			this.colorPickerSystem._Text = "Pick a system color";
			this.colorPickerSystem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.colorPickerSystem.Color = System.Drawing.SystemColors.Desktop;
			this.colorPickerSystem.ColorSet = PJLControls.ColorSet.System;
			this.colorPickerSystem.ColorSortOrder = PJLControls.ColorSortOrder.Brightness;
			this.colorPickerSystem.ColorWellSize = new System.Drawing.Size(36, 36);
			this.colorPickerSystem.CustomColors = new System.Drawing.Color[] {
																				 System.Drawing.Color.White};
			this.colorPickerSystem.Enabled = false;
			this.colorPickerSystem.Location = new System.Drawing.Point(16, 160);
			this.colorPickerSystem.Name = "colorPickerSystem";
			this.colorPickerSystem.PanelBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.colorPickerSystem.Size = new System.Drawing.Size(240, 21);
			this.colorPickerSystem.TabIndex = 9;
			this.colorPickerSystem.ColorChanged += new PJLControls.ColorChangedEventHandler(this.colorPickerSystem_ColorChanged);
			// 
			// cbShowColorName
			// 
			this.cbShowColorName.Location = new System.Drawing.Point(208, 56);
			this.cbShowColorName.Name = "cbShowColorName";
			this.cbShowColorName.Size = new System.Drawing.Size(128, 24);
			this.cbShowColorName.TabIndex = 3;
			this.cbShowColorName.Text = "Show Color Name";
			this.cbShowColorName.CheckedChanged += new System.EventHandler(this.cbShowColorName_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(288, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "This will demo the custom color panel when it\'s done.";
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.dudZ,
																				   this.label1,
																				   this.customColorPanel1});
			this.tabPage4.Location = new System.Drawing.Point(4, 4);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(464, 357);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Custom Panel";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1,
																					  this.tabPage2,
																					  this.tabPage3,
																					  this.tabPage4});
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(472, 365);
			this.tabControl1.TabIndex = 8;
			// 
			// DemoForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(472, 365);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.Name = "DemoForm";
			this.Text = "Color Picker Demo";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new DemoForm());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			SetControlColor( labelPanel, colorPanel.Color );
			SetControlColor( labelWeb, colorPickerWeb.Color );
			SetControlColor( labelSystem, colorPickerSystem.Color );

			dudSortColorsWeb.Items.AddRange( Enum.GetValues( typeof(PJLControls.ColorSortOrder) ) );
			dudSortColorsWeb.SelectedIndex = 0;

			dudSortColorsPanel.Items.AddRange( Enum.GetValues( typeof(PJLControls.ColorSortOrder) ) );
			dudSortColorsPanel.SelectedIndex = 0;
			
			dudColorSetPanel.Items.AddRange( Enum.GetValues( typeof(PJLControls.ColorSet) ) );
			dudColorSetPanel.SelectedIndex = 0;

			dudColorSetOther.Items.AddRange( Enum.GetValues( typeof(PJLControls.ColorSet) ) );
			dudColorSetOther.SelectedIndex = 0;

			dudZ.Items.AddRange( Enum.GetValues( typeof(PJLControls.ZAxis) ) );
			dudZ.SelectedIndex = 0;

			ShowCustomColorPickerColor( customColorPicker.Color );
		}

		private void colorPanel_ColorChanged(object sender, PJLControls.ColorChangedEventArgs e)
		{
			SetControlColor( labelPanel, e.Color );
		}

		private void SetControlColor(Control ctrl, Color c)
		{
			ctrl.BackColor = c;
			string s = string.Format( "{0}, {1:X}", c.Name, c.ToArgb() );
			ctrl.Text = s;
			ctrl.ForeColor = ( c.GetBrightness() < 0.3 ) ? (Color.White) : (Color.Black);
		}

		private void colorPickerWeb_ColorChanged(object sender, PJLControls.ColorChangedEventArgs e)
		{
			SetControlColor( labelWeb, e.Color );
		}

		private void colorPickerSystem_ColorChanged(object sender, PJLControls.ColorChangedEventArgs e)
		{
			SetControlColor( labelSystem, e.Color );
		}
		
		private void ShowCustomColorPickerColor( Color c )
		{
			labelCustomColor.BackColor = c;
			labelCustomName.Text       = ColorTranslator.ToHtml(c);
		}

		private void customColorPicker_ColorChanged(object sender, PJLControls.ColorChangedEventArgs e)
		{
			ShowCustomColorPickerColor(e.Color);
		}

		private void cbShowPickColor_CheckedChanged(object sender, System.EventArgs e)
		{
			colorPickerWeb.DisplayColor = cbShowPickColor.Checked;
		}

		private void cbShowColorName_CheckedChanged(object sender, System.EventArgs e)
		{
			colorPickerWeb.DisplayColorName = cbShowColorName.Checked;
		}

		private void chkContinuous_CheckedChanged(object sender, System.EventArgs e)
		{
			customColorPicker.EnableContinuousScrollZ = chkContinuous.Checked;
		}

		private void cbEnableSystemColors_CheckedChanged(object sender, System.EventArgs e)
		{
			colorPickerSystem.Enabled = cbEnableSystemColors.Checked;
		}

		private void cbEnableCustom_CheckedChanged(object sender, System.EventArgs e)
		{
			customColorPicker.Enabled = cbEnableCustom.Checked;
		}

		private void cbEnablePanel_CheckedChanged(object sender, System.EventArgs e)
		{
			colorPanel.Enabled = cbEnablePanel.Checked;
		}

		private void dudSortColorsWeb_SelectedItemChanged(object sender, System.EventArgs e)
		{
			colorPickerWeb.ColorSortOrder = (PJLControls.ColorSortOrder)dudSortColorsWeb.SelectedItem;
		}

		private void dudSortColorsPanel_SelectedItemChanged(object sender, System.EventArgs e)
		{
			colorPanel.ColorSortOrder = (PJLControls.ColorSortOrder)dudSortColorsPanel.SelectedItem;
		}

		private void dudColorSetPanel_SelectedItemChanged(object sender, System.EventArgs e)
		{
			colorPanel.ColorSet = (PJLControls.ColorSet)dudColorSetPanel.SelectedItem;
		}

		private void dudColorSetOther_SelectedItemChanged(object sender, System.EventArgs e)
		{
			colorPicker.ColorSet = (PJLControls.ColorSet)dudColorSetOther.SelectedItem;
		}

		private void dudZ_SelectedItemChanged(object sender, System.EventArgs e)
		{
			customColorPanel1.ZAxis = (PJLControls.ZAxis)dudZ.SelectedItem;
		}
	}
}
