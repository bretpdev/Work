using System;
using System.Collections.Generic;

namespace INCIDENTRP
{
	partial class BorrowerDataInvolvedOption : BaseDataInvolvedOption
	{
		private BorrowerDataInvolved _borrowerDataInvolved;

		public BorrowerDataInvolvedOption(BorrowerDataInvolved borrowerDataInvolved, List<string> states, List<string> regions)
		{
			InitializeComponent();
			cmbState.DataSource = states;
			cmbRegion.DataSource = regions;
			_borrowerDataInvolved = borrowerDataInvolved;
			borrowerDataInvolvedBindingSource.DataSource = _borrowerDataInvolved;

			if (_borrowerDataInvolved.NotifierKnowsPiiOwner)
                radYes.Checked = true;
		}

		private void radYes_CheckedChanged(object sender, EventArgs e)
		{
			_borrowerDataInvolved.NotifierKnowsPiiOwner = radYes.Checked;
			lblRelationship.Visible = radYes.Checked;
			txtRelationship.Visible = radYes.Checked;
			if (!radYes.Checked)
                txtRelationship.Clear();
		}
	}
}
