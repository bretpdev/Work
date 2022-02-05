using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using ACS.Infrastructure;
// Q is removed from this project.

namespace ACS.Processors
{
    public class Processor
    {
        private Name_Spec NameSpec = new Name_Spec();
        private ReflectionInterface RI = null;
        private DataAccess DA = null;
        private ProcessLogRun PLR = null;
        private List<Ref_Spec> Dupes = new List<Ref_Spec>();
        private List<AcsOlFileRecord> OlRecords;
        private List<AcsUhFileRecord> UhRecords;
        private FileProcessor FP;
        Parse_Spec parse = new Parse_Spec();

        public Processor(ReflectionInterface _ri, DataAccess _da, ProcessLogRun _logRun)
        {
            RI = _ri;
            DA = _da;
            PLR = _logRun;
            FP = new FileProcessor(DA, PLR);
            OlRecords = new List<AcsOlFileRecord>();
            UhRecords = new List<AcsUhFileRecord>();
        }


        /// <summary>
        /// Main method for data processing.
        /// </summary>
        /// <param name="startDate">Null if today, otherwise desired start date</param>
        /// <returns></returns>
        public int? Process(DateTime startDate)
        {
            int parseAndLoadResult = FP.AddRecordsToProcess(startDate);
            int oneLinkResult = ProcessOneLINKRecords();
            int uheaaResult = ProcessUheaaRecords();

            return parseAndLoadResult + oneLinkResult + uheaaResult;
        }

