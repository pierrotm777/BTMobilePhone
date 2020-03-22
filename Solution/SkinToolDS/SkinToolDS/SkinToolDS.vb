Imports System
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Windows.Forms.FileDialog
Imports System.IO.File
Imports System.IO.Ports
Imports System.Threading
Imports System.Math
Imports System.Diagnostics
Imports System.Reflection
Imports System.Runtime.InteropServices

Imports SkinToolDS.NativeMethods

Public Class SkinToolDS
    Dim SDK As Object
    Dim sWatch As New Stopwatch
    Dim btnClearListBox As New Button
    Dim btnPauseListBox As New Button
    Dim btnSaveListBox As New Button
    Dim selFile As New SelectFiles
    Dim debugFrm As New DebugForm
    Dim objStreamWriter As StreamWriter
    Dim arrayCombo() As String
    Dim arrayLabel() As String
    Dim selTools As New SelectTools

    Private Sub SkinToolDS_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load


        Me.Location = My.Settings.LastPosition
        Me.TopMost = False
        btnCommands.Enabled = False
        lblInfo.Text = "Enter any command that " & FrontEndName & " understands, such as AUDIO to go to the music screen"
        txtBoxDebugLog.Visible = False

        groupScreen.Visible = False

        btnSendCommand1.Enabled = False
        btnSendCommand2.Enabled = False
        btnSendCommand3.Enabled = False
        btnSendCommand4.Enabled = False



        btnClearListBox.Location = New Point(14, 530)
        btnClearListBox.Width = 70
        btnClearListBox.Height = 30
        btnClearListBox.Text = "Clear"
        Me.Controls.Add(btnClearListBox)
        AddHandler btnClearListBox.Click, AddressOf btnClearDebugList_Click
        btnPauseListBox.Location = New Point(90, 530)
        btnPauseListBox.Width = 307
        btnPauseListBox.Height = 30
        btnPauseListBox.Text = "Pause Debugger"
        Me.Controls.Add(btnPauseListBox)
        AddHandler btnPauseListBox.Click, AddressOf btnPauseDebugList_Click
        btnSaveListBox.Location = New Point(403, 530)
        btnSaveListBox.Width = 70
        btnSaveListBox.Height = 30
        btnSaveListBox.Text = "Save"
        Me.Controls.Add(btnSaveListBox)
        AddHandler btnSaveListBox.Click, AddressOf btnSaveDebugList_Click

        btnClearListBox.Visible = False
        btnPauseListBox.Visible = False
        btnSaveListBox.Visible = False
        btnSeperateWin.Visible = False

        DebuLogIsActive = False

        grpSlider.Enabled = False

        blinkGet1.Visible = False
        blinkGet2.Visible = False
        blinkGet3.Visible = False

        TcpIpGpsGate.Visible = True

        INIPath = Path.GetDirectoryName(Application.ExecutablePath) & "\"
        'MessageBox.Show(INIPath)

        'Create a new IniFile object.
        INI = New IniFile(INIPath & "Cmds.lst")

        If File.Exists(INIPath & "Cmds.lst") = False Then
            INI.WriteValue("Cmds", "", "")
            INI.WriteValue("Info", "", "")
        End If

        btnTools.Image = ResizeImage(My.Resources.tools)
        btnTools.ImageAlign = ContentAlignment.MiddleLeft
        btnTools.TextAlign = ContentAlignment.MiddleRight
        btnTools.Text = "Tools"


        If CheckIfRunning("riderunner") = True Then
            FrontEndName = "RR"
        ElseIf (CheckIfRunning("icards") = True) Then
            FrontEndName = "iCarDS"
        Else
            FrontEndName = " ... "
        End If

        btnMinimize.Text = "Minimize " & FrontEndName
        btnRestartRR.Text = "- Start - " & FrontEndName
        btnScreen.Text = FrontEndName & " Screen"

        ToolTip1.SetToolTip(btnRestartRR, "Restart " & FrontEndName)
        ToolTip1.SetToolTip(btnReloadScreen, "Reload Screen")
        ToolTip1.SetToolTip(btnReloadSkin, "Reload Skin")
        ToolTip1.SetToolTip(btnReloadExcTBL, "Reload ExecTBL")
        ToolTip1.SetToolTip(btnMinimize, "Minimize " & FrontEndName)
        ToolTip1.SetToolTip(btnOpenScreen, "Edit Screen")
        ToolTip1.SetToolTip(btnScreen, "Screen")
        ToolTip1.SetToolTip(btnTools, "Tools")

        ToolTip1.SetToolTip(btnOpenScreenInList, "Edit Screen from list")

        UpdateComboList()
        UpdateLabelList()

        caretIndex = txtBoxDebugLog.SelectionStart
        lineNumber = txtBoxDebugLog.GetLineFromCharIndex(caretIndex)

        Main.Start()
    End Sub
    Private Sub SkinToolDS_FormClosing(sender As System.Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        TcpIpGPS.CloseCheckGPS()

        If IsNothing(SDK) = False Then
            'SDK.CloseAllDocuments() 'SDK = Nothing
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(SDK)
            SDKIsReady = False
            SDK = Nothing
        End If

        If IsNothing(Main) = False Then
            Main.Stop()
        End If

        btnCommands.Enabled = False
        btnSendCommand1.Enabled = False
        btnSendCommand2.Enabled = False
        btnSendCommand3.Enabled = False
        btnSendCommand4.Enabled = False


        If Me.WindowState = FormWindowState.Normal Then
            My.Settings.LastPosition = Me.Location
            My.Settings.LastPositionDebug = debugFrm.Location
            My.Settings.LastSizeDebug = debugFrm.Size
        End If

        My.Settings.Save()

    End Sub


    Private Sub chkOnTop_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkOnTop.Click
        If chkOnTop.Checked = True Then
            Me.TopMost = True
        Else
            Me.TopMost = False
        End If
    End Sub

#Region "Buttons Top"
    Private Sub btnCommands_Click(sender As System.Object, e As System.EventArgs) Handles btnCommands.Click
        lblInfo.Text = "Enter any command that iCarDS understands, such as AUDIO to go to the music screen"
        btnCommands.Enabled = False
        btnLiveDebug.Enabled = True
        txtBoxDebugLog.Visible = False

        btnScreen.Visible = True

        grpSlider.Visible = True
        btnTools.Visible = True
        btnEditCmds.Visible = True

        btnClearListBox.Visible = False
        btnPauseListBox.Visible = False
        btnSaveListBox.Visible = False
        btnSeperateWin.Visible = False
    End Sub
    Private Sub btnLiveDebug_Click(sender As System.Object, e As System.EventArgs) Handles btnLiveDebug.Click
        DebuLogIsActive = True
        lblInfo.Text = "To copy a line to the clipboard: Select Line > Right Click > To Clipboard"
        btnLiveDebug.Enabled = False
        btnCommands.Enabled = True

        txtBoxDebugLog.Visible = True
        txtBoxDebugLog.Location = New Point(6, 58)
        txtBoxDebugLog.Height = 460
        txtBoxDebugLog.Width = 470

        groupScreen.Visible = False
        grpSlider.Visible = False
        btnEditCmds.Visible = False

        btnClearListBox.Visible = True
        btnPauseListBox.Visible = True
        btnSaveListBox.Visible = True
        btnSeperateWin.Visible = True

        btnScreen.Visible = False
        btnTools.Visible = False
    End Sub

    Private Sub btnRRFiles_Click(sender As System.Object, e As System.EventArgs) Handles btnRRFiles.Click

        If selFile.Visible = False Then
            selFile.Show()
            selFile.Location = New Point(Me.Location.X + 215, Me.Location.Y + 60)
        Else
            selFile.Hide()
        End If
    End Sub
#End Region

#Region "Send Commands"
    Private Sub btnSendCommand1_Click(sender As System.Object, e As System.EventArgs) Handles btnSendCommand1.Click
        If cmbCommand1.Text <> "" Then
            Try
                Dim t As New Threading.Thread(Sub() SendAndSave(cmbCommand1.Text))
                t.Start()
                'INI.Write("Cmds", cmbCommand1.Text, "")
            Catch ex As Exception
                'MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub
    Private Sub SendAndSave(cmd As String)
        SDK.Execute(cmd)

        If cmd.Contains("==") Then cmd = cmd.Replace("==", "++")

        INI.WriteValue("Cmds", cmd, "")
        UpdateComboList()
    End Sub

    Private Sub SaveGet(getvalue As String)
        INI.WriteValue("Info", getvalue, "")
        'UpdateLabelList()
    End Sub

    Private Sub btnSendCommand2_Click(sender As System.Object, e As System.EventArgs) Handles btnSendCommand2.Click
        If cmbCommand2.Text <> "" Then
            Try
                Dim t As New Threading.Thread(Sub() SendAndSave(cmbCommand2.Text))
                t.Start()
            Catch ex As Exception
                'MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub
    Private Sub btnSendCommand3_Click(sender As System.Object, e As System.EventArgs) Handles btnSendCommand3.Click
        If cmbCommand3.Text <> "" Then
            Try
                Dim t As New Threading.Thread(Sub() SendAndSave(cmbCommand3.Text))
                t.Start()
            Catch ex As Exception
                'MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub
    Private Sub btnSendCommand4_Click(sender As System.Object, e As System.EventArgs) Handles btnSendCommand4.Click
        If cmbCommand4.Text <> "" Then
            Try
                Dim t As New Threading.Thread(Sub() SendAndSave(cmbCommand4.Text))
                t.Start()
            Catch ex As Exception
                'MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub
#End Region

#Region "Buttons Middle"
    Private Sub btnRestartRR_Click(sender As System.Object, e As System.EventArgs) Handles btnRestartRR.Click
        If SDKIsReady = True Then
            SDK.Execute("RELOADRR")
        End If
    End Sub
    Private Sub btnReloadScreen_Click(sender As System.Object, e As System.EventArgs) Handles btnReloadScreen.Click
        If SDKIsReady = True Then
            SDK.Execute("ReloadScreen")
        End If
    End Sub
    Private Sub btnReloadSkin_Click(sender As System.Object, e As System.EventArgs) Handles btnReloadSkin.Click
        If SDKIsReady = True Then
            SDK.Execute("RELOADSKIN")
        End If
    End Sub
    Private Sub btnReloadExcTBL_Click(sender As System.Object, e As System.EventArgs) Handles btnReloadExcTBL.Click
        If SDKIsReady = True Then
            SDK.Execute("RELOADEXECTBL")
        End If
    End Sub
    Private Sub btnMinimize_Click(sender As System.Object, e As System.EventArgs) Handles btnMinimize.Click
        If SDKIsReady = True Then
            SDK.Execute("MINIMIZE")
        End If
    End Sub
    Private Sub btnOpenScreen_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenScreen.Click
        Try
            'RRPath = SDK.GetInfo("RRPATH")
            'RRScreen = SDK.GetInfo("RRSCREEN")
            'RRSkinPath = SDK.GetInfo("=$SKINPATH$")
            If CheckIfRunning("riderunner") = True Then
                If CheckIfNppExist() = True Then
                    If File.Exists(RRSkinPath & RRScreen) Then System.Diagnostics.Process.Start("notepad++.exe", RRSkinPath & RRScreen)
                Else
                    If File.Exists(RRSkinPath & RRScreen) Then System.Diagnostics.Process.Start("Notepad.Exe", RRSkinPath & RRScreen)
                End If

                My.Settings.SkinPath = RRSkinPath & RRScreen
            ElseIf CheckIfRunning("icards") = True Then
                If CheckIfNppExist() = True Then
                    If File.Exists(RRSkinPath & RRScreen & ".skin") Then System.Diagnostics.Process.Start("notepad++.exe", RRSkinPath & RRScreen & ".skin")
                Else
                    If File.Exists(RRSkinPath & RRScreen & ".skin") Then System.Diagnostics.Process.Start("Notepad.Exe", RRSkinPath & RRScreen & ".skin")
                End If

                My.Settings.SkinPath = RRSkinPath & RRScreen & ".skin"

            End If
            My.Settings.Save()
        Catch ex As Exception
            lblScreen.Text = "Screen: < No Data found >"
        End Try
    End Sub

#End Region

#Region "Buttons Live Debug"
    Private Sub btnClearDebugList_Click()
        txtBoxDebugLog.Clear()
    End Sub
    Private Sub btnPauseDebugList_Click()
        MessageBox.Show("Feature not ready")
    End Sub
    Private Sub btnSaveDebugList_Click()
        Dim FILE_NAME As String = Application.StartupPath & "SkinToolDS.log"
        'If System.IO.File.Exists(FILE_NAME) = True Then
        Dim objWriter As New System.IO.StreamWriter(FILE_NAME, True, Encoding.Unicode)
        objWriter.Write(txtBoxDebugLog.Text)
        objWriter.Close()
        MessageBox.Show("Profile: Debug file saved !")
        'Else
        'lblProfile.Text = "Profile: -"
        'End If

    End Sub
    Private Sub btnSeperateWin_Click(sender As System.Object, e As System.EventArgs) Handles btnSeperateWin.Click
        'debugFrm = New DebugForm
        debugFrm.Show()
        debugFrm.Location = New Point(My.Settings.LastPositionDebug.X, My.Settings.LastPositionDebug.Y)
        debugFrm.Size = My.Settings.LastSizeDebug
        btnCommands.PerformClick()
    End Sub
#End Region

#Region "Timers"
    Private Sub Main_Tick(sender As System.Object, e As System.EventArgs) Handles Main.Tick
        Try
            If CheckIfRunning("riderunner") = True Then
                FrontEndName = "RR"
                If IsNothing(SDK) = True Then
                    SDK = CreateObject("RideRunner.SDK")
                    SDKIsReady = True
                End If

                lblScreen.Text = "Screen: " & SDK.GetInfo("RRSCREEN")
                lblProfile.Text = "RideRunner"
            ElseIf CheckIfRunning("icards") = True Then
                FrontEndName = "iCarDS"
                If IsNothing(SDK) = True Then
                    SDK = CreateObject("RideRunner.SDK")
                    SDKIsReady = True
                End If

                lblScreen.Text = "Screen: " & SDK.GetInfo("RRSCREEN") & ".skin"
                lblProfile.Text = "Profile: iCarDS"
            Else
                SDKIsReady = False
                'SDK.CloseAllDocuments()
                'System.Runtime.InteropServices.Marshal.ReleaseComObject(SDK)
                SDK = Nothing
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

        If IsNothing(SDK) = True Then
            If chkUpdate1.Checked = True Then TimerContent1.Enabled = False
            If chkUpdate2.Checked = True Then TimerContent2.Enabled = False
            If chkUpdate3.Checked = True Then TimerContent3.Enabled = False
            Exit Sub
        Else
            TimerContent1.Enabled = True
            TimerContent2.Enabled = True
            TimerContent3.Enabled = True
        End If

        Try
            If SDKIsReady = True Then
                RRPath = SDK.GetInfo("RRPATH")
                RRScreen = SDK.GetInfo("RRSCREEN")
                RRSkinPath = SDK.GetInfo("=$SKINPATH$")

                If My.Settings.RRPath <> SDK.GetInfo("RRPATH") Or My.Settings.SkinPath <> SDK.GetInfo("=$SKINPATH$") Then
                    My.Settings.RRPath = SDK.GetInfo("RRPATH")
                    My.Settings.SkinPath = SDK.GetInfo("=$SKINPATH$")
                    My.Settings.Save()
                End If

                btnSendCommand1.Enabled = True
                btnSendCommand2.Enabled = True
                btnSendCommand3.Enabled = True
                btnSendCommand4.Enabled = True
                btnMinimize.Text = "Minimize " & FrontEndName
                btnRestartRR.Text = "- Start - " & FrontEndName
                btnScreen.Text = FrontEndName & " Screen"
            Else
                btnSendCommand1.Enabled = False
                btnSendCommand2.Enabled = False
                btnSendCommand3.Enabled = False
                btnSendCommand4.Enabled = False
                lblScreen.Text = "Screen: -"
                lblProfile.Text = "Profile: -"
                btnMinimize.Text = "Minimize " & "..."
                btnRestartRR.Text = "- Start - " & "..."
                btnScreen.Text = "..." & " Screen"
            End If
            If radGet1Var.Checked = True Or radGet1Lab.Checked = True Or radGet1Ind.Checked = True Then cmbGet1.Enabled = True : chkUpdate1.Enabled = True Else cmbGet1.Enabled = False : chkUpdate1.Enabled = False
            If radGet2Var.Checked = True Or radGet2Lab.Checked = True Or radGet2Ind.Checked = True Then cmbGet2.Enabled = True : chkUpdate2.Enabled = True Else cmbGet2.Enabled = False : chkUpdate2.Enabled = False
            If radGet3Var.Checked = True Or radGet3Lab.Checked = True Or radGet3Ind.Checked = True Then cmbGet3.Enabled = True : chkUpdate3.Enabled = True Else cmbGet3.Enabled = False : chkUpdate3.Enabled = False

            'GpsGate TcpIp connection
            If TcpIpGPS.CheckGPS() = True Then
                TcpIpGpsGate.Image = My.Resources.GpsGate_On
                ToolTip1.SetToolTip(TcpIpGpsGate, FrontEndName & " is connected to GpsGate through the port 20176")
            Else
                TcpIpGpsGate.Image = My.Resources.GpsGate_Off
                ToolTip1.SetToolTip(TcpIpGpsGate, FrontEndName & " receive nothing from GpsGate through the port 20176")
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.InnerException.ToString)
        End Try

    End Sub

    Private Sub TimerContent1_Tick(sender As System.Object, e As System.EventArgs) Handles TimerContent1.Tick
        Try
            If IsNothing(SDK) = False Then
                Dim t As New Threading.Thread(AddressOf btnGet1.PerformClick)
                t.Start()
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub TimerContent2_Tick(sender As System.Object, e As System.EventArgs) Handles TimerContent2.Tick
        Try
            If IsNothing(SDK) = False Then
                Dim t As New Threading.Thread(AddressOf btnGet2.PerformClick)
                t.Start()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TimerContent3_Tick(sender As System.Object, e As System.EventArgs) Handles TimerContent3.Tick
        Try
            If IsNothing(SDK) = False Then
                Dim t As New Threading.Thread(AddressOf btnGet3.PerformClick)
                t.Start()
            End If
        Catch ex As Exception

        End Try

    End Sub
