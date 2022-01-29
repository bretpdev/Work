<CLSCompliant(True)> _
Public MustInherit Class MDActivityComments

    Private _ssn As String
    ''' <summary>
    ''' SSN of borrower
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SSN() As String
        Get
            Return _ssn
        End Get
        Set(ByVal value As String)
            _ssn = value
        End Set
    End Property


    Public Sub New(ByVal tSSN As String)
        SSN = tSSN
    End Sub


    Public MustOverride Function AddCommentsToLP50AndTD22(ByVal CommentO As String, ByVal CommentC As String, ByVal ActionCode As String, ByVal ContactType As String, ByVal ActivityType As String, ByVal ARC As String) As Boolean

    Public MustOverride Function AddCommentsToLP50(ByVal Comment As String, ByVal ActionCode As String, ByVal ContactType As String, ByVal ActivityType As String) As Boolean

    Public MustOverride Function AddCommentsToTD22AllLoans(ByVal Comment As String, ByVal ARC As String) As Boolean

    Public MustOverride Function AddCommentsToTD37(ByVal Comment As String, ByVal ARC As String) As Boolean

    Protected MustOverride Function FindingAQueueOnTD22(ByVal ARC As String) As Boolean
End Class