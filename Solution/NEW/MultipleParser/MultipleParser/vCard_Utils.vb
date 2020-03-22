
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace LumiSoft.Net.Mime.vCard
	''' <summary>
	''' Provides vCard related methods.
	''' </summary>
	Friend Class vCard_Utils
		#Region "static method Encode"

		''' <summary>
		''' Encodes vcard value.
		''' </summary>
		''' <param name="version">VCard version.</param>
		''' <param name="value">Value to encode.</param>
		''' <returns>Returns encoded value.</returns>
		Public Shared Function Encode(version As String, value As String) As String
            Return Encode(version, Encoding.UTF8, value)
		End Function

		''' <summary>
		''' Encodes vcard value.
		''' </summary>
		''' <param name="version">VCard version.</param>
		''' <param name="charset">Charset used to encode.</param>
		''' <param name="value">Value to encode.</param>
		''' <returns>Returns encoded value.</returns>
		Public Shared Function Encode(version As String, charset As Encoding, value As String) As String
			If charset Is Nothing Then
				Throw New ArgumentNullException("charset")
			End If

			If version.StartsWith("3") Then
				' We need to escape CR LF COMMA SEMICOLON
				'value = value.Replace("\r","").Replace("\n","\\n").Replace(",","\\,").Replace(";","\\;");
                value = value.Replace(vbCr, "").Replace(vbLf, "\n").Replace(",", "\,").Replace(";", "\;")
			Else
                'value = QPEncode(charset.GetBytes(value))
			End If

			Return value
		End Function

		#End Region


	End Class
End Namespace

