Imports System.IO

Public Class RecoveryLog
    Private _logFile As String
    Private _recoveryValue As String

    ''' <summary>
    ''' Gets or sets the text contained in the log file.
    ''' </summary>
    ''' <value>The text to write out to the log file.</value>
    ''' <returns>The last text written to the log file, or an empty string if no value has been written.</returns>
    Public Property RecoveryValue() As String
        Get
            Return _recoveryValue
        End Get
        Set(ByVal value As String)
            If _recoveryValue <> value Then
                _recoveryValue = value
                Using logWriter As New StreamWriter(_logFile, False)
                    logWriter.WriteLine(value)
                End Using
            End If
        End Set
    End Property

    ''' <summary>
    ''' Creates a new instance of the RecoveryLog class and associates it with the appropriate log file.
    ''' If the log file is found, the RecoveryValue property is initialized with the log's first line of text.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="scriptId">The ID of the script as listed in Sacker.</param>
    Public Sub New(ByVal testMode As Boolean, ByVal scriptId As String)
        CreateCommercialRecovery(testMode, scriptId)
    End Sub

    ''' <summary>
    ''' Creates a new instance of the RecoveryLog class and associates it with the appropriate log file.
    ''' If the log file is found, the RecoveryValue property is initialized with the log's first line of text.
    ''' </summary>
    ''' <param name="fileName">The name of the recovery file.</param>
    ''' <param name="efs">EnterpriseFileSystem object.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal fileName As String, ByVal efs As EnterpriseFileSystem)
        _logFile = String.Format("{0}{1}.txt", efs.LogsFolder, fileName)
        InitializeRecoveryValue(_logFile)
    End Sub

    ''' <summary>
    ''' Deletes the log file if it exists and clears the RecoveryValue property..
    ''' </summary>
    Public Sub Delete()
        _recoveryValue = String.Empty
        Common.KillUntilDead(_logFile)
    End Sub

    Private Sub CreateCommercialRecovery(ByVal testMode As Boolean, ByVal scriptId As String)
        Dim testFolder As String = If(testMode, "Test\", String.Empty)
        _logFile = String.Format("{0}{1}{2}.txt", DataAccess.RecoveryLogDirectory, testFolder, scriptId)
        InitializeRecoveryValue(_logFile)
    End Sub

    Private Sub InitializeRecoveryValue(ByVal logFile As String)
        If File.Exists(logFile) Then
            _recoveryValue = File.ReadAllLines(logFile)(0)
        Else
            _recoveryValue = String.Empty
        End If
    End Sub
End Class
