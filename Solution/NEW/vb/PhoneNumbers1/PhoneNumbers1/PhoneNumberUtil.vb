
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
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports libphonenumber_csharp_portable
Imports CountryCodeSource = PhoneNumbers.PhoneNumber.Types.CountryCodeSource

Namespace PhoneNumbers
	'*
'    * INTERNATIONAL and NATIONAL formats are consistent with the definition in ITU-T Recommendation
'    * E123. For example, the number of the Google Switzerland office will be written as
'    * "+41 44 668 1800" in INTERNATIONAL format, and as "044 668 1800" in NATIONAL format.
'    * E164 format is as per INTERNATIONAL format but with no formatting applied, e.g.
'    * "+41446681800". RFC3966 is as per INTERNATIONAL format, but with all spaces and other
'    * separating symbols replaced with a hyphen, and with any phone number extension appended with
'    * ";ext=". It also will have a prefix of "tel:" added, e.g. "tel:+41-44-668-1800".
'    *
'    * Note: If you are considering storing the number in a neutral format, you are highly advised to
'    * use the PhoneNumber class.
'    

	Public Enum PhoneNumberFormat
		E164
		INTERNATIONAL
		NATIONAL
		RFC3966
	End Enum

	' Type of phone numbers.
	Public Enum PhoneNumberType
		FIXED_LINE
		MOBILE
		' In some regions (e.g. the USA), it is impossible to distinguish between fixed-line and
		' mobile numbers by looking at the phone number itself.
		FIXED_LINE_OR_MOBILE
		' Freephone lines
		TOLL_FREE
		PREMIUM_RATE
		' The cost of this call is shared between the caller and the recipient, and is hence typically
		' less than PREMIUM_RATE calls. See // http://en.wikipedia.org/wiki/Shared_Cost_Service for
		' more information.
		SHARED_COST
		' Voice over IP numbers. This includes TSoIP (Telephony Service over IP).
		VOIP
		' A personal number is associated with a particular person, and may be routed to either a
		' MOBILE or FIXED_LINE number. Some more information can be found here:
		' http://en.wikipedia.org/wiki/Personal_Numbers
		PERSONAL_NUMBER
		PAGER
		' Used for "Universal Access Numbers" or "Company Numbers". They may be further routed to
		' specific offices, but allow one number to be used for a company.
		UAN
		' Used for "Voice Mail Access Numbers".
		VOICEMAIL
		' A phone number is of type UNKNOWN when it does not fit any of the known patterns for a
		' specific region.
		UNKNOWN
	End Enum

	'*
'    * Utility for international phone numbers. Functionality includes formatting, parsing and
'    * validation.
'    *
'    * <p>If you use this library, and want to be notified about important changes, please sign up to
'    * our <a href="http://groups.google.com/group/libphonenumber-discuss/about">mailing list</a>.
'    *
'    * NOTE: A lot of methods in this class require Region Code strings. These must be provided using
'    * ISO 3166-1 two-letter country-code format. These should be in upper-case. The list of the codes
'    * can be found here:
'    * http://www.iso.org/iso/country_codes/iso_3166_code_lists/country_names_and_code_elements.htm
'    *
'    * @author Shaopeng Jia
'    * @author Lara Rennie
'    

	Public Class PhoneNumberUtil
		' Flags to use when compiling regular expressions for phone numbers.
		Friend Const REGEX_FLAGS As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant
		' The minimum and maximum length of the national significant number.
		Friend Const MIN_LENGTH_FOR_NSN As Integer = 2
		' The ITU says the maximum length should be 15, but we have found longer numbers in Germany.
		Friend Const MAX_LENGTH_FOR_NSN As Integer = 16
		' The maximum length of the country calling code.
		Friend Const MAX_LENGTH_COUNTRY_CODE As Integer = 3
		' We don't allow input strings for parsing to be longer than 250 chars. This prevents malicious
		' input from overflowing the regular-expression engine.
		Private Const MAX_INPUT_STRING_LENGTH As Integer = 250
		Friend Const META_DATA_FILE_PREFIX As [String] = "PhoneNumberMetaData.xml"
		Friend Const UNKNOWN_REGION As [String] = "ZZ"

		Private currentFilePrefix_ As [String] = META_DATA_FILE_PREFIX

		' A mapping from a country calling code to the region codes which denote the region represented
		' by that country calling code. In the case of multiple regions sharing a calling code, such as
		' the NANPA regions, the one indicated with "isMainCountryForCode" in the metadata should be
		' first.
		Private countryCallingCodeToRegionCodeMap_ As Dictionary(Of Integer, List(Of [String])) = Nothing

		' The set of regions the library supports.
		' There are roughly 240 of them and we set the initial capacity of the HashSet to 320 to offer a
		' load factor of roughly 0.75.
		Private supportedRegions_ As New HashSet(Of [String])()

		' The set of regions that share country calling code 1.
		Private nanpaRegions_ As New HashSet(Of [String])()
		Private Const NANPA_COUNTRY_CODE As Integer = 1

		' The prefix that needs to be inserted in front of a Colombian landline number when dialed from
		' a mobile phone in Colombia.
		Private Const COLOMBIA_MOBILE_TO_FIXED_LINE_PREFIX As [String] = "3"

		' The PLUS_SIGN signifies the international prefix.
		Friend Const PLUS_SIGN As Char = "+"C

		Private Const STAR_SIGN As Char = "*"C

		Const RFC3966_EXTN_PREFIX As [String] = ";ext="
		Private Const RFC3966_PREFIX As [String] = "tel:"
		Private Const RFC3966_PHONE_CONTEXT As [String] = ";phone-context="
		Private Const RFC3966_ISDN_SUBADDRESS As [String] = ";isub="

		' A map that contains characters that are essential when dialling. That means any of the
		' characters in this map must not be removed from a number when dialing, otherwise the call will
		' not reach the intended destination.
		Private Shared ReadOnly DIALLABLE_CHAR_MAPPINGS As Dictionary(Of Char, Char)

		' Only upper-case variants of alpha characters are stored.
		Private Shared ReadOnly ALPHA_MAPPINGS As Dictionary(Of Char, Char)

		' For performance reasons, amalgamate both into one map.
		Private Shared ReadOnly ALPHA_PHONE_MAPPINGS As Dictionary(Of Char, Char)

		' Separate map of all symbols that we wish to retain when formatting alpha numbers. This
		' includes digits, ASCII letters and number grouping symbols such as "-" and " ".
		Private Shared ReadOnly ALL_PLUS_NUMBER_GROUPING_SYMBOLS As Dictionary(Of Char, Char)

		Private Shared thisLock As [Object]

		' Pattern that makes it easy to distinguish whether a region has a unique international dialing
		' prefix or not. If a region has a unique international prefix (e.g. 011 in USA), it will be
		' represented as a string that contains a sequence of ASCII digits. If there are multiple
		' available international prefixes in a region, they will be represented as a regex string that
		' always contains character(s) other than ASCII digits.
		' Note this regex also includes tilde, which signals waiting for the tone.
		Private Shared ReadOnly UNIQUE_INTERNATIONAL_PREFIX As New PhoneRegex("[\d]+(?:[~?~~][\d]+)?", RegexOptions.None)

		' Regular expression of acceptable punctuation found in phone numbers. This excludes punctuation
		' found as a leading character only.
		' This consists of dash characters, white space characters, full stops, slashes,
		' square brackets, parentheses and tildes. It also includes the letter 'x' as that is found as a
		' placeholder for carrier information in some phone numbers. Full-width variants are also
		' present.
		Friend Const VALID_PUNCTUATION As [String] = "-x--?-?--/ " + " ­?? ()()[].\[\]/~?~~"

		Private Const DIGITS As [String] = "\p{Nd}"

		' We accept alpha characters in phone numbers, ASCII only, upper and lower case.
		Private Shared ReadOnly VALID_ALPHA As [String]

		Friend Const PLUS_CHARS As [String] = "++"
		Friend Shared ReadOnly PLUS_CHARS_PATTERN As New PhoneRegex("[" + PLUS_CHARS + "]+", RegexOptions.None)
		Private Shared ReadOnly SEPARATOR_PATTERN As New PhoneRegex("[" + VALID_PUNCTUATION + "]+", RegexOptions.None)
		Private Shared ReadOnly CAPTURING_DIGIT_PATTERN As Regex

		' Regular expression of acceptable characters that may start a phone number for the purposes of
		' parsing. This allows us to strip away meaningless prefixes to phone numbers that may be
		' mistakenly given to us. This consists of digits, the plus symbol and arabic-indic digits. This
		' does not contain alpha characters, although they may be used later in the number. It also does
		' not include other punctuation, as this will be stripped later during parsing and is of no
		' information value when parsing a number.
		Private Shared ReadOnly VALID_START_CHAR As [String]
		Public Shared ReadOnly VALID_START_CHAR_PATTERN As PhoneRegex

		' Regular expression of characters typically used to start a second phone number for the purposes
		' of parsing. This allows us to strip off parts of the number that are actually the start of
		' another number, such as for: (530) 583-6985 x302/x2303 -> the second extension here makes this
		' actually two phone numbers, (530) 583-6985 x302 and (530) 583-6985 x2303. We remove the second
		' extension so that the first number is parsed correctly.
		Private Shared ReadOnly SECOND_NUMBER_START As [String] = "[\\/] *x"
		Friend Shared ReadOnly SECOND_NUMBER_START_PATTERN As New Regex(SECOND_NUMBER_START, RegexOptions.None)

		' We use this pattern to check if the phone number has at least three letters in it - if so, then
		' we treat it as a number where some phone-number digits are represented by letters.
		Private Shared ReadOnly VALID_ALPHA_PHONE_PATTERN As New PhoneRegex("(?:.*?[A-Za-z]){3}.*", RegexOptions.None)

		' Regular expression of viable phone numbers. This is location independent. Checks we have at
		' least three leading digits, and only valid punctuation, alpha characters and
		' digits in the phone number. Does not include extension data.
		' The symbol 'x' is allowed here as valid punctuation since it is often used as a placeholder for
		' carrier codes, for example in Brazilian phone numbers. We also allow multiple "+" characters at
		' the start.
		' [digits]{minLengthNsn}|
		' plus_sign*(([punctuation]|[star])*[digits]){3,}([punctuation]|[star]|[digits]|[alpha])*
		'
		' The first reg-ex is to allow short numbers (two digits long) to be parsed if they are entered
		' as "15" etc, but only if there is no punctuation in them. The second expression restricts the
		' number of digits to three or more, but then allows them to be in international form, and to
		' have alpha-characters and punctuation.
		'
		' Note VALID_PUNCTUATION starts with a -, so must be the first in the range.
		Private Shared ReadOnly VALID_PHONE_NUMBER As [String]

		' Default extension prefix to use when formatting. This will be put in front of any extension
		' component of the number, after the main national number is formatted. For example, if you wish
		' the default extension formatting to be " extn: 3456", then you should specify " extn: " here
		' as the default extension prefix. This can be overridden by region-specific preferences.
		Private Shared ReadOnly DEFAULT_EXTN_PREFIX As [String] = " ext. "

		' Pattern to capture digits used in an extension. Places a maximum length of "7" for an
		' extension.
		Private Shared ReadOnly CAPTURING_EXTN_DIGITS As [String] = "(" + DIGITS + "{1,7})"
		' Regexp of all possible ways to write extensions, for use when parsing. This will be run as a
		' case-insensitive regexp match. Wide character versions are also provided after each ASCII
		' version.
		Friend Shared ReadOnly EXTN_PATTERNS_FOR_PARSING As [String]
		Friend Shared ReadOnly EXTN_PATTERNS_FOR_MATCHING As [String]

		'*
'        * Helper initialiser method to create the regular-expression pattern to match extensions,
'        * allowing the one-char extension symbols provided by {@code singleExtnSymbols}.
'        

		Private Shared Function CreateExtnPattern(singleExtnSymbols As [String]) As [String]
			' There are three regular expressions here. The first covers RFC 3966 format, where the
			' extension is added using ";ext=". The second more generic one starts with optional white
			' space and ends with an optional full stop (.), followed by zero or more spaces/tabs and then
			' the numbers themselves. The other one covers the special case of American numbers where the
			' extension is written with a hash at the end, such as "- 503#".
			' Note that the only capturing groups should be around the digits that you want to capture as
			' part of the extension, or else parsing will fail!
			' Canonical-equivalence doesn't seem to be an option with Android java, so we allow two options
			' for representing the accented o - the character itself, and one in the unicode decomposed
			' form with the combining acute accent.
			Return (RFC3966_EXTN_PREFIX + CAPTURING_EXTN_DIGITS + "|" + "[  \t,]*" + "(?:e?xt(?:ensi(?:o´?|ó))?n?|e?xtn?|" + "[" + singleExtnSymbols + "]|int|anexo|int)" + "[:\..]?[  \t,-]*" + CAPTURING_EXTN_DIGITS + "#?|" + "[- ]+(" + DIGITS + "{1,5})#")
		End Function

		' Regexp of all known extension prefixes used by different regions followed by 1 or more valid
		' digits, for use when parsing.
		Private Shared ReadOnly EXTN_PATTERN As Regex

		' We append optionally the extension pattern to the end here, as a valid phone number may
		' have an extension prefix appended, followed by 1 or more digits.
		Private Shared ReadOnly VALID_PHONE_NUMBER_PATTERN As PhoneRegex

		Friend Shared ReadOnly NON_DIGITS_PATTERN As New Regex("\D+", RegexOptions.None)

		' The FIRST_GROUP_PATTERN was originally set to $1 but there are some countries for which the
		' first group is not used in the national pattern (e.g. Argentina) so the $1 group does not match
		' correctly.  Therefore, we use \d, so that the first group actually used in the pattern will be
		' matched.
		Private Shared ReadOnly FIRST_GROUP_PATTERN As New Regex("(\$\d)", RegexOptions.None)
		Private Shared ReadOnly NP_PATTERN As New Regex("\$NP", RegexOptions.None)
		Private Shared ReadOnly FG_PATTERN As New Regex("\$FG", RegexOptions.None)
		Private Shared ReadOnly CC_PATTERN As New Regex("\$CC", RegexOptions.None)

		Shared Sub New()
			thisLock = New [Object]()

			' Simple ASCII digits map used to populate ALPHA_PHONE_MAPPINGS and
			' ALL_PLUS_NUMBER_GROUPING_SYMBOLS.
			Dim asciiDigitMappings = New Dictionary(Of Char, Char)() From { _
				{"0"C, "0"C}, _
				{"1"C, "1"C}, _
				{"2"C, "2"C}, _
				{"3"C, "3"C}, _
				{"4"C, "4"C}, _
				{"5"C, "5"C}, _
				{"6"C, "6"C}, _
				{"7"C, "7"C}, _
				{"8"C, "8"C}, _
				{"9"C, "9"C} _
			}

			Dim alphaMap = New Dictionary(Of Char, Char)()
			alphaMap("A"C) = "2"C
			alphaMap("B"C) = "2"C
			alphaMap("C"C) = "2"C
			alphaMap("D"C) = "3"C
			alphaMap("E"C) = "3"C
			alphaMap("F"C) = "3"C
			alphaMap("G"C) = "4"C
			alphaMap("H"C) = "4"C
			alphaMap("I"C) = "4"C
			alphaMap("J"C) = "5"C
			alphaMap("K"C) = "5"C
			alphaMap("L"C) = "5"C
			alphaMap("M"C) = "6"C
			alphaMap("N"C) = "6"C
			alphaMap("O"C) = "6"C
			alphaMap("P"C) = "7"C
			alphaMap("Q"C) = "7"C
			alphaMap("R"C) = "7"C
			alphaMap("S"C) = "7"C
			alphaMap("T"C) = "8"C
			alphaMap("U"C) = "8"C
			alphaMap("V"C) = "8"C
			alphaMap("W"C) = "9"C
			alphaMap("X"C) = "9"C
			alphaMap("Y"C) = "9"C
			alphaMap("Z"C) = "9"C
			ALPHA_MAPPINGS = alphaMap

			Dim combinedMap = New Dictionary(Of Char, Char)(ALPHA_MAPPINGS)
            For Each k In asciiDigitMappings
                combinedMap(k.Key) = k.Value
            Next
			ALPHA_PHONE_MAPPINGS = combinedMap

			Dim diallableCharMap = New Dictionary(Of Char, Char)()
            For Each k In asciiDigitMappings
                diallableCharMap(k.Key) = k.Value
            Next
			diallableCharMap(PLUS_SIGN) = PLUS_SIGN
			diallableCharMap("*"C) = "*"C
			DIALLABLE_CHAR_MAPPINGS = diallableCharMap

			Dim allPlusNumberGroupings = New Dictionary(Of Char, Char)()
			' Put (lower letter -> upper letter) and (upper letter -> upper letter) mappings.
            For Each c In ALPHA_MAPPINGS.Keys
                allPlusNumberGroupings(Char.ToLowerInvariant(c)) = c
                allPlusNumberGroupings(c) = c
            Next

            For Each k In asciiDigitMappings
                allPlusNumberGroupings(k.Key) = k.Value
            Next
			' Put grouping symbols.
			allPlusNumberGroupings("-"C) = "-"C
			allPlusNumberGroupings("-"C) = "-"C
			allPlusNumberGroupings("-"C) = "-"C
			allPlusNumberGroupings("-"C) = "-"C
			allPlusNumberGroupings("?"C) = "-"C
			allPlusNumberGroupings("–"C) = "-"C
			allPlusNumberGroupings("—"C) = "-"C
			allPlusNumberGroupings("?"C) = "-"C
			allPlusNumberGroupings("-"C) = "-"C
			allPlusNumberGroupings("/"C) = "/"C
			allPlusNumberGroupings("/"C) = "/"C
			allPlusNumberGroupings(" "C) = " "C
			allPlusNumberGroupings(" "C) = " "C
			allPlusNumberGroupings("?"C) = " "C
			allPlusNumberGroupings("."C) = "."C
			allPlusNumberGroupings("."C) = "."C
			ALL_PLUS_NUMBER_GROUPING_SYMBOLS = allPlusNumberGroupings

			' We accept alpha characters in phone numbers, ASCII only, upper and lower case.
			VALID_ALPHA = [String].Join("", ALPHA_MAPPINGS.Keys.Where(Function(c) Not "[, \[\]]".Contains(c.ToString())).ToList().ConvertAll(Function(c) c.ToString()).ToArray()) + [String].Join("", ALPHA_MAPPINGS.Keys.Where(Function(c) Not "[, \[\]]".Contains(c.ToString())).ToList().ConvertAll(Function(c) c.ToString()).ToArray()).ToLower()

			CAPTURING_DIGIT_PATTERN = New Regex("(" + DIGITS + ")", RegexOptions.None)
			VALID_START_CHAR = "[" + PLUS_CHARS + DIGITS + "]"
			VALID_START_CHAR_PATTERN = New PhoneRegex(VALID_START_CHAR, RegexOptions.None)

			CAPTURING_EXTN_DIGITS = "(" + DIGITS + "{1,7})"
			VALID_PHONE_NUMBER = DIGITS + "{" + MIN_LENGTH_FOR_NSN + "}" + "|" + "[" + PLUS_CHARS + "]*(?:[" + VALID_PUNCTUATION + STAR_SIGN + "]*" + DIGITS + "){3,}[" + VALID_PUNCTUATION + STAR_SIGN + VALID_ALPHA + DIGITS + "]*"

			' One-character symbols that can be used to indicate an extension.
			Dim singleExtnSymbolsForMatching As [String] = "xx##~~"
			' For parsing, we are slightly more lenient in our interpretation than for matching. Here we
			' allow a "comma" as a possible extension indicator. When matching, this is hardly ever used to
			' indicate this.
			Dim singleExtnSymbolsForParsing As [String] = "," + singleExtnSymbolsForMatching

			EXTN_PATTERNS_FOR_PARSING = CreateExtnPattern(singleExtnSymbolsForParsing)
			EXTN_PATTERNS_FOR_MATCHING = CreateExtnPattern(singleExtnSymbolsForMatching)

			EXTN_PATTERN = New Regex("(?:" + EXTN_PATTERNS_FOR_PARSING + ")$", REGEX_FLAGS)

			VALID_PHONE_NUMBER_PATTERN = New PhoneRegex(VALID_PHONE_NUMBER + "(?:" + EXTN_PATTERNS_FOR_PARSING + ")?", REGEX_FLAGS)
		End Sub

		Private Shared instance_ As PhoneNumberUtil = Nothing

		' A mapping from a region code to the PhoneMetadata for that region.
		Private regionToMetadataMap As New Dictionary(Of [String], PhoneMetadata)()

		' A mapping from a country calling code for a non-geographical entity to the PhoneMetadata for
		' that country calling code. Examples of the country calling codes include 800 (International
		' Toll Free Service) and 808 (International Shared Cost Service).
		Private ReadOnly countryCodeToNonGeographicalMetadataMap As New Dictionary(Of Integer, PhoneMetadata)()

		' A cache for frequently used region-specific regular expressions.
		' As most people use phone numbers primarily from one to two countries, and there are roughly 60
		' regular expressions needed, the initial capacity of 100 offers a rough load factor of 0.75.
		Private regexCache As New RegexCache(100)

		Public Const REGION_CODE_FOR_NON_GEO_ENTITY As [String] = "001"

		' Types of phone number matches. See detailed description beside the isNumberMatch() method.
		Public Enum MatchType
			NOT_A_NUMBER
			NO_MATCH
			SHORT_NSN_MATCH
			NSN_MATCH
			EXACT_MATCH
		End Enum

		' Possible outcomes when testing if a PhoneNumber is possible.
		Public Enum ValidationResult
			IS_POSSIBLE
			INVALID_COUNTRY_CODE
			TOO_SHORT
			TOO_LONG
		End Enum

		'*
