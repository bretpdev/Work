Module General
    Public TestMode As Boolean = True
    Public TestSQLConnStr As String = "workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""BART\BART"";persist security info=False;initial catalog=MasterBatch"
    Public LiveSQLConnStr As String = "workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""NOCHOUSE"";persist security info=False;initial catalog=MasterBatch"
    Public TestBSYSSQLConnStr As String = "workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""BART\BART"";persist security info=False;initial catalog=BSYS"
    Public LiveBSYSSQLConnStr As String = "workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""NOCHOUSE"";persist security info=False;initial catalog=BSYS"

    Public Function GetMasterBatchData(ByVal SelStr As String) As DataSet
        Dim DA As SqlClient.SqlDataAdapter
        Dim MBConn As New SqlClient.SqlConnection
        Dim MBSel As New SqlClient.SqlCommand
        Dim MBDS As DataSet
        MBDS = New DataSet
        DA = New SqlClient.SqlDataAdapter
        MBSel.CommandText = SelStr
        MBSel.Connection = MBConn
        If TestMode Then
            MBConn.ConnectionString = TestSQLConnStr
        Else
            MBConn.ConnectionString = LiveSQLConnStr
        End If
        MBSel.CommandType = CommandType.Text
        DA.SelectCommand = MBSel
        Try
            MBConn.Open()
            DA.Fill(MBDS)
            MBDS.Tables(0).TableName = "MBS"
            Return MBDS
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            MBConn.Close()
        End Try
    End Function

    Public Function GetSacker(ByVal SelStr As String) As DataSet
        Dim DA As SqlClient.SqlDataAdapter
        Dim SackerConn As SqlClient.SqlConnection
        Dim SackerSel As SqlClient.SqlCommand
        Dim SackerDS As DataSet
        SackerDS = New DataSet
        SackerConn = New SqlClient.SqlConnection
        DA = New SqlClient.SqlDataAdapter
        SackerSel = New SqlClient.SqlCommand
        SackerSel.CommandText = SelStr
        SackerSel.Connection = SackerConn
        If TestMode Then
            SackerConn.ConnectionString = TestBSYSSQLConnStr
        Else
            SackerConn.ConnectionString = LiveBSYSSQLConnStr
        End If
        SackerSel.CommandType = CommandType.Text
        DA.SelectCommand = SackerSel
        Try
            SackerConn.Open()
            DA.Fill(SackerDS)
            SackerDS.Tables(0).TableName = "ScriptSchedule"
            Return SackerDS
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            SackerConn.Close()
        End Try
    End Function
End Module
