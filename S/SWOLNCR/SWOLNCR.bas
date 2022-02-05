Attribute VB_Name = "SWOLNCR"
Private Enum SAS
    'The R2 SAS file has a combined SSN (9 digits) and loan sequence number (4 digits) for each record, followed by a bond ID.
    SSN_LoanSeq = 0
    bondID = 1
End Enum
Private FTPFolder As String
Private DocFolder As String
Private LogFolder As String
Private ArchiveFolder As String
Private ProcValue As String
Private RValue As String        'Recovery variable, should only have a value if in recovery mode
Private SelectedBuyerOwnerID As String

Sub Main()
    Dim MBS As Boolean
    Dim Testing As Boolean
    Dim SASFile As String
    Dim ReportFile As String
    Dim RedeemSaleDate As String
    Dim ServIDs() As String
       
    DocFolder = "T:\"
    MBS = Sp.Common.CalledByMBS()

    'prompt user to select process to run
    frmSWOLNCR.Show
    If frmSWOLNCR.rdoSpecial.Value = True Then
        ProcValue = "Special"
    ElseIf frmSWOLNCR.rdoECASLA.Value = True Then
        ProcValue = "ECASLA"
    ElseIf frmSWOLNCR.rdoRedeem.Value = True Then
        ProcValue = "Redeem"
        RedeemSaleDate = frmSWOLNCR.txtDate.Text
         
        ServIDs = Sp.Common.SqlEx("SELECT ID, Name FROM GENR_REF_LoanSaleList WHERE SaleType = 'Servicer'")
    
        Dim i As Integer
        i = 0
         'populate form with Lenders IDs and Names
        Do While i < UBound(ServIDs, 2)
            frmSWOLNDropDown.drpdownEdServicer.AddItem (ServIDs(0, i) + " - " + ServIDs(1, i))
            i = i + 1
        Loop
        frmSWOLNDropDown.Show
        SelectedBuyerOwnerID = frmSWOLNDropDown.drpdownEdServicer.Text
        SelectedBuyerOwnerID = StrReverse(SelectedBuyerOwnerID)
        SelectedBuyerOwnerID = Right(SelectedBuyerOwnerID, 6)
        SelectedBuyerOwnerID = StrReverse(SelectedBuyerOwnerID)
    End If
    Unload frmSWOLNCR
    
    If ProcValue = "Special" Then
        ArchiveFolder = "X:\Archive\Special Write off Program\"
        Sp.Common.TestMode , ArchiveFolder
        Testing = Sp.Common.TestMode(FTPFolder, , LogFolder)
    
        ReportFile = FTPFolder & "SWOSALEIDS.txt"
        
        If Not MBS Then
            If MsgBox("The script will create loan sales for the Special Write off Program.", vbOKCancel + vbInformation, "Special Write off Program") <> vbOK Then End
        End If
        
        'Check for log file
        If Dir(LogFolder & "SWOLNCRRecovery.txt") <> "" Then       'File exists, go into recovery mode
            Open LogFolder & "SWOLNCRRecovery.txt" For Input As #1
                Input #1, RValue        'Get the value of the last record processed
            Close #1
        End If
        
        'Check that the required SAS files are there.
        If RValue = "" And Dir(FTPFolder & "ULWA16.LWA16R2*") = "" Then
            MsgBox "The SAS files are not present. Please contact Systems Support for assistance.", vbOKOnly + vbCritical, "File not found"
            End
        End If
        
        'Process the SAS file.
        SASFile = Dir(FTPFolder & "ULWA16.LWA16R2*")
        ProcessSAS FTPFolder & SASFile, ReportFile
        
        'Print the list of Sale IDs and Bond IDs.
        PrintReport ReportFile
        
        'Delete the recovery log file now that all records are processed
        Kill LogFolder & "SWOLNCRRecovery.txt"
        
        If Not MBS Then
            MsgBox "The loan sales have been created, and a report of sale IDs and bond IDs has been printed.", vbInformation + vbOKOnly, "Processing Complete"
        End If
        
        
        
  '**************************************'
        
        
        
    ElseIf ProcValue = "ECASLA" Then
        ArchiveFolder = "X:\Archive\Delinquent ECASLA\"
        Sp.Common.TestMode , ArchiveFolder
        Testing = Sp.Common.TestMode(FTPFolder, , LogFolder)

        ReportFile = FTPFolder & "ECASLA.txt"
        
        If Not MBS Then
            If MsgBox("The script will create loan sales to transfer delinquent ECASLA loans.", vbOKCancel + vbInformation, "Delinquent ECASLA Loans") <> vbOK Then End
        End If
        
        'Check for log file
        If Dir(LogFolder & "ECASLARecovery.txt") <> "" Then       'File exists, go into recovery mode
            Open LogFolder & "ECASLARecovery.txt" For Input As #1
                Input #1, RValue        'Get the value of the last record processed
            Close #1
        End If
        
        'Check that the required SAS files are there.
        'If RValue = "" And Dir(FTPFolder & "ULWR16.LWR16R2*") = "" Then
        If RValue = "" And Dir(FTPFolder & "ULWR16.LWR16R2*") = "" Then
            MsgBox "The SAS files are not present. Please contact Systems Support for assistance.", vbOKOnly + vbCritical, "File not found"
            End
        End If
        
        'Process the SAS file.
        'SASFile = Dir(FTPFolder & "ULWR16.LWR16R2*")
        SASFile = Dir(FTPFolder & "ULWR16.LWR16R2*")
        ProcessSAS FTPFolder & SASFile, ReportFile
        
        'Print the list of Sale IDs and Bond IDs.
        PrintReport ReportFile
        
        'Delete the recovery log file now that all records are processed
        Kill LogFolder & "ECASLARecovery.txt"
        
        If Not MBS Then
            MsgBox "The loan sales have been created, and a report of sale IDs and bond IDs has been printed.", vbInformation + vbOKOnly, "Processing Complete"
        End If
    
    ElseIf ProcValue = "Redeem" Then
        ArchiveFolder = "X:\Archive\Put for Redeemed Loans\"
        Sp.Common.TestMode , ArchiveFolder
        Testing = Sp.Common.TestMode(FTPFolder, , LogFolder)

        ReportFile = FTPFolder & "PUTREDEEMEDLOANS.txt"
        
        If Not MBS Then
            If MsgBox("The script will create loan sales for the Put Loan Sale for Redeemed Loans.", vbOKCancel + vbInformation, "Delinquent ECASLA Loans") <> vbOK Then End
        End If
        
        'Check for log file
        If Dir(LogFolder & "RedeemRecovery.txt") <> "" Then       'File exists, go into recovery mode
            Open LogFolder & "RedeemRecovery.txt" For Input As #1
                Input #1, RValue        'Get the value of the last record processed
            Close #1
        End If
        
        'Check that the required SAS files are there.
        'If RValue = "" And Dir(FTPFolder & "PUTREDEEMEDLOANS.txt") = "" Then
        If RValue = "" And Dir(FTPFolder & "ULWA33.LWA33R2*") = "" Then
            MsgBox "The SAS files are not present. Please contact Systems Support for assistance.", vbOKOnly + vbCritical, "File not found"
            End
        End If
        
        'Process the SAS file.
        SASFile = Dir(FTPFolder & "ULWA33.LWA33R2*")
        ProcessSASRedeem FTPFolder & SASFile, ReportFile, RedeemSaleDate
        
        'Print the list of Sale IDs and Bond IDs.
        PrintReport ReportFile
        
        'Delete the recovery log file now that all records are processed
        Kill LogFolder & "RedeemRecovery.txt"
        
        If Not MBS Then
            MsgBox "The loan sales have been created, and a report of sale IDs and bond IDs has been printed.", vbInformation + vbOKOnly, "Processing Complete"
        End If
    End If
    
    End
