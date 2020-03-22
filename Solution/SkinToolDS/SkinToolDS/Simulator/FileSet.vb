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
            Simulator.DataFile = fdlg.FileName

            Dim BaseFileName As String = Mid(Simulator.DataFile, InStrRev(Simulator.DataFile, "\") + 1)
            lblFile.Text = "File: " & BaseFileName

            If Simulator.btnConnect.Enabled = False Then 'Serial port must be connected, start sending data
                Simulator.StartReadingFile()
            End If
        End If
    End Sub

    Private Sub btnStopReadingFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopReadingFile.Click
        Simulator.StopReadingFile()
    End Sub

    Private Sub boxDelimiter_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles boxDelimiter.SelectionChangeCommitted
        Simulator.FileWaitDelimiter = boxDelimiter.SelectedItem
    End Sub
End Class
