Option Strict Off
Option Explicit On
Friend Class Frm_Voice_Command
	Inherits System.Windows.Forms.Form
	Private My_menu As Integer
	Private Loop_1 As Integer
    Private TCount As Integer

	Private Sub Cmd_Exit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cmd_Exit.Click
		' Code to 'EXIT' the application
        Me.Close()
    End Sub
	
	Private Sub Cmd_Open_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cmd_Open.Click
		' Code to 'OPEN' a database
		CD1Open.ShowDialog()
		CD1Save.FileName = CD1Open.FileName
		' I've only opened the Common dialog 'ShowOpen' form but you add
		' the rest of the 'Open' code here
	End Sub
	
	Private Sub Cmd_Save_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cmd_Save.Click
		' Code to 'SAVE' a database
		CD1Save.ShowDialog()
		CD1Open.FileName = CD1Save.FileName
		' I've only opened the Common dialog 'ShowSave' form but you add
		' the rest of the 'Save' code here
	End Sub
	
	Private Sub Cmd_Start_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cmd_Start.Click
		If Cmd_Start.Text = "Listen" Then
			' Activate my list of commands. These will show in the 'What can I say' form of
			' Microsoft Voice apllication
			VoiceCmd.Activate(My_menu)
			Cmd_Start.Text = "Stop Listening" 'Change the command text
			Lst_Commands.Enabled = True 'enable the List of commands (Visual)
		Else
			' Deactivate my list of commands. These will now not show in the
			' 'What can I say' form of Microsoft Voice apllication
			VoiceCmd.Deactivate(My_menu)
			Cmd_Start.Text = "Listen" 'Change the command text
			Lst_Commands.Enabled = False 'disable the List of commands (Visual)
		End If
	End Sub
	
	Private Sub Frm_Voice_Command_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		' Dim some variables used for retriving commands from 'VoiceCmd'
        Dim Category, Command, Description, Action As String
		Dim Flags As Integer
        Command = vbNull
        Description = vbNull
        Category = vbNull
        Action = vbNull
        ' Initialize the voice control...
		VoiceCmd.Initialized = 1
		' Create and return a Menu control
		My_menu = VoiceCmd.get_MenuCreate("My Commands", "commands State", 4)
		' Enable our voice control
        VoiceCmd.CtlEnabled = 1
		' Suppress any voice errors that may occur
		'VoiceCmd.SuppressExceptions = 1
		' Load our list of commands into the menu.
		VoiceCmd.AddCommand(My_menu, 1, "white", "white colour", "listen list", 0, "")
		VoiceCmd.AddCommand(My_menu, 1, "red", "red colour", "listen list", 0, "")
		VoiceCmd.AddCommand(My_menu, 1, "green", "green colour", "listen list", 0, "")
		VoiceCmd.AddCommand(My_menu, 1, "blue", "blue colour", "listen list", 0, "")
		VoiceCmd.AddCommand(My_menu, 1, "open", "Open Database", "listen list", 0, "")
		VoiceCmd.AddCommand(My_menu, 1, "save", "Save Database", "listen list", 0, "")
		VoiceCmd.AddCommand(My_menu, 1, "exit", "Exit App", "listen list", 0, "")
		VoiceCmd.AddCommand(My_menu, 1, "stop listening", "Stop Listen", "listen list", 0, "")
		' Activate the List of commands
		VoiceCmd.Activate(My_menu)
		Cmd_Start.Text = "Stop Listening"
        Lst_Commands.Items.Clear()
        'load the commands from the menu in to the list
		TCount = VoiceCmd.get_CountCommands(My_menu)
		For Loop_1 = 1 To TCount
            VoiceCmd.GetCommand(My_menu, Loop_1, Command, Description, Category, Flags, Action)
            Lst_Commands.Items.Add(Command)
		Next Loop_1
	End Sub
	
	Private Sub Frm_Voice_Command_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		' Remove the commands from the menu
		TCount = VoiceCmd.get_CountCommands(My_menu)
		For Loop_1 = TCount To 1 Step -1
			VoiceCmd.Remove(My_menu, Loop_1)
		Next Loop_1
		' release the usage of the command menu.
		VoiceCmd.ReleaseMenu(My_menu)
        VoiceCmd.CtlEnabled = 0
	End Sub
	
	Private Sub Timer1_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Timer1.Tick
		' After a perion of silence reset the last word heard
		Detect.Text = "<Nothing>"
	End Sub
	
	Private Sub VoiceCmd_CommandOther(ByVal eventSender As System.Object, ByVal eventArgs As AxHSRLib._VcommandEvents_CommandOtherEvent) Handles VoiceCmd.CommandOther
		'If commands other than those listed in our menu are heard Display them
		Timer1.Enabled = False
		Detect.Text = eventArgs.Command
		Timer1.Enabled = True
        Timer1.Interval = 2000
	End Sub
	
	Private Sub Voicecmd_CommandRecognize(ByVal eventSender As System.Object, ByVal eventArgs As AxHSRLib._VcommandEvents_CommandRecognizeEvent) Handles Voicecmd.CommandRecognize
		' One of our listed commands has been spoken
		Timer1.Enabled = False
		' Display it.
		Detect.Text = eventArgs.Command
		' Look for it in a list and execute the relavant commands
		Select Case UCase(eventArgs.Command)
			Case "OPEN"
				Cmd_Open_Click(Cmd_Open, New System.EventArgs())
			Case "SAVE"
				Cmd_Save_Click(Cmd_Save, New System.EventArgs())
			Case "EXIT"
				Cmd_Exit_Click(Cmd_Exit, New System.EventArgs())
			Case "STOP LISTENING"
				Cmd_Start_Click(Cmd_Start, New System.EventArgs())
			Case "RED"
				Lst_Commands.BackColor = System.Drawing.ColorTranslator.FromOle(&H101FF)
			Case "GREEN"
				Lst_Commands.BackColor = System.Drawing.ColorTranslator.FromOle(&H1FF01)
			Case "BLUE"
				Lst_Commands.BackColor = System.Drawing.ColorTranslator.FromOle(&HFF0101)
			Case "WHITE"
				Lst_Commands.BackColor = System.Drawing.ColorTranslator.FromOle(&HFFFFFF)
		End Select
		' if we not exiting then reset the timer.
		If Not (UCase(eventArgs.Command) = "EXIT") Then
			Timer1.Enabled = True
			Timer1.Interval = 2000
		End If
	End Sub
	
	Private Sub VoiceCmd_VUMeter(ByVal eventSender As System.Object, ByVal eventArgs As AxHSRLib._VcommandEvents_VUMeterEvent) Handles VoiceCmd.VUMeter
		' This Procedure is called +- every 8 seconds.
		' set the level of out vu meter..
		If VU_Meter.Maximum < eventArgs.Level Then VU_Meter.Maximum = eventArgs.Level
		VU_Meter.Value = eventArgs.Level
	End Sub
End Class