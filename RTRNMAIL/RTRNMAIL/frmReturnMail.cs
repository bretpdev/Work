using Q;
using WHOAMI;

namespace RTRNMAIL
{
	partial class frmReturnMail : FormBase
	{
		private readonly ReflectionInterface _ri;

		public string RecipientId
		{
			get { return txtBorrowerId.Text; }
		}

		/// <summary>
		/// DO NOT USE!!! The no-parameter constructor is requred by Visual Studio's Form Designer, but it will not work with the script.
		/// </summary>
		public frmReturnMail()
		{
			InitializeComponent();
		}

		public frmReturnMail(ReflectionInterface ri)
		{
			InitializeComponent();
			_ri = ri;
		}

		private void btnWhoAmI_Click(object sender, System.EventArgs e)
		{
			WhoAmI who = new WhoAmI(_ri);
			SystemBorrowerDemographics borrower = who.SearchForBorrower();
			if (borrower != null) { txtBorrowerId.Text = borrower.SSN; }
		}//btnWhoAmI_Click()
	}//class
}//namespace
