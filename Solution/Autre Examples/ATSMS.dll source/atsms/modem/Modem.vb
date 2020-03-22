Imports System.IO.Ports
Imports System.Text
Imports System.Threading
Imports System.Management

Imports ATSMS.Common


Public Class Modem : Inherits AbstractObject : Implements IDisposable

#Region "Protected Members"

    ' Variables
    Protected iBatteryLevel As Integer
    Protected iDelayAfterPIN As Integer
    Protected enumEncoding As EnumEncoding
    Protected lErrorCode As Long
    Protected strErrorDescription As String
    Protected enumFlowControl As EnumFlowControl
    Protected strModemInitString As String
    Protected strModel As String
    Protected strManufacturer As String
    Protected strRevision As String
    Protected lSendDelay As Long
    Protected iSendRetry As Integer
    Protected iSignalStrength As Integer
    Protected iSignalStrengthDB As Integer
    Protected isEcho As Boolean
    Protected strName As String
    Protected disposed As Boolean = False
    Protected strSimPin As String

    Protected commandHandler As ATHandler
    Protected WithEvents serialDriver As SerialDriver

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        serialDriver = New SerialDriver
        commandHandler = New ATHandler
        enumEncoding = Common.EnumEncoding.GSM_Default_7Bit
    End Sub

#End Region

#Region "Dispose"

    ''' <summary>
    ''' Dispose method
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposed Then
            If disposing Then
                If (Not IsNothing(serialDriver)) Then
                    If (serialDriver.IsOpen) Then
                        serialDriver.Close()
                        serialDriver = Nothing
                    End If
                End If
            End If

            ' Add code here to release the unmanaged resource.

            ' Note that this is not thread safe.
        End If
        Me.disposed = True
    End Sub

#End Region

#Region " IDisposable Support "

    ' Do not change or add Overridable to these methods.
    ' Put cleanup code in Dispose(ByVal disposing As Boolean).
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub

#End Region

