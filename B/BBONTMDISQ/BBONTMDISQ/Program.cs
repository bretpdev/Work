using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;



namespace BBONTMDISQ
{
    public class Program
    {
        private const string scriptId = "BBONTMDISQ";

        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, scriptId, false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine($"Borrower Benefits Disqualification :: Version {version.Major}:{version.Minor}:{version.Build}:{version.Revision}");
            Console.WriteLine("");

            ProcessLogRun logRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            int process = new DisqualificationProcess(logRun, scriptId).Process();
            Console.WriteLine($"Return value:{process}");
            Console.WriteLine("Script Complete, logging end of process logger.");
            return process;
        }
    }
}