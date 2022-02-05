Imports System.Data.SqlClient

Public Class frmLogIn
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal tTestMode As Boolean)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        FrmCancelled = True
        TestMode = tTestMode
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lab1 As System.Windows.Forms.Label
    Friend WithEvents tbPW As System.Windows.Forms.TextBox
    Friend WithEvents tbUID As System.Windows.Forms.TextBox
    Friend WithEvents tbConfirmPW As System.Windows.Forms.TextBox
    Friend WithEvents tbNewPW As System.Windows.Forms.TextBox
    Friend WithEvents btnUpdateAndLogIn As System.Windows.Forms.Button
    Friend WithEvents btnLogIn As System.Windows.Forms.Button
    Friend WithEvents gbNewPW As System.Windows.Forms.GroupBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmLogIn))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnLogIn = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbPW = New System.Windows.Forms.TextBox
        Me.tbUID = New System.Windows.Forms.TextBox
        Me.gbNewPW = New System.Windows.Forms.GroupBox
        Me.btnUpdateAndLogIn = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.lab1 = New System.Windows.Forms.Label
        Me.tbConfirmPW = New System.Windows.Forms.TextBox
        Me.tbNewPW = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.gbNewPW.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnLogIn)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.tbPW)
        Me.GroupBox1.Controls.Add(Me.tbUID)
        Me.GroupBox1.Controls.Add(Me.gbNewPW)
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(328, 188)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Log In:"
        '
        'btnLogIn
        '
        Me.btnLogIn.Location = New System.Drawing.Point(108, 64)
        Me.btnLogIn.Name = "btnLogIn"
        Me.btnLogIn.Size = New System.Drawing.Size(113, 23)
        Me.btnLogIn.TabIndex = 2
        Me.btnLogIn.Text = "Log In"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(16, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Password:"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "User ID:"
        '
        'tbPW
        '
        Me.tbPW.Location = New System.Drawing.Point(120, 40)
        Me.tbPW.MaxLength = 20
        Me.tbPW.Name = "tbPW"
        Me.tbPW.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPW.Size = New System.Drawing.Size(192, 20)
        Me.tbPW.TabIndex = 1
        Me.tbPW.Text = ""
        '
        'tbUID
        '
        Me.tbUID.Location = New System.Drawing.Point(120, 16)
        Me.tbUID.Name = "tbUID"
        Me.tbUID.Size = New System.Drawing.Size(192, 20)
        Me.tbUID.TabIndex = 0
        Me.tbUID.Text = ""
        '
        'gbNewPW
        '
        Me.gbNewPW.BackColor = System.Drawing.SystemColors.Control
        Me.gbNewPW.Controls.Add(Me.btnUpdateAndLogIn)
        Me.gbNewPW.Controls.Add(Me.Label3)
        Me.gbNewPW.Controls.Add(Me.lab1)
        Me.gbNewPW.Controls.Add(Me.tbConfirmPW)
        Me.gbNewPW.Controls.Add(Me.tbNewPW)
        Me.gbNewPW.Enabled = False
        Me.gbNewPW.Location = New System.Drawing.Point(4, 92)
        Me.gbNewPW.Name = "gbNewPW"
        Me.gbNewPW.Size = New System.Drawing.Size(320, 92)
        Me.gbNewPW.TabIndex = 3
        Me.gbNewPW.TabStop = False
        Me.gbNewPW.Text = "New User/Update Password:"
        '
        'btnUpdateAndLogIn
        '
        Me.btnUpdateAndLogIn.Location = New System.Drawing.Point(108, 64)
        Me.btnUpdateAndLogIn.Name = "btnUpdateAndLogIn"
        Me.btnUpdateAndLogIn.Size = New System.Drawing.Size(113, 23)
        Me.btnUpdateAndLogIn.TabIndex = 6
        Me.btnUpdateAndLogIn.Text = "Update and Log In"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(16, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 16)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Confirm Password:"
        '
        'lab1
        '
        Me.lab1.Location = New System.Drawing.Point(16, 20)
        Me.lab1.Name = "lab1"
        Me.lab1.Size = New System.Drawing.Size(100, 16)
        Me.lab1.TabIndex = 6
        Me.lab1.Text = "New Password:"
        '
        'tbConfirmPW
        '
        Me.tbConfirmPW.Location = New System.Drawing.Point(120, 40)
        Me.tbConfirmPW.MaxLength = 20
        Me.tbConfirmPW.Name = "tbConfirmPW"
        Me.tbConfirmPW.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbConfirmPW.Size = New System.Drawing.Size(192, 20)
        Me.tbConfirmPW.TabIndex = 5
        Me.tbConfirmPW.Text = ""
        '
        'tbNewPW
        '
        Me.tbNewPW.Location = New System.Drawing.Point(120, 16)
        Me.tbNewPW.MaxLength = 20
        Me.tbNewPW.Name = "tbNewPW"
        Me.tbNewPW.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbNewPW.Size = New System.Drawing.Size(192, 20)
        Me.tbNewPW.TabIndex = 4
        Me.tbNewPW.Text = ""
        '
        'frmLogIn
        '
        Me.AcceptButton = Me.btnLogIn
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(328, 188)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLogIn"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TILP System Log In"
        Me.GroupBox1.ResumeLayout(False)
        Me.gbNewPW.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FrmCancelled As Boolean
    Private TestMode As Boolean
    Private Conn As SqlConnection
    Private Comm As New SqlCommand
    Private TheUser As user
    Private userNameReq As String

    Private Sub btnLogIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogIn.Click
        'make sure that the user provided a user ID
        If tbUID.TextLength = 0 Then
            MessageBox.Show("You must provide a user ID.  Please try again.", "User ID Needed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        'make sure that the user provided a password
        If tbPW.TextLength = 0 Then
            MessageBox.Show("You must provide a password when trying to log in.  Please try again.", "Password Needed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If OK4Access(False) Then
            FrmCancelled = False
            Me.Hide()
        End If
    End Sub

    Private Sub btnUpdateAndLogIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateAndLogIn.Click
        ''make sure that the user provided a user ID
        If tbUID.TextLength = 0 Then
            MessageBox.Show("You must provide a user ID.  Please try again.", "User ID Needed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        'make sure that the user provided a password
        If tbNewPW.TextLength = 0 Or tbConfirmPW.TextLength = 0 Then
            MessageBox.Show("You must provide a new password and confirm that password when trying to set up or update a password.  Please try again.", "New Password Needed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If OK4Access(True) Then
            FrmCancelled = False
            Me.Hide()
        End If
    End Sub

    'does DB checks and collects access levels
    Private Function OK4Access(ByVal Update As Boolean) As Boolean
        Dim Reader As SqlDataReader
        Try
            'check if user is even in the DB
            Comm.Connection.Open()
            Comm.CommandText = "SELECT * FROM UserDat WHERE UserID = '" + tbUID.Text + "' AND Valid = 1 AND Password = '" + New PWEncryption().EncryptString(tbPW.Text) + "'"
            Reader = Comm.ExecuteReader
            'check for authentication
            If Reader.Read() = False Then
                Comm.Connection.Close()
                MessageBox.Show("Access denied!  Either your log on information was incorrect or user access hasn't been set up yet.  Please contact Systems Support for assistance.", "Access Denied!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            Else
                'get user data
                TheUser = New user(CStr(Reader("UserID")), CInt(Reader("AuthLevel")))
                userNameReq = Reader("UserID")
            End If
            'if user has default password then they must give a new password
            If Reader("Password") = "rIQXpOFj2fmdGw2xbTdRWg==" And Update = False Then 'default password is "rIQXpOFj2fmdGw2xbTdRWg==" which is "welcome1" encrypted
                MessageBox.Show("Please provide a new password and confirm that new password.", "New Password Needed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Conn.Close()
                Return False
            End If
            Reader.Close()
            'is the password being setup/updated or is the user just logging in
            If Update Then
                'be sure the new and confirmation text equal
                If tbNewPW.Text <> tbConfirmPW.Text Then
                    MessageBox.Show("The new password you provided is not the same as the confirmation password you provided.", "No Match For New Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Comm.Connection.Close()
                    Return False
                End If
                'update password
                Comm.CommandText = "UPDATE UserDat SET Password = '" + New PWEncryption().EncryptString(tbNewPW.Text) + "' WHERE UserID = '" + tbUID.Text + "'"
                Comm.ExecuteNonQuery()
                Comm.Connection.Close()
                Return True 'authenticated and password update complete
            Else
                Return True 'authenticated and no password update needed
            End If
        Catch ex As Exception
            MessageBox.Show("An error occured while trying to confirm your user access to the TILP system.  Please contact Systems Support for assistance.", "Access Confirmation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
        'the code should always exit the function before getting here
        MessageBox.Show("An fatal error occurred while trying to process request.  Please contact Systems Support.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End
    End Function

    Public Function FormWasCancelled() As Boolean
        Return FrmCancelled
    End Function

    Private Sub frmLogIn_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'set up DB connection
        If TestMode Then
            'if in test mode
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            'if in live mode
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        Comm.Connection = Conn
    End Sub

    Public Function GetUID() As String
        Return TheUser.GetUID()
    End Function

    Public Function GetAccessLvl() As Integer
        Return TheUser.GetAccessLevel()
    End Function

    Private Sub tbUID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbUID.TextChanged
        If tbUID.TextLength > 0 And tbPW.TextLength > 0 Then
            gbNewPW.Enabled = True
        Else
            gbNewPW.Enabled = False
        End If
    End Sub

    Private Sub tbPW_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbPW.TextChanged
        If tbUID.TextLength > 0 And tbPW.TextLength > 0 Then
            gbNewPW.Enabled = True
        Else
            gbNewPW.Enabled = False
        End If
    End Sub

    Public Property UserNameProp() As String
        Get
            Return userNameReq
        End Get
        Set(ByVal value As String)
            userNameReq = value
        End Set
    End Property
End Class
