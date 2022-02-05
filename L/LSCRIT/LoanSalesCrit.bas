Attribute VB_Name = "LoanSalesCrit"
Dim nos As Integer                  'number of sales
Dim nov As Integer                  'number of valid sales from existing file
Dim ns As String                    'new sales indicator
Dim SaleIDsSel As Variant           'one dim
Dim SaleIDsVal As Variant           'mult dim
Dim CntlRec() As String
Dim MatchCrit() As String
Dim Edited(1 To 10) As String
Dim userId As String                '<1>
Dim userName As String              '<1>

'criteria group info -- 1 to 10 = group no; 1 = type (I/E), 2 = description
Dim GrpInfo(1 To 10, 1 To 2) As String
'criteria selection info -- loan statuses; 1 to 10 = group no; 1 = relation, 2 = value(s)
Dim CritDet000(1 To 10, 1 To 2) As String   'loan statuses
Dim CritDet004(1 To 10, 1 To 2) As String   'guarantor codes
Dim CritDet005(1 To 10, 1 To 2) As String   'loan types
Dim CritDet06A(1 To 10, 1 To 2) As String   'current principal balance
Dim CritDet06B(1 To 10, 1 To 2) As String   'current principal balance
Dim CritDet007(1 To 10, 1 To 2) As String   'first disbursement date
Dim CritDet021(1 To 10, 1 To 2) As String   'separation date
Dim CritDet023(1 To 10, 1 To 2) As String   'fully originated indicator
Dim CritDet027(1 To 10, 1 To 2) As String   'days since final disbursement
Dim CritDet045(1 To 10, 1 To 2) As String   'original school code
Dim CritDet046(1 To 10, 1 To 2) As String   'current school code
Dim CritDet047(1 To 10, 1 To 2) As String   'serialized loan
Dim CritDet052(1 To 10, 1 To 2) As String   'Guaranteed Date
Dim CritDet100(1 To 10, 1 To 2) As String   'Fully disbursed as of
Dim CritDet103(1 To 10, 1 To 2) As String   'Loan Term Begin Date
Dim CritDet104(1 To 10, 1 To 2) As String   'Loan Term end Date
Dim CritDet108(1 To 10, 1 To 2) As String   'Grade Level
Dim CritDet109(1 To 10, 1 To 2) As String   'Loan Term Begin Date
Dim CritDet110(1 To 10, 1 To 2) As String   'Loan Term end Date
Dim CritDet111(1 To 10, 1 To 2) As String   'First Disbursement Date
Dim CritDet112(1 To 10, 1 To 2) As String   'Guaranteed Date

Dim SaleID As String
Dim GrpNo As String
Dim GrpTyp As String
Dim GrpDesc As String
Dim CritCd As String
Dim CritRel As String
Dim CritVal As String
Dim CritList As String

Dim ProcDir As String

Const ECASLAList As String = "('ECASLA I','ECASLA II','PRE ECASLA','NON ECASLA')" 'used for all non ECASLA sale type

'call main dialog box
Sub LoanSalesCrit()
    If Sp.Common.TestMode() Then
        ProcDir = "X:\PADD\Compass\Test\"
    Else
        ProcDir = "X:\PADD\Compass\"
    End If
    userId = Sp.Common.GetUserID()
    'the script should still be on LP40
    userName = GetText(5, 14, 30) & " " & GetText(4, 14, 30) 'gather users name
    Main_LoanSales.Show 1
End Sub

'call criteria group dialog box
Sub CritCntl()
    'get list of valid loan sales from COMPASS
    SaleIDsVal = Main_LoanSales.GetSalesInfoArray(nov)
    'get list of sales selected by user
    SaleIDsSel = Main_LoanSales.GetSelectionArray(nos)
    'get preset criteria list selected by the user
    CritList = Main_LoanSales.GetCriteria() 'returns string out of criteria index file (Monthly, Fully, Special, Etc)
    LoadCritLists  'Loads crit group and loads crit list based off "CritList" var from files to arrays special processing for edit special sale option
    'load the criteria group and criteria selection dialog boxes
    Load LSCritGrps
    LSCritGrps.CritSettings CritList 'disables controls based off criteria option chosen
    Load LSCritSel
    LSCritSel.CritSettings CritList 'disables controls based off criteria option chosen
    'pass criteria group info from array to dialog box controls
    GetGrps
    'display criteria group dialog box
    LSCritGrps.Show
    'warn the user and end the script
    MsgBox "The loan sale information has been discarded.", , "Changes not Saved"
    End
End Sub

'control access to the criteria selection dialog box
Sub LSCritSelCntl(grp, IE)
    'pass criteria info from array to dialog box controls
    GetCrit grp, IE
    'display dialog box
    LSCritSel.Show
    'pass critiera from dialog box controls back to the array
    SaveCrit (grp)
End Sub

'pass criteria info from text files to arrays
Sub LoadCritLists()
    Dim crn As Integer                  'number of record from SaleIDSel array used as control record for prepopulating criteria info
    'pass criteria info from text file from Cyprus if the user wishes to edit existing criteria
    If Mid(CritList, 1, 12) = "Edit Special" Then
        'warn the user and use blank values or end the script if the file is empty
        If FileLen(ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt") = 0 Then
            'since there is nothing to edit
            If Mid(Main_LoanSales.GetSaleType(), 1, 11) = "Special Pre" Then
                CritList = "Special Pre ECASLA Sale"
            ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 9) = "ECASLA II" Then
                CritList = "Special ECASLA II Sale"
            ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 8) = "ECASLA I" Then
                CritList = "Special ECASLA I Sale"
            ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 10) = "Non ECASLA" Then
                CritList = "Special Non ECASLA Sale"
            ElseIf Main_LoanSales.GetSaleType() = "Special OF" Then
                CritList = "Special Sale - SO"
            ElseIf Main_LoanSales.GetSaleType() = "Special ZF" Then
                CritList = "Special Sale - SZ"
            ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 7) = "Special" Then
                CritList = "Special Sale"
            End If
            LoadCritLists
            Exit Sub
        End If
        'pass criteria group info
        SaleID = ""
        'find the first selected loan sale that is in the existing Cyprus file to use the values for that sale to prepopulate the dialog boxes
        crn = 0
        Do
            Open ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt" For Input As #1
            Do Until SaleID = SaleIDsSel(crn) Or EOF(1)
                Input #1, SaleID, GrpNo, GrpInfo(1, 1), GrpInfo(1, 2)
            Loop
            'if the sale was not found, look for the next sale
            If EOF(1) = True Then
                Close #1
                crn = crn + 1
            Else
                Exit Do
            End If
            'if the counter equals the number of sales created, none of the selected sales were found in the file of existing criteria
            If crn = nos Then
                GrpInfo(1, 1) = ""
                GrpInfo(1, 2) = ""
                Close #1
                'since there is nothing to edit
                If Mid(Main_LoanSales.GetSaleType(), 1, 11) = "Special Pre" Then
                    CritList = "Special Pre ECASLA Sale"
                ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 9) = "ECASLA II" Then
                    CritList = "Special ECASLA II Sale"
                ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 8) = "ECASLA I" Then
                    CritList = "Special ECASLA I Sale"
                ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 10) = "Non ECASLA" Then
                    CritList = "Special Non ECASLA Sale"
                ElseIf Main_LoanSales.GetSaleType() = "Special OF" Then
                    CritList = "Special Sale - SO"
                ElseIf Main_LoanSales.GetSaleType() = "Special ZF" Then
                    CritList = "Special Sale - SZ"
                ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 7) = "Special" Then
                    CritList = "Special Sale"
                End If
                LoadCritLists
                Exit Sub
            End If
        Loop
        'load info from the file until the next sale or end of file is reached
        i = 1
        Do While Not EOF(1)
            Input #1, SaleID, GrpNo, GrpTyp, GrpDesc
            If SaleID = SaleIDsSel(crn) Then
                i = i + 1
                GrpInfo(i, 1) = GrpTyp
                GrpInfo(i, 2) = GrpDesc
            Else
                Exit Do
            End If
        Loop
        Close #1
        'pass criteria selection info
        SaleID = ""
        Open ProcDir & "Loan_Sales\Working_Files\utlwo2_3.txt" For Input As #1
        Do Until SaleID = SaleIDsSel(crn)
            Input #1, SaleID, GrpNo, CritCd, CritRel, CritVal
        Loop
        GetCrit2Edit GrpNo, CritCd
        Do While Not EOF(1)
            Input #1, SaleID, GrpNo, CritCd, CritRel, CritVal
            If SaleID = SaleIDsSel(crn) Then
                GetCrit2Edit GrpNo, CritCd
            Else
                Exit Do
            End If
        Loop
        Close #1
    'pass criteria info from text file for presets selected by the user
    Else
        '** processing for all other criteria lists except edit special sale
        Open ProcDir & "Loan_Sales\Criteria_Lists\" & CritList & "_Grps.txt" For Input As #1

        i = 0
        Do Until EOF(1)
            i = i + 1
            Input #1, GrpNo, GrpInfo(i, 1), GrpInfo(i, 2)
        Loop
        Close #1
        'pass criteria selection info
        Open ProcDir & "Loan_Sales\Criteria_Lists\" & CritList & "_Crit.txt" For Input As #1
        For i = 1 To 10
            Input #1, GrpNo, CritCd, CritDet000(i, 1), CritDet000(i, 2)
            Input #1, GrpNo, CritCd, CritDet004(i, 1), CritDet004(i, 2)
            Input #1, GrpNo, CritCd, CritDet005(i, 1), CritDet005(i, 2)
            Input #1, GrpNo, CritCd, CritDet06A(i, 1), CritDet06A(i, 2)
            Input #1, GrpNo, CritCd, CritDet06B(i, 1), CritDet06B(i, 2)
            Input #1, GrpNo, CritCd, CritDet021(i, 1), CritDet021(i, 2)
            Input #1, GrpNo, CritCd, CritDet023(i, 1), CritDet023(i, 2)
            Input #1, GrpNo, CritCd, CritDet027(i, 1), CritDet027(i, 2)
            Input #1, GrpNo, CritCd, CritDet100(i, 1), CritDet100(i, 2)
            Input #1, GrpNo, CritCd, CritDet007(i, 1), CritDet007(i, 2)
            Input #1, GrpNo, CritCd, CritDet109(i, 1), CritDet109(i, 2)
            Input #1, GrpNo, CritCd, CritDet111(i, 1), CritDet111(i, 2)
            If CritDet021(i, 1) <> "" Then CritDet021(i, 2) = "('" & Format(DateValue(Format(DateAdd("m", -3, CDate(Main_LoanSales.GetSaleDate())), "MM/01/YYYY")) - 1, "MM/DD/YYYY") & "')"           '<1>
            If CritDet100(i, 1) <> "" Then CritDet100(i, 2) = "('" & Format(DateValue(Format(DateAdd("m", -3, Date), "MM/01/YYYY")) - 1, "MM/DD/YYYY") & "')"           '<1>
            'These are only used for special sales so they weren't added to the default sale crit
            CritDet045(i, 1) = ""
            CritDet045(i, 2) = ""
            CritDet046(i, 1) = ""
            CritDet046(i, 2) = ""
            CritDet047(i, 1) = ""
            CritDet047(i, 2) = ""
            CritDet052(i, 1) = ""
            CritDet052(i, 2) = ""
            CritDet103(i, 1) = ""
            CritDet103(i, 2) = ""
            CritDet104(i, 1) = ""
            CritDet104(i, 2) = ""
            CritDet108(i, 1) = ""
            CritDet108(i, 2) = ""
            CritDet110(i, 1) = ""
            CritDet110(i, 2) = ""
            CritDet112(i, 1) = ""
            CritDet112(i, 2) = ""
        Next i
        Close #1
    End If
