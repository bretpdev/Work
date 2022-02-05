Public Class DocumentPathAndName

    Private _originalDBEntry As String
    Public Property OriginalDBEntry() As String
        Get
            Return _originalDBEntry
        End Get
        Set(ByVal value As String)
            _originalDBEntry = value
        End Set
    End Property

    Private _calculatedPath As String
    Public Property CalculatedPath() As String
        Get
            Return _calculatedPath
        End Get
        Set(ByVal value As String)
            _calculatedPath = value
        End Set
    End Property

    Private _calculatedFileName As String
    Public Property CalculatedFileName() As String
        Get
            Return _calculatedFileName
        End Get
        Set(ByVal value As String)
            _calculatedFileName = value
        End Set
    End Property

End Class
