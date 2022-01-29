Imports System.Data.SqlClient

Public Class UserID
    Inherits ListViewItem
    Public ID As String
    Public Pass As String
    Public Status As String
    Public BeingUsedByMBS As Boolean
    'Public TestMode As Boolean

    Public Sub SetID(ByVal tID As String)
        ID = tID
        Me.SubItems(0).Text = tID
    End Sub

    Public Sub SetValid()
        Me.Status = "Valid"
        Me.SubItems(1).Text = "Valid"
        Me.ForeColor = Color.Lime
    End Sub

    Public Sub SetInvalid()
        Status = "Invalid"
        Me.SubItems(1).Text = "Invalid"
        Me.ForeColor = Color.Red
    End Sub

    Public Sub SetInUse()
        Me.Status = "In Use"
        Me.SubItems(1).Text = "In Use"
        Me.ForeColor = Color.Red
    End Sub

    Public Sub SetNotVerified()
        Me.Status = "Not Verified"
        Me.SubItems.Add("Not Verified")
        Me.ForeColor = Color.Red
    End Sub

    Public Function DBUIDOkay() As Boolean
        Dim Comm As New SqlCommand
        Dim CON As New SqlClient.SqlConnection
        If TestMode Then
            CON.ConnectionString = TestSQLConnStr
        Else
            CON.ConnectionString = LiveSQLConnStr
        End If
        Comm.Connection = CON
        Comm.CommandText = "SELECT * FROM UserIDsInUse WHERE UserID = '" & ID & "' AND PCName <> '" & Environment.MachineName & "'"
        CON.Open()
        If Comm.ExecuteScalar = Nothing Then
            DBUIDOkay = True
        Else
            DBUIDOkay = False
        End If
        CON.Close()
    End Function

    Public Function AddUIDToDB() As Boolean
        Dim Comm As New SqlCommand
        Dim CON As New SqlClient.SqlConnection
        If TestMode Then
            CON.ConnectionString = TestSQLConnStr
        Else
            CON.ConnectionString = LiveSQLConnStr
        End If
        Comm.Connection = CON
        Comm.CommandText = "INSERT INTO UserIDsInUse (UserID, PCName, EntryDate) VALUES('" & ID & "', '" & Environment.MachineName & "', '" & Now & "')"
        Try
            CON.Open()
            Comm.ExecuteNonQuery()
        Catch ex As Exception
            'this is if the userid is already in use by this PC
        Finally
            CON.Close()
        End Try
    End Function

    Public Sub RemoveFromDB()
        Dim Comm As New SqlCommand
        Dim CON As New SqlClient.SqlConnection
        If TestMode Then
            CON.ConnectionString = TestSQLConnStr
        Else
            CON.ConnectionString = LiveSQLConnStr
        End If
        Comm.Connection = CON
        Comm.CommandText = "DELETE FROM UserIDsInUse WHERE PCName = '" & Environment.MachineName & "' AND UserID = '" & ID & "'"
        CON.Open()
        Comm.ExecuteNonQuery()
        CON.Close()
    End Sub


End Class
