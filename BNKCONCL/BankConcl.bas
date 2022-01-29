Attribute VB_Name = "BankConcl"
Private Enum StatusButton 'From the radio buttons on the BnkRso form
    Discharged
    Dismissed
    Hardship
    Partial
End Enum
Private Type LoanStruct
    ClaimID As String
    CLID As String
    BankruptcyRecordFound As Boolean
    LPOD As String
    BankruptcyStatus As String
End Type
Private Loans() As LoanStruct
Private Status As StatusButton
Private SSN As String
Private StatusDate As String
Private Chapter As String
Private counter As Integer
Private LC59Sta As String

Private Sub ForTesting()
    BankConStart (False)
End Sub

'process information entered by the user
Public Sub BankConStart(Optional DoQueueProcessing = True)
    Dim AccountNumber As String
    Dim FiledDate As String
    Dim CaseNo As String
    Dim Repur As String 'TODO: Make this a boolean, with a better name.
    Dim LP50Ind As String
    Dim NumberOfEligibleLoans As Integer
    Dim ProcessBankruptcy As Boolean
    Dim LtrInd As String
    Dim DocType As String
    Dim dataFile As String
    
    DocPath = "X:\PADD\Bankruptcy\"
    TestMode , DocPath
    FiledDate = "12/31/9999"
    dataFile = "T:\BankConclDat.txt"
    'go to the BTOBEREP subroutine to process tasks in the BTOBEREP queue
    BTOBEREP DoQueueProcessing
    If Check4Text(1, 9, "ASCON") Then
        AccountNumber = Replace(GetText(17, 52, 12), " ", "")
        SSN = GetText(17, 70, 9)
        BnkRso.txtSSN.Text = AccountNumber
    End If
    
    'display a dialog box for the user to enter the SSN, status date, and select (1) discharge, (2) dismissal, (3) adversary discharge, or (4) partial discharge
    Load BnkRso
    BnkRso.Show
    SSN = BnkRso.txtSSN.Text
    If InStr(1, BnkRso.txtStatusDate.Text, "/", 1) = 3 Then
            StatusDate = BnkRso.txtStatusDate
        Else
            StatusDate = Format(BnkRso.txtStatusDate, "##/##/####")
    End If
    'Set the Status based on which radio button is checked.
    If BnkRso.radDischarged.Value = True Then
        Status = Discharged
    ElseIf BnkRso.radDismissed.Value = True Then
        Status = Dismissed
    ElseIf BnkRso.radHardship.Value = True Then
        Status = Hardship
    ElseIf BnkRso.radPartial.Value = True Then
        Status = Partial
    End If
    Unload BnkRso
    
    'set activity type to CD and contact type to 14
    ActTyp = "CD"
    ConTyp = "14"
    'verify the address is current and end the script if it is not
    'access LP22
    FastPath "LP22I;"
    If Len(SSN) = 9 Then
        PutText 4, 33, SSN, "ENTER"
    Else
        PutText 6, 33, SSN, "ENTER"
    End If
    'end script if SSN not found
    If Check4Text(22, 3, "47004") Then End
    SSN = GetText(3, 23, 9)
    AccountNumber = Replace(GetText(3, 60, 12), " ", "")
    'warn the user and end the script if the address is not valid
    If Not Check4Text(10, 57, "Y") Then
        MsgBox "The address for this borrower is not valid.  Update the address and run the script again to process the bankruptcy."
        End
    End If
    'go to the GetStatus subroutine to determine the status of the loans being processed
    GetStatus FiledDate, CaseNo, Repur, LP50Ind, NumberOfEligibleLoans, ProcessBankruptcy
    If ProcessBankruptcy = True Then
        'set date due to the first of next month
        DateDue = DateValue(Month(DateAdd("m", 1, Date)) & "/01/" & Year(DateAdd("m", 1, Date)))
        'go to the LC59Sel subroutine to determine if the account can be repurchased this month
        LC59Sel
        'go to the Rev4Disch subroutine to review the loans for discharge if the filed date is before 10/08/1998
        If Status = Discharged And DateValue(FiledDate) < DateValue("10/08/1998") Then
            Rev4Disch DoQueueProcessing
        End If
        'if (4) partial discharge was selected, search for an activity record with action code BRVW1 to indicate the account is prepared for discharge
        If Status = Partial Then
            'access LP50
            FastPath "LP50I" & SSN
            'enter action code BRVW1
            PutText 9, 20, "BRVW1"
            'enter date rate from 60 days past to today
            PutText 18, 29, Format(Date - 60, "MMDDYYYY")
            PutText 18, 41, Format(Date, "MMDDYYYY")
            Hit "ENTER"
            'warn the user and end the script if an activity record was not found
            If Check4Text(22, 3, "47004") Then
                MsgBox "An activity record with action code BRVW1 was not found.  The borrower's account may not have been prepared for discharge.", vbOKOnly, "Not Prepared for Discharge"
                End
            'ask the user if the appropriate actions have been taken
            Else
                MsgBox "Review the account to determine if the action needed to prepare the account for discharge has taken place and then hit Insert to continue.", vbOKOnly, "Review for Discharge Preparation"
                PauseForInsert
                If MsgBox("Has the needed action taken place?  Click Yes to process the discharge or No to end the script.", vbYesNo, "Verify Action") <> vbYes Then End
                'verify the user meant to proceed
                If MsgBox("Are you sure?  Click Yes to proceed or No to end.", vbYesNo, "Confirm Action") <> vbYes Then End
            End If
        End If
        'set the action code, comment, and document type based on the type of conclusion the user selected
        Select Case Status
            Case Discharged
                ActCd = "BDISC"
                Comment = "received notice of bankruptcy discharge, case no " & CaseNo & " discharged " & Format(StatusDate, "MMDDYYYY")
                DocType = "discharged"
            Case Dismissed
                ActCd = "BDISM"
                Comment = "received notice of bankruptcy dismissal, case no " & CaseNo & " dismissed " & Format(StatusDate, "MMDDYYYY")
                DocType = "dismissed"
            Case Partial
                ActCd = "BCOST"
                Comment = "received notice of court ordered settlement for case no " & CaseNo & " " & Format(StatusDate, "MMDDYYYY")
        End Select
        'update each loan
        For counter = 0 To UBound(Loans)
            'discharge the loans if the user selected (3) adversary discharge and the loan should be processed
            If Status = Hardship And Loans(counter).BankruptcyRecordFound Then
                'set the repur indicator to "N" so a task is not created in REPURCHS
                Repur = "N"
                'blank the action code indicator
                LP50Ind = ""
                'discharge the loan if it was not purchased in the current month
                If DateValue(Loans(counter).LPOD) < DateValue(Month(Date) & "/01/" & Year(Date)) Then
                    'set bankrupcty status to 03 (discharged)
                    Loans(counter).BankruptcyStatus = "03"
                    'search for an activity record with action code BRVW1 to indicate the account is approved for discharge
                    'access LP50
                    FastPath "LP50I" & SSN
                    'enter action code BRVW1
                    PutText 9, 20, "BRVW1"
                    'enter date rate from 60 days past to today
                    PutText 18, 29, Format(Date - 60, "MMDDYYYY")
                    PutText 18, 41, Format(Date, "MMDDYYYY")
                    Hit "ENTER"
                    'warn the user and end the script if the activity record is not found
                    If Check4Text(22, 3, "47004") Then
                        MsgBox "The borrower's account has not been approved for discharge.", vbOKOnly, "Not Approved for Discharge"
                        End
                    Else
                        'if the activity record is found, process the loan
                        'go to the LC34 subroutine to update the loan status
                        UpdateBankruptcyStatus Loans(counter).ClaimID, Loans(counter).BankruptcyStatus
                        'set the action code and comment and the activity record indicator so an activity record will be created
                        ActCd = "BXDOD"
                        Comment = "Received notice of discharge of debtor due to adversary proceeding, BNKDISCH letter sent to borrower"
                        LP50Ind = "A"
                        LtrInd = "D"
                    End If
                'warn the user and end the script if the loan was purchased in the current month
                Else
                    MsgBox "The script cannot discharge loan " & Loans(counter).ClaimID & " due to the recent lender payoff date.  Please verify that the loan should be discharged and process it next month.", vbOKOnly, "Recent Purchase"
                End If
            'process the loan if the bankruptcy status is 06 (repurchase) and the loan should be processed
            ElseIf Loans(counter).BankruptcyStatus = "06" And Loans(counter).BankruptcyRecordFound = True Then
                'go to the LC34 subroutine to update the loan status if the loan was not purchased in the current month
                If DateValue(Loans(counter).LPOD) < DateValue(Month(Date) & "/01/" & Year(Date)) And LC59Sta = "Y" Then
                    UpdateBankruptcyStatus Loans(counter).ClaimID, Loans(counter).BankruptcyStatus
                'create a task in the BTOBEREP queue if the loan was purchased in the current month
                Else
                    If DoQueueProcessing Then
                        'set queue to BTOBEREP
                        Queue = "BTOBEREP"
                        'set comments with information needed to process the loan
                        LP9OComment1 = "bankruptcy account to be repurchased"
                        LP9OComment2 = Loans(counter).ClaimID & Format(StatusDate, "MMDDYYYY") & ActCd
                        LP9OComment3 = ""
                        LP9OComment4 = ""
                        'go to the Common.LP90 subroutine to create the task
                        Common.LP9O
                    End If
                End If
            'process the loan if the bankruptcy status is 02 ([loan] not discharged] or 05 (dismissed) and the loan should be processed
            ElseIf (Loans(counter).BankruptcyStatus = "02" Or Loans(counter).BankruptcyStatus = "05") And Loans(counter).BankruptcyRecordFound = True Then
                'go to the LC34 subroutine to update the loan status
                UpdateBankruptcyStatus Loans(counter).ClaimID, Loans(counter).BankruptcyStatus
                'set the letter indicator to "C" so a bankruptcy conclusion letter will be generated
                LtrInd = "C"
            End If
        Next counter
        'create a task in the REPURCHS queue if the repur indicator is "Y"
        If Repur = "Y" Then
            'set the queue to REPURCHS
            Queue = "REPURCHS"
            'if the date is past the 15th, set the date due to the first of next month, other wise set the date due to today
            If Day(Date) > 15 Then
                DateDue = DateValue(Month(DateAdd("m", 1, Date)) & "/01/" & Year(DateAdd("m", 1, Date)))
            Else
                DateDue = Date
            End If
            If DoQueueProcessing Then
                LP9OComment1 = ""
                LP9OComment2 = ""
                LP9OComment3 = ""
                LP9OComment4 = ""
                'go to the Common.LP90 subroutine to create the task
                Common.LP9O
            End If
        End If
        'go to the Common.LP50 subroutine to create an activity record if the activity record indicator is "R" (record needed to indicate loans were repurchased) or "B" (both repurchase and default records needed)
        If LP50Ind = "R" Or LP50Ind = "B" Then
            Comment = Comment & ", loan(s) referred to records management for repurchase"
            Common.LP50
        End If
        'create a queue task for collections to resume collections, enter a type 14 hold on LC18, and create an activity record if the activity record indicator is "D" (record needed to indidate loans returned to default collections) or "B" (both repurchase and default records needed)
        If LP50Ind = "D" Or LP50Ind = "B" Then
            'add the task
            Queue = "DBKCONCL"
            DateDue = Date
            If DoQueueProcessing Then
                LP9OComment1 = "bankruptcy action concluded, review account to resume collection activity and set or negotiate payment arrangements"
                Common.LP9O
            End If
            'access LC18
            FastPath "LC18C" & SSN
            'enter the hold type 14 (management request)
            PutText 15, 30, "14"
            'enter expiration date of today + 60 days
            PutText 15, 12, Format(Date + 60, "MMDDYYYY"), "ENTER"
            'add activity record
            Comment = Comment & ", loan(s) to return to collections, bnkrptcy " & DocType & " ltr mailed to borr, LC18 hld 14 (mngt req) added, DBNKCONCL task added for staff review"
            Common.LP50
        End If
        'go to the Common.LP50 subroutine to create an activity record if the activity record indicator is "A" (record needed to indicate loans were discharged due to adversary proceeding)
        If LP50Ind = "A" Then
            Common.LP50
        End If
        'create a bankruptcy conclusion letter if the letter indicator is "Y"
        If LtrInd = "C" Then
            'go to the Common.LP22 subroutine to get the demographic information
            Common.LP22
            'create a data file
            Open dataFile For Output As #1
                 Write #1, "AccountNumber", "KeyLine", "LastName", "FirstName", "Address1", "Address2", "City", "State", "Country", "ZIP", "DocType", "StaDate"
                 Write #1, CommonLP22AccNum, ACSKeyLine(SSN), LastName, FirstName, Address1, Address2, City, State, Country, ZIP, DocType, Format(StatusDate, "mmmm d, yyyy")
            Close #1
            'set the document name and path
            Doc = "BANKCONCL"
            'Call the Document Generation script to print the letter.
            SP.DocCreateAndDeploy.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt Doc, stacBoth, SSN, dataFile, "AccountNumber", Doc, DocPath, UserID, "BNKCONCL", "State", dmLetter
            Kill dataFile
        End If
        If LtrInd = "D" Then
            DischLtr
        End If
    End If

    'warn the user that processing is complete and end the script
    If NumberOfEligibleLoans > 0 Or CompassBankruptcy(SSN) Then
        MsgBox "Processing Complete", vbOKOnly, "Processing Complete"
    Else
        MsgBox "No updateable loans were found.  To be updateable, a loan must have a claim status of 03 (open), an auxiliary claim status of 07 (bankruptcy), a blank assigned to ED date, and a bankrupcty status of 01 (pending) or 07 (paying outside the plan).", vbOKOnly, "Processing Complete"
    End If
