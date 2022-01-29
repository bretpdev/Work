<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CallCategorizationCommentOnly
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
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.CallCategorizationEntryBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        CType(Me.CallCategorizationEntryBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtComments
        '
        Me.txtComments.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtComments.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.CallCategorizationEntryBindingSource, "Comments", True))
        Me.txtComments.Location = New System.Drawing.Point(78, 3)
        Me.txtComments.MaxLength = 30
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.Size = New System.Drawing.Size(209, 24)
        Me.txtComments.TabIndex = 0
        '
        'CallCategorizationEntryBindingSource
        '
        Me.CallCategorizationEntryBindingSource.DataSource = GetType(SP.CallCategorizationEntry)
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(4, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Comments"
        '
        'CallCategorizationCommentOnly
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtComments)
        Me.Name = "CallCategorizationCommentOnly"
        Me.Size = New System.Drawing.Size(292, 30)
        CType(Me.CallCategorizationEntryBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CallCategorizationEntryBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents txtComments As System.Windows.Forms.TextBox

End Class
