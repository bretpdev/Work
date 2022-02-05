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
    public partial class DeleteAndReapplyPayment : OptionUIBase, IUserInputValidator
    {

        decimal _calculatedAmountTotalFromUserInput;

        /// <summary>
        /// Constructor
        /// </summary>
        public DeleteAndReapplyPayment()
            : base()
        {
            InitializeComponent();
        }


        private void CopyUIDataToBusinessObject()
        {
            //data binding doesn't appear to be possible for a listview so the data must be manually moved over
            ((DeleteAndReapplyOptionProcessor)Option).UserSelectedDeals = new List<Deal>();
            ((DeleteAndReapplyOptionProcessor)Option).UserSelectedLoanTypes = new List<string>();
            foreach (ListViewItem lvi in lvwDeals.SelectedItems)
            {
                ((DeleteAndReapplyOptionProcessor)Option).UserSelectedDeals.Add(((DealListViewItem)lvi).DealData);
            }
            foreach (ListViewItem lvi in lvwLoanTypes.SelectedItems)
            {
                ((DeleteAndReapplyOptionProcessor)Option).UserSelectedLoanTypes.Add(lvi.Text);
            }
        }

        #region IUserInputValidator Members

        public bool IsValidUserInput()
        {
            _calculatedAmountTotalFromUserInput = 0;  //zero out and then calculate in RecordDataIsValid calls.
            if (lvwDeals.SelectedItems.Count == 0 || lvwLoanTypes.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one deal # and one loan type.", "Information Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (pnlEDServicer.Controls.Count == 0 || pnlReapply.Controls.Count == 0 ||
                RecordDataIsValid(pnlEDServicer) == false || RecordDataIsValid(pnlReapply) == false)
            {
                MessageBox.Show("Some of the data your provided for the ED Servicer or to be Reapplied is invalid.  Please provide at least one entry under each section and ensure that all required fields are populated.", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (Math.Round(_calculatedAmountTotalFromUserInput,2) != Math.Round(decimal.Parse(((Entry)this.ParentForm).SystemGatheredData.Amount),2))
            {
                MessageBox.Show("The total of records being sent to ED and Repplied must equal the suspense transaction amount from Compass.", "Amounts Don't Equal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                CopyUIDataToBusinessObject();
                return true;
            }
        }

        #endregion

        //Cycles through all records on panel to be sure the data is valid, also adds totals of amounts
        private bool RecordDataIsValid(Panel pnl)
        {
            foreach (Control cntrl in pnl.Controls)
            {
                if (((DeleteAndReapplyRow)cntrl).UserInputIsValid() == false)
                {
                    return false; //data isn't valid for one of the records on the UI
                }
                else
                {
                    _calculatedAmountTotalFromUserInput = _calculatedAmountTotalFromUserInput + decimal.Parse(((DeleteAndReapplyRow)cntrl).Amount);
                }
            }
            return true; //all records appear to be valid for records on UI
        }

        private void btnEDServicerAdd_Click(object sender, EventArgs e)
        {

            ((DeleteAndReapplyOptionProcessor)Option).EDServicerRowData.Add(new DeleteAndReapplyRowData());
            pnlEDServicer.Controls.Add(new DeleteAndReapplyRow(((DeleteAndReapplyOptionProcessor)Option).EDServicerRowData[((DeleteAndReapplyOptionProcessor)Option).EDServicerRowData.Count - 1]));
        }

        private void btnEDServicerRemove_Click(object sender, EventArgs e)
        {
            if (pnlEDServicer.Controls.Count > 0)
            {
                pnlEDServicer.Controls.RemoveAt(pnlEDServicer.Controls.Count - 1);
                ((DeleteAndReapplyOptionProcessor)Option).EDServicerRowData.RemoveAt(((DeleteAndReapplyOptionProcessor)Option).EDServicerRowData.Count - 1);
            }
        }

        private void btnReapplyAdd_Click(object sender, EventArgs e)
        {
            ((DeleteAndReapplyOptionProcessor)Option).ReapplyRowData.Add(new DeleteAndReapplyRowData());
            pnlReapply.Controls.Add(new DeleteAndReapplyRow(((DeleteAndReapplyOptionProcessor)Option).ReapplyRowData[((DeleteAndReapplyOptionProcessor)Option).ReapplyRowData.Count - 1]));
        }

        private void btnReapplyRemove_Click(object sender, EventArgs e)
        {
            if (pnlReapply.Controls.Count > 0)
            {
                pnlReapply.Controls.RemoveAt(pnlReapply.Controls.Count - 1);
                ((DeleteAndReapplyOptionProcessor)Option).ReapplyRowData.RemoveAt(((DeleteAndReapplyOptionProcessor)Option).ReapplyRowData.Count - 1);
            }
        }

        private void DeleteAndReapplyPayment_Load(object sender, EventArgs e)
        {
            List<Deal> deals = DataAccess.GetDeals(((Entry)this.ParentForm).RI.TestMode);
            foreach (Deal deal in deals)
            {
                lvwDeals.Items.Add(new DealListViewItem(deal));
            }
            Option = new DeleteAndReapplyOptionProcessor(((Entry)this.ParentForm).RI, ((Entry)this.ParentForm).SystemGatheredData);
            optionProcessorBaseBindingSource.DataSource = Option;
        }


    }
}
