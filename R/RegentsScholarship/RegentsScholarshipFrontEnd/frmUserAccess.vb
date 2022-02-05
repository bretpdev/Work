Imports RegentsScholarshipBackEnd

Public Class frmUserAccess
    'Checking and unchecking the box for "Default Password" should flip between the default password
    'and the selected user's password. To enable that, we need to track the selected user's password.
    Private _selectedUserPassword As String

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmUserAccess_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If (Constants.TEST_MODE) Then Me.Text += " -- TEST MODE"
        'Populate the Access Level combo box with all available access levels.
        cmbAccessLevel.DataSource = Lookups.AccessLevels.OrderBy(Function(p) p).ToList()
        'Populate the User Name combo box with distinct user names, plus a blank entry.
        cmbUserName.Items.AddRange(DataAccess.GetAllUsers().Select(Function(p) p.Id).Distinct().OrderBy(Function(p) p).ToArray())
        cmbUserName.Items.Insert(0, "")
    End Sub

    Private Sub btnAddUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddUser.Click
        AddUser()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        DeleteUser()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        UpdateUser()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cbxDefault_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxDefault.CheckedChanged
        If (cbxDefault.Checked) Then
            txtPassword.Text = Constants.DEFAULT_PASSWORD
        ElseIf (Not String.IsNullOrEmpty(_selectedUserPassword)) Then
            txtPassword.Text = _selectedUserPassword
        Else
            txtPassword.Text = ""
        End If
    End Sub

    Private Sub cmbUserName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbUserName.SelectedIndexChanged
        CheckForDatabaseMatch()
    End Sub

    Private Sub cmbUserName_TextUpdate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbUserName.TextUpdate
        CheckForDatabaseMatch()
    End Sub

    Private Sub CheckForDatabaseMatch()
        Dim matchingUser As User = DataAccess.GetCurrentUserRecord(cmbUserName.Text)
        If (matchingUser Is Nothing) Then
            txtPassword.Text = ""
            cmbAccessLevel.Text = Constants.AccessLevel.INACTIVE
        Else
            _selectedUserPassword = matchingUser.PasswordHash
            txtPassword.Text = matchingUser.PasswordHash
            cmbAccessLevel.Text = matchingUser.AccessLevel
        End If
    End Sub

    Private Sub AddUser()
        Dim newUser As New User()
        newUser.Id = cmbUserName.Text
        newUser.PasswordHash = EncryptText(txtPassword.Text)
        newUser.PasswordDate = Date.Now
        newUser.AccessLevel = cmbAccessLevel.Text
        DataAccess.AddUser(newUser)

        Dim message As String = "User successfully added."
        If (cmbAccessLevel.Text = Constants.AccessLevel.BATCH_PROCESSING) Then
            message += Environment.NewLine + "Make sure the DataMatrix program is installed on the user's computer."
        End If
        MessageBox.Show(message, "User Added", MessageBoxButtons.OK, MessageBoxIcon.Information)
        btnAddUser.Enabled = False
    End Sub

    Private Sub DeleteUser()
        Dim newUser As New User()
        newUser.Id = cmbUserName.Text
        newUser.PasswordDate = Date.Now
        newUser.PasswordHash = txtPassword.Text
        newUser.AccessLevel = Constants.AccessLevel.INACTIVE
        DataAccess.AddUser(newUser)

        MessageBox.Show("User access deleted")
        btnAddUser.Enabled = False
    End Sub

    Private Sub UpdateUser()
        Dim currentPasswordHash As String = DataAccess.GetCurrentUserRecord(cmbUserName.Text).PasswordHash
        Dim newUser As New User()
        newUser.AccessLevel = cmbAccessLevel.Text
        If (txtPassword.Text = currentPasswordHash) Then
            newUser.PasswordHash = txtPassword.Text
        Else
            newUser.PasswordHash = EncryptText(txtPassword.Text)
        End If
        newUser.Id = cmbUserName.Text
        newUser.PasswordDate = Date.Now
        DataAccess.AddUser(newUser)

        Dim message As String = "User information updated."
        If (cmbAccessLevel.Text = Constants.AccessLevel.BATCH_PROCESSING) Then
            message += Environment.NewLine + "Make sure the DataMatrix program is installed on the user's computer."
        End If
        MessageBox.Show(message, "User updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
        btnAddUser.Enabled = False
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
End Class