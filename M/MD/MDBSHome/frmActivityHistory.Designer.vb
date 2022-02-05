Partial Public Class frmActivityHistory
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
    Friend WithEvents lbltittle As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents dgAH As System.Windows.Forms.DataGrid
    Friend WithEvents DV As System.Data.DataView
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmActivityHistory))
        Me.lbltittle = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        Me.dgAH = New System.Windows.Forms.DataGrid
        Me.DV = New System.Data.DataView
        CType(Me.dgAH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DV, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbltittle
        '
        Me.lbltittle.Font = New System.Drawing.Font("Bodoni MT Black", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbltittle.Location = New System.Drawing.Point(48, 8)
        Me.lbltittle.Name = "lbltittle"
        Me.lbltittle.Size = New System.Drawing.Size(752, 23)
        Me.lbltittle.TabIndex = 0
        Me.lbltittle.Text = "Borrower Activity History"
        Me.lbltittle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnClose.Location = New System.Drawing.Point(384, 360)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        '
        'dgAH
        '
        Me.dgAH.CaptionVisible = False
        Me.dgAH.DataMember = ""
        Me.dgAH.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.dgAH.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgAH.Location = New System.Drawing.Point(16, 40)
        Me.dgAH.Name = "dgAH"
        Me.dgAH.ReadOnly = True
        Me.dgAH.RowHeadersVisible = False
        Me.dgAH.Size = New System.Drawing.Size(808, 312)
        Me.dgAH.TabIndex = 3
        '
        'DV
        '
        '
        'frmActivityHistory
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(840, 390)
        Me.Controls.Add(Me.dgAH)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lbltittle)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmActivityHistory"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maui DUDE Borrower 30 Day Activity History"
        CType(Me.dgAH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub
End Class
