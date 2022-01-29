Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Linq
Imports System.IO
Imports Q

'TODO: Chances are the base class really is CLS-compliant. See if there are any downsides to marking it as such.
<CLSCompliant(False)> _
Public Class DataAccess
    Inherits DataAccessBase

#Region "BSYS"
    Private Shared _privateLoanTypes As List(Of String) = Nothing
    Public Shared Function GetPrivateLoanTypes(ByVal testMode As Boolean) As List(Of String)
        If _privateLoanTypes Is Nothing Then
            _privateLoanTypes = BsysDataContext(testMode).ExecuteQuery(Of String)("SELECT LoanType FROM GENR_REF_LoanTypes WHERE (TypeKey = 'Private')").ToList()
        End If
        Return _privateLoanTypes
    End Function

    Public Shared Function GetLoanRejectionDescription(ByVal testMode As Boolean, ByVal rejectCode As String) As String
        Dim query As String = String.Format("SELECT Description FROM GENR_REF_LoanRejectCodes WHERE (Code = '{0}')", rejectCode)
        Return BsysDataContext(testMode).ExecuteQuery(Of String)(query).Single()
    End Function

    Public Shared Function GetFavoriteScreen(ByVal testMode As Boolean) As String
        'See if the user info is in BSYS. (As of Dec. 2009 it's in the process of migrating there.)
        Dim query As String = String.Format("SELECT FavoriteScreen FROM SYSA_LST_UserLogonInfo WHERE WindowsUserID = '{0}'", Environment.UserName)
        Dim favoriteScreen As String = BsysDataContext(testMode).ExecuteQuery(Of String)(query).DefaultIfEmpty("").SingleOrDefault().Trim()
        If favoriteScreen.Length > 0 Then Return favoriteScreen

        'If it's not in BSYS, get it from the filesystem.
        Dim headers() As String = {"Blank", "FullName", "Title", "Extension", "UserId", "FavoriteScreen", "Region"}
        Dim userInfoTable As DataTable = Q.Common.CreateDataTableFromFile("T:\userinfo.txt", False, headers)
        If userInfoTable Is Nothing Then
            Return ""
        Else
            Return userInfoTable.Rows(0).Item("FavoriteScreen").ToString().Trim()
        End If
    End Function
#End Region 'BSYS

#Region "DUDE"
    Private Shared _testDataContext As DataContext = Nothing
    Private Shared _liveDataContext As DataContext = Nothing
    Private Shared Function DudeDataContext(ByVal testMode As Boolean) As DataContext
        If testMode Then
            If _testDataContext Is Nothing Then _testDataContext = New DataContext("Data Source=OPSDEV;Initial Catalog=MauiDUDE;Integrated Security=SSPI;")
            Return _testDataContext
        Else
            If _liveDataContext Is Nothing Then _liveDataContext = New DataContext("Data Source=NOCHOUSE;Initial Catalog=MauiDUDE;Integrated Security=SSPI;")
            Return _liveDataContext
        End If
    End Function

    ''' <summary>
    ''' Returns a list of all call categories from the CallCat_Categories table.
    ''' </summary>
    ''' <param name="testMode">True if in test mode.</param>
    Public Shared Function GetCallCategories(ByVal testMode As Boolean) As List(Of String)
        Dim query As String = "SELECT Category FROM CallCat_Categories"
        Return DudeDataContext(testMode).ExecuteQuery(Of String)(query).ToList()
    End Function

    ''' <summary>
    ''' Returns a list of reasons that apply to a given call category.
    ''' </summary>
    ''' <param name="testMode">True if in test mode.</param>
    ''' <param name="category">Call category.</param>
    Public Shared Function GetCallCategoryReasons(ByVal testMode As Boolean, ByVal category As String) As List(Of String)
        Dim query As String = String.Format("SELECT Reason FROM CallCat_CatReasonREF WHERE Category = '{0}'", category)
        Return DudeDataContext(testMode).ExecuteQuery(Of String)(query).ToList()
    End Function

    ''' <summary>
    ''' Inserts a call categorization record into the CallCat_Data table.
    ''' </summary>
    ''' <param name="testMode">True if in test mode.</param>
    ''' <param name="category">Call category.</param>
    ''' <param name="reason">Call reason.</param>
    ''' <param name="letterId">Letter ID. Apostrophes are automatically escaped.</param>
    ''' <param name="comments">User comments. Apostrophes are automatically escaped.</param>
    Public Shared Sub InsertCallCategorization(ByVal testMode As Boolean, ByVal category As String, ByVal reason As String, ByVal letterId As String, ByVal comments As String)
        Dim command As String = String.Format("EXEC spCallCategorizationInsert '{0}','{1}','{2}','{3}','{4}'", category, reason, letterId.Replace("'", "''"), comments.Replace("'", "''"), Environment.UserName)
        DudeDataContext(testMode).ExecuteCommand(command)
    End Sub
#End Region
End Class
