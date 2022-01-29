

Private Sub btnOK_Click()
    Dim AccNum As String
    Dim SSN As String
    Dim i As Integer
    Dim KeyLine As String
    Dim FN As String
    Dim LN As String
    Dim Addr1 As String
    Dim Addr2 As String
    Dim Addr3 As String
    Dim City As String
    Dim State As String
    Dim ZIP As String
    Dim Country As String
    Dim UserID As String
    Dim LtrDir As String
    
    LtrDir = "X:\PADD\BorrowerServices\"
    sp.QL.GatherIDPass
    sp.TestMode , LtrDir
    
    'do data validation
    If tbSSN.TextLength < 9 Then
        MsgBox "A ten digit account number or nine digit SSN is required."
        Exit Sub
    ElseIf IsNumeric(tbSSN.Text) = False Then
        MsgBox "The Acct Num/SSN must be numeric."
        Exit Sub
    ElseIf tbOldAccNum.TextLength <> 10 Then
        MsgBox "A ten digit old account number is required."
        Exit Sub
    ElseIf tbNewAccNum.TextLength <> 10 Then
        MsgBox "A ten digit new account number is required."
        Exit Sub
    End If
    Me.Hide
    UserID = sp.Common.GetUserID()
    'figure out what was provided, SSN or account number
    If tbSSN.TextLength = 9 Then
        SSN = tbSSN.Text
    Else
        AccNum = tbSSN.Text
    End If
    'if the script makes it this far then all data is valid
    If sp.Common.GetLP22(SSN, AccNum, LN, , FN, Addr1, Addr2, City, State, ZIP, Country) Then
        'if the borrower is on LP22
        sp.Common.AddLP50 SSN, "MSACC", "ACNTNOCHNG", "AM", "10", "Account number changed from " & tbOldAccNum.Text & " to " & tbNewAccNum.Text & ".  MLTACCT letter sent to borrower."
    Else
        'if the borrower isn't on LP22
        If GetSSNFromITS24(tbSSN.Text, AccNum, SSN) Then
            sp.Common.GetTX1J SSN, AccNum, LN, , FN, Addr1, Addr2, Addr3, City, State, ZIP, Country
            'if the borrower is on TX1J
            sp.Common.ATD22AllLoans SSN, "MSACC", "Account number changed from " & tbOldAccNum.Text & " to " & tbNewAccNum.Text & ".  MLTACCT letter sent to borrower.", "ACNTNOCHNG", UserID
        Else
            sp.QL.ToLCO
            'if the borrower isn't on TX1J
            If sp.Common.GetTPDD(tbSSN.Text, AccNum, LN, , FN, Addr1, Addr2, Addr3, City, State, ZIP, Country) Then
                'if the borrower was found on LCO
                'get SSN from TPDD
                SSN = Replace(GetText(3, 19, 11), "-", "")
                'add comment to UPUR
                FastPath "UPUR" & SSN
                'find right screen
                While Check4Text(1, 21, "** PURSUIT ACTIVITY - RECORD ADDITION **") = False
                    Hit "UP"
                Wend
                PutText 3, 29, Mid(UserID, 5, 3)
                PutText 6, 29, "215"
                PutText 6, 35, "Account number changed from " & tbOldAccNum.Text & " to " & tbNewAccNum.Text & ".  MLTACCT letter sent to borrower.", "ENTER"
            Else
                'if the borrower wasn't found on LCO
                sp.QL.ToCO
                MsgBox "The entered SSN wasn't found.  Please enter a valid SSN."
                Me.Show
                Exit Sub
            End If
            sp.QL.ToCO
        End If
    End If
    'create letter
    Open "T:\MLTACCT Dat.txt" For Output As #1
    'header row
    Write #1, "KeyLine", "FirstName", "LastName", "Address1", "Address2", "Address3", "City", "State", "ZIP", "Country", "Acct No", "NewAccountNum"
    'data row
    Write #1, sp.Common.ACSKeyLine(SSN), FN, LN, Addr1, Addr2, Addr3, City, State, ZIP, Country, tbNewAccNum.Text, tbNewAccNum.Text
    Close #1
    'print letter
    sp.DocCreateAndDeploy.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt "MLTACCT", stacBoth, SSN, "T:\MLTACCT Dat.txt", "Acct No", "MLTACCT", LtrDir, UserID, "ACNTNOCHNG", "State", dmLetter
'    If TestMode() Then
'        SP.Common.PrintDocs "X:\PADD\BorrowerServices\Test\", "MLTACCT", "T:\MLTACCT Dat.txt"
'    Else
'        SP.Common.PrintDocs "X:\PADD\BorrowerServices\", "MLTACCT", "T:\MLTACCT Dat.txt"
'    End If
        
    'delete File
    Kill "T:\MLTACCT Dat.txt"
    
    'blank all controls
    While i < Me.Controls.count
        If TypeName(Me.Controls(i)) = "TextBox" Then
            Me.Controls(i).Text = ""
        End If
        i = i + 1
    Wend
    Me.Show
End Sub

'checks if the borrower is on ITS24 and does SSN translation if they are.
Private Function GetSSNFromITS24(TheUnknownValue As String, AccountNum As String, SSN As String) As Boolean
    FastPath "TX3ZITS24" & TheUnknownValue
    If Check4Text(1, 76, "TSX25") Then
        GetSSNFromITS24 = True
        SSN = GetText(1, 9, 9)
        AccountNum = Replace(GetText(6, 10, 12), " ", "")
    Else
        GetSSNFromITS24 = False
        SSN = ""
        AccountNum = ""
    End If
End Function

Private Sub tbSSN_Exit(ByVal Cancel As MSForms.ReturnBoolean)
    If tbSSN.TextLength = 10 Then
        tbOldAccNum.Text = tbSSN.Text
    End If
End Sub

Private Sub UserForm_Terminate()
    End
End Sub

'new sr962, aa, 02/23/05, 03/02/05
