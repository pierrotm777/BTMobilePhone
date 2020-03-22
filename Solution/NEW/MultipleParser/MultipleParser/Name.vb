
Imports System.Collections.Generic
Imports System.Text

Namespace LumiSoft.Net.Mime.vCard
	''' <summary>
	''' vCard name implementation.
	''' </summary>
	Public Class Name
		Private m_LastName As String = ""
		Private m_FirstName As String = ""
		Private m_AdditionalNames As String = ""
		Private m_HonorificPrefix As String = ""
		Private m_HonorificSuffix As String = ""

		''' <summary>
		''' Default constructor.
		''' </summary>
		''' <param name="lastName">Last name.</param>
		''' <param name="firstName">First name.</param>
		''' <param name="additionalNames">Comma separated additional names.</param>
		''' <param name="honorificPrefix">Honorific prefix.</param>
		''' <param name="honorificSuffix">Honorific suffix.</param>
		Public Sub New(lastName As String, firstName As String, additionalNames As String, honorificPrefix As String, honorificSuffix As String)
			m_LastName = lastName
			m_FirstName = firstName
			m_AdditionalNames = additionalNames
			m_HonorificPrefix = honorificPrefix
			m_HonorificSuffix = honorificSuffix
		End Sub

		''' <summary>
		''' Internal parse constructor.
		''' </summary>
		Friend Sub New()
		End Sub


		#Region "method ToValueString"

		''' <summary>
		''' Converts item to vCard N structure string.
		''' </summary>
		''' <returns></returns>
		Public Function ToValueString() As String
			Return Convert.ToString((Convert.ToString((Convert.ToString((Convert.ToString(m_LastName & Convert.ToString(";")) & m_FirstName) + ";") & m_AdditionalNames) + ";") & m_HonorificPrefix) + ";") & m_HonorificSuffix
		End Function

		#End Region


		#Region "internal static method Parse"

		''' <summary>
		''' Parses name info from vCard N item.
		''' </summary>
		''' <param name="item">vCard N item.</param>
		Friend Shared Function Parse(item As Item) As Name
            Dim items As String() = item.Value.Split(";"c)
			Dim name As New Name()
			If items.Length >= 1 Then
				name.m_LastName = items(0)
			End If
			If items.Length >= 2 Then
				name.m_FirstName = items(1)
			End If
			If items.Length >= 3 Then
				name.m_AdditionalNames = items(2)
			End If
			If items.Length >= 4 Then
				name.m_HonorificPrefix = items(3)
			End If
			If items.Length >= 5 Then
				name.m_HonorificSuffix = items(4)
			End If
			Return name
		End Function

		#End Region


		#Region "Properties Implementation"

		''' <summary>
		''' Gets last name.
		''' </summary>
		Public ReadOnly Property LastName() As String
			Get
				Return m_LastName
			End Get
		End Property

		''' <summary>
		''' Gets first name.
		''' </summary>
		Public ReadOnly Property FirstName() As String
			Get
				Return m_FirstName
			End Get
		End Property

		''' <summary>
		''' Gets comma separated additional names.
		''' </summary>
		Public ReadOnly Property AdditionalNames() As String
			Get
				Return m_AdditionalNames
			End Get
		End Property

		''' <summary>
		''' Gets honorific prefix.
		''' </summary>
		Public ReadOnly Property HonorificPerfix() As String
			Get
				Return m_HonorificPrefix
			End Get
		End Property

		''' <summary>
		''' Gets honorific suffix.
		''' </summary>
		Public ReadOnly Property HonorificSuffix() As String
			Get
				Return m_HonorificSuffix
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
