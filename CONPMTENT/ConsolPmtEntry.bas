Attribute VB_Name = "ConsolPmtEntry"
Dim ssn As String
Dim TotAmtRcv As Currency
Dim WPA As Currency
Dim EffDate As String
Dim EffDateValue As Date
Dim InstID As String
Dim LenderID As String
Dim MultChk As Boolean
Dim iNum() As String
Dim iInstName1() As String
Dim iInstName2() As String
Dim iAddr1() As String
Dim iAddr2() As String
Dim iAddr3() As String
Dim Insti As Integer                'index number in array of institution

Dim AN As String
Dim fName As String
Dim MI As String
Dim lname As String
Dim FullName As String

Dim Maxi As Double
Dim i As Integer
Dim SeqNo() As String
Dim Disb1Dt() As String
Dim LonPgm() As String
Dim CurBal() As String
Dim Payoff() As Currency
Dim TotPayOff As Currency
Dim lender() As String
Dim PmtAmt As Currency
Dim CurPrin As String

Dim Proc As String
Dim Rec As Integer
Dim n As Integer
Dim X As Integer
Dim userName As String
Dim DocFolder As String
Dim ToPrinter As Boolean

Dim aSeqNo() As String
Dim aDisb1Dt() As String
Dim aLonPgm() As String
Dim aCurBal() As String
Dim aPayoff() As Currency
Dim aLender() As String
Dim xSeqNo() As String
Dim xDisb1Dt() As String
Dim xLonPgm() As String
Dim xCurBal() As String
Dim xPayOff() As Currency
Dim xLender() As String
Dim userId As String
Dim MasterLenderArr() As String ''Institution ID,Full Name,Address 1,Address 2,Address 3,City,Domestic State,ZIP,Foreign State,Country
Dim MasterLnSeqArr() As String 'LoanSeq,Payoff Amount,Daily Interest Amount,Loan Progam,Loan Status,Lender ID
Dim CPI As ConsolPmtInfo
Private Const TEMP_DIR As String = "T:\"

