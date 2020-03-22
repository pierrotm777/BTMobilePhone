
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
'    * Abstracts the way area code data is stored into memory and serialized to a stream. It is used by
'    * {@link AreaCodeMap} to support the most space-efficient storage strategy according to the
'    * provided data.
'    *
'    * @author Philippe Liard
'    

	Public MustInherit Class AreaCodeMapStorageStrategy
		Protected numOfEntries As Integer = 0
		Protected ReadOnly possibleLengths As New List(Of Integer)()

		'*
'         * Gets the phone number prefix located at the provided {@code index}.
'         *
'         * @param index  the index of the prefix that needs to be returned
'         * @return  the phone number prefix at the provided index
'         

		Public MustOverride Function getPrefix(index As Integer) As Integer

		Public MustOverride Function getStorageSize() As Integer

		'*
'         * Gets the description corresponding to the phone number prefix located at the provided {@code
'         * index}. If the description is not available in the current language an empty string is
'         * returned.
'         *
'         * @param index  the index of the phone number prefix that needs to be returned
'         * @return  the description corresponding to the phone number prefix at the provided index
'         

		Public MustOverride Function getDescription(index As Integer) As [String]

		'*
'         * Sets the internal state of the underlying storage implementation from the provided {@code
'         * sortedAreaCodeMap} that maps phone number prefixes to description strings.
'         *
'         * @param sortedAreaCodeMap  a sorted map that maps phone number prefixes including country
'         *    calling code to description strings
'         

		Public MustOverride Sub readFromSortedMap(sortedAreaCodeMap As SortedDictionary(Of Integer, [String]))

		'*
'         * @return  the number of entries contained in the area code map
'         

		Public Function getNumOfEntries() As Integer
			Return numOfEntries
		End Function

		'*
'         * @return  the set containing the possible lengths of prefixes
'         

		Public Function getPossibleLengths() As List(Of Integer)
			Return possibleLengths
		End Function

		Public Overrides Function ToString() As [String]
			Dim output As New StringBuilder()
			Dim numOfEntries As Integer = getNumOfEntries()
			For i As Integer = 0 To numOfEntries - 1
				output.Append(getPrefix(i)).Append("|").Append(getDescription(i)).Append(vbLf)
			Next
			Return output.ToString()
		End Function

	End Class
End Namespace


