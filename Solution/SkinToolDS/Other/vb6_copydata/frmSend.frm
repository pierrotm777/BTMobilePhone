VERSION 5.00
Begin VB.Form frmSend 
   BackColor       =   &H00000000&
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Send Data"
   ClientHeight    =   5175
   ClientLeft      =   3090
   ClientTop       =   3225
   ClientWidth     =   5190
   Icon            =   "frmSend.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5175
   ScaleWidth      =   5190
   Begin VB.Frame Frame3 
      BackColor       =   &H00000000&
      Caption         =   "Communicate with the Receiving Application"
      ForeColor       =   &H00FFFFFF&
      Height          =   4935
      Left            =   120
      TabIndex        =   10
      Top             =   120
      Width           =   4935
      Begin VB.Frame Frame4 
         BackColor       =   &H00000000&
         Caption         =   "Communicate via its Window"
         ForeColor       =   &H00FFFFFF&
         Height          =   1935
         Left            =   120
         TabIndex        =   14
         Top             =   960
         Width           =   4575
         Begin VB.CommandButton cmdSendStringData 
            Caption         =   "Send String Data"
            Height          =   375
            Left            =   2880
            TabIndex        =   4
            Top             =   1440
            Width           =   1575
         End
         Begin VB.TextBox txtnumItems 
            BackColor       =   &H8000000F&
            Height          =   285
            Left            =   240
            TabIndex        =   2
            Text            =   "10000"
            Top             =   1320
            Width           =   1215
         End
         Begin VB.CommandButton cmdSendNumData 
            Caption         =   "Send Numeric Data"
            Height          =   375
            Left            =   2880
            TabIndex        =   3
            Top             =   960
            Width           =   1575
         End
         Begin VB.CommandButton cmdSendData 
            Caption         =   "Send the Above String"
            Height          =   375
            Left            =   120
            TabIndex        =   1
            Top             =   360
            Width           =   1815
         End
         Begin VB.Line Line1 
            BorderColor     =   &H8000000F&
            BorderWidth     =   2
            X1              =   120
            X2              =   4440
            Y1              =   840
            Y2              =   840
         End
         Begin VB.Label Label2 
            BackStyle       =   0  'Transparent
            Caption         =   "Number of Items:"
            ForeColor       =   &H00FFFFFF&
            Height          =   255
            Left            =   240
            TabIndex        =   15
            Top             =   1080
            Width           =   1215
         End
      End
      Begin VB.TextBox txtString 
         BackColor       =   &H8000000F&
         Height          =   285
         Left            =   120
         TabIndex        =   0
         Top             =   600
         Width           =   4575
      End
      Begin VB.Frame Frame1 
         BackColor       =   &H00000000&
         Caption         =   "Communicate via a Textbox"
         ForeColor       =   &H00FFFFFF&
         Height          =   1815
         Left            =   120
         TabIndex        =   11
         Top             =   3000
         Width           =   4695
         Begin VB.CommandButton cmdCopyToHidden 
            Caption         =   "Copy a String to its Textbox"
            Height          =   375
            Left            =   120
            TabIndex        =   6
            Top             =   840
            Width           =   2175
         End
         Begin VB.CommandButton cmdSendToHidden 
            Caption         =   "Send a String to itsTextbox"
            Height          =   375
            Left            =   120
            TabIndex        =   5
            Top             =   360
            Width           =   2175
         End
         Begin VB.Frame Frame2 
            BackColor       =   &H00000000&
            Caption         =   "Using:"
            ForeColor       =   &H00FFFFFF&
            Height          =   1335
            Left            =   2400
            TabIndex        =   12
            Top             =   360
            Width           =   2175
            Begin VB.OptionButton optSend 
               BackColor       =   &H00000000&
               Caption         =   "SendMessage"
               ForeColor       =   &H00FFFFFF&
               Height          =   330
               Index           =   0
               Left            =   120
               TabIndex        =   8
               Top             =   360
               Value           =   -1  'True
               Width           =   1500
            End
            Begin VB.OptionButton optSend 
               BackColor       =   &H00000000&
               Caption         =   "SendMessageTimeout"
               ForeColor       =   &H00FFFFFF&
               Height          =   330
               Index           =   1
               Left            =   120
               TabIndex        =   9
               Top             =   840
               Width           =   1980
            End
         End
         Begin VB.CommandButton cmdGetFromVisible 
            Caption         =   "Get a String from its Textbox"
            Height          =   375
            Left            =   120
            TabIndex        =   7
            Top             =   1320
            Width           =   2175
         End
      End
      Begin VB.Label Label1 
         BackStyle       =   0  'Transparent
         Caption         =   "Enter the String to Send"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   120
         TabIndex        =   13
         Top             =   360
         Width           =   2175
      End
   End
