using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;

namespace NSLDSCONSO
{
    class BorrowerUploader
    {
        ProcessLogRun plr;
        DataAccess da;
        private Action<string> logItem;
        private Action<string, Exception, NotificationSeverityType> logError;
        public BorrowerUploader(ProcessLogRun plr, DataAccess da, Action<string> logItem, Action<string, Exception, NotificationSeverityType> logError)
        {
            this.plr = plr;
            this.da = da;
            this.logItem = logItem;
            this.logError = logError;
        }
        public void Process(DataHistoryParser parser)
        {
            int updateEvery = 25;
            int badUploadCount = 0;
            var updateCount = new Action(() =>
            {
                if (badUploadCount == 0)
                    logItem(string.Format("{0} / {1} borrowers processed.", parser.ProcessedBorrowers.Count, parser.TotalBorrowerCount));
                else
                    logItem(string.Format("{0} / {1} borrowers processed.  {2} borrower(s) unable to process.", parser.ProcessedBorrowers.Count - badUploadCount, parser.TotalBorrowerCount, badUploadCount));
            });
            using (var compass = new CompassHelper(plr))
            {
                if (!compass.Login())
                {
                    logItem("Unable to find valid compass login.");
                }
                else
                {
                    foreach (var borrower in parser.ParseFile())
                    {
                        bool isMissingNsldsLabel = borrower.UnderlyingLoans.Any(o => string.IsNullOrWhiteSpace(o.NsldsLabel));
                        if (isMissingNsldsLabel && !compass.RequestBorrower(borrower))
                        {
                            logItem("Unable to request NSLDS LIS for " + borrower.Ssn + ". " + compass.RI.GetText(22, 2, 80));
                            badUploadCount++;
                            continue;
                        }
                        //upload successfully parsed borrower
                        var success = da.UploadBorrower(parser.DataLoadRunId, borrower);
                        if (!success)
                        {
                            logError("Error uploading borrower " + borrower.Ssn, null, NotificationSeverityType.Warning);
                            badUploadCount++;
                        }
                        if (parser.ProcessedBorrowers.Count % updateEvery == 0)
                            updateCount();
                    }
                }
            }
            updateCount();
            int remaining = parser.TotalBorrowerCount - parser.ProcessedBorrowers.Count + badUploadCount;
            if (remaining == 0)
                logItem("Processing Complete.  All borrowers processed successfully.");
            else
                logItem(string.Format("Processing Complete: {0} borrowers unable to process successfully.", remaining));
        }
    }
}
