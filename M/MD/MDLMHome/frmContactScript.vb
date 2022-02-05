Imports System.Data.SqlClient
Imports SP

Public Class frmContactScript

#Region "Private Class Level Variables"
    Private bor As BorrowerLM
    Private script As String
    Private back As String
    Private connString As String
    Private sendChoice As String
    Private btnClick As String
    Private sqlStr As String
    Private judge As Boolean = False
    Private Ex As Boolean = False
    Private newlyDefault As Boolean = False
    Private eligible As Boolean = True
    Private saved As Boolean = False
    Private probe As String
    Private counter As Integer = 0
    Private awgscript As String
    Protected scripts As System.Collections.Generic.Dictionary(Of String, ScriptAndServiceMenuItem)
#End Region

    Private Sub frmContactScript_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Gathers information from borrower object and the session
        GatherInfo()
    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal tbor As BorrowerLM, ByRef _scripts As System.Collections.Generic.Dictionary(Of String, ScriptAndServiceMenuItem))
        InitializeComponent()
        bor = tbor
        scripts = _scripts
    End Sub

    ''' <summary>
    ''' Gather information from the borrower object that is passed in
    ''' Gather additional information from the session
    ''' Determine the borrower status using the data gathered
    ''' </summary>
    ''' <remarks></remarks>
