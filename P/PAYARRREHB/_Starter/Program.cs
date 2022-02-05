using System;
using PAYARRREHB;
using System.Reflection;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;


namespace _Starter
{
    class Program
    {
        public static int Main(string[] args)
        { 

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ReflectionInterface RI = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun("PAYARRREHB", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev);
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, RI, "PAYARRREHB", "BatchUheaa");

            PayArrangeRehab par = new PayArrangeRehab(RI);
            par.Main();

            BatchProcessingHelper.CloseConnection(helper);
            return 0;
        }
    }
}
