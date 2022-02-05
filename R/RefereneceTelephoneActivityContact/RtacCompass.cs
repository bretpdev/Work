using System.Windows.Forms;
using Q;

namespace RefereneceTelephoneActivityContact
{
	public class RtacCompass : ScriptBase
	{
		public RtacCompass(ReflectionInterface ri)
			: base(ri, "RTACCOMPAS")
		{
		}

		public override void Main()
		{
			//Check that we're on ITX1J for a reference.
			if (!Check4Text(1, 71, "TXX1R-03"))
			{
				string message = "You must be on the TX1JR screen to run this application. Please access the correct screen and try again.";
				MessageBox.Show(message, "Wrong Screen Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				EndDLLScript();
			}

			//Get the borrower and reference IDs and create a new ContactDetail object from them.
			string borrowerSsn = GetText(7, 11, 11).Replace(" ", "");
			string referenceId = GetText(3, 12, 11).Replace(" ", "");
			ContactDetail contactDetail = new ContactDetail(TestModeProperty, borrowerSsn, referenceId);

			//Show the contact form.
			ReferenceCall contactForm = new ReferenceCall(TestModeProperty, contactDetail);
			if (contactForm.ShowDialog() != DialogResult.OK) { return; }

			//Add system comments.
			foreach (string arc in contactDetail.Arcs)
			{
				string comment = string.Format("{0} [{1}]", contactDetail.Comment, referenceId);
				ATD22AllLoans(contactDetail.BorrowerId, arc, comment, false);
			}
		}//Main()
	}//class
}//namespace
