using System.Windows.Forms;
using Uheaa.Common.Scripts;

namespace INVOSAMPFD
{
    public class FsaInvoiceSampling : FedScript
    {
        public FsaInvoiceSampling(ReflectionInterface ri) : base(ri, "INVOSAMPFD") { }
        public override void Main()
        {
            Application.EnableVisualStyles();
            new StatusForm(RI, ProcessLogData).ShowDialog();
        }
    }
}
