Public Class Communication
    Private _entityID As String
    Public Property EntityID() As String
        Get
            Return _entityID
        End Get
        Set(ByVal value As String)
            _entityID = value
        End Set
    End Property

    Private _entityType As String
    Public Property EntityType() As String
        Get
            Return _entityType
        End Get
        Set(ByVal value As String)
            _entityType = value
        End Set
    End Property

    Private _is411 As Boolean
    Public Property Is411() As Boolean
        Get
            Return _is411
        End Get
        Set(ByVal value As Boolean)
            _is411 = value
        End Set
    End Property

    Private _source As String
    Public Property Source() As String
        Get
            Return _source
        End Get
        Set(ByVal value As String)
            _source = value
        End Set
    End Property

    Private _subject As String
    Public Property Subject() As String
        Get
            Return _subject
        End Get
        Set(ByVal value As String)
            _subject = value
        End Set
    End Property

    Private _text As String
    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    Private _timeStamp As DateTime
    Public Property TimeStamp() As DateTime
        Get
            Return _timeStamp
        End Get
        Set(ByVal value As DateTime)
            _timeStamp = value
        End Set
    End Property

    Private _type As String
    Public Property Type() As String
        Get
            Return _type
        End Get
        Set(ByVal value As String)
            _type = value
        End Set
    End Property

    Private _userId As String
    Public Property UserId() As String
        Get
            Return _userId
        End Get
        Set(ByVal value As String)
            _userId = value
        End Set
    End Property
End Class
