VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} CanRefPmtInfo 
   Caption         =   "Cancel/Refund Payment Information"
   ClientHeight    =   8970
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5655
   OleObjectBlob   =   "CanRefPmtInfo.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "CanRefPmtInfo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False





'<23->
Dim FirstDisbDt As String
Dim LnList As Variant
Dim ReaList As Variant


'Clear selection of radio buttons
Private Sub btnClear_Click()
    rdoStop.Value = False
    rdoVoid.Value = False
    ProtectIt
    TotPmtAmt.SetFocus
End Sub

Private Sub Label1_Click()

End Sub

Private Sub rdoStop_Click()
    ProtectIt
    txtCheckNo.SetFocus
End Sub

Private Sub rdoVoid_Click()
    ProtectIt
    txtCheckNo.SetFocus
End Sub

Private Sub rdoLoan_Click()
    ProtectIt
    TotPmtAmt.SetFocus
End Sub

Function ProtectIt()
    If rdoStop.Value Or rdoVoid.Value Then
        LnTyp.Enabled = True
        SeqNo.Enabled = True
        DisbDt.Enabled = True
        DisbNo.Enabled = True
        LnTyp.BackColor = &H80000005
        SeqNo.BackColor = &H80000005
        DisbDt.BackColor = &H80000005
        DisbNo.BackColor = &H80000005
        Label21.Visible = True
        Label22.Visible = True
    
        txtCheckNo.Enabled = True
        txtChkReason.Enabled = True
        txtCheckNo.BackColor = &H80000005
        txtChkReason.BackColor = &H80000005
        Label32.Visible = True
        Label33.Visible = True
    Else
        txtCheckNo = ""
        txtChkReason = ""
        txtCheckNo.BackColor = &H80000000
        txtChkReason.BackColor = &H80000000
        txtCheckNo.Enabled = False
        txtChkReason.Enabled = False
        Label32.Visible = False
        Label33.Visible = False
    End If
    If rdoLoan Then
        LnTyp = "STFFRD"
        SeqNo = "1"
        DisbDt = "00/00/0000"
        DisbNo = ""
        LnTyp.BackColor = &H80000000
        SeqNo.BackColor = &H80000000
        DisbDt.BackColor = &H80000000
        DisbNo.BackColor = &H80000000
        LnTyp.Enabled = False
        SeqNo.Enabled = False
        DisbDt.Enabled = False
        DisbNo.Enabled = False
        Label21.Visible = False
        Label22.Visible = False
    End If
End Function

Private Sub reissu_Click()
''    If reissu.Value Then
''        Label23.Visible = True
''        Label24.Visible = True
''        txtNewDsbAmt.Visible = True
''        txtNewDsbDt.Visible = True
''        Label19.Visible = True
''        Label20.Visible = True
''        txtReason.Visible = True
''        lblReason.Visible = True
''    Else
''        Label23.Visible = False
''        Label24.Visible = False
''        txtNewDsbAmt.Visible = False
''        txtNewDsbDt.Visible = False
''        Label19.Visible = False
''        Label20.Visible = False
''        txtReason.Visible = False
''        lblReason.Visible = False
''    End If
    
    If reissu.Value Then
        txtNewDsbAmt.BackColor = &H80000005
        txtNewDsbDt.BackColor = &H80000005
        txtReason.BackColor = &H80000005
        txtNewDsbAmt.Enabled = True
        txtNewDsbDt.Enabled = True
        txtReason.Enabled = True
        Label23.Visible = True
        Label24.Visible = True
        txtNewDsbDt.SetFocus
    Else
        txtNewDsbAmt = ""
        txtNewDsbDt = ""
        txtReason = ""
        txtNewDsbAmt.BackColor = &H80000000
        txtNewDsbDt.BackColor = &H80000000
        txtReason.BackColor = &H80000000
        txtNewDsbAmt.Enabled = False
        txtNewDsbDt.Enabled = False
        txtReason.Enabled = False
        Label23.Visible = False
        Label24.Visible = False

    End If
End Sub

Private Sub UserForm_Initialize()

    LnList = Sp.Common.Sql("SELECT LoanType FROM GENR_REF_LoanTypes WHERE System = 'Compass'")
