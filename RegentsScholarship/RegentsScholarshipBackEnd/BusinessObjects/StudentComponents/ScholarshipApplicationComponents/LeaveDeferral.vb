Public Class LeaveDeferral
#Region "Properties"
    Private _beginDate As Nullable(Of DateTime)
    Public Property BeginDate() As Nullable(Of DateTime)
        Get
            Return _beginDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            If (value.HasValue AndAlso value.Value = Constants.MINIMUM_SQL_SERVER_DATE) Then
                _beginDate = Nothing
            Else
                _beginDate = value
            End If
        End Set
    End Property

    Private _endDate As Nullable(Of DateTime)
    Public Property EndDate() As Nullable(Of DateTime)
        Get
            Return _endDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            If (value.HasValue AndAlso value.Value = Constants.MINIMUM_SQL_SERVER_DATE) Then
                _endDate = Nothing
            Else
                _endDate = value
            End If
        End Set
    End Property

    Private _leaveOrDeferral As String
    Public Function LeaveOrDeferral() As String
        Return _leaveOrDeferral
    End Function

    Private _parentApplication As ScholarshipApplication
    Public Function ParentApplication() As ScholarshipApplication
        Return _parentApplication
    End Function

    Private _reason As String
    Public Property Reason() As String
        Get
            Return _reason
        End Get
        Set(ByVal value As String)
            _reason = value
        End Set
    End Property
#End Region 'Properties

    Public Sub New()
        'Empty constructor provided so that this can be used as a projection class.
        'All initialization will be done in the Load() method.
    End Sub

    Public Sub New(ByVal leaveOrDeferral As String, ByVal parentApplication As ScholarshipApplication)
        _beginDate = Nothing
        _endDate = Nothing
        _leaveOrDeferral = leaveOrDeferral
        _parentApplication = parentApplication
        _reason = ""
    End Sub

    Public Sub Commit()
        DataAccess.SetLeaveOrDeferral(Me)
    End Sub

    Public Shared Function Load(ByVal leaveOrDeferral As String, ByVal parentApplication As ScholarshipApplication) As LeaveDeferral
        Dim storedLeaveOrDeferral As LeaveDeferral = DataAccess.GetLeaveOrDeferral(leaveOrDeferral, parentApplication)
        If (storedLeaveOrDeferral IsNot Nothing) Then
            storedLeaveOrDeferral._leaveOrDeferral = leaveOrDeferral
            storedLeaveOrDeferral._parentApplication = parentApplication
        End If
        Return storedLeaveOrDeferral
    End Function

    Public Sub Validate()
        If (Not Lookups.LeaveDeferralReasons.Contains(_reason)) Then
            Dim message As String = String.Format("The deferment reason {0} is not acceptable.", _reason)
            Throw New RegentsInvalidDataException(message)
        End If
    End Sub
End Class
