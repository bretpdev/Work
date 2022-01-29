Imports RegentsScholarshipBackEnd
Imports System.IO

Public Class frmBaseCommunications
    Private _user As User
    Private _entityType As String
    'Specify a directory that the file picker should start in.
    Const INITIAL_DIRECTORY As String = "\\AD4\Restricted\Regents Scholarships\Scan\"

    Private _entityID As String
    Public Property EntityID() As String
        Get
            Return _entityID
        End Get
        Set(ByVal value As String)
            _entityID = value
            CommunicationPrinter.Entity = value
        End Set
    End Property

    Public Property EntityName() As String
        Get
            Return CommunicationPrinter.EntityName
        End Get
        Set(ByVal value As String)
            CommunicationPrinter.EntityName = value
        End Set
    End Property

    Public Sub New(ByVal user As User, ByVal entityType As String)
        InitializeComponent()
        _user = user
        _entityType = entityType
        CommunicationPrinter.EntityType = entityType
        CommunicationPrinter.SortColumnName = "TimeStamp"
        CommunicationPrinter.SortAscending = True
        _entityID = ""
        cmbCommunicationType.DataSource = Lookups.CommunicationTypes.ToList()
        cmbCommunicationSource.DataSource = Lookups.CommunicationSources.ToList()
        txtCommunicationDateTime.Text = Date.Now
        txtCommunicationUserId.Text = _user.Id
        EntityName = ""
    End Sub

    ''' <summary>
    ''' Default constructor, needed by the form designer. (DO NOT USE)
    ''' </summary>
    Public Sub New()
        InitializeComponent()
    End Sub

    Protected Sub btnCommunicationClearFields_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommunicationClearFields.Click
        txtCommunicationComments.Text = ""
        txtCommunicationDateTime.Text = Date.Now
        cmbCommunicationSource.Text = ""
        cmbCommunicationType.Text = ""
        txtCommunicationSubject.Text = ""
        btnCommunicationSave.Enabled = True
    End Sub

    Public Sub LoadComments()
        Dim entityId As String = _entityType
        If (_entityType <> Constants.CommunicationEntityType.MISC) Then entityId = _entityID
        Dim commList As List(Of Communication) = DataAccess.GetCommunications(entityId, _entityType).ToList()
        CommunicationDataGridView.DataSource = commList
        CommunicationDataGridView.Refresh()

        If commList.Count < 1 Then
            MessageBox.Show("There are no messages.", "No messages", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Protected Sub GiveAccess()
        Select Case _user.AccessLevel
            Case Constants.AccessLevel.READ_ONLY, Constants.AccessLevel.APPLICATION_REVIEW
                grpCommunicationRecord.Enabled = True
            Case Constants.AccessLevel.PA, Constants.AccessLevel.DCR, Constants.AccessLevel.BATCH_PROCESSING
                grpCommunicationRecord.Enabled = False
        End Select
    End Sub

    Protected Sub btnCommunicationSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommunicationSave.Click
        If _entityID.Length = 0 Then
            Dim message As String = String.Format("You must select a {0} before continuing.  Please try again.", _entityType.ToLower)
            MessageBox.Show(message, "Option Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim comm As New Communication()
        comm.EntityID = _entityID
        comm.EntityType = _entityType
        comm.Source = cmbCommunicationSource.Text
        comm.Subject = txtCommunicationSubject.Text
        comm.Text = txtCommunicationComments.Text
        comm.TimeStamp = DateTime.Now
        comm.Type = cmbCommunicationType.Text
        comm.UserId = _user.Id
        comm.Is411 = False
        DataAccess.SetCommunication(comm)

        MessageBox.Show("New comment added", "New Comment Added", MessageBoxButtons.OK)
        LoadComments()
        txtCommunicationComments.Text = ""
        txtCommunicationSubject.Text = ""
    End Sub

    Protected Sub CommunicationDataGridView_CellContentDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles CommunicationDataGridView.CellContentDoubleClick
        If e.RowIndex = -1 Then Return
        txtCommunicationUserId.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("UserId").Value
        txtCommunicationDateTime.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("clmTimeStamp").Value
        cmbCommunicationType.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("clmType").Value
        cmbCommunicationSource.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("clmSource").Value
        txtCommunicationSubject.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("clmSubject").Value
        txtCommunicationComments.Text = CommunicationDataGridView.Rows(e.RowIndex).Cells("clmText").Value
        btnCommunicationSave.Enabled = False
    End Sub

    Protected Sub CommunicationDataGridView_ColumnHeaderMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles CommunicationDataGridView.ColumnHeaderMouseClick
        'Declare some static booleans to track the sort order for each column.
        Static userIDIsAscending As Boolean = False
        Static studentIDIsAscending As Boolean = False
        Static timeStampIsAscending As Boolean = False
        Static typeIsAscending As Boolean = False
        Static sourceIsAscending As Boolean = False
        Static subjectIsAscending As Boolean = False
        Static textIsAscending As Boolean = False

        'Based on the clicked column, set all the static booleans and call SortedSearch.
        Select Case e.ColumnIndex
            Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("UserID"))
                userIDIsAscending = Not userIDIsAscending
                studentIDIsAscending = False
                timeStampIsAscending = False
                typeIsAscending = False
                sourceIsAscending = False
                subjectIsAscending = False
                CommSortedSearch("UserId", userIDIsAscending)
                CommunicationPrinter.SortColumnName = "UserId"
                CommunicationPrinter.SortAscending = userIDIsAscending
            Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("clmTimeStamp"))
                userIDIsAscending = False
                studentIDIsAscending = False
                timeStampIsAscending = Not timeStampIsAscending
                typeIsAscending = False
                sourceIsAscending = False
                subjectIsAscending = False
                CommSortedSearch("TimeStamp", timeStampIsAscending)
                CommunicationPrinter.SortColumnName = "TimeStamp"
                CommunicationPrinter.SortAscending = timeStampIsAscending
            Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("clmType"))
                userIDIsAscending = False
                studentIDIsAscending = False
                timeStampIsAscending = False
                typeIsAscending = Not typeIsAscending
                sourceIsAscending = False
                subjectIsAscending = False
                CommSortedSearch("Type", typeIsAscending)
                CommunicationPrinter.SortColumnName = "Type"
                CommunicationPrinter.SortAscending = typeIsAscending
            Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("clmSource"))
                userIDIsAscending = False
                studentIDIsAscending = False
                timeStampIsAscending = False
                typeIsAscending = False
                sourceIsAscending = Not sourceIsAscending
                subjectIsAscending = False
                CommSortedSearch("Source", sourceIsAscending)
                CommunicationPrinter.SortColumnName = "Source"
                CommunicationPrinter.SortAscending = sourceIsAscending
            Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("clmSubject"))
                userIDIsAscending = False
                studentIDIsAscending = False
                timeStampIsAscending = False
                typeIsAscending = False
                sourceIsAscending = False
                subjectIsAscending = Not subjectIsAscending
                CommSortedSearch("Subject", subjectIsAscending)
                CommunicationPrinter.SortColumnName = "Subject"
                CommunicationPrinter.SortAscending = subjectIsAscending
            Case CommunicationDataGridView.Columns.IndexOf(CommunicationDataGridView.Columns("clmText"))
                userIDIsAscending = False
                studentIDIsAscending = False
                timeStampIsAscending = False
                typeIsAscending = False
                sourceIsAscending = False
                subjectIsAscending = False
                textIsAscending = Not textIsAscending
                CommSortedSearch("Comments", textIsAscending)
                CommunicationPrinter.SortColumnName = "Comments"
                CommunicationPrinter.SortAscending = textIsAscending
        End Select
    End Sub

    Protected Sub CommSortedSearch(ByVal sortColumnName As String, ByVal sortAscending As Boolean)
        Dim entityId As String = _entityType
        If (_entityType <> Constants.CommunicationEntityType.MISC) Then entityId = _entityID
        Dim commSearch As IEnumerable(Of Communication) = DataAccess.GetCommunications(entityId, _entityType)

        Select Case sortColumnName
            Case "UserID"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.UserId)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.UserId)
                End If
            Case "StudentID"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.EntityID)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.EntityID)
                End If
            Case "TimeStamp"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.TimeStamp)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.TimeStamp)
                End If
            Case "Type"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.Type)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.Type)
                End If
            Case "Source"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.Source)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.Source)
                End If
            Case "Subject"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.Subject)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.Subject)
                End If
            Case "Comments"
                If sortAscending Then
                    commSearch = commSearch.OrderBy(Function(p) p.Text)
                Else
                    commSearch = commSearch.OrderByDescending(Function(p) p.Text)
                End If
        End Select

        Cursor = Cursors.Default

        CommunicationDataGridView.DataSource = commSearch.ToList()
        CommunicationDataGridView.Refresh()
    End Sub

    Protected Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnCommuncationsLinkDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommuncationsLinkDoc.Click
        If _entityID.Length = 0 Then
            Dim message As String = String.Format("You must select a {0} before continuing.  Please try again.", _entityType.ToLower)
            MessageBox.Show(message, "Option Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim directoryEntity As String = String.Format("{0}_{1}\", _entityType, EntityID)
        'Compose the full path.
        Dim fullPath As String = Constants.NON_STUDENT_DOCUMENT_ROOT + directoryEntity
        'Make sure the full path exists.
        If Not Directory.Exists(fullPath) Then
            Try
                Directory.CreateDirectory(fullPath)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Filesystem error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

        'Pop up a file dialog so the user can select the file(s) to link.
        Dim linkDialog As New OpenFileDialog()
        linkDialog.InitialDirectory = INITIAL_DIRECTORY
        linkDialog.Multiselect = True   'Explicitly support multiple files.
        linkDialog.ShowDialog()

        'Check that the user selected at least one file to link.
        If linkDialog.FileNames.Count() = 0 Then Return

        'Move all selected files to the network directory.
        For Each fileName As String In linkDialog.FileNames
            Try
                Dim saveName As String = fileName.Substring(fileName.LastIndexOf("\") + 1)
                File.Move(fileName, fullPath + saveName)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error linking document", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
    End Sub

    Private Sub btnCommuncationsViewDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommuncationsViewDoc.Click
        If _entityID.Length = 0 Then
            Dim message As String = String.Format("You must select a {0} before continuing.  Please try again.", _entityType.ToLower)
            MessageBox.Show(message, "Option Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim directoryEntity As String = String.Format("{0}_{1}\", _entityType, EntityID)
        'Compose the full path.
        Dim fullPath As String = Constants.NON_STUDENT_DOCUMENT_ROOT + directoryEntity

        'See if there are any documents.
        If Not Directory.Exists(fullPath) OrElse Directory.GetFiles(fullPath).Count() = 0 Then
            Dim message As String = "No linked files found."
            MessageBox.Show(message, "No documents found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        'Show a file dialog so the user can select a file.
        Dim fetchDialog As New OpenFileDialog()
        fetchDialog.Multiselect = True  'Explicitly support multiple files.
        fetchDialog.InitialDirectory = fullPath
        fetchDialog.ShowDialog()

        'Check that the user selected at least one file.
        If fetchDialog.FileNames.Count() = 0 Then Return

        'Trust Windows to find the correct program to open each file.
        For Each fileName As String In fetchDialog.FileNames
            Try
                Process.Start(fileName)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error opening a linked document", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
    End Sub
End Class