End Sub

'pass criteria group info to array based on group being processed
Function GetCrit2Edit(GrpNo, CritCd)
    Select Case CritCd
        Case "000"
            CritDet000(GrpNo, 1) = CritRel
            CritDet000(GrpNo, 2) = CritVal
        Case "004"
            CritDet004(GrpNo, 1) = CritRel
            CritDet004(GrpNo, 2) = CritVal
        Case "005"
            CritDet005(GrpNo, 1) = CritRel
            CritDet005(GrpNo, 2) = CritVal
        Case "06A"
            CritDet06A(GrpNo, 1) = CritRel
            CritDet06A(GrpNo, 2) = CritVal
        Case "06B"
            CritDet06B(GrpNo, 1) = CritRel
            CritDet06B(GrpNo, 2) = CritVal
        Case "007"
            CritDet007(GrpNo, 1) = CritRel
            CritDet007(GrpNo, 2) = CritVal
        Case "021"
            CritDet021(GrpNo, 1) = CritRel
            CritDet021(GrpNo, 2) = CritVal
        Case "023"
            CritDet023(GrpNo, 1) = CritRel
            CritDet023(GrpNo, 2) = CritVal
        Case "027"
            CritDet027(GrpNo, 1) = CritRel
            CritDet027(GrpNo, 2) = CritVal
        Case "045"
            CritDet045(GrpNo, 1) = CritRel
            CritDet045(GrpNo, 2) = CritVal
        Case "046"
            CritDet046(GrpNo, 1) = CritRel
            CritDet046(GrpNo, 2) = CritVal
        Case "047"
            CritDet047(GrpNo, 1) = CritRel
            CritDet047(GrpNo, 2) = CritVal
        Case "052"
            CritDet052(GrpNo, 1) = CritRel
            CritDet052(GrpNo, 2) = CritVal
        Case "100"
            CritDet100(GrpNo, 1) = CritRel
            CritDet100(GrpNo, 2) = CritVal
        Case "103"
            CritDet103(GrpNo, 1) = CritRel
            CritDet103(GrpNo, 2) = CritVal
        Case "104"
            CritDet104(GrpNo, 1) = CritRel
            CritDet104(GrpNo, 2) = CritVal
        Case "108"
            CritDet108(GrpNo, 1) = CritRel
            CritDet108(GrpNo, 2) = CritVal
        Case "109"
            CritDet109(GrpNo, 1) = CritRel
            CritDet109(GrpNo, 2) = CritVal
        Case "110"
            CritDet110(GrpNo, 1) = CritRel
            CritDet110(GrpNo, 2) = CritVal
        Case "111"
            CritDet111(GrpNo, 1) = CritRel
            CritDet111(GrpNo, 2) = CritVal
        Case "112"
            CritDet112(GrpNo, 1) = CritRel
            CritDet112(GrpNo, 2) = CritVal
    End Select
End Function

'pass criteria group info from array to dialog box controls
Sub GetGrps()
    '**AA criteria group info = GrpInfo -- 1 to 10 = group no; 1 = type (I/E), 2 = description
    LSCritGrps.Typ1.Caption = GrpInfo(1, 1)
    LSCritGrps.Desc1.Value = GrpInfo(1, 2)
    LSCritGrps.Typ2.Caption = GrpInfo(2, 1)
    LSCritGrps.Desc2.Value = GrpInfo(2, 2)
    LSCritGrps.Typ3.Caption = GrpInfo(3, 1)
    LSCritGrps.Desc3.Value = GrpInfo(3, 2)
    LSCritGrps.Typ4.Caption = GrpInfo(4, 1)
    LSCritGrps.Desc4.Value = GrpInfo(4, 2)
    LSCritGrps.Typ5.Caption = GrpInfo(5, 1)
    LSCritGrps.Desc5.Value = GrpInfo(5, 2)
    LSCritGrps.Typ6.Caption = GrpInfo(6, 1)
    LSCritGrps.Desc6.Value = GrpInfo(6, 2)
    LSCritGrps.Typ7.Caption = GrpInfo(7, 1)
    LSCritGrps.Desc7.Value = GrpInfo(7, 2)
    LSCritGrps.Typ8.Caption = GrpInfo(8, 1)
    LSCritGrps.Desc8.Value = GrpInfo(8, 2)
    LSCritGrps.Typ9.Caption = GrpInfo(9, 1)
    LSCritGrps.Desc9.Value = GrpInfo(9, 2)
    LSCritGrps.Typ10.Caption = GrpInfo(10, 1)
    LSCritGrps.Desc10.Value = GrpInfo(10, 2)
    'disable description boxes if populated
    If LSCritGrps.Desc1.Value <> "" Then LSCritGrps.Desc1.Enabled = False
    If LSCritGrps.Desc2.Value <> "" Then LSCritGrps.Desc2.Enabled = False
    If LSCritGrps.Desc3.Value <> "" Then LSCritGrps.Desc3.Enabled = False
    If LSCritGrps.Desc4.Value <> "" Then LSCritGrps.Desc4.Enabled = False
    If LSCritGrps.Desc5.Value <> "" Then LSCritGrps.Desc5.Enabled = False
    If LSCritGrps.Desc6.Value <> "" Then LSCritGrps.Desc6.Enabled = False
    If LSCritGrps.Desc7.Value <> "" Then LSCritGrps.Desc7.Enabled = False
    If LSCritGrps.Desc8.Value <> "" Then LSCritGrps.Desc8.Enabled = False
    If LSCritGrps.Desc9.Value <> "" Then LSCritGrps.Desc9.Enabled = False
    If LSCritGrps.Desc10.Value <> "" Then LSCritGrps.Desc10.Enabled = False
End Sub

'pass criteria group info from dialog box controls to the array
Sub SaveGrps()
    GrpInfo(1, 1) = LSCritGrps.Typ1.Caption
    GrpInfo(1, 2) = LSCritGrps.Desc1.Value
    GrpInfo(2, 1) = LSCritGrps.Typ2.Caption
    GrpInfo(2, 2) = LSCritGrps.Desc2.Value
    GrpInfo(3, 1) = LSCritGrps.Typ3.Caption
    GrpInfo(3, 2) = LSCritGrps.Desc3.Value
    GrpInfo(4, 1) = LSCritGrps.Typ4.Caption
    GrpInfo(4, 2) = LSCritGrps.Desc4.Value
    GrpInfo(5, 1) = LSCritGrps.Typ5.Caption
    GrpInfo(5, 2) = LSCritGrps.Desc5.Value
    GrpInfo(6, 1) = LSCritGrps.Typ6.Caption
    GrpInfo(6, 2) = LSCritGrps.Desc6.Value
    GrpInfo(7, 1) = LSCritGrps.Typ7.Caption
    GrpInfo(7, 2) = LSCritGrps.Desc7.Value
    GrpInfo(8, 1) = LSCritGrps.Typ8.Caption
    GrpInfo(8, 2) = LSCritGrps.Desc8.Value
    GrpInfo(9, 1) = LSCritGrps.Typ9.Caption
    GrpInfo(9, 2) = LSCritGrps.Desc9.Value
    GrpInfo(10, 1) = LSCritGrps.Typ10.Caption
    GrpInfo(10, 2) = LSCritGrps.Desc10.Value
End Sub

