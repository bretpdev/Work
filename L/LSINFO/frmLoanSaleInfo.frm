VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmLoanSaleInfo 
   Caption         =   "Loan Sale Information"
   ClientHeight    =   4320
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4095
   OleObjectBlob   =   "frmLoanSaleInfo.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frmLoanSaleInfo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private LenderPremTbl() As String       'index 0 = lender ID, 1 = Lender name, 2 = premium    '<4>

Private Sub btnCancel_Click()
    End
End Sub

Private Sub btnOK_Click()
'<1->
'    Dim SaleIDInfo() As String
'    Dim FinalSaleIDList() As String
'    ReDim SaleIDInfo(5, 0)
'    ReDim FinalSaleIDList(5, 0)
    ReDim LenderPremTbl(2, 0)               '<4>
    Dim Nelnet() As String
    ReDim Nelnet(11, 0)
    Dim UHEAA() As String
    ReDim UHEAA(11, 0)
    Dim FinalNelnet() As String
    ReDim FinalNelnet(5, 0)
    Dim FinalUHEAA() As String
    ReDim FinalUHEAA(5, 0)
    Dim FinalNelnetNS() As String
    ReDim FinalNelnetNS(5, 0)
    Dim FinalUHEAANS() As String
    ReDim FinalUHEAANS(5, 0)
    Dim NelSaleTypeCouldNotBeDetermined() As String             '<4>
    Dim UHEAASaleTypeCouldNotBeDetermined() As String           '<4>
    ReDim NelSaleTypeCouldNotBeDetermined(11, 0)                '<4>
    ReDim UHEAASaleTypeCouldNotBeDetermined(11, 0)              '<4>
'</1>
    'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
    'do data validation
    'check date given
    If CDate(Cal1.Value) = Date Then
        If vbNo = MsgBox("You have selected today as the sale date.  Click Yes to confirm that is correct.", vbYesNo, "Confirm date selection") Then Exit Sub
    End If
    'check if radio button is selected
    If rbELST.Value = False And rbFLST.Value = False Then
        MsgBox "You must select either ""Estimate Loan Sale Totals"" or ""Final Loan Sale Totals""."
        Exit Sub
    End If
    Me.Hide
    '<4->
    'get lender premium table data
    If SP.Common.TestMode() Then
        Open "X:\Sessions\Lists\Test\Lender Premium Data.txt" For Input As #1
    Else
        Open "X:\Sessions\Lists\Lender Premium Data.txt" For Input As #1
    End If
    While Not EOF(1)
        Input #1, LenderPremTbl(0, UBound(LenderPremTbl, 2)), LenderPremTbl(1, UBound(LenderPremTbl, 2)), LenderPremTbl(2, UBound(LenderPremTbl, 2))
        ReDim Preserve LenderPremTbl(2, UBound(LenderPremTbl, 2) + 1)
    Wend
    Close #1
    '</4>
'<1->
'    LoanSaleTotals Format(Date, "YY") & "*", SaleIDInfo
'    LoanSaleTotals Format(Date + 365, "YY") & "*", SaleIDInfo
'    LoanSaleTotals Format(Date - 365, "YY") & "*", SaleIDInfo
    LoanSaleTotals Format(Date, "YY") & "*", Nelnet, UHEAA
    LoanSaleTotals Format(Date + 365, "YY") & "*", Nelnet, UHEAA
    LoanSaleTotals Format(Date - 365, "YY") & "*", Nelnet, UHEAA
'    If UBound(SaleIDInfo, 2) = 0 Then 'check if any results were found
'        MsgBox "Processing Complete.  There were no results found for the entered criteria."
'        End
'    End If
'   SumTotals SaleIDInfo, FinalSaleIDList
'<4>    SumTotals Nelnet, FinalNelnet
'<4>    SumTotals UHEAA, FinalUHEAA
    SumTotals Nelnet, FinalNelnet, FinalNelnetNS, NelSaleTypeCouldNotBeDetermined           '<4>
    SumTotals UHEAA, FinalUHEAA, FinalUHEAANS, UHEAASaleTypeCouldNotBeDetermined              '<4>
    If UBound(Nelnet, 2) = 0 And UBound(UHEAA, 2) = 0 Then 'check if any results were found
        MsgBox "Processing Complete.  There were no results found for the entered criteria."
        End
    End If
    'Create Report
'    GenerateReport FinalSaleIDList
'<4>    GenerateReport FinalNelnet, FinalUHEAA
    GenerateReport FinalNelnet, FinalUHEAA, FinalNelnetNS, FinalUHEAANS, NelSaleTypeCouldNotBeDetermined, UHEAASaleTypeCouldNotBeDetermined
'</1>
    MsgBox "Processing Complete"
    End
End Sub

