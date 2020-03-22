
Imports System.IO
Imports System.Collections.Generic
Imports System.Text

Namespace LumiSoft.Net.Mime.vCard
	''' <summary>
	''' RFC 2426 vCard implementation.
	''' </summary>
	Public Class vCard
		Private m_pCharset As Encoding = Nothing
		Private m_pItems As ItemCollection = Nothing

		Private m_pPhoneNumbers As PhoneNumberCollection = Nothing


		''' <summary>
		''' Default constructor.
		''' </summary>
		Public Sub New()
            m_pCharset = Encoding.UTF8
			m_pItems = New ItemCollection(Me)
            Me.Version = "3.0" 'voir https://en.wikipedia.org/wiki/VCard#vCard_3.0
			Me.UID = Guid.NewGuid().ToString()
		End Sub


		#Region "method ToByte"

		''' <summary>
		''' Stores vCard structure to byte[].
		''' </summary>
		''' <returns></returns>
		Public Function ToByte() As Byte()
			Dim ms As New MemoryStream()
			ToStream(ms)
			Return ms.ToArray()
		End Function

		#End Region

		#Region "method ToFile"

		''' <summary>
		''' Stores vCard to the specified file.
		''' </summary>
        ''' <param name="file__1">File name with path where to store vCard.</param>
		Public Sub ToFile(file__1 As String)
			Using fs As FileStream = File.Create(file__1)
				ToStream(fs)
			End Using
		End Sub

		#End Region

		#Region "method ToStream"

		''' <summary>
		''' Stores vCard structure to the specified stream.
		''' </summary>
		''' <param name="stream">Stream where to store vCard structure.</param>
		Public Sub ToStream(stream As Stream)
			' 