End Sub

'check LC05, LC12, and LC02 to get the loan status
Private Sub GetStatus(ByRef FiledDate As String, ByRef CaseNo As String, ByRef Repur As String, ByRef LP50Ind As String, ByRef NumberOfEligibleLoans As Integer, ByRef ProcessBankruptcy As Boolean)
    Dim x As Integer
    Dim PreClmStat As String
    
    NumberOfEligibleLoans = 0
    'access LC05
    FastPath "LC05I" & SSN
    If Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then Exit Sub
    'select the first loan
    If Check4Text(21, 3, "SEL") Then
        Session.TransmitANSI "01"
        Hit "ENTER"
    End If
    ReDim Loans(0)
    Do While Not Check4Text(22, 3, "46004")
        'get the loan information if the claim status is 03, the aux status is 07, and the assigned to ED date is blank
        If Check4Text(4, 10, "03") And Check4Text(4, 26, "07") And Check4Text(19, 73, "MMDDCCYY") Then
            'get the lender payoff date, status date, and claim ID
            Loans(UBound(Loans)).LPOD = DateValue(Format(GetText(4, 73, 8), "##/##/####"))
            Loans(UBound(Loans)).ClaimID = GetText(21, 11, 4)
            'page to page 3
            Hit "F10"
            Hit "F10"
            'get the commonline unique ID
            Loans(UBound(Loans)).CLID = GetText(3, 13, 19)
            'Resize the array to make room for another loan.
            ReDim Preserve Loans(UBound(Loans) + 1)
        End If
        'go to the next loan
        Hit "F8"
    Loop
    'Chop off the empty index at the end of the Loans array.
    ReDim Preserve Loans(UBound(Loans) - 1)
    'access LC12
    FastPath "LC12I" & SSN
    'select the first loan
    If Check4Text(21, 3, "SEL") Then
        Session.TransmitANSI "01"
        Hit "ENTER"
    End If
    'process each loan identified on LC05
    For counter = 0 To UBound(Loans)
        'forward through each loan until the unique id matches the unique id of the loan being processed
        Do Until Check4Text(2, 60, Loans(counter).CLID) And (Check4Text(6, 10, "01") Or Check4Text(6, 10, "07"))
            Hit "F8"
            'set the process status to "N" (do no process) if no bankruptcy record is found for the loan
            If Check4Text(22, 3, "46004") Then
                If Loans(counter).BankruptcyRecordFound = True Then
                    Exit Do
                End If
                Loans(counter).BankruptcyRecordFound = False
                Exit Do
            End If
        'get the case number and filed date and set the process status to "Y" (process) if the bankruptcy status is 01 (pending) or 07 (paying outside the plan)
        If Not Check4Text(22, 3, "46004") Then
                Loans(counter).BankruptcyRecordFound = True
                CaseNo = GetText(7, 18, 15)
                FiledDate = Format(GetText(9, 25, 8), "##/##/####")
                Chapter = GetText(5, 31, 2)
                NumberOfEligibleLoans = NumberOfEligibleLoans + 1
        End If
        Loop
        'go back to the first loan to start looking for the next loan
        Do While Not Check4Text(22, 3, "46003")
            Hit "F7"
        Loop
    Next counter
    'LC02
    'look for the preclaim status to determine what action to date for each processable loan (process status = "Y")
    For counter = 0 To UBound(Loans)
        If Loans(counter).BankruptcyRecordFound = True Then
            'get the preclaim status from LC05
            FastPath "LC05I" & SSN
            Do
                For x = 7 To 16 Step 3
                    If Check4Text(x, 8, Loans(counter).ClaimID) Then
                        PreClmStat = GetText(x + 1, 9, 2)
                        Exit Do
                    End If
                Next x
                Hit "F8"
            Loop
            
            'set values if the preclaim reason indicates bankruptcy (BC, BH, or BO)
            If PreClmStat = "BC" Or PreClmStat = "BH" Or PreClmStat = "BO" Then
                'set bankruptcy status to 06
                Loans(counter).BankruptcyStatus = "06"
                'set repur indicator to "Y" if the loan was not purchased in the current month so a task will be created in the REPURCHS queue
                If DateValue(Loans(counter).LPOD) < DateValue(Month(Date) & "/01/" & Year(Date)) Then
                    Repur = "Y"
                End If
                'if the activity record indicator is blank or "R", set the indicator to "R", otherwise set it to "B"
                If LP50Ind = "" Or LP50Ind = "R" Then
                    LP50Ind = "R"
                Else
                    LP50Ind = "B"
                End If
            End If
            'set values if the preclaim reason indicates default (DB, DF, DQ, or DU)
            If PreClmStat = "DB" Or PreClmStat = "DF" Or PreClmStat = "DQ" Or PreClmStat = "DU" Then
                'set the bankruptcy status based on the conclusion type selected by the user
                Select Case Status
                    Case Discharged
                        '(1) discharge = 02 ([loan] not discharged)
                        Loans(counter).BankruptcyStatus = "02"
                    Case Dismissed
                        '(2) dismissal = 05 (dismissed)
                        Loans(counter).BankruptcyStatus = "05"
                    Case Partial
                        '(4) partial discharge = 04 (court ordered settlement)
                        Loans(counter).BankruptcyStatus = "02"
                End Select
                'if the activity record indicator is blank or "D", set the indicator to "D", otherwise set it to "B"
                If LP50Ind = "" Or LP50Ind = "D" Then
                    LP50Ind = "D"
                Else
                    LP50Ind = "B"
                End If
            End If
        End If
    Next counter
    'A populated ClaimID for Loans(0) means we picked up at least one loan.
    If Loans(0).ClaimID <> "" Then
        If MsgBox("Is Case Number " & CaseNo & " the correct bankruptcy to process?", vbYesNo) = vbYes Then
            ProcessBankruptcy = True
        Else
            ProcessBankruptcy = False
        End If
    End If
