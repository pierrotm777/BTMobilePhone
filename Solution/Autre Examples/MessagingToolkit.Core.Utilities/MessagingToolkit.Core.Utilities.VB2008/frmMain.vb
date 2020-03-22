'===============================================================================
' OSML - Open Source Messaging Library
'
'===============================================================================
' Copyright © TWIT88.COM.  All rights reserved.
'
' This file is part of Open Source Messaging Library.
'
' Open Source Messaging Library is free software: you can redistribute it 
' and/or modify it under the terms of the GNU General Public License version 3.
'
' Open Source Messaging Library is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this software.  If not, see <http://www.gnu.org/licenses/>.
'===============================================================================
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

''' <summary>
''' Main form
''' </summary>
Partial Public Class frmMain
    Inherits Form

    Private Sub btnMMSMM1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMMSMM1.Click, Button7.Click, Button1.Click
        Dim mm1Form As New frmMM1()
        mm1Form.ShowDialog(Me)
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub btnSmpp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSmpp.Click, Button8.Click, Button4.Click
        Dim smppForm As New frmSMPP()
        smppForm.ShowDialog(Me)
    End Sub

    Private Sub frmMain_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class