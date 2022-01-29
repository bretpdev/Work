Public Class EmployerDemographics
    Inherits Demographics

    ''' <summary>
    ''' Employer's name
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


End Class
