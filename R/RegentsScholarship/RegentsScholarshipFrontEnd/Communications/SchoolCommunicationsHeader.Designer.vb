<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SchoolCommunicationsHeader
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
        Me.btnGetSchoolCommunications = New System.Windows.Forms.Button
        Me.lblSelectSchool = New System.Windows.Forms.Label
        Me.cmbSchoolName = New System.Windows.Forms.ComboBox
        Me.cmbCeebCode = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'btnGetSchoolCommunications
        '
        Me.btnGetSchoolCommunications.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGetSchoolCommunications.AutoSize = True
        Me.btnGetSchoolCommunications.BackColor = System.Drawing.SystemColors.Control
        Me.btnGetSchoolCommunications.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGetSchoolCommunications.Location = New System.Drawing.Point(614, 4)
        Me.btnGetSchoolCommunications.Name = "btnGetSchoolCommunications"
        Me.btnGetSchoolCommunications.Size = New System.Drawing.Size(175, 23)
        Me.btnGetSchoolCommunications.TabIndex = 30
        Me.btnGetSchoolCommunications.Text = "Get School Communications"
        Me.btnGetSchoolCommunications.UseVisualStyleBackColor = False
        '
        'lblSelectSchool
        '
        Me.lblSelectSchool.AutoSize = True
        Me.lblSelectSchool.Location = New System.Drawing.Point(3, 9)
        Me.lblSelectSchool.Name = "lblSelectSchool"
        Me.lblSelectSchool.Size = New System.Drawing.Size(193, 13)
        Me.lblSelectSchool.TabIndex = 31
        Me.lblSelectSchool.Text = "Select a school by name or CEEB code"
        '
        'cmbSchoolName
        '
        Me.cmbSchoolName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbSchoolName.DropDownHeight = 500
        Me.cmbSchoolName.FormattingEnabled = True
        Me.cmbSchoolName.IntegralHeight = False
        Me.cmbSchoolName.Location = New System.Drawing.Point(202, 6)
        Me.cmbSchoolName.Name = "cmbSchoolName"
        Me.cmbSchoolName.Size = New System.Drawing.Size(337, 21)
        Me.cmbSchoolName.TabIndex = 28
        '
        'cmbCeebCode
        '
        Me.cmbCeebCode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbCeebCode.DropDownHeight = 500
        Me.cmbCeebCode.FormattingEnabled = True
        Me.cmbCeebCode.IntegralHeight = False
        Me.cmbCeebCode.Location = New System.Drawing.Point(545, 6)
        Me.cmbCeebCode.Name = "cmbCeebCode"
        Me.cmbCeebCode.Size = New System.Drawing.Size(63, 21)
        Me.cmbCeebCode.TabIndex = 29
        '
        'HighSchoolCommunicationsHeader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnGetSchoolCommunications)
        Me.Controls.Add(Me.lblSelectSchool)
        Me.Controls.Add(Me.cmbSchoolName)
        Me.Controls.Add(Me.cmbCeebCode)
        Me.Name = "HighSchoolCommunicationsHeader"
        Me.Size = New System.Drawing.Size(793, 31)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnGetSchoolCommunications As System.Windows.Forms.Button
    Friend WithEvents lblSelectSchool As System.Windows.Forms.Label
    Friend WithEvents cmbSchoolName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCeebCode As System.Windows.Forms.ComboBox

End Class
