Public Class LetterRecordCreationResults

    '''' <summary>
    '''' Constructor
    '''' </summary>
    '''' <param name="tempNewRecordIdentity">The identity (primary key) from the newly created letter record.</param>
    '''' <param name="tempBarcodeSeqNum">The sequential barcode number.</param>
    '''' <remarks></remarks>
    'Public Sub New(ByVal tempNewRecordIdentity As Long, ByVal tempBarcodeSeqNum As Integer)
    '    _newRecordIdentity = tempNewRecordIdentity
    '    _barcodeSeqNum = tempBarcodeSeqNum
    'End Sub

    ''' <summary>
    ''' The identity (primary key) from the newly created letter record.
    ''' </summary>
    ''' <remarks></remarks>
    Private _newRecordIdentity As Long
    Public Property NewRecordIdentity() As Long
        Get
            Return _newRecordIdentity
        End Get
        Set(ByVal value As Long)
            _newRecordIdentity = value
        End Set
    End Property

    ''' <summary>
    ''' The sequential barcode number.
    ''' </summary>
    ''' <remarks></remarks>
    Private _barcodeSeqNum As Long
    Public Property BarcodeSeqNum() As Long
        Get
            Return _barcodeSeqNum
        End Get
        Set(ByVal value As Long)
            _barcodeSeqNum = value
        End Set
    End Property

End Class
