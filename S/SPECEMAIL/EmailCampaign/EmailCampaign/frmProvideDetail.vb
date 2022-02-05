Imports System.Data.SqlClient
Public Class frmProvideDetail


    Private Camp As Campaign
    Private CampNumber As Integer
    Private NewCamp As Boolean
    Dim isValid As Boolean

    Public Sub New(ByVal tCamp As Campaign)
        InitializeComponent()
        Camp = tCamp
        If Not (tCamp Is Nothing) Then 'Pull record from table
            'load textboxes and controls
            CampNumber = Camp.Data.Item("CampID")

            tbSubject.Text = Camp.Data.Item("EmailSubjectLine")
            cbCOMPASS.Checked = CharToBool(Camp.Data.Item("Compass"))
            cbOnelink.Checked = CharToBool(Camp.Data.Item("OneLINK"))
            If Camp.Data.Item("ARC") <> "" Then tbARC.Text = Camp.Data.Item("ARC")
            If Camp.Data.Item("ActionCode") <> "" Then tbActionCode.Text = Camp.Data.Item("ActionCode")
            tbCommentText.Text = Camp.Data.Item("CommentText")
            tbDataFile.Text = Camp.Data.Item("DataFile")
            tbHTMLFile.Text = Camp.Data.Item("HTMLFile")

            tbFromEmail.Text = Camp.Data.Item("EmailFrom")
            tbFromDisplay.Text = Camp.Data.Item("EmailFromDisplay")

            NewCamp = False
        Else 'Create new record
            NewCamp = True
        End If
    End Sub

    Private Sub tbARC_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbARC.Enter
        tbARC.SelectAll()
    End Sub

    Private Sub tbActionCode_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActionCode.Enter
        tbActionCode.SelectAll()
    End Sub

    Private Sub btnBrowseDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDF.Click
        OpenFileDialog1.ShowDialog()
        tbDataFile.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub btnBrowseHF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseHF.Click
        OpenFileDialog1.ShowDialog()
        tbHTMLFile.Text = OpenFileDialog1.FileName
    End Sub

    Private Function CreateValsArrayList() As ArrayList
        CreateValsArrayList = New ArrayList()
        If NewCamp = False Then
            CreateValsArrayList.Add(Camp.ID.ToString)
        Else
            CreateValsArrayList.Add("")
        End If
        CreateValsArrayList.Add(tbSubject.Text.Replace("'", "''"))
        CreateValsArrayList.Add(CheckBoxValToChar(cbCOMPASS))
        CreateValsArrayList.Add(CheckBoxValToChar(cbOnelink))
        'only add contents of text box if not defualt value
        If tbARC.Text <> "ARC" Then
            CreateValsArrayList.Add(tbARC.Text)
        Else
            CreateValsArrayList.Add("")
        End If
        'only add contents of text box if not defualt value
        If tbActionCode.Text <> "Action Code" Then
            CreateValsArrayList.Add(tbActionCode.Text)
        Else
            CreateValsArrayList.Add("")
        End If
        CreateValsArrayList.Add(tbCommentText.Text.Replace("'", "''"))
        CreateValsArrayList.Add(tbDataFile.Text)
        CreateValsArrayList.Add(tbHTMLFile.Text)
        CreateValsArrayList.Add(tbFromEmail.Text)
        CreateValsArrayList.Add(tbFromDisplay.Text)
    End Function

    Private Function CheckBoxValToChar(ByRef CB As CheckBox) As String
        If CB.Checked Then
            Return "Y"
        Else
            Return "N"
        End If
    End Function

    Private Function CharToBool(ByVal DBVal As String) As Boolean
        If DBVal = "Y" Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function ConnStr() As String
        If CBool(My.Resources.TestMode) Then
            ConnStr = My.Resources.BSYSTestConn
        Else
            ConnStr = My.Resources.BSYSLiveConn
        End If
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'do data checks
        Dim emailCount As Integer


        Dim Conn As SqlConnection
        Dim Comm As SqlCommand
        Conn = New SqlConnection(ConnStr())
        If (NewCamp) Then
            Comm = New SqlCommand(String.Format("SELECT Count(EmailFromDisplay) FROM EMCP_DAT_EmailCampaigns WHERE EmailFrom = '" + tbFromEmail.Text + "'"), Conn)
        Else
            Comm = New SqlCommand(String.Format("SELECT Count(EmailFromDisplay) FROM EMCP_DAT_EmailCampaigns WHERE EmailFrom = '" + tbFromEmail.Text + "' AND CampID <> '" + CampNumber.ToString() + "'"), Conn)
        End If
        Conn.Open()
        emailCount = CInt(Comm.ExecuteScalar()) 'return campaign ID
        Conn.Close()



        isValid = True
        If tbSubject.TextLength = 0 Then
            MessageBox.Show("You must provide an Email Subject Line.  Please try again.", "Email Subject Line Needed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            isValid = False
        ElseIf cbCOMPASS.Checked And tbARC.TextLength <> 5 Then
            MessageBox.Show("You must provide a Compass ARC if comments are to be added to Compass.  Please try again.", "Compass ARC Needed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            isValid = False
        ElseIf cbOnelink.Checked And tbActionCode.TextLength <> 5 Then
            MessageBox.Show("You must provide a OneLINK Action Code if comments are to be added to OneLINK.  Please try again.", "OneLINK Action Code Needed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            isValid = False
        ElseIf tbDataFile.TextLength = 0 Then
            MessageBox.Show("You must provide a Data File.  Please try again.", "Data File Needed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            isValid = False
        ElseIf tbHTMLFile.TextLength = 0 Then
            MessageBox.Show("You must provide a HTML File.  Please try again.", "HTML File Needed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            isValid = False
        ElseIf tbFromEmail.TextLength = 0 Then
            MessageBox.Show("You must provide an email address from which to send.  Please try again.", "From email required", MessageBoxButtons.OK, MessageBoxIcon.Error)
            isValid = False
        ElseIf (emailCount > 0) And (NewCamp) Then 'See if email is used in a previous campaign
            MessageBox.Show("You must provide a unique email address from which to send.  This email has already been used with another campaign. Please try again.", "From email required", MessageBoxButtons.OK, MessageBoxIcon.Error)
            isValid = False
        ElseIf (emailCount > 0) And (NewCamp = False) Then 'See if email is used in a previous campaign
            MessageBox.Show("You must provide a unique email address from which to send.  This email has already been used with another campaign. Please try again.", "From email required", MessageBoxButtons.OK, MessageBoxIcon.Error)
            isValid = False
        ElseIf tbFromDisplay.TextLength = 0 Then
            MessageBox.Show("You must provide an email address from which to send.  Please try again.", "From email required", MessageBoxButtons.OK, MessageBoxIcon.Error)
            isValid = False
        End If

        'update or create Campaign
        If (isValid = True) Then
            If Not (Camp Is Nothing) Then
                Campaign.UpdateDB(CreateValsArrayList())
                Camp.Data = Campaign.GetUpdatedDataRow(Camp.ID)
            Else
                Camp = New Campaign(Campaign.GetUpdatedDataRow(Campaign.InsertIntoDB(CreateValsArrayList()))) 'create new campaign
                NewCamp = False 'it's not new any more
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        End
    End Sub

    Private Sub cbCOMPASS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCOMPASS.CheckedChanged
        If cbCOMPASS.Checked Then
            tbARC.ReadOnly = False
        Else
            tbARC.ReadOnly = True
            tbARC.Text = "ARC"
        End If
    End Sub

    Private Sub cbOnelink_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbOnelink.CheckedChanged
        If cbOnelink.Checked Then
            tbActionCode.ReadOnly = False
        Else
            tbActionCode.ReadOnly = True
            tbActionCode.Text = "Action Code"
        End If
    End Sub

    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTest.Click
        btnSave.PerformClick() 'save data first

        If (isValid) Then
            Dim AcctNum As String
            Dim TestEmailAddress As String = Environment.UserName() + "@utahsbr.edu"
            AcctNum = InputBox("You have chosen to test the campaign.  Please ensure that you have only one session open and that it is logged into the test region." + vbCrLf + vbCrLf + "Please provide a test Account Number, which is in the test region, for comment testing.", "Testing Campaign", "Account Number")
            While AcctNum.Length <> 10 OrElse IsNumeric(AcctNum) = False
                If MessageBox.Show("The Account Number you provided wasn't valid.  Do you still want to test the campaign?", "Invalid Account Number", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.No Then
                    'if the user decides to not test the campaign then stop process and leave sub
                    Exit Sub
                Else
                    AcctNum = InputBox("You have chosen to test the campaign.  Please ensure that you have only one session open and that it is logged into the test region." + vbCrLf + vbCrLf + "Please provide a test Account Number, which is in the test region, for comment testing.", "Testing Campaign", AcctNum)
                End If
            End While
            'create test file so it flows through the same logic that the running live logic runs through
            FileOpen(1, "T:\TestEmailCampaign.txt", OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
            PrintLine(1, "ACCTNUM,NAME,DX_ADR_EML") 'header row
            WriteLine(1, AcctNum, "EARL J PICKLESNARF", TestEmailAddress) 'data row
            FileClose(1)
            Me.Hide()
            Camp.Run("T:\TestEmailCampaign.txt")
        End If
    End Sub

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click

        btnSave.PerformClick() 'save data first

        If (isValid) Then
            If MessageBox.Show("You have chosen to run the campaign.  This functionality NEVER uses test functionality.  It will ALWAYS send the email to the email addresses in the file.  If you are testing be sure to be logged into the test region and have test email addresses in your file.  Do you wish to continue?", "Are You Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub 'exist sub if the user doesn't want to proceed
            End If

            Me.Hide()
            Camp.Run()
        End If
    End Sub
End Class