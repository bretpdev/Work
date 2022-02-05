Public Class BorrowerContactPhone

    Private _borrower As SP.Borrower
    Public ReadOnly Property Borrower() As SP.Borrower
        Get
            Return _borrower
        End Get
    End Property


    Private _contactPhoneNumber As String
    Public Property ContactPhoneNumber() As String
        Get
            Return _contactPhoneNumber
        End Get
        Set(ByVal value As String)
            _contactPhoneNumber = value
        End Set
    End Property

    Public Sub New(ByVal Bor As SP.Borrower)
        _borrower = Bor
    End Sub

End Class
