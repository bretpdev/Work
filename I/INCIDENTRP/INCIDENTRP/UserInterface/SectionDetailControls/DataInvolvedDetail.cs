using System;
using System.Collections.Generic;

namespace INCIDENTRP
{
	partial class DataInvolvedDetail : BaseDetail
	{
		private Incident _incident;
		private List<string> _regions;
		private List<string> _states;

		public DataInvolvedDetail(Incident incident, List<string> options, List<string> states, List<string> regions)
		{
			InitializeComponent();
			_incident = incident;
			_regions = regions;
			_states = states;
			cmbDataInvolvedOptions.DataSource = options;
			//Run the event handler to show a DataInvolvedOption control.
			cmbDataInvolvedOptions_SelectedIndexChanged(this, new EventArgs());
		}

		public override void CheckValidity()
		{
			bool isValid = true;
			if (isValid != _isValidated)
			{
				_isValidated = isValid;
			}
		}

		private void cmbDataInvolvedOptions_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Assign the DataInvolved property on the Incident object.
			//We're doing this here rather than data binding, because this event
			//appears to fire before the binding pushes the value back into the class,
			//which results in a null reference being passed to the panel's new control.
			_incident.DataInvolved = cmbDataInvolvedOptions.SelectedItem.ToString();

			//Load the panel based on the selected option.
			pnlOptionContent.Controls.Clear();
			switch (_incident.DataInvolved)
			{
				case Incident.AGENCY_DATA:
					pnlOptionContent.Controls.Add(new AgencyDataInvolvedOption(_incident.AgencyDataInvolved));
					break;
				case Incident.AGENCY_EMPLOYEE_HR_DATA:
					pnlOptionContent.Controls.Add(new AgencyEmployeeHrDataInvolvedOption(_incident.AgencyEmployeeDataInvolved, _states));
					break;
				case Incident.BORROWER_DATA:
					pnlOptionContent.Controls.Add(new BorrowerDataInvolvedOption(_incident.BorrowerDataInvolved, _states, _regions));
					break;
				case Incident.THIRD_PARTY_DATA:
					pnlOptionContent.Controls.Add(new ThirdPartyDataInvolvedOption(_incident.ThirdPartyDataInvolved, _states, _regions));
					break;
			}//switch
		}
	}//class
}//namespace
