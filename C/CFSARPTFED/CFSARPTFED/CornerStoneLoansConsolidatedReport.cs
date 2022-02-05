using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace CFSARPTFED
{
    class CornerStoneLoansConsolidatedReport : ReportsBase
    {
        public CornerStoneLoansConsolidatedReport(ReportData reports, ErrorReport err)
            : base(err)
        {
            Report = reports;
        }

        public void CreateTheReport()
        {
            string fileToProcess = GetTheSasFile();
            if (fileToProcess.IsNullOrEmpty())
                return;

            List<Unwc11FileData> fileData = ReadTheFile(fileToProcess);
            using (ExcelGenerator excel = new ExcelGenerator(Report.ReportName + ".xls"))
            {
                excel.SetActiveWorksheet(1, "Sheet 1");

                excel.MergeCells("B2", "F2");
                excel.InsertData("B2", "C2", "CornerStone Loans Consolidated", new Font("Calibri", 18, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
                excel.MergeCells("B3", "F3");
                excel.InsertData("B3", "C3", string.Format("Month Ending: {0:MMMM yyyy}", DateTime.Now.AddMonths(-1)), Color.White, Color.Black, new Font("Calibri", 12, FontStyle.Italic), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertImage("cornerstone_color_logo copy.jpg", 0, 0, 135, 80);
                excel.InsertData("A8", "A8", "Date Loan Added", Purple, Color.White, FontWithBold, true, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertData("B8", "B8", "Source/ Prev Servicer Code", Purple, Color.White, FontWithBold, true, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertData("C8", "C8", "Loan Prgrm", Purple, Color.White, FontWithBold, true, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertData("D8", "D8", "Deal ID", Purple, Color.White, FontWithBold, true, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertData("E8", "E8", "Total Borrowers", Purple, Color.White, FontWithBold, true, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertData("F8", "F8", "Total Loans", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertData("G8", "G8", "Total Principal", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertData("H8", "H8", "Total Interest", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertData("I8", "I8", "Total Transferred", Purple, Color.White, FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);

                int row = 9;
                foreach (Unwc11FileData data in fileData)
                {
                    excel.InsertData("A" + row, "A" + row, data.DateLoanAdded);
                    excel.InsertData("B" + row, "B" + row, data.Source);
                    excel.InsertData("C" + row, "C" + row, data.LoanProgram);
                    excel.InsertData("D" + row, "D" + row, data.DealId);
                    excel.InsertData("E" + row, "E" + row, data.TotalBorrowers);
                    excel.InsertData("F" + row, "F" + row, data.TotalLoans);
                    excel.InsertData("G" + row, "G" + row, data.TotalPrincipal);
                    excel.InsertData("H" + row, "H" + row, data.TotalInterest);
                    excel.InsertData("I" + row, "I" + row, data.TotalTransferred);

                    if (data.DateLoanAdded.IsNullOrEmpty())
                        excel.InsertData("C" + row, "C" + row, "Totals:");
                    row++;
                }

                EnterDataDictionary(excel, ++row);

                excel.AdjustWidth("A1", "A1", 11.5);
                excel.AdjustWidth("B1", "B1", 15.71);
                excel.AdjustWidth("C1", "D1", 15.57);
                excel.AdjustWidth("E1", "F1", 11.57);
                excel.AdjustWidth("G1", "I1", 14.00);
            }

            File.Delete(fileToProcess);
        }

        private void EnterDataDictionary(ExcelGenerator excel, int row)
        {
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Data Dictionary", new Font("Calibri", 9, FontStyle.Bold | FontStyle.Underline), true);
            row += 2;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Date Loan Added", FontWithBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Date the loan/s were added to the CornerStone portfolio", FontWithNoBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Source/Prev Servicer Code", FontWithBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Location of the loan before being added to the CornerStone portfolio", FontWithNoBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Loan Program", FontWithBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Loan Program Code the added loan/s falls under", FontWithNoBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Deal ID", FontWithBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Deal ID the loan was transferred with from the previous servicer", FontWithNoBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Total Borrowers", FontWithBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Total number of unique borrowers added per date", FontWithNoBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Total Loans", FontWithBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Total number of loans added per date", FontWithNoBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Total Principal", FontWithBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Total principal balance of added loans per date", FontWithNoBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Total Interest", FontWithBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Total interest balance of added loans per date", FontWithNoBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Total Transferred", FontWithBold, true);
            row++;
            excel.MergeCells("A" + row, "F" + row);
            excel.InsertData("A" + row, "A" + row, "Total combined principal and interest balance of added loans per date", FontWithNoBold, true);
            row++;
        }

        private List<Unwc11FileData> ReadTheFile(string file)
        {
            List<Unwc11FileData> fileData = new List<Unwc11FileData>();
            using (StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine();//Read in the header row
                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    fileData.Add(new Unwc11FileData()
                    {
                        DateLoanAdded = temp[0],
                        Source = temp[1],
                        LoanProgram = temp[2],
                        DealId = temp[3],
                        TotalBorrowers = temp[4],
                        TotalLoans = temp[5],
                        TotalPrincipal = temp[6],
                        TotalInterest = temp[7],
                        TotalTransferred = temp[8]
                    });
                }
            }
            return fileData;
        }
    }

    class Unwc11FileData
    {
        public string DateLoanAdded { get; set; }
        public string Source { get; set; }
        public string LoanProgram { get; set; }
        public string DealId { get; set; }
        public string TotalBorrowers { get; set; }
        public string TotalLoans { get; set; }
        public string TotalPrincipal { get; set; }
        public string TotalInterest { get; set; }
        public string TotalTransferred { get; set; }
    }
}
