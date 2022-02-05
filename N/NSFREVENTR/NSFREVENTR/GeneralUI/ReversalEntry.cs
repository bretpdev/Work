using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace NSFREVENTR
{
    public class ReversalEntry
    {

        public string SSN { get; set; }
        public string PaymentAmount { get; set; }
        public string EffectiveDate { get; set; }
        public NSFReason NSFRe { get; set; }
        public NSFReversalEntry.System System { get; set; }
        public NSFReversalEntry.BatchType BatchType { get; set; }
        public NSFReversalEntry.LoanListLocation LoanListProvidedMethod { get; set; }
        public string LoanCriteria { get; set; }
        public List<int> Loans { get; set; }
        public bool ProcessForDeconvertedLoans { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReversalEntry()
        {
            Loans = new List<int>();
            LoanCriteria = "Example: 1,2,4-6";
            ProcessForDeconvertedLoans = false;
        }

        /// <summary>
        /// Checks if data is valid for normal processing.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (System == NSFReversalEntry.System.None)
            {
                MessageBox.Show("You must provide a system.  Please try again.", "Invalid Entry Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (SSN == null || SSN.Length < 9)
            {
                MessageBox.Show("You must provide a nine digit SSN or a ten digit account number.  Please try again.","Invalid Entry Provided",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
            if (PaymentAmount == null || PaymentAmount.IsNumeric() == false)
            {
                MessageBox.Show("You must provide a numeric payment amount.  Please try again.", "Invalid Entry Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (EffectiveDate == null || EffectiveDate.IsValidDate() == false)
            {
                MessageBox.Show("You must provide a valid date as an effective date.  Please try again.", "Invalid Entry Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (NSFRe == null)
            {
                MessageBox.Show("You must provide a reason for the NSF.  Please try again.", "Invalid Entry Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (System == NSFReversalEntry.System.OneLINK && BatchType == NSFReversalEntry.BatchType.None)
            {
                MessageBox.Show("You must provide a batch type for the OneLINK system.  Please try again.", "Invalid Entry Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (System == NSFReversalEntry.System.Compass)
            {
                if (LoanListProvidedMethod == NSFReversalEntry.LoanListLocation.SeeListBelow)
                {
                    if (IsValidLoanCriteriaProvided() == false)
                    {
                        MessageBox.Show("You must provide valid loan targeting criteria (example: \"1,2,4-6\").  Please try again.", "Invalid Entry Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Check if data is valid for print processing only
        /// </summary>
        /// <returns></returns>
        public bool IsValidForPrinting()
        {
            if (System == NSFReversalEntry.System.None)
            {
                MessageBox.Show("You must provide a system.  Please try again.", "Invalid Entry Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        //checks if loan crit. is valid from the user
        private bool IsValidLoanCriteriaProvided()
        {
            List<int> tempLoans = new List<int>();
            //remove spaces from string
            string temp = LoanCriteria.Replace(" ","");
            try
            {
                //start dissecting the string
                List<string> resultsFromFirstDissection = new List<string>(LoanCriteria.Split(",".ToCharArray()[0]));
                foreach (string critPart in resultsFromFirstDissection)
                {
                    if (critPart.IsNumeric())
                    {
                        //if the entry is numeric then it should be a single loan number
                        tempLoans.Add(int.Parse(critPart));
                    }
                    else
                    {
                        //if not numeric then it should have a "-" and nothing else so ti can be split on the "-" character
                        List<string> resultsFromSecondDissection = new List<string>(critPart.Split("-".ToCharArray()[0]));
                        //check that the desired results are found
                        if (resultsFromSecondDissection.Count != 2 || resultsFromSecondDissection[0].IsNumeric() == false || resultsFromSecondDissection[1].IsNumeric() == false)
                        {
                            return false;
                        }
                        else
                        {
                            int min = int.Parse(resultsFromSecondDissection[0]);
                            int max = int.Parse(resultsFromSecondDissection[1]);
                            while (min != (max + 1))
                            {
                                tempLoans.Add(min);
                                min++;
                            }
                        }
                    }
                }
                Loans = tempLoans;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
