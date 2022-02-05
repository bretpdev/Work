<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHighSchoolCommunications
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
        Me.HighSchoolCommunicationsHeader1 = New RegentsScholarshipFrontEnd.SchoolCommunicationsHeader
        Me.pnlSchoolCommunicationsSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSchoolCommunicationsSearch
        '
        Me.pnlSchoolCommunicationsSearch.Controls.Add(Me.HighSchoolCommunicationsHeader1)
        '
        'HighSchoolCommunicationsHeader1
        '
        Me.HighSchoolCommunicationsHeader1.Location = New System.Drawing.Point(0, -1)
        Me.HighSchoolCommunicationsHeader1.Name = "HighSchoolCommunicationsHeader1"
        Me.HighSchoolCommunicationsHeader1.Size = New System.Drawing.Size(793, 31)
        Me.HighSchoolCommunicationsHeader1.TabIndex = 0
        '
        'frmHighSchoolCommunications
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 698)
        Me.Name = "frmHighSchoolCommunications"
        Me.Text = "High School Communications"
        Me.pnlSchoolCommunicationsSearch.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents HighSchoolCommunicationsHeader1 As RegentsScholarshipFrontEnd.SchoolCommunicationsHeader
End Class
