<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BatchReviewItemControl
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
        Me.components = New System.ComponentModel.Container
        Me.chkReviewed = New System.Windows.Forms.CheckBox
        Me.txtLastName = New System.Windows.Forms.TextBox
        Me.BatchQuickReviewItemBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.txtFirstName = New System.Windows.Forms.TextBox
        Me.txtStudentID = New System.Windows.Forms.TextBox
        CType(Me.BatchQuickReviewItemBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkReviewed
        '
        Me.chkReviewed.AutoSize = True
        Me.chkReviewed.Location = New System.Drawing.Point(3, 3)
        Me.chkReviewed.Name = "chkReviewed"
        Me.chkReviewed.Size = New System.Drawing.Size(74, 17)
        Me.chkReviewed.TabIndex = 0
        Me.chkReviewed.Text = "Reviewed"
        Me.chkReviewed.UseVisualStyleBackColor = True
        '
        'txtLastName
        '
        Me.txtLastName.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BatchQuickReviewItemBindingSource, "LastName", True))
        Me.txtLastName.Location = New System.Drawing.Point(112, 3)
        Me.txtLastName.Name = "txtLastName"
        Me.txtLastName.ReadOnly = True
        Me.txtLastName.Size = New System.Drawing.Size(197, 20)
        Me.txtLastName.TabIndex = 1
        '
        'BatchQuickReviewItemBindingSource
        '
        Me.BatchQuickReviewItemBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.BatchQuickReviewItem)
        '
        'txtFirstName
        '
        Me.txtFirstName.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BatchQuickReviewItemBindingSource, "FirstName", True))
        Me.txtFirstName.Location = New System.Drawing.Point(315, 3)
        Me.txtFirstName.Name = "txtFirstName"
        Me.txtFirstName.ReadOnly = True
        Me.txtFirstName.Size = New System.Drawing.Size(197, 20)
        Me.txtFirstName.TabIndex = 2
        '
        'txtStudentID
        '
        Me.txtStudentID.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BatchQuickReviewItemBindingSource, "StateStudentId", True))
        Me.txtStudentID.Location = New System.Drawing.Point(518, 3)
        Me.txtStudentID.Name = "txtStudentID"
        Me.txtStudentID.ReadOnly = True
        Me.txtStudentID.Size = New System.Drawing.Size(189, 20)
        Me.txtStudentID.TabIndex = 3
        '
        'BatchReviewItemControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtStudentID)
        Me.Controls.Add(Me.txtFirstName)
        Me.Controls.Add(Me.txtLastName)
        Me.Controls.Add(Me.chkReviewed)
        Me.Name = "BatchReviewItemControl"
        Me.Size = New System.Drawing.Size(710, 25)
        CType(Me.BatchQuickReviewItemBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkReviewed As System.Windows.Forms.CheckBox
    Friend WithEvents txtLastName As System.Windows.Forms.TextBox
    Friend WithEvents txtFirstName As System.Windows.Forms.TextBox
    Friend WithEvents txtStudentID As System.Windows.Forms.TextBox
    Friend WithEvents BatchQuickReviewItemBindingSource As System.Windows.Forms.BindingSource

End Class
