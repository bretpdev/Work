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
    public partial class DeleteSuspensePayment : OptionUIBase, IUserInputValidator
    {


        /// <summary>
        /// Constructor
        /// </summary>
        public DeleteSuspensePayment()
        {
            InitializeComponent();
        }

        private void CopyUIDataToBusinessObject()
        {
            //data binding doesn't appear to be possible for a listview so the data must be manually moved over
            ((DeleteSuspenseOptionProcessor) Option).UserSelectedDeals = new List<Deal>();
            ((DeleteSuspenseOptionProcessor) Option).UserSelectedLoanTypes = new List<string>();
            foreach (ListViewItem lvi in lvwDeals.SelectedItems)
            {
                ((DeleteSuspenseOptionProcessor)Option).UserSelectedDeals.Add(((DealListViewItem)lvi).DealData);
            }
            foreach (ListViewItem lvi in lvwLoanTypes.SelectedItems)
            {
                ((DeleteSuspenseOptionProcessor) Option).UserSelectedLoanTypes.Add(lvi.Text);
            }
        }

        #region IUserInputValidator Members

        public bool IsValidUserInput()
        {
            if (lvwDeals.SelectedItems.Count == 0 || lvwLoanTypes.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one deal # and one loan type.", "Information Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                CopyUIDataToBusinessObject();
                return true;
            }
        }

        #endregion

        private void DeleteSuspensePayment_Load(object sender, EventArgs e)
        {
            List<Deal> deals = DataAccess.GetDeals(((Entry)this.ParentForm).RI.TestMode);
            foreach (Deal deal in deals)
            {
                lvwDeals.Items.Add(new DealListViewItem(deal));
            }
            Option = new DeleteSuspenseOptionProcessor(((Entry)this.ParentForm).RI, ((Entry)this.ParentForm).SystemGatheredData);
            optionProcessorBaseBindingSource.DataSource = Option;
        }
    }
}
