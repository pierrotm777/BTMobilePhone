
'
' * Copyright (C) 2013 Erez A. Korn
' * Changed usage from XmlDocument to XDocument
' * 
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
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Reflection
Imports System.Xml.Linq
Imports libphonenumber_csharp_portable
Imports System.Diagnostics

Namespace PhoneNumbers
	Public Class BuildMetadataFromXml
		' String constants used to fetch the XML nodes and attributes.
		Private Shared ReadOnly CARRIER_CODE_FORMATTING_RULE As [String] = "carrierCodeFormattingRule"
		Private Shared ReadOnly COUNTRY_CODE As [String] = "countryCode"
		Private Shared ReadOnly EMERGENCY As [String] = "emergency"
		Private Shared ReadOnly EXAMPLE_NUMBER As [String] = "exampleNumber"
		Private Shared ReadOnly FIXED_LINE As [String] = "fixedLine"
		Private Shared ReadOnly FORMAT As [String] = "format"
		Private Shared ReadOnly GENERAL_DESC As [String] = "generalDesc"
		Private Shared ReadOnly INTERNATIONAL_PREFIX As [String] = "internationalPrefix"
		Private Shared ReadOnly INTL_FORMAT As [String] = "intlFormat"
		Private Shared ReadOnly LEADING_DIGITS As [String] = "leadingDigits"
		Private Shared ReadOnly LEADING_ZERO_POSSIBLE As [String] = "leadingZeroPossible"
		Private Shared ReadOnly MAIN_COUNTRY_FOR_CODE As [String] = "mainCountryForCode"
		Private Shared ReadOnly MOBILE As [String] = "mobile"
		Private Shared ReadOnly NATIONAL_NUMBER_PATTERN As [String] = "nationalNumberPattern"
		Private Shared ReadOnly NATIONAL_PREFIX As [String] = "nationalPrefix"
		Private Shared ReadOnly NATIONAL_PREFIX_FORMATTING_RULE As [String] = "nationalPrefixFormattingRule"
		Private Shared ReadOnly NATIONAL_PREFIX_OPTIONAL_WHEN_FORMATTING As [String] = "nationalPrefixOptionalWhenFormatting"
		Private Shared ReadOnly NATIONAL_PREFIX_FOR_PARSING As [String] = "nationalPrefixForParsing"
		Private Shared ReadOnly NATIONAL_PREFIX_TRANSFORM_RULE As [String] = "nationalPrefixTransformRule"
		Private Shared ReadOnly NO_INTERNATIONAL_DIALLING As [String] = "noInternationalDialling"
		Private Shared ReadOnly NUMBER_FORMAT As [String] = "numberFormat"
		Private Shared ReadOnly PAGER As [String] = "pager"
		Private Shared ReadOnly PATTERN As [String] = "pattern"
		Private Shared ReadOnly PERSONAL_NUMBER As [String] = "personalNumber"
		Private Shared ReadOnly POSSIBLE_NUMBER_PATTERN As [String] = "possibleNumberPattern"
		Private Shared ReadOnly PREFERRED_EXTN_PREFIX As [String] = "preferredExtnPrefix"
		Private Shared ReadOnly PREFERRED_INTERNATIONAL_PREFIX As [String] = "preferredInternationalPrefix"
		Private Shared ReadOnly PREMIUM_RATE As [String] = "premiumRate"
		Private Shared ReadOnly SHARED_COST As [String] = "sharedCost"
		Private Shared ReadOnly TOLL_FREE As [String] = "tollFree"
		Private Shared ReadOnly UAN As [String] = "uan"
		Private Shared ReadOnly VOICEMAIL As [String] = "voicemail"
		Private Shared ReadOnly VOIP As [String] = "voip"

		' Build the PhoneMetadataCollection from the input XML file.
		Public Shared Function BuildPhoneMetadataCollection(input As Stream, liteBuild As Boolean) As PhoneMetadataCollection
			Dim document = XDocument.Load(input)
			Dim metadataCollection = New PhoneMetadataCollection.Builder()
			For Each territory As XElement In document.GetElementsByTagName("territory")
				Dim regionCode As [String] = ""
				' For the main metadata file this should always be set, but for other supplementary data
				' files the country calling code may be all that is needed.
				If territory.HasAttribute("id") Then
					regionCode = territory.GetAttribute("id")
				End If
				Dim metadata As PhoneMetadata = LoadCountryMetadata(regionCode, territory, liteBuild)
				metadataCollection.AddMetadata(metadata)
			Next
			Return metadataCollection.Build()
		End Function

		' Build a mapping from a country calling code to the region codes which denote the country/region
		' represented by that country code. In the case of multiple countries sharing a calling code,
		' such as the NANPA countries, the one indicated with "isMainCountryForCode" in the metadata
		' should be first.
		Public Shared Function BuildCountryCodeToRegionCodeMap(metadataCollection As PhoneMetadataCollection) As Dictionary(Of Integer, List(Of [String]))
			Dim countryCodeToRegionCodeMap As New Dictionary(Of Integer, List(Of [String]))()
			For Each metadata As PhoneMetadata In metadataCollection.MetadataList
				Dim regionCode As [String] = metadata.Id
				Dim countryCode As Integer = metadata.CountryCode
				If countryCodeToRegionCodeMap.ContainsKey(countryCode) Then
					If metadata.MainCountryForCode Then
						countryCodeToRegionCodeMap(countryCode).Insert(0, regionCode)
					Else
						countryCodeToRegionCodeMap(countryCode).Add(regionCode)
					End If
				Else
					' For most countries, there will be only one region code for the country calling code.
					Dim listWithRegionCode As New List(Of [String])(1)
					If regionCode.Length > 0 Then
						listWithRegionCode.Add(regionCode)
					End If
					countryCodeToRegionCodeMap(countryCode) = listWithRegionCode
				End If
			Next
			Return countryCodeToRegionCodeMap
		End Function

		Public Shared Function ValidateRE(regex As [String]) As [String]
			Return ValidateRE(regex, False)
		End Function

		Public Shared Function ValidateRE(regex__1 As [String], removeWhitespace As Boolean) As [String]
			' Removes all the whitespace and newline from the regexp. Not using pattern compile options to
			' make it work across programming languages.
			If removeWhitespace Then
				regex__1 = Regex.Replace(regex__1, "\s", "")
			End If
			New Regex(regex__1, RegexOptions.None)
			' return regex itself if it is of correct regex syntax
			' i.e. compile did not fail with a PatternSyntaxException.
			Return regex__1
		End Function

		'*
