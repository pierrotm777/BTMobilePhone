
'
' * Copyright (C) 2011 Google Inc.
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
	'*
'    * Default area code map storage strategy that is used for data not containing description
'    * duplications. It is mainly intended to avoid the overhead of the string table management when it
'    * is actually unnecessary (i.e no string duplication).
'    *
'    * @author Shaopeng Jia
'    

	Public Class DefaultMapStorage
		Inherits AreaCodeMapStorageStrategy
		Public Sub New()
		End Sub

		Private phoneNumberPrefixes As Integer()
		Private descriptions As [String]()

		Public Overrides Function getPrefix(index As Integer) As Integer
			Return phoneNumberPrefixes(index)
		End Function

		Public Overrides Function getStorageSize() As Integer
			Return phoneNumberPrefixes.Length * 4 + descriptions.Sum(Function(d) d.Length)
		End Function

		Public Overrides Function getDescription(index As Integer) As [String]
			Return descriptions(index)
		End Function

		Public Overrides Sub readFromSortedMap(sortedAreaCodeMap As SortedDictionary(Of Integer, [String]))
			numOfEntries = sortedAreaCodeMap.Count
			phoneNumberPrefixes = New Integer(numOfEntries - 1) {}
			descriptions = New [String](numOfEntries - 1) {}
			Dim index As Integer = 0
			Dim possibleLengthsSet = New HashSet(Of Integer)()
			For Each prefix As Integer In sortedAreaCodeMap.Keys
				phoneNumberPrefixes(index) = prefix
				descriptions(index) = sortedAreaCodeMap(prefix)
				index += 1
				Dim lengthOfPrefix = CInt(Math.Log10(prefix)) + 1
				possibleLengthsSet.Add(lengthOfPrefix)
			Next
			possibleLengths.Clear()
			possibleLengths.AddRange(possibleLengthsSet)
			possibleLengths.Sort()
		End Sub
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
