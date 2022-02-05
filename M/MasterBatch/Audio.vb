Imports System.IO
Public Class Audio
    Declare Auto Function PlaySound Lib "winmm.dll" (ByVal name _
      As String, ByVal hmod As Integer, ByVal flags As Integer) As Integer

    Declare Auto Function PlaySound Lib "winmm.dll" (ByVal name _
      As Byte(), ByVal hmod As Integer, ByVal flags As Integer) As Integer

    Public Const SND_SYNC = &H0 ' play synchronously 
    Public Const SND_ASYNC = &H1 ' play asynchronously 
    Public Const SND_MEMORY = &H4  'Play wav in memory
    Public Const SND_ALIAS = &H10000 'Play system alias wav 
    Public Const SND_NODEFAULT = &H2
    Public Const SND_FILENAME = &H20000 ' name is file name 
    Public Const SND_RESOURCE = &H40004 ' name is resource name or atom 

    Public Shared Sub PlayWaveFile(ByVal fileWaveFullPath As String)
        Try
            PlaySound(fileWaveFullPath, 0, SND_FILENAME)
        Catch
        End Try
    End Sub

    Public Shared Sub PlayWaveResource(ByVal WaveResourceName As String)

        ' get the namespace 
        Dim strNameSpace As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString()

        ' get the resource into a stream
        Dim resourceStream As Stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(strNameSpace + "." + WaveResourceName)
        If resourceStream Is Nothing Then Exit Sub

        ' bring stream into a byte array
        Dim wavData As Byte()
        ReDim wavData(CInt(resourceStream.Length))
        resourceStream.Read(wavData, 0, CInt(resourceStream.Length))

        ' play the resource
        PlaySound(wavData, 0, SND_ASYNC Or SND_MEMORY)
    End Sub

    Public Shared Sub PlayWaveSystem(ByVal SystemWaveName As String)
        'EXAMPLE= Sound.PlayWaveSystem("SystemExit")
        PlaySound(SystemWaveName, 0&, SND_ALIAS Or SND_ASYNC Or SND_NODEFAULT)
    End Sub
End Class
