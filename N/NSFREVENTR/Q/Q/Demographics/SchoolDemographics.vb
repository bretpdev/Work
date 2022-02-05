Public Class SchoolDemographics
    Inherits Demographics

    ''' <summary>
    ''' The name of the school
    ''' </summary>
    ''' <remarks></remarks>
    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    ''' <summary>
    ''' The department code
    ''' </summary>
    ''' <remarks></remarks>
    Private _department As String
    Public Property Department() As String
        Get
            Return _department
        End Get
        Set(ByVal value As String)
            _department = value
        End Set
    End Property



End Class