'        * Leniency when {@linkplain PhoneNumberUtil#findNumbers finding} potential phone numbers in text
'        * segments. The levels here are ordered in increasing strictness.
'        

		Public Enum Leniency
			'*
'            * Phone numbers accepted are {@linkplain PhoneNumberUtil#isPossibleNumber(PhoneNumber)
'            * possible}, but not necessarily {@linkplain PhoneNumberUtil#isValidNumber(PhoneNumber) valid}.
'            

			POSSIBLE
			'*
'            * Phone numbers accepted are {@linkplain PhoneNumberUtil#isPossibleNumber(PhoneNumber)
'            * possible} and {@linkplain PhoneNumberUtil#isValidNumber(PhoneNumber) valid}. Numbers written
'            * in national format must have their national-prefix present if it is usually written for a
'            * number of this type.
'            

			VALID
			'*
'            * Phone numbers accepted are {@linkplain PhoneNumberUtil#isValidNumber(PhoneNumber) valid} and
'            * are grouped in a possible way for this locale. For example, a US number written as
'            * "65 02 53 00 00" and "650253 0000" are not accepted at this leniency level, whereas
'            * "650 253 0000", "650 2530000" or "6502530000" are.
'            * Numbers with more than one '/' symbol are also dropped at this level.
'            * <p>
'            * Warning: This level might result in lower coverage especially for regions outside of country
'            * code "+1". If you are not sure about which level to use, email the discussion group
'            * libphonenumber-discuss@googlegroups.com.
'            

			STRICT_GROUPING
			'*
'            * Phone numbers accepted are {@linkplain PhoneNumberUtil#isValidNumber(PhoneNumber) valid} and
'            * are grouped in the same way that we would have formatted it, or as a single block. For
'            * example, a US number written as "650 2530000" is not accepted at this leniency level, whereas
'            * "650 253 0000" or "6502530000" are.
'            * Numbers with more than one '/' symbol are also dropped at this level.
'            * <p>
'            * Warning: This level might result in lower coverage especially for regions outside of country
'            * code "+1". If you are not sure about which level to use, email the discussion group
'            * libphonenumber-discuss@googlegroups.com.
'            

			EXACT_GROUPING
		End Enum

		Public Function Verify(leniency__1 As Leniency, number As PhoneNumber, candidate As [String], util As PhoneNumberUtil) As Boolean
			Select Case leniency__1
				Case Leniency.POSSIBLE
					Return IsPossibleNumber(number)
				Case Leniency.VALID
					If True Then
						If Not util.IsValidNumber(number) OrElse Not PhoneNumberMatcher.ContainsOnlyValidXChars(number, candidate, util) Then
							Return False
						End If
						Return PhoneNumberMatcher.IsNationalPrefixPresentIfRequired(number, util)
					End If
				Case Leniency.STRICT_GROUPING
					If True Then
						If Not util.IsValidNumber(number) OrElse Not PhoneNumberMatcher.ContainsOnlyValidXChars(number, candidate, util) OrElse PhoneNumberMatcher.ContainsMoreThanOneSlash(candidate) OrElse Not PhoneNumberMatcher.IsNationalPrefixPresentIfRequired(number, util) Then
							Return False
						End If
						Return PhoneNumberMatcher.CheckNumberGroupingIsValid(number, candidate, util, Function(u As PhoneNumberUtil, n As PhoneNumber, nc As StringBuilder, eg As [String]()) 
						Return PhoneNumberMatcher.AllNumberGroupsRemainGrouped(u, n, nc, eg)

End Function)
					End If
				Case Leniency.EXACT_GROUPING, Else
					If True Then
						If Not util.IsValidNumber(number) OrElse Not PhoneNumberMatcher.ContainsOnlyValidXChars(number, candidate, util) OrElse PhoneNumberMatcher.ContainsMoreThanOneSlash(candidate) OrElse Not PhoneNumberMatcher.IsNationalPrefixPresentIfRequired(number, util) Then
							Return False
						End If
						Return PhoneNumberMatcher.CheckNumberGroupingIsValid(number, candidate, util, Function(u As PhoneNumberUtil, n As PhoneNumber, normalizedCandidate As StringBuilder, expectedNumberGroups As [String]()) 
						Return PhoneNumberMatcher.AllNumberGroupsAreExactlyPresent(u, n, normalizedCandidate, expectedNumberGroups)

End Function)
					End If
			End Select
		End Function

		' This class implements a singleton, so the only constructor is private.
		Private Sub New()
		End Sub

		Private Sub Init(filePrefix As [String])
			currentFilePrefix_ = filePrefix
            For Each regionCodes In countryCallingCodeToRegionCodeMap_
                supportedRegions_.UnionWith(regionCodes.Value)
            Next
			supportedRegions_.Remove(REGION_CODE_FOR_NON_GEO_ENTITY)
			Dim regions As List(Of [String]) = Nothing
			If countryCallingCodeToRegionCodeMap_.TryGetValue(NANPA_COUNTRY_CODE, regions) Then
				nanpaRegions_.UnionWith(regions)
			End If
		End Sub

		Private Sub LoadMetadataFromFile(filePrefix As [String], regionCode As [String], countryCallingCode As Integer)
			Dim asm = GetType(PhoneNumberUtil).GetTypeInfo().Assembly
			Dim isNonGeoRegion As Boolean = REGION_CODE_FOR_NON_GEO_ENTITY.Equals(regionCode)
			Dim name = If(asm.GetManifestResourceNames().Where(Function(n) n.EndsWith(filePrefix)).FirstOrDefault(), "missing")
			Using stream = asm.GetManifestResourceStream(name)
				Try
					Dim meta = BuildMetadataFromXml.BuildPhoneMetadataCollection(stream, False)
                    For Each m In meta.MetadataList
                        If isNonGeoRegion Then
                            countryCodeToNonGeographicalMetadataMap(m.CountryCode) = m
                        Else
                            regionToMetadataMap(m.Id) = m
                        End If
                    Next
				Catch generatedExceptionName As IOException
				End Try
			End Using
		End Sub

		'*
'        * Attempts to extract a possible number from the string passed in. This currently strips all
'        * leading characters that cannot be used to start a phone number. Characters that can be used to
'        * start a phone number are defined in the VALID_START_CHAR_PATTERN. If none of these characters
'        * are found in the number passed in, an empty string is returned. This function also attempts to
'        * strip off any alternative extensions or endings if two or more are present, such as in the case
'        * of: (530) 583-6985 x302/x2303. The second extension here makes this actually two phone numbers,
'        * (530) 583-6985 x302 and (530) 583-6985 x2303. We remove the second extension so that the first
'        * number is parsed correctly.
'        *
'        * @param number  the string that might contain a phone number
'        * @return        the number, stripped of any non-phone-number prefix (such as "Tel:") or an empty
'        *                string if no character used to start phone numbers (such as + or any digit) is
'        *                found in the number
'        

		Public Shared Function ExtractPossibleNumber(number As [String]) As [String]
			Dim m = VALID_START_CHAR_PATTERN.Match(number)
			If Not m.Success Then
				Return ""
			End If
			number = number.Substring(m.Index)
			' Remove trailing non-alpha non-numerical characters.
			number = PhoneNumberMatcher.TrimAfterUnwantedChars(number)
			' Check for extra numbers at the end.
			Dim secondNumber = SECOND_NUMBER_START_PATTERN.Match(number)
			If secondNumber.Success Then
				number = number.Substring(0, secondNumber.Index)
			End If
			Return number
		End Function

		'*
'        * Checks to see if the string of characters could possibly be a phone number at all. At the
'        * moment, checks to see that the string begins with at least 2 digits, ignoring any punctuation
'        * commonly found in phone numbers.
'        * This method does not require the number to be normalized in advance - but does assume that
'        * leading non-number symbols have been removed, such as by the method extractPossibleNumber.
'        *
'        * @param number  string to be checked for viability as a phone number
'        * @return        true if the number could be a phone number of some sort, otherwise false
'        

		Public Shared Function IsViablePhoneNumber(number As [String]) As Boolean
			If number.Length < MIN_LENGTH_FOR_NSN Then
				Return False
			End If
			Return VALID_PHONE_NUMBER_PATTERN.MatchAll(number).Success
		End Function

		'*
'        * Normalizes a string of characters representing a phone number. This performs the following
'        * conversions:
'        *   Punctuation is stripped.
'        *   For ALPHA/VANITY numbers:
'        *   Letters are converted to their numeric representation on a telephone keypad. The keypad
'        *       used here is the one defined in ITU Recommendation E.161. This is only done if there are
'        *       3 or more letters in the number, to lessen the risk that such letters are typos.
'        *   For other numbers:
'        *   Wide-ascii digits are converted to normal ASCII (European) digits.
'        *   Arabic-Indic numerals are converted to European numerals.
'        *   Spurious alpha characters are stripped.
'        *   Arabic-Indic numerals are converted to European numerals.
'        *
'        * @param number  a string of characters representing a phone number
'        * @return        the normalized string version of the phone number
'        

		Public Shared Function Normalize(number As [String]) As [String]
			If VALID_ALPHA_PHONE_PATTERN.MatchAll(number).Success Then
				Return NormalizeHelper(number, ALPHA_PHONE_MAPPINGS, True)
			Else
				Return NormalizeDigitsOnly(number)
			End If
		End Function

		Private Shared Sub Normalize(number As StringBuilder)
			Dim n = Normalize(number.ToString())
			'XXX: ToString
			number.Length = 0
			number.Append(n)
		End Sub

		'*
'        * Normalizes a string of characters representing a phone number. This converts wide-ascii and
'        * arabic-indic numerals to European numerals, and strips punctuation and alpha characters.
'        *
'        * @param number  a string of characters representing a phone number
'        * @return        the normalized string version of the phone number
'        

		Public Shared Function NormalizeDigitsOnly(number As [String]) As [String]
			' strip non-digits 
			Return NormalizeDigits(number, False).ToString()
		End Function

		Friend Shared Function NormalizeDigits(number As [String], keepNonDigits As Boolean) As StringBuilder
			Dim normalizedDigits As New StringBuilder(number.Length)
			For Each c As Char In number.ToCharArray()
				Dim digit As Integer = CInt(Char.GetNumericValue(c))
				If digit <> -1 Then
					normalizedDigits.Append(digit)
				ElseIf keepNonDigits Then
					normalizedDigits.Append(c)
				End If
			Next
			Return normalizedDigits
		End Function

		'*
'        * Converts all alpha characters in a number to their respective digits on a keypad, but retains
'        * existing formatting.
'        

		Public Shared Function ConvertAlphaCharactersInNumber(number As [String]) As [String]
			Return NormalizeHelper(number, ALPHA_PHONE_MAPPINGS, False)
		End Function

		'*
'        * Gets the length of the geographical area code from the {@code nationalNumber_} field of the
'        * PhoneNumber object passed in, so that clients could use it to split a national significant
'        * number into geographical area code and subscriber number. It works in such a way that the
'        * resultant subscriber number should be diallable, at least on some devices. An example of how
'        * this could be used:
'        *
'        * <pre>
'        * PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance();
'        * PhoneNumber number = phoneUtil.parse("16502530000", "US");
'        * String nationalSignificantNumber = phoneUtil.getNationalSignificantNumber(number);
'        * String areaCode;
'        * String subscriberNumber;
'        *
'        * int areaCodeLength = phoneUtil.getLengthOfGeographicalAreaCode(number);
'        * if (areaCodeLength > 0) {
'        *   areaCode = nationalSignificantNumber.substring(0, areaCodeLength);
'        *   subscriberNumber = nationalSignificantNumber.substring(areaCodeLength);
'        * } else {
'        *   areaCode = "";
'        *   subscriberNumber = nationalSignificantNumber;
'        * }
'        * </pre>
'        *
'        * N.B.: area code is a very ambiguous concept, so the I18N team generally recommends against
'        * using it for most purposes, but recommends using the more general {@code national_number}
'        * instead. Read the following carefully before deciding to use this method:
'        * <ul>
'        *  <li> geographical area codes change over time, and this method honors those changes;
'        *    therefore, it doesn't guarantee the stability of the result it produces.
'        *  <li> subscriber numbers may not be diallable from all devices (notably mobile devices, which
'        *    typically requires the full national_number to be dialled in most regions).
'        *  <li> most non-geographical numbers have no area codes, including numbers from non-geographical
'        *    entities
'        *  <li> some geographical numbers have no area codes.
'        * </ul>
'        *
'        * @param number  the PhoneNumber object for which clients want to know the length of the area
'        *     code.
'        * @return  the length of area code of the PhoneNumber object passed in.
'        

		Public Function GetLengthOfGeographicalAreaCode(number As PhoneNumber) As Integer
			Dim regionCode = GetRegionCodeForNumber(number)
			If Not IsValidRegionCode(regionCode) Then
				Return 0
			End If
			Dim metadata = GetMetadataForRegion(regionCode)
			' If a country doesn't use a national prefix, and this number doesn't have an Italian leading
			' zero, we assume it is a closed dialling plan with no area codes.
			If Not metadata.HasNationalPrefix AndAlso Not number.ItalianLeadingZero Then
				Return 0
			End If

			Dim type = GetNumberTypeHelper(GetNationalSignificantNumber(number), metadata)
			' Most numbers other than the two types below have to be dialled in full.
			If type <> PhoneNumberType.FIXED_LINE AndAlso type <> PhoneNumberType.FIXED_LINE_OR_MOBILE Then
				Return 0
			End If
			Return GetLengthOfNationalDestinationCode(number)
		End Function

		'*
