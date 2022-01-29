Attribute VB_Name = "InsNo"
Dim SSN As String
Dim Lender As String
Dim CLUID As String
Dim AppTyp As String
Dim bssn As String
Dim BName As String
Dim ProcDt As String
Dim InsNum As String
Dim Batch As String
Dim Sequence As String
Dim i As Boolean

'search a list of old Guarantec Insurance (Guaranty) Numbers for preconversion loans.
Sub InsNo()
With Session
    'prompt for the SSN
    SSN = InputBox("Enter a borrower's SSN for a list of associated insurance numbers.", "Insurance Number Query")
    If SSN = "" Then
        End
    ElseIf Len(SSN) <> 9 Then
        MsgBox "Invalid SSN entered, please click OK and re-enter the SSN.", vbOKOnly, "Invalid Entry"
        InsNo
    End If
    
    'display please wait message
    Load PleaseWait
    PleaseWait.Show 0
    Wait 1
    
    'clear the previous file
    Open "T:\INSNO.txt" For Output As #1
    Close #1
    'look up all matching rows and write them to the merge file
    Open "X:\PADD\Guarantee\INSNO.txt" For Input As #1
    Open "T:\INSNO.txt" For Append As #2
    i = False
    
    Write #2, "Lender", "CLUID", "AppTyp", "BSSN", "BName", "ProcDt", "InsNo", "Batch", "Sequence"
    Do Until EOF(1)
        Input #1, Lender, CLUID, AppTyp, bssn, BName, ProcDt, InsNum, Batch, Sequence
        If bssn = SSN Then
            Write #2, Lender, CLUID, AppTyp, bssn, BName, ProcDt, InsNum, Batch, Sequence
            i = True
        End If
    Loop
    Close #1
    Close #2

    If i Then

        'write the records to Word
        PrintDocs "X:\PADD\Guarantee\", "INSNO", "T:\INSNO.txt", False
        
        'close please wait message
        PleaseWait.Hide
        Unload PleaseWait

    Else
        'close please wait message
        PleaseWait.Hide
        Unload PleaseWait

        MsgBox "No pre-conversion loans were found for the entered SSN " & SSN & ".  Click OK to try again.", vbOKOnly, "Search Results"
        InsNo
    End If
End With
End Sub