'        * Returns the national prefix of the provided country element.
'        

		' @VisibleForTesting
		Public Shared Function GetNationalPrefix(element As XElement) As [String]
			Return If(element.HasAttribute(NATIONAL_PREFIX), element.GetAttribute(NATIONAL_PREFIX), "")
		End Function

		Public Shared Function LoadTerritoryTagMetadata(regionCode As [String], element As XElement, nationalPrefix As [String]) As PhoneMetadata.Builder
			Dim metadata = New PhoneMetadata.Builder()
			metadata.SetId(regionCode)
			metadata.SetCountryCode(Integer.Parse(element.GetAttribute(COUNTRY_CODE)))
			If element.HasAttribute(LEADING_DIGITS) Then
				metadata.SetLeadingDigits(ValidateRE(element.GetAttribute(LEADING_DIGITS)))
			End If
			metadata.SetInternationalPrefix(ValidateRE(element.GetAttribute(INTERNATIONAL_PREFIX)))
			If element.HasAttribute(PREFERRED_INTERNATIONAL_PREFIX) Then
				Dim preferredInternationalPrefix As [String] = element.GetAttribute(PREFERRED_INTERNATIONAL_PREFIX)
				metadata.SetPreferredInternationalPrefix(preferredInternationalPrefix)
			End If
			If element.HasAttribute(NATIONAL_PREFIX_FOR_PARSING) Then
				metadata.SetNationalPrefixForParsing(ValidateRE(element.GetAttribute(NATIONAL_PREFIX_FOR_PARSING), True))
				If element.HasAttribute(NATIONAL_PREFIX_TRANSFORM_RULE) Then
					metadata.SetNationalPrefixTransformRule(ValidateRE(element.GetAttribute(NATIONAL_PREFIX_TRANSFORM_RULE)))
				End If
			End If
			If Not [String].IsNullOrEmpty(nationalPrefix) Then
				metadata.SetNationalPrefix(nationalPrefix)
				If Not metadata.HasNationalPrefixForParsing Then
					metadata.SetNationalPrefixForParsing(nationalPrefix)
				End If
			End If
			If element.HasAttribute(PREFERRED_EXTN_PREFIX) Then
				metadata.SetPreferredExtnPrefix(element.GetAttribute(PREFERRED_EXTN_PREFIX))
			End If
			If element.HasAttribute(MAIN_COUNTRY_FOR_CODE) Then
				metadata.SetMainCountryForCode(True)
			End If
			If element.HasAttribute(LEADING_ZERO_POSSIBLE) Then
				metadata.SetLeadingZeroPossible(True)
			End If
			Return metadata
		End Function

		'*
