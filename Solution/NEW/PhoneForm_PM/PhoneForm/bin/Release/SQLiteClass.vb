Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SQLite
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms

Namespace Simple_Disk_Catalog
	Public Class SQLiteDatabase
		Private DBConnection As [String]

		Private ReadOnly _sqLiteTransaction As SQLiteTransaction

		Private ReadOnly _sqLiteConnection As SQLiteConnection

		Private ReadOnly _transaction As Boolean

		''' <summary>
		'''     Default Constructor for SQLiteDatabase Class.
		''' </summary>
		''' <param name="transaction">Allow programmers to insert, update and delete values in one transaction</param>
		Public Sub New(Optional transaction As Boolean = False)
			_transaction = transaction
			DBConnection = "Data Source=recipes.s3db"
			If transaction Then
				_sqLiteConnection = New SQLiteConnection(DBConnection)
				_sqLiteConnection.Open()
				_sqLiteTransaction = _sqLiteConnection.BeginTransaction()
			End If
		End Sub

		''' <summary>
		'''     Single Param Constructor for specifying the DB file.
		''' </summary>
		''' <param name="inputFile">The File containing the DB</param>
		Public Sub New(inputFile As [String])
			DBConnection = [String].Format("Data Source={0}", inputFile)
		End Sub

		''' <summary>
		'''     Commit transaction to the database.
		''' </summary>
		Public Sub CommitTransaction()
			_sqLiteTransaction.Commit()
			_sqLiteTransaction.Dispose()
			_sqLiteConnection.Close()
			_sqLiteConnection.Dispose()
		End Sub

		''' <summary>
		'''     Single Param Constructor for specifying advanced connection options.
		''' </summary>
		''' <param name="connectionOpts">A dictionary containing all desired options and their values</param>
		Public Sub New(connectionOpts As Dictionary(Of [String], [String]))
			Dim str As [String] = connectionOpts.Aggregate("", Function(current, row) current + [String].Format("{0}={1}; ", row.Key, row.Value))
			str = str.Trim().Substring(0, str.Length - 1)
			DBConnection = str
		End Sub

		''' <summary>
		'''     Allows the programmer to create new database file.
		''' </summary>
		''' <param name="filePath">Full path of a new database file.</param>
		''' <returns>true or false to represent success or failure.</returns>
		Public Shared Function CreateDB(filePath As String) As Boolean
			Try
				SQLiteConnection.CreateFile(filePath)
				Return True
			Catch e As Exception
				MessageBox.Show(e.Message, e.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
				Return False
			End Try
		End Function

		''' <summary>
		'''     Allows the programmer to run a query against the Database.
		''' </summary>
		''' <param name="sql">The SQL to run</param>
		''' <param name="allowDBNullColumns">Allow null value for columns in this collection.</param>
		''' <returns>A DataTable containing the result set.</returns>
		Public Function GetDataTable(sql As String, Optional allowDBNullColumns As IEnumerable(Of String) = Nothing) As DataTable
			Dim dt = New DataTable()
			If allowDBNullColumns IsNot Nothing Then
				For Each s As var In allowDBNullColumns
					dt.Columns.Add(s)
					dt.Columns(s).AllowDBNull = True
				Next
			End If
			Try
				Dim cnn = New SQLiteConnection(DBConnection)
				cnn.Open()
				Dim mycommand = New SQLiteCommand(cnn) With { _
					Key .CommandText = sql _
				}
				Dim reader = mycommand.ExecuteReader()
				dt.Load(reader)
				reader.Close()
				cnn.Close()
			Catch e As Exception
				Throw New Exception(e.Message)
			End Try
			Return dt
		End Function

		Public Function RetrieveOriginal(value As String) As String
			Return value.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", "<").Replace("&quot;", """").Replace("&apos;", "'")
		End Function

		''' <summary>
		'''     Allows the programmer to interact with the database for purposes other than a query.
		''' </summary>
		''' <param name="sql">The SQL to be run.</param>
		''' <returns>An Integer containing the number of rows updated.</returns>
		Public Function ExecuteNonQuery(sql As String) As Integer
			If Not _transaction Then
				Dim cnn = New SQLiteConnection(DBConnection)
				cnn.Open()
				Dim mycommand = New SQLiteCommand(cnn) With { _
					Key .CommandText = sql _
				}
				Dim rowsUpdated = mycommand.ExecuteNonQuery()
				cnn.Close()
				Return rowsUpdated
			Else
				Dim mycommand = New SQLiteCommand(_sqLiteConnection) With { _
					Key .CommandText = sql _
				}
				Return mycommand.ExecuteNonQuery()
			End If
		End Function

		''' <summary>
		'''     Allows the programmer to retrieve single items from the DB.
		''' </summary>
		''' <param name="sql">The query to run.</param>
		''' <returns>A string.</returns>
		Public Function ExecuteScalar(sql As String) As String
			If Not _transaction Then
				Dim cnn = New SQLiteConnection(DBConnection)
				cnn.Open()
				Dim mycommand = New SQLiteCommand(cnn) With { _
					Key .CommandText = sql _
				}
				Dim value = mycommand.ExecuteScalar()
				cnn.Close()
				Return If(value IsNot Nothing, value.ToString(), "")
			Else
				Dim sqLiteCommand = New SQLiteCommand(_sqLiteConnection) With { _
					Key .CommandText = sql _
				}
				Dim value = sqLiteCommand.ExecuteScalar()
				Return If(value IsNot Nothing, value.ToString(), "")
			End If
		End Function

		''' <summary>
		'''     Allows the programmer to easily update rows in the DB.
		''' </summary>
		''' <param name="tableName">The table to update.</param>
		''' <param name="data">A dictionary containing Column names and their new values.</param>
		''' <param name="where">The where clause for the update statement.</param>
		''' <returns>A boolean true or false to signify success or failure.</returns>
		Public Function Update(tableName As [String], data As Dictionary(Of [String], [String]), where As [String]) As Boolean
			Dim vals As [String] = ""
			Dim returnCode As [Boolean] = True
			If data.Count >= 1 Then
				vals = data.Aggregate(vals, Function(current, val) current + [String].Format(" {0} = '{1}',", val.Key.ToString(CultureInfo.InvariantCulture), val.Value.ToString(CultureInfo.InvariantCulture)))
				vals = vals.Substring(0, vals.Length - 1)
			End If
			Try
				ExecuteNonQuery([String].Format("update {0} set {1} where {2};", tableName, vals, where))
			Catch
				returnCode = False
			End Try
			Return returnCode
		End Function

		''' <summary>
		'''     Allows the programmer to easily delete rows from the DB.
		''' </summary>
		''' <param name="tableName">The table from which to delete.</param>
		''' <param name="where">The where clause for the delete.</param>
		''' <returns>A boolean true or false to signify success or failure.</returns>
		Public Function Delete(tableName As [String], where As [String]) As Boolean
			Dim returnCode As [Boolean] = True
			Try
				ExecuteNonQuery([String].Format("delete from {0} where {1};", tableName, where))
			Catch fail As Exception
				MessageBox.Show(fail.Message, fail.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
				returnCode = False
			End Try
			Return returnCode
		End Function

		''' <summary>
		'''     Allows the programmer to easily insert into the DB
		''' </summary>
		''' <param name="tableName">The table into which we insert the data.</param>
		''' <param name="data">A dictionary containing the column names and data for the insert.</param>
		''' <returns>returns last inserted row id if it's value is zero than it means failure.</returns>
		Public Function Insert(tableName As [String], data As Dictionary(Of [String], [String])) As Long
			Dim columns As [String] = ""
			Dim values As [String] = ""
			Dim value As [String]
			For Each val As KeyValuePair(Of [String], [String]) In data
				columns += [String].Format(" {0},", val.Key.ToString(CultureInfo.InvariantCulture))
				values += [String].Format(" '{0}',", val.Value)
			Next
			columns = columns.Substring(0, columns.Length - 1)
			values = values.Substring(0, values.Length - 1)
			Try
				If Not _transaction Then
					Dim cnn = New SQLiteConnection(DBConnection)
					cnn.Open()
					Dim sqLiteCommand = New SQLiteCommand(cnn) With { _
						Key .CommandText = [String].Format("insert into {0}({1}) values({2});", tableName, columns, values) _
					}
					sqLiteCommand.ExecuteNonQuery()
					sqLiteCommand = New SQLiteCommand(cnn) With { _
						Key .CommandText = "SELECT last_insert_rowid()" _
					}
					value = sqLiteCommand.ExecuteScalar().ToString()
				Else
					ExecuteNonQuery([String].Format("insert into {0}({1}) values({2});", tableName, columns, values))
					value = ExecuteScalar("SELECT last_insert_rowid()")
				End If
			Catch fail As Exception
				MessageBox.Show(fail.Message, fail.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
				Return 0
			End Try
			Return Long.Parse(value)
		End Function

		''' <summary>
		'''     Allows the programmer to easily delete all data from the DB.
		''' </summary>
		''' <returns>A boolean true or false to signify success or failure.</returns>
		Public Function ClearDB() As Boolean
			Try
				Dim tables = GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;")
				For Each table As DataRow In tables.Rows
					ClearTable(table("NAME").ToString())
				Next
				Return True
			Catch
				Return False
			End Try
		End Function

		''' <summary>
		'''     Allows the user to easily clear all data from a specific table.
		''' </summary>
		''' <param name="table">The name of the table to clear.</param>
		''' <returns>A boolean true or false to signify success or failure.</returns>
		Public Function ClearTable(table As [String]) As Boolean
			Try
				ExecuteNonQuery([String].Format("delete from {0};", table))
				Return True
			Catch
				Return False
			End Try
		End Function

		''' <summary>
		'''     Allows the user to easily reduce size of database.
		''' </summary>
		''' <returns>A boolean true or false to signify success or failure.</returns>
		Public Function CompactDB() As Boolean
			Try
				ExecuteNonQuery("Vacuum;")
				Return True
			Catch generatedExceptionName As Exception
				Return False
			End Try
		End Function
	End Class
End Namespace



'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
