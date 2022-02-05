Attribute VB_Name = "DocCreateAndDeploy"
Option Explicit

Public Enum DeploymentMethod
    dmUserPrompt = 0
    dmFax = 1
    dmEmail = 2
    dmLetter = 3
End Enum

Public Enum SystemToAddComments
    stacCOMPASS = 0
    stacOneLINK = 1
    stacBoth = 2
End Enum

Public Enum LetterRecipient
    lrBorrower = 0
    lrReference = 1
    lrOther = 2
End Enum

'main sub that calls all helper functions as needed
Public Sub GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(LetterID As String, SystemToAddCommentsTo As SystemToAddComments, SSN As String, DataFile As String, AcctNumOrRefIDFieldNm As String, DocName As String, DocPath As String, UserId As String, scriptId As String, StateCodeFieldNm As String, Optional DeployMethod As DeploymentMethod = dmUserPrompt, Optional ContactType As String = "03", Optional LetterRecip As LetterRecipient = lrBorrower)
    Dim ProcessingMethod As DeploymentMethod
    Dim NewFile As String
    Dim QueryResults() As String
    Dim SaveAs As String
    Dim Re1 As Long
    Dim Re2 As Long
    Dim DialableFaxNum As String
    Dim FaxCoverSheetDataFile As Integer
    Dim CoverSheetDocPath As String
    Dim FrmMethod As New frmLtrDeliveryMeth
    Dim AN As String
    FrmMethod.SetDefault DeployMethod, SSN 'set defaults
    'get delivery method information if needed
    If DeployMethod <> dmLetter Then
        'if anything other than letter is the default then get needed information
        FrmMethod.Show
        ProcessingMethod = FrmMethod.GetUserChosenMethod()
    Else
        'if letter is default then no other information is needed
        ProcessingMethod = dmLetter
    End If
    If ProcessingMethod = dmEmail Then
        Dim FN As String
        Dim EmailTo As String
        Dim FTD As String
        'update email address if one on LP22 was blank or invalid
        If FrmMethod.OriginalEmailWasBlankOrInvalid() And FrmMethod.DoSystemEmailUpdate() Then
            'update TX1J and LP22, also add comments to systems
            'LP22
            FastPath "LP22C" & SSN
            If Check4Text(1, 62, "PERSON DEMOGRAPHICS") = False Then
                MsgBox "There was an error while trying to access LP22."
            Else
                'source code
                PutText 3, 9, "F"
                PutText 19, 9, FrmMethod.tbEmailTo.Text
                Hit "End"
                PutText 18, 56, "Y", "F6" 'validate and enter date
            End If
            'TX1J
            FastPath "TX3ZCTX1J;" & SSN
            If Check4Text(1, 72, "TXX1K") = False And Check4Text(1, 71, "TXX1R-01") = False Then
                MsgBox "There was an error while trying to access TX1J."
            ElseIf Check4Text(1, 72, "TXX1K") = False Then
                'try and update if borrower exists on COMPASS
                Hit "F2"
                Hit "F10"
                'source code
                PutText 9, 20, "41" 'TODO change to correct source code
                'verified date
                PutText 11, 17, Format(Date, "MMDDYY")
                'verified indicator
                PutText 12, 14, "Y"
                'blank out current email addr
                PutText 14, 10, "", "End"
                PutText 15, 10, "", "End"
                PutText 16, 10, "", "End"
                PutText 17, 10, "", "End"
                PutText 18, 10, "", "End"
                'add new email addr
                PutText 14, 10, FrmMethod.tbEmailTo.Text, "Enter"
                'add activity comments
                AddComments stacBoth, SSN, "MXADD", "04", "TC", UserId, scriptId, "Updated email address through document generation script", FrmMethod.GetBUEmail(), AN
            End If
        End If
        'create barcode fields in data file
        NewFile = AddBarcodeAndStaticCurrentDate(DataFile, AcctNumOrRefIDFieldNm, LetterID, True, , True, , AN)
        On Error GoTo WordErrorEmail
        FTD = Format(Now, "MMDDYYYYhhmmss")
        SP.Common.SaveDocs DocPath, DocName, NewFile, "T:\DOCGENRSCRPT_" & LetterID & "_" & FTD & ".doc"
        If Common.TestMode() Then
            EmailTo = Environ("USERNAME") & "@utahsbr.edu"
        Else
            EmailTo = FrmMethod.tbEmailTo.Text
        End If
        If SP.Common.SendMail(EmailTo, FrmMethod.GetBUEmail(), FrmMethod.tbEmailSubject.Text, FrmMethod.tbEmailMessage.Text, , , "T:\DOCGENRSCRPT_" & LetterID & "_" & FTD & ".doc") = False Then
            MsgBox "An error occurred while trying to email the letter.  Please contact Systems Support"
            End
        End If
        'clean up
        Kill NewFile
        'delete files older that 30 day from local PC
        FN = Dir("T:\DOCGENRSCRPT_*")
        While FN <> ""
            If FileDateTime("T:\" & FN) < DateAdd("D", -30, Now) Then Kill "T:\" & FN
            FN = Dir()
        Wend
        'add comments
        AddComments SystemToAddCommentsTo, SSN, "MLEX1", ContactType, "EM", UserId, scriptId, "e-mailed " & LetterID & " document to " & FrmMethod.tbEmailTo.Text, FrmMethod.GetBUEmail(), AN
        Exit Sub 'exit sub so normal processing doesn't run into error handling
WordErrorEmail:
        'handles word error
        MsgBox "The letter you requested was not generated.  Please re-run the script and re-send the letter.", vbOKOnly + vbCritical, "Letter Wasn't Generated"
        End
    ElseIf ProcessingMethod = dmLetter Then
        'create print record and get id and batch id back
        SQLEx4StoredProcedures "spPRNT_CreateLetterRec", "@LetterID;" & LetterID & ";10;IN,@AcctNum;" & FrmMethod.GetAcctNum() & ";20;IN,@BU;" & FrmMethod.GetOriginalBU() & ";50;IN,@Dom;" & GetStateCode(DataFile, StateCodeFieldNm) & ";2;IN,@NewRecNum;;;OUT,@BarcodeSeqNum;;;OUT", , Re1, Re2
        If Re1 = 0 Or Re2 = 0 Then
            MsgBox "An error occurred while trying to update and retrieve information from the database.  Please contact Systems Support.", vbCritical
            End
        End If
        NewFile = AddBarcodeAndStaticCurrentDate(DataFile, AcctNumOrRefIDFieldNm, LetterID, True, Re2, True, LetterRecip, AN)
        If SP.TestMode() Then
            SaveAs = "X:\PADD\Central Printing\Print\Test\" & LetterID & "_" & CStr(Re1) & ".doc"
        Else
            SaveAs = "X:\PADD\Central Printing\Print\" & LetterID & "_" & CStr(Re1) & ".doc"
        End If
        SP.Common.SaveDocs DocPath, DocName, NewFile, SaveAs
        'clean up
        Kill NewFile
        'add comments
        If DeployMethod <> dmLetter Then AddComments SystemToAddCommentsTo, SSN, "MLEX1", ContactType, "LT", UserId, scriptId, "mailed " & LetterID & " document to legal address", FrmMethod.GetBUEmail(), AN
    ElseIf ProcessingMethod = dmFax Then
        Dim CommentsAddedTo As String
        'strip out formatting if user added some
        DialableFaxNum = Replace(FrmMethod.tbFaxFaxNum.Text, "-", "")
        DialableFaxNum = Replace(DialableFaxNum, ")", "")
        DialableFaxNum = Replace(DialableFaxNum, "(", "")
        DialableFaxNum = Replace(DialableFaxNum, ".", "")
        DialableFaxNum = Replace(DialableFaxNum, " ", "")
        'create string to document which system comments are to be added to
        If SystemToAddCommentsTo = stacBoth Then
            CommentsAddedTo = "BOTH"
        ElseIf SystemToAddCommentsTo = stacCOMPASS Then
            CommentsAddedTo = "COMPASS"
        ElseIf SystemToAddCommentsTo = stacOneLINK Then
            CommentsAddedTo = "ONELINK"
        End If
        'create print record and get id and batch id back
        SQLEx4StoredProcedures "spPRNT_CreateFaxRec", "@FaxNum;" & DialableFaxNum & ";20;IN,@AcctNum;" & FrmMethod.GetAcctNum() & ";20;IN,@BU;" & FrmMethod.GetOriginalBU() & ";50;IN,@LID;" & LetterID & ";10;IN,@CommentsAddedTo;" & CommentsAddedTo & ";10;IN,@NewRecNum;;;OUT", , Re1
        If Re1 = 0 Then
            MsgBox "An error occurred while trying to update and retrieve information from the database.  Please contact Systems Support.", vbCritical
            End
        End If
        NewFile = AddBarcodeAndStaticCurrentDate(DataFile, AcctNumOrRefIDFieldNm, LetterID, True, , True, , AN)
        'create fax cover sheet
        FaxCoverSheetDataFile = FreeFile()
        Open "T:\Generic Fax Coversheet " & LetterID & "dat.txt" For Output As #FaxCoverSheetDataFile
        Write #FaxCoverSheetDataFile, "BusinessUnit", "SendTo", "At", "Fax", "Pages", "Sender", "Phone", "Subject", "Message"
        Write #FaxCoverSheetDataFile, FrmMethod.GetOriginalBU(), FrmMethod.tbFaxTo.Text, FrmMethod.tbFaxAt.Text, FrmMethod.tbFaxFaxNum.Text, CStr(FigureSheetCount(LetterID, True) + 1), FrmMethod.tbFaxSender.Text, FrmMethod.tbFaxPhoneNum.Text, FrmMethod.tbFaxSubject.Text, FrmMethod.tbFaxMessage.Text
        Close #FaxCoverSheetDataFile
        If SP.TestMode() Then
            SaveAs = "X:\PADD\Central Printing\Fax\Test\" & CStr(Re1) & "_CVR.doc"
            CoverSheetDocPath = "X:\PADD\General\Test\"
        Else
            SaveAs = "X:\PADD\Central Printing\Fax\" & CStr(Re1) & "_CVR.doc"
            CoverSheetDocPath = "X:\PADD\General\"
        End If
        On Error GoTo WordErrorFax
        SP.Common.SaveDocs CoverSheetDocPath, "FAXCVRCP", "T:\Generic Fax Coversheet " & LetterID & "dat.txt", SaveAs
        'delete coversheet data file
        Kill "T:\Generic Fax Coversheet " & LetterID & "dat.txt"
        'create main document
        If SP.TestMode() Then
            SaveAs = "X:\PADD\Central Printing\Fax\Test\" & CStr(Re1) & "_FAX.doc"
        Else
            SaveAs = "X:\PADD\Central Printing\Fax\" & CStr(Re1) & "_FAX.doc"
        End If
        SP.Common.SaveDocs DocPath, DocName, NewFile, SaveAs
        'clean up
        Kill NewFile
        'add comments
        AddComments SystemToAddCommentsTo, SSN, "MLEX1", ContactType, "LT", UserId, scriptId, "faxed " & LetterID & " document to " & FrmMethod.tbFaxFaxNum.Text & " at " & FrmMethod.tbFaxAt.Text, FrmMethod.GetBUEmail(), AN
        Exit Sub 'exit sub so normal processing doesn't run into error handling
WordErrorFax:
        'handles word error
        MsgBox "The letter you requested was not generated.  Please re-run the script and re-send the letter.", vbOKOnly + vbCritical, "Letter Wasn't Generated"
        End
    End If
End Sub

Private Function GetStateCode(DataFile As String, StateCodeFieldName As String) As String
    Dim Handle As Integer
    Dim Rec As String
    Dim SplitRec() As String
    Dim i As Integer
    Dim II As Integer
    Handle = FreeFile()
    Open DataFile For Input As #Handle
    Line Input #Handle, Rec
    Rec = Replace(Rec, """", "")
    SplitRec = Split(Rec, ",")
    While SplitRec(i) <> StateCodeFieldName
        i = i + 1
    Wend
    While II <> i
        Input #Handle, GetStateCode
        II = II + 1
    Wend
    Input #Handle, GetStateCode
    'search for statecode field
    Close #Handle
End Function

Private Sub AddComments(SystemToAddCommentsTo As SystemToAddComments, SSN As String, ActionCode As String, ContactType As String, ActivityType As String, UserId As String, scriptId As String, comment As String, BUEmailAddr As String, AN As String)
    'check if comments should be added to OneLINK
    If SystemToAddCommentsTo = stacBoth Or SystemToAddCommentsTo = stacOneLINK Then
        'add comments to OneLINK
        If SP.Common.AddLP50(SSN, ActionCode, scriptId, ActivityType, ContactType, comment) = False Then
            MsgBox "There was an error while trying to add a comment to LP50."
        End If
    End If
    'check if commnets should be added to COMPASS
    If SystemToAddCommentsTo = stacBoth Or SystemToAddCommentsTo = stacCOMPASS Then
        'add comments to COMPASS
        If SP.Common.ATD22AllLoans(SSN, ActionCode, comment, scriptId, UserId) = False Then
            'if comment couldn't be added to TD22 then try and add it to TD37
            If SP.Common.ATD37FirstLoan(SSN, ActionCode, comment, UserId, scriptId) = False Then
                MsgBox "There was an error while trying to add a comment to COMPASS (TD22 and TD37).  An error email has been sent to your business unit's email address and to Systems Support."
                Common.SendMail BUEmailAddr & "," & Common.BSYSRecips("DocumentGenerationCommentErr"), "Document Generation Script", "Document Generation Script Error Report", "Acct Num:" & AN & " Action Code/ARC that failed to add: " & ActionCode & vbCrLf & vbCrLf & "The script attempted to add this comment to all loans on TD22 and the first loan listed on TD37 and failed to add."
            End If
        End If
    End If
End Sub

Public Function AddBarcodeAndStaticCurrentDate(FileToProc As String, AcctNumOrRefIDFieldNm As String, ByVal LetterID As String, Optional NewFile As Boolean = False, Optional BatchDocNum As Long = 0, Optional AddStaticCurrentDate As Boolean = True, Optional LetterRecip As LetterRecipient = lrBorrower, Optional ReturnOfAcctNum As String = "") As String
    Dim HeaderRowAddition As String
    Dim OriginalHeaderRow As String
    Dim OHRFields() As String
    Dim AcctNumIndex As Integer
    Dim PaperSheetNumber As Integer
    Dim PaperSheetI As Integer
    Dim LineI As Long
    Dim Fields() As String
    Dim FieldI As Integer
    Dim NumberOfFields As Integer
    Dim NewRowData As String
    Dim RMBC() As String
    Dim SMBC() As String
    Dim OldFileHandle As Integer
    Dim NewFileHandle As Integer
    Dim NewAcctNum As String
    'figure paper sheet count
    PaperSheetNumber = FigureSheetCount(LetterID)
    'buffer letter id with spaces
    While Len(LetterID) <> 10
        LetterID = " " & LetterID
    Wend
    OldFileHandle = FreeFile()
    Open FileToProc For Input As #OldFileHandle
    NewFileHandle = FreeFile()
    Open "T:\Add Return Mail Barcode Temp " & LetterID & ".txt" For Output As #NewFileHandle
    'get header row
    Line Input #OldFileHandle, OriginalHeaderRow
    If LetterRecip <> lrOther Then
        'set up header row addition
        If PaperSheetNumber = 1 Then
            HeaderRowAddition = "BC1,BC2,BC3,BC4,BC5,BC6," & _
                                "SMBC1,SMBC2,SMBC3,SMBC4,"
        ElseIf PaperSheetNumber = 2 Then
            HeaderRowAddition = "BC1,BC2,BC3,BC4,BC5,BC6," & _
                                "SMBC1,SMBC2,SMBC3,SMBC4," & _
                                "SMBC_Pg2_Ln1,SMBC_Pg2_Ln2,SMBC_Pg2_Ln3,SMBC_Pg2_Ln4,"
        ElseIf PaperSheetNumber = 3 Then
            HeaderRowAddition = "BC1,BC2,BC3,BC4,BC5,BC6," & _
                                "SMBC1,SMBC2,SMBC3,SMBC4," & _
                                "SMBC_Pg2_Ln1,SMBC_Pg2_Ln2,SMBC_Pg2_Ln3,SMBC_Pg2_Ln4," & _
                                "SMBC_Pg3_Ln1,SMBC_Pg3_Ln2,SMBC_Pg3_Ln3,SMBC_Pg3_Ln4,"
        ElseIf PaperSheetNumber = 4 Then
            HeaderRowAddition = "BC1,BC2,BC3,BC4,BC5,BC6," & _
                                "SMBC1,SMBC2,SMBC3,SMBC4," & _
                                "SMBC_Pg2_Ln1,SMBC_Pg2_Ln2,SMBC_Pg2_Ln3,SMBC_Pg2_Ln4," & _
                                "SMBC_Pg3_Ln1,SMBC_Pg3_Ln2,SMBC_Pg3_Ln3,SMBC_Pg3_Ln4," & _
                                "SMBC_Pg4_Ln1,SMBC_Pg4_Ln2,SMBC_Pg4_Ln3,SMBC_Pg4_Ln4,"
        Else
            MsgBox "The paper sheet count for the letter id that the script is using isn't populated.  Please contact a member of Systems Support", vbCritical, "Error"
            End
        End If
    Else
        'set up header row addition
        If PaperSheetNumber = 1 Then
            HeaderRowAddition = "SMBC1,SMBC2,SMBC3,SMBC4,"
        ElseIf PaperSheetNumber = 2 Then
            HeaderRowAddition = "SMBC1,SMBC2,SMBC3,SMBC4," & _
                                "SMBC_Pg2_Ln1,SMBC_Pg2_Ln2,SMBC_Pg2_Ln3,SMBC_Pg2_Ln4,"
        ElseIf PaperSheetNumber = 3 Then
            HeaderRowAddition = "SMBC1,SMBC2,SMBC3,SMBC4," & _
                                "SMBC_Pg2_Ln1,SMBC_Pg2_Ln2,SMBC_Pg2_Ln3,SMBC_Pg2_Ln4," & _
                                "SMBC_Pg3_Ln1,SMBC_Pg3_Ln2,SMBC_Pg3_Ln3,SMBC_Pg3_Ln4,"
        ElseIf PaperSheetNumber = 4 Then
            HeaderRowAddition = "SMBC1,SMBC2,SMBC3,SMBC4," & _
                                "SMBC_Pg2_Ln1,SMBC_Pg2_Ln2,SMBC_Pg2_Ln3,SMBC_Pg2_Ln4," & _
                                "SMBC_Pg3_Ln1,SMBC_Pg3_Ln2,SMBC_Pg3_Ln3,SMBC_Pg3_Ln4," & _
                                "SMBC_Pg4_Ln1,SMBC_Pg4_Ln2,SMBC_Pg4_Ln3,SMBC_Pg4_Ln4,"
        Else
            MsgBox "The paper sheet count for the letter id that the script is using isn't populated.  Please contact a member of Systems Support", vbCritical, "Error"
            End
        End If
    End If
    'add static current date if desired
    If AddStaticCurrentDate Then
        HeaderRowAddition = HeaderRowAddition & "StaticCurrentDate,"
    End If
    'remove quotes from header row adn split out in to array
    OriginalHeaderRow = Replace(OriginalHeaderRow, """", "")
    OHRFields = Split(OriginalHeaderRow, ",")
    'get total number of fields in record
    NumberOfFields = UBound(OHRFields)
    If LetterRecip <> lrOther Then
        'find account number index
        While UCase(OHRFields(AcctNumIndex)) <> UCase(AcctNumOrRefIDFieldNm)
            AcctNumIndex = AcctNumIndex + 1
        Wend
    End If
    'add new header row to revised file
    Print #NewFileHandle, HeaderRowAddition & OriginalHeaderRow
    'process all other records
    While Not EOF(OldFileHandle)
        If BatchDocNum <> 0 Then
            LineI = BatchDocNum
        Else
            LineI = LineI + 1
        End If
        'blank array
        ReDim Fields(NumberOfFields)
        'read record fields in one at a time
        For FieldI = 0 To NumberOfFields
            Input #OldFileHandle, Fields(FieldI)
        Next
        NewAcctNum = Replace(Fields(AcctNumIndex), " ", "")
        ReturnOfAcctNum = NewAcctNum
        If LetterRecip = lrReference Then NewAcctNum = NewAcctNum & " "
        If LetterRecip <> lrOther Then
            'return mail barcode
            RMBC = Split(EncNDM(NewAcctNum & LetterID & Format(Date, "MMDDYYYY")), vbCrLf)
            'add return mail data to string
            NewRowData = """" & RMBC(0) & """,""" & RMBC(1) & """,""" & RMBC(2) & """,""" & RMBC(3) & """,""" & RMBC(4) & """,""" & RMBC(5) & """"
        End If
        'state mail barcode
        PaperSheetI = 0
        While PaperSheetI <> PaperSheetNumber
            If PaperSheetI = 0 Then
                SMBC = Split(IDAutomation2DBarcode.EncNDM("1" & CStr(PaperSheetNumber) & "0" & CStr(PaperSheetI + 1) & Format(LineI, "00000#")), vbCrLf)
            Else
                SMBC = Split(IDAutomation2DBarcode.EncNDM("0" & CStr(PaperSheetNumber) & "0" & CStr(PaperSheetI + 1) & Format(LineI, "00000#")), vbCrLf)
            End If
            If LetterRecip <> lrOther Then
                NewRowData = NewRowData & ",""" & SMBC(0) & """,""" & SMBC(1) & """,""" & SMBC(2) & """,""" & SMBC(3) & """"
            Else
                NewRowData = NewRowData & """" & SMBC(0) & """,""" & SMBC(1) & """,""" & SMBC(2) & """,""" & SMBC(3) & """"
            End If
            PaperSheetI = PaperSheetI + 1
        Wend
        'add static current date if desired
        If AddStaticCurrentDate Then
            NewRowData = NewRowData & ",""" & Format(Date, "mmmm dd, yyyy") & """"
        End If
        'write the rest of the data out to row then write row out to file
        For FieldI = 0 To NumberOfFields
            NewRowData = NewRowData & ",""" & Fields(FieldI) & """"
        Next
        Print #NewFileHandle, NewRowData
    Wend
    Close #NewFileHandle
    Close #OldFileHandle
    If NewFile Then
        AddBarcodeAndStaticCurrentDate = "T:\Add Return Mail Barcode Temp " & LetterID & ".txt"
    Else
        'delete original data file
        Kill FileToProc
        'copy new data file to original file location
        Name "T:\Add Return Mail Barcode Temp " & LetterID & ".txt" As FileToProc
        AddBarcodeAndStaticCurrentDate = FileToProc
    End If
End Function


Private Function FigureSheetCount(LetterID As String, Optional DoNotCalc As Boolean = False) As Integer
    Dim Temp() As String
    'query DB to get paper sheet count
    Temp = SP.SQLEx("SELECT Pages, Duplex FROM LTDB_DAT_CentralPrintingDocData WHERE ID = '" & LetterID & "'")
    If UBound(Temp, 2) = 0 Then
        MsgBox "The letter id that the script is using doesn't appear to exist.  Please contact a member of Systems Support", vbCritical, "Error"
        End
    ElseIf Temp(0, 0) = "0" Then
        MsgBox "The paper sheet count for the letter id that the script is using isn't populated.  Please contact a member of Systems Support", vbCritical, "Error"
        End
    Else
        If DoNotCalc = False Then
            'the sub should always calculate page number based off page number and duplex for printing
            If CInt(Temp(0, 0)) = 1 Then 'if page number equals 1 then the sheet count is 1
                FigureSheetCount = 1
            ElseIf Temp(1, 0) = False Then 'if not duplex then sheet count equals page count
                FigureSheetCount = CInt(Temp(0, 0))
            Else 'if marked to do duplex then figure out how many pages it is going to take
                FigureSheetCount = CInt(Temp(0, 0)) \ 2
                If (CInt(Temp(0, 0)) Mod 2) > 0 Then
                    FigureSheetCount = FigureSheetCount + 1
                End If
            End If
        Else
            'the sub should never calculate page number for faxing
            FigureSheetCount = CInt(Temp(0, 0))
        End If
    End If
End Function

'allows stored procedures to return output parameters
Private Function SQLEx4StoredProcedures(SPName As String, Params As String, Optional db As String = "BSYS", Optional Output1 As Long = 0, Optional Output2 As Long = 0)
    Dim cmd As ADODB.Command
    Dim prm As ADODB.Parameter
    Dim rs As ADODB.Recordset
    Dim rsResults() As String
    Dim dbServer As String
    Dim dbConnection As String
    Dim i As Integer
    Dim II As Integer
    Dim ParamSplit() As String
    Dim cn As ADODB.Connection
    Dim l As Long
    Set cn = New ADODB.Connection
    'set server and connection string
    If SP.Common.TestMode Then dbServer = "BART\BART" Else dbServer = "NOCHOUSE"
    dbConnection = "Driver={ODBC Driver 11 for SQL Server};Server=" & dbServer & ";Database=" & db & ";Trusted_Connection=yes;"
    cn.Open dbConnection
    Set cmd = New ADODB.Command
    Set cmd.ActiveConnection = cn
    cmd.CommandTimeout = 0
    cmd.CommandText = SPName
    cmd.CommandType = adCmdStoredProc
    
    ParamSplit = Split(Params, ",")
    
    While UBound(ParamSplit) >= II
        If Split(Replace(ParamSplit(II), "'", ""), ";")(3) = "IN" Then
            Set prm = cmd.CreateParameter(Split(Replace(ParamSplit(II), "'", ""), ";")(0), adVarChar, adParamInput, CInt(Split(Replace(ParamSplit(II), "'", ""), ";")(2)), Split(Replace(ParamSplit(II), "'", ""), ";")(1))
        Else
            Set prm = cmd.CreateParameter(Split(Replace(ParamSplit(II), "'", ""), ";")(0), adBigInt, adParamOutput)
        End If
        cmd.Parameters.Append prm
        II = II + 1
    Wend
    
    cmd.Execute
    
    II = 0
    While cmd.Parameters(II).Direction <> adParamOutput
        II = II + 1
    Wend
    Output1 = cmd.Parameters.Item(II).Value 'return output 1 variable
    If cmd.Parameters.count > (II + 1) Then Output2 = cmd.Parameters.Item(II + 1).Value  'return output 2 variable


End Function
