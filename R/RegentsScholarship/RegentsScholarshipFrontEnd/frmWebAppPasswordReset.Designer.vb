<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWebAppPasswordReset
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWebAppPasswordReset))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.grpUserIDSearch = New System.Windows.Forms.GroupBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtUserIDSearch = New System.Windows.Forms.TextBox
        Me.radUserID = New System.Windows.Forms.RadioButton
        Me.grpEmailSearch = New System.Windows.Forms.GroupBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtEmailSearch = New System.Windows.Forms.TextBox
        Me.radEmail = New System.Windows.Forms.RadioButton
        Me.grpNameSearch = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtLastNameSearch = New System.Windows.Forms.TextBox
        Me.txtFirstNameSearch = New System.Windows.Forms.TextBox
        Me.radNames = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.TextBox9 = New System.Windows.Forms.TextBox
        Me.TextBox8 = New System.Windows.Forms.TextBox
        Me.TextBox7 = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.TextBox6 = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.txt2 = New System.Windows.Forms.TextBox
        Me.txt1 = New System.Windows.Forms.TextBox
        Me.btnResetPassword = New System.Windows.Forms.Button
        Me.WebAppUserBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.grpUserIDSearch.SuspendLayout()
        Me.grpEmailSearch.SuspendLayout()
        Me.grpNameSearch.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.WebAppUserBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.grpUserIDSearch)
        Me.GroupBox1.Controls.Add(Me.radUserID)
        Me.GroupBox1.Controls.Add(Me.grpEmailSearch)
        Me.GroupBox1.Controls.Add(Me.radEmail)
        Me.GroupBox1.Controls.Add(Me.grpNameSearch)
        Me.GroupBox1.Controls.Add(Me.radNames)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(333, 444)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'grpUserIDSearch
        '
        Me.grpUserIDSearch.Controls.Add(Me.Label4)
        Me.grpUserIDSearch.Controls.Add(Me.txtUserIDSearch)
        Me.grpUserIDSearch.Enabled = False
        Me.grpUserIDSearch.Location = New System.Drawing.Point(47, 390)
        Me.grpUserIDSearch.Name = "grpUserIDSearch"
        Me.grpUserIDSearch.Size = New System.Drawing.Size(277, 44)
        Me.grpUserIDSearch.TabIndex = 6
        Me.grpUserIDSearch.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "User ID"
        '
        'txtUserIDSearch
        '
        Me.txtUserIDSearch.Location = New System.Drawing.Point(84, 13)
        Me.txtUserIDSearch.Name = "txtUserIDSearch"
        Me.txtUserIDSearch.Size = New System.Drawing.Size(187, 20)
        Me.txtUserIDSearch.TabIndex = 0
        '
        'radUserID
        '
        Me.radUserID.AutoSize = True
        Me.radUserID.Location = New System.Drawing.Point(6, 367)
        Me.radUserID.Name = "radUserID"
        Me.radUserID.Size = New System.Drawing.Size(148, 17)
        Me.radUserID.TabIndex = 5
        Me.radUserID.TabStop = True
        Me.radUserID.Text = "User provided their user id"
        Me.radUserID.UseVisualStyleBackColor = True
        '
        'grpEmailSearch
        '
        Me.grpEmailSearch.Controls.Add(Me.Label5)
        Me.grpEmailSearch.Controls.Add(Me.txtEmailSearch)
        Me.grpEmailSearch.Enabled = False
        Me.grpEmailSearch.Location = New System.Drawing.Point(47, 253)
        Me.grpEmailSearch.Name = "grpEmailSearch"
        Me.grpEmailSearch.Size = New System.Drawing.Size(277, 44)
        Me.grpEmailSearch.TabIndex = 4
        Me.grpEmailSearch.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(32, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Email"
        '
        'txtEmailSearch
        '
        Me.txtEmailSearch.Location = New System.Drawing.Point(84, 13)
        Me.txtEmailSearch.Name = "txtEmailSearch"
        Me.txtEmailSearch.Size = New System.Drawing.Size(187, 20)
        Me.txtEmailSearch.TabIndex = 0
        '
        'radEmail
        '
        Me.radEmail.AutoSize = True
        Me.radEmail.Location = New System.Drawing.Point(6, 230)
        Me.radEmail.Name = "radEmail"
        Me.radEmail.Size = New System.Drawing.Size(181, 17)
        Me.radEmail.TabIndex = 3
        Me.radEmail.TabStop = True
        Me.radEmail.Text = "User provided their email address"
        Me.radEmail.UseVisualStyleBackColor = True
        '
        'grpNameSearch
        '
        Me.grpNameSearch.Controls.Add(Me.Label3)
        Me.grpNameSearch.Controls.Add(Me.Label2)
        Me.grpNameSearch.Controls.Add(Me.txtLastNameSearch)
        Me.grpNameSearch.Controls.Add(Me.txtFirstNameSearch)
        Me.grpNameSearch.Location = New System.Drawing.Point(50, 98)
        Me.grpNameSearch.Name = "grpNameSearch"
        Me.grpNameSearch.Size = New System.Drawing.Size(277, 70)
        Me.grpNameSearch.TabIndex = 2
        Me.grpNameSearch.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 42)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Last Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "First Name"
        '
        'txtLastNameSearch
        '
        Me.txtLastNameSearch.Location = New System.Drawing.Point(84, 39)
        Me.txtLastNameSearch.Name = "txtLastNameSearch"
        Me.txtLastNameSearch.Size = New System.Drawing.Size(187, 20)
        Me.txtLastNameSearch.TabIndex = 1
        '
        'txtFirstNameSearch
        '
        Me.txtFirstNameSearch.Location = New System.Drawing.Point(84, 13)
        Me.txtFirstNameSearch.Name = "txtFirstNameSearch"
        Me.txtFirstNameSearch.Size = New System.Drawing.Size(187, 20)
        Me.txtFirstNameSearch.TabIndex = 0
        '
        'radNames
        '
        Me.radNames.AutoSize = True
        Me.radNames.Location = New System.Drawing.Point(9, 75)
        Me.radNames.Name = "radNames"
        Me.radNames.Size = New System.Drawing.Size(202, 17)
        Me.radNames.TabIndex = 1
        Me.radNames.TabStop = True
        Me.radNames.Text = "User provided their first and last name"
        Me.radNames.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(298, 34)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Please select one of the options below and provide the needed information."
        '
        'btnSearch
        '
        Me.btnSearch.Image = Global.RegentsScholarshipFrontEnd.My.Resources.Resources.ZoomHS
        Me.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearch.Location = New System.Drawing.Point(357, 213)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(81, 23)
        Me.btnSearch.TabIndex = 1
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.Label14)
        Me.GroupBox5.Controls.Add(Me.TextBox9)
        Me.GroupBox5.Controls.Add(Me.TextBox8)
        Me.GroupBox5.Controls.Add(Me.TextBox7)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.TextBox6)
        Me.GroupBox5.Controls.Add(Me.Label12)
        Me.GroupBox5.Controls.Add(Me.TextBox5)
        Me.GroupBox5.Controls.Add(Me.Label11)
        Me.GroupBox5.Controls.Add(Me.TextBox4)
        Me.GroupBox5.Controls.Add(Me.Label10)
        Me.GroupBox5.Controls.Add(Me.TextBox3)
        Me.GroupBox5.Controls.Add(Me.Label9)
        Me.GroupBox5.Controls.Add(Me.Label8)
        Me.GroupBox5.Controls.Add(Me.Label7)
        Me.GroupBox5.Controls.Add(Me.Label6)
        Me.GroupBox5.Controls.Add(Me.TextBox2)
        Me.GroupBox5.Controls.Add(Me.TextBox1)
        Me.GroupBox5.Controls.Add(Me.txt2)
        Me.GroupBox5.Controls.Add(Me.txt1)
        Me.GroupBox5.Location = New System.Drawing.Point(460, 91)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(419, 266)
        Me.GroupBox5.TabIndex = 2
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Search Results"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(14, 232)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(43, 13)
        Me.Label14.TabIndex = 19
        Me.Label14.Text = "Country"
        '
        'TextBox9
        '
        Me.TextBox9.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "Country", True))
        Me.TextBox9.Location = New System.Drawing.Point(119, 229)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.ReadOnly = True
        Me.TextBox9.Size = New System.Drawing.Size(216, 20)
        Me.TextBox9.TabIndex = 18
        '
        'TextBox8
        '
        Me.TextBox8.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "Zip", True))
        Me.TextBox8.Location = New System.Drawing.Point(301, 203)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.ReadOnly = True
        Me.TextBox8.Size = New System.Drawing.Size(105, 20)
        Me.TextBox8.TabIndex = 17
        '
        'TextBox7
        '
        Me.TextBox7.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "State", True))
        Me.TextBox7.Location = New System.Drawing.Point(252, 203)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ReadOnly = True
        Me.TextBox7.Size = New System.Drawing.Size(43, 20)
        Me.TextBox7.TabIndex = 16
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(14, 206)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(74, 13)
        Me.Label13.TabIndex = 15
        Me.Label13.Text = "City/State/Zip"
        '
        'TextBox6
        '
        Me.TextBox6.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "City", True))
        Me.TextBox6.Location = New System.Drawing.Point(119, 203)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ReadOnly = True
        Me.TextBox6.Size = New System.Drawing.Size(127, 20)
        Me.TextBox6.TabIndex = 14
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(14, 180)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(61, 13)
        Me.Label12.TabIndex = 13
        Me.Label12.Text = "Address #2"
        '
        'TextBox5
        '
        Me.TextBox5.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "Address2", True))
        Me.TextBox5.Location = New System.Drawing.Point(119, 177)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(287, 20)
        Me.TextBox5.TabIndex = 12
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(14, 154)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(61, 13)
        Me.Label11.TabIndex = 11
        Me.Label11.Text = "Address #1"
        '
        'TextBox4
        '
        Me.TextBox4.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "Address1", True))
        Me.TextBox4.Location = New System.Drawing.Point(119, 151)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(287, 20)
        Me.TextBox4.TabIndex = 10
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(14, 128)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(32, 13)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Email"
        '
        'TextBox3
        '
        Me.TextBox3.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "EmailAddress", True))
        Me.TextBox3.Location = New System.Drawing.Point(119, 125)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(287, 20)
        Me.TextBox3.TabIndex = 8
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(14, 102)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(38, 13)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Phone"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(14, 76)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(58, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Last Name"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(14, 50)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "First Name"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(14, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "User ID"
        '
        'TextBox2
        '
        Me.TextBox2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "LastName", True))
        Me.TextBox2.Location = New System.Drawing.Point(119, 73)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(216, 20)
        Me.TextBox2.TabIndex = 3
        '
        'TextBox1
        '
        Me.TextBox1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "PhoneNumber", True))
        Me.TextBox1.Location = New System.Drawing.Point(119, 99)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(216, 20)
        Me.TextBox1.TabIndex = 2
        '
        'txt2
        '
        Me.txt2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "FirstName", True))
        Me.txt2.Location = New System.Drawing.Point(119, 47)
        Me.txt2.Name = "txt2"
        Me.txt2.ReadOnly = True
        Me.txt2.Size = New System.Drawing.Size(216, 20)
        Me.txt2.TabIndex = 1
        '
        'txt1
        '
        Me.txt1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.WebAppUserBindingSource, "Username", True))
        Me.txt1.Location = New System.Drawing.Point(119, 21)
        Me.txt1.Name = "txt1"
        Me.txt1.ReadOnly = True
        Me.txt1.Size = New System.Drawing.Size(216, 20)
        Me.txt1.TabIndex = 0
        '
        'btnResetPassword
        '
        Me.btnResetPassword.Enabled = False
        Me.btnResetPassword.Location = New System.Drawing.Point(608, 372)
        Me.btnResetPassword.Name = "btnResetPassword"
        Me.btnResetPassword.Size = New System.Drawing.Size(123, 23)
        Me.btnResetPassword.TabIndex = 3
        Me.btnResetPassword.Text = "Reset Password"
        Me.btnResetPassword.UseVisualStyleBackColor = True
        '
        'WebAppUserBindingSource
        '
        Me.WebAppUserBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.WebAppUser)
        '
        'frmWebAppPasswordReset
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(888, 461)
        Me.Controls.Add(Me.btnResetPassword)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWebAppPasswordReset"
        Me.Text = "Web Password Reset"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpUserIDSearch.ResumeLayout(False)
        Me.grpUserIDSearch.PerformLayout()
        Me.grpEmailSearch.ResumeLayout(False)
        Me.grpEmailSearch.PerformLayout()
        Me.grpNameSearch.ResumeLayout(False)
        Me.grpNameSearch.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        CType(Me.WebAppUserBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents radNames As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpNameSearch As System.Windows.Forms.GroupBox
    Friend WithEvents grpEmailSearch As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtEmailSearch As System.Windows.Forms.TextBox
    Friend WithEvents radEmail As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtLastNameSearch As System.Windows.Forms.TextBox
    Friend WithEvents txtFirstNameSearch As System.Windows.Forms.TextBox
    Friend WithEvents grpUserIDSearch As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtUserIDSearch As System.Windows.Forms.TextBox
    Friend WithEvents radUserID As System.Windows.Forms.RadioButton
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents txt1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents txt2 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents btnResetPassword As System.Windows.Forms.Button
    Friend WithEvents WebAppUserBindingSource As System.Windows.Forms.BindingSource
End Class
