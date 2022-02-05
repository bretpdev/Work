using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace UHECORPRT
{
    partial class BatchPrinting
    {
        private void AddArcs(ScriptData file, PrintProcessingData data)
        {
           ArcAddResults result = null;
            foreach (ArcInformation arc in file.Arcs)
            {
                List<int> loanSeq = null;
                List<string> loanPgms = null;
                if (arc.Type == ArcData.ArcType.Atd22ByLoan)
                {
                    loanSeq = ProcessByLoan(file, data, arc, loanSeq);
                }
                else if (arc.Type == ArcData.ArcType.Atd22ByLoanProgram)
                {
                    loanPgms = ProcessByLoanPgm(file, data, arc, loanPgms);
                }

                string accountNumber = data.AccountNumber;
                string recipientId = "";
                if(file.IsEndorser)
                {
                    recipientId = DA.GetSsnFromAcctNumber(data.AccountNumber);
                    accountNumber = DA.GetAccountNumberFromSsn(data.LetterData.SplitAndRemoveQuotes(",")[file.EndorsersBorrowerSSNIndex.Value + (file.BarcodeOffset.HasValue ? file.BarcodeOffset.Value : 0)]);
                }

                ArcData arcData = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    AccountNumber = accountNumber,
                    ArcTypeSelected = arc.ActivityType == null ? arc.Type: ArcData.ArcType.OneLINK,
                    LoanPrograms = loanPgms,
                    LoanSequences = loanSeq,
                    Comment = arc.Comment,
                    IsEndorser = file.IsEndorser,
                    RecipientId = recipientId,
                    ScriptId = file.ScriptID,
                    Arc = arc.Arc,
                    ResponseCode = arc.ResponseCode,
                    ActivityType = arc.ActivityType,
                    ActivityContact = arc.ActivityContact
                };

                result = arcData.AddArc();
                if (!result.ArcAdded)
                { 
                    ProcessLogger.AddNotification(Program.PL.ProcessLogId, string.Format("Unable to add Arc for PrintProcessingId:{0} Arc:{1} AccountNUmber:{2} RecipientId:{3} Comment:{4}", 
                        data.PrintProcessingId, arc.Arc, accountNumber, recipientId, arc.Comment), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }                
            }

            if (result.ArcAdded)
                data.MarkArcAdded(DA, result.ArcAddProcessingId);
        }

        private static List<string> ProcessByLoanPgm(ScriptData file, PrintProcessingData data, ArcInformation arc, List<string> loanPgms)
        {
            loanPgms = new List<string>();
            foreach (string arcHeader in arc.HeaderNames)
            {
                string pgm = data.LetterData.SplitAndRemoveQuotes(",")[file.FileHeader.SplitAndRemoveQuotes(",").IndexOf(arcHeader)].Trim();
                if (pgm.IsPopulated())
                    loanPgms.Add(pgm);
            }
            return loanPgms;
        }

        private static List<int> ProcessByLoan(ScriptData file, PrintProcessingData data, ArcInformation arc, List<int> loanSeq)
        {
            loanSeq = new List<int>();
            foreach (string arcHeader in arc.HeaderNames)
            {
                int? seq = data.LetterData.SplitAndRemoveQuotes(",")[file.FileHeader.SplitAndRemoveQuotes(",").IndexOf(arcHeader)].ToIntNullable();
                if (seq.HasValue)
                    loanSeq.Add(seq.Value);
            }
            return loanSeq;
        }
    }
}
