Attribute VB_Name = "PmtHistBorr"
Dim SSN As String
Dim AN As String                                                        '<7>
Dim Balance As Double
Dim MoInt As Double
Dim BilledAmt As Double
Dim ReducedAmt As Double
Dim DueDate As Variant
Dim DueDay As String
Dim PayoffDate As String
Dim TotalAcc As Double
Dim TotalCol As Double
Dim TotalDef As Double
Dim PrincCur As Double
Dim PrincCol As Double
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
Dim CCCur As Double
Dim CCProj As Double
Dim TotalCur As Double

Dim Name As String
Dim EffDate As String
Dim PmtType As String
Dim PmtAmt As Double
Dim PmtAmt1 As Double
Dim PmtAmt2 As Double
Dim PrincBeg As Double
Dim PrincPur As Double
Dim Legal As Double
Dim other As Double
Dim CC As Double
Dim OV As Double
Dim RevType As String
Dim counter As Integer
Dim row As Double
Dim PartType As String

Dim Lns As Integer                                                      '<3>
Dim pii As Integer
Dim aii As Integer
Dim LPOD(1 To 99) As String                                             '<3>
Dim CLID(1 To 99) As String
Dim PurchAmt(1 To 99) As Double                                         '<3>
Dim EDDate(1 To 99) As String
Dim EDAmt(1 To 99) As Double
Dim EDPrinc(1 To 99) As Double
Dim EDInt(1 To 99) As Double
Dim EDLegal(1 To 99) As Double
Dim EDOther(1 To 99) As Double
Dim EDCC(1 To 99) As Double
Dim EDInd As Integer
Dim LastEffDate As String
Dim ExcelApp As Excel.Application




Sub BorrPmtHist()
    With Session
        
        counter = 0
        'check to see if there are payments to report
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LC41I"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .MoveCursor 7, 36                                               '<1>
        .TransmitANSI "X"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
'        If Session.GetDisplayText(22, 3, 5) = "48012" Then
'            MsgBox "There are no payments to report."
'            End
'        End If
'        Do While .GetDisplayText(5, 9, 3) = "SOC"                       </1>
        Do While .GetDisplayText(22, 3, 5) = "40004"
            .WaitForTerminalKey rcIBMInsertKey, "1:00:00"
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        Loop
        If Session.GetDisplayText(22, 3, 5) = "47004" Then              '<1>
            MsgBox "There are no payments to report."
            End
        End If                                                         '</1>
    'get balance information from LC10
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LC10I"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        If SP.Common.CalledByMauiDUDE(SSN) = False Then
            SSN = GetText(1, 9, 9)
        End If
                                                '<7>
        If .GetDisplayText(9, 10, 3) = "SOC" Then                       '<2>
            TotalCur = 0.0009999
        Else                                                           '</2>
            'get amounts
            PrincPur = Session.GetDisplayText(7, 35, 11)
            PrincCol = Session.GetDisplayText(8, 35, 11)
            IntAcc = Session.GetDisplayText(9, 35, 11)
            IntCol = Session.GetDisplayText(10, 35, 11)
            LegalAcc = Session.GetDisplayText(11, 35, 11)
            LegalCol = Session.GetDisplayText(12, 35, 11)
            OtherAcc = Session.GetDisplayText(13, 35, 11)
            OtherCol = Session.GetDisplayText(14, 35, 11)
            CCAcc = Session.GetDisplayText(15, 35, 11)
            CCCol = Session.GetDisplayText(16, 35, 11)
            CCProj = Session.GetDisplayText(17, 35, 11)
            'calculate current balances
            PrincCur = PrincPur - PrincCol
            IntCur = IntAcc - IntCol
            LegalCur = LegalAcc - LegalCol
            OtherCur = OtherAcc - OtherCol
            CCCur = CCAcc - CCCol
            TotalCur = PrincCur + IntCur + LegalCur + OtherCur + CCCur + CCProj
        End If
    'get purchase amounts from LC05
        LC05                                                            '<3>
'<7->
    'get account number from LP22
        GetLP22 SSN, AN
