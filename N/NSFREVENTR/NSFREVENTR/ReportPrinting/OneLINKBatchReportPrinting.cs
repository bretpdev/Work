using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace NSFREVENTR
{
    class OneLINKBatchReportPrinting : BatchReportPrinting
    {

        public OneLINKBatchReportPrinting(bool testMode)
            : base(testMode)
        {
            _testModeResults = TestMode(NSFReversalEntry.ONELINK_DIR);
        }

        /// <summary>
        /// Generates needed file and prints them.
        /// </summary>
        public override void GenerateAndPrintReports()
        {
            //process posting file
            string postingFile = string.Format("{0}{1}", _testModeResults.DocFolder, NSFReversalEntry.ONELINK_NSF_ENTRY_FILE);
            if (File.Exists(postingFile))
            {
                DataTable _postingTable;
                Dictionary<string, Type> tableDictionary = new Dictionary<string, Type>();
                tableDictionary.Add("AccountNumber", typeof(string));
                tableDictionary.Add("LastName", typeof(string));
                tableDictionary.Add("EffectiveDate", typeof(string));
                tableDictionary.Add("PostingDate", typeof(string));
                tableDictionary.Add("Amount", typeof(double));
                tableDictionary.Add("BatchType", typeof(string));
                tableDictionary.Add("PaymentType", typeof(string));
                tableDictionary.Add("ReasonReturned", typeof(string));
                tableDictionary.Add("DontKnowWhatThisIs", typeof(string));
                _postingTable = CreateDataTableFromFile(postingFile, false, tableDictionary);
                ReportPrinting.OneLINKPostingReport rpt = new ReportPrinting.OneLINKPostingReport();
                rpt.SetDataSource(_postingTable);
                rpt.PrintToPrinter(1, true, 1, -1);
            }
        }
    }
}
