Public Class frmGenericCallCategorization

    Enum CallCategory
        SingleMoms
        UtahFutures
        NoUHEAAConnection
    End Enum

    Private _entry As CallCategorizationEntry

    ''' <summary>
    ''' Default constructor. Do not use.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal callCat As CallCategory)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'based off what call category enum is passed in insert entry values
        If callCat = CallCategory.SingleMoms Then
            _entry = New CallCategorizationEntry() With {.Category = "Single Moms"}
        ElseIf callCat = CallCategory.UtahFutures Then
            _entry = New CallCategorizationEntry() With {.Category = "Utah Futures"}
        ElseIf callCat = CallCategory.NoUHEAAConnection Then
            _entry = New CallCategorizationEntry() With {.Category = "No UHEAA Connection"}
        End If
        CallCatCmt.CallCategorizationEntryBindingSource.DataSource = _entry
    End Sub


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        DataAccess.AddCallCategorizationRecord(_entry)
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class