'</7>
    'get payment information from LC41
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LC41I"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .MoveCursor 7, 36                                              '<1->
        .TransmitANSI "X"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1                    '</1>
        If Session.GetDisplayText(1, 75, 6) = "SELECT" Then
            .MoveCursor 2, 73
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI GetDisplayText(2, 79, 2)
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitANSI "01"
            .TransmitTerminalKey rcIBMEnterKey
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            Do Until GetDisplayText(22, 3, 5) = "46004"
                .TransmitTerminalKey rcIBMPf8Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            Loop
        End If
'<7>    SSN = Format(.GetDisplayText(1, 9, 9), "@@@-@@-@@@@")
        Name = Trim(.GetDisplayText(2, 2, 40))
        PrincBeg = 0                                                   '<3->
        LastEffDate = "01/01/1900"
        pii = 0
        aii = 0                                                        '</3>
        Do Until .GetDisplayText(22, 3, 5) = "46003"
            'skip to first payment of first no repur loan              '<3->
            Do While DateValue(Format(.GetDisplayText(4, 72, 10), "##/##/####")) < DateValue(LPOD(1))
                .TransmitTerminalKey rcIBMPf7Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                If .GetDisplayText(22, 3, 5) = "46003" Then
                    MsgBox "There are no payments to report for the current occurance of the loan(s)."
                    End
                End If
            Loop                                                       '</3>
            'skip bogus advices
'<1>        Do While .GetDisplayText(4, 35, 6) = "ADVICE" And GetDisplayText(14, 68, 12) < 5 And .GetDisplayText(21, 3, 5) <> "46003"
'<2>        Do While .GetDisplayText(4, 32, 6) = "ADVICE" And GetDisplayText(14, 68, 12) < 5 And .GetDisplayText(21, 3, 5) <> "46003"           '<1>
            Do While .GetDisplayText(4, 32, 6) = "ADVICE" And GetDisplayText(15, 68, 12) = 0 And .GetDisplayText(22, 3, 5) <> "46003"           '<2>
                .TransmitTerminalKey rcIBMPf7Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            Loop
            If .GetDisplayText(22, 3, 5) = "46003" Then
                If counter = 0 Then
                    MsgBox "The only transactions found for this borrower are advices that do not affect the account balance.  As a result, a payment history cannot be generated."
                    End
                End If
                Exit Do
            End If
'<3>        'get beginning principal amount
'            PrincBeg = session.GetDisplayText(8, 68, 12)        <1>
'<3>        If counter = 0 Then
'            If counter = 0 Or PrincBeg <= 0 Then
'<3>            PrincBeg = .GetDisplayText(7, 68, 12)
'           Else
'               PrincBeg = Val(PrincBeg) + Val(PrincCol)
'           End If                                              '</1>'</3>
            'get effective date of payment
            EffDate = Format(.GetDisplayText(4, 72, 8), "##/##/####")
'<1>         If .GetDisplayText(6, 31, 10) = "INJ SPOUSE" Or .GetDisplayText(6, 31, 10) = "POST ERROR" Or .GetDisplayText(6, 31, 10) = " BAD CHECK" Then
            If .GetDisplayText(6, 28, 10) = "POST ERROR" Or .GetDisplayText(6, 28, 10) = " BAD CHECK" Then      '<1>
                FullReversal
            ElseIf .GetDisplayText(4, 28, 10) = "FEDERL TXO" And .GetDisplayText(6, 28, 10) = "INJ SPOUSE" Then       '<1>
                .TransmitTerminalKey rcIBMPf7Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                If .GetDisplayText(4, 28, 10) = "FEDERL TXO" And .GetDisplayText(6, 28, 10) = "          " And DateValue(Format(.GetDisplayText(4, 72, 8), "##/##/####")) = DateValue(EffDate) Then
                    .TransmitTerminalKey rcIBMPf8Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    PartType = "FOBC"
                    PartialReversal
                ElseIf .GetDisplayText(22, 3, 5) = "46003" Then
                    NormalTrans
                Else
                    .TransmitTerminalKey rcIBMPf8Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    FullReversal
                End If
            ElseIf .GetDisplayText(4, 28, 10) = "FEDERL TXO" And .GetDisplayText(6, 28, 10) = "          " Then
                .TransmitTerminalKey rcIBMPf7Key
                .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                If .GetDisplayText(4, 28, 10) = "FEDERL TXO" And .GetDisplayText(6, 28, 10) = "INJ SPOUSE" And DateValue(Format(.GetDisplayText(4, 72, 8), "##/##/####")) = DateValue(EffDate) Then
                    PartType = "FO"
                    PartialReversal
                    .TransmitTerminalKey rcIBMPf7Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                ElseIf .GetDisplayText(22, 3, 5) = "46003" Then
                    NormalTrans
                Else
                    .TransmitTerminalKey rcIBMPf8Key
                    .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
                    NormalTrans
                End If                                                                  '</1>
            Else
                NormalTrans
            End If
            'check to see if purchase or assignment transactions need to be inserted
            Rev4Ins
