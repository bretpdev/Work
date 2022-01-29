Public Class frmUnderConstruct
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
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblComment As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmUnderConstruct))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lblComment = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(8, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(144, 184)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'lblComment
        '
        Me.lblComment.Font = New System.Drawing.Font("Bodoni MT Black", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblComment.Location = New System.Drawing.Point(160, 16)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.Size = New System.Drawing.Size(304, 144)
        Me.lblComment.TabIndex = 1
        Me.lblComment.Text = "Sorry Dude, but that wave hasn't arrived yet. "
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(200, 168)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        '
        'frmUnderConstruction
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(472, 205)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblComment)
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(480, 232)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(480, 232)
        Me.Name = "frmUnderConstruction"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maui DUDE Under Construction"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub

    Public Shared Sub ShowUnderConstruct()
        Dim UC As New frmUnderConstruct
        UC.Showdialog()
    End Sub

End Class
