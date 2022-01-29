Public Class frmFunctions
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents daSAD As System.Data.OleDb.OleDbDataAdapter
    Friend WithEvents conSAD As System.Data.OleDb.OleDbConnection
    Friend WithEvents daAgents As System.Data.OleDb.OleDbDataAdapter
    Friend WithEvents OleDbSelectCommand2 As System.Data.OleDb.OleDbCommand
    Friend WithEvents OleDbInsertCommand2 As System.Data.OleDb.OleDbCommand
    Friend WithEvents OleDbUpdateCommand2 As System.Data.OleDb.OleDbCommand
    Friend WithEvents OleDbDeleteCommand2 As System.Data.OleDb.OleDbCommand
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents TableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents Col1 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Col2 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataSet1 As SAD.DataSet1
    Friend WithEvents OleDbSelectCommand1 As System.Data.OleDb.OleDbCommand
    Friend WithEvents OleDbInsertCommand1 As System.Data.OleDb.OleDbCommand
    Friend WithEvents OleDbUpdateCommand1 As System.Data.OleDb.OleDbCommand
    Friend WithEvents OleDbDeleteCommand1 As System.Data.OleDb.OleDbCommand
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.daSAD = New System.Data.OleDb.OleDbDataAdapter
        Me.OleDbDeleteCommand1 = New System.Data.OleDb.OleDbCommand
        Me.conSAD = New System.Data.OleDb.OleDbConnection
        Me.OleDbInsertCommand1 = New System.Data.OleDb.OleDbCommand
        Me.OleDbSelectCommand1 = New System.Data.OleDb.OleDbCommand
        Me.OleDbUpdateCommand1 = New System.Data.OleDb.OleDbCommand
        Me.daAgents = New System.Data.OleDb.OleDbDataAdapter
        Me.OleDbDeleteCommand2 = New System.Data.OleDb.OleDbCommand
        Me.OleDbInsertCommand2 = New System.Data.OleDb.OleDbCommand
        Me.OleDbSelectCommand2 = New System.Data.OleDb.OleDbCommand
        Me.OleDbUpdateCommand2 = New System.Data.OleDb.OleDbCommand
        Me.DataGrid1 = New System.Windows.Forms.DataGrid
        Me.DataSet1 = New SAD.DataSet1
        Me.TableStyle1 = New System.Windows.Forms.DataGridTableStyle
        Me.Col1 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.Col2 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.btnSave = New System.Windows.Forms.Button
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'daSAD
        '
        Me.daSAD.DeleteCommand = Me.OleDbDeleteCommand1
        Me.daSAD.InsertCommand = Me.OleDbInsertCommand1
        Me.daSAD.SelectCommand = Me.OleDbSelectCommand1
        Me.daSAD.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "lstFunctions", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("FunctionName", "FunctionName"), New System.Data.Common.DataColumnMapping("Description", "Description")})})
        Me.daSAD.UpdateCommand = Me.OleDbUpdateCommand1
        '
        'OleDbDeleteCommand1
        '
        Me.OleDbDeleteCommand1.CommandText = "DELETE FROM lstFunctions WHERE (FunctionName = ?) AND (FunctionSuffix = ? OR ? IS" & _
        " NULL AND FunctionSuffix IS NULL)"
        Me.OleDbDeleteCommand1.Connection = Me.conSAD
        Me.OleDbDeleteCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_FunctionName", System.Data.OleDb.OleDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "FunctionName", System.Data.DataRowVersion.Original, Nothing))
        Me.OleDbDeleteCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_FunctionSuffix", System.Data.OleDb.OleDbType.VarChar, 75, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Description", System.Data.DataRowVersion.Original, Nothing))
        Me.OleDbDeleteCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_FunctionSuffix1", System.Data.OleDb.OleDbType.VarChar, 75, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Description", System.Data.DataRowVersion.Original, Nothing))
        '
        'conSAD
        '
        Me.conSAD.ConnectionString = "Integrated Security=SSPI;Packet Size=4096;Data Source=""BART\BART"";Tag with column" & _
        " collation when possible=False;Initial Catalog=SAD;Use Procedure for Prepare=1;A" & _
        "uto Translate=True;Persist Security Info=False;Provider=""SQLOLEDB.1"";Workstation" & _
        " ID=""LPP-1493"";Use Encryption for Data=False"
        '
        'OleDbInsertCommand1
        '
        Me.OleDbInsertCommand1.CommandText = "INSERT INTO lstFunctions(FunctionName, FunctionSuffix) VALUES (?, ?); SELECT Func" & _
        "tionName AS FunctionName, FunctionSuffix AS Description FROM lstFunctions WHERE " & _
        "(FunctionName = ?)"
        Me.OleDbInsertCommand1.Connection = Me.conSAD
        Me.OleDbInsertCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("FunctionName", System.Data.OleDb.OleDbType.VarChar, 50, "FunctionName"))
        Me.OleDbInsertCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("FunctionSuffix", System.Data.OleDb.OleDbType.VarChar, 75, "Description"))
        Me.OleDbInsertCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("Select_FunctionName", System.Data.OleDb.OleDbType.VarChar, 50, "FunctionName"))
        '
        'OleDbSelectCommand1
        '
        Me.OleDbSelectCommand1.CommandText = "SELECT FunctionName AS FunctionName, FunctionSuffix AS Description FROM lstFuncti" & _
        "ons"
        Me.OleDbSelectCommand1.Connection = Me.conSAD
        '
        'OleDbUpdateCommand1
        '
        Me.OleDbUpdateCommand1.CommandText = "UPDATE lstFunctions SET FunctionName = ?, FunctionSuffix = ? WHERE (FunctionName " & _
        "= ?) AND (FunctionSuffix = ? OR ? IS NULL AND FunctionSuffix IS NULL); SELECT Fu" & _
        "nctionName AS FunctionName, FunctionSuffix AS Description FROM lstFunctions WHER" & _
        "E (FunctionName = ?)"
        Me.OleDbUpdateCommand1.Connection = Me.conSAD
        Me.OleDbUpdateCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("FunctionName", System.Data.OleDb.OleDbType.VarChar, 50, "FunctionName"))
        Me.OleDbUpdateCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("FunctionSuffix", System.Data.OleDb.OleDbType.VarChar, 75, "Description"))
        Me.OleDbUpdateCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_FunctionName", System.Data.OleDb.OleDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "FunctionName", System.Data.DataRowVersion.Original, Nothing))
        Me.OleDbUpdateCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_FunctionSuffix", System.Data.OleDb.OleDbType.VarChar, 75, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Description", System.Data.DataRowVersion.Original, Nothing))
        Me.OleDbUpdateCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_FunctionSuffix1", System.Data.OleDb.OleDbType.VarChar, 75, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Description", System.Data.DataRowVersion.Original, Nothing))
        Me.OleDbUpdateCommand1.Parameters.Add(New System.Data.OleDb.OleDbParameter("Select_FunctionName", System.Data.OleDb.OleDbType.VarChar, 50, "FunctionName"))
        '
        'daAgents
        '
        Me.daAgents.DeleteCommand = Me.OleDbDeleteCommand2
        Me.daAgents.InsertCommand = Me.OleDbInsertCommand2
        Me.daAgents.SelectCommand = Me.OleDbSelectCommand2
        Me.daAgents.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "lstAgents", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("AgentName", "AgentName"), New System.Data.Common.DataColumnMapping("Status", "Status"), New System.Data.Common.DataColumnMapping("Email", "Email")})})
        Me.daAgents.UpdateCommand = Me.OleDbUpdateCommand2
        '
        'OleDbDeleteCommand2
        '
        Me.OleDbDeleteCommand2.CommandText = "DELETE FROM lstAgents WHERE (AgentName = ?) AND (Email = ?) AND (Status = ?)"
        Me.OleDbDeleteCommand2.Connection = Me.conSAD
        Me.OleDbDeleteCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_AgentName", System.Data.OleDb.OleDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "AgentName", System.Data.DataRowVersion.Original, Nothing))
        Me.OleDbDeleteCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_Email", System.Data.OleDb.OleDbType.VarChar, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Email", System.Data.DataRowVersion.Original, Nothing))
        Me.OleDbDeleteCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_Status", System.Data.OleDb.OleDbType.VarChar, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Status", System.Data.DataRowVersion.Original, Nothing))
        '
        'OleDbInsertCommand2
        '
        Me.OleDbInsertCommand2.CommandText = "INSERT INTO lstAgents(AgentName, Status, Email) VALUES (?, ?, ?); SELECT AgentNam" & _
        "e, Status, Email FROM lstAgents WHERE (AgentName = ?)"
        Me.OleDbInsertCommand2.Connection = Me.conSAD
        Me.OleDbInsertCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("AgentName", System.Data.OleDb.OleDbType.VarChar, 50, "AgentName"))
        Me.OleDbInsertCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Status", System.Data.OleDb.OleDbType.VarChar, 1, "Status"))
        Me.OleDbInsertCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Email", System.Data.OleDb.OleDbType.VarChar, 1, "Email"))
        Me.OleDbInsertCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Select_AgentName", System.Data.OleDb.OleDbType.VarChar, 50, "AgentName"))
        '
        'OleDbSelectCommand2
        '
        Me.OleDbSelectCommand2.CommandText = "SELECT AgentName, Status, Email FROM lstAgents"
        Me.OleDbSelectCommand2.Connection = Me.conSAD
        '
        'OleDbUpdateCommand2
        '
        Me.OleDbUpdateCommand2.CommandText = "UPDATE lstAgents SET AgentName = ?, Status = ?, Email = ? WHERE (AgentName = ?) A" & _
        "ND (Email = ?) AND (Status = ?); SELECT AgentName, Status, Email FROM lstAgents " & _
        "WHERE (AgentName = ?)"
        Me.OleDbUpdateCommand2.Connection = Me.conSAD
        Me.OleDbUpdateCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("AgentName", System.Data.OleDb.OleDbType.VarChar, 50, "AgentName"))
        Me.OleDbUpdateCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Status", System.Data.OleDb.OleDbType.VarChar, 1, "Status"))
        Me.OleDbUpdateCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Email", System.Data.OleDb.OleDbType.VarChar, 1, "Email"))
        Me.OleDbUpdateCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_AgentName", System.Data.OleDb.OleDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "AgentName", System.Data.DataRowVersion.Original, Nothing))
        Me.OleDbUpdateCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_Email", System.Data.OleDb.OleDbType.VarChar, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Email", System.Data.DataRowVersion.Original, Nothing))
        Me.OleDbUpdateCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Original_Status", System.Data.OleDb.OleDbType.VarChar, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Status", System.Data.DataRowVersion.Original, Nothing))
        Me.OleDbUpdateCommand2.Parameters.Add(New System.Data.OleDb.OleDbParameter("Select_AgentName", System.Data.OleDb.OleDbType.VarChar, 50, "AgentName"))
        '
        'DataGrid1
        '
        Me.DataGrid1.CaptionText = "Business System Functions"
        Me.DataGrid1.DataMember = ""
        Me.DataGrid1.DataSource = Me.DataSet1.lstFunctions
        Me.DataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGrid1.Location = New System.Drawing.Point(8, 8)
        Me.DataGrid1.Name = "DataGrid1"
        Me.DataGrid1.Size = New System.Drawing.Size(848, 360)
        Me.DataGrid1.TabIndex = 0
        Me.DataGrid1.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.TableStyle1})
        '
        'DataSet1
        '
        Me.DataSet1.DataSetName = "DataSet1"
        Me.DataSet1.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'TableStyle1
        '
        Me.TableStyle1.AlternatingBackColor = System.Drawing.SystemColors.ScrollBar
        Me.TableStyle1.DataGrid = Me.DataGrid1
        Me.TableStyle1.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.Col1, Me.Col2})
        Me.TableStyle1.HeaderForeColor = System.Drawing.Color.White
        Me.TableStyle1.MappingName = "lstFunctions"
        '
        'Col1
        '
        Me.Col1.Format = ""
        Me.Col1.FormatInfo = Nothing
        Me.Col1.HeaderText = "Function"
        Me.Col1.MappingName = "FunctionName"
        Me.Col1.Width = 325
        '
        'Col2
        '
        Me.Col2.Format = ""
        Me.Col2.FormatInfo = Nothing
        Me.Col2.HeaderText = "Description"
        Me.Col2.MappingName = "Description"
        Me.Col2.Width = 486
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(168, 384)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "Save"
        '
        'frmFunctions
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(1017, 693)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.DataGrid1)
        Me.Name = "frmFunctions"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Business Systems Functions"
        CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSet1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    'Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim name As String
    '    Dim suffix As String
    '    Dim response As MsgBoxResult
    '    Dim SQLStr As String
    '    'Dim cmd As New SqlCommand("", conSAD)
    '    'Dim TextLine As String

    '    FileOpen(1, "C:\Windows\Temp\functions.txt", OpenMode.Input)   ' Open file.
    '    'FileOpen(2, "C:\Windows\Temp\functions2.txt", OpenMode.Output)    ' Open file.

    '    Try
    '        conSAD.Open()
    '        'Dim P As New SqlParameter("@FunctionName", SqlDbType.VarChar)

    '        Do While Not EOF(1)   ' Loop until end of file.
    '            Input(1, name)
    '            Input(1, suffix)
    '            'SQLStr = "INSERT INTO lstFunctions(FunctionName, FunctionSuffix) VALUES ('" & name & "','" & suffix & "');"
    '            'cmd.CommandText = SQLStr
    '            'daSAD.InsertCommand = cmd
    '            daSAD.InsertCommand.CommandText() = "INSERT INTO lstFunctions(FunctionName, FunctionSuffix) VALUES ('" & name & "','" & suffix & "');"
    '            daSAD.InsertCommand.ExecuteNonQuery()
    '        Loop
    '        conSAD.Close()
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    '    'FileClose(2)   ' Close file.
    '    FileClose(1)   ' Close file.

    '    response = MsgBox("Done")
    '    End
    'End Sub

    'Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim name As String
    '    Dim response As MsgBoxResult


    '    DataGrid1.DataSource() = daSAD.SelectCommand.CommandText = "SELECT * FROM lstFunctions"




    '    FileOpen(1, "C:\Windows\Temp\agents.txt", OpenMode.Input)   ' Open file.

    '    Try
    '        conSAD.Open()

    '        Do While Not EOF(1)   ' Loop until end of file.
    '            Input(1, name)
    '            daSAD.InsertCommand.CommandText() = "INSERT INTO lstAgents(AgentName) VALUES ('" & name & "');"
    '            daSAD.InsertCommand.ExecuteNonQuery()
    '        Loop
    '        conSAD.Close()
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    '    FileClose(1)   ' Close file.

    '    response = MsgBox("Done")
    '    End
    'End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DataGrid1.TableStyles.Clear()
        DataGrid1.TableStyles.Add(MakeTableStyle)
        daSAD.Fill(DataSet1)

    End Sub

    Private Function MakeTableStyle() As DataGridTableStyle
        Dim dgTableStyle As New DataGridTableStyle
        dgTableStyle.MappingName = "lstFunctions"


        Dim column1 As New DataGridTextBoxColumn
        With column1
            .TextBox.MaxLength = 50
            .MappingName = "FunctionName"
            .HeaderText = "Function"
            .Width = 310
        End With
        'AddHandler column1.TextBox.KeyPress, AddressOf over50Keys
        dgTableStyle.GridColumnStyles.Add(column1)

        Dim column2 As New DataGridTextBoxColumn
        With column2
            .TextBox.MaxLength = 75
            .MappingName = "Description"
            .HeaderText = "Description"
            .Width = 475
        End With
        'AddHandler column2.TextBox.KeyPress, AddressOf over50Keys
        dgTableStyle.GridColumnStyles.Add(column2)



        Return dgTableStyle
    End Function


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim updateSet As DataSet
        If DataSet1.HasChanges Then
            Try
                updateSet = DataSet1.GetChanges
                Try

                    If (Not (updateSet) Is Nothing) Then
                        conSAD.Open()
                        daSAD.Update(updateSet)
                        MsgBox("Save Successful!")
                    End If
                Catch updateException As System.Exception
                    Throw updateException
                Finally
                    conSAD.Close()
                End Try

                DataSet1.AcceptChanges()
                DataGrid1.Refresh()

                'Catch ex As Exception
            Catch ex As Exception
                'Dim customErrorMessage As String
                'customErrorMessage = "Concurrency violation" & vbCrLf
                'customErrorMessage += CType(ex.Row.Item(0), String)
                'MessageBox.Show(customErrorMessage)
                MsgBox(ex.ToString)
                MsgBox(ex.Message)

                'MsgBox(ex.Message)
            End Try
        End If
        ''http://www.dotnet247.com/247reference/msgs/1/7039.aspx
    End Sub

    'Private Sub over50Keys(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If DataGrid1.CurrentCell.ColumnNumber = 0 Then
    '        If CStr(DataGrid1.Item(DataGrid1.CurrentCell.RowNumber, DataGrid1.CurrentCell.ColumnNumber)).Length > 50 Then
    '            e.Handled = True
    '        End If
    '    End If
    'End Sub

End Class
