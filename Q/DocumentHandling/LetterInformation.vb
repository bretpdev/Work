Public Class LetterInformation


    Private _costCenter As String
    Public Property CostCenter() As String
        Get
            Return _costCenter
        End Get
        Set(ByVal value As String)
            _costCenter = value
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


    Private _pages As Decimal
    Public Property Pages() As Decimal
        Get
            Return _pages
        End Get
        Set(ByVal value As Decimal)
            _pages = value
        End Set
    End Property


    Private _specialHandling As Boolean
    Public Property SpecialHandling() As Boolean
        Get
            Return _specialHandling
        End Get
        Set(ByVal value As Boolean)
            _specialHandling = value
        End Set
    End Property


    Private _letterId As String
    Public Property LetterId() As String
        Get
            Return _letterId
        End Get
        Set(ByVal value As String)
            _letterId = value
        End Set
    End Property


    Private _instructions As String
    Public Property Instructions() As String
        Get
            Return _instructions
        End Get
        Set(ByVal value As String)
            _instructions = value
        End Set
    End Property




End Class
