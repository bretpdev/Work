Partial Public Class frmCompassLoans
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
    Friend WithEvents IL As System.Windows.Forms.ImageList
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgLnHist As System.Windows.Forms.DataGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCompassLoans))
        Me.IL = New System.Windows.Forms.ImageList(Me.components)
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.dgLnHist = New System.Windows.Forms.DataGrid
        CType(Me.dgLnHist, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'IL
        '
        Me.IL.ImageStream = CType(resources.GetObject("IL.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.IL.TransparentColor = System.Drawing.Color.Transparent
        Me.IL.Images.SetKeyName(0, "")
        Me.IL.Images.SetKeyName(1, "")
        Me.IL.Images.SetKeyName(2, "")
        Me.IL.Images.SetKeyName(3, "")
        Me.IL.Images.SetKeyName(4, "")
        Me.IL.Images.SetKeyName(5, "")
        '
        'btnClose
        '
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(472, 208)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(1000, 23)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "COMPASS Loans"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgLnHist
        '
        Me.dgLnHist.CaptionVisible = False
        Me.dgLnHist.DataMember = ""
        Me.dgLnHist.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgLnHist.Location = New System.Drawing.Point(8, 40)
        Me.dgLnHist.Name = "dgLnHist"
        Me.dgLnHist.ReadOnly = True
        Me.dgLnHist.Size = New System.Drawing.Size(1000, 160)
        Me.dgLnHist.TabIndex = 6
        '
        'frmCompassLoans
        '
        Me.AcceptButton = Me.btnClose
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1020, 238)
        Me.Controls.Add(Me.dgLnHist)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1028, 272)
        Me.MinimumSize = New System.Drawing.Size(1028, 272)
        Me.Name = "frmCompassLoans"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "COMPASS Loan History"
        CType(Me.dgLnHist, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub
End Class
