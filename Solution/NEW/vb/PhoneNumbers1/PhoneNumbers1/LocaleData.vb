
' Locale information.
' Holds a map from ISO 3166-1 country code (e.g. GB) to a dict.
' Each dict maps from an ISO 639-1 language code (e.g. ja) to the country's name in that language.
'
' Generated from java.util.Locale, generation info:
' java.version=1.6.0_26
' java.vendor=Sun Microsystems Inc.
' os.name=Windows XP
' os.arch=x86
' os.version=5.1
'

Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.Serialization.Json
'
Namespace PhoneNumbers
	Public Class LocaleData
		Private Shared _Data As Dictionary(Of [String], Dictionary(Of [String], [String])) = Nothing

		Public Shared ReadOnly Property Data() As Dictionary(Of [String], Dictionary(Of [String], [String]))
			Get
				If LocaleData._Data Is Nothing Then
					LocaleData._Data = New Dictionary(Of String, Dictionary(Of String, String))()
					Dim asm = GetType(LocaleData).GetTypeInfo().Assembly
					Dim name = If(asm.GetManifestResourceNames().Where(Function(n) n.EndsWith("localedata")).FirstOrDefault(), "missing")
					Using ms As Stream = asm.GetManifestResourceStream(name)
						Dim serializer = New DataContractJsonSerializer(LocaleData._Data.[GetType]())
						ms.Position = 0
						LocaleData._Data = DirectCast(serializer.ReadObject(ms), Dictionary(Of [String], Dictionary(Of [String], [String])))
					End Using
				End If
				Return LocaleData._Data
			End Get
		End Property
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
