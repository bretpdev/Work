Public Class frmPasswordExpired
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbPW As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbPWC As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmPasswordExpired))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbPW = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbPWC = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 100)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "New Password:"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 124)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 32)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "New Password (Confirmation):"
        '
        'tbPW
        '
        Me.tbPW.Location = New System.Drawing.Point(96, 96)
        Me.tbPW.MaxLength = 8
        Me.tbPW.Name = "tbPW"
        Me.tbPW.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPW.Size = New System.Drawing.Size(144, 20)
        Me.tbPW.TabIndex = 2
        Me.tbPW.Text = ""
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(0, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(248, 76)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Your OneLINK/COMPASS password appears to have expired.  Please provide a new pass" & _
        "word and then enter it again to confirm it.  System passwords must be 8 characte" & _
        "rs long and must have a number as the second character."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbPWC
        '
        Me.tbPWC.Location = New System.Drawing.Point(96, 132)
        Me.tbPWC.MaxLength = 8
        Me.tbPWC.Name = "tbPWC"
        Me.tbPWC.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPWC.Size = New System.Drawing.Size(144, 20)
        Me.tbPWC.TabIndex = 4
        Me.tbPWC.Text = ""
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(88, 164)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "OK"
        '
        'frmPasswordExpired
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(248, 193)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tbPWC)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbPW)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPasswordExpired"
        Me.Text = "System Password Expired"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'confirm that the two passwords match
        If tbPW.Text <> tbPWC.Text Then
            SP.frmWhoaDUDE.WhoaDUDE("The new passwords do not match, please enter them again.  The format must be A1AAAAAA.", "Passwords Must Match")
            Exit Sub
        End If
        If IsNumeric(tbPW.Text.ToCharArray()(1)) = False Then
            SP.frmWhoaDUDE.WhoaDUDE("The second character of the password must be a number. (Example: A1AAAAAA)", "Passwords Follow Format")
            Exit Sub
        End If
        Me.Hide()
    End Sub

    Public Function GetPW() As String
        Return tbPW.Text
    End Function
End Class