'        * Extracts the pattern for international format. If there is no intlFormat, default to using the
'        * national format. If the intlFormat is set to "NA" the intlFormat should be ignored.
'        *
'        * @throws  RuntimeException if multiple intlFormats have been encountered.
'        * @return  whether an international number format is defined.
'        

		' @VisibleForTesting
		Public Shared Function LoadInternationalFormat(metadata As PhoneMetadata.Builder, numberFormatElement As XElement, nationalFormat As [String]) As Boolean
			Dim intlFormat As New NumberFormat.Builder()
			SetLeadingDigitsPatterns(numberFormatElement, intlFormat)
			intlFormat.SetPattern(numberFormatElement.GetAttribute(PATTERN))
			Dim intlFormatPattern = numberFormatElement.GetElementsByTagName(INTL_FORMAT)
			Dim hasExplicitIntlFormatDefined As Boolean = False

			If intlFormatPattern.Count() > 1 Then
				'LOGGER.log(Level.SEVERE,
				'          "A maximum of one intlFormat pattern for a numberFormat element should be " +
				'           "defined.");
				Throw New Exception("Invalid number of intlFormat patterns for country: " + metadata.Id)
			ElseIf intlFormatPattern.Count() = 0 Then
				' Default to use the same as the national pattern if none is defined.
				intlFormat.SetFormat(nationalFormat)
			Else
				Dim intlFormatPatternValue As [String] = intlFormatPattern.First().Value
				If Not intlFormatPatternValue.Equals("NA") Then
					intlFormat.SetFormat(intlFormatPatternValue)
				End If
				hasExplicitIntlFormatDefined = True
			End If

			If intlFormat.HasFormat Then
				metadata.AddIntlNumberFormat(intlFormat)
			End If
			Return hasExplicitIntlFormatDefined
		End Function

		'*
'         * Extracts the pattern for the national format.
'         *
'         * @throws  RuntimeException if multiple or no formats have been encountered.
'         * @return  the national format string.
'         

		' @VisibleForTesting
		Public Shared Function LoadNationalFormat(metadata As PhoneMetadata.Builder, numberFormatElement As XElement, format__1 As NumberFormat.Builder) As [String]
			SetLeadingDigitsPatterns(numberFormatElement, format__1)
			format__1.SetPattern(ValidateRE(numberFormatElement.GetAttribute(PATTERN)))

			Dim formatPattern = numberFormatElement.GetElementsByTagName(FORMAT)
			If formatPattern.Count() <> 1 Then
				'LOGGER.log(Level.SEVERE,
				'           "Only one format pattern for a numberFormat element should be defined.");
				Throw New Exception("Invalid number of format patterns for country: " + metadata.Id)
			End If
			Dim nationalFormat As [String] = formatPattern.First().Value
			format__1.SetFormat(nationalFormat)
			Return nationalFormat
		End Function

		'*
