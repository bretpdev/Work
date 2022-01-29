Imports System.Text.RegularExpressions

Public Class Validator
    Public Shared Function IsValidStreetAddress(ByVal streetAddress As String) As Boolean
        'Check that there's something there.
        If String.IsNullOrEmpty(streetAddress) Then
            Return False
        End If
        'Check that the address contains at least one number (house number, P.O. Box number).
        'Seriously, Caleb Figge claims that his street address is "Utah." That won't do.
        If Not Regex.IsMatch(streetAddress, "[0-9]") Then
            Return False
        End If

        Return True
    End Function

    Public Shared Function IsValidCity(ByVal city As String) As Boolean
        'Check that there's something there.
        If String.IsNullOrEmpty(city) Then
            Return False
        End If

        Return True
    End Function

    Public Shared Function IsValidState(ByVal state As String) As Boolean
        'Check that there's something there.
        If String.IsNullOrEmpty(state) Then
            Return False
        End If
        'Check state against database lookup.
        Return Lookups.States.Select(Function(p) p.Abbreviation).Contains(state)
    End Function

    Public Shared Function IsValidZipCode(ByVal zipCode As String) As Boolean
        'Check that there's something there.
        If String.IsNullOrEmpty(zipCode) Then
            Return False
        End If
        'Call it good if the whole zip code is 5 digits...
        If Regex.IsMatch(zipCode, "^[0-9]{5}$") Then
            Return True
        End If
        '...or 9 digits...
        If Regex.IsMatch(zipCode, "^[0-9]{9}$") Then
            Return True
        End If
        '...or 5 digits, a hyphen, and 4 more digits.
        If Regex.IsMatch(zipCode, "^[0-9]{5}-[0-9]{4}$") Then
            Return True
        End If

        Return False
    End Function

    Public Shared Function IsValidPhoneNumber(ByVal phoneNumber As String) As Boolean
        'Check that there's something there.
        If String.IsNullOrEmpty(phoneNumber) Then
            Return False
        End If
        'Because foreign phone numbers can involve unpredictable formatting,
        'we won't try to match a particular set of regular expressions.
        'Instead, just check the length and make sure there aren't any letters.
        If phoneNumber.Length < 10 OrElse phoneNumber.Length > 17 OrElse Regex.IsMatch(phoneNumber, "[a-zA-Z]") Then
            Return False
        End If

        Return True
    End Function

    Public Shared Function IsValidEmailAddress(ByVal emailAddress As String) As Boolean
        'Check that there's something there.
        If String.IsNullOrEmpty(emailAddress) Then
            Return False
        End If
        'Check that it's not too long. (The regular expression check later on will ensure that it's not too short.)
        If emailAddress.Length > 56 Then
            Return False
        End If
        'Check the format against the official specification.
        If Not Regex.IsMatch(emailAddress, "^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") Then
            Return False
        End If

        Return True
    End Function

    Public Shared Function IsValidPassword(ByVal password As String) As Boolean
        'Make sure string is not empty
        If String.IsNullOrEmpty(password) Then
            Return False
        End If

        'Check length to make sure it's long enough
        If password.Length < 5 Then
            Return False
        End If

        If Not Regex.IsMatch(password, "(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$") Then
            Return False
        End If

        Return True
    End Function

    Public Shared Function DateIsCurrentOrInPast(ByVal theDate As Nullable(Of Date)) As Boolean
        If theDate Is Nothing Then Return True
        If theDate.Value.Date > Today.Date Then
            Return False
        Else
            Return True
        End If
    End Function
End Class
