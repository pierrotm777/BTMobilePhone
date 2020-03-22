
Imports ATSMS.SMS
Imports ATSMS.SMS.Encoder.SMS
Imports ATSMS.SMS.Encoder.ConcatenatedShortMessage

Public Class frmPDUEncoder
    Inherits System.Windows.Forms.Form
    Dim SMSObject As Object  'Object To Store SMS or ConcatenatedShortMessage. Late Blinding.
    Dim DataCodingScheme As ENUM_TP_DCS
    Dim ValidPeriod As ENUM_TP_VPF
    Dim PDUCodes() As String

#Region " Windows "

    Public Sub New()
        MyBase.New()


        InitializeComponent()



    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows 
    Private components As System.ComponentModel.IContainer

    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtMsgRef As System.Windows.Forms.TextBox
    Friend WithEvents cmbValidPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDataCodingScheme As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkStatusReport As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtDestNum As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtServiceCenterNum As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtUserData As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdReset As System.Windows.Forms.Button
    Friend WithEvents cmdGetPDU As System.Windows.Forms.Button
    Friend WithEvents stsBar As System.Windows.Forms.StatusBar
    Friend WithEvents stsBarCharCount As System.Windows.Forms.StatusBarPanel
    Friend WithEvents txtPDU As System.Windows.Forms.TextBox
    Friend WithEvents cmdCopyToClipboard As System.Windows.Forms.Button
    Friend WithEvents stsPDULength As System.Windows.Forms.StatusBarPanel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPDUEncoder))
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtMsgRef = New System.Windows.Forms.TextBox
        Me.cmbValidPeriod = New System.Windows.Forms.ComboBox
        Me.cmbDataCodingScheme = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.chkStatusReport = New System.Windows.Forms.CheckBox
        Me.cmdReset = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtDestNum = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtServiceCenterNum = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.txtUserData = New System.Windows.Forms.TextBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.txtPDU = New System.Windows.Forms.TextBox
        Me.cmdGetPDU = New System.Windows.Forms.Button
        Me.stsBar = New System.Windows.Forms.StatusBar
        Me.stsBarCharCount = New System.Windows.Forms.StatusBarPanel
        Me.stsPDULength = New System.Windows.Forms.StatusBarPanel
        Me.cmdCopyToClipboard = New System.Windows.Forms.Button
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.stsBarCharCount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stsPDULength, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtMsgRef)
        Me.GroupBox2.Controls.Add(Me.cmbValidPeriod)
        Me.GroupBox2.Controls.Add(Me.cmbDataCodingScheme)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.chkStatusReport)
        Me.GroupBox2.Controls.Add(Me.cmdReset)
        Me.GroupBox2.Location = New System.Drawing.Point(7, 104)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(280, 145)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Options"
        '
        'txtMsgRef
        '
        Me.txtMsgRef.Location = New System.Drawing.Point(137, 82)
        Me.txtMsgRef.Name = "txtMsgRef"
        Me.txtMsgRef.Size = New System.Drawing.Size(133, 20)
        Me.txtMsgRef.TabIndex = 9
        '
        'cmbValidPeriod
        '
        Me.cmbValidPeriod.Location = New System.Drawing.Point(137, 52)
        Me.cmbValidPeriod.Name = "cmbValidPeriod"
        Me.cmbValidPeriod.Size = New System.Drawing.Size(136, 21)
        Me.cmbValidPeriod.TabIndex = 8
        '
        'cmbDataCodingScheme
        '
        Me.cmbDataCodingScheme.Location = New System.Drawing.Point(137, 22)
        Me.cmbDataCodingScheme.Name = "cmbDataCodingScheme"
        Me.cmbDataCodingScheme.Size = New System.Drawing.Size(136, 21)
        Me.cmbDataCodingScheme.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(23, 85)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(94, 15)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Message Reference"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(23, 56)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(94, 15)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "ValidityPeriod"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(23, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(108, 19)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "DataCodingScheme"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkStatusReport
        '
        Me.chkStatusReport.CheckAlign = System.Drawing.ContentAlignment.BottomRight
        Me.chkStatusReport.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkStatusReport.Location = New System.Drawing.Point(27, 115)
        Me.chkStatusReport.Name = "chkStatusReport"
        Me.chkStatusReport.Size = New System.Drawing.Size(120, 15)
        Me.chkStatusReport.TabIndex = 0
        Me.chkStatusReport.Text = "Status Report"
        Me.chkStatusReport.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'cmdReset
        '
        Me.cmdReset.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cmdReset.Location = New System.Drawing.Point(177, 110)
        Me.cmdReset.Name = "cmdReset"
        Me.cmdReset.Size = New System.Drawing.Size(90, 26)
        Me.cmdReset.TabIndex = 10
        Me.cmdReset.Text = "&Reset To Default"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtDestNum)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtServiceCenterNum)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(280, 86)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Number"
        '
        'txtDestNum
        '
        Me.txtDestNum.Location = New System.Drawing.Point(137, 54)
        Me.txtDestNum.Name = "txtDestNum"
        Me.txtDestNum.Size = New System.Drawing.Size(136, 20)
        Me.txtDestNum.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(13, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 22)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Destination Number"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtServiceCenterNum
        '
        Me.txtServiceCenterNum.Location = New System.Drawing.Point(137, 24)
        Me.txtServiceCenterNum.Name = "txtServiceCenterNum"
        Me.txtServiceCenterNum.Size = New System.Drawing.Size(136, 20)
        Me.txtServiceCenterNum.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(10, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 23)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Service Center Number"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtUserData)
        Me.GroupBox3.Location = New System.Drawing.Point(293, 11)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(294, 86)
        Me.GroupBox3.TabIndex = 8
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "User Data"
        '
        'txtUserData
        '
        Me.txtUserData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtUserData.Location = New System.Drawing.Point(3, 16)
        Me.txtUserData.Multiline = True
        Me.txtUserData.Name = "txtUserData"
        Me.txtUserData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtUserData.Size = New System.Drawing.Size(288, 67)
        Me.txtUserData.TabIndex = 0
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.txtPDU)
        Me.GroupBox4.Location = New System.Drawing.Point(293, 104)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(294, 145)
        Me.GroupBox4.TabIndex = 9
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "PDU Code"
        '
        'txtPDU
        '
        Me.txtPDU.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPDU.Location = New System.Drawing.Point(3, 16)
        Me.txtPDU.Multiline = True
        Me.txtPDU.Name = "txtPDU"
        Me.txtPDU.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPDU.Size = New System.Drawing.Size(288, 126)
        Me.txtPDU.TabIndex = 2
        '
        'cmdGetPDU
        '
        Me.cmdGetPDU.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdGetPDU.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cmdGetPDU.Location = New System.Drawing.Point(293, 256)
        Me.cmdGetPDU.Name = "cmdGetPDU"
        Me.cmdGetPDU.Size = New System.Drawing.Size(94, 26)
        Me.cmdGetPDU.TabIndex = 11
        Me.cmdGetPDU.Text = "&Get PDU Code"
        '
        'stsBar
        '
        Me.stsBar.Location = New System.Drawing.Point(0, 309)
        Me.stsBar.Name = "stsBar"
        Me.stsBar.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.stsBarCharCount, Me.stsPDULength})
        Me.stsBar.ShowPanels = True
        Me.stsBar.Size = New System.Drawing.Size(710, 19)
        Me.stsBar.TabIndex = 12
        '
        'stsBarCharCount
        '
        Me.stsBarCharCount.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.stsBarCharCount.Name = "stsBarCharCount"
        Me.stsBarCharCount.Width = 594
        '
        'stsPDULength
        '
        Me.stsPDULength.Name = "stsPDULength"
        '
        'cmdCopyToClipboard
        '
        Me.cmdCopyToClipboard.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cmdCopyToClipboard.Location = New System.Drawing.Point(393, 256)
        Me.cmdCopyToClipboard.Name = "cmdCopyToClipboard"
        Me.cmdCopyToClipboard.Size = New System.Drawing.Size(133, 26)
        Me.cmdCopyToClipboard.TabIndex = 13
        Me.cmdCopyToClipboard.Text = "&Copy To Clipboard"
        '
        'frmPDUEncoder
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(710, 328)
        Me.Controls.Add(Me.cmdCopyToClipboard)
        Me.Controls.Add(Me.stsBar)
        Me.Controls.Add(Me.cmdGetPDU)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPDUEncoder"
        Me.Opacity = 0.95
        Me.Text = "PDU Encoder"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.stsBarCharCount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stsPDULength, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub txtUserData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUserData.TextChanged
        'Count for number of PDUs
        Dim i As Integer
        Dim Encoding As Integer '0 for English 1 for Unicode
        Encoding = cmbDataCodingScheme.SelectedIndex
        Dim Text As String = txtUserData.Text
        For i = 0 To Text.Length - 1
            If Asc(Text.Chars(i)) < 0 Then
                Encoding = 1
                Exit For
            End If
        Next

        Dim TxtLength As Integer = txtUserData.TextLength
        With stsBarCharCount
            .Text = "CharCount:" & TxtLength
            Dim Piece As Integer

            If Encoding = 0 Then
                If TxtLength <= 160 Then
                    Piece = 1
                    .Text += "/160"
                Else
                    Piece = (TxtLength \ 152) + ((TxtLength Mod 152) = 0) + 1
                    .Text += "/152"
                End If
            End If

            If Encoding = 1 Then
                If TxtLength <= 70 Then
                    Piece = 1
                    .Text += "/70"
                Else
                    Piece = (TxtLength \ 66) + ((TxtLength Mod 66) = 0) + 1
                    .Text += "/66"
                End If
            End If

            .Text += "  Split into " & Piece & " PDUs"
        End With
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Init Controls
        cmbDataCodingScheme.Items.Add(ENUM_TP_DCS.DefaultAlphabet & ":" & ENUM_TP_DCS.DefaultAlphabet.ToString)
        cmbDataCodingScheme.Items.Add(ENUM_TP_DCS.UCS2 & ":" & ENUM_TP_DCS.UCS2.ToString)
        cmbDataCodingScheme.Items.Add(ENUM_TP_DCS.Class2_UD_7bits & ":" & ENUM_TP_DCS.Class2_UD_7bits.ToString)
        cmbDataCodingScheme.SelectedIndex = 0

        cmbValidPeriod.Items.Add(ENUM_TP_VALID_PERIOD.Maximum & ":" & ENUM_TP_VALID_PERIOD.Maximum.ToString)
        cmbValidPeriod.Items.Add(ENUM_TP_VALID_PERIOD.OneDay & ":" & ENUM_TP_VALID_PERIOD.OneDay.ToString)
        cmbValidPeriod.Items.Add(ENUM_TP_VALID_PERIOD.OneHour & ":" & ENUM_TP_VALID_PERIOD.OneHour.ToString)
        cmbValidPeriod.Items.Add(ENUM_TP_VALID_PERIOD.OneWeek & ":" & ENUM_TP_VALID_PERIOD.OneWeek.ToString)
        cmbValidPeriod.Items.Add(ENUM_TP_VALID_PERIOD.SixHours & ":" & ENUM_TP_VALID_PERIOD.SixHours.ToString)
        cmbValidPeriod.Items.Add(ENUM_TP_VALID_PERIOD.ThreeHours & ":" & ENUM_TP_VALID_PERIOD.ThreeHours.ToString)
        cmbValidPeriod.Items.Add(ENUM_TP_VALID_PERIOD.TwelveHours & ":" & ENUM_TP_VALID_PERIOD.TwelveHours.ToString)
        cmbValidPeriod.SelectedIndex = 0

        txtMsgRef.Text = 0
    End Sub

    Private Sub cmdGetPDU_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGetPDU.Click
        'Check all the information has input.
        If txtServiceCenterNum.TextLength = 0 Then MsgBox("Please Enter Service Center Number") : Return
        If txtDestNum.TextLength = 0 Then MsgBox("Please Enter Destination Number") : Return
        If txtUserData.Text = "" Then MsgBox("Please Enter UserData") : Return

        'Get PDU Code
        PDUCodes = GetPDU(txtServiceCenterNum.Text, txtDestNum.Text, Val(cmbDataCodingScheme.Text), Val(cmbValidPeriod.Text), Val(txtMsgRef.Text), chkStatusReport.Checked, txtUserData.Text)
        'Add PDU Codes to Text
        Dim i As Integer
        stsPDULength.Text = ""
        txtPDU.Text = ""
        For i = 0 To PDUCodes.Length - 1
            txtPDU.Text += "PDU Number:" & i + 1
            txtPDU.Text += vbTab + "Length For AT:" & (PDUCodes(i).Length - Val("&H" & Mid(PDUCodes(i), 1, 2)) * 2 - 2) / 2 & vbCrLf    'Calculate PDU Length for AT command
            txtPDU.Text += PDUCodes(i) & vbCrLf
        Next
    End Sub

    Private Function GetPDU(ByVal ServiceCenterNumber As String, _
                            ByVal DestNumber As String, _
                            ByVal DataCodingScheme As ENUM_TP_DCS, _
                            ByVal ValidPeriod As ENUM_TP_VALID_PERIOD, _
                            ByVal MsgReference As Integer, _
                            ByVal StatusReport As Boolean, _
                            ByVal UserData As String) As String()
        'Check for SMS type
        Dim Type As Integer '0 for SMS;1 For ConcatenatedShortMessage
        Dim Result() As String
        SMSObject = New ATSMS.SMS.Encoder.SMS
        Select Case DataCodingScheme
            Case ENUM_TP_DCS.DefaultAlphabet, ENUM_TP_DCS.Class2_UD_7bits
                If txtUserData.TextLength > 160 Then
                    SMSObject = New ATSMS.SMS.Encoder.ConcatenatedShortMessage
                    Type = 1
                End If
            Case ENUM_TP_DCS.UCS2
                If txtUserData.TextLength > 70 Then
                    SMSObject = New ATSMS.SMS.Encoder.ConcatenatedShortMessage
                    Type = 1
                End If
        End Select

        With SMSObject
            .ServiceCenterNumber = ServiceCenterNumber
            If StatusReport = True Then
                .TP_Status_Report_Request = ATSMS.SMS.ENUM_TP_SRI.Request_SMS_Report
            Else
                .TP_Status_Report_Request = ATSMS.SMS.ENUM_TP_SRI.No_SMS_Report
            End If
            .TP_Destination_Address = DestNumber
            .TP_Data_Coding_Scheme = DataCodingScheme
            .TP_Message_Reference = CInt(txtMsgRef.Text)
            .TP_Validity_Period = ValidPeriod
            .TP_User_Data = UserData
        End With

        If Type = 0 Then
            ReDim Result(0)
            Result(0) = SMSObject.GetSMSPDUCode
        Else
            Result = SMSObject.GetEMSPDUCode            'Note here must use GetEMSPDUCode to get right PDU codes
        End If
        Return Result
    End Function

    Private Sub cmdCopyToClipboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopyToClipboard.Click
        Try
            Dim Data As String = String.Empty, i As Integer
            For i = 0 To PDUCodes.Length - 1
                Data += PDUCodes(i) & vbCrLf
            Next
            Data = Data.Remove(Data.Length - 2, 2) 'Remove the last vbCrLf
            Clipboard.SetDataObject(Data)
        Catch ex As System.Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        txtServiceCenterNum.Text = ""
        txtDestNum.Text = ""
        cmbDataCodingScheme.SelectedIndex = 0
        cmbValidPeriod.SelectedIndex = 0
        txtMsgRef.Text = 0
        chkStatusReport.Checked = False
        txtUserData.Text = ""
    End Sub

    Private Sub cmbDataCodingScheme_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDataCodingScheme.SelectedIndexChanged
        txtUserData_TextChanged(Nothing, Nothing)
    End Sub


End Class
