Attribute VB_Name = "ENDCO30DAY"
Private ftpFolder As String
Private DocFolder As String
Private LogFolder As String
Private printed As Boolean
Private UserID As String
Private recoveryValue As String        'Recovery variable: This should only have a value if in recovery mode.
Private Const SCRIPT_ID As String = "ENDCO30DAY"
Private Const LOG_FILE As String = SCRIPT_ID & ".txt"

Public Sub Main()
    'Before doing anything, make sure the user wants to run this script.
    If Not SP.Common.CalledByMBS() Then
        If MsgBox("This script sends letters to endorsers/comakers warning them that their wages will be garnished if payment arrangements are not made. Click OK to continue, or Cancel to quit.", vbOKCancel, "Endorser/Comaker 30 day notice") <> vbOK Then
            End
        End If
    End If
    
    'Set the module-level variables.
    DocFolder = "X:\PADD\LoanManagement\"
    If SP.Common.TestMode(ftpFolder, DocFolder, LogFolder) Then
        printed = False
    Else
        printed = True
    End If
    UserID = SP.Common.GetUserID()

    'Check for log file to see if we're in recovery.
    If Dir(LogFolder & LOG_FILE) <> "" Then       'File exists, go into recovery mode.
        Open LogFolder & LOG_FILE For Input As #2
            Input #2, recoveryValue        'Get the value of the last record processed.
        Close #2
    End If

    'Check for SAS files.
    If Dir(ftpFolder & "ULWD36.LWD36R2*") = "" Then
        MsgBox "Files are not available. Please contact Systems Support for assistance.", vbOKOnly + vbCritical, "Missing SAS File"
        End
    End If
        
    'Process all ULWD32 files found in the FTP directory.
    Dim timeStamp As String
    timeStamp = Time
    Dim SasFile As String
    Dim mergeDataFile As String
    Do While Dir(ftpFolder & "ULWD36.LWD36R2*") <> ""
        SasFile = Dir(ftpFolder & "ULWD36.LWD36R2*")
        'Empty SAS files mean no records were returned, so only process it if there are records to process.
        If FileLen(ftpFolder & SasFile) <> 0 Then
            'Print the state mail cover sheet and letters
            mergeDataFile = Barcode2D.AddBarcodeAndStaticCurrentDate(ftpFolder & SasFile, "AccountNumber", "30DAYEND", True)
            SP.CostCenterPrinting.Main DocFolder, "30DAYEND", "30 Day Notice Prior to Garnishment (Endorser)", Page4, "30DAYEND", mergeDataFile, timeStamp, SCRIPT_ID, True, printed
            'Fill in comments on COMPASS for each borrower
            ParseSasForComments ftpFolder & SasFile
        End If
        'Delete the SAS file
        Kill ftpFolder & SasFile
    Loop
    
    'Delete the recovery log file now that all records are processed
    If Dir(LogFolder & LOG_FILE) <> "" Then Kill LogFolder & LOG_FILE

    ProcComp "MBS30DAYEND.TXT"
End Sub

'The ParseSasForComments subroutine is a go-between that opens a SAS file and calls the AddLP50 subroutine for each record in the SAS file.
Private Sub ParseSasForComments(SasFile As String)
    Dim Record As String        'Holds a record from the SAS file
    Dim Field() As String       'Fields from the record
    
    Open SasFile For Input As #1
    'The first line in the SAS file is the field names, so read it in to get it out of the way
    Line Input #1, Record
    
    'If we're in recovery mode, we also need to skip all the lines up to and including the last one we processed
    If recoveryValue <> "" Then
        Do
            Line Input #1, Record
            Field = Split(Record, ",")
            If Field(0) = recoveryValue Then
                'Reset the recovery variable to get out of recovery mode, and move on to normal processing
                recoveryValue = ""
                Exit Do
            End If
        Loop
    End If
    
    'Now process the remaining records
    Do While Not EOF(1)
        Line Input #1, Record
        'Split the record's fields into an array
        Field = Split(Record, ",")
        'Call AddLP50 using the account number (item 0 in the array)
        AddLP50 Field(0), "DL30E", SCRIPT_ID, "LT", "69", "30 day notice prior to wage withholding sent to Endorser/Comaker"
        'LP9O needs an SSN, so get that from LP22.
        Dim ssn As String
        SP.Common.GetLP22 ssn, Field(0)
        'Set the due date on LP9O.
        SP.Common.AddLP9O ssn, "DEND30NT", Format(DateAdd("d", 30, Now), "MM/dd/yyyy")
        'Write the account number out to the log file
        Open LogFolder & LOG_FILE For Output As #2
        Write #2, Field(0)
        Close #2
    Loop
    Close #1
End Sub
