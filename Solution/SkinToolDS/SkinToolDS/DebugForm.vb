Imports System.Text
Imports System.Runtime.InteropServices
Imports SkinToolDS.NativeMethods

Public Class DebugForm

    Private Sub DebugForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = My.Settings.LastPositionDebug
        Me.Size = My.Settings.LastSizeDebug
        Me.Width = 501

    End Sub


    Private Sub SelectFiles_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Me.WindowState = FormWindowState.Normal Then
            My.Settings.LastPositionDebug = Me.Location
            My.Settings.LastSizeDebug = Me.Size
        End If

        My.Settings.Save()
        Me.Visible = False
        e.Cancel = True

    End Sub

    Private Sub chkDebugOnTop_Click(sender As System.Object, e As System.EventArgs) Handles chkDebugOnTop.Click
        If chkDebugOnTop.Checked = True Then
            Me.TopMost = True
        Else
            Me.TopMost = False
        End If
    End Sub


    Private Sub btnDebugHide_Click(sender As System.Object, e As System.EventArgs) Handles btnDebugHide.Click
        If Me.WindowState = FormWindowState.Normal Then
            My.Settings.LastPositionDebug = Me.Location
            My.Settings.LastSizeDebug = Me.Size
        End If

        My.Settings.Save()
        Me.Hide()
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
                txtBoxDebuLog.AppendText(fileName & Environment.NewLine & "-" & Environment.NewLine)
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
    Private Sub txtBoxDebuLog_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles txtBoxDebuLog.MouseDown
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

            txtBoxDebuLog.ContextMenu = Contextmenu1

        End If
    End Sub

    'Private Sub CutAction(sender As Object, e As EventArgs)
    '    txtBoxDebugLog.Cut()
    'End Sub

    Private Sub CopyAction(sender As Object, e As EventArgs)
        'Dim objGraphics As Graphics
        'Clipboard.SetData(DataFormats.Rtf, txtBoxDebugLog.SelectedText)
        'Clipboard.Clear()
        txtBoxDebuLog.Copy()
    End Sub

    Private Sub PasteAction(sender As Object, e As EventArgs)
        'If Clipboard.ContainsText(TextDataFormat.Rtf) Then
        '    If IsNothing(SDK) = False Then
        '        SDK.execute(Clipboard.GetData(DataFormats.Rtf).ToString())
        '    End If
        'End If
        'txtBoxDebugLog.Paste()
    End Sub
#End Region

    Private Sub btnDebugClear_Click(sender As System.Object, e As System.EventArgs) Handles btnDebugClear.Click
        txtBoxDebuLog.Clear()
    End Sub

    Private Sub btnDebugPause_Click(sender As System.Object, e As System.EventArgs) Handles btnDebugPause.Click
        MessageBox.Show("Feature not ready")
    End Sub

    Private Sub btnDebugSave_Click(sender As System.Object, e As System.EventArgs) Handles btnDebugSave.Click
        Dim FILE_NAME As String = Application.StartupPath & "SkinToolDS.log"
        'If System.IO.File.Exists(FILE_NAME) = True Then
        Dim objWriter As New System.IO.StreamWriter(FILE_NAME, True, Encoding.Unicode)
        objWriter.Write(txtBoxDebuLog.Text)
        objWriter.Close()
        MessageBox.Show("Profile: Debug file saved !")
        'Else
        'lblProfile.Text = "Profile: -"
        'End If
    End Sub
End Class