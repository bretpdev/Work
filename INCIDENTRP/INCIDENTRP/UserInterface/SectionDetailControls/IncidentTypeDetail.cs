using System;
using System.Collections.Generic;

namespace INCIDENTRP
{
	partial class IncidentTypeDetail : BaseDetail
	{
		private Incident _incident;

		public IncidentTypeDetail(Incident incident, List<string> incidentTypes)
		{
			InitializeComponent();
			_incident = incident;
			cmbTypeOptions.DataSource = incidentTypes;
			//Run the event handler to show an IncidentType control.
			cmbTypeOptions_SelectedIndexChanged(this, new EventArgs());
		}

		public override void CheckValidity()
		{
			bool isValid = true;
			if (isValid != _isValidated)
			{
				_isValidated = isValid;
			}
		}

		private void cmbTypeOptions_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Assign the Type property on the Incident object.
			//We're doing this here rather than data binding, because this event
			//appears to fire before the binding pushes the value back into the class,
			//which results in a null reference being passed to the panel's new control.
			_incident.Type = cmbTypeOptions.SelectedItem.ToString();

			//Load the panel based on the selected option.
			pnlOptionContent.Controls.Clear();
			switch (_incident.Type)
			{
				case Incident.ACCESS_CONTROL:
					pnlOptionContent.Controls.Add(new AccessControlIncidentType(_incident.AccessControlIncident));
					break;
				case Incident.DATA_ENTRY:
					pnlOptionContent.Controls.Add(new DataEntryIncidentType(_incident.DataEntryIncident));
					break;
				case Incident.DISPOSAL_DESTRUCTION:
					pnlOptionContent.Controls.Add(new DisposalOrDestructionIncidentType(_incident.DisposalOrDestructionIncident));
					break;
				case Incident.ELECTRONIC_MAIL_DELIVERY:
					pnlOptionContent.Controls.Add(new ElectronicMailDeliveryIncidentType(_incident.ElectronicMailDeliveryIncident));
					break;
				case Incident.FAX:
					pnlOptionContent.Controls.Add(new FaxIncidentType(_incident.FaxIncident));
					break;
				case Incident.ODD_COMPUTER_BEHAVIOR:
					pnlOptionContent.Controls.Add(new OddComputerBehaviorIncidentType(_incident.OddComputerBehaviorIncident));
					break;
				case Incident.PHYSICAL_DAMAGE_LOSS_THEFT:
					pnlOptionContent.Controls.Add(new PhysicalDamageIncidentType(_incident.PhysicalDamageLossOrTheftIncident));
					break;
				case Incident.REGULAR_MAIL_DELIVERY:
					pnlOptionContent.Controls.Add(new RegularMailDeliveryIncidentType(_incident.RegularMailDeliveryIncident));
					break;
				case Incident.SCANS_PROBES:
					pnlOptionContent.Controls.Add(new ScansProbesIncidentType(_incident.ScanOrProbeIncident));
					break;
				case Incident.SYSTEM_OR_NETWORK_UNAVAILABLE:
					pnlOptionContent.Controls.Add(new SystemOrNetworkUnavailableIncidentType(_incident.SystemOrNetworkUnavailableIncident));
					break;
				case Incident.TELEPHONE:
					pnlOptionContent.Controls.Add(new TelephoneIncidentType(_incident.TelephoneIncident));
					break;
				case Incident.UNAUTHORIZED_PHYSICAL_ACCESS:
					pnlOptionContent.Controls.Add(new UnauthorizedPhysicalAccessIncidentType(_incident.UnauthorizedPhysicalAccessIncident));
					break;
				case Incident.UNAUTHORIZED_SYSTEM_ACCESS:
					pnlOptionContent.Controls.Add(new UnauthorizedSystemAccessIncidentType(_incident.UnauthorizedSystemAccessIncident));
					break;
				case Incident.VIOLATION_OF_ACCEPTABLE_USE:
					pnlOptionContent.Controls.Add(new ViolationOfAcceptableUseIncidentType(_incident.ViolationOfAcceptableUseIncident));
					break;
			}//switch
		}
	}//class
}//namespace
