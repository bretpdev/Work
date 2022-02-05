Public Class Installment
    Private _amount As Double = 0
    Public Property Amount() As Double
        Get
            Return _amount
        End Get
        Set(ByVal value As Double)
            _amount = value
        End Set
    End Property

    Private _firstDueDate As DateTime = DateTime.MinValue
    Public Property FirstDueDate() As DateTime
        Get
            Return _firstDueDate
        End Get
        Set(ByVal value As DateTime)
            _firstDueDate = value
        End Set
    End Property
End Class
