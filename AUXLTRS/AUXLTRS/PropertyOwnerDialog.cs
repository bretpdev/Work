using Q;

namespace AUXLTRS
{
    public partial class PropertyOwnerDialog : FormBase
    {
        public PropertyOwnerDialog(bool testMode, PropertyOwner propertyOwner)
        {
            InitializeComponent();

            this.propertyOwnerBindingSource.DataSource = propertyOwner;
            cmbState.DataSource = DataAccess.GetStateAbbreviations(testMode);
            propertyOwner.State = "UT"; //Default the state to Utah.
        }
    }//class
}//namespace