'pass criteria selection info from array to dialog box controls
Sub GetCrit(grp, IE)
    LSCritSel.Crit000Rel.Value = CritDet000(grp, 1)
    Get000Val (grp)
    LSCritSel.Crit004Rel.Value = CritDet004(grp, 1)
    Get004Val (grp)
    LSCritSel.Crit005Rel.Value = CritDet005(grp, 1)
    Get005Val (grp)
    LSCritSel.Crit06ARel.Value = CritDet06A(grp, 1)
    If CritDet06A(grp, 2) = "()" Or CritDet06A(grp, 2) = "" Then LSCritSel.Crit06AVal.Value = "" Else LSCritSel.Crit06AVal.Value = CDbl(CritDet06A(grp, 2)) * -1
    LSCritSel.Crit06BRel.Value = CritDet06B(grp, 1)
    If CritDet06B(grp, 2) = "()" Or CritDet06B(grp, 2) = "" Then LSCritSel.Crit06BVal.Value = "" Else LSCritSel.Crit06BVal.Value = CDbl(CritDet06B(grp, 2)) * -1
    LSCritSel.Crit007Rel.Value = CritDet007(grp, 1)
    If CritDet007(grp, 2) = "()" Or CritDet007(grp, 2) = "" Then LSCritSel.Crit007Val.Value = CritDet007(grp, 2) Else LSCritSel.Crit007Val.Value = Mid(CritDet007(grp, 2), 3, 10)
    LSCritSel.Crit021Rel.Value = CritDet021(grp, 1)
    'If there is a max sales amount, do not put in a date
    If Main_LoanSales.GetMaxSaleAmt Then
        LSCritSel.Crit021Val.Value = ""
        LSCritSel.Crit021Rel.Clear
    Else
        If CritDet021(grp, 2) = "()" Or CritDet021(grp, 2) = "" Then LSCritSel.Crit021Val.Value = CritDet021(grp, 2) Else LSCritSel.Crit021Val.Value = Mid(CritDet021(grp, 2), 3, 10)
    End If
    
    If CritDet023(grp, 1) = "" Then
        LSCritSel.Crit023Rel.Value = ""
        LSCritSel.Crit023Val.Value = ""
    Else
        If CritDet023(grp, 2) = "(' ')" Then
            LSCritSel.Crit023Val.ListIndex = 0
        ElseIf CritDet023(grp, 2) = "('Y')" Then
            LSCritSel.Crit023Val.ListIndex = 1
        End If
        LSCritSel.Crit023Rel.Value = CritDet023(grp, 1)
    End If
    
    LSCritSel.Crit027Rel.Value = CritDet027(grp, 1)
    If CritDet027(grp, 2) = "()" Or CritDet027(grp, 2) = "" Then LSCritSel.Crit027Val.Value = "" Else LSCritSel.Crit027Val.Value = CDbl(CritDet027(grp, 2)) * -1
    
    
    LSCritSel.Crit045Rel.Value = CritDet045(grp, 1)
    LSCritSel.Crit045Val.Value = CritDet045(grp, 2)
    LSCritSel.Crit045Val.Value = Replace(LSCritSel.Crit045Val.Value, "(", "")
    LSCritSel.Crit045Val.Value = Replace(LSCritSel.Crit045Val.Value, ")", "")
    LSCritSel.Crit045Val.Value = Replace(LSCritSel.Crit045Val.Value, "'", "")
    LSCritSel.Crit045Val.Value = Replace(LSCritSel.Crit045Val.Value, " ", "")
    
    LSCritSel.Crit046Rel.Value = CritDet046(grp, 1)
    LSCritSel.Crit046Val.Value = CritDet046(grp, 2)
    LSCritSel.Crit046Val.Value = Replace(LSCritSel.Crit046Val.Value, "(", "")
    LSCritSel.Crit046Val.Value = Replace(LSCritSel.Crit046Val.Value, ")", "")
    LSCritSel.Crit046Val.Value = Replace(LSCritSel.Crit046Val.Value, "'", "")
    LSCritSel.Crit046Val.Value = Replace(LSCritSel.Crit046Val.Value, " ", "")
    
    
    LSCritSel.Crit047Val.Selected(0) = False
    LSCritSel.Crit047Val.Selected(1) = False
    LSCritSel.Crit047Val.Selected(2) = False
    If CritDet047(grp, 1) = "" Then
        LSCritSel.Crit047Rel.Value = ""
        LSCritSel.Crit047Val.Value = ""
    Else
        If InStr(1, CritDet047(grp, 2), "A") <> 0 Then
            LSCritSel.Crit047Val.Selected(0) = True
        End If
        If InStr(1, CritDet047(grp, 2), "M") <> 0 Then
            LSCritSel.Crit047Val.Selected(1) = True
        End If
        If InStr(1, CritDet047(grp, 2), "S") <> 0 Then
            LSCritSel.Crit047Val.Selected(2) = True
        End If
        LSCritSel.Crit047Rel.Value = CritDet047(grp, 1)
    End If
    
    LSCritSel.Crit052Rel.Value = CritDet052(grp, 1)
    If CritDet052(grp, 2) = "()" Or CritDet052(grp, 2) = "" Then LSCritSel.Crit052Val.Value = CritDet052(grp, 2) Else LSCritSel.Crit052Val.Value = Mid(CritDet052(grp, 2), 3, 10)
    
    If CritDet100(grp, 1) = "" Then
        LSCritSel.Crit100Rel.Value = ""
        LSCritSel.Crit100Val.Text = ""
    Else
        LSCritSel.Crit100Val.Text = Mid(CritDet100(grp, 2), 3, 10)
        LSCritSel.Crit100Rel.Value = CritDet100(grp, 1)
    End If
    
    LSCritSel.Crit103Rel.Value = CritDet103(grp, 1)
    If CritDet103(grp, 2) = "()" Or CritDet103(grp, 2) = "" Then LSCritSel.Crit103Val.Value = CritDet103(grp, 2) Else LSCritSel.Crit103Val.Value = Mid(CritDet103(grp, 2), 3, 10)
    
    LSCritSel.Crit104Rel.Value = CritDet104(grp, 1)
    If CritDet104(grp, 2) = "()" Or CritDet104(grp, 2) = "" Then LSCritSel.Crit104Val.Value = CritDet104(grp, 2) Else LSCritSel.Crit104Val.Value = Mid(CritDet104(grp, 2), 3, 10)
    
    Dim StrTemp108 As String
    Dim ArrTemp108() As String
    Dim i As Integer
    Dim II As Integer
    StrTemp108 = CritDet108(grp, 2)
    StrTemp108 = Replace(StrTemp108, "(", "")
    StrTemp108 = Replace(StrTemp108, ")", "")
    ArrTemp108 = Split(StrTemp108, ",")
    'unselect all previous selected options
    For i = 0 To LSCritSel.Crit108Val.ListCount - 1
        LSCritSel.Crit108Val.Selected(i) = False
    Next
    For i = 0 To UBound(ArrTemp108)
        'number in string - 1 should match up with the index in the array
        LSCritSel.Crit108Val.Selected(CInt(ArrTemp108(i)) - 1) = True
    Next
    LSCritSel.Crit108Rel.Value = CritDet108(grp, 1)
    
    LSCritSel.Crit109Rel.Value = CritDet109(grp, 1)
    If CritDet109(grp, 2) = "()" Or CritDet109(grp, 2) = "" Then LSCritSel.Crit109Val.Value = CritDet109(grp, 2) Else LSCritSel.Crit109Val.Value = Mid(CritDet109(grp, 2), 3, 10)
    
    LSCritSel.Crit110Rel.Value = CritDet110(grp, 1)
    If CritDet110(grp, 2) = "()" Or CritDet110(grp, 2) = "" Then LSCritSel.Crit110Val.Value = CritDet110(grp, 2) Else LSCritSel.Crit110Val.Value = Mid(CritDet110(grp, 2), 3, 10)
    
     LSCritSel.Crit111Rel.Value = CritDet111(grp, 1)
    If CritDet111(grp, 2) = "()" Or CritDet111(grp, 2) = "" Then LSCritSel.Crit111Val.Value = CritDet111(grp, 2) Else LSCritSel.Crit111Val.Value = Mid(CritDet111(grp, 2), 3, 10)
    
     LSCritSel.Crit112Rel.Value = CritDet112(grp, 1)
    If CritDet112(grp, 2) = "()" Or CritDet112(grp, 2) = "" Then LSCritSel.Crit112Val.Value = CritDet112(grp, 2) Else LSCritSel.Crit112Val.Value = Mid(CritDet112(grp, 2), 3, 10)
End Sub

'select loan status values based on statuses listed in array string value
Sub Get000Val(grp)
    If InStrRev(CritDet000(grp, 2), "06") <> 0 Then LSCritSel.Crit000Val.Selected(0) = True Else LSCritSel.Crit000Val.Selected(0) = False
    If InStrRev(CritDet000(grp, 2), "07") <> 0 Then LSCritSel.Crit000Val.Selected(1) = True Else LSCritSel.Crit000Val.Selected(1) = False
    If InStrRev(CritDet000(grp, 2), "08") <> 0 Then LSCritSel.Crit000Val.Selected(2) = True Else LSCritSel.Crit000Val.Selected(2) = False
    If InStrRev(CritDet000(grp, 2), "09") <> 0 Then LSCritSel.Crit000Val.Selected(3) = True Else LSCritSel.Crit000Val.Selected(3) = False
    If InStrRev(CritDet000(grp, 2), "10") <> 0 Then LSCritSel.Crit000Val.Selected(4) = True Else LSCritSel.Crit000Val.Selected(4) = False
    If InStrRev(CritDet000(grp, 2), "11") <> 0 Then LSCritSel.Crit000Val.Selected(5) = True Else LSCritSel.Crit000Val.Selected(5) = False
    If InStrRev(CritDet000(grp, 2), "12") <> 0 Then LSCritSel.Crit000Val.Selected(6) = True Else LSCritSel.Crit000Val.Selected(6) = False
    If InStrRev(CritDet000(grp, 2), "16") <> 0 Then LSCritSel.Crit000Val.Selected(7) = True Else LSCritSel.Crit000Val.Selected(7) = False
    If InStrRev(CritDet000(grp, 2), "17") <> 0 Then LSCritSel.Crit000Val.Selected(8) = True Else LSCritSel.Crit000Val.Selected(8) = False
    If InStrRev(CritDet000(grp, 2), "18") <> 0 Then LSCritSel.Crit000Val.Selected(9) = True Else LSCritSel.Crit000Val.Selected(9) = False
    If InStrRev(CritDet000(grp, 2), "19") <> 0 Then LSCritSel.Crit000Val.Selected(10) = True Else LSCritSel.Crit000Val.Selected(10) = False
    If InStrRev(CritDet000(grp, 2), "20") <> 0 Then LSCritSel.Crit000Val.Selected(11) = True Else LSCritSel.Crit000Val.Selected(11) = False
    If InStrRev(CritDet000(grp, 2), "21") <> 0 Then LSCritSel.Crit000Val.Selected(12) = True Else LSCritSel.Crit000Val.Selected(12) = False
    If InStrRev(CritDet000(grp, 2), "23") <> 0 Then LSCritSel.Crit000Val.Selected(13) = True Else LSCritSel.Crit000Val.Selected(13) = False
    If InStrRev(CritDet000(grp, 2), "88") <> 0 Then LSCritSel.Crit000Val.Selected(14) = True Else LSCritSel.Crit000Val.Selected(14) = False
    If InStrRev(CritDet000(grp, 2), "98") <> 0 Then LSCritSel.Crit000Val.Selected(15) = True Else LSCritSel.Crit000Val.Selected(15) = False
End Sub

'select guarantor code values based on statuses listed in array string value
Sub Get004Val(grp)
    If InStrRev(CritDet004(grp, 2), "000749") <> 0 Then LSCritSel.Crit004Val.Selected(0) = True Else LSCritSel.Crit004Val.Selected(0) = False
    If InStrRev(CritDet004(grp, 2), "000800") <> 0 Then LSCritSel.Crit004Val.Selected(1) = True Else LSCritSel.Crit004Val.Selected(1) = False
End Sub

'select loan type values based on statuses listed in array string value
Sub Get005Val(grp)
    If InStrRev(CritDet005(grp, 2), "STFFRD") <> 0 Then LSCritSel.Crit005Val.Selected(0) = True Else LSCritSel.Crit005Val.Selected(0) = False
    If InStrRev(CritDet005(grp, 2), "UNSTFD") <> 0 Then LSCritSel.Crit005Val.Selected(1) = True Else LSCritSel.Crit005Val.Selected(1) = False
    If InStrRev(CritDet005(grp, 2), "PLUS") <> 0 Then LSCritSel.Crit005Val.Selected(2) = True Else LSCritSel.Crit005Val.Selected(2) = False
    If InStrRev(CritDet005(grp, 2), "PLUSGB") <> 0 Then LSCritSel.Crit005Val.Selected(3) = True Else LSCritSel.Crit005Val.Selected(3) = False
End Sub

