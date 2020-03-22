'modInternet - Written by Jesse Yeager.
Option Explicit On
Option Strict On

Imports System.Runtime.InteropServices
Imports System.Net.NetworkInformation

Module ModInternet


    'reminder:  a GUID datatype is 16 bytes.

    <DllImport("Wlanapi", EntryPoint:="WlanOpenHandle")> _
    Private Function WlanOpenHandle(ByVal dwClientVersion As UInteger, ByVal pReserved As IntPtr, <Out()> ByRef pdwNegotiatedVersion As UInteger, ByRef phClientHandle As IntPtr) As UInteger
    End Function

    <DllImport("Wlanapi", EntryPoint:="WlanCloseHandle")> _
    Private Function WlanCloseHandle(<[In]()> ByVal hClientHandle As IntPtr, ByVal pReserved As IntPtr) As UInteger
    End Function

    <DllImport("Wlanapi", EntryPoint:="WlanEnumInterfaces")> _
    Private Function WlanEnumInterfaces(ByVal hClientHandle As IntPtr, ByVal pReserved As IntPtr, ByRef ppInterfaceList As IntPtr) As UInteger
    End Function

    <DllImport("Wlanapi.dll", EntryPoint:="WlanGetAvailableNetworkList")>
    Public Function WlanGetAvailableNetworkList(ByVal hClientHandle As IntPtr, ByRef pInterfaceGuid As IntPtr, ByVal dwFlags As UInteger, ByVal pReserved As IntPtr, ByRef ppAvailableNetworkList As IntPtr) As UInteger
    End Function


    Public Sub Net_GetUpDownStats(ByRef bytesSent As Long, ByRef bytesReceived As Long)
        bytesSent = 0
        bytesReceived = 0
        Try

            Dim iColl() As NetworkInterface = Nothing

            iColl = NetworkInterface.GetAllNetworkInterfaces()

            Dim retBytesSent As Long = 0
            Dim retBytesReceived As Long = 0

            Dim i As Integer
            Dim iStats As IPv4InterfaceStatistics = Nothing
            For i = 0 To iColl.Length - 1

                iStats = iColl(i).GetIPv4Statistics

                retBytesSent = retBytesSent + iStats.BytesSent
                retBytesReceived = retBytesReceived + iStats.BytesReceived

            Next i

            bytesSent = retBytesSent
            bytesReceived = retBytesReceived
        Catch ex As Exception

        End Try
    End Sub

    Public Function Net_IsWiFiConnected() As Boolean
        Dim retBool As Boolean = False
        Dim tempNetIFace As Net.NetworkInformation.NetworkInterface
        For Each tempNetIFace In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces
            If tempNetIFace.OperationalStatus = Net.NetworkInformation.OperationalStatus.Up Then
                Select Case tempNetIFace.Speed
                    Case 2000000            'wifi A
                        retBool = True
                    Case 11000000           'wifi B
                        retBool = True
                    Case 54000000           'wifi G  (tested)
                        retBool = True
                    Case 150000000          'wifi N
                        retBool = True
                End Select
                If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.Wireless80211 Then
                    retBool = True
                    Exit For
                End If
            End If

        Next
        Return retBool
    End Function


    'Public Function Net_IsCellularConnected() As Boolean

    '    Dim retBool As Boolean = False

    '    Dim tempNetIFace As Net.NetworkInformation.NetworkInterface

    '    For Each tempNetIFace In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces
    '        If tempNetIFace.OperationalStatus = Net.NetworkInformation.OperationalStatus.Up Then


    '            'If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.Wman Then        'WiMAX
    '            '    retBool = True
    '            'End If
    '            'If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.Wwanpp Then      'GSM
    '            '    retBool = True
    '            'End If
    '            'If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.Wwanpp2 Then     'CDMA
    '            '    retBool = True
    '            'End If
    '            If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.Wireless80211 Then        'WiFi
    '                retBool = True
    '                Exit For
    '            End If
    '            If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.GenericModem Then      'GSM
    '                retBool = True
    '                Exit For
    '            End If
    '            If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.PrimaryIsdn Then     'CDMA
    '                retBool = True
    '                Exit For
    '            End If
    '        End If

    '    Next

    '    Return retBool

    'End Function

    Public Function Net_IsLANconnected() As Boolean
        Dim retBool As Boolean = False
        Dim tempNetIFace As Net.NetworkInformation.NetworkInterface
        For Each tempNetIFace In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces
            If tempNetIFace.OperationalStatus = Net.NetworkInformation.OperationalStatus.Up Then
                If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.Ethernet Then
                    retBool = True
                End If
                If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.Ethernet3Megabit Then
                    retBool = True
                End If
                If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.FastEthernetFx Then
                    retBool = True
                End If
                If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.FastEthernetT Then
                    retBool = True
                End If
                If tempNetIFace.NetworkInterfaceType = Net.NetworkInformation.NetworkInterfaceType.GigabitEthernet Then
                    retBool = True
                End If
            End If
        Next
        Return retBool
    End Function
    Public Function Net_LanType() As String
        Dim retStr As String = ""
        Dim tempNetIFace As Net.NetworkInformation.NetworkInterface
        For Each tempNetIFace In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces
            If tempNetIFace.OperationalStatus = Net.NetworkInformation.OperationalStatus.Up Then
                Select Case tempNetIFace.NetworkInterfaceType
                    Case Net.NetworkInformation.NetworkInterfaceType.Ethernet 'IEE802.3
                        retStr = "Ethernet"
                    Case Net.NetworkInformation.NetworkInterfaceType.Ethernet3Megabit 'RCF895
                        retStr = "Ethernet3Megabit"
                    Case Net.NetworkInformation.NetworkInterfaceType.FastEthernetFx '100Base-FX (optical fibre)
                        retStr = "100Base-FX"
                    Case Net.NetworkInformation.NetworkInterfaceType.FastEthernetT '100Base-T
                        retStr = "100Base-T"
                    Case Net.NetworkInformation.NetworkInterfaceType.GigabitEthernet '1Gigabit
                        retStr = "1Gigabit"

                End Select

            End If
        Next
        Return retStr
    End Function

    Public Function Net_NetworkDescription() As String
        Dim tempNetIFace As Net.NetworkInformation.NetworkInterface
        Dim tempName As String = ""
        For Each tempNetIFace In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces
            If tempNetIFace.OperationalStatus = Net.NetworkInformation.OperationalStatus.Up Then
                tempName = tempNetIFace.Description
                Exit For
            End If
        Next
        Return tempName
    End Function

    Public Sub Net_CheckBetterNetwork()
        Try
            NetWorkNameList.Clear()
            Dim tempNetIFace As Net.NetworkInformation.NetworkInterface
            Dim retLanType As String = Net_LanType()
            For Each tempNetIFace In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces
                If tempNetIFace.OperationalStatus = Net.NetworkInformation.OperationalStatus.Up Then
                    NetWorkNameList.Add(tempNetIFace.Description)
                End If
            Next

            'For n As Integer = 0 To NetWorkNameList.Count - 1
            '    MessageBox.Show(NetWorkNameList.Item(n))
            'Next
            If NetWorkNameList.Contains("Bluetooth PAN Network Adapter") Then
                BluetoothPANNetworkAdapter = True
            Else
                BluetoothPANNetworkAdapter = False
            End If
            If NetWorkNameList.Contains("Bluetooth PAN Network Adapter") And Net_NetworkDescription() <> "Bluetooth PAN Network Adapter" Then
                ToLog("Better speed Network than PAN service is found!")
                btnUnTether()
                Threading.Thread.Sleep(1000)
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

    End Sub

    Public Function Net_IsBlueToothPANconnected() As Boolean
        Dim retBool As Boolean = False
        Dim tempNetIFace As Net.NetworkInformation.NetworkInterface
        Dim tempName As String = ""
        Dim tempDesc As String = ""
        For Each tempNetIFace In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces
            If tempNetIFace.OperationalStatus = Net.NetworkInformation.OperationalStatus.Up Then
                Try
                    tempName = Replace(tempNetIFace.Name, " ", "")
                    If InStr(1, tempName, "bluetooth", CompareMethod.Text) <> 0 Then
                        retBool = True
                        Exit For
                    End If
                Catch ex As Exception

                End Try
                Try
                    tempDesc = Replace(tempNetIFace.Description, " ", "")
                    If InStr(1, tempDesc, "bluetooth", CompareMethod.Text) <> 0 Then
                        retBool = True
                        Exit For
                    End If
                Catch ex As Exception

                End Try
            End If
        Next
        Return retBool

    End Function

    Public Function INet_GetOnlineStatus(Optional ByVal urlForTest As String = "http://www.google.com/") As Boolean
        If Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable = False Then
            Return False
        End If
        Dim retBool As Boolean = False
        'retBool = WebAPI_TestURL(urlForTest)
        'Return retBool
        'Exit Function
        Dim strURL As String = urlForTest
        Dim objWebReq As System.Net.WebRequest
        Dim objResp As System.Net.WebResponse
        Try
            objWebReq = System.Net.WebRequest.Create(strURL)

            'change the user-agent to a mobile browser.  maybe this will result in less data being transferred.
            'CType(objWebReq, Net.HttpWebRequest).UserAgent = DriveLine_Internet_MobileUserAgent

            objResp = objWebReq.GetResponse

            If IsNothing(objResp) = False Then
                If objResp.IsFromCache = False Then
                    If objResp.ContentType.ToString <> "" Then
                        retBool = True
                    End If
                End If
            End If


            objResp.Close()
            objWebReq = Nothing
            objResp = Nothing
            objWebReq = Nothing

        Catch ex As Exception
            objWebReq = Nothing
            objResp = Nothing
            objWebReq = Nothing
        End Try

        Return retBool

    End Function



    Public Function INet_GetFavIcon(ByVal objWebClient As Net.WebClient, ByVal siteURL As String) As IO.MemoryStream

        Return Nothing

    End Function


    Public Function INet_GetFileAsString(ByVal objWebClient As Net.WebClient, ByVal webFileName As String, Optional ByVal webUserName As String = "", Optional ByVal webPassWord As String = "") As String


        objWebClient.Credentials = New System.Net.NetworkCredential(webUserName, webPassWord)

        Dim retString As String = ""

        Try
            retString = objWebClient.DownloadString(webFileName)
        Catch ex As Exception

        End Try

        objWebClient.Dispose()

        Return retString

    End Function



    Public Function INet_GetFileSize(ByVal objWebClient As Net.WebClient, ByVal webFileName As String, Optional ByVal webUserName As String = "", Optional ByVal webPassWord As String = "") As Long


        objWebClient.Credentials = New System.Net.NetworkCredential(webUserName, webPassWord)

        Dim retLong As Long = -1

        Dim tempStream As IO.Stream = Nothing

        Try
            tempStream = objWebClient.OpenRead(webFileName)
            retLong = Long.Parse(objWebClient.ResponseHeaders("Content-Length"))

        Catch ex As Exception
            retLong = -1

        End Try

        If IsNothing(tempStream) Then tempStream.Dispose()

        objWebClient.Dispose()

        Return retLong

    End Function


    Public Function INet_DownloadFile_Resume(ByVal sSourceURL As String, ByVal sDestinationPath As String, ByRef progressPct As Double) As Boolean

        progressPct = 0

        Dim iFileSize As Long = 0
        Dim iBufferSize As Integer = 1024
        iBufferSize *= 1000
        Dim iExistLen As Long = 0
        Dim saveFileStream As System.IO.FileStream
        If System.IO.File.Exists(sDestinationPath) Then
            Dim fINfo As New System.IO.FileInfo(sDestinationPath)
            iExistLen = fINfo.Length
        End If

        If iExistLen > 0 Then
            saveFileStream = New System.IO.FileStream(sDestinationPath, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite)
        Else
            saveFileStream = New System.IO.FileStream(sDestinationPath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite)
        End If

        Dim hwRq As System.Net.HttpWebRequest
        Dim hwRes As System.Net.HttpWebResponse
        hwRq = DirectCast(System.Net.HttpWebRequest.Create(sSourceURL), System.Net.HttpWebRequest)
        hwRq.AddRange(CInt(iExistLen))
        Dim smRespStream As System.IO.Stream
        hwRes = DirectCast(hwRq.GetResponse(), System.Net.HttpWebResponse)
        smRespStream = hwRes.GetResponseStream()

        iFileSize = hwRes.ContentLength

        Dim iByteSize As Integer
        Dim downBuffer As Byte() = New Byte(iBufferSize - 1) {}

        Dim bytesRetrieved As Long = iExistLen


        Do
            iByteSize = smRespStream.Read(downBuffer, 0, downBuffer.Length)
            bytesRetrieved = bytesRetrieved + iByteSize

            If iFileSize > 0 Then
                progressPct = bytesRetrieved / iFileSize * 100
            Else
                progressPct = 0
            End If

            If iByteSize > 0 Then
                saveFileStream.Write(downBuffer, 0, iByteSize)
            Else
                Exit Do
            End If

        Loop

        saveFileStream.Close()
        saveFileStream.Dispose()

        smRespStream.Close()
        smRespStream.Dispose()



        If System.IO.File.Exists(sDestinationPath) Then
            Dim fINfo As New System.IO.FileInfo(sDestinationPath)
            iExistLen = fINfo.Length

            If iExistLen = iFileSize Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If


    End Function

End Module
