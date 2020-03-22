
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
'    * A utility which knows the data files that are available for the geocoder to use. The data files
'    * contain mappings from phone number prefixes to text descriptions, and are organized by country
'    * calling code and language that the text descriptions are in.
'    *
'    * @author Shaopeng Jia
'    

	Public Class MappingFileProvider
		Private numOfEntries As Integer = 0
		Private countryCallingCodes As Integer()
		Private availableLanguages As List(Of HashSet(Of [String]))
		Private Shared ReadOnly LOCALE_NORMALIZATION_MAP As Dictionary(Of [String], [String])

		Shared Sub New()
			Dim normalizationMap = New Dictionary(Of [String], [String])()
			normalizationMap("zh_TW") = "zh_Hant"
			normalizationMap("zh_HK") = "zh_Hant"
			normalizationMap("zh_MO") = "zh_Hant"
			LOCALE_NORMALIZATION_MAP = normalizationMap
		End Sub

		'*
'        * Creates an empty {@link MappingFileProvider}. The default constructor is necessary for
'        * implementing {@link Externalizable}. The empty provider could later be populated by
'        * {@link #readFileConfigs(java.util.SortedMap)} or {@link #readExternal(java.io.ObjectInput)}.
'        

		Public Sub New()
		End Sub

		'*
'         * Initializes an {@link MappingFileProvider} with {@code availableDataFiles}.
'         *
'         * @param availableDataFiles  a map from country calling codes to sets of languages in which data
'         *     files are available for the specific country calling code. The map is sorted in ascending
'         *     order of the country calling codes as integers.
'         

		Public Sub ReadFileConfigs(availableDataFiles As SortedDictionary(Of Integer, HashSet(Of [String])))
			numOfEntries = availableDataFiles.Count
			countryCallingCodes = New Integer(numOfEntries - 1) {}
			availableLanguages = New List(Of HashSet(Of [String]))(numOfEntries)
			Dim index As Integer = 0
			For Each countryCallingCode As Integer In availableDataFiles.Keys
				countryCallingCodes(System.Math.Max(System.Threading.Interlocked.Increment(index),index - 1)) = countryCallingCode
				availableLanguages.Add(New HashSet(Of [String])(availableDataFiles(countryCallingCode)))
			Next
		End Sub

		'*
'         * Returns a string representing the data in this class. The string contains one line for each
'         * country calling code. The country calling code is followed by a '|' and then a list of
'         * comma-separated languages sorted in ascending order.
'         

		Public Overrides Function ToString() As [String]
			Dim output As New StringBuilder()
			For i As Integer = 0 To numOfEntries - 1
				output.Append(countryCallingCodes(i))
				output.Append("|"C)
                For Each lang In availableLanguages(i).OrderBy(Function(a) a)
                    output.Append(lang)
                    output.Append(","c)
                Next
				output.Append(ControlChars.Lf)
			Next
			Return output.ToString()
		End Function

		'*
'         * Gets the name of the file that contains the mapping data for the {@code countryCallingCode} in
'         * the language specified.
'         *
'         * @param countryCallingCode  the country calling code of phone numbers which the data file
'         *     contains
'         * @param language  two-letter lowercase ISO language codes as defined by ISO 639-1
'         * @param script  four-letter titlecase (the first letter is uppercase and the rest of the letters
'         *     are lowercase) ISO script codes as defined in ISO 15924
'         * @param region  two-letter uppercase ISO country codes as defined by ISO 3166-1
'         * @return  the name of the file, or empty string if no such file can be found
'         

		Public Function GetFileName(countryCallingCode As Integer, language As [String], script As [String], region As [String]) As [String]
			If language.Length = 0 Then
				Return ""
			End If
			Dim index As Integer = Array.BinarySearch(countryCallingCodes, countryCallingCode)
			If index < 0 Then
				Return ""
			End If
			Dim setOfLangs = availableLanguages(index)
			If setOfLangs.Count > 0 Then
				Dim languageCode As [String] = FindBestMatchingLanguageCode(setOfLangs, language, script, region)
				If languageCode.Length > 0 Then
					Dim fileName As New StringBuilder()
					fileName.Append(countryCallingCode).Append("_"C).Append(languageCode)
					Return fileName.ToString()
				End If
			End If
			Return ""
		End Function

		Private Function FindBestMatchingLanguageCode(setOfLangs As HashSet(Of [String]), language As [String], script As [String], region As [String]) As [String]
			Dim fullLocale As StringBuilder = ConstructFullLocale(language, script, region)
			Dim fullLocaleStr As [String] = fullLocale.ToString()
			Dim normalizedLocale As [String]
			If LOCALE_NORMALIZATION_MAP.TryGetValue(fullLocaleStr, normalizedLocale) Then
				If setOfLangs.Contains(normalizedLocale) Then
					Return normalizedLocale
				End If
			End If
			If setOfLangs.Contains(fullLocaleStr) Then
				Return fullLocaleStr
			End If

			If OnlyOneOfScriptOrRegionIsEmpty(script, region) Then
				If setOfLangs.Contains(language) Then
					Return language
				End If
			ElseIf script.Length > 0 AndAlso region.Length > 0 Then
				Dim langWithScript As StringBuilder = New StringBuilder(language).Append("_"C).Append(script)
				Dim langWithScriptStr As [String] = langWithScript.ToString()
				If setOfLangs.Contains(langWithScriptStr) Then
					Return langWithScriptStr
				End If

				Dim langWithRegion As StringBuilder = New StringBuilder(language).Append("_"C).Append(region)
				Dim langWithRegionStr As [String] = langWithRegion.ToString()
				If setOfLangs.Contains(langWithRegionStr) Then
					Return langWithRegionStr
				End If

				If setOfLangs.Contains(language) Then
					Return language
				End If
			End If
			Return ""
		End Function

		Private Function OnlyOneOfScriptOrRegionIsEmpty(script As [String], region As [String]) As Boolean
			Return (script.Length = 0 AndAlso region.Length > 0) OrElse (region.Length = 0 AndAlso script.Length > 0)
		End Function

		Private Function ConstructFullLocale(language As [String], script As [String], region As [String]) As StringBuilder
			Dim fullLocale As New StringBuilder(language)
			AppendSubsequentLocalePart(script, fullLocale)
			AppendSubsequentLocalePart(region, fullLocale)
			Return fullLocale
		End Function

		Private Sub AppendSubsequentLocalePart(subsequentLocalePart As [String], fullLocale As StringBuilder)
			If subsequentLocalePart.Length > 0 Then
				fullLocale.Append("_"C).Append(subsequentLocalePart)
			End If
		End Sub
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
