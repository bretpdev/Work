using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACHRIRDF
{
    public class QueueRecord
    {
        public int ProcessQueueId { get; set; }
        public string Report { get; set; }
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public int LoanSequence { get; set; }
        public string OwnerCode { get; set; }
        public bool VariableRate { get; set; }
        public DateTime? DefermentOrForbearanceOriginalBeginDate { get; set; }
        public DateTime? DefermentOrForbearanceOriginalEndDate { get; set; }
        public DateTime? DefermentOrForbearanceBeginDate { get; set; }
        public DateTime? DefermentOrForbearanceEndDate { get; set; }
        public decimal? OldRate { get; set; }
        //Properties are used to determine partially covered R/M/P rates that need to be manually reviewed
        public bool? HasPartialReducedRate { get; set; }

        //These properties are populated at runtime
        public decimal? NewRate { get; set; }
        public string NewRateType { get; set; }

        public QueueRecord()
        {

        }

        public QueueRecord(int ProcessQueueId, string Report, string Ssn, string AccountNumber, int LoanSequence, string OwnerCode, DateTime? DefermentOrForbearanceBeginDate, DateTime? DefermentOrForbearanceEndDate, bool VariableRate)
        {
            this.ProcessQueueId = ProcessQueueId;
            this.Report = Report;
            this.Ssn = Ssn;
            this.AccountNumber = AccountNumber;
            this.LoanSequence = LoanSequence;
            this.OwnerCode = OwnerCode;
            this.DefermentOrForbearanceBeginDate = DefermentOrForbearanceBeginDate;
            this.DefermentOrForbearanceEndDate = DefermentOrForbearanceEndDate;
            this.VariableRate = VariableRate;
        }
    }
}
