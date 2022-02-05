using System.Windows.Forms;
using Q;

namespace AUXLTRS
{
    public partial class SatisfactionDialog : FormBase
    {
        public enum Reason
        {
            SmallBalance,
            EmployerRequest,
            Bankruptcy,
            Other
        }

        private Satisfaction _satisfiedBorrower;

        public SatisfactionDialog(Satisfaction satisfiedBorrower)
        {
            InitializeComponent();

            _satisfiedBorrower = satisfiedBorrower;
            this.satisfiedBorrowerBindingSource.DataSource = satisfiedBorrower;
        }

        private void SetReasonAndReturn(Reason reason)
        {
            _satisfiedBorrower.Reason = reason;
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        #region Event Handlers
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Hide();
        }

        private void chkBankruptcy_Click(object sender, System.EventArgs e)
        {
            SetReasonAndReturn(Reason.Bankruptcy);
        }

        private void chkEmployerRequest_Click(object sender, System.EventArgs e)
        {
            SetReasonAndReturn(Reason.EmployerRequest);
        }

        private void chkOther_Click(object sender, System.EventArgs e)
        {
            SetReasonAndReturn(Reason.Other);
        }

        private void chkSmallBalance_Click(object sender, System.EventArgs e)
        {
            SetReasonAndReturn(Reason.SmallBalance);
        }
        #endregion Event Handlers
    }//class
}//namespace
