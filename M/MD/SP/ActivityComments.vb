Imports Q

Public Class ActivityComments
    Inherits MDActivityComments

    Public Sub New(ByVal tSSN As String)
        MyBase.New(tSSN)
    End Sub


    'This function adds comments to COMPASS and OneLINK
    Public Overrides Function AddCommentsToLP50AndTD22(ByVal CommentO As String, ByVal CommentC As String, ByVal ActionCode As String, ByVal ContactType As String, ByVal ActivityType As String, ByVal ARC As String) As Boolean
        If AddCommentsToLP50(CommentO, ActionCode, ContactType, ActivityType) = False Or AddCommentsToTD22AllLoans(CommentC, ARC) = False Then
            Return False
        Else
            Return True
        End If
    End Function

    'This function adds comments to LP50
    Public Overrides Function AddCommentsToLP50(ByVal Comment As String, ByVal ActionCode As String, ByVal ContactType As String, ByVal ActivityType As String) As Boolean
        Dim frm As New frmGetActionCode
        frm.lblAT.Text = ActivityType
        frm.lblCT.Text = ContactType
        SP.Q.FastPath("LP50A" & SSN)
        SP.Q.PutText(9, 20, ActionCode, True) 'enter action code and sp.q.hit enter
        '40004 THIS FIELD MUST NOT BE LEFT BLANK - DATA MUST BE ENTERED
        Do While SP.Q.Check4Text(22, 3, "44157 ACTION CODE IS NOT A VALID ACTION CODE") Or SP.Q.Check4Text(22, 3, "40004 THIS FIELD MUST NOT BE LEFT BLANK - DATA MUST BE ENTERED")
            frm.txtAction.Text = ActionCode
            frm.ShowDialog()
            SP.Q.PutText(9, 20, frm.txtAction.Text, True)
        Loop
        SP.Q.PutText(7, 2, ActivityType & ContactType) 'enter activity and contact type
        SP.Q.PutText(13, 2, Comment) 'enter comment
        SP.Q.Hit("F6") 'post comment
        If SP.Q.Check4Text(22, 3, "48003") = False And SP.Q.Check4Text(22, 3, "48081") = False Then '48081 ACTIVITY ROW ADDED, DUE DILIGENCE UPDATES NOT PERFORMED
            MsgBox("Maui DUDE had problems updating LP50.  Please contact Systems Support.", MsgBoxStyle.Critical, "Error While Updating LP50")
            Return False
        End If
        Return True
    End Function

    'This function adds comments to TD22 and marks all loans
    Public Overrides Function AddCommentsToTD22AllLoans(ByVal Comment As String, ByVal ARC As String) As Boolean
        AddCommentsToTD22AllLoans = True
        SP.Q.FastPath("TX3Z/ATD22" & SSN)
        If SP.Q.Check4Text(23, 2, "01019 ENTERED KEY NOT FOUND") = True Then Exit Function
        'look for ARC
        If FindingAQueueOnTD22(ARC) = False Then
            SP.frmWhoaDUDE.WhoaDUDE("The BIG KAHUNA hasn't given you access to the " & ARC & " ARC.", "No ARC Access", True)
            If ARC = "MXADD" Then
                Return False
            Else
                Exit Function
            End If
        End If
        If SP.Q.Check4Text(23, 2, "50108 NO LOANS FOUND FOR THE BORROWER") Then
            Exit Function
        End If
        'select all loans
        SP.Q.PutText(11, 3, "X")
        While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
            If SP.Q.RIBM.cursorrow = 21 Then
                SP.Q.Hit("F8")
                SP.Q.PutText(11, 3, "X")
            Else
                SP.Q.PutText(SP.Q.RIBM.cursorrow, SP.Q.RIBM.cursorcolumn, "X")
            End If
        End While
        'goto the bigger comment area screen
        SP.Q.Hit("Enter")
        SP.Q.Hit("F4")
        'enter comment
        SP.Q.PutText(8, 5, Comment, True)
        'check if comment took
        If SP.Q.Check4Text(23, 2, "02114 RECORD(S) SUCCESSFULLY ADDED") = False And SP.Q.Check4Text(23, 2, "01003 NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED") = False Then
            MsgBox("Maui DUDE had problems updating TD22.  Please contact Systems Support.", MsgBoxStyle.Critical, "Error While Updating TD22")
            Return False
        End If
        Return True
    End Function

    'This function adds comments to TD37 and marks all loans
    Public Overrides Function AddCommentsToTD37(ByVal Comment As String, ByVal ARC As String) As Boolean
        AddCommentsToTD37 = True
        SP.Q.FastPath("TX3Z/ATD37" & SSN)
        If SP.Q.Check4Text(23, 2, "01019") Then Exit Function
        'look for ARC
        If FindingAQueueOnTD22(ARC) = False Then
            SP.frmWhoaDUDE.WhoaDUDE("The BIG KAHUNA hasn't given you access to the " & ARC & " ARC.", "No ARC Access", True)
            If ARC = "MXADD" Then
                Return False
            Else
                Exit Function
            End If
        End If
        'abort if DUDE is not on the screen to enter the activity record
        If Not SP.Q.Check4Text(1, 72, "TDX39") Then Exit Function
        'select the first loan
        SP.Q.PutText(11, 18, "X")
        'goto the bigger comment area screen
        SP.Q.Hit("Enter")
        SP.Q.Hit("F4")
        'enter comment
        SP.Q.PutText(8, 5, Comment, True)
        'check if comment took
        If SP.Q.Check4Text(23, 2, "02114 RECORD(S) SUCCESSFULLY ADDED") = False Then
            MsgBox("Maui DUDE had problems updating TD37.  Please contact Systems Support.", MsgBoxStyle.Critical, "Error While Updating TD37")
        End If
    End Function

    'This Function Searches for a Queue on TD22 if it finds it, the function selects it and returns true, else it returns false.
    Protected Overrides Function FindingAQueueOnTD22(ByVal ARC As String) As Boolean
        Dim row As Integer
        row = 8
        FindingAQueueOnTD22 = False
        While (Not SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
            If (SP.Q.Check4Text(row, 7, " " & ARC & " ")) Then
                SP.Q.PutText(row, 3, "01", True)
                FindingAQueueOnTD22 = True
                Exit Function
            Else
                If (SP.Q.Check4Text(row, 47, " " & ARC & " ")) Then
                    SP.Q.PutText(row, 43, "01", True)
                    FindingAQueueOnTD22 = True
                    Exit Function
                End If
            End If
            row = row + 1
            If (Not row < 23) Then
                SP.Q.Hit("F8")
                row = 8
            End If
        End While
    End Function
End Class