Public Class UserWithBusinessUnit
    Inherits User

    Private _businessUnit As String
    ''' <summary>
    ''' Business unit of user.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BusinessUnit() As String
        Get
            Return _businessUnit
        End Get
        Set(ByVal value As String)
            _businessUnit = value
        End Set
    End Property

End Class