'This function creates the Excel Report
'<1>Function GenerateReport(Final() As String)
Function GenerateReport(N() As String, U() As String, NNS() As String, UNS() As String, NNoSaleType() As String, UNoSaleType() As String)
    Dim I As Integer
    Dim Row As Integer
    Dim ExcelApp As Excel.Application
    Set ExcelApp = CreateObject("Excel.Application")
        'sort comma delimited file in excel, create report, print report
        With ExcelApp
            'standard loan sale *******************************************************************
            'Nelnet ************************************************************
            .Visible = True
            .Workbooks.Add
            .Sheets("Sheet1").Select
            .Sheets("Sheet1").Name = "Standard"
            'create heading
            .Range("A1:F2").Select
            .Selection.Merge
            .Range("A1:F2").Select
            If rbFLST.Value Then
                .ActiveCell.FormulaR1C1 = "Nelnet Loan Sale Totals For: " & Cal1.Value
            Else
                .ActiveCell.FormulaR1C1 = "Nelnet Estimated Loan Sale Totals For: " & Cal1.Value
            End If
            .Selection.HorizontalAlignment = xlCenter
            .Selection.VerticalAlignment = xlCenter
            .Selection.Font.Size = 14
            .Range("A1:F3").Select
            .Selection.Font.Bold = True
            With .Selection.Interior
                .ColorIndex = 15
                .Pattern = xlSolid
            End With
            Row = 3
            'write out header rows
            .Worksheets("Standard").Cells(Row, 1).Value = "Lender ID"
            .Worksheets("Standard").Cells(Row, 2).Value = "Lender Name"
            .Worksheets("Standard").Cells(Row, 3).Value = "Principal"
            .Worksheets("Standard").Cells(Row, 4).Value = "Interest"
            .Worksheets("Standard").Cells(Row, 5).Value = "# of Borr"
            .Worksheets("Standard").Cells(Row, 6).Value = "# of Loans"
            Row = Row + 1
            'write out data rows
            For I = 0 To UBound(N, 2) - 1
                .Worksheets("Standard").Cells(Row, 1).Value = N(0, I)
                .Worksheets("Standard").Cells(Row, 2).Value = N(1, I)
                .Worksheets("Standard").Cells(Row, 3).Value = Format(N(2, I), "$###,###,##0.00")
                .Worksheets("Standard").Cells(Row, 4).Value = Format(N(3, I), "$###,###,##0.00")
                .Worksheets("Standard").Cells(Row, 5).Value = N(4, I)
                .Worksheets("Standard").Cells(Row, 6).Value = N(5, I)
            Row = Row + 1
            Next
            'sort rows
            .Cells.Select
            'auto fit all columns
            .Columns("A:F").Select
            .Selection.Columns.AutoFit
            'UHEAA *****************************************************************
            'create heading
            .Range("H1:M2").Select
            .Selection.Merge
            .Range("H1:M2").Select
            If rbFLST.Value Then
                .ActiveCell.FormulaR1C1 = "UHEAA Loan Sale Totals For: " & Cal1.Value
            Else
                .ActiveCell.FormulaR1C1 = "UHEAA Estimated Loan Sale Totals For: " & Cal1.Value
            End If
            .Selection.HorizontalAlignment = xlCenter
            .Selection.VerticalAlignment = xlCenter
            .Selection.Font.Size = 14
            .Range("H1:M3").Select
            .Selection.Font.Bold = True
            With .Selection.Interior
                .ColorIndex = 15
                .Pattern = xlSolid
            End With
            Row = 3
            'write out header rows
            .Worksheets("Standard").Cells(Row, 8).Value = "Lender ID"
            .Worksheets("Standard").Cells(Row, 9).Value = "Lender Name"
            .Worksheets("Standard").Cells(Row, 10).Value = "Principal"
            .Worksheets("Standard").Cells(Row, 11).Value = "Interest"
            .Worksheets("Standard").Cells(Row, 12).Value = "# of Borr"
            .Worksheets("Standard").Cells(Row, 13).Value = "# of Loans"
            Row = Row + 1
            'write out data rows
            For I = 0 To UBound(U, 2) - 1
                .Worksheets("Standard").Cells(Row, 8).Value = U(0, I)
                .Worksheets("Standard").Cells(Row, 9).Value = U(1, I)
                .Worksheets("Standard").Cells(Row, 10).Value = Format(U(2, I), "$###,###,##0.00")
                .Worksheets("Standard").Cells(Row, 11).Value = Format(U(3, I), "$###,###,##0.00")
                .Worksheets("Standard").Cells(Row, 12).Value = U(4, I)
                .Worksheets("Standard").Cells(Row, 13).Value = U(5, I)
            Row = Row + 1
            Next
            'sort rows
            .Cells.Select
            'auto fit all columns
            .Columns("H:M").Select
            .Selection.Columns.AutoFit
            'Non standard loan sale *******************************************************************
            .Sheets("Sheet2").Select
            .Sheets("Sheet2").Name = "Non Standard"
            'Nelnet ************************************************************
            'create heading
            .Range("A1:F2").Select
            .Selection.Merge
            .Range("A1:F2").Select
            If rbFLST.Value Then
                .ActiveCell.FormulaR1C1 = "Nelnet Loan Sale Totals For: " & Cal1.Value
            Else
                .ActiveCell.FormulaR1C1 = "Nelnet Estimated Loan Sale Totals For: " & Cal1.Value
            End If
            .Selection.HorizontalAlignment = xlCenter
            .Selection.VerticalAlignment = xlCenter
            .Selection.Font.Size = 14
            .Range("A1:F3").Select
            .Selection.Font.Bold = True
            With .Selection.Interior
                .ColorIndex = 15
                .Pattern = xlSolid
            End With
            Row = 3
            'write out header rows
            .Worksheets("Non Standard").Cells(Row, 1).Value = "Lender ID"
            .Worksheets("Non Standard").Cells(Row, 2).Value = "Lender Name"
            .Worksheets("Non Standard").Cells(Row, 3).Value = "Principal"
            .Worksheets("Non Standard").Cells(Row, 4).Value = "Interest"
            .Worksheets("Non Standard").Cells(Row, 5).Value = "# of Borr"
            .Worksheets("Non Standard").Cells(Row, 6).Value = "# of Loans"
            Row = Row + 1
            'write out data rows
            For I = 0 To UBound(NNS, 2) - 1
                .Worksheets("Non Standard").Cells(Row, 1).Value = NNS(0, I)
                .Worksheets("Non Standard").Cells(Row, 2).Value = NNS(1, I)
                .Worksheets("Non Standard").Cells(Row, 3).Value = Format(NNS(2, I), "$###,###,##0.00")
                .Worksheets("Non Standard").Cells(Row, 4).Value = Format(NNS(3, I), "$###,###,##0.00")
                .Worksheets("Non Standard").Cells(Row, 5).Value = NNS(4, I)
                .Worksheets("Non Standard").Cells(Row, 6).Value = NNS(5, I)
            Row = Row + 1
            Next
            'sort rows
            .Cells.Select
            'auto fit all columns
            .Columns("A:F").Select
            .Selection.Columns.AutoFit
            'UHEAA *****************************************************************
            'create heading
            .Range("H1:M2").Select
            .Selection.Merge
            .Range("H1:M2").Select
            If rbFLST.Value Then
                .ActiveCell.FormulaR1C1 = "UHEAA Loan Sale Totals For: " & Cal1.Value
            Else
                .ActiveCell.FormulaR1C1 = "UHEAA Estimated Loan Sale Totals For: " & Cal1.Value
            End If
            .Selection.HorizontalAlignment = xlCenter
            .Selection.VerticalAlignment = xlCenter
            .Selection.Font.Size = 14
            .Range("H1:M3").Select
            .Selection.Font.Bold = True
            With .Selection.Interior
                .ColorIndex = 15
                .Pattern = xlSolid
            End With
            Row = 3
            'write out header rows
            .Worksheets("Non Standard").Cells(Row, 8).Value = "Lender ID"
            .Worksheets("Non Standard").Cells(Row, 9).Value = "Lender Name"
            .Worksheets("Non Standard").Cells(Row, 10).Value = "Principal"
            .Worksheets("Non Standard").Cells(Row, 11).Value = "Interest"
            .Worksheets("Non Standard").Cells(Row, 12).Value = "# of Borr"
            .Worksheets("Non Standard").Cells(Row, 13).Value = "# of Loans"
            Row = Row + 1
            'write out data rows
            For I = 0 To UBound(UNS, 2) - 1
                .Worksheets("Non Standard").Cells(Row, 8).Value = UNS(0, I)
                .Worksheets("Non Standard").Cells(Row, 9).Value = UNS(1, I)
                .Worksheets("Non Standard").Cells(Row, 10).Value = Format(UNS(2, I), "$###,###,##0.00")
                .Worksheets("Non Standard").Cells(Row, 11).Value = Format(UNS(3, I), "$###,###,##0.00")
                .Worksheets("Non Standard").Cells(Row, 12).Value = UNS(4, I)
                .Worksheets("Non Standard").Cells(Row, 13).Value = UNS(5, I)
            Row = Row + 1
            Next
            'sort rows
            .Cells.Select
            'auto fit all columns
            .Columns("H:M").Select
            .Selection.Columns.AutoFit
            ' No Sale Type *************************************************************************
            .Sheets("Sheet3").Select
            .Sheets("Sheet3").Name = "No Sale Type"
            'Nelnet ************************************************************
            'create heading
            .Range("A1:H2").Select
            .Selection.Merge
            .Range("A1:H2").Select
            If rbFLST.Value Then
                .ActiveCell.FormulaR1C1 = "Nelnet No Sale Type List for: " & Cal1.Value
            Else
                .ActiveCell.FormulaR1C1 = "Nelnet Estimated No Sale Type List for: " & Cal1.Value
            End If
            .Selection.HorizontalAlignment = xlCenter
            .Selection.VerticalAlignment = xlCenter
            .Selection.Font.Size = 14
            .Range("A1:H3").Select
            .Selection.Font.Bold = True
            With .Selection.Interior
                .ColorIndex = 15
                .Pattern = xlSolid
            End With
            Row = 3
            'write out header rows
            .Worksheets("No Sale Type").Cells(Row, 1).Value = "Lender ID"
            .Worksheets("No Sale Type").Cells(Row, 2).Value = "Lender Name"
            .Worksheets("No Sale Type").Cells(Row, 3).Value = "Sale ID"
            .Worksheets("No Sale Type").Cells(Row, 4).Value = "Principal"
            .Worksheets("No Sale Type").Cells(Row, 5).Value = "Interest"
            .Worksheets("No Sale Type").Cells(Row, 6).Value = "# of Borr"
            .Worksheets("No Sale Type").Cells(Row, 7).Value = "# of Loans"
            .Worksheets("No Sale Type").Cells(Row, 8).Value = "Bond ID"
            Row = Row + 1
            'write out data rows
            'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
            For I = 0 To UBound(NNoSaleType, 2) - 1
                .Worksheets("No Sale Type").Cells(Row, 1).Value = NNoSaleType(0, I)
                .Worksheets("No Sale Type").Cells(Row, 2).Value = NNoSaleType(1, I)
                .Worksheets("No Sale Type").Cells(Row, 3).Value = NNoSaleType(11, I)
                .Worksheets("No Sale Type").Cells(Row, 4).Value = Format(NNoSaleType(2, I), "$###,###,##0.00")
                .Worksheets("No Sale Type").Cells(Row, 5).Value = Format(NNoSaleType(3, I), "$###,###,##0.00")
                .Worksheets("No Sale Type").Cells(Row, 6).Value = NNoSaleType(4, I)
                .Worksheets("No Sale Type").Cells(Row, 7).Value = NNoSaleType(5, I)
                .Worksheets("No Sale Type").Cells(Row, 8).Value = NNoSaleType(6, I)
                Row = Row + 1
            Next
            'sort rows
            .Cells.Select
            'auto fit all columns
            .Columns("A:H").Select
            .Selection.Columns.AutoFit
            'UHEAA *****************************************************************
            'create heading
            .Range("J1:Q2").Select
            .Selection.Merge
            .Range("J1:Q2").Select
            If rbFLST.Value Then
                .ActiveCell.FormulaR1C1 = "UHEAA No Sale Type List for: " & Cal1.Value
            Else
                .ActiveCell.FormulaR1C1 = "UHEAA Estimated No Sale Type List for: " & Cal1.Value
            End If
            .Selection.HorizontalAlignment = xlCenter
            .Selection.VerticalAlignment = xlCenter
            .Selection.Font.Size = 14
            .Range("J1:Q3").Select
            .Selection.Font.Bold = True
            With .Selection.Interior
                .ColorIndex = 15
                .Pattern = xlSolid
            End With
            Row = 3
            'write out header rows
            .Worksheets("No Sale Type").Cells(Row, 10).Value = "Lender ID"
            .Worksheets("No Sale Type").Cells(Row, 11).Value = "Lender Name"
            .Worksheets("No Sale Type").Cells(Row, 12).Value = "Sale ID"
            .Worksheets("No Sale Type").Cells(Row, 13).Value = "Principal"
            .Worksheets("No Sale Type").Cells(Row, 14).Value = "Interest"
            .Worksheets("No Sale Type").Cells(Row, 15).Value = "# of Borr"
            .Worksheets("No Sale Type").Cells(Row, 16).Value = "# of Loans"
            .Worksheets("No Sale Type").Cells(Row, 17).Value = "Bond ID"
            Row = Row + 1
            'write out data rows
            'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
            For I = 0 To UBound(UNoSaleType, 2) - 1
                .Worksheets("No Sale Type").Cells(Row, 10).Value = UNoSaleType(0, I)
                .Worksheets("No Sale Type").Cells(Row, 11).Value = UNoSaleType(1, I)
                .Worksheets("No Sale Type").Cells(Row, 12).Value = UNoSaleType(11, I)
                .Worksheets("No Sale Type").Cells(Row, 13).Value = Format(UNoSaleType(2, I), "$###,###,##0.00")
                .Worksheets("No Sale Type").Cells(Row, 14).Value = Format(UNoSaleType(3, I), "$###,###,##0.00")
                .Worksheets("No Sale Type").Cells(Row, 15).Value = UNoSaleType(4, I)
                .Worksheets("No Sale Type").Cells(Row, 16).Value = UNoSaleType(5, I)
                .Worksheets("No Sale Type").Cells(Row, 17).Value = UNoSaleType(6, I)
                Row = Row + 1
            Next
            'sort rows
            .Cells.Select
            'auto fit all columns
            .Columns("J:Q").Select
            .Selection.Columns.AutoFit
            'select first sheet
            .Sheets("Standard").Select
        End With
