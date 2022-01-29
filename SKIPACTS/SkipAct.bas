Attribute VB_Name = "SkipAct"
Dim ssn As String
Dim ActCd As String
Dim comment As String
Dim UserID As String
Dim LogFolder As String
Dim lSSN As String
Dim ldate As String
Dim lPhase As String                    '<2>
Dim ComplCd As String                   '<2>
Dim lACTCD As String                    '<2>
Dim lcomment As String                  '<2>
Dim lFile As String                     '<4>
Dim DocFolder As String                 '<2>


'add OneLINK skip activities to COMPASS from records in a SAS file
Sub SkipActMain()
    Dim SasFile As String
    Dim ftpFolder As String
    Dim HasFullFile As Boolean

    DocFolder = "X:\PADD\Skip\"             '<2>
    
    'Advise user of what the script does and have them either continue or cancel
    If Not SP.Common.CalledByMBS Then If MsgBox("This script creates skip activities in COMPASS for borrowers listed in a SAS file.", 65, "Skip Activities") <> 1 Then End '<3>
    
    'Determine if in production or test and assign directory accordindly
    SP.Common.TestMode ftpFolder, DocFolder, LogFolder
    
    'get th user ID
    UserID = GetUserID

'<4->
'    'Use the MadatorySASProcessing function to find the appropriate files
'    SASFile = MandatorySASProcessing(FTPFolder & "ULWK07.LWK07R2.*.*", FTPFolder & "ULWK07.LWK07R1.*.*")
    
    'delete the log if it exists
    If Dir(ftpFolder & "ULWK07.LWK07R1.*.*") <> "" Then Kill ftpFolder & Dir(ftpFolder & "ULWK07.LWK07R1.*.*")
    
    'warn the user and end if no files are found
    If Dir(ftpFolder & "ULWK07.LWK07R2.*.*") = "" Then
        MsgBox "No files were found.  Contact Systems Support for assistance.", 48, "No Files Found"
        End
    End If
'</4>
    
'process log
    'create an empty log if none exists
    If Dir(LogFolder & "SkipActs.txt") = "" Then
        Open LogFolder & "SkipActs.txt" For Output As #2
        Close #2
    End If
    'read the contents of the log if it is not empty
    If FileLen(LogFolder & "SkipActs.txt") <> 0 Then
        Open LogFolder & "SkipActs.txt" For Input As #2
        '<2>Input #2, LSSN, ldate
        Input #2, lSSN, lACTCD, lcomment, ldate, lPhase, lFile           '<2>, '<4>
        Close #2
    'assign values to indicate all processing is needed
    Else
        lSSN = ""
        lACTCD = ""                             '<2>
        lcomment = ""                           '<2>
        ldate = ""
        lPhase = "1"                            '<2>
        lFile = ""                              '<4>
    End If
    'set the status to indicate processing has been completed for the day
    If lSSN = "done" And ldate = Format(Date, "MM/DD/YYYY") Then
        MsgBox "Processing is already complete.", 48, "Skip Activities"
        ProcComp "MBSSKIPACTS.TXT", False '<3>
        End
    'assign values to indicate all processing is needed
    ElseIf lSSN = "done" Then
        lSSN = ""
        lACTCD = ""                             '<2>
        lcomment = ""                           '<2>
        ldate = ""
        lPhase = "1"                            '<2>
        lFile = ""                              '<4>
    End If

'<4->
    HasFullFile = False
    While Dir(ftpFolder & "ULWK07.LWK07R2.*.*") <> ""
        If lFile = "" Then SasFile = ftpFolder & Dir(ftpFolder & "ULWK07.LWK07R2.*.*") Else SasFile = lFile
        'process non-empty files
        If FileLen(SasFile) <> 0 Then
            ProcFiles SasFile
            HasFullFile = True
        'delete empty files
        Else
            Kill SasFile
        End If
    Wend
    
    'update the recovery log
    Open LogFolder & "SkipActs.txt" For Output As #2
    Write #2, "done", "done", "done", Format(Date, "MM/DD/YYYY"), "0", ""
    Close #2
    
    'print and delete Error report
    If Dir("T:\Skip Activity Error dat.txt") <> "" Then
        SP.Common.PrintDocs DocFolder, "Skip Activity Error Rpt", "T:\Skip Activity Error dat.txt"
        MsgBox "Please pick up error report at printer.", vbOK, "Error Report Printed"
        Kill "T:\Skip Activity Error dat.txt"
    End If
    
    'warn the user if there no loans for processing
    If Not HasFullFile Then MsgBox "There were no loans for processing.", vbInformation, "No Loans for Processing"
    
    ProcComp "MBSSKIPACTS.TXT"

