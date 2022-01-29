Public Class FrmNoFiles
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
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FrmNoFiles))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.label1 = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.lblVersion = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(8, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(136, 80)
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(152, 32)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(192, 23)
        Me.label1.TabIndex = 10
        Me.label1.Text = "There were no files to process."
        Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(208, 64)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 11
        Me.btnOK.Text = "OK"
        '
        'lblVersion
        '
        Me.lblVersion.Font = New System.Drawing.Font("Century Gothic", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.Location = New System.Drawing.Point(144, 8)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(72, 23)
        Me.lblVersion.TabIndex = 12
        Me.lblVersion.Text = "VERSION 5.0"
        Me.lblVersion.Visible = False
        '
        'FrmNoFiles
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(346, 95)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(356, 124)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(356, 124)
        Me.Name = "FrmNoFiles"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "UHEAA FIXIT"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub

    Private Sub FrmNoFiles_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
