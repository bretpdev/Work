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
    class CornerStoneLoansTransferedToFromTpd : ReportsBase
    {
        public CornerStoneLoansTransferedToFromTpd(ReportData reports, ErrorReport err)
            : base(err)
        {
            Report = reports;
        }

        public void CreateTheReport()
        {
            string fileToProcess = GetTheSasFile();
            if (fileToProcess.IsNullOrEmpty())
                return;

            List<UnwC16FileData> fileData = ReadTheFile(fileToProcess);
            using (ExcelGenerator excel = new ExcelGenerator(Report.ReportName + ".xls"))
            {
                excel.SetActiveWorksheet(1, "11017_ToFrmTpd");
                excel.MergeCells("B2", "H2");
                excel.InsertData("B2", "B2", "Loans Transferred to/from TPD Report", Purple, Color.Black, new Font("Calibri", 18, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
                excel.MergeCells("B3", "H3");
                excel.InsertData("B3", "B3", string.Format("Data As of Month End: {0:MMMM yyyy}", DateTime.Now.AddMonths(-1)), Purple, Color.Black, new Font("Calibri", 12, FontStyle.Italic), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertImage("cornerstone_color_logo copy.jpg", 0, 0, 135, 80);
                EnterHeaderInfo(excel, "Loans Transferred To TPD During the Month", 8);
                int currentLineNumber = EnterSasData(fileData, excel, 10, true);
                EnterHeaderInfo(excel, "Loans Transferred From TPD During the Month (as a result of recalls from servicer)", currentLineNumber);
                currentLineNumber += 2;
                currentLineNumber = EnterSasData(fileData, excel, currentLineNumber, false);
                currentLineNumber++;
            }
        }

        private void EnterHeaderInfo(ExcelGenerator excel, string message, int line)
        {
            excel.MergeCells("A" + line, "H" + line);
            excel.InsertData("A" + line, "A" + line, message, new Font("Calibri", 9, FontStyle.Bold | FontStyle.Underline), false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Date Accepted", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
            excel.InsertData("B" + line, "B" + line, "Loan Program", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
            excel.InsertData("C" + line, "C" + line, "Loan Type", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
            excel.InsertData("D" + line, "D" + line, "Borrower Count", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
            excel.InsertData("E" + line, "E" + line, "Loan Count", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
            excel.InsertData("F" + line, "F" + line, "PBO", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
            excel.InsertData("G" + line, "G" + line, "IRB", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
            excel.InsertData("H" + line, "H" + line, "Total Accepted", Purple, Color.White, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
        }

        private int EnterSasData(List<UnwC16FileData> fileData, ExcelGenerator excel, int lineNumber, bool transferred)
        {
            int lineNumberForSum = lineNumber;
            foreach (UnwC16FileData fileLine in fileData.Where(p => p.Transferred == transferred))
            {
                excel.InsertData("A" + lineNumber, "A" + lineNumber, fileLine.DateAccepted, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("B" + lineNumber, "B" + lineNumber, fileLine.LoanProgram, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("C" + lineNumber, "C" + lineNumber, fileLine.LoanType, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("D" + lineNumber, "D" + lineNumber, fileLine.BorrowerCount, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("E" + lineNumber, "E" + lineNumber, fileLine.LoanCount, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("F" + lineNumber, "F" + lineNumber, fileLine.Pbo, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("G" + lineNumber, "G" + lineNumber, fileLine.Irb, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("H" + lineNumber, "H" + lineNumber, fileLine.TotalAmountAccepted, FontWithNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                lineNumber++;
            }

            excel.InsertData("A" + lineNumber, "A" + lineNumber, "Total", FontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
            excel.Sum("D" + lineNumberForSum, "D" + (lineNumber - 1), "D" + lineNumber, "D" + lineNumber, FontWithBold);
            excel.Sum("E" + lineNumberForSum, "E" + (lineNumber - 1), "E" + lineNumber, "E" + lineNumber, FontWithBold);
            excel.Sum("F" + lineNumberForSum, "F" + (lineNumber - 1), "F" + lineNumber, "F" + lineNumber, FontWithBold);
            excel.Sum("G" + lineNumberForSum, "G" + (lineNumber - 1), "G" + lineNumber, "G" + lineNumber, FontWithBold);
            excel.Sum("H" + lineNumberForSum, "H" + (lineNumber - 1), "H" + lineNumber, "H" + lineNumber, FontWithBold);
            lineNumber++;
            excel.MergeCells("A" + lineNumber, "H" + lineNumber);
            excel.SetBackground("A" + lineNumber, "A" + lineNumber, Purple);
            lineNumber += 2;

            return lineNumber;
        }

        private void EnterDataDictionary(ExcelGenerator excel, int line)
        {
            excel.MergeCells("A" + line, "H" + line);
            excel.InsertData("A" + line, "A" + line, "Data Dictionary", new Font("Calibri", 9, FontStyle.Bold | FontStyle.Underline), false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line += 2;
            excel.InsertData("A" + line, "A" + line, "Loan Program", FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "The loan program code which the loan(s) belong to:" + Environment.NewLine + "- Direct = Direct Loans" + Environment.NewLine + "- FFEL = FFEL Loans", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Loan Type", FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, CreateLoanTypeString(), FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Borrower Count", FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Number of Borrowers with a loan being sent to DMCS" + Environment.NewLine + "- Direct = Direct Loans" + Environment.NewLine + "- FFEL = FFEL Loans", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Loan Count", FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Number of loans being sent to DMCS" + Environment.NewLine + "- Direct = Direct Loans" + Environment.NewLine + "- FFEL = FFEL Loans", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "PBO", FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Principal Balance still owed on loans" + Environment.NewLine + "- Direct = Direct Loans" + Environment.NewLine + "- FFEL = FFEL Loans", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "IRB", FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Interest still owed on loans" + Environment.NewLine + "- Direct = Direct Loans" + Environment.NewLine + "- FFEL = FFEL Loans", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Total Amount Accepted", FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Total amount accepted by DMCS and/or TPD" + Environment.NewLine + "- Direct = Direct Loans" + Environment.NewLine + "- FFEL = FFEL Loans", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Total", FontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
            excel.InsertData("A" + line, "A" + line, "Total amounts for column types" + Environment.NewLine + "- Direct = Direct Loans" + Environment.NewLine + "- FFEL = FFEL Loans", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            line++;
        }

        private List<UnwC16FileData> ReadTheFile(string file)
        {
            List<UnwC16FileData> data = new List<UnwC16FileData>();
            using (StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine();//Read in the header row.
                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    data.Add(new UnwC16FileData() { DateAccepted = temp[0], LoanProgram = temp[1], LoanType = temp[2], BorrowerCount = temp[3], LoanCount = temp[4], Pbo = temp[5], Irb = temp[6], TotalAmountAccepted = temp[7], Transferred = temp[8].Contains("T") });
                }
            }
            return data;
        }
    }

    class UnwC16FileData
    {
        public string DateAccepted { get; set; }
        public string LoanProgram { get; set; }
        public string LoanType { get; set; }
        public string BorrowerCount { get; set; }
        public string LoanCount { get; set; }
        public string Pbo { get; set; }
        public string Irb { get; set; }
        public string TotalAmountAccepted { get; set; }
        public bool Transferred { get; set; }
    }
}
