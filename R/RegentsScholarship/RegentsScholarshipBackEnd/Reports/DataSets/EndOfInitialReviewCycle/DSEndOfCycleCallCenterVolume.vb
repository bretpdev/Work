Public Class DSEndOfCycleCallCenterVolume
    Private _entityId As String
    Public Property EntityId() As String
        Get
            Return _entityId
        End Get
        Set(ByVal value As String)
            _entityId = value
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

    Private _userId As String
    Public Property userId() As String
        Get
            Return _userId
        End Get
        Set(ByVal value As String)
            _userId = value
        End Set
    End Property
End Class