End Function

'this function sums totals and creates a list with one row per lender
Function SumTotals(SII() As String, Final() As String, FinalNS() As String, STCNBD() As String)
    'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
    Dim I1 As Integer
    Dim I2 As Integer
    Dim I As Integer        '<4>
    'load lender list
'<1>Open "X:\Sessions\Lists\LenderListForLnSaleInfo.txt" For Input As #1
    Open "X:\Sessions\Lists\LenderList.txt" For Input As #1
    While Not EOF(1)
        Input #1, Final(0, UBound(Final, 2)), Final(1, UBound(Final, 2))
        Final(0, UBound(Final, 2)) = Replace(Final(0, UBound(Final, 2)), "UT", "")
        Final(2, UBound(Final, 2)) = 0
        Final(3, UBound(Final, 2)) = 0
        Final(4, UBound(Final, 2)) = 0
        Final(5, UBound(Final, 2)) = 0
'<4->
        'populate non standard list
        FinalNS(0, UBound(FinalNS, 2)) = Final(0, UBound(Final, 2))
        FinalNS(1, UBound(FinalNS, 2)) = Final(1, UBound(Final, 2))
        FinalNS(2, UBound(FinalNS, 2)) = 0
        FinalNS(3, UBound(FinalNS, 2)) = 0
        FinalNS(4, UBound(FinalNS, 2)) = 0
        FinalNS(5, UBound(FinalNS, 2)) = 0
        ReDim Preserve FinalNS(5, UBound(FinalNS, 2) + 1)
