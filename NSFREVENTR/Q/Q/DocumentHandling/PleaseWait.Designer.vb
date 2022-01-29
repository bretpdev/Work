<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PleaseWait
    Inherits FormBase

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
        Me.Message = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Message
        '
        Me.Message.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Message.Location = New System.Drawing.Point(12, 9)
        Me.Message.Name = "Message"
        Me.Message.Size = New System.Drawing.Size(433, 69)
        Me.Message.TabIndex = 0
        Me.Message.Text = "Please wait while your documents are generated."
        Me.Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PleaseWait
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(457, 87)
        Me.ControlBox = False
        Me.Controls.Add(Me.Message)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "PleaseWait"
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Message As System.Windows.Forms.Label

End Class
