VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} SchDemoRun 
   Caption         =   "Run Updates"
   ClientHeight    =   4485
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5220
   OleObjectBlob   =   "SchDemoRun.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "SchDemoRun"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private UpdateMode As String
Private NameUpdated As Boolean
Private AddUpdated As Boolean
Private PhnUpdated As Boolean
Private FaxUpdated As Boolean
Private ContactUpdated As Boolean
Private SI(0 To 26) As String
'    (0) = school name, (1) = school code, (2) = OneLINK dept, (3) = Compass dept
'    (4) = address 1, (5) = address 2, (6) = address 3, (7) = city, (8) = state,
'    (9) = zip, (10) = foreign state, (11) = country, (12) = domestic phone,
'    (13) = domestic extension, (14) = domestic fax, (15) = phone IC,
'    (16) = phone cny, (17) = phone cty, (18) = phn local,
'    (19) = foreign extension, (20) = fax IC, (21) = fax cny, (22) = fax cty,
'    (23) = fax local, (24) contact first name, (25) = contact last name,
'    (26) = submitted date
Private Depts As String

'run updates
Private Sub CommandButton1_Click()
    Dim DataLine As String
    
    'warn the user and return to the form if the input file is empty
    If FileLen("C:\Windows\Temp\SchoolUpdates.txt") = 0 Then
        MsgBox "The SchoolUpdates.txt file is empty.  There are no updates to process.", 16, "No Updates to Process"
        Exit Sub
    End If
                      
    'hide form
    Me.Hide
    
    'read values from input file
    Open "C:\Windows\Temp\SchoolUpdates.txt" For Input As #1
    While Not EOF(1)
        'input data
        Input #1, SI(0), SI(1), SI(2), SI(3), SI(4), SI(5), SI(6), SI(7), SI(8), _
                  SI(9), SI(10), SI(11), SI(12), SI(13), SI(14), SI(15), SI(16), _
                  SI(17), SI(18), SI(19), SI(20), SI(21), SI(22), SI(23), SI(24), _
                  SI(25), SI(26)
                  
    'Update OneLINK
        ClearValues
        'update all depts or specified department
        If SI(2) = "ALL" Then
            UpdateOneLINK "GEN", False
            UpdateOneLINK "110", False
            UpdateOneLINK "112", False
            UpdateOneLINK "111", False
        Else
            UpdateOneLINK SI(2), True
        End If
        'add activity record if something changed
        If NameUpdated Or AddUpdated Or PhnUpdated Or FaxUpdated Then
            FastPath "LP54A" & SI(1) & ";001;;;;QMADD"
            PutText 7, 2, "MS95"
            PutText 11, 2, UpdateMode & " " & UpdatedItems & " for dept(s) " & Depts & "  {SCHDEMOUP}", "F6"
        End If
        
    'Update COMPASS
        ClearValues
        'update all depts or specified department
        If SI(3) = "ALL" Then
            UpdateCOMPASS "000", False
            UpdateCOMPASS "004", False
            UpdateCOMPASS "001", False
            UpdateCOMPASS "003", False
        Else
            UpdateCOMPASS SI(3), True
        End If
        'add activity record if something changed
        If NameUpdated Or AddUpdated Or PhnUpdated Or FaxUpdated Or ContactUpdated Then
            FastPath "TX3Z/APO2X04" & SI(1)
            PutText 8, 18, "0530020"
            PutText 14, 2, UpdateMode & " " & UpdatedItems & " for dept(s) " & Depts & "  {SCHDEMOUP}", "Enter"
        End If
    Wend
    'close the data file an display the dialog box
    Close #1
    'warn the user that updates are complete
    MsgBox "OneLINK and COMPASS have been successfully updated.", 64, "Updates Complete"
    'return to the run updates form
    Me.Show
End Sub

'login to live
Private Sub CommandButton2_Click()
    Me.Hide
    Logon "Live"
    Me.Show
End Sub

'login to test
Private Sub CommandButton3_Click()
    Me.Hide
    Logon "Test"
    Me.Show