'    LnList = Array("STFFRD", "UNSTFD", "SLS", "PLUS", "SUBCNS", "UNCNS", "GATEUG", "GATEGL", "GATEMD")
    LnTyp.List() = LnList
    FirstDisbDt = "blank"
    
    txtChkReason.List() = Array("Lost check not received by school", "Lost check not received by borrower")
    
    If Not CanRefPmtEntry.PassOnSysStatus Then
        rdoLoan.Value = True
        rdoStop.Enabled = False
        rdoVoid.Enabled = False
    Else
        LnTyp.SetFocus
    End If
End Sub
'</23>


Private Sub Cancel_Click()
    Okay = 2
    Me.Hide
End Sub


Private Sub OK_Click()
'<23->
''<21->
''    Okay = 1
''    Me.Hide
'    'warn the user if the SSN and unique ID are missing
'    If SSN = "" And id = "" Then
'        MsgBox "You must enter either the SSN or the unique ID.", 48, "Missing Information"
'    'warn the user if the unique ID and loan sequence number are missing
'    ElseIf id = "" And SeqNo = "" Then
'        MsgBox "You must enter either the unique ID or the sequence number.", 48, "Missing Information"
'    'get the SSN from LG02 if it is missing
'    ElseIf SSN = "" Then
'        'access LG02
'        fastpath "LG02I;;;" & id
'        'warn the user if the unique ID is not found
'        If check4text(1, 60, "LOAN APPLICATION MENU") Then
'            MsgBox "The unique ID entered was not found.", 48, "Unique ID not Found"
'        'get the SSN and continue processing if the unique ID is found
'        Else
'            SSN = GetText(1, 9, 9)
'            Okay = 1
'            Me.Hide
'        End If
'    'continue processing if all required data is present
'    Else
'        Okay = 1
'        Me.Hide
'    End If
''</21>
    Dim row As Integer
    Dim mtchs As Integer                                                '<24>

    'warn the user if required information is not entered or is not valid
    If Not rdoLoan And LnTyp = "" And val(SeqNo) = 0 Then
        MsgBox "You must enter the loan type or loan sequence number.", 48, "Enter Missing Information"
    ElseIf Not rdoLoan And val(SeqNo) = 0 And DisbDt = "" Then
        MsgBox "You must enter the disbursement date if the loan sequence number is left blank.", 48, "Enter Missing Information"
    ElseIf Not rdoLoan And DisbDt = "" And DisbNo = "" Then
        MsgBox "You must enter the disbursement date or number.", 48, "Enter Missing Information"
    ElseIf Not rdoLoan And DisbDt <> "" And Not Sp.DateFormat(DisbDt, 10) Then
        MsgBox "The disbursement date entered was not a valid date.", 48, "Invalid Date Entered"
    ElseIf EffDate <> "" And Not Sp.DateFormat(EffDate, 10) Then
        MsgBox "The effective date entered was not a valid date.", 48, "Invalid Date Entered"
    ElseIf TotPmtAmt = "" Then
        MsgBox "You must enter the payment amount.", 48, "Enter Missing Information"
'<26>
    ElseIf reissu.Value = True And IsDate(txtNewDsbDt.Text) = False Then
        MsgBox "The New Disbursement Date is invalid."
    ElseIf reissu.Value = True And IsNumeric(txtNewDsbAmt.Text) = False Then
        MsgBox "The new Disbursement Amount is invalid."
'</26>
    ElseIf (rdoStop.Value = True Or rdoVoid.Value = True) And txtChkReason = "" Then
        MsgBox "You must enter the reason when Stop Payment or Voided Check is selected.", 48, "Enter Missing Information"
    'find and process loan if enough information was entered
    ElseIf Not rdoLoan Then
