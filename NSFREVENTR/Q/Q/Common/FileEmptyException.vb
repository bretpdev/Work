Public Class FileEmptyException
    Inherits System.IO.FileNotFoundException

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Public Sub New(ByVal message As String, ByVal fileName As String)
        MyBase.New(message, fileName)
    End Sub

    Public Sub New(ByVal message As String, ByVal fileName As String, ByVal innerException As Exception)
        MyBase.New(message, fileName, innerException)
    End Sub
End Class