End Sub

'purge the data file
Private Sub CommandButton4_Click()
    'warn the user and purge the file if Yes is clicked
    If MsgBox("Are you sure you want to purge the SchoolUpdates.txt file?", 36, "Purge File") = 6 Then
        Open "C:\Windows\Temp\SchoolUpdates.txt" For Output As #1
        Close #1
    End If
End Sub

'end the script
Private Sub CommandButton5_Click()
    Me.Hide
    End
End Sub

'update OneLINK
Private Function UpdateOneLINK(Dept As String, AddDept As Boolean)
    Dim FgnPhn As String
    Dim FgnFax As String
    
    'acess LPSC
    FastPath "LPSCC" & SI(1) & Dept
    
    'switch to add mode and update text of update mode for comment if the dept isn't found
    If Check4Text(1, 55, "INSTITUTION NAME/ID SEARCH") Then
        'exit the function if the dept should not be added
        If Not AddDept Then Exit Function
        If UpdateMode = "" Then
            UpdateMode = "added"
        ElseIf UpdateMode = "changed" Then
            UpdateMode = "added/changed"
        End If
        PutText 1, 7, "A", "ENTER"
    'update text of update mode for comment
    Else
        If UpdateMode = "" Then
            UpdateMode = "changed"
        ElseIf UpdateMode = "added" Then
            UpdateMode = "added/changed"
        End If
    End If
    
    'compile fragmented values
    FgnPhn = SI(15) & SI(16) & SI(17) & SI(18)
    FgnFax = SI(20) & SI(21) & SI(22) & SI(23)
    
    'update the indicators for the items updated
    If GetText_(8, 21, 40) <> SI(4) Or _
       GetText_(9, 21, 40) <> SI(5) Or _
       GetText_(10, 21, 40) <> SI(6) Or _
       GetText_(11, 21, 30) <> SI(7) Or _
       (GetText_(11, 59, 2) <> SI(8) And (GetText_(13, 23, 2) <> "FC" And SI(8) <> "")) Or _
       GetText_(11, 66, 17) <> SI(9) Or _
       GetText_(12, 21, 15) <> SI(10) Or _
       GetText_(12, 55, 25) <> SI(11) Then AddUpdated = True
'<2>
    If GetText_(15, 19, 10) <> SI(12) Or _
       (GetText_(15, 34, 4) <> SI(13) And GetText_(15, 34, 4) <> SI(19)) Or _
       GetText_(16, 19, 17) <> FgnPhn Then PhnUpdated = True
'   If GetText_(16, 19, 10) <> SI(12) Or _
'      (GetText_(16, 34, 4) <> SI(13) And GetText_(16, 34, 4) <> SI(19)) Or _
'      GetText_(17, 19, 17) <> FgnPhn Then PhnUpdated = True

'    If GetText_(16, 70, 10) <> SI(14) Or _
'       GetText_(17, 63, 17) <> FgnFax Then FaxUpdated = True
    If GetText_(15, 70, 10) <> SI(14) Or _
       GetText_(16, 63, 17) <> FgnFax Then FaxUpdated = True
'</2>
    'update the fields if they are different than what was entered
    If GetText_(8, 21, 40) <> SI(4) Then PutText 8, 21, SI(4), "END" 'add 1
    If GetText_(9, 21, 40) <> SI(5) Then PutText 9, 21, SI(5), "END" 'add 2
    If GetText_(10, 21, 40) <> SI(6) Then PutText 10, 21, SI(6), "END" 'add 3
    If GetText_(11, 21, 30) <> SI(7) Then PutText 11, 21, SI(7), "END" 'city
    If GetText_(11, 59, 2) <> SI(8) Then PutText 11, 59, SI(8), "END" 'state
    If SI(10) <> "" Or SI(11) <> "" Or SI(19) <> "" Or FgnPhn <> "" Or FgnFax <> "" Then PutText 11, 59, "FC" 'enter FC as dom state if there is foreign address info
    If GetText_(11, 66, 17) <> SI(9) Then PutText 11, 66, SI(9), "END" 'zip
    If GetText_(12, 21, 15) <> SI(10) Then PutText 12, 21, SI(10), "END" 'fgn st
    If GetText_(12, 55, 25) <> SI(11) Then
        PutText 12, 55, SI(11) 'fgn cny
        If Len(SI(11)) < 25 Then Hit "END"
    End If
