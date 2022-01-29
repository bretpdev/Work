using System;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;


namespace ACURINTR
{
    public class DataAccess
    {

        public ProcessLogRun PLR { get; set; }
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun plr)
        {
            PLR = plr;
            LDA = PLR.LDA;
        }


        /// <summary>
        /// Persists a list of QueueTask representing pending PDEMs to BSYS for recovery.
        /// </summary>
        /// <param name="task">QueueTask object to be persisted for recovery.</param>
        [UsesSproc(DataAccessHelper.Database.Odw, "ValidTaskAdd")]
        public bool ValidTask(string accountNumber)
        {
            int eax = LDA.ExecuteSingle<int>("ValidTaskAdd", DataAccessHelper.Database.Odw, 
            new SqlParameter("AccountNumber", accountNumber)).Result;
            return eax == 1 ? true : false;
        }


        /// <summary>
        /// Persists a list of QueueTask representing pending PDEMs to BSYS for recovery.
        /// </summary>
        /// <param name="task">QueueTask object to be persisted for recovery.</param>
        [UsesSproc(DataAccessHelper.Database.Bsys, "AddAccurintrRecoveryRecord")]
        public void AddRecoveryRecord(QueueTask task)
        {
            LDA.Execute("AddAccurintrRecoveryRecord", DataAccessHelper.Database.Bsys,
                new SqlParameter("AccountNumber", task.Demographics.AccountNumber),
                new SqlParameter("PageNumber", task.PageNumber),
                new SqlParameter("PdemVerificationDate", task.PdemVerificationDate),
                new SqlParameter("OriginalDemographicsText", task.OriginalDemographicsText ?? ""),
                new SqlParameter("Address1", task.Demographics.Address1 ?? ""),
                new SqlParameter("Address2", task.Demographics.Address2 ?? ""),
                new SqlParameter("City", task.Demographics.City ?? ""),
                new SqlParameter("DemographicsSource", task.DemographicsSource),
                new SqlParameter("PdemSource", task.PdemSource ?? ""),
                new SqlParameter("Phone", task.Demographics.PrimaryPhone ?? ""),
                new SqlParameter("State", task.Demographics.State ?? ""),
                new SqlParameter("SystemSource", task.SystemSource),
                new SqlParameter("Zip", task.Demographics.ZipCode ?? ""),
                new SqlParameter("AdditionalInfo", task.AdditionalInfo ?? ""));
        }

        public string LppManagerId()
        {
            return GetManagerId("ACURINTR-LPP");
        }


        public string LgpManagerId()
        {
            return GetManagerId("ACURINTR-LGP");
        }

        /// <summary>
        /// Gets the user ID of the manager of Borrower Services.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "GetManagerOfBusinessUnit")]
        private string GetManagerId(string efsKey)
        {
            string buName = EnterpriseFileSystem.GetPath(efsKey);
            try
            {
                var result = LDA.ExecuteSingle<string>("GetManagerOfBusinessUnit", DataAccessHelper.Database.Bsys, new SqlParameter("BusinessUnit", buName)).Result;
                if (string.IsNullOrWhiteSpace(result))
                    throw new Exception();
                return result;
            }
            catch (Exception)
            {
                string message = "A unique user ID for the manager of " + buName + " was not found. Please contact Process Automation for assistance.";
                Console.WriteLine(message);
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                throw new EarlyTerminationException();
            }
        }

        /// <summary>
        /// Gets a list of sources and their associated locate types,
        /// OneLINK source codes, and COMPASS source codes from BSYS.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "GetSourceLocateTypes")]
        public List<SystemCode> SystemCodes()
        {
            return DataAccessHelper.ExecuteList<SystemCode>("GetSourceLocateTypes", DataAccessHelper.Database.Bsys);
        }

        /// <summary>
        /// Removes an individual pending PDEM record from recovery to prevent it from being processed again.
        /// </summary>
        /// <param name="task">A QueueTask object representing the pending PDEM to be removed from recovery.</param>
        [UsesSproc(DataAccessHelper.Database.Bsys, "DeleteAccurintrRecoveryRecord")]
        public void DeletePendingPdemRecoveryRecord(QueueTask pdem)
        {
            LDA.Execute("DeleteAccurintrRecoveryRecord", DataAccessHelper.Database.Bsys, new SqlParameter("AccountNumber", pdem.Demographics.AccountNumber), new SqlParameter("PageNumber", pdem.PageNumber));
        }

