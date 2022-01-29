using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace PHONESUCSN
{
    public class UpdateHelper
    {
        public ReflectionInterface RI { get; set; }
        public DataAccess DA { get; set; }
        public List<PhoneData> Data { get; set; }

        public UpdateHelper(ReflectionInterface ri, DataAccess da)
        {
            RI = ri;
            DA = da;
            Data = new List<PhoneData>();
        }

        /// <summary>
        /// Updates the home phone with the new phone number being passed in.
        /// </summary>
        public bool UpdatePhone(UpdateData data)
        {
            string message = $"Updating phone type {data.ToType} from phone type {data.FromType} for {(data.IsEndorser ? "endorser" : "borrower")}: {data.Ssn} to phone number: {data.Phone}.";
            WriteLine(message);
            RI.FastPath($"TX3ZCTX1J{(data.IsEndorser ? "E" : "B")};{data.Ssn}");
            if (RI.MessageCode.IsIn("01019", "01080"))
            {
                message = $"updating phone because the {(data.IsEndorser ? "endorser" : "borrower")} is not found: {data.Ssn}. {RI.Message}";
                LogError(data.PhoneSuccessionId, message);
                return false;
            }
            RI.Hit(F6, 3);
            RI.PutText(16, 14, data.ToType, Enter);
            if (!RI.CheckForText(17, 54, data.IsValid))
            {
                RI.PutText(16, 20, data.Ind);
                RI.PutText(16, 30, data.Consent);
                RI.PutText(16, 45, data.VerifiedDate.To6DigitDate());
                if (data.IsForeign)
                {
                    ClearPhone();
                    RI.PutText(18, 15, data.Phone);
                }
                else
                {
                    ClearForeignPhone();
                    RI.PutText(17, 14, data.Phone);
                }
                RI.PutText(17, 40, data.Ext.Trim(), true);
                RI.PutText(19, 14, data.Src);
                RI.PutText(17, 54, data.IsValid, Enter);
                if (!RI.MessageCode.IsIn("01097", "01100"))
                {
                    LogError(data.PhoneSuccessionId, message);
                    return false;
                }
                Data.Remove(Data.Where(p => p.PhoneSuccessionId == data.PhoneSuccessionId).SingleOrDefault());
            }
            DA.MarkProcessed(data.PhoneSuccessionId);
            return true;
        }

        /// <summary>
        /// Moves the invalid phone number up to the location where the valid phone number was
        /// </summary>
        public bool MoveInvalidPhone(UpdateData data)
        {
            string message = $"Moivng the invlaid phone type {data.FromType} to phone type {data.ToType} for {(data.IsEndorser ? "endorser" : "borrower")}: {data.Ssn} to phone number: {data.Phone}.";
            WriteLine(message);
            RI.FastPath($"TX3ZCTX1J{(data.IsEndorser ? "E" : "B")};{data.Ssn}");
            RI.Hit(F6, 3);
            RI.PutText(16, 14, data.ToType, Enter);
            RI.PutText(16, 20, data.Ind);
            RI.PutText(16, 30, data.Consent);
            RI.PutText(16, 45, data.VerifiedDate.To6DigitDate());
            if (data.IsForeign)
            {
                ClearPhone();
                RI.PutText(18, 15, data.Phone);
            }
            else
            {
                ClearForeignPhone();
                RI.PutText(17, 14, data.Phone);
            }
            RI.PutText(17, 40, data.Ext.Trim(), true);
            RI.PutText(19, 14, data.Src);
            RI.PutText(17, 54, data.IsValid, Enter);
            if (!RI.MessageCode.IsIn("01097", "01100"))
            {
                LogError(data.PhoneSuccessionId, message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Invalidates the phone number
        /// </summary>
        public bool Invalidate(UpdateData data)
        {
            string message = $"Invalidating phone for borrower: {data.Ssn}; Number: {data.Phone}; Phone Type:{data.FromType}.";
            WriteLine(message);
            RI.FastPath($"TX3ZCTX1J{(data.IsEndorser ? "E" : "B")};{data.Ssn}");
            if (RI.MessageCode.IsIn("01080", "01019"))
            {
                message = $"invalidating phone because the {(data.IsEndorser ? "endorser" : "borrower")} is not found: {data.Ssn}. {RI.Message}";
                LogError(data.PhoneSuccessionId, message);
                return false;
            }
            RI.Hit(F6, 3);
            RI.PutText(16, 14, data.FromType, Enter);
            RI.PutText(16, 20, data.Ind);
            RI.PutText(16, 30, data.Consent);
            RI.PutText(16, 45, data.VerifiedDate.To6DigitDate());
            RI.PutText(19, 14, data.Src);
            RI.PutText(17, 54, "N", Enter);
            if (RI.MessageCode != "01097")
            {
                LogError(data.PhoneSuccessionId, message);
                return false;
            }
            DA.MarkInvalidatedAt(data.PhoneSuccessionId);
            return true;
        }

        /// <summary>
        /// Logs an error to the console, Process Logger and updates the record in the database as having an error
        /// </summary>
        public void LogError(int phoneSuccessionId, string message)
        {
            message = $"There was an error {message}";
            RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            DA.LogError(phoneSuccessionId);
        }

        public void AddComments(UpdateData data, string comment)
        {
            string borSsn = data.IsEndorser ? DA.GetBorrowerSsn(data.Ssn) : data.Ssn;
            string endSsn = data.IsEndorser ? data.Ssn : borSsn; //Use the endorser ssn if it is an endorser or the bor ssn if not
            string regardsCode = data.IsEndorser ? "E" : "B";
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = borSsn,
                Arc = "PHNSC",
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                RecipientId = endSsn,
                RegardsCode = regardsCode,
                RegardsTo = endSsn,
                ScriptId = Program.ScriptId
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string borType = data.IsEndorser ? $"to borrower account: {borSsn} for endorser: {endSsn}" : $"for borrower: {borSsn}";
                string message = $"There was an error adding the PHNSC ARC {borType}. EX: {result.Ex}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, result.Ex);
            }
        }

        private void ClearPhone()
        {
            RI.PutText(17, 14, "", true);
            RI.PutText(17, 23, "", true);
            RI.PutText(17, 31, "", true);
            RI.PutText(17, 40, "", true);
        }

        private void ClearForeignPhone()
        {
            RI.PutText(18, 15, "", true);
            RI.PutText(18, 24, "", true);
            RI.PutText(18, 36, "", true);
            RI.PutText(18, 53, "", true);
        }
    }
}