#Region "Initial Screen Setup"
    Private Sub GatherInfo()
        Dim i As Integer = 7
        Dim count As Integer = 1

        'Gather information from either the borrower object or the session
        bor.LMBorCnt.SSN = bor.SSN
        bor.LMBorCnt.IneligibleReason = bor.IneligibleForRehabCode
        bor.LMBorCnt.RehabCounter = bor.RehabCounter
        bor.LMBorCnt.RehabActionCode = bor.RehabCode
        SP.FastPath("LP22I")
        bor.LMBorCnt.DrvLic = SP.GetText(6, 10, 2) & SP.GetText(6, 13, 20)

        If bor.LC05Detail Is Nothing Then
        Else
            Dim foundRows As DataRow() = bor.LC05Detail.Select("", "LenderPayOffDate DESC")
            Dim c As DataRow
            For Each c In foundRows
                bor.LMBorCnt.LendPayOffDate = c.Item(0)
                bor.LMBorCnt.CollCostProj = c.Item(14)
                bor.LMBorCnt.OutstandingBalance = c.Item(15)
                bor.LMBorCnt.TaxOffsetID = c.Item(21)
                bor.LMBorCnt.NextDueDate = c.Item(20)
                bor.LMBorCnt.ExpectPayAmt = c.Item(34)
                Exit For
            Next
        End If

        SP.FastPath("LP2CI")
        Dim gotFirst As Integer = 0
        If SP.Check4Text(1, 65, "REFERENCE SELECT") Then
            While SP.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                If SP.Check4Text(i, 27, "A") Then
                    SP.PutText(21, 13, CStr(count))
                    SP.Hit("Enter")
                    If SP.Q.Check4Text(8, 53, "Y") Then
                        If gotFirst = 2 Then
                            Exit While
                        End If
                        'Go to LP2CI and gather additional data from the session
                        CaptureLP2CI()
                        gotFirst = gotFirst + 1
                        SP.Q.Hit("F12")
                    ElseIf SP.Q.Check4Text(8, 53, "N") Then
                        SP.Q.Hit("F12")
                    End If
                End If
                i = i + 3
                count = count + 1
                If SP.Check4Text(i - 1, 3, " ") Or i > 18 Then
                    i = 7
                    count = 1
                    SP.Hit("F8")
                End If
            End While
        ElseIf SP.Check4Text(1, 59, "REFERENCE DEMOGRAPHICS") Then
            'Go to LP2CI and gather additional data from the session
            CaptureLP2CI()
        End If

        'Check status for Judgment, Execution & AWG
        SP.FastPath("LC67I" & bor.LMBorCnt.SSN)
        If SP.Q.Check4Text(1, 59, "LEGAL ACTION SELECTION") Then
            For index As Integer = 7 To 19
                If SP.Q.Check4Text(index, 10, "JUDICIAL") Or bor.LMBorCnt.LC67 = "JD" Then
                    If SP.Q.Check4Text(index, 10, "JUDICIAL") Then
                        PutText(21, 13, CStr(index - 6))
                        Hit("Enter")
                        If GetText(8, 19, 2) = "" Then
                            bor.LMBorCnt.LC67 = "JD"
                            SP.FastPath("LC67I" & bor.LMBorCnt.SSN)
                        End If
                    ElseIf SP.Q.Check4Text(index, 10, "EXECUTION") Then
                        bor.LMBorCnt.LC67 = "EX"
                        SP.FastPath("LC67I" & bor.LMBorCnt.SSN)
                        Exit For
                    End If
                ElseIf SP.Q.Check4Text(index, 10, "AWG") And Not bor.LMBorCnt.LC67 = "JD" Then
                    PutText(21, 13, CStr(index - 6))
                    Hit("Enter")
                    If GetText(8, 19, 2) = "" Then
                        bor.LMBorCnt.LC67 = "GG"
                    End If
                    SP.FastPath("LC67I" & bor.LMBorCnt.SSN)
                End If
            Next
        Else
            If GetText(8, 19, 2) = "" Then
                bor.LMBorCnt.LC67 = SP.Q.GetText(1, 19, 2)
            End If
        End If

        'Determines borrower status using the information gathered
        determineBorStatus()
    End Sub

    Private Sub CaptureLP2CI()

        Select Case SP.GetText(6, 15, 2)
            Case "E"
                bor.LMBorCnt.Ref1rel = "Employer"
            Case "EM"
                bor.LMBorCnt.Relationship = "Employer"
            Case "F"
                bor.LMBorCnt.Relationship = "Friend"
            Case "FR"
                bor.LMBorCnt.Relationship = "Friend"
            Case "G"
                bor.LMBorCnt.Relationship = "Guardian"
            Case "M"
                bor.LMBorCnt.Relationship = "Spouse"
            Case "N"
                bor.LMBorCnt.Relationship = "Not Available"
            Case "NE"
                bor.LMBorCnt.Relationship = "Neighbor"
            Case "O"
                bor.LMBorCnt.Relationship = "Other"
            Case "OT"
                bor.LMBorCnt.Relationship = "Other"
            Case "P"
                bor.LMBorCnt.Relationship = "Parent"
            Case "PA"
                bor.LMBorCnt.Relationship = "Parent"
            Case "R"
                bor.LMBorCnt.Relationship = "Relative"
            Case "RE"
                bor.LMBorCnt.Relationship = "Relative"
            Case "RM"
                bor.LMBorCnt.Relationship = "Roommate"
            Case "S"
                bor.LMBorCnt.Relationship = "Sibling"
            Case "SI"
                bor.LMBorCnt.Relationship = "Sibling"
            Case "SP"
                bor.LMBorCnt.Relationship = "Spouse"
        End Select
        'temporary string used to put "-" in the phone number
        Dim temp As String

        If counter = 0 Then
            bor.LMBorCnt.Ref1fname = Trim(SP.Q.GetText(4, 44, 12))
            bor.LMBorCnt.Ref1lname = Trim(SP.Q.GetText(4, 5, 25))
            bor.LMBorCnt.Ref1Addy1 = SP.Q.GetText(8, 9, 35)
            bor.LMBorCnt.Ref1Addy2 = SP.Q.GetText(9, 9, 35)
            bor.LMBorCnt.Ref1city = SP.Q.GetText(10, 9, 35)
            bor.LMBorCnt.Ref1state = SP.Q.GetText(10, 52, 2)
            bor.LMBorCnt.Ref1zip = SP.Q.GetText(10, 60, 9)
            temp = SP.Q.GetText(13, 17, 10)
            temp = temp.Insert(6, "-")
            temp = temp.Insert(3, "-")
            bor.LMBorCnt.Ref1phone = temp
            bor.LMBorCnt.Ref1rel = SP.Q.GetText(6, 20, 8)
            counter = counter + 1
        Else
            bor.LMBorCnt.Ref2fname = Trim(SP.Q.GetText(4, 44, 12))
            bor.LMBorCnt.Ref2lname = Trim(SP.Q.GetText(4, 5, 25))
            bor.LMBorCnt.Ref2addy1 = SP.Q.GetText(8, 9, 35)
            bor.LMBorCnt.Ref2Addy2 = SP.Q.GetText(9, 9, 35)
            bor.LMBorCnt.Ref2city = SP.Q.GetText(10, 9, 35)
            bor.LMBorCnt.Ref2state = SP.Q.GetText(10, 52, 2)
            bor.LMBorCnt.Ref2zip = SP.Q.GetText(10, 60, 9)
            temp = SP.Q.GetText(13, 17, 10)
            temp = temp.Insert(6, "-")
            temp = temp.Insert(3, "-")
            bor.LMBorCnt.Ref2phone = temp
            bor.LMBorCnt.Ref2rel = SP.Q.GetText(6, 20, 8)
        End If

    End Sub

    Private Sub determineBorStatus()

        If SP.Q.TestMode Then
            connString = "Data Source=OPSDEV;Initial Catalog=MauiDUDE;Integrated Security=SSPI;"
        Else
            connString = "Data Source=NOCHOUSE;Initial Catalog=MauiDUDE;Integrated Security=SSPI;"
        End If

        'Create Sql Strings depending on borrower status    
        'Not eligiblefor Rehab
        If bor.LMBorCnt.IneligibleReason = "PR" Or bor.LMBorCnt.IneligibleReason = "IN" Then
            If bor.DD136 = True Then
                gbxScript.Text = "Not Rehab Eligible"
                sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Not Rehab Eligible'"
                Connection(sqlStr)
                NotEligRehab()
            ElseIf bor.DD136 = False And DateAdd("d", -60, Date.Now.ToShortDateString) > bor.String8ToDate(bor.LMBorCnt.LendPayOffDate) Then
                gbxScript.Text = "No Rehab-Missed Payment"
                sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'No Rehab-Missed Payment'"
                Connection(sqlStr)
                NotEligRehab()
            ElseIf bor.DD136 = False And DateAdd("d", -60, Date.Now.ToShortDateString) < bor.String8ToDate(bor.LMBorCnt.LendPayOffDate) Then
                gbxScript.Text = "New Not Rehab Eligible"
                sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'New Not Rehab Eligible' and Paragraph = 'A'"
                Connection(sqlStr)
                NotEligRehab()
            End If

            'Execution
        ElseIf bor.LMBorCnt.LC67 = "EX" Then
            gbxScript.Text = "Execution"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Judgments' and Paragraph = 'B'"
            Connection(sqlStr)
            Execution()
            'Judgement

        ElseIf bor.LMBorCnt.LC67 = "JD" Then
            gbxScript.Text = "Judgment"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Judgments' and Paragraph = 'A'"
            Connection(sqlStr)
            Judgment()

            'AWG
        ElseIf bor.LMBorCnt.LC67 = "GG" Then
            gbxScript.Text = "AWG"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'AWG' and Paragraph = 'A'"
            Connection(sqlStr)
            AWG()

        ElseIf bor.LMBorCnt.NextDueDate <> "MMDDCCYY" And bor.LMBorCnt.NextDueDate <> Nothing Then
            'Past Due With Collection Cost
            If bor.LMBorCnt.CollCostProj <> 0 And bor.String8ToDate(bor.LMBorCnt.NextDueDate) > DateAdd("d", -20, Date.Now) Then
                If bor.LMBorCnt.RehabCounter >= 1 Then
                    gbxScript.Text = "Past Due With Collection Costs On Track for Rehabilitation"
                    sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Past Due on Payment Arrangement w/ Collection Costs'" _
                    & " and Paragraph = 'A'"
                    Connection(sqlStr)
                    PastDueWCollection()
                ElseIf bor.LMBorCnt.RehabCounter < 1 Then
                    gbxScript.Text = "Past Due With Collection Costs. Start Over Rehab"
                    sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Past Due on Payment Arrangement w/ Collection Costs'" _
                    & " and Paragraph = 'B'"
                    Connection(sqlStr)
                    PastDueWCollection()
                End If

                'Past Due Without Collection Costs
            ElseIf bor.LMBorCnt.CollCostProj = 0 And bor.String8ToDate(bor.LMBorCnt.NextDueDate) > DateAdd("d", -20, Date.Now) Then
                If bor.LMBorCnt.RehabCounter >= 1 Then
                    gbxScript.Text = "Past Due With Out Collection Costs On Track for Rehabilitation"
                    sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Past Due on Payment Arrangement w/o Collection Costs'" _
                    & " and Paragraph = 'A'"
                    Connection(sqlStr)
                    PastDueWOCollection()
                ElseIf bor.LMBorCnt.RehabCounter < 1 Then
                    gbxScript.Text = "Past Due With Out Collection Costs. Start Over Rehab"
                    sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Past Due on Payment Arrangement w/o Collection Costs'" _
                    & " and Paragraph = 'B'"
                    Connection(sqlStr)
                    PastDueWOCollection()
                End If

                'New Default
            ElseIf bor.String8ToDate(bor.LMBorCnt.LendPayOffDate) < DateAdd("d", -60, Date.Now) Then
                If bor.DateLastCntct = "" Then
                    gbxScript.Text = "Newly Defaulted"
                    sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Newly Defaulted Borrower' and Paragraph = 'A'"
                    Connection(sqlStr)
                    NewDefault()
                End If
            Else
                MsgBox("This borrower does not meet the criteria for the contact script", MsgBoxStyle.OkOnly, "Does not meet criteria")
            End If

            'No Payment Arrangement
        ElseIf bor.DD136 = False Then
            gbxScript.Text = "No Payment Arrangement"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'No Payment Arrangement'"
            Connection(sqlStr)
            NoPayArrang()
        Else
            MsgBox("This borrower does not meet the criteria for the contact script", MsgBoxStyle.OkOnly, "Does not meet criteria")
        End If
    End Sub
