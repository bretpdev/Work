Public Class frmPassword
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
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtConfirmPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmPassword))
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtConfirmPassword = New System.Windows.Forms.TextBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.btnChange = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AccessibleDescription = resources.GetString("Label4.AccessibleDescription")
        Me.Label4.AccessibleName = resources.GetString("Label4.AccessibleName")
        Me.Label4.Anchor = CType(resources.GetObject("Label4.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = CType(resources.GetObject("Label4.AutoSize"), Boolean)
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
        'txtConfirmPassword
        '
        Me.txtConfirmPassword.AccessibleDescription = resources.GetString("txtConfirmPassword.AccessibleDescription")
        Me.txtConfirmPassword.AccessibleName = resources.GetString("txtConfirmPassword.AccessibleName")
        Me.txtConfirmPassword.Anchor = CType(resources.GetObject("txtConfirmPassword.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.txtConfirmPassword.AutoSize = CType(resources.GetObject("txtConfirmPassword.AutoSize"), Boolean)
        Me.txtConfirmPassword.BackgroundImage = CType(resources.GetObject("txtConfirmPassword.BackgroundImage"), System.Drawing.Image)
        Me.txtConfirmPassword.Dock = CType(resources.GetObject("txtConfirmPassword.Dock"), System.Windows.Forms.DockStyle)
        Me.txtConfirmPassword.Enabled = CType(resources.GetObject("txtConfirmPassword.Enabled"), Boolean)
        Me.txtConfirmPassword.Font = CType(resources.GetObject("txtConfirmPassword.Font"), System.Drawing.Font)
        Me.txtConfirmPassword.ImeMode = CType(resources.GetObject("txtConfirmPassword.ImeMode"), System.Windows.Forms.ImeMode)
        Me.txtConfirmPassword.Location = CType(resources.GetObject("txtConfirmPassword.Location"), System.Drawing.Point)
        Me.txtConfirmPassword.MaxLength = CType(resources.GetObject("txtConfirmPassword.MaxLength"), Integer)
        Me.txtConfirmPassword.Multiline = CType(resources.GetObject("txtConfirmPassword.Multiline"), Boolean)
        Me.txtConfirmPassword.Name = "txtConfirmPassword"
        Me.txtConfirmPassword.PasswordChar = CType(resources.GetObject("txtConfirmPassword.PasswordChar"), Char)
        Me.txtConfirmPassword.RightToLeft = CType(resources.GetObject("txtConfirmPassword.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.txtConfirmPassword.ScrollBars = CType(resources.GetObject("txtConfirmPassword.ScrollBars"), System.Windows.Forms.ScrollBars)
        Me.txtConfirmPassword.Size = CType(resources.GetObject("txtConfirmPassword.Size"), System.Drawing.Size)
        Me.txtConfirmPassword.TabIndex = CType(resources.GetObject("txtConfirmPassword.TabIndex"), Integer)
        Me.txtConfirmPassword.Text = resources.GetString("txtConfirmPassword.Text")
        Me.txtConfirmPassword.TextAlign = CType(resources.GetObject("txtConfirmPassword.TextAlign"), System.Windows.Forms.HorizontalAlignment)
        Me.txtConfirmPassword.Visible = CType(resources.GetObject("txtConfirmPassword.Visible"), Boolean)
        Me.txtConfirmPassword.WordWrap = CType(resources.GetObject("txtConfirmPassword.WordWrap"), Boolean)
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
        'btnChange
        '
        Me.btnChange.AccessibleDescription = resources.GetString("btnChange.AccessibleDescription")
        Me.btnChange.AccessibleName = resources.GetString("btnChange.AccessibleName")
        Me.btnChange.Anchor = CType(resources.GetObject("btnChange.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.btnChange.BackgroundImage = CType(resources.GetObject("btnChange.BackgroundImage"), System.Drawing.Image)
        Me.btnChange.Dock = CType(resources.GetObject("btnChange.Dock"), System.Windows.Forms.DockStyle)
        Me.btnChange.Enabled = CType(resources.GetObject("btnChange.Enabled"), Boolean)
        Me.btnChange.FlatStyle = CType(resources.GetObject("btnChange.FlatStyle"), System.Windows.Forms.FlatStyle)
        Me.btnChange.Font = CType(resources.GetObject("btnChange.Font"), System.Drawing.Font)
        Me.btnChange.Image = CType(resources.GetObject("btnChange.Image"), System.Drawing.Image)
        Me.btnChange.ImageAlign = CType(resources.GetObject("btnChange.ImageAlign"), System.Drawing.ContentAlignment)
        Me.btnChange.ImageIndex = CType(resources.GetObject("btnChange.ImageIndex"), Integer)
        Me.btnChange.ImeMode = CType(resources.GetObject("btnChange.ImeMode"), System.Windows.Forms.ImeMode)
        Me.btnChange.Location = CType(resources.GetObject("btnChange.Location"), System.Drawing.Point)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.RightToLeft = CType(resources.GetObject("btnChange.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.btnChange.Size = CType(resources.GetObject("btnChange.Size"), System.Drawing.Size)
        Me.btnChange.TabIndex = CType(resources.GetObject("btnChange.TabIndex"), Integer)
        Me.btnChange.Text = resources.GetString("btnChange.Text")
        Me.btnChange.TextAlign = CType(resources.GetObject("btnChange.TextAlign"), System.Drawing.ContentAlignment)
        Me.btnChange.Visible = CType(resources.GetObject("btnChange.Visible"), Boolean)
        '
        'Label1
        '
        Me.Label1.AccessibleDescription = resources.GetString("Label1.AccessibleDescription")
        Me.Label1.AccessibleName = resources.GetString("Label1.AccessibleName")
        Me.Label1.Anchor = CType(resources.GetObject("Label1.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = CType(resources.GetObject("Label1.AutoSize"), Boolean)
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
        'Label2
        '
        Me.Label2.AccessibleDescription = resources.GetString("Label2.AccessibleDescription")
        Me.Label2.AccessibleName = resources.GetString("Label2.AccessibleName")
        Me.Label2.Anchor = CType(resources.GetObject("Label2.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = CType(resources.GetObject("Label2.AutoSize"), Boolean)
        Me.Label2.Dock = CType(resources.GetObject("Label2.Dock"), System.Windows.Forms.DockStyle)
        Me.Label2.Enabled = CType(resources.GetObject("Label2.Enabled"), Boolean)
        Me.Label2.Font = CType(resources.GetObject("Label2.Font"), System.Drawing.Font)
        Me.Label2.Image = CType(resources.GetObject("Label2.Image"), System.Drawing.Image)
        Me.Label2.ImageAlign = CType(resources.GetObject("Label2.ImageAlign"), System.Drawing.ContentAlignment)
        Me.Label2.ImageIndex = CType(resources.GetObject("Label2.ImageIndex"), Integer)
        Me.Label2.ImeMode = CType(resources.GetObject("Label2.ImeMode"), System.Windows.Forms.ImeMode)
        Me.Label2.Location = CType(resources.GetObject("Label2.Location"), System.Drawing.Point)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = CType(resources.GetObject("Label2.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.Label2.Size = CType(resources.GetObject("Label2.Size"), System.Drawing.Size)
        Me.Label2.TabIndex = CType(resources.GetObject("Label2.TabIndex"), Integer)
        Me.Label2.Text = resources.GetString("Label2.Text")
        Me.Label2.TextAlign = CType(resources.GetObject("Label2.TextAlign"), System.Drawing.ContentAlignment)
        Me.Label2.Visible = CType(resources.GetObject("Label2.Visible"), Boolean)
        '
        'frmPassword
        '
        Me.AccessibleDescription = resources.GetString("$this.AccessibleDescription")
        Me.AccessibleName = resources.GetString("$this.AccessibleName")
        Me.AutoScaleBaseSize = CType(resources.GetObject("$this.AutoScaleBaseSize"), System.Drawing.Size)
        Me.AutoScroll = CType(resources.GetObject("$this.AutoScroll"), Boolean)
        Me.AutoScrollMargin = CType(resources.GetObject("$this.AutoScrollMargin"), System.Drawing.Size)
        Me.AutoScrollMinSize = CType(resources.GetObject("$this.AutoScrollMinSize"), System.Drawing.Size)
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = CType(resources.GetObject("$this.ClientSize"), System.Drawing.Size)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnChange)
        Me.Controls.Add(Me.txtConfirmPassword)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.Label4)
        Me.Enabled = CType(resources.GetObject("$this.Enabled"), Boolean)
        Me.Font = CType(resources.GetObject("$this.Font"), System.Drawing.Font)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.ImeMode = CType(resources.GetObject("$this.ImeMode"), System.Windows.Forms.ImeMode)
        Me.Location = CType(resources.GetObject("$this.Location"), System.Drawing.Point)
        Me.MaximizeBox = False
        Me.MaximumSize = CType(resources.GetObject("$this.MaximumSize"), System.Drawing.Size)
        Me.MinimizeBox = False
        Me.MinimumSize = CType(resources.GetObject("$this.MinimumSize"), System.Drawing.Size)
        Me.Name = "frmPassword"
        Me.RightToLeft = CType(resources.GetObject("$this.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.ShowInTaskbar = False
        Me.StartPosition = CType(resources.GetObject("$this.StartPosition"), System.Windows.Forms.FormStartPosition)
        Me.Text = resources.GetString("$this.Text")
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private ExitwhenCanceled As Boolean

    Private sqlCmd As New SqlClient.SqlCommand
    Private sqlRdr As SqlClient.SqlDataReader
    Private PW As New PWEncryption

    'load exit when canceled value
    Public Overloads Sub Show(ByVal ExitonCancel As Boolean)
        ExitwhenCanceled = ExitonCancel

        Me.ShowDialog()
    End Sub

    'change the password
    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        Dim Pswd As String

        'warn the user if the fields are blank
        If txtPassword.Text = "" Or txtConfirmPassword.Text = "" Then
            MsgBox("You must enter and confirm the new password.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            Exit Sub
        End If

        'warn the user if the new and confirmed passwords don't match
        If txtPassword.Text <> txtConfirmPassword.Text Then
            MsgBox("The password and confirmed password do not match.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            txtPassword.Text = ""
            txtConfirmPassword.Text = ""
            Exit Sub
        End If

        'warn the user if the new or confirmed password has an apostrophe 
        If InStr(txtPassword.Text, "'") Or InStr(txtConfirmPassword.Text, "'") Then
            MsgBox("The password may not contain apostrophes.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            txtPassword.Text = ""
            txtConfirmPassword.Text = ""
            Exit Sub
        End If

        'warn the user if the new password has been used within the past six months
        sqlCmd.Connection = dbConnection
        dbConnection.Open()

        'encrypt password
        Pswd = PW.EncryptString(txtPassword.Text)

        sqlCmd.CommandText = "SELECT * FROM PswdHist WHERE UserID = '" & UserID & "' AND Pswd = '" & Pswd & "' AND DATEDIFF(day,PswdCreateDt,GETDATE()) < 183"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()

        If sqlRdr.HasRows Then
            MsgBox("The new password has been used within the past six months.  Please select a different password.", MsgBoxStyle.Exclamation, "New Century Scholarship Program")
            sqlRdr.Close()
            dbConnection.Close()

            txtPassword.Text = ""
            txtConfirmPassword.Text = ""
            Exit Sub
        End If
        sqlRdr.Close()

        'add new password to password history
        sqlCmd.CommandText = "INSERT INTO PswdHist (UserID, Pswd, PswdCreateDt, PswdExpDt) Values ('" & UserID & "', '" & Pswd & "', GETDATE(), DATEADD(day,30,GETDATE()))"
        sqlCmd.ExecuteNonQuery()
        sqlRdr.Close()

        'update user information with new password
        sqlCmd.CommandText = "UPDATE UserInfo SET Pswd = '" & Pswd & "', PswdCreateDt = GETDATE() WHERE UserID  = '" & UserID & "'"
        sqlCmd.ExecuteNonQuery()
        sqlRdr.Close()

        dbConnection.Close()

        MsgBox("Your password has been changed.", MsgBoxStyle.Information, "New Century Scholarship Program")

        Me.Hide()
    End Sub

    'close form
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If ExitwhenCanceled Then End
        Me.Hide()
    End Sub

    'close application if needed
    Private Sub frmPassword_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        If ExitwhenCanceled Then End
    End Sub

End Class
