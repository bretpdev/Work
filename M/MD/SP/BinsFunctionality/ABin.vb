Imports System.Data.SqlClient

Public Class ABin
    Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal BU As String, ByVal BinName As String, ByVal FunctionKey As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        TaskCount = 0
        gbBinLevel.Text = 0
        gbBinLevel.Height = ZeroLevelBin
        TheBinName = BinName
        gbBin.Text = BinName
        gbBinLevel.BackColor = SP.UsrInf.FColor
        gbBinLevel.ForeColor = SP.UsrInf.BColor
        BusinessUnit = BU
        FKey = "F" + FunctionKey
        btnBin.Text = "F" + FunctionKey
        'collect data from DB
        Dim DA As SqlDataAdapter
        DS = New DataSet
        DA = New SqlDataAdapter("EXEC spGetQueueInfoForBin '" + TheBinName + "','" + BusinessUnit + "'", SP.UsrInf.Conn)
        DA.Fill(DS, "QueueInfo")
    End Sub

    ''' <summary>
    ''' Default constructor, DO NOT CALL!!!!
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub

    'UserControl overrides dispose to clean up the component list.
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
    Friend WithEvents gbBin As System.Windows.Forms.GroupBox
    Friend WithEvents gbBinLevel As System.Windows.Forms.GroupBox
    Friend WithEvents btnBin As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.gbBin = New System.Windows.Forms.GroupBox
        Me.gbBinLevel = New System.Windows.Forms.GroupBox
        Me.btnBin = New System.Windows.Forms.Button
        Me.gbBin.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbBin
        '
        Me.gbBin.Controls.Add(Me.gbBinLevel)
        Me.gbBin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbBin.Location = New System.Drawing.Point(8, 0)
        Me.gbBin.Name = "gbBin"
        Me.gbBin.Size = New System.Drawing.Size(120, 544)
        Me.gbBin.TabIndex = 0
        Me.gbBin.TabStop = False
        Me.gbBin.Text = "Bin Name"
        '
        'gbBinLevel
        '
        Me.gbBinLevel.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(64, Byte), CType(64, Byte))
        Me.gbBinLevel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.gbBinLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbBinLevel.Location = New System.Drawing.Point(8, 520)
        Me.gbBinLevel.Name = "gbBinLevel"
        Me.gbBinLevel.Size = New System.Drawing.Size(104, 20)
        Me.gbBinLevel.TabIndex = 0
        Me.gbBinLevel.TabStop = False
        '
        'btnBin
        '
        Me.btnBin.Location = New System.Drawing.Point(8, 552)
        Me.btnBin.Name = "btnBin"
        Me.btnBin.Size = New System.Drawing.Size(120, 23)
        Me.btnBin.TabIndex = 1
        Me.btnBin.Text = "Button1"
        '
        'ABin
        '
        Me.Controls.Add(Me.btnBin)
        Me.Controls.Add(Me.gbBin)
        Me.Name = "ABin"
        Me.Size = New System.Drawing.Size(135, 576)
        Me.gbBin.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Const MaximumHeightOfBin As Integer = 500 'All calculations done using this variable
    Const ZeroLevelBin As Integer = 20
    Private BusinessUnit As String
    Private DS As DataSet
    Private FKey As String
    Private QueuesWData As ArrayList

    Private TaskCount As Integer
    Public Property BinTaskCount() As Integer
        Get
            Return TaskCount
        End Get
        Set(ByVal value As Integer)
            TaskCount = value
        End Set
    End Property

    Private TheBinName As String
    Public Property BinName() As String
        Get
            Return TheBinName
        End Get
        Set(ByVal value As String)
            TheBinName = value
        End Set
    End Property


    'checks to see if the queue is part of this bin
    Public Function IHaveThatQueue(ByVal Queue As String) As Boolean
        If DS.Tables("QueueInfo").Select(String.Format("Queue = '{0}'", Queue)).GetLength(0) = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Sub UpdateQueueInfo(ByVal Queue As String, ByVal QueueCount As Integer)
        Dim I As Integer
        'find the queue row and update to show that there is data for it
        While DS.Tables("QueueInfo").Rows(I)("Queue") <> Queue
            I = I + 1
        End While
        DS.Tables("QueueInfo").Rows(I)("DataPresent") = "Y" 'track this originally in data set so we can keep track of priority
        UpdateTaskCountForBin(QueueCount)
    End Sub

    Private Sub UpdateTaskCountForBin(ByVal AddToCount As Integer)
        TaskCount = TaskCount + AddToCount
        gbBinLevel.Text = TaskCount.ToString
        gbBinLevel.Top = gbBinLevel.Top - AddToCount 'move location up so the group box expands down to starting point
        If TaskCount <= 500 Then 'graph only displays up to 500 so only the lable will change not the graph height
            gbBinLevel.Height = TaskCount + ZeroLevelBin
        Else
            gbBinLevel.Height = 500 + ZeroLevelBin
        End If
    End Sub

    Private Sub btnBin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBin.Click
        BtnClicked()
    End Sub

    Public Overridable Sub BtnClicked()
        Dim I As Integer = 0
        QueuesWData = New ArrayList
        'create array list from data set only noting queues with data (gathers info from dataset so the queues can be worked in priority)
        While I < DS.Tables("QueueInfo").Rows.Count
            If DS.Tables("QueueInfo").Rows(I)("DataPresent") = "Y" Then
                QueuesWData.Add(DS.Tables("QueueInfo").Rows(I)("Queue"))
            End If
            I = I + 1
        End While
        'if there are no queues with data then give user error message and exit sub
        If QueuesWData.Count = 0 Then
            SP.frmWhoaDUDE.WhoaDUDE("Whoa DUDE, the " + TheBinName + " bin doesn't have any tasks to work, why don't you try something different.", "Bin is empty")
            Exit Sub
        End If
        'set form's selected bin
        CType(Me.ParentForm, frmBins).SetSelectedBin(Me)
        Me.ParentForm.Hide()
    End Sub

    'function key event handler
    Private Sub FunctionKeyHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) 'ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim KeyToHandle As Windows.Forms.Keys
        'decide which function key to handle
        If FKey = "F1" Then
            KeyToHandle = Windows.Forms.Keys.F1
        ElseIf FKey = "F2" Then
            KeyToHandle = Windows.Forms.Keys.F2
        ElseIf FKey = "F3" Then
            KeyToHandle = Windows.Forms.Keys.F3
        ElseIf FKey = "F4" Then
            KeyToHandle = Windows.Forms.Keys.F4
        ElseIf FKey = "F5" Then
            KeyToHandle = Windows.Forms.Keys.F5
        ElseIf FKey = "F6" Then
            KeyToHandle = Windows.Forms.Keys.F6
        ElseIf FKey = "F7" Then
            KeyToHandle = Windows.Forms.Keys.F7
        ElseIf FKey = "F8" Then
            KeyToHandle = Windows.Forms.Keys.F8
        ElseIf FKey = "F9" Then
            KeyToHandle = Windows.Forms.Keys.F9
        ElseIf FKey = "F10" Then
            KeyToHandle = Windows.Forms.Keys.F10
        ElseIf FKey = "F11" Then
            KeyToHandle = Windows.Forms.Keys.F11
        ElseIf FKey = "F12" Then
            KeyToHandle = Windows.Forms.Keys.F12
        End If
        'handle calculated function key
        If e.KeyCode = KeyToHandle Then
            e.Handled = True
            BtnClicked()
        End If
    End Sub

    'sets up function key event handling
    Public Sub SetUpFKeyHandler()
        AddHandler Me.ParentForm.KeyUp, AddressOf FunctionKeyHandler
    End Sub

    'accessor function for data set
    Public Function GetDataSet() As DataSet
        Return DS
    End Function

    'accessor function for array list that contains all queues that have tasks to be worked in priority order
    Public Function GetQueuesToBeWorked() As ArrayList
        Return QueuesWData
    End Function

End Class
