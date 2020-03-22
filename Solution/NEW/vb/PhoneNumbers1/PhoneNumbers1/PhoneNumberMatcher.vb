
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

Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions

Namespace PhoneNumbers
	Public Class PhoneNumberMatcher
		Implements IEnumerator(Of PhoneNumberMatch)
		'*
'        * The phone number pattern used by {@link #find}, similar to
'        * {@code PhoneNumberUtil.VALID_PHONE_NUMBER}, but with the following differences:
'        * <ul>
'        *   <li>All captures are limited in order to place an upper bound to the text matched by the
'        *       pattern.
'        * <ul>
'        *   <li>Leading punctuation / plus signs are limited.
'        *   <li>Consecutive occurrences of punctuation are limited.
'        *   <li>Number of digits is limited.
'        * </ul>
'        *   <li>No whitespace is allowed at the start or end.
'        *   <li>No alpha digits (vanity numbers such as 1-800-SIX-FLAGS) are currently supported.
'        * </ul>
'        

		Private Shared ReadOnly PATTERN As Regex

		'*
'        * Matches strings that look like publication pages. Example:
'        * <pre>Computing Complete Answers to Queries in the Presence of Limited Access Patterns.
'        * Chen Li. VLDB J. 12(3): 211-227 (2003).</pre>
'        *
'        * The string "211-227 (2003)" is not a telephone number.
'        

		Private Shared ReadOnly PUB_PAGES As New Regex("\d{1,5}-+\d{1,5}\s{0,4}\(\d{1,4}", RegexOptions.None)

		'*
'        * Matches strings that look like dates using "/" as a separator. Examples: 3/10/2011, 31/10/96 or
'        * 08/31/95.
'        

		Private Shared ReadOnly SLASH_SEPARATED_DATES As New Regex("(?:(?:[0-3]?\d/[01]?\d)|(?:[01]?\d/[0-3]?\d))/(?:[12]\d)?\d{2}", RegexOptions.None)

		'*
'        * Matches timestamps. Examples: "2012-01-02 08:00". Note that the reg-ex does not include the
'        * trailing ":\d\d" -- that is covered by TIME_STAMPS_SUFFIX.
'        

		Private Shared ReadOnly TIME_STAMPS As New Regex("[12]\d{3}[-/]?[01]\d[-/]?[0-3]\d [0-2]\d$", RegexOptions.None)
		Private Shared ReadOnly TIME_STAMPS_SUFFIX As New PhoneRegex(":[0-5]\d", RegexOptions.None)


		'*
'        * Pattern to check that brackets match. Opening brackets should be closed within a phone number.
'        * This also checks that there is something inside the brackets. Having no brackets at all is also
'        * fine.
'        

		Private Shared ReadOnly MATCHING_BRACKETS As PhoneRegex

		'*
'        * Punctuation that may be at the start of a phone number - brackets and plus signs.
'        

		Private Shared ReadOnly LEAD_CLASS As PhoneRegex

		'*
'        * Matches white-space, which may indicate the end of a phone number and the start of something
'        * else (such as a neighbouring zip-code). If white-space is found, continues to match all
'        * characters that are not typically used to start a phone number.
'        

		Private Shared ReadOnly GROUP_SEPARATOR As PhoneRegex

		Shared Sub New()
			' Builds the MATCHING_BRACKETS and PATTERN regular expressions. The building blocks below exist
'            * to make the pattern more easily understood. 


			Dim openingParens As [String] = "(\[(["
			Dim closingParens As [String] = ")\])]"
			Dim nonParens As [String] = "[^" + openingParens + closingParens + "]"

			' Limit on the number of pairs of brackets in a phone number. 

			Dim bracketPairLimit As [String] = Limit(0, 3)
			'
'            * An opening bracket at the beginning may not be closed, but subsequent ones should be.  It's
'            * also possible that the leading bracket was dropped, so we shouldn't be surprised if we see a
'            * closing bracket first. We limit the sets of brackets in a phone number to four.
'            

			MATCHING_BRACKETS = New PhoneRegex("(?:[" + openingParens + "])?" + "(?:" + nonParens + "+" + "[" + closingParens + "])?" + nonParens + "+" + "(?:[" + openingParens + "]" + nonParens + "+[" + closingParens + "])" + bracketPairLimit + nonParens + "*", RegexOptions.None)

			' Limit on the number of leading (plus) characters. 

			Dim leadLimit As [String] = Limit(0, 2)
			' Limit on the number of consecutive punctuation characters. 

			Dim punctuationLimit As [String] = Limit(0, 4)
			' The maximum number of digits allowed in a digit-separated block. As we allow all digits in a
