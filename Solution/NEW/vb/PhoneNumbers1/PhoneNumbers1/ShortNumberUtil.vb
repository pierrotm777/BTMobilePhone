
'
'* Copyright (C) 2011 The Libphonenumber Authors
'*
'* Licensed under the Apache License, Version 2.0 (the "License");
'* you may not use this file except in compliance with the License.
'* You may obtain a copy of the License at
'*
'* http://www.apache.org/licenses/LICENSE-2.0
'*
'* Unless required by applicable law or agreed to in writing, software
'* distributed under the License is distributed on an "AS IS" BASIS,
'* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'* See the License for the specific language governing permissions and
'* limitations under the License.
'

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace PhoneNumbers
	'
'    * Utility for international short phone numbers, such as short codes and emergency numbers. Note
'    * most commercial short numbers are not handled here, but by the PhoneNumberUtil.
'    *
'    * @author Shaopeng Jia
'    

	Public Class ShortNumberUtil
		Private Shared phoneUtil As PhoneNumberUtil

		Public Sub New()
			phoneUtil = PhoneNumberUtil.GetInstance()
		End Sub

		' @VisibleForTesting
		Public Sub New(util As PhoneNumberUtil)
			phoneUtil = util
		End Sub

		'*
'        * Returns true if the number might be used to connect to an emergency service in the given
'        * region.
'        *
'        * This method takes into account cases where the number might contain formatting, or might have
'        * additional digits appended (when it is okay to do that in the region specified).
'        *
'        * @param number  the phone number to test
'        * @param regionCode  the region where the phone number is being dialed
'        * @return  if the number might be used to connect to an emergency service in the given region.
'        

		Public Function ConnectsToEmergencyNumber(number As [String], regionCode As [String]) As Boolean
				' allows prefix match 
			Return MatchesEmergencyNumberHelper(number, regionCode, True)
		End Function

		'*
'        * Returns true if the number exactly matches an emergency service number in the given region.
'        *
'        * This method takes into account cases where the number might contain formatting, but doesn't
'        * allow additional digits to be appended.
'        *
'        * @param number  the phone number to test
'        * @param regionCode  the region where the phone number is being dialed
'        * @return  if the number exactly matches an emergency services number in the given region.
'        

		Public Function IsEmergencyNumber(number As [String], regionCode As [String]) As Boolean
				' doesn't allow prefix match 
			Return MatchesEmergencyNumberHelper(number, regionCode, False)
		End Function

		Private Function MatchesEmergencyNumberHelper(number As [String], regionCode As [String], allowPrefixMatch As Boolean) As Boolean
			number = PhoneNumberUtil.ExtractPossibleNumber(number)
			If PhoneNumberUtil.PLUS_CHARS_PATTERN.MatchBeginning(number).Success Then
				' Returns false if the number starts with a plus sign. We don't believe dialing the country
				' code before emergency numbers (e.g. +1911) works, but later, if that proves to work, we can
				' add additional logic here to handle it.
				Return False
			End If
			Dim metadata As PhoneMetadata = phoneUtil.GetMetadataForRegion(regionCode)
			If metadata Is Nothing OrElse Not metadata.HasEmergency Then
				Return False
			End If
			Dim emergencyNumberPattern = New PhoneRegex(metadata.Emergency.NationalNumberPattern)
			Dim normalizedNumber As [String] = PhoneNumberUtil.NormalizeDigitsOnly(number)
			' In Brazil, it is impossible to append additional digits to an emergency number to dial the
			' number.
			Return If((Not allowPrefixMatch OrElse regionCode.Equals("BR")), emergencyNumberPattern.MatchAll(normalizedNumber).Success, emergencyNumberPattern.MatchBeginning(normalizedNumber).Success)

		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
