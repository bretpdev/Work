Public Class Grade
    Private _letter As String
    Private _gpaValue As Double

    Public Property Letter() As String
        Get
            Return _letter
        End Get
        Set(ByVal value As String)
            _letter = value
        End Set
    End Property

    Public Property GpaValue() As Double
        Get
            Return _gpaValue
        End Get
        Set(ByVal value As Double)
            _gpaValue = value
        End Set
    End Property
End Class
