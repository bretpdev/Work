using System;
using System.Collections.Generic;

namespace INCIDENTRP
{
	partial class AgencyEmployeeHrDataInvolvedOption : BaseDataInvolvedOption
	{
		private AgencyEmployeeHrDataInvolved _employeeDataInvolved;

		public AgencyEmployeeHrDataInvolvedOption(AgencyEmployeeHrDataInvolved employeeDataInvolved, List<string> states)
		{
			InitializeComponent();
			cmbState.DataSource = states;
			_employeeDataInvolved = employeeDataInvolved;
			agencyEmployeeHrDataInvolvedBindingSource.DataSource = _employeeDataInvolved;

			if (_employeeDataInvolved.NotifierKnowsEmployee)
                radYes.Checked = true; 
		}

		private void radYes_CheckedChanged(object sender, EventArgs e)
		{
			_employeeDataInvolved.NotifierKnowsEmployee = radYes.Checked;
			lblRelationship.Visible = radYes.Checked;
			txtRelationship.Visible = radYes.Checked;
			if (!radYes.Checked)
                txtRelationship.Clear();
		}
	}
}
