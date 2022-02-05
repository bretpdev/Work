Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Word
Imports DMATRIXLib
Imports RFCOMAPILib
Imports Q.DataAccess
Imports Q.ReflectionInterface.Key
Imports OSSMTP
Imports System.Windows.Forms

Public Class DocumentHandling

#Region "Enums"

    Public Enum DocProcessingPath
        Save
        Print
    End Enum

    Public Enum DestinationOrPageCount
        DocServices = -2
        BusinessUnit = -1
        Page1 = 1
        Page2 = 2
        Page3 = 3
        Page4 = 4
    End Enum

    Public Enum PrintOptions
        None = 0
        DisplayOnScreen = 1
        SendToPrinter = 2
    End Enum

    Public Enum Barcode2DLetterRecipient
        lrBorrower = 0
        lrReference = 1
        lrOther = 2
    End Enum

    Public Enum CentralizedPrintingDeploymentMethod
        dmUserPrompt = 0
        dmFax = 1
        dmEmail = 2
        dmLetter = 3
    End Enum

    Public Enum CentralizedPrintingSystemToAddComments
        stacCOMPASS = 0
        stacOneLINK = 1
        stacBoth = 2
    End Enum

    <Flags()> _
    Public Enum CostCenterOption
        None = 0
        ToPrinter = 1
        AddBarcode = 2
    End Enum

#End Region

