using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public partial class UnexpectedResults : Form
    {
        protected UnexpectedResultsAndBrightIdeaCoordinator _processor = new UnexpectedResultsAndBrightIdeaCoordinator();

        public static void ShowUnexpectedResults()
        {
            using(UnexpectedResults results = new UnexpectedResults())
            {
                results.ShowDialog();
            }
        }

        public UnexpectedResults()
        {
            KnarlyDUDE.ShowKnarlyDude("Make sure that your Reflection Session is on the correct screen and OK to send a Screen Shot and Message to the Big Kahuna.", "MauiDUDE");
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(txtComments.Text.Replace(" ", "") == "")
            {
                WhoaDUDE.ShowWhoaDUDE("Message was empty. Please add comments.", "Empty Message", true);
                return;
            }
            _processor.UnexpectedResultsProcessing(txtComments.Text);
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
