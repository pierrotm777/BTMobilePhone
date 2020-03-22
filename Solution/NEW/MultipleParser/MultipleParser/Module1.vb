Imports MultipleParser.LumiSoft.Net.Mime.vCard
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports System.IO

Module Module1

    Public Sub Main()
        Dim appPath As String = Application.StartupPath
        'MessageBox.Show(appPath)
        Dim cs As String = "URI=file:" & appPath & "\phonebook.s3db"
        Dim itm As List(Of vCard) = vCard.ParseMultiple(appPath & "\pb.vcf")
 
        Try

            'Dim cs As String = "URI=file:phonebook.s3db"
            'Dim itm As List(Of vCard) = vCard.ParseMultiple("pb.vcf")

            'ajout pm 11-12-2015
            SQLiteConnection.CreateFile(appPath & "\phonebook.s3db")
            Dim myQuery As String = "create table Contacts (Name varchar(50), MobilePhone varchar(50), HomePhone varchar(50), " & _
                                                "Work varchar(50), Fax varchar(50))"
            Dim dbConnection As SQLiteConnection = New SQLiteConnection(cs) '("Data Source=" + appPath + ";Version=3;")
            Dim command As SQLiteCommand = New SQLiteCommand(myQuery, dbConnection)
            dbConnection.Open() '//Open the connection with database
            command.ExecuteNonQuery() '//Executes the SQL query
            dbConnection.Close() '//Close the connection with database
            'ajout pm 11-12-2015

            For Each eyetm In itm

                Using con As New SQLiteConnection(cs)
                    Dim SQLCommand As SQLiteCommand
                    con.Open()
                    SQLCommand = con.CreateCommand

                    If eyetm.FormattedName IsNot "" Then
                        SQLCommand.CommandText = "SELECT * FROM Contacts WHERE Name =""" & eyetm.FormattedName & """"
                        If SQLCommand.ExecuteScalar() Is Nothing Then
                            SQLCommand.CommandText = "INSERT INTO Contacts (Name) VALUES (""" & eyetm.FormattedName & """)"
                            SQLCommand.ExecuteNonQuery()
                        End If
                    End If
                    Dim phones As PhoneNumberCollection = eyetm.PhoneNumbers
                    For Each phone As PhoneNumber In phones
                        'MessageBox.Show(phone.Item.ToItemString & vbCrLf & phone.Number & vbCrLf & phone.NumberType)
                        Select Case phone.NumberType
                            Case PhoneNumberType_enum.Cellular
                                SQLCommand.CommandText = "UPDATE Contacts SET MobilePhone = '" & phone.Number & "' WHERE Name = """ & eyetm.FormattedName & """"
                                SQLCommand.ExecuteNonQuery()
                                'MessageBox.Show(phone.Number, "Cellular")
                            Case PhoneNumberType_enum.Home
                                SQLCommand.CommandText = "UPDATE Contacts SET HomePhone = '" & phone.Number & "' WHERE Name = """ & eyetm.FormattedName & """"
                                SQLCommand.ExecuteNonQuery()
                                'MessageBox.Show(phone.Number, "Home")
                            Case PhoneNumberType_enum.Work
                                SQLCommand.CommandText = "UPDATE Contacts SET Work = '" & phone.Number & "' WHERE Name = """ & eyetm.FormattedName & """"
                                SQLCommand.ExecuteNonQuery()
                                'MessageBox.Show(phone.Number, "Work")
                            Case PhoneNumberType_enum.Fax
                                SQLCommand.CommandText = "UPDATE Contacts SET Fax = '" & phone.Number & "' WHERE Name = """ & eyetm.FormattedName & """"
                                SQLCommand.ExecuteNonQuery()
                                'MessageBox.Show(phone.Number, "Fax")
                            Case PhoneNumberType_enum.Preferred Or PhoneNumberType_enum.Cellular
                                SQLCommand.CommandText = "UPDATE Contacts SET MobilePhone = '" & phone.Number & "' WHERE Name = """ & eyetm.FormattedName & """"
                                SQLCommand.ExecuteNonQuery()
                                'MessageBox.Show(phone.Number)
                            Case PhoneNumberType_enum.Preferred Or PhoneNumberType_enum.Home
                                SQLCommand.CommandText = "UPDATE Contacts SET HomePhone = '" & phone.Number & "' WHERE Name = """ & eyetm.FormattedName & """"
                                SQLCommand.ExecuteNonQuery()
                                'MessageBox.Show(phone.Number)
                            Case PhoneNumberType_enum.Preferred Or PhoneNumberType_enum.Work
                                SQLCommand.CommandText = "UPDATE Contacts SET Work = '" & phone.Number & "' WHERE Name = """ & eyetm.FormattedName & """"
                                SQLCommand.ExecuteNonQuery()
                                'MessageBox.Show(phone.Number)
                        End Select
                    Next

                   
                    SQLCommand.Dispose()
                    con.Close()
                End Using
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


        'Dim c As String = "URI=file:phonebook.s3db"
        'Dim count As Integer = 200
        'Dim it As List(Of vCard) = vCard.ParseMultiple("cch.vcf")
        'For Each tm In it.Take(count)
        '    Using conn As New SQLiteConnection(c)
        '        Dim SQLCommand As SQLiteCommand
        '        conn.Open()
        '        SQLCommand = conn.CreateCommand
        '        If tm.FormattedName Is "" Then
        '            SQLCommand.CommandText = "INSERT INTO CallHistory (Name,CallType,PhoneType,PhoneNumber,DateTime) VALUES (""Unknown"",""" & tm.CallType & """,""" & Nothing & """,""" & tm.CallHistoryNum & """, """ & tm.CallDateTime & """)"
        '            SQLCommand.ExecuteNonQuery()
        '        Else
        '            SQLCommand.CommandText = "INSERT INTO CallHistory (Name,CallType,PhoneType,PhoneNumber,DateTime) VALUES (""" & tm.FormattedName & """,""" & tm.CallType & """,""" & tm.PhoneType & """,""" & tm.CallHistoryNum & """, """ & tm.CallDateTime & """)"
        '            SQLCommand.ExecuteNonQuery()
        '        End If

        '        SQLCommand.CommandText = "INSERT INTO CallHistory () VALUES (""" & tm.FormattedName & """)"

        '        SQLCommand.Dispose()
        '        conn.Close()
        '    End Using

        'Next
    End Sub


End Module
