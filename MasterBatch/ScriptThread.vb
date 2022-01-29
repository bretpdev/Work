Imports System.IO
Imports Q

Public Class ScriptThread

    Private LD As New LockData
    Private TDirector As Threading.Thread
    Private TScript As Threading.Thread
    Private LVBatch As ListView
    Private LVMain As ListView
    Private MainFrm As frmMasterBatch
    Private RefInterface As ReflectionInterface

    'contructor
    Public Sub New(ByRef NewSI As ScriptItem, ByRef NewUID As UserID, ByRef NewMainFrm As frmMasterBatch, ByRef tLVBatch As ListView, ByRef tLVMain As ListView)
        LD.SI = NewSI
        LD.UID = NewUID
        MainFrm = NewMainFrm
        LVBatch = tLVBatch
        LVMain = tLVMain
    End Sub

    Public Function LockedData() As LockData
        LockedData = LD
    End Function


    'the main starting point for object level processing.  Called by main program
    Public Sub StartProc()
        'create, link and start thread
        TDirector = New Threading.Thread(AddressOf TheDirector)
        TDirector.IsBackground = True
        TDirector.Start()
    End Sub

    'main thread
    Private Sub TheDirector()
        Dim Script As SubScriptThread
        Threading.Monitor.Enter(LD)
        LD.SI.SetRunning() 'set to running
        'setup connection: link code, connect to server, create session 
        RefInterface = New ReflectionInterface(True)
        'RefInterface.GetRefSession().Windowstate = 1
        'MainFrm.Focus()

        'login to region
        While RefInterface.Login(LD.UID.ID, LD.UID.Pass) = False
            'close current session
            RefInterface.CloseSession()
            'notify user of error and give them option of what to do
            If MessageBox.Show("MBS was not able to log in using " + LD.UID.ID + ".  Do you want to try again?  Click yes to try again or no to remove the user id from MBS.", "Login Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                'if the user doesn't want to try again then 
                'remove id from user id list
                LD.UID.SetInUse()
                LD.UID.RemoveFromDB()
                LD.UID.BeingUsedByMBS = False ' not being used by thread any more
                'change status back to "ready" and 
                LD.SI.SetReady()
                'return control to main program
                Threading.Monitor.Exit(LD)
                Threading.Thread.CurrentThread.Abort()
            End If
            'try and login again
            RefInterface = New ReflectionInterface(LD.SI.TestMode)
        End While
        'once login is successful then start up script thread
        Script = New SubScriptThread(RefInterface.ReflectionSession, LD.SI.ModAndSub, LD.UID.ID, LD.UID.Pass)
        TScript = New Threading.Thread(AddressOf Script.Script)
        TScript.IsBackground = True
        TScript.Start()
        TScript.Join() 'wait until script has returned before parent thread checks for log file
        'close current session
        RefInterface.CloseSession()
        LD.UID.BeingUsedByMBS = False
        'check for log file
        If File.Exists(LD.SI.LogFile) Then
            File.Delete(LD.SI.LogFile) 'delete log file
            LD.SI.SetComplete() 'script is complete
            'remove from batch list
            MainFrm.Invoke(New RemoveItemFromLVDelegate(AddressOf MainFrm.RemoveItemFromLV), New Object() {LVBatch, LD.SI})
        Else
            'script was not successful
            LD.SI.SetDebug()
            'do async calls to UI thread
            MainFrm.Invoke(New RemoveItemFromLVDelegate(AddressOf MainFrm.RemoveItemFromLV), New Object() {LVBatch, LD.SI})
            MainFrm.Invoke(New AddItemToLVDelegate(AddressOf MainFrm.AddItemToLV), New Object() {LVMain, LD.SI})
        End If
        Threading.Monitor.Exit(LD)
    End Sub

End Class

Public Class SubScriptThread

    Private RIBM As Object
    Private ModAndSub As String
    Private ID As String
    Private Pass As String

    Public Sub New(ByRef tRIBM As Object, ByVal tModAndSub As String, ByVal tID As String, ByVal tPass As String)
        RIBM = tRIBM
        ModAndSub = tModAndSub
        ID = tID
        Pass = tPass
    End Sub

    'threaded sub for running script
    Public Sub Script()
        'SyncLock GetType(SubScriptThread)
        'run script
        RIBM.RunMacro(ModAndSub, "MasterBatchScript," & ID & "," & Pass & "," & CStr(TestMode))
        'End SyncLock
    End Sub

End Class

Public Class LockData
    Public SI As ScriptItem
    Public UID As UserID
End Class

