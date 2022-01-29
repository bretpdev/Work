Imports RegentsScholarshipBackEnd

Public Class AuthorizedThirdPartyControl

    Private _authorizedThirdPartyData As AuthorizedThirdParty
    Public Property AuthorizedThirdPartyData() As AuthorizedThirdParty
        Get
            Return _authorizedThirdPartyData
        End Get
        Set(ByVal value As AuthorizedThirdParty)
            _authorizedThirdPartyData = value
        End Set
    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cmbDemographicsState.DataSource = Lookups.States.Select(Function(p) p.Abbreviation).ToList()
    End Sub
End Class
