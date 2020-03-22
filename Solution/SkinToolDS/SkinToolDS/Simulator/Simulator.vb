
' this code is open source and based on this link http://lefebure.com/software/gpsutils/

Public Class Simulator
    Public myUCGenSet As GenSet = New GenSet
    Public myUCFileSet As FileSet = New FileSet

    Dim WithEvents COMPort As New System.IO.Ports.SerialPort
    Public Delegate Sub StringSubPointer(ByVal Buffer As String)
    Dim timeriteration As Integer = -1
    Dim maxlogentries As Integer = 11
    Dim currentlogentries As Integer = 0
    Dim generatedgpgsvnumber As Integer = 0
    Dim TotalBytesIn As Integer = 0

    Public LastTurnDirection As Integer = -1
    Public AutoTurnEnabled As Boolean = False
    Public AutoTurnStraightCounter As Integer = 0
    Public AutoTurnStraightTarget As Integer = 30

    Public Shared RMCHz As Integer = 5
    Public Shared DataFile As String = ""

    Public ReadFileThread As Threading.Thread
    Public Shared AbortReadFileThread As Boolean = False
    Public Shared FileWaitDelimiter As String = "$GPGGA"


    Public Sub OpenMySerialPort(ByVal portname As String, ByVal portspeed As Integer)
        If COMPort.IsOpen Then
            COMPort.RtsEnable = False
            COMPort.DtrEnable = False
            COMPort.Close()
            Application.DoEvents()
            System.Threading.Thread.Sleep(200)
        End If
        TotalBytesIn = 0
        lblBytesReceived.Text = "0"

        COMPort.PortName = portname
        COMPort.BaudRate = portspeed
        COMPort.WriteTimeout = 2000
        COMPort.ReadTimeout = 2000 'Set timeout to 2 seconds.
        COMPort.NewLine = Chr(13)
        Try
            COMPort.Open()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        If COMPort.IsOpen Then
            COMPort.DiscardInBuffer()
            COMPort.DiscardOutBuffer()
            COMPort.RtsEnable = True
            COMPort.DtrEnable = True

            'Kick start the serial port so it starts reading data.
            'COMPort.BreakState = True
            'System.Threading.Thread.Sleep((11000 / COMPort.BaudRate) + 2) ' Min. 11 bit delay (startbit, 8 data bits, parity bit, stopbit)
            'COMPort.BreakState = False
        End If
    End Sub
    Private Sub SerialInput(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles COMPort.DataReceived
        'COMPort.DiscardInBuffer()
        Try
            Dim DataIn As String = COMPort.ReadExisting
            SendSerialByteCountToUIThread(DataIn.Length)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub SendSerialByteCountToUIThread(ByVal NBytes As Integer)
        Try
            Dim uidel As New SendSerialByteCountToUIThreadDelegate(AddressOf CallBackSerialByteCounttoUIThread)
            Dim o(0) As Object
            o(0) = NBytes
            Invoke(uidel, o)
        Catch ex As Exception
        End Try
    End Sub
    Delegate Sub SendSerialByteCountToUIThreadDelegate(ByVal NBytes As Integer)
    Private Sub CallBackSerialByteCounttoUIThread(ByVal NBytes As Integer)
        TotalBytesIn += NBytes
        lblBytesReceived.Text = TotalBytesIn.ToString("#,#,#,0")
    End Sub
    Public Sub CloseMySerialPort()
        StopReadingFile()
        Try
            If COMPort.IsOpen Then
                'MsgBox("Was Open")
            End If
            COMPort.Close()
        Catch ex As Exception
        End Try
    End Sub

    Public Sub SendLineToSerialPort(ByVal line As String)
        'Send the data to the serial port
        Try
            COMPort.Write(line & vbCrLf)
        Catch ex As Exception
            btnDisconnect.PerformClick()
            MsgBox(ex.Message)
        End Try


        'Log the data, if the log isn't full
        If currentlogentries >= maxlogentries Then
            'Don't do anything, because the log is full
        Else
            If currentlogentries = 0 Then
                lblLog.Text = line
            Else
                lblLog.Text = lblLog.Text & vbCrLf & line
            End If
            currentlogentries = currentlogentries + 1
        End If

    End Sub

    Public Sub CalculateNewLocation()
        'Get variables from the form
        Dim lat As Double = Double.Parse(myUCGenSet.tbLat.Text)
        Dim lon As Double = Double.Parse(myUCGenSet.tbLon.Text)
        Dim speed As Double = Double.Parse(myUCGenSet.tbSpeed.Text)
        Dim targetheading As Integer = Integer.Parse(myUCGenSet.tbTHeading.Text)
        Dim currentheading As Integer = Integer.Parse(myUCGenSet.tbCHeading.Text)
        Dim newlon As Double = 0
        Dim newlat As Double = 0


        'If moving, then update location and if turning, update current heading.
        If myUCGenSet.lblIsMoving.Text = "Yes" Then
            'If target heading not equal to heading, turn heading 1 degree closer on every tick.
            If Not currentheading = targetheading Then
                AutoTurnStraightCounter = 0 'Keep reseting this if turning

                If targetheading < currentheading Then
                    If (currentheading - targetheading) < 180 Then
                        'turn left
                        currentheading = (currentheading + 359) Mod 360
                    Else
                        'turn right
                        currentheading = (currentheading + 1) Mod 360
                    End If
                Else
                    If (targetheading - currentheading) < 180 Then
                        'turn right
                        currentheading = (currentheading + 1) Mod 360
                    Else
                        'turn left
                        currentheading = (currentheading + 359) Mod 360
                    End If
                End If

                myUCGenSet.tbCHeading.Text = currentheading
            End If


            'Calculate new lon/lat using the current heading and speed
            Dim distanceinradians As Double = ((speed / 3956) / 36000) 'There are 3600 seconds per hour, and 10 ticks per second. This is radians moved per tick of the timer.
            Dim tc As Double = (currentheading / 180) * Math.PI
            'Convert degrees to radians
            lat = CDbl(lat / 180 * Math.PI)
            lon = CDbl(lon / 180 * Math.PI)
            newlat = Math.Asin(Math.Sin(lat) * Math.Cos(distanceinradians) + Math.Cos(lat) * Math.Sin(distanceinradians) * Math.Cos(tc))
            newlon = lon + Math.Atan2(Math.Sin(tc) * Math.Sin(distanceinradians) * Math.Cos(lat), Math.Cos(distanceinradians) - Math.Sin(lat) * Math.Sin(newlat))
            'Convert radians to degrees
            newlat = CDbl(newlat / Math.PI * 180)
            newlon = CDbl(newlon / Math.PI * 180)

            newlon = Math.Round(newlon, 10)
            newlat = Math.Round(newlat, 10)


            'Update the form with new lon/lat
            myUCGenSet.tbLon.Text = newlon.ToString
            myUCGenSet.tbLat.Text = newlat.ToString
        End If
    End Sub
    Public Sub GenerateGPRMCcode()
        Dim newlat As Double = Double.Parse(myUCGenSet.tbLat.Text)
        Dim newlon As Double = Double.Parse(myUCGenSet.tbLon.Text)
        Dim speed As Double = Double.Parse(myUCGenSet.tbSpeed.Text)
        Dim currentheading As Integer = Integer.Parse(myUCGenSet.tbCHeading.Text)

        Dim UTCTime As Date = Date.UtcNow

        Dim posnum As Double = 0
        Dim minutes As Double = 0
        Dim mycode As String = "GPRMC,"
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
        mycode = mycode & ",A,"

        posnum = Math.Abs(newlat)
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

        If newlat > 0 Then
            mycode = mycode & ",N,"
        Else
            mycode = mycode & ",S,"
        End If

        posnum = Math.Abs(newlon)
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

        If newlon > 0 Then
            mycode = mycode & ",E,"
        Else
            mycode = mycode & ",W,"
        End If
        If myUCGenSet.lblIsMoving.Text = "Yes" Then
            'If moving, send speed info
            mycode = mycode & Math.Round((speed * 0.868976242), 3).ToString
        Else
            'If not moving, send a speed of 0
            mycode = mycode & "0"
        End If
        mycode = mycode & ","
        mycode = mycode & currentheading.ToString
        mycode = mycode & ","
        If DateAndTime.Day(UTCTime) < "10" Then
            mycode = mycode & "0"
        End If
        mycode = mycode & DateAndTime.Day(UTCTime)
        If DateAndTime.Month(UTCTime) < "10" Then
            mycode = mycode & "0"
        End If
        mycode = mycode & DateAndTime.Month(UTCTime)
        mycode = mycode & Mid(DateAndTime.Year(UTCTime), 3, 2)
        'mycode = mycode & Right(DateAndTime.Year(UTCTime), 2)
        mycode = mycode & ",0,W" 'I don't know what magnetic variation does, so set as 0

        mycode = "$" & mycode & "*" & CalculateChecksum(mycode)   'Add checksum data
        SendLineToSerialPort(mycode)

        'Generate NMEA Code
        '$GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A
        'RMC          Recommended Minimum sentence C
        '123519       Fix taken at 12:35:19 UTC
        'A            Status A=active or V=Void.
        '4807.038,N   Latitude 48 deg 07.038' N
        '01131.000,E  Longitude 11 deg 31.000' E
        '022.4        Speed over the ground in knots
        '084.4        Track angle in degrees True
        '230394       Date - 23rd of March 1994
        '003.1,W      Magnetic Variation
        '*6A          The checksum data, always begins with *

        GenerateGPGGAcode()
        GenerateGPGSAcode()
        GenerateGPGSVcode()
    End Sub
    Public Sub GenerateGPGGAcode()
        Dim newlat As Double = Double.Parse(myUCGenSet.tbLat.Text)
        Dim newlon As Double = Double.Parse(myUCGenSet.tbLon.Text)
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
        posnum = Math.Abs(newlat)
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

        If newlat > 0 Then
            mycode = mycode & ",N,"
        Else
            mycode = mycode & ",S,"
        End If

        posnum = Math.Abs(newlon)
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

        If newlon > 0 Then
            mycode = mycode & ",E,"
        Else
            mycode = mycode & ",W,"
        End If

        Select Case myUCGenSet.boxPositionType.SelectedIndex
            Case 1 '1 - Plain GPS (No Differential)
                mycode = mycode & "1"
            Case 2 '2 - Differential GPS
                mycode = mycode & "2"
            Case 3 '4 - RTK
                mycode = mycode & "4"
            Case 4 '5 - Float RTK
                mycode = mycode & "5"
            Case 5 '8 - Simulation
                mycode = mycode & "8"
            Case Else '0 - Invalid Position
                mycode = mycode & "0"
        End Select

        mycode = mycode & ",08,1.1,"

        'Add elevation data
        If IsNumeric(myUCGenSet.tbElevation.Text) Then
            mycode = mycode & Format(CDec(myUCGenSet.tbElevation.Text), "0.00")
        Else
            'Not numeric, just set it to zero
            mycode = mycode & "0.0"
        End If


        mycode = mycode & ",M,-32.6,M,"

        mycode = mycode & Format(Second(Now) + 1, "00") & ",0000" 'This is correction age and station ID

        mycode = "$" & mycode & "*" & CalculateChecksum(mycode)   'Add checksum data
        SendLineToSerialPort(mycode)
    End Sub
    Public Sub GenerateGPGSAcode()
        Dim mycode As String = "GPGSA,"
        mycode = mycode & "A,3,01,05,,,,18,,22,30,31,48,51,2.5,1.1,1.9"
        mycode = "$" & mycode & "*" & CalculateChecksum(mycode)   'Add checksum data
        SendLineToSerialPort(mycode)
    End Sub
    Public Sub GenerateGPGSVcode()
        Dim mycode As String = "GPGSV,"

        Select Case generatedgpgsvnumber
            Case 0
                mycode = mycode & "3,1,12,01,25,250,30,05,25,230,30,11,25,288,00,12,25,048,30"
                generatedgpgsvnumber += 1
            Case 1
                mycode = mycode & "3,2,12,14,25,055,00,18,25,210,30,20,25,314,00,22,25,190,30"
                generatedgpgsvnumber += 1
            Case Else
                mycode = mycode & "3,3,12,30,25,170,30,31,25,150,30,48,25,130,30,51,25,110,30"
                generatedgpgsvnumber = 0
        End Select

        mycode = "$" & mycode & "*" & CalculateChecksum(mycode)   'Add checksum data
        SendLineToSerialPort(mycode)
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
    Private Function ACOSH(ByVal x As Double)
        Dim result As Double = 0
        result = Math.Log(x + Math.Sqrt(x * x - 1))
        Return result
    End Function



    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'This sets the decimal delimiter for users outside the US using strange number formating settings. This resolves the bug in IsNumeric()
        'http://www.codeproject.com/KB/vb/isnumeric.aspx
        Dim ci As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US", False)
        ci.NumberFormat.CurrencyDecimalSeparator = ","
        ci.NumberFormat.CurrencyGroupSeparator = "."
        Threading.Thread.CurrentThread.CurrentCulture = ci

        lblVersion.Text = "Version: " & My.Application.Info.Version.Major & "." & Format(My.Application.Info.Version.Minor, "00") & "." & Format(My.Application.Info.Version.Build, "00")

        Dim foundsomeports As Boolean = False

        For Each portName As String In My.Computer.Ports.SerialPortNames
            Dim portNumberChars() As Char = portName.Substring(3).ToCharArray() 'Remove "COM", put the rest in a character array
            portName = "COM" 'Start over with "COM"
            For Each portNumberChar As Char In portNumberChars
                If Char.IsDigit(portNumberChar) Then 'Good character, append to portName
                    portName += portNumberChar.ToString()
                End If
            Next
            boxSerialPorts.Items.Add(portName)
            foundsomeports = True
        Next
        If foundsomeports Then
            boxSerialPorts.SelectedIndex = 0
        Else
            MsgBox("No Serial Ports were found in your system.")
            btnConnect.Enabled = False
            btnConnect.BackColor = Color.DarkGray
        End If

        boxDataSource.SelectedIndex = 0
        boxRMCHertz.SelectedIndex = 2
        boxSerialSpeed.SelectedIndex = 4
        myUCGenSet.boxPositionType.SelectedIndex = 5
        Panel1.Controls.Add(myUCGenSet)

        lblLog.Text = lblLog.Text & vbCrLf & vbCrLf & vbCrLf & "This program generates GPRMC, GPGGA, GPGSA, and GPGSV lines like a GPS antenna would."
        lblLog.Text = lblLog.Text & vbCrLf & vbCrLf & "How to use: Run this on one computer, which is connected to another computer via a null-modem cable."
        lblLog.Text = lblLog.Text & vbCrLf & "If your receiving app is on this computer, you can use null-modem emulator software such as ""com0com""."
        lblLog.Text = lblLog.Text & vbCrLf & "If you are not sure if this program is sending data, set up hyperterminal on the other end of the link."

        lblLog.Text = lblLog.Text & vbCrLf & vbCrLf & "Heading changes 1 degree per tick of the timer, which runs at 10Hz."

        myUCGenSet.tbLat.Text = "46.0"
        myUCGenSet.tbLon.Text = "-95.0"

        myUCFileSet.boxDelimiter.SelectedIndex = 0
        myUCFileSet.boxEOF.SelectedIndex = 0
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        StopReadingFile()
        If e.CloseReason = CloseReason.UserClosing Then
            'cancel the close
            e.Cancel = True
            Me.Hide()
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'Every 10 tics (1 second), log the data point, if it has changed since last second.
        timeriteration += 1
        If timeriteration = 10 Then timeriteration = 0


        'Check if we need to auto turn
        Dim speed As Double = Double.Parse(myUCGenSet.tbSpeed.Text)
        If AutoTurnEnabled Then
            If speed > 0 And myUCGenSet.lblIsMoving.Text = "Yes" Then
                AutoTurnStraightCounter += 1
                If AutoTurnStraightCounter > AutoTurnStraightTarget * 10 Then
                    AutoTurnStraightCounter = 0 'Reset this back to zero
                    'Issue a turn
                    If LastTurnDirection = 1 Then
                        myUCGenSet.btnTurnLeft90.PerformClick()
                        myUCGenSet.btnTurnLeft90.PerformClick()
                    Else
                        myUCGenSet.btnTurnRight90.PerformClick()
                        myUCGenSet.btnTurnRight90.PerformClick()
                    End If
                End If
            End If
        End If



        'Every tick, calculate new location.
        CalculateNewLocation()

        Select Case RMCHz
            Case 1
                If timeriteration = 0 Then GenerateGPRMCcode()
            Case 2
                If timeriteration = 0 Then GenerateGPRMCcode()
                If timeriteration = 5 Then GenerateGPRMCcode()
            Case 5
                If timeriteration = 0 Then GenerateGPRMCcode()
                If timeriteration = 2 Then GenerateGPRMCcode()
                If timeriteration = 4 Then GenerateGPRMCcode()
                If timeriteration = 6 Then GenerateGPRMCcode()
                If timeriteration = 8 Then GenerateGPRMCcode()
            Case Else '10hz, generate on every tick.
                GenerateGPRMCcode()
        End Select

        'GGA, GSA, and GSV are generated every time RMC is. Called right from the RMC subroutine.
    End Sub

    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        Dim failed As Boolean = False

        If boxDataSource.SelectedIndex = 0 Then 'Generated
            If IsNumeric(myUCGenSet.tbSpeed.Text) Then
                Dim speed As Double = CDbl(myUCGenSet.tbSpeed.Text)
                If speed < -10 Then
                    failed = True
                    MsgBox("Speed is out of range. Needs to be more than -10.")
                End If
            Else
                failed = True
                MsgBox("Speed is not a number")
            End If

            If IsNumeric(myUCGenSet.tbTHeading.Text) Then
                Dim theading As Double = CDbl(myUCGenSet.tbTHeading.Text)
                If Not theading = CInt(theading) Then
                    failed = True
                    MsgBox("Target Heading is not an integer.")
                Else
                    If theading < 0 Or theading > 359 Then
                        failed = True
                        MsgBox("Target Heading is out of range. Needs to be between 0 and 359.")
                    End If
                End If
            Else
                failed = True
                MsgBox("Target Heading is not a number")
            End If

            If IsNumeric(myUCGenSet.tbCHeading.Text) Then
                Dim cheading As Double = CDbl(myUCGenSet.tbCHeading.Text)
                If Not cheading = CInt(cheading) Then
                    failed = True
                    MsgBox("Current Heading is not an integer.")
                Else
                    If cheading < 0 Or cheading > 359 Then
                        failed = True
                        MsgBox("Current Heading is out of range. Needs to be between 0 and 359.")
                    End If
                End If
            Else
                failed = True
                MsgBox("Current Heading is not a number")
            End If

            If IsNumeric(myUCGenSet.tbLat.Text) Then
                If CDbl(myUCGenSet.tbLat.Text) >= 90 Or CDbl(myUCGenSet.tbLat.Text) <= -90 Then
                    failed = True
                    MsgBox("Latitude is out of range. Needs to be between 90 and -90.")
                End If
            Else
                failed = True
                MsgBox("Latitude is not a number")
            End If

            If IsNumeric(myUCGenSet.tbLon.Text) Then
                If CDbl(myUCGenSet.tbLon.Text) >= 180 Or CDbl(myUCGenSet.tbLon.Text) <= -180 Then
                    failed = True
                    MsgBox("Longitude is out of range. Needs to be between 180 and -180.")
                End If
            Else
                failed = True
                MsgBox("Latitude is not a number")
            End If

            If IsNumeric(myUCGenSet.tbElevation.Text) Then
                Dim elev As Double = CDbl(myUCGenSet.tbElevation.Text)
                If elev < 0 Then
                    failed = True
                    MsgBox("Elevation is out of range. Can't be less than 0.")
                End If
            Else
                failed = True
                MsgBox("Elevation is not a number")
            End If

            If failed Then Exit Sub

        Else 'From File
            'Nothing to check before opening the connection.
        End If

        OpenMySerialPort(boxSerialPorts.SelectedItem, CInt(boxSerialSpeed.SelectedItem))

        If boxDataSource.SelectedIndex = 0 Then
            Timer1.Start()
        Else
            'If file is selected, start thread
            If DataFile.Length > 1 Then
                StartReadingFile()
            End If
        End If


        btnConnect.Enabled = False
        btnDisconnect.Enabled = True

        boxSerialPorts.Enabled = False
        boxSerialSpeed.Enabled = False
        boxDataSource.Enabled = False
        If boxDataSource.SelectedIndex = 0 Then
            boxRMCHertz.Enabled = False
        End If
        myUCGenSet.tbSpeed.Enabled = False
        myUCGenSet.tbTHeading.Enabled = False
        myUCGenSet.tbLat.Enabled = False
        myUCGenSet.tbLon.Enabled = False
        myUCGenSet.tbElevation.Enabled = False

        If currentlogentries >= maxlogentries Then
            currentlogentries = 0
        End If
    End Sub
    Private Sub btnDisconnect_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisconnect.Click
        CloseMySerialPort()
        Timer1.Stop()

        btnConnect.Enabled = True
        btnDisconnect.Enabled = False

        boxSerialPorts.Enabled = True
        boxSerialSpeed.Enabled = True
        boxDataSource.Enabled = True
        boxRMCHertz.Enabled = True
        myUCGenSet.tbSpeed.Enabled = True
        myUCGenSet.tbTHeading.Enabled = True
        myUCGenSet.tbLat.Enabled = True
        myUCGenSet.tbLon.Enabled = True
        myUCGenSet.tbElevation.Enabled = True
        myUCGenSet.lblIsMoving.Text = "No"
    End Sub




    Private Sub btnClearLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearLog.Click
        currentlogentries = 0
    End Sub




    Private Sub boxSerialSpeed_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles boxSerialSpeed.SelectedIndexChanged
        Select Case boxSerialSpeed.SelectedIndex
            Case 0
                '4800 baud, need 1Hz or less
                If boxRMCHertz.SelectedIndex > 0 Then boxRMCHertz.SelectedIndex = 0
            Case 1
                '9600 baud, need 2Hz or less
                If boxRMCHertz.SelectedIndex > 1 Then boxRMCHertz.SelectedIndex = 1
            Case 2
                '14400 baud, need 2Hz or less
                If boxRMCHertz.SelectedIndex > 1 Then boxRMCHertz.SelectedIndex = 1
            Case 3
                '19200 baud, need 5Hz or less
                If boxRMCHertz.SelectedIndex > 2 Then boxRMCHertz.SelectedIndex = 2
        End Select
    End Sub

    Private Sub boxHertz_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles boxRMCHertz.SelectedIndexChanged
        Select Case boxRMCHertz.SelectedIndex
            Case 0
                '1 hz
                RMCHz = 1
            Case 1
                '2Hz, need 9600 baud or more
                RMCHz = 2
                If boxSerialSpeed.SelectedIndex < 1 Then boxSerialSpeed.SelectedIndex = 1
            Case 2
                '5Hz, need 19200 baud or more
                RMCHz = 5
                If boxSerialSpeed.SelectedIndex < 3 Then boxSerialSpeed.SelectedIndex = 3
            Case 3
                '10Hz, need 38400 baud or more
                RMCHz = 10
                If boxSerialSpeed.SelectedIndex < 4 Then boxSerialSpeed.SelectedIndex = 4
        End Select

    End Sub

    Private Sub boxDataSource_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles boxDataSource.SelectionChangeCommitted
        Panel1.Controls.Clear()
        If boxDataSource.SelectedIndex = 0 Then
            Panel1.Controls.Add(myUCGenSet)
        Else
            Panel1.Controls.Add(myUCFileSet)
        End If
    End Sub





    Public Sub StartReadingFile()
        If Not ReadFileThread Is Nothing Then
            If ReadFileThread.IsAlive Then
                StopReadingFile()
                Exit Sub
            End If
        End If

        AbortReadFileThread = False
        ReadFileThread = New Threading.Thread(AddressOf ReadFileLoop)
        ReadFileThread.Start()

        myUCFileSet.btnStopReadingFile.Visible = True
    End Sub
    Public Sub StopReadingFile()
        AbortReadFileThread = True
        'Wait for the thread to notice the change and stop.
        Threading.Thread.Sleep(100)
        Application.DoEvents()
        Threading.Thread.Sleep(100)
        Application.DoEvents()

        'Ok, kill the thread if it is still running.
        If Not ReadFileThread Is Nothing Then
            If ReadFileThread.IsAlive Then
                ReadFileThread.Abort()
                Threading.Thread.Sleep(100)
                Application.DoEvents()
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            End If
        End If

        myUCFileSet.btnStopReadingFile.Visible = False
    End Sub
    Private Sub ReadFileLoop()
        Threading.Thread.Sleep(100)
        Dim LineCount As Integer = 0
        ReadFileUpdateUIThread(0, "")
        Dim WaitCount As Integer = 0

        If Not My.Computer.FileSystem.FileExists(DataFile) Then
            ReadFileUpdateUIThread(100, "File doesn't Exist.")
            ReadFileThread.Abort()
        End If

        Try
            Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(DataFile)

            Dim LastLine As String = ""

            While oRead.Peek <> -1
                If WaitCount = 0 Then
                    For i = 0 To 50
                        ReadFileUpdateUIThread(1, LastLine)
                        LineCount += 1
                        LastLine = Trim(oRead.ReadLine)
                        If LastLine.Contains(FileWaitDelimiter) Then
                            Exit For
                        End If

                        If Not oRead.Peek <> -1 Then
                            Exit For
                        End If

                        If i = 50 Then
                            ReadFileUpdateUIThread(100, "Delimiter not found for last 50 lines at line " & LineCount)
                            ReadFileThread.Abort()
                        End If
                    Next

                    'Determine wait time
                    Select Case RMCHz
                        Case 1
                            WaitCount = 9
                        Case 2
                            WaitCount = 4
                        Case 5
                            WaitCount = 1
                        Case Else '10
                            'Leave waitcount at 0
                    End Select
                Else
                    WaitCount -= 1
                End If

                If AbortReadFileThread Then
                    ReadFileUpdateUIThread(100, "Thread had to abort.")
                    ReadFileThread.Abort()
                End If

                Threading.Thread.Sleep(100)
            End While
            oRead.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


        ReadFileUpdateUIThread(101, "")
        ReadFileThread.Abort()
    End Sub
    Private Sub ReadFileUpdateUIThread(ByVal Item As Integer, ByVal Value As String)
        Try
            Dim uidel As New ReadFileUpdateUIThreadDelegate(AddressOf ReadFileCallBacktoUIThread)
            Dim o(1) As Object
            o(0) = Item
            o(1) = Value
            Invoke(uidel, o)
        Catch ex As Exception
        End Try
    End Sub
    Delegate Sub ReadFileUpdateUIThreadDelegate(ByVal Item As Integer, ByVal Value As String)
    Private Sub ReadFileCallBacktoUIThread(ByVal Item As Integer, ByVal Value As String)
        Select Case Item
            Case 0 'Sending data
                myUCFileSet.lblStatus.Text = "Status: Sending Data"
            Case 1 'Sending a line of data in
                SendLineToSerialPort(Value)
            Case 100 'Thread commited suicide for some reason.
                StopReadingFile()
                myUCFileSet.lblStatus.Text = "Status: Error: " & Value
            Case 101 'Ran out of file
                StopReadingFile()
                If myUCFileSet.boxEOF.SelectedIndex = 1 Then
                    myUCFileSet.lblStatus.Text = "Status: Finished Sending File ... Restarting"
                    StartReadingFile()
                Else
                    myUCFileSet.lblStatus.Text = "Status: Finished Sending File"
                End If
        End Select
    End Sub

End Class
