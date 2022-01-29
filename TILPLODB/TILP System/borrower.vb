Imports System.Data.SqlClient

Public Class borrower
    'Public SSN As String
    Private SSN As String
    Public FN As String
    Public LN As String
    Public MI As String
    Public DOB As String
    Public PromDtSig As String
    Public AwardInst As String
    Public NewRecipLetterDt As String
    Public NewAcct As Boolean
    Public BorDemo As demo
    Public BorRefs As refs
    Public BorLoans As loans
    Public TestMode As Boolean

    Public Sub New(ByVal tTestMode As Boolean, ByVal tNewAcct As Boolean, ByVal tSSN As String)
        SSN = tSSN
        FN = ""
        LN = ""
        MI = ""
        DOB = ""
        PromDtSig = ""
        NewAcct = tNewAcct
        TestMode = tTestMode
        BorDemo = New demo(SSN, TestMode)
        BorRefs = New refs(SSN, TestMode)
        BorLoans = New loans(SSN, TestMode)
    End Sub

    'use to get next activity sequence number that should be used
    Public Shared Function NextActivitySeqNum(ByVal LTestMode As Boolean, ByVal LSSN As String) As Integer
        Dim Conn As SqlConnection
        Dim Comm As New SqlCommand
        Dim Result As Object
        'set up DB connection
        If LTestMode Then
            'if in test mode
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            'if in live mode
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        Comm.Connection = Conn
        Comm.CommandText = "SELECT MAX(ActivitySeq) FROM ActivityDat WHERE SSN = '" + LSSN + "'"
        Conn.Open() 'open connection
        Result = Comm.ExecuteScalar() 'get result
        Conn.Close() 'close connection
        If Result.GetType.ToString = "System.DBNull" Then
            Return 1
        Else
            Return (CInt(Result) + 1)
        End If
    End Function

    'Loads data from DB
    Public Sub borLoadDat()
        Dim Conn As SqlConnection
        Dim Comm As SqlCommand
        Dim Reader As SqlDataReader
        'set up DB connection
        If TestMode Then
            'if in test mode
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            'if in live mode
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        Comm = New SqlCommand
        Comm.Connection = Conn
        Comm.Connection.Open()
        Comm.CommandText = "SELECT FirstName, LastName, MiddleInit, DOB, PnoteSigDate, NewRecipLetterDt, AwardInst  FROM BorrowerDat WHERE SSN = '" + SSN + "' "
        Reader = Comm.ExecuteReader
        'cycle through all records and load into objects
        While Reader.Read
            FN = Reader("FirstName").ToString()
            LN = Reader("LastName").ToString()
            MI = Reader("MiddleInit").ToString()
            DOB = Format(CDate(Reader("DOB").ToString()), "MM/dd/yyyy")
            PromDtSig = Format(CDate(Reader("PnoteSigDate").ToString()), "MM/dd/yyyy")
            NewRecipLetterDt = Format(CDate(Reader("NewRecipLetterDt").ToString()), "MM/dd/yyyy")
            AwardInst = Reader("AwardInst").ToString()
        End While
        Reader.Close()
        Comm.Connection.Close()
    End Sub

    'Update the SSN in the Borrower object and related objects
    Public Sub changeKey(ByRef newSSN As String)
        SSN = newSSN
        BorDemo.SSN = SSN
        BorLoans.SSN = SSN
        BorRefs.SSN = SSN
    End Sub

    Public Function getKey() As String
        Return SSN
    End Function

    Public Sub LockAccount(ByVal UserID As String)
        Dim Conn As SqlConnection
        Dim Comm As New SqlCommand
        'set up DB connection
        If TestMode Then
            'if in test mode
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            'if in live mode
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        Conn.Open()
        Comm.Connection = Conn
        'unlock any records locked by the user ID
        Comm.CommandText = "DELETE FROM LockedAccounts WHERE ByWho = '" + UserID + "'"
        Comm.ExecuteNonQuery()
        'create lock record for current account
        Comm.CommandText = "INSERT INTO LockedAccounts (LockedSSN, ByWho) VALUES ('" + SSN + "', '" + UserID + "')"
        Comm.ExecuteNonQuery()
        Conn.Close()
    End Sub

    Public Sub UnlockAccount()
        Dim Conn As SqlConnection
        Dim Comm As New SqlCommand
        'set up DB connection
        If TestMode Then
            'if in test mode
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            'if in live mode
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        Conn.Open()
        Comm.Connection = Conn
        'unlock account
        Comm.CommandText = "DELETE FROM LockedAccounts WHERE LockedSSN = '" + SSN + "'"
        Comm.ExecuteNonQuery()
        Conn.Close()
    End Sub

    'print letter
    Public Shared Sub PrintDoc(ByVal Doc As String, ByVal Folder As String, ByVal inTest As Boolean, Optional ByVal Dat As String = "T:\TILPdat.txt")

        If inTest = True Then Folder = Folder & "Test\"

        Doc = Folder & Doc & ".doc"

        'create the document
        Dim Word As New Microsoft.Office.Interop.Word.Application
        Word.Visible = False
        With Word
            'open merge document
            Word.Documents.Open(FileName:=Doc, ConfirmConversions:=False, _
                ReadOnly:=True, AddToRecentFiles:=False, PasswordDocument:="", _
                PasswordTemplate:="", Revert:=False, WritePasswordDocument:="", _
                WritePasswordTemplate:="")
            'set data file
            Word.ActiveDocument.MailMerge.OpenDataSource(Name:=Dat, _
                ConfirmConversions:=False, ReadOnly:= _
                False, LinkToSource:=True, AddToRecentFiles:=False, PasswordDocument:="", _
                PasswordTemplate:="", WritePasswordDocument:="", WritePasswordTemplate:= _
                "", Revert:=False, Connection:="", SQLStatement _
                :="", SQLStatement1:="")
            'perform merge
            With .ActiveDocument.MailMerge
                .Destination = .Destination.wdSendToNewDocument
                .SuppressBlankLines = True
                .Execute(Pause:=False)
            End With
            'close form file
            Word.Documents(Doc).Close(False)
            'print the file
            Word.ActiveDocument.PrintOut(Background:=False)
            Word.Application.Quit(False)
            MsgBox("Please retrieve your document from the printer.", MsgBoxStyle.Information, "Document Printed")
        End With
    End Sub

End Class
