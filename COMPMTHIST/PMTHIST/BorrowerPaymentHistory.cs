using System;
using System.Windows.Forms;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using Uheaa.Common.Scripts;

namespace COMPMTHIST
{
    public class BorrowerPaymentHistory : ScriptBase
    {

        private readonly Dictionary<string, string> _paymentTypeTranslation;

        public BorrowerPaymentHistory(ReflectionInterface ri)
            : base(ri, "COMPMTHIST")
        {
            _paymentTypeTranslation = CreateAndPopulatePaymentTypeTranslationDictionary();
        }

        public override void Main()
        {
            BorrowerPaymentInformation bpi = new BorrowerPaymentInformation();
            MainFrm mainForm = new MainFrm(bpi);
            //show the form once to get the SSN
            if (mainForm.ShowDialog() == DialogResult.Cancel)
                return;

            try
            {
                GetLoanInformation(bpi);
            }
            catch (InvalidTS26BorrowerException)
            {
                MessageBox.Show("Invalid SSN.  Please try again.", "Invalid SSN", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //show the form again to get a loan
            mainForm = new MainFrm(bpi);
            List<Loan> userSelectedLoans;
            if (mainForm.ShowDialog() == DialogResult.Cancel)
                return;
            userSelectedLoans = mainForm.UserSelectedLoans;
            GetPaymentInformation(bpi, userSelectedLoans);
            GetBalanceInformation(bpi, userSelectedLoans);
            CreateAndDisplayExcelReportOfPaymentHistory(bpi);
            MessageBox.Show("Processing Complete!", "Processing Complete!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void CreateAndDisplayExcelReportOfPaymentHistory(BorrowerPaymentInformation bpi)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            //sort comma delimited file in excel, create report, print report
            excelApp.Visible = true;
            //create blank spreadsheet
            Microsoft.Office.Interop.Excel.Workbook workBook = excelApp.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet workSheet = (excelApp.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet);
            workSheet.get_Range("A1", "L1").MergeCells = true;
            workSheet.get_Range("A1", "L1").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.get_Range("A1", "L1").Font.Bold = true;
            workSheet.get_Range("A1", "L1").EntireColumn.NumberFormat = "@";
            workSheet.Cells[1, 1] = "UHEAA BORROWER TRANSACTION HISTORY";
            workSheet.Cells[2, 1] = "NAME:";
            workSheet.Cells[3, 1] = "SSN:";
            workSheet.get_Range("A2", "A3").Font.Bold = true;
            workSheet.Cells[2, 3] = bpi.BorrowerName;
            workSheet.Cells[3, 3] = bpi.SSN.Insert(5, "-").Insert(3, "-");
            //header rows
            workSheet.get_Range("F4", "H4").MergeCells = true;
            workSheet.Cells[4, 6] = "Transaction Applied";
            workSheet.get_Range("I4", "K4").MergeCells = true;
            workSheet.Cells[4, 9] = "Refund Or Reversal";
            workSheet.Cells[5, 1] = string.Format("Effective{0}Date", "\n");
            workSheet.Cells[5, 2] = string.Format("Posted{0}Date", "\n");
            workSheet.Cells[5, 3] = string.Format("Transaction{0}Type", "\n");
            workSheet.Cells[5, 4] = string.Format("Beginning {0}Principal{0}Balance", "\n");
            workSheet.Cells[5, 5] = string.Format("Transaction{0}Amount", "\n");
            workSheet.Cells[5, 6] = "Principal";
            workSheet.Cells[5, 7] = "Interest";
            workSheet.Cells[5, 8] = string.Format("Late{0}Fees", "\n");
            workSheet.Cells[5, 9] = "Amount";
            workSheet.Cells[5, 10] = "Type";
            workSheet.get_Range("A4", "L5").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.get_Range("A4", "L5").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignBottom;
            workSheet.get_Range("A4", "L5").Font.Bold = true;
            int i = bpi.Transactions.Count();
            int row = 6;
            double transactionTotal = 0.0;
            double principalAppliedTotal = 0.0;
            double interestTotal = 0.0;
            double lateFeeTotal = 0.0;
            double revOrRefTotal = 0.0;
            double borrPaymentsTotal = 0.0;
            double capInterestTotal = 0.0;
            while (i != 0)
            {
                i--;
                workSheet.Cells[row, 1] = bpi.Transactions[i].EffectiveDate;
                workSheet.Cells[row, 2] = bpi.Transactions[i].PostedDate;
                workSheet.Cells[row, 3] = bpi.Transactions[i].Type;
                if (bpi.Transactions[i].Type == "Borrower Payment")
                {
                    borrPaymentsTotal = borrPaymentsTotal + bpi.Transactions[i].FinancialActAmount; //figure total
                }
                else if (bpi.Transactions[i].Type == "Capped Interest")
                {
                    capInterestTotal = capInterestTotal + bpi.Transactions[i].AppliedPrincipalAmount; //figure total
                }
                workSheet.Cells[row, 4] = string.Format("{0:c}", bpi.Transactions[i].BeginningPrincipalBalance);
                if (bpi.Transactions[i].Type == "Capped Interest")
                {
                    workSheet.Cells[row, 5] = string.Format("{0:c}", bpi.Transactions[i].AppliedPrincipalAmount);
                    transactionTotal = transactionTotal + bpi.Transactions[i].AppliedPrincipalAmount; //figure total
                }
                else
                {
                    workSheet.Cells[row, 5] = string.Format("{0:c}", bpi.Transactions[i].FinancialActAmount);
                    transactionTotal = transactionTotal + bpi.Transactions[i].FinancialActAmount; //figure total
                }
                workSheet.Cells[row, 6] = string.Format("{0:c}", bpi.Transactions[i].AppliedPrincipalAmount);
                principalAppliedTotal = principalAppliedTotal + bpi.Transactions[i].AppliedPrincipalAmount; //figure total
                workSheet.Cells[row, 7] = string.Format("{0:c}", bpi.Transactions[i].AppliedInterestAmount);
                interestTotal = interestTotal + bpi.Transactions[i].AppliedInterestAmount; //figure total
                workSheet.Cells[row, 8] = string.Format("{0:c}", bpi.Transactions[i].AppliedLateFeeAmount);
                lateFeeTotal = lateFeeTotal + bpi.Transactions[i].AppliedLateFeeAmount; //figure total
                if (bpi.Transactions[i].ReversalReason.Length != 0)
                {
                    workSheet.Cells[row, 9] = string.Format("{0:c}", bpi.Transactions[i].FinancialActAmount);
                    revOrRefTotal = revOrRefTotal + bpi.Transactions[i].FinancialActAmount; //figure total
                }
                else
                {
                    workSheet.Cells[row, 9] = "$0.00";
                }
                workSheet.Cells[row, 10] = bpi.Transactions[i].ReversalReason;
                row++;
            }
            //skip a row
            row++;
            //add totals to report
            //Transactions
            row++;
            workSheet.Cells[row, 1] = "Transaction Total:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", Math.Abs(transactionTotal));
            //Principal applied
            row++;
            workSheet.Cells[row, 1] = "Amount Applied To Principal Total:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", Math.Abs(principalAppliedTotal));
            //Interest applied
            row++;
            workSheet.Cells[row, 1] = "Amount Applied To Interest Total:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", Math.Abs(interestTotal));
            //Late fees
            row++;
            workSheet.Cells[row, 1] = "Amount Applied To Late Fees Total:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", Math.Abs(lateFeeTotal));
            //Reversal Amount
            row++;
            workSheet.Cells[row, 1] = "Reversal/Refund Total:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", Math.Abs(revOrRefTotal));
            //Borrower Payments
            row++;
            workSheet.Cells[row, 1] = "Borrower Payments Total:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", Math.Abs(borrPaymentsTotal));
            //Capped interest
            row++;
            workSheet.Cells[row, 1] = "Capped Interest Total:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", Math.Abs(capInterestTotal));
            //pay off
            row++;
            workSheet.Cells[row, 1] = "Payoff Amount:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", bpi.BalanceInfo.PayOffAmount);
            //principal due
            row++;
            workSheet.Cells[row, 1] = "Principal Due:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", bpi.BalanceInfo.PrincipalDue);
            //Interest Due
            row++;
            workSheet.Cells[row, 1] = "Interest Due:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", bpi.BalanceInfo.InterestDue);
            //Past Due Amount
            row++;
            workSheet.Cells[row, 1] = "Past Due Amount:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", bpi.BalanceInfo.PastDueAmount);
            //late fees due
            row++;
            workSheet.Cells[row, 1] = "Late Fees Due:";
            workSheet.Cells[row, 3] = string.Format("{0:c}", bpi.BalanceInfo.LateFeesDue);
            workSheet.get_Range("A1", "L1").EntireColumn.AutoFit();
        }

        private void GetPaymentInformation(BorrowerPaymentInformation bpi, List<Loan> userSelectedLoans)
        {
            int row = 7;
            RI.FastPath("TX3ZITS2C" + bpi.SSN);
            RI.PutText(8, 2, "", true);
            RI.PutText(10, 2, "X", Key.Enter);
            //check if the script found a selection or target screen
            if (RI.CheckForText(1, 72, "TSX2M"))
            {
                //selection screen
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    var seqNum = RI.GetText(row, 14, 4);
                    if (userSelectedLoans.Any(o => o.SequenceNum == seqNum)) //if loan was selected by borrower then gather payment information for it
                    {
                        //blank previous selection if any
                        RI.PutText(21, 18, "", true);
                        //select row
                        RI.PutText(21, 18, RI.GetText(row, 2, 2), Key.Enter);
                        GetFinancialDataForLoan(bpi.Transactions);
                        RI.Hit(Key.F12); //back up to loan selection screen
                    }
                    row++;
                    if (RI.CheckForText(row, 3, " "))
                    {
                        row = 7;
                        RI.Hit(Key.F8);
                    }
                }
            }
            else
            {
                //target screen (only one loan)
                GetFinancialDataForLoan(bpi.Transactions);
            }
        }

        private void GetBalanceInformation(BorrowerPaymentInformation bpi, List<Loan> userSelectedLoans)
        {
            int row = 13;
            bool markedOne = false;
            if (userSelectedLoans.Any(o => double.Parse(o.Balance) > 0)) //only go to TS2O if a loan with a balnace was selected
            {
                RI.FastPath("TX3ZITS2O" + bpi.SSN);
                RI.PutText(7, 26, DateTime.Today.ToString("MMddyy"));
                RI.PutText(9, 54, "Y");
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    string sequenceNumberToTest = string.Format("0{0}", RI.GetText(row, 16, 4));
                    double balance = double.Parse(RI.GetText(row, 4, 10));
                    bool isCredit = RI.CheckForText(row, 14, "-");
                    if (balance > 0 && !isCredit && userSelectedLoans.Any(o => o.SequenceNum == sequenceNumberToTest))
                    {
                        RI.PutText(row, 2, "X"); //mark for balance calculation
                        markedOne = true;
                    }
                    row++;
                    if (RI.CheckForText(row, 2, "_") == false)
                    {
                        //page forward
                        row = 13;
                        if (markedOne)
                        {
                            RI.Hit(Key.Enter);
                        }
                        RI.Hit(Key.F8);
                        if (RI.MessageCode == "01292")
                        {
                            RI.Hit(Key.Enter);
                            RI.Hit(Key.F8);
                        }
                        markedOne = false;
                    }
                }
                RI.Hit(Key.Enter);
                //get pay off info
                bpi.BalanceInfo.PayOffAmount = RI.GetText(12, 29, 11);
                bpi.BalanceInfo.PrincipalDue = RI.GetText(14, 29, 11);
                bpi.BalanceInfo.InterestDue = RI.GetText(15, 29, 11);
                bpi.BalanceInfo.PastDueAmount = RI.GetText(18, 29, 11);
                bpi.BalanceInfo.LateFeesDue = RI.GetText(19, 29, 11);
            }
            else
            {
                bpi.BalanceInfo.PayOffAmount = "$0.00";
                bpi.BalanceInfo.PrincipalDue = "$0.00";
                bpi.BalanceInfo.InterestDue = "$0.00";
                bpi.BalanceInfo.PastDueAmount = "$0.00";
                bpi.BalanceInfo.LateFeesDue = "$0.00";
            }
        }

        private void GetFinancialDataForLoan(List<Transaction> transactions)
        {
            int row = 11;
            string translatedType;
            while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
            {
                //check reversal reason value
                if (RI.CheckForText(row, 8, " ") || RI.CheckForText(row, 8, "1"))
                {
                    //get translation type
                    try
                    {
                        translatedType = _paymentTypeTranslation[RI.GetText(row, 33, 4)];
                    }
                    catch (KeyNotFoundException)
                    {
                        //if can't be found
                        translatedType = RI.GetText(row, 33, 4);
                    }
                    //clear previous selection if any
                    RI.PutText(22, 18, "", true);
                    //select financial record
                    RI.PutText(22, 18, RI.GetText(row, 2, 2), Key.Enter);
                    //gather information for transaction
                    Transaction tempTransaction = new Transaction();
                    //if not summed with another transaction then create another payment entry into the array
                    tempTransaction.Type = translatedType;
                    tempTransaction.EffectiveDate = RI.GetText(16, 66, 8);
                    if (RI.GetText(12, 15, 10).Length > 0)
                    {
                        tempTransaction.BeginningPrincipalBalance = double.Parse(RI.GetText(12, 15, 10));
                    }
                    else
                    {
                        tempTransaction.BeginningPrincipalBalance = 0;
                    }
                    //applied principal balance
                    if (RI.CheckForText(13, 25, "CR"))
                    {
                        tempTransaction.AppliedPrincipalAmount = double.Parse(RI.GetText(13, 15, 10)) * -1;
                    }
                    else
                    {
                        tempTransaction.AppliedPrincipalAmount = double.Parse(RI.GetText(13, 15, 10));
                    }
                    //applied interest amount
                    if (RI.CheckForText(13, 37, "CR"))
                    {
                        tempTransaction.AppliedInterestAmount = double.Parse(RI.GetText(13, 28, 9)) * -1;
                    }
                    else
                    {
                        tempTransaction.AppliedInterestAmount = double.Parse(RI.GetText(13, 28, 9));
                    }
                    tempTransaction.AppliedLateFeeAmount = double.Parse(RI.GetText(13, 40, 10));
                    if (RI.GetText(14, 15, 10).Length > 0)
                    {
                        tempTransaction.RemainingPrincipalBalance = double.Parse(RI.GetText(14, 15, 10));
                    }
                    else
                    {
                        tempTransaction.RemainingPrincipalBalance = 0;
                    }
                    if (RI.CheckForText(9, 27, "CR"))
                    {
                        tempTransaction.FinancialActAmount = double.Parse(RI.GetText(9, 17, 10)) * -1;
                    }
                    else
                    {
                        tempTransaction.FinancialActAmount = double.Parse(RI.GetText(9, 17, 10));
                    }
                    tempTransaction.ReversalReason = RI.GetText(8, 55, 25);
                    tempTransaction.RejectReason = RI.GetText(9, 55, 25);
                    tempTransaction.PostedDate = RI.GetText(18, 24, 10);


                    //check for an existing payment that this should be summed with the gathered transactions
                    List<Transaction> searchResults = (from t in transactions
                                                       where t.Type == tempTransaction.Type &&
                                                             t.EffectiveDate == tempTransaction.EffectiveDate &&
                                                             t.ReversalReason == tempTransaction.ReversalReason &&
                                                             t.RejectReason == tempTransaction.RejectReason &&
                                                             t.PostedDate == tempTransaction.PostedDate
                                                       select t).ToList();

                    if (searchResults.Count == 0)
                    {
                        //match to sum not found
                        transactions.Add(tempTransaction);
                    }
                    else
                    {
                        //sum loan level transations to borrower level transaction
                        searchResults[0].SumTransactions(tempTransaction);
                    }

                    RI.Hit(Key.F12);   //back up to transaction screen again
                }
                row++;
                //check if the script is at the bottom of the screen
                if (RI.CheckForText(row, 3, " "))
                {
                    row = 11;
                    RI.Hit(Key.F8);
                }
            }
        }

        private void GetLoanInformation(BorrowerPaymentInformation bpi)
        {
            int row = 8;
            RI.FastPath("TX3ZITS26" + bpi.SSN);
            if (RI.CheckForText(1, 72, "T1X07"))
            {
                throw new InvalidTS26BorrowerException();
            }
            //if selection screen
            if (RI.CheckForText(1, 72, "TSX28"))
            {
                //get borrower's name
                bpi.BorrowerName = RI.GetText(4, 35, 47);
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    bpi.Loans.Add(new Loan(RI.GetText(row, 14, 4), RI.GetText(row, 5, 8), RI.GetText(row, 19, 6), RI.GetText(row, 59, 12)));
                    row++;
                    if (RI.CheckForText(row, 3, " "))
                    {
                        RI.Hit(Key.F8);
                        row = 8;
                    }
                }
            }
            else
            {
                bpi.BorrowerName = RI.GetText(5, 34, 44);
                bpi.Loans.Add(new Loan(RI.GetText(7, 35, 4), RI.GetText(6, 18, 8), RI.GetText(6, 66, 6), RI.GetText(11, 12, 12)));
            }
        }

        private Dictionary<string, string> CreateAndPopulatePaymentTypeTranslationDictionary()
        {
            Dictionary<string, string> paymentTypeTranslation = new Dictionary<string, string>();
            paymentTypeTranslation.Add("0101", "Automatic Disbursement");
            paymentTypeTranslation.Add("0102", "Manual Disbursement");
            paymentTypeTranslation.Add("0290", "Conversion - Purchase");
            paymentTypeTranslation.Add("0291", "Conversion - Non Purchase");
            paymentTypeTranslation.Add("0390", "Purchase Balance");
            paymentTypeTranslation.Add("0395", "Loan Sale");
            paymentTypeTranslation.Add("0495", "Deconversion Loan Sale");
            paymentTypeTranslation.Add("0496", "Deconversion Non Sale");
            paymentTypeTranslation.Add("0685", "Reallocation Increase");
            paymentTypeTranslation.Add("0686", "Reallocation Decrease");
            paymentTypeTranslation.Add("1010", "Borrower Payment");
            paymentTypeTranslation.Add("1011", "Borrower Interest Only Payment");
            paymentTypeTranslation.Add("1012", "Borrower Prinicpal Only Payment");
            paymentTypeTranslation.Add("1020", "Borrower Payment By Third Party");
            paymentTypeTranslation.Add("1021", "Borrower Interest Only Payment By Third Party");
            paymentTypeTranslation.Add("1027", "Origination Fee Credit BB");
            paymentTypeTranslation.Add("1029", "PLUS Interest Credit BB");
            paymentTypeTranslation.Add("1030", "Claim Payment");
            paymentTypeTranslation.Add("1034", "Timely Origination Fee Credit BB");
            paymentTypeTranslation.Add("1040", "School Refund");
            paymentTypeTranslation.Add("1041", "Borrower Cancellation Payment");
            paymentTypeTranslation.Add("1045", "Cancellation Payment");
            paymentTypeTranslation.Add("1050", "Loan Forgiveness Payment");
            paymentTypeTranslation.Add("1070", "External Consolidation Payment");
            paymentTypeTranslation.Add("1080", "Internal Consolidation Payment");
            paymentTypeTranslation.Add("1401", "Origination Fee Adjustment");
            paymentTypeTranslation.Add("2010", "Borrower Refund");
            paymentTypeTranslation.Add("2030", "Claim Refund");
            paymentTypeTranslation.Add("2040", "School Refund Return");
            paymentTypeTranslation.Add("2070", "Consolidation Refund");
            paymentTypeTranslation.Add("2080", "Consolidation Refund");
            paymentTypeTranslation.Add("2601", "Late Fees");
            paymentTypeTranslation.Add("5001", "Write-Off Prinicpal/Interest");
            paymentTypeTranslation.Add("5002", "Write-Off Prinicpal/Interest");
            paymentTypeTranslation.Add("5501", "Charge Off");
            paymentTypeTranslation.Add("5502", "Charge Off");
            paymentTypeTranslation.Add("6001", "Write Up");
            paymentTypeTranslation.Add("6002", "Write Up");
            paymentTypeTranslation.Add("7001", "Capped Interest");
            paymentTypeTranslation.Add("8001", "Pre-Conversion Adjustment");
            return paymentTypeTranslation;
        }
    }
}
