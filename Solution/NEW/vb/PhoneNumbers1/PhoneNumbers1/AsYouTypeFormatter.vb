
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
	'*
'     * A formatter which formats phone numbers as they are entered.
'     *
'     * <p>An AsYouTypeFormatter can be created by invoking
'     * {@link PhoneNumberUtil#getAsYouTypeFormatter}. After that, digits can be added by invoking
'     * {@link #inputDigit} on the formatter instance, and the partially formatted phone number will be
'     * returned each time a digit is added. {@link #clear} can be invoked before formatting a new
'     * number.
'     *
'     * <p>See the unittests for more details on how the formatter is to be used.
'     *
'     * @author Shaopeng Jia
'     

	Public Class AsYouTypeFormatter
		Private currentOutput As [String] = ""
		Private formattingTemplate As New StringBuilder()
		' The pattern from numberFormat that is currently used to create formattingTemplate.
		Private currentFormattingPattern As [String] = ""
		Private accruedInput As New StringBuilder()
		Private accruedInputWithoutFormatting As New StringBuilder()
		' This indicates whether AsYouTypeFormatter is currently doing the formatting.
		Private ableToFormat As Boolean = True
		' Set to true when users enter their own formatting. AsYouTypeFormatter will do no formatting at
		' all when this is set to true.
		Private inputHasFormatting As Boolean = False
		Private isInternationalFormatting As Boolean = False
		Private isExpectingCountryCallingCode As Boolean = False
		Private ReadOnly phoneUtil As PhoneNumberUtil = PhoneNumberUtil.GetInstance()
		Private defaultCountry As [String]

		Private Shared ReadOnly EMPTY_METADATA As PhoneMetadata = New PhoneMetadata.Builder().SetInternationalPrefix("NA").BuildPartial()
		Private defaultMetaData As PhoneMetadata
		Private currentMetaData As PhoneMetadata

		' A pattern that is used to match character classes in regular expressions. An example of a
		' character class is [1-4].
		Private Shared ReadOnly CHARACTER_CLASS_PATTERN As New Regex("\[([^\[\]])*\]", RegexOptions.None)
		' Any digit in a regular expression that actually denotes a digit. For example, in the regular
		' expression 80[0-2]\d{6,10}, the first 2 digits (8 and 0) are standalone digits, but the rest
		' are not.
		' Two look-aheads are needed because the number following \\d could be a two-digit number, since
		' the phone number can be as long as 15 digits.
		Private Shared ReadOnly STANDALONE_DIGIT_PATTERN As New Regex("\d(?=[^,}][^,}])", RegexOptions.None)

		' A pattern that is used to determine if a numberFormat under availableFormats is eligible to be
		' used by the AYTF. It is eligible when the format element under numberFormat contains groups of
		' the dollar sign followed by a single digit, separated by valid phone number punctuation. This
		' prevents invalid punctuation (such as the star sign in Israeli star numbers) getting into the
		' output of the AYTF.
		Private Shared ReadOnly ELIGIBLE_FORMAT_PATTERN As New PhoneRegex("[" + PhoneNumberUtil.VALID_PUNCTUATION + "]*" + "(\$\d" + "[" + PhoneNumberUtil.VALID_PUNCTUATION + "]*)+", RegexOptions.None)

		' This is the minimum length of national number accrued that is required to trigger the
		' formatter. The first element of the leadingDigitsPattern of each numberFormat contains a
		' regular expression that matches up to this number of digits.
		Private Shared ReadOnly MIN_LEADING_DIGITS_LENGTH As Integer = 3

		' The digits that have not been entered yet will be represented by a \u2008, the punctuation
		' space.
		Private digitPlaceholder As [String] = "?"
		Private digitPattern As Regex
		Private lastMatchPosition As Integer = 0
		' The position of a digit upon which inputDigitAndRememberPosition is most recently invoked, as
		' found in the original sequence of characters the user entered.
		Private originalPosition As Integer = 0
		' The position of a digit upon which inputDigitAndRememberPosition is most recently invoked, as
		' found in accruedInputWithoutFormatting.
		Private positionToRemember As Integer = 0
		' This contains anything that has been entered so far preceding the national significant number,
		' and it is formatted (e.g. with space inserted). For example, this can contain IDD, country
		' code, and/or NDD, etc.
		Private prefixBeforeNationalNumber As New StringBuilder()
		' This contains the national prefix that has been extracted. It contains only digits without
		' formatting.
		Private nationalPrefixExtracted As [String] = ""
		Private nationalNumber As New StringBuilder()
		Private possibleFormats As New List(Of NumberFormat)()

		' A cache for frequently used country-specific regular expressions.
		Private regexCache As New RegexCache(64)

		'*
