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
    class CornerStoneBankruptcyAgingReport : ReportsBase
    {
        public CornerStoneBankruptcyAgingReport(ReportData reports, ErrorReport err)
            : base(err)
        {
            Report = reports;
        }

        public void CreateTheReport()
        {
            List<string> files = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, Report.ReportName).ToList();

            if (!CheckMutlipleFiles(files))
                return;

            //Load the file data into objects.
            List<Unwc07FileData> r2FileData = LoadTheFile(files.Where(p => p.Contains("R2")).First());
            List<Unwc07FileData> r3FileData = LoadTheFile(files.Where(p => p.Contains("R3")).First());

            using (ExcelGenerator excel = new ExcelGenerator(Report.ReportName + "xlsx"))
            {
                excel.SetActiveWorksheet(1, "Sheet1");
                excel.MergeCells("C2", "F2");
                excel.InsertData("C2", "C2", "CornerStone - Bankruptcy Aging Report", new Font("Arial", 14, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
                excel.MergeCells("C3", "F3");
                excel.InsertData("C3", "C3", string.Format("Month ending - {0:MMMM yyyy}", DateTime.Now.AddMonths(-1)), new Font("Arial", 12, FontStyle.Bold), false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
                excel.AdjustWidth("A1", "A1", 4.86);
                excel.InsertImage("cornerstone_color_logo copy.jpg", 35, 5, 135, 80);
                excel.SetBorder("C2", "F3", 3);

                excel.InsertData("B8", "B8", "Direct Loans", new Font("Arial", 14, FontStyle.Bold), false);
                excel.InsertData("B9", "B9", "Bankruptcy Type and Aging", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);

                int row = EnterData(excel, 10, "", r2FileData);
                excel.SetBorder("B10", "E" + (row - 1), 2);
                row += 3;

                excel.InsertData("B" + row, "B" + row, "FFEL", new Font("Arial", 14, FontStyle.Bold), false);
                row++;
                excel.InsertData("B" + row, "B" + row, "Bankruptcy Type and Aging", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
                row++;
                int rowForBaorder = row;
                row = EnterData(excel, row, "", r3FileData);
                excel.SetBorder("B" + rowForBaorder, "E" + (row - 1), 2);
                row += 2;

                EnterDataDictionary(excel, row);

                excel.AdjustWidth("B1", "B1", 28.71);
                excel.AdjustWidth("C1", "C1", 21.71);
                excel.AdjustWidth("D1", "D1", 18.29);
                excel.AdjustWidth("E1", "E1", 16);
            }

            File.Delete(files.Where(p => p.Contains("R2")).First());
            File.Delete(files.Where(p => p.Contains("R2")).First());
        }

        private void EnterDataDictionary(ExcelGenerator excel, int row)
        {
            int rowForBoarder = row;
            excel.InsertData("B" + row, "B" + row, "Field name", Italics, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.MergeCells("C" + row, "D" + row);
            excel.InsertData("C" + row, "C" + row, "Definition", Italics, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            row++;
            excel.InsertData("B" + row, "B" + row, "Bankruptcy Type and Aging", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Top);
            excel.MergeCells("C" + row, "D" + row);
            excel.InsertData("C" + row, "C" + row, "Bankruptcy chapter and categories for the length of time the account has been in bankruptcy.", FontWithNoBold, true);
            excel.AdjustHeight("A" + row, "A" + row, 29.25);
            row++;

            excel.InsertData("B" + row, "B" + row, "No. Borrowers", FontWithNoBold, false);
            excel.MergeCells("C" + row, "D" + row);
            excel.InsertData("C" + row, "C" + row, "Total number of borrowers.", FontWithNoBold, false);
            row++;

            excel.InsertData("B" + row, "B" + row, "No. Loans", FontWithNoBold, false);
            excel.MergeCells("C" + row, "D" + row);
            excel.InsertData("C" + row, "C" + row, "Total number of loans.", FontWithNoBold, false);
            row++;

            excel.InsertData("B" + row, "B" + row, "Dollars", FontWithNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Top);
            excel.MergeCells("C" + row, "D" + row);
            excel.InsertData("C" + row, "C" + row, "Total sum of principal balance outstanding and interest balance outstanding.", FontWithNoBold, true);
            excel.AdjustHeight("A" + row, "A" + row, 29.25);
            row++;

            excel.InsertData("B" + row, "B" + row, "Total", FontWithNoBold, false);
            excel.MergeCells("C" + row, "D" + row);
            excel.InsertData("C" + row, "C" + row, "Total for selected column.", FontWithNoBold, false);

            excel.SetBorder("B" + rowForBoarder, "D" + row, 3);
        }

        private int EnterData(ExcelGenerator excel, int row, string chapter, List<Unwc07FileData> fileData)
        {
            row = EnterHeader(excel, row, "7");
            int rowForSum = row;

            List<Unwc07FileData> selectedFileData = fileData.Where(p => p.BankruptcyType.Contains("7")).ToList();
            row = EnterSasData(excel, selectedFileData, row);
            row = SumRows(excel, rowForSum, row, "7");
            excel.SetBackground("B" + row, "E" + row, Purple);
            row++;

            row = EnterHeader(excel, row, "11");
            rowForSum = row;
            selectedFileData = fileData.Where(p => p.BankruptcyType.Contains("11")).ToList();
            row = EnterSasData(excel, selectedFileData, row);
            row = SumRows(excel, rowForSum, row, "11");
            excel.SetBackground("B" + row, "E" + row, Purple);
            row++;

            row = EnterHeader(excel, row, "12");
            rowForSum = row;
            selectedFileData = fileData.Where(p => p.BankruptcyType.Contains("12")).ToList();
            row = EnterSasData(excel, selectedFileData, row);
            row = SumRows(excel, rowForSum, row, "12");
            excel.SetBackground("B" + row, "E" + row, Purple);
            row++;

            row = EnterHeader(excel, row, "13");
            rowForSum = row;
            selectedFileData = fileData.Where(p => p.BankruptcyType.Contains("13")).ToList();
            row = EnterSasData(excel, selectedFileData, row);
            row = SumRows(excel, rowForSum, row, "13");
            excel.SetBackground("B" + row, "E" + row, Purple);

            return ++row;
        }

        private int EnterSasData(ExcelGenerator excel, List<Unwc07FileData> selectedFileData, int row)
        {
            row = GroupAndEnterSasData(excel, selectedFileData, "< 6 Months", row);
            row = GroupAndEnterSasData(excel, selectedFileData, "> 6 Months <= 1 year", row);
            row = GroupAndEnterSasData(excel, selectedFileData, "> 1 Months <= 2 year", row);
            row = GroupAndEnterSasData(excel, selectedFileData, "> 2 Months <= 3 year", row);
            row = GroupAndEnterSasData(excel, selectedFileData, "> 3 Months <= 4 year", row);
            row = GroupAndEnterSasData(excel, selectedFileData, "> 4 Months <= 5 year", row);
            return GroupAndEnterSasData(excel, selectedFileData, "> 5 years", row);
        }

        private int EnterHeader(ExcelGenerator excel, int row, string chapter)
        {
            excel.InsertData("B" + row, "B" + row, string.Format("Chapter {0}", chapter), Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("C" + row, "C" + row, "No. Borrowers", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("D" + row, "D" + row, "No. Loans", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("E" + row, "E" + row, "Dollars", Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            return ++row;
        }

        private int GroupAndEnterSasData(ExcelGenerator excel, List<Unwc07FileData> fileData, string type, int row)
        {
            string numberOfBorrowers = fileData.Where(p => p.Age.Contains(type.ToUpper())).Count().ToString();
            string numberOfLoans = fileData.Where(p => p.Age.Contains(type.ToUpper())).Sum(p => int.Parse(p.NumberOfLoans)).ToString();
            string dollars = "$" + fileData.Where(p => p.Age.Contains(type.ToUpper())).Sum(p => decimal.Parse(p.Total.Replace("$", ""))).ToString();

            excel.InsertData("B" + row, "B" + row, type, Purple, Color.White, ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("C" + row, "C" + row, numberOfBorrowers, ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("D" + row, "D" + row, numberOfLoans, ArialFontNoBold, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("E" + row, "E" + row, dollars, ArialFontNoBold, false);

            return ++row;
        }

        private int SumRows(ExcelGenerator excel, int rowForSum, int row, string chapter)
        {
            excel.InsertData("B" + row, "B" + row, string.Format("Chapter {0} Total", chapter), Purple, Color.White, ArialFontWithBold, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.Sum("C" + rowForSum, "C" + (row - 1), "C" + row, "C" + row, ArialFontWithBold, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.Sum("D" + rowForSum, "D" + (row - 1), "D" + row, "D" + row, ArialFontWithBold, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Bottom);
            excel.Sum("E" + rowForSum, "E" + (row - 1), "E" + row, "E" + row, ArialFontWithBold);
            return ++row;
        }

        private List<Unwc07FileData> LoadTheFile(string fileName)
        {
            List<Unwc07FileData> fileData = new List<Unwc07FileData>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                sr.ReadLine();//Read in the header row.

                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    //Add the object to the list.
                    fileData.Add(new Unwc07FileData()
                    {
                        Ssn = temp[0],
                        BankruptcyType = temp[1],
                        Age = temp[2],
                        NumberOfMonths = temp[3],
                        BankruptcyStatus = temp[4],
                        ReceivedDate = temp[5],
                        NumberOfLoans = temp[6],
                        Pbo = temp[7],
                        Irb = temp[8],
                        Total = temp[9]
                    });
                }
            }

            return fileData;
        }
    }

    class Unwc07FileData
    {
        public string Ssn { get; set; }
        public string BankruptcyType { get; set; }
        public string Age { get; set; }
        public string NumberOfMonths { get; set; }
        public string BankruptcyStatus { get; set; }
        public string ReceivedDate { get; set; }
        public string NumberOfLoans { get; set; }
        public string Pbo { get; set; }
        public string Irb { get; set; }
        public string Total { get; set; }
    }
}
