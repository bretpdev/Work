using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Uheaa.Common.DataAccess;
using Excel = Microsoft.Office.Interop.Excel;

namespace Uheaa.Common.Scripts
{
    public class ExcelGenerator : IDisposable
    {
        private Excel.Application XlApp;
        private Excel.Workbook Workbook;
        private Excel.Workbooks Workbooks;
        private Excel.Worksheet Worksheet;
        private string FilePath;

        public enum HCellAlignment
        {
            Left = Excel.XlHAlign.xlHAlignLeft,
            Right = Excel.XlHAlign.xlHAlignRight,
            Center = Excel.XlHAlign.xlHAlignCenter
        }

        public enum VCellAlignment
        {
            Top = Excel.XlVAlign.xlVAlignTop,
            Bottom = Excel.XlVAlign.xlVAlignBottom,
            Center = Excel.XlVAlign.xlVAlignCenter
        }

        public ExcelGenerator(string filePath)
        {
            FilePath = filePath;
            XlApp = new Excel.Application();
            Workbooks = XlApp.Workbooks;
            XlApp.Visible = true;
            if (File.Exists(FilePath))
                Workbook = Workbooks.Open(FilePath);
            else
                Workbook = Workbooks.Add();

            XlApp.DisplayAlerts = false;
        }
        
        public void ToAccountingFormat(string range1, string range2)
        {
            Worksheet.get_Range(range1, range2).NumberFormat = "#,##0.00_);(#,##0.00)";     
        }

        public void ToMoneyFormat(string range1, string range2)
        {
            Worksheet.get_Range(range1, range2).NumberFormat = "$#,##0.00_);($#,##0.00)";
        }

        /// <summary>
        /// Insert text into the given cell range.  Default: Font Calibri; Size 11; Background color: white; Foreground color: Black; 
        /// </summary>
        /// <param name="range1">Starting cell location</param>
        /// <param name="range2">Ending cell location</param>
        /// <param name="message">String to write into range</param>
        /// <remarks></remarks>
        public void InsertData(string range1, string range2, string message)
        {
            Worksheet.get_Range(range1, range2).NumberFormat = "@";
            Worksheet.Range[range1, range2].Font.Size = 9;
            Worksheet.Range[range1, range2].Value2 = message;
            Worksheet.Columns.AutoFit();
        }

        public void InsertData(string range1, string range2, decimal message)
        {
            Worksheet.get_Range(range1, range2).NumberFormat = "@";
            Worksheet.Range[range1, range2].Font.Size = 9;
            Worksheet.Range[range1, range2].Value2 = message;
            Worksheet.Columns.AutoFit();
        }

        /// <summary>
        /// Overload with default Background color: white; Foreground color: Black; Horizontal Align: Left; Vert Align: Bottom
        /// </summary>
        /// <param name="range1">Starting cell location</param>
        /// <param name="range2">Ending cell location</param>
        /// <param name="message">String to write into range</param>
        /// <param name="fonts">Use Font class to determine style type size etc..</param>
        /// <param name="textWrap">Bool to textwrap text within the given range</param>
        /// <remarks></remarks>
        public void InsertData(string range1, string range2, string message, Font fonts, bool textWrap)
        {
            Worksheet.Range[range1, range2].Font.Bold = fonts.Bold;
            Worksheet.Range[range1, range2].Font.Underline = fonts.Underline;
            Worksheet.Range[range1, range2].Font.Italic = fonts.Italic;
            Worksheet.Range[range1, range2].Font.Name = fonts.Name;
            Worksheet.Range[range1, range2].Font.Size = fonts.Size;
            Worksheet.Range[range1, range2].Value2 = message;

            if (textWrap)
                Worksheet.Range[range1, range2].WrapText = true;
            else
                Worksheet.Columns.AutoFit();
        }

