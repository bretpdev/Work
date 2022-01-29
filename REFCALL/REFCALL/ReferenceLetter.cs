using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace REFCALL
{
    public class ReferenceLetter
    {
        RecoveryHelper RH { get; set; }
        DataAccess DA { get; set; }
        CcpHelper CCH { get; set; }
        const string Queue = "KREFRLTR";

        public ProcessLogRun LogRun { get; set; }
        public ReflectionInterface RI { get; set; }
        public string ScriptId { get; set; }
        public RecoveryLog Recovery { get; set; }

        public ReferenceLetter(ReflectionInterface ri, ProcessLogRun logRun, string scriptId)
        {
            RI = ri;
            LogRun = logRun;
            ScriptId = scriptId;

            Recovery = new RecoveryLog(ScriptId);
            RH = new RecoveryHelper(Recovery);
            DA = new DataAccess(LogRun);
            CCH = new CcpHelper(RI, DA, LogRun);
        }

        public int Main()
        {
            int returnCode = 0;
            try
            {
                if (RH.CurrentStep != RecoveryHelper.RecoveryStep.Startup)
                    Console.WriteLine($"Recoverying on step: {(RecoveryHelper.RecoveryStep)Recovery.RecoveryValue?.Split(',')[0].ToInt()}");

                returnCode = AddBorrower();
                Recovery.Delete();
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error running REFCALL", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                returnCode = 1;
            }
            return returnCode;
        }

        private int AddBorrower()
        {
            string activityContact = "10";
            while (!RI.AltMessageCode.IsIn("47423", "47420"))
            {
                //get task
                RI.FastPath("LP9AC");
                if (!RI.CheckForText(3, 13, "WORK"))
                {
                    RI.EnterText(Queue);
                    RI.Hit(Key.Enter);
                }

                //stop processing if there are no more tasks
                if (RI.AltMessageCode.IsIn("47423", "47420") && !RH.InRecovery)
                    break;

                //warn user if the wrong queue is displayed
                if (!RI.CheckForText(1, 9, Queue))
                {
                    string message = $"You have an unresolved task in the {RI.GetText(1, 9, 8)} queue.  You must complete the task before working the {Queue} queue.";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return 1;
                }

                string actionCode = "KABL2";
                //get account information
                string ssn = RI.GetText(17, 70, 9);
                string refId = RI.GetText(12, 11, 60) + RI.GetText(13, 11, 60) + RI.GetText(14, 11, 60) + RI.GetText(15, 11, 60);
                refId = refId.SafeSubString(refId.IndexOf("RF@"), 9);
                string acctNum = RI.GetText(17, 52, 12).Replace(" ", "");

                //reassign the stask if the RefId doesn't match the target ID on LP8Y
                RI.FastPath(string.Format("LP8YISKP;KREFRLTR;{0};{1};{2};W", RI.UserId, ssn, refId));
                if (RI.CheckForText(1, 60, "QUEUE STATS SELECTION"))
                    ReassignTask(ssn);
                else//process the task
                {
                    if (RH.InRecovery && (RH.CurrentSsn != ssn || RH.CurrentReferenceId != refId))
                        RH.Reset(); //Different recovery borrower than the current queue
                    RH.CurrentSsn = ssn;
                    RH.CurrentReferenceId = refId;

                    //Determine Cost Center Code
                    var ccc = CCH.GetCostCenterCode(ssn);
                    FileRecord record = new FileRecord();
                    record.COST_CENTER_CODE = ccc.Ccc;
                    record.ACSPartCd = ccc.AcsPartCode;
                    record.ACSKeyLine = DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Reference, DocumentProcessing.ACSKeyLineAddressType.Legal);
                    record.RefID = refId;
                    record.AccountNumber = acctNum;

                    //access LP2C
                    RI.FastPath("LP2CI;" + refId);

                    string comment = "";
                    if (RI.CheckForText(6, 67, "I")) //cancel the task if the reference is inactive
                    {
                        comment = "KREFRLTR task canceled due to inactive reference";
                        actionCode = "KABA2";
                        Finish(ssn, refId, actionCode, activityContact, comment, false);
                    }
                    //cancel the task if the reference is inactive
                    else if (RI.CheckForText(6, 67, "A") && RI.CheckForText(13, 76, "04"))
                    {
                        comment = "KREFRLTR task canceled due to do not contact reference";
                        actionCode = "KUBA4";
                        Finish(ssn, refId, actionCode, activityContact, comment, false);
                    }
                    else //process the task
                        LoadDemos(actionCode, ssn, acctNum, record, ccc);
                }
            }
            return 0;
        }

        private void LoadDemos(string actionCode, string ssn, string acctNum, FileRecord record, CostCenterCode ccc)
        {
            record.RefName = RI.GetText(4, 44, 12) + " " + RI.GetText(4, 5, 35);
            record.RefAddress1 = RI.GetText(8, 9, 34);
            record.RefAddress2 = RI.GetText(9, 9, 34);
            record.RefCity = RI.GetText(10, 9, 34);
            if (RI.CheckForText(9, 55, " "))
                record.State_Ind = record.RefState = RI.GetText(10, 52, 2);
            record.RefZip = RI.GetText(10, 60, 9);
            if (record.RefZip.Length > 5)
                record.RefZip = record.RefZip.Insert(5, "-");
            record.RefCountry = RI.GetText(9, 55, 24);
            bool refradd = RI.CheckForText(8, 53, "Y");
            var demo = RI.GetDemographicsFromLP22(ssn);

            record.FirstName = demo.FirstName;
            record.LastName = demo.LastName;
            record.Address1 = demo.Address1;
            record.Address2 = demo.Address2;
            record.City = demo.City;
            record.ZIP = demo.ZipCode;
            record.State = demo.State == "FC" ? null : demo.State;
            record.Country = demo.Country;

            //Add the demos to the data file
            AddReference(refradd, ssn, acctNum, actionCode, record, ccc);
        }


        /// <summary>
        ///  Reassign the the task for call to be made if the reference address is blank or send a letter if the address is not blank.
        /// </summary>
        private void AddReference(bool referenceAdd, string ssn, string acctNum, string actionCode, FileRecord record, CostCenterCode ccc)
        {
            if (!referenceAdd)
                ReassignTask(ssn);
            else
            {
                if (RH.CurrentStep == RecoveryHelper.RecoveryStep.Startup)
                {
                    List<string> lines = CsvHelper.GetHeaderAndLineFromObject(record, ",");
                    lines.RemoveAt(0);
                    int? added = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, "REFRCAL", lines[0], record.AccountNumber, ccc.Ccc, DataAccessHelper.CurrentRegion);
                    if (!added.HasValue)
                    {
                        string message = $"There was an error adding the data to the print processing table for borrower: {ssn}.";
                        LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        Dialog.Warning.Ok(message);
                    }
                    RH.CurrentStep = RecoveryHelper.RecoveryStep.WroteReferenceData;
                }

                Finish(ssn, record.RefID, actionCode, "27", "Letter sent to reference to locate borrower", true);
            }
        }
        
        /// <summary>
        /// Reassign the task to a skip tracer.
        /// </summary>
        private void ReassignTask(string ssn)
        {
            string targetId = "";

            //access LP8Y
            RI.FastPath("LP8YCSKP;KREFRLTR;;" + ssn);

            //position cursor on the row in work status
            int row = 7;
            while (row <= 21)
            {
                if (RI.CheckForText(row, 33, "W") && RI.CheckForText(row, 38, RI.UserId))
                {
                    targetId = RI.GetText(row, 12, 9);
                    break;
                }
                else if (RI.CheckForText(row, 33, " "))
                {
                    LogRun.AddNotification($"Issue reassigning task KREFRLTR for {ssn}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return;
                }
                else
                {
                    row++;
                    if (row == 21)
                    {
                        RI.Hit(Key.F8);
                        row = 7;
                    }
                }
            }
            //update status to assigned
            RI.PutText(row, 33, "A", Key.F6);
            //find the task again
            RI.ReflectionSession.MoveCursor(7, 33);
            row = 7;
            while (row <= 21)
            {
                if (RI.CheckForText(row, 12, targetId) && RI.CheckForText(row, 33, "A") && RI.CheckForText(row, 38, "_"))
                    break;
                else if (RI.CheckForText(row, 33, " "))
                {
                    LogRun.AddNotification($"Issue reassigning task KREFRLTR for {ssn}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return;
                }
                else
                {
                    row++;
                    if (row == 21)
                    {
                        RI.Hit(Key.F8);
                        row = 7;
                    }
                    RI.ReflectionSession.MoveCursor(row, 33);
                }
            }
            string reassignId = DA.GetManagerId();
            //enter id of new user
            RI.Hit(Key.Tab);
            RI.EnterText(reassignId);
            RI.Hit(Key.Enter);
            RI.Hit(Key.F6);

        }


        private void Finish(string ssn, string refId, string actionCode, string activityContact, string comment, bool addBorrowerActionCode)
        {
            Sleep.For(2).Seconds();
            //add borrower action code
            if (RH.CurrentStep == RecoveryHelper.RecoveryStep.Startup || RH.CurrentStep == RecoveryHelper.RecoveryStep.WroteReferenceData)
            {
                RI.AddCommentInLP50(ssn, "CO", activityContact, actionCode, comment, ScriptId);
                RH.CurrentStep = RecoveryHelper.RecoveryStep.AddBorrowerActionCode;
            }
            if (RH.CurrentStep == RecoveryHelper.RecoveryStep.AddBorrowerActionCode)
            {
                if (addBorrowerActionCode)
                {
                    RI.FastPath("LP50A" + ssn);
                    RI.EnterText("KRREF");
                    RI.Hit(Key.Enter);
                    RI.EnterText("CO");
                    RI.EnterText("27");
                    RI.PutText(13, 2, "Letter sent to reference " + refId + " to locate borrower", Key.Enter);
                    RI.Hit(Key.F6);
                }
                RH.CurrentStep = RecoveryHelper.RecoveryStep.AddReferenceActionCode;
            }
            if (RH.CurrentStep == RecoveryHelper.RecoveryStep.AddReferenceActionCode)
            {
                RI.AddCommentInLP50(refId, "LT", "27", "KRREF", comment, ScriptId);
                RH.CurrentStep = RecoveryHelper.RecoveryStep.CloseQueue;
            }
            if (RH.CurrentStep == RecoveryHelper.RecoveryStep.CloseQueue)
            {
                RI.FastPath("LP9ACKREFRLTR");
                RI.Hit(Key.F6);
                //reassign the task if it was completed successfully
                if (RI.AltMessageCode != "49000")
                    ReassignTask(ssn);
            }
            RH.Reset();
        }
    }
}