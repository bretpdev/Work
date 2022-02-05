using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;
using Flag = Uheaa.Common.Scripts.ReflectionInterface.Flag;

namespace LOCATE
{
    class Program
    {
        public static readonly string ScriptId = "LOCATE";
        public static ProcessLogData LogData { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                    return;

                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                LogData = ProcessLogger.RegisterApplication("LOCATE", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
                new Program().Run();
        }

        /// <summary>
        /// Starting point for the app 
        /// </summary>
        public void Run()
        {
            ReflectionInterface ri = new ReflectionInterface(Flag.OpenSession);
            if (ri.ReflectionSession == null)
            {
                Dialog.Error.Ok("Unable to connect to an open session.  Please open a new session and try again.");
                return;
            }
            if (ri.ValidateRegion(DataAccessHelper.Region.CornerStone, false))//Since this app runs in both the app will set the region based upon what the user is logged in as.
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            else
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;

            string accountIdentifer = string.Empty;
            using (InputBox<AccountIdentifierTextBox> input = new InputBox<AccountIdentifierTextBox>("", "Enter Account Number or SSN"))
            {
                if (input.ShowDialog() == DialogResult.OK)
                    accountIdentifer = input.InputControl.Text;
                else
                    return;
            }

            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
                new UheaaProcessing(ri, accountIdentifer, LogData).Process();
            else
                new CornerStoneProcessing(ri, accountIdentifer, LogData).Process();

            ProcessLogger.LogEnd(LogData.ProcessLogId);        
        }
    }
}
