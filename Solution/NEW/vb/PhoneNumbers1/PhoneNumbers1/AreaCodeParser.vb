
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

'*
' * A utility that generates the binary serialization of the area code/location mappings from
' * human-readable text files. It also generates a configuration file which contains information on
' * data files available for use.
' *
' * <p> The text files must be located in sub-directories of the provided input path. For each input
' * file inputPath/lang/countryCallingCode.txt the corresponding binary file is generated as
' * outputPath/countryCallingCode_lang.
' *
' * @author Philippe Liard
' 

Namespace PhoneNumbers
	Class AreaCodeParser
		Public Shared Function ParseAreaCodeMap(stream As Stream) As AreaCodeMap
			Dim areaCodeMapTemp As New SortedDictionary(Of Integer, [String])()
			Using lines = New StreamReader(stream, Encoding.UTF8)
				Dim line As [String]
				While (InlineAssignHelper(line, lines.ReadLine())) IsNot Nothing
					line = line.Trim()
					If line.Length <= 0 OrElse line(0) = "#"C Then
						Continue While
					End If
					Dim indexOfPipe = line.IndexOf("|"C)
					If indexOfPipe = -1 Then
						Continue While
					End If
					Dim areaCode As [String] = line.Substring(0, indexOfPipe)
					Dim location As [String] = line.Substring(indexOfPipe + 1)
					areaCodeMapTemp(Integer.Parse(areaCode)) = location
				End While
				' Build the corresponding area code map and serialize it to the binary format.
				Dim areaCodeMap As New AreaCodeMap()
				areaCodeMap.readAreaCodeMap(areaCodeMapTemp)
				Return areaCodeMap
			End Using
		End Function
		Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
			target = value
			Return value
		End Function
	End Class
End Namespace

