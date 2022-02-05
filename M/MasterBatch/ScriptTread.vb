Imports Reflection.SessionClass
Imports Reflection.Constants
Imports Reflection.Settings
Imports System.IO

Public Class ScriptTread

    Private SI As ScriptItem
    Private UID As UserID
    Private TDirector As Threading.Thread
    Private TScript As Threading.Thread
    Private RefSession As Object
    Private ScriptWasSuccessful As Boolean = False

    'contructor
    Public Sub New(ByRef NewSI As ScriptItem, ByRef NewUID As UserID)
        SI = NewSI
        UID = NewUID
    End Sub

    'the main starting point for object level processing.  Called by main program
    Public Sub StartProc()
        'create, link and start thread
        TDirector = New Threading.Thread(AddressOf TheDirector)
        TDirector.IsBackground = True
        TDirector.Start()
    End Sub

    'accessor function for main program
    Public Function StillProcessing() As Boolean
        If TDirector.ThreadState = Threading.ThreadState.Running Then
            Return True
        Else
            Return False
        End If
    End Function

    'main thread
    Private Sub TheDirector()
        Dim Script As SubScriptThread
        'setup connection: link code, connect to server, create session 
        RefSessionSetUp()
        'login to region
        While LoginSuccessfully() = False
            'close current session
            RefSession.Quit()
            'notify user of error and give them option of what to do
            If MessageBox.Show("MBS was not able to log in using " + UID.ID + ".  Do you want to try again?  Click yes to try again or no to remove the user id from MBS.", "Login Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                'if the user doesn't want to try again then 
                'remove id from user id list
                UID.SetInUse()
                'change status back to "ready" and 
                SI.SetReady()
                'return control to main program
                Threading.Thread.CurrentThread.Abort()
            End If
            'try and login again
            RefSessionSetUp()
        End While
        'once login is successful then start up script thread
        Script = New SubScriptThread(RefSession, SI.ModAndSub, UID.ID, UID.Pass)
        TScript = New Threading.Thread(AddressOf Script.Script)
        TScript.IsBackground = True
        TScript.Start()
        TScript.Join() 'wait until script has returned before parent thread checks for log file
        'check for log file
        If File.Exists(SI.LogFile) Then
            File.Delete(SI.LogFile) 'delete log file
            ScriptWasSuccessful = True 'script was successful
        Else
            ScriptWasSuccessful = False 'script was not successful
        End If
    End Sub

    'called to find out if script ran successfully to completion
    Public Function ScriptSuccessful() As Boolean
        Return ScriptWasSuccessful
    End Function

    'try and login
    Private Function LoginSuccessfully() As Boolean
        'wait for the logon screen to be displayed
        RefSession.WaitForDisplayString(">", "0:0:30", 16, 10)
        If SI.TestMode Then 'test
            XYInput(16, 12, "QTOR", True)
            'wait for the greetings screen to be displayed
            RefSession.WaitForDisplayString("USERID", "0:0:30", 20, 8)
            XYInput(20, 18, UID.ID)
            XYInput(20, 40, UID.Pass, True)
            If TextCheck(20, 8, "USERID==>") Then
                LoginSuccessfully = False
                Exit Function
            End If
            RefSession.FindText("RS/UT", 3, 5)
            XYInput(RefSession.FoundTextRow, RefSession.FoundTextColumn - 2, "X", True)
        Else 'live 
            XYInput(16, 12, "PHEAA", True)
            'wait for the greetings screen to be displayed
            RefSession.WaitForDisplayString("USERID", "0:0:30", 20, 8)
            XYInput(20, 18, UID.ID)
            XYInput(20, 40, UID.Pass, True)
            If TextCheck(20, 8, "USERID==>") Then
                LoginSuccessfully = False
                Exit Function
            End If
        End If
        'check connection on LP00
        FastPathInput("LP00")
        If TextCheck(3, 32, "M A I N   M E N U") Then
            LoginSuccessfully = True
        Else
            LoginSuccessfully = False
        End If
    End Function

    'creates session and connects to hera
    Private Sub RefSessionSetUp()
        Dim Secs As Integer
        Dim ASecond As New TimeSpan(0, 0, 1)
        Dim Success As Boolean
        'create Reflection session
        RefSession = CreateObject("ReflectionIBM.Session")
        'try and connect to code
        While Success = False
            Try
                If SI.TestMode Then

                    RefSession.AddReference("X:\Sessions\Test\BatchProcTst.rvx")
                Else
                    RefSession.AddReference("X:\Sessions\BatchProcTst.rvx")
                End If
                Success = True
            Catch
                Success = False
                Secs = Secs + 1
                If Secs = 60 Then
                    MessageBox.Show("MBS couldn't connect to the code out on the X drive.  Please contact a member of Systems Support.", "MBS Code Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End
                End If
            End Try
        End While
        RefSession.Hostname = "hera"
        RefSession.Visible = True
        RefSession.Connect()
    End Sub

#Region " General Stuff "

    'this function will transmit a key for you
    Public Function Press(ByVal key As String, Optional ByVal keyset As String = "1")
        Try
            key = UCase(key)
            If TextCheck(23, 23, keyset) Then
                RefSession.TransmitTerminalKey(rcIBMPf2Key)
                RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            End If
            Select Case key
                Case "F1"
                    RefSession.TransmitTerminalKey(rcIBMPf1Key)
                Case "F2"
                    RefSession.TransmitTerminalKey(rcIBMPf2Key)
                Case "F3"
                    RefSession.TransmitTerminalKey(rcIBMPf3Key)
                Case "F4"
                    RefSession.TransmitTerminalKey(rcIBMPf4Key)
                Case "F5"
                    RefSession.TransmitTerminalKey(rcIBMPf5Key)
                Case "F6"
                    RefSession.TransmitTerminalKey(rcIBMPf6Key)
                Case "F7"
                    RefSession.TransmitTerminalKey(rcIBMPf7Key)
                Case "F8"
                    RefSession.TransmitTerminalKey(rcIBMPf8Key)
                Case "F9"
                    RefSession.TransmitTerminalKey(rcIBMPf9Key)
                Case "F10"
                    RefSession.TransmitTerminalKey(rcIBMPf10Key)
                Case "F11"
                    RefSession.TransmitTerminalKey(rcIBMPf11Key)
                Case "F12"
                    RefSession.TransmitTerminalKey(rcIBMPf12Key)
                Case "ENTER"
                    RefSession.TransmitTerminalKey(rcIBMEnterKey)
                Case "CLEAR"
                    RefSession.TransmitTerminalKey(rcIBMClearKey)
                Case "END"
                    RefSession.TransmitTerminalKey(rcIBMEraseEOFKey)
                Case "UP"
                    RefSession.TransmitTerminalKey(rcIBMPA1Key)
                Case "TAB"
                    RefSession.TransmitTerminalKey(rcIBMTabKey)
                Case "HOME"
                    RefSession.TransmitTerminalKey(rcIBMHomeKey)
                Case "INS"
                    RefSession.TransmitTerminalKey(rcIBMInsertKey)
                Case Else
                    MsgBox("There has been a key code error.  Please contact a programmer.", MsgBoxStyle.Critical)
                    End
            End Select
            RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please contact Systems Support.", MsgBoxStyle.Critical)
            End
        End Try
    End Function

    'Enters information into the Fast Path.
    Public Function FastPathInput(ByVal inp As String)
        Try
            RefSession.TransmitTerminalKey(rcIBMClearKey)
            RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            RefSession.TransmitANSI(inp)
            RefSession.TransmitTerminalKey(rcIBMEnterKey)
            RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please contact Systems Support.", MsgBoxStyle.Critical)
            End
        End Try
    End Function

    'Enters inp into the given X,Y coordinates
    Public Function XYInput(ByVal y As Integer, ByVal x As Integer, ByVal inp As String, Optional ByVal Enter As Boolean = False)
        Try
            RefSession.MoveCursor(y, x)
            RefSession.TransmitANSI(inp)
            'if enter = true then hit enter.
            If (Enter) Then
                RefSession.TransmitTerminalKey(rcIBMEnterKey)
                RefSession.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            End If
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please contact Systems Support.", MsgBoxStyle.Critical)
            End
        End Try
    End Function

    'Checks for specific text at a certain location on the screen
    Public Function TextCheck(ByVal y As Integer, ByVal x As Integer, ByVal i As String)
        Try
            If (RefSession.GetDisplayText(y, x, Len(i)) = i) Then
                TextCheck = True
            Else
                TextCheck = False
            End If
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please contact Systems Support.", MsgBoxStyle.Critical)
            End
        End Try
    End Function
#End Region

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
        SyncLock GetType(SubScriptThread)
            'run script
            RIBM.RunMacro(ModAndSub, ID & "," & Pass)
        End SyncLock
    End Sub

End Class