'            * single block, set high enough to accommodate the entire national number and the international
'            * country code. 

			Dim digitBlockLimit As Integer = PhoneNumberUtil.MAX_LENGTH_FOR_NSN + PhoneNumberUtil.MAX_LENGTH_COUNTRY_CODE
			' Limit on the number of blocks separated by punctuation. Uses digitBlockLimit since some
'            * formats use spaces to separate each digit. 

			Dim blockLimit As [String] = Limit(0, digitBlockLimit)

			' A punctuation sequence allowing white space. 

			Dim punctuation As [String] = "[" + PhoneNumberUtil.VALID_PUNCTUATION + "]" + punctuationLimit
			' A digits block without punctuation. 

			Dim digitSequence As [String] = "\p{Nd}" + Limit(1, digitBlockLimit)
			Dim leadClassChars As [String] = openingParens + PhoneNumberUtil.PLUS_CHARS
			Dim leadClass As [String] = "[" + leadClassChars + "]"
			LEAD_CLASS = New PhoneRegex(leadClass, RegexOptions.None)
			GROUP_SEPARATOR = New PhoneRegex("\p{Z}" + "[^" + leadClassChars + "\p{Nd}]*")

			' Phone number pattern allowing optional punctuation. 

			PATTERN = New Regex("(?:" + leadClass + punctuation + ")" + leadLimit + digitSequence + "(?:" + punctuation + digitSequence + ")" + blockLimit + "(?:" + PhoneNumberUtil.EXTN_PATTERNS_FOR_MATCHING + ")?", PhoneNumberUtil.REGEX_FLAGS)
		End Sub

		'* Returns a regular expression quantifier with an upper and lower limit. 

		Private Shared Function Limit(lower As Integer, upper As Integer) As [String]
			If (lower < 0) OrElse (upper <= 0) OrElse (upper < lower) Then
				Throw New ArgumentOutOfRangeException()
			End If
			Return "{" + lower + "," + upper + "}"
		End Function

		'* The phone number utility. 

		Private ReadOnly phoneUtil As PhoneNumberUtil
		'* The text searched for phone numbers. 

		Private ReadOnly text As [String]
		'*
'        * The region (country) to assume for phone numbers without an international prefix, possibly
'        * null.
'        

		Private ReadOnly preferredRegion As [String]
		'* The degree of validation requested. 

		Private ReadOnly leniency As PhoneNumberUtil.Leniency
		'* The maximum number of retries after matching an invalid number. 

		Private maxTries As Long

		'* The last successful match, null unless in {@link State#READY}. 

		Private lastMatch As PhoneNumberMatch = Nothing
		'* The next index to start searching at. Undefined in {@link State#DONE}. 

		Private searchIndex As Integer = 0

		'*
'        * Creates a new instance. See the factory methods in {@link PhoneNumberUtil} on how to obtain a
'        * new instance.
'        *
'        * @param util      the phone number util to use
'        * @param text      the character sequence that we will search, null for no text
'        * @param country   the country to assume for phone numbers not written in international format
'        *                  (with a leading plus, or with the international dialing prefix of the
'        *                  specified region). May be null or "ZZ" if only numbers with a
'        *                  leading plus should be considered.
'        * @param leniency  the leniency to use when evaluating candidate phone numbers
'        * @param maxTries  the maximum number of invalid numbers to try before giving up on the text.
'        *                  This is to cover degenerate cases where the text has a lot of false positives
'        *                  in it. Must be {@code >= 0}.
'        

		Public Sub New(util As PhoneNumberUtil, text As [String], country As [String], leniency As PhoneNumberUtil.Leniency, maxTries As Long)
			If util Is Nothing Then
				Throw New ArgumentNullException()
			End If

			If maxTries < 0 Then
				Throw New ArgumentOutOfRangeException()
			End If

			Me.phoneUtil = util
			Me.text = If((text IsNot Nothing), text, "")
			Me.preferredRegion = country
			Me.leniency = leniency
			Me.maxTries = maxTries
		End Sub

		'*