End
Attribute VB_Name = "frmSend"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Const cWINDOW_TITLE = "Receive Data"
Const cHIDDEN_WINDOW_TITLE = "Invisible Window to Receive a String"

Const WM_COPYDATA = &H4A
Const WM_CLOSE = &H10
Const WM_SETTEXT = &HC
Const WM_GETTEXT = &HD
Const WM_COPY = &H301
Const WM_PASTE = &H302
Const MAX_PATH = 260

Private Type COPYDATASTRUCT
    dwData As Long   ' Use this to identify your message
    cbData As Long   ' Number of bytes to be transferred
    lpData As Long   ' Address of data
End Type

Private Declare Function FindWindow Lib "user32" Alias _
   "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName _
   As String) As Long

Private Declare Function SendMessage Lib "user32" Alias _
   "SendMessageA" (ByVal hwnd As Long, ByVal wMsg As Long, ByVal _
   wParam As Long, lParam As Any) As Long

Private Declare Function SendMessageTimeout Lib "user32" Alias _
    "SendMessageTimeoutA" (ByVal hwnd As Long, ByVal Msg As Long, _
    ByVal wParam As Long, lParam As Any, ByVal fuFlags As Long, _
    ByVal uTimeout As Long, lpdwResult As Long) As Long
    
Private Declare Function FindWindowEx Lib "user32" Alias _
    "FindWindowExA" (ByVal hWnd1 As Long, ByVal hWnd2 As Long, _
    ByVal lpsz1 As String, ByVal lpsz2 As String) As Long
'
'Copies a block of memory from one location to another.
'
Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" _
   (hpvDest As Any, hpvSource As Any, ByVal cbCopy As Long)
Private Sub cmdCopyToHidden_Click()
Dim lHandle As Long
'
' Get the handle of the textbox on a hidden window.
' Copy text to the clipboard and paste it onto
' the hidden form's textbox.
'
lHandle = fGetTextBoxHandle(cHIDDEN_WINDOW_TITLE)
If lHandle Then
    Clipboard.Clear
    Call Clipboard.SetText(txtString.Text)
    Call fSendNumber(lHandle, WM_PASTE, 0, 0)
End If
End Sub

Private Sub cmdGetFromVisible_Click()
Dim lHandle As Long
Dim sText   As String
'
' Get the handle of the textbox on a visible window.
' Get the textbox's text.
'
lHandle = fGetTextBoxHandle(cWINDOW_TITLE)
If lHandle = 0 Then Exit Sub

sText = Space$(MAX_PATH)
Call fSendString(lHandle, WM_GETTEXT, MAX_PATH, sText)

If InStr(sText, vbNullChar) Then
    txtString = Left$(sText, InStr(sText, vbNullChar) - 1)
End If
End Sub
Private Sub cmdSendData_Click()
Dim sString As String
Dim lHwnd   As Long
Dim cds     As COPYDATASTRUCT
Dim buf(1 To 255) As Byte

sString = Trim$(txtString)
If sString = "" Then Exit Sub
'
' Get the handle of the target application's visible window.
'
lHwnd = FindWindow(vbNullString, cWINDOW_TITLE)
'
' Copy the string into a byte array,
' converting it to ASCII. Assign lpData
' the address of the byte array.
'
Call CopyMemory(buf(1), ByVal sString, Len(sString))
With cds
    .dwData = 3
    .cbData = Len(sString) + 1
    .lpData = VarPtr(buf(1))
End With
'
' Send the string.
'
Call SendMessage(lHwnd, WM_COPYDATA, Me.hwnd, cds)
End Sub

Private Sub cmdSendNumData_Click()
Dim l       As Long
Dim lUpper  As Long
Dim lLower  As Long
Dim lHandle As Long
Dim cds     As COPYDATASTRUCT
'
' Create an array of doubles.
'
Screen.MousePointer = vbHourglass
ReDim aDoubles(CLng(txtnumItems)) As Double
'
' Get the bounds of the array.
'
lLower = LBound(aDoubles)
lUpper = UBound(aDoubles)
'
' Populate the array.
'
For l = lLower To lUpper
    aDoubles(l) = CDbl(l)
