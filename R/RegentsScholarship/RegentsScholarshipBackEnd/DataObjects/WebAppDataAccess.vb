Imports System.Data.Linq
Imports System.Data.SqlClient

Public Class WebAppDataAccess
    Private Shared _liveROSSDataContext As DataContext
    Private Shared _testROSSDataContext As DataContext

    ''' <summary>
    ''' Creates datacontext as needed.  Ensures that only one is created to save processor and memory use.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    Protected Shared Function ROSSDataContext(ByVal testMode As Boolean) As DataContext
        If testMode Then
            If _testROSSDataContext Is Nothing Then
                _testROSSDataContext = New DataContext(My.Resources.TestWebApp)
            End If
            Return _testROSSDataContext
        Else
            If _liveROSSDataContext Is Nothing Then
                _liveROSSDataContext = New DataContext(My.Resources.LiveWebApp)
            End If
            Return _liveROSSDataContext
        End If
    End Function

    Public Shared Function GetWebAppUserByName(ByVal firstName As String, ByVal lastName As String) As List(Of WebAppUser)
        Dim query As String = String.Format("SELECT Ui.Username, " + _
                                             "Ui.EmailAddress, " + _
                                             "St.FirstName, " + _
                                             "St.LastName, " + _
                                             "St.StateStudentId, " + _
                                             "St.Phone as PhoneNumber, " + _
                                             "Coalesce(Sa.Address1, '') as Address1, " + _
                                             "Coalesce(Sa.Address2, '') as Address2, " + _
                                             "Coalesce(Sa.City, '') as City, " + _
                                             "Coalesce(Sl.Abbreviation, '') as [State], " + _
                                             "Coalesce(Sa.Zip, '') as Zip, " + _
                                             "Coalesce(Sa.Country, '') as Country " + _
                                            "FROM dbo.UserInfo Ui " + _
                                            "INNER JOIN dbo.Student St " + _
                                             "ON Ui.Username = St.Username " + _
                                            "LEFT JOIN dbo.StudentAddress Sa " + _
                                             "ON Ui.Username = Sa.Username " + _
                                            "LEFT JOIN dbo.StateLookup Sl " + _
                                             "ON Sa.StateCode = Sl.Code " + _
                                            "WHERE St.FirstName = '{0}' AND St.LastName = '{1}'", firstName, lastName)
        Return ROSSDataContext(Constants.TEST_MODE).ExecuteQuery(Of WebAppUser)(query).ToList()
    End Function

    Public Shared Function GetWebAppUserByEmail(ByVal emailAddress As String) As WebAppUser
        Dim query As String = String.Format("SELECT Ui.Username, " + _
                                             "Ui.EmailAddress, " + _
                                             "St.FirstName, " + _
                                             "St.LastName, " + _
                                             "St.StateStudentId, " + _
                                             "St.Phone as PhoneNumber, " + _
                                             "Coalesce(Sa.Address1, '') as Address1, " + _
                                             "Coalesce(Sa.Address2, '') as Address2, " + _
                                             "Coalesce(Sa.City, '') as City, " + _
                                             "Coalesce(Sl.Abbreviation, '') as [State], " + _
                                             "Coalesce(Sa.Zip, '') as Zip, " + _
                                             "Coalesce(Sa.Country, '') as Country " + _
                                            "FROM dbo.UserInfo Ui " + _
                                            "INNER JOIN dbo.Student St " + _
                                             "ON Ui.Username = St.Username " + _
                                            "LEFT JOIN dbo.StudentAddress Sa " + _
                                             "ON Ui.Username = Sa.Username " + _
                                            "LEFT JOIN dbo.StateLookup Sl " + _
                                             "ON Sa.StateCode = Sl.Code " + _
                                            "WHERE Ui.EmailAddress = '{0}'", emailAddress)
        Return ROSSDataContext(Constants.TEST_MODE).ExecuteQuery(Of WebAppUser)(query).SingleOrDefault()
    End Function

    Public Shared Function GetWebAppUserByUserId(ByVal userId As String) As WebAppUser
        Dim query As String = String.Format("SELECT Ui.Username, " + _
                                             "Ui.EmailAddress, " + _
                                             "St.FirstName, " + _
                                             "St.LastName, " + _
                                             "St.StateStudentId, " + _
                                             "St.Phone as PhoneNumber, " + _
                                             "Coalesce(Sa.Address1, '') as Address1, " + _
                                             "Coalesce(Sa.Address2, '') as Address2, " + _
                                             "Coalesce(Sa.City, '') as City, " + _
                                             "Coalesce(Sl.Abbreviation, '') as [State], " + _
                                             "Coalesce(Sa.Zip, '') as Zip, " + _
                                             "Coalesce(Sa.Country, '') as Country " + _
                                            "FROM dbo.UserInfo Ui " + _
                                            "INNER JOIN dbo.Student St " + _
                                             "ON Ui.Username = St.Username " + _
                                            "LEFT JOIN dbo.StudentAddress Sa " + _
                                             "ON Ui.Username = Sa.Username " + _
                                            "LEFT JOIN dbo.StateLookup Sl " + _
                                             "ON Sa.StateCode = Sl.Code " + _
                                            "WHERE Ui.Username = '{0}'", userId)
        Return ROSSDataContext(Constants.TEST_MODE).ExecuteQuery(Of WebAppUser)(query).SingleOrDefault()
    End Function

    Public Shared Sub UpdateWebAppUserPassword(ByVal user As WebAppUser)
        Dim query As String = String.Format("UPDATE dbo.UserInfo SET PasswordHash = '{0}', MustReset = 1 WHERE Username = '{1}'", user.GetAndEncryptPassword(), user.Username)
        ROSSDataContext(Constants.TEST_MODE).ExecuteCommand(query)
    End Sub
End Class
