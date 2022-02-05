Module DataAccess
    Public IsTestMode As Boolean = True
    Public SplashForm As New Splash
    Public dbConnection As SqlClient.SqlConnection
    Public DocFolder As String
    Public DocPath As String
    Public CoverDocPath As String
    Public ProcessedRecordsCount As Integer = 0
    Private sqlCmd As New SqlClient.SqlCommand
    Private sqlRdr As SqlClient.SqlDataReader

    'set up connection to db and folders
    Public Function SetUpConnections() As Boolean
        If IsTestMode Then
            'test
            DocFolder = "X:\PADD\NCSP\Test\"
            DocPath = "X:\PADD\NCSP\Test\Accounts\"
            CoverDocPath = "X:\PADD\General\"
            dbConnection = New SqlClient.SqlConnection("workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""BART\BART"";persist security info=False;initial catalog=NCSP")
        Else
            'live
            DocFolder = "X:\PADD\NCSP\"
            DocPath = "Q:\New Century\New System\Current Accounts\"
            CoverDocPath = "X:\PADD\General\Test"
            dbConnection = New SqlClient.SqlConnection("workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""NOCHOUSE"";persist security info=False;initial catalog=NCSP")
        End If
    End Function

    'get text to put in the letter
    Public Function GetLetterText(ByVal eligEndReason As String) As String
        Dim letterText As String

        sqlCmd.Connection = dbConnection
        dbConnection.Open()
        sqlCmd.CommandText = "SELECT LetterText FROM EligEnd WHERE EligEndReason = '" & eligEndReason & "'"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        letterText = sqlRdr("LetterText")
        dbConnection.Close()

        Return letterText
    End Function

    'get data set of records to process
    Public Function GetDataSet(ByVal queryText As String) As DataSet
        sqlCmd.CommandText = queryText

        Dim DS As New DataSet
        Dim DA As New SqlClient.SqlDataAdapter(sqlCmd)

        sqlCmd.Connection = dbConnection
        dbConnection.Open()
        DA.Fill(DS)
        dbConnection.Close()

        Return DS
    End Function

    'get text to put in balance owed merge field
    Public Function GetBalanceOwedText(ByVal balance As String) As String
        Dim textFromTable As String

        sqlCmd.Connection = dbConnection
        dbConnection.Open()
        sqlCmd.CommandText = "SELECT BalanceOwedText FROM BalanceOwedText"
        sqlRdr = sqlCmd.ExecuteReader
        sqlRdr.Read()
        textFromTable = sqlRdr("BalanceOwedText")
        dbConnection.Close()

        balance = (CDbl(balance * -1)).ToString("C")

        Return Replace(textFromTable, "[[[Balance]]]", balance)

    End Function
End Module