'        * Gets the length of the national destination code (NDC) from the PhoneNumber object passed in,
'        * so that clients could use it to split a national significant number into NDC and subscriber
'        * number. The NDC of a phone number is normally the first group of digit(s) right after the
'        * country calling code when the number is formatted in the international format, if there is a
'        * subscriber number part that follows. An example of how this could be used:
'        *
'        * <pre>
'        * PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance();
'        * PhoneNumber number = phoneUtil.parse("18002530000", "US");
'        * String nationalSignificantNumber = phoneUtil.getNationalSignificantNumber(number);
'        * String nationalDestinationCode;
'        * String subscriberNumber;
'        *
'        * int nationalDestinationCodeLength = phoneUtil.getLengthOfNationalDestinationCode(number);
'        * if (nationalDestinationCodeLength > 0) {
'        *   nationalDestinationCode = nationalSignificantNumber.substring(0,
'        *       nationalDestinationCodeLength);
'        *   subscriberNumber = nationalSignificantNumber.substring(nationalDestinationCodeLength);
'        * } else {
'        *   nationalDestinationCode = "";
'        *   subscriberNumber = nationalSignificantNumber;
'        * }
'        * </pre>
'        *
'        * Refer to the unittests to see the difference between this function and
'        * {@link #getLengthOfGeographicalAreaCode}.
'        *
'        * @param number  the PhoneNumber object for which clients want to know the length of the NDC.
'        * @return  the length of NDC of the PhoneNumber object passed in.
'        

		Public Function GetLengthOfNationalDestinationCode(number As PhoneNumber) As Integer
			Dim copiedProto As PhoneNumber
			If number.HasExtension Then
				' We don't want to alter the proto given to us, but we don't want to include the extension
				' when we format it, so we copy it and clear the extension here.
				Dim builder = New PhoneNumber.Builder()
				builder.MergeFrom(number)
				builder.ClearExtension()
				copiedProto = builder.Build()
			Else
				copiedProto = number
			End If
			Dim nationalSignificantNumber = Format(copiedProto, PhoneNumberFormat.INTERNATIONAL)
			Dim numberGroups = NON_DIGITS_PATTERN.Split(nationalSignificantNumber)
			' The pattern will start with "+COUNTRY_CODE " so the first group will always be the empty
			' string (before the + symbol) and the second group will be the country calling code. The third
			' group will be area code if it is not the last group.
			If numberGroups.Length <= 3 Then
				Return 0
			End If

			If GetRegionCodeForCountryCode(number.CountryCode) = "AR" AndAlso GetNumberType(number) = PhoneNumberType.MOBILE Then
				' Argentinian mobile numbers, when formatted in the international format, are in the form of
				' +54 9 NDC XXXX.... As a result, we take the length of the third group (NDC) and add 1 for
				' the digit 9, which also forms part of the national significant number.
				'
				' TODO: Investigate the possibility of better modeling the metadata to make it
				' easier to obtain the NDC.
				Return numberGroups(3).Length + 1
			End If
			Return numberGroups(2).Length
		End Function

		'*
'        * Normalizes a string of characters representing a phone number by replacing all characters found
'        * in the accompanying map with the values therein, and stripping all other characters if
'        * removeNonMatches is true.
'        *
'        * @param number                     a string of characters representing a phone number
'        * @param normalizationReplacements  a mapping of characters to what they should be replaced by in
'        *                                   the normalized version of the phone number
'        * @param removeNonMatches           indicates whether characters that are not able to be replaced
'        *                                   should be stripped from the number. If this is false, they
'        *                                   will be left unchanged in the number.
'        * @return  the normalized string version of the phone number
'        

		Private Shared Function NormalizeHelper(number As [String], normalizationReplacements As Dictionary(Of Char, Char), removeNonMatches As Boolean) As [String]
			Dim normalizedNumber = New StringBuilder(number.Length)
			Dim numberAsCharArray = number.ToCharArray()
            For Each character In numberAsCharArray
                Dim newDigit As Char
                If normalizationReplacements.TryGetValue(Char.ToUpper(character), newDigit) Then
                    normalizedNumber.Append(newDigit)
                ElseIf Not removeNonMatches Then
                    normalizedNumber.Append(character)
                    ' If neither of the above are true, we remove this character.
                End If
            Next
			Return normalizedNumber.ToString()
		End Function

        Public ReadOnly Property CallingCodeToRegion() As IReadOnlyDictionary(Of Integer, List(Of [String]))
            Get
                Return countryCallingCodeToRegionCodeMap_
            End Get
        End Property

		Public Shared Function GetInstance(baseFileLocation As [String], countryCallingCodeToRegionCodeMap As Dictionary(Of Integer, List(Of [String]))) As PhoneNumberUtil
			SyncLock thisLock
				If instance_ Is Nothing Then
					instance_ = New PhoneNumberUtil()
					instance_.countryCallingCodeToRegionCodeMap_ = countryCallingCodeToRegionCodeMap
					instance_.Init(baseFileLocation)
				End If
				Return instance_
			End SyncLock
		End Function

		'*
'        * Used for testing purposes only to reset the PhoneNumberUtil singleton to null.
'        

		Public Shared Sub ResetInstance()
			SyncLock thisLock
				instance_ = Nothing
			End SyncLock
		End Sub

		'*
'        * Convenience method to get a list of what regions the library has metadata for.
'        

		Public Function GetSupportedRegions() As HashSet(Of [String])
			Return supportedRegions_
		End Function

		'*
'        * Convenience method to get a list of what global network calling codes the library has metadata
'        * for.
'        

		Public Function GetSupportedGlobalNetworkCallingCodes() As Dictionary(Of Integer, PhoneMetadata).KeyCollection
			Return countryCodeToNonGeographicalMetadataMap.Keys
		End Function

		'*
'        * Gets a {@link PhoneNumberUtil} instance to carry out international phone number formatting,
'        * parsing, or validation. The instance is loaded with phone number metadata for a number of most
'        * commonly used regions.
'        *
'        * <p>The {@link PhoneNumberUtil} is implemented as a singleton. Therefore, calling getInstance
'        * multiple times will only result in one instance being created.
'        *
'        * @return a PhoneNumberUtil instance
'        

		Public Shared Function GetInstance() As PhoneNumberUtil
			SyncLock thisLock
				If instance_ Is Nothing Then
					Return GetInstance(META_DATA_FILE_PREFIX, CountryCodeToRegionCodeMap.GetCountryCodeToRegionCodeMap())
				End If
				Return instance_
			End SyncLock
		End Function

		'*
'        * Helper function to check region code is not unknown or null.
'        

		Private Function IsValidRegionCode(regionCode As [String]) As Boolean
			Return regionCode IsNot Nothing AndAlso supportedRegions_.Contains(regionCode)
		End Function

		'*
'        * Helper function to check the country calling code is valid.
'        

		Private Function HasValidCountryCallingCode(countryCallingCode As Integer) As Boolean
			Return countryCallingCodeToRegionCodeMap_.ContainsKey(countryCallingCode)
		End Function

		'*
'        * Formats a phone number in the specified format using default rules. Note that this does not
'        * promise to produce a phone number that the user can dial from where they are - although we do
'        * format in either 'national' or 'international' format depending on what the client asks for, we
'        * do not currently support a more abbreviated format, such as for users in the same "area" who
'        * could potentially dial the number without area code. Note that if the phone number has a
'        * country calling code of 0 or an otherwise invalid country calling code, we cannot work out
'        * which formatting rules to apply so we return the national significant number with no formatting
'        * applied.
'        *
'        * @param number         the phone number to be formatted
'        * @param numberFormat   the format the phone number should be formatted into
'        * @return  the formatted phone number
'        

		Public Function Format(number As PhoneNumber, numberFormat As PhoneNumberFormat) As [String]
			If number.NationalNumber = 0 AndAlso number.HasRawInput Then
				Dim rawInput As [String] = number.RawInput
				If rawInput.Length > 0 Then
					Return rawInput
				End If
			End If
			Dim formattedNumber = New StringBuilder(20)
			Format(number, numberFormat, formattedNumber)
			Return formattedNumber.ToString()
		End Function

		'*
'        * Same as {@link #format(PhoneNumber, PhoneNumberFormat)}, but accepts a mutable StringBuilder as
'        * a parameter to decrease object creation when invoked many times.
'        

		Public Sub Format(number As PhoneNumber, numberFormat As PhoneNumberFormat, formattedNumber As StringBuilder)
			' Clear the StringBuilder first.
			formattedNumber.Length = 0
			Dim countryCallingCode = number.CountryCode
			Dim nationalSignificantNumber = GetNationalSignificantNumber(number)
			If numberFormat = PhoneNumberFormat.E164 Then
				' Early exit for E164 case since no formatting of the national number needs to be applied.
				' Extensions are not formatted.
				formattedNumber.Append(nationalSignificantNumber)
				PrefixNumberWithCountryCallingCode(countryCallingCode, PhoneNumberFormat.E164, formattedNumber)
				Return
			End If
			' Note getRegionCodeForCountryCode() is used because formatting information for regions which
			' share a country calling code is contained by only one region for performance reasons. For
			' example, for NANPA regions it will be contained in the metadata for US.
			Dim regionCode = GetRegionCodeForCountryCode(countryCallingCode)
			If Not HasValidCountryCallingCode(countryCallingCode) Then
				formattedNumber.Append(nationalSignificantNumber)
				Return
			End If

			Dim metadata As PhoneMetadata = GetMetadataForRegionOrCallingCode(countryCallingCode, regionCode)
			formattedNumber.Append(FormatNsn(nationalSignificantNumber, metadata, numberFormat))
			MaybeAppendFormattedExtension(number, metadata, numberFormat, formattedNumber)
			PrefixNumberWithCountryCallingCode(countryCallingCode, numberFormat, formattedNumber)
		End Sub

		'*
'        * Formats a phone number in the specified format using client-defined formatting rules. Note that
'        * if the phone number has a country calling code of zero or an otherwise invalid country calling
'        * code, we cannot work out things like whether there should be a national prefix applied, or how
'        * to format extensions, so we return the national significant number with no formatting applied.
'        *
'        * @param number                        the phone number to be formatted
'        * @param numberFormat                  the format the phone number should be formatted into
'        * @param userDefinedFormats            formatting rules specified by clients
'        * @return  the formatted phone number
'        

		Public Function FormatByPattern(number As PhoneNumber, numberFormat As PhoneNumberFormat, userDefinedFormats As List(Of NumberFormat)) As [String]
			Dim countryCallingCode As Integer = number.CountryCode
			Dim nationalSignificantNumber = GetNationalSignificantNumber(number)
			' Note getRegionCodeForCountryCode() is used because formatting information for regions which
			' share a country calling code is contained by only one region for performance reasons. For
			' example, for NANPA regions it will be contained in the metadata for US.
			Dim regionCode = GetRegionCodeForCountryCode(countryCallingCode)
			If Not HasValidCountryCallingCode(countryCallingCode) Then
				Return nationalSignificantNumber
			End If

			Dim metadata As PhoneMetadata = GetMetadataForRegionOrCallingCode(countryCallingCode, regionCode)
			Dim formattedNumber As New StringBuilder(20)
			Dim formattingPattern As NumberFormat = ChooseFormattingPatternForNumber(userDefinedFormats, nationalSignificantNumber)
			If formattingPattern Is Nothing Then
				' If no pattern above is matched, we format the number as a whole.
				formattedNumber.Append(nationalSignificantNumber)
			Else
				Dim numFormatCopy = New NumberFormat.Builder()
				' Before we do a replacement of the national prefix pattern $NP with the national prefix, we
				' need to copy the rule so that subsequent replacements for different numbers have the
				' appropriate national prefix.
				numFormatCopy.MergeFrom(formattingPattern)
				Dim nationalPrefixFormattingRule As [String] = formattingPattern.NationalPrefixFormattingRule
				If nationalPrefixFormattingRule.Length > 0 Then
					Dim nationalPrefix As [String] = metadata.NationalPrefix
					If nationalPrefix.Length > 0 Then
						' Replace $NP with national prefix and $FG with the first group ($1).
						nationalPrefixFormattingRule = NP_PATTERN.Replace(nationalPrefixFormattingRule, nationalPrefix, 1)
						nationalPrefixFormattingRule = FG_PATTERN.Replace(nationalPrefixFormattingRule, "$$1", 1)
						numFormatCopy.SetNationalPrefixFormattingRule(nationalPrefixFormattingRule)
					Else
						' We don't want to have a rule for how to format the national prefix if there isn't one.
						numFormatCopy.ClearNationalPrefixFormattingRule()
					End If
				End If
				formattedNumber.Append(FormatNsnUsingPattern(nationalSignificantNumber, numFormatCopy.Build(), numberFormat))
			End If
			MaybeAppendFormattedExtension(number, metadata, numberFormat, formattedNumber)
			PrefixNumberWithCountryCallingCode(countryCallingCode, numberFormat, formattedNumber)
			Return formattedNumber.ToString()
		End Function

		'*
'        * Formats a phone number in national format for dialing using the carrier as specified in the
'        * {@code carrierCode}. The {@code carrierCode} will always be used regardless of whether the
'        * phone number already has a preferred domestic carrier code stored. If {@code carrierCode}
'        * contains an empty string, returns the number in national format without any carrier code.
'        * 
'        * @param number  the phone number to be formatted
'        * @param carrierCode  the carrier selection code to be used
'        * @return  the formatted phone number in national format for dialing using the carrier as
'        *          specified in the {@code carrierCode}
'        

		Public Function FormatNationalNumberWithCarrierCode(number As PhoneNumber, carrierCode As [String]) As [String]
			Dim countryCallingCode = number.CountryCode
			Dim nationalSignificantNumber = GetNationalSignificantNumber(number)
			' Note getRegionCodeForCountryCode() is used because formatting information for regions which
			' share a country calling code is contained by only one region for performance reasons. For
			' example, for NANPA regions it will be contained in the metadata for US.
			Dim regionCode = GetRegionCodeForCountryCode(countryCallingCode)
			If Not HasValidCountryCallingCode(countryCallingCode) Then
				Return nationalSignificantNumber
			End If

			Dim formattedNumber = New StringBuilder(20)
			Dim metadata As PhoneMetadata = GetMetadataForRegionOrCallingCode(countryCallingCode, regionCode)
			formattedNumber.Append(FormatNsn(nationalSignificantNumber, metadata, PhoneNumberFormat.NATIONAL, carrierCode))
			MaybeAppendFormattedExtension(number, metadata, PhoneNumberFormat.NATIONAL, formattedNumber)
			PrefixNumberWithCountryCallingCode(countryCallingCode, PhoneNumberFormat.NATIONAL, formattedNumber)
			Return formattedNumber.ToString()
		End Function

		Private Function GetMetadataForRegionOrCallingCode(countryCallingCode As Integer, regionCode As [String]) As PhoneMetadata
			Return If(REGION_CODE_FOR_NON_GEO_ENTITY.Equals(regionCode), GetMetadataForNonGeographicalRegion(countryCallingCode), GetMetadataForRegion(regionCode))
		End Function

		'*
'        * Formats a phone number in national format for dialing using the carrier as specified in the
'        * preferredDomesticCarrierCode field of the PhoneNumber object passed in. If that is missing,
'        * use the {@code fallbackCarrierCode} passed in instead. If there is no
'        * {@code preferredDomesticCarrierCode}, and the {@code fallbackCarrierCode} contains an empty
'        * string, return the number in national format without any carrier code.
'        *
'        * <p>Use {@link #formatNationalNumberWithCarrierCode} instead if the carrier code passed in
'        * should take precedence over the number's {@code preferredDomesticCarrierCode} when formatting.
'        *
'        * @param number  the phone number to be formatted
'        * @param fallbackCarrierCode  the carrier selection code to be used, if none is found in the
'        *     phone number itself
'        * @return  the formatted phone number in national format for dialing using the number's
'        *     {@code preferredDomesticCarrierCode}, or the {@code fallbackCarrierCode} passed in if
'        *     none is found
'        

		Public Function FormatNationalNumberWithPreferredCarrierCode(number As PhoneNumber, fallbackCarrierCode As [String]) As [String]
			Return FormatNationalNumberWithCarrierCode(number, If(number.HasPreferredDomesticCarrierCode, number.PreferredDomesticCarrierCode, fallbackCarrierCode))
		End Function

		'*
