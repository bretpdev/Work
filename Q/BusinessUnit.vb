Public Class BusinessUnit
    Implements IEquatable(Of BusinessUnit)

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _id As Integer
    Public Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Public Sub New()
        _name = ""
        _id = 0
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Shared Operator =(ByVal a As BusinessUnit, ByVal b As BusinessUnit) As Boolean
        If (Object.ReferenceEquals(a, b)) Then Return True
        If (CType(a, Object) Is Nothing OrElse CType(b, Object) Is Nothing) Then Return False
        Return a.ID = b.ID
    End Operator

    Public Shared Operator <>(ByVal a As BusinessUnit, ByVal b As BusinessUnit) As Boolean
        Return ((a = b) = False)
    End Operator

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Dim other As BusinessUnit = TryCast(obj, BusinessUnit)
        If (other Is Nothing) Then Return False
        Return MyBase.Equals(obj) AndAlso Me.ID = other.ID
    End Function

#Region "IEquatable(Of BusinessUnit) Members"

    Public Overloads Function Equals(ByVal other As BusinessUnit) As Boolean Implements IEquatable(Of BusinessUnit).Equals
        Return ID.Equals(other.ID)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return ID.GetHashCode()
    End Function

#End Region
End Class
