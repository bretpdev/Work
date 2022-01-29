'Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports Word


Public Class PaymentReceipts
    Inherits System.Windows.Forms.Form

    Private Userid As String
    'Private DBConn As OleDbConnection 'Access Connection
    Private DBConn As SqlConnection
    Private LastReceiptTypePrinted As String
    Private DALGPPayment As SqlDataAdapter
    Private DALPPPayment As SqlDataAdapter
    'Private DAUESPPayment As SqlDataAdapter
    Private DATILPPayment As SqlDataAdapter
    Private DATILPVoid As SqlDataAdapter
    Private DALGPVoid As SqlDataAdapter
    Private DALPPVoid As SqlDataAdapter
    Private DAUESPVoid As SqlDataAdapter
    Private DSLGPPayment As DataSet
    Private DSLPPPayment As DataSet
    'Private DSUESPPayment As DataSet
    Private DSTILPVoid As DataSet
    Private DSTILPPayment As DataSet
    Private DSLGPVoid As DataSet
    Private DSLPPVoid As DataSet
    Private DSUESPVoid As DataSet
    Private DSBen As DataSet
    Private ReceiptNumber As String
    Private PathModifier4TestMode As String


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents TabUESP As System.Windows.Forms.TabPage
    Friend WithEvents TabLPP As System.Windows.Forms.TabPage
    Friend WithEvents TabLGP As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents tbUESPTPA As System.Windows.Forms.TextBox
    Friend WithEvents btnUESPProcess As System.Windows.Forms.Button
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents btnUESPClear As System.Windows.Forms.Button
    Friend WithEvents tbUESPRB As System.Windows.Forms.TextBox
    Friend WithEvents tbUESPPN As System.Windows.Forms.TextBox
    Friend WithEvents Tabs As System.Windows.Forms.TabControl
    Friend WithEvents btnLPPProcess As System.Windows.Forms.Button
    Friend WithEvents btnLGPProcess As System.Windows.Forms.Button
    Friend WithEvents tbLPPPA As System.Windows.Forms.TextBox
    Friend WithEvents tbLPPRB As System.Windows.Forms.TextBox
    Friend WithEvents tbLPPCN As System.Windows.Forms.TextBox
    Friend WithEvents rbLPPCheck As System.Windows.Forms.RadioButton
    Friend WithEvents rbLPPCash As System.Windows.Forms.RadioButton
    Friend WithEvents lblLPPCN As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents btnLPPClear As System.Windows.Forms.Button
    Friend WithEvents tbLPPBN As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents tbLPPAN As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents tbLGPAN As System.Windows.Forms.TextBox
    Friend WithEvents btnLGPClear As System.Windows.Forms.Button
    Friend WithEvents tbLGPPA As System.Windows.Forms.TextBox
    Friend WithEvents tbLGPRB As System.Windows.Forms.TextBox
    Friend WithEvents tbLGPBN As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents TabAcctg As System.Windows.Forms.TabPage
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents rbAcctgCheck As System.Windows.Forms.RadioButton
    Friend WithEvents rbAcctgCash As System.Windows.Forms.RadioButton
    Friend WithEvents btnAcctgClear As System.Windows.Forms.Button
    Friend WithEvents btnAcctgProcess As System.Windows.Forms.Button
    Friend WithEvents tbAcctgAN As System.Windows.Forms.TextBox
    Friend WithEvents tbAcctgPA As System.Windows.Forms.TextBox
    Friend WithEvents tbAcctgRB As System.Windows.Forms.TextBox
    Friend WithEvents tbAcctgCN As System.Windows.Forms.TextBox
    Friend WithEvents tbAcctgName As System.Windows.Forms.TextBox
    Friend WithEvents cbAcctgGD As System.Windows.Forms.CheckBox
    Friend WithEvents lblAcctgCN As System.Windows.Forms.Label
    Friend WithEvents btnReprint As System.Windows.Forms.Button
    Friend WithEvents TabVoidR As System.Windows.Forms.TabPage
    Friend WithEvents TabReports As System.Windows.Forms.TabPage
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents TestDB As System.Data.SqlClient.SqlConnection
    Friend WithEvents LiveDB As System.Data.SqlClient.SqlConnection
    Friend WithEvents tbRecNum As System.Windows.Forms.TextBox
    Friend WithEvents cbPrgm As System.Windows.Forms.ComboBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents tbNotes As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents LPPSD As System.Windows.Forms.DateTimePicker
    Friend WithEvents LPPED As System.Windows.Forms.DateTimePicker
    Friend WithEvents LGPED As System.Windows.Forms.DateTimePicker
    Friend WithEvents LGPSD As System.Windows.Forms.DateTimePicker
    Friend WithEvents UESPED As System.Windows.Forms.DateTimePicker
    Friend WithEvents UESPSD As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnRptLPP As System.Windows.Forms.Button
    Friend WithEvents btnRptLGP As System.Windows.Forms.Button
    Friend WithEvents btnRptUESP As System.Windows.Forms.Button
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents DSLGPPayments1 As Receipts.DSLGPPayments
    Friend WithEvents DSLGPPayments2 As Receipts.DSLGPPayments
    Friend WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents TstLGPPayments As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents TstDSLGPPayments As Receipts.DSLGPPayments
    Friend WithEvents LivLGPPayments As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlSelectCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents LivDSLGPPayments As Receipts.LivDSLGPPayments
    Friend WithEvents TstLPPPayments As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlSelectCommand3 As System.Data.SqlClient.SqlCommand
    Friend WithEvents TstDSLPPPayments As Receipts.TstDSLGPPayments
    Friend WithEvents LivLPPPayments As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlSelectCommand4 As System.Data.SqlClient.SqlCommand
    Friend WithEvents LivDSLPPPayments As Receipts.LIVDSLPPPayments
    Friend WithEvents TstLPPVoids As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents GroupBox13 As System.Windows.Forms.GroupBox
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents btnVRptLPP As System.Windows.Forms.Button
    Friend WithEvents VLPPED As System.Windows.Forms.DateTimePicker
    Friend WithEvents VLPPSD As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnVRptLGP As System.Windows.Forms.Button
    Friend WithEvents VLGPED As System.Windows.Forms.DateTimePicker
    Friend WithEvents VLGPSD As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnVRptUESP As System.Windows.Forms.Button
    Friend WithEvents VUESPED As System.Windows.Forms.DateTimePicker
    Friend WithEvents VUESPSD As System.Windows.Forms.DateTimePicker
    Friend WithEvents SqlSelectCommand7 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDeleteCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents TstDSLPPVoids As Receipts.TstDSLPPVoids
    Friend WithEvents LivLPPVoids As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlSelectCommand8 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDeleteCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents LivDSLPPVoids As Receipts.LivDSLPPVoids
    Friend WithEvents LivLGPVoids As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlSelectCommand9 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand3 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand3 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDeleteCommand3 As System.Data.SqlClient.SqlCommand
    Friend WithEvents TstLGPVoids As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlSelectCommand10 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand4 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand4 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDeleteCommand4 As System.Data.SqlClient.SqlCommand
    Friend WithEvents TstDSLGPVoids As Receipts.TstDSLGPVoids
    Friend WithEvents LivDSLGPVoids As Receipts.LivDSLGPVoids
    Friend WithEvents TabTILP As System.Windows.Forms.TabPage
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents tbTILPAN As System.Windows.Forms.TextBox
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents btnTILPClear As System.Windows.Forms.Button
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents tbTILPPA As System.Windows.Forms.TextBox
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents tbTILPRB As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents tbTILPCN As System.Windows.Forms.TextBox
    Friend WithEvents rbTILPCheck As System.Windows.Forms.RadioButton
    Friend WithEvents rbTILPCash As System.Windows.Forms.RadioButton
    Friend WithEvents lblTILPCN As System.Windows.Forms.Label
    Friend WithEvents tbTILPBN As System.Windows.Forms.TextBox
    Friend WithEvents btnTILPProcess As System.Windows.Forms.Button
    Friend WithEvents GroupBox14 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox15 As System.Windows.Forms.GroupBox
    Friend WithEvents btnVRptTILP As System.Windows.Forms.Button
    Friend WithEvents VTILPED As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents VTILPSD As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox16 As System.Windows.Forms.GroupBox
    Friend WithEvents btnRptTILP As System.Windows.Forms.Button
    Friend WithEvents TILPED As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents TILPSD As System.Windows.Forms.DateTimePicker
    Friend WithEvents TstTILPVoids As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlDeleteCommand7 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand7 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlSelectCommand15 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand7 As System.Data.SqlClient.SqlCommand
    Friend WithEvents TstTILPPayments As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlSelectCommand13 As System.Data.SqlClient.SqlCommand
    Friend WithEvents LivTILPPayments As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlSelectCommand14 As System.Data.SqlClient.SqlCommand
    Friend WithEvents LivTILPVoids As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents SqlDeleteCommand8 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand8 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlSelectCommand16 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand8 As System.Data.SqlClient.SqlCommand
    Friend WithEvents TstDSTILPPayments As Receipts.TstDSTILPPayments
    Friend WithEvents LivDSTILPPayments As Receipts.LivDSTILPPayments
    Friend WithEvents TstDSTILPVoids As Receipts.TstDSTILPVoids
    Friend WithEvents LivDSTILPVoids As Receipts.LivDSTILPVoids
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents tbLGPCN As System.Windows.Forms.TextBox
    Friend WithEvents rbLGPCheck As System.Windows.Forms.RadioButton
    Friend WithEvents rbLGPCash As System.Windows.Forms.RadioButton
    Friend WithEvents lblLGPCN As System.Windows.Forms.Label
    Friend WithEvents dgBen As System.Windows.Forms.DataGrid
    Friend WithEvents lblTest As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PaymentReceipts))
        Me.Tabs = New System.Windows.Forms.TabControl
        Me.TabUESP = New System.Windows.Forms.TabPage
        Me.dgBen = New System.Windows.Forms.DataGrid
        Me.btnUESPClear = New System.Windows.Forms.Button
        Me.btnUESPProcess = New System.Windows.Forms.Button
        Me.Label18 = New System.Windows.Forms.Label
        Me.tbUESPTPA = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.tbUESPRB = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.lblTotal = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbUESPPN = New System.Windows.Forms.TextBox
        Me.TabLGP = New System.Windows.Forms.TabPage
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.tbLGPCN = New System.Windows.Forms.TextBox
        Me.rbLGPCheck = New System.Windows.Forms.RadioButton
        Me.rbLGPCash = New System.Windows.Forms.RadioButton
        Me.lblLGPCN = New System.Windows.Forms.Label
        Me.Label28 = New System.Windows.Forms.Label
        Me.tbLGPAN = New System.Windows.Forms.TextBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.btnLGPClear = New System.Windows.Forms.Button
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.tbLGPPA = New System.Windows.Forms.TextBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.tbLGPRB = New System.Windows.Forms.TextBox
        Me.tbLGPBN = New System.Windows.Forms.TextBox
        Me.btnLGPProcess = New System.Windows.Forms.Button
        Me.TabLPP = New System.Windows.Forms.TabPage
        Me.tbLPPAN = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.btnLPPClear = New System.Windows.Forms.Button
        Me.Label23 = New System.Windows.Forms.Label
        Me.btnLPPProcess = New System.Windows.Forms.Button
        Me.Label19 = New System.Windows.Forms.Label
        Me.tbLPPPA = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.tbLPPRB = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.tbLPPCN = New System.Windows.Forms.TextBox
        Me.rbLPPCheck = New System.Windows.Forms.RadioButton
        Me.rbLPPCash = New System.Windows.Forms.RadioButton
        Me.lblLPPCN = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.tbLPPBN = New System.Windows.Forms.TextBox
        Me.TabTILP = New System.Windows.Forms.TabPage
        Me.Label52 = New System.Windows.Forms.Label
        Me.tbTILPAN = New System.Windows.Forms.TextBox
        Me.Label53 = New System.Windows.Forms.Label
        Me.btnTILPClear = New System.Windows.Forms.Button
        Me.Label54 = New System.Windows.Forms.Label
        Me.Label55 = New System.Windows.Forms.Label
        Me.tbTILPPA = New System.Windows.Forms.TextBox
        Me.Label56 = New System.Windows.Forms.Label
        Me.tbTILPRB = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.tbTILPCN = New System.Windows.Forms.TextBox
        Me.rbTILPCheck = New System.Windows.Forms.RadioButton
        Me.rbTILPCash = New System.Windows.Forms.RadioButton
        Me.lblTILPCN = New System.Windows.Forms.Label
        Me.tbTILPBN = New System.Windows.Forms.TextBox
        Me.btnTILPProcess = New System.Windows.Forms.Button
        Me.TabAcctg = New System.Windows.Forms.TabPage
        Me.cbAcctgGD = New System.Windows.Forms.CheckBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.tbAcctgAN = New System.Windows.Forms.TextBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.btnAcctgClear = New System.Windows.Forms.Button
        Me.Label31 = New System.Windows.Forms.Label
        Me.tbAcctgPA = New System.Windows.Forms.TextBox
        Me.Label32 = New System.Windows.Forms.Label
        Me.tbAcctgRB = New System.Windows.Forms.TextBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.tbAcctgCN = New System.Windows.Forms.TextBox
        Me.rbAcctgCheck = New System.Windows.Forms.RadioButton
        Me.rbAcctgCash = New System.Windows.Forms.RadioButton
        Me.lblAcctgCN = New System.Windows.Forms.Label
        Me.tbAcctgName = New System.Windows.Forms.TextBox
        Me.btnAcctgProcess = New System.Windows.Forms.Button
        Me.Label34 = New System.Windows.Forms.Label
        Me.TabVoidR = New System.Windows.Forms.TabPage
        Me.Label37 = New System.Windows.Forms.Label
        Me.Label36 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.cbPrgm = New System.Windows.Forms.ComboBox
        Me.tbRecNum = New System.Windows.Forms.TextBox
        Me.tbNotes = New System.Windows.Forms.TextBox
        Me.Label33 = New System.Windows.Forms.Label
        Me.TabReports = New System.Windows.Forms.TabPage
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.GroupBox13 = New System.Windows.Forms.GroupBox
        Me.btnVRptUESP = New System.Windows.Forms.Button
        Me.VUESPED = New System.Windows.Forms.DateTimePicker
        Me.Label49 = New System.Windows.Forms.Label
        Me.Label50 = New System.Windows.Forms.Label
        Me.VUESPSD = New System.Windows.Forms.DateTimePicker
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.Label44 = New System.Windows.Forms.Label
        Me.UESPSD = New System.Windows.Forms.DateTimePicker
        Me.btnRptUESP = New System.Windows.Forms.Button
        Me.UESPED = New System.Windows.Forms.DateTimePicker
        Me.Label43 = New System.Windows.Forms.Label
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.GroupBox12 = New System.Windows.Forms.GroupBox
        Me.btnVRptLGP = New System.Windows.Forms.Button
        Me.VLGPED = New System.Windows.Forms.DateTimePicker
        Me.Label47 = New System.Windows.Forms.Label
        Me.Label48 = New System.Windows.Forms.Label
        Me.VLGPSD = New System.Windows.Forms.DateTimePicker
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.btnRptLGP = New System.Windows.Forms.Button
        Me.LGPED = New System.Windows.Forms.DateTimePicker
        Me.Label41 = New System.Windows.Forms.Label
        Me.Label42 = New System.Windows.Forms.Label
        Me.LGPSD = New System.Windows.Forms.DateTimePicker
        Me.Label38 = New System.Windows.Forms.Label
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.GroupBox11 = New System.Windows.Forms.GroupBox
        Me.btnVRptLPP = New System.Windows.Forms.Button
        Me.VLPPED = New System.Windows.Forms.DateTimePicker
        Me.Label45 = New System.Windows.Forms.Label
        Me.Label46 = New System.Windows.Forms.Label
        Me.VLPPSD = New System.Windows.Forms.DateTimePicker
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.btnRptLPP = New System.Windows.Forms.Button
        Me.LPPED = New System.Windows.Forms.DateTimePicker
        Me.Label40 = New System.Windows.Forms.Label
        Me.Label39 = New System.Windows.Forms.Label
        Me.LPPSD = New System.Windows.Forms.DateTimePicker
        Me.GroupBox14 = New System.Windows.Forms.GroupBox
        Me.GroupBox15 = New System.Windows.Forms.GroupBox
        Me.btnVRptTILP = New System.Windows.Forms.Button
        Me.VTILPED = New System.Windows.Forms.DateTimePicker
        Me.Label57 = New System.Windows.Forms.Label
        Me.Label58 = New System.Windows.Forms.Label
        Me.VTILPSD = New System.Windows.Forms.DateTimePicker
        Me.GroupBox16 = New System.Windows.Forms.GroupBox
        Me.btnRptTILP = New System.Windows.Forms.Button
        Me.TILPED = New System.Windows.Forms.DateTimePicker
        Me.Label59 = New System.Windows.Forms.Label
        Me.Label60 = New System.Windows.Forms.Label
        Me.TILPSD = New System.Windows.Forms.DateTimePicker
        Me.btnReprint = New System.Windows.Forms.Button
        Me.TestDB = New System.Data.SqlClient.SqlConnection
        Me.LiveDB = New System.Data.SqlClient.SqlConnection
        Me.TstLGPPayments = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlSelectCommand1 = New System.Data.SqlClient.SqlCommand
        Me.TstDSLGPPayments = New Receipts.DSLGPPayments
        Me.LivLGPPayments = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlSelectCommand2 = New System.Data.SqlClient.SqlCommand
        Me.LivDSLGPPayments = New Receipts.LivDSLGPPayments
        Me.TstLPPPayments = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlSelectCommand3 = New System.Data.SqlClient.SqlCommand
        Me.TstDSLPPPayments = New Receipts.TstDSLGPPayments
        Me.LivLPPPayments = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlSelectCommand4 = New System.Data.SqlClient.SqlCommand
        Me.LivDSLPPPayments = New Receipts.LIVDSLPPPayments
        Me.TstLPPVoids = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlDeleteCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlInsertCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand7 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand1 = New System.Data.SqlClient.SqlCommand
        Me.TstDSLPPVoids = New Receipts.TstDSLPPVoids
        Me.LivLPPVoids = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlDeleteCommand2 = New System.Data.SqlClient.SqlCommand
        Me.SqlInsertCommand2 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand8 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand2 = New System.Data.SqlClient.SqlCommand
        Me.LivDSLPPVoids = New Receipts.LivDSLPPVoids
        Me.LivLGPVoids = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlDeleteCommand3 = New System.Data.SqlClient.SqlCommand
        Me.SqlInsertCommand3 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand9 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand3 = New System.Data.SqlClient.SqlCommand
        Me.TstLGPVoids = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlDeleteCommand4 = New System.Data.SqlClient.SqlCommand
        Me.SqlInsertCommand4 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand10 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand4 = New System.Data.SqlClient.SqlCommand
        Me.TstDSLGPVoids = New Receipts.TstDSLGPVoids
        Me.LivDSLGPVoids = New Receipts.LivDSLGPVoids
        Me.TstTILPVoids = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlDeleteCommand7 = New System.Data.SqlClient.SqlCommand
        Me.SqlInsertCommand7 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand15 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand7 = New System.Data.SqlClient.SqlCommand
        Me.TstTILPPayments = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlSelectCommand13 = New System.Data.SqlClient.SqlCommand
        Me.LivTILPPayments = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlSelectCommand14 = New System.Data.SqlClient.SqlCommand
        Me.LivTILPVoids = New System.Data.SqlClient.SqlDataAdapter
        Me.SqlDeleteCommand8 = New System.Data.SqlClient.SqlCommand
        Me.SqlInsertCommand8 = New System.Data.SqlClient.SqlCommand
        Me.SqlSelectCommand16 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand8 = New System.Data.SqlClient.SqlCommand
        Me.TstDSTILPPayments = New Receipts.TstDSTILPPayments
        Me.LivDSTILPPayments = New Receipts.LivDSTILPPayments
        Me.TstDSTILPVoids = New Receipts.TstDSTILPVoids
        Me.LivDSTILPVoids = New Receipts.LivDSTILPVoids
        Me.lblTest = New System.Windows.Forms.Label
        Me.Tabs.SuspendLayout()
        Me.TabUESP.SuspendLayout()
        CType(Me.dgBen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabLGP.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabLPP.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabTILP.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabAcctg.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.TabVoidR.SuspendLayout()
        Me.TabReports.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox13.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox14.SuspendLayout()
        Me.GroupBox15.SuspendLayout()
        Me.GroupBox16.SuspendLayout()
        CType(Me.TstDSLGPPayments, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LivDSLGPPayments, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TstDSLPPPayments, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LivDSLPPPayments, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TstDSLPPVoids, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LivDSLPPVoids, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TstDSLGPVoids, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LivDSLGPVoids, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TstDSTILPPayments, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LivDSTILPPayments, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TstDSTILPVoids, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LivDSTILPVoids, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Tabs
        '
        Me.Tabs.Controls.Add(Me.TabUESP)
        Me.Tabs.Controls.Add(Me.TabLGP)
        Me.Tabs.Controls.Add(Me.TabLPP)
        Me.Tabs.Controls.Add(Me.TabTILP)
        Me.Tabs.Controls.Add(Me.TabAcctg)
        Me.Tabs.Controls.Add(Me.TabVoidR)
        Me.Tabs.Controls.Add(Me.TabReports)
        Me.Tabs.Location = New System.Drawing.Point(8, 8)
        Me.Tabs.Name = "Tabs"
        Me.Tabs.SelectedIndex = 0
        Me.Tabs.Size = New System.Drawing.Size(632, 592)
        Me.Tabs.TabIndex = 0
        '
        'TabUESP
        '
        Me.TabUESP.Controls.Add(Me.dgBen)
        Me.TabUESP.Controls.Add(Me.btnUESPClear)
        Me.TabUESP.Controls.Add(Me.btnUESPProcess)
        Me.TabUESP.Controls.Add(Me.Label18)
        Me.TabUESP.Controls.Add(Me.tbUESPTPA)
        Me.TabUESP.Controls.Add(Me.Label17)
        Me.TabUESP.Controls.Add(Me.tbUESPRB)
        Me.TabUESP.Controls.Add(Me.Label16)
        Me.TabUESP.Controls.Add(Me.lblTotal)
        Me.TabUESP.Controls.Add(Me.Label2)
        Me.TabUESP.Controls.Add(Me.Label1)
        Me.TabUESP.Controls.Add(Me.tbUESPPN)
        Me.TabUESP.Location = New System.Drawing.Point(4, 22)
        Me.TabUESP.Name = "TabUESP"
        Me.TabUESP.Size = New System.Drawing.Size(624, 566)
        Me.TabUESP.TabIndex = 0
        Me.TabUESP.Text = "UESP"
        '
        'dgBen
        '
        Me.dgBen.CaptionText = "Beneficiary"
        Me.dgBen.DataMember = ""
        Me.dgBen.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgBen.Location = New System.Drawing.Point(8, 120)
        Me.dgBen.Name = "dgBen"
        Me.dgBen.Size = New System.Drawing.Size(600, 212)
        Me.dgBen.TabIndex = 54
        '
        'btnUESPClear
        '
        Me.btnUESPClear.Location = New System.Drawing.Point(332, 376)
        Me.btnUESPClear.Name = "btnUESPClear"
        Me.btnUESPClear.TabIndex = 7
        Me.btnUESPClear.Text = "Clear"
        '
        'btnUESPProcess
        '
        Me.btnUESPProcess.Location = New System.Drawing.Point(228, 376)
        Me.btnUESPProcess.Name = "btnUESPProcess"
        Me.btnUESPProcess.TabIndex = 6
        Me.btnUESPProcess.Text = "Process"
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(372, 100)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(104, 16)
        Me.Label18.TabIndex = 53
        Me.Label18.Text = "Total Payment Amt:"
        '
        'tbUESPTPA
        '
        Me.tbUESPTPA.Location = New System.Drawing.Point(476, 96)
        Me.tbUESPTPA.Name = "tbUESPTPA"
        Me.tbUESPTPA.Size = New System.Drawing.Size(132, 20)
        Me.tbUESPTPA.TabIndex = 4
        Me.tbUESPTPA.Text = ""
        Me.tbUESPTPA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(384, 52)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(72, 16)
        Me.Label17.TabIndex = 51
        Me.Label17.Text = "Received By:"
        '
        'tbUESPRB
        '
        Me.tbUESPRB.Location = New System.Drawing.Point(456, 48)
        Me.tbUESPRB.Name = "tbUESPRB"
        Me.tbUESPRB.Size = New System.Drawing.Size(152, 20)
        Me.tbUESPRB.TabIndex = 2
        Me.tbUESPRB.Text = ""
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(408, 348)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(72, 16)
        Me.Label16.TabIndex = 49
        Me.Label16.Text = "Grand Total:"
        '
        'lblTotal
        '
        Me.lblTotal.Location = New System.Drawing.Point(480, 348)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(112, 16)
        Me.lblTotal.TabIndex = 48
        Me.lblTotal.Text = "0.00"
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(8, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(608, 23)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "UESP Payment"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(4, 52)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "  Account Owner's Name:"
        '
        'tbUESPPN
        '
        Me.tbUESPPN.Location = New System.Drawing.Point(136, 48)
        Me.tbUESPPN.MaxLength = 50
        Me.tbUESPPN.Name = "tbUESPPN"
        Me.tbUESPPN.Size = New System.Drawing.Size(240, 20)
        Me.tbUESPPN.TabIndex = 1
        Me.tbUESPPN.Text = ""
        '
        'TabLGP
        '
        Me.TabLGP.Controls.Add(Me.GroupBox3)
        Me.TabLGP.Controls.Add(Me.Label28)
        Me.TabLGP.Controls.Add(Me.tbLGPAN)
        Me.TabLGP.Controls.Add(Me.Label24)
        Me.TabLGP.Controls.Add(Me.btnLGPClear)
        Me.TabLGP.Controls.Add(Me.Label25)
        Me.TabLGP.Controls.Add(Me.Label26)
        Me.TabLGP.Controls.Add(Me.tbLGPPA)
        Me.TabLGP.Controls.Add(Me.Label27)
        Me.TabLGP.Controls.Add(Me.tbLGPRB)
        Me.TabLGP.Controls.Add(Me.tbLGPBN)
        Me.TabLGP.Controls.Add(Me.btnLGPProcess)
        Me.TabLGP.Location = New System.Drawing.Point(4, 22)
        Me.TabLGP.Name = "TabLGP"
        Me.TabLGP.Size = New System.Drawing.Size(624, 566)
        Me.TabLGP.TabIndex = 2
        Me.TabLGP.Text = "LGP"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.tbLGPCN)
        Me.GroupBox3.Controls.Add(Me.rbLGPCheck)
        Me.GroupBox3.Controls.Add(Me.rbLGPCash)
        Me.GroupBox3.Controls.Add(Me.lblLGPCN)
        Me.GroupBox3.Location = New System.Drawing.Point(28, 132)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(364, 48)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Payment Method"
        '
        'tbLGPCN
        '
        Me.tbLGPCN.Enabled = False
        Me.tbLGPCN.Location = New System.Drawing.Point(248, 20)
        Me.tbLGPCN.MaxLength = 20
        Me.tbLGPCN.Name = "tbLGPCN"
        Me.tbLGPCN.TabIndex = 2
        Me.tbLGPCN.Text = ""
        '
        'rbLGPCheck
        '
        Me.rbLGPCheck.Location = New System.Drawing.Point(80, 20)
        Me.rbLGPCheck.Name = "rbLGPCheck"
        Me.rbLGPCheck.Size = New System.Drawing.Size(64, 24)
        Me.rbLGPCheck.TabIndex = 1
        Me.rbLGPCheck.TabStop = True
        Me.rbLGPCheck.Text = "Check"
        '
        'rbLGPCash
        '
        Me.rbLGPCash.Location = New System.Drawing.Point(16, 20)
        Me.rbLGPCash.Name = "rbLGPCash"
        Me.rbLGPCash.Size = New System.Drawing.Size(64, 24)
        Me.rbLGPCash.TabIndex = 0
        Me.rbLGPCash.TabStop = True
        Me.rbLGPCash.Text = "Cash"
        '
        'lblLGPCN
        '
        Me.lblLGPCN.Enabled = False
        Me.lblLGPCN.Location = New System.Drawing.Point(160, 24)
        Me.lblLGPCN.Name = "lblLGPCN"
        Me.lblLGPCN.Size = New System.Drawing.Size(88, 16)
        Me.lblLGPCN.TabIndex = 5
        Me.lblLGPCN.Text = "Check Number:"
        '
        'Label28
        '
        Me.Label28.Location = New System.Drawing.Point(8, 84)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(88, 16)
        Me.Label28.TabIndex = 77
        Me.Label28.Text = " Received From:"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbLGPAN
        '
        Me.tbLGPAN.Location = New System.Drawing.Point(136, 104)
        Me.tbLGPAN.MaxLength = 10
        Me.tbLGPAN.Name = "tbLGPAN"
        Me.tbLGPAN.Size = New System.Drawing.Size(152, 20)
        Me.tbLGPAN.TabIndex = 3
        Me.tbLGPAN.Text = ""
        '
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(8, 108)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(128, 16)
        Me.Label24.TabIndex = 76
        Me.Label24.Text = " Account Number/SSN:"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnLGPClear
        '
        Me.btnLGPClear.Location = New System.Drawing.Point(328, 228)
        Me.btnLGPClear.Name = "btnLGPClear"
        Me.btnLGPClear.TabIndex = 7
        Me.btnLGPClear.Text = "Clear"
        '
        'Label25
        '
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.ForeColor = System.Drawing.Color.Red
        Me.Label25.Location = New System.Drawing.Point(8, 16)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(608, 23)
        Me.Label25.TabIndex = 75
        Me.Label25.Text = "LGP Payment"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label26
        '
        Me.Label26.Location = New System.Drawing.Point(8, 192)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(80, 16)
        Me.Label26.TabIndex = 74
        Me.Label26.Text = "Payment Amt:"
        '
        'tbLGPPA
        '
        Me.tbLGPPA.Location = New System.Drawing.Point(136, 188)
        Me.tbLGPPA.Name = "tbLGPPA"
        Me.tbLGPPA.Size = New System.Drawing.Size(144, 20)
        Me.tbLGPPA.TabIndex = 5
        Me.tbLGPPA.Text = "0.00"
        Me.tbLGPPA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label27
        '
        Me.Label27.Location = New System.Drawing.Point(8, 60)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(72, 16)
        Me.Label27.TabIndex = 73
        Me.Label27.Text = "Received By:"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbLGPRB
        '
        Me.tbLGPRB.Location = New System.Drawing.Point(136, 56)
        Me.tbLGPRB.Name = "tbLGPRB"
        Me.tbLGPRB.Size = New System.Drawing.Size(152, 20)
        Me.tbLGPRB.TabIndex = 1
        Me.tbLGPRB.Text = ""
        '
        'tbLGPBN
        '
        Me.tbLGPBN.Location = New System.Drawing.Point(136, 80)
        Me.tbLGPBN.MaxLength = 50
        Me.tbLGPBN.Name = "tbLGPBN"
        Me.tbLGPBN.Size = New System.Drawing.Size(256, 20)
        Me.tbLGPBN.TabIndex = 2
        Me.tbLGPBN.Text = ""
        '
        'btnLGPProcess
        '
        Me.btnLGPProcess.Location = New System.Drawing.Point(224, 228)
        Me.btnLGPProcess.Name = "btnLGPProcess"
        Me.btnLGPProcess.TabIndex = 6
        Me.btnLGPProcess.Text = "Process"
        '
        'TabLPP
        '
        Me.TabLPP.Controls.Add(Me.tbLPPAN)
        Me.TabLPP.Controls.Add(Me.Label21)
        Me.TabLPP.Controls.Add(Me.btnLPPClear)
        Me.TabLPP.Controls.Add(Me.Label23)
        Me.TabLPP.Controls.Add(Me.btnLPPProcess)
        Me.TabLPP.Controls.Add(Me.Label19)
        Me.TabLPP.Controls.Add(Me.tbLPPPA)
        Me.TabLPP.Controls.Add(Me.Label20)
        Me.TabLPP.Controls.Add(Me.tbLPPRB)
        Me.TabLPP.Controls.Add(Me.GroupBox2)
        Me.TabLPP.Controls.Add(Me.Label22)
        Me.TabLPP.Controls.Add(Me.tbLPPBN)
        Me.TabLPP.Location = New System.Drawing.Point(4, 22)
        Me.TabLPP.Name = "TabLPP"
        Me.TabLPP.Size = New System.Drawing.Size(624, 566)
        Me.TabLPP.TabIndex = 1
        Me.TabLPP.Text = "LPP"
        '
        'tbLPPAN
        '
        Me.tbLPPAN.Location = New System.Drawing.Point(136, 104)
        Me.tbLPPAN.MaxLength = 10
        Me.tbLPPAN.Name = "tbLPPAN"
        Me.tbLPPAN.Size = New System.Drawing.Size(152, 20)
        Me.tbLPPAN.TabIndex = 3
        Me.tbLPPAN.Text = ""
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(4, 108)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(128, 16)
        Me.Label21.TabIndex = 64
        Me.Label21.Text = " Account Number/SSN:"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnLPPClear
        '
        Me.btnLPPClear.Location = New System.Drawing.Point(328, 232)
        Me.btnLPPClear.Name = "btnLPPClear"
        Me.btnLPPClear.TabIndex = 7
        Me.btnLPPClear.Text = "Clear"
        '
        'Label23
        '
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(8, 16)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(608, 23)
        Me.Label23.TabIndex = 62
        Me.Label23.Text = "LPP Payment"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnLPPProcess
        '
        Me.btnLPPProcess.Location = New System.Drawing.Point(224, 232)
        Me.btnLPPProcess.Name = "btnLPPProcess"
        Me.btnLPPProcess.TabIndex = 6
        Me.btnLPPProcess.Text = "Process"
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(56, 200)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(80, 16)
        Me.Label19.TabIndex = 60
        Me.Label19.Text = "Payment Amt:"
        '
        'tbLPPPA
        '
        Me.tbLPPPA.Location = New System.Drawing.Point(136, 192)
        Me.tbLPPPA.Name = "tbLPPPA"
        Me.tbLPPPA.Size = New System.Drawing.Size(148, 20)
        Me.tbLPPPA.TabIndex = 5
        Me.tbLPPPA.Text = "0.00"
        Me.tbLPPPA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(8, 60)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(72, 16)
        Me.Label20.TabIndex = 59
        Me.Label20.Text = "Received By:"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbLPPRB
        '
        Me.tbLPPRB.Location = New System.Drawing.Point(136, 56)
        Me.tbLPPRB.Name = "tbLPPRB"
        Me.tbLPPRB.Size = New System.Drawing.Size(152, 20)
        Me.tbLPPRB.TabIndex = 1
        Me.tbLPPRB.Text = ""
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.tbLPPCN)
        Me.GroupBox2.Controls.Add(Me.rbLPPCheck)
        Me.GroupBox2.Controls.Add(Me.rbLPPCash)
        Me.GroupBox2.Controls.Add(Me.lblLPPCN)
        Me.GroupBox2.Location = New System.Drawing.Point(32, 136)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(360, 48)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Payment Method"
        '
        'tbLPPCN
        '
        Me.tbLPPCN.Enabled = False
        Me.tbLPPCN.Location = New System.Drawing.Point(248, 20)
        Me.tbLPPCN.MaxLength = 20
        Me.tbLPPCN.Name = "tbLPPCN"
        Me.tbLPPCN.TabIndex = 2
        Me.tbLPPCN.Text = ""
        '
        'rbLPPCheck
        '
        Me.rbLPPCheck.Location = New System.Drawing.Point(80, 20)
        Me.rbLPPCheck.Name = "rbLPPCheck"
        Me.rbLPPCheck.Size = New System.Drawing.Size(64, 24)
        Me.rbLPPCheck.TabIndex = 1
        Me.rbLPPCheck.TabStop = True
        Me.rbLPPCheck.Text = "Check"
        '
        'rbLPPCash
        '
        Me.rbLPPCash.Location = New System.Drawing.Point(16, 20)
        Me.rbLPPCash.Name = "rbLPPCash"
        Me.rbLPPCash.Size = New System.Drawing.Size(64, 24)
        Me.rbLPPCash.TabIndex = 0
        Me.rbLPPCash.TabStop = True
        Me.rbLPPCash.Text = "Cash"
        '
        'lblLPPCN
        '
        Me.lblLPPCN.Enabled = False
        Me.lblLPPCN.Location = New System.Drawing.Point(160, 24)
        Me.lblLPPCN.Name = "lblLPPCN"
        Me.lblLPPCN.Size = New System.Drawing.Size(88, 16)
        Me.lblLPPCN.TabIndex = 5
        Me.lblLPPCN.Text = "Check Number:"
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(4, 84)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(88, 16)
        Me.Label22.TabIndex = 56
        Me.Label22.Text = " Received From:"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbLPPBN
        '
        Me.tbLPPBN.Location = New System.Drawing.Point(136, 80)
        Me.tbLPPBN.MaxLength = 50
        Me.tbLPPBN.Name = "tbLPPBN"
        Me.tbLPPBN.Size = New System.Drawing.Size(256, 20)
        Me.tbLPPBN.TabIndex = 2
        Me.tbLPPBN.Text = ""
        '
        'TabTILP
        '
        Me.TabTILP.Controls.Add(Me.Label52)
        Me.TabTILP.Controls.Add(Me.tbTILPAN)
        Me.TabTILP.Controls.Add(Me.Label53)
        Me.TabTILP.Controls.Add(Me.btnTILPClear)
        Me.TabTILP.Controls.Add(Me.Label54)
        Me.TabTILP.Controls.Add(Me.Label55)
        Me.TabTILP.Controls.Add(Me.tbTILPPA)
        Me.TabTILP.Controls.Add(Me.Label56)
        Me.TabTILP.Controls.Add(Me.tbTILPRB)
        Me.TabTILP.Controls.Add(Me.GroupBox1)
        Me.TabTILP.Controls.Add(Me.tbTILPBN)
        Me.TabTILP.Controls.Add(Me.btnTILPProcess)
        Me.TabTILP.Location = New System.Drawing.Point(4, 22)
        Me.TabTILP.Name = "TabTILP"
        Me.TabTILP.Size = New System.Drawing.Size(624, 566)
        Me.TabTILP.TabIndex = 6
        Me.TabTILP.Text = "TILP"
        '
        'Label52
        '
        Me.Label52.Location = New System.Drawing.Point(8, 80)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(88, 16)
        Me.Label52.TabIndex = 101
        Me.Label52.Text = " Received From:"
        Me.Label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTILPAN
        '
        Me.tbTILPAN.Location = New System.Drawing.Point(136, 96)
        Me.tbTILPAN.MaxLength = 10
        Me.tbTILPAN.Name = "tbTILPAN"
        Me.tbTILPAN.Size = New System.Drawing.Size(128, 20)
        Me.tbTILPAN.TabIndex = 92
        Me.tbTILPAN.Text = ""
        '
        'Label53
        '
        Me.Label53.Location = New System.Drawing.Point(8, 104)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(128, 16)
        Me.Label53.TabIndex = 100
        Me.Label53.Text = " Account Number/SSN:"
        Me.Label53.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnTILPClear
        '
        Me.btnTILPClear.Location = New System.Drawing.Point(328, 224)
        Me.btnTILPClear.Name = "btnTILPClear"
        Me.btnTILPClear.TabIndex = 96
        Me.btnTILPClear.Text = "Clear"
        '
        'Label54
        '
        Me.Label54.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.ForeColor = System.Drawing.Color.Red
        Me.Label54.Location = New System.Drawing.Point(8, 8)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(608, 23)
        Me.Label54.TabIndex = 99
        Me.Label54.Text = "TILP Payment"
        Me.Label54.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label55
        '
        Me.Label55.Location = New System.Drawing.Point(56, 192)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(80, 16)
        Me.Label55.TabIndex = 98
        Me.Label55.Text = "Payment Amt:"
        '
        'tbTILPPA
        '
        Me.tbTILPPA.Location = New System.Drawing.Point(136, 184)
        Me.tbTILPPA.Name = "tbTILPPA"
        Me.tbTILPPA.Size = New System.Drawing.Size(144, 20)
        Me.tbTILPPA.TabIndex = 94
        Me.tbTILPPA.Text = "0.00"
        Me.tbTILPPA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label56
        '
        Me.Label56.Location = New System.Drawing.Point(8, 56)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(72, 16)
        Me.Label56.TabIndex = 97
        Me.Label56.Text = "Received By:"
        Me.Label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTILPRB
        '
        Me.tbTILPRB.Location = New System.Drawing.Point(136, 48)
        Me.tbTILPRB.Name = "tbTILPRB"
        Me.tbTILPRB.Size = New System.Drawing.Size(152, 20)
        Me.tbTILPRB.TabIndex = 90
        Me.tbTILPRB.Text = ""
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.tbTILPCN)
        Me.GroupBox1.Controls.Add(Me.rbTILPCheck)
        Me.GroupBox1.Controls.Add(Me.rbTILPCash)
        Me.GroupBox1.Controls.Add(Me.lblTILPCN)
        Me.GroupBox1.Location = New System.Drawing.Point(32, 128)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(360, 48)
        Me.GroupBox1.TabIndex = 93
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Payment Method"
        '
        'tbTILPCN
        '
        Me.tbTILPCN.Enabled = False
        Me.tbTILPCN.Location = New System.Drawing.Point(248, 20)
        Me.tbTILPCN.MaxLength = 20
        Me.tbTILPCN.Name = "tbTILPCN"
        Me.tbTILPCN.TabIndex = 2
        Me.tbTILPCN.Text = ""
        '
        'rbTILPCheck
        '
        Me.rbTILPCheck.Location = New System.Drawing.Point(80, 20)
        Me.rbTILPCheck.Name = "rbTILPCheck"
        Me.rbTILPCheck.Size = New System.Drawing.Size(64, 24)
        Me.rbTILPCheck.TabIndex = 1
        Me.rbTILPCheck.TabStop = True
        Me.rbTILPCheck.Text = "Check"
        '
        'rbTILPCash
        '
        Me.rbTILPCash.Location = New System.Drawing.Point(16, 20)
        Me.rbTILPCash.Name = "rbTILPCash"
        Me.rbTILPCash.Size = New System.Drawing.Size(64, 24)
        Me.rbTILPCash.TabIndex = 0
        Me.rbTILPCash.TabStop = True
        Me.rbTILPCash.Text = "Cash"
        '
        'lblTILPCN
        '
        Me.lblTILPCN.Enabled = False
        Me.lblTILPCN.Location = New System.Drawing.Point(160, 24)
        Me.lblTILPCN.Name = "lblTILPCN"
        Me.lblTILPCN.Size = New System.Drawing.Size(88, 16)
        Me.lblTILPCN.TabIndex = 5
        Me.lblTILPCN.Text = "Check Number:"
        '
        'tbTILPBN
        '
        Me.tbTILPBN.Location = New System.Drawing.Point(136, 72)
        Me.tbTILPBN.MaxLength = 50
        Me.tbTILPBN.Name = "tbTILPBN"
        Me.tbTILPBN.Size = New System.Drawing.Size(256, 20)
        Me.tbTILPBN.TabIndex = 91
        Me.tbTILPBN.Text = ""
        '
        'btnTILPProcess
        '
        Me.btnTILPProcess.Location = New System.Drawing.Point(224, 224)
        Me.btnTILPProcess.Name = "btnTILPProcess"
        Me.btnTILPProcess.TabIndex = 95
        Me.btnTILPProcess.Text = "Process"
        '
        'TabAcctg
        '
        Me.TabAcctg.Controls.Add(Me.cbAcctgGD)
        Me.TabAcctg.Controls.Add(Me.Label29)
        Me.TabAcctg.Controls.Add(Me.tbAcctgAN)
        Me.TabAcctg.Controls.Add(Me.Label30)
        Me.TabAcctg.Controls.Add(Me.btnAcctgClear)
        Me.TabAcctg.Controls.Add(Me.Label31)
        Me.TabAcctg.Controls.Add(Me.tbAcctgPA)
        Me.TabAcctg.Controls.Add(Me.Label32)
        Me.TabAcctg.Controls.Add(Me.tbAcctgRB)
        Me.TabAcctg.Controls.Add(Me.GroupBox4)
        Me.TabAcctg.Controls.Add(Me.tbAcctgName)
        Me.TabAcctg.Controls.Add(Me.btnAcctgProcess)
        Me.TabAcctg.Controls.Add(Me.Label34)
        Me.TabAcctg.Location = New System.Drawing.Point(4, 22)
        Me.TabAcctg.Name = "TabAcctg"
        Me.TabAcctg.Size = New System.Drawing.Size(624, 566)
        Me.TabAcctg.TabIndex = 3
        Me.TabAcctg.Text = "Acctg"
        '
        'cbAcctgGD
        '
        Me.cbAcctgGD.Location = New System.Drawing.Point(504, 64)
        Me.cbAcctgGD.Name = "cbAcctgGD"
        Me.cbAcctgGD.TabIndex = 6
        Me.cbAcctgGD.Text = "Group Deposit"
        '
        'Label29
        '
        Me.Label29.Location = New System.Drawing.Point(8, 68)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(48, 16)
        Me.Label29.TabIndex = 88
        Me.Label29.Text = " Name:"
        '
        'tbAcctgAN
        '
        Me.tbAcctgAN.Location = New System.Drawing.Point(136, 88)
        Me.tbAcctgAN.MaxLength = 10
        Me.tbAcctgAN.Name = "tbAcctgAN"
        Me.tbAcctgAN.Size = New System.Drawing.Size(128, 20)
        Me.tbAcctgAN.TabIndex = 2
        Me.tbAcctgAN.Text = ""
        '
        'Label30
        '
        Me.Label30.Location = New System.Drawing.Point(8, 92)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(128, 16)
        Me.Label30.TabIndex = 87
        Me.Label30.Text = " Account Number/SSN:"
        '
        'btnAcctgClear
        '
        Me.btnAcctgClear.Location = New System.Drawing.Point(328, 232)
        Me.btnAcctgClear.Name = "btnAcctgClear"
        Me.btnAcctgClear.TabIndex = 8
        Me.btnAcctgClear.Text = "Clear"
        '
        'Label31
        '
        Me.Label31.Location = New System.Drawing.Point(12, 116)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(80, 16)
        Me.Label31.TabIndex = 86
        Me.Label31.Text = "Payment Amt:"
        '
        'tbAcctgPA
        '
        Me.tbAcctgPA.Location = New System.Drawing.Point(136, 112)
        Me.tbAcctgPA.Name = "tbAcctgPA"
        Me.tbAcctgPA.Size = New System.Drawing.Size(128, 20)
        Me.tbAcctgPA.TabIndex = 3
        Me.tbAcctgPA.Text = "0.00"
        Me.tbAcctgPA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label32
        '
        Me.Label32.Location = New System.Drawing.Point(64, 196)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(72, 16)
        Me.Label32.TabIndex = 85
        Me.Label32.Text = "Received By:"
        '
        'tbAcctgRB
        '
        Me.tbAcctgRB.Location = New System.Drawing.Point(136, 192)
        Me.tbAcctgRB.Name = "tbAcctgRB"
        Me.tbAcctgRB.Size = New System.Drawing.Size(152, 20)
        Me.tbAcctgRB.TabIndex = 5
        Me.tbAcctgRB.Text = ""
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.tbAcctgCN)
        Me.GroupBox4.Controls.Add(Me.rbAcctgCheck)
        Me.GroupBox4.Controls.Add(Me.rbAcctgCash)
        Me.GroupBox4.Controls.Add(Me.lblAcctgCN)
        Me.GroupBox4.Location = New System.Drawing.Point(32, 136)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(360, 48)
        Me.GroupBox4.TabIndex = 4
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Payment Method"
        '
        'tbAcctgCN
        '
        Me.tbAcctgCN.Enabled = False
        Me.tbAcctgCN.Location = New System.Drawing.Point(248, 20)
        Me.tbAcctgCN.MaxLength = 20
        Me.tbAcctgCN.Name = "tbAcctgCN"
        Me.tbAcctgCN.TabIndex = 2
        Me.tbAcctgCN.Text = ""
        '
        'rbAcctgCheck
        '
        Me.rbAcctgCheck.Checked = True
        Me.rbAcctgCheck.Location = New System.Drawing.Point(80, 20)
        Me.rbAcctgCheck.Name = "rbAcctgCheck"
        Me.rbAcctgCheck.Size = New System.Drawing.Size(64, 24)
        Me.rbAcctgCheck.TabIndex = 1
        Me.rbAcctgCheck.TabStop = True
        Me.rbAcctgCheck.Text = "Check"
        '
        'rbAcctgCash
        '
        Me.rbAcctgCash.Location = New System.Drawing.Point(16, 20)
        Me.rbAcctgCash.Name = "rbAcctgCash"
        Me.rbAcctgCash.Size = New System.Drawing.Size(64, 24)
        Me.rbAcctgCash.TabIndex = 0
        Me.rbAcctgCash.TabStop = True
        Me.rbAcctgCash.Text = "Cash"
        '
        'lblAcctgCN
        '
        Me.lblAcctgCN.Location = New System.Drawing.Point(160, 24)
        Me.lblAcctgCN.Name = "lblAcctgCN"
        Me.lblAcctgCN.Size = New System.Drawing.Size(88, 16)
        Me.lblAcctgCN.TabIndex = 5
        Me.lblAcctgCN.Text = "Check Number:"
        '
        'tbAcctgName
        '
        Me.tbAcctgName.Location = New System.Drawing.Point(136, 64)
        Me.tbAcctgName.MaxLength = 50
        Me.tbAcctgName.Name = "tbAcctgName"
        Me.tbAcctgName.Size = New System.Drawing.Size(256, 20)
        Me.tbAcctgName.TabIndex = 1
        Me.tbAcctgName.Text = ""
        '
        'btnAcctgProcess
        '
        Me.btnAcctgProcess.Location = New System.Drawing.Point(224, 232)
        Me.btnAcctgProcess.Name = "btnAcctgProcess"
        Me.btnAcctgProcess.TabIndex = 7
        Me.btnAcctgProcess.Text = "Process"
        '
        'Label34
        '
        Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.ForeColor = System.Drawing.Color.Red
        Me.Label34.Location = New System.Drawing.Point(8, 16)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(608, 23)
        Me.Label34.TabIndex = 76
        Me.Label34.Text = "Accounting"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabVoidR
        '
        Me.TabVoidR.Controls.Add(Me.Label37)
        Me.TabVoidR.Controls.Add(Me.Label36)
        Me.TabVoidR.Controls.Add(Me.Label35)
        Me.TabVoidR.Controls.Add(Me.btnOK)
        Me.TabVoidR.Controls.Add(Me.cbPrgm)
        Me.TabVoidR.Controls.Add(Me.tbRecNum)
        Me.TabVoidR.Controls.Add(Me.tbNotes)
        Me.TabVoidR.Controls.Add(Me.Label33)
        Me.TabVoidR.Location = New System.Drawing.Point(4, 22)
        Me.TabVoidR.Name = "TabVoidR"
        Me.TabVoidR.Size = New System.Drawing.Size(624, 566)
        Me.TabVoidR.TabIndex = 4
        Me.TabVoidR.Text = "Void Receipts"
        '
        'Label37
        '
        Me.Label37.Location = New System.Drawing.Point(32, 116)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(100, 16)
        Me.Label37.TabIndex = 11
        Me.Label37.Text = "Reason:"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label36
        '
        Me.Label36.Location = New System.Drawing.Point(32, 92)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(100, 16)
        Me.Label36.TabIndex = 10
        Me.Label36.Text = "Receipt #:"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label35
        '
        Me.Label35.Location = New System.Drawing.Point(32, 68)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(100, 16)
        Me.Label35.TabIndex = 9
        Me.Label35.Text = "Program:"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(280, 232)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        '
        'cbPrgm
        '
        Me.cbPrgm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrgm.Items.AddRange(New Object() {"LGP", "LPP", "UESP", "TILP"})
        Me.cbPrgm.Location = New System.Drawing.Point(136, 64)
        Me.cbPrgm.Name = "cbPrgm"
        Me.cbPrgm.Size = New System.Drawing.Size(128, 21)
        Me.cbPrgm.TabIndex = 0
        '
        'tbRecNum
        '
        Me.tbRecNum.Location = New System.Drawing.Point(136, 88)
        Me.tbRecNum.Name = "tbRecNum"
        Me.tbRecNum.Size = New System.Drawing.Size(128, 20)
        Me.tbRecNum.TabIndex = 1
        Me.tbRecNum.Text = ""
        '
        'tbNotes
        '
        Me.tbNotes.Location = New System.Drawing.Point(136, 112)
        Me.tbNotes.MaxLength = 100
        Me.tbNotes.Multiline = True
        Me.tbNotes.Name = "tbNotes"
        Me.tbNotes.Size = New System.Drawing.Size(232, 104)
        Me.tbNotes.TabIndex = 2
        Me.tbNotes.Text = ""
        '
        'Label33
        '
        Me.Label33.BackColor = System.Drawing.SystemColors.Control
        Me.Label33.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.Color.Red
        Me.Label33.Location = New System.Drawing.Point(8, 16)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(608, 23)
        Me.Label33.TabIndex = 4
        Me.Label33.Text = "Void A Receipt"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabReports
        '
        Me.TabReports.Controls.Add(Me.GroupBox7)
        Me.TabReports.Controls.Add(Me.GroupBox6)
        Me.TabReports.Controls.Add(Me.Label38)
        Me.TabReports.Controls.Add(Me.GroupBox5)
        Me.TabReports.Controls.Add(Me.GroupBox14)
        Me.TabReports.Location = New System.Drawing.Point(4, 22)
        Me.TabReports.Name = "TabReports"
        Me.TabReports.Size = New System.Drawing.Size(624, 566)
        Me.TabReports.TabIndex = 5
        Me.TabReports.Text = "Reports"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.GroupBox13)
        Me.GroupBox7.Controls.Add(Me.GroupBox10)
        Me.GroupBox7.Location = New System.Drawing.Point(416, 48)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(200, 248)
        Me.GroupBox7.TabIndex = 6
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "UESP Reporting"
        '
        'GroupBox13
        '
        Me.GroupBox13.Controls.Add(Me.btnVRptUESP)
        Me.GroupBox13.Controls.Add(Me.VUESPED)
        Me.GroupBox13.Controls.Add(Me.Label49)
        Me.GroupBox13.Controls.Add(Me.Label50)
        Me.GroupBox13.Controls.Add(Me.VUESPSD)
        Me.GroupBox13.Location = New System.Drawing.Point(8, 140)
        Me.GroupBox13.Name = "GroupBox13"
        Me.GroupBox13.Size = New System.Drawing.Size(184, 100)
        Me.GroupBox13.TabIndex = 10
        Me.GroupBox13.TabStop = False
        Me.GroupBox13.Text = "Voids"
        '
        'btnVRptUESP
        '
        Me.btnVRptUESP.Location = New System.Drawing.Point(52, 68)
        Me.btnVRptUESP.Name = "btnVRptUESP"
        Me.btnVRptUESP.TabIndex = 3
        Me.btnVRptUESP.Text = "Print"
        '
        'VUESPED
        '
        Me.VUESPED.CustomFormat = "MM/dd/yyyy"
        Me.VUESPED.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.VUESPED.Location = New System.Drawing.Point(68, 44)
        Me.VUESPED.Name = "VUESPED"
        Me.VUESPED.Size = New System.Drawing.Size(108, 20)
        Me.VUESPED.TabIndex = 2
        '
        'Label49
        '
        Me.Label49.Location = New System.Drawing.Point(4, 20)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(60, 16)
        Me.Label49.TabIndex = 1
        Me.Label49.Text = "Start Date:"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label50
        '
        Me.Label50.Location = New System.Drawing.Point(4, 44)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(60, 16)
        Me.Label50.TabIndex = 0
        Me.Label50.Text = "End Date:"
        Me.Label50.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'VUESPSD
        '
        Me.VUESPSD.CustomFormat = "MM/dd/yyyy"
        Me.VUESPSD.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.VUESPSD.Location = New System.Drawing.Point(68, 20)
        Me.VUESPSD.Name = "VUESPSD"
        Me.VUESPSD.Size = New System.Drawing.Size(108, 20)
        Me.VUESPSD.TabIndex = 0
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.Label44)
        Me.GroupBox10.Controls.Add(Me.UESPSD)
        Me.GroupBox10.Controls.Add(Me.btnRptUESP)
        Me.GroupBox10.Controls.Add(Me.UESPED)
        Me.GroupBox10.Controls.Add(Me.Label43)
        Me.GroupBox10.Location = New System.Drawing.Point(8, 28)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(184, 100)
        Me.GroupBox10.TabIndex = 9
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Payments"
        '
        'Label44
        '
        Me.Label44.Location = New System.Drawing.Point(8, 48)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(60, 16)
        Me.Label44.TabIndex = 3
        Me.Label44.Text = "End Date:"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'UESPSD
        '
        Me.UESPSD.CustomFormat = "MM/dd/yyyy"
        Me.UESPSD.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.UESPSD.Location = New System.Drawing.Point(72, 20)
        Me.UESPSD.Name = "UESPSD"
        Me.UESPSD.Size = New System.Drawing.Size(108, 20)
        Me.UESPSD.TabIndex = 4
        '
        'btnRptUESP
        '
        Me.btnRptUESP.Location = New System.Drawing.Point(56, 68)
        Me.btnRptUESP.Name = "btnRptUESP"
        Me.btnRptUESP.TabIndex = 7
        Me.btnRptUESP.Text = "Print"
        '
        'UESPED
        '
        Me.UESPED.CustomFormat = "MM/dd/yyyy"
        Me.UESPED.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.UESPED.Location = New System.Drawing.Point(72, 44)
        Me.UESPED.Name = "UESPED"
        Me.UESPED.Size = New System.Drawing.Size(108, 20)
        Me.UESPED.TabIndex = 6
        '
        'Label43
        '
        Me.Label43.Location = New System.Drawing.Point(8, 24)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(60, 16)
        Me.Label43.TabIndex = 5
        Me.Label43.Text = "Start Date:"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.GroupBox12)
        Me.GroupBox6.Controls.Add(Me.GroupBox9)
        Me.GroupBox6.Location = New System.Drawing.Point(212, 48)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(200, 248)
        Me.GroupBox6.TabIndex = 5
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "LGP Reporting"
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.btnVRptLGP)
        Me.GroupBox12.Controls.Add(Me.VLGPED)
        Me.GroupBox12.Controls.Add(Me.Label47)
        Me.GroupBox12.Controls.Add(Me.Label48)
        Me.GroupBox12.Controls.Add(Me.VLGPSD)
        Me.GroupBox12.Location = New System.Drawing.Point(8, 140)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(184, 100)
        Me.GroupBox12.TabIndex = 10
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Voids"
        '
        'btnVRptLGP
        '
        Me.btnVRptLGP.Location = New System.Drawing.Point(52, 68)
        Me.btnVRptLGP.Name = "btnVRptLGP"
        Me.btnVRptLGP.TabIndex = 3
        Me.btnVRptLGP.Text = "Print"
        '
        'VLGPED
        '
        Me.VLGPED.CustomFormat = "MM/dd/yyyy"
        Me.VLGPED.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.VLGPED.Location = New System.Drawing.Point(68, 44)
        Me.VLGPED.Name = "VLGPED"
        Me.VLGPED.Size = New System.Drawing.Size(108, 20)
        Me.VLGPED.TabIndex = 2
        '
        'Label47
        '
        Me.Label47.Location = New System.Drawing.Point(4, 20)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(60, 16)
        Me.Label47.TabIndex = 1
        Me.Label47.Text = "Start Date:"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label48
        '
        Me.Label48.Location = New System.Drawing.Point(4, 44)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(60, 16)
        Me.Label48.TabIndex = 0
        Me.Label48.Text = "End Date:"
        Me.Label48.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'VLGPSD
        '
        Me.VLGPSD.CustomFormat = "MM/dd/yyyy"
        Me.VLGPSD.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.VLGPSD.Location = New System.Drawing.Point(68, 20)
        Me.VLGPSD.Name = "VLGPSD"
        Me.VLGPSD.Size = New System.Drawing.Size(108, 20)
        Me.VLGPSD.TabIndex = 0
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.btnRptLGP)
        Me.GroupBox9.Controls.Add(Me.LGPED)
        Me.GroupBox9.Controls.Add(Me.Label41)
        Me.GroupBox9.Controls.Add(Me.Label42)
        Me.GroupBox9.Controls.Add(Me.LGPSD)
        Me.GroupBox9.Location = New System.Drawing.Point(8, 28)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(184, 100)
        Me.GroupBox9.TabIndex = 9
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Payments"
        '
        'btnRptLGP
        '
        Me.btnRptLGP.Location = New System.Drawing.Point(52, 68)
        Me.btnRptLGP.Name = "btnRptLGP"
        Me.btnRptLGP.TabIndex = 7
        Me.btnRptLGP.Text = "Print"
        '
        'LGPED
        '
        Me.LGPED.CustomFormat = "MM/dd/yyyy"
        Me.LGPED.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.LGPED.Location = New System.Drawing.Point(64, 44)
        Me.LGPED.Name = "LGPED"
        Me.LGPED.Size = New System.Drawing.Size(108, 20)
        Me.LGPED.TabIndex = 6
        '
        'Label41
        '
        Me.Label41.Location = New System.Drawing.Point(4, 20)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(60, 16)
        Me.Label41.TabIndex = 5
        Me.Label41.Text = "Start Date:"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label42
        '
        Me.Label42.Location = New System.Drawing.Point(4, 44)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(60, 16)
        Me.Label42.TabIndex = 3
        Me.Label42.Text = "End Date:"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LGPSD
        '
        Me.LGPSD.CustomFormat = "MM/dd/yyyy"
        Me.LGPSD.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.LGPSD.Location = New System.Drawing.Point(64, 20)
        Me.LGPSD.Name = "LGPSD"
        Me.LGPSD.Size = New System.Drawing.Size(108, 20)
        Me.LGPSD.TabIndex = 4
        '
        'Label38
        '
        Me.Label38.BackColor = System.Drawing.SystemColors.Control
        Me.Label38.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.ForeColor = System.Drawing.Color.Red
        Me.Label38.Location = New System.Drawing.Point(8, 16)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(608, 23)
        Me.Label38.TabIndex = 4
        Me.Label38.Text = "Reports"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.GroupBox11)
        Me.GroupBox5.Controls.Add(Me.GroupBox8)
        Me.GroupBox5.Location = New System.Drawing.Point(8, 48)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(200, 248)
        Me.GroupBox5.TabIndex = 0
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "LPP Reporting"
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.btnVRptLPP)
        Me.GroupBox11.Controls.Add(Me.VLPPED)
        Me.GroupBox11.Controls.Add(Me.Label45)
        Me.GroupBox11.Controls.Add(Me.Label46)
        Me.GroupBox11.Controls.Add(Me.VLPPSD)
        Me.GroupBox11.Location = New System.Drawing.Point(8, 140)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(184, 100)
        Me.GroupBox11.TabIndex = 9
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Voids"
        '
        'btnVRptLPP
        '
        Me.btnVRptLPP.Location = New System.Drawing.Point(52, 68)
        Me.btnVRptLPP.Name = "btnVRptLPP"
        Me.btnVRptLPP.TabIndex = 3
        Me.btnVRptLPP.Text = "Print"
        '
        'VLPPED
        '
        Me.VLPPED.CustomFormat = "MM/dd/yyyy"
        Me.VLPPED.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.VLPPED.Location = New System.Drawing.Point(68, 44)
        Me.VLPPED.Name = "VLPPED"
        Me.VLPPED.Size = New System.Drawing.Size(108, 20)
        Me.VLPPED.TabIndex = 2
        '
        'Label45
        '
        Me.Label45.Location = New System.Drawing.Point(4, 20)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(60, 16)
        Me.Label45.TabIndex = 1
        Me.Label45.Text = "Start Date:"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label46
        '
        Me.Label46.Location = New System.Drawing.Point(4, 44)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(60, 16)
        Me.Label46.TabIndex = 0
        Me.Label46.Text = "End Date:"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'VLPPSD
        '
        Me.VLPPSD.CustomFormat = "MM/dd/yyyy"
        Me.VLPPSD.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.VLPPSD.Location = New System.Drawing.Point(68, 20)
        Me.VLPPSD.Name = "VLPPSD"
        Me.VLPPSD.Size = New System.Drawing.Size(108, 20)
        Me.VLPPSD.TabIndex = 0
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.btnRptLPP)
        Me.GroupBox8.Controls.Add(Me.LPPED)
        Me.GroupBox8.Controls.Add(Me.Label40)
        Me.GroupBox8.Controls.Add(Me.Label39)
        Me.GroupBox8.Controls.Add(Me.LPPSD)
        Me.GroupBox8.Location = New System.Drawing.Point(8, 28)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(184, 100)
        Me.GroupBox8.TabIndex = 8
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Payments"
        '
        'btnRptLPP
        '
        Me.btnRptLPP.Location = New System.Drawing.Point(52, 68)
        Me.btnRptLPP.Name = "btnRptLPP"
        Me.btnRptLPP.TabIndex = 3
        Me.btnRptLPP.Text = "Print"
        '
        'LPPED
        '
        Me.LPPED.CustomFormat = "MM/dd/yyyy"
        Me.LPPED.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.LPPED.Location = New System.Drawing.Point(68, 44)
        Me.LPPED.Name = "LPPED"
        Me.LPPED.Size = New System.Drawing.Size(108, 20)
        Me.LPPED.TabIndex = 2
        '
        'Label40
        '
        Me.Label40.Location = New System.Drawing.Point(4, 20)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(60, 16)
        Me.Label40.TabIndex = 1
        Me.Label40.Text = "Start Date:"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label39
        '
        Me.Label39.Location = New System.Drawing.Point(4, 44)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(60, 16)
        Me.Label39.TabIndex = 0
        Me.Label39.Text = "End Date:"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LPPSD
        '
        Me.LPPSD.CustomFormat = "MM/dd/yyyy"
        Me.LPPSD.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.LPPSD.Location = New System.Drawing.Point(68, 20)
        Me.LPPSD.Name = "LPPSD"
        Me.LPPSD.Size = New System.Drawing.Size(108, 20)
        Me.LPPSD.TabIndex = 0
        '
        'GroupBox14
        '
        Me.GroupBox14.Controls.Add(Me.GroupBox15)
        Me.GroupBox14.Controls.Add(Me.GroupBox16)
        Me.GroupBox14.Location = New System.Drawing.Point(8, 304)
        Me.GroupBox14.Name = "GroupBox14"
        Me.GroupBox14.Size = New System.Drawing.Size(200, 248)
        Me.GroupBox14.TabIndex = 10
        Me.GroupBox14.TabStop = False
        Me.GroupBox14.Text = "TILP Reporting"
        '
        'GroupBox15
        '
        Me.GroupBox15.Controls.Add(Me.btnVRptTILP)
        Me.GroupBox15.Controls.Add(Me.VTILPED)
        Me.GroupBox15.Controls.Add(Me.Label57)
        Me.GroupBox15.Controls.Add(Me.Label58)
        Me.GroupBox15.Controls.Add(Me.VTILPSD)
        Me.GroupBox15.Location = New System.Drawing.Point(8, 140)
        Me.GroupBox15.Name = "GroupBox15"
        Me.GroupBox15.Size = New System.Drawing.Size(184, 100)
        Me.GroupBox15.TabIndex = 9
        Me.GroupBox15.TabStop = False
        Me.GroupBox15.Text = "Voids"
        '
        'btnVRptTILP
        '
        Me.btnVRptTILP.Location = New System.Drawing.Point(52, 68)
        Me.btnVRptTILP.Name = "btnVRptTILP"
        Me.btnVRptTILP.TabIndex = 3
        Me.btnVRptTILP.Text = "Print"
        '
        'VTILPED
        '
        Me.VTILPED.CustomFormat = "MM/dd/yyyy"
        Me.VTILPED.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.VTILPED.Location = New System.Drawing.Point(68, 44)
        Me.VTILPED.Name = "VTILPED"
        Me.VTILPED.Size = New System.Drawing.Size(108, 20)
        Me.VTILPED.TabIndex = 2
        '
        'Label57
        '
        Me.Label57.Location = New System.Drawing.Point(4, 20)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(60, 16)
        Me.Label57.TabIndex = 1
        Me.Label57.Text = "Start Date:"
        Me.Label57.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label58
        '
        Me.Label58.Location = New System.Drawing.Point(4, 44)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(60, 16)
        Me.Label58.TabIndex = 0
        Me.Label58.Text = "End Date:"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'VTILPSD
        '
        Me.VTILPSD.CustomFormat = "MM/dd/yyyy"
        Me.VTILPSD.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.VTILPSD.Location = New System.Drawing.Point(68, 20)
        Me.VTILPSD.Name = "VTILPSD"
        Me.VTILPSD.Size = New System.Drawing.Size(108, 20)
        Me.VTILPSD.TabIndex = 0
        '
        'GroupBox16
        '
        Me.GroupBox16.Controls.Add(Me.btnRptTILP)
        Me.GroupBox16.Controls.Add(Me.TILPED)
        Me.GroupBox16.Controls.Add(Me.Label59)
        Me.GroupBox16.Controls.Add(Me.Label60)
        Me.GroupBox16.Controls.Add(Me.TILPSD)
        Me.GroupBox16.Location = New System.Drawing.Point(8, 28)
        Me.GroupBox16.Name = "GroupBox16"
        Me.GroupBox16.Size = New System.Drawing.Size(184, 100)
        Me.GroupBox16.TabIndex = 8
        Me.GroupBox16.TabStop = False
        Me.GroupBox16.Text = "Payments"
        '
        'btnRptTILP
        '
        Me.btnRptTILP.Location = New System.Drawing.Point(52, 68)
        Me.btnRptTILP.Name = "btnRptTILP"
        Me.btnRptTILP.TabIndex = 3
        Me.btnRptTILP.Text = "Print"
        '
        'TILPED
        '
        Me.TILPED.CustomFormat = "MM/dd/yyyy"
        Me.TILPED.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.TILPED.Location = New System.Drawing.Point(68, 44)
        Me.TILPED.Name = "TILPED"
        Me.TILPED.Size = New System.Drawing.Size(108, 20)
        Me.TILPED.TabIndex = 2
        '
        'Label59
        '
        Me.Label59.Location = New System.Drawing.Point(4, 20)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(60, 16)
        Me.Label59.TabIndex = 1
        Me.Label59.Text = "Start Date:"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label60
        '
        Me.Label60.Location = New System.Drawing.Point(4, 44)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(60, 16)
        Me.Label60.TabIndex = 0
        Me.Label60.Text = "End Date:"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TILPSD
        '
        Me.TILPSD.CustomFormat = "MM/dd/yyyy"
        Me.TILPSD.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.TILPSD.Location = New System.Drawing.Point(68, 20)
        Me.TILPSD.Name = "TILPSD"
        Me.TILPSD.Size = New System.Drawing.Size(108, 20)
        Me.TILPSD.TabIndex = 0
        '
        'btnReprint
        '
        Me.btnReprint.Location = New System.Drawing.Point(512, 608)
        Me.btnReprint.Name = "btnReprint"
        Me.btnReprint.Size = New System.Drawing.Size(120, 23)
        Me.btnReprint.TabIndex = 1
        Me.btnReprint.Text = "Reprint Last Receipt"
        '
        'TestDB
        '
        Me.TestDB.ConnectionString = "workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""" & _
        "BART\BART"";persist security info=False;initial catalog=ReceiptBook"
        '
        'LiveDB
        '
        Me.LiveDB.ConnectionString = "workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=N" & _
        "OCHOUSE;persist security info=False;initial catalog=ReceiptBook"
        '
        'TstLGPPayments
        '
        Me.TstLGPPayments.SelectCommand = Me.SqlSelectCommand1
        Me.TstLGPPayments.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "LGPPayments", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Name", "Name"), New System.Data.Common.DataColumnMapping("AccountNumber", "AccountNumber"), New System.Data.Common.DataColumnMapping("ReceivedBy", "ReceivedBy"), New System.Data.Common.DataColumnMapping("PaymentDate", "PaymentDate"), New System.Data.Common.DataColumnMapping("PaymentAmount", "PaymentAmount"), New System.Data.Common.DataColumnMapping("PaymentType", "PaymentType"), New System.Data.Common.DataColumnMapping("CheckNumber", "CheckNumber"), New System.Data.Common.DataColumnMapping("Voided", "Voided"), New System.Data.Common.DataColumnMapping("Notes", "Notes")})})
        '
        'SqlSelectCommand1
        '
        Me.SqlSelectCommand1.CommandText = "SELECT LGPPayments.ReceiptNumber, LGPPayments.Name, LGPPayments.AccountNumber, LG" & _
        "PPayments.ReceivedBy, LGPPayments.PaymentDate, LGPPayments.PaymentAmount, LGPPay" & _
        "ments.PaymentType, LGPPayments.CheckNumber, CASE WHEN LGPVoids.ReceiptNumber IS " & _
        "NULL THEN ' ' ELSE 'VOIDED' END AS Voided, LGPVoids.Notes FROM LGPPayments LEFT " & _
        "OUTER JOIN LGPVoids ON LGPPayments.ReceiptNumber = LGPVoids.ReceiptNumber WHERE " & _
        "(LGPPayments.PaymentDate >= @GTDate) AND (LGPPayments.PaymentDate <= @LTDate)"
        Me.SqlSelectCommand1.Connection = Me.TestDB
        Me.SqlSelectCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        Me.SqlSelectCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        '
        'TstDSLGPPayments
        '
        Me.TstDSLGPPayments.DataSetName = "DSLGPPayments"
        Me.TstDSLGPPayments.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'LivLGPPayments
        '
        Me.LivLGPPayments.SelectCommand = Me.SqlSelectCommand2
        Me.LivLGPPayments.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "LGPPayments", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Name", "Name"), New System.Data.Common.DataColumnMapping("AccountNumber", "AccountNumber"), New System.Data.Common.DataColumnMapping("ReceivedBy", "ReceivedBy"), New System.Data.Common.DataColumnMapping("PaymentDate", "PaymentDate"), New System.Data.Common.DataColumnMapping("PaymentAmount", "PaymentAmount"), New System.Data.Common.DataColumnMapping("PaymentType", "PaymentType"), New System.Data.Common.DataColumnMapping("CheckNumber", "CheckNumber"), New System.Data.Common.DataColumnMapping("Voided", "Voided"), New System.Data.Common.DataColumnMapping("Notes", "Notes")})})
        '
        'SqlSelectCommand2
        '
        Me.SqlSelectCommand2.CommandText = "SELECT dbo.LGPPayments.ReceiptNumber, dbo.LGPPayments.Name, dbo.LGPPayments.Accou" & _
        "ntNumber, dbo.LGPPayments.ReceivedBy, dbo.LGPPayments.PaymentDate, dbo.LGPPaymen" & _
        "ts.PaymentAmount, dbo.LGPPayments.PaymentType, dbo.LGPPayments.CheckNumber, CASE" & _
        " WHEN LGPVoids.ReceiptNumber IS NULL THEN ' ' ELSE 'VOIDED' END AS Voided, dbo.L" & _
        "GPVoids.Notes FROM dbo.LGPPayments LEFT OUTER JOIN dbo.LGPVoids ON dbo.LGPPaymen" & _
        "ts.ReceiptNumber = dbo.LGPVoids.ReceiptNumber WHERE (dbo.LGPPayments.PaymentDate" & _
        " >= @GTDate) AND (dbo.LGPPayments.PaymentDate <= @LTDate)"
        Me.SqlSelectCommand2.Connection = Me.LiveDB
        Me.SqlSelectCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        Me.SqlSelectCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        '
        'LivDSLGPPayments
        '
        Me.LivDSLGPPayments.DataSetName = "LivDSLGPPayments"
        Me.LivDSLGPPayments.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'TstLPPPayments
        '
        Me.TstLPPPayments.SelectCommand = Me.SqlSelectCommand3
        Me.TstLPPPayments.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "LPPPayments", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Name", "Name"), New System.Data.Common.DataColumnMapping("AccountNumber", "AccountNumber"), New System.Data.Common.DataColumnMapping("ReceivedBy", "ReceivedBy"), New System.Data.Common.DataColumnMapping("PaymentDate", "PaymentDate"), New System.Data.Common.DataColumnMapping("PaymentAmount", "PaymentAmount"), New System.Data.Common.DataColumnMapping("PaymentType", "PaymentType"), New System.Data.Common.DataColumnMapping("CheckNumber", "CheckNumber"), New System.Data.Common.DataColumnMapping("Voided", "Voided"), New System.Data.Common.DataColumnMapping("Notes", "Notes")})})
        '
        'SqlSelectCommand3
        '
        Me.SqlSelectCommand3.CommandText = "SELECT LPPPayments.ReceiptNumber, LPPPayments.Name, LPPPayments.AccountNumber, LP" & _
        "PPayments.ReceivedBy, LPPPayments.PaymentDate, LPPPayments.PaymentAmount, LPPPay" & _
        "ments.PaymentType, LPPPayments.CheckNumber, CASE WHEN LPPVoids.ReceiptNumber IS " & _
        "NULL THEN ' ' ELSE 'VOIDED' END AS Voided, LPPVoids.Notes FROM LPPPayments LEFT " & _
        "OUTER JOIN LPPVoids ON LPPPayments.ReceiptNumber = LPPVoids.ReceiptNumber WHERE " & _
        "(LPPPayments.PaymentDate >= @GTDate) AND (LPPPayments.PaymentDate <= @LTDate)"
        Me.SqlSelectCommand3.Connection = Me.TestDB
        Me.SqlSelectCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        Me.SqlSelectCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        '
        'TstDSLPPPayments
        '
        Me.TstDSLPPPayments.DataSetName = "TstDSLGPPayments"
        Me.TstDSLPPPayments.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'LivLPPPayments
        '
        Me.LivLPPPayments.SelectCommand = Me.SqlSelectCommand4
        Me.LivLPPPayments.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "LPPPayments", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Name", "Name"), New System.Data.Common.DataColumnMapping("AccountNumber", "AccountNumber"), New System.Data.Common.DataColumnMapping("ReceivedBy", "ReceivedBy"), New System.Data.Common.DataColumnMapping("PaymentDate", "PaymentDate"), New System.Data.Common.DataColumnMapping("PaymentAmount", "PaymentAmount"), New System.Data.Common.DataColumnMapping("PaymentType", "PaymentType"), New System.Data.Common.DataColumnMapping("CheckNumber", "CheckNumber"), New System.Data.Common.DataColumnMapping("Voided", "Voided"), New System.Data.Common.DataColumnMapping("Notes", "Notes")})})
        '
        'SqlSelectCommand4
        '
        Me.SqlSelectCommand4.CommandText = "SELECT dbo.LPPPayments.ReceiptNumber, dbo.LPPPayments.Name, dbo.LPPPayments.Accou" & _
        "ntNumber, dbo.LPPPayments.ReceivedBy, dbo.LPPPayments.PaymentDate, dbo.LPPPaymen" & _
        "ts.PaymentAmount, dbo.LPPPayments.PaymentType, dbo.LPPPayments.CheckNumber, CASE" & _
        " WHEN LPPVoids.ReceiptNumber IS NULL THEN ' ' ELSE 'VOIDED' END AS Voided, dbo.L" & _
        "PPVoids.Notes FROM dbo.LPPPayments LEFT OUTER JOIN dbo.LPPVoids ON dbo.LPPPaymen" & _
        "ts.ReceiptNumber = dbo.LPPVoids.ReceiptNumber WHERE (dbo.LPPPayments.PaymentDate" & _
        " >= @GTDate) AND (dbo.LPPPayments.PaymentDate <= @LTDate)"
        Me.SqlSelectCommand4.Connection = Me.LiveDB
        Me.SqlSelectCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        Me.SqlSelectCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        '
        'LivDSLPPPayments
        '
        Me.LivDSLPPPayments.DataSetName = "LIVDSLPPPayments"
        Me.LivDSLPPPayments.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'TstLPPVoids
        '
        Me.TstLPPVoids.DeleteCommand = Me.SqlDeleteCommand1
        Me.TstLPPVoids.InsertCommand = Me.SqlInsertCommand1
        Me.TstLPPVoids.SelectCommand = Me.SqlSelectCommand7
        Me.TstLPPVoids.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "LPPVoids", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Notes", "Notes"), New System.Data.Common.DataColumnMapping("DateVoided", "DateVoided")})})
        Me.TstLPPVoids.UpdateCommand = Me.SqlUpdateCommand1
        '
        'SqlDeleteCommand1
        '
        Me.SqlDeleteCommand1.CommandText = "DELETE FROM LPPVoids WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (DateVoi" & _
        "ded = @Original_DateVoided OR @Original_DateVoided IS NULL AND DateVoided IS NUL" & _
        "L)"
        Me.SqlDeleteCommand1.Connection = Me.TestDB
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        '
        'SqlInsertCommand1
        '
        Me.SqlInsertCommand1.CommandText = "INSERT INTO LPPVoids(ReceiptNumber, Notes, DateVoided) VALUES (@ReceiptNumber, @N" & _
        "otes, @DateVoided); SELECT ReceiptNumber, Notes, DateVoided FROM LPPVoids WHERE " & _
        "(ReceiptNumber = @ReceiptNumber)"
        Me.SqlInsertCommand1.Connection = Me.TestDB
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 1073741823, "Notes"))
        Me.SqlInsertCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlSelectCommand7
        '
        Me.SqlSelectCommand7.CommandText = "SELECT ReceiptNumber, Notes, DateVoided FROM LPPVoids WHERE (DateVoided >= @GTDat" & _
        "e) AND (DateVoided <= @LTDate)"
        Me.SqlSelectCommand7.Connection = Me.TestDB
        Me.SqlSelectCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlSelectCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlUpdateCommand1
        '
        Me.SqlUpdateCommand1.CommandText = "UPDATE LPPVoids SET ReceiptNumber = @ReceiptNumber, Notes = @Notes, DateVoided = " & _
        "@DateVoided WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (DateVoided = @O" & _
        "riginal_DateVoided OR @Original_DateVoided IS NULL AND DateVoided IS NULL); SELE" & _
        "CT ReceiptNumber, Notes, DateVoided FROM LPPVoids WHERE (ReceiptNumber = @Receip" & _
        "tNumber)"
        Me.SqlUpdateCommand1.Connection = Me.TestDB
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 1073741823, "Notes"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand1.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        '
        'TstDSLPPVoids
        '
        Me.TstDSLPPVoids.DataSetName = "TstDSLPPVoids"
        Me.TstDSLPPVoids.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'LivLPPVoids
        '
        Me.LivLPPVoids.DeleteCommand = Me.SqlDeleteCommand2
        Me.LivLPPVoids.InsertCommand = Me.SqlInsertCommand2
        Me.LivLPPVoids.SelectCommand = Me.SqlSelectCommand8
        Me.LivLPPVoids.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "LPPVoids", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Notes", "Notes"), New System.Data.Common.DataColumnMapping("DateVoided", "DateVoided")})})
        Me.LivLPPVoids.UpdateCommand = Me.SqlUpdateCommand2
        '
        'SqlDeleteCommand2
        '
        Me.SqlDeleteCommand2.CommandText = "DELETE FROM dbo.LPPVoids WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (Dat" & _
        "eVoided = @Original_DateVoided OR @Original_DateVoided IS NULL AND DateVoided IS" & _
        " NULL)"
        Me.SqlDeleteCommand2.Connection = Me.LiveDB
        Me.SqlDeleteCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        '
        'SqlInsertCommand2
        '
        Me.SqlInsertCommand2.CommandText = "INSERT INTO dbo.LPPVoids(ReceiptNumber, Notes, DateVoided) VALUES (@ReceiptNumber" & _
        ", @Notes, @DateVoided); SELECT ReceiptNumber, Notes, DateVoided FROM dbo.LPPVoid" & _
        "s WHERE (ReceiptNumber = @ReceiptNumber)"
        Me.SqlInsertCommand2.Connection = Me.LiveDB
        Me.SqlInsertCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlInsertCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 1073741823, "Notes"))
        Me.SqlInsertCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlSelectCommand8
        '
        Me.SqlSelectCommand8.CommandText = "SELECT ReceiptNumber, Notes, DateVoided FROM dbo.LPPVoids WHERE (DateVoided >= @G" & _
        "TDate) AND (DateVoided <= @LTDate)"
        Me.SqlSelectCommand8.Connection = Me.LiveDB
        Me.SqlSelectCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlSelectCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlUpdateCommand2
        '
        Me.SqlUpdateCommand2.CommandText = "UPDATE dbo.LPPVoids SET ReceiptNumber = @ReceiptNumber, Notes = @Notes, DateVoide" & _
        "d = @DateVoided WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (DateVoided " & _
        "= @Original_DateVoided OR @Original_DateVoided IS NULL AND DateVoided IS NULL); " & _
        "SELECT ReceiptNumber, Notes, DateVoided FROM dbo.LPPVoids WHERE (ReceiptNumber =" & _
        " @ReceiptNumber)"
        Me.SqlUpdateCommand2.Connection = Me.LiveDB
        Me.SqlUpdateCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlUpdateCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 1073741823, "Notes"))
        Me.SqlUpdateCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlUpdateCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand2.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        '
        'LivDSLPPVoids
        '
        Me.LivDSLPPVoids.DataSetName = "LivDSLPPVoids"
        Me.LivDSLPPVoids.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'LivLGPVoids
        '
        Me.LivLGPVoids.DeleteCommand = Me.SqlDeleteCommand3
        Me.LivLGPVoids.InsertCommand = Me.SqlInsertCommand3
        Me.LivLGPVoids.SelectCommand = Me.SqlSelectCommand9
        Me.LivLGPVoids.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "LGPVoids", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Notes", "Notes"), New System.Data.Common.DataColumnMapping("DateVoided", "DateVoided")})})
        Me.LivLGPVoids.UpdateCommand = Me.SqlUpdateCommand3
        '
        'SqlDeleteCommand3
        '
        Me.SqlDeleteCommand3.CommandText = "DELETE FROM dbo.LGPVoids WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (Dat" & _
        "eVoided = @Original_DateVoided OR @Original_DateVoided IS NULL AND DateVoided IS" & _
        " NULL) AND (Notes = @Original_Notes OR @Original_Notes IS NULL AND Notes IS NULL" & _
        ")"
        Me.SqlDeleteCommand3.Connection = Me.LiveDB
        Me.SqlDeleteCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_Notes", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Notes", System.Data.DataRowVersion.Original, Nothing))
        '
        'SqlInsertCommand3
        '
        Me.SqlInsertCommand3.CommandText = "INSERT INTO dbo.LGPVoids(ReceiptNumber, Notes, DateVoided) VALUES (@ReceiptNumber" & _
        ", @Notes, @DateVoided); SELECT ReceiptNumber, Notes, DateVoided FROM dbo.LGPVoid" & _
        "s WHERE (ReceiptNumber = @ReceiptNumber)"
        Me.SqlInsertCommand3.Connection = Me.LiveDB
        Me.SqlInsertCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlInsertCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 100, "Notes"))
        Me.SqlInsertCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlSelectCommand9
        '
        Me.SqlSelectCommand9.CommandText = "SELECT ReceiptNumber, Notes, DateVoided FROM dbo.LGPVoids WHERE (DateVoided >= @G" & _
        "TDate) AND (DateVoided <= @LTDate)"
        Me.SqlSelectCommand9.Connection = Me.LiveDB
        Me.SqlSelectCommand9.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlSelectCommand9.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlUpdateCommand3
        '
        Me.SqlUpdateCommand3.CommandText = "UPDATE dbo.LGPVoids SET ReceiptNumber = @ReceiptNumber, Notes = @Notes, DateVoide" & _
        "d = @DateVoided WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (DateVoided " & _
        "= @Original_DateVoided OR @Original_DateVoided IS NULL AND DateVoided IS NULL) A" & _
        "ND (Notes = @Original_Notes OR @Original_Notes IS NULL AND Notes IS NULL); SELEC" & _
        "T ReceiptNumber, Notes, DateVoided FROM dbo.LGPVoids WHERE (ReceiptNumber = @Rec" & _
        "eiptNumber)"
        Me.SqlUpdateCommand3.Connection = Me.LiveDB
        Me.SqlUpdateCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlUpdateCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 100, "Notes"))
        Me.SqlUpdateCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlUpdateCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand3.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_Notes", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Notes", System.Data.DataRowVersion.Original, Nothing))
        '
        'TstLGPVoids
        '
        Me.TstLGPVoids.DeleteCommand = Me.SqlDeleteCommand4
        Me.TstLGPVoids.InsertCommand = Me.SqlInsertCommand4
        Me.TstLGPVoids.SelectCommand = Me.SqlSelectCommand10
        Me.TstLGPVoids.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "LGPVoids", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Notes", "Notes"), New System.Data.Common.DataColumnMapping("DateVoided", "DateVoided")})})
        Me.TstLGPVoids.UpdateCommand = Me.SqlUpdateCommand4
        '
        'SqlDeleteCommand4
        '
        Me.SqlDeleteCommand4.CommandText = "DELETE FROM LGPVoids WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (DateVoi" & _
        "ded = @Original_DateVoided OR @Original_DateVoided IS NULL AND DateVoided IS NUL" & _
        "L) AND (Notes = @Original_Notes OR @Original_Notes IS NULL AND Notes IS NULL)"
        Me.SqlDeleteCommand4.Connection = Me.TestDB
        Me.SqlDeleteCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_Notes", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Notes", System.Data.DataRowVersion.Original, Nothing))
        '
        'SqlInsertCommand4
        '
        Me.SqlInsertCommand4.CommandText = "INSERT INTO LGPVoids(ReceiptNumber, Notes, DateVoided) VALUES (@ReceiptNumber, @N" & _
        "otes, @DateVoided); SELECT ReceiptNumber, Notes, DateVoided FROM LGPVoids WHERE " & _
        "(ReceiptNumber = @ReceiptNumber)"
        Me.SqlInsertCommand4.Connection = Me.TestDB
        Me.SqlInsertCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlInsertCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 100, "Notes"))
        Me.SqlInsertCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlSelectCommand10
        '
        Me.SqlSelectCommand10.CommandText = "SELECT ReceiptNumber, Notes, DateVoided FROM LGPVoids WHERE (DateVoided >= @GTDat" & _
        "e) AND (DateVoided <= @LTDate)"
        Me.SqlSelectCommand10.Connection = Me.TestDB
        Me.SqlSelectCommand10.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlSelectCommand10.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlUpdateCommand4
        '
        Me.SqlUpdateCommand4.CommandText = "UPDATE LGPVoids SET ReceiptNumber = @ReceiptNumber, Notes = @Notes, DateVoided = " & _
        "@DateVoided WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (DateVoided = @O" & _
        "riginal_DateVoided OR @Original_DateVoided IS NULL AND DateVoided IS NULL) AND (" & _
        "Notes = @Original_Notes OR @Original_Notes IS NULL AND Notes IS NULL); SELECT Re" & _
        "ceiptNumber, Notes, DateVoided FROM LGPVoids WHERE (ReceiptNumber = @ReceiptNumb" & _
        "er)"
        Me.SqlUpdateCommand4.Connection = Me.TestDB
        Me.SqlUpdateCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlUpdateCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 100, "Notes"))
        Me.SqlUpdateCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlUpdateCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand4.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_Notes", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Notes", System.Data.DataRowVersion.Original, Nothing))
        '
        'TstDSLGPVoids
        '
        Me.TstDSLGPVoids.DataSetName = "TstDSLGPVoids"
        Me.TstDSLGPVoids.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'LivDSLGPVoids
        '
        Me.LivDSLGPVoids.DataSetName = "LivDSLGPVoids"
        Me.LivDSLGPVoids.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'TstTILPVoids
        '
        Me.TstTILPVoids.DeleteCommand = Me.SqlDeleteCommand7
        Me.TstTILPVoids.InsertCommand = Me.SqlInsertCommand7
        Me.TstTILPVoids.SelectCommand = Me.SqlSelectCommand15
        Me.TstTILPVoids.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "TILPVoids", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Notes", "Notes"), New System.Data.Common.DataColumnMapping("DateVoided", "DateVoided")})})
        Me.TstTILPVoids.UpdateCommand = Me.SqlUpdateCommand7
        '
        'SqlDeleteCommand7
        '
        Me.SqlDeleteCommand7.CommandText = "DELETE FROM dbo.TILPVoids WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (Da" & _
        "teVoided = @Original_DateVoided OR @Original_DateVoided IS NULL AND DateVoided I" & _
        "S NULL) AND (Notes = @Original_Notes OR @Original_Notes IS NULL AND Notes IS NUL" & _
        "L)"
        Me.SqlDeleteCommand7.Connection = Me.TestDB
        Me.SqlDeleteCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_Notes", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Notes", System.Data.DataRowVersion.Original, Nothing))
        '
        'SqlInsertCommand7
        '
        Me.SqlInsertCommand7.CommandText = "INSERT INTO dbo.TILPVoids(ReceiptNumber, Notes, DateVoided) VALUES (@ReceiptNumbe" & _
        "r, @Notes, @DateVoided); SELECT ReceiptNumber, Notes, DateVoided FROM dbo.TILPVo" & _
        "ids WHERE (ReceiptNumber = @ReceiptNumber)"
        Me.SqlInsertCommand7.Connection = Me.TestDB
        Me.SqlInsertCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlInsertCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 100, "Notes"))
        Me.SqlInsertCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlSelectCommand15
        '
        Me.SqlSelectCommand15.CommandText = "SELECT ReceiptNumber, Notes, DateVoided FROM dbo.TILPVoids WHERE (DateVoided >= @" & _
        "GTDate) AND (DateVoided <= @LTDate)"
        Me.SqlSelectCommand15.Connection = Me.TestDB
        Me.SqlSelectCommand15.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlSelectCommand15.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlUpdateCommand7
        '
        Me.SqlUpdateCommand7.CommandText = "UPDATE dbo.TILPVoids SET ReceiptNumber = @ReceiptNumber, Notes = @Notes, DateVoid" & _
        "ed = @DateVoided WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (DateVoided" & _
        " = @Original_DateVoided OR @Original_DateVoided IS NULL AND DateVoided IS NULL) " & _
        "AND (Notes = @Original_Notes OR @Original_Notes IS NULL AND Notes IS NULL); SELE" & _
        "CT ReceiptNumber, Notes, DateVoided FROM dbo.TILPVoids WHERE (ReceiptNumber = @R" & _
        "eceiptNumber)"
        Me.SqlUpdateCommand7.Connection = Me.TestDB
        Me.SqlUpdateCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlUpdateCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 100, "Notes"))
        Me.SqlUpdateCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlUpdateCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand7.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_Notes", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Notes", System.Data.DataRowVersion.Original, Nothing))
        '
        'TstTILPPayments
        '
        Me.TstTILPPayments.SelectCommand = Me.SqlSelectCommand13
        Me.TstTILPPayments.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "TILPPayments", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Name", "Name"), New System.Data.Common.DataColumnMapping("AccountNumber", "AccountNumber"), New System.Data.Common.DataColumnMapping("ReceivedBy", "ReceivedBy"), New System.Data.Common.DataColumnMapping("PaymentDate", "PaymentDate"), New System.Data.Common.DataColumnMapping("PaymentAmount", "PaymentAmount"), New System.Data.Common.DataColumnMapping("PaymentType", "PaymentType"), New System.Data.Common.DataColumnMapping("CheckNumber", "CheckNumber"), New System.Data.Common.DataColumnMapping("Voided", "Voided"), New System.Data.Common.DataColumnMapping("Notes", "Notes")})})
        '
        'SqlSelectCommand13
        '
        Me.SqlSelectCommand13.CommandText = "SELECT dbo.TILPPayments.ReceiptNumber, dbo.TILPPayments.Name, dbo.TILPPayments.Ac" & _
        "countNumber, dbo.TILPPayments.ReceivedBy, dbo.TILPPayments.PaymentDate, dbo.TILP" & _
        "Payments.PaymentAmount, dbo.TILPPayments.PaymentType, dbo.TILPPayments.CheckNumb" & _
        "er, CASE WHEN TILPVoids.ReceiptNumber IS NULL THEN ' ' ELSE 'VOIDED' END AS Void" & _
        "ed, dbo.TILPVoids.Notes FROM dbo.TILPPayments LEFT OUTER JOIN dbo.TILPVoids ON d" & _
        "bo.TILPPayments.ReceiptNumber = dbo.TILPVoids.ReceiptNumber WHERE (dbo.TILPPayme" & _
        "nts.PaymentDate >= @GTDate) AND (dbo.TILPPayments.PaymentDate <= @LTDate)"
        Me.SqlSelectCommand13.Connection = Me.TestDB
        Me.SqlSelectCommand13.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        Me.SqlSelectCommand13.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        '
        'LivTILPPayments
        '
        Me.LivTILPPayments.SelectCommand = Me.SqlSelectCommand14
        Me.LivTILPPayments.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "TILPPayments", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Name", "Name"), New System.Data.Common.DataColumnMapping("AccountNumber", "AccountNumber"), New System.Data.Common.DataColumnMapping("ReceivedBy", "ReceivedBy"), New System.Data.Common.DataColumnMapping("PaymentDate", "PaymentDate"), New System.Data.Common.DataColumnMapping("PaymentAmount", "PaymentAmount"), New System.Data.Common.DataColumnMapping("PaymentType", "PaymentType"), New System.Data.Common.DataColumnMapping("CheckNumber", "CheckNumber"), New System.Data.Common.DataColumnMapping("Voided", "Voided"), New System.Data.Common.DataColumnMapping("Notes", "Notes")})})
        '
        'SqlSelectCommand14
        '
        Me.SqlSelectCommand14.CommandText = "SELECT dbo.TILPPayments.ReceiptNumber, dbo.TILPPayments.Name, dbo.TILPPayments.Ac" & _
        "countNumber, dbo.TILPPayments.ReceivedBy, dbo.TILPPayments.PaymentDate, dbo.TILP" & _
        "Payments.PaymentAmount, dbo.TILPPayments.PaymentType, dbo.TILPPayments.CheckNumb" & _
        "er, CASE WHEN TILPVoids.ReceiptNumber IS NULL THEN ' ' ELSE 'VOIDED' END AS Void" & _
        "ed, dbo.TILPVoids.Notes FROM dbo.TILPPayments LEFT OUTER JOIN dbo.TILPVoids ON d" & _
        "bo.TILPPayments.ReceiptNumber = dbo.TILPVoids.ReceiptNumber WHERE (dbo.TILPPayme" & _
        "nts.PaymentDate >= @GTDate) AND (dbo.TILPPayments.PaymentDate <= @LTDate)"
        Me.SqlSelectCommand14.Connection = Me.LiveDB
        Me.SqlSelectCommand14.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        Me.SqlSelectCommand14.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "PaymentDate"))
        '
        'LivTILPVoids
        '
        Me.LivTILPVoids.DeleteCommand = Me.SqlDeleteCommand8
        Me.LivTILPVoids.InsertCommand = Me.SqlInsertCommand8
        Me.LivTILPVoids.SelectCommand = Me.SqlSelectCommand16
        Me.LivTILPVoids.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "TILPVoids", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("ReceiptNumber", "ReceiptNumber"), New System.Data.Common.DataColumnMapping("Notes", "Notes"), New System.Data.Common.DataColumnMapping("DateVoided", "DateVoided")})})
        Me.LivTILPVoids.UpdateCommand = Me.SqlUpdateCommand8
        '
        'SqlDeleteCommand8
        '
        Me.SqlDeleteCommand8.CommandText = "DELETE FROM dbo.TILPVoids WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (Da" & _
        "teVoided = @Original_DateVoided OR @Original_DateVoided IS NULL AND DateVoided I" & _
        "S NULL) AND (Notes = @Original_Notes OR @Original_Notes IS NULL AND Notes IS NUL" & _
        "L)"
        Me.SqlDeleteCommand8.Connection = Me.LiveDB
        Me.SqlDeleteCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlDeleteCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_Notes", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Notes", System.Data.DataRowVersion.Original, Nothing))
        '
        'SqlInsertCommand8
        '
        Me.SqlInsertCommand8.CommandText = "INSERT INTO dbo.TILPVoids(ReceiptNumber, Notes, DateVoided) VALUES (@ReceiptNumbe" & _
        "r, @Notes, @DateVoided); SELECT ReceiptNumber, Notes, DateVoided FROM dbo.TILPVo" & _
        "ids WHERE (ReceiptNumber = @ReceiptNumber)"
        Me.SqlInsertCommand8.Connection = Me.LiveDB
        Me.SqlInsertCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlInsertCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 100, "Notes"))
        Me.SqlInsertCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlSelectCommand16
        '
        Me.SqlSelectCommand16.CommandText = "SELECT ReceiptNumber, Notes, DateVoided FROM dbo.TILPVoids WHERE (DateVoided >= @" & _
        "GTDate) AND (DateVoided <= @LTDate)"
        Me.SqlSelectCommand16.Connection = Me.LiveDB
        Me.SqlSelectCommand16.Parameters.Add(New System.Data.SqlClient.SqlParameter("@GTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlSelectCommand16.Parameters.Add(New System.Data.SqlClient.SqlParameter("@LTDate", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        '
        'SqlUpdateCommand8
        '
        Me.SqlUpdateCommand8.CommandText = "UPDATE dbo.TILPVoids SET ReceiptNumber = @ReceiptNumber, Notes = @Notes, DateVoid" & _
        "ed = @DateVoided WHERE (ReceiptNumber = @Original_ReceiptNumber) AND (DateVoided" & _
        " = @Original_DateVoided OR @Original_DateVoided IS NULL AND DateVoided IS NULL) " & _
        "AND (Notes = @Original_Notes OR @Original_Notes IS NULL AND Notes IS NULL); SELE" & _
        "CT ReceiptNumber, Notes, DateVoided FROM dbo.TILPVoids WHERE (ReceiptNumber = @R" & _
        "eceiptNumber)"
        Me.SqlUpdateCommand8.Connection = Me.LiveDB
        Me.SqlUpdateCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@ReceiptNumber", System.Data.SqlDbType.BigInt, 8, "ReceiptNumber"))
        Me.SqlUpdateCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 100, "Notes"))
        Me.SqlUpdateCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@DateVoided", System.Data.SqlDbType.DateTime, 4, "DateVoided"))
        Me.SqlUpdateCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_ReceiptNumber", System.Data.SqlDbType.BigInt, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiptNumber", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_DateVoided", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateVoided", System.Data.DataRowVersion.Original, Nothing))
        Me.SqlUpdateCommand8.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Original_Notes", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Notes", System.Data.DataRowVersion.Original, Nothing))
        '
        'TstDSTILPPayments
        '
        Me.TstDSTILPPayments.DataSetName = "TstDSTILPPayments"
        Me.TstDSTILPPayments.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'LivDSTILPPayments
        '
        Me.LivDSTILPPayments.DataSetName = "LivDSTILPPayments"
        Me.LivDSTILPPayments.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'TstDSTILPVoids
        '
        Me.TstDSTILPVoids.DataSetName = "TstDSTILPVoids"
        Me.TstDSTILPVoids.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'LivDSTILPVoids
        '
        Me.LivDSTILPVoids.DataSetName = "LivDSTILPVoids"
        Me.LivDSTILPVoids.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'lblTest
        '
        Me.lblTest.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTest.ForeColor = System.Drawing.Color.FromArgb(CType(192, Byte), CType(0, Byte), CType(0, Byte))
        Me.lblTest.Location = New System.Drawing.Point(256, 608)
        Me.lblTest.Name = "lblTest"
        Me.lblTest.Size = New System.Drawing.Size(128, 23)
        Me.lblTest.TabIndex = 2
        Me.lblTest.Text = "Test Mode"
        '
        'PaymentReceipts
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(648, 637)
        Me.Controls.Add(Me.lblTest)
        Me.Controls.Add(Me.btnReprint)
        Me.Controls.Add(Me.Tabs)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(656, 664)
        Me.MinimumSize = New System.Drawing.Size(656, 664)
        Me.Name = "PaymentReceipts"
        Me.Text = "Payment Receipts"
        Me.Tabs.ResumeLayout(False)
        Me.TabUESP.ResumeLayout(False)
        CType(Me.dgBen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabLGP.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.TabLPP.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.TabTILP.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.TabAcctg.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.TabVoidR.ResumeLayout(False)
        Me.TabReports.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox13.ResumeLayout(False)
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox14.ResumeLayout(False)
        Me.GroupBox15.ResumeLayout(False)
        Me.GroupBox16.ResumeLayout(False)
        CType(Me.TstDSLGPPayments, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LivDSLGPPayments, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TstDSLPPPayments, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LivDSLPPPayments, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TstDSLPPVoids, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LivDSLPPVoids, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TstDSLGPVoids, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LivDSLGPVoids, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TstDSTILPPayments, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LivDSTILPPayments, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TstDSTILPVoids, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LivDSTILPVoids, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub PaymentReceipts_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblTest.Visible = Test
        InitSession()
        'decide which DB to use and notify the user if the application is running in test mode
        If Test Then
            MsgBox("The application is running in test mode.", MsgBoxStyle.Information)
            DBConn = TestDB 'use test DB
            LiveDB.Dispose() 'delete connection to live DB
            'data adapters
            DALGPPayment = TstLGPPayments
            DALPPPayment = TstLPPPayments
            DALPPVoid = TstLPPVoids
            DALGPVoid = TstLGPVoids
            DATILPPayment = TstTILPPayments
            DATILPVoid = TstTILPVoids
            'data sets
            DSTILPPayment = TstDSTILPPayments
            DSTILPVoid = TstDSTILPVoids
            DSLGPPayment = TstDSLGPPayments
            DSLPPPayment = TstDSLPPPayments
            DSLPPVoid = TstDSLPPVoids
            DSLGPVoid = TstDSLGPVoids
        Else
            DBConn = LiveDB 'use live DB
            TestDB.Dispose() 'delete connection to test DB
            'data adapters
            DATILPVoid = LivTILPVoids
            DATILPPayment = LivTILPPayments
            DALGPPayment = LivLGPPayments
            DALPPPayment = LivLPPPayments
            DALPPVoid = LivLPPVoids
            DALGPVoid = LivLGPVoids
            'data sets
            DSTILPVoid = LivDSTILPVoids
            DSTILPPayment = LivDSTILPPayments
            DSLGPPayment = LivDSLGPPayments
            DSLPPPayment = LivDSLPPPayments
            DSLPPVoid = LivDSLPPVoids
            DSLGPVoid = LivDSLGPVoids
        End If
        If HasSession = False Then
            If HasNonSessionAccess(System.Environment.UserName) Then
                MsgBox("You will only be able to access UESP Receipts.")
                Tabs.TabPages.Remove(TabLGP)
                Tabs.TabPages.Remove(TabLGP)
                Tabs.TabPages.Remove(TabLPP)
                Tabs.TabPages.Remove(TabTILP)
                Tabs.TabPages.Remove(TabAcctg)
                Tabs.TabPages.Remove(TabVoidR)
                Tabs.TabPages.Remove(TabReports)
                'Exit Sub
            ElseIf IsUespUser(System.Environment.UserName) Then
                Tabs.TabPages.Remove(TabLGP)
                Tabs.TabPages.Remove(TabLGP)
                Tabs.TabPages.Remove(TabLPP)
                Tabs.TabPages.Remove(TabTILP)
                Tabs.TabPages.Remove(TabAcctg)
                Tabs.TabPages.Remove(TabVoidR)
                Tabs.TabPages.Remove(TabReports)
            Else
                MsgBox("You must have a Reflection session open.  Please open a Reflection session and try again.", MsgBoxStyle.Critical)
                End
            End If
        End If

        'gather userID for comments later
        'Userid = GetUserID()

        'setup calandar values
        LPPSD.Value = Today()
        LPPED.Value = Today()
        LGPSD.Value = Today()
        LGPED.Value = Today()
        UESPSD.Value = Today()
        UESPED.Value = Today()
        'default accept button is UESPProcess
        Me.AcceptButton = btnUESPProcess

        DSBen = New DataSet
        Dim tbl As New DataTable("Ben")
        tbl.Columns.Add("Name")
        tbl.Columns.Add("AccNum")
        tbl.Columns.Add("CheckNum")
        tbl.Columns.Add("Amt", System.Type.GetType("System.Double"))
        DSBen.Tables.Add(tbl)
        dgBen.DataSource = DSBen.Tables("Ben")
        dgBen.TableStyles.Clear()
        dgBen.TableStyles.Add(GetBenTableStyle)



    End Sub

    Function HasNonSessionAccess(ByVal UName As String) As Boolean
        Dim str As String = ""
        Dim access As String = ""
        Dim Reader As SqlDataReader
        Dim SelectStr As String = "Select UserName, AccessTo From Access Where Username = '" & UName & "'"
        Dim DBSelectComm As New SqlCommand(SelectStr, DBConn)
        Try
            DBConn.Open()
            Reader = DBSelectComm.ExecuteReader() 'Retrieve Max Receipt number for ParticipantName 
            If Reader.HasRows Then
                Reader.Read() 'read first result
                str = Reader.GetValue(0)
                access = Reader.GetValue(1)
            End If
            DBConn.Close()
        Catch ex As Exception
            MsgBox("An error occured while trying to update the database.  Please try again.  If the problem continues then please contact Systems Support.", MsgBoxStyle.Critical)
            MsgBox(ex.ToString & vbLf & vbLf & ex.Message)
            End
        End Try
        If str <> "" Then
            If access = "DECEMBER" Then
                If Now.Month = 12 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function


    Function IsUespUser(ByVal UName As String) As Boolean
        Dim str As String = ""
        Dim access As String = ""
        Dim Reader As SqlDataReader
        Dim SelectStr As String = "Select UserName, AccessTo From Access Where Username = '" & UName & "'"
        Dim DBSelectComm As New SqlCommand(SelectStr, DBConn)
        Try
            DBConn.Open()
            Reader = DBSelectComm.ExecuteReader() 'Retrieve Max Receipt number for ParticipantName 
            If Reader.HasRows Then
                Reader.Read() 'read first result
                str = Reader.GetValue(0)
                access = Reader.GetValue(1)
            End If
            DBConn.Close()
        Catch ex As Exception
            MsgBox("An error occured while trying to update the database.  Please try again.  If the problem continues then please contact Systems Support.", MsgBoxStyle.Critical)
            MsgBox(ex.ToString & vbLf & vbLf & ex.Message)
            End
        End Try
        If str <> "" Then
            If access = "UESP" Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function


    'changes accept button depending on which tab the user is on
    Private Sub Tabs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tabs.SelectedIndexChanged
        If Tabs.SelectedTab.Text = "LPP" Then
            Me.AcceptButton = btnLPPProcess
        ElseIf Tabs.SelectedTab.Text = "LGP" Then
            Me.AcceptButton = btnLGPProcess
        ElseIf Tabs.SelectedTab.Text = "Acctg" Then
            Me.AcceptButton = btnAcctgProcess
        ElseIf Tabs.SelectedTab.Text = "Void Receipts" Then
            Dim Lg As New Login
            Dim Re As String = Lg.Show(DBConn)
            If Re.IndexOf("VR") <> -1 Then
                Me.AcceptButton = btnOK
            Else
                If Re <> "Cancelled" Then MsgBox("You don't have access to void receipts.")
                Tabs.SelectedIndex = 0
            End If
        ElseIf Tabs.SelectedTab.Text = "Reports" Then
            Dim Lg As New Login
            Dim Re As String = Lg.Show(DBConn)
            If Re.IndexOf("RPT") = -1 Then
                If Re <> "Cancelled" Then MsgBox("You don't have access to print reports.")
                Tabs.SelectedIndex = 0
            End If
        Else 'if none of the top then it's UESP.  The first time this function is called the text is blank but the app is on UESP so this is the catch all
            Me.AcceptButton = btnUESPProcess
        End If
    End Sub

#Region " UESP Functionality "

    Private Sub FigureTotal()
        Dim r As DataRow
        Dim tot As Double
        For Each r In DSBen.Tables("Ben").Rows
            If r.Item("Amt").GetType.ToString <> "System.DBNull" Then
                tot += r.Item("Amt")
            End If
        Next
        lblTotal.Text = FormatCurrency(tot, 2)
    End Sub

    'reinits form vars
    Private Sub btnUESPClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUESPClear.Click
        'reinit all form vars
        tbUESPRB.Clear()
        UESPClear()
    End Sub

    Private Sub btnUESPProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUESPProcess.Click
        Me.Visible = False
        PleaseWait.Visible = True
        PleaseWait.Refresh()
        If DataCheckForUESP() Then
            If AddPaymentsRecords(tbUESPPN.Text, tbUESPRB.Text, Today.ToShortDateString) = False Then
                Exit Sub
            End If
            UESPClear() 'clear data
        End If
        PleaseWait.Visible = False
        Me.Visible = True
    End Sub

    Function AddPaymentsRecords(ByVal PName As String, ByVal RBy As String, ByVal PDate As String) As Boolean

        Dim Reader As SqlDataReader
        Dim DBInsertComm As SqlCommand
        Dim InsertStr As String
        RBy.Replace("'", "''")
        PName.Replace("'", "''")

        'ADD RECEIPT RECORD
        InsertStr = "Insert Into UESPPayment (ParticipantName, ReceivedBy,PaymentDate) " & _
                                   "values ('" & PName & "', '" & RBy & "', '" & PDate & "')"
        DBInsertComm = New SqlCommand(InsertStr, DBConn)
        Try
            DBConn.Open()
            DBInsertComm.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox("An error occured while trying to update the database.  Please try again.  If the problem continues then please contact Systems Support.", MsgBoxStyle.Critical)
            MsgBox(ex.ToString & vbLf & vbLf & ex.Message)
            Return False
        Finally
            DBConn.Close()
        End Try


        'GET RECEIPT NUMBER
        Dim SelectStr As String = "Select Max(ReceiptNumber) From UESPPayment Where ParticipantName = '" & PName & "' and ReceivedBy = '" & RBy & "' and PaymentDate = '" & PDate & "'"
        Dim DBSelectComm As New SqlCommand(SelectStr, DBConn)
        Try
            DBConn.Open()
            DBInsertComm.ExecuteNonQuery() 'insert data into DB
            Reader = DBSelectComm.ExecuteReader() 'Retrieve Max Receipt number for ParticipantName 
            Reader.Read() 'read first result
            ReceiptNumber = Reader.GetValue(0)
            DBConn.Close()
        Catch ex As Exception
            MsgBox("An error occured while trying to update the database.  Please try again.  If the problem continues then please contact Systems Support.", MsgBoxStyle.Critical)
            MsgBox(ex.ToString & vbLf & vbLf & ex.Message)
            Return False
        End Try

        'ADD EACH BENEFICIARY
        Dim row As DataRow
        For Each row In DSBen.Tables("Ben").Rows
            If AddReceiptBenef(ReceiptNumber, row.Item("Name"), row.Item("AccNum"), row.Item("CheckNum"), row.Item("Amt")) = False Then
                Return False
            End If
        Next


        Dim DSPrint As New DataSet
        SelectStr = "select A.ReceiptNumber, A.ParticipantName, A.ReceivedBy, A.PaymentDate,B.BenefName, B.BenefAcctNum, B.BenefCheckNum, B.BenefAmt From UESPPayment A inner join UESPPaymentsBenef B on A.ReceiptNumber = B.ReceiptNumber Where A.ReceiptNumber = " & ReceiptNumber
        Dim DA As New SqlClient.SqlDataAdapter(SelectStr, DBConn)
        DA.Fill(DSPrint)
        Dim rpt As New rptPaymentReceipts

        rpt.SetDataSource(DSPrint.Tables(0))
        rpt.PrintToPrinter(2, False, 1, -1)
        LastReceiptTypePrinted = "UESPPaymentReceipts"
        Return True

    End Function

    Function AddReceiptBenef(ByVal RNum As Integer, ByVal BName As String, ByVal BAcct As String, ByVal BCheckNum As String, ByVal BAmt As Double) As Boolean
        Dim DBInsertComm As SqlCommand
        Dim InsertStr As String

        InsertStr = "Insert Into UESPPaymentsBenef (ReceiptNumber,BenefName, BenefAcctNum,BenefCheckNum,BenefAmt) " & _
                                   "values (" & RNum & ", '" & BName & "', '" & BAcct & "', '" & BCheckNum & "'," & BAmt & ")"
        DBInsertComm = New SqlCommand(InsertStr, DBConn)
        Try
            DBConn.Open()
            DBInsertComm.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox("An error occured while trying to update the database.  Please try again.  If the problem continues then please contact Systems Support.", MsgBoxStyle.Critical)
            MsgBox(ex.ToString & vbLf & vbLf & ex.Message)
            Return False
        Finally
            DBConn.Close()
        End Try
        Return True
    End Function

    Private Function DataCheckForUESP() As Boolean
        'move focus in case it's on one of the total text boxes
        tbUESPPN.Focus()
        'check for participant name
        If tbUESPPN.TextLength = 0 Then
            MsgBox("You must provide a Account Owner Name.", MsgBoxStyle.Critical)
            tbUESPPN.Focus()
            Return False
        End If
        'check for received by
        If tbUESPRB.TextLength = 0 Then
            MsgBox("You must provide a Received By name.", MsgBoxStyle.Critical)
            tbUESPRB.Focus()
            Return False
        End If
        If CDbl(lblTotal.Text) = 0 Then
            MsgBox("The total dollar amount can't be zero.", MsgBoxStyle.Critical)
            Return False
        End If

        'check for total payment entry
        If tbUESPTPA.TextLength = 0 Then
            MsgBox("You must provide the total payment.", MsgBoxStyle.Critical)
            tbUESPTPA.Focus()
            Return False
        End If
        'check if the payment entry is numeric
        If IsNumeric(tbUESPTPA.Text) = False Then
            MsgBox("Total payment must be numeric.", MsgBoxStyle.Critical)
            tbUESPTPA.Focus()
            Return False
        End If
        'check if calculated total and user entered total =
        If CDbl(tbUESPTPA.Text) <> CDbl(lblTotal.Text) Then
            MsgBox("The total you provided and the calculated total don't match.", MsgBoxStyle.Critical)
            Return False
        End If
        'check misc for other beneficiaries
        Dim row As DataRow
        For Each row In DSBen.Tables("Ben").Rows
            If row.Item("Name").GetType.ToString = "System.DBNull" Or _
            row.Item("AccNum").GetType.ToString = "System.DBNull" Or _
            row.Item("CheckNum").GetType.ToString = "System.DBNull" Or _
            row.Item("Amt").GetType.ToString = "System.DBNull" Then
                MsgBox("If any information is provided for a Beneficiary then all information (name, nine digit account number and amount applied to beneficiary's account) is required for the Beneficiary.", MsgBoxStyle.Critical)
                Return False
            End If
            If ValidateRow(row.Item("Name"), row.Item("AccNum"), row.Item("CheckNum"), row.Item("Amt")) = False Then
                Return False
            End If
        Next

        Return True
    End Function

    'reinit form vars
    Private Sub UESPClear()
        tbUESPPN.Clear()
        DSBen.Tables("Ben").Clear()
        tbUESPTPA.Clear()

        lblTotal.Text = "$0.00"
        tbUESPPN.Focus()
    End Sub

    Function GetBenTableStyle() As DataGridTableStyle
        Dim tbs As New DataGridTableStyle
        tbs.MappingName = "Ben"
        point00 = New Point(dgBen.GetCellBounds(0, 0).X + 4, dgBen.GetCellBounds(0, 0).Y + 4)
        tbs.RowHeaderWidth = 45
        Dim c1 As New DataGridTextBoxColumn
        Dim c2 As New DataGridTextBoxColumn
        Dim c3 As New DataGridTextBoxColumn

        Dim c4 As New DataGridTextBoxColumn

        c1.HeaderText = "Name"
        c2.HeaderText = "Account Num"
        c3.HeaderText = "Check Num"
        c4.HeaderText = "Amount"

        c1.MappingName = "Name"
        c2.MappingName = "AccNum"
        c3.MappingName = "CheckNum"
        c4.MappingName = "Amt"

        c1.Width = 200
        c2.Width = 150
        c3.Width = 100
        c4.Width = 100

        c4.Format = "c"
        c1.NullText = ""
        c2.NullText = ""
        c3.NullText = ""
        c4.NullText = "$0.00"



        tbs.GridColumnStyles.Add(c1)
        tbs.GridColumnStyles.Add(c2)
        tbs.GridColumnStyles.Add(c3)
        tbs.GridColumnStyles.Add(c4)
        tbs.AllowSorting = False
        Return tbs
    End Function



    Function ValidateRow(ByVal name As String, ByVal Acc As String, ByVal Chk As String, ByVal Amt As Double) As Boolean
        If Trim(name) = "" Or Acc.Length < 9 Or Chk = "" Or Amt = 0 Then
            MsgBox("If any information is provided for a Beneficiary then all information (name, nine digit account number and amount applied to beneficiary's account) is required for the Beneficiary.", MsgBoxStyle.Critical)
            Return False
        End If
        If IsNumeric(Acc) = False And Acc.ToUpper <> "NEWACCOUNT" Then 'check if account number is numeric
            MsgBox("UESP account numbers must be numeric or contain ""newaccount"".", MsgBoxStyle.Critical)
            Return False
        End If
        Return True
    End Function

#End Region

#Region " LPP Functionality "

    Private Sub btnLPPProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLPPProcess.Click
        Dim SSN As String
        Dim AccNum As String
        Me.Visible = False
        PleaseWait.Visible = True
        PleaseWait.Refresh()

        If LPPDataCheck(SSN, AccNum) Then
            tbLPPBN.Focus()
            Dim ReceiptNumber As String
            Dim Reader As SqlDataReader
            Dim SelectStr As String = "Select Max(ReceiptNumber) From LPPPayments Where Name = '" & tbLPPBN.Text.Replace("'", "''") & "' and ReceivedBy = '" & tbLPPRB.Text.Replace("'", "''") & "' and CheckNumber = '" & tbLPPCN.Text & "'"       '<3>
            Dim DBSelectComm As New SqlCommand(SelectStr, DBConn)
            Dim InsertStr As String = "Insert Into LPPPayments (Name, AccountNumber, ReceivedBy,PaymentAmount,PaymentType," & _
                                   "CheckNumber) values ('" & tbLPPBN.Text.Replace("'", "''") & "', '" & AccNum & "', '" & tbLPPRB.Text.Replace("'", "''") & "', " & tbLPPPA.Text & ", '" & LPPPayType() & "', '" & tbLPPCN.Text & "')"
            Dim DBInsertComm As New SqlCommand(InsertStr, DBConn)
            Try
                DBConn.Open()
                DBInsertComm.ExecuteNonQuery() 'insert data into DB
                Reader = DBSelectComm.ExecuteReader() 'Retrieve Max Receipt number for ParticipantName 
                Reader.Read() 'read first result
                ReceiptNumber = Reader.GetValue(0)
                DBConn.Close()
            Catch ex As Exception
                MsgBox("An error occured while trying to update the database.  Please try again.  If the problem continues then please contact Systems Support.", MsgBoxStyle.Critical)
                MsgBox(ex.ToString & vbLf & vbLf & ex.Message)
                Exit Sub
            End Try
            'Merge and print receipts
            FileOpen(1, "T:\ReceiptDat.txt", OpenMode.Output, OpenAccess.Write)
            WriteLine(1, "Name", "AccNum", "RecBy", "PayAmt", "Cash", "Check", "CheckNum", "RecNum", "Copy")
            If rbLPPCash.Checked Then 'if payment is cash
                'participants copy
                WriteLine(1, tbLPPBN.Text, AccNum, tbLPPRB.Text, tbLPPPA.Text, "X", "", "", ReceiptNumber, "Accounting Copy")
                'Accountings copy
                WriteLine(1, tbLPPBN.Text, AccNum, tbLPPRB.Text, tbLPPPA.Text, "X", "", "", ReceiptNumber, "Borrower Copy")
            Else 'if payment is check
                'participants copy
                WriteLine(1, tbLPPBN.Text, AccNum, tbLPPRB.Text, tbLPPPA.Text, "", "X", tbLPPCN.Text, ReceiptNumber, "Accounting Copy")
                'Accountings copy
                WriteLine(1, tbLPPBN.Text, AccNum, tbLPPRB.Text, tbLPPPA.Text, "", "X", tbLPPCN.Text, ReceiptNumber, "Borrower Copy")
            End If
            FileClose(1)
            'Print receipts

            LastReceiptTypePrinted = "Q:\Payment Receipts\LPP Payment Receipt.doc"
            PrintDocs("Q:\Payment Receipts\LPP Payment Receipt.doc")
            'enter TD22 comment
            If ATD22AllLoans(SSN, "MPYMT", "LPP " & Format(Today, "MM/dd/yy") & " " & LPPCommentPayType() & " " & Format(CDbl(tbLPPPA.Text), "000000000.00") & " Walk in payment from " & tbLPPBN.Text & " accepted by " & tbLPPRB.Text & ".", "PMTRCPTS", Userid) = False Then
                MsgBox("There was an error while entering a comment into the TD22 ""MPYMT"" ARC.  Please contact Systems Support.")
                End
            End If
            LPPClear() 'clear data
        End If
        PleaseWait.Visible = False
        Me.Visible = True
    End Sub

    'returns a string containing an english description of the payment type
    Private Function LPPPayType() As String
        If rbLPPCash.Checked Then
            LPPPayType = "Cash"
        Else
            LPPPayType = "Check"
        End If
    End Function

    'returns payment type for TD22 comment
    Private Function LPPCommentPayType() As String
        If rbLPPCash.Checked Then
            LPPCommentPayType = "Cash "
        Else
            LPPCommentPayType = "Check"
        End If
    End Function

    'this function does data checks for the LPP tab
    '<1>Private Function LPPDataCheck(ByRef SSN As String) As Boolean
    Private Function LPPDataCheck(ByRef SSN As String, ByRef AccNum As String) As Boolean
        'check for participant name
        If tbLPPBN.TextLength = 0 Then
            MsgBox("You must provide a Borrower's Name.", MsgBoxStyle.Critical)
            tbLPPBN.Focus()
            Return False
        End If
        'check for received by
        If tbLPPRB.TextLength = 0 Then
            MsgBox("You must provide a Received By name.", MsgBoxStyle.Critical)
            tbLPPRB.Focus()
            Return False
        End If
        'check for payment method
        If rbLPPCash.Checked = False And rbLPPCheck.Checked = False Then
            MsgBox("You must select a Payment Method.", MsgBoxStyle.Critical)
            Return False
        End If
        'check for check number if applicable
        If rbLPPCheck.Checked Then
            If tbLPPCN.TextLength = 0 Then
                MsgBox("You must provide a Check Number if the Payment Method is Check.", MsgBoxStyle.Critical)
                tbLPPCN.Focus()
                Return False
            End If
        End If
        'check for total payment entry
        If tbLPPPA.TextLength = 0 Then
            MsgBox("You must provide the payment amount.", MsgBoxStyle.Critical)
            tbLPPPA.Focus()
            Return False
        End If
        'check if the payment entry is numeric
        If IsNumeric(tbLPPPA.Text) = False Then
            MsgBox("Payment amount must be numeric.", MsgBoxStyle.Critical)
            tbLPPPA.Focus()
            Return False
        End If
        '<3->
        If CDbl(tbLPPPA.Text) = 0 Then
            MsgBox("You must provide a payment amount.", MsgBoxStyle.Critical)
            tbLPPPA.Focus()
            Return False
        End If
        '</3>
        'check if the user provided a ten digit account number
        If tbLPPAN.TextLength < 9 Then
            MsgBox("You must provide a ten digit account number or an SSN.", MsgBoxStyle.Critical)
            tbLPPAN.Focus()
            Return False
        End If
        'check if the account number is numeric
        If IsNumeric(tbLPPAN.Text) = False Then
            MsgBox("You must provide a numeric account number or SSN.", MsgBoxStyle.Critical)
            tbLPPAN.Focus()
            Return False
        End If
        'Do OneLINK check and Account number translation
        If OneLINK(SSN, AccNum, tbLPPAN.Text) = False Then
            Return False
        End If
        'check if the borrower is on COMPASS
        FastPathInput("TX3Z/ITS24" & SSN)
        If TextCheck(1, 76, "TSX25") = False Then
            MsgBox("The borrower doesn't have any open loans on COMPASS are you sure the payment isn't for LGP.", MsgBoxStyle.Information)
            Return False
        End If
        Return True 'data is valid
    End Function

    'clears all text boxes
    Private Sub btnLPPClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLPPClear.Click
        tbLPPRB.Clear()
        LPPClear()
    End Sub

    'clears all text boxes except received by
    Private Sub LPPClear()
        tbLPPBN.Clear()
        rbLPPCheck.Checked = False
        rbLPPCash.Checked = False
        tbLPPCN.Clear()
        tbLPPPA.Text = "0.00"
        tbLPPAN.Clear()
        lblLPPCN.Enabled = False 'disable both check number components
        tbLPPCN.Enabled = False
    End Sub

    Private Sub rbLPPCash_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLPPCash.CheckedChanged
        If rbLPPCash.Checked Then 'if checked
            tbLPPCN.Clear() 'clear check number
            lblLPPCN.Enabled = False 'disable both check number components
            tbLPPCN.Enabled = False
        End If
    End Sub

    Private Sub rbLPPCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLPPCheck.CheckedChanged
        If rbLPPCheck.Checked Then
            lblLPPCN.Enabled = True 'enable both check number components
            tbLPPCN.Enabled = True
        End If
    End Sub

    Private Sub tbLPPPA_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbLPPPA.LostFocus
        If IsNumeric(tbLPPPA.Text) Then
            tbLPPPA.Text = Format(CDbl(tbLPPPA.Text), "########0.00")
        Else
            MsgBox("Payment amounts must be numeric.")
            tbLPPPA.Text = "0.00"
            tbLPPPA.Focus()
        End If
    End Sub

#End Region

#Region " LGP Functionality "

    Private Sub btnLGPProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLGPProcess.Click
        Dim SSN As String
        Dim AccNum As String
        Me.Visible = False
        PleaseWait.Visible = True
        PleaseWait.Refresh()
        If LGPDataCheck(SSN, AccNum) Then
            tbLGPBN.Focus()
            Dim ReceiptNumber As String
            Dim Reader As SqlDataReader

            Dim SelectStr As String = "Select Max(ReceiptNumber) From LGPPayments Where Name = '" & tbLGPBN.Text.Replace("'", "''") & "' and ReceivedBy = '" & tbLGPRB.Text.Replace("'", "''") & "' and CheckNumber = '" & tbLGPCN.Text & "'"           '<3>
            Dim DBSelectComm As New SqlCommand(SelectStr, DBConn)
            Dim InsertStr As String = "Insert Into LGPPayments (Name, AccountNumber, ReceivedBy,PaymentAmount,PaymentType," & _
                                   "CheckNumber) values ('" & tbLGPBN.Text.Replace("'", "''") & "', '" & AccNum & "', '" & tbLGPRB.Text.Replace("'", "''") & "', " & tbLGPPA.Text & ", '" & "Check" & "','" & tbLGPCN.Text & "')"           '<3>
            Dim DBInsertComm As New SqlCommand(InsertStr, DBConn)
            Try
                DBConn.Open()
                DBInsertComm.ExecuteNonQuery() 'insert data into DB
                Reader = DBSelectComm.ExecuteReader() 'Retrieve Max Receipt number for ParticipantName 
                Reader.Read() 'read first result
                ReceiptNumber = Reader.GetValue(0)
                DBConn.Close()
            Catch ex As Exception
                MsgBox("An error occured while trying to update the database.  Please try again.  If the problem continues then please contact Systems Support.", MsgBoxStyle.Critical)
                MsgBox(ex.ToString & vbLf & vbLf & ex.Message)
                Exit Sub
            End Try
            'Merge and print receipts
            FileOpen(1, "T:\ReceiptDat.txt", OpenMode.Output, OpenAccess.Write)
            WriteLine(1, "Name", "AccNum", "RecBy", "PayAmt", "Cash", "Check", "CheckNum", "RecNum", "Copy")
            If rbLGPCash.Checked Then 'if payment is cash
                'participants copy
                WriteLine(1, tbLGPBN.Text, AccNum, tbLGPRB.Text, tbLGPPA.Text, "X", "", "", ReceiptNumber, "Accounting Copy")
                'Accountings copy
                WriteLine(1, tbLGPBN.Text, AccNum, tbLGPRB.Text, tbLGPPA.Text, "X", "", "", ReceiptNumber, "Borrower Copy")
            Else 'if payment is check
                'participants copy
                WriteLine(1, tbLGPBN.Text, AccNum, tbLGPRB.Text, tbLGPPA.Text, "", "X", tbLGPCN.Text, ReceiptNumber, "Accounting Copy")
                'Accountings copy
                WriteLine(1, tbLGPBN.Text, AccNum, tbLGPRB.Text, tbLGPPA.Text, "", "X", tbLGPCN.Text, ReceiptNumber, "Borrower Copy")
            End If
            FileClose(1)
            'Print receipts
            LastReceiptTypePrinted = "Q:\Payment Receipts\LGP Payment Receipt.doc"
            PrintDocs("Q:\Payment Receipts\LGP Payment Receipt.doc")            '<2>
            'enter LP50 comment
            If rbLGPCash.Checked Then
                'cash
                If AddLP50(SSN, "MPYMT", "PMTRCPTS", "MS", "04", "LGP " & Format(Today, "MM/dd/yy") & " " & "Cash" & " " & Format(CDbl(tbLGPPA.Text), "000000000.00") & " Walk in payment from " & tbLGPBN.Text & " accepted by " & tbLGPRB.Text & ".") = False Then
                    MsgBox("There was an error while entering a comment into LP50.  Please contact Systems Support.")
                    End
                End If
            Else
                'check
                If AddLP50(SSN, "MPYMT", "PMTRCPTS", "MS", "04", "LGP " & Format(Today, "MM/dd/yy") & " " & "Check" & " " & Format(CDbl(tbLGPPA.Text), "000000000.00") & " Walk in payment from " & tbLGPBN.Text & " accepted by " & tbLGPRB.Text & ".") = False Then
                    MsgBox("There was an error while entering a comment into LP50.  Please contact Systems Support.")
                    End
                End If
            End If
            LGPClear() 'clear data
        End If
        PleaseWait.Visible = False
        Me.Visible = True
    End Sub
    Private Function LGPPayType() As String
        If rbLGPCash.Checked Then
            LGPPayType = "Cash"
        Else
            LGPPayType = "Check"
        End If
    End Function
    'returns payment type for TD22 comment
    Private Function LGPCommentPayType() As String
        If rbLGPCash.Checked Then
            LGPCommentPayType = "Cash "
        Else
            LGPCommentPayType = "Check"
        End If
    End Function

    'this function does data checks for the LGP tab
    Private Function LGPDataCheck(ByRef SSN As String, ByRef AccNum As String) As Boolean
        'check for participant name
        If tbLGPBN.TextLength = 0 Then
            MsgBox("You must provide a Borrower's Name.", MsgBoxStyle.Critical)
            tbLGPBN.Focus()
            Return False
        End If
        'check for received by
        If tbLGPRB.TextLength = 0 Then
            MsgBox("You must provide a Received By name.", MsgBoxStyle.Critical)
            tbLGPRB.Focus()
            Return False
        End If
        'check for payment method
        If rbLGPCash.Checked = False And rbLGPCheck.Checked = False Then
            MsgBox("You must select a Payment Method.", MsgBoxStyle.Critical)
            Return False
        End If

        If CDbl(tbLGPPA.Text) = 0 Then
            MsgBox("You must provide a payment amount.", MsgBoxStyle.Critical)
            tbLGPPA.Focus()
            Return False
        End If
        'check for check number if applicable
        If rbLGPCheck.Checked Then
            If tbLGPCN.TextLength = 0 Then
                MsgBox("You must provide a Check Number if the Payment Method is Check.", MsgBoxStyle.Critical)
                tbLGPCN.Focus()
                Return False
            End If
            If IsNumeric(tbLGPCN.Text) = False Then
                MsgBox("You must provide a numeric check number.", MsgBoxStyle.Critical)
                tbLGPCN.Focus()
                Return False
            End If
        End If
        'check for total payment entry
        If tbLGPPA.TextLength = 0 Then
            MsgBox("You must provide the payment amount.", MsgBoxStyle.Critical)
            tbLGPPA.Focus()
            Return False
        End If

        'check if the payment entry is numeric
        If IsNumeric(tbLGPPA.Text) = False Then
            MsgBox("Payment amount must be numeric.", MsgBoxStyle.Critical)
            tbLGPPA.Focus()
            Return False
        End If
        'check if the user provided a ten digit account number
        If tbLGPAN.TextLength < 9 Then
            MsgBox("You must provide a ten digit account number or an SSN.", MsgBoxStyle.Critical)
            tbLGPAN.Focus()
            Return False
        End If
        'check if the account number is numeric
        If IsNumeric(tbLGPAN.Text) = False Then
            MsgBox("You must provide a numeric account number or an SSN.", MsgBoxStyle.Critical)
            tbLGPAN.Focus()
            Return False
        End If
        'Do OneLINK check and Account number translation
        If OneLINK(SSN, AccNum, tbLGPAN.Text) = False Then
            Return False
        End If
        Return True 'data is valid
    End Function

    'clears all text boxes except received by
    Private Sub LGPClear()
        tbLGPBN.Clear()
        rbLGPCheck.Checked = False
        rbLGPCash.Checked = False
        tbLGPCN.Clear()
        tbLGPPA.Text = "0.00"
        tbLGPAN.Clear()
        lblLGPCN.Enabled = False 'disable both check number components
        tbLGPCN.Enabled = False
    End Sub

    Private Sub tbLGPPA_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbLGPPA.LostFocus
        If IsNumeric(tbLGPPA.Text) Then
            tbLGPPA.Text = Format(CDbl(tbLGPPA.Text), "########0.00")
        Else
            MsgBox("Payment amounts must be numeric.")
            tbLGPPA.Text = "0.00"
            tbLGPPA.Focus()
        End If
    End Sub

    Private Sub btnLGPClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLGPClear.Click
        'clears all text boxes
        tbLGPRB.Clear()
        LGPClear()
    End Sub

    Private Sub rbLGPCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLGPCheck.CheckedChanged
        If rbLGPCheck.Checked Then
            lblLGPCN.Enabled = True 'enable both check number components
            tbLGPCN.Enabled = True
        End If
    End Sub

    Private Sub rbLGPCash_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLGPCash.CheckedChanged
        If rbLGPCash.Checked Then 'if checked
            tbLGPCN.Clear() 'clear check number
            lblLGPCN.Enabled = False 'disable both check number components
            tbLGPCN.Enabled = False
        End If
    End Sub

#End Region

#Region " Acctg Functionality  "

    Private Sub btnAcctgProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAcctgProcess.Click
        Dim SSN As String
        Dim AccNum As String
        Me.Visible = False
        PleaseWait.Visible = True
        PleaseWait.Refresh()
        If AcctgDataCheck(SSN, AccNum) Then
            tbAcctgName.Focus()
            Dim ReceiptNumber As String
            Dim Reader As SqlDataReader
            Dim SelectStr As String = "Select Max(ReceiptNumber) From LGPPayments Where Name = '" & tbAcctgName.Text.Replace("'", "''") & "' and ReceivedBy = '" & tbAcctgRB.Text.Replace("'", "''") & "' and CheckNumber = '" & tbAcctgCN.Text & "'"   '<3>
            Dim DBSelectComm As New SqlCommand(SelectStr, DBConn)
            Dim InsertStr As String = "Insert Into LGPPayments (Name, AccountNumber, ReceivedBy,PaymentAmount,PaymentType," & _
                                   "CheckNumber) values ('" & tbAcctgName.Text.Replace("'", "''") & "', '" & AccNum & "', '" & tbAcctgRB.Text.Replace("'", "''") & "', " & tbAcctgPA.Text & ", '" & AcctgPayType() & "', '" & tbAcctgCN.Text & "')"     '<3>
            Dim DBInsertComm As New SqlCommand(InsertStr, DBConn)
            Try
                DBConn.Open()
                DBInsertComm.ExecuteNonQuery() 'insert data into DB
                Reader = DBSelectComm.ExecuteReader() 'Retrieve Max Receipt number for ParticipantName 
                Reader.Read() 'read first result
                ReceiptNumber = Reader.GetValue(0)
                DBConn.Close()
            Catch ex As Exception
                MsgBox("An error occured while trying to update the database.  Please try again.  If the problem continues then please contact Systems Support.", MsgBoxStyle.Critical)
                MsgBox(ex.ToString & vbLf & vbLf & ex.Message)
                Exit Sub
            End Try
            'Merge and print receipts
            FileOpen(1, "T:\ReceiptDat.txt", OpenMode.Output, OpenAccess.Write)
            WriteLine(1, "Name", "AccNum", "RecBy", "PayAmt", "Cash", "Check", "CheckNum", "RecNum", "Copy")
            If AccNum = "" Then AccNum = " " 'enter space if blank
            If rbAcctgCash.Checked Then 'if payment is cash
                'participants copy
                WriteLine(1, tbAcctgName.Text, AccNum, tbAcctgRB.Text, tbAcctgPA.Text, "X", "", "", ReceiptNumber, "Accounting Copy")
                'Accountings copy
                WriteLine(1, tbAcctgName.Text, AccNum, tbAcctgRB.Text, tbAcctgPA.Text, "X", "", "", ReceiptNumber, "Borrower Copy")
            Else 'if payment is check
                'participants copy
                WriteLine(1, tbAcctgName.Text, AccNum, tbAcctgRB.Text, tbAcctgPA.Text, "", "X", tbAcctgCN.Text, ReceiptNumber, "Accounting Copy")
                'Accountings copy
                WriteLine(1, tbAcctgName.Text, AccNum, tbAcctgRB.Text, tbAcctgPA.Text, "", "X", tbAcctgCN.Text, ReceiptNumber, "Borrower Copy")
            End If
            FileClose(1)
            'Print receipts
            PrintDocs("Q:\Payment Receipts\LGP Payment Receipt.doc")            '<2>
            'if group deposit box isn't checked
            If cbAcctgGD.Checked = False Then
                'enter LP50 comment
                If AddLP50(SSN, "MPYMT", "PMTRCPTS", "MS", "18", "Payment received from servicer.") = False Then                '<2>
                    MsgBox("There was an error while entering a comment into LP50.  Please contact Systems Support.")
                    End
                End If
            End If
            AcctgClear() 'clear data
        End If
        PleaseWait.Visible = False
        Me.Visible = True
    End Sub

    'returns a string containing an english description of the payment type
    Private Function AcctgPayType() As String
        If rbAcctgCash.Checked Then
            AcctgPayType = "Cash"
        Else
            AcctgPayType = "Check"
        End If
    End Function

    'returns payment type for TD22 comment
    Private Function AcctgCommentPayType() As String
        If rbAcctgCash.Checked Then
            AcctgCommentPayType = "Cash "
        Else
            AcctgCommentPayType = "Check"
        End If
    End Function

    'this function does data checks for the Acctg tab
    Private Function AcctgDataCheck(ByRef SSN As String, ByRef AccNum As String) As Boolean
        'check for received by
        If tbAcctgRB.TextLength = 0 Then
            MsgBox("You must provide a Received By name.", MsgBoxStyle.Critical)
            tbAcctgRB.Focus()
            Return False
        End If
        'check for check number if applicable
        If rbAcctgCheck.Checked Then
            If tbAcctgCN.TextLength = 0 Then
                MsgBox("You must provide a Check Number if the Payment Method is Check.", MsgBoxStyle.Critical)
                tbAcctgCN.Focus()
                Return False
            End If
        End If
        'check for total payment entry
        If tbAcctgPA.TextLength = 0 Then
            MsgBox("You must provide the payment amount.", MsgBoxStyle.Critical)
            tbAcctgPA.Focus()
            Return False
        End If
        '<3->
        If CDbl(tbAcctgPA.Text) = 0 Then
            MsgBox("You must provide a payment amount.", MsgBoxStyle.Critical)
            tbAcctgPA.Focus()
            Return False
        End If
        '</3>
        'check if the payment entry is numeric
        If IsNumeric(tbAcctgPA.Text) = False Then
            MsgBox("Payment amount must be numeric.", MsgBoxStyle.Critical)
            tbAcctgPA.Focus()
            Return False
        End If
        'if the group deposit check box isn't checked
        If cbAcctgGD.Checked = False Then
            'check for participant name
            If tbAcctgName.TextLength = 0 Then
                MsgBox("You must provide a Name.", MsgBoxStyle.Critical)
                tbAcctgName.Focus()
                Return False
            End If
            'check if the user provided a ten digit account number
            If tbAcctgAN.TextLength < 9 Then
                MsgBox("You must provide a ten digit account number.", MsgBoxStyle.Critical)
                tbAcctgAN.Focus()
                Return False
            End If
            'check if the account number is numeric
            If IsNumeric(tbAcctgAN.Text) = False Then
                MsgBox("You must provide a numeric account number.", MsgBoxStyle.Critical)
                tbAcctgAN.Focus()
                Return False
            End If
            'Do OneLINK check and Account number translation
            If OneLINK(SSN, AccNum, tbAcctgAN.Text) = False Then
                Return False
            End If
        End If
        Return True 'data is valid
    End Function

    'clears all text boxes except received by
    Private Sub AcctgClear()
        tbAcctgName.Clear()
        rbAcctgCash.Checked = False
        rbAcctgCheck.Checked = True
        tbAcctgCN.Clear()
        tbAcctgPA.Text = "0.00"
        tbAcctgAN.Clear()
        lblAcctgCN.Enabled = True 'disable both check number components
        tbAcctgCN.Enabled = True
        cbAcctgGD.Checked = False
        tbAcctgAN.Enabled = True
        tbAcctgName.Enabled = True
    End Sub

    Private Sub rbAcctgCash_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbAcctgCash.CheckedChanged
        If rbAcctgCash.Checked Then 'if checked
            tbAcctgCN.Clear() 'clear check number
            lblAcctgCN.Enabled = False 'disable both check number components
            tbAcctgCN.Enabled = False
        End If
    End Sub

    Private Sub rbAcctgCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbAcctgCheck.CheckedChanged
        If rbAcctgCheck.Checked Then
            lblAcctgCN.Enabled = True 'enable both check number components
            tbAcctgCN.Enabled = True
        End If
    End Sub

    Private Sub tbAcctgPA_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbAcctgPA.LostFocus
        If IsNumeric(tbAcctgPA.Text) Then
            tbAcctgPA.Text = Format(CDbl(tbAcctgPA.Text), "########0.00")
        Else
            MsgBox("Payment amounts must be numeric.")
            tbAcctgPA.Text = "0.00"
            tbAcctgPA.Focus()
        End If
    End Sub

    Private Sub btnAcctgClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAcctgClear.Click
        'clears all text boxes
        tbAcctgRB.Clear()
        AcctgClear()
    End Sub

    Private Sub cbAcctgGD_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAcctgGD.CheckedChanged
        If cbAcctgGD.Checked Then
            tbAcctgName.Clear()
            tbAcctgAN.Clear()
            tbAcctgName.Enabled = False
            tbAcctgAN.Enabled = False
        Else
            tbAcctgName.Enabled = True
            tbAcctgAN.Enabled = True
        End If
    End Sub

#End Region

    Private Sub btnReprint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReprint.Click
        If LastReceiptTypePrinted = "UESPPaymentReceipts" Then
            Dim SelectStr As String
            Dim DSPrint As New DataSet
            SelectStr = "select A.ReceiptNumber, A.ParticipantName, A.ReceivedBy, A.PaymentDate,B.BenefName, B.BenefAcctNum, B.BenefCheckNum, B.BenefAmt From UESPPayment A inner join UESPPaymentsBenef B on A.ReceiptNumber = B.ReceiptNumber Where A.ReceiptNumber = " & ReceiptNumber

            Try
                Dim DA As New SqlClient.SqlDataAdapter(SelectStr, DBConn)
                DA.Fill(DSPrint)
                Dim rpt As New rptPaymentReceipts
                rpt.SetDataSource(DSPrint.Tables(0))
                rpt.PrintToPrinter(1, False, 1, -1)
            Catch ex As Exception
                MsgBox("There was an error trying to print the last receipt.", MsgBoxStyle.Critical)
            End Try

        ElseIf LastReceiptTypePrinted <> "" Then
            PrintDocs(LastReceiptTypePrinted)
        Else
            MsgBox("There was an error trying to print the last receipt.", MsgBoxStyle.Critical)
        End If

    End Sub

#Region " Void Receipt "

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If VRDataValid() Then
            If cbPrgm.SelectedItem = "LGP" Then
                VoidReceipt("LGPPayments", "LGPVoids")
            ElseIf cbPrgm.SelectedItem = "LPP" Then
                VoidReceipt("LPPPayments", "LPPVoids")
            ElseIf cbPrgm.SelectedItem = "UESP" Then
                VoidReceipt("UESPPayment", "UESPVoids")
            ElseIf cbPrgm.SelectedItem = "TILP" Then
                VoidReceipt("TILPPayments", "TILPVoids")
            End If
        End If
    End Sub

    'this function does the voiding after doing a few more checks
    Sub VoidReceipt(ByVal Rtable As String, ByVal Vtable As String)
        Dim SQLComm As New SqlCommand
        Dim SQLReader As SqlDataReader
        DBConn.Open()
        SQLComm.Connection = DBConn
        'check for receipt match in payment table
        SQLComm.CommandText = "SELECT * FROM " & Rtable & " WHERE ReceiptNumber = '" & tbRecNum.Text & "'"
        SQLReader = SQLComm.ExecuteReader
        If SQLReader.Read = False Then
            DBConn.Close()
            MsgBox("The receipt number you entered couldn't be found in the Receipt Book database.  Please check the number you entered and try again.")
            Exit Sub
        End If
        SQLReader.Close()
        'check for already voided receipt
        SQLComm.CommandText = "SELECT * FROM " & Vtable & " WHERE ReceiptNumber = '" & tbRecNum.Text & "'"
        SQLReader = SQLComm.ExecuteReader
        If SQLReader.Read Then
            DBConn.Close()
            MsgBox("The receipt number you entered has already been voided.")
            Exit Sub
        End If
        SQLReader.Close()
        'insert data into the DB
        SQLComm.CommandText = "INSERT INTO " & Vtable & " (ReceiptNumber, Notes) VALUES (" & tbRecNum.Text & ", '" & tbNotes.Text.Replace("'", "''") & "')"
        SQLComm.ExecuteNonQuery()
        DBConn.Close()
        MsgBox("The receipt has been voided.", MsgBoxStyle.Information)
        'clear form
        cbPrgm.SelectedIndex = -1
        tbRecNum.Clear()
        tbNotes.Clear()
    End Sub

    'checks void receipts functionality data for validation
    Private Function VRDataValid() As Boolean
        'check for selected program
        If cbPrgm.SelectedIndex = -1 Then
            MsgBox("You must select a program.", MsgBoxStyle.Critical)
            Return False
        End If
        'check for receipt number
        If tbRecNum.TextLength = 0 Then
            MsgBox("You must enter a receipt number.", MsgBoxStyle.Critical)
            Return False
        End If
        'check for notes
        If tbNotes.TextLength = 0 Then
            MsgBox("You must enter a reason for voiding the receipt.", MsgBoxStyle.Critical)
            Return False
        End If
        Return True
    End Function

#End Region

#Region " Reports "

    Private Sub btnRptLPP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRptLPP.Click
        If LPPSD.Value() <= LPPED.Value() Then
            Dim CR As New CRLGPAndLPPPayments
            DALPPPayment.SelectCommand.Parameters.Item("@GTDate").Value = LPPSD.Value.ToShortDateString & " 00:00:00"
            DALPPPayment.SelectCommand.Parameters.Item("@LTDate").Value = LPPED.Value.ToShortDateString & " 23:59:59"
            DALPPPayment.Fill(DSLPPPayment, "Main")
            CR.SetDataSource(DSLPPPayment.Tables("Main"))
            CR.SummaryInfo.ReportTitle = "LPP Payments for dates " & LPPSD.Value.ToShortDateString & " - " & LPPED.Value.ToShortDateString
            CR.PrintToPrinter(1, False, 0, 0)
            DSLPPPayment.Tables("Main").Clear()
            MsgBox("Please retrieve your report from the printer.", MsgBoxStyle.Information)
        Else
            MsgBox("The start date must be the same as or come after the end date.")
        End If
    End Sub

    Private Sub btnRptLGP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRptLGP.Click
        If LGPSD.Value() <= LGPED.Value() Then
            Dim CR As New CRLGPAndLPPPayments
            DALGPPayment.SelectCommand.Parameters.Item("@GTDate").Value = LGPSD.Value.ToShortDateString & " 00:00:00"
            DALGPPayment.SelectCommand.Parameters.Item("@LTDate").Value = LGPED.Value.ToShortDateString & " 23:59:59"
            DALGPPayment.Fill(DSLGPPayment, "Main")
            CR.SetDataSource(DSLGPPayment.Tables("Main"))
            CR.SummaryInfo.ReportTitle = "LGP Payments for dates " & LGPSD.Value.ToShortDateString & " - " & LGPED.Value.ToShortDateString
            CR.PrintToPrinter(1, False, 0, 0)
            DSLGPPayment.Tables("Main").Clear()
            MsgBox("Please retrieve your report from the printer.", MsgBoxStyle.Information)
        Else
            MsgBox("The start date must be the same as or come after the end date.")
        End If
    End Sub

    Private Sub btnRptUESP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRptUESP.Click
        If UESPSD.Value() <= UESPED.Value() Then
            Try
                Dim DSPrint As New DataSet
                Dim SelectStr As String = "SELECT CASE WHEN UESPVoids.ReceiptNumber IS NULL THEN ' ' ELSE 'VOIDED' END AS Voided, dbo.UESPPayment.ReceiptNumber, dbo.UESPPayment.ParticipantName, dbo.UESPPayment.ReceivedBy, dbo.UESPPayment.PaymentDate, dbo.UESPPaymentsBenef.BenefName, dbo.UESPPaymentsBenef.BenefAcctNum, dbo.UESPPaymentsBenef.BenefCheckNum, dbo.UESPPaymentsBenef.BenefAmt, dbo.UESPVoids.Notes, dbo.UESPVoids.DateVoided, dbo.UESPPayment.ReceiptNumber AS Expr1, dbo.UESPPaymentsBenef.id FROM dbo.UESPPayment INNER JOIN dbo.UESPPaymentsBenef ON dbo.UESPPayment.ReceiptNumber = dbo.UESPPaymentsBenef.ReceiptNumber LEFT OUTER JOIN dbo.UESPVoids ON dbo.UESPPayment.ReceiptNumber = dbo.UESPVoids.ReceiptNumber WHERE (dbo.UESPPayment.PaymentDate >= '" & UESPSD.Value.ToShortDateString & " 00:00:00'" & ") AND (dbo.UESPPayment.PaymentDate <= '" & UESPED.Value.ToShortDateString & " 23:59:59'" & ") order by dbo.UESPPayment.PaymentDate ASC"
                Dim DA As New SqlClient.SqlDataAdapter(SelectStr, DBConn)
                DA.Fill(DSPrint)
                Dim CR As New rptCRUESPPayments
                CR.SetDataSource(DSPrint.Tables(0))
                CR.SummaryInfo.ReportTitle = "UESP Payments for dates " & UESPSD.Value.ToShortDateString & " - " & UESPED.Value.ToShortDateString
                CR.PrintToPrinter(1, False, 1, -1)
                MsgBox("Please retrieve your report from the printer.", MsgBoxStyle.Information)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            MsgBox("The end date must be the same as or come after the start date.")
        End If
    End Sub


    Private Sub btnVRptLPP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVRptLPP.Click
        If VLPPSD.Value() <= VLPPED.Value() Then
            Dim CR As New CRVoids
            DALPPVoid.SelectCommand.Parameters.Item("@GTDate").Value = VLPPSD.Value.ToShortDateString & " 00:00:00"
            DALPPVoid.SelectCommand.Parameters.Item("@LTDate").Value = VLPPED.Value.ToShortDateString & " 23:59:59"
            DALPPVoid.Fill(DSLPPVoid, "Main")
            CR.SetDataSource(DSLPPVoid.Tables("Main"))
            CR.SummaryInfo.ReportTitle = "LPP Voided Payments for dates " & VLPPSD.Value.ToShortDateString & " - " & VLPPED.Value.ToShortDateString
            CR.PrintToPrinter(1, False, 0, 0)
            DSLPPVoid.Tables("Main").Clear()
            MsgBox("Please retrieve your report from the printer.", MsgBoxStyle.Information)
        Else
            MsgBox("The start date must be the same as or come after the end date.")
        End If
    End Sub

    Private Sub btnVRptLGP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVRptLGP.Click
        If VLGPSD.Value() <= VLGPED.Value() Then
            Dim CR As New CRVoids
            DALGPVoid.SelectCommand.Parameters.Item("@GTDate").Value = VLGPSD.Value.ToShortDateString & " 00:00:00"
            DALGPVoid.SelectCommand.Parameters.Item("@LTDate").Value = VLGPED.Value.ToShortDateString & " 23:59:59"
            DALGPVoid.Fill(DSLGPVoid, "Main")
            CR.SetDataSource(DSLGPVoid.Tables("Main"))
            CR.SummaryInfo.ReportTitle = "LGP Voided Payments for dates " & VLGPSD.Value.ToShortDateString & " - " & VLGPED.Value.ToShortDateString
            CR.PrintToPrinter(1, False, 0, 0)
            DSLGPVoid.Tables("Main").Clear()
            MsgBox("Please retrieve your report from the printer.", MsgBoxStyle.Information)
        Else
            MsgBox("The start date must be the same as or come after the end date.")
        End If
    End Sub

    Private Sub btnVRptUESP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVRptUESP.Click
        If VUESPSD.Value() <= VUESPED.Value() Then
            Try
                Dim DSPrint As New DataSet
                Dim SelectStr As String = "SELECT ReceiptNumber, Notes, DateVoided from UESPVoids WHERE (DateVoided >= '" & UESPSD.Value.ToShortDateString & " 00:00:00'" & ") AND (DateVoided <= '" & UESPED.Value.ToShortDateString & " 23:59:59'" & ") order by DateVoided ASC"
                Dim DA As New SqlClient.SqlDataAdapter(SelectStr, DBConn)
                DA.Fill(DSPrint)
                Dim CR As New CRVoids
                CR.SetDataSource(DSPrint.Tables(0))
                CR.SummaryInfo.ReportTitle = "UESP Voided Payments for dates " & VUESPSD.Value.ToShortDateString & " - " & VUESPED.Value.ToShortDateString
                CR.PrintToPrinter(1, False, 1, -1)
                MsgBox("Please retrieve your report from the printer.", MsgBoxStyle.Information)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        Else
            MsgBox("The start date must be the same as or come after the end date.")
        End If
    End Sub

#End Region

#Region " TILP Functionality "
    Private Sub btnTILPProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTILPProcess.Click
        Me.Visible = False
        PleaseWait.Visible = True
        PleaseWait.Refresh()
        If TILPDataCheck() Then
            tbTILPBN.Focus()
            Dim ReceiptNumber As String
            Dim Reader As SqlDataReader
            Dim SelectStr As String = "Select Max(ReceiptNumber) From TILPPayments Where Name = '" & tbTILPBN.Text.Replace("'", "''") & "' and ReceivedBy = '" & tbTILPRB.Text.Replace("'", "''") & "' and CheckNumber = '" & tbTILPCN.Text & "'"
            Dim DBSelectComm As New SqlCommand(SelectStr, DBConn)
            Dim InsertStr As String = "Insert Into TILPPayments (Name, AccountNumber, ReceivedBy,PaymentAmount,PaymentType," & _
                                   "CheckNumber) values ('" & tbTILPBN.Text.Replace("'", "''") & "', '" & tbTILPAN.Text & "', '" & tbTILPRB.Text.Replace("'", "''") & "', " & tbTILPPA.Text & ", '" & TILPPayType() & "', '" & tbTILPCN.Text & "')"
            Dim DBInsertComm As New SqlCommand(InsertStr, DBConn)
            Try
                DBConn.Open()
                DBInsertComm.ExecuteNonQuery() 'insert data into DB
                Reader = DBSelectComm.ExecuteReader() 'Retrieve Max Receipt number for ParticipantName 
                Reader.Read() 'read first result
                ReceiptNumber = Reader.GetValue(0)
                DBConn.Close()
            Catch ex As Exception
                MsgBox("An error occured while trying to update the database.  Please try again.  If the problem continues then please contact Systems Support.", MsgBoxStyle.Critical)
                MsgBox(ex.ToString & vbLf & vbLf & ex.Message)
                Exit Sub
            End Try
            'Merge and print receipts
            FileOpen(1, "T:\ReceiptDat.txt", OpenMode.Output, OpenAccess.Write)
            WriteLine(1, "Name", "AccNum", "RecBy", "PayAmt", "Cash", "Check", "CheckNum", "RecNum", "Copy")
            If rbTILPCash.Checked Then 'if payment is cash
                'participants copy
                WriteLine(1, tbTILPBN.Text, tbTILPAN.Text, tbTILPRB.Text, tbTILPPA.Text, "X", "", "", ReceiptNumber, "Accounting Copy")
                'Accountings copy
                WriteLine(1, tbTILPBN.Text, tbTILPAN.Text, tbTILPRB.Text, tbTILPPA.Text, "X", "", "", ReceiptNumber, "Borrower Copy")
            Else 'if payment is check
                'participants copy
                WriteLine(1, tbTILPBN.Text, tbTILPAN.Text, tbTILPRB.Text, tbTILPPA.Text, "", "X", tbTILPCN.Text, ReceiptNumber, "Accounting Copy")
                'Accountings copy
                WriteLine(1, tbTILPBN.Text, tbTILPAN.Text, tbTILPRB.Text, tbTILPPA.Text, "", "X", tbTILPCN.Text, ReceiptNumber, "Borrower Copy")
            End If
            FileClose(1)
            'Print receipts
            LastReceiptTypePrinted = "Q:\Payment Receipts\" + PathModifier4TestMode + "TILP Payment Receipt.doc"
            PrintDocs("Q:\Payment Receipts\" + PathModifier4TestMode + "TILP Payment Receipt.doc")
            TILPClear() 'clear data
        End If
        PleaseWait.Visible = False
        Me.Visible = True
    End Sub
#End Region

    Private Sub rbTILPCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTILPCheck.CheckedChanged
        If rbTILPCheck.Checked Then
            lblTILPCN.Enabled = True 'enable both check number components
            tbTILPCN.Enabled = True
        End If
    End Sub

    Private Sub rbTILPCash_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTILPCash.CheckedChanged
        If rbTILPCash.Checked Then 'if checked
            tbTILPCN.Clear() 'clear check number
            lblTILPCN.Enabled = False 'disable both check number components
            tbTILPCN.Enabled = False
        End If
    End Sub

    'returns a string containing an english description of the payment type
    Private Function TILPPayType() As String
        If rbTILPCash.Checked Then
            TILPPayType = "Cash"
        Else
            TILPPayType = "Check"
        End If
    End Function

    'returns payment type for TD22 comment
    Private Function TILPCommentPayType() As String
        If rbTILPCash.Checked Then
            TILPCommentPayType = "Cash "
        Else
            TILPCommentPayType = "Check"
        End If
    End Function

    'this function does data checks for the LGP tab
    Private Function TILPDataCheck() As Boolean
        'check for participant name
        If tbTILPBN.TextLength = 0 Then
            MsgBox("You must provide a Borrower's Name.", MsgBoxStyle.Critical)
            tbTILPBN.Focus()
            Return False
        End If
        'check for received by
        If tbTILPRB.TextLength = 0 Then
            MsgBox("You must provide a Received By name.", MsgBoxStyle.Critical)
            tbTILPRB.Focus()
            Return False
        End If
        'check for payment method
        If rbTILPCash.Checked = False And rbTILPCheck.Checked = False Then
            MsgBox("You must select a Payment Method.", MsgBoxStyle.Critical)
            Return False
        End If
        '<3->
        If CDbl(tbTILPPA.Text) = 0 Then
            MsgBox("You must provide a payment amount.", MsgBoxStyle.Critical)
            tbTILPPA.Focus()
            Return False
        End If
        '</3>
        'check for check number if applicable
        If rbTILPCheck.Checked Then
            If tbTILPCN.TextLength = 0 Then
                MsgBox("You must provide a Check Number if the Payment Method is Check.", MsgBoxStyle.Critical)
                tbTILPCN.Focus()
                Return False
            End If
        End If
        'check for total payment entry
        If tbTILPPA.TextLength = 0 Then
            MsgBox("You must provide the payment amount.", MsgBoxStyle.Critical)
            tbTILPPA.Focus()
            Return False
        End If
        'check if the payment entry is numeric
        If IsNumeric(tbTILPPA.Text) = False Then
            MsgBox("Payment amount must be numeric.", MsgBoxStyle.Critical)
            tbTILPPA.Focus()
            Return False
        End If
        'check if the user provided a ten digit account number
        If tbTILPAN.TextLength < 9 And tbTILPAN.TextLength <> 5 Then
            MsgBox("You must provide a five digit account number, ten digit account number or an SSN.", MsgBoxStyle.Critical)
            tbTILPAN.Focus()
            Return False
        End If
        'check if the account number is numeric
        If IsNumeric(tbTILPAN.Text) = False Then
            MsgBox("You must provide a numeric account number or an SSN.", MsgBoxStyle.Critical)
            tbTILPAN.Focus()
            Return False
        End If
        Return True 'data is valid
    End Function

    'clears all text boxes except received by
    Private Sub TILPClear()
        tbTILPBN.Clear()
        rbTILPCheck.Checked = False
        rbTILPCash.Checked = False
        tbTILPCN.Clear()
        tbTILPPA.Text = "0.00"
        tbTILPAN.Clear()
        lblTILPCN.Enabled = False 'disable both check number components
        tbTILPCN.Enabled = False
    End Sub

    Private Sub tbTILPPA_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTILPPA.LostFocus
        If IsNumeric(tbTILPPA.Text) Then
            tbTILPPA.Text = Format(CDbl(tbTILPPA.Text), "########0.00")
        Else
            MsgBox("Payment amounts must be numeric.")
            tbTILPPA.Text = "0.00"
            tbTILPPA.Focus()
        End If
    End Sub

    Private Sub btnRptTILP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRptTILP.Click
        If TILPSD.Value() <= TILPED.Value() Then
            Dim CR As New CRLGPAndLPPPayments
            DATILPPayment.SelectCommand.Parameters.Item("@GTDate").Value = TILPSD.Value.ToShortDateString & " 00:00:00"
            DATILPPayment.SelectCommand.Parameters.Item("@LTDate").Value = TILPED.Value.ToShortDateString & " 23:59:59"
            DATILPPayment.Fill(DSTILPPayment, "Main")
            CR.SetDataSource(DSTILPPayment.Tables("Main"))
            CR.SummaryInfo.ReportTitle = "TILP Payments for dates " & TILPSD.Value.ToShortDateString & " - " & TILPED.Value.ToShortDateString
            CR.PrintToPrinter(1, False, 0, 0)
            DSTILPPayment.Tables("Main").Clear()
            MsgBox("Please retrieve your report from the printer.", MsgBoxStyle.Information)
        Else
            MsgBox("The start date must be the same as or come after the end date.")
        End If
    End Sub

    Private Sub btnVRptTILP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVRptTILP.Click
        If VTILPSD.Value() <= VTILPED.Value() Then
            Dim CR As New CRVoids
            DATILPVoid.SelectCommand.Parameters.Item("@GTDate").Value = VTILPSD.Value.ToShortDateString & " 00:00:00"
            DATILPVoid.SelectCommand.Parameters.Item("@LTDate").Value = VTILPED.Value.ToShortDateString & " 23:59:59"
            DATILPVoid.Fill(DSTILPVoid, "Main")
            CR.SetDataSource(DSTILPVoid.Tables("Main"))
            CR.SummaryInfo.ReportTitle = "TILP Voided Payments for dates " & VTILPSD.Value.ToShortDateString & " - " & VTILPED.Value.ToShortDateString
            CR.PrintToPrinter(1, False, 0, 0)
            DSTILPVoid.Tables("Main").Clear()
            MsgBox("Please retrieve your report from the printer.", MsgBoxStyle.Information)
        Else
            MsgBox("The start date must be the same as or come after the end date.")
        End If
    End Sub
    Public point00 As Point
    Private Sub dgBen_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles dgBen.Paint
        Dim row As Integer
        Dim yChange As Integer
        Dim y As Integer
        Dim RowText As String
        Dim hti As DataGrid.HitTestInfo
        'this Static boolean keeps this Paint code from
        'executing when the form is first loaded.
        Static FirstLoad As Boolean = True
        If Not FirstLoad Then
            Try
                'get the row that is currently displayed at top
                hti = dgBen.HitTest(point00)
                row = hti.Row
                yChange = dgBen.GetCellBounds(row, 0).Height + 1
                y = dgBen.GetCellBounds(row, 0).Top + 2
                While (y <= dgBen.Height - yChange And row < DSBen.Tables("Ben").Rows.Count)
                    RowText = (row + 1).ToString

                    e.Graphics.DrawString(RowText, dgBen.Font, New SolidBrush(Color.Black), 12, y)
                    y += yChange
                    row += 1
                End While
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            FirstLoad = False
        End If
    End Sub

    Private Sub dgBen_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgBen.CurrentCellChanged
        FigureTotal()
    End Sub

    Private Sub dgBen_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgBen.Leave
        FigureTotal()
    End Sub

End Class


#Region " Data migration "
'/*Update Payments table*/
'insert into UESPPayment (ReceiptNumber, ParticipantName, ReceivedBy, PaymentDate,PaymentAmount)
'select ReceiptNumber,ParticipantName,ReceivedBy,PaymentDate,PaymentAmount
'from UESPPayments



'/*Update PaymentsBenef table*/
'insert into UESPPaymentsBenef (ReceiptNumber, BenefName, BenefAcctNum,BenefCheckNum,BenefAmt)
'select ReceiptNumber
',BenefName1 as 'BenefName' ,BenefAcctNum1 as 'BenefAcctNum',CheckNumber,BenefAmt1 as 'BenefAmt'
'from UESPPayments
'union
'select ReceiptNumber
',BenefName2 as 'BenefName' ,BenefAcctNum2 as 'BenefAcctNum',CheckNumber,BenefAmt2 as 'BenefAmt'
'from UESPPayments where BenefName2 <> ''
'union
'select ReceiptNumber
',BenefName3 as 'BenefName' ,BenefAcctNum3 as 'BenefAcctNum',CheckNumber,BenefAmt3 as 'BenefAmt'
'from UESPPayments where BenefName3 <> ''
'union
'select ReceiptNumber
',BenefName4 as 'BenefName' ,BenefAcctNum4 as 'BenefAcctNum',CheckNumber,BenefAmt4 as 'BenefAmt'
'from UESPPayments where BenefName4 <> ''
'union
'select ReceiptNumber
',BenefName5 as 'BenefName' ,BenefAcctNum5 as 'BenefAcctNum',CheckNumber,BenefAmt5 as 'BenefAmt'
'from UESPPayments where BenefName5 <> ''
'union
'select ReceiptNumber
',BenefName6 as 'BenefName' ,BenefAcctNum6 as 'BenefAcctNum',CheckNumber,BenefAmt6 as 'BenefAmt'
'from UESPPayments where BenefName6 <> ''
'union
'select ReceiptNumber
',BenefName7 as 'BenefName' ,BenefAcctNum7 as 'BenefAcctNum',CheckNumber,BenefAmt7 as 'BenefAmt'
'from UESPPayments where BenefName7 <> ''
'union
'select ReceiptNumber
',BenefName8 as 'BenefName' ,BenefAcctNum8 as 'BenefAcctNum',CheckNumber,BenefAmt8 as 'BenefAmt'
'from UESPPayments where BenefName8 <> ''
'union
'select ReceiptNumber
',BenefName9 as 'BenefName' ,BenefAcctNum9 as 'BenefAcctNum',CheckNumber,BenefAmt9 as 'BenefAmt'
'from UESPPayments where BenefName9 <> ''
'union
'select ReceiptNumber
',BenefName10 as 'BenefName' ,BenefAcctNum10 as 'BenefAcctNum',CheckNumber,BenefAmt10 as 'BenefAmt'
'from UESPPayments where BenefName10 <> ''
#End Region