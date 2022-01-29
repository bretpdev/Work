Attribute VB_Name = "CanRefPmtEntry"
Dim AccountNumber As String
Dim amount As Double
Dim EffDate As String
Dim CLID As String
Dim Reissue As String
Dim SeqNo As Integer
Dim aLnSeq(1) As Integer
Dim APL As String
Dim Bal As Double
Dim lender As String
Dim LenderID As String
Dim School As String
Dim LoanStatus As String
Dim name As String
Dim lname As String
Dim LnTyp As String
Dim LnTypType As String
Dim DisbDate As String
Dim DisbAmt As Double
Dim ChkAmt As Double
Dim CheckNo As String
Dim CheckReason As String
Dim CurrSchool As String
Dim PmtAmt As Double
Dim PmtCd As String
Dim aDisbDate(1 To 99) As String
Dim aDisbAmt(1 To 99) As String
Dim counter As Integer
Dim OrgLenderID As String
Dim LID As String
Dim PmtAmount As Double
Dim PmtCode As String
Dim Indy As String
Dim PCV As Double
Dim lSSN As String
Dim lTrNo As String
Dim lDate As String
Dim SUID As String
Dim oop As String
Dim FrstDisbDt As String
Dim GotTS26 As Boolean
Dim NewDisbDate As String
Dim NewDisbAmt As Double
Dim IsOnSystem As Boolean
Dim Voided As Boolean
Dim Stopped As Boolean
Dim ReasonF As String
Dim NonSysSSN As String
Dim appID As String
Dim AppSeq As String
Dim aAppSeq(1) As Integer
Dim ClosedY9Task As Boolean
Dim userId As String
Dim LateDisbDays As Integer

'Dim OwnerID As String
'Dim DisbNo As Integer
'Dim PrevRefAmt As Double
'Dim TotPmts As String
'Dim BatchNo As String
'Dim Targ As String


