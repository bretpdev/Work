Imports System.Runtime.Remoting
Imports Reflection
Imports Q
Imports System.Windows.Forms

'<ComClass(CUBE.ClassId, CUBE.InterfaceId, CUBE.EventsId)> _
<ComClass(ComInterface.ClassId, ComInterface.InterfaceId)> _
Public Class ComInterface

    Private Const PC_DIRECTORY As String = "C:\Program Files\UHEAA\UHEAACodeBase\"

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "7C4E891F-6961-421b-9A58-0C408C66A4F6"
    Public Const InterfaceId As String = "1F2489A6-03CF-45bb-96A2-A76E6E05E570"
    'Public Const ClassId As String = "443199fe-5eb7-4848-8e47-9ca86766065c"
    'Public Const InterfaceId As String = "05fff462-b746-4499-a644-47280791f62a"
    'Public Const EventsId As String = "dae453b5-590e-486e-804b-518d0c355b23"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub StartScript(ByVal reflectionSession As Reflection.Session, ByVal testMode As Boolean, ByVal dllToLoad As String, ByVal objectToCreate As String)
        Dim objs(0) As Object
        Dim script As ObjectHandle
        objs(0) = New ReflectionInterface(reflectionSession, testMode)
        Try
            script = System.Activator.CreateInstanceFrom(PC_DIRECTORY + dllToLoad, objectToCreate, True, Nothing, Nothing, objs, Nothing, Nothing, Nothing)
            CType(script.Unwrap(), ScriptBase).Main()
        Catch ex As EndDLLException 'any time the coder wants the script to end they call a method that throws this exception
            Exit Sub 'end script
            'Catch ex As Exception
            '    MessageBox.Show(ex.ToString(), "C# Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    Exit Sub 'end script
        End Try
    End Sub

End Class


