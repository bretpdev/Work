Public Class EnterpriseFileSystem

    Private _tempFolder As String
    Public ReadOnly Property TempFolder() As String
        Get
            Return _tempFolder
        End Get
    End Property

    Private _ftpFolder As String
    Public ReadOnly Property FtpFolder() As String
        Get
            Return _ftpFolder
        End Get
    End Property

    Private _logsFolder As String
    Public ReadOnly Property LogsFolder() As String
        Get
            Return _logsFolder
        End Get
    End Property

    Private ReadOnly _fileSystemDataAccess As DataAccess

    Public Sub New(ByVal testMode As Boolean, ByVal region As ScriptSessionBase.Region)
        _fileSystemDataAccess = New DataAccess(testMode, region)
        _tempFolder = _fileSystemDataAccess.GetFileSystemPath("TEMP")
        _ftpFolder = _fileSystemDataAccess.GetFileSystemPath("FTP")
        _logsFolder = _fileSystemDataAccess.GetFileSystemPath("LOGS")
    End Sub

    Public Function GetPath(ByVal fileSystemKey As String) As String
        Return _fileSystemDataAccess.GetFileSystemPath(fileSystemKey)
    End Function

End Class
