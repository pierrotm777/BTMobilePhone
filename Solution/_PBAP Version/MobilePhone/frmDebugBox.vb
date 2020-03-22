Option Strict Off
Option Explicit On

Imports System.Text


Public Class frmDebugBox
    Public SDK As Object
    Dim objStreamWriter As StreamWriter
    Public Sub frmDebugBox_Load(sender As System.Object, e As System.EventArgs) Handles Me.Load
        SDK = CreateObject("RideRunner.SDK")
        'ScreenResolution
        Me.Size = New Size(800, 200)
        Me.Top = 470
        Me.Left = 0

        If File.Exists(MainPath & "MobilePhone_ATCMD.lst") Then
            Dim CmdArray() As String = File.ReadAllLines(MainPath & "MobilePhone_ATCMD.lst")
            For Each line In CmdArray
                ComboBox1.Items.Add(line)
            Next
        Else
            objStreamWriter = New StreamWriter(MainPath & "MobilePhone_ATCMD.lst", True, Encoding.Unicode)
            objStreamWriter.WriteLine("Add your AT commands ...")
            objStreamWriter.Close()
        End If

    End Sub

    Private Sub frmDebugBox_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            'cancel the close
            e.Cancel = True
            Me.Hide()
        End If
    End Sub

    Private Sub ComboBox1_Click(sender As System.Object, e As System.EventArgs) Handles ComboBox1.Click
        Dim CmdArray() As String = File.ReadAllLines(MainPath & "MobilePhone_ATCMD.lst")
        ComboBox1.Items.Clear()
        For Each line In CmdArray
            ComboBox1.Items.Add(line)
        Next
    End Sub


    Private Sub btnSend_Click(sender As System.Object, e As System.EventArgs) Handles btnSend.Click
        If ComboBox1.Text <> "" Then
            Try
                SDK.Execute("MOBILEPHONE_ATCMD;" & ComboBox1.Text)

            Catch ex As Exception
                MessageBox.Show("Error on debug form is --> " & ex.Message)
            End Try

        End If


        'If CType(sender, ComboBox).FindString(CType(sender, ComboBox).Text, 1) = True Then
        '    'if it doesnt exist then remove the last character/s that dont match and set the cursor position to the end
        '    CType(sender, ComboBox).Text = CType(sender, ComboBox).Text.Substring(0, CType(sender, ComboBox).Text.Length - 1)
        '    CType(sender, ComboBox).SelectionStart = CType(sender, ComboBox).Text.Length
        'End If
    End Sub

 
    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        objStreamWriter = New StreamWriter(MainPath & "MobilePhone_ATCMD.lst", True) ', Encoding.Unicode)
        objStreamWriter.WriteLine(TextBox1.Text)
        objStreamWriter.Close()

        MessageBox.Show("The command '" & TextBox1.Text & "' is added to the file 'MobilePhone_ATCMD.lst'")
        TextBox1.Text = "Add your AT commands ..."
    End Sub

    Private Sub TextBox1_Click(sender As System.Object, e As System.EventArgs) Handles TextBox1.Click
        TextBox1.Text = ""
    End Sub
End Class