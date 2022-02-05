Imports Q

Public Enum PWValidationResults
    Valid = 1
    NotValidInDB = 2
    NotValidSystemErr = 3
End Enum

Public Class PasswordCheck

    Private LV As ListView
    Private IPCs() As IndividualPWCheck

    'constructor
    Public Sub New(ByRef tLV As ListView)
        LV = tLV
    End Sub

    Public Function PerformPWCheck() As Boolean
        Dim I As Integer
        Dim TS As New TimeSpan(0, 0, 1)
        Dim StillAlive As Boolean = True
        Dim DBErrorResult As Boolean = False
        Dim SystemErrorResult As Boolean = False
        Dim AtLeastOneOK As Boolean = False
        Dim Processing As New frmValidatingUIDs
        'create thread objects
        Processing.Visible = True
        'Processing.Refresh()
        ReDim IPCs(LV.Items.Count) 'init array
        While I < LV.Items.Count
            IPCs(I) = New IndividualPWCheck(CType(LV.Items(I), UserID))
            I = I + 1
        End While
        Threading.Thread.Sleep(TS) 'sleep for 1 seconds
        'check for finished threads
        While StillAlive
            I = 0
            StillAlive = False
            While I < LV.Items.Count
                'check if thread sync lock is of object
                If Threading.Monitor.TryEnter(IPCs(I), 1000) = False Then
                    StillAlive = True
                Else
                    Threading.Monitor.Exit(IPCs(I)) 'unlock if successfully locked
                End If
                I = I + 1
            End While
        End While
        'check if all UIDs and Passwds were valid
        I = 0
        While I < LV.Items.Count
            'check if thread sync lock is of object
            If IPCs(I).UIDAndPassValid = PWValidationResults.NotValidInDB Then
                DBErrorResult = True
            ElseIf IPCs(I).UIDAndPassValid = PWValidationResults.NotValidSystemErr Then
                SystemErrorResult = True
            ElseIf IPCs(I).UIDAndPassValid = PWValidationResults.Valid Then
                AtLeastOneOK = True
            End If
            I = I + 1
        End While

        'display user error message based off results returned from threads
        If DBErrorResult And SystemErrorResult Then
            MessageBox.Show("One or more of the user IDs entered is already being used by another PC.  Please coordinate the release of the user ID throught other PCs running Master Batch." & vbCrLf & vbCrLf & _
                            "and" & vbCrLf & vbCrLf & _
                            "One or more of the user IDs entered was not validated by the system." & vbCrLf & vbCrLf & _
                            "Please see user ID list for details.", "User ID and Password Errors", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf SystemErrorResult Then
            MessageBox.Show("One or more of the user IDs entered was not validated by the system." & vbCrLf & vbCrLf & _
                            "Please see user ID list for details.", "User ID and Password Errors", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf DBErrorResult Then
            MessageBox.Show("One or more of the user IDs entered is already being used by another PC.  Please coordinate the release of the user ID throught other PCs running Master Batch." & vbCrLf & vbCrLf & _
                            "Please see user ID list for details.", "User ID and Password Errors", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        'check if at least one user id was OK
        If AtLeastOneOK Then
            PerformPWCheck = True 'at least one user id was ok
        Else
            PerformPWCheck = False 'none of the user ids were ok
        End If
        Processing.Visible = False
    End Function

End Class

Public Class IndividualPWCheck

    Private UIDObj As UserID
    Private ProcT As Threading.Thread
    Private RefI As ReflectionInterface
    Public UIDAndPassValid As PWValidationResults

    Public Sub New(ByRef tUIDObj As UserID)
        UIDObj = tUIDObj
        ProcT = New Threading.Thread(AddressOf Proc)
        ProcT.IsBackground = True
        ProcT.Start()
    End Sub

    'main thread sub
    Private Sub Proc()
        Threading.Monitor.Enter(Me) 'lock object for sync

        'RefI = New ReflectionInterface(UIDObj.TestMode, False)
        RefI = New ReflectionInterface(False)
        If RefI.Login(UIDObj.ID, UIDObj.Pass) Then
            If UIDObj.DBUIDOkay() Then
                UIDObj.AddUIDToDB()
                UIDObj.SetValid()
                UIDAndPassValid = PWValidationResults.Valid
            Else
                UIDObj.SetInUse()
                UIDAndPassValid = PWValidationResults.NotValidInDB
            End If
        Else
            UIDObj.SetInvalid()
            UIDAndPassValid = PWValidationResults.NotValidSystemErr
        End If

        RefI.CloseSession()
        
        Threading.Monitor.Exit(Me) 'unlock object for sync
    End Sub

    'Function UserIDIsTaken(ByVal ID As String) As Boolean
    '    Dim DS As DataSet
    '    DS = GetMasterBatchData("Select UserID, PCName, EntryDate from UserIDsInUse where UserID = '" & ID & "'")
    '    If DS.Tables(0).Rows.Count > 0 Then
    '        MsgBox("The UserID " & ID & " is in use by PC " & DS.Tables(0).Rows(0).Item("PCName") & " since " & DS.Tables(0).Rows(0).Item("EntryDate") & ".")
    '        Return True
    '        Exit Function
    '    End If
    '    Return False
    'End Function



End Class
