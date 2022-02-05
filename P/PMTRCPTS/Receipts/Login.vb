Imports System.Data.SqlClient

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents tbUN As System.Windows.Forms.TextBox
    Friend WithEvents tbPW As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Login))
        Me.tbUN = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbPW = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'tbUN
        '
        Me.tbUN.Location = New System.Drawing.Point(80, 4)
        Me.tbUN.Name = "tbUN"
        Me.tbUN.TabIndex = 0
        Me.tbUN.Text = ""
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(4, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "User Name:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(4, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Password:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbPW
        '
        Me.tbPW.Location = New System.Drawing.Point(80, 24)
        Me.tbPW.Name = "tbPW"
        Me.tbPW.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPW.TabIndex = 2
        Me.tbPW.Text = ""
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(20, 52)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(104, 52)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        '
        'Login
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(196, 77)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbPW)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbUN)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(204, 104)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(204, 104)
        Me.Name = "Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private AccessTo As String
    Private DBConn As SqlConnection

    Public Overloads Function Show(ByVal TempDBConn As SqlConnection) As String
        DBConn = TempDBConn
        Me.ShowDialog()
        Return AccessTo
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim Comm As New SqlCommand
        Dim Reader As SqlDataReader
        If tbUN.TextLength = 0 Or tbPW.TextLength = 0 Then
            MsgBox("You must provide a user name and password.", MsgBoxStyle.Critical)
            Exit Sub
        Else
            'check what kind access do they have if any
            DBConn.Open()
            Comm.Connection = DBConn
            Comm.CommandText = "SELECT AccessTo FROM Access WHERE UserName = '" & tbUN.Text & "' AND Password = '" & tbPW.Text & "'"
            Reader = Comm.ExecuteReader
            If Reader.Read Then
                AccessTo = Reader.Item("AccessTo")
            Else
                'Results weren't found so the user has no access
                AccessTo = "NONE"
            End If
            DBConn.Close()
            Me.Hide()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        AccessTo = "Cancelled"
        Me.Hide()
    End Sub

End Class
