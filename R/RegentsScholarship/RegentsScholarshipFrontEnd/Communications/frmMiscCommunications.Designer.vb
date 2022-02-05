<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMiscCommunications
    Inherits frmBaseCommunications

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
        Me.Label1 = New System.Windows.Forms.Label
        Me.pnlSchoolCommunicationsSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSchoolCommunicationsSearch
        '
        Me.pnlSchoolCommunicationsSearch.Controls.Add(Me.Label1)
        Me.pnlSchoolCommunicationsSearch.Size = New System.Drawing.Size(794, 47)
        '
        'btnCommuncationsViewDoc
        '
        Me.btnCommuncationsViewDoc.Enabled = False
        '
        'btnCommuncationsLinkDoc
        '
        Me.btnCommuncationsLinkDoc.Enabled = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(3, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(786, 28)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "This screen should only be used when you don't want the communication associated " & _
            "with a student, district or school."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmMiscCommunications
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 698)
        Me.Name = "frmMiscCommunications"
        Me.Text = "Misc. Communications"
        Me.pnlSchoolCommunicationsSearch.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
