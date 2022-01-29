using System;
using System.Collections.Generic;

namespace INCIDENTRP
{
	partial class ThirdPartyDataInvolvedOption : BaseDataInvolvedOption
	{
		private ThirdPartyDataInvolved _thirdPartyDataInvolved;

		public ThirdPartyDataInvolvedOption(ThirdPartyDataInvolved thirdPartyDataInvolved, List<string> states, List<string> regions)
		{
			InitializeComponent();
			cmbState.DataSource = states;
			cmbRegion.DataSource = regions;
			_thirdPartyDataInvolved = thirdPartyDataInvolved;
			thirdPartyDataInvolvedBindingSource.DataSource = _thirdPartyDataInvolved;

			if (_thirdPartyDataInvolved.NotifierKnowsPiiOwner)
                radYes.Checked = true;
		}

		private void radYes_CheckedChanged(object sender, EventArgs e)
		{
			_thirdPartyDataInvolved.NotifierKnowsPiiOwner = radYes.Checked;
			lblRelationship.Visible = radYes.Checked;
			txtRelationship.Visible = radYes.Checked;
			if (!radYes.Checked)
                txtRelationship.Clear();
		}
	}
}
