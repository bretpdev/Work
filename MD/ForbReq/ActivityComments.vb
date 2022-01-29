Public Class ActivityComments

    'This function adds comments to COMPASS and OneLINK
    Public Sub AddCommentsToLP50AndTD22(ByVal SSN As String, ByVal Comment As String, ByVal ActionCode As String, ByVal ContactType As String, ByVal ActivityType As String, ByVal ARC As String)
        AddCommentsToLP50(SSN, Comment, ActionCode, ContactType, ActivityType)
        AddCommentsToTD22AllLoans(SSN, Comment, ARC)
    End Sub

    'This function adds comments to LP50
    Public Sub AddCommentsToLP50(ByVal SSN As String, ByVal Comment As String, ByVal ActionCode As String, ByVal ContactType As String, ByVal ActivityType As String)
        FastPathInput("LP50A" & SSN)
        XYInput(9, 20, ActionCode, True) 'enter action code and press enter
        XYInput(7, 2, ActivityType & ContactType) 'enter activity and contact type
        XYInput(13, 2, Comment) 'enter comment
        Press("F6") 'post comment
    End Sub

    'This function adds comments to TD22 and marks all loans
    Public Sub AddCommentsToTD22AllLoans(ByVal SSN As String, ByVal Comment As String, ByVal ARC As String)
        Dim k As Integer
        FastPathInput("TX3Z/ATD22" & SSN)
        'look for ARC
        If FindingAQueueOnTD22(ARC) = False Then
            MsgBox("You haven't been given access to the " & ARC & " ARC.  Please contact Systems Support.", MsgBoxStyle.Critical, "ARC Access Issue")
            Exit Sub
        End If
        'select all loans
        XYInput(11, 3, "X")
        While TextCheck(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            If RIBM.cursorrow = 21 Then
                Press("F8")
                XYInput(11, 3, "X")
            Else
                XYInput(RIBM.cursorrow, RIBM.cursorcolumn, "X")
            End If
        End While
        'goto the bigger comment area screen
        If TextCheck(23, 2, "01764 TASK RECORD ALREADY EXISTS") Then
            MsgBox("A queue verbal forbearance task already exists for that borrower.", MsgBoxStyle.Information)
            Exit Sub
        End If
        '<sr1377->
        'enter short comments
        If Len(Comment) < 152 Then
            XYInput(21, 2, Comment)
            Press("Enter")
        Else 'enter long comments
            'fill the first screen
            XYInput(21, 2, Mid(Comment, 1, 152))
            Press("Enter")
            Press("F4")
            'enter the rest on the expanded comments screen
            For k = 153 To Len(Comment)
                RIBM.TransmitANSI(Mid(Comment, k, 260))
                k = k + 260
            Next k
            Press("ENTER")
        End If
        'Press("F4")
        ''enter comment
        'If Comment.Length < 260 Then
        '    XYInput(8, 5, Comment, True)
        'ElseIf Comment.Length > 260 And Comment.Length < 520 Then
        '    XYInput(8, 5, Comment.Substring(0, 260))
        '    XYInput(RIBM.cursorrow, RIBM.cursorcolumn, Comment.Substring(260, Comment.Length - 261), True)
        'ElseIf Comment.Length > 520 And Comment.Length < 780 Then
        '    XYInput(8, 5, Comment.Substring(0, 260))
        '    XYInput(RIBM.cursorrow, RIBM.cursorcolumn, Comment.Substring(260, 260))
        '    XYInput(RIBM.cursorrow, RIBM.cursorcolumn, Comment.Substring(520, Comment.Length - 521), True)
        'End If
        '</sr1377>
    End Sub

    'This function adds comments to TD37 and marks all loans
    Public Sub AddCommentsToTD37(ByVal SSN As String, ByVal Comment As String, ByVal ARC As String)
        FastPathInput("TX3Z/ATD37" & SSN)
        'look for ARC
        If FindingAQueueOnTD22(ARC) = False Then
            MsgBox("You haven't been given access to the " & ARC & " ARC.  Please contact Systems Support.", MsgBoxStyle.Critical, "ARC Access Issue")
            Exit Sub
        End If
        'abort if DUDE is not on the screen to enter the activity record
        If Not TextCheck(1, 72, "TDX39") Then Exit Sub
        'select the first loan
        XYInput(11, 18, "X")
        'goto the bigger comment area screen
        Press("Enter")
        Press("F4")
        'enter comment
        XYInput(8, 5, Comment, True)
    End Sub

    'This Function Searches for a Queue on TD22 if it finds it, the function selects it and returns true, else it returns false.
    Private Function FindingAQueueOnTD22(ByVal ARC As String) As Boolean
        Dim row As Integer
        row = 8
        FindingAQueueOnTD22 = False
        While (Not TextCheck(23, 2, "90007 NO MORE DATA TO DISPLAY"))
            If (TextCheck(row, 7, " " & ARC & " ")) Then
                XYInput(row, 3, "01", True)
                FindingAQueueOnTD22 = True
                Exit Function
            Else
                If (TextCheck(row, 47, " " & ARC & " ")) Then
                    XYInput(row, 43, "01", True)
                    FindingAQueueOnTD22 = True
                    Exit Function
                End If
            End If
            row = row + 1
            If (Not row < 23) Then
                Press("F8")
                row = 8
            End If
        End While
    End Function
End Class
