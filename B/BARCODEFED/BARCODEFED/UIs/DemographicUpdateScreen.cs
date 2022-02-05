using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BARCODEFED
{
    public partial class DemographicUpdateScreen : Form
	{
		BorrowerDemos _bData;
		ForwardingInfo _fData;
		List<string> _states;
		public DemographicUpdateScreen(BorrowerDemos bData, NoBarcodeData nBd, ForwardingInfo fData, DataAccess da)
		{
			InitializeComponent();
			cbState.SelectedIndex = -1;
			_fData = fData;
			_bData = bData;
            _states = da.GetStateCodes();
			cbState.DataSource = _states;
			borrowerDemosBindingSource.DataSource = _bData;
			noBarcodeDataBindingSource.DataSource = nBd;
		}

		private void btnCornerStoneAddr_Click(object sender, EventArgs e)
		{
			_fData.Address1 = _bData.Address1;
			_fData.Address2 = _bData.Address2;
			_fData.City = _bData.City;
			_fData.Zip = _bData.Zip;
			_fData.State = _bData.State;
			forwardingInfoBindingSource.DataSource = _fData;
			this.Refresh();
		}

		private void btnContinue_Click(object sender, EventArgs e)
		{
			_fData.Address1 = txtAddress1.Text;
			_fData.Address2 = txtAddress2.Text;
			_fData.City = txtCity.Text;
			_fData.State = cbState.Text;
			_fData.Zip = txtZip.Text;
			DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnDaRules_Click(object sender, EventArgs e)
		{
			AddressHygiene daRules = new AddressHygiene();
			daRules.ShowDialog();
		}
    }
}