using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDRUSERPRO
{
    public partial class RepaymentPlan : UserControl
    {
        public delegate void OnPlanSelected(RepaymentPlan sender);
        public event OnPlanSelected PlanSelected;
        public RepaymentPlan()
        {
            InitializeComponent();
            this.Enabled = false;
        }

        public string PlanTitle
        {
            get
            {
                return PlanTitleLink.Text;
            }
            set
            {
                PlanTitleLink.Text = value;
            }
        }

        public void ClearItems()
        {
            this.Enabled = false;
            LoansBox.Items.Clear();
            MonthlyInstallmentLabel.ForeColor = Color.Black;
            DeselectPlan();
            MonthlyInstallmentLabel.Text = "";
        }

        public LpcResults.PlanResult LoadedPlan { get; private set; }
        public void LoadPlan(LpcResults.PlanResult plan, List<LoanSequenceEligibility> eligibilityIndicators)
        {
            this.Enabled = true;
            LoansBox.Items.Clear();
            LoadedPlan = plan;
            if (plan.Status == LpcResults.ResultStatus.Successful)
            {
                MonthlyInstallmentLabel.Text = "$" + plan.MonthlyInstallment.ToString("0.00") + "/month";
                foreach (var ind in eligibilityIndicators.Where(o => o.FutureEligibilityIndicator != "I").OrderBy(p => p.LoanSequence))
                    LoansBox.Items.Add(string.Format("{0} - {1} - {2}", ind.LoanSequence, ind.LoanProgram, ind.CurrentBalance));
            }
            else
            {
                DisablePlan(plan.ErrorMessage);
            }
        }

        const int lineCharacterWidth = 20;
        public void DisablePlan(string errorMessage)
        {
            PlanTitleLink.Enabled = false;
            LoansBox.ClearSelected();
            string curMessage = null;
            List<string> errorMessages = new List<string>();
            foreach (var word in errorMessage.Split(' '))
            {
                if (curMessage == null)
                    curMessage = word;
                else
                {
                    if ((curMessage + " " + word).Length > lineCharacterWidth)
                    {
                        errorMessages.Add(curMessage);
                        curMessage = word;
                    }
                    else
                        curMessage += " " + word;
                }
            }
            errorMessages.Add(curMessage);
            foreach (var error in errorMessages)
                LoansBox.Items.Add(error);
        }

        public void EnablePlan()
        {
            PlanTitleLink.Enabled = true;
        }

        public void SelectPlan()
        {
            PlanTitleLink.BackColor = Color.Yellow;
            PlanTitleLink.Font = new Font(PlanTitleLink.Font, FontStyle.Bold);
            PlanSelected?.Invoke(this);
        }

        public void DeselectPlan()
        {
            PlanTitleLink.BackColor = Color.Transparent;
            PlanTitleLink.ForeColor = Color.Blue;
            PlanTitleLink.Enabled = true;
            PlanTitleLink.Font = new Font(PlanTitleLink.Font, FontStyle.Regular);
        }

        public void MarkAsLowestPlan()
        {
            MonthlyInstallmentLabel.ForeColor = Color.Green;
        }

        private void PlanTitleLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectPlan();
        }
    }
}
