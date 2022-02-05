Imports Q.DocumentHandling

Public Class CentralizedPrintingAnd2DBarcodeInfo

#Region "Properties"

    Private _usersBusinessUnit As String = ""
    Public Property UsersBusinessUnit() As String
        Get
            Return _usersBusinessUnit
        End Get
        Set(ByVal value As String)
            _usersBusinessUnit = value
        End Set
    End Property

    Private _usersOriginallyGatheredBusinessUnit As String = ""
    Public Property OriginallyGatheredBusinessUnit() As String
        Get
            Return _usersOriginallyGatheredBusinessUnit
        End Get
        Set(ByVal value As String)
            _usersOriginallyGatheredBusinessUnit = value
        End Set
    End Property

    Private _usersBuSentFromEmail As String = ""
    Public Property UsersBuSentFromEmail() As String
        Get
            Return _usersBuSentFromEmail
        End Get
        Set(ByVal value As String)
            _usersBuSentFromEmail = value
        End Set
    End Property

    Private _usersFirstName As String = ""
    Public Property UsersFirstName() As String
        Get
            Return _usersFirstName
        End Get
        Set(ByVal value As String)
            _usersFirstName = value
        End Set
    End Property

    Private _brwDemographics As SystemBorrowerDemographics
    Public Property BrwDemographics() As SystemBorrowerDemographics
        Get
            Return _brwDemographics
        End Get
        Set(ByVal value As SystemBorrowerDemographics)
            _brwDemographics = value
        End Set
    End Property

#End Region

    Public Sub ContinueInitializing(ByVal ri As ReflectionInterface, ByVal SSN As String, ByVal systemRegion As ScriptBase.Region)
        If systemRegion = ScriptSessionBase.Region.UHEAA Then
            Try
                _brwDemographics = Common.GetDemographicsFromLP22(ri, SSN)
            Catch ex As Exception
                _brwDemographics = Common.GetDemographicsFromTX1J(ri, SSN, True)
            End Try
        Else
            _brwDemographics = Common.GetDemographicsFromTX1J(ri, SSN, True)
        End If
    End Sub

End Class