'        *  Extracts the available formats from the provided DOM element. If it does not contain any
'        *  nationalPrefixFormattingRule, the one passed-in is retained. The nationalPrefix,
'        *  nationalPrefixFormattingRule and nationalPrefixOptionalWhenFormatting values are provided from
'        *  the parent (territory) element.
'        

		' @VisibleForTesting
		Public Shared Sub LoadAvailableFormats(metadata As PhoneMetadata.Builder, element As XElement, nationalPrefix As [String], nationalPrefixFormattingRule As [String], nationalPrefixOptionalWhenFormatting As Boolean)
			Dim carrierCodeFormattingRule As [String] = ""
			If element.HasAttribute(CARRIER_CODE_FORMATTING_RULE) Then
				carrierCodeFormattingRule = ValidateRE(GetDomesticCarrierCodeFormattingRuleFromElement(element, nationalPrefix))
			End If
			Dim numberFormatElements = element.GetElementsByTagName(NUMBER_FORMAT)
			Dim hasExplicitIntlFormatDefined As Boolean = False

			Dim numOfFormatElements As Integer = numberFormatElements.Count()
			If numOfFormatElements > 0 Then
				For Each numberFormatElement As XElement In numberFormatElements
					Dim format = New NumberFormat.Builder()

					If numberFormatElement.HasAttribute(NATIONAL_PREFIX_FORMATTING_RULE) Then
						format.SetNationalPrefixFormattingRule(GetNationalPrefixFormattingRuleFromElement(numberFormatElement, nationalPrefix))

						format.SetNationalPrefixOptionalWhenFormatting(numberFormatElement.HasAttribute(NATIONAL_PREFIX_OPTIONAL_WHEN_FORMATTING))
					Else
						format.SetNationalPrefixFormattingRule(nationalPrefixFormattingRule)
						format.SetNationalPrefixOptionalWhenFormatting(nationalPrefixOptionalWhenFormatting)
					End If
					If numberFormatElement.HasAttribute("carrierCodeFormattingRule") Then
						format.SetDomesticCarrierCodeFormattingRule(ValidateRE(GetDomesticCarrierCodeFormattingRuleFromElement(numberFormatElement, nationalPrefix)))
					Else
						format.SetDomesticCarrierCodeFormattingRule(carrierCodeFormattingRule)
					End If

					' Extract the pattern for the national format.
					Dim nationalFormat As [String] = LoadNationalFormat(metadata, numberFormatElement, format)
					metadata.AddNumberFormat(format)

					If LoadInternationalFormat(metadata, numberFormatElement, nationalFormat) Then
						hasExplicitIntlFormatDefined = True
					End If
				Next
				' Only a small number of regions need to specify the intlFormats in the xml. For the majority
				' of countries the intlNumberFormat metadata is an exact copy of the national NumberFormat
				' metadata. To minimize the size of the metadata file, we only keep intlNumberFormats that
				' actually differ in some way to the national formats.
				If Not hasExplicitIntlFormatDefined Then
					metadata.ClearIntlNumberFormat()
				End If
			End If
		End Sub

		Public Shared Sub SetLeadingDigitsPatterns(numberFormatElement As XElement, format As NumberFormat.Builder)
			For Each e As XElement In numberFormatElement.GetElementsByTagName(LEADING_DIGITS)
				format.AddLeadingDigitsPattern(ValidateRE(e.Value, True))
			Next
		End Sub

		Public Shared Function GetNationalPrefixFormattingRuleFromElement(element As XElement, nationalPrefix As [String]) As [String]
			Dim nationalPrefixFormattingRule As [String] = element.GetAttribute(NATIONAL_PREFIX_FORMATTING_RULE)
			' Replace $NP with national prefix and $FG with the first group ($1).
			nationalPrefixFormattingRule = ReplaceFirst(nationalPrefixFormattingRule, "$NP", nationalPrefix)
			nationalPrefixFormattingRule = ReplaceFirst(nationalPrefixFormattingRule, "$FG", "${1}")
			Return nationalPrefixFormattingRule
		End Function

		Public Shared Function GetDomesticCarrierCodeFormattingRuleFromElement(element As XElement, nationalPrefix As [String]) As [String]
			Dim carrierCodeFormattingRule As [String] = element.GetAttribute(CARRIER_CODE_FORMATTING_RULE)
			' Replace $FG with the first group ($1) and $NP with the national prefix.
			carrierCodeFormattingRule = ReplaceFirst(carrierCodeFormattingRule, "$FG", "${1}")
			carrierCodeFormattingRule = ReplaceFirst(carrierCodeFormattingRule, "$NP", nationalPrefix)
			Return carrierCodeFormattingRule
		End Function

		' @VisibleForTesting
		Public Shared Function IsValidNumberType(numberType As [String]) As Boolean
			Return numberType.Equals(FIXED_LINE) OrElse numberType.Equals(MOBILE) OrElse numberType.Equals(GENERAL_DESC)
		End Function

		'*
