Imports System.Data
Imports System.Threading

Public Class frmLMBinsHomePage
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SP.UsrInf.ChangeFormSettings(Me)
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
    Friend WithEvents ActCmt As MDLMHome.ActivityCmts

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLMBinsHomePage))
        Me.ActCmt = New MDLMHome.ActivityCmts
        Me.SuspendLayout()
        '
        'ActCmt
        '
        Me.ActCmt.Location = New System.Drawing.Point(3, 159)
        Me.ActCmt.Name = "ActCmt"
        Me.ActCmt.Size = New System.Drawing.Size(946, 138)
        Me.ActCmt.TabIndex = 0
        '
        'frmLMBinsHomePage
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(952, 604)
        Me.Controls.Add(Me.ActCmt)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLMBinsHomePage"
        Me.Text = "Account Resolution Home Page"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Bor As BorrowerLM
    Friend ActCmtCntrl As ActivityCmts

    ' '' '' ''this function does object and screen creation and coordination for the Loan Management (Account Resolution) bin and home page process
    '' '' ''Public Shared Sub LMBinHomePageProcCoord(ByVal BU As String)
    '' '' ''    Dim BinsFrm As frmLMBins
    '' '' ''    Dim HomePage As frmLMBinsHomePage
    '' '' ''    Dim BinBeingWorked As SP.ABin
    '' '' ''    Dim BorLt As SP.BorrowerLite
    '' '' ''    Dim BinPageSelectedOption As frmLMBins.WhatIAmGoingToGiveYou
    '' '' ''    While True
    '' '' ''        'always start out with the bins screen
    '' '' ''        BinsFrm = New frmLMBins(BU)
    '' '' ''        BinPageSelectedOption = BinsFrm.Showdialog()
    '' '' ''        If BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.SSNOrAcctNumAcctMain Or BinPageSelectedOption = frmLMBins.WhatIAmGoingToGiveYou.SSNOrAcctNumIncomingCall Then
    '' '' ''            'incoming call or account maintanance selected on bins screen
    '' '' ''            BorLt = New SP.BorrowerLite
    '' '' ''            'get SSN
    '' '' ''            BorLt.SSN = BinsFrm.GetSSN()
    '' '' ''            'do translation from account number to SSN
    '' '' ''            If BorLt.ConvertAccToSSN(BorLt.SSN) Then
    '' '' ''                'if SSN isn't valid then the borrower lite var is changed to nothing and flows through the rest of the loop and displays the bins page again immediately
    '' '' ''                BorLt = Nothing
    '' '' ''            End If
    '' '' ''        Else
    '' '' ''            'get selected bin from Bins form
    '' '' ''            BinBeingWorked = BinsFrm.GetSelectedBin()
    '' '' ''            'Get task from queue with tasks in selected bin
    '' '' ''            BorLt = SP.BorrowerLite.GetOLTaskForBin(BinBeingWorked.GetQueuesToBeWorked())
    '' '' ''        End If
    '' '' ''        'check if borrower came through populated
    '' '' ''        While Not (BorLt Is Nothing)
    '' '' ''            'process borrower lite if populated else skip it and display bin screen again
    '' '' ''            HomePage = New frmLMBinsHomePage()
    '' '' ''            'if false is returned below then for one reason or another the homepage shouldn't be shown
    '' '' ''            If HomePage.DoPreShowFormProcessing(BinPageSelectedOption, BorLt) Then
    '' '' ''                HomePage.ShowDialog()
    '' '' ''            End If
    '' '' ''            If (BinBeingWorked Is Nothing) Then
    '' '' ''                'if a bin wasn't selected and an SSN was provided then exit the loop and show bin form again
    '' '' ''                Exit While
    '' '' ''            Else
    '' '' ''                'Get task from queue with tasks in selected bin
    '' '' ''                BorLt = SP.BorrowerLite.GetOLTaskForBin(BinBeingWorked.GetQueuesToBeWorked())
    '' '' ''            End If
    '' '' ''        End While
    '' '' ''    End While
    '' '' ''End Sub

    ' '' '' ''return false if home page shouldn't be shown to the user
    '' '' ''Public Function DoPreShowFormProcessing(ByVal ModeOfProc As frmLMBins.WhatIAmGoingToGiveYou, ByVal BorrLite As SP.BorrowerLite) As Boolean
    '' '' ''    Dim Demo As SP.frmDemographics
    '' '' ''    DoPreShowFormProcessing = True
    '' '' ''    Bor = New BorrowerLM(BorrLite, Me)
    '' '' ''    If ModeOfProc = frmLMBins.WhatIAmGoingToGiveYou.SSNOrAcctNumIncomingCall Then
    '' '' ''        Demo = New SP.frmDemographics(Bor)
    '' '' ''        'if false is returned below then for some reason the demographic screen wasn't able to collect and/or display the information it needed (homepage not displayed)
    '' '' ''        If Demo.PopulateFrm(True) = False Then
    '' '' ''            DoPreShowFormProcessing = False
    '' '' ''            Exit Function
    '' '' ''        End If
    '' '' ''        Bor.turboSpeed.Start() 'multi-thread while demographics is up
    '' '' ''        Demo.ShowDialog()
    '' '' ''        'check if the user clicked the back button on the demgraphic homepage
    '' '' ''        If Demo.BackButtonClicked Then
    '' '' ''            DoPreShowFormProcessing = False
    '' '' ''            Exit Function
    '' '' ''        End If
    '' '' ''    ElseIf ModeOfProc = frmLMBins.WhatIAmGoingToGiveYou.SSNOrAcctNumAcctMain Or ModeOfProc = frmLMBins.WhatIAmGoingToGiveYou.Bin Then
    '' '' ''        Bor.Turbo()
    '' '' ''    End If
    '' '' ''End Function

    '' '' ''Private Sub frmLMBinsHomePage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    '' '' ''End Sub
End Class
