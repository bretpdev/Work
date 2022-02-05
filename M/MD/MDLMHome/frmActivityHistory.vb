Imports System.Reflection
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmActivityHistory
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

    Public Sub New(ByVal tBor As SP.Borrower)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Bor = tBor
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
    Friend WithEvents lbltittle As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents AC As MDLMHome.ActivityCmts
    Friend WithEvents DV As System.Data.DataView
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmActivityHistory))
        Me.lbltittle = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        Me.DV = New System.Data.DataView
        Me.AC = New MDLMHome.ActivityCmts
        CType(Me.DV, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbltittle
        '
        Me.lbltittle.Font = New System.Drawing.Font("Bodoni MT Black", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbltittle.Location = New System.Drawing.Point(48, 8)
        Me.lbltittle.Name = "lbltittle"
        Me.lbltittle.Size = New System.Drawing.Size(752, 23)
        Me.lbltittle.TabIndex = 0
        Me.lbltittle.Text = "Borrower Activity History"
        Me.lbltittle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnClose.Location = New System.Drawing.Point(384, 397)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        '
        'DV
        '
        '
        'AC
        '
        Me.AC.Location = New System.Drawing.Point(16, 40)
        Me.AC.Name = "AC"
        Me.AC.Size = New System.Drawing.Size(808, 312)
        Me.AC.TabIndex = 5
        '
        'frmActivityHistory
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(840, 427)
        Me.Controls.Add(Me.AC)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lbltittle)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmActivityHistory"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maui DUDE Borrower 30 Day Activity History"
        CType(Me.DV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Bor As SP.Borrower

    Public Overloads Function Show(ByVal days As Integer, ByVal Tittle As String) As Boolean
        Dim TempAC As ActivityCmts
        Me.Text = Tittle
        lbltittle.Text = Tittle
        TempAC = AC 'hold on to pointer to original to use location and size later
        AC = New ActivityCmts(ActivityCmts.DaysOrNumberOf.Days, days, False, Bor)
        'remove temp control (old control)
        Me.Controls.Remove(TempAC)
        'add it to that controls of home page
        Me.Controls.Add(AC)
        AC.dgAH.BackColor = Me.BackColor
        AC.dgAH.ForeColor = Me.ForeColor
        AC.Location = TempAC.Location
        AC.Size = TempAC.Size

        If AC.GetSuccess() Then
            Me.Show()
        Else
            SP.frmWipeOut.WipeOut("The borrower doesn't appear to have activity comments for the desired time period.", "No Activity Comments")
            Try
                AppActivate("Maui DUDE Loan Managment HomePage")
            Catch ex As Exception
            End Try
        End If
    End Function

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub DV_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles DV.ListChanged
        AC.dgAH.TableStyles.Clear()
        AC.dgAH.TableStyles.Add(AC.MakeTableStyleLP50)
    End Sub

    Private Sub frmActivityHistory_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        AC.dgAH.Width = Me.Width - 28
        AC.dgAH.Height = Me.Height - 120
    End Sub

End Class