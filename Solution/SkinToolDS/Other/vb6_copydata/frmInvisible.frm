VERSION 5.00
Begin VB.Form frmInvisible 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Invisible Window to Receive a String"
   ClientHeight    =   840
   ClientLeft      =   5310
   ClientTop       =   3285
   ClientWidth     =   4395
   ControlBox      =   0   'False
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   840
   ScaleWidth      =   4395
   ShowInTaskbar   =   0   'False
   Begin VB.TextBox txtReceive 
      Height          =   330
      Left            =   120
      TabIndex        =   0
      Top             =   210
      Width           =   4125
   End
End
Attribute VB_Name = "frmInvisible"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Declare Function ReplyMessage Lib "user32" (ByVal lReply As Long) As Long
Private Declare Function InSendMessage Lib "user32" () As Long

Private Sub Form_Load()
'
' Also set ShowInTaskBar=False in the
' development environment.
'
Me.Visible = False
End Sub


Private Sub Form_Unload(Cancel As Integer)
Set frmInvisible = Nothing

End Sub


Private Sub txtReceive_Change()
'
' If the checkbox is checked, return immediately
' to the SendMessage call in Send Data application
' to alow that app to continue to process without
' waiting for this message box to be dismissed.
'
If frmReceive.chkReplyMessage Then Call ReplyMessage(1)
'
' Only show the message box if the contents of
' the textbox where changed from an outside
'
If InSendMessage Then
    Call MsgBox("Got it!" & vbCrLf & txtReceive, vbInformation, "Receiving Application")
End If
End Sub


