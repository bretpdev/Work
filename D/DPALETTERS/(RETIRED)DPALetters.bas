Attribute VB_Name = "DPALetters"
'Variables common to both scripts:
Private docFolder As String
Private doc As String
Private CSSN As String
Private DrawDate As String
Private DPInd As String
Private Loan As Integer
Private ClaimID(1 To 99) As String
Private ltr As Integer
Private lnsta As String     'This is used only by Canc() and LC05(); probably should be local to Canc() and passed by reference.
Private seventh As String   'This is used by LC05() and LC34(); see if it can be local to LC05() and passed by reference.
Private ExpPmt As Double    'This is used only by Conf() and LC05(); probably should be local to Conf() and passed by reference.

'Variables belonging only to Canc(), but that need to be set from the form:
Private ActionCode As String
Private CommentText As String
Private VarTxt As String
Private OtherReason As String
Private ResultInd As String

Private AN As String
Private LastName As String
Private MI As String
Private FirstName As String
Private Address1 As String
Private Address2 As String
Private City As String
Private State As String
Private ZIP As String
Private Country As String
Private Keyline As String
Private UserID As String

'print a DPA confirmation letter
Sub Conf()
    Common.ResetPublicVars
    
    docFolder = "X:\PADD\DPA\"
    SP.TestMode , docFolder
    
    If SP.Common.CalledByMauiDUDE(SSN) = False Then
        'prompt the user for the SSN, print the letters and end the script if the dialog box is canceled
        SSN = InputBox("Enter an SSN or account number, or click Cancel to quit.", "DPA Letters", "Enter SSN or AN")
        If SSN = "" Then
            End
        End If
        If SSN = "Enter SSN or AN" Then
            SSN = GetText(3, 23, 9)
        End If
    End If
    'Restart the script if the user didn't input what looks like an SSN or AN.
    If Not IsNumeric(SSN) Or (Len(SSN) <> 9 And Len(SSN) <> 10) Then
        Conf
    End If
    'Make sure we have both the SSN and AN.
    If Len(SSN) = 10 Then
        AN = SSN
        FastPath "LP22I;"
        PutText 6, 33, AN, "ENTER"
        SSN = GetText(3, 23, 9)
    Else
        FastPath "LP22I" & SSN
        AN = Replace(GetText(3, 60, 12), " ", "")
    End If
    'warn the user if the borrower was not found
    If Not Check4Text(1, 62, "PERSON DEMOGRAPHICS") Then
        MsgBox "The SSN or account number entered was not valid.", vbExclamation, "Borrower not Found"
        Conf
    End If
    'prompt the user for a comaker SSN
    CSSN = InputBox("Enter the comaker's SSN if applicable.", "Comaker SSN", "Enter Comaker SSN")
    If CSSN = "" Then
        Conf
    End If
    If CSSN = "Enter Comaker SSN" Then
        CSSN = ""
    End If
    
    'access LC05
    FastPath "LC05I" & SSN
    'prompt the user for the drawdate, prompt the user to reenter the draw date if it is not within 60 days
    Do
        DrawDate = InputBox("Enter the first draw date as either the 7th, 15th, or 22nd in MMDDYYYY or MM/DD/YYYY format.", "Draw Date")
        If DrawDate = "" Then Conf
        If InStr(1, DrawDate, "/", 1) <> 3 Then DrawDate = Format(DrawDate, "##/##/####")
        If IsDate(DrawDate) = True Then
            If Day(DrawDate) <> 7 And Day(DrawDate) <> 15 And Day(DrawDate) <> 22 Then
                warn = MsgBox("The first draw date must be either the 7th, 15th or the 22nd in MMDDYYYY or MM/DD/YYYY format.  Click OK to reenter the date.", vbOKOnly, "Invalid Draw Date")
            ElseIf DateValue(DrawDate) > Date + 61 Or DateValue(DrawDate) < Date Then
                warn = MsgBox("The draw date must not be a past date or more than 60 days in the future.  Click OK to reenter the date.", vbOKOnly, "Invalid Draw Date")
            Else
                Exit Do
            End If
        Else
            warn = MsgBox("The date entered is not a valid date.  Click OK to reenter the date.", vbOKOnly, "Invalid Draw Date")
        End If
    Loop
    ltr = ltr + 1
    DPInd = "Y"
    'go to the LC05 subroutine to get loan information
    LC05
    Hit "F12"
    'prompt the user for the draw amount, prompt the user to reenter the draw amount if it is not greater than or equal to the expected payment amount
    Do
        warn = InputBox("Update the draw amount if the correct amount is not displayed.", "Draw Amount", ExpPmt)
        If warn = "" Then Conf
        If Val(warn) >= ExpPmt Then Exit Do
        warn = MsgBox("The draw amount cannot be less than the sum of the expected payment amounts for all open loans.", vbOKOnly, "Invalid Draw Amount")
    Loop
    BilledAmt = Val(warn)
    'go tot LC34 to update the loans
    LC34
    'set the activity record codes and go to the Common.LP50 subroutine to add an activity record
    Common.Script = "  {DPALETTERS}"
    Common.ActCd = "DRNDP"
    Common.ConTyp = "03"
    Common.ActTyp = "LT"
    Common.Comment = "sent dpa confirmation letter to borr"
    Common.Comment = "sent dpa confirmation letter to borr, first draw on " & DrawDate
    Common.LP50
    'get demographic information
    SP.Common.GetLP22 SSN, AN, LastName, MI, FirstName, Address1, Address2, City, State, ZIP, Country
    Keyline = SP.ACSKeyLine(SSN)
    'determine the draw day
    If Day(DrawDate) = 7 Then
        drawday = "7th"
    ElseIf Day(DrawDate) = 15 Then
        drawday = "15th"
    Else
        drawday = "22nd"
    End If
    'Write a merge file and print the letter.
    UserID = SP.Common.GetUserID
    Open "T:\dpadat.txt" For Output As #1
    Write #1, "SSN", "LastName", "FirstName", "Address1", "Address2", "City", "State", "Country", "ZIP", "BilledAmt", "DrawDate", "DrawDay", "Keyline", "AccountNumber"
    Write #1, Format(SSN, "@@@-@@-@@@@"), LastName, FirstName, Address1, Address2, City, State, Country, ZIP, Format(BilledAmt, "###,##0.00"), Format(DrawDate, "mmmm dd, yyyy"), drawday, Keyline, AN
    Close #1
    SP.DocCreateAndDeploy.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt "DPACONF", stacOneLINK, SSN, "T:\dpadat.txt", "AccountNumber", "DPACONF", docFolder, UserID, "DPALETTERS", "State", dmUserPrompt
    If Dir("T:\dpadat.txt") <> "" Then Kill "T:\dpadat.txt"
    'go back for another SSN
