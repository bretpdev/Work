using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CommentAuditTracker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (Assembly.GetExecutingAssembly().Location.StartsWith("Y:"))
            {
                MessageBox.Show("This application should not be run from a network drive.  Please copy it to your local machine before running.");
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (DataAccessHelper.StandardArgsCheck(args, "Comment Audit Tracker"))
            {
#if !DEBUG
                ProcessLogger.RegisterExceptionOnly("CommentAuditTracker", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
#endif
                Application.Run(new AgentForm());
            }
        }
    }
}
