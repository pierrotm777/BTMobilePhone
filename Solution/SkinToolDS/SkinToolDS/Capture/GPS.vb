Public Class GPS
    Private LastFixQuality As Integer = -1
    Private LastSatsTracked As Integer = -1
    Private LastHDOP As Decimal = 0
    Private LastVDOP As Decimal = 0
    Private LastPDOP As Decimal = 0
    Public LastElevInMeters As Decimal = 0
    Private LastCorrAge As String = "-"
    Private LastStationID As String = "0"

    Private LastGGATimeStamp As Integer = -1 'Used for detecting a new day (in GMT)

    Public InitialLoggedTime As Date = Now
    Public LastLoggedTime As Date = Now
    Public RecordQueue As String = ""
    Public LinesInRecordQueue As Integer = 0


    Dim inlatitude As Double = 0
    Dim inlongitude As Double = 0
    Dim infixquality As Integer = 0
    Dim insatstracked As Integer = 0
    Dim gpstype As String
    Dim fixsorted As Integer
    Dim lastfixsorted As Integer = -1


    Public Sub ProcessNMEAdata(ByVal x As String)
        'GPRMC (Required) contains time, lat, lon, speed, heading, date - We use RMC for **ALL** of the logging and steering commands
        'GPGGA (Required) contains fix quality, # of sats tracked, HDOP, and Altitude. Only useful to tell us when we lose sat signal.
        'GPGSV (Not Required) contains location and signal strength about up to 4 of the satellites in view. This is just used to display what sats are where.
        'GPGSA (Not Required) contains fix type, sat PRNs used, PDOP, HDOP, and VDOP. This just compliments GPGSV.

        If Form1.boxDataToLog.SelectedIndex = 2 Then
            RecordLine(x)
        End If

        Dim charlocation As Integer = x.LastIndexOf("$") 'Find location of the last $
        If charlocation = -1 Or charlocation + 1 > x.Length - 5 Then Exit Sub 'no $ found or not enough data left
        x = Mid(x, charlocation + 1) 'drop characters before the $

        charlocation = x.IndexOf("*") 'Find location of first *
        If charlocation = -1 Then Exit Sub 'no * found
        If x.Length < charlocation + 3 Then 'there aren't 2 characters after the *
            Exit Sub
        ElseIf x.Length > charlocation + 3 Then 'there is extra data after the *
            x = Mid(x, 1, charlocation + 3) 'remove the extra data after 2 chars after the *
        End If
        If x.Length < 8 Then Exit Sub 'not enough data left

        Dim aryNMEALine() As String = Split(x, "*")
        'lets see if the checksum matches the stuff before the astrix
        If CalculateChecksum(Replace(aryNMEALine(0), "$", "")) = aryNMEALine(1) Then
            'Checksum matches, send it to the respective subroutine for processing.
            Select Case Left(aryNMEALine(0), 6)
                Case "$GPRMC"
                    ProcessGPRMC(aryNMEALine(0))
                Case "$GPGGA"
                    'If Form1.IsRecording Then Form1.RecordNMEAData(x)
                    ProcessGPGGA(aryNMEALine(0), True)
                    Form1.MostRecentGGA = x
                Case "$GPGSV"
                    ProcessGPGSV(aryNMEALine(0))
                Case "$GPGSA"
                    'If Form1.IsRecording Then Form1.RecordNMEAData(x)
                    ProcessGPGSA(aryNMEALine(0))
            End Select
            'Else, Apparently not RMC, GGA, GSV, or GSA, so we don't care about it.

            Form1.AppendNMEALineToLog(x)
        Else
            'Checksum failed
            Form1.AppendNMEALineToLog("Checksum Failed: " & x)
        End If

        If Form1.IsRecording Then
            Select Case Form1.boxDataToLog.SelectedIndex
                Case 0
                    If Left(aryNMEALine(0), 6) = "$GPGGA" Then
                        RecordLine(x)
                    End If
                Case 1
                    If Left(aryNMEALine(0), 6) = "$GPGGA" Or Left(aryNMEALine(0), 6) = "$GPRMC" Then
                        RecordLine(x)
                    End If
                Case Else
                    'RecordLine(x) 'Moved this up above
            End Select
        End If
    End Sub




    Private Sub ProcessGPRMC(ByVal code As String)
        'This is a RMC line and has 9+ fields; check that it's active (good data)
        '$GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A
        '0     ,1     ,2,3       ,4,5        ,6,7    ,8    ,9     ,10   ,11

        Dim datafound As Boolean = False
        Dim inlatitude As Double = 0
        Dim inlongitude As Double = 0
        Dim inspeed As Decimal = 0
        Dim inheading As Decimal = 0
        Dim aryNMEAString() As String = Split(code, ",")

        If UBound(aryNMEAString) > 7 Then
            If aryNMEAString(2) = "A" Or aryNMEAString(2) = "D" Then
                'A and D represent a valid string
                'Check that we have data in the lat/lon fields.
                If aryNMEAString(3) <> "" And aryNMEAString(4) <> "" And aryNMEAString(5) <> "" And aryNMEAString(6) <> "" Then
                    If IsNumeric(aryNMEAString(3)) And IsNumeric(aryNMEAString(5)) Then
                        'get the coords, heading, speed, times, etc.
                        datafound = True

                        Dim snglat As Double = (CDbl(aryNMEAString(3)) / 100)
                        Dim snglatmins As Double = snglat Mod 1
                        snglat = snglat - snglatmins
                        snglatmins = snglatmins * 100 / 60
                        inlatitude = snglat + snglatmins
                        If aryNMEAString(4) = "S" Then
                            inlatitude = 0 - inlatitude
                        End If

                        Dim snglon As Double = (CDbl(aryNMEAString(5)) / 100)
                        Dim snglonmins As Double = snglon Mod 1
                        snglon = snglon - snglonmins
                        snglonmins = snglonmins * 100 / 60
                        inlongitude = snglon + snglonmins
                        If aryNMEAString(6) = "W" Then
                            inlongitude = 0 - inlongitude
                        End If

                        If aryNMEAString(7) <> "" And IsNumeric(aryNMEAString(7)) Then
                            inspeed = CDec(aryNMEAString(7)) ' * 1.15077945
                            'inspeed = Math.Round(inspeed, 2)
                        End If

                        If aryNMEAString(8) <> "" And IsNumeric(aryNMEAString(8)) Then
                            inheading = CDec(aryNMEAString(8))
                        End If
                    End If
                End If
            End If
        End If

        If datafound = True Then
            Dim headingsuffix As String = ""
            Select Case inheading
                Case Is < 22.5
                    headingsuffix = "N"
                Case Is < 67.5
                    headingsuffix = "NE"
                Case Is < 112.5
                    headingsuffix = "E"
                Case Is < 157.5
                    headingsuffix = "SE"
                Case Is < 202.5
                    headingsuffix = "S"
                Case Is < 247.5
                    headingsuffix = "SW"
                Case Is < 292.5
                    headingsuffix = "W"
                Case Is < 337.5
                    headingsuffix = "NW"
                Case Else
                    headingsuffix = "N"
            End Select

            'MainForm.lblStatLatitude.Text = inlatitude 'These are now coming from the GGA sentences
            'MainForm.lblStatLongitude.Text = inlongitude
            Form1.lblStatSpeed.Text = inspeed & " Km/Hr"
            Form1.lblStatHeading.Text = Format(inheading, "0.0").ToString & Chr(176) & " (" & headingsuffix & ")"
        End If
    End Sub

    Private Sub ProcessGPGGA(ByVal code As String, ByVal WriteToDataTable As Boolean)
        'This is a GGA line and has 11+ fields; check that we have at least 11
        '$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47
        '0     ,1     ,2       ,3,4        ,5,6,7 ,8  ,9   ,10,11  ,12

        Dim datafound As Boolean = False

        'Dim inhdop As Single = 0
        'Dim inaltitude As Single = 0
        Dim aryNMEAString() As String = Split(code, ",")

        If UBound(aryNMEAString) > 13 Then 'We have at least 14 fields.
            'Check the timestamp
            If IsNumeric(aryNMEAString(1)) Then
                Dim InTime As Integer = Fix(CDec(aryNMEAString(1)))

                If Form1.IsRecording Then
                    If Form1.LogDuration = 99999 Then
                        If LastGGATimeStamp - InTime > 230000 Then 'Roll to new LogFile
                            Form1.LogEvent("Stopped Recording... It's a new day in Greenwich.")
                            EmptyQueueAndStopRecording()
                        End If
                    End If

                    ''for testing
                    'If InTime Mod 100 = 0 Then
                    '    Form1.LogEvent("Stopped Recording... It's a new minute in Greenwich.")
                    '    EmptyQueueAndStopRecording()
                    'End If

                End If
                LastGGATimeStamp = InTime
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
                    inlatitude = snglat + snglatmins
                    If aryNMEAString(3) = "S" Then
                        inlatitude = 0 - inlatitude
                    End If

                    Dim snglon As Double = (CDbl(aryNMEAString(4)) / 100)
                    Dim snglonmins As Double = snglon Mod 1
                    snglon = snglon - snglonmins
                    snglonmins = snglonmins * 100 / 60
                    inlongitude = snglon + snglonmins
                    If aryNMEAString(5) = "W" Then
                        inlongitude = 0 - inlongitude
                    End If
                End If
            End If

            If aryNMEAString(6) <> "" And IsNumeric(aryNMEAString(6)) Then
                infixquality = CInt(aryNMEAString(6))
                If Not infixquality = LastFixQuality Then 'fix quality has changed
                    Select Case infixquality
                        Case 1 'GPS fix (SPS)
                            gpstype = "GPS fix (No Differential Correction)"
                            fixsorted = 10
                        Case 2 'DGPS fix
                            gpstype = "DGPS"
                            fixsorted = 20
                        Case 3 'PPS fix
                            gpstype = "PPS Fix"
                            fixsorted = 30
                        Case 4 'Real Time Kinematic
                            gpstype = "RTK (Real Time Kinematic)"
                            fixsorted = 40
                        Case 5 'Float RTK
                            gpstype = "Float RTK"
                            fixsorted = 39
                        Case 6 'estimated (dead reckoning) (2.3 feature)
                            gpstype = "Estimated (Dead Reconing)"
                            fixsorted = 11
                        Case 7 'Manual input mode
                            gpstype = "Manual Input Mode"
                            fixsorted = 8
                        Case 8 'Simulation mode
                            gpstype = "Simulation"
                            fixsorted = 9
                        Case 9 'WAAS on a Novatel
                            gpstype = "WAAS"
                            fixsorted = 21
                        Case Else
                            gpstype = "Invalid"
                            fixsorted = 0
                    End Select

                    Form1.lblStatGPSType.Text = gpstype

                    If LastFixQuality = -1 Then
                        Form1.LogEvent("GPS Fix Quality is " & infixquality & " (" & gpstype & ")")
                    ElseIf fixsorted > lastfixsorted Then
                        Form1.LogEvent("GPS Fix Quality Increased from " & LastFixQuality & " to " & infixquality & " (" & gpstype & ")")
                    Else
                        Form1.LogEvent("GPS Fix Quality Degraded from " & LastFixQuality & " to " & infixquality & " (" & gpstype & ")")
                    End If

                    LastFixQuality = infixquality
                    lastfixsorted = fixsorted
                End If

                Dim inCorrAge As String = aryNMEAString(13)
                If Not LastCorrAge = inCorrAge Then
                    If IsNumeric(inCorrAge) Then
                        Form1.lblStatCorrAge.Text = inCorrAge & " seconds"
                    Else
                        Form1.lblStatCorrAge.Text = inCorrAge
                    End If
                    LastCorrAge = inCorrAge
                End If

                Dim inStationID As String = aryNMEAString(14)
                If Not LastStationID = inStationID Then
                    Form1.lblStatCorrStationID.Text = inStationID
                    LastStationID = inStationID
                End If

            End If

            If aryNMEAString(7) <> "" And IsNumeric(aryNMEAString(7)) Then
                insatstracked = CInt(aryNMEAString(7))
                If Not insatstracked = LastSatsTracked Then
                    'sat count has changed
                    Form1.lblStatSatCount.Text = insatstracked.ToString

                    If LastSatsTracked = -1 Then
                        Form1.LogEvent("Number of Satellites tracked is " & insatstracked)
                    ElseIf insatstracked > LastSatsTracked Then
                        Form1.LogEvent("Number of Satellites tracked Increased from " & LastSatsTracked & " to " & insatstracked)
                    Else
                        Form1.LogEvent("Number of Satellites tracked Decreased from " & LastSatsTracked & " to " & insatstracked)
                    End If

                    LastSatsTracked = insatstracked
                End If
            End If

            If aryNMEAString(8) <> "" And IsNumeric(aryNMEAString(8)) Then
                LastHDOP = CDec(aryNMEAString(8))
                Form1.lblStatHDOP.Text = LastHDOP.ToString
            End If
        End If

        If aryNMEAString(9) <> "" And IsNumeric(aryNMEAString(9)) Then
            LastElevInMeters = CDec(aryNMEAString(9))
            Form1.lblStatAltitude.Text = LastElevInMeters.ToString & " M"
            'inaltitude = CInt(CSng(aryNMEAString(9)) * 3.2808399) 'This converts meters to feet
            'If Not inaltitude = lastaltitude Then
            '    'altitude has changed
            '    MainForm.lblStatAltitude.Text = inaltitude.ToString & " Feet"
            '    lastaltitude = inaltitude
            'End If
        End If

        If datafound = True Then
            Form1.lblStatLatitude.Text = inlatitude
            Form1.lblStatLongitude.Text = inlongitude
        End If
    End Sub

    Private Sub ProcessGPGSV(ByVal code As String)
        Dim PseudoRandomCode As String
        Dim Azimuth As String
        Dim Elevation As String
        Dim SignalToNoiseRatio As String
        ' Divide the sentence into words
        Dim Words() As String = Split(code, ",")
        ' Each sentence contains four blocks of satellite information.
        'Read each block and report each satellite's information
        Dim Count As Integer
        For Count = 1 To 4
            ' Does the sentence have enough words to analyze?
            If (Words.Length - 1) >= (Count * 4 + 3) Then
                If IsNumeric(Words(Count * 4)) And IsNumeric(Words(Count * 4 + 1)) And IsNumeric(Words(Count * 4 + 2)) And IsNumeric(Words(Count * 4 + 3)) Then
                    ' Yes.  Proceed with analyzing the block.  Does it contain any information?
                    If Words(Count * 4).Length > 0 And Words(Count * 4 + 1).Length > 0 And Words(Count * 4 + 2).Length > 0 And Words(Count * 4 + 3).Length > 0 Then
                        ' Yes. Extract satellite information and report it
                        PseudoRandomCode = Words(Count * 4)
                        Elevation = Words(Count * 4 + 1)
                        Azimuth = Words(Count * 4 + 2)
                        SignalToNoiseRatio = Words(Count * 4 + 3)
                        'If sat already exists, update info, otherwise, add it.
                        If Form1.dtsats.Rows.Contains(PseudoRandomCode) = True Then
                            Dim oldrow As DataRow = Form1.dtsats.Rows.Find(PseudoRandomCode)
                            oldrow("elevation") = Elevation
                            oldrow("azimuth") = Azimuth
                            oldrow("snr") = SignalToNoiseRatio
                            oldrow("lastupdate") = Now
                            'MsgBox(PseudoRandomCode & "," & Elevation & "," & Azimuth & "," & SignalToNoiseRatio)
                        Else
                            Form1.dtsats.Rows.Add(PseudoRandomCode, Elevation, Azimuth, SignalToNoiseRatio, Now)
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub ProcessGPGSA(ByVal code As String)
        Dim aryNMEAString() As String = Split(code, ",")
        If UBound(aryNMEAString) >= 17 Then
            'we have at least 15 fields.
            'Active sats can only include 12, which appear to be GPS only sats. Glonass isn't included

            If IsNumeric(aryNMEAString(15)) Then
                LastPDOP = CDec(aryNMEAString(15))
                Form1.lblStatPDOP.Text = LastPDOP
            End If
            If IsNumeric(aryNMEAString(16)) Then
                LastHDOP = CDec(aryNMEAString(16))
                Form1.lblStatHDOP.Text = LastHDOP
            End If
            If IsNumeric(aryNMEAString(17)) Then
                LastVDOP = CDec(aryNMEAString(17))
                Form1.lblStatVDOP.Text = LastVDOP
            End If

        End If
    End Sub



    Private Sub RecordLine(ByVal Line As String)
        If Form1.IsRecording Then
            If Form1.RecordedLineCount = 0 Then 'This is the first point. record the time it was taken at.
                InitialLoggedTime = Now
                Form1.lblStartTime.Text = "Start Time: " & InitialLoggedTime
                If Form1.LogDuration = 99999 Then
                    Form1.lblEndTime.Text = "End Time: New day (Greenwich Mean Time)"
                Else
                    Form1.lblEndTime.Text = "End Time: " & DateAdd(DateInterval.Minute, Form1.LogDuration, InitialLoggedTime).ToString
                End If
                Form1.lblStartTime.Visible = True
                Form1.lblEndTime.Visible = True

                Form1.LogEvent("Started Recording...")
                Form1.lblStatusBar.Text = "Recording data..."
                RecordQueue = ""
                If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\Data") Then
                    Try
                        My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\Data")
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                End If
                Dim filename As String = Year(Now) & Format(Month(Now), "00") & Format(DatePart(DateInterval.Day, Now), "00") & "-" & Format(Hour(Now), "00") & Format(Minute(Now), "00") & Format(Second(Now), "00") & "-" & Form1.tbSessionLabel.Text & ".gps"
                Form1.datafile = Application.StartupPath & "\Data\" & filename
                Form1.lblCurrentFile.Text = "Current File: " & filename
                Form1.lblCurrentFile.Visible = True
            End If

            Form1.RecordedLineCount += 1

            LastLoggedTime = Now()

            If DateDiff(DateInterval.Minute, InitialLoggedTime, LastLoggedTime) >= Form1.LogDuration Then 'Time has expired.
                Form1.IsRecording = False
                Form1.LogEvent("Stopped Recording... Automatic time limit expired.")
                EmptyQueueAndStopRecording()
                Exit Sub
            End If
        End If

        RecordQueue += Line & vbCrLf
        LinesInRecordQueue += 1
        If LinesInRecordQueue >= 300 Or RecordQueue.Length > 100000 Then
            WriteRecordingQueueToFile()
        End If
    End Sub

    Public Sub WriteRecordingQueueToFile()
        Try
            My.Computer.FileSystem.WriteAllText(Form1.datafile, RecordQueue, True)
            RecordQueue = "" 'Clear that out
            LinesInRecordQueue = 0
            Form1.lblStatusBar.Text = "Logged " & Form1.RecordedLineCount & " lines of data."
        Catch ex As Exception
            Form1.lblStatusBar.Text = "Write Error: " & ex.Message
        End Try

    End Sub

    Private Sub EmptyQueueAndStopRecording()
        WriteRecordingQueueToFile()
        Form1.IsRecording = False

        Select Case Form1.boxUponCompletion.SelectedIndex
            Case 0 'Start a new log file
                Form1.StopRecording()
                Form1.StartRecording()
            Case 1 'Do Nothing
                Form1.StopRecording()
            Case Else 'Close Program
                Form1.StopRecording()
                Form1.Close()
        End Select
    End Sub




    Public Function GenerateGPGGAcode() As String
        Dim posnum As Double = 0
        Dim minutes As Double = 0

        Dim UTCTime As Date = Date.UtcNow

        '$GPGGA,052158,4158.7333,N,09147.4277,W,2,08,3.1,260.4,M,-32.6,M,,*79

        Dim mycode As String = "GPGGA,"
        If Hour(UTCTime) < "10" Then
            mycode = mycode & "0"
        End If
        mycode = mycode & Hour(UTCTime)
        If Minute(UTCTime) < "10" Then
            mycode = mycode & "0"
        End If
        mycode = mycode & Minute(UTCTime)
        If Second(UTCTime) < "10" Then
            mycode = mycode & "0"
        End If
        mycode = mycode & Second(UTCTime)
        mycode = mycode & ","


        posnum = Math.Abs(Form1.NTRIPManualLat)
        minutes = posnum Mod 1
        posnum = posnum - minutes
        minutes = minutes * 60
        posnum = (posnum * 100) + minutes
        If posnum < 1000 Then
            mycode = mycode & "0"
            If posnum < 100 Then
                mycode = mycode & "0"
            End If
        End If
        mycode = mycode & posnum.ToString

        If Form1.NTRIPManualLat > 0 Then
            mycode = mycode & ",N,"
        Else
            mycode = mycode & ",S,"
        End If

        posnum = Math.Abs(Form1.NTRIPManualLon)
        minutes = posnum Mod 1
        posnum = posnum - minutes
        minutes = minutes * 60
        posnum = (posnum * 100) + minutes
        If posnum < 10000 Then
            mycode = mycode & "0"
            If posnum < 1000 Then
                mycode = mycode & "0"
                If posnum < 100 Then
                    mycode = mycode & "0"
                End If
            End If
        End If
        mycode = mycode & posnum.ToString

        If Form1.NTRIPManualLon > 0 Then
            mycode = mycode & ",E,"
        Else
            mycode = mycode & ",W,"
        End If

        mycode = mycode & "4,10,1,200,M,1,M,"

        mycode = mycode & (Second(Now) Mod 6) + 3 & ",0"


        mycode = "$" & mycode & "*" & CalculateChecksum(mycode)   'Add checksum data
        Return mycode
    End Function


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