'<2>
'    If SI(12) = "" Then 'blank dom phone
'        PutText 16, 19, "", "END"
'    ElseIf GetText_(16, 19, 10) <> SI(12) Then 'update dom phone
'        PutText 16, 19, SI(12)
'    End If
'    If SI(13) <> "" Then
'        If GetText_(16, 34, 4) <> SI(13) Then PutText 16, 34, SI(13), "END" 'dom ext
'    ElseIf SI(19) <> "" Then
'        If GetText_(16, 34, 4) <> SI(19) Then PutText 16, 34, SI(19), "END" 'fgn ext
'    Else
'        PutText 16, 34, "", "END" 'blank extension
'    End If
'    If SI(14) = "" Then 'blank dom fax
'        PutText 16, 70, "", "END"
'    ElseIf GetText_(16, 70, 10) <> SI(14) Then 'update dom fax
'        PutText 16, 70, SI(14)
'    End If
'    If GetText_(17, 19, 17) <> FgnPhn Then PutText 17, 19, FgnPhn, "END" 'fgn phn
'    If GetText_(17, 63, 17) <> FgnFax Then
'        PutText 17, 63, FgnFax 'fgn fax
'        If Len(FgnFax) < 17 Then Hit "END"
'    End If
    If SI(12) = "" Then 'blank dom phone
        PutText 15, 19, "", "END"
    ElseIf GetText_(15, 19, 10) <> SI(12) Then 'update dom phone
        PutText 15, 19, SI(12)
    End If
    If SI(13) <> "" Then
        If GetText_(15, 34, 4) <> SI(13) Then PutText 15, 34, SI(13), "END" 'dom ext
    ElseIf SI(19) <> "" Then
        If GetText_(15, 34, 4) <> SI(19) Then PutText 15, 34, SI(19), "END" 'fgn ext
    Else
        PutText 15, 34, "", "END" 'blank extension
    End If
    If SI(14) = "" Then 'blank dom fax
        PutText 15, 70, "", "END"
    ElseIf GetText_(15, 70, 10) <> SI(14) Then 'update dom fax
        PutText 15, 70, SI(14)
    End If
    If GetText_(16, 19, 17) <> FgnPhn Then PutText 16, 19, FgnPhn, "END" 'fgn phn
    If GetText_(16, 63, 17) <> FgnFax Then
        PutText 16, 63, FgnFax 'fgn fax
        If Len(FgnFax) < 17 Then Hit "END"
    End If
'</2>

    'update school name if it has changed
    If Not Check4Text(5, 21, SI(0)) And UpdateMode = "changed" Then
        NameUpdated = True
        'update the name if already on the general dept screen
        If Check4Text(7, 15, "GEN") Then
            PutText 5, 21, SI(0)
            If Len(SI(0)) < 40 Then Hit "END"
            VerifyUpdate "OL"
        'access the gen dept screen and update name
        Else
            VerifyUpdate "OL"
            FastPath "LPSCC" & SI(1) & "GEN"
            PutText 5, 21, SI(0)
            If Len(SI(0)) < 40 Then Hit "END"
            VerifyUpdate "OL"
        End If
    'commit and verify changes if the school name hasn't changed but something did
    ElseIf AddUpdated Or PhnUpdated Or FaxUpdated Then
        VerifyUpdate "OL"
        If Depts = "" Then Depts = Dept Else Depts = Depts & ", " & Dept
    End If
End Function