'<24->
'        If LnTyp = "" Or DisbDt = "" And Not CanRefPmtEntry.HasGotTS26 Then TS26
'        'look for loan if ln seq no was not entered
'        'access TS2H
'        FastPath "TX3Z/ITS2H" & SSN.Text
'        'find the loan if on the selection screen
'        If Check4Text(1, 72, "TSX2I") Then
'            row = 8
'            While Not Check4Text(23, 2, "90007")
'                'select the loan and look for the disb date if the loan type matches
'                If GetText(row, 16, 6) = LnTyp And (FrstDisbDt = "" Or GetText(row, 7, 8) = Format(FrstDisbDt, "MM/DD/YY")) Then
'                    PutText 21, 12, Session.GetDisplayText(row, 2, 2), "ENTER"
'                    If CanRefPmtEntry.TS2H Then
'                        If Not CanRefPmtEntry.HasGotTS26 Then TS26
'                        Okay = 1
'                        Me.Hide
'                        Exit Sub
'                    End If
'                    Hit "F12"
'                End If
        'if the loan type and disb date were entered but the loan seq no was not
        If LnTyp <> "" And DisbDt <> "" And val(SeqNo) = 0 Then
            mtchs = 0
            'access TS2H
            FastPath "TX3Z/ITS2H" & ssn.Text & LnTyp
            'find the loan if on the selection screen
            If check4text(1, 72, "TSX2I") Then
                row = 8
                While Not check4text(23, 2, "90007")
                    'select the loan and look for the disb date
                    puttext 21, 12, Session.GetDisplayText(row, 2, 2), "ENTER"
                    If CanRefPmtEntry.TS2H Then mtchs = mtchs + 1
                    hit "F12"
'</24>
                    row = row + 1
    
                    'go to the next page
                    If check4text(row, 3, " ") Then
                        hit "F8"
                        row = 8
                    End If
                Wend
'<24->
                'get TS26 info if only one match was found
                If mtchs = 1 Then
                    If DupTS26 Then
                        MsgBox "There are two or more loans on TS26 with the same loan type/first disbursement as the loan found on TS2H.  Click OK to pause the script, review the account, and determine the loan sequence number of the correct loan.  Hit <Insert> when you are ready and the form will be displayed for you to enter the loan sequence number.", 48, "Multiple Loan Matches Found"
                        Me.Hide
                        Sp.PauseForInsert
                        Me.Show
                        Exit Sub
                    End If
                    If TS26(False) Then
                        If Not IsCheckDuplicate Then
                            Okay = 1
                            Me.Hide
                        End If
                        Exit Sub
                    End If
                'warn the user if more than one match was found
                ElseIf mtchs > 1 Then
                    MsgBox "There are two or more loans with the loan type/disbursement date entered.  Click OK to pause the script, review the account, and determine the loan sequence number of the correct loan.  Hit <Insert> when you are ready and the form will be displayed for you to enter the loan sequence number.", 48, "Multiple Loan Matches Found"
                    Me.Hide
                    Sp.PauseForInsert
                    Me.Show
                    Exit Sub
                End If
'</24>
            'verify the loan was found if on the detail screen
            ElseIf check4text(1, 72, "TSX2J") Then
                If CanRefPmtEntry.TS2H Then
'<24->
'                   If Not CanRefPmtEntry.HasGotTS26 Then TS26
                    If TS26(False) Then
                        If Not IsCheckDuplicate Then
                            Okay = 1
                            Me.Hide
                        End If
                        Exit Sub
                    End If
                End If
            End If

        'if the loan seq no was entered, get TS26 and TS2H info
        ElseIf TS26(True) Then
            If Not IsCheckDuplicate Then
                Okay = 1
                Me.Hide
            End If
            Exit Sub
        End If
        
        'warn the user, pause the script, and return to the form if the loan was not found
        MsgBox "The loan could not be found using the data that was entered.  Click OK to pause the script and review the account.  Hit <Insert> when you are ready and the form will be displayed for you to correct the information.", 48, "Loan not Found"
        Me.Hide
        Sp.PauseForInsert
        Me.Show
    Else
        Me.Hide
    End If
'</23>
End Sub

