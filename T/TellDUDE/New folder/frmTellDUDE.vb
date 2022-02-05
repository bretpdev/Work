Imports System.Data.SqlClient
Imports System.Threading


Public Class frmTellDUDE
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblRevised As System.Windows.Forms.Label
    Friend WithEvents lblCode As System.Windows.Forms.Label
    Friend WithEvents btnGoto As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents cboGroup As System.Windows.Forms.ComboBox
    Friend WithEvents txtQuestion As System.Windows.Forms.TextBox
    Friend WithEvents mmTell As System.Windows.Forms.MainMenu
    Friend WithEvents miGroup As System.Windows.Forms.MenuItem
    Friend WithEvents miGroupAdd As System.Windows.Forms.MenuItem
    Friend WithEvents lblQuestionID As System.Windows.Forms.Label
    Friend WithEvents txtGQID As System.Windows.Forms.TextBox
    Friend WithEvents lblSelectGroup As System.Windows.Forms.Label
    Friend WithEvents lblGroupID As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents gbAll As System.Windows.Forms.GroupBox
    Friend WithEvents ColorsDlg As System.Windows.Forms.ColorDialog
    Friend WithEvents ttmain As System.Windows.Forms.ToolTip
    Friend WithEvents txtAnswer As System.Windows.Forms.RichTextBox
    Friend WithEvents cmAnswer As System.Windows.Forms.ContextMenu
    Friend WithEvents cmCopy As System.Windows.Forms.MenuItem
    Friend WithEvents cmCut As System.Windows.Forms.MenuItem
    Friend WithEvents cmPaste As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents cmBullet As System.Windows.Forms.MenuItem
    Friend WithEvents cmColor As System.Windows.Forms.MenuItem
    Friend WithEvents cmFont As System.Windows.Forms.MenuItem
    Friend WithEvents cmIndent As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents cmLeft As System.Windows.Forms.MenuItem
    Friend WithEvents cmCenter As System.Windows.Forms.MenuItem
    Friend WithEvents cmRight As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents cmUndo As System.Windows.Forms.MenuItem
    Friend WithEvents lstQuestion As System.Windows.Forms.ListBox
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents lblList As System.Windows.Forms.Label
    Friend WithEvents pbTop As System.Windows.Forms.PictureBox
    Friend WithEvents pbShark As System.Windows.Forms.PictureBox
    Friend WithEvents lblSelectQuestion As System.Windows.Forms.Label
    Friend WithEvents btnSort As System.Windows.Forms.Button
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents cmBitmap As System.Windows.Forms.MenuItem
    Friend WithEvents fdgFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cmRedo As System.Windows.Forms.MenuItem
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents mnNullReport As System.Windows.Forms.MenuItem
    Friend WithEvents mnGroupReport As System.Windows.Forms.MenuItem
    Friend WithEvents cboGroupDesc As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTellDUDE))
        Me.gbAll = New System.Windows.Forms.GroupBox
        Me.lblGroupID = New System.Windows.Forms.Label
        Me.cboGroupDesc = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnnew = New System.Windows.Forms.Button
        Me.btnSort = New System.Windows.Forms.Button
        Me.pbShark = New System.Windows.Forms.PictureBox
        Me.btnClear = New System.Windows.Forms.Button
        Me.lblList = New System.Windows.Forms.Label
        Me.lstQuestion = New System.Windows.Forms.ListBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtQuestion = New System.Windows.Forms.TextBox
        Me.lblQuestionID = New System.Windows.Forms.Label
        Me.lblSelectGroup = New System.Windows.Forms.Label
        Me.cboGroup = New System.Windows.Forms.ComboBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.lblRevised = New System.Windows.Forms.Label
        Me.lblCode = New System.Windows.Forms.Label
        Me.btnGoto = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblSelectQuestion = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtGQID = New System.Windows.Forms.TextBox
        Me.txtAnswer = New System.Windows.Forms.RichTextBox
        Me.cmAnswer = New System.Windows.Forms.ContextMenu
        Me.cmUndo = New System.Windows.Forms.MenuItem
        Me.cmRedo = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.cmCut = New System.Windows.Forms.MenuItem
        Me.cmCopy = New System.Windows.Forms.MenuItem
        Me.cmPaste = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.cmBullet = New System.Windows.Forms.MenuItem
        Me.cmIndent = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.cmLeft = New System.Windows.Forms.MenuItem
        Me.cmCenter = New System.Windows.Forms.MenuItem
        Me.cmRight = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.cmColor = New System.Windows.Forms.MenuItem
        Me.cmFont = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.cmBitmap = New System.Windows.Forms.MenuItem
        Me.pbTop = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.mmTell = New System.Windows.Forms.MainMenu(Me.components)
        Me.miGroup = New System.Windows.Forms.MenuItem
        Me.miGroupAdd = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.mnNullReport = New System.Windows.Forms.MenuItem
        Me.mnGroupReport = New System.Windows.Forms.MenuItem
        Me.ColorsDlg = New System.Windows.Forms.ColorDialog
        Me.ttmain = New System.Windows.Forms.ToolTip(Me.components)
        Me.fdgFile = New System.Windows.Forms.OpenFileDialog
        Me.gbAll.SuspendLayout()
        CType(Me.pbShark, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbTop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbAll
        '
        Me.gbAll.BackColor = System.Drawing.SystemColors.Control
        Me.gbAll.Controls.Add(Me.lblGroupID)
        Me.gbAll.Controls.Add(Me.cboGroupDesc)
        Me.gbAll.Controls.Add(Me.Label2)
        Me.gbAll.Controls.Add(Me.btnnew)
        Me.gbAll.Controls.Add(Me.btnSort)
        Me.gbAll.Controls.Add(Me.pbShark)
        Me.gbAll.Controls.Add(Me.btnClear)
        Me.gbAll.Controls.Add(Me.lblList)
        Me.gbAll.Controls.Add(Me.lstQuestion)
        Me.gbAll.Controls.Add(Me.Label4)
        Me.gbAll.Controls.Add(Me.txtQuestion)
        Me.gbAll.Controls.Add(Me.lblQuestionID)
        Me.gbAll.Controls.Add(Me.lblSelectGroup)
        Me.gbAll.Controls.Add(Me.cboGroup)
        Me.gbAll.Controls.Add(Me.btnSave)
        Me.gbAll.Controls.Add(Me.btnDelete)
        Me.gbAll.Controls.Add(Me.lblRevised)
        Me.gbAll.Controls.Add(Me.lblCode)
        Me.gbAll.Controls.Add(Me.btnGoto)
        Me.gbAll.Controls.Add(Me.btnExit)
        Me.gbAll.Controls.Add(Me.Label3)
        Me.gbAll.Controls.Add(Me.lblSelectQuestion)
        Me.gbAll.Controls.Add(Me.Label1)
        Me.gbAll.Controls.Add(Me.txtGQID)
        Me.gbAll.Controls.Add(Me.txtAnswer)
        Me.gbAll.Location = New System.Drawing.Point(8, 78)
        Me.gbAll.Name = "gbAll"
        Me.gbAll.Size = New System.Drawing.Size(776, 424)
        Me.gbAll.TabIndex = 4
        Me.gbAll.TabStop = False
        '
        'lblGroupID
        '
        Me.lblGroupID.Location = New System.Drawing.Point(8, 88)
        Me.lblGroupID.Name = "lblGroupID"
        Me.lblGroupID.Size = New System.Drawing.Size(184, 23)
        Me.lblGroupID.TabIndex = 22
        Me.lblGroupID.Visible = False
        '
        'cboGroupDesc
        '
        Me.cboGroupDesc.Location = New System.Drawing.Point(80, 88)
        Me.cboGroupDesc.Name = "cboGroupDesc"
        Me.cboGroupDesc.Size = New System.Drawing.Size(112, 21)
        Me.cboGroupDesc.TabIndex = 30
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(592, 240)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 16)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "Last Updated:"
        '
        'btnnew
        '
        Me.btnnew.Location = New System.Drawing.Point(288, 88)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(40, 23)
        Me.btnnew.TabIndex = 27
        Me.btnnew.Text = "All"
        Me.ttmain.SetToolTip(Me.btnnew, "Add new question to selected group.")
        '
        'btnSort
        '
        Me.btnSort.Location = New System.Drawing.Point(704, 8)
        Me.btnSort.Name = "btnSort"
        Me.btnSort.Size = New System.Drawing.Size(64, 24)
        Me.btnSort.TabIndex = 6
        Me.btnSort.TabStop = False
        Me.btnSort.Text = "Numeric"
        '
        'pbShark
        '
        Me.pbShark.Image = CType(resources.GetObject("pbShark.Image"), System.Drawing.Image)
        Me.pbShark.Location = New System.Drawing.Point(752, 152)
        Me.pbShark.Name = "pbShark"
        Me.pbShark.Size = New System.Drawing.Size(168, 32)
        Me.pbShark.TabIndex = 26
        Me.pbShark.TabStop = False
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(192, 392)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(120, 23)
        Me.btnClear.TabIndex = 7
        Me.btnClear.Text = "Clear"
        '
        'lblList
        '
        Me.lblList.Location = New System.Drawing.Point(336, 16)
        Me.lblList.Name = "lblList"
        Me.lblList.Size = New System.Drawing.Size(224, 16)
        Me.lblList.TabIndex = 25
        Me.lblList.Text = "ID     Question"
        Me.lblList.Visible = False
        '
        'lstQuestion
        '
        Me.lstQuestion.Location = New System.Drawing.Point(336, 32)
        Me.lstQuestion.Name = "lstQuestion"
        Me.lstQuestion.Size = New System.Drawing.Size(432, 121)
        Me.lstQuestion.TabIndex = 24
        Me.lstQuestion.TabStop = False
        Me.lstQuestion.Visible = False
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(208, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 16)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Question ID:"
        '
        'txtQuestion
        '
        Me.txtQuestion.Location = New System.Drawing.Point(8, 184)
        Me.txtQuestion.Multiline = True
        Me.txtQuestion.Name = "txtQuestion"
        Me.txtQuestion.ReadOnly = True
        Me.txtQuestion.Size = New System.Drawing.Size(760, 56)
        Me.txtQuestion.TabIndex = 4
        '
        'lblQuestionID
        '
        Me.lblQuestionID.Location = New System.Drawing.Point(208, 88)
        Me.lblQuestionID.Name = "lblQuestionID"
        Me.lblQuestionID.Size = New System.Drawing.Size(72, 16)
        Me.lblQuestionID.TabIndex = 6
        '
        'lblSelectGroup
        '
        Me.lblSelectGroup.Location = New System.Drawing.Point(8, 72)
        Me.lblSelectGroup.Name = "lblSelectGroup"
        Me.lblSelectGroup.Size = New System.Drawing.Size(192, 16)
        Me.lblSelectGroup.TabIndex = 19
        Me.lblSelectGroup.Text = "Select Group to add Question to:"
        '
        'cboGroup
        '
        Me.cboGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGroup.Location = New System.Drawing.Point(8, 88)
        Me.cboGroup.Name = "cboGroup"
        Me.cboGroup.Size = New System.Drawing.Size(72, 21)
        Me.cboGroup.TabIndex = 3
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(328, 392)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(120, 24)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "&Save"
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Location = New System.Drawing.Point(464, 392)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 24)
        Me.btnDelete.TabIndex = 9
        Me.btnDelete.Text = "&Delete"
        '
        'lblRevised
        '
        Me.lblRevised.Location = New System.Drawing.Point(680, 240)
        Me.lblRevised.Name = "lblRevised"
        Me.lblRevised.Size = New System.Drawing.Size(88, 16)
        Me.lblRevised.TabIndex = 15
        Me.lblRevised.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblCode
        '
        Me.lblCode.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCode.Location = New System.Drawing.Point(712, 36)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(56, 16)
        Me.lblCode.TabIndex = 14
        Me.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnGoto
        '
        Me.btnGoto.Location = New System.Drawing.Point(192, 32)
        Me.btnGoto.Name = "btnGoto"
        Me.btnGoto.Size = New System.Drawing.Size(136, 24)
        Me.btnGoto.TabIndex = 2
        Me.btnGoto.Text = "&Go to"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(600, 392)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(120, 24)
        Me.btnExit.TabIndex = 10
        Me.btnExit.Text = "&Exit"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 240)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(112, 16)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Answer"
        '
        'lblSelectQuestion
        '
        Me.lblSelectQuestion.Location = New System.Drawing.Point(8, 168)
        Me.lblSelectQuestion.Name = "lblSelectQuestion"
        Me.lblSelectQuestion.Size = New System.Drawing.Size(112, 16)
        Me.lblSelectQuestion.TabIndex = 8
        Me.lblSelectQuestion.Text = "Question"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Go to Question"
        '
        'txtGQID
        '
        Me.txtGQID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGQID.Location = New System.Drawing.Point(8, 32)
        Me.txtGQID.Name = "txtGQID"
        Me.txtGQID.Size = New System.Drawing.Size(168, 20)
        Me.txtGQID.TabIndex = 1
        '
        'txtAnswer
        '
        Me.txtAnswer.Location = New System.Drawing.Point(8, 256)
        Me.txtAnswer.Name = "txtAnswer"
        Me.txtAnswer.ReadOnly = True
        Me.txtAnswer.Size = New System.Drawing.Size(760, 128)
        Me.txtAnswer.TabIndex = 5
        Me.txtAnswer.TabStop = False
        Me.txtAnswer.Text = ""
        '
        'cmAnswer
        '
        Me.cmAnswer.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cmUndo, Me.cmRedo, Me.MenuItem4, Me.cmCut, Me.cmCopy, Me.cmPaste, Me.MenuItem1, Me.cmBullet, Me.cmIndent, Me.MenuItem2, Me.MenuItem7, Me.cmColor, Me.cmFont, Me.MenuItem5, Me.cmBitmap})
        '
        'cmUndo
        '
        Me.cmUndo.Index = 0
        Me.cmUndo.Text = "Undo"
        '
        'cmRedo
        '
        Me.cmRedo.Index = 1
        Me.cmRedo.Text = "Redo"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 2
        Me.MenuItem4.Text = "-"
        '
        'cmCut
        '
        Me.cmCut.Index = 3
        Me.cmCut.Text = "Cut"
        '
        'cmCopy
        '
        Me.cmCopy.Index = 4
        Me.cmCopy.Text = "Copy"
        '
        'cmPaste
        '
        Me.cmPaste.Index = 5
        Me.cmPaste.Text = "Paste"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 6
        Me.MenuItem1.Text = "-"
        '
        'cmBullet
        '
        Me.cmBullet.Index = 7
        Me.cmBullet.Text = "Bulleted"
        '
        'cmIndent
        '
        Me.cmIndent.Index = 8
        Me.cmIndent.Text = "Indent"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 9
        Me.MenuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cmLeft, Me.cmCenter, Me.cmRight})
        Me.MenuItem2.Text = "Alignment"
        '
        'cmLeft
        '
        Me.cmLeft.Index = 0
        Me.cmLeft.Text = "Left"
        '
        'cmCenter
        '
        Me.cmCenter.Index = 1
        Me.cmCenter.Text = "Center"
        '
        'cmRight
        '
        Me.cmRight.Index = 2
        Me.cmRight.Text = "Right"
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 10
        Me.MenuItem7.Text = "-"
        '
        'cmColor
        '
        Me.cmColor.Index = 11
        Me.cmColor.Text = "Font Color"
        '
        'cmFont
        '
        Me.cmFont.Index = 12
        Me.cmFont.Text = "Font"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 13
        Me.MenuItem5.Text = "-"
        '
        'cmBitmap
        '
        Me.cmBitmap.Index = 14
        Me.cmBitmap.Text = "Insert Bitmap"
        '
        'pbTop
        '
        Me.pbTop.BackgroundImage = CType(resources.GetObject("pbTop.BackgroundImage"), System.Drawing.Image)
        Me.pbTop.Location = New System.Drawing.Point(8, 6)
        Me.pbTop.Name = "pbTop"
        Me.pbTop.Size = New System.Drawing.Size(776, 64)
        Me.pbTop.TabIndex = 3
        Me.pbTop.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.BackgroundImage = CType(resources.GetObject("PictureBox2.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(8, 510)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(776, 64)
        Me.PictureBox2.TabIndex = 5
        Me.PictureBox2.TabStop = False
        '
        'mmTell
        '
        Me.mmTell.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.miGroup, Me.MenuItem6})
        '
        'miGroup
        '
        Me.miGroup.Index = 0
        Me.miGroup.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.miGroupAdd})
        Me.miGroup.Text = "G&roup"
        '
        'miGroupAdd
        '
        Me.miGroupAdd.Index = 0
        Me.miGroupAdd.Text = "Add Group"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 1
        Me.MenuItem6.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnNullReport, Me.mnGroupReport})
        Me.MenuItem6.Text = "Reports"
        '
        'mnNullReport
        '
        Me.mnNullReport.Index = 0
        Me.mnNullReport.Text = "Print Null Report"
        '
        'mnGroupReport
        '
        Me.mnGroupReport.Index = 1
        Me.mnGroupReport.Text = "Print Group Report"
        '
        'frmTellDUDE
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(792, 581)
        Me.Controls.Add(Me.gbAll)
        Me.Controls.Add(Me.pbTop)
        Me.Controls.Add(Me.PictureBox2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.mmTell
        Me.Name = "frmTellDUDE"
        Me.Text = "Tell DUDE"
        Me.gbAll.ResumeLayout(False)
        Me.gbAll.PerformLayout()
        CType(Me.pbShark, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbTop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public Const VK_TAB = &H9
    Public p As New System.Diagnostics.Process
    Private SharkRun As Boolean 'true if shark is moving
    Private ManRun As Boolean   'true if man is running in picture box
    Private Indent As Integer   'This is the number of pixels to indent on the answer text box
    Private Editmode As Boolean  'if true then use UPDATE else use INSERT for SQL statement


    Private Sub btnGoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGoto.Click
        'Load the record typed in the txtGQID text box
        runGoTo()
    End Sub
    Sub runGoTo()
        'Runs an SQL statement to get the question and loads it into the text boxes
        Dim QID As String
        Dim GID As String
        Dim Quest As String
        Dim Ans As String
        Dim Rev As String

        GID = Mid(txtGQID.Text, 1, 4)
        QID = Mid(txtGQID.Text, 5, 10)

        'This is the SQL command that will be run to search for the record
        Dim cmd As New SqlCommand("SELECT Question, Revised FROM QuestionTB Where GroupID = '" & GID & "' And QuestionID = '" & QID & "'", Conn)
        'Validate the text
        If txtGQID.Text.Length < 5 Or IsNumeric(Mid(txtGQID.Text, 5, 10)) = False Then
            MsgBox("You must have a 4 character Group ID followed by a number.")
            Exit Sub
        End If
        cboGroup.Text = GID
        lblQuestionID.Text = QID
        Try
            setGoto()
            Dim Reader As SqlDataReader
            Conn.Open()
            'return records matching search
            Reader = cmd.ExecuteReader
            While Reader.Read()

                txtQuestion.Text = Reader.GetValue(0).ToString
                lblRevised.Text = Reader.GetValue(1).ToString
            End While
            'close connection to database
            Reader.Close()

            Conn.Close()
            'If return data is blank then no record was found
            If txtQuestion.Text & lblRevised.Text = "" Then
                MsgBox("Record not found!")
                Exit Sub
            End If
            SqlBlob2File("T:\Testdoc2.rtf", GID, QID)
            Editmode = True
        Catch ex As Exception
            MsgBox(ex.Message)
            'MsgBox("There was an error loading the record! Contact System Support.")
            Conn.Close()
        End Try
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        txtAnswer.SaveFile("T:\TellDUDEdoc.rtf", RichTextBoxStreamType.RichText)
        SaveMethod()
    End Sub
    Private Sub File2SqlBlob(ByVal SourceFilePath As String, ByVal group As String, ByVal qid As String)
        'convert file to blob
        Dim cmd As SqlCommand
        Dim fs As New System.IO.FileStream(SourceFilePath, IO.FileMode.Open, IO.FileAccess.Read)
        cmd = New SqlCommand("Update QuestionTB set myim =@Picture , Revised = '" & Today & "' Where GroupID = '" & group & "' And QuestionID = " & qid, Conn)
        Dim b(fs.Length() - 1) As Byte
        fs.Read(b, 0, b.Length)
        fs.Close()
        'add blob to sql query as a parameter
        Dim P As New SqlParameter("@Picture", SqlDbType.Image, b.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, b)
        cmd.Parameters.Add(P)
        Conn.Open()
        cmd.ExecuteNonQuery()
        Conn.Close()

    End Sub
    Private Sub SqlBlob2File(ByVal DestFilePath As String, ByVal group As String, ByVal qid As String)
        'convert blob to file
        Dim PictureCol As Integer = 0 ' the column # of the BLOB field
        Dim cmd As New SqlCommand("SELECT myim FROM QuestionTB Where GroupID = '" & group & "' And QuestionID = " & qid, Conn)
        Dim Reader As SqlDataReader
        Conn.Open()
        Reader = cmd.ExecuteReader
        Reader.Read()
        Dim b(Reader.GetBytes(PictureCol, 0, Nothing, 0, Integer.MaxValue) - 1) As Byte
        Reader.GetBytes(PictureCol, 0, b, 0, b.Length)
        Reader.Close()
        Conn.Close()
        Dim fs As New System.IO.FileStream(DestFilePath, IO.FileMode.Create, IO.FileAccess.Write)
        fs.Write(b, 0, b.Length)
        fs.Close()
        'save blob to hard drive (this file will be loaded to the txtanswer box later)
        txtAnswer.LoadFile(DestFilePath, _
                  RichTextBoxStreamType.RichText)
    End Sub

    Sub SaveMethod()
        Dim SQLStr As String
        Dim AnswerStr As String
        'Dim tShark As New Thread(AddressOf moveShark)
        Dim dupQuestion As String
        Dim qus As String
        Dim fs As New System.IO.FileStream("T:\TellDUDEdoc.rtf", IO.FileMode.Open, IO.FileAccess.Read)
        If txtAnswer.Text.Replace(" ", "").Length = 0 Then
            AnswerStr = ""
        Else
            AnswerStr = txtAnswer.Text
        End If
        qus = txtQuestion.Text.Replace("'", "''")
        'format string for commas
        AnswerStr = Replace(AnswerStr, "'", "''")
        SQLStr = "INSERT INTO QuestionTB (QuestionID, GroupID, Question, Answer, Revised)VALUES('" & lblQuestionID.Text & "','" & cboGroup.Text & "','" & qus & "','" & AnswerStr & "','" & Today & "')"
        'save richtext file rich text file
        If Len(txtAnswer.Text) > 3994 Then
            MsgBox("Whoa! Thats a bit too wordy for me. I'm a simple Surf Bum. Can you tone it down to 3994 characters or less.", MsgBoxStyle.Critical, "TellDUDE")
            fs.Close()
            Exit Sub
        End If
        Dim cmd As New SqlCommand(SQLStr, Conn)
        Dim b(fs.Length() - 1) As Byte
        'Try
        'Editmode = true when a new record has not been created.
        'This is needed to know if you will need to run a Update SQL statment or Insert Statement
        If Editmode Then
            cmd = New SqlCommand("Update QuestionTB set myim =@Picture, Question = '" & qus & "', Answer = '" & AnswerStr & "', Revised = '" & Today & "' Where GroupID = '" & cboGroup.Text & "' And QuestionID = " & lblQuestionID.Text, Conn)
        Else    'if add mode do the following.
            'check for duplicate
            dupQuestion = validateQuestion(qus)
            If dupQuestion <> "" Then
                MsgBox("This Question is in use by QuestionID " & dupQuestion)
                fs.Close()
                Exit Sub
            End If
            'Validate text boxes
            If txtQuestion.Text.Replace(" ", "") = "" Then
                MsgBox("The Question field must not be blank.")
                fs.Close()
                Exit Sub
            End If
        End If
        fs.Read(b, 0, b.Length)
        fs.Close()
        Dim P As New SqlParameter("@Picture", SqlDbType.Image, b.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, b)
        cmd.Parameters.Add(P)
        cmd.Connection.Open()
        cmd.ExecuteNonQuery()
        cmd.Connection.Close()
        File2SqlBlob("T:\TellDUDEdoc.rtf", cboGroup.Text, lblQuestionID.Text)
        setSave()
        'If SharkRun = False Then
        '    tShark.Start()
        'End If
        MsgBox("Record has been saved.")
        'Catch e As Exception
        'MsgBox("There was an error Saving the record! Contact System Support.")
        'MsgBox(e.Message)
        fs.Close()
        cmd.Connection.Close()
        'End Try
    End Sub
    Function validateQuestion(ByVal q As String) As String
        Dim Question As String
        Dim QuestionID As String
        Dim GroupID As String
        Dim quest As String
        quest = q.Replace("'", "''")
        Dim cmd As New SqlCommand("SELECT QuestionID, GroupID FROM QuestionTB Where Question = '" & q & "'", Conn)
        'MsgBox("SELECT QuestionID, GroupID FROM QuestionTB Where Question = '" & q & "'")
        Dim Reader As SqlDataReader
        Try
            Conn.Open()
            'return records matching search
            Reader = cmd.ExecuteReader
            While Reader.Read
                QuestionID = Reader.GetValue(0).ToString()
                GroupID = Reader.GetValue(1).ToString()
            End While
            Reader.Close()
            Conn.Close()
            'If return data is blank then no record was found
            Return GroupID & QuestionID
        Catch ex As Exception

            Conn.Close()
            MsgBox(ex.Message)
            'MsgBox("There was an error Checking the Question! Contact System Support.")
            cmd.Connection.Close()
        End Try
    End Function
    Private Sub miGroupAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miGroupAdd.Click
        'This adds a new Group to the database.
        Dim group As String
        Dim groupDesc As String
        Dim GroupDlst As New ArrayList
        group = ""
        Dim cmd As SqlCommand
        Try
            'make sure the group field is 4 characters
            Do While group.Length <> 4
                group = UCase(InputBox("Enter a new group.", "Add Group", ""))
                groupDesc = UCase(InputBox("Enter a description for group """ & group & """.", "Add Group Description", ""))
                If Trim(group.Length) <> 4 Then
                    If MsgBox("This field must be 4 characters.", MsgBoxStyle.OkCancel, "Tell DUDE") = MsgBoxResult.Cancel Then
                        Exit Sub
                    End If
                End If
            Loop
            'SQL statement
            cmd = New SqlCommand("INSERT INTO GroupTB (GroupID,Description)VALUES('" & group & "','" & groupDesc & "')", Conn)
            'execute SQL statement
            cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        Catch
            MsgBox("There was an error Adding the Group! Contact System Support.")
            Exit Sub
        End Try
        'clear combobox
        cboGroup.Items.Clear()
        cboGroupDesc.Items.Clear()
        'Once the group has been added update combo box
        Dim ds As New DataSet
        Dim DA As New SqlDataAdapter
        Dim Grouplst As New ArrayList
        Try
            'return all Groups
            cmd = New SqlCommand("SELECT GroupID, Description FROM GroupTB", Conn)
            Dim Reader As SqlDataReader
            Conn.Open()
            'return records matching search
            Reader = cmd.ExecuteReader
            'get list of group IDs
            While Reader.Read
                Grouplst.Add(Reader.GetString(0))
                GroupDlst.Add(Reader.GetString(1))
            End While
            Reader.Close()
            Conn.Close()
            'populate combo box
            cboGroup.Items.AddRange(Grouplst.ToArray)
            cboGroupDesc.Items.AddRange(GroupDlst.ToArray)
        Catch ex As Exception
            MsgBox(ex.Message)
            'MsgBox("There was an error loading the GroupID box! Contact System Support.")
        End Try
    End Sub


    Private Sub frmTellDUDE_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ds As New DataSet
        Dim DA As New SqlDataAdapter
        Dim Grouplst As New ArrayList
        Dim GroupDlst As New ArrayList
        General.initialize()
        If General.TestMode Then Me.Text = Me.Text & " [TEST]"

        Dim cmd As New SqlCommand("SELECT GroupID, Description FROM GroupTB", Conn)
        Indent = 20 'set # of pixels to endent on the txtAnswer text box
        txtAnswer.BulletIndent = 20 'set # of pixels to endent bullets on the txtAnswer text box
        txtAnswer.WordWrap = True
        txtAnswer.AcceptsTab = True
        txtAnswer.AutoWordSelection = True
        Try
            Dim Reader As SqlDataReader
            Conn.Open()
            'return records matching search
            Reader = cmd.ExecuteReader
            'get list of group IDs
            While Reader.Read
                Grouplst.Add(Reader.GetString(0))
                GroupDlst.Add(Reader.GetString(1))
            End While
            Reader.Close()
            Conn.Close()
            'populate combo box
            cboGroup.Items.AddRange(Grouplst.ToArray)
            cboGroupDesc.Items.AddRange(GroupDlst.ToArray)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox("There was an error loading the GroupID box! Contact System Support.")
        End Try
        'add Key pressed handlers
        AddHandler txtGQID.KeyPress, AddressOf keypressed
        AddHandler txtQuestion.KeyPress, AddressOf QuestionKeyPressed
    End Sub
    Sub RunMan()
        'this animates the running man at the top of the window when the clear button is pressed
        Dim G As Graphics
        Dim bounce As Integer
        Dim b As Integer
        Dim x As Integer
        Dim y As Integer
        ManRun = True
        y = 35
        b = 0
        G = pbTop.CreateGraphics
        For x = 1 To pbTop.Width - 1 Step 6
            b = b + 1
            If b < 5 Then
                bounce = bounce + 1
            Else
                bounce = bounce - 1
            End If
            If b >= 10 Then
                b = 0
                bounce = 0
            End If
            y = 35 + bounce
            Thread.Sleep(10)
            DrawMan1(G, x, y, b, CInt(x Mod 4))
        Next
        pbTop.Refresh()
        ManRun = False
    End Sub
    Sub DrawMan1(ByVal g As Graphics, ByVal x As Integer, ByVal y As Integer, ByVal b As Integer, ByVal m As Integer)
        'this draws the running man
        If m = 1 Then
            pbTop.Refresh()
            'head and body

            g.FillEllipse(Brushes.Black, x - 4, y, 7, 7)
            g.DrawLine(New Pen(Color.Black, 2), x, y + 4, x - 4, y + 15)
            'arms
            g.DrawLine(New Pen(Color.Black, 2), x - 1, y + 6, x + 6, y + 10)
            g.DrawLine(New Pen(Color.Black, 2), x - 1, y + 6, x - 8, y + 10)
            'legs
            g.DrawLine(New Pen(Color.Black, 2), x - 4, y + 15, x - 8, y + 17)
            g.DrawLine(New Pen(Color.Black, 2), x - 4, y + 15, x + 4, y + 15)
            g.DrawLine(New Pen(Color.Black, 2), x - 8, y + 17, x - 10, y + 11)
            g.DrawLine(New Pen(Color.Black, 2), x + 4, y + 15, x + 2, y + 19)
            Thread.Sleep(15)
        ElseIf m = 2 Or m = 3 Then
            pbTop.Refresh()
            'head and body
            g.FillEllipse(Brushes.Black, x - 4, y, 7, 7)
            g.DrawLine(New Pen(Color.Black, 2), x, y + 4, x - 4, y + 15)
            'arms
            g.DrawLine(New Pen(Color.Black, 2), x - 1, y + 6, x + 6, y + 8)
            g.DrawLine(New Pen(Color.Black, 2), x - 1, y + 6, x - 10, y + 8)
            'legs
            g.DrawLine(New Pen(Color.Black, 2), x - 4, y + 15, x - 12, y + 15)
            g.DrawLine(New Pen(Color.Black, 2), x - 4, y + 15, x + 4, y + 15)
            Thread.Sleep(15)
        End If

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'exit Tell DUDE
        End
    End Sub

    Private Sub cboGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboGroup.SelectedIndexChanged
        'This adds one to the current highest number in a Group and creates a new quesiton using that ID.
        cboGroupDesc.SelectedIndex = cboGroup.SelectedIndex
        Dim QID As String
        Dim cmd As New SqlCommand("SELECT Max(QuestionID) FROM QuestionTB Where GroupID = '" & cboGroup.Text & "'", Conn)
        Try
            Editmode = False
            Dim Reader As SqlDataReader
            Conn.Open()
            'return records matching search
            Reader = cmd.ExecuteReader
            'get list of group IDs
            While Reader.Read
                QID = Reader.GetValue(0).ToString()
            End While
            Reader.Close()
            Conn.Close()
            'populate label
            If QID = Nothing Then
                QID = 0
            End If
            btnSort.Text = "Numeric"
            UpdateQuestionList("QuestionID")
            lblQuestionID.Text = QID + 1 'add one to maximum record
            'clear boxes.
            setAddRec()
        Catch ex As Exception
            MsgBox(ex.Message)

            'MsgBox("There was an error gathering the QuestionID! Contact System Support.")
        End Try
    End Sub

    Sub UpdateQuestionList(ByVal sort As String)
        Try
            'declare sql objects
            'Dim cmd As New SqlCommand("SELECT QuestionID, GroupID, Question, Answer, Revised FROM QuestionTB Where GroupID = '" & cboGroup.Text & "'", TellDUDEcon)
            Dim cmd As New SqlCommand("SELECT QuestionID, Question, Answer FROM QuestionTB Where GroupID = '" & cboGroup.Text & "' order by " & sort, Conn)
            Dim Reader As SqlDataReader
            Dim Questions As New ArrayList
            Dim QuestionID As New ArrayList
            Dim x As Integer
            Dim Quest As String
            Dim QID As String
            Dim Answ As String
            'open connection to database
            Conn.Open()
            'return records matching search
            Reader = cmd.ExecuteReader
            lstQuestion.Items.Clear()
            'transfer results to arrays
            While Reader.Read
                QID = Reader.GetValue(0).ToString
                Quest = Reader.GetValue(1).ToString
                Answ = Reader.GetValue(2).ToString
                If Answ.Replace(" ", "").Length = 0 Then
                    Answ = "*"
                Else
                    Answ = " "
                End If
                For x = 1 To 4 - QID.Length
                    QID = QID & " "
                Next
                Questions.Add(QID & "  " & Answ & "   " & Quest)
            End While

            'add data to arrays if no matches were found for the search
            If Questions.Count = 0 Then
                Questions.Add("Bummer DUDE!  Like, no questions or answers were found for the search terms you entered.")
                Questions.Add("Enter different search terms and surf on, DUDE!")
            End If
            'close connection to database
            Reader.Close()
            Conn.Close()
            'display questions
            lstQuestion.Items.AddRange(Questions.ToArray)
        Catch
            MsgBox("There was an error updating the question! Contact System Support.")
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        'Delete the record listed in the cboGroup  and lblQuestionID labels.
        Dim cmd As New SqlCommand("Delete from QuestionTB Where GroupID = '" & cboGroup.Text & "' And QuestionID = '" & lblQuestionID.Text & "'", Conn)
        Try
            If MsgBox("Do you realy want to delete this record?", MsgBoxStyle.YesNo, "Delete") = MsgBoxResult.No Then
                Exit Sub
            End If
            cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            setDelete()
            MsgBox("Record Deleted.")
        Catch
            MsgBox("There was an error deleting the question! Contact System Support.")
        End Try
    End Sub
    Sub setDelete()
        'sets buttons to proper configuration for the action
        lblSelectGroup.Visible = True
        btnDelete.Enabled = False
        btnSave.Enabled = False
        txtAnswer.Text = ""
        txtQuestion.Text = ""
        txtAnswer.BackColor = Me.BackColor
        txtQuestion.BackColor = Me.BackColor
        txtAnswer.ReadOnly = True
        txtQuestion.ReadOnly = True
        DisableCMAnswer()
        lblRevised.Text = ""
        lblQuestionID.Text = ""
        lstQuestion.Visible = False
        lblList.Visible = False
    End Sub
    Sub setGoto()
        'sets buttons to proper configuration for the action
        lblSelectGroup.Visible = False
        btnSave.Enabled = True
        btnDelete.Enabled = True
        txtAnswer.ReadOnly = False
        txtQuestion.ReadOnly = False
        EnableCMAnswer()
        txtAnswer.BackColor = Color.White
        txtQuestion.BackColor = Color.White
        txtAnswer.ForeColor = Color.Black
        txtQuestion.ForeColor = Color.Black
        txtQuestion.Focus()
        lstQuestion.Visible = False
        lblList.Visible = False
    End Sub
    Sub setListSelect()
        'sets buttons to proper configuration for the action
        lblSelectGroup.Visible = True
        btnSave.Enabled = True
        btnDelete.Enabled = True
        txtAnswer.ReadOnly = False
        txtQuestion.ReadOnly = False
        EnableCMAnswer()
        txtAnswer.BackColor = Color.White
        txtQuestion.BackColor = Color.White
        txtAnswer.ForeColor = Color.Black
        txtQuestion.ForeColor = Color.Black
        lstQuestion.Visible = True
        lblList.Visible = True
    End Sub
    Sub setSave()
        'sets buttons to proper configuration for the action
        txtQuestion.Text = ""
        txtAnswer.Text = ""
        lblQuestionID.Text = ""
        txtAnswer.BackColor = Me.BackColor
        txtQuestion.BackColor = Me.BackColor
        txtAnswer.ReadOnly = True
        txtQuestion.ReadOnly = True
        DisableCMAnswer()
        lblRevised.Text = ""
        lblSelectGroup.Visible = True
        btnSave.Enabled = False
        btnDelete.Enabled = False
        lstQuestion.Visible = False
        lblList.Visible = False
    End Sub
    Sub setAddRec()
        'sets buttons to proper configuration for the action
        txtAnswer.Text = ""
        txtQuestion.Text = ""
        lblRevised.Text = ""
        txtAnswer.ReadOnly = False
        txtQuestion.ReadOnly = False
        EnableCMAnswer()
        txtAnswer.BackColor = Color.White
        txtQuestion.BackColor = Color.White
        txtAnswer.ForeColor = Color.Black
        txtQuestion.ForeColor = Color.Black
        btnDelete.Enabled = False
        btnSave.Enabled = True
        lstQuestion.Visible = True
        lblList.Visible = True
    End Sub

    Sub moveShark()
        'animates the shark when file is saved
        Dim x As Integer
        SharkRun = True
        lstQuestion.BringToFront()
        lblSelectQuestion.BringToFront()
        pbShark.Visible = True
        For x = Me.Width To 1 - pbShark.Width Step -3
            pbShark.Left = x
            Thread.Sleep(5)
        Next
        pbShark.Visible = False
        SharkRun = False
    End Sub

    Sub DisableCMAnswer()
        'remove context menu to txtanswer
        txtAnswer.ContextMenu = Nothing
        txtQuestion.ContextMenu = Nothing
    End Sub
    Sub EnableCMAnswer()
        'add context menu to txtanswer
        txtAnswer.ContextMenu = cmAnswer
        txtQuestion.ContextMenu = cmAnswer
    End Sub
    'this is linked to the txtGQID text box
    Sub keypressed(ByVal o As [Object], ByVal e As KeyPressEventArgs)
        ' If the ENTER key is pressed, the Handled property is set to true, 
        ' to indicate the event is handled.
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Enter) Then
            e.Handled = True
            runGoTo()   'push the GoTo button
        End If
    End Sub 'keypressed
    Sub QuestionKeyPressed(ByVal o As [Object], ByVal e As KeyPressEventArgs)
        ' If the ENTER key is pressed, the Handled property is set to true, 
        ' to indicate the event is handled.
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Enter) Then
            e.Handled = True
            txtAnswer.Focus()
        End If
    End Sub 'keypressed

    Private Sub cmCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmCopy.Click
        txtAnswer.Copy()
    End Sub

    Private Sub cmCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmCut.Click
        txtAnswer.Cut()
    End Sub

    Private Sub cmPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmPaste.Click
        txtAnswer.Paste()
    End Sub

    Private Sub cmBullet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmBullet.Click
        'set bullet
        If txtAnswer.SelectionBullet Then
            txtAnswer.SelectionBullet = False
        Else
            txtAnswer.SelectionBullet = True
        End If
    End Sub

    Private Sub cmColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmColor.Click
        'set color for selected text
        Dim colordlg As New ColorDialog
        Dim col As Color
        If colordlg.ShowDialog() = DialogResult.OK Then
            col = colordlg.Color
            txtAnswer.SelectionColor = col
        End If
    End Sub

    Private Sub cmFont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmFont.Click
        'set font for selected text
        Dim mFontDialog As New FontDialog
        If mFontDialog.ShowDialog() = DialogResult.OK Then
            Dim newFont As Font = mFontDialog.Font
            txtAnswer.SelectionFont = newFont
        End If
    End Sub

    Private Sub cmIndent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmIndent.Click
        'indent current line
        If txtAnswer.SelectionIndent = Indent Then
            txtAnswer.SelectionIndent = 0
        Else
            txtAnswer.SelectionIndent = Indent
        End If
    End Sub

    Private Sub cmLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmLeft.Click
        txtAnswer.SelectionAlignment = HorizontalAlignment.Left
    End Sub

    Private Sub cmCenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmCenter.Click
        txtAnswer.SelectionAlignment = HorizontalAlignment.Center
    End Sub

    Private Sub cmRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmRight.Click
        txtAnswer.SelectionAlignment = HorizontalAlignment.Right
    End Sub

    Private Sub cmUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmUndo.Click
        txtAnswer.Undo()
    End Sub

    Private Sub lstQuestion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstQuestion.SelectedIndexChanged
        Dim ID As String
        Dim PictureCol As Integer = 0 ' the column # of the BLOB field
        Dim sqlstr As String
        ID = lstQuestion.SelectedItem
        ID = Mid(ID, 1, InStr(ID, " ") - 1)
        setListSelect()
        Try
            'declare sql objects
            sqlstr = "SELECT Question, Revised FROM QuestionTB Where GroupID = '" & cboGroup.Text & "' And QuestionID = '" & ID & "'"
            Dim cmd As New SqlCommand("SELECT Question, Revised FROM QuestionTB Where GroupID = '" & cboGroup.Text & "' And QuestionID = '" & ID & "'", Conn)
            Dim Reader As SqlDataReader


            'open connection to database
            Conn.Open()
            Editmode = True
            'return records matching search
            Reader = cmd.ExecuteReader
            txtQuestion.Text = ""
            txtAnswer.Text = ""
            lblRevised.Text = ""
            lblQuestionID.Text = ID
            'transfer results to arrays
            While Reader.Read()
                txtQuestion.Text = Reader.GetValue(0).ToString
                lblRevised.Text = Reader.GetValue(1).ToString
            End While
            'close connection to database
            Reader.Close()
            Conn.Close()

            SqlBlob2File("T:\TellDUDEdoc.rtf", cboGroup.Text, ID)
        Catch ex As Exception
            'MsgBox(ex.Message)
            'MsgBox(sqlstr)
            MsgBox("There was an error loading the question! Contact System Support.")
        End Try

    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Dim t As New Thread(AddressOf RunMan)

        cboGroup.SelectedIndex = -1
        lstQuestion.Items.Clear()
        setSave()


        If ManRun = False Then
            t.Start()
        End If

    End Sub

    Private Sub txtAnswer_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles txtAnswer.LinkClicked
        ' Call Process.Start method to open a browser
        ' with link text as URL.
        p = System.Diagnostics.Process.Start("IExplore.exe", e.LinkText)
    End Sub


    Private Sub btnSort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSort.Click
        'testmethod()
        If btnSort.Text = "Numeric" Then
            btnSort.Text = "Nulls"
            UpdateQuestionList("Answer")
        ElseIf btnSort.Text = "Nulls" Then
            btnSort.Text = "Alfabetic"
            UpdateQuestionList("Question")
        Else
            btnSort.Text = "Numeric"
            UpdateQuestionList("QuestionID")

        End If
    End Sub

    Private Sub cmBitmap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmBitmap.Click
        Dim MyBitmap As Bitmap
        If fdgFile.ShowDialog = DialogResult.OK Then
            MyBitmap = Bitmap.FromFile(fdgFile.FileName)
        End If
        ' Copy the bitmap to the clipboard.
        Clipboard.SetDataObject(MyBitmap)
        ' Get the format for the object type.
        Dim BMPFormat As DataFormats.Format = DataFormats.GetFormat(DataFormats.Bitmap)
        ' After verifying that the data can be pasted, paste it.
        If txtAnswer.CanPaste(BMPFormat) Then
            txtAnswer.Paste(BMPFormat)
            'PasteMyBitmap = True
        Else
            MessageBox.Show("The data format that you attempted to paste is not supported by this control.")
            'PasteMyBitmap = False
        End If
    End Sub


    Private Sub cmRedo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmRedo.Click
        If txtAnswer.CanRedo = True Then
            ' Perform the redo.
            txtAnswer.Redo()
        End If
    End Sub

    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        'This adds one to the current highest number in a Group and creates a new quesiton using that ID.
        Dim QID As String
        Dim cmd As New SqlCommand("SELECT Max(QuestionID) FROM QuestionTB Where GroupID = '" & cboGroup.Text & "'", Conn)
        Try
            If cboGroup.SelectedIndex = -1 Then
                Exit Sub
            End If
            Editmode = False
            Dim Reader As SqlDataReader
            Conn.Open()
            'return records matching search
            Reader = cmd.ExecuteReader
            'get list of group IDs
            While Reader.Read
                QID = Reader.GetValue(0).ToString()
            End While
            Reader.Close()
            Conn.Close()
            'populate label
            If QID = Nothing Then
                QID = 0
            End If
            btnSort.Text = "Numeric"
            UpdateQuestionList("QuestionID")
            lblQuestionID.Text = QID + 1 'add one to maximum record
            'clear boxes.
            setAddRec()
        Catch
            MsgBox("There was an error gathering the QuestionID! Contact System Support.")
        End Try
    End Sub



    Private Sub mnNullReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnNullReport.Click
        'this prints the Null Answer report for TellDUDE
        Dim nr As New NullReportfrm
        nr.Show()
    End Sub

    Private Sub mnGroupReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnGroupReport.Click
        Dim GroupReport As frmGroupReport = New frmGroupReport(Me.BackColor, Me.ForeColor)
        GroupReport.Show()
    End Sub


    Private Sub cboGroupDesc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboGroupDesc.SelectedIndexChanged
        cboGroup.SelectedIndex = cboGroupDesc.SelectedIndex
    End Sub
End Class
