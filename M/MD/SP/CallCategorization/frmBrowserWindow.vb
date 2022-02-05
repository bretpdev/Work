Public Class frmBrowserWindow

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="callCat"></param>
    ''' <param name="URL"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal callCat As frmGenericCallCategorization.CallCategory, ByVal URL As String)
        MyBase.New(callCat)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'browse to URL
        Browser.Navigate(URL)
    End Sub

    ''' <summary>
    ''' Default constructor.  Do not use.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    
End Class