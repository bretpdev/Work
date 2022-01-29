using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace BBONTMDISQ
{
    internal class DisqualificationProcess
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        private ReflectionInterface RI { get; set; }
        private BatchProcessingHelper LoginInfo { get; set; }
        private string ScriptId { get; set; }

        public DisqualificationProcess(ProcessLogRun logRun, string scriptId)
        {
            LogRun = logRun;
            ScriptId = scriptId;
            DA = new DataAccess(logRun);
            RI = new ReflectionInterface();
            LoginInfo = BatchProcessingLoginHelper.Login(logRun, RI, ScriptId, "BatchUheaa");
            RI.LogRun = LogRun;
        }

        internal int Process()
        {
            if (LoginInfo == null)
            {
                Console.WriteLine("Failed to log into session.  Try again when more batch IDs are available.");
                return 1;
            }
            else
                Console.WriteLine($"Logging into session using user: {LoginInfo.LoginId}");

            List<DisqualificationRecord> records = DA.GetDisqualificationRecords();
            if (records.Count == 0)
            {
                string message = "No records found to process.";
                LogRun.AddNotification(message, NotificationType.EndOfJob, NotificationSeverityType.Informational);
                Console.WriteLine(message);
                return 0;
            }
            
            var borrowerGroups = records.GroupBy(o => o.Ssn);
            List<DisqualificationRecord> erroredRecords = new List<DisqualificationRecord>();
            List<DisqualificationRecord> arcRecords = new List<DisqualificationRecord>();
            foreach (var borrower in borrowerGroups)
            {
                foreach (DisqualificationRecord record in borrower)
                {
                    if(!MarkDisqualified(record))
                        erroredRecords.Add(record);
                }
                
                arcRecords = borrower.Except(erroredRecords).ToList();
                if(arcRecords.Count != 0)
                    DropArc(arcRecords);

                erroredRecords.Clear();
                arcRecords.Clear();
            }

            LogRun.LogEnd();
            RI.CloseSession();
            return 0;
        }

        private void DropArc(List<DisqualificationRecord> arcRecords)
        {
            ArcAddResults result = null;
            string comment = "4th late bill occured. Disq loans:";
            List<int> loanSequences = new List<int>();

            foreach (DisqualificationRecord record in arcRecords)
            {
                comment += " " + record.LoanSequence + ",";
                loanSequences.Add(record.LoanSequence.ToInt());
            }
            comment.TrimRight(",");

            ArcData arcData = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = arcRecords.FirstOrDefault().Ssn,
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                LoanPrograms = null,
                LoanSequences = loanSequences,
                Comment = comment,
                IsEndorser = false,
                RecipientId = arcRecords.FirstOrDefault().Ssn,
                ScriptId = ScriptId,
                Arc = "U48MD",
                ResponseCode = null,
                ActivityType = null,
                ActivityContact = null
            };

            result = arcData.AddArc();
            if (!result.ArcAdded)
                LogRun.AddNotification($"Unable to add Arc for Ssn: {arcRecords.FirstOrDefault().Ssn}, message: {comment}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }

        internal bool MarkDisqualified(DisqualificationRecord record)
        {
            string loanSeq;
            string benefitType;
            string selection = "";
            RI.FastPath("TX3Z/CTSDS" + record.Ssn);
            if (RI.CheckForText(1, 75, "TSXDU")) //Selection screen
            {
                for(int row = 8; row <= 19 && !RI.CheckForMessage("90007"); row++)
                {
                    benefitType = RI.GetText(row, 69, 3);
                    loanSeq = RI.GetText(row, 8, 3);
                    if (record.LoanSequence.ToInt() == loanSeq.ToInt() && benefitType.IsIn("U48","N48")) 
                    {
                        selection = RI.GetText(row, 4, 2);
                        break;
                    }

                    if (row == 19 && selection.IsNullOrEmpty()) //Next screen
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 7;
                    }
                }

                if (selection.IsNullOrEmpty())
                {
                    LogRun.AddNotification($"Record not found in session. SSN: {record.Ssn}, Loan Sequence: {record.LoanSequence}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }
                else
                    RI.PutText(21, 18, selection, ReflectionInterface.Key.Enter); //Get to selection screen
            }

            if(RI.CheckForText(1, 72, "TSXDV")) //Target screen
            {
                RI.PutText(11, 16, "X"); //Mark as ineligible
                RI.PutText(13, 44, "02"); //delq bill
                RI.PutText(13, 16, record.DisqualificationDate.ToString("MMddyy"), ReflectionInterface.Key.Enter, true);
                RI.Hit(ReflectionInterface.Key.F6);
                RI.PutText(21, 11, "48 On Time Payment Disqualification due to the human error provision.  Borr is only allowed three late payments between 15 and 30 days delq.");
                RI.Hit(ReflectionInterface.Key.F6);

                if (!RI.CheckForMessage("01005") && !RI.CheckForMessage("01003"))
                {
                    LogRun.AddNotification($"Failed to update record for SSN: {record.Ssn}, Loan Sequence: {record.LoanSequence}, Disqualification Date: {record.DisqualificationDate.ToString("MMddyy")}. Session Error message: {RI.Message}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }
                else if(RI.CheckForMessage("01003"))
                {
                    LogRun.AddNotification($"Failed to update record for SSN: {record.Ssn}, Loan Sequence: {record.LoanSequence}, Disqualification Date: {record.DisqualificationDate.ToString("MMddyy")}. Session Error message: {RI.Message}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return false;
                }
                else
                {
                    LogRun.AddNotification($"Successfully updated record for SSN: {record.Ssn}, Loan Sequence: {record.LoanSequence}", NotificationType.EndOfJob, NotificationSeverityType.Informational);
                    return true;
                }
            }
            return false;
        }
    }
}