'        * Attempts to find the next subsequence in the searched sequence on or after {@code searchIndex}
'        * that represents a phone number. Returns the next match, null if none was found.
'        *
'        * @param index  the search index to start searching at
'        * @return  the phone number match found, null if none can be found
'        

		Private Function Find(index As Integer) As PhoneNumberMatch
			Dim matched As Match = Nothing
			While maxTries > 0 AndAlso (InlineAssignHelper(matched, PATTERN.Match(text, index))).Success
				Dim start As Integer = matched.Index
				Dim candidate As [String] = text.Substring(start, matched.Length)

				' Check for extra numbers at the end.
				' TODO: This is the place to start when trying to support extraction of multiple phone number
				' from split notations (+41 79 123 45 67 / 68).
				candidate = TrimAfterFirstMatch(PhoneNumberUtil.SECOND_NUMBER_START_PATTERN, candidate)

				Dim match As PhoneNumberMatch = ExtractMatch(candidate, start)
				If match IsNot Nothing Then
					Return match
				End If

				index = start + candidate.Length
				maxTries -= 1
			End While

			Return Nothing
		End Function

		'*
'        * Trims away any characters after the first match of {@code pattern} in {@code candidate},
'        * returning the trimmed version.
'        

		Private Shared Function TrimAfterFirstMatch(pattern As Regex, candidate As [String]) As [String]
			Dim trailingCharsMatcher = pattern.Match(candidate)
			If trailingCharsMatcher.Success Then
				candidate = candidate.Substring(0, trailingCharsMatcher.Index)
			End If
			Return candidate
		End Function

		'*
'        * Helper method to determine if a character is a Latin-script letter or not. For our purposes,
'        * combining marks should also return true since we assume they have been added to a preceding
'        * Latin character.
'        

		Public Shared Function IsLatinLetter(letter As Char) As Boolean
			' Combining marks are a subset of non-spacing-mark.
			If Not Char.IsLetter(letter) AndAlso CharUnicodeInfo.GetUnicodeCategory(letter) <> UnicodeCategory.NonSpacingMark Then
				Return False
			End If
			' BASIC_LATIN
			' LATIN_1_SUPPLEMENT
			' LATIN_EXTENDED_A
			' LATIN_EXTENDED_ADDITIONAL
			' LATIN_EXTENDED_B
				' COMBINING_DIACRITICAL_MARKS
			Return letter >= &H0 AndAlso letter <= &H7f OrElse letter >= &H80 AndAlso letter <= &Hff OrElse letter >= &H100 AndAlso letter <= &H17f OrElse letter >= &H1e00 AndAlso letter <= &H1eff OrElse letter >= &H180 AndAlso letter <= &H24f OrElse letter >= &H300 AndAlso letter <= &H36f
		End Function

		Private Shared Function IsInvalidPunctuationSymbol(character As Char) As Boolean
			Return character = "%"C OrElse CharUnicodeInfo.GetUnicodeCategory(character) = UnicodeCategory.CurrencySymbol
		End Function

		Public Shared Function TrimAfterUnwantedChars(s As [String]) As [String]
			Dim found As Integer = -1
			Dim c As Char
			Dim uc As UnicodeCategory
			Dim i As Integer = 0
			While i <> s.Length
				c = s(i)
				uc = CharUnicodeInfo.GetUnicodeCategory(c)
				If c <> "#"C AndAlso (uc <> UnicodeCategory.UppercaseLetter AndAlso uc <> UnicodeCategory.LowercaseLetter AndAlso uc <> UnicodeCategory.TitlecaseLetter AndAlso uc <> UnicodeCategory.ModifierLetter AndAlso uc <> UnicodeCategory.OtherLetter AndAlso uc <> UnicodeCategory.DecimalDigitNumber AndAlso uc <> UnicodeCategory.LetterNumber AndAlso uc <> UnicodeCategory.OtherNumber) Then
					If found < 0 Then
						found = i
					End If
				Else
					found = -1
				End If
				i += 1
			End While
			If found >= 0 Then
				Return s.Substring(0, found)
			End If
			Return s
		End Function

		'*
