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

namespace ONELINKAAP
{
    public class ArcProcessor
    {
        public ReflectionInterface RI { get; set; }
        private ProcessLogRun LogRun { get; set; }
        public BatchProcessingHelper LoginInfo { get; set; }
        private readonly DataAccessHelper.Region Region;
        private DataAccess DA { get; set; }

        public ArcProcessor(ProcessLogRun logRun, DataAccessHelper.Region region, string scriptId, DataAccess da)
        {
            LogRun = logRun;
            RI = new ReflectionInterface();
            string loginType = region == DataAccessHelper.Region.CornerStone ? "BatchCornerstone" : "BatchUheaa";//Sets the login type 
            LoginInfo = BatchProcessingLoginHelper.Login(LogRun, RI, scriptId, loginType);
            RI.LogRun = LogRun;
            Region = region;
            DataAccessHelper.CurrentRegion = region;
            DA = da;
        }

        private ArcRecord GetNextArc()
        {
            try
            {
                ArcRecord arc = DA.GetNextArc();
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
                Console.WriteLine("{3}: {0} Thread {1}; ArcAddProcesingId {2};", Region, id, arc.ArcAddProcessingId, DateTime.Now.ToString());
                if (!AddComment(arc))
                    break;

                arc = GetNextArc();
            }

            BatchProcessingHelper.CloseConnection(LoginInfo);
            Console.WriteLine("{0}: {1} Thread {2} finished", DateTime.Now.ToString(), Region, id);
            try
            {
                RI.CloseSession();
            }
            catch (COMException)
            {
                Console.Write("{0}: {1} Session {2} already closed", DateTime.Now.ToString(), Region, id);
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
            bool processed = true;//Processed will get set to false if there is a fatal error with the session

            processed = ProcessOneLink(arc);

            return processed; //Return true to show thread is still active
        }

        private bool ProcessOneLink(ArcRecord arc)
        {
            bool processed = false;
            string ssn = null;
            try
            {
                ssn = arc.AccountNumber;
                if (arc.AccountNumber.Trim().Length != 9)
                {
                    //We need to get the SSN, as thats all LP50 supports.
                    ssn = DA.GetSSNFromAccoutNumber(arc.AccountNumber);
                }
                if(arc.RecipientId.IsPopulated() && arc.ActivityType != null && arc.ActivityContact != null)
                    processed = RI.AppendCommentInLP50(arc.RecipientId, arc.ActivityType, arc.ActivityContact, arc.Arc, arc.Comment, arc.ScriptId);
                else if (ssn.IsPopulated())
                    processed = RI.AppendCommentInLP50(ssn, arc.ActivityType, arc.ActivityContact, arc.Arc, arc.Comment, arc.ScriptId);

            }
            catch (Exception ex)
            {
                string message = string.Format("Error adding {0} arc to ONELink for borrower: {1}, ScriptId: {2}, ArcAddId: {3}  Session Message {4} UserId: {5} RecipientId: {6}", arc.Arc, arc.AccountNumber, arc.ScriptId, arc.ArcAddProcessingId, RI.GetText(22, 3, 76), LoginInfo.UserName, arc.RecipientId ?? ssn ?? "");
                int notificationId = LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                DA.AddProcessLogsMapping(arc.ArcAddProcessingId, LogRun.ProcessLogId, notificationId);
                return false; //Return false if RI is not accessible to end the thread
            }

            if (!processed)
            {
                var notificationSeverity = NotificationSeverityType.Critical;
                var arcResult = RI.ArcErrorInfo.Where(p => p.ErrorCode == RI.GetText(22, 3, 5) && p.ProcessingAttempts == arc.ProcessingAttempts).SingleOrDefault();
                if (arcResult != null)
                    notificationSeverity = arcResult.NotificationSeverityTypeId;
                string message = string.Format("Error adding {0} arc to borrower: {1}, ScriptId: {2}, ArcAddId: {3}, Session Message {4} UserId: {5} RecipientId: {6}", arc.Arc, arc.AccountNumber, arc.ScriptId, arc.ArcAddProcessingId, RI.GetText(22, 3, 76), LoginInfo.UserName, arc.RecipientId ?? ssn ?? "");
                int notificationId = LogRun.AddNotification(message, NotificationType.ErrorReport, notificationSeverity);
                DA.AddProcessLogsMapping(arc.ArcAddProcessingId, LogRun.ProcessLogId, notificationId);
                DA.RequeueARC(arc.ArcAddProcessingId, arcResult != null ? arcResult.RequeueHours : 4);
            }
            else//this is a compass only thing so we are setting all values to 0
                DA.UpdateLN_ATY_SEQ(arc.ArcAddProcessingId, 0);

            return true;//This is returning that the thread is alive still
        }
    }
}