Attribute VB_Name = "ClaimLetters"
'LP22
Public SSN As String        'TODO: This variable desperately needs to be re-scoped.
Public LastFourSSN As String 'Used to display the SSN as XXX-XX-1234
Private Address1 As String  'TODO: See if this needs to be reset at some point.
Private Address2 As String  'TODO: See if this needs to be reset at some point.
Private Address3 As String  'TODO: See if this needs to be reset at some point.
Private City As String      'TODO: See if this needs to be reset at some point.
Private State As String     'TODO: See if this needs to be reset at some point.
Private Zip As String       'TODO: See if this needs to be reset at some point.
Private ZIP4 As String      'TODO: See if this needs to be reset at some point.

'Claims Letters
Private Type LoanStruct
    Holder As String
    Servicer As String
    LoanType As String
    ClaimPaidDate As String
    ClmDate As String
    ClmTime As String
    Name As String
    Lender As String
    ClaimType As String
    ClaimReceivedDate As String
    Desc As String
    CLID(1 To 9) As String
End Type
Private Loan As LoanStruct  'TODO: See about re-scoping this more narrowly.

Private DocFolder As String
Private ToPrinter As Boolean

'displays main menu for user to select letter to print
Sub DisplayMainMenu()
    DocFolder = "X:\PADD\Claims\"
    SP.Common.TestMode , DocFolder
    ToPrinter = False
    Load MainMenu
    MainMenu.Show
    Unload MainMenu
End Sub

Sub ReturnProc()
    'get information from LC55
    LC55
    GetIDs
    
    'getlender or servicer demographics
    GetLenderServicer

    'return to LC55
    FastPath "LC55I" & SSN & ";" & Loan.Holder & ";" & Loan.Servicer & ";" & Loan.LoanType & ";" & Loan.ClmDate & ";" & Loan.ClmTime

    'create letter
    CreateLetter "Return"
End Sub

Sub ClaimPaidProc()
    'get information from LC55
    LC55
    GetIDs

    'go to second page to get claim paid date
    PutText 1, 7, "C", "Enter"
    PutText 21, 46, "Y", "Enter"
    Loan.ClaimPaidDate = Format(GetText(4, 73, 8), "##/##/####")
        
    'go to lender or servicer demographics
    GetLenderServicer
    
    'return to LC55
    FastPath "LC55I" & SSN & ";" & Loan.Holder & ";" & Loan.Servicer & ";" & Loan.LoanType & ";" & Loan.ClmDate & ";" & Loan.ClmTime

    'create letter
    CreateLetter "ClaimPD"
End Sub

Sub RejectProc()
    'get information from LC55
    LC55
        
    'assign description to Desc if rejected for a timely filing violation
    If GetText(6, 5, 1) = "N" Then
        Loan.Desc = "Timely Filing Violation"
    'assign description to Desc if rejected for a timely converion violation
    ElseIf GetText(6, 11, 1) = "N" Then
        Loan.Desc = "Timely Conversion Violation"
    Else
        Loan.Desc = ""
    End If
    
    'assign description to Desc if rejected for a due diligence violation
    Select Case GetText(6, 17, 1)
        Case "E"
            Loan.Desc = "3 Due Diligence Violations"
        Case "F"
            Loan.Desc = "4 or More Due Diligence Violations"
        Case "G"
            Loan.Desc = "Gap in Due Diligence"
        Case "J"
            Loan.Desc = "Final Demand Letter Sent Late for an Ineligible Borrower"
        Case "R", "S", "U"
            Loan.Desc = "Abreviated Cure Procedures Incomplete for a Timely Filing Violation"
    End Select
                    
    'get unique IDs of loans
    GetIDs
    
    'go to lender or servicer demographics
    GetLenderServicer
    
    'return to LC55
    FastPath "LC55I" & SSN & ";" & Loan.Holder & ";" & Loan.Servicer & ";" & Loan.LoanType & ";" & Loan.ClmDate & ";" & Loan.ClmTime

    'create letter
    CreateLetter "Reject"
End Sub

'print manual letters
Sub ManualLettersProc(Doc As String)
    'display the ClaimInput dialog box
    Load ClaimInput
    ClaimInput.Show
    'quit if the OK button was not clicked
    If ClaimInput.OkWasClicked = False Then End
            
    'gather values from dialog box variables
    SSN = ClaimInput.txtSSN.Text
    LastFourSSN = "XXX-XX-" & Right(SSN, 4)
    Loan.Name = ClaimInput.txtBorrName.Text
    Loan.Servicer = ClaimInput.cmbServicerID.Text
    Loan.Holder = ClaimInput.cmbHolderID.Value
    Loan.ClaimType = ClaimInput.cmbClaimType.Text
    Loan.ClaimReceivedDate = Format(ClaimInput.txtReceivedDate, "##/##/####")
    'Warn the user of the letter's limits if the list box exceeds the limit.
    If ClaimInput.lstSelectedUniqueIDs.ListCount > 9 Then MsgBox "Only 9 of the Unique IDs will be shown on the letter."
    'Assign as many CLIDs as appear in the list box and will fit on the letter.
    Dim i As Integer
    For i = 1 To 9
        If ClaimInput.lstSelectedUniqueIDs.ListCount >= i Then
            Loan.CLID(i) = ClaimInput.lstSelectedUniqueIDs.list(i - 1)
        Else
            'Clear out any unused array indices.
            Loan.CLID(i) = ""
        End If
    Next i
    Unload ClaimInput
        
    'getlender or servicer demographics
    GetLenderServicer
        
    'create letter
    CreateLetter Doc
