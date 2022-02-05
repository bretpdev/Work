using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace CONPMTPST
{
    public partial class PaymentPosting : Form
    {
        private ConsolPaymentPosting PayPost;
        public List<PaymentSources> Sources { get; set; }
        public DataAccess DA { get; set; }

        public PaymentPosting(ConsolPaymentPosting payPost, DataAccess da)
        {
            InitializeComponent();
            PayPost = payPost;
            DA = da;

            Sources = DA.GetPaymentSources();
            Sources.Insert(0, new PaymentSources());
            Source.DataSource = Sources;
            Source.DisplayMember = "PaymentSource";
            Source.ValueMember = "PaymentSourcesId";

            Setup.Enabled = DA.CheckManagerAccess();
        }

        /// <summary>
        /// Validates all data input and sets the PaymentPostingData object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_Click(object sender, EventArgs e)
        {
            List<string> missingData = new List<string>();
            if (Amount.Text.ToDouble() > 0)
                PayPost.Post.PaymentAmount = Amount.Text.ToDouble();
            else
                missingData.Add("Total Amount Received");

            if (Cash.Checked || Wire.Checked)
                PayPost.Post.IsCash = Cash.Checked;
            else
                missingData.Add("Payment Type");

            if (!Source.Text.IsNullOrEmpty())
                PayPost.Post.PaymentSource.PaymentSource = Source.Text;
            else
                missingData.Add("Source of Consolidation Payment");

            if (missingData.Count > 0)
                NotifyUser(missingData);
            else
            {
                PayPost.Post.PaymentSource.FileName = Sources.Where(p => p.PaymentSource == Source.Text).Select(p => p.FileName).Single();
                PayPost.Post.PaymentSource.InstitutionId = Sources.Where(p => p.PaymentSource == Source.Text).Select(p => p.InstitutionId).Single();
                PayPost.Post.PaymentSource.FileType = Sources.Where(p => p.PaymentSource == Source.Text).Select(p => p.FileType).Single();
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Displays a message to the user of all the missing data.
        /// </summary>
        /// <param name="missingData"></param>
        private void NotifyUser(List<string> missingData)
        {
            string message = "Please provide the following missing data\r\n\r\n";
            foreach (string item in missingData)
            {
                message += item + "\r\n";
            }
            Dialog.Error.Ok(message, "Missing Data");
        }

        private void PaymentTypeSetup_Click(object sender, EventArgs e)
        {
            using (PaymentTypeSetup setup = new PaymentTypeSetup(DA))
            {
                this.Hide();
                setup.ShowDialog();
                this.Show();
            }
        }

        private void PaymentSourceSetup_Click(object sender, EventArgs e)
        {
            using (PaymentSourceSetup setup = new PaymentSourceSetup(DA))
            {
                this.Hide();
                setup.ShowDialog();
                Sources = null;
                Sources = DA.GetPaymentSources();
                Sources.Insert(0, new PaymentSources());
                Source.DataSource = Sources;
                Source.DisplayMember = "PaymentSource";
                Source.ValueMember = "PaymentSourcesId";
                this.Show();
            }
        }
    }
}