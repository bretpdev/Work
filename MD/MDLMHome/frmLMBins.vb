Imports System.Drawing
Imports System.Windows.Forms

Public Class frmLMBins
    Inherits SP.frmBins

    Protected OptionSelected As WhatIAmGoingToGiveYou
    Friend WithEvents LmBinFrmBtns1 As MDLMHome.LMBinFrmBtns
    Friend WithEvents cbPreviousContacts As System.Windows.Forms.ComboBox
    Shared FrmLocation As Point 'so form appears in the same location if user moves it because this is shared it is stateless


    Public Sub New() 'needed for the VS designer (never used)
        MyBase.New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal BU As String, ByRef tPreviousContacts As ArrayList)
        MyBase.New(BU)
        InitializeComponent()
        cbPreviousContacts.Items.AddRange(tPreviousContacts.ToArray())
        'setup continue button as form accept button
        Me.AcceptButton = Me.LmBinFrmBtns1.btnContinue
    End Sub

    Public Enum WhatIAmGoingToGiveYou
        SSN = 0
        Bin = 1
        SSN4OverflowACPCall = 2
        SSN4OverflowNotACPCall = 3
        AccountMaintenance = 4
    End Enum

    Public Shadows Function Showdialog() As WhatIAmGoingToGiveYou
        'display form
        MyBase.ShowDialog()
        'return to caller what was selected by the user
        If SelectedBin Is Nothing Then
            Showdialog = OptionSelected
        Else
            Showdialog = WhatIAmGoingToGiveYou.Bin
        End If
        Me.Close()
    End Function

    Public Sub SetOptionSelected(ByVal tOptionSelected As WhatIAmGoingToGiveYou)
        OptionSelected = tOptionSelected
    End Sub

    Private Sub InitializeComponent()
        Me.cbPreviousContacts = New System.Windows.Forms.ComboBox
        Me.LmBinFrmBtns1 = New MDLMHome.LMBinFrmBtns
        Me.SuspendLayout()
        '
        'cbPreviousContacts
        '
        Me.cbPreviousContacts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPreviousContacts.FormattingEnabled = True
        Me.cbPreviousContacts.Location = New System.Drawing.Point(422, 673)
        Me.cbPreviousContacts.Name = "cbPreviousContacts"
        Me.cbPreviousContacts.Size = New System.Drawing.Size(150, 21)
        Me.cbPreviousContacts.TabIndex = 2
        '
        'LmBinFrmBtns1
        '
        Me.LmBinFrmBtns1.Location = New System.Drawing.Point(204, -2)
        Me.LmBinFrmBtns1.Name = "LmBinFrmBtns1"
        Me.LmBinFrmBtns1.Size = New System.Drawing.Size(784, 84)
        Me.LmBinFrmBtns1.TabIndex = 1
        '
        'frmLMBins
        '
        Me.ClientSize = New System.Drawing.Size(990, 703)
        Me.Controls.Add(Me.cbPreviousContacts)
        Me.Controls.Add(Me.LmBinFrmBtns1)
        Me.Name = "frmLMBins"
        Me.Controls.SetChildIndex(Me.LmBinFrmBtns1, 0)
        Me.Controls.SetChildIndex(Me.cbPreviousContacts, 0)
        Me.Controls.SetChildIndex(Me.pnlBins, 0)
        Me.ResumeLayout(False)

    End Sub

    Private Sub cbPreviousContacts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbPreviousContacts.SelectedIndexChanged
        'plug SSN into user provided text box
        LmBinFrmBtns1.tbSSNOrAN.Text = cbPreviousContacts.SelectedItem.ToString
        LmBinFrmBtns1.btnContinue.PerformClick()
    End Sub

    Private Sub frmLMBins_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        FrmLocation = Me.Location
    End Sub

    Private Sub frmLMBins_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'move form to previously set location
        If FrmLocation <> Nothing Then
            Me.Left = FrmLocation.X
            Me.Top = FrmLocation.Y
        End If
    End Sub
End Class
