Imports System.Windows.Forms

<CLSCompliant(True)> _
Public MustInherit Class MDBorrowerLite


    Private _sSN As String
    ''' <summary>
    ''' Borrower's SSN.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SSN() As String
        Get
            Return _sSN
        End Get
        Set(ByVal value As String)
            _sSN = value
        End Set
    End Property

    Private _noACPBSVCall As Boolean
    Public Property NoACPBSVCall() As Boolean
        Get
            Return _noACPBSVCall
        End Get
        Set(ByVal value As Boolean)
            _noACPBSVCall = value
        End Set
    End Property

    Private _queue As String
    ''' <summary>
    ''' Queue the task is being taken from, if it is coming from a queue.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Queue() As String
        Get
            Return _queue
        End Get
        Set(ByVal value As String)
            _queue = value
        End Set
    End Property

    Private _subQueue As String
    ''' <summary>
    ''' Sub queue the task is being taken from, if it is coming from a queue.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SubQueue() As String
        Get
            Return _subQueue
        End Get
        Set(ByVal value As String)
            _subQueue = value
        End Set
    End Property

    Private _aCPSelection As String
    ''' <summary>
    ''' The ACP (TC00) selection. (???)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ACPSelection() As String
        Get
            Return _aCPSelection
        End Get
        Set(ByVal value As String)
            _aCPSelection = value
        End Set
    End Property

    Private _queueCommentText As String
    ''' <summary>
    ''' Text from the queue task, if it is coming from a queue task.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property QueueCommentText() As String
        Get
            Return _queueCommentText
        End Get
        Set(ByVal value As String)
            _queueCommentText = value
        End Set
    End Property


    Public MustOverride Function CheckStartingFromACP(ByVal txtAccountID As System.Windows.Forms.TextBox) As Boolean

    Public MustOverride Sub CheckStartingFromACP()

    Public MustOverride Function ConvertAccToSSN(ByVal SSNOrAcctNum As String) As Boolean

End Class
