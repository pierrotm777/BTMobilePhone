
Option Strict On

Imports System.Text.RegularExpressions
Imports System.Linq


Public Class PhoneNumberFormatter

    'example
    'Private Shared _switzerlandDefault As PhoneNumberFormatter
    'Public Shared ReadOnly Property SwitzerlandDefault() As PhoneNumberFormatter
    '    Get
    '        If _switzerlandDefault Is Nothing Then
    '            _switzerlandDefault = New PhoneNumberFormatter("0", "41", "00")
    '        End If

    '        Return _switzerlandDefault
    '    End Get
    'End Property

#Region "Enums"

    Public Enum OutputFormats
        International
        National
    End Enum

#End Region

#Region "Fields"

    'Siehe auch http://www.countrycallingcodes.com/
    Private _countryCode As String
    Private _internationalCarrierCode As String
    Private _networkCarrierCode As String


#End Region

#Region "Constructors"

    Public Sub New(ByVal networkCarrierCode As String, ByVal countryCode As String, ByVal internationalCarrierCode As String)
        Me._networkCarrierCode = networkCarrierCode
        Me._countryCode = countryCode
        Me._internationalCarrierCode = internationalCarrierCode
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property NetworkCarrierCode() As String
        Get
            Return Me._networkCarrierCode
        End Get
    End Property

    Public ReadOnly Property CountryCode() As String
        Get
            Return Me._countryCode
        End Get
    End Property

    Public ReadOnly Property InternationalCarrierCode() As String
        Get
            Return Me._internationalCarrierCode
        End Get
    End Property

#End Region

