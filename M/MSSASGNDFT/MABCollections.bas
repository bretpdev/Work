Attribute VB_Name = "MABCollections"
Private Qs() As String
Private UserSSNRef() As String

Sub Main()
    Dim Qcount As Integer

    If Not SP.CalledByMBS() Then MsgBox "This script will evaluate and assign accounts to agents in Account Resolution" & vbLf & " based on the last two digits of the last two digits of the borrower's SSN.", vbInformation, "Mass Assign Queue Tasks"
    
    'Get queues from BSYS
    Qs = SP.Common.SQL("SELECT * from MABC_LST_CollQueue") 'NOTE: Array index starts at 1
    
    'Get Users and their associated range of SSNs
    UserSSNRef = SP.Common.SQLEx("SELECT CASE WHEN A.UserID = '' THEN B.MUID ELSE A.UserID END AS UserID, A.SSNRangeBegin, A.SSNRangeEnd FROM MABC_SSN_User_XRef A LEFT OUTER JOIN (SELECT '' as UserID ,UserID AS MUID FROM GENR_REF_BU_Agent_Xref A INNER JOIN SYSA_LST_UserIDInfo B ON A.WindowsUserID = B.WindowsUserName WHERE BusinessUnit = 'Loan Management' AND Role = 'Manager') B ON A.UserID = B.UserID where Dept = 'Account Resolution'")
    Qcount = 1
    Do While Qcount <= UBound(Qs())
        'Process each queue in BSYS
        SP.Q.FastPath "LP8YCDFT;" & Qs(Qcount) & ";;;;A;;"
        If SP.Q.Check4Text(22, 3, "46011") Then
            TaskAssign Qs(Qcount)
        End If
        Qcount = Qcount + 1
    Loop
    
    MsgBox "Processing complete", vbInformation

End Sub

Sub TaskAssign(QueueName As String)
    Dim UserNum As Integer
    Dim PageRec As Integer
    Dim dateStr As String
    Dim newDate As Date
    Do While Not SP.Q.Check4Text(22, 3, "46004")
        For PageRec = 7 To 20
            'get the date information
            dateStr = SP.Q.GetText(PageRec, 26, 2) & "/" & SP.Q.GetText(PageRec, 28, 2) & "/" & SP.Q.GetText(PageRec, 22, 4)
            'If the date is not empty then convert it to a date
            If dateStr <> "" And dateStr <> "//" Then
                newDate = CDate(dateStr)
            End If
            'Assign the task if it fits the right criteria
            If SP.Q.Check4Text(PageRec, 33, "A") And ((newDate >= Date) Or (dateStr = "//")) And _
                (QueueName = "DFOLLOW" Or QueueName = "DPYMTRVW" Or QueueName = "DPYMT" Or QueueName = "DPROMPAY") Then
                UserNum = 0
                Do While UserNum <= UBound(UserSSNRef, 2)
                    If CInt(UserSSNRef(1, UserNum)) <= CInt(SP.Q.GetText(PageRec, 9, 2)) And _
                    CInt(SP.Q.GetText(PageRec, 9, 2)) <= CInt(UserSSNRef(2, UserNum)) Then
                        SP.Q.PutText PageRec, 38, UserSSNRef(0, UserNum)
                        Exit Do
                    End If
                    UserNum = UserNum + 1
                Loop
            ElseIf Check4Text(PageRec, 33, "A") And ((newDate <= Date) Or (dateStr = "//")) Then
                UserNum = 0
                Do While UserNum <= UBound(UserSSNRef, 2)
                    If CInt(UserSSNRef(1, UserNum)) <= CInt(SP.Q.GetText(PageRec, 9, 2)) And _
                    CInt(SP.Q.GetText(PageRec, 9, 2)) <= CInt(UserSSNRef(2, UserNum)) Then
                        SP.Q.PutText PageRec, 38, UserSSNRef(0, UserNum)
                        Exit Do
                    End If
                    UserNum = UserNum + 1
                Loop
            End If
        Next
        SP.Q.Hit "F8"
        If SP.Q.Check4Text(22, 3, "40281") Then
            Exit Do
        End If
    Loop
    SP.Q.Hit "F6"
''    If sp.Q.Check4Text(22, 3, "40281") Then
''        Exit Sub
''    End If
''
''    If Not sp.Q.Check4Text(22, 3, "49000") Then
''        If sp.Q.Check4Text(22, 3, "49007") Then
''            MsgBox "No record changes for " & QueueName
''            Exit Sub
''        End If
''        MsgBox "Script cannot post changes. Please correct the problem and manually post changes.", vbCritical, "Posting Error"
''        End
''    End If
    
    If Not SP.Q.Check4Text(22, 3, "49000") And Not SP.Q.Check4Text(22, 3, "40281") And Not SP.Q.Check4Text(22, 3, "49007") Then
        MsgBox "Script cannot post changes. Please correct the problem and manually post changes.", vbCritical, "Posting Error"
        End
    End If
End Sub
