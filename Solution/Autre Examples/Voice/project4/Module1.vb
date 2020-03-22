Option Strict Off
Option Explicit On
Module Module1
	Public Const SWP_NOMOVE As Short = 2
	Public Const SWP_NOSIZE As Short = 1
	Public Const FLAGS As Boolean = SWP_NOMOVE Or SWP_NOSIZE
	Public Const HWND_TOPMOST As Short = -1
	Public Const HWND_NOTOPMOST As Short = -2
	Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
	
	Structure POINTAPI
		Dim x As Integer
		Dim y As Integer
	End Structure
    Declare Function GetCaretPos Lib "user32" (ByRef lpPoint As POINTAPI) As Integer
End Module