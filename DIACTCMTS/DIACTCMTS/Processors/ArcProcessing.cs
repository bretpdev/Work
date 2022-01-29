using System;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DIACTCMTS
{
    class ArcProcessing
    {
        private ProcessLogRun LogRun { get; set; }
        private ReaderWriterLockSlim ThreadLock { get; set; }
        private DataAccess DA { get; set; }

        public ArcProcessing(ProcessLogRun logRun, ReaderWriterLockSlim threadLock, DataAccess da)
        {
            LogRun = logRun;
            ThreadLock = threadLock;
            DA = da;
        }

        /// <summary>
        /// Starts the thread up to process
        /// </summary>
        /// <param name="threadNumber"></param>
        public void Process(int threadNumber)
        {
            NobleCallHistoryData item = GetNextItem();
            while (item != null)
            {
                ArcCommentResponse data = DA.GetDataFromDisposition(item.DispositionCode, item.AccountIdentifier);
                Console.WriteLine($"Thread {threadNumber} processing borrower {item.AccountIdentifier}");
                AddTheArc(item, data);
                item = GetNextItem();
            }
        }

        private NobleCallHistoryData GetNextItem()
        {
            ThreadLock.EnterWriteLock();
            NobleCallHistoryData item = null;
            if (UheaaProcessing.Data.Count != 0)
                item = UheaaProcessing.Data.Dequeue();

            ThreadLock.ExitWriteLock();
            return item;
        }

        private void AddTheArc(NobleCallHistoryData call, ArcCommentResponse data)
        {
            string comment = call.CoborrowerAccountNumber.IsPopulated() ?
                $"NobleCallHistoryId:{call.NobleCallHistoryId}; Agent:{(call.AgentId ?? @"N/A ")}; CallResult:{data.Comment.Trim()}; CallDate:{call.ActivityDate} CoborrowerAccountNumber:{call.CoborrowerAccountNumber}" : 
                $"NobleCallHistoryId:{call.NobleCallHistoryId}; Agent:{(call.AgentId ?? @"N/A ")}; CallResult:{data.Comment.Trim()}; CallDate:{call.ActivityDate}";
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = call.AccountIdentifier,
                Arc = data.Arc,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                LoanPrograms = null,
                LoanSequences = null,
                NeedBy = null,
                ProcessFrom = null,
                ProcessOn = null,
                ProcessTo = null,
                RecipientId = "",
                RegardsCode = null,
                RegardsTo = "",
                ScriptId = DialerActivityComments.ScriptId,
                ResponseCode = data.ResponseCode
            };
            if (arc.AccountNumber.IsPopulated() && arc.AccountNumber.IsNumeric() && arc.AccountNumber.Length <= 10) //Account number is valid
            {
                ArcAddResults result = arc.AddArc();

                if (result.ArcAdded)
                    DA.UpdateProcessedAt(result.ArcAddProcessingId, call.NobleCallHistoryId);
                else
                {
                    string message = $"Error adding arc {arc.Arc} comment '{(arc.Comment + "; " + (result.Errors.Count > 0 ? result.Errors[0] : ""))}' to ArcAdd database for {arc.AccountNumber}, script {DialerActivityComments.ScriptId}";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
                }
            }
        }
    }
}