'         * Constructs an as-you-type formatter. Should be obtained from {@link
'         * PhoneNumberUtil#getAsYouTypeFormatter}.
'         *
'         * @param regionCode  the country/region where the phone number is being entered
'         

		Public Sub New(regionCode As [String])
			digitPattern = New Regex(digitPlaceholder, RegexOptions.None)
			defaultCountry = regionCode
			currentMetaData = GetMetadataForRegion(defaultCountry)
			defaultMetaData = currentMetaData
		End Sub

		' The metadata needed by this class is the same for all regions sharing the same country calling
		' code. Therefore, we return the metadata for "main" region for this country calling code.
		Private Function GetMetadataForRegion(regionCode As [String]) As PhoneMetadata
			Dim countryCallingCode As Integer = phoneUtil.GetCountryCodeForRegion(regionCode)
			Dim mainCountry As [String] = phoneUtil.GetRegionCodeForCountryCode(countryCallingCode)
			Dim metadata As PhoneMetadata = phoneUtil.GetMetadataForRegion(mainCountry)
			If metadata IsNot Nothing Then
				Return metadata
			End If
			' Set to a default instance of the metadata. This allows us to function with an incorrect
			' region code, even if formatting only works for numbers specified with "+".
			Return EMPTY_METADATA
		End Function

		' Returns true if a new template is created as opposed to reusing the existing template.
		Private Function MaybeCreateNewTemplate() As Boolean
			' When there are multiple available formats, the formatter uses the first format where a
			' formatting template could be created.
			While possibleFormats.Count > 0
				Dim numberFormat As NumberFormat = possibleFormats(0)
				Dim pattern As [String] = numberFormat.Pattern
				If currentFormattingPattern.Equals(pattern) Then
					Return False
				End If
				If CreateFormattingTemplate(numberFormat) Then
					currentFormattingPattern = pattern
					' With a new formatting template, the matched position using the old template needs to be
					' reset.
					lastMatchPosition = 0
					Return True
				Else
					possibleFormats.RemoveAt(0)
				End If
			End While
			ableToFormat = False
			Return False
		End Function

		Private Sub GetAvailableFormats(leadingThreeDigits As [String])
			Dim formatList As IList(Of NumberFormat) = If((isInternationalFormatting AndAlso currentMetaData.IntlNumberFormatCount > 0), currentMetaData.IntlNumberFormatList, currentMetaData.NumberFormatList)
			For Each format As NumberFormat In formatList
				If IsFormatEligible(format.Format) Then
					possibleFormats.Add(format)
				End If
			Next
			NarrowDownPossibleFormats(leadingThreeDigits)
		End Sub

		Private Function IsFormatEligible(format As [String]) As Boolean
			Return ELIGIBLE_FORMAT_PATTERN.MatchAll(format).Success
		End Function

		Private Sub NarrowDownPossibleFormats(leadingDigits As [String])
			Dim indexOfLeadingDigitsPattern As Integer = leadingDigits.Length - MIN_LEADING_DIGITS_LENGTH
			Dim i As Integer = 0
			While i <> possibleFormats.Count
				Dim format As NumberFormat = possibleFormats(i)
				If format.LeadingDigitsPatternCount > indexOfLeadingDigitsPattern Then
					Dim leadingDigitsPattern = regexCache.GetPatternForRegex(format.LeadingDigitsPatternList(indexOfLeadingDigitsPattern))
					Dim m = leadingDigitsPattern.MatchBeginning(leadingDigits)
					If Not m.Success Then
						possibleFormats.RemoveAt(i)
						Continue While
					End If
				End If
				' else the particular format has no more specific leadingDigitsPattern, and it should be
				' retained.
				i += 1
			End While
		End Sub

		Private Function CreateFormattingTemplate(format As NumberFormat) As Boolean
			Dim numberPattern As [String] = format.Pattern

			' The formatter doesn't format numbers when numberPattern contains "|", e.g.
			' (20|3)\d{4}. In those cases we quickly return.
			If numberPattern.IndexOf("|"C) <> -1 Then
				Return False
			End If

			' Replace anything in the form of [..] with \d
			numberPattern = CHARACTER_CLASS_PATTERN.Replace(numberPattern, "\d")

			' Replace any standalone digit (not the one in d{}) with \d
			numberPattern = STANDALONE_DIGIT_PATTERN.Replace(numberPattern, "\d")
			formattingTemplate.Length = 0
			Dim tempTemplate As [String] = GetFormattingTemplate(numberPattern, format.Format)
			If tempTemplate.Length > 0 Then
				formattingTemplate.Append(tempTemplate)
				Return True
			End If
			Return False
		End Function

		' Gets a formatting template which can be used to efficiently format a partial number where
		' digits are added one by one.
		Private Function GetFormattingTemplate(numberPattern As [String], numberFormat As [String]) As [String]
			' Creates a phone number consisting only of the digit 9 that matches the
			' numberPattern by applying the pattern to the longestPhoneNumber string.
			Dim longestPhoneNumber As [String] = "999999999999999"
			Dim m = regexCache.GetPatternForRegex(numberPattern).Match(longestPhoneNumber)
			Dim aPhoneNumber As [String] = m.Groups(0).Value
			' No formatting template can be created if the number of digits entered so far is longer than
			' the maximum the current formatting rule can accommodate.
			If aPhoneNumber.Length < nationalNumber.Length Then
				Return ""
			End If
			' Formats the number according to numberFormat
			Dim template As [String] = Regex.Replace(aPhoneNumber, numberPattern, numberFormat)
			' Replaces each digit with character digitPlaceholder
			template = template.Replace("9", digitPlaceholder)
			Return template
		End Function

		'*