        /// <summary>
        /// Overload with default Background color: white; Foreground color: Black;
        /// </summary>
        /// <param name="range1">Starting cell location</param>
        /// <param name="range2">Ending cell locatio</param>
        /// <param name="message">String to write into range</param>
        /// <param name="fonts">Use Font class to determine style type size etc..</param>
        /// <param name="textWrap">Bool to textwrap text within the given range</param>
        /// <param name="hCellAlign">Sets the horizontal alignment</param>
        /// <param name="vCellAlign">Sets the vertical alignment</param>
        public void InsertData(string range1, string range2, string message, Font fonts, bool textWrap, HCellAlignment hCellAlign, VCellAlignment vCellAlign)
        {
            Worksheet.Range[range1, range2].Font.Bold = fonts.Bold;
            Worksheet.Range[range1, range2].Font.Underline = fonts.Underline;
            Worksheet.Range[range1, range2].Font.Italic = fonts.Italic;
            Worksheet.Range[range1, range2].Font.Name = fonts.Name;
            Worksheet.Range[range1, range2].Font.Size = fonts.Size;
            Worksheet.Range[range1, range2].Value2 = message;
            Worksheet.Range[range1, range2].HorizontalAlignment = hCellAlign;
            Worksheet.Range[range1, range2].VerticalAlignment = vCellAlign;

            if (textWrap)
                Worksheet.Range[range1, range2].WrapText = true;
            else
                Worksheet.Columns.AutoFit();
        }

        /// <summary>
        /// Overload with no defaults
        /// </summary>
        /// <param name="range1">Starting cell location</param>
        /// <param name="range2">Ending cell location</param>
        /// <param name="message">String to write into range</param>
        /// <param name="background">Cell background color</param>
        /// <param name="textColor">Text color</param>
        /// <param name="fonts">Use Font class to determine style type size etc..</param>
        /// <param name="textWrap">Bool to textwrap text within the given range</param>
        /// <param name="hCellAlign">Sets the horizontal alignment</param>
        /// <param name="vCellAlign">Sets the vertical alignment</param>
        /// <remarks></remarks>
        public void InsertData(string range1, string range2, string message, Color background, Color textColor, Font fonts, bool textWrap, HCellAlignment hCellAlign, VCellAlignment vCellAlign)
        {
            Worksheet.Range[range1, range2].Font.Bold = fonts.Bold;
            Worksheet.Range[range1, range2].Font.Underline = fonts.Underline;
            Worksheet.Range[range1, range2].Font.Italic = fonts.Italic;
            Worksheet.Range[range1, range2].Font.Name = fonts.Name;
            Worksheet.Range[range1, range2].Font.Size = fonts.Size;
            Worksheet.Range[range1, range2].Value2 = message;
            Worksheet.Range[range1, range2].Interior.Color = ColorTranslator.ToOle(background);
            Worksheet.Range[range1, range2].Font.Color = ColorTranslator.ToOle(textColor);
            Worksheet.Range[range1, range2].HorizontalAlignment = hCellAlign;
            Worksheet.Range[range1, range2].VerticalAlignment = vCellAlign;

            if (textWrap)
                Worksheet.Range[range1, range2].WrapText = true;
            else
                Worksheet.Columns.AutoFit();
        }

        /// <summary>
        /// Sum a range of cells
        /// </summary>
        /// <param name="rangeToSum1">Starting cell location to sum</param>
        /// <param name="rangeToSum2">Ending cell location to sum</param>
        /// <param name="range1ForSum">Starting location for sum value</param>
        /// <param name="range2ForSum">Ending location for sum value</param>
        /// <param name="fonts">Use Font class to determine style type size etc..</param>
        /// <remarks></remarks>
        public void Sum(string rangeToSum1, string rangeToSum2, string range1ForSum, string range2ForSum, Font fonts)
        {
            Worksheet.Range[range1ForSum, range2ForSum].Font.Bold = fonts.Bold;
            Worksheet.Range[range1ForSum, range2ForSum].Font.Underline = fonts.Underline;
            Worksheet.Range[range1ForSum, range2ForSum].Font.Italic = fonts.Italic;
            Worksheet.Range[range1ForSum, range2ForSum].Font.Size = fonts.Size;
            Worksheet.Range[range1ForSum, range2ForSum].Font.Name = fonts.Name;
            Worksheet.Range[range1ForSum, range2ForSum].Formula = string.Format("=SUM({0}:{1})", rangeToSum1, rangeToSum2);
        }

