
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

Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Reflection
Imports System.Threading
Imports System.Text
Imports System.Globalization

Namespace PhoneNumbers

	Public Class Locale
		Public Shared ReadOnly ENGLISH As New Locale("en", "GB")
		Public Shared ReadOnly FRENCH As New Locale("fr", "FR")
		Public Shared ReadOnly GERMAN As New Locale("de", "DE")
		Public Shared ReadOnly ITALIAN As New Locale("it", "IT")
		Public Shared ReadOnly KOREAN As New Locale("ko", "KR")
		Public Shared ReadOnly SIMPLIFIED_CHINESE As New Locale("zh", "CN")

		Public ReadOnly Language As [String]
		Public ReadOnly Country As [String]

		Public Sub New(language__1 As [String], countryCode As [String])
			Language = language__1
			Country = countryCode
		End Sub

		Public Function GetDisplayCountry(language__1 As [String]) As [String]
			If [String].IsNullOrEmpty(Country) Then
				Return ""
			End If
			Dim name = GetCountryName(Country, language__1)
			If name IsNot Nothing Then
				Return name
			End If
			Dim lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
			If lang <> language__1 Then
				name = GetCountryName(Country, lang)
				If name IsNot Nothing Then
					Return name
				End If
			End If
			If language__1 <> "en" AndAlso lang <> "en" Then
				name = GetCountryName(Country, "en")
				If name IsNot Nothing Then
					Return name
				End If
			End If
			name = GetCountryName(Country, Language)
			Return If(name, "")
		End Function

		Private Function GetCountryName(country__1 As [String], language As [String]) As [String]
			Dim names = LocaleData.Data(Country)
			Dim name As [String]
			If Not names.TryGetValue(language, name) Then
				Return Nothing
			End If
			If name.Length > 0 AndAlso name(0) = "*"C Then
				Return names(name.Substring(1))
			End If
			Return name
		End Function
	End Class
	'*
'     * An offline geocoder which provides geographical information related to a phone number.
'     *
'     * @author Shaopeng Jia
'     

	Public Class PhoneNumberOfflineGeocoder
		Private Shared instance As PhoneNumberOfflineGeocoder = Nothing
		Private Const MAPPING_DATA_DIRECTORY As [String] = "res.prod_"
		Private Shared thisLock As New [Object]()

		Private ReadOnly phoneUtil As PhoneNumberUtil = PhoneNumberUtil.GetInstance()
		Private ReadOnly phonePrefixDataDirectory As [String]

		' The mappingFileProvider knows for which combination of countryCallingCode and language a phone
		' prefix mapping file is available in the file system, so that a file can be loaded when needed.
		Private mappingFileProvider As New MappingFileProvider()

		' A mapping from countryCallingCode_lang to the corresponding phone prefix map that has been
		' loaded.
		Private availablePhonePrefixMaps As New Dictionary(Of [String], AreaCodeMap)()

		' @VisibleForTesting
		Public Sub New(phonePrefixDataDirectory As [String])
			Me.phonePrefixDataDirectory = phonePrefixDataDirectory
			LoadMappingFileProvider()
		End Sub

		Private Sub LoadMappingFileProvider()
			Dim files = New SortedDictionary(Of Integer, HashSet(Of [String]))()
			Dim asm = GetType(PhoneNumberOfflineGeocoder).GetTypeInfo().Assembly
			Dim allNames = asm.GetManifestResourceNames()
			Dim prefix = asm.GetName().Name + "." + phonePrefixDataDirectory
			Dim names = allNames.Where(Function(n) n.StartsWith(prefix))
			For Each n As var In names
				Dim name = n.Substring(prefix.Length)
				Dim pos = name.IndexOf("_")
				Dim country As Integer
				Try
					country = Integer.Parse(name.Substring(0, pos))
				Catch generatedExceptionName As FormatException
					Throw New Exception("Failed to parse geocoding file name: " + name)
				End Try
				Dim language = name.Substring(pos + 1)
				If Not files.ContainsKey(country) Then
					files(country) = New HashSet(Of [String])()
				End If
				files(country).Add(language)
			Next
			mappingFileProvider = New MappingFileProvider()
			mappingFileProvider.ReadFileConfigs(files)
		End Sub

		Private Function GetPhonePrefixDescriptions(prefixMapKey As Integer, language As [String], script As [String], region As [String]) As AreaCodeMap
			Dim fileName As [String] = mappingFileProvider.GetFileName(prefixMapKey, language, script, region)
			If fileName.Length = 0 Then
				Return Nothing
			End If
			If Not availablePhonePrefixMaps.ContainsKey(fileName) Then
				LoadAreaCodeMapFromFile(fileName)
			End If
			Dim map As AreaCodeMap
			Return If(availablePhonePrefixMaps.TryGetValue(fileName, map), map, Nothing)
		End Function

		Private Sub LoadAreaCodeMapFromFile(fileName As [String])
			Dim asm = GetType(PhoneNumberOfflineGeocoder).GetTypeInfo().Assembly
			Dim prefix = asm.GetName().Name + "." + phonePrefixDataDirectory
			Dim resName = prefix + fileName
			Using fp = asm.GetManifestResourceStream(resName)
				Dim areaCodeMap = AreaCodeParser.ParseAreaCodeMap(fp)
				availablePhonePrefixMaps(fileName) = areaCodeMap
			End Using
		End Sub

		'*