Sub ConsolPmtMain()
    ReDim iNum(0)
    ReDim iInstName1(0)
    ReDim iInstName2(0)
    ReDim iAddr1(0)
    ReDim iAddr2(0)
    ReDim iAddr3(0)
    ReDim aSeqNo(0)
    ReDim aDisb1Dt(0)
    ReDim aLonPgm(0)
    ReDim aCurBal(0)
    ReDim aPayoff(0)
    ReDim aLender(0)
    ReDim xSeqNo(0)
    ReDim xDisb1Dt(0)
    ReDim xLonPgm(0)
    ReDim xCurBal(0)
    ReDim xPayOff(0)
    ReDim xLender(0)
    ReDim SeqNo(0)
    ReDim Disb1Dt(0)
    ReDim LonPgm(0)
    ReDim CurBal(0)
    ReDim lender(0)
    ReDim Payoff(0)
    
    Dim PIF As String
    Dim Y As Integer
    
    Dim oneOfTheARCSWhereFound As Boolean
    
    'if batch file doesnt exist, create a blank one
    If Dir$(TEMP_DIR & "cnspmtbatch.txt") = "" Then
        Open TEMP_DIR & "cnspmtbatch.txt" For Append As #1
        Close #1
    End If
    
    'set print variables for test mode
    DocFolder = "X:\PADD\Compass\Payments\"
    If Sp.Common.TestMode(, DocFolder) Then ToPrinter = False Else ToPrinter = True
    'get user name from LP40
    FastPath "LP40I"
    If Not check4text(1, 77, "ANCE") Then
        hit "Enter"
    End If
    userName = StrConv(GetText(5, 14, 12) & " " & GetText(4, 14, 35), 3)
    userId = GetText(3, 14, 7)
    EnterConsolPmtInfo
    'get borrower info if SSN is valid, check for OneLINK account otherwise
    If Not GetTX1J(ssn, AN, lname, MI, fName) Then
        LC05Chk
        ConsolPmtMain
        End
    End If
    'format full name
    If MI <> "_" Then MI = MI & "." Else MI = "."
    If MI = "." Then
        FullName = fName & " " & lname
    Else
        FullName = fName & " " & MI & " " & lname
    End If
    'make sure bwr has open loans
    ITS26BalChk
    'Gather LnSeq from Arcs
    CancelPressed = False
    oneOfTheARCSWhereFound = ARCsFound()
    If oneOfTheARCSWhereFound Then
        If UBound(MasterLnSeqArr, 2) = 0 Then
            If MasterLnSeqArr(0, UBound(MasterLnSeqArr, 2)) = "" Then
                MsgBox "No loans selected for consolidation payment.", vbExclamation, "Consolidation Payment Entry"
                ConsolPmtMain
                End
            End If
        End If
        ITS26
    Else
        ITS26NoARCAlternative
    End If
    'if no loans were selected, end the script
    If UBound(MasterLnSeqArr, 2) = 0 Then
        If MasterLnSeqArr(0, UBound(MasterLnSeqArr, 2)) = "" Then
            MsgBox "No loans selected for consolidation payment.", vbExclamation, "Consolidation Payment Entry"
            ConsolPmtMain
            End
        End If
    End If
    'warn if only consolidation loans are selected
    If AllConsols Then
        MsgBox "There has already been a consolidation loan processed for this borrower.  Please review this account to determine whether or not to proceed with processing this payment.  Hit Insert to continue after you have reviewed the account.", 48, "Consolidation Loan Already Processed"
        PauseForInsert
        'end if the payment should not be processed
        If MsgBox("Do you want to process the payment?", 36, "Process Payment") = 7 Then End
    End If
    'calculate the payoff amounts for selected loans
    ITS2O
    'pay out the money as type 70
    Open TEMP_DIR & "cnspmtbatch.txt" For Append As #1
    For n = LBound(MasterLnSeqArr, 2) To UBound(MasterLnSeqArr, 2)
        If WPA >= CDbl(MasterLnSeqArr(1, n)) Then
            PmtAmt = CDbl(MasterLnSeqArr(1, n))
        Else
            PmtAmt = WPA
        End If
        If PmtAmt > 0 Then Write #1, ssn, PmtAmt, 70, EffDate, LenderID, CDbl(MasterLnSeqArr(0, n)), lname, PIF, MasterLenderArr(1, Insti)
        WPA = WPA - PmtAmt
        If WPA = 0 Then Exit For
    Next n
    Dim SuspenseTxt As String
    If Sp.Common.TestMode Then
        SuspenseTxt = "X:\PADD\Operational Accounting\Suspense\Test\Suspense.txt"
    Else
        SuspenseTxt = "X:\PADD\Operational Accounting\Suspense\Suspense.txt"
    End If
    Dim atLeastOneUheaaAffiliatedLoanIsDeconverted As Boolean
    atLeastOneUheaaAffiliatedLoanIsDeconverted = False
    For n = LBound(MasterLnSeqArr, 2) To UBound(MasterLnSeqArr, 2)
        If MasterLnSeqArr(5, n) = "DECONVERTED" Then
            If Sp.Common.IsAffiliatedLenderCode(MasterLnSeqArr(6, n)) Then
                atLeastOneUheaaAffiliatedLoanIsDeconverted = True
                Exit For
            End If
        End If
    Next n
    'if money is left over
    If WPA > 0 And atLeastOneUheaaAffiliatedLoanIsDeconverted = False Then
        PmtAmt = WPA
        'write transaction to the last loan as type 40 and print refund letter if >= $5
        If WPA >= 5 Then
            Write #1, ssn, PmtAmt, 40, EffDate, LenderID, MasterLnSeqArr(0, Maxi), lname, PIF, MasterLenderArr(1, Insti)
            Open SuspenseTxt For Append As #9
                Write #9, ssn, Format(CDbl(PmtAmt), "0.00"), "1040", EffDate, LenderID
            Close #9
            'write info to data file for refund letter
            Open TEMP_DIR & "clref.txt" For Output As #2
            Write #2, "StaticCurrentDate", "Name1", "Address1", "Address2", "Address3", "Borrower", "SSN", "ChkAmt", "RecDate"
            
            A1 = MasterLenderArr(2, Insti)
            A2 = MasterLenderArr(3, Insti) & " " & MasterLenderArr(4, Insti)
            A3 = MasterLenderArr(5, Insti) & ", " & MasterLenderArr(6, Insti) & " " & MasterLenderArr(7, Insti)
            Write #2, Format(Date, "mmmm dd, yyyy"), MasterLenderArr(1, Insti), A1, A2, A3, FullName, "XXX-XX" & Right(ssn, 4), Format(PmtAmt, "###,###,##0.00"), EffDate
            Write #2, Format(Date, "mmmm dd, yyyy"), MasterLenderArr(1, Insti), A1, A2, A3, FullName, "XXX-XX" & Right(ssn, 4), Format(PmtAmt, "###,###,##0.00"), EffDate
            Close #2
            PrintDocs DocFolder, "CLREF", TEMP_DIR & "clref.txt", ToPrinter
            Kill TEMP_DIR & "clref.txt"
        'write transaction to the last loan as type 70 if <$5
        Else
            Write #1, ssn, PmtAmt, 70, EffDate, LenderID, MasterLnSeqArr(0, Maxi), lname, PIF, MasterLenderArr(1, Insti)
        End If

        WPA = WPA - PmtAmt
    End If
    Close #1
    
    'if (TotAmtRcv < TotPayOff - 10) then create the consolidation letter
    If (TotAmtRcv < TotPayOff - 10) Then
        Dim Unpaid As Currency
        Unpaid = TotPayOff - TotAmtRcv
        
        'write info out to merge file cnsdat.txt
        Open TEMP_DIR & "cnsdat.txt" For Output As #1
        Write #1, "StaticCurrentDate", "InstName1", "Addr1", "Addr2", "Addr3", _
            "FullName", "SSN", "Date", "TotAmtRcv", "Unpaid", "EffDate"
        A1 = MasterLenderArr(2, Insti)
        A2 = MasterLenderArr(3, Insti) & " " & MasterLenderArr(4, Insti)
        A3 = MasterLenderArr(5, Insti) & ", " & MasterLenderArr(6, Insti) & " " & MasterLenderArr(7, Insti)
        Write #1, Format(Date, "mmmm dd, yyyy"), MasterLenderArr(1, Insti), A1, A2, A3, _
            FullName, "XXX-XX" & Right(ssn, 4), Format(Date, "MM/DD/YYYY"), _
            Format(TotAmtRcv, "$###,##0.00"), _
            Format(Unpaid, "$###,##0.00"), Format(EffDate, "MM/DD/YYYY")
        Write #1, Format(Date, "mmmm dd, yyyy"), MasterLenderArr(1, Insti), A1, A2, A3, _
            FullName, "XXX-XX" & Right(ssn, 4), Format(Date, "MM/DD/YYYY"), _
            Format(TotAmtRcv, "$###,##0.00"), _
            Format(Unpaid, "$###,##0.00"), Format(EffDate, "MM/DD/YYYY")
        Close #1
        'create a new letter
        PrintDocs DocFolder, "consolletter", TEMP_DIR & "cnsdat.txt", ToPrinter
        Kill TEMP_DIR & "cnsdat.txt"
        'Get an array of loan sequence numbers from MasterLnSeqArr that exist on TD22.
        Dim seqArr() As Integer
        ReDim seqArr(0)
        Dim seqRow As Integer
        For n = LBound(MasterLnSeqArr, 2) To UBound(MasterLnSeqArr, 2)
            FastPath "TX3Z/ATD22" & ssn & "PCON1"
            Do While Not check4text(23, 2, "90007")
                For seqRow = 11 To 18
                    If check4text(seqRow, 5, "   ") Then
                        Exit Do
                    ElseIf CInt(val(GetText(seqRow, 5, 3))) = CInt(MasterLnSeqArr(0, n)) Then
                        seqArr(UBound(seqArr)) = CInt(MasterLnSeqArr(0, n))
                        ReDim Preserve seqArr(UBound(seqArr) + 1)
                        Exit Do
                    End If
                Next seqRow
                hit "F8"
                seqRow = 11
            Loop
        Next n
        ReDim Preserve seqArr(UBound(seqArr) - 1)
        If Sp.Common.ATD22ByLoan(ssn, "PCON1", "", seqArr, "CONPMTENT", userId) = False Then
            MsgBox "You need access to the ""PCON1"" ARC.  Please contact Systems Support."
            End
        End If
    End If

    ConsolPmtMain