'update Compass
Private Function UpdateCOMPASS(Dept As String, AddDept As Boolean)
    Dim Phn As String
    Dim Fax As String
    Dim FgnPhn As String
    Dim FgnFax As String
    Dim scrFgnPhn As String
    Dim scrFgnFax As String
    
    'access TX0Y
    FastPath "TX3Z/CTX0Y" & SI(1) & Dept
    
    'switch to add mode and update text of update mode for comment if the dept isn't found
    If Check4Text(1, 73, "TXX00") Then
        'exit the function if the dept should not be added
        If Not AddDept Then Exit Function
        If UpdateMode = "" Then
            UpdateMode = "added"
        ElseIf UpdateMode = "changed" Then
            UpdateMode = "added/changed"
        End If
        PutText 1, 4, "A", "ENTER"
    'update text of update mode for comment
    Else
        If UpdateMode = "" Then
            UpdateMode = "changed"
        ElseIf UpdateMode = "added" Then
            UpdateMode = "added/changed"
        End If
    End If
    
    'compile fragmented values
    Phn = GetText_(18, 20, 3) & GetText_(18, 26, 3) & GetText_(18, 30, 4)
    Fax = GetText_(19, 20, 3) & GetText_(19, 26, 3) & GetText_(19, 30, 4)
    FgnPhn = SI(15) & SI(16) & SI(17) & SI(18)
    FgnFax = SI(20) & SI(21) & SI(22) & SI(23)
    scrFgnPhn = GetText_(20, 21, 3) & GetText_(20, 34, 3) & GetText_(20, 44, 4) & GetText_(20, 56, 7)
    scrFgnFax = GetText_(21, 21, 3) & GetText_(21, 34, 3) & GetText_(21, 44, 4) & GetText_(21, 56, 7)
    
    'update the indicators for the items updated
'    (0) = school name, (1) = school code, (2) = OneLINK dept, (3) = Compass dept
'    (4) = address 1, (5) = address 2, (6) = address 3, (7) = city, (8) = state,
'    (9) = zip, (10) = foreign state, (11) = country, (12) = domestic phone,
'    (13) = domestic extension, (14) = domestic fax, (15) = phone IC,
'    (16) = phone cny, (17) = phone cty, (18) = phn local,
'    (19) = foreign extension, (20) = fax IC, (21) = fax cny, (22) = fax cty,
'    (23) = fax local, (24) contact first name, (25) = contact last name,
'    (26) = submitted date

'<4>
'    If GetText_(9, 23, 30) <> SI(4) Or _
'       GetText_(10, 23, 30) <> SI(5) Or _
'       GetText_(11, 23, 30) <> SI(6) Or _
'       GetText_(12, 23, 20) <> SI(7) Or _
'       GetText_(13, 23, 2) <> SI(8) Or _
'       GetText_(13, 46, 9) <> SI(9) Or _
'       GetText_(14, 23, 15) <> SI(10) Or _
'       GetText_(14, 51, 25) <> SI(11) Then AddUpdated = True
    If GetText_(11, 23, 30) <> SI(4) Or _
       GetText_(12, 23, 30) <> SI(5) Or _
       GetText_(13, 23, 30) <> SI(6) Or _
       GetText_(14, 13, 20) <> SI(7) Or _
       GetText_(14, 53, 2) <> SI(8) Or _
       GetText_(14, 69, 9) <> SI(9) Or _
       GetText_(15, 21, 15) <> SI(10) Or _
       GetText_(15, 49, 25) <> SI(11) Then AddUpdated = True
'</4>
    If Phn <> SI(12) Or _
       GetText_(18, 43, 4) <> SI(13) Or _
       scrFgnPhn <> FgnPhn Or _
       GetText_(20, 75, 4) <> SI(19) Then PhnUpdated = True
    If Fax <> SI(14) Or scrFgnFax <> FgnFax Then FaxUpdated = True
    If GetText_(15, 32, 10) <> SI(24) Or _
       GetText_(16, 32, 20) <> SI(25) Then ContactUpdated = True
    
    'update the fields if they are different than what was entered