Next
'
' Get the handle of the target application's visible window.
'
lHandle = FindWindow(vbNullString, cWINDOW_TITLE)
'
' Fill the structure.
'
If lHandle Then
    cds.dwData = 1   ' Array of Doubles
    cds.cbData = (lUpper - lLower + 1) * Len(aDoubles(lLower))
    cds.lpData = VarPtr(aDoubles(lLower))
    
    Call SendMessage(lHandle, WM_COPYDATA, Me.hwnd, cds)
End If
Screen.MousePointer = vbDefault
End Sub

Private Sub cmdSendStringData_Click()
Dim l       As Long
Dim lUpper  As Long
Dim lLower  As Long
Dim lHandle As Long
Dim pb      As New PropertyBag
Dim bytes() As Byte
Dim cds     As COPYDATASTRUCT
'
' Create an array of strings.
'
Screen.MousePointer = vbHourglass
ReDim aStrings(CLng(txtnumItems)) As String
'
' Get the bounds of the array.
'
lLower = LBound(aStrings)
lUpper = UBound(aStrings)
'
' Populate the array.
'
For l = lLower To lUpper
    aStrings(l) = "Item #" & l
Next
'
' Get the handle of the target application's visible window.
'
lHandle = FindWindow(vbNullString, cWINDOW_TITLE)

If lHandle Then
    '
    ' Store the string data in a property bag.
    ' Remember the number of elements.
    '
    pb.WriteProperty "NumEls", lUpper
    '
    ' Store each item in the property bag.
    '
    For l = lLower To lUpper
        pb.WriteProperty LTrim$(Str$(l)), aStrings(l)
    Next
    '
    ' Get the binary contents.
    '
    bytes() = pb.Contents
    '
    ' Fill the structure.
    '
    cds.dwData = 2  ' Property bag
    cds.cbData = UBound(bytes) - LBound(bytes) + 1
    cds.lpData = VarPtr(bytes(LBound(bytes)))
    
    Call SendMessage(lHandle, WM_COPYDATA, Me.hwnd, cds)
End If
Screen.MousePointer = vbDefault
End Sub


Private Sub cmdSendToHidden_Click()
Dim lHandle As Long
'
' Get the handle of the textbox on a hidden window.
' Send a string to that textbox.
'
lHandle = fGetTextBoxHandle(cHIDDEN_WINDOW_TITLE)
If lHandle Then
    Call fSendString(lHandle, WM_SETTEXT, 0, txtString.Text)
End If
End Sub
Function fSendString(ByVal hwnd As Long, ByVal uMsg As Long, ByVal wParam As Long, lParam As String) As Long
Dim lResult As Long
'
' Send a String message to a window.
'
If optSend(0) Then
    '
    ' Send a message and wait for the receiving app to complete
    ' its process before this call returns.
    '
    fSendString = SendMessage(hwnd, uMsg, wParam, ByVal lParam)
End If

If optSend(1) Then
    '
    ' Send a message, wait for 100 millisecods.
    '
    If SendMessageTimeout(hwnd, uMsg, wParam, ByVal lParam, 0, 100, lResult) Then
        '
        ' If this function returns true, the result holds the return value.
        '
        fSendString = lResult
    End If
End If
End Function
Function fSendNumber(ByVal hwnd As Long, ByVal uMsg As Long, ByVal wParam As Long, lParam As Long) As Long
Dim lResult As Long
'
' Send a Numeric message to a window.
'
If optSend(0) Then
    '
    ' Send a message and wait for the receiving app to complete
    ' its process before this call returns.
    '
    fSendNumber = SendMessage(hwnd, uMsg, wParam, ByVal lParam)
End If

If optSend(1) Then
    '
    ' Send a message, wait for 100 millisecods.
    '
    If SendMessageTimeout(hwnd, uMsg, wParam, ByVal lParam, 0, 100, lResult) Then
        '
        ' If this function returns true, the result holds the return value.
        '
        fSendNumber = lResult
    End If
End If
End Function
Function fGetTextBoxHandle(sWindowTitle As String) As Long
Dim lHandle As Long
'
' Return the handle of the textbox on a window with a
' specific title. Returns zero if not found.
'
' Get the handle of the window.
'
lHandle = FindWindow(vbNullString, sWindowTitle)
If lHandle Then
    '
    ' Get the handle of the textbox.
    '
    fGetTextBoxHandle = FindWindowEx(lHandle, 0, vbNullString, vbNullString)
End If

If lHandle = 0 Then
    Call MsgBox("Window not found", vbExclamation, "Error Finding Window")
End If

If fGetTextBoxHandle = 0 Then
    Call MsgBox("Textbox not found", vbExclamation, "Error Finding Window")
End If
End Function



