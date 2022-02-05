using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace COMREFCOM
{
    partial class CompassRefundCommentFrm : Form
    {

        public List<SSNAndRefundAmtCombo> UserProvidedDataList { get; set; }
        public bool VoidChecked
        {
            get
            {
                return chkVoidCheck.Checked;
            }
        }
        public string VoidReason
        {
            get
            {
                return txtVoidReason.Text;
            }
        }

        /// <summary>
        /// Constructor (DO NOT USE).
        /// </summary>
        public CompassRefundCommentFrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CompassRefundCommentFrm(Refund data, string utUserID)
        {
            InitializeComponent();
            UserProvidedDataList = new List<SSNAndRefundAmtCombo>();
            txtUserID.Text = utUserID;
            refundBindingSource.DataSource = data;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //check to be sure that the void data is provided
            if (chkVoidCheck.Checked && txtVoidReason.Text.Length == 0)
            {
                MessageBox.Show("The void reason is required if Void Check is checked.", "Enter Void Reason", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //capture transaction information
            UserProvidedDataList.Add(new SSNAndRefundAmtCombo(txtSSN.Text, txtRefundAmount.Text));

            txtSSN.Clear();
            txtRefundAmount.Clear();
            txtSSN.Focus();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            //check to be sure that the void data is provided
            if (chkVoidCheck.Checked && txtVoidReason.Text.Length == 0)
            {
                MessageBox.Show("The void reason is required if Void Check is checked.", "Enter Void Reason", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //gather last transaction if one was provided
            if (txtSSN.Text.Length != 0)
            {
                //capture transaction information
                UserProvidedDataList.Add(new SSNAndRefundAmtCombo(txtSSN.Text, txtRefundAmount.Text));
            }

            this.DialogResult = DialogResult.OK;
            this.Hide();

        }

    }
}
