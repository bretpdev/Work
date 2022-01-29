Public Class frm411
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
        ProtectInfo()
        Me.Hide()
        'If disposing Then
        '    If Not (components Is Nothing) Then
        '        components.Dispose()
        '    End If
        'End If
        'MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblSSN As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtInfo1 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtInfo2 As System.Windows.Forms.TextBox
    Friend WithEvents txtInfo3 As System.Windows.Forms.TextBox
    Friend WithEvents txtInfo4 As System.Windows.Forms.TextBox
    Friend WithEvents txtInfo5 As System.Windows.Forms.TextBox
    Friend WithEvents txtInfo6 As System.Windows.Forms.TextBox
    Friend WithEvents txtInfo8 As System.Windows.Forms.TextBox
    Friend WithEvents txtInfo7 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frm411))
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtInfo1 = New System.Windows.Forms.TextBox
        Me.btnChange = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.lblName = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblSSN = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtInfo8 = New System.Windows.Forms.TextBox
        Me.txtInfo7 = New System.Windows.Forms.TextBox
        Me.txtInfo6 = New System.Windows.Forms.TextBox
        Me.txtInfo5 = New System.Windows.Forms.TextBox
        Me.txtInfo4 = New System.Windows.Forms.TextBox
        Me.txtInfo3 = New System.Windows.Forms.TextBox
        Me.txtInfo2 = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("PosterBodoni BT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(632, 24)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Maui DUDE Borrower Information"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtInfo1
        '
        Me.txtInfo1.AutoSize = False
        Me.txtInfo1.BackColor = System.Drawing.SystemColors.Control
        Me.txtInfo1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtInfo1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInfo1.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo1.Location = New System.Drawing.Point(12, 76)
        Me.txtInfo1.MaxLength = 64
        Me.txtInfo1.Name = "txtInfo1"
        Me.txtInfo1.ReadOnly = True
        Me.txtInfo1.Size = New System.Drawing.Size(612, 18)
        Me.txtInfo1.TabIndex = 3
        Me.txtInfo1.Text = ""
        Me.txtInfo1.WordWrap = False
        '
        'btnChange
        '
        Me.btnChange.Location = New System.Drawing.Point(376, 240)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.TabIndex = 2
        Me.btnChange.Text = "C&hange"
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(464, 240)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.TabIndex = 11
        Me.btnSave.Text = "&Save"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(552, 240)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "C&ancel"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(16, 240)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "&Close"
        '
        'lblName
        '
        Me.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblName.Location = New System.Drawing.Point(172, 40)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(460, 23)
        Me.lblName.TabIndex = 8
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(132, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 23)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Name:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSSN
        '
        Me.lblSSN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSSN.Location = New System.Drawing.Point(44, 40)
        Me.lblSSN.Name = "lblSSN"
        Me.lblSSN.Size = New System.Drawing.Size(72, 23)
        Me.lblSSN.TabIndex = 9
        Me.lblSSN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(12, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 24)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "SSN:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtInfo8)
        Me.GroupBox1.Controls.Add(Me.txtInfo7)
        Me.GroupBox1.Controls.Add(Me.txtInfo6)
        Me.GroupBox1.Controls.Add(Me.txtInfo5)
        Me.GroupBox1.Controls.Add(Me.txtInfo4)
        Me.GroupBox1.Controls.Add(Me.txtInfo3)
        Me.GroupBox1.Controls.Add(Me.txtInfo2)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 69)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(622, 160)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        '
        'txtInfo8
        '
        Me.txtInfo8.AutoSize = False
        Me.txtInfo8.BackColor = System.Drawing.SystemColors.Control
        Me.txtInfo8.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtInfo8.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInfo8.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo8.Location = New System.Drawing.Point(2, 140)
        Me.txtInfo8.MaxLength = 64
        Me.txtInfo8.Name = "txtInfo8"
        Me.txtInfo8.ReadOnly = True
        Me.txtInfo8.Size = New System.Drawing.Size(614, 18)
        Me.txtInfo8.TabIndex = 10
        Me.txtInfo8.Text = ""
        Me.txtInfo8.WordWrap = False
        '
        'txtInfo7
        '
        Me.txtInfo7.AutoSize = False
        Me.txtInfo7.BackColor = System.Drawing.SystemColors.Control
        Me.txtInfo7.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtInfo7.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInfo7.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo7.Location = New System.Drawing.Point(2, 121)
        Me.txtInfo7.MaxLength = 64
        Me.txtInfo7.Name = "txtInfo7"
        Me.txtInfo7.ReadOnly = True
        Me.txtInfo7.Size = New System.Drawing.Size(614, 18)
        Me.txtInfo7.TabIndex = 9
        Me.txtInfo7.Text = ""
        Me.txtInfo7.WordWrap = False
        '
        'txtInfo6
        '
        Me.txtInfo6.AutoSize = False
        Me.txtInfo6.BackColor = System.Drawing.SystemColors.Control
        Me.txtInfo6.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtInfo6.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInfo6.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo6.Location = New System.Drawing.Point(2, 102)
        Me.txtInfo6.MaxLength = 64
        Me.txtInfo6.Name = "txtInfo6"
        Me.txtInfo6.ReadOnly = True
        Me.txtInfo6.Size = New System.Drawing.Size(614, 18)
        Me.txtInfo6.TabIndex = 8
        Me.txtInfo6.Text = ""
        Me.txtInfo6.WordWrap = False
        '
        'txtInfo5
        '
        Me.txtInfo5.AutoSize = False
        Me.txtInfo5.BackColor = System.Drawing.SystemColors.Control
        Me.txtInfo5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtInfo5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInfo5.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo5.Location = New System.Drawing.Point(2, 83)
        Me.txtInfo5.MaxLength = 64
        Me.txtInfo5.Name = "txtInfo5"
        Me.txtInfo5.ReadOnly = True
        Me.txtInfo5.Size = New System.Drawing.Size(614, 18)
        Me.txtInfo5.TabIndex = 7
        Me.txtInfo5.Text = ""
        Me.txtInfo5.WordWrap = False
        '
        'txtInfo4
        '
        Me.txtInfo4.AutoSize = False
        Me.txtInfo4.BackColor = System.Drawing.SystemColors.Control
        Me.txtInfo4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtInfo4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInfo4.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo4.Location = New System.Drawing.Point(2, 64)
        Me.txtInfo4.MaxLength = 64
        Me.txtInfo4.Name = "txtInfo4"
        Me.txtInfo4.ReadOnly = True
        Me.txtInfo4.Size = New System.Drawing.Size(614, 18)
        Me.txtInfo4.TabIndex = 6
        Me.txtInfo4.Text = ""
        Me.txtInfo4.WordWrap = False
        '
        'txtInfo3
        '
        Me.txtInfo3.AutoSize = False
        Me.txtInfo3.BackColor = System.Drawing.SystemColors.Control
        Me.txtInfo3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtInfo3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInfo3.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo3.Location = New System.Drawing.Point(2, 45)
        Me.txtInfo3.MaxLength = 64
        Me.txtInfo3.Name = "txtInfo3"
        Me.txtInfo3.ReadOnly = True
        Me.txtInfo3.Size = New System.Drawing.Size(614, 18)
        Me.txtInfo3.TabIndex = 5
        Me.txtInfo3.Text = ""
        Me.txtInfo3.WordWrap = False
        '
        'txtInfo2
        '
        Me.txtInfo2.AutoSize = False
        Me.txtInfo2.BackColor = System.Drawing.SystemColors.Control
        Me.txtInfo2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtInfo2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInfo2.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo2.Location = New System.Drawing.Point(2, 26)
        Me.txtInfo2.MaxLength = 64
        Me.txtInfo2.Name = "txtInfo2"
        Me.txtInfo2.ReadOnly = True
        Me.txtInfo2.Size = New System.Drawing.Size(614, 18)
        Me.txtInfo2.TabIndex = 4
        Me.txtInfo2.Text = ""
        Me.txtInfo2.WordWrap = False
        '
        'frm411
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(638, 268)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblSSN)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnChange)
        Me.Controls.Add(Me.txtInfo1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximumSize = New System.Drawing.Size(648, 298)
        Me.Name = "frm411"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maui DUDE Borrower Information"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    'display user information
    Public Overloads Sub Show(ByVal SSN As String, ByVal Name As String)
        GetBorrInfo(SSN)
        'change colors
        txtInfo1.ForeColor = Me.ForeColor
        txtInfo2.ForeColor = Me.ForeColor
        txtInfo3.ForeColor = Me.ForeColor
        txtInfo4.ForeColor = Me.ForeColor
        txtInfo5.ForeColor = Me.ForeColor
        txtInfo6.ForeColor = Me.ForeColor
        txtInfo7.ForeColor = Me.ForeColor
        txtInfo8.ForeColor = Me.ForeColor
        txtInfo1.BackColor = Me.BackColor
        txtInfo2.BackColor = Me.BackColor
        txtInfo3.BackColor = Me.BackColor
        txtInfo4.BackColor = Me.BackColor
        txtInfo5.BackColor = Me.BackColor
        txtInfo6.BackColor = Me.BackColor
        txtInfo7.BackColor = Me.BackColor
        txtInfo8.BackColor = Me.BackColor
        'populate(SSN And Name)
        Me.lblSSN.Text = SSN
        Me.lblName.Text = Name
        Me.Show()
    End Sub

    'display user information
    Public Overloads Sub ShowDialog(ByVal SSN As String, ByVal Name As String)
        'change colors
        txtInfo1.ForeColor = Me.ForeColor
        txtInfo2.ForeColor = Me.ForeColor
        txtInfo3.ForeColor = Me.ForeColor
        txtInfo4.ForeColor = Me.ForeColor
        txtInfo5.ForeColor = Me.ForeColor
        txtInfo6.ForeColor = Me.ForeColor
        txtInfo7.ForeColor = Me.ForeColor
        txtInfo8.ForeColor = Me.ForeColor
        txtInfo1.BackColor = Me.BackColor
        txtInfo2.BackColor = Me.BackColor
        txtInfo3.BackColor = Me.BackColor
        txtInfo4.BackColor = Me.BackColor
        txtInfo5.BackColor = Me.BackColor
        txtInfo6.BackColor = Me.BackColor
        txtInfo7.BackColor = Me.BackColor
        txtInfo8.BackColor = Me.BackColor
        If GetBorrInfo(SSN) Then
            'populate ssn and name
            Me.lblSSN.Text = SSN
            Me.lblName.Text = Name
            Me.ShowDialog()
        End If
    End Sub

    Sub MakeVisible()
        txtInfo1.ForeColor = Me.ForeColor
        txtInfo2.ForeColor = Me.ForeColor
        txtInfo3.ForeColor = Me.ForeColor
        txtInfo4.ForeColor = Me.ForeColor
        txtInfo5.ForeColor = Me.ForeColor
        txtInfo6.ForeColor = Me.ForeColor
        txtInfo7.ForeColor = Me.ForeColor
        txtInfo8.ForeColor = Me.ForeColor
        txtInfo1.BackColor = Me.BackColor
        txtInfo2.BackColor = Me.BackColor
        txtInfo3.BackColor = Me.BackColor
        txtInfo4.BackColor = Me.BackColor
        txtInfo5.BackColor = Me.BackColor
        txtInfo6.BackColor = Me.BackColor
        txtInfo7.BackColor = Me.BackColor
        txtInfo8.BackColor = Me.BackColor
        Me.Visible = True
    End Sub
    'get info from LP50
    Function GetBorrInfo(ByVal SSN As String) As Boolean
        Dim HasComment As Boolean
        GetBorrInfo = True
        HasComment = False
        'access LP50
        FastPath("LP50I" & SSN & ";;;;;M1411")
        'select most recent record if multiple records are displayed
        If Check4Text(1, 58, "ACTIVITY SUMMARY SELECT") Then PutText(6, 2, "X", True)
        'get info from each line if detail screen is displayed
        If Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY") Then
            Me.txtInfo1.Text = GetText(13, 2, 75)
            Me.txtInfo2.Text = GetText(14, 2, 75)
            Me.txtInfo3.Text = GetText(15, 2, 75)
            Me.txtInfo4.Text = GetText(16, 2, 75)
            Me.txtInfo5.Text = GetText(17, 2, 75)
            Me.txtInfo6.Text = GetText(18, 2, 75)
            Me.txtInfo7.Text = GetText(19, 2, 75)
            Me.txtInfo8.Text = GetText(20, 2, 75)

            If InStr(1, Me.txtInfo1.Text, "(UT") - 2 > 0 Then
                HasComment = True
                Me.txtInfo1.Text = LTrim(Mid(Me.txtInfo1.Text, 1, InStr(1, Me.txtInfo1.Text, "(UT") - 1))
            ElseIf InStr(1, Me.txtInfo2.Text, "(UT") > 0 Then
                HasComment = True
                Me.txtInfo2.Text = LTrim(Mid(Me.txtInfo2.Text, 1, InStr(1, Me.txtInfo2.Text, "(UT") - 1))
            ElseIf InStr(1, Me.txtInfo3.Text, "(UT") > 0 Then
                HasComment = True
                Me.txtInfo3.Text = LTrim(Mid(Me.txtInfo3.Text, 1, InStr(1, Me.txtInfo3.Text, "(UT") - 1))
            ElseIf InStr(1, Me.txtInfo4.Text, "(UT") > 0 Then
                HasComment = True
                Me.txtInfo4.Text = LTrim(Mid(Me.txtInfo4.Text, 1, InStr(1, Me.txtInfo4.Text, "(UT") - 1))
            ElseIf InStr(1, Me.txtInfo5.Text, "(UT") > 0 Then
                HasComment = True
                Me.txtInfo5.Text = LTrim(Mid(Me.txtInfo5.Text, 1, InStr(1, Me.txtInfo5.Text, "(UT") - 1))
            ElseIf InStr(1, Me.txtInfo6.Text, "(UT") > 0 Then
                HasComment = True
                Me.txtInfo6.Text = LTrim(Mid(Me.txtInfo6.Text, 1, InStr(1, Me.txtInfo6.Text, "(UT") - 1))
            ElseIf InStr(1, Me.txtInfo7.Text, "(UT") > 0 Then
                HasComment = True
                Me.txtInfo7.Text = LTrim(Mid(Me.txtInfo7.Text, 1, InStr(1, Me.txtInfo7.Text, "(UT") - 1))
            ElseIf InStr(1, Me.txtInfo8.Text, "(UT") > 0 Then
                HasComment = True
                Me.txtInfo8.Text = LTrim(Mid(Me.txtInfo8.Text, 1, InStr(1, Me.txtInfo8.Text, "(UT") - 1))
            End If
            'populate values if no info is found
        Else
            Me.txtInfo1.Text = "No Borrower Information Found"
            Me.txtInfo2.Text = ""
            Me.txtInfo3.Text = ""
            Me.txtInfo4.Text = ""
            Me.txtInfo5.Text = ""
            Me.txtInfo6.Text = ""
            Me.txtInfo7.Text = ""
            Me.txtInfo8.Text = ""
            GetBorrInfo = False
        End If
        If HasComment = False Then
            Me.txtInfo1.Text = ""
            Me.txtInfo2.Text = ""
            Me.txtInfo3.Text = ""
            Me.txtInfo4.Text = ""
            Me.txtInfo5.Text = ""
            Me.txtInfo6.Text = ""
            Me.txtInfo7.Text = ""
            Me.txtInfo8.Text = ""
            GetBorrInfo = False
        End If


    End Function
    'close the form without saving any changes to the borrower information
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ProtectInfo()
        Me.Hide()
    End Sub

    'close the form without saving any changes to the borrower information
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ProtectInfo()
        Me.Hide()
    End Sub

    'unprotect info fields so they can be edited
    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        Me.txtInfo1.BackColor = System.Drawing.SystemColors.Window
        Me.txtInfo2.BackColor = System.Drawing.SystemColors.Window
        Me.txtInfo3.BackColor = System.Drawing.SystemColors.Window
        Me.txtInfo4.BackColor = System.Drawing.SystemColors.Window
        Me.txtInfo5.BackColor = System.Drawing.SystemColors.Window
        Me.txtInfo6.BackColor = System.Drawing.SystemColors.Window
        Me.txtInfo7.BackColor = System.Drawing.SystemColors.Window
        Me.txtInfo8.BackColor = System.Drawing.SystemColors.Window
        Me.txtInfo1.ReadOnly = False
        Me.txtInfo2.ReadOnly = False
        Me.txtInfo3.ReadOnly = False
        Me.txtInfo4.ReadOnly = False
        Me.txtInfo5.ReadOnly = False
        Me.txtInfo6.ReadOnly = False
        Me.txtInfo7.ReadOnly = False
        Me.txtInfo8.ReadOnly = False
        Me.btnSave.Enabled = True
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'protect text box
        ProtectInfo()
        'hide form
        Me.Hide()
        'access LP50
        FastPath("LP50A" & Me.lblSSN.Text & ";;;AM;10;M1411")
        'write info from each field to corresponding line on LP50
        PutText(13, 2, Me.txtInfo1.Text)
        PutText(14, 2, Me.txtInfo2.Text)
        PutText(15, 2, Me.txtInfo3.Text)
        PutText(16, 2, Me.txtInfo4.Text)
        PutText(17, 2, Me.txtInfo5.Text)
        PutText(18, 2, Me.txtInfo6.Text)
        PutText(19, 2, Me.txtInfo7.Text)
        PutText(20, 2, Me.txtInfo8.Text)
        'save changes
        Hit("F6")
    End Sub

    'protect info fields and save button
    Sub ProtectInfo()
        Me.txtInfo1.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtInfo2.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtInfo3.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtInfo4.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtInfo5.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtInfo6.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtInfo7.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtInfo8.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtInfo1.ReadOnly = True
        Me.txtInfo2.ReadOnly = True
        Me.txtInfo3.ReadOnly = True
        Me.txtInfo4.ReadOnly = True
        Me.txtInfo5.ReadOnly = True
        Me.txtInfo6.ReadOnly = True
        Me.txtInfo7.ReadOnly = True
        Me.txtInfo8.ReadOnly = True
        Me.btnSave.Enabled = False
        btnClose.Focus()
    End Sub

    Private Sub frm411_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
