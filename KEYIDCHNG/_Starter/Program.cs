using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            var run = new ProcessLogRun("IDKEYCHNG", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            var ri = new ReflectionInterface();
            var login = BatchProcessingLoginHelper.Login(run, ri, run.ScriptId, "BatchUheaa");
            new KEYIDCHNG.KeyIdentifierChangeScript(ri).Main();
        }
    }
}
