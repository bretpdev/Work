Imports System
Imports System.Windows.Forms

Public Class Login

    Private _credentials As Credentials

    ''' <summary>
    ''' Creates a login form that will hold onto the Username and Password
    ''' </summary>
    ''' <param name="credentials">Credentials object bound to the controls on the form</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal credentials As Credentials)
        InitializeComponent()
        CredentialsBindingSource.DataSource = credentials
        _credentials = credentials
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOk.Click
        Dim userName As String = txtUsername.Text.PadLeft(5, "0")
        If userName.Length = 5 Then
            userName = userName.Insert(0, "UT")
        End If

        _credentials.UserName = userName
        _credentials.Password = txtPassword.Text

        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub txtUsername_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtUsername.KeyUp, txtPassword.KeyUp
        If e.KeyCode = Keys.Enter Then
            btnOk_Click(Me, New EventArgs())
        End If

        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = DialogResult.Cancel
        End If
    End Sub
End Class