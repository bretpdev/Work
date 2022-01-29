Attribute VB_Name = "PmtHistLoan"
Dim PrincPur As Double
Dim PrincCol As Double
Dim PrincCur As Double
Dim IntAcc As Double
Dim IntCol As Double
Dim IntCur As Double
Dim LegalAcc As Double
Dim LegalCol As Double
Dim LegalCur As Double
Dim OtherAcc As Double
Dim OtherCol As Double
Dim OtherCur As Double
Dim CCAcc As Double
Dim CCCol As Double
Dim CCProj As Double
Dim CCCur As Double
Dim TotalCur As Double
Dim ExitDoNow As String

Dim SSN As String
Dim AN As String                                                        '<4>
Dim Loan As String
Dim CLID As String
Dim Name As String
Dim EffDate As String
Dim PmtType As String
Dim PmtAmt As String
Dim Legal As String
Dim other As String
Dim CC As String
Dim OV As String
Dim counter As Integer
Dim warn As String
Dim RevType As String
Dim Row As String
Dim PartType As String

Sub LoanPmtHist()
    With Session
        SortOpt = MsgBox("Do you want the transactions sorted by loan?", vbYesNo, "Sort Option")    '<1>
        'check to see if there are payments to report
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LC44I"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'pause for the user to enter an SSN
        Do While .GetDisplayText(22, 3, 5) = "     " And .GetDisplayText(7, 10, 3) = "SOC"
            .WaitForTerminalKey rcIBMInsertKey, "1:00:00"
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        Loop
        'warn the user and end the script if no payments are found
        If Session.GetDisplayText(22, 3, 5) = "47004" Then
            MsgBox "There are no payments to report."
            End
        End If
      
    'get balance information from LC10
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LC10I"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        If SP.Common.CalledByMauiDUDE(SSN) = False Then
            SSN = GetText(1, 9, 9)
        End If                                        '<4>
        If .GetDisplayText(9, 10, 3) = "SOC" Then
            TotalCur = 0.0009999
        Else
            'get amounts
            PrincPur = GetDisplayText(7, 35, 11)
            PrincCol = GetDisplayText(8, 35, 11)
            IntAcc = GetDisplayText(9, 35, 11)
            IntCol = GetDisplayText(10, 35, 11)
            LegalAcc = GetDisplayText(11, 35, 11)
            LegalCol = GetDisplayText(12, 35, 11)
            OtherAcc = GetDisplayText(13, 35, 11)
            OtherCol = GetDisplayText(14, 35, 11)
            CCAcc = GetDisplayText(15, 35, 11)
            CCCol = GetDisplayText(16, 35, 11)
            CCProj = GetDisplayText(17, 35, 11)
            'calculate current balances
            PrincCur = PrincPur - PrincCol
            IntCur = IntAcc - IntCol
            LegalCur = LegalAcc - LegalCol
            OtherCur = OtherAcc - OtherCol
            CCCur = CCAcc - CCCol
            TotalCur = PrincCur + IntCur + LegalCur + OtherCur + CCCur + CCProj
        End If
'<4->
    'get account number from LP22
        GetLP22 SSN, AN
'</4>
    'get payment information from LC44
        'access LC44
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LC44I"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'access the first loan
        .TransmitANSI "01"
        .Wait 1
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'get the SSN and name
'<4>    SSN = Format(.GetDisplayText(1, 9, 9), "@@@-@@-@@@@")
        Name = Trim(Session.GetDisplayText(2, 2, 40))
        ExitDoNow = "N"
        counter = 0
        'process each transaction
        Do
            'skip bogus advices and injured spouse refunds
