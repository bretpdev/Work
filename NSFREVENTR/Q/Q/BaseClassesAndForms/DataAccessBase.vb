Imports System.Collections.Generic
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Text
Imports System.DirectoryServices

Public Class DataAccessBase
    Private Const BSYS_LIVE_CONNECTION_STRING As String = "Data Source=Nochouse;Initial Catalog=BSYS;Integrated Security=SSPI;"
    Private Const BSYS_OPSDEV_CONNECTION_STRING As String = "Data Source=OPSDEV;Initial Catalog=BSYS;Integrated Security=SSPI;"
    Private Const NORAD_LIVE_CONNECTION_STRING As String = "Data Source=Nochouse;Initial Catalog=NORAD;User Id={0};Password={1};"
    Private Const NORAD_TEST_CONNECTION_STRING As String = "Data Source=OPSDEV;Initial Catalog=NORAD;User Id={0};Password={1};"

    Private Const CSYS_LIVE_CONNECTION_STRING As String = "Data Source=Nochouse;Initial Catalog=CSYS;Integrated Security=SSPI;"
    Private Const CSYS_QA_CONNECTION_STRING As String = "Data Source=NochouseQA;Initial Catalog=CSYS;Integrated Security=SSPI;"
    Private Const CSYS_TEST_CONNECTION_STRING As String = "Data Source=NochouseTEST;Initial Catalog=CSYS;Integrated Security=SSPI;"
    Private Const CSYS_DEV_CONNECTION_STRING As String = "Data Source=OPSDEV;Initial Catalog=CSYS;Integrated Security=SSPI;"
    Private Const CDW_LIVE_CONNECTION_STRING As String = "Data Source=UHEAASQLDB;Initial Catalog=CDW;Integrated Security=SSPI;"
    Private Const CDW_TEST_CONNECTION_STRING As String = "Data Source=OPSDEV;Initial Catalog=CDW;Integrated Security=SSPI;"
    Private Const CLS_LIVE_CONNECTION_STRING As String = "Data Source=UHEAASQLDB;Initial Catalog=CLS;Integrated Security=SSPI;"
    Private Const CLS_TEST_CONNECTION_STRING As String = "Data Source=OPSDEV;Initial Catalog=CLS;Integrated Security=SSPI;"
    Private Const UDW_LIVE_CONNECTION_STRING As String = "Data Source=UHEAASQLDB;Initial Catalog=UDW;Integrated Security=SSPI;"
    Private Const UDW_TEST_CONNECTION_STRING As String = "Data Source=OPSDEV;Initial Catalog=UDW;Integrated Security=SSPI;"
    Private Const ULS_LIVE_CONNECTION_STRING As String = "Data Source=UHEAASQLDB;Initial Catalog=ULS;Integrated Security=SSPI;"
    Private Const ULS_TEST_CONNECTION_STRING As String = "Data Source=OPSDEV;Initial Catalog=ULS;Integrated Security=SSPI;"
    Private Const BATCHPROCESSING_TEST_CONNECTION As String = "Data Source=OPSDEV;Initial Catalog=BatchProcessing;Integrated Security=SSPI;"
    Private Const BATCHPROCESSING_LIVE_CONNECTION As String = "Data Source=UHEAASQLDB;Initial Catalog=BatchProcessing;Integrated Security=SSPI;"
    Private Const REPORTING_LIVE_CONNECTION As String = "Data Source=Nochouse;Initial Catalog=Reporting;Integrated Security=SSPI;"
    Private Const REPORTING_QA_CONNECTION As String = "Data Source=NochouseQA;Initial Catalog=Reporting;Integrated Security=SSPI;"
    Private Const REPORTING_TEST_CONNECTION As String = "Data Source=NochouseTest;Initial Catalog=Reporting;Integrated Security=SSPI;"
    Private Const REPORTING_DEV_CONNECTION As String = "Data Source=OPSDEV;Initial Catalog=Reporting;Integrated Security=SSPI;"


    Protected Const OSCS_LIVE_CONNECTION_STRING As String = "Data Source=Nochouse;Initial Catalog=OperationsStatisticsCornerStone;Integrated Security=SSPI;"
    Protected Const OSCS_TEST_CONNECTION_STRING As String = "Data Source=OPSDEV;Initial Catalog=OperationsStatisticsCornerStone;Integrated Security=SSPI;"


    Protected Shared Function OscsDataContext(ByVal testMode As Boolean) As DataContext
        Return New DataContext(If(testMode, OSCS_TEST_CONNECTION_STRING, OSCS_LIVE_CONNECTION_STRING))
    End Function

    <FlagsAttribute()> _
    Public Enum EmailLookupOption
        None = 0
        ErrorOnEmpty = 1
    End Enum

    Public Enum ConfigurationMode
        Live = 0
        QA = 1
        Test = 2
        Dev = 3
    End Enum