'</4>
        ReDim Preserve Final(5, UBound(Final, 2) + 1)
    Wend
    Close #1
    'cycle through current list
    For I1 = 0 To UBound(SII, 2) - 1
        '<4>MatchingIDFound = False
        'cycle through lists being created
        'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
        If (SII(7, I1) <> "T" And SII(10, I1) = "S") Or _
           (SII(7, I1) = "T" And (SII(8, I1) = "MONTHLY LOAN SALE" Or SII(8, I1) = "SPECIAL SALE") And (SII(9, I1) = "LOANS WITH ORIGINATION FEE AND WITH ZERO ORIGINATION FEE" Or SII(9, I1) = "LOANS WITH ORIGINATION FEE ONLY")) Or _
           (SII(7, I1) = "T" And StandOrNonStand(SII(0, I1)) = "Standard") Then
            'standard
            For I2 = 0 To UBound(Final, 2) - 1
                If Mid(SII(0, I1), 1, 6) = Final(0, I2) Then
                    Final(2, I2) = CDbl(SII(2, I1)) + CDbl(Final(2, I2))
                    Final(3, I2) = CDbl(SII(3, I1)) + CDbl(Final(3, I2))
                    Final(4, I2) = CLng(SII(4, I1)) + CLng(Final(4, I2))
                    Final(5, I2) = CLng(SII(5, I1)) + CLng(Final(5, I2))
                End If
            Next
        ElseIf (SII(7, I1) <> "T" And SII(10, I1) = "N") Or _
               (SII(7, I1) = "T" And (SII(8, I1) = "MONTHLY LOAN SALE" Or SII(8, I1) = "SPECIAL SALE") And SII(9, I1) = "LOANS WITH ZERO ORIGINATION FEE ONLY") Or _
               (SII(7, I1) = "T" And SII(8, I1) = "FULLY ORIGINATED 90 DAYS") Or _
               (SII(7, I1) = "T" And StandOrNonStand(SII(0, I1)) = "Non Standard") Then
               'non standard
                    For I2 = 0 To UBound(FinalNS, 2) - 1
                        If Mid(SII(0, I1), 1, 6) = FinalNS(0, I2) Then
                            FinalNS(2, I2) = CDbl(SII(2, I1)) + CDbl(FinalNS(2, I2))
                            FinalNS(3, I2) = CDbl(SII(3, I1)) + CDbl(FinalNS(3, I2))
                            FinalNS(4, I2) = CLng(SII(4, I1)) + CLng(FinalNS(4, I2))
                            FinalNS(5, I2) = CLng(SII(5, I1)) + CLng(FinalNS(5, I2))
                        End If
                    Next
        Else
            'sale type couldn't be determined
            I = 0
            'copy data to new array
            While I < UBound(STCNBD, 1) + 1
                STCNBD(I, UBound(STCNBD, 2)) = SII(I, I1)
                I = I + 1
            Wend
            'create new opening in array
            ReDim Preserve STCNBD(11, UBound(STCNBD, 2) + 1)
        End If
    Next
