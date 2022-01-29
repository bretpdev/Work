using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ECRBRNTFED
{
    static class Program
    {
        public static SqlConnection Conn { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!DataAccessHelper.StandardArgsCheck(args, "Ecorr Borrowers Notification FED"))
                return 1;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            int procesLogId = ProcessLogger.RegisterApplication("ECRBRNTFED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), false).ProcessLogId;
            Conn = new SqlConnection(DataAccessHelper.GetConnectionString(DataAccessHelper.Database.ECorrFed, DataAccessHelper.CurrentMode));
            Conn.Open();
            int retVal = new Emailer().Process();
            ProcessLogger.LogEnd(procesLogId);
            Conn.Dispose();
            return retVal;
        }
    }
}
