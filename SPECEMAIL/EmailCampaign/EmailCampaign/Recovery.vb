Public Class Recovery

    Public Shared LogFile As String = "T:\EmailCamp Recovery Log.txt"
    Public Shared InRecovery As Boolean 'tracks whether the app is in recovery or not
    Public Enum Phase
        NotInRecovery = 0
        Emailing = 1
        ActivityComments = 2
    End Enum

    'checks if a log file exists, if on does then we are in recovery
    Public Shared Function IsAppInRecovery() As Boolean
        If Dir(LogFile) <> "" Then
            InRecovery = True
        Else
            InRecovery = False
        End If
    End Function

    'updates the recovery log
    Public Shared Sub UpdateLog(ByVal InPhase As Phase, ByVal SSN As String)
        FileOpen(3, LogFile, OpenMode.Output, OpenAccess.Write, OpenShare.LockWrite)
        If InPhase = Phase.Emailing Then
            PrintLine(3, "InProcess,")
        Else
            PrintLine(3, "Done,InProcess")
        End If
        PrintLine(3, SSN)
        FileClose(3)
    End Sub

    'Returns recovery phase
    Public Shared Function RecoveryPhase() As Phase
        Dim DataFileLine As String = ""
        If Dir(LogFile) = "" Then
            'not in recovery
            Return Phase.NotInRecovery
        Else
            'possibly in recovery
            If InRecovery Then 'check if recovery flag was set
                FileOpen(3, LogFile, OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
                DataFileLine = LineInput(3)
                If DataFileLine = "InProcess," Then
                    FileClose(3)
                    Return Phase.Emailing
                Else
                    FileClose(3)
                    Return Phase.ActivityComments
                End If
            Else
                Return Phase.NotInRecovery
            End If
        End If
    End Function

    'returns the SSN stored in the recovery log
    Public Shared Function GetAcctNum() As String
        Dim DataFileLine As String = ""
        FileOpen(3, LogFile, OpenMode.Input, OpenAccess.Read, OpenShare.LockRead)
        DataFileLine = LineInput(3) 'recovery phase
        DataFileLine = LineInput(3) 'recovery SSN
        FileClose(3)
        Return DataFileLine
    End Function

    'deletes the recovery log
    Public Shared Sub DeleteLog()
        If Dir(LogFile) <> "" Then Kill(LogFile)
    End Sub

End Class
