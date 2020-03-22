Public Class Form1
    Dim a As New Speech.Synthesis.SpeechSynthesizer
    Private WithEvents OutputListBox As New ListBox With {.Dock = DockStyle.Fill, .IntegralHeight = False, .ForeColor = Color.AntiqueWhite, .BackColor = Color.Black}
    Private WithEvents SpeechEngine As New System.Speech.Recognition.SpeechRecognitionEngine(System.Globalization.CultureInfo.GetCultureInfo("en-US"))
    Dim tms As Integer = 0
    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SpeechEngine.RecognizeAsyncCancel()
        SpeechEngine.Dispose()
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Speech recognition, by Doc Oc, version:" & My.Application.Info.Version.ToString & " , say hello to piss the pc off"

        Controls.Add(OutputListBox)
        SpeechEngine.LoadGrammar(New System.Speech.Recognition.DictationGrammar)
        SpeechEngine.SetInputToDefaultAudioDevice()
        SpeechEngine.RecognizeAsync(Speech.Recognition.RecognizeMode.Multiple)
    End Sub

    Private Sub SpeechEngine_SpeechRecognized(sender As Object, e As System.Speech.Recognition.SpeechRecognizedEventArgs) Handles SpeechEngine.SpeechRecognized
        OutputListBox.Items.Add("You said: " & e.Result.Text)
        If e.Result.Text.ToLower.Contains("home") = True Or e.Result.Text.ToLower.Contains("her") = True Or e.Result.Text.ToLower.Contains("hello") = True Or e.Result.Text.ToLower.Contains("o") = True Or e.Result.Text.ToLower.Contains("who") = True Or e.Result.Text.ToLower.Contains("will") = True Or e.Result.Text.ToLower.Contains("hole") = True Or e.Result.Text.ToLower.Contains("whole") = True Or e.Result.Text.ToLower.Contains("hold") Then
            Select Case tms
                Case 0
                    a.SpeakAsync("you said hello, i'm angry with you")
                Case 1
                    a.SpeakAsync("say that one more time!")
                Case 2
                    a.SpeakAsync("stop it now!")
                Case 3
                    a.SpeakAsync("that's it, i'm outta here!")
                Case 4
                    a.Rate = -9
                    a.Speak("you blithering beeping moaning never being a quite man")
                    End




            End Select
            tms += 1
        End If
    End Sub
End Class