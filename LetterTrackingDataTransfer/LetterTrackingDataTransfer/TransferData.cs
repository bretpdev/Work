using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace LetterTrackingDataTransfer
{
    public partial class TransferData : Form
    {
        public DataAccess DA { get; set; }
        List<CentralPrintingDocData> DocData { get; set; }
        public Process Proc { get; set; }

        public TransferData(Process proc)
        {
            InitializeComponent();
            DA = new DataAccess();
            Proc = proc;

            DocData = DA.GetLiveLetterIds().Where(i => i.ID.IsPopulated()).OrderBy(p => p.ID).ToList();
            DocData.Insert(0, new CentralPrintingDocData());
            LetterId.DataSource = DocData;
            LetterId.DisplayMember = "ID";
            LetterId.ValueMember = "DocSeqNo";
        }

        private void OK_Click(object sender, System.EventArgs e)
        {
            if (LetterId.Text.IsPopulated())
            {
                if (DA.GetLiveLetterIds(DataAccessHelper.Mode.Dev).Any(p => p.ID == LetterId.Text)) //Letter is already in test
                {
                    if (MessageBox.Show("This letter is already in test. Do you want to update the letter data?", "Letter Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        Proc.AddLetterToEcorr();
                        return;
                    }
                }
                if (Proc.AddLetterData())
                {
                    Transfered.Text = "Transfered";
                    Transfered.Visible = true;
                    Proc.AddLetterToEcorr();
                }
                else
                {
                    Transfered.Text = "Error Transferring";
                    Transfered.Visible = false;
                }
            }
        }

        private void LetterId_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Transfered.Visible = false;
        }
    }
}