
'
' * Copyright (C) 2012 The Libphonenumber Authors
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
Imports System.Reflection

Namespace PhoneNumbers
	'*
'    * Class encapsulating loading of PhoneNumber Metadata information. Currently this is used only for
'    * additional data files such as PhoneNumberAlternateFormats, but in the future it is envisaged it
'    * would handle the main metadata file (PhoneNumberMetaData.xml) as well.
'    *
'    * @author Lara Rennie
'    

	Public Class MetadataManager
		Friend Const ALTERNATE_FORMATS_FILE_PREFIX As [String] = "PhoneNumberAlternateFormats.xml"

		Private Shared ReadOnly callingCodeToAlternateFormatsMap As New Dictionary(Of Integer, PhoneMetadata)()

		' A set of which country calling codes there are alternate format data for. If the set has an
		' entry for a code, then there should be data for that code linked into the resources.
		Private Shared ReadOnly countryCodeSet As Dictionary(Of Integer, List(Of [String])) = BuildMetadataFromXml.GetCountryCodeToRegionCodeMap(ALTERNATE_FORMATS_FILE_PREFIX)

		Private Sub New()
		End Sub

		Private Shared Sub LoadMedataFromFile(filePrefix As [String])
			Dim asm = GetType(MetadataManager).GetTypeInfo().Assembly
			Dim name = If(asm.GetManifestResourceNames().Where(Function(n) n.EndsWith(filePrefix)).FirstOrDefault(), "missing")
			Using stream = asm.GetManifestResourceStream(name)
				Dim meta = BuildMetadataFromXml.BuildPhoneMetadataCollection(stream, False)
				For Each m As var In meta.MetadataList
					callingCodeToAlternateFormatsMap(m.CountryCode) = m
				Next
			End Using
		End Sub

		Public Shared Function GetAlternateFormatsForCountry(countryCallingCode As Integer) As PhoneMetadata
			SyncLock callingCodeToAlternateFormatsMap
				If Not countryCodeSet.ContainsKey(countryCallingCode) Then
					Return Nothing
				End If
				If Not callingCodeToAlternateFormatsMap.ContainsKey(countryCallingCode) Then
					LoadMedataFromFile(ALTERNATE_FORMATS_FILE_PREFIX)
				End If
				Return If(callingCodeToAlternateFormatsMap.ContainsKey(countryCallingCode), callingCodeToAlternateFormatsMap(countryCallingCode), Nothing)
			End SyncLock
		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
