using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PMTCANCL
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public DataAccess(LogDataAccess LDA)
        {
            this.LDA = LDA;
        }

        public List<UserRole> GetRoles()
        {
            return LDA.ExecuteList<UserRole>("pmtcancl.GetRoles", DataAccessHelper.Database.Csys).Result;
        }

        public List<UserRole> GetUserID()
        {
            return LDA.ExecuteList<UserRole>("dbo.GetActiveUsersRole", DataAccessHelper.Database.Csys, SqlParams.Single("WindowsUserId", Environment.UserName)).Result;
        }

        public List<PaymentInfo> GetPaymentsCornerstone(bool? processed, DateTime madeAfter, bool isSsn, string accountIdentifier)
        {
            object processedParam = processed.HasValue ? (object)processed.Value : DBNull.Value;
            object accountIdentifierParam = accountIdentifier != null ? (object)accountIdentifier : DBNull.Value;
            return LDA.ExecuteList<PaymentInfo>("dbo.GetPendingPayments", DataAccessHelper.Database.Cls, new SqlParameter("Processed", processedParam), new SqlParameter("MadeAfter", madeAfter.ToShortDateString()),
                new SqlParameter("IsSsn", isSsn), new SqlParameter("AccountIdentifier", accountIdentifierParam)).Result;
        }

        public string GetSsnFromAccountNumber(string accountNumber)
        {
            return LDA.ExecuteSingle<string>("dbo.spGetSSNFromAcctNumber", DataAccessHelper.Database.Udw, SqlParams.Single("AccountNumber", accountNumber)).Result;
        }

        public List<PaymentInfo> GetPaymentsUheaa(bool? processed, DateTime madeAfter, bool isSsn, string accountIdentifier)
        {
            string Ssn;
            if(accountIdentifier == null)
            {
                Ssn = null;
            }
            else if(accountIdentifier.Length == 10)
            {
                Ssn = GetSsnFromAccountNumber(accountIdentifier);
            }
            else if(accountIdentifier.Length == 9)
            {
                Ssn = accountIdentifier;
            }
            else
            {
                MessageBox.Show("Account Identifier was not 9 or 10 characters, either provide a valid social or account number", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new FormatException("Account Identifier was not 9 or 10 characters");
            }

            List<PaymentInfo> paymentInfo = new List<PaymentInfo>();
            List<DemographicInfo> demoInfo = new List<DemographicInfo>();

            object processedParam = processed.HasValue ? (object)processed.Value : DBNull.Value;
            object SsnParam = Ssn != null ? (object)Ssn : DBNull.Value;
            paymentInfo = LDA.ExecuteList<PaymentInfo>("dbo.GetPendingPaymentsNochouse", DataAccessHelper.Database.Norad, SqlParams.Single("Processed", processedParam), SqlParams.Single("MadeAfter", madeAfter.ToShortDateString()),
                SqlParams.Single("Ssn", SsnParam)).Result;
            demoInfo = LDA.ExecuteList<DemographicInfo>("dbo.GetDemographicsForPendingPayments", DataAccessHelper.Database.Udw, SqlParams.Single("Ssn", SsnParam)).Result;
            
            IEnumerable<PaymentInfo> result = 
                    from pinfo in paymentInfo
                    join dinfo in demoInfo
                    on pinfo.Ssn equals dinfo.Ssn
                    select new PaymentInfo
                    {
                        Conf = pinfo.Conf,
                        Ssn = dinfo.Ssn,
                        AccountNumber = dinfo.AccountNumber,
                        Borrower = dinfo.Borrower,
                        PayType = pinfo.PayType,
                        PayAmt = pinfo.PayAmt,
                        PaySource = pinfo.PaySource,
                        PayCreated = pinfo.PayCreated,
                        PayEffective = pinfo.PayEffective,
                        ProcessedDate = pinfo.ProcessedDate,
                        Deleted = pinfo.Deleted
                    };
            return new List<PaymentInfo>(result.OrderBy(p => int.Parse(p.Conf)));
        }

        public bool SetDeletedPendingPaymentUheaa(long id)
        {
            return LDA.Execute("dbo.SetDeletedPendingPayment", DataAccessHelper.Database.Norad, SqlParams.Single("RecNo", id));
        }

        public bool SetDeletedPendingPaymentCornerstone(int id)
        {
            return LDA.Execute("dbo.SetDeletedPendingPayment", DataAccessHelper.Database.Cls, SqlParams.Single("RecNo", id));
        }
    }
}
