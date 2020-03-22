Public Class Form1
    'Send comments to Lance@Lefebure.com
    Public GPS As New Global.SkinToolDS.GPS
    Dim settingsfile As String = Application.StartupPath & "\Settings.txt"
    Public datafile As String = ""
    Dim EventLogFile As String = ""

    Public RecordingSession As String = ""
    Public LogDuration As Integer = 99999
    Public IsRecording As Boolean = False
    Public RecordedLineCount As Integer = 0

    Dim TimerTickCount As Integer = 0
    Public dtsats As New DataTable
    Dim SatLog1(1, -1) As Integer
    Dim SatLog2(1, -1) As Integer
    Dim SatLog3(1, -1) As Integer
    Dim SatLog4(1, -1) As Integer
    Dim SatLog5(1, -1) As Integer
    Dim SatLog6(1, -1) As Integer
    Dim SatLog7(1, -1) As Integer

    Public SerialPort As Integer = 1
    Public SerialSpeed As Integer = 9600
    Public SerialDataBits As Integer = 8
    Public SerialStopBits As Integer = 1
    Dim ReceiveBuffer As String
    Dim WithEvents COMPort As New System.IO.Ports.SerialPort

    Public StartNTRIPThreadIn As Integer = 0
    Public NTRIPShouldBeConnected As Boolean = False
    Public Shared NTRIPUseManualGGA As Boolean = False
    Public Shared NTRIPManualLat As Decimal = 41
    Public Shared NTRIPManualLon As Decimal = -91
    Public Shared NTRIPCaster As String = ""
    Public Shared NTRIPPort As Integer = 2101
    Public Shared NTRIPUsername As String = ""
    Public Shared NTRIPPassword As String = ""
    Public Shared NTRIPMountPoint As String = ""
    Public PreferredMountPoint As String = ""
    Public NTRIPThread As Threading.Thread
    Public NTRIPIsConnected As Boolean = False
    Public NTRIPConnectionAttempt As Integer = 1
    Public Shared NTRIPStreamRequiresGGA As Boolean = False
    Dim NTRIPByteCount As Integer = 0
    Public NTRIPStreamArray(1, -1) As String
    Public Shared MostRecentGGA As String = ""


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'This sets the decimal delimiter for users outside the US using strange number formating settings. This resolves the bug in IsNumeric()
        'http://www.codeproject.com/KB/vb/isnumeric.aspx
        Dim ci As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US", False)
        ci.NumberFormat.CurrencyDecimalSeparator = ","
        ci.NumberFormat.CurrencyGroupSeparator = "."
        Threading.Thread.CurrentThread.CurrentCulture = ci

        Dim ver As String = "Version: " & My.Application.Info.Version.Major & "." & Format(My.Application.Info.Version.Minor, "00") & "." & Format(My.Application.Info.Version.Build, "00")
        If My.Application.Info.Version.Revision <> 0 Then ver += " (Rev " & My.Application.Info.Version.Revision & ")"
        lblStatusBar.Text = ver

        LoadSettingsFile()
        LoadNTRIPSettings()

        dtsats.Columns.Add("id")
        dtsats.PrimaryKey = New DataColumn() {dtsats.Columns("id")}
        dtsats.Columns.Add("elevation")
        dtsats.Columns.Add("azimuth")
        dtsats.Columns.Add("snr")
        dtsats.Columns.Add("lastupdate")
        dtsats.DefaultView.Sort = "id"

        boxDataToLog.SelectedIndex = 0
        boxUponCompletion.SelectedIndex = 0

        boxLogForMinutes.Items.Add("1 Minute")
        boxLogForMinutes.Items.Add("10 Minutes")
        boxLogForMinutes.Items.Add("1 Hour")
        boxLogForMinutes.Items.Add("2 Hours")
        boxLogForMinutes.Items.Add("4 Hours")
        boxLogForMinutes.Items.Add("6 Hours")
        boxLogForMinutes.Items.Add("12 Hours")
        boxLogForMinutes.Items.Add("24 Hours")
        boxLogForMinutes.Items.Add("36 Hours")
        boxLogForMinutes.Items.Add("48 Hours")
        boxLogForMinutes.Items.Add("72 Hours")
        boxLogForMinutes.Items.Add("End of day (GMT)")
        boxLogForMinutes.SelectedIndex = 11

        Timer1.Start()
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Timer1.Stop()

        If Not NTRIPThread Is Nothing Then
            If NTRIPThread.IsAlive Then
                LogEvent("Closing NTRIP Thread...")
                Threading.Thread.Sleep(10)
                Application.DoEvents()

                NTRIPIsConnected = False
                'Wait for the thread to notice the change and stop.
                Threading.Thread.Sleep(100)
                Application.DoEvents()
                Threading.Thread.Sleep(100)
                Application.DoEvents()
                If NTRIPThread.IsAlive Then
                    NTRIPThread.Abort() 'Ok, kill the thread if it is still running.
                    Threading.Thread.Sleep(100)
                    Application.DoEvents()
                    Threading.Thread.Sleep(100)
                    Application.DoEvents()
                End If

                LogEvent("NTRIP Thread Closed")
                Threading.Thread.Sleep(10)
                Application.DoEvents()
            End If

        End If


        If COMPort.IsOpen Then
            LogEvent("Closing Serial Port...")
            Threading.Thread.Sleep(10)
            Application.DoEvents()

            RemoveHandler COMPort.DataReceived, AddressOf SerialInput
            Application.DoEvents()
            Threading.Thread.Sleep(1000)
            'CloseMySerialPort()

            LogEvent("Serial Port Closed")
            Threading.Thread.Sleep(10)
            Application.DoEvents()
        End If

        If e.CloseReason = CloseReason.UserClosing Then
            'cancel the close
            e.Cancel = True
            Me.Hide()
        End If

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If StartNTRIPThreadIn > 0 Then
            StartNTRIPThreadIn -= 1
            If StartNTRIPThreadIn = 0 Then StartNTRIP()
        End If

        TimerTickCount += 1
        If TimerTickCount = 3000 Then '5 minutes at 10 Hz
            'Every 5 minutes, grab a copy of sat locations
            SatLog7 = SatLog6
            SatLog6 = SatLog5
            SatLog5 = SatLog4
            SatLog4 = SatLog3
            SatLog3 = SatLog2
            SatLog2 = SatLog1
            Dim slcount As Integer = -1
            ReDim SatLog1(2, slcount)
            For Each row In dtsats.Rows
                If DateDiff(DateInterval.Second, row("lastupdate"), Now) < 60 Then
                    slcount += 1
                    ReDim Preserve SatLog1(2, slcount)
                    SatLog1(0, slcount) = row("elevation")
                    SatLog1(1, slcount) = row("azimuth")
                    SatLog1(2, slcount) = row("id")
                End If
            Next
            TimerTickCount = 0
        End If
    End Sub



    Private Sub LoadSettingsFile()
        'Check to make sure directory exists, if not, throw a WTF message.
        If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath) Then
            MsgBox("Error: The Application's folder doesn't exist. Settings file not loaded.")
            Exit Sub
        End If

        If Not My.Computer.FileSystem.FileExists(settingsfile) Then 'File doesn't exist. Create it.
            Dim fn As New IO.StreamWriter(IO.File.Open(settingsfile, IO.FileMode.Create))
            fn.WriteLine("# This is the Lefebure GPS Data Path Pointer file. You need to use the format ""Key=Value"" for all settings.")
            fn.WriteLine("# Any line that starts with a # symbol will be ignored.")
            fn.WriteLine("# The only setting in this file should be the Data Path Location.")
            fn.WriteLine("")
            fn.Close()
        End If

        'Open and read file
        Dim SettingsArray(1, 0) As String
        Dim keyvalpair(1) As String
        Dim key As String
        Dim value As String
        Dim lCtr As Integer = 0

        Try
            Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(settingsfile)
            Dim linein

            While oRead.Peek <> -1
                linein = Trim(oRead.ReadLine)
                If Len(linein) < 3 Then
                    'Line is too short
                ElseIf Asc(linein) = 35 Then
                    'Line starts with a #
                ElseIf InStr(linein, "=") < 2 Then
                    'There is no equal sign in the string
                Else
                    keyvalpair = Split(linein, "=", 2)
                    key = Trim(keyvalpair(0))
                    value = Trim(keyvalpair(1))
                    If Len(key) > 0 And Len(value) > 0 Then
                        'Looks good, add it to the array
                        ReDim Preserve SettingsArray(1, lCtr)
                        SettingsArray(0, lCtr) = LCase(key)
                        SettingsArray(1, lCtr) = value
                        lCtr = lCtr + 1
                    End If
                End If
            End While
            oRead.Close()
        Catch ex As Exception
        End Try

        Dim i As Integer = 0
        If lCtr > 0 Then
            For i = 0 To UBound(SettingsArray, 2)
                value = SettingsArray(1, i)
                Select Case SettingsArray(0, i)
                    Case "serial port number"
                        If IsNumeric(value) Then
                            Dim portnumber As Integer = CInt(value)
                            If portnumber > 0 And portnumber < 1025 Then
                                SerialPort = portnumber
                            Else
                                LogEvent("Specified Serial Port Number isn't in the range of 1 to 1024.")
                            End If
                        Else
                            LogEvent("Specified Serial Port Number isn't numeric.")
                        End If
                    Case "serial port speed"
                        If IsNumeric(value) Then
                            Dim portspeed As Integer = CInt(value)
                            If portspeed > 2399 And portspeed < 115201 Then
                                SerialSpeed = portspeed
                            Else
                                LogEvent("Specified Serial Port Speed isn't in the range of 2400 to 115200.")
                            End If
                        Else
                            LogEvent("Specified Serial Port Speed isn't numeric.")
                        End If
                    Case "serial port data bits"
                        If value = "7" Then
                            SerialDataBits = 7
                        ElseIf value = "8" Then
                            SerialDataBits = 8
                        Else
                            LogEvent("Specified Serial Port Data bits should be 7 or 8.")
                        End If
                    Case "serial port stop bits"
                        If value = "0" Then
                            SerialStopBits = 0
                        ElseIf value = "1" Then
                            SerialStopBits = 1
                        Else
                            LogEvent("Specified Serial Port Stop bits should be 0 or 1.")
                        End If


                    Case "ntrip use manual gga"
                        If LCase(value) = "yes" Then NTRIPUseManualGGA = True
                    Case "ntrip manual latitude"
                        If IsNumeric(value) Then
                            Dim inlat As Decimal = CDec(value)
                            If inlat > -90 And inlat < 90 Then
                                NTRIPManualLat = inlat
                            Else
                                LogEvent("Specified NTRIP Manual Latitude should be between -90 and 90.")
                            End If
                        Else
                            LogEvent("Specified NTRIP Manual Latitude should be numeric.")
                        End If
                    Case "ntrip manual longitude"
                        If IsNumeric(value) Then
                            Dim inlon As Decimal = CDec(value)
                            If inlon > -180 And inlon < 180 Then
                                NTRIPManualLon = inlon
                            Else
                                LogEvent("Specified NTRIP Manual Longitude should be between -180 and 180.")
                            End If
                        Else
                            LogEvent("Specified NTRIP Manual Longitude should be numeric.")
                        End If

                    Case "session label"
                        tbSessionLabel.Text = value

                    Case Else
                        'Key not found
                        If Not SettingsArray(0, i) = "" Then
                            'This will be blank if no settings were loaded
                            LogEvent("Just FYI, the """ & SettingsArray(0, i) & """ key in the data path pointer file isn't valid, so it was skipped.")
                        End If
                End Select
            Next
        End If





        boxSerialPort.Items.Clear()
        Dim targetport As String = "COM" & SerialPort.ToString

        i = 0
        Dim portindex As Integer = 0
        For Each portName As String In My.Computer.Ports.SerialPortNames
            boxSerialPort.Items.Add(portName)
            If portName = targetport Then
                portindex = i
            End If
            i += 1
        Next
        If i = 0 Then
            boxSerialPort.Items.Add("No Serial Ports Found")
        End If
        boxSerialPort.SelectedIndex = portindex

        If boxSpeed.Items.Count = 9 Then
            boxSpeed.Items.RemoveAt(8)
        End If

        Select Case SerialSpeed
            Case 2400
                boxSpeed.SelectedIndex = 0
            Case 4800
                boxSpeed.SelectedIndex = 1
            Case 9600
                boxSpeed.SelectedIndex = 2
            Case 14400
                boxSpeed.SelectedIndex = 3
            Case 19200
                boxSpeed.SelectedIndex = 4
            Case 38400
                boxSpeed.SelectedIndex = 5
            Case 57600
                boxSpeed.SelectedIndex = 6
            Case 115200
                boxSpeed.SelectedIndex = 7
            Case Else 'How did this happen
                If boxSpeed.Items.Count = 8 Then
                    boxSpeed.Items.Add(SerialSpeed.ToString)
                End If
                boxSpeed.SelectedIndex = 8
        End Select

        If SerialDataBits = 7 Then
            boxDataBits.SelectedIndex = 0
        Else
            boxDataBits.SelectedIndex = 1
        End If
    End Sub
    Public Sub SaveSetting(ByVal key1 As String, ByVal value1 As String, Optional ByVal key2 As String = "", Optional ByVal value2 As String = "", Optional ByVal key3 As String = "", Optional ByVal value3 As String = "")
        If Not My.Computer.FileSystem.FileExists(settingsfile) Then 'File doesn't exist. Create it.
            Dim fn As New IO.StreamWriter(IO.File.Open(settingsfile, IO.FileMode.Create))
            fn.WriteLine("# This is the Lefebure GPS Data Path Pointer file. You need to use the format ""Key=Value"" for all settings.")
            fn.WriteLine("# Any line that starts with a # symbol will be ignored.")
            fn.WriteLine("# The only setting in this file should be the Data Path Location.")
            fn.WriteLine("")
            fn.Close()
        End If


        Dim keyvalpair(1) As String
        Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(settingsfile)
        Dim linein As String
        Dim newfile As String = ""
        Dim foundkey1 As Boolean = False
        Dim foundkey2 As Boolean = False
        Dim foundkey3 As Boolean = False

        While oRead.Peek <> -1
            linein = Trim(oRead.ReadLine)
            If Len(linein) < 3 Then
                'Line is too short
                newfile += linein
            ElseIf Asc(linein) = 35 Then
                'Line starts with a #
                newfile += linein
            ElseIf InStr(linein, "=") < 2 Then
                'There is no equal sign in the string
                newfile += linein
            Else
                keyvalpair = Split(linein, "=", 2)
                If LCase(Trim(keyvalpair(0))) = LCase(key1) Then
                    'Found the right key, update it.
                    newfile += keyvalpair(0) & "=" & value1
                    foundkey1 = True
                ElseIf key2.Length > 0 And LCase(Trim(keyvalpair(0))) = LCase(key2) Then
                    newfile += keyvalpair(0) & "=" & value2
                    foundkey2 = True
                ElseIf key3.Length > 0 And LCase(Trim(keyvalpair(0))) = LCase(key3) Then
                    newfile += keyvalpair(0) & "=" & value3
                    foundkey3 = True
                Else
                    newfile += linein
                End If
            End If
            newfile += vbCrLf
        End While
        oRead.Close()

        If Not foundkey1 Then
            newfile += key1 & "=" & value1 & vbCrLf
        End If
        If key2.Length > 0 And Not foundkey2 Then
            newfile += key2 & "=" & value2 & vbCrLf
        End If
        If key3.Length > 0 And Not foundkey3 Then
            newfile += key3 & "=" & value3 & vbCrLf
        End If


        Try
            Dim sWriter As IO.StreamWriter = New IO.StreamWriter(settingsfile)
            sWriter.Write(newfile)
            sWriter.Flush()
            sWriter.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub LoadNTRIPSettings()
        boxMountpoint.Items.Add("Download Source Table")
        boxMountpoint.SelectedIndex = 0

        'Load NTRIP settings file
        Dim ntripconfigfile As String = Application.StartupPath & "\ntripconfig.txt"
        If My.Computer.FileSystem.FileExists(ntripconfigfile) Then
            Dim SettingsArray(1, 0) As String
            Dim keyvalpair(1) As String
            Dim key As String
            Dim value As String
            Dim lCtr As Integer = 0

            Try
                Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(ntripconfigfile)
                Dim linein

                While oRead.Peek <> -1
                    linein = Trim(oRead.ReadLine)
                    If Len(linein) < 3 Then
                        'Line is too short
                    ElseIf Asc(linein) = 35 Then
                        'Line starts with a #
                    ElseIf InStr(linein, "=") < 2 Then
                        'There is no equal sign in the string
                    Else
                        keyvalpair = Split(linein, "=", 2)
                        key = Trim(keyvalpair(0))
                        value = Trim(keyvalpair(1))
                        If Len(key) > 0 And Len(value) > 0 Then
                            'Looks good, add it to the array
                            ReDim Preserve SettingsArray(1, lCtr)
                            SettingsArray(0, lCtr) = LCase(key)
                            SettingsArray(1, lCtr) = value
                            lCtr = lCtr + 1
                        End If
                    End If
                End While
                oRead.Close()
            Catch ex As Exception
            End Try

            If lCtr > 0 Then
                For i = 0 To UBound(SettingsArray, 2)
                    value = SettingsArray(1, i)
                    Select Case SettingsArray(0, i)
                        Case "ntrip caster"
                            NTRIPCaster = value
                        Case "ntrip caster port"
                            If IsNumeric(value) Then
                                NTRIPPort = CInt(value)
                            End If
                        Case "ntrip username"
                            NTRIPUsername = value
                        Case "ntrip password"
                            NTRIPPassword = value
                        Case "ntrip mountpoint"
                            PreferredMountPoint = value
                    End Select
                Next
            End If
        End If

        tbNTRIPCasterIP.Text = NTRIPCaster
        tbNTRIPCasterPort.Text = NTRIPPort
        tbNTRIPUsername.Text = NTRIPUsername
        tbNTRIPPassword.Text = NTRIPPassword

        'Load sourcetable file into drop down list
        Dim sourcetablefile As String = Application.StartupPath & "\sourcetable.dat"
        If My.Computer.FileSystem.FileExists(sourcetablefile) Then
            'File exists. Open and parse
            Try
                Dim sourcefile As String = ""
                Dim linein As String
                Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(sourcetablefile)
                While oRead.Peek <> -1
                    linein = Trim(oRead.ReadLine)
                    sourcefile += linein & vbCrLf
                End While
                oRead.Close()

                If sourcefile.Length > 10 Then
                    ParseSourceTable(sourcefile)
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub
    Public Sub ParseSourceTable(ByVal table As String)
        ReDim NTRIPStreamArray(1, -1)
        Dim StreamCount As Integer = -1 'zero based array
        boxMountpoint.Items.Clear()
        boxMountpoint.Items.Add("Download Source Table")
        boxMountpoint.SelectedIndex = 0

        Dim lines() As String = Split(table, vbCrLf) 'Chr(13)
        For i = 0 To UBound(lines)
            Dim fields() As String = Split(lines(i), ";")
            If UBound(fields) > 4 Then
                If LCase(fields(0)) = "str" Then
                    'We found a STReam
                    boxMountpoint.Items.Add(fields(1))
                    StreamCount += 1
                    ReDim Preserve NTRIPStreamArray(1, StreamCount)
                    NTRIPStreamArray(0, StreamCount) = fields(1)
                    NTRIPStreamArray(1, StreamCount) = fields(11)
                End If
            End If
        Next

        Dim k As Integer = 0
        Dim selectedmnt As Integer = 0
        For Each item In boxMountpoint.Items
            If item = PreferredMountPoint Then
                selectedmnt = k
            End If
            k += 1
        Next

        If boxMountpoint.Items.Count = 1 Then
            boxMountpoint.SelectedIndex = 0
        Else
            boxMountpoint.SelectedIndex = selectedmnt
        End If
    End Sub
    Public Sub SaveNTRIPSettings()
        Dim ntripsettings As String = "NTRIP Caster=" & NTRIPCaster & vbCrLf
        ntripsettings += "NTRIP Caster Port=" & NTRIPPort.ToString & vbCrLf
        ntripsettings += "NTRIP Username=" & NTRIPUsername & vbCrLf
        ntripsettings += "NTRIP Password=" & NTRIPPassword & vbCrLf
        ntripsettings += "NTRIP MountPoint=" & PreferredMountPoint & vbCrLf
        Dim targetfile As String = Application.StartupPath & "\ntripconfig.txt"
        My.Computer.FileSystem.WriteAllText(targetfile, ntripsettings, False)
    End Sub

    Public Sub LogEvent(ByVal Message As String)
        If rtbEvents.TextLength > 5000 Then
            Dim NewText As String = Mid(rtbEvents.Text, 1000) 'Drop first 1000 characters
            NewText = NewText.Remove(0, NewText.IndexOf(ChrW(10)) + 1) 'Drop up to the next new line
            rtbEvents.Text = NewText
        End If

        rtbEvents.AppendText(vbCrLf & TimeOfDay() & " - " & Message)
        rtbEvents.SelectionStart = rtbEvents.TextLength
        rtbEvents.ScrollToCaret()

        If EventLogFile.Length > 0 Then
            Dim myStream As System.IO.StreamWriter = New System.IO.StreamWriter(EventLogFile, True)
            myStream.WriteLine(TimeOfDay() & " - " & Message)
            myStream.Close()
            myStream = Nothing
        End If
    End Sub
    Private Sub btnEventsToFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEventsToFile.Click
        If btnEventsToFile.Text = "Save Events to File" Then
            Dim sfd As New SaveFileDialog()
            sfd.InitialDirectory = Environment.CurrentDirectory()
            sfd.FileName = "eventlog" & Year(Now) & Format(Month(Now), "00") & Format(DatePart(DateInterval.Day, Now), "00") & "-" & Format(Hour(Now), "00") & Format(Minute(Now), "00") & Format(Second(Now), "00") & ".txt"
            sfd.Filter = "txt files (*.txt)|*.txt"
            sfd.FilterIndex = 2

            If sfd.ShowDialog() = DialogResult.OK Then
                'Open file
                Dim myStream As System.IO.StreamWriter = New System.IO.StreamWriter(sfd.FileName, False)

                'write to file
                Dim lines() As String = Split(rtbEvents.Text, vbLf)
                For i = 0 To UBound(lines)
                    myStream.WriteLine(lines(i))
                Next

                'close file
                myStream.Close()
                myStream = Nothing

                EventLogFile = sfd.FileName

                btnEventsToFile.Text = "Stop Saving Events"
            End If

        Else
            EventLogFile = ""
            btnEventsToFile.Text = "Save Events to File"
        End If
    End Sub

    Public Sub AppendNMEALineToLog(ByVal Line As String)
        If rtbDataStream.TextLength > 10000 Then
            Dim NewText As String = Mid(rtbDataStream.Text, 1000) 'Drop first 1000 characters
            NewText = NewText.Remove(0, NewText.IndexOf(ChrW(10)) + 1) 'Drop up to the next new line
            rtbDataStream.Text = NewText
        End If

        rtbDataStream.AppendText(vbCrLf & TimeOfDay() & " - " & Line)
        rtbDataStream.SelectionStart = rtbDataStream.TextLength
        rtbDataStream.ScrollToCaret()
    End Sub



    Private Sub btnSerialConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSerialConnect.Click
        If btnSerialConnect.Text = "Connect" Then
            If boxSerialPort.SelectedItem = "No Serial Ports Found" Then
                'Do nothing here
            Else 'Some serial port was selected
                SerialPort = CInt(Replace(boxSerialPort.SelectedItem, "COM", ""))
                SaveSetting("Serial Port Number", SerialPort)
            End If

            Select Case boxSpeed.SelectedIndex
                Case 0
                    SerialSpeed = 2400
                Case 1
                    SerialSpeed = 4800
                Case 2
                    SerialSpeed = 9600
                Case 3
                    SerialSpeed = 14400
                Case 4
                    SerialSpeed = 19200
                Case 5
                    SerialSpeed = 38400
                Case 6
                    SerialSpeed = 57600
                Case 7
                    SerialSpeed = 115200
                Case 8
                    'custom speed selected. Don't change it
            End Select

            If boxDataBits.SelectedIndex = 0 Then
                SerialDataBits = 7
            Else
                SerialDataBits = 8
            End If

            SaveSetting("Serial Port Speed", SerialSpeed, "Serial Port Data Bits", SerialDataBits, "Serial Port Stop Bits", SerialStopBits)


            OpenMySerialPort(True)
        Else
            CloseMySerialPort()
            'SaveSetting("Serial Should be Connected", "No")
        End If
    End Sub


    Private Sub btnNTRIPConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNTRIPConnect.Click
        If btnNTRIPConnect.Text = "Connect" Then
            NTRIPCaster = tbNTRIPCasterIP.Text
            If IsNumeric(tbNTRIPCasterPort.Text) Then
                NTRIPPort = CInt(tbNTRIPCasterPort.Text)
            Else
                tbNTRIPCasterPort.Text = NTRIPPort
            End If
            NTRIPUsername = tbNTRIPUsername.Text
            NTRIPPassword = tbNTRIPPassword.Text

            NTRIPConnectionAttempt = 1
            NTRIPShouldBeConnected = True
            'SaveSetting("NTRIP Should be Connected", "Yes")
            StartNTRIPThreadIn = 1
        Else
            NTRIPShouldBeConnected = False
            'SaveSetting("NTRIP Should be Connected", "No")
            StopNTRIP()
        End If
    End Sub


    Public Sub OpenMySerialPort(ByVal UserClickedConnect As Boolean)
        If COMPort.IsOpen Then
            COMPort.RtsEnable = False
            COMPort.DtrEnable = False
            COMPort.Close()
            Application.DoEvents()
            System.Threading.Thread.Sleep(500)
        End If

        lblSerialStatus.Text = "Connecting"

        COMPort.PortName = "COM" & SerialPort
        COMPort.BaudRate = SerialSpeed
        COMPort.DataBits = SerialDataBits
        'If SerialStopBits = 1 Then
        COMPort.StopBits = IO.Ports.StopBits.One
        'Else
        'COMPort.StopBits = IO.Ports.StopBits.None
        'End If
        COMPort.WriteTimeout = 2000
        COMPort.ReadTimeout = 2000

        AddHandler COMPort.DataReceived, AddressOf SerialInput

        Try
            COMPort.Open()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        If COMPort.IsOpen Then
            COMPort.RtsEnable = True
            COMPort.DtrEnable = True

            'Kick start the serial port so it starts reading data.
            COMPort.BreakState = True
            System.Threading.Thread.Sleep((11000 / COMPort.BaudRate) + 2) ' Min. 11 bit delay (startbit, 8 data bits, parity bit, stopbit)
            COMPort.BreakState = False

            'Change connect/disconnect button display status
            lblSerialStatus.Text = "Connected to COM " & SerialPort & " at " & SerialSpeed & "bps"
            btnSerialConnect.Text = "Disconnect"

            'lblLastLine.Text = "Waiting for data..."
            'lblLastLine.Visible = True
            LastLineTextBox.Text = "Waiting for data..." & vbCrLf & vbCrLf
            LastLineTextBox.Visible = True
        Else
            lblSerialStatus.Text = "Unable to open serial port"
        End If
    End Sub
    Public Sub CloseMySerialPort()
        RemoveHandler COMPort.DataReceived, AddressOf SerialInput
        Application.DoEvents()
        Threading.Thread.Sleep(1000)

        If COMPort.IsOpen Then COMPort.Close()

        'Change connect/disconnect button display status
        btnSerialConnect.Text = "Connect"
        lblSerialStatus.Text = "Disconnected"
        'lblLastLine.Visible = False
    End Sub

    Private Sub SerialInput(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs)
        Try
            ReceiveBuffer += COMPort.ReadExisting

            If InStr(ReceiveBuffer, vbCrLf) Then
                'If InStr(ReceiveBuffer, Chr(13)) Then
                'Contains at least one carridge return
                Dim lines() As String = Split(ReceiveBuffer, vbCrLf)
                'Dim lines() As String = Split(ReceiveBuffer, Chr(13))
                For i = 0 To UBound(lines) - 1
                    If lines(i).Length > 5 Then
                        SendSerialLineToUIThread(Trim(lines(i)))
                    End If
                Next
                ReceiveBuffer = lines(UBound(lines))
            Else
                'Data doesn't contain any line breaks
                If ReceiveBuffer.Length > 4000 Then
                    ReceiveBuffer = ""
                    SendSerialLineToUIThread("No line breaks found in data stream.")
                End If
            End If
        Catch ex As TimeoutException
            'MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub SendSerialLineToUIThread(ByVal Line As String)
        Try
            Dim uidel As New SendSerialLineToUIThreadDelegate(AddressOf CallBackSerialtoUIThread)
            Dim o(0) As Object
            o(0) = Line
            Invoke(uidel, o)
        Catch ex As Exception
        End Try
    End Sub
    Delegate Sub SendSerialLineToUIThreadDelegate(ByVal Line As String)
    Private Sub CallBackSerialtoUIThread(ByVal Line As String)
        'lblLastLine.Text = Line
        LastLineTextBox.AppendText(Line & vbCrLf)

        GPS.ProcessNMEAdata(Line)
    End Sub




    Public Sub StartNTRIP()
        'Check the status to see if it is already connected
        If Not NTRIPThread Is Nothing Then
            If NTRIPThread.IsAlive Then
                btnNTRIPConnect.Text = "Disconnect"
                LogEvent("NTRIP thread is already running. Please disconnect first before trying to connect again.")
                StopNTRIP()
                Exit Sub
            End If
        End If

        If NTRIPCaster = "" Then
            lblNTRIPStatus.Text = "No NTRIP Caster Specified"
            Exit Sub
        End If
        If NTRIPPort = 0 Then
            lblNTRIPStatus.Text = "No NTRIP Caster Port Specified"
            Exit Sub
        End If
        If NTRIPPort < 1 Or NTRIPPort > 65535 Then
            lblNTRIPStatus.Text = "Invalid Port Number"
            Exit Sub
        End If


        NTRIPMountPoint = boxMountpoint.SelectedItem
        If NTRIPMountPoint = "Download Source Table" Then
            NTRIPMountPoint = ""
        Else
            PreferredMountPoint = NTRIPMountPoint
        End If

        SaveNTRIPSettings()

        NTRIPStreamRequiresGGA = False
        For i = 0 To UBound(NTRIPStreamArray, 2)
            If NTRIPStreamArray(0, i) = NTRIPMountPoint Then
                If NTRIPStreamArray(1, i) = "1" Then
                    NTRIPStreamRequiresGGA = True
                End If
            End If
        Next

        boxMountpoint.Enabled = False
        btnNTRIPConnect.Text = "Disconnect"

        lblNTRIPStatus.Text = "Starting NTRIP Thread"
        Application.DoEvents()

        NTRIPIsConnected = True
        NTRIPThread = New Threading.Thread(AddressOf NTRIPLoop)
        NTRIPThread.Priority = Threading.ThreadPriority.AboveNormal
        NTRIPThread.Start()
    End Sub
    Public Sub StopNTRIP()
        'This gets called from the MyBase.FormClosed event as the app closes
        'Attempt to disconnect the nice way
        StartNTRIPThreadIn = 0
        NTRIPIsConnected = False
        lblNTRIPStatus.Text = "Disconnecting..."
        lblNTRIPConnectedAt.Visible = False

        'Wait for the thread to notice the change and stop.
        Threading.Thread.Sleep(100)
        Application.DoEvents()
        Threading.Thread.Sleep(100)
        Application.DoEvents()

        'Ok, kill the thread if it is still running.
        If Not NTRIPThread Is Nothing Then
            If NTRIPThread.IsAlive Then
                NTRIPThread.Abort()
                Threading.Thread.Sleep(100)
                Application.DoEvents()
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            End If
        End If

        pbNTRIP.Visible = False
        NTRIPConnectionAttempt += 1
        lblNTRIPStatus.Text = "Disconnected"

        If NTRIPConnectionAttempt > 1000000 Then
            btnNTRIPConnect.Visible = True
            NTRIPShouldBeConnected = False
            'SaveSetting("NTRIP Should be Connected", "No")
            LogEvent("NTRIP Client is Disconnected, 1000000 Failed Connection Attempts.")
        End If

        If NTRIPShouldBeConnected Then
            StartNTRIPThreadIn = 5
        Else
            btnNTRIPConnect.Text = "Connect"
            btnNTRIPConnect.Visible = True
            boxMountpoint.Enabled = True
        End If
    End Sub
    Public Sub NTRIPLoop()
        'Pause for a bit in case we just disconnected and are now reconnecting.
        Threading.Thread.Sleep(1000)

        Dim NeedsToSendGGA As Boolean = NTRIPStreamRequiresGGA 'This is a thread-local option that can get set to false later if only need to send GGA once.

        'This sub gets called on a new thread, it send/receives data, waits 100ms, then loops.
        If NeedsToSendGGA And Not NTRIPUseManualGGA Then 'Is GGA data required?
            If MostRecentGGA = "" Then 'Has GGA data been received?
                NTRIPUpdateUIThread(-1, "", Nothing) 'Waiting for GGA data
                Do While True
                    If Not MostRecentGGA = "" Then
                        Exit Do
                    End If
                    If Not NTRIPIsConnected Then 'Flag changed, kill the thread
                        NTRIPThread.Abort()
                    End If
                    Threading.Thread.Sleep(100)
                Loop
            End If
        End If


        Dim sckt As Net.Sockets.Socket
        Dim lcount As Integer = 97
        NTRIPUpdateUIThread(0, "", Nothing) 'Connecting


        'Connect to server
        sckt = New Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
        Try
            'sckt.Connect(New Net.IPEndPoint(NTRIPCaster, NTRIPPort))
            sckt.Connect(NTRIPCaster, NTRIPPort)
        Catch ex As Exception
            NTRIPUpdateUIThread(100, "Server did not respond.", Nothing)
            NTRIPThread.Abort()
        End Try


        NTRIPUpdateUIThread(1, "", Nothing) 'Connected

        'Build request message
        Dim msg As String = "GET /" & NTRIPMountPoint & " HTTP/1.0" & vbCr & vbLf
        msg += "User-Agent: NTRIP LefebureNTRIPClient/20090509" & vbCr & vbLf
        msg += "Accept: */*" & vbCr & vbLf & "Connection: close" & vbCr & vbLf
        If NTRIPUsername.Length > 0 Then
            Dim auth As String = ToBase64(NTRIPUsername & ":" & NTRIPPassword)
            msg += "Authorization: Basic " & auth & vbCr & vbLf 'This line can be removed if no authorization is needed
        End If
        msg += vbCr & vbLf

        'Send request
        Dim data As Byte() = System.Text.Encoding.ASCII.GetBytes(msg)
        sckt.Send(data)
        Threading.Thread.Sleep(100)

        'Wait for response
        Dim returndata As Byte() = New Byte(255) {}
        Try
            sckt.Receive(returndata)
        Catch ex As Exception
            NTRIPUpdateUIThread(100, "Unknown Response.", Nothing)
            NTRIPThread.Abort()
        End Try


        'Get response
        Dim responseData As String = System.Text.Encoding.ASCII.GetString(returndata, 0, returndata.Length)

        If responseData.Contains("SOURCETABLE 200 OK") Then
            'Start of source table was downloaded. Check for more data.
            For i = 0 To 50
                returndata = New Byte(255) {}
                Threading.Thread.Sleep(100)
                sckt.Receive(returndata)
                responseData += System.Text.Encoding.ASCII.GetString(returndata, 0, returndata.Length)
                If responseData.Contains("ENDSOURCETABLE") Then Exit For
            Next

            Dim targetfile As String = Application.StartupPath & "\sourcetable.dat"
            My.Computer.FileSystem.WriteAllText(targetfile, responseData, False)
            NTRIPUpdateUIThread(101, responseData, Nothing) 'Send on sourcetable for parsing

            sckt.Disconnect(False)
            NTRIPUpdateUIThread(100, "Downloaded Source Table", Nothing)
            NTRIPThread.Abort()
        ElseIf responseData.Contains("401 Unauthorized") Then
            'Login failed
            sckt.Disconnect(False)
            NTRIPUpdateUIThread(100, "Invalid Username or Password.", Nothing)
            NTRIPThread.Abort()
        ElseIf responseData.Contains("ICY 200 OK") Then
            NTRIPUpdateUIThread(2, "", Nothing) 'ICY 200 OK, Waiting for data
            Dim DataNotReceivedFor As Integer = 0
            Do While True
                Dim DataLength As Integer = sckt.Available
                If DataLength = 0 Then
                    DataNotReceivedFor += 1
                    If DataNotReceivedFor > 300 Then
                        'Data not received for 30 seconds. Terminate the connection.
                        NTRIPUpdateUIThread(100, "Connection Timed Out.", Nothing)
                        NTRIPThread.Abort()
                    End If
                Else
                    DataNotReceivedFor = 0
                    Dim InBytes(DataLength - 1) As Byte
                    sckt.Receive(InBytes, DataLength, Net.Sockets.SocketFlags.None)
                    NTRIPUpdateUIThread(3, Nothing, InBytes)
                End If

                lcount += 1
                If lcount = 100 Then
                    If NeedsToSendGGA Then
                        Dim TheGGA As String
                        If NTRIPUseManualGGA Then
                            TheGGA = GPS.GenerateGPGGAcode() 'This function runs in the NTRIP thread.
                            NeedsToSendGGA = False 'Only needs to be once when using a manual GGA
                        Else
                            TheGGA = MostRecentGGA
                        End If

                        Dim nmeadata As Byte() = System.Text.Encoding.ASCII.GetBytes(TheGGA & vbCrLf)
                        Try
                            sckt.Send(nmeadata)
                        Catch ex As Exception
                            NTRIPUpdateUIThread(100, "Error: " & ex.Message, Nothing)
                        End Try

                        NeedsToSendGGA = False 'Only send GGA data once
                    End If
                    lcount = 0
                End If

                If Not NTRIPIsConnected Then 'Flag changed, kill the thread
                    sckt.Disconnect(False)
                    NTRIPUpdateUIThread(100, "", Nothing)
                    NTRIPThread.Abort()
                End If
                Threading.Thread.Sleep(100)
            Loop
        Else
            sckt.Disconnect(False)
            NTRIPUpdateUIThread(100, "Unknown Response.", Nothing)
            NTRIPThread.Abort()
        End If
    End Sub
    Private Sub NTRIPUpdateUIThread(ByVal Item As Integer, ByVal Value As String, ByVal myBytes() As Byte)
        Try
            Dim uidel As New NTRIPUpdateUIThreadDelegate(AddressOf NTRIPCallBacktoUIThread)
            Dim o(2) As Object
            o(0) = Item
            o(1) = Value
            o(2) = myBytes
            Invoke(uidel, o)
        Catch ex As Exception
        End Try
    End Sub
    Delegate Sub NTRIPUpdateUIThreadDelegate(ByVal Item As Integer, ByVal Value As String, ByVal myBytes() As Byte)
    Private Sub NTRIPCallBacktoUIThread(ByVal Item As Integer, ByVal Value As String, ByVal myBytes() As Byte)
        Select Case Item
            Case -1
                lblNTRIPStatus.Text = "Waiting for NMEA GGA data..."
            Case 0
                lblNTRIPStatus.Text = "Connecting..."
                If NTRIPConnectionAttempt > 1 Then
                    lblNTRIPStatus.Text += " Attempt " & NTRIPConnectionAttempt
                    LogEvent("NTRIP Client is attempting to reconnect, Attempt " & NTRIPConnectionAttempt)
                Else
                    LogEvent("NTRIP Client is attempting to connect.")
                    If NTRIPStreamRequiresGGA And NTRIPUseManualGGA Then
                        LogEvent("NTRIP is using a simulated location of " & NTRIPManualLat & ", " & NTRIPManualLon)
                    End If
                End If
            Case 1
                lblNTRIPStatus.Text = "Connected, Requesting Data..."
                NTRIPByteCount = 0

            Case 2
                lblNTRIPStatus.Text = "Connected, Waiting for Data..."
                pbNTRIP.Value = 0
                pbNTRIP.Visible = True
                LogEvent("NTRIP Client is Connected, Waiting for Data.")

            Case 3
                Try
                    If COMPort.IsOpen Then
                        COMPort.Write(myBytes, 0, myBytes.Length)
                    End If
                Catch ex As Exception
                End Try
                If NTRIPByteCount = 0 Then
                    LogEvent("NTRIP Client is receiving data.")
                    lblNTRIPConnectedAt.Text = "Connected At: " & Now
                    lblNTRIPConnectedAt.Visible = True
                End If
                NTRIPByteCount += myBytes.Length
                lblNTRIPStatus.Text = "Connected, " & Format(NTRIPByteCount, "###,###,###,##0") & " bytes received."

                Dim remainder As Integer = CInt(NTRIPByteCount) Mod 5000
                remainder = CInt(remainder / 50)
                pbNTRIP.Value = remainder

            Case 100 'Thread commited suicide for some reason.
                If Value = "Invalid Username or Password." Then
                    NTRIPShouldBeConnected = False
                    'SaveSetting("NTRIP Should be Connected", "No")
                End If
                If Value = "Unknown Response." Then
                    'NTRIPShouldBeConnected = False
                    'SaveSetting("NTRIP Should be Connected", "No")
                End If

                If Value = "" Then
                    'lblNTRIPStatus.Text = "Disconnected"
                    LogEvent("NTRIP Client is Disconnected.")
                Else
                    'lblNTRIPStatus.Text = "Disconnected, " & Value
                    LogEvent("NTRIP Client is Disconnected, " & Value)
                End If

                StopNTRIP()

            Case 101 'Got Source Table, parse it
                ParseSourceTable(Value)
                btnNTRIPConnect.Visible = True
                LogEvent("NTRIP Client downloaded the Source Table.")
                NTRIPShouldBeConnected = False
                'SaveSetting("NTRIP Should be Connected", "No")
                StopNTRIP()

        End Select
    End Sub
    Private Function ToBase64(ByVal str As String) As String
        Dim asciiEncoding As System.Text.Encoding = System.Text.Encoding.ASCII
        Dim byteArray As Byte() = New Byte(asciiEncoding.GetByteCount(str) - 1) {}
        byteArray = asciiEncoding.GetBytes(str)
        Return Convert.ToBase64String(byteArray, 0, byteArray.Length)
    End Function



    Private Sub TabSatMap_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles TabSatMap.Paint
        'When this gets shown, plot the sat data points.

        'I can't write to a label in the paint event more than once, so we build the string and write it at the end
        Dim status As String = "PRN: Signal"

        'x=16, y=32, w and h =408

        Dim g As Graphics = e.Graphics
        Dim x As Integer = 16
        Dim y As Integer = 32
        Dim w As Integer = 408
        Dim h As Integer = 408
        Dim xc As Integer = w / 2 + x
        Dim yc As Integer = h / 2 + y

        g.DrawLine(Pens.Blue, xc, y, xc, y + h)
        g.DrawLine(Pens.Blue, x + w, yc, x, yc)

        g.DrawEllipse(Pens.Blue, x, y, w, h)
        g.DrawEllipse(Pens.Blue, CInt(x + w / 4), CInt(y + h / 4), CInt(w / 2), CInt(h / 2))

        Dim myFont As New Font("Ariel", 12)
        g.DrawString("N", myFont, Brushes.Gray, xc + 2, y + 3)
        g.DrawString("S", myFont, Brushes.Gray, xc + 2, y + h - 20)
        g.DrawString("W", myFont, Brushes.Gray, x + 4, yc - 18)
        g.DrawString("E", myFont, Brushes.Gray, x + w - 18, yc - 18)


        Dim prn As Integer
        Dim elevation As Integer
        Dim azimuth As Integer
        Dim snr As Integer
        Dim lastupdate As Date
        Dim satx As Integer
        Dim saty As Integer


        Dim trailcolor As Brush = New SolidBrush(Color.FromArgb(220, 220, 220))
        For i = 0 To UBound(SatLog7, 2)
            prn = SatLog7(2, i)
            elevation = 90 - SatLog7(0, i)
            azimuth = (SatLog7(1, i) - 90) Mod 360
            satx = CInt(xc + elevation * Math.Cos(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            saty = CInt(yc + elevation * Math.Sin(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            'g.FillEllipse(trailcolor, satx - 4, saty - 4, 8, 8)
            If prn <= 32 Then 'GPS, Circle
                g.FillEllipse(trailcolor, satx - 7, saty - 7, 14, 14)
            ElseIf prn >= 120 Then 'SBAS, Half Circle
                g.FillPie(trailcolor, satx - 10, saty - 5, 20, 20, 180, 180)
            ElseIf prn >= 38 And prn <= 61 Then 'Glonass, Square
                g.FillRectangle(trailcolor, satx - 6, saty - 6, 12, 12)
            Else 'Other, Triangle
                Dim pts() As Point = {New Point(satx, saty - 7), New Point(satx + 7, saty + 7), New Point(satx - 7, saty + 7)}
                g.FillPolygon(trailcolor, pts)
            End If
        Next
        trailcolor = New SolidBrush(Color.FromArgb(200, 200, 200))
        For i = 0 To UBound(SatLog6, 2)
            prn = SatLog6(2, i)
            elevation = 90 - SatLog6(0, i)
            azimuth = (SatLog6(1, i) - 90) Mod 360
            satx = CInt(xc + elevation * Math.Cos(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            saty = CInt(yc + elevation * Math.Sin(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            'g.FillEllipse(trailcolor, satx - 4, saty - 4, 8, 8)
            If prn <= 32 Then 'GPS, Circle
                g.FillEllipse(trailcolor, satx - 7, saty - 7, 14, 14)
            ElseIf prn >= 120 Then 'SBAS, Half Circle
                g.FillPie(trailcolor, satx - 10, saty - 5, 20, 20, 180, 180)
            ElseIf prn >= 38 And prn <= 61 Then 'Glonass, Square
                g.FillRectangle(trailcolor, satx - 6, saty - 6, 12, 12)
            Else 'Other, Triangle
                Dim pts() As Point = {New Point(satx, saty - 7), New Point(satx + 7, saty + 7), New Point(satx - 7, saty + 7)}
                g.FillPolygon(trailcolor, pts)
            End If
        Next
        trailcolor = New SolidBrush(Color.FromArgb(180, 180, 180))
        For i = 0 To UBound(SatLog5, 2)
            prn = SatLog5(2, i)
            elevation = 90 - SatLog5(0, i)
            azimuth = (SatLog5(1, i) - 90) Mod 360
            satx = CInt(xc + elevation * Math.Cos(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            saty = CInt(yc + elevation * Math.Sin(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            'g.FillEllipse(trailcolor, satx - 4, saty - 4, 8, 8)
            If prn <= 32 Then 'GPS, Circle
                g.FillEllipse(trailcolor, satx - 7, saty - 7, 14, 14)
            ElseIf prn >= 120 Then 'SBAS, Half Circle
                g.FillPie(trailcolor, satx - 10, saty - 5, 20, 20, 180, 180)
            ElseIf prn >= 38 And prn <= 61 Then 'Glonass, Square
                g.FillRectangle(trailcolor, satx - 6, saty - 6, 12, 12)
            Else 'Other, Triangle
                Dim pts() As Point = {New Point(satx, saty - 7), New Point(satx + 7, saty + 7), New Point(satx - 7, saty + 7)}
                g.FillPolygon(trailcolor, pts)
            End If
        Next
        trailcolor = New SolidBrush(Color.FromArgb(160, 160, 160))
        For i = 0 To UBound(SatLog4, 2)
            prn = SatLog4(2, i)
            elevation = 90 - SatLog4(0, i)
            azimuth = (SatLog4(1, i) - 90) Mod 360
            satx = CInt(xc + elevation * Math.Cos(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            saty = CInt(yc + elevation * Math.Sin(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            'g.FillEllipse(trailcolor, satx - 4, saty - 4, 8, 8)
            If prn <= 32 Then 'GPS, Circle
                g.FillEllipse(trailcolor, satx - 7, saty - 7, 14, 14)
            ElseIf prn >= 120 Then 'SBAS, Half Circle
                g.FillPie(trailcolor, satx - 10, saty - 5, 20, 20, 180, 180)
            ElseIf prn >= 38 And prn <= 61 Then 'Glonass, Square
                g.FillRectangle(trailcolor, satx - 6, saty - 6, 12, 12)
            Else 'Other, Triangle
                Dim pts() As Point = {New Point(satx, saty - 7), New Point(satx + 7, saty + 7), New Point(satx - 7, saty + 7)}
                g.FillPolygon(trailcolor, pts)
            End If
        Next
        trailcolor = New SolidBrush(Color.FromArgb(140, 140, 140))
        For i = 0 To UBound(SatLog3, 2)
            prn = SatLog3(2, i)
            elevation = 90 - SatLog3(0, i)
            azimuth = (SatLog3(1, i) - 90) Mod 360
            satx = CInt(xc + elevation * Math.Cos(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            saty = CInt(yc + elevation * Math.Sin(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            'g.FillEllipse(trailcolor, satx - 4, saty - 4, 8, 8)
            If prn <= 32 Then 'GPS, Circle
                g.FillEllipse(trailcolor, satx - 7, saty - 7, 14, 14)
            ElseIf prn >= 120 Then 'SBAS, Half Circle
                g.FillPie(trailcolor, satx - 10, saty - 5, 20, 20, 180, 180)
            ElseIf prn >= 38 And prn <= 61 Then 'Glonass, Square
                g.FillRectangle(trailcolor, satx - 6, saty - 6, 12, 12)
            Else 'Other, Triangle
                Dim pts() As Point = {New Point(satx, saty - 7), New Point(satx + 7, saty + 7), New Point(satx - 7, saty + 7)}
                g.FillPolygon(trailcolor, pts)
            End If
        Next
        trailcolor = New SolidBrush(Color.FromArgb(120, 120, 120))
        For i = 0 To UBound(SatLog2, 2)
            prn = SatLog2(2, i)
            elevation = 90 - SatLog2(0, i)
            azimuth = (SatLog2(1, i) - 90) Mod 360
            satx = CInt(xc + elevation * Math.Cos(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            saty = CInt(yc + elevation * Math.Sin(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            'g.FillEllipse(trailcolor, satx - 4, saty - 4, 8, 8)
            If prn <= 32 Then 'GPS, Circle
                g.FillEllipse(trailcolor, satx - 7, saty - 7, 14, 14)
            ElseIf prn >= 120 Then 'SBAS, Half Circle
                g.FillPie(trailcolor, satx - 10, saty - 5, 20, 20, 180, 180)
            ElseIf prn >= 38 And prn <= 61 Then 'Glonass, Square
                g.FillRectangle(trailcolor, satx - 6, saty - 6, 12, 12)
            Else 'Other, Triangle
                Dim pts() As Point = {New Point(satx, saty - 7), New Point(satx + 7, saty + 7), New Point(satx - 7, saty + 7)}
                g.FillPolygon(trailcolor, pts)
            End If
        Next
        trailcolor = New SolidBrush(Color.FromArgb(100, 100, 100))
        For i = 0 To UBound(SatLog1, 2)
            prn = SatLog1(2, i)
            elevation = 90 - SatLog1(0, i)
            azimuth = (SatLog1(1, i) - 90) Mod 360
            satx = CInt(xc + elevation * Math.Cos(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            saty = CInt(yc + elevation * Math.Sin(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
            'g.FillEllipse(trailcolor, satx - 4, saty - 4, 8, 8)
            If prn <= 32 Then 'GPS, Circle
                g.FillEllipse(trailcolor, satx - 7, saty - 7, 14, 14)
            ElseIf prn >= 120 Then 'SBAS, Half Circle
                g.FillPie(trailcolor, satx - 10, saty - 5, 20, 20, 180, 180)
            ElseIf prn >= 38 And prn <= 61 Then 'Glonass, Square
                g.FillRectangle(trailcolor, satx - 6, saty - 6, 12, 12)
            Else 'Other, Triangle
                Dim pts() As Point = {New Point(satx, saty - 7), New Point(satx + 7, saty + 7), New Point(satx - 7, saty + 7)}
                g.FillPolygon(trailcolor, pts)
            End If
        Next


        For line As Integer = 0 To dtsats.Rows.Count - 1
            prn = dtsats.Rows(line).Item(0)
            elevation = dtsats.Rows(line).Item(1)
            azimuth = dtsats.Rows(line).Item(2)
            snr = dtsats.Rows(line).Item(3)
            lastupdate = dtsats.Rows(line).Item(4)

            status = status & vbCrLf & prn.ToString & ": " & snr.ToString

            Dim satcolor As Brush
            If DateDiff(DateInterval.Second, lastupdate, Now) < 300 Then
                elevation = 90 - elevation
                azimuth = (azimuth - 90) Mod 360
                satx = CInt(xc + elevation * Math.Cos(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))
                saty = CInt(yc + elevation * Math.Sin(azimuth * Math.PI / 180) / 90 * (w / 2 - 8))

                Select Case snr
                    Case Is < 15
                        satcolor = Brushes.DarkRed
                    Case Is < 30
                        satcolor = Brushes.Yellow
                    Case Else
                        satcolor = Brushes.Green
                End Select

                'PRNs 1-32 = GPS, 38-61 = GLONASS, 120-138 = SBAS
                If prn <= 32 Then 'GPS, Circle
                    g.FillEllipse(satcolor, satx - 7, saty - 7, 14, 14)
                ElseIf prn >= 120 Then 'SBAS, Half Circle
                    g.FillPie(satcolor, satx - 10, saty - 5, 20, 20, 180, 180)
                ElseIf prn >= 38 And prn <= 61 Then 'Glonass, Square
                    g.FillRectangle(satcolor, satx - 6, saty - 6, 12, 12)
                Else 'Other, Triangle
                    Dim pts() As Point = {New Point(satx, saty - 7), New Point(satx + 7, saty + 7), New Point(satx - 7, saty + 7)}
                    g.FillPolygon(satcolor, pts)
                End If

                Dim stringsize As SizeF = g.MeasureString(prn, myFont)
                g.DrawString(prn, myFont, satcolor, satx - Int(stringsize.Width / 2), saty + 8)
            End If
        Next

        g.Dispose()

        lblPrnSignal.Text = status
    End Sub



    Private Sub boxLogForMinutes_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles boxLogForMinutes.SelectionChangeCommitted
        Select Case boxLogForMinutes.SelectedIndex
            Case 1
                LogDuration = 10
            Case 2
                LogDuration = 60
            Case 3
                LogDuration = 120
            Case 4
                LogDuration = 240
            Case 5
                LogDuration = 360
            Case 6
                LogDuration = 720
            Case 7
                LogDuration = 1440
            Case 8
                LogDuration = 2160
            Case 9
                LogDuration = 2880
            Case 10
                LogDuration = 4320
            Case 11
                LogDuration = 99999 'rolls ever 24 hours, should never hit this limit
            Case Else
                LogDuration = 1
        End Select

        If LogDuration = 99999 Then
            lblEndTime.Text = "End Time: New day (Greenwich Mean Time)"
        Else
            lblEndTime.Text = "End Time: " & DateAdd(DateInterval.Minute, LogDuration, GPS.InitialLoggedTime).ToString
        End If
    End Sub


    Private Sub btnStartRecording_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartRecording.Click
        SaveSetting("Session Label", tbSessionLabel.Text)
        StartRecording()
    End Sub
    Private Sub btnStopRecording_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopRecording.Click
        StopRecording()
    End Sub
    Public Sub StartRecording()
        tbSessionLabel.Enabled = False
        lblStatusBar.Text = "Waiting for data..."
        Me.Text = tbSessionLabel.Text
        RecordedLineCount = 0
        IsRecording = True
        tbSessionLabel.Enabled = False
        boxDataToLog.Enabled = False
        btnStartRecording.Enabled = False
        btnStopRecording.Enabled = True
    End Sub
    Public Sub StopRecording()
        If GPS.RecordQueue.Length > 0 Then
            GPS.WriteRecordingQueueToFile()
        End If

        tbSessionLabel.Enabled = True
        lblStatusBar.Text = ""
        Me.Text = "Lefebure GPS Capture"
        IsRecording = False
        tbSessionLabel.Enabled = True
        boxDataToLog.Enabled = True
        btnStartRecording.Enabled = True
        btnStopRecording.Enabled = False
        lblCurrentFile.Visible = False
        lblStartTime.Visible = False
        lblEndTime.Visible = False
    End Sub

#Region "TextBox Clipboard"
    Private Sub LastLineTextBox_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles LastLineTextBox.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then

            Dim Contextmenu1 As New ContextMenu
            'Dim menuItem1Cut As New MenuItem("Cut")
            'AddHandler menuItem1Cut.Click, AddressOf menuItem1Cut_Click
            Dim menuItem2Copy As New MenuItem("To ClipBoard")
            AddHandler menuItem2Copy.Click, AddressOf CopyAction
            Dim menuItem3Paste As New MenuItem("Re-Execute Command")
            AddHandler menuItem3Paste.Click, AddressOf PasteAction
            'Contextmenu1.MenuItems.Add(menuItem1Cut)
            Contextmenu1.MenuItems.Add(menuItem2Copy)
            Contextmenu1.MenuItems.Add(menuItem3Paste)

            LastLineTextBox.ContextMenu = Contextmenu1

        End If
    End Sub

    'Private Sub CutAction(sender As Object, e As EventArgs)
    '    txtBoxDebugLog.Cut()
    'End Sub

    Private Sub CopyAction(sender As Object, e As EventArgs)
        'Dim objGraphics As Graphics
        'Clipboard.SetData(DataFormats.Rtf, txtBoxDebugLog.SelectedText)
        'Clipboard.Clear()
        LastLineTextBox.Copy()
    End Sub

    Private Sub PasteAction(sender As Object, e As EventArgs)
        If Clipboard.ContainsText(TextDataFormat.Rtf) Then
            'If IsNothing(SDK) = False Then
            '    SDK.execute(Clipboard.GetData(DataFormats.Rtf).ToString())
            'End If
        End If
        'txtBoxDebugLog.Paste()
    End Sub
#End Region


End Class