'        * Returns a number formatted in such a way that it can be dialed from a mobile phone in a
'        * specific region. If the number cannot be reached from the region (e.g. some countries block
'        * toll-free numbers from being called outside of the country), the method returns an empty
'        * string.
'        *
'        * @param number  the phone number to be formatted
'        * @param regionCallingFrom  the region where the call is being placed
'        * @param withFormatting  whether the number should be returned with formatting symbols, such as
'        *     spaces and dashes.
'        * @return  the formatted phone number
'        

		Public Function FormatNumberForMobileDialing(number As PhoneNumber, regionCallingFrom As [String], withFormatting As Boolean) As [String]
			Dim countryCallingCode As Integer = number.CountryCode
			If Not HasValidCountryCallingCode(countryCallingCode) Then
				Return If(number.HasRawInput, number.RawInput, "")
			End If

			Dim formattedNumber As [String]
			' Clear the extension, as that part cannot normally be dialed together with the main number.
			Dim numberNoExt As PhoneNumber = New PhoneNumber.Builder().MergeFrom(number).ClearExtension().Build()
			Dim numberType As PhoneNumberType = GetNumberType(numberNoExt)
			Dim regionCode As [String] = GetRegionCodeForCountryCode(countryCallingCode)
			If regionCode.Equals("CO") AndAlso regionCallingFrom.Equals("CO") Then
				If numberType = PhoneNumberType.FIXED_LINE Then
					formattedNumber = FormatNationalNumberWithCarrierCode(numberNoExt, COLOMBIA_MOBILE_TO_FIXED_LINE_PREFIX)
				Else
					' E164 doesn't work at all when dialing within Colombia.
					formattedNumber = Format(numberNoExt, PhoneNumberFormat.NATIONAL)
				End If
			ElseIf regionCode.Equals("PE") AndAlso regionCallingFrom.Equals("PE") Then
				' In Peru, numbers cannot be dialled using E164 format from a mobile phone for Movistar.
				' Instead they must be dialled in national format.
				formattedNumber = Format(numberNoExt, PhoneNumberFormat.NATIONAL)
			ElseIf regionCode.Equals("BR") AndAlso regionCallingFrom.Equals("BR") AndAlso ((numberType = PhoneNumberType.FIXED_LINE) OrElse (numberType = PhoneNumberType.MOBILE) OrElse (numberType = PhoneNumberType.FIXED_LINE_OR_MOBILE)) Then
				' Brazilian fixed line and mobile numbers need to be dialed with a carrier code when
				' called within Brazil. Without that, most of the carriers won't connect the call.
				' Because of that, we return an empty string here.
				formattedNumber = If(numberNoExt.HasPreferredDomesticCarrierCode, FormatNationalNumberWithPreferredCarrierCode(numberNoExt, ""), "")
			ElseIf CanBeInternationallyDialled(numberNoExt) Then
				Return If(withFormatting, Format(numberNoExt, PhoneNumberFormat.INTERNATIONAL), Format(numberNoExt, PhoneNumberFormat.E164))
			Else
				formattedNumber = If((regionCallingFrom = regionCode), Format(numberNoExt, PhoneNumberFormat.NATIONAL), "")
			End If
				' remove non matches 
			Return If(withFormatting, formattedNumber, NormalizeHelper(formattedNumber, DIALLABLE_CHAR_MAPPINGS, True))
		End Function

		'*
'        * Formats a phone number for out-of-country dialing purposes. If no regionCallingFrom is
'        * supplied, we format the number in its INTERNATIONAL format. If the country calling code is the
'        * same as that of the region where the number is from, then NATIONAL formatting will be applied.
'        *
'        * <p>If the number itself has a country calling code of zero or an otherwise invalid country
'        * calling code, then we return the number with no formatting applied.
'        *
'        * <p>Note this function takes care of the case for calling inside of NANPA and between Russia and
'        * Kazakhstan (who share the same country calling code). In those cases, no international prefix
'        * is used. For regions which have multiple international prefixes, the number in its
'        * INTERNATIONAL format will be returned instead.
'        *
'        * @param number               the phone number to be formatted
'        * @param regionCallingFrom    the region where the call is being placed
'        * @return  the formatted phone number
'        

		Public Function FormatOutOfCountryCallingNumber(number As PhoneNumber, regionCallingFrom As [String]) As [String]
			If Not IsValidRegionCode(regionCallingFrom) Then
				' LOGGER.log(Level.WARNING,
				'      "Trying to format number from invalid region "
				'      + regionCallingFrom
				'      + ". International formatting applied.");
				Return Format(number, PhoneNumberFormat.INTERNATIONAL)
			End If
			Dim countryCallingCode As Integer = number.CountryCode
			Dim nationalSignificantNumber = GetNationalSignificantNumber(number)
			If Not HasValidCountryCallingCode(countryCallingCode) Then
				Return nationalSignificantNumber
			End If
			If countryCallingCode = NANPA_COUNTRY_CODE Then
				If IsNANPACountry(regionCallingFrom) Then
					' For NANPA regions, return the national format for these regions but prefix it with the
					' country calling code.
					Return countryCallingCode + " " + Format(number, PhoneNumberFormat.NATIONAL)
				End If
			ElseIf countryCallingCode = GetCountryCodeForValidRegion(regionCallingFrom) Then
				' For regions that share a country calling code, the country calling code need not be dialled.
				' This also applies when dialling within a region, so this if clause covers both these cases.
				' Technically this is the case for dialling from La Reunion to other overseas departments of
				' France (French Guiana, Martinique, Guadeloupe), but not vice versa - so we don't cover this
				' edge case for now and for those cases return the version including country calling code.
				' Details here: http://www.petitfute.com/voyage/225-info-pratiques-reunion
				Return Format(number, PhoneNumberFormat.NATIONAL)
			End If
			Dim metadataForRegionCallingFrom As PhoneMetadata = GetMetadataForRegion(regionCallingFrom)
			Dim internationalPrefix As [String] = metadataForRegionCallingFrom.InternationalPrefix

			' For regions that have multiple international prefixes, the international format of the
			' number is returned, unless there is a preferred international prefix.
			Dim internationalPrefixForFormatting = ""
			If UNIQUE_INTERNATIONAL_PREFIX.MatchAll(internationalPrefix).Success Then
				internationalPrefixForFormatting = internationalPrefix
			ElseIf metadataForRegionCallingFrom.HasPreferredInternationalPrefix Then
				internationalPrefixForFormatting = metadataForRegionCallingFrom.PreferredInternationalPrefix
			End If

			Dim regionCode As [String] = GetRegionCodeForCountryCode(countryCallingCode)
			Dim metadataForRegion As PhoneMetadata = GetMetadataForRegionOrCallingCode(countryCallingCode, regionCode)
			Dim formattedNationalNumber As [String] = FormatNsn(nationalSignificantNumber, metadataForRegion, PhoneNumberFormat.INTERNATIONAL)
			Dim formattedNumber = New StringBuilder(formattedNationalNumber)
			MaybeAppendFormattedExtension(number, metadataForRegion, PhoneNumberFormat.INTERNATIONAL, formattedNumber)
			If internationalPrefixForFormatting.Length > 0 Then
				formattedNumber.Insert(0, " ").Insert(0, countryCallingCode).Insert(0, " ").Insert(0, internationalPrefixForFormatting)
			Else
				PrefixNumberWithCountryCallingCode(countryCallingCode, PhoneNumberFormat.INTERNATIONAL, formattedNumber)
			End If
			Return formattedNumber.ToString()
		End Function

		'*
'        * Formats a phone number using the original phone number format that the number is parsed from.
'        * The original format is embedded in the country_code_source field of the PhoneNumber object
'        * passed in. If such information is missing, the number will be formatted into the NATIONAL
'        * format by default. When the number contains a leading zero and this is unexpected for this
'        * country, or we don't have a formatting pattern for the number, the method returns the raw input
'        * when it is available.
'        *
'        * Note this method guarantees no digit will be inserted, removed or modified as a result of
'        * formatting.
'        * 
'        * @param number  the phone number that needs to be formatted in its original number format
'        * @param regionCallingFrom  the region whose IDD needs to be prefixed if the original number
'        *     has one
'        * @return  the formatted phone number in its original number format
'        

		Public Function FormatInOriginalFormat(number As PhoneNumber, regionCallingFrom As [String]) As [String]
			If number.HasRawInput AndAlso (HasUnexpectedItalianLeadingZero(number) OrElse Not HasFormattingPatternForNumber(number)) Then
				' We check if we have the formatting pattern because without that, we might format the number
				' as a group without national prefix.
				Return number.RawInput
			End If

			If Not number.HasCountryCodeSource Then
				Return Format(number, PhoneNumberFormat.NATIONAL)
			End If

			Dim formattedNumber As [String]
			Select Case number.CountryCodeSource
				Case CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN
					formattedNumber = Format(number, PhoneNumberFormat.INTERNATIONAL)
					Exit Select
				Case CountryCodeSource.FROM_NUMBER_WITH_IDD
					formattedNumber = FormatOutOfCountryCallingNumber(number, regionCallingFrom)
					Exit Select
				Case CountryCodeSource.FROM_NUMBER_WITHOUT_PLUS_SIGN
					formattedNumber = Format(number, PhoneNumberFormat.INTERNATIONAL).Substring(1)
					Exit Select
				Case CountryCodeSource.FROM_DEFAULT_COUNTRY, Else
					' Fall-through to default case.
					Dim regionCode As [String] = GetRegionCodeForCountryCode(number.CountryCode)
					' We strip non-digits from the NDD here, and from the raw input later, so that we can
					' compare them easily.
						' strip non-digits 
					Dim nationalPrefix As [String] = GetNddPrefixForRegion(regionCode, True)
					Dim nationalFormat As [String] = Format(number, PhoneNumberFormat.NATIONAL)
					If nationalPrefix Is Nothing OrElse nationalPrefix.Length = 0 Then
						' If the region doesn't have a national prefix at all, we can safely return the national
						' format without worrying about a national prefix being added.
						formattedNumber = nationalFormat
						Exit Select
					End If
					' Otherwise, we check if the original number was entered with a national prefix.
					If RawInputContainsNationalPrefix(number.RawInput, nationalPrefix, regionCode) Then
						' If so, we can safely return the national format.
						formattedNumber = nationalFormat
						Exit Select
					End If
					Dim metadata As PhoneMetadata = GetMetadataForRegion(regionCode)
					Dim nationalNumber As [String] = GetNationalSignificantNumber(number)
					Dim formatRule As NumberFormat = ChooseFormattingPatternForNumber(metadata.NumberFormatList, nationalNumber)
					' When the format we apply to this number doesn't contain national prefix, we can just
					' return the national format.
					' TODO: Refactor the code below with the code in isNationalPrefixPresentIfRequired.
					Dim candidateNationalPrefixRule As [String] = formatRule.NationalPrefixFormattingRule
					' We assume that the first-group symbol will never be _before_ the national prefix.
					Dim indexOfFirstGroup As Integer = candidateNationalPrefixRule.IndexOf("${1}")
					If indexOfFirstGroup <= 0 Then
						formattedNumber = nationalFormat
						Exit Select
					End If
					candidateNationalPrefixRule = candidateNationalPrefixRule.Substring(0, indexOfFirstGroup)
					candidateNationalPrefixRule = NormalizeDigitsOnly(candidateNationalPrefixRule)
					If candidateNationalPrefixRule.Length = 0 Then
						' National prefix not used when formatting this number.
						formattedNumber = nationalFormat
						Exit Select
					End If
					' Otherwise, we need to remove the national prefix from our output.
					Dim numFormatCopy = New NumberFormat.Builder().MergeFrom(formatRule).ClearNationalPrefixFormattingRule().Build()
					Dim numberFormats As New List(Of NumberFormat)(1)
					numberFormats.Add(numFormatCopy)
					formattedNumber = FormatByPattern(number, PhoneNumberFormat.NATIONAL, numberFormats)
					Exit Select
			End Select
			Dim rawInput As [String] = number.RawInput
			' If no digit is inserted/removed/modified as a result of our formatting, we return the
			' formatted phone number; otherwise we return the raw input the user entered.
			' remove non matches 
				' remove non matches 
			Return If((formattedNumber IsNot Nothing AndAlso NormalizeHelper(formattedNumber, DIALLABLE_CHAR_MAPPINGS, True).Equals(NormalizeHelper(rawInput, DIALLABLE_CHAR_MAPPINGS, True))), formattedNumber, rawInput)
		End Function

		' Check if rawInput, which is assumed to be in the national format, has a national prefix. The
		' national prefix is assumed to be in digits-only form.
		Private Function RawInputContainsNationalPrefix(rawInput As [String], nationalPrefix As [String], regionCode As [String]) As Boolean
			Dim normalizedNationalNumber As [String] = NormalizeDigitsOnly(rawInput)
			If normalizedNationalNumber.StartsWith(nationalPrefix) Then
				Try
					' Some Japanese numbers (e.g. 00777123) might be mistaken to contain the national prefix
					' when written without it (e.g. 0777123) if we just do prefix matching. To tackle that, we
					' check the validity of the number if the assumed national prefix is removed (777123 won't
					' be valid in Japan).
					Return IsValidNumber(Parse(normalizedNationalNumber.Substring(nationalPrefix.Length), regionCode))
				Catch generatedExceptionName As NumberParseException
					Return False
				End Try
			End If
			Return False
		End Function

		'*
'        * Returns true if a number is from a region whose national significant number couldn't contain a
'        * leading zero, but has the italian_leading_zero field set to true.
'        

		Private Function HasUnexpectedItalianLeadingZero(number As PhoneNumber) As Boolean
			Return number.ItalianLeadingZero AndAlso Not IsLeadingZeroPossible(number.CountryCode)
		End Function

		Private Function HasFormattingPatternForNumber(number As PhoneNumber) As Boolean
			Dim countryCallingCode As Integer = number.CountryCode
			Dim phoneNumberRegion As [String] = GetRegionCodeForCountryCode(countryCallingCode)
			Dim metadata As PhoneMetadata = GetMetadataForRegionOrCallingCode(countryCallingCode, phoneNumberRegion)
			If metadata Is Nothing Then
				Return False
			End If
			Dim nationalNumber As [String] = GetNationalSignificantNumber(number)
			Dim formatRule As NumberFormat = ChooseFormattingPatternForNumber(metadata.NumberFormatList, nationalNumber)
			Return formatRule IsNot Nothing
		End Function

		'*
'        * Formats a phone number for out-of-country dialing purposes.
'        *
'        * Note that in this version, if the number was entered originally using alpha characters and
'        * this version of the number is stored in raw_input, this representation of the number will be
'        * used rather than the digit representation. Grouping information, as specified by characters
'        * such as "-" and " ", will be retained.
'        *
'        * <p><b>Caveats:</b></p>
'        * <ul>
'        *  <li> This will not produce good results if the country calling code is both present in the raw
'        *       input _and_ is the start of the national number. This is not a problem in the regions
'        *       which typically use alpha numbers.
'        *  <li> This will also not produce good results if the raw input has any grouping information
'        *       within the first three digits of the national number, and if the function needs to strip
'        *       preceding digits/words in the raw input before these digits. Normally people group the
'        *       first three digits together so this is not a huge problem - and will be fixed if it
'        *       proves to be so.
'        * </ul>
'        *
'        * @param number  the phone number that needs to be formatted
'        * @param regionCallingFrom  the region where the call is being placed
'        * @return  the formatted phone number
'        

		Public Function FormatOutOfCountryKeepingAlphaChars(number As PhoneNumber, regionCallingFrom As [String]) As [String]
			Dim rawInput = number.RawInput
			' If there is no raw input, then we can't keep alpha characters because there aren't any.
			' In this case, we return formatOutOfCountryCallingNumber.
			If rawInput.Length = 0 Then
				Return FormatOutOfCountryCallingNumber(number, regionCallingFrom)
			End If

			Dim countryCode As Integer = number.CountryCode
			If Not HasValidCountryCallingCode(countryCode) Then
				Return rawInput
			End If

			' Strip any prefix such as country calling code, IDD, that was present. We do this by comparing
			' the number in raw_input with the parsed number.
			' To do this, first we normalize punctuation. We retain number grouping symbols such as " "
			' only.
			rawInput = NormalizeHelper(rawInput, ALL_PLUS_NUMBER_GROUPING_SYMBOLS, True)
			' Now we trim everything before the first three digits in the parsed number. We choose three
			' because all valid alpha numbers have 3 digits at the start - if it does not, then we don't
			' trim anything at all. Similarly, if the national number was less than three digits, we don't
			' trim anything at all.
			Dim nationalNumber = GetNationalSignificantNumber(number)
			If nationalNumber.Length > 3 Then
				Dim firstNationalNumberDigit As Integer = rawInput.IndexOf(nationalNumber.Substring(0, 3))
				If firstNationalNumberDigit <> -1 Then
					rawInput = rawInput.Substring(firstNationalNumberDigit)
				End If
			End If
			Dim metadataForRegionCallingFrom = GetMetadataForRegion(regionCallingFrom)
			If countryCode = NANPA_COUNTRY_CODE Then
				If IsNANPACountry(regionCallingFrom) Then
					Return countryCode + " " + rawInput
				End If
			ElseIf IsValidRegionCode(regionCallingFrom) AndAlso countryCode = GetCountryCodeForValidRegion(regionCallingFrom) Then
				Dim formattingPattern As NumberFormat = ChooseFormattingPatternForNumber(metadataForRegionCallingFrom.NumberFormatList, nationalNumber)
				If formattingPattern Is Nothing Then
					' If no pattern above is matched, we format the original input.
					Return rawInput
				End If

				Dim newFormat = New NumberFormat.Builder()
				newFormat.MergeFrom(formattingPattern)
				' The first group is the first group of digits that the user wrote together.
				newFormat.SetPattern("(\d+)(.*)")
				' Here we just concatenate them back together after the national prefix has been fixed.
				newFormat.SetFormat("$1$2")
				' Now we format using this pattern instead of the default pattern, but with the national
				' prefix prefixed if necessary.
				' This will not work in the cases where the pattern (and not the leading digits) decide
				' whether a national prefix needs to be used, since we have overridden the pattern to match
				' anything, but that is not the case in the metadata to date.
				Return FormatNsnUsingPattern(rawInput, newFormat.Build(), PhoneNumberFormat.NATIONAL)
			End If
			Dim internationalPrefixForFormatting As [String] = ""
			' If an unsupported region-calling-from is entered, or a country with multiple international
			' prefixes, the international format of the number is returned, unless there is a preferred
			' international prefix.
			If metadataForRegionCallingFrom IsNot Nothing Then
				Dim internationalPrefix As [String] = metadataForRegionCallingFrom.InternationalPrefix
				internationalPrefixForFormatting = If(UNIQUE_INTERNATIONAL_PREFIX.MatchAll(internationalPrefix).Success, internationalPrefix, metadataForRegionCallingFrom.PreferredInternationalPrefix)
			End If
			Dim formattedNumber = New StringBuilder(rawInput)
			Dim regionCode As [String] = GetRegionCodeForCountryCode(countryCode)
			Dim metadataForRegion As PhoneMetadata = GetMetadataForRegionOrCallingCode(countryCode, regionCode)
			MaybeAppendFormattedExtension(number, metadataForRegion, PhoneNumberFormat.INTERNATIONAL, formattedNumber)
			If internationalPrefixForFormatting.Length > 0 Then
				formattedNumber.Insert(0, " ").Insert(0, countryCode).Insert(0, " ").Insert(0, internationalPrefixForFormatting)
			Else
				' Invalid region entered as country-calling-from (so no metadata was found for it) or the
				' region chosen has multiple international dialling prefixes.
				' LOGGER.log(Level.WARNING,
				' "Trying to format number from invalid region "
				' + regionCallingFrom
				' + ". International formatting applied.");
				PrefixNumberWithCountryCallingCode(countryCode, PhoneNumberFormat.INTERNATIONAL, formattedNumber)
			End If
			Return formattedNumber.ToString()
		End Function

		'*
