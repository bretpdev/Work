Public Class Payment
    Private _amount As Double = 0
    Public Property Amount() As Double
        Get
            Return _amount
        End Get
        Set(ByVal value As Double)
            _amount = value
        End Set
    End Property

    Private _receivedDate As String = ""
    Public Property ReceivedDate() As String
        Get
            Return _receivedDate
        End Get
        Set(ByVal value As String)
            _receivedDate = value
        End Set
    End Property
End Class
