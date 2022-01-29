Attribute VB_Name = "CondDblty"
Private Type LoanStruct
    LoanType As String
    CommonLineID As String
    ClaimID As String
    GuaranteeAmount As String
    DisbursementDate As String
    Balance As String   'Storing the numeric values as strings will keep the letter from printing "$0.00" for loans that don't exist.
End Type

Private Const LC05_ERROR_DATA As String = "T:\condsblty LC05.txt"
Private Const LP22_ERROR_DATA As String = "T:\condsblty LP22.txt"

Private UserID As String
Private DocFolder As String
Private SSN As String

Sub CondDsblty()
    Dim LP22HadProblems As Boolean
    Dim LC05HadProblems As Boolean
    Dim Testing As Boolean
    
    ResetPublicVars
    LP22HadProblems = False
    LC05HadProblems = False
    
    'tell the user what the script will do and end the script if the dialog box is canceled
    If MsgBox("This script will work tasks in the CNDDISAB queue to update loans approved for conditional disability discharge.", vbOKCancel, "Conditional Disability Discharge") <> vbOK Then End
    
    'set up data files and path
    DocFolder = "X:\PADD\Collections\"
    Testing = sp.Common.TestMode(, DocFolder)
    Open LC05_ERROR_DATA For Output As #1
    Write #1, "SSN", "Blank"
    Close #1
    Open LP22_ERROR_DATA For Output As #2
    Write #2, "SSN", "CLID"
    Close #2
    UserID = sp.Common.GetUserID
    
    'get the task from LP9A
    FastPath "LP9ACCNDDISAB"
    
    'warn the user and end the script if there are no tasks
    If Check4Text(22, 3, "47423") Or Check4Text(22, 3, "47420") Then
        MsgBox "There are no tasks in the CNDDISAB queue."
        End
    End If
    'warn the user and end the script is a task for a queue other than DGARN is displayed
    If Check4Text(1, 9, "CNDDISAB") = False Then
        MsgBox "You have an unresolved task in the " & GetText(1, 9, 8) & " queue.  You must complete the task before working the CNDDISAB queue."
        End
    End If
    
    'Process queue tasks until there are none left.
    Do While Not Check4Text(22, 3, "46004")
        ProcessTasks LP22HadProblems, LC05HadProblems, Testing
    Loop
    
    'send error reports
    SendErrorReports LP22HadProblems, LC05HadProblems, Testing
    
    'delete temporary files
    If Dir(LP22_ERROR_DATA) <> "" Then Kill LP22_ERROR_DATA
    If Dir(LC05_ERROR_DATA) <> "" Then Kill LC05_ERROR_DATA
    
    'warn the user that processing is complete
    MsgBox "There are no more tasks in the CNDDISAB queue.  Processing is complete."
End Sub

Private Sub ProcessTasks(ByRef LP22HadProblems As Boolean, ByRef LC05HadProblems As Boolean, ByVal Testing As Boolean)
    Dim Loans() As LoanStruct
    Dim AccountNum As String
    Dim HasValidAddress As Boolean
    Dim DeterDate As String
    ReDim Loans(0)
    
    'get the ssn
    SSN = GetText(17, 70, 9)
    AccountNum = Replace(GetText(17, 52, 12), " ", "")
    'get date of determination for RDBDD activity record
    FastPath "LP50I" & SSN
    PutText 9, 20, "RDBDD", "ENTER"
    'warn the user and end the script if no record is found
    If Check4Text(22, 3, "47004") Then
        MsgBox "An activity record with action code RDBDD has not been created for " & AccountNum & ".", vbOKOnly, "No RDBDD Activity Record Found"
        SendErrorReports LP22HadProblems, LC05HadProblems, Testing
        End
    End If
    'select the first (most recent) record if the selection screen is displayed
    If Check4Text(3, 2, "SEL") Then PutText 3, 13, "X", "ENTER"
    'get the date
    DeterDate = GetText(7, 15, 8)
    'verify that the permanent disability date has been entered on LP22
    'access LP22
    FastPath "LP22I" & SSN
    'set print letter trigger to "Y" and get demographic information
        'if address is valid
    If Check4Text(11, 57, "Y") Then
        HasValidAddress = True
        'get demographic info from LP22
        AccountNum = GetText(3, 60, 12)
        DBQLetters.LastName = GetText(4, 5, 35)
        DBQLetters.FirstName = GetText(4, 44, 12)
        DBQLetters.Address1 = GetText(11, 9, 35)
        DBQLetters.Address2 = GetText(12, 9, 35)
        DBQLetters.City = GetText(13, 9, 35)
        DBQLetters.State = GetText(13, 52, 2)
        DBQLetters.Country = GetText(12, 55, 25)
        DBQLetters.ZIP = GetText(13, 60, 5)
        DBQLetters.ZIP4 = GetText(13, 65, 4)
        'create ZIP+4 string
        If DBQLetters.ZIP4 <> "" Then
            DBQLetters.ZIP = DBQLetters.ZIP + "-" + DBQLetters.ZIP4
        End If
    Else
        HasValidAddress = False
        'add the ssn and unique ID to an error report
        Open LP22_ERROR_DATA For Append As #2
        Write #2, AccountNum, "No Valid Address"
        Close #2
        LP22HadProblems = True
    End If
    'prompt the use to enter the date if it is blank
    Do While Check4Text(6, 48, "MMDDCCYY")
        MsgBox "The permanent disability date has not been entered.  Enter the date and hit Insert to continue.", vbOKOnly, "Enter Permanent Disability Date"
        PauseForInsert
    Loop
    
    'Get loan data from LC05.
    FastPath "LC05I" & SSN
    If Not Check4Text(22, 3, "47004") Then GetLC05 Loans, DeterDate
    
    If UBound(Loans) = 0 Then
        Open LC05_ERROR_DATA For Append As #1
        Write #1, AccountNum, ""
        Close #1
        LC05HadProblems = True
    Else
        If HasValidAddress Then
            'print the conditional disability letter
            PrintLetter AccountNum, Loans
            DBQLetters.ActCd = "DLSCD"
        Else
            'add the ssn and unique ID to an error report
            Open LP22_ERROR_DATA For Append As #2
            Dim i As Integer
            For i = 1 To UBound(Loans)
                Write #2, AccountNum, Loans(i).CommonLineID
            Next i
            Close #2
            DBQLetters.ActCd = "DLNCD"
            LP22HadProblems = True
        End If
        'add an activity record
        sp.Common.AddLP50 SSN, DBQLetters.ActCd, "CONDSBLTY", "LT", "03"
    End If
    
    'close the task and go to the next task
    FastPath "LP9ACCNDDISAB"
    Hit "F6"
    Hit "F8"