'        * Attempts to extract a match from a {@code candidate} character sequence.
'        *
'        * @param candidate  the candidate text that might contain a phone number
'        * @param offset  the offset of {@code candidate} within {@link #text}
'        * @return  the match found, null if none can be found
'        

		Private Function ExtractMatch(candidate As [String], offset As Integer) As PhoneNumberMatch
			' Skip a match that is more likely a publication page reference or a date.
			If PUB_PAGES.Match(candidate).Success OrElse SLASH_SEPARATED_DATES.Match(candidate).Success Then
				Return Nothing
			End If
			' Skip potential time-stamps.
			If TIME_STAMPS.Match(candidate).Success Then
				Dim followingText As [String] = text.ToString().Substring(offset + candidate.Length)
				If TIME_STAMPS_SUFFIX.MatchBeginning(followingText).Success Then
					Return Nothing
				End If
			End If

			' Try to come up with a valid match given the entire candidate.
			Dim rawString As [String] = candidate
			Dim match As PhoneNumberMatch = ParseAndVerify(rawString, offset)
			If match IsNot Nothing Then
				Return match
			End If

			' If that failed, try to find an "inner match" - there might be a phone number within this
			' candidate.
			Return ExtractInnerMatch(rawString, offset)
		End Function

		'*
'        * Attempts to extract a match from {@code candidate} if the whole candidate does not qualify as a
'        * match.
'        *
'        * @param candidate  the candidate text that might contain a phone number
'        * @param offset  the current offset of {@code candidate} within {@link #text}
'        * @return  the match found, null if none can be found
'        

		Private Function ExtractInnerMatch(candidate As [String], offset As Integer) As PhoneNumberMatch
			' Try removing either the first or last "group" in the number and see if this gives a result.
			' We consider white space to be a possible indications of the start or end of the phone number.
			Dim groupMatcher = GROUP_SEPARATOR.Match(candidate)
			If groupMatcher.Success Then
				' Try the first group by itself.
				Dim firstGroupOnly As [String] = candidate.Substring(0, groupMatcher.Index)
				firstGroupOnly = TrimAfterUnwantedChars(firstGroupOnly)
				Dim match As PhoneNumberMatch = ParseAndVerify(firstGroupOnly, offset)
				If match IsNot Nothing Then
					Return match
				End If
				maxTries -= 1

				Dim withoutFirstGroupStart As Integer = groupMatcher.Index + groupMatcher.Length
				' Try the rest of the candidate without the first group.
				Dim withoutFirstGroup As [String] = candidate.Substring(withoutFirstGroupStart)
				withoutFirstGroup = TrimAfterUnwantedChars(withoutFirstGroup)
				match = ParseAndVerify(withoutFirstGroup, offset + withoutFirstGroupStart)
				If match IsNot Nothing Then
					Return match
				End If
				maxTries -= 1

				If maxTries > 0 Then
					Dim lastGroupStart As Integer = withoutFirstGroupStart
					While (InlineAssignHelper(groupMatcher, groupMatcher.NextMatch())).Success
						' Find the last group.
						lastGroupStart = groupMatcher.Index
					End While
					Dim withoutLastGroup As [String] = candidate.Substring(0, lastGroupStart)
					withoutLastGroup = TrimAfterUnwantedChars(withoutLastGroup)
					If withoutLastGroup.Equals(firstGroupOnly) Then
						' If there are only two groups, then the group "without the last group" is the same as
						' the first group. In these cases, we don't want to re-check the number group, so we exit
						' already.
						Return Nothing
					End If
					match = ParseAndVerify(withoutLastGroup, offset)
					If match IsNot Nothing Then
						Return match
					End If
					maxTries -= 1
				End If
			End If
			Return Nothing
		End Function

		'*
