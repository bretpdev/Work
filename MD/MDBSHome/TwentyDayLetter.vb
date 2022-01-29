Public Class TwentyDayLetter
    Private _numberSent As String = ""
    Public Property NumberSent() As String
        Get
            Return _numberSent
        End Get
        Set(ByVal value As String)
            _numberSent = value
        End Set
    End Property

    Private _sentDates(0 To 3) As String
    Public Property SentDates() As String()
        Get
            Return _sentDates
        End Get
        Set(ByVal value As String())
            _sentDates = value
        End Set
    End Property
End Class