'         * Clears the internal state of the formatter, so it can be reused.
'         

		Public Sub Clear()
			currentOutput = ""
			accruedInput.Length = 0
			accruedInputWithoutFormatting.Length = 0
			formattingTemplate.Length = 0
			lastMatchPosition = 0
			currentFormattingPattern = ""
			prefixBeforeNationalNumber.Length = 0
			nationalPrefixExtracted = ""
			nationalNumber.Length = 0
			ableToFormat = True
			inputHasFormatting = False
			positionToRemember = 0
			originalPosition = 0
			isInternationalFormatting = False
			isExpectingCountryCallingCode = False
			possibleFormats.Clear()
			If Not currentMetaData.Equals(defaultMetaData) Then
				currentMetaData = GetMetadataForRegion(defaultCountry)
			End If
		End Sub

		'*
'         * Formats a phone number on-the-fly as each digit is entered.
'         *
'         * @param nextChar  the most recently entered digit of a phone number. Formatting characters are
'         *     allowed, but as soon as they are encountered this method formats the number as entered and
'         *     not "as you type" anymore. Full width digits and Arabic-indic digits are allowed, and will
'         *     be shown as they are.
'         * @return  the partially formatted phone number.
'         

		Public Function InputDigit(nextChar As Char) As [String]
			currentOutput = InputDigitWithOptionToRememberPosition(nextChar, False)
			Return currentOutput
		End Function

		'*
