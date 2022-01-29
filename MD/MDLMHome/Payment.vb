Public Class Payment
    Inherits System.Windows.Forms.ListViewItem

    Private _amountApplied As String
    Public Property AmountApplied() As String
        Get
            Return _amountApplied
        End Get
        Set(ByVal value As String)
            _amountApplied = value
        End Set
    End Property

    Private _effectiveDt As String
    Public Property EffectiveDt() As String
        Get
            Return _effectiveDt
        End Get
        Set(ByVal value As String)
            _effectiveDt = value
        End Set
    End Property

    Private _transTyp As String
    Public Property TransTyp() As String
        Get
            Return _transTyp
        End Get
        Set(ByVal value As String)
            _transTyp = value
        End Set
    End Property

    Private _reversalType As String
    Public Property ReversalType() As String
        Get
            Return _reversalType
        End Get
        Set(ByVal value As String)
            _reversalType = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal tAmountApplied As String, ByVal tEffectiveDt As String, ByVal tTransTyp As String, ByVal tReversalType As String)
        _amountApplied = tAmountApplied
        _effectiveDt = tEffectiveDt
        _transTyp = tTransTyp
        _reversalType = tReversalType
        Me.Text = _amountApplied
        Me.SubItems.Add(_effectiveDt)
        Me.SubItems.Add(_transTyp)
        Me.SubItems.Add(_reversalType)
    End Sub





End Class
