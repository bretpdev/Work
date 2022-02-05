Attribute VB_Name = "DeathStatEnrl"
Private SASDir As String
Private LogDir As String

Sub Main()
    Dim SASFileToProc As String
    Dim RecoveryLine As String
    Dim FileLine As String
    Dim UID As String
    If Not SP.Common.CalledByMBS Then If vbOK <> MsgBox("This is the Death Status Enrollment script.  Click OK to continue or Cancel to end the script.", vbOKCancel, "Populate PM01 queue") Then End
    SP.Common.TestMode SASDir, , LogDir
    If Dir(SASDir & "ULWG29.LWG29R2*") = "" Then
        MsgBox "SAS ""ULWG29.LWG29R2*"" does not exist.  Please contact Systems Support for assistance.", vbCritical
        End
    End If
    If AllFilesEmpty() Then
        MsgBox "The SAS file is empty.  There is nothing to process today.", vbCritical
'<1->
'       End
        ProcComp "MBSDEASTAENR.txt", False
'</1>
    End If
    'get user id for TD22
    UID = SP.Common.GetUserID()
    'check for recovery mode
    If Dir(LogDir & "Death Status Enrollment.txt") <> "" Then
        Open LogDir & "Death Status Enrollment.txt" For Input As #1
        Input #1, RecoveryLine
        Close #1
    End If
    'get most recent file
    RetrieveMostRecentDelRest SASFileToProc
    Open SASDir & SASFileToProc For Input As #1
    'if recovery information is populated then search the file for the last line to be processed
    If RecoveryLine <> "" Then
        While FileLine <> RecoveryLine
            Input #1, FileLine
        Wend
    End If
    'process line
    While Not EOF(1)
        Input #1, FileLine
        If lATD22AllLoans(FileLine, "DEOL1", "Enroll Status death on OneLINK; active loans on COMPASS", "DEASTAENR", UID) = False Then
            MsgBox "You need access to the ""DEOL1"" ARC to use this script.", vbCritical
            End
        End If
        'write to recovery file
        Open LogDir & "Death Status Enrollment.txt" For Output As #2
        Write #2, FileLine
        Close #2
    Wend
    Close #1
    Kill SASDir & SASFileToProc
    Kill LogDir & "Death Status Enrollment.txt"
'<1->
'   MsgBox "Processing Complete", vbInformation
'   End
    ProcComp "MBSDEASTAENR.txt"
'</1>
End Sub


'this function iterates through all files with SAS naming convention and deletes all empty files and returns true if all of them were empty
Function AllFilesEmpty() As Boolean
    Dim Temp As String
    AllFilesEmpty = True
    Temp = Dir(SASDir & "ULWG29.LWG29R2*")
    While Temp <> ""
        If FileLen(SASDir & Temp) = 0 Then
            Kill SASDir & Temp
        Else
            AllFilesEmpty = False
        End If
        Temp = Dir()
    Wend
End Function

'this function retrieves the most recent file and deletes the rest of them
Function RetrieveMostRecentDelRest(ByRef SASFTP As String)
    Dim Temp As String
    Dim SASFTPTDS As Date
    SASFTP = Dir(SASDir & "ULWG29.LWG29R2*")
    SASFTPTDS = FileDateTime(SASDir & SASFTP)
    Temp = Dir()
    While Temp <> ""
        If FileDateTime(SASDir & SASFTP) < FileDateTime(SASDir & Temp) Then
            Kill SASDir & SASFTP 'delete old file
            SASFTP = Temp
        End If
        Temp = Dir()
    Wend
End Function

'enters an activity record/action request in COMPASS selecting only the loans specified
Function lATD22AllLoans(ssn As String, ARC As String, comment As String, Script As String, UserID As String, Optional PauPls As Boolean = False) As Boolean
    Dim row As Integer
    
    lATD22AllLoans = True
    FastPath "TX3Z/ATD22" & ssn
    If Not Check4Text(1, 72, "TDX23") Then
        lATD22AllLoans = False
        Exit Function
    End If
    'find the ARC
    Do
        found = Session.FindText(ARC, 8, 8)
        If found Then Exit Do
        Hit "F8"
        If Check4Text(23, 2, "90007") Then
            lATD22AllLoans = False
            Exit Function
        End If
    Loop
    'select the ARC
    puttext Session.FoundTextRow, Session.FoundTextColumn - 5, "01", "ENTER"
    'exit the function if the selection screen is not displayed
    If Not Check4Text(1, 72, "TDX24") Then
        lATD22AllLoans = False
        Exit Function
    End If
    'select all of the loans
    Do
        puttext 11, 3, "XXXXXXXX"
        If Check4Text(8, 75, "+") Then Hit "F8" Else Exit Do
    Loop
    'blank extra X's
    puttext 21, 2, "", "END"
    'enter short comments
    If Len(comment) < 132 Then
        puttext 21, 2, comment & "  {" & Script & "} /" & UserID
        If PauPls Then
            If vbYes = MsgBox("Do you want to enter additional comments?  Click Yes to pause the script, add the additional comments, and then hit <Insert> to resume the script.  Click No to continue processing without adding additional comments.", 36, "Add Additional Comments") Then
                Session.WaitForTerminalKey rcIBMInsertKey, "1:00:00"
                Hit "INS"
            End If
        End If
        Hit "ENTER"
        If Check4Text(23, 2, "01764 TASK RECORD ALREADY EXISTS") Then
            Exit Function
        End If
        If Not Check4Text(23, 2, "02860") Then lATD22AllLoans = False
    'enter long comments
    Else
        'fill the first screen
        puttext 21, 2, MID(comment, 1, 154), "ENTER"
        If Check4Text(23, 2, "01764 TASK RECORD ALREADY EXISTS") Then
            Exit Function
        End If
        If Not Check4Text(23, 2, "02860") Then
            lATD22AllLoans = False
            Exit Function
        End If
        Hit "F4"
        'enter the rest on the expanded comments screen
        For k = 155 To Len(comment)
            Session.TransmitANSI MID(comment, k, 260)
            k = k + 260
        Next k
        Session.TransmitANSI "  {" & Script & "} /" & UserID
        If PauPls Then
            If vbYes = MsgBox("Do you want to enter additional comments?  Click Yes to pause the script, add the additional comments, and then hit <Insert> to resume the script.  Click No to continue processing without adding additional comments.", 36, "Add Additional Comments") Then
                Session.WaitForTerminalKey rcIBMInsertKey, "1:00:00"
                Hit "INS"
            End If
        End If
        Hit "ENTER"
        If Not Check4Text(23, 2, "02114") Then lATD22AllLoans = False
    End If
End Function

'new sr1280, aa
'<1> sr1551, jd, disabled prompts if called by MBS


