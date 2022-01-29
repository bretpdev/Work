using System;

namespace INCIDENTRP
{
	partial class ElectronicMailDeliveryIncidentType : BaseIncidentType
	{
		private ElectronicMailDeliveryIncident _electronicMailDeliveryIncident;

		public ElectronicMailDeliveryIncidentType(ElectronicMailDeliveryIncident electronicMailDeliveryIncident)
		{
			InitializeComponent();
			_electronicMailDeliveryIncident = electronicMailDeliveryIncident;
			electronicMailDeliveryIncidentBindingSource.DataSource = _electronicMailDeliveryIncident;

			if (_electronicMailDeliveryIncident.EmailAddressWasDisclosed)
                chkEmailAddressesDisclosed.Checked = true;
			if (_electronicMailDeliveryIncident.FtpTransmissionWasSentToIncorrectDestination)
                chkFtp.Checked = true;
			if (_electronicMailDeliveryIncident.IncorrectAttachmentContainedPii)
                chkIncorrectAttachment.Checked = true;
			if (_electronicMailDeliveryIncident.EmailWasDeliveredToIncorrectAddress)
                chkIncorrectEmailAddress.Checked = true;
		}

		private void chkEmailAddressesDisclosed_CheckedChanged(object sender, EventArgs e)
		{
			_electronicMailDeliveryIncident.EmailAddressWasDisclosed = chkEmailAddressesDisclosed.Checked;
			if (chkEmailAddressesDisclosed.Checked)
			{
				chkFtp.Checked = false;
				chkIncorrectAttachment.Checked = false;
				chkIncorrectEmailAddress.Checked = false;
				lblDetail.Text = "Addresses Disclosed";
			}
		}

		private void chkFtp_CheckedChanged(object sender, EventArgs e)
		{
			_electronicMailDeliveryIncident.FtpTransmissionWasSentToIncorrectDestination = chkFtp.Checked;
			if (chkFtp.Checked)
			{
				chkEmailAddressesDisclosed.Checked = false;
				chkIncorrectAttachment.Checked = false;
				chkIncorrectEmailAddress.Checked = false;
				lblDetail.Text = "IP Address or DNS of Incorrect Transmission";
			}
		}

		private void chkIncorrectAttachment_CheckedChanged(object sender, EventArgs e)
		{
			_electronicMailDeliveryIncident.IncorrectAttachmentContainedPii = chkIncorrectAttachment.Checked;
			if (chkIncorrectAttachment.Checked)
			{
				chkEmailAddressesDisclosed.Checked = false;
				chkFtp.Checked = false;
				chkIncorrectEmailAddress.Checked = false;
				lblDetail.Text = "E-mail Address";
			}
		}

		private void chkIncorrectEmailAddress_CheckedChanged(object sender, EventArgs e)
		{
			_electronicMailDeliveryIncident.EmailWasDeliveredToIncorrectAddress = chkIncorrectEmailAddress.Checked;
			if (chkIncorrectEmailAddress.Checked)
			{
				chkEmailAddressesDisclosed.Checked = false;
				chkFtp.Checked = false;
				chkIncorrectAttachment.Checked = false;
				lblDetail.Text = "E-mail Address";
			}
		}
	}
}
