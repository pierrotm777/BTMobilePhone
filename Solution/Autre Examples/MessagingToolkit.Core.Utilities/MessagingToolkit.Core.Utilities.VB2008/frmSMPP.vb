'===============================================================================
' OSML - Open Source Messaging Library
'
'===============================================================================
' Copyright © TWIT88.COM.  All rights reserved.
'
' This file is part of Open Source Messaging Library.
'
' Open Source Messaging Library is free software: you can redistribute it 
' and/or modify it under the terms of the GNU General Public License version 3.
'
' Open Source Messaging Library is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this software.  If not, see <http://www.gnu.org/licenses/>.
'===============================================================================

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Reflection

Imports MessagingToolkit
Imports MessagingToolkit.Core
Imports MessagingToolkit.Core.Smpp
Imports MessagingToolkit.Core.Smpp.Packet
Imports MessagingToolkit.core.Smpp.Packet.PDU
Imports MessagingToolkit.Core.Smpp.Utility
Imports MessagingToolkit.Core.Smpp.Packet.Request
Imports MessagingToolkit.Core.Smpp.Packet.Response
Imports MessagingToolkit.Core.Smpp.EventObjects

'Namespace MessagingToolkit.Core.Utilities
Partial Public Class frmSMPP
    Inherits Form
    ''' <summary>
    ''' SMPP gateway interface
    ''' </summary>
    Private smppGateway As ISmppGateway = SmppGatewayFactory.[Default]

    ''' <summary>
    ''' System mode - bind type
    ''' </summary>
    Private SystemMode As New Dictionary(Of String, BindingType)()


    ''' <summary>
    ''' Interface version
    ''' </summary>
    Private InterfaceVersion As New Dictionary(Of String, SmppVersionType)()

    ''' <summary>
    ''' Interface version
    ''' </summary>
    Private DataCoding As New Dictionary(Of String, DataCodingType)()

    ''' <summary>
    ''' Type of number
    ''' </summary>
		Private TypeOfNumber As New Dictionary(Of String, TonType)()

    ''' <summary>
    ''' Numbering plan indicator
    ''' </summary>
		Private NumberType As New Dictionary(Of String, NpiType)()
    Private Sub LoadDictionaries()

        ''' <summary>
        ''' System mode - bind type
        ''' </summary>
        SystemMode.add("Transceiver", BindingType.BindAsTransceiver)
        SystemMode.add("Transmitter", BindingType.BindAsTransmitter)
        SystemMode.add("Receiver", BindingType.BindAsReceiver)


        ''' <summary>
        ''' Interface version
        ''' </summary>
        InterfaceVersion.add("3.4", SmppVersionType.Version3_4)
        InterfaceVersion.add("3.3", SmppVersionType.Version3_3)


        ''' <summary>
        ''' Interface version
        ''' </summary>
        DataCoding.Add("SMSC Default", DataCodingType.SMSCDefault)
        DataCoding.Add("IA5 Ascii", DataCodingType.IA5_ASCII)
        DataCoding.Add("Octet Unspecified B", DataCodingType.OctetUnspecifiedB)
        DataCoding.Add("Latin 1", DataCodingType.Latin1)
        DataCoding.Add("Octet Unspecified A", DataCodingType.OctetUnspecifiedA)
        DataCoding.Add("JIS", DataCodingType.JIS)
        DataCoding.Add("Cyrillic", DataCodingType.Cyrillic)
        DataCoding.Add("Latin Hebrew", DataCodingType.Latin_Hebrew)
        DataCoding.Add("UCS2", DataCodingType.Ucs2)
        DataCoding.Add("Pictogram", DataCodingType.Pictogram)
        DataCoding.Add("Default Flash SMS", DataCodingType.DefaultFlashSms)
        DataCoding.Add("Unicode Flash SMS", DataCodingType.UnicodeFlashSms)
        DataCoding.Add("MusicCodes", DataCodingType.MusicCodes)
        DataCoding.Add("Extended Kanji JIS", DataCodingType.ExtendedKanjiJIS)
        DataCoding.Add("KS C", DataCodingType.KS_C)



        ''' <summary>
        ''' Type of number
        ''' </summary>
        TypeOfNumber.add("Default", TonType.Unknown)
        TypeOfNumber.add("International", TonType.International)
        TypeOfNumber.add("National", TonType.National)
        TypeOfNumber.add("Network Specific", TonType.NetworkSpecific)
        TypeOfNumber.add("Subscriber Number", TonType.SubscriberNumber)
        TypeOfNumber.add("Alphanumeric", TonType.Alphanumeric)
        TypeOfNumber.add("Abbreviated", TonType.Abbreviated)



        ''' <summary>
        ''' Numbering plan indicator
        ''' </summary>
        NumberType.add("Default", NpiType.Unknown)
        NumberType.add("ISDN", NpiType.ISDN)
        NumberType.add("Data", NpiType.Data)
        NumberType.add("Telex", NpiType.Telex)
        NumberType.add("Land Mobile", NpiType.LandMobile)
        NumberType.add("National", NpiType.National)
        NumberType.add("Private", NpiType.[Private])
        NumberType.add("ERMES", NpiType.ERMES)
        NumberType.add("Internet", NpiType.Internet)

    End Sub

    Private Delegate Sub SetTextCallback(ByVal text As String)

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Shows the error.
    ''' </summary>
    ''' <param name="errorMessage">The error message.</param>
    Private Sub ShowError(ByVal errorMessage As String)
        MessageBox.Show(errorMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    ''' <summary>
    ''' Handles the Load event of the frmSMPP control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub frmSMPP_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LoadDictionaries()

        'cboSystemMode.Items.AddRange(SystemMode.Keys.ToArray())
        cboSystemMode.DataSource = New BindingSource(SystemMode, Nothing)
        cboSystemMode.DisplayMember = "Key"
        cboSystemMode.ValueMember = "Value"
        cboSystemMode.SelectedIndex = 0

        'cboInterfaceVersion.Items.AddRange(InterfaceVersion.Keys.ToArray())
        cboInterfaceVersion.DataSource = New BindingSource(InterfaceVersion, Nothing)
        cboInterfaceVersion.DisplayMember = "Key"
        cboInterfaceVersion.ValueMember = "Value"
        cboInterfaceVersion.SelectedIndex = 0

        'cboDataCoding.Items.AddRange(DataCoding.Keys.ToArray())
        cboDataCoding.DataSource = New BindingSource(DataCoding, Nothing)
        cboDataCoding.DisplayMember = "Key"
        cboDataCoding.ValueMember = "Value"
        cboDataCoding.SelectedIndex = 0

        'cboSourceTon.Items.AddRange(TypeOfNumber.Keys.ToArray())
        cboSourceTon.DataSource = New BindingSource(TypeOfNumber, Nothing)
        cboSourceTon.DisplayMember = "Key"
        cboSourceTon.ValueMember = "Value"
        cboSourceTon.SelectedIndex = 1

        'cboTon.Items.AddRange(TypeOfNumber.Keys.ToArray())
        cboTon.DataSource = New BindingSource(TypeOfNumber, Nothing)
        cboTon.DisplayMember = "Key"
        cboTon.ValueMember = "Value"
        cboTon.SelectedIndex = 1

        'cboDestinationTon.Items.AddRange(TypeOfNumber.Keys.ToArray())
        cboDestinationTon.DataSource = New BindingSource(TypeOfNumber, Nothing)
        cboDestinationTon.DisplayMember = "Key"
        cboDestinationTon.ValueMember = "Value"
        cboDestinationTon.SelectedIndex = 0

        'cboSourceNpi.Items.AddRange(NumberType.Keys.ToArray())
        cboSourceNpi.DataSource = New BindingSource(NumberType, Nothing)
        cboSourceNpi.DisplayMember = "Key"
        cboSourceNpi.ValueMember = "Value"
        cboSourceNpi.SelectedIndex = 1

        'cboNpi.Items.AddRange(NumberType.Keys.ToArray())
        cboNpi.DataSource = New BindingSource(NumberType, Nothing)
        cboNpi.DisplayMember = "Key"
        cboNpi.ValueMember = "Value"
        cboNpi.SelectedIndex = 1

        'cboDestinationNpi.Items.AddRange(NumberType.Keys.ToArray())
        cboDestinationNpi.DataSource = New BindingSource(NumberType, Nothing)
        cboDestinationNpi.DisplayMember = "Key"
        cboDestinationNpi.ValueMember = "Value"
        cboDestinationNpi.SelectedIndex = 0

    End Sub

    ''' <summary>
    ''' Handles the Click event of the btnConnect control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub btnConnect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConnect.Click
        Try
            btnConnect.Enabled = False

            ' Get the message gateway
            Dim messageGateway As MessageGateway(Of ISmppGateway, SmppGatewayConfiguration) = messageGateway.NewInstance()

            ' Create the configuration instance
            Dim smppGatewayConfiguration__1 As SmppGatewayConfiguration = SmppGatewayConfiguration.NewInstance()

            ' Set the port
            Try
                smppGatewayConfiguration__1.Port = Convert.ToInt16(txtPort.Text)
            Catch ex As Exception
                ShowError(ex.Message)
                Return
            End Try

            ' Set the bind type
            Dim bindingType As BindingType
            If SystemMode.TryGetValue(cboSystemMode.Text, bindingType) Then
                smppGatewayConfiguration__1.BindType = bindingType
            End If

            ' Set other connection information
            smppGatewayConfiguration__1.Host = txtServer.Text
            smppGatewayConfiguration__1.Password = txtPassword.Text
            smppGatewayConfiguration__1.SystemId = txtSystemId.Text
            smppGatewayConfiguration__1.SystemType = txtSystemType.Text
            If Not String.IsNullOrEmpty(txtAddressRange.Text) Then
                smppGatewayConfiguration__1.AddressRange = txtAddressRange.Text
            End If

            ' Set the version
            Dim smppVersionType As SmppVersionType
            If InterfaceVersion.TryGetValue(cboInterfaceVersion.Text, smppVersionType) Then
                smppGatewayConfiguration__1.Version = smppVersionType
            End If

            ' Server keep alive
            smppGatewayConfiguration__1.EnquireLinkInterval = Convert.ToInt32(txtServerKeepAlive.Text)

            ' Sleep after socket failure
            smppGatewayConfiguration__1.SleepTimeAfterSocketFailure = Convert.ToInt32(txtSleepAfterSocketFailure.Text)


            ' Set Ton type
            Dim tonType As TonType
            If TypeOfNumber.TryGetValue(cboTon.Text, tonType) Then
                smppGatewayConfiguration__1.TonType = tonType
            End If

            ' Set NPI
            Dim npiType As NpiType
            If NumberType.TryGetValue(cboNpi.Text, npiType) Then
                smppGatewayConfiguration__1.NpiType = npiType
            End If

            ' Set to verbose
            smppGatewayConfiguration__1.LogLevel = Core.Log.LogLevel.Verbose


            ' Get the gateway
            smppGateway = messageGateway.Find(smppGatewayConfiguration__1)

            ' Display the log file  path
            txtLogFile.Text = smppGateway.LogFile

            ' Now we bind
            smppGateway.Bind()


            ' Bind the events                
            'smppGateway.OnDeliverSm += New DeliverSmEventHandler(AddressOf smppGateway_OnDeliverSm)
            'smppGateway.OnAlert += New AlertEventHandler(AddressOf smppGateway_OnAlert)
            AddHandler smppGateway.OnDeliverSm, AddressOf Me.smppGateway_OnDeliverSm
            AddHandler smppGateway.OnAlert, AddressOf Me.smppGateway_OnAlert




            ' You can bind additional events if you want


            MessageBox.Show("Connected to gateway successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            btnConnect.Enabled = True
        End Try
    End Sub


    ''' <summary>
    ''' Handles the OnAlert event of the smppGateway control.
    ''' </summary>
    ''' <param name="source">The source of the event.</param>
    ''' <param name="e">The <see cref="MessagingToolkit.Core.Smpp.EventObjects.AlertEventArgs"/> instance containing the event data.</param>
    Private Sub smppGateway_OnAlert(ByVal source As Object, ByVal e As AlertEventArgs)
        Output("AlertPdu")
        Output("-------------------------------")
        Output("Source address: {0}", e.AlertPdu.SourceAddress)
        Output("Command status: {0}", e.AlertPdu.CommandStatus)
        Output(vbCr & vbLf)
    End Sub

    ''' <summary>
    ''' Handles the OnDeliverSm event of the smppGateway control.
    ''' </summary>
    ''' <param name="source">The source of the event.</param>
    ''' <param name="e">The <see cref="MessagingToolkit.Core.Smpp.EventObjects.DeliverSmEventArgs"/> instance containing the event data.</param>
    Private Sub smppGateway_OnDeliverSm(ByVal source As Object, ByVal e As DeliverSmEventArgs)
        Output("DeliveredSmPdu")
        Output("-------------------------------")
        Output("Source address: {0}, Destination address: {1}", e.DeliverSmPdu.SourceAddress, e.DeliverSmPdu.DestinationAddress)
        If e.DeliverSmPdu.SmLength > 1 Then
            Output("Content: {0} ", e.DeliverSmPdu.ShortMessage)
        Else
            Try
                Output("Content: {0} ", e.DeliverSmPdu.MessagePayload)
            Catch ex As Exception
            End Try
        End If
        Output(vbCr & vbLf)
    End Sub

    ''' <summary>
    ''' Handles the FormClosing event of the frmSMPP control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
    Private Sub frmSMPP_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
        If smppGateway IsNot Nothing Then
            smppGateway.Unbind()
            smppGateway = SmppGatewayFactory.[Default]
        End If
    End Sub

    ''' <summary>
    ''' Handles the Click event of the btnDisconnect control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub btnDisconnect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisconnect.Click
        If smppGateway IsNot Nothing Then
            smppGateway.Unbind()
            smppGateway = SmppGatewayFactory.[Default]

            MessageBox.Show("Disconnected from gateway", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ''' <summary>
    ''' Handles the Click event of the tabMain control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub tabMain_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tabMain.Click
        
    End Sub

    Private Sub btnSendSms_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendSms.Click
        Try
            If String.IsNullOrEmpty(txtRecipients.Text) Then
                ShowError("Recipients must not be empty")
                Return
            End If

            If String.IsNullOrEmpty(txtMessage.Text) Then
                ShowError("Message must not be empty")
                Return
            End If

            If String.IsNullOrEmpty(txtSourceAddress.Text) Then
                ShowError("Source address must not be empty")
                Return
            End If

            btnSendSms.Enabled = False

            Dim recipients As String() = txtRecipients.Text.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)

            If recipients.Length() = 1 Then
                ' Only 1 recipient
                Dim submitSm As New SmppSubmitSm()
                submitSm.SourceAddress = txtSourceAddress.Text

                ' Set the version
                Dim smppVersionType As SmppVersionType
                If InterfaceVersion.TryGetValue(cboInterfaceVersion.Text, smppVersionType) Then
                    submitSm.ProtocolId = smppVersionType
                End If

                ' Set Ton type
                Dim tonType As TonType
                If TypeOfNumber.TryGetValue(cboSourceTon.Text, tonType) Then
                    submitSm.SourceAddressTon = tonType
                End If

                ' Set NPI
                Dim npiType As NpiType
                If NumberType.TryGetValue(cboSourceNpi.Text, npiType) Then
                    submitSm.SourceAddressNpi = npiType
                End If


                ' Set the destination address
                submitSm.DestinationAddress = recipients(0)

                ' Set Ton type                
                If TypeOfNumber.TryGetValue(cboDestinationTon.Text, tonType) Then
                    submitSm.DestinationAddressTon = tonType
                End If

                ' Set NPI              
                If NumberType.TryGetValue(cboDestinationNpi.Text, npiType) Then
                    submitSm.DestinationAddressNpi = npiType
                End If

                ' Set data coding
                Dim dataCoding__1 As DataCodingType
                If DataCoding.TryGetValue(cboDataCoding.Text, dataCoding__1) Then
                    submitSm.DataCoding = dataCoding__1
                End If

                ' Set the message content
                If Not chkPayload.Checked Then
                    submitSm.ShortMessage = txtMessage.Text
                Else
                    submitSm.MessagePayload = txtMessage.Text
                End If

                If chkDeliveryReport.Checked Then
                    submitSm.AlertOnMsgDelivery = 1
                End If

                smppGateway.SendPdu(submitSm)
            Else
                ' Multiple recipients
                Dim submitMulti As New SmppSubmitMulti()

                ' Set Ton type
                Dim tonType As TonType
                If TypeOfNumber.TryGetValue(cboSourceTon.Text, tonType) Then
                    submitMulti.SourceAddressTon = tonType
                End If

                ' Set NPI
                Dim npiType As NpiType
                If NumberType.TryGetValue(cboSourceNpi.Text, npiType) Then
                    submitMulti.SourceAddressNpi = npiType
                End If

                ' Set Ton type                

                If TypeOfNumber.TryGetValue(cboDestinationTon.Text, tonType) Then
                End If

                ' Set NPI              

                If NumberType.TryGetValue(cboDestinationNpi.Text, npiType) Then
                End If

                Dim destinationAddressList As DestinationAddress() = New DestinationAddress(recipients.Length - 1) {}
                Dim i As Integer = 0
                For Each recipient As String In recipients
                    Dim destAddress As New DestinationAddress(tonType, npiType, recipient)
                    destinationAddressList(System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)) = destAddress
                Next
                submitMulti.DestinationAddresses = destinationAddressList


                ' Set the version
                Dim smppVersionType As SmppVersionType
                If InterfaceVersion.TryGetValue(cboInterfaceVersion.Text, smppVersionType) Then
                    submitMulti.ProtocolId = smppVersionType
                End If

                ' Set data coding
                Dim dataCoding__1 As DataCodingType
                If DataCoding.TryGetValue(cboDataCoding.Text, dataCoding__1) Then
                    submitMulti.DataCoding = dataCoding__1
                End If

                ' Set the message content
                If Not chkPayload.Checked Then
                    submitMulti.ShortMessage = txtMessage.Text
                Else
                    submitMulti.MessagePayload = txtMessage.Text
                End If

                If chkDeliveryReport.Checked Then
                    submitMulti.AlertOnMsgDelivery = 1
                End If

                ' Send the PDU

                smppGateway.SendPdu(submitMulti)
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            btnSendSms.Enabled = True
        End Try
    End Sub

    ''' <summary>
    ''' Outputs the specified text.
    ''' </summary>
    ''' <param name="text">The text.</param>
    Private Sub Output(ByVal text As String)
        If Me.txtReceivedMessage.InvokeRequired Then
            Dim stc As New SetTextCallback(AddressOf Output)
            Me.Invoke(stc, New Object() {text})
        Else
            txtReceivedMessage.AppendText(text)
            txtReceivedMessage.AppendText(vbCr & vbLf)
        End If
    End Sub

    ''' <summary>
    ''' Outputs the specified text.
    ''' </summary>
    ''' <param name="text">The text.</param>
    ''' <param name="args">The args.</param>
    Private Sub Output(ByVal text As String, ByVal ParamArray args As Object())
        Dim msg As String = String.Format(text, args)
        Output(msg)
    End Sub

    Private Sub tabMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabMain.SelectedIndexChanged
        Dim tc As TabControl = DirectCast(sender, TabControl)
        If tc.SelectedTab Is tabAbout Then
            Dim mobileGateway_assembly = DirectCast(smppGateway, Object)
            Dim assembly__1 As Assembly = Assembly.GetAssembly(mobileGateway_assembly.GetType())

            Dim name As String = assembly__1.GetName().Name
            Dim version As String = assembly__1.GetName().Version.ToString()
            Dim title As String = String.Empty
            Dim description As String = String.Empty
            Dim attributes As Object() = assembly__1.GetCustomAttributes(GetType(AssemblyTitleAttribute), False)
            If attributes.Length = 1 Then
                title = DirectCast(attributes(0), AssemblyTitleAttribute).Title
            End If
            attributes = assembly__1.GetCustomAttributes(GetType(AssemblyDescriptionAttribute), False)
            If attributes.Length = 1 Then
                description = DirectCast(attributes(0), AssemblyDescriptionAttribute).Description
            End If
            lblAbout.Text = title & vbLf & version
            If smppGateway.License.Valid Then
                lblLicense.Text = "Licensed Copy"
            Else
                lblLicense.Text = "Community Copy"

            End If
        End If
    End Sub
End Class