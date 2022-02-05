Imports System.Drawing
Public Class frmMoreStatus
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
    Friend WithEvents lstStatus As System.Windows.Forms.ListBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMoreStatus))
        Me.Label1 = New System.Windows.Forms.Label
        Me.lstStatus = New System.Windows.Forms.ListBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Bodoni MT Black", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(144, 40)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Mo' Status"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lstStatus
        '
        Me.lstStatus.Location = New System.Drawing.Point(16, 64)
        Me.lstStatus.Name = "lstStatus"
        Me.lstStatus.Size = New System.Drawing.Size(144, 160)
        Me.lstStatus.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(48, 232)
        Me.Button1.Name = "Button1"
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Close"
        '
        'frmMoreStatus
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(176, 266)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lstStatus)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMoreStatus"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Mo Status"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Overloads Function Show(ByRef list As ArrayList, ByVal TransPar As Double, ByVal BackColor As Color, ByVal ForeColor As Color) As Boolean
        'ByVal list() As String,
        lstStatus.BackColor = BackColor
        lstStatus.ForeColor = ForeColor

        lstStatus.DataSource = list.ToArray
        Me.Show()


    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class
