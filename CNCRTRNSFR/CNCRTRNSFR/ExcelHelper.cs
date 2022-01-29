using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Uheaa.Common.ProcessLogger;

namespace CNCRTRNSFR
{
    public class ExcelHelper
    {
        public string SaveAs { get; set; }
        public Application xlApp { get; set; }
        public Workbook xlWorkBook { get; set; }
        public Worksheet xlWorkSheet { get; set; }
        public object MisValue = System.Reflection.Missing.Value;
        public ProcessLogRun LogRun { get; set; }

        public ExcelHelper(string file, ProcessLogRun logRun)
        {
            xlApp = new Application();
            xlWorkBook = xlApp.Workbooks.Add(MisValue);
            xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Range cells = xlWorkBook.Worksheets[1].Cells;
            cells.NumberFormat = "@";
            SaveAs = file;
            LogRun = logRun;
        }

        /// <summary>
        /// Adds the header to the new file
        /// </summary>
        public void AddHeader()
        {
            xlWorkSheet.Cells[1, 1] = "Counter";
            xlWorkSheet.Cells[1, 2] = "SSN";
            xlWorkSheet.Cells[1, 3] = "Award ID";
            xlWorkSheet.Cells[1, 4] = "Deferment Begin Date";
            xlWorkSheet.Cells[1, 5] = "Deferment End Date";
            xlWorkSheet.Cells[1, 6] = "Deferment Month Count";
            xlWorkSheet.Cells[1, 7] = "Forbearance Begin Date";
            xlWorkSheet.Cells[1, 8] = "Forbearance End Date";
            xlWorkSheet.Cells[1, 9] = "Forbearance Month Count";
        }

        public void AddRecord(string data, int cell1, int cell2)
        {
            xlWorkSheet.Cells[cell1, cell2] = data;
        }

        public void SaveAndQuit()
        {
            xlWorkSheet.Columns.AutoFit();
            try
            {
                xlWorkBook.SaveAs(SaveAs, XlFileFormat.xlOpenXMLWorkbook, MisValue, MisValue, MisValue, MisValue, XlSaveAsAccessMode.xlExclusive, MisValue, MisValue, MisValue, MisValue, MisValue);
            }
            catch (Exception ex)
            {
                string message = $"There was an error saving the document to {SaveAs}. Error Message: {ex.ToString()}";
                Console.WriteLine(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
            xlWorkBook.Close(true, MisValue, MisValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
        }
    }
}