End Sub

'review loans for discharge where the claim was filed before 10/08/1998
Private Sub Rev4Disch(ByVal DoQueueProcessing As Boolean)
    'access LP50
    FastPath "LP50I" & SSN
    'enter action codes BRVW1 (discharge approved) and BRVW0 (discharge not approved)
    PutText 9, 20, "BRVW1"
    PutText 9, 28, "BRVW0"
    'specify a date range from 60 days ago to today
    PutText 18, 29, Format(Date - 60, "MMDDYYYY")
    PutText 18, 41, Format(Date, "MMDDYYYY")
    Hit "ENTER"
    'warn the user, create a task in the BKRPTRVW queue, and end the script if no activity records are found
    If Check4Text(22, 3, "47004") And DoQueueProcessing Then
        'warn the user
        MsgBox "The borrower's filing date may possibly make the borrower's loans eligible for discharge.  Since the action codes BRVW1 and BRVW0 were not found, a queue task will be created in the BKRPTRVW queue for review.", vbOKOnly, "Review Needed for Possible Discharge"
        'access LP9O
        FastPath "LP9OA" & SSN & ";;BKRPTRVW"
        'prompt user if unable to add task
        If Not Check4Text(22, 3, "44000") Then
            MsgBox "Unable to add the task.  Wait for the script to finish and then enter the task manually."
        Else
            'enter info
            PutText 16, 12, "review bankruptcy filing for possible discharge of borrower's loans", "ENTER"
            Hit "F6"
            Session.Wait 2 'go figure why this has to be here but it wouldn't work otherwise
        End If
        End
    'update the loan status, create an activity record, and end the script if an activity record is found with the action code BRVW1 (discharge approved)
    ElseIf Check4Text(8, 2, "BRVW1") Or Check4Text(7, 7, "BRVW1") Then
        'go to the LC34 subroutine to update the bankrupcty status for each loan
        For counter = 0 To UBound(Loans)
            Loans(counter).BankruptcyStatus = "03"
            UpdateBankruptcyStatus Loans(counter).ClaimID, Loans(counter).BankruptcyStatus
        Next counter
        'set the action code and comment
        ActCd = "BXDOD"
        Comment = "Received notice of discharge of bankruptcy, borrower account is eligible for discharge and has been approved, BNKDISCH letter sent to borrower"
        'go to the Common.LP50 subroutine to create the activity record
        Common.LP50
        DischLtr
        End
    End If