End Sub

Sub ProcessSAS(SASFile As String, ReportFile As String)
    Dim Record As String
    Dim Field() As String
    Dim SaleID As String
    Dim LastBondID As String
    Dim SQLBondID() As String
    
    Dim EffSaleDate As String
    Dim BuyerOwner As String
    Dim PortfolioID As String
    
    If ProcValue = "Special" Then
        EffSaleDate = Format(Date + 2, "MMDDYYYY")
        BuyerOwner = "828476"
        PortfolioID = "Special W/O Program"
    ElseIf ProcValue = "ECASLA" Then
        EffSaleDate = Format(Date + 1, "MMDDYYYY")
        'BuyerOwner = "834396"
        BuyerOwner = "82847601"
        PortfolioID = "Del ECASLA"
    End If
    
    Open SASFile For Input As #2
    
    'If we're in recovery mode, we need to skip all the lines up to and including the last one we processed.
    If RValue <> "" Then
        Do
            Line Input #2, Record
            Field = Split(Record, ",")
            If Field(SAS.bondID) = RValue Then
                'Reset the recovery variable to get out of recovery mode, and move on to normal processing.
                RValue = ""
                Exit Do
            End If
        Loop
    End If
    
    'Get the first data record and split it into an array of its fields.
    Line Input #2, Record
    Field() = Split(Record, ",")
        
    Do
        'Access TS4P in add mode.
        FastPath "TX3Z/ATS4P"
        'Enter information in the screen.
        puttext 4, 30, "T"               'Sale Type
        EffSaleDate = Replace(EffSaleDate, "/", "")
        puttext 5, 23, EffSaleDate      'Effective Sale Date
        puttext 6, 20, "828476"         'Seller Owner ID
        puttext 7, 20, "Dave"           'Contact (first name)
        puttext 7, 34, "Schwanke"       'Contact (last name)
        puttext 7, 67, "8013217276"     'Phone
        puttext 8, 20, Field(SAS.bondID)    'Bond Issue
        puttext 9, 20, BuyerOwner       'Buyer Owner ID
        puttext 10, 20, "Dave"          'Contact (first name)
        puttext 10, 34, "Schwanke"      'Contact (last name)
        puttext 10, 67, "8013217276"    'Phone
        If ProcValue = "Special" Then        'Bond Issue
        SQLBondID = Sp.Common.SqlEx("SELECT BondID FROM GENR_REF_SpecialRefBondID")
            puttext 11, 20, SQLBondID(0, 0)
        ElseIf ProcValue = "ECASLA" Then
            puttext 11, 20, Field(SAS.bondID)
