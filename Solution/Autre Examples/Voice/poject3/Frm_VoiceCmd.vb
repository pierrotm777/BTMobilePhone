Option Strict Off
Option Explicit On
Friend Class Frm_VoiceCmd
	Inherits System.Windows.Forms.Form
	Private Loop_1 As Integer
	Private TCount As Integer
	
	Private Sub Frm_VoiceCmd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Refresh_List()
        Frm_Main.Frm_VC_Vis = True
        Me.Width = 223
        Me.Height = 187
	End Sub
	
	Public Sub Refresh_List()
		Lst_Commands.Items.Clear()
        Dim Category, Command, Description, Action As String
        Category = vbNull
        Command = vbNull
        Description = vbNull
        Action = vbNull
		Dim Flags As Integer
		TCount = Frm_Main.VoiceCmd.get_CountCommands(Frm_Main.My_menu)
		For Loop_1 = 1 To TCount
            Frm_Main.VoiceCmd.GetCommand(Frm_Main.My_menu, Loop_1, Command, Description, Category, Flags, Action)
            Lst_Commands.Items.Add(Command)
		Next Loop_1
		
		TCount = Frm_Main.VoiceCmd.get_CountCommands(Frm_Main.Sub_menu)
		For Loop_1 = 1 To TCount
            Frm_Main.VoiceCmd.GetCommand(Frm_Main.Sub_menu, Loop_1, Command, Description, Category, Flags, Action)
            Lst_Commands.Items.Add(Command)
		Next Loop_1
	End Sub
	Private Sub Frm_VoiceCmd_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		Frm_Main.Frm_VC_Vis = False
	End Sub
End Class