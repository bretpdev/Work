<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProvideDetail
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProvideDetail))
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.Label9 = New System.Windows.Forms.Label
        Me.tbFromDisplay = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbFromEmail = New System.Windows.Forms.TextBox
        Me.btnRun = New System.Windows.Forms.Button
        Me.btnTest = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnBrowseHF = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.tbHTMLFile = New System.Windows.Forms.TextBox
        Me.btnBrowseDF = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.tbDataFile = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbCommentText = New System.Windows.Forms.TextBox
        Me.tbActionCode = New System.Windows.Forms.TextBox
        Me.tbARC = New System.Windows.Forms.TextBox
        Me.cbOnelink = New System.Windows.Forms.CheckBox
        Me.cbCOMPASS = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbSubject = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(14, 61)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(99, 15)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Email Display Name:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbFromDisplay
        '
        Me.tbFromDisplay.Location = New System.Drawing.Point(131, 59)
        Me.tbFromDisplay.MaxLength = 8000
        Me.tbFromDisplay.Name = "tbFromDisplay"
        Me.tbFromDisplay.Size = New System.Drawing.Size(221, 20)
        Me.tbFromDisplay.TabIndex = 26
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(-1, 38)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(114, 15)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "From: (Email Address)"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbFromEmail
        '
        Me.tbFromEmail.Location = New System.Drawing.Point(131, 33)
        Me.tbFromEmail.MaxLength = 8000
        Me.tbFromEmail.Name = "tbFromEmail"
        Me.tbFromEmail.Size = New System.Drawing.Size(221, 20)
        Me.tbFromEmail.TabIndex = 25
        '
        'btnRun
        '
        Me.btnRun.Location = New System.Drawing.Point(370, 489)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(63, 23)
        Me.btnRun.TabIndex = 36
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(289, 489)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(63, 23)
        Me.btnTest.TabIndex = 35
        Me.btnTest.Text = "Test"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(131, 397)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(413, 74)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = resources.GetString("Label8.Text")
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(131, 265)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(413, 47)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "IMPORTANT: Please ensure that all images referenced in the HTML are located in ""h" & _
            "ttp://www.uheaa.org/images/"".  A ticket may need to be opened to coordinate that" & _
            " with CNOC."
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(131, 342)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(413, 32)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "The data file must have a header row, and must have the following fields in the f" & _
            "ollowing order (Account Number, Borrower Name, Email Address)."
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(451, 489)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 23)
        Me.btnCancel.TabIndex = 37
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(208, 489)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(63, 23)
        Me.btnSave.TabIndex = 34
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnBrowseHF
        '
        Me.btnBrowseHF.Location = New System.Drawing.Point(573, 373)
        Me.btnBrowseHF.Name = "btnBrowseHF"
        Me.btnBrowseHF.Size = New System.Drawing.Size(63, 23)
        Me.btnBrowseHF.TabIndex = 33
        Me.btnBrowseHF.Text = "Browse"
        Me.btnBrowseHF.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(23, 377)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(90, 15)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "HTML File:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbHTMLFile
        '
        Me.tbHTMLFile.Location = New System.Drawing.Point(131, 374)
        Me.tbHTMLFile.MaxLength = 8000
        Me.tbHTMLFile.Name = "tbHTMLFile"
        Me.tbHTMLFile.ReadOnly = True
        Me.tbHTMLFile.Size = New System.Drawing.Size(413, 20)
        Me.tbHTMLFile.TabIndex = 13
        '
        'btnBrowseDF
        '
        Me.btnBrowseDF.Location = New System.Drawing.Point(573, 314)
        Me.btnBrowseDF.Name = "btnBrowseDF"
        Me.btnBrowseDF.Size = New System.Drawing.Size(63, 23)
        Me.btnBrowseDF.TabIndex = 32
        Me.btnBrowseDF.Text = "Browse"
        Me.btnBrowseDF.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(23, 320)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 15)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Data File:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbDataFile
        '
        Me.tbDataFile.Location = New System.Drawing.Point(131, 315)
        Me.tbDataFile.MaxLength = 8000
        Me.tbDataFile.Name = "tbDataFile"
        Me.tbDataFile.ReadOnly = True
        Me.tbDataFile.Size = New System.Drawing.Size(413, 20)
        Me.tbDataFile.TabIndex = 19
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(23, 144)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 15)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Comment Text:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbCommentText
        '
        Me.tbCommentText.Location = New System.Drawing.Point(131, 139)
        Me.tbCommentText.Multiline = True
        Me.tbCommentText.Name = "tbCommentText"
        Me.tbCommentText.Size = New System.Drawing.Size(505, 123)
        Me.tbCommentText.TabIndex = 29
        '
        'tbActionCode
        '
        Me.tbActionCode.Location = New System.Drawing.Point(152, 115)
        Me.tbActionCode.MaxLength = 5
        Me.tbActionCode.Name = "tbActionCode"
        Me.tbActionCode.ReadOnly = True
        Me.tbActionCode.Size = New System.Drawing.Size(119, 20)
        Me.tbActionCode.TabIndex = 31
        Me.tbActionCode.Text = "Action Code"
        '
        'tbARC
        '
        Me.tbARC.Location = New System.Drawing.Point(152, 91)
        Me.tbARC.MaxLength = 5
        Me.tbARC.Name = "tbARC"
        Me.tbARC.ReadOnly = True
        Me.tbARC.Size = New System.Drawing.Size(119, 20)
        Me.tbARC.TabIndex = 30
        Me.tbARC.Text = "ARC"
        '
        'cbOnelink
        '
        Me.cbOnelink.AutoSize = True
        Me.cbOnelink.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbOnelink.Location = New System.Drawing.Point(59, 118)
        Me.cbOnelink.Name = "cbOnelink"
        Me.cbOnelink.Size = New System.Drawing.Size(73, 17)
        Me.cbOnelink.TabIndex = 28
        Me.cbOnelink.Text = "OneLINK:"
        Me.cbOnelink.UseVisualStyleBackColor = True
        '
        'cbCOMPASS
        '
        Me.cbCOMPASS.AutoSize = True
        Me.cbCOMPASS.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbCOMPASS.Location = New System.Drawing.Point(51, 94)
        Me.cbCOMPASS.Name = "cbCOMPASS"
        Me.cbCOMPASS.Size = New System.Drawing.Size(81, 17)
        Me.cbCOMPASS.TabIndex = 27
        Me.cbCOMPASS.Text = "COMPASS:"
        Me.cbCOMPASS.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(23, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 15)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Email Subject Line:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbSubject
        '
        Me.tbSubject.Location = New System.Drawing.Point(131, 7)
        Me.tbSubject.MaxLength = 200
        Me.tbSubject.Name = "tbSubject"
        Me.tbSubject.Size = New System.Drawing.Size(505, 20)
        Me.tbSubject.TabIndex = 24
        '
        'frmProvideDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(656, 529)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.tbFromDisplay)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbFromEmail)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.btnTest)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnBrowseHF)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.tbHTMLFile)
        Me.Controls.Add(Me.btnBrowseDF)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbDataFile)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbCommentText)
        Me.Controls.Add(Me.tbActionCode)
        Me.Controls.Add(Me.tbARC)
        Me.Controls.Add(Me.cbOnelink)
        Me.Controls.Add(Me.cbCOMPASS)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbSubject)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmProvideDetail"
        Me.Text = "Campaign Detail"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents tbFromDisplay As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbFromEmail As System.Windows.Forms.TextBox
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnBrowseHF As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbHTMLFile As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseDF As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbDataFile As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbCommentText As System.Windows.Forms.TextBox
    Friend WithEvents tbActionCode As System.Windows.Forms.TextBox
    Friend WithEvents tbARC As System.Windows.Forms.TextBox
    Friend WithEvents cbOnelink As System.Windows.Forms.CheckBox
    Friend WithEvents cbCOMPASS As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbSubject As System.Windows.Forms.TextBox
End Class
