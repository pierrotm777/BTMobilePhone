VERSION 5.00
Begin VB.Form frmReceive 
   BackColor       =   &H00000000&
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Receive Data"
   ClientHeight    =   3660
   ClientLeft      =   8370
   ClientTop       =   3240
   ClientWidth     =   2910
   Icon            =   "frmReceive.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3660
   ScaleWidth      =   2910
   Begin VB.ListBox lstArray 
      BackColor       =   &H8000000F&
      ForeColor       =   &H00000000&
      Height          =   2205
      Left            =   120
      TabIndex        =   3
      Top             =   1320
      Width           =   2655
   End
   Begin VB.CheckBox chkReplyMessage 
      BackColor       =   &H00000000&
      Caption         =   "Send Reply"
      ForeColor       =   &H00FFFFFF&
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   720
      Width           =   1575
   End
   Begin VB.TextBox txtString 
      BackColor       =   &H8000000F&
      ForeColor       =   &H00000000&
      Height          =   285
      Left            =   120
      TabIndex        =   0
      Top             =   360
      Width           =   2655
   End
   Begin VB.Label Label2 
      BackStyle       =   0  'Transparent
      Caption         =   "Here is the data that was sent:"
      ForeColor       =   &H00FFFFFF&
      Height          =   255
      Left            =   120
      TabIndex        =   4
      Top             =   1080
      Width           =   2415
   End
   Begin VB.Label Label1 
      BackStyle       =   0  'Transparent
      Caption         =   "Here is the string that was sent:"
      ForeColor       =   &H00FFFFFF&
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   2415
   End
End
Attribute VB_Name = "frmReceive"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private Sub Form_Load()
'
' Load an invisible form containing a textbox
' to receive a string from the sending
' application.
'
Load frmInvisible
' Get this form's handle.
' Subclass this form to trap Windows messages
' so we know when a message is send from the
' sending application.
'
lHwnd = Me.hwnd
Call pHook
End Sub

Private Sub Form_Unload(Cancel As Integer)
Unload frmInvisible
'
' Un-subclass the form.
'
Call pUnhook
End Sub


