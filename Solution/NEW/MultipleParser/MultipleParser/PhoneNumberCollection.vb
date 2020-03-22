
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text

Namespace LumiSoft.Net.Mime.vCard
	''' <summary>
	''' vCard phone number collection implementation.
	''' </summary>
	Public Class PhoneNumberCollection
        'Implements IEnumerable
		Private m_pOwner As vCard = Nothing
		Private m_pCollection As List(Of PhoneNumber) = Nothing

		''' <summary>
		''' Default constructor.
		''' </summary>
		''' <param name="owner">Owner vCard.</param>
		Friend Sub New(owner As vCard)
			m_pOwner = owner
			m_pCollection = New List(Of PhoneNumber)()

            For Each item As Item In owner.Items.[Get]("TEL")
                m_pCollection.Add(PhoneNumber.Parse(item))
            Next
		End Sub


		#Region "method Add"

		''' <summary>
		''' Add new phone number to the collection.
		''' </summary>
		''' <param name="type">Phone number type. Note: This value can be flagged value !</param>
		''' <param name="number">Phone number.</param>
        Public Sub Add(type As PhoneNumberType_enum, number As String)
            Dim item As Item = m_pOwner.Items.Add("TEL", PhoneNumber.PhoneTypeToString(type), number)
            m_pCollection.Add(New PhoneNumber(item, type, number))
        End Sub

		#End Region

		#Region "method Remove"

		''' <summary>
		''' Removes specified item from the collection.
		''' </summary>
		''' <param name="item">Item to remove.</param>
		Public Sub Remove(item As PhoneNumber)
			m_pOwner.Items.Remove(item.Item)
			m_pCollection.Remove(item)
		End Sub

		#End Region

		#Region "method Clear"

		''' <summary>
		''' Removes all items from the collection.
		''' </summary>
		Public Sub Clear()
			For Each number As PhoneNumber In m_pCollection
				m_pOwner.Items.Remove(number.Item)
			Next
			m_pCollection.Clear()
		End Sub

		#End Region


		#Region "interface IEnumerator"

		''' <summary>
		''' Gets enumerator.
		''' </summary>
		''' <returns></returns>
		Public Function GetEnumerator() As IEnumerator
			Return m_pCollection.GetEnumerator()
		End Function

		#End Region

		#Region "Properties Implementation"

		''' <summary>
		''' Gets number of items in the collection.
		''' </summary>
		Public ReadOnly Property Count() As Integer
			Get
				Return m_pCollection.Count
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
