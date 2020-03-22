Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class Main_F
	Inherits System.Windows.Forms.Form
	Dim File_Name As String
	Dim Start_Pos As Integer
	Dim Loop_1 As Integer
	Private Caret As POINTAPI
	
	
	Private Sub DirectSS1_AudioStop(ByVal eventSender As System.Object, ByVal eventArgs As AxACTIVEVOICEPROJECTLib._DirectSSEvents_AudioStopEvent) Handles DirectSS1.AudioStop
		Toolbar1.Buttons(15).Value = ComctlLib.ValueConstants.tbrUnpressed
		Toolbar1.Buttons(15).Image = 14
	End Sub
	
	Public Sub Main_F_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		On Error GoTo Err_han
		
		If (Vdict1.Initialized = 0) Then
			Vdict1.Initialized = 1
			Vdict1.Mode = VSRMODE_DCTONLY
		End If
		
		listen((0))
		
		File_Name = ""
        textReset()
		Toolbar1.ButtonHeight = ImageList1.ImageHeight
		Toolbar1.ButtonWidth = ImageList1.ImageHeight
		Toolbar1.Height = VB6.TwipsToPixelsY(Toolbar1.ButtonHeight)
		RichTextBox1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Toolbar1.Top) + VB6.PixelsToTwipsY(Toolbar1.Height))
		
        CommonDialog1Open.Filter = "Rich Text (*.rtf)|*.rtf|Text (*.txt)|*.txt"
		CommonDialog1Save.Filter = "Rich Text (*.rtf)|*.rtf|Text (*.txt)|*.txt"
		Exit Sub
Err_han: 
		MsgBox("Unable to initialize dictation engine.")
		Me.Close()
	End Sub
	
	Public Sub Addword_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Addword.Click
		On Error GoTo addend
		Vdict1.LexiconDlg(Me.Handle.ToInt32, "Add Word")
addend: 
	End Sub
	
    Private Sub TextReset()
        RichTextBox1.Font = VB6.FontChangeName(RichTextBox1.Font, "Times New Roman")
        RichTextBox1.Font = VB6.FontChangeSize(RichTextBox1.Font, 14)
        SelectAll_Click(SelectAll, New System.EventArgs())
        Delete_Click(Delete, New System.EventArgs())

        RichTextBox1.Font = VB6.FontChangeName(RichTextBox1.Font, "Times New Roman")
        RichTextBox1.Font = VB6.FontChangeSize(RichTextBox1.Font, 14)

    End Sub
	
	Public Sub listening_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles listening.Click
		listening.Checked = Not listening.Checked
		If listening.Checked Then
			listen((1))
		Else
			listen((0))
		End If
	End Sub
	
	Public Sub ReadDocument_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ReadDocument.Click
		Dim readtext As String
		Dim Start As Integer
		On Error Resume Next
		If (DirectSS1.Speaking) Then
			Stopreading_Click(Stopreading, New System.EventArgs())
		Else
			listening.Checked = False
			listen((0))
			Toolbar1.Buttons(15).Value = ComctlLib.ValueConstants.tbrPressed
			Toolbar1.Buttons(15).Image = 13
			Start_Pos = RichTextBox1.SelectionStart
			readtext = VB.Right(RichTextBox1.Text, Len(RichTextBox1.Text) - RichTextBox1.SelectionStart)
			DirectSS1.Speak(readtext)
		End If
	End Sub
	
	Private Sub SetText(ByRef newText As String, ByRef ui As Boolean)
		Vdict1.Lock()
		Vdict1.TextSelSet(RichTextBox1.SelectionStart, 0)
		Vdict1.TextSet(newText, RichTextBox1.SelectionStart, RichTextBox1.SelectionLength, 65536)
		Vdict1.Unlock()
		If (ui) Then
			RichTextBox1.SelectedText = newText
		End If
	End Sub
	
	Private Sub RichTextBox1_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles RichTextBox1.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim s As String
		s = Chr(KeyAscii)
		SetText(s, False)
		ShowCorrectionWindow((showcorrection.Checked))
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub RichTextBox1_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles RichTextBox1.MouseUp
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		Vdict1.Lock()
		Vdict1.TextSelSet(RichTextBox1.SelectionStart, RichTextBox1.SelectionLength)
		Vdict1.Unlock()
		ShowCorrectionWindow((showcorrection.Checked))
	End Sub
	
	
	Public Sub showcorrection_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles showcorrection.Click
		If (showcorrection.Checked) Then
			ShowCorrectionWindow((0))
		Else
			ShowCorrectionWindow((1))
		End If
	End Sub
	
	
	Public Sub Stopreading_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Stopreading.Click
		DirectSS1.AudioReset()
	End Sub
	
	Private Sub Vdict1_AttribChanged(ByVal eventSender As System.Object, ByVal eventArgs As AxDICTLib._VdictEvents_AttribChangedEvent) Handles Vdict1.AttribChanged
		If eventArgs.Attrib And 2 = 2 Then
			If (Vdict1.Mode And VSRMODE_DCTONLY) Then
				ListenUI((1))
			Else
				ListenUI((0))
			End If
		End If
	End Sub
	
	Private Sub Vdict1_TextChangedEvent(ByVal eventSender As System.Object, ByVal eventArgs As AxDICTLib._VdictEvents_TextChangedEvent) Handles Vdict1.TextChangedEvent
		Dim newStart As Integer
		Dim newend As Integer
		Dim oldStart As Integer
		Dim oldEnd As Integer
		Dim selstart As Integer
		Dim sellen As Integer
		Dim theText As String
        theText = vbNull
		Vdict1.Lock()
		
		On Error GoTo spuriouserror
		Vdict1.GetChanges(newStart, newend, oldStart, oldEnd)
		
		If (oldStart < oldEnd) Then
			RichTextBox1.SelectionStart = oldStart
			RichTextBox1.SelectionLength = oldEnd - oldStart
			RichTextBox1.SelectedText = ""
		End If
		
		If (newend > newStart) Then
			
			RichTextBox1.SelectionStart = newStart
			RichTextBox1.SelectionLength = 0
			
			Vdict1.TextGet(newStart, newend - newStart, theText)
			RichTextBox1.SelectedText = theText
		End If
		ShowCorrectionWindow(2)
		
