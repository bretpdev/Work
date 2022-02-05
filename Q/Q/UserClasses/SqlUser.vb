Imports System

Public Class SqlUser
    Implements IEquatable(Of SqlUser)

    Private _id As Integer
    Public Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Private _windowsUserName As String
    Public Property WindowsUserName() As String
        Get
            Return _windowsUserName
        End Get
        Set(ByVal value As String)
            _windowsUserName = value
        End Set
    End Property

    Private _firstName As String
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Private _middleInitial As String
    Public Property MiddleInitial() As String
        Get
            Return _middleInitial
        End Get
        Set(ByVal value As String)
            _middleInitial = value
        End Set
    End Property

    Private _lastName As String
    Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _emailAddress As String
    Public Property EmailAddress() As String
        Get
            Return _emailAddress
        End Get
        Set(ByVal value As String)
            _emailAddress = value
        End Set
    End Property

    Private _title As String
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Private _primaryExtension As String
    Public Property PrimaryExtension() As String
        Get
            Return _primaryExtension
        End Get
        Set(ByVal value As String)
            _primaryExtension = value
        End Set
    End Property

    Private _secondaryExtension As String
    Public Property SecondaryExtension() As String
        Get
            Return _secondaryExtension
        End Get
        Set(ByVal value As String)
            _secondaryExtension = value
        End Set
    End Property

    Private _aesAccountNumber As String
    Public Property AesAccountNumber() As String
        Get
            Return _aesAccountNumber
        End Get
        Set(ByVal value As String)
            _aesAccountNumber = value
        End Set
    End Property

    Private _businessUnit As Integer
    Public Property BusinessUnit() As Integer
        Get
            Return _businessUnit
        End Get
        Set(ByVal value As Integer)
            _businessUnit = value
        End Set
    End Property

    Private _role As Integer
    Public Property Role() As Integer
        Get
            Return _role
        End Get
        Set(ByVal value As Integer)
            _role = value
        End Set
    End Property

    Private _status As String
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _legalName As String
    Public Property LegalName() As String
        Get
            Return FirstName + " " + LastName
        End Get
        Set(ByVal value As String)
            _legalName = value
        End Set
    End Property

    Public Sub New()
        ID = 0
        WindowsUserName = ""
        FirstName = ""
        MiddleInitial = ""
        LastName = ""
        EmailAddress = ""
        Title = ""
        PrimaryExtension = ""
        SecondaryExtension = ""
        AesAccountNumber = ""
        Role = 0
        Status = ""
        LegalName = ""
    End Sub

    Public Sub New(ByVal id As Integer, ByVal windowsUserName As String, ByVal firstName As String, ByVal middleInitial As String, ByVal lastName As String, ByVal email As String, ByVal title As String, ByVal primaryExt As String, ByVal secondaryExt As String, ByVal aesAccountNum As String, ByVal Role As Integer, ByVal status As String, ByVal legasName As String)
        id = id
        windowsUserName = windowsUserName
        firstName = firstName
        middleInitial = middleInitial
        lastName = lastName
        EmailAddress = email
        Title = Title
        PrimaryExtension = primaryExt
        SecondaryExtension = secondaryExt
        AesAccountNumber = aesAccountNum
        Role = Role
        status = status
        LegalName = LegalName
    End Sub


    ''' <summary>
    ''' Returns the user's first and last name.
    ''' </summary>
    Public Overrides Function ToString() As String
        Return String.Format("{0} {1}", FirstName, LastName)
    End Function

    Public Shared Operator =(ByVal a As SqlUser, ByVal b As SqlUser) As Boolean
        If (Object.ReferenceEquals(a, b)) Then Return True
        If (CType(a, Object) Is Nothing OrElse CType(b, Object) Is Nothing) Then Return False
        Return a.ID = b.ID
    End Operator

    Public Shared Operator <>(ByVal a As SqlUser, ByVal b As SqlUser) As Boolean
        Return ((a = b) = False)
    End Operator

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Dim other As SqlUser = TryCast(obj, SqlUser)
        If (other Is Nothing) Then Return False
        Return MyBase.Equals(obj) AndAlso Me.ID = other.ID
    End Function

#Region "IEquatable(Of SqlUser) Members"

    Public Overloads Function Equals(ByVal other As SqlUser) As Boolean Implements IEquatable(Of SqlUser).Equals
        If ID = 0 OrElse other.ID = 0 Then Return False
        Return ID.Equals(other.ID)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return ID.GetHashCode()
    End Function

#End Region
End Class