#End Region

#Region "Get Contents 1"
    Private Sub radGet1Var_Click(sender As System.Object, e As System.EventArgs) Handles radGet1Var.Click
        If radGet1Lab.Checked = True Then radGet1Lab.Checked = False
        If radGet1Ind.Checked = True Then radGet1Ind.Checked = False
    End Sub
    Private Sub radGet1Lab_Click(sender As System.Object, e As System.EventArgs) Handles radGet1Lab.Click
        If radGet1Var.Checked = True Then radGet1Var.Checked = False
        If radGet1Ind.Checked = True Then radGet1Ind.Checked = False
    End Sub
    Private Sub radGet1Ind_Click(sender As System.Object, e As System.EventArgs) Handles radGet1Ind.Click
        If radGet1Var.Checked = True Then radGet1Var.Checked = False
        If radGet1Lab.Checked = True Then radGet1Lab.Checked = False
    End Sub
    Private Sub btnGet1_Click(sender As System.Object, e As System.EventArgs) Handles btnGet1.Click
        If SDK IsNot Nothing Then
            If radGet1Var.Checked = True Then
                If Not cmbGet1.Text.StartsWith("=$") Then
                    Dim t As New Threading.Thread(Sub() txtBoxGet1.Text = SDK.GetInfo("=$" & cmbGet1.Text & "$"))
                    t.Start()
                End If

            ElseIf radGet1Lab.Checked = True Then
                If cmbGet1.Text.ToLower = "rrscreen" Then
                    Dim t As New Threading.Thread(Sub() txtBoxGet1.Text = SDK.GetInfo(cmbGet1.Text) & ".skin")
                    t.Start()
                Else
                    Dim t As New Threading.Thread(Sub() txtBoxGet1.Text = SDK.GetInfo(cmbGet1.Text))
                    t.Start()
                End If
            ElseIf radGet1Ind.Checked = True Then
                Dim t As New Threading.Thread(Sub() txtBoxGet1.Text = SDK.GetInd(cmbGet1.Text))
                t.Start()
            End If
        End If

        Dim u As New Threading.Thread(Sub() SaveGet(cmbGet1.Text))
        u.Start()
    End Sub
    Private Sub chkUpdate1_Click(sender As System.Object, e As System.EventArgs) Handles chkUpdate1.Click
        If chkUpdate1.Checked = True Then
            If IsNothing(SDK) = False Then TimerContent1.Start()
        Else
            TimerContent1.Stop()
        End If
    End Sub
