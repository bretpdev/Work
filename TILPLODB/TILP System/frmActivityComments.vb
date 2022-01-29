Imports System.Data.SqlClient
Imports System.Reflection

Public Class frmActivityComments
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal tSSN As String, ByVal tTestMode As Boolean, ByVal tBorName As String, ByRef tTheUser As user)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SSN = tSSN
        TestMode = tTestMode
        BorName = tBorName
        TheUser = tTheUser
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
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents tbSSN As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbComments As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dgAH As System.Windows.Forms.DataGrid
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnHistory As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbName = New System.Windows.Forms.TextBox
        Me.tbSSN = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbComments = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.dgAH = New System.Windows.Forms.DataGrid
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnHistory = New System.Windows.Forms.Button
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgAH, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.tbName)
        Me.GroupBox2.Controls.Add(Me.tbSSN)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(56, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(716, 40)
        Me.GroupBox2.TabIndex = 21
        Me.GroupBox2.TabStop = False
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(160, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Name"
        '
        'tbName
        '
        Me.tbName.Enabled = False
        Me.tbName.Location = New System.Drawing.Point(196, 12)
        Me.tbName.MaxLength = 13
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(512, 20)
        Me.tbName.TabIndex = 9
        Me.tbName.Text = ""
        '
        'tbSSN
        '
        Me.tbSSN.Enabled = False
        Me.tbSSN.Location = New System.Drawing.Point(36, 12)
        Me.tbSSN.Name = "tbSSN"
        Me.tbSSN.Size = New System.Drawing.Size(120, 20)
        Me.tbSSN.TabIndex = 1
        Me.tbSSN.Text = ""
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(4, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "SSN"
        '
        'tbComments
        '
        Me.tbComments.Location = New System.Drawing.Point(8, 56)
        Me.tbComments.Multiline = True
        Me.tbComments.Name = "tbComments"
        Me.tbComments.Size = New System.Drawing.Size(808, 112)
        Me.tbComments.TabIndex = 22
        Me.tbComments.Text = ""
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(808, 16)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Comments"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'dgAH
        '
        Me.dgAH.CaptionVisible = False
        Me.dgAH.DataMember = ""
        Me.dgAH.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.dgAH.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgAH.Location = New System.Drawing.Point(8, 208)
        Me.dgAH.Name = "dgAH"
        Me.dgAH.ReadOnly = True
        Me.dgAH.RowHeadersVisible = False
        Me.dgAH.Size = New System.Drawing.Size(808, 304)
        Me.dgAH.TabIndex = 24
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(224, 176)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(120, 23)
        Me.btnSave.TabIndex = 27
        Me.btnSave.Text = "Save To History"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(352, 176)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(120, 23)
        Me.btnCancel.TabIndex = 28
        Me.btnCancel.Text = "Cancel"
        '
        'btnHistory
        '
        Me.btnHistory.Location = New System.Drawing.Point(480, 176)
        Me.btnHistory.Name = "btnHistory"
        Me.btnHistory.Size = New System.Drawing.Size(120, 23)
        Me.btnHistory.TabIndex = 29
        Me.btnHistory.Text = "Show History"
        '
        'frmActivityComments
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(824, 516)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnHistory)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.dgAH)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbComments)
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "frmActivityComments"
        Me.Text = "Activity Comments"
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.dgAH, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private SSN As String
    Private BorName As String
    Private TestMode As Boolean
    Private Conn As SqlConnection
    Private Comm As SqlCommand
    Private DA As SqlDataAdapter
    Private DS As New DataSet
    Private TheUser As user
    'constants for resizing form
    Const SizeWithNoHistory As Integer = 232
    Const SizeWithHistory As Integer = 544
    Const FormWidth As Integer = 832

    Private Sub frmActivityComments_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'set header fields
        tbSSN.Text = SSN
        tbName.Text = BorName
        'resize form 
        Me.Width = FormWidth
        Me.Height = SizeWithNoHistory
        'set up DB connection
        If TestMode Then
            'if in test mode
            Conn = New SqlClient.SqlConnection("Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
            DA = New SqlDataAdapter("SELECT ActivitySeq, ActivityDate, UserId, ActivityText FROM ActivityDat WHERE SSN = '" + SSN + "'", "Data Source=OPSDEV;Initial Catalog=TLP;Integrated Security=SSPI;")
        Else
            'if in live mode
            Conn = New SqlClient.SqlConnection("Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
            DA = New SqlDataAdapter("SELECT ActivitySeq, ActivityDate, UserId, ActivityText FROM ActivityDat WHERE SSN = '" + SSN + "'", "Data Source=NOCHOUSE;Initial Catalog=TLP;Integrated Security=SSPI;")
        End If
        Comm = New SqlCommand
        Comm.Connection = Conn
        'if access level = 4 then don't allow to add comments, only read them
        If TheUser.GetAccessLevel() = 4 Then
            btnSave.Enabled = False
            tbComments.Enabled = False
        End If
    End Sub

    Private Sub btnHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHistory.Click
        If Me.Height = SizeWithNoHistory Then
            btnHistory.Text = "Hide History"
            Me.Height = SizeWithHistory

            'set up data grid
            dgAH.TableStyles.Clear()
            dgAH.TableStyles.Add(MakeTableStyle())
            'get data from DB for datagrid
            DS.Clear()
            DA.Fill(DS, "History") 'select statement and connection was set up in the form load event
            'put data from DB into datagrid
            dgAH.DataSource = DS.Tables("History")
        Else
            btnHistory.Text = "Show History"
            Me.Height = SizeWithNoHistory
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If tbComments.TextLength = 0 Then
            MessageBox.Show("You haven't provided a comment yet.  Please provide one, then save it to history.", "No Comment Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        Else
            Comm.Connection.Open()
            Comm.CommandText = "INSERT INTO ActivityDat (SSN, ActivitySeq, UserID, ActivityText) VALUES ('" + SSN + "', " + borrower.NextActivitySeqNum(TestMode, SSN).ToString + ", '" + TheUser.GetUID + "', '" + tbComments.Text.Replace("'", "''") + "')"
            Comm.ExecuteNonQuery()
            Comm.Connection.Close()
            tbComments.Clear()
            MessageBox.Show("Activity history has been updated.", "History Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
            dgAH.TableStyles.Clear()
            dgAH.TableStyles.Add(MakeTableStyle())
            'get data from DB for datagrid
            DS.Clear()
            DA.Fill(DS, "History") 'select statement and connection was set up in the form load event
            'put data from DB into datagrid
            dgAH.DataSource = DS.Tables("History")
        End If
    End Sub

    'creates table style for data grid
    Function MakeTableStyle() As DataGridTableStyle
        Dim dgTableStyle As New DataGridTableStyle
        dgTableStyle.MappingName = "History"
        dgTableStyle.PreferredRowHeight = 12
        'ActivitySeq, ActivityDate, UserId, ActivityText
        Dim column1 As New MultiLineColumn
        With column1
            .MappingName = "ActivitySeq"
            .HeaderText = "Seq Num"
            .Width = 58
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        dgTableStyle.GridColumnStyles.Add(column1)
        Dim column2 As New MultiLineColumn
        With column2
            .MappingName = "ActivityDate"
            .HeaderText = "Activity Date"
            .Width = 150
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        dgTableStyle.GridColumnStyles.Add(column2)
        Dim column3 As New MultiLineColumn
        With column3
            .MappingName = "UserId"
            .HeaderText = "User ID"
            .Width = 100
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        dgTableStyle.GridColumnStyles.Add(column3)
        Dim column4 As New MultiLineColumn
        With column4
            .MappingName = "ActivityText"
            .HeaderText = "Comment"
            .Width = 500
            .TextBox.ForeColor = Me.ForeColor
            .TextBox.BackColor = Me.BackColor
            .ReadOnly = True
        End With
        dgTableStyle.GridColumnStyles.Add(column4)
        dgTableStyle.RowHeadersVisible = False
        dgTableStyle.ForeColor = Me.ForeColor
        dgTableStyle.BackColor = Me.BackColor
        dgTableStyle.HeaderBackColor = Me.ForeColor
        dgTableStyle.HeaderForeColor = Me.BackColor
        dgTableStyle.GridLineColor = Me.ForeColor
        Return dgTableStyle
    End Function

End Class

Public Class MultiLineColumn
    Inherits DataGridTextBoxColumn
    Private mTxtAlign As HorizontalAlignment
    Private mDrawTxt As New StringFormat
    Private mbAdjustHeight As Boolean = True
    Private m_intPreEditHeight As Integer
    Private m_rownum As Integer
    Dim WithEvents dg As DataGrid
    Private arHeights As ArrayList

    Private Sub GetHeightList()
        Dim mi As MethodInfo = dg.GetType().GetMethod("get_DataGridRows", _
        BindingFlags.FlattenHierarchy Or BindingFlags.IgnoreCase Or _
        BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public Or _
        BindingFlags.Static)
        Dim dgra As Array = CType(mi.Invoke(Me.dg, Nothing), Array)
        arHeights = New ArrayList
        Dim dgRowHeight As Object
        For Each dgRowHeight In dgra
            If dgRowHeight.ToString().EndsWith("DataGridRelationshipRow") = True Then
                arHeights.Add(dgRowHeight)
            End If
        Next
    End Sub

    Public Sub New()
        mTxtAlign = HorizontalAlignment.Left
        mDrawTxt.Alignment = StringAlignment.Near
        Me.ReadOnly = True
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal bounds _
As System.Drawing.Rectangle, ByVal [readOnly] As Boolean, ByVal instantText _
As String, ByVal cellIsVisible As Boolean)
        MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
        Me.TextBox.TextAlign = mTxtAlign
        Me.TextBox.Multiline = mbAdjustHeight
        If rowNum = source.Count - 1 Then
            GetHeightList()
        End If
    End Sub

    Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As System.Drawing.Brush, ByVal foreBrush As System.Drawing.Brush, ByVal alignToRight As Boolean)
        Static bPainted As Boolean = False

        If Not bPainted Then
            dg = Me.DataGridTableStyle.DataGrid
            GetHeightList()
        End If
        'clear the cell 
        g.FillRectangle(backBrush, bounds)
        'draw the value 
        Dim s As String = Me.GetColumnValueAtRow([source], rowNum).ToString()
        Dim r As New RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height)
        r.Inflate(0, -1)
        ' get the height column should be 
        Dim sDraw As SizeF = g.MeasureString(s, Me.TextBox.Font, Me.Width, mDrawTxt)
        Dim h As Integer = sDraw.Height + 15
        If mbAdjustHeight Then
            Try
                Dim pi As PropertyInfo = arHeights(rowNum).GetType().GetProperty("Height")
                ' get current height 
                Dim curHeight As Integer = pi.GetValue(arHeights(rowNum), Nothing)
                ' adjust height 
                If h > curHeight Then
                    pi.SetValue(arHeights(rowNum), h, Nothing)
                    Dim sz As Size = dg.Size
                    dg.Size = New Size(sz.Width - 1, sz.Height - 1)
                    dg.Size = sz
                End If
            Catch
                ' something wrong leave default height 
                GetHeightList()
            End Try
        End If
        g.DrawString(s, MyBase.TextBox.Font, foreBrush, r, mDrawTxt)
        bPainted = True

    End Sub
    Private Property DataAlignment() As HorizontalAlignment
        Get
            Return mTxtAlign
        End Get
        Set(ByVal Value As HorizontalAlignment)
            mTxtAlign = Value
            If mTxtAlign = HorizontalAlignment.Center Then
                mDrawTxt.Alignment = StringAlignment.Center
            ElseIf mTxtAlign = HorizontalAlignment.Right Then
                mDrawTxt.Alignment = StringAlignment.Far
            Else
                mDrawTxt.Alignment = StringAlignment.Near
            End If
        End Set
    End Property
    Private Property AutoAdjustHeight() As Boolean
        Get
            Return mbAdjustHeight
        End Get
        Set(ByVal Value As Boolean)
            mbAdjustHeight = Value
            Try
                dg.Invalidate()
            Catch
            End Try
        End Set
    End Property

End Class