'        * Gets the national significant number of the a phone number. Note a national significant number
'        * doesn't contain a national prefix or any formatting.
'        *
'        * @param number  the PhoneNumber object for which the national significant number is needed
'        * @return  the national significant number of the PhoneNumber object passed in
'        

		Public Function GetNationalSignificantNumber(number As PhoneNumber) As [String]
			' If a leading zero has been set, we prefix this now. Note this is not a national prefix.
			Dim nationalNumber As New StringBuilder(If(number.ItalianLeadingZero, "0", ""))
			nationalNumber.Append(number.NationalNumber)
			Return nationalNumber.ToString()
		End Function

		'*
'        * A helper function that is used by format and formatByPattern.
'        

		Private Sub PrefixNumberWithCountryCallingCode(countryCallingCode As Integer, numberFormat As PhoneNumberFormat, formattedNumber As StringBuilder)
			Select Case numberFormat
				Case PhoneNumberFormat.E164
					formattedNumber.Insert(0, countryCallingCode).Insert(0, PLUS_SIGN)
					Return
				Case PhoneNumberFormat.INTERNATIONAL
					formattedNumber.Insert(0, " ").Insert(0, countryCallingCode).Insert(0, PLUS_SIGN)
					Return
				Case PhoneNumberFormat.RFC3966
					formattedNumber.Insert(0, "-").Insert(0, countryCallingCode).Insert(0, PLUS_SIGN).Insert(0, RFC3966_PREFIX)
					Return
				Case PhoneNumberFormat.NATIONAL, Else
					Return
			End Select
		End Sub

		' Simple wrapper of formatNsn for the common case of no carrier code.
		Private Function FormatNsn(number As [String], metadata As PhoneMetadata, numberFormat As PhoneNumberFormat) As [String]
			Return FormatNsn(number, metadata, numberFormat, Nothing)
		End Function

		' Note in some regions, the national number can be written in two completely different ways
		' depending on whether it forms part of the NATIONAL format or INTERNATIONAL format. The
		' numberFormat parameter here is used to specify which format to use for those cases. If a
		' carrierCode is specified, this will be inserted into the formatted string to replace $CC.
		Private Function FormatNsn(number As [String], metadata As PhoneMetadata, numberFormat As PhoneNumberFormat, carrierCode As [String]) As [String]
			Dim intlNumberFormats = metadata.IntlNumberFormatList
			' When the intlNumberFormats exists, we use that to format national number for the
			' INTERNATIONAL format instead of using the numberDesc.numberFormats.
			Dim availableFormats = If((intlNumberFormats.Count = 0 OrElse numberFormat = PhoneNumberFormat.NATIONAL), metadata.NumberFormatList, metadata.IntlNumberFormatList)
			Dim formattingPattern As NumberFormat = ChooseFormattingPatternForNumber(availableFormats, number)
			Return If((formattingPattern Is Nothing), number, FormatNsnUsingPattern(number, formattingPattern, numberFormat, carrierCode))
		End Function

		Friend Function ChooseFormattingPatternForNumber(availableFormats As IList(Of NumberFormat), nationalNumber As [String]) As NumberFormat
			For Each numFormat As NumberFormat In availableFormats
				Dim size As Integer = numFormat.LeadingDigitsPatternCount
				' We always use the last leading_digits_pattern, as it is the most detailed.
				If size = 0 OrElse regexCache.GetPatternForRegex(numFormat.LeadingDigitsPatternList(size - 1)).MatchBeginning(nationalNumber).Success Then
					If regexCache.GetPatternForRegex(numFormat.Pattern).MatchAll(nationalNumber).Success Then
						Return numFormat
					End If
				End If
			Next
			Return Nothing
		End Function


		' Simple wrapper of formatNsnUsingPattern for the common case of no carrier code.
		Friend Function FormatNsnUsingPattern(nationalNumber As [String], formattingPattern As NumberFormat, numberFormat As PhoneNumberFormat) As [String]
			Return FormatNsnUsingPattern(nationalNumber, formattingPattern, numberFormat, Nothing)
		End Function

		' Note that carrierCode is optional - if NULL or an empty string, no carrier code replacement
		' will take place.
		Private Function FormatNsnUsingPattern(nationalNumber As [String], formattingPattern As NumberFormat, numberFormat As PhoneNumberFormat, carrierCode As [String]) As [String]
			Dim numberFormatRule As [String] = formattingPattern.Format
			Dim m = regexCache.GetPatternForRegex(formattingPattern.Pattern)
			Dim formattedNationalNumber As [String] = ""
			If numberFormat = PhoneNumberFormat.NATIONAL AndAlso carrierCode IsNot Nothing AndAlso carrierCode.Length > 0 AndAlso formattingPattern.DomesticCarrierCodeFormattingRule.Length > 0 Then
				' Replace the $CC in the formatting rule with the desired carrier code.
				Dim carrierCodeFormattingRule = formattingPattern.DomesticCarrierCodeFormattingRule
				carrierCodeFormattingRule = CC_PATTERN.Replace(carrierCodeFormattingRule, carrierCode, 1)
				' Now replace the $FG in the formatting rule with the first group and the carrier code
				' combined in the appropriate way.
				Dim r = FIRST_GROUP_PATTERN.Replace(numberFormatRule, carrierCodeFormattingRule, 1)
				formattedNationalNumber = m.Replace(nationalNumber, r)
			Else
				' Use the national prefix formatting rule instead.
				Dim nationalPrefixFormattingRule = formattingPattern.NationalPrefixFormattingRule
				If numberFormat = PhoneNumberFormat.NATIONAL AndAlso nationalPrefixFormattingRule IsNot Nothing AndAlso nationalPrefixFormattingRule.Length > 0 Then
					Dim r = FIRST_GROUP_PATTERN.Replace(numberFormatRule, nationalPrefixFormattingRule, 1)
					formattedNationalNumber = m.Replace(nationalNumber, r)
				Else
					formattedNationalNumber = m.Replace(nationalNumber, numberFormatRule)
				End If
			End If
			If numberFormat = PhoneNumberFormat.RFC3966 Then
				' Strip any leading punctuation.
				If SEPARATOR_PATTERN.MatchBeginning(formattedNationalNumber).Success Then
					formattedNationalNumber = SEPARATOR_PATTERN.Replace(formattedNationalNumber, "", 1)
				End If
				' Replace the rest with a dash between each number group.
				formattedNationalNumber = SEPARATOR_PATTERN.Replace(formattedNationalNumber, "-")
			End If
			Return formattedNationalNumber
		End Function

		'*
'        * Gets a valid number for the specified region.
'        *
'        * @param regionCode  region for which an example number is needed
'        * @return  a valid fixed-line number for the specified region. Returns null when the metadata
'        *    does not contain such information, or the region 001 is passed in. For 001 (representing
'        *    non-geographical numbers), call {@link #getExampleNumberForNonGeoEntity} instead.
'        

		Public Function GetExampleNumber(regionCode As [String]) As PhoneNumber
			Return GetExampleNumberForType(regionCode, PhoneNumberType.FIXED_LINE)
		End Function

		'*
'        * Gets a valid number for the specified region and number type.
'        *
'        * @param regionCode  region for which an example number is needed
'        * @param type  the type of number that is needed
'        * @return  a valid number for the specified region and type. Returns null when the metadata
'        *     does not contain such information or if an invalid region or region 001 was entered.
'        *     For 001 (representing non-geographical numbers), call
'        *     {@link #getExampleNumberForNonGeoEntity} instead.
'        

		Public Function GetExampleNumberForType(regionCode As [String], type As PhoneNumberType) As PhoneNumber
			' Check the region code is valid.
			If Not IsValidRegionCode(regionCode) Then
				Return Nothing
			End If
			Dim desc = GetNumberDescByType(GetMetadataForRegion(regionCode), type)
			Try
				If desc.HasExampleNumber Then
					Return Parse(desc.ExampleNumber, regionCode)
				End If
			Catch generatedExceptionName As NumberParseException
			End Try
			Return Nothing
		End Function

		'*
'        * Gets a valid number for the specified country calling code for a non-geographical entity.
'        *
'        * @param countryCallingCode  the country calling code for a non-geographical entity
'        * @return  a valid number for the non-geographical entity. Returns null when the metadata
'        *    does not contain such information, or the country calling code passed in does not belong
'        *    to a non-geographical entity.
'        

		Public Function GetExampleNumberForNonGeoEntity(countryCallingCode As Integer) As PhoneNumber
			Dim metadata As PhoneMetadata = GetMetadataForNonGeographicalRegion(countryCallingCode)
			If metadata IsNot Nothing Then
				Dim desc As PhoneNumberDesc = metadata.GeneralDesc
				Try
					If desc.HasExampleNumber Then
						Return Parse("+" + countryCallingCode + desc.ExampleNumber, "ZZ")
					End If
						'LOGGER.log(Level.SEVERE, e.toString());
				Catch generatedExceptionName As NumberParseException
				End Try
					'LOGGER.log(Level.WARNING,
					'  "Invalid or unknown country calling code provided: " + countryCallingCode);
			Else
			End If
			Return Nothing
		End Function

		'*
'        * Appends the formatted extension of a phone number to formattedNumber, if the phone number had
'        * an extension specified.
'        

		Private Sub MaybeAppendFormattedExtension(number As PhoneNumber, metadata As PhoneMetadata, numberFormat As PhoneNumberFormat, formattedNumber As StringBuilder)
			If number.HasExtension AndAlso number.Extension.Length > 0 Then
				If numberFormat = PhoneNumberFormat.RFC3966 Then
					formattedNumber.Append(RFC3966_EXTN_PREFIX).Append(number.Extension)
				Else
					If metadata.HasPreferredExtnPrefix Then
						formattedNumber.Append(metadata.PreferredExtnPrefix).Append(number.Extension)
					Else
						formattedNumber.Append(DEFAULT_EXTN_PREFIX).Append(number.Extension)
					End If
				End If
			End If
		End Sub

		Private Function GetNumberDescByType(metadata As PhoneMetadata, type As PhoneNumberType) As PhoneNumberDesc
			Select Case type
				Case PhoneNumberType.PREMIUM_RATE
					Return metadata.PremiumRate
				Case PhoneNumberType.TOLL_FREE
					Return metadata.TollFree
				Case PhoneNumberType.MOBILE
					Return metadata.Mobile
				Case PhoneNumberType.FIXED_LINE, PhoneNumberType.FIXED_LINE_OR_MOBILE
					Return metadata.FixedLine
				Case PhoneNumberType.SHARED_COST
					Return metadata.SharedCost
				Case PhoneNumberType.VOIP
					Return metadata.Voip
				Case PhoneNumberType.PERSONAL_NUMBER
					Return metadata.PersonalNumber
				Case PhoneNumberType.PAGER
					Return metadata.Pager
				Case PhoneNumberType.UAN
					Return metadata.Uan
				Case PhoneNumberType.VOICEMAIL
					Return metadata.Voicemail
				Case Else
					Return metadata.GeneralDesc
			End Select
		End Function

		'*
'        * Gets the type of a phone number.
'        *
'        * @param number  the phone number that we want to know the type
'        * @return  the type of the phone number
'        

		Public Function GetNumberType(number As PhoneNumber) As PhoneNumberType
			Dim regionCode = GetRegionCodeForNumber(number)
			If Not IsValidRegionCode(regionCode) AndAlso Not REGION_CODE_FOR_NON_GEO_ENTITY.Equals(regionCode) Then
				Return PhoneNumberType.UNKNOWN
			End If
			Dim nationalSignificantNumber = GetNationalSignificantNumber(number)
			Dim metadata As PhoneMetadata = GetMetadataForRegionOrCallingCode(number.CountryCode, regionCode)
			Return GetNumberTypeHelper(nationalSignificantNumber, metadata)
		End Function

		Private Function GetNumberTypeHelper(nationalNumber As [String], metadata As PhoneMetadata) As PhoneNumberType
			Dim generalNumberDesc = metadata.GeneralDesc
			If Not generalNumberDesc.HasNationalNumberPattern OrElse Not IsNumberMatchingDesc(nationalNumber, generalNumberDesc) Then
				Return PhoneNumberType.UNKNOWN
			End If

			If IsNumberMatchingDesc(nationalNumber, metadata.PremiumRate) Then
				Return PhoneNumberType.PREMIUM_RATE
			End If

			If IsNumberMatchingDesc(nationalNumber, metadata.TollFree) Then
				Return PhoneNumberType.TOLL_FREE
			End If

			If IsNumberMatchingDesc(nationalNumber, metadata.SharedCost) Then
				Return PhoneNumberType.SHARED_COST
			End If

			If IsNumberMatchingDesc(nationalNumber, metadata.Voip) Then
				Return PhoneNumberType.VOIP
			End If

			If IsNumberMatchingDesc(nationalNumber, metadata.PersonalNumber) Then
				Return PhoneNumberType.PERSONAL_NUMBER
			End If

			If IsNumberMatchingDesc(nationalNumber, metadata.Pager) Then
				Return PhoneNumberType.PAGER
			End If

			If IsNumberMatchingDesc(nationalNumber, metadata.Uan) Then
				Return PhoneNumberType.UAN
			End If

			If IsNumberMatchingDesc(nationalNumber, metadata.Voicemail) Then
				Return PhoneNumberType.VOICEMAIL
			End If

			Dim isFixedLine = IsNumberMatchingDesc(nationalNumber, metadata.FixedLine)
			If isFixedLine Then
				If metadata.SameMobileAndFixedLinePattern Then
					Return PhoneNumberType.FIXED_LINE_OR_MOBILE
				ElseIf IsNumberMatchingDesc(nationalNumber, metadata.Mobile) Then
					Return PhoneNumberType.FIXED_LINE_OR_MOBILE
				End If
				Return PhoneNumberType.FIXED_LINE
			End If
			' Otherwise, test to see if the number is mobile. Only do this if certain that the patterns for
			' mobile and fixed line aren't the same.
			If Not metadata.SameMobileAndFixedLinePattern AndAlso IsNumberMatchingDesc(nationalNumber, metadata.Mobile) Then
				Return PhoneNumberType.MOBILE
			End If
			Return PhoneNumberType.UNKNOWN
		End Function

		Public Function GetMetadataForRegion(regionCode As [String]) As PhoneMetadata
			If Not IsValidRegionCode(regionCode) Then
				Return Nothing
			End If
			SyncLock regionToMetadataMap
				If Not regionToMetadataMap.ContainsKey(regionCode) Then
					' The regionCode here will be valid and won't be '001', so we don't need to worry about
					' what to pass in for the country calling code.
					LoadMetadataFromFile(currentFilePrefix_, regionCode, 0)
				End If
			End SyncLock
			Return If(regionToMetadataMap.ContainsKey(regionCode), regionToMetadataMap(regionCode), Nothing)
		End Function

		Public Function GetMetadataForNonGeographicalRegion(countryCallingCode As Integer) As PhoneMetadata
			SyncLock countryCodeToNonGeographicalMetadataMap
				If Not countryCallingCodeToRegionCodeMap_.ContainsKey(countryCallingCode) Then
					Return Nothing
				End If
				If Not countryCodeToNonGeographicalMetadataMap.ContainsKey(countryCallingCode) Then
					LoadMetadataFromFile(currentFilePrefix_, REGION_CODE_FOR_NON_GEO_ENTITY, countryCallingCode)
				End If
			End SyncLock
			Dim metadata As PhoneMetadata = Nothing
			countryCodeToNonGeographicalMetadataMap.TryGetValue(countryCallingCode, metadata)
			Return metadata
		End Function


		Private Function IsNumberMatchingDesc(nationalNumber As [String], numberDesc As PhoneNumberDesc) As Boolean
			Dim possibleNumberPatternMatch = regexCache.GetPatternForRegex(numberDesc.PossibleNumberPattern).MatchAll(nationalNumber)
			Dim nationalNumberPatternMatch = regexCache.GetPatternForRegex(numberDesc.NationalNumberPattern).MatchAll(nationalNumber)
			Return possibleNumberPatternMatch.Success AndAlso nationalNumberPatternMatch.Success
		End Function

		'*
