using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Windows.Forms;
using System.IO;

namespace NSFREVENTR
{
    public class CompassProcessor
    {
        private List<COMPASSPayment> _payments = new List<COMPASSPayment>();
        private string _userID;

        protected TestModeResults _testModeResults;
        protected ReversalEntry _userEntry;
        public ReflectionInterface RI { get; set; }
        public NSFReversalEntry RE { get; set; }
        public BorrowerDemographics Demos { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CompassProcessor(ReversalEntry userEntry, ReflectionInterface ri, string userId, NSFReversalEntry re, BorrowerDemographics demos)
        {
            _userEntry = userEntry;
            RE = re;
            RI = ri;
            _userID = userId;
            _testModeResults = re.TMR;
            Demos = demos;
        }

        /// <summary>
        /// Main starting point for processing.
        /// </summary>
        public void ProcessEntry()
        {
            //process entry
            if (_userEntry.ProcessForDeconvertedLoans == false)
            {
                TS2CProcessing();
            }
            else
            {
                DeconvertedLoansProcessing();
            }
            CloseR301QueueTasks();
        }

        private void TS2CProcessing()
        {
            RI.FastPath(string.Format("TX3Z/ITS2C{0}", _userEntry.SSN));
            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.Check4Text(1, 72, "TSX7S"))
            {
                throw new ExitScriptException("That SSN doesn't appear to be a valid SSN on the COMPASS system.  Please try again.");
            }
            if (RI.Check4Text(1, 72, "TSX2M"))
            {
                if (_userEntry.LoanListProvidedMethod == NSFReversalEntry.LoanListLocation.All)
                {
                    //select first loan if selection sreen is encountered
                    RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter);
                }
                else
                {
                    //find one of the loans that was specified
                    int loanSearchingRow = 7;
                    while (true)
                    {
                        List<int> results = (
                            from ln in _userEntry.Loans
                            where ln == int.Parse(RI.GetText(loanSearchingRow, 14, 4))
                            select ln).ToList<int>();
                        if (results.Count > 0)
                        {
                            //loan found
                            RI.PutText(21, 18, RI.GetText(loanSearchingRow, 2, 3).PadLeft(2, "0".ToCharArray()[0]), ReflectionInterface.Key.Enter);
                            break;
                        }
                        loanSearchingRow++;
                        if (RI.Check4Text(loanSearchingRow, 3, " "))
                        {
                            loanSearchingRow = 7;
                            RI.Hit(ReflectionInterface.Key.F8);
                            if (RI.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
                            {
                                MessageBox.Show("The loan sequence(s) number you provided can't be found.  Please review the loan(s) manually and try again.", "Review Manually", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                RE.EndScript();
                            }
                        }
                    }
                }
            }
            int row = 11;
            //try and get population based off exact effective date match
            while (RI.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
            {
                if (RI.Check4Text(row, 33, "1010C", "1041C") && RI.Check4Text(row, 12, DateTime.Parse(_userEntry.EffectiveDate).ToString("MM/dd/yy")))
                {
                    string transactionType = RI.GetText(row, 35, 2);
                    RI.PutText(22, 18, RI.GetText(row, 2, 3).PadLeft(2, "0".ToCharArray()[0]), ReflectionInterface.Key.Enter);
                    _payments.Add(new COMPASSPayment(RI.GetText(21, 24, 1), RI.GetText(16, 66, 8), RI.GetText(20, 66, 14), RI.GetText(19, 66, 14), transactionType, RI.GetText(18, 24, 8)));
                    RI.Hit(ReflectionInterface.Key.F12);
                }
                row++;
                if (RI.GetText(row, 3, 1) == "")
                {
                    row = 11;
                    RI.Hit(ReflectionInterface.Key.F8);
                }
            }
            //if no transactioins were found above then search for transactions just before the effective date
            if (_payments.Count == 0)
            {
                RI.Hit(ReflectionInterface.Key.F12);
                if (RI.Check4Text(1, 72, "TSX7S"))
                    RI.Hit(ReflectionInterface.Key.Enter);
                else
                    RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter);
                Nullable<DateTime> systemEffectiveDate = null;
                //try and get population based off exact effective date match
                while (RI.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    if (RI.Check4Text(row, 33, "1010C", "1041C") && DateTime.Parse(RI.GetText(row, 12, 8)) < DateTime.Parse(_userEntry.EffectiveDate))
                    {
                        //only gather information if the script hasn't found a effective date to target or it has one and the transaction effective date is the same as the one it has targeted.
                        if (systemEffectiveDate == null || systemEffectiveDate == DateTime.Parse(RI.GetText(row, 12, 8)))
                        {
                            systemEffectiveDate = DateTime.Parse(RI.GetText(row, 12, 8));
                            string transactionType = RI.GetText(row, 35, 2);
                            RI.PutText(22, 18, RI.GetText(row, 2, 3).PadLeft(2, "0".ToCharArray()[0]), ReflectionInterface.Key.Enter);
                            _payments.Add(new COMPASSPayment(RI.GetText(21, 24, 1), RI.GetText(16, 66, 8), RI.GetText(20, 66, 14), RI.GetText(19, 66, 14), transactionType, RI.GetText(18, 24, 8)));
                            RI.Hit(ReflectionInterface.Key.F12);
                        }
                    }
                    row++;
                    if (RI.GetText(row, 3, 1) == "")
                    {
                        row = 11;
                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                }
            }
            //decide what to do based off the number of payments found
            if (_payments.Count == 0)
            {
                //end script 
                MessageBox.Show("The script wasn't able to find a transaction that fit the criteria you provided.  Please try again.", "No Matches Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RE.EndScript();
            }
            else if (_payments.Count == 1)
            {
                //check that the amount is the same as the one provided by the user
                if (double.Parse(_userEntry.PaymentAmount) == double.Parse(_payments[0].RemittanceAmount))
                {
                    WriteToPostingFile(_payments[0]);
                }
                else
                {
                    //end script 
                    MessageBox.Show("The script wasn't able to find a transaction that fit the criteria you provided.  Please try again.", "No Matches Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RE.EndScript();
                }
            }
            else if (_payments.Count > 1)
            {
                GiveThemAChoice();
            }
        }

        private void DeconvertedLoansProcessing()
        {
            WriteToNonPostingFile();
        }

        //writes payment data out to the file
        private void WriteToPostingFile(COMPASSPayment payment)
        {
            using (StreamWriter sw = new StreamWriter(_testModeResults.DocFolder + NSFReversalEntry.COMPASS_NSF_ENTRY_FILE, true))
            {
                string line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", Demos.SSN, payment.BatchCode,
                    payment.EffectiveDate, payment.Batch, payment.RemittanceAmount.Replace(",", ""), ACHCheck(payment),
                    _userID, string.Format("{0} - {1}", _userEntry.NSFRe.Description, _userEntry.NSFRe.Code), payment.TransactionType);
                sw.WriteLine(line);
            }
        }

        //writes payment data out to the file
        private void WriteToNonPostingFile()
        {
            using (StreamWriter sw = new StreamWriter(_testModeResults.DocFolder + NSFReversalEntry.COMPASS_NSF_NON_POSTING_ENTRY_FILE, true))
            {
                string line = string.Format("{0},{1},{2},{3},{4},{5}", Demos.SSN, string.Format("{0} {1}", Demos.FName, Demos.LName),
                    "", _userEntry.EffectiveDate, _userEntry.PaymentAmount, _userEntry.NSFRe.Description);
                sw.WriteLine(line);
            }
        }

        //Calculates ACH indicator value
        private string ACHCheck(COMPASSPayment payment)
        {
            if (payment.BatchCode == "2")
            {
                RI.FastPath(string.Format(@"TX3Z/ITS7O{0}", _userEntry.SSN));
                if (RI.Check4Text(1, 72, "TSX7K") && RI.Check4Text(10, 18, "A"))
                    return "Y";
                else
                    return "N";
            }
            else
                return "N";
        }

        private void GiveThemAChoice()
        {
            UserChoiceOfPayments choiceFrm = new UserChoiceOfPayments(double.Parse(_userEntry.PaymentAmount), _payments);
            if (choiceFrm.ShowDialog() == DialogResult.OK)
                foreach (COMPASSPayment payment in choiceFrm.SelectedPayments)
                    WriteToPostingFile(payment);
            else
                RE.EndScript();
        }

        //closes R301 Queue tasks
        private void CloseR301QueueTasks()
        {
            RI.FastPath(string.Format("TX3Z/ITX6T{0}", _userEntry.SSN));
            while (RI.Check4Text(23, 2, "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA") == false)
            {
                bool taskFound = false;
                while (RI.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    for (int row = 7; row < 16; row = row + 4)
                    {
                        if (RI.Check4Text(row, 8, "R3") && RI.Check4Text(row, 24, "01"))
                        {
                            RI.PutText(21, 18, RI.GetText(row, 2, 2).PadLeft(2, "0".ToCharArray()[0]), ReflectionInterface.Key.Enter);
                            RI.PutText(9, 12, "Y");
                            RI.EnterText("0");
                            RI.Hit(ReflectionInterface.Key.EndKey);
                            RI.Hit(ReflectionInterface.Key.Enter);
                            RI.Hit(ReflectionInterface.Key.F11);
                            RI.PutText(8, 19, "C", ReflectionInterface.Key.Enter);
                            taskFound = true; //flag to exit loops
                            break;
                        }
                    }
                    if (taskFound) break; //exit loop if a task was found
                    RI.Hit(ReflectionInterface.Key.F8); //if loop wasn't exited then hit F8 to move to the next page
                }
                if (taskFound == false) break; //exit loop and method if a task wasn't found
                //check for another queue task
                RI.FastPath(string.Format("TX3Z/ITX6T{0}", _userEntry.SSN));
            }

        }


    }
}
