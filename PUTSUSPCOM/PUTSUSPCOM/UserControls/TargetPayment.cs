using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace PUTSUSPCOM
{
    public partial class TargetPayment : OptionUIBase, IUserInputValidator
    {

        

        /// <summary>
        /// Constructor
        /// </summary>
        public TargetPayment()
        {
            InitializeComponent();
        }


        #region IUserInputValidator Members

        public bool IsValidUserInput()
        {
            try
            {
                List<string> temp = ((TargetPaymentOptionProcessor)Option).CreateLoanSequenceNumberList();
                if (txtAmount.TextLength != 0 && txtAmount.Text.IsNumeric() == false)
                {
                    MessageBox.Show("If an amount is going to be provided then it must be nummeric.", "Error Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (txtAmount.TextLength != 0 && temp.Count != 1)
                {
                    MessageBox.Show("If an amount is going to be provided the you may only provide a single loan sequence number.", "Error Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    return true; //if exception not thrown then everythings is ok.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        private void TargetPayment_Load(object sender, EventArgs e)
        {
            Option = new TargetPaymentOptionProcessor(((Entry)this.ParentForm).RI, ((Entry)this.ParentForm).SystemGatheredData);
            optionProcessorBaseBindingSource.DataSource = Option;
            targetPaymentOptionProcessorBindingSource.DataSource = ((TargetPaymentOptionProcessor)Option);
        }
    }
}
