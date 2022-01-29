Imports Reflection.SessionClass
Imports Reflection.Constants
Imports Reflection.Settings
Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO

Public Module Q
    Public RIBM As Object
    Public Session As Reflection.Session
    Public UserId As String
    Public Password As String
    Public TestModeBool As Boolean
    'create timer to handle system time outs
    Public TimeOutTimer As System.timers.Timer
    Public TimerInterval As Double = 1050000 '17.5 minutes
    Public Processing As New frmProcessing
    Public HawaiianDict As frmHawaiianDictionary
    Public AboutDUDE As frmAboutDUDE
    Public BorrInfo As New frm411
    Public AskDUDE As MDAskDUDE.frmAskDUDE
    Public UsrInf As UserInfo

    Public Sub TimeOutPrompt(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        Dim ErrorDisplayTime As DateTime
        ErrorDisplayTime = Now
        If SP.frmYesNo.YesNo("It has been 17.5 minutes since DUDE interacted with the system, which means your session is about to time out. Would you like DUDE to keep you connected?", True) Then
            'if yes 
            TooLongToAnswerTimeOutPrompt(ErrorDisplayTime) 'may still shut down dude if it took the user too long to answer prompt
            'create and start timer again
            TimeOutTimer = New System.Timers.Timer(TimerInterval)
            AddHandler TimeOutTimer.Elapsed, AddressOf TimeOutPrompt
            TimeOutTimer.AutoReset = False
            TimeOutTimer.Enabled = True
            TimeOutTimer.Start()
            'hit F5 to keep session alive
            Hit("F5")
        Else
            'if no
            SP.RIBM.Exit() 'exit Reflection
            Process.GetCurrentProcess.Kill() 'kill DUDE
        End If
    End Sub

    'if the user takes too long to answer the prompt then the system has logged them out and this function handles that
    Public Function TooLongToAnswerTimeOutPrompt(ByVal EDT As DateTime) As Boolean
        Dim TimeSpanCompareResult As Integer
        TimeSpanCompareResult = TimeSpan.Compare(Now.Subtract(EDT), New TimeSpan(0, 2, 30))
        If TimeSpanCompareResult = 0 Or TimeSpanCompareResult = 1 Then 'equal to 2.5 minutes or more than then
            frmWipeOut.WipeOut("You waited too long and your session may have now timed out.  Maui DUDE is now shutting down.", "Session Timed Out")
            SP.RIBM.Exit() 'exit Reflection
            Process.GetCurrentProcess.Kill() 'kill DUDE
        End If
    End Function

    Public Function Check4Text(ByVal y As Integer, ByVal x As Integer, ByVal i As String) As Boolean
        Try
            If (RIBM.GetDisplayText(y, x, Len(i)) = i) Then
                Check4Text = True
            Else
                Check4Text = False
            End If
        Catch ex As Exception
            'GetExistingSession("")
            If RIBM Is Nothing Then
                SP.frmWipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
                      "         - You currently don't have a Reflection session open." & vbLf & _
                      "         - The Reflection session that you originally had opened " & vbLf & _
                      "           has been shutdown and restarted." & vbLf & _
                      "         - A lolo mano is using our network as a pu pu." & vbLf & vbLf & _
                      "In any case please be sure that you have a current Reflection session" & vbLf & _
                      "open, restart DUDE, and DUDE will then jive with Reflection.", "Jive Link Failure")
                Process.GetCurrentProcess.Kill()
            End If
        End Try
    End Function

    Public Function GetText(ByVal y As Integer, ByVal x As Integer, ByVal len As Integer) As String
        Try
            GetText = Trim(RIBM.GetDisplayText(y, x, len))
        Catch ex As Exception
            'GetExistingSession("")
            If RIBM Is Nothing Then
                SP.frmWipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
                     "         - You currently don't have a Reflection session open." & vbLf & _
                     "         - The Reflection session that you originally had opened " & vbLf & _
                     "           has been shutdown and restarted." & vbLf & _
                     "         - A lolo mano is using our network as a pu pu." & vbLf & vbLf & _
                     "In any case please be sure that you have a current Reflection session" & vbLf & _
                     "open, restart DUDE, and DUDE will then jive with Reflection.", "Jive Link Failure")
                Process.GetCurrentProcess.Kill()
            End If
            GetText = ""
        End Try
    End Function

    'this function will transmit a key for you
    Public Function Hit(ByVal key As String, Optional ByVal keyset As String = "1") As Boolean
        Try
            key = UCase(key)
            If Check4Text(23, 23, keyset) Then
                RIBM.TransmitTerminalKey(rcIBMPf2Key)
                RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            End If
            Select Case key
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
                Case Else
                    SP.frmWipeOut.WipeOut("There has been a key code error.  Please contact a programmer.", "Bad Move", True)
            End Select
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            'do the session time out timer only if the BU selected by user has a home page
            If UsrInf.BUHasHomePage Then
                'check if timer has been created yet
                If TimeOutTimer Is Nothing Then
                    'create timer
                    TimeOutTimer = New System.Timers.Timer(TimerInterval)
                    AddHandler TimeOutTimer.Elapsed, AddressOf TimeOutPrompt
                    TimeOutTimer.AutoReset = False
                    TimeOutTimer.Enabled = True
                    TimeOutTimer.Start()
                Else
                    'if timer has been created then just reset it
                    TimeOutTimer.Interval = TimerInterval 'reset timer
                End If
            End If
        Catch ex As Exception
            'GetExistingSession("")
            If RIBM Is Nothing Then
                SP.frmWipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
                       "         - You currently don't have a Reflection session open." & vbLf & _
                       "         - The Reflection session that you originally had opened " & vbLf & _
                       "           has been shutdown and restarted." & vbLf & _
                       "         - A lolo mano is using our network as a pu pu." & vbLf & vbLf & _
                       "In any case please be sure that you have a current Reflection session" & vbLf & _
                       "open, restart DUDE, and DUDE will then jive with Reflection.", "Jive Link Failure")
                Process.GetCurrentProcess.Kill()
            End If
        End Try
    End Function

    'this function pauses Maui DUDE until insert is hit
    Public Sub UserInputWait()
        Try
            SP.frmWhoaDUDE.WhoaDUDE("Whoa Dude, there was a problem while updating the system.  Please take a moment and make it mo'betta, then click OK.", "Make it Mo'betta", True)
        Catch ex As Exception
            'GetExistingSession("")
            If RIBM Is Nothing Then
                SP.frmWipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
                     "         - You currently don't have a Reflection session open." & vbLf & _
                     "         - The Reflection session that you originally had opened " & vbLf & _
                     "           has been shutdown and restarted." & vbLf & _
                     "         - A lolo mano is using our network as a pu pu." & vbLf & vbLf & _
                     "In any case please be sure that you have a current Reflection session" & vbLf & _
                     "open, restart DUDE, and DUDE will then jive with Reflection.", "Jive Link Failure")
                Process.GetCurrentProcess.Kill()
            End If
        End Try
    End Sub

    'Enters information into the Fast Path.
    Public Sub FastPath(ByVal inp As String)
        Try
            RIBM.TransmitTerminalKey(rcIBMClearKey)
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            RIBM.TransmitANSI(inp)
            RIBM.TransmitTerminalKey(rcIBMEnterKey)
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            'GetExistingSession("")
            If RIBM Is Nothing Then
                SP.frmWipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
                    "         - You currently don't have a Reflection session open." & vbLf & _
                    "         - The Reflection session that you originally had opened " & vbLf & _
                    "           has been shutdown and restarted." & vbLf & _
                    "         - A lolo mano is using our network as a pu pu." & vbLf & vbLf & _
                    "In any case please be sure that you have a current Reflection session" & vbLf & _
                    "open, restart DUDE, and DUDE will then jive with Reflection.", "Jive Link Failure")
                Process.GetCurrentProcess.Kill()
            End If
        End Try
    End Sub

    'Enters inp into the given X,Y coordinates
    Public Sub PutText(ByVal y As Integer, ByVal x As Integer, ByVal inp As String, Optional ByVal Enter As Boolean = False)
        Dim inpSize As Long
        Try
            RIBM.MoveCursor(y, x)
            If inp.Length > 260 Then
                While (inp.Length - inpSize) > 260
                    RIBM.TransmitANSI(Mid(inp, inpSize + 1, 260))
                    inpSize = inpSize + 260
                End While
                RIBM.TransmitANSI(Mid(inp, inpSize + 1, (inp.Length - inpSize)))
            Else
                RIBM.TransmitANSI(inp)
            End If
            'if enter = true then hit enter.
            If (Enter) Then
                Hit("Enter")
            End If
        Catch ex As Exception
            'GetExistingSession("")
            If RIBM Is Nothing Then
                SP.frmWipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
                        "         - You currently don't have a Reflection session open." & vbLf & _
                        "         - The Reflection session that you originally had opened " & vbLf & _
                        "           has been shutdown and restarted." & vbLf & _
                        "         - A lolo mano is using our network as a pu pu." & vbLf & vbLf & _
                        "In any case please be sure that you have a current Reflection session" & vbLf & _
                        "open, restart DUDE, and DUDE will then jive with Reflection.", "Jive Link Failure")
                Process.GetCurrentProcess.Kill()
            End If
        End Try
    End Sub

    Public Function TestMode() As Boolean
        Return TestModeBool
        'Try
        '    If Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Dev" Or Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Tst" Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'Catch ex As Exception
        '    MsgBox("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", MsgBoxStyle.Critical)
        '    Process.GetCurrentProcess.Kill()
        'End Try
    End Function

    Sub LoginToLCO()
        Try
            RIBM.RunMacro("SP.QL.ToLCO", "000000000,1," & CStr(SP.Q.TestMode()))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub LoginToCompass()
        Try
            RIBM.RunMacro("SP.QL.ToCO", "000000000,1," & CStr(SP.Q.TestMode()))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub GatherLoginInfo()
        Try
            RIBM.SwitchToWindow(1)
            RIBM.RunMacro("SP.QL.GatherIDPass", "000000000,1," & CStr(SP.Q.TestMode()))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub DisplayHawaiianDictionary()
        If HawaiianDict Is Nothing OrElse HawaiianDict.IsDisposed Then
            HawaiianDict = New frmHawaiianDictionary
        End If
        If SP.HawaiianDict.Visible = True Then
            SP.HawaiianDict.Focus()
        Else
            SP.HawaiianDict.Show()
        End If
    End Sub

    Public Sub DisplayAboutDude()
        If AboutDUDE Is Nothing OrElse AboutDUDE.IsDisposed Then
            AboutDUDE = New frmAboutDUDE
        End If
        If SP.AboutDUDE.Visible = True Then
            SP.AboutDUDE.Focus()
        Else
            SP.AboutDUDE.Show()
        End If
    End Sub

    Public Sub DisplayAskDude()
        If AskDUDE Is Nothing Then
            AskDUDE = New MDAskDUDE.frmAskDUDE(TestModeBool)
            SP.Q.AskDUDE.Show(SP.UsrInf.Opacity, SP.UsrInf.BColor, SP.UsrInf.FColor)
        End If
        If SP.AskDUDE.Visible = True Then
            SP.AskDUDE.Focus()
        Else
            SP.AskDUDE.Show()
        End If
    End Sub

    'ensures the correct set of hotkeys is displayed
    Public Sub CheckSet(ByVal sSet As Integer)
        If GetText(23, 23, sSet) Then Hit("F2")
    End Sub

    'translates ACS keyline encrypted SSN into SSN 
    Public Function ACSTranslation(ByRef KeyLn As String) As String
        Dim i As Integer
        ACSTranslation = ""
        For i = 1 To 9
            KeyLn = UCase(KeyLn) 'capitalize everything
            Select Case Mid(KeyLn, i, 1)
                Case "M"
                    ACSTranslation = ACSTranslation & "0"
                Case "Y"
                    ACSTranslation = ACSTranslation & "9"
                Case "L"
                    ACSTranslation = ACSTranslation & "8"
                Case "A"
                    ACSTranslation = ACSTranslation & "7"
                Case "U"
                    ACSTranslation = ACSTranslation & "6"
                Case "G"
                    ACSTranslation = ACSTranslation & "5"
                Case "H"
                    ACSTranslation = ACSTranslation & "4"
                Case "T"
                    ACSTranslation = ACSTranslation & "3"
                Case "E"
                    ACSTranslation = ACSTranslation & "2"
                Case "R"
                    ACSTranslation = ACSTranslation & "1"
                Case Else
                    ACSTranslation = ""
                    Exit Function
            End Select
        Next
    End Function

    'send an e-mail message using SMTP
    Public Function SendMail(ByVal mFrom As String, ByVal mTo As String, Optional ByVal mSubject As String = "", Optional ByVal mBody As String = "", Optional ByVal mCC As String = "", Optional ByVal mBCC As String = "", Optional ByVal mAttach As String = "") As Boolean
        Dim aAttach() As String
        Dim i As Integer
        Dim eMail As OSSMTP.SMTPSession
        eMail = New OSSMTP.SMTPSession

        'set server
        eMail.Server = "mail.utahsbr.edu"

        'create message
        eMail.MailFrom = mFrom
        eMail.SendTo = mTo
        eMail.CC = mCC
        eMail.BCC = mBCC
        eMail.MessageSubject = mSubject
        eMail.MessageText = mBody

        'add attachments if there are any
        If Len(mAttach) > 0 Then
            'split file names from string
            aAttach = Split(mAttach, ",")

            'add attachments
            For i = 0 To UBound(aAttach)
                eMail.Attachments.Add(aAttach(i))
            Next i
        End If

        'send message
        eMail.SendEmail()

        'verify the message was sent
        If eMail.Status = "SMTP connection closed" Then
            SendMail = True
        Else
            SendMail = False
        End If
    End Function

End Module
