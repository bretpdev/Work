VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmLVC 
   Caption         =   "Compass LVC"
   ClientHeight    =   7365
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4980
   OleObjectBlob   =   "frmLVC.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frmLVC"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False




Private Enum Dat    'This should match the merge fields written in CreatePrintRecords, and comes in handy in PrintApprovedLVCs.
    AgName  'Borrower-level stuff starts here.
    HldCode1
    HldName1
    PayOffDt
    AddComs
    BrwFDCode
    BrwSSN
    BrwFN
    BrwMI
    BrwLN
    BrwAdd1
    BrwAdd2
    BrwCity
    BrwST
    BrwZip
    BrwCntry
    BrwPhn
    BrwTtl
    Ineltext
    CurDate
    HldName2
    HldCode2
    LnAcntNo    'Loan-level stuff starts here.
    LnTypCd
    IntRt
    Principal
    Interest
    total
    DSBDT
    InSch
    SchCd
    ComplDt
    Dflt
    LnCd
    GACd
    BrwSta
    GrcEnd
    FirstPmt
    DlyInt
End Enum
Private Type BorrowerInfo
    SSN As String
    AcctNo As String
    lastName As String
    MI As String
    firstName As String
    address1 As String
    address2 As String
    City As String
    State As String
    ZIP As String
    country As String
    Phone As String
    BBRate As String
End Type
Private Type LoanInfo
    AcntSta As String
    AcntNo As String
    LnSeq As String
    OwnerName As String
    Owner As String
    LnTyp As String
    LnTypCd As String
    disbDate As String
    Rate As String
    Prin As Double
    GrcEndDt As String
    FirstDue As String
    CurrFirstDue As String
    OosDate As String
    School As String
    Status As String
    CLID As String
    FullDisb As Boolean
    TotalAmt As String
    PrincAmt As String
    IntAmt As String
    PerDiem As String
    IsElig As Boolean
    LnCd As String     'FL item 6
    TransDate As Date
End Type
Private aLD() As LoanInfo
Private taLD As LoanInfo
Private UserId As String
Private DocFolder As String
Private ToPrinter As Boolean
Private ArchLib As String
Private LndrList() As String
Private ProofOfAppSgDtRequired As Boolean
Private LetterFolder As String
Private RE1 As String
Private RE2 As String
Private RE3 As String
Private RE4 As String
Private RE5 As String
Private RE6 As String
Private RE7 As String
Private RE8 As String
Private RE9 As String
Private RE10 As String
Private RE11 As String
Private RE12 As String
Private RE13 As String
Private Re14 As String
Private Re15 As String
Private Re16 As String
Private Binfo As BorrowerInfo
Private DeniedReason As String

Private Sub UserForm_Initialize()
    'Start with the small version of the form.
    lblPrompt.Visible = False
    lblName.Visible = False
    lblAddress1.Visible = False
    lblAddress2.Visible = False
    lblCity.Visible = False
    lblState.Visible = False
    lblZip.Visible = False
    lblCountry.Visible = False
    txtName.Visible = False
    txtAddress1.Visible = False
    txtAddress2.Visible = False
    txtCity.Visible = False
    txtState.Visible = False
    txtZip.Visible = False
    txtCountry.Visible = False
    btnOK.Top = 152
    btnCancel.Top = 152
    frmLVC.Height = 210
    
    'get user ID from LVC module
    UserId = LVC.PassUserID
                
    'set docfolder value
    DocFolder = "X:\PADD\LoanOrigination\"
    LetterFolder = "X:\PADD\AccountServices\"
    SP.Common.TestMode , LetterFolder
    SP.Common.TestMode , DocFolder
    
    'Pre-set the Anticipated Payoff Date to 30 days from now.
    txtAntPODate.Text = DateAdd("d", 30, Date)
    
    'Load agency names from BSYS.
    Dim QueryList() As String
    QueryList = SP.Common.SQLEx("SELECT Name FROM GENR_LST_Lenders ORDER BY Name")
    'The query results have two indices, which are in the reverse order from what Windows forms expect.
    'Copy the results into the LndrList array, using QueryList's second array index. Don't copy the last item, since it's empty.
    ReDim LndrList(UBound(QueryList, 2) - 1)
    Dim i As Integer
    For i = 0 To UBound(LndrList)
        LndrList(i) = QueryList(0, i)
    Next
    cmbAgency.list() = LndrList
    cmbAgency.AddItem "other"
End Sub

'Adjust the form based on the agency.
Private Sub cmbAgency_Change()
    'Allow a Federal Direct code only if the agency is Federal Direct.
    If cmbAgency.Text = "Federal Direct" Then
        txtFedDirCd.BackColor = &H80000005
        txtFedDirCd.Locked = False
        txtAppRectDate.BackColor = &H80000005
        txtAppRectDate.Locked = False
    Else
        txtFedDirCd.Text = ""
        txtFedDirCd.BackColor = &H80000000
        txtFedDirCd.Locked = True
        txtAppRectDate.Text = ""
        txtAppRectDate.BackColor = &H80000000
        txtAppRectDate.Locked = True
    End If
    'Show address input fields if the agency is "other" or hide the fields for existing agencies.
    If cmbAgency.Text = "other" Then
        frmLVC.Height = 387.75
        btnOK.Top = 330
        btnCancel.Top = 330
        lblPrompt.Visible = True
        lblName.Visible = True
        lblAddress1.Visible = True
        lblAddress2.Visible = True
        lblCity.Visible = True
        lblState.Visible = True
        lblZip.Visible = True
        lblCountry.Visible = True
        txtName.Visible = True
        txtAddress1.Visible = True
        txtAddress2.Visible = True
        txtCity.Visible = True
        txtState.Visible = True
        txtZip.Visible = True
        txtCountry.Visible = True
    Else
        lblPrompt.Visible = False
        lblName.Visible = False
        lblAddress1.Visible = False
        lblAddress2.Visible = False
        lblCity.Visible = False
        lblState.Visible = False
        lblZip.Visible = False
        lblCountry.Visible = False
        txtName.Visible = False
        txtAddress1.Visible = False
        txtAddress2.Visible = False
        txtCity.Visible = False
        txtState.Visible = False
        txtZip.Visible = False
        txtCountry.Visible = False
        btnOK.Top = 152
        btnCancel.Top = 152
        frmLVC.Height = 210
    End If
End Sub