End Sub

Private Sub UpdateBankruptcyStatus(ByVal ClaimID As String, ByVal BankruptcyStatus As String)
    Dim row As Integer
    
    'access LC34
    FastPath "LC34C" & SSN & "04"
    'enter the bankruptcy status and status date entered by the user
    PutText 4, 14, BankruptcyStatus
    Session.TransmitANSI Format(StatusDate, "MMDDYYYY")
    
    'select the loans
    row = 11
    'compare each claim ID against the claim ID on the row being processed until a match is found
    Do
        'enter an X in the selection field to select the loan if the claim ID matches, the claim status is 03, and the assigned to ED status is blank
        If Check4Text(row, 7, ClaimID) Then
            PutText row, 3, "X"
            'exit the processing loop to process the next loan
            Exit Do
        End If
        row = row + 1
        'if the row is blank, forward to the next page, if no more pages are displayed, stop processing
        If Check4Text(row, 3, " ") Then
            Hit "F8"
            If Check4Text(22, 3, "46004") Then
                Exit Do
            Else
                row = 11
            End If
        End If
    Loop
    'enter to update the selected loan
    Hit "ENTER"
    'warn the user and end the script if the loans were not updated
    If Not Check4Text(22, 3, "49000") Then
        MsgBox "The loans were not updated.  Review the loans and run the script again."
        End
    End If