End Sub

Public Sub SetMasterLenderArr(arr() As String)
    MasterLenderArr = arr
End Sub

Function ARCsFound() As Boolean
    'Gather LoanSeq from ARCs
    Dim str1 As String
    Dim Arr1() As String
    Dim ARCArr1() As String
    Dim X As Integer

    ARCsFound = True 'assume that one of the arcs will be found

    'Search for Consolidation Ineligibility Codes and print returned funds letters if found
    ReDim MasterLnSeqArr(6, 0)
    FastPath "TX3Z/ITD2A" & ssn
    puttext 11, 65, "DSLVC", "ENTER"
    If check4text(23, 2, "01019 ENTERED KEY NOT FOUND") = False Then
        If check4text(5, 75, "01") = False Then
            MsgBox "Multiple Arcs were found. Please select one and press insert."
            Sp.Q.PauseForInsert
        End If
        ParseComment
    End If
    
    FastPath "TX3Z/ITD2A" & ssn
    puttext 11, 65, "DFDLC", "ENTER"
    If check4text(23, 2, "01019 ENTERED KEY NOT FOUND") = False Then
        If check4text(5, 75, "01") = False Then
            MsgBox "Multiple Arcs were found. Please select one and press insert."
            Sp.Q.PauseForInsert
        End If
        If UBound(MasterLnSeqArr, 2) > 0 Then
            If MsgBox("There was an arc found for ""DSLVC"" and ""DFDLC"".  Do you want to use ""DSLVC""?  Press YES to use ""DSLVC"".  Press NO to use ""DFDLC"".", vbYesNo) = vbYes Then
                'yes use DSLVC
                ReDim Preserve MasterLnSeqArr(6, UBound(MasterLnSeqArr, 2) - 1)
                Exit Function
            Else
                'no use DFDLC
                ReDim MasterLnSeqArr(6, 0)
            End If
        End If
        ParseComment
    End If
        
        
    If UBound(MasterLnSeqArr, 2) = 0 Then
        If MasterLnSeqArr(0, UBound(MasterLnSeqArr, 2)) = "" Then
            ARCsFound = False 'neither arc was found
            Exit Function
        End If
    End If
    ReDim Preserve MasterLnSeqArr(6, UBound(MasterLnSeqArr, 2) - 1)
End Function

