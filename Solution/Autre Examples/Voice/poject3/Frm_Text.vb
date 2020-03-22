Option Strict Off
Option Explicit On
Friend Class Frm_Text
	Inherits System.Windows.Forms.Form
	Public My_Index As Integer

    Private Sub Frm_Text_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        Frm_Main.Frm_Active = My_Index
        Frm_Main.VoiceDic.Lock()
        RTBox.SelectionStart = 0
        Frm_Main.VoiceDic.TextSelSet(RTBox.SelectionStart, 65536)
        Frm_Main.VoiceDic.TextSet(RTBox.Text, RTBox.SelectionStart, RTBox.SelectionLength, 65536)
        Frm_Main.VoiceDic.Unlock()

    End Sub
	
	Private Sub Frm_Text_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Frm_Main.Frm_Count = Frm_Main.Frm_Count + 1
		If Frm_Main.Frm_Count = 1 Then
			Frm_Main.File_Close.Visible = True
			Frm_Main.File_Save.Visible = True
			Frm_Main.Add_Sub_Commands()
		End If
	End Sub
	
    Private Sub Frm_Text_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
        RTBox.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 300)
        RTBox.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - 650)
    End Sub
	
	Public Sub Load_File(ByRef File_Name As String)
		Me.Text = File_Name
        RTBox.LoadFile(File_Name, RichTextBoxStreamType.PlainText)
	End Sub
	
	Private Sub Frm_Text_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		Frm_Main.Unload_Text_Frm((My_Index))
		Frm_Main.Frm_Count = Frm_Main.Frm_Count - 1
		If Frm_Main.Frm_Count = 0 Then
			Frm_Main.File_Close.Visible = False
			Frm_Main.File_Save.Visible = False
			Frm_Main.Rem_Sub_Commands()
		End If
	End Sub
	
	Public Sub Save_text()
		Dim F_Num As Integer
		F_Num = FreeFile
		FileOpen(F_Num, Me.Text, OpenMode.Output)
		PrintLine(F_Num, RTBox.Text)
		FileClose(F_Num)
	End Sub
	
    Public Sub Voice_commands(ByRef Command As String)
        Dim Temp_RT As Integer
        Select Case UCase(Command)
            Case "SELECT ALL"
                RTBox.SelectionStart = 0
                RTBox.SelectionLength = Len(RTBox.Text)
                RTBox_Click(RTBox, New System.EventArgs())
            Case "SELECT TO END"
                RTBox.SelectionLength = Len(RTBox.Text) - RTBox.SelectionStart
                RTBox_Click(RTBox, New System.EventArgs())
            Case "SELECT TO BEGIN"
                Temp_RT = RTBox.SelectionStart
                RTBox.SelectionStart = 0
                RTBox.SelectionLength = Temp_RT
                RTBox_Click(RTBox, New System.EventArgs())
            Case "SELECT WORD"
                RTBox.SelectionLength = InStr(RTBox.SelectionStart + 1, RTBox.Text, " ", CompareMethod.Binary) - RTBox.SelectionStart
                RTBox_Click(RTBox, New System.EventArgs())
            Case "MOVE TO END"
                RTBox.SelectionStart = Len(RTBox.Text)
                RTBox_Click(RTBox, New System.EventArgs())
            Case "MOVE TO BEGIN"
                RTBox.SelectionStart = 0
                RTBox_Click(RTBox, New System.EventArgs())
            Case "MOVE NEXT WORD"
                RTBox.SelectionStart = InStr(RTBox.SelectionStart + 1, RTBox.Text, " ", CompareMethod.Binary)
                RTBox_Click(RTBox, New System.EventArgs())
        End Select
    End Sub
	
	Private Sub RTBox_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles RTBox.Click
		Frm_Main.VoiceDic.Lock()
		Frm_Main.VoiceDic.TextSelSet(RTBox.SelectionStart, RTBox.SelectionLength)
		Frm_Main.VoiceDic.Unlock()
		
	End Sub
	
	Private Sub RTBox_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles RTBox.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Chr(KeyAscii)
		Frm_Main.VoiceDic.Lock()
		Frm_Main.VoiceDic.TextSelSet(RTBox.SelectionStart, 0)
		Frm_Main.VoiceDic.TextSet(Chr(KeyAscii), RTBox.SelectionStart, RTBox.SelectionLength, 65536)
		Frm_Main.VoiceDic.Unlock()
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
End Class