<CLSCompliant(True)> _
Public MustInherit Class PersonDemographics
    Inherits Demographics

    Private _fName As String
    ''' <summary>
    ''' Borrower First Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FName() As String
        Get
            Return _fName
        End Get
        Set(ByVal value As String)
            _fName = value
        End Set
    End Property

    Private _mi As String
    ''' <summary>
    ''' Borrower Middle Initial
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MI() As String
        Get
            Return _mi
        End Get
        Set(ByVal value As String)
            _mi = value
        End Set
    End Property

    Private _lName As String
    ''' <summary>
    ''' Borrower Last Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LName() As String
        Get
            Return _lName
        End Get
        Set(ByVal value As String)
            _lName = value
        End Set
    End Property

End Class
