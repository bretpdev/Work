using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace _Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            var ri = new ReflectionInterface();
            ri.WaitForText(16, 2, "LOGON", 10);
            ri.Login(args[0], args[1]);
            ri.ProcessLogData = ProcessLogger.RegisterScript("ACCURINT", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            new ACCURINT.Accurint(ri).Main();
        }
    }
}
