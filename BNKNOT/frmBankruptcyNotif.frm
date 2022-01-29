VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmBankruptcyNotif 
   Caption         =   "Bankruptcy Notification "
   ClientHeight    =   9600
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   7350
   OleObjectBlob   =   "frmBankruptcyNotif.frx":0000
End
Attribute VB_Name = "frmBankruptcyNotif"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False





'Misc -----------------------------------------------------------------------------------
Private AddBRToCOMPASS As Boolean 'decided in the TS24AddBankruptcyToCOMPASS function
Private AddBRToOneLINK As Boolean 'decided in the LC05AddBankruptcyToOneLINK function
Private ApplicableClaims() As String 'a list of claims on LC05 that will be marked on LC34
Private ClaimPaidDT() As String '<4> list of claim paid dates on LC05
Private DocketNum As String 'once the docket number pices are gathered then this will hold the full docket number
Private UID As String 'userid
Private COMPASSDateIncurred As String 'earliest disbursement date with a balance
Private OneLINKDateIncurred As String 'earliest disbursement date with a balance
Private COMPASSPayOffAmt As String 'total payoff amount for COMPASS
Private OneLINKPayOffAmt As String 'total payoff amount for OneLINK
Private ResponseGiven As Boolean 'This tracks if a response has already been given to the user
Private TotalRefund As Double 'total refund gathered from LC41

'Borrowers Demographic Info -------------------------------------------------------------
Private Addr1 As String
Private Addr2 As String
Private City As String
Private State As String
Private ZIP As String
Private AccNum As String

'Court Info -----------------------------------------------------------------------------
Private CourtState As String
Private CourtCity As String
Private CourtID As String

'Attorney Info --------------------------------------------------------------------------
Private ARefID As String
Private AFN As String
Private ALN As String
Private AAddr1 As String
Private AAddr2 As String
Private ACity As String
Private AState As String
Private AZip As String
Private APhone As String

Private DocketVarified As Boolean
Private LC12BankruptcyStatus() As String

Private Sub btnCancel_Click()
    End
End Sub

Private Sub btnDone_Click()
    End
End Sub

Private Sub btnOk_Click()
    Dim BRRecordCreated As Boolean
    Dim x As Integer
    Dim CLMPD As Boolean
    Dim vu As Boolean 'valid user input
    'capitalize Docket number and move into one variable
    vu = ValidUserInput
    If vu Then
        If DocketVarified = False Then
            DocketNum = UCase(tbDN1.Text & "-" & tbDN2.Text & "-" & tbDN3.Text)
            DocketVarified = True
            tbDN1.Text = ""
            tbDN2.Text = ""
            tbDN3.Text = ""
            MsgBox "Please Re-enter the Docket number for verification."
            tbDN1.SetFocus
            Exit Sub
        Else
            'if docketvarified then check if correct.
            If DocketNum <> UCase(tbDN1.Text & "-" & tbDN2.Text & "-" & tbDN3.Text) Then
                'if docket number is not the same reset script.
                MsgBox "The Docket Numbers you entered did not match."
                DocketVarified = False
                tbDN1.Text = ""
                tbDN2.Text = ""
                tbDN3.Text = ""
                Exit Sub
            End
            End If
            DocketVarified = False
        End If
        If vu Then
            Me.Hide
            'COMPASS Processing
            If AddBRToCOMPASS Then
                If TD0TOKOnOtherBankruptcies() Then 'if the bankruptcy should be added on COMPASS
                    ITX1JGetAttorneyInfo
                    ITX1IGetCourtInfo
                    If ITS26GetDebtIncurredDate() = False Then 'if there is no balance then don't add bankruptcy
                        If ATD0TCreateBankruptcyRecord() Then 'only finish processing if the bankruptcy was added
                            BRRecordCreated = True
                            ITS2OGetPayOffAmt
                            'if a chapter 13 bankruptcy was added the create a POC form
                            If cbChapter.Value = "Chapter 13" Then
                                GeneratePOC "COMPASS"
                                If lATD22AllLoans(tbSSN.Text, "B101A", BankruptcyStr() & " Proof of Claim was generated", "BKRUPNOTF", UID) = False Then
                                    MsgBox "Access to the B101A ARC is needed.  Please contact Systems Support."
                                    End
                                End If
                            Else
                                'Put a record in the database to be printed later.
                                SP.Common.SQLNonQuery "INSERT INTO BKNO_DAT_ChangeOfAddress (DocketNum, SSN, Name) VALUES ('" & DocketNum & "', '" & tbSSN.Text & "', '" & tbFN.Text & " " & tbLN.Text & "')"
                                If lATD22AllLoans(tbSSN.Text, "B101A", BankruptcyStr(), "BKRUPNOTF", UID) = False Then
                                    MsgBox "Access to the B101A ARC is needed.  Please contact Systems Support."
                                    End
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            If AddBRToOneLINK Then
                LC10GetPayOffAmt
                If LC12OKOnOtherBankruptcies() Then
                    LC41CreateRefundRequest
                    LC67CheckLegalStuff
                    LC34AddBankruptcyRecord
                    BRRecordCreated = True
                    LC12IncludeAdditionalInfo
                    If cbChapter.Value = "Chapter 13" Then
                        If GeneratePOC("OneLINK") Then
                            SP.Common.AddLP50 tbSSN.Text, "B101A", "BKRUPNOTF", "FO", "14", BankruptcyStr & " Proof of Claim was generated"
                        Else
                            SP.Common.AddLP50 tbSSN.Text, "B101A", "BKRUPNOTF", "FO", "14", BankruptcyStr
                        End If
                    Else
                        SP.Common.AddLP50 tbSSN.Text, "B101A", "BKRUPNOTF", "FO", "14", BankruptcyStr
                    End If
                End If
            End If
            CLMPD = False
            x = 0
            While x < UBound(ClaimPaidDT)
            If CDate(ClaimPaidDT(x)) > Date Then
                CLMPD = True
            End If
            x = x + 1
            Wend
            If CLMPD Then
                Session.TransmitTerminalKey rcIBMClearKey
                Session.WaitForDisplayString " ", "0:0:30", 1, 2
                Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                SP.Common.AddLP9O tbSSN.Text, "BQCTHEBK", , "Review bankruptcy record."
            End If
        End If
        'if a BKRUP Record was added on either or both systems
        If BRRecordCreated Then
            SP.Common.AddLP9O tbSSN.Text, "XPIREMPN", , "[" & tbFD.Text & "] Borrower filed " & cbChapter.Value & " bankruptcy, Case # " & DocketNum & ", " & tbFD.Text & ", " & CStr(Date) & ".  Review for MPN Expiration."
            PO6LCheck
        End If
        Me.Hide
        RescreenForm
        Me.Show
    Else
        tbFD.SetFocus
        Exit Sub
    End If
End Sub

'this function does some addtional processing on PO6L
Private Function PO6LCheck()
    FastPath "PO6LI" & tbSSN.Text
    Dim row As Integer
    row = 14
    'if the query info is not found then exit the function
    If Check4Text(1, 75, "POX6M") Then Exit Function
    'selection screen is encountered
    If Check4Text(1, 75, "POX6N") Then
        PutText 21, 14, "01", "Enter"
    End If
    While Check4Text(22, 2, "90007 NO MORE DATA TO DISPLAY") = False
        While Check4Text(row, 65, "     ") = False
            If CDate(GetText(row, 2, 10)) >= Date Then
                If IsNumeric(GetText(row, 29, 10)) Then
                    If Val(GetText(row, 29, 10)) < Val(GetText(row, 15, 10)) Then
                        SP.Common.AddLP9O tbSSN.Text, "XCNCLDIS", , "[" & tbFD.Text & "] Borrower filed " & cbChapter.Value & " bankruptcy, Case # " & DocketNum & ", " & tbFD.Text & ", " & CStr(Date) & ".  Review for Disbursement Cancellation."
                        Exit Function
                    End If
                Else
                    SP.Common.AddLP9O tbSSN.Text, "XCNCLDIS", , "[" & tbFD.Text & "] Borrower filed " & cbChapter.Value & " bankruptcy, Case # " & DocketNum & ", " & tbFD.Text & ", " & CStr(Date) & ".  Review for Disbursement Cancellation."
                    Exit Function
                End If
            End If
            row = row + 1
        Wend
        row = 14
        Hit "F8"
    Wend
