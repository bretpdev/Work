Imports System.Windows.Forms

Public Class ExceptionForm
    Private Const BORDER_WIDTH As Integer = 12
    Private Const EM_AVG_WIDTH As Integer = 6
    Private Const EM_HEIGHT As Integer = 15
    Private Const TITLE_BAR_HEIGHT As Integer = 30
    Private _exception As Exception

#Region "Event handlers"
    Private Sub btnContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinue.Click
        Me.Close()
    End Sub

    Private Sub btnQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuit.Click
        Application.Exit()
    End Sub

    Private Sub btnDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        'Declare a static boolean to keep track of the button state.
        Static viewDetail As Boolean = False

        'Switch state and respond to the new state.
        viewDetail = Not viewDetail

        If viewDetail Then
            btnDetails.Text = "Hide Details"
            'Show the Stack Trace text box.
            txtStackTrace.Visible = True
            'Set the Stack Trace text box's height.
            Dim lineWidth As Integer = Decimal.ToInt32(Math.Ceiling(Convert.ToDecimal(txtStackTrace.Width) / Convert.ToDecimal(EM_AVG_WIDTH)))
            Dim numberOfLines As Integer = Decimal.ToInt32(Math.Ceiling(Convert.ToDecimal(_exception.StackTrace.Length) / Convert.ToDecimal(lineWidth)))
            Dim textHeight As Integer = numberOfLines * EM_HEIGHT
            'Expand the form's height to just below the Stack Trace text box.
            Me.Height = TITLE_BAR_HEIGHT + txtStackTrace.Top + textHeight + BORDER_WIDTH
        Else
            btnDetails.Text = "View Details"
            'Hide the Stack Trace text box.
            txtStackTrace.Visible = False
            'Shrink the form's height to just below the Details button.
            Me.Height = TITLE_BAR_HEIGHT + btnDetails.Bottom + BORDER_WIDTH
        End If
    End Sub
#End Region 'Event handlers

    Public Sub New(ByVal topException As Exception)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _exception = topException
    End Sub

    Public Overloads Sub Show()
        'This form should always be modal, so call ShowDialog().
        Me.ShowDialog()
    End Sub

    Public Overloads Sub ShowDialog()
        lblMessage.Text = _exception.Message
        'Add messages from all inner exceptions to the top of the stack trace window.
        Dim inner As Exception = _exception.InnerException
        Do While inner IsNot Nothing
            txtStackTrace.Text += inner.Message + Environment.NewLine
            inner = inner.InnerException
        Loop
        'Add the top exception's stack trace.
        txtStackTrace.Text += _exception.StackTrace
        'Set the form up.
        txtStackTrace.Visible = False
        Me.Height = TITLE_BAR_HEIGHT + btnDetails.Bottom + BORDER_WIDTH

        MyBase.ShowDialog()
    End Sub
End Class