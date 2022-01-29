﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.2032
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Runtime.Serialization
Imports System.Xml


<Serializable(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Diagnostics.DebuggerStepThrough(),  _
 System.ComponentModel.ToolboxItem(true)>  _
Public Class TstDSTILPPayments
    Inherits DataSet
    
    Private tableTILPPayments As TILPPaymentsDataTable
    
    Public Sub New()
        MyBase.New
        Me.InitClass
        Dim schemaChangedHandler As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
        AddHandler Me.Tables.CollectionChanged, schemaChangedHandler
        AddHandler Me.Relations.CollectionChanged, schemaChangedHandler
    End Sub
    
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New
        Dim strSchema As String = CType(info.GetValue("XmlSchema", GetType(System.String)),String)
        If (Not (strSchema) Is Nothing) Then
            Dim ds As DataSet = New DataSet
            ds.ReadXmlSchema(New XmlTextReader(New System.IO.StringReader(strSchema)))
            If (Not (ds.Tables("TILPPayments")) Is Nothing) Then
                Me.Tables.Add(New TILPPaymentsDataTable(ds.Tables("TILPPayments")))
            End If
            Me.DataSetName = ds.DataSetName
            Me.Prefix = ds.Prefix
            Me.Namespace = ds.Namespace
            Me.Locale = ds.Locale
            Me.CaseSensitive = ds.CaseSensitive
            Me.EnforceConstraints = ds.EnforceConstraints
            Me.Merge(ds, false, System.Data.MissingSchemaAction.Add)
            Me.InitVars
        Else
            Me.InitClass
        End If
        Me.GetSerializationData(info, context)
        Dim schemaChangedHandler As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
        AddHandler Me.Tables.CollectionChanged, schemaChangedHandler
        AddHandler Me.Relations.CollectionChanged, schemaChangedHandler
    End Sub
    
    <System.ComponentModel.Browsable(false),  _
     System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)>  _
    Public ReadOnly Property TILPPayments As TILPPaymentsDataTable
        Get
            Return Me.tableTILPPayments
        End Get
    End Property
    
    Public Overrides Function Clone() As DataSet
        Dim cln As TstDSTILPPayments = CType(MyBase.Clone,TstDSTILPPayments)
        cln.InitVars
        Return cln
    End Function
    
    Protected Overrides Function ShouldSerializeTables() As Boolean
        Return false
    End Function
    
    Protected Overrides Function ShouldSerializeRelations() As Boolean
        Return false
    End Function
    
    Protected Overrides Sub ReadXmlSerializable(ByVal reader As XmlReader)
        Me.Reset
        Dim ds As DataSet = New DataSet
        ds.ReadXml(reader)
        If (Not (ds.Tables("TILPPayments")) Is Nothing) Then
            Me.Tables.Add(New TILPPaymentsDataTable(ds.Tables("TILPPayments")))
        End If
        Me.DataSetName = ds.DataSetName
        Me.Prefix = ds.Prefix
        Me.Namespace = ds.Namespace
        Me.Locale = ds.Locale
        Me.CaseSensitive = ds.CaseSensitive
        Me.EnforceConstraints = ds.EnforceConstraints
        Me.Merge(ds, false, System.Data.MissingSchemaAction.Add)
        Me.InitVars
    End Sub
    
    Protected Overrides Function GetSchemaSerializable() As System.Xml.Schema.XmlSchema
        Dim stream As System.IO.MemoryStream = New System.IO.MemoryStream
        Me.WriteXmlSchema(New XmlTextWriter(stream, Nothing))
        stream.Position = 0
        Return System.Xml.Schema.XmlSchema.Read(New XmlTextReader(stream), Nothing)
    End Function
    
    Friend Sub InitVars()
        Me.tableTILPPayments = CType(Me.Tables("TILPPayments"),TILPPaymentsDataTable)
        If (Not (Me.tableTILPPayments) Is Nothing) Then
            Me.tableTILPPayments.InitVars
        End If
    End Sub
    
    Private Sub InitClass()
        Me.DataSetName = "TstDSTILPPayments"
        Me.Prefix = ""
        Me.Namespace = "http://www.tempuri.org/TstDSTILPPayments.xsd"
        Me.Locale = New System.Globalization.CultureInfo("en-US")
        Me.CaseSensitive = false
        Me.EnforceConstraints = true
        Me.tableTILPPayments = New TILPPaymentsDataTable
        Me.Tables.Add(Me.tableTILPPayments)
    End Sub
    
    Private Function ShouldSerializeTILPPayments() As Boolean
        Return false
    End Function
    
    Private Sub SchemaChanged(ByVal sender As Object, ByVal e As System.ComponentModel.CollectionChangeEventArgs)
        If (e.Action = System.ComponentModel.CollectionChangeAction.Remove) Then
            Me.InitVars
        End If
    End Sub
    
    Public Delegate Sub TILPPaymentsRowChangeEventHandler(ByVal sender As Object, ByVal e As TILPPaymentsRowChangeEvent)
    
    <System.Diagnostics.DebuggerStepThrough()>  _
    Public Class TILPPaymentsDataTable
        Inherits DataTable
        Implements System.Collections.IEnumerable
        
        Private columnReceiptNumber As DataColumn
        
        Private columnName As DataColumn
        
        Private columnAccountNumber As DataColumn
        
        Private columnReceivedBy As DataColumn
        
        Private columnPaymentDate As DataColumn
        
        Private columnPaymentAmount As DataColumn
        
        Private columnPaymentType As DataColumn
        
        Private columnCheckNumber As DataColumn
        
        Private columnVoided As DataColumn
        
        Private columnNotes As DataColumn
        
        Friend Sub New()
            MyBase.New("TILPPayments")
            Me.InitClass
        End Sub
        
        Friend Sub New(ByVal table As DataTable)
            MyBase.New(table.TableName)
            If (table.CaseSensitive <> table.DataSet.CaseSensitive) Then
                Me.CaseSensitive = table.CaseSensitive
            End If
            If (table.Locale.ToString <> table.DataSet.Locale.ToString) Then
                Me.Locale = table.Locale
            End If
            If (table.Namespace <> table.DataSet.Namespace) Then
                Me.Namespace = table.Namespace
            End If
            Me.Prefix = table.Prefix
            Me.MinimumCapacity = table.MinimumCapacity
            Me.DisplayExpression = table.DisplayExpression
        End Sub
        
        <System.ComponentModel.Browsable(false)>  _
        Public ReadOnly Property Count As Integer
            Get
                Return Me.Rows.Count
            End Get
        End Property
        
        Friend ReadOnly Property ReceiptNumberColumn As DataColumn
            Get
                Return Me.columnReceiptNumber
            End Get
        End Property
        
        Friend ReadOnly Property NameColumn As DataColumn
            Get
                Return Me.columnName
            End Get
        End Property
        
        Friend ReadOnly Property AccountNumberColumn As DataColumn
            Get
                Return Me.columnAccountNumber
            End Get
        End Property
        
        Friend ReadOnly Property ReceivedByColumn As DataColumn
            Get
                Return Me.columnReceivedBy
            End Get
        End Property
        
        Friend ReadOnly Property PaymentDateColumn As DataColumn
            Get
                Return Me.columnPaymentDate
            End Get
        End Property
        
        Friend ReadOnly Property PaymentAmountColumn As DataColumn
            Get
                Return Me.columnPaymentAmount
            End Get
        End Property
        
        Friend ReadOnly Property PaymentTypeColumn As DataColumn
            Get
                Return Me.columnPaymentType
            End Get
        End Property
        
        Friend ReadOnly Property CheckNumberColumn As DataColumn
            Get
                Return Me.columnCheckNumber
            End Get
        End Property
        
        Friend ReadOnly Property VoidedColumn As DataColumn
            Get
                Return Me.columnVoided
            End Get
        End Property
        
        Friend ReadOnly Property NotesColumn As DataColumn
            Get
                Return Me.columnNotes
            End Get
        End Property
        
        Public Default ReadOnly Property Item(ByVal index As Integer) As TILPPaymentsRow
            Get
                Return CType(Me.Rows(index),TILPPaymentsRow)
            End Get
        End Property
        
        Public Event TILPPaymentsRowChanged As TILPPaymentsRowChangeEventHandler
        
        Public Event TILPPaymentsRowChanging As TILPPaymentsRowChangeEventHandler
        
        Public Event TILPPaymentsRowDeleted As TILPPaymentsRowChangeEventHandler
        
        Public Event TILPPaymentsRowDeleting As TILPPaymentsRowChangeEventHandler
        
        Public Overloads Sub AddTILPPaymentsRow(ByVal row As TILPPaymentsRow)
            Me.Rows.Add(row)
        End Sub
        
        Public Overloads Function AddTILPPaymentsRow(ByVal Name As String, ByVal AccountNumber As String, ByVal ReceivedBy As String, ByVal PaymentDate As Date, ByVal PaymentAmount As Decimal, ByVal PaymentType As String, ByVal CheckNumber As String, ByVal Voided As String, ByVal Notes As String) As TILPPaymentsRow
            Dim rowTILPPaymentsRow As TILPPaymentsRow = CType(Me.NewRow,TILPPaymentsRow)
            rowTILPPaymentsRow.ItemArray = New Object() {Nothing, Name, AccountNumber, ReceivedBy, PaymentDate, PaymentAmount, PaymentType, CheckNumber, Voided, Notes}
            Me.Rows.Add(rowTILPPaymentsRow)
            Return rowTILPPaymentsRow
        End Function
        
        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return Me.Rows.GetEnumerator
        End Function
        
        Public Overrides Function Clone() As DataTable
            Dim cln As TILPPaymentsDataTable = CType(MyBase.Clone,TILPPaymentsDataTable)
            cln.InitVars
            Return cln
        End Function
        
        Protected Overrides Function CreateInstance() As DataTable
            Return New TILPPaymentsDataTable
        End Function
        
        Friend Sub InitVars()
            Me.columnReceiptNumber = Me.Columns("ReceiptNumber")
            Me.columnName = Me.Columns("Name")
            Me.columnAccountNumber = Me.Columns("AccountNumber")
            Me.columnReceivedBy = Me.Columns("ReceivedBy")
            Me.columnPaymentDate = Me.Columns("PaymentDate")
            Me.columnPaymentAmount = Me.Columns("PaymentAmount")
            Me.columnPaymentType = Me.Columns("PaymentType")
            Me.columnCheckNumber = Me.Columns("CheckNumber")
            Me.columnVoided = Me.Columns("Voided")
            Me.columnNotes = Me.Columns("Notes")
        End Sub
        
        Private Sub InitClass()
            Me.columnReceiptNumber = New DataColumn("ReceiptNumber", GetType(System.Int64), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnReceiptNumber)
            Me.columnName = New DataColumn("Name", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnName)
            Me.columnAccountNumber = New DataColumn("AccountNumber", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnAccountNumber)
            Me.columnReceivedBy = New DataColumn("ReceivedBy", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnReceivedBy)
            Me.columnPaymentDate = New DataColumn("PaymentDate", GetType(System.DateTime), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnPaymentDate)
            Me.columnPaymentAmount = New DataColumn("PaymentAmount", GetType(System.Decimal), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnPaymentAmount)
            Me.columnPaymentType = New DataColumn("PaymentType", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnPaymentType)
            Me.columnCheckNumber = New DataColumn("CheckNumber", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnCheckNumber)
            Me.columnVoided = New DataColumn("Voided", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnVoided)
            Me.columnNotes = New DataColumn("Notes", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnNotes)
            Me.columnReceiptNumber.AutoIncrement = true
            Me.columnReceiptNumber.AllowDBNull = false
            Me.columnReceiptNumber.ReadOnly = true
            Me.columnVoided.ReadOnly = true
        End Sub
        
        Public Function NewTILPPaymentsRow() As TILPPaymentsRow
            Return CType(Me.NewRow,TILPPaymentsRow)
        End Function
        
        Protected Overrides Function NewRowFromBuilder(ByVal builder As DataRowBuilder) As DataRow
            Return New TILPPaymentsRow(builder)
        End Function
        
        Protected Overrides Function GetRowType() As System.Type
            Return GetType(TILPPaymentsRow)
        End Function
        
        Protected Overrides Sub OnRowChanged(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowChanged(e)
            If (Not (Me.TILPPaymentsRowChangedEvent) Is Nothing) Then
                RaiseEvent TILPPaymentsRowChanged(Me, New TILPPaymentsRowChangeEvent(CType(e.Row,TILPPaymentsRow), e.Action))
            End If
        End Sub
        
        Protected Overrides Sub OnRowChanging(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowChanging(e)
            If (Not (Me.TILPPaymentsRowChangingEvent) Is Nothing) Then
                RaiseEvent TILPPaymentsRowChanging(Me, New TILPPaymentsRowChangeEvent(CType(e.Row,TILPPaymentsRow), e.Action))
            End If
        End Sub
        
        Protected Overrides Sub OnRowDeleted(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowDeleted(e)
            If (Not (Me.TILPPaymentsRowDeletedEvent) Is Nothing) Then
                RaiseEvent TILPPaymentsRowDeleted(Me, New TILPPaymentsRowChangeEvent(CType(e.Row,TILPPaymentsRow), e.Action))
            End If
        End Sub
        
        Protected Overrides Sub OnRowDeleting(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowDeleting(e)
            If (Not (Me.TILPPaymentsRowDeletingEvent) Is Nothing) Then
                RaiseEvent TILPPaymentsRowDeleting(Me, New TILPPaymentsRowChangeEvent(CType(e.Row,TILPPaymentsRow), e.Action))
            End If
        End Sub
        
        Public Sub RemoveTILPPaymentsRow(ByVal row As TILPPaymentsRow)
            Me.Rows.Remove(row)
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThrough()>  _
    Public Class TILPPaymentsRow
        Inherits DataRow
        
        Private tableTILPPayments As TILPPaymentsDataTable
        
        Friend Sub New(ByVal rb As DataRowBuilder)
            MyBase.New(rb)
            Me.tableTILPPayments = CType(Me.Table,TILPPaymentsDataTable)
        End Sub
        
        Public Property ReceiptNumber As Long
            Get
                Return CType(Me(Me.tableTILPPayments.ReceiptNumberColumn),Long)
            End Get
            Set
                Me(Me.tableTILPPayments.ReceiptNumberColumn) = value
            End Set
        End Property
        
        Public Property Name As String
            Get
                Try 
                    Return CType(Me(Me.tableTILPPayments.NameColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("Cannot get value because it is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableTILPPayments.NameColumn) = value
            End Set
        End Property
        
        Public Property AccountNumber As String
            Get
                Try 
                    Return CType(Me(Me.tableTILPPayments.AccountNumberColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("Cannot get value because it is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableTILPPayments.AccountNumberColumn) = value
            End Set
        End Property
        
        Public Property ReceivedBy As String
            Get
                Try 
                    Return CType(Me(Me.tableTILPPayments.ReceivedByColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("Cannot get value because it is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableTILPPayments.ReceivedByColumn) = value
            End Set
        End Property
        
        Public Property PaymentDate As Date
            Get
                Try 
                    Return CType(Me(Me.tableTILPPayments.PaymentDateColumn),Date)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("Cannot get value because it is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableTILPPayments.PaymentDateColumn) = value
            End Set
        End Property
        
        Public Property PaymentAmount As Decimal
            Get
                Try 
                    Return CType(Me(Me.tableTILPPayments.PaymentAmountColumn),Decimal)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("Cannot get value because it is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableTILPPayments.PaymentAmountColumn) = value
            End Set
        End Property
        
        Public Property PaymentType As String
            Get
                Try 
                    Return CType(Me(Me.tableTILPPayments.PaymentTypeColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("Cannot get value because it is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableTILPPayments.PaymentTypeColumn) = value
            End Set
        End Property
        
        Public Property CheckNumber As String
            Get
                Try 
                    Return CType(Me(Me.tableTILPPayments.CheckNumberColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("Cannot get value because it is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableTILPPayments.CheckNumberColumn) = value
            End Set
        End Property
        
        Public Property Voided As String
            Get
                Try 
                    Return CType(Me(Me.tableTILPPayments.VoidedColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("Cannot get value because it is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableTILPPayments.VoidedColumn) = value
            End Set
        End Property
        
        Public Property Notes As String
            Get
                Try 
                    Return CType(Me(Me.tableTILPPayments.NotesColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("Cannot get value because it is DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableTILPPayments.NotesColumn) = value
            End Set
        End Property
        
        Public Function IsNameNull() As Boolean
            Return Me.IsNull(Me.tableTILPPayments.NameColumn)
        End Function
        
        Public Sub SetNameNull()
            Me(Me.tableTILPPayments.NameColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsAccountNumberNull() As Boolean
            Return Me.IsNull(Me.tableTILPPayments.AccountNumberColumn)
        End Function
        
        Public Sub SetAccountNumberNull()
            Me(Me.tableTILPPayments.AccountNumberColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsReceivedByNull() As Boolean
            Return Me.IsNull(Me.tableTILPPayments.ReceivedByColumn)
        End Function
        
        Public Sub SetReceivedByNull()
            Me(Me.tableTILPPayments.ReceivedByColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsPaymentDateNull() As Boolean
            Return Me.IsNull(Me.tableTILPPayments.PaymentDateColumn)
        End Function
        
        Public Sub SetPaymentDateNull()
            Me(Me.tableTILPPayments.PaymentDateColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsPaymentAmountNull() As Boolean
            Return Me.IsNull(Me.tableTILPPayments.PaymentAmountColumn)
        End Function
        
        Public Sub SetPaymentAmountNull()
            Me(Me.tableTILPPayments.PaymentAmountColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsPaymentTypeNull() As Boolean
            Return Me.IsNull(Me.tableTILPPayments.PaymentTypeColumn)
        End Function
        
        Public Sub SetPaymentTypeNull()
            Me(Me.tableTILPPayments.PaymentTypeColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsCheckNumberNull() As Boolean
            Return Me.IsNull(Me.tableTILPPayments.CheckNumberColumn)
        End Function
        
        Public Sub SetCheckNumberNull()
            Me(Me.tableTILPPayments.CheckNumberColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsVoidedNull() As Boolean
            Return Me.IsNull(Me.tableTILPPayments.VoidedColumn)
        End Function
        
        Public Sub SetVoidedNull()
            Me(Me.tableTILPPayments.VoidedColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsNotesNull() As Boolean
            Return Me.IsNull(Me.tableTILPPayments.NotesColumn)
        End Function
        
        Public Sub SetNotesNull()
            Me(Me.tableTILPPayments.NotesColumn) = System.Convert.DBNull
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThrough()>  _
    Public Class TILPPaymentsRowChangeEvent
        Inherits EventArgs
        
        Private eventRow As TILPPaymentsRow
        
        Private eventAction As DataRowAction
        
        Public Sub New(ByVal row As TILPPaymentsRow, ByVal action As DataRowAction)
            MyBase.New
            Me.eventRow = row
            Me.eventAction = action
        End Sub
        
        Public ReadOnly Property Row As TILPPaymentsRow
            Get
                Return Me.eventRow
            End Get
        End Property
        
        Public ReadOnly Property Action As DataRowAction
            Get
                Return Me.eventAction
            End Get
        End Property
    End Class
End Class