End Function

'this function checks for possible AWG problems with the bankruptcy
Private Function LC67CheckLegalStuff()
    FastPath "LC67I" & tbSSN.Text
    If Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then Exit Function
    'if the script encounters a selection screen
    If Check4Text(1, 59, "LEGAL ACTION SELECTION") Then
        PutText 21, 13, "01", "Enter"
    End If
    'cycle through all of the records for applicable criteria
    While Check4Text(21, 3, "46004 NO MORE DATA TO DISPLAY") = False And Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
        If (Check4Text(1, 57, "LEGAL ACTION AWG DISPLAY") And Check4Text(8, 19, "     ")) Or Check4Text(2, 21, "EX EXECUTION") Then
            SP.Common.AddLP9O tbSSN.Text, "LBANKRUP", , "Borrower filed " & cbChapter.Value & " bankruptcy on " & tbFD.Text & " date.  Review account for pending garnishment activity."
            Exit Function
        End If
        Hit "F8"
    Wend
End Function

'this function gathers needed information from LC41 and creates a Refund Request
Private Function LC41CreateRefundRequest()
    Dim Total As Double
    Dim row As Integer
    Dim counter As Integer
    counter = 1
    'figure total amount of refund
    FastPath "LC41I" & tbSSN.Text
    PutText 7, 36, "X", "Enter" 'select all
    'if target screen is found (Only one payment)
    If Check4Text(1, 59, "PAYMENT RECORD DISPLAY") Then
        MsgBox "The script is unable to create the Refund Request automatically.  Please access LC41 and create the Refund Request manually after the script has completed."
        Exit Function
    End If
    If Check4Text(21, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then Exit Function
    row = 7
    While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
        'if the right conditions are found then add the amount applied to the total variable
        If (Check4Text(row, 34, "SO") Or Check4Text(row, 34, "FO") Or _
           Check4Text(row, 34, "GP") Or Check4Text(row, 34, "BR") Or _
           Check4Text(row, 34, "EP")) And (CDate(Format(GetText(row, 5, 8), "##/##/####")) > CDate(tbFD.Text)) And _
           (Check4Text(row, 39, "    ")) Then
                Total = Total + CDbl(Replace(GetText(row, 15, 9), ",", ""))
        End If
        row = row + 1
        If Check4Text(row, 5, "      ") Then
            Hit "F8"
            row = 7
        End If
    Wend
    If Total > 0 Then
        'add records to the database to be merged
        FastPath "LC41I" & tbSSN.Text
        PutText 7, 36, "X", "Enter" 'select all
        row = 7
        While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            'if the right conditions are found then add the amount applied to the total variable
            If (Check4Text(row, 34, "SO") Or Check4Text(row, 34, "FO") Or _
               Check4Text(row, 34, "GP") Or Check4Text(row, 34, "BR") Or _
               Check4Text(row, 34, "EP")) And (CDate(Format(GetText(row, 5, 8), "##/##/####")) > CDate(tbFD.Text)) And _
               (Check4Text(row, 39, "    ")) Then
                    'write data to table
                    SP.Common.SQLNonQuery "INSERT INTO BKNO_DAT_RefundRequest (RecordNumber, Name, SSN, Address1, Address2, City, State, Zip, Refund, TranType, DateApplied, Amount) " _
                        & "VALUES ('" & counter & "', '" & tbFN.Text & " " & tbLN.Text & "', '" & tbSSN.Text & "', '" & Addr1 & "', '" & Addr2 & "', '" & City & "', '" & State & "', '" & ZIP & "', '" _
                        & Format(Total, "###,##0.00") & "', '" & GetText(row, 34, 2) & "', '" & Format(GetText(row, 25, 8), "##/##/####") & "', '" & GetText(row, 15, 9) & "')"
                    counter = counter + 1
            End If
            row = row + 1
            If Check4Text(row, 5, "      ") Then
                Hit "F8"
                row = 7
            End If
        Wend
        SP.Common.AddLP50 tbSSN.Text, "BRRFD", "BKRUPNOTF", "FO", "39"
    End If
    TotalRefund = Total
End Function

'This function adds additional info to the bankruptcy record
Private Function LC12IncludeAdditionalInfo()
    Dim row As Integer
    FastPath "LC12I" & tbSSN.Text
    'if a selection screen is found
    If Check4Text(1, 65, "BANKRUPTCY RECAP") Then
        row = 7
        While Check4Text(row, 51, CStr(Format(CDate(tbFD.Text), "MMDDYYYY"))) = False
            row = row + 1
            If row = 19 Then
                SP.Q.Hit ("F8")
                row = 7
                If SP.Q.Check4Text(22, 3, "46004") Then
                    MsgBox "Error: Date Filed not Found on LC12.  Ending Script."
                    End
                End If
            End If
        Wend
        PutText 21, 13, GetText(row, 5, 2), "Enter"
    End If
    Hit "F11" 'hit the LCB1 hot key
    If Check4Text(1, 61, "BANKRUPTCY CASE INFO") Then 'if the record doesn't have a LCB1 record then add one
        PutText 1, 7, "A", "Enter" 'change to add mode
        If AFN <> "" Then 'if attorney info was gathered on COMPASS
            PutText 7, 17, AFN & " " & ALN & " " & APhone
            PutText 8, 17, AAddr1
            PutText 9, 17, AAddr2
            PutText 10, 17, ACity
            PutText 11, 17, AState
            PutText 11, 48, AZip
            PutText 13, 17, CourtState & "**" & CourtCity
            PutText 14, 17, tbTN.Text & " " & tbTP.Text, "Enter"
        Else
            If vbYes = MsgBox("Would you like to add the attorney information to LCB1?", vbYesNo, "Add Info To LCB1?") Then
                MsgBox "Press <Insert> when done adding the applicable information."
                SP.Q.PauseForInsert
            End If
        End If
    End If
End Function

'This function adds the Bankruptcy record to OneLINK
Private Function LC34AddBankruptcyRecord()
    Dim row As Integer
    Dim counter As Integer
    Dim found As Boolean
    Dim x As Integer
    FastPath "LC34C" & tbSSN.Text & ";04;"
    If Check4Text(22, 3, "40321") Then  'Garn Withdrawn Letter Ind Must Be Updated
        MsgBox "The letter indicator needs to be updated. Contact Systems Support, and hit Insert when the situation has been resolved.", vbOKOnly, "Bankruptcy Notification"
        PauseForInsert
    End If
    'Bankruptcy type
    If cbChapter.Value = "Chapter 13" Then 'chapter 13
        PutText 3, 8, "04"
    Else 'chapter 7
        PutText 3, 8, "01"
    End If
    PutText 3, 21, DocketNum 'case number
    If tbDAN.Text <> "" Then
        PutText 3, 51, Format(CDate(tbDAN.Text), "MMDDYYYY") 'date notified
    End If
    PutText 4, 14, "01" 'status
    If tbFD.Text <> "" Then
        PutText 4, 17, Format(CDate(tbFD.Text), "MMDDYYYY") 'date filed
    End If
    If tbMOC.Text <> "" Then
        PutText 4, 31, Format(CDate(tbMOC.Text), "MMDDYYYY") 'MOC Issuance Date
    End If
    If tbConDate.Text <> "" Then
        PutText 4, 72, Format(CDate(tbConDate.Text), "MMDDYYYY") 'Confirmation date Date
    End If
    If cbChapter.Value = "Chapter 13" Then 'chapter 13
        If tbPF.TextLength <> 0 Then
            PutText 5, 12, Format(CDate(tbPF.Text), "MMDDYYYY") 'POC Filed date
        Else
            PutText 5, 12, Format(Date, "MMDDYYYY") 'POC Date = current date
        End If
        If tbPA.TextLength > 0 Then 'if user gave a POC amount
            PutText 5, 31, Format(tbPA.Text, "0.00") 'amount
            If Len(Format(tbPA.Text, "0.00")) < 12 Then Hit "End" 'blank out remaining chars in field
        Else 'if user left POC amount blank
            PutText 5, 31, Format(CDbl(OneLINKPayOffAmt) + TotalRefund, "0.00") 'amount
            If Len(Format(CDbl(OneLINKPayOffAmt) + TotalRefund, "0.00")) < 12 Then Hit "End" 'blank out remaining chars in field
        End If
    End If
    'POC Deadline
    If tbPD.TextLength <> 0 Then
        PutText 8, 15, Format(CDate(tbPD.Text), "MMDDYYYY")
    End If
    'add applicable claims gathered from LC05
    row = 11
    While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
        While counter < UBound(ApplicableClaims) And found = False
            If Check4Text(row, 7, ApplicableClaims(counter)) And InStr(1, ApplicableClaims(counter), "*") < 1 Then PutText row, 3, "A"
            counter = counter + 1
        Wend
        found = False
        counter = 0
        row = row + 1
        If Check4Text(row, 7, "    ") Then
            Hit "F8"
            row = 11
        End If
    Wend
    Hit "Enter" 'post changes
End Function

'this function checks for other open bankruptcies and decides what yo do accordingly
Private Function LC12OKOnOtherBankruptcies() As Boolean
    Dim x As Integer '<4>
    Dim FindClm As Boolean '<4>
    Dim FindClmCnt As Integer '<4> count the number of claims found
    Dim FindClmNewDocket As Boolean  '<4> this is true if a claim was found with a different docket number
    FastPath "LC12I" & tbSSN.Text
    'if no record is found then the bankruptcy may be added to OneLINK
    If Check4Text(1, 61, "          BANKRUPTCY") Then
        LC12OKOnOtherBankruptcies = True
    Else 'if record(s) are found
        If Check4Text(1, 65, "BANKRUPTCY RECAP") Then 'if a selection screen is found
            PutText 21, 13, "01", "Enter"
        End If
        LC12OKOnOtherBankruptcies = False
        'cycle through each loan and look for the appropriate conditions
        FindClmCnt = 0
        FindClmNewDocket = False
        While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            For x = 0 To UBound(ApplicableClaims) - 1
                If SP.Q.Check4Text(3, 75, ApplicableClaims(x)) Then
                    FindClmCnt = FindClmCnt + 1
                    If Check4Text(6, 10, "01") Then
                        If Check4Text(7, 18, DocketNum) Then
                            'same claim # and same docket #
                            ApplicableClaims(x) = "*" & ApplicableClaims(x) 'add a star to the claims that should not be added
                        Else
                            'same claim # but different docket #
                            FindClmNewDocket = True
                            ApplicableClaims(x) = "*" & ApplicableClaims(x) 'add a star to the claims that should not be added
                        End If
                        Exit For
                    Else
                        LC12OKOnOtherBankruptcies = True
                    End If
                    Exit For
                End If
            Next x
           Hit "F8"
        Wend
        If FindClmNewDocket Then
            SP.Common.AddLP9O tbSSN.Text, "BNKNOTFY", , BankruptcyStrLP9O()
        End If
        If LC12OKOnOtherBankruptcies Then 'if a record with an 06 was found and not the others then check the REPURCH1,REPURCH2 and REPURCHS queues and cancel any tasks for the borrower.
            CancelREPURCHXTasks
        Else 'if the script exits the function this way and LC12OKOnOtherBankruptcies = false then a record with the necessary criteria is wasn't found and a bankruptcy may be added
             If FindClmCnt < UBound(ApplicableClaims) Then
                'not all cliams were found
                LC12OKOnOtherBankruptcies = True
             Else
                'all claims were found
                Exit Function
             End If
        End If
    End If
End Function

'this function cancels any queue tasks in the REPURCH1, REPURCH2, and REPURCHS queues
Private Function CancelREPURCHXTasks()
    Dim row As Integer
    Dim GotOne As Boolean 'This is true when a queue was found to cancel.
    GotOne = False
    FastPath "LP8YCALL;REPURCH1;;" & tbSSN.Text & ";;A"
    If Check4Text(22, 3, "04011") = False Then 'if the user has access to cancel queue tasks
        If KillQueueTask Then GotOne = True
        FastPath "LP8YCALL;REPURCH1;;" & tbSSN.Text & ";;W"
        If KillQueueTask Then GotOne = True
        FastPath "LP8YCALL;REPURCH2;;" & tbSSN.Text & ";;A"
        If KillQueueTask Then GotOne = True
        FastPath "LP8YCALL;REPURCH2;;" & tbSSN.Text & ";;W"
        If KillQueueTask Then GotOne = True
        FastPath "LP8YCALL;REPURCHS;;" & tbSSN.Text & ";;A"
        If KillQueueTask Then GotOne = True
        FastPath "LP8YCALL;REPURCHS;;" & tbSSN.Text & ";;W"
        If KillQueueTask Then GotOne = True
    Else 'if the user doesn't have access to cancel queue tasks
        MsgBox "You don't have access to cancel the appropriate queue tasks. Please have your supervisor cancel them when you are finished."
    End If
    If GotOne Then
        'Create a queue task in the BTOBEREP if a repurchase queue was canceled
        SP.Common.AddLP9O tbSSN.Text, "BTOBEREP", , "New BK filing:  Review account to determine if bankruptcy repurchase may be cancelled."
    End If
End Function

Function KillQueueTask() As Boolean
    Dim row As Integer
    If Check4Text(1, 64, "QUEUE TASK DETAIL") Then
        row = 7
        Do
        If SP.Q.GetText(row, 33, 1) <> "" Then
            PutText row, 33, "X", "Enter"
        Else
            row = 6
            SP.Q.Hit "F8"
            If SP.Q.Check4Text(22, 3, "46004") Then
                Exit Do
            End If
        End If
        row = row + 1
        Loop
        Hit "F6"
        KillQueueTask = True
    Else
        KillQueueTask = False
    End If
End Function

'this function retrieves the payoff amount from LC10
Private Function LC10GetPayOffAmt()
    FastPath "LC10I" & tbSSN.Text
    PutText 9, 20, Format(CDate(tbFD.Text), "MMDDYYYY"), "Enter"
    OneLINKPayOffAmt = CDbl(GetText(18, 36, 10)) - CDbl(GetText(17, 36, 10))
End Function

'this function creates a POC form for either system
Private Function GeneratePOC(System As String) As Boolean
    GeneratePOC = True
    Dim HasDt As Boolean 'True if Borrower has a claim paid date less than today
    HasDt = False
    Dim x As Integer
    x = 0
    If System = "OneLINK" Then
        While x < UBound(ClaimPaidDT)
        If CDate(ClaimPaidDT(x)) < Date Then
            HasDt = True
        End If
        x = x + 1
        Wend
        If HasDt = False Then
            GeneratePOC = False
            Exit Function
        End If
    End If

    If System = "COMPASS" Then
        'add COMPASS ARC comment
        If lATD22AllLoans(tbSSN.Text, "BXPOC", "POC was generated for " & COMPASSPayOffAmt, "BKRUPNOTF", UID) = False Then
            MsgBox "You need access to the ""BXPOC"" ARC.  Please contact Systems Support."
            End
        End If
        If SP.Common.AddLP9O(tbSSN.Text, "BKRVWPOC", CStr(Date + 30), "POC was generated for " & COMPASSPayOffAmt) = False Then
            MsgBox "There was a problem while trying to add a task to ""BKRVWPOC"".  Please contact Systems Support."
            End
        End If
        SP.Common.SQLNonQuery "INSERT INTO BKNO_DAT_ProofOfClaim (Debtor, Debtor2, Court, State, CaseNumber, SendTo, AccountNumber, ClaimAmount) " _
                           & "VALUES ('" & tbFN.Text & " " & tbLN.Text & "', '" & Debt2.Text & "', '" & Court.Text & "', '" & txtCourtState.Text & "', '" & DocketNum & "','UHEAA - Account Services', '" & AccNum & "','" & CStr(CCur(COMPASSPayOffAmt)) & "')"
    Else
        'if a POC amount was provided by the user then use it otherwise use amount gathered from LC10
        If tbPA.TextLength = 0 Then
            If SP.Common.AddLP50(tbSSN.Text, "BXPOC", "BKRUPNOTF", "FO", "13", "POC was generated for " & CDbl(OneLINKPayOffAmt) + TotalRefund) = False Then
                MsgBox "There was a problem while trying to add a comment to ""BXPOC"".  Please contact Systems Support."
                End
            End If
            If SP.Common.AddLP9O(tbSSN.Text, "BKRVWPOC", CStr(Date + 30), "POC was generated for " & CDbl(OneLINKPayOffAmt) + TotalRefund) = False Then
                MsgBox "There was a problem while trying to add a task to ""BKRVWPOC"".  Please contact Systems Support."
                End
            End If
            SP.Common.SQLNonQuery "INSERT INTO BKNO_DAT_ProofOfClaim (Debtor, Debtor2, Court, State, CaseNumber,SendTo, AccountNumber, ClaimAmount) " _
                & "VALUES ('" & tbFN.Text & " " & tbLN.Text & "', '" & Debt2.Text & "', '" & Court.Text & "', '" & txtCourtState.Text & "', '" & DocketNum & "','UHEAA - Post Claim Services', '" & AccNum & "','" & CStr(CCur(OneLINKPayOffAmt) + TotalRefund) & "')"
        Else
            If SP.Common.AddLP50(tbSSN.Text, "BXPOC", "BKRUPNOTF", "FO", "13", "POC was generated for " & tbPA.Text) = False Then
                MsgBox "There was a problem while trying to add a comment to ""BXPOC"".  Please contact Systems Support."
                End
            End If
            If SP.Common.AddLP9O(tbSSN.Text, "BKRVWPOC", CStr(Date + 30), "POC was generated for " & tbPA.Text) = False Then
                MsgBox "There was a problem while trying to add a task to ""BKRVWPOC"".  Please contact Systems Support."
                End
            End If
            SP.Common.SQLNonQuery "INSERT INTO BKNO_DAT_ProofOfClaim (Debtor, Debtor2, Court, State, CaseNumber,SendTo, AccountNumber, ClaimAmount) " _
                & "VALUES ('" & tbFN.Text & " " & tbLN.Text & "', '" & Debt2.Text & "', '" & Court.Text & "', '" & txtCourtState.Text & "', '" & DocketNum & "','UHEAA - Post Claim Services', '" & AccNum & "','" & tbPA.Text & "')"
        End If
    End If
End Function

'this function checks for JD records on LC67 and returns it's Date Entered if one is found
Private Function LC67JDRecordCheck() As String
    FastPath "LC67I" & tbSSN.Text & ";JD"
    If Check4Text(1, 65, "JUDGMENT DISPLAY") Then
        LC67JDRecordCheck = Format(GetText(7, 26, 8), "##/##/####")
    Else
        LC67JDRecordCheck = ""
    End If
End Function

'this function figures out the total payoff amount for COMPASS loans
Private Function ITS2OGetPayOffAmt()
    Dim row As Integer
    FastPath "TX3Z/ITS2O" & tbSSN.Text
    PutText 7, 26, Format(CDate(tbFD.Text), "MMDDYY")
    PutText 9, 54, "Y"
    PutText 9, 16, "X"
    Hit "Enter"
    Hit "Enter"
    COMPASSPayOffAmt = GetText(12, 29, 10) 'get payoff amount
End Function

'this function adds the bankruptcy record to COMPASS if one hasn't been added today already
Private Function ATD0TCreateBankruptcyRecord() As Boolean
    FastPath "TX3Z/ATD0T" & tbSSN.Text
    'if a bankruptcy was added today already
    If Check4Text(23, 2, "01508 RECORD ALREADY ADDED TODAY") Then
        ATD0TCreateBankruptcyRecord = False
        If lATD22AllLoans(tbSSN.Text, "B101A", BankruptcyStr & " BK record already added today", "BKRUPNOTF", UID) = False Then
            MsgBox "Access to the B101A ARC is needed.  Please contact Systems Support."
            End
        End If
    Else 'if the script can add the bankruptcy then populate the necessary information
        ATD0TCreateBankruptcyRecord = True
        PutText 6, 12, ARefID 'attorney Ref ID
        PutText 12, 15, Format(CDate(tbFD.Text), "MMDDYY") 'filed date
        PutText 12, 31, CourtID 'court ID
        'chapter
        If cbChapter.Value = "Chapter 13" Then
            PutText 13, 18, "13"
        Else
            PutText 13, 18, "07"
        End If
        SP.Q.PutText 17, 41, Format(DateAdd("m", 4, CDate(tbFD.Text)), "MMDDYY")
        SP.Q.PutText 17, 16, "4"
        PutText 13, 29, DocketNum 'docket number
        PutText 13, 48, CourtCity 'city of the court
        PutText 13, 76, CourtState 'state of the court
        PutText 16, 32, Format(CDate(tbDAN.Text), "MMDDYY") 'date corr received
        PutText 22, 48, "N", True
        Hit "Enter"
        If SP.Q.Check4Text(23, 2, "01004") = False Then
            MsgBox "There was an error in adding the bankruptcy record. Ending script. "
            End
        End If
    End If
End Function

'this function figures out as of what date the debt was incurred
Private Function ITS26GetDebtIncurredDate() As Boolean
    Dim row As Integer
    FastPath "TX3Z/ITS26" & tbSSN.Text
    If Check4Text(1, 72, "TSX28") Then 'if selection screen is found then
        row = 8
        'find a loan with a balance greater than zero
        While Val(GetText(row, 59, 10)) = 0
            row = row + 1
            If Check4Text(row, 59, "          ") Then
                row = 8
                Hit "F8"
                If Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") Then
                    ITS26GetDebtIncurredDate = True
                    Exit Function
                End If
            End If
        Wend
        'get disbursement date for the loan with a balance greater than 0
        COMPASSDateIncurred = GetText(row, 5, 8)
        'search for oldest loan with a balance
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            'if the disb date is earlier and it has a balance gather it as the date that debt was incurred
            If CDate(GetText(row, 5, 8)) < CDate(COMPASSDateIncurred) And Val(GetText(row, 59, 10)) <> 0 Then
                COMPASSDateIncurred = GetText(row, 5, 8)
            End If
            row = row + 1
            If Check4Text(row, 59, "          ") Then
                row = 8
                Hit "F8"
            End If
        Wend
    Else
        COMPASSDateIncurred = GetText(6, 18, 8)
    End If
End Function

'This function figures out and retrieves if needed the court information
Private Function ITX1IGetCourtInfo()
    If cbCourt.Value = "Utah" Then
        CourtState = "UT"
        CourtCity = "Salt Lake City"
        CourtID = "I0000001"
    Else 'go to TX1I
        'get to the correct institution record
        FastPath "TX3Z/ITX1I;;;;;;"
        MsgBox "Please take a moment and select or add the court to the system.  Press <Insert> when you are done."
        While Check4Text(1, 73, "TXX05") = False
            PauseForInsert
            If Check4Text(1, 73, "TXX05") = False Then
                MsgBox "You aren't on the appropriate screen.  Please try again."
            End If
        Wend
        'gather the appropriate information
        CourtState = GetText(14, 53, 2)
        CourtCity = GetText(14, 13, 20)
        CourtID = GetText(5, 19, 8)
    End If
End Function

'this function gets the attorney info
Private Function ITX1JGetAttorneyInfo()
    FastPath "TX3Z/ITX1JR"
    'get the user to select the attorney's reference demographic record
    MsgBox "Please take a moment and select or add the attorney reference record.  Press <Insert> to continue."
    While Check4Text(1, 71, "TXX1R") = False Or Check4Text(8, 15, "15") = False
        PauseForInsert
        If Check4Text(1, 71, "TXX1R") = False Or Check4Text(8, 15, "15") = False Then
            MsgBox "You aren't on the appropriate screen or haven't selected an appropriate reference.  Please try again."
        End If
    Wend
    'retrieve attorney's demographic info
    ARefID = Replace(GetText(3, 12, 11), " ", "")
    AFN = GetText(4, 34, 13)
    ALN = GetText(4, 6, 24)
    AAddr1 = GetText(11, 10, 30)
    AAddr2 = GetText(12, 10, 30)
    ACity = GetText(14, 8, 22)
    AState = GetText(14, 32, 2)
    AZip = GetText(14, 40, 18)
    APhone = GetText(17, 14, 3) & GetText(17, 23, 3) & GetText(17, 31, 4)
End Function

'This function checks TD0T for other bankruptcies, if one is already open it returns false
Private Function TD0TOKOnOtherBankruptcies() As Boolean
    Dim row As Integer
    Dim BRFound As Boolean
    FastPath "TX3Z/ITD0T" & tbSSN.Text
    'check for a verified or alleged bankruptcy
    If Check4Text(1, 72, "TDX2J") Then 'if a selection screen is found
        row = 8
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False And BRFound = False
            If Check4Text(row, 25, "VERIFIED") Or Check4Text(row, 25, "ALLEGED") Then
                'select the found verified or alleged bankruptcy
                PutText 21, 18, GetText(row, 5, 2), "Enter"
                BRFound = True
            Else
                row = row + 1
                'if the spot is blank then F8 to the next screen
                If Check4Text(row, 25, "      ") Then
                    row = 8
                    Hit "F8"
                End If
            End If
        Wend
        If BRFound = False Then 'add bankruptcy
            TD0TOKOnOtherBankruptcies = True
            Exit Function
        End If
    Else
        If Check4Text(5, 15, "VERIFIED") = False And Check4Text(5, 15, "ALLEGED") = False Then
            TD0TOKOnOtherBankruptcies = True 'add bankruptcy
            Exit Function
        End If
    End If
    'if the script makes it this far then false will be returned and the bankruptcy will not be added to COMPASS
    'check docket number
    If Check4Text(13, 29, DocketNum) = False Then 'if the docket number = user input
        If lATD22AllLoans(tbSSN.Text, "B101A", BankruptcyStr & " -- BK record already exists -- ", "BKRUPNOTF", UID) = False Then
            MsgBox "Access to B101A ARC needed.  Please contact Systems Support."
            End
        End If
    Else
        MsgBox "That bankruptcy has already been added to COMPASS."
    End If
End Function

'this function creates a comma delimited string of all the bankruptcy information and descriptions
Private Function BankruptcyStr() As String
    BankruptcyStr = "Trustee Name:" & tbTN.Text & ", Trustee Addr:" & tbTA.Text & ", Trustee Phone:" & tbTP.Text & ", POC Filed:" & tbPF.Text & ", POC Amt:" & _
                    tbPA.Text & ", Inst ID:" & ", Filed Date:" & tbFD.Text & ", Docket Num:" & DocketNum & ", Date Agency Notf:" & _
                    tbDAN.Text & ", Chapter:" & cbChapter.Text & ", MOC:" & tbMOC.Text & ", POC Deadline:" & tbPD.Text & ", Adversary Proceeding:" & _
                    cbAP.Value & ", Court:" & cbCourt.Value
End Function

'this function creates a comma delimited string of all the bankruptcy information
Private Function BankruptcyStrLP9O() As String
    BankruptcyStrLP9O = tbTN.Text & "," & tbTA.Text & "," & tbTP.Text & "," & tbPF.Text & "," & _
                    tbPA.Text & "," & "," & tbFD.Text & "," & DocketNum & "," & _
                    tbDAN.Text & "," & cbChapter.Text & "," & tbMOC.Text & "," & tbPD.Text & "," & _
                    cbAP.Value & "," & cbCourt.Value
End Function

'this function checks to be sure that all the user input is valid
Function ValidUserInput() As Boolean
    Dim FD As String
    Dim DAN As String
    Dim MOC As String
    Dim PD As String
    Dim PF As String
    Dim ConDate As String '<4>
    ValidUserInput = True 'default is True
    'check if a chapter has been selected
    If cbChapter.ListIndex < 0 Then
        MsgBox "You must select a chapter."
        ValidUserInput = False
        Exit Function
    End If
    FD = tbFD.Text
    DAN = tbDAN.Text
    MOC = tbMOC.Text
    'check if all required valid dates are valid
    If TheDateFormat(FD) = False Or TheDateFormat(DAN) = False Then
            MsgBox "The Filed Date and Date Agency Notified fields all require valid dates."
            ValidUserInput = False
            Exit Function
    End If
    If TheDateFormat(MOC) = False And MOC <> "" Then
            MsgBox "The MOC Date field requires a valid date if it is populated."
            ValidUserInput = False
            Exit Function
    End If
    
    tbFD.Text = FD
    tbDAN.Text = DAN
    tbMOC.Text = MOC
    PF = tbPF.Text
    'if chapter 13 is selected
    If cbChapter.Value = "Chapter 13" Then
        PD = tbPD.Text
        If TheDateFormat(PD) = False Then
            MsgBox "If chapter 13 is selected then the POC Deadline field is required."
            ValidUserInput = False
            Exit Function
        End If
        tbPD.Text = PD
    End If
    If cbChapter.Value = "Chapter 7" And tbPD.Text <> "" Then
        PD = tbPD.Text
        If TheDateFormat(PD) = False Then
            MsgBox "The POC Deadline field is invalid."
            ValidUserInput = False
            Exit Function
        End If
        tbPD.Text = PD
    End If

    'if confirmation date has data be sure it is valid data
    If tbConDate.TextLength <> 0 Then
        ConDate = tbConDate.Text
        If TheDateFormat(ConDate) = False Then
            MsgBox "The Confermation Date field requires a valid date."
            ValidUserInput = False
            Exit Function
        End If
        tbConDate.Text = ConDate
    End If
    
    'if POC Filed has data be sure it is valid data
    If tbPF.TextLength <> 0 Then
        If TheDateFormat(PF) = False Then
            MsgBox "The POC Filed field requires a valid date."
            ValidUserInput = False
            Exit Function
        End If
        tbPF.Text = PF
    End If
    'be sure that all other required fields have text in them
    If tbDN1.TextLength <> 2 Or tbDN2.TextLength <> 5 Or tbDN3.TextLength < 2 Or _
       tbTN.TextLength = 0 Or tbTA.TextLength = 0 Or tbTP.TextLength = 0 Then
            MsgBox "A full docket number, a trustee name and trustee demographic information is required to continue."
            ValidUserInput = False
            Exit Function
    End If
    If tbPA.TextLength > 0 Then
        If IsNumeric(tbPA.Text) Then
            If InStr(1, tbPA.Text, ".") <> (tbPA.TextLength - 2) Then
                MsgBox "The third from the last character must be a "".""."
                ValidUserInput = False
                Exit Function
            End If
        Else
            MsgBox "If a POC Amount is provided then it must be numeric."
            ValidUserInput = False
            Exit Function
        End If
    End If
End Function

'format date strings
Function TheDateFormat(ByRef DateString As String, Optional ReturnFormat As Integer = 10) As Boolean
    Dim FormatConst As String
    TheDateFormat = True
    'determine return format to use based on constant supplied
    If ReturnFormat = 6 Then
        FormatConst = "MMDDYY"
    ElseIf ReturnFormat = 8 Then
        FormatConst = "MMDDYYYY"
    Else
        FormatConst = "MM/DD/YYYY"
    End If
    
    'return false if the date string has a period in it (the string will be interpreted incorrectly by VBA)
    If InStrRev(DateString, ".") > 0 Then
        TheDateFormat = False
    'format the date if it is a recognizable date format
    ElseIf IsDate(DateString) Then
        DateString = Format(DateValue(DateString), FormatConst)
    'format the date if it is in MMDDYY format
    ElseIf Len(DateString) = 6 And IsDate(Format(DateString, "##/##/##")) = True Then
        DateString = Format(DateValue(Format(DateString, "##/##/##")), FormatConst)
    'format the date if it is in MMDDYYYY format
    ElseIf Len(DateString) = 8 And IsDate(Format(DateString, "##/##/####")) = True Then
        DateString = Format(DateValue(Format(DateString, "##/##/####")), FormatConst)
    'return false
    Else
        TheDateFormat = False
    End If
End Function

Private Sub ProcessClick()
    If LP22TranslateAccNum() = False Then Exit Sub 'if false is returned then the user didn't enter a valid SSN or Account number
    'decide if the bankruptcy needs to be added to either OneLINK or COMPASS
    TS24AddBankruptcyToCOMPASS
    ReDim LC12BankruptcyStatus(1, 0)
    GatherLC12BankruptcyStatus
    LC05AddBankruptcyToOneLINK
    If AddBRToOneLINK = False And AddBRToCOMPASS = False Then 'check for Nelnet forward and then rescreen the form
        If ResponseGiven = False Then LG10CheckForNelnetForward
        ResponseGiven = False
        RescreenForm
    Else 'resize the form and disable applicable boxes

        Me.Height = 501.75
        tbSSN.Enabled = False
        tbFN.Enabled = False
        tbLN.Enabled = False

        btnDone.Enabled = False
        btnProcess.Enabled = False
        btnSendEmails.Enabled = False
        Label1.Enabled = False
        Label2.Enabled = False
        Label3.Enabled = False
        tbFD.SetFocus
    End If
End Sub

Private Sub btnProcess_Click()
    ProcessClick
End Sub

'this function checks to be sure that the bankruptcy paperwork shouldn't be forwarded to Nelnet
Private Function LG10CheckForNelnetForward()
    Dim row As Integer
    Dim SubRow As Integer
    FastPath "LG10I" & tbSSN.Text
    'decide which screen is being displayed
    SubRow = 11
    If Check4Text(1, 53, "LOAN BWR STATUS RECAP SELECT") = False Then  'if a target screen is displayed
        If Check4Text(5, 27, "700121") Then 'only cycle through if the servicer is Nelnet
            While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                If IsNumeric(GetText(SubRow, 47, 9)) Then
                    If Val(GetText(SubRow, 47, 9)) <> 0 Then
                        MsgBox "Please forward the bankruptcy notification paper work to Nelnet."
                        Exit Function
                    End If
                End If
                SubRow = SubRow + 1
                If SubRow = 21 Then
                    SubRow = 10
                    Hit "F8"
                End If
            Wend
        End If
    Else 'if a selection screen is displayed
        row = 7
        While Check4Text(20, 3, "46004 NO MORE DATA TO DISPLAY") = False
            If Check4Text(row, 46, "700121") Then
                PutText 19, 15, GetText(row, 5, 2)
                If Len(GetText(row, 5, 2)) = 1 Then Hit "End"
                Hit "Enter"
                While Check4Text(22, 3, "46004") = False
                    If IsNumeric(GetText(SubRow, 47, 9)) Then
                        If Val(GetText(SubRow, 47, 9)) <> 0 Then
                            MsgBox "Please forward the bankruptcy notification paper work to Nelnet."
                            Exit Function
                        End If
                    End If
                    SubRow = SubRow + 1
                    If SubRow = 21 Then
                        SubRow = 10
                        Hit "F8"
                    End If
                Wend
                Hit "F12"
                SubRow = 11
                row = row + 1
            ElseIf Check4Text(row, 46, "      ") Then
                Hit "F8"
                row = 7
            Else
                row = row + 1
            End If
        Wend
    End If
    'if the script exits through here then the paper work should be shredded
    MsgBox "Please shred the bankruptcy notification paper work."
End Function

'this function decides whether the bankruptcy should be added to OneLINK
Private Function LC05AddBankruptcyToOneLINK()
    Dim Status03Found As Boolean 'tracks if a 03 status is found
    ReDim ClaimPaidDT(0) '<4>
    FastPath "LC05I" & tbSSN.Text
    If Check4Text(1, 60, "DEFAULT/CLAIM DISPLAY") Then
        AddBRToOneLINK = False
    Else
        're dimension array
        ReDim ApplicableClaims(0)
        'if a selection screen is found
        If Check4Text(1, 70, "CLAIM RECAP") Then
            PutText 21, 13, "01", "Enter" 'select first option
        End If
        'cycle through and get all applicable claim numbers
        While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
            If (Check4Text(4, 10, "03") And Check4Text(19, 73, "MMDDCCYY") And (Check4Text(4, 26, "07") = False)) Or (Check4Text(4, 26, "07") And CDate(Format(SP.Q.GetText(4, 73, 8), "##/##/####")) >= Date) Or (Check4Text(4, 26, "07") And CDate(Format(SP.Q.GetText(4, 73, 8), "##/##/####")) <= Date And LC12BankruptcyIs06or01(GetText(21, 11, 4))) Then    '</4>
                'get claim ID if the claim hasn't been assigned to ED and it has a 03 status
                ApplicableClaims(UBound(ApplicableClaims)) = GetText(21, 11, 4)
                ReDim Preserve ApplicableClaims(UBound(ApplicableClaims) + 1) 'add one opening to the list
                'get the claim paid date for any loan that should be added to onelink.
                ClaimPaidDT(UBound(ClaimPaidDT)) = Format(GetText(4, 73, 8), "##/##/####")
                ReDim Preserve ClaimPaidDT(UBound(ClaimPaidDT) + 1)
            ElseIf Check4Text(4, 10, "03") Then
                Status03Found = True
            End If
            Hit "F8" 'page forward to the next claim
        Wend
        If UBound(ApplicableClaims) <> 0 Then
            AddBRToOneLINK = True
        ElseIf Status03Found = True Then
            MsgBox "Please forward the bankruptcy paper work to the U.S. Department of Education."
            AddBRToOneLINK = False
        Else
            AddBRToOneLINK = False
        End If
    End If
End Function

Sub GatherLC12BankruptcyStatus()
    FastPath "LC12I" & tbSSN.Text
    If SP.Q.Check4Text(22, 3, "48012") Then Exit Sub
    If SP.Q.Check4Text(1, 65, "BANKRUPTCY RECAP") Then
        'selection screen
        SP.Q.PutText 21, 13, "01", "ENTER"
    End If
    Do
        LC12BankruptcyStatus(0, UBound(LC12BankruptcyStatus, 2)) = SP.Q.GetText(3, 75, 4) 'claim ID
        LC12BankruptcyStatus(1, UBound(LC12BankruptcyStatus, 2)) = SP.Q.GetText(6, 10, 2) 'Status
        ReDim Preserve LC12BankruptcyStatus(1, UBound(LC12BankruptcyStatus, 2) + 1)
        SP.Q.Hit "F8"
        If SP.Q.Check4Text(22, 3, "46004") Then
            Exit Do
        End If
    Loop
End Sub

Function LC12BankruptcyIs06or01(CLMID As String) As Boolean
    Dim x As Integer
    For x = 0 To UBound(LC12BankruptcyStatus, 2)
        If LC12BankruptcyStatus(0, x) = CLMID Then
            If LC12BankruptcyStatus(1, x) = "06" Or LC12BankruptcyStatus(1, x) = "01" Then
                LC12BankruptcyIs06or01 = True
                Exit Function
            Else
                LC12BankruptcyIs06or01 = False
                Exit Function
            End If
        End If
    Next x
    LC12BankruptcyIs06or01 = False
End Function

'this function decides whether the Bankruptcy should be added to COMPASS
Private Function TS24AddBankruptcyToCOMPASS()
    FastPath "TX3Z/ITS24" & tbSSN.Text
    If Check4Text(1, 76, "TSX25") Then
        AddBRToCOMPASS = True
    Else
        AddBRToCOMPASS = False
    End If
End Function

'this function decides if the SSN/Account Number is valid, translates the Account number into an SSN,
'and gathers the borrowers demographic info
Private Function LP22TranslateAccNum() As Boolean
    LP22TranslateAccNum = True
    'check if the user input for SSN is even close to what it should be
    If IsNumeric(tbSSN.Text) = False Or tbSSN.TextLength < 9 Then
        LP22TranslateAccNum = False
        Exit Function
    End If
    'access LP22 depending on the length of the SSN or Account Num
    If tbSSN.TextLength = 9 Then
        FastPath "LP22I" & tbSSN.Text
    Else
        FastPath "LP22I;;;;;;" & tbSSN.Text
    End If
    If Check4Text(1, 60, "PERSON NAME/ID SEARCH") Then 'if the record wasn't found for some reason
        If vbYes <> MsgBox("You entered " & tbSSN.Text & " as the SSN/Account number.  Please take a moment and compare this SSN/Account number to the information of the bankruptcy notification paper work.  Is it the correct SSN/Account number?", vbYesNo, "Please Confirm!!!!") Then
            MsgBox "Please correct the error."
            LP22TranslateAccNum = False
            Exit Function
        Else
            MsgBox "Please shred the bankruptcy notification paper work."
            RescreenForm 'rescreen form
            LP22TranslateAccNum = False
            Exit Function
        End If
    End If
    'get SSN
    tbSSN.Text = GetText(1, 9, 9)
    'get first name and or last name if not given other wise just capitalize them
    If tbFN.Text = "" Or tbLN.Text = "" Then
        tbFN.Text = GetText(4, 44, 12)
        tbLN.Text = GetText(4, 5, 35)
    Else
        tbFN.Text = UCase(tbFN.Text)
        tbLN.Text = UCase(tbLN.Text)
    End If
    'get demographic info
    Addr1 = GetText(10, 9, 35)
    Addr2 = GetText(11, 9, 35)
    City = GetText(12, 9, 30)
    State = GetText(12, 52, 2)
    ZIP = GetText(12, 60, 9)
    AccNum = Replace(GetText(3, 60, 12), " ", "")
End Function

'this function resets all necessary variables and rescreens the form to the beginning size
Private Function RescreenForm()
    AddBRToCOMPASS = False
    AddBRToOneLINK = False
    DocketNum = ""
    COMPASSDateIncurred = ""
    OneLINKDateIncurred = ""
    COMPASSPayOffAmt = ""
    OneLINKPayOffAm = ""
    Me.Height = 130.5
    ReDim ApplicableClaims(0)
    CourtState = ""
    CourtCity = ""
    CourtID = ""
    AFN = ""
    ALN = ""
    AAddr1 = ""
    AAddr2 = ""
    ACity = ""
    AState = ""
    AZip = ""
    APhone = ""
    tbSSN.Text = ""
    tbFN.Text = ""
    tbLN.Text = ""
    Addr1 = ""
    Addr2 = ""
    City = ""
    State = ""
    ZIP = ""
    tbFD.Text = ""
    tbDN1.Text = ""
    tbDN2.Text = ""
    tbDN3.Text = ""
    tbDAN.Text = ""
    tbMOC.Text = ""
    tbPD.Text = ""
    tbConDate.Text = ""
    tbTN.Text = ""
    tbTA.Text = ""
    tbTP.Text = ""
    tbPF.Text = ""
    tbPA.Text = ""
    tbSSN.Enabled = True
    tbFN.Enabled = True
    tbLN.Enabled = True
    btnDone.Enabled = True
    btnProcess.Enabled = True
    btnSendEmails.Enabled = True
    Label1.Enabled = True
    Label2.Enabled = True
    Label3.Enabled = True
End Function

Private Sub Frame1_Click()

End Sub

Private Sub tbDN1_Change()
    If tbDN1.TextLength = 2 Then tbDN2.SetFocus
End Sub

Private Sub tbDN2_Change()
    If tbDN2.TextLength = 5 Then tbDN3.SetFocus
End Sub

Private Sub tbPF_Change()

End Sub

Private Sub tbSSN_AfterUpdate()
    ProcessClick
End Sub

Private Sub txtCourtState_Change()

End Sub

Private Sub UserForm_Initialize()
    Dim counter As Integer
    'setup lender ID box
    ReDim LenderID(1, counter)
    Open "X:\Sessions\Lists\LenderList.txt" For Input As #8
    While (Not EOF(8))
        If counter = 3 Then
            LenderID(0, counter) = "700126"
            LenderID(1, counter) = "Borrower Services"
        ElseIf counter = 4 Then
            LenderID(0, counter) = "813285"
            LenderID(1, counter) = "Chela"
        ElseIf counter = 12 Then
            LenderID(0, counter) = "700121"
            LenderID(1, counter) = "Nelnet"
        ElseIf counter = 13 Then
            LenderID(0, counter) = "888885"
            LenderID(1, counter) = "Sallie Mae"
        Else
            Input #8, LenderID(0, counter), LenderID(1, counter)
        End If
        counter = counter + 1
        ReDim Preserve LenderID(1, counter)
    Wend
    Close #8
    LenderID(0, counter) = "Other"
    LenderID(1, counter) = "Other"
    Me.Height = 132
    'setup chapter box
    cbChapter.AddItem "Chapter 13"
    cbChapter.AddItem "Chapter 7"
    'set up court box
    cbCourt.AddItem "Utah"
    cbCourt.AddItem "Other"
    cbCourt.ListIndex = 0
    'set up adversary proceeding box
    cbAP.AddItem "No"
    cbAP.AddItem "Yes"
    cbAP.ListIndex = 0
    UID = SP.Common.GetUserID()
    DocketVarified = False
End Sub

Private Sub UserForm_Terminate()
    End
End Sub

'enters an activity record/action request in COMPASS selecting only the loans specified
Function lATD22AllLoans(SSN As String, ARC As String, comment As String, Script As String, UID As String) As Boolean
    Dim row As Integer
    
    lATD22AllLoans = True
    'exit if the comment is too long
    If Len(comment) > 1200 Then
        lATD22AllLoans = False
        Exit Function
    End If
    FastPath "TX3Z/ATD22" & tbSSN.Text
    If Not Check4Text(1, 72, "TDX23") Then
        lATD22AllLoans = False
        Exit Function
    End If
    'find the ARC
    Do
        found = Session.FindText(ARC, 8, 8)
        If found Then Exit Do
        Hit "F8"
        If Check4Text(23, 2, "90007") Then
            lATD22AllLoans = False
            Exit Function
        End If
    Loop
    'select the ARC
    PutText Session.FoundTextRow, Session.FoundTextColumn - 5, "01", "ENTER"
    Do
        PutText 11, 3, "XXXXXXXX"
        If Check4Text(8, 75, "+") Then Hit "F8" Else Exit Do
    Loop
    'blank extra X's and access expanded comments screen
    PutText 21, 2, "", "END"
    'enter short comments
    If Len(comment) < 132 Then
        PutText 21, 2, comment & "  {" & Script & "} /" & UID, "ENTER"
        If Not Check4Text(23, 2, "02860") Then lATD22AllLoans = False
    'enter long comments
    Else
        'fill the first screen
        PutText 21, 2, Mid(comment, 1, 154), "ENTER"
        If Not Check4Text(23, 2, "02860") Then
            lATD22AllLoans = False
            Exit Function
        End If
        Hit "F4"
        'enter the rest on the expanded comments screen
        For k = 155 To Len(comment)
            Session.TransmitANSI Mid(comment, k, 260)
            k = k + 260
        Next k
        Session.TransmitANSI "  {" & Script & "} /" & UID
        Hit "ENTER"
        If Not Check4Text(23, 2, "02114") Then lATD22AllLoans = False
    End If
End Function

'-----------------------------------------------------------------------------'
'Functions related to the "Send E-mails" button
Private Sub btnSendEmails_Click()
    'End the script if there's no data to merge to the letters.
    If ProcessLetters() = False Then
        MsgBox "There is nothing to be printed.", vbOKOnly, "Bankruptcy Notification"
        End
    End If
End Sub

'ProcessLetters coordinates the major steps of gathering merge data, merging the letters, and sending e-mail notifications.
'Its return value is taken from the MergeLetter function, and indicates whether any merge data was found in the database.
Private Function ProcessLetters() As Boolean
    'Get merge data from the BSYS.BKNO_DAT_* tables.
    Dim RefundRequest() As String
    RefundRequest = SP.Common.SQLEx("SELECT * FROM BKNO_DAT_RefundRequest")
    Dim ProofOfClaim() As String
    ProofOfClaim = SP.Common.SQLEx("SELECT * FROM BKNO_DAT_ProofOfClaim")
    Dim ChangeOfAddress() As String
    ChangeOfAddress = SP.Common.SQLEx("SELECT * FROM BKNO_DAT_ChangeOfAddress")
    
    If UBound(RefundRequest, 2) = 0 And UBound(ProofOfClaim, 2) = 0 And UBound(ChangeOfAddress, 2) = 0 Then
        ProcessLetters = False
        Exit Function
    End If
    
    'Set up identifiers that apply to all letters.
    Dim DataHandle As Integer
    Dim DataFile As String
    Dim Record As Integer
    Dim DocName As String
    Dim SaveDoc As String
    Dim DocPath As String
    DocPath = "X:\PADD\Aux Services\"
    Dim Testing As Boolean
    Testing = SP.Common.TestMode(, DocPath)
    Dim FSO As Object
    Set FSO = CreateObject("Scripting.FileSystemObject")
    Const SavePath As String = "Q:\Auxiliary Services\Bankruptcy Documents for Printing\"   'TODO: Make sure this path exists on the programmer's and testers' machines.
    Dim TimeStamp As String
    TimeStamp = Format(Now, "mmddyyhhmmss")
    
    '---------- Refund Request ----------'
    If UBound(RefundRequest) > 0 Then
        'Write a merge data file from the RefundRequest array.
        DataFile = "T:\BRNotifRefundRequestdat.txt"
        DataHandle = FreeFile
        Open DataFile For Output As #DataHandle
        Write #DataHandle, "RecordNumber", "Name", "SSN", "Address1", "Address2", "City", "State", "ZIP", "Refund", "TranTyp", "DateApplied", "Amount"
        For Record = 0 To UBound(RefundRequest, 2) - 1
            Write #DataHandle, RefundRequest(1, Record), RefundRequest(2, Record), RefundRequest(3, Record), RefundRequest(4, Record), RefundRequest(5, Record), RefundRequest(6, Record), RefundRequest(7, Record), RefundRequest(8, Record), RefundRequest(9, Record), RefundRequest(10, Record), RefundRequest(11, Record), RefundRequest(12, Record)
        Next Record
        Close #DataHandle
        'Merge the letter.
        DocName = "RefundRequest"
        SaveDoc = "RefundRequest-" & TimeStamp
        SP.Common.SaveDocs DocPath, DocName, DataFile, SavePath & SaveDoc
        'Save a copy to X:\Archive.
        FSO.CopyFile SavePath & SaveDoc & ".doc", "X:\Archive\" & SaveDoc & ".doc"
        'Delete the database records.
        SP.Common.SQLNonQuery "DELETE FROM BKNO_DAT_RefundRequest"
    End If
    
    '---------- Proof of Claim ----------'
    If UBound(ProofOfClaim) > 0 Then
        'Write a merge data file from the ProofOfClaim array.
        DataFile = "T:\BRNotifPOCData.txt"
        DataHandle = FreeFile
        Open DataFile For Output As #DataHandle
        Write #DataHandle, "Debtor1", "Debtor2", "Court", "State", "CaseNumber", "SentTo", "A1", "ClaimAmount"
        For Record = 0 To UBound(ProofOfClaim, 2) - 1
            Write #DataHandle, ProofOfClaim(1, Record), ProofOfClaim(2, Record), ProofOfClaim(3, Record), ProofOfClaim(4, Record), ProofOfClaim(5, Record), ProofOfClaim(6, Record), ProofOfClaim(7, Record), Format(ProofOfClaim(8, Record), "#,##0.00")
        Next Record
        Close #DataHandle
        'Merge the letter.
        DocName = "POC"
        SaveDoc = "POC-" & TimeStamp
        SP.Common.SaveDocs DocPath, DocName, DataFile, SavePath & SaveDoc
        'Save a copy to X:\Archive.
        FSO.CopyFile SavePath & SaveDoc & ".doc", "X:\Archive\" & SaveDoc & ".doc"
        'Delete the database records.
        SP.Common.SQLNonQuery "DELETE FROM BKNO_DAT_ProofOfClaim"
    End If
    
    '---------- Change of Address ----------'
    If UBound(ChangeOfAddress) > 0 Then
        'Write a merge data file from the ChangeOfAddress array.
        DataFile = "T:\BRNotifChangeOfAddrDat.txt"
        DataHandle = FreeFile
        Open DataFile For Output As #DataHandle
        Write #DataHandle, "Case", "SSN", "Name"
        For Record = 0 To UBound(ChangeOfAddress, 2) - 1
            Write #DataHandle, ChangeOfAddress(1, Record), ChangeOfAddress(2, Record), ChangeOfAddress(3, Record)
        Next Record
        Close #DataHandle
        'Merge the letter.
        DocName = "ChangeOfAddr"
        SaveDoc = "ChangeOfAddr-" & TimeStamp
        SP.Common.SaveDocs DocPath, DocName, DataFile, SavePath & SaveDoc
        'Save a copy to X:\Archive.
        FSO.CopyFile SavePath & SaveDoc & ".doc", "X:\Archive\" & SaveDoc & ".doc"
        'Delete the database records.
        SP.Common.SQLNonQuery "DELETE FROM BKNO_DAT_ChangeOfAddress"
    End If
    
    'Send an e-mail notifying that bankruptcy documents are ready for printing.
    Const Recipients As String = "ORSGroup@utahsbr.edu"
    Const Subject As String = "Bankruptcy Documents to Print"
    Const Message As String = "Bankruptcy documents are ready for printing on " & SavePath
    Const CopyRecipients As String = ""
    SP.Common.SendMail Recipients, , Subject, Message, CopyRecipients, , , , SP.Common.TestMode
    
    ProcessLetters = True
End Function
'End of functions related to the "Send E-mails" button
'-----------------------------------------------------------------------------'
