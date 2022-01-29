Imports System

''' <summary>
''' Represents an integral value that can only be incremented.
''' </summary>
Public Class Count
    Implements IEquatable(Of Count)

    'Use an event to notify clients when the count changes.
    '(As of this class's creation date, this is needed by the EndOfJobReport class so it can write the new value to a backing store.)
    Public Event ValueChanged(ByVal newValue As Integer)

    Private _count As Integer

    Public Sub New(ByVal initialValue As Integer)
        _count = initialValue
    End Sub

    Public Sub Increment()
        _count += 1
        RaiseEvent ValueChanged(_count)
    End Sub

    'Allow integers to be compared to Count objects.
    Public Shared Operator =(ByVal a As Count, ByVal b As Integer) As Boolean
        If (CType(a, Object) Is Nothing) Then Return False
        Return a._count = b
    End Operator

    Public Shared Operator <>(ByVal a As Count, ByVal b As Integer) As Boolean
        Return (Not (a = b))
    End Operator

    Public Shared Operator >(ByVal a As Count, ByVal b As Integer) As Boolean
        If (CType(a, Object) Is Nothing) Then Return False
        Return a._count > b
    End Operator

    Public Shared Operator <(ByVal a As Count, ByVal b As Integer) As Boolean
        If (CType(a, Object) Is Nothing) Then Return False
        Return a._count < b
    End Operator

    'Override the standard stuff.
    Public Shared Operator =(ByVal a As Count, ByVal b As Count) As Boolean
        If (Object.ReferenceEquals(a, b)) Then Return True
        If (CType(a, Object) Is Nothing OrElse CType(b, Object) Is Nothing) Then Return False
        Return a._count = b._count
    End Operator

    Public Shared Operator <>(ByVal a As Count, ByVal b As Count) As Boolean
        Return (Not (a = b))
    End Operator

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Dim other As Count = TryCast(obj, Count)
        If (other Is Nothing) Then Return False
        Return MyBase.Equals(obj) AndAlso Me._count = other._count
    End Function

    Public Overrides Function ToString() As String
        Return _count.ToString()
    End Function

#Region "IEquatable(Of Count) Members"

    Public Overloads Function Equals(ByVal other As Count) As Boolean Implements IEquatable(Of Count).Equals
        Return _count.Equals(other._count)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return _count.GetHashCode()
    End Function

#End Region
End Class
