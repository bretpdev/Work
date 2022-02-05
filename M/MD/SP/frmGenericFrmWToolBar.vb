Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmGenericFrmWToolBar
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        PrintSpilGuts()
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
    Protected WithEvents Options As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents ColorsDlg As System.Windows.Forms.ColorDialog
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuAskDUDE1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuAskDUDE As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem10 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem11 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem13 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem12 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem14 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGenericFrmWToolBar))
        Me.Options = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuAskDUDE1 = New System.Windows.Forms.MenuItem
        Me.MenuAskDUDE = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.MenuItem13 = New System.Windows.Forms.MenuItem
        Me.MenuItem10 = New System.Windows.Forms.MenuItem
        Me.MenuItem11 = New System.Windows.Forms.MenuItem
        Me.MenuItem12 = New System.Windows.Forms.MenuItem
        Me.MenuItem14 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.ColorsDlg = New System.Windows.Forms.ColorDialog
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'Options
        '
        Me.Options.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuAskDUDE1, Me.MenuItem7, Me.MenuItem13, Me.MenuItem4})
        '
        'MenuAskDUDE1
        '
        Me.MenuAskDUDE1.Index = 0
        Me.MenuAskDUDE1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuAskDUDE})
        Me.MenuAskDUDE1.Text = "&Tools"
        '
        'MenuAskDUDE
        '
        Me.MenuAskDUDE.Index = 0
        Me.MenuAskDUDE.Text = "Ask &DUDE"
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 1
        Me.MenuItem7.Text = "T&raining Modules"
        '
        'MenuItem13
        '
        Me.MenuItem13.Index = 2
        Me.MenuItem13.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem10, Me.MenuItem11, Me.MenuItem12, Me.MenuItem14})
        Me.MenuItem13.Text = "Non-System Calls"
        '
        'MenuItem10
        '
        Me.MenuItem10.Index = 0
        Me.MenuItem10.Text = "Regents’ Log In"
        '
        'MenuItem11
        '
        Me.MenuItem11.Index = 1
        Me.MenuItem11.Text = "Single Moms"
        '
        'MenuItem12
        '
        Me.MenuItem12.Index = 2
        Me.MenuItem12.Text = "Utah Futures"
        '
        'MenuItem14
        '
        Me.MenuItem14.Index = 3
        Me.MenuItem14.Text = "No UHEAA Connection"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 3
        Me.MenuItem4.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem5, Me.MenuItem6, Me.MenuItem8, Me.MenuItem9})
        Me.MenuItem4.Text = "&Help"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 0
        Me.MenuItem5.Text = "&Dictionary"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 1
        Me.MenuItem6.Text = "&About DUDE"
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 2
        Me.MenuItem8.Text = "Unexpected Result"
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 3
        Me.MenuItem9.Text = "Bright Idea"
        '
        'frmGenericFrmWToolBar
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(990, 679)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1000, 732)
        Me.Menu = Me.Options
        Me.Name = "frmGenericFrmWToolBar"
        Me.Text = ".."
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _streamToPrint As StreamReader
    Private _printFont As Drawing.Font

    Private Sub Trans40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SP.UsrInf.UpdateSettingsFile(SP.UserInfo.WhatSettingToUpdate.Opacity, 0.4)
    End Sub

    Private Sub Trans60_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SP.UsrInf.UpdateSettingsFile(SP.UserInfo.WhatSettingToUpdate.Opacity, 0.6)
    End Sub

    Private Sub Trans80_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SP.UsrInf.UpdateSettingsFile(SP.UserInfo.WhatSettingToUpdate.Opacity, 0.8)
    End Sub

    Private Sub Trans100_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SP.UsrInf.UpdateSettingsFile(SP.UserInfo.WhatSettingToUpdate.Opacity, 1)
    End Sub

    Private Sub BackgroundColors_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ColorsDlg.Color = Me.BackColor
        ColorsDlg.FullOpen = True
        ColorsDlg.ShowDialog()
        SP.UsrInf.UpdateSettingsFile(SP.UserInfo.WhatSettingToUpdate.BackColor, ColorsDlg.Color)
    End Sub

    Private Sub TextColors_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ColorsDlg.Color = Me.BackColor
        ColorsDlg.FullOpen = True
        ColorsDlg.ShowDialog()
        SP.UsrInf.UpdateSettingsFile(SP.UserInfo.WhatSettingToUpdate.ForeColor, ColorsDlg.Color)
    End Sub

    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem5.Click
        SP.DisplayHawaiianDictionary()
    End Sub

    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem6.Click
        SP.DisplayAboutDude()
    End Sub

    'display the Ask DUDE information form
    Private Sub btnAsk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SP.DisplayAskDude()
    End Sub

    Sub runPP()
        Dim Training As New frmTraining
        Training.Show()
    End Sub

    Private Sub MenuAskDUDE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAskDUDE.Click
        SP.DisplayAskDude()
    End Sub

    Private Sub MenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem7.Click
        Dim T As Thread = New Thread(AddressOf runPP)
        T.Start()
    End Sub

    Private Sub MenuItem8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem8.Click
        If SP.frmEmailComments.UnexpectedResults() = True Then
            AppActivate(Me.Text)
            Exit Sub
        End If
    End Sub

    Private Sub MenuItem9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem9.Click
        If SP.frmEmailComments.BrightIdea() = True Then
            AppActivate(Me.Text)
            Exit Sub
        End If
        AppActivate(Me.Text)
    End Sub

    Private Sub MenuItem10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem10.Click
        'update DB with call categorization data
        DataAccess.AddCallCategorizationRecord(New CallCategorizationEntry() With {.Category = "Regents", .Comments = "Accessed Regents System"})
        Shell("C:\Enterprise Program Files\RegentsScholarshipProgram\RegentsScholarshipFrontEnd.exe")
    End Sub

    Sub PrintSpilGuts()
        'print the last record if DUDE exited prematurely
        If Dir("T:\MauiDUDE_LastWords.txt") <> "" Then
            Try
                _streamToPrint = New StreamReader("T:\MauiDUDE_LastWords.txt")
                Try
                    _printFont = New Drawing.Font("Arial", 10)
                    Dim pd As Drawing.Printing.PrintDocument = New Drawing.Printing.PrintDocument   'Assumes the default printer
                    AddHandler pd.PrintPage, AddressOf Pd_PrintPage
                    pd.Print()
                Finally
                    _streamToPrint.Close()
                End Try

            Catch ex As Exception
                MessageBox.Show("An error occurred printing the file - " + ex.Message)
            End Try

            SP.frmKnarlyDUDE.KnarlyDude("It Looks Like DUDE Wiped Out last time it ran. The Information from the last run can be found at your printer.", "Printing Error")
            Kill("T:\MauiDUDE_LastWords.txt")
            'Duplicate code as above to check if the file is on T:\ drive
        ElseIf Dir("T:\MauiDUDE_LastWords.txt") <> "" Then
            Try
                _streamToPrint = New StreamReader("T:\MauiDUDE_LastWords.txt")
                Try
                    _printFont = New Drawing.Font("Arial", 10)
                    Dim pd As Drawing.Printing.PrintDocument = New Drawing.Printing.PrintDocument   'Assumes the default printer
                    AddHandler pd.PrintPage, AddressOf Pd_PrintPage
                    pd.Print()
                Finally
                    _streamToPrint.Close()
                End Try

            Catch ex As Exception
                MessageBox.Show("An error occurred printing the file - " + ex.Message)
            End Try

            SP.frmKnarlyDUDE.KnarlyDude("It Looks Like DUDE Wiped Out last time it ran. The Information from the last run can be found at your printer.", "Printing Error")
            Kill("T:\MauiDUDE_LastWords.txt")
        End If
    End Sub

    Private Sub Pd_PrintPage(ByVal sender As Object, ByVal ev As System.Drawing.Printing.PrintPageEventArgs)
        'Event fired for each page to print
        Dim lpp As Single = 0
        Dim yPos As Single = 0
        Dim count As Integer = 0
        Dim leftMargin As Single = ev.MarginBounds.Left
        Dim topMargin As Single = ev.MarginBounds.Top
        Dim line As String
        'Work out the number of lines per page
        'Use the MarginBounds on the event to do this
        lpp = ev.MarginBounds.Height / _printFont.GetHeight(ev.Graphics)
        'Now iterate over the file printing out each line
        'NOTE WELL: This assumes that a single line is not wider than the page width
        'Check count first so that we don't read line that we won't print
        line = _streamToPrint.ReadLine()
        While (count < lpp And line <> Nothing)
            yPos = topMargin + (count * _printFont.GetHeight(ev.Graphics))
            'Print Preview control will not work.
            ev.Graphics.DrawString(line, _printFont, Brushes.Black, leftMargin, _
                                    yPos, New StringFormat)
            count = count + 1
            If (count < lpp) Then
                line = _streamToPrint.ReadLine()
            End If
        End While
        'If we have more lines then print another page
        If (line <> Nothing) Then
            ev.HasMorePages = True
        Else
            ev.HasMorePages = False
        End If
    End Sub

    Private Sub MenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem11.Click
        Dim browser As New frmBrowserWindow(frmGenericCallCategorization.CallCategory.SingleMoms, "http://www.uheaa.org/singlemom/index.html")
        Me.Visible = False
        browser.ShowDialog()
        Me.Visible = True
    End Sub

    Private Sub MenuItem12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem12.Click
        Dim browser As New frmBrowserWindow(frmGenericCallCategorization.CallCategory.UtahFutures, "http://utahfutures.org/")
        Me.Visible = False
        browser.ShowDialog()
        Me.Visible = True
    End Sub

    Private Sub MenuItem14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem14.Click
        Dim NoUheaaConnection As New frmGenericCallCategorization(frmGenericCallCategorization.CallCategory.NoUHEAAConnection)
        Me.Visible = False
        NoUheaaConnection.ShowDialog()
        Me.Visible = True
    End Sub
End Class