'process LVC if OK button is clicked
Private Sub btnOK_Click()
    Dim InList As Boolean
    Dim i As Integer
    Dim TempDate As String
    Dim row As Integer
    'AddAntiRaidComments = False
    
    'Get the SSN and account number from LP22 based on which one we got on the form.
    If Len(txtSSN.Text) = 10 Then
        Binfo.SSN = ""
        Binfo.AcctNo = txtSSN.Text
    Else
        Binfo.SSN = txtSSN.Text
        Binfo.AcctNo = ""
    End If
    SP.Common.GetLP22 Binfo.SSN, Binfo.AcctNo
        
    'verify the payoff date entered is valid and format it MM/DD/YYYY
    TempDate = txtAntPODate.Text
    If Not SP.Common.DateFormat(TempDate) Then
        MsgBox "The Anticipated Payoff Date is not a valid date.  Please reenter the date.", 48, "Invalid Date"
        Exit Sub
    ElseIf DateValue(TempDate) < DateValue(Date) Then
        MsgBox "The Anticipated Payoff Date cannot be in the past.  Please enter a future date.", 48, "Invalid Date"
        Exit Sub
    ElseIf DateValue(TempDate) > DateValue(Date) + 365 Then
        MsgBox "The Anticipated Payoff Date cannot be more than one year in the future.  Please enter a different future date.", 48, "Invalid Date"
        Exit Sub
    End If
    txtAntPODate.Text = TempDate
    
    'If Federal Direct, verify the app receipt date is valid and format it MM/DD/YYYY
    TempDate = txtAppRectDate.Text
    If Not txtAppRectDate.Locked Then
        If Not SP.Common.DateFormat(TempDate) Then
            MsgBox "The app receipt date is not a valid date. Please reenter the date.", vbOKOnly, "Invalid Date"
            Exit Sub
        End If
    End If
    txtAppRectDate.Text = TempDate
    
    'If using an agency not in the database, verify that the user has entered the required address info.
    If cmbAgency.Text = "other" Then
        If txtName.Text = "" Or txtAddress1.Text = "" Or txtCity.Text = "" Or txtZip.Text = "" Then
            MsgBox "At a minimum, the lender's name, address line 1, city, and zip/postal code must be filled in.", vbExclamation + vbOKOnly, "Missing address information"
            Exit Sub
        End If
    ElseIf cmbAgency.Text = "Federal Direct" Then
        If txtFedDirCd.TextLength = 0 Then
            MsgBox "If Federal Direct is the consolidating lender then you must provide a Federal Direct Code.", vbExclamation + vbOKOnly, "Missing address information"
            Exit Sub
        End If
    End If
    
    'get borrower demographics and warn user if SSN is invalid
    If Not SP.Common.GetTX1J(Binfo.SSN, Binfo.AcctNo, Binfo.lastName, Binfo.MI, Binfo.firstName, Binfo.address1, Binfo.address2, , Binfo.City, Binfo.State, Binfo.ZIP, Binfo.country, , Binfo.Phone) Then
        MsgBox "The borrower was not found on COMPASS.  Please reenter the SSN or Account Number.", 48, "Invalid SSN or Account Number"
        Exit Sub
    End If
    
    RE1 = ""
    RE2 = ""
    RE3 = ""
    RE4 = ""
    RE5 = ""
    RE6 = ""
    RE7 = ""
    RE8 = ""
    RE9 = ""
    RE10 = ""
    RE11 = ""
    RE12 = ""
    RE13 = ""
    Re14 = ""
    Re15 = ""
    Re16 = ""
    
    'hide form
    Me.Hide
    
    If MsgBox("Was a valid lender name and an ID that starts with an ""8"" provided on the LVC?", vbYesNo, "LVC") = vbNo Then
        'No
        DeniedReason = "LVC denied, Valid lender name or lender ID is missing from LVC" 'reason 9 on the letter
        RE9 = "X"
        CreatePrintRecords "Deny"
        Exit Sub
    End If
    frmLVC2.rad1.Value = False
    frmLVC2.rad2.Value = False
    frmLVC2.rad3.Value = False
    frmLVC2.rad4.Value = False
    FastPath "TX3Z/ITD2A" & Binfo.SSN
    PutText 21, 16, Format(Date - 90, "MMDDYY"), "ENTER"
    frmLVC2.Show
    
    If frmLVC2.rad2.Value = True Or frmLVC2.rad4.Value = True Then
        'If user selects either Do not process, store the denial reason(s) and go to the Create Print Records Subroutine
        If frmLVC2.rad2.Value = True Then
            DeniedReason = "LVC denied, LVC was received w/in 90 days" 'reason 8 on the letter
            RE8 = "X"
        End If
        If frmLVC2.rad4.Value = True Then
            If DeniedReason <> "" Then
                DeniedReason = DeniedReason & ", "
            Else
                DeniedReason = "LVC denied, "
            End If
            DeniedReason = DeniedReason & "LVC was received while in-school" 'reason 11 on the letter
            RE11 = "X"
        End If
        CreatePrintRecords "Deny"
    ElseIf frmLVC2.rad1.Value = True Or frmLVC2.rad3.Value = True Then
        'If user selects either Process options, go to the Process LVC step
    
        'add space to separate MI from last name and format phone number
        If Binfo.MI <> "_" Then Binfo.MI = Binfo.MI & " " Else Binfo.MI = ""
        If Len(Binfo.Phone) = 10 Then Binfo.Phone = Format(Binfo.Phone, "(###) ###-####")
        
        'determine if the agency entered is in the list file
        InList = False
        For i = 1 To UBound(LndrList)
            If cmbAgency.Text = LndrList(i) Then
                InList = True
                Exit For
            End If
        Next i
        
        'add the agency to the list file if it isn't there already
        If Not InList Then
            Open DocFolder & "Compass LVC Lender List.txt" For Append As #1
            Write #1, cmbAgency.Text
            Close #1
        End If
        
        'do ARC Check
        ProofOfAppSgDtRequired = IsProofOfAppSignedRequired()
        
        'warn the user if the LVC is a duplicate
        If VerifiedDuplicateLVC Then
            MsgBox "The LVC is a duplicate of an LVC already processed today for the borrower.", 48, "Duplicate LVC"
            AddActRec "duplicate lvc already received and completed the same day"
            Exit Sub
        End If
        
        'get the autopay BB reduction rate
        'select the rate information record for on TSC0
        'FastPath "TX3Z/ITSC0;"
        'PutText 8, 36, "828476"
        'PutText 9, 36, "SUBCNS", "ENTER"
        'find the rate for beg and first disb date = 05/01/06
        'If Check4Text(1, 72, "TSXC2") Then
            'row = 7
            'Do While Not Check4Text(23, 2, "90007")
                'get the rate if the beg and first disb date = 05/01/06
                'If Check4Text(row, 41, "05/01/06") And Check4Text(row, 61, "05/01/06") Then
                    'PutText 21, 17, GetText(row, 2, 2), "ENTER"
                    'Binfo.BBRate = GetText(16, 19, 6)
                    'Exit Do
                'End If
                'row = row + 1
                
                'go to the next record
                'If Check4Text(row, 3, " ") Then
                    'Hit "F8"
                    'row = 7
                'End If
            'Loop
        'End If
        
        CompassLoanInfo
    End If
    If ProofOfAppSgDtRequired Then
        MsgBox "Remember to send the Proof of Application Signed letter."
    End If
    If cmbAgency.Text = "other" Then
        MsgBox "If " & txtName.Text & " should be a permanent part of the lender list, please submit a DCR to have it added to the database.", vbInformation + vbOKOnly
    End If
End Sub

'end script if cancel button is clicked
Private Sub btnCancel_Click()
    End
End Sub

'determine if the LVC is a duplicate
Function VerifiedDuplicateLVC() As Boolean
    VerifiedDuplicateLVC = True
    
    'check TD2A for fed dir and FFELP ARCs
    If CheckITD2A("DFDLC") Then Exit Function
    If CheckITD2A("DSLVC") Then Exit Function
    
    'access LP50 records for SSN, action codes, and today's date
    FastPath "LP50I" & Binfo.SSN
    PutText 9, 20, "MDSE1MDSE2"
    PutText 18, 29, Format(Date, "MMDDYYYY") & Format(Date, "MMDDYYYY"), "ENTER"
    
    'review the records if there are any
    If Not Check4Text(1, 68, "ACTIVITY MENU") Then
        'select all records if there are more than one
        If Check4Text(1, 58, "ACTIVITY SUMMARY SELECT") Then PutText 3, 13, "X", "ENTER"
        
        'review each record
        While Not Check4Text(22, 3, "46004")
            'exit function if a duplicate record found
            If Session.FindText("AGENCY = " & UCase(cmbAgency.Text), 13, 2) Or Session.FindText("COMPLETED LVC FOR " & UCase(cmbAgency.Text), 17, 2) Then Exit Function
            
            'go to the next record
            Hit "F8"
        Wend
    End If
    
    VerifiedDuplicateLVC = False
End Function

'check TD2A for the ARC indicated
Function CheckITD2A(ARC As String) As Boolean
    CheckITD2A = True
    
    'access TD2A records for the SSN, ARC, and today's date
    FastPath "TX3Z/ITD2A" & Binfo.SSN
    PutText 11, 65, ARC
    PutText 21, 16, Format(Date, "MMDDYY"), "ENTER"
    
    'review all records if there is are any
    If Not Check4Text(1, 72, "TDX2B") Then
        'select all records if there are more than one
        If Check4Text(1, 72, "TDX2C") Then PutText 5, 14, "X", "ENTER"
        
        'review each record
        While Not Check4Text(23, 2, "90007")
            'exit function if a duplicate record found
            If Session.FindText("AGENCY = " & UCase(cmbAgency.Text), 17, 2) Or Session.FindText("COMPLETED LVC FOR " & UCase(cmbAgency.Text), 17, 2) Then Exit Function
            
            'go to the next record
            Hit "F8"
            If Check4Text(23, 2, "01033") Then
                Hit "ENTER"
                If Check4Text(1, 72, "TDX2C") Then PutText 5, 14, "X", "ENTER"
            End If
        Wend
    End If
    
    CheckITD2A = False
End Function

'get loan info from COMPASS
Sub CompassLoanInfo()
    Dim row As Integer
    Dim i As Integer
    
    SP.QL.ToCO
    
    ReDim aLD(0) As LoanInfo
    'access ITS26
    FastPath "TX3Z/ITS26" & Binfo.SSN
    
    'review each loan if the selection screen is displayed
    If Check4Text(1, 72, "TSX28") Then
        row = 8
        While Not Check4Text(23, 2, "90007")
            TS26 row
            row = row + 1
        
            If Check4Text(row, 3, " ") Then
                Hit "F8"
                row = 8
            End If
        Wend
    'review the loan if the target is displayed
    ElseIf Check4Text(1, 72, "TSX29") Then
        TS26 0
    End If
    
    'review for denial
    Dim IsDeath As Boolean
    Dim IsDisabled As Boolean
    Dim IsClaim As Boolean
    Dim IsNotElig As Boolean
    Dim IsInSch As Boolean
    Dim IsPIF As Boolean
    Dim IsDeConv As Boolean
    Dim Isconsol As Boolean
    Dim PendingApp As Boolean
    Dim IsElig As Boolean
    Dim IsSubmitted As Boolean
    Dim IsNotFullyDisb As Boolean
    Dim HasEligConsol As Boolean
    Dim IsCNSLDStopPursuit As Boolean
    Dim HasFederalDirectDateExemption As Boolean
    
    IsDeath = False
    IsDisabled = False
    IsClaim = False
    IsNotElig = True
    IsInSch = False
    IsPIF = False
    IsDeConv = False
    Isconsol = True
    PendingApp = False
    IsElig = False
    IsSubmitted = False
    IsNotFullyDisb = False
    DeniedReason = "LVC denied"
    HasEligConsol = False
    IsCNSLDStopPursuit = False
    HasFederalDirectDateExemption = False
    
    'review loans to determine statuses for denial
