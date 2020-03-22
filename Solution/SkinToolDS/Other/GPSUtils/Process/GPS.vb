Public Class GPS
    Public GGACount As Integer = 0

    Public ResetStartTime As Boolean = True
    Dim LastTimeStamp As Date
    Dim SecondsSinceStart As Integer = 0

    Public ReqFixType As Integer = 0
    Public MinSatCount As Integer = 0
    Public MaxCorrAge As Decimal = 99
    Public MaxHDOP As Decimal = 99

    Dim datafound As Boolean
    Dim InLat As Double = 0
    Dim InLon As Double = 0
    Dim InFixType As Integer = 0
    Dim InSatsTracked As Integer = 0
    Dim InHDOP As Single = 0
    Dim InAltitude As Single = 0
    Dim intime As String = ""
    Dim gpstime As String
    Dim ghour As String
    Dim gminute As String
    Dim gsecond As String
    Dim UTCtime As Date
    Dim diffseconds As Integer

    Public Sub ProcessNMEAdata(ByVal x As String)
        'GPRMC (Required) contains time, lat, lon, speed, heading, date
        'GPGGA (Required) contains fix quality, # of sats tracked, HDOP, and Altitude

        Dim charlocation As Integer = x.LastIndexOf("$") 'Find location of the last $
        If charlocation = -1 Or charlocation + 1 > x.Length - 5 Then Exit Sub 'no $ found or not enough data left
        x = Mid(x, charlocation + 1) 'drop characters before the $

        charlocation = x.IndexOf("*") 'Find location of first *
        If charlocation = -1 Then
            Exit Sub 'no * found
        End If
        If x.Length < charlocation + 3 Then 'there aren't 2 characters after the *
            'AppendNMEALineToLog("Checksum Missing: " & x)
            Exit Sub
        ElseIf x.Length > charlocation + 3 Then 'there is extra data after the *
            x = Mid(x, 1, charlocation + 3) 'remove the extra data after 2 chars after the *
        End If
        If x.Length < 8 Then
            Exit Sub 'not enough data left
        End If


        Dim aryNMEALine() As String = Split(x, "*")
        If CalculateChecksum(Replace(aryNMEALine(0), "$", "")) = aryNMEALine(1) Then
            'Checksum matches, send it to the respective subroutine for processing.
            Select Case Left(aryNMEALine(0), 6)
                Case "$GPGGA"
                    ProcessGPGGA(aryNMEALine(0))
                Case Else
                    Dim stophere As Integer = 0
            End Select
            'Else, Apparently not RMC, GGA, GSV, or GSA, so we don't care about it.

            'AppendNMEALineToLog(x)
        Else
            'Checksum failed
            Dim stophere As Integer = 0
            'AppendNMEALineToLog("Checksum Failed: " & x)
        End If
    End Sub

    Private Sub ProcessGPGGA(ByVal code As String)
        'This is a GGA line and has 11+ fields; check that we have at least 11
        '$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47
        '0     ,1     ,2       ,3,4        ,5,6,7 ,8  ,9   ,10,11  ,12

        datafound = False

        Dim aryNMEAString() As String = Split(code, ",")
        If UBound(aryNMEAString) > 13 Then
            'we have at least 14 fields.
            'Ignore time, lat, lon

            If Len(aryNMEAString(1)) > 5 Then
                intime = aryNMEAString(1)
                ghour = intime.Substring(0, 2)
                gminute = intime.Substring(2, 2)
                gsecond = intime.Substring(4, 2)
                If IsNumeric(ghour) And IsNumeric(gminute) And IsNumeric(gsecond) Then
                    gpstime = "01/01/2000 " & CInt(ghour).ToString & ":" & gminute & ":" & gsecond 'We only care about the time.
                    If IsDate(gpstime) Then
                        UTCtime = CDate(gpstime)
                        If ResetStartTime Then
                            ResetStartTime = False
                            SecondsSinceStart = 0
                        Else
                            diffseconds = DateDiff(DateInterval.Second, LastTimeStamp, UTCtime)
                            If diffseconds < 0 Then diffseconds += 86400 'Time stamp rolls to new day, add 1 day worth of seconds.
                            SecondsSinceStart += diffseconds
                        End If
                        LastTimeStamp = UTCtime
                    End If
                End If
            Else
                Dim stophere As Integer = 0
            End If

            'Check that we have data in the lat/lon fields.
            If aryNMEAString(2) <> "" And aryNMEAString(3) <> "" And aryNMEAString(4) <> "" And aryNMEAString(5) <> "" Then
                If IsNumeric(aryNMEAString(2)) And IsNumeric(aryNMEAString(4)) Then
                    'get the coords
                    datafound = True

                    Dim snglat As Double = (CDbl(aryNMEAString(2)) / 100)
                    Dim snglatmins As Double = snglat Mod 1
                    snglat = snglat - snglatmins
                    snglatmins = snglatmins * 100 / 60
                    InLat = snglat + snglatmins
                    If aryNMEAString(3) = "S" Then
                        InLat = 0 - InLat
                    End If

                    Dim snglon As Double = (CDbl(aryNMEAString(4)) / 100)
                    Dim snglonmins As Double = snglon Mod 1
                    snglon = snglon - snglonmins
                    snglonmins = snglonmins * 100 / 60
                    InLon = snglon + snglonmins
                    If aryNMEAString(5) = "W" Then
                        InLon = 0 - InLon
                    End If
                End If
            Else
                Dim stophere As Integer = 0
            End If

            If aryNMEAString(6) <> "" And IsNumeric(aryNMEAString(6)) Then
                InFixType = CInt(aryNMEAString(6))
            End If

            Dim InCorrAge As String = aryNMEAString(13)
            If InCorrAge.Length = 0 Then
                InCorrAge = "0"
            Else
                If Not IsNumeric(InCorrAge) Then
                    InCorrAge = "0"
                End If
            End If

            Dim InStationID As String = aryNMEAString(14)

            If aryNMEAString(7) <> "" And IsNumeric(aryNMEAString(7)) Then
                InSatsTracked = CInt(aryNMEAString(7))
            End If

            If aryNMEAString(8) <> "" And IsNumeric(aryNMEAString(8)) Then
                InHDOP = CDec(aryNMEAString(8))
            End If

            If aryNMEAString(9) <> "" And IsNumeric(aryNMEAString(9)) Then
                InAltitude = CDec(aryNMEAString(9))
            End If


            'Check to see if the data fits within the limits.
            If ReqFixType > 0 Then
                If Not InFixType = ReqFixType Then datafound = False
            End If
            If InCorrAge > MaxCorrAge Then datafound = False
            If InSatsTracked < MinSatCount Then datafound = False
            If InHDOP > MaxHDOP Then datafound = False
        End If




        '--------------------Now Record the data if we got any.
        If datafound = True Then
            GGACount += 1
            Dim R As DataRow = Form1.dtcoords.NewRow()
            R.Item("lat") = InLat
            R.Item("lon") = InLon
            R.Item("sigtype") = InFixType
            R.Item("hdop") = InHDOP
            R.Item("elev") = InAltitude
            R.Item("rtime") = SecondsSinceStart
            R.Item("sats") = InSatsTracked
            Form1.dtcoords.Rows.Add(R)
        Else
            Dim stophere As Integer = 0
        End If
    End Sub

    









    Public Function CalculateChecksum(ByVal sentence As String) As String
        ' Calculates the checksum for a sentence
        ' Loop through all chars to get a checksum
        Dim Character As Char
        Dim Checksum As Integer
        For Each Character In sentence
            Select Case Character
                Case "$"c
                    ' Ignore the dollar sign
                Case "*"c
                    ' Stop processing before the asterisk
                    Exit For
                Case Else
                    ' Is this the first value for the checksum?
                    If Checksum = 0 Then
                        ' Yes. Set the checksum to the value
                        Checksum = Convert.ToByte(Character)
                    Else
                        ' No. XOR the checksum with this character's value
                        Checksum = Checksum Xor Convert.ToByte(Character)
                    End If
            End Select
        Next
        ' Return the checksum formatted as a two-character hexadecimal
        Return Checksum.ToString("X2")
    End Function
End Class