        /// <summary>
        /// Sum a range of cells
        /// </summary>
        /// <param name="rangeToSum1">Starting cell location to sum</param>
        /// <param name="rangeToSum2">Ending cell location to sum</param>
        /// <param name="range1ForSum">Starting location for sum value</param>
        /// <param name="range2ForSum">Ending location for sum value</param>
        /// <param name="fonts">Use Font class to determine style type size etc..</param>
        /// <param name="hCellAlign">Horizontal Alignment</param>
        /// <param name="vCellAlign"> Vertical Alignment</param>
        /// <remarks></remarks>
        public void Sum(string rangeToSum1, string rangeToSum2, string range1ForSum, string range2ForSum, Font fonts, HCellAlignment hCellAlign, VCellAlignment vCellAlign)
        {
            Worksheet.Range[range1ForSum, range2ForSum].Font.Bold = fonts.Bold;
            Worksheet.Range[range1ForSum, range2ForSum].Font.Underline = fonts.Underline;
            Worksheet.Range[range1ForSum, range2ForSum].Font.Italic = fonts.Italic;
            Worksheet.Range[range1ForSum, range2ForSum].Font.Size = fonts.Size;
            Worksheet.Range[range1ForSum, range2ForSum].Font.Name = fonts.Name;
            Worksheet.Range[range1ForSum, range2ForSum].Formula = string.Format("=SUM({0}:{1})", rangeToSum1, rangeToSum2);
            Worksheet.Range[range1ForSum, range2ForSum].HorizontalAlignment = hCellAlign;
            Worksheet.Range[range1ForSum, range2ForSum].VerticalAlignment = vCellAlign;
        }

        /// <summary>
        /// Sets and names the active worksheet
        /// </summary>
        /// <param name="sheetNumber">Worksheet to set as the active sheet</param>
        /// <param name="worksheetName">Name for the active worksheet</param>
        /// <remarks></remarks>
        public void SetActiveWorksheet(object sheetNumber, string sheetName)
        {
            Worksheet = (Excel.Worksheet)Workbook.Worksheets[sheetNumber];
            Worksheet.Name = sheetName;
        }

        /// <summary>
        /// Saves your current worksheet.  If the file exsists it will save it, if the file does not exsist it will Save As.The file path was defined in the Constructor
        /// </summary>
        /// <remarks></remarks
        public void Save()
        {
            if (File.Exists(FilePath))
                Workbook.Save();
            else
                Workbook.SaveAs(FilePath);
        }

        /// <summary>
        /// Merges a range of cells
        /// </summary>
        /// <param name="range1">Starting cell location</param>
        /// <param name="range2">Ending cell location</param>
        /// <remarks></remarks>
        public void MergeCells(string range1, string range2)
        {
            Worksheet.Range[range1, range2].Merge();
        }

        /// <summary>
        /// Adjusts the width of cells
        /// </summary>
        /// <param name="range1">Starting cell location</param>
        /// <param name="range2">Ending cell location</param>
        /// <param name="width">Width of cells</param>
        /// <remarks></remarks>
        public void AdjustWidth(string range1, string range2, double width)
        {
            Worksheet.Range[range1, range2].ColumnWidth = width;
        }

        /// <summary>
        /// Adjusts the height of cells
        /// </summary>
        /// <param name="range1">Starting cell location</param>
        /// <param name="range2">Ending cell location</param>
        /// <param name="height">Height of cells</param>
        /// <remarks></remarks>
        public void AdjustHeight(string range1, string range2, double height)
        {
            Worksheet.Range[range1, range2].RowHeight = height;
        }

