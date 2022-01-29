using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace CFSARPTFED
{
    class CornerStoneDischargeReports : ReportsBase
    {
        public CornerStoneDischargeReports(ReportData reports, ErrorReport err)
            : base(err)
        {
            Report = reports;
        }

        public void CreateTheReport()
        {
            string fileToProcess = GetTheSasFile();
            if (fileToProcess.IsNullOrEmpty())
                return;

            List<DischargeSasData> fileData = ReadTheFile(fileToProcess);
            using (ExcelGenerator excel = new ExcelGenerator(Report.ReportName + ".xls"))
            {
                SetUpTabAndStaticInfo("DL", Report.ReportName, excel);
                EnterMonthlyData(excel, fileData.Where(p => p.DlIndicator == true).ToList());
                EnterDataDictionary(excel);
                excel.AdjustWidth("A1", "A1", 24.5);
                excel.AdjustWidth("B1", "N1", 5);
                SetUpTabAndStaticInfo("FFEL", Report.ReportName, excel);
                EnterMonthlyData(excel, fileData.Where(p => p.DlIndicator == false).ToList());
                EnterDataDictionary(excel);
                excel.AdjustWidth("A1", "A1", 24.5);
                excel.AdjustWidth("B1", "N1", 5);
            }

            File.Delete(fileToProcess);
        }

        private void SetUpTabAndStaticInfo(string type, string heading, ExcelGenerator excel)
        {
            excel.SetActiveWorksheet(type.Contains("DL") ? 1 : 2, type);
            excel.MergeCells("B2", "N2");
            excel.InsertData("B2", "B2", heading, new Font("Calibri", 18, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
            excel.MergeCells("B3", "N3");
            excel.InsertData("B3", "B3", string.Format("Data As of Month End: {0:MMMM yyyy}", DateTime.Now.AddMonths(-1)), Color.White, Color.Black, new Font("Calibri", 12, FontStyle.Italic), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertImage("cornerstone_color_logo copy.jpg", 0, 0, 135, 80);
            excel.SetBackground("A8", "A8", Purple);
            excel.MergeCells("B8", "N8");
            excel.InsertData("B8", "B8", "Received by Month", Purple, Color.White, new Font("Calibri", 9, FontStyle.Italic), false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("A9", "A9", "Discharge Type", Purple, Color.White, new Font("Calibri", 9, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("B9", "B9", "Oct", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("C9", "C9", "Nov", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("D9", "D9", "Dec", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("E9", "E9", "Jan", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("F9", "F9", "Feb", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("G9", "G9", "Mar", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("H9", "H9", "Apr", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("I9", "I9", "May", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("J9", "J9", "Jun", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("K9", "K9", "July", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("L9", "L9", "Aug", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("M9", "M9", "Sept", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("N9", "N9", "YTD", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
        }

        private void EnterMonthlyData(ExcelGenerator excel, List<DischargeSasData> fileData)
        {
            foreach (DischargeSasData data in fileData)
            {
                if (data.MonthlyData[0].Contains("BKP"))
                {
                    EnterData(12, "Bankruptcy", data, excel);
                }
                else if (data.MonthlyData[0].Contains("CSH"))
                {
                    EnterData(13, "Closed School", data, excel);
                }
                else if (data.MonthlyData[0].Contains("DTH"))
                {
                    EnterData(10, "Death", data, excel);
                }
                else if (data.MonthlyData[0].Contains("FCR"))
                {
                    EnterData(14, "False Cert", data, excel);
                }
                else if (data.MonthlyData[0].Contains("PSF"))
                {
                    EnterData(17, "Public Service Loan Forgiveness", data, excel);
                }
                else if (data.MonthlyData[0].Contains("S11"))
                {
                    EnterData(18, "Sept 11 Survivors", data, excel);
                }
                else if (data.MonthlyData[0].Contains("TLF"))
                {
                    EnterData(16, "Teacher Loan Forgiveness", data, excel);
                }
                else if (data.MonthlyData[0].Contains("TPD"))
                {
                    EnterData(11, "TPD", data, excel);
                }
                else
                {
                    EnterData(15, "Unpaid Refund", data, excel);
                }
            }
        }

        private List<DischargeSasData> ReadTheFile(string file)
        {
            List<DischargeSasData> data = new List<DischargeSasData>();
            using (StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine();//Read in the header row.
                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    data.Add(new DischargeSasData()
                    {
                        DlIndicator = temp[14].Contains("DL"),
                        MonthlyData = PullOutData(temp)
                    });
                }
            }

            return data;
        }

        private List<string> PullOutData(List<string> temp)
        {
            temp.RemoveAt(14);
            return temp;
        }

        private void EnterData(int row, string type, DischargeSasData data, ExcelGenerator excel)
        {
            excel.InsertData(string.Concat("A", row), string.Concat("A", row), type, FontWithNoBold, false);
            excel.InsertData(string.Concat("B", row), string.Concat("B", row), data.MonthlyData[10], FontWithNoBold, false);
            excel.InsertData(string.Concat("C", row), string.Concat("C", row), data.MonthlyData[11], FontWithNoBold, false);
            excel.InsertData(string.Concat("D", row), string.Concat("D", row), data.MonthlyData[12], FontWithNoBold, false);
            excel.InsertData(string.Concat("E", row), string.Concat("E", row), data.MonthlyData[1], FontWithNoBold, false);
            excel.InsertData(string.Concat("F", row), string.Concat("F", row), data.MonthlyData[2], FontWithNoBold, false);
            excel.InsertData(string.Concat("G", row), string.Concat("G", row), data.MonthlyData[3], FontWithNoBold, false);
            excel.InsertData(string.Concat("H", row), string.Concat("H", row), data.MonthlyData[4], FontWithNoBold, false);
            excel.InsertData(string.Concat("I", row), string.Concat("I", row), data.MonthlyData[5], FontWithNoBold, false);
            excel.InsertData(string.Concat("J", row), string.Concat("J", row), data.MonthlyData[6], FontWithNoBold, false);
            excel.InsertData(string.Concat("K", row), string.Concat("K", row), data.MonthlyData[7], FontWithNoBold, false);
            excel.InsertData(string.Concat("L", row), string.Concat("L", row), data.MonthlyData[8], FontWithNoBold, false);
            excel.InsertData(string.Concat("M", row), string.Concat("M", row), data.MonthlyData[9], FontWithNoBold, false);
            excel.InsertData(string.Concat("N", row), string.Concat("N", row), data.MonthlyData[13], FontWithNoBold, false);
        }

        private void EnterDataDictionary(ExcelGenerator excel)
        {
            Font bold = new Font("Calibri", 9, FontStyle.Bold);
            excel.MergeCells("A21", "F21");
            excel.InsertData("A21", "F21", "Data Dictionary", new Font("Calibri", 10, FontStyle.Underline | FontStyle.Bold), false);
            excel.MergeCells("A22", "F22");
            excel.InsertData("A22", "F22", "Discharge Type", bold, false);
            excel.MergeCells("A23", "F23");
            excel.InsertData("A23", "F23", "The types of discharge application received.");
            excel.MergeCells("A24", "F24");
            excel.InsertData("A24", "F24", "Received by Month", bold, false);
            excel.MergeCells("A25", "F25");
            excel.InsertData("A25", "F25", "The month the discharge application was received.");
            excel.MergeCells("A26", "F26");
            excel.InsertData("A26", "F26", "YTD", bold, false);
            excel.MergeCells("A27", "F27");
            excel.InsertData("A27", "F27", "Year to date total of discharge applications received.");
            excel.MergeCells("A28", "F28");
            excel.InsertData("A28", "F28", "Totals:", bold, false);
            excel.MergeCells("A29", "F29");
            excel.InsertData("A29", "F29", "Total number of discharge types received by month.");
        }
    }

    class DischargeSasData
    {
        public List<string> MonthlyData { get; set; }
        public bool DlIndicator { get; set; }
    }
}
