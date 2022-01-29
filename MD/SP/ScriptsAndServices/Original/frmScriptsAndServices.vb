Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.Remoting
Imports Reflection
Imports Q


Public Class frmScriptsAndServices
    Inherits System.Windows.Forms.Form
    'Dim Queue As String
    'Dim subQueue As String
    'Dim ACPSelection As String
    Dim AddVer As Boolean
    Dim PhoneVer As Boolean
    Dim EMailVer As Boolean
    Dim Bor As SP.Borrower

    Const pcDir As String = "C:\Enterprise Program Files\Nexus\"
    Const testNetworkDir As String = "X:\PADU\UHEAACodeBase\"
    Const liveNetworkDir As String = "X:\Sessions\UHEAA Codebase\"

#Region " Windows Form Designer generated code "


    Public Sub New(ByVal TempSSN As String, ByRef TempNoteDude As SP.frmNoteDUDE, ByRef TempLV As ListView, ByVal TempDaysDql As String, ByVal TempDueDate As String, ByRef tBor As SP.Borrower, ByVal tAddVer As Boolean, ByVal tPhoneVer As Boolean, ByVal tEMailVer As Boolean, ByVal TempHPTitleText As String, ByVal tGatherScriptsOptionsFor As String, Optional ByVal IneligibleForCheckByPhone As Boolean = False)
        MyBase.New()
        'Queue = tQueue
        'subQueue = tSubQueue
        'ACPSelection = tACPSelection
        AddVer = tAddVer
        PhoneVer = tPhoneVer
        EMailVer = tEMailVer
        Bor = tBor

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        GatherScriptsOptionsFor = tGatherScriptsOptionsFor
        DueDate = TempDueDate
        Panel1.BackColor = Me.BackColor
        Panel1.ForeColor = Me.ForeColor
        SSN = TempSSN 'init SSN
        NoteDude = TempNoteDude 'init Ref to Note DUDE
        HomePageLV = TempLV
        HPTitleText = TempHPTitleText
        Me.IneligibleForCheckByPhone = IneligibleForCheckByPhone
        If TempDaysDql.Length = 0 Then
            DaysDql = 0
        Else
            DaysDql = CInt(TempDaysDql)
        End If
        ReQ = New frmReQueue(SSN)
        LibForbReq = New ForbReq.frmForbReq(LibForbReqHist, DaysDql, SP.Q.RIBM, SSN, HPTitleText)
        ChgDD = New frmChangeDueDate(Bor)
        DBThread = New Threading.Thread(AddressOf DataInit)
        DBThread.IsBackground = True
        DBThread.Start()
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
    Friend WithEvents TV As System.Windows.Forms.TreeView
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents LV As System.Windows.Forms.ListView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents IL As System.Windows.Forms.ImageList
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDeleteCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DATest As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents ConnTest As System.Data.SqlClient.SqlConnection
    Friend WithEvents ConnLive As System.Data.SqlClient.SqlConnection
    Friend WithEvents DALive As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlSelectCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents lblNotAv As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmScriptsAndServices))
        Me.TV = New System.Windows.Forms.TreeView
        Me.IL = New System.Windows.Forms.ImageList(Me.components)
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.LV = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.btnCloseWindow = New System.Windows.Forms.Button
        Me.DATest = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlDeleteCommand1 = New System.Data.SqlClient.SqlCommand
        Me.ConnTest = New System.Data.SqlClient.SqlConnection
        Me.SqlInsertCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand1 = New System.Data.SqlClient.SqlCommand
        Me.ConnLive = New System.Data.SqlClient.SqlConnection
        Me.DALive = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlInsertCommand2 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand2 = New System.Data.SqlClient.SqlCommand
        Me.lblNotAv = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TV
        '
        Me.TV.Dock = System.Windows.Forms.DockStyle.Left
        Me.TV.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TV.ImageIndex = 0
        Me.TV.ImageList = Me.IL
        Me.TV.Location = New System.Drawing.Point(0, 0)
        Me.TV.Name = "TV"
        Me.TV.PathSeparator = ","
        Me.TV.SelectedImageIndex = 1
        Me.TV.Size = New System.Drawing.Size(184, 413)
        Me.TV.TabIndex = 0
        '
        'IL
        '
        Me.IL.ImageStream = CType(resources.GetObject("IL.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.IL.TransparentColor = System.Drawing.Color.Transparent
        Me.IL.Images.SetKeyName(0, "")
        Me.IL.Images.SetKeyName(1, "")
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(184, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(1, 413)
        Me.Splitter1.TabIndex = 1
        Me.Splitter1.TabStop = False
        '
        'LV
        '
        Me.LV.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.LV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LV.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LV.FullRowSelect = True
        Me.LV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.LV.Location = New System.Drawing.Point(185, 0)
        Me.LV.MultiSelect = False
        Me.LV.Name = "LV"
        Me.LV.Size = New System.Drawing.Size(431, 413)
        Me.LV.TabIndex = 2
        Me.LV.UseCompatibleStateImageBehavior = False
        Me.LV.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 243
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.btnCloseWindow)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel1.Location = New System.Drawing.Point(424, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(192, 413)
        Me.Panel1.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 23)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Scripts && Services"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(8, 96)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(176, 176)
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Location = New System.Drawing.Point(56, 288)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(80, 24)
        Me.btnCloseWindow.TabIndex = 0
        Me.btnCloseWindow.Text = "Close"
        '
        'DATest
        '
        Me.DATest.DeleteCommand = Me.SqlDeleteCommand1
        Me.DATest.InsertCommand = Me.SqlInsertCommand1
        Me.DATest.SelectCommand = Me.SqlSelectCommand1
        Me.DATest.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "HomePageScriptsAndServices", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Expr1", "Expr1"), New System.Data.Common.DataColumnMapping("ScriptID", "ScriptID"), New System.Data.Common.DataColumnMapping("InternalOrExternal", "InternalOrExternal"), New System.Data.Common.DataColumnMapping("Category", "Category"), New System.Data.Common.DataColumnMapping("SubCategory1", "SubCategory1"), New System.Data.Common.DataColumnMapping("SubCategory2", "SubCategory2"), New System.Data.Common.DataColumnMapping("SubCategory3", "SubCategory3"), New System.Data.Common.DataColumnMapping("DisplayName", "DisplayName"), New System.Data.Common.DataColumnMapping("SubToBeCalled", "SubToBeCalled"), New System.Data.Common.DataColumnMapping("ToBeCalledImm", "ToBeCalledImm"), New System.Data.Common.DataColumnMapping("ToBeCalledAtEnd", "ToBeCalledAtEnd"), New System.Data.Common.DataColumnMapping("HomePage", "HomePage"), New System.Data.Common.DataColumnMapping("DataForFunctionCall", "DataForFunctionCall"), New System.Data.Common.DataColumnMapping("CallForNoteDUDECleanUp", "CallForNoteDUDECleanUp")})})
        Me.DATest.UpdateCommand = Me.SqlUpdateCommand1
        '
        'SqlDeleteCommand1
        '
        Me.SqlDeleteCommand1.CommandText = resources.GetString("SqlDeleteCommand1.CommandText")
        Me.SqlDeleteCommand1.Connection = Me.ConnTest
        Me.SqlDeleteCommand1.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_ScriptID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ScriptID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_HomePage", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "HomePage", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CallForNoteDUDECleanUp", System.Data.SqlDbType.NVarChar, 5, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CallForNoteDUDECleanUp", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Category", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Category", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DataForFunctionCall", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DataForFunctionCall", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DisplayName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DisplayName", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Expr1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "HomePage", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_InternalOrExternal", System.Data.SqlDbType.NVarChar, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "InternalOrExternal", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SubCategory1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SubCategory1", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SubCategory2", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SubCategory2", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SubCategory3", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SubCategory3", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SubToBeCalled", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SubToBeCalled", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ToBeCalledAtEnd", System.Data.SqlDbType.NVarChar, 5, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ToBeCalledAtEnd", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ToBeCalledImm", System.Data.SqlDbType.NVarChar, 5, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ToBeCalledImm", System.Data.DataRowVersion.Original, Nothing)})
        '
        'ConnTest
        '
        Me.ConnTest.ConnectionString = "Data Source=OPSDEV;Initial Catalog=MauiDUDE;Integrated Security=SSPI;"
        Me.ConnTest.FireInfoMessageEventOnUserErrors = False
        '
        'SqlInsertCommand1
        '
        Me.SqlInsertCommand1.CommandText = resources.GetString("SqlInsertCommand1.CommandText")
        Me.SqlInsertCommand1.Connection = Me.ConnTest
        Me.SqlInsertCommand1.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@HomePage", System.Data.SqlDbType.NVarChar, 50, "HomePage"), New System.Data.SqlClient.SqlParameter("@InternalOrExternal", System.Data.SqlDbType.NVarChar, 8, "InternalOrExternal"), New System.Data.SqlClient.SqlParameter("@Category", System.Data.SqlDbType.NVarChar, 50, "Category"), New System.Data.SqlClient.SqlParameter("@SubCategory1", System.Data.SqlDbType.NVarChar, 50, "SubCategory1"), New System.Data.SqlClient.SqlParameter("@SubCategory2", System.Data.SqlDbType.NVarChar, 50, "SubCategory2"), New System.Data.SqlClient.SqlParameter("@SubCategory3", System.Data.SqlDbType.NVarChar, 50, "SubCategory3"), New System.Data.SqlClient.SqlParameter("@DisplayName", System.Data.SqlDbType.NVarChar, 50, "DisplayName"), New System.Data.SqlClient.SqlParameter("@SubToBeCalled", System.Data.SqlDbType.NVarChar, 100, "SubToBeCalled"), New System.Data.SqlClient.SqlParameter("@ToBeCalledImm", System.Data.SqlDbType.NVarChar, 5, "ToBeCalledImm"), New System.Data.SqlClient.SqlParameter("@ToBeCalledAtEnd", System.Data.SqlDbType.NVarChar, 5, "ToBeCalledAtEnd"), New System.Data.SqlClient.SqlParameter("@DataForFunctionCall", System.Data.SqlDbType.NVarChar, 50, "DataForFunctionCall"), New System.Data.SqlClient.SqlParameter("@CallForNoteDUDECleanUp", System.Data.SqlDbType.NVarChar, 5, "CallForNoteDUDECleanUp")})
        '
        'SqlSelectCommand1
        '
        Me.SqlSelectCommand1.CommandText = resources.GetString("SqlSelectCommand1.CommandText")
        Me.SqlSelectCommand1.Connection = Me.ConnTest
        Me.SqlSelectCommand1.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@HP", System.Data.SqlDbType.NVarChar, 50, "Expr1")})
        '
        'SqlUpdateCommand1
        '
        Me.SqlUpdateCommand1.CommandText = resources.GetString("SqlUpdateCommand1.CommandText")
        Me.SqlUpdateCommand1.Connection = Me.ConnTest
        Me.SqlUpdateCommand1.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@HomePage", System.Data.SqlDbType.NVarChar, 50, "HomePage"), New System.Data.SqlClient.SqlParameter("@InternalOrExternal", System.Data.SqlDbType.NVarChar, 8, "InternalOrExternal"), New System.Data.SqlClient.SqlParameter("@Category", System.Data.SqlDbType.NVarChar, 50, "Category"), New System.Data.SqlClient.SqlParameter("@SubCategory1", System.Data.SqlDbType.NVarChar, 50, "SubCategory1"), New System.Data.SqlClient.SqlParameter("@SubCategory2", System.Data.SqlDbType.NVarChar, 50, "SubCategory2"), New System.Data.SqlClient.SqlParameter("@SubCategory3", System.Data.SqlDbType.NVarChar, 50, "SubCategory3"), New System.Data.SqlClient.SqlParameter("@DisplayName", System.Data.SqlDbType.NVarChar, 50, "DisplayName"), New System.Data.SqlClient.SqlParameter("@SubToBeCalled", System.Data.SqlDbType.NVarChar, 100, "SubToBeCalled"), New System.Data.SqlClient.SqlParameter("@ToBeCalledImm", System.Data.SqlDbType.NVarChar, 5, "ToBeCalledImm"), New System.Data.SqlClient.SqlParameter("@ToBeCalledAtEnd", System.Data.SqlDbType.NVarChar, 5, "ToBeCalledAtEnd"), New System.Data.SqlClient.SqlParameter("@DataForFunctionCall", System.Data.SqlDbType.NVarChar, 50, "DataForFunctionCall"), New System.Data.SqlClient.SqlParameter("@CallForNoteDUDECleanUp", System.Data.SqlDbType.NVarChar, 5, "CallForNoteDUDECleanUp"), New System.Data.SqlClient.SqlParameter("@Original_ScriptID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ScriptID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_HomePage", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "HomePage", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CallForNoteDUDECleanUp", System.Data.SqlDbType.NVarChar, 5, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CallForNoteDUDECleanUp", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Category", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Category", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DataForFunctionCall", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DataForFunctionCall", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DisplayName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DisplayName", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Expr1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "HomePage", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_InternalOrExternal", System.Data.SqlDbType.NVarChar, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "InternalOrExternal", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SubCategory1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SubCategory1", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SubCategory2", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SubCategory2", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SubCategory3", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SubCategory3", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SubToBeCalled", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SubToBeCalled", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ToBeCalledAtEnd", System.Data.SqlDbType.NVarChar, 5, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ToBeCalledAtEnd", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ToBeCalledImm", System.Data.SqlDbType.NVarChar, 5, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ToBeCalledImm", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@ScriptID", System.Data.SqlDbType.Int, 4, "ScriptID")})
        '
        'ConnLive
        '
        Me.ConnLive.ConnectionString = "workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=N" & _
            "OCHOUSE;persist security info=False;initial catalog=MauiDUDE"
        Me.ConnLive.FireInfoMessageEventOnUserErrors = False
        '
        'DALive
        '
        Me.DALive.InsertCommand = Me.SqlInsertCommand2
        Me.DALive.SelectCommand = Me.SqlSelectCommand2
        Me.DALive.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "HomePageScriptsAndServices", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Expr1", "Expr1"), New System.Data.Common.DataColumnMapping("ScriptID", "ScriptID"), New System.Data.Common.DataColumnMapping("InternalOrExternal", "InternalOrExternal"), New System.Data.Common.DataColumnMapping("Category", "Category"), New System.Data.Common.DataColumnMapping("SubCategory1", "SubCategory1"), New System.Data.Common.DataColumnMapping("SubCategory2", "SubCategory2"), New System.Data.Common.DataColumnMapping("SubCategory3", "SubCategory3"), New System.Data.Common.DataColumnMapping("DisplayName", "DisplayName"), New System.Data.Common.DataColumnMapping("SubToBeCalled", "SubToBeCalled"), New System.Data.Common.DataColumnMapping("ToBeCalledImm", "ToBeCalledImm"), New System.Data.Common.DataColumnMapping("ToBeCalledAtEnd", "ToBeCalledAtEnd"), New System.Data.Common.DataColumnMapping("HomePage", "HomePage"), New System.Data.Common.DataColumnMapping("DataForFunctionCall", "DataForFunctionCall"), New System.Data.Common.DataColumnMapping("CallForNoteDUDECleanUp", "CallForNoteDUDECleanUp")})})
        '
        'SqlInsertCommand2
        '
        Me.SqlInsertCommand2.CommandText = resources.GetString("SqlInsertCommand2.CommandText")
        Me.SqlInsertCommand2.Connection = Me.ConnLive
        Me.SqlInsertCommand2.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@HomePage", System.Data.SqlDbType.NVarChar, 50, "HomePage"), New System.Data.SqlClient.SqlParameter("@ScriptID", System.Data.SqlDbType.Int, 4, "ScriptID"), New System.Data.SqlClient.SqlParameter("@InternalOrExternal", System.Data.SqlDbType.NVarChar, 8, "InternalOrExternal"), New System.Data.SqlClient.SqlParameter("@Category", System.Data.SqlDbType.NVarChar, 50, "Category"), New System.Data.SqlClient.SqlParameter("@SubCategory1", System.Data.SqlDbType.NVarChar, 50, "SubCategory1"), New System.Data.SqlClient.SqlParameter("@SubCategory2", System.Data.SqlDbType.NVarChar, 50, "SubCategory2"), New System.Data.SqlClient.SqlParameter("@SubCategory3", System.Data.SqlDbType.NVarChar, 50, "SubCategory3"), New System.Data.SqlClient.SqlParameter("@DisplayName", System.Data.SqlDbType.NVarChar, 50, "DisplayName"), New System.Data.SqlClient.SqlParameter("@SubToBeCalled", System.Data.SqlDbType.NVarChar, 100, "SubToBeCalled"), New System.Data.SqlClient.SqlParameter("@ToBeCalledImm", System.Data.SqlDbType.NVarChar, 5, "ToBeCalledImm"), New System.Data.SqlClient.SqlParameter("@ToBeCalledAtEnd", System.Data.SqlDbType.NVarChar, 5, "ToBeCalledAtEnd"), New System.Data.SqlClient.SqlParameter("@DataForFunctionCall", System.Data.SqlDbType.NVarChar, 50, "DataForFunctionCall"), New System.Data.SqlClient.SqlParameter("@CallForNoteDUDECleanUp", System.Data.SqlDbType.NVarChar, 5, "CallForNoteDUDECleanUp")})
        '
        'SqlSelectCommand2
        '
        Me.SqlSelectCommand2.CommandText = resources.GetString("SqlSelectCommand2.CommandText")
        Me.SqlSelectCommand2.Connection = Me.ConnLive
        Me.SqlSelectCommand2.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@HP", System.Data.SqlDbType.NVarChar, 50, "Expr1")})
        '
        'lblNotAv
        '
        Me.lblNotAv.BackColor = System.Drawing.Color.Red
        Me.lblNotAv.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNotAv.ForeColor = System.Drawing.Color.Black
        Me.lblNotAv.Location = New System.Drawing.Point(0, 160)
        Me.lblNotAv.Name = "lblNotAv"
        Me.lblNotAv.Size = New System.Drawing.Size(616, 72)
        Me.lblNotAv.TabIndex = 5
        Me.lblNotAv.Text = "Scripts and Services is currently unavailable.       Please contact Systems Suppo" & _
            "rt."
        Me.lblNotAv.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmScriptsAndServices
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(616, 413)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblNotAv)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.LV)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.TV)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(624, 440)
        Me.MinimumSize = New System.Drawing.Size(624, 440)
        Me.Name = "frmScriptsAndServices"
        Me.ShowInTaskbar = False
        Me.Text = "Scripts And Services"
        Me.Panel1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private VerbalForbPerf As Boolean = False
    Private ChkByPhonePerf As Boolean = False
    Private SSN As String
    Private SAndSInfo As New ArrayList
    Private NoteDude As SP.frmNoteDUDE
    Private CheckByPhoneNotes As String
    Private DSGeneral As New DataSet
    Private SelectedSandSIDs As New ArrayList
    Private HomePageLV As ListView
    Private ReQ As frmReQueue
    Private DA As SqlClient.SqlDataAdapter
    Private Conn As SqlClient.SqlConnection
    Private DueDate As String
    Private ChgDD As frmChangeDueDate
    Private HPTitleText As String
    Private DBThread As Threading.Thread
    Private DataCollectionSuccessful As Boolean
    Private IneligibleForCheckByPhone As Boolean
    Private GatherScriptsOptionsFor As String
    Declare Function OpenIcon Lib "user32" (ByVal hwnd As Long) As Long
    Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long

    'Plug-In vars
    Private LibForbReqHist As New frmScriptsAndServicesDeferForbHistory
    Private DaysDql As Integer
    Private LibForbReq As ForbReq.frmForbReq

    Public Overloads Sub Show()
        If DataCollectionSuccessful = False Then
            lblNotAv.Visible = True
        Else
            lblNotAv.Visible = False
        End If
        MyBase.Show()
    End Sub

    'this sub is threaded so the BS homepage can be used even if the DB is down
    Private Sub DataInit()

        'check for test mode
        If SP.TestMode() Then
            DA = DATest
            Conn = ConnTest
        Else
            DA = DALive
            Conn = ConnLive
        End If

        DA.SelectCommand.Parameters("@HP").Value = GatherScriptsOptionsFor     'set parameter for data adapter

        Try
            DA.Fill(DSGeneral, "Scripts")         'fill data adapter
            CreateTreeViewStructure()         'set up tree view
            DataCollectionSuccessful = True       'connection to DB was successful
        Catch ex As Exception
            DataCollectionSuccessful = False          'connection to DB wasn't successful
        End Try
    End Sub

    'this sub creates the tree view structure
    Private Sub CreateTreeViewStructure()
        Dim Comm As New SqlCommand
        Dim Reader As SqlDataReader
        Dim I As Integer
        Dim I1 As Integer
        Dim I2 As Integer
        Dim JMIx As TreeNode
        'gather information from the DB
        Conn.Open()
        Comm.Connection = Conn
        'Create tree view structure
        'get root categories
        Comm.CommandText = "SELECT Distinct Category FROM HomePageScriptsAndServices"
        Reader = Comm.ExecuteReader
        While Reader.Read
            JMIx = TV.Nodes.Add(Reader.Item("Category"))
            JMIx.NodeFont = New Font(TV.Font.FontFamily, 12)
        End While
        Reader.Close()
        'get sub categories #1 for each root node
        I = 0
        While I < TV.GetNodeCount(False)
            Comm.CommandText = "SELECT Distinct SubCategory1, Category FROM HomePageScriptsAndServices WHERE Category = '" & TV.Nodes(I).Text & "'"
            Reader = Comm.ExecuteReader
            While Reader.Read
                If Reader.Item("SubCategory1") <> "Nothing" Then
                    JMIx = TV.Nodes(I).Nodes.Add(Reader.Item("SubCategory1"))
                    JMIx.NodeFont = New Font(TV.Font.FontFamily, 10)
                End If
            End While
            Reader.Close()
            I = I + 1
        End While
        'get sub categories #2 for each root node
        I = 0
        While I < TV.GetNodeCount(False)
            I1 = 0
            While I1 < TV.Nodes(I).GetNodeCount(False)
                Comm.CommandText = "SELECT Distinct SubCategory2, SubCategory1, Category FROM HomePageScriptsAndServices WHERE Category = '" & TV.Nodes(I).Text & "' AND SubCategory1 = '" & TV.Nodes(I).Nodes(I1).Text & "'"
                Reader = Comm.ExecuteReader
                While Reader.Read
                    If Reader.Item("SubCategory2") <> "Nothing" Then
                        JMIx = TV.Nodes(I).Nodes(I1).Nodes.Add(Reader.Item("SubCategory2"))
                        JMIx.NodeFont = New Font(TV.Font.FontFamily, 8)
                    End If
                End While
                Reader.Close()
                I1 = I1 + 1
            End While
            I = I + 1
        End While
        'get sub categories #3 for each root node
        I = 0
        While I < TV.GetNodeCount(False)
            I1 = 0
            While I1 < TV.Nodes(I).GetNodeCount(False)
                I2 = 0
                While I2 < TV.Nodes(I).Nodes(I1).GetNodeCount(False)
                    Comm.CommandText = "SELECT Distinct SubCategory3, SubCategory2, SubCategory1, Category FROM HomePageScriptsAndServices WHERE Category = '" & TV.Nodes(I).Text & "' AND SubCategory1 = '" & TV.Nodes(I).Nodes(I1).Text & "' AND SubCategory2 = '" & TV.Nodes(I).Nodes(I1).Nodes(I2).Text & "'"
                    Reader = Comm.ExecuteReader
                    While Reader.Read
                        If Reader.Item("SubCategory3") <> "Nothing" Then
                            JMIx = TV.Nodes(I).Nodes(I1).Nodes(I2).Nodes.Add(Reader.Item("SubCategory3"))
                            JMIx.NodeFont = New Font(TV.Font.FontFamily, 6)
                        End If
                    End While
                    Reader.Close()
                    I2 = I2 + 1
                End While
                I1 = I1 + 1
            End While
            I = I + 1
        End While
        Conn.Close()
        I = 0
        While I < TV.GetNodeCount(False)
            TV.Nodes(I).ExpandAll()
            I += 1
        End While
    End Sub

    Private Sub TV_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TV.AfterSelect
        Dim TempA() As String
        Dim LIdex As Integer
        Dim DRs() As DataRow
        Dim I As Integer
        TempA = Split(TV.SelectedNode.FullPath, ",")
        If UBound(TempA) = 3 Then
            DRs = DSGeneral.Tables("Scripts").Select("Category = '" & TempA(0) & "' AND SubCategory1 = '" & TempA(1) & "' AND SubCategory2 = '" & TempA(2) & "' AND SubCategory3 = '" & TempA(3) & "'")
        ElseIf UBound(TempA) = 2 Then
            DRs = DSGeneral.Tables("Scripts").Select("Category = '" & TempA(0) & "' AND SubCategory1 = '" & TempA(1) & "' AND SubCategory2 = '" & TempA(2) & "' AND SubCategory3 = 'Nothing'")
        ElseIf UBound(TempA) = 1 Then
            DRs = DSGeneral.Tables("Scripts").Select("Category = '" & TempA(0) & "' AND SubCategory1 = '" & TempA(1) & "' AND SubCategory2 = 'Nothing' AND SubCategory3 = 'Nothing'")
        Else
            DRs = DSGeneral.Tables("Scripts").Select("Category = '" & TempA(0) & "' AND SubCategory1 = 'Nothing' AND SubCategory2 = 'Nothing' AND SubCategory3 = 'Nothing'")
        End If
        LV.Items.Clear()
        While I < (DRs.GetUpperBound(0) + 1)
            'display display name
            If Not (IneligibleForCheckByPhone And DRs(I).Item("DisplayName") = "Check By Phone") Then
                LIdex = LV.Items.Add(DRs(I).Item("DisplayName")).Index              'display name
                LV.Items(LIdex).SubItems.Add(CStr(DRs(I).Item("ScriptID")))
            End If
            I = I + 1
        End While
    End Sub

    Private Sub LV_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles LV.DoubleClick
        'Run the script identified by the selected item's ScriptID, stored in SubItem(1).
        RunScript(LV.SelectedItems(0).SubItems(1).Text)
    End Sub

    'The public RunScript subroutine can be called by any code to run a script or queue a letter, based on the ScriptID in the database.
    'The calling program will need to look up the appropriate ScriptID itself. See MDBSContact.frmContact.StartService() for an example.
    Public Sub RunScript(ByVal ScriptID As Integer)
        Dim DRs() As DataRow
        Dim I As Integer
        Dim LIdex As Integer
        Dim CLS() As String
        CLS = Split(Replace(SP.Q.RIBM.CommandLineSwitches, """", ""), "\")
        'check if the script has already been selected 
        If SelectedSandSIDs.Contains(ScriptID) Then
            SP.frmWhoaDUDE.WhoaDUDE("Whoa Dude, that script has already been selected.  You cannot select it again.", "Already Selected", True)
            Exit Sub
        End If
        'if the script hasn't been selected yet
        SelectedSandSIDs.Add(ScriptID)        'add to master list
        'look up other applicable information and store in array
        DRs = DSGeneral.Tables("Scripts").Select("ScriptID = " & ScriptID)
        If DRs(0).Item("ToBeCalledImm") = "True" Then
            If GatherScriptsOptionsFor = "Borrower Services" Then
                'only do ACP functionality if scripts and services is for borrower services
                '********************
                'Must be on ACP to run scripts
                If Not (Bor.BorLite.Queue = Nothing And Bor.BorLite.SubQueue = Nothing And Bor.BorLite.ACPSelection = Nothing) Then
                    If SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False Then
                        ReturnToACP(Bor.BorLite.Queue, Bor.BorLite.SubQueue, Bor.BorLite.ACPSelection)
                    End If
                End If
            End If
            '********************

            'is the script or service code in Maui DUDE or in the open Reflection session
            If DRs(0).Item("InternalOrExternal") = "External" Then    'Code is outside Maui DUDE
                LIdex = HomePageLV.Items.Add(DRs(0).Item("DisplayName")).index
                'check if script needs to be ran now or later 
                If DRs(0).Item("ToBeCalledImm") = "True" And DRs(0).Item("ToBeCalledAtEnd") = "True" Then
                    HomePageLV.Items(LIdex).SubItems.Add("Partially Complete")
                    Bor.Scripts = Bor.Scripts & DRs(0).Item("DisplayName") & " --Partially Complete" & vbCrLf       '<1502>
                ElseIf DRs(I).Item("ToBeCalledImm") = "True" Then
                    HomePageLV.Items(LIdex).SubItems.Add("Complete")
                    Bor.Scripts = Bor.Scripts & DRs(0).Item("DisplayName") & " --Complete" & vbCrLf
                End If
                HomePageLV.Items(LIdex).SubItems.Add(DRs(0).Item("ScriptID"))
                'bring reflection session to the top of all windows
                LActivatePrevInstance(CLS(CLS.GetUpperBound(0)))
                SP.Q.RIBM.SwitchToWindow(1)
                'call script
                Try
                    SP.Q.RIBM.RunMacro(DRs(0).Item("SubToBeCalled"), SSN & ",1," & CStr(SP.Q.TestMode()))
                    'mark flag for change due date if cehck by phone
                    If DRs(0).Item("SubToBeCalled") = "SP.OPSChkByPhn.main" Then
                        ChkByPhonePerf = True
                    End If
                    'check for existence of script completion file
                    If DRs(0).Item("CompletionFile") <> "Nothing" Then
                        If File.Exists(DRs(0).Item("CompletionFile")) = False Then
                            'if script completion file doesn't exist then
                            LActivatePrevInstance(HPTitleText)
                            LActivatePrevInstance("Scripts And Services")
                            HomePageLV.Items.RemoveAt(LIdex)
                            SelectedSandSIDs.Remove(ScriptID)
                            SP.frmWhoaDUDE.WhoaDUDE("DUDE detected that either the script was manually cancelled or the script ended abnormally.  In either case DUDE doesn't register that the script was run.", "Holy Maco")
                            Exit Sub
                        Else
                            'delete script completion file if it exists
                            File.Delete(DRs(0).Item("CompletionFile"))
                        End If
                    End If
                Catch ex As Exception
                    LActivatePrevInstance(HPTitleText)
                    LActivatePrevInstance("Scripts And Services")
                    HomePageLV.Items.RemoveAt(LIdex)
                    SelectedSandSIDs.Remove(ScriptID)
                    SP.frmWhoaDUDE.WhoaDUDE("DUDE was unable to find your script." & vbLf & ex.Message, "Holy Maco")
                    'remove info from box and master list if service was cancelled
                    Exit Sub
                End Try

                LActivatePrevInstance(HPTitleText)
            ElseIf DRs(0).Item("InternalOrExternal") = "DLL" Then    'if functionality is in DLL
                If DRs(0).Item("SubToBeCalled") = "ForbReqProc" Then
                    If ForbReqProc() = False Then
                        'if processing completes successfully then update list
                        LIdex = HomePageLV.Items.Add(DRs(0).Item("DisplayName")).index
                        HomePageLV.Items(LIdex).SubItems.Add("Partially Complete")
                        Bor.Scripts = Bor.Scripts & DRs(0).Item("DisplayName") & " --Partially Complete" & vbCrLf       '<1502>
                        HomePageLV.Items(LIdex).SubItems.Add(DRs(0).Item("ScriptID"))
                        VerbalForbPerf = True
                    Else
                        SelectedSandSIDs.Remove(CInt(DRs(0).Item("ScriptID")))
                    End If
                End If
            ElseIf DRs(0).Item("InternalOrExternal") = ".NET DLL" Then
                LIdex = HomePageLV.Items.Add(DRs(0).Item("DisplayName")).index
                'check if script needs to be ran now or later 
                If DRs(0).Item("ToBeCalledImm") = "True" And DRs(0).Item("ToBeCalledAtEnd") = "True" Then
                    HomePageLV.Items(LIdex).SubItems.Add("Partially Complete")
                    Bor.Scripts = Bor.Scripts & DRs(0).Item("DisplayName") & " --Partially Complete" & vbCrLf       '<1502>
                ElseIf DRs(I).Item("ToBeCalledImm") = "True" Then
                    HomePageLV.Items(LIdex).SubItems.Add("Complete")
                    Bor.Scripts = Bor.Scripts & DRs(0).Item("DisplayName") & " --Complete" & vbCrLf
                End If
                HomePageLV.Items(LIdex).SubItems.Add(DRs(0).Item("ScriptID"))
                Bor.ScriptInfoToGenericBusinessUnit.ScriptCompletedSuccessfully = False
                RunDotNETDll(DRs(0), Bor, 1)
                'check if script completed if applicable
                If DRs(0).Item("CompletionFile") <> "Nothing" Then
                    If Bor.ScriptInfoToGenericBusinessUnit.ScriptCompletedSuccessfully = False Then
                        'if script completion file doesn't exist then
                        LActivatePrevInstance(HPTitleText)
                        LActivatePrevInstance("Scripts And Services")
                        HomePageLV.Items.RemoveAt(LIdex)
                        SelectedSandSIDs.Remove(ScriptID)
                        SP.frmWhoaDUDE.WhoaDUDE("DUDE detected that either the script was manually cancelled or the script ended abnormally.  In either case DUDE doesn't register that the script was run.", "Holy Maco")
                        Exit Sub
                    End If
                End If
                LActivatePrevInstance(HPTitleText)
            Else    'if code is inside Maui DUDE
                LIdex = HomePageLV.Items.Add(DRs(0).Item("DisplayName")).index

                'check if script needs to be run now or later 
                If DRs(0).Item("CallForNoteDUDECleanUp") = "True" Then
                    HomePageLV.Items(LIdex).SubItems.Add("Documented in Note DUDE")
                    HomePageLV.Items(LIdex).BackColor = Color.White       'white out if the user can back out of processing
                    HomePageLV.Items(LIdex).ForeColor = Color.Black     'white out if the user can back out of processing
                ElseIf DRs(0).Item("ToBeCalledImm") = "True" And DRs(0).Item("ToBeCalledAtEnd") = "True" Then
                    HomePageLV.Items(LIdex).SubItems.Add("Partially Complete")
                    Bor.Scripts = Bor.Scripts & DRs(0).Item("DisplayName") & " --Partially Complete" & vbCrLf       '<1502>
                ElseIf DRs(I).Item("ToBeCalledImm") = "True" Then
                    HomePageLV.Items(LIdex).SubItems.Add("Complete")
                    Bor.Scripts = Bor.Scripts & DRs(0).Item("DisplayName") & " --Complete" & vbCrLf
                End If

                HomePageLV.Items(LIdex).SubItems.Add(DRs(0).Item("ScriptID"))
                'decide what internal Maui DUDE code to run if the service needs to be ran immediately
                If DRs(0).Item("SubToBeCalled") = "CheckByPhone" Then
                    AddCheckByPhoneNotes()
                ElseIf DRs(0).Item("SubToBeCalled") = "ReQueueTask" Then
                    ReQ.ShowDialog()
                    'was the form cancelled
                    If ReQ.InformationGathered() = False Then
                        'remove info from box and master list if service was cancelled
                        HomePageLV.Items.RemoveAt(LIdex)
                        SelectedSandSIDs.Remove(ScriptID)
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Queued to Run" & vbCrLf, "")       '<1502>
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Partially Complete" & vbCrLf, "")       '<1502>
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Complete" & vbCrLf, "")       '<1502>
                    End If
                ElseIf DRs(0).Item("SubToBeCalled") = "ChangeDueDate" Then
                    If DaysDql >= 30 Then
                        If VerbalForbPerf = False And ChkByPhonePerf = False Then
                            SP.frmWhoaDUDE.WhoaDUDE("This borrower is 30 or more days delinquent.  You must run the verbal forbearance or check by phone script before you run this script.", "30 or More Days Delinquent")
                            'remove info from box and master list if service was cancelled
                            HomePageLV.Items.RemoveAt(LIdex)
                            SelectedSandSIDs.Remove(ScriptID)
                            LActivatePrevInstance(HPTitleText)
                            Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Queued to Run" & vbCrLf, "")       '<1502>
                            Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Partially Complete" & vbCrLf, "")       '<1502>
                            Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Complete" & vbCrLf, "")       '<1502>
                            Exit Sub
                        End If
                    End If
                    If DueDate = "" Then
                        SP.frmWhoaDUDE.WhoaDUDE("Borrower doesn't have a due date.", "No Due Date")
                        'remove info from box and master list if service was cancelled
                        HomePageLV.Items.RemoveAt(LIdex)
                        SelectedSandSIDs.Remove(ScriptID)
                        LActivatePrevInstance(HPTitleText)
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Queued to Run" & vbCrLf, "")       '<1502>
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Partially Complete" & vbCrLf, "")       '<1502>
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Complete" & vbCrLf, "")       '<1502>
                        Exit Sub
                    End If
                    If ChgDD.Show(DueDate) = False Then
                        'remove info from box and master list if service was cancelled
                        HomePageLV.Items.RemoveAt(LIdex)
                        SelectedSandSIDs.Remove(ScriptID)
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Queued to Run" & vbCrLf, "")       '<1502>
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Partially Complete" & vbCrLf, "")       '<1502>
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Complete" & vbCrLf, "")       '<1502>
                    End If
                    LActivatePrevInstance(HPTitleText)
                ElseIf DRs(0).Item("SubToBeCalled") = "RePrintBill" Then
                    If RePrintBill() = False Then
                        'remove info from box and master list if service was cancelled
                        HomePageLV.Items.RemoveAt(LIdex)
                        SelectedSandSIDs.Remove(ScriptID)
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Queued to Run" & vbCrLf, "")       '<1502>
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Partially Complete" & vbCrLf, "")       '<1502>
                        Bor.Scripts = Replace(Bor.Scripts, DRs(0).Item("DisplayName") & " --Complete" & vbCrLf, "")       '<1502>
                    End If
                    LActivatePrevInstance(HPTitleText)
                End If
            End If
        Else    'scripts that run later
            LIdex = HomePageLV.Items.Add(DRs(0).Item("DisplayName")).index
            HomePageLV.Items(LIdex).SubItems.Add("Queued to Run")
            Bor.Scripts = Bor.Scripts & DRs(0).Item("DisplayName") & " --Queued to Run" & vbCrLf
            HomePageLV.Items(LIdex).BackColor = Color.White          'white out if the user can back out of processing
            HomePageLV.Items(LIdex).ForeColor = Color.Black           'white out if the user can back out of processing
            HomePageLV.Items(LIdex).SubItems.Add(DRs(0).Item("ScriptID"))
        End If
        Bor.SpillGuts()
    End Sub

    'this function will update the home page list, its own list, and any clean up when a script is removed
    'this function makes the assumption that the script is allowed to be removed
    Public Sub UpdateScriptListData()
        Dim DRs() As DataRow
        'If HomePageLV.Items.Count <> 0 Then
        'look up other applicable information and store in array
        DRs = DSGeneral.Tables("Scripts").Select("ScriptID = " & HomePageLV.SelectedItems(0).SubItems(2).Text)
        'remove from Borrower Class String
        Bor.Scripts = Replace(Bor.Scripts, HomePageLV.SelectedItems(0).Text & " --Queued to Run" & vbCrLf, "")  '<1502>
        'remove from master list
        SelectedSandSIDs.Remove(CInt(HomePageLV.SelectedItems(0).SubItems(2).Text))
        'remove from list view on home page
        HomePageLV.Items.Remove(HomePageLV.SelectedItems(0))
        'check for clean up
        If HomePageLV.Items.Count <> 0 Then
            HomePageLV.Items(HomePageLV.Items.Count - 1).Selected = True
        End If
        If DRs(0).Item("SubToBeCalled") = "CheckByPhone" Then
            'remove notes
            NoteDude.RemoveComment(CheckByPhoneNotes)
        End If
        'Else
        'SelectedSandSIDs.Clear()
        'End If
        Bor.SpillGuts()
    End Sub

    'creates and adds the check by phone notes
    Sub AddCheckByPhoneNotes()
        Dim Amt As String
        'remind user to process check by phone on OPS or with the check by phone script
        SP.frmWhoaDUDE.WhoaDUDE("This service adds the amount of the check by phone to system notes.  It DOES NOT actually perform the check by phone process.  To actually process a check by phone use the OPS web site.", "Notes Only", True)
        'gather amount from user
        Amt = Microsoft.VisualBasic.InputBox("Please enter the amount of the check by phone.", "Check By Phone Amount")
        While Amt.Length = 0 And IsNumeric(Amt) = False
            If Amt.Length = 0 Then
                Amt = Microsoft.VisualBasic.InputBox("You must enter a check by phone dollar amount.", "Check By Phone Amount")
            Else
                Amt = Microsoft.VisualBasic.InputBox("The amount you enter must be numeric.", "Check By Phone Amount")
            End If
        End While
        Amt = Format(CDbl(Amt), "$###,##0.00")
        'hold notes value in a class level variable in case user decides to remove notes
        CheckByPhoneNotes = "CK BY PHN " & Amt
        NoteDude.EnterComment(CheckByPhoneNotes)          'add notes to Note DUDE
    End Sub

    'this sub will run the rest of the partially run scripts and all queued scripts
    Public Function RunRemainingScriptsAndServices() As String
        Dim DRs() As DataRow
        Dim ErrorStr As String = ""
        Dim I As Integer
        Dim ReflectionIsDisplayed As Boolean
        'cycle through all listed scripts and see if they need to be ran now
        While I < (SelectedSandSIDs.Count)
            DRs = DSGeneral.Tables("Scripts").Select("ScriptID = " & SelectedSandSIDs.Item(I))
            'run the script if indicator is true
            If DRs(0).Item("ToBeCalledAtEnd") = "True" Then
                'check if the script is internal or external
                If DRs(0).Item("InternalOrExternal") = "External" Then
                    'only move focus to Reflection for the first iteration
                    If ReflectionIsDisplayed = False Then
                        'bring reflection session to the top of all windows
                        SP.Q.RIBM.SwitchToWindow(1)
                        ReflectionIsDisplayed = True                         'mark indicator so focus isn't moved to Reflection for each iteration
                    End If
                    'call script
                    Try
                        SP.Q.RIBM.RunMacro(DRs(0).Item("SubToBeCalled"), SSN & ",2," & CStr(SP.Q.TestMode()))
                    Catch ex As Exception
                        ErrorStr += DRs(0).Item("DisplayName") & vbLf
                    End Try
                ElseIf DRs(0).Item("InternalOrExternal") = "DLL" Then
                    If DRs(0).Item("SubToBeCalled") = "ForbReqProc" Then
                        LibForbReq.AddActivityComments()
                    End If
                ElseIf DRs(0).Item("InternalOrExternal") = ".NET DLL" Then
                    RunDotNETDll(DRs(0), Bor, 2)
                Else                'internal scripts and services
                    If DRs(0).Item("SubToBeCalled") = "AddLetterARC" Then
                        AddLetterARC(DRs(0).Item("DataForFunctionCall"))
                    ElseIf DRs(0).Item("SubToBeCalled") = "ReQueueTask" Then
                        ReQ.ReQueueTask()
                    ElseIf DRs(0).Item("SubToBeCalled") = "ChangeDueDate" Then
                        ChgDD.WriteActivityCommentOut()
                    End If
                End If
            End If
            I = I + 1
        End While
        Return ErrorStr
    End Function

    'this sub is one of the internal services the user can select.  It adds an ARC record for a letter to be produced later
    Private Sub AddLetterARC(ByVal ARC As String)
        Bor.ActivityCmts.AddCommentsToTD22AllLoans("", ARC)
    End Sub

    Private Sub btnCloseWindow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseWindow.Click
        Me.Hide()
    End Sub

    Sub LActivatePrevInstance(ByVal argStrAppToFind As String)
        Dim PrevHndl As Long
        Dim result As Long
        'Variable to hold individual Process.
        Dim objProcess As New Process
        'Collection of all the Processes running on local machine
        Dim objProcesses() As Process
        'Get all processes into the collection
        objProcesses = Process.GetProcesses()
        For Each objProcess In objProcesses
            'Check and exit if we have SMS running already
            If UCase(objProcess.MainWindowTitle) = UCase(argStrAppToFind) Then
                PrevHndl = objProcess.MainWindowHandle.ToInt32()
                Exit For
            End If
        Next
        'If no previous instance found exit the application.
        If PrevHndl = 0 Then Exit Sub
        'If previous instance found.
        result = OpenIcon(PrevHndl)       'Restore the program.
        result = SetForegroundWindow(PrevHndl)        'Activate the application.
    End Sub

    Private Function ForbReqProc() As Boolean
        Try
            LibForbReq.ShowFrm()
        Catch ex As Exception
            SP.frmWhoaDUDE.WhoaDUDE("An error occured while the Forbearance Request service tried to process.  Please contact Systems Support.", "Forbearance Request Service Error")
            Return False
        End Try
        Return LibForbReq.WasServiceCancelled()       'return whether the service was cancelled by the user or the service it's self
    End Function

    'for bill re-print functionality
    Private Function RePrintBill() As Boolean
        Dim Row As Integer
        Dim SubRow As Integer
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
        SP.Q.Hit("F8")
        If SP.Q.Check4Text(1, 72, "TSX28") Then
            'TS26 selection screen
            Row = 8
            While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                If SP.Q.Check4Text(Row, 64, " 0.00") = False And SP.Q.Check4Text(Row, 69, "CR") = False Then
                    SP.Q.PutText(21, 12, SP.Q.GetText(Row, 2, 3), True)                   'select loan
                    SP.Q.Hit("Enter")
                    SP.Q.Hit("Enter")
                    SP.Q.Hit("F6")
                    If SP.Q.Check4Text(1, 72, "T1X01") Then
                        'billing screen not displayed
                        'back up until back in ACP
                        While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                            SP.Q.Hit("F12")
                        End While
                        'if an active bill wasn't found
                        SP.frmWhoaDUDE.WhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found")
                        Return False
                    ElseIf SP.Q.Check4Text(1, 72, "TSX15") Then
                        'Target screen
                        If SP.Q.Check4Text(6, 54, "A") Then
                            SP.Q.Hit("F2")
                            'back up until back in ACP
                            While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                                SP.Q.Hit("F12")
                            End While
                            'add comment text to Note DUDE
                            NoteDude.EnterComment(" Duplicate Bill Requested ")
                            SP.frmKnarlyDUDE.KnarlyDude("Processing Complete", "Processing Complete")
                            Return True
                        Else
                            'back up until back in ACP
                            While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                                SP.Q.Hit("F12")
                            End While
                            'if an active bill wasn't found
                            SP.frmWhoaDUDE.WhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found")
                            Return False
                        End If
                    ElseIf SP.Q.Check4Text(1, 72, "TSX14") Then
                        'selection screen
                        'search for active bill
                        SubRow = 8
                        While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                            If SP.Q.Check4Text(SubRow, 24, "A") Then
                                SP.Q.PutText(21, 12, SP.Q.GetText(SubRow, 2, 3), True)          'select loan
                                SP.Q.Hit("F2")
                                'back up until back in ACP
                                While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                                    SP.Q.Hit("F12")
                                End While
                                'add comment text to Note DUDE
                                NoteDude.EnterComment(" Duplicate Bill Requested ")
                                SP.frmKnarlyDUDE.KnarlyDude("Processing Complete", "Processing Complete")
                                Return True
                            End If
                            SubRow += 1
                            If SubRow = 21 Then
                                SubRow = 8
                                SP.Q.Hit("F8")
                            End If
                        End While
                        'back up until back in ACP
                        While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                            SP.Q.Hit("F12")
                        End While
                        'if an active bill wasn't found
                        SP.frmWhoaDUDE.WhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found")
                        Return False
                    End If
                End If
                Row += 1
                'check for page forward
                If Row = 21 Then
                    Row = 8
                    SP.Q.Hit("F8")
                End If
            End While
            'back up until back in ACP
            While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                SP.Q.Hit("F12")
            End While
            SP.frmWhoaDUDE.WhoaDUDE("Maui DUDE couldn't find a COMPASS loan with a balance greater than zero.", "Active Bill Not Found")
            Return False
        Else
            'TS26 target screen
            If SP.Q.Check4Text(11, 17, " 0.00") = False And SP.Q.Check4Text(11, 22, "CR") = False Then
                SP.Q.Hit("Enter")
                SP.Q.Hit("Enter")
                SP.Q.Hit("F6")
                If SP.Q.Check4Text(1, 72, "T1X01") Then
                    'billing screen not displayed
                    'back up until back in ACP
                    While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                        SP.Q.Hit("F12")
                    End While
                    'if an active bill wasn't found
                    SP.frmWhoaDUDE.WhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found")
                    Return False
                ElseIf SP.Q.Check4Text(1, 72, "TSX15") Then
                    'Target screen
                    If SP.Q.Check4Text(6, 54, "A") Then
                        SP.Q.Hit("F2")
                        'back up until back in ACP
                        While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                            SP.Q.Hit("F12")
                        End While
                        'add comment text to Note DUDE
                        NoteDude.EnterComment(" Duplicate Bill Requested ")
                        SP.frmKnarlyDUDE.KnarlyDude("Processing Complete", "Processing Complete")
                        Return True
                    Else
                        'back up until back in ACP
                        While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                            SP.Q.Hit("F12")
                        End While
                        'if an active bill wasn't found
                        SP.frmWhoaDUDE.WhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found")
                        Return False
                    End If
                ElseIf SP.Q.Check4Text(1, 72, "TSX14") Then
                    'selection screen
                    'search for active bill
                    SubRow = 8
                    While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
                        If SP.Q.Check4Text(SubRow, 24, "A") Then
                            SP.Q.PutText(21, 12, SP.Q.GetText(SubRow, 2, 3), True)       'select loan
                            SP.Q.Hit("F2")
                            'back up until back in ACP
                            While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                                SP.Q.Hit("F12")
                            End While
                            'add comment text to Note DUDE
                            NoteDude.EnterComment(" Duplicate Bill Requested ")
                            SP.frmKnarlyDUDE.KnarlyDude("Processing Complete", "Processing Complete")
                            Return True
                        End If
                        SubRow += 1
                        If SubRow = 21 Then
                            SubRow = 8
                            SP.Q.Hit("F8")
                        End If
                    End While
                    'back up until back in ACP
                    While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                        SP.Q.Hit("F12")
                    End While
                    'if an active bill wasn't found
                    SP.frmWhoaDUDE.WhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found")
                    Return False
                End If
            Else
                'back up until back in ACP
                While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                    SP.Q.Hit("F12")
                End While
                SP.frmWhoaDUDE.WhoaDUDE("Maui DUDE couldn't find a COMPASS loan with a balance greater than zero.", "Active Bill Not Found")
                Return False
            End If
            Row += 1
            'check for page forward
            If Row = 21 Then
                Row = 8
                SP.Q.Hit("F8")
            End If
        End If
    End Function

    Public Sub ReturnToACP(ByVal Queue As String, ByVal SubQueue As String, ByVal ACPSelection As String)
        ReturnToACP(Queue, SubQueue, ACPSelection, "")
    End Sub

    Public Sub ReturnToACP(ByVal Queue As String, ByVal SubQueue As String, ByVal ACPSelection As String, ByVal contactPhone As String)
        'determine what the ACPSelection is according to the activity code
        ACPSelection = GetACpSelection()
        'Trim the empty spaces from the variables
        ACPSelection = ACPSelection.Trim()
        Bor.TCX04SelectionValue = Bor.TCX04SelectionValue.Trim()
        'decide whether to return to TC00 directly or through TX6X if a queue was recorded then return through TX6X
        If Queue = "" Then    'if Queue = "" then return directly through TC00
            'return directly to TC00
            SP.Q.FastPath("TX3Z/ATC00" & SSN)
            SP.Q.PutText(19, 38, ACPSelection.Replace("0", ""), True)
        Else    'If Queue is not "" then 
            'return to TC00 through TX6X
            SP.Q.FastPath("TX3Z/ITX6X" & Queue & ";" & SubQueue)
            SP.Q.PutText(21, 18, "1", True)    'select task in working status
            SP.Q.Hit("F4")
        End If

        If Check4Text(1, 74, "TCX13") Then
            'Note screen--nothing to do here.
            Return
        End If

        'TODO: When promoting be sure in set for live
        'enter demographic validation indicators
        'Live
        ''SP.Q.PutText(21, 66, "N")
        ' ''address
        ''If AddVer Then
        ''    SP.Q.PutText(22, 36, "Y")
        ''Else
        ''    SP.Q.PutText(22, 36, "N")
        ''End If
        ' ''phone
        ''If PhoneVer Then
        ''    SP.Q.PutText(22, 52, "Y")
        ''Else
        ''    SP.Q.PutText(22, 52, "N")
        ''End If
        ' ''email
        ''If EMailVer Then
        ''    SP.Q.PutText(22, 68, "Y")
        ''Else
        ''    SP.Q.PutText(22, 68, "N")
        ''End If
        'Test
        SP.Q.PutText(22, 22, "N")
        'address
        If AddVer Then
            SP.Q.PutText(22, 41, "Y")
        Else
            SP.Q.PutText(22, 41, "N")
        End If
        'phone
        If PhoneVer Then
            SP.Q.PutText(22, 60, "Y")
        Else
            SP.Q.PutText(22, 60, "N")
        End If
        'email
        If EMailVer Then
            SP.Q.PutText(22, 79, "Y")
        Else
            SP.Q.PutText(22, 79, "N")
        End If

        SP.Q.Hit("Enter")
        If Check4Text(1, 74, "TCX13") Then
            PutText(12, 10, "")
            Hit("Enter")
            Return
        End If

        While SP.Q.Check4Text(23, 2, "88009") Or SP.Q.Check4Text(23, 2, "88471") Or SP.Q.Check4Text(23, 2, "88005")
            'TODO: When promoting be sure in set for live
            'Live
            ''If SP.Q.Check4Text(23, 2, "88009") Then    'if phone indicator doesn't work then change
            ''    SP.Q.PutText(22, 52, "N", True)
            ''ElseIf SP.Q.Check4Text(23, 2, "88471") Then    'if email indicator doesn't work then change
            ''    SP.Q.PutText(22, 68, "N", True)
            ''ElseIf SP.Q.Check4Text(23, 2, "88005") Then    'if address indicator doesn't work then change
            ''    SP.Q.PutText(22, 36, "N", True)
            ''End If
            'Test
            If SP.Q.Check4Text(23, 2, "88009") Then    'if phone indicator doesn't work then change
                SP.Q.PutText(22, 60, "N", True)
            ElseIf SP.Q.Check4Text(23, 2, "88471") Then    'if email indicator doesn't work then change
                SP.Q.PutText(22, 79, "N", True)
            ElseIf SP.Q.Check4Text(23, 2, "88005") Then    'if address indicator doesn't work then change
                SP.Q.PutText(22, 41, "N", True)
            End If
        End While
        While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False And SP.Q.Check4Text(1, 72, "TCX0C") = False
            If SP.Q.Check4Text(1, 74, "TCX02") Then
                'Enter gathered info
                If SP.Q.Check4Text(8, 2, "_") Then
                    SP.Q.PutText(8, 2, "X")
                End If
            ElseIf SP.Q.Check4Text(1, 74, "TCX04") Then
                'Enter the phone number selected in the Demo popup
                If Not contactPhone = "" And ACPSelection.Replace("0", "") = "2" Then
                    PutText(7, 17, "")
                    Hit("End")
                    PutText(7, 17, contactPhone)
                End If
                SP.Q.PutText(22, 35, Bor.TCX04SelectionValue)
            ElseIf SP.Q.Check4Text(1, 72, "TCX14") Then
                'Enter gathered info
                If SP.Q.GetText(10, 6, 2) = "BK" Then
                    Bor.TCX14SelectionValue = "1"
                ElseIf SP.Q.GetText(16, 6, 2) = "BK" Then
                    Bor.TCX14SelectionValue = "2"
                Else
                    Bor.TCX14SelectionValue = ""
                End If
                SP.Q.PutText(22, 13, Bor.TCX14SelectionValue)
            ElseIf SP.Q.Check4Text(1, 72, "TCX0A") Then
                SP.Q.Hit("Enter")
            Else
                SP.frmWhoaDUDE.WhoaDUDE("Whoa! That's like a totally new screen for me. Im gonna need some help with this.  Please contact Systems Support.", "Knarly DUDE", True)
                Exit Sub
            End If
            SP.Q.Hit("Enter")
        End While
    End Sub

    Private Function GetACpSelection() As String
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim ACPSelection As String = ""
        Conn.Open()
        cmd.Connection = Conn
        If Bor.ActivityCode = "TT" Or Bor.ActivityCode = "TE" Then
            cmd.CommandText = "SELECT TCX01, TCX04 FROM ContactCode WHERE ActivityCode = '" + Bor.ActivityCode + "' AND ContactCode = '" + Bor.ContactCode + "'  AND Description = '" + Bor.AttemptType + "'"
        ElseIf Bor.ActivityCode = "TC" Or Bor.ActivityCode = "TT" Or Bor.ActivityCode = "TE" Or Bor.ActivityCode = "OV" Then
            cmd.CommandText = "SELECT TCX01, TCX04 FROM ContactCode WHERE ActivityCode = '" + Bor.ActivityCode + "' AND ContactCode = '" + Bor.ContactCode + "'"
        Else
            cmd.CommandText = "SELECT TCX01, TCX04 FROM ContactCode WHERE ActivityCode = '' AND ContactCode = ''"
        End If
        reader = cmd.ExecuteReader
        While reader.Read
            ACPSelection = reader.Item("TCX01")
            Bor.TCX04SelectionValue = reader.Item("TCX04")
        End While
        Conn.Close()
        If (ACPSelection = "") Then ACPSelection = "04"
        Return ACPSelection
    End Function

    Public Shared Sub RunDotNETDll(ByVal Script As DataRow, ByVal Bor As Borrower, ByVal RunNumber As Integer)
        Dim mainDllToLoad As String = Script.Item("DLLToLoad").ToString()
        Dim objs(2) As Object
        Dim scriptInstance As ObjectHandle
        FileUpdater(Script.Item("DLLsToCopy").ToString()) 'update files
        'if in test mode then load test dll
        If SP.TestMode() Then mainDllToLoad = "Test\" & mainDllToLoad
        'load parameters for script's constructor
        objs(0) = New ReflectionInterface(SP.Q.RIBM, TestMode)
        objs(1) = Bor
        objs(2) = RunNumber
        'start script
        Try
            scriptInstance = System.Activator.CreateInstanceFrom(pcDir + mainDllToLoad, Script.Item("ObjectToCreate").ToString(), True, Nothing, Nothing, objs, Nothing, Nothing, Nothing)
            CType(scriptInstance.Unwrap(), ScriptBase).Main()
        Catch ex As EndDLLException 'any time the coder wants the script to end they call a method that throws this exception
            Exit Sub 'end script
        End Try
    End Sub

    Private Shared Sub UpdateDlls(ByVal dllFiles As String)
        Dim workingDir As String
        Dim pcWorkingDir As String
        Dim Dlls() As String
        Dim Dll As Object
        Dlls = Split(dllFiles, ",")
        If SP.TestMode() = False Then
            workingDir = liveNetworkDir
            pcWorkingDir = pcDir
        Else
            workingDir = testNetworkDir
            pcWorkingDir = pcDir & "Test\"
            'check for existence of test directory and create if needed
            If Dir(pcWorkingDir) = "" Then
                MkDir(pcWorkingDir)
                'copy over all files from live directory if the test directory was just created
                Dim dllToCopy As String
                dllToCopy = Dir(pcDir & "*")
                While dllToCopy <> ""
                    FileCopy(pcDir & dllToCopy, pcWorkingDir & dllToCopy)
                    dllToCopy = Dir()
                End While
            End If
        End If
        Try
            For Each Dll In Dlls
                If Dir(pcWorkingDir & Dll) = "" Then
                    'if Dll doesn't exist then pull it down from the network
                    FileCopy(workingDir & Dll, pcWorkingDir & Dll)
                Else
                    'if Dll exists then check if it needs to be updated
                    If FileDateTime(workingDir & Dll) <> FileDateTime(pcWorkingDir & Dll) Then
                        'if time date stamps don't equal then update the Dll
                        Kill(pcWorkingDir & Dll)
                        FileCopy(workingDir & Dll, pcWorkingDir & Dll)
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New System.Exception("An error occurred while trying to update your script code.  The most likely reason for this is because there is new code to be loaded and the code loaded on your PC is old.  It is suggested that you shutdown your Reflection session and start it back up to refresh your code.  If you feel that you have received this error for other reasons then please contact Systems Support.")
        End Try
    End Sub

    'deletes old dlls
    Private Shared Sub DeleteOldDlls(ByVal dllName As String)
        If Dir(pcDir & dllName & ".dll") <> "" Then
            Kill(pcDir & dllName & ".dll")
            Kill(pcDir & dllName & ".tlb")
        End If
    End Sub

    'updates .net DLLs
    Private Shared Sub FileUpdater(ByVal dllFilesToCopy As String)
        UpdateDlls("Q.dll") 'update Q
        UpdateDlls(dllFilesToCopy) 'update all other dlls
        'delete old DLLs
        DeleteOldDlls("Alderaan")
        DeleteOldDlls("Coruscant")
        DeleteOldDlls("Dagobah")
        DeleteOldDlls("Endor")
        DeleteOldDlls("Ferengi")
        DeleteOldDlls("Gallifrey")
        DeleteOldDlls("Gondor")
        DeleteOldDlls("Hoth")
        DeleteOldDlls("Klingon")
        DeleteOldDlls("Mordor")
        DeleteOldDlls("Moria")
        DeleteOldDlls("Naboo")
        DeleteOldDlls("Rivendale")
        DeleteOldDlls("Rohan")
        DeleteOldDlls("Romulan")
        DeleteOldDlls("Shire")
        DeleteOldDlls("Tatooine")
        DeleteOldDlls("Vulcan")
        DeleteOldDlls("Yavin")
    End Sub

    Private Sub LV_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LV.SelectedIndexChanged

    End Sub
End Class