        /// <summary>
        /// Insert an image onto a given location in the active worksheet
        /// </summary>
        ///<param name="efs">Efs object</param>
        /// <param name="fileName">Name of the Image</param>
        /// <param name="left">left position</param>
        /// <param name="top">Top position</param>
        /// <param name="width">Width of image</param>
        /// <param name="height">Height of image</param>
        /// <remarks></remarks>
        public void InsertImage(string fileName, int left, int top, int width, int height)
        {
            //Worksheet.Shapes.AddPicture(string.Concat(EnterpriseFileSystem.GetPath("FSA_REPORTS_IMAGE"), fileName), Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left, top, width, height);
        }

        /// <summary>
        /// Freeze pane for the given rows
        /// </summary>
        /// <param name="range1">Row number to freeze</param>
        /// <remarks></remarks>
        public void FreezeRowPane(string range1)
        {
            Excel.Range range = (Excel.Range)Worksheet.Rows[range1];
            range.Select();
            XlApp.ActiveWindow.FreezePanes = true;
        }

        /// <summary>
        /// Will get the next cell that is empty based upon the starting row and column
        /// </summary>
        /// <param name="row">Row to start searching with</param>
        /// <param name="column">Column to check if text is entered</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int GetNextRow(int row, string column)
        {
            while (!Worksheet.Range[string.Concat(column, row), string.Concat(column, row)].Text.ToString().IsNullOrEmpty())
                row += 1;

            return row;
        }

        /// <summary>
        /// returns the row based upon the data to search for. Starting row is  = 1
        /// </summary>
        /// <param name="column">Column to search</param>
        /// <param name="dataToSearchFor">String of text to search for</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int GetRow(string column, string dataToSearchFor)
        {
            int row = 1;
            while (!Worksheet.Range[string.Concat(column, row), string.Concat(column, row)].Text.ToString().Contains(dataToSearchFor))
                row += 1;
            return row;
        }

        /// <summary>
        /// Sets the background color for a given range
        /// </summary>
        /// <param name="range1">Starting cell location</param>
        /// <param name="range2">Ending Cell location</param>
        /// <param name="background">Color for background</param>
        /// <remarks></remarks>
        public void SetBackground(string range1, string range2, Color background)
        {
            Worksheet.Range[range1, range2].Interior.Color = ColorTranslator.ToOle(background);
        }

        /// <summary>
        /// Sets a default boarder Default LineStyle: 1; Color: Black; Weight: 2;
        /// </summary>
        /// <param name="range1">Starting cell location</param>
        /// <param name="range2">Ending Cell location</param>
        /// <remarks></remarks>
        public void SetBorder(string range1, string range2)
        {
            Worksheet.Range[range1, range2].Borders.LineStyle = 1;
            Worksheet.Range[range1, range2].Borders.Color = ColorTranslator.ToOle(Color.Black);
            Worksheet.Range[range1, range2].Borders.Weight = 2;
        }

        /// <summary>
        /// Sets a default boarder Default LineStyle: 1; Color: Black; 
        /// </summary>
        /// <param name="range1">Starting cell location</param>
        /// <param name="range2">Ending Cell location</param>
        /// <param name="weight">Weight of the boarder</param>
        /// <remarks></remarks>
        public void SetBorder(string range1, string range2, int weight)
        {
            Worksheet.Range[range1, range2].Borders.LineStyle = 1;
            Worksheet.Range[range1, range2].Borders.Color = ColorTranslator.ToOle(Color.Black);
            Worksheet.Range[range1, range2].Borders.Weight = weight;
        }

        private bool disposedValue = false; //To detect redundant calls

        //IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    Save();
                    Workbook.Close(false, null, null);
                    Workbooks.Close();
                    
                    XlApp.Application.Quit();
                    XlApp.Quit();

                    Marshal.ReleaseComObject(XlApp);
                    Marshal.ReleaseComObject(Workbooks);
                    Marshal.ReleaseComObject(Workbook);
                    //Marshal.ReleaseComObject(Worksheet);
                    
                }
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            //Do not change this code, it is needed to implement IDisposable.
            //Put amy cleanup code in Dispose(bool disposing) method above/
            Dispose(true);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }
}
