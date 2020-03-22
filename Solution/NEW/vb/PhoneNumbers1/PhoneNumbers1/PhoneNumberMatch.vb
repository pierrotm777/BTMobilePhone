
'
' * Copyright (C) 2009 Google Inc.
' *
' * Licensed under the Apache License, Version 2.0 (the "License");
' * you may not use this file except in compliance with the License.
' * You may obtain a copy of the License at
' *
' * http://www.apache.org/licenses/LICENSE-2.0
' *
' * Unless required by applicable law or agreed to in writing, software
' * distributed under the License is distributed on an "AS IS" BASIS,
' * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' * See the License for the specific language governing permissions and
' * limitations under the License.
' 

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace PhoneNumbers
	Public Class PhoneNumberMatch
		Public Property Start() As Integer
			Get
				Return m_Start
			End Get
			Private Set
				m_Start = Value
			End Set
		End Property
		Private m_Start As Integer
		Public ReadOnly Property Length() As Integer
			Get
				Return RawString.Length
			End Get
		End Property
		Public Property RawString() As [String]
			Get
				Return m_RawString
			End Get
			Private Set
				m_RawString = Value
			End Set
		End Property
		Private m_RawString As [String]
		Public Property Number() As PhoneNumber
			Get
				Return m_Number
			End Get
			Private Set
				m_Number = Value
			End Set
		End Property
		Private m_Number As PhoneNumber

		Public Sub New(start__1 As Integer, rawString__2 As [String], number__3 As PhoneNumber)
			If start__1 < 0 Then
				Throw New ArgumentException("Start index must be >= 0.")
			End If
			If rawString__2 Is Nothing OrElse number__3 Is Nothing Then
				Throw New ArgumentNullException()
			End If
			Start = start__1
			RawString = rawString__2
			Number = number__3
		End Sub

		Public Overrides Function Equals(obj As Object) As Boolean
			If Me = obj Then
				Return True
			End If
			Dim p = TryCast(obj, PhoneNumberMatch)
			Return p IsNot Nothing AndAlso RawString = p.RawString AndAlso Start = p.Start AndAlso Number.Equals(p.Number)
		End Function

		Public Overrides Function GetHashCode() As Integer
			Dim hash As Integer = [GetType]().GetHashCode()
			hash = hash Xor Start.GetHashCode()
			hash = hash Xor RawString.GetHashCode()
			hash = hash Xor Number.GetHashCode()
			Return hash
		End Function

		Public Overrides Function ToString() As [String]
			Return "PhoneNumberMatch [" + Start + "," + Length + ") " + RawString
		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
