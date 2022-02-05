Imports RegentsScholarshipBackEnd

Public Class SchoolOption

    Public Enum SchoolOptions
        All
        HighSchoolOnly
        JrHighSchoolOnly
    End Enum

    Public Sub New(ByVal id As String, ByVal name As String)
        _id = id
        _name = name
    End Sub

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

    Public Overrides Function ToString() As String
        Return _name
    End Function

    Public Shared Sub PopulateSchoolDropDown(ByVal cmb As ComboBox, Optional ByVal addBlankToTop As Boolean = True, Optional ByVal displayCeebCode As Boolean = False, Optional ByVal schoolsToInclude As SchoolOptions = SchoolOptions.All)
        Dim allSchoolOptions As New List(Of SchoolOption)
        Dim schools As List(Of Lookups.School)
        If addBlankToTop Then
            'add blank to top
            allSchoolOptions.Add(New SchoolOption("", ""))
        End If
        If schoolsToInclude = SchoolOptions.All Then
            schools = Lookups.Schools.Select(Function(p) p).OrderBy(Function(p) p.Name).ToList()
        ElseIf schoolsToInclude = SchoolOptions.HighSchoolOnly Then
            schools = Lookups.Schools.Where(Function(p) p.Type = "HIGH").Select(Function(p) p).OrderBy(Function(p) p.Name).ToList()
        Else 'Jr high
            schools = Lookups.Schools.Where(Function(p) p.Type <> "HIGH").Select(Function(p) p).OrderBy(Function(p) p.Name).ToList()
        End If
        For Each s As Lookups.School In schools
            allSchoolOptions.Add(New SchoolOption(s.CeebCode, s.Name))
        Next
        'data bind it
        cmb.DataSource = allSchoolOptions
        If displayCeebCode Then
            cmb.DisplayMember = "ID"
        Else
            cmb.DisplayMember = "Name"
        End If
        cmb.ValueMember = "ID"
    End Sub

End Class