'    If PendingApplicationDenial(Binfo.SSN) Then
'        If PendingApp = False Then DeniedReason = DeniedReason & ", Pending Consolidation Application"
'        PendingApp = True
'        For i = 1 To UBound(aLD)
'            aLD(i).IsElig = False
'        Next i
'        RE12 = "X"
'    End If
            
    For i = 1 To UBound(aLD)
        If aLD(i).IsElig = False And aLD(i).AcntSta <> "DECONVERTED" Then  'Paid in full
            If IsPIF = False Then DeniedReason = DeniedReason & ", loans paid in full"
            IsPIF = True
            aLD(i).IsElig = False
            Re15 = "X"
        End If
        
        Select Case aLD(i).AcntSta
            Case "CLAIM PAID"
                If IsClaim = False Then DeniedReason = DeniedReason & ", loans previously held are now in default, loans held by guarantor"
                IsClaim = True
                aLD(i).IsElig = False
                RE2 = "X"
            Case "CLAIM PENDING"
                DeniedReason = DeniedReason & ", loans are over 270 days past due and a claim is pending."
                aLD(i).IsElig = False
                RE7 = "X"
            Case "DECONVERTED"
                If (aLD(i).TransDate < "09/23/2009") Then
                    DeniedReason = DeniedReason & ", loans transferred to Nelnet"
                    RE3 = "X"
                ElseIf (aLD(i).TransDate >= "09/23/2009") Then
                    DeniedReason = DeniedReason & ", Loans were transferred to FedLoan Servicing"
                    RE3 = "X"
                End If
                IsNotFullyDisb = True
                aLD(i).IsElig = False
            'Case "NOT FULLY DISBURSED"
                'If IsNotFullyDisb = False Then DeniedReason = DeniedReason & ", loans not fully disbursed"
                'IsNotFullyDisb = True
                'aLD(i).IsElig = False
                'RE5 = "X"
            Case "IN SCHOOL"
                'Check the app receipt date for federal direct loans.
                If IsDate(txtAppRectDate.Text) Then
                    If DateValue(txtAppRectDate.Text) >= DateValue("7/1/2010") And DateValue(txtAppRectDate.Text) < DateValue("7/1/2011") Then
                        HasFederalDirectDateExemption = True
                    Else
                        HasFederalDirectDateExemption = False
                    End If
                Else
                    HasFederalDirectDateExemption = False
                End If
                If Not HasFederalDirectDateExemption Then
                    If IsInSch = False Then DeniedReason = DeniedReason & ", loans are not in grace or repayment"
                    IsInSch = True
                    aLD(i).IsElig = False
                    RE6 = "X"
                End If
            Case "CLAIM SUBMITTED"
                If IsSubmitted = False Then DeniedReason = DeniedReason & ", previously held loans are more than 270 days delinquent and a claim has been filed"
                IsSubmitted = True
                aLD(i).IsElig = False
                RE7 = "X"
            Case "VERIFIED DISABILITY"
                If IsDisabled = False Then DeniedReason = DeniedReason & ", loans currently in disability status"
                IsDisabled = True
                aLD(i).IsElig = False
                RE13 = "X"
            Case "VERIFIED DEATH"
                If IsDeath = False Then DeniedReason = DeniedReason & ", loans currently in death status"
                IsDeath = True
                aLD(i).IsElig = False
                Re14 = "X"
            Case "PAID IN FULL"
                If IsPIF = False Then DeniedReason = DeniedReason & ", loans paid in full"
                IsPIF = True
                aLD(i).IsElig = False
                Re15 = "X"
            Case "CNSLD-STOP PURSUIT"
                If IsCNSLDStopPursuit = False Then DeniedReason = DeniedReason & ", loans already consolidated"
                IsCNSLDStopPursuit = True
                aLD(i).IsElig = False
                Re16 = "X"
        End Select
        
        If (aLD(i).LnTyp <> "CNSLDN" And _
            aLD(i).LnTyp <> "SUBCNS" And _
            aLD(i).LnTyp <> "UNCNS" And _
            aLD(i).LnTyp <> "SUBSPC" And _
            aLD(i).LnTyp <> "UNSPC") And aLD(i).IsElig = True Then
            Isconsol = False
        Else
            If (aLD(i).LnTyp = "CNSLDN" Or _
                aLD(i).LnTyp = "SUBCNS" Or _
                aLD(i).LnTyp = "UNCNS" Or _
                aLD(i).LnTyp = "SUBSPC" Or _
                aLD(i).LnTyp = "UNSPC") And aLD(i).IsElig = True Then
                HasEligConsol = True
            Else
                aLD(i).IsElig = False
            End If
        End If
            
        If aLD(i).IsElig Then
            IsNotElig = False
        End If
        
        If aLD(i).FullDisb And aLD(i).IsElig Then
            IsElig = True
        End If
    Next i
        
    'deny LVCs
    If Isconsol And HasEligConsol And cmbAgency.Text <> "UHEAA" And cmbAgency.Text <> "Federal Direct" Then
        If MsgBox("Has proof of eligible loan been received?", vbYesNo) = vbNo Then
            'deny
            IsNotElig = True
            For i = 1 To UBound(aLD)
                aLD(i).IsElig = False
            Next i
            DeniedReason = Replace(DeniedReason, ", loans transferred to Nelnet", "")
            RE3 = ""
            DeniedReason = Replace(DeniedReason, ", Pending Consolidation Application", "")
            RE12 = ""
            DeniedReason = Replace(DeniedReason, ", loans paid in full", "")
            Re15 = ""
            DeniedReason = DeniedReason & ", only loans held are consolidation loans"
            RE10 = "X"
        End If
    End If
    
    If DeniedReason = "LVC denied" Then
        DeniedReason = ""
    End If

    If IsNotElig = False Then
        'set fully disb = True (PO1N will change it if necessary)
        For i = 1 To UBound(aLD)
            aLD(i).FullDisb = True
        Next i
        
'        FastPath "TX3Z/IPO1N" & Binfo.SSN
'        'review each loan if on selection screen
'        If Check4Text(1, 75, "POX1P") Then
'            row = 9
'            While Not Check4Text(22, 2, "90007")
'                PutText 21, 16, GetText(row, 2, 2), "ENTER"
'                PO1N
'                Hit "F12"
'                row = row + 1
'
'                'go to the next page
'                If Check4Text(row, 3, " ") Then
'                    Hit "F8"
'                    row = 9
'                End If
'            Wend
'        'review the loan if only one
'        ElseIf Check4Text(1, 75, "POX1S") Then
'            PO1N
'        End If
        
        'determine the subroutine to go to if a fully disbursed loan with a balance is found
        For i = 1 To UBound(aLD)
            If aLD(i).FullDisb And aLD(i).IsElig Then
                LVCInformation
                CreatePrintRecords "AP"
                Exit Sub
            End If
        Next i
    Else
        CreatePrintRecords
    End If
End Sub

'Function PendingApplicationDenial(SSN As String) As Boolean
'    Dim X As Integer
'    Dim found As Boolean
'    SP.QL.ToLCO
'    SP.q.FastPath "TOMD" & SSN
'    found = False
'    X = 11
'    Do
'        If SP.q.GetText(X, 4, 1) <> "_" Then
'            PendingApplicationDenial = False
'            Exit Do
'        End If
'        SP.q.PutText X, 4, "X", "ENTER"
'        SP.q.Hit "UP"
'        If SP.q.GetText(13, 23, 10) = "" Or SP.q.GetText(18, 23, 10) = "MM DD YYYY" Then
'            'If Date App Rcvd is not populated continue to determine fully disbursed status step
'            PendingApplicationDenial = False
'        Else
'            If SP.q.GetText(5, 13, 2) <> "" And SP.q.GetText(5, 13, 2) <> "H5" Then
'                'If Date App Rcvd is populated and the Acct Stat <> blank or H5 continue to determine fully disbursed status step
'                PendingApplicationDenial = False
'                SP.q.Hit "F12"
'            Else
'                'If Date App Rcvd is populated, Acct Status = blank or H5, denial reason = 'Pending Consolidation Application'
'                SP.q.Hit "UP"
'                SP.q.Hit "UP"
'                If SP.q.Check4Text(18, 26, "        ") Or SP.q.Check4Text(18, 26, "APPLSEND") Then
'                    PendingApplicationDenial = True
'                    Exit Do
'                Else
'                    SP.q.Hit "F12"
'                End If
'            End If
'        End If
'        X = X + 1
'    Loop
'    SP.QL.ToCO
'End Function

