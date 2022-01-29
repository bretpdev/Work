Imports RegentsScholarshipBackEnd

Public Class DistrictOption
    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _id As String
    Public Property ID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Public Sub New(ByVal id As String, ByVal name As String)
        _id = id
        _name = name
    End Sub

    Public Overrides Function ToString() As String
        Return _name
    End Function

    Public Shared Sub PopulateDistrictDropDown(ByVal cmb As ComboBox, ByVal addBlankToTop As Boolean)
        Dim districtOptions As New List(Of DistrictOption)
        Dim districts As List(Of RegentsScholarshipBackEnd.Lookups.District)
        If addBlankToTop Then
            'add blank to top
            districtOptions.Add(New DistrictOption("", ""))
        End If
        'look up all districts
        districts = Lookups.Districts.Select(Function(p) p).OrderBy(Function(p) p.Name).ToList()
        For Each d As RegentsScholarshipBackEnd.Lookups.District In districts
            districtOptions.Add(New DistrictOption(d.CommunicationDirectoryID, d.Name))
        Next
        'data bind it
        cmb.DataSource = districtOptions
        cmb.DisplayMember = "Name"
        cmb.ValueMember = "ID"
    End Sub

End Class
