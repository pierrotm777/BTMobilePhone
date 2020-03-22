Imports ATSMS

Public Class MobilePhone
    Inherits GSMModem

    Public bCommunicating As Boolean = False

    Public Sub New()
        MyBase.New()
        Dim sPath = "./MobilePhone.ini"
        sInternationalPrefix1 = INIFile.Read(sPath, "PHONEBOOK", "InternationalPrefix", "0032")
        sInternationalShortCutPrefix2 = INIFile.Read(sPath, "PHONEBOOK", "ShortCode", "+32")
        sNationalPrefix = INIFile.Read(sPath, "PHONEBOOK", "NationalPrefix", "0")
    End Sub

    Public Class Entry
        Private nIndex As Integer = -1
        Private sName As String = ""
        Private sNumber As String = ""
        Private nType As Integer = -1
        Private sContactFlag As String = ""

        Public Property Index() As Integer
            Get
                Return nIndex
            End Get
            Set(ByVal value As Integer)
                nIndex = value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return sName
            End Get
            Set(ByVal value As String)
                sName = value
            End Set
        End Property

        Public Property Number() As String
            Get
                Return sNumber
            End Get
            Set(ByVal value As String)
                sNumber = value
            End Set
        End Property
        Public Property Type() As Integer
            Get
                Return nType
            End Get
            Set(ByVal value As Integer)
                nType = value
            End Set
        End Property
        Public Property ContactFlag() As String
            Get
                Return sContactFlag
            End Get
            Set(ByVal value As String)
                sContactFlag = value
            End Set
        End Property
    End Class