'        * Tests whether a phone number matches a valid pattern. Note this doesn't verify the number
'        * is actually in use, which is impossible to tell by just looking at a number itself.
'        *
'        * @param number       the phone number that we want to validate
'        * @return  a boolean that indicates whether the number is of a valid pattern
'        

		Public Function IsValidNumber(number As PhoneNumber) As Boolean
			Dim regionCode = GetRegionCodeForNumber(number)
			Return IsValidNumberForRegion(number, regionCode)
		End Function

		'*
'        * Tests whether a phone number is valid for a certain region. Note this doesn't verify the number
'        * is actually in use, which is impossible to tell by just looking at a number itself. If the
'        * country calling code is not the same as the country calling code for the region, this
'        * immediately exits with false. After this, the specific number pattern rules for the region are
'        * examined. This is useful for determining for example whether a particular number is valid for
'        * Canada, rather than just a valid NANPA number.
'        *
'        * @param number       the phone number that we want to validate
'        * @param regionCode   the region that we want to validate the phone number for
'        * @return  a boolean that indicates whether the number is of a valid pattern
'        

		Public Function IsValidNumberForRegion(number As PhoneNumber, regionCode As [String]) As Boolean
			Dim countryCode As Integer = number.CountryCode
			Dim metadata As PhoneMetadata = GetMetadataForRegionOrCallingCode(countryCode, regionCode)
			If (metadata Is Nothing) OrElse (Not REGION_CODE_FOR_NON_GEO_ENTITY.Equals(regionCode) AndAlso countryCode <> GetCountryCodeForValidRegion(regionCode)) Then
				' Either the region code was invalid, or the country calling code for this number does not
				' match that of the region code.
				Return False
			End If
			Dim generalNumDesc = metadata.GeneralDesc
			Dim nationalSignificantNumber = GetNationalSignificantNumber(number)

			' For regions where we don't have metadata for PhoneNumberDesc, we treat any number passed in
			' as a valid number if its national significant number is between the minimum and maximum
			' lengths defined by ITU for a national significant number.
			If Not generalNumDesc.HasNationalNumberPattern Then
				Dim numberLength As Integer = nationalSignificantNumber.Length
				Return numberLength > MIN_LENGTH_FOR_NSN AndAlso numberLength <= MAX_LENGTH_FOR_NSN
			End If
			Return GetNumberTypeHelper(nationalSignificantNumber, metadata) <> PhoneNumberType.UNKNOWN
		End Function

		'*
'        * Returns the region where a phone number is from. This could be used for geocoding at the region
'        * level.
'        *
'        * @param number  the phone number whose origin we want to know
'        * @return  the region where the phone number is from, or null if no region matches this calling
'        *     code
'        

		Public Function GetRegionCodeForNumber(number As PhoneNumber) As [String]
			Dim regions As List(Of [String]) = Nothing
			countryCallingCodeToRegionCodeMap_.TryGetValue(number.CountryCode, regions)
			If regions Is Nothing Then
				' String numberString = getNationalSignificantNumber(number);
				' LOGGER.log(Level.WARNING,
				'    "Missing/invalid country_code (" + countryCode + ") for number " + numberString);
				Return Nothing
			End If
			If regions.Count = 1 Then
				Return regions(0)
			End If
			Return GetRegionCodeForNumberFromRegionList(number, regions)
		End Function

		Private Function GetRegionCodeForNumberFromRegionList(number As PhoneNumber, regionCodes As List(Of [String])) As [String]
			Dim nationalNumber As [String] = GetNationalSignificantNumber(number)
            For Each regionCode In regionCodes
                ' If leadingDigits is present, use this. Otherwise, do full validation.
                Dim metadata As Phonemetadata = GetMetadataForRegion(regionCode)
                If metadata.HasLeadingDigits Then
                    If regexCache.GetPatternForRegex(metadata.LeadingDigits).MatchBeginning(nationalNumber).Success Then
                        Return regionCode
                    End If
                ElseIf GetNumberTypeHelper(nationalNumber, metadata) <> PhoneNumberType.UNKNOWN Then
                    Return regionCode
                End If
            Next
			Return Nothing
		End Function

		'*
'        * Returns the region code that matches the specific country calling code. In the case of no
'        * region code being found, ZZ will be returned. In the case of multiple regions, the one
'        * designated in the metadata as the "main" region for this calling code will be returned.
'        

		Public Function GetRegionCodeForCountryCode(countryCallingCode As Integer) As [String]
			Dim regionCodes As List(Of [String]) = Nothing
			Return If(countryCallingCodeToRegionCodeMap_.TryGetValue(countryCallingCode, regionCodes), regionCodes(0), UNKNOWN_REGION)
		End Function

		'*
'        * Returns the country calling code for a specific region. For example, this would be 1 for the
'        * United States, and 64 for New Zealand.
'        *
'        * @param regionCode  region that we want to get the country calling code for
'        * @return  the country calling code for the region denoted by regionCode
'        

		Public Function GetCountryCodeForRegion(regionCode As [String]) As Integer
			If Not IsValidRegionCode(regionCode) Then
				' LOGGER.log(Level.WARNING,
				'    "Invalid or missing region code ("
				'    + ((regionCode == null) ? "null" : regionCode)
				'    + ") provided.");
				Return 0
			End If
			Return GetCountryCodeForValidRegion(regionCode)
		End Function

		'*
'        * Returns the country calling code for a specific region. For example, this would be 1 for the
'        * United States, and 64 for New Zealand. Assumes the region is already valid.
'        *
'        * @param regionCode  the region that we want to get the country calling code for
'        * @return  the country calling code for the region denoted by regionCode
'        

		Private Function GetCountryCodeForValidRegion(regionCode As [String]) As Integer
			Dim metadata As PhoneMetadata = GetMetadataForRegion(regionCode)
			Return metadata.CountryCode
		End Function

		'*
'        * Returns the national dialling prefix for a specific region. For example, this would be 1 for
'        * the United States, and 0 for New Zealand. Set stripNonDigits to true to strip symbols like "~"
'        * (which indicates a wait for a dialling tone) from the prefix returned. If no national prefix is
'        * present, we return null.
'        *
'        * <p>Warning: Do not use this method for do-your-own formatting - for some regions, the
'        * national dialling prefix is used only for certain types of numbers. Use the library's
'        * formatting functions to prefix the national prefix when required.
'        *
'        * @param regionCode  the region that we want to get the dialling prefix for
'        * @param stripNonDigits  true to strip non-digits from the national dialling prefix
'        * @return  the dialling prefix for the region denoted by regionCode
'        

		Public Function GetNddPrefixForRegion(regionCode As [String], stripNonDigits As Boolean) As [String]
			If Not IsValidRegionCode(regionCode) Then
				'LOGGER.log(Level.WARNING,
				'    "Invalid or missing region code ("
				'    + ((regionCode == null) ? "null" : regionCode)
				'    + ") provided.");
				Return Nothing
			End If
			Dim metadata As PhoneMetadata = GetMetadataForRegion(regionCode)
			Dim nationalPrefix As [String] = metadata.NationalPrefix
			' If no national prefix was found, we return null.
			If nationalPrefix.Length = 0 Then
				Return Nothing
			End If
			If stripNonDigits Then
				' Note: if any other non-numeric symbols are ever used in national prefixes, these would have
				' to be removed here as well.
				nationalPrefix = nationalPrefix.Replace("~", "")
			End If
			Return nationalPrefix
		End Function

		'*
'        * Checks if this is a region under the North American Numbering Plan Administration (NANPA).
'        *
'        * @return  true if regionCode is one of the regions under NANPA
'        

		Public Function IsNANPACountry(regionCode As [String]) As Boolean
			Return regionCode IsNot Nothing AndAlso nanpaRegions_.Contains(regionCode)
		End Function

		'*
'        * Checks whether the country calling code is from a region whose national significant number
'        * could contain a leading zero. An example of such a region is Italy. Returns false if no
'        * metadata for the country is found.
'        

		Public Function IsLeadingZeroPossible(countryCallingCode As Integer) As Boolean
			Dim mainMetadataForCallingCode As PhoneMetadata = GetMetadataForRegion(GetRegionCodeForCountryCode(countryCallingCode))
			If mainMetadataForCallingCode Is Nothing Then
				Return False
			End If
			Return mainMetadataForCallingCode.LeadingZeroPossible
		End Function

		'*
'        * Checks if the number is a valid vanity (alpha) number such as 800 MICROSOFT. A valid vanity
'        * number will start with at least 3 digits and will have three or more alpha characters. This
'        * does not do region-specific checks - to work out if this number is actually valid for a region,
'        * it should be parsed and methods such as {@link #isPossibleNumberWithReason} and
'        * {@link #isValidNumber} should be used.
'        *
'        * @param number  the number that needs to be checked
'        * @return  true if the number is a valid vanity number
'        

		Public Function IsAlphaNumber(number As [String]) As Boolean
			If Not IsViablePhoneNumber(number) Then
				' Number is too short, or doesn't match the basic phone number pattern.
				Return False
			End If
			Dim strippedNumber = New StringBuilder(number)
			MaybeStripExtension(strippedNumber)
			Return VALID_ALPHA_PHONE_PATTERN.MatchAll(strippedNumber.ToString()).Success
			'XXX: ToString
		End Function

		'*
'        * Convenience wrapper around {@link #isPossibleNumberWithReason}. Instead of returning the reason
'        * for failure, this method returns a boolean value.
'        * @param number  the number that needs to be checked
'        * @return  true if the number is possible
'        

		Public Function IsPossibleNumber(number As PhoneNumber) As Boolean
			Return IsPossibleNumberWithReason(number) = ValidationResult.IS_POSSIBLE
		End Function

		'*
'        * Helper method to check a number against a particular pattern and determine whether it matches,
'        * or is too short or too long. Currently, if a number pattern suggests that numbers of length 7
'        * and 10 are possible, and a number in between these possible lengths is entered, such as of
'        * length 8, this will return TOO_LONG.
'        

		Private Function TestNumberLengthAgainstPattern(numberPattern As PhoneRegex, number As [String]) As ValidationResult
			If numberPattern.MatchAll(number).Success Then
				Return ValidationResult.IS_POSSIBLE
			End If
			If numberPattern.MatchBeginning(number).Success Then
				Return ValidationResult.TOO_LONG
			End If
			Return ValidationResult.TOO_SHORT
		End Function

		'*
'        * Check whether a phone number is a possible number. It provides a more lenient check than
'        * {@link #isValidNumber} in the following sense:
'        *<ol>
'        * <li> It only checks the length of phone numbers. In particular, it doesn't check starting
'        *      digits of the number.
'        * <li> It doesn't attempt to figure out the type of the number, but uses general rules which
'        *      applies to all types of phone numbers in a region. Therefore, it is much faster than
'        *      isValidNumber.
'        * <li> For fixed line numbers, many regions have the concept of area code, which together with
'        *      subscriber number constitute the national significant number. It is sometimes okay to dial
'        *      the subscriber number only when dialing in the same area. This function will return
'        *      true if the subscriber-number-only version is passed in. On the other hand, because
'        *      isValidNumber validates using information on both starting digits (for fixed line
'        *      numbers, that would most likely be area codes) and length (obviously includes the
'        *      length of area codes for fixed line numbers), it will return false for the
'        *      subscriber-number-only version.
'        * </ol
'        * @param number  the number that needs to be checked
'        * @return  a ValidationResult object which indicates whether the number is possible
'        

		Public Function IsPossibleNumberWithReason(number As PhoneNumber) As ValidationResult
			Dim nationalNumber = GetNationalSignificantNumber(number)
			Dim countryCode As Integer = number.CountryCode
			' Note: For Russian Fed and NANPA numbers, we just use the rules from the default region (US or
			' Russia) since the getRegionCodeForNumber will not work if the number is possible but not
			' valid. This would need to be revisited if the possible number pattern ever differed between
			' various regions within those plans.
			If Not HasValidCountryCallingCode(countryCode) Then
				Return ValidationResult.INVALID_COUNTRY_CODE
			End If
			Dim regionCode As [String] = GetRegionCodeForCountryCode(countryCode)
			Dim metadata As PhoneMetadata = GetMetadataForRegionOrCallingCode(countryCode, regionCode)
			Dim generalNumDesc As PhoneNumberDesc = metadata.GeneralDesc
			' Handling case of numbers with no metadata.
			If Not generalNumDesc.HasNationalNumberPattern Then
				Dim numberLength As Integer = nationalNumber.Length
				If numberLength < MIN_LENGTH_FOR_NSN Then
					Return ValidationResult.TOO_SHORT
				End If
				If numberLength > MAX_LENGTH_FOR_NSN Then
					Return ValidationResult.TOO_LONG
				End If
				Return ValidationResult.IS_POSSIBLE
			End If
			Dim possibleNumberPattern = regexCache.GetPatternForRegex(generalNumDesc.PossibleNumberPattern)
			Return TestNumberLengthAgainstPattern(possibleNumberPattern, nationalNumber)
		End Function

		'*
'        * Check whether a phone number is a possible number given a number in the form of a string, and
'        * the region where the number could be dialed from. It provides a more lenient check than
'        * {@link #isValidNumber}. See {@link #isPossibleNumber(PhoneNumber)} for details.
'        *
'        * <p>This method first parses the number, then invokes {@link #isPossibleNumber(PhoneNumber)}
'        * with the resultant PhoneNumber object.
'        *
'        * @param number  the number that needs to be checked, in the form of a string
'        * @param regionDialingFrom  the region that we are expecting the number to be dialed from.
'        *     Note this is different from the region where the number belongs.  For example, the number
'        *     +1 650 253 0000 is a number that belongs to US. When written in this form, it can be
'        *     dialed from any region. When it is written as 00 1 650 253 0000, it can be dialed from any
'        *     region which uses an international dialling prefix of 00. When it is written as
'        *     650 253 0000, it can only be dialed from within the US, and when written as 253 0000, it
'        *     can only be dialed from within a smaller area in the US (Mountain View, CA, to be more
'        *     specific).
'        * @return  true if the number is possible
'        

		Public Function IsPossibleNumber(number As [String], regionDialingFrom As [String]) As Boolean
			Try
				Return IsPossibleNumber(Parse(number, regionDialingFrom))
			Catch generatedExceptionName As NumberParseException
				Return False
			End Try
		End Function

		'*
'        * Attempts to extract a valid number from a phone number that is too long to be valid, and resets
'        * the PhoneNumber object passed in to that valid version. If no valid number could be extracted,
'        * the PhoneNumber object passed in will not be modified.
'        * @param number a PhoneNumber object which contains a number that is too long to be valid.
'        * @return  true if a valid phone number can be successfully extracted.
'        

		Public Function TruncateTooLongNumber(number As PhoneNumber.Builder) As Boolean
			If IsValidNumber(number.Clone().Build()) Then
				Return True
			End If
			Dim copy As PhoneNumber = Nothing
			Dim nationalNumber As ULong = number.NationalNumber
			Do
				nationalNumber /= 10
				Dim numberCopy As PhoneNumber.Builder = number.Clone()
				numberCopy.SetNationalNumber(nationalNumber)
				copy = numberCopy.Build()
				If IsPossibleNumberWithReason(copy) = ValidationResult.TOO_SHORT OrElse nationalNumber = 0 Then
					Return False
				End If
			Loop While Not IsValidNumber(copy)
			number.SetNationalNumber(nationalNumber)
			Return True
		End Function

		'*
'        * Gets an {@link com.google.i18n.phonenumbers.AsYouTypeFormatter} for the specific region.
'        *
'        * @param regionCode  region where the phone number is being entered
'        *
'        * @return  an {@link com.google.i18n.phonenumbers.AsYouTypeFormatter} object, which can be used
'        *     to format phone numbers in the specific region "as you type"
'        

		Public Function GetAsYouTypeFormatter(regionCode As [String]) As AsYouTypeFormatter
			Return New AsYouTypeFormatter(regionCode)
		End Function

		' Extracts country calling code from fullNumber, returns it and places the remaining number in
		' nationalNumber. It assumes that the leading plus sign or IDD has already been removed. Returns
		' 0 if fullNumber doesn't start with a valid country calling code, and leaves nationalNumber
		' unmodified.
		Friend Function ExtractCountryCode(fullNumber As StringBuilder, nationalNumber As StringBuilder) As Integer
			If (fullNumber.Length = 0) OrElse (fullNumber(0) = "0"C) Then
				' Country codes do not begin with a '0'.
				Return 0
			End If
			Dim potentialCountryCode As Integer
			Dim numberLength As Integer = fullNumber.Length
			Dim i As Integer = 1
			While i <= MAX_LENGTH_COUNTRY_CODE AndAlso i <= numberLength
				potentialCountryCode = Integer.Parse(fullNumber.ToString().Substring(0, i))
				'XXX: ToString
				If countryCallingCodeToRegionCodeMap_.ContainsKey(potentialCountryCode) Then
					nationalNumber.Append(fullNumber.ToString().Substring(i))
					'XXX: ToString
					Return potentialCountryCode
				End If
				i += 1
			End While
			Return 0
		End Function

		'*