#Region "Properties"

    Public Property ATCommandHandler() As ATHandler
        Get
            Return commandHandler
        End Get
        Set(ByVal value As ATHandler)
            commandHandler = value
        End Set
    End Property

    Public Property SerialDriverHandler() As SerialDriver
        Get
            Return serialDriver
        End Get
        Set(ByVal value As SerialDriver)
            serialDriver = value
        End Set
    End Property

    Public Property Echo() As Boolean
        Get
            Return Me.isEcho
        End Get
        Set(ByVal value As Boolean)
            Me.isEcho = value
            If IsConnected Then
                If isEcho Then
                    serialDriver.SendCmd(ATHandler.ECHO_COMMAND)
                Else
                    serialDriver.SendCmd(ATHandler.NO_ECHO_COMMAND)
                End If
            End If

        End Set
    End Property

    Public ReadOnly Property BatteryLevel() As Integer
        Get
            Return Me.iBatteryLevel
        End Get
    End Property


    Public Property BaudRate() As Integer
        Get
            Return serialDriver.BaudRate
        End Get
        Set(ByVal Value As Integer)
            serialDriver.BaudRate = Value
        End Set
    End Property

    Public Property DataBits() As Integer
        Get
            Return serialDriver.DataBits
        End Get
        Set(ByVal Value As Integer)
            serialDriver.DataBits = Value
        End Set
    End Property

    Public Property DelayAfterPIN() As Integer
        Get
            Return Me.iDelayAfterPIN
        End Get
        Set(ByVal Value As Integer)
            Me.iDelayAfterPIN = Value
        End Set
    End Property

    Public Property Encoding() As EnumEncoding
        Get
            Return Me.enumEncoding
        End Get
        Set(ByVal Value As EnumEncoding)
            Me.enumEncoding = Value
        End Set
    End Property

    Public Property ErrorCode() As Long
        Get
            Return Me.lErrorCode
        End Get
        Set(ByVal Value As Long)
            Me.lErrorCode = Value
        End Set
    End Property

    Public Property ErrorDescription() As String
        Get
            Return Me.strErrorDescription
        End Get
        Set(ByVal Value As String)
            Me.strErrorDescription = Value
        End Set
    End Property

    Public Property FlowControl() As EnumFlowControl
        Get
            Return Me.enumFlowControl
        End Get
        Set(ByVal Value As EnumFlowControl)
            Me.enumFlowControl = Value
        End Set
    End Property

    Public Property StopBits() As EnumStopBits
        Get
            Return serialDriver.StopBits
        End Get
        Set(ByVal Value As EnumStopBits)
            serialDriver.StopBits = Value
        End Set
    End Property

    Public Property Port() As String
        Get
            Return serialDriver.PortName
        End Get
        Set(ByVal Value As String)
            serialDriver.PortName = Value
        End Set
    End Property

    Public Property Parity() As EnumParity
        Get
            Return serialDriver.Parity
        End Get
        Set(ByVal Value As EnumParity)
            serialDriver.Parity = Value
        End Set
    End Property

    Public Property ModemInitString() As String
        Get
            Return Me.strModemInitString
        End Get
        Set(ByVal Value As String)
            Me.strModemInitString = Value
        End Set
    End Property

    Public Property Timeout() As Integer
        Get
            Return serialDriver.WriteTimeout
        End Get
        Set(ByVal Value As Integer)
            serialDriver.WriteTimeout = Value
        End Set
    End Property

    Public Property ReadIntervalTimeout() As Integer
        Get
            Return serialDriver.ReadTimeout
        End Get
        Set(ByVal Value As Integer)
            serialDriver.ReadTimeout = Value
        End Set
    End Property

    Public ReadOnly Property IsConnected() As Boolean
        Get
            Return serialDriver.IsOpen
        End Get
    End Property

    Public ReadOnly Property Manufacturer() As String
        Get
            If (Len(Me.strManufacturer) = 0) Then
                Me.strManufacturer = GetManufacturer()
            End If
            Return Me.strManufacturer
        End Get
    End Property

    Public ReadOnly Property Model() As String
        Get
            Return Me.strModel
        End Get
    End Property

    Public ReadOnly Property Revision() As String
        Get
            If (Len(Me.strManufacturer) = 0) Then
                Me.strRevision = GetRevision()
            End If
            Return Me.strRevision
        End Get
    End Property

    Public Property SendDelay() As Long
        Get
            Return Me.lSendDelay
        End Get
        Set(ByVal Value As Long)
            Me.lSendDelay = Value
        End Set
    End Property

    Public Property SendRetry() As Integer
        Get
            Return Me.iSendRetry
        End Get
        Set(ByVal Value As Integer)
            Me.iSendRetry = Value
        End Set
    End Property

    Public Property DtrEnable() As Boolean
        Get
            Return serialDriver.DtrEnable
        End Get
        Set(ByVal Value As Boolean)
            serialDriver.DtrEnable = Value
        End Set
    End Property

    Public Property Handsake() As EnumHandsake
        Get
            Return serialDriver.Handshake
        End Get
        Set(ByVal Value As EnumHandsake)
            serialDriver.Handshake = Value
        End Set
    End Property

    Public ReadOnly Property SignalStrength() As Integer
        Get
            Return Me.iSignalStrength
        End Get
    End Property

    Public ReadOnly Property SignalStrengthDB() As Integer
        Get
            Return Me.iSignalStrengthDB
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return Me.strName
        End Get
    End Property


    Public Property PIN() As String
        Get
            Return strSimPin
        End Get
        Set(ByVal value As String)
            strSimPin = value.Trim
        End Set
    End Property

#End Region

