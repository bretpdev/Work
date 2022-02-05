Public Class FrmProcessing
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
    Public WithEvents PB As System.Windows.Forms.ProgressBar
    Public WithEvents FileProcessing As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmProcessing))
        Me.PB = New System.Windows.Forms.ProgressBar
        Me.FileProcessing = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lblVersion = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'PB
        '
        Me.PB.Location = New System.Drawing.Point(152, 40)
        Me.PB.Name = "PB"
        Me.PB.Size = New System.Drawing.Size(688, 23)
        Me.PB.TabIndex = 0
        '
        'FileProcessing
        '
        Me.FileProcessing.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FileProcessing.Location = New System.Drawing.Point(152, 16)
        Me.FileProcessing.Name = "FileProcessing"
        Me.FileProcessing.Size = New System.Drawing.Size(688, 24)
        Me.FileProcessing.TabIndex = 1
        Me.FileProcessing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(8, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(136, 80)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'lblVersion
        '
        Me.lblVersion.Font = New System.Drawing.Font("Century Gothic", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.Location = New System.Drawing.Point(152, 8)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(72, 23)
        Me.lblVersion.TabIndex = 13
        Me.lblVersion.Text = "VERSION 5.0"
        Me.lblVersion.Visible = False
        '
        'FrmProcessing
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(848, 98)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.FileProcessing)
        Me.Controls.Add(Me.PB)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(616, 104)
        Me.Name = "FrmProcessing"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub FrmProcessing_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Draw version
        Dim I As Image
        Dim Gr As Graphics
        Dim F As New Font(lblVersion.Font.FontFamily, 7, FontStyle.Bold, GraphicsUnit.Pixel)
        I = PictureBox1.Image
        Gr = Graphics.FromImage(I)
        Gr.DrawString(lblVersion.Text, F, Brushes.Black, 85, 5)
        PictureBox1.Image = I
    End Sub
End Class
