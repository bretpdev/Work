Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Word
Imports System.IO

Public Class ImageDocumentProcessor
    Implements IDisposable

    'passed in values
    Private _indexOfSsn As Integer
    Private _docID As String
    Private _docPath As String
    Private _doc As String
    Private _dataFile As String
    Private _testMode As Boolean

    'inner working values
    Private _archLib As String
    Private _word As Word.Application
    Private _currentFileLine As Long
    Private _fileHandle As Integer
    Private _componentsInitialized As Boolean
    Private _endOfFileAlreadyFound As Boolean

    'recovery values
    Private _recoveryFileHandle As Integer
    Private _recoveryFilePathAndName As String
    Private _recoveryPointFound As Boolean
    Private _doInternalRecovery As Boolean

    'properties

    Private _imageCount As Long
    ''' <summary>
    ''' Number of documents imaged.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ImageCount() As Long
        Get
            Return _imageCount
        End Get
        Set(ByVal value As Long)
            _imageCount = value
        End Set
    End Property


    Public Sub New(ByVal indexOfSsn As Integer, ByVal docId As String, ByVal docPath As String, ByVal doc As String, ByVal dataFile As String, ByVal testMode As Boolean)
        _indexOfSsn = indexOfSsn
        _docID = docId
        _docPath = docPath
        _doc = doc
        _dataFile = dataFile
        _testMode = testMode
        _componentsInitialized = False
        _endOfFileAlreadyFound = False
        _recoveryFilePathAndName = String.Format("{0}{1} Imaging Log.txt", Common.TestMode(String.Empty, testMode).LogFolder, _dataFile.Split("\").Last())
        _recoveryPointFound = False
    End Sub

    Public Sub New(ByVal ssnFieldNm As String, ByVal docId As String, ByVal docPath As String, ByVal doc As String, ByVal dataFile As String, ByVal testMode As Boolean)
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

        _indexOfSsn = fieldCount
        _docID = docId
        _docPath = docPath
        _doc = doc
        _dataFile = dataFile
        _testMode = testMode
        _componentsInitialized = False
        _endOfFileAlreadyFound = False
        _recoveryFilePathAndName = String.Format("{0}{1} Imaging Log.txt", Common.TestMode(String.Empty, testMode).LogFolder, _dataFile.Split("\").Last())
        _recoveryPointFound = False
    End Sub

#Region " API "

    ''' <summary>
    ''' Image the entire file.  Handles file level recovery for the calling script.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ProcessEntireFile()
        _doInternalRecovery = True
        Dim rSSN As String = String.Empty
        'figure out recovery values
        If (File.Exists(_recoveryFilePathAndName)) Then
            _recoveryFileHandle = FreeFile()
            FileOpen(_recoveryFileHandle, _recoveryFilePathAndName, OpenMode.Input)
            Input(_recoveryFileHandle, rSSN)
            FileClose(_recoveryFileHandle)
        End If
        'do processing
        If _componentsInitialized = False Then InitializeComponents()
        Dim ssn As String = ImageRecord(rSSN)
        If String.IsNullOrEmpty(ssn) AndAlso _endOfFileAlreadyFound Then
            Throw New EntireFileImagedException("The file has already been imaged fully.  Please contact a member of systems support for assistance.")
        Else
            While (ssn.Length > 0)
                ssn = ImageRecord(rSSN)
            End While
        End If
    End Sub

    ''' <summary>
    ''' Processes a single record and returns the ssn from it.  An empty string is returned when the end of file is encountered.  Requires custom recovery to be built into the calling script.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessSingleRecordFromFile() As String
        _doInternalRecovery = False
        If _componentsInitialized = False Then InitializeComponents()
        If _endOfFileAlreadyFound Then
            Throw New EntireFileImagedException("The file has already been imaged fully.  Please contact a member of systems support for assistance.")
        End If
        Return ImageRecord()
    End Function

#End Region

#Region " Inner Workings "

    ''' <summary>
    ''' Initialzes components for image processing
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeComponents()
        'set imaging and printing parameters
        _archLib = "CR"
        If _testMode Then
            _archLib = "Test"
        End If

        _fileHandle = FreeFile()


        FileOpen(_fileHandle, _dataFile, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
        'Read in header row
        LineInput(_fileHandle)


        'set up Word object
        _word = New Word.Application()
        'Word.Visible = False
        'open form doc
        _word.Documents.Open(FileName:=_docPath & _doc & ".doc", ConfirmConversions:=False, _
            ReadOnly:=False, AddToRecentFiles:=False, PasswordDocument:="", _
            PasswordTemplate:="", Revert:=False, WritePasswordDocument:="", _
            WritePasswordTemplate:="", Format:=WdOpenFormat.wdOpenFormatAuto)
        _word.ActiveDocument.MailMerge.OpenDataSource(Name:=_dataFile, _
            ConfirmConversions:=False, ReadOnly:=False, LinkToSource:=True, _
            AddToRecentFiles:=False, PasswordDocument:="", PasswordTemplate:="", _
            WritePasswordDocument:="", WritePasswordTemplate:="", Revert:=False, _
            Format:=WdOpenFormat.wdOpenFormatAuto, Connection:="", SQLStatement:="", SQLStatement1 _
            :="", SubType:=WdMergeSubType.wdMergeSubTypeOther)
        _word.ActiveDocument.SaveAs(FileName:=DataAccessBase.PersonalDataDirectory & _doc & ".doc", _
            FileFormat:=WdSaveFormat.wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
            True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
            False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
            SaveAsAOCELetter:=False)
        'merge one file for each record in the merge data file
        _currentFileLine = 1

        'used to ensure that the components are only initalized once
        _componentsInitialized = True
    End Sub

    ''' <summary>
    ''' Images record from initialized data file and returns the ssn from the record it processed.  Returns empty string if a record to process wasn't found. 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ImageRecord() As String
        Return ImageRecord(String.Empty)
    End Function

    ''' <summary>
    ''' Images record from initialized data file and returns the ssn from the record it processed.  Returns empty string if a record to process wasn't found.  If trying to recover pass in the ssn and the process will skip each record until the record with the ssn is encountered (it will then process from there on out).
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ImageRecord(ByVal lastSSNSuccessfullyProcessed As String) As String
        Dim ssn As String

        'check if it is the end of the file
        If EOF(_fileHandle) Then
            ssn = String.Empty
            _endOfFileAlreadyFound = True
        Else
            'read in row
            Dim fileLine As String = LineInput(1)
            Dim fields() As String = fileLine.SplitAgnosticOfQuotes(",").ToArray
            ssn = fields(_indexOfSsn)

            'check to see if the record should be skipped because of recovery
            If (lastSSNSuccessfullyProcessed.Length > 0 AndAlso _recoveryPointFound = False) Then
                If ssn = lastSSNSuccessfullyProcessed Then
                    _recoveryPointFound = True 'recovery point was found and all other records can be processed
                End If
            Else
                'merge the data
                With _word.ActiveDocument.MailMerge
                    .Destination = WdMailMergeDestination.wdSendToNewDocument
                    .MailAsAttachment = False
                    .MailAddressFieldName = ""
                    .MailSubject = ""
                    .SuppressBlankLines = True
                    'limit the record being merged to the current record number from the merge data file
                    With .DataSource
                        .FirstRecord = _currentFileLine
                        .LastRecord = _currentFileLine
                    End With
                    .Execute(Pause:=True)
                End With
                'set time stamp to add to file names so files for the same borrower are not overwritten
                Dim secondsSinceMidnight As Double = (Date.Now.Subtract(Date.Now.Date)).Milliseconds / 1000.0
                Dim timeStamp As String = secondsSinceMidnight.ToString().Replace(".", "").SafeSubstring(0, 7)
                'password protect document
                _word.ActiveDocument.Protect(2, , timeStamp)
                'save the file
				_word.ActiveDocument.SaveAs(FileName:="\\imgprodkofax\\ascent$\UT" & _archLib & "Other_imp\" & ssn & "_" & timeStamp & ".doc", _
					FileFormat:=WdSaveFormat.wdFormatDocument, LockComments:=False, Password:="", AddToRecentFiles:= _
					True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
					False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
					SaveAsAOCELetter:=False)
                'close the file
                _word.ActiveDocument.Close()
                'create the imaging control file for the doc to be imaged
				FileOpen(9, "\\imgprodkofax\\ascent$\UT" & _archLib & "Other_imp\" & ssn & "_" & timeStamp & ".ctl", OpenMode.Output)
                'write info to control file for correspondence
                PrintLine(9, "~^Folder~" & Date.Now.ToString("MM/dd/yyyy") & " " & Date.Now.TimeOfDay.ToString() & ", Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~" & ssn & "^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~" & _docID & "^Attribute~DOC_DATE~STR~" & Date.Now.ToString("MM/dd/yyyy") & "^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~" & Date.Now.ToString("MM/dd/yyyy") & "^Attribute~SCAN_TIME~STR~" & Date.Now.ToString("HH:mm:ss") & "^Attribute~DESCRIPTION~STR~" & Date.Now.ToString("MM/dd/yyyy") & " " & Date.Now.TimeOfDay.ToString())
				PrintLine(9, "DesktopDoc~\\imgprodkofax\\ascent$\UTCROther_imp\" & ssn & "_" & timeStamp & ".doc~" & Date.Now.ToString("MM/dd/yyyy") & " " & Date.Now.TimeOfDay.ToString() & ", Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~" & ssn & "^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~" & _docID & "^Attribute~DOC_DATE~STR~" & Date.Now.ToString("MM/dd/yyyy") & "^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~" & Date.Now.ToString("MM/dd/yyyy") & "^Attribute~SCAN_TIME~STR~" & Date.Now.ToString("HH:mm:ss"))
                FileClose(9)

                If _doInternalRecovery Then
                    'update recovery log
                    _recoveryFileHandle = FreeFile()
                    FileOpen(_recoveryFileHandle, _recoveryFilePathAndName, OpenMode.Output)
                    Write(_recoveryFileHandle, ssn)
                    FileClose(_recoveryFileHandle)
                End If

            End If
            _currentFileLine += 1
        End If

        _imageCount = _imageCount + 1

        Return ssn
    End Function

    ''' <summary>
    ''' Disposes of resources
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisposeOfComponents()
        'close Word
        _word.Application.Quit(SaveChanges:=WdSaveOptions.wdDoNotSaveChanges)
        FileClose(_fileHandle)

        'sometimes file control isn't released, this makes sure it is before continuing
        While True
            Try
                FileOpen(_fileHandle, _dataFile, OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
                FileClose(_fileHandle)

                Exit Sub
            Catch ex As IOException

            End Try
        End While

        'delete recovery log if it exists
        If (File.Exists(_recoveryFilePathAndName)) Then File.Delete(_recoveryFilePathAndName)
    End Sub


#End Region

#Region " IDisposable Support "
    Private _isDisposed As Boolean = False 'To detect redundant calls

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If (Not _isDisposed) Then
            If (disposing) Then
                'Dispose managed objects.
            End If

            DisposeOfComponents()

            _word = Nothing
        End If
        _isDisposed = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
