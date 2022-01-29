Partial Public Class frmBorrowerBenefits
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents c1 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents dgBB As System.Windows.Forms.DataGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBorrowerBenefits))
        Me.btnClose = New System.Windows.Forms.Button
        Me.dgBB = New System.Windows.Forms.DataGrid
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle
        Me.c1 = New System.Windows.Forms.DataGridTextBoxColumn
        CType(Me.dgBB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(432, 256)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        '
        'dgBB
        '
        Me.dgBB.CaptionVisible = False
        Me.dgBB.DataMember = ""
        Me.dgBB.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgBB.Location = New System.Drawing.Point(16, 8)
        Me.dgBB.Name = "dgBB"
        Me.dgBB.ReadOnly = True
        Me.dgBB.Size = New System.Drawing.Size(912, 240)
        Me.dgBB.TabIndex = 2
        Me.dgBB.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.DataGrid = Me.dgBB
        Me.DataGridTableStyle1.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.c1})
        Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        '
        'c1
        '
        Me.c1.Format = ""
        Me.c1.FormatInfo = Nothing
        Me.c1.HeaderText = "Ln Seq"
        Me.c1.Width = 75
        '
        'frmBorrowerBenefits
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(942, 299)
        Me.Controls.Add(Me.dgBB)
        Me.Controls.Add(Me.btnClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(952, 328)
        Me.MinimumSize = New System.Drawing.Size(952, 328)
        Me.Name = "frmBorrowerBenefits"
        Me.ShowInTaskbar = False
        Me.Text = "Maui DUDE Borrower Benefits"
        CType(Me.dgBB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub
End Class
