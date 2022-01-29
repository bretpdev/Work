Public Class frmLogin
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtUserID As System.Windows.Forms.TextBox
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmLogin))
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtUserID = New System.Windows.Forms.TextBox
        Me.btnLogin = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AccessibleDescription = resources.GetString("Label3.AccessibleDescription")
        Me.Label3.AccessibleName = resources.GetString("Label3.AccessibleName")
        Me.Label3.Anchor = CType(resources.GetObject("Label3.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = CType(resources.GetObject("Label3.AutoSize"), Boolean)
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Dock = CType(resources.GetObject("Label3.Dock"), System.Windows.Forms.DockStyle)
        Me.Label3.Enabled = CType(resources.GetObject("Label3.Enabled"), Boolean)
        Me.Label3.Font = CType(resources.GetObject("Label3.Font"), System.Drawing.Font)
        Me.Label3.Image = CType(resources.GetObject("Label3.Image"), System.Drawing.Image)
        Me.Label3.ImageAlign = CType(resources.GetObject("Label3.ImageAlign"), System.Drawing.ContentAlignment)
        Me.Label3.ImageIndex = CType(resources.GetObject("Label3.ImageIndex"), Integer)
        Me.Label3.ImeMode = CType(resources.GetObject("Label3.ImeMode"), System.Windows.Forms.ImeMode)
        Me.Label3.Location = CType(resources.GetObject("Label3.Location"), System.Drawing.Point)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = CType(resources.GetObject("Label3.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.Label3.Size = CType(resources.GetObject("Label3.Size"), System.Drawing.Size)
        Me.Label3.TabIndex = CType(resources.GetObject("Label3.TabIndex"), Integer)
        Me.Label3.Text = resources.GetString("Label3.Text")
        Me.Label3.TextAlign = CType(resources.GetObject("Label3.TextAlign"), System.Drawing.ContentAlignment)
        Me.Label3.Visible = CType(resources.GetObject("Label3.Visible"), Boolean)
        '
        'Label4
        '
        Me.Label4.AccessibleDescription = resources.GetString("Label4.AccessibleDescription")
        Me.Label4.AccessibleName = resources.GetString("Label4.AccessibleName")
        Me.Label4.Anchor = CType(resources.GetObject("Label4.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = CType(resources.GetObject("Label4.AutoSize"), Boolean)
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Dock = CType(resources.GetObject("Label4.Dock"), System.Windows.Forms.DockStyle)
        Me.Label4.Enabled = CType(resources.GetObject("Label4.Enabled"), Boolean)
        Me.Label4.Font = CType(resources.GetObject("Label4.Font"), System.Drawing.Font)
        Me.Label4.Image = CType(resources.GetObject("Label4.Image"), System.Drawing.Image)
        Me.Label4.ImageAlign = CType(resources.GetObject("Label4.ImageAlign"), System.Drawing.ContentAlignment)
        Me.Label4.ImageIndex = CType(resources.GetObject("Label4.ImageIndex"), Integer)
        Me.Label4.ImeMode = CType(resources.GetObject("Label4.ImeMode"), System.Windows.Forms.ImeMode)
        Me.Label4.Location = CType(resources.GetObject("Label4.Location"), System.Drawing.Point)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = CType(resources.GetObject("Label4.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.Label4.Size = CType(resources.GetObject("Label4.Size"), System.Drawing.Size)
        Me.Label4.TabIndex = CType(resources.GetObject("Label4.TabIndex"), Integer)
        Me.Label4.Text = resources.GetString("Label4.Text")
        Me.Label4.TextAlign = CType(resources.GetObject("Label4.TextAlign"), System.Drawing.ContentAlignment)
        Me.Label4.Visible = CType(resources.GetObject("Label4.Visible"), Boolean)
        '
        'txtPassword
        '
        Me.txtPassword.AccessibleDescription = resources.GetString("txtPassword.AccessibleDescription")
        Me.txtPassword.AccessibleName = resources.GetString("txtPassword.AccessibleName")
        Me.txtPassword.Anchor = CType(resources.GetObject("txtPassword.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.txtPassword.AutoSize = CType(resources.GetObject("txtPassword.AutoSize"), Boolean)
        Me.txtPassword.BackgroundImage = CType(resources.GetObject("txtPassword.BackgroundImage"), System.Drawing.Image)
        Me.txtPassword.Dock = CType(resources.GetObject("txtPassword.Dock"), System.Windows.Forms.DockStyle)
        Me.txtPassword.Enabled = CType(resources.GetObject("txtPassword.Enabled"), Boolean)
        Me.txtPassword.Font = CType(resources.GetObject("txtPassword.Font"), System.Drawing.Font)
        Me.txtPassword.ImeMode = CType(resources.GetObject("txtPassword.ImeMode"), System.Windows.Forms.ImeMode)
        Me.txtPassword.Location = CType(resources.GetObject("txtPassword.Location"), System.Drawing.Point)
        Me.txtPassword.MaxLength = CType(resources.GetObject("txtPassword.MaxLength"), Integer)
        Me.txtPassword.Multiline = CType(resources.GetObject("txtPassword.Multiline"), Boolean)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = CType(resources.GetObject("txtPassword.PasswordChar"), Char)
        Me.txtPassword.RightToLeft = CType(resources.GetObject("txtPassword.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.txtPassword.ScrollBars = CType(resources.GetObject("txtPassword.ScrollBars"), System.Windows.Forms.ScrollBars)
        Me.txtPassword.Size = CType(resources.GetObject("txtPassword.Size"), System.Drawing.Size)
        Me.txtPassword.TabIndex = CType(resources.GetObject("txtPassword.TabIndex"), Integer)
        Me.txtPassword.Text = resources.GetString("txtPassword.Text")
        Me.txtPassword.TextAlign = CType(resources.GetObject("txtPassword.TextAlign"), System.Windows.Forms.HorizontalAlignment)
        Me.txtPassword.Visible = CType(resources.GetObject("txtPassword.Visible"), Boolean)
        Me.txtPassword.WordWrap = CType(resources.GetObject("txtPassword.WordWrap"), Boolean)
        '
        'txtUserID
        '
        Me.txtUserID.AccessibleDescription = resources.GetString("txtUserID.AccessibleDescription")
        Me.txtUserID.AccessibleName = resources.GetString("txtUserID.AccessibleName")
        Me.txtUserID.Anchor = CType(resources.GetObject("txtUserID.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.txtUserID.AutoSize = CType(resources.GetObject("txtUserID.AutoSize"), Boolean)
        Me.txtUserID.BackgroundImage = CType(resources.GetObject("txtUserID.BackgroundImage"), System.Drawing.Image)
        Me.txtUserID.Dock = CType(resources.GetObject("txtUserID.Dock"), System.Windows.Forms.DockStyle)
        Me.txtUserID.Enabled = CType(resources.GetObject("txtUserID.Enabled"), Boolean)
        Me.txtUserID.Font = CType(resources.GetObject("txtUserID.Font"), System.Drawing.Font)
        Me.txtUserID.ImeMode = CType(resources.GetObject("txtUserID.ImeMode"), System.Windows.Forms.ImeMode)
        Me.txtUserID.Location = CType(resources.GetObject("txtUserID.Location"), System.Drawing.Point)
        Me.txtUserID.MaxLength = CType(resources.GetObject("txtUserID.MaxLength"), Integer)
        Me.txtUserID.Multiline = CType(resources.GetObject("txtUserID.Multiline"), Boolean)
        Me.txtUserID.Name = "txtUserID"
        Me.txtUserID.PasswordChar = CType(resources.GetObject("txtUserID.PasswordChar"), Char)
        Me.txtUserID.RightToLeft = CType(resources.GetObject("txtUserID.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.txtUserID.ScrollBars = CType(resources.GetObject("txtUserID.ScrollBars"), System.Windows.Forms.ScrollBars)
        Me.txtUserID.Size = CType(resources.GetObject("txtUserID.Size"), System.Drawing.Size)
        Me.txtUserID.TabIndex = CType(resources.GetObject("txtUserID.TabIndex"), Integer)
        Me.txtUserID.Text = resources.GetString("txtUserID.Text")
        Me.txtUserID.TextAlign = CType(resources.GetObject("txtUserID.TextAlign"), System.Windows.Forms.HorizontalAlignment)
        Me.txtUserID.Visible = CType(resources.GetObject("txtUserID.Visible"), Boolean)
        Me.txtUserID.WordWrap = CType(resources.GetObject("txtUserID.WordWrap"), Boolean)
        '
        'btnLogin
        '
        Me.btnLogin.AccessibleDescription = resources.GetString("btnLogin.AccessibleDescription")
        Me.btnLogin.AccessibleName = resources.GetString("btnLogin.AccessibleName")
        Me.btnLogin.Anchor = CType(resources.GetObject("btnLogin.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.btnLogin.BackgroundImage = CType(resources.GetObject("btnLogin.BackgroundImage"), System.Drawing.Image)
        Me.btnLogin.Dock = CType(resources.GetObject("btnLogin.Dock"), System.Windows.Forms.DockStyle)
        Me.btnLogin.Enabled = CType(resources.GetObject("btnLogin.Enabled"), Boolean)
        Me.btnLogin.FlatStyle = CType(resources.GetObject("btnLogin.FlatStyle"), System.Windows.Forms.FlatStyle)
        Me.btnLogin.Font = CType(resources.GetObject("btnLogin.Font"), System.Drawing.Font)
        Me.btnLogin.Image = CType(resources.GetObject("btnLogin.Image"), System.Drawing.Image)
        Me.btnLogin.ImageAlign = CType(resources.GetObject("btnLogin.ImageAlign"), System.Drawing.ContentAlignment)
        Me.btnLogin.ImageIndex = CType(resources.GetObject("btnLogin.ImageIndex"), Integer)
        Me.btnLogin.ImeMode = CType(resources.GetObject("btnLogin.ImeMode"), System.Windows.Forms.ImeMode)
        Me.btnLogin.Location = CType(resources.GetObject("btnLogin.Location"), System.Drawing.Point)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.RightToLeft = CType(resources.GetObject("btnLogin.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.btnLogin.Size = CType(resources.GetObject("btnLogin.Size"), System.Drawing.Size)
        Me.btnLogin.TabIndex = CType(resources.GetObject("btnLogin.TabIndex"), Integer)
        Me.btnLogin.Text = resources.GetString("btnLogin.Text")
        Me.btnLogin.TextAlign = CType(resources.GetObject("btnLogin.TextAlign"), System.Drawing.ContentAlignment)
        Me.btnLogin.Visible = CType(resources.GetObject("btnLogin.Visible"), Boolean)
        '
        'Label1
        '
        Me.Label1.AccessibleDescription = resources.GetString("Label1.AccessibleDescription")
        Me.Label1.AccessibleName = resources.GetString("Label1.AccessibleName")
        Me.Label1.Anchor = CType(resources.GetObject("Label1.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = CType(resources.GetObject("Label1.AutoSize"), Boolean)
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Dock = CType(resources.GetObject("Label1.Dock"), System.Windows.Forms.DockStyle)
        Me.Label1.Enabled = CType(resources.GetObject("Label1.Enabled"), Boolean)
        Me.Label1.Font = CType(resources.GetObject("Label1.Font"), System.Drawing.Font)
        Me.Label1.Image = CType(resources.GetObject("Label1.Image"), System.Drawing.Image)
        Me.Label1.ImageAlign = CType(resources.GetObject("Label1.ImageAlign"), System.Drawing.ContentAlignment)
        Me.Label1.ImageIndex = CType(resources.GetObject("Label1.ImageIndex"), Integer)
        Me.Label1.ImeMode = CType(resources.GetObject("Label1.ImeMode"), System.Windows.Forms.ImeMode)
        Me.Label1.Location = CType(resources.GetObject("Label1.Location"), System.Drawing.Point)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = CType(resources.GetObject("Label1.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.Label1.Size = CType(resources.GetObject("Label1.Size"), System.Drawing.Size)
        Me.Label1.TabIndex = CType(resources.GetObject("Label1.TabIndex"), Integer)
        Me.Label1.Text = resources.GetString("Label1.Text")
        Me.Label1.TextAlign = CType(resources.GetObject("Label1.TextAlign"), System.Drawing.ContentAlignment)
        Me.Label1.Visible = CType(resources.GetObject("Label1.Visible"), Boolean)
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleDescription = resources.GetString("btnCancel.AccessibleDescription")
        Me.btnCancel.AccessibleName = resources.GetString("btnCancel.AccessibleName")
        Me.btnCancel.Anchor = CType(resources.GetObject("btnCancel.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = CType(resources.GetObject("btnCancel.BackgroundImage"), System.Drawing.Image)
        Me.btnCancel.Dock = CType(resources.GetObject("btnCancel.Dock"), System.Windows.Forms.DockStyle)
        Me.btnCancel.Enabled = CType(resources.GetObject("btnCancel.Enabled"), Boolean)
        Me.btnCancel.FlatStyle = CType(resources.GetObject("btnCancel.FlatStyle"), System.Windows.Forms.FlatStyle)
        Me.btnCancel.Font = CType(resources.GetObject("btnCancel.Font"), System.Drawing.Font)
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = CType(resources.GetObject("btnCancel.ImageAlign"), System.Drawing.ContentAlignment)
        Me.btnCancel.ImageIndex = CType(resources.GetObject("btnCancel.ImageIndex"), Integer)
        Me.btnCancel.ImeMode = CType(resources.GetObject("btnCancel.ImeMode"), System.Windows.Forms.ImeMode)
        Me.btnCancel.Location = CType(resources.GetObject("btnCancel.Location"), System.Drawing.Point)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.RightToLeft = CType(resources.GetObject("btnCancel.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.btnCancel.Size = CType(resources.GetObject("btnCancel.Size"), System.Drawing.Size)
        Me.btnCancel.TabIndex = CType(resources.GetObject("btnCancel.TabIndex"), Integer)
        Me.btnCancel.Text = resources.GetString("btnCancel.Text")
        Me.btnCancel.TextAlign = CType(resources.GetObject("btnCancel.TextAlign"), System.Drawing.ContentAlignment)
        Me.btnCancel.Visible = CType(resources.GetObject("btnCancel.Visible"), Boolean)
        '
        'PictureBox1
        '
        Me.PictureBox1.AccessibleDescription = resources.GetString("PictureBox1.AccessibleDescription")
        Me.PictureBox1.AccessibleName = resources.GetString("PictureBox1.AccessibleName")
        Me.PictureBox1.Anchor = CType(resources.GetObject("PictureBox1.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.Dock = CType(resources.GetObject("PictureBox1.Dock"), System.Windows.Forms.DockStyle)
        Me.PictureBox1.Enabled = CType(resources.GetObject("PictureBox1.Enabled"), Boolean)
        Me.PictureBox1.Font = CType(resources.GetObject("PictureBox1.Font"), System.Drawing.Font)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.ImeMode = CType(resources.GetObject("PictureBox1.ImeMode"), System.Windows.Forms.ImeMode)
        Me.PictureBox1.Location = CType(resources.GetObject("PictureBox1.Location"), System.Drawing.Point)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.RightToLeft = CType(resources.GetObject("PictureBox1.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.PictureBox1.Size = CType(resources.GetObject("PictureBox1.Size"), System.Drawing.Size)
        Me.PictureBox1.SizeMode = CType(resources.GetObject("PictureBox1.SizeMode"), System.Windows.Forms.PictureBoxSizeMode)
        Me.PictureBox1.TabIndex = CType(resources.GetObject("PictureBox1.TabIndex"), Integer)
        Me.PictureBox1.TabStop = False
        Me.PictureBox1.Text = resources.GetString("PictureBox1.Text")
        Me.PictureBox1.Visible = CType(resources.GetObject("PictureBox1.Visible"), Boolean)
        '
        'frmLogin
        '
        Me.AcceptButton = Me.btnLogin
        Me.AccessibleDescription = resources.GetString("$this.AccessibleDescription")
        Me.AccessibleName = resources.GetString("$this.AccessibleName")
        Me.AutoScaleBaseSize = CType(resources.GetObject("$this.AutoScaleBaseSize"), System.Drawing.Size)
        Me.AutoScroll = CType(resources.GetObject("$this.AutoScroll"), Boolean)
        Me.AutoScrollMargin = CType(resources.GetObject("$this.AutoScrollMargin"), System.Drawing.Size)
        Me.AutoScrollMinSize = CType(resources.GetObject("$this.AutoScrollMinSize"), System.Drawing.Size)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = CType(resources.GetObject("$this.ClientSize"), System.Drawing.Size)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUserID)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Enabled = CType(resources.GetObject("$this.Enabled"), Boolean)
        Me.Font = CType(resources.GetObject("$this.Font"), System.Drawing.Font)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.ImeMode = CType(resources.GetObject("$this.ImeMode"), System.Windows.Forms.ImeMode)
        Me.Location = CType(resources.GetObject("$this.Location"), System.Drawing.Point)
        Me.MaximizeBox = False
        Me.MaximumSize = CType(resources.GetObject("$this.MaximumSize"), System.Drawing.Size)
        Me.MinimizeBox = False
        Me.MinimumSize = CType(resources.GetObject("$this.MinimumSize"), System.Drawing.Size)
        Me.Name = "frmLogin"
        Me.RightToLeft = CType(resources.GetObject("$this.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.StartPosition = CType(resources.GetObject("$this.StartPosition"), System.Windows.Forms.FormStartPosition)
        Me.Text = resources.GetString("$this.Text")
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private sqlCmd As New SqlClient.SqlCommand
    Private sqlRdr As SqlClient.SqlDataReader
    Private PW As New PWEncryption

    'verify user name and password
    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Dim Pswd As String

        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'warn the user if the user id was not found
        sqlCmd.CommandText = "SELECT * FROM UserInfo WHERE UserID = '" & txtUserID.Text & "'"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()

        If Not sqlRdr.HasRows Then
            MsgBox("The user ID entered was not found.  Please contact the Systems Support Help Desk for assistance.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            sqlRdr.Close()
            dbConnection.Close()
            Exit Sub
        End If

        'decrypt password
        Pswd = PW.DecryptString(sqlRdr("Pswd"))

        'warn the user if the password was not correct
        If Pswd <> txtPassword.Text Then
            MsgBox("The password entered was not correct.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            sqlRdr.Close()
            dbConnection.Close()
            Exit Sub
        End If

        UserID = txtUserID.Text
        UserAccess = sqlRdr("AccessLevel")

        'prompt the user to change the password if it has expired
        If Pswd = "welcome1" Or DateValue(DateAdd(DateInterval.Day, 30, sqlRdr("PswdCreateDt"))) < DateValue(Today) Then
            MsgBox("The password has expired.  You must enter a new password.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            sqlRdr.Close()
            dbConnection.Close()
            ShowForms.ChangePassword(True)
            dbConnection.Open()
        Else
            sqlRdr.Close()
        End If

        'update user log
        sqlCmd.CommandText = "INSERT INTO UserLog (UserID, LogIn) VALUES ('" & UserID & "', GETDATE())"
        sqlCmd.ExecuteNonQuery()
        sqlRdr.Close()

        dbConnection.Close()

        'display the main menu
        ShowForms.NCSP()
        Me.Hide()
    End Sub

    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetdbConnection()
        Me.AcceptButton = btnLogin
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Application.Exit()
    End Sub

    '#Region " Conversion "
    '    Sub addSchedIDToTransactions()

    '        Dim DA As New SqlClient.SqlDataAdapter("select AcctID, SchedID,InstAtt,Semester,SchedYr from Schedule where RowStatus = 'A'", dbConnection)
    '        Dim DS As New DataSet
    '        DA.Fill(DS)
    '        Dim r As DataRow
    '        sqlCmd.Connection = dbConnection

    '        For Each r In DS.Tables(0).Rows

    '            sqlCmd.CommandText = "update [Transaction] set SchedID = " & r.Item("SchedID") & " where SchedInst = '" & getInstId(r.Item("InstAtt")) & "' and SchedSemEnr = '" & r.Item("Semester") & "' and SchedYrEnr = '" & r.Item("SchedYr") & "' and AcctID = " & r.Item("AcctID")
    '            dbConnection.Open()
    '            sqlCmd.ExecuteNonQuery()
    '            dbConnection.Close()
    '        Next
    '        MsgBox("Finished Conversion")
    '        End

    '    End Sub

    '    Function getInstId(ByVal inst As String) As String
    '        Dim str As String
    '        sqlCmd.Connection = dbConnection
    '        dbConnection.Open()
    '        sqlCmd.CommandText = "SELECT InstID FROM Inst where InstLong = '" & inst & "'"
    '        sqlRdr = sqlCmd.ExecuteReader
    '        sqlRdr.Read()
    '        If sqlRdr.HasRows Then
    '            str = sqlRdr("InstID")
    '        End If
    '        sqlRdr.Close()
    '        dbConnection.Close()
    '        Return str
    '    End Function

    '#End Region

End Class