'         * Gets a {@link PhoneNumberOfflineGeocoder} instance to carry out international phone number
'         * geocoding.
'         *
'         * <p> The {@link PhoneNumberOfflineGeocoder} is implemented as a singleton. Therefore, calling
'         * this method multiple times will only result in one instance being created.
'         *
'         * @return  a {@link PhoneNumberOfflineGeocoder} instance
'         

		Public Shared Function GetInstance() As PhoneNumberOfflineGeocoder
			SyncLock thisLock
				If instance Is Nothing Then
					instance = New PhoneNumberOfflineGeocoder(MAPPING_DATA_DIRECTORY)
				End If
				Return instance
			End SyncLock
		End Function

		'*
'         * Preload the data file for the given language and country calling code, so that a future lookup
'         * for this language and country calling code will not incur any file loading.
'         *
'         * @param locale  specifies the language of the data file to load
'         * @param countryCallingCode   specifies the country calling code of phone numbers that are
'         *     contained by the file to be loaded
'         

		Public Sub LoadDataFile(locale As Locale, countryCallingCode As Integer)
			instance.GetPhonePrefixDescriptions(countryCallingCode, locale.Language, "", locale.Country)
		End Sub

		'*
'         * Returns the customary display name in the given language for the given territory the phone
'         * number is from.
'         

		Private Function GetCountryNameForNumber(number As PhoneNumber, language As Locale) As [String]
			Dim regionCode As [String] = phoneUtil.GetRegionCodeForNumber(number)
			Return GetRegionDisplayName(regionCode, language)
		End Function

		'*
'        * Returns the customary display name in the given language for the given region.
'        

		Private Function GetRegionDisplayName(regionCode As [String], language As Locale) As [String]
			Return If((regionCode Is Nothing OrElse regionCode.Equals("ZZ") OrElse regionCode.Equals(PhoneNumberUtil.REGION_CODE_FOR_NON_GEO_ENTITY)), "", New Locale("", regionCode).GetDisplayCountry(language.Language))
		End Function

		'*
'        * Returns a text description for the given phone number, in the language provided. The
'        * description might consist of the name of the country where the phone number is from, or the
'        * name of the geographical area the phone number is from if more detailed information is
'        * available.
'        *
'        * <p>This method assumes the validity of the number passed in has already been checked.
'        *
'        * @param number  a valid phone number for which we want to get a text description
'        * @param languageCode  the language code for which the description should be written
'        * @return  a text description for the given language code for the given phone number
'        

		Public Function GetDescriptionForValidNumber(number As PhoneNumber, languageCode As Locale) As [String]
			Dim langStr As [String] = languageCode.Language
			Dim scriptStr As [String] = ""
			' No script is specified
			Dim regionStr As [String] = languageCode.Country

			Dim areaDescription As [String] = GetAreaDescriptionForNumber(number, langStr, scriptStr, regionStr)
			Return If((areaDescription.Length > 0), areaDescription, GetCountryNameForNumber(number, languageCode))
		End Function

		'*