'        * Processes a phone number description element from the XML file and returns it as a
'        * PhoneNumberDesc. If the description element is a fixed line or mobile number, the general
'        * description will be used to fill in the whole element if necessary, or any components that are
'        * missing. For all other types, the general description will only be used to fill in missing
'        * components if the type has a partial definition. For example, if no "tollFree" element exists,
'        * we assume there are no toll free numbers for that locale, and return a phone number description
'        * with "NA" for both the national and possible number patterns.
'        *
'        * @param generalDesc  a generic phone number description that will be used to fill in missing
'        *                     parts of the description
'        * @param countryElement  the XML element representing all the country information
'        * @param numberType  the name of the number type, corresponding to the appropriate tag in the XML
'        *                    file with information about that type
'        * @return  complete description of that phone number type
'        

		Public Shared Function ProcessPhoneNumberDescElement(generalDesc As PhoneNumberDesc, countryElement As XElement, numberType As [String], liteBuild As Boolean) As PhoneNumberDesc
			If generalDesc Is Nothing Then
				generalDesc = New PhoneNumberDesc.Builder().Build()
			End If
			Dim phoneNumberDescList = countryElement.GetElementsByTagName(numberType)
			Dim numberDesc = New PhoneNumberDesc.Builder()
			If phoneNumberDescList.Count() = 0 AndAlso Not IsValidNumberType(numberType) Then
				numberDesc.SetNationalNumberPattern("NA")
				numberDesc.SetPossibleNumberPattern("NA")
				Return numberDesc.Build()
			End If
			numberDesc.MergeFrom(generalDesc)
			If phoneNumberDescList.Count() > 0 Then
				Dim element As XElement = phoneNumberDescList.First()
				Dim possiblePattern = element.GetElementsByTagName(POSSIBLE_NUMBER_PATTERN)
				If possiblePattern.Count() > 0 Then
					numberDesc.SetPossibleNumberPattern(ValidateRE(possiblePattern.First().Value, True))
				End If

				Dim validPattern = element.GetElementsByTagName(NATIONAL_NUMBER_PATTERN)
				If validPattern.Count() > 0 Then
					numberDesc.SetNationalNumberPattern(ValidateRE(validPattern.First().Value, True))
				End If

				If Not liteBuild Then
					Dim exampleNumber = element.GetElementsByTagName(EXAMPLE_NUMBER)
					If exampleNumber.Count() > 0 Then
						numberDesc.SetExampleNumber(exampleNumber.First().Value)
					End If
				End If
			End If
			Return numberDesc.Build()
		End Function

		Private Shared Function ReplaceFirst(input As [String], value As [String], replacement As [String]) As [String]
			Dim p = input.IndexOf(value)
			If p >= 0 Then
				input = input.Substring(0, p) + replacement + input.Substring(p + value.Length)
			End If
			Return input
		End Function

		' @VisibleForTesting
		Public Shared Sub LoadGeneralDesc(metadata As PhoneMetadata.Builder, element As XElement, liteBuild As Boolean)
			Dim generalDesc = ProcessPhoneNumberDescElement(Nothing, element, GENERAL_DESC, liteBuild)
			metadata.SetGeneralDesc(generalDesc)

			metadata.SetFixedLine(ProcessPhoneNumberDescElement(generalDesc, element, FIXED_LINE, liteBuild))
			metadata.SetMobile(ProcessPhoneNumberDescElement(generalDesc, element, MOBILE, liteBuild))
			metadata.SetTollFree(ProcessPhoneNumberDescElement(generalDesc, element, TOLL_FREE, liteBuild))
			metadata.SetPremiumRate(ProcessPhoneNumberDescElement(generalDesc, element, PREMIUM_RATE, liteBuild))
			metadata.SetSharedCost(ProcessPhoneNumberDescElement(generalDesc, element, SHARED_COST, liteBuild))
			metadata.SetVoip(ProcessPhoneNumberDescElement(generalDesc, element, VOIP, liteBuild))
			metadata.SetPersonalNumber(ProcessPhoneNumberDescElement(generalDesc, element, PERSONAL_NUMBER, liteBuild))
			metadata.SetPager(ProcessPhoneNumberDescElement(generalDesc, element, PAGER, liteBuild))
			metadata.SetUan(ProcessPhoneNumberDescElement(generalDesc, element, UAN, liteBuild))
			metadata.SetVoicemail(ProcessPhoneNumberDescElement(generalDesc, element, VOICEMAIL, liteBuild))
			metadata.SetEmergency(ProcessPhoneNumberDescElement(generalDesc, element, EMERGENCY, liteBuild))
			metadata.SetNoInternationalDialling(ProcessPhoneNumberDescElement(generalDesc, element, NO_INTERNATIONAL_DIALLING, liteBuild))
			metadata.SetSameMobileAndFixedLinePattern(metadata.Mobile.NationalNumberPattern.Equals(metadata.FixedLine.NationalNumberPattern))
		End Sub

		Public Shared Function LoadCountryMetadata(regionCode As [String], element As XElement, liteBuild As Boolean) As PhoneMetadata
			Dim nationalPrefix As [String] = GetNationalPrefix(element)
			Dim metadata As PhoneMetadata.Builder = LoadTerritoryTagMetadata(regionCode, element, nationalPrefix)
			Dim nationalPrefixFormattingRule As [String] = GetNationalPrefixFormattingRuleFromElement(element, nationalPrefix)
			LoadAvailableFormats(metadata, element, nationalPrefix.ToString(), nationalPrefixFormattingRule.ToString(), element.HasAttribute(NATIONAL_PREFIX_OPTIONAL_WHEN_FORMATTING))
			LoadGeneralDesc(metadata, element, liteBuild)
			Return metadata.Build()
		End Function

		Public Shared Function GetCountryCodeToRegionCodeMap(filePrefix As [String]) As Dictionary(Of Integer, List(Of [String]))
			Dim asm = GetType(BuildMetadataFromXml).GetTypeInfo().Assembly
			Dim name = If(asm.GetManifestResourceNames().Where(Function(n) n.EndsWith(filePrefix)).FirstOrDefault(), "missing")
			Using stream = asm.GetManifestResourceStream(name)
				Dim collection = BuildPhoneMetadataCollection(stream, False)
				Return BuildCountryCodeToRegionCodeMap(collection)
			End Using
		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