End Sub

'complete tasks in the BTOBEREP queue
Private Sub BTOBEREP(ByVal DoQueueProcessing As Boolean)
     'access the queue task in LP9A
    FastPath "LP9ACBTOBEREP"
    'warn the user and end the script if a task is displayed for a queue other than BTOBEREP
    If Check4Text(1, 9, "BTOBEREP") = False And Check4Text(1, 9, "ASCON") = False Then
        MsgBox "You have an unresolved task in the " & GetText(1, 9, 8) & " queue.  You must complete the task before running this script."
        End
    End If
    If Check4Text(1, 9, "ASCON") Then
        Exit Sub
    End If
    'process the tasks if any are displayed
    If (Not Check4Text(22, 3, "47423")) And (Not Check4Text(22, 3, "47420")) Then
        'warn the user to wait
        MsgBox "Please wait while the script completes tasks in the BTOBEREP queue.", vbOKOnly, "BTOBEREP"
        'process each task
        Dim ClaimID As String
        Do While Not Check4Text(22, 3, "46004")
            'get the ssn, claim id, status date, and action code
            SSN = GetText(17, 70, 9)
            ClaimID = GetText(12, 61, 4)
            StatusDate = DateValue(Format(GetText(12, 65, 4) & GetText(13, 11, 4), "##/##/####"))
            ActCd = GetText(13, 15, 5)
            FastPath "LP8YIALL;REPURCHS;;" & SSN
            If (Not Check4Text(1, 75, "DETAIL")) And DoQueueProcessing Then
                'create task in REPURCHS queue
                Queue = "REPURCHS"
                DateDue = Date
                LP9OComment1 = ""
                LP9OComment2 = ""
                LP9OComment3 = ""
                LP9OComment4 = ""
                'go to the Common.LP90 subroutine to create the task
                Common.LP9O
            End If
            'go to the LC34 subroutine to update the loan status
            UpdateBankruptcyStatus ClaimID, "06"
            'set the activity type to CD and the contact type to 14
            ActTyp = "CD"
            ConTyp = "14"
            'set the comment
            Comment = "updated borrower bankruptcy status in LC34"
            'go to the Common.LP50 subroutine to create the activity record
            Common.LP50
            'go back to LP9A
            FastPath "LP9ACBTOBEREP"
            'use hotkey F6 to complete the task
            Hit "F6"
            'use hotkey F8 to go to the next task
            Hit "F8"
            'go to the next task unless no more are displayed
        Loop
    End If
