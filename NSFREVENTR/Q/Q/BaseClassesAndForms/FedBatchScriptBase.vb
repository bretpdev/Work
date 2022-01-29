Imports System.IO
Imports System.Windows.Forms

Public MustInherit Class FedBatchScriptBase
    Inherits BatchScriptBase

    Private ReadOnly _err As ErrorReport
    Protected ReadOnly Property Err() As ErrorReport
        Get
            Return _err
        End Get
    End Property

    Private ReadOnly _eoj As EndOfJobReport
    Protected ReadOnly Property Eoj() As EndOfJobReport
        Get
            Return _eoj
        End Get
    End Property

    Private ReadOnly _userId As String
    Protected ReadOnly Property UserID() As String
        Get
            Return _userId
        End Get
    End Property


    Private _recovery As RecoveryLog
    ''' <summary>
    ''' Recovery log object
    ''' </summary>
    Protected Overrides ReadOnly Property Recovery() As RecoveryLog
        Get
            Return _recovery
        End Get
    End Property

    ''' <summary>
    ''' Creates a convenience layer for writing scripts in the federal region and automatically validates to the federal region.
    ''' </summary>
    ''' <param name="ri"></param>
    ''' <param name="scriptId"></param>
    ''' <remarks></remarks>
    Protected Sub New(ByVal ri As ReflectionInterface, ByVal scriptId As String)
        Me.New(ri, scriptId, Region.CornerStone)
    End Sub

    ''' <summary>
    ''' Creates a convenience layer for writing scripts in the federal region and validates to the specified region.
    ''' </summary>
    ''' <param name="ri">An interface to the running Reflection session.</param>
    ''' <param name="scriptId">Script ID from Sacker's detail screen.</param>
    ''' <param name="regionToValidate">
    ''' The region the script is required to start in.
    ''' Note that validating the required region involves changing screens. To avoid this behavior, select "None."
    ''' </param>
    Protected Sub New(ByVal ri As ReflectionInterface, ByVal scriptId As String, ByVal regionToValidate As Region)
        MyBase.New(ri, scriptId)
        MyBase._enterpriseFileSystem = New EnterpriseFileSystem(ri.TestMode, Region.CornerStone)
        Try
            ri.FastPath("PROF")
            _userId = ri.GetText(2, 49, 7)
            ValidateRegion(regionToValidate)
        Catch ex As Exception
            Dim scriptName As String = DataAccess.GetScriptName(scriptId)
            Dim message As String = String.Format("The {0} script was unable to access the system to get the user ID and/or validate the region.  Resolve the system access issue and run the script again.", scriptName)
            If (CalledByJams) Then
                Throw New Exception(message)
            Else
                Throw New StupRegionSpecifiedException(message)
            End If
        End Try
        _recovery = New RecoveryLog(String.Format("{0}_{1}", scriptId, _userId), Efs)
    End Sub

    ''' <summary>
    ''' Creates a convenience layer for writing scripts in the federal region and validates to the specified region.
    ''' </summary>
    ''' <param name="ri">An interface to the running Reflection session.</param>
    ''' <param name="scriptId">Script ID from Sacker's detail screen.</param>
    ''' <param name="eojReportFilesystemKey">
    ''' The key in CSYS.GENR_DAT_EnterpriseFileSystem that points (along with regionToValidate) to the directory where the end-of-job report should be saved.
    ''' </param>
    ''' <param name="errorReportFilesystemKey">
    ''' The key in CSYS.GENR_DAT_EnterpriseFileSystem that points (along with regionToValidate) to the directory where the error report should be saved.
    ''' </param>
    Protected Sub New(ByVal ri As ReflectionInterface, ByVal scriptId As String, ByVal errorReportFilesystemKey As String, ByVal eojReportFilesystemKey As String, ByVal eojFields As IEnumerable(Of String))
        Me.New(ri, scriptId, Region.CornerStone)
        Dim applicationName As String = DataAccess.GetScriptName(scriptId)
        _err = New ErrorReport(ri.TestMode, applicationName, errorReportFilesystemKey, Region.CornerStone)
        _eoj = New EndOfJobReport(ri.TestMode, applicationName, eojReportFilesystemKey, Region.CornerStone, eojFields)
    End Sub

    ''' <summary>
    ''' Does cost center printing for federal loan servicing.
    ''' Barcode fields are automatically taken care of without touching the original data file.
    ''' </summary>
    ''' <param name="letterId">The letter ID from Letter Tracking.</param>
    ''' <param name="dataFile">The full path and name of the data file to be merged into the letter.</param>
    ''' <param name="stateCodeFieldName">The header field name for the data file's state code column.</param>
    ''' <param name="acctNumFieldName">The header field name for the data file's account number column.</param>
    ''' <param name="recipient">The recipient type for the letters (e.g., borrower, endorser, reference).</param>
    Protected Overloads Sub CostCenterPrinting(ByVal letterId As String, ByVal dataFile As String, ByVal stateCodeFieldName As String, ByVal acctNumFieldName As String, ByVal recipient As DocumentHandling.Barcode2DLetterRecipient)
        DocumentHandling.FederalCostCenterPrinting(RI.TestMode, letterId, dataFile, stateCodeFieldName, ScriptID, acctNumFieldName, recipient, DocumentHandling.CostCenterOption.AddBarcode)
    End Sub

    ''' <summary>
    ''' Checks the recovery log to see which row number was last processed, and returns the next row number.
    ''' When calling this method from multi-stage recovery, you will need to determine whether all stages
    ''' have completed for the row, and if not, decrement the value returned by this method.
    ''' Row numbers are one-based.
    ''' </summary>
    ''' <returns>The row number following the last one processed, or 1 if not in recovery.</returns>
    Protected Overloads Function GetRecoveryRow() As Integer
        Dim nextRow As Integer = 1
        If (Recovery.RecoveryValue.Length > 0) Then
            If Not Integer.TryParse(Recovery.RecoveryValue.Split(",")(0), nextRow) Then
                Throw New Exception("The script is in recovery, but the first value in the recovery log is not a row number.")
            End If
            'The recovery value is the last row processed. Increment it to get the next row to process.
            nextRow += 1
            'Reset the recovery value to get out of recovery mode.
            Recovery.RecoveryValue = String.Empty
        End If
        Return nextRow
    End Function

    ''' <summary>
    ''' Publishes the error report (if it exists), updates the MBS log file's time stamp, and ends the script.
    ''' If not being run by Master Batch Script or JAMS, a prompt is shown indicating that procesing is complete.
    ''' </summary>
    Protected Overrides Sub ProcessingComplete()
        ProcessingComplete(True)
    End Sub

    Protected Overloads Sub ProcessingComplete(ByVal publishReports As Boolean)
        If (publishReports) Then
            If (_err IsNot Nothing) Then _err.Publish()
            If (_eoj IsNot Nothing) Then _eoj.Publish()
        End If
        Using File.Create(String.Format("{0}MBS{1}.TXT", Efs.LogsFolder, ScriptID))
        End Using
        Recovery.Delete()
        If ((Not CalledByMasterBatchScript()) AndAlso (Not CalledByJams)) Then MessageBox.Show("Processing Complete", ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Information)
        EndDLLScript()
    End Sub

End Class
