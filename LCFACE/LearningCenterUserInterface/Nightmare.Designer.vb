Partial Public Class frmNightmare
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents btnBatch As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnWebView As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmNightmare))
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.btnBatch = New System.Windows.Forms.Button
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.btnWebView = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(80, 240)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(48, 23)
        Me.btnClose.TabIndex = 12
        Me.btnClose.Text = "Close"
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(40, 144)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(120, 23)
        Me.btnBrowse.TabIndex = 13
        Me.btnBrowse.Text = "Convert File"
        '
        'btnBatch
        '
        Me.btnBatch.Location = New System.Drawing.Point(40, 200)
        Me.btnBatch.Name = "btnBatch"
        Me.btnBatch.Size = New System.Drawing.Size(120, 23)
        Me.btnBatch.TabIndex = 14
        Me.btnBatch.Text = "Convert Batch"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(8, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(192, 216)
        Me.PictureBox1.TabIndex = 15
        Me.PictureBox1.TabStop = False
        '
        'btnWebView
        '
        Me.btnWebView.Location = New System.Drawing.Point(40, 112)
        Me.btnWebView.Name = "btnWebView"
        Me.btnWebView.Size = New System.Drawing.Size(120, 23)
        Me.btnWebView.TabIndex = 16
        Me.btnWebView.Text = "Web View"
        '
        'frmNightmare
        '
        Me.AllowDrop = True
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gold
        Me.ClientSize = New System.Drawing.Size(200, 277)
        Me.Controls.Add(Me.btnWebView)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.btnBatch)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnClose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmNightmare"
        Me.Text = "LCUI"
        Me.ResumeLayout(False)

    End Sub
End Class
