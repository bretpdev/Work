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
    class LastMonthData
    {
        public int FiscalYearToDateBorrowers { get; set; }
        public int FiscalYearToDateLoans { get; set; }
        public decimal FiscalYearToDateAmount { get; set; }

        public LastMonthData()
        {
            FiscalYearToDateAmount = 0;
            FiscalYearToDateBorrowers = 0;
            FiscalYearToDateLoans = 0;
        }
    }

    class CornerStoneBankruptcyReport : ReportsBase
    {
        public CornerStoneBankruptcyReport(ReportData reports, ErrorReport err)
            : base(err)
        {
            Report = reports;
        }

        public void CreateTheReport()
        {
            List<string> filesToProcess = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, Report.ReportName).ToList();

            if (!CheckMutlipleFiles(filesToProcess))
                return;

            int row = 7;
            using (ExcelGenerator excel = new ExcelGenerator(Report.ReportName + "xlsx"))
            {
                excel.SetActiveWorksheet(1, "Sheet1");
                excel.MergeCells("D2", "G2");
                excel.InsertData("D2", "D2", "CornerStone - Bankruptcy Report", new Font("Arial", 14, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
                excel.MergeCells("D3", "G3");
                excel.InsertData("D3", "D3", string.Format("Month ending - {0:MMMM yyyy}", DateTime.Now.AddMonths(-1)), new Font("Arial", 12, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
                excel.AdjustWidth("A1", "A1", 8.34);
                excel.InsertImage("cornerstone_color_logo copy.jpg", 40, 5, 135, 80);
                excel.SetBorder("D2", "G3", 3);

                foreach (string file in filesToProcess)
                {
                    List<Unwc06FileData> fileData = LoadTheFile(file);
                    row = CreateTableInsertData(excel, fileData, row, file.Contains("R2") ? "Direct Loans" : "FFEL");
                    row++;
                }

                EnterDataDictionary(excel, ++row);
                excel.AdjustWidth("B1", "B1", 8.43);
                excel.AdjustWidth("C1", "C1", 15.29);
                excel.AdjustWidth("D1", "D1", 16.71);
                excel.AdjustWidth("E1", "E1", 15.29);
                excel.AdjustWidth("F1", "F1", 17.29);
                excel.AdjustWidth("G1", "G1", 14.86);
            }
        }

        private void EnterDataDictionary(ExcelGenerator excel, int row)
        {
            int rowForBoarder = row;
            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, "Field Name", new Font("Calibri", 9, FontStyle.Italic), false);
            excel.MergeCells("D" + row, "F" + row);
            excel.InsertData("D" + row, "F" + row, "Definition", new Font("Calibri", 9, FontStyle.Italic), false);
            row++;

            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, "Bankruptcy Suspense", FontWithNoBold, false);
            excel.MergeCells("D" + row, "F" + row);
            excel.InsertData("D" + row, "F" + row, "Summary of accounts in collection activity suspended due to bankruptcy by month ending and fiscal year to date.", FontWithNoBold, true);
            excel.AdjustHeight("A" + row, "A" + row, 27.75);
            row++;

            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, "First Meeting of Creditors", FontWithNoBold, false);
            excel.MergeCells("D" + row, "F" + row);
            excel.InsertData("D" + row, "F" + row, "Summary of accounts that have received a meeting of creditors by month ending and fiscal year to date.", FontWithNoBold, true);
            excel.AdjustHeight("A" + row, "A" + row, 27.75);
            row++;

            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, "# Borrowers in Bankruptcy", FontWithNoBold, false);
            excel.MergeCells("D" + row, "F" + row);
            excel.InsertData("D" + row, "F" + row, "Total number of borrowers in bankruptcy.", FontWithNoBold, false);
            row++;

            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, "# Loans in Bankruptcy", FontWithNoBold, false);
            excel.MergeCells("D" + row, "F" + row);
            excel.InsertData("D" + row, "F" + row, "Total number of loans in bankruptcy.", FontWithNoBold, false);
            row++;

            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, "# Dollars in Bankruptcy", FontWithNoBold, false);
            excel.MergeCells("D" + row, "F" + row);
            excel.InsertData("D" + row, "F" + row, "Total principal balance and interest balance outstanding for loans in bankruptcy.", FontWithNoBold, true);
            excel.AdjustHeight("A" + row, "A" + row, 27.75);

            excel.SetBorder("B" + rowForBoarder, "F" + row);
        }

        private int CreateTableInsertData(ExcelGenerator excel, List<Unwc06FileData> fileData, int row, string type)
        {
            int rowForBoarder = row;
            int bkMocThisMonth = fileData.Where(p => p.Status.Contains("06") && p.ReceivedDate.Date > DateTime.Now.AddMonths(-2).Date).Count();
            int bkMocLoansThisMonth = fileData.Where(p => p.Status.Contains("06") && p.ReceivedDate.Date > DateTime.Now.AddMonths(-2).Date).Sum(p => p.NumberOfLoans);

            int bkTotalThisMonth = fileData.Where(p => p.Status.Contains("06")).Count();
            int bkTotalLoansThisMonth = fileData.Where(p => p.Status.Contains("06")).Sum(p => p.NumberOfLoans);
            decimal bkBorrowerDollars = fileData.Where(p => p.Status.Contains("06")).Sum(p => p.Total);
            decimal bkLoanDollars = fileData.Where(p => p.Status.Contains("06") && p.ReceivedDate.Date > DateTime.Now.AddMonths(-2).Date).Sum(p => p.Total);

            LastMonthData fyToDateNumbers = DataAccess.GetYearToDateNumbers(type.Contains("Direct Loans"));

            int borrowers = (fyToDateNumbers.FiscalYearToDateBorrowers + bkMocThisMonth);
            int loans = (fyToDateNumbers.FiscalYearToDateLoans + bkMocLoansThisMonth);
            decimal dollars = (fyToDateNumbers.FiscalYearToDateAmount + bkBorrowerDollars);

            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, type, new Font("Arial", 14, FontStyle.Bold), false);
            row++;

            excel.MergeCells("B" + row, "C" + row);
            excel.SetBackground("B" + row, "B" + row, Color.White);
            excel.MergeCells("D" + row, "E" + row);
            excel.InsertData("D" + row, "D" + row, "Bankruptcy Suspense", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.MergeCells("F" + row, "G" + row);
            excel.InsertData("F" + row, "G" + row, "First Meeting of Creditors", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            row++;
            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, "Category", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("D" + row, "D" + row, "This Month", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("E" + row, "E" + row, "FY to Date", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("F" + row, "F" + row, "This Month", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("G" + row, "G" + row, "FY to Date", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            row++;

            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, "# Borrowers in Bankruptcy", Purple, Color.White, ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("D" + row, "D" + row, bkTotalThisMonth.ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("E" + row, "E" + row, borrowers.ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("F" + row, "F" + row, bkMocThisMonth.ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("G" + row, "G" + row, (fyToDateNumbers.FiscalYearToDateBorrowers + bkMocThisMonth).ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            row++;

            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, "# Loans in Bankruptcy", Purple, Color.White, ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("D" + row, "D" + row, bkTotalLoansThisMonth.ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("E" + row, "E" + row, loans.ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("F" + row, "F" + row, bkMocLoansThisMonth.ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("G" + row, "G" + row, (fyToDateNumbers.FiscalYearToDateLoans + bkMocLoansThisMonth).ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            row++;

            excel.MergeCells("B" + row, "C" + row);
            excel.InsertData("B" + row, "B" + row, "# Dollars in Bankruptcy", Purple, Color.White, ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("D" + row, "D" + row, bkBorrowerDollars.ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("E" + row, "E" + row, dollars.ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("F" + row, "F" + row, bkLoanDollars.ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("G" + row, "G" + row, (fyToDateNumbers.FiscalYearToDateAmount + bkLoanDollars).ToString(), ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.ToMoneyFormat("D" + row, "G" + row);
            excel.SetBorder("B" + (rowForBoarder+1), "G" + row);

            DataAccess.UpdateTotals(borrowers, loans, dollars, type.Contains("Direct Loans"));

            return ++row;
        }

        private List<Unwc06FileData> LoadTheFile(string fileName)
        {
            List<Unwc06FileData> fileData = new List<Unwc06FileData>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                sr.ReadLine();//Read out the header.

                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    fileData.Add(new Unwc06FileData()
                    {
                        Ssn = temp[0],
                        Status = temp[1],
                        Chapter = temp[2],
                        ReceivedDate = DateTime.Parse(temp[3]),
                        StatusDate = DateTime.Parse(temp[4]),
                        NumberOfLoans = int.Parse(temp[5]),
                        Total = decimal.Parse(temp[8].Replace("$", ""))
                    });
                }
            }

            return fileData;
        }
    }

    class Unwc06FileData
    {
        public string Ssn { get; set; }
        public string Status { get; set; }
        public string Chapter { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime StatusDate { get; set; }
        public int NumberOfLoans { get; set; }
        public decimal Total { get; set; }
    }
}
