using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace DeskAudits
{
    public partial class AuditMainScreen : Form
    {
        ProcessLogRun LogRun { get; set; }
        DataAccess DA { get; set; }

        public AuditMainScreen(ProcessLogRun logRun, DataAccess da, string userName)
        {
            InitializeComponent();
            LogRun = logRun;
            DA = da;
            UserAccess access = DA.GetUserAccess(userName);
            if (access == null || (!access.SearchAccess.HasValue || !access.SubmitAccess.HasValue) || (access.SearchAccess.HasValue && !access.SearchAccess.Value && access.SubmitAccess.HasValue && !access.SubmitAccess.Value))
            {
                LogRun.AddNotification($"User: {userName} does not have access to log or look up audits. Ending run.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Warning.Ok("You do not have access to log or look up audits. Ending run.");
                LogRun.LogEnd();
                Environment.Exit(1);
            }

            if (access.SearchAccess.Value)
            {
                AuditSearch.InitializeValues(DA, userName, LogRun);
                if (!access.SubmitAccess.Value)
                    TabsControl.TabPages.Remove(SubmitTab);
            }

            if (access.SubmitAccess.Value)
                AuditSubmission.InitializeValues(DA, userName, LogRun);
            
        }

        private void AuditMainScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogRun.LogEnd();
            Environment.Exit(0);
        }
    }
}
