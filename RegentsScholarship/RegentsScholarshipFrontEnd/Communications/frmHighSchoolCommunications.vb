Imports RegentsScholarshipBackEnd

Public Class frmHighSchoolCommunications
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

    Private Sub frmHighSchoolCommunications_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HighSchoolCommunicationsHeader1.LoadComboBoxDataSources(True)
    End Sub
End Class