End Function


'checks if APR status = A
Function APRisA() As Boolean
    hit "F4" 'go to second page
    APRisA = check4text(18, 20, "A") '<5> 15 to 16, <6> 16 to 18
    hit "F12" 'go back to original page
End Function

'this function gathers the needed information from TS4P
Function LoanSaleTotals(ByVal SearchCrit As String, ByRef N() As String, ByRef U() As String)
    Dim Row As Integer
    Row = 8
'<5>fastpath "TX3ZITS4P" & SearchCrit & ";;;" & Format(Cal1.Value, "MMDDYYYY")
    fastpath "TX3ZITS4P" & SearchCrit & ";;;;;" & Format(Cal1.Value, "MMDDYYYY")
'</5>
'<1->
    If check4text(1, 75, "TSX4Q") = False Then 'only process if results are found
        If check4text(1, 75, "TSX4R") Then 'target screen
            If rbELST.Value Then 'if the request is for estimated totals
                If check4text(4, 70, "OPEN") And APRisA() Then '<5> from 69 to 70
                    'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                    If check4text(9, 20, "828476") Then  'if uheaa '<5> 10 to 9
                        If check4text(6, 20, "828476") = False Then                         '<4>
                            'if uheaa isn't the seller as well then collect info
                            'if uheaa is the seller then it is a bond swap and shouldn't be collected
                            U(0, UBound(U, 2)) = GetText(6, 20, 10)
                            U(1, UBound(U, 2)) = GetText(6, 31, 50)
                            U(6, UBound(U, 2)) = Replace(GetText(11, 20, 8), "_", "") '<5> 12 to 11
'<4->
                            'get loan sale type info
                            hit "F4"
                            'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                            If check4text(9, 18, "T") Then '<5> 8 to 9
                                U(7, UBound(U, 2)) = "T"
                                hit "F10"
                                U(8, UBound(U, 2)) = GetText(5, 15, 63)
                                U(9, UBound(U, 2)) = GetText(6, 15, 63)
                                hit "F12"
                            Else
                                U(7, UBound(U, 2)) = ""
                                PopulatePremiumVal U(0, UBound(U, 2)), U(10, UBound(U, 2)), U(11, UBound(U, 2))
                            End If
                            hit "F12"
'</4>
                            hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
                            U(2, UBound(U, 2)) = Replace(GetText(11, 30, 16), "$", "")
                            U(3, UBound(U, 2)) = Replace(GetText(11, 49, 13), "$", "")
                            U(4, UBound(U, 2)) = GetText(11, 10, 8)
                            U(5, UBound(U, 2)) = GetText(11, 19, 8)
                            ReDim Preserve U(11, UBound(U, 2) + 1) 'add one empty slot in the array
                        End If                                                              '<4>
                    ElseIf check4text(9, 20, "888885") = False Then  'if not uheaa and not SLMA '<5>10 to 9
                        If check4text(9, 20, "813760UT") = False Or check4text(6, 20, "813760") = False Then             '<4> '<5>10 to 9
                            N(0, UBound(N, 2)) = GetText(6, 20, 10)
                            N(1, UBound(N, 2)) = GetText(6, 31, 50)
                            N(6, UBound(N, 2)) = Replace(GetText(11, 20, 8), "_", "") '<5> 12 to 11
'<4->
                            'get loan sale type info
                            hit "F4"
                            'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                            If check4text(9, 18, "T") Then '<5> 8 to 9
                                N(7, UBound(N, 2)) = "T"
                                hit "F10"
                                N(8, UBound(N, 2)) = GetText(5, 15, 63)
                                N(9, UBound(N, 2)) = GetText(6, 15, 63)
                                hit "F12"
                            Else
                                N(7, UBound(N, 2)) = ""
                                PopulatePremiumVal N(0, UBound(N, 2)), N(10, UBound(N, 2)), N(11, UBound(N, 2))
                            End If
                            hit "F12"
'</4>
                            hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
                            N(2, UBound(N, 2)) = Replace(GetText(11, 30, 16), "$", "")
                            N(3, UBound(N, 2)) = Replace(GetText(11, 49, 13), "$", "")
                            N(4, UBound(N, 2)) = GetText(11, 10, 8)
                            N(5, UBound(N, 2)) = GetText(11, 19, 8)
                            ReDim Preserve N(11, UBound(N, 2) + 1) 'add one empty slot in the array
                        End If
                    End If
                End If
            Else 'if the request is for final totals
                If check4text(4, 70, "COMPLETE") Then '<5> 69 to 70
                    'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                    If check4text(9, 20, "828476") Then  'if uheaa '<5> 10 to 9
                        If check4text(6, 20, "828476") = False Then             '<4>
                            'if uheaa isn't the seller as well then collect info
                            'if uheaa is the seller then it is a bond swap and shouldn't be collected
                            U(0, UBound(U, 2)) = GetText(6, 20, 10)
                            U(1, UBound(U, 2)) = GetText(6, 31, 50)
                            U(6, UBound(U, 2)) = Replace(GetText(11, 20, 8), "_", "") '<5> 12 to 11
'<4->
                            'get loan sale type info
                            hit "F4"
                            'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                            If check4text(9, 18, "T") Then '<5> 8 to 9
                                U(7, UBound(U, 2)) = "T"
                                hit "F10"
                                U(8, UBound(U, 2)) = GetText(5, 15, 63)
                                U(9, UBound(U, 2)) = GetText(6, 15, 63)
                                hit "F12"
                            Else
                                U(7, UBound(U, 2)) = ""
                                PopulatePremiumVal U(0, UBound(U, 2)), U(10, UBound(U, 2)), U(11, UBound(U, 2))
                            End If
                            hit "F12"