#End Region

#Region "Get Contents 2"
    Private Sub radGet2Var_Click(sender As System.Object, e As System.EventArgs) Handles radGet2Var.Click
        If radGet2Lab.Checked = True Then radGet2Lab.Checked = False
        If radGet2Ind.Checked = True Then radGet2Ind.Checked = False
    End Sub
    Private Sub radGet2Lab_Click(sender As System.Object, e As System.EventArgs) Handles radGet2Lab.Click
        If radGet2Var.Checked = True Then radGet2Var.Checked = False
        If radGet2Ind.Checked = True Then radGet2Ind.Checked = False
    End Sub
    Private Sub radGet2Ind_Click(sender As System.Object, e As System.EventArgs) Handles radGet2Ind.Click
        If radGet2Var.Checked = True Then radGet2Var.Checked = False
        If radGet2Lab.Checked = True Then radGet2Lab.Checked = False
    End Sub
    Private Sub btnGet2_Click(sender As System.Object, e As System.EventArgs) Handles btnGet2.Click
        If SDK IsNot Nothing Then
            If radGet2Var.Checked = True Then
                If Not cmbGet2.Text.StartsWith("=$") Then
                    Dim t As New Threading.Thread(Sub() txtBoxGet2.Text = SDK.GetInfo("=$" & cmbGet2.Text & "$"))
                    t.Start()
                End If
            ElseIf radGet2Lab.Checked = True Then
                If cmbGet2.Text.ToLower = "rrscreen" Then
                    Dim t As New Threading.Thread(Sub() txtBoxGet2.Text = SDK.GetInfo(cmbGet2.Text) & ".skin")
                    t.Start()
                Else
                    Dim t As New Threading.Thread(Sub() txtBoxGet2.Text = SDK.GetInfo(cmbGet2.Text))
                    t.Start()
                End If
            ElseIf radGet2Ind.Checked = True Then
                Dim t As New Threading.Thread(Sub() txtBoxGet2.Text = SDK.GetInd(cmbGet2.Text))
                t.Start()
            End If
        End If

        Dim u As New Threading.Thread(Sub() SaveGet(cmbGet2.Text))
        u.Start()
    End Sub
    Private Sub chkUpdate2_Click(sender As System.Object, e As System.EventArgs) Handles chkUpdate2.Click
        If chkUpdate2.Checked = True Then
            If IsNothing(SDK) = False Then TimerContent2.Start()
        Else
            TimerContent2.Stop()
        End If
    End Sub

