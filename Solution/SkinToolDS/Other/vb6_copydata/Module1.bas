Attribute VB_Name = "Module1"
Option Explicit

Public lpPrevWndProc As Long
Public lHwnd         As Long

Public Const GWL_WNDPROC = (-4)
Public Const WM_COPYDATA = &H4A

Public Type COPYDATASTRUCT
    dwData As Long
    cbData As Long
    lpData As Long
End Type
'
'Copies a block of memory from one location to another.
'
Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" _
   (hpvDest As Any, hpvSource As Any, ByVal cbCopy As Long)

Declare Function CallWindowProc Lib "user32" Alias _
   "CallWindowProcA" (ByVal lpPrevWndFunc As Long, ByVal hwnd As _
   Long, ByVal Msg As Long, ByVal wParam As Long, ByVal lParam As _
   Long) As Long

Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" _
   (ByVal hwnd As Long, ByVal nIndex As Long, ByVal dwNewLong As _
   Long) As Long
Public Sub pHook()
'
' Sub class the form to trap for Windows messages.
'
lpPrevWndProc = SetWindowLong(lHwnd, GWL_WNDPROC, AddressOf fWindowProc)
Debug.Print lpPrevWndProc
End Sub
Sub pReceiveMsg(lParam As Long)
Dim sString       As String
Dim l             As Long
Dim lnumEls       As Long
Dim bfailed       As Boolean
Dim pb            As New PropertyBag
Dim cds           As COPYDATASTRUCT
Dim buf(1 To 255) As Byte

'
' Copy the data sent to this application
' into a local structure.
'
Call CopyMemory(cds, ByVal lParam, Len(cds))

Select Case cds.dwData
    Case 1 ' An array of Doubles.
        '
        ' Get the number of items.
        ' Each Double is 8 bytes.
        '
        lnumEls = cds.cbData \ 8
        
        ReDim aDoubles(0 To lnumEls - 1) As Double
        
        Call CopyMemory(aDoubles(0), ByVal cds.lpData, cds.cbData)
        '
        ' Fill the listbox.
        '
        frmReceive.lstArray.Clear
        For l = LBound(aDoubles) To UBound(aDoubles)
            frmReceive.lstArray.AddItem aDoubles(l)
            If aDoubles(l) <> l Then bfailed = True
        Next
    
    Case 2 ' An array of strings in a property bag.
        ReDim bytes(0 To cds.cbData - 1) As Byte
        '
        ' Copy the transferred data to a byte array
        '
        Call CopyMemory(bytes(0), ByVal cds.lpData, cds.cbData)
        pb.Contents = bytes
        '
        ' Fill the array.
        '
        lnumEls = pb.ReadProperty("NumEls")
        ReDim aStrings(0 To lnumEls) As String
        
        On Error Resume Next
        For l = 0 To UBound(aStrings)
            aStrings(l) = pb.ReadProperty(LTrim$(Str$(l)))
        Next
        On Error GoTo 0
        
        ' fill the listbox, and check that data was transferred correctly
        frmReceive.lstArray.Clear
        For l = LBound(aStrings) To UBound(aStrings)
            frmReceive.lstArray.AddItem aStrings(l)
            If aStrings(l) <> "Item #" & l Then bfailed = True
        Next
        
    Case 3 ' A string was passed.
        '
        ' Copy the string that was passed into a byte array.
        '
        Call CopyMemory(buf(1), ByVal cds.lpData, cds.cbData)
        '
        ' Convert the ASCII byte array back to a Unicode string.
        '
        sString = StrConv(buf, vbUnicode)
        sString = Left$(sString, InStr(1, sString, Chr$(0)) - 1)
        '
        ' Display the received string.
        '
        frmReceive.txtString = sString
End Select

If bfailed Then
    Call MsgBox("Data transferred incorrectly", vbExclamation, "Error Receiving Data")
End If
End Sub
Public Sub pUnhook()
'
' Remove the subclassing.
'
Call SetWindowLong(lHwnd, GWL_WNDPROC, lpPrevWndProc)
End Sub
Function fWindowProc(ByVal hw As Long, ByVal uMsg As Long, _
         ByVal wParam As Long, ByVal lParam As Long) As Long
'
' This callback routine is called by Windows whenever
' a message is sent to this form.  If it is the copy
' message call our procedure to receive the message.
'
If uMsg = WM_COPYDATA Then Call pReceiveMsg(lParam)
'
' Call the original window procedure associated with this form.
'
fWindowProc = CallWindowProc(lpPrevWndProc, hw, uMsg, wParam, lParam)
End Function


