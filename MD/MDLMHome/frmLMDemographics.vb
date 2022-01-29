Public Class frmLMDemographics

    Public Sub New(ByVal tBor As SP.Borrower, ByVal RM As Resources.ResourceManager)
        MyBase.New(tBor, RM)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub LoadActivityAndContactType(ByVal AT As String, ByVal CT As String)
        Me.txtActivityCode.Text = AT
        Me.txtContactCode.Text = CT
    End Sub

    Public Sub GetActivityAndContactType(ByRef AT As String, ByRef CT As String)
        AT = Me.txtActivityCode.Text
        CT = Me.txtContactCode.Text
    End Sub

End Class