'create tasks to sell loans
'Sub LoanSale()
'    Dim aSL() As Integer
'    Dim i As Integer
'
'    'add loans to the aSL array if the creditor is not "828476" and the loan is fully disb and is eligible
'    ReDim aSL(0) As Integer
'    For i = 1 To UBound(aLD)
'        If aLD(i).Owner <> "828476" And aLD(i).FullDisb And aLD(i).IsElig Then
'            ReDim Preserve aSL(UBound(aSL) + 1) As Integer
'            aSL(UBound(aSL)) = Val(aLD(i).LnSeq)
'        End If
'    Next i
'
'    'add an ARC and warn the user if there are loans to be sold
'    If UBound(aSL) <> 0 And cmbAgency.Text = "UHEAA" Then
'        ATD22ByLoan Binfo.SSN, "SLCON", "", aSL(), "COMPASSLVC", UserId
'        MsgBox "An action request has been created to sell loans not already owned by UHEAA.  Please set the LVC aside and rerun the script after the loan sale is complete.", 48, "Loans to be Sold"
'    'continue processing if there are no loans to sell
'    Else
'        LVCInformation
'    End If
'End Sub

'get information
Sub LVCInformation()
    Dim row As Integer
    Dim i As Integer
    
    'access TS06 and get information
    FastPath "TX3Z/ITS06" & Binfo.SSN
    If Check4Text(1, 72, "TSX05") Then
        row = 8
        While Not Check4Text(23, 2, "90007")
            For i = 1 To UBound(aLD)
                If Check4Text(row, 47, aLD(i).LnSeq) Then
                    PutText 21, 18, Format(GetText(row, 2, 3), "0#"), "ENTER"
                    TS06 i
                    Hit "F12"
                    Exit For
                End If
            Next i
            row = row + 1
            If Check4Text(row, 4, " ") Then
                Hit "F8"
                row = 8
            End If
        Wend
    ElseIf Check4Text(1, 71, "TSX07") Then
        TS06 1
    End If
    
    'get first due for repayment loans
    For i = 1 To UBound(aLD)
        If aLD(i).Status = "RP" And aLD(i).FullDisb And aLD(i).IsElig Then
            FastPath "TX3Z/ITD0L" & Binfo.SSN
            'find the loan
            Do Until Check4Text(9, 2, aLD(i).LnSeq) Or _
                     Check4Text(10, 2, aLD(i).LnSeq) Or _
                     Check4Text(11, 2, aLD(i).LnSeq)
                If Check4Text(7, 78, "+") Then
                    Hit "F8"
                ElseIf Check4Text(13, 78, "+") Then
                    Hit "F6"
                    Hit "F8"
                    Hit "F6"
                Else
                    Exit Do
                End If
            Loop
            'get the first due date if the loan and date are displayed, otherwise use the current first due from ITS26
            If (Check4Text(9, 2, aLD(i).LnSeq) Or _
                     Check4Text(10, 2, aLD(i).LnSeq) Or _
                     Check4Text(11, 2, aLD(i).LnSeq)) And _
                    Not Check4Text(16, 35, " ") Then
                aLD(i).FirstDue = GetText(16, 35, 8)
            Else
                aLD(i).FirstDue = aLD(i).CurrFirstDue
            End If
        End If
    Next i
    'get payoff info
    RetrievePayoff
End Sub

'get payoff info
Sub RetrievePayoff()
    Dim row As Integer
    Dim i As Integer

    'access TS2O
    FastPath "TX3Z/ITS2O" & Binfo.SSN
    PutText 7, 26, Format(txtAntPODate.Text, "MMDDYY")
    PutText 9, 54, "Y", "ENTER"
    
    'select loans and get info
    row = 13
    While Not Check4Text(23, 2, "90007")
        For i = 1 To UBound(aLD)
            If Check4Text(row, 16, aLD(i).LnSeq) Then
                'select loan and get payoff amounts if the loan is eligible for consolidation
                If aLD(i).FullDisb And aLD(i).IsElig Then
                    PutText row, 2, "X", "ENTER"
                    Hit "ENTER"
                    aLD(i).TotalAmt = GetText(12, 29, 10)
                    aLD(i).PrincAmt = GetText(14, 29, 10)
                    aLD(i).IntAmt = GetText(17, 29, 10)
                    aLD(i).PerDiem = GetText(17, 29, 10)
        
                    'return to the selection screen and blank the selection field
                    Hit "F12"
                    PutText row, 2, " "
                'set the payoff amounts to 0 if the loan is not eligible for consolidation
                Else
                    aLD(i).TotalAmt = "0"
                    aLD(i).PrincAmt = "0"
                    aLD(i).IntAmt = "0"
                    aLD(i).PerDiem = "0"
                End If
                Exit For
            End If
        Next i
        row = row + 1
        
        'go to the next page
        If Check4Text(row, 2, " ") Or row > 22 Then
            Hit "ENTER"
            Hit "F8"
            row = 13
        End If
    Wend
End Sub

'get interest rate
Function TS06(i As Integer)
    Dim row As Integer
    Dim tDate As Double
    
    'get current date
    tDate = DateValue(Date)
    
    row = 11
    While Not Check4Text(23, 2, "90007")
        'get the rate if it is effective for the current date
        If DateValue(GetText(row, 21, 10)) <= tDate And DateValue(GetText(row, 33, 10)) >= tDate And Check4Text(row, 50, "A") Then
            aLD(i).Rate = GetText(row, 73, 7)
            Exit Function
        End If
        row = row + 1
        'go to the next page
        If Check4Text(row, 3, " ") Then
            Hit "F8"
            row = 11
        End If
    Wend
End Function

'get Compass info
Function TS26(row As Integer)
     Dim Ln As Integer
    
    'set the loan counter
    ReDim Preserve aLD(UBound(aLD) + 1) As LoanInfo
    
    Ln = UBound(aLD)

    'select the loan if the selection screen is displayed
    If row > 0 Then PutText 21, 12, Format(GetText(row, 2, 2), "0#"), "ENTER"
    
    'get the information
    aLD(Ln).AcntSta = GetText(3, 10, 20)
    If aLD(Ln).AcntSta = "DECONVERTED" Then
        Hit "ENTER"
        Hit "ENTER"
        aLD(Ln).TransDate = GetText(19, 16, 8)
        Hit "F12"
        Hit "F12"
    End If

    aLD(Ln).LnSeq = GetText(7, 36, 3)
    aLD(Ln).OwnerName = Replace(Trim(GetText(7, 59, 20)), ",", "")
    aLD(Ln).Owner = GetText(7, 48, 6)
    aLD(Ln).LnTyp = GetText(6, 66, 6)
    aLD(Ln).disbDate = GetText(6, 18, 8)
    aLD(Ln).Prin = Replace(GetText(11, 12, 10), ",", "")
    If Check4Text(11, 22, "CR") Then aLD(Ln).Prin = aLD(Ln).Prin * -1
    aLD(Ln).GrcEndDt = GetText(16, 45, 8)
    If Not Check4Text(17, 44, " ") Then aLD(Ln).FirstDue = Format(DateAdd("m", 1, GetText(17, 44, 8)), "MM/DD/YY")
    aLD(Ln).CurrFirstDue = GetText(18, 71, 8)
    aLD(Ln).OosDate = GetText(18, 17, 8)
    aLD(Ln).School = GetText(13, 18, 8)
    
    'set the in-school status and loan status code
    If aLD(Ln).Prin > 0 Then aLD(Ln).IsElig = True Else aLD(Ln).IsElig = False
    If aLD(Ln).LnTyp = "TILP" Then aLD(Ln).IsElig = False
    Select Case aLD(Ln).AcntSta
        Case "IN GRACE"
            aLD(Ln).Status = "GR"
        Case "IN SCHOOL"
            aLD(Ln).Status = "IS"
        Case "REPAYMENT"
            aLD(Ln).Status = "RP"
        Case "DEFERMENT"
            aLD(Ln).Status = "DE"
        Case "FORBEARANCE"
            aLD(Ln).Status = "FB"
        Case "VERIFIED BANKRUPTCY"
            aLD(Ln).Status = "BA"
        Case "PRE-CLAIM SUBMITTED"
            aLD(Ln).Status = "AV"
        Case Else
            aLD(Ln).IsElig = False
    End Select
    
    'add "UHEAA" to owners other than UHEAA
    If aLD(Ln).OwnerName <> "UHEAA" Then
        aLD(Ln).OwnerName = aLD(Ln).OwnerName & "/UHEAA"
    End If
    
    'set loan code for FFELP and loan type code
    Select Case aLD(Ln).LnTyp
        Case "STFFRD"
            aLD(Ln).LnCd = "SS"
            aLD(Ln).LnTypCd = "A"
        Case "UNSTFD"
            aLD(Ln).LnCd = "US"
            aLD(Ln).LnTypCd = "G"
        Case "PLUS"
            aLD(Ln).LnCd = "PLUS"
            aLD(Ln).LnTypCd = "S"
        Case "SUBCNS"
            aLD(Ln).LnCd = "SCON"
            aLD(Ln).LnTypCd = "O"
        Case "UNCNS"
            aLD(Ln).LnCd = "UCON"
            aLD(Ln).LnTypCd = "J"
        Case "SUBSPC"
            aLD(Ln).LnCd = "SCON"
            aLD(Ln).LnTypCd = "O"
        Case "UNSPC"
            aLD(Ln).LnCd = "UCON"
            aLD(Ln).LnTypCd = "J"
    End Select

    'set "account number" for the loan
    aLD(Ln).AcntNo = Binfo.SSN & "0" & aLD(Ln).LnSeq

    'get first due date for deferment and forbearance loans
    If aLD(Ln).Status = "DE" Or aLD(Ln).Status = "FB" Then
        Hit "F2"
        Hit "F7"
        If Check4Text(1, 72, "TSX31") Then
            aLD(Ln).FirstDue = Format(DateValue(Replace(GetText(12, 30, 8), " ", "/")) + 30, "MM/DD/YY")
            Hit "F12"
        End If
        Hit "F2"
    End If
    
    'get the clid from page 2
    Hit "ENTER"
    aLD(Ln).CLID = GetText(8, 17, 17) & Format(GetText(8, 58, 2), "0#")

    'return to the TX26 selection screen
    Hit "F12"
    Hit "F12"