'<4>
'    If GetText_(9, 23, 30) <> SI(4) Then PutText 9, 23, SI(4), "END" 'add 1
'    If GetText_(10, 23, 30) <> SI(5) Then PutText 10, 23, SI(5), "END" 'add 2
'    If GetText_(11, 23, 30) <> SI(6) Then PutText 11, 23, SI(6), "END" 'add 3
    If GetText_(11, 23, 30) <> SI(4) Then PutText 11, 23, SI(4), "END" 'add 1
    If GetText_(12, 23, 30) <> SI(5) Then PutText 12, 23, SI(5), "END" 'add 2
    If GetText_(13, 23, 30) <> SI(6) Then PutText 13, 23, SI(6), "END" 'add 3
'    If GetText_(12, 23, 20) <> SI(7) Then PutText 12, 23, SI(7), "END" 'city
'    If GetText_(13, 23, 2) <> SI(8) Then PutText 13, 23, SI(8), "END" 'state
'    If GetText_(13, 46, 9) <> SI(9) Then PutText 13, 46, SI(9), "END" 'zip
'    If GetText_(14, 23, 15) <> SI(10) Then PutText 14, 23, SI(10), "END" 'fgn st
'    If GetText_(14, 51, 25) <> SI(11) Then PutText 14, 51, SI(11), "END" 'fgn cny
'    If GetText_(15, 32, 10) <> SI(24) Then PutText 15, 32, SI(24), "END" 'contact first name
'    If GetText_(16, 32, 20) <> SI(25) Then
'        PutText 16, 32, SI(25) 'contact last name
'        If Len(SI(25)) < 25 Then Hit "END"
'    End If
    If GetText_(14, 13, 20) <> SI(7) Then PutText 14, 13, SI(7), "END" 'city
    If GetText_(14, 53, 2) <> SI(8) Then PutText 14, 53, SI(8), "END" 'state
    If GetText_(14, 69, 9) <> SI(9) Then PutText 14, 69, SI(9), "END" 'zip
    If GetText_(15, 21, 15) <> SI(10) Then PutText 15, 21, SI(10), "END" 'fgn st
    If GetText_(15, 49, 25) <> SI(11) Then PutText 15, 49, SI(11), "END" 'fgn cny
    If GetText_(16, 31, 10) <> SI(24) Then PutText 16, 31, SI(24), "END" 'contact first name
    If GetText_(16, 56, 20) <> SI(25) Then
        PutText 16, 56, SI(25) 'contact last name
        If Len(SI(25)) < 25 Then Hit "END"
    End If
'</4>
    If SI(12) = "" Then 'blank dom phn
        PutText 18, 20, "", "END"
        PutText 18, 26, "", "END"
        PutText 18, 30, "", "END"
    ElseIf Phn <> SI(12) Then 'update dom phn
        PutText 18, 20, SI(12)
    End If
    If GetText_(18, 43, 4) <> SI(13) Then 'dom ext
        PutText 18, 43, SI(13)
        If Len(SI(13)) < 4 Then Hit "END"
    End If
    If SI(14) = "" Then 'blank dom fax
        PutText 19, 20, "", "END"
        PutText 19, 26, "", "END"
        PutText 19, 30, "", "END"
    ElseIf Fax <> SI(14) Then 'update dom fax
        PutText 19, 20, SI(14)
    End If
    If scrFgnPhn <> FgnPhn Then
        PutText 20, 21, SI(15), "END" 'fgn IC
        PutText 20, 34, SI(16), "END" 'fgn cny
        PutText 20, 44, SI(17), "END" 'fgn cty
        PutText 20, 56, SI(18) 'fgn lcl
        If Len(SI(18)) < 7 Then Hit "END"
    End If
    If GetText_(20, 75, 4) <> SI(19) Then PutText 20, 75, SI(19), "END" 'fgn ext
    If scrFgnFax <> FgnFax Then
        PutText 21, 21, SI(20), "END" 'fgn fax IC
        PutText 21, 34, SI(21), "END" 'fgn fax cny
        PutText 21, 44, SI(22), "END" 'fgn fax cty
        PutText 21, 56, SI(23), "END" 'fgn fax lcl
    End If
    PutText 22, 10, "Y" 'validity
    PutText 22, 31, Format(SI(26), "MMDDYY") 'submitted date
    
    'update school name if it has changed
    If GetText(6, 19, 40) <> SI(0) Then
        NameUpdated = True
        'update the name if already on the general dept screen
        If Check4Text(8, 8, "000") Then
            PutText 6, 19, SI(0)
            If Len(SI(0)) < 40 Then Hit "END"
            VerifyUpdate "CS"
        'access the gen dept screen and update name
        Else
            VerifyUpdate "CS"
            FastPath "TX3Z/CTX0Y" & SI(1) & "000"
            PutText 6, 19, SI(0)
            If Len(SI(0)) < 40 Then Hit "END"
            VerifyUpdate "CS"
        End If
    'commit and verify changes if the school name hasn't changed but something did
    ElseIf AddUpdated Or PhnUpdated Or FaxUpdated Or ContactUpdated Then
        VerifyUpdate "CS"
        If Depts = "" Then Depts = Dept Else Depts = Depts & ", " & Dept
    End If
