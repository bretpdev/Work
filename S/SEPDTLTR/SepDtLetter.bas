Attribute VB_Name = "SepDtLetter"
Private ftpFolder As String
Private DocFolder As String
Private SasFile As String
Private ToPrinter As Boolean
Private AN As String
Private SepDate As String
Private v As String

'print letters for borrowers who are no longer attending school
Sub SepDtLetter()
    Dim ARC As String
    Dim comment As String
    Dim UserID As String
    
    'warn the user of the purpose of the script and end the script if the dialog box is cancelled
    If Not SP.Common.CalledByMBS Then
        warn = MsgBox("This script print letters for borrowers who are no longer attending school at least half time.  Click OK to continue or Cancel to quit.", 65, "Separation Date Letter")
        If warn = 2 Then End
        'prompt the user to set the printer to duplex
        MsgBox "The letters printed by this script must be printed two-sided.  Please set the printer to duplex and then click OK to continue.", 64, "Duplex"
    End If
    'set values
    DocFolder = "X:\PADD\BorrowerServices\"
    If SP.Common.TestMode(ftpFolder, DocFolder) Then
        PrnCls = "N"
        ToPrinter = False
    Else
        PrnCls = "Y"
        ToPrinter = True
    End If
    'Make a new data file for comments.
    Open "T:\sepcom.txt" For Output As #2
    Close #2
    'get and process each SAS file
    Do
        'get the name of the SAS file
        SasFile = Dir(ftpFolder & "ULWS01.LWS01R2.*.*")
        'exit the loop if the file is not found
        If SasFile = "" Then Exit Do
        If FileLen(ftpFolder & SasFile) <> 0 Then LetterProc
        Kill ftpFolder & SasFile
    Loop
    'add info from SAS files to data file for comments
    Do
        'get the name of the SAS file
        SasFile = Dir(ftpFolder & "ULWS01.LWS01R3.*.*")
        'exit the loop if the file is not found
        If SasFile = "" Then Exit Do
        If FileLen(ftpFolder & SasFile) <> 0 Then
            'open the file
            Open ftpFolder & SasFile For Input As #1
            'read the header row
            Line Input #1, Data
            'read each data row to get the SSN and sep date
            Do Until EOF(1)
                Input #1, ssn, AN, v, v, v, v, v, v, v, v, v, v, SepDate, v, v, v, _
                      v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
                      v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
                      v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
                      v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
                      v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
                      v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
                      v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
                      v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
                      v, v, v, v, v, v
                'write the ssn, sep date and a comment to the comment data file
                Open "T:\sepcom.txt" For Append As #2
                Write #2, ssn, SepDate, "sep date letter not mailed due to an invalid address", "ELBS2"
                Close #2
            Loop
            Close #1
        End If
        'delete the original
        Kill ftpFolder & SasFile
    Loop
    'close the data file
    Close #2
    'add comments
    'TODO: See if a TD22 function from SP.Common will work just as well.
    Open "T:\sepcom.txt" For Input As #2
    Do Until EOF(2)
        Input #2, ssn, SepDate, comment, ARC
        'access TD22
        FastPath "TX3Z/ATD22" & ssn
        'find the RIA28 ARC
        Do
            found = Session.FindText(ARC, 8, 8)
            If found Then Exit Do
            Hit "F8"
        Loop
        'select the ARC
        PutText Session.FoundTextRow, Session.FoundTextColumn - 5, "01", "ENTER"
        'find the loan
        Do
            found = Session.FindText(Format(AppNo, "000"), 11, 5)
            If found Then Exit Do
            Hit "F8"
        Loop
        'select the loan
        PutText Session.FoundTextRow, 3, "X"
        'enter the comment
        PutText 21, 2, comment & "  {SEPDTLTR} / " & UserID, "ENTER"
    Loop
    Close #2
    
    'delete temporary files
    If Dir("T:\sepcom.txt") <> "" Then Kill "T:\sepcom.txt"
    If Dir("T:\sepdat.txt") <> "" Then Kill "T:\sepdat.txt"
    
    'prompt the user to resset the printer
    If Not SP.Common.CalledByMBS Then MsgBox "The script is done printing letters.  Please reset the printer to print single-sided and then click OK to continue.", 64, "Duplex"
    ProcComp "MBSSEPDTLTR.txt"
End Sub

'process the SAS file to print letters
Sub LetterProc()
    Dim TSTMP As String
    Dim fso As Object
    Dim ltrcntr As Integer
    
    TSTMP = CStr(Now())
    Set fso = CreateObject("Scripting.FileSystemObject")
    'copy the SAS file to be used as a Word merge data file
    fso.CopyFile ftpFolder & SasFile, "T:\sepdat.txt"
    'open the file
    Open "T:\sepdat.txt" For Input As #1
    'read the header row
    Line Input #1, Data
    'read each data row to get the SSN and sep date
    ltrcntr = 0
    Do Until EOF(1)
        Input #1, ssn, AN, v, v, v, v, v, v, v, v, v, v, SepDate, v, v, v, _
              v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
              v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
              v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
              v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
              v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
              v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
              v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
              v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, v, _
              v, v, v, v, v, v, v, v
        'write the ssn, sep date and a comment to the comment data file
        Open "T:\sepcom.txt" For Append As #2
        Write #2, ssn, SepDate, "sep date letter mailed", "ELBS1"
        Close #2
        'count the number of records for the total letters printed
        ltrcntr = ltrcntr + 1
    Loop
    Close #1
    'print the letters
    Barcode2D.AddBarcodeAndStaticCurrentDate "T:\sepdat.txt", "DF_SPE_ACC_ID", "SEPLTR"
    SP.CostCenterPrinting.Main DocFolder, "SEPLTR", "Separate Date Letter", 1, "SEPLT", "T:\sepdat.txt", TSTMP, "SEPDTLTR", False, ToPrinter
End Sub
