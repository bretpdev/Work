Sub Main()
    Dim TaskText As String          '<1>
    Dim TaskParts() As String       '<1>
    'prompt the user regarding the functionality of the script
    If Not SP.Common.CalledByMBS Then If vbOK <> MsgBox("This script updates the State Offset Parm Card from DSTPARMU queue tasks.", vbOKCancel, "State Offset Parm Card Update") Then End
    'access LP9A for DSTPARMU queue
    FastPath "LP9ACDSTPARMU"
    'check if there are any tasks in the queue
    If Check4Text(1, 66, "QUEUE SELECTION") Then
        MsgBox "There aren't any tasks in the ""DSTPARMU"" queue."
        End
    End If
    
    If Not Check4Text(3, 24, "DSTPARMU") Then
            MsgBox "This User Id is already assigned to another queue task. Please complete the task and try again."
        End
    End If

    'process all tasks in the queue
    While Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
'<1->
        'gather task text
        TaskText = GetText(12, 11, 61) & GetText(13, 11, 61) & GetText(14, 11, 61) & GetText(15, 11, 61)
        'remove all "_"
        TaskText = Replace(TaskText, "_", "")
        'break into parts
        TaskParts = Split(TaskText, ",")
        'take out userid
        TaskParts(UBound(TaskParts)) = MID(TaskParts(UBound(TaskParts)), 1, Len(TaskParts(UBound(TaskParts))) - 10)
        If Check4Text(12, 11, "B") Then 'first character = B
            'LP4C(ByVal SSN As String, ByVal TypeOfRec As String, ByVal Addr1 As String, ByVal Addr2 As String, ByVal City As String, ByVal State As String, ByVal ZIP As String, Optional ByVal FN As String = "", Optional ByVal MI As String = "", Optional ByVal LN As String)
            LP4C GetText(17, 70, 9), TaskParts(0), TaskParts(1), TaskParts(2), TaskParts(3), TaskParts(4), TaskParts(5)
        Else 'first character = S
            LP4C GetText(17, 70, 9), TaskParts(0), TaskParts(4), TaskParts(5), TaskParts(6), TaskParts(7), TaskParts(8), TaskParts(1), TaskParts(2), TaskParts(3)
        End If
'        If check4text(12, 11, "B") Then 'first character = B
'            'LP4C(ByVal SSN As String, ByVal TypeOfRec As String, ByVal Addr1 As String, ByVal Addr2 As String, ByVal City As String, ByVal State As String, ByVal ZIP As String, Optional ByVal FN As String = "", Optional ByVal MI As String = "", Optional ByVal LN As String)
'            LP4C GetText(17, 70, 9), GetText(12, 11, 1), GetText(12, 13, 18), GetText(12, 32, 18), GetText(12, 51, 18), GetText(13, 12, 2), GetText(13, 15, 5)
'        Else 'first character = S
'            LP4C GetText(17, 70, 9), GetText(12, 11, 1), GetText(12, 53, 16) & GetText(13, 11, 2), GetText(13, 14, 18), GetText(13, 33, 16), GetText(13, 52, 2), GetText(13, 55, 5), GetText(12, 13, 18), GetText(12, 32, 2), GetText(12, 34, 18)
'        End If
'</1>
        FastPath "LP9ACDSTPARMU" 'go back to queue
        Hit "F6" 'complete queue task
        Hit "F8" 'move to the next task
    Wend
'<3->
'   MsgBox "Processing Complete"
    Common.ProcComp "MBSSOPRMCRD.txt"
'</3>
End Sub

'add info to parm card
Function LP4C(ByVal ssn As String, ByVal TypeOfRec As String, ByVal Addr1 As String, ByVal Addr2 As String, ByVal city As String, ByVal state As String, ByVal zip As String, Optional ByVal FN As String = "", Optional ByVal MI As String = "", Optional ByVal ln As String)
    'access LP4C for the correct type
    If TypeOfRec = "B" Then 'borrower
        FastPath "LP4CALCXS2;01"
        PutText 7, 4, ssn
        PutText 7, 23, Addr1
        PutText 7, 42, Addr2, "Enter"
        FastPath "LP4CALCXS2;02"
        PutText 7, 4, ssn
        PutText 7, 23, city
        PutText 7, 42, state
        PutText 7, 61, zip, "Enter"
    Else 'spouse
        FastPath "LP4CALCXS2;03"
        PutText 7, 4, ssn
        PutText 7, 23, FN
        PutText 7, 42, MI
        PutText 7, 61, ln, "Enter"
        FastPath "LP4CALCXS2;04"
        PutText 7, 4, ssn
        PutText 7, 23, Addr1
        PutText 7, 42, Addr2, "Enter"
        FastPath "LP4CALCXS2;05"
        PutText 7, 4, ssn
        PutText 7, 23, city
        PutText 7, 42, state
        PutText 7, 61, zip, "Enter"
    End If
    Wait "2" '<2>
    SP.Common.AddLP50 ssn, "DPMOK", "SOPRMCRD", "AM", "10", "Parm card update with address for borrower and or spouse for state offset processed."
End Function

'new sr 845, aa, 11/23/04, 01/26/05
'<1> sr 982, aa, 03/01/05, 03/03/05
'<2> sr 993, tp, 03/08/05, 03/09/05
'<3> sr1589, jd, disabled prompts if called by MBS
