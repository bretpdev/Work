Imports RegentsScholarshipBackEnd

Public Class frmWebAppPasswordReset

    Private _webAppUser As WebAppUser

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        btnResetPassword.Enabled = True
        If radEmail.Checked Or radNames.Checked Or radUserID.Checked Then
            If radNames.Checked Then
                Dim webAppUsers As List(Of WebAppUser) = WebAppDataAccess.GetWebAppUserByName(txtFirstNameSearch.Text, txtLastNameSearch.Text)
                'if multiple users are returned then prompt the user to try another method.
                If webAppUsers.Count > 1 Then
                    MessageBox.Show("Multiple matches found, please use another search criteria.", "Multiple Matches", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtFirstNameSearch.Clear()
                    txtLastNameSearch.Clear()
                    Exit Sub
                Else
                    _webAppUser = webAppUsers(0)
                End If
            ElseIf radEmail.Checked Then
                _webAppUser = WebAppDataAccess.GetWebAppUserByEmail(txtEmailSearch.Text)
            ElseIf radUserID.Checked Then
                _webAppUser = WebAppDataAccess.GetWebAppUserByUserId(txtUserIDSearch.Text)
            End If
            If _webAppUser Is Nothing Then
                MessageBox.Show("No students were found that match the search criteria.", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                WebAppUserBindingSource.DataSource = _webAppUser
            End If
        Else
            btnResetPassword.Enabled = False
            'none of the methods were selected so give user error message
            MessageBox.Show("You must select a method to search by.  Please try again.", "No Method Selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btnResetPassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetPassword.Click
        _webAppUser.SetRandomPassword()
        WebAppDataAccess.UpdateWebAppUserPassword(_webAppUser)
        MessageBox.Show(String.Format("The user's password has been updated to ""{0}"".  All passwords are case sensitive.", _webAppUser.NewPassword))
    End Sub

    Private Sub radNames_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radNames.CheckedChanged
        If radNames.Checked Then
            grpNameSearch.Enabled = True
        Else
            grpNameSearch.Enabled = False
        End If
    End Sub

    Private Sub radEmail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radEmail.CheckedChanged
        If radEmail.Checked Then
            grpEmailSearch.Enabled = True
        Else
            grpEmailSearch.Enabled = False
        End If
    End Sub

    Private Sub radUserID_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radUserID.CheckedChanged
        If radUserID.Checked Then
            grpUserIDSearch.Enabled = True
        Else
            grpUserIDSearch.Enabled = False
        End If
    End Sub
End Class