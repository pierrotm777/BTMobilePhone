Imports System
Imports System.Text
Imports System.Windows.Forms
Imports System.Speech
Imports System.Speech.Synthesis
Imports System.IO

Namespace SoundsVB
    Public Class speakerForm
        Dim speaker As New SpeechSynthesizer()

        Private Sub exitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles exitButton.Click
            Application.Exit()

        End Sub

        Private Sub speakButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles speakButton.Click
            speaker.Rate = Convert.ToInt32(speedUpDown.Value)
            speaker.Volume = Convert.ToInt32(volumeUpDown.Value)
            speaker.SpeakAsync(speakTextBox.Text)

        End Sub

        Private Sub fileButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fileButton.Click
            SaveFileDialog1.FileName = fileTextBox.Text
            If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                fileTextBox.Text = SaveFileDialog1.FileName
            End If
        End Sub

        Private Sub exportButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles exportButton.Click
            If fileTextBox.Text = String.Empty Then
                MessageBox.Show("Please select a location to save the file.", "File not defined", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                Try
                    speaker.Rate = Convert.ToInt32(speedUpDown.Value)
                    speaker.Volume = Convert.ToInt32(volumeUpDown.Value)
                    speaker.SetOutputToWaveFile(fileTextBox.Text)
                    speaker.Speak(speakTextBox.Text)
                    speaker.SetOutputToDefaultAudioDevice()
                    MessageBox.Show("File written successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("There was an error writing the file." & vbNewLine & ex.Message, "File error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub
    End Class

End Namespace

