using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CANCOMPQUE
{
    partial class frmCancelQueue : Form
    {
        private CancelData _data;

        public frmCancelQueue(CancelData data)
        {
            InitializeComponent();
            _data = data;
            cancelDataBindingSource.DataSource = data;
            txtDateFrom.Value = DateTime.Now;
            txtDateThrough.Value = DateTime.Now;
        }

        /// <summary>
        /// Disable the Error Activity groupbox and enable the error message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoQueueOnly_CheckedChanged(object sender, EventArgs e)
        {
            gbxError.Enabled = false;
        }

        /// <summary>
        /// Enable the Error Activity group box and disable the error message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoBoth_CheckedChanged(object sender, EventArgs e)
        {
            gbxError.Enabled = true;
        }

        /// <summary>
        /// Once there queue is set, move to the sub queue textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQueue_TextChanged(object sender, EventArgs e)
        {
            if (txtQueue.Text.Length == 2) { txtSubQueue.Focus(); }
        }

        /// <summary>
        /// Once the sub queue is set, move to the message text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSubQueue_TextChanged(object sender, EventArgs e)
        {
            if (txtSubQueue.Text.Length == 2) { txtErrorMessage.Focus(); }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                _data.FromDate = Convert.ToDateTime(txtDateFrom.Value);
                _data.ToDate = Convert.ToDateTime(txtDateThrough.Value);
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Validates all of the controls on the form
        /// </summary>
        /// <returns>True if all controls have the right data</returns>
        private bool IsValid()
        {
            if (rdoBoth.Checked)
            {
                if (txtDateFrom.Value.ToShortDateString() == DateTime.Now.ToShortDateString() && txtDateThrough.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                {
                    //return false if they choose no
                    if( MessageBox.Show("The dates are the same. Is this correct?", "Same Date", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    return false;
                }
                if (txtArc.Text == string.Empty)
                {
                    MessageBox.Show("A valid ARC is required", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            //Validation that is always checked
            if (txtQueue.Text.Length != 2 || txtSubQueue.Text.Length != 2)
            {
                MessageBox.Show("A two character Queue, a two character SubQueue and an Error message is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtErrorMessage.Text.Length < 1 && txtArc.Text == string.Empty)
            {
                //Return false if they choose no
                if (MessageBox.Show("Are you sure you want to leave the Error Message blank.  When the Error Message is left blank the script will cancel all tasks in that queue.  Click 'Yes' to continue or 'No' to enter a Error Message.", "Validation Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return false; }
            }

            return true;
        }

    }//Class
}//Namespace
