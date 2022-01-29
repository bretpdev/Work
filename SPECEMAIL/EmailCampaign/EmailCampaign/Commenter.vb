Imports Reflection.SessionClass
Imports Reflection.Constants
Imports Reflection.Settings

Public Class Commenter

    Private _rIBM As Object 'reflection session
    Private _userID As String

    Public Sub New()
        InitSession()
        FastPath("LP40I;")
        Hit("enter")
        _userID = GetText(3, 14, 7) 'get user ID
    End Sub

    'this function gets the SSN from LP22
    Public Function AccountNumberTranslation(ByVal AcctNum As String) As String
        FastPath("LP22I;;;;;;" & AcctNum)
        If Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then
            Return "" 'return blank (account number not found on the system)
        Else
            Return GetText(3, 23, 9) 'return SSN
        End If
    End Function

    'This function adds comments to LP50
    Public Function AddCommentsToLP50(ByVal SSN As String, ByVal Comment As String, ByVal ActionCode As String, ByVal ContactType As String, ByVal ActivityType As String) As Boolean
        FastPath("LP50A" & SSN)
        PutText(9, 20, ActionCode, True) 'enter action code and sp.q.hit enter
        Do While Check4Text(22, 3, "44157 ACTION CODE IS NOT A VALID ACTION CODE") Or Check4Text(22, 3, "40004 THIS FIELD MUST NOT BE LEFT BLANK - DATA MUST BE ENTERED")
            Return False
        Loop
        PutText(7, 2, ActivityType & ContactType) 'enter activity and contact type
        PutText(13, 2, Comment) 'enter comment
        Hit("F6") 'post comment
        If Check4Text(22, 3, "48003") = False And Check4Text(22, 3, "48081") = False Then
            Return False
        End If
        Return True
    End Function

    'This function adds comments to TD22 and marks all loans
    Public Function AddCommentsToTD22AllLoansWithBal(ByVal SSN As String, ByVal Comment As String, ByVal ARC As String) As Boolean
        Dim AtleastOneLoanSelected As Boolean = False
        Dim Row As Integer = 11
        FastPath("TX3Z/ATD22" & SSN)
        If Check4Text(23, 2, "01019 ENTERED KEY NOT FOUND") = True Then Exit Function
        'look for ARC
        If FindingAQueueOnTD22(ARC) = False Then
            Return False
        End If
        If Check4Text(23, 2, "50108 NO LOANS FOUND FOR THE BORROWER") Then
            Return False
        End If
        'select all loans with balances
        While Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            'if the line has data on it then check if the loan has a balance
            If CDbl(GetText(Row, 67, 12)) > 0 Then
                PutText(Row, 3, "X")
                AtleastOneLoanSelected = True
            End If
            Row = Row + 1
            If Check4Text(Row, 3, "_") = False Then
                Row = 11
                Hit("F8")
            End If
        End While
        'check if anything was selected and if not the return false
        If AtleastOneLoanSelected = False Then
            Return False
        End If
        'goto the bigger comment area screen
        Hit("Enter")
        Hit("F4")
        'enter comment
        PutText(8, 5, Comment + String.Format(" ({0})", _userID), True)
        'check if comment took
        If Check4Text(23, 2, "02114 RECORD(S) SUCCESSFULLY ADDED") = False And Check4Text(23, 2, "01003 NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED") = False Then
            Return False
        End If
        Return True
    End Function

    'This function adds comments to TD37 and marks all loans
    Public Function AddCommentsToTD37(ByVal SSN As String, ByVal Comment As String, ByVal ARC As String) As Boolean
        FastPath("TX3Z/ATD37" & SSN)
        If Check4Text(23, 2, "01019") Then Return False
        'look for ARC
        If FindingAQueueOnTD22(ARC) = False Then
            Return False
        End If
        'abort if app is not on the screen to enter the activity record
        If Not Check4Text(1, 72, "TDX39") Then Return False
        'select the first loan
        PutText(11, 18, "X")
        'goto the bigger comment area screen
        Hit("Enter")
        Hit("F4")
        'enter comment
        PutText(8, 5, Comment + String.Format(" ({0})", _userID), True)
        'check if comment took
        If Check4Text(23, 2, "02114 RECORD(S) SUCCESSFULLY ADDED") = False Then
            Return False
        End If
        Return True
    End Function

    'This Function Searches for a Queue on TD22 if it finds it, the function selects it and returns true, else it returns false.
    Private Function FindingAQueueOnTD22(ByVal ARC As String) As Boolean
        Dim row As Integer
        row = 8
        FindingAQueueOnTD22 = False
        While (Not Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
            If (Check4Text(row, 7, " " & ARC & " ")) Then
                PutText(row, 3, "01", True)
                FindingAQueueOnTD22 = True
                Exit Function
            Else
                If (Check4Text(row, 47, " " & ARC & " ")) Then
                    PutText(row, 43, "01", True)
                    FindingAQueueOnTD22 = True
                    Exit Function
                End If
            End If
            row = row + 1
            If (Not row < 23) Then
                Hit("F8")
                row = 8
            End If
        End While
    End Function

    Private Sub InitSession()
        Try
            _rIBM = GetObject(, "ReflectionIBM.Session")
        Catch ex As Exception
            MsgBox("You must have a Reflection session open.  Please open a Reflection session and try again.", MsgBoxStyle.Critical)
            End
        End Try
    End Sub

    Private Function Check4Text(ByVal y As Integer, ByVal x As Integer, ByVal i As String) As Boolean
        Try
            If (_rIBM.GetDisplayText(y, x, Len(i)) = i) Then
                Check4Text = True
            Else
                Check4Text = False
            End If
        Catch ex As Exception
            'GetExistingSession("")
            If _rIBM Is Nothing Then
                MsgBox("Something appears to have happened to the Reflection session that the application was communicating with.  Please shut down the application and the Reflection session and start them both up to try again.", MsgBoxStyle.Critical)
                End
            End If
        End Try
    End Function

    Private Function GetText(ByVal y As Integer, ByVal x As Integer, ByVal len As Integer) As String
        Try
            GetText = Trim(_rIBM.GetDisplayText(y, x, len))
        Catch ex As Exception
            'GetExistingSession("")
            If _rIBM Is Nothing Then
                MsgBox("Something appears to have happened to the Reflection session that the application was communicating with.  Please shut down the application and the Reflection session and start them both up to try again.", MsgBoxStyle.Critical)
                End
            End If
            GetText = ""
        End Try
    End Function

    'this function will transmit a key for you
    Private Function Hit(ByVal key As String, Optional ByVal keyset As String = "1") As Boolean
        Try
            key = UCase(key)
            If Check4Text(23, 23, keyset) Then
                _rIBM.TransmitTerminalKey(rcIBMPf2Key)
                _rIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            End If
            Select Case key
                Case "F1"
                    _rIBM.TransmitTerminalKey(rcIBMPf1Key)
                Case "F2"
                    _rIBM.TransmitTerminalKey(rcIBMPf2Key)
                Case "F3"
                    _rIBM.TransmitTerminalKey(rcIBMPf3Key)
                Case "F4"
                    _rIBM.TransmitTerminalKey(rcIBMPf4Key)
                Case "F5"
                    _rIBM.TransmitTerminalKey(rcIBMPf5Key)
                Case "F6"
                    _rIBM.TransmitTerminalKey(rcIBMPf6Key)
                Case "F7"
                    _rIBM.TransmitTerminalKey(rcIBMPf7Key)
                Case "F8"
                    _rIBM.TransmitTerminalKey(rcIBMPf8Key)
                Case "F9"
                    _rIBM.TransmitTerminalKey(rcIBMPf9Key)
                Case "F10"
                    _rIBM.TransmitTerminalKey(rcIBMPf10Key)
                Case "F11"
                    _rIBM.TransmitTerminalKey(rcIBMPf11Key)
                Case "F12"
                    _rIBM.TransmitTerminalKey(rcIBMPf12Key)
                Case "ENTER"
                    _rIBM.TransmitTerminalKey(rcIBMEnterKey)
                Case "CLEAR"
                    _rIBM.TransmitTerminalKey(rcIBMClearKey)
                Case "END"
                    _rIBM.TransmitTerminalKey(rcIBMEraseEOFKey)
                Case "UP"
                    _rIBM.TransmitTerminalKey(rcIBMPA1Key)
                Case "TAB"
                    _rIBM.TransmitTerminalKey(rcIBMTabKey)
                Case "HOME"
                    _rIBM.TransmitTerminalKey(rcIBMHomeKey)
                Case Else
            End Select
            _rIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            'GetExistingSession("")
            If _rIBM Is Nothing Then
                MsgBox("Something appears to have happened to the Reflection session that the application was communicating with.  Please shut down the application and the Reflection session and start them both up to try again.", MsgBoxStyle.Critical)
                End
            End If
        End Try
    End Function

    'Enters information into the Fast Path.
    Private Sub FastPath(ByVal inp As String)
        Try
            _rIBM.TransmitTerminalKey(rcIBMClearKey)
            _rIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
            _rIBM.TransmitANSI(inp)
            _rIBM.TransmitTerminalKey(rcIBMEnterKey)
            _rIBM.WaitForEvent(rcKbdEnabled, "30", "0", 1, 1)
        Catch ex As Exception
            If _rIBM Is Nothing Then
                MsgBox("Something appears to have happened to the Reflection session that the application was communicating with.  Please shut down the application and the Reflection session and start them both up to try again.", MsgBoxStyle.Critical)
                End
            End If
        End Try
    End Sub

    'Enters inp into the given X,Y coordinates
    Private Sub PutText(ByVal y As Integer, ByVal x As Integer, ByVal inp As String, Optional ByVal Enter As Boolean = False)
        Dim inpSize As Long
        Try
            _rIBM.MoveCursor(y, x)
            If inp.Length > 260 Then
                While (inp.Length - inpSize) > 260
                    _rIBM.TransmitANSI(Mid(inp, inpSize + 1, 260))
                    inpSize = inpSize + 260
                End While
                _rIBM.TransmitANSI(Mid(inp, inpSize + 1, (inp.Length - inpSize)))
            Else
                _rIBM.TransmitANSI(inp)
            End If
            If (Enter) Then
                Hit("Enter")
            End If
        Catch ex As Exception
            If _rIBM Is Nothing Then
                MsgBox("Something appears to have happened to the Reflection session that the application was communicating with.  Please shut down the application and the Reflection session and start them both up to try again.", MsgBoxStyle.Critical)
                End
            End If
        End Try
    End Sub

End Class

