Public Class AltAddress


    Private _demos As Demographics

    Public Sub New()
        MyBase.New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub Databind(ByVal tDemos As Demographics)
        _demos = tDemos
        DemographicsBindingSource.DataSource = _demos
    End Sub

End Class
