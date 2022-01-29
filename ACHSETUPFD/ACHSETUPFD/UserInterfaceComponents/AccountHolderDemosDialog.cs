using System.Windows.Forms;
using Uheaa.Common.Scripts;

namespace ACHSETUPFD
{
	public partial class AccountHolderDemosDialog : Form
	{
        SystemBorrowerDemographics Demos { get; set; }

		public AccountHolderDemosDialog(SystemBorrowerDemographics demos)
		{
			InitializeComponent();
            Demos = demos;
		}

        private void OK_Click(object sender, System.EventArgs e)
        {
            Demos.FirstName = NameTextBox.Text;
            Demos.Address1 = Address1.Text;
            Demos.Address2 = Address2.Text;
            Demos.City = City.Text;
            Demos.State = State.Text;
            Demos.ZipCode = Zip.Text;
        }

	}
}
