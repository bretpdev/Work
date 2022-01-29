<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrivateLoans
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.dgPvLnHist = New System.Windows.Forms.DataGrid
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        CType(Me.dgPvLnHist, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgPvLnHist
        '
        Me.dgPvLnHist.CaptionVisible = False
        Me.dgPvLnHist.DataMember = ""
        Me.dgPvLnHist.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgPvLnHist.Location = New System.Drawing.Point(12, 45)
        Me.dgPvLnHist.Name = "dgPvLnHist"
        Me.dgPvLnHist.ReadOnly = True
        Me.dgPvLnHist.Size = New System.Drawing.Size(858, 160)
        Me.dgPvLnHist.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(134, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(633, 23)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Private Loans"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnClose
        '
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(418, 211)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 23)
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "Close"
        '
        'frmPrivateLoans
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(882, 248)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgPvLnHist)
        Me.MaximumSize = New System.Drawing.Size(890, 282)
        Me.MinimumSize = New System.Drawing.Size(890, 282)
        Me.Name = "frmPrivateLoans"
        Me.Text = "frmPrivateLoans"
        CType(Me.dgPvLnHist, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgPvLnHist As System.Windows.Forms.DataGrid
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
