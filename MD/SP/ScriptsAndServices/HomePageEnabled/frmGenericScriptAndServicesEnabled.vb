Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.Remoting
Imports Reflection
Imports Q

Public Class frmGenericScriptAndServicesEnabled

    Private DA As SqlDataAdapter
    Private SSBL As SP.BorrowerLite
    Private HomePage As String
    'vars from old Scripts and Services that seem to be needed
    Private ReQ As frmReQueue
    Protected VerbalForbPerf As Boolean = False
    Declare Function OpenIcon Lib "user32" (ByVal hwnd As Long) As Long
    Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long
    Protected Scripts As New System.Collections.Generic.Dictionary(Of String, ScriptAndServiceMenuItem) 'this dictionary provides a way to call scripts from the child form
    Const pcDir As String = "C:\Enterprise Program Files\Nexus\"
    Const testNetworkDir As String = "X:\PADU\UHEAACodeBase\"
    Const liveNetworkDir As String = "X:\Sessions\UHEAA Codebase\"

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub New(ByRef tHomePage As String, ByRef tSSBL As SP.BorrowerLite, ByVal ScriptsDisableKey As String)
        InitializeComponent()
        DA = New SqlDataAdapter("", SP.UsrInf.Conn)
        SSBL = tSSBL
        HomePage = tHomePage
        CreateMenuItems(Nothing, HomePage, ScriptsDisableKey)
    End Sub

    'this function is recursive and sets up all menu items for the screen
    Private Sub CreateMenuItems(ByVal ParentMenu As ToolStripMenuItem, ByVal HomePage As String, ByVal ScriptsDisableKey As String)
        Dim I As String = 0
        Dim DS As New DataSet
        Dim SSMI As ScriptAndServiceMenuItem
        Dim PMenu As String
        Dim Comm As New SqlCommand
        If ParentMenu Is Nothing Then
            PMenu = ""
        Else
            PMenu = ParentMenu.Text
        End If
        DA.SelectCommand.CommandText = String.Format("SELECT * FROM MenuOptionsScriptsAndServices WHERE HomePage = '{0}' and ParentMenu = '{1}'", HomePage, PMenu)
        DA.Fill(DS, "MenuOptions")
        While I < DS.Tables("MenuOptions").Rows.Count
            If ParentMenu Is Nothing Then
                'if the menu item is being directly added to the menu bar
                SSMI = New ScriptAndServiceMenuItem(DS.Tables("MenuOptions").Rows(I), DS.Tables("MenuOptions").Rows(I).Item("DisplayName"))
                If DS.Tables("MenuOptions").Rows(I).Item("SubToBeCalled") <> "" Then
                    'if the DB has a sub to be called then add even handler
                    AddHandler SSMI.Click, AddressOf EventHandler4MenuItems 'add event handler for menu item
                End If
                ScriptsAndServicesMainMenu.Items.Add(SSMI) 'add to menu
            Else
                'if menu item is being added to a list under a menu item
                SSMI = New ScriptAndServiceMenuItem(DS.Tables("MenuOptions").Rows(I), DS.Tables("MenuOptions").Rows(I).Item("DisplayName"))
                If DS.Tables("MenuOptions").Rows(I).Item("SubToBeCalled") <> "" Then
                    'if the DB has a sub to be called then add even handler
                    AddHandler SSMI.Click, AddressOf EventHandler4MenuItems 'add event handler for menu item
                End If
                Scripts.Add(DS.Tables("MenuOptions").Rows(I).Item("DisplayName"), SSMI)
                ParentMenu.DropDownItems.Add(SSMI) 'add to menu
                'check if script should be disabled based off option the user selected on bins screen
                If ScriptsDisableKey <> "" Then
                    'disable the menu option if account maintance was selected on the bins page
                    If InStr(DS.Tables("MenuOptions").Rows(I).Item("DisableKey").ToString, "AM", CompareMethod.Binary) Then
                        SSMI.Enabled = False
                    End If
                End If
            End If
            'check if menu item should also be a parent menu item
            Comm.CommandText = String.Format("SELECT COUNT(*) FROM dbo.MenuOptionsScriptsAndServices WHERE HomePage = '{0}' and ParentMenu = '{1}'", HomePage, DS.Tables("MenuOptions").Rows(I).Item("DisplayName"))
            Comm.Connection = SP.UsrInf.Conn
            Comm.Connection.Open()
            If CInt(Comm.ExecuteScalar()) > 0 Then
                Comm.Connection.Close()
                'create child menu items if needed
                CreateMenuItems(CType(SSMI, ToolStripMenuItem), HomePage, ScriptsDisableKey)
            Else
                Comm.Connection.Close()
            End If
            I = I + 1
        End While
    End Sub

    Public Shared Sub RunScriptExternalScript(ByVal Script As ScriptAndServiceMenuItem, ByVal SSN As String)
        Dim CLS() As String
        CLS = Split(Replace(SP.Q.RIBM.CommandLineSwitches, """", ""), "\")
        'bring reflection session to the top of all windows
        LActivatePrevInstance(CLS(CLS.GetUpperBound(0)))
        SP.Q.RIBM.SwitchToWindow(1)
        'call script
        Try
            SP.Q.RIBM.RunMacro(Script.gsData.Item("SubToBeCalled"), SSN & ",1," & CStr(SP.Q.TestMode()))
            'check for existence of script completion file
            If Script.gsData.Item("CompletionFile") <> "Nothing" Then
                If File.Exists(Script.gsData.Item("CompletionFile")) = False Then
                    'if script completion file doesn't exist then
                    SP.frmWhoaDUDE.WhoaDUDE("DUDE detected that either the script was manually cancelled or the script ended abnormally.  In either case DUDE doesn't register that the script was run.", "Holy Maco")
                    Exit Sub
                Else
                    'delete script completion file if it exists
                    File.Delete(Script.gsData.Item("CompletionFile"))
                End If
            End If
        Catch ex As Exception
            SP.frmWhoaDUDE.WhoaDUDE("DUDE was unable to find your script." & vbLf & ex.Message, "Holy Maco")
            Exit Sub
        End Try
    End Sub


    Public Shared Sub RunDotNETDll(ByVal Script As ScriptAndServiceMenuItem, ByVal Bor As Borrower, ByVal RunNumber As Integer)
        Dim mainDllToLoad As String = Script.gsData.Item("DLLToLoad").ToString()
        Dim objs(2) As Object
        Dim scriptInstance As ObjectHandle
        FileUpdater(Script.gsData.Item("DLLsToCopy").ToString()) 'update files
        'if in test mode then load test dll
        If SP.TestMode() Then mainDllToLoad = "Test\" & mainDllToLoad
        'load parameters for script's constructor
        objs(0) = New ReflectionInterface(SP.Q.RIBM, TestMode)
        objs(1) = Bor
        objs(2) = RunNumber
        'start script
        Try
            scriptInstance = System.Activator.CreateInstanceFrom(pcDir + mainDllToLoad, Script.gsData.Item("ObjectToCreate").ToString(), True, Nothing, Nothing, objs, Nothing, Nothing, Nothing)
            CType(scriptInstance.Unwrap(), ScriptBase).Main()
        Catch ex As EndDLLException 'any time the coder wants the script to end they call a method that throws this exception
            Exit Sub 'end script
        End Try
    End Sub

    Private Shared Sub UpdateDlls(ByVal dllFiles As String)
        Dim workingDir As String
        Dim pcWorkingDir As String
        Dim Dlls() As String
        Dim Dll As Object
        Dlls = Split(dllFiles, ",")
        If SP.TestMode() = False Then
            workingDir = liveNetworkDir
            pcWorkingDir = pcDir
        Else
            workingDir = testNetworkDir
            pcWorkingDir = pcDir & "Test\"
            'check for existence of test directory and create if needed
            If Dir(pcWorkingDir) = "" Then
                MkDir(pcWorkingDir)
                'copy over all files from live directory if the test directory was just created
                Dim dllToCopy As String
                dllToCopy = Dir(pcDir & "*")
                While dllToCopy <> ""
                    FileCopy(pcDir & dllToCopy, pcWorkingDir & dllToCopy)
                    dllToCopy = Dir()
                End While
            End If
        End If
        Try
            For Each Dll In Dlls
                If Dir(pcWorkingDir & Dll) = "" Then
                    'if Dll doesn't exist then pull it down from the network
                    FileCopy(workingDir & Dll, pcWorkingDir & Dll)
                Else
                    'if Dll exists then check if it needs to be updated
                    If FileDateTime(workingDir & Dll) <> FileDateTime(pcWorkingDir & Dll) Then
                        'if time date stamps don't equal then update the Dll
                        Kill(pcWorkingDir & Dll)
                        FileCopy(workingDir & Dll, pcWorkingDir & Dll)
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New System.Exception("An error occurred while trying to update your script code.  The most likely reason for this is because there is new code to be loaded and the code loaded on your PC is old.  It is suggested that you shutdown your Reflection session and start it back up to refresh your code.  If you feel that you have received this error for other reasons then please contact Systems Support.")
        End Try
    End Sub

    'deletes old dlls
    Private Shared Sub DeleteOldDlls(ByVal dllName As String)
        If Dir(pcDir & dllName & ".dll") <> "" Then
            Kill(pcDir & dllName & ".dll")
            Kill(pcDir & dllName & ".tlb")
        End If
    End Sub

    'updates .net DLLs
    Private Shared Sub FileUpdater(ByVal dllFilesToCopy As String)
        UpdateDlls("Q.dll") 'update Q
        UpdateDlls(dllFilesToCopy) 'update all other dlls
        'delete old DLLs
        DeleteOldDlls("Alderaan")
        DeleteOldDlls("Coruscant")
        DeleteOldDlls("Dagobah")
        DeleteOldDlls("Endor")
        DeleteOldDlls("Ferengi")
        DeleteOldDlls("Gallifrey")
        DeleteOldDlls("Gondor")
        DeleteOldDlls("Hoth")
        DeleteOldDlls("Klingon")
        DeleteOldDlls("Mordor")
        DeleteOldDlls("Moria")
        DeleteOldDlls("Naboo")
        DeleteOldDlls("Rivendale")
        DeleteOldDlls("Rohan")
        DeleteOldDlls("Romulan")
        DeleteOldDlls("Shire")
        DeleteOldDlls("Tatooine")
        DeleteOldDlls("Vulcan")
        DeleteOldDlls("Yavin")
    End Sub

    Public Shared Sub LActivatePrevInstance(ByVal argStrAppToFind As String)
        Dim PrevHndl As Long
        Dim result As Long
        'Variable to hold individual Process.
        Dim objProcess As New Process
        'Collection of all the Processes running on local machine
        Dim objProcesses() As Process
        'Get all processes into the collection
        objProcesses = Process.GetProcesses()
        For Each objProcess In objProcesses
            'Check and exit if we have SMS running already
            If UCase(objProcess.MainWindowTitle) = UCase(argStrAppToFind) Then
                PrevHndl = objProcess.MainWindowHandle.ToInt32()
                Exit For
            End If
        Next
        'If no previous instance found exit the application.
        If PrevHndl = 0 Then Exit Sub
        'If previous instance found.
        result = OpenIcon(PrevHndl)       'Restore the program.
        result = SetForegroundWindow(PrevHndl)        'Activate the application.
    End Sub

    Public Overridable Sub EventHandler4MenuItems(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'guts should be in child class
    End Sub

End Class