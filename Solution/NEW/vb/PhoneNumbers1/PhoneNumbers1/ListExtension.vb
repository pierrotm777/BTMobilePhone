
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace libphonenumber_csharp_portable
	Friend NotInheritable Class ListExtension
		Private Sub New()
		End Sub
		<System.Runtime.CompilerServices.Extension> _
		Friend Shared Function ConvertAll(Of TOutput)(list As List(Of Char), converter As Func(Of Char, TOutput)) As List(Of TOutput)
			Dim returnedList As New List(Of TOutput)(list.Count)
			For Each character As Char In list
				returnedList.Add(converter(character))
			Next
			Return returnedList
		End Function
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
