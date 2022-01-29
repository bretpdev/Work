Public Class TestModeResults

    Private _ftpFolder As String
    Public Property FtpFolder() As String
        Get
            Return _ftpFolder
        End Get
        Set(ByVal value As String)
            _ftpFolder = value
        End Set
    End Property

    Private _docFolder As String
    Public Property DocFolder() As String
        Get
            Return _docFolder
        End Get
        Set(ByVal value As String)
            _docFolder = value
        End Set
    End Property

    Private _logFolder As String
    Public Property LogFolder() As String
        Get
            Return _logFolder
        End Get
        Set(ByVal value As String)
            _logFolder = value
        End Set
    End Property

    Private _isInTestMode As Boolean
    Public Property IsInTestMode() As Boolean
        Get
            Return _isInTestMode
        End Get
        Set(ByVal value As Boolean)
            _isInTestMode = value
        End Set
    End Property

End Class
