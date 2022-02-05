using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace CentralizedPrintingProcess
{
    public partial class StatusForm : Form
    {
        public MiscDat MD { get; set; }

        public StatusForm(MiscDat md)
        {
            InitializeComponent();
            MD = md;
            Text = $"{Text}  :: Version:{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void StatusForm_Shown(object sender, EventArgs e)
        {
            var util = new ComUtilities(
                MD,
                PrintingStatus,
                FaxingStatus,
                EmailHandlerControl
            );
            EmailHandlerControl.DataAccess = util.DA;

            bool donePrinting = false;
            bool doneFaxing = false;
            var processingComplete = new Action(() =>
            {
                if (donePrinting && doneFaxing)
                {
                    BeginInvoke(
                        new Action(() =>
                            {
                            Text = "Centralized Printing - COMPLETE as of " + DateTime.Now.ToString();
                            }
                        )
                    );
                    util.LogRun.LogEnd();
                    DataAccessHelper.CloseAllManagedConnections();
                }
            });

            Task.Factory.StartNew(() =>
            {
                util.Printer.Process();
                donePrinting = true;
                processingComplete();
            });
            Task.Factory.StartNew(() =>
            {
                util.Fax.Process();
                doneFaxing = true;
                processingComplete();
            });
        }
    }
}