<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Phone
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
        Me.txtExt = New System.Windows.Forms.TextBox
        Me.Label48 = New System.Windows.Forms.Label
        Me.Label31 = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.txtPhone1 = New System.Windows.Forms.TextBox
        Me.txtPhone2 = New System.Windows.Forms.TextBox
        Me.txtPhone3 = New System.Windows.Forms.TextBox
        Me.cbxPhnMBL = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'txtExt
        '
        Me.txtExt.Location = New System.Drawing.Point(161, 3)
        Me.txtExt.MaxLength = 4
        Me.txtExt.Name = "txtExt"
        Me.txtExt.Size = New System.Drawing.Size(48, 20)
        Me.txtExt.TabIndex = 93
        '
        'Label48
        '
        Me.Label48.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label48.Location = New System.Drawing.Point(137, 0)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(26, 20)
        Me.Label48.TabIndex = 92
        Me.Label48.Text = "Ext:"
        Me.Label48.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label31
        '
        Me.Label31.Location = New System.Drawing.Point(73, 3)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(8, 16)
        Me.Label31.TabIndex = 91
        Me.Label31.Text = "-"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label29
        '
        Me.Label29.Location = New System.Drawing.Point(33, 3)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(8, 16)
        Me.Label29.TabIndex = 90
        Me.Label29.Text = "-"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtPhone1
        '
        Me.txtPhone1.Location = New System.Drawing.Point(1, 3)
        Me.txtPhone1.MaxLength = 3
        Me.txtPhone1.Name = "txtPhone1"
        Me.txtPhone1.Size = New System.Drawing.Size(32, 20)
        Me.txtPhone1.TabIndex = 86
        '
        'txtPhone2
        '
        Me.txtPhone2.Location = New System.Drawing.Point(41, 3)
        Me.txtPhone2.MaxLength = 3
        Me.txtPhone2.Name = "txtPhone2"
        Me.txtPhone2.Size = New System.Drawing.Size(32, 20)
        Me.txtPhone2.TabIndex = 87
        '
        'txtPhone3
        '
        Me.txtPhone3.Location = New System.Drawing.Point(81, 3)
        Me.txtPhone3.MaxLength = 4
        Me.txtPhone3.Name = "txtPhone3"
        Me.txtPhone3.Size = New System.Drawing.Size(48, 20)
        Me.txtPhone3.TabIndex = 88
        '
        'cbxPhnMBL
        '
        Me.cbxPhnMBL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxPhnMBL.FormattingEnabled = True
        Me.cbxPhnMBL.Items.AddRange(New Object() {"M", "L", "U"})
        Me.cbxPhnMBL.Location = New System.Drawing.Point(222, 3)
        Me.cbxPhnMBL.Name = "cbxPhnMBL"
        Me.cbxPhnMBL.Size = New System.Drawing.Size(38, 21)
        Me.cbxPhnMBL.TabIndex = 94
        '
        'Phone
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cbxPhnMBL)
        Me.Controls.Add(Me.txtExt)
        Me.Controls.Add(Me.Label48)
        Me.Controls.Add(Me.Label31)
        Me.Controls.Add(Me.Label29)
        Me.Controls.Add(Me.txtPhone1)
        Me.Controls.Add(Me.txtPhone2)
        Me.Controls.Add(Me.txtPhone3)
        Me.Name = "Phone"
        Me.Size = New System.Drawing.Size(271, 26)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents txtExt As System.Windows.Forms.TextBox
    Public WithEvents Label48 As System.Windows.Forms.Label
    Public WithEvents Label31 As System.Windows.Forms.Label
    Public WithEvents Label29 As System.Windows.Forms.Label
    Public WithEvents txtPhone1 As System.Windows.Forms.TextBox
    Public WithEvents txtPhone2 As System.Windows.Forms.TextBox
    Public WithEvents txtPhone3 As System.Windows.Forms.TextBox
    Friend WithEvents cbxPhnMBL As System.Windows.Forms.ComboBox

End Class
