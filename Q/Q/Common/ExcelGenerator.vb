Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Core
Imports System.IO
Imports System.Runtime.InteropServices

Public Class ExcelGenerator
	Implements IDisposable

	Public Enum HCellAlignment
		Left = Excel.XlHAlign.xlHAlignLeft
		Right = Excel.XlHAlign.xlHAlignRight
		Center = Excel.XlHAlign.xlHAlignCenter
	End Enum

	Public Enum VCellAlignment
		Top = Excel.XlVAlign.xlVAlignTop
		Bottom = Excel.XlVAlign.xlVAlignBottom
		Center = Excel.XlVAlign.xlVAlignCenter
	End Enum

	Private _xlApp As Excel.Application
	Private _workbook As Excel.Workbook
	Private _worksheet As Excel.Worksheet
	Private _filePath As String

	Public Sub New()

	End Sub

	Public Sub New(ByVal efs As EnterpriseFileSystem, ByVal fileName As String)
		_filePath = String.Concat(efs.GetPath("FSA_REPORTS"), fileName)
		If File.Exists(_filePath) Then
			_xlApp = New Excel.Application
			_workbook = _xlApp.Workbooks.Open(_filePath)
		Else
			_xlApp = New Excel.Application
			_workbook = _xlApp.Workbooks.Add()

		End If
	End Sub

	''' <summary>
	''' Insert text into the given cell range.  Default: Font Calibri; Size 11; Background color: white; Foreground color: Black; 
	''' </summary>
	''' <param name="range1">Starting cell location</param>
	''' <param name="range2">Ending cell location</param>
	''' <param name="message">String to write into range</param>
	''' <remarks></remarks>
	Public Sub InsertData(ByVal range1 As String, ByVal range2 As String, ByVal message As String)

		_worksheet.Range(range1, range2).Value2 = message

	End Sub

	''' <summary>
	''' Overload with default Background color: white; Foreground color: Black; Horizontal Align: Left; Vert Align: Bottom
	''' </summary>
	''' <param name="range1">Starting cell location</param>
	''' <param name="range2">Ending cell location</param>
	''' <param name="message">String to write into range</param>
	''' <param name="fonts">Use Font class to determine style type size etc..</param>
	''' <param name="textWrap">Bool to textwrap text within the given range</param>
	''' <remarks></remarks>
	Public Sub InsertData(ByVal range1 As String, ByVal range2 As String, ByVal message As String, ByVal fonts As Font, ByVal textWrap As Boolean)

		_worksheet.Range(range1, range2).Font.Bold = fonts.Bold
		_worksheet.Range(range1, range2).Font.Underline = fonts.Underline
		_worksheet.Range(range1, range2).Font.Italic = fonts.Italic
		_worksheet.Range(range1, range2).Font.Name = fonts.Name
		_worksheet.Range(range1, range2).Font.Size = fonts.Size
		_worksheet.Range(range1, range2).Value2 = message

		If textWrap Then
			_worksheet.Range(range1, range2).WrapText = True
		Else
			_worksheet.Columns.AutoFit()
		End If
	End Sub

	''' <summary>
	''' Overload default Background color: white; Foreground color: Black;
	''' </summary>
	''' <param name="range1">Starting cell location</param>
	''' <param name="range2">Ending cell location</param>
	''' <param name="message">String to write into range</param>
	''' <param name="fonts">Use Font class to determine style type size etc..</param>
	''' <param name="textWrap">Bool to textwrap text within the given range</param>
	''' <param name="HCellAlign">Horizontal Alignment</param>
	''' <param name="VCellAlign">Vertical Alignment</param>
	''' <remarks></remarks>
	Public Sub InsertData(ByVal range1 As String, ByVal range2 As String, ByVal message As String, ByVal fonts As Font, ByVal textWrap As Boolean, ByVal HCellAlign As HCellAlignment, ByVal VCellAlign As VCellAlignment)

		_worksheet.Range(range1, range2).Font.Bold = fonts.Bold
		_worksheet.Range(range1, range2).Font.Underline = fonts.Underline
		_worksheet.Range(range1, range2).Font.Italic = fonts.Italic
		_worksheet.Range(range1, range2).Font.Name = fonts.Name
		_worksheet.Range(range1, range2).Font.Size = fonts.Size
		_worksheet.Range(range1, range2).Value2 = message
		_worksheet.Range(range1, range2).HorizontalAlignment = HCellAlign
		_worksheet.Range(range1, range2).VerticalAlignment = VCellAlign
		If textWrap Then
			_worksheet.Range(range1, range2).WrapText = True
		Else
			_worksheet.Columns.AutoFit()
		End If
	End Sub

	''' <summary>
	''' Overload with no defaults
	''' </summary>
	''' <param name="range1">Starting cell location</param>
	''' <param name="range2">Ending cell location</param>
	''' <param name="message">String to write into range</param>
	''' <param name="background">Cell background color</param>
	''' <param name="textColor">Text color</param>
	''' <param name="fonts">Use Font class to determine style type size etc..</param>
	''' <param name="textWrap">Bool to textwrap text within the given range</param>
	''' <param name="HCellAlign"></param>
	''' <param name="VCellAlign"></param>
	''' <remarks></remarks>
	Public Sub InsertData(ByVal range1 As String, ByVal range2 As String, ByVal message As String, ByVal background As Color, ByVal textColor As Color, ByVal fonts As Font, ByVal textWrap As Boolean, ByVal HCellAlign As HCellAlignment, ByVal VCellAlign As VCellAlignment)

		_worksheet.Range(range1, range2).Font.Bold = fonts.Bold
		_worksheet.Range(range1, range2).Font.Underline = fonts.Underline
		_worksheet.Range(range1, range2).Font.Italic = fonts.Italic
		_worksheet.Range(range1, range2).Font.Size = fonts.Size
		_worksheet.Range(range1, range2).Font.Name = fonts.Name
		_worksheet.Range(range1, range2).Interior.Color = ColorTranslator.ToOle(background)
		_worksheet.Range(range1, range2).Font.Color = ColorTranslator.ToOle(textColor)
		_worksheet.Range(range1, range2).Value2 = message
		_worksheet.Range(range1, range2).HorizontalAlignment = HCellAlign
		_worksheet.Range(range1, range2).VerticalAlignment = VCellAlign

		If textWrap Then
			_worksheet.Range(range1, range2).WrapText = True
		Else
			_worksheet.Columns.AutoFit()
		End If
	End Sub

	''' <summary>
	''' Sum a range of cells
	''' </summary>
	''' <param name="rangeToSum1">Starting cell location to sum</param>
	''' <param name="rangeToSum2">Ending cell location to sum</param>
	''' <param name="range1ForSum">Starting location for sum value</param>
	''' <param name="range2ForSum">Ending location for sum value</param>
	''' <param name="fonts">Use Font class to determine style type size etc..</param>
	''' <remarks></remarks>
	Public Sub Sum(ByVal rangeToSum1 As String, ByVal rangeToSum2 As String, ByVal range1ForSum As String, ByVal range2ForSum As String, ByVal fonts As Font)
		_worksheet.Range(range1ForSum, range2ForSum).Font.Bold = fonts.Bold
		_worksheet.Range(range1ForSum, range2ForSum).Font.Underline = fonts.Underline
		_worksheet.Range(range1ForSum, range2ForSum).Font.Italic = fonts.Italic
		_worksheet.Range(range1ForSum, range2ForSum).Font.Size = fonts.Size
		_worksheet.Range(range1ForSum, range2ForSum).Font.Name = fonts.Name
		_worksheet.Range(range1ForSum, range2ForSum).Formula = String.Format("=SUM({0}:{1})", rangeToSum1, rangeToSum2)
	End Sub
	''' <summary>
	''' Sets the active worksheet
	''' </summary>
	''' <param name="sheetNumber">Worksheet to set as the active sheet</param>
	''' <remarks></remarks>
	Public Sub SetActiveWorksheet(ByVal sheetNumber)
		_worksheet = _workbook.Worksheets(sheetNumber)
	End Sub

	''' <summary>
	''' Sets and names the active worksheet
	''' </summary>
	''' <param name="sheetNumber">Worksheet to set as the active sheet</param>
	''' <param name="worksheetName">Name for the active worksheet</param>
	''' <remarks></remarks>
	Public Sub SetActiveWorksheet(ByVal sheetNumber As Integer, ByVal worksheetName As String)
		_worksheet = _workbook.Worksheets(sheetNumber)
		_worksheet.Name = worksheetName
	End Sub
	''' <summary>
	''' Saves your current worksheet.  If the file exsists it will save it, if the file does not exsist it will Save As.The file path was defined in the Constructor
	''' </summary>
	''' <remarks></remarks>
	Public Sub Save()
		If File.Exists(_filePath) Then
			_workbook.Save()
		Else
			_workbook.SaveAs(_filePath)
		End If
	End Sub
	''' <summary>
	''' Merges a range of cells
	''' </summary>
	''' <param name="range1">Starting cell location</param>
	''' <param name="range2">Ending cell location</param>
	''' <remarks></remarks>
	Public Sub MergeCells(ByVal range1 As String, ByVal range2 As String)
		_worksheet.Range(range1, range2).Merge()
	End Sub

	''' <summary>
	''' Adjusts the width of cells
	''' </summary>
	''' <param name="range1">Starting cell location</param>
	''' <param name="range2">Ending cell location</param>
	''' <param name="width">Width of cells</param>
	''' <remarks></remarks>
	Public Sub AdjustWidth(ByVal range1 As String, ByVal range2 As String, ByVal width As Double)
		_worksheet.Range(range1, range2).ColumnWidth = width
	End Sub

	''' <summary>
	''' Adjusts the height of cells
	''' </summary>
	''' <param name="range1">Starting cell location</param>
	''' <param name="range2">Ending cell location</param>
	''' <param name="height">Height of cells</param>
	''' <remarks></remarks>
	Public Sub AdjustHeight(ByVal range1 As String, ByVal range2 As String, ByVal height As Double)
		_worksheet.Range(range1, range2).RowHeight = height
	End Sub

	''' <summary>
	''' Insert an image onto a given location in the active worksheet
	''' </summary>
	'''<param name="efs">Efs object</param>
	''' <param name="fileName">Name of the Image</param>
	''' <param name="left">left position</param>
	''' <param name="top">Top position</param>
	''' <param name="width">Width of image</param>
	''' <param name="height">Height of image</param>
	''' <remarks></remarks>
	Public Sub InsertImage(ByVal efs As EnterpriseFileSystem, ByVal fileName As String, ByVal left As Integer, ByVal top As Integer, ByVal width As Integer, ByVal height As Integer)
        '_worksheet.Shapes.AddPicture(String.Concat(efs.GetPath("FSA_REPORTS_IMAGE"), fileName), Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left, top, width, height)
	End Sub
	''' <summary>
	''' Freeze pane for the given rows
	''' </summary>
	''' <param name="range1">Row number to freeze</param>
	''' <remarks></remarks>
	Public Sub FreezeRowPane(ByVal range1 As Integer)
		_worksheet.Rows(range1).Select()
		_xlApp.ActiveWindow.FreezePanes = True
	End Sub
	''' <summary>
	''' Will get the next cell that is empty 
	''' Note this will only look at the first column
	''' </summary>
	''' <param name="row"> Row to start searching with</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function GetNextRow(ByVal row As Integer) As Integer
		Do While _worksheet.Range(String.Concat("A", row), String.Concat("A", row)).Text <> String.Empty
			row += 1
		Loop
		Return row
	End Function
	''' <summary>
	''' Will get the next cell that is empty based upon the starting row and column
	''' </summary>
	''' <param name="row">Row to start searching with</param>
	''' <param name="column">Column to check if text is entered</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function GetNextRow(ByVal row As Integer, ByVal column As String) As Integer
		Do While _worksheet.Range(String.Concat(column, row), String.Concat(column, row)).Text <> String.Empty
			row += 1
		Loop
		Return row
	End Function
	''' <summary>
	''' returns the row based upon the data to search for. Starting row is  = 1
	''' </summary>
	''' <param name="column">Column to search</param>
	''' <param name="dataToSearchFor">String of text to search for</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function GetRow(ByVal column As String, ByVal dataToSearchFor As String) As Integer
		Dim row As Integer
		row = 1
		Do While _worksheet.Range(String.Concat(column, row), String.Concat(column, row)).Text <> dataToSearchFor
			row += 1
		Loop
		Return row
	End Function
	''' <summary>
	''' Sets the background color for a given range
	''' </summary>
	''' <param name="range1">Starting cell location</param>
	''' <param name="range2">Ending Cell location</param>
	''' <param name="background">Color for background</param>
	''' <remarks></remarks>
	Public Sub SetBackGround(ByVal range1 As String, ByVal range2 As String, ByVal background As Color)
		_worksheet.Range(range1, range2).Interior.Color = ColorTranslator.ToOle(background)
	End Sub
	''' <summary>
	''' Sets a default boarder Default LineStyle: 1; Color: Black; Weight: 2;
	''' </summary>
	''' <param name="range1">Starting cell location</param>
	''' <param name="range2">Ending Cell location</param>
	''' <remarks></remarks>
	Public Sub SetBoarder(ByVal range1 As String, ByVal range2 As String)
		_worksheet.Range(range1, range2).Borders.LineStyle = 1
		_worksheet.Range(range1, range2).Borders.Color = ColorTranslator.ToOle(Color.Black)
		_worksheet.Range(range1, range2).Borders.Weight = 2
	End Sub

	Private disposedValue As Boolean = False ' To detect redundant calls

	' IDisposable
	Protected Overridable Sub Dispose(ByVal disposing As Boolean)
		If Not Me.disposedValue Then
			If disposing Then
				Save()
				_xlApp.Quit()
				Marshal.ReleaseComObject(_workbook)
				Marshal.ReleaseComObject(_xlApp)
			End If
		End If
		Me.disposedValue = True
	End Sub

#Region " IDisposable Support "
	' This code added by Visual Basic to correctly implement the disposable pattern.
	Public Sub Dispose() Implements IDisposable.Dispose
		' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub
#End Region

End Class
