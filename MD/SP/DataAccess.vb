Imports System.Data.Linq
Imports System.Data.SqlClient

Public Class DataAccess

    Private Shared _dudeDataContext As DataContext
    Protected ReadOnly Property DudeDataContext() As DataContext
        Get
            If _dudeDataContext Is Nothing Then
                _dudeDataContext = New DataContext(UsrInf.Conn.ConnectionString)
            End If
            Return _dudeDataContext
        End Get
    End Property

    ''' <summary>
    ''' Inserts Call Categorization record into the DB
    ''' </summary>
    ''' <param name="entry">A populated call categorization entry object.</param>
    ''' <remarks></remarks>
    Public Shared Sub AddCallCategorizationRecord(ByVal entry As CallCategorizationEntry)
        'update DB with call categorization data
        Dim Comm As New SqlCommand(String.Format("EXEC spCallCategorizationInsert '{0}','{1}','{2}','{3}','{4}'", entry.Category, entry.Reason, entry.LetterID, entry.Comments.Replace("'", "''"), entry.UserID), SP.UsrInf.Conn)
        SP.UsrInf.Conn.Open()
        Comm.ExecuteNonQuery()
        SP.UsrInf.Conn.Close()
    End Sub
End Class
