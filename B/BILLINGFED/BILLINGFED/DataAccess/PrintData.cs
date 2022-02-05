using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace BILLINGFED
{
    public class PrintData
    {
        public Borrower Bor { get; set; }
        public EcorrData EcorrInfo { get; set; }
        public string ScriptId { get; set; }
        public string UserId { get; set; }
        public int ReportNumber { get; set; }
        public string LetterId { get; set; }
        public string Directory { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public PrintData(Borrower bor, EcorrData ecorr, string scriptId, string userId, int reportNumber, string letterId, string dir, ProcessLogRun logRun)
        {
            Bor = bor;
            EcorrInfo = ecorr;
            ScriptId = scriptId;
            UserId = userId;
            ReportNumber = reportNumber;
            LetterId = letterId;
            Directory = dir;
            LogRun = logRun;
        }
    }
}
