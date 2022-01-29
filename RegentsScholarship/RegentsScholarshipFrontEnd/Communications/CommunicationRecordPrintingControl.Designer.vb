<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CommunicationRecordPrintingControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.btnCommunicationPrint = New System.Windows.Forms.Button
        Me.radCommunicationPrintDateRange = New System.Windows.Forms.RadioButton
        Me.radCommunicationPrintAll = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtpTo = New RegentsScholarshipFrontEnd.NullableDateTimePicker
        Me.dtpFrom = New RegentsScholarshipFrontEnd.NullableDateTimePicker
        Me.grpDateRange = New System.Windows.Forms.GroupBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.grpDateRange.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCommunicationPrint
        '
        Me.btnCommunicationPrint.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCommunicationPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCommunicationPrint.Location = New System.Drawing.Point(4, 54)
        Me.btnCommunicationPrint.Name = "btnCommunicationPrint"
        Me.btnCommunicationPrint.Size = New System.Drawing.Size(280, 32)
        Me.btnCommunicationPrint.TabIndex = 27
        Me.btnCommunicationPrint.Text = "Print"
        Me.btnCommunicationPrint.UseVisualStyleBackColor = True
        '
        'radCommunicationPrintDateRange
        '
        Me.radCommunicationPrintDateRange.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.radCommunicationPrintDateRange.AutoSize = True
        Me.radCommunicationPrintDateRange.Location = New System.Drawing.Point(4, 35)
        Me.radCommunicationPrintDateRange.Name = "radCommunicationPrintDateRange"
        Me.radCommunicationPrintDateRange.Size = New System.Drawing.Size(57, 17)
        Me.radCommunicationPrintDateRange.TabIndex = 26
        Me.radCommunicationPrintDateRange.Text = "Range"
        Me.radCommunicationPrintDateRange.UseVisualStyleBackColor = True
        '
        'radCommunicationPrintAll
        '
        Me.radCommunicationPrintAll.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.radCommunicationPrintAll.AutoSize = True
        Me.radCommunicationPrintAll.Checked = True
        Me.radCommunicationPrintAll.Location = New System.Drawing.Point(4, 11)
        Me.radCommunicationPrintAll.Name = "radCommunicationPrintAll"
        Me.radCommunicationPrintAll.Size = New System.Drawing.Size(36, 17)
        Me.radCommunicationPrintAll.TabIndex = 25
        Me.radCommunicationPrintAll.TabStop = True
        Me.radCommunicationPrintAll.Text = "All"
        Me.radCommunicationPrintAll.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(1, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(24, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Frm"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(106, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "To"
        '
        'dtpTo
        '
        Me.dtpTo.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.dtpTo.CustomFormat = ""
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTo.FormatAsString = "M/d/yyyy"
        Me.dtpTo.Location = New System.Drawing.Point(127, 10)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.NullValue = " "
        Me.dtpTo.Size = New System.Drawing.Size(87, 20)
        Me.dtpTo.TabIndex = 33
        Me.dtpTo.Value = Nothing
        '
        'dtpFrom
        '
        Me.dtpFrom.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.dtpFrom.CustomFormat = ""
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFrom.FormatAsString = "M/d/yyyy"
        Me.dtpFrom.Location = New System.Drawing.Point(25, 10)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.NullValue = " "
        Me.dtpFrom.Size = New System.Drawing.Size(83, 20)
        Me.dtpFrom.TabIndex = 32
        Me.dtpFrom.Value = Nothing
        '
        'grpDateRange
        '
        Me.grpDateRange.Controls.Add(Me.dtpFrom)
        Me.grpDateRange.Controls.Add(Me.dtpTo)
        Me.grpDateRange.Controls.Add(Me.Label1)
        Me.grpDateRange.Controls.Add(Me.Label2)
        Me.grpDateRange.Enabled = False
        Me.grpDateRange.Location = New System.Drawing.Point(67, 20)
        Me.grpDateRange.Name = "grpDateRange"
        Me.grpDateRange.Size = New System.Drawing.Size(217, 34)
        Me.grpDateRange.TabIndex = 34
        Me.grpDateRange.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.radCommunicationPrintAll)
        Me.GroupBox1.Controls.Add(Me.radCommunicationPrintDateRange)
        Me.GroupBox1.Location = New System.Drawing.Point(3, -3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(64, 57)
        Me.GroupBox1.TabIndex = 35
        Me.GroupBox1.TabStop = False
        '
        'CommunicationRecordPrintingControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpDateRange)
        Me.Controls.Add(Me.btnCommunicationPrint)
        Me.Name = "CommunicationRecordPrintingControl"
        Me.Size = New System.Drawing.Size(288, 89)
        Me.grpDateRange.ResumeLayout(False)
        Me.grpDateRange.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCommunicationPrint As System.Windows.Forms.Button
    Friend WithEvents radCommunicationPrintDateRange As System.Windows.Forms.RadioButton
    Friend WithEvents radCommunicationPrintAll As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents dtpTo As RegentsScholarshipFrontEnd.NullableDateTimePicker
    Friend WithEvents grpDateRange As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox

End Class
