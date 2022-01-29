using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace FINALBFED
{
    public class FinalBill
    {
        private DataAccess DA { get; set; }
        private ProcessLogRun PLR { get; set; }

        public FinalBill(ProcessLogRun plr)
        {
            PLR = plr;
            DA = new DataAccess(PLR);
        }

        /// <summary>
        /// Gets all records and processes them.
        /// </summary>
        /// <returns>0 if successful 1 if not</returns>
        public int Process()
        {
            Console.WriteLine("Adding coborrower records for pending borrower records.");
            int rowsAdded = DA.AddCoborrowerRecords();
            Console.WriteLine(string.Format("{0} records added into coborrower table.", rowsAdded));

            List<LT20Data> unprocessedRecs = GetRecords();

            foreach (LT20Data letter in unprocessedRecs)
            {
                if (letter.InvalidLoanStatus)
                    DA.InactivateLetter(letter, 2);
                else
                    PrintLetter(letter);
            }
            return 0;
        }

        private List<LT20Data> GetRecords()
        {
            List<LT20Data> unprocessedRecords = DA.GetUprocessedRecords();
            Parallel.ForEach(unprocessedRecords, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, record =>
            {
                record.RN_SEQ_REC_PRC = DA.GetLetterRecSeq(record);
                record.RM_DSC_LTR_PRC = record.RM_DSC_LTR_PRC.Trim();
                if (record.EndorsersSsn.IsPopulated() || record.Recipient.IsPopulated())
                    record.OnEcorr = false;
                Console.WriteLine("Getting letter Data for Account {0} LetterId {1}", record.DF_SPE_ACC_ID, record.RM_DSC_LTR_PRC);
            });
            return unprocessedRecords;
        }

        private void PrintLetter(LT20Data record)
        {
            List<LoanInformation> loanInfo = DA.GetBorrowersLoanInfo(record);
            if (loanInfo == null || loanInfo.Count() == 0)
            {
                DA.InactivateLetter(record, 2);
                PLR.AddNotification(string.Format("Loan information not available {0}, letter {1}.", record.DF_SPE_ACC_ID, record.RM_DSC_LTR_PRC), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }

            new PdfGenerator(record, loanInfo, DA, PLR).CreatePdf();
        }
    }
}