End Function

'determine if loan is fully disbursed
Function PO1N()
    Dim col As Integer
    Dim i As Integer
    
    PutText 2, 78, "DI", "ENTER"
    For i = 1 To UBound(aLD)
        If GetText(5, 20, 17) & GetText(5, 38, 2) = aLD(i).CLID Then
            col = 30
            Do While Not Check4Text(22, 2, "90007")
                'loan is not fully disbursed if status is anticipated and canceled amount does not equal disursed amount
                If Check4Text(8, col, "NTICIPATED") And GetText(10, col, 10) <> GetText(12, col, 10) Then
                    aLD(i).FullDisb = False
                    Exit Do
                End If
                col = col + 20
                
                'go to the next page
                If col > 70 Then
                    Hit "F8"
                    col = 30
                End If
            Loop
            Exit For
        End If
    Next i
End Function

'print LVC and denied letters
Function CreatePrintRecords(Optional ProcOpts As String = "")
    Dim i As Integer
    Dim M As Integer
    Dim j As Integer
    Dim InSchAddWarningMsg As Boolean
    Dim ConsolAddWarningMsg As Boolean
    Dim WarningMsg As String
    Dim CommentText As String
    Dim AdditionalComments As String
    Dim AgencyName As String
    Dim AgencyAddress1 As String
    Dim AgencyAddress2 As String
    Dim AgencyCity As String
    Dim AgencyState As String
    Dim AgencyZip As String
    Dim AgencyCountry As String
    Dim tempProtectSSN As String
    
    ConsolAddWarningMsg = True
    InSchAddWarningMsg = False
    
    'Set the lending agency's address from the form's text boxes if it's not in BSYS, or from BSYS otherwise.
    If cmbAgency.Text = "other" Then
        AgencyName = txtName.Text
        AgencyAddress1 = txtAddress1.Text
        AgencyAddress2 = txtAddress2.Text
        AgencyCity = txtCity.Text
        AgencyState = txtState.Text
        AgencyZip = txtZip.Text
        AgencyCountry = txtCountry.Text
    Else
        Dim Agency() As String
        Agency() = SP.Common.SQLEx("SELECT * FROM GENR_LST_Lenders WHERE Name = '" & cmbAgency.Text & "'")
        AgencyName = Agency(0, 0)
        AgencyAddress1 = Agency(1, 0)
        AgencyAddress2 = Agency(2, 0)
        AgencyCity = Agency(3, 0)
        AgencyState = Agency(4, 0)
        AgencyZip = Agency(5, 0)
        AgencyCountry = Agency(6, 0)
    End If
    
    'remove internal commas from borrower data
    Binfo.AcctNo = Replace(Binfo.AcctNo, ",", " ")
    Binfo.address1 = Replace(Binfo.address1, ",", " ")
    Binfo.address2 = Replace(Binfo.address2, ",", " ")
    Binfo.BBRate = Replace(Binfo.BBRate, ",", " ")
    Binfo.City = Replace(Binfo.City, ",", " ")
    Binfo.country = Replace(Binfo.country, ",", " ")
    Binfo.firstName = Replace(Binfo.firstName, ",", " ")
    Binfo.lastName = Replace(Binfo.lastName, ",", " ")
    Binfo.MI = Replace(Binfo.MI, ",", " ")
    Binfo.Phone = Replace(Binfo.Phone, ",", " ")
    Binfo.State = Replace(Binfo.State, ",", " ")
    Binfo.ZIP = Replace(Binfo.ZIP, ",", " ")
    
    'create print record for approved LVCs
    If ProcOpts = "AP" Then
        'sort loan data array by owner
        For M = 1 To UBound(aLD) - 1
            For j = M + 1 To UBound(aLD)
                If aLD(M).Owner > aLD(j).Owner Then
                    taLD = aLD(j)
                    aLD(j) = aLD(M)
                    aLD(M) = taLD
                End If
            Next j
        Next M
        
        'check for only consolidation loan types or any in school status
        For i = 1 To UBound(aLD)
            If aLD(i).FullDisb And aLD(i).IsElig Then
                If aLD(i).LnCd <> "SCON" And aLD(i).LnCd <> "UCON" Then
                    ConsolAddWarningMsg = False
                End If
                If aLD(i).Status = "IS" Then
                    InSchAddWarningMsg = True
                End If
            End If
        Next
        'If any loans were denied and loans were approved add the denied loans to the LVC
        Dim LoansApproved As Boolean
        Dim LoansDenied As Boolean
        LoansApproved = False
        LoansDenied = False
        For i = 1 To UBound(aLD)
            If aLD(i).FullDisb And aLD(i).IsElig Then
                LoansApproved = True
            Else
                LoansDenied = True
            End If
        Next i
        
        If LoansApproved Then
            Dim DataFile As String
            Dim letterId As String
            If txtFedDirCd.Text = "" Then
                DataFile = "T:\LVCFFELPapproved.txt"
                letterId = "LVCFLCO"
            Else
                DataFile = "T:\LVCFDapproved.txt"
                letterId = "LVCFDCO"
            End If
            AdditionalComments = ""
            WarningMsg = ""
            Open DataFile For Output As #1
            'create a print record for each loan that fully disbursed and eligible
            For i = 1 To UBound(aLD)
                If aLD(i).FullDisb And aLD(i).IsElig Then
                    Write #1, AgencyName, aLD(i).Owner, aLD(i).OwnerName, txtAntPODate.Text, AdditionalComments, txtFedDirCd.Text, _
                        Format(Binfo.SSN, "@@@@@@@@@"), Binfo.firstName, Binfo.MI, Binfo.lastName, _
                        Binfo.address1, Binfo.address2, Binfo.City, Binfo.State, Binfo.ZIP, Binfo.country, Binfo.Phone, _
                        "", WarningMsg, Format(Date, "MM/DD/YYYY"), aLD(i).OwnerName, aLD(i).Owner, _
                        aLD(i).AcntNo, aLD(i).LnTypCd, aLD(i).Rate, Replace(aLD(i).PrincAmt, ",", ""), _
                        Replace(aLD(i).IntAmt, ",", ""), Replace(aLD(i).TotalAmt, ",", ""), aLD(i).disbDate, "No", _
                        aLD(i).School, aLD(i).OosDate, "No", aLD(i).LnCd, "UHEAA", aLD(i).Status, _
                        aLD(i).GrcEndDt, aLD(i).FirstDue, aLD(i).PerDiem
                ElseIf aLD(i).LnTyp <> "TILP" Then  'Add only non-TILP loans
                    Write #1, AgencyName, aLD(i).Owner, aLD(i).OwnerName, txtAntPODate.Text, "", txtFedDirCd.Text, _
                        Format(Binfo.SSN, "@@@@@@@@@"), Binfo.firstName, Binfo.MI, Binfo.lastName, _
                        Binfo.address1, Binfo.address2, Binfo.City, Binfo.State, Binfo.ZIP, Binfo.country, Binfo.Phone, _
                        "", "", Format(Date, "MM/DD/YYYY"), aLD(i).OwnerName, aLD(i).Owner, _
                        Binfo.SSN & "0" & aLD(i).LnSeq, aLD(i).LnTypCd, "", "0.00", _
                        "0", "0", aLD(i).disbDate, "", _
                        "", "", "", "", "", "", _
                        "", "", ""
                End If
            Next i
            Close #1
            PrintApprovedLVCs DataFile, letterId
        End If
        
        If LoansDenied Then
            Open "T:\Denialletter.txt" For Output As #1
                
                tempProtectSSN = Binfo.SSN
                Write #1, "Lender", "Address1", "Address2", "City", "State", "Zip", "Country", "SS_N", "BorName", "Re1", "Re2", "Re3", "Re4", "Re5", "Re6", "Re7", "Re8", "Re9", "Re10", "Re11", "Re12", "Re13", "Re14", "Re15", "Re16", "AN", "ForeignInd", "COST_CENTER_CODE", "SSN"
                Write #1, AgencyName, AgencyAddress1, AgencyAddress2, AgencyCity, AgencyState, AgencyZip, AgencyCountry, Binfo.SSN, Binfo.firstName & " " & Binfo.lastName, RE1, RE2, RE3, RE4, RE5, RE6, RE7, RE8, RE9, RE10, RE11, RE12, RE13, Re14, Re15, Re16, Binfo.AcctNo, "UT", "MA2324", protectSSN(tempProtectSSN)
            Close #1
            SP.Common.ImageDocs 19, "LSLVC", LetterFolder, "CNSLDNRS", SP.DocCreateAndDeploy.AddBarcodeAndStaticCurrentDate("T:\Denialletter.txt", "AN", "CNSLDNRS", True)
            SP.DocCreateAndDeploy.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt "CNSLDNRS", stacCOMPASS, Binfo.SSN, "T:\Denialletter.txt", "AN", "CNSLDNRS", LetterFolder, UserId, "COMPASSLVC", "State", dmLetter, "05", lrOther
        End If
        
        'add an activity record
        AddApvActRec
        If ProofOfAppSgDtRequired = True Then
            CommentText = "Proof of application signed date required."
            AddApvActRec4InEligAndInSch CommentText
        End If

        Exit Function
    Else
        'create print record for denied LVCs
        If ProcOpts <> "Deny" Then
            Dim Isconsol As Boolean
            Dim HasEligConsol As Boolean
            Isconsol = True
            HasEligConsol = False
            
            For i = 1 To UBound(aLD)
                If (aLD(i).LnTyp <> "CNSLDN" And _
                    aLD(i).LnTyp <> "SUBCNS" And _
                    aLD(i).LnTyp <> "UNCNS" And _
                    aLD(i).LnTyp <> "SUBSPC" And _
                    aLD(i).LnTyp <> "UNSPC") And aLD(i).IsElig = True Then
                    Isconsol = False
                Else
                    If (aLD(i).LnTyp = "CNSLDN" Or _
                        aLD(i).LnTyp = "SUBCNS" Or _
                        aLD(i).LnTyp = "UNCNS" Or _
                        aLD(i).LnTyp = "SUBSPC" Or _
                        aLD(i).LnTyp = "UNSPC") And aLD(i).IsElig = True Then
                        HasEligConsol = True
                    Else
                        aLD(i).IsElig = False
                    End If
                End If
            Next i
            
            If Isconsol = False Or HasEligConsol = False Then
                ConsolAddWarningMsg = False
            End If

            If ConsolAddWarningMsg = True Or InSchAddWarningMsg = True Or ProofOfAppSgDtRequired = True Then
                If ConsolAddWarningMsg = True Then
                    CommentText = "Ineligible to consolidate due to sole loan for borrower is a consolidation loan."
                ElseIf InSchAddWarningMsg = True Then
                    CommentText = "Ineligible to consolidate due to loan in an in-school status."
                ElseIf ProofOfAppSgDtRequired = True Then
                    CommentText = "Proof of application signed date required."
                End If
                AddApvActRec4InEligAndInSch CommentText
            End If
        End If
        'Print the denial letter
        Open "T:\Denialletter.txt" For Output As #1
            tempProtectSSN = Binfo.SSN
            Write #1, "Lender", "Address1", "Address2", "City", "State", "Zip", "Country", "SS_N", "BorName", "Re1", "Re2", "Re3", "Re4", "Re5", "Re6", "Re7", "Re8", "Re9", "Re10", "Re11", "Re12", "Re13", "Re14", "Re15", "Re16", "AN", "ForeignInd", "COST_CENTER_CODE", "SSN"
            Write #1, AgencyName, AgencyAddress1, AgencyAddress2, AgencyCity, AgencyState, AgencyZip, AgencyCountry, Binfo.SSN, Binfo.firstName & " " & Binfo.lastName, RE1, RE2, RE3, RE4, RE5, RE6, RE7, RE8, RE9, RE10, RE11, RE12, RE13, Re14, Re15, Re16; Binfo.AcctNo, "UT", "MA2324", protectSSN(tempProtectSSN)
        Close #1
        SP.Common.ImageDocs 19, "LSLVC", LetterFolder, "CNSLDNRS", SP.DocCreateAndDeploy.AddBarcodeAndStaticCurrentDate("T:\Denialletter.txt", "AN", "CNSLDNRS", True)
        SP.DocCreateAndDeploy.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt "CNSLDNRS", stacCOMPASS, Binfo.SSN, "T:\Denialletter.txt", "AN", "CNSLDNRS", LetterFolder, UserId, "COMPASSLVC", "State", dmLetter, "05", lrOther
    End If
    
    'add an activity record
    AddActRec DeniedReason