Sub EnterConsolPmtInfo()
    Do
        Okay = 0
        warn = 0
        ConsolPmtInfo.Show
        'quit if the OK button was not clicked
        If Okay <> 1 Then
            'Unload ConsolPmtInfo
            End
        End If
        'gather values from dialog box variables
        ssn = ConsolPmtInfo.bwrssn
        TotAmtRcv = val(ConsolPmtInfo.TotAmtRcv.Text)
        MultChk = ConsolPmtInfo.chkMultBrwChk
        effdate1 = ConsolPmtInfo.effdate1
        effdate2 = ConsolPmtInfo.effdate2
        effdate3 = ConsolPmtInfo.effdate3
        LenderID = ConsolPmtInfo.LenderID.Text
        'end the script if no SSN was entered or the dialog box was canceled
        If ssn = "" Then
            End
        'ensure ssn has nine characters
        ElseIf Len(ssn) < 9 Then
            warn = MsgBox("The borrower SSN must contain 9 characters.  Click OK to re-enter the information.", _
            vbExclamation, "Invalid Entry")
            GoTo 99
        'ensure amount received is a positive value
        ElseIf TotAmtRcv <= 0 Then
            warn = MsgBox("The amount received must be greater than zero.  Click OK to re-enter the information.", _
            vbExclamation, "Invalid Entry")
            GoTo 99
        End If
        
        'check effective date for valid date
        If IsDate((effdate1 + "/" + effdate2 + "/" + effdate3)) = False Then
            warn = MsgBox("Invalid effective date entered.  Click OK to re-enter the information in MM/DD/YYYY format.", _
            vbExclamation, "Invalid Entry")
            GoTo 99
        Else
            EffDateValue = DateValue(effdate1 + "/" + effdate2 + "/" + effdate3)
        End If
        
        For m = LBound(MasterLenderArr, 2) To UBound(MasterLenderArr, 2)
            If ConsolPmtInfo.LenderID.Text = MasterLenderArr(0, m) Then
                Insti = m
                Exit For
            End If
        Next m
        
        If EffDateValue < #6/1/1990# Or EffDateValue > Date Then
            warn = MsgBox("Invalid effective date entered.  Click OK to re-enter the information in MM/DD/YYYY format.", _
            vbExclamation, "Invalid Entry")
        Else
            If warn = 0 Then
            Exit Do
            End If
        End If
99  Loop
    'Unload ConsolPmtInfo
    ConsolPmtInfo.bwrssn.Text = ""
    ConsolPmtInfo.TotAmtRcv.Text = ""
    ConsolPmtInfo.effdate1.Text = ""
    ConsolPmtInfo.effdate2.Text = ""
    ConsolPmtInfo.LenderID.Text = ""
    ConsolPmtInfo.chkMultBrwChk = False
        
    WPA = TotAmtRcv
    EffDate = Format(EffDateValue, "MM/DD/YY")
End Sub

'calculates total current balance for borrower on IST26
Function ITS26BalChk() As Boolean
    Dim row As Integer
    Dim CurBal As Currency
    TotCurBal = 0
    
    FastPath "TX3Z/ITS26" & ssn
    
    'if multiple loans on ITS26
    If check4text(1, 72, "TSX28") Then
        'determine borrowers total current balance
        Do Until check4text(23, 2, "90007")
            row = 8
            Do Until val(GetText(row, 14, 4)) = 0
                TotCurBal = TotCurBal + GetText(row, 59, 10)
                row = row + 1
            Loop
            hit "F8"
        Loop
    'if single loan on ITS26
    ElseIf check4text(1, 72, "TSX29") Then
        TotCurBal = GetText(11, 12, 10)
    End If
        
    If TotCurBal = 0 Then
        If MsgBox("Borrower has no open loans, do you want to still post the funds?", vbExclamation + vbYesNo, "Consolidation Payment Entry") = vbYes Then
            'pay out the money as type 70
            Open TEMP_DIR & "cnspmtbatch.txt" For Append As #1
            Write #1, ssn, WPA, 70, EffDate, LenderID, "", lname, "", MasterLenderArr(1, Insti)
            Close #1
            ConsolPmtMain
        Else
            End
        End If
    End If
    
    ITS26BalChk = True
End Function

