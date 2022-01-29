<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DemographicsForUpdate
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblEmail = New System.Windows.Forms.Label
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.DemographicsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Other2Phn = New SP.Phone
        Me.OtherPhn = New SP.Phone
        Me.HomePhn = New SP.Phone
        Me.Addr = New SP.Address
        CType(Me.DemographicsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Home:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 128)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Other:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 151)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Other 2:"
        '
        'lblEmail
        '
        Me.lblEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmail.Location = New System.Drawing.Point(2, 171)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(40, 16)
        Me.lblEmail.TabIndex = 48
        Me.lblEmail.Text = "E-mail:"
        Me.lblEmail.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtEmail
        '
        Me.txtEmail.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEmail.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DemographicsBindingSource, "Email", True))
        Me.txtEmail.Location = New System.Drawing.Point(82, 171)
        Me.txtEmail.MaxLength = 56
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(275, 20)
        Me.txtEmail.TabIndex = 47
        '
        'DemographicsBindingSource
        '
        Me.DemographicsBindingSource.DataSource = GetType(SP.Demographics)
        '
        'Other2Phn
        '
        Me.Other2Phn.Location = New System.Drawing.Point(82, 145)
        Me.Other2Phn.Name = "Other2Phn"
        Me.Other2Phn.Size = New System.Drawing.Size(275, 26)
        Me.Other2Phn.TabIndex = 5
        '
        'OtherPhn
        '
        Me.OtherPhn.Location = New System.Drawing.Point(82, 122)
        Me.OtherPhn.Name = "OtherPhn"
        Me.OtherPhn.Size = New System.Drawing.Size(275, 26)
        Me.OtherPhn.TabIndex = 3
        '
        'HomePhn
        '
        Me.HomePhn.Location = New System.Drawing.Point(82, 98)
        Me.HomePhn.Name = "HomePhn"
        Me.HomePhn.Size = New System.Drawing.Size(275, 26)
        Me.HomePhn.TabIndex = 1
        '
        'Addr
        '
        Me.Addr.Location = New System.Drawing.Point(1, 2)
        Me.Addr.Name = "Addr"
        Me.Addr.Size = New System.Drawing.Size(358, 100)
        Me.Addr.TabIndex = 0
        '
        'DemographicsForUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Other2Phn)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.OtherPhn)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.HomePhn)
        Me.Controls.Add(Me.Addr)
        Me.Name = "DemographicsForUpdate"
        Me.Size = New System.Drawing.Size(360, 196)
        CType(Me.DemographicsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Addr As SP.Address
    Public WithEvents HomePhn As SP.Phone
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents OtherPhn As SP.Phone
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Other2Phn As SP.Phone
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents DemographicsBindingSource As System.Windows.Forms.BindingSource

End Class