#Region "Public methods"

    Public Overridable Function ConvertToCallablePhoneNumber(ByVal phoneNumber As String, ByVal outputFormat As OutputFormats) As String

        Try

            'S'assurer que le Numéro n'est pas formaté
            phoneNumber = Me.ConvertToUnformatedPhoneNumber(phoneNumber)

            'Le signe + (Pnb: +41) remplace International Carrier Code
            Dim regEx As New Regex("^[+].*")
            If regEx.IsMatch(phoneNumber) Then
                '+ erstezen mit internationaler Verkehrausscheidungsziffer der Schweiz
                phoneNumber = Me.InternationalCarrierCode + phoneNumber.Substring(1)
            End If

            If outputFormat = OutputFormats.National Then

                'Numéros de actuelles de Pays dans leur Format
                If phoneNumber.StartsWith(Me.InternationalCarrierCode + Me.CountryCode) Then
                    'Swiss -> 004131xxx to 031xxx
                    phoneNumber = Me.NetworkCarrierCode + phoneNumber.Substring((InternationalCarrierCode + Me.CountryCode).Length)
                End If
            ElseIf outputFormat = OutputFormats.International Then
                'do nothing. Standard is international
                ToLog(phoneNumber & " is already in international format")
            End If

        Catch ex As Exception
            ToLog("ConvertToCallablePhoneNumber conversion error")
        End Try

        Return phoneNumber

    End Function

    Public Overridable Function ConvertToUnformatedPhoneNumber(ByVal phoneNumber As String) As String

        Try

            If Not phoneNumber Is Nothing Then

                'supprimer les espaces
                phoneNumber = phoneNumber.Replace(" ", "")

                'tout numéro de téléphone , mais qui ne commence pas avec le Code du transporteur réseau avec Code du transporteur international
                '...Créer Motif RegEx Sous
                Dim rexSubPattern As String = String.Empty
                For i As Integer = 0 To 9 Step 1
                    If i.ToString() <> Me.NetworkCarrierCode Then

                        If rexSubPattern.Length > 0 Then
                            rexSubPattern += "|"
                        End If

                        rexSubPattern += i.ToString()

                    End If
                Next

                Dim regEx As New Regex("^[" + Me.NetworkCarrierCode + "][" + rexSubPattern + "].*")
                If regEx.IsMatch(phoneNumber) Then
                    'Set code de pays
                    phoneNumber = "+" + Me.CountryCode + phoneNumber.Substring(1)
                End If

                'un numéro qui commence le Code du transporteur international
                'Bsp: 0041 to +41
                regEx = New Regex("^[" + Me.InternationalCarrierCode + "].*")
                If regEx.IsMatch(phoneNumber) Then
                    'International Carrier Code erstezen mit '+' Platzhalter
                    phoneNumber = "+" + phoneNumber.Substring(Me.InternationalCarrierCode.Length)
                End If

                'Peut-être supprimer les crochets existants. Bsp: 0041 (0)31 388 10 10
                regEx = New Regex("(\[|\()(\d)*(\]|\))")
                Dim m As Match = regEx.Match(phoneNumber)
                While m.Success
                    phoneNumber = phoneNumber.Remove(m.Index, m.Length)
                    'm = m.NextMatch() 'Ne pas aller parce que la chaîne est déjà changée avec Remplacer
                    m = regEx.Match(phoneNumber)
                End While

                'Si elle est un numéro Vanity
                regEx = New Regex("(\d|\s){4,}[a-z]", RegexOptions.IgnoreCase)
                If regEx.IsMatch(phoneNumber) Then
                    'Vanity traduire valeur Nombre
                    regEx = New Regex("[a-z]", RegexOptions.IgnoreCase)
                    m = regEx.Match(phoneNumber)
                    While m.Success
                        phoneNumber = phoneNumber.Replace(phoneNumber.Substring(m.Index, m.Length), VanityLetterToDigit(m.Value))
                        m = regEx.Match(phoneNumber)
                    End While
                End If

                '(Non numérique ) Supprimez les caractères restants maintenant
                'Le premier + reste cependant (le cas échéant )
                Dim national As Boolean = phoneNumber.StartsWith("+")
                regEx = New Regex("\D")
                If regEx.IsMatch(phoneNumber) Then
                    m = regEx.Match(phoneNumber)
                    While m.Success
                        phoneNumber = phoneNumber.Remove(m.Index, m.Length)
                        m = regEx.Match(phoneNumber)
                    End While
                End If
                If national Then phoneNumber = "+" + phoneNumber

            End If

        Catch ex As Exception
            ToLog("ConvertToUnformatedPhoneNumber error")
        End Try

        Return phoneNumber

    End Function

    Protected Overridable Function VanityLetterToDigit(ByVal str As String) As String

        Select Case str.ToLower()
            Case "a"
                Return "2"
            Case "b"
                Return "2"
            Case "c"
                Return "2"
            Case "d"
                Return "3"
            Case "e"
                Return "3"
            Case "f"
                Return "3"
            Case "g"
                Return "4"
            Case "h"
                Return "4"
            Case "i"
                Return "4"
            Case "j"
                Return "5"
            Case "k"
                Return "5"
            Case "l"
                Return "5"
            Case "m"
                Return "6"
            Case "n"
                Return "6"
            Case "o"
                Return "6"
            Case "p"
                Return "7"
            Case "q"
                Return "7"
            Case "r"
                Return "7"
            Case "s"
                Return "7"
            Case "t"
                Return "8"
            Case "u"
                Return "8"
            Case "v"
                Return "8"
            Case "w"
                Return "9"
            Case "x"
                Return "9"
            Case "y"
                Return "9"
            Case "z"
                Return "9"

            Case Else
                Return ""

        End Select

    End Function

    ' note: only the CH format is implemented
    Private Function ConvertToRegionalFormat(ByVal phoneNumber As String, ByVal format As OutputFormats) As String

        If phoneNumber IsNot Nothing Then

            Dim workingNumber As String = phoneNumber.Trim()
            If workingNumber.StartsWith("00") OrElse workingNumber.StartsWith("+") Then
                workingNumber = Me.ConvertToUnformatedPhoneNumber(workingNumber)
            End If

            Try
                ' Siehe auch: 'Technische und administrative Vorschriften betreffend der Aufteilung der E.164-Nummern' des Bakoms

                Dim prefix As String = "0"
                If format = OutputFormats.International Then
                    ' note: with ending space!
                    prefix = "+41 "
                End If

                If workingNumber.StartsWith("1") Then
                    ' Die Kurznummern haben das Format 1xx, wobei einzelne Kurznummern mit einer oder mehreren 
                    ' Ziffern durch das BAKOM erweitert werden können. Je nach Anwendung können Inhaberinnen oder 
                    ' Inhaber von Kurznummern selber bestimmen, ob diese auch aus dem Ausland angewählt werden können.
                    '
                    ' Liste der CH-Kurznummern:
                    ' https://www.eofcom.ch/viewSearchSN.do

                    ' note: Kurznummer sind in der Regeln vom Ausland her nicht erreichbar. Deshalb wird +41 nicht vorangestellt.
                    ' return as is...
                    Return workingNumber

                ElseIf workingNumber.StartsWith("+4186") AndAlso workingNumber.Length = 14 Then
                    ' Kennzahl 860; Zugang zu Anrufbeantwortersystemen -> Siehe BAKOM
                    ' ...Für Abfragen aus der Schweiz:  0860 31 765 43 21 
                    ' ...Für Abfragen aus dem Ausland:  +41 860 31 765 43 21 
                    Return String.Format(prefix & "86 {5}{6} {7}{8}{9} {10}{11} {12}{13}", workingNumber.ToArray().Cast(Of Object)().ToArray())
                ElseIf (workingNumber.StartsWith("+41800") OrElse _
                        workingNumber.StartsWith("+4184") OrElse _
                        workingNumber.StartsWith("+41878") OrElse _
                        workingNumber.StartsWith("+41900")) AndAlso workingNumber.Length = 12 Then

                    'Einzelnummern (0800, 084x, 0878, 090x)
                    ' ... Liste von BAKOM Stand 18.05.2011
                    '0800 xxx xxx Gratisnummern 
                    '0840 xxx xxx Gebührenteilungsnummern 
                    '0842 xxx xxx Gebührenteilungsnummern 
                    '0844 xxx xxx Gebührenteilungsnummern 
                    '0848 xxx xxx Gebührenteilungsnummern 
                    '0878 xxx xxx Persönliche Nummern 
                    '0900 xxx xxx Mehrwertdienstenummern für Business, Marketing 
                    '0901 xxx xxx Mehrwertdienstenummern für Unterhaltung, Spiele, Response 
                    '0906 xxx xxx Mehrwertdienstenummern für Erwachsenenunterhaltung 

                    ' return format +41 xxx xxx xxx
                    Return String.Format(prefix & "{3}{4}{5} {6}{7}{8} {9}{10}{11}", workingNumber.ToArray().Cast(Of Object)().ToArray())
                ElseIf workingNumber.StartsWith("+41") AndAlso workingNumber.Length = 12 Then
                    ' return format +41 xx xxx xx xx
                    Return String.Format(prefix & "{3}{4} {5}{6}{7} {8}{9} {10}{11}", workingNumber.ToArray().Cast(Of Object)().ToArray())

                End If
                ' ignore this exception an continue with default return value (see below).
            Catch ex As Exception
            End Try
        End If

        ' return input as default
        Return phoneNumber
    End Function

#End Region

    Function ConvertToCallablePhoneNumber() As String
        Throw New NotImplementedException
    End Function

End Class

