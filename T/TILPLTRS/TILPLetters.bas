Attribute VB_Name = "TILPLetters"
Private ftpFolder As String
Private DocFolder As String
Private LogFolder As String
Private printed As Boolean
Private UserID As String
Private RValue As String        'Recovery variable, should only have a value if in recovery mode

Sub Main()
    Dim mbs As Boolean
    Dim timeStamp As String
    Dim SasFile As String
    Dim mergeFile As String
    
    DocFolder = "X:\PADD\TILP\"
    mbs = SP.Common.CalledByMBS()
    timeStamp = Time
    
    If Not mbs Then
        If MsgBox("This script prints TILP letters and adds activity records.  Click OK to continue.", vbInformation + vbOKCancel, "TILP Specific Letters") <> vbOK Then End
    End If
    
    UserID = SP.Common.GetUserID()
    
    If SP.Common.TestMode(ftpFolder, DocFolder, LogFolder) Then printed = False Else printed = True
    
    'Check for log file
    If Dir(LogFolder & "TILPRecovery.txt") <> "" Then       'File exists, go into recovery mode
        Open LogFolder & "TILPRecovery.txt" For Input As #2
            Input #2, RValue        'Get the value of the last record processed
        Close #2
    End If
    
    If RValue = "" And Not SasFilesPresent() Then   'We're not in recovery, and files are missing
        MsgBox "There are SAS files missing from the FTP folder. Contact Systems Support for assistance.", vbOKOnly + vbCritical, "Missing SAS File(s)"
        End
    End If
    
    'Process each file in turn
    'Dir will return an empty string if the SAS file isn't there, which means we're in recovery mode.
    SasFile = Dir(ftpFolder & "ULWG96.LWG96R2*")
    Do While SasFile <> ""
        'Empty SAS files mean no records were returned, so only process it if there are records to process.
        If FileLen(ftpFolder & SasFile) <> 0 Then
            'Print the state mail cover sheet and letters
            mergeFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "CNGRTS", True)
            SP.CostCenterPrinting.Main DocFolder, "CNGRTS", "TILP Graduation Letters", SpclInstFromLetterTracker, "CNGRTS", mergeFile, timeStamp, "TILPLTRS", True, printed
            Kill mergeFile
            'Fill in comments on COMPASS for each borrower
            ParseSasForComments ftpFolder & SasFile, "TLPGR", "TILP", "TILP Graduation Letter sent to Borrower", "TILP Specific Letters"
        End If
        'Delete the SAS file
        Kill ftpFolder & SasFile
        'get next file
        SasFile = Dir(ftpFolder & "ULWG96.LWG96R2*")
    Loop
    SasFile = Dir(ftpFolder & "ULWG96.LWG96R3*")
    Do While SasFile <> ""
        If FileLen(ftpFolder & SasFile) <> 0 Then
            mergeFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "NOTCHLS", True)
            SP.CostCenterPrinting.Main DocFolder, "NOTCHLS", "TILP Teaching License Received", Page1, "NOTCHLS", mergeFile, timeStamp, "TILPLTRS", True, printed
            Kill mergeFile
            ParseSasForComments ftpFolder & SasFile, "TLPTL", "TILP", "TILP Teaching License Not Received Letter sent to Borrower", "TILP Specific Letters"
        End If
        Kill ftpFolder & SasFile
        SasFile = Dir(ftpFolder & "ULWG96.LWG96R3*")
    Loop
    SasFile = Dir(ftpFolder & "ULWG96.LWG96R4*")
    Do While SasFile <> ""
        If FileLen(ftpFolder & SasFile) <> 0 Then
            mergeFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "BELOAWLEXP", True)
            SP.CostCenterPrinting.Main DocFolder, "BELOAWLEXP", "TILP LOA Will Expire", Page1, "BELOAWLEXP", mergeFile, timeStamp, "TILPLTRS", True, printed
            Kill mergeFile
            ParseSasForComments ftpFolder & SasFile, "TLLAE", "TILP", "TILP Leave of Absence Will Expire Letter sent to Borrower", "TILP Specific Letters"
        End If
        Kill ftpFolder & SasFile
        SasFile = Dir(ftpFolder & "ULWG96.LWG96R4*")
    Loop
    SasFile = Dir(ftpFolder & "ULWG96.LWG96R5*")
    Do While SasFile <> ""
        If FileLen(ftpFolder & SasFile) <> 0 Then
            mergeFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "BELOAEXPAY", True)
            SP.CostCenterPrinting.Main DocFolder, "BELOAEXPAY", "TILP LOA Has Expired", Page1, "BELOAEXPAY", mergeFile, timeStamp, "TILPLTRS", True, printed
            Kill mergeFile
            ParseSasForComments ftpFolder & SasFile, "TLLAX", "TILP", "TILP Leave of Absence Has Expired Letter sent to Borrower", "TILP Specific Letters"
        End If
        Kill ftpFolder & SasFile
        SasFile = Dir(ftpFolder & "ULWG96.LWG96R5*")
    Loop
    SasFile = Dir(ftpFolder & "ULWG96.LWG96R6*")
    Do While SasFile <> ""
        If FileLen(ftpFolder & SasFile) <> 0 Then
            mergeFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "TILPSTATUS", True)
            SP.CostCenterPrinting.Main DocFolder, "TILPSTATUS", "TILP Borrower Dropped", Page1, "TILPSTATUS", mergeFile, timeStamp, "TILPLTRS", True, printed
            Kill mergeFile
            ParseSasForComments ftpFolder & SasFile, "TLPDP", "TILP", "Borrower Dropped from TILP Program Letter sent to Borrower", "TILP Specific Letters"
        End If
        Kill ftpFolder & SasFile
        SasFile = Dir(ftpFolder & "ULWG96.LWG96R6*")
    Loop
    SasFile = Dir(ftpFolder & "ULWG96.LWG96R7*")
    Do While SasFile <> ""
        If FileLen(ftpFolder & SasFile) <> 0 Then
            mergeFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "TILPSIXSEM", True)
            SP.CostCenterPrinting.Main DocFolder, "TILPSIXSEM", "TILP Borrower Never Accepted", Page1, "TILPSIXSEM", mergeFile, timeStamp, "TILPLTRS", True, printed
            Kill mergeFile
            ParseSasForComments ftpFolder & SasFile, "TLBNA", "TILP", "Borrower Never Accepted to Teaching Program Letter sent to Borrower", "TILP Specific Letters"
        End If
        Kill ftpFolder & SasFile
        SasFile = Dir(ftpFolder & "ULWG96.LWG96R7*")
    Loop
    SasFile = Dir(ftpFolder & "ULWG96.LWG96R8*")
    Do While SasFile <> ""
        If FileLen(ftpFolder & SasFile) <> 0 Then
            mergeFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "TCHSTCVLT", True)
            SP.CostCenterPrinting.Main DocFolder, "TCHSTCVLT", "TILP 1 Yr Graced Used", Page2, "TCHSTCVLT", mergeFile, timeStamp, "TILPLTRS", True, printed
            Kill mergeFile
            ParseSasForComments ftpFolder & SasFile, "TI1YR", "TILP", "One Year TILP Grace Used Letter sent to Borrower", "TILP Specific Letters"
        End If
        Kill ftpFolder & SasFile
        SasFile = Dir(ftpFolder & "ULWG96.LWG96R8*")
    Loop
    SasFile = Dir(ftpFolder & "ULWG96.LWG96R9*")
    Do While SasFile <> ""
        If FileLen(ftpFolder & SasFile) <> 0 Then
            mergeFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "TILPMAXSEM", True)
            SP.CostCenterPrinting.Main DocFolder, "TILPMAXSEM", "TILP Max Semesters Used", Page1, "TILPMAXSEM", mergeFile, timeStamp, "TILPLTRS", True, printed
            Kill mergeFile
            ParseSasForComments ftpFolder & SasFile, "TLMSA", "TILP", "TILP Maximum Semesters Allotted Used Letter sent to Borrower", "TILP Specific Letters"
        End If
        Kill ftpFolder & SasFile
        SasFile = Dir(ftpFolder & "ULWG96.LWG96R9*")
    Loop
    SasFile = Dir(ftpFolder & "ULWG96.LWG96R10*")
    Do While SasFile <> ""
        If FileLen(ftpFolder & SasFile) <> 0 Then
            mergeFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "STATNTRCV", True)
            SP.CostCenterPrinting.Main DocFolder, "STATNTRCV", "TILP Teaching Status Follow Up", Page2, "STATNTRCV", mergeFile, timeStamp, "TILPLTRS", True, printed
            Kill mergeFile
            ParseSasForComments ftpFolder & SasFile, "TI1FU", "TILP", "TILP Teaching Status Follow Up Letter sent to Borrower", "TILP Specific Letters"
        End If
        Kill ftpFolder & SasFile
        SasFile = Dir(ftpFolder & "ULWG96.LWG96R10*")
    Loop
    'The following SAS file is a monthly report from a different SAS job. If it's not present, just skip it.
    SasFile = Dir(ftpFolder & "ULWS26.LWS26R2*")
    Do While SasFile <> ""
        If FileLen(ftpFolder & SasFile) <> 0 Then
            mergeFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "DF_SPE_ACC_ID", "CR4TCH", True)
            SP.CostCenterPrinting.Main DocFolder, "CR4TCH", "TILP Credit for Teaching", Page1, "CR4TCH", mergeFile, timeStamp, "TILPLTRS", True, printed
            Kill mergeFile
            ParseSasForComments ftpFolder & SasFile, "TLTCP", "TILP", "TILP Teaching Teaching Credit Applied Letter sent to Borrower", "TILP Specific Letters"
        End If
        Kill ftpFolder & SasFile
        SasFile = Dir(ftpFolder & "ULWS26.LWS26R2*")
    Loop

    'Delete the recovery log file now that all records are processed
    If Dir(LogFolder & "TILPRecovery.txt") <> "" Then Kill LogFolder & "TILPRecovery.txt"
    
    If mbs Then ProcComp "MBSTILPLTRS.TXT" Else MsgBox "The TILP letters have finished processing.", vbInformation + vbOKOnly, "TILP Processing Complete"
