<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmJrHighSchoolCommunications
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
        Me.SchoolCommunicationsHeader1 = New RegentsScholarshipFrontEnd.SchoolCommunicationsHeader
        Me.pnlSchoolCommunicationsSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSchoolCommunicationsSearch
        '
        Me.pnlSchoolCommunicationsSearch.Controls.Add(Me.SchoolCommunicationsHeader1)
        '
        'SchoolCommunicationsHeader1
        '
        Me.SchoolCommunicationsHeader1.Location = New System.Drawing.Point(0, -1)
        Me.SchoolCommunicationsHeader1.Name = "SchoolCommunicationsHeader1"
        Me.SchoolCommunicationsHeader1.Size = New System.Drawing.Size(793, 31)
        Me.SchoolCommunicationsHeader1.TabIndex = 0
        '
        'frmJrHighSchoolCommunications
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 698)
        Me.Name = "frmJrHighSchoolCommunications"
        Me.Text = "Jr High School Communications"
        Me.pnlSchoolCommunicationsSearch.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SchoolCommunicationsHeader1 As RegentsScholarshipFrontEnd.SchoolCommunicationsHeader
End Class