End Function

'warn the user and pause the script if there are errors
Private Function VerifyUpdate(System As String)
    Hit "ENTER"
    
    'OneLINK
    If System = "OL" Then
        If Not Check4Text(22, 3, "49000") And Not Check4Text(22, 3, "48003") Then
            MsgBox "An error was encountered while changing or adding the record.  Click OK to pause the script, review and correct the information, and hit <Enter> to save the changes.  Hit <Insert> when you are done to resume the script.", 48, "Information not Updated"
            PauseForInsert
        End If
    'COMPASS
    Else
        If Not Check4Text(23, 2, "01005") And Not Check4Text(23, 2, "01004") Then
            MsgBox "An error was encountered while changing or adding the record.  Click OK to pause the script, review and correct the information, and hit <Enter> to save the changes.  Hit <Insert> when you are done to resume the script.", 48, "Information not Updated"
            PauseForInsert
        End If
    End If
End Function

'complile list of items updated
Private Function UpdatedItems() As String
    UpdatedItems = ""

    'name
    If NameUpdated Then UpdatedItems = "name"
    
    'address
    If AddUpdated Then
        If UpdatedItems = "name" Then
            UpdatedItems = "name/address"
        Else
            UpdatedItems = "address"
        End If
    End If
    
    'phone
    If PhnUpdated Then
        If UpdatedItems = "name" Then
            UpdatedItems = "name/phone"
        ElseIf UpdatedItems = "name/address" Then
            UpdatedItems = "name/address/phone"
        ElseIf UpdatedItems = "address" Then
            UpdatedItems = "address/phone"
        Else
            UpdatedItems = "phone"
        End If
    End If
    
    'fax
    If FaxUpdated Then
        If UpdatedItems = "name" Then
            UpdatedItems = "name/fax"
        ElseIf UpdatedItems = "name/address" Then
            UpdatedItems = "name/address/fax"
        ElseIf UpdatedItems = "name/address/phone" Then
            UpdatedItems = "name/address/phone/fax"
        ElseIf UpdatedItems = "address" Then
            UpdatedItems = "address/fax"
        ElseIf UpdatedItems = "address/phone" Then
            UpdatedItems = "address/phone/fax"
        ElseIf UpdatedItems = "phone" Then
            UpdatedItems = "phone/fax"
        Else
            UpdatedItems = "fax"
        End If
    End If
    
    'contact
    If ContactUpdated Then
        If UpdatedItems = "name" Then
            UpdatedItems = "name/contact"
        ElseIf UpdatedItems = "name/address" Then
            UpdatedItems = "name/address/contact"
        ElseIf UpdatedItems = "name/address/phone" Then
            UpdatedItems = "name/address/phone/contact"
        ElseIf UpdatedItems = "name/address/phone/fax" Then
            UpdatedItems = "name/address/phone/fax/contact"
        ElseIf UpdatedItems = "address" Then
            UpdatedItems = "address/contact"
        ElseIf UpdatedItems = "address/phone" Then
            UpdatedItems = "address/phone/contact"
        ElseIf UpdatedItems = "address/phone/fax" Then
            UpdatedItems = "address/phone/fax/contact"
        ElseIf UpdatedItems = "phone" Then
            UpdatedItems = "phone/contact"
        ElseIf UpdatedItems = "phone/fax" Then
            UpdatedItems = "phone/fax/contact"
        ElseIf UpdatedItems = "fax" Then
            UpdatedItems = "fax/contact"
        Else
            UpdatedItems = "contact"
        End If
    End If