'           counter = counter + 1
'           'determine row number for spreadsheet
'</3>       row = Trim(Str(counter + 7))
            'go to previous payment
            .TransmitTerminalKey rcIBMPf7Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            'add the payment to the spreadsheet                        '<3->
            AddRow
            'subtract the principal collected from the beginning principal to get the new beginning principal balance
            PrincBeg = PrincBeg + PrincCol 'princol is neg so it has to be added
            LastEffDate = EffDate
        Loop
        'check to see if purchase or assignment transactions need to be inserted after the last payment transaction
        Do Until pii = Lns And aii = EDInd
            EffDate = "12/31/3000"
            Rev4Ins
        Loop
        'add the balance info the bottom of the spreadsheet
        AddBalInfo
    End With
End Sub                                                                '</3>
            
Sub AddRow()                                                           '<3->
    With Session
        'determine row number for spreadsheet
        counter = counter + 1
        row = Trim(str(counter + 7))                                   '</3>
        'set object and open workbook first time through processing loop
        If counter = 1 Then
'<3>        Dim excelApp As Excel.Application
            Set ExcelApp = CreateObject("Excel.Application")
            ExcelApp.Visible = True
'<7->
'           excelApp.Workbooks.Open FileName:="X:\PADD\General\TransHistory.xls"
            If SP.TestMode Then
                ExcelApp.Workbooks.Open Filename:="X:\PADD\General\Test\TransHistory.xls"
            Else
                ExcelApp.Workbooks.Open Filename:="X:\PADD\General\TransHistory.xls"
            End If
'</7>
            'Name
            ExcelApp.Range("B2").Select
            ExcelApp.ActiveCell.FormulaR1C1 = Name
            'SSN
            ExcelApp.Range("B3").Select
