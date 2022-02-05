Imports System.Threading
Imports System.IO
Imports System.Diagnostics.FileVersionInfo
Module NetStartClient
    Function CheckForNewVersion(ByVal liveFolder As String) As Boolean
        Dim getNewFiles As Boolean
        Dim currentFolder As String
        Dim lfv As FileVersionInfo
        Dim cfv As FileVersionInfo
        Dim exe As String
        currentFolder = System.Reflection.Assembly.GetExecutingAssembly.Location
        exe = currentFolder.Substring(currentFolder.LastIndexOf("\") + 1)
        currentFolder = currentFolder.Replace(exe, "")

        getNewFiles = False
        'get list of live files
        Dim farr As IO.FileInfo() = New IO.DirectoryInfo(liveFolder).GetFiles()
        Dim fi As IO.FileInfo
        For Each fi In farr
            'MsgBox(fi.Name)
            If Dir(currentFolder & fi.Name) = "" Then
                'missing file
                getNewFiles = True
                Exit For
            Else
                cfv = FileVersionInfo.GetVersionInfo(currentFolder & fi.Name)
                lfv = FileVersionInfo.GetVersionInfo(liveFolder & fi.Name)
                If cfv.FileVersion <> lfv.FileVersion Then
                    getNewFiles = True
                    Exit For
                End If
            End If

        Next

        If getNewFiles Then
            FileOpen(1, "T:\NETstart.txt", OpenMode.Output)
            WriteLine(1, liveFolder)
            WriteLine(1, currentFolder)
            WriteLine(1, exe.Replace(".exe", ""))
            FileClose(1)
            Process.Start("T:\NETstart.exe")
            End
        End If
    End Function
End Module
