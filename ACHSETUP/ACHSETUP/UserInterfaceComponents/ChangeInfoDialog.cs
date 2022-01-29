using System.Windows.Forms;
using Uheaa.Common;

namespace ACHSETUP
{
	partial class ChangeInfoDialog : Form
	{
		/// <summary>
		/// DO NOT USE!!!
		/// The parameterless constructor is required by the Windows Forms Designer,
		/// but it won't work with the script.
		/// </summary>
		public ChangeInfoDialog()
		{
			InitializeComponent();
		}

		public ChangeInfoDialog(ChangeData changeData)
		{
			InitializeComponent();
			changeDataBindingSource.DataSource = changeData;
		}

		private void AccountType_KeyDown(object sender, KeyEventArgs e)
		{
			//Ignore key presses that aren't the C or S key.
			if (e.KeyCode != Keys.C && e.KeyCode != Keys.S) 
				e.Handled = true;
		}

        private void OK_Click(object sender, System.EventArgs e)
        {
            //acct number
            if(textBox4.Text.Length > 17)
            {
                Dialog.Warning.Ok("A bank account number can not be longer than 17 digits.  Please try again.", "Invalid Data Provided");
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}