'identify eligible loans and write them into an array (no ARC found to gather seq numbers from)
Sub ITS26NoARCAlternative()
    Dim row As Integer

    'MasterLnSeqArr - LoanSeq,Payoff Amount,Daily Interest Amount,Loan Progam,Loan Status,Lender ID
    FastPath "TX3Z/ITS26" & ssn
    If check4text(1, 72, "TSX28") Then
        'selection screen
        row = 8
        While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            If CDbl(GetText(row, 59, 10)) > 0 And check4text(row, 69, "CR") = False Then 'if balance > 0
                'select loan
                puttext 21, 12, Format(GetText(row, 2, 2), "00"), "Enter"
                'start collecting information for the loan
                MasterLnSeqArr(0, UBound(MasterLnSeqArr, 2)) = CInt(GetText(7, 35, 4)) 'loan sequence number
                MasterLnSeqArr(1, UBound(MasterLnSeqArr, 2)) = GetText(11, 12, 10) 'current principal/payoff amount
                MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = GetText(6, 66, 8)
                'Set the value to sort by
                If MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "UNSTFD" Then
                    MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 1
                ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "STFFRD" Then
                    MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 2
                ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "PLUS" Then
                    MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 3
                ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "PLUSGB" Then
                    MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 3
                ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "SUBCNS" Then
                    MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 4
                ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "UNCNS" Then
                    MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 5
                ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "SUBSPC" Then
                    MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 6
                ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "UNSPC" Then
                    MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 7
                Else
                    MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 10
                End If
                
                If check4text(3, 10, "IN SCHOOL") Then
                    MasterLnSeqArr(5, UBound(MasterLnSeqArr, 2)) = "IN SCHOOL"
                ElseIf check4text(3, 11, "DECONVERTED") Then
                    MasterLnSeqArr(5, UBound(MasterLnSeqArr, 2)) = "DECONVERTED"
                    MasterLnSeqArr(6, UBound(MasterLnSeqArr, 2)) = GetText(7, 48, 6)
                Else
                    MasterLnSeqArr(5, UBound(MasterLnSeqArr, 2)) = ""
                End If
                hit "Enter"
                MasterLnSeqArr(2, UBound(MasterLnSeqArr, 2)) = GetText(15, 65, 10)
                hit "F12"
                hit "F12"
                'add one to the array
                ReDim Preserve MasterLnSeqArr(6, UBound(MasterLnSeqArr, 2) + 1)
            End If
            row = row + 1
            If check4text(row, 3, " ") Then
                row = 8
                hit "F8"
            End If
        Wend
    Else
        'target screen
        If CDbl(GetText(11, 12, 10)) > 0 And check4text(11, 22, "CR") = False Then 'if balance > 0
            'start collecting information for the loan
            MasterLnSeqArr(0, UBound(MasterLnSeqArr, 2)) = CInt(GetText(7, 35, 4)) 'loan sequence number
            MasterLnSeqArr(1, UBound(MasterLnSeqArr, 2)) = GetText(11, 12, 10) 'current principal/payoff amount
            MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = GetText(6, 66, 8)
            'Set the value to sort by
            If MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "UNSTFD" Then
                MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 1
            ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "STFFRD" Then
                MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 2
            ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "PLUS" Then
                MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 3
            ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "PLUSGB" Then
                MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 3
            ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "SUBCNS" Then
                MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 4
            ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "UNCNS" Then
                MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 5
            ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "SUBSPC" Then
                MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 6
            ElseIf MasterLnSeqArr(3, UBound(MasterLnSeqArr, 2)) = "UNSPC" Then
                MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 7
            Else
                MasterLnSeqArr(4, UBound(MasterLnSeqArr, 2)) = 10
            End If
            
            If check4text(3, 10, "IN SCHOOL") Then
                MasterLnSeqArr(5, UBound(MasterLnSeqArr, 2)) = "IN SCHOOL"
            ElseIf check4text(3, 11, "DECONVERTED") Then
                MasterLnSeqArr(5, UBound(MasterLnSeqArr, 2)) = "DECONVERTED"
                MasterLnSeqArr(6, UBound(MasterLnSeqArr, 2)) = GetText(7, 48, 6)
            Else
                MasterLnSeqArr(5, UBound(MasterLnSeqArr, 2)) = ""
            End If
            hit "Enter"
            MasterLnSeqArr(2, UBound(MasterLnSeqArr, 2)) = GetText(15, 65, 10)
            'add one to the array
            ReDim Preserve MasterLnSeqArr(6, UBound(MasterLnSeqArr, 2) + 1)
        End If
    End If
    
    
    'Allow the user to 'de-select' any loan by double clicking on the loan
    frmLPPLoanSelection.setData MasterLnSeqArr
    frmLPPLoanSelection.Show
    MasterLnSeqArr = frmLPPLoanSelection.getData
    'MasterLnSeqArr = frmLPPLoanSelection.MasterLnSeqArr
    If frmLPPLoanSelection.getCanceled Then
        ReDim MasterLnSeqArr(6, 0)
        Exit Sub
    End If
    
    'Bubble Sort The MasterLnSeqArr array
    Dim tempArr(4)
    For X = LBound(MasterLnSeqArr, 2) To UBound(MasterLnSeqArr, 2) - 1
        For xx = X + 1 To UBound(MasterLnSeqArr, 2)
            If MasterLnSeqArr(4, xx) < MasterLnSeqArr(4, X) Then
                tempArr(0) = MasterLnSeqArr(0, X)
                tempArr(1) = MasterLnSeqArr(1, X)
                tempArr(2) = MasterLnSeqArr(2, X)
                tempArr(3) = MasterLnSeqArr(3, X)
                tempArr(4) = MasterLnSeqArr(4, X)
                
                MasterLnSeqArr(0, X) = MasterLnSeqArr(0, xx)
                MasterLnSeqArr(1, X) = MasterLnSeqArr(1, xx)
                MasterLnSeqArr(2, X) = MasterLnSeqArr(2, xx)
                MasterLnSeqArr(3, X) = MasterLnSeqArr(3, xx)
                MasterLnSeqArr(4, X) = MasterLnSeqArr(4, xx)
                
                MasterLnSeqArr(0, xx) = tempArr(0)
                MasterLnSeqArr(1, xx) = tempArr(1)
                MasterLnSeqArr(2, xx) = tempArr(2)
                MasterLnSeqArr(3, xx) = tempArr(3)
                MasterLnSeqArr(4, xx) = tempArr(4)
            End If
        Next xx
    Next X
End Sub

