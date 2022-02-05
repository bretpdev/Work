Imports System.Drawing

Public Class Bins

    Private Bins As ArrayList
    Private BU As String
    Private DSBinsInfo As DataSet
    Private DA As SqlClient.SqlDataAdapter
    Const WidthNeededForABin As Integer = 140
    Const HeightOfBinPanel As Integer = 584

    Public Sub New(ByVal tBU As String, ByVal Frm As frmBins)
        BU = tBU
        DSBinsInfo = New DataSet
        Bins = New ArrayList
        DA = New SqlClient.SqlDataAdapter(String.Format("SELECT * FROM dbo.BUAndBinXRef WHERE BU = '{0}' ORDER BY FunctionKey", BU), SP.UsrInf.Conn)
        DA.Fill(DSBinsInfo, "Bins")
        'Create Bins and add to panel
        Dim DR As DataRow
        Dim XCoor As Integer = 0
        For Each DR In DSBinsInfo.Tables("Bins").Rows
            'bin controls are 1135 pixels in width and they are going to be placed with 5 pixels between them
            Bins.Add(New ABin(DR("BU").ToString, DR("Bin").ToString, DR("FunctionKey").ToString)) 'create new bin
            'add bin to panel 
            CType(Bins(Bins.Count - 1), ABin).Location = New Point(XCoor, 0)
            Frm.pnlBins.Controls.Add(CType(Bins(Bins.Count - 1), ABin))
            CType(Bins(Bins.Count - 1), ABin).SetUpFKeyHandler() 'setup handler for function keys
            XCoor = XCoor + WidthNeededForABin
        Next
        'figure out panel (that holds bins) width and position
        '**the goal here is to center the panel if there are less than 7 bins to display, 
        '**else the panel needs to be big enough to display 7 and allow the user to scroll to 
        '**all other bins not displayed
        Dim DifferenceBetweenFormAndPanelWidth As Integer
        Dim CalLeftOfPanel As Integer
        If Bins.Count <= 7 Then
            Frm.pnlBins.Width = Bins.Count * WidthNeededForABin
        Else
            Frm.pnlBins.Width = 7 * WidthNeededForABin
        End If
        DifferenceBetweenFormAndPanelWidth = Frm.Size.Width - Frm.pnlBins.Width
        CalLeftOfPanel = DifferenceBetweenFormAndPanelWidth \ 2
        Frm.pnlBins.Left = CalLeftOfPanel
        Frm.pnlBins.Height = HeightOfBinPanel
        GetQueueInfo() 'gets system population for bins
    End Sub

    Public Function GetBinsArrayList() As ArrayList
        Return Bins
    End Function

    'hits system and gathers queue info
    Public Sub GetQueueInfo()
        Dim Reader As SqlClient.SqlDataReader
        Dim Comm As New SqlClient.SqlCommand(String.Format("SELECT QDept FROM BUAndQDeptXRef WHERE BU = '{0}'", BU), SP.UsrInf.Conn)
        Dim Row As Integer = 8
        Dim Searcher As Integer
        'get departments from DB
        Comm.Connection.Open()
        Reader = Comm.ExecuteReader()
        While Reader.Read()
            SP.Q.FastPath("LP8YI" + Reader("QDept").ToString + ";;" + SP.UsrInf.Userid)
            'make sure that the queue has something in it
            If SP.Q.Check4Text(1, 60, "QUEUE STATS SELECTION") = False Then
                While SP.Q.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                    Searcher = 0
                    While Searcher < Bins.Count
                        If CType(Bins(Searcher), ABin).IHaveThatQueue(SP.Q.GetText(Row, 11, 9)) Then
                            'if the targeted bin has the Queue then update bin
                            If CInt(SP.Q.GetText(Row, 69, 6)) > 0 Then CType(Bins(Searcher), ABin).UpdateQueueInfo(SP.Q.GetText(Row, 11, 9), CInt(SP.Q.GetText(Row, 69, 6)))
                            Exit While
                        End If
                        Searcher = Searcher + 1
                    End While
                    Row = Row + 1
                    If SP.Q.Check4Text(Row, 11, " ") Then
                        'no queue listed on next line
                        Row = 8
                        SP.Q.Hit("F8")
                    End If
                End While
            End If
        End While
        Comm.Connection.Close()
    End Sub

End Class