'        * Parses a phone number from the {@code candidate} using {@link PhoneNumberUtil#parse} and
'        * verifies it matches the requested {@link #leniency}. If parsing and verification succeed, a
'        * corresponding {@link PhoneNumberMatch} is returned, otherwise this method returns null.
'        *
'        * @param candidate  the candidate match
'        * @param offset  the offset of {@code candidate} within {@link #text}
'        * @return  the parsed and validated phone number match, or null
'        

		Private Function ParseAndVerify(candidate As [String], offset As Integer) As PhoneNumberMatch
			Try
				' Check the candidate doesn't contain any formatting which would indicate that it really
				' isn't a phone number.
				If Not MATCHING_BRACKETS.MatchAll(candidate).Success Then
					Return Nothing
				End If

				' If leniency is set to VALID or stricter, we also want to skip numbers that are surrounded
				' by Latin alphabetic characters, to skip cases like abc8005001234 or 8005001234def.
				If leniency >= PhoneNumberUtil.Leniency.VALID Then
					' If the candidate is not at the start of the text, and does not start with phone-number
					' punctuation, check the previous character.
					If offset > 0 AndAlso Not LEAD_CLASS.MatchBeginning(candidate).Success Then
						Dim previousChar As Char = text(offset - 1)
						' We return null if it is a latin letter or an invalid punctuation symbol.
						If IsInvalidPunctuationSymbol(previousChar) OrElse IsLatinLetter(previousChar) Then
							Return Nothing
						End If
					End If
					Dim lastCharIndex As Integer = offset + candidate.Length
					If lastCharIndex < text.Length Then
						Dim nextChar As Char = text(lastCharIndex)
						If IsInvalidPunctuationSymbol(nextChar) OrElse IsLatinLetter(nextChar) Then
							Return Nothing
						End If
					End If
				End If

				Dim number As PhoneNumber = phoneUtil.ParseAndKeepRawInput(candidate, preferredRegion)
				If phoneUtil.Verify(leniency, number, candidate, phoneUtil) Then
					' We used parseAndKeepRawInput to create this number, but for now we don't return the extra
					' values parsed. TODO: stop clearing all values here and switch all users over
					' to using rawInput() rather than the rawString() of PhoneNumberMatch.
					Dim bnumber = number.ToBuilder()
					bnumber.ClearCountryCodeSource()
					bnumber.ClearRawInput()
					bnumber.ClearPreferredDomesticCarrierCode()
					Return New PhoneNumberMatch(offset, candidate, bnumber.Build())
				End If
					' ignore and continue
			Catch generatedExceptionName As NumberParseException
			End Try
			Return Nothing
		End Function

		'*
'        * Returns true if the groups of digits found in our candidate phone number match our
'        * expectations.
'        *
'        * @param number  the original number we found when parsing
'        * @param normalizedCandidate  the candidate number, normalized to only contain ASCII digits,
'        *     but with non-digits (spaces etc) retained
'        * @param expectedNumberGroups  the groups of digits that we would expect to see if we
'        *     formatted this number
'        

		Public Delegate Function CheckGroups(util As PhoneNumberUtil, number As PhoneNumber, normalizedCandidate As StringBuilder, expectedNumberGroups As [String]()) As Boolean

		Public Shared Function AllNumberGroupsRemainGrouped(util As PhoneNumberUtil, number As PhoneNumber, normalizedCandidate As StringBuilder, formattedNumberGroups As [String]()) As Boolean
			Dim fromIndex As Integer = 0
			' Check each group of consecutive digits are not broken into separate groupings in the
			' {@code normalizedCandidate} string.
			For i As Integer = 0 To formattedNumberGroups.Length - 1
				' Fails if the substring of {@code normalizedCandidate} starting from {@code fromIndex}
				' doesn't contain the consecutive digits in formattedNumberGroups[i].
				fromIndex = normalizedCandidate.ToString().IndexOf(formattedNumberGroups(i), fromIndex)
				If fromIndex < 0 Then
					Return False
				End If
				' Moves {@code fromIndex} forward.
				fromIndex += formattedNumberGroups(i).Length
				If i = 0 AndAlso fromIndex < normalizedCandidate.Length Then
					' We are at the position right after the NDC.
					If Char.IsDigit(normalizedCandidate(fromIndex)) Then
						' This means there is no formatting symbol after the NDC. In this case, we only
						' accept the number if there is no formatting symbol at all in the number, except
						' for extensions.
						Dim nationalSignificantNumber As [String] = util.GetNationalSignificantNumber(number)
						Return normalizedCandidate.ToString().Substring(fromIndex - formattedNumberGroups(i).Length).StartsWith(nationalSignificantNumber)
					End If
				End If
			Next
			' The check here makes sure that we haven't mistakenly already used the extension to
			' match the last group of the subscriber number. Note the extension cannot have
			' formatting in-between digits.
			Return normalizedCandidate.ToString().Substring(fromIndex).Contains(number.Extension)
		End Function

		Public Shared Function AllNumberGroupsAreExactlyPresent(util As PhoneNumberUtil, number As PhoneNumber, normalizedCandidate As StringBuilder, formattedNumberGroups As [String]()) As Boolean
			Dim candidateGroups As [String]() = PhoneNumberUtil.NON_DIGITS_PATTERN.Split(normalizedCandidate.ToString())
			' Set this to the last group, skipping it if the number has an extension.
			Dim candidateNumberGroupIndex As Integer = If(number.HasExtension, candidateGroups.Length - 2, candidateGroups.Length - 1)
			' First we check if the national significant number is formatted as a block.
			' We use contains and not equals, since the national significant number may be present with
			' a prefix such as a national number prefix, or the country code itself.
			If candidateGroups.Length = 1 OrElse candidateGroups(candidateNumberGroupIndex).Contains(util.GetNationalSignificantNumber(number)) Then
				Return True
			End If
			' Starting from the end, go through in reverse, excluding the first group, and check the
			' candidate and number groups are the same.
			Dim formattedNumberGroupIndex As Integer = (formattedNumberGroups.Length - 1)
			While formattedNumberGroupIndex > 0 AndAlso candidateNumberGroupIndex >= 0
				If Not candidateGroups(candidateNumberGroupIndex).Equals(formattedNumberGroups(formattedNumberGroupIndex)) Then
					Return False
				End If
				formattedNumberGroupIndex -= 1
				candidateNumberGroupIndex -= 1
			End While
			' Now check the first group. There may be a national prefix at the start, so we only check
			' that the candidate group ends with the formatted number group.
			Return (candidateNumberGroupIndex >= 0 AndAlso candidateGroups(candidateNumberGroupIndex).EndsWith(formattedNumberGroups(0)))
		End Function

		'*
