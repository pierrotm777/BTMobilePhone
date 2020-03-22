Imports System.Globalization
Imports System.Text

''' <summary>
'''     A formatter that will apply a format to a string of numeric values.
''' </summary>
''' <example>
'''     The following example converts a string of numbers and inserts dashes between them.
'''     <code>
''' public class Example
''' {
'''      public static void Main()
'''      {          
'''          string stringValue = "123456789";
'''  
'''          Console.WriteLine(String.Format(new NumericStringFormatter(),
'''                                          "{0} (formatted: {0:###-##-####})",stringValue));
'''      }
'''  }
'''  //  The example displays the following output:
'''  //      123456789 (formatted: 123-45-6789)
'''  </code> 
''' </example>
Public Class NumericStringFormatter : Implements IFormatProvider, ICustomFormatter
    'Implements IFormatProvider
    'Implements ICustomFormatter
    ''' <summary>
    '''     Converts the value of a specified object to an equivalent string representation using specified format and
    '''     culture-specific formatting information.
    ''' </summary>
    ''' <param name="format">A format string containing formatting specifications.</param>
    ''' <param name="arg">An object to format.</param>
    ''' <param name="formatProvider">An object that supplies format information about the current instance.</param>
    ''' <returns>
    '''     The string representation of the value of <paramref name="arg" />, formatted as specified by
    '''     <paramref name="format" /> and <paramref name="formatProvider" />.
    ''' </returns>
    ''' <exception cref="System.NotImplementedException"></exception>
    Public Function Format(format__1 As String, arg As Object, formatProvider As IFormatProvider) As String Implements ICustomFormatter.Format
        Dim strArg = TryCast(arg, String)

        '  If the arg is not a string then determine if it can be handled by another formatt
        If strArg Is Nothing Then
            Try
                Return HandleOtherFormats(format__1, arg)
            Catch e As FormatException
                Throw New FormatException(String.Format("The format of '{0}' is invalid.", format__1), e)
            End Try
        End If

        ' If the format is not set then determine if it can be handled by another formatter
        If String.IsNullOrEmpty(format__1) Then
            Try
                Return HandleOtherFormats(format__1, arg)
            Catch e As FormatException
                Throw New FormatException(String.Format("The format of '{0}' is invalid.", format__1), e)
            End Try
        End If
        Dim sb = New StringBuilder()
        Dim i = 0

        For Each c As String In format__1
            If c = "#"c Then
                If i < strArg.Length Then
                    sb.Append(strArg(i))
                End If
                i += 1
            Else
                sb.Append(c)
            End If
        Next

        Return sb.ToString()
    End Function

    ''' <summary>
    '''     Returns an object that provides formatting services for the specified type.
    ''' </summary>
    ''' <param name="formatType">An object that specifies the type of format object to return.</param>
    ''' <returns>
    '''     An instance of the object specified by <paramref name="formatType" />, if the
    '''     <see cref="T:System.IFormatProvider" /> implementation can supply that type of object; otherwise, null.
    ''' </returns>
    Public Function GetFormat(formatType As Type) As Object Implements IFormatProvider.GetFormat
        ' Determine whether custom formatting object is requested. 
        Return If(formatType Is GetType(ICustomFormatter), Me, Nothing)
    End Function

    Private Function HandleOtherFormats(format As String, arg As Object) As String
        If TypeOf arg Is IFormattable Then
            Return DirectCast(arg, IFormattable).ToString(format, CultureInfo.CurrentCulture)
        ElseIf arg IsNot Nothing Then
            Return arg.ToString()
        Else
            Return String.Empty
        End If
    End Function
End Class
