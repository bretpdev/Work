<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUserAccess
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
        Me.btnCancel = New System.Windows.Forms.Button
        Me.cmbAccessLevel = New System.Windows.Forms.ComboBox
        Me.lblAccessLevel = New System.Windows.Forms.Label
        Me.lblPassword = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.lblUserName = New System.Windows.Forms.Label
        Me.btnAddUser = New System.Windows.Forms.Button
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.cbxDefault = New System.Windows.Forms.CheckBox
        Me.cmbUserName = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(432, 111)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(97, 23)
        Me.btnCancel.TabIndex = 16
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cmbAccessLevel
        '
        Me.cmbAccessLevel.FormattingEnabled = True
        Me.cmbAccessLevel.Location = New System.Drawing.Point(234, 66)
        Me.cmbAccessLevel.Name = "cmbAccessLevel"
        Me.cmbAccessLevel.Size = New System.Drawing.Size(121, 21)
        Me.cmbAccessLevel.TabIndex = 10
        '
        'lblAccessLevel
        '
        Me.lblAccessLevel.AutoSize = True
        Me.lblAccessLevel.Location = New System.Drawing.Point(153, 68)
        Me.lblAccessLevel.Name = "lblAccessLevel"
        Me.lblAccessLevel.Size = New System.Drawing.Size(70, 13)
        Me.lblAccessLevel.TabIndex = 14
        Me.lblAccessLevel.Text = "Access level:"
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(167, 42)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblPassword.TabIndex = 13
        Me.lblPassword.Text = "Password:"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(234, 39)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(171, 20)
        Me.txtPassword.TabIndex = 9
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Location = New System.Drawing.Point(162, 17)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(61, 13)
        Me.lblUserName.TabIndex = 11
        Me.lblUserName.Text = "User name:"
        '
        'btnAddUser
        '
        Me.btnAddUser.AutoSize = True
        Me.btnAddUser.Location = New System.Drawing.Point(45, 111)
        Me.btnAddUser.Name = "btnAddUser"
        Me.btnAddUser.Size = New System.Drawing.Size(97, 23)
        Me.btnAddUser.TabIndex = 12
        Me.btnAddUser.Text = "Add New User"
        Me.btnAddUser.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(174, 111)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(97, 23)
        Me.btnUpdate.TabIndex = 17
        Me.btnUpdate.Text = "Update User"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(303, 111)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(97, 23)
        Me.btnDelete.TabIndex = 18
        Me.btnDelete.Text = "Delete User"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'cbxDefault
        '
        Me.cbxDefault.AutoSize = True
        Me.cbxDefault.Location = New System.Drawing.Point(411, 42)
        Me.cbxDefault.Name = "cbxDefault"
        Me.cbxDefault.Size = New System.Drawing.Size(91, 17)
        Me.cbxDefault.TabIndex = 20
        Me.cbxDefault.Text = "Reset Default"
        Me.cbxDefault.UseVisualStyleBackColor = True
        '
        'cmbUserName
        '
        Me.cmbUserName.FormattingEnabled = True
        Me.cmbUserName.Location = New System.Drawing.Point(234, 12)
        Me.cmbUserName.Name = "cmbUserName"
        Me.cmbUserName.Size = New System.Drawing.Size(191, 21)
        Me.cmbUserName.TabIndex = 21
        '
        'frmUserAccess
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(571, 151)
        Me.Controls.Add(Me.cmbUserName)
        Me.Controls.Add(Me.cbxDefault)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.cmbAccessLevel)
        Me.Controls.Add(Me.lblAccessLevel)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.lblUserName)
        Me.Controls.Add(Me.btnAddUser)
        Me.Name = "frmUserAccess"
        Me.Text = "User Access"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cmbAccessLevel As System.Windows.Forms.ComboBox
    Friend WithEvents lblAccessLevel As System.Windows.Forms.Label
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblUserName As System.Windows.Forms.Label
    Friend WithEvents btnAddUser As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents cbxDefault As System.Windows.Forms.CheckBox
    Friend WithEvents cmbUserName As System.Windows.Forms.ComboBox
End Class
