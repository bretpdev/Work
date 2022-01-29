<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ManualComments
    Inherits Q.FormBase

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
        Me.txtComment = New System.Windows.Forms.TextBox
        Me.UserProvidedCommentBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblRemainingChar = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        CType(Me.UserProvidedCommentBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtComment
        '
        Me.txtComment.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.UserProvidedCommentBindingSource, "Text", True))
        Me.txtComment.Location = New System.Drawing.Point(16, 34)
        Me.txtComment.MaxLength = 32000
        Me.txtComment.Multiline = True
        Me.txtComment.Name = "txtComment"
        Me.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComment.Size = New System.Drawing.Size(349, 131)
        Me.txtComment.TabIndex = 0
        '
        'UserProvidedCommentBindingSource
        '
        Me.UserProvidedCommentBindingSource.DataSource = GetType(Q.UserProvidedComment)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(162, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Enter Additional Comments Here:"
        '
        'lblRemainingChar
        '
        Me.lblRemainingChar.AutoSize = True
        Me.lblRemainingChar.Location = New System.Drawing.Point(23, 187)
        Me.lblRemainingChar.Name = "lblRemainingChar"
        Me.lblRemainingChar.Size = New System.Drawing.Size(0, 13)
        Me.lblRemainingChar.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(57, 187)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(111, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Characters Remaining"
        '
        'btnOk
        '
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(196, 182)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 4
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(286, 182)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'ManualComments
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(387, 218)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblRemainingChar)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtComment)
        Me.Name = "ManualComments"
        Me.Text = "ManualComments"
        CType(Me.UserProvidedCommentBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtComment As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblRemainingChar As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents UserProvidedCommentBindingSource As System.Windows.Forms.BindingSource
End Class
