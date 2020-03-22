Dim sqLiteDatabase = New SQLiteDatabase(True)

sqLiteDatabase.Delete("Disks", String.Format(" id = '{0}'", ID))

sqLiteDatabase.Delete("Folders", String.Format(" DiskID = '{0}'", ID))

sqLiteDatabase.Delete("Files", String.Format(" DiskID = '{0}'", ID))

sqLiteDatabase.Delete("Loan", String.Format(" DiskID = '{0}'", ID))

sqLiteDatabase.CommitTransaction()

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
