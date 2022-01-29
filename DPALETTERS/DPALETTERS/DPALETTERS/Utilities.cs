using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace DPALETTERS
{
    public class Utilities
    {
        public ReflectionInterface RI { get; set; }
        public string ResultIndicator { get; set; }

        public Utilities(ReflectionInterface ri)
        {
            RI = ri;
        }

        public enum ProcessLoop
        {
            Start,
            Retry,
            Success,
            Failure
        }

        public BorrowerResponse GetBorrowerFromUser(bool optional, string overrideText = null)
        {
            //prompt the user for the SSN, print the letters and end the script if the dialog box is cancelled
            using (BorrowerInput input = new BorrowerInput(optional, overrideText))
            {
                DialogResult result = input.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (input.accountIdentifierTextBox.Text.Trim().IsNullOrEmpty())
                    {
                        return new BorrowerResponse() { Borrower = null, Response = ProcessLoop.Success };
                    }
                    else
                    {
                        string accountIdentifier = input.accountIdentifierTextBox.Text;
                        Borrower borrower = GetBorrowerFromIdentifier(accountIdentifier);
                        return new BorrowerResponse() { Borrower = borrower, Response = borrower == null ? ProcessLoop.Retry : ProcessLoop.Success };
                    }
                }
                else
                {
                    return new BorrowerResponse() { Borrower = null, Response = ProcessLoop.Failure};
                }
            }
        }

        public Borrower GetBorrowerFromIdentifier(string accountIdentifier)
        {
            if (accountIdentifier.Length == 10)
            {
                RI.FastPath("LP22I;");
                RI.PutText(6, 33, accountIdentifier, Key.Enter);
                if (!RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS"))
                {
                    return null;
                }
                return new Borrower() { AccountNumber = accountIdentifier, Ssn = RI.GetText(3, 23, 9) };
            }
            else if (accountIdentifier.Length == 9)
            {
                RI.FastPath($"LP22I{accountIdentifier}");
                if (!RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS"))
                {
                    return null;
                }
                return new Borrower() { AccountNumber = RI.GetText(3, 60, 12).Replace(" ", ""), Ssn = accountIdentifier };
            }
            else
            {
                return null;
            }
        }

        public DrawDateResponse GetDrawDate(Borrower borrower)
        {
            //prompt the user for the drawdate, prompt the user to reenter the draw date if it is not within 60 days
            using (DrawDateInput input = new DrawDateInput())
            {
                DialogResult result = input.ShowDialog();
                return new DrawDateResponse() { Result = result, DrawDate = input.DrawDate };
            }
        }

        public DrawAmountResponse ValidateDrawAmount(decimal expectedDrawAmount)
        {
            //prompt the user to vlaidate the draw amount, the draw amount has to be greater than or equal to the expected amount
            using (ValidateDrawAmount validate = new ValidateDrawAmount(expectedDrawAmount))
            {
                DialogResult result = validate.ShowDialog();
                return new DrawAmountResponse() { Result = result, DrawAmount = validate.DrawAmount };
            }
        }

        public void UpdateLC34(LC05Information info)
        {
            //access LC34
            RI.FastPath($"LC34C{info.Borrower.Ssn}01");
            //enter the due date if the current due date is not the 7th
            if(info.DueDate.HasValue)
            {
                RI.PutText(4, 42, info.DueDate.Value.ToString("MMddyyyy"));
            }
            //enter the DPA indicator
            RI.PutText(6, 40, info.DPInd);

            int row = 9;
            //select the loans
            while (!RI.CheckForText(22,3, "46004"))
            {
                int counter = 0;
                while(counter < info.LoanList.Count)
                {
                    if(RI.CheckForText(row,5, info.LoanList[counter]))
                    {
                        RI.PutText(row, 3, "X");
                        //exit the inner loop to process the next loan
                        break;
                    }
                    counter++;
                }
                row++;
                //if the row is blank, forward to the next page, if no more pages or an error are displayed, stop processing
                if(RI.CheckForText(row,3," "))
                {
                    RI.Hit(Key.F8);
                }
            }
            RI.Hit(Key.Enter);
            if(!RI.CheckForText(22,3,"49000") && !RI.CheckForText(22,3,"40238") && !RI.CheckForText(22,3,"40239") && !RI.CheckForText(22,3,"49233"))
            {
                Dialog.Warning.Ok("The loans were not updated. Wait for the script to end and update the direct payment indicator manually.", "Loans not Updated");
            }

        }

        /// <summary>
        /// Get the borrowers information off of LC05, the input info object is modified and returned
        /// </summary>
        public LC05Information GetLC05Information(LC05Information info)
        {
            //Access LC05
            RI.FastPath($"LC05I{info.Borrower.Ssn}");
            
            if(RI.GetText(22,3,5) == "47004")
            {
                Dialog.Warning.Ok("The borrower does not have any records on LC05.", "No LC05 Records");
                //Loop back to the form so they can process another borrower
                return null;
            }

            //access the first loan
            RI.PutText(21, 13, "01", Key.Enter);

            while(!RI.CheckForText(22,3,"46004"))
            {
                if(!RI.CheckForText(4,10,"04") && RI.CheckForText(19,73,"MMDDCCYY"))
                {
                    info.LoanStatus = "O";
                }
                //set the due date
                if(info.DrawDate.HasValue)
                {
                    //Get the adjusted due date if the one in the session is missing or does not match the current draw date
                    if(RI.CheckForText(10,75,"DD"))
                    {
                        info.DueDate = GetDueDate(info.DrawDate.Value, DateTime.Today);
                    }
                    //we use toint here because it should return 0 and not match the draw date day if a value is not available in the session
                    else if(RI.GetText(10,75,2).ToInt() != info.DrawDate.Value.Day)
                    {
                        info.DueDate = GetDueDate(info.DrawDate.Value, DateTime.Today);
                    }
                }
                //get the claim ID to update the loan on LC34 if the loan is open
                if(RI.CheckForText(4,10,"03") && RI.CheckForText(19,73,"MMDDCCYY"))
                {
                    //verify the loan is for the comaker if a comaker SSN was entered
                    if(info.Comaker != null)
                    {
                        RI.Hit(Key.F10);
                        if(RI.CheckForText(14,54, info.Comaker.Ssn))
                        {
                            RI.Hit(Key.F10);
                            RI.Hit(Key.F10);
                            info.LoanList.Add(RI.GetText(21,11,4));
                            info.ExpectedPayment += RI.GetText(11, 71, 10).ToDecimal();
                        }
                    }
                    else
                    {
                        info.LoanList.Add(RI.GetText(21, 11, 4));
                        info.ExpectedPayment += RI.GetText(11, 71, 10).ToDecimal();
                    }
                }

                //set the status date if the status date of the loan is more recent than the previous status date and get the loan status
                if(RI.CheckForText(19,73,"MMDDCCYY"))
                {
                    DateTime? loanStatusDate = RI.GetText(5, 13, 8).ToDateNullable();
                    if (loanStatusDate.HasValue && loanStatusDate.Value > info.LoanStatusDate)
                    {
                        info.LoanStatusDate = loanStatusDate.Value;
                        info.Aux = RI.GetText(4, 26, 2);
                    }
                }
                else
                {
                    DateTime? loanStatusDate = RI.GetText(19, 73, 88).ToDateNullable();
                    if (loanStatusDate.HasValue && loanStatusDate.Value > info.LoanStatusDate)
                    {
                        info.LoanStatusDate = loanStatusDate.Value;
                        info.Aux = RI.GetText(4, 26, 2);
                    }
                }
                //go to the next loan
                RI.Hit(Key.F8);
            }
            RI.Hit(Key.F12);

            //warn the user and go back for another SSN if an open letter was closed but the borrower has no open loans
            if(info.LoanList.Count == 0 && info.DPInd != "N")
            {
                
                Dialog.Warning.Ok("The borrower does not have any loans eligible for DPA.", "No Eligible Loans");
                //Loop back to the form so they can process another borrower
                return null;
            }
            //warn the user and go back for another SSN if teh loan status of the loan with the most recent status date does not correspond to the letter chosen
            if((ResultIndicator == "P" && info.Aux != "") || (ResultIndicator == "R" && info.Aux != "10") || (ResultIndicator == "C" && info.Aux != "11" && info.Aux != "12"))
            {
                Dialog.Warning.Ok("A PIF, rehabilitation, or consolidation cancellation letter cannot be generated for the borrower because"
                    + "the loan status of the loan with the most recent status date does not correspond to the letter chosen.", "Open Loans");
                //Loop back to the form so they can process another borrower
                return null;
            }

            return info;
        }

        private DateTime GetDueDate(DateTime drawDate, DateTime date)
        {
            if (drawDate.Day == 7)
            {
                if (date.Day > 21)
                {
                    //get the 7th day of the month, 2 months from now
                    return new DateTime(date.AddMonths(2).Year, date.AddMonths(2).Month, 7);
                }
                else
                {
                    // get the 7th day of the month, 1 month from now
                    return new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 7);
                }
            }
            else if (drawDate.Day == 15)
            {
                // get the 15th day of the month, 1 month from now
                return new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 15);
            }
            else
            {
                if (date.Day < 7)
                {
                    return drawDate;
                }
                else
                {
                    return new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 22);
                }
            }
        }
    }
}
