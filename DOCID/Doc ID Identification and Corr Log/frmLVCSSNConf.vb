Public Class frmLVCSSNConf
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
    Friend WithEvents tbSSN As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmLVCSSNConf))
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbSSN = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(224, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Please re-enter the SSN for LVC log confirmation."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbSSN
        '
        Me.tbSSN.Location = New System.Drawing.Point(8, 40)
        Me.tbSSN.Name = "tbSSN"
        Me.tbSSN.Size = New System.Drawing.Size(224, 20)
        Me.tbSSN.TabIndex = 1
        Me.tbSSN.Text = ""
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(80, 72)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 24)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        '
        'frmLVCSSNConf
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(240, 100)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tbSSN)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLVCSSNConf"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Confirm SSN For LVC Tracking"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Shadows Function Show() As String
        Me.ShowDialog()
        Return tbSSN.Text
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Hide()
    End Sub
End Class
