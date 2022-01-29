Imports System.Windows.Forms
Public Class Credentials

    Private _userName As String
    Public Property UserName() As String
        Get
            Return _userName
        End Get
        Set(ByVal value As String)
            _userName = value
        End Set
    End Property


    Private _password As String
    Public Property Password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Shared Function FromPrompt() As Credentials
        Dim credentials As New Credentials
        Dim login As New Login(credentials)
        If login.ShowDialog = DialogResult.OK Then
            Return credentials
        Else
            Return Nothing
        End If
    End Function

End Class