'create a posting file for cancel/refund payments
Sub CanRefInput()
    Dim row As Integer
    Dim aDateSeq() As String
    Dim dateSeq As String
    Dim ProcIt As Boolean
    Dim SUIDResult As Integer
    
    '''SuppressCompassCAM ("1")

    'In some cases, local variables with the same names as some public variables
    ' are not properly going out of scope. The following function call will clear
    ' the contents of those variables so that the previous record's information
    ' does not persist into this iteration.
    ResetPublicVars

    userId = Sp.Common.GetUserID
    
    Do
        Do
            'clear variables
            ssn = ""
            name = ""
            LnTyp = ""
            LnTypType = ""
            SeqNo = 0
            FrstDisbDt = ""
            GotTS26 = False
            ClosedY9Task = False

    'Prompt the user to enter the following information:
            'prompt the user for the SSN of unique ID
            SUID = InputBox("Enter the SSN or 19-character CommonLine unique ID.", "Enter SSN or Unique ID")
            If SUID = "" Then
                'prompt the user to print batch report or end
                If MsgBox("Do you want to print the CANCEL/REFUND PAYMENT BATCH report?", 36, "Print Report") = 6 Then
                    Okay = 3
                    PostingDetail
                    If MsgBox("Do you want to print the CANCEL/REFUND PAYMENT suspense report?", 36, "Print Report") = 6 Then
                        PrintSuspenseForm
                        End
                    Else
                        End
                    End If
                ElseIf MsgBox("Do you want to print the CANCEL/REFUND PAYMENT suspense report?", 36, "Print Report") = 6 Then
                    PrintSuspenseForm
                    End
                Else
                    End
                End If
            End If

            'convert to upper case since matching functions are case sensitive
            SUID = UCase(SUID)

            IsOnSystem = True
    'If the SSN is not entered but the CommonLine ID is
            If Len(SUID) > 9 Then
                FastPath "TX3ZIPO1N*"
                puttext 8, 41, SUID, "ENTER"
                If Not check4text(1, 75, "POX1S") Then
                    SUIDResult = MsgBox("The CommonLine unique ID entered was not found on PO1N.  Should the loan be on the system?  Click Yes to reenter the unique ID.  Click No to process the payment for a loan that is not on the system.  Click Cancel to quit.", vbYesNoCancel, "ID not Found")
                    If SUIDResult = vbNo Then
                        ssn = ""
                        NonSysSSN = "111111111"
                        IsOnSystem = False
                        Exit Do
                    ElseIf SUIDResult = vbCancel Then
                        End
                    End If
                Else
                    ssn = GetText(1, 9, 9)
                    appID = Replace(GetText(4, 46, 11), " ", "")
                    AppSeq = GetText(3, 77, 3)
                    aAppSeq(1) = AppSeq
    'Access TS26 page 2 for each loan until a match is found using CommonLine ID/Seq
                    If Not FindCLID Then
                        SUIDResult = MsgBox("The CommonLine unique ID entered was not found on PO1N.  Should the loan be on the system?  Click Yes to reenter the unique ID.  Click No to process the payment for a loan that is not on the system.  Click Cancel to quit.", vbYesNoCancel, "ID not Found")
                        If SUIDResult = vbNo Then
                            ssn = ""
                            NonSysSSN = "111111111"
                            IsOnSystem = False
                            Exit Do
                        ElseIf SUIDResult = vbCancel Then
                            End
                        End If
                    End If
                End If
            Else
                ssn = SUID
            End If

    'Access TX1J in inquire mode
            'get the borrower's name from TX1J unless the loan wasn't found
            If ssn <> "" Then
                If Sp.Common.GetTX1J(ssn, AccountNumber, lname, , name) Then
                    name = name & " " & lname
                    Exit Do
                Else
                    SUIDResult = MsgBox("The SSN entered was not found.  Should the borrower have loans on the system?  Click Yes to reenter the SSN.  Click No to process the payment for a loan that is not on the system.  Click Cancel to quit.", vbYesNoCancel, "SSN not Found")
                    If SUIDResult = vbNo Then
                        NonSysSSN = ssn
                        ssn = ""
                        IsOnSystem = False
                        Exit Do
                    ElseIf SUIDResult = vbCancel Then
                        End
                    End If
                End If
            End If
        Loop

        If IsOnSystem Then
    'Access TX6X in inquire mode for Y901
            'prompt the user to select a Y901 task to complete
            ReDim aDateSeq(0) As String
            aDateSeq(0) = "NONE"
            FastPath "TX3Z/ITX6XY901"
            If check4text(1, 74, "TXX71") Then
                row = 9
                While Not check4text(23, 2, "90007")
                    'add tasks for the borrower to the array
                    If check4text(row, 22, ssn) And check4text(row, 7, appID) Then
                        ReDim Preserve aDateSeq(UBound(aDateSeq) + 1) As String
                        aDateSeq(UBound(aDateSeq)) = GetText(row - 1, 6, 18)
                    End If
                    row = row + 3

                    'go to the next page
                    If row > 19 Then
                        hit "F8"
                        row = 9
                    End If
                Wend
            End If

    'Enter Payment Information
            'display the tasks found if there were any
            If UBound(aDateSeq) > 0 Then
                MsgBox "There is a CL change queue task for this borrower.  Please review the CL change report for the date/sequence numbers listed below and determine if any match the transaction you are posting. Press Insert when you are ready to continue."
                Sp.Q.PauseForInsert
                Load CanRefCLChng
                CanRefCLChng.lstDateSeq.List = aDateSeq
                CanRefCLChng.lstDateSeq.Value = "NONE"
                CanRefCLChng.Show
                dateSeq = CanRefCLChng.lstDateSeq.Value
                Unload CanRefCLChng

                'complete the selected task
                If dateSeq <> "NONE" Then
                    'find the task
                    FastPath "TX3Z/ITX6XY901"
                    row = 8
                    While Not check4text(row, 6, dateSeq)
                        row = row + 3
                        If row > 19 Then
                            hit "F8"
                            row = 8
                        End If
                    Wend

                    'select the task to be worked
                    puttext 21, 19, GetText(row, 4, 1), "ENTER"

                    'update something
                    puttext 6, 10, "560", "F6"
                    puttext 1, 4, "PO6L", "ENTER"
                    FindText "0231", 7, 2
                    puttext Session.FoundTextRow, Session.FoundTextColumn - 2, "X", "F6"

                    'find the task again
                    FastPath "TX3Z/ITX6XY901"
                    row = 8
                    While Not check4text(row, 75, "W")
                        row = row + 3
                        If row > 19 Then
                            hit "F8"
                            row = 8
                        End If
                    Wend

                    'complete the task
                    puttext 21, 19, GetText(row, 4, 1), "F2"
                    puttext 8, 19, "C", "ENTER"
                    ClosedY9Task = True
                End If
            End If
        Else
            ssn = NonSysSSN
            name = "Unknown"
            LnTyp = "STFFRD"
            SeqNo = "1"
        End If

        'prompt the user for payment information
        Okay = 0
        Load CanRefPmtInfo
        CanRefPmtInfo.ssn = ssn
        CanRefPmtInfo.BrwName = name
        CanRefPmtInfo.LnTyp = LnTyp
        If SeqNo <> 0 Then CanRefPmtInfo.SeqNo = SeqNo Else CanRefPmtInfo.SeqNo = ""
        CanRefPmtInfo.FrstDisbDt = FrstDisbDt
        CanRefPmtInfo.EffDate = Date

        CanRefPmtInfo.Show
        If Okay = 2 Then
            End
        ElseIf Okay = 3 Then
            PostingDetail
            End
        End If

        'get information from dialog box variables not already gathered from TS26 and TS2H
        amount = val(CanRefPmtInfo.TotPmtAmt)
        EffDate = CanRefPmtInfo.EffDate
        DateFormat EffDate
        If CanRefPmtInfo.reissu = True Then
            Reissue = "Y"
        Else
            Reissue = "N"
        End If
        CheckNo = CanRefPmtInfo.txtCheckNo
        CheckReason = CanRefPmtInfo.txtChkReason
        If CanRefPmtInfo.rdoStop Then Stopped = True Else Stopped = False
        If CanRefPmtInfo.rdoVoid Then Voided = True Else Voided = False

        NewDisbDate = Format(CanRefPmtInfo.txtNewDsbDt.Text, "MM/DD/YY")
        If CanRefPmtInfo.txtNewDsbAmt.Text = "" Then
            NewDisbAmt = 0
        Else
            NewDisbAmt = CDbl(CanRefPmtInfo.txtNewDsbAmt.Text)
        End If
        ReasonF = CanRefPmtInfo.txtReason.Text

        aLnSeq(1) = SeqNo
        If Sp.Common.IsLoanType(LnTyp, "FFEL") Then
            LnTypType = "FFEL"
            LateDisbDays = 120
        Else
            LnTypType = "Non-FFEL"
            LateDisbDays = 30
        End If
        
    'check for GRTCR activity record
        If LnTypType = "FFEL" Then
            'warn the user if a GRTCR activity record is found
            FastPath "LP50I" & ssn & ";;;;;GRTCR"
            'select all records
            If check4text(1, 58, "ACTIVITY SUMMARY SELECT") Then puttext 3, 13, "X", "ENTER"

            'review all records until an open one is found
            If check4text(1, 58, "ACTIVITY DETAIL DISPLAY") Then
                Do While Not check4text(22, 3, "46004")
                    'prompt the user to review the record if it is open
                    If check4text(8, 15, "MMDDCCYY") Then
                        MsgBox "There is at least open GRTCR activity record for this borrower.  Please review it to determine if the data is helpful in posting this transaction and then hit <Insert> to continue.", 48, "Review GRTCR Activity Record"
                        Sp.PauseForInsert
                        Exit Do
                    End If

                    'go to next record
                    hit "F8"
                Loop
            End If
        End If
        
        'suppress CAM 09 records
        If LnTypType = "FFEL" And ClosedY9Task And LoanStatus <> "DECONVERTED" Then SuppressCompassCAM "09"

    'For the designated disbursement determine how to allocate the payment based on the following criteria:
        'calculate payment and create posting record
        Indy = "Y" 'target flag
        ProcIt = True 'go to the Process Payment Subroutine
        
        'if the loan balance = 0
        If Bal = 0 And Not CanRefPmtInfo.rdoLoan Then
            If MsgBox("The loan has a zero balance and is in " & LoanStatus & " status.  Do you want to post the payment?  Click Yes to post the payment or click No to end the script.", vbYesNo + vbQuestion, "Zero Balance Loan") = vbNo Then End
        End If

        'if the Stop Payment or Voided Check radio button was selected
        If CanRefPmtInfo.rdoStop Or CanRefPmtInfo.rdoVoid Then
            PmtAmount = amount
            PmtCode = "45"
            WritePmt
        'if the Loan Not on System radio button was selected
        ElseIf CanRefPmtInfo.rdoLoan Then
            PmtAmount = amount
            AccountNumber = NonSysSSN
            Open "T:\pmtlistcr.txt" For Append As #1
            Write #1, NonSysSSN, "1", "STFFRD", EffDate, amount, Reissue, "828476", "828476", _
                      "00/00/0000", "0", "0", "40", "Unknown", "N", "", "0", "828476"
            Close #1
            GenerateSuspenseForm "loans not on system, review source of funds to return payment"
            ProcIt = False
        'if the Amount Received < Check Amount ITS2H
        ElseIf amount < ChkAmt Then
            PmtAmount = amount
            PmtCode = "40"
            If LoanStatus = "DECONVERTED" Or Bal = 0 Then Indy = "N"
            WritePmt
        'if the Amount Received = Check Amount ITS2H
        ElseIf amount = ChkAmt Then
            PmtAmount = amount
            'if Disb Date ITS2H <= latedisbdays days from Effective Date
            If DateValue(DisbDate) >= DateValue(EffDate) - LateDisbDays Then
                PmtCode = "45"
                If LoanStatus = "DECONVERTED" Or Bal = 0 Then Indy = "N"
            'if Disb Date ITS2H > latedisbdays days from Effective Date
            Else
                If Not Sp.Common.ATD22ByLoan(ssn, "LTCNL", SeqNo & ", " & amount & ", " & EffDate, aLnSeq(), "CXL/REF", "") Then
                    Sp.Common.ATD37ByLoan ssn, "LTCNL", SeqNo & ", " & amount & ", " & EffDate, aAppSeq(), userId, "CXL/REF"
                End If
                PmtCode = "40"
                If LoanStatus = "DECONVERTED" Or Bal = 0 Then Indy = "N"
            End If
            WritePmt
        'if the Amount Received > Check Amount ITS2H (no other specified scenario met)
        Else
            PmtAmount = ChkAmt
            'if Disb Date ITS2H <= latedisbdays days from Effective Date
            If DateValue(DisbDate) >= DateValue(EffDate) - LateDisbDays Then
                PmtCode = "45"
                If LoanStatus = "DECONVERTED" Or Bal = 0 Then Indy = "N"
            'if Disb Date ITS2H > latedisbdays days from Effective Date
            Else
                PmtCode = "40"
                If LoanStatus = "DECONVERTED" Or Bal = 0 Then Indy = "N"
            End If
            WritePmt
            
            'write line for overpay transaction
            PmtAmount = amount - ChkAmt
            PmtCode = "40"
            Indy = "N"
            WritePmt
        End If
        If ProcIt Then ProcessPayment 'And LoanStatus <> "DECONVERTED"  'Removed this as per Tram's request. SR 3003
        Unload CanRefPmtInfo
    Loop
End Sub

Sub ProcessPayment()
    With Session
        'replace original lender with new lender if original lender merged to a new lender
        FastPath "TX3Z/IPO3L" & OrgLenderID
        If Not check4text(17, 23, "_") Then OrgLenderID = Replace(GetText(17, 23, 8), "_", "")

        'access PO1N
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "PO1NI" & ssn
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'select the loan if the selection screen is displayed
        If .GetDisplayText(1, 75, 5) = "POX1P" Then
            row = 9
            Do Until .GetDisplayText(row, 41, 17) = Mid(CLID, 1, 17) And .GetDisplayText(row, 59, 2) = Mid(CLID, 18, 2)
                row = row + 1
                If .GetDisplayText(row, 3, 1) = " " Then
                    .TransmitTerminalKey rcIBMPf8Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    row = 9
                End If
            Loop
            .TransmitANSI .GetDisplayText(row, 2, 2)
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        End If
        .MoveCursor 2, 78
        .TransmitANSI "DI"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1

        'capture application sequence number
        APL = Replace(.GetDisplayText(4, 46, 11), " ", "")

        If Reissue = "Y" And Not Sp.Common.IsAffiliatedLenderCode(LenderID) Then
            'review each disbursement
            i = 0
            col = 30
            Do Until .GetDisplayText(11, col, 1) = "_" 'disb date is blank
            'get info if the status is active or active/reissue
                If .GetDisplayText(8, (col - 2), 12) = "      ACTUAL" Or .GetDisplayText(8, (col - 2), 12) = "REISSUE-ACTL" Then
                    i = i + 1
                    aDisbAmt(i) = .GetDisplayText(10, col - 2, 12)
                    aDisbDate(i) = .GetDisplayText(11, col, 2) & "/" & .GetDisplayText(11, col + 3, 2) & "/" & .GetDisplayText(11, col + 6, 4)
                End If
                col = col + 20
                'go to the next page
                If col > 70 Then
                    .TransmitTerminalKey rcIBMPf8Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    If .GetDisplayText(22, 2, Len("90007 NO MORE DATA TO DISPLAY")) = "90007 NO MORE DATA TO DISPLAY" Then Exit Do
                    col = 30
                End If
            Loop
        End If

        'add a queue task if the reissue indicator was selected and LenderID = "828476"
        'add queue tasks and activity records
        If Reissue = "Y" Then
            If LnTypType = "FFEL" Then
                'GCNCLREI
                If Sp.Common.IsAffiliatedLenderCode(LenderID) And OrgLenderID <> LenderID Then
                    Sp.Common.AddLP9O ssn, "GCNCLREI", , "SCHOOL:" & School & ", LOAN:" & LnTyp & ", APP:" & APL & ", CLUID:" & CLID & ", ORG DISB DT:" & DisbDate & ", ORG DISB AMT:" & DisbAmt
                    If Not Sp.Common.ATD22ByLoan(ssn, "PRRRQ", "Reissue Disbursement Denied.  Loan has been sold.  GCNCLREI Q task requested, " & School & ", " & LenderID & ", " & APL & ", " & DisbDate & ", " & DisbAmt & " - " & ReasonF, aLnSeq(), "CXL/REF", "") Then
                        Sp.Common.ATD37ByLoan ssn, "PRRRQ", "Reissue Disbursement Denied.  Loan has been sold.  GCNCLREI Q task requested, " & School & ", " & LenderID & ", " & APL & ", " & DisbDate & ", " & DisbAmt & " - " & ReasonF, aAppSeq(), userId, "CXL/REF"
                    End If
                'GREISSUE
                Else
                    Sp.Common.AddLP9O ssn, "GREISSUE", , "SCHOOL:" & School & ", LEND ID:" & LenderID & ", LOAN:" & LnTyp & ", APP:" & APL & ", CLUID:" & CLID & ", ORG DISB DT:" & DisbDate & ", ORG DISB AMT:" & DisbAmt & ", NEW DISB DATE:" & NewDisbDate & ", NEW DISB AMT:" & NewDisbAmt
                    If Not Sp.Common.ATD22ByLoan(ssn, "PRRRQ", "Reissue Disbursement requested in queue GREISSUE, " & School & ", " & LenderID & ", " & APL & ", " & DisbDate & ", " & DisbAmt & ", " & NewDisbDate & ", " & NewDisbAmt & " - " & ReasonF, aLnSeq(), "CXL/REF", "") Then
                        Sp.Common.ATD37ByLoan ssn, "PRRRQ", "Reissue Disbursement requested in queue GREISSUE, " & School & ", " & LenderID & ", " & APL & ", " & DisbDate & ", " & DisbAmt & ", " & NewDisbDate & ", " & NewDisbAmt & " - " & ReasonF, aAppSeq(), userId, "CXL/REF"
                    End If
                End If
            End If
            If Sp.Common.IsLoanType(LnTyp, "Private") Then
                If Not Sp.Common.ATD22ByLoan(ssn, "PRRRA", "SCHOOL:" & School & ", LEND ID:" & LenderID & ", LOAN:" & LnTyp & ", APP:" & APL & ", CLUID:" & CLID & ", ORG DISB DT:" & DisbDate & ", ORG DISB AMT:" & DisbAmt & ", NEW DISB DATE:" & NewDisbDate & ", NEW DISB AMT:" & NewDisbAmt, aLnSeq(), "CXL/REF", "") Then
                    Sp.Common.ATD37ByLoan ssn, "PRRRA", "SCHOOL:" & School & ", LEND ID:" & LenderID & ", LOAN:" & LnTyp & ", APP:" & APL & ", CLUID:" & CLID & ", ORG DISB DT:" & DisbDate & ", ORG DISB AMT:" & DisbAmt & ", NEW DISB DATE:" & NewDisbDate & ", NEW DISB AMT:" & NewDisbAmt, aAppSeq(), userId, "CXL/REF"
                End If
                If Not Sp.Common.ATD22ByLoan(ssn, "PRRRQ", "Reissue Disbursement requested in RR queue.  " & School & ", " & LenderID & ", " & APL & ", " & DisbDate & ", " & DisbAmt & ", " & NewDisbDate & ", " & NewDisbAmt & " - " & ReasonF, aLnSeq(), "CXL/REF", "") Then
                    Sp.Common.ATD37ByLoan ssn, "PRRRQ", "Reissue Disbursement requested in RR queue.  " & School & ", " & LenderID & ", " & APL & ", " & DisbDate & ", " & DisbAmt & ", " & NewDisbDate & ", " & NewDisbAmt & " - " & ReasonF, aAppSeq(), userId, "CXL/REF"
                End If
            End If
        End If

        If oop = "Y" And Reissue = "N" Then 'And APL <> 0 Then
            'select loan from PO6L
            .TransmitTerminalKey rcIBMClearKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "PO6LC" & ssn
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'if a selection screen is displayed then select the appropriate loan based off CLID
            If .GetDisplayText(1, 75, 5) = "POX6N" Then
                row = 8
                While .GetDisplayText(row, 59, 17) <> Mid(CLID, 1, 17)
                    row = row + 1
                    If row = 21 Then 'if at the bottom of the page then
                        .TransmitTerminalKey rcIBMPf8Key
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        row = 8
                    End If
                Wend
                'select the loan with the right CLID
                .TransmitANSI .GetDisplayText(row, 2, 2)
                .TransmitTerminalKey rcIBMEnterKey
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            End If
            MsgBox "Cancel the disbursements and hit Insert to continue.", vbOKOnly, "Cancel Disbursements"
            .WaitForTerminalKey rcIBMInsertKey, "1:0:0"
            .TransmitTerminalKey rcIBMInsertKey
        End If

        'add activity records for stop payment and voided check
        If Stopped Then
            If Not Sp.Common.ATD22ByLoan(ssn, "STOPD", "Stop disbursement check " & CheckNo & ", " & amount & ". reason:  " & CheckReason, aLnSeq(), "CXL/REF", "") Then
                Sp.Common.ATD37ByLoan ssn, "STOPD", "Stop disbursement check " & CheckNo & ", " & amount & ". reason:  " & CheckReason, aAppSeq(), userId, "CXL/REF"
            End If
        ElseIf Voided Then
            If Not Sp.Common.ATD22ByLoan(ssn, "VOIDD", "Voided disbursement check " & CheckNo & ", " & amount & ". reason:  " & CheckReason, aLnSeq(), "CXL/REF", "") Then
                Sp.Common.ATD37ByLoan ssn, "VOIDD", "Voided disbursement check " & CheckNo & ", " & amount & ". reason:  " & CheckReason, aAppSeq(), userId, "CXL/REF"
            End If
        End If
        
        'Suppress Compass CAM 10 record
        If Indy = "Y" And LnTypType = "FFEL" Then SuppressCompassCAM "10"

        'set the counter to indicate a record has been added to the posting file
        counter = 2
    End With
End Sub


Sub WritePmt()
    With Session
        If val(LenderID) = 829505 And PmtCode = "45" And _
           DateValue(DisbDate) <= DateValue("02/05/2003") Then
            LID = OrgLenderID
        'use the orig lender if the owner is in-house servicing
        ElseIf IsUHEAALender And PmtCode = "45" Then
            LID = OrgLenderID
        Else
            LID = LenderID
        End If
        Open "T:\pmtlistcr.txt" For Append As #1
        Write #1, ssn, SeqNo, LnTyp, EffDate, PmtAmount, Reissue, LID, School, _
            DisbDate, DisbAmt, ChkAmt, PmtCode, lname, Indy, CLID, PCV, OrgLenderID
        Close #1
    End With
End Sub

Function GenerateSuspenseForm(SuspenseReason As String)
    Dim userName() As String
''    Dim SuspDocPath As String
''    Dim ToPrinter As Boolean

    userName = Sp.Common.Sql("SELECT FirstName + ' ' + LastName AS UserName FROM SYSA_LST_Users WHERE WindowsUserName = '" & Sp.Common.WindowsUserName & "'")

''    SuspDocPath = "X:\PADD\Operational Accounting\"
''    If sp.TestMode(, SuspDocPath) Then ToPrinter = False Else ToPrinter = True
    Open "T:\suspformdat.txt" For Append As #1
    Write #1, "AccntNo", "Name", "Trans", "EffDate", "Amount", "Reason", "User", "Date"
    Write #1, AccountNumber, name, "10" & PmtCode, EffDate, PmtAmount, SuspenseReason, userName(1), Format(Date, "MM/DD/YYYY")
    Close #1
''    sp.Common.PrintDocs SuspDocPath, "SUSPFORM", "T:\suspformdat.txt", ToPrinter


End Function

Function PrintSuspenseForm()
    Dim SuspDocPath As String
    Dim ToPrinter As Boolean

    If Dir("T:\suspformdat.txt") <> "" Then
        If FileLen("T:\suspformdat.txt") > 0 Then
            SuspDocPath = "X:\PADD\Operational Accounting\"
            If Sp.TestMode(, SuspDocPath) Then ToPrinter = False Else ToPrinter = True
            Sp.Common.PrintDocs SuspDocPath, "SUSPFORM", "T:\suspformdat.txt", ToPrinter
            Exit Function
        End If
    End If
    MsgBox "There are no suspense items to print.", vbOKOnly + vbExclamation, "No Suspense Items"
    
End Function

Function PassOnSysStatus() As Boolean
    If IsOnSystem Then PassOnSysStatus = True Else PassOnSysStatus = False
End Function

'<23->
'find the unique ID on TS26 and get TS26 info
Function FindCLID() As Boolean
    Dim row As Integer

    FindCLID = True
    FastPath "TX3Z/ITS26" & ssn
    If check4text(1, 72, "TSX28") Then
        row = 8
        While Not check4text(23, 2, "90007")
            puttext 21, 12, Session.GetDisplayText(row, 2, 2), "ENTER"
            hit "ENTER"
            If GetText(8, 17, 17) & Format(GetText(8, 58, 2), "0#") = SUID Then
'</24->
'               Hit "F12"
'               GetTS26Info
                FrstDisbDt = GetText(4, 45, 8)
                hit "F12"
                SeqNo = val(GetText(7, 35, 4))
                LnTyp = GetText(6, 66, 6)
'</24>
                Exit Function
            End If
            
            hit "F12"
            hit "F12"
            row = row + 1
            
            If check4text(row, 3, " ") Then
                hit "F8"
                row = 8
            End If
        Wend
    ElseIf check4text(1, 72, "TSX29") Then
        hit "ENTER"
        If GetText(8, 17, 17) & Format(GetText(8, 58, 2), "0#") = SUID Then
'<24->
'           Hit "F12"
'           GetTS26Info
            FrstDisbDt = GetText(4, 45, 8)
            hit "F12"
            SeqNo = val(GetText(7, 35, 4))
            LnTyp = GetText(6, 66, 6)
'</24>
            Exit Function
        End If
    End If
    
    FindCLID = False
End Function

'get info from TS26
Function GetTS26Info()
    'get info from page1
    Bal = GetText(11, 12, 10)
    LoanStatus = GetText(3, 10, 20)
''    'warn the user and end if the balance is zero and the loan is not deconverted
''    If Bal = 0 And LoanStatus <> "DECONVERTED" Then
''        MsgBox "The loan balance equals zero.", 48, "Balance Equals Zero"
''        End
''    End If
    
    'get page 1 info
    SeqNo = val(GetText(7, 35, 4))
    CanRefPmtInfo.SeqNo = SeqNo
    lender = GetText(7, 59, 20) 'lender name
    LenderID = GetText(7, 48, 8) 'aka "Owner"
    LnTyp = GetText(6, 66, 6)
    CanRefPmtInfo.LnTyp = LnTyp
    
    'go to page 2
    hit "ENTER"
    FrstDisbDt = GetText(4, 45, 8)
    CanRefPmtInfo.FrstDisbDt = FrstDisbDt
    School = GetText(9, 40, 8)
    OrgLenderID = GetText(10, 40, 8)
    CLID = GetText(8, 17, 17) & Format(GetText(8, 58, 2), "0#")
End Function

'suppress CAM records
Function SuppressCompassCAM(camCode As String)
    Dim i As Integer
    
    Dim CamColumnTSX3K As Integer
    Dim CamColumnTSX3L As Integer
    Dim FoundLoanSeq As Boolean
    FoundLoanSeq = False
    
    CamColumnTSX3K = 10 ' default for SUID > 9
    CamColumnTSX3L = 41
    FastPath "TX3ZCTS1P" & ssn
        
    If Len(SUID) < 10 Then
        CamColumnTSX3K = 5
        CamColumnTSX3L = 16
        
        AppSeq = SeqNo '*Note that we use the variable AppSeq here but it is actually the user entered Loan Seq.
        If SeqNo < 10 Then
            AppSeq = "00" + AppSeq
        ElseIf SeqNo < 100 Then
            AppSeq = "0" + AppSeq
        End If
    End If
    
    'find the loan on the selection screen
    If check4text(1, 72, "TSX3K") Then
        Do While Not check4text(23, 2, "90007")
            For i = 8 To 20
                If check4text(i, CamColumnTSX3K, AppSeq) Then
                    puttext 21, 12, GetText(i, 2, 2), "ENTER"
                    Exit Do
                End If
            Next i
            hit "F8"
        Loop
    End If
    
    'hit F10 on the detail screen
    If check4text(1, 72, "TSX3L") And check4text(7, CamColumnTSX3L, AppSeq) Then
        FoundLoanSeq = True
        hit "F10"
        'find the loan on the selection screen
        If check4text(1, 72, "TSX40") Then
            Do While Not check4text(23, 2, "90007")
                For i = 11 To 20
                    If GetText(i, 3, 1) = "" Then Exit For
                    If DateValue(GetText(i, 18, 10)) = DateValue(DisbDate) And CDbl(GetText(i, 36, 10)) = CDbl(DisbAmt) Then
                        puttext 21, 12, GetText(i, 2, 2), "ENTER"
                        Exit Do
                    End If
                Next i
                hit "F8"
            Loop
        End If
        
        'suppress the record on the detail screen
        If check4text(1, 72, "TSX53") Then
            If DateValue(GetText(8, 16, 10)) = DateValue(DisbDate) And CDbl(GetText(8, 41, 10)) = CDbl(DisbAmt) Then
                For i = 13 To 22
                    If check4text(i, 5, camCode) Then
                        puttext i, 69, "S", "F6"
                        Exit For
                    End If
                Next i
            End If
        End If
    End If
    
    'warn the user and pause the script if there was an error.
    If Not check4text(23, 2, "01005") Then
        If FoundLoanSeq = False Then '*Note that we use the variable AppSeq here but it is actually the user entered Loan Seq.
            MsgBox "The requested loan seq number " + AppSeq + " was not found.", vbOKOnly + vbExclamation, "Loan SEQ not found."
        Else
            MsgBox "An error occurred while supressing the CAM record.  Review the record and hit Insert to continue.", vbOKOnly + vbExclamation, "CAM Supression Error"
            Sp.PauseForInsert
        End If
    End If
End Function

Function TS2H() As Boolean
    Dim col As Integer
    
    TS2H = False
    oop = ""
    col = 19
    While Not check4text(23, 2, "90007")
        If check4text(10, col, "ANTCPD") And GetText(13, col, 10) <> GetText(16, col, 10) Then oop = "Y"
        'check TS26 for the loan info if the disb date is found
        If (CanRefPmtInfo.DisbDt <> "" And GetText(15, col, 8) = Format(CanRefPmtInfo.DisbDt, "MM/DD/YY")) Or _
           (CanRefPmtInfo.DisbDt = "" And GetText(9, col, 2) = Format(val(CanRefPmtInfo.DisbNo), "0#")) Or _
           (check4text(9, col, "SI") And val(CanRefPmtInfo.DisbNo) = 1) Then
            FrstDisbDt = GetText(6, 17, 8)
            CanRefPmtInfo.FrstDisbDt = FrstDisbDt
            DisbDate = GetText(15, col, 8)
            CanRefPmtInfo.DisbDt = DisbDate
            DisbAmt = GetText(16, col, 10)
            ChkAmt = GetText(20, col, 10)
            CanRefPmtInfo.txtCheckAmt = ChkAmt
            CanRefPmtInfo.txtCheckNo2 = GetText(21, col, 10)
            CurrSchool = Sp.Q.GetText(13, 18, 8) '<26>
            If check4text(14, col, "          ") Then
                PCV = 0
            Else
                PCV = GetText(14, col, 10)
            End If
            TS2H = True
        End If
'<24->
'        col = col + 16
'        If col = 51 Then col = 52
'
'        'go to the next page
'        If col > 68 Then
'            Hit "F8"
'            col = 19
'        End If
        
        'go to next disb
        If col < 35 Then
            col = col + 16
        Else
            col = col + 17
        End If
                  
        'if past 4th disb on screen, try to page to next screen
        If col > 69 Then
            hit "F8"
            col = 19
        End If
'</24>
    Wend
End Function
'</23>

'<24->
''<23->
''determine if TS26 info was gathered
'Function HasGotTS26() As Boolean
'    HasGotTS26 = GotTS26
'End Function
''</23>
'</24>

'new  sr 151, jd, 11/01/02, 11/07/02
'<1>  sr 162, jd, 11/14/02, 11/15/02
'<2>  sr 164, jd, 11/14/02, 11/15/02
'<3>  sr 165, jd, 11/15/02, 11/18/02
'<4>  sr 211, mc, 02/03/03, 02/10/03
'<5>  sr 231, jd, 03/03/03, 03/27/03
'<6>  sr 250, jd, 03/27/03, 03/31/03
'<7>  sr 267, jd, 04/25/03, 05/06/03
'<8>  sr 286, jd, 05/05/03, 05/06/03
'<9>  sr 329, jd, 06/11/03, 06/30/03 changed \\Utahsbr1\APPS\Apps\FileTransfers\COMMON\OneLINK\OPA\ to X:\PADD\
'<10> sr 378, jd, 07/23/03, 09/22/03
'<11> sr 443, jd, 10/14/03, 10/28/03
'<12> sr 465, jd, 12/17/03, 02/03/04
'<13> sr 535, jd, 02/10/04, 02/17/04
'<14> sr 554, jd, 02/27/04, 03/04/04
'<15> sr 608, aa, 04/05/04, 04/17/04
'<16> sr 599, jd, 04/20/04, 04/28/04
'<17> sr 649, aa, 05/18/04, 05/19/04
'<18> sr 667, jd, 06/11/04, 06/11/04 updated code for changes to PO1N
'<19> sr 673, aa, 06/17/04, 06/17/04
'<20> sr 818, tp, 10/25/04, 12/02/04 added CLUID to queue task
'<21> sr1285, jd, all changes are in the form CanRefPmtInfo
'<22> sr1411, jd
'<23> sr1471, jd
'<24> sr1632, jd
'<25> sr1673, jd, added PLUSGB
'<26> sr1890, tp
'<27> sr1932, tp, 12/18/06
'<28> sr2051, jd
'<29> sr2216, db
'<30> sr2217, db