End Sub

Private Sub DischLtr()
    Dim Keyline As String
    Dim CLIDt As String
    Dim GuarDt As String
    Dim LnTyp As String
    Dim NetGuar As Double
    Dim GuarAmt As Double
    Dim RefAmt As Double
    Dim CancAmt As Double
    Dim dataFile As String
    
    'go to the Common.LP22 subroutine to get the demographic information
    Common.LP22
    Keyline = ACSKeyLine(SSN)
    'create a data file
    dataFile = "T:\DischLtrDat.txt"
    Open dataFile For Output As #1
    Write #1, "AccountNumber", "SSN", "LastName", "FirstName", "KeyLine", "Address1", "Address2", "City", "State", "Country", "ZIP", "Chapter", "StaDate", "CLID", "LnTyp", "GuarDt", "NetGuar"
    Close #1
    FastPath "LG02I;" & SSN
    If Check4Text(21, 3, "SEL") Then
        PutText 21, 13, "01", "ENTER"
    End If
    Do
        counter = 1
        Do
            'get the loan information
            If Check4Text(1, 56, "CON") Then           'CL
                GuarDt = GetText(5, 10, 8)
                LnTyp = "Consolidation"
                CLIDt = GetText(5, 31, 19)
            ElseIf Check4Text(1, 60, "PLUS LOA") Then  'PL pre-common/common
                GuarDt = GetText(4, 10, 8)
                LnTyp = "PLUS"
                If Check4Text(2, 32, "ID") Then        'common
                    CLIDt = GetText(2, 35, 19)
                Else
                    CLIDt = GetText(2, 33, 19)          'pre-common
                End If
            ElseIf Check4Text(1, 60, "PLUS MAS") Then  'PL MPN
                GuarDt = GetText(5, 10, 8)
                LnTyp = "PLUS"
                CLIDt = GetText(3, 30, 19)
            ElseIf Check4Text(1, 61, "SLS") Then       'SL
                GuarDt = GetText(4, 10, 8)
                LnTyp = "SLS"
                CLIDt = GetText(2, 33, 19)
            ElseIf Check4Text(1, 65, "MAS") Then       'SF,SU MPN
                If Check4Text(4, 59, "SUB") Then
                    LnTyp = "Subsidized"
                Else
                    LnTyp = "Unsubsidized"
                End If
                GuarDt = GetText(5, 10, 8)
                CLIDt = GetText(3, 32, 19)
            Else                                                'SF,SU
                If Check4Text(12, 7, "LOAN") Then
                    If Check4Text(4, 59, "SUB") Then
                        LnTyp = "Subsidized"
                    Else
                        LnTyp = "Unsubsidized"
                    End If
                Else
                    If Check4Text(13, 16, "000000") Then
                        LnTyp = "Unsubsidized"
                    Else
                        LnTyp = "Subsidized"
                    End If
                End If
                GuarDt = GetText(5, 10, 8)
                CLIDt = GetText(3, 33, 19)
            End If
            If CLIDt = Loans(counter).CLID Then
                'use the hotkey to acess LG0H and get the net guarantee amount
                If Check4Text(24, 45, "LG0H") Then
                    Hit "F10"
                Else
                    Hit "F11"
                End If
                GuarAmt = GetText(2, 40, 10)
                If Check4Text(7, 27, "CC") Then
                    RefAmt = 0
                Else
                    RefAmt = GetText(7, 19, 10)
                End If
                If Check4Text(7, 54, "CC") Then
                    CancAmt = 0
                Else
                    CancAmt = GetText(7, 46, 10)
                End If
                NetGuar = GuarAmt - RefAmt - CancAmt
                'go back to LG02
                Hit "F12"
                'write data to text file
                Open dataFile For Append As #1
                Write #1, CommonLP22AccNum, Format(SSN, "@@@-@@-@@@@"), LastName, FirstName, Keyline, Address1, Address2, City, State, Country, ZIP, Chapter, Format(StatusDate, "mmmm d, yyyy"), Loans(counter).CLID, LnTyp, Format(GuarDt, "##/##/####"), Format(NetGuar, "###,##0.00")
                Close #1
                Exit Do
            End If
            counter = counter + 1
        Loop Until counter > UBound(Loans)
        Hit "F8"
        If Check4Text(22, 3, "46004") Then
            Exit Do
        End If
    Loop
    PrintInd = "Y"
    'set the document name and path
    Doc = "BNKDISCH"
    'Call the Document Generation script to print the letter.
    SP.DocCreateAndDeploy.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt Doc, stacBoth, SSN, dataFile, "AccountNumber", Doc, DocPath, UserID, "BNKCONCL", "State", dmLetter
    Kill dataFile
