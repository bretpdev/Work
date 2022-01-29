Imports System.Text
Imports Reflection
Imports Reflection.Constants
Imports System.Windows.Forms

Public Class ReflectionInterface

    Declare Function OpenIcon Lib "user32" (ByVal hwnd As Long) As Long
    Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long

    <Flags()> _
    Public Enum Flag
        None = 0
        ''' <summary>If set, the session will be initialized such that the BatchScriptBase.CalledByMasterBatchScript() function returns true.</summary>
        MasterBatchScript = 1
        ''' <summary>If set, the session will be initialized such that the BatchScriptBase.CalledByJams property returns true.</summary>
        Jams = 2
    End Enum

    Public Enum Key
        F1
        F2
        F3
        F4
        F5
        F6
        F7
        F8
        F9
        F10
        F11
        F12
        Enter
        Clear
        EndKey
        Up
        Tab
        Home
        Insert
        Down
        Esc
        PrintScreen
        None
    End Enum

    Private _reflectionSession As Session
    ''' <summary>
    ''' DEPRECATED!!!
    ''' Clients should never use this property. All session interaction must go through
    ''' a method of ReflectionInterface to shield clients from upgrades to Reflection.
    ''' </summary>
    Public Property ReflectionSession() As Session
        Get
            Return _reflectionSession
        End Get
        Set(ByVal value As Session)
            _reflectionSession = value
        End Set
    End Property

    Private _testMode As Boolean
    ''' <summary>
    ''' Is the script functioning in "Test Mode"
    ''' </summary>
    Public ReadOnly Property TestMode() As Boolean
        Get
            Return _testMode
        End Get
    End Property

    ''' <summary>
    ''' Opens a generic UHEAA session, and then creates the Reflection interface object with that session with no flags.
    ''' </summary>
    ''' <param name="testMode">Test mode indicator</param>
    Public Sub New(ByVal testMode As Boolean)
        Me.New(OpenExistingSession(testMode, Flag.None, ScriptSessionBase.Region.UHEAA), testMode)
    End Sub

    ''' <summary>
    ''' Creates a ReflectionInterface object using a generic UHEAA session, with the specified Flags applied.
    ''' </summary>
    ''' <param name="testMode">True if running in test mode.</param>
    ''' <param name="flags">Bit-wise collection of Flag values.</param>
    Public Sub New(ByVal testMode As Boolean, ByVal flags As Flag)
        Me.New(OpenExistingSession(testMode, flags, ScriptSessionBase.Region.UHEAA), testMode)
    End Sub

    ''' <summary>
    ''' Creates a ReflectionInterface object using a generic session for the region specified, with no Flags applied.
    ''' </summary>
    ''' <param name="testMode"></param>
    ''' <param name="region"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal testMode As Boolean, ByVal region As Q.ScriptSessionBase.Region)
        Me.New(OpenExistingSession(testMode, Flag.None, region), testMode)
    End Sub

    ''' <summary>
    ''' Creates a ReflectionInterface object using a generic session for the region specified, with the specified Flags applied.
    ''' </summary>
    ''' <param name="testMode"></param>
    ''' <param name="flags"></param>
    ''' <param name="region"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal testMode As Boolean, ByVal flags As Flag, ByVal region As Q.ScriptSessionBase.Region)
        Me.New(OpenExistingSession(testMode, flags, region), testMode)
    End Sub

    ''' <summary>
    ''' Creates new instance of ReflectionInterface
    ''' </summary>
    ''' <param name="reflectionSession">The Reflection Session</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal reflectionSession As Session, ByVal testMode As Boolean)
        _reflectionSession = reflectionSession
        _testMode = testMode
    End Sub

    ''' <summary>
    ''' Factory method that connects to an existing Reflection session.
    ''' If multiple open sessions are found, the user is prompted to select a session.
    ''' </summary>
    ''' <returns>A ReflectionInterface object connected to the session, or null if no sessions are found.</returns>
    Public Shared Function ConnectToOpenSession() As ReflectionInterface
        'Check that there's at least one RIBM process.
        If Process.GetProcessesByName("R8win").Length = 0 Then Return Nothing

        'Search for up to 10 DUDE sessions.
        Dim openSessions As New List(Of Session)()
        For dudeIndex As Integer = 1 To 10
            Try
                openSessions.Add(CType(GetObject("RIBMMD" + dudeIndex.ToString()), Session))
            Catch ex As Exception
                'A session by that name doesn't exist, so skip it.
            End Try
        Next
        'Check for a non-DUDE session.
        Try
            openSessions.Add(CType(GetObject("RIBM"), Session))
        Catch ex As Exception
            'A session by that name doesn't exist, so skip it.
        End Try

        'Have the user select a session if more than one was found.
        Dim selectedSession As Session = Nothing
        If openSessions.Count = 1 Then
            selectedSession = openSessions(0)
        Else
            Dim messageBuilder As New StringBuilder(String.Format("Which session do you want to connect to?{0}{0}", Environment.NewLine))
            For i As Integer = 1 To openSessions.Count
                messageBuilder.AppendFormat("{0} - {1}{2}", i.ToString(), openSessions(i - 1).Caption, Environment.NewLine)
            Next i
            Dim selection As InputBoxResults = InputBox.ShowDialog(messageBuilder.ToString(), "Select a Session")
            While (Not selection.UserProvidedText.IsNumeric()) OrElse (Integer.Parse(selection.UserProvidedText) < 1) OrElse (Integer.Parse(selection.UserProvidedText) > openSessions.Count)
                MessageBox.Show("That wasn't a valid option.  Please try again.", "Not a Valid Option", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                selection = InputBox.ShowDialog(messageBuilder.ToString(), "Select a Session")
            End While
            Dim selectedSessionIndex As Integer = Integer.Parse(selection.UserProvidedText) - 1
            selectedSession = openSessions(selectedSessionIndex)
        End If

        'Create a ReflectionInterface object from the selected session, if any.
        If selectedSession Is Nothing Then
            Return Nothing
        Else
            Dim testMode As Boolean = (selectedSession.CommandLineSwitches.ToLower().Contains("tst.rsf") OrElse selectedSession.CommandLineSwitches.ToLower().Contains("dev.rsf"))
            Return New ReflectionInterface(selectedSession, testMode)
        End If
    End Function

#Region "Instance Calls"

    ''' <summary>
    ''' Brings the Reflection session to the front, in case it's minimzed or hidden by other windows.
    ''' </summary>
    Public Sub BringSessionToFront()
        BringSessionToFront(_reflectionSession)
    End Sub

    ''' <summary>
    ''' Transmits a key
    ''' </summary>
    ''' <param name="keyToHit">The key to transmit</param>
    ''' <returns>Returns true if the key is in the defined Key enumeration</returns>
    ''' <remarks></remarks>
    Public Function Hit(ByVal keyToHit As Key) As Boolean
        Return Hit(_reflectionSession, keyToHit)
    End Function

    ''' <summary>
    ''' Clears screen and enters provided text into the Fast Path
    ''' </summary>
    ''' <param name="input">Text to be entered into the Fast Path</param>
    Public Sub FastPath(ByVal input As String)
        Hit(_reflectionSession, Key.Clear)
        PutText(_reflectionSession, 1, 1, input, Key.Enter)
    End Sub

    ''' <summary>
    ''' Clears screen and enters provided text into the Fast Path
    ''' </summary>
    ''' <param name="format">Text to be entered into the Fast Path</param>
    ''' <param name="args">Values to be inserted into the format strings placeholders, if any.</param>
    Public Sub FastPath(ByVal format As String, ByVal ParamArray args() As Object)
        Dim finalString As String = String.Format(format, args)
        Hit(_reflectionSession, Key.Clear)
        PutText(_reflectionSession, 1, 1, finalString, Key.Enter)
    End Sub

    ''' <summary>
    ''' Attempts to find the given text on the Reflection screen, starting from the upper left corner.
    ''' </summary>
    ''' <param name="text">The text to find.</param>
    ''' <returns>
    ''' A Coordinate object whose Row and Column properties point to the beginning of the found text,
    ''' or null if the text is not found.
    ''' </returns>
    Public Function FindText(ByVal text As String) As Coordinate
        Return FindText(text, 1, 1)
    End Function

    ''' <summary>
    ''' Attempts to find the given text on the Reflection screen, starting from a specified position.
    ''' </summary>
    ''' <param name="text">The text to find.</param>
    ''' <param name="startRow">The row where the search will start.</param>
    ''' <param name="startColumn">The column where the search will start.</param>
    ''' <returns>
    ''' A Coordinate object whose Row and Column properties point to the beginning of the found text,
    ''' or null if the text is not found.
    ''' </returns>
    Public Function FindText(ByVal text As String, ByVal startRow As Integer, ByVal startColumn As Integer) As Coordinate
        Return ReflectionInterface.FindText(_reflectionSession, text, startRow, startColumn)
    End Function

    ''' <summary>
    ''' Retrieves the screen contents from the specified row and column for the full length requested.
    ''' Same as GetText, except that nothing is trimmed from either end.
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="length">The number of characters to pull from the screen</param>
    ''' <returns>The contents pulled from location specified</returns>
    Public Function GetDisplayText(ByVal row As Integer, ByVal column As Integer, ByVal length As Integer) As String
        Return GetDisplayText(_reflectionSession, row, column, length)
    End Function

    ''' <summary>
    ''' Retrieves the text from the specifed row, column, length coordinates (this function trims off empty spaces on the front and back of the string)
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="length">The number of characters to pull from the screen (function trims spaces before returning)</param>
    ''' <returns>The text pulled from location specified minus any spaces on the front or back</returns>
    ''' <remarks></remarks>
    Public Function GetText(ByVal row As Integer, ByVal column As Integer, ByVal length As Integer) As String
        Return GetText(_reflectionSession, row, column, length)
    End Function

    ''' <summary>
    ''' Logs the user into the commercial system region.  If successful then it returns true else false.
    ''' </summary>
    ''' <param name="userId">The user's UT system ID.</param>
    ''' <param name="passWord">The user's system password.</param>
    Public Function Login(ByVal userId As String, ByVal passWord As String) As Boolean
        Return Login(userId, passWord, ScriptSessionBase.Region.UHEAA)
    End Function

    ''' <summary>
    ''' Logs the user into the system.  If successful then it returns true else false.
    ''' </summary>
    ''' <param name="userId">The user's UT system ID.</param>
    ''' <param name="passWord">The user's system password.</param>
    ''' <param name="region">The region to log into.</param>
    Public Function Login(ByVal userId As String, ByVal passWord As String, ByVal region As ScriptSessionBase.Region) As Boolean
        If region = ScriptSessionBase.Region.None Then
            Throw New ArgumentException("You must specify a region when calling the Login function.", "region")
        End If

        'wait for the logon screen to be displayed
        ReflectionSession.WaitForDisplayString(">", "0:0:30", 16, 10)

        'set the value to be entered in the LOGON field on the logon screen depending on the test mode
        Dim logonText As String = "PHEAA"
        If TestMode Then
            logonText = "QTOR"
        End If
        PutText(16, 12, logonText, Key.Enter)
        ReflectionSession.WaitForDisplayString("USERID", "0:0:30", 20, 8)

        PutText(20, 18, userId)
        PutText(20, 40, passWord, Key.Enter)

        'return false if credentials are rejected
        If Check4Text(20, 8, "USERID==>") Then
            Return False
        End If

        'select the specified region
        If TestMode Then
            Dim regionLabel As String = "_ RS/UT"

            If region = ScriptSessionBase.Region.CornerStone Then
                regionLabel = "_ K1/FD"
            End If

            ReflectionSession.FindText(regionLabel, 3, 5)
            PutText(ReflectionSession.FoundTextRow, ReflectionSession.FoundTextColumn, "X", Key.Enter)
            If region = ScriptSessionBase.Region.CornerStone Then Hit(Key.F10)
        ElseIf region = ScriptSessionBase.Region.CornerStone Then
			FastPath("STUPKU")
			Hit(Key.F10)
        End If

        'verify the user is logged in
        FastPath("TX3ZITX1J")
        If Check4Text(1, 72, "TXX1K") Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Logs the user out of the system, returning you to the login screen.
    ''' </summary>
    ''' <remarks>This is helpful for re-attempting a login after a failed login, since the Login method expects the session to be on the login screen.</remarks>
    Public Sub LogOut()
        Hit(Key.Clear)
        PutText(1, 2, "LOG", Key.Enter)
    End Sub

    ''' <summary>
    ''' Enters the given text in the coordinates given
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="text">The text to be entered</param>
    Public Sub PutText(ByVal row As Integer, ByVal column As Integer, ByVal text As String)
        ReflectionInterface.PutText(_reflectionSession, row, column, text, Key.None, False)
    End Sub

    ''' <summary>
    ''' Enters the given text in the coordinates given
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="text">The text to be entered</param>
    ''' <param name="blankFieldFirst">If true, the field will be blanked out before putting in text.</param>
    Public Sub PutText(ByVal row As Integer, ByVal column As Integer, ByVal text As String, ByVal blankFieldFirst As Boolean)
        ReflectionInterface.PutText(_reflectionSession, row, column, text, Key.None, blankFieldFirst)
    End Sub

    ''' <summary>
    ''' Enters the given text in the coordinates given
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="text">The text to be entered</param>
    ''' <param name="keyToHit">The key to transmit</param>
    Public Sub PutText(ByVal row As Integer, ByVal column As Integer, ByVal text As String, ByVal keyToHit As Key)
        ReflectionInterface.PutText(_reflectionSession, row, column, text, keyToHit, False)
    End Sub

    ''' <summary>
    ''' Enters the given text in the coordinates given
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="text">The text to be entered</param>
    ''' <param name="keyToHit">The key to transmit</param>
    ''' <param name="blankFieldFirst">If true, the field will be blanked out before putting in text.</param>
    Public Sub PutText(ByVal row As Integer, ByVal column As Integer, ByVal text As String, ByVal keyToHit As Key, ByVal blankFieldFirst As Boolean)
        ReflectionInterface.PutText(_reflectionSession, row, column, text, keyToHit, blankFieldFirst)
    End Sub

    ''' <summary>
    ''' Compares the provided text to the text found in the coordinates provided.
    ''' The comparison is not case sensitive.
    ''' </summary>
    ''' <param name="row">The row half of the coordinate to start at</param>
    ''' <param name="column">The column half of the coordinate to start at</param>
    ''' <param name="text">The text to be tested against. Multiple text items can be checked.</param>
    ''' <returns>True if a match is found.</returns>
    Public Function Check4Text(ByVal row As Integer, ByVal column As Integer, ByVal ParamArray text() As String) As Boolean
        Return Check4Text(_reflectionSession, row, column, text)
    End Function

    ''' <summary>
    ''' Simply enters text where the cursor is currently located.
    ''' </summary>
    ''' <param name="text">Text to be entered.</param>
    ''' <remarks></remarks>
    Public Sub EnterText(ByVal text As String)
        _reflectionSession.TransmitANSI(text)
    End Sub

    ''' <summary>
    ''' Returns the current coordinate the cursor is at.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCurrentCursorCoordinate() As Coordinate
        Return (New Coordinate() With {.Column = _reflectionSession.CursorColumn, .Row = _reflectionSession.CursorRow})
    End Function

#End Region

#Region "Shared Calls"

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Sub BringSessionToFront(ByVal rs As Session)
        'The session's CommandLineSwitches property has the full path and file name of the session, enclosed in quotes.
        'We need just the file name, so get the end of the text starting after the last '\' character and strip off the end quote.
        '(This assumes the path and file name are the only text in CommandLineSwitches.)
        Dim fileName As String = rs.CommandLineSwitches.Substring(rs.CommandLineSwitches.LastIndexOf("\") + 1).Replace("""", "")

        'Get the process handle that has the session's file name as its window title.
        Dim sessionProcess As Process = ( _
            From p In Process.GetProcesses() _
            Where p.MainWindowTitle.ToUpper() = fileName.ToUpper() _
        ).FirstOrDefault()

        'Restore the session (if needed) and bring it to the front.
        If sessionProcess IsNot Nothing Then
            OpenIcon(sessionProcess.MainWindowHandle.ToInt32())
            SetForegroundWindow(sessionProcess.MainWindowHandle.ToInt32())
        End If
    End Sub

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Sub FastPath(ByVal rs As Session, ByVal format As String, ByVal ParamArray args() As Object)
        Dim finalString As String = String.Format(format, args)
        Hit(rs, Key.Clear)
        PutText(rs, 1, 1, finalString, Key.Enter)
    End Sub

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Function FindText(ByVal rs As Session, ByVal text As String) As Coordinate
        Return ReflectionInterface.FindText(rs, text, 1, 1)
    End Function

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Function FindText(ByVal rs As Session, ByVal text As String, ByVal startRow As Integer, ByVal startColumn As Integer) As Coordinate
        If rs.FindText(text, startRow, startColumn) = 0 Then
            Return Nothing
        End If
        Return New Coordinate With {.Row = rs.FoundTextRow, .Column = rs.FoundTextColumn}
    End Function

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Function GetDisplayText(ByVal rs As Session, ByVal row As Integer, ByVal column As Integer, ByVal length As Integer) As String
        Return rs.GetDisplayText(row, column, length)
    End Function

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Function GetText(ByVal rs As Session, ByVal row As Integer, ByVal column As Integer, ByVal length As Integer) As String
        GetText = Trim(rs.GetDisplayText(row, column, length))
    End Function

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Function Hit(ByVal rs As Session, ByVal keyToHit As Key) As Boolean
        Hit = True
        Select Case keyToHit
            Case Key.F1
                rs.TransmitTerminalKey(rcIBMPf1Key)
            Case Key.F2
                rs.TransmitTerminalKey(rcIBMPf2Key)
            Case Key.F3
                rs.TransmitTerminalKey(rcIBMPf3Key)
            Case Key.F4
                rs.TransmitTerminalKey(rcIBMPf4Key)
            Case Key.F5
                rs.TransmitTerminalKey(rcIBMPf5Key)
            Case Key.F6
                rs.TransmitTerminalKey(rcIBMPf6Key)
            Case Key.F7
                rs.TransmitTerminalKey(rcIBMPf7Key)
            Case Key.F8
                rs.TransmitTerminalKey(rcIBMPf8Key)
            Case Key.F9
                rs.TransmitTerminalKey(rcIBMPf9Key)
            Case Key.F10
                rs.TransmitTerminalKey(rcIBMPf10Key)
            Case Key.F11
                rs.TransmitTerminalKey(rcIBMPf11Key)
            Case Key.F12
                rs.TransmitTerminalKey(rcIBMPf12Key)
            Case Key.Enter
                rs.TransmitTerminalKey(rcIBMEnterKey)
            Case Key.Clear
                rs.TransmitTerminalKey(rcIBMClearKey)
            Case Key.EndKey
                rs.TransmitTerminalKey(rcIBMEraseEOFKey)
            Case Key.Up
                rs.TransmitTerminalKey(rcIBMPA1Key)
            Case Key.Down
                rs.TransmitTerminalKey(rcIBMPA2Key)
            Case Key.Tab
                rs.TransmitTerminalKey(rcIBMTabKey)
            Case Key.Insert
                rs.TransmitTerminalKey(rcIBMInsertKey)
            Case Key.Esc
                rs.TransmitTerminalKey(rcIBMResetKey)
            Case Key.PrintScreen
                rs.TransmitTerminalKey(rcIBMPrintKey)
            Case Else
                Hit = False
        End Select
        rs.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
    End Function

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Sub PutText(ByVal rs As Session, ByVal row As Integer, ByVal column As Integer, ByVal text As String)
        PutText(rs, row, column, text, Key.None, False)
    End Sub

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Sub PutText(ByVal rs As Session, ByVal row As Integer, ByVal column As Integer, ByVal text As String, ByVal blankFieldFirst As Boolean)
        PutText(rs, row, column, text, Key.None, blankFieldFirst)
    End Sub

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Sub PutText(ByVal rs As Session, ByVal row As Integer, ByVal column As Integer, ByVal text As String, ByVal keyToHit As Key)
        PutText(rs, row, column, text, keyToHit, False)
    End Sub

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Sub PutText(ByVal rs As Session, ByVal row As Integer, ByVal column As Integer, ByVal text As String, ByVal keyToHit As Key, ByVal blankFieldFirst As Boolean)
        'Check that the session will allow us to write text at the requested location.
        If (rs.GetFieldAttributes(row, column) And rcProtected) Then
            Dim message As String = String.Format("Attempted to write ""{0}"" into row {1} column {2}, but that location on the screen is not writeable.", text, row, column)
            Throw New Exception(message)
        End If

        'Sessions will only allow 260 characters to be entered, so check that we're not over that limit.
        Const MAX_TEXT_LENGTH As Integer = 260
        If text IsNot Nothing AndAlso text.Length > MAX_TEXT_LENGTH Then
            Dim message As String = String.Format("The script has been asked to enter more text than the session will allow. The session allows {0} characters to be written at once, but the requested text length is {1} characters.", MAX_TEXT_LENGTH, text.Length)
            message += Environment.NewLine
            message += "The requested text follows:"
            message += Environment.NewLine
            message += text
            Throw New Exception(message)
        End If

        'Blank the field if requested.
        If blankFieldFirst Then
            rs.MoveCursor(row, column)
            Hit(rs, Key.EndKey)
        End If

        'Enter the text.
        rs.MoveCursor(row, column)
        rs.TransmitANSI(text)

        'Hit a key if requested.
        If keyToHit <> Key.None Then
            Hit(rs, keyToHit)
        End If
    End Sub

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Shared Function Check4Text(ByVal rs As Session, ByVal row As Integer, ByVal column As Integer, ByVal ParamArray text() As String) As Boolean
        For Each textItem As String In text
            If (String.Compare(rs.GetDisplayText(row, column, textItem.Length), textItem, True) = 0) Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Sub EnterText(ByVal rs As Session, ByVal text As String)
        rs.TransmitANSI(text)
    End Sub

    ''' <summary>
    ''' DEPRECATED!!!
    ''' Never use a method that requires you to pass a Reflection session as an argument.
    ''' </summary>
    Public Function GetCurrentCursorCoordinate(ByVal rs As Session) As Coordinate
        Return (New Coordinate() With {.Column = rs.CursorColumn, .Row = rs.CursorRow})
    End Function

#End Region

    ''' <summary>
    ''' Closes the session that is tied to this ReflectionInterface.
    ''' </summary>
    ''' <remarks>
    ''' Once this method is called, the ReflectionInterface object is useless
    ''' and will throw a null reference exception if you try to do anything with it.
    ''' </remarks>
    Public Sub CloseSession()
        ReflectionSession.Quit()
    End Sub

    ' ''creates session and connects to hera and links to code
    ''Private Shared Function CreateSession() As Session
    ''    Return CreateSession(Flag.None)
    ''End Function

    ''Private Shared Function CreateSession(ByVal flags As Flag) As Session
    ''    Dim theSession As Session = CreateObject("ReflectionIBM.Session")

    ''    'Apply any flags requested.
    ''    Dim macroData As New List(Of String)()
    ''    If ((flags And Flag.MasterBatchScript) = Flag.MasterBatchScript) Then macroData.Add(BatchScriptBase.MASTER_BATCH_SCRIPT_ID)
    ''    If ((flags And Flag.Jams) = Flag.Jams) Then macroData.Add(BatchScriptBase.JAMS_ID)
    ''    theSession.MacroData = String.Join(",", macroData.ToArray())

    ''    theSession.BDTIgnoreScrollLock = True
    ''    theSession.Visible = True
    ''    theSession.Hostname = "hera2"
    ''    theSession.TelnetPort = "1022"
    ''    theSession.TelnetEncryption = True
    ''    theSession.TelnetEncryptionVerifyHostName = False


    ''    theSession.Connect()
    ''    Return theSession
    ''End Function

    Private Shared Function OpenExistingSession(ByVal testMode As Boolean, ByVal flags As Flag, ByVal region As Q.ScriptSessionBase.Region) As Session
        Dim efs As New EnterpriseFileSystem(testMode, region)
        Dim sessionName As String = String.Format("{0}Generic.rsf", efs.GetPath("Sessions"))
        Dim Pro As New System.Diagnostics.Process
        Dim newSession As Object

        If testMode Then sessionName = sessionName.Replace(".", "Tst.")
        Pro.StartInfo.FileName = sessionName
        Pro.Start()

        'wait until session is ready to handle commands
        Threading.Thread.Sleep(New TimeSpan(0, 0, 3))
        While True
            Try
                'get new session just started and change the OLE server name so other scripts can't see it
                newSession = GetObject("RIBMGEN")
                newSession.OLEServerName = String.Format("GENRIBM{0}", TimeOfDay.ToString)

                'apply any flags requested.
                Dim macroData As New List(Of String)()
                If ((flags And Flag.MasterBatchScript) = Flag.MasterBatchScript) Then macroData.Add(BatchScriptBase.MASTER_BATCH_SCRIPT_ID)
                If ((flags And Flag.Jams) = Flag.Jams) Then macroData.Add(BatchScriptBase.JAMS_ID)
                newSession.MacroData = String.Join(",", macroData.ToArray())

                'apply settings
                newSession.BDTIgnoreScrollLock = True
                newSession.Visible = True

                Exit While
            Catch ex As Exception
                MessageBox.Show(ex.StackTrace)
                Threading.Thread.Sleep(New TimeSpan(0, 0, 2))
            End Try
        End While

        Return newSession
    End Function

    Public Sub PauseForInsert()
        ReflectionSession.WaitForTerminalKey(rcIBMInsertKey, "1:00:00")
        ReflectionInterface.Hit(_reflectionSession, Key.Insert)
    End Sub

End Class