#Region "Properties for commonly used directories"

    ''' <summary>
    ''' DEPRECATED!
    ''' Use EnterpriseFileSystem.TempFolder instead.
    ''' </summary>
    <Obsolete("Use EnterpriseFileSystem.TempFolder instead.")> _
    Public Shared ReadOnly Property PersonalDataDirectory() As String
        Get
            Return "T:\"
        End Get
    End Property

    ''' <summary>
    ''' DEPRECATED!
    ''' Use EnterpriseFileSystem.FtpFolder instead.
    ''' </summary>
    <Obsolete("Use EnterpriseFileSystem.FtpFolder instead.")> _
    Public Shared ReadOnly Property SASDataFileDirectory() As String
        Get
            Return "X:\PADD\FTP\"
        End Get
    End Property

    ''' <summary>
    ''' DEPRECATED!
    ''' Use EnterpriseFileSystem.LogsFolder instead.
    ''' </summary>
    <Obsolete("Use EnterpriseFileSystem.LogsFolder instead.")> _
    Public Shared ReadOnly Property RecoveryLogDirectory() As String
        Get
            Return "X:\PADD\Logs\"
        End Get
    End Property

#End Region

    Protected Shared Function BatchProcessingDataContext(ByVal testMode As Boolean) As DataContext
        Return New DataContext(If(testMode, BATCHPROCESSING_TEST_CONNECTION, BATCHPROCESSING_LIVE_CONNECTION))
    End Function
    'TODO: Only a few scripts are using BSYS on OpsDev. Let's change them to use ULS and get rid of this DataContext.
    <Obsolete("Migrate to ULS")> _
    Protected Shared Function BsysDataContextOPSDEVEnabled(ByVal testMode As Boolean) As DataContext
        Return New DataContext(If(testMode, BSYS_OPSDEV_CONNECTION_STRING, BSYS_LIVE_CONNECTION_STRING))
    End Function

    <Obsolete("BSYS will eventually be retired, use CSYS if you can or make sure the script gets added to the list of BSYS scripts")> _
    Protected Shared Function BsysDataContext(ByVal testMode As Boolean) As DataContext
        Return New DataContext(If(testMode, BSYS_OPSDEV_CONNECTION_STRING, BSYS_LIVE_CONNECTION_STRING))
    End Function




    Protected Shared Function CSYSDataContext(ByVal testMode As Boolean) As DataContext
        Return New DataContext(If(testMode, CSYS_DEV_CONNECTION_STRING, CSYS_LIVE_CONNECTION_STRING))
    End Function

    ''' <summary>
    ''' Creates a new DataContext
    ''' </summary>
    ''' <param name="mode">ConfigurationMode</param>
    ''' <returns>New DataContext for the given mode</returns>
    ''' <remarks>Default is Dev</remarks>
    Protected Shared Function CSYSDataContext(ByVal mode As ConfigurationMode) As DataContext
        Select Case mode
            Case ConfigurationMode.Live
                Return New DataContext(CSYS_LIVE_CONNECTION_STRING)
            Case ConfigurationMode.QA
                Return New DataContext(CSYS_QA_CONNECTION_STRING)
            Case ConfigurationMode.Test
                Return New DataContext(CSYS_TEST_CONNECTION_STRING)
            Case Else
                Return New DataContext(CSYS_DEV_CONNECTION_STRING)
        End Select
    End Function

    ''' <summary>
    ''' Creates a new Reporting DataContext
    ''' </summary>m
    ''' <param name="mode">COnfigurationMode</param>
    ''' <returns>New DataContext for the given mode</returns>
    ''' <remarks>Default is Dev mode</remarks>
    Protected Shared Function ReportingDataContext(ByVal mode As ConfigurationMode) As DataContext
        Select Case mode
            Case ConfigurationMode.Live
                Return New DataContext(REPORTING_LIVE_CONNECTION)
            Case ConfigurationMode.QA
                Return New DataContext(REPORTING_QA_CONNECTION)
            Case ConfigurationMode.Test
                Return New DataContext(REPORTING_TEST_CONNECTION)
            Case Else
                Return New DataContext(REPORTING_DEV_CONNECTION)
        End Select
    End Function

    Protected Shared Function ClsDataContext(ByVal testMode As Boolean) As DataContext
        Return New DataContext(If(testMode, CLS_TEST_CONNECTION_STRING, CLS_LIVE_CONNECTION_STRING))
    End Function

    Protected Shared Function CDWDataContext(ByVal testMode As Boolean) As DataContext
        Return New DataContext(If(testMode, CDW_TEST_CONNECTION_STRING, CDW_LIVE_CONNECTION_STRING))
    End Function

    Protected Shared Function UlsDataContext(ByVal testMode As Boolean) As DataContext
        Return New DataContext(If(testMode, ULS_TEST_CONNECTION_STRING, ULS_LIVE_CONNECTION_STRING))
    End Function

    Protected Shared Function UDWDataContext(ByVal testMode As Boolean) As DataContext
        Return New DataContext(If(testMode, UDW_TEST_CONNECTION_STRING, UDW_LIVE_CONNECTION_STRING))
    End Function








    ''' <summary>
    ''' Returns a collection of all business units from CSYS.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    <Obsolete("This will return a list of Business Units in a testMode, use the new function that takes a ConfigurationMode")> _
    Public Shared Function GetBusinessUnits(ByVal testMode As Boolean) As IEnumerable(Of BusinessUnit)
        Return CSYSDataContext(testMode).ExecuteQuery(Of BusinessUnit)("EXEC spGENR_GetBusinessUnits").ToList()
    End Function

    ''' <summary>
    ''' Returns a collection of all business units from CSYS.
    ''' </summary>
    ''' <param name="mode">Sets up the configuration mode </param>
    Public Shared Function GetBusinessUnits(ByVal mode As ConfigurationMode) As IEnumerable(Of BusinessUnit)
        Return CSYSDataContext(mode).ExecuteQuery(Of BusinessUnit)("EXEC spGENR_GetBusinessUnits").ToList()
    End Function

    ''' <summary>
    ''' Returns a List of e-mail addresses from GENR_REF_MiscEmailNotif that match the given type key.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="typeKey">The type key associated with the desired e-mail recipients.</param>
    ''' <param name="options">Bit-wise flags for fine-tuning the method's behavior.</param>
    ''' <remarks>
    ''' Recipients who don't have an "@" sign in the table will have "@utahsbr.edu" appended by this method.
    ''' If the type key is not found in the table, setting the ErrorOnEmpty option will cause
    ''' the method to throw an exception; otherwise, the method will return an empty string.
    ''' </remarks>
    <Obsolete("Uses BSYS data which is being replaced by CSYS user keys, use GetEmailForKeyList instead.")> _
    Public Shared Function GetEmailRecipienlList(ByVal testMode As Boolean, ByVal typeKey As String, ByVal options As EmailLookupOption) As List(Of String)
        Dim query As String = String.Format("SELECT WinUName FROM GENR_REF_MiscEmailNotif WHERE TypeKey = '{0}'", typeKey)
        Dim userNames As List(Of String) = BsysDataContext(testMode).ExecuteQuery(Of String)(query).ToList()

        'See if we need to respond to any passed-in options.
        If (userNames.Count = 0 AndAlso (options And EmailLookupOption.ErrorOnEmpty = EmailLookupOption.ErrorOnEmpty)) Then
            Dim message As String = String.Format("The TypeKey ""{0}"" was not found in the table.", typeKey)
            Throw New Exception(message)
        End If

        'Append "@utahsbr.edu" if no domain is given in the table.
        For Each userName As String In userNames
            If (Not userName.Contains("@")) Then userName += "@utahsbr.edu"
        Next

        Return userNames
    End Function

    ''' <summary>
    ''' Returns a comma-delimited string of e-mail addresses from GENR_REF_MiscEmailNotif that match the given type key.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="typeKey">The type key associated with the desired e-mail recipients.</param>
    ''' <param name="options">Bit-wise flags for fine-tuning the method's behavior.</param>
    ''' <remarks>
    ''' Recipients who don't have an "@" sign in the table will have "@utahsbr.edu" appended by this method.
    ''' In the case that the type key is not found in the table, setting the ErrorOnEmpty option
    ''' will cause the method to throw an exception; otherwise, the method will return an empty string.
    ''' </remarks>
    <Obsolete("Uses BSYS data which is being replaced by CSYS user keys, use GetEmailForKeyString instead.")> _
    Public Shared Function GetEmailRecipientString(ByVal testMode As Boolean, ByVal typeKey As String, ByVal options As EmailLookupOption) As String
        Dim userNames As List(Of String) = GetEmailRecipienlList(testMode, typeKey, options)
        Return String.Join(",", userNames.ToArray())
    End Function

    ''' <summary>
    ''' Returns a list of e-mail addresss for users with the specified user notification key
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="userKey">The user key associated with the desired e-mail recipients.</param>
    ''' <param name="businessUnit">The ID of the business unit for which the key must apply. A business unit ID of 0 indicates no filtering should be done on business unit.</param>
    ''' <param name="options">Bit-wise flags for fine-tuning the method's behavior.</param>
    ''' <remarks>
    ''' In the case that the type key is not found in the table, setting the ErrorOnEmpty option
    ''' will cause the method to throw an exception; otherwise, the method will return an empty string.
    ''' </remarks>
    Public Shared Function GetEmailForKeyList(ByVal testMode As Boolean, ByVal userKey As String, ByVal businessUnit As Integer, ByVal options As EmailLookupOption) As List(Of String)
        Dim sprocCommand As New SprocCommandBuilder("spSYSA_GetEmailForKey")
        sprocCommand.AddParameter("Key", userKey)
        If businessUnit > 0 Then sprocCommand.AddParameter("BU", businessUnit)
        Dim eMailAddresses As List(Of String) = CSYSDataContext(testMode).ExecuteQuery(Of String)(sprocCommand.Command, sprocCommand.ParameterValues).ToList()

        'See if we need to respond to any passed-in options.
        If (eMailAddresses.Count = 0 AndAlso (options And EmailLookupOption.ErrorOnEmpty = EmailLookupOption.ErrorOnEmpty)) Then
            Dim message As String = String.Format("The user key ""{0}"" was not found in the table.", userKey)
            Throw New Exception(message)
        End If

        Return eMailAddresses
    End Function

    ''' <summary>
    ''' Returns a comma-delimited string of e-mail addresss for users with the specified user notification key
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="userKey">The user key associated with the desired e-mail recipients.</param>
    ''' <param name="businessUnit">The ID of the business unit for which the key must apply. A business unit ID of 0 indicates no filtering should be done on business unit.</param>
    ''' <param name="options">Bit-wise flags for fine-tuning the method's behavior.</param>
    ''' <remarks>
    ''' In the case that the type key is not found in the table, setting the ErrorOnEmpty option
    ''' will cause the method to throw an exception; otherwise, the method will return an empty string.
    ''' </remarks>
    Public Shared Function GetEmailForKeyString(ByVal testMode As Boolean, ByVal userKey As String, ByVal businessUnit As Integer, ByVal options As EmailLookupOption) As String
        Dim eMailAddresses As List(Of String) = GetEmailForKeyList(testMode, userKey, businessUnit, options)
        Return String.Join(",", eMailAddresses.ToArray())
    End Function







    ''' <summary>
    ''' Returns a collection of two-letter state codes in alphabetic order.
    ''' </summary>
    ''' <param name="includeTerritories">
    ''' If true, codes for U.S. territories (such as Guam, Marshall Islands) will be included.
    ''' If false, only the standard 50 states plus DC are included.
    ''' </param>
    ''' <remarks>We may as well have this return a List(Of String) to reassure clients about its behavior.</remarks>
    Public Shared Function GetStateCodes(ByVal includeTerritories As Boolean) As IEnumerable(Of String)
        Return CSYSDataContext(False).ExecuteQuery(Of String)("EXEC spGENR_GetStateCodes {0}", includeTerritories).ToList()
    End Function

    ''' <summary>
    ''' Returns the name of the state for the state code provided
    ''' </summary>
    ''' <param name="stateCode"></param>
    ''' The state code for which the state name is needed
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetStateNameForStateCode(ByVal mode As ConfigurationMode, ByVal stateCode As String) As String
        Return CSYSDataContext(mode).ExecuteQuery(Of String)("EXEC spGENR_GetStateNameForStateCode {0}", stateCode).SingleOrDefault()
    End Function

    ''' <summary>
    ''' Returns a list of address source code descriptions
    ''' </summary>
    ''' <param name="mode"></param>
    ''' The configuration mode
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAddressSourceDescriptions(ByVal mode As ConfigurationMode) As IEnumerable(Of String)
        Return CSYSDataContext(mode).ExecuteQuery(Of String)("EXEC spGENR_GetAddressSourceDescriptions").ToList()
    End Function

    ''' <summary>
    ''' Returns the address source code for the address source description provided
    ''' </summary>
    ''' <param name="mode"></param>
    ''' The configuration mode
    ''' <param name="description"></param>
    ''' The description of the address source code that is needed
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAddressSourceCodeForDescription(ByVal mode As ConfigurationMode, ByVal description As String) As String
        Return CSYSDataContext(mode).ExecuteQuery(Of String)("EXEC spGENR_GetAddressSourceCodeForDescription {0}", description).SingleOrDefault()
    End Function

    ''' <summary>
    ''' Returns a list of country names
    ''' </summary>
    ''' <param name="mode"></param>
    ''' The configuration mode
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCountryNames(ByVal mode As ConfigurationMode) As IEnumerable(Of String)
        Return CSYSDataContext(mode).ExecuteQuery(Of String)("EXEC spGENR_GetCountryNames").ToList()
    End Function

    ''' <summary>
    ''' Returns the code of the country for the name provided
    ''' </summary>
    ''' <param name="mode"></param>
    ''' The configuration mode
    ''' <param name="name"></param>
    ''' The country name for which the country code is needed
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCountryCodeForName(ByVal mode As ConfigurationMode, ByVal name As String) As String
        Return CSYSDataContext(mode).ExecuteQuery(Of String)("EXEC spGENR_GetCountryCodeForName {0}", name).SingleOrDefault()
    End Function

    ''' <summary>
    ''' Returns the description of the relationship for the code provided
    ''' </summary>
    ''' <param name="mode"></param>
    ''' The configuration mode
    ''' <param name="code"></param>
    ''' The relationship code for which the description name is needed
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRelationshipDescriptionForCode(ByVal mode As ConfigurationMode, ByVal code As String) As String
        Return CSYSDataContext(mode).ExecuteQuery(Of String)("EXEC spGENR_GetRelationshipDescriptionForCode {0}", code).SingleOrDefault()
    End Function

    ''' <summary>
    ''' Returns a list of relationship descriptions
    ''' </summary>
    ''' <param name="mode"></param>
    ''' The configuration mode
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRelationshipDescriptions(ByVal mode As ConfigurationMode) As IEnumerable(Of String)
        Return CSYSDataContext(mode).ExecuteQuery(Of String)("EXEC spGENR_GetRelationshipDescriptions").ToList()
    End Function

    ''' <summary>
    ''' Returns the relationship code for the relationship description provided
    ''' </summary>
    ''' <param name="mode"></param>
    ''' The configuration mode
    ''' <param name="description"></param>
    ''' The relationship description
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRelationshipCodeForDescription(ByVal mode As ConfigurationMode, ByVal description As String) As String
        Return CSYSDataContext(mode).ExecuteQuery(Of String)("EXEC spGENR_GetRelationshipCodeForDescription {0}", description).SingleOrDefault()
    End Function

    ''' <summary>
    ''' Returns a list of suffixes
    ''' </summary>
    ''' <param name="mode"></param>
    ''' The configuration mode
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetSuffixes(ByVal mode As ConfigurationMode) As IEnumerable(Of String)
        Return CSYSDataContext(mode).ExecuteQuery(Of String)("EXEC spGENR_GetSuffixes").ToList()
    End Function

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Use the GetSqlUser() method instead and use the FirstName and LastName properties (or the ToString() method) from the resulting SqlUser object.
    ''' </summary>
    <Obsolete("Use the GetSqlUser() method instead")> _
    Public Shared Function GetUsersName(ByVal testMode As Boolean) As String
        Dim query As String = "SELECT FirstName + ' ' + LastName as FullName FROM SYSA_LST_Users WHERE WindowsUserName = {0}"
        Return BsysDataContext(testMode).ExecuteQuery(Of String)(query, Environment.UserName).SingleOrDefault()
    End Function





    ''' <summary>
    ''' DEPRECATED!!!
    ''' Migrate all authorization to Active Directory, and use the IsAuthorizedInActiveDirectory() method to check authorization.
    ''' </summary>
    <Obsolete("Migrate all authorization to Active Directory, and use the IsAuthorizedInActiveDirectory() method to check authorization")> _
    Public Shared Function IsAuthorizedInBSYSAuthAccessTable(ByVal testMode As Boolean, ByVal typeKey As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM GENR_REF_AuthAccess WHERE WinUName = {0} AND TypeKey = {1}"
        Return (BsysDataContext(testMode).ExecuteQuery(Of Integer)(query, Environment.UserName, typeKey).Single() > 0)
    End Function

    ''' <summary>
    ''' Checks Active Directory to determine if the user is part of a group (role) authorized for the specified key
    ''' </summary>
    ''' <param name="testMode">True if running in test mode</param>
    ''' <param name="key">The user key assigned to the script</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsAuthorizedInActiveDirectory(ByVal testMode As Boolean, ByVal key As String) As Boolean
        Dim searchEntry As DirectoryEntry = New DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local")
        Dim searcher As DirectorySearcher = New DirectorySearcher()
        searcher.SearchRoot = searchEntry
        searcher.Filter = String.Format("SAMAccountName={0}", Environment.UserName)
        Dim result As SearchResult = searcher.FindOne()

        If Not result.Equals(Nothing) Then
            Dim attributes As ResultPropertyCollection = result.Properties
            'get a list of roles (Active Directory groups) to which the key is assigned
            Dim roles As List(Of String) = CSYSDataContext(testMode).ExecuteQuery(Of String)("EXEC dbo.spSYSA_GetRolesForKey {0}", key).ToList

            'return True if the user is found in one of the roles to which the key is assigned
            For Each role As String In roles
                If ((From p In attributes("memberOf").OfType(Of String)() Where (p.ToLowerInvariant()).Contains(role.ToLowerInvariant()) Select (p)).Count()) > 0 Then Return True
            Next
        End If

        Return False
    End Function



    ''' <summary>
    ''' Checks whether the current user is assigned a key, without regard to business unit.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="key">The user key (from the CSYS UserKey table) to check for.</param>
    <Obsolete("Migrate all authorization to Active Directory, and use the IsAuthorizedInActiveDirectory() method to check authorization")> _
    Public Shared Function HasAccess(ByVal testMode As Boolean, ByVal key As String) As Boolean
        Return HasAccess(testMode, 0, key, 0)
    End Function

    ''' <summary>
    ''' Checks whether a SqlUser is assigned a key, without regard to business unit.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="sqlUserId">The SQL user ID of the user in question.</param>
    ''' <param name="key">The user key (from the CSYS UserKey table) to check for.</param>
    <Obsolete("Migrate all authorization to Active Directory, and use the IsAuthorizedInActiveDirectory() method to check authorization")> _
    Public Shared Function HasAccess(ByVal testMode As Boolean, ByVal sqlUserId As Integer, ByVal key As String) As Boolean
        Return HasAccess(testMode, sqlUserId, key, 0)
    End Function

    ''' <summary>
    ''' Checks whether the current user is assigned a key for a business unit.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="key">The user key (from the CSYS UserKey table) to check for.</param>
    ''' <param name="businessUnit">The ID of the business unit for which the key assignment must apply.</param>
    <Obsolete("Migrate all authorization to Active Directory, and use the IsAuthorizedInActiveDirectory() method to check authorization")> _
    Public Shared Function HasAccess(ByVal testMode As Boolean, ByVal key As String, ByVal businessUnit As Integer) As Boolean
        Return HasAccess(testMode, 0, key, businessUnit)
    End Function

    ''' <summary>
    ''' Checks whether a SqlUser is assigned a key for a business unit.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="sqlUserId">The SQL user ID of the user in question.</param>
    ''' <param name="key">The user key (from the CSYS UserKey table) to check for.</param>
    ''' <param name="businessUnit">The ID of the business unit for which the key assignment must apply.</param>
    <Obsolete("Migrate all authorization to Active Directory, and use the IsAuthorizedInActiveDirectory() method to check authorization")> _
    Public Shared Function HasAccess(ByVal testMode As Boolean, ByVal sqlUserId As Integer, ByVal key As String, ByVal businessUnit As Integer) As Boolean
        Dim query As String = "EXEC spSYSA_UserHasAccess {0},{1},{2},{3}"
        Return (CSYSDataContext(testMode).ExecuteQuery(Of Integer)(query, key, businessUnit, Environment.UserName, sqlUserId).Single() > 0)
    End Function

    ''' <summary>
    ''' Gets invalid values for the specified key
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="valueKey">The key identifying the invalid values.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsInvalidValue(ByVal testMode As Boolean, ByVal valueKey As String, ByVal theValue As String) As Boolean
        Dim query As String = "EXEC spGENR_CheckForInvalidValues {0}, {1}"
        Return (CSYSDataContext(testMode).ExecuteQuery(Of Integer)(query, valueKey, theValue).Single() > 0)
    End Function

    Public Shared Function GetPasswordForBatchProcessing(ByVal testMode As Boolean, ByVal userId As String) As String
        Return (BatchProcessingDataContext(testMode).ExecuteQuery(Of String)("EXEC spGetDecrpytedPassword {0}", userId).SingleOrDefault())
    End Function

#Region "Projection classes"
    Private Class FlattenedBusinessUnitAgent
        Public BusinessUnitId As Integer
        Public SqlUserId As Integer
        Public Role As String




    End Class
#End Region 'Projection classes
End Class