'        * As per {@link #getDescriptionForValidNumber(PhoneNumber, Locale)} but also considers the
'        * region of the user. If the phone number is from the same region as the user, only a lower-level
'        * description will be returned, if one exists. Otherwise, the phone number's region will be
'        * returned, with optionally some more detailed information.
'        *
'        * <p>For example, for a user from the region "US" (United States), we would show "Mountain View,
'        * CA" for a particular number, omitting the United States from the description. For a user from
'        * the United Kingdom (region "GB"), for the same number we may show "Mountain View, CA, United
'        * States" or even just "United States".
'        *
'        * <p>This method assumes the validity of the number passed in has already been checked.
'        *
'        * @param number  the phone number for which we want to get a text description
'        * @param languageCode  the language code for which the description should be written
'        * @param userRegion  the region code for a given user. This region will be omitted from the
'        *     description if the phone number comes from this region. It is a two-letter uppercase ISO
'        *     country code as defined by ISO 3166-1.
'        * @return  a text description for the given language code for the given phone number, or empty
'        *     string if the number passed in is invalid
'        

		Public Function GetDescriptionForValidNumber(number As PhoneNumber, languageCode As Locale, userRegion As [String]) As [String]
			' If the user region matches the number's region, then we just show the lower-level
			' description, if one exists - if no description exists, we will show the region(country) name
			' for the number.
			Dim regionCode As [String] = phoneUtil.GetRegionCodeForNumber(number)
			If userRegion.Equals(regionCode) Then
				Return GetDescriptionForValidNumber(number, languageCode)
			End If
			' Otherwise, we just show the region(country) name for now.
			Return GetRegionDisplayName(regionCode, languageCode)
			' TODO: Concatenate the lower-level and country-name information in an appropriate
			' way for each language.
		End Function

		'*
'        * As per {@link #getDescriptionForValidNumber(PhoneNumber, Locale)} but explicitly checks
'        * the validity of the number passed in.
'        *
'        * @param number  the phone number for which we want to get a text description
'        * @param languageCode  the language code for which the description should be written
'        * @return  a text description for the given language code for the given phone number, or empty
'        *     string if the number passed in is invalid
'        

		Public Function GetDescriptionForNumber(number As PhoneNumber, languageCode As Locale) As [String]
			If Not phoneUtil.IsValidNumber(number) Then
				Return ""
			End If
			Return GetDescriptionForValidNumber(number, languageCode)
		End Function

		'*
'        * As per {@link #getDescriptionForValidNumber(PhoneNumber, Locale, String)} but
'        * explicitly checks the validity of the number passed in.
'        *
'        * @param number  the phone number for which we want to get a text description
'        * @param languageCode  the language code for which the description should be written
'        * @param userRegion  the region code for a given user. This region will be omitted from the
'        *     description if the phone number comes from this region. It is a two-letter uppercase ISO
'        *     country code as defined by ISO 3166-1.
'        * @return  a text description for the given language code for the given phone number, or empty
'        *     string if the number passed in is invalid
'        

		Public Function GetDescriptionForNumber(number As PhoneNumber, languageCode As Locale, userRegion As [String]) As [String]
			If Not phoneUtil.IsValidNumber(number) Then
				Return ""
			End If
			Return GetDescriptionForValidNumber(number, languageCode, userRegion)
		End Function

		Private Function GetAreaDescriptionForNumber(number As PhoneNumber, lang As [String], script As [String], region As [String]) As [String]
			Dim countryCallingCode As Integer = number.CountryCode
			' As the NANPA data is split into multiple files covering 3-digit areas, use a phone number
			' prefix of 4 digits for NANPA instead, e.g. 1650.
			'int phonePrefix = (countryCallingCode != 1) ?
			'    countryCallingCode : (1000 + (int) (number.NationalNumber / 10000000));
			Dim phonePrefix As Integer = countryCallingCode

			Dim phonePrefixDescriptions As AreaCodeMap = GetPhonePrefixDescriptions(phonePrefix, lang, script, region)
			Dim description As [String] = If((phonePrefixDescriptions IsNot Nothing), phonePrefixDescriptions.Lookup(number), Nothing)
			' When a location is not available in the requested language, fall back to English.
			If (description Is Nothing OrElse description.Length = 0) AndAlso MayFallBackToEnglish(lang) Then
				Dim defaultMap As AreaCodeMap = GetPhonePrefixDescriptions(phonePrefix, "en", "", "")
				If defaultMap Is Nothing Then
					Return ""
				End If
				description = defaultMap.Lookup(number)
			End If
			Return If(description IsNot Nothing, description, "")
		End Function

		Private Function MayFallBackToEnglish(lang As [String]) As Boolean
			' Don't fall back to English if the requested language is among the following:
			' - Chinese
			' - Japanese
			' - Korean
			Return Not lang.Equals("zh") AndAlso Not lang.Equals("ja") AndAlso Not lang.Equals("ko")
		End Function

	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
