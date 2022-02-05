Public Class Verification
#Region "Properties"
    Private _timeStamp As Nullable(Of DateTime) = Nothing
    Public Property TimeStamp() As Nullable(Of DateTime)
        Get
            Return _timeStamp
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            If (value.HasValue AndAlso value.Value = Constants.MINIMUM_SQL_SERVER_DATE) Then
                _timeStamp = Nothing
            Else
                _timeStamp = value
            End If
        End Set
    End Property

    Private _userId As String = ""
    Public Property UserId() As String
        Get
            Return _userId
        End Get
        Set(ByVal value As String)
            _userId = value
        End Set
    End Property
#End Region 'Properties
End Class
