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
    class CornerStoneLoansRehabilitatedReport : ReportsBase
    {
        public CornerStoneLoansRehabilitatedReport(ReportData reports, ErrorReport err)
            : base(err)
        {
            Report = reports;
        }

        public void CreateTheReport()
        {
            List<string> files = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, Report.ReportName).ToList();

            if (!CheckMutlipleFiles(files))
                return;

            using (ExcelGenerator excel = new ExcelGenerator(Report.ReportName + ".xls"))
            {
                excel.SetActiveWorksheet(1, "Sheet1");
                foreach (string file in files)
                {
                    List<Unwc05FileData> fileToProcess = LoadTheFile(file);
                    excel.SetActiveWorksheet(file.Contains("R3") ? 1 : 2, file.Contains("R3") ? "DL" : "FFELP");
                    excel.MergeCells("B2", "G2");
                    excel.InsertData("B2", "B2", "Loans Rehabilitated", new Font("Calibri", 18, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
                    excel.MergeCells("B3", "G3");
                    excel.InsertData("B3", "B3", string.Format("Data As of Month End: {0:MMMM yyyy}", DateTime.Now.AddMonths(-1)), Color.White, Color.Black, new Font("Calibri", 12, FontStyle.Italic), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
                    excel.InsertImage("cornerstone_color_logo copy.jpg", 0, 0, 135, 80);
                    excel.InsertData("A8", "A8", file.Contains("R3") ? "DL" : "FFELP", new Font("Calibri", 9, FontStyle.Bold | FontStyle.Underline), false);

                    int counter = 10;//starting cell everytime the counter is incremented it will move to the next row
                    excel.InsertData(string.Concat("A", counter), string.Concat("A", counter), "Current Status", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                    excel.InsertData(string.Concat("B", counter), string.Concat("B", counter), "Number of Loans", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                    excel.InsertData(string.Concat("C", counter), string.Concat("C", counter), "Total PBO", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                    excel.InsertData(string.Concat("D", counter), string.Concat("D", counter), "Total Interest", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                    excel.InsertData(string.Concat("E", counter), string.Concat("E", counter), "Total", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                    counter++;

                    int sumIndex = counter;//will be used below as the position of where the sum will go
                    foreach (Unwc05FileData data in fileToProcess)
                    {
                        excel.InsertData(string.Concat("A", counter), string.Concat("A", counter), data.CurrentStatus, FontWithNoBold, false);
                        excel.InsertData(string.Concat("B", counter), string.Concat("B", counter), data.NumberOfLoans, FontWithNoBold, false);
                        excel.InsertData(string.Concat("C", counter), string.Concat("C", counter), data.TotalPbo, FontWithNoBold, false);
                        excel.InsertData(string.Concat("D", counter), string.Concat("D", counter), data.TotalInterest, FontWithNoBold, false);
                        excel.InsertData(string.Concat("E", counter), string.Concat("E", counter), data.Total, FontWithNoBold, false);
                        counter++;
                    }

                    excel.InsertData(string.Concat("A", counter), string.Concat("A", counter), "TOTALS", new Font("Calibri", 9, FontStyle.Bold | FontStyle.Underline), false);
                    excel.Sum(string.Concat("B", sumIndex), string.Concat("B", (counter - 1)), string.Concat("B", counter), string.Concat("B", counter), FontWithBold);
                    excel.Sum(string.Concat("C", sumIndex), string.Concat("C", counter - 1), string.Concat("C", counter), string.Concat("C", counter), FontWithBold);
                    excel.Sum(string.Concat("D", sumIndex), string.Concat("D", counter - 1), string.Concat("D", counter), string.Concat("D", counter), FontWithBold);
                    excel.Sum(string.Concat("E", sumIndex), string.Concat("E", counter - 1), string.Concat("E", counter), string.Concat("E", counter), FontWithBold);

                    counter += 3;
                    MergeCellsInsertData(excel, counter, "Data Dictionary", new Font("Calibri", 9, FontStyle.Bold | FontStyle.Underline), false);
                    counter += 2;
                    MergeCellsInsertData(excel, counter, "Program ID", FontWithBold, false);
                    counter++;
                    MergeCellsInsertData(excel, counter, "The loan program code which the rehabilitated loan(s) belong to.", FontWithNoBold, true);
                    counter++;
                    MergeCellsInsertData(excel, counter, "Source", FontWithBold, false);
                    counter++;
                    excel.AdjustHeight(string.Concat("A", counter), string.Concat("A", counter), 27);
                    MergeCellsInsertData(excel, counter, "The source of the rehabilitated loan(s).  The loan(s) may be received via transfer from another servicer or directly from DMCS.", FontWithNoBold, true);
                    counter++;
                    MergeCellsInsertData(excel, counter, "Current Status", FontWithBold, false);
                    counter++;
                    string statusDef = "The status of the rehabilitated loan as of the run date. The status will display as one of the following:  Current Repay (Loan is in repayment and is less than 30 days delinquent); Forbearance (Loan is in a forbearance); Deferment (Loan is in a deferment); Suspense (Loan is in a suspense status due to school, grace, death, disability, or bankruptcy); Delinquent (Loan is in repayment and equal to or greater than 30 days delinquent); Other (Loan does not belong to any of the above categories).";
                    excel.AdjustHeight(string.Concat("A", counter), string.Concat("A", counter), 88);
                    MergeCellsInsertData(excel, counter, statusDef, FontWithNoBold, true);
                    counter++;
                    MergeCellsInsertData(excel, counter, "Number of Loans", FontWithBold, false);
                    counter++;
                    MergeCellsInsertData(excel, counter, "Total number of rehabilitated loans.", FontWithNoBold, true);
                    counter++;
                    MergeCellsInsertData(excel, counter, "Total PBO", FontWithBold, false);
                    counter++;
                    MergeCellsInsertData(excel, counter, "Total current principal balance of rehabilitated loans.", FontWithNoBold, true);
                    counter++;
                    MergeCellsInsertData(excel, counter, "Total Interest", FontWithBold, false);
                    counter++;
                    MergeCellsInsertData(excel, counter, "Total current interest balance of rehabilitated loans.", FontWithNoBold, true);
                    counter++;
                    MergeCellsInsertData(excel, counter, "Total", FontWithBold, false);
                    counter++;
                    MergeCellsInsertData(excel, counter, "Total PBO and interest balances of rehabilitated loans.", FontWithNoBold, true);
                    counter++;

                    excel.AdjustWidth("A1", "E1", 12.6);
                    File.Delete(file);
                }
            }
        }

        private List<Unwc05FileData> LoadTheFile(string fileToProcess)
        {
            List<Unwc05FileData> file = new List<Unwc05FileData>();

            using (StreamReader sr = new StreamReader(fileToProcess))
            {
                sr.ReadLine();//Read in the header.
                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    file.Add(new Unwc05FileData()
                    {
                        CurrentStatus = temp[0],
                        NumberOfLoans = temp[1],
                        TotalPbo = temp[2],
                        TotalInterest = temp[3],
                        Total = temp[4]
                    });
                }
            }
            return file;
        }

        private void MergeCellsInsertData(ExcelGenerator excel, int position, string text, Font fonts, bool textWrap)
        {
            excel.MergeCells(string.Concat("A", position), string.Concat("E", position));
            excel.InsertData(string.Concat("A", position), string.Concat("A", position), text, fonts, textWrap);
        }
    }

    class Unwc05FileData
    {
        public string CurrentStatus { get; set; }
        public string NumberOfLoans { get; set; }
        public string TotalPbo { get; set; }
        public string TotalInterest { get; set; }
        public string Total { get; set; }
    }
}
