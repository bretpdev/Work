Attribute VB_Name = "RehabBorrowerInPreClaim"
Private ftpFolder As String
Private DocFolder As String
Private LogFolder As String
Private printed As Boolean
Private UserID As String
Private RValue As String        'Recovery variable, should only have a value if in recovery mode
Private HasRun As Boolean

Sub Main()
    'Declarations
    Dim mbs As Boolean
    Dim timeStamp As String
    Dim SasFile As String
    'Initializations
    DocFolder = "X:\PADD\LoanManagement\"
    mbs = SP.Common.CalledByMBS()
    timeStamp = Time
    HasRun = False
    If SP.Common.TestMode(ftpFolder, DocFolder, LogFolder) Then printed = False Else printed = True
    
    If Not mbs Then
        If MsgBox("This script prints letters reminding rehabilitated borrowers of the dangers of re-entering default.  Please set the printer to double-sided printing. Click OK when you're ready to continue.", vbInformation + vbOKCancel, "Rehabilitated Borrowers") <> vbOK Then End
    End If
    
    'Check for log file
    If Dir(LogFolder & "RHBRWINPCRecovery.txt") <> "" Then       'File exists, go into recovery mode
        Open LogFolder & "RHBRWINPCRecovery.txt" For Input As #2
            Input #2, RValue        'Get the value of the last record processed
        Close #2
    End If
    
    'Process as many reports as are present.
    SasFile = Dir(ftpFolder & "ULWD30.LWD30R3*")
    If SasFile = "" Then        'There are no SAS files to begin with; stop the script.
        MsgBox "The SAS reports are missing from the FTP folder. Contact Systems Support for assistance.", vbOKOnly + vbCritical, "Missing SAS File(s)"
        End
    Else        'We have files to work with.
        Do While SasFile <> ""
            'Empty SAS files mean no records were returned, so only process it if there are records to process.
            If FileLen(ftpFolder & SasFile) <> 0 Then
                'Print the state mail cover sheet and letters
                Dim newF As String
                newF = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "DATREDEF", True)
                SP.CostCenterPrinting.Main DocFolder, "DATREDEF", "DEFAULT AVERSION RE-DEFAULT AFTER REHAB LETTER", 1, "DATREDEF", newF, timeStamp, "RHBRWINPC", True, printed
                If Dir(newF) <> "" Then Kill newF
                'Fill in comments on OneLINK for each borrower
                SP.Common.Login
                UserID = SP.Common.GetUserID()
                ParseSasForComments ftpFolder & SasFile, "ALSBR", "RHBRWINPC", "LT", "03", "Default Aversion letter sent to borrower."
                HasRun = True
            End If
            'Delete the SAS file:
            Kill ftpFolder & SasFile
            'Dir() will return an empty string when a file is not found, breaking us out of our loop.
            SasFile = Dir(ftpFolder & "ULWD30.LWD30R3*")
        Loop
    End If
    
    'Clean up and finish
    If Dir(LogFolder & "RHBRWINPCRecovery.txt") <> "" Then Kill LogFolder & "RHBRWINPCRecovery.txt"
    If mbs Then
        ProcComp "MBSRHBRWINPC.TXT"
    Else
        If HasRun Then
            MsgBox "The letters have finished processing. Please set your printer back to single-sided printing.", vbInformation + vbOKOnly, "Processing Complete"
        Else
            MsgBox "File empty, processing complete", vbInformation, "Processing complete"
        End If
    End If
End Sub

'The ParseSasForComments subroutine is a go-between that opens a SAS file and calls the AddLP50 subroutine for each record in the SAS file.
' Besides the name of the SAS file, the arguments for AddLP50 need to be passed in (except for the SSN, which is read out of the SAS file).
Sub ParseSasForComments(SasFile As String, ActCd As String, Script As String, Optional ActTyp As String = "", Optional ConTyp As String = "", Optional comment As String = "", Optional PauPlea As Boolean = False, Optional IsRef As Boolean = False, Optional tSSN As String)
    Dim Record As String        'Holds a record from the SAS file
    Dim Field() As String       'Fields from the record
    
    Open SasFile For Input As #1
    'The first line in the SAS file is the field names, so read it in to get it out of the way.
    Line Input #1, Record
    
    'If we're in recovery mode, we also need to skip all the lines up to and including the last one we processed.
    If RValue <> "" Then
        Do
            Line Input #1, Record
            Field = Split(Record, ",")
            If Field(0) = RValue Then
                'Reset the recovery variable to get out of recovery mode, and move on to normal processing.
                RValue = ""
                Exit Do
            End If
        Loop
    End If
    
    'Now process the remaining records
    Do While Not EOF(1)
        Line Input #1, Record
        'Split the record's fields into an array
        Field = Split(Record, ",")
        'Pass the account number and passed-in arguments to ATD22ByPgm to add comments for this borrower
        SP.Common.AddLP50 Field(0), ActCd, Script, ActTyp, ConTyp, comment ', PauPlea, IsRef, tSSN
        'Write the account number out to the log file
        Open LogFolder & "RHBRWINPCRecovery.txt" For Output As #2
        Write #2, Field(0)
        Close #2
    Loop
    Close #1
End Sub
