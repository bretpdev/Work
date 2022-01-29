<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DocIdSelection
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DocIdSelections = New System.Windows.Forms.ComboBox()
        Me.OK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(540, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "This borrower is not in OneLink. Please choose to Doc ID for this document."
        '
        'DocIdSelections
        '
        Me.DocIdSelections.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DocIdSelections.FormattingEnabled = True
        Me.DocIdSelections.Location = New System.Drawing.Point(16, 49)
        Me.DocIdSelections.Name = "DocIdSelections"
        Me.DocIdSelections.Size = New System.Drawing.Size(536, 28)
        Me.DocIdSelections.TabIndex = 1
        '
        'OK
        '
        Me.OK.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OK.Location = New System.Drawing.Point(497, 83)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(55, 30)
        Me.OK.TabIndex = 2
        Me.OK.Text = "OK"
        Me.OK.UseVisualStyleBackColor = True
        '
        'DocIdSelection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(566, 119)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.DocIdSelections)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximumSize = New System.Drawing.Size(582, 153)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(582, 153)
        Me.Name = "DocIdSelection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Doc ID Selection"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DocIdSelections As System.Windows.Forms.ComboBox
    Friend WithEvents OK As System.Windows.Forms.Button
End Class
