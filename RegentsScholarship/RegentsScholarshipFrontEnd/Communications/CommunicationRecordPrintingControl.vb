Imports RegentsScholarshipBackEnd

Public Class CommunicationRecordPrintingControl

    Private _theStartOfRegentsApp As New Date(2009, 1, 1)

    Private _entity As String = ""
    Public Property Entity() As String
        Get
            Return _entity
        End Get
        Set(ByVal value As String)
            _entity = value
        End Set
    End Property

    Private _entityType As String
    Public Property EntityType() As String
        Get
            Return _entityType
        End Get
        Set(ByVal value As String)
            _entityType = value

        End Set
    End Property

    Private _entityName As String
    Public Property EntityName() As String
        Get
            Return _entityName
        End Get
        Set(ByVal value As String)
            _entityName = value
        End Set
    End Property

    Private _sortAscending As Boolean
    Public Property SortAscending() As Boolean
        Get
            Return _sortAscending
        End Get
        Set(ByVal value As Boolean)
            _sortAscending = value
        End Set
    End Property

    Private _sortColumnName As String
    Public Property SortColumnName() As String
        Get
            Return _sortColumnName
        End Get
        Set(ByVal value As String)
            _sortColumnName = value
        End Set
    End Property

    Private Sub radCommunicationPrintDateRange_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radCommunicationPrintDateRange.CheckedChanged
        grpDateRange.Enabled = radCommunicationPrintDateRange.Checked
    End Sub

    Private Sub btnCommunicationPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommunicationPrint.Click
        If SortColumnName.Length = 0 Then
            MessageBox.Show("The sort column has not been set for the printed report. Please contact Systems Support for assistance.", "Sort Column Not Set", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If Entity.Length = 0 Then
            MessageBox.Show(String.Format("You must select a {0} to report on first.", EntityType))
            Return
        End If
        If radCommunicationPrintAll.Checked Then
            Reports.CommunicationRecords(Entity, EntityType, _theStartOfRegentsApp.ToString("MM/dd/yyyy"), DateAndTime.Today.AddDays(1).ToString("MM/dd/yyyy"), EntityName, SortColumnName, SortAscending)
        Else
            If dtpFrom.Value Is Nothing OrElse dtpTo.Value Is Nothing Then
                MessageBox.Show("You must provide both a From and To date for the date range functionality.  Please try again.", "From and To", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            If dtpFrom.Value > dtpTo.Value Then
                MessageBox.Show("The From date must be earlier than the To date.  Please try again.", "From must be earlier than To", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Reports.CommunicationRecords(Entity, EntityType, dtpFrom.Text, dtpTo.Text, EntityName, SortColumnName, SortAscending)
        End If
        MessageBox.Show("Please retrieve your document from the printer.", "Document Printed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class
