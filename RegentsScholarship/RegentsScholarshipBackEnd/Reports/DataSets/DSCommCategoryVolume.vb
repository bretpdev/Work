Public Class DSCommCategoryVolume
    Private _startMonth As String
    Public Property StartMonth() As String
        Get
            Return _startMonth
        End Get
        Set(ByVal value As String)
            _startMonth = value
        End Set
    End Property

    Private _startYear As String
    Public Property StartYear() As String
        Get
            Return _startYear
        End Get
        Set(ByVal value As String)
            _startYear = value
        End Set
    End Property

    Private _type As String
    Public Property Type() As String
        Get
            Return _type
        End Get
        Set(ByVal value As String)
            _type = value
        End Set
    End Property

    Private _category As String
    Public Property Category() As String
        Get
            Return _category
        End Get
        Set(ByVal value As String)
            _category = value
        End Set
    End Property
End Class
