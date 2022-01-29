Imports System.Data.SqlClient

Public Class refs

    Public allRefs As ArrayList
    Public SSN As String
    Public TestMode As Boolean
    Private Conn As SqlConnection
    Private Comm As SqlCommand
    Private Reader As SqlDataReader

    Public Sub New(ByVal tSSN As String, ByVal tTestmode As Boolean)

        allRefs = New ArrayList
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

    'loads data into array list for form
    'Public Sub GetRefData(ByRef BorRefs As ArrayList)
    '    Dim I As Integer
    '    Dim R As ref
    '    BorRefs = New ArrayList
    '    While allRefs.Count > I
    '        R = New ref
    '        R.ID = CType(allRefs(I), ref).ID
    '        R.FN = CType(allRefs(I), ref).FN
    '        R.LN = CType(allRefs(I), ref).LN
    '        R.MI = CType(allRefs(I), ref).MI
    '        R.Addr1 = CType(allRefs(I), ref).Addr1
    '        R.Addr2 = CType(allRefs(I), ref).Addr2
    '        R.City = CType(allRefs(I), ref).City
    '        R.ST = CType(allRefs(I), ref).ST
    '        R.Zip = CType(allRefs(I), ref).Zip
    '        R.AddValid = CType(allRefs(I), ref).AddValid
    '        R.hPhone = CType(allRefs(I), ref).hPhone
    '        R.hPhoneValid = CType(allRefs(I), ref).hPhoneValid
    '        BorRefs.Add(R)
    '        I = I + 1
    '    End While
    'End Sub

    'loads data from DB for existing refs
    Public Sub DoDBLoad()
        Dim R As ref

        Comm.Connection.Open()
        Comm.CommandText = "SELECT A.* FROM ReferenceDat A WHERE A.SSN = '" & SSN & "' ORDER BY A.RefID"
        Reader = Comm.ExecuteReader

        'cycle through all records and load into objects
        While Reader.Read
            R = New ref
            R.ID = CInt(Reader("RefID"))
            R.FN = Reader("RefFirstName").ToString()
            R.LN = Reader("RefLastName").ToString()
            R.Addr1 = Reader("RefAdd1").ToString()
            R.City = Reader("RefCity").ToString()
            R.ST = Reader("RefState").ToString()
            R.Zip = Reader("RefZip").ToString()
            R.hPhone = Reader("RefHomePhone").ToString()
            R.AddValid = CType(Reader("RefAddValidity"), Boolean)
            R.hPhoneValid = CType(Reader("RefHomePhoneValidity"), Boolean)
            'check possible null values
            If Reader("RefMiddleInit") Is System.DBNull.Value Then
                R.MI = ""
            Else
                R.MI = Reader("RefMiddleInit").ToString()
            End If
            If Reader("RefAdd2") Is System.DBNull.Value Then
                R.Addr2 = ""
            Else
                R.Addr2 = Reader("RefAdd2").ToString()
            End If
            allRefs.Add(R) 'add object to array list
        End While
        Reader.Close()
        Comm.Connection.Close()
    End Sub

End Class

Public Class ref
    Public ID As Integer
    Public FN As String
    Public LN As String
    Public MI As String
    Public Addr1 As String
    Public Addr2 As String
    Public City As String
    Public ST As String
    Public Zip As String
    Public AddValid As Boolean
    Public hPhone As String
    Public hPhoneValid As Boolean

    Public Sub New()
        ID = 0
        FN = ""
        LN = ""
        MI = ""
        Addr1 = ""
        Addr2 = ""
        City = ""
        ST = ""
        Zip = ""
        AddValid = False
        hPhone = ""
        hPhoneValid = False
    End Sub

End Class