#End Region

#Region "Get Contents 3"
    Private Sub radGet3Var_Click(sender As System.Object, e As System.EventArgs) Handles radGet3Var.Click
        If radGet1Lab.Checked = True Then radGet1Lab.Checked = False
        If radGet1Ind.Checked = True Then radGet1Ind.Checked = False
    End Sub
    Private Sub radGet3Lab_Click(sender As System.Object, e As System.EventArgs) Handles radGet3Lab.Click
        If radGet1Var.Checked = True Then radGet1Var.Checked = False
        If radGet1Ind.Checked = True Then radGet1Ind.Checked = False
    End Sub
    Private Sub radGet3Ind_Click(sender As System.Object, e As System.EventArgs) Handles radGet3Ind.Click
        If radGet1Var.Checked = True Then radGet1Var.Checked = False
        If radGet1Lab.Checked = True Then radGet1Lab.Checked = False
    End Sub
    Private Sub btnGet3_Click(sender As System.Object, e As System.EventArgs) Handles btnGet3.Click
        If SDK IsNot Nothing Then
            If radGet3Var.Checked = True Then
                If Not cmbGet3.Text.StartsWith("=$") Then
                    Dim t As New Threading.Thread(Sub() txtBoxGet3.Text = SDK.GetInfo("=$" & cmbGet3.Text & "$"))
                    t.Start()
                End If
            ElseIf radGet3Lab.Checked = True Then
                If cmbGet3.Text.ToLower = "rrscreen" Then
                    Dim t As New Threading.Thread(Sub() txtBoxGet3.Text = SDK.GetInfo(cmbGet3.Text) & ".skin")
                    t.Start()
                Else
                    Dim t As New Threading.Thread(Sub() txtBoxGet3.Text = SDK.GetInfo(cmbGet3.Text))
                    t.Start()
                End If
            ElseIf radGet3Ind.Checked = True Then
                Dim t As New Threading.Thread(Sub() txtBoxGet3.Text = SDK.GetInd(cmbGet3.Text))
                t.Start()
            End If
        End If

        Dim u As New Threading.Thread(Sub() SaveGet(cmbGet3.Text))
        u.Start()
    End Sub

    Private Sub chkUpdate3_Click(sender As System.Object, e As System.EventArgs) Handles chkUpdate3.Click
        If chkUpdate3.Checked = True Then
            If IsNothing(SDK) = False Then TimerContent3.Start()
        Else
            TimerContent3.Stop()
        End If
    End Sub
