using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Excel = Microsoft.Office.Interop.Excel;

namespace CLMPMTPST
{
    public class ReportGenerator
    {
        ProcessLogRun LogRun { get; set; }
        private static string REPORT_FILE_SAVE_PATH { get; set; }

        public ReportGenerator(ProcessLogRun logRun)
        {
            LogRun = logRun;
            if (DataAccessHelper.TestMode)
                REPORT_FILE_SAVE_PATH = @"X:\PADD\Compass\Payments\Test\ClaimTargetErrors.xls";
            else
                REPORT_FILE_SAVE_PATH = @"X:\PADD\Compass\Payments\ClaimTargetErrors.xls";
        }

        public void Generate(string errorFileSource)
        {
            DataTable data = FileSystemHelper.CreateDataTableFromFile(errorFileSource);
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workBook = excelApp.Workbooks.Add(Missing.Value);
            try
            {
                Excel.Worksheet sheet;
                excelApp.Visible = true;
                sheet = (Excel.Worksheet)workBook.Sheets[1];
                WriteErrors(data, sheet, "Errors");
                workBook.SaveAs(REPORT_FILE_SAVE_PATH);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error encountered trying to write out Excel file of errors.";
                LogRun.AddNotification($"{errorMessage}. Exception encountered: {ex.Message}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Warning.Ok(errorMessage);
                return;
            }
            finally
            {
                //Dispose of Excel app
                workBook.Close(0);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        private void WriteErrors(DataTable data, Excel.Worksheet sheet, string tabName)
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
                sheet.Cells[rowCount, columnCount++] = row["SSN"].ToString().Insert(5, "-").Insert(3, "-"); // Format SSN as ####-##-###
                sheet.Cells[rowCount, columnCount++] = double.Parse(row["Amount"].ToString()).ToString("###,##0.00");
                sheet.Cells[rowCount, columnCount++] = row["Effective Date"].ToString();
                sheet.Cells[rowCount, columnCount++] = row["Loan Sequence"].ToString();
                sheet.Cells[rowCount, columnCount++] = row["Guarantor Code"].ToString();

                columnCount = 1;
                rowCount++;
            }

            //AutoFit columns A:E.
            range = sheet.get_Range("A1", "E1");
            range.EntireColumn.AutoFit();
        }
    }
}
