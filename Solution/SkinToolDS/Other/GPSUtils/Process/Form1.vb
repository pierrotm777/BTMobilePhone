Public Class Form1
    Public GPS As New Process.GPS


    Public dtcoords As New DataTable
    Dim BaseFilePath As String = ""
    Dim BaseFileName As String = ""

    Dim AvgLat As Double = 0
    Dim AvgLon As Double = 0
    Dim AvgAlt As Double = 0
    Dim HLastAvg As Double = 1
    Dim HLast500 As Double = 1
    Dim HLast682 As Double = 1
    Dim HLast955 As Double = 1
    Dim HLast997 As Double = 1
    Dim HLastMax As Double = 1
    Dim VLastAvg As Double = 1
    Dim VLast500 As Double = 1
    Dim VLast682 As Double = 1
    Dim VLast955 As Double = 1
    Dim VLast997 As Double = 1
    Dim VLastMax As Double = 1
    Dim ShortTermPointsFound As Integer = 0

    Dim w As Integer = 1000
    Dim h As Integer = 1000
    Dim TimeDelay As Integer = 900 '15 minutes, short term drift time


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

        If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath) Then
            MsgBox("Error: The Application's folder doesn't exist. Settings file not loaded.")
        End If

        If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\Data") Then
            Try
                My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\Data")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If

        dtcoords.Columns.Add("lat", GetType(Decimal))
        dtcoords.Columns.Add("lon", GetType(Decimal))
        dtcoords.Columns.Add("sigtype", GetType(Integer))
        dtcoords.Columns.Add("hdop", GetType(Decimal))
        dtcoords.Columns.Add("elev", GetType(Decimal))
        dtcoords.Columns.Add("distfromhc", GetType(Double))
        dtcoords.Columns.Add("distfromvc", GetType(Double))
        dtcoords.Columns.Add("distfromx", GetType(Double))
        dtcoords.Columns.Add("distfromy", GetType(Double))
        dtcoords.Columns.Add("distfromz", GetType(Double))
        dtcoords.Columns.Add("rtime", GetType(Integer)) 'run time
        dtcoords.Columns.Add("sats", GetType(Integer))


        Dim lastfile As String = ""
        'If My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\Data") Then
        '    For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\Data")
        '        If Mid(foundFile, InStrRev(foundFile, "\") + 1).Contains(".gps") Then
        '            lastfile = foundFile
        '        End If
        '    Next
        'End If
        If My.Computer.FileSystem.DirectoryExists("W:\Web-WWW\farming\satests") Then
            For Each foundFile As String In My.Computer.FileSystem.GetFiles("W:\Web-WWW\farming\satests")
                If Mid(foundFile, InStrRev(foundFile, "\") + 1).Contains(".gps") Then
                    lastfile = foundFile
                End If
            Next
        End If
        tbFile.Text = lastfile

        tbMaxCorrAge.Text = 10
        boxFixType.SelectedIndex = 2
        boxSatCount.SelectedIndex = 0
    End Sub

 

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim fdlg As OpenFileDialog = New OpenFileDialog()
        fdlg.Title = "GPS File to Process"
        If tbFile.Text.Length > 0 Then
            fdlg.InitialDirectory = tbFile.Text.Substring(0, InStrRev(tbFile.Text, "\"))
            'fdlg. = tbFile.Text



        Else
            fdlg.InitialDirectory = Application.StartupPath & "\Data"
        End If
        fdlg.Filter = "GPS Data Files (*.gps)|*.gps"
        fdlg.FilterIndex = 2
        fdlg.RestoreDirectory = True
        If fdlg.ShowDialog() = DialogResult.OK Then
            tbFile.Text = fdlg.FileName
            lblStatus.Text = ""
        End If
    End Sub



    Private Sub btnProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        If tbFile.Text.Length < 2 Then
            MsgBox("You need to select a file to process.")
            Exit Sub
        End If

        If tbMaxHDOP.Text.Length > 0 Then
            If IsNumeric(tbMaxHDOP.Text) Then
                GPS.MaxHDOP = CDec(tbMaxHDOP.Text)
            Else
                MsgBox("Max HDOP needs to be numeric.")
                Exit Sub
            End If
        End If

        If tbMaxCorrAge.Text.Length > 0 Then
            If IsNumeric(tbMaxCorrAge.Text) Then
                GPS.MaxCorrAge = CDec(tbMaxCorrAge.Text)
            Else
                MsgBox("Max Correction Age needs to be numeric.")
                Exit Sub
            End If
        End If

        Select Case boxFixType.SelectedIndex
            Case 1
                GPS.ReqFixType = 2
            Case 2
                GPS.ReqFixType = 4
            Case Else
                GPS.ReqFixType = 0
        End Select

        Select Case boxSatCount.SelectedIndex
            Case 1 '4
                GPS.MinSatCount = 4
            Case 2
                GPS.MinSatCount = 5
            Case 3
                GPS.MinSatCount = 6
            Case 4
                GPS.MinSatCount = 7
            Case 5
                GPS.MinSatCount = 8
            Case 6
                GPS.MinSatCount = 9
            Case 7
                GPS.MinSatCount = 10
            Case Else 'Any
                GPS.MinSatCount = 0
        End Select

        If tbFile.Text.EndsWith("\") Then
            'process all .gps files in the directory
            If My.Computer.FileSystem.DirectoryExists(tbFile.Text) Then
                For Each FilePath As String In My.Computer.FileSystem.GetFiles(tbFile.Text)
                    If My.Computer.FileSystem.GetFileInfo(FilePath).Extension = ".gps" Then
                        ThingsToDo(FilePath)
                    End If
                Next
            Else
                MsgBox("The path you entered doesn't exist.")
            End If
        Else
            ThingsToDo(tbFile.Text)
        End If
    End Sub
    Private Sub ThingsToDo(ByVal filename As String)
        If Not My.Computer.FileSystem.FileExists(filename) Then
            MsgBox("File not found.")
            Exit Sub
        End If

        lblStatus.Text = "Reading data file... "
        lblStatus.Visible = True
        Application.DoEvents()
        LoadDataFromFile(filename)

        lblStatus.Text += "Done. (Found " & GPS.GGACount & " lines)"
        If dtcoords.Rows.Count = 0 Then Exit Sub 'If no data was found, stop here.
        lblStatus.Text += vbCrLf & "Processing Coordinates... "
        Application.DoEvents()
        ProcessCoords()

        lblStatus.Text += "Done." & vbCrLf & "Creating Path Image... "
        Application.DoEvents()
        GeneratePathImage()

        lblStatus.Text += "Done." & vbCrLf & "Creating X Distribution Image... "
        Application.DoEvents()
        GenerateXImage()

        lblStatus.Text += "Done." & vbCrLf & "Creating Y Distribution Image... "
        Application.DoEvents()
        GenerateYImage()

        lblStatus.Text += "Done." & vbCrLf & "Creating Z Distribution Image... "
        Application.DoEvents()
        GenerateZImage()

        lblStatus.Text += "Done." & vbCrLf & "Creating Satellite Visibility Image... "
        Application.DoEvents()
        GenerateSatVisImage()

        lblStatus.Text += "Done." & vbCrLf & "Processing Short Term Drift Statistics... "
        Application.DoEvents()
        ProcessShortTermDrift()

        If ShortTermPointsFound > 0 Then
            lblStatus.Text += "Done." & vbCrLf & "Creating Short Term Horizontal Drift Image... "
            Application.DoEvents()
            GenerateShortTermHImage()

            lblStatus.Text += "Done." & vbCrLf & "Creating Short Term Vertical Drift Image... "
            Application.DoEvents()
            GenerateShortTermVImage()

            lblStatus.Text += "Done." & vbCrLf & "Processing Short Term East-West Drift Statistics... "
            Application.DoEvents()
            ProcessShortTermXDrift()

            lblStatus.Text += "Done." & vbCrLf & "Creating Short Term East-West Drift Image... "
            Application.DoEvents()
            GenerateShortTermXImage()

            lblStatus.Text += "Done." & vbCrLf & "Processing Short Term North-South Drift Statistics... "
            Application.DoEvents()
            ProcessShortTermYDrift()

            lblStatus.Text += "Done." & vbCrLf & "Creating Short Term North-South Drift Image... "
            Application.DoEvents()
            GenerateShortTermYImage()
        End If

        lblStatus.Text += "Done." & vbCrLf & "Creating HTML File... "
        Application.DoEvents()
        GenerateHTMLFile()

        lblStatus.Text += "Done." & vbCrLf & "Finished."
    End Sub


    Public Sub LoadDataFromFile(ByVal filename As String)
        GPS.GGACount = 0
        GPS.ResetStartTime = True
        dtcoords.Rows.Clear()

        BaseFileName = Replace(Mid(filename, InStrRev(filename, "\") + 1), ".gps", "")
        BaseFilePath = filename.Substring(0, InStrRev(filename, "\"))

        Try
            Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(filename)
            Dim linein

            While oRead.Peek <> -1
                linein = Trim(oRead.ReadLine)
                If Len(linein) < 3 Then
                    'Line is too short
                ElseIf Asc(linein) = 35 Then
                    'Line starts with a #
                Else
                    GPS.ProcessNMEAdata(linein)
                End If
            End While
            oRead.Close()
        Catch ex As Exception
        End Try
    End Sub
    Public Sub ProcessCoords()
        AvgLat = CDbl(dtcoords.Compute("AVG(lat)", ""))
        AvgLon = CDbl(dtcoords.Compute("AVG(lon)", ""))
        AvgAlt = CDbl(dtcoords.Compute("AVG(elev)", ""))
        'lblAvgLat.Text = AvgLat
        'lblAvgLon.Text = AvgLon
        'lblAvgAlt.Text = Format(AvgAlt * 3.2808399, "#.00") & " feet above sea level"

        Dim lat0 As Double = AvgLat / 180 * Math.PI
        Dim lon0 As Double = AvgLon / 180 * Math.PI

        Dim latP As Double
        Dim lonP As Double
        Dim dlat As Double
        Dim dlon As Double
        Dim A As Double
        Dim C As Double
        Dim distancefrom0toP As Double
        Dim AltDistance As Double

        For Each row In dtcoords.Rows
            latP = row("lat") / 180 * Math.PI
            lonP = row("lon") / 180 * Math.PI

            dlat = lat0 - latP
            dlon = lon0 - lonP
            A = ((Math.Sin(dlat / 2)) ^ 2) + Math.Cos(latP) * Math.Cos(lat0) * ((Math.Sin(dlon / 2)) ^ 2)
            C = 2 * Math.Atan2(Math.Sqrt(A), Math.Sqrt(1 - A))
            distancefrom0toP = (3956 * C) * 5280 * 12
            row("distfromhc") = distancefrom0toP

            dlat = 0
            dlon = lon0 - lonP
            A = ((Math.Sin(dlat / 2)) ^ 2) + Math.Cos(latP) * Math.Cos(lat0) * ((Math.Sin(dlon / 2)) ^ 2)
            C = 2 * Math.Atan2(Math.Sqrt(A), Math.Sqrt(1 - A))
            distancefrom0toP = (3956 * C) * 5280 * 12
            If dlon > 0 Then
                row("distfromx") = distancefrom0toP * -1
            Else
                row("distfromx") = distancefrom0toP
            End If

            dlat = lat0 - latP
            dlon = 0
            A = ((Math.Sin(dlat / 2)) ^ 2) + Math.Cos(latP) * Math.Cos(lat0) * ((Math.Sin(dlon / 2)) ^ 2)
            C = 2 * Math.Atan2(Math.Sqrt(A), Math.Sqrt(1 - A))
            distancefrom0toP = (3956 * C) * 5280 * 12
            If dlat > 0 Then
                row("distfromy") = distancefrom0toP * -1
            Else
                row("distfromy") = distancefrom0toP
            End If

            AltDistance = row("elev") - AvgAlt
            row("distfromz") = AltDistance
            If AltDistance < 0 Then AltDistance = AltDistance * -1
            row("distfromvc") = AltDistance
        Next


        '1 = 68.2 (sigmas)
        '2 = 95.5
        '3 = 99.7


        Dim filter As String = ""
        Dim sortby As String = "distfromhc"
        Dim points As DataRow() = dtcoords.Select(filter, sortby)
        Dim rowcount As Integer = points.GetUpperBound(0) + 1

        Dim x500 As Integer = Math.Round(rowcount * 0.5) - 1
        Dim x682 As Integer = Math.Round(rowcount * 0.682) - 1
        Dim x955 As Integer = Math.Round(rowcount * 0.955) - 1
        Dim x997 As Integer = Math.Round(rowcount * 0.997) - 1

        If x500 < 0 Then x500 = 0
        If x682 < 0 Then x682 = 0
        If x955 < 0 Then x955 = 0
        If x997 < 0 Then x997 = 0

        HLastAvg = CDbl(dtcoords.Compute("AVG(distfromhc)", ""))
        HLast500 = points(x500)(5)
        HLast682 = points(x682)(5)
        HLast955 = points(x955)(5)
        HLast997 = points(x997)(5)
        HLastMax = CDbl(dtcoords.Compute("MAX(distfromhc)", ""))






        sortby = "distfromvc"
        Dim Vpoints As DataRow() = dtcoords.Select(filter, sortby)
        Dim Vrowcount As Integer = Vpoints.GetUpperBound(0) + 1

        Dim Vx500 As Integer = Math.Round(Vrowcount * 0.5) - 1
        Dim Vx682 As Integer = Math.Round(Vrowcount * 0.682) - 1
        Dim Vx955 As Integer = Math.Round(Vrowcount * 0.955) - 1
        Dim Vx997 As Integer = Math.Round(Vrowcount * 0.997) - 1

        If Vx500 < 0 Then Vx500 = 0
        If Vx682 < 0 Then Vx682 = 0
        If Vx955 < 0 Then Vx955 = 0
        If Vx997 < 0 Then Vx997 = 0

        VLastAvg = CDbl(dtcoords.Compute("AVG(distfromvc)", "")) * 3.2808399 * 12
        VLast500 = Vpoints(Vx500)(6) * 3.2808399 * 12
        VLast682 = Vpoints(Vx682)(6) * 3.2808399 * 12
        VLast955 = Vpoints(Vx955)(6) * 3.2808399 * 12
        VLast997 = Vpoints(Vx997)(6) * 3.2808399 * 12
        VLastMax = CDbl(dtcoords.Compute("MAX(distfromvc)", "")) * 3.2808399 * 12




        'MsgBox("rowcount=" & rowcount & vbCrLf & "500=" & x500 & vbCrLf & "682=" & x682 & vbCrLf & "955=" & x955 & vbCrLf & "997=" & x997)


        'Dim minuteson As Integer = DateDiff(DateInterval.Minute, GPSForm.InitialLoggedTime, GPSForm.LastLoggedTime)
        'Dim hourson As Integer = Fix(minuteson / 60)
        'minuteson = minuteson Mod 60

        'GPSForm.LogEvent("Logged " & dtcoords.Rows.Count & " points over " & hourson & " hours and " & minuteson & " minutes.")

        'GPSForm.LogEvent("Horizontal Plane: Lat/Lon = " & AvgLat & ", " & AvgLon)
        'GPSForm.LogEvent("- Average: " & Format(HLastAvg, "Fixed") & " inches")
        'GPSForm.LogEvent("- CEP (50%): " & Format(HLast500, "Fixed") & " inches")
        'GPSForm.LogEvent("- Sigma 1 (68.2%): " & Format(HLast682, "Fixed") & " inches")
        'GPSForm.LogEvent("- Sigma 2 (95.5%): " & Format(HLast955, "Fixed") & " inches")
        'GPSForm.LogEvent("- Sigma 3 (99.7%): " & Format(HLast997, "Fixed") & " inches")
        'GPSForm.LogEvent("- Maximum: " & Format(HLastMax, "Fixed") & " inches")

        'GPSForm.LogEvent("Vertical Plane: Elevation = " & AvgAlt)
        'GPSForm.LogEvent("- Average: " & Format(VLastAvg, "Fixed") & " inches")
        'GPSForm.LogEvent("- CEP (50%): " & Format(VLast500, "Fixed") & " inches")
        'GPSForm.LogEvent("- Sigma 1 (68.2%): " & Format(VLast682, "Fixed") & " inches")
        'GPSForm.LogEvent("- Sigma 2 (95.5%): " & Format(VLast955, "Fixed") & " inches")
        'GPSForm.LogEvent("- Sigma 3 (99.7%): " & Format(VLast997, "Fixed") & " inches")
        'GPSForm.LogEvent("- Maximum: " & Format(VLastMax, "Fixed") & " inches")

    End Sub
    Public Sub GeneratePathImage()
        Dim MapCenterLat As Double = AvgLat / 180 * Math.PI
        Dim MapCenterLon As Double = AvgLon / 180 * Math.PI

        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim xc As Integer = Fix(w / 2) - 1
        Dim yc As Integer = Fix(h / 2) - 1
        Dim memoryBitmap As Bitmap = New Bitmap(w, h)
        Dim MapInchesPerPixel As Double = 1
        Dim myFont As New Font("Ariel", 12)

        If HLast997 = 0 Then HLast997 = 10
        If w > h Then
            MapInchesPerPixel = HLast997 / yc
        Else
            MapInchesPerPixel = HLast997 / xc
        End If


        'Wipe the map clear, so we can start over
        Dim g As Graphics = Graphics.FromImage(memoryBitmap)
        g.Clear(Color.White)
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        Dim MyBrush As New SolidBrush(Color.FromArgb(30, 0, 0, 0))

        Dim lat0 As Double = AvgLat / 180 * Math.PI
        Dim lon0 As Double = AvgLon / 180 * Math.PI

        Dim latP As Double
        Dim lonP As Double
        Dim dlat As Double
        Dim dlon As Double
        Dim A As Double
        Dim C As Double
        Dim azx As Double
        Dim azy As Double
        Dim distancefrom0toP As Double
        Dim headingfrom0toP As Double

        Dim px As Integer = 0
        Dim py As Integer = 0

        For Each row In dtcoords.Rows
            latP = row("lat") / 180 * Math.PI
            lonP = row("lon") / 180 * Math.PI

            dlat = lat0 - latP
            dlon = lon0 - lonP
            A = ((Math.Sin(dlat / 2)) ^ 2) + Math.Cos(latP) * Math.Cos(lat0) * ((Math.Sin(dlon / 2)) ^ 2)
            C = 2 * Math.Atan2(Math.Sqrt(A), Math.Sqrt(1 - A))
            distancefrom0toP = (3956 * C) * 5280 * 12
            azx = Math.Sin(lon0 - lonP) * Math.Cos(latP)
            azy = Math.Cos(lat0) * Math.Sin(latP) - Math.Sin(lat0) * Math.Cos(latP) * Math.Cos(lon0 - lonP)
            headingfrom0toP = Math.Atan2(azx, azy) * -1
            'headingfrom0toP = headingfrom0toP / Math.PI * 180

            px = (distancefrom0toP * Math.Sin(headingfrom0toP)) / MapInchesPerPixel + xc
            py = ((distancefrom0toP * Math.Cos(headingfrom0toP)) / MapInchesPerPixel) * -1 + yc


            If px < 0 Or py < 0 Or px >= w Or py >= h Then
                'Don't display the point, it's off the screen.
            Else
                g.FillEllipse(MyBrush, New Rectangle(px - 8, py - 8, 16, 16))
                memoryBitmap.SetPixel(px, py, Color.Black)
            End If
        Next


        Dim delta As Integer = HLast500 / MapInchesPerPixel
        g.DrawEllipse(Pens.Orange, xc - delta, yc - delta, delta * 2, delta * 2)
        delta = HLast682 / MapInchesPerPixel
        g.DrawEllipse(Pens.Red, xc - delta, yc - delta, delta * 2, delta * 2)
        delta = HLast955 / MapInchesPerPixel
        g.DrawEllipse(Pens.Green, xc - delta, yc - delta, delta * 2, delta * 2)
        delta = HLast997 / MapInchesPerPixel
        g.DrawEllipse(Pens.Blue, xc - delta, yc - delta, delta * 2, delta * 2)

        Dim smallFont As New Font("Ariel", 8)
        g.DrawString("Lefebure.com TestGPS", smallFont, Brushes.LightGray, 5, h - 12)


        g.DrawString(BaseFileName, myFont, Brushes.Black, 10, 10)
        g.DrawString("Horizontal Travel Path (X and Y)", myFont, Brushes.Black, 10, 35)
        g.DrawString("Average Drift: " & Format(HLastAvg, "Fixed") & """", myFont, Brushes.Black, 10, 60)
        g.DrawString("(50%) CEP: " & Format(HLast500, "Fixed") & """", myFont, Brushes.Orange, 10, 85)
        g.DrawString("(68.2%) 1 Sigma: " & Format(HLast682, "Fixed") & """", myFont, Brushes.Red, 10, 110)
        g.DrawString("(95.5%) 2 Sigma: " & Format(HLast955, "Fixed") & """", myFont, Brushes.Green, 10, 135)
        g.DrawString("(99.7%) 3 Sigma: " & Format(HLast997, "Fixed") & """", myFont, Brushes.Blue, 10, 160)
        g.DrawString("Maximum: " & Format(HLastMax, "Fixed") & """", myFont, Brushes.Black, 10, 185)
        g.DrawString("Data Points: " & dtcoords.Rows.Count, myFont, Brushes.Black, 10, 210) '210

        g.Dispose()

        memoryBitmap.Save(BaseFilePath & BaseFileName & "-path.gif", System.Drawing.Imaging.ImageFormat.Gif)
    End Sub
    Public Sub GenerateXImage()
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim xc As Integer = Fix(w / 2) - 1
        Dim yc As Integer = Fix(h / 2) - 1
        Dim memoryBitmap As Bitmap = New Bitmap(w, h)
        Dim MapInchesPerPixel As Double = 1
        Dim myFont As New Font("Ariel", 12)

        Dim filter As String = ""
        Dim sortby As String = "distfromx"
        Dim points As DataRow() = dtcoords.Select(filter, sortby)
        Dim rowcount As Integer = points.GetUpperBound(0)

        Dim zMin As Double = points(0)(7)
        Dim zMax As Double = points(rowcount)(7)

        Dim lines As Integer = rowcount / 4
        If lines > w Then lines = w

        Dim line(lines) As Integer
        For i = 0 To UBound(line)
            line(i) = 0
        Next

        If zMax = zMin Then 'Can't divide by zero
            line(Math.Round(lines / 2)) = lines
        Else
            For i = 0 To rowcount
                Dim Place As Integer = Math.Round((points(i)(7) - zMin) / (zMax - zMin) * UBound(line))
                line(Place) += 1
            Next
        End If

        Dim MaxCount As Integer = 0
        For i = 0 To UBound(line)
            If line(i) > MaxCount Then MaxCount = line(i)
        Next

        Dim g As Graphics = Graphics.FromImage(memoryBitmap)
        g.Clear(Color.White)

        Dim ThisVal As Integer
        Dim LastVal As Integer = h - 24 - (line(0) / MaxCount * (h - 25))

        For i = 1 To UBound(line)
            ThisVal = h - 24 - (line(i) / MaxCount * (h - 25))
            g.DrawLine(Pens.Black, CInt((i - 1) / lines * w), LastVal, CInt(i / lines * w), ThisVal)
            LastVal = ThisVal
        Next

        g.DrawString(BaseFileName, myFont, Brushes.Black, 10, 10)
        g.DrawString("X Distribution (East-West)", myFont, Brushes.Black, 10, 35)
        'g.DrawString(Now(), myFont, Brushes.Black, 10, 10)
        'g.DrawString("AVG: " & Format(HLastAvg, "Fixed") & """", myFont, Brushes.Black, 10, 35)
        'g.DrawString("CEP: " & Format(HLast500, "Fixed") & """", myFont, Brushes.Orange, 10, 60)
        'g.DrawString("1 Sigma: " & Format(HLast682, "Fixed") & """", myFont, Brushes.Red, 10, 85)
        'g.DrawString("2 Sigma: " & Format(HLast955, "Fixed") & """", myFont, Brushes.Green, 10, 110)
        'g.DrawString("3 Sigma: " & Format(HLast997, "Fixed") & """", myFont, Brushes.Blue, 10, 135)
        'g.DrawString("Max: " & Format(HLastMax, "Fixed") & """", myFont, Brushes.Black, 10, 160)
        ''g.DrawString("Duration: " & hourson & ":" & Format(minuteson, "00"), myFont, Brushes.Black, 10, 185)
        'g.DrawString("Data Points: " & dtcoords.Rows.Count, myFont, Brushes.Black, 10, 185) '210
        g.DrawString("West", myFont, Brushes.Black, 3, h - 20)
        g.DrawString("East", myFont, Brushes.Black, w - 40, h - 20)

        g.Dispose()

        memoryBitmap.Save(BaseFilePath & BaseFileName & "-xdist.gif", System.Drawing.Imaging.ImageFormat.Gif)
    End Sub
    Public Sub GenerateYImage()
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim xc As Integer = Fix(w / 2) - 1
        Dim yc As Integer = Fix(h / 2) - 1
        Dim memoryBitmap As Bitmap = New Bitmap(w, h)
        Dim MapInchesPerPixel As Double = 1
        Dim myFont As New Font("Ariel", 12)

        Dim filter As String = ""
        Dim sortby As String = "distfromy"
        Dim points As DataRow() = dtcoords.Select(filter, sortby)
        Dim rowcount As Integer = points.GetUpperBound(0)

        Dim zMin As Double = points(0)(8)
        Dim zMax As Double = points(rowcount)(8)

        Dim lines As Integer = rowcount / 4
        If lines > w Then lines = w

        Dim line(lines) As Integer
        For i = 0 To UBound(line)
            line(i) = 0
        Next

        If zMax = zMin Then 'Can't divide by zero
            line(Math.Round(lines / 2)) = lines
        Else
            For i = 0 To rowcount
                Dim Place As Integer = Math.Round((points(i)(8) - zMin) / (zMax - zMin) * UBound(line))
                line(Place) += 1
            Next
        End If

        Dim MaxCount As Integer = 0
        For i = 0 To UBound(line)
            If line(i) > MaxCount Then MaxCount = line(i)
        Next

        Dim g As Graphics = Graphics.FromImage(memoryBitmap)
        g.Clear(Color.White)

        Dim ThisVal As Integer
        Dim LastVal As Integer = w - 52 - (line(0) / MaxCount * (w - 53))
        For i = 1 To UBound(line)
            ThisVal = w - 52 - (line(i) / MaxCount * (w - 53))
            g.DrawLine(Pens.Black, LastVal, CInt((i - 1) / lines * h), ThisVal, CInt(i / lines * h))
            LastVal = ThisVal
        Next

        g.DrawString(BaseFileName, myFont, Brushes.Black, 10, 10)
        g.DrawString("Y Distribution (North-South)", myFont, Brushes.Black, 10, 35)
        'g.DrawString(Now(), myFont, Brushes.Black, 10, 10)
        'g.DrawString("AVG: " & Format(HLastAvg, "Fixed") & """", myFont, Brushes.Black, 10, 35)
        'g.DrawString("CEP: " & Format(HLast500, "Fixed") & """", myFont, Brushes.Orange, 10, 60)
        'g.DrawString("1 Sigma: " & Format(HLast682, "Fixed") & """", myFont, Brushes.Red, 10, 85)
        'g.DrawString("2 Sigma: " & Format(HLast955, "Fixed") & """", myFont, Brushes.Green, 10, 110)
        'g.DrawString("3 Sigma: " & Format(HLast997, "Fixed") & """", myFont, Brushes.Blue, 10, 135)
        'g.DrawString("Max: " & Format(HLastMax, "Fixed") & """", myFont, Brushes.Black, 10, 160)
        ''g.DrawString("Duration: " & hourson & ":" & Format(minuteson, "00"), myFont, Brushes.Black, 10, 185)
        'g.DrawString("Data Points: " & dtcoords.Rows.Count, myFont, Brushes.Black, 10, 185) '210
        g.DrawString("North", myFont, Brushes.Black, w - 48, 3)
        g.DrawString("South", myFont, Brushes.Black, w - 48, h - 20)

        g.Dispose()

        memoryBitmap.Save(BaseFilePath & BaseFileName & "-ydist.gif", System.Drawing.Imaging.ImageFormat.Gif)
    End Sub
    Public Sub GenerateZImage()
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim xc As Integer = Fix(w / 2) - 1
        Dim yc As Integer = Fix(h / 2) - 1
        Dim memoryBitmap As Bitmap = New Bitmap(w, h)
        Dim MapInchesPerPixel As Double = 1
        Dim myFont As New Font("Ariel", 12)

        Dim filter As String = ""
        Dim sortby As String = "distfromz"
        Dim points As DataRow() = dtcoords.Select(filter, sortby)
        Dim rowcount As Integer = points.GetUpperBound(0)

        Dim zMin As Double = points(0)(9)
        Dim zMax As Double = points(rowcount)(9)

        Dim lines As Integer = rowcount / 4
        If lines > w Then lines = w

        Dim line(lines) As Integer
        For i = 0 To UBound(line)
            line(i) = 0
        Next

        If zMax = zMin Then 'Can't divide by zero
            line(Math.Round(lines / 2)) = lines
        Else
            For i = 0 To rowcount
                Dim Place As Integer = Math.Round((points(i)(9) - zMin) / (zMax - zMin) * UBound(line))
                line(Place) += 1
            Next
        End If

        Dim MaxCount As Integer = 0
        For i = 0 To UBound(line)
            If line(i) > MaxCount Then MaxCount = line(i)
        Next

        Dim g As Graphics = Graphics.FromImage(memoryBitmap)
        g.Clear(Color.White)

        Dim ThisVal As Integer
        Dim LastVal As Integer = w - 44 - (line(0) / MaxCount * (w - 45))
        For i = 1 To UBound(line)
            ThisVal = w - 44 - (line(i) / MaxCount * (w - 45))
            g.DrawLine(Pens.Black, LastVal, CInt((i - 1) / lines * h), ThisVal, CInt(i / lines * h))
            LastVal = ThisVal
        Next

        g.DrawString(BaseFileName, myFont, Brushes.Black, 10, 10)
        g.DrawString("Z Distribution (Elevation)", myFont, Brushes.Black, 10, 35)
        g.DrawString("AVG: " & Format(VLastAvg, "Fixed") & """", myFont, Brushes.Black, 10, 60)
        g.DrawString("CEP: " & Format(VLast500, "Fixed") & """", myFont, Brushes.Orange, 10, 85)
        g.DrawString("1 Sigma: " & Format(VLast682, "Fixed") & """", myFont, Brushes.Red, 10, 110)
        g.DrawString("2 Sigma: " & Format(VLast955, "Fixed") & """", myFont, Brushes.Green, 10, 135)
        g.DrawString("3 Sigma: " & Format(VLast997, "Fixed") & """", myFont, Brushes.Blue, 10, 160)
        g.DrawString("Max: " & Format(VLastMax, "Fixed") & """", myFont, Brushes.Black, 10, 185)
        g.DrawString("High", myFont, Brushes.Black, w - 40, 3)
        g.DrawString("Low", myFont, Brushes.Black, w - 40, h - 20)

        g.Dispose()

        memoryBitmap.Save(BaseFilePath & BaseFileName & "-zdist.gif", System.Drawing.Imaging.ImageFormat.Gif)
    End Sub
    Public Sub GenerateSatVisImage()
        Dim memoryBitmap As Bitmap = New Bitmap(w, h)
        Dim myFont As New Font("Ariel", 12)

        'Find average, min, max sat vis
        Dim SatSum As Integer = 0
        Dim PointSum As Integer = 0
        Dim MinSats As Integer = 0
        Dim MaxSats As Integer = 0
        For Each row In dtcoords.Rows
            SatSum += row("sats")
            If PointSum = 0 Then
                MinSats = row("sats")
                MaxSats = row("sats")
            Else
                If row("sats") > MaxSats Then MaxSats = row("sats")
                If row("sats") < MinSats Then MinSats = row("sats")
            End If
            PointSum += 1
        Next
        Dim AvgSats As Single = SatSum / PointSum


        Dim g As Graphics = Graphics.FromImage(memoryBitmap)
        g.Clear(Color.White)

        Dim yScale As Double = MaxSats
        If yScale <= 0 Then yScale = 1
        Dim xScale As Integer = dtcoords.Rows(dtcoords.Rows.Count - 1).Item("rtime")
        Dim px As Integer
        Dim py As Integer
        Dim lpx As Integer = -1
        Dim lpy As Integer = -1
        For Each row In dtcoords.Rows
            px = (row("rtime") / xScale) * (w - 1)
            py = (h - 21) - ((row("sats") / yScale) * (h - 21))
            'If py >= 0 Then
            'memoryBitmap.SetPixel(px, py, Color.Black)
            'End If
            If lpx > -1 And lpy > -1 Then
                g.DrawLine(Pens.Black, lpx, lpy, px, py)
            End If
            lpx = px
            lpy = py
        Next

        g.DrawString(BaseFileName, myFont, Brushes.Black, 10, 10)
        g.DrawString("Satellite Visibility", myFont, Brushes.Black, 10, 35)
        g.DrawString("Max: " & MaxSats, myFont, Brushes.Black, 10, 60)
        g.DrawString("Avg: " & Format(AvgSats, "Fixed"), myFont, Brushes.Black, 10, 85)
        g.DrawString("Min: " & MinSats, myFont, Brushes.Black, 10, 110)
        g.DrawLine(Pens.Black, 0, h - 20, w, h - 20)
        g.DrawString("Start", myFont, Brushes.Black, 3, h - 20)
        g.DrawString("End", myFont, Brushes.Black, w - 40, h - 20)
        g.Dispose()

        memoryBitmap.Save(BaseFilePath & BaseFileName & "-satvis.gif", System.Drawing.Imaging.ImageFormat.Gif)
    End Sub
    Public Sub ProcessShortTermDrift()
        ShortTermPointsFound = 0

        Dim lat0 As Double
        Dim lon0 As Double
        Dim latP As Double
        Dim lonP As Double
        Dim dlat As Double
        Dim dlon As Double
        Dim A As Double
        Dim C As Double
        Dim distancefrom0toP As Double
        Dim AltDistance As Double


        Dim pindex As Integer = 0 'previous time index
        Dim startindex As Integer = -1
        Dim totalrows As Integer = dtcoords.Rows.Count

        For i = 0 To totalrows - 1
            If dtcoords.Rows(i).Item("rtime") >= TimeDelay Then
                startindex = i
                Exit For
            End If
        Next

        If startindex = -1 Then 'we didn't find a starting place
            Exit Sub
        End If

        For i = startindex To totalrows - 1
            'See if we can advance the pindex?
            Do While True
                If dtcoords.Rows(i).Item("rtime") - dtcoords.Rows(pindex + 1).Item("rtime") >= TimeDelay Then
                    pindex += 1
                Else
                    Exit Do
                End If
            Loop
            ShortTermPointsFound += 1

            'Find the horizontal difference
            lat0 = dtcoords.Rows(pindex).Item("lat") / 180 * Math.PI
            lon0 = dtcoords.Rows(pindex).Item("lon") / 180 * Math.PI
            latP = dtcoords.Rows(i).Item("lat") / 180 * Math.PI
            lonP = dtcoords.Rows(i).Item("lon") / 180 * Math.PI
            dlat = lat0 - latP
            dlon = lon0 - lonP
            A = ((Math.Sin(dlat / 2)) ^ 2) + Math.Cos(latP) * Math.Cos(lat0) * ((Math.Sin(dlon / 2)) ^ 2)
            C = 2 * Math.Atan2(Math.Sqrt(A), Math.Sqrt(1 - A))
            distancefrom0toP = (3956 * C) * 5280 * 12
            dtcoords.Rows(i).Item("distfromhc") = distancefrom0toP 'in inches

            'Find vertical difference
            AltDistance = dtcoords.Rows(i).Item("elev") - dtcoords.Rows(pindex).Item("elev")
            dtcoords.Rows(i).Item("distfromvc") = Math.Abs(AltDistance) 'in meters
        Next
    End Sub
    Public Sub GenerateShortTermHImage()
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim xc As Integer = Fix(w / 2) - 1
        Dim yc As Integer = Fix(h / 2) - 1
        Dim memoryBitmap As Bitmap = New Bitmap(w, h)
        Dim MapInchesPerPixel As Double = 1
        Dim myFont As New Font("Ariel", 12)

        Dim filter As String = ""
        Dim sortby As String = "distfromhc"
        Dim points As DataRow() = dtcoords.Select(filter, sortby)
        Dim rowcount As Integer = points.GetUpperBound(0) + 1

        Dim x500 As Integer = Math.Round(rowcount * 0.5) - 1
        Dim x682 As Integer = Math.Round(rowcount * 0.682) - 1
        Dim x955 As Integer = Math.Round(rowcount * 0.955) - 1
        Dim x997 As Integer = Math.Round(rowcount * 0.997) - 1

        If x500 < 0 Then x500 = 0
        If x682 < 0 Then x682 = 0
        If x955 < 0 Then x955 = 0
        If x997 < 0 Then x997 = 0

        HLastAvg = CDbl(dtcoords.Compute("AVG(distfromhc)", ""))
        HLast500 = points(x500)(5)
        HLast682 = points(x682)(5)
        HLast955 = points(x955)(5)
        HLast997 = points(x997)(5)
        HLastMax = CDbl(dtcoords.Compute("MAX(distfromhc)", ""))


        Dim g As Graphics = Graphics.FromImage(memoryBitmap)
        g.Clear(Color.White)

        Dim yScale As Double = HLast997
        If yScale <= 0 Then yScale = 1
        Dim xScale As Integer = dtcoords.Rows(dtcoords.Rows.Count - 1).Item("rtime")
        Dim px As Integer
        Dim py As Integer

        Dim startindex As Integer = -1
        Dim totalrows As Integer = dtcoords.Rows.Count
        For Each row In dtcoords.Rows
            If row("rtime") >= TimeDelay Then
                px = (row("rtime") / xScale) * (w - 1)
                py = (h - 21) - ((row("distfromhc") / yScale) * (h - 21))
                If py >= 0 Then
                    memoryBitmap.SetPixel(px, py, Color.Black)
                End If
            End If
        Next

        g.DrawString(BaseFileName, myFont, Brushes.Black, 10, 10)
        g.DrawString("Short Term Horizontal Drift Distribution (" & TimeDelay & " seconds)", myFont, Brushes.Black, 10, 35)
        g.DrawString("AVG: " & Format(HLastAvg, "Fixed") & """", myFont, Brushes.Black, 10, 60)
        g.DrawString("CEP: " & Format(HLast500, "Fixed") & """", myFont, Brushes.Orange, 10, 85)
        g.DrawString("1 Sigma: " & Format(HLast682, "Fixed") & """", myFont, Brushes.Red, 10, 110)
        g.DrawString("2 Sigma: " & Format(HLast955, "Fixed") & """", myFont, Brushes.Green, 10, 135)
        g.DrawString("3 Sigma: " & Format(HLast997, "Fixed") & """", myFont, Brushes.Blue, 10, 160)
        g.DrawString("Max: " & Format(HLastMax, "Fixed") & """", myFont, Brushes.Black, 10, 185)
        g.DrawLine(Pens.Black, 0, h - 20, w, h - 20)
        g.DrawString("Start", myFont, Brushes.Black, 3, h - 20)
        g.DrawString("End", myFont, Brushes.Black, w - 40, h - 20)
        g.Dispose()

        memoryBitmap.Save(BaseFilePath & BaseFileName & "-sthdist.gif", System.Drawing.Imaging.ImageFormat.Gif)



        'Dim textfile As String = BaseFilePath & BaseFileName & "-sthdist.csv"
        'Try
        '    Dim sWriter As IO.StreamWriter = New IO.StreamWriter(textfile)
        '    sWriter.WriteLine("Time,Horizontal Combined")
        '    For Each row In dtcoords.Rows
        '        If row("rtime") >= TimeDelay And row("rtime") < 30000 + TimeDelay Then
        '            sWriter.WriteLine(row("rtime") & "," & row("distfromhc"))
        '        End If
        '    Next
        '    sWriter.Flush()
        '    sWriter.Close()
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub
    Public Sub ProcessShortTermXDrift()
        ShortTermPointsFound = 0

        Dim lat0 As Double
        Dim lon0 As Double
        Dim latP As Double
        Dim lonP As Double
        Dim dlat As Double
        Dim dlon As Double
        Dim A As Double
        Dim C As Double
        Dim distancefrom0toP As Double
        'Dim AltDistance As Double


        Dim pindex As Integer = 0 'previous time index
        Dim startindex As Integer = -1
        Dim totalrows As Integer = dtcoords.Rows.Count

        For i = 0 To totalrows - 1
            If dtcoords.Rows(i).Item("rtime") >= TimeDelay Then
                startindex = i
                Exit For
            End If
        Next

        If startindex = -1 Then 'we didn't find a starting place
            Exit Sub
        End If

        For i = startindex To totalrows - 1
            'See if we can advance the pindex?
            Do While True
                If dtcoords.Rows(i).Item("rtime") - dtcoords.Rows(pindex + 1).Item("rtime") >= TimeDelay Then
                    pindex += 1
                Else
                    Exit Do
                End If
            Loop
            ShortTermPointsFound += 1

            'Find the horizontal difference
            lat0 = dtcoords.Rows(pindex).Item("lat") / 180 * Math.PI
            lon0 = dtcoords.Rows(pindex).Item("lon") / 180 * Math.PI
            latP = dtcoords.Rows(i).Item("lat") / 180 * Math.PI
            lonP = dtcoords.Rows(i).Item("lon") / 180 * Math.PI
            dlat = 0 'lat0 - latP
            dlon = lon0 - lonP
            A = ((Math.Sin(dlat / 2)) ^ 2) + Math.Cos(latP) * Math.Cos(lat0) * ((Math.Sin(dlon / 2)) ^ 2)
            C = 2 * Math.Atan2(Math.Sqrt(A), Math.Sqrt(1 - A))
            distancefrom0toP = (3956 * C) * 5280 * 12
            dtcoords.Rows(i).Item("distfromhc") = distancefrom0toP 'in inches
        Next
    End Sub
    Public Sub GenerateShortTermXImage()
        'Dim x As Integer = 0
        'Dim y As Integer = 0
        'Dim xc As Integer = Fix(w / 2) - 1
        'Dim yc As Integer = Fix(h / 2) - 1
        Dim memoryBitmap As Bitmap = New Bitmap(w, h)
        'Dim MapInchesPerPixel As Double = 1
        Dim myFont As New Font("Ariel", 12)

        Dim filter As String = ""
        Dim sortby As String = "distfromhc"
        Dim points As DataRow() = dtcoords.Select(filter, sortby)
        Dim rowcount As Integer = points.GetUpperBound(0) + 1

        Dim x500 As Integer = Math.Round(rowcount * 0.5) - 1
        Dim x682 As Integer = Math.Round(rowcount * 0.682) - 1
        Dim x955 As Integer = Math.Round(rowcount * 0.955) - 1
        Dim x997 As Integer = Math.Round(rowcount * 0.997) - 1

        If x500 < 0 Then x500 = 0
        If x682 < 0 Then x682 = 0
        If x955 < 0 Then x955 = 0
        If x997 < 0 Then x997 = 0

        HLastAvg = CDbl(dtcoords.Compute("AVG(distfromhc)", ""))
        HLast500 = points(x500)(5)
        HLast682 = points(x682)(5)
        HLast955 = points(x955)(5)
        HLast997 = points(x997)(5)
        HLastMax = CDbl(dtcoords.Compute("MAX(distfromhc)", ""))


        Dim g As Graphics = Graphics.FromImage(memoryBitmap)
        g.Clear(Color.White)

        Dim yScale As Double = HLast997
        If yScale <= 0 Then yScale = 1
        Dim xScale As Integer = dtcoords.Rows(dtcoords.Rows.Count - 1).Item("rtime")
        Dim px As Integer
        Dim py As Integer
        For Each row In dtcoords.Rows
            If row("rtime") >= TimeDelay Then
                px = (row("rtime") / xScale) * (w - 1)
                py = (h - 21) - ((row("distfromhc") / yScale) * (h - 21))
                If py >= 0 Then
                    memoryBitmap.SetPixel(px, py, Color.Black)
                End If
            End If
        Next

        g.DrawString(BaseFileName, myFont, Brushes.Black, 10, 10)
        g.DrawString("Short Term East-West Drift Distribution (" & TimeDelay & " seconds)", myFont, Brushes.Black, 10, 35)
        g.DrawString("AVG: " & Format(HLastAvg, "Fixed") & """", myFont, Brushes.Black, 10, 60)
        g.DrawString("CEP: " & Format(HLast500, "Fixed") & """", myFont, Brushes.Orange, 10, 85)
        g.DrawString("1 Sigma: " & Format(HLast682, "Fixed") & """", myFont, Brushes.Red, 10, 110)
        g.DrawString("2 Sigma: " & Format(HLast955, "Fixed") & """", myFont, Brushes.Green, 10, 135)
        g.DrawString("3 Sigma: " & Format(HLast997, "Fixed") & """", myFont, Brushes.Blue, 10, 160)
        g.DrawString("Max: " & Format(HLastMax, "Fixed") & """", myFont, Brushes.Black, 10, 185)
        g.DrawLine(Pens.Black, 0, h - 20, w, h - 20)
        g.DrawString("Start", myFont, Brushes.Black, 3, h - 20)
        g.DrawString("End", myFont, Brushes.Black, w - 40, h - 20)
        g.Dispose()

        memoryBitmap.Save(BaseFilePath & BaseFileName & "-stxdist.gif", System.Drawing.Imaging.ImageFormat.Gif)


        'Dim textfile As String = BaseFilePath & BaseFileName & "-stxdist.csv"
        'Try
        '    Dim sWriter As IO.StreamWriter = New IO.StreamWriter(textfile)
        '    sWriter.WriteLine("Time,East-West")
        '    For Each row In dtcoords.Rows
        '        If row("rtime") >= TimeDelay And row("rtime") < 30000 + TimeDelay Then
        '            sWriter.WriteLine(row("rtime") & "," & row("distfromhc"))
        '        End If
        '    Next
        '    sWriter.Flush()
        '    sWriter.Close()
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub
    Public Sub ProcessShortTermYDrift()
        ShortTermPointsFound = 0

        Dim lat0 As Double
        Dim lon0 As Double
        Dim latP As Double
        Dim lonP As Double
        Dim dlat As Double
        Dim dlon As Double
        Dim A As Double
        Dim C As Double
        Dim distancefrom0toP As Double
        'Dim AltDistance As Double


        Dim pindex As Integer = 0 'previous time index
        Dim startindex As Integer = -1
        Dim totalrows As Integer = dtcoords.Rows.Count

        For i = 0 To totalrows - 1
            If dtcoords.Rows(i).Item("rtime") >= TimeDelay Then
                startindex = i
                Exit For
            End If
        Next

        If startindex = -1 Then 'we didn't find a starting place
            Exit Sub
        End If

        For i = startindex To totalrows - 1
            'See if we can advance the pindex?
            Do While True
                If dtcoords.Rows(i).Item("rtime") - dtcoords.Rows(pindex + 1).Item("rtime") >= TimeDelay Then
                    pindex += 1
                Else
                    Exit Do
                End If
            Loop
            ShortTermPointsFound += 1

            'Find the horizontal difference
            lat0 = dtcoords.Rows(pindex).Item("lat") / 180 * Math.PI
            lon0 = dtcoords.Rows(pindex).Item("lon") / 180 * Math.PI
            latP = dtcoords.Rows(i).Item("lat") / 180 * Math.PI
            lonP = dtcoords.Rows(i).Item("lon") / 180 * Math.PI
            dlat = lat0 - latP
            dlon = 0 'lon0 - lonP
            A = ((Math.Sin(dlat / 2)) ^ 2) + Math.Cos(latP) * Math.Cos(lat0) * ((Math.Sin(dlon / 2)) ^ 2)
            C = 2 * Math.Atan2(Math.Sqrt(A), Math.Sqrt(1 - A))
            distancefrom0toP = (3956 * C) * 5280 * 12
            dtcoords.Rows(i).Item("distfromhc") = distancefrom0toP 'in inches

            'Find vertical difference
            'AltDistance = dtcoords.Rows(i).Item("elev") - dtcoords.Rows(pindex).Item("elev")
            'dtcoords.Rows(i).Item("distfromvc") = Math.Abs(AltDistance) 'in meters
        Next
    End Sub
    Public Sub GenerateShortTermYImage()
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim xc As Integer = Fix(w / 2) - 1
        Dim yc As Integer = Fix(h / 2) - 1
        Dim memoryBitmap As Bitmap = New Bitmap(w, h)
        Dim MapInchesPerPixel As Double = 1
        Dim myFont As New Font("Ariel", 12)

        Dim filter As String = ""
        Dim sortby As String = "distfromhc"
        Dim points As DataRow() = dtcoords.Select(filter, sortby)
        Dim rowcount As Integer = points.GetUpperBound(0) + 1

        Dim x500 As Integer = Math.Round(rowcount * 0.5) - 1
        Dim x682 As Integer = Math.Round(rowcount * 0.682) - 1
        Dim x955 As Integer = Math.Round(rowcount * 0.955) - 1
        Dim x997 As Integer = Math.Round(rowcount * 0.997) - 1

        If x500 < 0 Then x500 = 0
        If x682 < 0 Then x682 = 0
        If x955 < 0 Then x955 = 0
        If x997 < 0 Then x997 = 0

        HLastAvg = CDbl(dtcoords.Compute("AVG(distfromhc)", ""))
        HLast500 = points(x500)(5)
        HLast682 = points(x682)(5)
        HLast955 = points(x955)(5)
        HLast997 = points(x997)(5)
        HLastMax = CDbl(dtcoords.Compute("MAX(distfromhc)", ""))


        Dim g As Graphics = Graphics.FromImage(memoryBitmap)
        g.Clear(Color.White)

        Dim yScale As Double = HLast997
        If yScale <= 0 Then yScale = 1
        Dim xScale As Integer = dtcoords.Rows(dtcoords.Rows.Count - 1).Item("rtime")
        Dim px As Integer
        Dim py As Integer
        For Each row In dtcoords.Rows
            If row("rtime") >= TimeDelay Then
                px = (row("rtime") / xScale) * (w - 1)
                py = (h - 21) - ((row("distfromhc") / yScale) * (h - 21))
                If py >= 0 Then
                    memoryBitmap.SetPixel(px, py, Color.Black)
                End If
            End If
        Next

        g.DrawString(BaseFileName, myFont, Brushes.Black, 10, 10)
        g.DrawString("Short Term North-South Drift Distribution (" & TimeDelay & " seconds)", myFont, Brushes.Black, 10, 35)
        g.DrawString("AVG: " & Format(HLastAvg, "Fixed") & """", myFont, Brushes.Black, 10, 60)
        g.DrawString("CEP: " & Format(HLast500, "Fixed") & """", myFont, Brushes.Orange, 10, 85)
        g.DrawString("1 Sigma: " & Format(HLast682, "Fixed") & """", myFont, Brushes.Red, 10, 110)
        g.DrawString("2 Sigma: " & Format(HLast955, "Fixed") & """", myFont, Brushes.Green, 10, 135)
        g.DrawString("3 Sigma: " & Format(HLast997, "Fixed") & """", myFont, Brushes.Blue, 10, 160)
        g.DrawString("Max: " & Format(HLastMax, "Fixed") & """", myFont, Brushes.Black, 10, 185)
        g.DrawLine(Pens.Black, 0, h - 20, w, h - 20)
        g.DrawString("Start", myFont, Brushes.Black, 3, h - 20)
        g.DrawString("End", myFont, Brushes.Black, w - 40, h - 20)
        g.Dispose()

        memoryBitmap.Save(BaseFilePath & BaseFileName & "-stydist.gif", System.Drawing.Imaging.ImageFormat.Gif)



        'Dim textfile As String = BaseFilePath & BaseFileName & "-stydist.csv"
        'Try
        '    Dim sWriter As IO.StreamWriter = New IO.StreamWriter(textfile)
        '    sWriter.WriteLine("Time,North-South")
        '    For Each row In dtcoords.Rows
        '        If row("rtime") >= TimeDelay And row("rtime") < 30000 + TimeDelay Then
        '            sWriter.WriteLine(row("rtime") & "," & row("distfromhc"))
        '        End If
        '    Next
        '    sWriter.Flush()
        '    sWriter.Close()
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub
    Public Sub GenerateShortTermVImage()
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim xc As Integer = Fix(w / 2) - 1
        Dim yc As Integer = Fix(h / 2) - 1
        Dim memoryBitmap As Bitmap = New Bitmap(w, h)
        Dim MapInchesPerPixel As Double = 1
        Dim myFont As New Font("Ariel", 12)

        Dim filter As String = ""
        Dim sortby As String = "distfromvc"
        Dim Vpoints As DataRow() = dtcoords.Select(filter, sortby)
        Dim Vrowcount As Integer = Vpoints.GetUpperBound(0) + 1

        Dim Vx500 As Integer = Math.Round(Vrowcount * 0.5) - 1
        Dim Vx682 As Integer = Math.Round(Vrowcount * 0.682) - 1
        Dim Vx955 As Integer = Math.Round(Vrowcount * 0.955) - 1
        Dim Vx997 As Integer = Math.Round(Vrowcount * 0.997) - 1

        If Vx500 < 0 Then Vx500 = 0
        If Vx682 < 0 Then Vx682 = 0
        If Vx955 < 0 Then Vx955 = 0
        If Vx997 < 0 Then Vx997 = 0

        VLastAvg = CDbl(dtcoords.Compute("AVG(distfromvc)", "")) * 3.2808399 * 12
        VLast500 = Vpoints(Vx500)(6) * 3.2808399 * 12
        VLast682 = Vpoints(Vx682)(6) * 3.2808399 * 12
        VLast955 = Vpoints(Vx955)(6) * 3.2808399 * 12
        VLast997 = Vpoints(Vx997)(6) * 3.2808399 * 12
        VLastMax = CDbl(dtcoords.Compute("MAX(distfromvc)", "")) * 3.2808399 * 12


        Dim g As Graphics = Graphics.FromImage(memoryBitmap)
        g.Clear(Color.White)

        Dim yScale As Double = VLast997
        If yScale <= 0 Then yScale = 1
        Dim xScale As Integer = dtcoords.Rows(dtcoords.Rows.Count - 1).Item("rtime")
        Dim px As Integer
        Dim py As Integer
        For Each row In dtcoords.Rows
            If row("rtime") >= TimeDelay Then
                px = (row("rtime") / xScale) * (w - 1)
                py = (h - 21) - (((row("distfromvc") * 3.2808399 * 12) / yScale) * (h - 21))
                If py >= 0 Then
                    memoryBitmap.SetPixel(px, py, Color.Black)
                End If
            End If
        Next

        g.DrawString(BaseFileName, myFont, Brushes.Black, 10, 10)
        g.DrawString("Short Term Vertical Drift Distribution (" & TimeDelay & " seconds)", myFont, Brushes.Black, 10, 35)
        g.DrawString("AVG: " & Format(VLastAvg, "Fixed") & """", myFont, Brushes.Black, 10, 60)
        g.DrawString("CEP: " & Format(VLast500, "Fixed") & """", myFont, Brushes.Orange, 10, 85)
        g.DrawString("1 Sigma: " & Format(VLast682, "Fixed") & """", myFont, Brushes.Red, 10, 110)
        g.DrawString("2 Sigma: " & Format(VLast955, "Fixed") & """", myFont, Brushes.Green, 10, 135)
        g.DrawString("3 Sigma: " & Format(VLast997, "Fixed") & """", myFont, Brushes.Blue, 10, 160)
        g.DrawString("Max: " & Format(VLastMax, "Fixed") & """", myFont, Brushes.Black, 10, 185)
        g.DrawLine(Pens.Black, 0, h - 20, w, h - 20)
        g.DrawString("Start", myFont, Brushes.Black, 3, h - 20)
        g.DrawString("End", myFont, Brushes.Black, w - 40, h - 20)

        g.Dispose()

        memoryBitmap.Save(BaseFilePath & BaseFileName & "-stvdist.gif", System.Drawing.Imaging.ImageFormat.Gif)



        'Dim textfile As String = BaseFilePath & BaseFileName & "-stvdist.csv"
        'Try
        '    Dim sWriter As IO.StreamWriter = New IO.StreamWriter(textfile)
        '    sWriter.WriteLine("Time,Vertical")
        '    For Each row In dtcoords.Rows
        '        If row("rtime") >= TimeDelay And row("rtime") < 30000 + TimeDelay Then
        '            sWriter.WriteLine(row("rtime") & "," & row("distfromvc") * 3.2808399 * 12)
        '        End If
        '    Next
        '    sWriter.Flush()
        '    sWriter.Close()
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub
    Public Sub GenerateHTMLFile()
        Dim HTMLFile As String = BaseFilePath & BaseFileName & ".htm"
        With My.Computer.FileSystem
            .WriteAllText(HTMLFile, "<HTML><HEAD><TITLE>" & BaseFileName & "</TITLE></HEAD>" & vbCrLf, False)
            .WriteAllText(HTMLFile, "<BODY>" & vbCrLf, True)

            .WriteAllText(HTMLFile, "<B>Data File: " & BaseFileName & "</B><BR>" & vbCrLf, True)
            .WriteAllText(HTMLFile, "Processed At: " & Now() & "<BR>" & vbCrLf, True)

            .WriteAllText(HTMLFile, "Average Latitude: " & AvgLat & "<BR>" & vbCrLf, True)
            .WriteAllText(HTMLFile, "Average Longitude: " & AvgLon & "<BR>" & vbCrLf, True)
            .WriteAllText(HTMLFile, "Average Altitude: " & AvgAlt & " meters<BR>" & vbCrLf, True)

            .WriteAllText(HTMLFile, "<BR><BR>" & vbCrLf, True)
            .WriteAllText(HTMLFile, "<IMG SRC=""" & BaseFileName & "-path.gif"" WIDTH=1000 HEIGHT=1000>" & vbCrLf, True)

            .WriteAllText(HTMLFile, "<BR><BR>" & vbCrLf, True)
            .WriteAllText(HTMLFile, "<IMG SRC=""" & BaseFileName & "-xdist.gif"" WIDTH=1000 HEIGHT=1000>" & vbCrLf, True)

            .WriteAllText(HTMLFile, "<BR><BR>" & vbCrLf, True)
            .WriteAllText(HTMLFile, "<IMG SRC=""" & BaseFileName & "-ydist.gif"" WIDTH=1000 HEIGHT=1000>" & vbCrLf, True)

            .WriteAllText(HTMLFile, "<BR><BR>" & vbCrLf, True)
            .WriteAllText(HTMLFile, "<IMG SRC=""" & BaseFileName & "-zdist.gif"" WIDTH=1000 HEIGHT=1000>" & vbCrLf, True)

            .WriteAllText(HTMLFile, "<BR><BR>" & vbCrLf, True)
            .WriteAllText(HTMLFile, "<IMG SRC=""" & BaseFileName & "-satvis.gif"" WIDTH=1000 HEIGHT=1000>" & vbCrLf, True)

            If ShortTermPointsFound > 0 Then
                .WriteAllText(HTMLFile, "<BR><BR>" & vbCrLf, True)
                .WriteAllText(HTMLFile, "<IMG SRC=""" & BaseFileName & "-sthdist.gif"" WIDTH=1000 HEIGHT=1000>" & vbCrLf, True)

                .WriteAllText(HTMLFile, "<BR><BR>" & vbCrLf, True)
                .WriteAllText(HTMLFile, "<IMG SRC=""" & BaseFileName & "-stvdist.gif"" WIDTH=1000 HEIGHT=1000>" & vbCrLf, True)

                .WriteAllText(HTMLFile, "<BR><BR>" & vbCrLf, True)
                .WriteAllText(HTMLFile, "<IMG SRC=""" & BaseFileName & "-stxdist.gif"" WIDTH=1000 HEIGHT=1000>" & vbCrLf, True)

                .WriteAllText(HTMLFile, "<BR><BR>" & vbCrLf, True)
                .WriteAllText(HTMLFile, "<IMG SRC=""" & BaseFileName & "-stydist.gif"" WIDTH=1000 HEIGHT=1000>" & vbCrLf, True)
            End If

            .WriteAllText(HTMLFile, "<BR><BR>" & vbCrLf, True)
            If My.Computer.FileSystem.FileExists(BaseFilePath & BaseFileName & ".zip") Then
                .WriteAllText(HTMLFile, "Download Data File: <A HREF=""" & BaseFileName & ".zip"">" & BaseFileName & ".zip</A>" & vbCrLf, True)
            Else
                .WriteAllText(HTMLFile, "Download Data File: <A HREF=""" & BaseFileName & ".gps"">" & BaseFileName & ".gps</A>" & vbCrLf, True)
            End If

            .WriteAllText(HTMLFile, "</BODY></HTML>" & vbCrLf, True)
        End With
    End Sub

End Class
