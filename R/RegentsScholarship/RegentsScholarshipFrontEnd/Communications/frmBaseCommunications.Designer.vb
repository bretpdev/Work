<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBaseCommunications
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBaseCommunications))
        Me.btnCommunicationClearFields = New System.Windows.Forms.Button
        Me.btnCommunicationSave = New System.Windows.Forms.Button
        Me.grpCommunicationRecord = New System.Windows.Forms.GroupBox
        Me.cmbCommunicationSource = New System.Windows.Forms.ComboBox
        Me.lblCommunicationSource = New System.Windows.Forms.Label
        Me.txtCommunicationComments = New System.Windows.Forms.TextBox
        Me.txtCommunicationSubject = New System.Windows.Forms.TextBox
        Me.cmbCommunicationType = New System.Windows.Forms.ComboBox
        Me.txtCommunicationUserId = New System.Windows.Forms.TextBox
        Me.txtCommunicationDateTime = New System.Windows.Forms.TextBox
        Me.lblCommunicationSubject = New System.Windows.Forms.Label
        Me.lblCommunicationType = New System.Windows.Forms.Label
        Me.lblCommunicationUserId = New System.Windows.Forms.Label
        Me.lblCommunicationDateTime = New System.Windows.Forms.Label
        Me.pnlSchoolCommunicationsSearch = New System.Windows.Forms.Panel
        Me.CommunicationDataGridView = New System.Windows.Forms.DataGridView
        Me.UserId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmTimeStamp = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmType = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmSource = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmSubject = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmText = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CommunicationBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnCommuncationsViewDoc = New System.Windows.Forms.Button
        Me.btnCommuncationsLinkDoc = New System.Windows.Forms.Button
        Me.CommunicationPrinter = New RegentsScholarshipFrontEnd.CommunicationRecordPrintingControl
        Me.grpCommunicationRecord.SuspendLayout()
        CType(Me.CommunicationDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CommunicationBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCommunicationClearFields
        '
        Me.btnCommunicationClearFields.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCommunicationClearFields.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCommunicationClearFields.Location = New System.Drawing.Point(102, 660)
        Me.btnCommunicationClearFields.Name = "btnCommunicationClearFields"
        Me.btnCommunicationClearFields.Size = New System.Drawing.Size(92, 32)
        Me.btnCommunicationClearFields.TabIndex = 18
        Me.btnCommunicationClearFields.Text = "Clear Fields"
        Me.btnCommunicationClearFields.UseVisualStyleBackColor = True
        '
        'btnCommunicationSave
        '
        Me.btnCommunicationSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCommunicationSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCommunicationSave.Location = New System.Drawing.Point(-1, 660)
        Me.btnCommunicationSave.Name = "btnCommunicationSave"
        Me.btnCommunicationSave.Size = New System.Drawing.Size(94, 32)
        Me.btnCommunicationSave.TabIndex = 17
        Me.btnCommunicationSave.Text = "Save"
        Me.btnCommunicationSave.UseVisualStyleBackColor = True
        '
        'grpCommunicationRecord
        '
        Me.grpCommunicationRecord.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.grpCommunicationRecord.Controls.Add(Me.cmbCommunicationSource)
        Me.grpCommunicationRecord.Controls.Add(Me.lblCommunicationSource)
        Me.grpCommunicationRecord.Controls.Add(Me.txtCommunicationComments)
        Me.grpCommunicationRecord.Controls.Add(Me.txtCommunicationSubject)
        Me.grpCommunicationRecord.Controls.Add(Me.cmbCommunicationType)
        Me.grpCommunicationRecord.Controls.Add(Me.txtCommunicationUserId)
        Me.grpCommunicationRecord.Controls.Add(Me.txtCommunicationDateTime)
        Me.grpCommunicationRecord.Controls.Add(Me.lblCommunicationSubject)
        Me.grpCommunicationRecord.Controls.Add(Me.lblCommunicationType)
        Me.grpCommunicationRecord.Controls.Add(Me.lblCommunicationUserId)
        Me.grpCommunicationRecord.Controls.Add(Me.lblCommunicationDateTime)
        Me.grpCommunicationRecord.Location = New System.Drawing.Point(3, 53)
        Me.grpCommunicationRecord.Name = "grpCommunicationRecord"
        Me.grpCommunicationRecord.Size = New System.Drawing.Size(787, 212)
        Me.grpCommunicationRecord.TabIndex = 16
        Me.grpCommunicationRecord.TabStop = False
        Me.grpCommunicationRecord.Text = "Communication Record"
        '
        'cmbCommunicationSource
        '
        Me.cmbCommunicationSource.FormattingEnabled = True
        Me.cmbCommunicationSource.Location = New System.Drawing.Point(486, 39)
        Me.cmbCommunicationSource.Name = "cmbCommunicationSource"
        Me.cmbCommunicationSource.Size = New System.Drawing.Size(173, 21)
        Me.cmbCommunicationSource.TabIndex = 5
        '
        'lblCommunicationSource
        '
        Me.lblCommunicationSource.AutoSize = True
        Me.lblCommunicationSource.Location = New System.Drawing.Point(439, 42)
        Me.lblCommunicationSource.Name = "lblCommunicationSource"
        Me.lblCommunicationSource.Size = New System.Drawing.Size(41, 13)
        Me.lblCommunicationSource.TabIndex = 10
        Me.lblCommunicationSource.Text = "Source"
        '
        'txtCommunicationComments
        '
        Me.txtCommunicationComments.Location = New System.Drawing.Point(111, 107)
        Me.txtCommunicationComments.MaxLength = 500
        Me.txtCommunicationComments.Multiline = True
        Me.txtCommunicationComments.Name = "txtCommunicationComments"
        Me.txtCommunicationComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCommunicationComments.Size = New System.Drawing.Size(548, 99)
        Me.txtCommunicationComments.TabIndex = 7
        '
        'txtCommunicationSubject
        '
        Me.txtCommunicationSubject.Location = New System.Drawing.Point(111, 81)
        Me.txtCommunicationSubject.MaxLength = 50
        Me.txtCommunicationSubject.Name = "txtCommunicationSubject"
        Me.txtCommunicationSubject.Size = New System.Drawing.Size(548, 20)
        Me.txtCommunicationSubject.TabIndex = 6
        '
        'cmbCommunicationType
        '
        Me.cmbCommunicationType.FormattingEnabled = True
        Me.cmbCommunicationType.Location = New System.Drawing.Point(486, 12)
        Me.cmbCommunicationType.Name = "cmbCommunicationType"
        Me.cmbCommunicationType.Size = New System.Drawing.Size(173, 21)
        Me.cmbCommunicationType.TabIndex = 4
        '
        'txtCommunicationUserId
        '
        Me.txtCommunicationUserId.Location = New System.Drawing.Point(111, 43)
        Me.txtCommunicationUserId.Name = "txtCommunicationUserId"
        Me.txtCommunicationUserId.ReadOnly = True
        Me.txtCommunicationUserId.Size = New System.Drawing.Size(173, 20)
        Me.txtCommunicationUserId.TabIndex = 0
        Me.txtCommunicationUserId.TabStop = False
        '
        'txtCommunicationDateTime
        '
        Me.txtCommunicationDateTime.Location = New System.Drawing.Point(111, 17)
        Me.txtCommunicationDateTime.Name = "txtCommunicationDateTime"
        Me.txtCommunicationDateTime.ReadOnly = True
        Me.txtCommunicationDateTime.Size = New System.Drawing.Size(173, 20)
        Me.txtCommunicationDateTime.TabIndex = 0
        Me.txtCommunicationDateTime.TabStop = False
        '
        'lblCommunicationSubject
        '
        Me.lblCommunicationSubject.AutoSize = True
        Me.lblCommunicationSubject.Location = New System.Drawing.Point(9, 84)
        Me.lblCommunicationSubject.Name = "lblCommunicationSubject"
        Me.lblCommunicationSubject.Size = New System.Drawing.Size(43, 13)
        Me.lblCommunicationSubject.TabIndex = 3
        Me.lblCommunicationSubject.Text = "Subject"
        '
        'lblCommunicationType
        '
        Me.lblCommunicationType.AutoSize = True
        Me.lblCommunicationType.Location = New System.Drawing.Point(439, 15)
        Me.lblCommunicationType.Name = "lblCommunicationType"
        Me.lblCommunicationType.Size = New System.Drawing.Size(31, 13)
        Me.lblCommunicationType.TabIndex = 2
        Me.lblCommunicationType.Text = "Type"
        '
        'lblCommunicationUserId
        '
        Me.lblCommunicationUserId.AutoSize = True
        Me.lblCommunicationUserId.Location = New System.Drawing.Point(7, 46)
        Me.lblCommunicationUserId.Name = "lblCommunicationUserId"
        Me.lblCommunicationUserId.Size = New System.Drawing.Size(43, 13)
        Me.lblCommunicationUserId.TabIndex = 1
        Me.lblCommunicationUserId.Text = "User ID"
        '
        'lblCommunicationDateTime
        '
        Me.lblCommunicationDateTime.AutoSize = True
        Me.lblCommunicationDateTime.Location = New System.Drawing.Point(7, 20)
        Me.lblCommunicationDateTime.Name = "lblCommunicationDateTime"
        Me.lblCommunicationDateTime.Size = New System.Drawing.Size(98, 13)
        Me.lblCommunicationDateTime.TabIndex = 0
        Me.lblCommunicationDateTime.Text = "Date/Time Created"
        '
        'pnlSchoolCommunicationsSearch
        '
        Me.pnlSchoolCommunicationsSearch.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.pnlSchoolCommunicationsSearch.BackColor = System.Drawing.Color.Gainsboro
        Me.pnlSchoolCommunicationsSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSchoolCommunicationsSearch.Location = New System.Drawing.Point(0, 0)
        Me.pnlSchoolCommunicationsSearch.Name = "pnlSchoolCommunicationsSearch"
        Me.pnlSchoolCommunicationsSearch.Size = New System.Drawing.Size(794, 32)
        Me.pnlSchoolCommunicationsSearch.TabIndex = 73
        '
        'CommunicationDataGridView
        '
        Me.CommunicationDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CommunicationDataGridView.AutoGenerateColumns = False
        Me.CommunicationDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CommunicationDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.CommunicationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.CommunicationDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.UserId, Me.clmTimeStamp, Me.clmType, Me.clmSource, Me.clmSubject, Me.clmText})
        Me.CommunicationDataGridView.DataSource = Me.CommunicationBindingSource
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.CommunicationDataGridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.CommunicationDataGridView.Location = New System.Drawing.Point(3, 271)
        Me.CommunicationDataGridView.Name = "CommunicationDataGridView"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CommunicationDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.CommunicationDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.CommunicationDataGridView.Size = New System.Drawing.Size(787, 335)
        Me.CommunicationDataGridView.TabIndex = 8
        '
        'UserId
        '
        Me.UserId.DataPropertyName = "UserId"
        Me.UserId.HeaderText = "User ID"
        Me.UserId.Name = "UserId"
        Me.UserId.Width = 68
        '
        'clmTimeStamp
        '
        Me.clmTimeStamp.DataPropertyName = "TimeStamp"
        Me.clmTimeStamp.HeaderText = "Time Stamp"
        Me.clmTimeStamp.Name = "clmTimeStamp"
        Me.clmTimeStamp.Width = 88
        '
        'clmType
        '
        Me.clmType.DataPropertyName = "Type"
        Me.clmType.HeaderText = "Type"
        Me.clmType.Name = "clmType"
        Me.clmType.Width = 56
        '
        'clmSource
        '
        Me.clmSource.DataPropertyName = "Source"
        Me.clmSource.HeaderText = "Source"
        Me.clmSource.Name = "clmSource"
        Me.clmSource.Width = 66
        '
        'clmSubject
        '
        Me.clmSubject.DataPropertyName = "Subject"
        Me.clmSubject.HeaderText = "Subject"
        Me.clmSubject.Name = "clmSubject"
        Me.clmSubject.Width = 68
        '
        'clmText
        '
        Me.clmText.DataPropertyName = "Text"
        Me.clmText.HeaderText = "Comments"
        Me.clmText.Name = "clmText"
        Me.clmText.Width = 81
        '
        'CommunicationBindingSource
        '
        Me.CommunicationBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.Communication)
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(696, 661)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(92, 32)
        Me.btnClose.TabIndex = 75
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnCommuncationsViewDoc
        '
        Me.btnCommuncationsViewDoc.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCommuncationsViewDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCommuncationsViewDoc.Location = New System.Drawing.Point(595, 661)
        Me.btnCommuncationsViewDoc.Name = "btnCommuncationsViewDoc"
        Me.btnCommuncationsViewDoc.Size = New System.Drawing.Size(92, 32)
        Me.btnCommuncationsViewDoc.TabIndex = 77
        Me.btnCommuncationsViewDoc.Text = "View Doc."
        Me.btnCommuncationsViewDoc.UseVisualStyleBackColor = True
        '
        'btnCommuncationsLinkDoc
        '
        Me.btnCommuncationsLinkDoc.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCommuncationsLinkDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCommuncationsLinkDoc.Location = New System.Drawing.Point(495, 661)
        Me.btnCommuncationsLinkDoc.Name = "btnCommuncationsLinkDoc"
        Me.btnCommuncationsLinkDoc.Size = New System.Drawing.Size(92, 32)
        Me.btnCommuncationsLinkDoc.TabIndex = 76
        Me.btnCommuncationsLinkDoc.Text = "Link Doc."
        Me.btnCommuncationsLinkDoc.UseVisualStyleBackColor = True
        '
        'CommunicationPrinter
        '
        Me.CommunicationPrinter.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.CommunicationPrinter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CommunicationPrinter.Entity = Nothing
        Me.CommunicationPrinter.EntityName = Nothing
        Me.CommunicationPrinter.EntityType = Nothing
        Me.CommunicationPrinter.Location = New System.Drawing.Point(200, 609)
        Me.CommunicationPrinter.Name = "CommunicationPrinter"
        Me.CommunicationPrinter.Size = New System.Drawing.Size(288, 87)
        Me.CommunicationPrinter.TabIndex = 78
        '
        'frmBaseCommunications
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 698)
        Me.Controls.Add(Me.CommunicationPrinter)
        Me.Controls.Add(Me.btnCommuncationsViewDoc)
        Me.Controls.Add(Me.btnCommuncationsLinkDoc)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.CommunicationDataGridView)
        Me.Controls.Add(Me.pnlSchoolCommunicationsSearch)
        Me.Controls.Add(Me.btnCommunicationClearFields)
        Me.Controls.Add(Me.btnCommunicationSave)
        Me.Controls.Add(Me.grpCommunicationRecord)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBaseCommunications"
        Me.Text = "BaseCommunications"
        Me.grpCommunicationRecord.ResumeLayout(False)
        Me.grpCommunicationRecord.PerformLayout()
        CType(Me.CommunicationDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CommunicationBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCommunicationClearFields As System.Windows.Forms.Button
    Friend WithEvents btnCommunicationSave As System.Windows.Forms.Button
    Friend WithEvents grpCommunicationRecord As System.Windows.Forms.GroupBox
    Friend WithEvents cmbCommunicationSource As System.Windows.Forms.ComboBox
    Friend WithEvents lblCommunicationSource As System.Windows.Forms.Label
    Friend WithEvents txtCommunicationComments As System.Windows.Forms.TextBox
    Friend WithEvents txtCommunicationSubject As System.Windows.Forms.TextBox
    Friend WithEvents cmbCommunicationType As System.Windows.Forms.ComboBox
    Friend WithEvents txtCommunicationUserId As System.Windows.Forms.TextBox
    Friend WithEvents txtCommunicationDateTime As System.Windows.Forms.TextBox
    Friend WithEvents lblCommunicationSubject As System.Windows.Forms.Label
    Friend WithEvents lblCommunicationType As System.Windows.Forms.Label
    Friend WithEvents lblCommunicationUserId As System.Windows.Forms.Label
    Friend WithEvents lblCommunicationDateTime As System.Windows.Forms.Label
    Friend WithEvents CommunicationBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CommunicationDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents UserId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmStudentId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmTimeStamp As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmSource As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmSubject As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmText As System.Windows.Forms.DataGridViewTextBoxColumn
    Protected WithEvents pnlSchoolCommunicationsSearch As System.Windows.Forms.Panel
    Protected WithEvents btnCommuncationsViewDoc As System.Windows.Forms.Button
    Protected WithEvents btnCommuncationsLinkDoc As System.Windows.Forms.Button
    Friend WithEvents CommunicationPrinter As RegentsScholarshipFrontEnd.CommunicationRecordPrintingControl
End Class
