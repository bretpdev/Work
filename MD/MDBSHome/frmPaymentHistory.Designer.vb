Partial Public Class frmPaymentHistory
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.Hide()
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents LVHist As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lbl48Pay As System.Windows.Forms.Label
    Friend WithEvents IL As System.Windows.Forms.ImageList
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmPaymentHistory))
        Me.LVHist = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.IL = New System.Windows.Forms.ImageList(Me.components)
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lbl48Pay = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'LVHist
        '
        Me.LVHist.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader8, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader9})
        Me.LVHist.FullRowSelect = True
        Me.LVHist.Location = New System.Drawing.Point(8, 32)
        Me.LVHist.Name = "LVHist"
        Me.LVHist.Size = New System.Drawing.Size(704, 136)
        Me.LVHist.SmallImageList = Me.IL
        Me.LVHist.TabIndex = 0
        Me.LVHist.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Reason Reversal"
        Me.ColumnHeader1.Width = 102
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Effective Date"
        Me.ColumnHeader2.Width = 84
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Date Posted"
        Me.ColumnHeader3.Width = 76
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Trans Type"
        Me.ColumnHeader4.Width = 71
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Trans Amt"
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Principal"
        Me.ColumnHeader5.Width = 73
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Interest"
        Me.ColumnHeader6.Width = 51
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Late Fee"
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Principal Balance"
        Me.ColumnHeader9.Width = 106
        '
        'IL
        '
        Me.IL.ImageSize = New System.Drawing.Size(16, 16)
        Me.IL.ImageStream = CType(resources.GetObject("IL.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.IL.TransparentColor = System.Drawing.Color.Transparent
        '
        'btnClose
        '
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(400, 240)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Close"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("PosterBodoni BT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(848, 23)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Borrower Payment History"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(720, 24)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(152, 136)
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'lbl48Pay
        '
        Me.lbl48Pay.Font = New System.Drawing.Font("PosterBodoni BT", 12.0!)
        Me.lbl48Pay.ForeColor = System.Drawing.Color.Red
        Me.lbl48Pay.Location = New System.Drawing.Point(8, 200)
        Me.lbl48Pay.Name = "lbl48Pay"
        Me.lbl48Pay.Size = New System.Drawing.Size(864, 32)
        Me.lbl48Pay.TabIndex = 6
        Me.lbl48Pay.Text = "Label2"
        Me.lbl48Pay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.Label2.ImageIndex = 0
        Me.Label2.ImageList = Me.IL
        Me.Label2.Location = New System.Drawing.Point(8, 168)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(704, 24)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "       = Bounced check or some other type of payment that wasn't collectible"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmPaymentHistory
        '
        Me.AcceptButton = Me.btnClose
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(880, 269)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lbl48Pay)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.LVHist)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(888, 296)
        Me.MinimumSize = New System.Drawing.Size(888, 296)
        Me.Name = "frmPaymentHistory"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Borrower Payment History"
        Me.ResumeLayout(False)
    End Sub
End Class