End Function

'add an activity record for approved LVCs
Function AddApvActRec()
    Dim ARC As String
    Dim row As Integer
    Dim i As Integer
    Dim j As Integer
    Dim l As Integer
    Dim comment As String
    
    If txtFedDirCd <> "" Then ARC = "DFDLC" Else ARC = "DSLVC"
        
    'access TD22
    FastPath "TX3Z/ATD22" & Binfo.SSN
    'find the ARC
    Do
        found = Session.FindText(ARC, 8, 8)
        If found Then Exit Do
        Hit "F8"
        If Check4Text(23, 2, "90007") Then
            MsgBox "You do not have access to the ARC " & ARC & ".  Contact Systems Support to get access to the ARC and then run the script again.", 16, "ARC not Found"
            End
        End If
    Loop
    'select the ARC
    PutText Session.FoundTextRow, Session.FoundTextColumn - 5, "01", "ENTER"
    
    'review each loan for selection
    row = 11
    Do
        'select the loan if it is in the array
        For i = 1 To UBound(aLD)
             If Check4Text(row, 5, aLD(i).LnSeq) And aLD(i).FullDisb And aLD(i).IsElig Then
                PutText row, 3, "X"
                Exit For
            End If
        Next i
        row = row + 1
        'go to the next page
        If Not Check4Text(row, 3, "_") Then
            If Check4Text(8, 75, "+") Then
                Hit "F8"
                row = 11
            Else
                Exit Do
            End If
        End If
    Loop
    'access additional comments
    Hit "ENTER"
    Hit "F4"
    
    'enter comment
    comment = "completed LVC for " & cmbAgency.Text & " for " & txtAntPODate & " " & UserId & "  {COMPASSLVC}"
    j = 79 - Len(comment)
    PutText 8, 5, comment
    'enter spaces to loan info starts on next line on TD2A
    For i = 1 To j
        Session.TransmitANSI " "
    Next i
    'enter loan information
    row = 0
    For i = 1 To UBound(aLD)
        If aLD(i).FullDisb And aLD(i).IsElig Then
            row = row + 1
            Session.TransmitANSI aLD(i).LnSeq
            Session.TransmitANSI ","
            Session.TransmitANSI Format(CDbl(aLD(i).PrincAmt), "000000.00")
            Session.TransmitANSI ","
            Session.TransmitANSI Format(CDbl(aLD(i).IntAmt), "000000.00")
            Session.TransmitANSI ";"
        End If
    Next i
    Hit "ENTER"
    CloseQ
End Function

'add an activity record
Function AddActRec(comment As String)
    Dim ARC As String
    Dim ActCd As String
    
    'set ARC and action code based on type of LVC
    If txtFedDirCd <> "" Then
        ARC = "DFDLC"
        ActCd = "MDSE2"
    Else
        ARC = "DSLVC"
        ActCd = "MDSE1"
    End If
    
    'add agency name to beginning of comment
    comment = "agency = " & cmbAgency.Text & "; " & comment
    'If AddAntiRaidComments Then Comment = Comment & "; anti raid letter sent to borrower"
    'add activity record to TD22 then to TD37 then to LP50 until one of them takes
    If Not SP.Common.ATD22AllLoans(Binfo.SSN, ARC, comment, "COMPASSLVC", UserId) Then
        If Not SP.Common.ATD37FirstLoan(Binfo.SSN, ARC, comment, UserId, "COMPASSLVC") Then
            If Not SP.Common.AddLP50(Binfo.SSN, ActCd, "COMPASSLVC", "AM", "10", comment) Then
                MsgBox ("The script was unable to add a commont to onelink or compass, please make sure a comment is added.")
            End If
        End If
    End If
    CloseQ
End Function