End Sub

'get information from LC55
Function LC55()
    'end the script if the user in not in LC55
    If Not Check4Text(1, 53, "CLAIM NOTE PRORATION DISPLAY") Then
        MsgBox "You must be in the LC55 CLAIM NOTE PRORATION DISPLAY to run this script.", vbCritical, "Claim Letters"
        End
    End If

    'get loan information
    Loan.LoanType = GetText(1, 33, 2)
    Loan.ClmDate = GetText(4, 22, 8)
    Loan.ClmTime = GetText(4, 31, 12)
    SSN = GetText(1, 9, 9)
    Loan.Name = Trim(GetText(2, 2, 40))
    Loan.Holder = GetText(3, 9, 6)
    Loan.Servicer = GetText(3, 28, 6)
    Loan.ClaimType = GetText(2, 51, 2)
    
    'assign description to ClaimType
    Select Case Loan.ClaimType
        Case "BC", "BH", "BO"
            Loan.ClaimType = "bankruptcy"
        Case "CS"
            Loan.ClaimType = "closed school"
        Case "DE"
            Loan.ClaimType = "death"
        Case "DI"
            Loan.ClaimType = "disability"
        Case "DU"
            Loan.ClaimType = "abreviated cure"
        Case "FC"
            Loan.ClaimType = "false certification"
        Case "IN"
            Loan.ClaimType = "ineligible borrower"
        Case Else
            Loan.ClaimType = "default"
    End Select
    
    'get and format claim received date
    Loan.ClaimReceivedDate = Format(GetText(3, 72, 8), "##/##/####")

End Function

'get the loan unique IDs
Function GetIDs()
    Dim i As Integer
    For i = 1 To 9
        Loan.CLID(i) = ""
    Next i

    'get unique IDs of loans
    Loan.CLID(1) = GetText(15, 2, 19)
    Loan.CLID(2) = Trim(GetText(17, 2, 19))
    Loan.CLID(3) = Trim(GetText(19, 2, 19))
    Hit "F8"
    If GetText(22, 3, 5) <> "46004" Then
        Loan.CLID(4) = GetText(15, 2, 19)
        Loan.CLID(5) = Trim(GetText(17, 2, 19))
        Loan.CLID(6) = Trim(GetText(19, 2, 19))
        Hit "F8"
        If GetText(22, 3, 5) <> "46004" Then
            Loan.CLID(7) = GetText(15, 2, 19)
            Loan.CLID(8) = Trim(GetText(17, 2, 19))
            Loan.CLID(9) = Trim(GetText(19, 2, 19))
        End If
    End If
End Function

'get the lender or servicer information
Function GetLenderServicer()
    If Loan.Servicer = Loan.Holder Or Loan.Servicer = "" Then
        FastPath "LPLDI" & Loan.Holder
    Else
        FastPath "LPSVI" & Loan.Servicer
    End If
    
    'get lender/servicer information
    Loan.Lender = Trim(GetText(6, 21, 20))
    Address1 = Trim(GetText(8, 21, 40))
    Address2 = Trim(GetText(9, 21, 40))
    Address3 = Trim(GetText(10, 21, 40))
    City = Trim(GetText(11, 21, 30))
    State = GetText(11, 59, 2)
    Zip = GetText(11, 66, 5)
    ZIP4 = Trim(GetText(11, 71, 4))
    'create ZIP+4 string
    If ZIP4 <> "" Then
        Zip = Zip + "-" + ZIP4
    End If
End Function

'create the letter
Function CreateLetter(Doc As String)
    Open "T:\claimdat.txt" For Output As #1
    Write #1, "SSN", "Name", "LenderNo", "Lender", "Address1", "Address2", "Address3", "City", "State", "ZIP", "Type", "Received", "Desc", "CLID1", "CLID2", "CLID3", "CLID4", "CLID5", "CLID6", "CLID7", "CLID8", "CLID9", "ClmPaid"
    Write #1, "XXX-XX-" & Right(SSN, 4), Loan.Name, Loan.Holder, Loan.Lender, Address1, Address2, Address3, City, State, Zip, Loan.ClaimType, Loan.ClaimReceivedDate, Loan.Desc, Loan.CLID(1), Loan.CLID(2), Loan.CLID(3), Loan.CLID(4), Loan.CLID(5), Loan.CLID(6), Loan.CLID(7), Loan.CLID(8), Loan.CLID(9), Loan.ClaimPaidDate
    Close #1
    SP.Common.PrintDocs DocFolder, Doc, "T:\claimdat.txt", ToPrinter
    Kill "T:\claimdat.txt"
End Function
