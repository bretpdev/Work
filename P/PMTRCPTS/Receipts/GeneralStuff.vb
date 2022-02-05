Imports Reflection.SessionClass
Imports Reflection.Constants
Imports Reflection.Settings
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Word

Module GeneralStuff

    Public RIBM As Object 'reflection session
    Public PleaseWait As New Please_Wait
    Public HasSession As Boolean
    Public Test As Boolean = False

#Region " General Use Functionality "

    'enter comments in LP50
    Public Function AddLP50(ByVal SSN As String, ByVal ActCd As String, ByVal Script As String, Optional ByVal ActTyp As String = "", Optional ByVal ConTyp As String = "", Optional ByVal comment As String = "", Optional ByVal PauPlea As Boolean = False) As Boolean
        Dim row As Integer
        Dim col As Integer
        Dim warn As Integer

        AddLP50 = True
        With RIBM
            'access LP50
            FastPathInput("LP50A" & SSN & ";;;" & ActTyp & ";" & ConTyp & ";" & ActCd)
            'pause for the user to enter the activity type and contact type if the acttyp is blank
            If ActTyp = "" Then
                MsgBox("Enter the activity type and the contact type and then hit <Insert> to resume the script.", 48, "Enter Contact Type and Activity Type") '<3>
                .WaitForTerminalKey(rcIBMInsertKey, "1:00:00")
                Press("INS")
                Press("ENTER")
            ElseIf TextCheck(7, 2, "__") Then
                XYInput(7, 2, ActTyp & ConTyp)
            End If
            'enter the comment
            If PauPlea = True Then
                XYInput(13, 2, comment)
                warn = MsgBox("Do you want to enter additional comments?  Click Yes to pause the script, add the additional comments, and then hit <Insert> to resume the script.  Click No to continue processing without adding additional comments.", 36, "Add Additional Comments")
                If warn = 6 Then
                    .WaitForTerminalKey(rcIBMInsertKey, "1:00:00")
                    Press("INS")
                End If
                row = 13
                col = 2
                Do Until row = 21
                    If .GetDisplayText(row, col, 12) = "____________" And _
                      (.GetDisplayText(row + 1, 2, 1) = "_" Or .GetDisplayText(row + 1, 2, 1) = " ") Then
                        XYInput(row, col, "  {" & Script & "}")
                        Exit Do
                    ElseIf col = 62 Then
                        row = row + 1
                        col = 2
                    Else
                        col = col + 1
                    End If
                Loop
            Else
                XYInput(13, 2, comment & "  {" & Script & "}")
            End If
            Press("F6")
            If TextCheck(22, 3, "48003") <> True Then AddLP50 = False
        End With
    End Function

    'enters an activity record/action request in COMPASS selecting only the loans specified
    Function ATD22AllLoans(ByVal SSN As String, ByVal ARC As String, ByVal comment As String, ByVal Script As String, ByVal userid As String) As Boolean
        Dim found As Boolean
        ATD22AllLoans = True
        FastPathInput("TX3Z/ATD22" & SSN)
        If Not TextCheck(1, 72, "TDX23") Then
            ATD22AllLoans = False
            Exit Function
        End If
        'find the ARC
        Do
            found = RIBM.FindText(ARC, 8, 8)
            If found Then Exit Do
            Press("F8")
            If TextCheck(23, 2, "90007") Then
                ATD22AllLoans = False
                Exit Function
            End If
        Loop
        'select the ARC
        XYInput(RIBM.FoundTextRow, RIBM.FoundTextColumn - 5, "01", True)
        Do
            XYInput(11, 3, "XXXXXXXX")
            If TextCheck(8, 75, "+") Then Press("F8") Else Exit Do
        Loop
        'blank extra X's and access expanded comments screen
        XYInput(21, 2, "")
        Press("End")
        Press("ENTER")
        If Not TextCheck(23, 2, "02860") Then
            ATD22AllLoans = False
            Exit Function
        End If
        Press("F4")
        'enter comment
        XYInput(8, 5, comment & "  {" & Script & "} /" & userid, True)
        If Not TextCheck(23, 2, "02114") Then ATD22AllLoans = False
    End Function

    'get the user ID
    Function GetUserID() As String
        FastPathInput("LP40I")
        'check if the user is logged into OneLINK
        If TextCheck(1, 2, "LP40I COMMAND UNRECOGNIZED") Or TextCheck(3, 21, "OPERATOR IS NOT LOGGED ON") Then
            MsgBox("You aren't logged into OneLINK.  Please log on then try again.", MsgBoxStyle.Information)
            End
        End If
        If Not TextCheck(1, 77, "ANCE") Then Press("ENTER")
        GetUserID = RIBM.GetDisplayText(3, 14, 7)
    End Function

    

    'this function checks to be sure that the user is logged on to OneLINK and that the borrower is on the system
    Public Function OneLINK(ByRef SSN As String, ByRef AccNumForDB As String, ByVal AccNum As String) As Boolean
        'do datachecks on the OneLink and COMPASS
        '<1->     FastPathInput("LP22I;;;;;;" & AccNum)
        If AccNum.Length = 10 Then
            FastPathInput("LP22I;;;;;;" & AccNum)
        Else
            FastPathInput("LP22I" & AccNum)
        End If
        '</1>
        'check if the user is logged into OneLINK
        If TextCheck(1, 2, "LP22I COMMAND UNRECOGNIZED") Or TextCheck(3, 21, "OPERATOR IS NOT LOGGED ON") Then
            MsgBox("You aren't logged into OneLINK.  Please log on then try again.", MsgBoxStyle.Information)
            Return False
        End If
        'check if the borrower is on OneLINK
        If TextCheck(1, 62, "PERSON DEMOGRAPHICS") = False Then
            MsgBox("That account number wasn't found on OneLINK.", MsgBoxStyle.Information)
            Return False
        End If
        'Retrieve the borrowers SSN and return True
        SSN = GetText(3, 23, 9)
        'gather Account number and get rid of extra spaces
        AccNumForDB = GetText(3, 60, 12)                            '<1>
        AccNumForDB = AccNumForDB.Replace(" ", "")                                '<1>
        Return True
    End Function

    'PrintDocs merges the merge data source text file with the specified document
    Public Sub PrintDocs(ByVal DOC As String)
        ''set up Word object
        Dim Word As New Microsoft.Office.Interop.Word.Application
        Word.Visible = False
        With Word
            Word.Documents.Open(FileName:=DOC, ConfirmConversions:=False, _
            ReadOnly:=True, AddToRecentFiles:=False, PasswordDocument:="", _
            PasswordTemplate:="", Revert:=False, WritePasswordDocument:="", _
            WritePasswordTemplate:="")
            .ActiveDocument.MailMerge.OpenDataSource(Name:= _
                "T:\ReceiptDat.txt", ConfirmConversions:=False, ReadOnly:= _
                False, LinkToSource:=True, AddToRecentFiles:=False, PasswordDocument:="", _
                PasswordTemplate:="", WritePasswordDocument:="", WritePasswordTemplate:= _
                "", Revert:=False, Connection:="", SQLStatement _
                :="", SQLStatement1:="")
            With .ActiveDocument.MailMerge
                .Destination = .Destination.wdSendToPrinter
                .SuppressBlankLines = True
                .Execute(Pause:=False)
            End With
            .Application.Quit(False)
        End With
        MsgBox("Please pick up the printed receipts from the printer.", MsgBoxStyle.Information)
    End Sub

    Public Sub InitSession()
        Try
            RIBM = GetObject(, "ReflectionIBM.Session")
            HasSession = True
        Catch ex As Exception
            HasSession = False
            'MsgBox("You must have a Reflection session open.  Please open a Reflection session and try again.", MsgBoxStyle.Critical)
            'End
        End Try
    End Sub

    'there is also a Public Test variable at the top of this module, it appears this function isn't even used
    Public Function TestMode() As Boolean
        Try
            If Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Dev" Or Mid(RIBM.CommandLineSwitches, Len(RIBM.CommandLineSwitches) - 7, 3) = "Tst" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", MsgBoxStyle.Critical)
            End
        End Try
    End Function

    'Checks for specific text at a certain location on the screen
    Public Function TextCheck(ByVal y As Integer, ByVal x As Integer, ByVal i As String) As Boolean
        Try
            If (RIBM.GetDisplayText(y, x, Len(i)) = i) Then
                TextCheck = True
            Else
                TextCheck = False
            End If
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", MsgBoxStyle.Critical)
            End
        End Try
    End Function

    Public Function GetText(ByVal y As Integer, ByVal x As Integer, ByVal len As Integer) As String
        Try
            GetText = Trim(RIBM.GetDisplayText(y, x, len))
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", MsgBoxStyle.Critical)
            End
        End Try
    End Function

    'this function will transmit a key for you
    Public Sub Press(ByVal key As String, Optional ByVal keyset As String = "1")
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
                Case "INS"
                    RIBM.TransmitTerminalKey(rcIBMInsertKey)
                Case Else
                    MsgBox("There has been a key code error.  Please contact a programmer.", MsgBoxStyle.Critical)
                    End
            End Select
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", MsgBoxStyle.Critical)
            End
        End Try
    End Sub

    'Enters information into the Fast Path.
    Public Sub FastPathInput(ByVal inp As String)
        Try
            RIBM.TransmitTerminalKey(rcIBMClearKey)
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            RIBM.TransmitANSI(inp)
            RIBM.TransmitTerminalKey(rcIBMEnterKey)
            RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", MsgBoxStyle.Critical)
            End
        End Try
    End Sub

    'Enters inp into the given X,Y coordinates
    Public Sub XYInput(ByVal y As Integer, ByVal x As Integer, ByVal inp As String, Optional ByVal Enter As Boolean = False)
        Try
            RIBM.MoveCursor(y, x)
            RIBM.TransmitANSI(inp)
            'if enter = true then hit enter.
            If (Enter) Then
                RIBM.TransmitTerminalKey(rcIBMEnterKey)
                RIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            End If
        Catch ex As Exception
            MsgBox("An error has occured while trying to communicate with your Reflection session.  Please restart this application after you have opened a Reflection session.", MsgBoxStyle.Critical)
            End
        End Try
    End Sub

#End Region
End Module
