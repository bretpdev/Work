Imports Reflection.SessionClass
Imports Reflection.Constants
Imports Reflection.Settings
Imports System.Drawing
Imports System.Windows.Forms

Public Module Q
    Public RIBM As Object
    Public ConnRIBMCaption As String
    Public UserId As String
    Public Password As String

    Public Function InitSession() As Boolean
        Dim SearchInt As Integer
        Dim OpeningFound As Boolean = False
        Dim TempRIBM As Object
        Dim CLS() As String
        'search for opening in sequence
        While OpeningFound = False
            Try
                SearchInt += 1
                If SearchInt = 17 Then
                    MsgBox("Maui DUDE wasn't able to find a Reflection session to connect to.  Please start up a new Reflection session for Maui DUDE to connect to.")
                    Return False
                End If
                RIBM = GetObject("RIBMMD" & SearchInt)
                OpeningFound = False
            Catch ex As Exception
                OpeningFound = True
            End Try
        End While
        Try
            RIBM = GetObject("RIBM")
            CLS = Split(Replace(RIBM.CommandLineSwitches, """", ""), "\")
            RIBM.SaveChanges = 0 'default to not save settings
            RIBM.OLEServerName = "RIBMMD" & SearchInt 'change OLE server name
            RIBM.Caption = CLS(CLS.GetUpperBound(0)).Replace(".rsf", "") & " (MD #" & SearchInt & ")" 'change caption
            ConnRIBMCaption = CLS(CLS.GetUpperBound(0)).Replace(".rsf", "") & " (MD #" & SearchInt & ")"
            If RIBM Is Nothing Then
                Throw New Exception
            End If
        Catch ex As Exception
            WipeOut.WipeOut("You must open your Reflection session first.", "Reflection not Open", True)
            Process.GetCurrentProcess.Kill()
        End Try
        Return True
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
                WipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
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

    Public Function GetText(ByVal y, ByVal x, ByVal len) As String
        Try
            GetText = Trim(RIBM.GetDisplayText(y, x, len))
        Catch ex As Exception
            'GetExistingSession("")
            If RIBM Is Nothing Then
                WipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
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
                    WipeOut.WipeOut("There has been a key code error.  Please contact a programmer.", "Bad Move", True)
            End Select
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            'GetExistingSession("")
            If RIBM Is Nothing Then
                WipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
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
    Public Function UserInputWait()
        Try
            EmailScreenError()
            WhoaDude.WhoaDUDE("Whoa Dude, there was a problem while updating the system.  Please take a moment and make it mo'betta, then click OK.", "Make it Mo'betta", True)
        Catch ex As Exception
            'GetExistingSession("")
            If RIBM Is Nothing Then
                WipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
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

    'Enters information into the Fast Path.
    Public Function FastPath(ByVal inp As String)
        Try
            RIBM.TransmitTerminalKey(rcIBMClearKey)
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            RIBM.TransmitANSI(inp)
            RIBM.TransmitTerminalKey(rcIBMEnterKey)
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            'GetExistingSession("")
            If RIBM Is Nothing Then
                WipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
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

    'Enters inp into the given X,Y coordinates
    Public Function PutText(ByVal y As Integer, ByVal x As Integer, ByVal inp As String, Optional ByVal Enter As Boolean = False)
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
            'GetExistingSession("")
            If RIBM Is Nothing Then
                WipeOut.WipeOut("DUDE can't jive with your Reflection Session.  Usually dis cuz:" & vbLf & vbLf & _
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

    Public Function TestMode() As Boolean
        Try
            If Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Dev" Or Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Tst" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", MsgBoxStyle.Critical)
            Process.GetCurrentProcess.Kill()
        End Try
    End Function

    Sub LoginToLCO()
        Try
            RIBM.RunMacro("SP.QL.ToLCO", "")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub LoginToCompass()
        Try
            RIBM.RunMacro("SP.QL.ToCO", "")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub GatherLoginInfo()
        Try
            RIBM.SwitchToWindow(1)
            RIBM.RunMacro("SP.QL.GatherIDPass", "")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Module
