Imports System.IO
Imports Reflection
Imports System.Windows.Forms

Public MustInherit Class BatchScriptBase
    Inherits ScriptBase

    Public Const JAMS_ID As String = "JAMS"
    Public Const MASTER_BATCH_SCRIPT_ID As String = "MasterBatchScript"

    ''' <summary>
    ''' Indicates whether the session was opened by JAMS.
    ''' </summary>
    ''' <returns>True if the session's MacroData string contains "JAMS"</returns>
    Protected Overridable ReadOnly Property CalledByJams() As Boolean
        Get
            If String.IsNullOrEmpty(RS.MacroData) Then
                Return False
            Else
                Return RS.MacroData.Contains(JAMS_ID)
            End If
        End Get
    End Property

    Private _errorReportFolder As String
    ''' <summary>
    ''' Convenience property for batch scripts that returns the EnterpriseFileSystem directory for the "ERR_BU35" key.
    ''' </summary>
    ''' <remarks>
    ''' This property has little value now that we have an ErrorReport class whose constructor takes an EnterpriseFileSystem key.
    ''' As of now, the only scripts using this property are the federal Queue Builder and Small Balance Write-Off scripts.
    ''' They may as well move away from using this property, at which point we can get rid of it.
    ''' </remarks>
    <Obsolete("Use Efs.GetPath() if you really need Systems Support's error report folder for something other than an ErrorReport object")> _
    Protected Overridable ReadOnly Property ErrorReportFolder() As String
        Get
            If (String.IsNullOrEmpty(_errorReportFolder)) Then
                _errorReportFolder = _enterpriseFileSystem.GetPath("ERR_BU35")
            End If
            Return _errorReportFolder
        End Get
    End Property

    Private _ftpFolder As String
    ''' <summary>
    ''' Convenience property for batch scripts that returns the EnterpriseFileSystem directory for the "FTP" key.
    ''' </summary>
    ''' <remarks>
    ''' This property has little value, since Efs is available to every script and it includes an FtpFolder property.
    ''' As of now, it looks like we have 8 scripts using it (3 federal and 5 commercial).
    ''' They may as well move away from using this property, at which point we can get rid of it.
    ''' </remarks>
    <Obsolete("Use Efs.FtpFolder instead")> _
    Protected Overridable ReadOnly Property FtpFolder() As String
        Get
            If (String.IsNullOrEmpty(_ftpFolder)) Then _ftpFolder = Efs.FtpFolder
            Return _ftpFolder
        End Get
    End Property

    Private _recovery As RecoveryLog
    ''' <summary>
    ''' Provides access to a simple recovery log that is tied to the script and the session user.
    ''' </summary>
    Protected Overridable ReadOnly Property Recovery() As RecoveryLog
        Get
            Return _recovery
        End Get
    End Property

    ''' <summary>
    ''' Creates a BatchScript object for the given script ID, with session interaction happening through the given ReflectionInterface.
    ''' </summary>
    ''' <param name="ri">Reflection Interface</param>
    ''' <param name="scriptID">Script ID from Sacker</param>
    Protected Sub New(ByVal ri As ReflectionInterface, ByVal scriptID As String)
        MyBase.New(ri, scriptID)
        _recovery = New RecoveryLog(TestModeProperty, scriptID)
    End Sub

    ''' <summary>
    ''' Indicates whether the session was opened by Master Batch Script.
    ''' </summary>
    ''' <returns>True if the session's MacroData string contains "MasterBatchScript"</returns>
    ''' <remarks>This really should be a property.</remarks>
    Protected Function CalledByMasterBatchScript() As Boolean
        If String.IsNullOrEmpty(RS.MacroData) Then
            Return False
        Else
            Return RS.MacroData.Contains(MASTER_BATCH_SCRIPT_ID)
        End If
    End Function

    ''' <summary>
    ''' Merges a comma-delimited data file with a Word document and sends the results, along with a State Mail cover sheet, to the system's default printer.
    ''' </summary>
    ''' <param name="letterId">The letter ID from Letter Tracking.</param>
    ''' <param name="dataFile">The full path and name of the data file to be merged into the letter.</param>
    ''' <param name="costCenterFieldName">The header field name for the data file's cost center column.</param>
    ''' <param name="stateCodeFieldName">The header field name for the data file's state code column.</param>
    ''' <remarks>
    ''' 2-D barcode fields and the current date are added by this method to a temporary copy of the data file,
    ''' which is used to perform the actual merge and is deleted before the method returns.
    ''' As of now, there is only one federal cost center, so the "costCenterFieldName" parameter is ignored.
    ''' </remarks>
    Protected Overridable Sub CostCenterPrinting(ByVal letterId As String, ByVal dataFile As String, ByVal costCenterFieldName As String, ByVal stateCodeFieldName As String)
        DocumentHandling.CostCenterPrinting(RI.TestMode, letterId, dataFile, costCenterFieldName, stateCodeFieldName, ScriptID)
    End Sub

    ''' <summary>
    ''' Gets the name of the newest file that matches a given search pattern,
    ''' and deletes older files that match the pattern in the same directory.
    ''' </summary>
    ''' <param name="path">The directory in which to look for files.</param>
    ''' <param name="searchPattern">The pattern that the file name must match. Wild cards are acceptable.</param>
    ''' <param name="options">Flags for additional checks.</param>
    ''' <returns>
    ''' The name of the newest file in the given path,
    ''' or an empty string if no matches are found and the ErrorOnMissing option is not selected.
    ''' </returns>
    ''' <remarks>Exceptions will not be generated if their respective options are not selected.</remarks>
    ''' <exception cref="System.IO.FileNotFoundException"></exception>
    ''' <exception cref="FileEmptyException"></exception>
    Protected Function DeleteOldFilesReturnMostCurrent(ByVal path As String, ByVal searchPattern As String, ByVal options As Common.FileOptions) As String
        Return Common.DeleteOldFilesReturnMostCurrent(path, searchPattern, options)
    End Function

    ''' <summary>
    ''' Searches for the next file (as determined by the Visual Basic Dir() function) that is populated.
    ''' If file found is empty then it deletes it and searches for another else it returns it.
    ''' </summary>
    ''' <param name="path">Path to the file.</param>
    ''' <param name="searchPattern">The pattern that the file name must match. Wild cards are acceptable.</param>
    ''' <returns>The full path and name of the next populated file, or an empty string if no populated files remain.</returns>
    Protected Function GetNextPopulatedFile(ByVal path As String, ByVal searchPattern As String) As String
        Return Common.GetNextPopulatedFile(path, searchPattern)
    End Function

    ''' <summary>
    ''' Finds the oldest file that matches a search pattern.
    ''' </summary>
    ''' <param name="path">Path to search.</param>
    ''' <param name="searchPattern">Search pattern.</param>
    ''' <returns>The full path and name of the oldest file, or an empty string if no files match the search pattern.</returns>
    Protected Function ReturnOldestFile(ByVal path As String, ByVal searchPattern As String) As String
        Return Common.ReturnOldestFile(path, searchPattern)
    End Function

    ''' <summary>
    ''' Checks the recovery log to see which row number needs to be processed next.
    ''' This is useful only if the recovery log stores row numbers as they are processed.
    ''' Assumes zero-based row numbers.
    ''' </summary>
    ''' <param name="indexOfRowValue">
    ''' For comma-delimited log formats, the zero-based index of the value that stores the last processed row.
    ''' (I.e., the array index where the last processed row would appear after using the Split() function on the log.)
    ''' For single-value logs, use 0.
    ''' </param>
    ''' <returns>The next row number to be processed, or 0 if not in recovery.</returns>
    ''' <remarks>This method has proven to be opaque enough that it's not very practical. We should probably deprecate it at the first opportunity.</remarks>
    <Obsolete("This method has proven to be opaque enough that it's not very practical")> _
    Protected Function GetRecoveryRow(ByVal indexOfRowValue As Integer) As Integer
        Dim nextRow As Integer = 0
        If (Not String.IsNullOrEmpty(Recovery.RecoveryValue)) Then
            If Not Integer.TryParse(Recovery.RecoveryValue.Split(",")(indexOfRowValue), nextRow) Then
                Throw New Exception("The script is in recovery, but the value in the recovery log is not a row number.")
            End If
            'The recovery value is the last row processed. Increment it to get the next row to process.
            nextRow += 1
            'Reset the recovery value to get out of recovery mode.
            Recovery.RecoveryValue = String.Empty
        End If
        Return nextRow
    End Function

    ''' <summary>
    ''' Outputs a message to the console if run by JAMS, or in a message box otherwise.
    ''' </summary>
    ''' <param name="format">A composite format string.</param>
    ''' <param name="args">Objects to format.</param>
    Protected Sub NotifyAndContinue(ByVal format As String, ByVal ParamArray args() As Object)
        Dim message As String = String.Format(format, args)
        If (CalledByJams) Then
            Console.WriteLine(message)
        Else
            MessageBox.Show(message, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ''' <summary>
    ''' Outputs a message to the console if run by JAMS, or in a message box otherwise, and ends the script.
    ''' Creates an error file in the EnterpriseFileSystem directory for the "ERR_BU35" key,
    ''' named after the script (according to Sacker), and containing the given message.
    ''' </summary>
    ''' <param name="format">A composite format string.</param>
    ''' <param name="args">Objects to format.</param>
    ''' <remarks>
    ''' The message to the JAMS console happens via an exception thrown from this method, so it assumes
    ''' someone up the call stack (such as BatchScriptStarter) is catching exceptions and writing them to the console.
    ''' </remarks>
    Protected Sub NotifyAndEnd(ByVal format As String, ByVal ParamArray args() As Object)
        Dim reportFolder As String = Efs.GetPath("ERR_BU35")
        Dim scriptName As String = DataAccess.GetScriptName(ScriptID)
        Dim errorFile As String = String.Format("{0}{1} {2:MM-dd-yyyy HH.mm}.txt", reportFolder, scriptName, DateTime.Now)
        Dim message As String = String.Format(format, args)
        File.WriteAllText(errorFile, message)
        If (CalledByJams) Then
            Throw New Exception(message)
        Else
            MessageBox.Show(message, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Error)
            EndDLLScript()
        End If
    End Sub

