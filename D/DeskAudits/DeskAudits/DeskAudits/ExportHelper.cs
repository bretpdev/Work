using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Excel = Microsoft.Office.Interop.Excel;

namespace DeskAudits
{
    public class ExportHelper
    {
        ProcessLogRun LogRun { get; set; }

        public ExportHelper(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        public bool? ExportData(string filePath, DataTable data)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workBook = excelApp.Workbooks.Add(Missing.Value);
            try
            {
                Excel.Worksheet sheet;
                excelApp.Visible = true;
                sheet = (Excel.Worksheet)workBook.Sheets[1];
                WriteAuditRecords(data, sheet, "Audits");
                workBook.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error encountered trying to write out Excel file.";
                LogRun.AddNotification($"{errorMessage}. Exception encountered: {ex.Message}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Warning.Ok(errorMessage);
                return false;
            }
            finally
            {
                //Dispose of Excel app
                workBook.Close(0);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

            return true;
        }

        private void WriteAuditRecords(DataTable data, Excel.Worksheet sheet, string tabName)
        {
            Excel.Range range;
            sheet.Name = tabName;
            int columnCount = 1;
            foreach (DataColumn column in data.Columns)
                sheet.Cells[1, columnCount++] = column.ColumnName;

            sheet.Cells.NumberFormat = "@"; // Set type to text so that the date field doesn't get morphed by Excel
            columnCount = 1;
            int rowCount = 2;//starting at 2 to account for the header
            foreach (DataRow row in data.Rows)
            {
                sheet.Cells[rowCount, columnCount++] = row["Auditor"].ToString();
                sheet.Cells[rowCount, columnCount++] = row["Auditee"].ToString();
                sheet.Cells[rowCount, columnCount++] = row["Result"].ToString();
                sheet.Cells[rowCount, columnCount++] = row["Fail Reason"].ToString();
                DateTime dt = (DateTime)row["Audit Date"];
                sheet.Cells[rowCount, columnCount++] = dt.ToString("MM-dd-yyyy hh:mm:ss tt");

                columnCount = 1;
                rowCount++;
            }

            //AutoFit columns A:E.
            range = sheet.get_Range("A1", "E1");
            range.EntireColumn.AutoFit();
        }
    }
}