'pass criteria selection info from dialog box controls to the array
Sub SaveCrit(grp)

    Dim i As Integer

'loan statuses
    'pass values
gsc: CritDet000(grp, 1) = LSCritSel.Crit000Rel.Value
    CritDet000(grp, 2) = Bld000Val()
    'warn user of incomplete info
    If CritDet000(grp, 1) <> "" And CritDet000(grp, 2) = "()" Or _
       CritDet000(grp, 1) = "" And CritDet000(grp, 2) <> "()" Then
        MsgBox "Incomplete information was entered for criteria 000.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    
'guarantor codes
    'pass values
    CritDet004(grp, 1) = LSCritSel.Crit004Rel.Value
    CritDet004(grp, 2) = Bld004Val()
    'warn user of incomplete info
    If CritDet004(grp, 1) <> "" And CritDet004(grp, 2) = "()" Or _
       CritDet004(grp, 1) = "" And CritDet004(grp, 2) <> "()" Then
        MsgBox "Incomplete information was entered for criteria 004.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    
'loan types
    'pass values
    CritDet005(grp, 1) = LSCritSel.Crit005Rel.Value
    CritDet005(grp, 2) = Bld005Val()
    'warn user of incomplete info
    If CritDet005(grp, 1) <> "" And CritDet005(grp, 2) = "()" Or _
       CritDet005(grp, 1) = "" And CritDet005(grp, 2) <> "()" Then
        MsgBox "Incomplete information was entered for criteria 005.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    
