using System.Drawing;
using System.Windows.Forms;

namespace ACDCAccess
{
	partial class UserKeyInfoForRemovingAccess : UserControl
	{
		public UserAccessKey KeyData { get; set; }
		public bool Checked
		{
			get
			{
				return chkRemove.Checked;
			}
		}

		public UserKeyInfoForRemovingAccess()
		{
			InitializeComponent();
			lblRemove.Visible = true;
			chkRemove.Visible = false;
		}

		public UserKeyInfoForRemovingAccess(UserAccessKey uak)
		{
			InitializeComponent();
			lblDescription.MaximumSize = new Size(lblDescription.Width, 0); //allows the description label to grow vertically (for some reason when you do this in the designer it switches the height to a number other that 0 when you compile which messes everything up)
			lblBusinessUnit.MaximumSize = new Size(lblBusinessUnit.Width, 0); //allows the business unit label to grow vertically (for some reason when you do this in the designer it switches the height to a number other that 0 when you compile which messes everything up)
			lblKey.MaximumSize = new Size(lblKey.Width, 0); //allows the key label to grow vertically (for some reason when you do this in the designer it switches the height to a number other that 0 when you compile which messes everything up)
			lblRemove.Visible = false;
			chkRemove.Visible = true;
			KeyData = uak;
			userAccessKeyBindingSource.DataSource = KeyData;
		}
	}
}
