Imports System
Imports System.Windows.Forms

Public Class Login

    ''' <summary>
    ''' Creates a login form that will hold onto the Username and Password
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOk.Click
        Dim userName As String = txtUsername.Text.PadLeft(5, "0")
        If userName.Length = 5 Then
            userName = userName.Insert(0, "UT")
        End If

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