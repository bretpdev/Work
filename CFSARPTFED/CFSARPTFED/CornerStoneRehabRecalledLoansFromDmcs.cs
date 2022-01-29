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
    class CornerStoneRehabRecalledLoansFromDmcs : ReportsBase
    {
        public CornerStoneRehabRecalledLoansFromDmcs(ReportData reports, ErrorReport err)
            : base(err)
        {
            Report = reports;
        }

        public void CreateTheReport()
        {
            //Locate the file to be used for processing.
            string fileToProcess = GetTheSasFile();
            if (fileToProcess.IsNullOrEmpty())
                return;

            List<UnwC10FileData> fileData = LoadTheFile(fileToProcess);

            using (ExcelGenerator excel = new ExcelGenerator(Report.ReportName + ".xlsx"))
            {
                excel.SetActiveWorksheet(1, "11015_RecallRehabRpt");
                excel.MergeCells("A2", "H2");
                excel.InsertData("A2", "A2", "Rehab/Recalled Loans From DMCS", Purple, Color.Black, new Font("Calibri", 18, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
                excel.MergeCells("A3", "H3");
                excel.InsertData("A3", "A3", string.Format("Data As of Month End: {0:MMMM yyyy}", DateTime.Now.AddMonths(-1)), Purple, Color.Black, new Font("Calibri", 12, FontStyle.Italic), false, ExcelGenerator.HCellAlignment.Right, ExcelGenerator.VCellAlignment.Bottom);
                excel.InsertImage("cornerstone_color_logo copy.jpg", 0, 0, 135, 80);
                excel.SetBorder("A2", "H3");

                int row = EnterHeaderInfo(excel, 8, "Loans Sent During the Month from DMCS for Loans Recalled by Servicer");
                row = EnterSasDataAndSumTotals(excel, fileData, "recall", row);
                row++;
                excel.SetBackground("A" + row, "H" + row, Purple);
                row += 2;

                row = EnterHeaderInfo(excel, row, "Recalled Loans Accepted at Servicer");
                row = EnterSasDataAndSumTotals(excel, fileData, "rehab", row);
                row++;
                excel.SetBackground("A" + row, "H" + row, Purple);
                row += 2;
                EnterDataDictionary(excel, row);
            }

            File.Delete(fileToProcess);
        }

        private void EnterDataDictionary(ExcelGenerator excel, int row)
        {
            int startingRow = row;
            excel.InsertData("A" + row, "A" + row, "Loan Type Key");
            excel.MergeCells("B" + row, "D" + row);
            row++;
            excel.MergeCells("A" + row, "D" + row);
            row++;

            excel.InsertData("A" + row, "A" + row, "D1");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "Direct Stafford Subsidized");
            row++;

            excel.InsertData("A" + row, "A" + row, "D2");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "Direct Stafford Unsubsidized");
            row++;

            excel.InsertData("A" + row, "A" + row, "D3");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "Direct Graduate PLUS");
            row++;

            excel.InsertData("A" + row, "A" + row, "D4");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "Direct Plus");
            row++;

            excel.InsertData("A" + row, "A" + row, "D5");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "Direct Consolidation Unsubsidized");
            row++;

            excel.InsertData("A" + row, "A" + row, "D6");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "Direct Consolidation Subsidized");
            row++;

            excel.InsertData("A" + row, "A" + row, "D7");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "Direct Plus Consolidation");
            row++;

            excel.InsertData("A" + row, "A" + row, "D8");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "TEACH loan converted from a TEACH grant");
            row++;

            excel.InsertData("A" + row, "A" + row, "CL");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "FFEL Consolidation");
            row++;

            excel.InsertData("A" + row, "A" + row, "GB");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "FFEL Graduate PLUS");
            row++;

            excel.InsertData("A" + row, "A" + row, "PL");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "FFEL PLUS");
            row++;

            excel.InsertData("A" + row, "A" + row, "RF");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "FFEL Refinanced");
            row++;
            excel.InsertData("A" + row, "A" + row, "SF");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "FFEL Stafford Subsidized");
            row++;

            excel.InsertData("A" + row, "A" + row, "SU");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "FFEL Stafford Unsubsidized");
            row++;

            excel.InsertData("A" + row, "A" + row, "FI");
            excel.MergeCells("B" + row, "D" + row);
            excel.InsertData("B" + row, "D" + row, "Federally Insured (FISL)");

            excel.SetBorder("A" + startingRow, "A" + row);
            excel.SetBorder("B" + startingRow, "D" + row);
        }

        private int EnterSasDataAndSumTotals(ExcelGenerator excel, List<UnwC10FileData> filedata, string type, int row)
        {
            int rowForSum = row;
            if (filedata.Where(p => p.RehabRecall.Contains(type)).Count() > 0)
            {
                foreach (UnwC10FileData fileLine in filedata.Where(p => p.RehabRecall.Contains(type)))
                {
                    excel.InsertData("A" + row, "A" + row, fileLine.PostDateAccepted);
                    excel.InsertData("B" + row, "B" + row, fileLine.LoanProgram);
                    excel.InsertData("C" + row, "C" + row, fileLine.LoanType);
                    excel.InsertData("D" + row, "D" + row, fileLine.NumberOfBorrowers);
                    excel.InsertData("E" + row, "E" + row, fileLine.NumberOfLoans);
                    excel.InsertData("F" + row, "F" + row, fileLine.Pbo);
                    excel.InsertData("G" + row, "G" + row, fileLine.Irb);
                    excel.InsertData("H" + row, "H" + row, fileLine.TotalSent);
                    row++;
                }
            }
            else
            {
                row++;
            }

            excel.InsertData("A" + row, "A" + row, "Total", FontWithBold, false);
            excel.Sum("D" + rowForSum, "D" + (row - 1), "D" + row, "D" + row, FontWithBold);
            excel.Sum("E" + rowForSum, "E" + (row - 1), "E" + row, "E" + row, FontWithBold);
            excel.Sum("F" + rowForSum, "F" + (row - 1), "F" + row, "F" + row, FontWithBold);
            excel.Sum("G" + rowForSum, "G" + (row - 1), "G" + row, "G" + row, FontWithBold);
            excel.Sum("H" + rowForSum, "H" + (row - 1), "H" + row, "H" + row, FontWithBold);

            excel.SetBorder("A" + rowForSum, "H" + row);
            return row;
        }

        private int EnterHeaderInfo(ExcelGenerator excel, int row, string header)
        {
            int startingRow = row;
            excel.MergeCells("A" + row, "H" + row);
            excel.InsertData("A" + row, "A" + row, header, new Font("Calibri", 9, FontStyle.Bold | FontStyle.Underline), false);
            row++;
            excel.InsertData("A" + row, "A" + row, "Date Transfered In", FontWithBold, false);
            excel.InsertData("B" + row, "B" + row, "Loan Program", FontWithBold, false);
            excel.InsertData("C" + row, "C" + row, "Loan Type", FontWithBold, false);
            excel.InsertData("D" + row, "D" + row, "Total No. of Borrowers", FontWithBold, false);
            excel.InsertData("E" + row, "E" + row, "Number of Loans", FontWithBold, false);
            excel.InsertData("F" + row, "F" + row, "Total Principal", FontWithBold, false);
            excel.InsertData("G" + row, "G" + row, "Total Interest", FontWithBold, false);
            excel.InsertData("H" + row, "H" + row, "Total Sent", FontWithBold, false);
            excel.SetBorder("A" + startingRow, "H" + row);
            return ++row;
        }

        private List<UnwC10FileData> LoadTheFile(string file)
        {
            List<UnwC10FileData> fileData = new List<UnwC10FileData>();

            using (StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine();//Read in the header.

                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    fileData.Add(new UnwC10FileData()
                    {
                        PostDateAccepted = temp[0],
                        LoanProgram = temp[1],
                        LoanType = temp[2],
                        NumberOfBorrowers = temp[3],
                        NumberOfLoans = temp[4],
                        Pbo = temp[5],
                        Irb = temp[6],
                        TotalSent = temp[7],
                        RehabRecall = temp[8]
                    });
                }

            }

            return fileData;
        }
    }

    class UnwC10FileData
    {
        public string PostDateAccepted { get; set; }
        public string RehabRecall { get; set; }
        public string LoanProgram { get; set; }
        public string LoanType { get; set; }
        public string NumberOfBorrowers { get; set; }
        public string NumberOfLoans { get; set; }
        public string Pbo { get; set; }
        public string Irb { get; set; }
        public string TotalSent { get; set; }
    }
}
