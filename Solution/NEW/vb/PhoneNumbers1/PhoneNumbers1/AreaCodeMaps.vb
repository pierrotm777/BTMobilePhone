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
'    * A utility that maps phone number prefixes to a string describing the geographical area the prefix
'    * covers.
'    *
'    * @author Shaopeng Jia
'    

	Public Class AreaCodeMap
		Private ReadOnly phoneUtil As PhoneNumberUtil = PhoneNumberUtil.GetInstance()

		Private areaCodeMapStorage As AreaCodeMapStorageStrategy

		Public Function getAreaCodeMapStorage() As AreaCodeMapStorageStrategy
			Return areaCodeMapStorage
		End Function

		'*
'         * Creates an empty {@link AreaCodeMap}. The default constructor is necessary for implementing
'         * {@link Externalizable}. The empty map could later be populated by
'         * {@link #readAreaCodeMap(java.util.SortedMap)} or {@link #readExternal(java.io.ObjectInput)}.
'         

		Public Sub New()
		End Sub

		'*
'         * Gets the size of the provided area code map storage. The map storage passed-in will be filled
'         * as a result.
'         

		Private Shared Function getSizeOfAreaCodeMapStorage(mapStorage As AreaCodeMapStorageStrategy, areaCodeMap As SortedDictionary(Of Integer, [String])) As Integer
			mapStorage.readFromSortedMap(areaCodeMap)
			Return mapStorage.getStorageSize()
		End Function

		Private Function createDefaultMapStorage() As AreaCodeMapStorageStrategy
			Return New DefaultMapStorage()
		End Function

		Private Function createFlyweightMapStorage() As AreaCodeMapStorageStrategy
			Return New FlyweightMapStorage()
		End Function

		'*
'         * Gets the smaller area code map storage strategy according to the provided area code map. It
'         * actually uses (outputs the data to a stream) both strategies and retains the best one which
'         * make this method quite expensive.
'         

		' @VisibleForTesting
		Public Function getSmallerMapStorage(areaCodeMap As SortedDictionary(Of Integer, [String])) As AreaCodeMapStorageStrategy
			Dim flyweightMapStorage As AreaCodeMapStorageStrategy = createFlyweightMapStorage()
			Dim sizeOfFlyweightMapStorage As Integer = getSizeOfAreaCodeMapStorage(flyweightMapStorage, areaCodeMap)

			Dim defaultMapStorage As AreaCodeMapStorageStrategy = createDefaultMapStorage()
			Dim sizeOfDefaultMapStorage As Integer = getSizeOfAreaCodeMapStorage(defaultMapStorage, areaCodeMap)

			Return If(sizeOfFlyweightMapStorage < sizeOfDefaultMapStorage, flyweightMapStorage, defaultMapStorage)
		End Function

		'*
'         * Creates an {@link AreaCodeMap} initialized with {@code sortedAreaCodeMap}.  Note that the
'         * underlying implementation of this method is expensive thus should not be called by
'         * time-critical applications.
'         *
'         * @param sortedAreaCodeMap  a map from phone number prefixes to descriptions of corresponding
'         *     geographical areas, sorted in ascending order of the phone number prefixes as integers.
'         

		Public Sub readAreaCodeMap(sortedAreaCodeMap As SortedDictionary(Of Integer, [String]))
			areaCodeMapStorage = getSmallerMapStorage(sortedAreaCodeMap)
		End Sub

		'*
'         * Returns the description of the geographical area the {@code number} corresponds to. This method
'         * distinguishes the case of an invalid prefix and a prefix for which the name is not available in
'         * the current language. If the description is not available in the current language an empty
'         * string is returned. If no description was found for the provided number, null is returned.
'         *
'         * @param number  the phone number to look up
'         * @return  the description of the geographical area
'         

		Public Function Lookup(number As PhoneNumber) As [String]
			Dim numOfEntries As Integer = areaCodeMapStorage.getNumOfEntries()
			If numOfEntries = 0 Then
				Return Nothing
			End If
			Dim phonePrefix As Long = Long.Parse(number.CountryCode + phoneUtil.GetNationalSignificantNumber(number))
			Dim currentIndex As Integer = numOfEntries - 1
			Dim currentSetOfLengths As List(Of Integer) = areaCodeMapStorage.getPossibleLengths()
			Dim length = currentSetOfLengths.Count
			While length > 0
				Dim possibleLength As Integer = currentSetOfLengths(length - 1)
				Dim phonePrefixStr As [String] = phonePrefix.ToString()
				If phonePrefixStr.Length > possibleLength Then
					phonePrefix = Long.Parse(phonePrefixStr.Substring(0, possibleLength))
				End If
				currentIndex = binarySearch(0, currentIndex, phonePrefix)
				If currentIndex < 0 Then
					Return Nothing
				End If
				Dim currentPrefix As Integer = areaCodeMapStorage.getPrefix(currentIndex)
				If phonePrefix = currentPrefix Then
					Return areaCodeMapStorage.getDescription(currentIndex)
				End If
				While length > 0 AndAlso currentSetOfLengths(length - 1) >= possibleLength
					length -= 1
				End While
			End While
			Return Nothing
		End Function

		'*
'         * Does a binary search for {@code value} in the provided array from {@code start} to {@code end}
'         * (inclusive). Returns the position if {@code value} is found; otherwise, returns the
'         * position which has the largest value that is less than {@code value}. This means if
'         * {@code value} is the smallest, -1 will be returned.
'         

		Private Function binarySearch(start As Integer, [end] As Integer, value As Long) As Integer
			Dim current As Integer = 0
			While start <= [end]
				current = (start + [end]) / 2
				Dim currentValue As Integer = areaCodeMapStorage.getPrefix(current)
				If currentValue = value Then
					Return current
				ElseIf currentValue > value Then
					current -= 1
					[end] = current
				Else
					start = current + 1
				End If
			End While
			Return current
		End Function

		'*
'         * Dumps the mappings contained in the area code map.
'         

		Public Overrides Function ToString() As [String]
			Return areaCodeMapStorage.ToString()
		End Function
	End Class
End Namespace