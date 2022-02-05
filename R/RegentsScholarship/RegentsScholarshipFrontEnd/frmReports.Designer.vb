<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReports
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReports))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.cmbSelectReport = New System.Windows.Forms.ToolStripComboBox
        Me.CRptViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.BindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ToolStrip1.SuspendLayout()
        CType(Me.BindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmbSelectReport})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(779, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'cmbSelectReport
        '
        Me.cmbSelectReport.Name = "cmbSelectReport"
        Me.cmbSelectReport.Size = New System.Drawing.Size(320, 25)
        '
        'CRptViewer
        '
        Me.CRptViewer.ActiveViewIndex = -1
        Me.CRptViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CRptViewer.DisplayBackgroundEdge = False
        Me.CRptViewer.DisplayGroupTree = False
        Me.CRptViewer.DisplayStatusBar = False
        Me.CRptViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CRptViewer.Location = New System.Drawing.Point(0, 25)
        Me.CRptViewer.Name = "CRptViewer"
        Me.CRptViewer.SelectionFormula = ""
        Me.CRptViewer.ShowGroupTreeButton = False
        Me.CRptViewer.ShowRefreshButton = False
        Me.CRptViewer.Size = New System.Drawing.Size(779, 636)
        Me.CRptViewer.TabIndex = 2
        Me.CRptViewer.TabStop = False
        Me.CRptViewer.ViewTimeSelectionFormula = ""
        '
        'BindingSource
        '
        Me.BindingSource.DataSource = GetType(RegentsScholarshipBackEnd.HighSchool)
        '
        'frmReports
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(779, 661)
        Me.Controls.Add(Me.CRptViewer)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmReports"
        Me.Text = "Reports"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.BindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents BindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CRptViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents cmbSelectReport As System.Windows.Forms.ToolStripComboBox
End Class