'</4>
                            hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
                            U(2, UBound(U, 2)) = Replace(GetText(11, 30, 16), "$", "")
                            U(3, UBound(U, 2)) = Replace(GetText(11, 49, 13), "$", "")
                            U(4, UBound(U, 2)) = GetText(11, 10, 8)
                            U(5, UBound(U, 2)) = GetText(11, 19, 8)
                            ReDim Preserve U(11, UBound(U, 2) + 1) 'add one empty slot in the array
                        End If                                                  '<4>
                    ElseIf check4text(9, 20, "888885") = False Then  'if not uheaa and not SLMA '<5> 10 to 9
                        If check4text(9, 20, "813760UT") = False Or check4text(6, 20, "813760") = False Then             '<4> '<5> 10 to 9
                            N(0, UBound(N, 2)) = GetText(6, 20, 10)
                            N(1, UBound(N, 2)) = GetText(6, 31, 50)
                            N(6, UBound(N, 2)) = Replace(GetText(11, 20, 8), "_", "") '<5> 12 to 11
'<4->
                            'get loan sale type info
                            hit "F4"
                            'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                            If check4text(9, 18, "T") Then '<5> 8 to 9
                                N(7, UBound(N, 2)) = "T"
                                hit "F10"
                                N(8, UBound(N, 2)) = GetText(5, 15, 63)
                                N(9, UBound(N, 2)) = GetText(6, 15, 63)
                                hit "F12"
                            Else
                                N(7, UBound(N, 2)) = ""
                                PopulatePremiumVal N(0, UBound(N, 2)), N(10, UBound(N, 2)), N(11, UBound(N, 2))
                            End If
                            hit "F12"
