using Q;

namespace AUXLTRS
{
    public partial class PostOfficeDialog : FormBase
    {
        public PostOfficeDialog(bool testMode, PostOfficeBorrower postOfficeBorrower)
        {
            InitializeComponent();

            this.postOfficeBorrowerBindingSource.DataSource = postOfficeBorrower;
            cmbState.DataSource = DataAccess.GetStateAbbreviations(testMode);
            postOfficeBorrower.State = "UT"; //Default the state to Utah.
        }//constructor
    }//class
}//namespace
