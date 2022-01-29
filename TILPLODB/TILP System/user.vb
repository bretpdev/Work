Imports System.Data.SqlClient

Public Class user
    Private UserID As String
    Private AccessLevel As Integer
    Private UserName As String

    Public Sub New(ByVal tUID As String, ByVal tAccessLevel As Integer, Optional ByVal GetBSYSUserName As Boolean = False)
        UserID = tUID
        AccessLevel = tAccessLevel
        If GetBSYSUserName Then
            'always get the user name from NOCHOUSE just in case test user isn't in test BSYS
            Dim UserIDMinusPossibleNum As String
            Dim Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=BSYS;Integrated Security=SSPI;")
            Dim Comm As SqlCommand
            'for testing ease numbers are sometime found at the end of the userids this will remove them 
            If IsNumeric(UserID.ToCharArray()(UserID.Length - 1)) Then
                UserIDMinusPossibleNum = UserID.Substring(0, UserID.Length - 1)
            Else
                UserIDMinusPossibleNum = UserID
            End If
            Comm = New SqlCommand("SELECT FirstName + ' ' + LastName FROM SYSA_LST_Users WHERE WindowsUserName = '" + UserIDMinusPossibleNum + "'", Conn)
            Conn.open()
            UserName = Comm.ExecuteScalar().ToString
            Conn.close()
        End If
    End Sub

    Public Function GetUID() As String
        Return UserID
    End Function

    Public Function GetAccessLevel() As Integer
        Return AccessLevel
    End Function

    Public Function GetUserName() As String
        Return UserName
    End Function
End Class
