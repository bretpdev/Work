Partial Public Class frmBankruptcy
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbLenderList As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbList As System.Windows.Forms.RadioButton
    Friend WithEvents rbType As System.Windows.Forms.RadioButton
    Friend WithEvents tbLender As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmBankruptcy))
        Me.cbLenderList = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbLender = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.rbList = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbType = New System.Windows.Forms.RadioButton
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbLenderList
        '
        Me.cbLenderList.Enabled = False
        Me.cbLenderList.Location = New System.Drawing.Point(24, 40)
        Me.cbLenderList.Name = "cbLenderList"
        Me.cbLenderList.Size = New System.Drawing.Size(280, 21)
        Me.cbLenderList.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(296, 23)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Please select a lender,"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(296, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "or enter a lender."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tbLender
        '
        Me.tbLender.Enabled = False
        Me.tbLender.Location = New System.Drawing.Point(24, 88)
        Me.tbLender.Name = "tbLender"
        Me.tbLender.Size = New System.Drawing.Size(280, 20)
        Me.tbLender.TabIndex = 3
        Me.tbLender.Text = ""
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(88, 136)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(72, 24)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        '
        'rbList
        '
        Me.rbList.Location = New System.Drawing.Point(8, 40)
        Me.rbList.Name = "rbList"
        Me.rbList.Size = New System.Drawing.Size(16, 24)
        Me.rbList.TabIndex = 5
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbType)
        Me.GroupBox1.Controls.Add(Me.rbList)
        Me.GroupBox1.Controls.Add(Me.tbLender)
        Me.GroupBox1.Controls.Add(Me.cbLenderList)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(312, 120)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        '
        'rbType
        '
        Me.rbType.Location = New System.Drawing.Point(8, 88)
        Me.rbType.Name = "rbType"
        Me.rbType.Size = New System.Drawing.Size(16, 24)
        Me.rbType.TabIndex = 6
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(168, 136)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        '
        'frmBankruptcy
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(328, 166)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnOK)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(336, 193)
        Me.MinimumSize = New System.Drawing.Size(336, 193)
        Me.Name = "frmBankruptcy"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bankruptcy Check"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
End Class
