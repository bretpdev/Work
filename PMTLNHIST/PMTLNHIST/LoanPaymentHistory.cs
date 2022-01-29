using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;
using static Uheaa.Common.Dialog;

namespace PMTLNHIST
{
    public class LoanPaymentHistory : ScriptBase
    {
        public DataAccess DA { get; set; }
        public new ReflectionInterface RI { get; set; }
        public new string ScriptId { get; set; } = "PMTLNHIST";

        public LoanPaymentHistory(ReflectionInterface ri)
            : base()
        {
            RI = ri;
            RI.LogRun = RI.LogRun ?? new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DA = new DataAccess(ri.LogRun.LDA);
        }

        public override void Main()
        {
            string ssn = GetSsn();
            if (ssn.IsNullOrEmpty())
                return;

            BorrowerData borrowerData = GetBorrowerData(ssn);
            if (borrowerData == null)
                return;
            DA.InsertBorrowerData(borrowerData);
            DA.InsertLoanData(borrowerData.AccountNumber);
            System.Diagnostics.Process.Start($"{EnterpriseFileSystem.GetPath("pmtlnhistreport", DataAccessHelper.Region.Uheaa)}&AccountIdentifier={borrowerData.AccountNumber}");
        }

        /// <summary>
        /// Checks to see if the 
        /// </summary>
        /// <returns></returns>
        public string GetSsn(string account = null)
        {
            string ssn = "";
            if (account.IsNullOrEmpty())
            {
                while (ssn.Length < 9 || ssn.Length > 10)
                {
                    if (RI.GetText(1, 9, 9).ToIntNullable().HasValue)
                    {
                        ssn = RI.GetText(1, 9, 9);
                        if (!Question.YesNo($"Did you want to create a history report for {RI.GetText(2, 2, 40)}; SSN: {ssn}?", "Create Report?"))
                            return "";
                    }
                    else
                    {
                        InputBox<RequiredAccountNumberTextBox> acct = new InputBox<RequiredAccountNumberTextBox>("SSN or Account Number for the borrower.", "Loan Payment History");
                        if (acct.ShowDialog() == DialogResult.Cancel)
                            return "";
                        else
                            ssn = acct.InputControl.Text;
                    }
                }
            }
            else
                ssn = account;
            RI.FastPath($"LP22I{(ssn.Length == 9 ? "" : ";;;;;;")}{ssn}");
            return RI.GetText(3, 23, 9);
        }

        public BorrowerData GetBorrowerData(string ssn)
        {
            BorrowerData bData = new BorrowerData();
            if (!RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS"))
                return ShowError("LP22");
            bData.AccountNumber = RI.GetText(3, 60, 12).Replace(" ", "");
            bData.Name = $"{RI.GetText(4, 44, 13).Trim()} {RI.GetText(4, 60, 2).Trim()} {RI.GetText(4, 5, 36).Trim()}";
            RI.FastPath($"LC10I{ssn}");
            if (RI.AltMessageCode == "45003")
                return ShowError("LC10");
            bData.Principal = RI.GetText(8, 70, 11).ToDouble();
            bData.Interest = RI.GetText(9, 70, 11).ToDouble();
            bData.LegalCosts = RI.GetText(10, 70, 11).ToDouble();
            bData.OtherCosts = RI.GetText(11, 70, 11).ToDouble();
            bData.CollectionCosts = RI.GetText(12, 70, 11).ToDouble();
            bData.ProjectedCollectionCosts = RI.GetText(17, 35, 11).ToDouble();
            return bData;
        }

        public BorrowerData ShowError(string screen)
        {
            string message = $"The borrower was not found in {screen}. Please check the borrower account and try again";
            Error.Ok(message);
            return null;
        }
    }
}