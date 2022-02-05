Partial Public Class frmOpenOrAll
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents rbOpen As System.Windows.Forms.RadioButton
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents rbAll As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbAll = New System.Windows.Forms.RadioButton
        Me.rbOpen = New System.Windows.Forms.RadioButton
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.rbAll)
        Me.GroupBox1.Controls.Add(Me.rbOpen)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(184, 96)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'rbAll
        '
        Me.rbAll.BackColor = System.Drawing.Color.Transparent
        Me.rbAll.Location = New System.Drawing.Point(40, 56)
        Me.rbAll.Name = "rbAll"
        Me.rbAll.Size = New System.Drawing.Size(120, 24)
        Me.rbAll.TabIndex = 1
        Me.rbAll.Text = "All Loan(s)"
        Me.rbAll.UseVisualStyleBackColor = False
        '
        'rbOpen
        '
        Me.rbOpen.Checked = True
        Me.rbOpen.Location = New System.Drawing.Point(40, 24)
        Me.rbOpen.Name = "rbOpen"
        Me.rbOpen.Size = New System.Drawing.Size(120, 24)
        Me.rbOpen.TabIndex = 0
        Me.rbOpen.TabStop = True
        Me.rbOpen.Text = "Open Loan(s) Only"
        '
        'btnOK
        '
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(64, 102)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 24)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(64, 132)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 24)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        '
        'frmOpenOrAll
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(198, 162)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximumSize = New System.Drawing.Size(208, 198)
        Me.MinimumSize = New System.Drawing.Size(208, 198)
        Me.Name = "frmOpenOrAll"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "All Loan(s) or Open Loan(s) Only"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub
End Class
