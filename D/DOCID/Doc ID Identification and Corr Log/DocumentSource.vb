Public Class DocumentSource
    Private _code As Code

    Public Enum Code
        PO
        BU
        CF
        OT
    End Enum

    Public Sub New(ByVal sourceCode As Code)
        _code = sourceCode
    End Sub

    Public Overrides Function ToString() As String
        Select Case _code
            Case Code.BU
                Return "BU"
            Case Code.CF
                Return "CF"
            Case Code.OT
                Return "OT"
            Case Code.PO
                Return "PO"
            Case Else
                Debug.Assert(False, "Invalid document source.")
                Return String.Empty
        End Select
    End Function
End Class
