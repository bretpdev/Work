using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using static Microsoft.VisualBasic.Financial;
using static System.Math;
using static Uheaa.Common.Dialog;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace REPAYTERM
{
    public partial class RepaymentTermForm : Form
    {
        public ReflectionInterface RI { get; set; }
        public RepaymentData RepayData { get; set; }
        public DataAccess DA { get; set; }

        public RepaymentTermForm(ReflectionInterface ri)
        {
            InitializeComponent();
            RI = ri;
            DA = new DataAccess(RI.LogRun.LDA);

            ClearFields();
            VersionLbl.Text = $"Version: {Assembly.GetExecutingAssembly().GetName().Version}";
            PayInFullBy.Text = string.Format(PayInFullBy.Text, DateTime.Now.AddDays(30).ToShortDateString());
        }

        private void AccountIdentifier_TextChanged(object sender, EventArgs e)
        {
            if (AccountIdentifier.Text.Length >= 9)
            {
                if (!CheckLoggedIn())
                    return;
                RepayData = GetRepayData();
                if (RepayData == null)
                    return;
                if (AccountIdentifier.Text.Length == 9)
                    RI.FastPath($"LP22I{AccountIdentifier.Text}");
                else
                    RI.FastPath($"LP22I;;;;;;{AccountIdentifier.Text}");
                if (RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS") && RepayData.Ssn.IsPopulated())
                {
                    NameLbl.Text = RepayData.Name;
                    RepayData.Payoff = GetPayoff(RepayData.Ssn);
                    GetBalance();
                    if (RepayData.Payoff > 0.00)
                        DisplayTotal();
                    OK.Enabled = true;
                    RepayAmount.Focus();
                }
                else
                {
                    Info.Ok("The borrower was not found in the warehouse");
                    ClearFields();
                }
            }
            else
                ClearFields();
        }

        private bool CheckLoggedIn()
        {
            try
            {
                RI.FastPath("LP22I");
                return (RI.CheckForText(1, 62, "PERSON") || RI.CheckForText(1, 60, "PERSON"));
            }
            catch (Exception)
            {
                Error.Ok("Please log into the session to run this script.");
                AccountIdentifier.Text = "";
                return false;
            }
        }

        private RepaymentData GetRepayData()
        {
            RepaymentData data = DA.GetRepayData(AccountIdentifier.Text);
            if (data == null)
            {
                Info.Ok("The borrower does not have any open default loans.", "No Open Default Loans");
                ClearFields();
                AccountIdentifier.Text = "";
                return null;
            }
            if (data.HasSC)
            {
                Info.Ok("The account has an open specialty claim so no repayment term may be calculated.", "Specialty Claim");
                ClearFields();
                AccountIdentifier.Text = "";
                return null;
            }
            if (data.HasNewLoan)
                Info.Ok($"The borrower has a newly defaulted loan.  Explain to the borrower that collection costs will be assessed and the debt will be reported to national credit reporting agencies if the account is not paid in full or payment arrangements are not made by {RepayData.PayoffDate?.AddDays(61).ToShortDateString()}", "New Default");

            return data;
        }

        /// <summary>
        /// Gets the outstanding balance due in 30 days from today
        /// </summary>
        private double GetPayoff(string ssn)
        {
            RI.FastPath($"LC10I{ssn}");
            if (RI.AltMessageCode.IsIn("45003", "48012"))
            {
                Error.Ok("THe script is unable to calculate repayment information because the borrower is not in LC10");
                return 0.00;
            }
            RI.PutText(9, 20, DateTime.Now.AddDays(30).ToTenDigitDate().Replace("/", ""), Key.Enter);
            return RI.GetText(18, 36, 10).ToDouble();
        }

        private void GetBalance()
        {
            RI.FastPath($"LC05I{RepayData.Ssn}");
            RepayData.Balance = RI.GetText(4, 69, 12).ToDouble();
            RepayData.Interest = RI.GetText(4, 43, 12).ToDouble();

            //Borrower has a loan amount over $31,000 and all loans are newer than 10/7/1998
            if (RepayData.HasExtendedLoans)
            {
                TwentyFiveYear.Visible = true;
                TwentyFiveYearLbl.Visible = true;
            }
        }

        private void DisplayTotal()
        {
            AccountBalance.Text = $"$ {RepayData.Balance:0.00}";
            MonthlyInterest.Text = $"$ {RepayData.Interest:0.00}";
            PayInFull.Text = $"$ {RepayData.Payoff:0.00}";
            ThreeYearPayoff.Text = $"$ {Round(Pmt(RepayData.WeightedRate / 100 / 12, 36, -RepayData.Balance), 0)}";
            PercentBalance.Text = $"$ {Round((RepayData.Balance * .02), 0)}";
            SevenYearPayoff.Text = $"$ {Round(Pmt(RepayData.WeightedRate / 100 / 12, 84, -RepayData.Balance), 0)}";
            TenYearPayoff.Text = $"$ {Round(Pmt(RepayData.WeightedRate / 100 / 12, 120, -RepayData.Balance), 0)}";
            TwentyFiveYear.Text = $"$ {Round(Pmt(RepayData.WeightedRate / 100 / 12, 300, -RepayData.Balance), 0)}";
        }

        private void ClearFields()
        {
            NameLbl.Text = "";
            AccountBalance.Text = "";
            MonthlyInterest.Text = "";
            PayInFull.Text = "";
            ThreeYearPayoff.Text = "";
            PercentBalance.Text = "";
            SevenYearPayoff.Text = "";
            TenYearPayoff.Text = "";
            TwentyFiveYear.Text = "";
            TwentyFiveYear.Visible = false;
            TwentyFiveYearLbl.Visible = false;
            OK.Enabled = false;
            RepayAmount.Text = "";
            MessageLbl.Text = "";
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!CheckLoggedIn())
                return;
            double amount = RepayAmount.Text.ToDouble();
            if (amount <= RepayData.Interest)
                MessageLbl.Text = $"The proposed payment amount of ${amount} is less than or equal to monthly interest and may be accepted as Rehabilitation repayment arrangement, but will require a FBS form to be completed and returned.  Update the proposed payment amount and click OK to review another amount, or click Close to quit.";
            else
            {
                try
                {
                    double numPmts = Round(NPer(RepayData.WeightedRate / 100 / 12, -amount, RepayData.Balance), 0);
                    int years = ((int)(numPmts / 12));
                    int months = ((int)Round((numPmts / 12 - years) * 12, 0));
                    if (numPmts <= 120 && amount >= 50)
                        MessageLbl.Text = $"The proposed payment amount of ${amount} will pay the outstanding balance in {years} years and {months} months and is greater than or equal to $50 so the proposed amount may be accepted on a permanent basis.  Update the proposed payment amount and click OK to review another amount, or click Close to quit.";
                    else if (amount >= 50)
                        MessageLbl.Text = $"The proposed payment amount of ${amount} will pay the outstanding balance in {years} years and {months} months but is equal to or greater than $50 and is more than the monthly interest so the proposed amount may be accepted as Rehabilitation repayment arrangement, but will require a FBS form to be completed and returned.  Update the proposed payment amount and click OK to review another amount, or click Close to quit.";
                    else
                        MessageLbl.Text = $"The proposed payment amount of ${amount} will pay the outstanding balance in {years} years and {months} months but is less than $50 so it will require an FBS.  Update the proposed payment amount and click OK to review another amount, or click Close to quit.";
                }
                catch (Exception)
                {
                    //The NPer function could not calculate the number of payments and threw and exception.
                    if (amount >= 50)
                        MessageLbl.Text = $"The proposed payment amount of ${amount} is more than monthly interest but the script is unable to calculate the pay off term because the term would be too long.  However, the proposed amount is greater than or equal to $50 so it may be accepted as Rehabilitation repayment arrangement, but will require a FBS form to be completed and returned.  Update the proposed payment amount and click OK to review another amount, or click Close to quit.";
                    else
                        MessageLbl.Text = $"The proposed payment amount of ${amount} is more than monthly interest but the script is unable to calculate the pay off term because the term would be too long.  Since the proposed amount is less than $50, it may be accepted as Rehabilitation repayment arrangement, but will require a FBS form to be completed and returned.  Update the proposed payment amount and click OK to review another amount, or click Close to quit.";
                }
            }
        }

        private void RepayAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                OK_Click(sender, new EventArgs());
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            AccountIdentifier.Text = "";
            AccountIdentifier.Focus();
        }
    }
}