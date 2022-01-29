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
    class CornerStoneLoansTransferredToDMCSReport : ReportsBase
    {
        private Font FontBoldUnderline;
        public CornerStoneLoansTransferredToDMCSReport(ReportData reports, ErrorReport err)
            : base(err)
        {
            Report = reports;
            FontBoldUnderline = new Font("Calibri", 9, FontStyle.Bold | FontStyle.Underline);
        }

        public void CreateTheReport()
        {
            string fileToProcess = GetTheSasFile();
            if (fileToProcess.IsNullOrEmpty())
                return;

            List<UnwC12FileData> fileData = ReadTheFile(fileToProcess);

            using (ExcelGenerator excel = new ExcelGenerator(Report.ReportName + ".xls"))
            {
                excel.SetActiveWorksheet(1, "Sheet 1");
                excel.MergeCells("B2", "H2");
                excel.InsertData("B2", "B2", "Transferred to DMCS", Purple, Color.Black, new Font("Calibri", 18, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
                excel.MergeCells("B3", "H3");
                excel.InsertData("B3", "B3", string.Format("Data As of Month End: {0:MMMM yyyy}", DateTime.Now.AddMonths(-1)), Purple, Color.Black, new Font("Calibri", 12, FontStyle.Italic), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertImage("cornerstone_color_logo copy.jpg", 0, 0, 135, 80);
                excel.InsertData("A8", "A8", "NEVER REJECTED", Color.White, Color.Black, FontBoldUnderline, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                excel.MergeCells("B8", "H8");
                excel.InsertData("B8", "B8", "(loans that were never rejected by DMCS in the past and have been accepted on first submission)", Color.White, Color.Black, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                int currentRow = EnterHeaderInformation(excel, 9);
                int rowForSum = currentRow;

                foreach (UnwC12FileData fileLine in fileData.Where(p => p.RejectedStatus.Contains("N")))
                {
                    excel.InsertData("B" + currentRow, "B" + currentRow, fileLine.LoanProgram);
                    excel.InsertData("C" + currentRow, "C" + currentRow, fileLine.LoanType);
                    excel.InsertData("D" + currentRow, "D" + currentRow, fileLine.NumberOfBorrowers);
                    excel.InsertData("E" + currentRow, "E" + currentRow, fileLine.NumberOfLoans);
                    excel.InsertData("F" + currentRow, "F" + currentRow, fileLine.PrincipalBalance);
                    excel.InsertData("G" + currentRow, "G" + currentRow, fileLine.Interest);
                    excel.InsertData("H" + currentRow, "H" + currentRow, fileLine.TotalAmountAccepted);
                    currentRow++;
                }

                SumTotals(excel, rowForSum, currentRow, fileData);
                currentRow++;
                excel.MergeCells("A" + currentRow, "H" + currentRow);
                excel.SetBackground("A" + currentRow, "A" + currentRow, Purple);
                currentRow++;
                excel.MergeCells("A" + currentRow, "H" + currentRow);
                currentRow++;

                excel.MergeCells("A" + currentRow, "B" + currentRow);
                excel.InsertData("A" + currentRow, "A" + currentRow, "REJECTED AND ACCPETED", Color.White, Color.Black, FontBoldUnderline, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                excel.MergeCells("C" + currentRow, "H" + currentRow);
                excel.InsertData("C" + currentRow, "C" + currentRow, "(Was previously submitted to DMCS & rejected, but has been resubmitted and has been accepted)", Color.White, Color.Black, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                currentRow++;
                currentRow = EnterHeaderInformation(excel, currentRow);

                rowForSum = currentRow;
                foreach (UnwC12FileData fileLine in fileData.Where(p => p.RejectedStatus.Contains("R")))
                {
                    excel.InsertData("B" + currentRow, "B" + currentRow, fileLine.LoanProgram);
                    excel.InsertData("C" + currentRow, "C" + currentRow, fileLine.LoanType);
                    excel.InsertData("D" + currentRow, "D" + currentRow, fileLine.NumberOfBorrowers);
                    excel.InsertData("E" + currentRow, "E" + currentRow, fileLine.NumberOfLoans);
                    excel.InsertData("F" + currentRow, "F" + currentRow, fileLine.PrincipalBalance);
                    excel.InsertData("G" + currentRow, "G" + currentRow, fileLine.Interest);
                    excel.InsertData("H" + currentRow, "H" + currentRow, fileLine.TotalAmountAccepted);
                    currentRow++;
                }

                SumTotals(excel, rowForSum, currentRow, fileData);
                currentRow++;

                excel.MergeCells("A" + currentRow, "H" + currentRow);
                excel.SetBackground("A" + currentRow, "A" + currentRow, Purple);
                currentRow++;
                excel.MergeCells("A" + currentRow, "H" + currentRow);
                currentRow++;

                excel.MergeCells("A" + currentRow, "B" + currentRow);
                excel.InsertData("A" + currentRow, "A" + currentRow, "Totals for Month", FontBoldUnderline, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                currentRow++;

                currentRow = EnterHeaderInformation(excel, currentRow);
                excel.InsertData("B" + currentRow, "B" + currentRow, "Total", FontWithBold, false);
                excel.InsertData("D" + currentRow, "D" + currentRow, fileData.Sum(p => decimal.Parse(p.NumberOfBorrowers)).ToString(), FontWithBold, false);
                excel.InsertData("E" + currentRow, "E" + currentRow, fileData.Sum(p => decimal.Parse(p.NumberOfLoans)).ToString(), FontWithBold, false);
                excel.InsertData("F" + currentRow, "F" + currentRow, fileData.Sum(p => decimal.Parse(p.PrincipalBalance)).ToString(), FontWithBold, false);
                excel.InsertData("G" + currentRow, "G" + currentRow, fileData.Sum(p => decimal.Parse(p.Interest)).ToString(), FontWithBold, false);
                excel.InsertData("H" + currentRow, "H" + currentRow, fileData.Sum(p => decimal.Parse(p.TotalAmountAccepted)).ToString(), FontWithBold, false);
                currentRow++;
                excel.MergeCells("A" + currentRow, "H" + currentRow);
                excel.SetBackground("A" + currentRow, "A" + currentRow, Purple);
                currentRow++;
                excel.MergeCells("A" + currentRow, "H" + currentRow);
                currentRow++;
                excel.MergeCells("A" + currentRow, "H" + currentRow);
                currentRow++;
                EnterDataDictionary(excel, currentRow);
            }
        }

        private void EnterDataDictionary(ExcelGenerator excel, int currentRow)
        {
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Data Dictionary", FontBoldUnderline, false);
            currentRow += 2;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Loan Program", FontWithBold, false);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "The loan program code which the loan(s) belong to:" + Environment.NewLine + "- Direct = Direct Loans" + Environment.NewLine
                + "- FFEL = FFEL Loans", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Loan Tyoe", FontWithBold, false);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, CreateLoanTypeString(), FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Borrower Count", FontWithBold, false);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Number of Borrowers with a loan being sent to DMCS", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Loan Count", FontWithBold, false);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Number of loans being sent to DMCS", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "PBO", FontWithBold, false);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Principal Balance still owed on loans", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "IRB", FontWithBold, false);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Interest still owed on loans", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Total Amount Accepted", FontWithBold, false);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Total amount accepted by DMCS and/or TPD", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Total", FontWithBold, false);
            currentRow++;
            excel.MergeCells("A" + currentRow, "H" + currentRow);
            excel.InsertData("A" + currentRow, "A" + currentRow, "Total amounts for column types", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
        }

        private void SumTotals(ExcelGenerator excel, int rowForSum, int currentRow, List<UnwC12FileData> fileData)
        {
            excel.InsertData("A" + currentRow, "A" + currentRow, "Total", FontWithBold, false);
            if (fileData.Where(p => p.RejectedStatus.Contains("N")).Count() > 0)
            {
                excel.Sum("D" + rowForSum, "D" + (currentRow - 1), "D" + currentRow, "D" + currentRow, FontWithBold);
                excel.Sum("E" + rowForSum, "E" + (currentRow - 1), "E" + currentRow, "E" + currentRow, FontWithBold);
                excel.Sum("F" + rowForSum, "F" + (currentRow - 1), "F" + currentRow, "F" + currentRow, FontWithBold);
                excel.Sum("G" + rowForSum, "G" + (currentRow - 1), "G" + currentRow, "G" + currentRow, FontWithBold);
                excel.Sum("H" + rowForSum, "H" + (currentRow - 1), "H" + currentRow, "H" + currentRow, FontWithBold);
            }
            else
            {
                excel.InsertData("D" + currentRow, "D" + currentRow, "0");
                excel.InsertData("E" + currentRow, "E" + currentRow, "0");
                excel.InsertData("F" + currentRow, "F" + currentRow, "$0.00");
                excel.InsertData("G" + currentRow, "G" + currentRow, "$0.00");
                excel.InsertData("H" + currentRow, "H" + currentRow, "$0.00");
            }
        }

        private int EnterHeaderInformation(ExcelGenerator excel, int nextRow)
        {
            excel.InsertData("A" + nextRow, "A" + nextRow, "Date Accepted", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("B" + nextRow, "B" + nextRow, "Loan Program", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("C" + nextRow, "C" + nextRow, "Loan Type", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("D" + nextRow, "D" + nextRow, "Borrower Count", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("E" + nextRow, "E" + nextRow, "Loan Count", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("F" + nextRow, "F" + nextRow, "PBO", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("G" + nextRow, "G" + nextRow, "IRB", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("H" + nextRow, "H" + nextRow, "Total Accepted", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);

            return nextRow++;
        }

        private List<UnwC12FileData> ReadTheFile(string file)
        {
            List<UnwC12FileData> data = new List<UnwC12FileData>();
            using (StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine();//Read in the header row.
                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    data.Add(new UnwC12FileData()
                    {
                        LoanProgram = temp[0],
                        LoanType = temp[1],
                        NumberOfBorrowers = temp[2],
                        NumberOfLoans = temp[3],
                        PrincipalBalance = temp[4],
                        Interest = temp[5],
                        TotalAmountAccepted = temp[6],
                        RejectedStatus = temp[7]
                    });
                }
            }
            return data;
        }
    }

    class UnwC12FileData
    {
        public string LoanProgram { get; set; }
        public string LoanType { get; set; }
        public string NumberOfBorrowers { get; set; }
        public string NumberOfLoans { get; set; }
        public string PrincipalBalance { get; set; }
        public string Interest { get; set; }
        public string TotalAmountAccepted { get; set; }
        public string RejectedStatus { get; set; }
    }
}
