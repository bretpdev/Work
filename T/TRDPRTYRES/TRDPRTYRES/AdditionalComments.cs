using System.Windows.Forms;

namespace TRDPRTYRES
{
    public partial class AdditionalComments : Form
	{
		public AdditionalComments(BorReferenceInfo bData)
		{
			InitializeComponent();
			borReferenceInfoBindingSource.DataSource = bData;
		}
	}
}