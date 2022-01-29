Imports System.Diagnostics
Imports Reflection
Imports System.Linq.Enumerable

Public Class SessionMainIWin32Window
    Implements System.Windows.Forms.IWin32Window

    ''' <summary>
    ''' Creates new main Reflection Session window handle with IWin32Window wrapper around it
    ''' </summary>
    ''' <param name="rs">The Reflection Session</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal rs As Session)
        Try
            _hwnd = CalculateSessionMainWindowHandle(rs)
        Catch ex As Exception
            _hwnd = Nothing
        End Try
    End Sub

    Public ReadOnly Property Handle() As IntPtr Implements IWin32Window.Handle
        Get
            Return _hwnd
        End Get
    End Property

    Private _hwnd As IntPtr

    ' Calculates Session Main Window Handle through the magic of LINQ
    Private Function CalculateSessionMainWindowHandle(ByVal rs As Session) As IntPtr
        Dim processes As Process() = Process.GetProcesses()
        Dim sessionProc As Process
        sessionProc = (From procs In processes _
                    Where procs.MainWindowTitle = rs.Caption _
                    Select procs).SingleOrDefault()
        Return sessionProc.MainWindowHandle
    End Function

End Class

