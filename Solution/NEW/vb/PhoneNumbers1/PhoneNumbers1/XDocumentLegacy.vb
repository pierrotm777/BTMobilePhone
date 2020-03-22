
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Xml.Linq

Namespace libphonenumber_csharp_portable
	NotInheritable Class XDocumentLegacy
		Private Sub New()
		End Sub
		<System.Runtime.CompilerServices.Extension> _
		Friend Shared Function GetElementsByTagName(document As XDocument, tagName As String) As IEnumerable(Of XElement)
			Return (From d In document.Descendants() Where d.Name.LocalName = tagNamed)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Friend Shared Function GetElementsByTagName(document As XElement, tagName As String) As IEnumerable(Of XElement)
			Return (From d In document.Descendants() Where d.Name.LocalName = tagNamed)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Friend Shared Function HasAttribute(element As XElement, attribute As String) As Boolean
			Return (element.Attribute(attribute) IsNot Nothing)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Friend Shared Function GetAttribute(element As XElement, attribute As String) As String
			Dim value As String

			If element.HasAttribute(attribute) Then
				value = element.Attribute(attribute).Value
			Else
				value = ""
			End If

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
