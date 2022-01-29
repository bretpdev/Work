using System;
using System.Windows.Forms;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace PMTHISTFED
{
    public class BorrowerPaymentHistory : FedScript
    {

        private readonly Dictionary<string, string> _paymentTypeTranslation;

        public BorrowerPaymentHistory(ReflectionInterface ri)
            : base(ri, "PMTHISTFED")
        {

        }

        public override void Main()
        {
            //warn the user and end the script if the user is not in an active directory group authorized to run the script
            if (!SystemAccessHelper.IsAuthorizedInActiveDirectory("Run Compass Payment History - FED"))
            {
                MessageBox.Show("You are not a member of an approved active directory group to run this script.", "Not Authorized to Run Script", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            BorrowerPaymentInformation bpi = new BorrowerPaymentInformation();
            MainFrm mainForm = new MainFrm(bpi);
            //show the form once to get the account number
            if (mainForm.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                var result = GetLoanInformation(bpi);
                if (!result)
                {
                    MessageBox.Show("Invalid account number.  Please try again.", "Invalid Account Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            //show the form again to get a loan
            mainForm = new MainFrm(bpi);
            if (mainForm.ShowDialog() != DialogResult.Cancel)
            {
                var userSelectedLoans = mainForm.UserSelectedLoans;
                GetPaymentInformation(bpi, userSelectedLoans);
                GetBalanceInformation(bpi, userSelectedLoans);
                bpi.Transactions.Sort();
                CreateAndDisplayExcelReportOfPaymentHistory(bpi, mainForm.SelectedAllLoans);
                MessageBox.Show("Processing Complete!", "Processing Complete!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void CreateAndDisplayExcelReportOfPaymentHistory(BorrowerPaymentInformation bpi, bool selectedAllLoans)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            //sort comma delimited file in excel, create report, print report
            excelApp.Visible = true;
            //create blank spreadsheet
            Microsoft.Office.Interop.Excel.Workbook workBook = excelApp.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet workSheet = (excelApp.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet);
            workSheet.get_Range("A1", "J1").MergeCells = true;
            workSheet.get_Range("A1", "J1").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.get_Range("A1", "J1").Font.Bold = true;
            workSheet.get_Range("A1", "J1").EntireColumn.NumberFormat = "@";
            workSheet.Cells[1, 1] = "CORNERSTONE BORROWER TRANSACTION HISTORY";
            workSheet.Cells[2, 1] = "NAME:";
            workSheet.Cells[3, 1] = "ACCOUNT NUMBER:";
            workSheet.get_Range("A2", "A3").Font.Bold = true;
            workSheet.Cells[2, 3] = bpi.BorrowerName;
            workSheet.Cells[3, 3] = bpi.AccountNumber;
            //header rows
            workSheet.get_Range("F4", "H4").MergeCells = true;
            workSheet.Cells[4, 6] = "Transaction Applied";
            workSheet.get_Range("I4", "J4").MergeCells = true;
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
            workSheet.get_Range("A4", "J5").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.get_Range("A4", "J5").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignBottom;
            workSheet.get_Range("A4", "J5").Font.Bold = true;
            //int i = bpi.Transactions.Count();
            int i = 0;
            int row = 6;
            double transactionTotal = 0.0;
            double principalAppliedTotal = 0.0;
            double interestTotal = 0.0;
            double lateFeeTotal = 0.0;
            double revOrRefTotal = 0.0;
            double borrPaymentsTotal = 0.0;
            double borrInterestRebateTotal = 0.0;
            double capInterestTotal = 0.0;
            while (i != bpi.Transactions.Count())
            {

                workSheet.Cells[row, 1] = bpi.Transactions[i].EffectiveDate;
                workSheet.Cells[row, 2] = bpi.Transactions[i].PostedDate;
                workSheet.Cells[row, 3] = bpi.Transactions[i].Type;
                if (bpi.Transactions[i].Type == "BORROWER PAYMENT")
                {
                    borrPaymentsTotal = borrPaymentsTotal + bpi.Transactions[i].FinancialActAmount; //figure total
                }
                else if (bpi.Transactions[i].Type == "AUTOMATIC CAP")
                {
                    capInterestTotal = capInterestTotal + bpi.Transactions[i].AppliedPrincipalAmount; //figure total
                }
                workSheet.Cells[row, 4] = string.Format("{0:c}", bpi.Transactions[i].BeginningPrincipalBalance);
                if (bpi.Transactions[i].Type == "AUTOMATIC CAP")
                {
                    workSheet.Cells[row, 5] = string.Format("{0:c}", bpi.Transactions[i].AppliedPrincipalAmount);
                    transactionTotal = transactionTotal + bpi.Transactions[i].AppliedPrincipalAmount; //figure total
                }
                else
                {
                    workSheet.Cells[row, 5] = string.Format("{0:c}", bpi.Transactions[i].FinancialActAmount);
                    transactionTotal = transactionTotal + bpi.Transactions[i].FinancialActAmount; //figure total
                }

                //tally totals unless the payment is a conversion payment
                if (bpi.Transactions[i].Type != "NON-PURCH CONVERSION")
                {
                    principalAppliedTotal = principalAppliedTotal + bpi.Transactions[i].AppliedPrincipalAmount; //figure total
                    interestTotal = interestTotal + bpi.Transactions[i].AppliedInterestAmount; //figure total
                    lateFeeTotal = lateFeeTotal + bpi.Transactions[i].AppliedLateFeeAmount; //figure total
                }
                else
                {
                    borrInterestRebateTotal = borrInterestRebateTotal + bpi.Transactions[i].InterestRebateAmount; //figure total
                }

                //write amounts to the spreadsheet
                workSheet.Cells[row, 6] = string.Format("{0:c}", bpi.Transactions[i].AppliedPrincipalAmount);
                workSheet.Cells[row, 7] = string.Format("{0:c}", bpi.Transactions[i].AppliedInterestAmount);
                workSheet.Cells[row, 8] = string.Format("{0:c}", bpi.Transactions[i].AppliedLateFeeAmount);

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
                i++;
            }

            for (int r = 5; r < row; r++)
                foreach (char character in "ABCDEFGHIJ")
                    workSheet.get_Range(character + r.ToString(), character + r.ToString()).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic, XlColorIndex.xlColorIndexAutomatic);

            //skip a row
            row++;
            //add totals to report
            //Transactions
            row++;
            workSheet.Cells[row, 1] = "Transaction Total:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Math.Abs(transactionTotal));
            //Principal applied
            row++;
            workSheet.Cells[row, 1] = "Amount Applied To Principal Total:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Math.Abs(principalAppliedTotal));
            //Interest applied
            row++;
            workSheet.Cells[row, 1] = "Amount Applied To Interest Total:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Math.Abs(interestTotal));
            //Late fees
            row++;
            workSheet.Cells[row, 1] = "Amount Applied To Late Fees Total:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Math.Abs(lateFeeTotal));
            //Reversal Amount
            row++;
            workSheet.Cells[row, 1] = "Reversal/Refund Total:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Math.Abs(revOrRefTotal));
            //Borrower Payments
            row++;
            workSheet.Cells[row, 1] = "Borrower Payments Total:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Math.Abs(borrPaymentsTotal));
            //Interest Rebate Total
            row++;
            workSheet.Cells[row, 1] = "Interest Rebate Total:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Math.Abs(borrInterestRebateTotal));
            //Capped interest
            row++;
            workSheet.Cells[row, 1] = "Capped Interest Total:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Math.Abs(capInterestTotal));
            //pay off
            row++;
            workSheet.Cells[row, 1] = "Payoff Amount:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Convert.ToDecimal(bpi.BalanceInfo.PayOffAmount));
            //principal due
            row++;
            workSheet.Cells[row, 1] = "Principal Due:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Convert.ToDecimal(bpi.BalanceInfo.PrincipalDue));
            //Interest Due
            row++;
            workSheet.Cells[row, 1] = "Interest Due:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Convert.ToDecimal(bpi.BalanceInfo.InterestDue));
            //Past Due Amount
            row++;
            workSheet.Cells[row, 1] = "Past Due Amount:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Convert.ToDecimal(bpi.BalanceInfo.PastDueAmount));
            //late fees due
            row++;
            workSheet.Cells[row, 1] = "Late Fees Due:";
            workSheet.Cells[row, 4] = string.Format("{0:c}", Convert.ToDecimal(bpi.BalanceInfo.LateFeesDue));

            //format column widths
            workSheet.get_Range("A1", "J1").EntireColumn.ColumnWidth = 11;
            workSheet.get_Range("C1", "C1").EntireColumn.AutoFit();

            //warning
            if (!selectedAllLoans)
            {
                workSheet.Cells[row + 2, 1] = "Warning: All of the borrower’s loans have not been included in this report. As such it may not indicate accurate account level totals.";
            }
        }

        private void GetPaymentInformation(BorrowerPaymentInformation bpi, List<Loan> userSelectedLoans)
        {
            int row = 7;
            FastPath("TX3ZITS2C" + bpi.AccountNumber);
            PutText(8, 2, "", true);
            PutText(10, 2, "X");
            Hit(Key.Enter);
            //check if the script found a selection or target screen
            if (RI.CheckForText(1, 72, "TSX2M"))
            {
                //selection screen
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    if ((from usl in userSelectedLoans
                         where usl.SequenceNum == GetText(row, 14, 4)
                         select usl).Count() > 0) //if loan was selected by borrower then gather payment information for it
                    {
                        //blank previous selection if any
                        PutText(21, 18, "", true);
                        //select row
                        PutText(21, 18, GetText(row, 2, 2), Key.Enter);
                        GetFinancialDataForLoan(bpi.Transactions);
                        Hit(Key.F12); //back up to loan selection screen
                    }
                    row++;
                    if (RI.CheckForText(row, 3, " "))
                    {
                        row = 7;
                        Hit(Key.F8);
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
            if (userSelectedLoans.Any(o => o.Balance.ToDouble() > 0)) //only go to TS2O if a loan with a balance was selected
            {
                FastPath("TX3ZITS2O" + bpi.AccountNumber);
                PutText(7, 26, DateTime.Today.ToString("MMddyy"));
                PutText(9, 54, "Y");
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    string sequenceNumberToTest = string.Format("0{0}", GetText(row, 16, 4));
                    if ((from usl in userSelectedLoans
                         where usl.SequenceNum == sequenceNumberToTest
                         select usl).Count() > 0 && double.Parse(GetText(row, 4, 10)) > 0)
                    {
                        PutText(row, 2, "X"); //mark for balance calculation
                        markedOne = true;
                    }
                    row++;
                    if (RI.CheckForText(row, 2, "_") == false)
                    {
                        //page forward
                        row = 13;
                        if (markedOne)
                        {
                            Hit(Key.Enter);
                        }
                        Hit(Key.F8);
                        markedOne = false;
                    }
                }
                Hit(Key.Enter);
                //get pay off info
                bpi.BalanceInfo.PayOffAmount = GetText(12, 29, 11);
                bpi.BalanceInfo.PrincipalDue = GetText(14, 29, 11);
                bpi.BalanceInfo.InterestDue = GetText(15, 29, 11);
                bpi.BalanceInfo.PastDueAmount = GetText(18, 29, 11);
                bpi.BalanceInfo.LateFeesDue = GetText(19, 29, 11);
            }
            else
            {
                bpi.BalanceInfo.PayOffAmount = "0.00";
                bpi.BalanceInfo.PrincipalDue = "0.00";
                bpi.BalanceInfo.InterestDue = "0.00";
                bpi.BalanceInfo.PastDueAmount = "0.00";
                bpi.BalanceInfo.LateFeesDue = "0.00";
            }
        }

        private void GetFinancialDataForLoan(List<Transaction> transactions)
        {
            int row = 11;

            while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
            {
                //check reversal reason value
                if (RI.CheckForText(row, 8, " ") || RI.CheckForText(row, 8, "1"))
                {
                    //clear previous selection if any
                    PutText(22, 18, "", true);
                    //select financial record
                    PutText(22, 18, GetText(row, 2, 2), Key.Enter);

                    //gather information for transaction
                    Transaction tempTransaction = new Transaction();

                    //transaction type
                    tempTransaction.Type = GetText(17, 24, 10) + " " + GetText(16, 24, 10);
                    //effective date
                    tempTransaction.EffectiveDate = GetText(16, 66, 8);
                    //posted date
                    tempTransaction.PostedDate = GetText(18, 24, 10);
                    //Beginning Principal Balance
                    if (GetText(12, 15, 10).Length > 0)
                    {
                        tempTransaction.BeginningPrincipalBalance = double.Parse(GetText(12, 15, 10));
                    }
                    else
                    {
                        tempTransaction.BeginningPrincipalBalance = 0;
                    }
                    //Transaction Amount
                    if (RI.CheckForText(9, 27, "CR"))
                    {
                        tempTransaction.FinancialActAmount = double.Parse(GetText(9, 17, 10)) * -1;
                    }
                    else
                    {
                        tempTransaction.FinancialActAmount = double.Parse(GetText(9, 17, 10));
                    }

                    //applied principal balance
                    if (RI.CheckForText(13, 25, "CR"))
                    {
                        tempTransaction.AppliedPrincipalAmount = double.Parse(GetText(13, 15, 10)) * -1;
                    }
                    else
                    {
                        tempTransaction.AppliedPrincipalAmount = double.Parse(GetText(13, 15, 10));
                    }
                    //use applied principal amount for transaction amount if transaction type is CAP
                    if (RI.CheckForText(16, 24, "CAP"))
                    {
                        tempTransaction.FinancialActAmount = tempTransaction.AppliedPrincipalAmount;
                    }
                    //applied interest amount
                    if (RI.CheckForText(13, 38, "CR"))
                    {
                        tempTransaction.AppliedInterestAmount = double.Parse(GetText(13, 28, 10)) * -1;
                    }
                    else
                    {
                        tempTransaction.AppliedInterestAmount = double.Parse(GetText(13, 28, 10));
                    }
                    //late fee
                    tempTransaction.AppliedLateFeeAmount = double.Parse(GetText(13, 42, 10));

                    //reversal reason
                    tempTransaction.ReversalReason = GetText(8, 55, 25);

                    //interest rebate amount
                    if (tempTransaction.Type == "NON-PURCH CONVERSION")
                    {
                        Hit(Key.F2);
                        Hit(Key.F11);

                        if (RI.CheckForText(13, 25, "CR"))
                        {
                            tempTransaction.InterestRebateAmount = double.Parse(GetText(13, 15, 10)) * -1;
                        }
                        else
                        {
                            tempTransaction.InterestRebateAmount = double.Parse(GetText(13, 15, 10));
                        }

                        Hit(Key.F10);
                        Hit(Key.F2);
                    }
                    else
                    {
                        tempTransaction.InterestRebateAmount = 0;
                    }

                    //check for an existing payment that this should be summed with the gathered transactions
                    List<Transaction> searchResults = (from t in transactions
                                                       where t.Type == tempTransaction.Type &&
                                                             t.EffectiveDate == tempTransaction.EffectiveDate &&
                                                             t.ReversalReason == tempTransaction.ReversalReason &&
                                                             t.PostedDate == tempTransaction.PostedDate
                                                       select t).ToList();

                    if (searchResults.Count == 0)
                    {
                        //if not summed with another transaction then create another payment entry into the array
                        transactions.Add(tempTransaction);
                    }
                    else
                    {
                        //sum loan level transations to borrower level transaction
                        searchResults[0].SumTransactions(tempTransaction);
                    }

                    Hit(Key.F12);   //back up to transaction screen again
                }
                row++;
                //check if the script is at the bottom of the screen
                if (RI.CheckForText(row, 3, " "))
                {
                    row = 11;
                    Hit(Key.F8);
                    if (RI.CheckForText(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA"))
                    {
                        Hit(Key.Enter);
                    }
                }
            }
        }

        private bool GetLoanInformation(BorrowerPaymentInformation bpi)
        {
            int row = 8;
            FastPath("TX3ZITS26" + bpi.AccountNumber);
            if (RI.CheckForText(1, 72, "T1X07"))
            {
                return false;
            }
            //if selection screen
            if (RI.CheckForText(1, 72, "TSX28"))
            {
                //get borrower's name
                bpi.BorrowerName = GetText(4, 35, 47);
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    bpi.Loans.Add(new Loan(GetText(row, 14, 4), GetText(row, 5, 8), GetText(row, 19, 6), GetText(row, 59, 11)));
                    row++;
                    if (RI.CheckForText(row, 3, " "))
                    {
                        Hit(Key.F8);
                        row = 8;
                    }
                }
            }
            else
            {
                bpi.BorrowerName = GetText(5, 34, 44);
                bpi.Loans.Add(new Loan(GetText(7, 35, 4), GetText(6, 18, 8), GetText(6, 66, 6), GetText(11, 12, 11)));
            }
            return true;
        }
    }
}
