using System.Drawing;
using System.Windows.Forms;

namespace ACDCAccess
{
	partial class ResearchResult : UserControl
	{
		public ResearchResult()
		{
			InitializeComponent();
		}

		public ResearchResult(Key key)
		{
			InitializeComponent();
			lblDescription.MaximumSize = new Size(lblDescription.Width, 0); //allows the description label to grow vertically (for some reason when you do this in the designer it switches the height to a number other that 0 when you compile which messes everything up)
			lblKey.MaximumSize = new Size(lblKey.Width, 0); //allows the key label to grow vertically (for some reason when you do this in the designer it switches the height to a number other that 0 when you compile which messes everything up)
			keyBindingSource.DataSource = key;
		}
	}
}