        private int ProcessOneLINKRecords()
        {
            int arcId = 0;
            int result = 0;
            OlRecords = GetUnprocessedOneLINKRecords();

            if (OlRecords.Count == 0)
            {
                PLR.AddNotification("No OneLINK records found for processing", NotificationType.NoFile, NotificationSeverityType.Informational);
                return result;
            }

            foreach (AcsOlFileRecord record in OlRecords)
            {
                arcId = 0;
                arcId = ProcessOneLINKRecord(record);
                if (arcId > 0)
                    DA.MarkOneLinkRecordProcessed(record.OneLinkDemographicsId, arcId);
                else
                {
                    PLR.AddNotification(string.Format("File {0} PP ID {1} for account {2}: Arc not added or db record not updated.", record.FileId, record.OneLinkDemographicsId, record.SSN), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    result++;
                }
            }
            return result;
        }

        private int ProcessUheaaRecords()
        {
            int arcId = 0;
            int result = 0;
            UhRecords = GetUnprocessUheaaRecords();

            if (UhRecords.Count == 0)
            {
                PLR.AddNotification("No UHEAA records found for processing", NotificationType.NoFile, NotificationSeverityType.Informational);
                return result;
            }

            foreach (AcsUhFileRecord record in UhRecords)
            {
                arcId = 0;
                arcId = ProcessUheaaRecord(record);
                if (arcId > 0)
                    DA.MarkUheaaRecordProcessed(record.UheaaDemographicsId, arcId);
                else
                {
                    PLR.AddNotification($"Arc not added or DB record not updated for File: {record.FileId}, UheaaDemographicsId: {record.UheaaDemographicsId}, account: {record.SSN}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    result++;
                }
            }
            return result;
        }

        private int ProcessUheaaRecord(AcsUhFileRecord record)
        {
            int arcId = 0;
            arcId = AddUheaaActivityComment(record, "ACRTN");
            return arcId;
        }

        /// <summary>
        /// Gathers unprocessed UH records from the DB and then populates
        /// its properties based on the demo info on the DB.
        /// </summary>
        /// <returns></returns>
        private List<AcsUhFileRecord> GetUnprocessUheaaRecords()
        {
            List<AcsUhFileRecord> uhRecords = DA.GetUnprocessedUheaaRecords();
            foreach (var rec in uhRecords) // Populate fields that are not 1-to-1 mapped from obj to DB table
            {
                rec.NewAddress.Addr1 = rec.Address1.Trim();
                rec.NewAddress.Addr2 = rec.Address2?.Trim() ?? "";
                rec.NewAddress.City = rec.City.Trim();
                rec.NewAddress.State = rec.State.Trim();
                rec.NewAddress.Zip = rec.Zip.Trim();
                rec.ConcatenatedData.NewAddress = rec.NewAddressFull;
                rec.ConcatenatedData.OldAddress = rec.OldAddressFull;
                rec.FirstName = rec.FullName.Split(',').ToArray()[1].Trim();
                rec.LastName = rec.FullName.Split(',').ToArray()[0].Trim();
            }
            return uhRecords;
        }

        /// <summary>
        /// Gathers unprocessed OL records from the DB and then populates
        /// its properties based on the demo info on the DB.
        /// </summary>
        /// <returns></returns>
        private List<AcsOlFileRecord> GetUnprocessedOneLINKRecords()
        {
            List<AcsOlFileRecord> olRecords = DA.GetUnprocessedOneLINKRecords();
            foreach (var rec in olRecords) // Populate fields that are not 1-to-1 mapped from obj to DB table
            {
                rec.NewAddress.Addr1 = rec.Address1.Trim();
                rec.NewAddress.Addr2 = rec.Address2?.Trim() ?? "";
                rec.NewAddress.City = rec.City.Trim();
                rec.NewAddress.State = rec.State.Trim();
                rec.NewAddress.Zip = rec.Zip.Trim();
                rec.ConcatenatedData.NewAddress = rec.NewAddressFull;
                rec.ConcatenatedData.OldAddress = rec.OldAddressFull;
                rec.FirstName = rec.FullName.Split(',').ToArray()[1].Trim();
                rec.LastName = rec.FullName.Split(',').ToArray()[0].Trim();
            }
            return olRecords;
        }


        /// <summary>
        /// Singular record processor
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private int ProcessOneLINKRecord(AcsOlFileRecord record)
        {
            int arcId = 0;

            if (record.PType == PersonTypeEnum.Reference)
                arcId = Reference_QueueTask(record);
            else
                arcId = Borrower_QueueTask(record);

            return arcId;
        }



        /// <summary>
        /// Puts the arc in ArcAddProcessing
        /// </summary>
        /// <param name="acsRecord"></param>
        /// <param name="acct"></param>
        /// <param name="theArc"></param>
        /// <param name="actType"></param>
        /// <param name="actContact"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public int AddOneLINKActivityComment(AcsOlFileRecord acsRecord, string theArc, string actType, string actContact, string comment)
        {
            List<int> loanSequences = new List<int>();
            List<string> loanPrograms = new List<string>();
            bool isRef = acsRecord.PType == PersonTypeEnum.Reference;

            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                Arc = theArc,
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                ActivityType = actType,
                ActivityContact = actContact,
                Comment = comment,
                ScriptId = DA.ScriptId,
                LoanSequences = loanSequences,
                LoanPrograms = loanPrograms,
                IsReference = isRef,
                IsEndorser = false,
                AccountNumber = acsRecord.AccountNumber,
                ProcessOn = DateTime.Now
            };

            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"Error adding arc for Account {acsRecord.AccountNumber}.";
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return -1;
            }
            return result.ArcAddProcessingId;
        }

        /// <summary>
        /// Adds the ACRTN ARC for the record, creating an LX task that has
        /// the new demo info in the task message in a comma-delimited format.
        /// </summary>
        /// <param name="acsRecord"></param>
        /// <param name="acct"></param>
        /// <param name="arc"></param>
        /// <returns></returns>
        public int AddUheaaActivityComment(AcsUhFileRecord acsRecord, string arc)
        {
            List<int> loanSequences = new List<int>();
            List<string> loanPrograms = new List<string>();
            bool isRef = acsRecord.PType == PersonTypeEnum.Reference;

            //This is a comma-delimited message that is read by ACURINTC. For comparison, ACCURINT creates the LX comma-delimited message as well.
            string comment = $"{acsRecord.NewAddress.Addr1.Replace(",", "")}, {acsRecord.NewAddress.Addr2.Replace(",", "")}, {acsRecord.NewAddress.City.Replace(",", "")}, {acsRecord.NewAddress.State.Replace(",", "")}, {acsRecord.Zip.SafeSubString(0, 5).Replace(",", "")},,,,, ACS.";

            ArcData arcData = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                Arc = arc,
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                AccountNumber = acsRecord.AccountNumber,
                Comment = comment,
                ScriptId = DA.ScriptId,
                LoanSequences = new List<int>(),
                IsReference = isRef,
                IsEndorser = false
            };

