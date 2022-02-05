using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace TCPAPNS
{
    public class UpdateOnelink
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }

        private const int Primary = 13;
        private const int Alternate = 14;
        private const int Other = 15;

        public UpdateOnelink(ProcessLogRun logRun, DataAccess da)
        {
            LogRun = logRun;
            DA = da;
        }

        public bool Update(ReflectionInterface ri, FileProcessingRecord record)
        {
            ri.FastPath("LP22C;");
            ri.PutText(6, 33, record.AccountNumber, Enter);

            if (!ri.CheckForText(22, 3, "46011"))
            {
                LogRun.AddNotification($"Unable to find borrower through quick search through LP22C account: {record.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            ri.PutText(3, 9, "K");

            bool updatedPrimary;
            bool updatedAlternate = false;
            bool updatedOther = false;
            LP50Comment comment = new LP50Comment();

            string borrowerSsn = ri.GetText(3, 23, 9);

            //Update the primary phone if it matches the record's phone
            string primaryPhone = ri.GetText(Primary, 12, 10);
            updatedPrimary = UpdatePhone(ri, record, primaryPhone, Primary, "primary", comment);

            //Update the alternate phone if it matches the record's phone 
            //and the number does not match the primary number
            string altPhone = ri.GetText(Alternate, 12, 10);
            if (altPhone != primaryPhone)
                updatedAlternate = UpdatePhone(ri, record, altPhone, Alternate, "alternate", comment);
            else
            {
                ri.PutText(Alternate, 38, "N");
                comment.Comment += "Alternate number invalidated.";
            }

            //Update the other phone if it matches the record's phone 
            //and the number does not match the primary number
            string otherPhone = ri.GetText(Other, 12, 10);
            if (otherPhone != primaryPhone)
                updatedOther = UpdatePhone(ri, record, otherPhone, Other, "other", comment);
            else
            {
                ri.PutText(Other, 38, "N");
                comment.Comment += "Other number invalidated.";
            }

            ri.Hit(F6);

            if (!ri.CheckForText(22, 3, "49000"))
            {
                string mobileIndicator = record.MobileIndicator ? "M" : "L";
                ri.AddQueueTaskInLP9O(borrowerSsn, "POSSIBLE", null, $"{record.AccountNumber},{record.PhoneNumber},{mobileIndicator}, Unable to save OneLINK demographic changes", "", "", "");
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            if (!updatedOther && !updatedAlternate && !updatedPrimary)
            {
                LogRun.AddNotification($"Phone number not found on OneLink account: {record.AccountNumber} phone: {record.PhoneNumber}", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            if (!AddCommentInLP50(ri, borrowerSsn, false, comment))
            {
                LogRun.AddNotification($"Unable to add LP50 comment for account: {record.AccountNumber} phone: {record.PhoneNumber}", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            if (!DA.MarkProcessed(record.FileProcessingId, null))
            {
                LogRun.AddNotification($"Unable to mark account as processed account: {record.AccountNumber} arc add id: null(onelink)", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            return true;
        }

        public bool UpdateReference(ReflectionInterface ri, FileProcessingRecord record)
        {
            ri.FastPath($"LP2CC;{record.AccountNumber}");

            if (!ri.CheckForText(22, 3, "46011"))
            {
                LogRun.AddNotification($"Unable to find reference through quick search through LP2CC account: {record.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            bool updatedPrimary;
            bool updatedAlternate = false;
            LP50Comment comment = new LP50Comment();

            string primaryPhone = ri.GetText(Primary, 16, 10);
            updatedPrimary = UpdateReferencePhone(ri, record, primaryPhone, Primary, "primary", comment);

            string alternatePhone = ri.GetText(Alternate, 16, 10);
            if (alternatePhone != primaryPhone)
                updatedAlternate = UpdateReferencePhone(ri, record, alternatePhone, Alternate, "alternate", comment);
            else
            {
                ri.PutText(Alternate, 36, "N");
                comment.Comment += "Alternate number invalidated.";
            }

            if (!updatedPrimary && !updatedAlternate)
            {
                string mobileIndicator = record.MobileIndicator ? "M" : "L";
                ri.AddQueueTaskInLP9O(record.AccountNumber, "POSSIBLE", null, $"{record.AccountNumber},{record.PhoneNumber},{mobileIndicator}, Phone number not on file for reference", "", "", "");
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            //Date format MMddyyyy
            ri.PutText(15, 71, record.Date.Substring(0, 2) + record.Date.Substring(3, 2) + record.Date.Substring(6, 4), F6);

            if (!ri.CheckForText(22, 3, "49000"))
            {
                string mobileIndicator = record.MobileIndicator ? "M" : "L";
                ri.AddQueueTaskInLP9O(record.AccountNumber, "POSSIBLE", null, $"{record.AccountNumber},{record.PhoneNumber},{mobileIndicator}, Unable to save OneLINK demographic changes", "", "", "");
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            if (!AddCommentInLP50(ri, record.AccountNumber, false, comment))
            {
                string mobileIndicator = record.MobileIndicator ? "M" : "L";
                ri.AddQueueTaskInLP9O(record.AccountNumber, "POSSIBLE", null, $"{record.AccountNumber},{record.PhoneNumber},{mobileIndicator}, Unable to input note regarding change to reference's demographic information", "", "", "");
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            if (!DA.MarkProcessed(record.FileProcessingId, null))
            {
                LogRun.AddNotification($"Unable to mark account as processed account: {record.AccountNumber} arc add id: null(onelink)", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                DA.MarkDeleted(record.FileProcessingId);
                return false;
            }

            return true;
        }

        public bool UpdateReferencePhone(ReflectionInterface ri, FileProcessingRecord record, string phoneNumber, int phoneLocation, string phoneType, LP50Comment comment)
        {
            if (record.PhoneNumber == phoneNumber && ri.CheckForText(13, 36, "Y"))
            {
                if (record.MobileIndicator && record.HasConsentArcOneLink)
                {
                    ri.PutText(phoneLocation, 46, "P");
                    comment.Comment += $"Changed CONSENT field to P for {phoneType.ToUpper()} phone because of Possible Now update.";
                }
                else if (record.MobileIndicator && !record.HasConsentArcOneLink)
                {
                    ri.PutText(phoneLocation, 46, "N");
                    comment.Comment += $"Changed CONSENT field to N for {phoneType.ToUpper()} phone because of Possible Now update.";
                }
                else if (!record.MobileIndicator)
                {
                    ri.PutText(phoneLocation, 46, "L");
                    comment.Comment += $"Changed CONSENT field to L for {phoneType.ToUpper()} phone because of Possible Now update.";
                }
                else
                    return false;
                return true;
            }
            return false;
        }

        public bool UpdatePhone(ReflectionInterface ri, FileProcessingRecord record, string phoneNumber, int phoneLocation, string phoneType, LP50Comment comment)
        {
            if (record.PhoneNumber == phoneNumber && ri.CheckForText(phoneLocation, 38, "Y"))
            {
                if (record.MobileIndicator && record.HasConsentArcOneLink)
                {
                    ri.PutText(phoneLocation, 68, "P");
                    comment.Comment += $"Changed CONSENT field to P for {phoneType.ToUpper()} phone because of Possible Now update.";
                }
                else if (record.MobileIndicator && !record.HasConsentArcOneLink)
                {
                    ri.PutText(phoneLocation, 68, "N");
                    comment.Comment += $"Changed CONSENT field to N for {phoneType.ToUpper()} phone because of Possible Now update.";
                }
                else if (!record.MobileIndicator)
                {
                    ri.PutText(phoneLocation, 68, "L");
                    comment.Comment += $"Changed CONSENT field to L for {phoneType.ToUpper()} phone because of Possible Now update.";
                }
                else
                    return false;
                return true;
            }
            return false;
        }

        private bool AddCommentInLP50(ReflectionInterface ri, string borrowerSsn, bool isEndorser, LP50Comment comment)
        {
            string arc = string.Empty;

            if (borrowerSsn.Contains("RF@"))
                arc = "MPRUF";
            else if (isEndorser)
                arc = "MPEUF";
            else
                arc = "MPBUF";

            return ri.AddCommentInLP50(borrowerSsn, "ET", "55", arc, comment.Comment, Program.ScriptId);
        }
    }
}