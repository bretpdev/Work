Imports System.Data.SqlClient
Public Class demo
    Public Addr1 As String
    Public Addr2 As String
    Public City As String
    Public ST As String
    Public Zip As String
    Public AddValid As Boolean
    Public hPhone As String
    Public hPhoneValid As Boolean
    Public aPhone As String
    Public aPhoneValid As Boolean
    Public Email As String
    Public SSN As String
    Public Gender As String
    Public Ethnicity As String
    Public TestMode As Boolean
    Private Conn As SqlConnection
    Private Comm As SqlCommand
    Private Reader As SqlDataReader

    Public Sub New(ByVal tSSN As String, ByVal tTestmode As Boolean)
        SSN = tSSN
        TestMode = tTestmode
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
    End Sub

    'Loads data from DB
    Public Sub loadDemo()
        Comm.Connection.Open()
        Comm.CommandText = "SELECT * FROM BorrowerDat WHERE SSN = '" + SSN + "' "
        Reader = Comm.ExecuteReader
        'cycle through all records and load into objects
        While Reader.Read
            'required fields
            Addr1 = Reader("Add1").ToString()
            City = Reader("City").ToString()
            ST = Reader("State").ToString()
            Zip = Reader("Zip").ToString()
            If Reader("AddressValidity") Is System.DBNull.Value Then
                AddValid = False
            Else
                AddValid = Reader("AddressValidity")
            End If

            'optional fields
            Addr2 = Reader("Add2").ToString()

            If Reader("HomePhone") Is System.DBNull.Value Then
                hPhone = ""
            ElseIf Reader("HomePhone").ToString.IndexOf(" ") <> -1 Then
                hPhone = ""
            Else
                hPhone = Reader("HomePhone").ToString()
            End If

            If Reader("AltPhone") Is System.DBNull.Value Then
                aPhone = ""
            ElseIf Reader("AltPhone").ToString.IndexOf(" ") <> -1 Then
                aPhone = ""
            Else
                aPhone = Reader("AltPhone").ToString()
            End If

            If Reader("HomePhoneValidity") Is System.DBNull.Value Then
                hPhoneValid = False
            Else
                hPhoneValid = Reader("HomePhonevalidity")
            End If

            If Reader("AltPhoneValidity") Is System.DBNull.Value Then
                aPhoneValid = False
            Else
                aPhoneValid = CType(Reader("AltPhoneValidity"), Boolean)
            End If

            Email = Reader("Email").ToString()
            Gender = Reader("Gender").ToString()
            Ethnicity = Reader("Ethnicity").ToString()

        End While
        Reader.Close()
        Comm.Connection.Close()
    End Sub
End Class
