/*=====================================================================
  File:      frmDemo.cs

  Summary:   Main form for GSMComm Demo application. Demonstrates how
             to use GSMComm for various operations.

---------------------------------------------------------------------

This source code is intended only as a supplement to the GSMComm
development package or on-line documentation and may not be distributed
separately.
=====================================================================*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;
using GsmComm.GsmCommunication;
using GsmComm.Interfaces;
using GsmComm.Server;
using System.Collections.Generic;

namespace GSMCommDemo
{
	/// <summary>
	/// The main form for the GSMComm demo.
	/// </summary>
	public class frmDemo : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.TextBox txtOutput;
		private System.Windows.Forms.Button btnIdentify;
		private System.Windows.Forms.Button btnReadMessages;
		private System.Windows.Forms.Button btnDumpPhonebook;
		private System.Windows.Forms.Button btnDelMessage;
		private System.Windows.Forms.Button btnMsgStorages;
		private System.Windows.Forms.Button btnDelAllMsgs;
		private System.Windows.Forms.Button btnCopyMessages;
		private System.Windows.Forms.Button btnExportMessages;
		private System.Windows.Forms.Button btnImportMessages;
		private System.Windows.Forms.Button btnExportPhonebook;
		private System.Windows.Forms.Button btnImportPhonebook;
		private System.Windows.Forms.Button btnDeletePhonebookEntry;
		private System.Windows.Forms.Button btnDeletePhonebook;
		private System.Windows.Forms.Button btnMsgNotification;
		private System.Windows.Forms.Button btnMsgNotificationOff;
		private System.Windows.Forms.Button btnMsgRoutingOff;
		private System.Windows.Forms.Button btnMsgRoutingOn;
		private System.Windows.Forms.CheckBox chkEnableLogging;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnIsConnected;
		private System.Windows.Forms.TabPage tabPhonebook;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.Button txtClearOutput;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton rbMessageSIM;
		private System.Windows.Forms.RadioButton rbMessagePhone;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RadioButton rbPhonebookPhone;
		private System.Windows.Forms.RadioButton rbPhonebookSIM;
		private System.Windows.Forms.Button btnCreatePbEntry;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtPbName;
		private System.Windows.Forms.TextBox txtPbNumber;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtDelMsgIndex;
		private System.Windows.Forms.TextBox txtDelPbIndex;
		private System.Windows.Forms.Button btnPbStorages;
		private System.Windows.Forms.Label lblNotConnected;
		private System.Windows.Forms.Button btnMsgMemStatus;
		private System.Windows.Forms.Button btnPbMemStatus;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button btnSendConcatSMS;
		private System.Windows.Forms.TextBox txtConcatNumber;
		private System.Windows.Forms.TextBox txtConcatMessage;
		private System.Windows.Forms.Button btnCreateConcatSms;
		private System.Windows.Forms.CheckBox chkCreateEmptyOpLogo;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label lblBitmap;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label lblMCC;
		private System.Windows.Forms.TextBox txtMNC;
		private System.Windows.Forms.TextBox txtMCC;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox testBitmap;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button btnSendOperatorLogo;
		private System.Windows.Forms.TabPage tabSmartMessaging;
		private System.Windows.Forms.TabPage tabManageSMS;
		private System.Windows.Forms.TabPage tabSendSMS;
		private System.Windows.Forms.CheckBox chkReport;
		private System.Windows.Forms.CheckBox chkAlert;
		private System.Windows.Forms.Button btnSendMessage;
		private System.Windows.Forms.TextBox txtNumber;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkUnicode;
		private System.Windows.Forms.CheckBox chkSMSC;
		private System.Windows.Forms.TextBox txtSMSC;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TabPage tabTextMode;
		private System.Windows.Forms.Button btnGetCharset;
		private System.Windows.Forms.Button btnCharsets;
		private System.Windows.Forms.Button btnSelectCharset;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtCharset;
		private System.Windows.Forms.CheckBox chkSmartUnicode;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnBatteryCharge;
		private System.Windows.Forms.Button btnSignalQuality;
		private System.Windows.Forms.TextBox txtNewSMSC;
		private System.Windows.Forms.Button btnSetSMSC;
		private System.Windows.Forms.Button btnGetSMSC;
		private System.Windows.Forms.CheckBox chkNewSMSCType;
		private System.Windows.Forms.TextBox txtNewSMSCType;
		private System.Windows.Forms.TabPage tabNetwork;
		private System.Windows.Forms.Button GetOperator;
		private System.Windows.Forms.Button GetOpSelectionMode;
		private System.Windows.Forms.Button btnListOperators;
		private TabPage tabRemoting;
		private GroupBox groupBox4;
		private TextBox txtRemoteNumber;
		private Button btnRemoteSendSMS;
		private Label label16;
		private Label label13;
		private TextBox txtRemoteMessage;
		private Button btnStopRemotingServer;
		private Label label10;
		private TextBox txtRemotingPort;
		private Button btnStartRemotingServer;
		private CheckBox chkRemoteUnicode;
		private TextBox txtRemoteServer;
		private Label label17;
		private TextBox txtRemotePort;
		private Label label18;
		private Button btnSubscriberNumbers;
		private CheckBox chkSecureServer;
		private CheckBox chkRemoteSecure;
		private Button btnRemoteSecurity;

		private GsmCommMain comm;
		private bool registerMessageReceived;
		private SmsServer smsServer;
		private CheckBox chkAllowAnonymous;
		private CheckBox chkSmsBatchMode;
		private Label label19;
		private TextBox txtSendTimes;
		private CheckBox chkMultipleTimes;
		private TextBox txtPin;
		private Button btnEnterPin;
		private Button btnGetPinStatus;
		private Label label20;
		private RadioButton rbExportText;
		private RadioButton rbExportBinary;
		private Panel panel2;
		private TabPage tabCustomCommands;
		private Button btnReleaseProtocol;
		private Button btnGetProtocol;
		private ListBox lstProtocolCommands;
		private GroupBox gbProtocolCommands;
		private Button btnExecuteCommand;
		private Label lblError;
		private TextBox txtError;
		private Label lblPattern;
		private Label lblData;
		private TextBox txtPattern;
		private TextBox txtData;
		private SecuritySettings remotingSecurity;
		private TabPage tabSmartMessaging2;
		private GroupBox groupBox5;
		private Label label21;
		private Label label22;
		private TextBox txtCombinedConcatMessage;
		private Button btnCombineParts;
		private RadioButton rbConcatIncoming;
		private RadioButton rbConcatOutgoing;
		private TextBox txtCombineSourceParts;
		private TextBox txtDestinationPort;
		private CheckBox chkDestinationPort;
		private CheckBox chkConcatAsText;
	
		private delegate void SetTextCallback(string text);
		private IProtocol protocolLevel;

		public frmDemo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.comm = null;
			this.registerMessageReceived = false;
			this.smsServer = null;
			this.remotingSecurity = new SecuritySettings();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDemo));
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.btnIdentify = new System.Windows.Forms.Button();
			this.btnReadMessages = new System.Windows.Forms.Button();
			this.btnDumpPhonebook = new System.Windows.Forms.Button();
			this.btnDelMessage = new System.Windows.Forms.Button();
			this.btnMsgStorages = new System.Windows.Forms.Button();
			this.btnDelAllMsgs = new System.Windows.Forms.Button();
			this.btnCopyMessages = new System.Windows.Forms.Button();
			this.btnExportMessages = new System.Windows.Forms.Button();
			this.btnImportMessages = new System.Windows.Forms.Button();
			this.btnExportPhonebook = new System.Windows.Forms.Button();
			this.btnImportPhonebook = new System.Windows.Forms.Button();
			this.btnDeletePhonebookEntry = new System.Windows.Forms.Button();
			this.btnDeletePhonebook = new System.Windows.Forms.Button();
			this.btnMsgRoutingOff = new System.Windows.Forms.Button();
			this.btnMsgRoutingOn = new System.Windows.Forms.Button();
			this.btnMsgNotificationOff = new System.Windows.Forms.Button();
			this.btnMsgNotification = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.chkEnableLogging = new System.Windows.Forms.CheckBox();
			this.btnIsConnected = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.txtPin = new System.Windows.Forms.TextBox();
			this.btnEnterPin = new System.Windows.Forms.Button();
			this.btnGetPinStatus = new System.Windows.Forms.Button();
			this.btnBatteryCharge = new System.Windows.Forms.Button();
			this.btnSignalQuality = new System.Windows.Forms.Button();
			this.tabSendSMS = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtDestinationPort = new System.Windows.Forms.TextBox();
			this.chkDestinationPort = new System.Windows.Forms.CheckBox();
			this.chkSmsBatchMode = new System.Windows.Forms.CheckBox();
			this.label19 = new System.Windows.Forms.Label();
			this.txtSendTimes = new System.Windows.Forms.TextBox();
			this.chkMultipleTimes = new System.Windows.Forms.CheckBox();
			this.chkSMSC = new System.Windows.Forms.CheckBox();
			this.chkUnicode = new System.Windows.Forms.CheckBox();
			this.chkReport = new System.Windows.Forms.CheckBox();
			this.chkAlert = new System.Windows.Forms.CheckBox();
			this.txtSMSC = new System.Windows.Forms.TextBox();
			this.btnSendMessage = new System.Windows.Forms.Button();
			this.txtNumber = new System.Windows.Forms.TextBox();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabManageSMS = new System.Windows.Forms.TabPage();
			this.panel2 = new System.Windows.Forms.Panel();
			this.rbExportBinary = new System.Windows.Forms.RadioButton();
			this.rbExportText = new System.Windows.Forms.RadioButton();
			this.label20 = new System.Windows.Forms.Label();
			this.txtNewSMSCType = new System.Windows.Forms.TextBox();
			this.chkNewSMSCType = new System.Windows.Forms.CheckBox();
			this.btnGetSMSC = new System.Windows.Forms.Button();
			this.btnSetSMSC = new System.Windows.Forms.Button();
			this.txtNewSMSC = new System.Windows.Forms.TextBox();
			this.btnMsgMemStatus = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.txtDelMsgIndex = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.rbMessagePhone = new System.Windows.Forms.RadioButton();
			this.rbMessageSIM = new System.Windows.Forms.RadioButton();
			this.tabPhonebook = new System.Windows.Forms.TabPage();
			this.btnPbMemStatus = new System.Windows.Forms.Button();
			this.btnPbStorages = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.txtDelPbIndex = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtPbNumber = new System.Windows.Forms.TextBox();
			this.txtPbName = new System.Windows.Forms.TextBox();
			this.btnCreatePbEntry = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.rbPhonebookPhone = new System.Windows.Forms.RadioButton();
			this.rbPhonebookSIM = new System.Windows.Forms.RadioButton();
			this.tabSmartMessaging = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.lblBitmap = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.lblMCC = new System.Windows.Forms.Label();
			this.txtMNC = new System.Windows.Forms.TextBox();
			this.txtMCC = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.testBitmap = new System.Windows.Forms.PictureBox();
			this.btnSendOperatorLogo = new System.Windows.Forms.Button();
			this.chkCreateEmptyOpLogo = new System.Windows.Forms.CheckBox();
			this.label14 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkSmartUnicode = new System.Windows.Forms.CheckBox();
			this.txtConcatMessage = new System.Windows.Forms.TextBox();
			this.btnCreateConcatSms = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.btnSendConcatSMS = new System.Windows.Forms.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.txtConcatNumber = new System.Windows.Forms.TextBox();
			this.tabSmartMessaging2 = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.chkConcatAsText = new System.Windows.Forms.CheckBox();
			this.txtCombineSourceParts = new System.Windows.Forms.TextBox();
			this.rbConcatIncoming = new System.Windows.Forms.RadioButton();
			this.rbConcatOutgoing = new System.Windows.Forms.RadioButton();
			this.btnCombineParts = new System.Windows.Forms.Button();
			this.label22 = new System.Windows.Forms.Label();
			this.txtCombinedConcatMessage = new System.Windows.Forms.TextBox();
			this.label21 = new System.Windows.Forms.Label();
			this.tabRemoting = new System.Windows.Forms.TabPage();
			this.chkAllowAnonymous = new System.Windows.Forms.CheckBox();
			this.chkSecureServer = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.btnRemoteSecurity = new System.Windows.Forms.Button();
			this.chkRemoteSecure = new System.Windows.Forms.CheckBox();
			this.txtRemotePort = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.txtRemoteServer = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.chkRemoteUnicode = new System.Windows.Forms.CheckBox();
			this.txtRemoteNumber = new System.Windows.Forms.TextBox();
			this.btnRemoteSendSMS = new System.Windows.Forms.Button();
			this.label16 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.txtRemoteMessage = new System.Windows.Forms.TextBox();
			this.btnStopRemotingServer = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.txtRemotingPort = new System.Windows.Forms.TextBox();
			this.btnStartRemotingServer = new System.Windows.Forms.Button();
			this.tabNetwork = new System.Windows.Forms.TabPage();
			this.btnSubscriberNumbers = new System.Windows.Forms.Button();
			this.btnListOperators = new System.Windows.Forms.Button();
			this.GetOpSelectionMode = new System.Windows.Forms.Button();
			this.GetOperator = new System.Windows.Forms.Button();
			this.tabTextMode = new System.Windows.Forms.TabPage();
			this.label3 = new System.Windows.Forms.Label();
			this.txtCharset = new System.Windows.Forms.TextBox();
			this.btnSelectCharset = new System.Windows.Forms.Button();
			this.btnCharsets = new System.Windows.Forms.Button();
			this.btnGetCharset = new System.Windows.Forms.Button();
			this.tabCustomCommands = new System.Windows.Forms.TabPage();
			this.gbProtocolCommands = new System.Windows.Forms.GroupBox();
			this.btnExecuteCommand = new System.Windows.Forms.Button();
			this.lblError = new System.Windows.Forms.Label();
			this.txtError = new System.Windows.Forms.TextBox();
			this.lblPattern = new System.Windows.Forms.Label();
			this.lblData = new System.Windows.Forms.Label();
			this.txtPattern = new System.Windows.Forms.TextBox();
			this.txtData = new System.Windows.Forms.TextBox();
			this.lstProtocolCommands = new System.Windows.Forms.ListBox();
			this.btnReleaseProtocol = new System.Windows.Forms.Button();
			this.btnGetProtocol = new System.Windows.Forms.Button();
			this.txtClearOutput = new System.Windows.Forms.Button();
			this.lblNotConnected = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.tabControl.SuspendLayout();
			this.tabGeneral.SuspendLayout();
			this.tabSendSMS.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabManageSMS.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tabPhonebook.SuspendLayout();
			this.tabSmartMessaging.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.testBitmap)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.tabSmartMessaging2.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.tabRemoting.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabNetwork.SuspendLayout();
			this.tabTextMode.SuspendLayout();
			this.tabCustomCommands.SuspendLayout();
			this.gbProtocolCommands.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtOutput
			// 
			this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutput.Location = new System.Drawing.Point(8, 256);
			this.txtOutput.MaxLength = 0;
			this.txtOutput.Multiline = true;
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ReadOnly = true;
			this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtOutput.Size = new System.Drawing.Size(512, 184);
			this.txtOutput.TabIndex = 1;
			// 
			// btnIdentify
			// 
			this.btnIdentify.Location = new System.Drawing.Point(8, 56);
			this.btnIdentify.Name = "btnIdentify";
			this.btnIdentify.Size = new System.Drawing.Size(128, 32);
			this.btnIdentify.TabIndex = 1;
			this.btnIdentify.Text = "Identify phone";
			this.btnIdentify.Click += new System.EventHandler(this.btnIdentify_Click);
			// 
			// btnReadMessages
			// 
			this.btnReadMessages.Location = new System.Drawing.Point(8, 16);
			this.btnReadMessages.Name = "btnReadMessages";
			this.btnReadMessages.Size = new System.Drawing.Size(128, 32);
			this.btnReadMessages.TabIndex = 0;
			this.btnReadMessages.Text = "Read all messages";
			this.btnReadMessages.Click += new System.EventHandler(this.btnReadMessages_Click);
			// 
			// btnDumpPhonebook
			// 
			this.btnDumpPhonebook.Location = new System.Drawing.Point(8, 16);
			this.btnDumpPhonebook.Name = "btnDumpPhonebook";
			this.btnDumpPhonebook.Size = new System.Drawing.Size(128, 32);
			this.btnDumpPhonebook.TabIndex = 0;
			this.btnDumpPhonebook.Text = "Read phonebook";
			this.btnDumpPhonebook.Click += new System.EventHandler(this.btnDumpPhonebook_Click);
			// 
			// btnDelMessage
			// 
			this.btnDelMessage.Location = new System.Drawing.Point(8, 56);
			this.btnDelMessage.Name = "btnDelMessage";
			this.btnDelMessage.Size = new System.Drawing.Size(64, 36);
			this.btnDelMessage.TabIndex = 1;
			this.btnDelMessage.Text = "Delete Message";
			this.btnDelMessage.Click += new System.EventHandler(this.btnDelMessage_Click);
			// 
			// btnMsgStorages
			// 
			this.btnMsgStorages.Location = new System.Drawing.Point(8, 144);
			this.btnMsgStorages.Name = "btnMsgStorages";
			this.btnMsgStorages.Size = new System.Drawing.Size(128, 36);
			this.btnMsgStorages.TabIndex = 5;
			this.btnMsgStorages.Text = "Get supported message storages";
			this.btnMsgStorages.Click += new System.EventHandler(this.btnMsgStorages_Click);
			// 
			// btnDelAllMsgs
			// 
			this.btnDelAllMsgs.Location = new System.Drawing.Point(8, 104);
			this.btnDelAllMsgs.Name = "btnDelAllMsgs";
			this.btnDelAllMsgs.Size = new System.Drawing.Size(128, 32);
			this.btnDelAllMsgs.TabIndex = 4;
			this.btnDelAllMsgs.Text = "Delete all messages";
			this.btnDelAllMsgs.Click += new System.EventHandler(this.btnDelAllMsgs_Click);
			// 
			// btnCopyMessages
			// 
			this.btnCopyMessages.Location = new System.Drawing.Point(152, 168);
			this.btnCopyMessages.Name = "btnCopyMessages";
			this.btnCopyMessages.Size = new System.Drawing.Size(128, 36);
			this.btnCopyMessages.TabIndex = 11;
			this.btnCopyMessages.Text = "Copy all messages from SIM to phone";
			this.btnCopyMessages.Click += new System.EventHandler(this.btnCopyMessages_Click);
			// 
			// btnExportMessages
			// 
			this.btnExportMessages.Location = new System.Drawing.Point(296, 16);
			this.btnExportMessages.Name = "btnExportMessages";
			this.btnExportMessages.Size = new System.Drawing.Size(128, 32);
			this.btnExportMessages.TabIndex = 12;
			this.btnExportMessages.Text = "Export all messages";
			this.btnExportMessages.Click += new System.EventHandler(this.btnExportMessages_Click);
			// 
			// btnImportMessages
			// 
			this.btnImportMessages.Location = new System.Drawing.Point(296, 56);
			this.btnImportMessages.Name = "btnImportMessages";
			this.btnImportMessages.Size = new System.Drawing.Size(128, 32);
			this.btnImportMessages.TabIndex = 14;
			this.btnImportMessages.Text = "Import messages";
			this.btnImportMessages.Click += new System.EventHandler(this.btnImportMessages_Click);
			// 
			// btnExportPhonebook
			// 
			this.btnExportPhonebook.Location = new System.Drawing.Point(152, 56);
			this.btnExportPhonebook.Name = "btnExportPhonebook";
			this.btnExportPhonebook.Size = new System.Drawing.Size(128, 32);
			this.btnExportPhonebook.TabIndex = 7;
			this.btnExportPhonebook.Text = "Export phonebook";
			this.btnExportPhonebook.Click += new System.EventHandler(this.btnExportPhonebook_Click);
			// 
			// btnImportPhonebook
			// 
			this.btnImportPhonebook.Location = new System.Drawing.Point(152, 96);
			this.btnImportPhonebook.Name = "btnImportPhonebook";
			this.btnImportPhonebook.Size = new System.Drawing.Size(128, 32);
			this.btnImportPhonebook.TabIndex = 8;
			this.btnImportPhonebook.Text = "Import phonebook";
			this.btnImportPhonebook.Click += new System.EventHandler(this.btnImportPhonebook_Click);
			// 
			// btnDeletePhonebookEntry
			// 
			this.btnDeletePhonebookEntry.Location = new System.Drawing.Point(8, 56);
			this.btnDeletePhonebookEntry.Name = "btnDeletePhonebookEntry";
			this.btnDeletePhonebookEntry.Size = new System.Drawing.Size(64, 36);
			this.btnDeletePhonebookEntry.TabIndex = 1;
			this.btnDeletePhonebookEntry.Text = "Delete entry";
			this.btnDeletePhonebookEntry.Click += new System.EventHandler(this.btnDeletePhonebookEntry_Click);
			// 
			// btnDeletePhonebook
			// 
			this.btnDeletePhonebook.Location = new System.Drawing.Point(8, 104);
			this.btnDeletePhonebook.Name = "btnDeletePhonebook";
			this.btnDeletePhonebook.Size = new System.Drawing.Size(128, 32);
			this.btnDeletePhonebook.TabIndex = 4;
			this.btnDeletePhonebook.Text = "Delete all entries";
			this.btnDeletePhonebook.Click += new System.EventHandler(this.btnDeletePhonebook_Click);
			// 
			// btnMsgRoutingOff
			// 
			this.btnMsgRoutingOff.Location = new System.Drawing.Point(224, 16);
			this.btnMsgRoutingOff.Name = "btnMsgRoutingOff";
			this.btnMsgRoutingOff.Size = new System.Drawing.Size(56, 48);
			this.btnMsgRoutingOff.TabIndex = 7;
			this.btnMsgRoutingOff.Text = "Disable";
			this.btnMsgRoutingOff.Click += new System.EventHandler(this.btnMsgRoutingOff_Click);
			// 
			// btnMsgRoutingOn
			// 
			this.btnMsgRoutingOn.Location = new System.Drawing.Point(152, 16);
			this.btnMsgRoutingOn.Name = "btnMsgRoutingOn";
			this.btnMsgRoutingOn.Size = new System.Drawing.Size(72, 48);
			this.btnMsgRoutingOn.TabIndex = 6;
			this.btnMsgRoutingOn.Text = "Enable message routing";
			this.btnMsgRoutingOn.Click += new System.EventHandler(this.btnMsgRoutingOn_Click);
			// 
			// btnMsgNotificationOff
			// 
			this.btnMsgNotificationOff.Location = new System.Drawing.Point(224, 72);
			this.btnMsgNotificationOff.Name = "btnMsgNotificationOff";
			this.btnMsgNotificationOff.Size = new System.Drawing.Size(56, 48);
			this.btnMsgNotificationOff.TabIndex = 9;
			this.btnMsgNotificationOff.Text = "Disable";
			this.btnMsgNotificationOff.Click += new System.EventHandler(this.btnMsgNotificationOff_Click);
			// 
			// btnMsgNotification
			// 
			this.btnMsgNotification.Location = new System.Drawing.Point(152, 72);
			this.btnMsgNotification.Name = "btnMsgNotification";
			this.btnMsgNotification.Size = new System.Drawing.Size(72, 48);
			this.btnMsgNotification.TabIndex = 8;
			this.btnMsgNotification.Text = "Enable message notifications";
			this.btnMsgNotification.Click += new System.EventHandler(this.btnMsgNotification_Click);
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(8, 96);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(128, 32);
			this.btnReset.TabIndex = 2;
			this.btnReset.Text = "Reset to default config";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// chkEnableLogging
			// 
			this.chkEnableLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkEnableLogging.Location = new System.Drawing.Point(8, 448);
			this.chkEnableLogging.Name = "chkEnableLogging";
			this.chkEnableLogging.Size = new System.Drawing.Size(112, 24);
			this.chkEnableLogging.TabIndex = 2;
			this.chkEnableLogging.Text = "Enable logging";
			this.chkEnableLogging.CheckedChanged += new System.EventHandler(this.chkEnableLogging_CheckedChanged);
			// 
			// btnIsConnected
			// 
			this.btnIsConnected.Location = new System.Drawing.Point(8, 16);
			this.btnIsConnected.Name = "btnIsConnected";
			this.btnIsConnected.Size = new System.Drawing.Size(128, 32);
			this.btnIsConnected.TabIndex = 0;
			this.btnIsConnected.Text = "Is connected?";
			this.btnIsConnected.Click += new System.EventHandler(this.btnIsConnected_Click);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabGeneral);
			this.tabControl.Controls.Add(this.tabSendSMS);
			this.tabControl.Controls.Add(this.tabManageSMS);
			this.tabControl.Controls.Add(this.tabPhonebook);
			this.tabControl.Controls.Add(this.tabSmartMessaging);
			this.tabControl.Controls.Add(this.tabSmartMessaging2);
			this.tabControl.Controls.Add(this.tabRemoting);
			this.tabControl.Controls.Add(this.tabNetwork);
			this.tabControl.Controls.Add(this.tabTextMode);
			this.tabControl.Controls.Add(this.tabCustomCommands);
			this.tabControl.Location = new System.Drawing.Point(8, 8);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(512, 240);
			this.tabControl.TabIndex = 0;
			// 
			// tabGeneral
			// 
			this.tabGeneral.Controls.Add(this.txtPin);
			this.tabGeneral.Controls.Add(this.btnEnterPin);
			this.tabGeneral.Controls.Add(this.btnGetPinStatus);
			this.tabGeneral.Controls.Add(this.btnBatteryCharge);
			this.tabGeneral.Controls.Add(this.btnSignalQuality);
			this.tabGeneral.Controls.Add(this.btnIdentify);
			this.tabGeneral.Controls.Add(this.btnIsConnected);
			this.tabGeneral.Controls.Add(this.btnReset);
			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tabGeneral.Size = new System.Drawing.Size(504, 214);
			this.tabGeneral.TabIndex = 2;
			this.tabGeneral.Text = "General";
			this.tabGeneral.UseVisualStyleBackColor = true;
			// 
			// txtPin
			// 
			this.txtPin.Location = new System.Drawing.Point(212, 63);
			this.txtPin.Name = "txtPin";
			this.txtPin.Size = new System.Drawing.Size(58, 20);
			this.txtPin.TabIndex = 7;
			this.txtPin.UseSystemPasswordChar = true;
			// 
			// btnEnterPin
			// 
			this.btnEnterPin.Location = new System.Drawing.Point(142, 56);
			this.btnEnterPin.Name = "btnEnterPin";
			this.btnEnterPin.Size = new System.Drawing.Size(64, 32);
			this.btnEnterPin.TabIndex = 6;
			this.btnEnterPin.Text = "Enter PIN";
			this.btnEnterPin.Click += new System.EventHandler(this.btnEnterPin_Click);
			// 
			// btnGetPinStatus
			// 
			this.btnGetPinStatus.Location = new System.Drawing.Point(142, 16);
			this.btnGetPinStatus.Name = "btnGetPinStatus";
			this.btnGetPinStatus.Size = new System.Drawing.Size(128, 32);
			this.btnGetPinStatus.TabIndex = 5;
			this.btnGetPinStatus.Text = "Get PIN status";
			this.btnGetPinStatus.Click += new System.EventHandler(this.btnGetPinStatus_Click);
			// 
			// btnBatteryCharge
			// 
			this.btnBatteryCharge.Location = new System.Drawing.Point(8, 176);
			this.btnBatteryCharge.Name = "btnBatteryCharge";
			this.btnBatteryCharge.Size = new System.Drawing.Size(128, 32);
			this.btnBatteryCharge.TabIndex = 4;
			this.btnBatteryCharge.Text = "Get battery charge";
			this.btnBatteryCharge.Click += new System.EventHandler(this.btnBatteryCharge_Click);
			// 
			// btnSignalQuality
			// 
			this.btnSignalQuality.Location = new System.Drawing.Point(8, 136);
			this.btnSignalQuality.Name = "btnSignalQuality";
			this.btnSignalQuality.Size = new System.Drawing.Size(128, 32);
			this.btnSignalQuality.TabIndex = 3;
			this.btnSignalQuality.Text = "Get signal quality";
			this.btnSignalQuality.Click += new System.EventHandler(this.btnSignalQuality_Click);
			// 
			// tabSendSMS
			// 
			this.tabSendSMS.Controls.Add(this.groupBox1);
			this.tabSendSMS.Controls.Add(this.btnSendMessage);
			this.tabSendSMS.Controls.Add(this.txtNumber);
			this.tabSendSMS.Controls.Add(this.txtMessage);
			this.tabSendSMS.Controls.Add(this.label2);
			this.tabSendSMS.Controls.Add(this.label1);
			this.tabSendSMS.Location = new System.Drawing.Point(4, 22);
			this.tabSendSMS.Name = "tabSendSMS";
			this.tabSendSMS.Padding = new System.Windows.Forms.Padding(3);
			this.tabSendSMS.Size = new System.Drawing.Size(504, 214);
			this.tabSendSMS.TabIndex = 4;
			this.tabSendSMS.Text = "Send SMS";
			this.tabSendSMS.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtDestinationPort);
			this.groupBox1.Controls.Add(this.chkDestinationPort);
			this.groupBox1.Controls.Add(this.chkSmsBatchMode);
			this.groupBox1.Controls.Add(this.label19);
			this.groupBox1.Controls.Add(this.txtSendTimes);
			this.groupBox1.Controls.Add(this.chkMultipleTimes);
			this.groupBox1.Controls.Add(this.chkSMSC);
			this.groupBox1.Controls.Add(this.chkUnicode);
			this.groupBox1.Controls.Add(this.chkReport);
			this.groupBox1.Controls.Add(this.chkAlert);
			this.groupBox1.Controls.Add(this.txtSMSC);
			this.groupBox1.Location = new System.Drawing.Point(8, 88);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(488, 120);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Options";
			// 
			// txtDestinationPort
			// 
			this.txtDestinationPort.Location = new System.Drawing.Point(348, 66);
			this.txtDestinationPort.Name = "txtDestinationPort";
			this.txtDestinationPort.Size = new System.Drawing.Size(48, 20);
			this.txtDestinationPort.TabIndex = 10;
			this.txtDestinationPort.Text = "0";
			// 
			// chkDestinationPort
			// 
			this.chkDestinationPort.Location = new System.Drawing.Point(232, 64);
			this.chkDestinationPort.Name = "chkDestinationPort";
			this.chkDestinationPort.Size = new System.Drawing.Size(110, 24);
			this.chkDestinationPort.TabIndex = 9;
			this.chkDestinationPort.Text = "Destination port:";
			this.chkDestinationPort.UseVisualStyleBackColor = true;
			// 
			// chkSmsBatchMode
			// 
			this.chkSmsBatchMode.Location = new System.Drawing.Point(232, 40);
			this.chkSmsBatchMode.Name = "chkSmsBatchMode";
			this.chkSmsBatchMode.Size = new System.Drawing.Size(232, 24);
			this.chkSmsBatchMode.TabIndex = 8;
			this.chkSmsBatchMode.Text = "Enable SMS batch mode";
			this.chkSmsBatchMode.UseVisualStyleBackColor = true;
			this.chkSmsBatchMode.CheckedChanged += new System.EventHandler(this.chkSmsBatchMode_CheckedChanged);
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(402, 19);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(31, 13);
			this.label19.TabIndex = 7;
			this.label19.Text = "times";
			// 
			// txtSendTimes
			// 
			this.txtSendTimes.Location = new System.Drawing.Point(348, 18);
			this.txtSendTimes.Name = "txtSendTimes";
			this.txtSendTimes.Size = new System.Drawing.Size(48, 20);
			this.txtSendTimes.TabIndex = 6;
			this.txtSendTimes.Text = "10";
			// 
			// chkMultipleTimes
			// 
			this.chkMultipleTimes.Location = new System.Drawing.Point(232, 16);
			this.chkMultipleTimes.Name = "chkMultipleTimes";
			this.chkMultipleTimes.Size = new System.Drawing.Size(104, 24);
			this.chkMultipleTimes.TabIndex = 5;
			this.chkMultipleTimes.Text = "Send message";
			this.chkMultipleTimes.UseVisualStyleBackColor = true;
			// 
			// chkSMSC
			// 
			this.chkSMSC.Location = new System.Drawing.Point(8, 16);
			this.chkSMSC.Name = "chkSMSC";
			this.chkSMSC.Size = new System.Drawing.Size(64, 24);
			this.chkSMSC.TabIndex = 0;
			this.chkSMSC.Text = "SMSC:";
			this.chkSMSC.CheckedChanged += new System.EventHandler(this.chkSMSC_CheckedChanged);
			// 
			// chkUnicode
			// 
			this.chkUnicode.Location = new System.Drawing.Point(8, 88);
			this.chkUnicode.Name = "chkUnicode";
			this.chkUnicode.Size = new System.Drawing.Size(208, 24);
			this.chkUnicode.TabIndex = 4;
			this.chkUnicode.Text = "Send as Unicode (UCS2)";
			// 
			// chkReport
			// 
			this.chkReport.Location = new System.Drawing.Point(8, 64);
			this.chkReport.Name = "chkReport";
			this.chkReport.Size = new System.Drawing.Size(208, 24);
			this.chkReport.TabIndex = 3;
			this.chkReport.Text = "Request status report";
			// 
			// chkAlert
			// 
			this.chkAlert.Location = new System.Drawing.Point(8, 40);
			this.chkAlert.Name = "chkAlert";
			this.chkAlert.Size = new System.Drawing.Size(208, 24);
			this.chkAlert.TabIndex = 2;
			this.chkAlert.Text = "Request immediate display (alert)";
			// 
			// txtSMSC
			// 
			this.txtSMSC.Enabled = false;
			this.txtSMSC.Location = new System.Drawing.Point(72, 16);
			this.txtSMSC.Name = "txtSMSC";
			this.txtSMSC.Size = new System.Drawing.Size(128, 20);
			this.txtSMSC.TabIndex = 1;
			// 
			// btnSendMessage
			// 
			this.btnSendMessage.Location = new System.Drawing.Point(216, 48);
			this.btnSendMessage.Name = "btnSendMessage";
			this.btnSendMessage.Size = new System.Drawing.Size(128, 32);
			this.btnSendMessage.TabIndex = 15;
			this.btnSendMessage.Text = "Send Message";
			this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
			// 
			// txtNumber
			// 
			this.txtNumber.Location = new System.Drawing.Point(72, 48);
			this.txtNumber.MaxLength = 30;
			this.txtNumber.Name = "txtNumber";
			this.txtNumber.Size = new System.Drawing.Size(128, 20);
			this.txtNumber.TabIndex = 3;
			this.txtNumber.Text = "+483341234567";
			// 
			// txtMessage
			// 
			this.txtMessage.Location = new System.Drawing.Point(72, 16);
			this.txtMessage.MaxLength = 160;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(256, 20);
			this.txtMessage.TabIndex = 1;
			this.txtMessage.Text = "This is a test!";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Number:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Message:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tabManageSMS
			// 
			this.tabManageSMS.Controls.Add(this.panel2);
			this.tabManageSMS.Controls.Add(this.label20);
			this.tabManageSMS.Controls.Add(this.txtNewSMSCType);
			this.tabManageSMS.Controls.Add(this.chkNewSMSCType);
			this.tabManageSMS.Controls.Add(this.btnGetSMSC);
			this.tabManageSMS.Controls.Add(this.btnSetSMSC);
			this.tabManageSMS.Controls.Add(this.txtNewSMSC);
			this.tabManageSMS.Controls.Add(this.btnMsgMemStatus);
			this.tabManageSMS.Controls.Add(this.label8);
			this.tabManageSMS.Controls.Add(this.txtDelMsgIndex);
			this.tabManageSMS.Controls.Add(this.label4);
			this.tabManageSMS.Controls.Add(this.btnReadMessages);
			this.tabManageSMS.Controls.Add(this.btnDelMessage);
			this.tabManageSMS.Controls.Add(this.btnDelAllMsgs);
			this.tabManageSMS.Controls.Add(this.btnMsgStorages);
			this.tabManageSMS.Controls.Add(this.btnMsgNotification);
			this.tabManageSMS.Controls.Add(this.btnMsgNotificationOff);
			this.tabManageSMS.Controls.Add(this.btnMsgRoutingOn);
			this.tabManageSMS.Controls.Add(this.btnMsgRoutingOff);
			this.tabManageSMS.Controls.Add(this.btnCopyMessages);
			this.tabManageSMS.Controls.Add(this.btnExportMessages);
			this.tabManageSMS.Controls.Add(this.btnImportMessages);
			this.tabManageSMS.Controls.Add(this.rbMessagePhone);
			this.tabManageSMS.Controls.Add(this.rbMessageSIM);
			this.tabManageSMS.Location = new System.Drawing.Point(4, 22);
			this.tabManageSMS.Name = "tabManageSMS";
			this.tabManageSMS.Padding = new System.Windows.Forms.Padding(3);
			this.tabManageSMS.Size = new System.Drawing.Size(504, 214);
			this.tabManageSMS.TabIndex = 0;
			this.tabManageSMS.Text = "Manage SMS";
			this.tabManageSMS.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.rbExportBinary);
			this.panel2.Controls.Add(this.rbExportText);
			this.panel2.Location = new System.Drawing.Point(430, 11);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(68, 45);
			this.panel2.TabIndex = 13;
			// 
			// rbExportBinary
			// 
			this.rbExportBinary.Checked = true;
			this.rbExportBinary.Location = new System.Drawing.Point(3, 3);
			this.rbExportBinary.Name = "rbExportBinary";
			this.rbExportBinary.Size = new System.Drawing.Size(60, 18);
			this.rbExportBinary.TabIndex = 0;
			this.rbExportBinary.TabStop = true;
			this.rbExportBinary.Text = "Binary";
			// 
			// rbExportText
			// 
			this.rbExportText.Location = new System.Drawing.Point(3, 23);
			this.rbExportText.Name = "rbExportText";
			this.rbExportText.Size = new System.Drawing.Size(60, 18);
			this.rbExportText.TabIndex = 1;
			this.rbExportText.Text = "Text";
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(429, 64);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(64, 16);
			this.label20.TabIndex = 23;
			this.label20.Text = "Binary only";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtNewSMSCType
			// 
			this.txtNewSMSCType.Location = new System.Drawing.Point(368, 192);
			this.txtNewSMSCType.Name = "txtNewSMSCType";
			this.txtNewSMSCType.Size = new System.Drawing.Size(48, 20);
			this.txtNewSMSCType.TabIndex = 19;
			this.txtNewSMSCType.Text = "145";
			// 
			// chkNewSMSCType
			// 
			this.chkNewSMSCType.Location = new System.Drawing.Point(296, 192);
			this.chkNewSMSCType.Name = "chkNewSMSCType";
			this.chkNewSMSCType.Size = new System.Drawing.Size(64, 24);
			this.chkNewSMSCType.TabIndex = 18;
			this.chkNewSMSCType.Text = "Type:";
			// 
			// btnGetSMSC
			// 
			this.btnGetSMSC.Location = new System.Drawing.Point(296, 96);
			this.btnGetSMSC.Name = "btnGetSMSC";
			this.btnGetSMSC.Size = new System.Drawing.Size(128, 32);
			this.btnGetSMSC.TabIndex = 15;
			this.btnGetSMSC.Text = "Get SMSC Address";
			this.btnGetSMSC.Click += new System.EventHandler(this.btnGetSMSC_Click);
			// 
			// btnSetSMSC
			// 
			this.btnSetSMSC.Location = new System.Drawing.Point(296, 136);
			this.btnSetSMSC.Name = "btnSetSMSC";
			this.btnSetSMSC.Size = new System.Drawing.Size(128, 32);
			this.btnSetSMSC.TabIndex = 16;
			this.btnSetSMSC.Text = "Set SMSC Address";
			this.btnSetSMSC.Click += new System.EventHandler(this.btnSetSMSC_Click);
			// 
			// txtNewSMSC
			// 
			this.txtNewSMSC.Location = new System.Drawing.Point(296, 168);
			this.txtNewSMSC.Name = "txtNewSMSC";
			this.txtNewSMSC.Size = new System.Drawing.Size(128, 20);
			this.txtNewSMSC.TabIndex = 17;
			this.txtNewSMSC.Text = "+483340815";
			// 
			// btnMsgMemStatus
			// 
			this.btnMsgMemStatus.Location = new System.Drawing.Point(152, 128);
			this.btnMsgMemStatus.Name = "btnMsgMemStatus";
			this.btnMsgMemStatus.Size = new System.Drawing.Size(128, 32);
			this.btnMsgMemStatus.TabIndex = 10;
			this.btnMsgMemStatus.Text = "Get memory status";
			this.btnMsgMemStatus.Click += new System.EventHandler(this.btnMsgMemStatus_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(80, 56);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 16);
			this.label8.TabIndex = 2;
			this.label8.Text = "Index:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtDelMsgIndex
			// 
			this.txtDelMsgIndex.Location = new System.Drawing.Point(80, 72);
			this.txtDelMsgIndex.Name = "txtDelMsgIndex";
			this.txtDelMsgIndex.Size = new System.Drawing.Size(56, 20);
			this.txtDelMsgIndex.TabIndex = 3;
			this.txtDelMsgIndex.Text = "1";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(432, 128);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 32);
			this.label4.TabIndex = 20;
			this.label4.Text = "Message storage";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// rbMessagePhone
			// 
			this.rbMessagePhone.Location = new System.Drawing.Point(432, 184);
			this.rbMessagePhone.Name = "rbMessagePhone";
			this.rbMessagePhone.Size = new System.Drawing.Size(64, 24);
			this.rbMessagePhone.TabIndex = 22;
			this.rbMessagePhone.Text = "Phone";
			// 
			// rbMessageSIM
			// 
			this.rbMessageSIM.Checked = true;
			this.rbMessageSIM.Location = new System.Drawing.Point(432, 160);
			this.rbMessageSIM.Name = "rbMessageSIM";
			this.rbMessageSIM.Size = new System.Drawing.Size(64, 24);
			this.rbMessageSIM.TabIndex = 21;
			this.rbMessageSIM.TabStop = true;
			this.rbMessageSIM.Text = "SIM";
			// 
			// tabPhonebook
			// 
			this.tabPhonebook.Controls.Add(this.btnPbMemStatus);
			this.tabPhonebook.Controls.Add(this.btnPbStorages);
			this.tabPhonebook.Controls.Add(this.label9);
			this.tabPhonebook.Controls.Add(this.txtDelPbIndex);
			this.tabPhonebook.Controls.Add(this.label7);
			this.tabPhonebook.Controls.Add(this.label6);
			this.tabPhonebook.Controls.Add(this.txtPbNumber);
			this.tabPhonebook.Controls.Add(this.txtPbName);
			this.tabPhonebook.Controls.Add(this.btnCreatePbEntry);
			this.tabPhonebook.Controls.Add(this.label5);
			this.tabPhonebook.Controls.Add(this.rbPhonebookPhone);
			this.tabPhonebook.Controls.Add(this.rbPhonebookSIM);
			this.tabPhonebook.Controls.Add(this.btnExportPhonebook);
			this.tabPhonebook.Controls.Add(this.btnImportPhonebook);
			this.tabPhonebook.Controls.Add(this.btnDumpPhonebook);
			this.tabPhonebook.Controls.Add(this.btnDeletePhonebookEntry);
			this.tabPhonebook.Controls.Add(this.btnDeletePhonebook);
			this.tabPhonebook.Location = new System.Drawing.Point(4, 22);
			this.tabPhonebook.Name = "tabPhonebook";
			this.tabPhonebook.Padding = new System.Windows.Forms.Padding(3);
			this.tabPhonebook.Size = new System.Drawing.Size(504, 214);
			this.tabPhonebook.TabIndex = 1;
			this.tabPhonebook.Text = "Manage Phonebook";
			this.tabPhonebook.UseVisualStyleBackColor = true;
			// 
			// btnPbMemStatus
			// 
			this.btnPbMemStatus.Location = new System.Drawing.Point(152, 16);
			this.btnPbMemStatus.Name = "btnPbMemStatus";
			this.btnPbMemStatus.Size = new System.Drawing.Size(128, 32);
			this.btnPbMemStatus.TabIndex = 6;
			this.btnPbMemStatus.Text = "Get memory status";
			this.btnPbMemStatus.Click += new System.EventHandler(this.btnPbMemStatus_Click);
			// 
			// btnPbStorages
			// 
			this.btnPbStorages.Location = new System.Drawing.Point(8, 144);
			this.btnPbStorages.Name = "btnPbStorages";
			this.btnPbStorages.Size = new System.Drawing.Size(128, 36);
			this.btnPbStorages.TabIndex = 5;
			this.btnPbStorages.Text = "Get supported phonebook storages";
			this.btnPbStorages.Click += new System.EventHandler(this.btnPbStorages_Click);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(80, 56);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(48, 16);
			this.label9.TabIndex = 2;
			this.label9.Text = "Index:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtDelPbIndex
			// 
			this.txtDelPbIndex.Location = new System.Drawing.Point(80, 72);
			this.txtDelPbIndex.Name = "txtDelPbIndex";
			this.txtDelPbIndex.Size = new System.Drawing.Size(56, 20);
			this.txtDelPbIndex.TabIndex = 3;
			this.txtDelPbIndex.Text = "1";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(304, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 24);
			this.label7.TabIndex = 9;
			this.label7.Text = "Name:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(304, 40);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 24);
			this.label6.TabIndex = 11;
			this.label6.Text = "Number:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtPbNumber
			// 
			this.txtPbNumber.Location = new System.Drawing.Point(368, 40);
			this.txtPbNumber.Name = "txtPbNumber";
			this.txtPbNumber.Size = new System.Drawing.Size(128, 20);
			this.txtPbNumber.TabIndex = 12;
			this.txtPbNumber.Text = "+483341234567";
			// 
			// txtPbName
			// 
			this.txtPbName.Location = new System.Drawing.Point(368, 16);
			this.txtPbName.Name = "txtPbName";
			this.txtPbName.Size = new System.Drawing.Size(128, 20);
			this.txtPbName.TabIndex = 10;
			this.txtPbName.Text = "Test dummy";
			// 
			// btnCreatePbEntry
			// 
			this.btnCreatePbEntry.Location = new System.Drawing.Point(328, 72);
			this.btnCreatePbEntry.Name = "btnCreatePbEntry";
			this.btnCreatePbEntry.Size = new System.Drawing.Size(128, 32);
			this.btnCreatePbEntry.TabIndex = 13;
			this.btnCreatePbEntry.Text = "Create new entry";
			this.btnCreatePbEntry.Click += new System.EventHandler(this.btnCreatePbEntry_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(432, 128);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 32);
			this.label5.TabIndex = 20;
			this.label5.Text = "Phonebook storage";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// rbPhonebookPhone
			// 
			this.rbPhonebookPhone.Location = new System.Drawing.Point(432, 184);
			this.rbPhonebookPhone.Name = "rbPhonebookPhone";
			this.rbPhonebookPhone.Size = new System.Drawing.Size(64, 24);
			this.rbPhonebookPhone.TabIndex = 22;
			this.rbPhonebookPhone.Text = "Phone";
			// 
			// rbPhonebookSIM
			// 
			this.rbPhonebookSIM.Checked = true;
			this.rbPhonebookSIM.Location = new System.Drawing.Point(432, 160);
			this.rbPhonebookSIM.Name = "rbPhonebookSIM";
			this.rbPhonebookSIM.Size = new System.Drawing.Size(64, 24);
			this.rbPhonebookSIM.TabIndex = 21;
			this.rbPhonebookSIM.TabStop = true;
			this.rbPhonebookSIM.Text = "SIM";
			// 
			// tabSmartMessaging
			// 
			this.tabSmartMessaging.Controls.Add(this.groupBox3);
			this.tabSmartMessaging.Controls.Add(this.groupBox2);
			this.tabSmartMessaging.Location = new System.Drawing.Point(4, 22);
			this.tabSmartMessaging.Name = "tabSmartMessaging";
			this.tabSmartMessaging.Padding = new System.Windows.Forms.Padding(3);
			this.tabSmartMessaging.Size = new System.Drawing.Size(504, 214);
			this.tabSmartMessaging.TabIndex = 3;
			this.tabSmartMessaging.Text = "Smart Messaging";
			this.tabSmartMessaging.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.lblBitmap);
			this.groupBox3.Controls.Add(this.label15);
			this.groupBox3.Controls.Add(this.lblMCC);
			this.groupBox3.Controls.Add(this.txtMNC);
			this.groupBox3.Controls.Add(this.txtMCC);
			this.groupBox3.Controls.Add(this.panel1);
			this.groupBox3.Controls.Add(this.btnSendOperatorLogo);
			this.groupBox3.Controls.Add(this.chkCreateEmptyOpLogo);
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Location = new System.Drawing.Point(8, 128);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(488, 80);
			this.groupBox3.TabIndex = 43;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Send operator logo message (number is used from above)";
			// 
			// lblBitmap
			// 
			this.lblBitmap.Location = new System.Drawing.Point(8, 16);
			this.lblBitmap.Name = "lblBitmap";
			this.lblBitmap.Size = new System.Drawing.Size(48, 23);
			this.lblBitmap.TabIndex = 38;
			this.lblBitmap.Text = "Bitmap:";
			this.lblBitmap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(224, 48);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(48, 23);
			this.label15.TabIndex = 34;
			this.label15.Text = "MNC:";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblMCC
			// 
			this.lblMCC.Location = new System.Drawing.Point(224, 24);
			this.lblMCC.Name = "lblMCC";
			this.lblMCC.Size = new System.Drawing.Size(48, 23);
			this.lblMCC.TabIndex = 32;
			this.lblMCC.Text = "MCC:";
			this.lblMCC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtMNC
			// 
			this.txtMNC.Location = new System.Drawing.Point(280, 48);
			this.txtMNC.Name = "txtMNC";
			this.txtMNC.Size = new System.Drawing.Size(40, 20);
			this.txtMNC.TabIndex = 35;
			this.txtMNC.Text = "45";
			// 
			// txtMCC
			// 
			this.txtMCC.Location = new System.Drawing.Point(280, 24);
			this.txtMCC.Name = "txtMCC";
			this.txtMCC.Size = new System.Drawing.Size(40, 20);
			this.txtMCC.TabIndex = 33;
			this.txtMCC.Text = "123";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Silver;
			this.panel1.Controls.Add(this.testBitmap);
			this.panel1.Location = new System.Drawing.Point(64, 16);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(88, 24);
			this.panel1.TabIndex = 37;
			// 
			// testBitmap
			// 
			this.testBitmap.Image = ((System.Drawing.Image)(resources.GetObject("testBitmap.Image")));
			this.testBitmap.Location = new System.Drawing.Point(8, 5);
			this.testBitmap.Name = "testBitmap";
			this.testBitmap.Size = new System.Drawing.Size(72, 14);
			this.testBitmap.TabIndex = 17;
			this.testBitmap.TabStop = false;
			this.testBitmap.Click += new System.EventHandler(this.testBitmap_Click);
			// 
			// btnSendOperatorLogo
			// 
			this.btnSendOperatorLogo.Location = new System.Drawing.Point(392, 48);
			this.btnSendOperatorLogo.Name = "btnSendOperatorLogo";
			this.btnSendOperatorLogo.Size = new System.Drawing.Size(88, 23);
			this.btnSendOperatorLogo.TabIndex = 40;
			this.btnSendOperatorLogo.Text = "Send";
			this.btnSendOperatorLogo.Click += new System.EventHandler(this.btnSendOperatorLogo_Click);
			// 
			// chkCreateEmptyOpLogo
			// 
			this.chkCreateEmptyOpLogo.Location = new System.Drawing.Point(352, 16);
			this.chkCreateEmptyOpLogo.Name = "chkCreateEmptyOpLogo";
			this.chkCreateEmptyOpLogo.Size = new System.Drawing.Size(128, 32);
			this.chkCreateEmptyOpLogo.TabIndex = 36;
			this.chkCreateEmptyOpLogo.Text = "Ignore bitmap and create empty logo";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(8, 40);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(192, 32);
			this.label14.TabIndex = 39;
			this.label14.Text = "Click bitmap to change. black&white, max 72x14 pixels.";
			this.label14.UseMnemonic = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.chkSmartUnicode);
			this.groupBox2.Controls.Add(this.txtConcatMessage);
			this.groupBox2.Controls.Add(this.btnCreateConcatSms);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.btnSendConcatSMS);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.txtConcatNumber);
			this.groupBox2.Location = new System.Drawing.Point(8, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(488, 112);
			this.groupBox2.TabIndex = 42;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Send concatenated SMS message";
			// 
			// chkSmartUnicode
			// 
			this.chkSmartUnicode.Location = new System.Drawing.Point(208, 80);
			this.chkSmartUnicode.Name = "chkSmartUnicode";
			this.chkSmartUnicode.Size = new System.Drawing.Size(72, 24);
			this.chkSmartUnicode.TabIndex = 15;
			this.chkSmartUnicode.Text = "Unicode";
			// 
			// txtConcatMessage
			// 
			this.txtConcatMessage.Location = new System.Drawing.Point(64, 16);
			this.txtConcatMessage.MaxLength = 480;
			this.txtConcatMessage.Multiline = true;
			this.txtConcatMessage.Name = "txtConcatMessage";
			this.txtConcatMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtConcatMessage.Size = new System.Drawing.Size(416, 56);
			this.txtConcatMessage.TabIndex = 12;
			this.txtConcatMessage.Text = resources.GetString("txtConcatMessage.Text");
			// 
			// btnCreateConcatSms
			// 
			this.btnCreateConcatSms.Location = new System.Drawing.Point(296, 80);
			this.btnCreateConcatSms.Name = "btnCreateConcatSms";
			this.btnCreateConcatSms.Size = new System.Drawing.Size(88, 23);
			this.btnCreateConcatSms.TabIndex = 16;
			this.btnCreateConcatSms.Text = "Create only";
			this.btnCreateConcatSms.Click += new System.EventHandler(this.btnCreateConcatSms_Click);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 80);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(56, 23);
			this.label11.TabIndex = 13;
			this.label11.Text = "Number:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnSendConcatSMS
			// 
			this.btnSendConcatSMS.Location = new System.Drawing.Point(392, 80);
			this.btnSendConcatSMS.Name = "btnSendConcatSMS";
			this.btnSendConcatSMS.Size = new System.Drawing.Size(88, 23);
			this.btnSendConcatSMS.TabIndex = 17;
			this.btnSendConcatSMS.Text = "Send";
			this.btnSendConcatSMS.Click += new System.EventHandler(this.btnSendConcatSMS_Click);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(8, 16);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(56, 24);
			this.label12.TabIndex = 11;
			this.label12.Text = "Message:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtConcatNumber
			// 
			this.txtConcatNumber.Location = new System.Drawing.Point(64, 80);
			this.txtConcatNumber.MaxLength = 30;
			this.txtConcatNumber.Name = "txtConcatNumber";
			this.txtConcatNumber.Size = new System.Drawing.Size(128, 20);
			this.txtConcatNumber.TabIndex = 14;
			this.txtConcatNumber.Text = "+483341234567";
			// 
			// tabSmartMessaging2
			// 
			this.tabSmartMessaging2.Controls.Add(this.groupBox5);
			this.tabSmartMessaging2.Location = new System.Drawing.Point(4, 22);
			this.tabSmartMessaging2.Name = "tabSmartMessaging2";
			this.tabSmartMessaging2.Padding = new System.Windows.Forms.Padding(3);
			this.tabSmartMessaging2.Size = new System.Drawing.Size(504, 214);
			this.tabSmartMessaging2.TabIndex = 9;
			this.tabSmartMessaging2.Text = "Smart Messaging 2";
			this.tabSmartMessaging2.UseVisualStyleBackColor = true;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.chkConcatAsText);
			this.groupBox5.Controls.Add(this.txtCombineSourceParts);
			this.groupBox5.Controls.Add(this.rbConcatIncoming);
			this.groupBox5.Controls.Add(this.rbConcatOutgoing);
			this.groupBox5.Controls.Add(this.btnCombineParts);
			this.groupBox5.Controls.Add(this.label22);
			this.groupBox5.Controls.Add(this.txtCombinedConcatMessage);
			this.groupBox5.Controls.Add(this.label21);
			this.groupBox5.Location = new System.Drawing.Point(6, 6);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(492, 202);
			this.groupBox5.TabIndex = 0;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Combine concatenated message parts";
			// 
			// chkConcatAsText
			// 
			this.chkConcatAsText.AutoSize = true;
			this.chkConcatAsText.Checked = true;
			this.chkConcatAsText.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkConcatAsText.Location = new System.Drawing.Point(238, 107);
			this.chkConcatAsText.Name = "chkConcatAsText";
			this.chkConcatAsText.Size = new System.Drawing.Size(105, 17);
			this.chkConcatAsText.TabIndex = 4;
			this.chkConcatAsText.Text = "Get result as text";
			this.chkConcatAsText.UseVisualStyleBackColor = true;
			// 
			// txtCombineSourceParts
			// 
			this.txtCombineSourceParts.Location = new System.Drawing.Point(68, 16);
			this.txtCombineSourceParts.Multiline = true;
			this.txtCombineSourceParts.Name = "txtCombineSourceParts";
			this.txtCombineSourceParts.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtCombineSourceParts.Size = new System.Drawing.Size(416, 82);
			this.txtCombineSourceParts.TabIndex = 1;
			this.txtCombineSourceParts.Text = resources.GetString("txtCombineSourceParts.Text");
			this.txtCombineSourceParts.WordWrap = false;
			// 
			// rbConcatIncoming
			// 
			this.rbConcatIncoming.AutoSize = true;
			this.rbConcatIncoming.Location = new System.Drawing.Point(142, 107);
			this.rbConcatIncoming.Name = "rbConcatIncoming";
			this.rbConcatIncoming.Size = new System.Drawing.Size(68, 17);
			this.rbConcatIncoming.TabIndex = 3;
			this.rbConcatIncoming.Text = "Incoming";
			this.rbConcatIncoming.UseVisualStyleBackColor = true;
			// 
			// rbConcatOutgoing
			// 
			this.rbConcatOutgoing.AutoSize = true;
			this.rbConcatOutgoing.Checked = true;
			this.rbConcatOutgoing.Location = new System.Drawing.Point(68, 107);
			this.rbConcatOutgoing.Name = "rbConcatOutgoing";
			this.rbConcatOutgoing.Size = new System.Drawing.Size(68, 17);
			this.rbConcatOutgoing.TabIndex = 2;
			this.rbConcatOutgoing.TabStop = true;
			this.rbConcatOutgoing.Text = "Outgoing";
			this.rbConcatOutgoing.UseVisualStyleBackColor = true;
			// 
			// btnCombineParts
			// 
			this.btnCombineParts.Location = new System.Drawing.Point(377, 104);
			this.btnCombineParts.Name = "btnCombineParts";
			this.btnCombineParts.Size = new System.Drawing.Size(109, 23);
			this.btnCombineParts.TabIndex = 5;
			this.btnCombineParts.Text = "Combine parts";
			this.btnCombineParts.UseVisualStyleBackColor = true;
			this.btnCombineParts.Click += new System.EventHandler(this.btnCombineParts_Click);
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(6, 133);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(56, 56);
			this.label22.TabIndex = 6;
			this.label22.Text = "Combined message:";
			// 
			// txtCombinedConcatMessage
			// 
			this.txtCombinedConcatMessage.Location = new System.Drawing.Point(68, 133);
			this.txtCombinedConcatMessage.Multiline = true;
			this.txtCombinedConcatMessage.Name = "txtCombinedConcatMessage";
			this.txtCombinedConcatMessage.ReadOnly = true;
			this.txtCombinedConcatMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtCombinedConcatMessage.Size = new System.Drawing.Size(416, 56);
			this.txtCombinedConcatMessage.TabIndex = 7;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(6, 16);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(56, 82);
			this.label21.TabIndex = 0;
			this.label21.Text = "Source parts (one per line):";
			// 
			// tabRemoting
			// 
			this.tabRemoting.Controls.Add(this.chkAllowAnonymous);
			this.tabRemoting.Controls.Add(this.chkSecureServer);
			this.tabRemoting.Controls.Add(this.groupBox4);
			this.tabRemoting.Controls.Add(this.btnStopRemotingServer);
			this.tabRemoting.Controls.Add(this.label10);
			this.tabRemoting.Controls.Add(this.txtRemotingPort);
			this.tabRemoting.Controls.Add(this.btnStartRemotingServer);
			this.tabRemoting.Location = new System.Drawing.Point(4, 22);
			this.tabRemoting.Name = "tabRemoting";
			this.tabRemoting.Padding = new System.Windows.Forms.Padding(3);
			this.tabRemoting.Size = new System.Drawing.Size(504, 214);
			this.tabRemoting.TabIndex = 7;
			this.tabRemoting.Text = "Remoting";
			this.tabRemoting.UseVisualStyleBackColor = true;
			// 
			// chkAllowAnonymous
			// 
			this.chkAllowAnonymous.AutoSize = true;
			this.chkAllowAnonymous.Enabled = false;
			this.chkAllowAnonymous.Location = new System.Drawing.Point(338, 39);
			this.chkAllowAnonymous.Name = "chkAllowAnonymous";
			this.chkAllowAnonymous.Size = new System.Drawing.Size(145, 17);
			this.chkAllowAnonymous.TabIndex = 5;
			this.chkAllowAnonymous.Text = "Allow anonymous access";
			this.chkAllowAnonymous.UseVisualStyleBackColor = true;
			// 
			// chkSecureServer
			// 
			this.chkSecureServer.AutoSize = true;
			this.chkSecureServer.Location = new System.Drawing.Point(278, 39);
			this.chkSecureServer.Name = "chkSecureServer";
			this.chkSecureServer.Size = new System.Drawing.Size(60, 17);
			this.chkSecureServer.TabIndex = 4;
			this.chkSecureServer.Text = "Secure";
			this.chkSecureServer.UseVisualStyleBackColor = true;
			this.chkSecureServer.CheckedChanged += new System.EventHandler(this.chkSecureServer_CheckedChanged);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.btnRemoteSecurity);
			this.groupBox4.Controls.Add(this.chkRemoteSecure);
			this.groupBox4.Controls.Add(this.txtRemotePort);
			this.groupBox4.Controls.Add(this.label18);
			this.groupBox4.Controls.Add(this.txtRemoteServer);
			this.groupBox4.Controls.Add(this.label17);
			this.groupBox4.Controls.Add(this.chkRemoteUnicode);
			this.groupBox4.Controls.Add(this.txtRemoteNumber);
			this.groupBox4.Controls.Add(this.btnRemoteSendSMS);
			this.groupBox4.Controls.Add(this.label16);
			this.groupBox4.Controls.Add(this.label13);
			this.groupBox4.Controls.Add(this.txtRemoteMessage);
			this.groupBox4.Location = new System.Drawing.Point(6, 54);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(492, 150);
			this.groupBox4.TabIndex = 6;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Send SMS remotely";
			// 
			// btnRemoteSecurity
			// 
			this.btnRemoteSecurity.Enabled = false;
			this.btnRemoteSecurity.Location = new System.Drawing.Point(219, 40);
			this.btnRemoteSecurity.Name = "btnRemoteSecurity";
			this.btnRemoteSecurity.Size = new System.Drawing.Size(107, 23);
			this.btnRemoteSecurity.TabIndex = 5;
			this.btnRemoteSecurity.Text = "Security settings...";
			this.btnRemoteSecurity.UseVisualStyleBackColor = true;
			this.btnRemoteSecurity.Click += new System.EventHandler(this.btnRemoteSecurity_Click);
			// 
			// chkRemoteSecure
			// 
			this.chkRemoteSecure.AutoSize = true;
			this.chkRemoteSecure.Location = new System.Drawing.Point(144, 44);
			this.chkRemoteSecure.Name = "chkRemoteSecure";
			this.chkRemoteSecure.Size = new System.Drawing.Size(60, 17);
			this.chkRemoteSecure.TabIndex = 4;
			this.chkRemoteSecure.Text = "Secure";
			this.chkRemoteSecure.UseVisualStyleBackColor = true;
			this.chkRemoteSecure.CheckedChanged += new System.EventHandler(this.chkRemoteSecure_CheckedChanged);
			// 
			// txtRemotePort
			// 
			this.txtRemotePort.Location = new System.Drawing.Point(70, 42);
			this.txtRemotePort.MaxLength = 30;
			this.txtRemotePort.Name = "txtRemotePort";
			this.txtRemotePort.Size = new System.Drawing.Size(68, 20);
			this.txtRemotePort.TabIndex = 3;
			this.txtRemotePort.Text = "2000";
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(6, 42);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(56, 23);
			this.label18.TabIndex = 2;
			this.label18.Text = "Port:";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtRemoteServer
			// 
			this.txtRemoteServer.Location = new System.Drawing.Point(70, 16);
			this.txtRemoteServer.Name = "txtRemoteServer";
			this.txtRemoteServer.Size = new System.Drawing.Size(128, 20);
			this.txtRemoteServer.TabIndex = 1;
			this.txtRemoteServer.Text = "localhost";
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(6, 16);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(56, 23);
			this.label17.TabIndex = 0;
			this.label17.Text = "Server:";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkRemoteUnicode
			// 
			this.chkRemoteUnicode.Location = new System.Drawing.Point(9, 120);
			this.chkRemoteUnicode.Name = "chkRemoteUnicode";
			this.chkRemoteUnicode.Size = new System.Drawing.Size(163, 24);
			this.chkRemoteUnicode.TabIndex = 11;
			this.chkRemoteUnicode.Text = "Send as Unicode (UCS2)";
			// 
			// txtRemoteNumber
			// 
			this.txtRemoteNumber.Location = new System.Drawing.Point(70, 94);
			this.txtRemoteNumber.MaxLength = 30;
			this.txtRemoteNumber.Name = "txtRemoteNumber";
			this.txtRemoteNumber.Size = new System.Drawing.Size(128, 20);
			this.txtRemoteNumber.TabIndex = 9;
			this.txtRemoteNumber.Text = "+483341234567";
			// 
			// btnRemoteSendSMS
			// 
			this.btnRemoteSendSMS.Location = new System.Drawing.Point(204, 94);
			this.btnRemoteSendSMS.Name = "btnRemoteSendSMS";
			this.btnRemoteSendSMS.Size = new System.Drawing.Size(128, 32);
			this.btnRemoteSendSMS.TabIndex = 10;
			this.btnRemoteSendSMS.Text = "Send Message";
			this.btnRemoteSendSMS.Click += new System.EventHandler(this.btnRemoteSendSMS_Click);
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(6, 68);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(56, 24);
			this.label16.TabIndex = 6;
			this.label16.Text = "Message:";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(6, 94);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(56, 23);
			this.label13.TabIndex = 8;
			this.label13.Text = "Number:";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtRemoteMessage
			// 
			this.txtRemoteMessage.Location = new System.Drawing.Point(70, 68);
			this.txtRemoteMessage.MaxLength = 160;
			this.txtRemoteMessage.Name = "txtRemoteMessage";
			this.txtRemoteMessage.Size = new System.Drawing.Size(256, 20);
			this.txtRemoteMessage.TabIndex = 7;
			this.txtRemoteMessage.Text = "This is a remoting test!";
			// 
			// btnStopRemotingServer
			// 
			this.btnStopRemotingServer.Location = new System.Drawing.Point(140, 16);
			this.btnStopRemotingServer.Name = "btnStopRemotingServer";
			this.btnStopRemotingServer.Size = new System.Drawing.Size(128, 32);
			this.btnStopRemotingServer.TabIndex = 1;
			this.btnStopRemotingServer.Text = "Stop server";
			this.btnStopRemotingServer.Click += new System.EventHandler(this.btnStopRemotingServer_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(275, 13);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(57, 20);
			this.label10.TabIndex = 2;
			this.label10.Text = "TCP port:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtRemotingPort
			// 
			this.txtRemotingPort.Location = new System.Drawing.Point(338, 13);
			this.txtRemotingPort.MaxLength = 5;
			this.txtRemotingPort.Name = "txtRemotingPort";
			this.txtRemotingPort.Size = new System.Drawing.Size(68, 20);
			this.txtRemotingPort.TabIndex = 3;
			this.txtRemotingPort.Text = "2000";
			// 
			// btnStartRemotingServer
			// 
			this.btnStartRemotingServer.Location = new System.Drawing.Point(6, 16);
			this.btnStartRemotingServer.Name = "btnStartRemotingServer";
			this.btnStartRemotingServer.Size = new System.Drawing.Size(128, 32);
			this.btnStartRemotingServer.TabIndex = 0;
			this.btnStartRemotingServer.Text = "Start server";
			this.btnStartRemotingServer.Click += new System.EventHandler(this.btnStartRemotingServer_Click);
			// 
			// tabNetwork
			// 
			this.tabNetwork.Controls.Add(this.btnSubscriberNumbers);
			this.tabNetwork.Controls.Add(this.btnListOperators);
			this.tabNetwork.Controls.Add(this.GetOpSelectionMode);
			this.tabNetwork.Controls.Add(this.GetOperator);
			this.tabNetwork.Location = new System.Drawing.Point(4, 22);
			this.tabNetwork.Name = "tabNetwork";
			this.tabNetwork.Padding = new System.Windows.Forms.Padding(3);
			this.tabNetwork.Size = new System.Drawing.Size(504, 214);
			this.tabNetwork.TabIndex = 6;
			this.tabNetwork.Text = "Network";
			this.tabNetwork.UseVisualStyleBackColor = true;
			// 
			// btnSubscriberNumbers
			// 
			this.btnSubscriberNumbers.Location = new System.Drawing.Point(8, 142);
			this.btnSubscriberNumbers.Name = "btnSubscriberNumbers";
			this.btnSubscriberNumbers.Size = new System.Drawing.Size(128, 36);
			this.btnSubscriberNumbers.TabIndex = 3;
			this.btnSubscriberNumbers.Text = "Get subscriber numbers";
			this.btnSubscriberNumbers.Click += new System.EventHandler(this.btnSubscriberNumbers_Click);
			// 
			// btnListOperators
			// 
			this.btnListOperators.Location = new System.Drawing.Point(8, 100);
			this.btnListOperators.Name = "btnListOperators";
			this.btnListOperators.Size = new System.Drawing.Size(128, 36);
			this.btnListOperators.TabIndex = 2;
			this.btnListOperators.Text = "List operators";
			this.btnListOperators.Click += new System.EventHandler(this.btnListOperators_Click);
			// 
			// GetOpSelectionMode
			// 
			this.GetOpSelectionMode.Location = new System.Drawing.Point(8, 58);
			this.GetOpSelectionMode.Name = "GetOpSelectionMode";
			this.GetOpSelectionMode.Size = new System.Drawing.Size(128, 36);
			this.GetOpSelectionMode.TabIndex = 1;
			this.GetOpSelectionMode.Text = "Get operator selection mode";
			this.GetOpSelectionMode.Click += new System.EventHandler(this.GetOpSelectionMode_Click);
			// 
			// GetOperator
			// 
			this.GetOperator.Location = new System.Drawing.Point(8, 16);
			this.GetOperator.Name = "GetOperator";
			this.GetOperator.Size = new System.Drawing.Size(128, 36);
			this.GetOperator.TabIndex = 0;
			this.GetOperator.Text = "Get current operator";
			this.GetOperator.Click += new System.EventHandler(this.GetOperator_Click);
			// 
			// tabTextMode
			// 
			this.tabTextMode.Controls.Add(this.label3);
			this.tabTextMode.Controls.Add(this.txtCharset);
			this.tabTextMode.Controls.Add(this.btnSelectCharset);
			this.tabTextMode.Controls.Add(this.btnCharsets);
			this.tabTextMode.Controls.Add(this.btnGetCharset);
			this.tabTextMode.Location = new System.Drawing.Point(4, 22);
			this.tabTextMode.Name = "tabTextMode";
			this.tabTextMode.Padding = new System.Windows.Forms.Padding(3);
			this.tabTextMode.Size = new System.Drawing.Size(504, 214);
			this.tabTextMode.TabIndex = 5;
			this.tabTextMode.Text = "Text Mode";
			this.tabTextMode.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(151, 100);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 16);
			this.label3.TabIndex = 3;
			this.label3.Text = "Character set:";
			// 
			// txtCharset
			// 
			this.txtCharset.Location = new System.Drawing.Point(151, 116);
			this.txtCharset.MaxLength = 30;
			this.txtCharset.Name = "txtCharset";
			this.txtCharset.Size = new System.Drawing.Size(128, 20);
			this.txtCharset.TabIndex = 4;
			this.txtCharset.Text = "8859-1";
			// 
			// btnSelectCharset
			// 
			this.btnSelectCharset.Location = new System.Drawing.Point(8, 100);
			this.btnSelectCharset.Name = "btnSelectCharset";
			this.btnSelectCharset.Size = new System.Drawing.Size(128, 36);
			this.btnSelectCharset.TabIndex = 2;
			this.btnSelectCharset.Text = "Select character set";
			this.btnSelectCharset.Click += new System.EventHandler(this.btnSelectCharset_Click);
			// 
			// btnCharsets
			// 
			this.btnCharsets.Location = new System.Drawing.Point(8, 58);
			this.btnCharsets.Name = "btnCharsets";
			this.btnCharsets.Size = new System.Drawing.Size(128, 36);
			this.btnCharsets.TabIndex = 1;
			this.btnCharsets.Text = "Get supported character sets";
			this.btnCharsets.Click += new System.EventHandler(this.btnCharsets_Click);
			// 
			// btnGetCharset
			// 
			this.btnGetCharset.Location = new System.Drawing.Point(8, 16);
			this.btnGetCharset.Name = "btnGetCharset";
			this.btnGetCharset.Size = new System.Drawing.Size(128, 36);
			this.btnGetCharset.TabIndex = 0;
			this.btnGetCharset.Text = "Get current character set";
			this.btnGetCharset.Click += new System.EventHandler(this.btnGetCharset_Click);
			// 
			// tabCustomCommands
			// 
			this.tabCustomCommands.Controls.Add(this.gbProtocolCommands);
			this.tabCustomCommands.Controls.Add(this.btnReleaseProtocol);
			this.tabCustomCommands.Controls.Add(this.btnGetProtocol);
			this.tabCustomCommands.Location = new System.Drawing.Point(4, 22);
			this.tabCustomCommands.Name = "tabCustomCommands";
			this.tabCustomCommands.Padding = new System.Windows.Forms.Padding(3);
			this.tabCustomCommands.Size = new System.Drawing.Size(504, 214);
			this.tabCustomCommands.TabIndex = 8;
			this.tabCustomCommands.Text = "Custom Commands";
			this.tabCustomCommands.UseVisualStyleBackColor = true;
			// 
			// gbProtocolCommands
			// 
			this.gbProtocolCommands.Controls.Add(this.btnExecuteCommand);
			this.gbProtocolCommands.Controls.Add(this.lblError);
			this.gbProtocolCommands.Controls.Add(this.txtError);
			this.gbProtocolCommands.Controls.Add(this.lblPattern);
			this.gbProtocolCommands.Controls.Add(this.lblData);
			this.gbProtocolCommands.Controls.Add(this.txtPattern);
			this.gbProtocolCommands.Controls.Add(this.txtData);
			this.gbProtocolCommands.Controls.Add(this.lstProtocolCommands);
			this.gbProtocolCommands.Location = new System.Drawing.Point(8, 58);
			this.gbProtocolCommands.Name = "gbProtocolCommands";
			this.gbProtocolCommands.Size = new System.Drawing.Size(490, 150);
			this.gbProtocolCommands.TabIndex = 18;
			this.gbProtocolCommands.TabStop = false;
			this.gbProtocolCommands.Text = "Protocol commands";
			// 
			// btnExecuteCommand
			// 
			this.btnExecuteCommand.Location = new System.Drawing.Point(314, 104);
			this.btnExecuteCommand.Name = "btnExecuteCommand";
			this.btnExecuteCommand.Size = new System.Drawing.Size(128, 36);
			this.btnExecuteCommand.TabIndex = 24;
			this.btnExecuteCommand.Text = "Execute";
			this.btnExecuteCommand.UseVisualStyleBackColor = true;
			this.btnExecuteCommand.Click += new System.EventHandler(this.btnExecuteCommand_Click);
			// 
			// lblError
			// 
			this.lblError.AutoSize = true;
			this.lblError.Location = new System.Drawing.Point(251, 74);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(32, 13);
			this.lblError.TabIndex = 23;
			this.lblError.Text = "Error:";
			// 
			// txtError
			// 
			this.txtError.Location = new System.Drawing.Point(301, 71);
			this.txtError.MaxLength = 0;
			this.txtError.Name = "txtError";
			this.txtError.Size = new System.Drawing.Size(183, 20);
			this.txtError.TabIndex = 22;
			// 
			// lblPattern
			// 
			this.lblPattern.AutoSize = true;
			this.lblPattern.Location = new System.Drawing.Point(251, 48);
			this.lblPattern.Name = "lblPattern";
			this.lblPattern.Size = new System.Drawing.Size(44, 13);
			this.lblPattern.TabIndex = 21;
			this.lblPattern.Text = "Pattern:";
			// 
			// lblData
			// 
			this.lblData.AutoSize = true;
			this.lblData.Location = new System.Drawing.Point(251, 22);
			this.lblData.Name = "lblData";
			this.lblData.Size = new System.Drawing.Size(33, 13);
			this.lblData.TabIndex = 20;
			this.lblData.Text = "Data:";
			// 
			// txtPattern
			// 
			this.txtPattern.Location = new System.Drawing.Point(301, 45);
			this.txtPattern.MaxLength = 0;
			this.txtPattern.Name = "txtPattern";
			this.txtPattern.Size = new System.Drawing.Size(183, 20);
			this.txtPattern.TabIndex = 19;
			// 
			// txtData
			// 
			this.txtData.Location = new System.Drawing.Point(301, 19);
			this.txtData.MaxLength = 0;
			this.txtData.Name = "txtData";
			this.txtData.Size = new System.Drawing.Size(183, 20);
			this.txtData.TabIndex = 18;
			// 
			// lstProtocolCommands
			// 
			this.lstProtocolCommands.FormattingEnabled = true;
			this.lstProtocolCommands.Location = new System.Drawing.Point(6, 19);
			this.lstProtocolCommands.Name = "lstProtocolCommands";
			this.lstProtocolCommands.Size = new System.Drawing.Size(228, 121);
			this.lstProtocolCommands.TabIndex = 12;
			this.lstProtocolCommands.SelectedIndexChanged += new System.EventHandler(this.lstCustomCommands_SelectedIndexChanged);
			// 
			// btnReleaseProtocol
			// 
			this.btnReleaseProtocol.Location = new System.Drawing.Point(142, 16);
			this.btnReleaseProtocol.Name = "btnReleaseProtocol";
			this.btnReleaseProtocol.Size = new System.Drawing.Size(128, 36);
			this.btnReleaseProtocol.TabIndex = 1;
			this.btnReleaseProtocol.Text = "Release Protocol";
			this.btnReleaseProtocol.UseVisualStyleBackColor = true;
			this.btnReleaseProtocol.Click += new System.EventHandler(this.btnReleaseProtocol_Click);
			// 
			// btnGetProtocol
			// 
			this.btnGetProtocol.Location = new System.Drawing.Point(8, 16);
			this.btnGetProtocol.Name = "btnGetProtocol";
			this.btnGetProtocol.Size = new System.Drawing.Size(128, 36);
			this.btnGetProtocol.TabIndex = 0;
			this.btnGetProtocol.Text = "Get Protocol";
			this.btnGetProtocol.UseVisualStyleBackColor = true;
			this.btnGetProtocol.Click += new System.EventHandler(this.btnGetProtocol_Click);
			// 
			// txtClearOutput
			// 
			this.txtClearOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txtClearOutput.Location = new System.Drawing.Point(432, 448);
			this.txtClearOutput.Name = "txtClearOutput";
			this.txtClearOutput.Size = new System.Drawing.Size(88, 23);
			this.txtClearOutput.TabIndex = 3;
			this.txtClearOutput.Text = "Clear Output";
			this.txtClearOutput.Click += new System.EventHandler(this.txtClearOutput_Click);
			// 
			// lblNotConnected
			// 
			this.lblNotConnected.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblNotConnected.BackColor = System.Drawing.Color.White;
			this.lblNotConnected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblNotConnected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNotConnected.ForeColor = System.Drawing.Color.Red;
			this.lblNotConnected.Location = new System.Drawing.Point(166, 448);
			this.lblNotConnected.Name = "lblNotConnected";
			this.lblNotConnected.Size = new System.Drawing.Size(196, 23);
			this.lblNotConnected.TabIndex = 28;
			this.lblNotConnected.Text = "NO PHONE CONNECTED";
			this.lblNotConnected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblNotConnected.UseMnemonic = false;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "Bitmaps (*.bmp, *.jpg, *.gif)|*.bmp;*.jpg;*.gif|All files (*.*)|*.*";
			this.openFileDialog1.Title = "Load bitmap";
			// 
			// frmDemo
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 478);
			this.Controls.Add(this.lblNotConnected);
			this.Controls.Add(this.txtClearOutput);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.chkEnableLogging);
			this.Controls.Add(this.txtOutput);
			this.Name = "frmDemo";
			this.Text = "GSMComm Demo";
			this.Load += new System.EventHandler(this.frmDemo_Load);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmDemo_Closing);
			this.tabControl.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
			this.tabGeneral.PerformLayout();
			this.tabSendSMS.ResumeLayout(false);
			this.tabSendSMS.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabManageSMS.ResumeLayout(false);
			this.tabManageSMS.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.tabPhonebook.ResumeLayout(false);
			this.tabPhonebook.PerformLayout();
			this.tabSmartMessaging.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.testBitmap)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabSmartMessaging2.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.tabRemoting.ResumeLayout(false);
			this.tabRemoting.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.tabNetwork.ResumeLayout(false);
			this.tabTextMode.ResumeLayout(false);
			this.tabTextMode.PerformLayout();
			this.tabCustomCommands.ResumeLayout(false);
			this.gbProtocolCommands.ResumeLayout(false);
			this.gbProtocolCommands.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.DoEvents();
			Application.Run(new frmDemo());
		}

		private void frmDemo_Load(object sender, System.EventArgs e)
		{
			// Prompt user for connection settings
			string portName = GsmCommMain.DefaultPortName;
			int baudRate = GsmCommMain.DefaultBaudRate;
			int timeout = GsmCommMain.DefaultTimeout;
			frmConnection dlg = new frmConnection();
			dlg.StartPosition = FormStartPosition.CenterScreen;
			dlg.SetData(portName, baudRate, timeout);
			if (dlg.ShowDialog(this) == DialogResult.OK)
				dlg.GetData(out portName, out baudRate, out timeout);
			else
			{
				Close();
				return;
			}

			Cursor.Current = Cursors.WaitCursor;
			comm = new GsmCommMain(portName, baudRate, timeout);
			Cursor.Current = Cursors.Default;
			comm.PhoneConnected += new EventHandler(comm_PhoneConnected);
			comm.PhoneDisconnected += new EventHandler(comm_PhoneDisconnected);

			bool retry;
			do
			{
				retry = false;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					comm.Open();
					Cursor.Current = Cursors.Default;
				}
				catch(Exception)
				{
					Cursor.Current = Cursors.Default;
					if (MessageBox.Show(this, "Unable to open the port.", "Error",
						MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
						retry = true;
					else
					{
						Close();
						return;
					}
				}
			}
			while(retry);

			// Add custom commands
			ProtocolCommand[] commands = new ProtocolCommand[]
			{
				new ProtocolCommand("Send", true, false, false), // NeedsData
				new ProtocolCommand("Receive", false, false, false),
				new ProtocolCommand("ExecCommand", true, false, false), // NeedsData
				new ProtocolCommand("ExecCommand2", true, false, true), // NeedsData, NeedsError
				new ProtocolCommand("ExecAndReceiveMultiple", true, false, false), // NeedsData
				new ProtocolCommand("ExecAndReceiveAnything", true, true, false), // NeedsData, NeedsPattern
				new ProtocolCommand("ReceiveMultiple", false, false, false),
				new ProtocolCommand("ReceiveAnyhing", false, true, false) // NeedsPattern
			};
			lstProtocolCommands.DataSource = commands;
			lstProtocolCommands.DisplayMember = "Name";
			lstProtocolCommands.ValueMember = "";
			this.protocolLevel = null;
			this.btnExecuteCommand.Enabled = false;
		}

		private void frmDemo_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Kill SMS server if running
			DestroySmsServer();

			// Clean up comm object
			if (comm != null)
			{
				// Unregister events
				if (chkEnableLogging.Checked)
				{
					comm.LoglineAdded -= new LoglineAddedEventHandler(comm_LoglineAdded);
					chkEnableLogging.Checked = false;
				}
				comm.PhoneConnected -= new EventHandler(comm_PhoneConnected);
				comm.PhoneDisconnected -= new EventHandler(comm_PhoneDisconnected);
				if (registerMessageReceived)
				{
					comm.MessageReceived -= new MessageReceivedEventHandler(comm_MessageReceived);
					registerMessageReceived = false;
				}

				// Close connection to phone
				if (comm != null && comm.IsOpen())
					comm.Close();

				comm = null;
			}
		}

		private void Output(string text)
		{
			if (this.txtOutput.InvokeRequired)
			{
				SetTextCallback stc = new SetTextCallback(Output);
				this.Invoke(stc, new object[] { text });
			}
			else
			{
				txtOutput.AppendText(text);
				txtOutput.AppendText("\r\n");
			}
		}

		private void Output(string text, params object[] args)
		{
			string msg = string.Format(text, args);
			Output(msg);
		}

		private void ShowException(Exception ex)
		{
			Output("Error: " + ex.Message + " (" + ex.GetType().ToString() + ")");
			Output("");
		}

		private void ShowException(Exception ex, string messagePrefix)
		{
			Output(messagePrefix + " " + ex.Message + " (" + ex.GetType().ToString() + ")");
			Output("");
		}

		private bool Confirmed()
		{
			return (MessageBox.Show(this, "Really?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
				== DialogResult.Yes);
		}

		private string GetMessageStorage()
		{
			string storage = string.Empty;
			if (rbMessageSIM.Checked)
				storage = PhoneStorageType.Sim;
			if (rbMessagePhone.Checked)
				storage = PhoneStorageType.Phone;

			if (storage.Length == 0)
				throw new ApplicationException("Unknown message storage.");
			else
				return storage;
		}

		private string GetPhonebookStorage()
		{
			string storage = string.Empty;
			if (rbPhonebookSIM.Checked)
				storage = PhoneStorageType.Sim;
			if (rbPhonebookPhone.Checked)
				storage = PhoneStorageType.Phone;

			if (storage.Length == 0)
				throw new ApplicationException("Unknown phonebook storage.");
			else
				return storage;
		}

		#region General
		private void btnIdentify_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				IdentificationInfo info = comm.IdentifyDevice();
				Output("Manufacturer: " + info.Manufacturer);
				Output("Model: " + info.Model);
				Output("Revision: " + info.Revision);
				Output("Serial number: " + info.SerialNumber);
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Reset to default configuration
				comm.ResetToDefaultConfig();
				Output("Config reset");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}
	
		private void btnIsConnected_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				if (!comm.IsConnected())
					Output("Phone is NOT connected.");
				else
					Output("Phone is connected.");

				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnSignalQuality_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Get signal quality
				SignalQualityInfo info = comm.GetSignalQuality();
				Output("Signal strength: " + info.SignalStrength.ToString());
				Output("Bit error rate: " + info.BitErrorRate.ToString());
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnBatteryCharge_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Get battery charge
				BatteryChargeInfo info = comm.GetBatteryCharge();
				Output("Battery charging status: " + info.BatteryChargingStatus.ToString());
				Output("Battery charge level: " + info.BatteryChargeLevel.ToString());
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnGetPinStatus_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Get PIN status
				PinStatus status = comm.GetPinStatus();
				Output("PIN status: " + status.ToString());
				Output("");
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnEnterPin_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Enter PIN
				comm.EnterPin(txtPin.Text);
				Output("PIN entry OK.");
				Output("");
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		#endregion

		#region Send SMS

		private void chkSMSC_CheckedChanged(object sender, System.EventArgs e)
		{
			txtSMSC.Enabled = chkSMSC.Checked;
			if (chkSMSC.Checked && txtSMSC.Text.Length == 0)
				txtSMSC.Focus();
		}

		private void chkSmsBatchMode_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkSmsBatchMode.Checked)
				chkMultipleTimes.Checked = true;
		}

		private void btnSendMessage_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Send an SMS message
				SmsSubmitPdu pdu;
				if (!chkUnicode.Checked)
				{
					// Send message in the default format
					pdu = new SmsSubmitPdu(txtMessage.Text, txtNumber.Text);
				}
				else
				{
					// Send message in Unicode format
					byte dcs = (byte)DataCodingScheme.GeneralCoding.Alpha16Bit;
					pdu = new SmsSubmitPdu(txtMessage.Text, txtNumber.Text, dcs);
				}

				// Request immediate display (alert)
				if (chkAlert.Checked)
					pdu.DataCodingScheme |= (byte)DataCodingScheme.GeneralCoding.Class0;

				// Send message to a destination port
				if (chkDestinationPort.Checked)
				{
					ushort destinationPort = ushort.Parse(txtDestinationPort.Text);
					byte[] userDataHeader = SmartMessageFactory.CreatePortAddressHeader(destinationPort);
					pdu.AddUserDataHeader(userDataHeader);
				}

				// Use an explicit SMSC if this is set
				if (chkSMSC.Checked)
					pdu.SmscAddress = txtSMSC.Text;

				// If a status report should be generated, set that here
				if (chkReport.Checked)
					pdu.RequestStatusReport = true;

				// Send the same message multiple times if this is set
				int times = chkMultipleTimes.Checked ? int.Parse(txtSendTimes.Text) : 1;

				// If SMS batch mode should be activated, do it immediately before sending the first message
				if (chkSmsBatchMode.Checked)
					comm.EnableTemporarySmsBatchMode();

				// Send the message the specified number of times
				for (int i = 0; i < times; i++)
				{
					comm.SendMessage(pdu);
					Output("Message {0} of {1} sent.", i+1, times);
					Output("");
				}
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}
		#endregion

		#region Manage SMS
		private void btnReadMessages_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetMessageStorage();

			try
			{
				// Read all SMS messages from the storage
				DecodedShortMessage[] messages = comm.ReadMessages(PhoneMessageStatus.All, storage);
				foreach(DecodedShortMessage message in messages)
				{
					Output(string.Format("Message status = {0}, Location = {1}/{2}",
						StatusToString(message.Status),	message.Storage, message.Index));
					ShowMessage(message.Data);
					Output("");
				}
				Output(string.Format("{0,9} messages read.", messages.Length.ToString()));
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnDelMessage_Click(object sender, System.EventArgs e)
		{
			int index;
			try
			{
				index = int.Parse(txtDelMsgIndex.Text);
			}
			catch(Exception ex)
			{
				ShowException(ex);
				return;
			}

			if (!Confirmed()) return;
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetMessageStorage();
			try
			{
				// Delete the message with the specified index from storage
				comm.DeleteMessage(index, storage);
				Output("Message deleted.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnMsgStorages_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Get the supported short message storages
				MessageStorageInfo storages = comm.GetMessageStorages();
				Output("Supported read storages: " + MakeArrayString(storages.ReadStorages));
				Output("Supported write storages: " + MakeArrayString(storages.WriteStorages));
				Output("Supported receive storages: " + MakeArrayString(storages.ReceiveStorages));
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnDelAllMsgs_Click(object sender, System.EventArgs e)
		{
			if (!Confirmed()) return;
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetMessageStorage();
			try
			{
				// Delete all messages from phone memory
				comm.DeleteMessages(DeleteScope.All, storage);
				Output("Messages deleted.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnCopyMessages_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				Cursor.Current = Cursors.WaitCursor;

				// Copy all messages from SIM memory to phone memory
				ShortMessageFromPhone[] messages = comm.ReadRawMessages(PhoneMessageStatus.All, PhoneStorageType.Sim);
				Output(messages.Length.ToString() + " messages read.");
				foreach(ShortMessageFromPhone msg in messages)
				{
					int index = comm.WriteRawMessage(msg, PhoneStorageType.Phone);
					Output("Message copied from SIM index " + msg.Index.ToString() +
						" to phone index " + index.ToString() + ".");
				}
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnExportMessages_Click(object sender, System.EventArgs e)
		{
			if (rbExportBinary.Checked)
				ExportMessagesAsBinary();
			else if (rbExportText.Checked)
				ExportMessagesAsText();
			else
				Output("Unknown export method.");
		}

		private void ExportMessagesAsBinary()
		{
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetMessageStorage();
			try
			{
				// Read and serialize all short messages from storage into an XML file.
				ShortMessage[] messages = comm.ReadRawMessages(PhoneMessageStatus.All, storage);
				Output(messages.Length.ToString() + " message(s) read.");

				const string filename = @"C:\messages.xml";
				Output("Exporting the messages to " + filename + "...");
				System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(
					typeof(ShortMessageFromPhone[]));
				System.IO.StreamWriter sw = System.IO.File.CreateText(filename);
				ser.Serialize(sw, messages);
				sw.Close();
				Output("Done.");
				Output("");
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void ExportMessagesAsText()
		{
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetMessageStorage();
			try
			{
				// Read and serialize all short messages from storage into an XML file.
				DecodedShortMessage[] messages = comm.ReadMessages(PhoneMessageStatus.All, storage);
				Output(messages.Length.ToString() + " message(s) read.");

				const string filename = @"C:\messages_text.xml";
				Output("Exporting the messages to " + filename + "...");
				System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(filename, System.Text.Encoding.UTF8);
				// Change default writer settings to produce readable output
				xmlWriter.Formatting = System.Xml.Formatting.Indented;
				xmlWriter.Indentation = 4;

				xmlWriter.WriteStartDocument();
				xmlWriter.WriteStartElement("ShortMessages");
				foreach (DecodedShortMessage message in messages)
				{
					xmlWriter.WriteStartElement("Message");
					string sender = string.Empty;
					string timestamp = string.Empty;
					string text = message.Data.UserDataText;
					if (message.Data is SmsDeliverPdu)
					{
						SmsDeliverPdu deliver = (SmsDeliverPdu)message.Data;
						sender = deliver.OriginatingAddress;
						timestamp = deliver.SCTimestamp.ToSortableString();
					}
					xmlWriter.WriteElementString("Sender", sender);
					xmlWriter.WriteElementString("Timestamp", timestamp);
					xmlWriter.WriteElementString("Text", text);
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndDocument();
				xmlWriter.Close();
				Output("Done.");
				Output("");
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnImportMessages_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetMessageStorage();
			try
			{
				// Load and copy all saved short messages to storage
				const string filename = @"C:\messages.xml";
				Output("Importing messages from " + filename + "...");
				System.IO.StreamReader sr = System.IO.File.OpenText(filename);
				System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(ShortMessageFromPhone[]));
				object temp = ser.Deserialize(sr);
				sr.Close();
				ShortMessageFromPhone[] messages = (ShortMessageFromPhone[])temp;
				foreach(ShortMessageFromPhone msg in messages)
				{
					int index = comm.WriteRawMessage(msg, storage);
					Output("Saved message to storage index " + index.ToString() + ".");
				}
				Output("Done.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnMsgNotification_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Enable notifications about new received messages
				if (!registerMessageReceived)
				{
					comm.MessageReceived += new MessageReceivedEventHandler(comm_MessageReceived);
					registerMessageReceived = true;
				}
				comm.EnableMessageNotifications();
				Output("Message notifications activated.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnMsgNotificationOff_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Disable message notifications
				comm.DisableMessageNotifications();
				if (registerMessageReceived)
				{
					comm.MessageReceived -= new MessageReceivedEventHandler(comm_MessageReceived);
					registerMessageReceived = false;
				}
				Output("Message notifications deactivated.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnMsgRoutingOn_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Enable direct message routing to the application
				if (!registerMessageReceived)
				{
					comm.MessageReceived += new MessageReceivedEventHandler(comm_MessageReceived);
					registerMessageReceived = true;
				}
				comm.EnableMessageRouting();
				Output("Message routing activated.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnMsgRoutingOff_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Disable message routing
				comm.DisableMessageRouting();
				if (registerMessageReceived)
				{
					comm.MessageReceived -= new MessageReceivedEventHandler(comm_MessageReceived);
					registerMessageReceived = false;
				}
				Output("Message routing deactivated.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnMsgMemStatus_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetMessageStorage();
			try
			{
				// Get memory status of the storage
				MemoryStatus status = comm.GetMessageMemoryStatus(storage);
				int percentUsed = (status.Total > 0) ? (int)((double)status.Used/(double)status.Total*100) : 0;
				Output(string.Format("Message memory status: {0} of {1} used ({2}% used, {3}% free)",
					status.Used, status.Total, percentUsed, 100-percentUsed));
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnGetSMSC_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				// Get the SMSC address
				Cursor.Current = Cursors.WaitCursor;
				AddressData data = comm.GetSmscAddress();
				Output("SMSC address: " + data.Address);
				Output("Address type: " + data.TypeOfAddress.ToString());
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnSetSMSC_Click(object sender, System.EventArgs e)
		{
			// Get the address
			string address = txtNewSMSC.Text;

			// Get also the type, if selected
			int typeOfAddress = 0;
			if (chkNewSMSCType.Checked)
			{
				try
				{
					typeOfAddress = int.Parse(txtNewSMSCType.Text);
				}
				catch(Exception ex)
				{
					ShowException(ex);
					return;
				}
			}

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Set the SMSC address
				if (chkNewSMSCType.Checked)
					comm.SetSmscAddress(new AddressData(address, typeOfAddress));
				else
					comm.SetSmscAddress(address);

				Output("New SMSC address set.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		#endregion

		#region Manage phonebook

		private void btnExportPhonebook_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetPhonebookStorage();
			try
			{
				// Read and serialize all phonebook entries from storage into an XML file.
				PhonebookEntry[] entries = comm.GetPhonebook(storage);
				Output(entries.Length.ToString() + " entries read.");
			
				const string filename = @"C:\phonebook.xml";
				Output("Exporting the phonebook entries to " + filename + "...");
				System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(PhonebookEntry[]));
				System.IO.StreamWriter sw = System.IO.File.CreateText(filename);
				ser.Serialize(sw, entries);
				sw.Close();
				Output("Done.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnImportPhonebook_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetPhonebookStorage();
			try
			{
				// Load and copy all saved phonebook entries to storage.
				const string filename = @"C:\phonebook.xml";
				Output("Importing phonebook entries from " + filename + "...");
				System.IO.StreamReader sr = System.IO.File.OpenText(filename);
				System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(PhonebookEntry[]));
				object temp = ser.Deserialize(sr);
				sr.Close();
				PhonebookEntry[] entries = (PhonebookEntry[])temp;
				Output(entries.Length.ToString() + " entries to create.");
				foreach(PhonebookEntry entry in entries)
				{
					comm.CreatePhonebookEntry(entry, storage);
					Output("Entry \"" + entry.Text + "\" created.");
				}
				Output("Done.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnDumpPhonebook_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetPhonebookStorage();
			try
			{
				// Read the whole phonebook from the storage
				PhonebookEntry[] entries = comm.GetPhonebook(storage);
				if (entries.Length > 0)
				{
					// Display the entries read
					foreach(PhonebookEntry entry in entries)
					{
						Output(string.Format("{0,-20}{1}", entry.Number, entry.Text));
					}
				}
				Output(string.Format("{0,9} entries read.", entries.Length.ToString()));
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnDeletePhonebookEntry_Click(object sender, System.EventArgs e)
		{
			int index;
			try
			{
				index = int.Parse(txtDelPbIndex.Text);
			}
			catch(Exception ex)
			{
				ShowException(ex);
				return;
			}

			if (!Confirmed()) return;
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetPhonebookStorage();
			try
			{
				// Delete the phonebook entry with the specified index from storage
				comm.DeletePhonebookEntry(index, storage);
				Output("Entry deleted.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnDeletePhonebook_Click(object sender, System.EventArgs e)
		{
			if (!Confirmed()) return;
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetPhonebookStorage();
			try
			{
				// Delete all phonebook entries from storage
				comm.DeleteAllPhonebookEntries(storage);
				Output("Phonebook deleted.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnCreatePbEntry_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetPhonebookStorage();
			try
			{
				// Create the phonebook entry in storage
				comm.CreatePhonebookEntry(new PhonebookEntry(0, txtPbNumber.Text,
					txtPbNumber.Text.StartsWith("+") ? AddressType.InternationalPhone : AddressType.UnknownPhone,
					txtPbName.Text), storage);
				Output("Phonebook created.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnPbStorages_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Get the supported phonebook storages
				string[] storages = comm.GetPhonebookStorages();
				Output("Supported phonebook storages: " + MakeArrayString(storages));
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnPbMemStatus_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string storage = GetPhonebookStorage();
			try
			{
				// Get memory status of the phonebook storage
				MemoryStatus status = comm.GetPhonebookMemoryStatus(storage);
				int percentUsed = (status.Total > 0) ? (int)((double)status.Used/(double)status.Total*100) : 0;
				Output(string.Format("Phonebook memory status: {0} of {1} used ({2}% used, {3}% free)",
					status.Used, status.Total, percentUsed, 100-percentUsed));
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		#endregion

		#region Smart Messaging

		private OutgoingSmsPdu[] CreateConcatMessage(string message, string number, bool unicode, bool showParts)
		{
			OutgoingSmsPdu[] pdus = null;
			try
			{
				if (!unicode)
				{
					Output("Creating concatenated message.");
					pdus = SmartMessageFactory.CreateConcatTextMessage(message, number);
				}
				else
				{
					Output("Creating concatenated Unicode message.");
					pdus = SmartMessageFactory.CreateConcatTextMessage(message, true, number);
				}
			}
			catch(Exception ex)
			{
				ShowException(ex);
				return null;
			}

			if (pdus.Length == 0)
			{
				Output("Error: No PDU parts have been created!");
				Output("");
				return null;
			}
			else
			{
				if (showParts)
				{
					for (int i = 0; i < pdus.Length; i++)
					{
						Output("Part #" + (i + 1).ToString() + ": " + Environment.NewLine + pdus[i].ToString());
					}
				}
				Output(pdus.Length.ToString() + " message part(s) created.");
				Output("");
			}

			return pdus;
		}

		private void SendMultiple(OutgoingSmsPdu[] pdus)
		{
			int num = pdus.Length;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				// Send the created messages
				int i=0;
				foreach(OutgoingSmsPdu pdu in pdus)
				{
					i++;
					Output("Sending message " + i.ToString() + " of " + num.ToString() + "...");
					comm.SendMessage(pdu);
				}
				Output("Done.");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
				Output("Message sending aborted because of an error.");
				Output("");
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnSendConcatSMS_Click(object sender, System.EventArgs e)
		{
			OutgoingSmsPdu[] pdus = CreateConcatMessage(txtConcatMessage.Text, txtConcatNumber.Text,
				chkSmartUnicode.Checked, false);
			if (pdus != null)
				SendMultiple(pdus);
		}

		private void btnCreateConcatSms_Click(object sender, System.EventArgs e)
		{
			// Just create the messages to show them in output window
			CreateConcatMessage(txtConcatMessage.Text, txtConcatNumber.Text, chkSmartUnicode.Checked, true);
		}

		private OutgoingSmsPdu[] CreateOperatorLogoMsg(Bitmap bitmap, string number, string mcc, string mnc)
		{
			Output("Creating operator logo message.");
			OutgoingSmsPdu[] pdus = null;
			try
			{
				byte[] logo;
				if (bitmap != null)
				{
					logo = SmartMessageFactory.CreateOperatorLogo(new OtaBitmap(bitmap), mcc, mnc);
				}
				else
				{
					// Empty operator logo
					Output("Empty operator logo is used.");
					logo = SmartMessageFactory.EmptyOperatorLogo;
				}
				pdus = SmartMessageFactory.CreateOperatorLogoMessage(logo, number);
			}
			catch(Exception ex)
			{
				ShowException(ex);
				return null;
			}

			Output(pdus.Length.ToString() + " message part(s) created.");
			Output("");

			return pdus;
		}

		private void btnSendOperatorLogo_Click(object sender, System.EventArgs e)
		{
			Bitmap bitmap = null;
			if (!chkCreateEmptyOpLogo.Checked)
			{
				if (!(testBitmap.Image is Bitmap))
				{
					Output("Error: Loaded image is not a bitmap.");
					Output("");
					return;
				}
				else
					bitmap = (Bitmap)testBitmap.Image;
			}

			OutgoingSmsPdu[] pdus = CreateOperatorLogoMsg(bitmap, txtConcatNumber.Text,
				txtMCC.Text, txtMNC.Text);
			if (pdus != null)
				SendMultiple(pdus);
		}

		private void btnCombineParts_Click(object sender, EventArgs e)
		{
			if (txtCombineSourceParts.Lines.Length == 0)
			{
				Output("Error: No source parts have been specified.");
				Output("");
				return;
			}
			else
			{
				try
				{
					txtCombinedConcatMessage.Text = string.Empty;
					string[] lines = txtCombineSourceParts.Lines;
					List<SmsPdu> parts = new List<SmsPdu>();
					for (int i = 0; i < lines.Length; i++)
					{
						// Ignore empty lines
						if (lines[i].Length == 0)
							continue;
						try
						{
							SmsPdu partPdu;
							if (rbConcatOutgoing.Checked)
							{
								// Outgoing message
								partPdu = OutgoingSmsPdu.Decode(lines[i], true);
							}
							else
							{
								// Incoming message
								partPdu = IncomingSmsPdu.Decode(lines[i], true);
							}
							parts.Add(partPdu);
						}
						catch (Exception ex)
						{
							ShowException(ex, "Error creating part #" + (i + 1).ToString() + ":");
							return;
						}
					}
					// Verifying whether all parts are present before combining them prevents exceptions
					// if not all parts are present.
					if (!SmartMessageDecoder.AreAllConcatPartsPresent(parts))
					{
						Output("Error: Not all message parts are present.");
						Output("");
						return;
					}
					if (chkConcatAsText.Checked)
					{
						// Get result as text
						string data = SmartMessageDecoder.CombineConcatMessageText(parts);
						txtCombinedConcatMessage.Text = data;
					}
					else
					{
						// Get result as binary data
						byte[] data = SmartMessageDecoder.CombineConcatMessage(parts);
						txtCombinedConcatMessage.Text = Calc.IntToHex(data);
					}
					Output("Combined " + parts.Count + " message parts.");
					Output("");
				}
				catch (Exception ex)
				{
					ShowException(ex);
				}
			}
		}

		#endregion

		#region Remoting

		private void CreateSmsServer()
		{
			if (smsServer == null)
			{
				smsServer = new SmsServer();

				// Register events
				smsServer.MessageSendStarting += new MessageSendEventHandler(smsServer_MessageSendStarting);
				smsServer.MessageSendComplete += new MessageSendEventHandler(smsServer_MessageSendComplete);
				smsServer.MessageSendFailed += new MessageSendErrorEventHandler(smsServer_MessageSendFailed);
			}
		}

		private void DestroySmsServer()
		{
			if (smsServer != null)
			{
				if (smsServer.IsRunning())
					smsServer.Stop();

				// Unregister events
				smsServer.MessageSendStarting -= new MessageSendEventHandler(smsServer_MessageSendStarting);
				smsServer.MessageSendComplete -= new MessageSendEventHandler(smsServer_MessageSendComplete);
				smsServer.MessageSendFailed -= new MessageSendErrorEventHandler(smsServer_MessageSendFailed);

				smsServer = null;
			}
		}

		private void btnStartRemotingServer_Click(object sender, EventArgs e)
		{
			CreateSmsServer();

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				bool closed = false;
				if (comm.IsOpen())
				{
					Output("Closing active connection to phone...");
					comm.Close();
					closed = true;
				}

				// Start SMS server
				int port = int.Parse(txtRemotingPort.Text);
				smsServer.PortName = comm.PortName;
				smsServer.BaudRate = comm.BaudRate;
				smsServer.Timeout = comm.Timeout;
				smsServer.NetworkPort = port;
				smsServer.IsSecured = chkSecureServer.Checked;
				smsServer.AllowAnonymous = chkAllowAnonymous.Checked;

				if (closed)
					Output("Starting SMS server...");
				smsServer.Start();

				Output("SMS server started at port {0}, {1}.", port,
					smsServer.IsSecured ? "secured" : "not secured");
				Output("");
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnStopRemotingServer_Click(object sender, EventArgs e)
		{
			CreateSmsServer();

			Cursor.Current = Cursors.WaitCursor;
			
			try
			{
				if (smsServer.IsRunning())
				{
					// Stop SMS server
					smsServer.Stop();
					Output("SMS server stopped.");

					if (!comm.IsOpen())
					{
						Output("Restoring connection to phone...");
						comm.Open();
						Output("Done.");
					}
					Output("");
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnRemoteSecurity_Click(object sender, EventArgs e)
		{
			frmSecurity dlg = new frmSecurity();
			SecuritySettings sec = remotingSecurity;
			dlg.chkAuthUserNamePassword.Checked = sec.AuthUserNamePassword;
			dlg.txtUsername.Text = sec.UserName;
			dlg.txtPassword.Text = sec.Password;
			dlg.txtDomain.Text = sec.Domain;
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				sec.AuthUserNamePassword = dlg.chkAuthUserNamePassword.Checked;
				sec.UserName = dlg.txtUsername.Text;
				sec.Password = dlg.txtPassword.Text;
				sec.Domain = dlg.txtDomain.Text;
			}
		}

		private void btnRemoteSendSMS_Click(object sender, EventArgs e)
		{
			string url = string.Format("tcp://{0}:{1}/SMSSender", txtRemoteServer.Text, txtRemotePort.Text);
			Output("Sending SMS via {0}...", url);

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Set up a client channel
				SecuritySettings sec = remotingSecurity;
				IDictionary props = new Hashtable();
				if (chkRemoteSecure.Checked)
				{
					props["secure"] = "true";
					if (sec.AuthUserNamePassword)
					{
						// Add user data
						props["username"] = sec.UserName;
						props["password"] = sec.Password;
						props["domain"] = sec.Domain;
					}
				}
				TcpClientChannel clientChannel = new TcpClientChannel(props, null);
				ChannelServices.RegisterChannel(clientChannel, chkRemoteSecure.Checked);
				try
				{
					ISmsSender smsSender = (ISmsSender)Activator.GetObject(typeof(ISmsSender), url);
					if (!chkRemoteUnicode.Checked)
						smsSender.SendMessage(txtRemoteMessage.Text, txtRemoteNumber.Text); // Standard message
					else
						smsSender.SendMessage(txtRemoteMessage.Text, txtRemoteNumber.Text, true); // Unicode message
				}
				finally
				{
					// Destroy client channel
					ChannelServices.UnregisterChannel(clientChannel);
				}

				Output("Done.");
				Output("");
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void chkSecureServer_CheckedChanged(object sender, EventArgs e)
		{
			chkAllowAnonymous.Enabled = chkSecureServer.Checked;
		}

		private void chkRemoteSecure_CheckedChanged(object sender, EventArgs e)
		{
			btnRemoteSecurity.Enabled = chkRemoteSecure.Checked;
		}

		private void LogSmsServer(string text)
		{
			Output("[SMS Server] {0}", text);
		}

		private void LogSmsServer(string text, string userName)
		{
			if (userName.Length == 0)
				LogSmsServer(text);
			else
				Output("[SMS Server] {0} User name: {1}", text, userName);
		}

		private void smsServer_MessageSendStarting(object sender, MessageSendEventArgs e)
		{
			LogSmsServer(string.Format("Sending message to {0}.", e.Destination), e.UserName);
		}

		private void smsServer_MessageSendComplete(object sender, MessageSendEventArgs e)
		{
			LogSmsServer(string.Format("Message sent to {0}.", e.Destination), e.UserName);
		}

		private void smsServer_MessageSendFailed(object sender, MessageSendErrorEventArgs e)
		{
			LogSmsServer(string.Format("Failed to send message to {0}. Details: {1}",
				e.Destination, e.Exception.Message), e.UserName);
		}

		#endregion

		#region Text mode
		private void btnGetCharset_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Get the currently selected text mode character set
				string charset = comm.GetCurrentCharacterSet();
				Output("Current text mode character set: " + charset);
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		
		}

		private void btnCharsets_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Get the supported text mode character sets
				string[] sets = comm.GetSupportedCharacterSets();
				Output("Supported text mode character sets: " + MakeArrayString(sets));
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnSelectCharset_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Select the specified text mode character set
				string charset = txtCharset.Text;
				comm.SelectCharacterSet(charset);
				Output("Text mode character set is now " + charset + ".");
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}
		#endregion

		#region Network

		private void GetOperator_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Retrieve info about the currently selected operator
				OperatorInfo info = comm.GetCurrentOperator();
				if (info!=null)
				{
					Output("Operator info:");
					Output("  Format: " + info.Format);
					Output("  Operator: " + info.TheOperator);
					Output("  Access Technology: " + info.AccessTechnology);
					Output("");
				}
				else
				{
					Output("There is currently no operator selected!");
					Output("");
				}
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void GetOpSelectionMode_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Get the current operator selection mode
				OperatorSelectionMode mode = comm.GetOperatorSelectionMode();
				Output("Current operator selection mode: {0}", mode);
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnListOperators_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// List the operators detected by the phone
				OperatorInfo2[] info2 = comm.ListOperators();
				Output("Operator list:");
				Output("-------------------------------------------------------------------");
				foreach(OperatorInfo2 info in info2)
				{
					Output("Status: {0}\r\nLong alphanumeric: {1}\r\nShort alphanumeric: {2}\r\n" + 
						"Numeric: {3}\r\nAccess Technology: {4}",
						info.Status, info.LongAlphanumeric, info.ShortAlphanumeric,	info.Numeric, info.AccessTechnology);
					Output("");
				}
				Output("-------------------------------------------------------------------");
				Output("{0} operator(s) found.", info2.Length.ToString());
				Output("");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnSubscriberNumbers_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				// Get the subscriber numbers
				SubscriberInfo[] info2 = comm.GetSubscriberNumbers();
				foreach (SubscriberInfo info in info2)
				{
					Output("{0,-20}Type {1}, Speed {2}, Service {3}, ITC {4}, Alpha {5}",
						info.Number, info.Type, info.Speed, info.Service, info.Itc, info.Alpha);
				}
				Output("{0,9} number(s) found.", info2.Length.ToString());
				Output("");
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		#endregion

		#region Custom commands

		private void btnGetProtocol_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				if (this.protocolLevel == null)
				{
					this.protocolLevel = comm.GetProtocol();
					this.btnExecuteCommand.Enabled = (lstProtocolCommands.SelectedItem != null);
					Output("Enabled access to protocol level.");
					Output("");
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void btnReleaseProtocol_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				if (this.protocolLevel != null)
				{
					comm.ReleaseProtocol();
					this.protocolLevel = null;
					this.btnExecuteCommand.Enabled = false;
					Output("Disabled access to protocol level.");
					Output("");
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}

			Cursor.Current = Cursors.Default;
		}

		private void lstCustomCommands_SelectedIndexChanged(object sender, EventArgs e)
		{
			ProtocolCommand command = lstProtocolCommands.SelectedItem as ProtocolCommand;
			txtData.Enabled = (command != null) ? command.NeedsData : false;
			txtPattern.Enabled = (command != null) ? command.NeedsPattern : false;
			txtError.Enabled = (command != null) ? command.NeedsError : false;
			btnExecuteCommand.Enabled = (command != null && this.protocolLevel != null);
		}

		private void btnExecuteCommand_Click(object sender, EventArgs e)
		{
			ProtocolCommand command = lstProtocolCommands.SelectedItem as ProtocolCommand;
			if (command != null && this.protocolLevel != null)
			{
				string data = txtData.Text;
				string pattern = txtPattern.Text;
				string error = txtError.Text;
				string input = null;
				Output("Executing custom command...");
				if (command.NeedsData)
					Output("Data: " + data);
				if (command.NeedsPattern)
					Output("Pattern: " + pattern);
				if (command.NeedsError)
					Output("Error: " + error);
				Cursor.Current = Cursors.WaitCursor;
				try
				{
					switch (command.Name)
					{
						case "Send":
							this.protocolLevel.Send(data);
							break;
						case "Receive":
							this.protocolLevel.Receive(out input);
							break;
						case "ExecCommand":
							this.protocolLevel.ExecCommand(data);
							break;
						case "ExecCommand2":
							this.protocolLevel.ExecCommand(data, error);
							break;
						case "ExecAndReceiveMultiple":
							input = this.protocolLevel.ExecAndReceiveMultiple(data);
							break;
						case "ExecAndReceiveAnything":
							input = this.protocolLevel.ExecAndReceiveAnything(data, pattern);
							break;
						case "ReceiveMultiple":
							input = this.protocolLevel.ReceiveMultiple();
							break;
						case "ReceiveMultiple2":
							input = this.protocolLevel.ReceiveAnything(pattern);
							break;
					}
					if (input != null)
					{
						Output("Received input: " + input);
					}
					Output("Done.");
					Output("");
				}
				catch (Exception ex)
				{
					ShowException(ex);
				}
				Cursor.Current = Cursors.Default;
			}

		}

		#endregion

		private void chkEnableLogging_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (chkEnableLogging.Checked)
				{
					comm.LoglineAdded += new LoglineAddedEventHandler(comm_LoglineAdded);
					Output("Logging enabled.");
					Output("");
				}
				else
				{
					comm.LoglineAdded -= new LoglineAddedEventHandler(comm_LoglineAdded);
					Output("Logging disabled.");
					Output("");
				}
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}
		}

		private void txtClearOutput_Click(object sender, System.EventArgs e)
		{
			txtOutput.Clear();
		}

/////////////////////// Event handlers for events from comm object

		private void comm_LoglineAdded(object sender, LoglineAddedEventArgs e)
		{
			Output("{0,-10} {1}", e.Level, e.Text);
		}

		private void comm_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			try
			{
				IMessageIndicationObject obj = e.IndicationObject;
				if (obj is MemoryLocation)
				{
					MemoryLocation loc = (MemoryLocation)obj;
					Output(string.Format("New message received in storage \"{0}\", index {1}.",
						loc.Storage, loc.Index));
					Output("");
					return;
				}
				if (obj is ShortMessage)
				{
					ShortMessage msg = (ShortMessage)obj;
					SmsPdu pdu = comm.DecodeReceivedMessage(msg);
					Output("New message received:");
					ShowMessage(pdu);
					Output("");
					return;
				}
				Output("Error: Unknown notification object!");
			}
			catch(Exception ex)
			{
				ShowException(ex);
			}
		}

		private delegate void ConnectedHandler(bool connected);
		private void OnPhoneConnectionChange(bool connected)
		{
			lblNotConnected.Visible = !connected;
		}

		private void comm_PhoneConnected(object sender, EventArgs e)
		{
			this.Invoke(new ConnectedHandler(OnPhoneConnectionChange), new object[] { true });
		}

		private void comm_PhoneDisconnected(object sender, EventArgs e)
		{
			this.Invoke(new ConnectedHandler(OnPhoneConnectionChange), new object[] { false });
		}

/////////////////////// Helpers

		private string MakeArrayString(string[] array)
		{
			System.Text.StringBuilder str = new System.Text.StringBuilder();
			foreach(string item in array)
			{
				if (str.Length > 0)
					str.Append(", ");
				str.Append(item);
			}

			return str.ToString();
		}
		
		private void ShowMessage(SmsPdu pdu)
		{
			if (pdu is SmsSubmitPdu)
			{
				// Stored (sent/unsent) message
				SmsSubmitPdu data = (SmsSubmitPdu)pdu;
				Output("SENT/UNSENT MESSAGE");
				Output("Recipient: " + data.DestinationAddress);
				Output("Message text: " + data.UserDataText);
				Output("-------------------------------------------------------------------");
				return;
			}
			if (pdu is SmsDeliverPdu)
			{
				// Received message
				SmsDeliverPdu data = (SmsDeliverPdu)pdu;
				Output("RECEIVED MESSAGE");
				Output("Sender: " + data.OriginatingAddress);
				Output("Sent: " + data.SCTimestamp.ToString());
				Output("Message text: " + data.UserDataText);
				Output("-------------------------------------------------------------------");
				return;
			}
			if (pdu is SmsStatusReportPdu)
			{
				// Status report
				SmsStatusReportPdu data = (SmsStatusReportPdu)pdu;
				Output("STATUS REPORT");
				Output("Recipient: " + data.RecipientAddress);
				Output("Status: " + data.Status.ToString());
				Output("Timestamp: " + data.DischargeTime.ToString());
				Output("Message ref: " + data.MessageReference.ToString());
				Output("-------------------------------------------------------------------");
				return;
			}
			Output("Unknown message type: " + pdu.GetType().ToString());
		}

		private string StatusToString(PhoneMessageStatus status)
		{
			// Map a message status to a string
			string ret;
			switch(status)
			{
				case PhoneMessageStatus.All:
					ret = "All";
					break;
				case PhoneMessageStatus.ReceivedRead:
					ret = "Read";
					break;
				case PhoneMessageStatus.ReceivedUnread:
					ret = "Unread";
					break;
				case PhoneMessageStatus.StoredSent:
					ret = "Sent";
					break;
				case PhoneMessageStatus.StoredUnsent:
					ret = "Unsent";
					break;
				default:
					ret = "Unknown (" + status.ToString() + ")";
					break;
			}
			return ret;
		}

		private void testBitmap_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					testBitmap.Image = Bitmap.FromFile(openFileDialog1.FileName);
				}
				catch(Exception ex)
				{
					MessageBox.Show("Error loading bitmap: " + ex.Message, "Error");
					return;
				}
			}
		}

/////////////////////// Other stuff

	}
}
