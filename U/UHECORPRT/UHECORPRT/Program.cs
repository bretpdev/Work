using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UHECORPRT
{
    public class Program
    {
        public static readonly string ScriptId = "UHECORPRT";
        public static ProcessLogRun PL { get; set; }
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!DataAccessHelper.StandardArgsCheck(args, "UHECORPRT"))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            PL = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode,false,true);
            var DA = new DataAccess(PL);
            int returnVal = 0;
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid Args provided, the second argument must specify FILELOAD or PRINT");
                return 1;
            }
            else if (args[1] == "PRINT")
            {
                int numberOfProcesses = 15;
                if (args.Length > 2)
                    numberOfProcesses = args[2].ToInt();

                returnVal = new BatchPrinting(numberOfProcesses, DA).RunPrinting();
            }
            else if (args[1] == "FILELOAD")
            {
                returnVal = new FileLoader().RunFileLoader(DA);
            }
            else
            {
                Console.WriteLine("Invalid Args provided, the second argument was {0} but you must specify FILELOAD or PRINT", args[1]);
                return 1;
            }

            Console.WriteLine("Return Value:{0}", returnVal);
            PL.LogEnd();
            return returnVal;
        }
    }
}
