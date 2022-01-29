Public Class Course
    Private _category As String
    Private _title As String
    Private _weight As String
    Private _gradeLevel As String
    Private _credits As Double
    Private _isAcceptable As Nullable(Of Boolean)
    Private _verification As Verification
    Private _grades As List(Of Grade)
    Private _weightedAverageGrade As Grade

    Public Property Category() As String
        Get
            Return _category
        End Get
        Set(ByVal value As String)
            _category = value
        End Set
    End Property

    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Public Property Weight() As String
        Get
            Return _weight
        End Get
        Set(ByVal value As String)
            _weight = value
        End Set
    End Property

    Public Property GradeLevel() As String
        Get
            Return _gradeLevel
        End Get
        Set(ByVal value As String)
            _gradeLevel = value
        End Set
    End Property

    Public Property Credits() As Double
        Get
            Return _credits
        End Get
        Set(ByVal value As Double)
            _credits = value
        End Set
    End Property

    ''' <summary>
    ''' A null value means "In Progress."
    ''' </summary>
    Public Property IsAcceptable() As Nullable(Of Boolean)
        Get
            Return _isAcceptable
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            _isAcceptable = value
        End Set
    End Property

    Public Property Verification() As Verification
        Get
            Return _verification
        End Get
        Set(ByVal value As Verification)
            _verification = value
        End Set
    End Property

    Public Property Grades() As List(Of Grade)
        Get
            Return _grades
        End Get
        Set(ByVal value As List(Of Grade))
            _grades = value
        End Set
    End Property

    ''' <summary>
    ''' Calculated from the Grades collection.
    ''' </summary>
    Public ReadOnly Property WeightedAverageGrade() As Grade
        Get
            'TODO: Get the WAG according to the formula in the spec.
        End Get
    End Property
End Class