#End Region

    ''' <summary>
    ''' Connection to the SQL database
    ''' Changes the 6 labels on the form according to the data pulled
    ''' </summary>
    ''' <param name="Sqlstr">The command text used to pull data from the database</param>
    ''' <remarks></remarks>
#Region "Sql Connection"
    Private Sub Connection(ByVal Sqlstr As String)
        Dim conn As New SqlConnection()
        Dim cmd As New SqlCommand()
        Dim dr As SqlDataReader
        Dim count As Integer
        'count variable telling the datareader which label to use.
        If gbxScript.Text = "Payment Options" Then
            count = 1
        Else
            count = 0
        End If

        conn.ConnectionString = connString
        conn.Open()
        cmd.Connection = conn
        cmd.CommandText = Sqlstr
        dr = cmd.ExecuteReader
        While (dr.Read())
            If count = 0 Then
                'Change the characters in the script to the information that was pulled earlier.
                If dr("Script").Contains("[") Then
                    If bor.LMBorCnt.NextDueDate = "MMDDCCYY" Then
                        lblScript.Text = dr("Script").Replace("[]", bor.LMBorCnt.RehabCounter.ToString)
                        lblScript.Text = lblScript.Text.Replace("[$]", bor.LMBorCnt.OutstandingBalance)
                        lblScript.Text = lblScript.Text.Replace("[$$]", "$" & bor.LMBorCnt.PtpAmt)
                        lblScript.Text = lblScript.Text.Replace("[date]", bor.LMBorCnt.PtpDate)
                    Else
                        lblScript.Text = dr("Script").Replace("[]", bor.LMBorCnt.RehabCounter.ToString)
                        lblScript.Text = lblScript.Text.Replace("[date1]", bor.String8ToDate(bor.LMBorCnt.NextDueDate))
                        lblScript.Text = lblScript.Text.Replace("[date2]", DateAdd("d", 20, bor.String8ToDate(bor.LMBorCnt.NextDueDate)))
                        lblScript.Text = lblScript.Text.Replace("[$]", bor.LMBorCnt.OutstandingBalance)
                        lblScript.Text = lblScript.Text.Replace("[$$]", "$" & bor.LMBorCnt.PtpAmt)
                        lblScript.Text = lblScript.Text.Replace("[date]", bor.LMBorCnt.PtpDate)
                    End If
                Else
                    lblScript.Text = dr("Script")
                End If
                If probe = "ConsolApp" Then
                    Dim QuestTask As New frmCSQueueTask(dr("Script"), bor)
                    QuestTask.Show()
                    lblScript.Text = ""
                    btnYes.Visible = False
                    btnNo.Visible = True
                    btnNo.Text = "Finished"
                    probe = "Close"
                End If
                If Not IsDBNull(dr("Note")) Then
                    lblAgentNote.Text = dr("Note")
                Else
                    lblAgentNote.Text = ""
                End If
                count = count + 1
            ElseIf count = 1 Then
                lbl1.Text = dr("Script")
                If Not IsDBNull(dr("Note")) Then
                    lblAgentNote.Text = dr("Note")
                Else
                    lblAgentNote.Text = ""
                End If
                count = count + 1
            ElseIf count = 2 Then
                lbl2.Text = dr("Script")
                If Not IsDBNull(dr("Note")) Then
                    lblAgentNote.Text = dr("Note")
                Else
                    lblAgentNote.Text = ""
                End If
                count = count + 1
            ElseIf count = 3 Then
                lbl3.Text = dr("Script")
                If Not IsDBNull(dr("Note")) Then
                    lblAgentNote.Text = dr("Note")
                Else
                    lblAgentNote.Text = ""
                End If
                count = count + 1
            End If
        End While
        dr.Close()
        conn.Close()
    End Sub