End Sub

Private Sub GetLC05(ByRef Loans() As LoanStruct, ByVal DeterDate As String)
    Dim Row As Integer
    Dim Counter As Integer
    
    'Select the first record.
    PutText 21, 13, "01", "ENTER"
    Do While Not Check4Text(22, 3, "46004")
        'Get the loan data if it is deferred or disability.
        If Check4Text(4, 10, "03") And (Check4Text(4, 26, "05") Or Check4Text(4, 26, "03")) Then
            ReDim Preserve Loans(UBound(Loans) + 1)
            'Get the loan details.
            Loans(UBound(Loans)).Balance = CStr(CDbl(GetText(20, 32, 12)) - CDbl(GetText(19, 32, 12)))
            Select Case GetText(3, 13, 2)
                Case "SF", "SU"
                    Loans(UBound(Loans)).LoanType = "Stafford"
                Case "PL"
                    Loans(UBound(Loans)).LoanType = "PLUS"
                Case "SL"
                    Loans(UBound(Loans)).LoanType = "SLS"
            End Select
            Loans(UBound(Loans)).ClaimID = GetText(21, 11, 4)
            Hit "F10"
            Hit "F10"
            Loans(UBound(Loans)).CommonLineID = GetText(3, 13, 19)
        End If
        'Go to the next loan.
        Hit "F8"
    Loop
End Sub

