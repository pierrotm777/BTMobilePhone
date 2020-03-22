
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text

Namespace LumiSoft.Net.Mime.vCard
	''' <summary>
	''' vCard item collection.
	''' </summary>
	Public Class ItemCollection
        'Implements IEnumerable
		Private m_pCard As vCard = Nothing
		Private m_pItems As List(Of Item) = Nothing

		''' <summary>
		''' Default constructor.
		''' </summary>
		''' <param name="card">Owner card.</param>
		Friend Sub New(card As vCard)
			m_pCard = card

			m_pItems = New List(Of Item)()
		End Sub


		#Region "method Add"

		''' <summary>
		''' Adds new vCard item to the collection.
		''' </summary>
		''' <param name="name">Item name.</param>
		''' <param name="parametes">Item parameters.</param>
		''' <param name="value">Item value.</param>
		Public Function Add(name As String, parametes As String, value As String) As Item
			Dim item As New Item(m_pCard, name, parametes, value)
			m_pItems.Add(item)

			Return item
		End Function

		#End Region

		#Region "method Remove"

		''' <summary>
		''' Removes all items with the specified name.
		''' </summary>
		''' <param name="name">Item name.</param>
		Public Sub Remove(name As String)
			For i As Integer = 0 To m_pItems.Count - 1
				If m_pItems(i).Name.ToLower() = name.ToLower() Then
					m_pItems.RemoveAt(i)
					i -= 1
				End If
			Next
		End Sub

		''' <summary>
		''' Removes specified item from the collection.
		''' </summary>
		''' <param name="item">Item to remove.</param>
		Public Sub Remove(item As Item)
			m_pItems.Remove(item)
		End Sub

		#End Region

		#Region "method Clear"

		''' <summary>
		''' Clears all items in the collection.
		''' </summary>
		Public Sub Clear()
			m_pItems.Clear()
		End Sub

		#End Region


		#Region "method GetFirst"

		''' <summary>
		''' Gets first item with specified name. Returns null if specified item doesn't exists.
		''' </summary>
		''' <param name="name">Item name. Name compare is case-insensitive.</param>
		''' <returns>Returns first item with specified name or null if specified item doesn't exists.</returns>
		Public Function GetFirst(name As String) As Item
			For Each item As Item In m_pItems
				If item.Name.ToLower() = name.ToLower() Then
					Return item
				End If
			Next

			Return Nothing
		End Function

		#End Region

		#Region "method Get"

		''' <summary>
		''' Gets items with specified name.
		''' </summary>
		''' <param name="name">Item name.</param>
		''' <returns></returns>
		Public Function [Get](name As String) As Item()
			Dim retVal As New List(Of Item)()
			For Each item As Item In m_pItems
				If item.Name.ToLower() = name.ToLower() Then
					retVal.Add(item)
				End If
			Next

			Return retVal.ToArray()
		End Function

		#End Region

		#Region "method SetDecodedStringValue"

		''' <summary>
		''' Sets first item with specified value.  If item doesn't exist, item will be appended to the end.
		''' If value is null, all items with specified name will be removed.
		''' Value is encoed as needed and specified item.ParametersString will be updated accordingly.
		''' </summary>
		''' <param name="name">Item name.</param>
		''' <param name="value">Item value.</param>
		Public Sub SetDecodedValue(name As String, value As String)
			If value Is Nothing Then
				Remove(name)
				Return
			End If

			Dim item As Item = GetFirst(name)
			If item IsNot Nothing Then
				item.SetDecodedValue(value)
			Else
				item = New Item(m_pCard, name, "", "")
				m_pItems.Add(item)
				item.SetDecodedValue(value)
			End If
		End Sub

		#End Region

		#Region "method SetValue"

		''' <summary>
		''' Sets first item with specified encoded value.  If item doesn't exist, item will be appended to the end.
		''' If value is null, all items with specified name will be removed.
		''' </summary>
		''' <param name="name">Item name.</param>
		''' <param name="value">Item encoded value.</param>
		Public Sub SetValue(name As String, value As String)
			SetValue(name, "", value)
		End Sub

		''' <summary>
		''' Sets first item with specified name encoded value.  If item doesn't exist, item will be appended to the end.
		''' If value is null, all items with specified name will be removed.
		''' </summary>
		''' <param name="name">Item name.</param>
		''' <param name="parametes">Item parameters.</param>
		''' <param name="value">Item encoded value.</param>
		Public Sub SetValue(name As String, parametes As String, value As String)
			If value Is Nothing Then
				Remove(name)
				Return
			End If

			Dim item As Item = GetFirst(name)
			If item IsNot Nothing Then
				item.Value = value
			Else
				m_pItems.Add(New Item(m_pCard, name, parametes, value))
			End If
		End Sub

		#End Region


		#Region "interface IEnumerator"

		''' <summary>
		''' Gets enumerator.
		''' </summary>
		''' <returns></returns>
		Public Function GetEnumerator() As IEnumerator
			Return m_pItems.GetEnumerator()
		End Function

		#End Region

		#Region "Properties Implementation"

		''' <summary>
		''' Gets number of vCard items in the collection.
		''' </summary>
		Public ReadOnly Property Count() As Integer
			Get
				Return m_pItems.Count
			End Get
		End Property

		#End Region

	End Class
End Namespace