End Sub

'select the LC59 records to review
Private Sub LC59Sel()
    Dim rec As Integer
    
    LC59Sta = "Y"
    'access LC59
    FastPath "LC59I" & SSN
    If Check4Text(22, 3, "47020") Then Exit Sub
        
    rec = 1
    'select the records and go to the LC59Rev subroutine to review the records
    If Check4Text(21, 3, "SEL") Then
        Do
            Session.TransmitANSI rec
            Hit "ENTER"
            If Check4Text(22, 3, "47001") Then
                Hit "F8"
                If Check4Text(22, 3, "46004") Then
                    Exit Sub
                Else
                    rec = 1
                    GoTo 99 'If we ever start using VB 2005 or newer, there's now a "Continue Do" statement that behaves like a "continue" statement in C and obviates the need for this GoTo.
                End If
            End If
            'LC59Rev
            rc = 1
            'select the records and go to the LC59Rev subroutine to review the records
            If Check4Text(21, 3, "SEL") Then
                Do
                    Session.TransmitANSI rc
                    Hit "ENTER"
                    If Check4Text(22, 3, "47001") Then
                        Hit "F8"
                        If Check4Text(22, 3, "46004") Then
                            Exit Do
                        Else
                            rc = 1
                            Session.TransmitANSI rc
                            Hit "ENTER"
                        End If
                    End If
                    LC59Rev
                    If LC59Sta = "N" Then Exit Sub
                    Hit "F12"
                    rc = rc + 1
                Loop
            Else
                LC59Rev
            End If
            'exit the subroutine if the status is "N" or continue processing
            If LC59Sta = "N" Then Exit Sub
            Hit "F12"
            rec = rec + 1
99      Loop
    Else
        LC59Rev
    End If
End Sub

'review LC59 records
Private Sub LC59Rev()
    'review each record, if the returned date less than or equal to the received date of the CLM PMT PROC date is in the current month, the account cannot be repurchased this month
    Do
        'If the RETURNED date is not blank and the RESUBMITTED date is blank and the CLM PMT PROC date is blank, the claim was returned so the account can be repurchased this month.
        If Check4Text(6, 16, "MMDDCCYY") = False And Check4Text(7, 16, "MMDDCCYY") = True And Check4Text(6, 72, "MMDDCCYY") = True Then
            LC59Sta = "Y"
        End If
        'If the CLM PMT PROC date is not blank, the claim (or resubmitted claim) has been paid.
        If Not Check4Text(6, 72, "MMDDCCYY") Then
            'If the CLM PMT PROC date is greater than the first of the current month, the claim was paid in the current month or is pending payment (the CLM PMT PROC date may be future dated to the next claim payment date) and so the account cannot be repurchased this month.
            If DateValue(Format(GetText(6, 72, 8), "##/##/####")) > DateValue(Month(Date) & "/01/" & Year(Date)) Then
                LC59Sta = "N"
                'Set the date due for the BTOBEREP task to the be the first of the month after the CLM PMT PROC date to make sure the account is not repurchased in the same month the claim was/is paid.
                DateDue = DateValue(Month(DateAdd("m", 1, DateValue(Format(GetText(6, 72, 8), "##/##/####")))) & "/01/" & Year(DateAdd("m", 1, DateValue(Format(GetText(6, 72, 8), "##/##/####")))))
                Exit Sub
            End If
        End If
        'If the RETURNED date is not blank and the RESUBMITTED date is not blank and the CLM PMT PROC date is blank, the resubmitted claim was either returned or is still being processed.
        If Check4Text(6, 16, "MMDDCCYY") = False And Check4Text(7, 16, "MMDDCCYY") = False And Check4Text(6, 72, "MMDDCCYY") = True Then
            'If the RETURNED date is greater than the RESUBMITTED date, the resubmitted claim was returned and it is okay to repurchase the account.
            If DateValue(Format(GetText(6, 16, 8), "##/##/####")) > DateValue(Format(GetText(7, 16, 8), "##/##/####")) Then
                LC59Sta = "Y"
            'If the RETURNED date is less than the RESUBMITTED date, the resubmitted claim is still being processed so the account cannot be repurchased this month.
            Else
                LC59Sta = "N"
                'Set the date due for the BTOBEREP task to the be the first of the month after the date which is 45 days future from the RESUBMITTED date.  Claims has 45 days to review and return or pay the claim so this edit will make sure the account is not repurchased in the same month the claim is paid.
                DateDue = DateValue(Month(DateAdd("m", 1, DateValue(Format(GetText(7, 16, 8), "##/##/####")) + 45)) & "/01/" & Year(DateAdd("m", 1, DateValue(Format(GetText(7, 16, 8), "##/##/####")) + 45)))
                Exit Sub
            End If
        End If
        'If the RETURNED and RESUBMITTED dates are blank but the CLM PMT PROC and RECEIVED dates are present, the loan can be repurchased.
        If Check4Text(6, 16, "MMDDCCYY") = True And Check4Text(7, 16, "MMDDCCYY") = True And Check4Text(6, 72, "MMDDCCYY") = False And Check4Text(5, 16, "MMDDCCYY") = False Then
            LC59Sta = "Y"
        End If
        'If the RETURNED date is blank and the RESUBMITTED date is blank and the CLM PMT PROC date is blank, the claim is still being processed so the account cannot be repurchased this month.
        If Check4Text(6, 16, "MMDDCCYY") = True And Check4Text(7, 16, "MMDDCCYY") = True And Check4Text(6, 72, "MMDDCCYY") = True Then
            LC59Sta = "N"
            'Set the date due for the BTOBEREP task to the be the first of the month after the date which is 45 days future from the RECEIVED date.  Claims has 45 days to review and return or pay the claim so this edit will make sure the account is not repurchased in the same month the claim is paid.
            DateDue = DateValue(Month(DateAdd("m", 1, DateValue(Format(GetText(5, 16, 8), "##/##/####")) + 45)) & "/01/" & Year(DateAdd("m", 1, DateValue(Format(GetText(5, 16, 8), "##/##/####")) + 45)))
            Exit Sub
        End If
        'go to the next record
        Hit "F8"
        'exit the loop if there are no more loans to process
        If Check4Text(22, 3, "46004") Then Exit Do
    Loop
