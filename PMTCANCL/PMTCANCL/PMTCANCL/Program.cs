using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;

namespace PMTCANCL
{
    class Program
    {
        public readonly static string ScriptId = "PMTCANCL";

        static void Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false) || args.Length < 1)
            {
                MessageBox.Show("Make sure to provide a region as an argument. Please contact systems support with this information.", "Bad Request", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Output version
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine(string.Format("PMTCANCL user script :: Version {0}:{1}:{2}:{3}", version.Major, version.Minor, version.Build, version.Revision));
            Console.WriteLine("");

            //Set up data access for pre script user validation
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            LogDataAccess LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, false);
            DataAccess DA = new DataAccess(LDA);

            UserValidator validator = new UserValidator(DA, logRun);
            if(!validator.ValidateUser())
            {
                //User does not have access to this script
                return;
            }

            CancellationProcessor cp = new CancellationProcessor(DA, validator,logRun);
            cp.Process();
        }
    }
}
