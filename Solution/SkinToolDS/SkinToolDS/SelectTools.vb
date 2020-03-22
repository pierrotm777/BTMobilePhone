Imports System.IO

Public Class SelectTools

    Private Sub SelectFiles_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Fixed3D
        'Me.Size = New Point(80, 170)
        'Me.Width = 90
        Me.ControlBox = False
        Me.Text = String.Empty

        'Me.Size = New Size
        'Me.Width = 100
    End Sub
    Private Sub SelectFiles_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Me.WindowState = FormWindowState.Minimized
        Me.Visible = False
        e.Cancel = True
    End Sub


    Dim simu As New Simulator
    Private Sub btnGpsSimu_Click(sender As System.Object, e As System.EventArgs) Handles btnGpsSimu.Click
        If simu.Visible = False Then
            simu.Show()
            simu.Location = New Point(My.Settings.LastPosition.X - 535, My.Settings.LastPosition.Y)
            '; 577
        Else
            simu.Hide()
        End If
        Me.Visible = False
    End Sub

    Dim capt As New Form1
    Private Sub btnCaptureGps_Click(sender As System.Object, e As System.EventArgs) Handles btnCaptureGps.Click
        If capt.Visible = False Then
            capt.Show()
            capt.Location = New Point(My.Settings.LastPosition.X - 789, My.Settings.LastPosition.Y)
            '789; 544
        Else
            capt.Hide()
        End If
        Me.Visible = False
    End Sub

    Dim proc As New Process
    Private Sub btnProcessGps_Click(sender As System.Object, e As System.EventArgs) Handles btnProcessGps.Click
        If proc.Visible = False Then
            proc.Show()
            proc.Location = New Point(My.Settings.LastPosition.X - 664, My.Settings.LastPosition.Y)
            '789; 544
        Else
            proc.Hide()
        End If
        Me.Visible = False
    End Sub

    Dim pickerFrm As New DemoForm
    Private Sub btnColorPicker_Click(sender As System.Object, e As System.EventArgs) Handles btnColorPicker.Click
        If pickerFrm.Visible = False Then
            pickerFrm.Show()
            pickerFrm.Location = New Point(My.Settings.LastPosition.X - 488, My.Settings.LastPosition.Y)
            '488; 401
        Else
            pickerFrm.Hide()
        End If
        Me.Visible = False
    End Sub

    Dim keyFrm As New frmKey
    Private Sub btnKeyCode_Click(sender As System.Object, e As System.EventArgs) Handles btnKeyCode.Click
        If keyFrm.Visible = False Then
            keyFrm.Show()
            keyFrm.Location = New Point(My.Settings.LastPosition.X - 488, My.Settings.LastPosition.Y)
            '488; 401
        Else
            keyFrm.Hide()
        End If
        Me.Visible = False
    End Sub

    Dim cur As New CursorPosition
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If cur.Visible = False Then
            cur.Show()
            cur.Location = New Point(My.Settings.LastPosition.X - 267, My.Settings.LastPosition.Y)
            '267; 114
        Else
            cur.Hide()
        End If
        Me.Visible = False
    End Sub
End Class