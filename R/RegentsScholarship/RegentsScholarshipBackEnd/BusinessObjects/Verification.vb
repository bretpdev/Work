Public Class Verification
    Private _userId As String
    Private _timeStamp As Date

    Public Property UserId() As String
        Get
            Return _userId
        End Get
        Set(ByVal value As String)
            _userId = value
        End Set
    End Property

    Public Property TimeStamp() As Date
        Get
            Return _timeStamp
        End Get
        Set(ByVal value As Date)
            _timeStamp = value
        End Set
    End Property
End Class