'<1>        Do While ((.GetDisplayText(7, 23, 6) = "ADVICE" And GetDisplayText(13, 71, 10) < 5) Or (.GetDisplayText(7, 23, 10) = "FEDERL TXO" And .GetDisplayText(8, 23, 10) = "INJ SPOUSE" And .GetDisplayText(4, 71, 10) <> .GetDisplayText(9, 71, 10))) And .GetDisplayText(21, 3, 5) <> "46004"
            Do While ((.GetDisplayText(7, 23, 6) = "ADVICE" And GetDisplayText(14, 71, 10) = 0) Or (.GetDisplayText(7, 23, 10) = "FEDERL TXO" And .GetDisplayText(8, 23, 10) = "INJ SPOUSE" And .GetDisplayText(4, 71, 10) <> .GetDisplayText(9, 71, 10))) And .GetDisplayText(22, 3, 5) <> "46004"     '<1>
                'look for an FO transaction if the transaction is an FOBC
                If .GetDisplayText(7, 23, 10) = "FEDERL TXO" Then
                    'get the transaction effective date and loan number
                    EffDate = Format(.GetDisplayText(6, 73, 8), "##/##/####")
                    Loan = .GetDisplayText(4, 23, 4)
                    'page forward to the next transaction for the loan
                    Do
                        .TransmitTerminalKey rcIBMPf8Key
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    Loop Until .GetDisplayText(4, 23, 4) = Loan Or .GetDisplayText(22, 3, 5) = "46004"
                    'if the transaction is an FO and the effective date is the same
                    If .GetDisplayText(22, 3, 5) <> "46004" And .GetDisplayText(7, 23, 10) = "FEDERL TXO" And .GetDisplayText(8, 23, 10) = "          " And DateValue(Format(.GetDisplayText(6, 73, 8), "##/##/####")) = DateValue(EffDate) Then
                        'page back to the transaction being processed so the next transaction can be processed
                        Do
                            .TransmitTerminalKey rcIBMPf7Key
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        Loop Until .GetDisplayText(4, 23, 4) = Loan
                    'if an FO transaction was not found for the loan
                    Else
                        If .GetDisplayText(22, 3, 5) = "46004" And .GetDisplayText(4, 23, 4) = Loan Then
                        'page back to the transaction being processed
                        Else
                            Do
                                .TransmitTerminalKey rcIBMPf7Key
                                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            Loop Until .GetDisplayText(4, 23, 4) = Loan
                        End If
                        'page back to the next transaction for the loan
                        Do
                            .TransmitTerminalKey rcIBMPf7Key
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        Loop Until .GetDisplayText(4, 23, 4) = Loan Or .GetDisplayText(22, 3, 5) = "46003"
                        'if an FO transaction was found and the effective date is the same
                        If .GetDisplayText(22, 3, 5) <> "46003" And .GetDisplayText(7, 23, 10) = "FEDERL TXO" And .GetDisplayText(8, 23, 10) = "          " And DateValue(Format(.GetDisplayText(6, 73, 8), "##/##/####")) = DateValue(EffDate) Then
                            'page forward to the transaction being processed so the next transaction can be processed
                            Do
                                .TransmitTerminalKey rcIBMPf8Key
                                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            Loop Until .GetDisplayText(4, 23, 4) = Loan
                        'if an FO transaction was not found for the loan, 'page forward to the transaction being processed so the transaction can be processed
                        ElseIf .GetDisplayText(22, 3, 5) = "46003" Then
                            Do Until .GetDisplayText(4, 23, 4) = Loan
                                .TransmitTerminalKey rcIBMPf8Key
                                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            Loop
                            Exit Do
                        Else
                            Do
                                .TransmitTerminalKey rcIBMPf8Key
                                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                            Loop Until .GetDisplayText(4, 23, 4) = Loan
                            Exit Do
                        End If
                    End If
                End If
                'page forward to the next transaction
                .TransmitTerminalKey rcIBMPf8Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            Loop
            'warn the user and end the script if only advices were found
            If .GetDisplayText(22, 3, 5) = "46004" Then
                If counter = 0 Then
                    MsgBox "The only transactions found for this borrower are advices that do not affect the account balance.  As a result, a payment history cannot be generated."
                    End
                End If
                Exit Do
            End If
            'get the effective date of payment, loan number, and unique ID
            EffDate = Format(.GetDisplayText(6, 73, 8), "##/##/####")
            Loan = .GetDisplayText(4, 23, 4)
            CLID = .GetDisplayText(19, 12, 19)
            'process post errored transactions and back checks as full reversals
            If .GetDisplayText(8, 23, 10) = "POST ERROR" Or .GetDisplayText(8, 23, 10) = "BAD CHECK " Then
                FullReversal
            'process and error transaction
            ElseIf .GetDisplayText(7, 23, 10) = "FEDERL TXO" And .GetDisplayText(8, 23, 10) = "INJ SPOUSE" And .GetDisplayText(4, 71, 10) <> .GetDisplayText(9, 71, 10) Then
                ErrorTrans
            'process full federal offset reversals
            ElseIf .GetDisplayText(7, 23, 10) = "FEDERL TXO" And .GetDisplayText(8, 23, 10) = "INJ SPOUSE" Then
                FullReversal
            'for federal offsets (FO), look for an injured spouse reversal (FOBC), if one is found, process a partial injured spouse reversal, otherwise process a normal transaction
            ElseIf .GetDisplayText(7, 23, 10) = "FEDERL TXO" And .GetDisplayText(8, 23, 10) = "          " Then
                Do
                    .TransmitTerminalKey rcIBMPf8Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                Loop Until .GetDisplayText(4, 23, 4) = Loan Or .GetDisplayText(22, 3, 5) = "46004"
                If .GetDisplayText(22, 3, 5) <> "46004" And .GetDisplayText(7, 23, 10) = "FEDERL TXO" And .GetDisplayText(8, 23, 10) = "INJ SPOUSE" And DateValue(Format(.GetDisplayText(6, 73, 8), "##/##/####")) = DateValue(EffDate) Then
                    PartType = "FO"
                    PartialReversal
                Else
                    If .GetDisplayText(22, 3, 5) = "46004" And .GetDisplayText(4, 23, 4) = Loan Then
                    Else
                        Do
                            .TransmitTerminalKey rcIBMPf7Key
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        Loop Until .GetDisplayText(4, 23, 4) = Loan
                    End If
                    Do
                        .TransmitTerminalKey rcIBMPf7Key
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    Loop Until .GetDisplayText(4, 23, 4) = Loan Or .GetDisplayText(22, 3, 5) = "46003"
                    If .GetDisplayText(22, 3, 5) <> "46003" And .GetDisplayText(7, 23, 10) = "FEDERL TXO" And .GetDisplayText(8, 23, 10) = "INJ SPOUSE" And DateValue(Format(.GetDisplayText(6, 73, 8), "##/##/####")) = DateValue(EffDate) Then
                        PartType = "FOBC"
                        PartialReversal
                    ElseIf .GetDisplayText(22, 3, 5) = "46003" Then
                        Do Until .GetDisplayText(4, 23, 4) = Loan
                            .TransmitTerminalKey rcIBMPf8Key
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        Loop
                        NormalTrans
                    Else
                        Do
                            .TransmitTerminalKey rcIBMPf8Key
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        Loop Until .GetDisplayText(4, 23, 4) = Loan
                        NormalTrans
                    End If
                End If
            Else
                NormalTrans
            End If
                
            counter = counter + 1
            'determine row number for spreadsheet
            Row = Trim(str(counter + 7))
            'go to previous payment
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            If .GetDisplayText(22, 3, 5) = "46004" Then
                .TransmitTerminalKey rcIBMPf12Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                If .GetDisplayText(2, 79, 2) = "20" Then
                    firsteffdate = .GetDisplayText(7, 10, 8)
                    Do
                        .MoveCursor 2, 73
                        .TransmitANSI "20"
                        .TransmitTerminalKey rcIBMEnterKey
                        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        If .GetDisplayText(22, 3, 5) <> "46005" Then
                            ExitDoNow = "Y"
                            Exit Do
                        Else
                            .TransmitTerminalKey rcIBMEnterKey
                            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                        End If
                    Loop Until .GetDisplayText(7, 10, 8) <> firsteffdate And DateValue(Format(.GetDisplayText(7, 10, 8), "##/##/####")) <= DateValue(EffDate)
                    .TransmitANSI "01"
                    .TransmitTerminalKey rcIBMEnterKey
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                Else
                    ExitDoNow = "Y"
                End If
            End If
                        
            'activate Excel
            'set object and open workbook first time through processing loop
            If counter = 1 Then
                Dim ExcelApp As Excel.Application
                Set ExcelApp = CreateObject("Excel.Application")
                ExcelApp.Visible = True
