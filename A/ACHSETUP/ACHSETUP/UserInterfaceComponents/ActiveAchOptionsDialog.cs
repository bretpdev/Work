using System.Windows.Forms;
using Uheaa.Common;

namespace ACHSETUP
{
	public partial class ActiveAchOptionsDialog : Form
	{
        private ActiveACHRecordFoundOption EnumContainer;

		/// <summary>
		/// DO NOT USE!!!
		/// The parameterless constructor is required by the Windows Forms Designer,
		/// but it won't work with the script.
		/// </summary>
		public ActiveAchOptionsDialog()
		{
			InitializeComponent();
		}

        public ActiveAchOptionsDialog(ActiveACHRecordFoundOption enumContainer)
		{
			InitializeComponent();

			EnumContainer = enumContainer;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if (radAdd.Checked)
			{
                EnumContainer.SelectedOption = ActiveACHRecordFoundOption.Option.Add;
				DialogResult = DialogResult.OK;
			}
			else if (radChange.Checked)
			{
                EnumContainer.SelectedOption = ActiveACHRecordFoundOption.Option.Change;
				DialogResult = DialogResult.OK;
			}
			else if (radStop.Checked)
			{
                EnumContainer.SelectedOption = ActiveACHRecordFoundOption.Option.Stop;
				DialogResult = DialogResult.OK;
			}
			else
				Dialog.Def.Ok("You must make a selection.");
		}
	}
}