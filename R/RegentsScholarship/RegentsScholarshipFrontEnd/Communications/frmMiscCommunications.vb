Imports RegentsScholarshipBackEnd

Public Class frmMiscCommunications
    ''' <summary>
    ''' Default Constructor (DO NOT USE)
    ''' </summary>
    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub New(ByVal user As User)
        MyBase.New(user, Constants.CommunicationEntityType.MISC)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub frmMiscCommunications_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.LoadComments()
        MyBase.EntityID = "Misc"
    End Sub
End Class