'</4>
'               excelApp.Workbooks.Open FileName:="X:\PADD\General\LoanTransHistory.xls"
                If SP.TestMode Then
                    ExcelApp.Workbooks.Open Filename:="X:\PADD\General\Test\LoanTransHistory.xls"
                Else
                    ExcelApp.Workbooks.Open Filename:="X:\PADD\General\LoanTransHistory.xls"
                End If
'</4>
                'Name
                ExcelApp.Range("B2").Select
                ExcelApp.ActiveCell.FormulaR1C1 = Name
                'SSN
                ExcelApp.Range("B3").Select
'<4->
'               excelApp.ActiveCell.FormulaR1C1 = SSN
                ExcelApp.ActiveCell.FormulaR1C1 = AN
'</4>
            Else
            End If
            
        'enter payment data
            'claim ID
            ExcelApp.Range("A" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = Loan
            'unique ID
            ExcelApp.Range("B" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = CLID
            'effective date
            ExcelApp.Range("C" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = EffDate
            'payment type
            ExcelApp.Range("D" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = PmtType
            'payment amount
            ExcelApp.Range("E" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = PmtAmt
            'principal paid
            ExcelApp.Range("F" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = PrincCol
            'interest paid
            ExcelApp.Range("G" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = IntCol
            'legal costs paid
            ExcelApp.Range("H" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = Legal
            'other costs paid
            ExcelApp.Range("I" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = other
            'collection costs paid
            ExcelApp.Range("J" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = CC
            'reversal amount
            ExcelApp.Range("K" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = OV
            'reversal type
            ExcelApp.Range("L" + Row).Select
            ExcelApp.ActiveCell.FormulaR1C1 = RevType
        Loop Until ExitDoNow = "Y"
       
    'sort rows in spreadsheet by loan then date
        If SortOpt = 6 Then                             '<1>
            ExcelApp.Range("A8:L5000").Select
            ExcelApp.Selection.Sort Key1:=Range("A8"), Order1:=xlAscending, Key2:=Range("C8") _
                , Order2:=xlAscending, Header:=xlNo, OrderCustom:=1, MatchCase:= _
                False, Orientation:=xlTopToBottom
            ExcelApp.Range("A8").Select
        End If                                          '<1>
 
    'enter current balance information
        Row = Row + 3
        ExcelApp.Range("A" & Row).Select
        If TotalCur = 0.0009999 Then                                                                     '<2>
            ExcelApp.ActiveCell.FormulaR1C1 = "Account assigned to ED, current balance not available"
        Else                                                                                            '</2>
            ExcelApp.ActiveCell.FormulaR1C1 = "Current Balance"
        End If
        Row = Row + 1
        ExcelApp.Range("A" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Principal"
        ExcelApp.Range("E" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = PrincCur
        Row = Row + 1
        ExcelApp.Range("A" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Interest"
        ExcelApp.Range("E" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = IntCur
        Row = Row + 1
        ExcelApp.Range("A" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Legal Costs"
        ExcelApp.Range("E" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = LegalCur
        Row = Row + 1
        ExcelApp.Range("A" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Other Costs"
        ExcelApp.Range("E" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = OtherCur
        Row = Row + 1
        ExcelApp.Range("A" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Collection Costs"
        ExcelApp.Range("E" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = CCCur
        Row = Row + 1
        ExcelApp.Range("A" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Projected Collection Costs"
        ExcelApp.Range("E" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = CCProj
        Row = Row + 1
        ExcelApp.Range("E" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "'-----------------"
        Row = Row + 1
        ExcelApp.Range("A" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Total Balance"
        ExcelApp.Range("E" & Row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = TotalCur
    End With
End Sub

'get information for a non-reversed transaction (normal)
Sub NormalTrans()
    With Session
        PmtType = Trim(.GetDisplayText(7, 23, 10))
        If .GetDisplayText(19, 71, 10) <> 0 Then
            RevType = "REFUND"
        Else
            RevType = ""
        End If
        PmtAmt = "-" & .GetDisplayText(4, 71, 10)
        PrincCol = "-" + .GetDisplayText(14, 71, 10)
        IntCol = "-" + .GetDisplayText(15, 71, 10)
        Legal = "-" + .GetDisplayText(16, 71, 10)
        other = "-" + .GetDisplayText(17, 71, 10)
        CC = "-" + .GetDisplayText(18, 71, 10)
        OV = .GetDisplayText(19, 71, 10)
    End With
End Sub

'get information for a full reversal of a transaction
Sub FullReversal()
    With Session
        PmtType = Trim(.GetDisplayText(7, 23, 10))
        RevType = .GetDisplayText(8, 23, 10)
        PmtAmt = "-" & .GetDisplayText(4, 71, 10)
        PrincCol = "0"
        IntCol = "0"
        Legal = "0"
        other = "0"
        CC = "0"
        OV = .GetDisplayText(4, 71, 10)
    End With
End Sub

'get information for a partial reversal of a federal tax offset (injured spouse refund)
Sub PartialReversal()
    With Session
        PmtType = Trim(.GetDisplayText(7, 23, 10))
        RevType = .GetDisplayText(8, 23, 10)
        PmtAmt = "-" & .GetDisplayText(4, 71, 10)
        OV = .GetDisplayText(9, 71, 10)
        If PartType = "FOBC" Then
            Do
                .TransmitTerminalKey rcIBMPf8Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            Loop Until .GetDisplayText(4, 23, 4) = Loan
        Else
            Do
                .TransmitTerminalKey rcIBMPf7Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            Loop Until .GetDisplayText(4, 23, 4) = Loan
        End If
        PrincCol = "-" + .GetDisplayText(14, 71, 10)
        IntCol = "-" + .GetDisplayText(15, 71, 10)
        Legal = "-" + .GetDisplayText(16, 71, 10)
        other = "-" + .GetDisplayText(17, 71, 10)
        CC = "-" + .GetDisplayText(18, 71, 10)
    End With
End Sub

'get information for an erroneous transaction
Sub ErrorTrans()
    With Session
        PmtType = Trim(.GetDisplayText(7, 23, 10))
        RevType = "ERROR"
        PmtAmt = "-" & .GetDisplayText(4, 71, 10)
        PrincCol = "-" + .GetDisplayText(14, 71, 10)
        IntCol = "-" + .GetDisplayText(15, 71, 10)
        Legal = "-" + .GetDisplayText(16, 71, 10)
        other = "-" + .GetDisplayText(17, 71, 10)
        CC = "-" + .GetDisplayText(18, 71, 10)
        OV = .GetDisplayText(9, 71, 10)
    End With
End Sub

'<1> sr  71, jd, 07/19/02
'<2> sr 341, jd, 06/12/03 changed Y:\Apps\FileTransfers\COMMON\OneLINK\OPA\ to X:\PADD\General\
'<3> sr 590, jd, 03/23/04, 3/25/04 replaced (21, 3, 5) with (22, 3, 5) as coordinates for error message
'<4> sr1469, jd

