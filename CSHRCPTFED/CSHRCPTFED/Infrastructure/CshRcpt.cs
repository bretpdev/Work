using System;
using CSHRCPTFED.ViewModels;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common;


namespace CSHRCPTFED.Infrastructure
{
    public class CashReceiptProcessor
    {
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }
        private string ARC { get; } = "CASHR";

        public CashReceiptProcessor(DataAccess da, ProcessLogRun plr)
        {
            DA = da;
            PLR = plr;

        }
        public string Process(CashReceiptModel model, AccountIdentifiers identifier, string username)
        {
            username = username.Replace("UHEAA\\", "");
            var comment = $"Check# {model.CheckId} for ${model.Amount:C} received on {model.CheckDate.ToShortDateString()} from {model.Payee} has been forwarded to lockbox.";
            int? arcAddId = null;
            if (identifier.AccountNumber != AccountIdentifiers.NoAcountNumber)
            {
                arcAddId = ArcAdd(identifier.AccountNumber, comment);
                if (arcAddId == null)
                    return null;
            }

            int? checkRecordId = DA.LoadRecord(identifier.AccountNumber, model.BorrowerName, model.Amount.ToDecimal(), model.CheckId, model.Payee, model.CheckDate, arcAddId, username);
            if (checkRecordId == null)
            {
                PLR.AddNotification($"Unable to process cash receipt for account {identifier.AccountNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }

            return comment;
        }

        private int? ArcAdd(string accountNumber, string comment)
        {
            ArcData arcd = new ArcData(DataAccessHelper.Region.CornerStone)
            {
                Arc = ARC,
                Comment = comment,
                ScriptId = "CSHRCPTFED",
                AccountNumber = accountNumber,
                ProcessOn = DateTime.Now,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
            };

            ArcAddResults result = arcd.AddArc();
            if (!result.ArcAdded)
            {
                PLR.AddNotification($"[CSHRCPTFED] Failed to add arc. Account: {accountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }
            return result.ArcAddProcessingId;
        }
    }


}