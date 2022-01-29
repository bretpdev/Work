Imports System.DirectoryServices

Public Class Ldap
    Private Const UHEAA_DIRECTORY_PATH As String = "LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local"

    ''' <summary>
    ''' Attempts to authenticate a user with Active Directory and returns whether it was successful.
    ''' </summary>
    ''' <param name="userName">Windows user name</param>
    ''' <param name="password">Windows password</param>
    Public Shared Function AuthenticateUser(ByVal userName As String, ByVal password As String) As Boolean
        Try
            'Bind to the native ADSI object to force authentication.
            Dim foo As Object = New DirectoryEntry("", "UHEAA\" + userName, password).NativeObject
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function 'AuthenticateUser()
End Class
