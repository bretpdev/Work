using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ESPQUEUES
{
    class SessionHelper
    {
        private ReflectionInterface RI { get; set; }
        private string ScriptId { get; set; }

        public SessionHelper(ReflectionInterface ri, string scriptId)
        {
            RI = ri;
            ScriptId = scriptId;
        }

        /// <summary>
        /// Closes the current RB and given sub queue
        /// </summary>
        public bool CloseTask(string subQueue)
        {
            if (!Program.SkipTaskClose)
            {
                RI.FastPath("TX3Z/ITX6XRB;" + subQueue);
                RI.PutText(21, 18, "01", ReflectionInterface.Key.F2);
                RI.PutText(8, 19, "C", ReflectionInterface.Key.Enter);

                if (RI.MessageCode == "01005") // "01005" is the code used for successfully closing the task
                    return true;
                else
                    return false;
            }
            else
                return true; // Return true even though we are skipping task closure based off of args (used for testing)
        }

        /// <summary>
        /// Updates TS01
        /// </summary>
        /// <returns>Return True if there were errors while updating TS01.  Return False if no errors occurred.</returns>
        public bool? UpdateTs01(string ssn, string message, IEnumerable<Loan> loans, Loan firstUpdateLoan)
        {
            RI.FastPath("TX3Z/CTS01" + ssn);
            RI.Hit(ReflectionInterface.Key.F10);
            RI.PutText(4, 19, firstUpdateLoan.NewSeparationDate.ToString("MMddyy"));
            RI.PutText(4, 49, firstUpdateLoan.NewSeparationReason);
            RI.PutText(4, 73, "GR"); //SEPARATION SOURCE
            RI.PutText(5, 19, firstUpdateLoan.LenderNotifiedDate.ToString("MMddyy"));
            RI.PutText(5, 43, firstUpdateLoan.OneLinkSchoolCode);
            RI.PutText(6, 19, firstUpdateLoan.CertificationDate.ToString("MMddyy"));
            RI.PutText(6, 43, firstUpdateLoan.EnrollmentStatusEffectiveDate.ToString("MMddyy"));

            return SelectLoansOnTs01(message, loans);
        }

        /// <summary>
        /// Select loans on TS01
        /// </summary>
        /// <returns>Returns true if an error was encountered, False if no error was encountered</returns>
        private bool? SelectLoansOnTs01(string message, IEnumerable<Loan> loans)
        {
            bool encounteredError01710 = false;
            bool unableToProcessDueToLoanStatus = false;
            int row = 9;
            while (!RI.CheckForText(23, 2, "90007") && !unableToProcessDueToLoanStatus)
            {
                string loanProgram = RI.GetText(row, 33, 6);
                DateTime firstDisbursementDate = DateTime.Parse(RI.GetText(row, 40, 8));
                if (loans.ContainsUncommentedLoan(loanProgram, firstDisbursementDate)) { RI.PutText(row, 3, "X"); }
                row++;
                if (RI.CheckForText(row, 3, " "))
                {
                    RI.Hit(ReflectionInterface.Key.Enter); //Save changes.
                    switch (RI.GetText(23, 2, 5))
                    {
                        case "03861":
                        case "01709":
                        case "01713":
                        case "01714": //OVERLAP - REVIEW ENROLLMENT HISTORY FOR CONTINUOUS ENROLLMENT
                        case "01711":
                            RI.Hit(ReflectionInterface.Key.F6);
                            break;
                        case "01067":
                        case "01460": //No action needed.
                            break;
                        case "03001": //UNABLE TO PROCESS SEPARATION DATE DUE TO LOAN STATUS
                            string errorMessage = "The information was not updated due to loan status.";
                            if (Program.ShowPrompts) { MessageBox.Show(errorMessage, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information); }
                            return null;
                        case "01710":
                            RI.Hit(ReflectionInterface.Key.F6);
                            return true;
                        default:
                            string notFoundMessage = string.Format("Info message '{0}' not found.", message);
                            if (Program.ShowPrompts) { MessageBox.Show(notFoundMessage, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information); }
                            return null;
                    }
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 9;
                }
            }
            return encounteredError01710;
        }

        /// <summary>
        /// Gets data from LG46
        /// </summary>
        public IEnumerable<Loan> GetLg46Data(string ssn, IEnumerable<Loan> loans)
        {
            RI.FastPath("LG46I" + ssn);
            if (RI.CheckForText(20, 2, "SEL")) //Selection screen.
            {
                int row = 8;
                while (!RI.CheckForText(22, 3, "46004"))
                {
                    if (RI.CheckForText(row, 5, " "))
                        row++;
                    else
                    {
                        string selection = RI.GetText(row, 2, 2);
                        string currentSchoolCode = RI.GetText(row, 9, 8);
                        string enrollmentStatus = RI.GetText(row, 20, 1);
                        DateTime enrollmentStatusEffectiveDate = DateTime.Parse(RI.GetText(row, 25, 8).ToDateFormat()); //MMddyyyy
                        DateTime expectedGraduationDate = DateTime.Parse(RI.GetText(row, 34, 8).ToDateFormat()); //MMddyyyy
                        DateTime certificationDate = DateTime.Parse(RI.GetText(row, 43, 8).ToDateFormat()); //MMddyyyy
                        while (RI.CheckForText(row, 2, "  ", selection.PadLeft(2, ' ')) && row < 19)
                        {
                            Loan loan = loans.SingleOrDefault(p => p.CommonlineId == RI.GetText(row, 62, 19));
                            if (loan != null)
                            {
                                loan.OneLinkSchoolCode = currentSchoolCode;
                                loan.EnrollmentStatus = enrollmentStatus;
                                loan.EnrollmentStatusEffectiveDate = enrollmentStatusEffectiveDate;
                                loan.ExpectedGraduationDate = expectedGraduationDate;
                                loan.CertificationDate = certificationDate;
                            }
                            row++;
                        }
                    }
                    if (row >= 19)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 8;
                    }
                }
            }
            else //Target screen.
            {
                loans.First().OneLinkSchoolCode = RI.GetText(5, 46, 8);
                loans.First().EnrollmentStatus = RI.GetText(5, 9, 1);
                loans.First().EnrollmentStatusEffectiveDate = DateTime.Parse(RI.GetText(6, 17, 8).ToDateFormat());
                loans.First().ExpectedGraduationDate = DateTime.Parse(RI.GetText(6, 31, 8).ToDateFormat());
                loans.First().CertificationDate = DateTime.Parse(RI.GetText(6, 51, 8).ToDateFormat());
            }

            return loans;
        }

        /// <summary>
        /// Gets loan information from compass.
        /// </summary>
        public Loan GetLoanInfoFromCompass()
        {
            Loan loan = new Loan();
            loan.FirstDisbursementDate = DateTime.Parse(RI.GetText(6, 18, 8)); //MM/dd/yy
            loan.Program = RI.GetText(6, 66, 6);
            loan.Sequence = int.Parse(RI.GetText(7, 35, 4));
            loan.RepaymentStartDate = RI.GetText(17, 44, 8).ToDateNullable(); //MM/dd/yy
            loan.HasCurrentDefermentOrForbearance = CurrentDefermentOrForbearanceExists();
            RI.Hit(ReflectionInterface.Key.Enter); //Go to page 2.
            loan.CommonlineId = RI.GetText(8, 17, 17) + RI.GetText(8, 58, 2).PadLeft(2, '0');
            loan.GuarantyDate = RI.GetText(7, 16, 8).ToDateNullable(); //MM/dd/yy
            RI.Hit(ReflectionInterface.Key.F12); //Return to page 1.
            return loan;
        }

        /// <summary>
        /// Checks to see if a borrower is in a deferment or forbearance.
        /// </summary>
        /// <returns>True if the borrower is in a deferment or forbearance.  False if they are not.</returns>
        private bool CurrentDefermentOrForbearanceExists()
        {
            bool exists = false;
            RI.Hit(ReflectionInterface.Key.F2);
            RI.Hit(ReflectionInterface.Key.F7); //Hotkey to TS31 (assumes we're on TS26).
            if (RI.CheckForText(23, 2, "01753"))
            {
                //No deferment/forbearance found.
                RI.Hit(ReflectionInterface.Key.F2);
            }
            else if (RI.CheckForText(1, 72, "TSX31"))
            {
                RI.Hit(ReflectionInterface.Key.F10);

                for (int row = 9; !RI.CheckForText(23, 2, "90007"); row++)
                {
                    if (row > 21 || RI.CheckForText(row, 3, " "))
                    {
                        row = 8;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }
                    string bDate = RI.GetText(row, 30, 8);
                    string eDate = RI.GetText(row, 40, 8);

                    if (bDate.Substring(6).ToInt() < 60)
                        bDate = bDate.Insert(6, "20");
                    else
                        bDate = bDate.Insert(6, "19");

                    if (eDate.Substring(6).ToInt() < 60)
                        eDate = eDate.Insert(6, "20");
                    else
                        eDate = eDate.Insert(6, "19");

                    DateTime beginDate = DateTime.Parse(bDate);
                    DateTime endDate = DateTime.Parse(eDate);
                    bool defermentOrForbearanceIsCurrent = (beginDate < DateTime.Now.Date && endDate > DateTime.Now.Date);
                    if (defermentOrForbearanceIsCurrent && RI.CheckForText(row, 25, "F26", "D19", "D22", "D15", "D18"))
                    {
                        exists = true;
                        break;
                    }
                }//for
                if (!exists) { RI.Hit(ReflectionInterface.Key.F8); }

                RI.Hit(ReflectionInterface.Key.F12); //Back out to TS26
                RI.Hit(ReflectionInterface.Key.F12);
                RI.Hit(ReflectionInterface.Key.F2); //switch back to hotkey set 1.
            }
            else
            {

                RI.Hit(ReflectionInterface.Key.F12);//Back out to TS26  
                RI.Hit(ReflectionInterface.Key.F2); //switch back to hotkey set 1.
            }
            return exists;
        }

        /// <summary>
        /// Completes all St tasks for a given borrower.
        /// </summary>
        public void CompleteStTasks(string ssn)
        {
            RI.FastPath("TX3Z/ITX6T" + ssn);
            for (bool endOfTasks = false; RI.CheckForText(1, 74, "TXX6V") && !endOfTasks; RI.FastPath("TX3Z/ITX6T" + ssn))
            {
                for (int row = 7; !RI.CheckForText(23, 2, "90007"); row += 5)
                {
                    if (RI.CheckForText(row, 8, "ST"))
                    {
                        RI.PutText(21, 18, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter); //Select the task to put it in working status.
                        RI.FastPath("TX3Z/ITX6T" + ssn); //Re-enter the queue and go into the active task's maintenance screen.
                        RI.PutText(21, 18, "01", ReflectionInterface.Key.F2);
                        RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter); //Complete the task.
                        break;
                    }
                    if (RI.CheckForText(row, 3, " "))
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 2;
                    }
                }
                if (RI.CheckForText(23, 2, "90007")) { endOfTasks = true; }
            }
        }

        /// <summary>
        /// Gets loan data from TS26
        /// </summary>
        public IEnumerable<Loan> GetCompassLoansFromTS26(string ssn)
        {
            List<Loan> loans = new List<Loan>();
            RI.FastPath("TX3Z/ITS26" + ssn);
            if (RI.CheckForText(1, 72, "TSX28"))
            {
                int row = 8; //Selection screen. Get info for loans that have a balance.
                while (!RI.CheckForText(23, 2, "90007"))
                {
                    if (double.Parse(RI.GetText(row, 59, 10)) > 0 && RI.CheckForText(row, 69, "  "))
                    {
                        RI.PutText(21, 12, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter, true);
                        if (RI.CheckForText(6, 66, "SUBCNS", "UNCNS", "SUBSPC", "UNSPC"))
                        {
                            Loan loan = new Loan();
                            loan.Sequence = int.Parse(RI.GetText(7, 35, 4));
                            loans.Add(loan);
                        }
                        else
                            loans.Add(GetLoanInfoFromCompass());

                        RI.Hit(ReflectionInterface.Key.F12); //Return to the selection screen.
                    }
                    row++;
                    if (RI.CheckForText(row, 3, " "))
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 8;
                    }
                }
            }
            else if (double.Parse(RI.GetText(11, 12, 10)) > 0 && RI.CheckForText(11, 22, "  "))
                loans.Add(GetLoanInfoFromCompass()); //Target screen (assumed), and loan has a balance.

            return loans.AsEnumerable();
        }
    }
}
