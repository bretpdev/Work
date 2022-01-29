using System.Collections.Generic;
using SubSystemShared;
using Uheaa.Common.ProcessLogger;

namespace INCIDENTRP
{
	partial class IncidentReporterDetail : BaseDetail
	{
		public IncidentReporterDetail(Incident incident, List<SqlUser> currentEmployees, List<BusinessUnit> businessUnits, List<string> urgencies)
		{
			InitializeComponent();
			cmbUser.DataSource = currentEmployees;
			cmbBusinessUnit.DataSource = businessUnits;
			cmbPriority.DataSource = urgencies;
			incidentBindingSource.DataSource = incident;
			reporterBindingSource.DataSource = incident.Reporter;
			cmbUser.Text = incident.Reporter.User.ToString();
			cmbBusinessUnit.Text = incident.Reporter.BusinessUnit.Name;
			txtEmailAddress.Text = incident.Reporter.User.EmailAddress;
		}

		public override void CheckValidity()
		{
			bool isValid = true;
			if (isValid != _isValidated)
			{
				_isValidated = isValid;
			}
		}
	}
}