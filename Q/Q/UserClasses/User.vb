Public Class User


    Private _windowsUserName As String
    ''' <summary>
    ''' Windows user ID.  Usually the first letter of the users first name and the users last name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WindowsUserName() As String
        Get
            Return _windowsUserName
        End Get
        Set(ByVal value As String)
            _windowsUserName = value
        End Set
    End Property

    Private _fName As String
    ''' <summary>
    ''' First name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FName() As String
        Get
            Return _fName
        End Get
        Set(ByVal value As String)
            _fName = value
        End Set
    End Property


    Private _lName As String
    ''' <summary>
    ''' Last name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LName() As String
        Get
            Return _lName
        End Get
        Set(ByVal value As String)
            _lName = value
        End Set
    End Property


    Private _mi As String
    ''' <summary>
    ''' Middle initial
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MI() As String
        Get
            Return _mi
        End Get
        Set(ByVal value As String)
            _mi = value
        End Set
    End Property


    Private _ext As String
    ''' <summary>
    ''' Extension
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Ext() As String
        Get
            Return _ext
        End Get
        Set(ByVal value As String)
            _ext = value
        End Set
    End Property


    Private _ext2 As String
    ''' <summary>
    ''' Second extension (if applicable)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Ext2() As String
        Get
            Return _ext2
        End Get
        Set(ByVal value As String)
            _ext2 = value
        End Set
    End Property


    Private _gather4PhnDat As Boolean
    ''' <summary>
    ''' Indicator as to whether phone data is being collect of the user in the phone data data base
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Gather4PhnDat() As Boolean
        Get
            Return _gather4PhnDat
        End Get
        Set(ByVal value As Boolean)
            _gather4PhnDat = value
        End Set
    End Property


    Private _pseudoUser As Boolean
    ''' <summary>
    ''' Indicator as to whether the user is a pseudo user or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PseudoUser() As Boolean
        Get
            Return _pseudoUser
        End Get
        Set(ByVal value As Boolean)
            _pseudoUser = value
        End Set
    End Property


    Private _personalLoanInfoID As String
    ''' <summary>
    ''' The users account number of the AES systems if they are on those systems.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PersonalLoanInfoID() As String
        Get
            Return _personalLoanInfoID
        End Get
        Set(ByVal value As String)
            _personalLoanInfoID = value
        End Set
    End Property


End Class
