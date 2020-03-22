Namespace SMS

    Public Class vCard

        ' Specs can be found at: http://www.imc.org/pdi/pdiproddev.html
        Public Title As String = ""  'Mr., Mrs., Ms., Dr.
        Public FirstName As String = ""
        Public MiddleName As String = ""
        Public LastName As String = ""
        Public Suffix As String = "" 'I, II, Jr., Sr.
        Public FormattedName As String = ""
        Public Nickname As String = ""
        Public Organization As String = ""           ' MS Outlook calls this Company
        Public OrganizationalUnit As String = ""     ' MS Outlook calls this Department
        Public Role As String = ""                    ' MS Outlook calls this the profession
        Public JobTitle As String = ""
        Public Note As String = ""
        Public Birthday As Date

        'Collections
        Public URLs As New vURLs()
        Public Emails As New vEmails()
        Public Telephones As New vTelephones()
        Public Addresses As New vAddresss()

        Public LastModified As Date

        Public Overrides Function ToString() As String
            Dim result As New System.Text.StringBuilder()
            result.AppendFormat("//SCKE2 BEGIN:VCARD{0}", System.Environment.NewLine)
            result.AppendFormat("VERSION:2.1{0}", System.Environment.NewLine)
            result.AppendFormat("N:{0};{1};{2};{3};{4}{5}", LastName, FirstName, MiddleName, Title, Suffix, System.Environment.NewLine)
            If IsNotBlank(FormattedName) Then result.AppendFormat("FN:{0}{1}", FormattedName, System.Environment.NewLine)
            If IsNotBlank(Nickname) Then result.AppendFormat("NICKNAME:{0}{1}", Nickname, System.Environment.NewLine)
            If Birthday > Date.MinValue Then result.AppendFormat("BDAY:{0}{1}", Birthday.ToUniversalTime.ToString("yyyyMMdd"), System.Environment.NewLine)
            If IsNotBlank(Note) Then result.AppendFormat("NOTE;ENCODING=QUOTED-PRINTABLE:{0}{1}", Note.Replace(System.Environment.NewLine, "=0D=0A"), System.Environment.NewLine)
            result.AppendFormat("ORG:{0};{1}{2}", Organization, OrganizationalUnit, System.Environment.NewLine)
            If IsNotBlank(JobTitle) Then result.AppendFormat("TITLE:{0}{1}", JobTitle, System.Environment.NewLine)
            If IsNotBlank(Role) Then result.AppendFormat("ROLE:{0}{1}", Role, System.Environment.NewLine)
            result.Append(Emails.ToString())
            result.Append(Telephones.ToString())
            result.Append(URLs.ToString())
            result.Append(Addresses.ToString())
            result.AppendFormat("REV:{0}{1}", LastModified.ToUniversalTime.ToString("yyyyMMdd\THHmmss\Z"), System.Environment.NewLine)
            result.AppendFormat("END:VCARD{0}", System.Environment.NewLine)
            Return result.ToString
        End Function

        Public Class vEmails
            ' The first thing to do when building a CollectionBase class is to inherit from System.Collections.CollectionBase
            Inherits System.Collections.CollectionBase

            Public Overloads Function Add(ByVal Value As vEmail) As vEmail
                ' After you inherit the CollectionBase class, you can access an intrinsic object
                ' called InnerList that represents your collection. InnerList is of type ArrayList.
                If Value.Preferred Then
                    Dim item As vEmail
                    For Each item In Me.InnerList
                        item.Preferred = False
                    Next
                End If
                Me.InnerList.Add(Value)
                Return Value
            End Function

            Public Overloads Function Item(ByVal Index As Integer) As vEmail
                ' To retrieve an item from the InnerList, pass the index of that item to the .Item property.
                Return CType(Me.InnerList.Item(Index), vEmail)
            End Function

            Public Overloads Sub Remove(ByVal Index As Integer)
                ' This Remove expects an index.
                Dim cust As vEmail

                cust = CType(Me.InnerList.Item(Index), vEmail)
                If Not cust Is Nothing Then
                    Me.InnerList.Remove(cust)
                End If
            End Sub

            Public Overrides Function ToString() As String
                Dim result As New System.Text.StringBuilder()
                Dim item As vEmail
                For Each item In Me.InnerList
                    result.AppendFormat("{0}", item.ToString)
                Next
                Return result.ToString
            End Function
        End Class

        Public Class vEmail
            Public Preferred As Boolean
            Public EmailAddress As String = ""
            Public Type As String = "INTERNET"

            Public Sub New(ByVal Email As String)
                EmailAddress = Email
            End Sub

            Public Sub New(ByVal Email As String, ByVal IsPreferred As Boolean)
                EmailAddress = Email
                Preferred = IsPreferred
            End Sub

            Public Overrides Function ToString() As String
                Dim result As New System.Text.StringBuilder()
                result.Append("EMAIL")
                If Preferred Then result.Append(";PREF")
                result.AppendFormat(";{0}", Type.ToUpper)
                result.AppendFormat(":{0}{1}", EmailAddress, System.Environment.NewLine)
                Return result.ToString
            End Function
        End Class

        Public Class vURLs
            ' The first thing to do when building a CollectionBase class is to inherit from System.Collections.CollectionBase
            Inherits System.Collections.CollectionBase

            Public Overloads Function Add(ByVal Value As vURL) As vURL
                ' After you inherit the CollectionBase class, you can access an intrinsic object
                ' called InnerList that represents your collection. InnerList is of type ArrayList.
                If Value.Preferred Then
                    Dim item As vURL
                    For Each item In Me.InnerList
                        Value.Preferred = False
                    Next
                End If
                Me.InnerList.Add(Value)
                Return Value
            End Function

            Public Overloads Function Item(ByVal Index As Integer) As vURL
                ' To retrieve an item from the InnerList, pass the index of that item to the .Item property.
                Return CType(Me.InnerList.Item(Index), vURL)
            End Function

            Public Overloads Sub Remove(ByVal Index As Integer)
                ' This Remove expects an index.
                Dim cust As vURL

                cust = CType(Me.InnerList.Item(Index), vURL)
                If Not cust Is Nothing Then
                    Me.InnerList.Remove(cust)
                End If
            End Sub

            Public Overrides Function ToString() As String
                Dim result As New System.Text.StringBuilder()
                Dim item As vURL
                For Each item In Me.InnerList
                    result.AppendFormat("{0}", item.ToString)
                Next
                Return result.ToString
            End Function
        End Class

        Public Class vURL
            Public Preferred As Boolean
            Public URL As String = ""
            Public Location As vLocations = vLocations.WORK       'MS Outlook shows the WORK location on the contact form front page

            Public Sub New(ByVal NewURL As String)
                URL = NewURL
            End Sub

            Public Sub New(ByVal NewURL As String, ByVal IsPreffered As Boolean)
                URL = NewURL
                Preferred = IsPreffered
            End Sub

            Public Overrides Function ToString() As String
                Dim result As New System.Text.StringBuilder()
                result.Append("URL")
                If Preferred Then result.Append(";PREF")
                If Not IsNothing(Location) Then result.AppendFormat(";{0}", Location.ToString.ToUpper)
                result.AppendFormat(":{0}{1}", URL, System.Environment.NewLine)
                Return result.ToString
            End Function
        End Class

        Public Class vTelephones
            ' The first thing to do when building a CollectionBase class is to inherit from System.Collections.CollectionBase
            Inherits System.Collections.CollectionBase

            Public Overloads Function Add(ByVal Value As vTelephone) As vTelephone
                ' After you inherit the CollectionBase class, you can access an intrinsic object
                ' called InnerList that represents your collection. InnerList is of type ArrayList.
                If Value.Preferred Then
                    Dim item As vTelephone
                    For Each item In Me.InnerList
                        item.Preferred = False
                    Next
                End If
                Me.InnerList.Add(Value)
                Return Value
            End Function

            Public Overloads Function Item(ByVal Index As Integer) As vTelephone
                ' To retrieve an item from the InnerList, pass the index of that item to the .Item property.
                Return CType(Me.InnerList.Item(Index), vTelephone)
            End Function

            Public Overloads Sub Remove(ByVal Index As Integer)
                ' This Remove expects an index.
                Dim cust As vTelephone

                cust = CType(Me.InnerList.Item(Index), vTelephone)
                If Not cust Is Nothing Then
                    Me.InnerList.Remove(cust)
                End If
            End Sub

            Public Overrides Function ToString() As String
                Dim result As New System.Text.StringBuilder()
                Dim item As vTelephone
                For Each item In Me.InnerList
                    result.AppendFormat("{0}", item.ToString)
                Next
                Return result.ToString
            End Function
        End Class

        Public Class vTelephone
            Public Preferred As Boolean
            Public TelephoneNumber As String = ""
            Public Location As vLocations
            Public Type As vPhoneTypes

            Public Sub New(ByVal Number As String)
                TelephoneNumber = Number
            End Sub

            Public Sub New(ByVal Number As String, ByVal IsPreferred As Boolean)
                TelephoneNumber = Number
                Preferred = IsPreferred
            End Sub

            Public Sub New(ByVal Number As String, ByVal PhoneLocation As vLocations, ByVal PhoneType As vPhoneTypes, ByVal IsPreferred As Boolean)
                TelephoneNumber = Number
                Location = PhoneLocation
                Type = PhoneType
                Preferred = IsPreferred
            End Sub

            Public Overrides Function ToString() As String
                Dim result As New System.Text.StringBuilder()
                result.Append("TEL")
                If Preferred Then result.Append(";PREF")
                If Not IsNothing(Location) Then result.AppendFormat(";{0}", Location.ToString.ToUpper)
                If Not IsNothing(Type) Then result.AppendFormat(";{0}", Type.ToString.ToUpper)
                result.AppendFormat(":{0}{1}", TelephoneNumber, System.Environment.NewLine)
                Return result.ToString
            End Function

            Public Sub New()

            End Sub
        End Class

        Public Class vAddresss
            ' The first thing to do when building a CollectionBase class is to inherit from System.Collections.CollectionBase
            Inherits System.Collections.CollectionBase

            Public Overloads Function Add(ByVal Value As vAddress) As vAddress
                ' After you inherit the CollectionBase class, you can access an intrinsic object
                ' called InnerList that represents your collection. InnerList is of type ArrayList.
                If Value.Preferred Then
                    Dim item As vAddress
                    For Each item In Me.InnerList
                        item.Preferred = False
                    Next
                End If
                Me.InnerList.Add(Value)
                Return Value
            End Function

            Public Overloads Function Item(ByVal Index As Integer) As vAddress
                ' To retrieve an item from the InnerList, pass the index of that item to the .Item property.
                Return CType(Me.InnerList.Item(Index), vAddress)
            End Function

            Public Overloads Sub Remove(ByVal Index As Integer)
                ' This Remove expects an index.
                Dim cust As vAddress

                cust = CType(Me.InnerList.Item(Index), vAddress)
                If Not cust Is Nothing Then
                    Me.InnerList.Remove(cust)
                End If
            End Sub

            Public Overrides Function ToString() As String
                Dim result As New System.Text.StringBuilder()
                Dim item As vAddress
                For Each item In Me.InnerList
                    result.AppendFormat("{0}", item.ToString)
                Next
                Return result.ToString
            End Function
        End Class

        Public Class vAddress
            Public Preferred As Boolean
            Public AddressName As String = ""    'MS Outlook calls this Office
            Public StreetAddress As String = ""
            Public City As String = ""
            Public State As String = ""
            Public Zip As String = ""
            Public Country As String = ""
            Public AddressLabel As String = ""   'If you don't want to waste time creating this, don't and let the vCard reader format it for you
            Public Location As vLocations  'HOME, WORK, CELL
            Public Type As vAddressTypes    'PARCEL, DOM, INT

            Public Overrides Function ToString() As String
                Dim result As New System.Text.StringBuilder()

                'Write the Address
                result.Append("ADR")
                If Preferred Then result.Append(";PREF")
                If Not IsNothing(Location) Then result.AppendFormat(";{0}", Location.ToString.ToUpper)
                If Not IsNothing(Type) Then result.AppendFormat(";{0}", Type.ToString.ToUpper)
                result.AppendFormat(";ENCODING=QUOTED-PRINTABLE:;{0}", AddressName)
                result.AppendFormat(";{0}", StreetAddress.Replace(System.Environment.NewLine, "=0D=0A"))
                result.AppendFormat(";{0}", City.Replace(System.Environment.NewLine, "=0D=0A"))
                result.AppendFormat(";{0}", State.Replace(System.Environment.NewLine, "=0D=0A"))
                result.AppendFormat(";{0}", Zip.Replace(System.Environment.NewLine, "=0D=0A"))
                result.AppendFormat(";{0}", Country.Replace(System.Environment.NewLine, "=0D=0A"))
                result.Append(System.Environment.NewLine)

                'Write the Address label
                If AddressLabel.Length > 0 Then
                    result.Append("LABEL")
                    If Not IsNothing(Location) Then result.AppendFormat(";{0}", Location.ToString.ToUpper)
                    If Not IsNothing(Type) Then result.AppendFormat(";{0}", Type.ToString.ToUpper)
                    result.AppendFormat(";ENCODING=QUOTED-PRINTABLE:{0}", AddressLabel.Replace(System.Environment.NewLine, "=0D=0A"))
                End If

                Return result.ToString
            End Function
        End Class

        Public Enum vLocations
            HOME
            WORK
            CELL
        End Enum

        Public Enum vAddressTypes
            PARCEL  'Parcel post
            DOM     'Domestic
            INT     'International
        End Enum

        Public Enum vPhoneTypes
            VOICE
            FAX
            MSG
        End Enum

        Private Shared Function IsBlank(ByVal Value As String) As Boolean
            Return (IsNothing(Value) OrElse Value.Length = 0)
        End Function

        Private Shared Function IsNotBlank(ByVal Value As String) As Boolean
            Return Not IsBlank(Value)
        End Function
    End Class
End Namespace