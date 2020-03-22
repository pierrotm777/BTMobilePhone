Public Class FormPutGuiVb
    Private m_workings As Workings

    '---------------------------------------------------------------------------
    Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles browseButton.Click, MenuItem1.Click
        m_workings.button1_Click(sender, e)
    End Sub

    Private Sub cancelButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cancelButton1.Click, MenuItem2.Click
        m_workings.cancelButton_Click(sender, e)
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_workings = New Workings(Me)
    End Sub
End Class
