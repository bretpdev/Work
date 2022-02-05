using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CPINTRTLPD
{
    public class CPINTRTLPD : ScriptBase
    {
        const string SCRIPTID = "CPINTRTLPD";
        public CPINTRTLPD(ReflectionInterface ri)
            : base(ri, SCRIPTID)
        { }

        public override void Main()
        {
            var form = new MainForm();
            form.ShowDialog();
            if (form.Input != null)
            {
                LogForm logForm = new LogForm();
                var logTask = Task.Run(() => logForm.ShowDialog());
                ProcessLogData pld = ProcessLogger.RegisterScript(SCRIPTID, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
                Logger log = new Logger(pld, s => { logForm.AddItem(s); }, s => { logForm.AddItem(s); });
                logForm.AddItem("Process Log ID: " + pld.ProcessLogId);
                Processor processor = new Processor(RI, log, form.Input);
                processor.Process();
                logForm.AddItem("Processing Complete");
                Task.WaitAll(logTask);
            }
        }
    }
}