'        * Helper method to get the national-number part of a number, formatted without any national
'        * prefix, and return it as a set of digit blocks that would be formatted together.
'        

		Private Shared Function GetNationalNumberGroups(util As PhoneNumberUtil, number As PhoneNumber, formattingPattern As NumberFormat) As [String]()
			If formattingPattern Is Nothing Then
				' This will be in the format +CC-DG;ext=EXT where DG represents groups of digits.
				Dim rfc3966Format As [String] = util.Format(number, PhoneNumberFormat.RFC3966)
				' We remove the extension part from the formatted string before splitting it into different
				' groups.
				Dim endIndex As Integer = rfc3966Format.IndexOf(";"C)
				If endIndex < 0 Then
					endIndex = rfc3966Format.Length
				End If
				' The country-code will have a '-' following it.
				Dim startIndex As Integer = rfc3966Format.IndexOf("-"C) + 1
				Return rfc3966Format.Substring(startIndex, endIndex - startIndex).Split(New () {"-"C})
			Else
				' We format the NSN only, and split that according to the separator.
				Dim nationalSignificantNumber As [String] = util.GetNationalSignificantNumber(number)
				Return util.FormatNsnUsingPattern(nationalSignificantNumber, formattingPattern, PhoneNumberFormat.RFC3966).Split(New () {"-"C})
			End If
		End Function

		Public Shared Function CheckNumberGroupingIsValid(number As PhoneNumber, candidate As [String], util As PhoneNumberUtil, checker As CheckGroups) As Boolean
			' TODO: Evaluate how this works for other locales (testing has been limited to NANPA regions)
			' and optimise if necessary.
				' keep non-digits 
			Dim normalizedCandidate As StringBuilder = PhoneNumberUtil.NormalizeDigits(candidate, True)
			Dim formattedNumberGroups As [String]() = PhoneNumberMatcher.GetNationalNumberGroups(util, number, Nothing)
			If checker(util, number, normalizedCandidate, formattedNumberGroups) Then
				Return True
			End If
			' If this didn't pass, see if there are any alternate formats, and try them instead.
			Dim alternateFormats = MetadataManager.GetAlternateFormatsForCountry(number.CountryCode)
			If alternateFormats IsNot Nothing Then
                For Each alternateFormat In alternateFormats.NumberFormatList
                    formattedNumberGroups = GetNationalNumberGroups(util, number, alternateFormat)
                    If checker(util, number, normalizedCandidate, formattedNumberGroups) Then
                        Return True
                    End If
                Next
			End If
			Return False
		End Function

		Public Shared Function ContainsMoreThanOneSlash(candidate As [String]) As Boolean
			Dim firstSlashIndex As Integer = candidate.IndexOf("/"C)
			Return (firstSlashIndex > 0 AndAlso candidate.Substring(firstSlashIndex + 1).Contains("/"))
		End Function

		Public Shared Function ContainsOnlyValidXChars(number As PhoneNumber, candidate As [String], util As PhoneNumberUtil) As Boolean
			' The characters 'x' and 'X' can be (1) a carrier code, in which case they always precede the
			' national significant number or (2) an extension sign, in which case they always precede the
			' extension number. We assume a carrier code is more than 1 digit, so the first case has to
			' have more than 1 consecutive 'x' or 'X', whereas the second case can only have exactly 1 'x'
			' or 'X'. We ignore the character if it appears as the last character of the string.
			For index As Integer = 0 To candidate.Length - 2
				Dim charAtIndex As Char = candidate(index)
				If charAtIndex = "x"C OrElse charAtIndex = "X"C Then
					Dim charAtNextIndex As Char = candidate(index + 1)
					If charAtNextIndex = "x"C OrElse charAtNextIndex = "X"C Then
						' This is the carrier code case, in which the 'X's always precede the national
						' significant number.
						index += 1
						If util.IsNumberMatch(number, candidate.Substring(index)) <> PhoneNumberUtil.MatchType.NSN_MATCH Then
							Return False
							' This is the extension sign case, in which the 'x' or 'X' should always precede the
							' extension number.
						End If
					ElseIf Not PhoneNumberUtil.NormalizeDigitsOnly(candidate.Substring(index)).Equals(number.Extension) Then
						Return False
					End If
				End If
			Next
			Return True
		End Function

		Public Shared Function IsNationalPrefixPresentIfRequired(number As PhoneNumber, util As PhoneNumberUtil) As Boolean
			' First, check how we deduced the country code. If it was written in international format, then
			' the national prefix is not required.
			If number.CountryCodeSource <> PhoneNumber.Types.CountryCodeSource.FROM_DEFAULT_COUNTRY Then
				Return True
			End If
			Dim phoneNumberRegion As [String] = util.GetRegionCodeForCountryCode(number.CountryCode)
			Dim metadata As PhoneMetadata = util.GetMetadataForRegion(phoneNumberRegion)
			If metadata Is Nothing Then
				Return True
			End If
			' Check if a national prefix should be present when formatting this number.
			Dim nationalNumber As [String] = util.GetNationalSignificantNumber(number)
			Dim formatRule As NumberFormat = util.ChooseFormattingPatternForNumber(metadata.NumberFormatList, nationalNumber)
			' To do this, we check that a national prefix formatting rule was present and that it wasn't
			' just the first-group symbol ($1) with punctuation.
			If (formatRule IsNot Nothing) AndAlso formatRule.NationalPrefixFormattingRule.Length > 0 Then
				If formatRule.NationalPrefixOptionalWhenFormatting Then
					' The national-prefix is optional in these cases, so we don't need to check if it was
					' present.
					Return True
				End If
				' Remove the first-group symbol.
				Dim candidateNationalPrefixRule As [String] = formatRule.NationalPrefixFormattingRule
				' We assume that the first-group symbol will never be _before_ the national prefix.
				candidateNationalPrefixRule = candidateNationalPrefixRule.Substring(0, candidateNationalPrefixRule.IndexOf("${1}"))
				candidateNationalPrefixRule = PhoneNumberUtil.NormalizeDigitsOnly(candidateNationalPrefixRule)
				If candidateNationalPrefixRule.Length = 0 Then
					' National Prefix not needed for this number.
					Return True
				End If
				' Normalize the remainder.
				Dim rawInputCopy As [String] = PhoneNumberUtil.NormalizeDigitsOnly(number.RawInput)
				Dim rawInput As New StringBuilder(rawInputCopy)
				' Check if we found a national prefix and/or carrier code at the start of the raw input, and
				' return the result.
				Return util.MaybeStripNationalPrefixAndCarrierCode(rawInput, metadata, Nothing)
			End If
			Return True
		End Function

		Public ReadOnly Property Current() As PhoneNumberMatch
			Get
				Return lastMatch
			End Get
		End Property

		Private ReadOnly Property IEnumerator_Current() As [Object] Implements IEnumerator.Current
			Get
				Return lastMatch
			End Get
		End Property

		Public Function MoveNext() As Boolean
			lastMatch = Find(searchIndex)
			If lastMatch IsNot Nothing Then
				searchIndex = lastMatch.Start + lastMatch.Length
			End If
			Return lastMatch IsNot Nothing
		End Function

		Public Sub Reset()
			Throw New NotSupportedException()
		End Sub

		Public Sub Dispose()
		End Sub
		Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
			target = value
			Return value
		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