'    Conf
End Sub

'print a DPA cancellation letter
Sub Canc()
    Common.ResetPublicVars
    docFolder = "X:\PADD\DPA\"
    SP.Common.TestMode , docFolder
    clickedOk = False
    
    'prompt the user for the SSN and type of cancellation
    Load DPACncl
    Dim SSN As String
    If SP.CalledByMauiDUDE(SSN) Then
        DPACncl.txtSSN.Text = SSN
    End If
    DPACncl.Show
    'gather values from dialog box variables
    SSN = DPACncl.txtSSN
    CSSN = DPACncl.txtCSSN
    OtherReason = DPACncl.txtOtherReason
    Unload DPACncl
    If SP.Common.CalledByMauiDUDE(SSN) = False Then
        SSN = GetText(3, 23, 9)
    End If
    DrawDate = ""
    'Make sure we have both the SSN and AN.
    If Len(SSN) = 10 Then
        AN = SSN
        FastPath "LP22I;"
        PutText 6, 33, AN, "ENTER"
        SSN = GetText(3, 23, 9)
    Else
        FastPath "LP22I" & SSN
        AN = Replace(GetText(3, 60, 12), " ", "")
    End If
    'warn the user if the borrower was not found
    If Not Check4Text(1, 62, "PERSON DEMOGRAPHICS") Then
        MsgBox "The SSN or account number entered was not valid.", vbExclamation, "Borrower not Found"
        Conf
    End If
    'set the comment for "other"
    If OtherReason <> "" Then
        Common.ActCd = "DLUDC"
        Common.Comment = OtherReason & ", removed DPA and sent ltr to borr"
        VarTxt = OtherReason
        doc = "DPACANO"
    End If
    'access LC05
    FastPath "LC05I" & SSN
    DPInd = "N"
    'go to the LC05 subroutine to get loan information
    LC05
    'warn the user and end the script if the letter chosed does not correspond to the aux status of the loan with the greatest status date
    If doc = "DPACANP" And lnsta = "O" Then
        warn = MsgBox("A PIF, rehabilitation, or consolidation cancellation letter cannot be generated for the borrower because not all of the borrower's loans are closed.", vbOKOnly, "Open Loans")
        Canc
    End If
    'go to the LC34 subroutine to update the loans if there are open loans
    If lnsta = "O" Then
        LC34
    End If
    ltr = ltr + 1
    'set the activity record codes and go to the Common.LP50 subroutine to add an activity record
    Common.Script = "  {DPALETTERS}"
    Common.doc = doc
    Common.ConTyp = "03"
    Common.ActTyp = "LT"
    Common.LP50
    
    'get demographic information
    SP.Common.GetLP22 SSN, AN, LastName, MI, FirstName, Address1, Address2, City, State, ZIP, Country
    Keyline = SP.ACSKeyLine(SSN)

    'Write the merge files and print the letter.
    UserID = SP.Common.GetUserID
    If doc = "DPACANP" Then
        Open "T:\dpapdat.txt" For Output As #1
        Write #1, "SSN", "LastName", "FirstName", "Address1", "Address2", "City", "State", "Country", "ZIP", "VarTxt", "Keyline", "AccountNumber"
        Write #1, Format(SSN, "@@@-@@-@@@@"), LastName, FirstName, Address1, Address2, City, State, Country, ZIP, VarTxt, Keyline, AN
        Close #1
        SP.DocCreateAndDeploy.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt "DPACANP", stacOneLINK, SSN, "T:\dpapdat.txt", "AccountNumber", "DPACANP", docFolder, UserID, "DPALETTERS", "State"
    Else
        Open "T:\dpaodat.txt" For Output As #1
        Write #1, "SSN", "LastName", "FirstName", "Address1", "Address2", "City", "State", "Country", "ZIP", "VarTxt", "Keyline", "AccountNumber"
        Write #1, Format(SSN, "@@@-@@-@@@@"), LastName, FirstName, Address1, Address2, City, State, Country, ZIP, VarTxt, Keyline, AN
        Close #1
        SP.DocCreateAndDeploy.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt "DPACANO", stacOneLINK, SSN, "T:\dpaodat.txt", "AccountNumber", "DPACANO", docFolder, UserID, "DPALETTERS", "State"
    End If
    If Dir("T:\dpadat.txt") <> "" Then Kill "T:\dpadat.txt"
    'go back for another SSN
    Canc
