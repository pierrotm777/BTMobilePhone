'=====================================================================
'  File:      frmMain.vb
'
'  Summary:   VB.NET sample to show how to consume GSMComm's events.
'
'---------------------------------------------------------------------
'
'This source code is intended only as a supplement to the GSMComm
'development package or on-line documentation and may not be distributed
'separately.
'=====================================================================

Imports GsmComm.GsmCommunication
Imports GsmComm.PduConverter

Public Class frmMain
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    Friend WithEvents btnMsgNotification As System.Windows.Forms.Button
    Friend WithEvents btnMessageNotificationOff As System.Windows.Forms.Button
    Friend WithEvents btnMsgRouting As System.Windows.Forms.Button
    Friend WithEvents btnMsgRoutingOff As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNumber As System.Windows.Forms.TextBox
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents btnSendSMS As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtOutput = New System.Windows.Forms.TextBox
        Me.btnMsgNotification = New System.Windows.Forms.Button
        Me.btnMessageNotificationOff = New System.Windows.Forms.Button
        Me.btnMsgRouting = New System.Windows.Forms.Button
        Me.btnMsgRoutingOff = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtNumber = New System.Windows.Forms.TextBox
        Me.txtMessage = New System.Windows.Forms.TextBox
        Me.btnSendSMS = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtOutput
        '
        Me.txtOutput.Location = New System.Drawing.Point(8, 128)
        Me.txtOutput.MaxLength = 0
        Me.txtOutput.Multiline = True
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.ReadOnly = True
        Me.txtOutput.Size = New System.Drawing.Size(512, 192)
        Me.txtOutput.TabIndex = 0
        Me.txtOutput.Text = ""
        '
        'btnMsgNotification
        '
        Me.btnMsgNotification.Location = New System.Drawing.Point(16, 16)
        Me.btnMsgNotification.Name = "btnMsgNotification"
        Me.btnMsgNotification.Size = New System.Drawing.Size(112, 48)
        Me.btnMsgNotification.TabIndex = 1
        Me.btnMsgNotification.Text = "Enable message notifications"
        '
        'btnMessageNotificationOff
        '
        Me.btnMessageNotificationOff.Location = New System.Drawing.Point(128, 16)
        Me.btnMessageNotificationOff.Name = "btnMessageNotificationOff"
        Me.btnMessageNotificationOff.Size = New System.Drawing.Size(64, 48)
        Me.btnMessageNotificationOff.TabIndex = 2
        Me.btnMessageNotificationOff.Text = "Disable"
        '
        'btnMsgRouting
        '
        Me.btnMsgRouting.Location = New System.Drawing.Point(16, 72)
        Me.btnMsgRouting.Name = "btnMsgRouting"
        Me.btnMsgRouting.Size = New System.Drawing.Size(112, 48)
        Me.btnMsgRouting.TabIndex = 3
        Me.btnMsgRouting.Text = "Enable message routing"
        '
        'btnMsgRoutingOff
        '
        Me.btnMsgRoutingOff.Location = New System.Drawing.Point(128, 72)
        Me.btnMsgRoutingOff.Name = "btnMsgRoutingOff"
        Me.btnMsgRoutingOff.Size = New System.Drawing.Size(64, 48)
        Me.btnMsgRoutingOff.TabIndex = 4
        Me.btnMsgRoutingOff.Text = "Disable"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(224, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(232, 23)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Send an SMS message:"
        '
        'txtNumber
        '
        Me.txtNumber.Location = New System.Drawing.Point(224, 40)
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.Size = New System.Drawing.Size(232, 20)
        Me.txtNumber.TabIndex = 6
        Me.txtNumber.Text = "+483341234567"
        '
        'txtMessage
        '
        Me.txtMessage.Location = New System.Drawing.Point(224, 64)
        Me.txtMessage.MaxLength = 160
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(232, 48)
        Me.txtMessage.TabIndex = 7
        Me.txtMessage.Text = "This is a test. Have a nice day!"
        '
        'btnSendSMS
        '
        Me.btnSendSMS.Location = New System.Drawing.Point(464, 40)
        Me.btnSendSMS.Name = "btnSendSMS"
        Me.btnSendSMS.Size = New System.Drawing.Size(48, 72)
        Me.btnSendSMS.TabIndex = 8
        Me.btnSendSMS.Text = "Send"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(528, 334)
        Me.Controls.Add(Me.btnSendSMS)
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.txtNumber)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnMsgRoutingOff)
        Me.Controls.Add(Me.btnMsgRouting)
        Me.Controls.Add(Me.btnMessageNotificationOff)
        Me.Controls.Add(Me.btnMsgNotification)
        Me.Controls.Add(Me.txtOutput)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.Text = "VB Events Demo"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private WithEvents comm As GsmCommMain
    Private Delegate Sub SetTextCallback(ByVal text As String)

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        comm = New GsmCommMain("COM1")
        comm.Open()
    End Sub

    Private Sub frmMain_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        comm.Close()
    End Sub

    'This sub handles GsmCommMain's MessageReceived event
    Private Sub comm_MessageReceived(ByVal sender As Object, ByVal e As MessageReceivedEventArgs) Handles comm.MessageReceived
        Dim obj As IMessageIndicationObject = e.IndicationObject

        Try
            'If it's just a notification, print out the memory location of the new message
            If TypeOf obj Is MemoryLocation Then
                Dim loc As MemoryLocation = CType(obj, MemoryLocation)
                Output(String.Format("New message received in storage {0}, index {1}.", loc.Storage, loc.Index))
                Output("")
                Exit Sub
            End If

            'If it's a complete message, then it was routed directly
            If TypeOf obj Is ShortMessage Then
                Dim msg As ShortMessage = CType(obj, ShortMessage)
                Dim pdu As SmsPdu = comm.DecodeReceivedMessage(msg)
                ShowMessage(pdu)
                Output("")
                Exit Sub
            End If

            Output("Error: Unknown notification object!")
            Output("")
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Sub

    'Displays a short message
    Private Sub ShowMessage(ByVal pdu As SmsPdu)
        If TypeOf pdu Is SmsSubmitPdu Then
            'Stored (sent/unsent) message
            Dim data As SmsSubmitPdu = CType(pdu, SmsSubmitPdu)
            Output("SENT/UNSENT MESSAGE")
            Output("Recipient: " + data.DestinationAddress)
            Output("Message text: " + data.UserDataText)
            Output("-------------------------------------------------------------------")
            Exit Sub
        End If

        If TypeOf pdu Is SmsDeliverPdu Then
            'Received message
            Dim data As SmsDeliverPdu = CType(pdu, SmsDeliverPdu)
            Output("RECEIVED MESSAGE")
            Output("Sender: " + data.OriginatingAddress)
            Output("Sent: " + data.SCTimestamp.ToString())
            Output("Message text: " + data.UserDataText)
            Output("-------------------------------------------------------------------")
            Exit Sub
        End If

        If TypeOf pdu Is SmsStatusReportPdu Then
            'Status report
            Dim data As SmsStatusReportPdu = CType(pdu, SmsStatusReportPdu)
            Output("STATUS REPORT")
            Output("Recipient: " + data.RecipientAddress)
            Output("Status: " + data.Status.ToString())
            Output("Timestamp: " + data.DischargeTime.ToString())
            Output("Message ref: " + data.MessageReference.ToString())
            Output("-------------------------------------------------------------------")
            Exit Sub
        End If
        Output("Unknown message type: " + pdu.GetType().ToString())
    End Sub

    Private Sub Output(ByVal text As String)
        'If text is outputted from a different thread, invoke the sub again from the UI thread
        If txtOutput.InvokeRequired Then
            Dim stc As SetTextCallback = New SetTextCallback(AddressOf Output)
            Dim args() As Object = {text}
            txtOutput.Invoke(stc, args)
        Else
            txtOutput.AppendText(text)
            txtOutput.AppendText(Environment.NewLine)
        End If
    End Sub

    'Shows the exception details if an error occurs.
    Private Sub ShowException(ByVal ex As Exception)
        Output("Error: " + ex.Message + " (" + ex.GetType().ToString() + ")")
        Output("")
    End Sub

    Private Sub btnMsgNotification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMsgNotification.Click
        Try
            'Enable notifications about new received messages
            comm.EnableMessageNotifications()
            Output("Message notifications activated.")
            Output("")
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Sub

    Private Sub btnMessageNotificationOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMessageNotificationOff.Click
        Try
            'Disable notifications about new received messages
            comm.DisableMessageNotifications()
            Output("Message notifications deactivated.")
            Output("")
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Sub

    Private Sub btnMsgRouting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMsgRouting.Click
        Try
            'Enable direct routing of new messages to the application
            comm.EnableMessageRouting()
            Output("Message routing activated.")
            Output("")
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Sub

    Private Sub btnMsgRoutingOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMsgRoutingOff.Click
        Try
            'Disable direct routing of new messages to the application
            comm.DisableMessageRouting()
            Output("Message routing deactivated.")
            Output("")
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Sub

    Private Sub btnSendSMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendSMS.Click
        Try
            'Send an SMS message
            Dim pdu As SmsSubmitPdu = New SmsSubmitPdu(txtMessage.Text, txtNumber.Text)
            comm.SendMessage(pdu)
            Output("Message sent.")
            Output("")
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Sub
End Class
