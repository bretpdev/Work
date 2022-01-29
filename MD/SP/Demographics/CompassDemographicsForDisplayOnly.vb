Imports System.Windows.Forms

Public Class CompassDemographicsForDisplayOnly

    Private _otherPhone2 As String

    ''' <summary>
    ''' Default constructor.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Overrides Sub DataBind(ByVal tDemos As Demographics)
        MyBase.DataBind(tDemos)
        If Demos.OtherPhone2Num.Length > 0 Then _otherPhone2 = Demos.OtherPhone2Num.Insert(6, "-").Insert(3, "-")
        lblWork.Text = _otherPhone2
        lblCWorkInd.Text = tDemos.OtherPhone2ValidityIndicator
        lblCWorkDate.Text = tDemos.OtherPhone2VerificationDate
        HideOrShowDataLabels()
    End Sub

    'overridden here because the Compass side of things adds controls
    Public Overrides Sub HideOrShowDataLabels()
        Dim makeVisible As Boolean
        If Demos Is Nothing Then
            makeVisible = False
        Else
            makeVisible = True
        End If
        For Each cntrl As Control In gbSystem.Controls
            cntrl.Visible = makeVisible
        Next
        'the system error lable should be the opposite of the data labels
        lblSystemError.Visible = (Not makeVisible)
    End Sub

End Class
