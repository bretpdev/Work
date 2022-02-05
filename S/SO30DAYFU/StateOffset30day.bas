Attribute VB_Name = "StateOffset30day"
Sub Main()
    'prompt the user regarding the functionality of the script
    If Not SP.Common.CalledByMBS Then If vbOK <> MsgBox("This script works the tasks in the ""STOFFSET"" queue.", vbOKCancel, "State Offset 30 Day Follow-Up") Then End
    
    'access LP9A for STOFFSET queue
    FastPath "LP9ACSTOFFSET"
    'check if there are any tasks in the queue
    If Check4Text(1, 66, "QUEUE SELECTION") Then
        MsgBox "There aren't any tasks in the ""STOFFSET"" queue."
        ProcComp "MBSSO30DAYFU.TXT", False
        End
    End If
    'process all tasks in the queue
    While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
        LC05 GetText(17, 70, 9), FormatCurrency(GetText(12, 11, 8), 2), GetText(12, 20, 10), GetText(12, 31, 8)
        FastPath "LP9ACSTOFFSET" 'go back to queue
        Hit "F6" 'complete queue task
        Hit "F8" 'move to the next task
    Wend
    'sort datafile
    SP.Common.SortFile "T:\StateOffset 30 Day Dat.txt", 0, "SSN, GarnAmt, WarrNum, ListDt, Comment"
    If SP.Common.TestMode() Then
        SP.Common.PrintDocs "X:\PADD\Collections\Test\", "30 Day State Offset Report", "T:\StateOffset 30 Day Dat.txt"
    Else
        SP.Common.PrintDocs "X:\PADD\Collections\", "30 Day State Offset Report", "T:\StateOffset 30 Day Dat.txt"
    End If
    Kill "T:\StateOffset 30 Day Dat.txt"
    If Not SP.Common.CalledByMBS Then MsgBox "Processing Complete.  Please retrieve the report from the printer."
    ProcComp "MBSSO30DAYFU.TXT", False
End Sub

'this function checks for specified statuses on LC05
Function LC05(ssn As String, GarnAmt As String, WarrNum As String, ListDt As String)
    FastPath "LC05I" & ssn
'<2->
    'get balance
    Dim Bal As Double
    Bal = CDbl(GetText(4, 69, 12))
'</2>
    'if a selection screen is found then select the first option
    If Check4Text(1, 70, "CLAIM RECAP") Then
        puttext 21, 13, "1", "Enter"
    End If
    While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
        If Check4Text(4, 10, "03") And Check4Text(4, 26, "07") Then 'check for Bankruptcy status
            SP.Common.AddLP50 ssn, "DDOSF", "SO30DAYFU", "AM", "10", "Account is in open bankruptcy garnishment amount is 0.00"
            Wait "2" '<1>
            WriteToFile ssn, GarnAmt, WarrNum, ListDt, "BK"
        ElseIf Check4Text(4, 10, "04") = False Then 'check for NON 04 status
            'look for DDISR comment
            FastPath "LP50I" & ssn
            puttext 9, 20, "DDISR", "Enter"
            If Check4Text(1, 68, "ACTIVITY MENU") Then 'comment not found
                If CDbl(GarnAmt) > Bal Then GarnAmt = Format(Bal, "$####,##0.00")   '<2>
                SP.Common.AddLP50 ssn, "DDOSF", "SO30DAYFU", "AM", "10", GarnAmt & ";" & WarrNum & ";" & ListDt & " sending report to state to garnish entire amount."
                Wait "2" '<1>
                WriteToFile ssn, GarnAmt, WarrNum, ListDt, "Garnish " & GarnAmt
                Exit Function
            ElseIf Check4Text(1, 58, "ACTIVITY DETAIL DISPLAY") Then 'only one LP50 found
                'gather needed data and create comment
                SP.Common.AddLP50 ssn, "DDOSF", "SO30DAYFU", "AM", "10", GetText(13, 2, 9) & ";" & GetText(13, 12, 9) & ";" & ListDt & " injured spouse already released."
                Wait "2" '<1>
                WriteToFile ssn, GarnAmt, WarrNum, ListDt, "Is already released"
                Exit Function
            Else 'comment found
                'select the most current comment
                puttext 6, 2, "X", "Enter"
                'gather needed data and create comment
                SP.Common.AddLP50 ssn, "DDOSF", "SO30DAYFU", "AM", "10", GetText(13, 2, 9) & ";" & GetText(13, 12, 9) & ";" & ListDt & " injured spouse already released."
                Wait "2" '<1>
                WriteToFile ssn, GarnAmt, WarrNum, ListDt, "Is already released"
                Exit Function
            End If
        End If
        Hit "F8"
    Wend
    'if the script exits through here then all claims had a 04 status
    SP.Common.AddLP50 ssn, "DDOSF", "SO30DAYFU", "AM", "10", "Account is closed garnishment amount = 0.00"
    Wait "2" '<1>
    WriteToFile ssn, GarnAmt, WarrNum, ListDt, "Closed"
End Function

'this function writes information out to the data merge file
Function WriteToFile(ssn As String, GarnAmt As String, WarrNum As String, ListDt As String, comment As String)
    Open "T:\StateOffset 30 Day Dat.txt" For Append As #1
    Write #1, ssn, GarnAmt, WarrNum, ListDt, comment
    Close #1
End Function


'new, sr 847, aa, 12/02/04, 01/26/05
'<1>, sr 994, tp, 03/08/05, 03/09/05
'<2>, sr1078, jd, 04/26/05, 05/11/05
'<3>, sr1588, tp, 04/28/2006 Removed prompts to prepare script for MBS

