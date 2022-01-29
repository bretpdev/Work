using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace NSFREVENTR
{
    public partial class EntryForm : FormBase
    {

        public ReversalEntry UserProvidedEntry { get; set; }

        /// <summary>
        /// Constructor to use.
        /// </summary>
        public EntryForm(NSFReversalEntry.System defaultSystemToSelect, bool testMode)
        {
            InitializeComponent();
            LoadNSFReasons(testMode);
            UserProvidedEntry = new ReversalEntry();
            reversalEntryBindingSource.DataSource = UserProvidedEntry;
            //select system based off default
            if (defaultSystemToSelect == NSFReversalEntry.System.Compass)
            {
                radCompass.Checked = true;
            }
            else if (defaultSystemToSelect == NSFReversalEntry.System.OneLINK)
            {
                radOneLINK.Checked = true;
            }
            txtSSN.Focus();
        }

        /// <summary>
        /// Default constructor (do not use).
        /// </summary>
        public EntryForm()
        {
            InitializeComponent();
        }


        //loads NSF Reason combo box
        private void LoadNSFReasons(bool testMode)
        {
            List<NSFReason> nsfReasons = DataAccess.GetListOfNSFReasons(testMode);
            cmbReason.ValueMember = "Code";
            cmbReason.DisplayMember = "Text";
            cmbReason.Items.AddRange(nsfReasons.ToArray());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateUnboundProperties();
            //do data validation
            if (UserProvidedEntry.IsValid() == true)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void radOneLINK_CheckedChanged(object sender, EventArgs e)
        {
            if (radOneLINK.Checked)
            {
                grpDetail.Enabled = true;
                lblBatchType.Enabled = true;
                grpBatchType.Enabled = true;
                grpLoansToReverse.Enabled = false;
                lblLoansToReverse.Enabled = false;
                btnDeconvertedLoans.Enabled = false;
            }
        }

        private void radCompass_CheckedChanged(object sender, EventArgs e)
        {
            if (radCompass.Checked)
            {
                grpDetail.Enabled = true;
                lblBatchType.Enabled = false;
                grpBatchType.Enabled = false;
                grpLoansToReverse.Enabled = true;
                lblLoansToReverse.Enabled = true;
                btnDeconvertedLoans.Enabled = true;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //load the rest of the object from unbound form controls
            if (radOneLINK.Checked)
            {
                UserProvidedEntry.System = NSFReversalEntry.System.OneLINK;
            }
            else if (radCompass.Checked)
            {
                UserProvidedEntry.System = NSFReversalEntry.System.Compass;
            }
            else
            {
                UserProvidedEntry.System = NSFReversalEntry.System.None;
            }
            //do data validation
            if (UserProvidedEntry.IsValidForPrinting() == true)
            {
                this.DialogResult = DialogResult.Yes;
            }
        }

        private void radALL_CheckedChanged(object sender, EventArgs e)
        {
            if (radALL.Checked)
            {
                txtLoans.Enabled = false;
            }
        }

        private void radListed_CheckedChanged(object sender, EventArgs e)
        {
            if (radListed.Checked)
            {
                txtLoans.Enabled = true;
            }
        }

        private void txtLoans_Enter(object sender, EventArgs e)
        {
            txtLoans.SelectAll();
        }

        private void txtLoans_Click(object sender, EventArgs e)
        {
            txtLoans.SelectAll();
        }

        private void btnDeconvertedLoans_Click(object sender, EventArgs e)
        {
            UpdateUnboundProperties();
            //do data validation
            if (UserProvidedEntry.IsValid() == true)
            {
                UserProvidedEntry.ProcessForDeconvertedLoans = true; //flag that deconverted button was clicked
                this.DialogResult = DialogResult.OK;
            }
        }

        private void UpdateUnboundProperties()
        {
            //load the rest of the object from unbound form controls
            if (radOneLINK.Checked)
            {
                UserProvidedEntry.System = NSFReversalEntry.System.OneLINK;
            }
            else if (radCompass.Checked)
            {
                UserProvidedEntry.System = NSFReversalEntry.System.Compass;
            }
            else
            {
                UserProvidedEntry.System = NSFReversalEntry.System.None;
            }
            UserProvidedEntry.NSFRe = (NSFReason)cmbReason.SelectedItem;
            //loan list
            if (radALL.Checked)
            {
                UserProvidedEntry.LoanListProvidedMethod = NSFReversalEntry.LoanListLocation.All;
            }
            else
            {
                UserProvidedEntry.LoanListProvidedMethod = NSFReversalEntry.LoanListLocation.SeeListBelow;
            }
            //payment method
            if (radCash.Checked)
            {
                UserProvidedEntry.BatchType = NSFReversalEntry.BatchType.Cash;
            }
            else if (radWire.Checked)
            {
                UserProvidedEntry.BatchType = NSFReversalEntry.BatchType.Wire;
            }
            else
            {
                UserProvidedEntry.BatchType = NSFReversalEntry.BatchType.None;
            }
        }

    }
}
