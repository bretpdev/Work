Public Class Login
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
    Friend WithEvents tbAccID As System.Windows.Forms.TextBox
    Friend WithEvents tbPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnProcess As System.Windows.Forms.Button
    Friend WithEvents btnreports As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Login))
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnProcess = New System.Windows.Forms.Button
        Me.btnreports = New System.Windows.Forms.Button
        Me.tbAccID = New System.Windows.Forms.TextBox
        Me.tbPassword = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(8, 128)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(208, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        '
        'btnProcess
        '
        Me.btnProcess.Location = New System.Drawing.Point(8, 64)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(208, 23)
        Me.btnProcess.TabIndex = 2
        Me.btnProcess.Text = "&Process E-mail"
        '
        'btnreports
        '
        Me.btnreports.Location = New System.Drawing.Point(8, 96)
        Me.btnreports.Name = "btnreports"
        Me.btnreports.Size = New System.Drawing.Size(208, 23)
        Me.btnreports.TabIndex = 3
        Me.btnreports.Text = "&Reports (Login Not Required)"
        '
        'tbAccID
        '
        Me.tbAccID.Location = New System.Drawing.Point(80, 8)
        Me.tbAccID.Name = "tbAccID"
        Me.tbAccID.Size = New System.Drawing.Size(136, 20)
        Me.tbAccID.TabIndex = 0
        Me.tbAccID.Text = ""
        '
        'tbPassword
        '
        Me.tbPassword.Location = New System.Drawing.Point(80, 32)
        Me.tbPassword.Name = "tbPassword"
        Me.tbPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPassword.Size = New System.Drawing.Size(136, 20)
        Me.tbPassword.TabIndex = 1
        Me.tbPassword.Text = ""
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 23)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "  Password:"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(16, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 23)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Account ID:"
        '
        'Login
        '
        Me.AcceptButton = Me.btnProcess
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(224, 157)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbPassword)
        Me.Controls.Add(Me.tbAccID)
        Me.Controls.Add(Me.btnreports)
        Me.Controls.Add(Me.btnProcess)
        Me.Controls.Add(Me.btnCancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(232, 184)
        Me.Name = "Login"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "E-Mail Tracking Login"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        If tbAccID.TextLength = 0 Or tbPassword.TextLength = 0 Then
            MsgBox("You must provide a Account ID and Password.")
        Else
            Me.Hide()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        End
    End Sub

    Private Sub btnreports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnreports.Click
        Me.Hide()
    End Sub

    Private Sub Login_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        End
    End Sub
End Class
