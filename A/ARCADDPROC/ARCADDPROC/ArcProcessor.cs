using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ARCADDPROCESSING
{
    public class ArcProcessor
    {
        public ReflectionInterface RI { get; set; }
        private ProcessLogRun LogRun { get; set; }
        public BatchProcessingHelper LoginInfo { get; set; }
        private DataAccess DA { get; set; }

        public ArcProcessor(ProcessLogRun logRun, string scriptId, DataAccess da)
        {
            LogRun = logRun;
            RI = new ReflectionInterface();
            string loginType = "BatchUheaa";//Sets the login type 
            LoginInfo = BatchProcessingLoginHelper.Login(LogRun, RI, scriptId, loginType);
            RI.LogRun = LogRun;
            DA = da;
        }

        private ArcRecord GetNextArc()
        {
            try
            {
                ArcRecord arc = DA.GetNextArc();
                if (arc == null)
                    return null;
                if (arc.ArcAddProcessingId > 0) //Get the LoanPrograms and/or LoanSequences for the arc.
                {
                    arc.LoanPrograms = DA.GetLoanPgms(arc.ArcAddProcessingId);
                    arc.LoanSequences = DA.GetLoanSeqs(arc.ArcAddProcessingId);
                }

                return arc;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Loops through all available arcs, opens a connection to get the arc from the database
        /// </summary>
        public void Process(int id)
        {
            ArcRecord arc = GetNextArc();

            while (arc != null) //Keep looping until there are no more arcs in the database
            {
                Console.WriteLine($"{DateTime.Now}: {id}; ArcAddProcesingId {arc.ArcAddProcessingId}.");
                if (!AddComment(arc))
                    break;

                arc = GetNextArc();
            }

            BatchProcessingHelper.CloseConnection(LoginInfo);
            Console.WriteLine($"{DateTime.Now}: {id} finished.");
            try
            {
                RI.CloseSession();
            }
            catch (COMException)
            {
                Console.Write($"{DateTime.Now}: Session {id} already closed.");
            }
            LoginInfo = null;
        }


        /// <summary>
        /// Add the comment to the session using RI.
        /// </summary>
        /// <returns>True: RI is still active, False: RI is no longer active</returns>
        private bool AddComment(ArcRecord arc)
        {
            arc.Trim();
            return ProcessCompass(arc); //Will return false if there is a fatal error with the session, otherwise returns true to show thread is still active
        }

        private bool ProcessCompass(ArcRecord arc)
        {
            bool processed = false;
            try
            {
                switch ((ArcData.ArcType)arc.ArcTypeId)
                {
                    case ArcData.ArcType.Atd22AllLoans:
                        processed = RI.Atd22AllLoans(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.ScriptId, false, arc.RegardsCode, arc.RegardsTo, arc.ProcessFrom, arc.ProcessTo, arc.NeededBy, arc.ProcessingAttempts > 1);
                        break;
                    case ArcData.ArcType.Atd22AllLoansRegards:
                        processed = RI.Atd22AllLoans(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.ScriptId, false, arc.RegardsCode, arc.RegardsTo, arc.ProcessFrom, arc.ProcessTo, arc.NeededBy, arc.ProcessingAttempts > 1);
                        break;
                    case ArcData.ArcType.Atd22ByBalance:
                        processed = RI.Atd22ByBalance(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.ScriptId, false, arc.IsReference, arc.IsEndorser, arc.ProcessingAttempts > 1);
                        break;
                    case ArcData.ArcType.Atd22ByLoan:
                        processed = RI.Atd22ByLoan(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.LoanSequences, arc.ScriptId, false, arc.RegardsCode, arc.RegardsTo, arc.ProcessFrom, arc.ProcessTo, arc.NeededBy, arc.ProcessingAttempts > 1);
                        break;
                    case ArcData.ArcType.Atd22ByLoanProgram:
                        processed = RI.Atd22ByLoanProgram(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.ScriptId, false, arc.ProcessingAttempts > 1, arc.LoanPrograms.ToArray());
                        break;
                    case ArcData.ArcType.Atd22ByLoanRegards:
                        processed = RI.Atd22ByLoan(arc.AccountNumber, arc.Arc, arc.Comment, arc.RecipientId, arc.LoanSequences, arc.ScriptId, false, arc.RegardsCode, arc.RegardsTo, arc.ProcessFrom, arc.ProcessTo, arc.NeededBy, arc.ProcessingAttempts > 1);
                        break;
                    default:
                        string message = string.Format("The arc type {0} is not recognized, Borrower: {1}, ArcAddId: {2}", arc.ArcTypeId, arc.AccountNumber, arc.ArcAddProcessingId);
                        int notificationId = LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        DA.AddProcessLogsMapping(arc.ArcAddProcessingId, LogRun.ProcessLogId, notificationId);
                        processed = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Error adding {0} arc to borrower: {1}, ScriptId: {2}, ArcAddId: {3}  Session Message {4} UserId: {5}", arc.Arc, arc.AccountNumber, arc.ScriptId, arc.ArcAddProcessingId, RI.Message, LoginInfo.UserName);
                int notificationId = LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                DA.AddProcessLogsMapping(arc.ArcAddProcessingId, LogRun.ProcessLogId, notificationId);
                return false; //Return false if RI is not accessible to end the thread
            }

            if (RI.MessageCode.IsIn("02131", "01029"))
            {
                for (int trys = 0; trys < 10; trys++)
                {
                    Thread.Sleep(1000);//Adding a sleep for these error message because the session cannot handle adding multiple comments on borrowers accounts within seconds
                    RI.Hit(ReflectionInterface.Key.Enter);
                    processed = RI.CheckForText(23, 2, "02860", "02114");
                    if (processed)
                        break;
                }
            }

            if (!processed)
            {
                var notificationSeverity = NotificationSeverityType.Critical;
                var arcResult = RI.ArcErrorInfo.Where(p => p.ErrorCode == RI.MessageCode && p.ProcessingAttempts == arc.ProcessingAttempts).SingleOrDefault();
                if (arcResult != null)
                    notificationSeverity = arcResult.NotificationSeverityTypeId;
                string message = string.Format("Error adding {0} arc to borrower: {1}, ScriptId: {2}, ArcAddId: {3}, Session Message {4} UserId: {5}", arc.Arc, arc.AccountNumber, arc.ScriptId, arc.ArcAddProcessingId, RI.Message, LoginInfo.UserName);
                int notificationId = LogRun.AddNotification(message, NotificationType.ErrorReport, notificationSeverity);
                DA.AddProcessLogsMapping(arc.ArcAddProcessingId, LogRun.ProcessLogId, notificationId);
                DA.RequeueARC(arc.ArcAddProcessingId, arcResult != null ? arcResult.RequeueHours : 4);
            }
            else if (processed)
            {
                RI.Hit(ReflectionInterface.Key.F4);
                string seqNumber = RI.GetText(5, 18, 15);
                if (!arc.ResponseCode.IsNullOrEmpty())
                    AddResponseCode(arc, LoginInfo.UserName, seqNumber);

                DA.UpdateLN_ATY_SEQ(arc.ArcAddProcessingId, seqNumber.ToInt());
            }

            return true;
        }

        private void AddResponseCode(ArcRecord arc, string userId, string activitySeq)
        {
            RI.FastPath("TX3Z/CTD2A" + arc.AccountNumber);
            RI.PutText(12, 65, activitySeq, ReflectionInterface.Key.Enter);

            if (RI.ScreenCode != "TDX2D" && RI.ScreenCode != "TDX2C")
            {
                string message = string.Format("Unable to add the response code {0} to ARC {1} ArcAddId {5} for Account {2} Session Message {3} User Id {4}", arc.ResponseCode, arc.Arc, arc.AccountNumber, RI.MessageCode, userId, arc.ArcAddProcessingId);
                int notificationId = LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.AddProcessLogsMapping(arc.ArcAddProcessingId, LogRun.ProcessLogId, notificationId);
                return;
            }

            if (RI.ScreenCode == "TDX2C")
                RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);

            while (RI.MessageCode != "90007")
            {
                if (RI.CheckForText(15, 2, "__"))
                {
                    RI.PutText(15, 2, arc.ResponseCode, ReflectionInterface.Key.Enter);

                    if (RI.MessageCode != "01005")
                    {
                        string message = string.Format("Unable to add the response code {0} to ARC {1} ArcAddId {5} for Account {2} Session Message {3} User Id {4}", arc.ResponseCode, arc.Arc, arc.AccountNumber, RI.Message, userId, arc.ArcAddProcessingId);
                        int notificationId = LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        DA.AddProcessLogsMapping(arc.ArcAddProcessingId, LogRun.ProcessLogId, notificationId);
                        break;
                    }
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
        }
    }
}