'</4>
                            hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
                            N(2, UBound(N, 2)) = Replace(GetText(11, 30, 16), "$", "")
                            N(3, UBound(N, 2)) = Replace(GetText(11, 49, 13), "$", "")
                            N(4, UBound(N, 2)) = GetText(11, 10, 8)
                            N(5, UBound(N, 2)) = GetText(11, 19, 8)
                            ReDim Preserve N(11, UBound(N, 2) + 1) 'add one empty slot in the array
                        End If
                    End If
                End If
            End If
        Else 'selection screen
            If rbELST.Value Then 'if the request is for estimated totals
                While check4text(22, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    If check4text(Row, 73, "OPEN") And check4text(Row, 79, "A") Then
                        'blank selection field
                        puttext 21, 18, "", "End"
                        puttext 21, 18, GetText(Row, 2, 2)
                        hit "Enter" 'go to target screen
                        'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                        If check4text(9, 20, "828476") Then  'if uheaa '<5> 10 to 9
                            If check4text(6, 20, "828476") = False Then                         '<4>
                                'if uheaa isn't the seller as well then collect info
                                'if uheaa is the seller then it is a bond swap and shouldn't be collected
                                U(11, UBound(U, 2)) = GetText(4, 11, 9)                 '<4>
                                U(0, UBound(U, 2)) = GetText(6, 20, 10)
                                U(1, UBound(U, 2)) = GetText(6, 31, 50)
                                U(6, UBound(U, 2)) = Replace(GetText(11, 20, 8), "_", "") '<5> 12 to 11
'<4->
                                'get loan sale type info
                                hit "F4"
                                'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                                If check4text(9, 18, "T") Then '<5> 8 to 9
                                    U(7, UBound(U, 2)) = "T"
                                    hit "F10"
                                    U(8, UBound(U, 2)) = GetText(5, 15, 63)
                                    U(9, UBound(U, 2)) = GetText(6, 15, 63)
                                    hit "F12"
                                Else
                                    U(7, UBound(U, 2)) = ""
                                    PopulatePremiumVal U(0, UBound(U, 2)), U(10, UBound(U, 2)), U(11, UBound(U, 2))
                                End If
                                hit "F12"
'</4>
                                hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
                                U(2, UBound(U, 2)) = Replace(GetText(11, 30, 16), "$", "")
                                U(3, UBound(U, 2)) = Replace(GetText(11, 49, 13), "$", "")
                                U(4, UBound(U, 2)) = GetText(11, 10, 8)
                                U(5, UBound(U, 2)) = GetText(11, 19, 8)
                                ReDim Preserve U(11, UBound(U, 2) + 1) 'add one empty slot in the array
                                hit "F12"
                            End If                                                              '<4>
                        ElseIf check4text(9, 20, "888885") = False Then  'if not uheaa and not SLMA '<5> 10 to 9
                            If check4text(9, 20, "813760UT") = False Or check4text(6, 20, "813760") = False Then             '<4> '<5> 10 to 9
                                N(11, UBound(N, 2)) = GetText(4, 11, 9)                 '<4>
                                N(0, UBound(N, 2)) = GetText(6, 20, 10)
                                N(1, UBound(N, 2)) = GetText(6, 31, 50)
                                N(6, UBound(N, 2)) = Replace(GetText(11, 20, 8), "_", "") '<5> 12 to 11
'<4->
                                'get loan sale type info
                                hit "F4"
                                'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                                If check4text(9, 18, "T") Then '<5> 8 to 9
                                    N(7, UBound(N, 2)) = "T"
                                    hit "F10"
                                    N(8, UBound(N, 2)) = GetText(5, 15, 63)
                                    N(9, UBound(N, 2)) = GetText(6, 15, 63)
                                    hit "F12"
                                Else
                                    N(7, UBound(N, 2)) = ""
                                    PopulatePremiumVal N(0, UBound(N, 2)), N(10, UBound(N, 2)), N(11, UBound(N, 2))
                                End If
                                hit "F12"
'</4>
                                hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
                                N(2, UBound(N, 2)) = Replace(GetText(11, 30, 16), "$", "")
                                N(3, UBound(N, 2)) = Replace(GetText(11, 49, 13), "$", "")
                                N(4, UBound(N, 2)) = GetText(11, 10, 8)
                                N(5, UBound(N, 2)) = GetText(11, 19, 8)
                                ReDim Preserve N(11, UBound(N, 2) + 1) 'add one empty slot in the array
                                hit "F12"
                            End If
                        End If
                        hit "F12" 'go back to selection screen
                    End If
                    Row = Row + 1
                    If check4text(Row, 73, "    ") Then 'check for page forward
                        Row = 8
                        hit "F8"
                    End If
                Wend
            Else 'if the request is for final totals
                While check4text(22, 2, "90007 NO MORE DATA TO DISPLAY") = False
                    If check4text(Row, 73, "COMP") Then
                        'blank selection field
                        puttext 21, 18, "", "End"
                        puttext 21, 18, GetText(Row, 2, 2)
                        hit "Enter" 'go to target screen
                        'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                        If check4text(9, 20, "828476") Then  'if uheaa '<5> 10 to 9
                            If check4text(6, 20, "828476") = False Then                         '<4>
                                'if uheaa isn't the seller as well then collect info
                                'if uheaa is the seller then it is a bond swap and shouldn't be collected
                                U(11, UBound(U, 2)) = GetText(4, 11, 9)                 '<4>
                                U(0, UBound(U, 2)) = GetText(6, 20, 10)
                                U(1, UBound(U, 2)) = GetText(6, 31, 50)
                                U(6, UBound(U, 2)) = Replace(GetText(11, 20, 8), "_", "") '<5> 12 to 11
'<4->
                                'get loan sale type info
                                hit "F4"
                                'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                                If check4text(9, 18, "T") Then '<5> 8 to 9
                                    U(7, UBound(U, 2)) = "T"
                                    hit "F10"
                                    U(8, UBound(U, 2)) = GetText(5, 15, 63)
                                    U(9, UBound(U, 2)) = GetText(6, 15, 63)
                                    hit "F12"
                                Else
                                    U(7, UBound(U, 2)) = ""
                                    PopulatePremiumVal U(0, UBound(U, 2)), U(10, UBound(U, 2)), U(11, UBound(U, 2))
                                End If
                                hit "F12"
'</4>
                                hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
                                U(2, UBound(U, 2)) = Replace(GetText(11, 30, 16), "$", "")
                                U(3, UBound(U, 2)) = Replace(GetText(11, 49, 13), "$", "")
                                U(4, UBound(U, 2)) = GetText(11, 10, 8)
                                U(5, UBound(U, 2)) = GetText(11, 19, 8)
                                ReDim Preserve U(11, UBound(U, 2) + 1) 'add one empty slot in the array
                                hit "F12"
                            End If                                                              '<4>
                        ElseIf check4text(9, 20, "888885") = False Then  'if not uheaa and not SLMA '<5> 10 to 9
                            If check4text(9, 20, "813760UT") = False Or check4text(6, 20, "813760") = False Then             '<4> '<5> 10 to 9
    '<3>                        N(0, UBound(N, 2)) = GetText(7, 20, 10)
                                N(11, UBound(N, 2)) = GetText(4, 11, 9)                 '<4>
                                N(0, UBound(N, 2)) = GetText(6, 20, 10)                 '<3>
                                N(1, UBound(N, 2)) = GetText(6, 31, 50)
                                N(6, UBound(N, 2)) = Replace(GetText(11, 20, 8), "_", "") '<5> 12 to 11
'<4->
                                'get loan sale type info
                                hit "F4"
                                'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
                                If check4text(9, 18, "T") Then '<5> 8 to 9
                                    N(7, UBound(N, 2)) = "T"
                                    hit "F10"
                                    N(8, UBound(N, 2)) = GetText(5, 15, 63)
                                    N(9, UBound(N, 2)) = GetText(6, 15, 63)
                                    hit "F12"
                                Else
                                    N(7, UBound(N, 2)) = ""
                                    PopulatePremiumVal N(0, UBound(N, 2)), N(10, UBound(N, 2)), N(11, UBound(N, 2))
                                End If
                                hit "F12"
'</4>
                                hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
                                N(2, UBound(N, 2)) = Replace(GetText(11, 30, 16), "$", "")
                                N(3, UBound(N, 2)) = Replace(GetText(11, 49, 13), "$", "")
                                N(4, UBound(N, 2)) = GetText(11, 10, 8)
                                N(5, UBound(N, 2)) = GetText(11, 19, 8)
                                ReDim Preserve N(11, UBound(N, 2) + 1) 'add one empty slot in the array
                                hit "F12"
                            End If
                        End If
                        hit "F12" 'go back to selection screen
                    End If
                    Row = Row + 1
                    If check4text(Row, 73, "    ") Then 'check for page forward
                        Row = 8
                        hit "F8"
                    End If
                Wend
            End If
        End If
    End If
'    If check4text(1, 72, "TSX4Q") = False Then 'only process if results are found
'        If check4text(1, 72, "TSX4R") Then 'target screen
'            If rbELST.Value Then 'if the request is for estimated totals
'                If check4text(4, 69, "OPEN") And APRisA() Then
'                    'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
'                    N(0, UBound(N, 2)) = GetText(7, 20, 10)
'                    N(1, UBound(N, 2)) = GetText(7, 31, 50)
'                    hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
'                    N(2, UBound(N, 2)) = Replace(GetText(11, 30, 16), "$", "")
'                    N(3, UBound(N, 2)) = Replace(GetText(11, 49, 13), "$", "")
'                    N(4, UBound(N, 2)) = GetText(11, 10, 8)
'                    N(5, UBound(N, 2)) = GetText(11, 19, 8)
'                    ReDim Preserve N(5, UBound(N, 2) + 1) 'add one empty slot in the array
'                End If
'            Else 'if the request is for final totals
'                If check4text(4, 69, "COMPLETE") Then
'                    'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
'                    N(0, UBound(N, 2)) = GetText(7, 20, 10)
'                    N(1, UBound(N, 2)) = GetText(7, 31, 50)
'                    hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
'                    N(2, UBound(N, 2)) = Replace(GetText(11, 30, 16), "$", "")
'                    N(3, UBound(N, 2)) = Replace(GetText(11, 49, 13), "$", "")
'                    N(4, UBound(N, 2)) = GetText(11, 10, 8)
'                    N(5, UBound(N, 2)) = GetText(11, 19, 8)
'                    ReDim Preserve N(5, UBound(N, 2) + 1) 'add one empty slot in the array
'                End If
'            End If
'        Else 'selection screen
'            If rbELST.Value Then 'if the request is for estimated totals
'                While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
'                    If check4text(Row, 73, "OPEN") And check4text(Row, 79, "A") Then
'                        'blank selection field
'                        Session.MoveCursor 21, 18
'                        hit "End"
'                        puttext 21, 18, GetText(Row, 2, 2)
'                        hit "Enter" 'go to target screen
'                        'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
'                        N(0, UBound(N, 2)) = GetText(7, 20, 10)
'                        N(1, UBound(N, 2)) = GetText(7, 31, 50)
'                        hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
'                        N(2, UBound(N, 2)) = Replace(GetText(11, 30, 16), "$", "")
'                        N(3, UBound(N, 2)) = Replace(GetText(11, 49, 13), "$", "")
'                        N(4, UBound(N, 2)) = GetText(11, 10, 8)
'                        N(5, UBound(N, 2)) = GetText(11, 19, 8)
'                        ReDim Preserve N(5, UBound(N, 2) + 1) 'add one empty slot in the array
'                        hit "F12"
'                        hit "F12" 'go back to selection screen
'                    End If
'                    Row = Row + 1
'                    If check4text(Row, 73, "    ") Then 'check for page forward
'                        Row = 8
'                        hit "F8"
'                    End If
'                Wend
'            Else 'if the request is for final totals
'                While check4text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
'                    If check4text(Row, 73, "COMP") Then
'                        'blank selection field
'                        Session.MoveCursor 21, 18
'                        hit "End"
'                        puttext 21, 18, GetText(Row, 2, 2)
'                        hit "Enter" 'go to target screen
'                        'indicies 0 = seller ID, 1 = seller Name, 2 = Principle, 3 = Interest, 4 = # of Borrowers, 5 = # of Loans, 6 = Bond ID, 7 = T or not, 8 = Type 1, 9 = type 2, 10 = premium, 11 = Sale ID
'                        N(0, UBound(N, 2)) = GetText(7, 20, 10)
'                        N(1, UBound(N, 2)) = GetText(7, 31, 50)
'                        hit "F6" 'go to LOAN SALE FINANCIAL STATISTICS "ITS9F" screen
'                        N(2, UBound(N, 2)) = Replace(GetText(11, 30, 16), "$", "")
'                        N(3, UBound(N, 2)) = Replace(GetText(11, 49, 13), "$", "")
'                        N(4, UBound(N, 2)) = GetText(11, 10, 8)
'                        N(5, UBound(N, 2)) = GetText(11, 19, 8)
'                        ReDim Preserve N(5, UBound(N, 2) + 1) 'add one empty slot in the array
'                        hit "F12"
'                        hit "F12" 'go back to selection screen
'                    End If
'                    Row = Row + 1
'                    If check4text(Row, 73, "    ") Then 'check for page forward
'                        Row = 8
'                        hit "F8"
'                    End If
'                Wend
'            End If
'        End If
'    End If
'</1>
End Function

Private Sub UserForm_Initialize()
    Cal1.Value = Date
End Sub

Private Sub UserForm_Terminate()
    End
End Sub

'<4->
'figures Premium values
Sub PopulatePremiumVal(LenderID As String, Premium As String, SaleID As String)
    'index of LenderPremTbl: 0 = lender ID, 1 = Lender name, 2 = premium
    Dim I As Integer
    Dim TempSaleID As String
    Dim TempSaleType As String
    While I < UBound(LenderPremTbl, 2)
    'check if lender ID matches
        If LenderID = LenderPremTbl(0, I) Then
            If "Non Standard" = LenderPremTbl(2, I) Then
                Premium = "N"
            Else
                Premium = "S"
            End If
            Exit Sub 'premium was found
        End If
        I = I + 1
    Wend
    'if the script exits through here then
    'search quick sale file
    On Error GoTo ErrorHandling 'catch file access collision error
    If SP.Common.TestMode() Then
        Open "X:\PADD\Compass\Test\Loan_Sales\Sale ID Type.txt" For Input As #1
    Else
        Open "X:\PADD\Compass\Loan_Sales\Sale ID Type.txt" For Input As #1
    End If
    While Not EOF(1)
        Input #1, TempSaleID, TempSaleType
        If TempSaleID = SaleID Then
            If TempSaleType = "OF" Then
                Premium = "S"
                Close #1
                Exit Sub
            Else
                Premium = "N"
                Close #1
                Exit Sub
            End If
        End If
    Wend
    Close #1
    'if the script exits through here then premium is non standard
    Premium = "N"
    Exit Sub
ErrorHandling:
    MsgBox "Someone appears to be accessing the Sale ID Type text file.  Please try again in a few minutes.  Contact Systems Support if you feel you recieved this message in error.", vbCritical
    End
End Sub

Function StandOrNonStand(LenderID As String) As String
    Dim I As Integer
    'LenderPremTbl index 0 = lender ID, 1 = Lender name, 2 = premium
    While I < UBound(LenderPremTbl, 2)
        If LenderPremTbl(0, I) = LenderID Then
            StandOrNonStand = LenderPremTbl(2, I)
            Exit Function
        End If
        I = I + 1
    Wend
    StandOrNonStand = ""
End Function
'</4>

'new sr1037, aa, 04/06/05, 04/13/05
'<1> sr1058, aa, 04/18/05,
'<2> sr1314, jd, field positions changed, see spec for details
'<3> sr1316, aa
'<4> sr1490, aa
'<5> sr1861, tp, 10/03/06
'<6> sr2107, jd
