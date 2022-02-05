using System;
using System.Windows.Forms;

namespace ACHSETUP
{
    public partial class frmCannotDetermineNextPayDue : Form
	{
		private string _result;

		public string Result
		{
			get { return _result; }
		}
		public frmCannotDetermineNextPayDue()
		{
			InitializeComponent();
			dtpNextpayDue.MinDate = DateTime.Now;
		}

		private void Ok_Click(object sender, EventArgs e)
		{
			_result = dtpNextpayDue.Value.ToShortDateString();
			DialogResult = DialogResult.OK;
		}
	}
}