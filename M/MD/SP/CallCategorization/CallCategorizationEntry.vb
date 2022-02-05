Public Class CallCategorizationEntry

    Public Sub New()
        _userID = Environment.UserName
    End Sub

    Private _category As String = String.Empty
    ''' <summary>
    ''' Category for db entry
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Category() As String
        Get
            Return _category
        End Get
        Set(ByVal value As String)
            _category = value
        End Set
    End Property

    Private _reason As String = String.Empty
    ''' <summary>
    ''' Reason for db entry
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Reason() As String
        Get
            Return _reason
        End Get
        Set(ByVal value As String)
            _reason = value
        End Set
    End Property

    Private _letterID As String = String.Empty
    ''' <summary>
    ''' Letter ID for db entry
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LetterID() As String
        Get
            Return _letterID
        End Get
        Set(ByVal value As String)
            _letterID = value
        End Set
    End Property

    Private _comments As String = String.Empty
    ''' <summary>
    ''' Comments for db entry
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Comments() As String
        Get
            Return _comments
        End Get
        Set(ByVal value As String)
            _comments = value
        End Set
    End Property

    Private _userID As String
    Public Property UserID() As String
        Get
            Return _userID
        End Get
        Set(ByVal value As String)
            _userID = value
        End Set
    End Property






End Class