#Region "ProcessingComplete"
    ''' <summary>
    ''' Updates the MBS log file's time stamp and ends the script.
    ''' If not being run by Master Batch Script or JAMS, a prompt is optionally shown indicating that processing is complete.
    ''' </summary>
    ''' <param name="showPrompt">False to suppress the prompt that processing is complete.</param>
    Protected Sub ProcessingComplete(ByVal showPrompt As Boolean)
        ProcessingComplete(showPrompt, Nothing)
    End Sub

    ''' <summary>
    ''' Updates the MBS log file's time stamp and ends the script.
    ''' If not being run by Master Batch Script or JAMS, the given message is shown in a prompt.
    ''' </summary>
    ''' <param name="message">The message to be displayed when not being run by Master Batch Script or JAMS.</param>
    Protected Sub ProcessingComplete(ByVal message As String)
        ProcessingComplete(True, message)
    End Sub

    ''' <summary>
    ''' Updates the MBS log file's time stamp and ends the script.
    ''' If not being run by Master Batch Script or JAMS, a prompt is shown indicating that procesing is complete.
    ''' </summary>
    Protected Overridable Sub ProcessingComplete()
        ProcessingComplete(True, Nothing)
    End Sub

    Private Sub ProcessingComplete(ByVal showPrompt As Boolean, ByVal message As String)
        File.WriteAllText(String.Format("{0}MBS{1}.TXT", Efs.LogsFolder, ScriptID), "")
        Recovery.Delete()
        If (Not (CalledByMasterBatchScript() OrElse CalledByJams)) AndAlso showPrompt Then
            If String.IsNullOrEmpty(message) Then message = "Processing Complete"
            MessageBox.Show(message, MyBase.ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        EndDLLScript()
    End Sub
#End Region 'ProcessingComplete

    ''' <summary>
    ''' Sorts file data
    ''' </summary>
    ''' <param name="FilePathAndName">The full path and file name of file to sort.</param>
    ''' <param name="ElementToSortBy">The index of the column to sort.</param>
    ''' <param name="HeaderRow">The header row to prepend to the file.  Before the C# conversion this was set as an empty string.  If and empty string is passed, the function won't prepend anything to the file.</param>
    ''' <remarks>The only script using this method is OCR Autopay, which can probably find better ways to do it. Let's deprecate this.</remarks>
    <Obsolete("The only script using this method is OCR Autopay, which can probably find better ways to do it")> _
    Protected Sub SortFile(ByVal filePathAndName As String, ByVal elementToSortBy As Integer, ByVal headerRow As String)
        Common.SortFile(filePathAndName, elementToSortBy, headerRow)
    End Sub

    ''' <summary>
    ''' Checks whether the script is running interactively (i.e., not by JAMS or Master Batch)
    ''' and if so, shows a message box with the given message, the script name as a caption,
    ''' and OK and Cancel buttons. If the OK button is not clicked, the script ends.
    ''' </summary>
    ''' <param name="message">The message to show, ideally with instructions to click OK to continue or Cancel to end.</param>
    Protected Sub StartupMessage(ByVal message As String)
        If (Not CalledByJams) AndAlso (Not CalledByMasterBatchScript()) Then
            Dim scriptName As String = DataAccess.GetScriptName(ScriptID)
            If (MessageBox.Show(message, scriptName, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) <> DialogResult.OK) Then EndDLLScript()
        End If
    End Sub

    ''' <summary>
    ''' Adds 2D barcode fields and the static date field to the given data file.  FOR BATCH SCRIPT USE ONLY.
    ''' </summary>
    ''' <param name="dataFile">The full path and name of the data file that needs barcode data.</param>
    ''' <param name="accountNumberHeader">The name of the account number field (from header row).</param>
    ''' <param name="letterID">
    ''' The letter ID (from Letter Tracking) of the document that will be merged with this data.
    ''' (The number of State Mail barcodes is determined from the page count and duplex indicator from Letter Tracking.)
    ''' </param>
    ''' <param name="createNewFile">
    ''' If true, a new file is created with the original data plus the barcode data, and the original data file is left as-is.
    ''' If false, the barcode data will be prepended to the original data file.
    ''' Note that the only correct way to use this method is to say "true" for this parameter, and delete the new data file after printing is done.
    ''' </param>
    ''' <param name="personType">The person type who will be receiving the letter. (Determines whether to generate a return mail barcode.)</param>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <returns>
    ''' The full path and name of the resulting data file.
    ''' If "createNewFile" is false, the return value will be the same as the passed-in "dataFile" value.
    ''' </returns>
    ''' <remarks>We really should let the CostCenterPrinting method handle barcode data (like the federal one does) and deprecate this method.</remarks>
    <Obsolete("We really should let the CostCenterPrinting method handle barcode data (like the federal one does) and deprecate this method")> _
    Protected Function AddBarcodeAndStaticCurrentDateForBatchProcessing(ByVal dataFile As String, ByVal accountNumberHeader As String, ByVal letterID As String, ByVal createNewFile As Boolean, ByVal personType As DocumentHandling.Barcode2DLetterRecipient, ByVal testMode As Boolean) As String
        Return DocumentHandling.AddBarcodeAndStaticCurrentDateForBatchProcessing(dataFile, accountNumberHeader, letterID, createNewFile, personType, testMode)
    End Function
End Class
