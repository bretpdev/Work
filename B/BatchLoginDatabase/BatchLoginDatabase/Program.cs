using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace BatchLoginDatabase
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if(!DataAccessHelper.StandardArgsCheck(args, "Batch Login Database"))
                return;
            if (!CheckAccess())
                return;

            Application.Run(new UserNameSelection());
        }

        private static bool CheckAccess()
        {
            if (!CheckActiveDirectoryGroup())
            {
                string emailBody = string.Format("UserID: {0} tried to access the Batch Login Database Application on {1}", Environment.UserName, DateTime.Now);
                EmailHelper.SendMail(DataAccessHelper.TestMode, "SSHELP@utahsbr.edu;", "Batch Login Database", "Unauthorized Batch Login Database Access Attempt", emailBody, "SECURITY@utahsbr.edu", "", EmailHelper.EmailImportance.High, true);
                MessageBox.Show("You do not have access rights to this script/database. Please contact System Support to inquire about this access.", "No Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private static bool CheckActiveDirectoryGroup()
        {
            DirectorySearcher searcher = new DirectorySearcher()
            {
                SearchRoot = new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local"),
                Filter = string.Format("SAMAccountName={0}", Environment.UserName)
            };

            SearchResult result = searcher.FindOne();

            if (result != null)
                return CheckSearchResults(result);

            return false;
        }

        private static bool CheckSearchResults(SearchResult result)
        {
            ResultPropertyCollection attributes = result.Properties;
            int developerCount = attributes["memberOf"].OfType<string>().Where(p => p.Contains("Developers")).Select(p => p).Count();
            int analystCount = attributes["memberOf"].OfType<string>().Where(p => p.Contains("SystemAnalysts")).Select(p => p).Count();

            if (DataAccessHelper.TestMode && (analystCount > 0 || developerCount > 0))
                return true;
            else if (!DataAccessHelper.TestMode && analystCount > 0)//If this is in Live then only system analyst should have access
                return true;
            else//anyone else should not have access
                return false;
        }
    }
}