End Sub

'gather loan information
Sub LC05()
    Dim StatusDate As String
    Dim Aux As String
    
    StatusDate = "01/01/1900"
    Aux = ""
    ExpPmt = 0
    'access the first loan
    Session.TransmitANSI "01"
    Hit "ENTER"
    lnsta = ""
    seventh = ""
    Loan = 0
    'look at each loan
    Do While Not Check4Text(22, 3, "46004")
        If Check4Text(4, 10, "04") = False And Check4Text(19, 73, "MMDDCCYY") = True Then lnsta = "O"
        'set the due date
        If DrawDate <> "" Then
            If Check4Text(10, 75, "DD") Then
                If Day(DrawDate) = 7 Then
                    If Day(Date) > 21 Then
                        seventh = Format(DateValue(Month(DateAdd("m", 2, Date)) & "/07/" & Year(DateAdd("m", 2, Date))), "MMDDYYYY")
                    Else
                        seventh = Format(DateValue(Month(DateAdd("m", 1, Date)) & "/07/" & Year(DateAdd("m", 1, Date))), "MMDDYYYY")
                    End If
                ElseIf Day(DrawDate) = 15 Then
                    seventh = Format(DateValue(Month(DateAdd("m", 1, Date)) & "/15/" & Year(DateAdd("m", 1, Date))), "MMDDYYYY")
                Else
                    If Day(Date) < 7 Then
                        seventh = Format(DateValue(DrawDate), "MMDDYYYY")
                    Else
                        seventh = Format(DateValue(Month(DateAdd("m", 1, Date)) & "/22/" & Year(DateAdd("m", 1, Date))), "MMDDYYYY")
                    End If
                End If
            ElseIf Val(GetText(10, 75, 2)) <> Day(DrawDate) Then
                If Day(DrawDate) = 7 Then
                    If Day(Date) > 21 Then
                        seventh = Format(DateValue(Month(DateAdd("m", 2, Date)) & "/07/" & Year(DateAdd("m", 2, Date))), "MMDDYYYY")
                    Else
                        seventh = Format(DateValue(Month(DateAdd("m", 1, Date)) & "/07/" & Year(DateAdd("m", 1, Date))), "MMDDYYYY")
                    End If
                ElseIf Day(DrawDate) = 15 Then
                    seventh = Format(DateValue(Month(DateAdd("m", 1, Date)) & "/15/" & Year(DateAdd("m", 1, Date))), "MMDDYYYY")
                Else
                    If Day(Date) < 7 Then
                        seventh = Format(DateValue(DrawDate), "MMDDYYYY")
                    Else
                        seventh = Format(DateValue(Month(DateAdd("m", 1, Date)) & "/22/" & Year(DateAdd("m", 1, Date))), "MMDDYYYY")
                    End If
                End If
            End If
        End If
        'get the claim ID to update the loan on LC34 if the loan is open
        If Check4Text(4, 10, "03") And Check4Text(19, 73, "MMDDCCYY") Then
            'verify the loan is for the comaker if a comaker SSN was entered
            If CSSN <> "" Then
                Hit "F10"
                If Check4Text(14, 54, CSSN) Then
                    Hit "F10"
                    Hit "F10"
                    Loan = Loan + 1
                    ClaimID(Loan) = GetText(21, 11, 4)
                    ExpPmt = Val(GetText(11, 71, 10)) + ExpPmt
                End If
            Else
                Loan = Loan + 1
                ClaimID(Loan) = GetText(21, 11, 4)
                ExpPmt = Val(GetText(11, 71, 10)) + ExpPmt
            End If
        End If
        
        'set the status date if the status date of the loan is more recent than the previous status date and get the loan status
        If Check4Text(19, 73, "MMDDCCYY") Then
            If DateValue(Format(GetText(5, 13, 8), "##/##/####")) > DateValue(StatusDate) Then
                StatusDate = DateValue(Format(GetText(5, 13, 8), "##/##/####"))
                Aux = GetText(4, 26, 2)
            End If
        Else
            If DateValue(Format(GetText(19, 73, 8), "##/##/####")) > DateValue(StatusDate) Then
                StatusDate = DateValue(Format(GetText(19, 73, 8), "##/##/####"))
                Aux = GetText(4, 26, 2)
            End If
        End If
        'go to the next loan
        Hit "F8"
    Loop
    'warn the user and go back for another SSN if an open letter was chosed but the borrower has no open loans
    If Loan = 0 And DPInd <> "N" Then
        warn = MsgBox("The borrower does not have any loans eligible for DPA.", vbOKOnly, "No Eligible Loans")
        Canc
    End If
    'warn the user and go back for another SSN if the loan status of the loan with the most recent status date does not correspond to the letter chosen
    If (ResultInd = "P" And Aux <> "") Or (ResultInd = "R" And Aux <> "10") Or (ResultInd = "C" And Aux <> "11" And Aux <> "12") Then
        warn = MsgBox("A PIF, rehabilitation, or consolidation cancellation letter cannot be generated for the borrower because the loan status of the loan with the most recent status date does not correspond to the letter chosen.", vbOKOnly, "Open Loans")
        Canc
    End If