'print the conditional disability letter
Sub PrintLetter(ByVal AccountNum As String, ByRef Loans() As LoanStruct)
    Const MaxLoans As Integer = 25  'The most we can put on the letter.
    Const DataFile As String = "T:\condsblty.txt"
    Const LetterID As String = "DSBLTY"
    Const ScriptID As String = "CONDSBLTY"
    
    'get guaranty amount and 1st disb date from LG0H for each loan
    Dim Counter As Integer
    For Counter = 1 To UBound(Loans)
        FastPath "LG0HI;" & SSN & ";;" & Loans(Counter).CommonLineID
        Loans(Counter).GuaranteeAmount = GetText(2, 40, 10)
        Loans(Counter).DisbursementDate = Format(GetText(12, 20, 8), "##/##/####")
    Next Counter
    
    'write info to data file
    ReDim Preserve Loans(MaxLoans)   'Ensure we have exactly as many loans as the letter needs.
    Open DataFile For Output As #3
    Write #3, "AccountNumber", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Country", "KeyLine", _
        "Loan1", "Type1", "DisbDate1", "GuarAmt1", "Bal1", "Loan2", "Type2", "DisbDate2", "GuarAmt2", "Bal2", "Loan3", "Type3", "DisbDate3", "GuarAmt3", "Bal3", "Loan4", "Type4", "DisbDate4", "GuarAmt4", "Bal4", "Loan5", "Type5", "DisbDate5", "GuarAmt5", "Bal5", _
        "Loan6", "Type6", "DisbDate6", "GuarAmt6", "Bal6", "Loan7", "Type7", "DisbDate7", "GuarAmt7", "Bal7", "Loan8", "Type8", "DisbDate8", "GuarAmt8", "Bal8", "Loan9", "Type9", "DisbDate9", "GuarAmt9", "Bal9", "Loan10", "Type10", "DisbDate10", "GuarAmt10", "Bal10", _
        "Loan11", "Type11", "DisbDate11", "GuarAmt11", "Bal11", "Loan12", "Type12", "DisbDate12", "GuarAmt12", "Bal12", "Loan13", "Type13", "DisbDate13", "GuarAmt13", "Bal13", "Loan14", "Type14", "DisbDate14", "GuarAmt14", "Bal14", "Loan15", "Type15", "DisbDate15", "GuarAmt15", "Bal15", _
        "Loan16", "Type16", "DisbDate16", "GuarAmt16", "Bal16", "Loan17", "Type17", "DisbDate17", "GuarAmt17", "Bal17", "Loan18", "Type18", "DisbDate18", "GuarAmt18", "Bal18", "Loan19", "Type19", "DisbDate19", "GuarAmt19", "Bal19", "Loan20", "Type20", "DisbDate20", "GuarAmt20", "Bal20", _
        "Loan21", "Type21", "DisbDate21", "GuarAmt21", "Bal21", "Loan22", "Type22", "DisbDate22", "GuarAmt22", "Bal22", "Loan23", "Type23", "DisbDate23", "GuarAmt23", "Bal23", "Loan24", "Type24", "DisbDate24", "GuarAmt24", "Bal24", "Loan25", "Type25", "DisbDate25", "GuarAmt25", "Bal25"
    Write #3, AccountNum, DBQLetters.FirstName, DBQLetters.LastName, DBQLetters.Address1, DBQLetters.Address2, DBQLetters.City, DBQLetters.State, DBQLetters.ZIP, DBQLetters.Country, sp.Common.ACSKeyLine(SSN), _
        Loans(1).CommonLineID, Loans(1).LoanType, Loans(1).DisbursementDate, Loans(1).GuaranteeAmount, Loans(1).Balance, Loans(2).CommonLineID, Loans(2).LoanType, Loans(2).DisbursementDate, Loans(2).GuaranteeAmount, Loans(2).Balance, Loans(3).CommonLineID, Loans(3).LoanType, Loans(3).DisbursementDate, Loans(3).GuaranteeAmount, Loans(3).Balance, Loans(4).CommonLineID, Loans(4).LoanType, Loans(4).DisbursementDate, Loans(4).GuaranteeAmount, Loans(4).Balance, Loans(5).CommonLineID, Loans(5).LoanType, Loans(5).DisbursementDate, Loans(5).GuaranteeAmount, Loans(5).Balance, _
        Loans(6).CommonLineID, Loans(6).LoanType, Loans(6).DisbursementDate, Loans(6).GuaranteeAmount, Loans(6).Balance, Loans(7).CommonLineID, Loans(7).LoanType, Loans(7).DisbursementDate, Loans(7).GuaranteeAmount, Loans(7).Balance, Loans(8).CommonLineID, Loans(8).LoanType, Loans(8).DisbursementDate, Loans(8).GuaranteeAmount, Loans(8).Balance, Loans(9).CommonLineID, Loans(9).LoanType, Loans(9).DisbursementDate, Loans(9).GuaranteeAmount, Loans(9).Balance, Loans(10).CommonLineID, Loans(10).LoanType, Loans(10).DisbursementDate, Loans(10).GuaranteeAmount, Loans(10).Balance, _
        Loans(11).CommonLineID, Loans(11).LoanType, Loans(11).DisbursementDate, Loans(11).GuaranteeAmount, Loans(11).Balance, Loans(12).CommonLineID, Loans(12).LoanType, Loans(12).DisbursementDate, Loans(12).GuaranteeAmount, Loans(12).Balance, Loans(13).CommonLineID, Loans(13).LoanType, Loans(13).DisbursementDate, Loans(13).GuaranteeAmount, Loans(13).Balance, Loans(14).CommonLineID, Loans(14).LoanType, Loans(14).DisbursementDate, Loans(14).GuaranteeAmount, Loans(14).Balance, Loans(15).CommonLineID, Loans(15).LoanType, Loans(15).DisbursementDate, Loans(15).GuaranteeAmount, Loans(15).Balance, _
        Loans(16).CommonLineID, Loans(16).LoanType, Loans(16).DisbursementDate, Loans(16).GuaranteeAmount, Loans(16).Balance, Loans(17).CommonLineID, Loans(17).LoanType, Loans(17).DisbursementDate, Loans(17).GuaranteeAmount, Loans(17).Balance, Loans(18).CommonLineID, Loans(18).LoanType, Loans(18).DisbursementDate, Loans(18).GuaranteeAmount, Loans(18).Balance, Loans(19).CommonLineID, Loans(19).LoanType, Loans(19).DisbursementDate, Loans(19).GuaranteeAmount, Loans(19).Balance, Loans(20).CommonLineID, Loans(20).LoanType, Loans(20).DisbursementDate, Loans(20).GuaranteeAmount, Loans(20).Balance, _
        Loans(21).CommonLineID, Loans(21).LoanType, Loans(21).DisbursementDate, Loans(21).GuaranteeAmount, Loans(21).Balance, Loans(22).CommonLineID, Loans(22).LoanType, Loans(22).DisbursementDate, Loans(22).GuaranteeAmount, Loans(22).Balance, Loans(23).CommonLineID, Loans(23).LoanType, Loans(23).DisbursementDate, Loans(23).GuaranteeAmount, Loans(23).Balance, Loans(24).CommonLineID, Loans(24).LoanType, Loans(24).DisbursementDate, Loans(24).GuaranteeAmount, Loans(24).Balance, Loans(25).CommonLineID, Loans(25).LoanType, Loans(25).DisbursementDate, Loans(25).GuaranteeAmount, Loans(25).Balance
    Close #3
    
    'Call the centralized printing subroutine.
    sp.DocCreateAndDeploy.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt LetterID, stacOneLINK, SSN, DataFile, "AccountNumber", LetterID, DocFolder, UserID, ScriptID, "State", dmLetter
    Kill DataFile
