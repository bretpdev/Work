Imports System.Windows.Forms
Public Class frmDeferForbHistory
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        '****Me.BackColor = SP.GeneralBackColor
        '****Me.ForeColor = SP.GeneralForeColor
        '****LVHist.BackColor = SP.GeneralBackColor
        '****LVHist.ForeColor = SP.GeneralForeColor
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.Hide()
        'If disposing Then
        '    If Not (components Is Nothing) Then
        '        components.Dispose()
        '    End If
        'End If
        'MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents IL As System.Windows.Forms.ImageList
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents LVHist As System.Windows.Forms.ListView
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDeferForbHistory))
        Me.LVHist = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.IL = New System.Windows.Forms.ImageList(Me.components)
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'LVHist
        '
        Me.LVHist.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8})
        Me.LVHist.FullRowSelect = True
        Me.LVHist.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.LVHist.Location = New System.Drawing.Point(8, 40)
        Me.LVHist.MultiSelect = False
        Me.LVHist.Name = "LVHist"
        Me.LVHist.Size = New System.Drawing.Size(696, 176)
        Me.LVHist.SmallImageList = Me.IL
        Me.LVHist.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.LVHist.TabIndex = 0
        Me.LVHist.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Defer Or Forb"
        Me.ColumnHeader1.Width = 181
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Begin Date"
        Me.ColumnHeader2.Width = 73
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "End Date"
        Me.ColumnHeader3.Width = 67
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Cap Ind"
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Days Used"
        Me.ColumnHeader5.Width = 72
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Days Left"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Total Months Used"
        Me.ColumnHeader7.Width = 109
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Cert Date"
        '
        'IL
        '
        Me.IL.ImageSize = New System.Drawing.Size(16, 16)
        Me.IL.ImageStream = CType(resources.GetObject("IL.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.IL.TransparentColor = System.Drawing.Color.Transparent
        '
        'btnClose
        '
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(320, 224)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("PosterBodoni BT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(696, 23)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Deferment/Forbearance History"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmDeferForbHistory
        '
        Me.AcceptButton = Me.btnClose
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(716, 261)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.LVHist)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(724, 288)
        Me.MinimumSize = New System.Drawing.Size(724, 288)
        Me.Name = "frmDeferForbHistory"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Deferment/Forbearance History"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public NoResults As Boolean = False

    Public Overloads Sub Show(Optional ByVal WarnIfNoResults As Boolean = True)
        Process()
        MyBase.Show()
        If NoResults And WarnIfNoResults = True Then
            SP.frmWhoaDUDE.WhoaDUDE("No results were found.", "Deferment/Forbearance History")
            Me.Hide()
            Exit Sub
        End If
    End Sub

    Public Sub Process()
        Dim SelectionCounter As Integer
        SP.Processing.Visible = True
        SP.Processing.Refresh()
        'only collect the data if it hasn't be collected yet
        If LVHist.Items.Count = 0 Then
            If NoResults = False Then
                'switch keys until right option is displayed
                If SP.Q.Check4Text(1, 74, "TCX0D") Or SP.Q.Check4Text(1, 72, "TCX06") Then
                    While SP.Q.Check4Text(24, 52, "LOAN") = False
                        SP.Q.Hit("F2")
                    End While
                ElseIf SP.Q.Check4Text(1, 72, "TCX0I") Then
                    While SP.Q.Check4Text(24, 58, "LOAN") = False
                        SP.Q.Hit("F2")
                    End While
                End If
                'access TS26
                SP.Q.Hit("F8")
                If SP.Q.Check4Text(1, 72, "TSX28") Then
                    'selection screen
                    SelectionCounter = 1
                    While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                        If SelectionCounter > 9 Then
                            SP.Q.PutText(21, 12, CStr(SelectionCounter), True)
                        Else
                            SP.Q.PutText(21, 12, "0" & CStr(SelectionCounter), True)
                        End If
                        If SP.Q.Check4Text(23, 2, "01032 SELECTION MUST CORRESPOND TO A DISPLAYED KEY") Then
                            'if there are no other loans to select
                            SelectionCounter = 1
                            SP.Q.Hit("F8") 'try and page forward to the next set of loans
                        Else
                            'target screen
                            PopulateListView(True)
                            SelectionCounter += 1
                        End If
                    End While
                Else
                    'target screen
                    PopulateListView(False)
                End If
                If LVHist.Items.Count = 0 Then
                    NoResults = True
                End If
            End If
        End If
        While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
            SP.Q.Hit("F12")
        End While
        SP.Processing.Visible = False
    End Sub

    Private Sub PopulateListView(ByVal SelectionScreen As Boolean)
        'This subroutine gets called by the Process() subroutine when the target screen for TS26 is found.
        Dim Row As Integer
        Dim LVIdx As Integer
        Dim ExistingItem As Integer
        Dim IsAlreadyInList As Boolean

        SP.Q.Hit("F2")
        SP.Q.Hit("F7")
        If SP.Q.Check4Text(1, 72, "TSX31") Then
            Row = 12
            While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                IsAlreadyInList = False
                For ExistingItem = 0 To LVHist.Items.Count - 1
                    'Check whether this deferment or forbearance is already in the list.
                    If LVHist.Items(ExistingItem).SubItems(0).Text.EndsWith(SP.Q.GetText(Row, 10, 11)) _
                        And LVHist.Items(ExistingItem).SubItems(1).Text = Replace(SP.Q.GetText(Row, 21, 8), " ", "/") _
                        And LVHist.Items(ExistingItem).SubItems(2).Text = Replace(SP.Q.GetText(Row, 30, 8), " ", "/") _
                    Then
                        IsAlreadyInList = True
                        Exit For
                    End If
                Next
                'Add this deferment or forbearance if it's not in the list already.
                If IsAlreadyInList = False Then
                    Select Case SP.Q.GetText(Row, 6, 1)
                        Case "D"
                            LVIdx = LVHist.Items.Add("Defer: " & SP.Q.GetText(Row, 10, 11)).Index()
                            LVHist.Items(LVIdx).ImageIndex = 1
                        Case "F"
                            LVIdx = LVHist.Items.Add("Forb: " & SP.Q.GetText(Row, 10, 11)).Index()
                            LVHist.Items(LVIdx).ImageIndex = 0
                    End Select
                    LVHist.Items(LVIdx).SubItems.Add(Replace(SP.Q.GetText(Row, 21, 8), " ", "/"))
                    LVHist.Items(LVIdx).SubItems.Add(Replace(SP.Q.GetText(Row, 30, 8), " ", "/"))
                    LVHist.Items(LVIdx).SubItems.Add(SP.Q.GetText(Row, 49, 1))
                    LVHist.Items(LVIdx).SubItems.Add(SP.Q.GetText(Row, 53, 4))
                    LVHist.Items(LVIdx).SubItems.Add(SP.Q.GetText(Row, 58, 5))
                    LVHist.Items(LVIdx).SubItems.Add(SP.Q.GetText(Row, 64, 6))
                    LVHist.Items(LVIdx).SubItems.Add(Replace(SP.Q.GetText(Row, 72, 8), " ", "/"))
                End If
                Row += 1
                If SP.Q.Check4Text(Row, 6, " ") Then
                    SP.Q.Hit("F8")
                    Row = 12
                End If
            End While
            If SelectionScreen = True Then
                SP.Q.Hit("F12")
                SP.Q.Hit("F12")
            End If
        Else
            If SelectionScreen = True Then
                SP.Q.Hit("F12")
            End If
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub

    Public Function GetLV() As ListView
        GetLV = LVHist
    End Function
End Class
