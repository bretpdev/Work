<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTransactionAuditHistory
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTransactionAuditHistory))
        Me.pnlTransactionSearch = New System.Windows.Forms.Panel
        Me.radSortByUserId = New System.Windows.Forms.RadioButton
        Me.lblSortBy = New System.Windows.Forms.Label
        Me.radSortByDate = New System.Windows.Forms.RadioButton
        Me.lblSelectStudentId = New System.Windows.Forms.Label
        Me.btnViewTransactionHistory = New System.Windows.Forms.Button
        Me.lblSelectUserId = New System.Windows.Forms.Label
        Me.cmbUserId = New System.Windows.Forms.ComboBox
        Me.cmbStudentId = New System.Windows.Forms.ComboBox
        Me.CrystalReportViewer1 = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.pnlTransactionSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTransactionSearch
        '
        Me.pnlTransactionSearch.BackColor = System.Drawing.Color.Gainsboro
        Me.pnlTransactionSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTransactionSearch.Controls.Add(Me.radSortByUserId)
        Me.pnlTransactionSearch.Controls.Add(Me.lblSortBy)
        Me.pnlTransactionSearch.Controls.Add(Me.radSortByDate)
        Me.pnlTransactionSearch.Controls.Add(Me.lblSelectStudentId)
        Me.pnlTransactionSearch.Controls.Add(Me.btnViewTransactionHistory)
        Me.pnlTransactionSearch.Controls.Add(Me.lblSelectUserId)
        Me.pnlTransactionSearch.Controls.Add(Me.cmbUserId)
        Me.pnlTransactionSearch.Controls.Add(Me.cmbStudentId)
        Me.pnlTransactionSearch.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTransactionSearch.Location = New System.Drawing.Point(0, 0)
        Me.pnlTransactionSearch.Name = "pnlTransactionSearch"
        Me.pnlTransactionSearch.Size = New System.Drawing.Size(835, 34)
        Me.pnlTransactionSearch.TabIndex = 74
        '
        'radSortByUserId
        '
        Me.radSortByUserId.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.radSortByUserId.AutoSize = True
        Me.radSortByUserId.Location = New System.Drawing.Point(639, 7)
        Me.radSortByUserId.Name = "radSortByUserId"
        Me.radSortByUserId.Size = New System.Drawing.Size(61, 17)
        Me.radSortByUserId.TabIndex = 34
        Me.radSortByUserId.Text = "User ID"
        Me.radSortByUserId.UseVisualStyleBackColor = True
        '
        'lblSortBy
        '
        Me.lblSortBy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSortBy.AutoSize = True
        Me.lblSortBy.Location = New System.Drawing.Point(536, 9)
        Me.lblSortBy.Name = "lblSortBy"
        Me.lblSortBy.Size = New System.Drawing.Size(43, 13)
        Me.lblSortBy.TabIndex = 33
        Me.lblSortBy.Text = "Sort by:"
        '
        'radSortByDate
        '
        Me.radSortByDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.radSortByDate.AutoSize = True
        Me.radSortByDate.Checked = True
        Me.radSortByDate.Location = New System.Drawing.Point(585, 7)
        Me.radSortByDate.Name = "radSortByDate"
        Me.radSortByDate.Size = New System.Drawing.Size(48, 17)
        Me.radSortByDate.TabIndex = 32
        Me.radSortByDate.TabStop = True
        Me.radSortByDate.Text = "Date"
        Me.radSortByDate.UseVisualStyleBackColor = True
        '
        'lblSelectStudentId
        '
        Me.lblSelectStudentId.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSelectStudentId.AutoSize = True
        Me.lblSelectStudentId.Location = New System.Drawing.Point(298, 9)
        Me.lblSelectStudentId.Name = "lblSelectStudentId"
        Me.lblSelectStudentId.Size = New System.Drawing.Size(109, 13)
        Me.lblSelectStudentId.TabIndex = 31
        Me.lblSelectStudentId.Text = "...and/or a student ID"
        '
        'btnViewTransactionHistory
        '
        Me.btnViewTransactionHistory.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnViewTransactionHistory.AutoSize = True
        Me.btnViewTransactionHistory.BackColor = System.Drawing.SystemColors.Control
        Me.btnViewTransactionHistory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViewTransactionHistory.Location = New System.Drawing.Point(709, 4)
        Me.btnViewTransactionHistory.Name = "btnViewTransactionHistory"
        Me.btnViewTransactionHistory.Size = New System.Drawing.Size(121, 23)
        Me.btnViewTransactionHistory.TabIndex = 3
        Me.btnViewTransactionHistory.Text = "View Transactions"
        Me.btnViewTransactionHistory.UseVisualStyleBackColor = False
        '
        'lblSelectUserId
        '
        Me.lblSelectUserId.AutoSize = True
        Me.lblSelectUserId.Location = New System.Drawing.Point(3, 9)
        Me.lblSelectUserId.Name = "lblSelectUserId"
        Me.lblSelectUserId.Size = New System.Drawing.Size(92, 13)
        Me.lblSelectUserId.TabIndex = 27
        Me.lblSelectUserId.Text = "Select a user ID..."
        '
        'cmbUserId
        '
        Me.cmbUserId.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbUserId.FormattingEnabled = True
        Me.cmbUserId.Location = New System.Drawing.Point(101, 6)
        Me.cmbUserId.Name = "cmbUserId"
        Me.cmbUserId.Size = New System.Drawing.Size(191, 21)
        Me.cmbUserId.TabIndex = 1
        '
        'cmbStudentId
        '
        Me.cmbStudentId.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbStudentId.FormattingEnabled = True
        Me.cmbStudentId.Location = New System.Drawing.Point(413, 6)
        Me.cmbStudentId.Name = "cmbStudentId"
        Me.cmbStudentId.Size = New System.Drawing.Size(117, 21)
        Me.cmbStudentId.TabIndex = 2
        '
        'CrystalReportViewer1
        '
        Me.CrystalReportViewer1.ActiveViewIndex = -1
        Me.CrystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CrystalReportViewer1.DisplayGroupTree = False
        Me.CrystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CrystalReportViewer1.Location = New System.Drawing.Point(0, 34)
        Me.CrystalReportViewer1.Name = "CrystalReportViewer1"
        Me.CrystalReportViewer1.SelectionFormula = ""
        Me.CrystalReportViewer1.Size = New System.Drawing.Size(835, 570)
        Me.CrystalReportViewer1.TabIndex = 4
        Me.CrystalReportViewer1.ViewTimeSelectionFormula = ""
        '
        'frmTransactionAuditHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(835, 604)
        Me.Controls.Add(Me.CrystalReportViewer1)
        Me.Controls.Add(Me.pnlTransactionSearch)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmTransactionAuditHistory"
        Me.Text = "Transaction Audit History"
        Me.pnlTransactionSearch.ResumeLayout(False)
        Me.pnlTransactionSearch.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlTransactionSearch As System.Windows.Forms.Panel
    Friend WithEvents btnViewTransactionHistory As System.Windows.Forms.Button
    Friend WithEvents lblSelectUserId As System.Windows.Forms.Label
    Friend WithEvents cmbUserId As System.Windows.Forms.ComboBox
    Friend WithEvents cmbStudentId As System.Windows.Forms.ComboBox
    Friend WithEvents lblSelectStudentId As System.Windows.Forms.Label
    Friend WithEvents CrystalReportViewer1 As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents radSortByUserId As System.Windows.Forms.RadioButton
    Friend WithEvents lblSortBy As System.Windows.Forms.Label
    Friend WithEvents radSortByDate As System.Windows.Forms.RadioButton
End Class
