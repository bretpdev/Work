Imports System.Threading
Imports RegentsScholarshipBackEnd

Public Class frmLogin
    Private _allUsers As IEnumerable(Of User)
    Private _currentUsers As List(Of User)
    Private _warned As Boolean 'Keeps us from re-warning the user about an impending password expiration.
    Private _user As User

    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _allUsers = DataAccess.GetAllUsers().ToList()
        _currentUsers = New List(Of User)
        For Each userId As String In _allUsers.Select(Function(p) p.Id).Distinct()
            Dim currentId As String = userId
            _currentUsers.Add(_allUsers.Where(Function(p) p.Id = currentId).OrderBy(Function(p) p.PasswordDate).Last())
        Next
        _warned = False
        lblTestMode.Visible = Constants.TEST_MODE
    End Sub

    Private Sub btnUserAccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUserAccess.Click
        Dim useraccess As New frmUserAccess()
        useraccess.ShowDialog()
    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        If Not Validator.IsValidPassword(tbxPassword.Text) Then
            Dim message As String = "Your password must contain 1 uppercase letter, 1 lowercase letter, 1 number or special character and be 8 character long"
            MessageBox.Show(message, "Incorrect password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If
        ValidateUser()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If (tbxUserID.Text = "") AndAlso (tbxPassword.Text = "") Then
            MessageBox.Show("Please put in your Username and Password and click update again", "Update Password", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim currentUser As User = _currentUsers.Where(Function(p) p.Id = tbxUserID.Text).SingleOrDefault()
        If (currentUser Is Nothing) Then Return

        If (currentUser.Id = tbxUserID.Text) AndAlso (currentUser.PasswordHash = EncryptText(tbxPassword.Text)) Then
            lblPassword.Text = "New Password"
            tbxPassword.Text = ""
            tbxPassword.Focus()
            Return
        End If

        If lblPassword.Text = "New Password" Then
            If (_allUsers.Where(Function(p) p.Id = tbxUserID.Text).Select(Function(p) p.PasswordHash).Contains(EncryptText(tbxPassword.Text))) Then
                MessageBox.Show("You have already used this password. Please choose another one")
            Else
                currentUser.PasswordHash = EncryptText(tbxPassword.Text)
                currentUser.PasswordDate = DateTime.Now
                DataAccess.AddUser(currentUser)
                'Notify the user of the change and reset the form.
                MessageBox.Show(String.Format("Password changed to '{0}'", tbxPassword.Text), "Password Updated", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                lblPassword.Text = "Password"
            End If
        Else
            MessageBox.Show("Please put in your current password to change it", "Passsword needed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End If
    End Sub

    Private Sub tbxPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxPassword.TextChanged
        AssignUser()
    End Sub

    Private Sub tbxUserID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxUserID.TextChanged
        AssignUser()
    End Sub

    Private Sub tbxPassword_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnLogin_Click(sender, e)
        End If
    End Sub

    Private Sub ValidateUser()
        If (tbxUserID.Text = "") OrElse (tbxPassword.Text = "") Then
            Dim message As String = "You must fill out both User name and Password boxes"
            MessageBox.Show(message, "Missing fields", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If

        Dim currentUser As User = _currentUsers.Where(Function(p) p.Id = tbxUserID.Text).SingleOrDefault()
        If (currentUser Is Nothing) Then Return

        If (currentUser.PasswordHash <> EncryptText(tbxPassword.Text)) Then
            Dim message As String = "Invalid password.  Please change your password and try again"
            MessageBox.Show(message, "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If

        If (currentUser.PasswordDate.Date.AddDays(30) > DateTime.Now.Date.AddDays(6)) Then
            If (currentUser.AccessLevel = Constants.AccessLevel.INACTIVE) Then
                Dim message As String = "You do not have access at this time.  Please see system support to gain access"
                MessageBox.Show(message, "Invalid Access", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
                Return
            End If

            If (currentUser.AccessLevel = Constants.AccessLevel.BATCH_PROCESSING) Then
                Dim batchForm As New frmBatch()
                batchForm.Show()
                Me.Hide()
                Return
            End If

            If (currentUser.PasswordHash = EncryptText(Constants.DEFAULT_PASSWORD)) Then
                Dim message As String = "You must change your new password first"
                MessageBox.Show(message, "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                btnUpdate_Click(Me, New EventArgs())
                Return
            End If
        Else
            If (Not _warned) AndAlso (PasswordIsExpired(currentUser)) Then Return
        End If

        Dim regents As New frmRegents(_user)
        regents.Show()
        Me.Hide()
    End Sub

    Private Function EncryptText(ByVal text As String) As String
        'Get a character array from the text string.
        Dim textChars() As Char = text.ToCharArray()
        'Use the ASCII encoder to convert the characters into bytes.
        Dim textBytes() As Byte = System.Text.Encoding.ASCII.GetBytes(textChars)
        'Have SHA1 hash the bytes.
        Dim hashBytes() As Byte = New System.Security.Cryptography.SHA1Managed().ComputeHash(textBytes)
        'Convert the hashed bytes into our return value.
        Return Convert.ToBase64String(hashBytes)
    End Function

    Private Sub AssignUser()
        ResetForm()
        Dim currentUser As User = _currentUsers.Where(Function(p) p.Id = tbxUserID.Text).SingleOrDefault()
        If (currentUser Is Nothing) Then Return

        'TODO: See if we can refactor the process so we're not setting class-level state.
        _user = currentUser

        If currentUser.AccessLevel = "DCR" Then
            btnUserAccess.Enabled = True
            btnUserAccess.Visible = True
        End If
    End Sub

    Private Sub ResetForm()
        btnUserAccess.Enabled = False
        btnUserAccess.Visible = False
    End Sub

    Private Function PasswordIsExpired(ByVal currentUser As User) As Boolean
        _warned = True
        Dim expired As Boolean = False
        Select Case currentUser.PasswordDate.Date.AddDays(30)
            Case DateTime.Now.Date.AddDays(5)
                MessageBox.Show("Your password will expire in 5 days", "Password will expire soon", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Case DateTime.Now.Date.AddDays(4)
                MessageBox.Show("Your password will expire in 4 days", "Password will expire soon", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Case DateTime.Now.Date.AddDays(3)
                MessageBox.Show("Your password will expire in 3 days", "Password will expire soon", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Case DateTime.Now.Date.AddDays(2)
                MessageBox.Show("Your password will expire in 2 days", "Password will expire soon", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Case DateTime.Now.Date.AddDays(1)
                MessageBox.Show("Your password will expire tomorrow", "Password will expire soon", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Case Else
                MessageBox.Show("Your password has expired and needs to be reset", "Update password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                expired = True
                btnUpdate_Click(Me, New EventArgs())
        End Select
        Return expired
    End Function
End Class