'current principal balance
    'pass values
    CritDet06A(grp, 1) = LSCritSel.Crit06ARel.Value
    CritDet06A(grp, 2) = "(" & LSCritSel.Crit06AVal.Value & ")"
    'warn user of incomplete info
    If (CritDet06A(grp, 1) <> "" And CritDet06A(grp, 2) = "()") Or _
       (CritDet06A(grp, 1) = "" And CritDet06A(grp, 2) <> "()") Or _
       (CritDet06A(grp, 2) <> "()" And IsNumeric(CritDet06A(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 06A or the value entered was not numeric.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    CritDet06B(grp, 1) = LSCritSel.Crit06BRel.Value
    CritDet06B(grp, 2) = "(" & LSCritSel.Crit06BVal.Value & ")"
    'warn user of incomplete info
    If (CritDet06B(grp, 1) <> "" And CritDet06B(grp, 2) = "()") Or _
       (CritDet06B(grp, 1) = "" And CritDet06B(grp, 2) <> "()") Or _
       (CritDet06B(grp, 2) <> "()" And IsNumeric(CritDet06B(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 06B or the value entered was not numeric.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    
'First Disbursement Date
    'pass values
    CritDet007(grp, 1) = LSCritSel.Crit007Rel.Value
    CritDet007(grp, 2) = DateFormatter(LSCritSel.Crit007Val.Value)
    'warn user of incomplete info
    If (CritDet007(grp, 1) <> "" And CritDet007(grp, 2) = "") Or _
       (CritDet007(grp, 1) = "" And CritDet007(grp, 2) <> "") Or _
       (CritDet007(grp, 2) <> "" And IsDate(CritDet007(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 007 or the value entered was not a valid date.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    'format the date for SQL IN statement
    If CritDet007(grp, 2) <> "" Then CritDet007(grp, 2) = "('" & DateFormatter(LSCritSel.Crit007Val.Value) & "')"
    'pass values
    CritDet111(grp, 1) = LSCritSel.Crit111Rel.Value
    CritDet111(grp, 2) = DateFormatter(LSCritSel.Crit111Val.Value)
    'warn user of incomplete info
    If (CritDet111(grp, 1) <> "" And CritDet111(grp, 2) = "") Or _
       (CritDet111(grp, 1) = "" And CritDet111(grp, 2) <> "") Or _
       (CritDet111(grp, 2) <> "" And IsDate(CritDet111(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 111 or the value entered was not a valid date.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    'format the date for SQL IN statement
    If CritDet111(grp, 2) <> "" Then CritDet111(grp, 2) = "('" & DateFormatter(LSCritSel.Crit111Val.Value) & "')"
    
'separation date
    'pass values
    CritDet021(grp, 1) = LSCritSel.Crit021Rel.Value
    CritDet021(grp, 2) = DateFormatter(LSCritSel.Crit021Val.Value)
    'warn user of incomplete info
    If (CritDet021(grp, 1) <> "" And CritDet021(grp, 2) = "") Or _
       (CritDet021(grp, 1) = "" And CritDet021(grp, 2) <> "") Or _
       (CritDet021(grp, 2) <> "" And IsDate(CritDet021(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 021 or the value entered was not a valid date.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    'format the date for SQL IN statement
    If CritDet021(grp, 2) <> "" Then CritDet021(grp, 2) = "('" & DateFormatter(LSCritSel.Crit021Val.Value) & "')"

'fully originated indicator
    'pass values
    CritDet023(grp, 1) = LSCritSel.Crit023Rel.Value
    If LSCritSel.Crit023Val.ListIndex = 0 Then
        CritDet023(grp, 2) = "('N')"
    ElseIf LSCritSel.Crit023Val.ListIndex = 1 Then
        CritDet023(grp, 2) = "('Y')"
    Else
        CritDet023(grp, 2) = "('')"
    End If
    'warn user of incomplete info
    If (CritDet023(grp, 1) <> "" And CritDet023(grp, 2) = "('')") Or _
       (CritDet023(grp, 1) = "" And CritDet023(grp, 2) <> "('')") Then
        MsgBox "Incomplete information was entered for criteria 023.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If

'days since final disbursement
    'pass values
    CritDet027(grp, 1) = LSCritSel.Crit027Rel.Value
    CritDet027(grp, 2) = "(" & LSCritSel.Crit027Val.Value & ")"
    'warn user of incomplete info
    If (CritDet027(grp, 1) <> "" And CritDet027(grp, 2) = "()") Or _
       (CritDet027(grp, 1) = "" And CritDet027(grp, 2) <> "()") Or _
       (CritDet027(grp, 2) <> "()" And IsNumeric(CritDet027(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 027 or the value entered was not numeric.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If

'Original School Code
    'pass values
    CritDet045(grp, 1) = LSCritSel.Crit045Rel.Value
    'warn user of incomplete info
    If (CritDet045(grp, 1) <> "" And LSCritSel.Crit045Val.Value = "") Or _
       (CritDet045(grp, 1) = "" And LSCritSel.Crit045Val.Value <> "") Then
        MsgBox "Incomplete information was entered for criteria 045.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    If LSCritSel.Crit045Val.Value <> "" Then
        CritDet045(grp, 2) = "('" & Replace(LSCritSel.Crit045Val.Value, ",", "','") & "')"
    Else
        CritDet045(grp, 2) = "('')"
    End If
    
'Current School Code
    'pass values
    CritDet046(grp, 1) = LSCritSel.Crit046Rel.Value
    'warn user of incomplete info
    If (CritDet046(grp, 1) <> "" And LSCritSel.Crit046Val.Value = "") Or _
       (CritDet046(grp, 1) = "" And LSCritSel.Crit046Val.Value <> "") Then
        MsgBox "Incomplete information was entered for criteria 046.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    If LSCritSel.Crit046Val.Value <> "" Then
        CritDet046(grp, 2) = "('" & Replace(LSCritSel.Crit046Val.Value, ",", "','") & "')"
    Else
        CritDet046(grp, 2) = "('')"
    End If

'Serialzed Loan
    Dim Temp047 As String
    CritDet047(grp, 1) = LSCritSel.Crit047Rel.Value
    For i = 0 To LSCritSel.Crit047Val.ListCount - 1
        If LSCritSel.Crit047Val.Selected(i) Then
            If Temp047 = "" Then
                Temp047 = "'" & Mid(LSCritSel.Crit047Val.Column(0, i), 1, 1) & "'"
            Else
                Temp047 = Temp047 & ",'" & Mid(LSCritSel.Crit047Val.Column(0, i), 1, 1) & "'"
            End If
        End If
    Next
    CritDet047(grp, 2) = "(" & Temp047 & ")"
    'warn user of incomplete info
    If (CritDet047(grp, 1) <> "" And CritDet047(grp, 2) = "()") Or _
       (CritDet047(grp, 1) = "" And CritDet047(grp, 2) <> "()") Then
        MsgBox "Incomplete information was entered for criteria 047.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If

'Guaranteed Date (052)
    'pass values
    CritDet052(grp, 1) = LSCritSel.Crit052Rel.Value
    CritDet052(grp, 2) = DateFormatter(LSCritSel.Crit052Val.Value)
    'warn user of incomplete info
    If (CritDet052(grp, 1) <> "" And CritDet052(grp, 2) = "") Or _
       (CritDet052(grp, 1) = "" And CritDet052(grp, 2) <> "") Or _
       (CritDet052(grp, 2) <> "" And IsDate(CritDet052(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 052 or the value entered was not a valid date.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    'format the date for SQL IN statement
    If CritDet052(grp, 2) <> "" Then CritDet052(grp, 2) = "('" & DateFormatter(LSCritSel.Crit052Val.Value) & "')"

'Fully Disbursed as of Last Disbursement Date
    'pass values
    If LSCritSel.Crit100Rel.Value <> "" Then
        CritDet100(grp, 1) = LSCritSel.Crit100Rel.Value
        CritDet100(grp, 2) = LSCritSel.Crit100Val.Text
    Else
        CritDet100(grp, 1) = ""
        CritDet100(grp, 2) = ""
    End If
    CritDet100(grp, 2) = DateFormatter(LSCritSel.Crit100Val.Text)
    'warn user of incomplete info
    If (CritDet100(grp, 1) <> "" And CritDet100(grp, 2) = "") Or _
       (CritDet100(grp, 1) = "" And CritDet100(grp, 2) <> "") Or _
       (CritDet100(grp, 2) <> "" And IsDate(CritDet100(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 100 or the value entered was not a valid date.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    'format the date for SQL IN statement
    If CritDet100(grp, 2) <> "" Then CritDet100(grp, 2) = "('" & DateFormatter(LSCritSel.Crit100Val.Text) & "')"
    
'Loan Term Begin Date (103)
    'pass values
    CritDet103(grp, 1) = LSCritSel.Crit103Rel.Value
    CritDet103(grp, 2) = DateFormatter(LSCritSel.Crit103Val.Value)
    'warn user of incomplete info
    If (CritDet103(grp, 1) <> "" And CritDet103(grp, 2) = "") Or _
       (CritDet103(grp, 1) = "" And CritDet103(grp, 2) <> "") Or _
       (CritDet103(grp, 2) <> "" And IsDate(CritDet103(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 103 or the value entered was not a valid date.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    'format the date for SQL IN statement
    If CritDet103(grp, 2) <> "" Then CritDet103(grp, 2) = "('" & DateFormatter(LSCritSel.Crit103Val.Value) & "')"
    
'Loan Term End Date (104)
    'pass values
    CritDet104(grp, 1) = LSCritSel.Crit104Rel.Value
    CritDet104(grp, 2) = DateFormatter(LSCritSel.Crit104Val.Value)
    'warn user of incomplete info
    If (CritDet104(grp, 1) <> "" And CritDet104(grp, 2) = "") Or _
       (CritDet104(grp, 1) = "" And CritDet104(grp, 2) <> "") Or _
       (CritDet104(grp, 2) <> "" And IsDate(CritDet104(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 104 or the value entered was not a valid date.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    'format the date for SQL IN statement
    If CritDet104(grp, 2) <> "" Then CritDet104(grp, 2) = "('" & DateFormatter(LSCritSel.Crit104Val.Value) & "')"
    
'Grade Level (108)
    Dim Temp108 As String
    CritDet108(grp, 1) = LSCritSel.Crit108Rel.Value
    For i = 0 To LSCritSel.Crit108Val.ListCount - 1
        If LSCritSel.Crit108Val.Selected(i) Then
            If Temp108 = "" Then
                Temp108 = Trim(Split(LSCritSel.Crit108Val.Column(0, i), "-")(0))
            Else
                Temp108 = Temp108 & "," & Trim(Split(LSCritSel.Crit108Val.Column(0, i), "-")(0))
            End If
        End If
    Next
    CritDet108(grp, 2) = "(" & Temp108 & ")"
    'warn user of incomplete info
    If (CritDet108(grp, 1) <> "" And CritDet108(grp, 2) = "()") Or _
       (CritDet108(grp, 1) = "" And CritDet108(grp, 2) <> "()") Then
        MsgBox "Incomplete information was entered for criteria 108.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    
'Loan Term End Date (109)
    'pass values
    CritDet109(grp, 1) = LSCritSel.Crit109Rel.Value
    CritDet109(grp, 2) = DateFormatter(LSCritSel.Crit109Val.Value)
    'warn user of incomplete info
    If (CritDet109(grp, 1) <> "" And CritDet109(grp, 2) = "") Or _
       (CritDet109(grp, 1) = "" And CritDet109(grp, 2) <> "") Or _
       (CritDet109(grp, 2) <> "" And IsDate(CritDet109(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 109 or the value entered was not a valid date.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    'format the date for SQL IN statement
    If CritDet109(grp, 2) <> "" Then CritDet109(grp, 2) = "('" & DateFormatter(LSCritSel.Crit109Val.Value) & "')"
    
'Loan Term End Date (110)
    'pass values
    CritDet110(grp, 1) = LSCritSel.Crit110Rel.Value
    CritDet110(grp, 2) = DateFormatter(LSCritSel.Crit110Val.Value)
    'warn user of incomplete info
    If (CritDet110(grp, 1) <> "" And CritDet110(grp, 2) = "") Or _
       (CritDet110(grp, 1) = "" And CritDet110(grp, 2) <> "") Or _
       (CritDet110(grp, 2) <> "" And IsDate(CritDet110(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 110 or the value entered was not a valid date.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    'format the date for SQL IN statement
    If CritDet110(grp, 2) <> "" Then CritDet110(grp, 2) = "('" & DateFormatter(LSCritSel.Crit110Val.Value) & "')"
    
'Guaranteed Date (112)
    'pass values
    CritDet112(grp, 1) = LSCritSel.Crit112Rel.Value
    CritDet112(grp, 2) = DateFormatter(LSCritSel.Crit112Val.Value)
    'warn user of incomplete info
    If (CritDet112(grp, 1) <> "" And CritDet112(grp, 2) = "") Or _
       (CritDet112(grp, 1) = "" And CritDet112(grp, 2) <> "") Or _
       (CritDet112(grp, 2) <> "" And IsDate(CritDet112(grp, 2)) = False) Then
        MsgBox "Incomplete information was entered for criteria 112 or the value entered was not a valid date.  Please correct the error to continue.", , "Selection Error"
        LSCritSel.Show
        GoTo gsc:
    End If
    'format the date for SQL IN statement
    If CritDet112(grp, 2) <> "" Then CritDet112(grp, 2) = "('" & DateFormatter(LSCritSel.Crit112Val.Value) & "')"
End Sub

'add loan status values to string value formatted for SQL
Function Bld000Val()
    Bld000Val = "("
    If LSCritSel.Crit000Val.Selected(0) = True Then Bld000Val = Bld000Val & "'06',"
    If LSCritSel.Crit000Val.Selected(1) = True Then Bld000Val = Bld000Val & "'07',"
    If LSCritSel.Crit000Val.Selected(2) = True Then Bld000Val = Bld000Val & "'08',"
    If LSCritSel.Crit000Val.Selected(3) = True Then Bld000Val = Bld000Val & "'09',"
    If LSCritSel.Crit000Val.Selected(4) = True Then Bld000Val = Bld000Val & "'10',"
    If LSCritSel.Crit000Val.Selected(5) = True Then Bld000Val = Bld000Val & "'11',"
    If LSCritSel.Crit000Val.Selected(6) = True Then Bld000Val = Bld000Val & "'12',"
    If LSCritSel.Crit000Val.Selected(7) = True Then Bld000Val = Bld000Val & "'16',"
    If LSCritSel.Crit000Val.Selected(8) = True Then Bld000Val = Bld000Val & "'17',"
    If LSCritSel.Crit000Val.Selected(9) = True Then Bld000Val = Bld000Val & "'18',"
    If LSCritSel.Crit000Val.Selected(10) = True Then Bld000Val = Bld000Val & "'19',"
    If LSCritSel.Crit000Val.Selected(11) = True Then Bld000Val = Bld000Val & "'20',"
    If LSCritSel.Crit000Val.Selected(12) = True Then Bld000Val = Bld000Val & "'21',"
    If LSCritSel.Crit000Val.Selected(13) = True Then Bld000Val = Bld000Val & "'23',"
    If LSCritSel.Crit000Val.Selected(14) = True Then Bld000Val = Bld000Val & "'88',"
    If LSCritSel.Crit000Val.Selected(15) = True Then Bld000Val = Bld000Val & "'98',"
    If Right(Bld000Val, 1) = "," Then Bld000Val = Mid(Bld000Val, 1, Len(Bld000Val) - 1)
    Bld000Val = Bld000Val & ")"
End Function

'add guarantor code values to string value formatted for SQL
Function Bld004Val()
    Bld004Val = "("
    If LSCritSel.Crit004Val.Selected(0) = True Then Bld004Val = Bld004Val & "'000749',"
    If LSCritSel.Crit004Val.Selected(1) = True Then Bld004Val = Bld004Val & "'000800'"
    If Right(Bld004Val, 1) = "," Then Bld004Val = Mid(Bld004Val, 1, Len(Bld004Val) - 1)
    Bld004Val = Bld004Val & ")"
End Function

'add loan type values to string value formatted for SQL
Function Bld005Val()
    Bld005Val = "("
    If LSCritSel.Crit005Val.Selected(0) = True Then Bld005Val = Bld005Val & "'STFFRD',"
    If LSCritSel.Crit005Val.Selected(1) = True Then Bld005Val = Bld005Val & "'UNSTFD',"
    If LSCritSel.Crit005Val.Selected(2) = True Then Bld005Val = Bld005Val & "'PLUS',"
    If LSCritSel.Crit005Val.Selected(2) = True Then Bld005Val = Bld005Val & "'PLUSGB'"
    If Right(Bld005Val, 1) = "," Then Bld005Val = Mid(Bld005Val, 1, Len(Bld005Val) - 1)
    Bld005Val = Bld005Val & ")"
End Function

'format strings in MM/DD/YYYY format
Function DateFormatter(dt) As String
    With Session
        'return an invalid date if the length is not 6, 8, or 10 (lengths of valid date strings)
        If Len(dt) <> 6 And Len(dt) <> 8 And Len(dt) <> 10 Then
            DateFormatter = ""
        'format 6 digit date with no slashes
        ElseIf Len(dt) = 6 And IsDate(Format(dt, "##/##/##")) = True Then
            DateFormatter = Format(DateValue(Format(dt, "##/##/##")), "MM/DD/YYYY")
        'format 6 digit date with slashes
        ElseIf Len(dt) = 8 And IsDate(dt) = True Then
            DateFormatter = Format(DateValue(dt), "MM/DD/YYYY")
        'format 8 digit date with no slashes
        ElseIf Len(dt) = 8 Then
            DateFormatter = Format(dt, "0#/##/####")
        'return string given if formatting not needed or undetermined
        Else
            DateFormatter = dt
        End If
    End With
End Function

'save info from arrays to text files
Function Save2File(yn)
    Dim isInclusion As Boolean
    Dim a As Integer
    Dim critGrpLn As String
    'sort criteria group info with non blank values to first rows of the arrays
    For i = 1 To 9
        If GrpInfo(i, 1) = "" Then
            j = i
            Do Until GrpInfo(j, 1) <> ""
                j = j + 1
                If j = 11 Then Exit Do
            Loop
            If j = 11 Then Exit For
            GrpInfo(i, 1) = GrpInfo(j, 1)
            GrpInfo(i, 2) = GrpInfo(j, 2)
            CritDet000(i, 1) = CritDet000(j, 1)
            CritDet000(i, 2) = CritDet000(j, 2)
            CritDet004(i, 1) = CritDet004(j, 1)
            CritDet004(i, 2) = CritDet004(j, 2)
            CritDet005(i, 1) = CritDet005(j, 1)
            CritDet005(i, 2) = CritDet005(j, 2)
            CritDet06A(i, 1) = CritDet06A(j, 1)
            CritDet06A(i, 2) = CritDet06A(j, 2)
            CritDet06B(i, 1) = CritDet06B(j, 1)
            CritDet06B(i, 2) = CritDet06B(j, 2)
            CritDet007(i, 1) = CritDet007(j, 1)
            CritDet007(i, 2) = CritDet007(j, 2)
            CritDet021(i, 1) = CritDet021(j, 1)
            CritDet021(i, 2) = CritDet021(j, 2)
            CritDet023(i, 1) = CritDet023(j, 1)
            CritDet023(i, 2) = CritDet023(j, 2)
            CritDet027(i, 1) = CritDet027(j, 1)
            CritDet027(i, 2) = CritDet027(j, 2)
            CritDet045(i, 1) = CritDet045(j, 1)
            CritDet045(i, 2) = CritDet045(j, 2)
            CritDet046(i, 1) = CritDet046(j, 1)
            CritDet046(i, 2) = CritDet046(j, 2)
            CritDet047(i, 1) = CritDet047(j, 1)
            CritDet047(i, 2) = CritDet047(j, 2)
            CritDet052(i, 1) = CritDet052(j, 1)
            CritDet052(i, 2) = CritDet052(j, 2)
            CritDet100(i, 1) = CritDet100(j, 1)
            CritDet100(i, 2) = CritDet100(j, 2)
            CritDet103(i, 1) = CritDet103(j, 1)
            CritDet103(i, 2) = CritDet103(j, 2)
            CritDet104(i, 1) = CritDet104(j, 1)
            CritDet104(i, 2) = CritDet104(j, 2)
            CritDet108(i, 1) = CritDet108(j, 1)
            CritDet108(i, 2) = CritDet108(j, 2)
            CritDet109(i, 1) = CritDet109(j, 1)
            CritDet109(i, 2) = CritDet109(j, 2)
            CritDet110(i, 1) = CritDet110(j, 1)
            CritDet110(i, 2) = CritDet110(j, 2)
            CritDet111(i, 1) = CritDet111(j, 1)
            CritDet111(i, 2) = CritDet111(j, 2)
            CritDet112(i, 1) = CritDet112(j, 1)
            CritDet112(i, 2) = CritDet112(j, 2)
            GrpInfo(j, 1) = ""
            GrpInfo(j, 2) = ""
            CritDet000(j, 1) = ""
            CritDet000(j, 2) = ""
            CritDet004(j, 1) = ""
            CritDet004(j, 2) = ""
            CritDet005(j, 1) = ""
            CritDet005(j, 2) = ""
            CritDet06A(j, 1) = ""
            CritDet06A(j, 2) = ""
            CritDet06B(j, 1) = ""
            CritDet06B(j, 2) = ""
            CritDet007(j, 1) = ""
            CritDet007(j, 2) = ""
            CritDet021(j, 1) = ""
            CritDet021(j, 2) = ""
            CritDet023(j, 1) = ""
            CritDet023(j, 2) = ""
            CritDet027(j, 1) = ""
            CritDet027(j, 2) = ""
            CritDet045(j, 1) = ""
            CritDet045(j, 2) = ""
            CritDet046(j, 1) = ""
            CritDet046(j, 2) = ""
            CritDet047(j, 1) = ""
            CritDet047(j, 2) = ""
            CritDet052(j, 1) = ""
            CritDet052(j, 2) = ""
            CritDet100(j, 1) = ""
            CritDet100(j, 2) = ""
            CritDet103(j, 1) = ""
            CritDet103(j, 2) = ""
            CritDet104(j, 1) = ""
            CritDet104(j, 2) = ""
            CritDet108(j, 1) = ""
            CritDet108(j, 2) = ""
            CritDet109(j, 1) = ""
            CritDet109(j, 2) = ""
            CritDet110(j, 1) = ""
            CritDet110(j, 2) = ""
            CritDet111(j, 1) = ""
            CritDet111(j, 2) = ""
            CritDet112(j, 1) = ""
            CritDet112(j, 2) = ""
            
        End If
    Next i
    'warn the user and return to the dialog box if no criteria were selected for a group
    X = 1
    Do Until GrpInfo(X, 1) = ""
        If CritDet000(X, 1) = "" And CritDet004(X, 1) = "" And CritDet005(X, 1) = "" And _
           CritDet06A(X, 1) = "" And CritDet021(X, 1) = "" And CritDet023(X, 1) = "" And _
           CritDet027(X, 1) = "" And CritDet100(X, 1) = "" And CritDet06B(X, 1) = "" And _
           CritDet007(X, 1) = "" And CritDet111(X, 1) = "" And _
           CritDet045(X, 1) = "" And CritDet046(X, 1) = "" And CritDet047(X, 1) = "" And _
           CritDet052(X, 1) = "" And CritDet112(X, 1) = "" And _
           CritDet103(X, 1) = "" And CritDet109(X, 1) = "" And _
           CritDet104(X, 1) = "" And CritDet110(X, 1) = "" And CritDet108(X, 1) = "" Then
            MsgBox "No criteria were selected for group " & Format(X, "0#") & ".  Click OK to remove the group or select criteria for the group.", , "No Criteria Selected"
            Exit Function
        End If
        X = X + 1
    Loop
    'delete the previous existing working files from Cyprus
    If Dir(ProcDir & "Loan_Sales\Working_Files\xutlwo2_2.txt") <> "" Then Kill ProcDir & "Loan_Sales\Working_Files\xutlwo2_2.txt"
    If Dir(ProcDir & "Loan_Sales\Working_Files\xutlwo2_3.txt") <> "" Then Kill ProcDir & "Loan_Sales\Working_Files\xutlwo2_3.txt"
    'rename the existing production files from Cyprus so new files can be created
    Name ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt" As ProcDir & "Loan_Sales\Working_Files\xutlwo2_2.txt"
    Name ProcDir & "Loan_Sales\Working_Files\utlwo2_3.txt" As ProcDir & "Loan_Sales\Working_Files\xutlwo2_3.txt"
    'save criteria group info in the array to the new Cyprus text file for loan sales selected by the user
    X = X - 1
    Open ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt" For Output As #1
    For i = 0 To nos - 1
        For j = 1 To X
            Write #1, SaleIDsSel(i), Format(j, "0#"), GrpInfo(j, 1), GrpInfo(j, 2)
        Next j
    Next i
    Close #1
    'save criteria group info in the existing Cyprus text file to the new Cyprus text file for valid loan sales from Compass
    Open ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt" For Append As #1
    Open ProcDir & "Loan_Sales\Working_Files\xutlwo2_2.txt" For Input As #2
    Do Until EOF(2)
        Input #2, SaleID, GrpNo, GrpTyp, GrpDesc
        For i = 0 To nos - 1
            If SaleID = SaleIDsSel(i) Then GoTo 2:
        Next i
        For i = 0 To nov - 1
            If SaleID = SaleIDsVal(0, i) Then
                Write #1, SaleID, GrpNo, GrpTyp, GrpDesc
                Exit For
            End If
        Next i
2:  Loop
    Close #1
    Close #2
    'save criteria selection info in the array to the new Cyprus text file for loan sales selected by the user
    Open ProcDir & "Loan_Sales\Working_Files\utlwo2_3.txt" For Output As #1
    For i = 0 To nos - 1
        For j = 1 To X
            If CritDet000(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "000", CritDet000(j, 1), CritDet000(j, 2)
            If CritDet004(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "004", CritDet004(j, 1), CritDet004(j, 2)
            If CritDet005(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "005", CritDet005(j, 1), CritDet005(j, 2)
            If CritDet06A(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "06A", CritDet06A(j, 1), CritDet06A(j, 2)
            If CritDet06B(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "06B", CritDet06B(j, 1), CritDet06B(j, 2)
            If CritDet007(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "007", CritDet007(j, 1), CritDet007(j, 2)
            If CritDet021(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "021", CritDet021(j, 1), CritDet021(j, 2)
            If CritDet023(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "023", CritDet023(j, 1), CritDet023(j, 2)
            If CritDet027(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "027", CritDet027(j, 1), CritDet027(j, 2)
            If CritDet045(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "045", CritDet045(j, 1), CritDet045(j, 2)
            If CritDet046(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "046", CritDet046(j, 1), CritDet046(j, 2)
            If CritDet047(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "047", CritDet047(j, 1), CritDet047(j, 2)
            If CritDet052(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "052", CritDet052(j, 1), CritDet052(j, 2)
            If CritDet100(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "100", CritDet100(j, 1), CritDet100(j, 2)
            If CritDet103(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "103", CritDet103(j, 1), CritDet103(j, 2)
            If CritDet104(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "104", CritDet104(j, 1), CritDet104(j, 2)
            If CritDet108(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "108", CritDet108(j, 1), CritDet108(j, 2)
            If CritDet109(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "109", CritDet109(j, 1), CritDet109(j, 2)
            If CritDet110(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "110", CritDet110(j, 1), CritDet110(j, 2)
            If CritDet111(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "111", CritDet111(j, 1), CritDet111(j, 2)
            If CritDet112(j, 1) <> "" Then Write #1, SaleIDsSel(i), Format(j, "0#"), "112", CritDet112(j, 1), CritDet112(j, 2)
            'add ECASLA criteria if the group is marked as inclusion "I"
            isInclusion = False
            a = 0
            Open ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt" For Input As #2
            Line Input #2, critGrpLn
            critGrpLn = Replace(critGrpLn, """", "")
            While Split(critGrpLn, ",")(0) <> SaleIDsSel(i) Or Split(critGrpLn, ",")(1) <> Format(j, "0#")
                Line Input #2, critGrpLn
                critGrpLn = Replace(critGrpLn, """", "")
            Wend
            Close #2
            If Split(critGrpLn, ",")(2) = "I" Then
                'write out ECASLA criteria
                If Mid(Main_LoanSales.GetSaleType(), 1, 11) = "Special Pre" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", "('PRE ECASLA')"
                ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 9) = "ECASLA II" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", "('ECASLA II')"
                ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 8) = "ECASLA I" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", "('ECASLA I')"
                ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 10) = "Non ECASLA" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", "('NON ECASLA')"
                ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 7) = "Special" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", ECASLAList
                ElseIf Main_LoanSales.GetSaleType() = "Fully Pre" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", "('PRE ECASLA')"
                ElseIf Main_LoanSales.GetSaleType() = "Fully" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", ECASLAList
                ElseIf Main_LoanSales.GetSaleType() = "Fully OF" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", ECASLAList
                ElseIf Main_LoanSales.GetSaleType() = "Fully ZF" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", ECASLAList
                ElseIf Mid(Main_LoanSales.GetSaleType(), 1, 9) = "Mthly Pre" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", "('PRE ECASLA')"
                ElseIf Main_LoanSales.GetSaleType() = "Monthly" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", ECASLAList
                ElseIf Main_LoanSales.GetSaleType() = "Monthly OF" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", ECASLAList
                ElseIf Main_LoanSales.GetSaleType() = "Monthly ZF" Then
                    Write #1, SaleIDsSel(i), Format(j, "0#"), "113", "EQ", ECASLAList
                End If
            End If
        Next j
    Next i
    Close #1
    'save criteria selection info in the existing Cyprus text file to the new Cyprus text file for valid loan sales from Compass
    Open ProcDir & "Loan_Sales\Working_Files\utlwo2_3.txt" For Append As #1
    Open ProcDir & "Loan_Sales\Working_Files\xutlwo2_3.txt" For Input As #2
    Do Until EOF(2)
        Input #2, SaleID, GrpNo, CritCd, CritRel, CritVal
        For i = 0 To nos - 1
            If SaleID = SaleIDsSel(i) Then GoTo 3:
        Next i
        For i = 0 To nov - 1
            If SaleID = SaleIDsVal(0, i) Then
                Write #1, SaleID, GrpNo, CritCd, CritRel, CritVal
                Exit For
            End If
        Next i
3:  Loop
    Close #1
    Close #2
    Save2File = "Y"
End Function

'FTP new text files to Cyprus
Sub Promote()
    Dim FTPAttemps As Integer
    Dim FTPProcSuccessful As Boolean
    Dim EmailList As String
    Dim EmailListArray() As String

    Set fso = CreateObject("Scripting.FileSystemObject")

    Do
        FTPAttemps = FTPAttemps + 1
        'run the put_ls_ssh.bat batch program to FTP the files to Duster
        ftpvar = Shell(ProcDir & "Loan_Sales\Working_Files\put_ls_ssh.bat", 1)
        If FTPAttemps = 1 Then
            MsgBox "Click OK once the FTP process is done.", vbCritical
        Else
            MsgBox "The script encountered an error while trying to FTP the data files to DUSTER.  The script will now try again.  Click OK once the FTP process is done.", vbCritical
        End If
        
        'copy updated files to storage folder
        fso.CopyFile ProcDir & "Loan_Sales\Working_Files\utlwo2_1.txt", ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_1.txt"
        fso.CopyFile ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt", ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_2.txt"
        fso.CopyFile ProcDir & "Loan_Sales\Working_Files\utlwo2_3.txt", ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_3.txt"

        FTPProcSuccessful = FTPCheckOK()
        
    Loop Until FTPProcSuccessful = True Or FTPAttemps = 5

'if FTP was unsuccessful then send email and notify the user
    If FTPProcSuccessful = False And FTPAttemps = 5 Then
        ReDim EmailListArray(0) As String
        
        'get recipients from table
        EmailListArray = Sp.Common.Sql("SELECT WinUName + '@utahsbr.edu' FROM GENR_REF_MiscEmailNotif WHERE TypeKey = 'LSCritFTPError'")
        'start list with hardcoded recipients
        EmailList = "UHEAAOperationalAccounting@utahsbr.edu," & Environ("USERNAME") & "@utahsbr.edu"
        'add recipients to table to list
        For i = 1 To UBound(EmailListArray)
            EmailList = EmailList & "," & EmailListArray(i)
        Next i
        'send message
        Sp.Common.SendMail EmailList, , "Loan Sale FTP Problem", "There was a problem with the Loan Sale Criteria script during the FTP process.  If it isn't resolved then the loan sale process will not function properly.", , , , 2
        
        MsgBox "The FTP process was tried 5 times and was unsuccessful.  An email has been sent to certain Systems Support staff members notifying them of the problem.  If this is an emergency please personally contact Systems Support."
        End
    End If

    'review the files to make sure they are in sync
    If FileLen(ProcDir & "Loan_Sales\Working_Files\utlwo2_1.txt") > 0 Then ChkFileIntegrity
    
    'notify the user that processing is complete
    MsgBox "The loan sale information has been saved and promoted to Duster.", , "Changes Saved"
    End
End Sub

Function FTPCheckOK() As Boolean
    'check if all files match
    If FilesMatch("utlwo2_1.txt") = False Then Exit Function
    If FilesMatch("utlwo2_2.txt") = False Then Exit Function
    If FilesMatch("utlwo2_3.txt") = False Then Exit Function
    'everything went as it should have
    Open ProcDir & "Loan_Sales\Working_Files\FTP Error Log.txt" For Append As #3
    Write #3, userId, userName, CStr(Now()), "Y"
    Close #3
    FTPCheckOK = True
End Function

'checks if the two files are the same
Function FilesMatch(fName As String) As Boolean
    Dim File1Line As String
    Dim File2Line As String
    If Dir("T:\" & fName) = "" Then
        Exit Function
    End If
    Open "T:\" & fName For Input As #1
    Open ProcDir & "Loan_Sales\Working_Files\" & fName For Input As #2
    While Not EOF(1) And Not EOF(2)
        Line Input #1, File1Line
        Line Input #2, File2Line
        If File1Line <> File2Line Then
            Open ProcDir & "Loan_Sales\Working_Files\FTP Error Log.txt" For Append As #3
            Write #3, userId, userName, CStr(Now()), "N"
            Close #3
            Close #2
            Close #1
            MsgBox "An error occured wile trying to FTP the files down to Cyprus.  The script will now try and FTP the files again."
            Exit Function
        End If
    Wend
    If Not EOF(1) And EOF(2) Then
        Open ProcDir & "Loan_Sales\Working_Files\FTP Error Log.txt" For Append As #3
        Write #3, userId, userName, CStr(Now()), "N"
        Close #3
        Close #2
        Close #1
        MsgBox "An error occured wile trying to FTP the files down to Cyprus.  The script will now try and FTP the files again."
        Exit Function
    ElseIf Not EOF(2) And EOF(1) Then
        Open ProcDir & "Loan_Sales\Working_Files\FTP Error Log.txt" For Append As #3
        Write #3, userId, userName, CStr(Now()), "N"
        Close #3
        Close #2
        Close #1
        MsgBox "An error occured wile trying to FTP the files down to Cyprus.  The script will now try and FTP the files again."
        Exit Function
    End If
    Close #2
    Close #1
    FilesMatch = True
End Function

'locates a given test record from file 2 to test all other records against to ensure that all loan sales selected have the same criteria
Function GetControlRecordFile2(ByVal counter As Integer) As Boolean
    GetControlRecordFile2 = True
    ReDim CntlRec(9, 1 To 4)
    Open ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt" For Input As #1
    Do Until CntlRec(0, 1) = SaleIDsSel(0) Or EOF(1)
        Input #1, CntlRec(0, 1), CntlRec(0, 2), CntlRec(0, 3), CntlRec(0, 4)
    Loop
    If EOF(1) = True Then
        VerNewSale
        Close #1
        GetControlRecordFile2 = False
        Exit Function
    Else
        While (counter < 10 And CntlRec(counter - 1, 1) = SaleIDsSel(0) And Not EOF(1))
            Input #1, CntlRec(counter, 1), CntlRec(counter, 2), CntlRec(counter, 3), CntlRec(counter, 4)
            counter = counter + 1
        Wend
        CntlRec(counter - 1, 1) = ""
        CntlRec(counter - 1, 2) = ""
        CntlRec(counter - 1, 3) = ""
        CntlRec(counter - 1, 4) = ""
    End If
    Close #1
End Function

'locates a given test record from file 3 to test all other records against to ensure that all loan sales selected have the same criteria
Function GetControlRecordFile3(ByVal counter As Integer) As Boolean
    GetControlRecordFile3 = True
    ReDim CntlRec(9, 1 To 5)
    Open ProcDir & "Loan_Sales\Working_Files\utlwo2_3.txt" For Input As #1
    Do Until CntlRec(0, 1) = SaleIDsSel(0) Or EOF(1)
        Input #1, CntlRec(0, 1), CntlRec(0, 2), CntlRec(0, 3), CntlRec(0, 4), CntlRec(0, 5)
    Loop
    If EOF(1) = True Then
        Mismatch
        Close #1
        GetControlRecordFile3 = False
        Exit Function
    Else
        While (counter < 10 And CntlRec(counter - 1, 1) = SaleIDsSel(0) And Not EOF(1))
            Input #1, CntlRec(counter, 1), CntlRec(counter, 2), CntlRec(counter, 3), CntlRec(counter, 4), CntlRec(counter, 5)
            counter = counter + 1
        Wend
        CntlRec(counter - 1, 1) = ""
        CntlRec(counter - 1, 2) = ""
        CntlRec(counter - 1, 3) = ""
        CntlRec(counter - 1, 4) = ""
        CntlRec(counter - 1, 5) = ""
    End If
    Close #1
End Function

'tests test record found in "GetControlRecord2" to ensure that all loan sales selected have the same criteria
Function CheckOtherRecordsFile2(ByVal counter As Integer) As Boolean
    CheckOtherRecordsFile2 = True
        For j = 1 To nos - 1
        counter = 1
            ReDim MatchCrit(9, 1 To 4)
            Open ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt" For Input As #2
            Do
                If EOF(2) = True Then
                    Mismatch
                    Close #2
                    CheckOtherRecordsFile2 = False
                    Exit Function
                End If
                Input #2, MatchCrit(0, 1), MatchCrit(0, 2), MatchCrit(0, 3), MatchCrit(0, 4)
            Loop Until MatchCrit(0, 1) = SaleIDsSel(j) And _
                       MatchCrit(0, 2) = CntlRec(0, 2) And _
                       MatchCrit(0, 3) = CntlRec(0, 3)
            While (counter < 10 And MatchCrit(counter - 1, 1) = SaleIDsSel(j) And Not EOF(2))
                Input #2, MatchCrit(counter, 1), MatchCrit(counter, 2), MatchCrit(counter, 3), MatchCrit(counter, 4)
                counter = counter + 1
            Wend
            If (Not EOF(2)) Then
                MatchCrit(counter - 1, 1) = ""
                MatchCrit(counter - 1, 2) = ""
                MatchCrit(counter - 1, 3) = ""
                MatchCrit(counter - 1, 4) = ""
            End If
            Close #2
            If (Not RecordCompareFile2()) Then
                Mismatch
                CheckOtherRecordsFile2 = False
                Exit Function
            End If
        Next j
End Function

'tests test record found in "GetControlRecord3" to ensure that all loan sales selected have the same criteria
Function CheckOtherRecordsFile3(ByVal counter As Integer)
    For j = 1 To nos - 1
    counter = 1
            ReDim MatchCrit(9, 1 To 5)
            Open ProcDir & "Loan_Sales\Working_Files\utlwo2_3.txt" For Input As #2
            Do
                If EOF(2) = True Then
                    Mismatch
                    Close #2
                    Exit Function
                End If
                Input #2, MatchCrit(0, 1), MatchCrit(0, 2), MatchCrit(0, 3), MatchCrit(0, 4), MatchCrit(0, 5)
            Loop Until MatchCrit(0, 1) = SaleIDsSel(j) And _
                       MatchCrit(0, 2) = CntlRec(0, 2) And _
                       MatchCrit(0, 3) = CntlRec(0, 3) And _
                       MatchCrit(0, 4) = CntlRec(0, 4) And _
                       MatchCrit(0, 5) = CntlRec(0, 5)
            While (counter < 10 And MatchCrit(counter - 1, 1) = SaleIDsSel(j) And Not EOF(2))
                Input #2, MatchCrit(counter, 1), MatchCrit(counter, 2), MatchCrit(counter, 3), MatchCrit(counter, 4), MatchCrit(counter, 5)
                counter = counter + 1
            Wend
            If (Not EOF(2)) Then
                MatchCrit(counter - 1, 1) = ""
                MatchCrit(counter - 1, 2) = ""
                MatchCrit(counter - 1, 3) = ""
                MatchCrit(counter - 1, 4) = ""
                MatchCrit(counter - 1, 5) = ""
            End If
            Close #2
            If (Not RecordCompareFile3()) Then
                Mismatch
                Exit Function
            End If
        Next j
End Function

'does all data checks to ensure that all loan sales selected have the same criteria set up
Sub VerCritMatch()
    Dim counter As Integer
    SaleIDsSel = Main_LoanSales.GetSelectionArray(nos)
    counter = 1
    If (0 <> FileLen(ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt")) Then
        If (GetControlRecordFile2(counter)) Then
            If (CheckOtherRecordsFile2(counter)) Then
                If (GetControlRecordFile3(counter)) Then
                    CheckOtherRecordsFile3 (counter)
                End If
            End If
        End If
    End If
End Sub

Function RecordCompareFile3() As Boolean
    Dim counter As Integer
    While (counter < 10)
        If (MatchCrit(counter, 2) <> CntlRec(counter, 2) Or MatchCrit(counter, 3) <> CntlRec(counter, 3) _
        Or MatchCrit(counter, 4) <> CntlRec(counter, 4) Or MatchCrit(counter, 5) <> CntlRec(counter, 5)) Then
            RecordCompareFile3 = False
            Exit Function
        End If
        counter = counter + 1
    Wend
    RecordCompareFile3 = True
End Function

Function RecordCompareFile2() As Boolean
    Dim counter As Integer
    While (counter < 10)
        If (MatchCrit(counter, 2) <> CntlRec(counter, 2) Or MatchCrit(counter, 3) <> CntlRec(counter, 3) _
        Or MatchCrit(counter, 4) <> CntlRec(counter, 4)) Then
            RecordCompareFile2 = False
            Exit Function
        End If
        counter = counter + 1
    Wend
    RecordCompareFile2 = True
End Function

Sub VerNewSale()
    ns = ""
    ReDim MatchCrit(1 To 4)
    For i = 1 To nos - 1
        Open ProcDir & "Loan_Sales\Working_Files\utlwo2_2.txt" For Input As #15
        Do Until EOF(15)
            Input #15, MatchCrit(1), MatchCrit(2), MatchCrit(3), MatchCrit(4)
            If MatchCrit(1) = SaleIDsSel(i) Then
                Close #15
                Mismatch
                Exit Sub
            End If
        Loop
        Close #15
    Next i
    ns = "Y"
End Sub

Sub Mismatch()
    warn = MsgBox("All of the loan sales selected do not currently have the same criteria.  Do you want all of the selected loan sales to have the same criteria?  Click Yes to select the criteria or No to quit.", vbYesNo, "Loan Sale Criteria Mismatch")
    If warn <> 6 Then End
End Sub

'verify that the files are in sync
Sub ChkFileIntegrity()
    Dim fErr As String
    Dim fErr1 As String
    Dim Msg As String
    'holds file data index 0 = salesID, 1 = sellerID, 2 = buyerID, 3 = Pre1date, 4 = pre2date, 5 = Pre3date, 6 = pre4date, 7 = pre5date, 8 = special run, 9 = LockDownDate, 10 = salesdate, 11 = Max Sale Value, 12 = Exceed Max, 13 = Include Interest, 14 = Include Late Fees, 15 = Sale Criteria, 16 = flag
    'holds file data index 0 = salesID, 1 = sellerID, 2 = buyerID, 3 = Pre1date, 4 = pre2date, 5 = special run, 6 = LockDownDate, 7 = salesdate, 8 = Sale Criteria
    Dim Data(0 To 15) As String
    Dim Data2(0 To 4) As String
         
'verify that each sale in file 1 has a record in file 2, warn the user otherwise
    Msg = "The following sales have been set up in COMPASS but do not have any criteria selected.  No loans will be selected for the sales until criteria is selected." & Chr(13) & Chr(13) & "SaleID" & Chr(9) & "Seller"
    Open ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_1.txt" For Input As #1
    If FileLen(ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_2.txt") = 0 Then
        Do Until EOF(1)
'<1>        Input #1, data(0), data(1), data(2), data(3), data(4), data(5), data(6)
            Input #1, Data(0), Data(1), Data(2), Data(3), Data(4), Data(5), Data(6), Data(7), Data(8), Data(9), Data(10), Data(11), Data(12), Data(13), Data(14), Data(15) '<1>
            Msg = Msg & Chr(13) & Data(0) & Chr(9) & Data(1)
        Loop
        MsgBox Msg, , "Criteria not Selected"
        If FileLen(ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_3.txt") <> 0 Then
            MsgBox "The utlwo2_3 criteria detail file contains detail for groups which are not in the utlwo2_2 criteria group file.  The files have become corrupted.  Contact Systems Support for assistance.", , "File Error"
        End If
        Close #1
        Exit Sub
    Else
        fErr1 = ""
        Do Until EOF(1)
'<1>        Input #1, data(0), data(1), data(2), data(3), data(4), data(5), data(6)
            Input #1, Data(0), Data(1), Data(2), Data(3), Data(4), Data(5), Data(6), Data(7), Data(8), Data(9), Data(10), Data(11), Data(12), Data(13), Data(14), Data(15)       '<1>
            fErr = ""
            Open ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_2.txt" For Input As #2
            Do Until EOF(2)
                Input #2, Data2(0), Data2(1), Data2(2), Data2(3)
                If Data(0) = Data2(0) Then
                    fErr = "N"
                    Exit Do
                End If
            Loop
            Close #2
            If fErr <> "N" Then
                Msg = Msg & Chr(13) & Data(0) & Chr(9) & Data(1)
                fErr1 = "Y"
            End If
        Loop
        If fErr1 = "Y" Then MsgBox Msg, , "Criteria not Selected"
    End If
    Close #1
        
'verify there is detail in file 3 for each group in file 2
    Open ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_2.txt" For Input As #1
    Do Until EOF(1)
        Input #1, Data(0), Data(1), Data(2), Data(3)
        fErr = ""
        Open ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_3.txt" For Input As #2
        Do Until EOF(2)
            Input #2, Data2(0), Data2(1), Data2(2), Data2(3), Data2(4)
            If Data(0) = Data2(0) And Data(1) = Data2(1) Then
                fErr = "N"
                Exit Do
            End If
        Loop
        Close #2
        If fErr <> "N" Then
            MsgBox "The utlwo2_2 criteria group file contains groups for which the utlwo2_3 criteria detail file does not contain detail.  The files have become corrupted.  Contact Systems Support for assistance.", , "File Error"
            Exit Do
        End If
    Loop
    Close #1
    
'verify there is a group in file 2 for each detail record in file 3
    Open ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_3.txt" For Input As #1
    Do Until EOF(1)
        Input #1, Data(0), Data(1), Data(2), Data(3), Data(4)
        fErr = ""
        Open ProcDir & "Loan_Sales\Working_Files\Cyprus\utlwo2_2.txt" For Input As #2
        Do Until EOF(2)
            Input #2, Data2(0), Data2(1), Data2(2), Data2(3)
            If Data(0) = Data2(0) And Data(1) = Data2(1) Then
                fErr = "N"
                Exit Do
            End If
        Loop
        Close #2
        If fErr <> "N" Then
            MsgBox "The utlwo2_3 criteria detail file contains detail for groups which are not in the utlwo2_2 criteria group file.  The files have become corrupted.  Contact Systems Support for assistance.", , "File Error"
            Exit Do
        End If
    Loop
    Close #1
    
End Sub

'translates sale comment text to type codes
Function TranslateType(Line1Txt As String, Line2Txt As String) As String
    If Line1Txt = UCase("Monthly Loan Sale") Then
        TranslateType = "Monthly"
    ElseIf Line1Txt = UCase("Fully Originated 90 days") Then
        TranslateType = "Fully"
    ElseIf Line1Txt = UCase("Special Sale") Then
        TranslateType = "Special"
    ElseIf Line1Txt = "MONTHLY PRE-ECASLA LOAN SALE" Then
        TranslateType = "Mthly Pre"
    ElseIf Line1Txt = "FULLY ORIGINATED PRE-ECASLA LOAN SALE" Then
        TranslateType = "Fully Pre"
    ElseIf Line1Txt = "SPECIAL SALE PRE-ECASLA" Then
        TranslateType = "Special Pre"
    ElseIf Line1Txt = "SPECIAL SALE ECASLA I" Then
        TranslateType = "ECASLA I"
    ElseIf Line1Txt = "SPECIAL SALE ECASLA II" Then
        TranslateType = "ECASLA II"
    ElseIf Line1Txt = "SPECIAL SALE NON ECASLA" Then
        TranslateType = "Non ECASLA"
    End If
    If TranslateType = "" Then
        MsgBox "The script encountered a sale type it couldn't translate.  Please contact Systems Support.", vbCritical
        End
    End If
    If Line2Txt = UCase("Loans with Origination Fee and with Zero Origination Fee") Then
        'leave second part of indicator blank
    ElseIf Line2Txt = UCase("Loans with Origination Fee Only") Then
        TranslateType = TranslateType & " OF"
    ElseIf Line2Txt = UCase("Loans with Zero Origination Fee Only") Then
        TranslateType = TranslateType & " ZF"
    End If
End Function

'translates sale comment text to type codes
Function RevTranslateType(code As String) As String
    If Mid(code, 1, 7) = "Monthly" Then
        RevTranslateType = "Monthly Loan Sale"
    ElseIf Mid(code, 1, 5) = "Fully" Then
        RevTranslateType = "Fully Originated 90 days"
    ElseIf Mid(code, 1, 7) = "Special" Then
        RevTranslateType = "Special Sale"
    End If
    If RevTranslateType = "" Then
        RevTranslateType = "ERROR"
        Exit Function
    End If
    If Len(code) = 1 Then
        Exit Function
    ElseIf Mid(code, Len(code) - 2, 3) = " OF" Then
        RevTranslateType = RevTranslateType & " - Loans with Origination Fee Only"
    ElseIf Mid(code, Len(code) - 2, 3) = " ZF" Then
        RevTranslateType = RevTranslateType & " - Loans with Zero Origination Fee Only"
    End If
End Function


'<1> sr 961, aa, 03/30/05,  'create ref to Groupwise, FTP obj and telnet obj, move standard and fully originated crit lists from test to live, Copy report document
'<2> sr1313, aa
'<3> sr1772, jd
'<4> sr1859, tp, 10/03/06