End Sub

'This fucntion finds the newest set of required SAS files in the FTP folder,
' checks whether all are present, and deletes the old files.
Function SasFilesPresent() As Boolean
    If Dir(ftpFolder & "ULWS26.LWS26R2*") <> "" Then SP.Common.DeleteOldFilesReturnMostCurrent ftpFolder, "ULWS26.LWS26R2*"
    If (Dir(ftpFolder & "ULWG96.LWG96R2*") <> "") And _
        (Dir(ftpFolder & "ULWG96.LWG96R3*") <> "") And _
        (Dir(ftpFolder & "ULWG96.LWG96R4*") <> "") And _
        (Dir(ftpFolder & "ULWG96.LWG96R5*") <> "") And _
        (Dir(ftpFolder & "ULWG96.LWG96R6*") <> "") And _
        (Dir(ftpFolder & "ULWG96.LWG96R7*") <> "") And _
        (Dir(ftpFolder & "ULWG96.LWG96R8*") <> "") And _
        (Dir(ftpFolder & "ULWG96.LWG96R9*") <> "") And _
        (Dir(ftpFolder & "ULWG96.LWG96R10*") <> "") Then
        
        SasFilesPresent = SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWG96.LWG96R2*") <> "" And _
        SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWG96.LWG96R3*") <> "" And _
        SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWG96.LWG96R4*") <> "" And _
        SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWG96.LWG96R5*") <> "" And _
        SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWG96.LWG96R6*") <> "" And _
        SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWG96.LWG96R7*") <> "" And _
        SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWG96.LWG96R8*") <> "" And _
        SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWG96.LWG96R9*") <> "" And _
        SP.Common.DeleteOldFilesReturnMostCurrent(ftpFolder, "ULWG96.LWG96R10*") <> ""
    Else
        SasFilesPresent = False
    End If
