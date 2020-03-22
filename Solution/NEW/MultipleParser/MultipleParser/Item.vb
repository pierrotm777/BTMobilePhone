
Imports System.Collections.Generic
Imports System.Text


Namespace LumiSoft.Net.Mime.vCard




    '=======================================================
    'Service provided by Telerik (www.telerik.com)
    'Conversion powered by NRefactory.
    'Twitter: @telerik
    'Facebook: facebook.com/telerik
    '=======================================================

    ''' <summary>
    ''' vCard structure item.
    ''' </summary>
    Public Class Item


#Region "static method IsAscii"

        ''' <summary>
        ''' Gets if the specified string is ASCII string.
        ''' </summary>
        ''' <param name="value">String value.</param>
        ''' <returns>Returns true if specified string is ASCII string, otherwise false.</returns>
        ''' <exception cref="ArgumentNullException">Is raised when <b>value</b> is null reference.</exception>
        Public Shared Function IsAscii(value As String) As Boolean
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            For Each c As Char In value
                If CInt(Val(c)) > 127 Then
                    Return False
                End If
            Next

            Return True
        End Function

#End Region
        Private m_pCard As vCard = Nothing
        Private m_Name As String = ""
        Private m_Parameters As String = ""
        Private m_Value As String = ""
        Private m_FoldData As Boolean = True

        ''' <summary>
        ''' Default constructor.
        ''' </summary>
        ''' <param name="card">Owner card.</param>
        ''' <param name="name">Item name.</param>
        ''' <param name="parameters">Item parameters.</param>
        ''' <param name="value">Item encoded value value.</param>
        Friend Sub New(card As vCard, name As String, parameters As String, value As String)
            m_pCard = card
            m_Name = name
            m_Parameters = parameters
            m_Value = value
        End Sub


#Region "method SetDecodedValue"

        ''' <summary>
        ''' Sets item decoded value. Value will be encoded as needed and stored to item.Value property.
        ''' </summary>
        ''' <param name="value"></param>
        Public Sub SetDecodedValue(value As String)
            ' RFC 2426 vCrad 5. Differences From vCard v2.1
            '                The QUOTED-PRINTABLE inline encoding has been eliminated.
            '                Only the "B" encoding of [RFC 2047] is an allowed value for
            '                the ENCODING parameter.
            '              
            '                The CRLF character sequence in a text type value is specified 
            '                with the backslash character sequence "\n" or "\N".
            '             
            '                Any COMMA or SEMICOLON in a text type value must be backslash escaped.
            '            


            ' Remove encoding and charset parameters
            Dim newParmString As String = ""
            Dim parameters As String() = m_Parameters.ToLower().Split(";"c)
            For Each parameter As String In parameters
                Dim name_value As String() = parameter.Split("="c)
                If name_value(0) = "encoding" OrElse name_value(0) = "charset" Then
                ElseIf parameter.Length > 0 Then
                    newParmString += parameter & Convert.ToString(";")
                End If
            Next

            If m_pCard.Version.StartsWith("3") Then
                ' Add encoding parameter
                If Not IsAscii(value) Then
                    newParmString += "CHARSET=utf-8"
                End If

                Me.ParametersString = newParmString
                Me.Value = vCard_Utils.Encode(m_pCard.Version, m_pCard.Charset, value)
            Else
                If NeedEncode(value) Then
                    ' Add encoding parameter
                    newParmString += "ENCODING=QUOTED-PRINTABLE;CHARSET=" + m_pCard.Charset.WebName

                    Me.ParametersString = newParmString
                    Me.Value = vCard_Utils.Encode(m_pCard.Version, m_pCard.Charset, value)
                Else
                    Me.ParametersString = newParmString
                    Me.Value = value
                End If
            End If
        End Sub

#End Region


#Region "internal method ToItemString"

        ''' <summary>
        ''' Converts item to vCal item string.
        ''' </summary>
        ''' <returns></returns>
        Friend Function ToItemString() As String
            Dim value As String = m_Value
            If m_FoldData Then
                value = FoldData(value)
            End If

            If m_Parameters.Length > 0 Then
                Return Convert.ToString((Convert.ToString(m_Name & Convert.ToString(";")) & m_Parameters) + ":") & value
            Else
                Return Convert.ToString(m_Name & Convert.ToString(":")) & value
            End If
        End Function

#End Region


#Region "method NeedEncode"

        ''' <summary>
        ''' CHecks if specified value must be encoded.
        ''' </summary>
        ''' <param name="value">String value.</param>
        ''' <returns>Returns true if value must be encoded, otherwise false.</returns>
        Private Function NeedEncode(value As String) As Boolean
            ' We have 8-bit chars.
            If Not IsAscii(value) Then
                Return True
            End If

            ' Allow only printable chars and whitespaces.
            For Each c As Char In value
                If Char.IsControl(c) Then
                    Return True
                End If
            Next

            Return False
        End Function

#End Region


#Region "static method FoldData"

        ''' <summary>
        ''' Folds long data line to folded lines.
        ''' </summary>
        ''' <param name="data">Data to fold.</param>
        ''' <returns></returns>
        Private Function FoldData(data As String) As String
            ' Folding rules:
            '                *) Line may not be bigger than 76 chars.
            '                *) If possible fold between TAB or SP
            '                *) If no fold point, just fold from char 76
            '            


            ' Data line too big, we need to fold data.
            If data.Length > 76 Then
                Dim startPosition As Integer = 0
                Dim lastPossibleFoldPos As Integer = -1
                Dim retVal As New StringBuilder()
                For i As Integer = 0 To data.Length - 1
                    Dim c As Char = data(i)
                    ' We have possible fold point
                    If c = " "c OrElse c = ControlChars.Tab Then
                        lastPossibleFoldPos = i
                    End If

                    ' End of data reached
                    If i = (data.Length - 1) Then
                        retVal.Append(data.Substring(startPosition))
                        ' We need to fold
                    ElseIf (i - startPosition) >= 76 Then
                        ' There wasn't any good fold point(word is bigger than line), just fold from current position.
                        If lastPossibleFoldPos = -1 Then
                            lastPossibleFoldPos = i
                        End If

                        retVal.Append(data.Substring(startPosition, lastPossibleFoldPos - startPosition) + vbCr & vbLf & vbTab)

                        i = lastPossibleFoldPos
                        lastPossibleFoldPos = -1
                        startPosition = i
                    End If
                Next

                Return retVal.ToString()
            Else
                Return data
            End If
        End Function

#End Region


#Region "Properties Implementation"

        ''' <summary>
        ''' Gest item name.
        ''' </summary>
        Public ReadOnly Property Name() As String
            Get
                Return m_Name
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets item parameters.
        ''' </summary>
        Public Property ParametersString() As String
            Get
                Return m_Parameters
            End Get

            Set(value As String)
                m_Parameters = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets item encoded value. NOTE: If you set this property value, you must encode data 
        ''' by yourself and also set right ENCODING=encoding; and CHARSET=charset; prameter in item.ParametersString !!!
        ''' Normally use method item.SetDecodedStringValue method instead, this does all you need.
        ''' </summary>
        Public Property Value() As String
            Get
                Return m_Value
            End Get

            Set(value As String)
                m_Value = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets if long data lines are foolded.
        ''' </summary>
        Public Property FoldLongLines() As Boolean
            Get
                Return m_FoldData
            End Get

            Set(value As Boolean)
                m_FoldData = value
            End Set
        End Property


        ''' <summary>
        ''' Gets owner.
        ''' </summary>        
        Friend ReadOnly Property Owner() As vCard
            Get
                Return m_pCard
            End Get
        End Property

#End Region

    End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