#End Region

    ''' <summary>
    ''' Borrower Status screen setup
    ''' </summary>
    ''' <remarks></remarks>
#Region "Borrower Status"
    Private Sub NotEligRehab()
        If gbxScript.Text = "Not Rehab Eligible" Then
            btnOne.Visible = False
            btnBack.Visible = False
            btnYes.Visible = True
            btnNo.Visible = TestMode()
            btnYes.Text = "Yes"
            btnNo.Text = "No"
        ElseIf gbxScript.Text = "No Rehab-Missed Payment" Then
            btnOne.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnBack.Visible = False
            btnYes.Text = "Yes"
            btnNo.Text = "No"
        ElseIf gbxScript.Text = "New Not Rehab Eligible" Then
            btnOne.Visible = False
            btnBack.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnYes.Text = "Yes"
            btnNo.Text = "No"
        End If
    End Sub

    Private Sub Execution()
        If gbxScript.Text = "Execution" Then
            btnOne.Visible = True
            btnBack.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnOne.Text = "VWA"
            btnYes.Text = "DPA"
            btnNo.Text = "No"
            awgscript = "Ex"
        End If
    End Sub

    Private Sub Judgment()
        If gbxScript.Text = "Judgment" Then
            btnOne.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnBack.Visible = False
            btnYes.Text = "Yes"
            btnNo.Text = "No"
        End If
    End Sub

    Private Sub AWG()
        If gbxScript.Text = "AWG" Then
            btnOne.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnBack.Visible = False
            btnYes.Text = "Yes"
            btnNo.Text = "No"
            awgscript = "AWG"
        End If
    End Sub

    Private Sub PastDueWCollection()
        If gbxScript.Text = "Past Due With Collection Costs On Track for Rehabilitation" Or _
            gbxScript.Text = "Past Due With Collection Costs. Start Over Rehab" Then
            btnOne.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnBack.Visible = False
            btnYes.Text = "Yes"
            btnNo.Text = "No"
        End If
    End Sub

    Private Sub PastDueWOCollection()
        If gbxScript.Text = "Past Due With Out Collection Costs On Track for Rehabilitation" Or _
            gbxScript.Text = "Past Due With Out Collection Costs. Start Over Rehab" Then
            btnBack.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnOne.Visible = False
            btnYes.Text = "Yes"
            btnNo.Text = "No"
            probe = "PDWOC"
        End If
    End Sub

    Private Sub NoPayArrang()
        If gbxScript.Text = "No Payment Arrangement" Then
            btnBack.Visible = False
            btnOne.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnYes.Text = "Yes"
            btnNo.Text = "No"
        End If
    End Sub

    Private Sub NewDefault()
        If gbxScript.Text = "Newly Defaulted" And newlyDefault <> False Then
            gbxScript.Text = "Newly Defaulted"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Newly Defaulted Borrower' and Paragraph = 'B'"
            Connection(sqlStr)
        End If
        btnBack.Visible = False
        btnOne.Visible = False
        btnYes.Visible = True
        btnNo.Visible = True
        btnYes.Text = "Yes"
        btnNo.Text = "No"
    End Sub
