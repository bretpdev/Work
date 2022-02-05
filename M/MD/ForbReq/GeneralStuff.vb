Imports Reflection.SessionClass
Imports Reflection.Constants
Imports Reflection.Settings

Module GeneralStuff
    Public RIBM As Object

    Sub InitSession()
        Try
            RIBM = GetObject("RIBM")
            If RIBM Is Nothing Then
                Throw New Exception
            End If
        Catch ex As Exception
            Throw New Exception
        End Try
    End Sub

    Sub GetExistingSession(ByVal Tssn As String)
        Dim objProcess As New Process
        'Collection of all the Processes running on local machine
        Dim objProcesses() As Process
        'Get all processes into the collection
        objProcesses = Process.GetProcessesByName("R8win")

        For Each objProcess In objProcesses
            If InStr(objProcess.MainWindowTitle, ".rsf") And InStr(objProcess.MainWindowTitle, "Nelnet") = 0 Then

                If InStr(objProcess.MainWindowTitle, "Tst.") Or InStr(objProcess.MainWindowTitle, "dev.") Then
                    'test session
                    Try
                        RIBM = GetObject("X:\Sessions\Test\" & objProcess.MainWindowTitle)

                        FastPathInput("LP22I" & Tssn)
                        'check if the user is logged into OneLINK and COMPASS check if OLEserverName = RIBM. Must not be NELNET
                        If TextCheck(1, 2, "INVALID COMMAND SYNTAX") Or TextCheck(3, 11, "CICS0001: OPERATOR IS NOT LOGGED ON.") Or RIBM.OLEServerName() <> "RIBM" Then
                            Throw (New Exception)
                        End If
                        Exit For
                    Catch ex As Exception
                        RIBM = Nothing
                    End Try
                Else
                    'live session
                    Try
                        RIBM = GetObject("X:\Sessions\" & objProcess.MainWindowTitle)
                        FastPathInput("LP22I")
                        'check if the user is logged into OneLINK and COMPASS
                        If TextCheck(1, 2, "INVALID COMMAND SYNTAX") Or TextCheck(3, 11, "CICS0001: OPERATOR IS NOT LOGGED ON.") Then
                            Throw (New Exception)
                        End If
                        Exit For
                    Catch
                        RIBM = Nothing
                    End Try
                End If
            End If
        Next
    End Sub

    'Checks for specific text at a certain location on the screen
    Function TextCheck(ByVal y As Integer, ByVal x As Integer, ByVal i As String) As Boolean
        Try
            If (RIBM.GetDisplayText(y, x, Len(i)) = i) Then
                TextCheck = True
            Else
                TextCheck = False
            End If
        Catch ex As Exception
            GetExistingSession("")
            If RIBM Is Nothing Then
                Throw New Exception
            End If
            TextCheck = False
        End Try
    End Function

    Function GetText(ByVal y As Integer, ByVal x As Integer, ByVal len As Integer) As String
        Try
            GetText = Trim(RIBM.GetDisplayText(y, x, len))
        Catch ex As Exception
            GetExistingSession("")
            If RIBM Is Nothing Then
                Throw New Exception
            End If
            GetText = ""
        End Try
    End Function

    'this function will transmit a key for you
    Sub Press(ByVal key As String, Optional ByVal keyset As String = "1")
        Try
            key = UCase(key)
            If TextCheck(23, 23, keyset) Then
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
                    MsgBox("There has been a key code error.  Please contact a programmer.", MsgBoxStyle.Critical, "Bad Move")
            End Select
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            GetExistingSession("")
            If RIBM Is Nothing Then
                Throw New Exception
            End If
        End Try
    End Sub

    'Enters information into the Fast Path.
    Sub FastPathInput(ByVal inp As String)
        Try
            RIBM.TransmitTerminalKey(rcIBMClearKey)
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            RIBM.TransmitANSI(inp)
            RIBM.TransmitTerminalKey(rcIBMEnterKey)
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            GetExistingSession("")
            If RIBM Is Nothing Then
                Throw New Exception
            End If
        End Try
    End Sub

    'Enters inp into the given X,Y coordinates
    Sub XYInput(ByVal y As Integer, ByVal x As Integer, ByVal inp As String, Optional ByVal Enter As Boolean = False)
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
                RIBM.TransmitTerminalKey(rcIBMEnterKey)
                RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            End If
        Catch ex As Exception
            GetExistingSession("")
            If RIBM Is Nothing Then
                Throw New Exception
            End If
        End Try
    End Sub

    'ensures the correct set of hotkeys is displayed
    Sub CheckSet(ByVal sSet As Integer)
        If GetText(23, 23, sSet) Then Press("F2")
    End Sub

    'translates ACS keyline encrypted SSN into SSN 
    Function ACSTranslation(ByRef KeyLn As String) As String
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

    Public Function TestMode() As Boolean
        Try
            If Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Dev" Or Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Tst" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception
        End Try
    End Function
End Module
