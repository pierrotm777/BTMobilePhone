Imports System.IO

Public Class SelectFiles
    Dim arrayDocFiles() As String
    Dim frmDocs As Form
    Private Sub SelectFiles_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Fixed3D
        'Me.Size = New Point(124, 170)
        'Me.Width = 90
        Me.ControlBox = False
        Me.Text = String.Empty

        'If FrontEndName = "RR" Then
        btnRRIni.Text = FrontEndName & " Ini"
        btnRRDebug.Text = FrontEndName & " Debug"
        btnRRTBL.Text = FrontEndName & " ExecTBL"
        'End If

        'Me.StartPosition = FormStartPosition.Manual
        'Me.TopMost = True
    End Sub
    Private Sub SelectFiles_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Me.WindowState = FormWindowState.Minimized
        Me.Visible = False
        e.Cancel = True
    End Sub


    Private Sub btnRRIni_Click(sender As System.Object, e As System.EventArgs) Handles btnRRIni.Click
        If My.Settings.SkinPath <> "" Then
            If FrontEndName = "RR" Then
                'If File.Exists(My.Settings.SkinPath & "..\..\rr.ini") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.SkinPath & "..\..\rr.ini")
                If CheckIfNppExist() = True Then
                    If File.Exists(My.Settings.RRPath & "rr.ini") Then System.Diagnostics.Process.Start("notepad++.exe", My.Settings.RRPath & "rr.ini")
                Else
                    If File.Exists(My.Settings.RRPath & "rr.ini") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.RRPath & "rr.ini")
                End If
            ElseIf FrontEndName = "iCarDS" Then
                'MessageBox.Show(My.Settings.SkinPath & "setting.ini")
                If CheckIfNppExist() = True Then
                    If File.Exists(My.Settings.SkinPath & "setting.ini") Then System.Diagnostics.Process.Start("notepad++.exe", My.Settings.SkinPath & "setting.ini")
                Else
                    If File.Exists(My.Settings.SkinPath & "setting.ini") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.SkinPath & "setting.ini")
                End If
            End If

        End If
        Me.Visible = False
    End Sub
    Private Sub btnRRDebug_Click(sender As System.Object, e As System.EventArgs) Handles btnRRDebug.Click
        If My.Settings.RRPath <> "" Then 'If My.Settings.SkinPath <> "" Then
            If FrontEndName = "RR" Then
                'If File.Exists(My.Settings.SkinPath & "..\..\debug.txt") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.SkinPath & "..\..\debug.txt")
                If CheckIfNppExist() = True Then
                    If File.Exists(My.Settings.RRPath & "debug.txt") Then System.Diagnostics.Process.Start("notepad++.exe", My.Settings.RRPath & "debug.txt")
                Else
                    If File.Exists(My.Settings.RRPath & "debug.txt") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.RRPath & "debug.txt")
                End If
            ElseIf FrontEndName = "iCarDS" Then
                'If File.Exists(My.Settings.SkinPath & "..\..\debug.txt") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.SkinPath & "..\..\debug.txt")
                If CheckIfNppExist() = True Then
                    If File.Exists(My.Settings.SkinPath & "..\..\iCarDS.log") Then System.Diagnostics.Process.Start("notepad++.exe", My.Settings.SkinPath & "..\..\iCarDS.log")
                Else
                    If File.Exists(My.Settings.SkinPath & "..\..\iCarDS.log") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.SkinPath & "..\..\iCarDS.log")
                End If
            End If
        End If
        Me.Visible = False
    End Sub
    Private Sub btnRRTBL_Click(sender As System.Object, e As System.EventArgs) Handles btnRRTBL.Click
        If My.Settings.RRPath <> "" Then 'If My.Settings.SkinPath <> "" Then
            'If File.Exists(My.Settings.SkinPath & "..\..\execTbl.ini") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.SkinPath & "..\..\execTbl.ini")
            If CheckIfNppExist() = True Then
                If File.Exists(My.Settings.RRPath & "ExecTbl.ini") Then System.Diagnostics.Process.Start("notepad++.exe", My.Settings.RRPath & "ExecTbl.ini")
            Else
                If File.Exists(My.Settings.RRPath & "ExecTbl.ini") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.RRPath & "ExecTbl.ini")
            End If
        End If
        Me.Visible = False
    End Sub
    Private Sub btnSkinTBL_Click(sender As System.Object, e As System.EventArgs) Handles btnSkinTBL.Click
        If My.Settings.SkinPath <> "" Then 'If My.Settings.SkinPath <> "" Then
            If CheckIfNppExist() = True Then
                If File.Exists(My.Settings.SkinPath & "ExecTbl.ini") Then System.Diagnostics.Process.Start("notepad++.exe", My.Settings.SkinPath & "ExecTbl.ini")
            Else
                If File.Exists(My.Settings.SkinPath & "ExecTbl.ini") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.SkinPath & "ExecTbl.ini")
            End If
        End If
        Me.Visible = False
    End Sub
    Private Sub btnSkinIni_Click(sender As System.Object, e As System.EventArgs) Handles btnSkinIni.Click
        If My.Settings.SkinPath <> "" Then

            If CheckIfNppExist() = True Then
                If File.Exists(My.Settings.SkinPath & "skin.ini") Then System.Diagnostics.Process.Start("notepad++.exe", My.Settings.SkinPath & "skin.ini")
            Else
                If File.Exists(My.Settings.SkinPath & "skin.ini") Then System.Diagnostics.Process.Start("Notepad.Exe", My.Settings.SkinPath & "skin.ini")
            End If
        End If
        Me.Visible = False
    End Sub
    Private Sub btnRRDocs_Click(sender As System.Object, e As System.EventArgs) Handles btnRRDocs.Click
        frmDocs = New Form
        If Directory.Exists(My.Settings.RRPath & "Documentation") = True Then
            arrayDocFiles = Directory.GetFiles(My.Settings.RRPath & "Documentation\", "*.txt", SearchOption.TopDirectoryOnly)
        Else
            MessageBox.Show("No documentation directory found !", "Doc Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        frmDocs.Size = New System.Drawing.Size(185, (arrayDocFiles.Length * 22) + 7)
        Dim btnName As String
        Dim x As Integer = 0, i As Integer = 0
        frmDocs.FormBorderStyle = Windows.Forms.FormBorderStyle.Fixed3D

        frmDocs.ControlBox = False
        frmDocs.Text = String.Empty

        If arrayDocFiles.Length > 0 Then
            For i = 0 To arrayDocFiles.Length - 1
                'MessageBox.Show(Path.GetFileName(arrayDocFiles(i)))
                btnName = "btnDoc" & CStr(i) '
                Dim button1 As New Button
                button1.Name = btnName
                button1.Width = 175
                frmDocs.Controls.Add(button1)
                button1.Location = New Point(2, x)
                button1.Tag = "btnDoc" & CStr(i) '
                button1.Text = Path.GetFileName(arrayDocFiles(i)) '"Hello"
                AddHandler button1.Click, AddressOf Me.readFile
                x += 22
            Next
            frmDocs.Show()
            frmDocs.Location = New Point(My.Settings.LastPosition.X + 25, My.Settings.LastPosition.Y + 172)
            'Else
            '    MessageBox.Show("No documentation directory found !", "Doc Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.Visible = False
        End If


    End Sub

    Private Sub readFile(sender As Object, e As System.EventArgs)
        Dim currButton As Button = CType(sender, Button)
        'MessageBox.Show(currButton.Tag.ToString.Replace("btnDoc", ""))

        If CheckIfNppExist() = True Then
            System.Diagnostics.Process.Start("notepad++.exe", arrayDocFiles(currButton.Tag.ToString.Replace("btnDoc", "")))
        Else
            System.Diagnostics.Process.Start("Notepad.Exe", arrayDocFiles(currButton.Tag.ToString.Replace("btnDoc", "")))
        End If
        frmDocs.Hide()
        Me.Hide()
    End Sub

End Class