#End Region

    ''' <summary>
    ''' This section will change the script depending on365 the status
    ''' </summary>
    ''' <remarks></remarks>
#Region "Borrower Status depending on response"
    Private Sub PaymentOptions()
        If gbxScript.Text = "Past Due With Collection Costs On Track for Rehabilitation" Or _
        gbxScript.Text = "Past Due With Collection Costs. Start Over Rehab" Or _
        gbxScript.Text = "Past Due With Out Collection Costs On Track for Rehabilitation" Or _
        gbxScript.Text = "Past Due With Out Collection Costs. Start Over Rehab" Then
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Payment Options' AND Script NOT LIKE '- Auto%'"
        Else
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Payment Options'"
        End If
        btnBack.Visible = False
        btnOne.Visible = False
        btnYes.Visible = True
        btnNo.Visible = True
        btnYes.Text = "Check by Phone"
        btnNo.Text = "Promise-to-Pay"
        gbxScript.Text = "Payment Options"
        lblScript.Text = "You have a few options to make your payment:"
        Connection(sqlStr)
    End Sub

    Private Sub FedDirectConsol()
        btnBack.Visible = False
        btnOne.Visible = False
        btnYes.Visible = True
        btnNo.Visible = True
        btnYes.Text = "Yes"
        btnNo.Text = "No"
        gbxScript.Text = "Fed Direct Consol"
        sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Fed Direct Consol'"
        Connection(sqlStr)
    End Sub

    Private Sub ProbingQuestions(Optional ByVal selection As String = "")

        If selection = "A" Then
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Probing Questions' and Paragraph = 'A'"

            If gbxScript.Text = "No Rehab-Missed Payment" Then
                probe = "NoRMP"
            ElseIf gbxScript.Text = "Past Due With Collection Costs On Track for Rehabilitation" Then
                probe = "OnTrack"
            ElseIf gbxScript.Text = "Past Due With Collection Costs. Start Over Rehab" Then
                probe = "Restart"
            ElseIf gbxScript.Text = "Past Due With Out Collection Costs On Track for Rehabilitation" Then
                probe = "OnTrackWO"
            ElseIf gbxScript.Text = "Past Due With Out Collection Costs. Start Over Rehab" Then
                probe = "RestartWO"
            End If
            btnBack.Visible = True
            btnOne.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnYes.Text = "Payment Arragements"
            btnNo.Text = "No"
        ElseIf selection = "B" Then
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Probing Questions' and Paragraph = 'B'"
            If judge = True Then
                btnBack.Visible = True
                btnOne.Visible = False
                btnYes.Visible = False
                btnNo.Visible = True
                btnBack.Text = "Back"
                btnNo.Text = "Next"
                probe = "JD"
            ElseIf gbxScript.Text = "No Payment Arrangement" Or gbxScript.Text = "Newly Defaulted" Then
                btnBack.Visible = False
                btnOne.Visible = False
                btnYes.Visible = True
                btnNo.Visible = True
                btnYes.Text = "Payment Arrangements"
                btnNo.Text = "No"
                probe = "NoPay"
            End If
        ElseIf selection = "C" Then
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Probing Questions' and Paragraph = 'C'"

            If gbxScript.Text = "Execution" Then
                probe = "Ex"
            ElseIf gbxScript.Text = "AWG" Then
                probe = "AWG"
            End If
            btnBack.Visible = False
            btnOne.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnYes.Text = "Voluntary Payments"
            btnNo.Text = "No"

        ElseIf selection = "C" And gbxScript.Text = "No Rehab-Missed Payment" Then
            btnBack.Visible = False
            btnOne.Visible = False
            btnYes.Visible = True
            btnNo.Visible = True
            btnYes.Text = "Voluntary Payments"
            btnNo.Text = "No"
        ElseIf selection = "D" Then
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Probing Questions' and Paragraph = 'D'"
        Else
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Probing Questions'"
        End If

        gbxScript.Text = "Probing Questions"
        Connection(sqlStr)
    End Sub

    Private Sub NuclearAtom(ByVal eligible As Boolean)
        If eligible Then
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Nuclear Option' and Paragraph = 'B'"
        Else
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Nuclear Option' and Paragraph = 'A'"
        End If

        btnBack.Visible = False
        btnOne.Visible = False
        btnYes.Visible = True
        btnNo.Visible = True
        btnYes.Text = "Finished"
        btnNo.Text = "Payment Arrangements"
        gbxScript.Text = "Nuclear Option"
        Connection(sqlStr)
    End Sub

    Private Sub ClosingStatements(ByVal p As String)
        gbxScript.Text = "Closing Statements"
        btnBack.Visible = False
        btnOne.Visible = False
        btnYes.Visible = False
        btnNo.Visible = True
        lbl1.Text = ""
        lbl2.Text = ""
        lbl3.Text = ""
        btnNo.Text = "Finished"
        sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Closing Statements' and Paragraph = '" & p & "'"
        Connection(sqlStr)
    End Sub

    Private Sub ScriptRun(ByVal sr As String)
        lblScript.Text = "Did the script run to completion?"
        lbl1.Text = ""
        lbl2.Text = ""
        lbl3.Text = ""
        btnOne.Visible = False
        btnYes.Visible = True
        btnNo.Visible = True
        btnYes.Text = "Yes"
        btnNo.Text = "No"
        If sr = "AWG" Then
            gbxScript.Text = "AWG Release Script"
        ElseIf sr = "CBP" Then
            gbxScript.Text = "Check By Phone"
        ElseIf sr = "PTP" Then
            gbxScript.Text = "Promise to Pay"
        End If
    End Sub