#End Region



    Private Sub UpdateComboList()
        'Get the list of sections in the INI file.
        Dim sectionNames() As String = INI.GetSectionNames()
        'Get the list of keys in the first section.
        arrayCombo = INI.GetKeyNames(sectionNames(0))

        cmbCommand1.Items.Clear()
        cmbCommand2.Items.Clear()
        cmbCommand3.Items.Clear()
        cmbCommand4.Items.Clear()

        For Each line In arrayCombo
            'MessageBox.Show(line)
            If line <> "=" And Mid(line, 1, 1) <> "=" Then
                line = line.Replace("=", "")
                If line.Contains("++") Then line = line.Replace("++", "==")
                cmbCommand1.Items.Add(line)
                cmbCommand2.Items.Add(line)
                cmbCommand3.Items.Add(line)
                cmbCommand4.Items.Add(line)
            End If
            'MessageBox.Show(line)
        Next

        Dim arCb As New ArrayList
        ' Add all items from the ComboBox to the ArrayList.
        arCb.AddRange(cmbCommand1.Items)
        ' Reverse items.
        arCb.Reverse()
        ' Remove all items from ComboBox.
        cmbCommand1.Items.Clear()
        cmbCommand2.Items.Clear()
        cmbCommand3.Items.Clear()
        cmbCommand4.Items.Clear()

        For Index As Integer = 0 To arCb.Count - 1
            ' Add reversed items again to the ComboBox
            cmbCommand1.Items.Add(arCb.Item(Index))
            cmbCommand2.Items.Add(arCb.Item(Index))
            cmbCommand3.Items.Add(arCb.Item(Index))
            cmbCommand4.Items.Add(arCb.Item(Index))
        Next
        ' Clear List for later use.
        arCb.Clear()

    End Sub
    Private Sub UpdateLabelList()
        'Get the list of sections in the INI file.
        Dim sectionNames() As String = INI.GetSectionNames()
        'Get the list of keys in the first section.
        arrayLabel = INI.GetKeyNames(sectionNames(1))
        cmbGet1.Items.Clear()
        cmbGet2.Items.Clear()
        cmbGet3.Items.Clear()

        For Each line In arrayLabel
            'MessageBox.Show(line)
            If line <> "=" And Mid(line, 1, 1) <> "=" Then
                line = line.Replace("=", "")
                cmbGet1.Items.Add(line)
                cmbGet2.Items.Add(line)
                cmbGet3.Items.Add(line)
            End If

        Next

        Dim arCb As New ArrayList
        ' Add all items from the ComboBox to the ArrayList.
        arCb.AddRange(cmbGet1.Items)
        ' Reverse items.
        arCb.Reverse()
        ' Remove all items from ComboBox.
        cmbGet1.Items.Clear()
        cmbGet2.Items.Clear()
        cmbGet3.Items.Clear()

        For Index As Integer = 0 To arCb.Count - 1
            ' Add reversed items again to the ComboBox
            cmbGet1.Items.Add(arCb.Item(Index))
            cmbGet2.Items.Add(arCb.Item(Index))
            cmbGet3.Items.Add(arCb.Item(Index))
        Next
        ' Clear List for later use.
        arCb.Clear()
    End Sub

