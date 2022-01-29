using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ARCADDFED
{
    public class CreateArc
    {
        public ReflectionInterface RI { get; set; }
        public BatchProcessingHelper Batch { get; set; }
        public List<ArcRecord> Arcs { get; set; }
        public ProcessLogData LogData { get; set; }

        public CreateArc(BatchProcessingHelper batch, List<ArcRecord> arcs, ProcessLogData logData)
        {
            RI = new ReflectionInterface();
            Batch = batch;
            Arcs = arcs;
            LogData = logData;
        }

        public void ProcessArcs()
        {
            if (RI.Login(Batch.UserName, Batch.Password))
            {
                foreach (ArcRecord arc in Arcs)
                {
                    bool added = false;
                    switch ((ArcAddHelper.ArcType)arc.ArcTypeId)
                    {
                        case ArcAddHelper.ArcType.Atd22AllLoans:
                            added = RI.Atd22AllLoans(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.ScriptId, arc.PauseForManualComments);
                            break;
                        case ArcAddHelper.ArcType.Atd22AllLoansRegards:
                            added = RI.Atd22AllLoans(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.ScriptId, arc.PauseForManualComments, arc.RegardsCode, arc.RegardsTo, arc.ProcessFrom, arc.ProcessTo, arc.NeededBy);
                            break;
                        case ArcAddHelper.ArcType.Atd22ByBalance:
                            added = RI.Atd22ByBalance(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.ScriptId, arc.PauseForManualComments, arc.IsReference);
                            break;
                        case ArcAddHelper.ArcType.Atd22ByLoan:
                            added = RI.Atd22ByLoan(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.LoanSequences, arc.ScriptId, arc.PauseForManualComments);
                            break;
                        case ArcAddHelper.ArcType.Atd22ByLoanProgram:
                            added = RI.Atd22ByLoanProgram(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.ScriptId, arc.PauseForManualComments, arc.LoanPrograms.ToArray());
                            break;
                        case ArcAddHelper.ArcType.Atd22ByLoanRegards:
                            added = RI.Atd22ByLoan(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.LoanSequences, arc.ScriptId, arc.PauseForManualComments, arc.RegardsCode, arc.RegardsTo, arc.ProcessFrom, arc.ProcessTo, arc.NeededBy);
                            break;
                        default:
                            string message = string.Format("The Arc Type is not available in the list of types: {0}", (ArcAddHelper.ArcType)arc.ArcTypeId);
                            ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            break;
                    }
                    if (added)
                        DataAccess.UpdateRecordComplete(arc.ArcAddId);
                    else
                        AddError(arc);
                }
            }
        }

        /// <summary>
        /// Add error to process logger.
        /// </summary>
        private void AddError(ArcRecord arc)
        {
            string message = string.Format("Error adding arc for Borrower: {0}, Arc: {1}, ScriptId{2}, ProcessOn: {3}", arc.AccountNumber, arc.Arc, arc.ScriptId, DateTime.Now.ToShortDateString());
            ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }
    }
}