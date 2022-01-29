Imports System.Data.Linq
Imports System.Data.SqlClient

Public NotInheritable Class DataAccess
    Inherits DataAccessBase

    Private _bsys As DataContext
    Private _cls As DataContext
    Private _csys As DataContext
    Private _testMode As Boolean
    Private _region As ScriptSessionBase.Region

    Public Sub New(ByVal testMode As Boolean, ByVal region As ScriptSessionBase.Region)
        _testMode = testMode
        _region = region
        _bsys = BsysDataContext(testMode)
        _cls = ClsDataContext(testMode)
        _csys = CSYSDataContext(testMode)
    End Sub

    Public Sub AddFederalCostCenterPrintingRecord(ByVal letterId As String, ByVal foreignCount As Integer, ByVal domesticCount As Integer, ByVal costCenterCode As String)
        Dim sproc As New SprocCommandBuilder("spAddCostCenterPrintingRecord")
        sproc.AddParameter("LetterId", letterId)
        sproc.AddParameter("ForeignCount", foreignCount)
        sproc.AddParameter("DomesticCount", domesticCount)
        sproc.AddParameter("CostCenterCode", costCenterCode)
        _cls.ExecuteCommand(sproc.Command, sproc.ParameterValues)
    End Sub

    ''' <summary>
    ''' Gets user's business unit name for cost center printing
    ''' </summary>
    Public Function GetBusinessUnitNameForCostCenterPrinting(ByVal costCenterCode As String) As String
        Dim query As String = String.Format("SELECT Name FROM GENR_LST_UHEAACostCenters WHERE Code = '{0}'", costCenterCode)
        Return _bsys.ExecuteQuery(Of String)(query).SingleOrDefault()
    End Function

    ''' <summary>
    ''' Gets sheet count for batch 2D barcode process.
    ''' </summary>
    Public Function GetPaperSheetCountForBatch2D(ByVal letterID As String) As Barcode2DQueryResults
        Dim query As String = String.Format("SELECT TOP 1 Pages, Duplex FROM LTDB_DAT_CentralPrintingDocData WHERE ID = '{0}'", letterID)
        Return _bsys.ExecuteQuery(Of Barcode2DQueryResults)(query).SingleOrDefault()
    End Function

    ''' <summary>
    ''' Gets business unit for the Letter Delivery Form.
    ''' </summary>
    Public Shared Function GetBUForLtrDelvryMethFrm(ByVal testMode As Boolean) As List(Of String)
        Dim query As String = "SELECT BusinessUnit FROM GENR_REF_BUsAndAppKey WHERE ApplicationKey = 'Document Generation'"
        Return BsysDataContext(testMode).ExecuteQuery(Of String)(query).ToList
    End Function

    ''' <summary>
    ''' Get the loan types that match a given type key.
    ''' </summary>
    ''' <returns>A list of loan types.</returns>
    Public Shared Function GetLoanTypes(ByVal typeKey As String, ByVal testMode As Boolean) As List(Of String)
        Dim query As String = String.Format("SELECT LoanType FROM GENR_REF_LoanTypes WHERE TypeKey = '{0}'", typeKey)
        Return BsysDataContext(testMode).ExecuteQuery(Of String)(query).ToList()
    End Function

    Public Shared Function GetScriptName(ByVal scriptId As String) As String
        Dim query As String = String.Format("SELECT Script FROM SCKR_DAT_Scripts WHERE ID = '{0}'", scriptId)
        Return BsysDataContext(False).ExecuteQuery(Of String)(query).Single()
    End Function

    ''' <summary>
    ''' Gets applicable user information for user Centralized Printing and Barcode addition
    ''' </summary>
    Public Shared Function GetUserInfoForCentralizedPrintAnd2DObject(ByVal testMode As Boolean) As CentralizedPrintingAnd2DBarcodeInfo
        Dim query As String = "SELECT TOP 1 A.BusinessUnit as UsersBusinessUnit, A.BusinessUnit as OriginallyGatheredBusinessUnit, " + _
                                "A.AssociatedEmailAddr as UsersBuSentFromEmail, C.FirstName as UsersFirstName FROM GENR_LST_BusinessUnitEmailAddrs " + _
                                "A JOIN GENR_REF_BU_Agent_Xref B ON A.BusinessUnit = B.BusinessUnit JOIN SYSA_LST_Users C ON B.WindowsUserID = " + _
                                "C.WindowsUserName WHERE B.WindowsUserID = '" & Environment.UserName & "' AND B.Role = 'Member Of'"
        Return BsysDataContext(testMode).ExecuteQuery(Of CentralizedPrintingAnd2DBarcodeInfo)(query).SingleOrDefault()
    End Function

    ''' <summary>
    ''' Gets the business unit ID (integer) from CSYS for the business unit name provided.
    ''' </summary>
    ''' <param name="testmode"></param>
    ''' <param name="businessUnitName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetBusinessUnitId(ByVal testmode As Boolean, ByVal businessUnitName As String) As Integer
        Return CSYSDataContext(testmode).ExecuteQuery(Of Integer)("spGENR_GetBusinessUnitId {0}", businessUnitName).SingleOrDefault()
    End Function

    ''' <summary>
    ''' General use function to get email recipients from GENR_REF_MiscemailNotif BSYS table.
    ''' </summary>
    ''' <param name="typeKey">Type Key for desired rows.</param>
    ''' <param name="testMode">If the app is in test mode or not.</param>
    ''' <returns>Returns a comma delimited string of all recipients listed in the table for the provided Type Key.</returns>
    ''' GetEmailForKeyList
    <Obsolete("This method uses BSYS.GENR_REF_MiscEmailNotif which is obsolete, create a notification key in CSYS and use CSYS.spSYSA_GetEmailForKey instead")> _
    Public Shared Function GetBsysMiscEmailNotifRecipients(ByVal typeKey As String, ByVal testMode As Boolean) As String
        'Define the query, including a test mode clause.
        Dim query As String = String.Format("SELECT WinUName FROM GENR_REF_MiscemailNotif WHERE TypeKey = '{0}'", typeKey)
        If testMode Then
            query += " AND ExcldTest = 0"
        End If
        'Run the query and put the results into an array for easy manipulation.
        Dim userNames As String() = BsysDataContext(testMode).ExecuteQuery(Of String)(query).ToArray()
        'Append the mail domain to each user name.
        For i As Integer = userNames.GetLowerBound(0) To userNames.GetUpperBound(0)
            userNames(i) += "@utahsbr.edu"
        Next
        'Return the array joined into a single comma-delimited string.
        Return String.Join(",", userNames)
    End Function

    ''' <summary>
    ''' Gets all affiliated lender IDs for provided lender.
    ''' </summary>
    ''' <param name="lenderName">Lender to check for.</param>
    ''' <param name="testMode">If the application is in test mode or not.</param>
    ''' <returns>A list off all affiliated lender ids.</returns>
    Public Shared Function GetAffiliatedLenderIds(ByVal lenderName As String, ByVal testMode As Boolean) As List(Of String)
        Dim query As String = String.Format("SELECT LenderID FROM GENR_REF_LenderAffiliation WHERE Affiliation = '{0}'", lenderName)
        Return BsysDataContext(testMode).ExecuteQuery(Of String)(query).ToList()
    End Function


    ''' <summary>
    ''' Overload for older code.  Passes UHEAA as system region to main function.  Creates letter record for user Centralized Printing process.
    ''' </summary>
    Public Shared Function CreateLetterRecordForCentralizedPrinting(ByVal testMode As Boolean, ByVal letterId As String, ByVal acctNum As String, ByVal businessUnit As String, ByVal domesticState As String) As LetterRecordCreationResults
        Return CreateLetterRecordForCentralizedPrinting(testMode, letterId, acctNum, businessUnit, domesticState, ScriptSessionBase.Region.UHEAA)
    End Function

    ''' <summary>
    ''' Creates letter record for user Centralized Printing process.
    ''' </summary>
    Public Shared Function CreateLetterRecordForCentralizedPrinting(ByVal testMode As Boolean, ByVal letterId As String, ByVal acctNum As String, ByVal businessUnit As String, ByVal domesticState As String, ByVal systemRegion As ScriptBase.Region) As LetterRecordCreationResults
        If systemRegion = ScriptSessionBase.Region.UHEAA Then
            Dim comm As New SqlCommand("spPRNT_CreateLetterRec", New SqlConnection(BsysDataContext(testMode).Connection.ConnectionString))
            comm.CommandType = CommandType.StoredProcedure
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@LetterID", .Value = letterId, .Direction = ParameterDirection.Input, .Size = 10})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@AcctNum", .Value = acctNum, .Direction = ParameterDirection.Input, .Size = 20})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@BU", .Value = businessUnit, .Direction = ParameterDirection.Input, .Size = 50})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@Dom", .Value = domesticState, .Direction = ParameterDirection.Input, .Size = 2})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@NewRecNum", .Direction = ParameterDirection.Output, .Size = 8})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@BarcodeSeqNum", .Direction = ParameterDirection.Output, .Size = 8})
            comm.Connection.Open()
            comm.ExecuteScalar()
            'results = New LetterRecordCreationResults(CType(comm.Parameters("@NewRecNum").Value, Long), CType(comm.Parameters("@BarcodeSeqNum").Value, Long))
            Dim results As LetterRecordCreationResults = New LetterRecordCreationResults()
            results.NewRecordIdentity = CType(comm.Parameters("@NewRecNum").Value, Long)
            results.BarcodeSeqNum = CType(comm.Parameters("@BarcodeSeqNum").Value, Long)
            comm.Connection.Close()
            Return results
        Else
            'get domestic indicator
            Dim domesticIndicator As Boolean = CSYSDataContext(testMode).ExecuteQuery(Of Boolean)("EXEC spGENR_GetDomesticIndicator {0}", domesticState).SingleOrDefault

            'get letter info needed for subsequent queries
            Dim letterInfo As New LetterInformation
            letterInfo = BsysDataContext(testMode).ExecuteQuery(Of LetterInformation)("EXEC spLTDB_GetLetterInfo {0}", letterId).SingleOrDefault

            'get a list of letter IDs that are printed like the letterID being processed and convert it to a comma delimited string
            Dim query As String = String.Format("EXEC spLTDB_GetLikeLetterIds @CostCenter = '{0}', @Duplex = {1}, @Pages = {2}", letterInfo.CostCenter, letterInfo.Duplex, letterInfo.Pages)
            Dim likeLetterIds As List(Of String) = BsysDataContext(testMode).ExecuteQuery(Of String)(query).ToList
            Dim inLetterIds As String = String.Format("'{0}'", String.Join("','", likeLetterIds.ToArray))

            'convert the business unit name to the ID
            Dim businessUnitId As Integer = GetBusinessUnitId(testMode, businessUnit)

            'run stored procedure to add letter printing record and return state mail barcode seqno
            Dim comm As New SqlCommand("spCentralizedPrintingCreateLetterRec", New SqlConnection(ClsDataContext(testMode).Connection.ConnectionString))
            comm.CommandType = CommandType.StoredProcedure
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@LetterID", .Value = letterId, .Direction = ParameterDirection.Input, .Size = 10})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@AcctNum", .Value = acctNum, .Direction = ParameterDirection.Input, .Size = 20})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@BU", .Value = businessUnitId, .Direction = ParameterDirection.Input})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@DomInd", .Value = domesticIndicator, .Direction = ParameterDirection.Input})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@SpecialHandling", .Value = letterInfo.SpecialHandling, .Direction = ParameterDirection.Input})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@LetterIds", .Value = inLetterIds, .Direction = ParameterDirection.Input, .Size = -1})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@NewRecNum", .Direction = ParameterDirection.Output, .Size = 8})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@BarcodeSeqNum", .Direction = ParameterDirection.Output, .Size = 8})
            comm.Connection.Open()
            comm.ExecuteScalar()
            Dim results As LetterRecordCreationResults = New LetterRecordCreationResults()
            results.NewRecordIdentity = CType(comm.Parameters("@NewRecNum").Value, Long)
            results.BarcodeSeqNum = CType(comm.Parameters("@BarcodeSeqNum").Value, Long)
            comm.Connection.Close()
            Return results
        End If
    End Function

    ''' <summary>
    ''' Overload for older code.  Passes UHEAA as system region to main function.  Creates fax record for user Centralized Printing process.
    ''' </summary>
    Public Shared Function CreateFaxRecordForCentralizedPrinting(ByVal testMode As Boolean, ByVal dialableFaxNum As String, ByVal acctNum As String, ByVal businessUnit As String, ByVal letterID As String, ByVal commentsAddedTo As String) As Long
        CreateFaxRecordForCentralizedPrinting(testMode, dialableFaxNum, acctNum, businessUnit, letterID, commentsAddedTo, ScriptSessionBase.Region.UHEAA)
    End Function


    ''' <summary>
    ''' Creates fax record for user Centralized Printing process.
    ''' </summary>
    Public Shared Function CreateFaxRecordForCentralizedPrinting(ByVal testMode As Boolean, ByVal dialableFaxNum As String, ByVal acctNum As String, ByVal businessUnit As String, ByVal letterID As String, ByVal commentsAddedTo As String, ByVal systemRegion As ScriptBase.Region) As Long
        Dim comm As SqlCommand
        Dim newRecordNum As Long

        If systemRegion = ScriptSessionBase.Region.UHEAA Then
            comm = New SqlCommand("spPRNT_CreateFaxRec", BsysDataContext(testMode).Connection)
            comm.CommandType = CommandType.StoredProcedure
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@FaxNum", .Value = dialableFaxNum, .Direction = ParameterDirection.Input, .Size = 20})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@AcctNum", .Value = acctNum, .Direction = ParameterDirection.Input, .Size = 20})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@BU", .Value = businessUnit, .Direction = ParameterDirection.Input, .Size = 50})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@LID", .Value = letterID, .Direction = ParameterDirection.Input, .Size = 10})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@CommentsAddedTo", .Value = commentsAddedTo, .Direction = ParameterDirection.Input, .Size = 10})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@NewRecNum", .Direction = ParameterDirection.Output, .Size = 8})
        Else
            Dim businessUnitId As Integer = GetBusinessUnitId(testMode, businessUnit)

            comm = New SqlCommand("spCentralizedPrintingCreateFaxRec", New SqlConnection(ClsDataContext(testMode).Connection.ConnectionString))
            comm.CommandType = CommandType.StoredProcedure
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@LID", .Value = letterID, .Direction = ParameterDirection.Input, .Size = 10})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@AcctNum", .Value = acctNum, .Direction = ParameterDirection.Input, .Size = 20})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@BU", .Value = businessUnitId, .Direction = ParameterDirection.Input})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@FaxNum", .Value = dialableFaxNum, .Direction = ParameterDirection.Input, .Size = 20})
            comm.Parameters.Add(New SqlParameter With {.ParameterName = "@NewRecNum", .Direction = ParameterDirection.Output, .Size = 8})
        End If

        comm.Connection.Open()
        comm.ExecuteScalar()
        newRecordNum = CType(comm.Parameters("@NewRecNum").Value, Long)
        comm.Connection.Close()
        Return newRecordNum
    End Function

    Public Function GetCostCenterInstructions(ByVal letterId As String) As String
        Dim query As String = String.Format("SELECT COALESCE(Instructions, '') AS Instructions FROM LTDB_DAT_CentralPrintingDocData WHERE ID = '{0}'", letterId)
        Try
            Return _bsys.ExecuteQuery(Of String)(query).DefaultIfEmpty("").SingleOrDefault()
        Catch ex As Exception
            Dim message As String = String.Format("Could not find letter ID {0} in Letter Tracking.", letterId)
            Throw New Exception(message, ex)
        End Try
    End Function

    Public Shared Function GetCostCenterPages(ByVal testMode As Boolean, ByVal letterId As String) As Integer
        Dim query As String = String.Format("SELECT CAST(Pages AS INT) AS Pages FROM LTDB_DAT_CentralPrintingDocData WHERE ID = '{0}'", letterId)
        Try
            Return BsysDataContext(testMode).ExecuteQuery(Of Integer)(query).SingleOrDefault()
        Catch ex As Exception
            Dim message As String = String.Format("Could not retrieve the number of pages for letter ID {0} in Letter Tracking.", letterId)
            Throw New Exception(message, ex)
        End Try
    End Function

    Public Shared Function GetDocumentPathAndFileName(ByVal testMode As Boolean, ByVal docIDFromLetterTracking As String) As DocumentPathAndName
        Dim query As String = String.Format("SELECT TOP 1 Path as OriginalDBEntry FROM LTDB_DAT_CentralPrintingDocData WHERE ID = '{0}'", docIDFromLetterTracking)
        Try
            Return BsysDataContext(testMode).ExecuteQuery(Of DocumentPathAndName)(query).SingleOrDefault()
        Catch ex As Exception
            Dim message As String = String.Format("Could not find letter ID {0} in Letter Tracking.", docIDFromLetterTracking)
            Throw New Exception(message, ex)
        End Try
    End Function

    Public Function GetLetterName(ByVal letterId As String) As String
        Dim query As String = String.Format("SELECT DocName FROM LTDB_DAT_DocDetail WHERE ID = '{0}'", letterId)
        Try
            Return _bsys.ExecuteQuery(Of String)(query).Single()
        Catch ex As Exception
            Dim message As String = String.Format("Could not find letter ID {0} in Letter Tracking.", letterId)
            Throw New Exception(message, ex)
        End Try
    End Function

    Public Shared Function GetLoanProgramDescription(ByVal loanType As String, ByVal testMode As Boolean) As List(Of String)
        Dim query As String = String.Format("SELECT Description FROM GENR_REF_LoanTypes WHERE LoanType = '{0}'", loanType)
        Return BsysDataContext(testMode).ExecuteQuery(Of String)(query).ToList()
    End Function

    Public Function GetFileSystemPath(ByVal fileSystemKey As String) As String
		Dim command As String = "EXEC spGENR_GetFileSystemObject {0}, {1}, {2}"
		Try
			Return _csys.ExecuteQuery(Of String)(command, fileSystemKey, _testMode, _region.ToString).Single
		Catch ex As InvalidOperationException
			Throw New Exception(String.Format("The file key {0} was not found in EnterpriseFileSystem.  Please contact Systems Support.", fileSystemKey), ex)
		End Try
	End Function

	Public Function GetSsnFromAcctNum(ByVal acctNum As String)
		Dim selectStr As String = "EXEC spGetSSNFromAcctNumber {0}"
		Return CDWDataContext(_testMode).ExecuteQuery(Of String)(selectStr, acctNum).Single
	End Function

	Public Function GetSsnFromAcctNum(ByVal acctNum As String, ByVal region As ScriptSessionBase.Region)
		Dim selectStr As String = "EXEC spGetSSNFromAcctNumber {0}"
		If region = ScriptSessionBase.Region.CornerStone Then
			Return CDWDataContext(_testMode).ExecuteQuery(Of String)(selectStr, acctNum).Single
		Else
			Return UDWDataContext(_testMode).ExecuteQuery(Of String)(selectStr, acctNum).Single
		End If
	End Function
End Class
