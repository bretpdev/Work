using System.Windows.Forms;
using Q;

namespace CALLFWD
{
    public partial class BusinessUnitDialog : FormBase
    {
        public enum BusinessUnit
        {
            None = 0,
            AuxServices = 1,
            BorrowerServices = 2,
            LoanManagement = 4,
            PostClaim = 8,
            FedLoanServicing = 16
        }

        public BusinessUnitDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(BusinessUnit businessUnit)
        {
            //Show the selected business unit(s).
            lblAuxServices.Visible = ((businessUnit & BusinessUnit.AuxServices) == BusinessUnit.AuxServices);
            lblBorrowerServices.Visible = ((businessUnit & BusinessUnit.BorrowerServices) == BusinessUnit.BorrowerServices);
            lblLoanManagement.Visible = ((businessUnit & BusinessUnit.LoanManagement) == BusinessUnit.LoanManagement);
            lblPostClaim.Visible = ((businessUnit & BusinessUnit.PostClaim) == BusinessUnit.PostClaim);
            lblFedLoanServicing.Visible = ((businessUnit & BusinessUnit.FedLoanServicing) == BusinessUnit.FedLoanServicing);
            return base.ShowDialog();
        }//ShowDialog()
    }//class
}//namespace
