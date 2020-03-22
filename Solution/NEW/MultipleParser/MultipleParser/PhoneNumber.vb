
Imports System.Collections.Generic
Imports System.Text

Namespace LumiSoft.Net.Mime.vCard
	''' <summary>
	''' vCard phone number implementation.
	''' </summary>
	Public Class PhoneNumber
		Private m_pItem As Item = Nothing
		Private m_Type As PhoneNumberType_enum = PhoneNumberType_enum.Voice
		Private m_Number As String = ""

		''' <summary>
		''' Default constructor.
		''' </summary>
		''' <param name="item">Owner vCard item.</param>
		''' <param name="type">Phone number type. Note: This value can be flagged value !</param>
		''' <param name="number">Phone number.</param>
		Friend Sub New(item As Item, type As PhoneNumberType_enum, number As String)
			m_pItem = item
			m_Type = type
			m_Number = number
		End Sub


		#Region "method Changed"

		''' <summary>
		''' This method is called when some property has changed, wee need to update underlaying vCard item.
		''' </summary>
		Private Sub Changed()
			m_pItem.ParametersString = PhoneTypeToString(m_Type)
			m_pItem.Value = m_Number
		End Sub

		#End Region


		#Region "internal static method Parse"

		''' <summary>
		''' Parses phone from vCard TEL structure string.
		''' </summary>
		''' <param name="item">vCard TEL item.</param>
		Friend Shared Function Parse(item As Item) As PhoneNumber
			Dim type As PhoneNumberType_enum = PhoneNumberType_enum.NotSpecified
			If item.ParametersString.ToUpper().IndexOf("PREF") <> -1 Then
				type = type Or PhoneNumberType_enum.Preferred
			End If
			If item.ParametersString.ToUpper().IndexOf("HOME") <> -1 Then
				type = type Or PhoneNumberType_enum.Home
			End If
			If item.ParametersString.ToUpper().IndexOf("MSG") <> -1 Then
				type = type Or PhoneNumberType_enum.Msg
			End If
			If item.ParametersString.ToUpper().IndexOf("WORK") <> -1 Then
				type = type Or PhoneNumberType_enum.Work
			End If
			If item.ParametersString.ToUpper().IndexOf("VOICE") <> -1 Then
				type = type Or PhoneNumberType_enum.Voice
			End If
			If item.ParametersString.ToUpper().IndexOf("FAX") <> -1 Then
				type = type Or PhoneNumberType_enum.Fax
			End If
			If item.ParametersString.ToUpper().IndexOf("CELL") <> -1 Then
				type = type Or PhoneNumberType_enum.Cellular
			End If
			If item.ParametersString.ToUpper().IndexOf("VIDEO") <> -1 Then
				type = type Or PhoneNumberType_enum.Video
			End If
			If item.ParametersString.ToUpper().IndexOf("PAGER") <> -1 Then
				type = type Or PhoneNumberType_enum.Pager
			End If
			If item.ParametersString.ToUpper().IndexOf("BBS") <> -1 Then
				type = type Or PhoneNumberType_enum.BBS
			End If
			If item.ParametersString.ToUpper().IndexOf("MODEM") <> -1 Then
				type = type Or PhoneNumberType_enum.Modem
			End If
			If item.ParametersString.ToUpper().IndexOf("CAR") <> -1 Then
				type = type Or PhoneNumberType_enum.Car
			End If
			If item.ParametersString.ToUpper().IndexOf("ISDN") <> -1 Then
				type = type Or PhoneNumberType_enum.ISDN
			End If
			If item.ParametersString.ToUpper().IndexOf("PCS") <> -1 Then
				type = type Or PhoneNumberType_enum.PCS
			End If

            'Dim tmpNum As String = item.Value
            'If item.Value.Length = 10 Then
            '    tmpNum = tmpNum.Insert(3, "-")
            '    tmpNum = tmpNum.Insert(7, "-")
            'ElseIf tmpNum.Length = 11 Then
            '    tmpNum = tmpNum.Insert(1, "-")
            '    tmpNum = tmpNum.Insert(5, "-")
            '    tmpNum = tmpNum.Insert(9, "-")
            'End If
            'Return New PhoneNumber(item, type, tmpNum)
            Return New PhoneNumber(item, type, item.Value)
        End Function

		#End Region

		#Region "internal static PhoneTypeToString"

		''' <summary>
		''' Converts PhoneNumberType_enum to vCard item parameters string.
		''' </summary>
		''' <param name="type">Value to convert.</param>
		''' <returns></returns>
		Friend Shared Function PhoneTypeToString(type As PhoneNumberType_enum) As String
			Dim retVal As String = ""
			If (type And PhoneNumberType_enum.BBS) <> 0 Then
				retVal += "BBS,"
			End If
			If (type And PhoneNumberType_enum.Car) <> 0 Then
				retVal += "CAR,"
			End If
			If (type And PhoneNumberType_enum.Cellular) <> 0 Then
				retVal += "CELL,"
			End If
			If (type And PhoneNumberType_enum.Fax) <> 0 Then
				retVal += "FAX,"
			End If
			If (type And PhoneNumberType_enum.Home) <> 0 Then
				retVal += "HOME,"
			End If
			If (type And PhoneNumberType_enum.ISDN) <> 0 Then
				retVal += "ISDN,"
			End If
			If (type And PhoneNumberType_enum.Modem) <> 0 Then
				retVal += "MODEM,"
			End If
			If (type And PhoneNumberType_enum.Msg) <> 0 Then
				retVal += "MSG,"
			End If
			If (type And PhoneNumberType_enum.Pager) <> 0 Then
				retVal += "PAGER,"
			End If
			If (type And PhoneNumberType_enum.PCS) <> 0 Then
				retVal += "PCS,"
			End If
			If (type And PhoneNumberType_enum.Preferred) <> 0 Then
				retVal += "PREF,"
			End If
			If (type And PhoneNumberType_enum.Video) <> 0 Then
				retVal += "VIDEO,"
			End If
			If (type And PhoneNumberType_enum.Voice) <> 0 Then
				retVal += "VOICE,"
			End If
			If (type And PhoneNumberType_enum.Work) <> 0 Then
				retVal += "WORK,"
			End If
			If retVal.EndsWith(",") Then
				retVal = retVal.Substring(0, retVal.Length - 1)
			End If

			Return retVal
		End Function

		#End Region


		#Region "Properties Implementation"

		''' <summary>
		''' Gets underlaying vCrad item.
		''' </summary>
		Public ReadOnly Property Item() As Item
			Get
				Return m_pItem
			End Get
		End Property

		''' <summary>
		''' Gets or sets phone number type. Note: This property can be flagged value !
		''' </summary>
		Public Property NumberType() As PhoneNumberType_enum
			Get
				Return m_Type
			End Get

			Set
				m_Type = value
				Changed()
			End Set
		End Property

		''' <summary>
		''' Gets or sets phone number.
		''' </summary>
		Public Property Number() As String
			Get
				Return m_Number
			End Get

			Set
				m_Number = value
				Changed()
			End Set
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