End Function

'The ParseSasForComments subroutine is a go-between that opens a SAS file and calls the ATD22ByPgm subroutine for each record in the SAS file.
' Besides the name of the SAS file, the arguments for ATD22ByPgm need to be passed in (except for the SSN, which is read out of the SAS file).
Sub ParseSasForComments(SasFile As String, ARC As String, PGM As String, comment As String, Script As String)
    Dim Record As String        'Holds a record from the SAS file
    Dim Field() As String       'Fields from the record
    Dim SSNCol As Integer
    
    Open SasFile For Input As #1
    'The first line in the SAS file is the field names, so read it in to get it out of the way
    Line Input #1, Record
    'Get the index of the SSN column.
    Field = Split(Record, ",")
    For SSNCol = 0 To UBound(Field)
        If Field(SSNCol) = "BF_SSN" Then Exit For
    Next SSNCol

    'If we're in recovery mode, we also need to skip all the lines up to and including the last one we processed
    If RValue <> "" Then
        Do
            Line Input #1, Record
            Field = Split(Record, ",")
            If Field(SSNCol) = RValue Then
                'Reset the recovery variable to get out of recovery mode, and move on to normal processing
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
        ATD22ByPgm Field(SSNCol), ARC, PGM, comment, Script, UserID
        'Write the account number out to the log file
        Open LogFolder & "TILPRecovery.txt" For Output As #2
        Write #2, Field(SSNCol)
        Close #2
    Loop
    Close #1