'    While Dir(FTPFolder & "ULWK07.LWK07R2*") <> ""
'        If lPhase = "1" Then
'            'Open file and process records
'            Open FTPFolder & Dir(FTPFolder & "ULWK07.LWK07R2*") For Input As #1
'            'Account for header row and start procssing after 1st row has been processed.
'            Input #1, SSN, ActCd, Comment, ComplCd
'            'bypass successfully completed records
'            If lSSN <> "" Then
''<2>            While LSSN <> SSN
'                While lSSN <> SSN Or ActCd <> lACTCD Or Comment <> lcomment     '<2>
''<2>                Input #1, SSN, ACTCD, comment
'                    Input #1, SSN, ActCd, Comment, ComplCd                      '<2>
'                Wend
'            End If
'            'Start processing While loop
'            While Not EOF(1)
''<2>                Input #1, SSN, ACTCD, comment
'                    Input #1, SSN, ActCd, Comment, ComplCd                      '<2>
'                If Not ATD22AllLoans(SSN, ActCd, Comment, "SKIPACTS", UserID) Then
''<1->
''                   MsgBox "The activity record has not been added for ARC " & ACTCD & ".  Please correct any access issues and re-run this script.", 16, "Update Error"
''                   End
'                    'warn the user and end the script if the activity record was not added and the reason was not because the loan was not found (deconverted)
'                    If SP.Common.TestMode() = False Then
'                        If Not Check4Text(23, 2, "50108") Then
'                            MsgBox "The activity record has not been added for ARC " & ActCd & ".  Please contact Systems Support and re-run the script once the problem has been resolved.", 16, "Update Error"
'                            End
'                        End If
'                    End If
''</1>
'                End If
'                'read SSN to recovery log
'                Open LogFolder & "SkipActs.txt" For Output As #2
''<2>            Write #2, SSN, Format(Date, "MM/DD/YYYY")
'                Write #2, SSN, ActCd, Comment, Format(Date, "MM/DD/YYYY"), "1"                       '<2>
'                Close #2
'            Wend
'            'Close the file
'            Close #1
'        End If                                      '<2>
''<2->
'        Open FTPFolder & Dir(FTPFolder & "ULWK07.LWK07R2*") For Input As #1
'        'Account for header row and start procssing after 1st row has been processed.
'        Input #1, SSN, ActCd, Comment, ComplCd
'        'bypass successfully completed records
'        If lSSN <> "" And lPhase = "2" Then
'            While lSSN <> SSN Or ActCd <> lACTCD Or Comment <> lcomment
'                Input #1, SSN, ActCd, Comment, ComplCd
'            Wend
'        End If
'        'Start processing While loop
'        While Not EOF(1)
'            Input #1, SSN, ActCd, Comment, ComplCd
'            If ComplCd <> "COMPL" Then
'                FastPath "TX3ZCTD2A" & SSN
'                puttext 11, 65, ActCd
'                puttext 6, 60, "X"
'                puttext 21, 16, Format(Date, "MMDDYY") & Format(Date, "MMDDYY"), "Enter"
'                'skip all other processing if results aren't found
'                If Check4Text(23, 2, "01019 ENTERED KEY NOT FOUND") = False Then
'                    If Check4Text(1, 72, "TDX2C") Then
'                        'selection screen
'                        puttext 5, 14, "X", "Enter"
'                        While Check4Text(15, 2, "_") = False And Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
'                            Hit "F8"
'                        Wend
'                        'enter completion code if blank one if found else skip it
'                        If Check4Text(15, 2, "_") Then
'                            puttext 15, 2, ComplCd, "Enter"
'                            'if correct error code isn't displayed then end script
'                            If Check4Text(23, 2, "01005") = False Then
'                                MsgBox "The script has encountered a fatal error.  Please contact Systems Support."
'                                End
'                            End If
'                        End If
'                    Else
'                        'target screen
'                        'enter completion code if blank one if found else skip it
'                        If Check4Text(15, 2, "_") Then
'                            puttext 15, 2, ComplCd, "Enter"
'                            'if correct error code isn't displayed then end script
'                            If Check4Text(23, 2, "01005") = False Then
'                                Open "C:\Windows\Temp\Skip Activity Error dat.txt" For Append As #3
'                                'if size of file is 0 then create header row
'                                If Len("C:\Windows\Temp\Skip Activity Error dat.txt") = 0 Then
'                                    Write #3, "SSN", "ACTCD", "Comment", "C"
'                                End If
'                                'write out error data
'                                Write #3, SSN, ActCd, Comment, ComplCd
'                                Close #3
'                            End If
'                        End If
'                    End If
'                End If
'            End If
'            'read SSN to recovery log
'            Open LogFolder & "SkipActs.txt" For Output As #2
'            Write #2, SSN, ActCd, Comment, Format(Date, "MM/DD/YYYY"), "2"                       '<2>
'            Close #2
'        Wend
'        Close #1
'        'delete the sas file
'        Kill FTPFolder & SASFile
''</2>
'    Wend                                            '<2>
'    'update the recovery log
'    Open LogFolder & "SkipActs.txt" For Output As #2
''<2>Write #2, "done", Format(Date, "MM/DD/YYYY")
'    Write #2, "done", "done", "done", Format(Date, "MM/DD/YYYY"), "0"         '<2>
'    Close #2
''<2->
''    'delete the sas file
''    Kill FTPFolder & sasfile
'    'print and delete Error report
'    If Dir("C:\Windows\Temp\Skip Activity Error dat.txt") <> "" Then
'        SP.Common.PrintDocs DocFolder, "Skip Activity Error Rpt", "C:\Windows\Temp\Skip Activity Error dat.txt"
'        MsgBox "Please pick up error report at printer."
'        Kill "C:\Windows\Temp\Skip Activity Error dat.txt"
'    End If
''</2>
'    'Alert the user that processing is finished
''<3>    MsgBox "Processing complete.", 64, "Processing Complete"
'    ProcComp "MBSSKIPACTS.TXT"
''</3>
'</4>
End Sub

