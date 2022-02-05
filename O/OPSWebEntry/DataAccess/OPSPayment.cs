using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace OPSWebEntry
{
    [Serializable]
    public class OPSPayment
    {
        public int ID { get; set; }
        public string SSN { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string ABA { get; set; }
        public string BankAccountNumber { get; set; }
        //TODO: Convert to enum
        public string AccountType { get; set; }
        public decimal Amount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string AccountHolderName { get; set; }
        public DateTime? ProcessedDate { get; set; }

        /// <summary>
        /// Updates the database and marks this record as processed.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Norad, "spCKPH_UpdateRecordToProcessed")]
        public void MarkAsProcessed()
        {
            DataAccessHelper.Execute("spCKPH_UpdateRecordToProcessed", DataAccessHelper.Database.Norad, this.SqlParameters("ID"));
        }

        /// <summary>
        /// Updates the database and marks this record as skipped (by setting the date to 2001, which is terrible).
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Norad, "spCKPH_UpdateRecordToBeSkipped")]
        public void MarkAsSkipped()
        {
            DataAccessHelper.Execute("spCKPH_UpdateRecordToBeSkipped", DataAccessHelper.Database.Norad, this.SqlParameters("ID"));
        }

        /// <summary>
        /// Updates the database and adds this record.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Norad, "spCKPH_AddCheckByPhoneEntry")]
        public void Insert()
        {
            DataAccessHelper.Execute("spCKPH_AddCheckByPhoneEntry", DataAccessHelper.Database.Norad,
                new
                {
                    SSN = SSN,
                    Name = Name,
                    DOB = DOB.ToShortDateString(),
                    ABA = ABA,
                    BankAccountNumber = BankAccountNumber,
                    AccountType = AccountType,
                    Amount = Amount,
                    EffectiveDate = EffectiveDate.ToShortDateString(),
                    AccountHolderName = AccountHolderName
                }.SqlParameters());
        }

        [UsesSproc(DataAccessHelper.Database.Norad, "spCKPH_GetCheckByPhonesToBeProcessed")]
        public static List<OPSPayment> GetPendingPayments()
        {
            var payments = DataAccessHelper.ExecuteList<OPSPayment>("spCKPH_GetCheckByPhonesToBeProcessed", DataAccessHelper.Database.Norad);

            foreach (var p in payments)
                if (p.BankAccountNumber.EndsWith("=="))
                    p.BankAccountNumber = LegacyCryptography.Decrypt(p.BankAccountNumber, LegacyCryptography.Keys.NoradOPS);
            return payments;

        }

        [UsesSproc(DataAccessHelper.Database.Norad, "spCKPH_GetLast7DaysOfData")]
        public static IEnumerable<OPSPayment> GetLast7Days()
        {
            foreach (var p in DataAccessHelper.ExecuteList<OPSPayment>("spCKPH_GetLast7DaysOfData", DataAccessHelper.Database.Norad))
            {
                if (p.BankAccountNumber.EndsWith("=="))
                    p.BankAccountNumber = LegacyCryptography.Decrypt(p.BankAccountNumber, LegacyCryptography.Keys.NoradOPS);
                yield return p;
            }
        }
    }
}
