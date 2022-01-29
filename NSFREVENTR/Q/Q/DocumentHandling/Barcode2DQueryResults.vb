Public Class Barcode2DQueryResults

    Private _pages As Decimal
    Public Property Pages() As Decimal
        Get
            Return _pages
        End Get
        Set(ByVal value As Decimal)
            _pages = value
        End Set
    End Property

    Private _duplex As Boolean
    Public Property Duplex() As Boolean
        Get
            Return _duplex
        End Get
        Set(ByVal value As Boolean)
            _duplex = value
        End Set
    End Property

End Class
