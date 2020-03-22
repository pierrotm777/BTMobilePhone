Imports ATSMS.SMS.Decoder
Imports ATSMS.SMS

Public Class frmPDUDecoder
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents txtPDU As System.Windows.Forms.TextBox
    Friend WithEvents txtResult As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPDUDecoder))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Button1 = New System.Windows.Forms.Button
        Me.Splitter2 = New System.Windows.Forms.Splitter
        Me.txtPDU = New System.Windows.Forms.TextBox
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.txtResult = New System.Windows.Forms.TextBox
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.Splitter2)
        Me.Panel1.Controls.Add(Me.txtPDU)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(433, 67)
        Me.Panel1.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(336, 24)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 24)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Decode it!"
        '
        'Splitter2
        '
        Me.Splitter2.Location = New System.Drawing.Point(307, 0)
        Me.Splitter2.Name = "Splitter2"
        Me.Splitter2.Size = New System.Drawing.Size(6, 67)
        Me.Splitter2.TabIndex = 2
        Me.Splitter2.TabStop = False
        '
        'txtPDU
        '
        Me.txtPDU.Dock = System.Windows.Forms.DockStyle.Left
        Me.txtPDU.Location = New System.Drawing.Point(0, 0)
        Me.txtPDU.Multiline = True
        Me.txtPDU.Name = "txtPDU"
        Me.txtPDU.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPDU.Size = New System.Drawing.Size(307, 67)
        Me.txtPDU.TabIndex = 1
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Splitter1.Location = New System.Drawing.Point(0, 67)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(433, 7)
        Me.Splitter1.TabIndex = 2
        Me.Splitter1.TabStop = False
        '
        'txtResult
        '
        Me.txtResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtResult.Location = New System.Drawing.Point(0, 74)
        Me.txtResult.Multiline = True
        Me.txtResult.Name = "txtResult"
        Me.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResult.Size = New System.Drawing.Size(433, 263)
        Me.txtResult.TabIndex = 3
        '
        'frmPDUDecoder
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(433, 337)
        Me.Controls.Add(Me.txtResult)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPDUDecoder"
        Me.Text = "PDU Decoder"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim s As Object = Nothing
            Dim PDUCode As String = txtPDU.Text.Replace(vbCrLf, "")
            If PDUCode.StartsWith("00") Then
                PDUCode = "02810011" & Mid(PDUCode, 5, PDUCode.Length - 2)
            End If
            Dim T As SMSType = SMSBase.GetSMSType(PDUCode)
            txtResult.Text = txtResult.Text + T.ToString + vbCrLf
            Select Case T
                Case SMSType.EMS_RECEIVED
                    s = New EMS_RECEIVED(PDUCode)
                    txtResult.Text += "From:" + s.SrcAddressValue + "  Time:" + s.TP_SCTS + vbCrLf + vbCrLf
                Case SMSType.SMS_RECEIVED
                    s = New SMS_RECEIVED(PDUCode)
                    txtResult.Text += "From:" + s.SrcAddressValue + "  Time:" + s.TP_SCTS + vbCrLf + vbCrLf
                Case SMSType.EMS_SUBMIT
                    s = New EMS_SUBMIT(PDUCode)
                    txtResult.Text += "Send to:" + s.DesAddressValue + vbCrLf + vbCrLf
                Case SMSType.SMS_SUBMIT
                    s = New SMS_SUBMIT(PDUCode)
                    txtResult.Text += "Send to:" + s.DesAddressValue + vbCrLf + vbCrLf
                Case SMSType.SMS_STATUS_REPORT
                    s = New SMS_STATUS_REPORT(PDUCode)
                    txtResult.Text += "Send time:" + s.TP_SCTS + "  Receive time:" + s.TP_DP + "   ״̬:" + (s.status).ToString + vbCrLf + vbCrLf
                Case Else
                    txtResult.Text = "Sorry, maybe it is a wrong PDU Code"
            End Select
            '###########################
            'Correct when s is SMS type, no TP_UDL is found.
            'Note:Only EMS has the TP_UDHL and TP_UDH see 3GPP TS 23.040 V6.5.0 (2004-09)
            '###########################

            If s.tp_DCS = 0 Or s.tp_DCS = 242 Then
                If T = SMSType.SMS_RECEIVED Or T = SMSType.SMS_STATUS_REPORT Or T = SMSType.SMS_SUBMIT Then
                    '#############################
                    'add a parameter
                    '############################
                    txtResult.Text += s.decode7bit(s.tp_UD, s.TP_UDL) + vbCrLf
                End If
                If T = SMSType.EMS_RECEIVED Or T = SMSType.EMS_SUBMIT Then
                    txtResult.Text += s.decode7bit(s.tp_ud, s.tp_udl - 8 * (1 + s.tp_udhl) / 7) + vbCrLf
                End If
            Else
                txtResult.Text = txtResult.Text + s.DecodeUnicode(s.TP_UD) + vbCrLf
            End If
        Catch err As System.Exception
            Me.Text = err.Message
        End Try
    End Sub

    Private Sub frmPDUDecoder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