'            PutText 11, 20, "932006DD"  'test bond ID set up to work with 834396
        End If
        puttext 12, 16, PortfolioID     'Portfolio ID
        puttext 19, 27, "N"             'Trigger letter
        puttext 19, 47, "", "END"
        puttext 20, 18, "B"             'EFT Owner Resp
        puttext 20, 59, "S"             'BBS Owner Resp
        puttext 19, 79, "N"             'Write Off Late Fee
        hit "ENTER"     'Commit data
        
        hit "F4"    'Go to TS9N
        puttext 9, 18, "S"              'Selection Type
        puttext 21, 20, "U"             'Selection Status
        puttext 17, 16, "S"              'Org Fee Resp

        hit "ENTER"
        SaleID = GetText(5, 11, 7)
        
        'Create a file named for the Sale ID.
        Open FTPFolder & SaleID & ".txt" For Output As #4
        'Set LastBondID to this record's BondID.
        LastBondID = Field(SAS.bondID)
        'Write out loan data for BondIDs that match this one.
        Do While Field(SAS.bondID) = LastBondID
            Print #4, Field(SAS.SSN_LoanSeq)
            If EOF(2) Then Exit Do
            Line Input #2, Record
            Field() = Split(Record, ",")
        Loop
        Close #4
        Set fso = CreateObject("Scripting.FileSystemObject")
        fso.CopyFile FTPFolder & SaleID & ".txt", ArchiveFolder & SaleID & ".txt"
        
        'Add this Sale ID and Bond ID to the report data.
        Open ReportFile For Append As #3
        Print #3, SaleID; Tab; LastBondID
        Close #3
        
        'Update the recovery file
        If ProcValue = "Special" Then
            Open LogFolder & "SWOLNCRRecovery.txt" For Output As #1
        ElseIf ProcValue = "ECASLA" Then
            Open LogFolder & "ECASLARecovery.txt" For Output As #1
        End If
        Print #1, LastBondID
        Close #1
        If EOF(2) Then Exit Do
    Loop
    
    'All done processing data; delete the SAS file.
    Close #2
    Kill SASFile
End Sub

Sub PrintReport(ReportFile As String)
    Dim str As String
    Dim Word As Word.Application
    
    Set Word = CreateObject("Word.Application")
    
    Word.Visible = False
    Word.Documents.Add DocumentType:=wdNewBlankDocument
    'add a header
    Word.selection.Font.Size = 20
    If ProcValue = "Special" Then
        Word.selection.TypeText Text:="Special Write off Program Loan Sale Report"
    ElseIf ProcValue = "ECASLA" Then
        Word.selection.TypeText Text:="Delinquent ECASLA Loans Loan Sale Report"
    ElseIf ProcValue = "Redeem" Then
        Word.selection.TypeText Text:="Put for Redeemed Loans Loan Sale Report"
    End If
    Word.selection.TypeParagraph
    Word.selection.TypeParagraph
    Word.selection.Font.Size = 12
    Word.selection.TypeText Text:="Sale ID         Bond ID"
    Word.selection.TypeParagraph
    
    Open ReportFile For Input As #3
    While Not EOF(3)
        Line Input #3, str
        Word.selection.TypeText Text:=str
        Word.selection.TypeParagraph
    Wend
    Close #3
    
    Word.ActiveDocument.PrintOut True
    Word.Application.Quit SaveChanges:=wdDoNotSaveChanges