'         * Same as {@link #inputDigit}, but remembers the position where {@code nextChar} is inserted, so
'         * that it can be retrieved later by using {@link #getRememberedPosition}. The remembered
'         * position will be automatically adjusted if additional formatting characters are later
'         * inserted/removed in front of {@code nextChar}.
'         

		Public Function InputDigitAndRememberPosition(nextChar As Char) As [String]
			currentOutput = InputDigitWithOptionToRememberPosition(nextChar, True)
			Return currentOutput
		End Function

		Private Function InputDigitWithOptionToRememberPosition(nextChar As Char, rememberPosition As Boolean) As [String]
			accruedInput.Append(nextChar)
			If rememberPosition Then
				originalPosition = accruedInput.Length
			End If
			' We do formatting on-the-fly only when each character entered is either a digit, or a plus
			' sign (accepted at the start of the number only).
			If Not IsDigitOrLeadingPlusSign(nextChar) Then
				ableToFormat = False
				inputHasFormatting = True
			Else
				nextChar = NormalizeAndAccrueDigitsAndPlusSign(nextChar, rememberPosition)
			End If
			If Not ableToFormat Then
				' When we are unable to format because of reasons other than that formatting chars have been
				' entered, it can be due to really long IDDs or NDDs. If that is the case, we might be able
				' to do formatting again after extracting them.
				If inputHasFormatting Then
					Return accruedInput.ToString()
				ElseIf AttemptToExtractIdd() Then
					If AttemptToExtractCountryCallingCode() Then
						Return AttemptToChoosePatternWithPrefixExtracted()
					End If
				ElseIf AbleToExtractLongerNdd() Then
					' Add an additional space to separate long NDD and national significant number for
					' readability.
					prefixBeforeNationalNumber.Append(" ")
					Return AttemptToChoosePatternWithPrefixExtracted()
				End If
				Return accruedInput.ToString()
			End If

			' We start to attempt to format only when at least MIN_LEADING_DIGITS_LENGTH digits (the plus
			' sign is counted as a digit as well for this purpose) have been entered.
			Select Case accruedInputWithoutFormatting.Length
				Case 0, 1, 2
					Return accruedInput.ToString()
				Case 3
					If AttemptToExtractIdd() Then
						isExpectingCountryCallingCode = True
					Else
						' No IDD or plus sign is found, might be entering in national format.
						nationalPrefixExtracted = RemoveNationalPrefixFromNationalNumber()
						Return AttemptToChooseFormattingPattern()
					End If
					goto case default
				Case Else
					If isExpectingCountryCallingCode Then
						If AttemptToExtractCountryCallingCode() Then
							isExpectingCountryCallingCode = False
						End If
						Return prefixBeforeNationalNumber + nationalNumber.ToString()
					End If
					If possibleFormats.Count > 0 Then
						' The formatting pattern is already chosen.
						Dim tempNationalNumber As [String] = InputDigitHelper(nextChar)
						' See if the accrued digits can be formatted properly already. If not, use the results
						' from inputDigitHelper, which does formatting based on the formatting pattern chosen.
						Dim formattedNumber As [String] = AttemptToFormatAccruedDigits()
						If formattedNumber.Length > 0 Then
							Return formattedNumber
						End If
						NarrowDownPossibleFormats(nationalNumber.ToString())
						If MaybeCreateNewTemplate() Then
							Return InputAccruedNationalNumber()
						End If
						Return If(ableToFormat, prefixBeforeNationalNumber + tempNationalNumber, accruedInput.ToString())
					Else
						Return AttemptToChooseFormattingPattern()
					End If
			End Select
		End Function

		Private Function AttemptToChoosePatternWithPrefixExtracted() As [String]
			ableToFormat = True
			isExpectingCountryCallingCode = False
			possibleFormats.Clear()
			Return AttemptToChooseFormattingPattern()
		End Function

		' Some national prefixes are a substring of others. If extracting the shorter NDD doesn't result
		' in a number we can format, we try to see if we can extract a longer version here.
		Private Function AbleToExtractLongerNdd() As Boolean
			If nationalPrefixExtracted.Length > 0 Then
				' Put the extracted NDD back to the national number before attempting to extract a new NDD.
				nationalNumber.Insert(0, nationalPrefixExtracted)
				' Remove the previously extracted NDD from prefixBeforeNationalNumber. We cannot simply set
				' it to empty string because people sometimes enter national prefix after country code, e.g
				' +44 (0)20-1234-5678.
				Dim indexOfPreviousNdd As Integer = prefixBeforeNationalNumber.ToString().LastIndexOf(nationalPrefixExtracted)
				prefixBeforeNationalNumber.Length = indexOfPreviousNdd
			End If
			Return Not nationalPrefixExtracted.Equals(RemoveNationalPrefixFromNationalNumber())
		End Function

		Private Function IsDigitOrLeadingPlusSign(nextChar As Char) As Boolean
			Return Char.IsDigit(nextChar) OrElse (accruedInput.Length = 1 AndAlso PhoneNumberUtil.PLUS_CHARS_PATTERN.MatchAll(Char.ToString(nextChar)).Success)
		End Function

		Private Function AttemptToFormatAccruedDigits() As [String]
			For Each numFormat As NumberFormat In possibleFormats
				Dim m = regexCache.GetPatternForRegex(numFormat.Pattern)
				If m.MatchAll(nationalNumber.ToString()).Success Then
					Dim formattedNumber As [String] = m.Replace(nationalNumber.ToString(), numFormat.Format)
					Return prefixBeforeNationalNumber + formattedNumber
				End If
			Next
			Return ""
		End Function

		'*
