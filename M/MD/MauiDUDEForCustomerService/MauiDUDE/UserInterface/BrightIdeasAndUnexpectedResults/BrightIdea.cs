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
    public partial class BrightIdea : Form
    {
        protected UnexpectedResultsAndBrightIdeaCoordinator _processor = new UnexpectedResultsAndBrightIdeaCoordinator();

        public static void ShowBrightIdea()
        {
            using(BrightIdea brightIdea = new BrightIdea())
            {
                brightIdea.ShowDialog();
            }
        }

        public BrightIdea()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtComments.Text.Replace(" ", "") == "")
            {
                WhoaDUDE.ShowWhoaDUDE("Message was empty. Please add comments.", "Empty Message", true);
                return;
            }
            _processor.BrightIdeaProcessing(txtComments.Text);
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
