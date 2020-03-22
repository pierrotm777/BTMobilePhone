
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
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace PhoneNumbers
	'*
'    * Flyweight area code map storage strategy that uses a table to store unique strings and shorts to
'    * store the prefix and description indexes when possible. It is particularly space-efficient when
'    * the provided area code map contains a lot of description redundant descriptions.
'    *
'    * @author Philippe Liard
'    

	Public Class FlyweightMapStorage
		Inherits AreaCodeMapStorageStrategy
		Private Class ByteBuffer
			Private stream As MemoryStream
			Private reader As BinaryReader
			Private writer As BinaryWriter

			Public Sub New(size As Integer)
				stream = New MemoryStream(New Byte(size - 1) {})
				reader = New BinaryReader(stream)
				writer = New BinaryWriter(stream)
			End Sub

			Public Sub putShort(offset As Integer, value As Short)
				stream.Seek(offset, SeekOrigin.Begin)
				writer.Write(value)
			End Sub

			Public Sub putInt(offset As Integer, value As Integer)
				stream.Seek(offset, SeekOrigin.Begin)
				writer.Write(value)
			End Sub

			Public Function getShort(offset As Integer) As Short
				stream.Seek(offset, SeekOrigin.Begin)
				Return reader.ReadInt16()
			End Function

			Public Function getInt(offset As Integer) As Integer
				stream.Seek(offset, SeekOrigin.Begin)
				Return reader.ReadInt32()
			End Function

			Public Function getCapacity() As Integer
				Return stream.Capacity
			End Function
		End Class

		' Size of short and integer types in bytes.
		Private Shared ReadOnly SHORT_NUM_BYTES As Integer = 2
		Private Shared ReadOnly INT_NUM_BYTES As Integer = 4

		' The number of bytes used to store a phone number prefix.
		Private prefixSizeInBytes As Integer
		' The number of bytes used to store a description index. It is computed from the size of the
		' description pool containing all the strings.
		Private descIndexSizeInBytes As Integer

		Private phoneNumberPrefixes As ByteBuffer
		Private descriptionIndexes As ByteBuffer

		' Sorted string array of unique description strings.
		Private descriptionPool As [String]()

		Public Sub New()
		End Sub

		Public Overrides Function getPrefix(index As Integer) As Integer
			Return readWordFromBuffer(phoneNumberPrefixes, prefixSizeInBytes, index)
		End Function

		Public Overrides Function getStorageSize() As Integer
			Return phoneNumberPrefixes.getCapacity() + descriptionIndexes.getCapacity() + descriptionPool.Sum(Function(d) d.Length)
		End Function

		'*
'        * This implementation returns the same string (same identity) when called for multiple indexes
'        * corresponding to prefixes that have the same description.
'        

		Public Overrides Function getDescription(index As Integer) As [String]
			Dim indexInDescriptionPool As Integer = readWordFromBuffer(descriptionIndexes, descIndexSizeInBytes, index)
			Return descriptionPool(indexInDescriptionPool)
		End Function

		Public Overrides Sub readFromSortedMap(areaCodeMap As SortedDictionary(Of Integer, [String]))
			Dim descriptionsSet = New HashSet(Of [String])()
			numOfEntries = areaCodeMap.Count
			prefixSizeInBytes = getOptimalNumberOfBytesForValue(areaCodeMap.Keys.Last())
			phoneNumberPrefixes = New ByteBuffer(numOfEntries * prefixSizeInBytes)

			' Fill the phone number prefixes byte buffer, the set of possible lengths of prefixes and the
			' description set.
			Dim index As Integer = 0
			Dim possibleLengthsSet = New HashSet(Of Integer)()
			For Each entry As var In areaCodeMap
				Dim prefix As Integer = entry.Key
				storeWordInBuffer(phoneNumberPrefixes, prefixSizeInBytes, index, prefix)
				Dim lengthOfPrefixRef = CInt(Math.Log10(prefix)) + 1
				possibleLengthsSet.Add(lengthOfPrefixRef)
				descriptionsSet.Add(entry.Value)
				index += 1
			Next
			possibleLengths.Clear()
			possibleLengths.AddRange(possibleLengthsSet)
			possibleLengths.Sort()
			createDescriptionPool(descriptionsSet, areaCodeMap)
		End Sub

		'*
'        * Creates the description pool from the provided set of string descriptions and area code map.
'        

		Private Sub createDescriptionPool(descriptionsSet As HashSet(Of [String]), areaCodeMap As SortedDictionary(Of Integer, [String]))
			' Create the description pool.
			descIndexSizeInBytes = getOptimalNumberOfBytesForValue(descriptionsSet.Count - 1)
			descriptionIndexes = New ByteBuffer(numOfEntries * descIndexSizeInBytes)
			descriptionPool = descriptionsSet.ToArray()
			Array.Sort(descriptionPool)

			' Map the phone number prefixes to the descriptions.
			Dim index As Integer = 0
			For i As Integer = 0 To numOfEntries - 1
				Dim prefix As Integer = readWordFromBuffer(phoneNumberPrefixes, prefixSizeInBytes, i)
				Dim description As [String] = areaCodeMap(prefix)
				Dim positionInDescriptionPool As Integer = Array.BinarySearch(descriptionPool, description)
				storeWordInBuffer(descriptionIndexes, descIndexSizeInBytes, index, positionInDescriptionPool)
				index += 1
			Next
		End Sub

		'*
'         * Gets the minimum number of bytes that can be used to store the provided {@code value}.
'         

		Private Shared Function getOptimalNumberOfBytesForValue(value As Integer) As Integer
			Return If(value <= Short.MaxValue, SHORT_NUM_BYTES, INT_NUM_BYTES)
		End Function

		'*
'         * Stores the provided {@code value} to the provided byte {@code buffer} at the specified {@code
'         * index} using the provided {@code wordSize} in bytes. Note that only integer and short sizes are
'         * supported.
'         *
'         * @param buffer  the byte buffer to which the value is stored
'         * @param wordSize  the number of bytes used to store the provided value
'         * @param index  the index to which the value is stored
'         * @param value  the value that is stored assuming it does not require more than the specified
'         *    number of bytes.
'         

		Private Shared Sub storeWordInBuffer(buffer As ByteBuffer, wordSize As Integer, index As Integer, value As Integer)
			index *= wordSize
			If wordSize = SHORT_NUM_BYTES Then
				buffer.putShort(index, CShort(value))
			Else
				buffer.putInt(index, value)
			End If
		End Sub

		'*
'         * Reads the {@code value} at the specified {@code index} from the provided byte {@code buffer}.
'         * Note that only integer and short sizes are supported.
'         *
'         * @param buffer  the byte buffer from which the value is read
'         * @param wordSize  the number of bytes used to store the value
'         * @param index  the index where the value is read from
'         *
'         * @return  the value read from the buffer
'         

		Private Shared Function readWordFromBuffer(buffer As ByteBuffer, wordSize As Integer, index As Integer) As Integer
			index *= wordSize
			Return If(wordSize = SHORT_NUM_BYTES, buffer.getShort(index), buffer.getInt(index))
		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
