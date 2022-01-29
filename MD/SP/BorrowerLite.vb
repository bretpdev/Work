Imports System.Windows.Forms
Imports Q

Public Class BorrowerLite
    Inherits MDBorrowerLite

    Public Overrides Function CheckStartingFromACP(ByVal txtAccountID As System.Windows.Forms.TextBox) As Boolean
        'check if MauiDUDE is starting from ACP (TC00)
        If SP.Q.Check4Text(1, 72, "TCX05") Then 'ACP functionality
            CheckStartingFromACP = True 'Maui DUDE started from ACP
            txtAccountID.Text = SP.Q.GetText(1, 9, 9)
            SP.Hit("F12")
            If SP.Q.Check4Text(1, 72, "J0X02") Then 'user is using a queue or the dialer
                SP.Hit("Home")
                SP.Q.PutText(SP.Q.RIBM.CursorRow, SP.RIBM.CursorColumn, "ITX6X")
                SP.Hit("End")
                SP.Hit("Enter")
                If SP.Q.Check4Text(6, 37, "__") Then 'user is using the dialer
                    Queue = ""
                    SubQueue = ""
                    ACPSelection = "2"
                Else
                    Queue = SP.Q.GetText(6, 37, 2)
                    SubQueue = SP.Q.GetText(8, 37, 2)
                    ACPSelection = ""
                End If
            Else 'user went into straight into TC00 with out using a queue
                ACPSelection = SP.Q.GetText(19, 38, 1)
                Queue = ""
            End If
        Else
            CheckStartingFromACP = False 'Maui DUDE didn't start from ACP
        End If
    End Function

    Public Overrides Sub CheckStartingFromACP()
        'check if MauiDUDE is starting from ACP (TC00)
        If SP.Q.Check4Text(1, 72, "TCX05") Then 'ACP functionality
            SP.Hit("F12")
            If SP.Q.Check4Text(1, 72, "J0X02") Then 'user is using a queue or the dialer
                SP.Hit("Home")
                SP.Q.PutText(SP.Q.RIBM.CursorRow, SP.RIBM.CursorColumn, "ITX6X")
                SP.Hit("End")
                SP.Hit("Enter")
                If SP.Q.Check4Text(6, 37, "__") Then 'user is using the dialer
                    Queue = ""
                    SubQueue = ""
                    ACPSelection = "2"
                Else
                    Queue = SP.Q.GetText(6, 37, 2)
                    SubQueue = SP.Q.GetText(8, 37, 2)
                    ACPSelection = ""
                End If
            Else 'user went into straight into TC00 with out using a queue
                ACPSelection = SP.Q.GetText(19, 38, 1)
                Queue = ""
            End If
        End If
    End Sub

    Public Overrides Function ConvertAccToSSN(ByVal SSNOrAcctNum As String) As Boolean
        ConvertAccToSSN = True
        'get SSN from Account number
        If SSNOrAcctNum.Length = 10 Then
            SP.Q.FastPath("LP22I;;;;;;" & SSNOrAcctNum)
        Else
            SP.Q.FastPath("LP22I" & SSNOrAcctNum)
        End If
        'check if the user is logged into OneLINK and COMPASS
        If SP.Q.Check4Text(1, 2, "INVALID COMMAND SYNTAX") Or SP.Q.Check4Text(3, 11, "CICS0001: OPERATOR IS NOT LOGGED ON.") Then
            'GetExistingSession(SSN)
            If SP.Q.RIBM Is Nothing Then
                SP.frmWipeOut.WipeOut("That totally didn't work out.  How about you log in to OneLINK and COMPASS.", "Log In Problem", True)
                Return False
            End If
        End If
        'is the borrower located on OneLINK
        If SP.Q.Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") Then
            'check if the borrower is on COMPASS
            SP.FastPath("TX3ZITX1JB;")
            If SSNOrAcctNum.Length = 10 Then
                SP.PutText(6, 61, SSNOrAcctNum, True)
            Else
                SP.PutText(6, 16, SSNOrAcctNum, True)
            End If
            If SP.Check4Text(1, 72, "TXX1K") Then
                'if they aren't on Onelink or COMPASS then they aren't a valid Borrower
                SP.frmWhoaDUDE.WhoaDUDE("That was totally knarly, but DUDE needs a valid SSN or account number from OneLINK.", "Knarly DUDE", True)
                Return False
            Else
                'get SSN from COMPASS
                SSN = SP.Q.GetText(3, 12, 11).Replace(" ", "") 'SSN
            End If
        Else
            'get SSN from OneLINK
            SSN = SP.Q.GetText(3, 23, 9) 'SSN
        End If
    End Function

    Public Shared Sub CompleteOLTaskForBin(ByRef Queues As ArrayList)
        SP.FastPath("LP9AC")
        SP.Q.Hit("F6")
    End Sub

    'get bin task data and return borrower lite object
    Public Shared Function GetOLTaskForBin(ByRef Queues As ArrayList) As SP.BorrowerLite
        Dim BL As New BorrowerLite
        SP.FastPath("LP9AC" + Queues(0).ToString)
        'check if the queue had anything in it
        If SP.Check4Text(1, 66, "QUEUE SELECTION") Then
            'if nothing in it then remove queue from list and move on to the next queue
            Queues.RemoveAt(0)
            'check if there are additional queues to try and work
            If Queues.Count = 0 Then
                Return Nothing 'return nothing if the bin has been processed
            End If
            SP.FastPath("LP9AC" + Queues(0).ToString)
        End If
        If SP.Check4Text(3, 24, Queues(0).ToString) = False And SP.Check4Text(3, 18, Queues(0).ToString) = False Then
            'the user has a task open in a different queue give user error message and allow them to work that queue task
            SP.frmWhoaDUDE.WhoaDUDE("You already have a queue task open in another queue.  DUDE is going to help with that one first then we'll move on to the bin you selected.", "Task Already Found")
        End If
        If SP.Q.Check4Text(5, 66, "SSN") Then
            BL.SSN = SP.GetText(5, 70, 9)
        Else
            BL.SSN = SP.GetText(17, 70, 9)
        End If
        BL.QueueCommentText = (SP.GetText(12, 11, 59).Trim.Replace("_", "") + " " + SP.GetText(13, 11, 59).Trim.Replace("_", "") + " " + SP.GetText(14, 11, 59).Trim.Replace("_", "") + " " + SP.GetText(13, 11, 59).Trim.Replace("_", "")).Trim
        If SP.Check4Text(3, 7, "WORK GROUP") Then
            BL.Queue = SP.GetText(3, 18, 9)
        Else
            BL.Queue = SP.GetText(3, 24, 9)
        End If
        Return BL
    End Function

End Class
