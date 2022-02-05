using System.Drawing;
using System.Windows.Forms;

namespace ACDCAccess
{
	partial class UserKeyInfoForAddingAccess : UserControl
	{
		public Key KeyData { get; set; }
		public bool Checked
		{
			get
			{
				return chkAdd.Checked;
			}
		}

		public UserKeyInfoForAddingAccess()
		{
			InitializeComponent();
			lblAdd.Visible = true;
			chkAdd.Visible = false;
		}

		public UserKeyInfoForAddingAccess(Key uak)
		{
			InitializeComponent();
			lblDescription.MaximumSize = new Size(lblDescription.Width, 0); //allows the description label to grow vertically (for some reason when you do this in the designer it switches the height to a number other that 0 when you compile which messes everything up)
			lblAdd.Visible = false;
			chkAdd.Visible = true;
			KeyData = uak;
			keyBindingSource.DataSource = KeyData;
		}
	}
}
