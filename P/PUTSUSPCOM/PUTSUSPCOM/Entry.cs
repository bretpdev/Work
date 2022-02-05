using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace PUTSUSPCOM
{
    public partial class Entry : FormBase
    {

        public ReflectionInterface RI { get; set; }
        public Suspense SystemGatheredData { get; set; }

        public OptionProcessorBase ProcessingOption
        {
            get { return ((OptionUIBase)pnlTheGuts.Controls[0]).Option; }
            //set { option = value; }
        }

        /// <summary>
        /// Default constructor (Do Not Use).
        /// </summary>
        public Entry()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Entry(Suspense data, ReflectionInterface ri)
        {
            InitializeComponent();
            borrowerDemographicsBindingSource.DataSource = data.BorrowerDemos;
            SystemGatheredData = data;
            suspenseDataBindingSource.DataSource = SystemGatheredData;
            RI = ri;
            
        }

        private void radDeletePayment_CheckedChanged(object sender, EventArgs e)
        {
            if (pnlTheGuts.Controls.Count > 0)
            {
                pnlTheGuts.Controls.RemoveAt(0);
            }
            pnlTheGuts.Controls.Add(new DeleteSuspensePayment());
            pnlTheGuts.Refresh();
        }

        private void radTarget_CheckedChanged(object sender, EventArgs e)
        {
            if (pnlTheGuts.Controls.Count > 0)
            {
                pnlTheGuts.Controls.RemoveAt(0);
            }
            pnlTheGuts.Controls.Add(new TargetPayment());
            pnlTheGuts.Refresh();
        }

        private void radDeleteReappliedPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (pnlTheGuts.Controls.Count > 0)
            {
                pnlTheGuts.Controls.RemoveAt(0);
            }
            pnlTheGuts.Controls.Add(new DeleteAndReapplyPayment());
            pnlTheGuts.Refresh();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (pnlTheGuts.Controls.Count > 0)
            {
                if (((IUserInputValidator)pnlTheGuts.Controls[0]).IsValidUserInput())
                {
                    //if user input is valid then start processing
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                //option wasn't selected.
                MessageBox.Show("You must select an option first.","Option Needed",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
