using System.Collections.Generic;
using System.Windows.Forms;

namespace INCARBWRS
{
	partial class PrisonDialog : Form
    {


		public PrisonDialog(List<string> stateCodes, List<ContactSource> contactSources, PrisonInfo prison)
		{
			InitializeComponent();
            FormatComponents();
			stateCodes.Sort();
			stateCodes.Insert(0, "");
			cmbState.DataSource = stateCodes;
			contactSources.Insert(0, new ContactSource());
			contactSourceBindingSource.DataSource = contactSources;
			prisonInfoBindingSource.DataSource = prison;
		}

        private void FormatComponents()
        {
            comboBox1.Text = "";
            Ssn.Text = "";
            PrisonName.Text = "";
            PrisonAddress.Text = "";
            PrisonCity.Text = "";
            PrisonZip.Text = "";
            PrisonPhone.Text = ""; 
            PrisonInmateNumber.Text = "";
            PrisonReleaseDate.Text = "";
            PrisonFollowUpDate.Text = "";
            watermarkTextBox1.Text = "";
        }
    }
}