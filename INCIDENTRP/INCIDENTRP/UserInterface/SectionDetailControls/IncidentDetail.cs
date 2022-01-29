using System;
using System.Collections.Generic;

namespace INCIDENTRP
{
	partial class IncidentDetail : BaseDetail
	{
		public IncidentDetail(Ticket ticket, List<string> notificationMethods, List<string> notifierTypes, List<string> functionalAreas, List<string> relationships)
		{
			InitializeComponent();
			cmbFunctionalArea.DataSource = functionalAreas;
			cmbNotificationMethod.DataSource = notificationMethods;
			cmbNotifierType.DataSource = notifierTypes;
			cmbRelationship.DataSource = relationships;
			ticketBindingSource.DataSource = ticket;
			incidentBindingSource.DataSource = ticket.Incident;
			notifierBindingSource.DataSource = ticket.Incident.Notifier;
			IncidentDate.MaxDate = DateTime.Now.Date;
		}

		public override void CheckValidity()
		{
			bool isValid = true;
			if (isValid != _isValidated)
			{
				_isValidated = isValid;
			}
		}

		private void cmbNotificationMethod_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cmbNotificationMethod.Text == Notifier.OTHER)
			{
				lblOtherMethodDescription.Enabled = true;
				txtOtherMethodDescription.Enabled = true;
			}
			else
			{
				lblOtherMethodDescription.Enabled = false;
				txtOtherMethodDescription.Clear();
				txtOtherMethodDescription.Enabled = false;
			}
		}

		private void cmbNotifierType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cmbNotifierType.Text == Notifier.OTHER)
			{
				lblOtherTypeDescription.Enabled = true;
				txtOtherTypeDescription.Enabled = true;
			}
			else
			{
				lblOtherTypeDescription.Enabled = false;
				txtOtherTypeDescription.Clear();
				txtOtherTypeDescription.Enabled = false;
			}
		}

		private void cmbRelationship_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cmbRelationship.Text == Notifier.OTHER)
			{
				lblOtherRelationship.Enabled = true;
				txtOtherRelationship.Enabled = true;
			}
			else
			{
				lblOtherRelationship.Enabled = false;
				txtOtherRelationship.Clear();
				txtOtherRelationship.Enabled = false;
			}
		}
	}//class
}//namespace