'        * Tries to extract a country calling code from a number. This method will return zero if no
'        * country calling code is considered to be present. Country calling codes are extracted in the
'        * following ways:
'        * <ul>
'        *  <li> by stripping the international dialing prefix of the region the person is dialing from,
'        *       if this is present in the number, and looking at the next digits
'        *  <li> by stripping the '+' sign if present and then looking at the next digits
'        *  <li> by comparing the start of the number and the country calling code of the default region.
'        *       If the number is not considered possible for the numbering plan of the default region
'        *       initially, but starts with the country calling code of this region, validation will be
'        *       reattempted after stripping this country calling code. If this number is considered a
'        *       possible number, then the first digits will be considered the country calling code and
'        *       removed as such.
'        * </ul>
'        * It will throw a NumberParseException if the number starts with a '+' but the country calling
'        * code supplied after this does not match that of any known region.
'        *
'        * @param number  non-normalized telephone number that we wish to extract a country calling
'        *     code from - may begin with '+'
'        * @param defaultRegionMetadata  metadata about the region this number may be from
'        * @param nationalNumber  a string buffer to store the national significant number in, in the case
'        *     that a country calling code was extracted. The number is appended to any existing contents.
'        *     If no country calling code was extracted, this will be left unchanged.
'        * @param keepRawInput  true if the country_code_source and preferred_carrier_code fields of
'        *     phoneNumber should be populated.
'        * @param phoneNumber  the PhoneNumber object where the country_code and country_code_source need
'        *     to be populated. Note the country_code is always populated, whereas country_code_source is
'        *     only populated when keepCountryCodeSource is true.
'        * @return  the country calling code extracted or 0 if none could be extracted
'        

		Public Function MaybeExtractCountryCode(number As [String], defaultRegionMetadata As PhoneMetadata, nationalNumber As StringBuilder, keepRawInput As Boolean, phoneNumber As PhoneNumber.Builder) As Integer
			If number.Length = 0 Then
				Return 0
			End If
			Dim fullNumber As New StringBuilder(number)
			' Set the default prefix to be something that will never match.
			Dim possibleCountryIddPrefix As [String] = "NonMatch"
			If defaultRegionMetadata IsNot Nothing Then
				possibleCountryIddPrefix = defaultRegionMetadata.InternationalPrefix
			End If

			Dim countryCodeSource__1 As CountryCodeSource = MaybeStripInternationalPrefixAndNormalize(fullNumber, possibleCountryIddPrefix)
			If keepRawInput Then
				phoneNumber.SetCountryCodeSource(countryCodeSource__1)
			End If
			If countryCodeSource__1 <> CountryCodeSource.FROM_DEFAULT_COUNTRY Then
				If fullNumber.Length <= MIN_LENGTH_FOR_NSN Then
					Throw New NumberParseException(ErrorType.TOO_SHORT_AFTER_IDD, "Phone number had an IDD, but after this was not " + "long enough to be a viable phone number.")
				End If
				Dim potentialCountryCode As Integer = ExtractCountryCode(fullNumber, nationalNumber)
				If potentialCountryCode <> 0 Then
					phoneNumber.SetCountryCode(potentialCountryCode)
					Return potentialCountryCode
				End If

				' If this fails, they must be using a strange country calling code that we don't recognize,
				' or that doesn't exist.
				Throw New NumberParseException(ErrorType.INVALID_COUNTRY_CODE, "Country calling code supplied was not recognised.")
			ElseIf defaultRegionMetadata IsNot Nothing Then
				' Check to see if the number starts with the country calling code for the default region. If
				' so, we remove the country calling code, and do some checks on the validity of the number
				' before and after.
				Dim defaultCountryCode As Integer = defaultRegionMetadata.CountryCode
				Dim defaultCountryCodeString As [String] = defaultCountryCode.ToString()
				Dim normalizedNumber As [String] = fullNumber.ToString()
				If normalizedNumber.StartsWith(defaultCountryCodeString) Then
					Dim potentialNationalNumber As New StringBuilder(normalizedNumber.Substring(defaultCountryCodeString.Length))
					Dim generalDesc As PhoneNumberDesc = defaultRegionMetadata.GeneralDesc
					Dim validNumberPattern = regexCache.GetPatternForRegex(generalDesc.NationalNumberPattern)
						' Don't need the carrier code 
					MaybeStripNationalPrefixAndCarrierCode(potentialNationalNumber, defaultRegionMetadata, Nothing)
					Dim possibleNumberPattern = regexCache.GetPatternForRegex(generalDesc.PossibleNumberPattern)
					' If the number was not valid before but is valid now, or if it was too long before, we
					' consider the number with the country calling code stripped to be a better result and
					' keep that instead.
					'XXX: ToString
					'XXX: ToString
					If (Not validNumberPattern.MatchAll(fullNumber.ToString()).Success AndAlso validNumberPattern.MatchAll(potentialNationalNumber.ToString()).Success) OrElse TestNumberLengthAgainstPattern(possibleNumberPattern, fullNumber.ToString()) = ValidationResult.TOO_LONG Then
						nationalNumber.Append(potentialNationalNumber)
						If keepRawInput Then
							phoneNumber.SetCountryCodeSource(CountryCodeSource.FROM_NUMBER_WITHOUT_PLUS_SIGN)
						End If
						phoneNumber.SetCountryCode(defaultCountryCode)
						Return defaultCountryCode
					End If
				End If
			End If
			' No country calling code present.
			phoneNumber.SetCountryCode(0)
			Return 0
		End Function

		'*
'        * Strips the IDD from the start of the number if present. Helper function used by
'        * maybeStripInternationalPrefixAndNormalize.
'        

		Private Function ParsePrefixAsIdd(iddPattern As PhoneRegex, number As StringBuilder) As Boolean
			Dim m = iddPattern.MatchBeginning(number.ToString())
			If m.Success Then
				Dim matchEnd As Integer = m.Index + m.Length
				' Only strip this if the first digit after the match is not a 0, since country calling codes
				' cannot begin with 0.
				Dim digitMatcher = CAPTURING_DIGIT_PATTERN.Match(number.ToString().Substring(matchEnd))
				'XXX: ToString
				If digitMatcher.Success Then
					Dim normalizedGroup As [String] = NormalizeDigitsOnly(digitMatcher.Groups(1).Value)
					If normalizedGroup = "0" Then
						Return False
					End If
				End If
				number.Remove(0, matchEnd)
				Return True
			End If
			Return False
		End Function

		'*
'        * Strips any international prefix (such as +, 00, 011) present in the number provided, normalizes
'        * the resulting number, and indicates if an international prefix was present.
'        *
'        * @param number  the non-normalized telephone number that we wish to strip any international
'        *     dialing prefix from.
'        * @param possibleIddPrefix  the international direct dialing prefix from the region we
'        *     think this number may be dialed in
'        * @return  the corresponding CountryCodeSource if an international dialing prefix could be
'        *     removed from the number, otherwise CountryCodeSource.FROM_DEFAULT_COUNTRY if the number did
'        *     not seem to be in international format.
'        

		Public Function MaybeStripInternationalPrefixAndNormalize(number As StringBuilder, possibleIddPrefix As [String]) As CountryCodeSource
			If number.Length = 0 Then
				Return CountryCodeSource.FROM_DEFAULT_COUNTRY
			End If
			' Check to see if the number begins with one or more plus signs.
			Dim m = PLUS_CHARS_PATTERN.MatchBeginning(number.ToString())
			'XXX: ToString
			If m.Success Then
				number.Remove(0, m.Index + m.Length)
				' Can now normalize the rest of the number since we've consumed the "+" sign at the start.
				Normalize(number)
				Return CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN
			End If
			' Attempt to parse the first digits as an international prefix.
			Dim iddPattern = regexCache.GetPatternForRegex(possibleIddPrefix)
			Normalize(number)
			Return If(ParsePrefixAsIdd(iddPattern, number), CountryCodeSource.FROM_NUMBER_WITH_IDD, CountryCodeSource.FROM_DEFAULT_COUNTRY)
		End Function

		'*
'        * Strips any national prefix (such as 0, 1) present in the number provided.
'        *
'        * @param number  the normalized telephone number that we wish to strip any national
'        *     dialing prefix from
'        * @param metadata  the metadata for the region that we think this number is from
'        * @param carrierCode  a place to insert the carrier code if one is extracted
'        * @return true if a national prefix or carrier code (or both) could be extracted.
'        

		Public Function MaybeStripNationalPrefixAndCarrierCode(number As StringBuilder, metadata As PhoneMetadata, carrierCode As StringBuilder) As Boolean
			Dim numberLength As Integer = number.Length
			Dim possibleNationalPrefix As [String] = metadata.NationalPrefixForParsing
			If numberLength = 0 OrElse possibleNationalPrefix.Length = 0 Then
				' Early return for numbers of zero length.
				Return False
			End If
			' Attempt to parse the first digits as a national prefix.
			Dim prefixMatcher = regexCache.GetPatternForRegex(possibleNationalPrefix)
			Dim prefixMatch = prefixMatcher.MatchBeginning(number.ToString())
			'XXX: ToString
			If prefixMatch.Success Then
				Dim nationalNumberRule = regexCache.GetPatternForRegex(metadata.GeneralDesc.NationalNumberPattern)
				' Check if the original number is viable.
				Dim isViableOriginalNumber As Boolean = nationalNumberRule.MatchAll(number.ToString()).Success
				' prefixMatcher.group(numOfGroups) == null implies nothing was captured by the capturing
				' groups in possibleNationalPrefix; therefore, no transformation is necessary, and we just
				' remove the national prefix.
				Dim numOfGroups As Integer = prefixMatch.Groups.Count
				Dim transformRule As [String] = metadata.NationalPrefixTransformRule
				If transformRule Is Nothing OrElse transformRule.Length = 0 OrElse Not prefixMatch.Groups(numOfGroups - 1).Success Then
					' If the original number was viable, and the resultant number is not, we return.
					If isViableOriginalNumber AndAlso Not nationalNumberRule.MatchAll(number.ToString().Substring(prefixMatch.Index + prefixMatch.Length)).Success Then
						Return False
					End If
					If carrierCode IsNot Nothing AndAlso numOfGroups > 1 AndAlso prefixMatch.Groups(numOfGroups - 1).Success Then
						carrierCode.Append(prefixMatch.Groups(1).Value)
					End If
					number.Remove(0, prefixMatch.Index + prefixMatch.Length)
					Return True
				Else
					' Check that the resultant number is still viable. If not, return. Check this by copying
					' the string buffer and making the transformation on the copy first.
					Dim transformedNumber As New StringBuilder(prefixMatcher.Replace(number.ToString(), transformRule, 1))
					'XXX: ToString
					If isViableOriginalNumber AndAlso Not nationalNumberRule.MatchAll(transformedNumber.ToString()).Success Then
						Return False
					End If
					If carrierCode IsNot Nothing AndAlso numOfGroups > 2 Then
						carrierCode.Append(prefixMatcher.Match(number.ToString()).Groups(1).Value)
					End If
					number.Length = 0
					number.Append(transformedNumber.ToString())
					Return True
				End If
			End If
			Return False
		End Function

		'*
'        * Strips any extension (as in, the part of the number dialled after the call is connected,
'        * usually indicated with extn, ext, x or similar) from the end of the number, and returns it.
'        *
'        * @param number  the non-normalized telephone number that we wish to strip the extension from
'        * @return        the phone extension
'        

		Private Function MaybeStripExtension(number As StringBuilder) As [String]
			Dim m = EXTN_PATTERN.Match(number.ToString())
			'XXX: ToString
			' If we find a potential extension, and the number preceding this is a viable number, we assume
			' it is an extension.
			If m.Success AndAlso IsViablePhoneNumber(number.ToString().Substring(0, m.Index)) Then
				'XXX: ToString
				' The numbers are captured into groups in the regular expression.
				Dim i As Integer = 1, length As Integer = m.Groups.Count
				While i < length
					If m.Groups(i).Success Then
						' We go through the capturing groups until we find one that captured some digits. If none
						' did, then we will return the empty string.
						Dim extension As [String] = m.Groups(i).Value
						number.Remove(m.Index, number.Length - m.Index)
						Return extension
					End If
					i += 1
				End While
			End If
			Return ""
		End Function

		'*
'        * Checks to see that the region code used is valid, or if it is not valid, that the number to
'        * parse starts with a + symbol so that we can attempt to infer the region from the number.
'        * Returns false if it cannot use the region provided and the region cannot be inferred.
'        

		Private Function CheckRegionForParsing(numberToParse As [String], defaultRegion As [String]) As Boolean
			If Not IsValidRegionCode(defaultRegion) Then
				' If the number is null or empty, we can't infer the region.
				If numberToParse Is Nothing OrElse numberToParse.Length = 0 OrElse Not PLUS_CHARS_PATTERN.MatchBeginning(numberToParse).Success Then
					Return False
				End If
			End If
			Return True
		End Function

		'*
'        * Parses a string and returns it in proto buffer format. This method will throw a
'        * {@link com.google.i18n.phonenumbers.NumberParseException} if the number is not considered to be
'        * a possible number. Note that validation of whether the number is actually a valid number for a
'        * particular region is not performed. This can be done separately with {@link #isValidNumber}.
'        *
'        * @param numberToParse     number that we are attempting to parse. This can contain formatting
'        *                          such as +, ( and -, as well as a phone number extension. It can also
'        *                          be provided in RFC3966 format.
'        * @param defaultRegion     region that we are expecting the number to be from. This is only used
'        *                          if the number being parsed is not written in international format.
'        *                          The country_code for the number in this case would be stored as that
'        *                          of the default region supplied. If the number is guaranteed to
'        *                          start with a '+' followed by the country calling code, then "ZZ" or
'        *                          null can be supplied.
'        * @return                  a phone number proto buffer filled with the parsed number
'        * @throws NumberParseException  if the string is not considered to be a viable phone number or if
'        *                               no default region was supplied and the number is not in
'        *                               international format (does not start with +)
'        

		Public Function Parse(numberToParse As [String], defaultRegion As [String]) As PhoneNumber
			Dim phoneNumber = New PhoneNumber.Builder()
			Parse(numberToParse, defaultRegion, phoneNumber)
			Return phoneNumber.Build()
		End Function

		'*
'        * Same as {@link #parse(String, String)}, but accepts mutable PhoneNumber as a parameter to
'        * decrease object creation when invoked many times.
'        

		Public Sub Parse(numberToParse As [String], defaultRegion As [String], phoneNumber As PhoneNumber.Builder)
			ParseHelper(numberToParse, defaultRegion, False, True, phoneNumber)
		End Sub

		'*
'        * Parses a string and returns it in proto buffer format. This method differs from {@link #parse}
'        * in that it always populates the raw_input field of the protocol buffer with numberToParse as
'        * well as the country_code_source field.
'        *
'        * @param numberToParse     number that we are attempting to parse. This can contain formatting
'        *                          such as +, ( and -, as well as a phone number extension.
'        * @param defaultRegion     region that we are expecting the number to be from. This is only used
'        *                          if the number being parsed is not written in international format.
'        *                          The country calling code for the number in this case would be stored
'        *                          as that of the default region supplied.
'        * @return                  a phone number proto buffer filled with the parsed number
'        * @throws NumberParseException  if the string is not considered to be a viable phone number or if
'        *                               no default region was supplied
'        

		Public Function ParseAndKeepRawInput(numberToParse As [String], defaultRegion As [String]) As PhoneNumber
			Dim phoneNumber = New PhoneNumber.Builder()
			ParseAndKeepRawInput(numberToParse, defaultRegion, phoneNumber)
			Return phoneNumber.Build()
		End Function

		'*
'        * Same as{@link #parseAndKeepRawInput(String, String)}, but accepts a mutable PhoneNumber as
'        * a parameter to decrease object creation when invoked many times.
'        

		Public Sub ParseAndKeepRawInput(numberToParse As [String], defaultRegion As [String], phoneNumber As PhoneNumber.Builder)
			ParseHelper(numberToParse, defaultRegion, True, True, phoneNumber)
		End Sub

		'*
'        * Returns an iterable over all {@link PhoneNumberMatch PhoneNumberMatches} in {@code text}. This
'        * is a shortcut for {@link #findNumbers(CharSequence, String, Leniency, long)
'        * getMatcher(text, defaultRegion, Leniency.VALID, Long.MAX_VALUE)}.
'        *
'        * @param text              the text to search for phone numbers, null for no text
'        * @param defaultRegion     region that we are expecting the number to be from. This is only used
'        *                          if the number being parsed is not written in international format. The
'        *                          country_code for the number in this case would be stored as that of
'        *                          the default region supplied. May be null if only international
'        *                          numbers are expected.
'        

		Public Function FindNumbers(text As [String], defaultRegion As [String]) As IEnumerable(Of PhoneNumberMatch)
			Return FindNumbers(text, defaultRegion, Leniency.VALID, Long.MaxValue)
		End Function

		'*
'        * Returns an iterable over all {@link PhoneNumberMatch PhoneNumberMatches} in {@code text}.
'        *
'        * @param text              the text to search for phone numbers, null for no text
'        * @param defaultRegion     region that we are expecting the number to be from. This is only used
'        *                          if the number being parsed is not written in international format. The
'        *                          country_code for the number in this case would be stored as that of
'        *                          the default region supplied. May be null if only international
'        *                          numbers are expected.
'        * @param leniency          the leniency to use when evaluating candidate phone numbers
'        * @param maxTries          the maximum number of invalid numbers to try before giving up on the
'        *                          text. This is to cover degenerate cases where the text has a lot of
'        *                          false positives in it. Must be {@code >= 0}.
'        

		Public Function FindNumbers(text As [String], defaultRegion As [String], leniency As Leniency, maxTries As Long) As IEnumerable(Of PhoneNumberMatch)
			Return New EnumerableFromConstructor(Of PhoneNumberMatch)(Function() 
			Return New PhoneNumberMatcher(Me, text, defaultRegion, leniency, maxTries)

End Function)
		End Function

		'*
