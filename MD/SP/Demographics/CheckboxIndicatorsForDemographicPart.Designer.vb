<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CheckboxIndicatorsForDemographicPart
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
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.ckbInvalidateFirst = New System.Windows.Forms.CheckBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.ckbVerified = New System.Windows.Forms.CheckBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.ckbNotValid = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ckbConsent = New System.Windows.Forms.CheckBox
        Me.IndicatorsForDemographicPartBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.IndicatorsForDemographicPartBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.ckbInvalidateFirst)
        Me.GroupBox4.Location = New System.Drawing.Point(110, -4)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(50, 29)
        Me.GroupBox4.TabIndex = 97
        Me.GroupBox4.TabStop = False
        '
        'ckbInvalidateFirst
        '
        Me.ckbInvalidateFirst.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.IndicatorsForDemographicPartBindingSource, "InvalidateFirst", True))
        Me.ckbInvalidateFirst.Location = New System.Drawing.Point(18, 7)
        Me.ckbInvalidateFirst.Name = "ckbInvalidateFirst"
        Me.ckbInvalidateFirst.Size = New System.Drawing.Size(16, 20)
        Me.ckbInvalidateFirst.TabIndex = 88
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.ckbVerified)
        Me.GroupBox3.Location = New System.Drawing.Point(55, -4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(50, 29)
        Me.GroupBox3.TabIndex = 96
        Me.GroupBox3.TabStop = False
        '
        'ckbVerified
        '
        Me.ckbVerified.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.IndicatorsForDemographicPartBindingSource, "Verified", True))
        Me.ckbVerified.Location = New System.Drawing.Point(18, 7)
        Me.ckbVerified.Name = "ckbVerified"
        Me.ckbVerified.Size = New System.Drawing.Size(16, 20)
        Me.ckbVerified.TabIndex = 6
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ckbNotValid)
        Me.GroupBox2.Location = New System.Drawing.Point(0, -4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(50, 29)
        Me.GroupBox2.TabIndex = 95
        Me.GroupBox2.TabStop = False
        '
        'ckbNotValid
        '
        Me.ckbNotValid.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.IndicatorsForDemographicPartBindingSource, "NotValid", True))
        Me.ckbNotValid.Location = New System.Drawing.Point(19, 7)
        Me.ckbNotValid.Name = "ckbNotValid"
        Me.ckbNotValid.Size = New System.Drawing.Size(16, 20)
        Me.ckbNotValid.TabIndex = 88
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ckbConsent)
        Me.GroupBox1.Location = New System.Drawing.Point(165, -4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(50, 29)
        Me.GroupBox1.TabIndex = 98
        Me.GroupBox1.TabStop = False
        '
        'ckbConsent
        '
        Me.ckbConsent.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.IndicatorsForDemographicPartBindingSource, "Consent", True))
        Me.ckbConsent.Location = New System.Drawing.Point(18, 7)
        Me.ckbConsent.Name = "ckbConsent"
        Me.ckbConsent.Size = New System.Drawing.Size(16, 20)
        Me.ckbConsent.TabIndex = 88
        '
        'IndicatorsForDemographicPartBindingSource
        '
        Me.IndicatorsForDemographicPartBindingSource.DataSource = GetType(SP.IndicatorsForDemographicPart)
        '
        'CheckboxIndicatorsForDemographicPart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "CheckboxIndicatorsForDemographicPart"
        Me.Size = New System.Drawing.Size(220, 27)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.IndicatorsForDemographicPartBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Public WithEvents ckbInvalidateFirst As System.Windows.Forms.CheckBox
    Public WithEvents ckbVerified As System.Windows.Forms.CheckBox
    Public WithEvents ckbNotValid As System.Windows.Forms.CheckBox
    Friend WithEvents IndicatorsForDemographicPartBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents ckbConsent As System.Windows.Forms.CheckBox

End Class