spuriouserror: 
		Vdict1.Unlock()
	End Sub
	
	
	Private Sub Vdict1_TextSelChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Vdict1.TextSelChanged
		Dim selstart As Integer
		Dim sellen As Integer
		
		Vdict1.TextSelGet(selstart, sellen)
		RichTextBox1.SelectionStart = selstart
		RichTextBox1.SelectionLength = sellen
	End Sub
	
	
	Public Sub Copy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Copy.Click
		My.Computer.Clipboard.SetText(RichTextBox1.SelectedText)
	End Sub
	
	Public Sub Cut_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Cut.Click
		My.Computer.Clipboard.SetText(RichTextBox1.SelectedText)
		SetText("", True)
	End Sub
	
	Public Sub Delete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Delete.Click
		SetText("", True)
	End Sub
	
    Public Sub Exit_Renamed_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExitApp.Click
        Main_F_FormClosed(Me, eventSender)
        End
    End Sub
	
	
    Public Sub ShowCorrectionWindow(ByRef ShowVal As Integer)
        Dim sflags As Integer
        If (ShowVal) Then
            If (ShowVal = 2) Then
                If (showcorrection.Checked) Then
                    sflags = 1
                Else
                    sflags = 0
                End If
            Else
                showcorrection.Checked = True
                sflags = 1
            End If

        Else
            showcorrection.Checked = False
            sflags = 0
        End If

        Vdict1.flags = (Vdict1.flags And (Not 1)) Or sflags

        GetCaretPos(Caret)
        Caret.x = Caret.x + VB6.PixelsToTwipsX(Me.Left) / VB6.TwipsPerPixelX
        Caret.y = Caret.y + VB6.PixelsToTwipsY(Me.Top) / VB6.TwipsPerPixelY + VB6.PixelsToTwipsY(RichTextBox1.Top) / VB6.TwipsPerPixelY + 60

        Vdict1.SetSelRect(Caret.x, Caret.y, Caret.x, Caret.y)

    End Sub
	
	
	Private Sub FloatPad(ByRef ontop As Object)
		Dim Tp As Integer
		If (ontop) Then
			Tp = SetWindowPos(Me.Handle.ToInt32, HWND_TOPMOST, 0, 0, 0, 0, FLAGS)
		Else
			Tp = SetWindowPos(Me.Handle.ToInt32, HWND_NOTOPMOST, 0, 0, 0, 0, FLAGS)
		End If
	End Sub
	
	Private Function Max(ByRef a As Object, ByRef b As Object) As Object
        If (a > b) Then
            Max = a
        Else
            Max = b
        End If
	End Function
	
    Private Sub Main_F_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
        RichTextBox1.Width = VB6.TwipsToPixelsX(Max(VB6.PixelsToTwipsX(Me.Width) - 109, 0))
        RichTextBox1.Height = VB6.TwipsToPixelsY(Max(VB6.PixelsToTwipsY(Me.Height) - 1158, 0))
    End Sub
	
	Public Sub Main_F_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		listen((0))
		ShowCorrectionWindow((0))
		End
	End Sub
	
	
	Public Sub New_Renamed_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles New_Renamed.Click
		If MsgBox("This will erase the current text. Are you sure?", MsgBoxStyle.OKCancel, "New") = 1 Then
            TextReset()
			File_Name = ""
		End If
	End Sub
	
	
	Public Sub open_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles open.Click
		CommonDialog1Open.ShowDialog()
		CommonDialog1Save.FileName = CommonDialog1Open.FileName
		If (CommonDialog1Open.FileName <> "") Then
			New_Renamed_Click(New_Renamed, New System.EventArgs())
            RichTextBox1.LoadFile(CommonDialog1Open.FileName, RichTextBoxStreamType.PlainText)
            SetText((RichTextBox1.Text), False)
		End If
		
	End Sub
	
	Public Sub Options_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Options.Click
		Vdict1.GeneralDlg(Me.Handle.ToInt32, "Dictation Options")
	End Sub
	
	Public Sub Paste_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Paste.Click
		SetText(My.Computer.Clipboard.GetText, True)
	End Sub
	
	Private Sub DoSave()
		
		If (VB.Right(File_Name, 3) = "rtf") Then
			FileOpen(1, File_Name, OpenMode.Output)
			PrintLine(1, RichTextBox1.RTF)
			FileClose(1)
		Else
			FileOpen(1, File_Name, OpenMode.Output)
			PrintLine(1, RichTextBox1.Text)
			FileClose(1)
		End If
		
	End Sub
	Public Sub save_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles save.Click
		If (File_Name <> "") Then
			DoSave()
		Else
			saveas_Click(saveas, New System.EventArgs())
		End If
	End Sub
	
	Public Sub saveas_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles saveas.Click
		
		CommonDialog1Save.ShowDialog()
		CommonDialog1Open.FileName = CommonDialog1Save.FileName
		If (CommonDialog1Open.FileName <> "") Then
			File_Name = CommonDialog1Open.FileName
			DoSave()
		End If
	End Sub
	
	Public Sub ListenUI(ByRef op As Short)
		If (op = 1) Then
			
			Toolbar1.Buttons(1).Value = ComctlLib.ValueConstants.tbrPressed
			Toolbar1.Buttons(1).Image = 1
			listening.Checked = True
		Else
			Toolbar1.Buttons(1).Value = ComctlLib.ValueConstants.tbrUnpressed
			Toolbar1.Buttons(1).Image = 12
			listening.Checked = False
		End If
	End Sub
	
	Public Sub listen(ByRef op As Short)
		
		If (op = 1) Then
			Stopreading_Click(Stopreading, New System.EventArgs())
			Vdict1.Mode = VSRMODE_DCTONLY
			On Error GoTo NoActivate
			Vdict1.Activate()