'        * Parses a string and fills up the phoneNumber. This method is the same as the public
'        * parse() method, with the exception that it allows the default region to be null, for use by
'        * isNumberMatch(). checkRegion should be set to false if it is permitted for the default region
'        * to be null or unknown ("ZZ").
'        

		Private Sub ParseHelper(numberToParse As [String], defaultRegion As [String], keepRawInput As Boolean, checkRegion As Boolean, phoneNumber As PhoneNumber.Builder)
			If numberToParse Is Nothing Then
				Throw New NumberParseException(ErrorType.NOT_A_NUMBER, "The phone number supplied was null.")
			ElseIf numberToParse.Length > MAX_INPUT_STRING_LENGTH Then
				Throw New NumberParseException(ErrorType.TOO_LONG, "The string supplied was too long to parse.")
			End If

			Dim nationalNumber As New StringBuilder()
			BuildNationalNumberForParsing(numberToParse, nationalNumber)

			If Not IsViablePhoneNumber(nationalNumber.ToString()) Then
				Throw New NumberParseException(ErrorType.NOT_A_NUMBER, "The string supplied did not seem to be a phone number.")
			End If

			' Check the region supplied is valid, or that the extracted number starts with some sort of +
			' sign so the number's region can be determined.
			If checkRegion AndAlso Not CheckRegionForParsing(nationalNumber.ToString(), defaultRegion) Then
				Throw New NumberParseException(ErrorType.INVALID_COUNTRY_CODE, "Missing or invalid default region.")
			End If

			If keepRawInput Then
				phoneNumber.SetRawInput(numberToParse)
			End If

			' Attempt to parse extension first, since it doesn't require region-specific data and we want
			' to have the non-normalised number here.
			Dim extension As [String] = MaybeStripExtension(nationalNumber)
			If extension.Length > 0 Then
				phoneNumber.SetExtension(extension)
			End If

			Dim regionMetadata As PhoneMetadata = GetMetadataForRegion(defaultRegion)
			' Check to see if the number is given in international format so we know whether this number is
			' from the default region or not.
			Dim normalizedNationalNumber As New StringBuilder()
			Dim countryCode As Integer = 0
			Try
				' TODO: This method should really just take in the string buffer that has already
				' been created, and just remove the prefix, rather than taking in a string and then
				' outputting a string buffer.
				countryCode = MaybeExtractCountryCode(nationalNumber.ToString(), regionMetadata, normalizedNationalNumber, keepRawInput, phoneNumber)
			Catch e As NumberParseException
				Dim m = PLUS_CHARS_PATTERN.MatchBeginning(nationalNumber.ToString())
				If e.ErrorType = ErrorType.INVALID_COUNTRY_CODE AndAlso m.Success Then
					' Strip the plus-char, and try again.
					countryCode = MaybeExtractCountryCode(nationalNumber.ToString().Substring(m.Index + m.Length), regionMetadata, normalizedNationalNumber, keepRawInput, phoneNumber)
					If countryCode = 0 Then
						Throw New NumberParseException(ErrorType.INVALID_COUNTRY_CODE, "Could not interpret numbers after plus-sign.")
					End If
				Else
					Throw New NumberParseException(e.ErrorType, e.Message)
				End If
			End Try
			If countryCode <> 0 Then
				Dim phoneNumberRegion As [String] = GetRegionCodeForCountryCode(countryCode)
				If phoneNumberRegion <> defaultRegion Then
					regionMetadata = GetMetadataForRegionOrCallingCode(countryCode, phoneNumberRegion)
				End If
			Else
				' If no extracted country calling code, use the region supplied instead. The national number
				' is just the normalized version of the number we were given to parse.
				Normalize(nationalNumber)
				normalizedNationalNumber.Append(nationalNumber)
				If defaultRegion IsNot Nothing Then
					countryCode = regionMetadata.CountryCode
					phoneNumber.SetCountryCode(countryCode)
				ElseIf keepRawInput Then
					phoneNumber.ClearCountryCode()
				End If
			End If
			If normalizedNationalNumber.Length < MIN_LENGTH_FOR_NSN Then
				Throw New NumberParseException(ErrorType.TOO_SHORT_NSN, "The string supplied is too short to be a phone number.")
			End If

			If regionMetadata IsNot Nothing Then
				Dim carrierCode As New StringBuilder()
				MaybeStripNationalPrefixAndCarrierCode(normalizedNationalNumber, regionMetadata, carrierCode)
				If keepRawInput Then
					phoneNumber.SetPreferredDomesticCarrierCode(carrierCode.ToString())
				End If
			End If
			Dim lengthOfNationalNumber As Integer = normalizedNationalNumber.Length
			If lengthOfNationalNumber < MIN_LENGTH_FOR_NSN Then
				Throw New NumberParseException(ErrorType.TOO_SHORT_NSN, "The string supplied is too short to be a phone number.")
			End If

			If lengthOfNationalNumber > MAX_LENGTH_FOR_NSN Then
				Throw New NumberParseException(ErrorType.TOO_LONG, "The string supplied is too long to be a phone number.")
			End If

			If normalizedNationalNumber(0) = "0"C Then
				phoneNumber.SetItalianLeadingZero(True)
			End If
			phoneNumber.SetNationalNumber(ULong.Parse(normalizedNationalNumber.ToString()))
		End Sub

		Private Shared Function AreEqual(p1 As PhoneNumber.Builder, p2 As PhoneNumber.Builder) As Boolean
			Return p1.Clone().Build().Equals(p2.Clone().Build())
		End Function

		'*
'        * Converts numberToParse to a form that we can parse and write it to nationalNumber if it is
'        * written in RFC3966; otherwise extract a possible number out of it and write to nationalNumber.
'        

		Private Sub BuildNationalNumberForParsing(numberToParse As [String], nationalNumber As StringBuilder)
			Dim indexOfPhoneContext As Integer = numberToParse.IndexOf(RFC3966_PHONE_CONTEXT)
			If indexOfPhoneContext > 0 Then
				Dim phoneContextStart As Integer = indexOfPhoneContext + RFC3966_PHONE_CONTEXT.Length
				' If the phone context contains a phone number prefix, we need to capture it, whereas domains
				' will be ignored.
				If numberToParse(phoneContextStart) = PLUS_SIGN Then
					' Additional parameters might follow the phone context. If so, we will remove them here
					' because the parameters after phone context are not important for parsing the
					' phone number.
					Dim phoneContextEnd As Integer = numberToParse.IndexOf(";"C, phoneContextStart)
					If phoneContextEnd > 0 Then
						nationalNumber.Append(numberToParse.Substring(phoneContextStart, phoneContextEnd - phoneContextStart))
					Else
						nationalNumber.Append(numberToParse.Substring(phoneContextStart))
					End If
				End If

				' Now append everything between the "tel:" prefix and the phone-context. This should include
				' the national number, an optional extension or isdn-subaddress component.
				Dim indexOfPrefix As Integer = numberToParse.IndexOf(RFC3966_PREFIX) + RFC3966_PREFIX.Length
				nationalNumber.Append(numberToParse.Substring(indexOfPrefix, indexOfPhoneContext - indexOfPrefix))
			Else
				' Extract a possible number from the string passed in (this strips leading characters that
				' could not be the start of a phone number.)
				nationalNumber.Append(ExtractPossibleNumber(numberToParse))
			End If

			' Delete the isdn-subaddress and everything after it if it is present. Note extension won't
			' appear at the same time with isdn-subaddress according to paragraph 5.3 of the RFC3966 spec,
			Dim indexOfIsdn As Integer = nationalNumber.ToString().IndexOf(RFC3966_ISDN_SUBADDRESS)
			If indexOfIsdn > 0 Then
				nationalNumber.Remove(indexOfIsdn, nationalNumber.Length - indexOfIsdn)
			End If
			' If both phone context and isdn-subaddress are absent but other parameters are present, the
			' parameters are left in nationalNumber. This is because we are concerned about deleting
			' content from a potential number string when there is no strong evidence that the number is
			' actually written in RFC3966.
		End Sub

		'*
'        * Takes two phone numbers and compares them for equality.
'        *
'        * <p>Returns EXACT_MATCH if the country_code, NSN, presence of a leading zero for Italian numbers
'        * and any extension present are the same.
'        * Returns NSN_MATCH if either or both has no region specified, and the NSNs and extensions are
'        * the same.
'        * Returns SHORT_NSN_MATCH if either or both has no region specified, or the region specified is
'        * the same, and one NSN could be a shorter version of the other number. This includes the case
'        * where one has an extension specified, and the other does not.
'        * Returns NO_MATCH otherwise.
'        * For example, the numbers +1 345 657 1234 and 657 1234 are a SHORT_NSN_MATCH.
'        * The numbers +1 345 657 1234 and 345 657 are a NO_MATCH.
'        *
'        * @param firstNumberIn  first number to compare
'        * @param secondNumberIn  second number to compare
'        *
'        * @return  NO_MATCH, SHORT_NSN_MATCH, NSN_MATCH or EXACT_MATCH depending on the level of equality
'        *     of the two numbers, described in the method definition.
'        

		Public Function IsNumberMatch(firstNumberIn As PhoneNumber, secondNumberIn As PhoneNumber) As MatchType
			' Make copies of the phone number so that the numbers passed in are not edited.
			Dim firstNumber = New PhoneNumber.Builder()
			firstNumber.MergeFrom(firstNumberIn)
			Dim secondNumber = New PhoneNumber.Builder()
			secondNumber.MergeFrom(secondNumberIn)
			' First clear raw_input, country_code_source and preferred_domestic_carrier_code fields and any
			' empty-string extensions so that we can use the proto-buffer equality method.
			firstNumber.ClearRawInput()
			firstNumber.ClearCountryCodeSource()
			firstNumber.ClearPreferredDomesticCarrierCode()
			secondNumber.ClearRawInput()
			secondNumber.ClearCountryCodeSource()
			secondNumber.ClearPreferredDomesticCarrierCode()
			If firstNumber.HasExtension AndAlso firstNumber.Extension.Length = 0 Then
				firstNumber.ClearExtension()
			End If

			If secondNumber.HasExtension AndAlso secondNumber.Extension.Length = 0 Then
				secondNumber.ClearExtension()
			End If

			' Early exit if both had extensions and these are different.
			If firstNumber.HasExtension AndAlso secondNumber.HasExtension AndAlso Not firstNumber.Extension.Equals(secondNumber.Extension) Then
				Return MatchType.NO_MATCH
			End If

			Dim firstNumberCountryCode As Integer = firstNumber.CountryCode
			Dim secondNumberCountryCode As Integer = secondNumber.CountryCode
			' Both had country_code specified.
			If firstNumberCountryCode <> 0 AndAlso secondNumberCountryCode <> 0 Then
				If AreEqual(firstNumber, secondNumber) Then
					Return MatchType.EXACT_MATCH
				ElseIf firstNumberCountryCode = secondNumberCountryCode AndAlso IsNationalNumberSuffixOfTheOther(firstNumber, secondNumber) Then
					' A SHORT_NSN_MATCH occurs if there is a difference because of the presence or absence of
					' an 'Italian leading zero', the presence or absence of an extension, or one NSN being a
					' shorter variant of the other.
					Return MatchType.SHORT_NSN_MATCH
				End If
				' This is not a match.
				Return MatchType.NO_MATCH
			End If
			' Checks cases where one or both country_code fields were not specified. To make equality
			' checks easier, we first set the country_code fields to be equal.
			firstNumber.SetCountryCode(secondNumberCountryCode)
			' If all else was the same, then this is an NSN_MATCH.
			If AreEqual(firstNumber, secondNumber) Then
				Return MatchType.NSN_MATCH
			End If

			If IsNationalNumberSuffixOfTheOther(firstNumber, secondNumber) Then
				Return MatchType.SHORT_NSN_MATCH
			End If
			Return MatchType.NO_MATCH
		End Function

		' Returns true when one national number is the suffix of the other or both are the same.
		Private Function IsNationalNumberSuffixOfTheOther(firstNumber As PhoneNumber.Builder, secondNumber As PhoneNumber.Builder) As Boolean
			Dim firstNumberNationalNumber As [String] = firstNumber.NationalNumber.ToString()
			Dim secondNumberNationalNumber As [String] = secondNumber.NationalNumber.ToString()
			' Note that endsWith returns true if the numbers are equal.
			Return firstNumberNationalNumber.EndsWith(secondNumberNationalNumber) OrElse secondNumberNationalNumber.EndsWith(firstNumberNationalNumber)
		End Function

		'*
'        * Takes two phone numbers as strings and compares them for equality. This is a convenience
'        * wrapper for {@link #isNumberMatch(PhoneNumber, PhoneNumber)}. No default region is known.
'        *
'        * @param firstNumber  first number to compare. Can contain formatting, and can have country
'        *     calling code specified with + at the start.
'        * @param secondNumber  second number to compare. Can contain formatting, and can have country
'        *     calling code specified with + at the start.
'        * @return  NOT_A_NUMBER, NO_MATCH, SHORT_NSN_MATCH, NSN_MATCH, EXACT_MATCH. See
'        *     {@link #isNumberMatch(PhoneNumber, PhoneNumber)} for more details.
'        

		Public Function IsNumberMatch(firstNumber As [String], secondNumber As [String]) As MatchType
			Try
				Dim firstNumberAsProto As PhoneNumber = Parse(firstNumber, UNKNOWN_REGION)
				Return IsNumberMatch(firstNumberAsProto, secondNumber)
			Catch e As NumberParseException
				If e.ErrorType = ErrorType.INVALID_COUNTRY_CODE Then
					Try
						Dim secondNumberAsProto As PhoneNumber = Parse(secondNumber, UNKNOWN_REGION)
						Return IsNumberMatch(secondNumberAsProto, firstNumber)
					Catch e2 As NumberParseException
						If e2.ErrorType = ErrorType.INVALID_COUNTRY_CODE Then
							Try
								Dim firstNumberProto = New PhoneNumber.Builder()
								Dim secondNumberProto = New PhoneNumber.Builder()
								ParseHelper(firstNumber, Nothing, False, False, firstNumberProto)
								ParseHelper(secondNumber, Nothing, False, False, secondNumberProto)
								Return IsNumberMatch(firstNumberProto.Build(), secondNumberProto.Build())
									' Fall through and return MatchType.NOT_A_NUMBER.
							Catch generatedExceptionName As NumberParseException
							End Try
						End If
					End Try
				End If
			End Try
			' One or more of the phone numbers we are trying to match is not a viable phone number.
			Return MatchType.NOT_A_NUMBER
		End Function

		'*
'        * Takes two phone numbers and compares them for equality. This is a convenience wrapper for
'        * {@link #isNumberMatch(PhoneNumber, PhoneNumber)}. No default region is known.
'        *
'        * @param firstNumber  first number to compare in proto buffer format.
'        * @param secondNumber  second number to compare. Can contain formatting, and can have country
'        *     calling code specified with + at the start.
'        * @return  NOT_A_NUMBER, NO_MATCH, SHORT_NSN_MATCH, NSN_MATCH, EXACT_MATCH. See
'        *     {@link #isNumberMatch(PhoneNumber, PhoneNumber)} for more details.
'        

		Public Function IsNumberMatch(firstNumber As PhoneNumber, secondNumber As [String]) As MatchType
			' First see if the second number has an implicit country calling code, by attempting to parse
			' it.
			Try
				Dim secondNumberAsProto As PhoneNumber = Parse(secondNumber, UNKNOWN_REGION)
				Return IsNumberMatch(firstNumber, secondNumberAsProto)
			Catch e As NumberParseException
				If e.ErrorType = ErrorType.INVALID_COUNTRY_CODE Then
					' The second number has no country calling code. EXACT_MATCH is no longer possible.
					' We parse it as if the region was the same as that for the first number, and if
					' EXACT_MATCH is returned, we replace this with NSN_MATCH.
					Dim firstNumberRegion As [String] = GetRegionCodeForCountryCode(firstNumber.CountryCode)
					Try
						If Not firstNumberRegion.Equals(UNKNOWN_REGION) Then
							Dim secondNumberWithFirstNumberRegion As PhoneNumber = Parse(secondNumber, firstNumberRegion)
							Dim match As MatchType = IsNumberMatch(firstNumber, secondNumberWithFirstNumberRegion)
							If match = MatchType.EXACT_MATCH Then
								Return MatchType.NSN_MATCH
							End If
							Return match
						Else
							' If the first number didn't have a valid country calling code, then we parse the
							' second number without one as well.
							Dim secondNumberProto = New PhoneNumber.Builder()
							ParseHelper(secondNumber, Nothing, False, False, secondNumberProto)
							Return IsNumberMatch(firstNumber, secondNumberProto.Build())
						End If
							' Fall-through to return NOT_A_NUMBER.
					Catch generatedExceptionName As NumberParseException
					End Try
				End If
			End Try
			' One or more of the phone numbers we are trying to match is not a viable phone number.
			Return MatchType.NOT_A_NUMBER
		End Function

		'*
'        * Returns true if the number can be dialled from outside the region, or unknown. If the number
'        * can only be dialled from within the region, returns false. Does not check the number is a valid
'        * number.
'        * TODO: Make this method public when we have enough metadata to make it worthwhile.
'        *
'        * @param number  the phone-number for which we want to know whether it is only diallable from
'        *     outside the region
'        

		Public Function CanBeInternationallyDialled(number As PhoneNumber) As Boolean
			Dim regionCode As [String] = GetRegionCodeForNumber(number)
			If Not IsValidRegionCode(regionCode) Then
				' Note numbers belonging to non-geographical entities (e.g. +800 numbers) are always
				' internationally diallable, and will be caught here.
				Return True
			End If
			Dim metadata As PhoneMetadata = GetMetadataForRegion(regionCode)
			Dim nationalSignificantNumber As [String] = GetNationalSignificantNumber(number)
			Return Not IsNumberMatchingDesc(nationalSignificantNumber, metadata.NoInternationalDialling)
		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
