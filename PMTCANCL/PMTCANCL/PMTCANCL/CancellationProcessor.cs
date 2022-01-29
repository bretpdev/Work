using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PMTCANCL
{
    class CancellationProcessor
    {
        private DataAccess DA { get; set; }
        private UserValidator validator { get; set; }
        private ProcessLogRun LogRun { get; set; }
        public CancellationProcessor(DataAccess DA, UserValidator validator, ProcessLogRun logRun)
        {
            this.DA = DA;
            this.validator = validator;
        }

        public void Process()
        {
            UserQuery userQuery = null;
            PaymentInfo toDelete = null;
            while (toDelete == null || toDelete.BackResult)
            {
                userQuery = QueryUser(userQuery);
                if (userQuery == null)
                {
                    return;
                }

                toDelete = GetAndDisplayQuery(userQuery);
                if (toDelete == null)
                {
                    return;
                }
                if(!toDelete.BackResult)
                {
                    DeleteSelected(toDelete, userQuery);
                    toDelete.BackResult = true;
                    if(MessageBox.Show("Would like to exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        return;
                    }
                }            
            }
        }

        public UserQuery QueryUser(UserQuery query)
        {
            UserQuery userQuery = query == null ? new UserQuery() : query;
            UserQueryForm userQueryForm = new UserQueryForm(userQuery, validator);
            using (userQueryForm)
            {
                if (userQueryForm.ShowDialog() == DialogResult.Cancel)
                {
                    return null;
                }
            }
            return userQuery;
        }

        public PaymentInfo GetAndDisplayQuery(UserQuery userQuery)
        {
            bool isSsn = true;
            if(userQuery.borrower != null)
            {
                isSsn = userQuery.borrower.Length == 9 ? true : false;
            }
            List<PaymentInfo> paymentInfo;
            if (userQuery.region == DataAccessHelper.Region.Uheaa)
            {
                paymentInfo = DA.GetPaymentsUheaa(userQuery.processed, userQuery.madeAfter, isSsn, userQuery.borrower);
            }
            else
            {
                paymentInfo = DA.GetPaymentsCornerstone(userQuery.processed, userQuery.madeAfter, isSsn, userQuery.borrower);
            }
            paymentInfo = paymentInfo == null ? new List<PaymentInfo>() : paymentInfo;

            UserQueryResultsForm userQueryResultsForm = new UserQueryResultsForm(paymentInfo);
            using (userQueryResultsForm)
            {
                DialogResult result = userQueryResultsForm.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return null;
                }
                else if(result == DialogResult.Retry)
                {
                    var ret = new PaymentInfo();
                    ret.BackResult = true;
                    return ret;
                }
            }
            if (paymentInfo.Count > 1)
            {
                LogRun.AddNotification("User tried to delete more than one entry per execution. User: " + Environment.UserName, NotificationType.HandledException, NotificationSeverityType.Warning);
                throw new InvalidOperationException("No permission to delete more than one entry per execution.");
            }

            return paymentInfo.FirstOrDefault();
        }

        public void DeleteSelected(PaymentInfo toDelete, UserQuery userQuery)
        {
            bool success = false;
            if (userQuery.region == DataAccessHelper.Region.Uheaa)
            {
                long id;
                long.TryParse(toDelete.Conf, out id);
                success = DA.SetDeletedPendingPaymentUheaa(id);
            }
            else
            {
                int id;
                int.TryParse(toDelete.Conf, out id);
                success = DA.SetDeletedPendingPaymentCornerstone(id);
            }

            if (success)
            {
                MessageBox.Show("The specified payment was marked deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            else
            {
                MessageBox.Show("The specified payment was NOT marked deleted", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
