<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDistrictCommunications
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
        Me.DistrictCommunicationHeader1 = New RegentsScholarshipFrontEnd.DistrictCommunicationHeader
        Me.pnlSchoolCommunicationsSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSchoolCommunicationsSearch
        '
        Me.pnlSchoolCommunicationsSearch.Controls.Add(Me.DistrictCommunicationHeader1)
        '
        'DistrictCommunicationHeader1
        '
        Me.DistrictCommunicationHeader1.Location = New System.Drawing.Point(-1, -1)
        Me.DistrictCommunicationHeader1.Name = "DistrictCommunicationHeader1"
        Me.DistrictCommunicationHeader1.Size = New System.Drawing.Size(793, 31)
        Me.DistrictCommunicationHeader1.TabIndex = 0
        '
        'frmDistrictCommunications
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 698)
        Me.Name = "frmDistrictCommunications"
        Me.Text = "District Communications"
        Me.pnlSchoolCommunicationsSearch.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DistrictCommunicationHeader1 As RegentsScholarshipFrontEnd.DistrictCommunicationHeader
End Class
