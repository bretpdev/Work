using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace CentralizedPrintingProcess
{
    public interface IEmailHandler
    {
        void AddEmail(string body, string subject, string businessUnit = "", string bsysEmailKey = "", string attachments = "");
    }
    public class InstantEmailHandler : IEmailHandler
    {
        DataAccess da;
        public InstantEmailHandler(DataAccess da)
        {
            this.da = da;
        }
        public void AddEmail(string body, string subject, string businessUnit = "", string bsysEmailKey = "", string attachments = "")
        {
            string recipients = da.GetEmailRecipients(bsysEmailKey, businessUnit);

            EmailHelper.SendMail(DataAccessHelper.TestMode, recipients, "CentralizedPrintingProcess@utahsbr.edu", subject, body, "", EmailHelper.EmailImportance.Normal, true);
        }
    }
}
