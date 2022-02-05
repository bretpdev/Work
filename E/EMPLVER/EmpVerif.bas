Attribute VB_Name = "EmpVerif"
'Script ID = EMPLVER
Dim Emp As String
Dim AddEmp As Boolean
Dim SSN As String
Dim Name As String '<1>

Sub Main()
Dim ran As Boolean
Dim x As Integer
Dim found As Boolean 'found employer ID
Dim foundAll As Boolean 'found all 01 2000, true if all are 01 and 2000
    found = False
    foundAll = True
    
'<2>
    'In some cases, local variables with the same names as some public variables
    ' are not properly going out of scope. The following function call will clear
    ' the contents of those variables so that the previous record's information
    ' does not persist into this iteration.
    ResetPublicVars
'</2>
     If MsgBox("This script processes the Employer Verification Queue?", vbOKCancel, "Employer Verification") = vbCancel Then End
     ran = False
     SP.q.FastPath "LP9ACEMPVERIF"
     Do
        
        If SP.q.Check4Text(1, 71, "QUEUE TASK") = False Then
            If ran = False Then
                MsgBox "Queue is empty."
                End
            Else
                MsgBox "Script Complete!"
                End
            End If
        End If
        SSN = SP.q.GetText(17, 70, 9)
        Name = SP.q.GetText(17, 6, 38)
        SP.q.FastPath "LC20C" & SSN
        'if 01 2000 then employer is invalid
        If (SP.q.Check4Text(10, 2, "01") And SP.q.Check4Text(10, 5, "2000")) Or (SP.q.Check4Text(10, 2, "__") And SP.q.Check4Text(10, 5, "____")) Then
                foundAll = True
        Else
                foundAll = False
        End If
  
        If foundAll Then
            'if employer is invalid close queue task.
            SP.q.PutText 10, 78, "N"
            SP.q.PutText 13, 67, "**", "END"
            SP.q.PutText 14, 67, "**", "END"
            SP.q.Hit "ENTER"
            SP.Common.AddLP50 SSN, "DNVER", "EMPLVER", "AM", "10", , True
        Else
            
            Emp = SP.q.GetText(10, 12, 8)
            If MsgBox("Is the correct Employer: " & Emp & "?", vbYesNo) = vbYes Then
                'yes
                AddEmp = False
            Else
                'no
                AddEmp = True
                Emp = InputBox("Please enter the correct Employer ID:", "Employer Verification")
                If Emp = "" Then End
                
            End If
            SP.q.FastPath "LPEMI" & Emp
            MsgBox "Please call employer to verify employment. Press Insert when done." & Chr(13) & "Borrower:  " & Name & Chr(13) & "SSN:  " & SSN
            SP.q.PauseForInsert
            frmEmpVerif.Show
            
            If frmEmpVerif.radNot Then
            'a)  Borrower is no longer employed.
            
            NotEmp
            ElseIf frmEmpVerif.radVer Then
            'b)  Verified borrower is employed.
            VerEmp
            ElseIf frmEmpVerif.radUnable Then
            'c)  Was unable to contact employer
            Unable
            End If
        End If
        'close queue task
        SP.q.FastPath "LP9ACEMPVERIF"
        SP.q.Hit "F6"
        SP.q.FastPath "LP9ACEMPVERIF"
        ran = True
     Loop
     
End Sub

Sub NotEmp()
'borrower no longer employed
    Dim x As Integer
    Dim found As Boolean 'found employer ID
    Dim foundAll As Boolean 'found all 01 2000, true if all are 01 and 2000
    Dim Row As Integer
    found = False
    foundAll = True
    SP.q.FastPath "LC20C" & SSN
    If AddEmp Then SP.q.PutText 10, 12, Emp, "ENTER"
    For x = 0 To 11
        If SP.q.Check4Text(10 + x, 12, Emp) Then
            found = True
            Row = x
            Exit For
        End If
'        If (Not SP.Q.Check4Text(10 + x, 2, "01") And Not SP.Q.Check4Text(10 + x, 2, "")) Or (Not SP.Q.Check4Text(10 + x, 5, "2000") And Not SP.Q.Check4Text(10 + x, 5, "")) Then
'            foundAll = False
'        End If
    Next x
    If found Then
        SP.q.PutText 10 + Row, 2, "01"
        SP.q.PutText 10 + Row, 5, "2000"
    Else
        MsgBox "Employer not found. Contact Systems Support."
        End
    End If

    SP.q.PutText 10, 78, "N"
    SP.q.PutText 13, 67, "**", "END"
    SP.q.PutText 14, 67, "**", "END"

    SP.q.Hit "ENTER"
    SP.Common.AddLP50 SSN, "DNLEG", "EMPLVER", "TE", "81", , True
    
End Sub
Sub VerEmp()
    SP.q.FastPath "LC20C" & SSN
    If AddEmp Then SP.q.PutText 10, 12, Emp, "ENTER"
    SP.q.PutText 10, 78, "Y"
    SP.q.PutText 13, 67, Emp
    SP.q.PutText 14, 67, Format(Date, "MMDDYYYY"), "ENTER"
    SP.Common.AddLP50 SSN, "DEMVE", "EMPLVER", "TE", "81", , True
End Sub
Sub Unable()
    SP.Common.AddLP9O SSN, "EMPFLUP", , "Follow up to verify employment"
    SP.Common.AddLP50 SSN, "DEMEF", "EMPLVER", "TE", "81", , True
End Sub

'<new> SR1111, tp, 05/16/05, 06/27/05
'<1> sr 1199, tp, 07/05/05, 08/18/05
'<2> sr2227, db