Function IsCheckDuplicate() As Boolean
    Dim dSSN As String
    Dim dSeqNo As String
    Dim dDisbDate As String
    Dim dVar As String
    
    IsCheckDuplicate = False
    
    If rdoStop Or rdoVoid Then
        If CDbl(TotPmtAmt) <> CDbl(txtCheckAmt) Then
            MsgBox "The payment amount entered does not match the check amount on TS2H.", vbExclamation, "Amount does not Match"
            TotPmtAmt.SetFocus
            IsCheckDuplicate = True
        ElseIf CDbl(txtCheckNo) <> CDbl(txtCheckNo2) Then
            MsgBox "The check number entered does not match the check number on TS2H.", vbExclamation, "Check Number does not Match"
            txtCheckNo.SetFocus
            IsCheckDuplicate = True
        End If
    End If
    
    If IsCheckDuplicate Or Dir("T:\pmtlistcr.txt") = "" Then Exit Function
    
    Open "T:\pmtlistcr.txt" For Input Access Read As #1
    Do While Not EOF(1)
        Input #1, dSSN, dSeqNo, dVar, dVar, dVar, dVar, dVar, dVar, dDisbDate, dVar, dVar, dVar, dVar, dVar, dVar, dVar, dVar
        If ssn <> "11111111" And ssn = dSSN And val(SeqNo) = val(dSeqNo) And DisbDt = dDisbDate Then
            Close #1
            MsgBox "Review the loan because possible duplicate loan sequence numbers have been selected.  Click OK to pause the script, review the account, and then hit <Insert> when you are ready to proceed.", 48, "Review Account"
            Me.Hide
            Sp.PauseForInsert
            
            If MsgBox("Do you want to continue processing the payment as entered?", vbYesNo + vbQuestion, "Continue Processing") = vbNo Then
                IsCheckDuplicate = True
                Me.Show
            End If

            Exit Function
        End If
    Loop
    Close #1

End Function
'</23->
'find the loan on TS26
Private Function TS26(GetTS2H As Boolean) As Boolean                                      '<24> added argument and boolean type
    Dim row As Integer
'<24->
    Dim mtchs As Integer

    TS26 = True
    mtchs = 0
'</24>

    'access TS26
        FastPath "TX3Z/ITS26" & ssn.Text
        'find the loan if on the selection screen
        If check4text(1, 72, "TSX28") Then
            row = 8
            While Not check4text(23, 2, "90007")
                'select the loan and get loan information if the loan is found
'<24->
'               If Val(GetText(row, 14, 4)) = Val(SeqNo) Or (GetText(row, 5, 8) = FrstDisbDt And GetText(row, 19, 6) = LnTyp) Then
                If val(GetText(row, 14, 4)) = val(SeqNo) Or _
                   (val(SeqNo) = 0 And (GetText(row, 5, 8) = FrstDisbDt And GetText(row, 19, 6) = LnTyp)) Then
'</24>
                    puttext 21, 12, Session.GetDisplayText(row, 2, 2), "ENTER"
                    CanRefPmtEntry.GetTS26Info
'<24->
                    If GetTS2H Then
                        hit "F7"
                        If Not CanRefPmtEntry.TS2H Then TS26 = False
                    End If
'</24>
                    Exit Function

                End If
                row = row + 1
                
                'go to the next page
                If check4text(row, 3, " ") Then
                    hit "F8"
                    row = 8
                End If
            Wend
        'verify the loan was found if on the detail screen
        ElseIf check4text(1, 72, "TSX29") Then
            'select the loan and get loan information if the loan is found
            If val(GetText(7, 35, 4)) = val(SeqNo) Or (GetText(6, 18, 8) = FrstDisbDt And GetText(6, 66, 6) = LnTyp) Then
                CanRefPmtEntry.GetTS26Info
'<24->
                If GetTS2H Then
                    hit "F7"
                    If Not CanRefPmtEntry.TS2H Then TS26 = False
                End If
'</24>
                Exit Function
            End If
        End If
    
'<24->
'        'warn the user and return to the form if the loan was not found
'        MsgBox "The the loan sequence number entered was not found.", 48, "Loan not Found"
        TS26 = False
'</24>
End Function
'</23>

'<24->
'check for duplicate matches on TS26 for first disb date and loan type
Function DupTS26() As Boolean
    Dim row As Integer
    Dim mtchs As Integer

    DupTS26 = False
    mtchs = 0

    'access TS26
    FastPath "TX3Z/ITS26" & ssn.Text & LnTyp
    'find the loan if on the selection screen
    If check4text(1, 72, "TSX28") Then
        row = 8
        While Not check4text(23, 2, "90007")
            'select the loan and get loan information if the loan is found
            If GetText(row, 5, 8) = FrstDisbDt Then
                mtchs = mtchs + 1
                If mtchs > 1 Then
                    DupTS26 = True
                    Exit Function
                End If
            End If
            row = row + 1
            
            'go to the next page
            If check4text(row, 3, " ") Then
                hit "F8"
                row = 8
            End If
        Wend
    End If
End Function
'</24>

Private Sub PrintBatch_Click()
    Okay = 3
    Me.Hide
End Sub

