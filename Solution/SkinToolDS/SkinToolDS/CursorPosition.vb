Public Class CursorPosition

    Private Sub CursorPosition_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Mouse position"
        Me.ControlBox = False

    End Sub

#Region "Mouse Position"
    Structure PointAPI
        Public x As Int32 ' Integer if you prefer
        Public y As Int32 ' Integer if you prefer
    End Structure
    Public Declare Function GetCursorPos Lib "user32" (ByRef lpPoint As PointAPI) As Boolean
    'Declare Function ScreenToClient Lib "user32" (ByVal hwnd As Int32, ByRef lpPoint As PointAPI) As Int32
    Private Sub TimerMouse_Tick(sender As System.Object, e As System.EventArgs) Handles TimerMouse.Tick
        Dim lpPoint As PointAPI
        GetCursorPos(lpPoint)
        lblCursorPosition.Text = "[Cursor position X:" & CStr(lpPoint.x) & " Y:" & CStr(lpPoint.y) & "]"
    End Sub
#End Region

End Class