'         * Returns the current position in the partially formatted phone number of the character which was
'         * previously passed in as the parameter of {@link #inputDigitAndRememberPosition}.
'         

		Public Function GetRememberedPosition() As Integer
			If Not ableToFormat Then
				Return originalPosition
			End If
			Dim accruedInputIndex As Integer = 0, currentOutputIndex As Integer = 0
			While accruedInputIndex < positionToRemember AndAlso currentOutputIndex < currentOutput.Length
				If accruedInputWithoutFormatting(accruedInputIndex) = currentOutput(currentOutputIndex) Then
					accruedInputIndex += 1
				End If
				currentOutputIndex += 1
			End While
			Return currentOutputIndex
		End Function

		' Attempts to set the formatting template and returns a string which contains the formatted
		' version of the digits entered so far.
		Private Function AttemptToChooseFormattingPattern() As [String]
			' We start to attempt to format only when as least MIN_LEADING_DIGITS_LENGTH digits of national
			' number (excluding national prefix) have been entered.
			If nationalNumber.Length >= MIN_LEADING_DIGITS_LENGTH Then
				GetAvailableFormats(nationalNumber.ToString().Substring(0, MIN_LEADING_DIGITS_LENGTH))
				Return If(MaybeCreateNewTemplate(), InputAccruedNationalNumber(), accruedInput.ToString())
			Else
				Return prefixBeforeNationalNumber + nationalNumber.ToString()
			End If
		End Function

		' Invokes inputDigitHelper on each digit of the national number accrued, and returns a formatted
		' string in the end.
		Private Function InputAccruedNationalNumber() As [String]
			Dim lengthOfNationalNumber As Integer = nationalNumber.Length
			If lengthOfNationalNumber > 0 Then
				Dim tempNationalNumber As [String] = ""
				For i As Integer = 0 To lengthOfNationalNumber - 1
					tempNationalNumber = InputDigitHelper(nationalNumber(i))
				Next
				Return If(ableToFormat, prefixBeforeNationalNumber + tempNationalNumber, accruedInput.ToString())
			Else
				Return prefixBeforeNationalNumber.ToString()
			End If
		End Function

		' Returns the national prefix extracted, or an empty string if it is not present.
		Private Function RemoveNationalPrefixFromNationalNumber() As [String]
			Dim startOfNationalNumber As Integer = 0
			If currentMetaData.CountryCode = 1 AndAlso nationalNumber(0) = "1"C Then
				startOfNationalNumber = 1
				prefixBeforeNationalNumber.Append("1 ")
				isInternationalFormatting = True
			ElseIf currentMetaData.HasNationalPrefixForParsing Then
				Dim m = regexCache.GetPatternForRegex(currentMetaData.NationalPrefixForParsing).MatchBeginning(nationalNumber.ToString())
				If m.Success Then
					' When the national prefix is detected, we use international formatting rules instead of
					' national ones, because national formatting rules could contain local formatting rules
					' for numbers entered without area code.
					isInternationalFormatting = True
					startOfNationalNumber = m.Groups(0).Index + m.Groups(0).Length
					prefixBeforeNationalNumber.Append(nationalNumber.ToString().Substring(0, startOfNationalNumber))
				End If
			End If
			Dim nationalPrefix As [String] = nationalNumber.ToString().Substring(0, startOfNationalNumber)
			nationalNumber.Remove(0, startOfNationalNumber)
			Return nationalPrefix
		End Function

		'*