#Region "Private Functions"

    ''' <summary>
    ''' Get device manufacturer
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetManufacturer() As String
        Try
            Dim response() As String = ParseATResponse(serialDriver.SendCmd(ATHandler.MANUFACTURER_COMMAND))
            Return response(0)
        Catch ex As System.Exception
            Throw New InvalidCommandException(ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Get device revision
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRevision() As String
        Try
            Dim response As String = serialDriver.SendCmd(ATHandler.REVISION_COMMAND, ATHandler.RESPONSE_OK)
            Return response.Replace(ATHandler.RESPONSE_OK, "")
        Catch ex As System.Exception
            Throw New InvalidCommandException(ex.Message, ex)
        End Try
    End Function

#End Region

#Region "Protected Functions"

    ''' <summary>
    ''' Parse response from serial port into a 2 dimensional string array
    ''' </summary>
    ''' <param name="response"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function ParseSplitATResponse(ByVal response As String) As String(,)
        Dim rows() As String
        Dim cols() As String
        Dim results(1, 1) As String
        Dim i, j As Integer
        Dim rowCount, colCount As Integer
        rowCount = 0
        colCount = 0
        rows = response.Split(ControlChars.CrLf)
        For i = 0 To rows.Length - 1
            If (rows(i).Trim = String.Empty) Then Continue For
            cols = rows(i).Split(",")
            If (i = 0) Then
                ReDim results(rows.Length, cols.Length)
            End If
            colCount = 0
            For j = 0 To cols.Length - 1
                If (cols(j).Trim() <> String.Empty) Then
                    results(rowCount, colCount) = cols(j)
                    colCount = colCount + 1
                End If
            Next
            rowCount = rowCount + 1
        Next
        ParseSplitATResponse = results
    End Function

    ''' <summary>
    ''' Parse response from serial port into a string array
    ''' </summary>
    ''' <param name="response"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function ParseATResponse(ByVal response As String) As String()
        Dim rows() As String
        Dim results(1) As String
        Dim i As Integer
        Dim rowCount As Integer
        rowCount = 0
        rows = response.Split(ControlChars.CrLf)
        ReDim results(rows.Length)
        For i = 0 To rows.Length - 1
            If (rows(i).Trim = String.Empty) Then Continue For
            results(rowCount) = rows(i).Trim()
            rowCount = rowCount + 1
        Next

        ' TODO, extra checking here
        If rowCount = 0 Then
            ReDim Preserve results(0)
        ElseIf rowCount > 0 Then
            ReDim Preserve results(rowCount - 1)
        End If

        Return results
    End Function

#End Region

#Region "Public Functions"

    Public Function ClearLog() As Boolean
        ClearLog = True
    End Function

    Public Function ClearLog(ByVal keepLinesInLog As Integer) As Boolean
        ClearLog = True
    End Function

    ''' <summary>
    ''' Auto detect GSM modem
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AutoDetect() As Boolean
        Try
            Dim modems() As Modem = GetAvailableModems()
            Dim bluetooth, gprs, infrared, serial As Modem
            bluetooth = Nothing
            gprs = Nothing
            infrared = Nothing
            serial = Nothing
            For Each modem As Modem In modems
                If (modem.Name.ToLower.Contains("bluetooth modem")) Then
                    bluetooth = modem
                End If
                If (modem.Name.ToLower.Contains("gprs")) Then
                    gprs = modem
                End If
                If (modem.Name.ToLower.Contains("infrared")) Then
                    infrared = modem
                End If
                If (modem.Name.ToLower.Contains("serial")) Then
                    serial = modem
                End If
            Next

            If Not bluetooth Is Nothing Then
                Me.strName = bluetooth.Name
                Me.Port = bluetooth.Port
                Me.BaudRate = bluetooth.BaudRate
                Me.strModel = bluetooth.Model
                Return True
            End If

            If Not gprs Is Nothing Then
                Me.strName = gprs.Name
                Me.Port = gprs.Port
                Me.BaudRate = gprs.BaudRate
                Me.strModel = gprs.Model
                Return True
            End If

            If Not infrared Is Nothing Then
                Me.strName = infrared.Name
                Me.Port = infrared.Port
                Me.BaudRate = infrared.BaudRate
                Me.strModel = infrared.Model
                Return True
            End If

            If Not serial Is Nothing Then
                Me.strName = serial.Name
                Me.Port = serial.Port
                Me.BaudRate = serial.BaudRate
                Me.strModel = serial.Model
                Return True
            End If
            Return False
        Catch ex As System.Exception
            Throw New GeneralException(ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Auto detect bluetooth modem.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AutoDetectBluetooth() As Boolean
        Try
            Dim modems() As Modem = GetAvailableModems()

            For Each modem As Modem In modems
                If (modem.Name.ToLower.Contains("bluetooth modem")) Then
                    Me.strName = modem.Name
                    Me.Port = modem.Port
                    Me.BaudRate = modem.BaudRate
                    Me.strModel = modem.Model
                    AutoDetectBluetooth = True
                    Exit Function
                End If
            Next
            AutoDetectBluetooth = False
        Catch ex As System.Exception
            Throw New GeneralException(ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Auto detect infrared modem
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AutoDetectInfrared() As Boolean
        Try
            Dim modems() As Modem = GetAvailableModems()

            For Each modem As Modem In modems
                If (modem.Name.ToLower.Contains("infrared")) Then
                    Me.strName = modem.Name
                    Me.Port = modem.Port
                    Me.BaudRate = modem.BaudRate
                    Me.strModel = modem.Model
                    AutoDetectInfrared = True
                    Exit Function
                End If
            Next
            AutoDetectInfrared = False
        Catch ex As System.Exception
            Throw New GeneralException(ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Get available modems
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAvailableModems() As Modem()
        Try
            Dim query As New SelectQuery("CIM_PotsModem")

            ' ManagementObjectSearcher retrieves a collection of WMI objects based on 
            ' the query.
            Dim search As New ManagementObjectSearcher(query)

            ' Display each entry for Win32_processor
            Dim info As ManagementObject

            Dim modems() As Modem
            ReDim modems(search.Get().Count - 1)
            Dim counter As Integer

            counter = 0
            For Each info In search.Get()
                Dim propertyDataCollection As PropertyDataCollection = info.Properties()
                Dim modem As New Modem
                modem.strName = info("Name").ToString()
                modem.Port = info("AttachedTo").ToString()
                modem.BaudRate = CInt(info("MaxBaudRateToSerialPort"))
                modem.strModel = info("Model").ToString()
                modems(counter) = modem
                counter = counter + 1
            Next
            GetAvailableModems = modems
        Catch ex As System.Exception
            Throw New GeneralException(ex.Message, ex)
        End Try
    End Function


    ''' <summary>
    ''' Send heart beat to keep connection alive
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function KeepAlive() As Boolean
        Try
            If serialDriver.IsOpen Then
                serialDriver.SendCmd(ATHandler.KEEP_ALIVE_COMMAND)
            End If
            Return True
        Catch ex As System.Exception
            Return False
        End Try
    End Function

#End Region

#Region "Public Procedures"

    ''' <summary>
    ''' Connect to the serial port
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Connect()
        Try
            If (serialDriver.IsOpen) Then
                serialDriver.Close()
            End If
            serialDriver.Open()

            ' Check if there is a need to enter a PIN
            If Me.PIN <> String.Empty Then
                Dim responses() As String = ParseATResponse(serialDriver.SendCmd(ATHandler.CPIN_COMMAND & "?"))
                If Not responses Is Nothing And responses.Length > 0 Then
                    If Not responses(0) Is Nothing Then
                        If responses(0).IndexOf(ATHandler.RESPONSE_PIN_REQUIRED) >= 0 Then
                            ' PIN is needed
                            responses = ParseATResponse(serialDriver.SendCmd(ATHandler.CPIN_COMMAND & "=" & Me.PIN))
                            If Not responses Is Nothing And responses.Length > 0 Then
                                If Not responses(0) Is Nothing Then
                                    If Not responses(0).IndexOf(ATHandler.RESPONSE_OK) Then
                                        Throw New InvalidOpException("Unable to login using the PIN provided.")
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            Me.Echo = False
            If (Not String.IsNullOrEmpty(strModemInitString)) Then
                Dim response As String = serialDriver.SendCmd(strModemInitString)
                Dim results() As String = ParseATResponse(response)
            End If
        Catch ex As InvalidOperationException
            Throw New ConnectionException(ex.Message, ex)
        Catch ex As ArgumentOutOfRangeException
            Throw New ConnectionException(ex.Message, ex)
        Catch ex As ArgumentException
            Throw New ConnectionException(ex.Message, ex)
        Catch ex As IO.IOException
            Throw New ConnectionException(ex.Message, ex)
        Catch ex As UnauthorizedAccessException
            Throw New ConnectionException(ex.Message, ex)
        Catch ex As Exception
            Throw New ConnectionException(ex.Message, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Disconnect serial port
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Disconnect()
        If serialDriver.IsOpen Then
            serialDriver.Close()
            'serialPort = Nothing
        End If
    End Sub

#End Region

End Class


