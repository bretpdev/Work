Imports System.Windows.Forms

Public Class SystemDemographicsForDisplayOnly
    Private _demos As Demographics
    ''' <summary>
    ''' Demographics object that the control will be bound to and work with.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Demos() As Demographics
        Get
            Return _demos
        End Get
        Set(ByVal value As Demographics)
            _demos = value
        End Set
    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub SetupImages(ByVal RM As Resources.ResourceManager)
        'create resource manager for SP if a specific one wasn't sent in
        If RM Is Nothing Then
            RM = New Resources.ResourceManager("SP.MauiDUDERes", Me.GetType.Assembly)
        End If
        If Today.Month = 12 Then
            lblSystemError.Image = CType(RM.GetObject("SantaForDemo"), System.Drawing.Image)
        ElseIf Today.Month = 1 Or Today.Month = 2 Then
            lblSystemError.Image = CType(RM.GetObject("PolarBear"), System.Drawing.Image)
        ElseIf Today.Month = 3 Or Today.Month = 4 Or Today.Month = 5 Then
            lblSystemError.Image = CType(RM.GetObject("DopyForDemo"), System.Drawing.Image)
        Else
            lblSystemError.Image = CType(RM.GetObject("HangLoose"), System.Drawing.Image)
        End If
    End Sub

    Public Overridable Sub DataBind(ByVal tDemos As Demographics)
        _demos = tDemos
        Dim _phone As String = _demos.Phone
        Dim _otherPhone As String = Demos.OtherPhoneNum
        If _phone <> Nothing AndAlso _phone.Length > 0 Then _phone = _phone.Insert(6, "-").Insert(3, "-")
        If _otherPhone.Length > 0 Then _otherPhone = _otherPhone.Insert(6, "-").Insert(3, "-")
        DemographicsBindingSource.DataSource = _demos
        lblPhone.Text = _phone
        lblOther.Text = _otherPhone
        HideOrShowDataLabels()
    End Sub

    'is also overridden on the Compass side because the Compass side adds controls
    Public Overridable Sub HideOrShowDataLabels()
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

    Private Sub btnUseThisAddr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUseThisAddr.Click
        If _demos IsNot Nothing Then CType(Me.ParentForm, frmDemographics).DemoForUpdate.CopyObjectDataToTextBoxes(_demos)
    End Sub
End Class
