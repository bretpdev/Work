<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogin))
        Me.btnLogin = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.tbxUserID = New System.Windows.Forms.TextBox
        Me.tbxPassword = New System.Windows.Forms.TextBox
        Me.lblUserName = New System.Windows.Forms.Label
        Me.lblPassword = New System.Windows.Forms.Label
        Me.btnUserAccess = New System.Windows.Forms.Button
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.lblFormName = New System.Windows.Forms.Label
        Me.lblTestMode = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(154, 214)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(94, 23)
        Me.btnLogin.TabIndex = 2
        Me.btnLogin.Text = "Login"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(302, 214)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'tbxUserID
        '
        Me.tbxUserID.Location = New System.Drawing.Point(140, 117)
        Me.tbxUserID.Name = "tbxUserID"
        Me.tbxUserID.Size = New System.Drawing.Size(120, 20)
        Me.tbxUserID.TabIndex = 0
        '
        'tbxPassword
        '
        Me.tbxPassword.Location = New System.Drawing.Point(140, 172)
        Me.tbxPassword.MaxLength = 15
        Me.tbxPassword.Name = "tbxPassword"
        Me.tbxPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.tbxPassword.Size = New System.Drawing.Size(120, 20)
        Me.tbxPassword.TabIndex = 1
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.BackColor = System.Drawing.Color.Transparent
        Me.lblUserName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserName.Location = New System.Drawing.Point(139, 97)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(90, 16)
        Me.lblUserName.TabIndex = 4
        Me.lblUserName.Text = "User Name:"
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.BackColor = System.Drawing.Color.Transparent
        Me.lblPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPassword.Location = New System.Drawing.Point(139, 152)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(80, 16)
        Me.lblPassword.TabIndex = 5
        Me.lblPassword.Text = "Password:"
        '
        'btnUserAccess
        '
        Me.btnUserAccess.Enabled = False
        Me.btnUserAccess.Location = New System.Drawing.Point(266, 116)
        Me.btnUserAccess.Name = "btnUserAccess"
        Me.btnUserAccess.Size = New System.Drawing.Size(111, 23)
        Me.btnUserAccess.TabIndex = 4
        Me.btnUserAccess.Text = "User Access"
        Me.btnUserAccess.UseVisualStyleBackColor = True
        Me.btnUserAccess.Visible = False
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(266, 170)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(111, 23)
        Me.btnUpdate.TabIndex = 6
        Me.btnUpdate.Text = "Update Password"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'lblFormName
        '
        Me.lblFormName.AutoSize = True
        Me.lblFormName.BackColor = System.Drawing.Color.Transparent
        Me.lblFormName.Font = New System.Drawing.Font("Lucida Sans", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFormName.ForeColor = System.Drawing.Color.Ivory
        Me.lblFormName.Location = New System.Drawing.Point(168, 49)
        Me.lblFormName.Name = "lblFormName"
        Me.lblFormName.Size = New System.Drawing.Size(227, 17)
        Me.lblFormName.TabIndex = 7
        Me.lblFormName.Text = "Regents' Scholarship System"
        '
        'lblTestMode
        '
        Me.lblTestMode.AutoSize = True
        Me.lblTestMode.BackColor = System.Drawing.Color.Transparent
        Me.lblTestMode.Font = New System.Drawing.Font("Lucida Sans", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTestMode.ForeColor = System.Drawing.Color.Ivory
        Me.lblTestMode.Location = New System.Drawing.Point(218, 71)
        Me.lblTestMode.Name = "lblTestMode"
        Me.lblTestMode.Size = New System.Drawing.Size(97, 17)
        Me.lblTestMode.TabIndex = 8
        Me.lblTestMode.Text = "TEST MODE"
        '
        'frmLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.ushe
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(399, 249)
        Me.Controls.Add(Me.lblTestMode)
        Me.Controls.Add(Me.lblFormName)
        Me.Controls.Add(Me.btnUserAccess)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.lblUserName)
        Me.Controls.Add(Me.tbxPassword)
        Me.Controls.Add(Me.tbxUserID)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnLogin)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(399, 249)
        Me.MinimumSize = New System.Drawing.Size(399, 249)
        Me.Name = "frmLogin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Regents' Scholarship Login Screen"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tbxUserID As System.Windows.Forms.TextBox
    Friend WithEvents tbxPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblUserName As System.Windows.Forms.Label
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents btnUserAccess As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents lblFormName As System.Windows.Forms.Label
    Friend WithEvents lblTestMode As System.Windows.Forms.Label
End Class