'identify eligible loans and write them into an array
Sub ITS26()
    Dim X As Integer
    Dim xx As Integer
    For X = LBound(MasterLnSeqArr, 2) To UBound(MasterLnSeqArr, 2)
        FastPath "TX3Z/ITS26" & ssn
        If check4text(1, 72, "TSX28") Or check4text(1, 72, "TSX29") Then
            If check4text(1, 72, "TSX28") Then
                'selection screen
                Do While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    For xx = 8 To 20
                        If check4text(xx, 14, Format(MasterLnSeqArr(0, X), "0000")) Then
                            puttext 21, 12, Format(Sp.Q.GetText(xx, 2, 2), "00"), "ENTER"
                            Exit Do
                        End If
                    Next xx
                    hit "F8"
                Loop
            End If
            'Target screen
            If check4text(7, 35, Format(MasterLnSeqArr(0, X), "0000")) Then
                MasterLnSeqArr(3, X) = GetText(6, 66, 8)
                'Set the value to sort by
                If MasterLnSeqArr(3, X) = "UNSTFD" Then
                    MasterLnSeqArr(4, X) = 1
                ElseIf MasterLnSeqArr(3, X) = "STFFRD" Then
                    MasterLnSeqArr(4, X) = 2
                ElseIf MasterLnSeqArr(3, X) = "PLUS" Then
                    MasterLnSeqArr(4, X) = 3
                ElseIf MasterLnSeqArr(3, X) = "PLUSGB" Then
                    MasterLnSeqArr(4, X) = 3
                ElseIf MasterLnSeqArr(3, X) = "SUBCNS" Then
                    MasterLnSeqArr(4, X) = 4
                ElseIf MasterLnSeqArr(3, X) = "UNCNS" Then
                    MasterLnSeqArr(4, X) = 5
                ElseIf MasterLnSeqArr(3, X) = "SUBSPC" Then
                    MasterLnSeqArr(4, X) = 6
                ElseIf MasterLnSeqArr(3, X) = "UNSPC" Then
                    MasterLnSeqArr(4, X) = 7
                Else
                    MasterLnSeqArr(4, X) = 10
                End If
                
                If check4text(3, 10, "IN SCHOOL") Then
                    MasterLnSeqArr(5, X) = "IN SCHOOL"
                ElseIf check4text(3, 11, "DECONVERTED") Then
                    MasterLnSeqArr(5, X) = "DECONVERTED"
                    MasterLnSeqArr(6, X) = GetText(7, 48, 6)
                Else
                    MasterLnSeqArr(5, X) = ""
                End If
            End If
        End If
    Next X
    
    'Allow the user to 'de-select' any loan by double clicking on the loan
    frmLPPLoanSelection.setData MasterLnSeqArr
    frmLPPLoanSelection.Show
    MasterLnSeqArr = frmLPPLoanSelection.getData
    'MasterLnSeqArr = frmLPPLoanSelection.MasterLnSeqArr
    If frmLPPLoanSelection.getCanceled Then
        ReDim MasterLnSeqArr(6, 0)
        Exit Sub
    End If
    
    'Bubble Sort The MasterLnSeqArr array
    Dim tempArr(4)
    For X = LBound(MasterLnSeqArr, 2) To UBound(MasterLnSeqArr, 2) - 1
        For xx = X + 1 To UBound(MasterLnSeqArr, 2)
            If MasterLnSeqArr(4, xx) < MasterLnSeqArr(4, X) Then
                tempArr(0) = MasterLnSeqArr(0, X)
                tempArr(1) = MasterLnSeqArr(1, X)
                tempArr(2) = MasterLnSeqArr(2, X)
                tempArr(3) = MasterLnSeqArr(3, X)
                tempArr(4) = MasterLnSeqArr(4, X)
                
                MasterLnSeqArr(0, X) = MasterLnSeqArr(0, xx)
                MasterLnSeqArr(1, X) = MasterLnSeqArr(1, xx)
                MasterLnSeqArr(2, X) = MasterLnSeqArr(2, xx)
                MasterLnSeqArr(3, X) = MasterLnSeqArr(3, xx)
                MasterLnSeqArr(4, X) = MasterLnSeqArr(4, xx)
                
                MasterLnSeqArr(0, xx) = tempArr(0)
                MasterLnSeqArr(1, xx) = tempArr(1)
                MasterLnSeqArr(2, xx) = tempArr(2)
                MasterLnSeqArr(3, xx) = tempArr(3)
                MasterLnSeqArr(4, xx) = tempArr(4)
            End If
        Next xx
    Next X
End Sub

Sub ITS2O()
    Dim X As Integer
    
    'get total payoff amount
    TotPayOff = 0
    For n = LBound(MasterLnSeqArr, 2) To UBound(MasterLnSeqArr, 2)
        FastPath "TX3Z/ITS2O" & ssn
        puttext 7, 26, Mid(EffDate, 1, 2) & Mid(EffDate, 4, 2) & Mid(EffDate, 7, 2)
        puttext 9, 54, "Y", "ENTER"
        Do While check4text(23, 2, "90007") = False
            For X = 13 To 22
                 If check4text(X, 16, Format(MasterLnSeqArr(0, n), "000")) And val(GetText(X, 56, 12)) > 0 Then
                    puttext X, 2, "X"
                    Exit Do
                End If
            Next X
            hit "F8"
        Loop
        hit "ENTER"
        hit "ENTER"
        'If the loan sequence was found and selected, we'll be on TSX2Q at this point.
        If check4text(1, 72, "TSX2Q") Then
            MasterLnSeqArr(1, n) = Replace(GetText(12, 24, 16), ",", "")
            TotPayOff = TotPayOff + CDbl(MasterLnSeqArr(1, n))
        End If
    Next n