'         * Extracts IDD and plus sign to prefixBeforeNationalNumber when they are available, and places
'         * the remaining input into nationalNumber.
'         *
'         * @return  true when accruedInputWithoutFormatting begins with the plus sign or valid IDD for
'         *     defaultCountry.
'         

		Private Function AttemptToExtractIdd() As Boolean
			Dim internationalPrefix = regexCache.GetPatternForRegex("\" + PhoneNumberUtil.PLUS_SIGN + "|" + currentMetaData.InternationalPrefix)
			Dim iddMatcher = internationalPrefix.MatchBeginning(accruedInputWithoutFormatting.ToString())
			If iddMatcher.Success Then
				isInternationalFormatting = True
				Dim startOfCountryCallingCode As Integer = iddMatcher.Groups(0).Index + iddMatcher.Groups(0).Length
				nationalNumber.Length = 0
				nationalNumber.Append(accruedInputWithoutFormatting.ToString().Substring(startOfCountryCallingCode))
				prefixBeforeNationalNumber.Length = 0
				prefixBeforeNationalNumber.Append(accruedInputWithoutFormatting.ToString().Substring(0, startOfCountryCallingCode))
				If accruedInputWithoutFormatting(0) <> PhoneNumberUtil.PLUS_SIGN Then
					prefixBeforeNationalNumber.Append(" ")
				End If
				Return True
			End If
			Return False
		End Function

		'*
'         * Extracts the country calling code from the beginning of nationalNumber to
'         * prefixBeforeNationalNumber when they are available, and places the remaining input into
'         * nationalNumber.
'         *
'         * @return  true when a valid country calling code can be found.
'         

		Private Function AttemptToExtractCountryCallingCode() As Boolean
			If nationalNumber.Length = 0 Then
				Return False
			End If
			Dim numberWithoutCountryCallingCode As New StringBuilder()
			Dim countryCode As Integer = phoneUtil.ExtractCountryCode(nationalNumber, numberWithoutCountryCallingCode)
			If countryCode = 0 Then
				Return False
			End If
			nationalNumber.Length = 0
			nationalNumber.Append(numberWithoutCountryCallingCode)
			Dim newRegionCode As [String] = phoneUtil.GetRegionCodeForCountryCode(countryCode)
			If PhoneNumberUtil.REGION_CODE_FOR_NON_GEO_ENTITY.Equals(newRegionCode) Then
				currentMetaData = phoneUtil.GetMetadataForNonGeographicalRegion(countryCode)
			ElseIf Not newRegionCode.Equals(defaultCountry) Then
				currentMetaData = GetMetadataForRegion(newRegionCode)
			End If
			Dim countryCodeString As [String] = countryCode.ToString()
			prefixBeforeNationalNumber.Append(countryCodeString).Append(" ")
			Return True
		End Function

		' Accrues digits and the plus sign to accruedInputWithoutFormatting for later use. If nextChar
		' contains a digit in non-ASCII format (e.g. the full-width version of digits), it is first
		' normalized to the ASCII version. The return value is nextChar itself, or its normalized
		' version, if nextChar is a digit in non-ASCII format. This method assumes its input is either a
		' digit or the plus sign.
		Private Function NormalizeAndAccrueDigitsAndPlusSign(nextChar As Char, rememberPosition As Boolean) As Char
			Dim normalizedChar As Char
			If nextChar = PhoneNumberUtil.PLUS_SIGN Then
				normalizedChar = nextChar
				accruedInputWithoutFormatting.Append(nextChar)
			Else
				normalizedChar = CInt(Char.GetNumericValue(nextChar)).ToString()(0)
				accruedInputWithoutFormatting.Append(normalizedChar)
				nationalNumber.Append(normalizedChar)
			End If
			If rememberPosition Then
				positionToRemember = accruedInputWithoutFormatting.Length
			End If
			Return normalizedChar
		End Function

		Private Function InputDigitHelper(nextChar As Char) As [String]
			Dim digitMatcher = digitPattern.Match(formattingTemplate.ToString(), lastMatchPosition)
			If digitMatcher.Success Then
				'XXX: double match, can we fix that?
				digitMatcher = digitPattern.Match(formattingTemplate.ToString())
				Dim tempTemplate As [String] = digitPattern.Replace(formattingTemplate.ToString(), nextChar.ToString(), 1)
				formattingTemplate.Length = 0
				formattingTemplate.Append(tempTemplate)
				lastMatchPosition = digitMatcher.Groups(0).Index
				Return formattingTemplate.ToString().Substring(0, lastMatchPosition + 1)
			Else
				If possibleFormats.Count = 1 Then
					' More digits are entered than we could handle, and there are no other valid patterns to
					' try.
					ableToFormat = False
				End If
				' else, we just reset the formatting pattern.
				currentFormattingPattern = ""
				Return accruedInput.ToString()
			End If
		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