        /// <summary>
        /// Purges all records from the pending PDEM recovery table.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "DeleteAllAccurintrRecoveryRecords")]
        public void DeleteRecoveryRecords()
        {
            LDA.Execute("DeleteAllAccurintrRecoveryRecords", DataAccessHelper.Database.Bsys);
        }

        /// <summary>
        /// Gets a list of the queues to process and methods for parsing and processing them.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "GetAccurintrQueues")]
        public List<QueueData> GetQueues()
        {
            return LDA.ExecuteList<QueueData>("GetAccurintrQueues", DataAccessHelper.Database.Bsys).Result;
        }

        /// <summary>
        /// Gets a list of locate-related queue names from BSYS.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "GetLocateQueues")]
        public List<string> LocateQueues()
        {
            return LDA.ExecuteList<string>("GetLocateQueues", DataAccessHelper.Database.Bsys).Result;
        }

        /// <summary>
        /// Gets a list of QueueTask objects representing the pending PDEMs for the recovery queue task.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "GetAccurintrRecoveryData")]
        public List<QueueTask> GetRecordsFromRecovery()
        {
            List<QueueTask> taskList = new List<QueueTask>();
            //Get the flattened recovery data from BSYS.
            List<PdemRecoveryRecord> recoveryRecords = DataAccessHelper.ExecuteList<PdemRecoveryRecord>("GetAccurintrRecoveryData", DataAccessHelper.Database.Bsys);

            foreach (PdemRecoveryRecord record in recoveryRecords)
            {
                //Fill out a QueueTask object, along with its attendant AccurintRDemographics object, and add it to the list.
                AccurintRDemographics demos = new AccurintRDemographics();
                demos.AccountNumber = record.AccountNumber;
                demos.Address1 = record.Address1;
                demos.Address2 = record.Address2;
                demos.City = record.City;
                demos.EmailAddress = record.Email;
                demos.PrimaryPhone = record.Phone;
                demos.State = record.State;
                demos.ZipCode = record.Zip;
                QueueTask task = new QueueTask(record.DemographicsSource, record.SystemSource, this);
                task.Demographics = demos;
                task.OriginalDemographicsText = record.OriginalDemographicsText;
                task.PageNumber = record.PageNumber;
                task.PdemSource = record.PdemSource;
                task.PdemVerificationDate = record.PdemVerificationDate;
                task.AdditionalInfo = record.AdditionalInfo;
                taskList.Add(task);
            }
            return taskList;
        }

        /// <summary>
        /// Gets a list of demographics sources, reject reasons, and their associated
        /// action codes for address and phone processing from BSYS.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "GetRejectActionsForSource")]
        public List<RejectAction> GetRejectActions(string demographicsSource)
        {
            return LDA.ExecuteList<RejectAction>("GetRejectActionsForSource", DataAccessHelper.Database.Bsys, new SqlParameter("DemographicsSource", demographicsSource)).Result;
        }

        /// <summary>
        /// Gets a list of state codes from BSYS.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "GetStateCodes")]
        public List<string> StateCodes()
        {
            return LDA.ExecuteList<string>("GetStateCodes", DataAccessHelper.Database.Bsys).Result;
        }

        /// <summary>
        /// Updates the PDEM recovery table to use an account number where it had been previously using an SSN.
        /// </summary>
        /// <param name="ssn">The SSN that was used to initially populate the table.</param>
        /// <param name="accountNumber">The account number that will replace the SSN.</param>
        [UsesSproc(DataAccessHelper.Database.Bsys, "UpdateAccurintRecoverySSNtoAccountNumber")]
        public void UpdatePdemRecoveryAccountNumber(string ssn, string accountNumber)
        {
            LDA.Execute("UpdateAccurintRecoverySSNtoAccountNumber", DataAccessHelper.Database.Bsys, new SqlParameter("AccountNumber", accountNumber), new SqlParameter("SSN", ssn));
        }

        private class PdemRecoveryRecord
        {
            public string AccountNumber { get; set; }
            public string AdditionalInfo { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string DemographicsSource { get; set; }
            public string Email { get; set; }
            public string OriginalDemographicsText { get; set; }
            public int PageNumber { get; set; }
            public string PdemSource { get; set; }
            public DateTime PdemVerificationDate { get; set; }
            public string Phone { get; set; }
            public string State { get; set; }
            public string SystemSource { get; set; }
            public string Zip { get; set; }
        }
    }
}
