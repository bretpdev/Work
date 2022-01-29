Public Class Coordinate
    Private _row As Integer
    Public Property Row() As Integer
        Get
            Return _row
        End Get
        Set(ByVal value As Integer)
            _row = value
        End Set
    End Property

    Private _column As Integer
    Public Property Column() As Integer
        Get
            Return _column
        End Get
        Set(ByVal value As Integer)
            _column = value
        End Set
    End Property
End Class