#Region "Gestion auto with des combobox"
    'Private Sub cmbCommand1_DropDown(sender As Object, e As System.EventArgs) Handles cmbCommand1.DropDown
    '    Dim senderComboBox As ComboBox = DirectCast(sender, ComboBox)
    '    Dim width As Integer = senderComboBox.DropDownWidth
    '    Dim g As Graphics = senderComboBox.CreateGraphics()
    '    Dim font As Font = senderComboBox.Font
    '    Dim vertScrollBarWidth As Integer = If((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0)

    '    Dim newWidth As Integer
    '    For Each s As String In DirectCast(sender, ComboBox).Items
    '        newWidth = CInt(g.MeasureString(s, font).Width) + vertScrollBarWidth
    '        If width < newWidth Then
    '            width = newWidth
    '        End If
    '    Next
    '    senderComboBox.DropDownWidth = width
    'End Sub
    'Private Sub cmbCommand2_DropDown(sender As Object, e As System.EventArgs) Handles cmbCommand2.DropDown
    '    Dim senderComboBox As ComboBox = DirectCast(sender, ComboBox)
    '    Dim width As Integer = senderComboBox.DropDownWidth
    '    Dim g As Graphics = senderComboBox.CreateGraphics()
    '    Dim font As Font = senderComboBox.Font
    '    Dim vertScrollBarWidth As Integer = If((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0)

    '    Dim newWidth As Integer
    '    For Each s As String In DirectCast(sender, ComboBox).Items
    '        newWidth = CInt(g.MeasureString(s, font).Width) + vertScrollBarWidth
    '        If width < newWidth Then
    '            width = newWidth
    '        End If
    '    Next
    '    senderComboBox.DropDownWidth = width
    'End Sub
    'Private Sub cmbCommand3_DropDown(sender As Object, e As System.EventArgs) Handles cmbCommand3.DropDown
    '    Dim senderComboBox As ComboBox = DirectCast(sender, ComboBox)
    '    Dim width As Integer = senderComboBox.DropDownWidth
    '    Dim g As Graphics = senderComboBox.CreateGraphics()
    '    Dim font As Font = senderComboBox.Font
    '    Dim vertScrollBarWidth As Integer = If((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0)

    '    Dim newWidth As Integer
    '    For Each s As String In DirectCast(sender, ComboBox).Items
    '        newWidth = CInt(g.MeasureString(s, font).Width) + vertScrollBarWidth
    '        If width < newWidth Then
    '            width = newWidth
    '        End If
    '    Next
    '    senderComboBox.DropDownWidth = width
    'End Sub
    'Private Sub cmbCommand4_DropDown(sender As Object, e As System.EventArgs) Handles cmbCommand4.DropDown
    '    Dim senderComboBox As ComboBox = DirectCast(sender, ComboBox)
    '    Dim width As Integer = senderComboBox.DropDownWidth
    '    Dim g As Graphics = senderComboBox.CreateGraphics()
    '    Dim font As Font = senderComboBox.Font
    '    Dim vertScrollBarWidth As Integer = If((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0)

    '    Dim newWidth As Integer
    '    For Each s As String In DirectCast(sender, ComboBox).Items
    '        newWidth = CInt(g.MeasureString(s, font).Width) + vertScrollBarWidth
    '        If width < newWidth Then
    '            width = newWidth
    '        End If
    '    Next
    '    senderComboBox.DropDownWidth = width
    'End Sub
#End Region

    Private Sub btnTools_Click(sender As System.Object, e As System.EventArgs) Handles btnTools.Click
        If selTools.Visible = False Then
            selTools.Show()
            selTools.Location = New Point(Me.Location.X + 165, Me.Location.Y + 237)
        Else
            selTools.Hide()
        End If
    End Sub


#Region "Hold button"
    'Private Sub Button1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnScreen.MouseDown
    '    If e.Button = Windows.Forms.MouseButtons.Left Then
    '        sWatch.Start()
    '    End If
    'End Sub

    'Private Sub Button1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnScreen.MouseUp
    '    If e.Button = Windows.Forms.MouseButtons.Left Then
    '        sWatch.Stop()
    '        If sWatch.Elapsed.Seconds >= 2 Then
    '            'TextBox1.Text = ""
    '        Else
    '            'If String.IsNullOrEmpty(TextBox1.Text) Then
    '            '    TextBox1.Text = "5"
    '            'Else
    '            '    TextBox1.Text = Integer.Parse(TextBox1.Text) + 5
    '            'End If
    '        End If
    '        sWatch.Reset()
    '    End If
    'End Sub


