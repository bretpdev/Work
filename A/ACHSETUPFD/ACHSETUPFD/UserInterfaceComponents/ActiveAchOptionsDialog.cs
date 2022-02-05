using System.Windows.Forms;


namespace ACHSETUPFD
{
	public partial class ActiveAchOptionsDialog : Form
	{
        private ActiveACHRecordFoundOption EnumContainer { get; set; }

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
			{
				MessageBox.Show("You must make a selection.");
			}
		}
	}
}
