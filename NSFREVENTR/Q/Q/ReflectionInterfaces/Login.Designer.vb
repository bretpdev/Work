<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Login
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
        Me.components = New System.ComponentModel.Container
        Me.lblPasword = New System.Windows.Forms.Label
        Me.lblUsername = New System.Windows.Forms.Label
        Me.lblHeader = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.CredentialsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.txtUsername = New System.Windows.Forms.TextBox
        CType(Me.CredentialsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblPasword
        '
        Me.lblPasword.AutoSize = True
        Me.lblPasword.Location = New System.Drawing.Point(13, 67)
        Me.lblPasword.Name = "lblPasword"
        Me.lblPasword.Size = New System.Drawing.Size(56, 13)
        Me.lblPasword.TabIndex = 4
        Me.lblPasword.Text = "Password:"
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(13, 41)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(58, 13)
        Me.lblUsername.TabIndex = 5
        Me.lblUsername.Text = "Username:"
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Location = New System.Drawing.Point(12, 9)
        Me.lblHeader.MaximumSize = New System.Drawing.Size(200, 26)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(180, 26)
        Me.lblHeader.TabIndex = 6
        Me.lblHeader.Text = "Please enter your login credentials to leave ARC's after printing."
        Me.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPassword
        '
        Me.txtPassword.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.CredentialsBindingSource, "Password", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtPassword.Location = New System.Drawing.Point(75, 64)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(90, 20)
        Me.txtPassword.TabIndex = 1
        '
        'CredentialsBindingSource
        '
        Me.CredentialsBindingSource.DataSource = GetType(Q.Credentials)
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(113, 90)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(76, 23)
        Me.btnOk.TabIndex = 2
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(15, 90)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(76, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtUsername
        '
        Me.txtUsername.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.CredentialsBindingSource, "UserName", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtUsername.Location = New System.Drawing.Point(75, 38)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(90, 20)
        Me.txtUsername.TabIndex = 0
        '
        'Login
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(200, 123)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.lblHeader)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.lblPasword)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Login"
        Me.Text = "Login"
        CType(Me.CredentialsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblPasword As System.Windows.Forms.Label
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents lblHeader As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents CredentialsBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
End Class
