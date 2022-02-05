Public Class DSCallCenterVolume
    Private _timeStamp As DateTime
    Public Property TimeStamp() As DateTime
        Get
            Return _timeStamp
        End Get
        Set(ByVal value As DateTime)
            _timeStamp = value
        End Set
    End Property

    Private _entityId As String
    Public Property EntityId() As String
        Get
            Return _entityId
        End Get
        Set(ByVal value As String)
            _entityId = value
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

    Private _lastMonth As String
    Public Property LastMonth() As String
        Get
            Return _lastMonth
        End Get
        Set(ByVal value As String)
            _lastMonth = value
        End Set
    End Property
End Class