'<4->
Function ProcFiles(SasFile As String)
    If lPhase = "1" Then
        'Open file and process records
        Open SasFile For Input As #1
        'Account for header row and start procssing after 1st row has been processed.
        Input #1, ssn, ActCd, comment, ComplCd
        'bypass successfully completed records
        If lSSN <> "" Then
            While lSSN <> ssn Or ActCd <> lACTCD Or comment <> lcomment
                Input #1, ssn, ActCd, comment, ComplCd
            Wend
        End If
        'Start processing While loop
        While Not EOF(1)
                Input #1, ssn, ActCd, comment, ComplCd
            If Not SP.Common.ATD22AllLoans(ssn, ActCd, comment, "SKIPACTS", UserID) Then
                'warn the user and end the script if the activity record was not added and the reason was not because the loan was not found (deconverted)
                If SP.Common.TestMode() = False Then
                    If Not Check4Text(23, 2, "50108") Then
                        MsgBox "The activity record has not been added for ARC " & ActCd & ".  Please contact Systems Support and re-run the script once the problem has been resolved.", 16, "Update Error"
                        End
                    End If
                End If
            End If
            'read SSN to recovery log
            Open LogFolder & "SkipActs.txt" For Output As #2
            Write #2, ssn, ActCd, comment, Format(Date, "MM/DD/YYYY"), "1", SasFile
            Close #2
        Wend
        'Close the file
        Close #1
    End If

    Open SasFile For Input As #1
    'Account for header row and start procssing after 1st row has been processed.
    Input #1, ssn, ActCd, comment, ComplCd
    'bypass successfully completed records
    If lSSN <> "" And lPhase = "1" Then
        While lSSN <> ssn Or ActCd <> lACTCD Or comment <> lcomment
            Input #1, ssn, ActCd, comment, ComplCd
        Wend
    End If
    'Start processing While loop
    While Not EOF(1)
        Input #1, ssn, ActCd, comment, ComplCd
        If ComplCd <> "COMPL" Then
            FastPath "TX3ZCTD2A" & ssn
            puttext 11, 65, ActCd
            puttext 6, 60, "X"
            puttext 21, 16, Format(Date, "MMDDYY") & Format(Date, "MMDDYY"), "Enter"
            'skip all other processing if results aren't found
            If Check4Text(23, 2, "01019 ENTERED KEY NOT FOUND") = False Then
                If Check4Text(1, 72, "TDX2C") Then
                    'selection screen
                    puttext 5, 14, "X", "Enter"
                    While Check4Text(15, 2, "_") = False And Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                        Hit "F8"
                    Wend
                    'enter completion code if blank one if found else skip it
                    If Check4Text(15, 2, "_") Then
                        puttext 15, 2, ComplCd, "Enter"
                        'if correct error code isn't displayed then end script
                        If Check4Text(23, 2, "01005") = False Then
                            MsgBox "The script has encountered a fatal error.  Please contact Systems Support."
                            End
                        End If
                    End If
                Else
                    'target screen
                    'enter completion code if blank one if found else skip it
                    If Check4Text(15, 2, "_") Then
                        puttext 15, 2, ComplCd, "Enter"
                        'if correct error code isn't displayed then end script
                        If Check4Text(23, 2, "01005") = False Then
                            Open "T:\Skip Activity Error dat.txt" For Append As #3
                            'if size of file is 0 then create header row
                            If FileLen("T:\Skip Activity Error dat.txt") = 0 Then
                                Write #3, "SSN", "ACTCD", "Comment", "C"
                            End If
                            'write out error data
                            Write #3, ssn, ActCd, comment, ComplCd
                            Close #3
                        End If
                    End If
                End If
            End If
        End If
        
        'read SSN to recovery log
        Open LogFolder & "SkipActs.txt" For Output As #2
        Write #2, ssn, ActCd, comment, Format(Date, "MM/DD/YYYY"), "1", SasFile
        Close #2
    Wend
    Close #1
    
    'delete the sas file
    Kill SasFile
    