End Sub

Sub BatchDetail()
    Dim ssn As String
    Dim PmtAmt As String
    Dim PmtCode As String
    Dim EffDate As String
    Dim LenderID As String
    Dim SeqNox As String
    Dim lname As String
    Dim PIF As String
    Dim instName As String
    
    'if batch file doesnt exist, notify the user
    If Dir$(TEMP_DIR & "cnspmtbatch.txt") = "" Then
        MsgBox "No batch currently exists.", vbOKOnly, "Consolidation Payment Entry"
        ConsolPmtMain
    End If

    TotPmtAmt = 0

    'activate Excel application
    Dim ExcelApp As excel.Application
    Set ExcelApp = CreateObject("Excel.Application")
    ExcelApp.Visible = True
    ExcelApp.Workbooks.Open FileName:=DocFolder & "CnsPmtBatch.xls"
    'Today's Date
    ExcelApp.Range("B2").Select
    ExcelApp.ActiveCell.FormulaR1C1 = Date
    
    row = 7
    
    Open TEMP_DIR & "cnspmtbatch.txt" For Input As #1
        Do Until EOF(1)
            Input #1, ssn, PmtAmt, PmtCode, EffDate, LenderID, SeqNox, lname, PIF, instName
            'total payment amount for the batch
            TotPmtAmt = TotPmtAmt + PmtAmt
            
            'borrower ssn
            ExcelApp.Range("A" & row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = ssn
            'total payment amount
            ExcelApp.Range("B" & row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = PmtAmt
            'payment code for payment
            ExcelApp.Range("C" & row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = PmtCode
            'effective date for payment
            ExcelApp.Range("D" & row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = EffDate
            'lender id
            ExcelApp.Range("E" & row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = LenderID
            'sequence numbe for loan
            ExcelApp.Range("F" & row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = SeqNox
            'bwrs last name
            ExcelApp.Range("G" & row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = lname
            'PIF indicator
            ExcelApp.Range("H" & row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = PIF
            'institution name
            ExcelApp.Range("I" & row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = instName
            
            row = row + 1
        Loop
    Close #1

    'print the sum of all payment amounts
    ExcelApp.Range("A" & row + 1).Select
    ExcelApp.ActiveCell.FormulaR1C1 = "Total Payment Amount For This Batch:"

    ExcelApp.Range("E" & row + 1).Select
    ExcelApp.selection.NumberFormat = "$#,##0.00"
    ExcelApp.ActiveCell.FormulaR1C1 = TotPmtAmt
End Sub

Sub LC05Chk()
    FastPath "LC05I" & ssn
    If check4text(1, 70, "CLAIM RECAP") Then
        MsgBox "The borrower is not on COMPASS but has loans in postclaim collections.  Please forward the payment to LGP.", 48, "Consolidation Payment Entry"
    Else
        MsgBox "The borrower is not on COMPASS and does not have loans in postclaim collections.  Please research this payment to determine if a data entry error was made or if this payment needs to be returned to the sender.", 48, "Consolidation Payment Entry"
    End If
End Sub

'determine if there are any non-consolidation loans
Function AllConsols() As Boolean
    AllConsols = True
    For j = LBound(MasterLnSeqArr, 2) To UBound(MasterLnSeqArr, 2)
        If MasterLnSeqArr(3, j) <> "SUBCNS" And MasterLnSeqArr(3, j) <> "UNCNS" And MasterLnSeqArr(3, j) <> "SUBSPC" And MasterLnSeqArr(3, j) <> "UNSPC" Then
            AllConsols = False
            Exit Function
        End If
    Next j
End Function

'process returned funds letters
Function LetterProc(LtrID As String, ARC As String, comment As String)
    Dim ix As Integer
    'print letter
    Open TEMP_DIR & LtrID & "dat.txt" For Output As #2
    Write #2, "Consolidating_Lender", "Address1", "Address2", "Address3", "City", "State", "ForeignState", "Zip", "Country", "Borrower", "SSN"
    Write #2, MasterLenderArr(1, Insti), MasterLenderArr(2, Insti), MasterLenderArr(3, Insti), MasterLenderArr(4, Insti), MasterLenderArr(5, Insti), MasterLenderArr(6, Insti), MasterLenderArr(8, Insti), MasterLenderArr(7, Insti), MasterLenderArr(9, Insti), FullName, "XXX-XX-" & Right(ssn, 4)
    Close #2
    For ix = 1 To 2
        PrintDocs DocFolder, LtrID, TEMP_DIR & LtrID & "dat.txt", ToPrinter
    Next
    Kill TEMP_DIR & LtrID & "dat.txt"
    
    'add activity record
    Sp.Common.ATD22AllLoans ssn, ARC, comment, "CONPMTENT", userId

    'prompt the user to pull the check or add the check to the batch so it will go to suspense
    If MultChk = False Then
        MsgBox "The loan cannot be consolidated.  Please pull the check.", vbExclamation, "Pull Check"
    Else
        Open TEMP_DIR & "cnspmtbatch.txt" For Append As #1
        Write #1, ssn, CStr(TotAmtRcv), 40, EffDate, LenderID, 0, lname, PIF, MasterLenderArr(1, Insti)
        Close #1
    End If
    
    ConsolPmtMain 'show form again (I know this is bad form but I don't think it will be a problem since I don't anticipate some one running that many of these through on a specified day)
    'end the script
    End
End Function


'parse delimited loan information from comment into array
Function ParseComment()
    Dim str0 As String
    Dim str1 As String
    Dim Arr1() As String
    Dim ARCArr1() As String
    Dim X As Integer
    
    'get comment from first page
    str1 = Session.GetDisplayText(17, 2, 79)
    If GetText(18, 2, 79) <> "" Then str1 = str1 & Session.GetDisplayText(18, 2, 79)
    If GetText(19, 2, 79) <> "" Then str1 = str1 & Session.GetDisplayText(19, 2, 79)
    If GetText(20, 2, 79) <> "" Then str1 = str1 & Session.GetDisplayText(20, 2, 79)
    If GetText(21, 2, 79) <> "" Then str1 = str1 & Session.GetDisplayText(21, 2, 79)
    If GetText(22, 2, 79) <> "" Then str1 = str1 & Session.GetDisplayText(22, 2, 79)
    
    'get commment from subsequent pages
    hit "F6"
    hit "F6"
    While check4text(16, 80, "+")
        hit "F8"
        str1 = str1 & GetText(17, 2, 79) & GetText(18, 2, 79) & GetText(19, 2, 79) & GetText(20, 2, 79) & GetText(21, 2, 79) & GetText(22, 2, 79)
    Wend
    
    'trim string and remove portion before delimited loan information
    str1 = Trim(str1)
    If InStr(1, str1, "COMPLETED LVC", vbTextCompare) = 0 Then Exit Function 'if comment doesn't have "COMPLETED LVC" in it's text then it isn't a comment that should be processed.
    str1 = Mid(str1, InStr(1, str1, "}") + 1)
    
    'parse using ; as record and , as field delimiters if ; found (new style to support multiple page comments)
    If InStr(1, str1, ";") > 0 Then
        str1 = Replace(Trim(str1), " ", "")
        Arr1 = Split(str1, ";")
        For X = 0 To UBound(Arr1) - 1
            Arr1(X) = Trim(Arr1(X))
            If Arr1(X) <> "" Then
                ARCArr1 = Split(Arr1(X), ",") 'LoanSeq,Payoff Amount,Daily Interest Amount
                MasterLnSeqArr(0, UBound(MasterLnSeqArr, 2)) = ARCArr1(0)
                MasterLnSeqArr(1, UBound(MasterLnSeqArr, 2)) = ARCArr1(1)
                MasterLnSeqArr(2, UBound(MasterLnSeqArr, 2)) = ARCArr1(2)
                ReDim Preserve MasterLnSeqArr(6, UBound(MasterLnSeqArr, 2) + 1)
            End If
        Next X
    'parse using "  " (double space) as record and " " (single space) as field delimiters (old, pre SR 1965 style)
    Else
        Arr1 = Split(str1, "  ")
        For X = 0 To UBound(Arr1)
            Arr1(X) = Trim(Arr1(X))
            If Arr1(X) <> "" Then
                ARCArr1 = Split(Arr1(X), " ") 'LoanSeq,Payoff Amount,Daily Interest Amount
                MasterLnSeqArr(0, UBound(MasterLnSeqArr, 2)) = ARCArr1(0)
                MasterLnSeqArr(1, UBound(MasterLnSeqArr, 2)) = ARCArr1(1)
                MasterLnSeqArr(2, UBound(MasterLnSeqArr, 2)) = ARCArr1(2)
                ReDim Preserve MasterLnSeqArr(6, UBound(MasterLnSeqArr, 2) + 1)
            End If
        Next X
    End If
End Function

'adds test case of comment with multiple pages for sr1965
Sub additon()
    Sp.Common.ATD22AllLoans "528892401", "DFDLC", "COMPLETED LVC FOR NELNET FOR 10/02/2007;ANTI RAID LETTER SENT TO BORROWER / UT00180 {COMPASSLVC}007,000386.80,000023.24;006,000291.27,000017.51;005,000148.62,000008.93;003,001019.79,000061.34;004,000974.55,000058.62;002,000818.34,000049.23;001,000649.36,000039.06;008,000849.36,000089.06;009,000949.36,000099.06;010,001049.36,000109.06;011,001149.36,000119.06;012,001249.36,000129.06;013,001349.36,000139.06;014,001449.36,000149.06;015,001549.36,000159.06;016,001649.36,000169.06;017,001749.36,000179.06;0018,001849.36,000189.06;019,001949.36,000199.06;020,002049.36,000209.06;021,002149.36,000219.06;022,002249.36,000229.06;023,002349.36,000239.06;024,002449.36,000249.06;025,002549.36,000259.06;", "", ""
End Sub