End Sub

Private Sub SendErrorReports(ByVal LP22HadProblems As Boolean, ByVal LC05HadProblems As Boolean, ByVal Testing As Boolean)
    Const LC05_ERROR_REPORT As String = "T:\ConDblty_LC05_Errors.doc"
    Const LP22_ERROR_REPORT As String = "T:\ConDblty_LP22_Errors.doc"
    
    'This sub gets called even when there aren't errors to print, so first check if anything needs to be done.
    If LP22HadProblems = False And LC05HadProblems = False Then
        Exit Sub
    End If
    
    'Look up the Manager of Account Resolution in BSYS.
    Dim QueryResults() As String
    Dim RecipientName As String
    QueryResults = sp.Common.SQLEx("SELECT WindowsUserID FROM GENR_REF_BU_Agent_Xref WHERE Role = 'Manager' AND BusinessUnit = 'Loan Management'")
    RecipientName = QueryResults(0, 0)
    
    'Create and send the error report documents.
    Dim Subject As String
    Dim Message As String
    Dim Attachment As String
    DBQLetters.DocPath = DocFolder
    If LC05HadProblems Then
        DBQLetters.Doc = "Conditional Disability LC05 Error Report"
        sp.Common.SaveDocs DBQLetters.DocPath, DBQLetters.Doc, LC05_ERROR_DATA, Replace(LC05_ERROR_REPORT, ".doc", "")
        Subject = "Conditional Disability Errors LC05"
        Message = "The attached borrowers had errors on LC05 when the Conditional Disability Script was run."
        Attachment = LC05_ERROR_REPORT
        If Testing Then
            Subject = "TESTING--" & Subject
            Message = "This e-mail will be sent to " & RecipientName & "@utahsbr.edu when run in live." & vbLf & vbLf & Message
            sp.Common.SendMail Environ("USERNAME") & "@utahsbr.edu", , Subject, Message, , , Attachment, , False
        Else
            sp.Common.SendMail RecipientName & "@utahsbr.edu", , Subject, Message, , , Attachment, , False
        End If
    End If
    If LP22HadProblems Then
        DBQLetters.Doc = "Conditional Disability LP22 Error Report"
        sp.Common.SaveDocs DBQLetters.DocPath, DBQLetters.Doc, LP22_ERROR_DATA, Replace(LP22_ERROR_REPORT, ".doc", "")
        Subject = "Conditional Disability Errors LP22"
        Message = "The attached borrowers had errors on LP22 when the Conditional Disability Script was run."
        Attachment = LP22_ERROR_REPORT
        If Testing Then
            Subject = "TESTING--" & Subject
            Message = "This e-mail will be sent to " & RecipientName & "@utahsbr.edu when run in live." & vbLf & Message
            sp.Common.SendMail Environ("USERNAME") & "@utahsbr.edu", , Subject, Message, , , Attachment, , False
        Else
            sp.Common.SendMail RecipientName & "@utahsbr.edu", , Subject, Message, , , Attachment, , False
        End If
    End If
    If Dir(LC05_ERROR_REPORT) <> "" Then Kill LC05_ERROR_REPORT
    If Dir(LP22_ERROR_REPORT) <> "" Then Kill LP22_ERROR_REPORT
End Sub