NoActivate: 
		Else
			Vdict1.Mode = VSRMODE_OFF
			On Error GoTo NoDeactivate
			Vdict1.Deactivate()
NoDeactivate: 
			
		End If
		ListenUI((op))
		Exit Sub
ErrorMessage: 
		MsgBox("Unable to initialize dictation engine.")
		Me.Close()
	End Sub
	
	
	
	Private Sub toolbar1_ButtonClick(ByVal eventSender As System.Object, ByVal eventArgs As AxComctlLib.IToolbarEvents_ButtonClickEvent) Handles toolbar1.ButtonClick
		' Use the Key property with the SelectCase statement to specify
		' an action.
		Select Case eventArgs.Button.Key
			Case Is = "cut"
				Cut_Click(Cut, New System.EventArgs())
				
			Case Is = "cutall"
				SelectAll_Click(SelectAll, New System.EventArgs())
				Cut_Click(Cut, New System.EventArgs())
				
			Case Is = "copy"
				Copy_Click(Copy, New System.EventArgs())
				
			Case Is = "paste"
				Paste_Click(Paste, New System.EventArgs())
				
			Case Is = "new"
				New_Renamed_Click(New_Renamed, New System.EventArgs())
				
			Case Is = "save"
				save_Click(save, New System.EventArgs())
				
			Case Is = "open"
				open_Click(open, New System.EventArgs())
				
			Case Is = "listen"
				If (listening.Checked) Then
					listen((0))
				Else
					listen((1))
				End If
				
			Case Is = "showhide"
				showcorrection_Click(showcorrection, New System.EventArgs())
				
				
			Case Is = "addword"
				Addword_Click(Addword, New System.EventArgs())
				
			Case Is = "read"
				ReadDocument_Click(ReadDocument, New System.EventArgs())
		End Select
	End Sub
	
	Public Sub SelectAll_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SelectAll.Click
		RichTextBox1.SelectionStart = 0
		RichTextBox1.SelectionLength = Len(RichTextBox1.Text)
	End Sub
End Class