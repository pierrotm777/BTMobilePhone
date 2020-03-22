
'
' * Copyright (C) 2011 Patrick Mezard
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

Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions

Namespace PhoneNumbers
	Friend Class EnumerableFromConstructor(Of T)
		Implements IEnumerable(Of T)
		Private fn_ As Func(Of IEnumerator(Of T))

		Public Sub New(fn As Func(Of IEnumerator(Of T)))
			fn_ = fn
		End Sub

		Public Function GetEnumerator() As IEnumerator(Of T)
			Return fn_()
		End Function

		Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
			Return fn_()
		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