Sub PrintApprovedLVCs(inDat As String, Doc As String)
    Dim CurrentRecord As Integer
    Dim CurrentField As Integer
    Dim BlankLoan As Integer
    Dim xof6 As Integer         'rec no of 6 total for current sheet
    Dim BrwTtl As Double
    Dim InputRecord() As String
    Dim InputField() As String
    Dim NextRecordField() As String
    Dim LastLoan As Boolean
    'Strings to hold data for an output record
    Dim BorrowerFields As String
    Dim LoanFields As String
    LoanFields = ""
    
    ReDim InputRecord(0)
    
    'read records from text file to an array
    Open inDat For Input As #1
    While Not EOF(1)
        ReDim Preserve InputRecord(UBound(InputRecord) + 1) As String
        Line Input #1, InputRecord(UBound(InputRecord))
    Wend
    Close #1
    
    'set up a new data file
    Open "T:\LVCdat.txt" For Output As #2
    Write #2, "AgName", "HldCode", "HldName", "PayOffDt", "AddComs", "BrwFDCode", "BrwSSN", "BrwFN", "BrwMI", "BrwLN", "BrwAdd1", "BrwAdd2", "BrwCity", "BrwST", "BrwZip", "BrwCntry", "BrwPhn", "BrwTtl", "Ineltext", "CurDate", "HLDName", "HLDCode", _
        "LnAcntNo1", "LnTypCd1", "IntRt1", "Prin1", "Int1", "Ttl1", "DsbDt1", "InSch1", "SchCd1", "ComplDt1", "Dflt1", "LnCd1", "GACd1", "BrwSta1", "GrcEnd1", "FirstPmt1", "DlyInt1", _
        "LnAcntNo2", "LnTypCd2", "IntRt2", "Prin2", "Int2", "Ttl2", "DsbDt2", "InSch2", "SchCd2", "ComplDt2", "Dflt2", "LnCd2", "GACd2", "BrwSta2", "GrcEnd2", "FirstPmt2", "DlyInt2", _
        "LnAcntNo3", "LnTypCd3", "IntRt3", "Prin3", "Int3", "Ttl3", "DsbDt3", "InSch3", "SchCd3", "ComplDt3", "Dflt3", "LnCd3", "GACd3", "BrwSta3", "GrcEnd3", "FirstPmt3", "DlyInt3", _
        "LnAcntNo4", "LnTypCd4", "IntRt4", "Prin4", "Int4", "Ttl4", "DsbDt4", "InSch4", "SchCd4", "ComplDt4", "Dflt4", "LnCd4", "GACd4", "BrwSta4", "GrcEnd4", "FirstPmt4", "DlyInt4", _
        "LnAcntNo5", "LnTypCd5", "IntRt5", "Prin5", "Int5", "Ttl5", "DsbDt5", "InSch5", "SchCd5", "ComplDt5", "Dflt5", "LnCd5", "GACd5", "BrwSta5", "GrcEnd5", "FirstPmt5", "DlyInt5", _
        "LnAcntNo6", "LnTypCd6", "IntRt6", "Prin6", "Int6", "Ttl6", "DsbDt6", "InSch6", "SchCd6", "ComplDt6", "Dflt6", "LnCd6", "GACd6", "BrwSta6", "GrcEnd6", "FirstPmt6", "DlyInt6"
    xof6 = 0        'rec no of 6
      
    'print LVCs
    For CurrentRecord = 1 To UBound(InputRecord)
        'increment record counter and parse data from line
        xof6 = xof6 + 1
        InputField() = Split(InputRecord(CurrentRecord), ",")
        
        'remove extra double quotes
        For CurrentField = LBound(InputField) To UBound(InputField)
            InputField(CurrentField) = Replace(InputField(CurrentField), """", "")
        Next
        
        'tally borrower total payoff
        BrwTtl = Round(BrwTtl, 2) + Round(CDbl(InputField(Dat.total)), 2)
        
        'format loan amounts
        InputField(Dat.Principal) = Format(InputField(Dat.Principal), "###,##0.00")
        InputField(Dat.Interest) = Format(CDbl(InputField(Dat.Interest)), "###,##0.00")
        InputField(Dat.total) = Format(InputField(Dat.total), "###,##0.00")
        
        'if the record being processed is not the last in the array, read next record in the array to determine if the record being processed is the last of the borrower/owner
        If CurrentRecord <> UBound(InputRecord) Then
            NextRecordField() = Split(InputRecord(CurrentRecord + 1), ",")
            If InputField(Dat.AgName) <> Replace(NextRecordField(Dat.AgName), """", "") Or InputField(Dat.HldCode1) <> Replace(NextRecordField(Dat.HldCode1), """", "") Then
                LastLoan = True
            Else
                LastLoan = False
            End If
        'the record being processed is the last for the owner if the record being processed is the last in the array
        Else
            LastLoan = True
        End If

'***************** READ ME *****************
'1.  A separate form has to be printed for each agency/borrower/owner
'2.  The form only has rows for 6 loans so only six records can be written to the text file and merged at a time
'3.  AddComs and BrtTtl fields have to be blank for all records except record 6 (see note 5)
'4.  If the borrower/owner has less than 6 loans, the remaining records still need to be written to the file but they are blank (see note 5)
'5.  Record 6 is unique to get the additional comments [InputField(4)] and borrower total [InputField(34)] values to print correctly
'    a.  If the borrower/owner has more than 6 loans, record 6 must have "see attached sheets for additional loans" in the AddComs field
'    b.  Record 6 must have the borrower total payoff in the BrwTtl field unless the borrower has more than 6 loans in which case the field should be blank so the borrower total only prints on the last form for the borrower/owner
'6.  The AddComs and BrtTtl fields [InputField(4)] and [InputField(34)] are already blank when read from the file so they only need to be changed if the value should not be blank

        'if the owner for the current record is different than the owner for the previous record, print the LVC and create a new data for the new lender
        If LastLoan = True Or xof6 = 6 Then
            'write data to file for loan being processed and blank fill remaining records in data file
            If xof6 < 6 Then
                'Add loan-specific fields.
                For CurrentField = Dat.LnAcntNo To Dat.DlyInt
                    LoanFields = LoanFields & ",""" & InputField(CurrentField) & """"
                Next CurrentField
                For BlankLoan = xof6 + 1 To 5
                    'Add as many blanks as there are loan-specific fields.
                    For CurrentField = Dat.LnAcntNo To Dat.DlyInt
                        LoanFields = LoanFields & ","""""
                    Next CurrentField
                Next BlankLoan
            End If
            
            'process sixth loan if it is not blank
            If xof6 = 6 Then
                'write data to file and reset brwttl if last loan for borrower/owner
                If LastLoan Then
                    'Add loan-specific fields.
                    For CurrentField = Dat.LnAcntNo To Dat.DlyInt
                        LoanFields = LoanFields & ",""" & InputField(CurrentField) & """"
                    Next CurrentField
                    'Set borrower-specific fields.
                    BorrowerFields = InputField(Dat.AgName)
                    For CurrentField = Dat.HldCode1 To Dat.BrwPhn
                        BorrowerFields = BorrowerFields & ",""" & InputField(CurrentField) & """"
                    Next CurrentField
                    BorrowerFields = BorrowerFields & ",""" & CStr(BrwTtl) & """"
                    For CurrentField = Dat.Ineltext To Dat.HldCode2
                        BorrowerFields = BorrowerFields & ",""" & InputField(CurrentField) & """"
                    Next CurrentField
                    BrwTtl = 0
                'write data to file with "see attached . . ." message if not last loan for borrower/owner
                Else
                    'Add loan-specific fields.
                    For CurrentField = Dat.LnAcntNo To Dat.DlyInt
                        LoanFields = LoanFields & ",""" & InputField(CurrentField) & """"
                    Next CurrentField
                    'Set borrower-specific fields.
                    BorrowerFields = InputField(Dat.AgName)
                    For CurrentField = Dat.HldCode1 To Dat.PayOffDt
                        BorrowerFields = BorrowerFields & ",""" & InputField(CurrentField) & """"
                    Next CurrentField
                    BorrowerFields = BorrowerFields & ",""see attached sheets for additional loans"""
                    For CurrentField = Dat.BrwFDCode To Dat.BrwPhn
                        BorrowerFields = BorrowerFields & ",""" & InputField(CurrentField) & """"
                    Next CurrentField
                    BorrowerFields = BorrowerFields & ","
                    For CurrentField = Dat.Ineltext To Dat.HldCode2
                        BorrowerFields = BorrowerFields & ",""" & InputField(CurrentField) & """"
                    Next CurrentField
                End If
            'process sixth loan if it is blank
            Else
                'Add loan-specific fields.
                For CurrentField = Dat.LnAcntNo To Dat.DlyInt
                    LoanFields = LoanFields & ","""""
                Next CurrentField
                'Set borrower-specific fields.
                BorrowerFields = InputField(Dat.AgName)
                For CurrentField = Dat.HldCode1 To Dat.BrwPhn
                    BorrowerFields = BorrowerFields & ",""" & InputField(CurrentField) & """"
                Next CurrentField
                BorrowerFields = BorrowerFields & ",""" & CStr(BrwTtl) & """"
                For CurrentField = Dat.Ineltext To Dat.HldCode2
                    BorrowerFields = BorrowerFields & ",""" & InputField(CurrentField) & """"
                Next CurrentField
                BrwTtl = 0
            End If
        
            'Write out the data file and print and image the LVC.
            Print #2, BorrowerFields & LoanFields
            Close #2
            SP.DocCreateAndDeploy.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt Doc, stacCOMPASS, Binfo.SSN, "T:\LVCdat.txt", "", Doc, DocFolder, UserId, "COMPASSLVC", "BrwST", dmLetter, "05", lrOther
            SP.Common.ImageDocs Dat.BrwSSN, "LSLVC", DocFolder, Doc, "T:\LVCdat.txt"
            
            'create a new data file if there are more loan to process
            If CurrentRecord < UBound(InputRecord) Then
                'set up a new data file
                Open "T:\LVCdat.txt" For Output As #2
                Write #2, "AgName", "HldCode", "HldName", "PayOffDt", "AddComs", "BrwFDCode", "BrwSSN", "BrwFN", "BrwMI", "BrwLN", "BrwAdd1", "BrwAdd2", "BrwCity", "BrwST", "BrwZip", "BrwCntry", "BrwPhn", "BrwTtl", "Ineltext", "CurDate", "HLDName", "HLDCode", _
                    "LnAcntNo1", "LnTypCd1", "IntRt1", "Prin1", "Int1", "Ttl1", "DsbDt1", "InSch1", "SchCd1", "ComplDt1", "Dflt1", "LnCd1", "GACd1", "BrwSta1", "GrcEnd1", "FirstPmt1", "DlyInt1", _
                    "LnAcntNo2", "LnTypCd2", "IntRt2", "Prin2", "Int2", "Ttl2", "DsbDt2", "InSch2", "SchCd2", "ComplDt2", "Dflt2", "LnCd2", "GACd2", "BrwSta2", "GrcEnd2", "FirstPmt2", "DlyInt2", _
                    "LnAcntNo3", "LnTypCd3", "IntRt3", "Prin3", "Int3", "Ttl3", "DsbDt3", "InSch3", "SchCd3", "ComplDt3", "Dflt3", "LnCd3", "GACd3", "BrwSta3", "GrcEnd3", "FirstPmt3", "DlyInt3", _
                    "LnAcntNo4", "LnTypCd4", "IntRt4", "Prin4", "Int4", "Ttl4", "DsbDt4", "InSch4", "SchCd4", "ComplDt4", "Dflt4", "LnCd4", "GACd4", "BrwSta4", "GrcEnd4", "FirstPmt4", "DlyInt4", _
                    "LnAcntNo5", "LnTypCd5", "IntRt5", "Prin5", "Int5", "Ttl5", "DsbDt5", "InSch5", "SchCd5", "ComplDt5", "Dflt5", "LnCd5", "GACd5", "BrwSta5", "GrcEnd5", "FirstPmt5", "DlyInt5", _
                    "LnAcntNo6", "LnTypCd6", "IntRt6", "Prin6", "Int6", "Ttl6", "DsbDt6", "InSch6", "SchCd6", "ComplDt6", "Dflt6", "LnCd6", "GACd6", "BrwSta6", "GrcEnd6", "FirstPmt6", "DlyInt6"
                'reset the counter of which loan is being processed for the owner
                xof6 = 0
            End If
            LoanFields = ""
        'write data to file if loan is not the last for the LVC
        Else
            'Add loan-specific fields.
            For CurrentField = Dat.LnAcntNo To Dat.DlyInt
                LoanFields = LoanFields & ",""" & InputField(CurrentField) & """"
            Next CurrentField
        End If
    Next CurrentRecord
End Sub

'add an activity record for approved LVCs
Function AddApvActRec4InEligAndInSch(CommentText As String)
    Dim ARC As String
    Dim row As Integer
    Dim i As Integer
    Dim j As Integer
    Dim l As Integer
    ARC = "STPCN"
        
    'find the ARC on TD22
    FastPath "TX3Z/ATD22" & Binfo.SSN
    Do
        found = Session.FindText(ARC, 8, 8)
        If found Then Exit Do
        Hit "F8"
        If Check4Text(23, 2, "90007") Then
            If Not SP.Common.ATD37FirstLoan(Binfo.SSN, ARC, CommentText, UserId, "COMPASSLVC") Then
                MsgBox "The script was unable to add the activity comment on TD22 or TD37 for ineligiblity.  Please contact Systems Support for assistance."
                End
            End If
        End If
    Loop
    'select the ARC
    PutText Session.FoundTextRow, Session.FoundTextColumn - 5, "01", "ENTER"
    'check if the script is on the correct screen
    If Check4Text(1, 72, "TDX24") = False Then
        If Not SP.Common.ATD37FirstLoan(Binfo.SSN, ARC, CommentText, UserId, "COMPASSLVC") Then
            MsgBox "The script was unable to add the activity comment on TD22 or TD37 for ineligiblity.  Please contact Systems Support for assistance."
            End
        End If
    End If
    'review each loan for selection
    row = 11
    Do
        'select the loan if it is in the array
        PutText row, 3, "X"
        row = row + 1
        'go to the next page
        If Not Check4Text(row, 3, "_") Then
            If Check4Text(8, 75, "+") Then
                Hit "F8"
                row = 11
            Else
                Exit Do
            End If
        End If
    Loop
    'access additional comments
    Hit "ENTER"
    'check if the script was able to add the activity comment
    If Check4Text(23, 2, "02860 PROCESSING FOR SELECTED ACTION CODES HAS BEEN COMPLETED") = False Then
        If Not SP.Common.ATD37FirstLoan(Binfo.SSN, ARC, CommentText, UserId, "COMPASSLVC") Then
            MsgBox "The script was unable to add the activity comment on TD22 or TD37 for ineligiblity.  Please contact Systems Support for assistance."
            End
        End If
    End If
    Hit "F4"
    
    'enter comment
    PutText 8, 5, CommentText
    Hit "ENTER"
    CloseQ
End Function

Function IsProofOfAppSignedRequired() As Boolean
    Dim ARCs(3) As String
    Dim i As Integer
    Dim ProofFrm As New frmLVCProofOfAppSgDt
    ARCs(0) = "DFDLC"
    ARCs(1) = "DSLVC"
    ARCs(2) = "STPCN"
    For i = 0 To UBound(ARCs) - 1
        FastPath "TX3ZITD2A" & Binfo.SSN
        PutText 11, 65, ARCs(i)
        PutText 21, 16, "070106"
        PutText 21, 30, Format(Date, "MMDDYY"), "Enter"
        If Check4Text(1, 72, "TDX2B") = False Then
            'something was found
            IsProofOfAppSignedRequired = ProofFrm.ProofRequired()
            Exit Function
        End If
    Next
    IsProofOfAppSignedRequired = False
End Function

Sub CloseQ()
    Dim EarliestDate As String
    Dim Selection As String
    Dim row As Integer
    Dim PagesBack As Integer
    
    'Access ITX6T for the SSN
    FastPath "TX3Z/ITX6T" & Binfo.SSN
    If Check4Text(23, 2, "01020") Then Exit Sub 'No records found
    
    'Gather the sel number of the "VC" queue task with the earliest required date
    row = 7
    'EarliestDate = GetText(row + 1, 49, 10)
    PagesBack = -1  'Initialize to -1 so that it gets set to 0 when we start looping through pages
    Do While Not Check4Text(23, 2, "90007")
        PagesBack = PagesBack + 1
        Do While row < 18 And Not Check4Text(row, 3, " ")
            'If this is a "VC" task, see if the date is earlier than the earliest one found so far
            If Check4Text(row, 8, "VC") And EarliestDate = "" Then
                'Set this task's required date as the earliest
                EarliestDate = GetText(row + 1, 49, 10)
                Selection = GetText(row, 3, 1)
                'Note that the earliest date is on the current page
                PagesBack = 0
            ElseIf Check4Text(row, 8, "VC") Then
                If DateDiff("d", CDate(EarliestDate), CDate(GetText(row + 1, 49, 10))) <= 0 Then
                    'Set this task's required date as the earliest
                    EarliestDate = GetText(row + 1, 49, 10)
                    Selection = GetText(row, 3, 1)
                    'Note that the earliest date is on the current page
                    PagesBack = 0
                End If
            End If
            'Go to the next queue task
            row = row + 5
        Loop
        'Move on to the next page
        Hit "F8"
    Loop
    
    'Go back to the page with the earliest required date
    Dim i As Integer
    i = 0
    Do While i < PagesBack  'This would be a lot simpler if VB had more flexible "for" loops.
        Hit "F7"
        i = i + 1
    Loop
    
    'Enter the sel number in the selection field to place the queue task in a working status
    PutText 21, 18, Selection, "ENTER"
    
    'Access ITX6T for the SSN again
    FastPath "TX3Z/ITX6T" & Binfo.SSN
    
    'Gather the sel number for the "VC" queue task where the queue status is "W"
    row = 7
    Do While Not Check4Text(23, 2, "90007")
        Do While row < 18
            'See if this is a "VC" task with a "W" queue status
            If Check4Text(row, 8, "VC") And Check4Text(row + 1, 76, "W") Then
                'Enter the sel number gathered in the selection field and press <F2>
                PutText 21, 18, GetText(row, 3, 1), "F2"
                'Enter "C" in the task status field
                PutText 8, 19, "C"
                'Enter "COMPL" in the action response field
                PutText 9, 19, "COMPL", "ENTER"
                'Return to script form
                Exit Sub
            End If
            'Go to the next queue task
            row = row + 5
        Loop
        'Move to the next page if no "VC" task with a "W" queue status was found
        Hit "F8"
    Loop
End Sub

Public Function protectSSN(fullSSN As String) As String
    If fullSSN <> "" Then
        'SSN isn't blank needs to be protected
        fullSSN = "XXX-XX-" & Right(fullSSN, 4)
    End If
    protectSSN = fullSSN
End Function