#End Region

    ''' <summary>
    ''' Click Events for the four buttons on the form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
#Region "Click Events"
    Private Sub btnOne_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOne.Click
        ScriptRun("AWG")
        frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("AWG Release"), bor.LMBorCnt.SSN)
    End Sub

    Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Select Case gbxScript.Text
            Case "Not Rehab Eligible"
                frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("Payment Arrangements"), bor.LMBorCnt.SSN)
                PaymentOptions()
            Case "No Rehab-Missed Payment"
                PaymentOptions()
            Case "New Not Rehab Eligible"
                frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("Payment Arrangements"), bor.LMBorCnt.SSN)
                PaymentOptions()
            Case "Execution"
                ScriptRun("AWG")
                frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("AWG Release"), bor.LMBorCnt.SSN)
            Case "Judgment"
                frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("Payment Arrangements"), bor.LMBorCnt.SSN)
                PaymentOptions()
            Case "AWG"
                If btnYes.Text = "Yes" Then
                    btnOne.Visible = True
                    btnYes.Text = "DPA"
                    btnOne.Text = "VWA"
                    btnNo.Text = "No"
                    gbxScript.Text = "AWG"
                    sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'AWG' and Paragraph = 'B'"
                    Connection(sqlStr)
                Else
                    ScriptRun("AWG")
                    frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("AWG Release"), bor.LMBorCnt.SSN)
                    RehabActionCode()
                End If
            Case "Past Due With Collection Costs On Track for Rehabilitation"
                PaymentOptions()
            Case "Past Due With Collection Costs. Start Over Rehab"
                RehabActionCode()
                PaymentOptions()
            Case "Past Due With Out Collection Costs On Track for Rehabilitation"
                PaymentOptions()
            Case "Past Due With Out Collection Costs. Start Over Rehab"
                RehabActionCode()
                PaymentOptions()
            Case "No Payment Arrangement"
                RehabActionCode()
                frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("Payment Arrangements"), bor.LMBorCnt.SSN)
                PaymentOptions()
            Case "Newly Defaulted"
                If newlyDefault = True Then
                    RehabActionCode()
                    frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("Payment Arrangements"), bor.LMBorCnt.SSN)
                    PaymentOptions()
                Else
                    RehabActionCode()
                    PaymentOptions()
                End If
            Case "Payment Options"
                ScriptRun("CBP")
                frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("OneLink Check by Phone"), bor.LMBorCnt.SSN)
            Case "Probing Questions"
                If probe = "Ex" Then
                    gbxScript.Text = "Execution"
                    Execution()
                    Connection("Select Script, Note from LMContactScript where BorStatus = 'Judgments' and Paragraph = 'B'")
                ElseIf probe = "AWG" Then
                    gbxScript.Text = "AWG"
                    sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'AWG' and Paragraph = 'B'"
                    AWG()
                    btnOne.Visible = True
                    btnOne.Text = "VWA"
                    btnYes.Text = "DPA"
                    Connection(sqlStr)
                ElseIf probe = "NoRMP" Or probe = "OnTrack" Or probe = "Restart" Or probe = "OnTrackWO" Or probe = "RestartWO" Or probe = "NoPay" Then
                    frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("Payment Arrangements"), bor.LMBorCnt.SSN)
                    PaymentOptions()
                End If
            Case "Fed Direct Consol"
                probe = "ConsolApp"
                Connection("Select Script, Note from LMContactScript where BorStatus = 'Consol Application'")
            Case "Nuclear Option"
                If saved = False And tbxAddNote.Text <> "" Then
                    If tbxActCode.Text <> "" Then
                        btnSaveNote_Click(sender, e)
                        Me.Close()
                    Else
                        MsgBox("Please enter a comment and provide an Action Code", MsgBoxStyle.Exclamation, "Missing Information")
                    End If
                Else
                    Me.Close()
                End If
            Case "Promise to Pay"
                ClosingStatements("B")
            Case "Check By Phone"
                ClosingStatements("A")
            Case "AWG Release Script"
                RehabActionCode()
                ClosingStatements("A")
        End Select
    End Sub

    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Select Case gbxScript.Text
            Case "Not Rehab Eligible"
                NuclearAtom(False)
            Case "No Rehab-Missed Payment"
                ProbingQuestions("A")
            Case "New Not Rehab Eligible"
                FedDirectConsol()
            Case "Execution"
                ProbingQuestions("C")
            Case "Judgment"
                judge = True
                ProbingQuestions("B")
            Case "AWG"
                ProbingQuestions("C")
            Case "Past Due With Collection Costs On Track for Rehabilitation"
                ProbingQuestions("A")
            Case "Past Due With Collection Costs. Start Over Rehab"
                ProbingQuestions("A")
            Case "Past Due With Out Collection Costs On Track for Rehabilitation"
                ProbingQuestions("A")
            Case "Past Due With Out Collection Costs. Start Over Rehab"
                ProbingQuestions("A")
            Case "No Payment Arrangement"
                ProbingQuestions("B")
            Case "Newly Defaulted"
                If newlyDefault = False Then
                    newlyDefault = True
                    NewDefault()
                ElseIf newlyDefault Then
                    ProbingQuestions("B")
                End If
            Case "Payment Options"
                ScriptRun("PTP")
                Dim QuestTask As New frmCSQueueTask("What day will you send your payment", bor, True)
                QuestTask.Show()
            Case "Probing Questions"
                If probe = "Ex" Or probe = "AWG" Or probe = "JD" Or probe = "OnTrack" Or probe = "Restart" Or probe = "OnTrackWO" Or probe = "RestartWO" Or probe = "NoPay" Then
                    NuclearAtom(True)
                ElseIf probe = "NoRMP" Then
                    FedDirectConsol()
                End If
            Case "Payment Arragements"
                frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("Payment Arrangements"), bor.LMBorCnt.SSN)
            Case "No"
                If gbxScript.Text = "No Rehab-Missed Payment" Then
                    ProbingQuestions("C")
                End If
            Case "Nuclear Option"
                frmGenericScriptAndServicesEnabled.RunScriptExternalScript(scripts("Payment Arrangements"), bor.LMBorCnt.SSN)
                PaymentOptions()
            Case "Fed Direct Consol"
                If probe = "Close" Then
                    If bor.LMBorCnt.MadeChanges Then
                        FastPath("LP2CA" & bor.SSN)
                        If bor.LMBorCnt.RefChanged = "Ref1" Then
                            SP.Q.PutText(4, 5, bor.LMBorCnt.Ref1lname)
                            SP.Q.PutText(4, 44, bor.LMBorCnt.Ref1fname)
                            SP.Q.PutText(5, 9, "O")
                            bor.LMBorCnt.ChangeRef(bor.LMBorCnt.Ref1rel)
                            SP.Q.PutText(6, 15, bor.LMBorCnt.Ref1rel)
                            SP.Q.PutText(8, 9, bor.LMBorCnt.Ref1Addy1)
                            SP.Q.PutText(8, 53, "Y")
                            SP.Q.PutText(9, 9, bor.LMBorCnt.Ref1Addy2)
                            SP.Q.PutText(10, 9, bor.LMBorCnt.Ref1city)
                            SP.Q.PutText(10, 52, bor.LMBorCnt.Ref1state)
                            SP.Q.PutText(10, 60, bor.LMBorCnt.Ref1zip)
                            bor.LMBorCnt.Ref1phone = bor.LMBorCnt.Ref1phone.Replace("-", "")
                            SP.Q.PutText(13, 17, bor.LMBorCnt.Ref1phone)
                            SP.Q.PutText(13, 42, "Y")
                        ElseIf bor.LMBorCnt.RefChanged = "Ref2" Then
                            SP.Q.PutText(4, 5, bor.LMBorCnt.Ref2lname)
                            SP.Q.PutText(4, 44, bor.LMBorCnt.Ref2fname)
                            SP.Q.PutText(5, 9, "O")
                            bor.LMBorCnt.ChangeRef(bor.LMBorCnt.Ref2rel)
                            SP.Q.PutText(6, 15, bor.LMBorCnt.Ref1rel)
                            SP.Q.PutText(8, 9, bor.LMBorCnt.Ref2addy1)
                            SP.Q.PutText(8, 53, "Y")
                            SP.Q.PutText(9, 9, bor.LMBorCnt.Ref2Addy2)
                            SP.Q.PutText(10, 9, bor.LMBorCnt.Ref2city)
                            SP.Q.PutText(10, 52, bor.LMBorCnt.Ref2state)
                            SP.Q.PutText(10, 60, bor.LMBorCnt.Ref2zip)
                            bor.LMBorCnt.Ref2phone = bor.LMBorCnt.Ref2phone.Replace("-", "")
                            SP.Q.PutText(13, 17, bor.LMBorCnt.Ref2phone)
                            SP.Q.PutText(13, 42, "Y")
                        End If
                        SP.Q.Hit("F6")
                        If SP.Q.Check4Text(2, 23, "48003 DATA SUCCESSFULLY ADDED") Then
                            MsgBox("Reference Information updated successfully", MsgBoxStyle.Information, "Reference Update")
                        End If
                    End If
                    If saved = False And tbxAddNote.Text <> "" Then
                        btnSaveNote_Click(sender, e)
                        MsgBox("Comments saved", MsgBoxStyle.Exclamation, "Saved")
                    End If
                    Me.Close()
                Else
                    NuclearAtom(False)
                End If
            Case "Closing Statements"
                If saved = False And tbxAddNote.Text <> "" Then
                    btnSaveNote_Click(sender, e)
                End If
                Me.Close()
            Case "Promise to Pay"
                PaymentOptions()
            Case "No Due Date"
                Me.Close()
            Case "Check By Phone"
                PaymentOptions()
            Case "AWG Release Script"
                If awgscript = "AWG" Then
                    RehabActionCode()
                    gbxScript.Text = "AWG"
                    sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'AWG' and Paragraph = 'B'"
                    AWG()
                    btnOne.Visible = True
                    btnOne.Text = "VWA"
                    btnYes.Text = "DPA"
                    Connection(sqlStr)
                ElseIf awgscript = "Ex" Then
                    gbxScript.Text = "Execution"
                    sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Judgments' and Paragraph = 'B'"
                    Connection(sqlStr)
                    Execution()
                End If
        End Select
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If judge Then
            gbxScript.Text = "Judgment"
            judge = False
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Judgments' and Paragraph = 'A'"
            Connection(sqlStr)
            Judgment()
        ElseIf probe = "NoRMP" Then
            gbxScript.Text = "No Rehab-Missed Payment"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'No Rehab-Missed Payment'"
            Connection(sqlStr)
            NotEligRehab()
        ElseIf probe = "OnTrack" Then
            gbxScript.Text = "Past Due With Collection Costs On Track for Rehabilitation"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Past Due on Payment Arrangement w/ Collection Costs'" _
                & " and Paragraph = 'A'"
            Connection(sqlStr)
            PastDueWCollection()
        ElseIf probe = "Restart" Then
            gbxScript.Text = "Past Due With Collection Costs. Start Over Rehab"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Past Due on Payment Arrangement w/ Collection Costs'" _
            & " and Paragraph = 'B'"
            Connection(sqlStr)
            PastDueWCollection()
        ElseIf probe = "OnTrackWO" Then
            gbxScript.Text = "Past Due With Out Collection Costs On Track for Rehabilitation"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Past Due on Payment Arrangement w/o Collection Costs'" _
            & " and Paragraph = 'A'"
            Connection(sqlStr)
            PastDueWOCollection()
        ElseIf probe = "RestartWO" Then
            gbxScript.Text = "Past Due With Out Collection Costs. Start Over Rehab"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Past Due on Payment Arrangement w/o Collection Costs'" _
            & " and Paragraph = 'B'"
            Connection(sqlStr)
            PastDueWOCollection()
        ElseIf probe = "PDWOC" Then
            gbxScript.Text = "Past Due With Out Collection Costs On Track for Rehabilitation"
            sqlStr = "Select Script, Note from LMContactScript where BorStatus = 'Past Due on Payment Arrangement w/o Collection Costs'" _
            & " and Paragraph = 'A'"
            Connection(sqlStr)
            PastDueWOCollection()
        End If
    End Sub

    Private Sub btnSaveNote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveNote.Click
        If tbxAddNote.Text <> "" And tbxActCode.Text <> "" Then
            If Len(tbxActCode.Text) = 5 Then
                FastPath("LP50A")
                SP.Q.PutText(3, 13, bor.SSN)
                SP.Q.PutText(9, 20, tbxActCode.Text)
                SP.Q.Hit("Enter")
                SP.Q.PutText(7, 2, "TC")
                SP.Q.PutText(7, 5, "03")
                SP.Q.PutText(13, 2, tbxAddNote.Text)
                SP.Q.Hit("Enter")
                SP.Q.Hit("F6")
                If SP.Q.Check4Text(22, 3, "48003") Then
                    MsgBox("Comments saved", MsgBoxStyle.OkOnly, "Successful")
                    tbxActCode.Text = ""
                    tbxAddNote.Text = "Comment Added Successfully"
                End If
            Else
                MsgBox("Incorrect Action Code", MsgBoxStyle.Exclamation, "Too Short")
            End If
        Else
            MsgBox("Please enter a comment and provide an Action Code", MsgBoxStyle.Exclamation, "Missing Information")
        End If
        saved = True
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
#End Region

    ''' <summary>
    ''' If the Rehab Action Code is 08 then do not leave comments in LP50A, otherwise leave a comment.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RehabActionCode()
        If Not bor.LMBorCnt.RehabActionCode = "08" Then
            FastPath("LP50A" & bor.SSN)
            PutText(7, 20, "TC")
            PutText(8, 20, "03")
            PutText(9, 20, "DRBWR")
            Hit("Enter")
            PutText(13, 2, "Borrower requested to rehabilitate defaulted student loans")
            Hit("Enter")
            Hit("F6")
            If Check4Text(22, 3, "48003") Then
                Exit Sub
            Else
                MsgBox("Activity comment did not get added", MsgBoxStyle.Exclamation, "No comment added")
            End If
        End If
    End Sub

End Class