#Region "General Document Handling Method Calls"

    ''' <summary>
    ''' Merges the merge data source text file with the specified document as saves it to specifed path and file
    ''' </summary>
    ''' <param name="DocPath">The directory path for the letter file.</param>
    ''' <param name="Doc">The file name for the letter.</param>
    ''' <param name="Dat">The data file for the merge.</param>
    ''' <param name="SaveAs">Name of to be saved file.</param>
    ''' <remarks></remarks>
    Public Shared Sub SaveDocs(ByVal DocPath As String, ByVal Doc As String, ByVal Dat As String, ByVal SaveAs As String)
        PrintAndSaveDocs(DocProcessingPath.Save, DocPath, Doc, Dat, SaveAs, PrintOptions.None)
    End Sub

    Public Shared Sub SaveDocs(ByVal testMode As Boolean, ByVal letterId As String, ByVal dataFile As String, ByVal saveAs As String, ByVal efs As EnterpriseFileSystem, ByVal region As ScriptSessionBase.Region)
        Dim dp As DocumentPathAndName = GetDocumentPathAndFileName(testMode, letterId, region, efs)
        PrintAndSaveDocs(DocProcessingPath.Save, dp.CalculatedPath, dp.CalculatedFileName, dataFile, saveAs, PrintOptions.None)
    End Sub

    ''' <summary>
    ''' Merges the merge data source text file with the specified document and prints it.
    ''' </summary>
    ''' <param name="DocPath">The directory path for the letter file.</param>
    ''' <param name="Doc">The file name for the letter.</param>
    ''' <param name="Dat">The data file for the merge.</param>
    ''' <param name="ToPrinter">Determines whether to display on screen or print.</param>
    ''' <remarks></remarks>
    Public Shared Sub PrintDocs(ByVal DocPath As String, ByVal Doc As String, ByVal Dat As String, ByVal ToPrinter As PrintOptions)
        PrintAndSaveDocs(DocProcessingPath.Print, DocPath, Doc, Dat, , ToPrinter)
    End Sub

    ''' <summary>
    ''' Merges the merge data source text file with the specified document and prints it.
    ''' </summary>
    ''' <param name="DocPath">The directory path for the letter file.</param>
    ''' <param name="Doc">The file name for the letter.</param>
    ''' <param name="Dat">The data file for the merge.</param>
    ''' <remarks></remarks>
    Public Shared Sub PrintDocs(ByVal DocPath As String, ByVal Doc As String, ByVal Dat As String)
        PrintAndSaveDocs(DocProcessingPath.Print, DocPath, Doc, Dat, , PrintOptions.None)
    End Sub

    ''' <summary>
    ''' Merges the merge data source text file with the specified document and prints it.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="docId">The doc ID (from Letter Tracking) of the document to print.</param>
    ''' <param name="dataFile">The data file for the merge.</param>
    Public Shared Sub PrintDocs(ByVal testMode As Boolean, ByVal docId As String, ByVal dataFile As String)
        Dim pathAndName As DocumentPathAndName = GetDocumentPathAndFileName(testMode, docId)
        PrintDocs(pathAndName.CalculatedPath, pathAndName.CalculatedFileName, dataFile)
    End Sub

    'is used by the savedocs and printdocs accessor functions to print and save Word documents
    Private Shared Sub PrintAndSaveDocs(ByVal ProcPath As DocProcessingPath, ByVal DocPath As String, ByVal Doc As String, ByVal Dat As String, Optional ByVal SaveAs As String = "", Optional ByVal ToPrinter As PrintOptions = PrintOptions.None)
        'Ensure that the Doc name ends with .doc
        If Not Doc.EndsWith(".doc") Then
            Doc += ".doc"
        End If
        'set up Word object
        Dim Word As Word.Application
        PleaseWait.ShowForm()
        Word = CreateObject("Word.Application")
        Word.Visible = False
        'open form doc
        Word.Documents.Open(FileName:=DocPath & Doc, ConfirmConversions:=False, _
         ReadOnly:=True, AddToRecentFiles:=False, PasswordDocument:="", _
         PasswordTemplate:="", Revert:=False, WritePasswordDocument:="", _
         WritePasswordTemplate:="", Format:=WdOpenFormat.wdOpenFormatAuto)
        'merge files
        With Word.ActiveDocument.MailMerge
            .OpenDataSource(Name:=Dat, _
             ConfirmConversions:=False, ReadOnly:=True, LinkToSource:=True, _
             AddToRecentFiles:=False, PasswordDocument:="", PasswordTemplate:="", _
             WritePasswordDocument:="", WritePasswordTemplate:="", Revert:=False, _
             Format:=WdOpenFormat.wdOpenFormatAuto, Connection:="", SQLStatement:="", SQLStatement1 _
             :="", SubType:=WdMergeSubType.wdMergeSubTypeOther)
            If ProcPath = DocProcessingPath.Save Then
                .Destination = WdMailMergeDestination.wdSendToNewDocument
            Else
                If ToPrinter = PrintOptions.SendToPrinter Then
                    .Destination = WdMailMergeDestination.wdSendToPrinter
                Else
                    .Destination = WdMailMergeDestination.wdSendToNewDocument
                End If
            End If

            .MailAsAttachment = False
            .MailAddressFieldName = ""
            .MailSubject = ""
            .SuppressBlankLines = True
            With .DataSource
                .FirstRecord = WdMailMergeDefaultRecord.wdDefaultFirstRecord
                .LastRecord = WdMailMergeDefaultRecord.wdDefaultLastRecord
            End With
            .Execute(Pause:=True)
        End With
        'close form file
        Word.Documents(Doc).Close(SaveChanges:=WdSaveOptions.wdDoNotSaveChanges)
        'handle new Word document
        If ProcPath = DocProcessingPath.Save Then
            'save document
            Word.ActiveDocument.SaveAs(SaveAs, WdSaveFormat.wdFormatDocument)
            Word.Application.Quit(SaveChanges:=WdSaveOptions.wdDoNotSaveChanges)
        Else
            'display document or quit Word
            If ToPrinter = PrintOptions.SendToPrinter Then
                Word.Application.Quit(SaveChanges:=WdSaveOptions.wdDoNotSaveChanges)
            Else
                Word.Visible = True
            End If
        End If
        PleaseWait.HideForm()
    End Sub
    ''' <summary>
    ''' Adds copies of documents to the imaging system
    ''' </summary>
    ''' <param name="data"></param>
    ''' <remarks>Designed to make overload obsolite.  If you need to add another parameter just add it to the imaging class in ObjectsSentForParams </remarks>
    Public Shared Sub ImageDocs(ByVal data As ObjectsSentForParams.Imaging)
        Dim docPathAndName As DocumentPathAndName = GetDocumentPathAndFileName(data.TestMode, data.LetterTrackingDoc, data.Region, data.Efs)

        If data.ProcessOfT Then
            File.Copy(String.Format("{0}{1}", docPathAndName.CalculatedPath, docPathAndName.CalculatedFileName), String.Format("{0}{1}", data.Efs.TempFolder, docPathAndName.CalculatedFileName))
            docPathAndName.CalculatedPath = data.Efs.TempFolder
        End If

        Dim counter As Integer = 0 'counter to uniquely identify files
        Dim recoveryValue As Integer = 0
        Dim recoveryLog As String = String.Format("{0}imagingLog_{1}", data.Efs.LogsFolder, data.ScriptId)
        If File.Exists(recoveryLog) Then
            recoveryValue = Integer.Parse(File.ReadAllText(recoveryLog))
        End If

        Dim dataFileBarcodes As String = AddBarcodeAndStaticCurrentDateForBatchProcessing(data.DataFile, data.AcctNumFieldIndex, data.LetterTrackingDoc, True, Barcode2DLetterRecipient.lrBorrower, data.TestMode)
        Using fileReader As New StreamReader(dataFileBarcodes)
            Dim headerRow As String = fileReader.ReadLine() 'read in header row
            Dim acctNumIndex As Integer = headerRow.SplitAgnosticOfQuotes(",").IndexOf(data.AcctNumFieldIndex) 'get the acct number index
            While counter < recoveryValue
                fileReader.ReadLine()
                counter += 1
            End While
            While Not fileReader.EndOfStream

                Dim fileLine As String = fileReader.ReadLine() 'reading in borrower data
                Dim borrowerData As String = String.Format("{0}imagingData_{1}{2}.txt", data.Efs.TempFolder, data.ScriptId, counter)
                Using fileWriter As New StreamWriter(borrowerData) 'create file for SaveDocs
                    fileWriter.WriteLine(headerRow)
                    fileWriter.WriteLine(fileLine)
                End Using
                Dim borrowerDoc As String = String.Format("{0}imagingDoc_{1}{2}.doc", data.Efs.TempFolder, data.ScriptId, counter)
                SaveDocs(docPathAndName.CalculatedPath, docPathAndName.CalculatedFileName, borrowerData, borrowerDoc) 'save and merge
                Dim acctNum As String = fileLine.SplitAgnosticOfQuotes(",")(acctNumIndex)
                Dim selectStr As String = "EXEC spGetSSNFromAcctNumber {0}" 'get the SSN from the Acct number
                Dim da As New DataAccess(data.TestMode, ScriptSessionBase.Region.None)
                Dim ssn As String = da.GetSsnFromAcctNum(acctNum.Replace(" ", ""), data.Region)
                ImageFile(data.Efs, borrowerDoc, data.ImagingDocId, ssn)    ' send file to correct imaging directory
                File.Delete(borrowerData)
                File.Delete(borrowerDoc)
                counter += 1

                File.WriteAllText(recoveryLog, counter.ToString())
            End While

            If data.ProcessOfT Then
                File.Delete(String.Format("{0}{1}", data.Efs.TempFolder, docPathAndName.CalculatedFileName))
            End If

            File.Delete(recoveryLog)
        End Using
    End Sub


    ''' <summary>
    ''' Adds copies of documents to the imaging system
    ''' </summary>
    ''' <param name="testMode">Testmode indicator</param>
    ''' <param name="efs">efs object</param>
    ''' <param name="scriptId">ScriptId</param>
    ''' <param name="acctNumFieldName">AccountNumberfield name</param>
    ''' <param name="imagingDocId">DocId to use</param>
    ''' <param name="letterTrackingDoc">The File name for the document</param>
    ''' <param name="dataFile">Datafile for the merge</param>
    ''' <param name="region">ScriptSessionBase region</param>
    ''' <remarks></remarks>
    Public Shared Sub ImageDocs(ByVal testMode As Boolean, ByVal efs As EnterpriseFileSystem, ByVal scriptId As String, ByVal acctNumFieldName As String, ByVal imagingDocId As String, ByVal letterTrackingDoc As String, ByVal dataFile As String, ByVal region As ScriptSessionBase.Region)
        Dim docPathAndName As DocumentPathAndName = GetDocumentPathAndFileName(testMode, letterTrackingDoc, region, efs)
        Dim counter As Integer = 0 'counter to uniquely identify files
        Dim recoveryValue As Integer = 0
        Dim recoveryLog As String = String.Format("{0}imagingLog_{1}", efs.LogsFolder, scriptId)
        If File.Exists(recoveryLog) Then
            recoveryValue = Integer.Parse(File.ReadAllText(recoveryLog))
        End If

        Dim dataFileBarcodes As String = AddBarcodeAndStaticCurrentDateForBatchProcessing(dataFile, acctNumFieldName, letterTrackingDoc, True, Barcode2DLetterRecipient.lrBorrower, testMode)
        Using fileReader As New StreamReader(dataFileBarcodes)
            Dim headerRow As String = fileReader.ReadLine() 'read in header row
            Dim acctNumIndex As Integer = headerRow.SplitAgnosticOfQuotes(",").IndexOf(acctNumFieldName) 'get the acct number index
            While counter < recoveryValue
                fileReader.ReadLine()
                counter += 1
            End While
            While Not fileReader.EndOfStream

                Dim fileLine As String = fileReader.ReadLine() 'reading in borrower data
                Dim borrowerData As String = String.Format("{0}imagingData_{1}{2}.txt", efs.TempFolder, scriptId, counter)
                Using fileWriter As New StreamWriter(borrowerData) 'create file for SaveDocs
                    fileWriter.WriteLine(headerRow)
                    fileWriter.WriteLine(fileLine)
                End Using
                Dim borrowerDoc As String = String.Format("{0}imagingDoc_{1}{2}.doc", efs.TempFolder, scriptId, counter)
                SaveDocs(docPathAndName.CalculatedPath, docPathAndName.CalculatedFileName, borrowerData, borrowerDoc) 'save and merge
                Dim acctNum As String = fileLine.SplitAgnosticOfQuotes(",")(acctNumIndex)
                Dim selectStr As String = "EXEC spGetSSNFromAcctNumber {0}" 'get the SSN from the Acct number
                Dim da As New DataAccess(testMode, ScriptSessionBase.Region.None)
                Dim ssn As String = da.GetSsnFromAcctNum(acctNum, region)
                ImageFile(efs, borrowerDoc, imagingDocId, ssn)  ' send file to correct imaging directory
                File.Delete(borrowerData)
                File.Delete(borrowerDoc)
                counter += 1

                File.WriteAllText(recoveryLog, counter.ToString())
            End While
            File.Delete(recoveryLog)
        End Using
    End Sub

    <Obsolete("Use overload ImageDocs that takes a ScriptSessionBase Region")> _
    Public Shared Sub ImageDocs(ByVal testMode As Boolean, ByVal efs As EnterpriseFileSystem, ByVal scriptId As String, ByVal acctNumFieldName As String, ByVal imagingDocId As String, ByVal letterTrackingDoc As String, ByVal dataFile As String)

        Dim docPathAndName As DocumentPathAndName = GetDocumentPathAndFileName(testMode, letterTrackingDoc, ScriptSessionBase.Region.CornerStone, efs)
        Dim counter As Integer = 0 'counter to uniquely identify files
        Dim recoveryValue As Integer = 0
        Dim recoveryLog As String = String.Format("{0}imagingLog_{1}", efs.LogsFolder, scriptId)
        If File.Exists(recoveryLog) Then
            recoveryValue = Integer.Parse(File.ReadAllText(recoveryLog))
        End If

        Dim dataFileBarcodes As String = AddBarcodeAndStaticCurrentDateForBatchProcessing(dataFile, acctNumFieldName, letterTrackingDoc, True, Barcode2DLetterRecipient.lrBorrower, testMode)
        Using fileReader As New StreamReader(dataFileBarcodes)
            Dim headerRow As String = fileReader.ReadLine() 'read in header row
            Dim acctNumIndex As Integer = headerRow.SplitAgnosticOfQuotes(",").IndexOf(acctNumFieldName) 'get the acct number index
            While counter < recoveryValue
                fileReader.ReadLine()
                counter += 1
            End While
            While Not fileReader.EndOfStream

                Dim fileLine As String = fileReader.ReadLine() 'reading in borrower data
                Dim borrowerData As String = String.Format("{0}imagingData_{1}{2}.txt", efs.TempFolder, scriptId, counter)
                Using fileWriter As New StreamWriter(borrowerData) 'create file for SaveDocs
                    fileWriter.WriteLine(headerRow)
                    fileWriter.WriteLine(fileLine)
                End Using
                Dim borrowerDoc As String = String.Format("{0}imagingDoc_{1}{2}.doc", efs.TempFolder, scriptId, counter)
                SaveDocs(docPathAndName.CalculatedPath, docPathAndName.CalculatedFileName, borrowerData, borrowerDoc) 'save and merge
                Dim acctNum As String = fileLine.SplitAgnosticOfQuotes(",")(acctNumIndex)
                Dim selectStr As String = "EXEC spGetSSNFromAcctNumber {0}" 'get the SSN from the Acct number
                Dim da As New DataAccess(testMode, ScriptSessionBase.Region.None)
                Dim ssn As String = da.GetSsnFromAcctNum(acctNum)
                ImageFile(efs, borrowerDoc, imagingDocId, ssn)  ' send file to correct imaging directory
                File.Delete(borrowerData)
                File.Delete(borrowerDoc)
                counter += 1

                File.WriteAllText(recoveryLog, counter.ToString())
            End While
            File.Delete(recoveryLog)
        End Using
    End Sub

    ''' <summary>
    ''' Add copies of documents to the imaging system
    ''' </summary>
    ''' <param name="ssnFieldNm">Name of SSN field.</param>
    ''' <param name="docId">The document ID to log in under when placing in the imaging system.</param>
    ''' <param name="docPath">The directory path for the document file.</param>
    ''' <param name="doc">The file name for the document.</param>
    ''' <param name="dataFile">The data file for the merge.</param>
    ''' <param name="testMode">Is the application in test mode or not.</param>
    ''' <remarks></remarks>
    Public Shared Function ImageDocs(ByVal ssnFieldNm As String, ByVal docId As String, ByVal docPath As String, ByVal doc As String, ByVal dataFile As String, ByVal testMode As Boolean, ByVal doNotUseUntilSR3728IsPromoted As Boolean) As Long
        Using imgDoc As New ImageDocumentProcessor(ssnFieldNm, docId, docPath, doc, dataFile, testMode)
            imgDoc.ProcessEntireFile()
            Return imgDoc.ImageCount
        End Using
    End Function

    ''' <summary>
    ''' Add copies of documents to the imaging system
    ''' </summary>
    ''' <param name="indexOfSsn">Index of SSN field.</param>
    ''' <param name="docId">The document ID to log in under when placing in the imaging system.</param>
    ''' <param name="docPath">The directory path for the document file.</param>
    ''' <param name="doc">The file name for the document.</param>
    ''' <param name="dataFile">The data file for the merge.</param>
    ''' <param name="testMode">Is the application in test mode or not.</param>
    ''' <remarks></remarks>
    Public Shared Function ImageDocs(ByVal indexOfSsn As Integer, ByVal docId As String, ByVal docPath As String, ByVal doc As String, ByVal dataFile As String, ByVal testMode As Boolean, ByVal doNotUseUntilSR3728IsPromoted As Boolean) As Long
        Using imgDoc As New ImageDocumentProcessor(indexOfSsn, docId, docPath, doc, dataFile, testMode)
            imgDoc.ProcessEntireFile()
            Return imgDoc.ImageCount
        End Using
    End Function



    ''' <summary>
    ''' Add copies of documents to the imaging system
    ''' </summary>
    ''' <param name="ssnFieldNm">Name of SSN field.</param>
    ''' <param name="docId">The document ID to log in under when placing in the imaging system.</param>
    ''' <param name="docPath">The directory path for the document file.</param>
    ''' <param name="doc">The file name for the document.</param>
    ''' <param name="dataFile">The data file for the merge.</param>
    ''' <param name="testMode">Is the application in test mode or not.</param>
    ''' <remarks></remarks>
    Public Shared Sub ImageDocs(ByVal ssnFieldNm As String, ByVal docId As String, ByVal docPath As String, ByVal doc As String, ByVal dataFile As String, ByVal testMode As Boolean)
        Dim fileHandle As Integer = FreeFile()
        Dim fieldName As String = String.Empty
        Dim fieldCount As Integer = 0

        FileOpen(fileHandle, dataFile, OpenMode.Input, OpenAccess.Read)
        Input(fileHandle, fieldName)
        While ssnFieldNm <> fieldName
            Input(fileHandle, fieldName)
            fieldCount += 1
        End While
        FileClose(fileHandle)

        ImageDocs(fieldCount, docId, docPath, doc, dataFile, testMode)
    End Sub

    ''' <summary>
    ''' Add copies of documents to the imaging system
    ''' </summary>
    ''' <param name="indexOfSsn">Index of SSN field.</param>
    ''' <param name="docId">The document ID to log in under when placing in the imaging system.</param>
    ''' <param name="docPath">The directory path for the document file.</param>
    ''' <param name="doc">The file name for the document.</param>
    ''' <param name="dataFile">The data file for the merge.</param>
    ''' <param name="testMode">Is the application in test mode or not.</param>
    ''' <remarks></remarks>
    Public Shared Sub ImageDocs(ByVal indexOfSsn As Integer, ByVal docId As String, ByVal docPath As String, ByVal doc As String, ByVal dataFile As String, ByVal testMode As Boolean)
        'set imaging and printing parameters
        Dim archLib As String = "CR"
        If testMode Then
            archLib = "Test"
        End If

        FileOpen(1, dataFile, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
        'Read in header row
        LineInput(1)


        'set up Word object
        Dim Word As New Word.Application()
        'Word.Visible = False
        'open form doc
        Word.Documents.Open(FileName:=docPath & doc & ".doc", ConfirmConversions:=False, _
         ReadOnly:=False, AddToRecentFiles:=False, PasswordDocument:="", _
         PasswordTemplate:="", Revert:=False, WritePasswordDocument:="", _
         WritePasswordTemplate:="", Format:=WdOpenFormat.wdOpenFormatAuto)
        Word.ActiveDocument.MailMerge.OpenDataSource(Name:=dataFile, _
         ConfirmConversions:=False, ReadOnly:=False, LinkToSource:=True, _
         AddToRecentFiles:=False, PasswordDocument:="", PasswordTemplate:="", _
         WritePasswordDocument:="", WritePasswordTemplate:="", Revert:=False, _
         Format:=WdOpenFormat.wdOpenFormatAuto, Connection:="", SQLStatement:="", SQLStatement1 _
         :="", SubType:=WdMergeSubType.wdMergeSubTypeOther)
        Word.ActiveDocument.SaveAs(FileName:=DataAccessBase.PersonalDataDirectory & doc & ".doc", _
         FileFormat:=WdSaveFormat.wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
         True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
         False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
         SaveAsAOCELetter:=False)
        'merge one file for each record in the merge data file
        Dim currentFileLine As Integer = 1
        While Not EOF(1)

            'read in row
            Dim fileLine As String = LineInput(1)
            Dim fields() As String = fileLine.SplitAgnosticOfQuotes(",").ToArray
            Dim ssn As String = fields(indexOfSsn)

            'merge the data
            With Word.ActiveDocument.MailMerge
                .Destination = WdMailMergeDestination.wdSendToNewDocument
                .MailAsAttachment = False
                .MailAddressFieldName = ""
                .MailSubject = ""
                .SuppressBlankLines = True
                'limit the record being merged to the current record number from the merge data file
                With .DataSource
                    .FirstRecord = currentFileLine
                    .LastRecord = currentFileLine
                End With
                .Execute(Pause:=True)
            End With
            'set time stamp to add to file names so files for the same borrower are not overwritten
            Dim secondsSinceMidnight As Double = (Date.Now.Subtract(Date.Now.Date)).Milliseconds / 1000.0
            Dim timeStamp As String = secondsSinceMidnight.ToString().Replace(".", "").SafeSubstring(0, 7)
            'password protect document
            Word.ActiveDocument.Protect(2, , timeStamp)
            'save the file
            Word.ActiveDocument.SaveAs(FileName:="\\imgprodkofax\\ascent$\UT" & archLib & "Other_imp\" & ssn & "_" & timeStamp & ".doc", _
             FileFormat:=WdSaveFormat.wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
             True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
             False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
             SaveAsAOCELetter:=False)
            'close the file
            Word.ActiveDocument.Close()
            'create the imaging control file for the doc to be imaged
            FileOpen(9, "\\imgprodkofax\ascent$\UT" & archLib & "Other_imp\" & ssn & "_" & timeStamp & ".ctl", OpenMode.Output)
            'write info to control file for correspondence
            PrintLine(9, "~^Folder~" & Date.Now.ToString("MM/dd/yyyy") & " " & Date.Now.TimeOfDay.ToString() & ", Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~" & ssn & "^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~" & docId & "^Attribute~DOC_DATE~STR~" & Date.Now.ToString("MM/dd/yyyy") & "^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~" & Date.Now.ToString("MM/dd/yyyy") & "^Attribute~SCAN_TIME~STR~" & Date.Now.ToString("HH:mm:ss") & "^Attribute~DESCRIPTION~STR~" & Date.Now.ToString("MM/dd/yyyy") & " " & Date.Now.TimeOfDay.ToString())
            PrintLine(9, "DesktopDoc~\\imgprodkofax\ascent$\UTCROther_imp\" & ssn & "_" & timeStamp & ".doc~" & Date.Now.ToString("MM/dd/yyyy") & " " & Date.Now.TimeOfDay.ToString() & ", Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~" & ssn & "^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~" & docId & "^Attribute~DOC_DATE~STR~" & Date.Now.ToString("MM/dd/yyyy") & "^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~" & Date.Now.ToString("MM/dd/yyyy") & "^Attribute~SCAN_TIME~STR~" & Date.Now.ToString("HH:mm:ss"))
            FileClose(9)
            currentFileLine += 1
        End While
        'close Word
        Word.Application.Quit(SaveChanges:=WdSaveOptions.wdDoNotSaveChanges)
        FileClose(1)

        'sometimes file control isn't released, this makes sure it is before continuing
        While True
            Try
                FileOpen(1, dataFile, OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
                FileClose(1)
                Exit Sub
            Catch ex As IOException

            End Try
        End While

    End Sub

    ''' <summary>
    ''' Auto-archives the given file in the imaging system.
    ''' </summary>
    ''' <param name="efs">An EnterpriseFileSystem object initialized with the appropriate region and mode (test/live).</param>
    ''' <param name="filePathAndName">The full path and name of the file to archive.</param>
    ''' <param name="imagingDocId">The document ID to log in under when placing in the imaging system.</param>
    ''' <param name="ssn">The SSN of the borrower to whom this document pertains.</param>
    Public Shared Sub ImageFile(ByVal efs As EnterpriseFileSystem, ByVal filePathAndName As String, ByVal imagingDocId As String, ByVal ssn As String)
        'Make a note of the file extension.
        Dim fileExtension As String = filePathAndName.Substring(filePathAndName.LastIndexOf("."))

        'Copy the file to the auto-archive folder.
        Dim secondsSinceMidnight As Double = (Date.Now.Subtract(Date.Today)).Milliseconds / 1000.0
        Dim timeStamp As String = secondsSinceMidnight.ToString().Replace(".", "").SafeSubstring(0, 7)
        Dim imagingFolder As String = efs.GetPath("Imaging")
        Dim destination As String = String.Format("{0}{1}_{2}{3}", imagingFolder, ssn, timeStamp, fileExtension)
        File.Copy(filePathAndName, destination)

        'Create the control file that tells the imaging system to pick up the file.
        Dim controlFile As String = String.Format("{0}{1}_{2}.ctl", imagingFolder, ssn, timeStamp)
        Using controlWriter As New StreamWriter(controlFile)
            controlWriter.WriteLine(String.Format("~^Folder~{0:MM/dd/yyyy} {1}, Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~{2}^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~{3}^Attribute~DOC_DATE~STR~{0:MM/dd/yyyy}^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~{0:MM/dd/yyyy}^Attribute~SCAN_TIME~STR~{0:HH:mm:ss}^Attribute~DESCRIPTION~STR~{0:MM/dd/yyyy} {1}", DateTime.Now, DateTime.Now.TimeOfDay, ssn, imagingDocId))
            controlWriter.WriteLine(String.Format("DesktopDoc~{0}{1}_{2}{3}~{4:MM/dd/yyyy} {5}, Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~{1}^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~{6}^Attribute~DOC_DATE~STR~{4:MM/dd/yyyy}^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~{4:MM/dd/yyyy}^Attribute~SCAN_TIME~STR~{4:HH:mm:ss}", imagingFolder, ssn, timeStamp, fileExtension, DateTime.Now, DateTime.Now.TimeOfDay, imagingDocId))
        End Using
    End Sub

    ''' <summary>
    ''' Overload for older scripts that passes system region and efs (assumes UHEAA) to main function.
    ''' </summary>
    ''' <param name="testMode"></param>
    ''' <param name="letterID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetDocumentPathAndFileName(ByVal testMode As Boolean, ByVal letterID As String) As DocumentPathAndName
        Dim efs = New EnterpriseFileSystem(testMode, ScriptSessionBase.Region.UHEAA)
        Return GetDocumentPathAndFileName(testMode, letterID, ScriptSessionBase.Region.UHEAA, efs)
    End Function

    'is used to gather information from the Db and calculate where to find a document and what it is called
    Private Shared Function GetDocumentPathAndFileName(ByVal testMode As Boolean, ByVal letterID As String, ByVal systemRegion As ScriptBase.Region, ByVal efs As EnterpriseFileSystem) As DocumentPathAndName
        Dim tempDPN As DocumentPathAndName
        Dim splitDPN As New List(Of String)
        Dim badFormatExceptionMsg As String = String.Format("The letter tracking path and file name data for document ""{0}"" is not in the correct format.  Please contact a member of Systems Support.  Note To Systems Support: Please ensure that letter tracking has the full file path and document name (the test directory shouldn't be included in the path).", letterID)
        Dim emptyExceptionMsg As String = String.Format("The letter tracking path and file name data for document ""{0}"" is empty.  Please contact a member of Systems Support.  Note To Systems Support: Please ensure that letter tracking has the full file path and document name.", letterID)
        tempDPN = DataAccess.GetDocumentPathAndFileName(testMode, letterID)
        'check for empty entry
        If tempDPN Is Nothing Then
            Throw New DocumentFileNameOrPathException(emptyExceptionMsg)
        End If
        If tempDPN.OriginalDBEntry Is Nothing Then
            Throw New DocumentFileNameOrPathException(emptyExceptionMsg)
        End If

        splitDPN.AddRange(tempDPN.OriginalDBEntry.Split("\"))
        'check for bad formats
        If splitDPN.Count <= 1 Then
            Throw New DocumentFileNameOrPathException(badFormatExceptionMsg)
        End If
        If splitDPN.Last().Contains(".") = False Then
            Throw New DocumentFileNameOrPathException(badFormatExceptionMsg)
        End If
        If splitDPN.Item(splitDPN.Count - 2).ToUpper = "TEST" Then
            Throw New DocumentFileNameOrPathException(badFormatExceptionMsg)
        End If
        tempDPN.CalculatedFileName = splitDPN.Last() 'get file name
        splitDPN.RemoveAt(splitDPN.Count - 1) 'remove file name from list

        'TESTIT:
        If systemRegion = ScriptSessionBase.Region.UHEAA Then
            If testMode Then
                tempDPN.CalculatedPath = String.Join("\", splitDPN.ToArray()) + "\Test\" 'calculate path
            Else
                tempDPN.CalculatedPath = String.Join("\", splitDPN.ToArray()) + "\" 'calculate path
            End If
        Else
            tempDPN.CalculatedPath = String.Join("\", splitDPN.ToArray()) + "\"
            If testMode Then
                Dim testDrive As String = efs.GetPath("DRIVE")
                tempDPN.CalculatedPath = String.Format("{0}{1}", testDrive, tempDPN.CalculatedPath.Substring(3))
            End If
        End If
        Return tempDPN
    End Function

#End Region

#Region "Cost Center Printing"
    Private Const COVER_SHEET_FOLDER As String = "X:\PADD\General\"

#Region "Federal"
    ''' <summary>
    ''' Does cost center printing for federal loan servicing.
    ''' </summary>
    ''' <param name="letterId">The letter ID from Letter Tracking.</param>
    ''' <param name="dataFile">The full path and name of the data file to be merged into the letter.</param>
    ''' <param name="stateCodeFieldName">The header field name for the data file's state code column.</param>
    ''' <param name="scriptId">The calling script's ID from Sacker.</param>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="acctNumFieldName"></param>
    ''' <param name="letterRecipient"></param>
    ''' <param name="options"></param>
    Public Shared Sub FederalCostCenterPrinting(ByVal testMode As Boolean, ByVal letterId As String, ByVal dataFile As String, ByVal stateCodeFieldName As String, ByVal scriptId As String, ByVal acctNumFieldName As String, ByVal letterRecipient As Barcode2DLetterRecipient, ByVal options As CostCenterOption)
        Dim efs As New EnterpriseFileSystem(testMode, ScriptSessionBase.Region.CornerStone)

        'creates a new datafile adding the 2D barcode
        Dim barcodedData As String = dataFile
        If (options And CostCenterOption.AddBarcode) = CostCenterOption.AddBarcode Then
            barcodedData = AddBarcodeAndStaticCurrentDateForBatchProcessing(dataFile, acctNumFieldName, letterId, True, letterRecipient, testMode)
        End If

        'Print the cover sheet.
        PrintFederalStateMailCoverSheet(testMode, barcodedData, letterId, stateCodeFieldName)

        'Print the letter.
        Dim docPathAndName As DocumentPathAndName = GetDocumentPathAndFileName(testMode, letterId, ScriptSessionBase.Region.CornerStone, efs)
        PrintDocs(docPathAndName.CalculatedPath, docPathAndName.CalculatedFileName, barcodedData, PrintOptions.SendToPrinter)
        If (options And CostCenterOption.AddBarcode) = CostCenterOption.AddBarcode Then File.Delete(barcodedData)
    End Sub

    Public Shared Sub PrintFederalStateMailCoverSheet(ByVal testMode As Boolean, ByVal barcodedDataFile As String, ByVal letterId As String, ByVal stateCodeFieldName As String)
        Dim coverSheetFolder As String = Common.TestMode(COVER_SHEET_FOLDER, testMode).DocFolder
        Dim coverSheetDataFile As String = CreateFederalCoverSheetDataFile(testMode, letterId, barcodedDataFile, stateCodeFieldName)
        PrintDocs(coverSheetFolder, "Scripted State Mail Cover Sheet", coverSheetDataFile, PrintOptions.SendToPrinter)
        If File.Exists(coverSheetDataFile) Then File.Delete(coverSheetDataFile)
    End Sub

    Private Shared Function CreateFederalCoverSheetDataFile(ByVal testMode As Boolean, ByVal letterId As String, ByVal dataFile As String, ByVal stateCodeFieldName As String) As String
        Const FEDERAL_BUSINESS_UNIT As String = "Federal Servicing"
        Const FEDERAL_COST_CENTER As String = "MA4481"

        Dim data As New DataAccess(testMode, ScriptSessionBase.Region.CornerStone)
        Dim efs As New EnterpriseFileSystem(testMode, ScriptSessionBase.Region.CornerStone)

        'Scan the data file to get counts of domestic and foreign addresses.
        Dim domesticCount As Integer = 0
        Dim foreignCount As Integer = 0
        Dim coverSheetInstructions As String = data.GetCostCenterInstructions(letterId)
        Using dataReader As New StreamReader(dataFile)
            Dim headerFields As List(Of String) = dataReader.ReadLine().SplitAgnosticOfQuotes(",")
            Dim stateCodeIndex As String = headerFields.IndexOf(stateCodeFieldName)
            If (stateCodeIndex < 0) Then
                Throw New Exception(String.Format("The state code field name ({0}) was not found in the header row.", stateCodeFieldName))
            End If
            While Not dataReader.EndOfStream
                Dim dataFields As List(Of String) = dataReader.ReadLine().SplitAgnosticOfQuotes(",")
                Dim stateCode As String = dataFields(stateCodeIndex)
                If (stateCode = "FC" OrElse stateCode.Trim().Length = 0) Then
                    foreignCount += 1
                Else
                    domesticCount += 1
                End If
            End While
        End Using

        'Determine the file name for the cover sheet data file.
        Dim dataFileBaseName As String = dataFile.Split("\").Last()
        Dim dataFileBaseNameMinusExtension As String = dataFileBaseName.Substring(0, dataFileBaseName.LastIndexOf("."))
        Dim coverSheetDataFile As String = String.Format("{0}{1}_Cover.txt", efs.TempFolder, dataFileBaseNameMinusExtension)

        'Write the header and data line to the cover sheet data file.
        Using coverSheetDataWriter As New StreamWriter(coverSheetDataFile)
            Dim letterName As String = data.GetLetterName(letterId)
            Dim pageCount As String = FigureSheetCount(data.GetPaperSheetCountForBatch2D(letterId)).ToString()
            coverSheetDataWriter.WriteCommaDelimitedLine("BU", "Description", "NumPages", "Cost", "Standard", "Foreign", "CoverComment")
            coverSheetDataWriter.WriteCommaDelimitedLine(FEDERAL_BUSINESS_UNIT, letterName, pageCount, FEDERAL_COST_CENTER, domesticCount, foreignCount, coverSheetInstructions)
        End Using

        'Add a record to the Cost Center Printing table if there's no special handling.
        If String.IsNullOrEmpty(coverSheetInstructions) Then
            data.AddFederalCostCenterPrintingRecord(letterId, foreignCount, domesticCount, FEDERAL_COST_CENTER)
        End If

        Return coverSheetDataFile
    End Function
#End Region 'Federal

#Region "Commercial"
    ''' <summary>
    ''' Does cost center printing for commercial loan servicing.
    ''' Unlike the other overload, this version does not require data files to be sorted by cost center.
    ''' </summary>
    ''' <param name="letterId">The letter ID from Letter Tracking.</param>
    ''' <param name="dataFile">The full path and name of the data file to be merged into the letter.</param>
    ''' <param name="costCenterFieldName">The header field name for the data file's cost center column.</param>
    ''' <param name="stateCodeFieldName">The header field name for the data file's state code column.</param>
    ''' <param name="scriptId">The calling script's ID from Sacker.</param>
    ''' <param name="testMode">True if running in test mode.</param>
    Public Shared Sub CostCenterPrinting(ByVal testMode As Boolean, ByVal letterId As String, ByVal dataFile As String, ByVal costCenterFieldName As String, ByVal stateCodeFieldName As String, ByVal scriptId As String)
        CostCenterPrinting(testMode, letterId, dataFile, costCenterFieldName, stateCodeFieldName, scriptId, "", Barcode2DLetterRecipient.lrBorrower, CostCenterOption.None)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="testMode"></param>
    ''' <param name="letterId"></param>
    ''' <param name="dataFile"></param>
    ''' <param name="costCenterFieldName"></param>
    ''' <param name="stateCodeFieldName"></param>
    ''' <param name="scriptId"></param>
    ''' <param name="acctNumFieldName"></param>
    ''' <param name="letterRecipient"></param>
    ''' <param name="options"></param>
    ''' <remarks></remarks>
    Private Shared Sub CostCenterPrinting(ByVal testMode As Boolean, ByVal letterId As String, ByVal dataFile As String, ByVal costCenterFieldName As String, ByVal stateCodeFieldName As String, ByVal scriptId As String, ByVal acctNumFieldName As String, ByVal letterRecipient As Barcode2DLetterRecipient, ByVal options As CostCenterOption)
        'creates new datafile adding the 2D barcode
        Dim barcodedData As String = dataFile
        If (options And CostCenterOption.AddBarcode) = CostCenterOption.AddBarcode Then
            barcodedData = AddBarcodeAndStaticCurrentDateForBatchProcessing(dataFile, acctNumFieldName, letterId, True, letterRecipient, testMode)
        End If

        Dim folders As TestModeResults = Common.TestMode(COVER_SHEET_FOLDER, testMode)

        'See if we're in recovery.
        Dim recoveryLog As String = String.Format("{0}CCP_{1}.txt", folders.LogFolder, scriptId)
        Dim alreadyPrintedCostCenters As New List(Of String)()
        If (File.Exists(recoveryLog) AndAlso New FileInfo(recoveryLog).CreationTime.Date = DateTime.Now.Date) Then
            'We're in recovery. Get a list of the cost centers that have already printed.
            For Each recoveryLine As String In File.ReadAllLines(recoveryLog)
                Dim recoveryFields As List(Of String) = recoveryLine.SplitAgnosticOfQuotes(",")
                If (recoveryFields(0) = letterId) Then alreadyPrintedCostCenters.Add(recoveryFields(1))
            Next recoveryLine
        Else
            'We're not in recovery. Re-initialize the recovery log.
            Using logWriter As New StreamWriter(recoveryLog, False)
                logWriter.WriteCommaDelimitedLine("LetterID", "CostCenterCode", "DomesticCount", "ForeignCount", "TimeStamp")
                logWriter.Close()
            End Using
        End If

        'Split the data file into separate files for each cost center.
        Dim costCenterDataFiles As List(Of CostCenterFileData) = CreateCostCenterDataFiles(barcodedData, costCenterFieldName, stateCodeFieldName)

        'Print the letters for each cost center file that hasn't been printed yet.
        For Each costCenterData As CostCenterFileData In costCenterDataFiles.Where(Function(p) alreadyPrintedCostCenters.Contains(p.CostCenterCode) = False)
            'Print the cover sheet.
            Dim coverSheetDataFile As String = CreateCoverSheetDataFile(testMode, letterId, costCenterData)
            PrintDocs(folders.DocFolder, "Scripted State Mail Cover Sheet", coverSheetDataFile, PrintOptions.SendToPrinter)
            If File.Exists(coverSheetDataFile) Then File.Delete(coverSheetDataFile)

            'Print the letter.
            Dim docPathAndName As DocumentPathAndName = GetDocumentPathAndFileName(testMode, letterId)
            PrintDocs(docPathAndName.CalculatedPath, docPathAndName.CalculatedFileName, costCenterData.DataFileName, PrintOptions.SendToPrinter)
            If File.Exists(costCenterData.DataFileName) Then File.Delete(costCenterData.DataFileName)

            UpdateLogFiles(recoveryLog, folders.DocFolder, letterId, costCenterData.CostCenterCode, costCenterData.DataFileName, costCenterData.DomesticAddressCount, costCenterData.ForeignAddressCount)
        Next costCenterData

        'delete the data file if it was created in this subroutine
        If (options And CostCenterOption.AddBarcode) = CostCenterOption.AddBarcode Then File.Delete(barcodedData)
    End Sub

    ''' <summary>
    ''' Splits a data file into separate files based on cost center.
    ''' Returns an array of objects that have a cost center code, data file name,
    ''' foreign address count, and domestic address count for each cost center found in the data file.
    ''' </summary>
    ''' <param name="dataFile">The data file to be split by cost center.</param>
    ''' <param name="costCenterFieldName">The header field name for the data file's cost center column.</param>
    ''' <param name="stateCodeFieldName">The header field name for the data file's state code column.</param>
    Private Shared Function CreateCostCenterDataFiles(ByVal dataFile As String, ByVal costCenterFieldName As String, ByVal stateCodeFieldName As String) As List(Of CostCenterFileData)
        'Make sure the data file exists.
        If (Not File.Exists(dataFile)) Then
            Throw New Exception(String.Format("File {0} is missing. Contact Systems Support.", dataFile))
        End If

        'Initialize the list that will make up the return value.
        Dim costCenterFileList As New List(Of CostCenterFileData)()

        'Open the data file.
        Using fileReader As New StreamReader(dataFile)
            'Get the indices of the cost center and state code fields from the header row.
            Dim headerFields As List(Of String) = fileReader.ReadLine().SplitAgnosticOfQuotes(",")
            Dim costCenterIndex As Integer = headerFields.IndexOf(costCenterFieldName)
            If (costCenterIndex < 0) Then
                Throw New Exception(String.Format("The cost center code field name ({0}) was not found in the header row.", costCenterFieldName))
            End If
            Dim stateCodeIndex As Integer = headerFields.IndexOf(stateCodeFieldName)
            If (stateCodeIndex < 0) Then
                Throw New Exception(String.Format("The state code field name ({0}) was not found in the header row.", stateCodeFieldName))
            End If

            'Create a dictionary of StreamWriters, indexed by cost center.
            'This will make it easy to copy a record from the main data file into the correct
            'cost center data file, and close all the cost center data files when we're done.
            Dim costCenterWriters As New Dictionary(Of String, StreamWriter)()

            'Read each line and see what cost center it belongs to.
            Do While Not fileReader.EndOfStream
                Dim dataFields As List(Of String) = fileReader.ReadLine().SplitAgnosticOfQuotes(",")
                Dim costCenter As String = dataFields(costCenterIndex)
                If (Not costCenterWriters.ContainsKey(costCenter)) Then
                    'Create a new StreamWriter for this cost center and add it to the dictionary.
                    Dim costCenterDataFile As String = String.Format("{0}{1}{2}.txt", DataAccessBase.PersonalDataDirectory, dataFile.Substring(dataFile.LastIndexOf("\") + 1), costCenter)
                    Dim costCenterWriter As New StreamWriter(costCenterDataFile, False)
                    costCenterWriters.Add(costCenter, costCenterWriter)
                    'Write the header row and the data line.
                    costCenterWriter.WriteCommaDelimitedLine(headerFields.ToArray())
                    costCenterWriter.WriteCommaDelimitedLine(dataFields.ToArray())

                    'Create an entry in the return list.
                    Dim costCenterData As New CostCenterFileData With {.CostCenterCode = costCenter, .DataFileName = costCenterDataFile, .DomesticAddressCount = 0, .ForeignAddressCount = 0}
                    Dim stateCode As String = dataFields(stateCodeIndex)
                    If (stateCode = "FC" OrElse stateCode.Trim().Length = 0) Then
                        costCenterData.ForeignAddressCount = 1
                    Else
                        costCenterData.DomesticAddressCount = 1
                    End If
                    costCenterFileList.Add(costCenterData)
                Else
                    'Write the data line to the appropriate StreamWriter.
                    costCenterWriters(costCenter).WriteCommaDelimitedLine(dataFields.ToArray())

                    'Increment the foreign or domestic count in the appropriate entry in the return list.
                    Dim costCenterData As CostCenterFileData = costCenterFileList.Where(Function(p) p.CostCenterCode = costCenter).Single()
                    Dim stateCode As String = dataFields(stateCodeIndex)
                    If (stateCode = "FC" OrElse stateCode.Trim().Length = 0) Then
                        costCenterData.ForeignAddressCount += 1
                    Else
                        costCenterData.DomesticAddressCount += 1
                    End If
                End If
            Loop

            'Close and dispose all of the StreamWriters.
            For Each costCenterWriter As StreamWriter In costCenterWriters.Values
                costCenterWriter.Close()
                costCenterWriter.Dispose()
            Next

            fileReader.Close()
        End Using

        Return costCenterFileList
    End Function

    Private Shared Function CreateCoverSheetDataFile(ByVal testMode As Boolean, ByVal letterId As String, ByVal costCenterData As CostCenterFileData) As String
        'Determine the file name for the cover sheet data file.
        Dim dataFileBaseName As String = costCenterData.DataFileName.Split("\").Last()
        Dim dataFileBaseNameMinusExtension As String = dataFileBaseName.Substring(0, dataFileBaseName.LastIndexOf("."))
        Dim coverSheetDataFile As String = String.Format("{0}{1}_Cover.txt", DataAccessBase.PersonalDataDirectory, dataFileBaseNameMinusExtension)

        'Write the header and data line to the cover sheet data file.
        Dim data As New DataAccess(testMode, ScriptSessionBase.Region.UHEAA)
        Using coverSheetDataWriter As New StreamWriter(coverSheetDataFile)
            Dim businessUnit As String = data.GetBusinessUnitNameForCostCenterPrinting(costCenterData.CostCenterCode)
            Dim letterName As String = data.GetLetterName(letterId)
            Dim pageCount As String = FigureSheetCount(data.GetPaperSheetCountForBatch2D(letterId)).ToString()
            Dim coverSheetComment As String = data.GetCostCenterInstructions(letterId)
            coverSheetDataWriter.WriteCommaDelimitedLine("BU", "Description", "NumPages", "Cost", "Standard", "Foreign", "CoverComment")
            coverSheetDataWriter.WriteCommaDelimitedLine(businessUnit, letterName, pageCount, costCenterData.CostCenterCode, costCenterData.DomesticAddressCount, costCenterData.ForeignAddressCount, coverSheetComment)
            coverSheetDataWriter.Close()
        End Using

        Return coverSheetDataFile
    End Function

    Private Shared Sub UpdateLogFiles(ByVal recoveryFile As String, ByVal docFolder As String, ByVal letterId As String, ByVal costCenterCode As String, ByVal costCenterDataFile As String, ByVal domesticCount As Integer, ByVal foreignCount As Integer)
        'Make sure the Master Cost Center file exists. Create it, with a header row, if needed.
        Dim masterFile As String = String.Format("{0}MasterCostCenterFile.txt", docFolder)
        If (Not File.Exists(masterFile)) Then
            Using masterWriter As New StreamWriter(masterFile)
                masterWriter.WriteCommaDelimitedLine("Date", "LetterID", "Foreign", "Count", "CostCenterCode", "File", "TimeStamp")
                masterWriter.Close()
            End Using
        End If

        'Append to the Master Cost Center file.
        Do
            Try
                Using masterWriter As New StreamWriter(masterFile, True)
                    If domesticCount > 0 Then
                        masterWriter.WriteCommaDelimitedLine(DateTime.Now.ToString("MM/dd/yyyy"), letterId, "", domesticCount.ToString(), costCenterCode, costCenterDataFile, DateTime.Now.ToString())
                    End If
                    If foreignCount > 0 Then
                        masterWriter.WriteCommaDelimitedLine(DateTime.Now.ToString("MM/dd/yyyy"), letterId, "F", foreignCount.ToString(), costCenterCode, costCenterDataFile, DateTime.Now.ToString())
                    End If
                    masterWriter.Close()
                End Using
                Exit Do
            Catch ex As IOException
                'The file is busy. Wait one second and try again.
                Thread.Sleep(1000)
            End Try
        Loop

        'Append to the recovery log.
        Using logWriter As New StreamWriter(recoveryFile, True)
            logWriter.WriteCommaDelimitedLine(letterId, costCenterCode, domesticCount.ToString(), foreignCount.ToString(), DateTime.Now.ToString())
            logWriter.Close()
        End Using
    End Sub
#End Region 'Commercial

#Region "DEPRECATED"
    ''' <summary>
    ''' DEPRECATED!
    ''' This variable is only used by the CostCenterPrinting overload that's being deprecated.
    ''' It can go away when that overload goes away.
    ''' </summary>
    Private Shared costCenterTestModeRe As TestModeResults

    ''' <summary>
    ''' DEPRECATED!
    ''' This overload is being replaced by a new one that does automatic recovery and gets
    ''' data from Letter Tracking based on the letter ID, and so requires fewer parameters.
    ''' </summary>
    ''' <param name="letterDescription">The letter's description.  Will be added to cost center cover sheet.</param>
    ''' <param name="pageCount">The page count for a letter.  If page number is not known then this can also double as who to have it delivered to.</param>
    ''' <param name="letterId">The letter id from Letter Tracking.</param>
    ''' <param name="dataFile">The data file for the merge.</param>
    ''' <param name="costCenterCodeFieldName">Cost center code field name</param>
    ''' <param name="stateCodeFieldName">State code field name</param>
    ''' <param name="timeStamp">Time date stamp</param>
    ''' <param name="scriptId">Script id from Sacker</param>
    ''' <param name="isTestMode">Is the application in test mode.</param>
    ''' <param name="inRecovery">Is the application in recovery.  Before conversion to C# this defaulted to false.</param>
    ''' <param name="toPrinter">Print to printer.  Before conversion to C# this defaulted to true.</param>
    ''' <remarks></remarks>
    Public Shared Sub CostCenterPrinting(ByVal letterDescription As String, ByVal pageCount As DestinationOrPageCount, ByVal letterId As String, ByVal dataFile As String, ByVal costCenterCodeFieldName As String, ByVal stateCodeFieldName As String, ByVal timeStamp As String, ByVal scriptId As String, ByVal isTestMode As Boolean, ByVal inRecovery As Boolean, ByVal toPrinter As Boolean)
        'create comment for variable page letters
        'when PagesPerDocStr = "" no record will be added to the Master CC File
        Dim coverSheetComment As String = String.Empty
        Dim coverSheetPagesPerDoc As String = pageCount
        Dim DocPathAndName As DocumentPathAndName = GetDocumentPathAndFileName(isTestMode, letterId)
        costCenterTestModeRe = Common.TestMode(COVER_SHEET_FOLDER, isTestMode)
        If pageCount = -1 Then
            coverSheetComment = "Deliver mail to business unit for processing"
            coverSheetPagesPerDoc = String.Empty
        ElseIf pageCount = -2 Then
            coverSheetComment = "Special Handling Required - Document Services"
            coverSheetPagesPerDoc = String.Empty
        End If

        Dim logFile As String = String.Format("{0}CCP{1}.txt", costCenterTestModeRe.LogFolder, scriptId)
        If (Not File.Exists(logFile)) Then
            FileOpen(3, logFile, OpenMode.Output)
            Write(3, timeStamp)
            FileClose(3)
        End If
        If (Not inRecovery) Then
            'write over the old log file.
            FileOpen(3, logFile, OpenMode.Output)
            Write(3, timeStamp)
            FileClose(3)
        End If

        'create cover sheets
        Dim data As New DataAccess(isTestMode, ScriptSessionBase.Region.UHEAA)
        For Each costCenterFile As CostCenterFileData In CcpSplit(dataFile, costCenterCodeFieldName, stateCodeFieldName)
            Dim costCenterDataFilePathElements() As String = Split(costCenterFile.DataFileName, "\")
            Dim coverSheetDataFile As String = DataAccessBase.PersonalDataDirectory & Mid(costCenterDataFilePathElements(UBound(costCenterDataFilePathElements)), 1, Len(costCenterDataFilePathElements(UBound(costCenterDataFilePathElements))) - 4) & "Cover.txt"
            FileOpen(1, coverSheetDataFile, OpenMode.Output)
            WriteLine(1, "BU", "Description", "NumPages", "Cost", "Standard", "Foreign", "CoverComment")
            WriteLine(1, data.GetBusinessUnitNameForCostCenterPrinting(costCenterFile.CostCenterCode), letterDescription, coverSheetPagesPerDoc, costCenterFile.CostCenterCode, costCenterFile.DomesticAddressCount, costCenterFile.ForeignAddressCount, coverSheetComment)
            FileClose(1)

            'print cover sheet and corelating data file
            Dim commentExistsInMasterFile As Boolean = False 'This gets set to true if the comment is already found in the master file.
            Dim options As PrintOptions = PrintOptions.None
            If (isTestMode) Then
                options = options Or PrintOptions.DisplayOnScreen
            Else
                options = options Or PrintOptions.SendToPrinter
            End If
            If (Not inRecovery) Then
                PrintDocs(costCenterTestModeRe.DocFolder, "Scripted State Mail Cover Sheet", coverSheetDataFile, options)
                If File.Exists(coverSheetDataFile) Then
                    File.Delete(coverSheetDataFile)
                End If
                'print letters for this cover sheet.
                PrintDocs(DocPathAndName.CalculatedPath, DocPathAndName.CalculatedFileName, costCenterFile.DataFileName, options)
                'add comment to log file data file printed
            Else
                FileOpen(5, logFile, OpenMode.Input)
                Input(5, timeStamp)
                FileClose(5)
                Dim addressCount As Integer = costCenterFile.ForeignAddressCount
                If addressCount = 0 Then
                    addressCount = costCenterFile.DomesticAddressCount
                End If
                If (Not FindInMasterCCFile(isTestMode, Date.Now.ToString("MM/dd/yyyy"), letterId, addressCount.ToString(), costCenterFile.CostCenterCode, costCenterFile.DataFileName, timeStamp)) Then
                    PrintDocs(costCenterTestModeRe.DocFolder, "Scripted State Mail Cover Sheet", coverSheetDataFile, options)
                    PrintDocs(DocPathAndName.CalculatedPath, DocPathAndName.CalculatedFileName, costCenterFile.DataFileName, options)
                Else
                    'comment exists in master file already, documents already printed printing.
                    commentExistsInMasterFile = True
                End If
            End If

            If File.Exists(coverSheetDataFile) Then
                File.Delete(coverSheetDataFile)
            End If
            If File.Exists(costCenterFile.DataFileName) Then
                File.Delete(costCenterFile.DataFileName)
            End If

            'Add record to the Master Cost Center File if (your not in recovery, or if you are in recovery and the file is not found in the Master CC File) and also this is not a -1 or -2 PagesPerDoc
            If ((Not inRecovery) OrElse ((inRecovery) AndAlso (Not commentExistsInMasterFile))) _
            AndAlso coverSheetPagesPerDoc <> String.Empty _
            Then
                If (Not File.Exists(String.Format("{0}MasterCostCenterFile.txt", costCenterTestModeRe.DocFolder))) Then
                    FileOpen(3, String.Format("{0}MasterCostCenterFile.txt", costCenterTestModeRe.DocFolder), OpenMode.Output)
                    Write(3, "Date", "LetterID", "Foreign", "Count", "CostCenterCode", "File", "TimeStamp")
                    FileClose(3)
                End If
                'loop until file access is granted
                Do
                    Try
                        FileClose(3)
                        FileOpen(3, String.Format("{0}MasterCostCenterFile.txt", costCenterTestModeRe.DocFolder), OpenMode.Append)
                        Exit Do
                    Catch ex As IOException
                        'The file is busy
                    End Try
                    Thread.Sleep(1000)
                Loop
                If costCenterFile.ForeignAddressCount > 0 Then
                    WriteLine(3, Date.Now.ToString("MM/dd/yyyy"), letterId, "F", costCenterFile.ForeignAddressCount, costCenterFile.CostCenterCode, costCenterFile.DataFileName, timeStamp)
                End If
                If costCenterFile.DomesticAddressCount > 0 Then
                    WriteLine(3, Date.Now.ToString("MM/dd/yyyy"), letterId, "", costCenterFile.DomesticAddressCount, costCenterFile.CostCenterCode, costCenterFile.DataFileName, timeStamp)
                End If
                FileClose(3)
            End If
        Next costCenterFile
    End Sub

    ''' <summary>
    ''' DEPRECATED!
    ''' This function is only used by the CostCenterPrinting overload that has been marked DEPRECATED,
    ''' and will be going away when that overload goes away.
    ''' </summary>
    Private Shared Function FindInMasterCCFile(ByVal isTestMode As Boolean, ByVal printDate As String, ByVal letterId As String, ByVal addressCount As String, ByVal costCenterCode As String, ByVal fileName As String, ByVal timeStamp As String) As Boolean
        'accepts info to search for in Master cost center file, returns true if file is found
        'loop until file access is granted
        Do
            Try
                FileClose(3)
                FileOpen(3, String.Format("{0}MasterCostCenterFile.txt", costCenterTestModeRe.DocFolder), OpenMode.Append)
                Exit Do
            Catch ex As IOException
                'The file is busy
            End Try
            Thread.Sleep(1000)
        Loop

        Do While Not EOF(3)
            Dim fields() As String = LineInput(3).Replace("""", "").Split(",")
            If printDate = fields(0) AndAlso letterId = fields(1) AndAlso addressCount = fields(3) AndAlso _
            costCenterCode = fields(4) AndAlso fileName = fields(5) AndAlso timeStamp = fields(6) Then
                FileClose(3)
                Return True
            End If
        Loop
        FileClose(3)

        Return False
    End Function

    ''' <summary>
    ''' DEPRECATED!
    ''' This function is only used by the CostCenterPrinting overload that has been marked DEPRECATED,
    ''' and will be going away when that overload goes away.
    ''' </summary>
    Private Shared Function CcpSplit(ByVal dataFile As String, ByVal costCenterCodeFieldName As String, ByVal stateCodeFieldName As String) As List(Of CostCenterFileData)
        'This function returns a list of objects containing foriegn count, non-foriegn count and file name
        'for each Cost Center Code contained in the dataFile inserted in to the function.
        If (Not File.Exists(dataFile)) Then
            Throw New Exception(String.Format("File {0} is missing. Contact Systems Support.", dataFile))
        End If

        'Get the indices of the cost center code and state code.
        Dim costCenterCodeIndex As Integer = -1
        Dim stateCodeIndex As Integer = -1
        FileOpen(1, dataFile, OpenMode.Input)
        Dim headerLine As String = LineInput(1)
        Dim headerFields() As String = headerLine.Split(",")
        For currentIndex = 0 To headerFields.Count() - 1
            Dim currentFieldMinusQuotes As String = headerFields(currentIndex).Replace("""", "")
            If currentFieldMinusQuotes = costCenterCodeFieldName Then
                costCenterCodeIndex = currentIndex
            End If
            If currentFieldMinusQuotes = stateCodeFieldName Then
                stateCodeIndex = currentIndex
            End If
        Next currentIndex
        'Make sure we found both indices.
        If costCenterCodeIndex < 0 Then
            FileClose(1)
            Throw New Exception(String.Format("The cost center code field name ({0}) was not found in the header row.", costCenterCodeFieldName))
        End If
        If stateCodeIndex < 0 Then
            FileClose(1)
            Throw New Exception(String.Format("The state code field name ({0}) was not found in the header row.", stateCodeFieldName))
        End If

        'Build the list that will make up the return value.
        Dim costCenterFileList As New List(Of CostCenterFileData)()
        Dim previousCostCenterCode As String = String.Empty
        Do While Not EOF(1)
            'Read in a line and get the cost center code and state code.
            Dim dataLine As String = LineInput(1)
            Dim dataFields() As String = dataLine.Split(New String() {""","""}, StringSplitOptions.None)
            Dim costCenterCode As String = dataFields(costCenterCodeIndex).Replace("""", "")
            Dim stateCode As String = dataFields(stateCodeIndex).Replace("""", "")
            Dim costCenterDataFile As String = String.Format("{0}{1}{2}.txt", PersonalDataDirectory, dataFile.Substring(dataFile.LastIndexOf("\") + 1), costCenterCode)

            If costCenterCode <> previousCostCenterCode Then
                'This is a new cost center. Add an item to the list.
                'We use the Insert method so that we can be sure which index the new item is.
                costCenterFileList.Insert(0, New CostCenterFileData With {.CostCenterCode = costCenterCode, .DataFileName = costCenterDataFile, .DomesticAddressCount = 0, .ForeignAddressCount = 0})
                'Create a new data file for this cost center and write out the header and current line of data.
                FileOpen(2, costCenterDataFile, OpenMode.Output)
                PrintLine(2, headerLine)
                PrintLine(2, dataLine)
                FileClose(2)
            Else
                'Add the current line of data to the cost center data file.
                FileOpen(2, costCenterDataFile, OpenMode.Append)
                PrintLine(2, dataLine)
                FileClose(2)
            End If
            'Increment the appropriate address counter.
            If stateCode = "FC" OrElse stateCode.Trim() = String.Empty Then
                costCenterFileList(0).ForeignAddressCount += 1
            Else
                costCenterFileList(0).DomesticAddressCount += 1
            End If
            previousCostCenterCode = costCenterCode
        Loop
        FileClose(1)

        Return costCenterFileList
    End Function
#End Region 'DEPRECATED

    Private Class CostCenterFileData
        Private _costCenterCode As String
        Public Property CostCenterCode() As String
            Get
                Return _costCenterCode
            End Get
            Set(ByVal value As String)
                _costCenterCode = value
            End Set
        End Property

        Private _dataFileName As String
        Public Property DataFileName() As String
            Get
                Return _dataFileName
            End Get
            Set(ByVal value As String)
                _dataFileName = value
            End Set
        End Property

        Private _domesticAddressCount As Integer
        Public Property DomesticAddressCount() As Integer
            Get
                Return _domesticAddressCount
            End Get
            Set(ByVal value As Integer)
                _domesticAddressCount = value
            End Set
        End Property

        Private _foreignAddressCount As Integer
        Public Property ForeignAddressCount() As Integer
            Get
                Return _foreignAddressCount
            End Get
            Set(ByVal value As Integer)
                _foreignAddressCount = value
            End Set
        End Property
    End Class
#End Region 'Cost Center Printing

#Region "2D Barcode and Centralized Printing"
    'interacts with the 2D barcode dll
    Private Shared Function EncNDM(ByVal dataToEncode As String, Optional ByVal procTilde As Integer = 0, Optional ByVal encMode As Integer = 0, Optional ByVal prefFormat As Integer = 0) As String
        EncNDM = ""
        'Format the data to the Data Matrix Font by calling the ActiveX DLL:
        Dim DMFontEncoder As DMATRIXLib.Datamatrix
        DMFontEncoder = New Datamatrix
        DMFontEncoder.FontEncode(dataToEncode, procTilde, encMode, prefFormat, EncNDM)
    End Function

    'calculates the sheet count
    Private Shared Function FigureSheetCount(ByVal pageCountResults As Barcode2DQueryResults) As Integer
        Dim paperSheetNumber As Integer
        If pageCountResults.Pages = "0" Then
            'sheet count wasn't populated
            Throw (New BarcodeException("The paper sheet count for the letter id that the script is using isn't populated.  Please contact a member of Systems Support"))
        Else
            If CInt(pageCountResults.Pages) = 1 Then 'if page number equals 1 then the sheet count is 1
                paperSheetNumber = 1
            ElseIf pageCountResults.Duplex = False Then 'if not duplex then sheet count equals page count
                paperSheetNumber = CInt(pageCountResults.Pages)
            Else 'if marked to do duplex then figure out how many pages it is going to take
                paperSheetNumber = CInt(pageCountResults.Pages) \ 2
                If (CInt(pageCountResults.Pages) Mod 2) > 0 Then
                    paperSheetNumber = paperSheetNumber + 1
                End If
            End If
        End If
        Return paperSheetNumber
    End Function

#Region "Batch 2D Barcode"
    ''' <summary>
    ''' Adds 2D barcode fields and the static date field to the given data file.  FOR BATCH SCRIPT USE ONLY.
    ''' </summary>
    ''' <param name="dataFile">Data file to add fields and data to.</param>
    ''' <param name="acctNumFieldName">The name of the account number field (from header row).</param>
    ''' <param name="letterID">The letter id of the document</param>
    ''' <param name="createNewFile">True = new file created and existing one stays as is or False = write over the top of existing file</param>
    ''' <param name="letterRecipient">The letter recipient</param>
    ''' <param name="testMode">Is the appication in test mode</param>
    ''' <returns>The file name to process.  This may be the same as fileToProc depending on value of newFile.</returns>
    Public Shared Function AddBarcodeAndStaticCurrentDateForBatchProcessing(ByVal dataFile As String, ByVal acctNumFieldName As String, ByVal letterId As String, ByVal createNewFile As Boolean, ByVal letterRecipient As Barcode2DLetterRecipient, ByVal testMode As Boolean) As String
        If (New FileInfo(dataFile).Length = 0) Then
            'Ignore empty data files.
            Return dataFile
        Else
            'The user 2D barcode function does exactly what we need, as long as we pass in a 0 for the batchDocNum parameter.
            Return DocumentHandling.AddBarcodeAndStaticCurrentDateForUserProcessing(testMode, dataFile, acctNumFieldName, letterId, createNewFile, 0, True, letterRecipient).NewFileName
        End If
    End Function
#End Region

#Region "User 2D Barcode and Centeralized Printing"

    ''' <summary>
    ''' Overload to support older code which does not specify the system region.  Passes UHEAA as the region to the main subroutine.  Main entry point that calls all helper functions as needed to accomplish Centralized printing, adding the static current date and adding the 2D barcode.  FOR USER SCRIPTS ONLY.
    ''' </summary>
    Public Shared Sub GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(ByVal ri As ReflectionInterface, ByVal letterID As String, ByVal systemToAddCommentsTo As CentralizedPrintingSystemToAddComments, ByVal ssn As String, ByVal dataFile As String, ByVal acctNumOrRefIDFieldNm As String, ByVal scriptID As String, ByVal stateCodeFieldNm As String, ByVal passedInDeployMethod As CentralizedPrintingDeploymentMethod, ByVal ContactType As String, ByVal LetterRecip As Barcode2DLetterRecipient)
        GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(ri, letterID, systemToAddCommentsTo, ssn, dataFile, acctNumOrRefIDFieldNm, scriptID, stateCodeFieldNm, passedInDeployMethod, ContactType, LetterRecip, ScriptSessionBase.Region.UHEAA)
    End Sub
    ''' <summary>
    ''' Overload to support older code which does not specify if the Document should be moved to the users T drive before processing.  This will not copy the document to the T drive for processing FOR USER SCRIPTS ONLY.
    ''' </summary>
    Public Shared Function GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(ByVal ri As ReflectionInterface, ByVal letterID As String, ByVal systemToAddCommentsTo As CentralizedPrintingSystemToAddComments, ByVal ssn As String, ByVal dataFile As String, ByVal acctNumOrRefIDFieldNm As String, ByVal scriptID As String, ByVal stateCodeFieldNm As String, ByVal passedInDeployMethod As CentralizedPrintingDeploymentMethod, ByVal ContactType As String, ByVal LetterRecip As Barcode2DLetterRecipient, ByVal systemRegion As ScriptBase.Region) As String
        Return GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(ri, letterID, systemToAddCommentsTo, ssn, dataFile, acctNumOrRefIDFieldNm, scriptID, stateCodeFieldNm, passedInDeployMethod, ContactType, LetterRecip, systemRegion, False)
    End Function

    ''' <summary>
    ''' Main entry point that calls all helper functions as needed to accomplish Centralized printing, adding the static current date and adding the 2D barcode.  FOR USER SCRIPTS ONLY.
    ''' </summary>
    ''' <param name="ri">Instance of Reflection Interface</param>
    ''' <param name="letterID">Letter ID from Letter Tracking</param>
    ''' <param name="systemToAddCommentsTo">System to add comments to (OneLINK or Compass)</param>
    ''' <param name="ssn">SSN of the borrower</param>
    ''' <param name="dataFile">The data file for the merge operation (Must have one data row)</param>
    ''' <param name="acctNumOrRefIDFieldNm">Field name for the Account Number (for borrower) or Reference ID (for reference)</param>
    ''' <param name="scriptID">Script id from Sacker</param>
    ''' <param name="stateCodeFieldNm">Field name for the state code field</param>
    ''' <param name="passedInDeployMethod">Desired deployment method.  If anything other than User Prompt is passsed then the user is required to use that method else the user will be given a prompt to choose a method and provide the needed information for the chosen method.</param>
    ''' <param name="ContactType">The contact type to be used when adding notes to the systems (OneLINK or Compass).  If not specified in the spec "03" should be used (was the default before moving to C#.</param>
    ''' <param name="LetterRecip">The recipient of the letter.  If not specified in the spec "borrower" should be used (was default before moving to C#).</param>
    ''' <remarks>This method expects the data file that is sent to it to have a single data row in it</remarks>
    ''' Public Shared Sub GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(ByVal ri As ReflectionInterface, ByVal letterID As String, ByVal systemToAddCommentsTo As CentralizedPrintingSystemToAddComments, ByVal ssn As String, ByVal dataFile As String, ByVal acctNumOrRefIDFieldNm As String, ByVal docName As String, ByVal docPath As String, ByVal userID As String, ByVal scriptID As String, ByVal stateCodeFieldNm As String, ByVal passedInDeployMethod As CentralizedPrintingDeploymentMethod, ByVal ContactType As String, ByVal LetterRecip As Barcode2DLetterRecipient)
    Public Shared Function GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(ByVal ri As ReflectionInterface, ByVal letterID As String, ByVal systemToAddCommentsTo As CentralizedPrintingSystemToAddComments, ByVal ssn As String, ByVal dataFile As String, ByVal acctNumOrRefIDFieldNm As String, ByVal scriptID As String, ByVal stateCodeFieldNm As String, ByVal passedInDeployMethod As CentralizedPrintingDeploymentMethod, ByVal ContactType As String, ByVal LetterRecip As Barcode2DLetterRecipient, ByVal systemRegion As ScriptBase.Region, ByVal processOffT As Boolean) As String
        Dim processingMethod As CentralizedPrintingDeploymentMethod
        Dim saveAs As String = ""
        Dim efs As New EnterpriseFileSystem(ri.TestMode, systemRegion)
        Dim barcodeAdditionResults As UserBarcodeAdditionResults
        'get information from DB for user, populate object and return it
        Dim procDat As CentralizedPrintingAnd2DBarcodeInfo = GetUserInfoForCentralizedPrintAnd2DObject(ri.TestMode)
        procDat.ContinueInitializing(ri, ssn, systemRegion)
        Dim FrmMethod As New LetterDeliveryMethod(ri.TestMode, procDat, passedInDeployMethod)
        Dim DocPathAndName As DocumentPathAndName = GetDocumentPathAndFileName(ri.TestMode, letterID, systemRegion, efs)
        If processOffT Then

            If File.Exists(String.Format("{0}{1}", efs.TempFolder, DocPathAndName.CalculatedFileName)) Then
                File.Delete(String.Format("{0}{1}", efs.TempFolder, DocPathAndName.CalculatedFileName))
            End If
            File.Copy(String.Format("{0}{1}", DocPathAndName.CalculatedPath, DocPathAndName.CalculatedFileName), String.Format("{0}{1}", efs.TempFolder, DocPathAndName.CalculatedFileName))
            DocPathAndName.CalculatedPath = efs.TempFolder
        End If
        'get delivery method information if needed
        If passedInDeployMethod <> CentralizedPrintingDeploymentMethod.dmLetter Then
            'if anything other than letter is the default then get needed information
            If FrmMethod.ShowDialog() = DialogResult.Cancel Then
                Common.EndDLLScript()
            End If
            processingMethod = FrmMethod.SelectedDeploymentMethod
        Else
            'if letter is default then no other information is needed
            processingMethod = CentralizedPrintingDeploymentMethod.dmLetter
        End If
        If processingMethod = CentralizedPrintingDeploymentMethod.dmEmail Then
            Dim FN As String
            Dim EmailTo As String
            Dim FTD As String
            'update email address if one on LP22 was blank or invalid
            If FrmMethod.OriginalEmailBlankOrInvalid AndAlso FrmMethod.radUpdateEmail.Checked Then
                'update TX1J and LP22, also add comments to systems
                If systemRegion = ScriptSessionBase.Region.UHEAA Then
                    'LP22
                    ri.FastPath("LP22C" & ssn)
                    If ri.Check4Text(1, 62, "PERSON DEMOGRAPHICS") Then
                        'source code
                        ri.PutText(3, 9, "F")
                        ri.PutText(19, 9, FrmMethod.txtEmailTo.Text)
                        ri.PutText(18, 56, "Y", F6) 'validate and enter date
                    End If
                End If
                'TX1J
                ri.FastPath("TX3ZCTX1J;" & ssn)
                If ri.Check4Text(1, 72, "TXX1K") = False AndAlso ri.Check4Text(1, 71, "TXX1R-01") = False Then
                    Throw New DemographicUpdateException("There was an error while trying to access TX1J.")
                ElseIf ri.Check4Text(1, 72, "TXX1K") = False Then
                    'try and update if borrower exists on COMPASS
                    ri.Hit(F2)
                    ri.Hit(F10)
                    'source code
                    ri.PutText(9, 20, "41")
                    'verified date
                    ri.PutText(11, 17, Now.ToString("MMddyy"))
                    'verified indicator
                    ri.PutText(12, 14, "Y")
                    'blank out current email addr
                    ri.PutText(14, 10, "", EndKey)
                    ri.PutText(15, 10, "", EndKey)
                    ri.PutText(16, 10, "", EndKey)
                    ri.PutText(17, 10, "", EndKey)
                    ri.PutText(18, 10, "", EndKey)
                    'add new email addr
                    ri.PutText(14, 10, FrmMethod.txtEmailTo.Text, Enter)
                    'add activity comments
                    AddComments(ri, CentralizedPrintingSystemToAddComments.stacBoth, ssn, "MXADD", "04", "TC", scriptID, "Updated email address through document generation script", procDat.UsersBuSentFromEmail, procDat.BrwDemographics.AccountNumber)
                End If
            End If
            'create barcode fields in data file
            barcodeAdditionResults = AddBarcodeAndStaticCurrentDateForUserProcessing(ri, dataFile, acctNumOrRefIDFieldNm, letterID, True, 0, True, Barcode2DLetterRecipient.lrBorrower, efs)
            Try
                FTD = Now.ToString("MMddyyyyhhmmss")
                saveAs = efs.TempFolder & "DOCGENRSCRPT_" & letterID & "_" & FTD & ".doc"
                SaveDocs(DocPathAndName.CalculatedPath, DocPathAndName.CalculatedFileName, barcodeAdditionResults.NewFileName, saveAs)
                If ri.TestMode Then
                    EmailTo = Environ("USERNAME") & "@utahsbr.edu"
                Else
                    EmailTo = FrmMethod.txtEmailTo.Text
                End If
                Common.SendMail(ri.TestMode, EmailTo, procDat.UsersBuSentFromEmail, FrmMethod.txtEmailSubject.Text, FrmMethod.txtEmailMessage.Text, "", "", saveAs, OSSMTP.importance_level.ImportanceNormal, True)
                'clean up
                Kill(barcodeAdditionResults.NewFileName)
                'delete files older that 30 day from local PC
                FN = Dir(efs.TempFolder & "DOCGENRSCRPT_*")
                While FN <> ""
                    If FileDateTime(efs.TempFolder & FN) < DateAdd("D", -30, Now) Then Kill(efs.TempFolder & FN)
                    FN = Dir()
                End While
                'add comments
                AddComments(ri, systemToAddCommentsTo, ssn, "MLEX1", ContactType, "EM", scriptID, "e-mailed " & letterID & " document to " & FrmMethod.txtEmailTo.Text, procDat.UsersBuSentFromEmail, barcodeAdditionResults.AcctNum)
            Catch ex As Exception
                Throw New CentralizedPrintingException("The letter you requested was not generated.  Please re-run the script and re-send the letter.  Please see inner exception for more information.", ex)
            End Try
        ElseIf processingMethod = CentralizedPrintingDeploymentMethod.dmLetter Then
            Dim results As LetterRecordCreationResults = New LetterRecordCreationResults()
            'create print record and get id and batch id back
            results = CreateLetterRecordForCentralizedPrinting(ri.TestMode, letterID, procDat.BrwDemographics.AccountNumber, procDat.UsersBusinessUnit, GetStateCode(dataFile, stateCodeFieldNm), systemRegion)
            barcodeAdditionResults = AddBarcodeAndStaticCurrentDateForUserProcessing(ri, dataFile, acctNumOrRefIDFieldNm, letterID, True, results.BarcodeSeqNum, True, LetterRecip, efs)

            saveAs = String.Format("{0}{1}_{2}.doc", efs.GetPath("Central Print"), letterID, CStr(results.NewRecordIdentity))

            SaveDocs(DocPathAndName.CalculatedPath, DocPathAndName.CalculatedFileName, barcodeAdditionResults.NewFileName, saveAs)
            'clean up
            Kill(barcodeAdditionResults.NewFileName)
            'add comments
            If passedInDeployMethod <> CentralizedPrintingDeploymentMethod.dmLetter Then AddComments(ri, systemToAddCommentsTo, ssn, "MLEX1", ContactType, "LT", scriptID, "mailed " & letterID & " document to legal address", procDat.UsersBuSentFromEmail, barcodeAdditionResults.AcctNum)
        ElseIf processingMethod = CentralizedPrintingDeploymentMethod.dmFax Then
            Dim faxLetterId As String
            If systemRegion = ScriptSessionBase.Region.UHEAA Then
                faxLetterId = "FAXCVRCP"
            Else
                faxLetterId = "FAXCVRFED"
            End If
            Dim saveFaxAs As String
            Dim coverSheetDocPath As DocumentPathAndName = GetDocumentPathAndFileName(ri.TestMode, faxLetterId, systemRegion, efs)
            Dim commentsAddedTo As String = String.Empty
            Dim dialableFaxNum As String
            Dim faxCoverSheetDataFile As Integer
            ''Dim coverSheetDocPath As String
            Dim newRecordNum As Long
            Dim queryResults As Barcode2DQueryResults
            Dim data As New DataAccess(ri.TestMode, ScriptSessionBase.Region.UHEAA)

            'strip out formatting if user added some
            dialableFaxNum = Replace(FrmMethod.txtFaxFaxNum.Text, "-", "")
            dialableFaxNum = Replace(dialableFaxNum, ")", "")
            dialableFaxNum = Replace(dialableFaxNum, "(", "")
            dialableFaxNum = Replace(dialableFaxNum, ".", "")
            dialableFaxNum = Replace(dialableFaxNum, " ", "")
            'create string to document which system comments are to be added to
            If systemRegion = ScriptSessionBase.Region.CornerStone Then
                commentsAddedTo = "COMPASS"
            Else
                If systemToAddCommentsTo = CentralizedPrintingSystemToAddComments.stacBoth Then
                    commentsAddedTo = "BOTH"
                ElseIf systemToAddCommentsTo = CentralizedPrintingSystemToAddComments.stacCOMPASS Then
                    commentsAddedTo = "COMPASS"
                ElseIf systemToAddCommentsTo = CentralizedPrintingSystemToAddComments.stacOneLINK Then
                    commentsAddedTo = "ONELINK"
                End If
            End If
            'create print record and get id back
            newRecordNum = CreateFaxRecordForCentralizedPrinting(ri.TestMode, dialableFaxNum, procDat.BrwDemographics.AccountNumber, procDat.UsersBusinessUnit, letterID, commentsAddedTo, systemRegion)
            If newRecordNum = 0 Then
                Throw New CentralizedPrintingException("An error occurred while trying to update and retrieve information from the database.  Please contact Systems Support.")
            End If
            barcodeAdditionResults = AddBarcodeAndStaticCurrentDateForUserProcessing(ri, dataFile, acctNumOrRefIDFieldNm, letterID, True, 0, True, Barcode2DLetterRecipient.lrBorrower, efs)
            queryResults = data.GetPaperSheetCountForBatch2D(letterID)
            'create fax cover sheet
            faxCoverSheetDataFile = FreeFile()
            FileOpen(faxCoverSheetDataFile, efs.TempFolder & "Generic Fax Coversheet " & letterID & "dat.txt", OpenMode.Output)
            WriteLine(faxCoverSheetDataFile, "BusinessUnit", "SendTo", "At", "Fax", "Pages", "Sender", "Phone", "Subject", "Message")
            WriteLine(faxCoverSheetDataFile, procDat.UsersBusinessUnit, FrmMethod.txtFaxTo.Text, FrmMethod.txtFaxAt.Text, FrmMethod.txtFaxFaxNum.Text, CStr(FigureSheetCount(queryResults) + 1), FrmMethod.txtFaxSender.Text, FrmMethod.txtFaxPhoneNum.Text, FrmMethod.txtFaxSubject.Text, FrmMethod.txtFaxMessage.Text)
            FileClose(faxCoverSheetDataFile)

            saveAs = String.Format("{0}{1}_CVR.doc", efs.GetPath("Central Fax"), CStr(newRecordNum))

            Try
                SaveDocs(coverSheetDocPath.CalculatedPath, faxLetterId, efs.TempFolder & "Generic Fax Coversheet " & letterID & "dat.txt", saveAs)

                'delete coversheet data file
                Kill(efs.TempFolder & "Generic Fax Coversheet " & letterID & "dat.txt")

                'create main document
                saveFaxAs = String.Format("{0}{1}_FAX.doc", efs.GetPath("Central Fax"), CStr(newRecordNum))

                SaveDocs(DocPathAndName.CalculatedPath, DocPathAndName.CalculatedFileName, barcodeAdditionResults.NewFileName, saveAs)
                'clean up
                Kill(barcodeAdditionResults.NewFileName)
                'add comments
                AddComments(ri, systemToAddCommentsTo, ssn, "MLEX1", ContactType, "LT", scriptID, "faxed " & letterID & " document to " & FrmMethod.txtFaxFaxNum.Text & " at " & FrmMethod.txtFaxAt.Text, procDat.UsersBusinessUnit, barcodeAdditionResults.AcctNum)
            Catch ex As Exception
                Throw New CentralizedPrintingException("The letter you requested was not generated.  Please re-run the script and re-send the letter.", ex)
            End Try
        End If

        If processOffT Then
            File.Delete(String.Format("{0}{1}", efs.TempFolder, DocPathAndName.CalculatedFileName)) 'Delete the Word doc that was copied to T
        End If
        Return saveAs
    End Function

    ''' <summary>
    ''' Overload to support older code which does not specify the system region.  Passes UHEAA as the region to the main subroutine.  Main entry point that calls all helper functions as needed to accomplish Centralized printing, adding the static current date and adding the 2D barcode.  FOR STANDALONE APPLICATIONS ONLY.
    ''' </summary>
    Public Shared Sub GiveMeItAll_Without_ReflectionInterface(ByVal recipientDemographics As SystemBorrowerDemographics, ByVal letterID As String, ByVal dataFile As String, ByVal acctNumOrRefIDFieldNm As String, ByVal stateCodeFieldNm As String, ByVal passedInDeployMethod As CentralizedPrintingDeploymentMethod, ByVal LetterRecip As Barcode2DLetterRecipient, ByVal testMode As Boolean, ByVal documentCopyPathAndFilename As String)
        GiveMeItAll_Without_ReflectionInterface(recipientDemographics, letterID, dataFile, acctNumOrRefIDFieldNm, stateCodeFieldNm, passedInDeployMethod, LetterRecip, testMode, documentCopyPathAndFilename, ScriptSessionBase.Region.UHEAA)
    End Sub

    ''' <summary>
    ''' Main entry point that calls all helper functions as needed to accomplish Centralized printing, adding the static current date and adding the 2D barcode.  FOR STANDALONE APPLICATIONS ONLY.
    ''' </summary>
    ''' <param name="recipientDemographics">BorrowerDemographics object</param>
    ''' <param name="letterID">Letter ID from Letter Tracking</param>
    ''' <param name="dataFile">The data file for the merge operation (Must have one data row)</param>
    ''' <param name="acctNumOrRefIDFieldNm">Field name for the Account Number (for borrower) or Reference ID (for reference)</param>
    ''' <param name="stateCodeFieldNm">Field name for the state code field</param>
    ''' <param name="passedInDeployMethod">Desired deployment method.  If anything other than User Prompt is passsed then the user is required to use that method else the user will be given a prompt to choose a method and provide the needed information for the chosen method.</param>
    ''' <param name="LetterRecip">The recipient of the letter.  If not specified in the spec "borrower" should be used (was default before moving to C#).</param>
    ''' <param name="testMode">Test mode</param>
    ''' <param name="documentCopyPathAndFilename">The filename (including full path) to use to save a copy of the document that is being printed if the system needs a copy put in a document repository for the account.  Pass blank if no copy needs to be saved.</param>
    ''' <remarks>This method expects the data file that is sent to it to have a single data row in it</remarks>
    ''' Public Shared Sub GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(ByVal ri As ReflectionInterface, ByVal letterID As String, ByVal systemToAddCommentsTo As CentralizedPrintingSystemToAddComments, ByVal ssn As String, ByVal dataFile As String, ByVal acctNumOrRefIDFieldNm As String, ByVal docName As String, ByVal docPath As String, ByVal userID As String, ByVal scriptID As String, ByVal stateCodeFieldNm As String, ByVal passedInDeployMethod As CentralizedPrintingDeploymentMethod, ByVal ContactType As String, ByVal LetterRecip As Barcode2DLetterRecipient)
    Public Shared Function GiveMeItAll_Without_ReflectionInterface(ByVal recipientDemographics As SystemBorrowerDemographics, ByVal letterID As String, ByVal dataFile As String, ByVal acctNumOrRefIDFieldNm As String, ByVal stateCodeFieldNm As String, ByVal passedInDeployMethod As CentralizedPrintingDeploymentMethod, ByVal LetterRecip As Barcode2DLetterRecipient, ByVal testMode As Boolean, ByVal documentCopyPathAndFilename As String, ByVal systemRegion As ScriptBase.Region) As String
        Dim processingMethod As CentralizedPrintingDeploymentMethod
        Dim saveAs As String = ""
        Dim efs As New EnterpriseFileSystem(testMode, systemRegion)
        Dim barcodeAdditionResults As UserBarcodeAdditionResults
        Dim procDat As CentralizedPrintingAnd2DBarcodeInfo = GetUserInfoForCentralizedPrintAnd2DObject(testMode)
        procDat.BrwDemographics = recipientDemographics
        Dim FrmMethod As New LetterDeliveryMethod(testMode, procDat, passedInDeployMethod)
        Dim DocPathAndName As DocumentPathAndName = GetDocumentPathAndFileName(testMode, letterID, systemRegion, efs)

        'get delivery method information if needed
        If passedInDeployMethod <> CentralizedPrintingDeploymentMethod.dmLetter Then
            'if anything other than letter is the default then get needed information
            If FrmMethod.ShowDialog() = DialogResult.Cancel Then
                Common.EndDLLScript()
            End If
            processingMethod = FrmMethod.SelectedDeploymentMethod
        Else
            'if letter is default then no other information is needed
            processingMethod = CentralizedPrintingDeploymentMethod.dmLetter
        End If

        'deliver the document by e-mail
        If processingMethod = CentralizedPrintingDeploymentMethod.dmEmail Then
            Dim FN As String
            Dim EmailTo As String
            Dim fileNameDateTimeStamp As String = Now.ToString("MMddyyyyhhmmss")
            saveAs = String.Format("{0}DOCGENRSCRPT_{1}_{2}.doc", efs.TempFolder, letterID, fileNameDateTimeStamp)

            'update email address so calling system can update itself if it wants to
            procDat.BrwDemographics.Email = FrmMethod.txtEmailTo.Text

            'create barcode fields in data file
            barcodeAdditionResults = AddBarcodeAndStaticCurrentDateForUserProcessing(testMode, dataFile, acctNumOrRefIDFieldNm, letterID, True, 0, True, Barcode2DLetterRecipient.lrBorrower, efs)
            Try
                SaveDocs(DocPathAndName.CalculatedPath, DocPathAndName.CalculatedFileName, barcodeAdditionResults.NewFileName, saveAs)
                'save a copy in the system document repository
                If documentCopyPathAndFilename.Length <> 0 Then
                    File.Copy(saveAs, documentCopyPathAndFilename)
                End If

                If testMode Then
                    EmailTo = Environ("USERNAME") & "@utahsbr.edu"
                Else
                    EmailTo = FrmMethod.txtEmailTo.Text
                End If
                Common.SendMail(testMode, EmailTo, procDat.UsersBuSentFromEmail, FrmMethod.txtEmailSubject.Text, FrmMethod.txtEmailMessage.Text, "", "", saveAs, OSSMTP.importance_level.ImportanceNormal, True)
                'clean up
                Kill(barcodeAdditionResults.NewFileName)
                'delete files older that 30 day from local PC
                FN = Dir(efs.TempFolder & "DOCGENRSCRPT_*")
                While FN <> ""
                    If FileDateTime(efs.TempFolder & FN) < DateAdd("D", -30, Now) Then Kill(efs.TempFolder & FN)
                    FN = Dir()
                End While
            Catch ex As Exception
                Throw New CentralizedPrintingException("The letter you requested was not generated.  Please re-run the process and re-send the letter.  Please see inner exception for more information.", ex)
            End Try

            'deliver the document by printed letter
        ElseIf processingMethod = CentralizedPrintingDeploymentMethod.dmLetter Then
            Dim results As LetterRecordCreationResults
            results = CreateLetterRecordForCentralizedPrinting(testMode, letterID, procDat.BrwDemographics.AccountNumber, procDat.UsersBusinessUnit, GetStateCode(dataFile, stateCodeFieldNm), systemRegion)
            barcodeAdditionResults = AddBarcodeAndStaticCurrentDateForUserProcessing(testMode, dataFile, acctNumOrRefIDFieldNm, letterID, True, results.BarcodeSeqNum, True, LetterRecip, efs)

            saveAs = String.Format("{0}{1}_{2}.doc", efs.GetPath("Central Print"), letterID, CStr(results.NewRecordIdentity))

            SaveDocs(DocPathAndName.CalculatedPath, DocPathAndName.CalculatedFileName, barcodeAdditionResults.NewFileName, saveAs)
            'save a copy in the system document repository
            If documentCopyPathAndFilename.Length <> 0 Then
                File.Copy(saveAs, documentCopyPathAndFilename)
            End If

            'clean up
            Kill(barcodeAdditionResults.NewFileName)

            'deliver the document by fax
        ElseIf processingMethod = CentralizedPrintingDeploymentMethod.dmFax Then
            Dim faxLetterId As String
            If systemRegion = ScriptSessionBase.Region.UHEAA Then
                faxLetterId = "FAXCVRCP"
            Else
                faxLetterId = "FAXCVRFED"
            End If
            Dim saveFaxAs As String
            Dim coverSheetDocPath As DocumentPathAndName = GetDocumentPathAndFileName(testMode, faxLetterId, systemRegion, efs)
            Dim commentsAddedTo As String = String.Empty
            Dim dialableFaxNum As String
            Dim faxCoverSheetDataFile As Integer
            ''Dim coverSheetDocPath As String
            Dim newRecordNum As Long
            Dim queryResults As Barcode2DQueryResults
            Dim data As New DataAccess(testMode, ScriptSessionBase.Region.UHEAA)

            'strip out formatting if user added some
            dialableFaxNum = Replace(FrmMethod.txtFaxFaxNum.Text, "-", "")
            dialableFaxNum = Replace(dialableFaxNum, ")", "")
            dialableFaxNum = Replace(dialableFaxNum, "(", "")
            dialableFaxNum = Replace(dialableFaxNum, ".", "")
            dialableFaxNum = Replace(dialableFaxNum, " ", "")

            'create print record and get id back
            newRecordNum = CreateFaxRecordForCentralizedPrinting(testMode, dialableFaxNum, procDat.BrwDemographics.AccountNumber, procDat.UsersBusinessUnit, letterID, commentsAddedTo, systemRegion)
            If newRecordNum = 0 Then
                Throw New CentralizedPrintingException("An error occurred while trying to update and retrieve information from the database.  Please contact Systems Support.")
            End If

            barcodeAdditionResults = AddBarcodeAndStaticCurrentDateForUserProcessing(testMode, dataFile, acctNumOrRefIDFieldNm, letterID, True, 0, True, Barcode2DLetterRecipient.lrBorrower, efs)
            queryResults = data.GetPaperSheetCountForBatch2D(letterID)

            'create fax cover sheet
            faxCoverSheetDataFile = FreeFile()
            FileOpen(faxCoverSheetDataFile, efs.TempFolder & "Generic Fax Coversheet " & letterID & "dat.txt", OpenMode.Output)
            WriteLine(faxCoverSheetDataFile, "BusinessUnit", "SendTo", "At", "Fax", "Pages", "Sender", "Phone", "Subject", "Message")
            WriteLine(faxCoverSheetDataFile, procDat.UsersBusinessUnit, FrmMethod.txtFaxTo.Text, FrmMethod.txtFaxAt.Text, FrmMethod.txtFaxFaxNum.Text, CStr(FigureSheetCount(queryResults) + 1), FrmMethod.txtFaxSender.Text, FrmMethod.txtFaxPhoneNum.Text, FrmMethod.txtFaxSubject.Text, FrmMethod.txtFaxMessage.Text)
            FileClose(faxCoverSheetDataFile)

            saveAs = String.Format("{0}{1}_CVR.doc", efs.GetPath("Central Fax"), CStr(newRecordNum))

            Try
                SaveDocs(coverSheetDocPath.CalculatedPath, faxLetterId, efs.TempFolder & "Generic Fax Coversheet " & letterID & "dat.txt", saveAs)
                'delete coversheet data file
                Kill(efs.TempFolder & "Generic Fax Coversheet " & letterID & "dat.txt")
                'create main document
                saveFaxAs = String.Format("{0}{1}_FAX.doc", efs.GetPath("Central Fax"), CStr(newRecordNum))

                SaveDocs(DocPathAndName.CalculatedPath, DocPathAndName.CalculatedFileName, barcodeAdditionResults.NewFileName, saveAs)

                'save a copy in the system document repository
                If documentCopyPathAndFilename.Length <> 0 Then
                    File.Copy(saveAs, documentCopyPathAndFilename)
                End If

                'clean up
                Kill(barcodeAdditionResults.NewFileName)
                'add comments
            Catch ex As Exception
                Throw New CentralizedPrintingException("The letter you requested was not generated.  Please re-run the process and re-send the letter.", ex)
            End Try
        End If
        Return saveAs
    End Function

    'gets state code from file
    Private Shared Function GetStateCode(ByVal DataFile As String, ByVal StateCodeFieldName As String) As String
        Dim Handle As Integer
        Dim Rec As String
        Dim SplitRec() As String
        Dim I As Integer
        Dim II As Integer
        GetStateCode = ""
        Handle = FreeFile()
        FileOpen(Handle, DataFile, OpenMode.Input)
        Rec = LineInput(Handle)
        Rec = Replace(Rec, """", "")
        SplitRec = Split(Rec, ",")
        While SplitRec(I) <> StateCodeFieldName
            I = I + 1
        End While
        While II <> I
            Input(Handle, GetStateCode)
            II = II + 1
        End While
        Input(Handle, GetStateCode)
        'search for statecode field
        FileClose(Handle)
    End Function

    'adds comments to systems for centralized printing
    Private Shared Sub AddComments(ByVal ri As ReflectionInterface, ByVal systemToAddCommentsTo As CentralizedPrintingSystemToAddComments, ByVal ssn As String, ByVal actionCode As String, ByVal contactType As String, ByVal activityType As String, ByVal scriptID As String, ByVal comment As String, ByVal buEmailAddr As String, ByVal acctNum As String)
        'check if comments should be added to OneLINK
        If systemToAddCommentsTo = CentralizedPrintingSystemToAddComments.stacBoth OrElse systemToAddCommentsTo = CentralizedPrintingSystemToAddComments.stacOneLINK Then
            'add comments to OneLINK
            If Common.AddCommentInLP50(ri.ReflectionSession, ssn, actionCode, scriptID, activityType, contactType, comment) = False Then
                MsgBox("There was an error while trying to add a comment to LP50.")
            End If
        End If
        'check if commnets should be added to COMPASS
        If systemToAddCommentsTo = CentralizedPrintingSystemToAddComments.stacBoth OrElse systemToAddCommentsTo = CentralizedPrintingSystemToAddComments.stacCOMPASS Then
            'add comments to COMPASS
            If Common.ATD22AllLoans(ri.ReflectionSession, ssn, actionCode, comment, scriptID) = False Then
                'if comment couldn't be added to TD22 then try and add it to TD37
                If Common.ATD37FirstLoan(ri.ReflectionSession, ssn, actionCode, comment, scriptID) = False Then
                    MsgBox("There was an error while trying to add a comment to COMPASS (TD22 and TD37).  An error email has been sent to your business unit's email address and to Systems Support.")
                    Common.SendMail(ri.TestMode, buEmailAddr & "," & GetBsysMiscEmailNotifRecipients("DocumentGenerationCommentErr", ri.TestMode), "Document Generation Script", "Document Generation Script Error Report", "Acct Num:" & acctNum & " Action Code/ARC that failed to add: " & actionCode & vbCrLf & vbCrLf & "The script attempted to add this comment to all loans on TD22 and the first loan listed on TD37 and failed to add.", "", "", "", OSSMTP.importance_level.ImportanceNormal, True)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' This function can be used to handle special situations where GiveMeItAll_LtrEmlFax_BarCd_StaCurDt doesn't handle a special situation (example: letter needs 2D barcode but doesn't need to be centralized) and the Enterprise File Systems object is not provided (older scripts).  FOR USER SCRIPTS ONLY.
    ''' </summary>
    ''' <param name="ri">Reflection interface</param>
    ''' <param name="dataFile">Data file to be used in the merge process</param>
    ''' <param name="acctNumOrRefIdHeader">Field name for the Account Number (for borrower) or Reference ID (for reference)</param>
    ''' <param name="letterID">Letter ID from Letter Tracking</param>
    ''' <param name="createNewFile">Do you want the function to create a new file for you and return the file name (true or false)</param>
    ''' <param name="batchDocNum">Used by Centralized printing (default was 0 before C#).  GiveMeItAll_LtrEmlFax_BarCd_StaCurDt calulates for you if needed.  If you are calling this outside using GiveMeItAll_LtrEmlFax_BarCd_StaCurDt (it sucks to be you) see GiveMeItAll_LtrEmlFax_BarCd_StaCurDt for details of how to calculate this.</param>
    ''' <param name="addStaticCurrentDate">Do you want the method to add the static current date to the file (defaulted to 0 before C#)</param>
    ''' <param name="letterRecipient">Who is the letter recipient (defaulted to Borrower before C#)</param>
    ''' <returns>A populated instance of UserBarcodeAdditionResults</returns>
    ''' <remarks></remarks>
    Public Shared Function AddBarcodeAndStaticCurrentDateForUserProcessing(ByVal ri As ReflectionInterface, ByVal dataFile As String, ByVal acctNumOrRefIdHeader As String, ByVal letterId As String, ByVal createNewFile As Boolean, ByVal batchDocNum As Long, ByVal addStaticCurrentDate As Boolean, ByVal letterRecipient As Barcode2DLetterRecipient) As UserBarcodeAdditionResults
        Dim efs As New EnterpriseFileSystem(ri.TestMode, ScriptSessionBase.Region.UHEAA)
        Return AddBarcodeAndStaticCurrentDateForUserProcessing(ri.TestMode, dataFile, acctNumOrRefIdHeader, letterId, createNewFile, batchDocNum, addStaticCurrentDate, letterRecipient, efs)
    End Function

    ''' <summary>
    ''' This function can be used to handle special situations where GiveMeItAll_LtrEmlFax_BarCd_StaCurDt doesn't handle a special situation (example: letter needs 2D barcode but doesn't need to be centralized) and the Enterprise File Systems object is provided.  FOR USER SCRIPTS ONLY.
    ''' </summary>
    ''' <param name="ri">Reflection interface</param>
    ''' <param name="dataFile">Data file to be used in the merge process</param>
    ''' <param name="acctNumOrRefIdHeader">Field name for the Account Number (for borrower) or Reference ID (for reference)</param>
    ''' <param name="letterID">Letter ID from Letter Tracking</param>
    ''' <param name="createNewFile">Do you want the function to create a new file for you and return the file name (true or false)</param>
    ''' <param name="batchDocNum">Used by Centralized printing (default was 0 before C#).  GiveMeItAll_LtrEmlFax_BarCd_StaCurDt calulates for you if needed.  If you are calling this outside using GiveMeItAll_LtrEmlFax_BarCd_StaCurDt (it sucks to be you) see GiveMeItAll_LtrEmlFax_BarCd_StaCurDt for details of how to calculate this.</param>
    ''' <param name="addStaticCurrentDate">Do you want the method to add the static current date to the file (defaulted to 0 before C#)</param>
    ''' <param name="letterRecipient">Who is the letter recipient (defaulted to Borrower before C#)</param>
    ''' <param name="efs"></param>
    ''' <returns>A populated instance of UserBarcodeAdditionResults</returns>
    ''' <remarks></remarks>
    Public Shared Function AddBarcodeAndStaticCurrentDateForUserProcessing(ByVal ri As ReflectionInterface, ByVal dataFile As String, ByVal acctNumOrRefIdHeader As String, ByVal letterId As String, ByVal createNewFile As Boolean, ByVal batchDocNum As Long, ByVal addStaticCurrentDate As Boolean, ByVal letterRecipient As Barcode2DLetterRecipient, ByVal efs As EnterpriseFileSystem) As UserBarcodeAdditionResults
        Return AddBarcodeAndStaticCurrentDateForUserProcessing(ri.TestMode, dataFile, acctNumOrRefIdHeader, letterId, createNewFile, batchDocNum, addStaticCurrentDate, letterRecipient, efs)
    End Function

    ''' <summary>
    ''' This function can be used to handle special situations where GiveMeItAll_LtrEmlFax_BarCd_StaCurDt doesn't handle a special situation (example: letter needs 2D barcode but doesn't need to be centralized) and the Enterprise File Systems object is not provided (older scripts).  FOR USER SCRIPTS ONLY.
    ''' </summary>
    ''' <param name="testMode">Test mode</param>
    ''' <param name="dataFile">Data file to be used in the merge process</param>
    ''' <param name="acctNumOrRefIdHeader">Field name for the Account Number (for borrower) or Reference ID (for reference)</param>
    ''' <param name="letterID">Letter ID from Letter Tracking</param>
    ''' <param name="createNewFile">Do you want the function to create a new file for you and return the file name (true or false)</param>
    ''' <param name="batchDocNum">Used by Centralized printing (default was 0 before C#).  GiveMeItAll_LtrEmlFax_BarCd_StaCurDt calulates for you if needed.  If you are calling this outside using GiveMeItAll_LtrEmlFax_BarCd_StaCurDt (it sucks to be you) see GiveMeItAll_LtrEmlFax_BarCd_StaCurDt for details of how to calculate this.</param>
    ''' <param name="addStaticCurrentDate">Do you want the method to add the static current date to the file (defaulted to 0 before C#)</param>
    ''' <param name="letterRecipient">Who is the letter recipient (defaulted to Borrower before C#)</param>
    ''' <returns>A populated instance of UserBarcodeAdditionResults</returns>
    ''' <remarks></remarks>
    Public Shared Function AddBarcodeAndStaticCurrentDateForUserProcessing(ByVal testMode As Boolean, ByVal dataFile As String, ByVal acctNumOrRefIdHeader As String, ByVal letterId As String, ByVal createNewFile As Boolean, ByVal batchDocNum As Long, ByVal addStaticCurrentDate As Boolean, ByVal letterRecipient As Barcode2DLetterRecipient) As UserBarcodeAdditionResults
        Dim efs As New EnterpriseFileSystem(testMode, ScriptSessionBase.Region.UHEAA)
        Return AddBarcodeAndStaticCurrentDateForUserProcessing(testMode, dataFile, acctNumOrRefIdHeader, letterId, createNewFile, batchDocNum, addStaticCurrentDate, letterRecipient, efs)
    End Function

    ''' <summary>
    ''' This function can be used to handle special situations where GiveMeItAll_LtrEmlFax_BarCd_StaCurDt doesn't handle a special situation (example: letter needs 2D barcode but doesn't need to be centralized) and the Enterprise File Systems object is provided.  FOR USER SCRIPTS ONLY.
    ''' </summary>
    ''' <param name="testMode">Test mode</param>
    ''' <param name="dataFile">Data file to be used in the merge process</param>
    ''' <param name="acctNumOrRefIdHeader">Field name for the Account Number (for borrower) or Reference ID (for reference)</param>
    ''' <param name="letterID">Letter ID from Letter Tracking</param>
    ''' <param name="createNewFile">Do you want the function to create a new file for you and return the file name (true or false)</param>
    ''' <param name="batchDocNum">Used by Centralized printing (default was 0 before C#).  GiveMeItAll_LtrEmlFax_BarCd_StaCurDt calulates for you if needed.  If you are calling this outside using GiveMeItAll_LtrEmlFax_BarCd_StaCurDt (it sucks to be you) see GiveMeItAll_LtrEmlFax_BarCd_StaCurDt for details of how to calculate this.</param>
    ''' <param name="addStaticCurrentDate">Do you want the method to add the static current date to the file (defaulted to 0 before C#)</param>
    ''' <param name="letterRecipient">Who is the letter recipient (defaulted to Borrower before C#)</param>
    ''' <returns>A populated instance of UserBarcodeAdditionResults</returns>
    ''' <remarks></remarks>
    Public Shared Function AddBarcodeAndStaticCurrentDateForUserProcessing(ByVal testMode As Boolean, ByVal dataFile As String, ByVal acctNumOrRefIdHeader As String, ByVal letterId As String, ByVal createNewFile As Boolean, ByVal batchDocNum As Long, ByVal addStaticCurrentDate As Boolean, ByVal letterRecipient As Barcode2DLetterRecipient, ByVal efs As EnterpriseFileSystem) As UserBarcodeAdditionResults
        'Set up a return value and a file name for the barcoded data file.
        Dim results As New UserBarcodeAdditionResults()
        Dim barcodedFile As String = String.Format("{0}Add Return Mail Barcode Temp {1}.txt", efs.TempFolder, letterId)

        'Query Letter Tracking to get paper sheet count.
        Dim data As New DataAccess(testMode, ScriptSessionBase.Region.UHEAA)
        Dim queryResults As Barcode2DQueryResults = data.GetPaperSheetCountForBatch2D(letterId)
        If (queryResults Is Nothing) Then Throw New BarcodeException("The letter ID that the script is using doesn't appear to exist.  Please contact a member of Systems Support")
        Dim pageCount As Integer = FigureSheetCount(queryResults)
        If (pageCount < 1 OrElse pageCount > 7) Then Throw New BarcodeException("The paper sheet count for the letter ID that the script is using isn't populated.  Please contact a member of Systems Support")

        Using originalDataReader As New StreamReader(dataFile)
            'Read in the header row.
            Dim originalFields As List(Of String) = originalDataReader.ReadLine().SplitAgnosticOfQuotes(",")
            Dim acctNumIndex As Integer = 0
            If (letterRecipient <> Barcode2DLetterRecipient.lrOther) Then acctNumIndex = originalFields.IndexOf(acctNumOrRefIdHeader)

            'Define the new header row.
            Dim barcodedFields As New List(Of String)()
            If (letterRecipient <> Barcode2DLetterRecipient.lrOther) Then barcodedFields.AddRange(New String() {"BC1", "BC2", "BC3", "BC4", "BC5", "BC6"})
            barcodedFields.AddRange(New String() {"SMBC1", "SMBC2", "SMBC3", "SMBC4"})
            If (pageCount > 1) Then barcodedFields.AddRange(New String() {"SMBC_Pg2_Ln1", "SMBC_Pg2_Ln2", "SMBC_Pg2_Ln3", "SMBC_Pg2_Ln4"})
            If (pageCount > 2) Then barcodedFields.AddRange(New String() {"SMBC_Pg3_Ln1", "SMBC_Pg3_Ln2", "SMBC_Pg3_Ln3", "SMBC_Pg3_Ln4"})
            If (pageCount > 3) Then barcodedFields.AddRange(New String() {"SMBC_Pg4_Ln1", "SMBC_Pg4_Ln2", "SMBC_Pg4_Ln3", "SMBC_Pg4_Ln4"})
            If (pageCount > 4) Then barcodedFields.AddRange(New String() {"SMBC_Pg5_Ln1", "SMBC_Pg5_Ln2", "SMBC_Pg5_Ln3", "SMBC_Pg5_Ln4"})
            If (pageCount > 5) Then barcodedFields.AddRange(New String() {"SMBC_Pg6_Ln1", "SMBC_Pg6_Ln2", "SMBC_Pg6_Ln3", "SMBC_Pg6_Ln4"})
            If (pageCount > 6) Then barcodedFields.AddRange(New String() {"SMBC_Pg7_Ln1", "SMBC_Pg7_Ln2", "SMBC_Pg7_Ln3", "SMBC_Pg7_Ln4"})
            If (addStaticCurrentDate) Then barcodedFields.Add("StaticCurrentDate")
            barcodedFields.AddRange(originalFields)

            Using barcodedDataFile As New StreamWriter(barcodedFile, False)
                'Write the new header row to the file.
                barcodedDataFile.WriteCommaDelimitedLine(barcodedFields.ToArray())

                'Loop through the remaining records.
                Dim BARCODE_DELIMITER As Char() = {vbCr, vbLf}
                Dim dataFileLineNumber As Long = 0
                While Not originalDataReader.EndOfStream
                    If (batchDocNum = 0) Then
                        dataFileLineNumber += 1
                    Else
                        dataFileLineNumber = batchDocNum
                    End If
                    'Read in the next record and split it into fields.
                    originalFields = originalDataReader.ReadLine().SplitAgnosticOfQuotes(",")
                    Dim accountNumber As String = originalFields(acctNumIndex).Replace(" ", "")
                    results.AcctNum = accountNumber
                    If (letterRecipient = Barcode2DLetterRecipient.lrReference) Then accountNumber += " "
                    barcodedFields = New List(Of String)()
                    If (letterRecipient <> Barcode2DLetterRecipient.lrOther) Then
                        'Add return mail barcode data.
                        For Each returnMailBarcodeField As String In EncNDM(String.Format("{0}{1}{2:MMddyyyy}", accountNumber, letterId.PadLeft(10), DateTime.Now)).Split(BARCODE_DELIMITER).Where(Function(p) Not String.IsNullOrEmpty(p))
                            barcodedFields.Add(returnMailBarcodeField)
                        Next returnMailBarcodeField
                    End If
                    'Add state mail barcode data.
                    For pageNumber As Integer = 1 To pageCount
                        Dim leadingDigit As Integer = If(pageNumber = 1, 1, 0)
                        For Each stateMailBarcodeField As String In EncNDM(String.Format("{0}{1}0{2}{3:00000#}", leadingDigit, pageCount, pageNumber, dataFileLineNumber)).Split(BARCODE_DELIMITER).Where(Function(p) Not String.IsNullOrEmpty(p))
                            barcodedFields.Add(stateMailBarcodeField)
                        Next stateMailBarcodeField
                    Next pageNumber
                    If (addStaticCurrentDate) Then barcodedFields.Add(DateTime.Now.ToString("MMMM dd, yyyy"))
                    'Append the original data fields and write out to the file.
                    barcodedFields.AddRange(originalFields)
                    barcodedDataFile.WriteCommaDelimitedLine(barcodedFields.ToArray())
                End While
                barcodedDataFile.Close()
            End Using
            originalDataReader.Close()
        End Using

        If (createNewFile) Then
            results.NewFileName = barcodedFile
        Else
            'Delete the original data file.
            Common.KillUntilDead(dataFile)
            'Rename the new data file with the original file's name.
            File.Move(barcodedFile, dataFile)
            results.NewFileName = dataFile
        End If

        Return results
    End Function
#End Region
#End Region

#Region "Fax"
    Private Const FAX_SERVER_NAME As String = "IMAGING-FAX"
    Private Const FAX_PROTOCOL As CommunicationProtocolType = CommunicationProtocolType.cpTCPIP
    Private Const FAX_USE_NT_AUTHENTICATION As BoolType = BoolType.True
    Private Const FAX_UHEAA_NUMBER As String = "98013217198"
    Private Const FAX_HAS_RIGHTFAX_COVERSHEET As BoolType = BoolType.False

    ''' <summary>
    ''' Sends a fax to the given fax number, or to UHEAA's fax number if running in test mode.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="faxNumber">The fax number of the intended recipient.</param>
    ''' <param name="recipientName">The recipient name(s) to appear on the remote fax machine.</param>
    ''' <param name="pages">
    ''' A list of file names for the documents to be faxed.
    ''' If sending form letters, perform the merge first and list the finished document names here.
    ''' </param>
    Public Shared Sub SendFax(ByVal testMode As Boolean, ByVal faxNumber As String, ByVal recipientName As String, ByVal pages As List(Of String))
        'Connect to the fax server.
        Dim faxServer As New FaxServer()
        faxServer.ServerName = FAX_SERVER_NAME
        faxServer.Protocol = FAX_PROTOCOL
        faxServer.UseNTAuthentication = FAX_USE_NT_AUTHENTICATION
        faxServer.AuthorizationUserID = Environment.UserName
        faxServer.OpenServer()

        'Create a Fax object and set its properties.
        Dim newFax As Fax = CType(faxServer.CreateObject(CreateObjectType.coFax), Fax)
        newFax.ToFaxNumber = faxNumber
        newFax.ToName = recipientName
        newFax.HasCoversheet = FAX_HAS_RIGHTFAX_COVERSHEET
        For Each page As String In pages
            newFax.Attachments.Add(page)
        Next

        'Make any adjustments necessary.
        If testMode Then newFax.ToFaxNumber = FAX_UHEAA_NUMBER
        If Not newFax.ToFaxNumber.StartsWith("9") Then newFax.ToFaxNumber = "9" + newFax.ToFaxNumber

        'Send the fax and disconnect from the server.
        newFax.Send()
        faxServer.CloseServer()
    End Sub
#End Region 'Fax

End Class