#End Region

    Private Sub btnOpenScreenInList_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenScreenInList.Click
        'Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.Title = "Select a Screen File"
        openFileDialog1.InitialDirectory = My.Settings.SkinPath
        openFileDialog1.Filter = "Ini Files|*.ini|Screen Files|*.skin|Text Files|*.txt"
        openFileDialog1.FilterIndex = 2 'here select *.skin
        openFileDialog1.RestoreDirectory = False
        openFileDialog1.Multiselect = True
        'openFileDialog1.ShowHelp = True
        'openFileDialog1.FileName = "toto"

        ' Set validate names and check file exists to false otherwise windows will
        ' not let you select "Folder Selection."
        'openFileDialog1.ValidateNames = False
        'openFileDialog1.CheckFileExists = False
        'openFileDialog1.CheckPathExists = True

        ' Show the Dialog.
        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                'myStream = openFileDialog1.OpenFile()

                'If (myStream IsNot Nothing) Then
                ' Insert code to read the stream here.
                If CheckIfNppExist() = True Then
                    System.Diagnostics.Process.Start("notepad++.exe", openFileDialog1.FileName)
                Else
                    System.Diagnostics.Process.Start("Notepad.Exe", openFileDialog1.FileName)
                End If
                'End If
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                ' Check this again, since we need to make sure we didn't throw an exception on open.
                'If (myStream IsNot Nothing) Then
                '    myStream.Close()
                'End If
            End Try


        End If
    End Sub

    Private Sub btnScreen_Click(sender As System.Object, e As System.EventArgs) Handles btnScreen.Click

        Try
            Dim readText() As String, newScreenPath As String = ""
            If FrontEndName = "iCarDS" Then
                newScreenPath = RRSkinPath & RRScreen & ".skin"
            ElseIf FrontEndName = "RR" Then
                newScreenPath = RRSkinPath & RRScreen
            End If

            My.Settings.SkinPath = newScreenPath
            My.Settings.Save()

            readText = File.ReadAllLines(newScreenPath)
            'MessageBox.Show(newScreenPath)

            groupScreen.Visible = Not groupScreen.Visible

            groupScreen.Location = New Point(6, 219)
            groupScreen.Width = 479
            groupScreen.Height = 367

            If groupScreen.Visible = True Then
                If SDKIsReady = True Then
                    'MessageBox.Show(RRSkinPath)
                    If readText(2).Contains(",") Then
                        Dim screenArray() As String = readText(2).Split(",")
                        'MessageBox.Show(My.Settings.SkinPath & screenArray(0))
                        screenPictureBox1.Image = Image.FromFile(RRSkinPath & screenArray(0))
                        screenPictureBox2.Image = Image.FromFile(RRSkinPath & screenArray(1))
                        screenPictureBox3.Image = Image.FromFile(RRSkinPath & screenArray(2))
                        screenPictureBox4.Image = Image.FromFile(RRSkinPath & screenArray(3))

                        lblPictureBox1.Text = screenArray(0)
                        lblPictureBox2.Text = screenArray(1)
                        lblPictureBox3.Text = screenArray(2)
                        lblPictureBox4.Text = screenArray(3)
                    Else
                        MessageBox.Show("Screen info not found !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                Else
                    MessageBox.Show("SDK not ready", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    

    End Sub

    Private Sub btn1PainNET_Click(sender As System.Object, e As System.EventArgs) Handles btn1PainNET.Click
        Try
            If File.Exists("C:\Program Files\Paint.NET\PaintDotNet.exe") = True Then
                System.Diagnostics.Process.Start("C:\Program Files\Paint.NET\PaintDotNet.exe", RRSkinPath & lblPictureBox1.Text)
            Else
                MessageBox.Show("Paint.Net isn't installed in the path:" & vbCrLf & "C:\Program Files\Paint.NET\PaintDotNet.exe ", "Paint.NET error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btn2PainNET_Click(sender As System.Object, e As System.EventArgs) Handles btn2PainNET.Click
        Try
            If File.Exists("C:\Program Files\Paint.NET\PaintDotNet.exe") = True Then
                System.Diagnostics.Process.Start("C:\Program Files\Paint.NET\PaintDotNet.exe", RRSkinPath & lblPictureBox2.Text)
            Else
                MessageBox.Show("Paint.Net isn't installed in the path:" & vbCrLf & "C:\Program Files\Paint.NET\PaintDotNet.exe ", "Paint.NET error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btn3PainNET_Click(sender As System.Object, e As System.EventArgs) Handles btn3PainNET.Click
        Try
            If File.Exists("C:\Program Files\Paint.NET\PaintDotNet.exe") = True Then
                System.Diagnostics.Process.Start("C:\Program Files\Paint.NET\PaintDotNet.exe", RRSkinPath & lblPictureBox3.Text)
            Else
                MessageBox.Show("Paint.Net isn't installed in the path:" & vbCrLf & "C:\Program Files\Paint.NET\PaintDotNet.exe ", "Paint.NET error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btn4PainNET_Click(sender As System.Object, e As System.EventArgs) Handles btn4PainNET.Click
        Try
            If File.Exists("C:\Program Files\Paint.NET\PaintDotNet.exe") = True Then
                System.Diagnostics.Process.Start("C:\Program Files\Paint.NET\PaintDotNet.exe", RRSkinPath & lblPictureBox4.Text)
            Else
                MessageBox.Show("Paint.Net isn't installed in the path:" & vbCrLf & "C:\Program Files\Paint.NET\PaintDotNet.exe ", "Paint.NET error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    'Private Sub EmbedButtonInPictureBox(ByVal btn As Button, ByVal pbx As PictureBox)
    '    Dim buttonLocation As Point = pbx.PointToClient(Me.PointToScreen(btn.Location))

    '    btn.Parent = pbx
    '    btn.Location = buttonLocation

    '    Dim buttonBackground As New Bitmap(btn.Width, btn.Height)

    '    Using g As Graphics = Graphics.FromImage(buttonBackground)
    '        g.DrawImage(pbx.Image, _
    '                    New Rectangle(0, _
    '                                  0, _
    '                                  buttonBackground.Width, _
    '                                  buttonBackground.Height), _
    '                    btn.Bounds, _
    '                    GraphicsUnit.Pixel)
    '    End Using

    '    btn.BackgroundImage = buttonBackground

    'End Sub

    Private Sub btnEditCmds_Click(sender As System.Object, e As System.EventArgs) Handles btnEditCmds.Click
        btnEditCmds.Text = "Edit  "
        If CheckIfNppExist() = True Then
            System.Diagnostics.Process.Start("notepad++.exe", INIPath & "Cmds.lst")
        Else
            System.Diagnostics.Process.Start("Notepad.Exe", INIPath & "Cmds.lst")
        End If
    End Sub

#Region "CopyData"
    'http://www.codeproject.com/Tips/1017834/How-to-send-data-from-one-process-to-another-in-cs
    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = NativeMethods.WM_COPYDATA Then
            ' Extract the file name
            Dim copyData As NativeMethods.COPYDATASTRUCT = DirectCast(Marshal.PtrToStructure(m.LParam, GetType(NativeMethods.COPYDATASTRUCT)), NativeMethods.COPYDATASTRUCT)
            Dim dataType As Integer = CInt(copyData.dwData)
            If dataType = 2 Then
                Dim fileName As String = Marshal.PtrToStringAnsi(copyData.lpData)

                ' Add the file name to the edit box
                txtBoxDebugLog.AppendText(fileName & Environment.NewLine & "-" & Environment.NewLine)
                'txtBoxDebugLog.AppendText(vbCr & vbLf)

            Else
                'MessageBox.Show(String.Format("Unrecognized data type = {0}.", dataType), "SendMessageDemo", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End If
        Else
            MyBase.WndProc(m)
        End If
    End Sub
#End Region

#Region "TextBox Clipboard"
    Private Sub txtBoxDebugLog_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles txtBoxDebugLog.MouseDown
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

            txtBoxDebugLog.ContextMenu = Contextmenu1

        End If
    End Sub

    'Private Sub CutAction(sender As Object, e As EventArgs)
    '    txtBoxDebugLog.Cut()
    'End Sub

    Private Sub CopyAction(sender As Object, e As EventArgs)
        'Dim objGraphics As Graphics
        'Clipboard.SetData(DataFormats.Rtf, txtBoxDebugLog.SelectedText)
        'Clipboard.Clear()
        txtBoxDebugLog.Copy()
    End Sub

    Private Sub PasteAction(sender As Object, e As EventArgs)
        If Clipboard.ContainsText(TextDataFormat.Rtf) Then
            If IsNothing(SDK) = False Then
                SDK.execute(Clipboard.GetData(DataFormats.Rtf).ToString())
            End If
        End If
        'txtBoxDebugLog.Paste()
    End Sub
#End Region

    Private Sub TimerBlink_Tick(sender As System.Object, e As System.EventArgs) Handles TimerBlink.Tick
        If chkUpdate1.Checked = True Then
            blinkGet1.Visible = Not blinkGet1.Visible
        Else
            blinkGet1.Visible = False
        End If
        If chkUpdate2.Checked = True Then
            blinkGet2.Visible = Not blinkGet2.Visible
        Else
            blinkGet2.Visible = False
        End If
        If chkUpdate3.Checked = True Then
            blinkGet3.Visible = Not blinkGet3.Visible
        Else
            blinkGet3.Visible = False
        End If

    End Sub


    'Private Sub cmbCommand1_Click(sender As System.Object, e As System.EventArgs) Handles cmbCommand1.Click
    '    UpdateComboList()
    'End Sub
    'Private Sub cmbCommand2_Click(sender As System.Object, e As System.EventArgs) Handles cmbCommand2.Click
    '    UpdateComboList()
    'End Sub
    'Private Sub cmbCommand3_Click(sender As System.Object, e As System.EventArgs) Handles cmbCommand3.Click
    '    UpdateComboList()
    'End Sub
    'Private Sub cmbCommand4_Click(sender As System.Object, e As System.EventArgs) Handles cmbCommand4.Click

    'End Sub

    'Private Sub cmbGet1_Click(sender As System.Object, e As System.EventArgs) Handles cmbGet1.Click
    '    cmbGet1.SelectionStart = cmbGet1.Text.Length + 1
    'End Sub
    'Private Sub cmbGet2_Click(sender As System.Object, e As System.EventArgs) Handles cmbGet2.Click
    '    cmbGet2.SelectionStart = cmbGet2.Text.Length + 1
    'End Sub
    'Private Sub cmbGet3_Click(sender As System.Object, e As System.EventArgs) Handles cmbGet3.Click
    '    cmbGet3.SelectionStart = cmbGet3.Text.Length + 1
    'End Sub
End Class

