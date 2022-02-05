using System.Collections.Generic;

namespace INCIDENTRP
{
    partial class CauseDetail : BaseDetail
    {
        private Incident _incident;

        public CauseDetail(Incident incident, List<string> causes)
        {
            InitializeComponent();
            cmbCauseOptions.DataSource = causes;
            _incident = incident;
            incidentBindingSource.DataSource = _incident;
        }

        public override void CheckValidity()
        {
            bool isValid = true;
            if (isValid != _isValidated)
            {
                _isValidated = isValid;
            }
        }

        private void cmbCauseOptions_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cmbCauseOptions.Text == Incident.BORROWER_RELATIVE)
                pnlOptionContent.Controls.Add(new BorrowersRelativeCauseOption(_incident));
            else
                pnlOptionContent.Controls.Clear();
        }
    }
}
