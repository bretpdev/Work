using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace TCPAPNS
{
    public class UpdateCompass
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }

        public UpdateCompass(ProcessLogRun logRun, DataAccess da)
        {
            LogRun = logRun;
            DA = da;
        }

        public bool Update(ReflectionInterface ri, FileProcessingRecord record)
        {
            ri.FastPath("TX3Z/CTX1J");
            ClearCTX1J(ri);

            if (record.AccountNumber.Contains("P"))
                ri.PutText(6, 16, record.AccountNumber, Enter);
            else
                ri.PutText(6, 61, record.AccountNumber, Enter);

            if (!ri.CheckForText(1, 71, "TXX1R"))
            {
                LogRun.AddNotification($"Account not found on Compass: Account {record.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            string personType = ri.GetText(1, 9, 1);
            string borrowerSsn;
            List<string> referenceBorrowers = new List<string>();

            if (personType.Contains("B"))
                borrowerSsn = ri.GetText(3, 12, 11).Replace(" ", "");
            else
                borrowerSsn = ri.GetText(7, 11, 11).Replace(" ", "");

            if (personType.Contains("R"))
            {
                LogRun.AddNotification($"Unable to handle person type R, account: {record.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            if (personType.Contains("E"))
            {
                ri.FastPath("TX3Z/CTX1J");
                ClearCTX1J(ri);

                if (record.AccountNumber.Contains("P"))
                    ri.PutText(6, 16, record.AccountNumber, Enter);
                else
                    ri.PutText(6, 61, record.AccountNumber, Enter);
            }

            ri.Hit(F6, referenceBorrowers.Count() > 0 ? 2 : 3);

            UpdatePhone(ri, record, borrowerSsn, personType, referenceBorrowers);

            return true;
        }

        private bool UpdatePhone(ReflectionInterface ri, FileProcessingRecord record, string borrowerSsn, string personType, List<string> referenceBorrowers)
        {
            List<string> updatedPhoneTypes = new List<string>();
            //Update phone
            bool phoneFound = false;
            bool personUpdated = false;
            string updateResults = "";

            record.PhoneTypes = DA.GetPhoneData
            (
                personType == "B" ? borrowerSsn : record.AccountNumber,
                record.PhoneNumber,
                record.MobileIndicator,
                record.HasConsentArc
            );

            if (record.PhoneTypes == null || record.PhoneTypes.Count == 0)
            {
                LogRun.AddNotification($"Compass: Unable to get phone type for account: {record.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            foreach (string phoneType in record.PhoneTypes)
            {
                ri.PutText(16, 14, phoneType, Enter);
                string borrowersPhone = ri.GetText(17, 14, 3) + ri.GetText(17, 23, 3) + ri.GetText(17, 31, 4);

                //Found the phone number
                if (record.PhoneNumber == borrowersPhone)
                {
                    phoneFound = true;
                    bool updated = MobileUpdate(ri, record, phoneType);
                    updateResults += phoneType + ",";
                    if (updated)
                    {
                        updatedPhoneTypes.Add(phoneType);
                        personUpdated = true;
                    }
                }
            }

            if (!phoneFound)
            {
                //Did not find the phone number
                LogRun.AddNotification($"Phone Type not in H/A/W/M for account: {record.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                //Mark it as deleted since the information is not in the session
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            if (personUpdated)
            {
                string phoneTypesComment = "";
                foreach (string phone in updatedPhoneTypes)
                    phoneTypesComment += phone + ",";

                if (phoneTypesComment.Length > 0)
                    phoneTypesComment = phoneTypesComment.Remove(phoneTypesComment.Length - 1);

                ArcAddResults result = LeaveComments(record, Program.ScriptId, updateResults, borrowerSsn, personType, phoneTypesComment);
                if (!result.ArcAdded)
                {
                    LogRun.AddNotification($"Failed to leave arc on account account: {record.AccountNumber}. This needs to be addressed manually at review.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.MarkDeleted(record.FileProcessingId);
                    return false;
                }
                if (!DA.MarkProcessed(record.FileProcessingId, result.ArcAddProcessingId))
                {
                    LogRun.AddNotification($"Unable to mark account as processed account: {record.AccountNumber} arc add id: {result.ArcAddProcessingId}", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                    DA.MarkDeleted(record.FileProcessingId);
                    return false;
                }
            }
            else
            {
                LogRun.AddNotification($"No updates made to account: {record.AccountNumber}, marking record as deleted", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                DA.MarkDeleted(record.FileProcessingId);
            }
            return true;
        }

        private void ClearCTX1J(ReflectionInterface ri)
        {
            ri.PutText(5, 16, "", true);
            ri.PutText(6, 16, "", true);
            ri.PutText(6, 20, "", true);
            ri.PutText(6, 23, "", true);
        }

        private List<string> GetBorrowersForReference(ReflectionInterface ri)
        {
            List<string> values = new List<string>();
            ri.Hit(F6);

            while (ri.GetText(23, 2, 5) != "90007")
            {
                values.Add(ri.GetText(7, 11, 11).Replace(" ", ""));
                ri.Hit(F8);
            }
            return values;
        }

        public bool MobileUpdate(ReflectionInterface ri, FileProcessingRecord record, string phoneType)
        {
            if (!ri.CheckForText(17, 54, "Y"))
            {
                LogRun.AddNotification($"Unable to update phone for account: {record.AccountNumber}; phone type: {phoneType}; phone was not valid.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return false;
            }

            ri.PutText(17, 54, "Y");
            bool isMobile = ri.CheckForText(16, 20, "M");
            ri.PutText(16, 20, !record.MobileIndicator ? "L" : "M");
            ri.PutText(16, 45, $"{DateTime.Now:MMddyy}");

            if (record.HasConsentArc)
                ri.PutText(16, 30, "Y");
            else
            {
                if (ri.CheckForText(16, 20, "L"))
                    ri.PutText(16, 30, "Y");
                else if (isMobile && ri.CheckForText(16, 30, "Y"))
                    ri.PutText(16, 30, "Y");
                else
                    ri.PutText(16, 30, "N");
            }

            ri.PutText(19, 14, "32", Enter);

            if (!ri.CheckForText(23, 2, "01097"))
            {
                LogRun.AddNotification($"Compass: Error Saving Mobile Indicator: {record.AccountNumber}; phone type: {phoneType}; screen code: {ri.ScreenCode}.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return false;
            }

            return true;
        }

        public bool CheckTd22(ReflectionInterface ri, string accountNumber, bool checkEnd)
        {
            string arcToUse = checkEnd ? "TCPAE" : "TCPAB";
            string personTypeToCheck = checkEnd ? "E" : "B";
            ri.FastPath($"TX3Z/ATD22{accountNumber};{arcToUse}");

            if (!ri.CheckForText(1, 72, "TDX24"))
            {
                ri.FastPath("TX3Z/ITX1J");
                ri.PutText(5, 16, personTypeToCheck, true);
                ri.PutText(6, 16, "", true);
                ri.PutText(6, 20, "", true);
                ri.PutText(6, 23, "", true);
                ri.PutText(6, 61, accountNumber, Enter);

                if (ri.CheckForText(1, 71, "TXX1R"))
                    return true;

                return false;
            }
            return true;
        }

        public ArcAddResults LeaveComments(FileProcessingRecord record, string file, string updateResults, string borrowerSsn, string personType, string phoneTypesComment)
        {
            string borrowerAccountNumber = DA.GetAccountNumber(borrowerSsn);

            string[] parts = new string[]
            {
                "CHANGED MOBILE INDICATOR FIELD TO ",
                !record.MobileIndicator ? "L" : "M",
                " FOR PHONE {0} AND PHN TYP(S) {1} BECAUSE OF POSSIBLE NOW UPDATE."
            };

            //PhoneTypesComment tracks the phone types that were actually updated not just reviewed
            string comment = string.Format(string.Join(" ", parts), record.PhoneNumber, phoneTypesComment);

            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion);
            ArcAddResults result;

            arc.AccountNumber = borrowerAccountNumber;
            arc.ArcTypeSelected = ArcData.ArcType.Atd22AllLoans;

            if (personType == "B")
                arc.Arc = "TCPAB";
            else if (personType == "E")
                arc.Arc = "TCPAE";
            else if (personType == "R")
                arc.Arc = "TCPAR";
            else if (personType == "S")
                arc.Arc = "TCPAS";
            else
                arc.Arc = "UNKWN";

            arc.RecipientId = personType == "R" ? borrowerSsn : null;
            arc.Comment = comment;
            arc.ScriptId = Program.ScriptId;

            result = arc.AddArc();
            if (!result.ArcAdded)
            {
                if (personType == "B")
                    LogRun.AddNotification($"Compass: Unable to leave comment in borrower’s {record.AccountNumber} notes: Phone {record.PhoneNumber} Mobile {record.PhoneNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                else if (personType == "E")
                    LogRun.AddNotification($"Compass: Unable to leave comment in endorser’s borrower {borrowerAccountNumber} notes: Account {record.AccountNumber} Phone {record.PhoneNumber} Mobile {record.PhoneNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                else if (personType == "R")
                    LogRun.AddNotification($"Compass: Unable to leave comment in reference's borrower {borrowerAccountNumber} notes: Account {record.AccountNumber} Phone {record.PhoneNumber} Mobile {record.PhoneNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                else if (personType == "S")
                    LogRun.AddNotification($"Compass: Unable to leave comment in student's borrower {borrowerAccountNumber} notes: Account {record.AccountNumber} Phone {record.PhoneNumber} Mobile {record.PhoneNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                else
                    LogRun.AddNotification("Compass: Unknown Person Type", NotificationType.ErrorReport, NotificationSeverityType.Informational);
            }

            return result;
        }
    }
}