'<7->
'           excelApp.ActiveCell.FormulaR1C1 = SSN
            ExcelApp.ActiveCell.FormulaR1C1 = AN
        End If
    'enter payment data
        'effective date
        ExcelApp.Range("A" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = EffDate
        'payment type
        ExcelApp.Range("B" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = PmtType
        'payment amount
        ExcelApp.Range("C" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = PrincBeg
        'beginning principal
        ExcelApp.Range("D" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = PmtAmt
        'principal paid
        ExcelApp.Range("E" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = PrincCol
        'interest paid
        ExcelApp.Range("F" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = IntCol
        'legal costs paid
        ExcelApp.Range("G" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = Legal
        'other costs paid
        ExcelApp.Range("H" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = other
        'collection costs paid
        ExcelApp.Range("I" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = CC
        'overpayment or reversal amount
        ExcelApp.Range("J" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = OV
        'reversal type
        ExcelApp.Range("K" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = RevType
'<3->   Loop
    End With
End Sub                                                                '</3>

Sub AddBalInfo()                                                       '<3->
    With Session           '</3>
        'enter current balance information
        row = row + 3
        ExcelApp.Range("A" & row).Select
        If TotalCur = 0.0009999 Then                                                                     '<2>
            ExcelApp.ActiveCell.FormulaR1C1 = "Account assigned to ED, current balance not available"
        Else                                                                                            '</2>
            ExcelApp.ActiveCell.FormulaR1C1 = "Current Balance"
        End If
        row = row + 1
        ExcelApp.Range("A" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Principal"
        ExcelApp.Range("C" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = PrincCur
        row = row + 1
        ExcelApp.Range("A" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Interest"
        ExcelApp.Range("C" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = IntCur
        row = row + 1
        ExcelApp.Range("A" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Legal Costs"
        ExcelApp.Range("C" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = LegalCur
        row = row + 1
        ExcelApp.Range("A" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Other Costs"
        ExcelApp.Range("C" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = OtherCur
        row = row + 1
        ExcelApp.Range("A" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Collection Costs"
        ExcelApp.Range("C" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = CCCur
        row = row + 1
        ExcelApp.Range("A" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Projected Collection Costs"
        ExcelApp.Range("C" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = CCProj
        row = row + 1
        ExcelApp.Range("C" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "'-------------------"
        row = row + 1
        ExcelApp.Range("A" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = "Total Balance"
        ExcelApp.Range("C" & row).Select
        ExcelApp.ActiveCell.FormulaR1C1 = TotalCur
    End With
End Sub

'get information for a non-reversed transaction (normal)
Sub NormalTrans()
    With Session
'        PmtType = Trim(.GetDisplayText(4, 31, 10))      <1>
        PmtType = Trim(.GetDisplayText(4, 28, 10))      '<1>
        If .GetDisplayText(20, 68, 12) <> 0 Then
            RevType = "REFUND"
        Else
            RevType = ""
        End If
'        PmtAmt = "-" + .GetDisplayText(3, 68, 12)       <1>
        PmtAmt = "-" + .GetDisplayText(3, 26, 12)       '<1>
        PrincCol = "-" + .GetDisplayText(15, 68, 12)
        IntCol = "-" + .GetDisplayText(16, 68, 12)
        Legal = "-" + .GetDisplayText(17, 68, 12)
        other = "-" + .GetDisplayText(18, 68, 12)
        CC = "-" + .GetDisplayText(19, 68, 12)
        OV = .GetDisplayText(20, 68, 12)
    End With
End Sub

'get information for a full reversal of a transaction
Sub FullReversal()
    With Session
'        PmtType = Trim(.GetDisplayText(4, 31, 10))      <1>
'        RevType = .GetDisplayText(6, 31, 10)
'        PmtAmt = "0"
        PmtType = Trim(.GetDisplayText(4, 28, 10))
        RevType = .GetDisplayText(6, 28, 10)
        PmtAmt = "-" & .GetDisplayText(3, 26, 12)      '</1>
        PrincCol = "0"
        IntCol = "0"
        Legal = "0"
        other = "0"
        CC = "0"
'        OV = .GetDisplayText(3, 68, 12)                 <1>
        OV = .GetDisplayText(3, 26, 12)                 '<1>
    End With
End Sub

'get information for a partial reversal of a federal tax offset (injured spouse refund)
Sub PartialReversal()                                                       '<1>
    With Session
        PmtType = Trim(.GetDisplayText(4, 28, 10))
        RevType = .GetDisplayText(6, 28, 10)
        PmtAmt = "-" & .GetDisplayText(3, 26, 12)
        OV = .GetDisplayText(9, 26, 12)
        If PartType = "FOBC" Then
            .TransmitTerminalKey rcIBMPf7Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        Else
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        End If
        PrincCol = "-" + .GetDisplayText(15, 68, 12)
        IntCol = "-" + .GetDisplayText(16, 68, 12)
        Legal = "-" + .GetDisplayText(17, 68, 12)
        other = "-" + .GetDisplayText(18, 68, 12)
        CC = "-" + .GetDisplayText(19, 68, 12)
    End With
End Sub                                                                     '</1>

'<3->
'get loan information
Sub LC05()
    With Session
        Dim v1(1 To 99) As String
        Dim v2(1 To 99) As Double
        Dim v3(1 To 99) As String
        Dim v4(1 To 99) As String
        Dim v5(1 To 99) As String
        Dim tv1(1 To 99) As String
        Dim tv2(1 To 99) As Double
        Dim tv3(1 To 99) As String
        Dim tv4(1 To 99) As String
        Dim tv5(1 To 99) As String
        
        EDInd = 0
        'access LC05
        .TransmitTerminalKey rcIBMClearKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        .TransmitANSI "LC05I"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        'access LC05 target
        .TransmitANSI "01"
        .TransmitTerminalKey rcIBMEnterKey
        .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        Lns = 0
        'review each loan
        Do While .GetDisplayText(22, 3, 5) <> "46004"
            Lns = Lns + 1
            LPOD(Lns) = DateValue(Format(.GetDisplayText(4, 73, 8), "##/##/####"))
            PurchAmt(Lns) = .GetDisplayText(9, 32, 12)
            'get assignment info if loan was assigned to ED
            If .GetDisplayText(19, 73, 2) <> "MM" And .GetDisplayText(19, 70, 2) = "03" Then
                EDDate(Lns) = Format(.GetDisplayText(19, 72, 10), "##/##/####")
                EDAmt(Lns) = .GetDisplayText(20, 32, 12)
                EDPrinc(Lns) = CDbl(.GetDisplayText(9, 32, 12)) - CDbl(.GetDisplayText(10, 32, 12))
                EDInt(Lns) = CDbl(.GetDisplayText(11, 32, 12)) - CDbl(.GetDisplayText(12, 32, 12))
                EDLegal(Lns) = CDbl(.GetDisplayText(13, 32, 12)) - CDbl(.GetDisplayText(14, 32, 12))
                EDOther(Lns) = CDbl(.GetDisplayText(15, 32, 12)) - CDbl(.GetDisplayText(16, 32, 12))
                EDCC(Lns) = CDbl(.GetDisplayText(17, 32, 12)) - CDbl(.GetDisplayText(18, 32, 12)) + CDbl(.GetDisplayText(18, 32, 12))
                EDInd = EDInd + 1
            End If
            'go to page 3
            .TransmitTerminalKey rcIBMPf10Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            .TransmitTerminalKey rcIBMPf10Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
            CLID(Lns) = .GetDisplayText(3, 13, 19)
            'compare the unique ID to the other loans and only keep the most current purchase of the loan
            For i = 1 To Lns - 1
                If CLID(Lns) = CLID(i) Then
                    If DateValue(LPOD(i)) < DateValue(LPOD(Lns)) Then
                        LPOD(i) = LPOD(Lns)
                        PurchAmt(i) = PurchAmt(Lns)
                        EDDate(i) = EDDate(Lns)
                        EDAmt(i) = EDAmt(Lns)
                        CLID(i) = CLID(Lns)
                    End If
                    Lns = Lns - 1
                    Exit For
                End If
            Next i
            'go to the next loan
            .TransmitTerminalKey rcIBMPf8Key
            .WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        Loop
        'sort the loans by purchase date (LPOD)
        For m = 1 To Lns - 1
            For j = m + 1 To Lns
                If DateValue(LPOD(m)) > DateValue(LPOD(j)) Then
                    tLPOD = LPOD(j)
                    tPurchAmt = PurchAmt(j)
                    tEDDate = EDDate(j)
                    tEDAmt = EDAmt(j)
                    tCLID = CLID(j)
                                        
                    LPOD(j) = LPOD(m)
                    PurchAmt(j) = PurchAmt(m)
                    EDDate(j) = EDDate(m)
                    EDAmt(j) = EDAmt(m)
                    CLID(j) = CLID(m)
                                        
                    LPOD(m) = tLPOD
                    PurchAmt(m) = tPurchAmt
                    EDDate(m) = tEDDate
                    EDAmt(m) = tEDAmt
                    CLID(m) = tCLID
                End If
            Next j
        Next m
    End With
End Sub
'</3>

'<3->
'check see if a purchase or assignment row needs to be inserted
Sub Rev4Ins()
    With Session
        'insert purchase rows if the purchase insert indicator is less than the number of purchases
        If pii < Lns Then
            'review each purchase
            For i = 1 To Lns
                'insert the purchase if it falls between the last and the current transactions
                If DateValue(LPOD(i)) >= DateValue(LastEffDate) And _
                   DateValue(LPOD(i)) <= DateValue(EffDate) Then
                    'transfer payment info to temp variables
                    tv1 = EffDate
                    tv2 = PmtType
                    tv3 = PmtAmt
                    tv4 = PrincCol
                    tv5 = IntCol
                    tv6 = Legal
                    tv7 = other
                    tv8 = CC
                    tv9 = OV
                    tv10 = RevType
                    'transfer purchase info to payment variables
                    EffDate = LPOD(i)
'<5>                PmtType = "NEW DEFAULT"
                    PmtType = "CLAIM PURCHASE"                          '<5>
                    PmtAmt = PurchAmt(i)
                    PrincCol = PurchAmt(i)
                    IntCol = 0
                    Legal = 0
                    other = 0
                    CC = 0
                    OV = 0
                    RevType = ""
                    'add the purchase row to the spreadsheet
                    AddRow
                    pii = pii + 1
                    'transfer the payment values from temp storage to the pmt variables
                    EffDate = tv1
                    PmtType = tv2
                    PmtAmt = tv3
                    PrincCol = tv4
                    IntCol = tv5
                    Legal = tv6
                    other = tv7
                    CC = tv8
                    OV = tv9
                    RevType = tv10
                    'add the purchase amount to the beginning princ bal
                    PrincBeg = PrincBeg + PurchAmt(i)
                    LastEffDate = LPOD(i)
                End If
            Next i
        End If
        'insert assignment rows if the ed indicator is yes (there are loans assigned to ED)
        If EDInd <> 0 Then
            'review each purchase
            For i = 1 To EDInd
                'insert the assignment if it falls between the last and the current transactions
                If DateValue(EDDate(i)) >= DateValue(LastEffDate) And _
                   DateValue(EDDate(i)) <= DateValue(EffDate) Then
                    'transfer payment info to temp variables
                    tv1 = EffDate
                    tv2 = PmtType
                    tv3 = PmtAmt
                    tv4 = PrincCol
                    tv5 = IntCol
                    tv6 = Legal
                    tv7 = other
                    tv8 = CC
                    tv9 = OV
                    tv10 = RevType
                    'transfer assignment info to payment variables
                    EffDate = EDDate(i)
                    PmtType = "ASSIGNED ED"
                    PmtAmt = 0 - EDAmt(i)
                    PrincCol = 0 - EDPrinc(i)
                    IntCol = 0 - EDInt(i)
                    Legal = 0 - EDLegal(i)
                    other = 0 - EDOther(i)
                    CC = 0 - EDCC(i)
                    OV = 0
                    RevType = ""
                    'add the purchase row to the spreadsheet
                    AddRow
                    aii = aii + 1
                    'transfer the payment values from temp storage to the pmt variables
                    EffDate = tv1
                    PmtType = tv2
                    PmtAmt = tv3
                    PrincCol = tv4
                    IntCol = tv5
                    Legal = tv6
                    other = tv7
                    CC = tv8
                    OV = tv9
                    RevType = tv10
                    'add the assigned amount to the beginning princ bal
                    PrincBeg = PrincBeg - EDAmt(i)
                    LastEffDate = EDDate(i)
                End If
            Next i
        End If
    End With
End Sub
'</3>

'new pre sr, jd, ??/??/??, ??/??/??
'<1> sr  58, jd, 05/31/02, 06/12/02
'<2> sr  72, jd, 07/19/02, 08/12/02
'<3> sr 268, jd, 05/09/03, 07/22/03
'<4> sr 342, jd, 06/12/03, 07/22/03 changed Y:\Apps\FileTransfers\COMMON\OneLINK\OPA\ to X:\PADD\General\
'<5> sr 353, jd, 06/13/03, 07/22/03
'<6> sr 641, jd, 05/07/04, 05/07/04 changed position of error message from 21, 3 to 22, 3 and replaced "??????????" with blanks for value of reversal type field
'<7> sr1468, jd

