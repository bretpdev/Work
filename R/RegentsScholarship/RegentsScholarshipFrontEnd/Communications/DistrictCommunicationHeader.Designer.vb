<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DistrictCommunicationHeader
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
        Me.lbl1 = New System.Windows.Forms.Label
        Me.cmbDistrictName = New System.Windows.Forms.ComboBox
        Me.btnGetDistrictCommunications = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lbl1
        '
        Me.lbl1.AutoSize = True
        Me.lbl1.Location = New System.Drawing.Point(3, 9)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(82, 13)
        Me.lbl1.TabIndex = 33
        Me.lbl1.Text = "Select a district:"
        '
        'cmbDistrictName
        '
        Me.cmbDistrictName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbDistrictName.DropDownHeight = 500
        Me.cmbDistrictName.FormattingEnabled = True
        Me.cmbDistrictName.IntegralHeight = False
        Me.cmbDistrictName.Location = New System.Drawing.Point(91, 6)
        Me.cmbDistrictName.Name = "cmbDistrictName"
        Me.cmbDistrictName.Size = New System.Drawing.Size(517, 21)
        Me.cmbDistrictName.TabIndex = 32
        '
        'btnGetDistrictCommunications
        '
        Me.btnGetDistrictCommunications.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGetDistrictCommunications.AutoSize = True
        Me.btnGetDistrictCommunications.BackColor = System.Drawing.SystemColors.Control
        Me.btnGetDistrictCommunications.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGetDistrictCommunications.Location = New System.Drawing.Point(614, 5)
        Me.btnGetDistrictCommunications.Name = "btnGetDistrictCommunications"
        Me.btnGetDistrictCommunications.Size = New System.Drawing.Size(176, 23)
        Me.btnGetDistrictCommunications.TabIndex = 34
        Me.btnGetDistrictCommunications.Text = "Get District Communications"
        Me.btnGetDistrictCommunications.UseVisualStyleBackColor = False
        '
        'DistrictCommunicationHeader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnGetDistrictCommunications)
        Me.Controls.Add(Me.lbl1)
        Me.Controls.Add(Me.cmbDistrictName)
        Me.Name = "DistrictCommunicationHeader"
        Me.Size = New System.Drawing.Size(793, 31)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbl1 As System.Windows.Forms.Label
    Friend WithEvents cmbDistrictName As System.Windows.Forms.ComboBox
    Friend WithEvents btnGetDistrictCommunications As System.Windows.Forms.Button

End Class
