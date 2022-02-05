using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace AACEMAIL
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            string scriptId = "AACEMAIL";
            if (!DataAccessHelper.StandardArgsCheck(args, scriptId))
                return 1;

            ProcessLogData logData = ProcessLogger.RegisterApplication(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());

            int count = new Process(scriptId, logData).Run();
            if (count == 0)
                Console.WriteLine("Processing complete");
            Thread.Sleep(5000);
            ProcessLogger.LogEnd(logData.ProcessLogId);
            return count;
        }
    }
}