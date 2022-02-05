Public Class frmAltAddress

    ''' <summary>
    ''' Defualt constructor (DO NOT USE)
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal demos As Demographics)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        demos.Addr2 = ""
        AltAddressControl.Databind(demos)
        AltAddressControl.lblMBL.Visible = False
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If AltAddressControl.ValidUserInput() = True Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub
End Class