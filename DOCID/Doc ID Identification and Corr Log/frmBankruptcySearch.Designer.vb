Partial Public Class frmBankruptcySearch
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents IL As System.Windows.Forms.ImageList
    Friend WithEvents Select1 As System.Windows.Forms.Button
    Friend WithEvents Results1 As System.Windows.Forms.TextBox
    Friend WithEvents Results2 As System.Windows.Forms.TextBox
    Friend WithEvents Select2 As System.Windows.Forms.Button
    Friend WithEvents Results3 As System.Windows.Forms.TextBox
    Friend WithEvents Select3 As System.Windows.Forms.Button
    Friend WithEvents Results4 As System.Windows.Forms.TextBox
    Friend WithEvents Select4 As System.Windows.Forms.Button
    Friend WithEvents Results5 As System.Windows.Forms.TextBox
    Friend WithEvents Select5 As System.Windows.Forms.Button
    Friend WithEvents Results6 As System.Windows.Forms.TextBox
    Friend WithEvents Select6 As System.Windows.Forms.Button
    Friend WithEvents Results7 As System.Windows.Forms.TextBox
    Friend WithEvents Select7 As System.Windows.Forms.Button
    Friend WithEvents Results8 As System.Windows.Forms.TextBox
    Friend WithEvents Select8 As System.Windows.Forms.Button
    Friend WithEvents Results9 As System.Windows.Forms.TextBox
    Friend WithEvents Select9 As System.Windows.Forms.Button
    Friend WithEvents Results10 As System.Windows.Forms.TextBox
    Friend WithEvents Select10 As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tbCaseNumber As System.Windows.Forms.TextBox
    Friend WithEvents Searchlbl As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmBankruptcySearch))
        Me.IL = New System.Windows.Forms.ImageList(Me.components)
        Me.Select1 = New System.Windows.Forms.Button
        Me.Results1 = New System.Windows.Forms.TextBox
        Me.Results2 = New System.Windows.Forms.TextBox
        Me.Select2 = New System.Windows.Forms.Button
        Me.Results3 = New System.Windows.Forms.TextBox
        Me.Select3 = New System.Windows.Forms.Button
        Me.Results4 = New System.Windows.Forms.TextBox
        Me.Select4 = New System.Windows.Forms.Button
        Me.Results5 = New System.Windows.Forms.TextBox
        Me.Select5 = New System.Windows.Forms.Button
        Me.Results6 = New System.Windows.Forms.TextBox
        Me.Select6 = New System.Windows.Forms.Button
        Me.Results7 = New System.Windows.Forms.TextBox
        Me.Select7 = New System.Windows.Forms.Button
        Me.Results8 = New System.Windows.Forms.TextBox
        Me.Select8 = New System.Windows.Forms.Button
        Me.Results9 = New System.Windows.Forms.TextBox
        Me.Select9 = New System.Windows.Forms.Button
        Me.Results10 = New System.Windows.Forms.TextBox
        Me.Select10 = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.tbCaseNumber = New System.Windows.Forms.TextBox
        Me.Searchlbl = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'IL
        '
        Me.IL.ImageSize = New System.Drawing.Size(16, 16)
        Me.IL.ImageStream = CType(resources.GetObject("IL.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.IL.TransparentColor = System.Drawing.Color.Transparent
        '
        'Select1
        '
        Me.Select1.Enabled = False
        Me.Select1.Location = New System.Drawing.Point(8, 104)
        Me.Select1.Name = "Select1"
        Me.Select1.Size = New System.Drawing.Size(75, 20)
        Me.Select1.TabIndex = 3
        Me.Select1.TabStop = False
        Me.Select1.Text = "Select"
        '
        'Results1
        '
        Me.Results1.Enabled = False
        Me.Results1.Location = New System.Drawing.Point(88, 104)
        Me.Results1.Name = "Results1"
        Me.Results1.Size = New System.Drawing.Size(228, 20)
        Me.Results1.TabIndex = 1
        Me.Results1.TabStop = False
        Me.Results1.Text = ""
        '
        'Results2
        '
        Me.Results2.Enabled = False
        Me.Results2.Location = New System.Drawing.Point(88, 124)
        Me.Results2.Name = "Results2"
        Me.Results2.Size = New System.Drawing.Size(228, 20)
        Me.Results2.TabIndex = 3
        Me.Results2.TabStop = False
        Me.Results2.Text = ""
        '
        'Select2
        '
        Me.Select2.Enabled = False
        Me.Select2.Location = New System.Drawing.Point(8, 124)
        Me.Select2.Name = "Select2"
        Me.Select2.Size = New System.Drawing.Size(75, 20)
        Me.Select2.TabIndex = 2
        Me.Select2.TabStop = False
        Me.Select2.Text = "Select"
        '
        'Results3
        '
        Me.Results3.Enabled = False
        Me.Results3.Location = New System.Drawing.Point(88, 144)
        Me.Results3.Name = "Results3"
        Me.Results3.Size = New System.Drawing.Size(228, 20)
        Me.Results3.TabIndex = 5
        Me.Results3.TabStop = False
        Me.Results3.Text = ""
        '
        'Select3
        '
        Me.Select3.Enabled = False
        Me.Select3.Location = New System.Drawing.Point(8, 144)
        Me.Select3.Name = "Select3"
        Me.Select3.Size = New System.Drawing.Size(75, 20)
        Me.Select3.TabIndex = 4
        Me.Select3.TabStop = False
        Me.Select3.Text = "Select"
        '
        'Results4
        '
        Me.Results4.Enabled = False
        Me.Results4.Location = New System.Drawing.Point(88, 164)
        Me.Results4.Name = "Results4"
        Me.Results4.Size = New System.Drawing.Size(228, 20)
        Me.Results4.TabIndex = 7
        Me.Results4.TabStop = False
        Me.Results4.Text = ""
        '
        'Select4
        '
        Me.Select4.Enabled = False
        Me.Select4.Location = New System.Drawing.Point(8, 164)
        Me.Select4.Name = "Select4"
        Me.Select4.Size = New System.Drawing.Size(75, 20)
        Me.Select4.TabIndex = 6
        Me.Select4.TabStop = False
        Me.Select4.Text = "Select"
        '
        'Results5
        '
        Me.Results5.Enabled = False
        Me.Results5.Location = New System.Drawing.Point(88, 184)
        Me.Results5.Name = "Results5"
        Me.Results5.Size = New System.Drawing.Size(228, 20)
        Me.Results5.TabIndex = 9
        Me.Results5.TabStop = False
        Me.Results5.Text = ""
        '
        'Select5
        '
        Me.Select5.Enabled = False
        Me.Select5.Location = New System.Drawing.Point(8, 184)
        Me.Select5.Name = "Select5"
        Me.Select5.Size = New System.Drawing.Size(75, 20)
        Me.Select5.TabIndex = 8
        Me.Select5.TabStop = False
        Me.Select5.Text = "Select"
        '
        'Results6
        '
        Me.Results6.Enabled = False
        Me.Results6.Location = New System.Drawing.Point(88, 204)
        Me.Results6.Name = "Results6"
        Me.Results6.Size = New System.Drawing.Size(228, 20)
        Me.Results6.TabIndex = 11
        Me.Results6.TabStop = False
        Me.Results6.Text = ""
        '
        'Select6
        '
        Me.Select6.Enabled = False
        Me.Select6.Location = New System.Drawing.Point(8, 204)
        Me.Select6.Name = "Select6"
        Me.Select6.Size = New System.Drawing.Size(75, 20)
        Me.Select6.TabIndex = 10
        Me.Select6.TabStop = False
        Me.Select6.Text = "Select"
        '
        'Results7
        '
        Me.Results7.Enabled = False
        Me.Results7.Location = New System.Drawing.Point(88, 224)
        Me.Results7.Name = "Results7"
        Me.Results7.Size = New System.Drawing.Size(228, 20)
        Me.Results7.TabIndex = 13
        Me.Results7.TabStop = False
        Me.Results7.Text = ""
        '
        'Select7
        '
        Me.Select7.Enabled = False
        Me.Select7.Location = New System.Drawing.Point(8, 224)
        Me.Select7.Name = "Select7"
        Me.Select7.Size = New System.Drawing.Size(75, 20)
        Me.Select7.TabIndex = 12
        Me.Select7.TabStop = False
        Me.Select7.Text = "Select"
        '
        'Results8
        '
        Me.Results8.Enabled = False
        Me.Results8.Location = New System.Drawing.Point(88, 244)
        Me.Results8.Name = "Results8"
        Me.Results8.Size = New System.Drawing.Size(228, 20)
        Me.Results8.TabIndex = 15
        Me.Results8.TabStop = False
        Me.Results8.Text = ""
        '
        'Select8
        '
        Me.Select8.Enabled = False
        Me.Select8.Location = New System.Drawing.Point(8, 244)
        Me.Select8.Name = "Select8"
        Me.Select8.Size = New System.Drawing.Size(75, 20)
        Me.Select8.TabIndex = 14
        Me.Select8.TabStop = False
        Me.Select8.Text = "Select"
        '
        'Results9
        '
        Me.Results9.Enabled = False
        Me.Results9.Location = New System.Drawing.Point(88, 264)
        Me.Results9.Name = "Results9"
        Me.Results9.Size = New System.Drawing.Size(228, 20)
        Me.Results9.TabIndex = 17
        Me.Results9.TabStop = False
        Me.Results9.Text = ""
        '
        'Select9
        '
        Me.Select9.Enabled = False
        Me.Select9.Location = New System.Drawing.Point(8, 264)
        Me.Select9.Name = "Select9"
        Me.Select9.Size = New System.Drawing.Size(75, 20)
        Me.Select9.TabIndex = 16
        Me.Select9.TabStop = False
        Me.Select9.Text = "Select"
        '
        'Results10
        '
        Me.Results10.Enabled = False
        Me.Results10.Location = New System.Drawing.Point(88, 284)
        Me.Results10.Name = "Results10"
        Me.Results10.Size = New System.Drawing.Size(228, 20)
        Me.Results10.TabIndex = 19
        Me.Results10.TabStop = False
        Me.Results10.Text = ""
        '
        'Select10
        '
        Me.Select10.Enabled = False
        Me.Select10.Location = New System.Drawing.Point(8, 284)
        Me.Select10.Name = "Select10"
        Me.Select10.Size = New System.Drawing.Size(75, 20)
        Me.Select10.TabIndex = 18
        Me.Select10.TabStop = False
        Me.Select10.Text = "Select"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(124, 76)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "Search"
        '
        'tbCaseNumber
        '
        Me.tbCaseNumber.Location = New System.Drawing.Point(56, 24)
        Me.tbCaseNumber.Name = "tbCaseNumber"
        Me.tbCaseNumber.Size = New System.Drawing.Size(208, 20)
        Me.tbCaseNumber.TabIndex = 0
        Me.tbCaseNumber.Text = ""
        '
        'Searchlbl
        '
        Me.Searchlbl.Location = New System.Drawing.Point(56, 8)
        Me.Searchlbl.Name = "Searchlbl"
        Me.Searchlbl.Size = New System.Drawing.Size(208, 16)
        Me.Searchlbl.TabIndex = 22
        Me.Searchlbl.Text = "Enter Case Number:"
        Me.Searchlbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(124, 312)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(56, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(208, 28)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "PLEASE EXCLUDE SPACES AND HYPHENS"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmBankruptcySearch
        '
        Me.AcceptButton = Me.btnSearch
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(320, 344)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Searchlbl)
        Me.Controls.Add(Me.tbCaseNumber)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Results10)
        Me.Controls.Add(Me.Select10)
        Me.Controls.Add(Me.Results9)
        Me.Controls.Add(Me.Select9)
        Me.Controls.Add(Me.Results8)
        Me.Controls.Add(Me.Select8)
        Me.Controls.Add(Me.Results7)
        Me.Controls.Add(Me.Select7)
        Me.Controls.Add(Me.Results6)
        Me.Controls.Add(Me.Select6)
        Me.Controls.Add(Me.Results5)
        Me.Controls.Add(Me.Select5)
        Me.Controls.Add(Me.Results4)
        Me.Controls.Add(Me.Select4)
        Me.Controls.Add(Me.Results3)
        Me.Controls.Add(Me.Select3)
        Me.Controls.Add(Me.Results2)
        Me.Controls.Add(Me.Select2)
        Me.Controls.Add(Me.Results1)
        Me.Controls.Add(Me.Select1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBankruptcySearch"
        Me.Text = "Bankruptcy Case Search"
        Me.ResumeLayout(False)

    End Sub
End Class
