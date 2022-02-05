namespace INCIDENTRP
{
    partial class RegularMailDeliveryIncidentType : BaseIncidentType
    {
        private RegularMailDeliveryIncident _regularMailDeliveryIncident;

        public RegularMailDeliveryIncidentType(RegularMailDeliveryIncident regularMailDeliveryIncident)
        {
            InitializeComponent();
            _regularMailDeliveryIncident = regularMailDeliveryIncident;
            regularMailDeliveryIncidentBindingSource.DataSource = _regularMailDeliveryIncident;

            if (_regularMailDeliveryIncident.Problem == RegularMailDeliveryIncident.INCORRECT_ADDRESS)
                chkIncorrectAddress.Checked = true;
            if (_regularMailDeliveryIncident.Problem == RegularMailDeliveryIncident.INCORRECT_CONTENTS)
                chkIncorrectContents.Checked = true;
        }

        private void chkIncorrectAddress_CheckedChanged(object sender, System.EventArgs e)
        {
            lblTrackingNumber.Visible = chkIncorrectAddress.Checked;
            txtTrackingNumber.Visible = chkIncorrectAddress.Checked;
            if (chkIncorrectAddress.Checked)
            {
                chkIncorrectContents.Checked = false;
                _regularMailDeliveryIncident.Problem = RegularMailDeliveryIncident.INCORRECT_ADDRESS;
            }
        }

        private void chkIncorrectContents_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkIncorrectContents.Checked)
            {
                chkIncorrectAddress.Checked = false;
                _regularMailDeliveryIncident.Problem = RegularMailDeliveryIncident.INCORRECT_CONTENTS;
            }
        }
    }
}