'<4->
    'clear recovery variables and recovery file
    lSSN = ""
    lACTCD = ""
    lcomment = ""
    ldate = ""
    lPhase = "1"
    lFile = ""
    
    Open LogFolder & "SkipActs.txt" For Output As #2
    Write #2, "", "", "", "", "1", ""
    Close #2
'</4>

End Function
'</4>

'<4->
''verify that the SAS file exists and delete the SAS log
'Function MandatorySASProcessing(SASFile As String, Optional LogFile As String) As String
'        Dim TempSASFile As String
'        Dim TempLogFile As String
'        TempSASFile = Dir(SASFile)
'        If (LogFile <> "") Then
'            TempLogFile = Dir(LogFile)
'            While ("" <> TempLogFile)
'                Kill FTPFolder & TempLogFile
'                TempLogFile = Dir(LogFile)
'            Wend
'        End If
'        If ("" = TempSASFile) Then
'            MsgBox "The needed SAS file can't be found.  Please contact Systems Support."
'            End
'        ElseIf (0 = FileLen(FTPFolder & TempSASFile)) Then
'            Kill FTPFolder & TempSASFile
'            TempSASFile = Dir(SASFile)
'            If ("" = TempSASFile) Then
'                MsgBox "The SAS file was empty.  The script will now end."
'                ProcComp "MBSSKIPACTS.TXT", False '<3>
'                End
'            End If
'        End If
'        MandatorySASProcessing = TempSASFile
'End Function
'</4>

'new sr 822, sb, 11/03/04, 11/12/04
'<1> sr 906, jd, 12/22/04, 01/25/05
'<2> sr1380, aa
'<3> sr1582, tp, 05/16/06, 05/31/06 Removed prompts to prepare script for MBS
'<4> sr2067, jd
'<5> sr2207, sb