End Function

Private Function Logon(LogonMode As String)
    'disconnect and reconnect if the user is connected
    If Not Check4Text(16, 10, ">") Then
        Session.Disconnect
        Session.WaitForEvent rcKbdEnabled, "30", "0", 1, 1
        Session.Connect
    End If
    
    'wait for the logon screen to be displayed
    Session.WaitForDisplayString ">", "0:0:30", 16, 10
    
    'select region
    Select Case LogonMode
        Case "Live"
            PutText 16, 12, "PHEAA", "ENTER"
'<1->
            Session.WaitForDisplayString "USERID", "0:0:30", 20, 8
            PutText 20, 18, "UT00"
'</1>
        Case "Test"
            PutText 16, 12, "QTOR", "ENTER"
'<1->
            Session.WaitForDisplayString "USERID", "0:0:30", 20, 8
            PutText 20, 18, "UT00"
'</1>
    End Select

'<1->
'    'wait for the greetings screen to be displayed
'    Session.WaitForDisplayString "USERID", "0:0:30", 20, 8
'
'    'enter first characters of userID
'    PutText 20, 18, "UT00"
'</1>

    'log on
'<1->
'   Do While Check4Text(20, 8, "USERID")
    Do While Check4Text(20, 8, "USERID") Or Check4Text(8, 22, "USERID")
'</1>
        'wait for user to enter the password
        PauseForInsert
        'enter if the user didn't already
'<1->
'       If Check4Text(20, 8, "USERID") Then
        If Check4Text(20, 8, "USERID") Or Check4Text(8, 22, "USERID") Then
'</1>
            Hit "ENTER"
            'warn the user and exit if the user does not have test region access
            If Check4Text(23, 2, "ON008") Then
                MsgBox "You are not authorized to access the Onelink and COMPASS test regions.  Contact System Operations if you need access to the test regions.", 16, "Test Region Access Not Available"
                Exit Function
            End If
        End If
    Loop
    
    'select regions and other functions
    Select Case LogonMode
        'change screen colors and access user's favorite screen in live
        Case "Live"
            'set the color of updateable fields to green
            Session.SetColorMap rcUnprotNormAlpha, rcGreen, rcBlack
            'access the user's favorite screen
            FastPath "LP00"
        'select the OneLINK and COMPASS test regions
        Case "Test"
            'select the setup/sys ID
            Session.FindText "RS/UT", 3, 5
            PutText Session.FoundTextRow, Session.FoundTextColumn - 2, "X", "ENTER"
            'change the color of updateable fields to magenta
            Session.SetColorMap rcUnprotNormAlpha, rcMagenta, rcBlack
            'access the user's favorite screen
            FastPath "LP00"
            'welcome the user
            MsgBox "Welcome to the OneLINK and COMPASS test regions.", 64, "OneLINK/COMPASS Test"
    End Select
End Function

'get text with underscores removed
Function GetText_(Row As Integer, col As Integer, length As Integer) As String
    GetText_ = Trim(Replace(GetText(Row, col, length), "_", ""))
End Function

'clear values
Private Function ClearValues()
    UpdateMode = ""
    NameUpdated = False
    AddUpdated = False
    PhnUpdated = False
    FaxUpdated = False
    ContactUpdated = False
    Depts = ""
End Function

'<1> sr1094, jd, 05/06/05, 05/06/05
'<3> sr1346, jd, changed field positions for changes to logon screen