End Sub

Sub ProcessSASRedeem(SASFile As String, ReportFile As String, EffSaleDate As String)
    Dim Record As String
    Dim Field() As String
    Dim SaleID As String
    Dim LastBondID As String
    
    'Dim EffSaleDate As String
    'Dim BuyerOwner As String
    Dim RetreivedPhone As String
    Dim PortfolioID As String
    
    'If ProcValue = "Special" Then
    '    EffSaleDate = Format(Date + 2, "MMDDYYYY")
    '    BuyerOwner = "828476"
    'ElseIf ProcValue = "ECASLA" Then
    '    EffSaleDate = Format(Date + 1, "MMDDYYYY")
    '    BuyerOwner = "82847601"
    'End If


    ''SP.Q.FastPath "TX3Z/ITX08" + SelectedBuyerOwner
    
    ''RetreivedServicerName = Trim(GetText(6, 19, 25))
    ''RetreivedPhone = GetText(18, 20, 3) + GetText(18, 26, 3) + GetText(18, 30, 4)
    
    Open SASFile For Input As #2
    
    'If we're in recovery mode, we need to skip all the lines up to and including the last one we processed.
    If RValue <> "" Then
        Do
            Line Input #2, Record
            Field = Split(Record, ",")
            If Field(SAS.bondID) = RValue Then
                'Reset the recovery variable to get out of recovery mode, and move on to normal processing.
                RValue = ""
                Exit Do
            End If
        Loop
    End If
    
    'Get the first data record and split it into an array of its fields.
    Line Input #2, Record
    Field() = Split(Record, ",")
        
    Do
        'Access TS4P in add mode.
        FastPath "TX3Z/ATS4P"
        'Enter information in the screen.
        puttext 4, 30, "S"               'Sale Type
        puttext 4, 67, "P"               'Sale Sub Type
        EffSaleDate = Replace(EffSaleDate, "/", "")
        puttext 5, 23, EffSaleDate      'Effective Sale Date
        puttext 5, 76, "N"              'Approved
        puttext 6, 20, "828476"         'Seller Owner ID
        puttext 7, 20, "Dave"           'Contact (first name)
        puttext 7, 34, "Schwanke"       'Contact (last name)
        puttext 7, 67, "8013217276"     'Phone
        puttext 8, 20, Field(SAS.bondID)    'Bond Issue
        puttext 9, 20, SelectedBuyerOwnerID   'Buyer Owner ID
        puttext 10, 20, ""             'Contact (first name)
        puttext 10, 34, ""      'Contact (last name)
        puttext 10, 67, "8006992908"       'Phone

        puttext 17, 2, "LOANS DECONVERTED TO DEPARTMENT OF EDUCATION - PUT OPTION " + SelectedBuyerOwnerID + " 800 699 2908"     'Activity Text
        puttext 19, 27, "N"             'Trigger letter
        puttext 19, 47, "", "END"
        puttext 19, 79, "N"             'Write Off Late Fee

        hit "ENTER"     'Commit data

        puttext 21, 19, "FD"             'Write Off Late Fee
        
        hit "ENTER"     'Commit data
                
        hit "F4"    'Go to TS9N
        puttext 9, 18, "S"              'Selection Type
        puttext 17, 16, "S"              'Org Fee Resp
        puttext 21, 20, "U"             'Selection Status

        hit "ENTER"
        SaleID = GetText(5, 11, 7)
        
        'Create a file named for the Sale ID.
        Open FTPFolder & SaleID & ".txt" For Output As #4
        'Set LastBondID to this record's BondID.
        LastBondID = Field(SAS.bondID)
        'Write out loan data for BondIDs that match this one.
        Do While Field(SAS.bondID) = LastBondID
            Print #4, Field(SAS.SSN_LoanSeq)
            If EOF(2) Then Exit Do
            Line Input #2, Record
            Field() = Split(Record, ",")
        Loop
        Close #4
        Set fso = CreateObject("Scripting.FileSystemObject")
        fso.CopyFile FTPFolder & SaleID & ".txt", ArchiveFolder & SaleID & ".txt"
        
        'Add this Sale ID and Bond ID to the report data.
        Open ReportFile For Append As #3
        Print #3, SaleID; Tab; LastBondID
        Close #3
        
        'Update the recovery file
        Open LogFolder & "RedeemRecovery.txt" For Output As #1

        Print #1, LastBondID
        Close #1
        If EOF(2) Then Exit Do
    Loop
    
    'All done processing data; delete the SAS file.
    Close #2
    Kill SASFile
End Sub



