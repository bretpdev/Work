'In all cases, there's no new functionality or data for the exception to handle;
'we just need to distinguish between situations that need to be handled differently.

Public Class RegentsInvalidDataException
    Inherits Exception

    Public Sub New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub
End Class