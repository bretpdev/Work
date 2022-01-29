using System;
using System.Reflection;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using System.Linq;

namespace BBREININSC
{
    public class BBReinstate
    {
        private static string FileLoadMode { get; set; }
        private const string SCRIPTID = "BBREININSC";
        private DateTime JAN_20_2006 = new DateTime(2006, 1, 20);
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }
        public ReflectionInterface RI { get; set; }
        BatchProcessingHelper Helper { get; set; }

        public BBReinstate(ProcessLogRun plr, string[] args)
        {
            RI = new ReflectionInterface();
            PLR = plr;
            DA = new DataAccess(PLR);
            FileLoadMode = null;
            if (args.Count() > 1)
                FileLoadMode = args[1].ToUpper();
            Helper = BatchProcessingLoginHelper.Login(PLR, RI, "BBREININSC", "BatchUheaa");
            PLR.AddNotification("BBREININSC: Begin script.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
        }

        public int Run()
        {
            if (Helper == null)
            {
                RI.CloseSession();
                PLR.AddNotification($"BBREININSC: Failed to open a session with login {Helper.LoginId}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                PLR.LogEnd();
                return 1;
            }

            if (FileLoadMode == "FILELOAD")
            {
                PLR.AddNotification("BBREININSC: Load Files", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                BatchProcessingHelper.CloseConnection(Helper);
                RI.CloseSession();
                DA.LoadFiles();
                PLR.LogEnd();
                return 0;
            }
            else if (FileLoadMode == "PROCESS")
            {
                PLR.AddNotification("BBREININSC: Process Records", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                ProcessRecords();
                PLR.AddNotification("BBREININSC: Exiting, script complete.", NotificationType.EndOfJob, NotificationSeverityType.Informational);
                BatchProcessingHelper.CloseConnection(Helper);
                RI.CloseSession();
                PLR.LogEnd();
                return 0;
            }
            else
            {
                PLR.AddNotification("BBREININSC: Invalid parameters.  Must use either FILELOAD or PROCESS.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                BatchProcessingHelper.CloseConnection(Helper);
                RI.CloseSession();
                PLR.LogEnd();
                return 1;
            }
        }

        public void ProcessRecords()
        {
            List<ReinstatementRecord> records = DA.GetReinstatementRecords();
            foreach (var borrowerRecords in records.GroupBy(p => p.BF_SSN))
            {
                ProcessBorrower(borrowerRecords);
            }
        }

        private void ProcessBorrower(IGrouping<string, ReinstatementRecord> borrowerRecords)
        {
            List<ReinstatementRecord> records = borrowerRecords.ToList();
            List<ReinstatementRecord> results = new List<ReinstatementRecord>();
            ReinstatementRecord tempRecord;
            foreach (var record in records)
            {
                tempRecord = record;
                RI.FastPath("TX3ZITS26" + tempRecord.BF_SSN);
                if (RI.CheckForText(1, 72, "T1X07")) //BORROWER NOT FOUND ON SYSTEM
                {
                    PLR.AddNotification(string.Format("BBREININSC: Borrower {0} not found on system.", tempRecord.BF_SSN), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    break;
                }

                RI.FastPath("TX3ZCTS8S" + tempRecord.BF_SSN);
                if (RI.CheckForText(1, 73, "TSX8R"))
                    PLR.AddNotification(string.Format("BBREININSC: TSX8R: No Loan Info Found for borrower: {0}", tempRecord.BF_SSN), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                else if (RI.CheckForText(1, 72, "TSX8V"))// Selection or Target screen.
                    tempRecord = TS8S_Selection(tempRecord); //return the passed object with UpdateTSDS populated
                else
                    tempRecord = TS8S_Target(tempRecord);

                //tempRecord = UpdateTSDS(tempRecord);

                results.Add(tempRecord);

                DA.SetProcessedAt(tempRecord.RecordId);
            }

            if(results.Any())
                EnterTD22Comments(results);
        }

        // Messages encountered to end looping.
        private bool LoopContinueCondition()
        {
            return !RI.MessageCode.IsIn("90007", "01753", "03363");
        }

        private ReinstatementRecord TS8S_Selection(ReinstatementRecord record)
        {
            for (int row = 8; !RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"); row++)
            {
                if (RI.CheckForText(row, 8, " ") || row == 20)
                {
                    row = 7;
                    RI.Hit(ReflectionInterface.Key.F8);
                }
                else if (RI.GetText(row, 8, 3).ToInt() == record.LN_SEQ)
                {
                    RI.PutText(21, 18, RI.GetText(row, 4, 3), ReflectionInterface.Key.Enter);
                    record = TS8S_Target(record);
                    break;
                }
            }
            return record;
        }

        private ReinstatementRecord TS8S_Target(ReinstatementRecord record)
        {
            record.CleanupBills = false;

            for (int billRow = 10; LoopContinueCondition(); billRow++)
            {
                if (RI.CheckForText(billRow, 8, " ")) // Check for page forward.
                {
                    RI.Hit(ReflectionInterface.Key.Enter);
                    if (RI.CheckForText(23, 2, "03944 PRESS PF4 TO UPDATE"))
                    {
                        RI.Hit(ReflectionInterface.Key.F4);
                        if (!RI.CheckForText(23, 2, "03347 ALL UPDATES COMPLETED"))
                            PLR.AddNotification(string.Format($"BBREININSC: TS8S_Target: Update failed for : {record.BF_SSN} {record.LN_SEQ}."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                    billRow = 10;
                    RI.Hit(ReflectionInterface.Key.F8);
                }

                if (RI.GetText(billRow, 19, 8).ToDate() == record.LD_BIL_DU) // If bill is the first bill we are looking for then mark to override and set flag.
                {
                    RI.PutText(billRow, 54, "Y");
                    break;
                }
            }

            RI.Hit(ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F4);
            if (!RI.CheckForText(23, 2, "03347 ALL UPDATES COMPLETED"))
            {
                PLR.AddNotification(string.Format($"BBREININSC: TS8S_Target: Update failed for : {record.BF_SSN} {record.LN_SEQ}. Session message: {RI.Message}"), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                record.CleanupBills = false;
                if (RI.CheckForText(2, 2, "* * * * * * *"))
                    PLR.AddNotification(string.Format($"BBREININSC: Loan abended in TS8S: {record.BF_SSN} {record.LN_SEQ}."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            else
            {
                record.CleanupBills = true;
            }

            

            return record;
        }

        /*private ReinstatementRecord UpdateTSDS(ReinstatementRecord record)
        {
            record.UpdateTSDS = false;
            RI.FastPath("TX3ZCTSDS" + record.BF_SSN);
            //05650 PENDING ADJUSTMENT EXISTS - BATCH RUN REQUIRED TO ENSURE CORRECT DATA 
            for (int row = 8; !RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"); row++)
            {
                if (RI.CheckForText(row, 8, " ") || row == 19)
                {
                    row = 7;
                    RI.Hit(ReflectionInterface.Key.F8);
                }
                else if ((RI.GetText(row, 8, 3).ToInt() == record.LN_SEQ)) // Look for matching sequence number
                {
                    RI.PutText(21, 18, RI.GetText(row, 4, 3), ReflectionInterface.Key.Enter, true);
                    break;
                }
            }
            if (RI.CheckForText(23, 2, "05650 PENDING ADJUSTMENT EXISTS - BATCH RUN REQUIRED TO ENSURE CORRECT DATA"))
            {
                string msg = string.Format($"Borrower {record.BF_SSN} has pending adjustment, batch run required, on screen TSDS.");
                PLR.AddNotification(msg, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                record.UpdateTSDS = false;
                return record;
            }

            RI.PutText(11, 16, "Y"); //Issue fix
            RI.PutText(13, 16, "", true);
            RI.PutText(13, 19, "", true);
            RI.PutText(13, 22, "", true);
            RI.PutText(13, 44, "", true);
            RI.Hit(ReflectionInterface.Key.F6);
            RI.PutText(21, 11, $"{record.RecordType} REINSTATED BENEFIT ELIGIBILITY. DELQ BILL WAS COVERED BY DEFER OR MANDATORY FORB.");
            RI.Hit(ReflectionInterface.Key.F6);

            if (!RI.CheckForText(23, 2, "01005 RECORD SUCCESSFULLY CHANGED"))
            {
                string msg = string.Format("Borrower {0} did not successfully update TSDS screen.", record.BF_SSN);
                PLR.AddNotification(msg, NotificationType.ErrorReport, NotificationSeverityType.Informational);
            }
            record.UpdateTSDS = true;
            return record;
        }*/

        public void EnterTD22Comments(List<ReinstatementRecord> records)
        {   // Note: If ARCs are ByLoan, sequence numbers are registered.
            List<int> loans = records.DistinctBy(p => p.LN_SEQ).Where(r => r.RecordType != "U36").Select(q => q.LN_SEQ).ToList();
            List<int> loansU36 = records.DistinctBy(p => p.LN_SEQ).Where(r => r.RecordType == "U36").Select(q => q.LN_SEQ).ToList();
            List<string> recordTypes = records.DistinctBy(p => p.RecordType).Select(q => q.RecordType).ToList();
            
            if (loans.Any())
            {
                ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa);
                ArcAddResults result;
                arc.AccountNumber = records.FirstOrDefault().BF_SSN;
                arc.ArcTypeSelected = ArcData.ArcType.Atd22ByLoan;
                arc.Arc = "U48OB";
                arc.Comment = "Bill Override for U48, N48, or R48";
                arc.ScriptId = SCRIPTID;
                arc.LoanSequences = loans;
                result = arc.AddArc();
                if (!result.ArcAdded)
                    PLR.AddNotification(string.Format($"BBREININSC: Add Arc U48OB failure for borrower: {records.FirstOrDefault().BF_SSN}"), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            if (loansU36.Any())
            {
                ArcData arcU36 = new ArcData(DataAccessHelper.Region.Uheaa);
                ArcAddResults resultU36;
                arcU36.AccountNumber = records.FirstOrDefault().BF_SSN;
                arcU36.ArcTypeSelected = ArcData.ArcType.Atd22ByLoan;
                arcU36.Arc = "U36OB";
                arcU36.Comment = "Bill Override for U36";
                arcU36.ScriptId = SCRIPTID;
                arcU36.LoanSequences = loansU36;
                resultU36 = arcU36.AddArc();
                if (!resultU36.ArcAdded)
                    PLR.AddNotification(string.Format($"BBREININSC: Add Arc U36OB failure for borrower: {records.FirstOrDefault().BF_SSN}"), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }            
        }
    }
}
