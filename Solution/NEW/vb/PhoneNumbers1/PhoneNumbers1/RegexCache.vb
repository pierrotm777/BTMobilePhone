
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
Imports System.Text.RegularExpressions

Namespace PhoneNumbers
	Public Class RegexCache
		Private Class Entry
			Public Regex As PhoneRegex
			Public Node As LinkedListNode(Of [String])
		End Class

		Private size_ As Integer
		Private lru_ As LinkedList(Of [String])
		Private cache_ As Dictionary(Of [String], Entry)

		Public Sub New(size As Integer)
			size_ = size
			cache_ = New Dictionary(Of [String], Entry)(size)
			lru_ = New LinkedList(Of [String])()
		End Sub

		Public Function GetPatternForRegex(regex As [String]) As PhoneRegex
			SyncLock Me
				Dim e As Entry = Nothing
				If Not cache_.TryGetValue(regex, e) Then
					' Insert new node
					Dim r = New PhoneRegex(regex)
					Dim n = lru_.AddFirst(regex)
					cache_(regex) = InlineAssignHelper(e, New Entry() With { _
						Key .Regex = r, _
						Key .Node = n _
					})
					' Check cache size
					If lru_.Count > size_ Then
						Dim o = lru_.Last.Value
						cache_.Remove(o)
						lru_.RemoveLast()
					End If
				Else
					If e.Node <> lru_.First Then
						lru_.Remove(e.Node)
						lru_.AddFirst(e.Node)
					End If
				End If
				Return e.Regex
			End SyncLock
		End Function

		' This method is used for testing.
		Public Function ContainsRegex(regex As [String]) As Boolean
			SyncLock Me
				Return cache_.ContainsKey(regex)
			End SyncLock
		End Function
		Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
			target = value
			Return value
		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
