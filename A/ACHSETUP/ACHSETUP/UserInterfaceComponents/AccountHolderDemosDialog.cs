using System.Windows.Forms;
using Uheaa.Common.Scripts;

namespace ACHSETUP
{
	public partial class AccountHolderDemosDialog : Form
	{
		/// <summary>
		/// DO NOT USE!!!
		/// The parameterless constructor is required by the Windows Forms Designer,
		/// but it won't work with the script.
		/// </summary>
		public AccountHolderDemosDialog()
		{
			InitializeComponent();
		}

		public AccountHolderDemosDialog(SystemBorrowerDemographics demos)
		{
			InitializeComponent();
            personDemographicsBindingSource.DataSource = demos;
		}
	}
}