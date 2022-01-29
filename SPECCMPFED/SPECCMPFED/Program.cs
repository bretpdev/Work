using System;
using System.IO;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SPECCMPFED
{
	class Program
	{
		[STAThread]
		static int Main(string [] args)
		{
            if (!DataAccessHelper.StandardArgsCheck(args, "SPECCMPFED"))
                return 1;
            //UNDONE: You need to check for the sprocs access

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
           
            bool calledByJams = args.Length > 1;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            ProcessLogger.RegisterApplication("SPECCMPFED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), false);
		
            int exitCode =  new CreateFile(calledByJams).Run();
			Environment.Exit(exitCode);
			return exitCode;
		}


	}
}
