Imports System.Windows.Forms


Public Class frmBins 'the bins form must be inherited
    Inherits SP.frmGenericFrmWToolBar

#Region " Windows Form Designer generated code "

    Public Sub New() 'this is needed so the designer works on the form that inherits (this form this constructor is never used)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    Public Sub New(ByVal BU As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'init session
        SP.UsrInf.FigureOutDB() 'setup DB connection
        BusinessUnit = BU
        SP.UsrInf.BUHasHomePage = True 'setting this to true makes it so the time out timer works.
        SP.Processing.Visible = True 'display processing window
        SP.Processing.Refresh() 'the form doesn't get written to the screen without this
        TheBins = New Bins(BusinessUnit, Me) 'create bins object and populate
        SP.Processing.Visible = False 'hide processing screen
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Public WithEvents pnlBins As System.Windows.Forms.Panel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBins))
        Me.pnlBins = New System.Windows.Forms.Panel
        Me.SuspendLayout()
        '
        'pnlBins
        '
        Me.pnlBins.AutoScroll = True
        Me.pnlBins.Location = New System.Drawing.Point(8, 88)
        Me.pnlBins.Name = "pnlBins"
        Me.pnlBins.Size = New System.Drawing.Size(980, 584)
        Me.pnlBins.TabIndex = 0
        '
        'frmBins
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(990, 703)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlBins)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1000, 752)
        Me.MinimizeBox = False
        Me.Name = "frmBins"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maui DUDE Bins"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Protected BusinessUnit As String
    Protected TheBins As Bins
    Protected SelectedBin As ABin
    Protected SSN As String

    Public Function GetBusinessUnit() As String
        Return BusinessUnit
    End Function

    Public Sub SetSelectedBin(ByVal SBin As ABin)
        SelectedBin = SBin
    End Sub

    Public Function GetSelectedBin() As ABin
        Return SelectedBin
    End Function

    Public Function GetSSN() As String
        Return SSN
    End Function

    Public Sub SetSSN(ByVal tSSN As String)
        SSN = tSSN
    End Sub

End Class