End Sub

Private Function CompassBankruptcy(ByVal SSN As String) As Boolean
    Dim x As Integer
    Dim BDN As String 'Bankruptcy Docket Number
    Dim UserID As String
    
    FastPath "TX3Z/ITD0T" & SSN
    If Check4Text(23, 2, "01019 ENTERED KEY NOT FOUND") Then
        CompassBankruptcy = False
        Exit Function
    End If
    If Check4Text(1, 72, "TDX2J") Then
        'selection
        Do While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            For x = 8 To 20
                If Check4Text(x, 25, "VERIFIED") Then
                    PutText 21, 18, GetText(x, 5, 2), "ENTER"
                    Exit Do
                End If
            Next x
            Hit "F8"
        Loop
    End If
    If Check4Text(1, 74, "TDX0U") = False Then
        CompassBankruptcy = False
        Exit Function
    End If
    If Check4Text(5, 10, "05") = False Then
        BDN = GetText(13, 29, 10)
        If MsgBox("Is " & BDN & " the Bankruptcy Case Number you want to process?", vbYesNo) = vbYes Then
            PutText 1, 4, "C", "ENTER"
            PutText 5, 10, "05", "ENTER"
            UserID = GetUserID
            If Status = Discharged Then
                ATD22AllLoans SSN, "BDISC", "", "BNKCONCL", UserID
            End If
            If Status = Dismissed Then
                ATD22AllLoans SSN, "BDISM", "", "BNKCONCL", UserID
            End If
        Else
            CompassBankruptcy = False
            Exit Function
        End If
    End If
    'At this point, we know there's a bankruptcy on COMPASS. Do any further processing needed.
    CompassBankruptcy = True
    'See if a Bankruptcy Forbearance exists on the account.
    FastPath "TX3Z/ITS0H" & SSN & ";;F10"
    If Check4Text(23, 2, "11113") Then  'No forbearance information found.
        Exit Function
    End If
    'See if the current date is within the forbearance period.
    If Date < CDate(GetText(7, 18, 10)) Or Date > CDate(GetText(8, 18, 10)) Then
        Exit Function
    End If
    FastPath "TX3Z/CTS0H" & SSN & ";;F10"
    PutText 8, 18, Format(Date, "MMDDYY") 'End Date = today
    PutText 9, 18, Format(Date, "MMDDYY") 'Date Certified = today
    PutText 17, 34, "Y"                     'Capitalize interest
    PutText 18, 34, "Y"                     'Signature of borrower
    PutText 20, 17, "", "END"               'Clear the Manual Review
    Hit "F6"
    ATD22AllLoans SSN, "OU200", "Bankruptcy dismissed or discharged, bankruptcy forbearance ended.", "BNKCONCL", UserID
End Function

