using SubSystemShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace NeedHelp
{
    class LibraryStarter
    {
        private static ProcessLogRun LogRun { get; set; }

        [STAThread]
        public static void Main(string[] args)
        {
            Repeater.TryRepeatedly(() => //Attempting to restart if there is a threading error
            {
                if (DataAccessHelper.StandardArgsCheck(args, "NEEDHELP"))
                {
                    RemoveSearchEXE();
                    Application.EnableVisualStyles();
                    LogRun = new ProcessLogRun("NEEDHELP", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);

                    SqlUser user = GetUser();
                    string role = Common.AuthenticateUser(user.WindowsUserName, LogRun);
                    SearchAndResultsProcessor processor = new SearchAndResultsProcessor(user, role, LogRun);
                    processor.Start();
                    DataAccessHelper.CloseAllManagedConnections();
                }
            }, 2);
        }

        private static SqlUser GetUser()
        {
            DataAccess DA = new DataAccess(LogRun.LDA);
            List<SqlUser> users = DA.GetSqlUsers(false);
            SqlUser user = DA.GetSqlUsers(false).Where(p => p.WindowsUserName.ToLower() == Environment.UserName.ToLower()).FirstOrDefault();
            return user;
        }

        private static void RemoveSearchEXE()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            string searchPath = EnterpriseFileSystem.GetPath("NeedHelpSearch");
            if (File.Exists(searchPath))
                Repeater.TryRepeatedly(() => File.Delete(searchPath));
        }
    }
}