Imports System.Windows.Forms

Public Class frmGetAttemptInfo



    Private pRunThirdPartyAuthScript As Boolean
    Public Property RunThirdPartyAuthScript() As Boolean
        Get
            Return pRunThirdPartyAuthScript
        End Get
        Set(ByVal value As Boolean)
            pRunThirdPartyAuthScript = value
        End Set
    End Property

    Private pDisplayDemoGraphics As Boolean
    Public Property DisplayDemoGraphics() As Boolean
        Get
            Return pDisplayDemoGraphics
        End Get
        Set(ByVal value As Boolean)
            pDisplayDemoGraphics = value
        End Set
    End Property

    Private pActionCode As String
    Public Property ActionCode() As String
        Get
            Return pActionCode
        End Get
        Set(ByVal value As String)
            pActionCode = value
        End Set
    End Property

    Private ResultsItems(1)() As String


    Public Sub New(ByVal Phones As Generic.List(Of String))
        InitializeComponent()
        pRunThirdPartyAuthScript = False
        pDisplayDemoGraphics = False
        cbPhoneNums.Items.AddRange(Phones.ToArray())
        ReDim ResultsItems(0)(5)
        ReDim ResultsItems(1)(5)
        ResultsItems(0)(0) = "Answering Machine"
        ResultsItems(1)(0) = "DD089"
        ResultsItems(0)(1) = "Busy"
        ResultsItems(1)(1) = "DD059"
        ResultsItems(0)(2) = "No Answer"
        ResultsItems(1)(2) = "DD056"
        ResultsItems(0)(3) = "Third Party"
        ResultsItems(1)(3) = "DD051"
        ResultsItems(0)(4) = "Disconnected"
        ResultsItems(1)(4) = "DD008"
        ResultsItems(0)(5) = "Wrong Number"
        ResultsItems(1)(5) = "DD099"
        cbCallResults.Items.AddRange(ResultsItems(0))
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Hide()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        '****there is also a event handler in the homepage for this button****
        '****it uses the flags set here to display and runs scripts (this handler is always run first)
        If cbPhoneNums.SelectedIndex = -1 Or cbCallResults.SelectedIndex = -1 Then
            SP.frmWhoaDUDE.WhoaDUDE("You must select a phone number and the call result.  Please try again.", "Incomplete Data Selections")
            Exit Sub
        End If
        'if everything is provided
        pActionCode = ResultsItems(1)(cbCallResults.SelectedIndex)
        'handle special cases for user selected options
        If ResultsItems(0)(cbCallResults.SelectedIndex) = "Third Party" Then
            'third party
            If SP.frmYesNo.YesNo("Would you like to send a third party authorization form?") = False Then
                pRunThirdPartyAuthScript = False
            Else
                pRunThirdPartyAuthScript = True
            End If
        ElseIf ResultsItems(0)(cbCallResults.SelectedIndex) = "Disconnected" Or ResultsItems(0)(cbCallResults.SelectedIndex) = "Wrong Number" Then
            pDisplayDemoGraphics = True
        End If
    End Sub

End Class
