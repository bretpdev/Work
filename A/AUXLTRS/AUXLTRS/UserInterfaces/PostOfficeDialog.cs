using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;

namespace AUXLTRS
{
    public partial class PostOfficeDialog : Form
    {
        public PostOfficeDialog(PostOfficeBorrower postOfficeBorrower, DataAccess da)
        {
            InitializeComponent();

            this.postOfficeBorrowerBindingSource.DataSource = postOfficeBorrower;
            cmbState.DataSource = da.GetStateAbbreviations() ;
            postOfficeBorrower.State = "UT"; //Default the state to Utah.
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if ((txtSsn.Text.Length == 9 || txtSsn.Text.Length ==10) && txtCity.Text != "" && txtZip.Text != "")
            {
                
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}