            ArcAddResults result = arcData.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"Error adding Arc: {arc} for Account: {acsRecord.AccountNumber}. Comment: {comment}";
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return -1;
            }
            return result.ArcAddProcessingId;
        }

        /// <summary>
        /// Determines actions for borrowers
        /// </summary>
        /// <param name="cust"></param>
        /// <returns></returns>
        private int Borrower_QueueTask(AcsOlFileRecord cust)
        {
            NameSpec = DA.OneLINKDemos(cust.SSN);
            if (NameSpec == null)
            {
                PLR.AddNotification(string.Format("ODWPD01 PDM INF record not found account: {0}", cust.SSN), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return -1;
            }

            int arcId = 0;

            if (cust.LastName.Trim().ToUpper() == NameSpec.LasName.Trim().ToUpper())
            {
                CreateBorrowerQueueTask(cust);
                arcId = AddOneLINKActivityComment(cust, "M4ACS", "AM", "90", "");
            }
            else if (NameSpec.FirstName.SafeSubString(0, 3).ToUpper() == cust.FirstName.SafeSubString(0, 3).ToUpper())
            {
                CreateBorrowerQueueTask(cust);
                arcId = AddOneLINKActivityComment(cust, "K4AKA", "AM", "10", string.Format("Possible new last name: {0}.", cust.LastName));
            }
            else
            {
                arcId = AddOneLINKActivityComment(cust, "M4ACS", "AM", "90", "");
            }

            return arcId;
        }


        /// <summary>
        /// Create Queue tesk for borrower
        /// </summary>
        /// <param name="acsRecord"></param>
        /// <param name="acct"></param>
        private void CreateBorrowerQueueTask(AcsOlFileRecord acsRecord)
        {
            acsRecord.NewAddress.Addr1 = DA.applyStandardAbbreviations(acsRecord.NewAddress.Addr1);
            acsRecord.NewAddress.Addr2 = DA.applyStandardAbbreviations(acsRecord.NewAddress.Addr2);
            string comments = $"{acsRecord.NewAddress.Addr1},{acsRecord.NewAddress?.Addr2},{acsRecord.NewAddress.City},{acsRecord.NewAddress.State},{acsRecord.NewAddress.Zip}";
            if (RI.AddQueueTaskInLP9O(acsRecord.SSN, "SRMNOADD", null, comments) == false)
            {
                PLR.AddNotification(string.Format("Error while adding SRMNOADD queue task. {0}", acsRecord.AccountNumber), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }


        /// <summary>
        /// Reference queue task
        /// </summary>
        /// <param name="acsRecord"></param>
        /// <returns></returns>
        private int Reference_QueueTask(AcsOlFileRecord acsRecord)
        {

            if (!Dupes.Any(p => p.ID == acsRecord.SSN && p.Address == acsRecord.ConcatenatedData.NewAddress)) // If we haven't already dropped a task on this ref during this script run, drop task
            {
                string comment = $"{acsRecord.AddressType} {acsRecord.POAddressDate} Reference Address Needs Updating: {acsRecord.ConcatenatedData.FullName} {acsRecord.ConcatenatedData.OldAddress} , {acsRecord.ConcatenatedData.NewAddress}";
                if (!RI.AddQueueTaskInLP9O(acsRecord.SSN, "DPOREF", null, comment))
                {
                    PLR.AddNotification(string.Format("Error while trying to add DPOREF queue task. {0}", acsRecord.SSN), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                else
                    Dupes.Add(new Ref_Spec(acsRecord.SSN, acsRecord.ConcatenatedData.NewAddress)); // Add record to dupe-tracking list
            }

            // If task has already been created for the day just add a comment.
            int arcId = AddOneLINKActivityComment(acsRecord, "M4ACS", "AM", "90", "ACS");
            return arcId;
        }
    }
}
