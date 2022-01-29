using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace IDRUSERPRO
{
    public partial class ExternalLoans : Form
    {
        private DataAccess DA { get; set; }
        public List<OtherLoans> OtherLoansBorrower { get; set; }
        public List<OtherLoans> OtherLoansSpouse { get; set; }
        private List<OtherLoans> OrigBorrower { get; set; }
        private List<OtherLoans> OrigSpouse { get; set; }
        private static List<LoanPrograms> LoanPrograms { get; set; }
        public bool? LoansInSameRegion { get; set; }
        public bool IsPending { get; set; }
        private int RowIndex { get; set; }

        private readonly bool spouseIndicator;
        public ExternalLoans(string type, List<OtherLoans> loans, List<LoanPrograms> loanPrograms, string borrowerSsn, string spouseSsn, bool spouseIndicator, DataAccess da, bool? loansInSameRegion)
        {
            InitializeComponent();
            this.spouseIndicator = spouseIndicator;
            DA = da;
            RowIndex = -1;
            LoansInSameRegion = loansInSameRegion;
            LoanPrograms = loanPrograms;

            if (spouseIndicator)
                BorrowerTypeLabel.Text = "Spouse External Loans";

            if (spouseIndicator)
                LoanSpouseData(loans);
            else
                LoadBwrData(loans);

            if (loans.Any() && DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
                btnCont.Enabled = true;
        }

        private void LoadBwrData(List<OtherLoans> loans)
        {
            OrigBorrower = loans;
            OtherLoansBorrower = loans;
            foreach (OtherLoans item in OtherLoansBorrower)
            {
                item.SetFfelp(LoanPrograms);
                item.SetMonthlyPay();
            }

            SetDataGrid();

            lblCount.Text = OtherLoansBorrower.Count.ToString();
        }

        private void LoanSpouseData(List<OtherLoans> loans)
        {
            OrigSpouse = loans;
            OtherLoansSpouse = loans;
            SameRegion.Enabled = true;
            if (LoansInSameRegion == true)
                SameRegionYes.Checked = true;
            else if (LoansInSameRegion == false)
                SameRegionNo.Checked = true;

            foreach (OtherLoans item in OtherLoansSpouse)
            {
                item.SetFfelp(LoanPrograms);
                item.SetMonthlyPay();
            }

            SetDataGrid();

            lblCount.Text = OtherLoansSpouse.Count.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;
            if (spouseIndicator)
            {
                if (OtherLoansSpouse.Count >= 40)
                {
                    MessageBox.Show("You cannot have more than 40 loans.");
                    return;
                }

                LoansInSameRegion = SameRegionYes.Checked;
                AddSpouseLoans();
                SetDataGrid();
            }
            else
            {
                if (OtherLoansBorrower.Count >= 40)
                {
                    MessageBox.Show("You cannot have more than 40 loans.");
                    return;
                }

                AddBorrowerLoans();
                SetDataGrid();
            }

            ResetFields();
        }

        private void AddSpouseLoans()
        {
            OtherLoans tempObj = new OtherLoans();
            if (OtherLoansSpouse.Any() && RowIndex > -1)
            {
                tempObj = OtherLoansSpouse[RowIndex];
                OtherLoansSpouse.RemoveAt(RowIndex);
            }

            OtherLoansSpouse.Add(new OtherLoans()
            {
                ApplicationId = 0,
                SpouseIndicator = spouseIndicator,
                LoanType = txtLoanType.Text,
                OwnerLender = txtOwnerLender.Text,
                OutstandingBalance = decimal.Parse(txtOutstandingBalance.Text),
                OutstandingInterest = txtOutstandingInterest.Text.ToDecimal(),
                MonthlyPay = txtMonthlyPay.Text.ToDecimalNullable(),
                InterestRate = txtIntRate.Text.ToDecimalNullable(),
                Ffelp = rdoFfelp.Checked,
                DF_PRS_ID = tempObj.DF_PRS_ID,
                LF_FED_AWD = tempObj.LF_FED_AWD,
                LN_FED_AWD_SEQ = tempObj.LN_FED_AWD_SEQ
            });
        }

        private void AddBorrowerLoans()
        {
            OtherLoans tempObj = new OtherLoans();
            if (OtherLoansBorrower.Any() && RowIndex > -1)
            {
                tempObj = OtherLoansBorrower[RowIndex];
                OtherLoansBorrower.RemoveAt(RowIndex);
            }

            OtherLoansBorrower.Add(new OtherLoans()
            {
                ApplicationId = 0,
                SpouseIndicator = spouseIndicator,
                LoanType = txtLoanType.Text,
                OwnerLender = txtOwnerLender.Text,
                OutstandingBalance = decimal.Parse(txtOutstandingBalance.Text),
                OutstandingInterest = txtOutstandingInterest.Text.ToDecimal(),
                MonthlyPay = txtMonthlyPay.Text.ToDecimalNullable(),
                InterestRate = txtIntRate.Text.ToDecimalNullable(),
                Ffelp = rdoFfelp.Checked,
                DF_PRS_ID = tempObj.DF_PRS_ID,
                LF_FED_AWD = tempObj.LF_FED_AWD,
                LN_FED_AWD_SEQ = tempObj.LN_FED_AWD_SEQ

            });
        }

        private void ResetFields()
        {
            lblCount.Text = Loans.Rows.Count.ToString();
            txtIntRate.Text = string.Empty;
            txtLoanType.Text = string.Empty;
            txtMonthlyPay.Text = string.Empty;
            txtOutstandingBalance.Text = string.Empty;
            txtOutstandingInterest.Text = string.Empty;
            txtOwnerLender.Text = string.Empty;
            RowIndex = -1;
        }

        ErrorAttacher ea = new ErrorAttacher();
        private bool ValidateInput()
        {
            ea.ClearAllErrors();
            if (txtOwnerLender.Text.Length < 6)
                ea.SetError("OwnerLender must be 6 characters.", txtOwnerLender, lblOwner);
            if (!IDRUSERPRO.LoanPrograms.Contains(LoanPrograms, txtLoanType.Text.ToUpper()))
            {
                List<string> options = LoanPrograms.Select(o => o.LoanProgram + (string.IsNullOrEmpty(o.NsldsCode) ? "" : " (" + o.NsldsCode + ")")).ToList();
                string message = "Please enter a valid loan type.  Options: " + Environment.NewLine + string.Join(Environment.NewLine, options);
                ea.SetError(message, txtLoanType, lblLoanType);
            }
            var outstandingBalanceDigits = new DigitHelper(txtOutstandingBalance.Text);
            if (!outstandingBalanceDigits.IsValidDecimal)
                ea.SetError("Please enter a valid outstanding balance.", txtOutstandingBalance, lblbalance);
            else if (outstandingBalanceDigits.DigitsBeforeDecimal > 8 || outstandingBalanceDigits.DigitsAfterDecimal > 2)
                ea.SetError("Balance must be no more than 8 digits before the decimal place and no more than 2 digits after the decimal place.", txtOutstandingBalance, lblbalance);

            var outstandingInterestDigits = new DigitHelper(txtOutstandingInterest.Text);
            if (!outstandingInterestDigits.IsValidDecimal)
                ea.SetError("Please enter a valid outstanding interest.", txtOutstandingInterest, lblOutstandingInterest);
            else if (outstandingInterestDigits.DigitsBeforeDecimal > 8 || outstandingInterestDigits.DigitsAfterDecimal > 2)
                ea.SetError("Balance must be no more than 8 digits before the decimal place and no more than 2 digits after the decimal place.", txtOutstandingInterest, lblOutstandingInterest);


            if (txtIntRate.Text.ToDecimalNullable() == null || txtIntRate.Text.ToDecimalNullable() > 99)
                ea.SetError("Please enter a valid interest rate.", txtIntRate, lblInterestrate);
            if (SameRegion.Enabled && !SameRegionYes.Checked && !SameRegionNo.Checked)
            {
                ea.SetError("Are loans in same region?", SameRegionYes);
                ea.SetError("Are loans in same region?", SameRegionNo);
            }
            return ea.ErrorCount == 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OtherLoansSpouse?.Clear();
            OtherLoansBorrower?.Clear();
            DialogResult = DialogResult.Cancel;
        }

        private void btnCont_Click(object sender, EventArgs e)
        {
            if (SameRegionNo.Enabled && (!SameRegionNo.Checked && !SameRegionYes.Checked))
            {
                Dialog.Error.Ok("Please check spouse in same region.");
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (spouseIndicator)
                ResetSpouse();
            else
                ResetBorrower();

            ResetFields();
            DisableFields();
        }

        private void DisableFields()
        {
            if (spouseIndicator)
            {
                if (OtherLoansSpouse.Count < 1)
                {
                    btnCont.Enabled = false;
                    btnRemove.Enabled = false;
                }
            }
            else
            {
                if (OtherLoansBorrower.Count < 1)
                {
                    btnCont.Enabled = false;
                    btnRemove.Enabled = false;
                }
            }
        }

        private void ResetBorrower()
        {
            OtherLoansBorrower.RemoveAt(Loans.CurrentRow.Index);
            SetDataGrid();
        }

        private void ResetSpouse()
        {
            OtherLoansSpouse.RemoveAt(Loans.CurrentRow.Index);
            SetDataGrid();
        }

        private void txtLoanType_Leave(object sender, EventArgs e)
        {
            if (txtLoanType.Text.IsPopulated() && LoanPrograms.Select(p => p.NsldsCode).ToList().Contains(txtLoanType.Text))
                txtLoanType.Text = LoanPrograms.Where(p => p.NsldsCode == txtLoanType.Text).SingleOrDefault().LoanProgram;

            CalcPmt();
            bool isDirect = LoanPrograms.SingleOrDefault(o => o.LoanProgram == txtLoanType.Text)?.IsDirect ?? false;
            if (isDirect)
                rdoDirect.Checked = true;
            else
                rdoFfelp.Checked = true;
        }

        private void CalcPmt()
        {
            if (txtIntRate.Text.IsPopulated() && txtLoanType.Text.IsPopulated() && txtOutstandingBalance.Text.IsPopulated() && txtOwnerLender.Text.IsPopulated())
            {
                double val = Math.Round(Math.Abs(Financial.Pmt(((txtIntRate.Text.ToDouble() / 100) / 12), 120, txtOutstandingBalance.Text.ToDouble())), 2);
                if (val < 1)
                    val = 1;
                txtMonthlyPay.Text = val.ToString();
            }
        }

        private void txtOwnerLender_Leave(object sender, EventArgs e)
        {
            CalcPmt();
        }

        private void txtOutstandingBalance_TextChanged(object sender, EventArgs e)
        {
            CalcPmt();
        }

        private void txtIntRate_Leave(object sender, EventArgs e)
        {
            CalcPmt();
        }

        private void SetDataGrid()
        {
            List<OtherLoans> pop = new List<OtherLoans>();
            if (spouseIndicator)
                pop = OtherLoansSpouse;
            else
                pop = OtherLoansBorrower;

            if (pop.Any())
            {
                Loans.DataSource = null;
                Loans.DataSource = pop;
                foreach (PropertyInfo pi in pop.First().GetType().GetProperties())
                {
                    DataGridViewAttribute attr = pi.GetCustomAttribute<DataGridViewAttribute>();
                    if (attr == null)
                        continue;
                    Loans.Columns[attr.Index].HeaderText = attr.DisplayName;
                    Loans.Columns[attr.Index].Visible = attr.Visible;
                    if (attr.Format.IsPopulated())
                        Loans.Columns[attr.Index].DefaultCellStyle.Format = attr.Format;
                }

                Loans.Columns["Coordinator"].Visible = false;  //unused properties
                if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live)
                    Loans.Columns["CalculatedOutstandingBalance"].Visible = false; //show in test only

                var noErrors = ea.ErrorCount == 0;
                btnCont.Enabled = noErrors;
            }
            else
            {
                Loans.DataSource = null;
            }
        }

        private void Loans_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (spouseIndicator)
                    SetSpouse(e.RowIndex);
                else
                    SetBorrower(e.RowIndex);

                RowIndex = e.RowIndex;

                btnRemove.Enabled = true;
            }
        }

        private void SetBorrower(int index)
        {
            txtIntRate.Text = OtherLoansBorrower[index].InterestRate.ToString();
            txtLoanType.Text = OtherLoansBorrower[index].LoanType;
            txtMonthlyPay.Text = string.Format("{0:0.00}", OtherLoansBorrower[index].MonthlyPay);
            txtOutstandingBalance.Text = string.Format("{0:0.00}", OtherLoansBorrower[index].OutstandingBalance);
            txtOutstandingInterest.Text = string.Format("{0:0.00}", OtherLoansBorrower[index].OutstandingInterest);
            txtOwnerLender.Text = OtherLoansBorrower[index].OwnerLender;
            if (OtherLoansBorrower[index].Ffelp)
                rdoFfelp.Checked = true;
            else
                rdoDirect.Checked = true;
        }

        private void SetSpouse(int index)
        {
            txtIntRate.Text = OtherLoansSpouse[index].InterestRate.ToString();
            txtLoanType.Text = OtherLoansSpouse[index].LoanType;
            txtMonthlyPay.Text = string.Format("{0:0.00}", OtherLoansSpouse[index].MonthlyPay);
            txtOutstandingBalance.Text = string.Format("{0:0.00}", OtherLoansSpouse[index].OutstandingBalance);
            txtOutstandingInterest.Text = string.Format("{0:0.00}", OtherLoansSpouse[index].OutstandingInterest);
            txtOwnerLender.Text = OtherLoansSpouse[index].OwnerLender;
            if (OtherLoansSpouse[index].Ffelp)
                rdoFfelp.Checked = true;
            else
                rdoDirect.Checked = true;

        }


        private void LoansOtherServicers_Load(object sender, EventArgs e)
        {
            //SetBackground();
        }

        private void Field_TextChanged(object sender, EventArgs e)
        {
            if (ea.ErrorCount > 0)
                ValidateInput();
        }

        private void SameRegion_CheckedChanged(object sender, EventArgs e)
        {
            if (SameRegionYes.Checked)
                MessageBox.Show("Spouse has loans in the same region, please process manually.");

            if (ea.ErrorCount > 0)
                ValidateInput();
        }

        private void txtOutstandingBalance_Leave(object sender, EventArgs e)
        {
            CalcPmt();
        }
    }
}