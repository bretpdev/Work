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
    class CornerStoneMonthlyIbrTracking : ReportsBase
    {
        private string MonthEndDate { get; set; }
        private string PortfolioType { get; set; }
        private string CumulativeApplicationReceived { get; set; }
        private string CumulativeApplicationApproved { get; set; }
        private string CumulativeApplicationRejected { get; set; }
        private string BorrowersOnIbr { get; set; }
        private string TotalProtfolio { get; set; }
        private string RepaymentPortfolio { get; set; }

        public CornerStoneMonthlyIbrTracking(ReportData reports, ErrorReport err)
            : base(err)
        {
            Report = reports;
        }

        public void CreateTheReport()
        {
            string fileToProcess = GetTheSasFile();
            if (fileToProcess.IsNullOrEmpty())
                return;

            LoadDataFromFile(fileToProcess);

            using (ExcelGenerator excel = new ExcelGenerator(Report.ReportName + ".xls"))
            {
                int row = excel.GetNextRow(0, "C");
                excel.InsertData("C" + row, "C" + row, CumulativeApplicationReceived);
                excel.InsertData("D" + row, "D" + row, CumulativeApplicationApproved);
                excel.InsertData("E" + row, "E" + row, CumulativeApplicationRejected);
                excel.InsertData("F" + row, "F" + row, TotalProtfolio);
                excel.InsertData("G" + row, "G" + row, RepaymentPortfolio);
            }

            File.Delete(fileToProcess);
        }

        private void LoadDataFromFile(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine();//Read in the header row
                List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");

                MonthEndDate = temp[0];
                PortfolioType = temp[1];
                CumulativeApplicationReceived = temp[2];
                CumulativeApplicationApproved = temp[3];
                CumulativeApplicationRejected = temp[4];
                BorrowersOnIbr = temp[5];
                TotalProtfolio = temp[6];
                RepaymentPortfolio = temp[7];
            }
        }
    }
}
