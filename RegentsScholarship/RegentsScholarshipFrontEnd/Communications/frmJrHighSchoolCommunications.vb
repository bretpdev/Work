Imports RegentsScholarshipBackEnd

Public Class frmJrHighSchoolCommunications
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
        MyBase.New(user, Constants.CommunicationEntityType.SCHOOL)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub frmJrHighSchoolCommunications_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SchoolCommunicationsHeader1.LoadComboBoxDataSources(False)
    End Sub
End Class