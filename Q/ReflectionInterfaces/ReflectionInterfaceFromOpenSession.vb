Imports Reflection
Imports System.Text

Public Class ReflectionInterfaceFromOpenSession
    'Hide the constructor and use a factory method to find an open session.
    Private Sub New()
    End Sub

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Use the factory method in ReflectionInterface instead, so we can retire this class.
    ''' </summary>
    Public Shared Function Connect() As ReflectionInterface
        Return ReflectionInterface.ConnectToOpenSession()
    End Function
End Class
