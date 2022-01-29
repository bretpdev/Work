<Serializable()> _
Public Class ScriptItem
    Inherits ListViewItem
    Public Script As String
    Public dMon As Boolean
    Public dTue As Boolean
    Public dWed As Boolean
    Public dThu As Boolean
    Public dFri As Boolean
    Public mJan As Boolean
    Public mFeb As Boolean
    Public mMar As Boolean
    Public mApr As Boolean
    Public mMay As Boolean
    Public mJun As Boolean
    Public mJul As Boolean
    Public mAug As Boolean
    Public mSep As Boolean
    Public mOct As Boolean
    Public mNov As Boolean
    Public mDec As Boolean
    Public mOn As String
    Public OnDemandDate As Date
    Public Other As String
    Public Hold As Boolean
    Public Holder As String
    Public Duplex As Boolean
    Public Perforated As Boolean
    Public Manual As Boolean
    Public DBUserID As String
    Public ModAndSub As String

    Public LogFile As String

    Public Status As String
    Public BatchNumber As String

    Public TestMode As Boolean


    Public Sub SetData(ByVal TScript As String, ByVal tdMon As Boolean, ByVal tdTue As Boolean, ByVal tdWed As Boolean, ByVal tdThu As Boolean, ByVal tdFri As Boolean, ByVal tmJan As Boolean, ByVal tmFeb As Boolean, ByVal tmMar As Boolean, ByVal tmApr As Boolean, ByVal tmMay As Boolean, ByVal tmJun As Boolean, ByVal tmJul As Boolean, ByVal tmAug As Boolean, ByVal tmSep As Boolean, ByVal tmOct As Boolean, ByVal tmNov As Boolean, ByVal tmDec As Boolean, ByVal tmOn As String, ByVal tOnDemandDate As Date, ByVal tOther As String, ByVal tHold As Boolean, ByVal tHolder As String, ByVal tDuplex As Boolean, ByVal tPerforated As Boolean, ByVal tManual As Boolean, ByVal tUserID As String, ByVal tLogFile As String, ByVal tModAndSub As String, ByVal TTestMode As Boolean, ByVal tStatus As String)
        Script = TScript
        dMon = tdMon
        dTue = tdTue
        dWed = tdWed
        dThu = tdThu
        dFri = tdFri
        mJan = tmJan
        mFeb = tmFeb
        mMar = tmMar
        mApr = tmApr
        mMay = tmMay
        mJun = tmJun
        mJul = tmJul
        mAug = tmAug
        mSep = tmSep
        mOct = tmOct
        mNov = tmNov
        mDec = tmDec
        mOn = tmOn
        OnDemandDate = tOnDemandDate
        Other = tOther
        Hold = tHold
        Holder = tHolder
        Duplex = tDuplex
        Perforated = tPerforated
        Manual = tManual
        DBUserID = tUserID
        ModAndSub = tModAndSub
        TestMode = TTestMode
        Status = tStatus
        If tLogFile.StartsWith("X:\") = False Then
            If TestMode Then
                LogFile = "X:\PADD\Logs\Test\MBS" & tLogFile & ".txt"
            Else
                LogFile = "X:\PADD\Logs\MBS" & tLogFile & ".txt"
            End If
        Else
            LogFile = tLogFile
        End If
    End Sub

    Private Sub UpdateData(ByVal cmd As String)
        Dim CON As New SqlClient.SqlConnection
        If TestMode Then
            CON.ConnectionString = TestSQLConnStr
        Else
            CON.ConnectionString = LiveSQLConnStr
        End If
        Dim DA As New SqlClient.SqlDataAdapter
        Dim IC As New SqlClient.SqlCommand
        IC.CommandType = CommandType.Text
        IC.CommandText = cmd
        IC.Connection = CON
        DA.InsertCommand = IC
        Try
            CON.Open()
            DA.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            CON.Close()
        End Try
    End Sub

    Function GetBatch() As Integer
        If Me.ListView.Name = "lstB1" Then
            Return 1
        ElseIf Me.ListView.Name = "lstB2" Then
            Return 2
        ElseIf Me.ListView.Name = "lstB3" Then
            Return 3
        ElseIf Me.ListView.Name = "lstB4" Then
            Return 4
        ElseIf Me.ListView.Name = "lstB5" Then
            Return 5
        Else
            Return 0
        End If
    End Function

    Public Sub SetPending()
        'when placed in lstSel list view set status to pending
        Dim SubI As New ListViewItem.ListViewSubItem
        Dim SubI2 As New ListViewItem.ListViewSubItem
        Me.UseItemStyleForSubItems = False
        SubI.BackColor = Color.Black
        SubI2.BackColor = Color.Black
        SubI.Font = Me.Font
        SubI2.Font = Me.Font
        Me.SubItems.Clear()
        If Status = "Debug" Then
            SubI.ForeColor = Color.Red
        Else
            Status = "Pending"
            SubI.ForeColor = Color.Lime
        End If
        SubI.Text = Status
        Me.Text = Script
        If Hold Then
            SubI2.Text = "Hold for " & Holder
            SubI2.ForeColor = Color.Orange
        ElseIf Manual Then
            SubI2.Text = "Manual Run"
            SubI2.ForeColor = Color.Yellow
        Else
            SubI2.Text = Other
            SubI2.ForeColor = Color.Lime
        End If
        '---------------------------------
        Me.SubItems.Add(SubI)
        Me.SubItems.Add(SubI2)
        UpdateData("INSERT INTO ScriptHistory( Script,  Status, PC, UserID, Batch) VALUES ('" & Script & "', '" & Status & "', '" & System.Environment.MachineName() & "', '" & DBUserID & "', " & GetBatch() & "); ")
    End Sub

    Public Sub SetScheduled()
        'when placed in batch list view set status to ready
        Dim SubI As New ListViewItem.ListViewSubItem
        Me.UseItemStyleForSubItems = True
        Me.SubItems.Clear()
        Status = "Scheduled"
        Me.Text = Script
        SubI.Text = Status
        Me.SubItems.Add(SubI)
        UpdateData("INSERT INTO ScriptHistory( Script,  Status, PC, UserID, Batch) VALUES ('" & Script & "', '" & Status & "', '" & System.Environment.MachineName() & "', '" & DBUserID & "', " & GetBatch() & "); ")
    End Sub

    Public Sub SetReady()
        'when the run button is pressed set status to ready
        Dim SubI As New ListViewItem.ListViewSubItem
        Me.UseItemStyleForSubItems = True
        Me.SubItems.Clear()
        Status = "Ready"
        Me.Text = Script
        SubI.Text = Status
        Me.SubItems.Add(SubI)
        UpdateData("INSERT INTO ScriptHistory( Script,  Status, PC, UserID, Batch) VALUES ('" & Script & "', '" & Status & "', '" & System.Environment.MachineName() & "', '" & DBUserID & "', " & GetBatch() & "); ")
    End Sub

    Public Sub SetRunning()
        'when run set status to running
        Dim SubI As New ListViewItem.ListViewSubItem
        Me.UseItemStyleForSubItems = True
        Me.SubItems.Clear()
        Status = "Running"
        Me.Text = Script
        SubI.Text = Status
        Me.SubItems.Add(SubI)
        UpdateData("INSERT INTO ScriptHistory( Script,  Status, PC, UserID, Batch) VALUES ('" & Script & "', '" & Status & "', '" & System.Environment.MachineName() & "', '" & DBUserID & "', " & GetBatch() & "); ")
    End Sub

    Public Sub SetComplete()
        'when complete set status to complete
        Dim SubI As New ListViewItem.ListViewSubItem
        Me.UseItemStyleForSubItems = True
        Me.SubItems.Clear()
        Status = "Complete"
        Me.Text = Script
        SubI.Text = Status
        Me.SubItems.Add(SubI)
        UpdateData("INSERT INTO ScriptHistory( Script,  Status, PC, UserID, Batch) VALUES ('" & Script & "', '" & Status & "', '" & System.Environment.MachineName() & "', '" & DBUserID & "', " & GetBatch() & "); ")
    End Sub

    Public Sub SetDebug()
        'when placed in batch list view set status to ready
        Dim SubI As New ListViewItem.ListViewSubItem
        Dim SubI2 As New ListViewItem.ListViewSubItem
        Me.UseItemStyleForSubItems = False
        Me.SubItems.Clear()
        Status = "Debug"
        Me.Text = Script
        SubI.BackColor = Color.Black
        SubI2.BackColor = Color.Black
        SubI.Font = Me.Font
        SubI2.Font = Me.Font
        SubI.Text = Status
        SubI.ForeColor = Color.Red
        If Hold Then
            SubI2.Text = "Hold for " & Holder
            SubI2.ForeColor = Color.Orange
        ElseIf Manual Then
            SubI2.Text = "Manual Run"
            SubI2.ForeColor = Color.Yellow
        Else
            SubI2.Text = Other
            SubI2.ForeColor = Color.Lime
        End If

        Me.SubItems.Add(SubI)
        Me.SubItems.Add(SubI2)
        UpdateData("INSERT INTO ScriptHistory( Script,  Status, PC, UserID, Batch) VALUES ('" & Script & "', '" & Status & "', '" & System.Environment.MachineName() & "', '" & DBUserID & "', " & GetBatch() & "); ")
    End Sub

End Class