End Sub

'update the loans
Sub LC34()
    Dim Row As Integer
    Dim counter As Integer
    
    'access LC34
    FastPath "LC34C" & SSN & "01"
    'enter the due date if the current due date is not the 7th
    If seventh <> "" Then
        PutText 4, 42, seventh
    End If
    'enter the DPA indicator (Y or N)
    PutText 6, 40, DPInd
    'select the loans
    Row = 9
    Do
        counter = 1
        Do
            If Check4Text(Row, 5, ClaimID(counter)) Then
                PutText Row, 3, "X"
                'exit the processing loop to process the next loan
                Exit Do
            End If
            counter = counter + 1
        Loop Until counter > Loan
        Row = Row + 1
        'if the row is blank, forward to the next page, if no more pages or an error are displayed, stop processing
        If Check4Text(Row, 3, " ") Then
            Hit "F8"
            If Not Check4Text(22, 3, "     ") Then
                Exit Do
            Else
                Row = 9
            End If
        End If
    Loop
    Hit "ENTER"
    If Check4Text(22, 3, "49000") = False And _
       Check4Text(22, 3, "40238") = False And _
       Check4Text(22, 3, "40239") = False Then
        warn = MsgBox("The loans were not updated.  Wait for the script to end and update the direct payment indicator manually.", vbOKOnly, "Loans not Updated")
    End If
End Sub

'Accessor methods needed by the DPACncl form:
Public Sub setOK(ByVal Clicked As Boolean)
    clickedOk = Clicked
End Sub
Public Sub setActionCode(ByVal Code As String)
    ActionCode = Code
End Sub
Public Sub setCommentText(ByVal Text As String)
    CommentText = Text
End Sub
Public Sub setVarTxt(ByVal Text As String)
    VarTxt = Txt
End Sub
Public Sub setDoc(ByVal DocID As String)
    doc = DocID
End Sub
Public Sub setOtherReason(ByVal reason As String)
    OtherReason = reason
End Sub
Public Sub setResultInd(ByVal Ind As String)
    ResultInd = Ind
End Sub
