Imports RegentsScholarshipBackEnd

Public Class frmDistrictCommunications
    ''' <summary>
    ''' Default Constructor (DO NOT USE)
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub New(ByVal user As User)
        MyBase.New(user, Constants.CommunicationEntityType.DISTRICT)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub frmDistrictCommunications_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DistrictCommunicationHeader1.LoadComboBoxDataSources()
    End Sub
End Class