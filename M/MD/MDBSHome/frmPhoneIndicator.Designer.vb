<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPhoneIndicator

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
        Me.btnOK = New System.Windows.Forms.Button
        Me.chkHome = New System.Windows.Forms.CheckBox
        Me.chkOther = New System.Windows.Forms.CheckBox
        Me.chkOther2 = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbxHome = New System.Windows.Forms.TextBox
        Me.tbxOther = New System.Windows.Forms.TextBox
        Me.tbxOther2 = New System.Windows.Forms.TextBox
        Me.tbxManual = New System.Windows.Forms.TextBox
        Me.chkManual = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Enabled = False
        Me.btnOK.Location = New System.Drawing.Point(82, 185)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'chkHome
        '
        Me.chkHome.AutoSize = True
        Me.chkHome.Enabled = False
        Me.chkHome.Location = New System.Drawing.Point(46, 59)
        Me.chkHome.Name = "chkHome"
        Me.chkHome.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkHome.Size = New System.Drawing.Size(54, 17)
        Me.chkHome.TabIndex = 1
        Me.chkHome.Text = "Home"
        Me.chkHome.UseVisualStyleBackColor = True
        '
        'chkOther
        '
        Me.chkOther.AutoSize = True
        Me.chkOther.Enabled = False
        Me.chkOther.Location = New System.Drawing.Point(48, 90)
        Me.chkOther.Name = "chkOther"
        Me.chkOther.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkOther.Size = New System.Drawing.Size(52, 17)
        Me.chkOther.TabIndex = 2
        Me.chkOther.Text = "Other"
        Me.chkOther.UseVisualStyleBackColor = True
        '
        'chkOther2
        '
        Me.chkOther2.AutoSize = True
        Me.chkOther2.Enabled = False
        Me.chkOther2.Location = New System.Drawing.Point(39, 121)
        Me.chkOther2.Name = "chkOther2"
        Me.chkOther2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkOther2.Size = New System.Drawing.Size(61, 17)
        Me.chkOther2.TabIndex = 3
        Me.chkOther2.Text = "Other 2"
        Me.chkOther2.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(43, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(163, 44)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Please choose the phone where you reached the borrower."
        '
        'tbxHome
        '
        Me.tbxHome.Location = New System.Drawing.Point(106, 56)
        Me.tbxHome.Name = "tbxHome"
        Me.tbxHome.ReadOnly = True
        Me.tbxHome.Size = New System.Drawing.Size(100, 20)
        Me.tbxHome.TabIndex = 5
        '
        'tbxOther
        '
        Me.tbxOther.Location = New System.Drawing.Point(106, 87)
        Me.tbxOther.Name = "tbxOther"
        Me.tbxOther.ReadOnly = True
        Me.tbxOther.Size = New System.Drawing.Size(100, 20)
        Me.tbxOther.TabIndex = 6
        '
        'tbxOther2
        '
        Me.tbxOther2.Location = New System.Drawing.Point(106, 118)
        Me.tbxOther2.Name = "tbxOther2"
        Me.tbxOther2.ReadOnly = True
        Me.tbxOther2.Size = New System.Drawing.Size(100, 20)
        Me.tbxOther2.TabIndex = 7
        '
        'tbxManual
        '
        Me.tbxManual.Location = New System.Drawing.Point(106, 149)
        Me.tbxManual.Name = "tbxManual"
        Me.tbxManual.Size = New System.Drawing.Size(100, 20)
        Me.tbxManual.TabIndex = 9
        '
        'chkManual
        '
        Me.chkManual.AutoSize = True
        Me.chkManual.Location = New System.Drawing.Point(12, 152)
        Me.chkManual.Name = "chkManual"
        Me.chkManual.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkManual.Size = New System.Drawing.Size(88, 17)
        Me.chkManual.TabIndex = 8
        Me.chkManual.Text = "Manual Input"
        Me.chkManual.UseVisualStyleBackColor = True
        '
        'frmPhoneIndicator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(233, 220)
        Me.Controls.Add(Me.tbxManual)
        Me.Controls.Add(Me.chkManual)
        Me.Controls.Add(Me.tbxOther2)
        Me.Controls.Add(Me.tbxOther)
        Me.Controls.Add(Me.tbxHome)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkOther2)
        Me.Controls.Add(Me.chkOther)
        Me.Controls.Add(Me.chkHome)
        Me.Controls.Add(Me.btnOK)
        Me.Name = "frmPhoneIndicator"
        Me.Text = "Choose a phone number"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents chkHome As System.Windows.Forms.CheckBox
    Friend WithEvents chkOther As System.Windows.Forms.CheckBox
    Friend WithEvents chkOther2 As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbxHome As System.Windows.Forms.TextBox
    Friend WithEvents tbxOther As System.Windows.Forms.TextBox
    Friend WithEvents tbxOther2 As System.Windows.Forms.TextBox
    Friend WithEvents tbxManual As System.Windows.Forms.TextBox
    Friend WithEvents chkManual As System.Windows.Forms.CheckBox
End Class