End Sub

'add ARC in TD22 for a given loan program
Function ATD22ByPgm(ssn As String, ARC As String, PGM As String, comment As String, Script As String, UserID As String, Optional PauPls As Boolean = False) As Boolean
    Dim row As Integer
    ATD22ByPgm = True
   
    FastPath "TX3Z/ATD22" & ssn & ";" & ARC
    If Not Check4Text(1, 72, "TDX24") Then
        ATD22ByPgm = False
        Exit Function
    End If
    
    'review each loan for selection
    row = 11
    Do
        If Trim(GetText(row, 61, 6)) = PGM Then PutText row, 3, "X"
        row = row + 1
        If Not Check4Text(row, 3, "_") Then
            If Check4Text(8, 75, "+") Then
                Hit "F8"
                Do While Check4Text(23, 2, "03483")
                    'bypass loan because 03483 SELECTED LOAN(S) IN DDB STATUS; CAN NOT BE ASSOCIATED TO ACTY REQUEST
                    Hit "END"
                    Hit "F8"
                Loop
                row = 11
            Else
                Exit Do
            End If
        End If
    Loop
    
    'enter short comments
    If Len(comment) < 132 Then
        PutText 21, 2, comment & "  {" & Script & "} /" & UserID
        
        'commit changes and handle errors
        Hit "ENTER"
        While Not Check4Text(23, 2, "02860")
            MsgBox "An error was recieved while adding a " & ARC & " activity record in Compass.  Please correct the error and then hit <Insert> to continue.", 48, "Activity Record Error"
            SP.PauseForInsert
            Hit "ENTER"
        Wend
    'enter long comments
    Else
        'fill the first screen, commit changes, and handle errors
        PutText 21, 2, MID(comment, 1, 154), "ENTER"
        While Not Check4Text(23, 2, "02860")
            MsgBox "An error was recieved while adding a " & ARC & " activity record in Compass.  Please correct the error and then hit <Insert> to continue.", 48, "Activity Record Error"
            SP.PauseForInsert
            Hit "ENTER"
        Wend
        Hit "F4"
        'enter the rest on the expanded comments screen
        For k = 155 To Len(comment) Step 260
            Session.TransmitANSI MID(comment, k, 260)
        Next
        Session.TransmitANSI "  {" & Script & "} /" & UserID
        
        'commit changes and handle errors
        Hit "ENTER"
        While Not Check4Text(23, 2, "02114")
            MsgBox "An error was recieved while adding a " & ARC & " activity record in Compass.  Please correct the error and then hit <Insert> to continue.", 48, "Activity Record Error"
            SP.PauseForInsert
            Hit "ENTER"
        Wend
    End If
End Function

