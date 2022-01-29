<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Address
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
        Me.Label27 = New System.Windows.Forms.Label
        Me.cbState = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.txtAddr2 = New System.Windows.Forms.TextBox
        Me.txtCity = New System.Windows.Forms.TextBox
        Me.txtZip = New System.Windows.Forms.TextBox
        Me.txtAddr1 = New System.Windows.Forms.TextBox
        Me.lblMBL = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label27
        '
        Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(141, 79)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(33, 13)
        Me.Label27.TabIndex = 63
        Me.Label27.Text = "Zip:"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'cbState
        '
        Me.cbState.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbState.ItemHeight = 13
        Me.cbState.Items.AddRange(New Object() {"", "AA", "AE", "AK", "AL", "AP", "AR", "AS", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "FM", "GA", "GU", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MH", "MI", "MN", "MO", "MP", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "PR", "PW", "RI", "SC", "SD", "TN", "TX", "UT", "VA", "VI", "VT", "WA", "WI", "WV", "WY"})
        Me.cbState.Location = New System.Drawing.Point(82, 75)
        Me.cbState.Name = "cbState"
        Me.cbState.Size = New System.Drawing.Size(48, 21)
        Me.cbState.Sorted = True
        Me.cbState.TabIndex = 57
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 16)
        Me.Label3.TabIndex = 62
        Me.Label3.Text = "Address 1:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(2, 27)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 16)
        Me.Label6.TabIndex = 61
        Me.Label6.Text = "Address 2:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label23
        '
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(2, 51)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(32, 16)
        Me.Label23.TabIndex = 60
        Me.Label23.Text = "City:"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label24
        '
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(2, 75)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(40, 16)
        Me.Label24.TabIndex = 59
        Me.Label24.Text = "State:"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtAddr2
        '
        Me.txtAddr2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddr2.Location = New System.Drawing.Point(82, 27)
        Me.txtAddr2.MaxLength = 30
        Me.txtAddr2.Name = "txtAddr2"
        Me.txtAddr2.Size = New System.Drawing.Size(272, 20)
        Me.txtAddr2.TabIndex = 55
        '
        'txtCity
        '
        Me.txtCity.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCity.Location = New System.Drawing.Point(82, 51)
        Me.txtCity.MaxLength = 20
        Me.txtCity.Name = "txtCity"
        Me.txtCity.Size = New System.Drawing.Size(193, 20)
        Me.txtCity.TabIndex = 56
        '
        'txtZip
        '
        Me.txtZip.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtZip.Location = New System.Drawing.Point(175, 75)
        Me.txtZip.MaxLength = 9
        Me.txtZip.Name = "txtZip"
        Me.txtZip.Size = New System.Drawing.Size(100, 20)
        Me.txtZip.TabIndex = 58
        '
        'txtAddr1
        '
        Me.txtAddr1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddr1.Location = New System.Drawing.Point(82, 3)
        Me.txtAddr1.MaxLength = 30
        Me.txtAddr1.Name = "txtAddr1"
        Me.txtAddr1.Size = New System.Drawing.Size(272, 20)
        Me.txtAddr1.TabIndex = 54
        '
        'lblMBL
        '
        Me.lblMBL.AutoSize = True
        Me.lblMBL.Location = New System.Drawing.Point(298, 81)
        Me.lblMBL.Name = "lblMBL"
        Me.lblMBL.Size = New System.Drawing.Size(47, 13)
        Me.lblMBL.TabIndex = 64
        Me.lblMBL.Text = "Ph Type"
        '
        'Address
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lblMBL)
        Me.Controls.Add(Me.Label27)
        Me.Controls.Add(Me.cbState)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.Label24)
        Me.Controls.Add(Me.txtAddr2)
        Me.Controls.Add(Me.txtCity)
        Me.Controls.Add(Me.txtZip)
        Me.Controls.Add(Me.txtAddr1)
        Me.Name = "Address"
        Me.Size = New System.Drawing.Size(358, 100)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Public WithEvents cbState As System.Windows.Forms.ComboBox
    Public WithEvents txtAddr2 As System.Windows.Forms.TextBox
    Public WithEvents txtCity As System.Windows.Forms.TextBox
    Public WithEvents txtZip As System.Windows.Forms.TextBox
    Public WithEvents txtAddr1 As System.Windows.Forms.TextBox
    Friend WithEvents lblMBL As System.Windows.Forms.Label

End Class
