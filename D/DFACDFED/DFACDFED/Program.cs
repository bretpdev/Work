using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace DFACDFED
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            if (DataAccessHelper.StandardArgsCheck(args, "DFACDFED"))
            {
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
                DateTime today = args.Skip(1).FirstOrDefault().ToDateNullable() ?? DateTime.Now;
                var data = ProcessLogger.RegisterApplication("DFACDFED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
                var processor = new Processor(data);
                new ResultsForm(processor, today).ShowDialog();
            }
        }
    }
}
