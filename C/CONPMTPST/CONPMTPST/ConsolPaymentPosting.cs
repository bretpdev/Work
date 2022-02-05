using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CONPMTPST
{
    public partial class ConsolPaymentPosting : ScriptBase
    {
        public static ProcessLogRun LogRun;

        public ConsolPaymentPosting(ReflectionInterface ri)
            : base(ri, "CONPMTPST", DataAccessHelper.Region.Uheaa)
        {
            LogRun = new ProcessLogRun(ProcessLogData.ProcessLogId, "CONPMTPST", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true);
            Recovery = new RecoveryLog(ScriptId + "RecoveryLog.txt");
        }

        public override void Main()
        {
            if (DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
            {
                Post = new PaymentPostingData();
                DisplayForm();
            }
        }

        /// <summary>
        /// Creates the PaymentPosting form and passes in a PaymentPostingData object.
        /// </summary>
        public void DisplayForm()
        {
            DA = new DataAccess(LogRun);
            PaymentPosting post = new PaymentPosting(this, DA);
            if (post.ShowDialog() == DialogResult.OK)
                ProcessPayment(DA);
            LogRun.LogEnd();
        }
    }
}