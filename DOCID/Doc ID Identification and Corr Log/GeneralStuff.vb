Imports System.Collections.Generic
Imports Reflection.SessionClass
Imports Reflection.Constants
Imports Reflection.Settings
Imports Q

Module GeneralStuff


    Public RIBM As Object 'reflection session
    Declare Function OpenIcon Lib "user32" (ByVal hwnd As Long) As Long
    Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long
    Dim UIDGen As String

    Public Enum TD22ReturnStatus
        Added
        LP50
        Err
    End Enum

    Public Enum SsnStatus
        Valid
        SemiValid
        Invalid
    End Enum

    Public ReadOnly Property TempDir() As String
        Get
            Return "T:\"
        End Get
    End Property

    Public ReadOnly Property UserID() As String
        Get
            FastPathInput("PROF")
            Return GetText(2, 49, 7)
        End Get
    End Property

    Public ReadOnly Property ScriptID() As String
        Get
            Return "DOCID"
        End Get
    End Property

#Region " General Use Functionality "
    'enter comments in LP50
    Public Function AddLP50(ByVal mailReceivedDate As Date, ByVal ssn As String, ByVal actionCode As String, Optional ByVal activityType As String = "", Optional ByVal contactType As String = "", Optional ByVal comment As String = "", Optional ByVal pauseForAdditionalCommentsIsRequested As Boolean = False) As Boolean
        'access LP50
        FastPathInput(String.Format("LP50A{0};;;{1};{2};{3}", ssn, activityType, contactType, actionCode))
        'pause for the user to enter the activity type and contact type if the acttyp is blank
        If String.IsNullOrEmpty(activityType) Then
            MessageBox.Show("Enter the activity type and the contact type and then hit <Insert> to resume the script.", "Enter Contact Type and Activity Type", MessageBoxButtons.OK)
            RIBM.WaitForTerminalKey(rcIBMInsertKey, "1:00:00")
            Press("INS")
            Press("ENTER")
        ElseIf CheckForText(7, 2, "__") Then
            PutText(7, 2, activityType + contactType)
        End If
        If mailReceivedDate.Date <> DateTime.Now.Date Then
            comment += String.Format(", MAIL RCVD DT: {0}", mailReceivedDate.ToString("MM/dd/yyyy"))
        End If

        'enter the comment
        If pauseForAdditionalCommentsIsRequested = True Then
            PutText(13, 2, comment)
            If MessageBox.Show("Do you want to enter additional comments?  Click Yes to pause the script, add the additional comments, and then hit <Insert> to resume the script.  Click No to continue processing without adding additional comments.", "Add Additional Comments", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                RIBM.WaitForTerminalKey(rcIBMInsertKey, "1:00:00")
                Press("INS")
            End If
            Dim row As Integer = 13
            Dim col As Integer = 2
            Do Until row = 21
                If CheckForText(row, col, "____________") AndAlso _
                  (CheckForText(row + 1, 2, "_") OrElse CheckForText(row + 1, 2, " ")) Then
                    PutText(row, col, String.Format("  {0}{1}{2}", "{", ScriptID, "}"))
                    Exit Do
                End If

                If col < 62 Then
                    col += 1
                Else
                    col = 2
                    row += 1
                End If
            Loop
        Else
            PutText(13, 2, String.Format("{0}  {1}{2}{3}", comment, "{", ScriptID, "}"))
        End If

        Press("F6")
        If CheckForText(22, 3, "48003") <> True Then
            Return False
        End If

        Return True
    End Function

    'enters an activity record/action request in COMPASS selecting only the loans specified
    Function ATD22AllLoans(ByVal ssn As String, ByVal arc As String, ByVal comment As String) As TD22ReturnStatus
        Dim addTd22 As New TD22(DirectCast(RIBM, Reflection.Session))
        Dim result As Common.CompassCommentScreenResults = addTd22.ATD22AllLoans(ssn, arc, comment, ScriptID)
        If result = Common.CompassCommentScreenResults.CommentAddedSuccessfully Then
            Return TD22ReturnStatus.Added
        ElseIf result = Common.CompassCommentScreenResults.ErroredDuringPosting Then
            Return TD22ReturnStatus.Err
        End If
    End Function

    Function ATC00GeneralComment(ByVal ssn As String, ByVal message As String) As TD22ReturnStatus
        FastPathInput("TX3Z/ATC00" + ssn)
        PutText(19, 38, "4", True)
        PutText(18, 2, message, True)
        If CheckForText(23, 2, "01004") Then
            Return TD22ReturnStatus.Added
        Else
            Return TD22ReturnStatus.Err
        End If
    End Function

    Public Sub ReferenceAddLP50(ByVal referenceId As String, ByVal actionCode As String, Optional ByVal activityType As String = "", Optional ByVal contactType As String = "", Optional ByVal comment As String = "", Optional ByVal pauseForAdditionalCommentsIsRequested As Boolean = False)
        'access LP50
        FastPathInput(String.Format("LP50A;{0};;{1};{2};{3}", referenceId, activityType, contactType, actionCode))
        'pause for the user to enter the activity type and contact type if the acttyp is blank
        If String.IsNullOrEmpty(activityType) Then
            MsgBox("Enter the activity type and the contact type and then hit <Insert> to resume the script.", 48, "Enter Contact Type and Activity Type")              '<3>
            RIBM.WaitForTerminalKey(rcIBMInsertKey, "1:00:00")
            Press("INS")
            Press("ENTER")
        ElseIf CheckForText(7, 2, "__") Then
            PutText(7, 2, activityType + contactType)
        End If

        'enter the comment
        If pauseForAdditionalCommentsIsRequested = True Then
            PutText(13, 2, comment)
            If MessageBox.Show("Do you want to enter additional comments?  Click Yes to pause the script, add the additional comments, and then hit <Insert> to resume the script.  Click No to continue processing without adding additional comments.", "Add Additional Comments", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                RIBM.WaitForTerminalKey(rcIBMInsertKey, "1:00:00")
                Press("INS")
            End If

            Dim row As Integer = 13
            Dim col As Integer = 2
            Do Until row = 21
                If CheckForText(row, col, "____________") AndAlso _
                  (CheckForText(row + 1, 2, "_") OrElse CheckForText(row + 1, 2, " ")) Then
                    PutText(row, col, String.Format("  {0}{1}{2}", "{", ScriptID, "}"))
                    Exit Do
                End If

                If col <= 62 Then
                    col += 1
                Else
                    col = 2
                    row += 1
                End If
            Loop
        Else
            PutText(13, 2, String.Format("{0}  {1}{2}{3}", comment, "{", ScriptID, "}"))
        End If
        Press("F6")
    End Sub

    'this function checks to be sure that the user is logged on to OneLINK and that the borrower is on the system
    Public Function GatherSSN(ByRef ssn As String, ByVal ssnOrAccountNumberForFastPath As String, ByVal documentTitle As String, ByRef whoAmIWasUsed As Boolean, ByRef accountNumber As String, Optional ByRef doLvcTracking As Boolean = False) As SsnStatus
        Dim bankruptcyDocuments As New List(Of String)()
        bankruptcyDocuments.Add("ASBKP -- Bankruptcy Document/Correspondence")
        bankruptcyDocuments.Add("Bankruptcy Document/Correspondence")
        bankruptcyDocuments.Add("Bankruptcy Discharge/Dismissal Documents")
        bankruptcyDocuments.Add("Bankruptcy Meeting of Creditors + Zions Report")
        bankruptcyDocuments.Add("ASCON -- Bankruptcy Discharge/Dismissal Documents")
        bankruptcyDocuments.Add("ASMOC -- Bankruptcy Meeting of Creditors + Zions Report")
        bankruptcyDocuments.Add("ASCON")
        bankruptcyDocuments.Add("ASBKP")
        bankruptcyDocuments.Add("ASMOC")

        If bankruptcyDocuments.Contains(documentTitle) Then
            If (frmMain.CompassOnly) Then
                Return CheckCompassWithBankruptcy(ssnOrAccountNumberForFastPath, whoAmIWasUsed, accountNumber, ssn)
            Else
                Return CheckOneLinkWithBankruptcy(ssnOrAccountNumberForFastPath, whoAmIWasUsed, accountNumber, ssn)
            End If
        Else
            If (frmMain.CompassOnly) Then
                Return CheckCompass(ssnOrAccountNumberForFastPath, whoAmIWasUsed, accountNumber, ssn, doLvcTracking)
            Else
                Return CheckOneLink(ssnOrAccountNumberForFastPath, whoAmIWasUsed, accountNumber, ssn, doLvcTracking)
            End If
        End If
    End Function

    ''' <summary>
    ''' Checks with OneLink to see if the user is logged in and if the SSN is valid when in bankruptcy
    ''' </summary>
    ''' <param name="ssnAccNum"></param>
    ''' <param name="whoAmIWasUsed"></param>
    ''' <param name="accountNumber"></param>
    ''' <param name="ssn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckOneLinkWithBankruptcy(ByVal ssnAccNum As String, ByRef whoAmIWasUsed As Boolean, ByRef accountNumber As String, ByRef ssn As String) As SsnStatus
        'bankruptcy document
        If ssnAccNum.Length = 10 Then
            FastPathInput(String.Format("LP22I;;;;;;{0}", ssnAccNum))
        Else
            FastPathInput(String.Format("LP22I{0}", ssnAccNum))
        End If
        If CheckForText(1, 62, "PERSON DEMOGRAPHICS") = False Then
            ssn = ssnAccNum
            whoAmIWasUsed = False
            Return SsnStatus.SemiValid
            'do bankruptcy functionality but invalid SSN            
        End If
        If whoAmIWasUsed = False Then
            Dim message As String = String.Format("   -First Name = {0}{1}   -Last Name = {2}{1}{1}Is this the borrower?", GetText(4, 44, 12), Environment.NewLine, GetText(4, 5, 35))
            If MessageBox.Show(message, "Confirm Borrower", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                ssn = GetText(3, 23, 9)
                accountNumber = GetText(3, 60, 12).Replace(" ", "")
                Return SsnStatus.Valid
            Else
                Return SsnStatus.Invalid
            End If
        Else
            whoAmIWasUsed = False
            ssn = GetText(3, 23, 9)
            accountNumber = GetText(3, 60, 12).Replace(" ", "")
            Return SsnStatus.Valid
        End If
    End Function

    ''' <summary>
    ''' Checks with Compass to see if the user is logged in and if the SSN is valid when in bankruptcy
    ''' </summary>
    ''' <param name="ssnAccNum"></param>
    ''' <param name="whoAmIWasUsed"></param>
    ''' <param name="accountNumber"></param>
    ''' <param name="ssn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckCompassWithBankruptcy(ByVal ssnAccNum As String, ByRef whoAmIWasUsed As Boolean, ByRef accountNumber As String, ByRef ssn As String) As SsnStatus
        FastPathInput("TX3Z/ITX1JB;" + ssnAccNum)
        If CheckForText(1, 72, "TXX1K") = False Then
            ssn = ssnAccNum
            accountNumber = GetText(3, 34, 12).Replace(" ", "")
            whoAmIWasUsed = False
        End If
        If whoAmIWasUsed = False Then
            Dim message As String = String.Format("   -First Name = {0}{1}   -Last Name = {2}{1}{1}Is this the borrower?", GetText(4, 34, 13), Environment.NewLine, GetText(4, 6, 23))
            If MessageBox.Show(message, "Confirm Borrower", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                ssn = GetText(3, 12, 11).Replace(" ", "")
                accountNumber = GetText(3, 34, 12).Replace(" ", "")
                Return SsnStatus.Valid
            Else
                Return SsnStatus.Invalid
            End If
        Else
            whoAmIWasUsed = False
            ssn = GetText(3, 12, 11).Replace(" ", "")
            accountNumber = GetText(3, 34, 12).Replace(" ", "")
            Return SsnStatus.Valid
        End If
    End Function

    ''' <summary>
    ''' Checks to see if the user is logged into the session and retrieves the SSN & Account Numbers from LP22
    ''' </summary>
    ''' <param name="ssnAccNum"></param>
    ''' <param name="whoAmIWasUsed"></param>
    ''' <param name="accountNumber"></param>
    ''' <param name="ssn"></param>
    ''' <param name="doLvcTracking"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckOneLink(ByVal ssnAccNum As String, ByRef whoAmIWasUsed As Boolean, ByRef accountNumber As String, ByRef ssn As String, Optional ByRef doLvcTracking As Boolean = False) As SsnStatus
        'not a bankruptcy document
        If ssnAccNum.Length = 10 Then
            FastPathInput(String.Format("LP22I;;;;;;{0}", ssnAccNum))
        Else
            FastPathInput(String.Format("LP22I{0}", ssnAccNum))
        End If
        'check if the user is logged into OneLINK
        If CheckForText(1, 2, "LP22I COMMAND UNRECOGNIZED") OrElse CheckForText(3, 21, "OPERATOR IS NOT LOGGED ON") Then
            MessageBox.Show("You aren't logged into OneLINK.  Please log on then try again.", "Not Logged On", MessageBoxButtons.OK, MessageBoxIcon.Information)
            whoAmIWasUsed = False
            Return SsnStatus.Invalid
        End If

        'check if the borrower is on OneLINK
        If CheckForText(1, 62, "PERSON DEMOGRAPHICS") = False Then
            If NotInLCO(ssnAccNum) Then
                MessageBox.Show("That SSN/Account # isn't in OneLINK, COMPASS or LCO.", "Borrower Not in OneLINK", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("That SSN/Account # is only on LCO.", "Borrower Not in OneLINK", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            doLvcTracking = True
            whoAmIWasUsed = False
            Return SsnStatus.Invalid
        End If
        'Retrieve the borrower's SSN and return True            
        ssn = GetText(3, 23, 9)
        accountNumber = GetText(3, 60, 12).Replace(" ", "")
        If whoAmIWasUsed = False Then
            Dim message As String = String.Format("   -First Name = {0}{1}   -Last Name = {2}{1}{1}Is this the borrower?", GetText(4, 44, 12), Environment.NewLine, GetText(4, 5, 35))
            If MessageBox.Show(message, "Confirm Borrower", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Return SsnStatus.Valid
            Else
                Return SsnStatus.Invalid
            End If
        Else
            whoAmIWasUsed = False
            Return SsnStatus.Valid
        End If
    End Function

    ''' <summary>
    ''' Checks to see if the user is logged into the session and retrieves the SSN & Account Numbers from TX1J
    ''' </summary>
    ''' <param name="ssnAccNum"></param>
    ''' <param name="whoAmIWasUsed"></param>
    ''' <param name="accountNumber"></param>
    ''' <param name="ssn"></param>
    ''' <param name="doLvcTracking"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckCompass(ByVal ssnAccNum As String, ByRef whoAmIWasUsed As Boolean, ByRef accountNumber As String, ByRef ssn As String, Optional ByRef doLvcTracking As Boolean = False) As SsnStatus
        FastPathInput("TX3Z/ITX1JB;" + ssnAccNum)
        If CheckForText(1, 2, "UNSUPPORTED FUNCTION") Then
            MessageBox.Show("You are not logged into Compass. Please log in and try again", "Not logged in", MessageBoxButtons.OK, MessageBoxIcon.Information)
            whoAmIWasUsed = False
        End If

        If CheckForText(1, 71, "TXX1R") = False Then
            MessageBox.Show("The SSN/Account Number for this borrower was not foundin Compass", "Borrower not in Compass", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return SsnStatus.Invalid
        End If

        ssn = GetText(3, 12, 11).Replace(" ", "")
        accountNumber = GetText(3, 34, 12).Replace(" ", "")
        If whoAmIWasUsed = False Then
            Dim message As String = String.Format("   -First Name = {0}{1}   -Last Name = {2}{1}{1}Is this the borrower?", GetText(4, 34, 13), Environment.NewLine, GetText(4, 6, 23))
            If MessageBox.Show(message, "Confirm Borrower", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Return SsnStatus.Valid
            Else
                Return SsnStatus.Invalid
            End If
        Else
            whoAmIWasUsed = False
            Return SsnStatus.Valid
        End If
    End Function

    Public Function NotInLCO(ByVal accountNumber As String) As Boolean
        If TestMode() Then
            MessageBox.Show("Please log into LCO test and then click OK.", "Switch to LCO", MessageBoxButtons.OK)
        End If
        FastPathInput(String.Format("TPDD{0}", accountNumber))
        Dim borrowerIsNotFound As Boolean = CheckForText(2, 2, "NO CONSOLIDATION PERSONAL DEMOGRAPHICS RECORD EXISTS FOR SS#")
        If TestMode() Then
            MessageBox.Show("Please log into regular test and then click OK.", "Switch Back", MessageBoxButtons.OK)
        End If
        Return borrowerIsNotFound
    End Function

    'this function does the LG10 and TS24 checks.  If it returns false then the the document shouldn't be corr logged
    Public Function CorrLogLG10andTS24Confirm(ByVal ssn As String) As Boolean
        FastPathInput(String.Format("LG10I{0}", ssn))
        'search for 700126 servicer code
        Dim servicerCodeWasFound As Boolean = False
        If CheckForText(1, 53, "LOAN BWR STATUS RECAP SELECT") Then
            'selection screen
            Dim row As Integer = 7
            While CheckForText(20, 3, "46004 NO MORE DATA TO DISPLAY") = False
                If CheckForText(row, 46, "700126") Then
                    servicerCodeWasFound = True
                    Exit While
                End If
                row += 1
                'when the script finds the end of the screen
                If row = 19 Then
                    row = 7
                    Press("F8")
                End If
            End While
        Else
            'target screen
            servicerCodeWasFound = CheckForText(5, 27, "700126")
        End If

        If servicerCodeWasFound Then
            'Do TS24 check
            FastPathInput(String.Format("TX3ZITS24{0}", ssn))
            Return CheckForText(1, 76, "TSX25")
        Else
            Return False
        End If
    End Function

    Public Sub InitSession()
        Try
            RIBM = GetObject(, "ReflectionIBM.Session")
        Catch ex As Exception
            MessageBox.Show("You must have a Reflection session open.  Please open a Reflection session and try again.", "No Session Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Sub

    Public Function TestMode() As Boolean
        Try
            Return RIBM.CommandLineSwitches.ToLower().Contains("tst.rsf") OrElse RIBM.CommandLineSwitches.ToLower().Contains("dev.rsf")
        Catch ex As Exception
            MessageBox.Show("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Function

    'Checks for specific text at a certain location on the screen
    Public Function CheckForText(ByVal row As Integer, ByVal column As Integer, ByVal text As String) As Boolean
        Try
            Return (RIBM.GetDisplayText(row, column, text.Length) = text)
        Catch ex As Exception
            MessageBox.Show("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Function

    Public Function GetText(ByVal row As Integer, ByVal column As Integer, ByVal length As Integer) As String
        Try
            Return RIBM.GetDisplayText(row, column, length).ToString().Trim()
        Catch ex As Exception
            MessageBox.Show("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Function

    'this function will transmit a key for you
    Public Sub Press(ByVal key As String, Optional ByVal keyset As String = "1")
        Try
            If CheckForText(23, 23, keyset) Then
                RIBM.TransmitTerminalKey(rcIBMPf2Key)
                RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            End If
            Select Case key.ToUpper()
                Case "F1"
                    RIBM.TransmitTerminalKey(rcIBMPf1Key)
                Case "F2"
                    RIBM.TransmitTerminalKey(rcIBMPf2Key)
                Case "F3"
                    RIBM.TransmitTerminalKey(rcIBMPf3Key)
                Case "F4"
                    RIBM.TransmitTerminalKey(rcIBMPf4Key)
                Case "F5"
                    RIBM.TransmitTerminalKey(rcIBMPf5Key)
                Case "F6"
                    RIBM.TransmitTerminalKey(rcIBMPf6Key)
                Case "F7"
                    RIBM.TransmitTerminalKey(rcIBMPf7Key)
                Case "F8"
                    RIBM.TransmitTerminalKey(rcIBMPf8Key)
                Case "F9"
                    RIBM.TransmitTerminalKey(rcIBMPf9Key)
                Case "F10"
                    RIBM.TransmitTerminalKey(rcIBMPf10Key)
                Case "F11"
                    RIBM.TransmitTerminalKey(rcIBMPf11Key)
                Case "F12"
                    RIBM.TransmitTerminalKey(rcIBMPf12Key)
                Case "ENTER"
                    RIBM.TransmitTerminalKey(rcIBMEnterKey)
                Case "CLEAR"
                    RIBM.TransmitTerminalKey(rcIBMClearKey)
                Case "END"
                    RIBM.TransmitTerminalKey(rcIBMEraseEOFKey)
                Case "UP"
                    RIBM.TransmitTerminalKey(rcIBMPA1Key)
                Case "TAB"
                    RIBM.TransmitTerminalKey(rcIBMTabKey)
                Case "HOME"
                    RIBM.TransmitTerminalKey(rcIBMHomeKey)
                Case "INS"
                    RIBM.TransmitTerminalKey(rcIBMInsertKey)
                Case Else
                    MessageBox.Show("There has been a key code error.  Please contact a programmer.", "Key Code Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End
            End Select
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            MessageBox.Show("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Sub

    'Enters information into the Fast Path.
    Public Sub FastPathInput(ByVal text As String)
        Try
            RIBM.TransmitTerminalKey(rcIBMClearKey)
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            RIBM.TransmitANSI(text)
            RIBM.TransmitTerminalKey(rcIBMEnterKey)
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            MessageBox.Show("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Sub

    'Enters inp into the given X,Y coordinates
    Public Sub PutText(ByVal row As Integer, ByVal column As Integer, ByVal text As String, Optional ByVal hitEnter As Boolean = False)
        Try
            RIBM.MoveCursor(row, column)
            RIBM.TransmitANSI(text)
            'if enter = true then hit enter.
            If (hitEnter) Then
                RIBM.TransmitTerminalKey(rcIBMEnterKey)
                RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            End If
        Catch ex As Exception
            MessageBox.Show("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Sub
#End Region

    Sub ActivatePreviousInstance(ByVal targetAppWindowTitle As String)
        Dim previousProcess As Process = ( _
            From p In Process.GetProcesses() _
            Where p.MainWindowTitle.ToUpper() = targetAppWindowTitle.ToUpper() _
        ).FirstOrDefault()

        If previousProcess IsNot Nothing Then
            'Restore the program.
            OpenIcon(previousProcess.MainWindowHandle.ToInt32())
            'Activate the application.
            SetForegroundWindow(previousProcess.MainWindowHandle.ToInt32())
        End If
    End Sub
End Module