'                BEGIN:VCARD<CRLF>
'                ....
'                END:VCARD<CRLF>
'            


			Dim retVal As New StringBuilder()
			retVal.Append("BEGIN:VCARD" & vbCr & vbLf)
			For Each item As Item In m_pItems
				retVal.Append(item.ToItemString() + vbCr & vbLf)
			Next
			retVal.Append("END:VCARD" & vbCr & vbLf)

			Dim data As Byte() = m_pCharset.GetBytes(retVal.ToString())
			stream.Write(data, 0, data.Length)
		End Sub

		#End Region


		#Region "static method ParseMultiple"

		''' <summary>
		''' Parses multiple vCards from the specified file (Apple Address Book and Gmail exports)
		''' </summary>
        ''' <param name="file__1">vCard file with path.</param>
		Public Shared Function ParseMultiple(file__1 As String) As List(Of vCard)
			Dim vCards As New List(Of vCard)()
			Dim fileStrings As New List(Of String)()
			Dim line As String = ""
			Dim hasBeginTag As Boolean = False
			Using fs As FileStream = File.OpenRead(file__1)
                Dim r As TextReader = New StreamReader(fs, System.Text.Encoding.[Default])
				While line IsNot Nothing
					line = r.ReadLine()
					If line IsNot Nothing AndAlso line.ToUpper() = "BEGIN:VCARD" Then
						hasBeginTag = True
					End If
					If hasBeginTag Then
						fileStrings.Add(line)
						If line IsNot Nothing AndAlso line.ToUpper() = "END:VCARD" Then
							' on END line process the Vcard, reinitialize, and will repeat the same thing for any others.
							Dim singleVcard As New vCard()
							singleVcard.ParseStrings(fileStrings)
							vCards.Add(singleVcard)
							fileStrings.Clear()
							hasBeginTag = False
						End If
					End If
				End While
			End Using
			Return vCards
		End Function

		#End Region

		#Region "method Parse"

		''' <summary>
		''' Parses single vCard from the specified file.
		''' </summary>
        ''' <param name="file__1">vCard file with path.</param>
		Public Sub Parse(file__1 As String)
			Dim fileStrings As New List(Of String)()
			Dim fileStringArray As String() = File.ReadAllLines(file__1, System.Text.Encoding.[Default])
			For Each fileString As String In fileStringArray
				fileStrings.Add(fileString)
			Next
			ParseStrings(fileStrings)
		End Sub

		''' <summary>
		''' Parses single vCard from the specified stream.
		''' </summary>
		''' <param name="stream">Stream what contains vCard.</param>
		Public Sub Parse(stream As FileStream)
			Dim fileStrings As New List(Of String)()
			Dim line As String = ""
			Dim r As TextReader = New StreamReader(stream, System.Text.Encoding.[Default])
			While line IsNot Nothing
				line = r.ReadLine()
				fileStrings.Add(line)
			End While
			ParseStrings(fileStrings)
		End Sub

		''' <summary>
		''' Parses single vCard from the specified stream.
		''' </summary>
		''' <param name="stream">Stream what contains vCard.</param>
		Public Sub Parse(stream As Stream)
			Dim fileStrings As New List(Of String)()
			Dim line As String = ""
			Dim r As TextReader = New StreamReader(stream, System.Text.Encoding.[Default])
			While line IsNot Nothing
				line = r.ReadLine()
				fileStrings.Add(line)
			End While
			ParseStrings(fileStrings)
		End Sub

		''' <summary>
		''' Parses vCard from the specified stream.
		''' </summary>
		''' <param name="fileStrings">List of strings that contains vCard.</param>
		Public Sub ParseStrings(fileStrings As List(Of String))
			m_pItems.Clear()
			m_pPhoneNumbers = Nothing


			Dim lineCount As Integer = 0
			Dim line As String = fileStrings(lineCount)
			' Find row BEGIN:VCARD
			While line IsNot Nothing AndAlso line.ToUpper() <> "BEGIN:VCARD"
				line = fileStrings(System.Math.Max(System.Threading.Interlocked.Increment(lineCount),lineCount - 1))
			End While
			' Read first vCard line after BEGIN:VCARD
			line = fileStrings(System.Math.Max(System.Threading.Interlocked.Increment(lineCount),lineCount - 1))
			While line IsNot Nothing AndAlso line.ToUpper() <> "END:VCARD"
				Dim item As New StringBuilder()
				item.Append(line)
				' Get next line, see if item continues (folded line).
				line = fileStrings(System.Math.Max(System.Threading.Interlocked.Increment(lineCount),lineCount - 1))
				While line IsNot Nothing AndAlso (line.StartsWith(vbTab) OrElse line.StartsWith(" "))
					item.Append(line.Substring(1))
					line = fileStrings(System.Math.Max(System.Threading.Interlocked.Increment(lineCount),lineCount - 1))
				End While

				Dim name_value As String() = item.ToString().Split(New Char() {":"C}, 2)

				' Item syntax: name[*(;parameter)]:value
				Dim name_params As String() = name_value(0).Split(New Char() {";"C}, 2)
				Dim name As String = name_params(0)
				Dim parameters As String = ""
				If name_params.Length = 2 Then
					parameters = name_params(1)
				End If
				Dim value As String = ""
				If name_value.Length = 2 Then
					value = name_value(1)
				End If
				m_pItems.Add(name, parameters, value)
			End While
		End Sub

		#End Region


		#Region "Properties Implementation"

		''' <summary>
		''' Gets or sets charset used by vcard. NOTE: Since version 3.0, the only allowed value is UTF-8.
		''' </summary>
		Public Property Charset() As Encoding
			Get
				Return m_pCharset
			End Get

			Set
				If value Is Nothing Then
					Throw New ArgumentNullException("value")
				End If

				m_pCharset = value
			End Set
		End Property

		''' <summary>
		''' Gets reference to vCard items.
		''' </summary>
		Public ReadOnly Property Items() As ItemCollection
			Get
				Return m_pItems
			End Get
		End Property

		''' <summary>
		''' Gets or sets vCard version. Returns null if VERSION: item doesn't exist.
		''' </summary>
		Public Property Version() As String
			Get
				Dim item As Item = m_pItems.GetFirst("VERSION")
				If item IsNot Nothing Then
                    Return item.Value
				Else
					Return Nothing
				End If
			End Get

			Set
				m_pItems.SetValue("VERSION", value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets name info.  Returns null if N: item doesn't exist.
		''' </summary>
		Public Property Name() As Name
			Get
				Dim item As Item = m_pItems.GetFirst("N")
				If item IsNot Nothing Then
					Return Name.Parse(item)
				Else
					Return Nothing
				End If
			End Get

			Set
				If value IsNot Nothing Then
					m_pItems.SetDecodedValue("N", value.ToValueString())
				Else
					m_pItems.SetDecodedValue("N", Nothing)
				End If
			End Set
		End Property

		''' <summary>
		''' Gets or sets formatted(Display name) name.  Returns null if FN: item doesn't exist.
		''' </summary>
		Public Property FormattedName() As String
			Get
				Dim item As Item = m_pItems.GetFirst("FN")
				If item IsNot Nothing Then
                    Return item.Value
				Else
					Return Nothing
				End If
			End Get

			Set
				m_pItems.SetDecodedValue("FN", value)
			End Set
		End Property

        Public Property CallType() As String
            Get
                Dim item As Item = m_pItems.GetFirst("X-IRMC-CALL-DATETIME")
                If item IsNot Nothing Then
                    Return item.ParametersString
                Else
                    Return Nothing
                End If
            End Get

            Set(value As String)

            End Set
        End Property

        Public Property CallDateTime() As String
            Get
                Dim item As Item = m_pItems.GetFirst("X-IRMC-CALL-DATETIME")
                If item IsNot Nothing Then
                    Dim d As String = item.Value.Substring(0, 8) + item.Value.Substring(9, 6)


                    d = d.Insert(4, "-")
                    d = d.Insert(7, "-")
                    d = d.Insert(10, " ")
                    d = d.Insert(13, ":")
                    d = d.Insert(16, ":")


                    Return d
                Else
                    Return Nothing
                End If
            End Get

            Set(value As String)

            End Set
        End Property

        Public Property PhoneType() As String
            Get
                Dim item As Item = m_pItems.GetFirst("TEL")
                If item IsNot Nothing Then
                    Return item.ParametersString
                Else
                    Return Nothing
                End If
            End Get

            Set(value As String)

            End Set
        End Property

        Public Property CallHistoryNum() As String
            Get
                Dim item As Item = m_pItems.GetFirst("TEL")
                If item IsNot Nothing Then
                    Return item.Value
                Else
                    Return Nothing
                End If
            End Get

            Set(value As String)

            End Set
        End Property

		''' <summary>
		''' Gets or sets nick name. Returns null if NICKNAME: item doesn't exist.
		''' </summary>
		Public Property NickName() As String
			Get
				Dim item As Item = m_pItems.GetFirst("NICKNAME")
				If item IsNot Nothing Then
                    Return item.Value
				Else
					Return Nothing
				End If
			End Get

			Set
				m_pItems.SetDecodedValue("NICKNAME", value)
			End Set
		End Property


		''' <summary>
		''' Gets or sets birth date. Returns DateTime.MinValue if not set.
		''' </summary>
		Public Property BirthDate() As DateTime
			Get
				Dim item As Item = m_pItems.GetFirst("BDAY")
				If item IsNot Nothing Then
                    Dim [date] As String = item.Value.Replace("-", "")
					Dim dateFormats As String() = New String() {"yyyyMMdd", "yyyyMMddz"}
					Return DateTime.ParseExact([date], dateFormats, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None)
				Else
					Return DateTime.MinValue
				End If
			End Get

			Set
				If value <> DateTime.MinValue Then
					m_pItems.SetValue("BDAY", value.ToString("yyyyMMdd"))
				Else
					m_pItems.SetValue("BDAY", Nothing)
				End If
			End Set
		End Property




		''' <summary>
		''' Gets phone number collection.
		''' </summary>
		Public ReadOnly Property PhoneNumbers() As PhoneNumberCollection
			Get
				' Delay collection creation, create it when needed.
				If m_pPhoneNumbers Is Nothing Then
					m_pPhoneNumbers = New PhoneNumberCollection(Me)
				End If

				Return m_pPhoneNumbers
			End Get
		End Property



		''' <summary>
		''' Gets or sets job title. Returns null if TITLE: item doesn't exist.
		''' </summary>
		Public Property Title() As String
			Get
				Dim item As Item = m_pItems.GetFirst("TITLE")
				If item IsNot Nothing Then
                    Return item.Value
				Else
					Return Nothing
				End If
			End Get

			Set
				m_pItems.SetDecodedValue("TITLE", value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets role. Returns null if ROLE: item doesn't exist.
		''' </summary>
		Public Property Role() As String
			Get
				Dim item As Item = m_pItems.GetFirst("ROLE")
				If item IsNot Nothing Then
                    Return item.Value
				Else
					Return Nothing
				End If
			End Get

			Set
				m_pItems.SetDecodedValue("ROLE", value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets organization name. Usually this value is: comapny;department;office. Returns null if ORG: item doesn't exist.
		''' </summary>
		Public Property Organization() As String
			Get
				Dim item As Item = m_pItems.GetFirst("ORG")
				If item IsNot Nothing Then
                    Return item.Value
				Else
					Return Nothing
				End If
			End Get

			Set
				m_pItems.SetDecodedValue("ORG", value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets note text. Returns null if NOTE: item doesn't exist.
		''' </summary>
		Public Property NoteText() As String
			Get
				Dim item As Item = m_pItems.GetFirst("NOTE")
				If item IsNot Nothing Then
                    Return item.Value
				Else
					Return Nothing
				End If
			End Get

			Set
				m_pItems.SetDecodedValue("NOTE", value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets vCard unique ID. Returns null if UID: item doesn't exist.
		''' </summary>
		Public Property UID() As String
			Get
				Dim item As Item = m_pItems.GetFirst("UID")
				If item IsNot Nothing Then
                    Return item.Value
				Else
					Return Nothing
				End If
			End Get

			Set
				m_pItems.SetDecodedValue("UID", value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets vCard home URL.
		''' </summary>
		Public Property HomeURL() As String
			Get
				Dim items As Item() = m_pItems.[Get]("URL")
				For Each item As Item In items
					If item.ParametersString = "" OrElse item.ParametersString.ToUpper().IndexOf("HOME") > -1 Then
                        Return item.Value
					End If
				Next

				Return Nothing
			End Get

			Set
				Dim items As Item() = m_pItems.[Get]("URL")
				For Each item As Item In items
					If item.ParametersString.ToUpper().IndexOf("HOME") > -1 Then
						If value IsNot Nothing Then
							item.Value = value
						Else
							m_pItems.Remove(item)
						End If
						Return
					End If
				Next

				If value IsNot Nothing Then
					' If we reach here, URL;Work  doesn't exist, add it.
					m_pItems.Add("URL", "HOME", value)
				End If
			End Set
		End Property

		''' <summary>
		''' Gets or sets vCard Work URL.
		''' </summary>
		Public Property WorkURL() As String
			Get
				Dim items As Item() = m_pItems.[Get]("URL")
				For Each item As Item In items
					If item.ParametersString.ToUpper().IndexOf("WORK") > -1 Then
                        Return item.Value
					End If
				Next

				Return Nothing
			End Get

			Set
				Dim items As Item() = m_pItems.[Get]("URL")
				For Each item As Item In items
					If item.ParametersString.ToUpper().IndexOf("WORK") > -1 Then
						If value IsNot Nothing Then
							item.Value = value
						Else
							m_pItems.Remove(item)
						End If
						Return
					End If
				Next

				If value IsNot Nothing Then
					' If we reach here, URL;Work  doesn't exist, add it.
					m_pItems.Add("URL", "WORK", value)
				End If
			End Set
		End Property

		#End Region

	End Class
End Namespace