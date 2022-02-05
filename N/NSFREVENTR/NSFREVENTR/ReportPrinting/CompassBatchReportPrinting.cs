using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Data;
using System.IO;

namespace NSFREVENTR
{
    public class CompassBatchReportPrinting : BatchReportPrinting
    {

        public CompassBatchReportPrinting(bool testMode)
            : base(testMode)
        {
            _testModeResults = TestMode(NSFReversalEntry.COMPASS_DIR);
        }

        /// <summary>
        /// Generates needed file and prints them.
        /// </summary>
        public override void GenerateAndPrintReports()
        {
            //process posting file first
            string postingFile = string.Format("{0}{1}", _testModeResults.DocFolder, NSFReversalEntry.COMPASS_NSF_ENTRY_FILE);
            DataTable postingTable;
            if (File.Exists(postingFile)) //only try and process if the file exists
            {
                Dictionary<string, Type> tableDictionary = new Dictionary<string, Type>();
                tableDictionary.Add("SSN", typeof(string));
                tableDictionary.Add("BatchCode", typeof(string));
                tableDictionary.Add("EffectiveDate", typeof(string));
                tableDictionary.Add("Batch", typeof(string));
                tableDictionary.Add("PaymentAmount", typeof(double));
                tableDictionary.Add("ACH", typeof(string));
                tableDictionary.Add("UserID", typeof(string));
                tableDictionary.Add("Reason", typeof(string));
                tableDictionary.Add("TransactionType", typeof(string));
                postingTable = CreateDataTableFromFile(postingFile, false, tableDictionary);
                ReportPrinting.CompassPostingReport rpt = new ReportPrinting.CompassPostingReport();
                rpt.SetDataSource(postingTable);
                rpt.PrintToPrinter(1, true, 1, -1);
            }
            string nonPostingFile = string.Format("{0}{1}", _testModeResults.DocFolder, NSFReversalEntry.COMPASS_NSF_NON_POSTING_ENTRY_FILE);
            if (File.Exists(nonPostingFile)) //only try and process if the file exists
            {
                Dictionary<string, Type> tableDictionary1 = new Dictionary<string, Type>();
                tableDictionary1.Add("SSN", typeof(string));
                tableDictionary1.Add("Name", typeof(string));
                tableDictionary1.Add("LoanSeq", typeof(string));
                tableDictionary1.Add("EffectiveDate", typeof(string));
                tableDictionary1.Add("PaymentAmount", typeof(double));
                tableDictionary1.Add("Reason", typeof(string));
                postingTable = CreateDataTableFromFile(nonPostingFile, false, tableDictionary1);
                ReportPrinting.CompassNonPostingReport rpt1 = new NSFREVENTR.ReportPrinting.CompassNonPostingReport();
                rpt1.SetDataSource(postingTable);
                rpt1.PrintToPrinter(1, true, 1, -1);
                rpt1.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, string.Format("{0}COMPASS NSF NON POSTING REPORT {1}.xls", _testModeResults.DocFolder, DateTime.Now.ToString("MM-dd-yy hhmmss")));
                System.Windows.Forms.MessageBox.Show(string.Format("An Excel copy of the Compass NSF Non-Posting report can be found {0}.", _testModeResults.DocFolder), "Excel Report", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

    }
}
