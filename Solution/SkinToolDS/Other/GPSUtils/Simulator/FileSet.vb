Public Class FileSet

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim fdlg As OpenFileDialog = New OpenFileDialog()
        fdlg.Title = "GPS File to Process"

        If My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\Data") Then
            fdlg.InitialDirectory = Application.StartupPath & "\Data"
        Else
            fdlg.InitialDirectory = Application.StartupPath
        End If
        fdlg.Filter = "GPS Data Files (*.gps)|*.gps"
        fdlg.FilterIndex = 2
        fdlg.RestoreDirectory = True
        If fdlg.ShowDialog() = DialogResult.OK Then
            Form1.DataFile = fdlg.FileName

            Dim BaseFileName As String = Mid(Form1.DataFile, InStrRev(Form1.DataFile, "\") + 1)
            lblFile.Text = "File: " & BaseFileName

            If Form1.btnConnect.Enabled = False Then 'Serial port must be connected, start sending data
                Form1.StartReadingFile()
            End If
        End If
    End Sub

    Private Sub btnStopReadingFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopReadingFile.Click
        Form1.StopReadingFile()
    End Sub

    Private Sub boxDelimiter_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles boxDelimiter.SelectionChangeCommitted
        Form1.FileWaitDelimiter = boxDelimiter.SelectedItem
    End Sub
End Class