#Region "METHODS"
    Public Function GetPhoneBookEntries(ByVal type As String) As Entry()
        'AT+CPBS="MC"	AT+CPBS="MC"<CR><CR><LF>OK<CR><LF>
        bCommunicating = True
        Dim entries As Entry() = Nothing
        Try
            Dim response() As String = ParseATResponse(serialDriver.SendCmd("AT+CPBS=""" & type & """"))
            Dim results As String = response(0)
            If (results = ATHandler.RESPONSE_OK) Then
                Dim info() As Integer = internal_GetPhoneBookInfo()
                entries = internal_GetPhoneBookEntries(info(0), info(1))
            End If
        Catch ex As System.Exception
        End Try
        bCommunicating = False
        Return entries
    End Function
    Public Function GetPhoneBookInfo() As Integer()
        bCommunicating = True
        Dim result As Integer() = internal_GetPhoneBookInfo()
        bCommunicating = False
        Return result
    End Function
    Private Function internal_GetPhoneBookInfo() As Integer()
        'AT+CPBR=?	    AT+CPBR=?<CR><CR><LF>+CPBR: (1-30),80,180<CR><LF><CR><LF>OK<CR><LF>

        Try
            Dim response() As String = ParseATResponse(serialDriver.SendCmd("AT+CPBR=?"))
            Dim results As String = response(1)
            If (results = ATHandler.RESPONSE_OK) Then
                Dim rangeValues As String = response(0)
                Dim result(1) As Integer
                Dim indexOfDash As Integer = rangeValues.IndexOf("-")
                Dim indexOfP1 As Integer = rangeValues.IndexOf("(")
                Dim indexOfP2 As Integer = rangeValues.IndexOf(")")

                result(0) = Integer.Parse(rangeValues.Substring(indexOfP1 + 1, indexOfDash - indexOfP1 - 1))
                result(1) = Integer.Parse(rangeValues.Substring(indexOfDash + 1, indexOfP2 - indexOfDash - 1))
                Return result
            End If
            Return Nothing
        Catch ex As System.Exception
            Throw New InvalidOpException("Unable to fetch phonebook info", ex)
        End Try

    End Function
    Private Function internal_GetPhoneBookEntries(ByVal minIndex As Integer, ByVal maxIndex As Integer) As Entry()
        'AT+CPBR=1	    AT+CPBR=1<CR><CR><LF>+CPBR: 1,"+32number",145,"Name of person/W"<CR><LF><CR><LF>OK<CR><LF>

        Try
            Dim response() As String = ParseATResponse(serialDriver.SendCmd("AT+CPBR=" & minIndex & "," & maxIndex))
            Dim results As String = response(0)
            If (results = ATHandler.RESPONSE_ERROR) Then
                Return Nothing
            Else
                If response.Length > 0 Then
                    Dim entries As New ArrayList
                    Dim i As Integer
                    For i = 0 To response.Length - 2
                        Dim info As String() = response(i).Substring(7).Trim().Split(",")
                        Dim entry As New Entry
                        entry.Index = Integer.Parse(info(0))
                        entry.Number = CleanPhoneNumber(info(1))
                        entry.Type = Integer.Parse(info(2))
                        Dim name As String = info(3)
                        Dim chArr() As Char = {" ", """"}
                        name = name.Trim(chArr)

                        If Len(name) > 0 Then
                            Dim flagIndex As Integer = name.IndexOf("/")
                            If flagIndex > 0 Then
                                entry.Name = name.Substring(0, flagIndex)
                                entry.ContactFlag = name.Substring(flagIndex, 2)
                            Else
                                entry.Name = name
                                entry.ContactFlag = "/H" 'Default Home
                            End If
                        Else
                            entry.Name = entry.Number
                        End If

                        entries.Add(entry)
                    Next
                    Return CType(entries.ToArray(GetType(Entry)), Entry())
                Else
                    Return Nothing
                End If
            End If
        Catch ex As System.Exception
            Throw New InvalidOpException("Unable to fetch phonebook entries", ex)
        End Try
    End Function

    Public Shadows Sub Connect()
        If Not IsConnected And Not bCommunicating Then
            bCommunicating = True
            Dim workerThread As Threading.Thread
            workerThread = New Threading.Thread(AddressOf connectProcess)
            workerThread.Priority = Threading.ThreadPriority.Lowest
            workerThread.Start()
        End If
    End Sub
    Private Sub connectProcess()
        Try
            internal_connect()
        Catch ex As Exception
            bCommunicating = False
        End Try
    End Sub
    Private Shadows Sub internal_connect()
        bCommunicating = True
        Dim sPath As String
        sPath = "./MobilePhone.ini"
        MyBase.Port = INIFile.Read(sPath, "CONNECTION", "Port", "COM7")
        MyBase.BaudRate = Convert.ToInt32(INIFile.Read(sPath, "CONNECTION", "BaudRate", "115200"))
        MyBase.DataBits = Convert.ToInt32(INIFile.Read(sPath, "CONNECTION", "DataBit", "8"))
        Dim sCommParity = Convert.ToInt32(INIFile.Read(sPath, "CONNECTION", "Parity", "None"))
        Dim sCommStopBit = INIFile.Read(sPath, "CONNECTION", "StopBit", "1")
        Dim sCommFlowControl = INIFile.Read(sPath, "CONNECTION", "FlowControl", "None")
        If sCommParity <> String.Empty Then
            Select Case sCommParity
                Case "Even"
                    MyBase.Parity = Common.EnumParity.Even
                Case "Mark"
                    MyBase.Parity = Common.EnumParity.Mark
                Case "None"
                    MyBase.Parity = Common.EnumParity.None
                Case "Odd"
                    MyBase.Parity = Common.EnumParity.Odd
                Case "Space"
                    MyBase.Parity = Common.EnumParity.Space
            End Select
        End If

        If sCommStopBit <> String.Empty Then
            Select Case sCommStopBit
                Case "1"
                    MyBase.StopBits = Common.EnumStopBits.One
                Case "1.5"
                    MyBase.StopBits = Common.EnumStopBits.OnePointFive
                Case "2"
                    MyBase.StopBits = Common.EnumStopBits.Two
            End Select
        End If

        If sCommFlowControl <> String.Empty Then
            Select Case sCommFlowControl
                Case "None"
                    MyBase.FlowControl = Common.EnumFlowControl.None
                Case "Hardware"
                    MyBase.FlowControl = Common.EnumFlowControl.RTS_CTS
                Case "Xon/Xoff"
                    MyBase.FlowControl = Common.EnumFlowControl.Xon_Xoff
            End Select
        End If

        Try
            MyBase.Connect()
            MyBase.IncomingCallIndication = True
            MyBase.NewMessageIndication = True
        Catch ex As Exception
        End Try
        bCommunicating = False
    End Sub
    Public Shadows Sub disconnect()
        bCommunicating = True
        MyBase.Disconnect()
        bCommunicating = False
    End Sub

#End Region

#Region "PROPERTIES"
    Private sSignalStrength
    Private sBatteryStrength
    Private sBatteryCharging
    Private sNetwork
    Private sIMEI
    Private sIMSI
    Private sOwnNumber
    Private sPhoneModel
    Private sManufacturer
    Private sRevision
    Private sSMSC
    Public Shadows ReadOnly Property SignalStrength() As String
        Get
            If IsConnected And Not bCommunicating Then
                bCommunicating = True
                Try
                    Dim rssi As Rssi = MyBase.GetRssi
                    Dim percent As Double = (rssi.Current - rssi.Minimum) / (rssi.Maximum - rssi.Minimum) * 100
                    sSignalStrength = Math.Round(percent)
                Catch ex As Exception
                    sSignalStrength = "N/A"
                End Try
                bCommunicating = False
            End If
            Return sSignalStrength
        End Get
    End Property
    Public ReadOnly Property BatteryCharging() As Boolean
        Get
            If IsConnected And Not bCommunicating Then
                bCommunicating = True
                Try

                    Dim battery As Battery = MyBase.GetBatteryLevel
                    sBatteryCharging = battery.BatteryCharged
                Catch ex As Exception
                    sBatteryCharging = False
                End Try
                bCommunicating = False
            End If
            Return sBatteryCharging
        End Get
    End Property
    Public Shadows ReadOnly Property BatteryStrength() As String
        Get
            If IsConnected And Not bCommunicating Then
                bCommunicating = True
                Try

                    Dim battery As Battery = MyBase.GetBatteryLevel
                    Dim percent As Double = (battery.BatteryLevel - battery.MinimumLevel) / (battery.MaximumLevel - battery.MinimumLevel) * 100
                    sBatteryStrength = Math.Round(percent)
                    sBatteryCharging = battery.BatteryCharged
                Catch ex As Exception
                    sBatteryStrength = "N/A"
                    sBatteryCharging = False
                End Try
                bCommunicating = False
            End If
            Return sBatteryStrength
        End Get
    End Property
    Public Shadows ReadOnly Property Network() As String
        Get
            If Len(sNetwork) = 0 And Not bCommunicating Then
                bCommunicating = True
                Try
                    'AT+COPS?<CR><CR><LF>+COPS: 0,0,"BEL PROXIMUS"<CR><LF><CR><LF>OK<CR><LF>
                    Dim result As String = serialDriver.SendCmd("AT+COPS?")
                    Dim response() As String = ParseATResponse(result)
                    Dim results As String = response(response.Length - 1)
                    If (results = ATHandler.RESPONSE_OK) Then
                        If response.Length > 1 Then
                            Dim info As String() = response(0).Substring(7).Trim().Split("""")
                            sNetwork = info(1)
                        Else
                            'Empty entry
                        End If
                    End If
                Catch ex As System.Exception
                End Try
                bCommunicating = False
            End If
            Return sNetwork
        End Get
    End Property
    Public Shadows ReadOnly Property IMEI() As String
        Get
            If Len(sIMEI) = 0 And Not bCommunicating Then
                bCommunicating = True
                Try
                    sIMEI = MyBase.IMEI
                Catch ex As InvalidOpException
                End Try
                bCommunicating = False
            End If
            Return sIMEI
        End Get
    End Property
    Public Shadows ReadOnly Property IMSI() As String
        Get
            If Len(sIMSI) = 0 And Not bCommunicating Then
                bCommunicating = True
                Try
                    sIMSI = MyBase.IMSI
                Catch ex As InvalidOpException
                End Try
                bCommunicating = False
            End If
            Return sIMSI
        End Get
    End Property
    Public Shadows ReadOnly Property OwnNumber() As String
        Get
            If Len(sOwnNumber) = 0 And Not bCommunicating Then
                bCommunicating = True
                Try
                    sOwnNumber = MyBase.OwnNumber
                Catch ex As InvalidOpException
                End Try
                bCommunicating = False
            End If
            Return sOwnNumber
        End Get
    End Property
    Public Shadows ReadOnly Property MSISDN() As String
        Get
            Return OwnNumber
        End Get
    End Property
    Public Shadows ReadOnly Property PhoneModel() As String
        Get
            If Len(sPhoneModel) = 0 And Not bCommunicating Then
                bCommunicating = True
                Try
                    sPhoneModel = MyBase.PhoneModel
                Catch ex As InvalidOpException
                End Try
                bCommunicating = False
            End If
            Return sPhoneModel
        End Get
    End Property
    Public Shadows ReadOnly Property Manufacturer() As String
        Get
            If Len(sManufacturer) = 0 And Not bCommunicating Then
                bCommunicating = True
                Try
                    sManufacturer = MyBase.Manufacturer
                Catch ex As InvalidOpException
                End Try
                bCommunicating = False
            End If
            Return sManufacturer
        End Get
    End Property
    Public Shadows ReadOnly Property Revision() As String
        Get
            If Len(sRevision) = 0 And Not bCommunicating Then
                bCommunicating = True
                Try
                    sRevision = MyBase.Revision
                Catch ex As InvalidOpException
                End Try
                bCommunicating = False
            End If
            Return sRevision
        End Get
    End Property
    Public Shadows ReadOnly Property SMSC() As String
        Get
            If Len(sSMSC) = 0 And Not bCommunicating Then
                bCommunicating = True
                Try
                    sSMSC = MyBase.SMSC
                Catch ex As InvalidOpException
                End Try
                bCommunicating = False
            End If
            Return sSMSC
        End Get
    End Property
#End Region

#Region "HELPERS"
    Public Shared sInternationalPrefix1 = "0032"
    Public Shared sInternationalShortCutPrefix2 = "+32"
    Public Shared sNationalPrefix = "0"
    Public Shared Function CleanPhoneNumber(ByVal n As String) As String
        ' remove whitespace and quotes
        Dim chArr() As Char = {" ", """"}
        n = n.Trim(chArr)
        'remove trailing prefixes (0 for Belgium) and add the short cut
        If n.StartsWith(sNationalPrefix) And Not n.StartsWith(sInternationalPrefix1) Then
            n = sInternationalShortCutPrefix2 & n.Substring(1)
        End If
        'replace the 00 country prefix with the international short cut standard using +
        If n.StartsWith(sInternationalPrefix1) Then
            n = sInternationalShortCutPrefix2 & n.Substring(Len(sInternationalPrefix1))
        End If
        'remove spaces, dashes and slashes
        CleanPhoneNumber = n.Replace(" ", "").Replace("-", "").Replace("/", "")
    End Function
#End Region



End Class
