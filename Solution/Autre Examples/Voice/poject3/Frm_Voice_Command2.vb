Option Strict Off
Option Explicit On
Friend Class Frm_Main
	Inherits System.Windows.Forms.Form
	Public My_menu As Integer
	Public Sub_menu As Integer
	Public Frm_Active As Integer
	Private Loop_1 As Integer
	Private TCount As Integer
    Private Text_Forms() As Frm_Text
	Private Frm_loaded() As Boolean
	Public Frm_Count As Integer
	Public Frm_VC_Vis As Boolean
	
	Public Sub Diction_Listen_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Diction_Listen.Click
		If Diction_Listen.Checked Then
			VoiceDic.Deactivate()
			Diction_Listen.Checked = False ' uncheck the menu item
        Else
            VoiceDic.Activate()
            Diction_Listen.Checked = True ' check the menu item
        End If
    End Sub

    Private Sub Frm_Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' release the usage of the command menu.
        VoiceCmd.CtlEnabled = 0
        TCount = VoiceCmd.get_CountCommands(My_menu)
        For Loop_1 = TCount To 1 Step -1
            VoiceCmd.Remove(My_menu, Loop_1)
        Next Loop_1
        VoiceCmd.ReleaseMenu(My_menu)
        TCount = VoiceCmd.get_CountCommands(Sub_menu)
        For Loop_1 = TCount To 1 Step -1
            VoiceCmd.Remove(Sub_menu, Loop_1)
        Next Loop_1
        VoiceCmd.ReleaseMenu(Sub_menu)
        VoiceDic.Deactivate()
        VoiceDic.Initialized = 0
    End Sub

	Private Sub Frm_Main_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        VoiceCmd.Initialized = 1
        My_menu = VoiceCmd.get_MenuCreate("Main Commands", "commands State", 4)
        VoiceCmd.CtlEnabled = 1
        VoiceCmd.AddCommand(My_menu, 1, "open", "Open File", "listen list", 0, "")
		VoiceCmd.AddCommand(My_menu, 1, "show list", "show voice commands list", "listen list", 0, "")
		VoiceCmd.AddCommand(My_menu, 1, "exit", "Exit App", "listen list", 0, "")
		VoiceCmd.AddCommand(My_menu, 1, "stop listening", "Stop Listen", "listen list", 0, "")
        Sub_menu = VoiceCmd.get_MenuCreate("Sub Commands", "commands State", 4)
		VoiceCmd.AddCommand(Sub_menu, 1, "close", "Close File", "listen list", 0, "")
		VoiceCmd.AddCommand(Sub_menu, 1, "save", "Save File", "listen list", 0, "")
		VoiceCmd.AddCommand(Sub_menu, 1, "select word", "Select till end of word", "listen list", 0, "")
		VoiceCmd.AddCommand(Sub_menu, 1, "select all", "Select all text", "listen list", 0, "")
		VoiceCmd.AddCommand(Sub_menu, 1, "select to end", "Select text to end", "listen list", 0, "")
		VoiceCmd.AddCommand(Sub_menu, 1, "select to begin", "select text to begin", "listen list", 0, "")
		VoiceCmd.AddCommand(Sub_menu, 1, "move to end", "move to last position", "listen list", 0, "")
		VoiceCmd.AddCommand(Sub_menu, 1, "move to begin", "move to begin positon", "listen list", 0, "")
		VoiceCmd.AddCommand(Sub_menu, 1, "move next word", "move to begin of next word", "listen list", 0, "")
		VoiceCmd.AddCommand(Sub_menu, 1, "start diction", "Start Diction", "listen list", 0, "")
		VoiceCmd.AddCommand(Sub_menu, 1, "stop diction", "Stop Diction", "listen list", 0, "")
        VoiceCmd.Activate(My_menu)
        ReDim Text_Forms(0)
		ReDim Frm_loaded(0)
        VoiceDic.Initialized = 1
        VoiceDic.Mode = &H20
        VoiceDic.Activate()
        Voice_Listen_Click(Voice_Listen, New System.EventArgs())
	End Sub
	
	Public Sub File_Close_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles File_Close.Click
		' Code to 'Close' a file
		If Frm_Active <> -1 Then
            Text_Forms(Frm_Active).Close()
		End If
	End Sub
	
	Public Sub File_Open_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles File_Open.Click
		' Code to 'OPEN' a File
		Dim New_Frm As Integer
		CD1Open.FileName = ""
        CD1Open.Filter = "*.txt|*.txt"
		CD1Open.ShowDialog()
		New_Frm = -1
		If (CD1Open.FileName <> "") Then
			For Loop_1 = 0 To UBound(Text_Forms)
				If Not Frm_loaded(Loop_1) Then
					New_Frm = Loop_1
					Exit For
				End If
			Next Loop_1
			If New_Frm = -1 Then
				New_Frm = (UBound(Text_Forms) + 1)
				ReDim Preserve Text_Forms(New_Frm)
				ReDim Preserve Frm_loaded(New_Frm)
			End If
			Text_Forms(New_Frm) = New Frm_Text
            Text_Forms(New_Frm).Show()
			Text_Forms(New_Frm).Load_File((CD1Open.FileName))
			Frm_loaded(New_Frm) = True
			Text_Forms(New_Frm).My_Index = New_Frm
		End If
	End Sub
	
	Public Sub File_Save_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles File_Save.Click
		' Code to 'SAVE' a file
		Text_Forms(Frm_Active).Save_text()
	End Sub
	
	Public Sub Main_Exit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Main_Exit.Click
		' Code to 'EXIT' the application
		Me.Close()
	End Sub
	
    Public Sub Voice_Listen_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Voice_Listen.Click
        If Voice_Listen.Checked Then
            ' Deactivate my list of commands. These will now not show in the
            ' 'What can I say' form of Microsoft Voice apllication
            VoiceCmd.Deactivate(My_menu)
            VoiceCmd.Deactivate(Sub_menu)
            Voice_Listen.Checked = False ' uncheck the menu item
        Else
            ' Activate my list of commands. These will show in the 'What can I say' form of
            ' Microsoft Voice apllication
            VoiceCmd.Activate(My_menu)
            VoiceCmd.Deactivate(Sub_menu)
            Voice_Listen.Checked = True ' check the menu item
        End If
    End Sub
	
	Private Sub Voicecmd_CommandRecognize(ByVal eventSender As System.Object, ByVal eventArgs As AxHSRLib._VcommandEvents_CommandRecognizeEvent) Handles Voicecmd.CommandRecognize
		If Frm_VC_Vis Then
			Frm_VoiceCmd.Detect.Text = eventArgs.Command
		End If
		' One of our listed commands has been spoken
		' Look for it in a list and execute the relavant commands
		Select Case UCase(eventArgs.Command)
			Case "OPEN"
				File_Open_Click(File_Open, New System.EventArgs())
			Case "EXIT"
				Me.Close()
			Case "SAVE"
				File_Save_Click(File_Save, New System.EventArgs())
			Case "CLOSE"
				File_Close_Click(File_Close, New System.EventArgs())
			Case "SHOW LIST"
				Frm_VoiceCmd.Visible = True
			Case "STOP LISTENING"
				Voice_Listen_Click(Voice_Listen, New System.EventArgs())
			Case "START DICTION"
				VoiceDic.Activate()
				Diction_Listen.Checked = True
			Case "STOP DICTION"
				VoiceDic.Deactivate()
				Diction_Listen.Checked = False
			Case Else
				If Not Frm_Active = -1 Then
					Text_Forms(Frm_Active).Voice_commands((UCase(eventArgs.Command)))
				End If
		End Select
	End Sub
	
	Public Sub Add_Sub_Commands()
		VoiceCmd.Activate(Sub_menu)
		If Frm_VC_Vis Then
			Frm_VoiceCmd.Refresh_List()
		End If
	End Sub
	Public Sub Rem_Sub_Commands()
		VoiceCmd.Deactivate(Sub_menu)
		If Frm_VC_Vis Then
			Frm_VoiceCmd.Refresh_List()
		End If
		Frm_Active = -1
	End Sub
	
	Private Sub VoiceCmd_VUMeter(ByVal eventSender As System.Object, ByVal eventArgs As AxHSRLib._VcommandEvents_VUMeterEvent) Handles VoiceCmd.VUMeter
		If Frm_VC_Vis Then
			If Frm_VoiceCmd.VU_Meter.Maximum < eventArgs.Level Then Frm_VoiceCmd.VU_Meter.Maximum = eventArgs.Level
			Frm_VoiceCmd.VU_Meter.Value = eventArgs.Level
		End If
	End Sub
	
	Private Sub VoiceDic_TextChangedEvent(ByVal eventSender As System.Object, ByVal eventArgs As AxDICTLib._VdictEvents_TextChangedEvent) Handles VoiceDic.TextChangedEvent
		Dim newStart As Integer
		Dim newend As Integer
		Dim oldStart As Integer
		Dim oldEnd As Integer
		Dim selstart As Integer
		Dim sellen As Integer
        Dim TextIn As String
        TextIn = vbNull
		If Not Frm_Active = -1 Then
			VoiceDic.Lock()
			On Error GoTo Err_Han
			VoiceDic.GetChanges(newStart, newend, oldStart, oldEnd)
			If (oldStart < oldEnd) Then
				Text_Forms(Frm_Active).RTBox.SelectionStart = oldStart
				Text_Forms(Frm_Active).RTBox.SelectionLength = oldEnd - oldStart
				Text_Forms(Frm_Active).RTBox.SelectedText = ""
			End If
			If (newend > newStart) Then
				Text_Forms(Frm_Active).RTBox.SelectionStart = newStart
				Text_Forms(Frm_Active).RTBox.SelectionLength = 0
                VoiceDic.TextGet(newStart, newend - newStart, TextIn)
                Text_Forms(Frm_Active).RTBox.SelectedText = TextIn
			End If
		End If
		
Err_Han: 
		VoiceDic.Unlock()
    End Sub


	Private Sub VoiceDic_UtteranceBegin(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles VoiceDic.UtteranceBegin
        Text_Forms(Frm_Active).RTBox.Enabled = False
	End Sub
	
	Private Sub VoiceDic_UtteranceEnd(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles VoiceDic.UtteranceEnd
        Text_Forms(Frm_Active).RTBox.Enabled = True
	End Sub
	
	Public Sub Voise_show_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Voise_show.Click
		Frm_VoiceCmd.Visible = True
		Frm_VC_Vis = True
	End Sub
	
	Public Sub Unload_Text_Frm(ByRef Index As Integer)
		Frm_loaded(Index) = False
	End Sub
End Class