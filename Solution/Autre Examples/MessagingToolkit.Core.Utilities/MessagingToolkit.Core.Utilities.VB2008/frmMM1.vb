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
'
'
'



Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.IO.Ports
Imports System.IO
Imports System.Reflection
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Drawing.Imaging

Imports MessagingToolkit.Core
Imports MessagingToolkit.Core.Log
Imports MessagingToolkit.Core.Mobile.Message
Imports MessagingToolkit.Core.Mobile.Event
Imports MessagingToolkit.Core.Helper
Imports MessagingToolkit.MMS
Imports MessagingToolkit.Core.Utilities
Imports MessagingToolkit.Core.Mobile

Imports MessagingToolkit.Barcode.Multi
Imports MessagingToolkit.Barcode
Imports MessagingToolkit.Barcode.Common

Imports System.Runtime.InteropServices
Imports System.IntPtr

Imports MessagingToolkit.Pdu
Imports MessagingToolkit.Pdu.Ie
Imports MessagingToolkit.Pdu.WapPush
Imports System.Collections

''' <summary>
''' MMS MM1 form
''' </summary>
Partial Public Class frmMM1
    Inherits Form

    ''' <summary>
    ''' Mobile gateway interface
    ''' </summary>
    Private mobileGateway As IMobileGateway = MobileGatewayFactory.[Default]

    ''' <summary>
    ''' Mobile gateway configuration
    ''' </summary>
    Private config As MobileGatewayConfiguration = MobileGatewayConfiguration.NewInstance()

    ''' <summary>
    ''' Port parity lookup
    ''' </summary>
    Private Parity As New Dictionary(Of String, PortParity)()

    ''' <summary>
    ''' Port stop bits lookup
    ''' </summary>
    Private StopBits As New Dictionary(Of String, PortStopBits)()

    ''' <summary>
    ''' Port handshake lookup
    ''' </summary>
    Private Handshake As New Dictionary(Of String, PortHandshake)()

    ''' <summary>
    ''' Message priority in queue
    ''' </summary>
    Private QueuePriority As New Dictionary(Of String, MessageQueuePriority)()

    ''' <summary>
    ''' MMS providers
    ''' </summary>
    Private mmsProviders As SortedDictionary(Of String, List(Of String))

    ''' <summary>
    ''' MMS provider file extension
    ''' </summary>
    Private Const ProviderFileExtension As String = ".mm1"

    ''' <summary>
    ''' Path which points to the embedded resource in the assembly
    ''' </summary>
    Private Const MMSProviderPath As String = "MMSProviders"



    ''' <summary>
    ''' Content types
    ''' </summary>
    Private ContentTypeMapping As New Dictionary(Of String, String)()

    ''' <summary>
    ''' Text content type mapping
    ''' </summary>
    Private TextContentTypeMapping As New Dictionary(Of String, ContentType)()


    ''' <summary>
    ''' Image content type mapping
    ''' </summary>
    Private ImageContentTypeMapping As New Dictionary(Of String, ContentType)()
    ''' <summary>
    ''' Audio content type mapping
    ''' </summary>
    Private AudioContentTypeMapping As New Dictionary(Of String, ContentType)()

    ''' <summary>
    ''' List of message contents
    ''' </summary>
    Private mmsContents As New List(Of MessagingToolkit.Core.Utilities.MessageContent)(3)


    Private Shared CustomDateTimeFormat As String = "dd MMM yyyy, hh:mm:ss tt"

    ''' <summary>
    ''' Message type lookup
    ''' </summary>
    Private MessageType As New Dictionary(Of String, MessageStatusType)()

    ''' <summary>
    ''' Message encoding
    ''' </summary>
    Private MessageEncoding As New Dictionary(Of String, MessageDataCodingScheme)()

    ''' <summary>
    ''' Message split option
    ''' </summary>
    Private MessageSplit As New Dictionary(Of String, MessageSplitOption)()


    ''' <summary>
    ''' Message priority in queue
    ''' </summary>
    Private ValidityPeriod As New Dictionary(Of String, MessageValidPeriod)()


    ' WAP push signal
    Private ServiceIndication As New Dictionary(Of String, ServiceIndicationAction)()

    ' vCard home work types
    Private vCardHomeWorkTypes As New Dictionary(Of String, HomeWorkTypes)()

    ' vCard phone types
    Private vCardPhoneTypes As New Dictionary(Of String, PhoneTypes)()


    ' Log level
    Private LoggingLevel As New Dictionary(Of String, LogLevel)()

    ' Message class
    Private MessageClass As New Dictionary(Of String, Integer)()

    ' DCS Message class enum lookup
    Private DcsMessageClass As New Dictionary(Of String, MessageClasses)()

    Private Sub LoadDictionaries()
        Parity.Add("None", PortParity.None)
        Parity.Add("Odd", PortParity.Odd)
        Parity.Add("Even", PortParity.Even)
        Parity.Add("Mark", PortParity.Mark)
        Parity.Add("Space", PortParity.Space)



        StopBits.Add("1", PortStopBits.One)
        StopBits.Add("1.5", PortStopBits.OnePointFive)
        StopBits.Add("2", PortStopBits.Two)
        StopBits.Add("None", PortStopBits.None)

        '      ''' <summary>
        '      ''' Port handshake lookup
        '      ''' </summary>
        Handshake.Add("None", PortHandshake.None)
        Handshake.Add("RequestToSendXOnXOff", PortHandshake.RequestToSendXOnXOff)
        Handshake.Add("XOnXOff", PortHandshake.XOnXOff)
        Handshake.Add("RequestToSend", PortHandshake.RequestToSend)



        ''' <summary>
        ''' Message priority in queue
        ''' </summary>
        QueuePriority.Add("Low", MessageQueuePriority.Low)
        QueuePriority.Add("Normal", MessageQueuePriority.Normal)
        QueuePriority.Add("High", MessageQueuePriority.High)




        ''' <summary>
        ''' Content types
        ''' </summary>
        'mms.MultimediaMessage.
        ContentTypeMapping.Add("Plain Text", MmsConstants.ContentTypeTextPlain)
        ContentTypeMapping.Add("HTML", MmsConstants.ContentTypeTextHtml)
        ContentTypeMapping.Add("WML", MmsConstants.ContentTypeTextWml)
        ContentTypeMapping.Add("GIF", MmsConstants.ContentTypeImageGif)
        ContentTypeMapping.Add("JPEG", MmsConstants.ContentTypeImageJpeg)
        ContentTypeMapping.Add("TIFF", MmsConstants.ContentTypeImageTiff)
        ContentTypeMapping.Add("PNG", MmsConstants.ContentTypeImagePng)
        ContentTypeMapping.Add("WBMP", MmsConstants.ContentTypeImageWbmp)
        ContentTypeMapping.Add("AMR", MmsConstants.ContentTypeAudioAmr)
        ContentTypeMapping.Add("SMIL", MmsConstants.ContentTypeApplicationSmil)
        ContentTypeMapping.Add("IMELODY", MmsConstants.ContentTypeaAudioIMelody)
        ContentTypeMapping.Add("MIDI", MmsConstants.ContentTypeAudioMidi)


        ''' <summary>
        ''' Text content type mapping
        ''' </summary>
        TextContentTypeMapping.Add(".txt", ContentType.TextPlain)
        TextContentTypeMapping.Add(".html", ContentType.TextHtml)
        TextContentTypeMapping.Add(".wml", ContentType.TextWml)



        ''' <summary>
        ''' Image content type mapping
        ''' </summary>
        ImageContentTypeMapping.Add(".gif", ContentType.ImageGif)
        ImageContentTypeMapping.Add(".jpg", ContentType.ImageJpeg)
        ImageContentTypeMapping.Add(".tiff", ContentType.ImageTiff)
        ImageContentTypeMapping.Add(".png", ContentType.ImagePng)
        ImageContentTypeMapping.Add(".wbmp", ContentType.ImageWbmp)


        ''' <summary>
        ''' Audio content type mapping
        ''' </summary>
        AudioContentTypeMapping.Add(".amr", ContentType.AudioAmr)
        AudioContentTypeMapping.Add(".imelody", ContentType.AudioIMelody)
        AudioContentTypeMapping.Add(".mid", ContentType.AudioMidi)




        ''' <summary>
        ''' Message type lookup
        ''' </summary>
        MessageType.Add("Received Unread Message", MessageStatusType.ReceivedUnreadMessages)
        MessageType.Add("Received Read Message", MessageStatusType.ReceivedReadMessages)
        MessageType.Add("Stored Unsent Message", MessageStatusType.StoredUnsentMessages)
        MessageType.Add("Stored Sent Message", MessageStatusType.StoredSentMessages)
        MessageType.Add("All Message", MessageStatusType.AllMessages)



        ''' <summary>
        ''' Message encoding
        ''' </summary>
        MessageEncoding.Add("Auto Detect", MessageDataCodingScheme.Undefined)
        MessageEncoding.Add("Default Alphabet - 7 Bits", MessageDataCodingScheme.DefaultAlphabet)
        MessageEncoding.Add("ANSI - 8 Bits", MessageDataCodingScheme.EightBits)
        MessageEncoding.Add("Unicode - 16 Bits", MessageDataCodingScheme.Ucs2)


        ''' <summary>
        ''' Message split option
        ''' </summary>
        MessageSplit.Add("Truncate", MessageSplitOption.Truncate)
        MessageSplit.Add("Simple Split", MessageSplitOption.SimpleSplit)
        MessageSplit.Add("Concatenate", MessageSplitOption.Concatenate)



        ''' <summary>
        ''' Message priority in queue
        ''' </summary>
        ValidityPeriod.Add("1 Hour", MessageValidPeriod.OneHour)
        ValidityPeriod.Add("3 Hours", MessageValidPeriod.ThreeHours)
        ValidityPeriod.Add("6 Hours", MessageValidPeriod.SixHours)
        ValidityPeriod.Add("12 Hours", MessageValidPeriod.TwelveHours)
        ValidityPeriod.Add("1 Day", MessageValidPeriod.OneDay)
        ValidityPeriod.Add("1 Week", MessageValidPeriod.OneWeek)
        ValidityPeriod.Add("Maximum", MessageValidPeriod.Maximum)


        ' WAP push signal
        ServiceIndication.Add("None", ServiceIndicationAction.SignalNone)
        ServiceIndication.Add("Low", ServiceIndicationAction.SignalLow)
        ServiceIndication.Add("Medium", ServiceIndicationAction.SignalMedium)
        ServiceIndication.Add("High", ServiceIndicationAction.SignalHigh)


        ' vCard home work types
        vCardHomeWorkTypes.Add("None", HomeWorkTypes.None)
        vCardHomeWorkTypes.Add("Home", HomeWorkTypes.Home)
        vCardHomeWorkTypes.Add("Work", HomeWorkTypes.Work)


        ' vCard phone types
        vCardPhoneTypes.Add("None", PhoneTypes.None)
        vCardPhoneTypes.Add("Voice", PhoneTypes.Voice)
        vCardPhoneTypes.Add("Fax", PhoneTypes.Fax)
        vCardPhoneTypes.Add("Msg", PhoneTypes.Msg)
        vCardPhoneTypes.Add("Cell", PhoneTypes.Cell)
        vCardPhoneTypes.Add("Pager", PhoneTypes.Pager)


        ' Log level
        LoggingLevel.Add("Error", LogLevel.[Error])
        LoggingLevel.Add("Warn", LogLevel.Warn)
        LoggingLevel.Add("Info", LogLevel.Info)
        LoggingLevel.Add("Verbose", LogLevel.Verbose)


        ' Message class
        MessageClass.Add("None", 0)
        MessageClass.Add("ME", PduUtils.DcsMessageClassMe)
        MessageClass.Add("SIM", PduUtils.DcsMessageClassSim)
        MessageClass.Add("TE", PduUtils.DcsMessageClassTe)


        ' DCS Message class enum lookup
        DcsMessageClass.Add("None", MessageClasses.None)
        DcsMessageClass.Add("ME", MessageClasses.[Me])
        DcsMessageClass.Add("SIM", MessageClasses.Sim)
        DcsMessageClass.Add("TE", MessageClasses.Te)
    End Sub


    Private Delegate Sub DisplayMessageLogDelegate(ByVal e As MessageReceivedEventArgs)
    Private Delegate Sub DisplayCallLogDelegate(ByVal e As IncomingCallEventArgs)
    Private Delegate Sub DisplayUssdResponseDelegate(ByVal e As UssdReceivedEventArgs)

    Private displayMessageLog As DisplayMessageLogDelegate
    Private displayUssdResponse As DisplayUssdResponseDelegate
    Private displayCallLog As DisplayCallLogDelegate



    ''' <summary>
    ''' Handles the Load event of the frmMM1 control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub frmMM1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LoadDictionaries()

        ' Add the port
        Dim portNames As String() = SerialPort.GetPortNames()
        Array.Sort(portNames)
        Dim sortedList = portNames

        For Each port As String In sortedList
            If Not cboPort.Items.Contains(port) Then
                cboPort.Items.Add(port)
            End If
        Next
        cboPort.SelectedIndex = 0

        ' Add baud rate
        For Each baudRate As String In [Enum].GetNames(GetType(PortBaudRate))
            cboBaudRate.Items.Add(CInt([Enum].Parse(GetType(PortBaudRate), baudRate)))
        Next
        cboBaudRate.Text = "115200"

        ' Add data bits
        For Each dataBit As String In [Enum].GetNames(GetType(PortDataBits))
            cboDataBits.Items.Add(CInt([Enum].Parse(GetType(PortDataBits), dataBit)))
        Next
        cboDataBits.Text = "8"

        ' Add parity            
        'cboParity.Items.AddRange(Parity.Keys.ToArray())
        cboParity.DataSource = New BindingSource(Parity, Nothing)
        cboParity.DisplayMember = "Key"
        cboParity.ValueMember = "Value"
        cboParity.SelectedIndex = 0

        ' Add stop bits         
        'cboStopBits.Items.AddRange(StopBits.Keys.ToArray())
        cboStopBits.DataSource = New BindingSource(StopBits, Nothing)
        cboStopBits.DisplayMember = "Key"
        cboStopBits.ValueMember = "Value"
        cboStopBits.SelectedIndex = 0

        ' Add handshake          
        'cboHandshake.Items.AddRange(Handshake.Keys.ToArray())
        cboHandshake.DataSource = New BindingSource(Handshake, Nothing)
        cboHandshake.DisplayMember = "Key"
        cboHandshake.ValueMember = "Value"
        cboHandshake.SelectedIndex = 0

        ' Queue priority
        'cboQueuePriority.Items.AddRange(QueuePriority.Keys.ToArray())
        cboQueuePriority.DataSource = New BindingSource(QueuePriority, Nothing)
        cboQueuePriority.DisplayMember = "Key"
        cboQueuePriority.ValueMember = "Value"
        cboQueuePriority.SelectedIndex = 1

        ' Populate MMS providers
        PopulateMMSProviders()
        InitializeMMS()

        ' Add available devices
        'cboDevice.Items.AddRange(GatewayHelper.GetActiveDevices().ToArray());
        ' Use this if you want to list all available devices
        cboDevice.Items.AddRange(GatewayHelper.GetAllDevices().ToArray())
        cboDevice.SelectedIndex = 0

        dtpvCalendarStartDateTimeMMS.CustomFormat = CustomDateTimeFormat
        dtpvCalendarStartDateTimeMMS.MinDate = DateTime.Now
        dtpvCalendarEndDateTimeMMS.CustomFormat = CustomDateTimeFormat
        dtpvCalendarEndDateTimeMMS.MinDate = DateTime.Now.AddMinutes(30)


        ' Add message type
        'cboMessageType.Items.AddRange(MessageType.Keys.ToArray())
        cboMessageType.DataSource = New BindingSource(MessageType, Nothing)
        cboMessageType.DisplayMember = "Key"
        cboMessageType.ValueMember = "Value"
        cboMessageType.SelectedIndex = 0

        ' Add message encoding
        'cboMessageEncoding.Items.AddRange(MessageEncoding.Keys.ToArray())
        cboMessageEncoding.DataSource = New BindingSource(MessageEncoding, Nothing)
        cboMessageEncoding.DisplayMember = "Key"
        cboMessageEncoding.ValueMember = "Value"
        cboMessageEncoding.SelectedIndex = 0


        ' Add message encoding
        'cboPduEncoding.Items.AddRange(MessageEncoding.Keys.ToArray())
        cboPduEncoding.DataSource = New BindingSource(MessageEncoding, Nothing)
        cboPduEncoding.DisplayMember = "Key"
        cboPduEncoding.ValueMember = "Value"
        cboPduEncoding.SelectedIndex = 0


        ' Long message option
        'cboLongMessage.Items.AddRange(MessageSplit.Keys.ToArray())
        cboLongMessage.DataSource = New BindingSource(MessageSplit, Nothing)
        cboLongMessage.DisplayMember = "Key"
        cboLongMessage.ValueMember = "Value"
        cboLongMessage.SelectedIndex = 2

        ' Message validity period
        'cboValidityPeriod.Items.AddRange(ValidityPeriod.Keys.ToArray())
        cboValidityPeriod.DataSource = New BindingSource(ValidityPeriod, Nothing)
        cboValidityPeriod.DisplayMember = "Key"
        cboValidityPeriod.ValueMember = "Value"
        cboValidityPeriod.SelectedIndex = 0


        ' Message indication option
        cboMessageIndicationOption.Items.AddRange(New String() {"Trigger", "Polling"})
        cboMessageIndicationOption.SelectedIndex = 0

        ' Service indication signal for WAP push
        'cboWapPushSignal.Items.AddRange(ServiceIndication.Keys.ToArray())
        cboWapPushSignal.DataSource = New BindingSource(ServiceIndication, Nothing)
        cboWapPushSignal.DisplayMember = "Key"
        cboWapPushSignal.ValueMember = "Value"
        cboWapPushSignal.SelectedIndex = 2

        displayMessageLog = New DisplayMessageLogDelegate(AddressOf Me.ShowMessageLog)
        displayCallLog = New DisplayCallLogDelegate(AddressOf Me.ShowCallLog)
        displayUssdResponse = New DisplayUssdResponseDelegate(AddressOf Me.ShowUssdResponse)

        dtpWapPushCreated.MaxDate = DateTime.Now
        dtpWapPushCreated.Value = DateTime.Now.AddDays(-1)

        dtpvCalendarStartDateTime.CustomFormat = CustomDateTimeFormat
        dtpvCalendarStartDateTime.MinDate = DateTime.Now
        dtpvCalendarEndDateTime.CustomFormat = CustomDateTimeFormat
        dtpvCalendarEndDateTime.MinDate = DateTime.Now.AddMinutes(30)

        'cbovCardAddressType.Items.AddRange(vCardHomeWorkTypes.Keys.ToArray())
        cbovCardAddressType.DataSource = New BindingSource(vCardHomeWorkTypes, Nothing)
        cbovCardAddressType.DisplayMember = "Key"
        cbovCardAddressType.ValueMember = "Value"
        cbovCardAddressType.SelectedIndex = 0

        'cbovCardPhoneNumberHomeWorkType.Items.AddRange(vCardHomeWorkTypes.Keys.ToArray())
        cbovCardPhoneNumberHomeWorkType.DataSource = New BindingSource(vCardHomeWorkTypes, Nothing)
        cbovCardPhoneNumberHomeWorkType.DisplayMember = "Key"
        cbovCardPhoneNumberHomeWorkType.ValueMember = "Value"
        cbovCardPhoneNumberHomeWorkType.SelectedIndex = 0

        'cbovCardPhoneNumberType.Items.AddRange(vCardPhoneTypes.Keys.ToArray())
        cbovCardPhoneNumberType.DataSource = New BindingSource(vCardPhoneTypes, Nothing)
        cbovCardPhoneNumberType.DisplayMember = "Key"
        cbovCardPhoneNumberType.ValueMember = "Value"
        cbovCardPhoneNumberType.SelectedIndex = 0

        'cboLoggingLevel.Items.AddRange(LoggingLevel.Keys.ToArray())
        cboLoggingLevel.DataSource = New BindingSource(LoggingLevel, Nothing)
        cboLoggingLevel.DisplayMember = "Key"
        cboLoggingLevel.ValueMember = "Value"
        cboLoggingLevel.SelectedIndex = 0

        'cboMessageClass.Items.AddRange(MessageClass.Keys.ToArray())
        cboMessageClass.DataSource = New BindingSource(MessageClass, Nothing)
        cboMessageClass.DisplayMember = "Key"
        cboMessageClass.ValueMember = "Value"
        cboMessageClass.SelectedIndex = 0

        'cboDcsMessageClass.Items.AddRange(MessageClass.Keys.ToArray())
        cboDcsMessageClass.DataSource = New BindingSource(MessageClass, Nothing)
        cboDcsMessageClass.DisplayMember = "Key"
        cboDcsMessageClass.ValueMember = "Value"
        cboDcsMessageClass.SelectedIndex = 0

    End Sub

    ''' <summary>
    ''' Populates the MMS providers from embedded resources
    ''' </summary>
    Private Sub PopulateMMSProviders()
        Dim RootDir As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location())
        Dim Root As New DirectoryInfo(RootDir & "\MMSProviders")

        Dim Dirs As DirectoryInfo() = Root.GetDirectories("*.*")
        Dim DirectoryName As DirectoryInfo

        Dim Files As FileInfo()
        Dim MyProviders As List(Of String)
        Dim FileName As FileInfo

        mmsProviders = New SortedDictionary(Of String, List(Of String))()
        For Each DirectoryName In Dirs
            Files = DirectoryName.GetFiles("*" & ProviderFileExtension)
            MyProviders = New List(Of String)
            For Each FileName In Files
                MyProviders.Add(Replace(FileName.Name.ToString, FileName.Extension, ""))
            Next

            mmsProviders.Add(DirectoryName.Name.ToString, MyProviders)
        Next
        cboCountry.DataSource = New BindingSource(mmsProviders.Keys, Nothing)
        cboCountry.DisplayMember = "Key"

    End Sub


    Private Sub cboCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboCountry.SelectedIndexChanged
        Dim country As String = cboCountry.Items(cboCountry.SelectedIndex).ToString()
        If Not String.IsNullOrEmpty(country) Then
            ' Get the providers
            Dim providers As List(Of String) = mmsProviders(country)
            providers.Sort()
            cboOperator.Items.Clear()
            cboOperator.Items.AddRange(providers.ToArray())
            cboParity.SelectedIndex = 0
        End If
    End Sub

    Private Sub cboOperator_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboOperator.SelectedIndexChanged
        Dim provider As String = cboOperator.Items(cboOperator.SelectedIndex).ToString()
        Dim country As String = cboCountry.Items(cboCountry.SelectedIndex).ToString()

        Dim RootDir As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location())
        RootDir = RootDir & "\MMSProviders\" & country & "\" & provider & ProviderFileExtension


        If Not String.IsNullOrEmpty(provider) Then
            'Dim stream As Stream = GetEmbeddedFile(resourcePath)
            Dim reader As New StreamReader(RootDir)
            Dim content As String = reader.ReadToEnd()
            Dim lines As String() = content.Split(New String() {vbCr & vbLf, vbCr, vbLf}, StringSplitOptions.None)

            Dim columns As String() = lines(0).Split("="c)
            If columns.Length > 1 Then
                txtMMSC.Text = columns(1)
            End If

            columns = lines(1).Split("="c)
            If columns.Length > 1 Then
                txtWAPGateway.Text = columns(1)
            End If

            columns = lines(2).Split("="c)
            If columns.Length > 1 Then
                txtAPN.Text = columns(1)
            End If

            columns = lines(3).Split("="c)
            If columns.Length > 1 Then
                txtAPNAccount.Text = columns(1)
            End If

            columns = lines(4).Split("="c)
            If columns.Length > 1 Then
                txtAPNPassword.Text = columns(1)

            End If
        End If
    End Sub

    Private Sub cboDevice_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDevice.SelectedIndexChanged
        config.DeviceName = cboDevice.Items(cboDevice.SelectedIndex).ToString()
        If GatewayHelper.GetDeviceConfiguration(config) Then
            cboPort.Text = config.PortName
            cboBaudRate.Text = Convert.ToString(CInt([Enum].Parse(GetType(PortBaudRate), [Enum].GetName(GetType(PortBaudRate), config.BaudRate))))
        Else
            MessageBox.Show("Unable to get port settings. Please configure them manually", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub btnConnect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConnect.Click
        ''######### MMS ###############################
        'This will apply mms configuration for the connection
        If rdoSMSMMS.Checked Then
            If String.IsNullOrEmpty(txtMMSC.Text) OrElse String.IsNullOrEmpty(txtWAPGateway.Text) OrElse String.IsNullOrEmpty(txtAPN.Text) Then
                MessageBox.Show("MMSC, WAP gateway and APN must be configured", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            config.ProviderAPN = txtAPN.Text
            config.ProviderAPNAccount = txtAPNAccount.Text
            config.ProviderAPNPassword = txtAPNPassword.Text
            config.ProviderMMSC = txtMMSC.Text
            config.ProviderWAPGateway = txtWAPGateway.Text
        End If
        ''######### MMS ###############################


        config.PortName = cboPort.Text
        config.BaudRate = DirectCast([Enum].Parse(GetType(PortBaudRate), cboBaudRate.Text), PortBaudRate)
        config.DataBits = DirectCast([Enum].Parse(GetType(PortDataBits), cboDataBits.Text), PortDataBits)



        config.LicenseKey = ""
        config.OperatorSelectionFormat = NetworkOperatorFormat.ShortFormatAlphanumeric



        If Not String.IsNullOrEmpty(txtPin.Text) Then
            config.Pin = txtPin.Text
        End If
        If Not String.IsNullOrEmpty(txtModelConfig.Text) Then
            config.Model = txtModelConfig.Text
        End If

        Dim parity__1 As PortParity
        If Parity.TryGetValue(cboParity.Text, parity__1) Then
            config.Parity = parity__1
        End If

        Dim stopBits__2 As PortStopBits
        If StopBits.TryGetValue(cboStopBits.Text, stopBits__2) Then
            config.StopBits = stopBits__2
        End If

        Dim handshake__3 As PortHandshake
        If Handshake.TryGetValue(cboHandshake.Text, handshake__3) Then
            config.Handshake = handshake__3
        End If

        ' Default not to check for the PIN
        config.DisablePinCheck = chkDisablePinCheck.Checked

        ' Default to verbose by default
        config.LogLevel = LogLevel.Verbose

        ' Create the gateway for mobile
        Dim messageGateway As MessageGateway(Of IMobileGateway, MobileGatewayConfiguration) = messageGateway.NewInstance()

        Try
            btnConnect.Enabled = False
            mobileGateway = messageGateway.Find(config)
            If mobileGateway Is Nothing Then
                ShowError("Error connecting to gateway. Check the log file")
                Return
            End If
            'mobileGateway.LogLevel = LogLevel.Verbose;
            updSendRetries.Value = mobileGateway.Configuration.SendRetries
            updSendWaitInterval.Value = mobileGateway.Configuration.SendWaitInterval
            txtLogFile.Text = mobileGateway.LogFile
            txtSmsc.Text = mobileGateway.ServiceCentreAddress.Number
            updPollingInterval.Value = mobileGateway.Configuration.MessagePollingInterval
            chkDeleteReceivedMessage.Checked = mobileGateway.Configuration.DeleteReceivedMessage

            ' Initialize the data connection
            If mobileGateway.InitializeDataConnection() Then
                ' Attach the events
                AddHandler mobileGateway.MessageSendingFailed, AddressOf OnMessageFailed
                AddHandler mobileGateway.MessageSent, AddressOf OnMessageSent

                mobileGateway.MessageStorage = MessageStorage.Sim

                MessageBox.Show("Gateway is initialized successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ShowError("Unable to initialize gateway. Error is " & vbLf & Convert.ToString(mobileGateway.LastError.Message))
            End If



            If mobileGateway.MessageStorage = MessageStorage.Phone Then
                radPhoneSMS.Checked = True
                radPhoneMMS.Checked = True
            ElseIf mobileGateway.MessageStorage = MessageStorage.Sim Then
                radSimSMS.Checked = True
                radSimMMS.Checked = True
            End If

            AddHandler mobileGateway.MessageReceived, AddressOf OnMessageReceived
            AddHandler mobileGateway.CallReceived, AddressOf OnCallReceived
            AddHandler mobileGateway.GatewayDisconnected, AddressOf OnGatewayDisconnect

            cboCharacterSets.Items.Clear()
            cboCharacterSets.Items.AddRange(mobileGateway.SupportedCharacterSets)

            Dim subscribers As Subscriber() = mobileGateway.Subscribers
            If subscribers.Length > 0 Then
                txtPhoneNumber.Text = subscribers(0).Number
            End If


        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            btnConnect.Enabled = True
        End Try

    End Sub

    Private Sub InitializeMMS()
        txtTransactionId.Text = "1234567890"
        txtPresentationId.Text = "<0000>"

        txtCalenTransitionID.Text = "1234567890"
        txtCalenPresentationId.Text = "<0000>"


        ' Add parity 
        'cboContentType.Items.Clear()
        'cboContentType.Items.AddRange(ContentTypeMapping.Keys.ToArray())
        cboContentType.DataSource = New BindingSource(ContentTypeMapping, Nothing)
        cboContentType.DisplayMember = "Key"
        cboContentType.ValueMember = "Value"
        cboContentType.SelectedIndex = 0

        lstContent.Items.Clear()

        mmsContents.Clear()

        txtFrom.Text = String.Empty
        txtTo.Text = String.Empty
        txtSubject.Text = String.Empty
        txtContentId.Text = String.Empty
        txtContentFileName.Text = String.Empty
        chkDeliveryReport.Checked = False
        chkReadReceipt.Checked = False
    End Sub

    Private Sub btnSendMMS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendMMS.Click
        If String.IsNullOrEmpty(txtTransactionId.Text) Then
            ShowError("Transaction id cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtPresentationId.Text) Then
            ShowError("Presentation id cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtFrom.Text) Then
            ShowError("From cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtTo.Text) Then
            ShowError("To cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtSubject.Text) Then
            ShowError("Subject cannot be empty")
            Return
        End If

        If lstContent.Items.Count <= 0 Then
            ShowError("At least 1 content must be added")
            Return
        End If

        ' Set the headers
        Dim objMMS As Mobile.Message.Mms = Mobile.Message.Mms.NewInstance(txtSubject.Text, txtFrom.Text)

        ' If it is SMIL based
        ' mms.MultipartRelatedType = MultimediaMessageConstants.ContentTypeApplicationSmil;

        ' Multipart mixed
        objMMS.MultipartRelatedType = MultimediaMessageConstants.ContentTypeApplicationMultipartMixed

        objMMS.PresentationId = txtPresentationId.Text
        objMMS.TransactionId = txtTransactionId.Text
        objMMS.AddToAddress(txtTo.Text, MmsAddressType.PhoneNumber)
        objMMS.DeliveryReport = chkDeliveryReport.Checked
        objMMS.ReadReply = chkReadReceipt.Checked


        'Dim CalendarByte As Byte()
        'Dim multimediaMessageContent As New MultimediaMessageContent()
        'CalendarByte = System.Text.Encoding.UTF8.GetBytes(txtcalendar.Text)
        'multimediaMessageContent.SetContent(CalendarByte, 0, CalendarByte.Length)
        ''multimediaMessageContent.SetContent(txtContentFileName.Text)
        'multimediaMessageContent.ContentId = "1"
        'multimediaMessageContent.Type = "text/x-vCalendar"
        'objMMS.AddContent(multimediaMessageContent)
        'objMMS.ContentType = "text/x-vCalendar"
        'objMMS.ContentType = "text/calendar"


        '' Add the contents
        AddContents(objMMS)

        Try
            btnSendMMS.Enabled = False

            If chkSendToQueue.Checked Then
                Dim priority As MessageQueuePriority
                If QueuePriority.TryGetValue(cboQueuePriority.Text, priority) Then
                    objMMS.QueuePriority = priority

                    ' You can also set the MMS message priority here
                    If priority = MessageQueuePriority.High Then
                        objMMS.Priority = MMS.MultimediaMessage.PriorityHigh
                    ElseIf priority = MessageQueuePriority.Normal Then
                        objMMS.Priority = MMS.MultimediaMessage.PriorityNormal
                    ElseIf priority = MessageQueuePriority.Low Then
                        objMMS.Priority = MMS.MultimediaMessage.PriorityLow

                    End If
                End If

                If mobileGateway.SendToQueue(objMMS) Then
                    MessageBox.Show("Message is queued successfully for " & objMMS.[To](0).GetAddress(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                If mobileGateway.Send(objMMS) Then
                    MessageBox.Show("Message is sent successfully. Message id is " + objMMS.MessageId, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Catch ex As Exception
            ShowError(ex.Message)
            ShowError(ex.StackTrace)
        Finally
            btnSendMMS.Enabled = True
        End Try

    End Sub


    ''' <summary>
    ''' Adds the contents.
    ''' </summary>
    ''' <param name="message">The message.</param>
    Private Sub AddContents(ByVal message As MultimediaMessage)
        For Each content As MessagingToolkit.Core.Utilities.MessageContent In mmsContents
            Dim multimediaMessageContent As New MultimediaMessageContent()
            multimediaMessageContent.SetContent(content.FileName)
            multimediaMessageContent.ContentId = content.ContentId
            'If "<>" are not used with this method, the result is Content-Location
            multimediaMessageContent.Type = content.ContentType
            message.AddContent(multimediaMessageContent)
        Next
    End Sub

    Private Sub btnResetMMS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetMMS.Click
        InitializeMMS()
    End Sub

    Private Sub ShowError(ByVal errorMessage As String)
        MessageBox.Show(errorMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub frmMM1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If mobileGateway IsNot Nothing Then
                If mobileGateway.Connected Then
                    If mobileGateway.Disconnect() Then
                        mobileGateway = Nothing
                    Else
                        ShowError(mobileGateway.LastError.Message)
                    End If
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub btnDisconnect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisconnect.Click
        Try
            If mobileGateway IsNot Nothing Then
                btnDisconnect.Enabled = False
                If mobileGateway.Disconnect() Then
                    mobileGateway = Nothing
                    mobileGateway = MobileGatewayFactory.[Default]
                    MessageBox.Show("Gateway is disconnected successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ShowError(mobileGateway.LastError.Message)
                End If
            End If
        Finally
            btnDisconnect.Enabled = True
        End Try
    End Sub

    Private Sub btnAddContent_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddContent.Click
        If String.IsNullOrEmpty(txtContentFileName.Text) Then
            ShowError("Content file name must be specified")
            Return
        End If
        If String.IsNullOrEmpty(txtContentId.Text) Then
            ShowError("Content id cannot be empty")
            Return
        End If

        If Not File.Exists(txtContentFileName.Text) Then
            ShowError(txtContentFileName.Text + " does not exist")
            Return
        End If

        Dim contentType As String

        Dim myfile As New FileInfo(txtContentFileName.Text.Trim())

        Try
            Dim aaa As New MIMETypes()
        Catch ex As Exception
            ShowError(ex.Message)
            ShowError(ex.StackTrace)
        End Try
        contentType = MIMETypes.ContentType(myfile.Extension)
        Dim content As New MessagingToolkit.Core.Utilities.MessageContent(contentType, txtContentId.Text.Trim(), txtContentFileName.Text.Trim())
        mmsContents.Add(content)
        'lstContent.Items.Add(cboContentType.Text + "| " + txtContentId.Text + "| " + txtContentFileName.Text);
        lstContent.Items.Add(((contentType & "| ") + txtContentId.Text & "| ") + txtContentFileName.Text)

        'if (ContentTypeMapping.TryGetValue(cboContentType.Text, out contentType))
        '{
        '    MessageContent content = new MessageContent(contentType, txtContentId.Text.Trim(),  txtContentFileName.Text.Trim());
        '    mmsContents.Add(content);
        '    lstContent.Items.Add(cboContentType.Text + "| " + txtContentId.Text + "| " + txtContentFileName.Text);
        '}
    End Sub

    Private Sub btnBrowseContent_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowseContent.Click
        openFileDialog1MMS.Filter = "All Files (*.*)|*.*"
        openFileDialog1MMS.FileName = String.Empty
        Dim dialogResult__1 As DialogResult = openFileDialog1MMS.ShowDialog()
        If dialogResult__1 = DialogResult.OK Then
            Dim fileName As String = openFileDialog1MMS.FileName
            txtContentFileName.Text = fileName
        End If
    End Sub

    Private Sub btnSaveMMS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveMMS.Click
        If String.IsNullOrEmpty(txtTransactionId.Text) Then
            ShowError("Transaction id cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtPresentationId.Text) Then
            ShowError("Presentation id cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtFrom.Text) Then
            ShowError("From cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtTo.Text) Then
            ShowError("To cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtSubject.Text) Then
            ShowError("Subject cannot be empty")
            Return
        End If

        If lstContent.Items.Count <= 0 Then
            ShowError("At least 1 content must be added")
            Return
        End If


        Try
            ' Set the headers
            Dim objMMS As Mobile.Message.Mms = Mobile.Message.Mms.NewInstance(txtSubject.Text, txtFrom.Text)

            objMMS.PresentationId = txtPresentationId.Text
            objMMS.TransactionId = txtTransactionId.Text
            objMMS.AddToAddress(txtTo.Text, MmsAddressType.PhoneNumber)
            objMMS.DeliveryReport = chkDeliveryReport.Checked
            objMMS.ReadReply = chkReadReceipt.Checked

            ' Add the contents
            AddContents(objMMS)

            saveFileDialog1MMS.Filter = "MMS File (*.mms)|*.mms"
            saveFileDialog1MMS.FileName = String.Empty
            Dim dialogResult__2 As DialogResult = saveFileDialog1MMS.ShowDialog()
            If dialogResult__2 = DialogResult.OK Then
                Dim saveFileName As String = saveFileDialog1MMS.FileName

                ' Save MMS file
                If objMMS.SaveToFile(saveFileName) Then

                    MessageBox.Show("File is saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ShowError(("Error saving file " & saveFileName & ": ") + objMMS.LastError.Message)
                End If
            End If
        Catch ex As Exception
            ShowError(ex.Message)
            ShowError(ex.StackTrace)
        End Try
    End Sub

    Private Sub btnBrowserMMSFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowserMMSFile.Click
        openFileDialog1MMS.Filter = "All Files (*.*)|*.*"
        openFileDialog1MMS.FileName = String.Empty
        Dim dialogResult__1 As DialogResult = openFileDialog1MMS.ShowDialog()
        If dialogResult__1 = DialogResult.OK Then
            Dim fileName As String = openFileDialog1MMS.FileName
            txtMMSFile.Text = fileName
        End If
    End Sub

    Private Sub btnSendMMSFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendMMSFile.Click
        If String.IsNullOrEmpty(txtMMSFile.Text) Then
            ShowError("MMS file cannot be empty")
            Return
        End If

        Try
            btnSendMMSFile.Enabled = False

            Dim objMMS As Mobile.Message.Mms = Mobile.Message.Mms.LoadFromFile(txtMMSFile.Text)

            If chkSendToQueue.Checked Then
                Dim priority As MessageQueuePriority
                If QueuePriority.TryGetValue(cboQueuePriority.Text, priority) Then
                    objMMS.QueuePriority = priority

                    ' You can also set the MMS message priority here
                    If priority = MessageQueuePriority.High Then
                        objMMS.Priority = MMS.MultimediaMessage.PriorityHigh
                    ElseIf priority = MessageQueuePriority.Normal Then
                        objMMS.Priority = MMS.MultimediaMessage.PriorityNormal
                    ElseIf priority = MessageQueuePriority.Low Then
                        objMMS.Priority = MMS.MultimediaMessage.PriorityLow
                    End If
                End If

                If mobileGateway.SendToQueue(objMMS) Then
                    MessageBox.Show("Message is queued successfully for " & objMMS.[To](0).GetAddress(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                If mobileGateway.Send(objMMS) Then
                    MessageBox.Show("Message is sent successfully. Message id is " + objMMS.MessageId, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End If
        Catch ex As Exception
            ShowError(ex.Message)
            ShowError(ex.StackTrace)
        Finally
            btnSendMMSFile.Enabled = True
        End Try

    End Sub
    Private Sub btnRefreshQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefreshQueue.Click
        lblMessageQueueCount.Text = "Messages in Queue: " & mobileGateway.GetQueueCount()
    End Sub

    Private Sub btnClearQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClearQueue.Click
        mobileGateway.ClearQueue()
    End Sub

    Private Sub chkEnableQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnableQueue.Click
        If chkEnableQueue.Checked Then

            MessageBox.Show("Messages will be queued and sent out", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Messages will be queued but NOT sent out", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        mobileGateway.IsMessageQueueEnabled = chkEnableQueue.Checked
    End Sub

    Private Sub chkSendToQueue_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSendToQueue.CheckedChanged
        cboQueuePriority.Enabled = chkSendToQueue.Checked
    End Sub

    Private Sub OnMessageSent(ByVal sender As Object, ByVal e As MessageEventArgs)
        Dim mms As Mobile.Message.Mms = DirectCast(e.Message, Mobile.Message.Mms)
        MessageBox.Show(("Message is sent successfully to " & mms.[To](0).GetAddress() & ". Message id is  ") + mms.MessageId, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

        Dim sms As Sms = DirectCast(e.Message, Sms)
        MessageBox.Show(("Message is sent successfully to " + sms.DestinationAddress & ". Message index is  " & sms.ReferenceNo.Item(0) & ". Message identifier is ") + sms.Identifier, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub OnMessageFailed(ByVal sender As Object, ByVal e As MessageErrorEventArgs)
        Dim mms As Mobile.Message.Mms = DirectCast(e.Message, Mobile.Message.Mms)
        MessageBox.Show("Failed to send message to " & mms.[To](0).GetAddress(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])

        Dim sms As Sms = DirectCast(e.Message, Sms)
        MessageBox.Show("Failed to send message to " + sms.DestinationAddress, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub btnBrowse1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowse1.Click
        openFileDialog1MMS.Filter = "All Files (*.*)|*.*"
        openFileDialog1MMS.FileName = String.Empty
        Dim dialogResult__1 As DialogResult = openFileDialog1MMS.ShowDialog()
        If dialogResult__1 = DialogResult.OK Then
            Dim fileName As String = openFileDialog1MMS.FileName
            txtAttachment1.Text = fileName
        End If
    End Sub

    Private Sub btnBrowse2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowse2.Click
        openFileDialog1MMS.Filter = "All Files (*.*)|*.*"
        openFileDialog1MMS.FileName = String.Empty
        Dim dialogResult__1 As DialogResult = openFileDialog1MMS.ShowDialog()
        If dialogResult__1 = DialogResult.OK Then
            Dim fileName As String = openFileDialog1MMS.FileName
            txtAttachment2.Text = fileName
        End If
    End Sub

    Private Sub btnSendMMSSlide_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendMMSSlide.Click
        If String.IsNullOrEmpty(txtMMSSlideFrom.Text) Then
            ShowError("From cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtMMSSlideTo.Text) Then
            ShowError("To cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtMMSSlideSubject.Text) Then
            ShowError("Subject cannot be empty")
            Return
        End If

        Try
            Dim objMMS As Mobile.Message.Mms = Mobile.Message.Mms.NewInstance(txtMMSSlideSubject.Text, txtMMSSlideFrom.Text)
            objMMS.AddToAddress(txtMMSSlideTo.Text, MmsAddressType.PhoneNumber)
            objMMS.DeliveryReport = chkMMSSlideDeliveryReport.Checked
            objMMS.ReadReply = chkMMSSlideReadReceipt.Checked

            ' Add the contents
            If Not String.IsNullOrEmpty(txtBody1.Text) OrElse Not String.IsNullOrEmpty(txtAttachment1.Text) Then
                Dim slide1 As MmsSlide = MmsSlide.NewInstance()
                slide1.Duration = 5


                If Not String.IsNullOrEmpty(txtBody1.Text) Then
                    slide1.AddText(txtBody1.Text)
                End If
                If Not String.IsNullOrEmpty(txtAttachment1.Text) Then
                    Dim contentType As ContentType



                    ' Derive the content type based on file extension
                    Dim fileExtension As String = System.IO.Path.GetExtension(txtAttachment1.Text).ToLower()
                    If TextContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide1.AddAttachment(txtAttachment1.Text, AttachmentType.Text, contentType)
                    ElseIf ImageContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide1.AddAttachment(txtAttachment1.Text, AttachmentType.Image, contentType)
                    ElseIf AudioContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide1.AddAttachment(txtAttachment1.Text, AttachmentType.Audio, contentType)
                    Else
                        ''ãÄÞÊÇ .... ÔÑíÍÉ ÎÇÕÉ ÈÇáãæÚÏ
                        'slide1.AddAttachment(txtAttachment1.Text, AttachmentType.Text, Mobile.ContentType.vCalendar)
                        ''############################

                        ShowError("The file " + txtAttachment1.Text & " is not a valid MMS content file")
                        Return

                    End If
                End If
                objMMS.AddSlide(slide1)

            End If

            'objMMS.ContentType = "text/x-vCalendar"
            'objMMS.ContentType = "text/calendar"



            ' Add the contents
            If Not String.IsNullOrEmpty(txtBody2.Text) OrElse Not String.IsNullOrEmpty(txtAttachment2.Text) Then
                Dim slide2 As MmsSlide = MmsSlide.NewInstance()
                slide2.Duration = 5

                If Not String.IsNullOrEmpty(txtBody2.Text) Then
                    slide2.AddText(txtBody2.Text)
                End If
                If Not String.IsNullOrEmpty(txtAttachment2.Text) Then
                    Dim contentType As ContentType

                    ' Derive the content type based on file extension
                    Dim fileExtension As String = System.IO.Path.GetExtension(txtAttachment2.Text).ToLower()
                    If TextContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide2.AddAttachment(txtAttachment2.Text, AttachmentType.Text, contentType)
                    ElseIf ImageContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide2.AddAttachment(txtAttachment2.Text, AttachmentType.Image, contentType)
                    ElseIf AudioContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide2.AddAttachment(txtAttachment2.Text, AttachmentType.Audio, contentType)
                    Else
                        ''ãÄÞÊÇ .... ÔÑíÍÉ ÎÇÕÉ ÈÇáãæÚÏ
                        'slide2.AddAttachment(txtAttachment2.Text, AttachmentType.Text, Mobile.ContentType.vCalendar)
                        ''############################

                        ShowError("The file " + txtAttachment2.Text & " is not a valid MMS content file")
                        Return
                    End If
                End If
                objMMS.AddSlide(slide2)
            End If


            ' If you want to save the MMS to a file, do the following
            ' mms.SaveToFile("filename");

            btnSendMMSSlide.Enabled = False

            If chkSendToQueue.Checked Then
                Dim priority As MessageQueuePriority
                If QueuePriority.TryGetValue(cboQueuePriority.Text, priority) Then
                    objMMS.QueuePriority = priority

                    ' You can also set the MMS message priority here
                    If priority = MessageQueuePriority.High Then
                        objMMS.Priority = MMS.MultimediaMessage.PriorityHigh
                    ElseIf priority = MessageQueuePriority.Normal Then
                        objMMS.Priority = MMS.MultimediaMessage.PriorityNormal
                    ElseIf priority = MessageQueuePriority.Low Then
                        objMMS.Priority = MMS.MultimediaMessage.PriorityLow

                    End If
                End If

                If mobileGateway.SendToQueue(objMMS) Then
                    MessageBox.Show("Message is queued successfully for " & objMMS.[To](0).GetAddress(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                If mobileGateway.Send(objMMS) Then
                    MessageBox.Show("Message is sent successfully. Message id is " + objMMS.MessageId, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Catch ex As Exception
            ShowError(ex.Message)
            ShowError(ex.StackTrace)
        Finally
            btnSendMMSSlide.Enabled = True
        End Try


    End Sub

    Private Sub btnSaveMMSSlide_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveMMSSlide.Click
        If String.IsNullOrEmpty(txtMMSSlideFrom.Text) Then
            ShowError("From cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtMMSSlideTo.Text) Then
            ShowError("To cannot be empty")
            Return
        End If
        If String.IsNullOrEmpty(txtMMSSlideSubject.Text) Then
            ShowError("Subject cannot be empty")
            Return
        End If

        Try
            Dim objMMS As Mobile.Message.Mms = Mobile.Message.Mms.NewInstance(txtMMSSlideSubject.Text, txtMMSSlideFrom.Text)
            objMMS.AddToAddress(txtMMSSlideTo.Text, MmsAddressType.PhoneNumber)
            objMMS.DeliveryReport = chkMMSSlideDeliveryReport.Checked
            objMMS.ReadReply = chkMMSSlideReadReceipt.Checked

            ' Add the contents
            If Not String.IsNullOrEmpty(txtBody1.Text) OrElse Not String.IsNullOrEmpty(txtAttachment1.Text) Then
                Dim slide1 As MmsSlide = MmsSlide.NewInstance()
                If Not String.IsNullOrEmpty(txtBody1.Text) Then
                    slide1.AddText(txtBody1.Text)
                End If
                If Not String.IsNullOrEmpty(txtAttachment1.Text) Then
                    Dim contentType As ContentType

                    ' Derive the content type based on file extension
                    Dim fileExtension As String = System.IO.Path.GetExtension(txtAttachment1.Text).ToLower()
                    If TextContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide1.AddAttachment(txtAttachment1.Text, AttachmentType.Text, contentType)
                    ElseIf ImageContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide1.AddAttachment(txtAttachment1.Text, AttachmentType.Image, contentType)
                    ElseIf AudioContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide1.AddAttachment(txtAttachment1.Text, AttachmentType.Audio, contentType)
                    Else
                        ShowError("The file " + txtAttachment1.Text & " is not a valid MMS content file")
                        Return

                    End If
                End If
                objMMS.AddSlide(slide1)
            End If


            ' Add the contents
            If Not String.IsNullOrEmpty(txtBody2.Text) OrElse Not String.IsNullOrEmpty(txtAttachment2.Text) Then
                Dim slide2 As MmsSlide = MmsSlide.NewInstance()
                If Not String.IsNullOrEmpty(txtBody2.Text) Then
                    slide2.AddText(txtBody2.Text)
                End If
                If Not String.IsNullOrEmpty(txtAttachment2.Text) Then
                    Dim contentType As ContentType

                    ' Derive the content type based on file extension
                    Dim fileExtension As String = System.IO.Path.GetExtension(txtAttachment2.Text).ToLower()
                    If TextContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide2.AddAttachment(txtAttachment2.Text, AttachmentType.Text, contentType)
                    ElseIf ImageContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide2.AddAttachment(txtAttachment2.Text, AttachmentType.Image, contentType)
                    ElseIf AudioContentTypeMapping.TryGetValue(fileExtension, contentType) Then
                        slide2.AddAttachment(txtAttachment2.Text, AttachmentType.Audio, contentType)
                    Else
                        ShowError("The file " + txtAttachment2.Text & " is not a valid MMS content file")
                        Return
                    End If
                End If
                objMMS.AddSlide(slide2)
            End If

            saveFileDialog1MMS.Filter = "MMS File (*.mms)|*.mms"
            saveFileDialog1MMS.FileName = String.Empty
            Dim dialogResult__2 As DialogResult = saveFileDialog1MMS.ShowDialog()
            If dialogResult__2 = DialogResult.OK Then
                Dim saveFileName As String = saveFileDialog1MMS.FileName

                ' Save MMS file
                If objMMS.SaveToFile(saveFileName) Then
                    MessageBox.Show("File is saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ShowError(("Error saving file " & saveFileName & ": ") + objMMS.LastError.Message)
                End If
            End If
        Catch ex As Exception
            ShowError(ex.Message)
            ShowError(ex.StackTrace)
        End Try
    End Sub

    Private Sub btnResetMMSSlide_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetMMSSlide.Click
        txtMMSSlideFrom.Text = String.Empty
        txtMMSSlideTo.Text = String.Empty
        txtMMSSlideSubject.Text = String.Empty
        chkMMSSlideDeliveryReport.Checked = False
        chkMMSSlideReadReceipt.Checked = False
        txtBody1.Text = "Hello World !!!"
        txtAttachment1.Text = String.Empty
        txtBody2.Text = String.Empty
        txtAttachment2.Text = String.Empty
    End Sub

    Private Sub btnDownloadMMSNotification_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDownloadMMSNotification.Click
        Try
            If radPhoneMMS.Checked Then
                mobileGateway.MessageStorage = MessageStorage.Phone
            End If

            If radSimMMS.Checked Then
                mobileGateway.MessageStorage = MessageStorage.Sim
            End If

            btnDownloadMMSNotification.Enabled = False

            ' If you want to retrieve new MMS notification, then use
            ' List<MessageInformation> mmsNotifications = mobileGateway.GetNotifications(NotificationType.Mms, NotificationStatus.New);

            ' Retrieve all MMS notifications
            'Dim mmsNotifications As List(Of MessageInformation) = mobileGateway.GetNotifications(NotificationType.Mms, NotificationStatus.None)
            Dim mmsNotifications As List(Of MessageInformation)

            If radStatusNew.Checked Then
                mmsNotifications = mobileGateway.GetNotifications(NotificationType.Mms, NotificationStatus.[New])
            Else
                mmsNotifications = mobileGateway.GetNotifications(NotificationType.Mms, NotificationStatus.None)
            End If

            dgdMMSNotifications.DataSource = mmsNotifications

            If mmsNotifications.Count > 0 Then
                lblStatusRetrieveMMS.Text = "Retrieving MMS. Please wait...."


                ' Folder to save the received MMS
                'Dim saveFolder As String = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                'saveFolder += System.IO.Path.DirectorySeparatorChar & "Received MMS" & System.IO.Path.DirectorySeparatorChar
                Dim RootDir As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location())
                Dim saveFolder As String = RootDir & "\MMS Inbox"


                If Not System.IO.Directory.Exists(saveFolder) Then
                    System.IO.Directory.CreateDirectory(saveFolder)
                End If

                For Each messageInformation As MessageInformation In mmsNotifications
                    Dim mms As Mobile.Message.Mms = Nothing
                    If mobileGateway.GetMms(messageInformation, mms) Then
                        Dim fullPathToFile As String = saveFolder + mms.MessageId & ".mms"
                        If mms.SaveToFile(fullPathToFile) Then
                            lblStatusRetrieveMMS.Text = "MMS with message id " + mms.MessageId & " is retrieved and save as " & fullPathToFile
                        Else
                            ShowError(("Error saving file " & fullPathToFile & ": ") + mms.LastError.Message)
                        End If
                    End If
                Next
                lblStatusRetrieveMMS.Text = "All MMS are saved under " & saveFolder
            End If
        Finally
            btnDownloadMMSNotification.Enabled = True
        End Try


    End Sub

    Private Sub btnEncode_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEncode.Click
        Try
            Dim barcodeWriter As New MultiFormatWriter()
            Dim bt As ByteMatrix = barcodeWriter.Encode(txtBarcodeData.Text, BarcodeFormat.QrCode, CInt(numWidth.Value), CInt(numHeight.Value))


            Dim btmImage As Bitmap = ConvertByteMatrixToImage(bt)
            picEncode.Image = btmImage.Clone
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Try
            Dim bm As Bitmap = DirectCast(picEncode.Image, Bitmap)
            If bm IsNot Nothing Then
                Dim sdlg As New SaveFileDialog()
                sdlg.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*"
                If sdlg.ShowDialog() = DialogResult.OK Then
                    bm.Save(sdlg.FileName, ImageFormat.Png)
                    MessageBox.Show("File is saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub



    Private Function ConvertByteMatrixToImage(ByVal bm As ByteMatrix) As Bitmap
        Dim MySByte As Array = bm.Array
        Dim MySByte_W As Array = MySByte.GetValue(0)

        Dim MySByte_One As SByte()

        ReDim MySByte_One(MySByte.Length * MySByte_W.Length)

        '****
        Dim MyBitmap As Bitmap = New Bitmap(bm.Width, bm.Height)
        Dim width As Integer = bm.Width
        Dim height As Integer = bm.Height


        Dim MyColor As Color
        For y As Integer = 0 To height - 1
            ' for each pixel
            Dim x As Integer = 0
            While x < width
                MyColor = New Color
                MyColor = IIf((bm.Array(y)(x) = True), Color.White, Color.Black)
                MyBitmap.SetPixel(x, y, MyColor)
                x += 1
            End While
        Next
        Return MyBitmap
        '****        


        'Dim image As Bitmap = CreateGrayscaleImage(bm.Width, bm.Height)
        'Dim sourceData As BitmapData

        'sourceData = image.LockBits(New Rectangle(0, 0, image.Width, image.Height), ImageLockMode.[ReadOnly], image.PixelFormat)
        'Dim width As Integer = sourceData.Width
        'Dim height As Integer = sourceData.Height
        'Dim srcOffset As Integer = sourceData.Stride - width

        'Dim src As Pointer(Of Byte) = CType(sourceData.Scan0.ToPointer(), Pointer(Of Byte))

        'For y As Integer = 0 To height - 1
        '    ' for each pixel
        '    Dim x As Integer = 0
        '    While x < width
        '        src.Target = CByte(bm.Array(y)(x))
        '        x += 1
        '        src += 1
        '    End While
        '    src += srcOffset
        'Next

        'image.UnlockBits(sourceData)
        'Return image

    End Function



    Public Shared Function CreateGrayscaleImage(ByVal width As Integer, ByVal height As Integer) As Bitmap
        ' create new image
        Dim image As New Bitmap(width, height, PixelFormat.Format8bppIndexed)
        ' set palette to grayscale
        ' get palette
        Dim cp As ColorPalette = image.Palette
        ' init palette
        For i As Integer = 0 To 255
            cp.Entries(i) = Color.FromArgb(i, i, i)
        Next
        ' set palette back
        image.Palette = cp
        ' return new image
        Return image
    End Function

    Private Sub btnSendvCalendar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendvCalendar.Click
        Try
            btnSendvCalendar.Enabled = False

            If Not String.IsNullOrEmpty(txtvCalendarEventSummaryMMS.Text) Then
                Dim vEvent As vEvent = vEvent.NewInstance()
                vEvent.DtStart = dtpvCalendarStartDateTimeMMS.Value
                vEvent.DtEnd = dtpvCalendarEndDateTimeMMS.Value
                vEvent.Summary = txtvCalendarEventSummaryMMS.Text
                vEvent.Location = txtvCalendarEventLocationMMS.Text
                vEvent.Description = txtvCalendarEventDescriptionMMS.Text

                Dim vCalendar As vCalendar = vCalendar.NewInstance(vEvent)

                vCalendar.TimeZone = "4"


                'You can add an vAlarm 
                Dim alarm As vAlarm
                alarm = New vAlarm(Now.AddMinutes(3), New TimeSpan(0, 0, 5, 0), 5, "reminder")
                vEvent.Alarms.Add(alarm)

                '###########
                'add another DALARM >> i'll replace one DALARM with AALARM later
                'Some handset didn't understand DALARM and others didn't understand AALARM
                'And when i add both,it will work at all handsets
                vEvent.Alarms.Add(alarm)
                '###########


                'You can add a recurrence rule
                Dim recurrenceRule As vRecurrenceRule = New vRecurrenceRule(EventRepeat.Daily, vEvent.DtEnd.AddDays(5))
                vEvent.RecurrenceRule = recurrenceRule


                ' Set the headers
                Dim objMMS As Mobile.Message.Mms = Mobile.Message.Mms.NewInstance(txtCalenSubject.Text, txtCalenFrom.Text)

                ' If it is SMIL based
                ' mms.MultipartRelatedType = MultimediaMessageConstants.ContentTypeApplicationSmil;

                ' Multipart mixed
                objMMS.MultipartRelatedType = MultimediaMessageConstants.ContentTypeApplicationMultipartMixed

                objMMS.PresentationId = txtCalenPresentationId.Text
                objMMS.TransactionId = txtCalenTransitionID.Text
                objMMS.AddToAddress(txtCalenTo.Text, MmsAddressType.PhoneNumber)
                objMMS.DeliveryReport = chkCalenDeliveryReport.Checked
                objMMS.ReadReply = chkCalenReadReceipt.Checked


                'txtcalendar.Text = vCalendar.ToString '.Replace("Z00", "").Replace("P0DT5H0M", "PT5M")
                'txtcalendar.Text = vCalendar.ToString.Replace("DALARM", "AALARM")


                '###########
                'To replace the first DALARM with AALARM
                Dim pos As Integer = vCalendar.ToString.IndexOf("DALARM")
                If pos > 0 Then
                    txtcalendar.Text = vCalendar.ToString.Substring(0, pos) & "AALARM" & vCalendar.ToString.Substring(pos + "DALARM".Length)
                End If
                '###########

                '################
                ' I used (txtcalendar) to show only the vCalender code but i can delete this textbox
                '################


                'AALARM;TYPE=X-EPOCSOUND:20110101T102300Z;;;
                '##############################################################
                'Visual (Dispaly) Alarms
                'Visual, or Display, alarms are in the format:
                'DALARM:initial run time;duration of snoozes;number of times to repeat snoozes;Display string 

                'Example()
                'Reminder of Mom's birthday to be displayed one week before her birthday on August 28th, repeated daily.

                'DALARM:20020821T000000;P1D;7;Mom's birthday on the 28th.  Get a card! 
                'Audio(Alarms)
                'Audio alarms are best left to the client because the audio file should be a local file on the user's machine. However, if you really want to do it, format your audio alarm like this:
                'AALARM;TYPE=PCM, WAVE, or AIFF (depending on the sound file type);value=what the alarm is:initial run time;duration of snoozes;number of times to repeat snoozes;where the file is 

                'Example()
                'Audio alarm to dispaly 30 minutes before a meeting at noon on August 30th.  Plays a user's local sound file.  Snoozes every 10 minutes three times.

                'AALARM;TYPE=WAVE;VALUE=URL:20020830T113000;PT10M;3;file:c:\sounds\ping.wav
                '##############################################################


                Dim CalendarByte As Byte()
                Dim multimediaMessageContent As New MultimediaMessageContent()
                CalendarByte = System.Text.Encoding.UTF8.GetBytes(txtcalendar.Text)
                multimediaMessageContent.SetContent(CalendarByte, 0, CalendarByte.Length)
                multimediaMessageContent.ContentId = "3"
                multimediaMessageContent.Type = "text/x-vCalendar"
                objMMS.AddContent(multimediaMessageContent)


                Try
                    btnSendvCalendar.Enabled = False

                    If chkSendToQueue.Checked Then
                        Dim priority As MessageQueuePriority
                        If QueuePriority.TryGetValue(cboQueuePriority.Text, priority) Then
                            objMMS.QueuePriority = priority

                            ' You can also set the MMS message priority here
                            If priority = MessageQueuePriority.High Then
                                objMMS.Priority = MMS.MultimediaMessage.PriorityHigh
                            ElseIf priority = MessageQueuePriority.Normal Then
                                objMMS.Priority = MMS.MultimediaMessage.PriorityNormal
                            ElseIf priority = MessageQueuePriority.Low Then
                                objMMS.Priority = MMS.MultimediaMessage.PriorityLow

                            End If
                        End If

                        If mobileGateway.SendToQueue(objMMS) Then
                            MessageBox.Show("Message is queued successfully for " & objMMS.[To](0).GetAddress(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Else
                        If mobileGateway.Send(objMMS) Then
                            MessageBox.Show("Message is sent successfully. Message id is " + objMMS.MessageId, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                Catch ex As Exception
                    ShowError(ex.Message)
                    ShowError(ex.StackTrace)
                Finally
                    btnSendvCalendar.Enabled = True
                End Try

                '
            End If
        Finally
            btnSendvCalendar.Enabled = True
        End Try
    End Sub


    '############################################################



    Private Sub txtTerminal_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtTerminal.KeyUp
        If e.KeyCode = Keys.Enter Then
            Dim command As String = txtTerminal.Lines(txtTerminal.Lines.Length - 2)
            If Not String.IsNullOrEmpty(command) Then
                Dim output As String = mobileGateway.SendCommand(command)
                Dim lines As String() = output.Split(New Char() {ControlChars.Cr, ControlChars.Lf})
                ScrollToEnd()
                For Each line As String In lines
                    If Not String.IsNullOrEmpty(line) Then
                        txtTerminal.AppendText(line & vbLf)
                    End If
                Next
                txtTerminal.AppendText(vbLf)
            End If
        End If
        ScrollToEnd()
    End Sub

    Private Sub ScrollToEnd()
        txtTerminal.SelectionStart = txtTerminal.Text.Length
        txtTerminal.ScrollToCaret()
    End Sub


    Private Sub btnRefreshPhoneBook_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefreshPhoneBook.Click
        Try
            btnRefreshPhoneBook.Enabled = False
            Dim storage As String = DirectCast(cboPhoneBookStorage.SelectedItem, String)
            If Not String.IsNullOrEmpty(storage) Then
                Dim phoneBookEntries As PhoneBookEntry() = mobileGateway.GetPhoneBook(storage)
                dgdPhoneBook.DataSource = phoneBookEntries
                lblPhoneBookEntryCount.Text = phoneBookEntries.Length & " record(s) found"
            End If
        Finally
            btnRefreshPhoneBook.Enabled = True
        End Try
    End Sub

    Private Sub btnDeleteEntry_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteEntry.Click
        Dim currentRow As DataGridViewRow = dgdPhoneBook.CurrentRow
        Try
            btnDeleteEntry.Enabled = False
            Dim index As Integer = CInt(dgdPhoneBook.CurrentRow.Cells(0).Value)
            Dim dialogResult__1 As DialogResult = MessageBox.Show("Are you sure you want to remove entry with index " & index & " ?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            If dialogResult__1 = DialogResult.Yes Then
                Dim storage As String = DirectCast(cboPhoneBookStorage.SelectedItem, String)
                If Not String.IsNullOrEmpty(storage) Then
                    If mobileGateway.DeletePhoneBookEntry(index, storage) Then
                        MessageBox.Show("Phonebook entry is deleted successfully. Click Refresh button to refresh the view", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("Unable to delete phonebook entry: " & Convert.ToString(mobileGateway.LastError.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
        Finally
            btnDeleteEntry.Enabled = True
        End Try
    End Sub

    Private Sub btnExportToXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportToXml.Click
        Try
            btnExportToXml.Enabled = False
            Dim storage As String = DirectCast(cboPhoneBookStorage.SelectedItem, String)
            If Not String.IsNullOrEmpty(storage) Then
                Dim currentDirectory As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                Dim outputFile As String = currentDirectory & Path.DirectorySeparatorChar & "phonebook.xml"

                Dim phoneBookEntries As PhoneBookEntry() = mobileGateway.GetPhoneBook(storage)
                Dim s As New XmlSerializer(GetType(PhoneBookEntry()))
                Dim w As TextWriter = New StreamWriter(outputFile, False)
                s.Serialize(w, phoneBookEntries)
                w.Close()
                MessageBox.Show("Phonebook entries are saved into " & outputFile, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnExportToXml.Enabled = True
        End Try
    End Sub

    Private Sub btnExportTovCard_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportTovCard.Click
        Try
            btnExportTovCard.Enabled = False
            Dim storage As String = DirectCast(cboPhoneBookStorage.SelectedItem, String)
            If Not String.IsNullOrEmpty(storage) Then
                Dim currentDirectory As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                Dim outputFile As String = currentDirectory & Path.DirectorySeparatorChar & "phonebook.vcard"
                Dim phoneBookEntries As PhoneBookEntry() = mobileGateway.GetPhoneBook(storage)
                Dim vCards As vCard() = mobileGateway.ExportPhoneBookTovCard(phoneBookEntries)
                Dim w As TextWriter = New StreamWriter(outputFile, False)
                For Each vCard As vCard In vCards
                    w.WriteLine(vCard.ToString())
                    w.WriteLine("")
                Next
                w.Close()
                MessageBox.Show("Phonebook entries are saved into " & outputFile, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnExportTovCard.Enabled = True
        End Try
    End Sub

    Private Sub btnRefreshDeviceInformation_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefreshDeviceInformation.Click
        Try
            btnRefreshDeviceInformation.Enabled = False
            Dim deviceInformation As DeviceInformation = mobileGateway.DeviceInformation
            txtModel.Text = deviceInformation.Model
            txtManufacturer.Text = deviceInformation.Manufacturer
            txtFirmware.Text = deviceInformation.FirmwareVersion
            txtSerialNo.Text = deviceInformation.SerialNo
            txtImsi.Text = deviceInformation.Imsi
        Finally
            btnRefreshDeviceInformation.Enabled = True
        End Try
    End Sub

    Private Sub btnDecodePdu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDecodePdu.Click
        If Not String.IsNullOrEmpty(txtPdu.Text) Then
            Dim pduParser As New PduParser()
            Dim pduFactory As New PduFactory()
            Dim pduGenerator As New PduGenerator()
            Dim pdu As Pdu.Pdu
            pdu = pduParser.ParsePdu(txtPdu.Text.Trim())
            If pdu.Binary Then
                pdu.SetDataBytes(pdu.UserDataAsBytes)
            End If
            '
            'string generatedPduString = pduGenerator.GeneratePduString(pdu);
            'pdu = pduParser.ParsePdu(generatedPduString);                
            '
            txtDecodedPdu.Text = pdu.ToString()
        End If
    End Sub

    Private Sub btnApplyConfiguration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApplyConfiguration.Click
        Dim sendRetries As Integer = Convert.ToInt32(updSendRetries.Value)
        Dim pollingInterval As Integer = Convert.ToInt32(updPollingInterval.Value)
        Dim sendWaitInterval As Integer = Convert.ToInt32(updSendWaitInterval.Value)
        Dim deleteReceivedMessage As Boolean = chkDeleteReceivedMessage.Checked
        Dim debugMode As Boolean = chkDebugMode.Checked

        If sendRetries > 0 Then
            mobileGateway.Configuration.SendRetries = sendRetries
        End If
        If pollingInterval > 0 Then
            mobileGateway.Configuration.MessagePollingInterval = pollingInterval
        End If
        If sendWaitInterval > 0 Then
            mobileGateway.Configuration.SendWaitInterval = sendWaitInterval
        End If

        mobileGateway.Configuration.DebugMode = debugMode
        mobileGateway.Configuration.DeleteReceivedMessage = deleteReceivedMessage
    End Sub

    Private Sub btnRefreshGatewayInformation_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefreshGatewayInformation.Click
        Try
            btnRefreshGatewayInformation.Enabled = False
            txtIncomingMessage.Text = Convert.ToString(mobileGateway.Statistics.IncomingSms)
            txtOutgoingMessage.Text = Convert.ToString(mobileGateway.Statistics.OutgoingSms)

            txtIncomingMMS.Text = Convert.ToString(mobileGateway.Statistics.IncomingMms)
            txtOutgoingMMS.Text = Convert.ToString(mobileGateway.Statistics.OutgoingMms)

            txtIncomingCall.Text = Convert.ToString(mobileGateway.Statistics.IncomingCall)
            txtOutgoingCall.Text = Convert.ToString(mobileGateway.Statistics.OutgoingCall)
        Finally
            btnRefreshGatewayInformation.Enabled = True
        End Try
    End Sub

    Private Sub radPhone_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radPhoneMMS.CheckedChanged
        If radPhoneMMS.Checked Then
            mobileGateway.MessageStorage = MessageStorage.Phone
        End If
    End Sub

    Private Sub radSim_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radSimMMS.CheckedChanged
        If radSimMMS.Checked Then
            mobileGateway.MessageStorage = MessageStorage.Sim
        End If
    End Sub

    Private Sub btnExportMessageToXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportMessageToXml.Click
        Try
            btnExportMessageToXml.Enabled = False
            Dim messageType__1 As MessageStatusType
            If MessageType.TryGetValue(cboMessageType.Text, messageType__1) Then
                Dim currentDirectory As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                Dim outputFile As String = currentDirectory & Path.DirectorySeparatorChar & "message.xml"

                Dim messages As List(Of MessageInformation) = DirectCast(mobileGateway.GetMessages(messageType__1), List(Of MessageInformation))
                Dim s As New XmlSerializer(GetType(List(Of MessageInformation)))
                Dim w As TextWriter = New StreamWriter(outputFile, False)
                s.Serialize(w, messages)
                w.Close()
                MessageBox.Show("Messages are saved into " & outputFile, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnExportMessageToXml.Enabled = True
        End Try
    End Sub

    Private Sub btnDeleteMessage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteMessage.Click
        Dim currentRow As DataGridViewRow = dgdMessages.CurrentRow
        Try
            btnDeleteMessage.Enabled = False
            Dim index As Integer = CInt(dgdMessages.CurrentRow.Cells(10).Value)
            Dim totalPiece As Integer = CInt(dgdMessages.CurrentRow.Cells(5).Value)

            If totalPiece > 1 Then
                MessageBox.Show("This is a long message. In order to delete it, you should use MessageInformation.Indexes to retrieve the message indexes and delete it one by one", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Dim dialogResult__1 As DialogResult = MessageBox.Show("Are you sure you want to remove message with index " & index & " ?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            If dialogResult__1 = DialogResult.Yes Then
                Dim messageType__2 As MessageStatusType
                If MessageType.TryGetValue(cboMessageType.Text, messageType__2) Then
                    ' This is assume that the message is not multipart. If it is a long message, then you should
                    ' use MessageInformation.Indexes property to get all indexes, and delete them 1 by 1
                    If totalPiece > 1 Then
                        Dim messageInformation As MessageInformation
                        Dim messageindex As Integer
                        messageInformation = DirectCast(dgdMessages.CurrentRow.DataBoundItem, MessageInformation)
                        For Each messageindex In messageInformation.Indexes
                            If mobileGateway.DeleteMessage(MessageDeleteOption.ByIndex, messageindex) Then
                                MessageBox.Show("Message is deleted successfully. Click Refresh button to refresh the view", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MessageBox.Show("Unable to delete message: " & Convert.ToString(mobileGateway.LastError.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
                            End If
                        Next
                    Else
                        If mobileGateway.DeleteMessage(MessageDeleteOption.ByIndex, index) Then
                            MessageBox.Show("Message is deleted successfully. Click Refresh button to refresh the view", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("Unable to delete message: " & Convert.ToString(mobileGateway.LastError.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
        Finally
            btnDeleteMessage.Enabled = True
        End Try
    End Sub


    Private Sub btnSendMessage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendMessage.Click
        Try
            If Not String.IsNullOrEmpty(txtPhoneNumberList.Text) AndAlso Not String.IsNullOrEmpty(txtMessage.Text) Then
                btnSendMessage.Enabled = False

                Dim phoneNumberList As String() = txtPhoneNumberList.Text.Split(","c)
                For Each phoneNumber As String In phoneNumberList
                    Dim sms__1 As Sms = Sms.NewInstance()
                    'This is a unique identifier you can set for your message
                    'You can use this identifier to identify the message if you queue up the message for sending
                    'Alternatively, if you do not assign a value, the identifier will be generated for you automatically
                    'sms.Identifier = "1234567890"

                    sms__1.DestinationAddress = phoneNumber.Trim()
                    sms__1.Content = txtMessage.Text

                    Dim encoding As MessageDataCodingScheme
                    If MessageEncoding.TryGetValue(cboMessageEncoding.Text, encoding) Then
                        sms__1.DataCodingScheme = encoding
                    End If
                    Dim validity As MessageValidPeriod
                    If ValidityPeriod.TryGetValue(cboValidityPeriod.Text, validity) Then
                        sms__1.ValidityPeriod = validity
                    End If
                    Dim splitOption As MessageSplitOption
                    If MessageSplit.TryGetValue(cboLongMessage.Text, splitOption) Then
                        sms__1.LongMessageOption = splitOption
                    End If


                    If chkStatusReport.Checked Then
                        sms__1.StatusReportRequest = MessageStatusReportRequest.SmsReportRequest
                    End If

                    If chkBatchMessageMode.Checked Then
                        mobileGateway.BatchMessageMode = BatchMessageMode.Temporary
                    Else
                        mobileGateway.BatchMessageMode = BatchMessageMode.Disabled
                    End If

                    If chkUseDefaultSmsc.Checked Then
                        sms__1.ServiceCenterNumber = Sms.DefaultSmscAddress
                    End If

                    ' Set the message class
                    Dim messageClass As MessageClasses
                    If DcsMessageClass.TryGetValue(cboDcsMessageClass.Text, messageClass) Then
                        sms__1.DcsMessageClass = messageClass
                    End If

                    'Put this after message class to override the message class setting
                    'Alternatively, set sms.DcsMessageClass = MessageClasses.Flash and it will do the same thing
                    If chkAlertMessage.Checked Then
                        sms__1.Flash = True
                    End If

                    'If you want to save the sent message, set it to true. Default is false
                    sms__1.SaveSentMessage = chkSaveSentMessage.Checked

                    If chkScheduled.Checked Then
                        'Set scheduled delivery date
                        sms__1.ScheduledDeliveryDate = dtpScheduledDeliveryDate.Value
                        'sms__1.ScheduledDeliveryDate = DateTime.Now.AddMinutes(1); ' for testing
                    End If

                    If chkSendToQueue.Checked Then
                        Dim priority As MessageQueuePriority
                        If QueuePriority.TryGetValue(cboQueuePriority.Text, priority) Then
                            sms__1.QueuePriority = priority
                        End If

                        If mobileGateway.SendToQueue(sms__1) Then
                            'MessageBox.Show("Message is queued successfully for " + sms.DestinationAddress, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Logger.LogThis("Queued message identifier: " + sms__1.Identifier, LogLevel.Verbose)
                        End If
                    Else
                        'sms.SaveSentMessage = true;
                        'mobileGateway.MessageStorage = MessageStorage.Sim;

                        If mobileGateway.Send(sms__1) Then
                            MessageBox.Show("Message is sent successfully to " + sms__1.DestinationAddress & ". Message index is  " & sms__1.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            MessageBox.Show("Message is sent successfully to " + sms__1.DestinationAddress & ". Message ReferenceNo is  " & sms__1.Indexes.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show(mobileGateway.LastError.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
                        End If
                    End If
                Next
            Else
                MessageBox.Show("Phone number and message content cannot be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnSendMessage.Enabled = True
        End Try
    End Sub

    ''' <summary>
    ''' Handles the CheckedChanged event of the chkEnableMessageIndication control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub chkEnableMessageIndication_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnableMessageIndication.CheckedChanged
        cboMessageIndicationOption.Enabled = chkEnableMessageIndication.Checked
    End Sub

    ''' <summary>
    ''' Handles the Click event of the btnApplyMessageSettings control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub btnApplyMessageSettings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApplyMessageSettings.Click
        Try
            btnApplyMessageSettings.Enabled = False
            Dim result As Boolean = False
            If chkEnableMessageIndication.Checked Then
                If cboMessageIndicationOption.Text.Equals("Trigger") Then
                    result = mobileGateway.EnableNewMessageNotification(MessageNotification.ReceivedMessage Or MessageNotification.StatusReport)
                    If result Then
                        mobileGateway.PollNewMessages = False
                    End If
                Else
                    result = mobileGateway.EnableNewMessageNotification(MessageNotification.StatusReport)
                    If result Then
                        mobileGateway.PollNewMessages = True
                    End If
                End If
            Else
                result = mobileGateway.DisableMessageNotifications()
                mobileGateway.PollNewMessages = False
            End If
            mobileGateway.Configuration.DeleteReceivedMessage = chkDeleteAfterReceive.Checked
            If result Then
                MessageBox.Show("Message settings are applied", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Error applying settings", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End If
        Finally
            btnApplyMessageSettings.Enabled = True
        End Try
    End Sub

    ''' <summary>
    ''' Called when [message received].
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="e">The <see cref="MessagingToolkit.Core.Mobile.Event.MessageReceivedEventArgs"/> instance containing the event data.</param>
    Private Sub OnMessageReceived(ByVal sender As Object, ByVal e As MessageReceivedEventArgs)
        txtMessageLog.BeginInvoke(DisplayMessageLog, e)
    End Sub


    ''' <summary>
    ''' Shows the message log.
    ''' </summary>
    ''' <param name="e">The <see cref="MessagingToolkit.Core.Mobile.Event.MessageReceivedEventArgs"/> instance containing the event data.</param>
    Private Sub ShowMessageLog(ByVal e As MessageReceivedEventArgs)
        If e.Message.MessageType = MessageTypeIndicator.MtiSmsDeliver Then
            txtMessageLog.AppendText("Received message from " & Convert.ToString(e.Message.PhoneNumber) & vbLf)
            txtMessageLog.AppendText("Received date: " & Convert.ToString(e.Message.ReceivedDate) & vbLf)
            txtMessageLog.AppendText("Message type: " & Convert.ToString(e.Message.MessageType) & vbLf)
            txtMessageLog.AppendText("Received content: " & Convert.ToString(e.Message.Content) & vbLf)
            txtMessageLog.AppendText(vbLf)
        Else
            If e.Message.DeliveryStatus = MessageStatusReportStatus.Success Then
                txtMessageLog.AppendText("Message is delivered to " & Convert.ToString(e.Message.PhoneNumber) & vbLf)
                txtMessageLog.AppendText("Delivered date: " & Convert.ToString(e.Message.ReceivedDate) & vbLf)
                txtMessageLog.AppendText("Message type: " & Convert.ToString(e.Message.MessageType) & vbLf)
                txtMessageLog.AppendText("Content: " & Convert.ToString(e.Message.Content) & vbLf)
                txtMessageLog.AppendText(vbLf)
            Else
                txtMessageLog.AppendText("Message is not delivered to " & Convert.ToString(e.Message.PhoneNumber) & vbLf)
                txtMessageLog.AppendText("Message type: " & Convert.ToString(e.Message.MessageType) & vbLf)
                txtMessageLog.AppendText("Delivery status: " & Convert.ToString(e.Message.DeliveryStatus) & vbLf)
                txtMessageLog.AppendText("Content: " & Convert.ToString(e.Message.Content) & vbLf)
                txtMessageLog.AppendText(vbLf)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Shows the call log.
    ''' </summary>
    ''' <param name="e">The <see cref="MessagingToolkit.Core.Mobile.Event.IncomingCallEventArgs"/> instance containing the event data.</param>
    Private Sub ShowCallLog(ByVal e As IncomingCallEventArgs)
        txtIncomingCallIndication.AppendText("Call received from " & Convert.ToString(e.CallInformation.Number))
        txtIncomingCallIndication.AppendText(vbLf)
        txtIncomingCallIndication.AppendText("Number type - " & Convert.ToString(e.CallInformation.NumberType))
        txtIncomingCallIndication.AppendText(vbLf & vbLf)
    End Sub

    ''' <summary>
    ''' Handles the Click event of the btnSendWapPush control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub btnSendWapPush_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendWapPush.Click
        Try
            If Not String.IsNullOrEmpty(txtWapPushPhoneNumber.Text) AndAlso Not String.IsNullOrEmpty(txtWapPushUrl.Text) AndAlso Not String.IsNullOrEmpty(txtWapPushMessage.Text) Then

                btnSendWapPush.Enabled = False
                Dim wappush__1 As Wappush = Wappush.NewInstance(txtWapPushPhoneNumber.Text, txtWapPushUrl.Text, txtWapPushMessage.Text)

                Dim signal As ServiceIndicationAction
                If ServiceIndication.TryGetValue(cboWapPushSignal.Text, signal) Then
                    wappush__1.Signal = signal
                End If
                If chkWapPushCreated.Checked Then
                    wappush__1.CreateDate = dtpWapPushCreated.Value
                End If
                If chkWapPushExpiry.Checked Then
                    wappush__1.ExpireDate = dtpWapPushExpiryDate.Value
                End If

                If mobileGateway.Send(wappush__1) Then
                    MessageBox.Show("WAP push message is sent successfully to " + wappush__1.DestinationAddress & ". Message index is  " & wappush__1.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBox.Show("Phone number, URL and message cannot be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnSendWapPush.Enabled = True
        End Try
    End Sub

    Private Sub chkWapPushCreated_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkWapPushCreated.CheckedChanged
        dtpWapPushCreated.Enabled = chkWapPushCreated.Checked
    End Sub

    Private Sub chkWapPushExpiry_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkWapPushExpiry.CheckedChanged
        dtpWapPushExpiryDate.Enabled = chkWapPushExpiry.Checked
    End Sub

    Private Sub btnSendvCalendarSMS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendvCalendarSMS.Click
        Try
            If Not String.IsNullOrEmpty(txtvCalendarPhoneNumber.Text) Then
                btnSendvCalendar.Enabled = False

                If Not String.IsNullOrEmpty(txtvCalendarEventSummary.Text) Then
                    Dim vEvent As vEvent = vEvent.NewInstance()
                    vEvent.DtStart = dtpvCalendarStartDateTime.Value
                    vEvent.DtEnd = dtpvCalendarEndDateTime.Value
                    vEvent.Summary = txtvCalendarEventSummary.Text
                    vEvent.Location = txtvCalendarEventLocation.Text
                    vEvent.Description = txtvCalendarEventDescription.Text


                    Dim vCalendar As vCalendar = vCalendar.NewInstance(vEvent)
                    vCalendar.DestinationAddress = txtvCalendarPhoneNumber.Text

                    vCalendar.TimeZone = "4"


                    'You can add an vAlarm 
                    Dim alarm As vAlarm
                    alarm = New vAlarm(Now.AddMinutes(10), New TimeSpan(1, 0, 0, 0), 2, "reminder")
                    vEvent.Alarms.Add(alarm)


                    'You can add a recurrence rule
                    Dim recurrenceRule As vRecurrenceRule = New vRecurrenceRule(EventRepeat.Daily, vEvent.DtEnd)
                    vEvent.RecurrenceRule = recurrenceRule

                    If mobileGateway.Send(vCalendar) Then
                        MessageBox.Show("vCalendar message is sent successfully to " + vCalendar.DestinationAddress & ". Message index is  " & vCalendar.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

                If Not String.IsNullOrEmpty(txtvCalendarFileLocation.Text) Then
                    Dim fileName As String = txtvCalendarFileLocation.Text
                    If File.Exists(fileName) Then
                        Dim fileContent As String = File.ReadAllText(fileName, Encoding.UTF8)
                        Dim vCalendar As vCalendar = vCalendar.NewInstance()
                        vCalendar.LoadString(fileContent)

                        If mobileGateway.Send(vCalendar) Then
                            MessageBox.Show("vCalendar message is sent successfully to " + vCalendar.DestinationAddress & ". Message index is  " & vCalendar.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If
            Else
                MessageBox.Show("Phone number cannot be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnSendvCalendar.Enabled = True
        End Try
    End Sub

    Private Sub btnBrowservCalendarFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowservCalendarFile.Click
        openFileDialog1SMS.Filter = "vCalendar File (*.vcs)|*.vcs"
        openFileDialog1SMS.FileName = String.Empty
        Dim dialogResult__1 As DialogResult = openFileDialog1SMS.ShowDialog()
        If dialogResult__1 = DialogResult.OK Then
            Dim fileName As String = openFileDialog1SMS.FileName

            ' Read the file content and send as vCalendar
            txtvCalendarFileLocation.Text = fileName
        End If
    End Sub

    Private Sub frmSMS_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If mobileGateway IsNot Nothing Then
                If mobileGateway.Connected Then
                    If mobileGateway.Disconnect() Then
                        mobileGateway = Nothing
                    End If
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub btnSendSmartSms_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendSmartSms.Click
        Try
            If Not String.IsNullOrEmpty(txtSmartSmsPhoneNumber.Text) Then
                btnSendSmartSms.Enabled = False

                If picOtaBitmap.Image IsNot Nothing Then
                    ' Send OTA bitmap message

                    Dim otaBitmap__1 As OtaBitmap = OtaBitmap.NewInstance(New Bitmap(picOtaBitmap.Image))
                    otaBitmap__1.DestinationAddress = txtSmartSmsPhoneNumber.Text
                    If mobileGateway.Send(otaBitmap__1) Then
                        MessageBox.Show("Bitmap message is sent successfully to " + otaBitmap__1.DestinationAddress & ". Message index is  " & otaBitmap__1.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                    ' Send operator logo
                    If Not String.IsNullOrEmpty(txtMCC.Text) AndAlso Not String.IsNullOrEmpty(txtMNC.Text) Then
                        Dim operatorLogo__2 As OperatorLogo = OperatorLogo.NewInstance(New Bitmap(picOtaBitmap.Image), txtMCC.Text, txtMNC.Text)
                        operatorLogo__2.DestinationAddress = txtSmartSmsPhoneNumber.Text
                        If mobileGateway.Send(operatorLogo__2) Then
                            MessageBox.Show("Operator logo is sent successfully to " + operatorLogo__2.DestinationAddress & ". Message index is  " & operatorLogo__2.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If

                ' Send ringtone
                If Not String.IsNullOrEmpty(txtSmartSmsRingtone.Text) Then
                    Dim ringtone__3 As Ringtone = Ringtone.NewInstance()
                    ringtone__3.Content = txtSmartSmsRingtone.Text
                    ringtone__3.DestinationAddress = txtSmartSmsPhoneNumber.Text
                    If mobileGateway.Send(ringtone__3) Then
                        MessageBox.Show("Ringtone message is sent successfully to " + ringtone__3.DestinationAddress & ". Message index is  " & ringtone__3.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

                ' Send custom Smart SMS
                If Not String.IsNullOrEmpty(txtSmartSmsMessage.Text) Then
                    ' Construt a Smart SMS
                    Dim smartSms As Sms = Sms.NewInstance()
                    smartSms.Content = txtSmartSmsMessage.Text
                    smartSms.DataCodingScheme = MessageDataCodingScheme.EightBits
                    smartSms.DestinationAddress = txtSmartSmsPhoneNumber.Text
                    smartSms.SourcePort = Convert.ToInt32(nupdSmartSmsSourcePort.Value)
                    smartSms.DestinationPort = Convert.ToInt32(nupdSmartSmsDestinationPort.Value)

                    If mobileGateway.Send(smartSms) Then
                        MessageBox.Show("Smart SMS message is sent successfully to " + smartSms.DestinationAddress & ". Message index is  " & smartSms.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Else
                MessageBox.Show("Phone number cannot be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnSendSmartSms.Enabled = True
        End Try

    End Sub

    Private Sub btnSendUsdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendUsdd.Click
        Try
            If Not String.IsNullOrEmpty(txtUssdCommand.Text) Then
                btnSendUsdd.Enabled = False

                Dim response As String = String.Empty
                If String.IsNullOrEmpty(txtDcs.Text) Then
                    response = mobileGateway.SendUssd(txtUssdCommand.Text)
                Else
                    Dim dcs As Integer = 0
                    Try
                        dcs = Convert.ToInt32(txtDcs.Text)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
                        Return
                    End Try
                    ' Here you can control the DCS
                    Dim ussdRequest As New UssdRequest(txtUssdCommand.Text)
                    ussdRequest.Dcs = UssdDcs.GetByNumeric(dcs)
                    Dim ussdResponse As UssdResponse = mobileGateway.SendUssd(ussdRequest)
                    response = ussdResponse.Content
                End If
                If String.IsNullOrEmpty(response) AndAlso Not mobileGateway.EnableUssdEvent Then
                    ' Error
                    txtUssdResponse.AppendText(mobileGateway.LastError.Message)
                    txtUssdResponse.AppendText(vbLf)
                Else
                    txtUssdResponse.AppendText(response)
                    txtUssdResponse.AppendText(vbLf)
                End If
            Else
                MessageBox.Show("USSD command cannot be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnSendUsdd.Enabled = True
        End Try
    End Sub

    Private Sub chkIncomingCallIndication_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkIncomingCallIndication.CheckedChanged
        If chkIncomingCallIndication.Checked Then
            mobileGateway.EnableCallNotifications()
        Else
            mobileGateway.DisableCallNotifications()
        End If
    End Sub

    Private Sub chkdEnableClir_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkdEnableClir.CheckedChanged
        If chkdEnableClir.Checked Then
            mobileGateway.EnableCLIR()
        Else
            mobileGateway.DisableCLIR()
        End If
    End Sub

    Private Sub btnMakeCall_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMakeCall.Click
        Try
            If Not String.IsNullOrEmpty(txtCallingNo.Text) Then
                btnMakeCall.Enabled = False
                mobileGateway.Dial(txtCallingNo.Text)
            Else
                MessageBox.Show("Calling number cannot be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnMakeCall.Enabled = True
        End Try
    End Sub

    Private Sub btnHangUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHangUp.Click
        Try
            btnHangUp.Enabled = False
            mobileGateway.HangUp()
        Finally
            btnHangUp.Enabled = True
        End Try
    End Sub

    Private Sub btnAnswerCall_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAnswerCall.Click
        Try
            btnAnswerCall.Enabled = False
            mobileGateway.Answer()
        Finally
            btnAnswerCall.Enabled = True
        End Try
    End Sub

    Private Sub OnCallReceived(ByVal sender As Object, ByVal e As IncomingCallEventArgs)
        txtCallingNo.BeginInvoke(displayCallLog, e)
    End Sub

    Private Sub btnGetPduCode_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetPduCode.Click
        Try
            If Not String.IsNullOrEmpty(txtPduDestinationNumber.Text) AndAlso Not String.IsNullOrEmpty(txtUserData.Text) Then
                btnGetPduCode.Enabled = False
                Dim pdu As SmsSubmitPdu
                If chkPduStatusReport.Checked Then
                    pdu = PduFactory.NewSmsSubmitPdu(PduUtils.TpSrrReport Or PduUtils.TpVpfInteger)
                Else
                    pdu = PduFactory.NewSmsSubmitPdu()
                End If
                If Not String.IsNullOrEmpty(txtPduServiceCentreAddress.Text) Then
                    pdu.SmscAddress = txtPduServiceCentreAddress.Text
                    Dim smscNumberForLengthCheck As String = pdu.SmscAddress
                    If pdu.SmscAddress.StartsWith("+") Then
                        smscNumberForLengthCheck = pdu.SmscAddress.Substring(1)
                    End If
                    pdu.SmscInfoLength = 1 + (smscNumberForLengthCheck.Length \ 2) + (IIf((smscNumberForLengthCheck.Length Mod 2 = 1), 1, 0))
                    pdu.SmscAddressType = PduUtils.GetAddressTypeFor(txtPduServiceCentreAddress.Text)
                Else
                    pdu.SmscAddress = String.Empty
                End If

                pdu.Address = txtPduDestinationNumber.Text
                Dim userData As String = txtUserData.Text

                ' Set message encoding
                Dim encoding__1 As MessageDataCodingScheme
                If MessageEncoding.TryGetValue(cboPduEncoding.Text, encoding__1) Then
                    If encoding__1 = MessageDataCodingScheme.DefaultAlphabet Then
                        pdu.DataCodingScheme = PduUtils.DcsEncoding7Bit
                    ElseIf encoding__1 = MessageDataCodingScheme.EightBits Then
                        pdu.DataCodingScheme = PduUtils.DcsEncoding8Bit
                    ElseIf encoding__1 = MessageDataCodingScheme.Ucs2 Then
                        pdu.DataCodingScheme = PduUtils.DcsEncodingUcs2
                    End If
                End If

                ' Set the message class
                Dim messageClass__2 As Integer
                If MessageClass.TryGetValue(cboMessageClass.Text, messageClass__2) Then
                    If messageClass__2 <> 0 Then
                        pdu.DataCodingScheme = pdu.DataCodingScheme Or messageClass__2
                    End If
                End If

                If encoding__1 = MessageDataCodingScheme.EightBits Then
                    If GetDataCodingScheme(userData) = MessageDataCodingScheme.Ucs2 Then
                        pdu.SetDataBytes(Encoding.GetEncoding("UTF-16").GetBytes(userData))
                    Else
                        pdu.SetDataBytes(Encoding.ASCII.GetBytes(userData))
                    End If
                Else
                    pdu.DecodedText = userData
                End If
                pdu.ValidityPeriod = Convert.ToInt32(nupdPduValidityPeriod.Value)
                pdu.ProtocolIdentifier = 0
                pdu.MessageReference = Convert.ToInt32(nupdPduMessageReferenceNo.Value)
                If nupdPduDestinationPort.Value > 0 OrElse nupdPduSourcePort.Value > 0 Then
                    pdu.AddInformationElement(InformationElementFactory.GeneratePortInfo(Convert.ToInt32(nupdPduDestinationPort.Value), Convert.ToInt32(nupdPduSourcePort.Value)))
                End If
                If chkPduFlashMessage.Checked Then
                    pdu.DataCodingScheme = pdu.DataCodingScheme Or PduUtils.DcsMessageClassFlash
                End If

                If Not String.IsNullOrEmpty(txtReplyPath.Text) Then
                    If txtReplyPath.Text.StartsWith("+") Then
                        pdu.AddReplyPath(txtReplyPath.Text.Substring(1), PduUtils.AddressTypeInternationFormat)
                    Else
                        pdu.AddReplyPath(txtReplyPath.Text, PduUtils.AddressTypeDomesticFormat)
                    End If
                End If
                Dim maxMessage As Integer = pdu.MpMaxNo
                Dim pduGenerator As New PduGenerator()
                Dim pduList As List(Of String) = pduGenerator.GeneratePduList(pdu, Convert.ToInt32(nupdPduMessageReferenceNo.Value))
                txtPduCode.Text = String.Empty
                For Each pduString As String In pduList
                    txtPduCode.AppendText("PDU" & vbCr & vbLf)
                    txtPduCode.AppendText("---------" & vbCr & vbLf)
                    txtPduCode.AppendText(pduString)
                    txtPduCode.AppendText(vbCr & vbLf)
                    txtPduCode.AppendText("Length: " & GetAtLength(pduString))
                    txtPduCode.AppendText(vbCr & vbLf & vbCr & vbLf)
                Next
            Else
                MessageBox.Show("Destination number and user data cannot be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnGetPduCode.Enabled = True
        End Try
    End Sub

    ''' <summary>
    ''' Calculate message length
    ''' </summary>
    ''' <param name="pduString">PDU string</param>
    ''' <returns>Message length</returns>
    Protected Function GetAtLength(ByVal pduString As String) As Integer
        ' Get AT command length
        Return (pduString.Length - Convert.ToInt32(pduString.Substring(0, 2), 16) * 2 - 2) \ 2
    End Function

    Private Sub btnSendvCard_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendvCard.Click
        Try
            If Not String.IsNullOrEmpty(txtvCardDestinationNumber.Text) Then
                btnSendvCard.Enabled = False

                If Not String.IsNullOrEmpty(txtvCardPhoneNumber.Text) OrElse Not String.IsNullOrEmpty(txtvCardFormattedName.Text) OrElse Not String.IsNullOrEmpty(txtvCardSurname.Text) Then
                    Dim vCard As vCard = vCard.NewInstance()


                    vCard.FormattedName = txtvCardFormattedName.Text
                    vCard.Surname = txtvCardSurname.Text
                    vCard.GivenName = txtvCardGivenName.Text
                    vCard.MiddleName = txtvCardMiddleName.Text
                    vCard.Prefix = txtvCardPrefix.Text
                    vCard.Suffix = txtvCardSuffix.Text
                    vCard.Title = txtvCardTitle.Text
                    vCard.Birthday = dtpvCardBirthday.Value
                    vCard.Org = txtvCardOrg.Text
                    vCard.Department = txtvCardDepartment.Text
                    vCard.Note = txtvCardNote.Text
                    vCard.Role = txtvCardRole.Text

                    If Not String.IsNullOrEmpty(txtvCardUrl.Text) Then
                        Dim url As New URL()
                        url.Address = txtvCardUrl.Text
                        vCard.URLs.Add(url)
                    End If

                    vCard.DestinationAddress = txtvCardDestinationNumber.Text

                    If Not String.IsNullOrEmpty(txtvCardStreet.Text) Then
                        Dim address As New Address()
                        address.Street = txtvCardStreet.Text
                        address.Postcode = txtvCardPostcode.Text
                        address.Region = txtvCardRegion.Text
                        address.Country = txtvCardCountry.Text
                        Dim homeWorkType As HomeWorkTypes
                        If vCardHomeWorkTypes.TryGetValue(cbovCardAddressType.Text, homeWorkType) Then
                            address.HomeWorkType = homeWorkType
                        End If
                        vCard.Addresses.Add(address)
                    End If

                    If Not String.IsNullOrEmpty(txtvCardEmail.Text) Then
                        Dim email As New EmailAddress()
                        email.Address = txtvCardEmail.Text
                        email.Pref = True
                        vCard.Emails.Add(email)
                    End If

                    If Not String.IsNullOrEmpty(txtvCardPhoneNumber.Text) Then
                        Dim phoneNumber As New PhoneNumber()
                        phoneNumber.Pref = chkvCardPhoneNumberPreferred.Checked
                        phoneNumber.Number = txtvCardPhoneNumber.Text
                        Dim homeWorkType As HomeWorkTypes
                        If vCardHomeWorkTypes.TryGetValue(cbovCardPhoneNumberHomeWorkType.Text, homeWorkType) Then
                            phoneNumber.HomeWorkType = homeWorkType
                        End If
                        Dim phoneType As PhoneTypes
                        If vCardPhoneTypes.TryGetValue(cbovCardPhoneNumberType.Text, phoneType) Then
                            phoneNumber.PhoneType = phoneType
                        End If

                        vCard.Phones.Add(phoneNumber)
                    End If
                    If mobileGateway.Send(vCard) Then
                        MessageBox.Show("vCard message is sent successfully to " + vCard.DestinationAddress & ". Message index is  " & vCard.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

                If Not String.IsNullOrEmpty(txtvCardFileLocation.Text) Then
                    Dim fileName As String = txtvCardFileLocation.Text
                    If File.Exists(fileName) Then
                        Dim fileContent As String = File.ReadAllText(fileName, Encoding.UTF8)
                        Dim vCard As vCard = vCard.NewInstance()
                        vCard.LoadString(fileContent)
                        If mobileGateway.Send(vCard) Then
                            MessageBox.Show("vCard message is sent successfully to " + vCard.DestinationAddress & ". Message index is  " & vCard.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If
            Else
                MessageBox.Show("Destination phone number and contact name cannot be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnSendvCard.Enabled = True
        End Try
    End Sub

    Private Sub btnBrowservCardFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowservCardFile.Click
        openFileDialog1SMS.Filter = "vCard File (*.vcf)|*.vcf"
        openFileDialog1SMS.FileName = String.Empty
        Dim dialogResult__1 As DialogResult = openFileDialog1SMS.ShowDialog()
        If dialogResult__1 = DialogResult.OK Then
            Dim fileName As String = openFileDialog1SMS.FileName

            ' Read the file content and send as vCard
            txtvCardFileLocation.Text = fileName
        End If
    End Sub

    Private Sub btnApplyLoggingLevel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApplyLoggingLevel.Click
        Dim logLevel As LogLevel
        If LoggingLevel.TryGetValue(cboLoggingLevel.Text, logLevel) Then
            Try
                btnApplyLoggingLevel.Enabled = False
                mobileGateway.LogLevel = logLevel
                MessageBox.Show("Log level is applied", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Finally
                btnApplyLoggingLevel.Enabled = True
            End Try
        End If
    End Sub

    Private Sub btnApplyCharacterSet_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApplyCharacterSet.Click
        If Not String.IsNullOrEmpty(cboCharacterSets.Text) Then
            Try
                btnApplyCharacterSet.Enabled = False
                If mobileGateway.SetCharacterSet(cboCharacterSets.Text) Then
                    MessageBox.Show("Character set is applied", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Unable to set character set", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Finally
                btnApplyCharacterSet.Enabled = True
            End Try
        End If
    End Sub

    Private Sub btnRefreshGatewayStatus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefreshGatewayStatus.Click
        Try
            btnRefreshGatewayStatus.Enabled = False

            Dim batteryCharge As BatteryCharge = mobileGateway.BatteryCharge
            progressBarBatteryLevel.Value = batteryCharge.BatteryChargeLevel

            Dim signalQuality As SignalQuality = mobileGateway.SignalQuality
            progressBarSignalQuality.Value = signalQuality.SignalStrengthPercent

            'mobileGateway.SendCommand("AT+COPS=0,0")

            Dim NetworkOperator As NetworkOperator = mobileGateway.NetworkOperator

            lbloperator.Text = NetworkOperator.OperatorInfo.ToString()
        Finally
            btnRefreshGatewayStatus.Enabled = True
        End Try
    End Sub


    Private Sub OnGatewayConnect(ByVal sender As Object, ByVal e As ConnectionEventArgs)
        ' Called when gateway is connected
        'Console.WriteLine(e.GatewayId);


    End Sub

    Private Sub OnGatewayDisconnect(ByVal sender As Object, ByVal e As ConnectionEventArgs)
        ' Called when gateway is disconnected
        'Console.WriteLine(e.GatewayId);
    End Sub

    Private Sub chkEnableUssdEvent_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnableUssdEvent.CheckedChanged
        mobileGateway.EnableUssdEvent = Me.chkEnableUssdEvent.Checked
        If chkEnableUssdEvent.Checked Then
            AddHandler mobileGateway.UssdResponseReceived, AddressOf OnUssdResponseReceived
        Else
            RemoveHandler mobileGateway.UssdResponseReceived, AddressOf OnUssdResponseReceived
        End If
    End Sub

    Private Sub OnUssdResponseReceived(ByVal sender As Object, ByVal e As UssdReceivedEventArgs)
        txtUssdResponse.BeginInvoke(displayUssdResponse, e)
    End Sub

    Private Sub ShowUssdResponse(ByVal e As UssdReceivedEventArgs)
        Dim ussdResponse As UssdResponse = e.UssdResponse
        txtUssdResponse.AppendText(ussdResponse.Content)
        txtUssdResponse.AppendText(vbLf)
    End Sub


    ''' <summary>
    ''' Determine the message data coding scheme
    ''' </summary>
    ''' <param name="content">Message content</param>
    ''' <returns>Message data coding scheme. See <see cref="MessageDataCodingScheme"/></returns>
    Private Shared Function GetDataCodingScheme(ByVal content As String) As MessageDataCodingScheme
        Dim i As Integer = 0
        For i = 1 To content.Length
            Dim code As Integer = Convert.ToInt32(Convert.ToChar(content.Substring(i - 1, 1)))
            If code < 0 OrElse code > 255 Then
                Return MessageDataCodingScheme.Ucs2
            End If
        Next
        Return MessageDataCodingScheme.DefaultAlphabet
    End Function

    ''' <summary>
    ''' Handles the Click event of the btnViewLogFile control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub btnViewLogFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewLogFile.Click
        If Not String.IsNullOrEmpty(txtLogFile.Text) AndAlso File.Exists(txtLogFile.Text) Then
            System.Diagnostics.Process.Start(txtLogFile.Text)
        End If
    End Sub

    ''' <summary>
    ''' Handles the Click event of the btnClearLogFile control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub btnClearLogFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClearLogFile.Click
        If Not String.IsNullOrEmpty(txtLogFile.Text) AndAlso File.Exists(txtLogFile.Text) Then
            mobileGateway.ClearLog()
        End If
    End Sub

    ''' <summary>
    ''' Handles the Click event of the btnSendPictureSms control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub btnSendPictureSms_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendPictureSms.Click
        Try
            If Not String.IsNullOrEmpty(txtPictureSmsPhoneNumber.Text) Then
                btnSendPictureSms.Enabled = False

                If picPictureSms.Image IsNot Nothing Then
                    ' Send Picture
                    Dim pictureSms__1 As PictureSms = PictureSms.NewInstance(New Bitmap(picPictureSms.Image), txtPictureSmsMessage.Text)
                    pictureSms__1.DestinationAddress = txtPictureSmsPhoneNumber.Text
                    If mobileGateway.Send(pictureSms__1) Then
                        MessageBox.Show("Picture SMS is sent successfully to " + pictureSms__1.DestinationAddress & ". Message index is  " & pictureSms__1.ReferenceNo.Item(0), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Else
                MessageBox.Show("Phone number cannot be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Finally
            btnSendPictureSms.Enabled = True
        End Try
    End Sub

    ''' <summary>
    ''' Handles the Click event of the btnBrowsePictureSms control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub btnBrowsePictureSms_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowsePictureSms.Click
        openFileDialog1SMS.Filter = "Image file (*.*)|*.*"
        openFileDialog1SMS.FileName = String.Empty
        Dim dialogResult__1 As DialogResult = openFileDialog1SMS.ShowDialog()
        If dialogResult__1 = DialogResult.OK Then
            Dim fileName As String = openFileDialog1SMS.FileName

            ' Read the file content and send as vCalendar
            txtPictureSms.Text = fileName
            PreviewPictureSms()
        End If
    End Sub

    ''' <summary>
    ''' Handles the CheckedChanged event of the chkPreviewPictureSms control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub chkPreviewPictureSms_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkPreviewPictureSms.CheckedChanged
        PreviewPictureSms()
    End Sub

    ''' <summary>
    ''' Previews the picture SMS.
    ''' </summary>
    Private Sub PreviewPictureSms()
        If chkPreviewPictureSms.Checked Then
            If Not String.IsNullOrEmpty(txtPictureSms.Text) Then
                If File.Exists(txtPictureSms.Text) Then
                    Dim bitmap As New Bitmap(txtPictureSms.Text)
                    If Not GatewayHelper.IsBlackAndWhite(bitmap) Then
                        bitmap = GatewayHelper.ConvertBlackAndWhite(bitmap)
                    End If

                    If bitmap.Height > &HFF OrElse bitmap.Width > &HFF Then
                        bitmap = GatewayHelper.ResizeImage(bitmap, 255, 255)
                    End If

                    picPictureSms.Image = bitmap
                End If
            End If
        End If
    End Sub


    ''' <summary>
    ''' Handles the Click event of the btnCancelUssdSession control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub btnCancelUssdSession_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelUssdSession.Click
        Try
            btnCancelUssdSession.Enabled = False
            ' Cannot cancel the session, check mobileGateway.LastError

            If Not mobileGateway.CancelUssdSession() Then
            End If
        Finally
            btnCancelUssdSession.Enabled = True
        End Try
    End Sub

    Private Sub chkPersistenceQueue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPersistenceQueue.CheckedChanged
        mobileGateway.PersistenceQueue = chkPersistenceQueue.Checked
    End Sub

    Private Sub tabMainSMS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabMainSMS.SelectedIndexChanged
        Dim tc As TabControl = DirectCast(sender, TabControl)
        If tc.SelectedTab Is tabTerminal Then
            txtTerminal.Focus()
            ScrollToEnd()
        ElseIf tc.SelectedTab Is tabPhonebook Then
            If cboPhoneBookStorage.Items.Count = 0 Then
                ' Add the phone book storages
                Dim phoneBookStorages As String() = mobileGateway.PhoneBookStorages
                'cboPhoneBookStorage.Items.AddRange(phoneBookStorages.OrderBy(Function(storage) storage).ToArray())
                cboPhoneBookStorage.Items.AddRange(phoneBookStorages)
            End If        
        End If
    End Sub
    Private Function GetMmsNotification(ByVal m As MessageInformation) As Boolean
        Return m.[GetType]() Is GetType(MmsNotification)
    End Function
    Private Function GetMmsReadReport(ByVal m As MessageInformation) As Boolean
        Return m.[GetType]() Is GetType(MmsReadReport)
    End Function
    Private Function GetMmsDeliveryNotification(ByVal m As MessageInformation) As Boolean
        Return m.[GetType]() Is GetType(MmsDeliveryNotification)
    End Function
    '

    Private Sub btnRetrieveMessage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRetrieveMessage.Click
        Try
            btnRetrieveMessage.Enabled = False
            Dim messageType__1 As MessageStatusType
            If MessageType.TryGetValue(cboMessageType.Text, messageType__1) Then
                Dim messages As List(Of MessageInformation) = mobileGateway.GetMessages(messageType__1)
                dgdMessages.DataSource = messages
                lblMessageCount.Text = messages.Count() & " message(s) found"
            End If
        Finally
            btnRetrieveMessage.Enabled = True
        End Try

    End Sub

    Private Sub btnRetrieveMMSNotification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetrieveMMSNotification.Click
        Try
            btnRetrieveMMSNotification.Enabled = False
            Dim messageType__1 As MessageStatusType
            If MessageType.TryGetValue(cboMessageType.Text, messageType__1) Then
                Dim Filter As New System.Predicate(Of MessageInformation)(AddressOf GetMmsNotification)
                Dim messages As List(Of MessageInformation) = mobileGateway.GetMessages(messageType__1).FindAll(Filter)

                Dim notifications As New List(Of MmsNotification)()
                For Each msg As MessageInformation In messages
                    notifications.Add(TryCast(msg, MmsNotification))
                Next
                dgdMessages.DataSource = notifications
                lblMessageCount.Text = messages.Count() & " message(s) found"
            End If
        Finally
            btnRetrieveMMSNotification.Enabled = True
        End Try

    End Sub

    Private Sub btnRetrieveMMSReadReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetrieveMMSReadReport.Click
        Try
            btnRetrieveMMSReadReport.Enabled = False
            Dim messageType__1 As MessageStatusType
            If MessageType.TryGetValue(cboMessageType.Text, messageType__1) Then
                Dim Filter As New System.Predicate(Of MessageInformation)(AddressOf GetMmsReadReport)
                Dim messages As List(Of MessageInformation) = mobileGateway.GetMessages(messageType__1).FindAll(Filter)
                Dim reports As New List(Of MmsReadReport)()
                For Each msg As MessageInformation In messages
                    reports.Add(TryCast(msg, MmsReadReport))
                Next
                dgdMessages.DataSource = reports
                lblMessageCount.Text = messages.Count() & " message(s) found"
            End If
        Finally
            btnRetrieveMMSReadReport.Enabled = True
        End Try

    End Sub

    Private Sub btnRetrieveMMSDeliveryReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetrieveMMSDeliveryReport.Click
        Try
            btnRetrieveMMSDeliveryReport.Enabled = False
            Dim messageType__1 As MessageStatusType
            If MessageType.TryGetValue(cboMessageType.Text, messageType__1) Then
                '
                Dim Filter As New System.Predicate(Of MessageInformation)(AddressOf GetMmsDeliveryNotification)
                Dim messages As List(Of MessageInformation) = mobileGateway.GetMessages(messageType__1).FindAll(Filter)
                Dim reports As New List(Of MmsDeliveryNotification)()
                For Each msg As MessageInformation In messages
                    reports.Add(TryCast(msg, MmsDeliveryNotification))
                Next
                dgdMessages.DataSource = reports
                lblMessageCount.Text = messages.Count() & " message(s) found"
            End If
        Finally
            btnRetrieveMMSDeliveryReport.Enabled = True
        End Try

    End Sub
    '###################################################

    Private Sub TabMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabMain.SelectedIndexChanged
        Dim tc As TabControl = DirectCast(sender, TabControl)
        If tc.SelectedTab Is tabAbout Then
            'Dim assembly__1 As Assembly = Assembly.GetAssembly(mobileGateway.[GetType]())
            Dim mobileGateway_assembly = DirectCast(mobileGateway, Object)
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
            If mobileGateway.License.Valid Then
                lblLicense.Text = "Licensed Copy"
            Else
                lblLicense.Text = "Community Copy"
            End If
        End If
    End Sub

    Private Sub radPhoneSMS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radPhoneSMS.CheckedChanged
        If radPhoneSMS.Checked Then
            mobileGateway.MessageStorage = MessageStorage.Phone
        End If
    End Sub

    Private Sub radSimSMS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radSimSMS.CheckedChanged
        If radSimSMS.Checked Then
            mobileGateway.MessageStorage = MessageStorage.Sim
        End If
    End Sub

    Private Sub radBothSMS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radBothSMS.CheckedChanged
        If radBothSMS.Checked Then
            mobileGateway.MessageStorage = MessageStorage.MobileTerminating
        End If
    End Sub

    Private Sub rdoSMSOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoSMSOnly.CheckedChanged
        If rdoSMSOnly.Checked Then
            gbCountry.Enabled = False
            gbMMSC.Enabled = False
        End If
    End Sub

    Private Sub rdoSMSMMS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoSMSMMS.CheckedChanged
        If rdoSMSMMS.Checked Then
            gbCountry.Enabled = True
            gbMMSC.Enabled = True
        End If
    End Sub

 
End Class