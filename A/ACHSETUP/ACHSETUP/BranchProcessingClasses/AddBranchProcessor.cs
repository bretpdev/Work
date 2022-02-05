using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACHSETUP
{
    class AddBranchProcessor : BaseBranchProcessor
    {
        //is returned from GatherNextDueDateFromTS26 method to indicate what to do next
        private enum NextPaymentDueDateAction
        {
            CreateApprovalLetter,
            DoTS12Thing,
            EndScript
        }

        private DataAccess DA { get; set; }
        private string FullName { get; set; }
        private List<int> SystemGatheredLoans { get; set; }
        private List<string> FirstPayDueDatesDayPortionOnly { get; set; }
        private bool DoCheckByPhoneProcessing { get; set; }
        private int DueDateDayPortion { get; set; }
        private ACHRecord AchRecToAdd { get; set; }
        private string NextPaymentDueDate { get; set; }
        private List<Ts26LoanData> Ts26Data { get; set; }
        private bool HasWyLoans { get; set; }

        public AddBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, string fullName, RecoveryProcessor recoveryProc, List<Ts26LoanData> ts26Data)
            : base(ri, brwDemos, scriptID, recoveryProc)
        {
            DA = new DataAccess(ri.LogRun);
            SystemGatheredLoans = new List<int>();
            FirstPayDueDatesDayPortionOnly = new List<string>();
            FullName = fullName;
            AchRecToAdd = new ACHRecord();
            Ts26Data = ts26Data;
            HasWyLoans = Ts26Data.Any(p => p.LenderId == "826717" || p.LenderId == "830248");
        }

        public override void Process()
        {
            AchRecToAdd = new ACHRecord();
            AddInfoDialog addACHUI = new AddInfoDialog(AchRecToAdd);
            if (addACHUI.ShowDialog() == DialogResult.Cancel)
                return; //End Script
            else
            {
                if (!UNSFAAndUNSFXArcCheck(Ts26Data))
                    return; //End Script
                RepaymentScheduleProcessing();
                //check to see if the user provided a list of loans
                if (AchRecToAdd.Loans.Count != 0)
                {
                    List<int> loans = CalculateLoanListFromUserAndSystemLists();
                    if (loans == null)
                        return; //End Script
                    AchRecToAdd.Loans = loans;
                }
                else
                {
                    //check if any data was collected from the repayment schedule
                    if (SystemGatheredLoans.Count == 0)
                    {
                        TD22CommentData td22Data = new TD22CommentData("Borrower not eligible for autopay due to not having any loans with an active repayment schedule.", AchRecToAdd.Loans, false);
                        string userPromptText = "Borrower does not have any loans with an active repayment schedule and therefore no loans are eligible for ACH.  Do you want to generate a denial letter?";
                        string denialReason = "Your account is not in repayment";
                        DenialLetter(BrwDemos, td22Data, denialReason, userPromptText, DA);
                        return; //End Script
                    }
                    //loans collected from system are the loans to use since no loans where provided by the user.
                    AchRecToAdd.Loans = SystemGatheredLoans;
                }
                //check to be sure that all loans have same due day
                if (!CheckForAlignedDueDays())
                    return; //End Script
                //all due dates should be the same at this point so the first one can be used as the Due Day
                DueDateDayPortion = FirstPayDueDatesDayPortionOnly.First().ToInt();
                //check to be sure all loans are owned by UHEAA also check for disqualification status
                if (!OwnerAndDelinquentAndDisqualificationStatusCheckOnTS26())
                    return; //End Script
                //assume that a check by phone is going to need to be made it will be calculated otherwise later if needed
                DoCheckByPhoneProcessing = true;
                //Check for active ACH records on system.  Handle it if one is found
                if (!Check4ActiveACHRecords())
                    return; //End Script
                //Create ACH record on system.
                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.AddOptionAddACH;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase))
                {
                    if (!AddACHToCompass())
                        return; //End Script
                    UpdateEbill();
                    RecoveryProcessor.UpdateLogWithNewPhase(phase);
                }
                //Gather next due date from TS26
                NextPaymentDueDateAction action = GatherNextDueDateFromTS26();
                if (action == NextPaymentDueDateAction.CreateApprovalLetter)
                    CreateApprovalLetters();//generate approval letters
                else if (action == NextPaymentDueDateAction.DoTS12Thing)
                    DoTheTS12Thing();//Do TS12 thing
                else
                    return; //End Script
                Dialog.Info.Ok("Processing Complete", "Processing Complete");
                return;
            }
        }

        /// <summary>
        /// Update borrower ebill indicator to a Y on TS7C
        /// </summary>
        private void UpdateTs7c()
        {
            if (RI.CheckForText(14, 48, "_")) { RI.PutText(14, 48, "N"); } //EXT TRM DEBT IND
            if (RI.CheckForText(18, 19, "__"))
            {
                string numberOfGraceMonths = (RI.CheckForText(6, 38, "STFFRD", "UNSTFD") ? "6" : RI.CheckForText(6, 38, "TILP") ? "02" : "0");
                RI.PutText(18, 19, numberOfGraceMonths);
            }

            RI.Hit(Key.Enter);
        }

        /// <summary>
        /// Updates the borrower ebill indicator to a Y
        /// </summary>
        private void UpdateEbill()
        {
            RI.FastPath("TX3Z/CTS7C*");
            RI.PutText(8, 42, BrwDemos.AccountNumber.Replace(" ", ""), Key.Enter, true);

            if (RI.ScreenCode == "TSX7D")
                UpdateTs7c();
            else if (RI.ScreenCode == "TSX3S")
            {
                for (int row = 7; RI.MessageCode != "90007"; row++)
                {
                    if (!RI.CheckForText(row, 20, " "))
                    {
                        if (AchRecToAdd.Loans.Contains(RI.GetText(row, 20, 4).ToInt()))
                        {
                            RI.PutText(22, 19, RI.GetText(row, 3, 3), Key.Enter, true);
                            UpdateTs7c();
                            RI.Hit(Key.F12);
                        }

                    }
                    else
                    {
                        RI.Hit(Key.F8);
                        row = 6;
                        continue;
                    }
                }
            }
            else
                return;
        }

        /// <summary>
        /// Get instances of a given arc.
        /// </summary>
        /// <param name="ssn">Borrowers Ssn</param>
        /// <param name="arc">Arc to check</param>
        /// <param name="loanAddDate">This should be the beginning date to search for an arc.</param>
        private List<ARCTracker> GetArcInstances(string arc, DateTime loanAddDate)
        {
            List<ARCTracker> unsfas = new List<ARCTracker>();
            RI.FastPath($"TX3ZITD2A{BrwDemos.Ssn}");
            RI.PutText(6, 31, "X"); //sort by request date
            RI.PutText(7, 60, "X"); //sort in descending order
            RI.PutText(11, 65, arc);
            RI.PutText(21, 16, $"{loanAddDate:MMddyy}{DateTime.Today:MMddyy}", Key.Enter);
            //no NSFs found
            if (RI.ScreenCode == "TDX2B")
                return unsfas;
            if (RI.ScreenCode == "TDX2D")
                return unsfas;  //one instance
            else
            {
                //mulitple instances
                for (int row = 7; RI.MessageCode != "90007"; row += 5)
                {
                    if (row > 17 || !RI.CheckForText(row, 2, "_"))
                    {
                        row = 2;
                        RI.Hit(Key.F8);
                        continue;
                    }

                    //get date from each instance
                    unsfas.Add(new ARCTracker(RI.GetText(row, 55, 8)));
                }

                return unsfas;
            }
        }

        /// <summary>
        /// Check Wy loans for Nsf's
        /// </summary>
        /// <param name="ssn">Borrower Ssn</param>
        /// <param name="loanAddDate">Borrowers loan add date</param>
        private bool CheckWyLoansForNsf(DateTime loanAddDate)
        {
            List<ARCTracker> possibleUnsfa = GetArcInstances("UNSFA", loanAddDate);
            List<ARCTracker> unsfx = GetArcInstances("UNSFX", loanAddDate);

            foreach (ARCTracker arc in possibleUnsfa)
            {
                DateTime requestedDate = arc.RequestedDate.ToDate();
                List<ARCTracker> t = possibleUnsfa.Where(p => p.RequestedDate.ToDate().AddMonths(1).Month - requestedDate.Month == 0).ToList();
                if (t.Count > 1)
                {
                    if (unsfx.Any(q => q.RequestedDate.ToDate() < requestedDate))
                        continue;
                    else
                        return true;
                }
            }

            return false;
        }

        //does UNSFA and UNSFX ARC checks
        private bool UNSFAAndUNSFXArcCheck(List<Ts26LoanData> data)
        {
            int wyAchCount = data.Where(p => p.LenderId == "826717").Count();
            if (wyAchCount != 0)
            {
                if (CheckWyLoansForNsf(data.Where(p => p.LenderId == "826717").Select(q => q.LoanAddDate).First()))
                {
                    string message = @"This borrowers lender id needs to be updated to ""830248"" as they are no longer eligible for the reduction benefit due to back to back NSF payments received in the past.";
                    Dialog.Info.Ok(message, "Change Lender");
                }

                //If all the loans are wy we are done if not we have to check the UHEAA loans
                if (wyAchCount == data.Count)
                    return true;
            }
            else if (data.Where(p => p.LenderId == "830248").Count() == data.Count)
                return true;//only has wy loans without the ACH BB so we can return.

            List<ARCTracker> unsfas = new List<ARCTracker>();
            //check for a UNSFA record with in the last year
            RI.FastPath($"TX3ZITD2A{BrwDemos.Ssn}");
            RI.PutText(6, 31, "X"); //sort by request date
            RI.PutText(7, 60, "X"); //sort in descending order
            RI.PutText(11, 65, "UNSFA");
            RI.PutText(21, 16, $"{DateTime.Today.AddYears(-1):MMddyy}{DateTime.Today:MMddyy}", Key.Enter);
            //no NSFs found
            if (RI.ScreenCode == "TDX2B")
                return true;
            if (RI.ScreenCode == "TDX2D")
                unsfas.Add(new ARCTracker(RI.GetText(13, 31, 8)));//one instance
            else
            {
                int row = 7;
                //multiple instances
                while (RI.CheckForText(row, 2, "_") && RI.MessageCode != "90007")
                {
                    //get date from each instance
                    unsfas.Add(new ARCTracker(RI.GetText(row, 55, 8)));
                    row += 5;
                    if (row > 17)
                    {
                        row = 7;
                        RI.Hit(Key.F8);
                    }
                }
            }

            //check for UNSFX records
            RI.FastPath($"TX3ZITD2A{BrwDemos.Ssn}");
            RI.PutText(6, 31, "X"); //sort by request date
            RI.PutText(7, 60, "X"); //sort in descending order
            RI.PutText(11, 65, "UNSFX");
            RI.PutText(21, 16, $"{DateTime.Today.AddYears(-1):MMddyy}{DateTime.Today:MMddyy}", Key.Enter);
            if (RI.ScreenCode != "TDX2B") //if no UNSFX records then skip nulling UNSFA records
            {
                //if mulitple instances then select all and step through them
                if (RI.ScreenCode == "TDX2C")
                    RI.PutText(5, 14, "X", Key.Enter);
                while (RI.MessageCode != "90007")
                {
                    //nullify earliest instance of UNSFA ARC
                    int searcher = 0;
                    while (unsfas[searcher].RequestedDate.ToDate() > RI.GetText(13, 31, 8).ToDate() || unsfas[searcher].Nulled)
                        searcher++;
                    unsfas[searcher].Nulled = true;
                    RI.Hit(Key.F8);
                }
            }
            //search for a UNSFA record that isn't nulled and is with in 365 days of today
            int anotherSearcher = 0;
            while (unsfas[anotherSearcher].Nulled)
            {
                anotherSearcher++;
                if (anotherSearcher >= unsfas.Count)
                    return true; //all UNSFA records were nulled
            }
            if (anotherSearcher >= unsfas.Count)
                return true; //all UNSFA records were nulled
            if (unsfas[anotherSearcher].RequestedDate.ToDate() > DateTime.Today.AddDays(-365))
            {
                anotherSearcher++; //move past originally found UNSFA record
                if (anotherSearcher >= unsfas.Count)
                    return true; //all UNSFA records were nulled except one and one isn't enough to stop ACH creation   
                DateTime aYearFromLastNSF = unsfas[anotherSearcher].RequestedDate.ToDate().AddDays(-365);
                //search for the next not nulled UNSFA record
                while (unsfas[anotherSearcher].Nulled)
                    anotherSearcher++;
                if (anotherSearcher == unsfas.Count)
                    return true; //all other UNSFA records were nulled   
                if (unsfas[anotherSearcher].RequestedDate.ToDate() > aYearFromLastNSF)
                {
                    StringBuilder wyMessage = new StringBuilder();
                    wyMessage.Append("This borrower has loans transfered from Wyoming and UHEAA loans.  The UHEAA loans are not eligible for ACH because ");
                    wyMessage.Append("this borrower has had too many NSF transactions in the past year.  The following loans are ");
                    wyMessage.Append("eligible for ACH but you will have to add them manually: \n\n");
                    foreach (Ts26LoanData item in Ts26Data.Where(p => p.LenderId == "826717" || p.LenderId == "830248"))
                        wyMessage.AppendFormat("Loan Seq: {0}\n", item.LoanSeq);

                    wyMessage.Append("\n Do you want to generate a denial letter?");

                    string msgBoxText = "This borrower had too many NSF transactions in the past and one year has not yet elapsed since the latest occurrence.  This borrower is not eligible for automatic payments.  Do you want to generate a denial letter?";
                    string denialReason = "You've had multiple insufficient funds";
                    TD22CommentData commentData = new TD22CommentData("Borrower not eligible for autopay due to NSF transactions.", false);
                    DenialLetter(BrwDemos, commentData, denialReason, HasWyLoans ? wyMessage.ToString() : msgBoxText, DA);
                    return false; //end script
                }
                else
                    return true;
            }
            else
                return true; //none of the not nulled UNSFA records fall with in a year of today
        }

        //does active repayment schedule checks and gets the initial loan sequence list
        private void RepaymentScheduleProcessing()
        {
            //first check for multiple and no repayment schedules
            RI.FastPath($"TX3ZITS2X{BrwDemos.Ssn}");
            if (RI.ScreenCode == "TSX2Y")
            {
                //If selection screen is found then check for multiple active repayments.  
                //If a selection screen isn't encountered then there can't be multiple active repayment schedules
                int row = 8;
                while (RI.MessageCode != "90007")
                {
                    if (RI.CheckForText(row, 7, "A"))
                    {
                        //collect loan information
                        RI.PutText(21, 14, RI.GetText(row, 3, 3), Key.Enter, true);
                        //get day portion of first pay due date
                        FirstPayDueDatesDayPortionOnly.Add(RI.GetText(7, 22, 2));
                        //get loan data if active
                        SystemGatheredLoans.AddRange(GetLoansFromActiveRepaymentSchedule().ToArray());
                        RI.Hit(Key.F12);
                    }
                    row++;
                    if (row == 21)
                    {
                        row = 8;
                        RI.Hit(Key.F8);
                    }
                }
            }
            else if (RI.CheckForText(1, 71, "TSX3W"))
            {
                //target screen
                //be sure that the record is active
                if (RI.CheckForText(7, 44, "A"))
                {
                    //get day portion of first pay due date
                    FirstPayDueDatesDayPortionOnly.Add(RI.GetText(7, 22, 2));
                    //get loan data if active
                    SystemGatheredLoans.AddRange(GetLoansFromActiveRepaymentSchedule());
                }
            }
        }

        //gets attached loans from Active repayment schedule
        private List<int> GetLoansFromActiveRepaymentSchedule()
        {
            RI.Hit(Key.F4);
            List<int> loans = new List<int>();
            while (RI.MessageCode != "90007")
            {
                if (!loans.Contains(RI.GetText(7, 44, 3).ToInt()))
                    loans.Add(RI.GetText(7, 44, 3).ToInt());
                RI.Hit(Key.F8);
            }
            RI.Hit(Key.F12);
            return loans;
        }

        //reconciles system loan list and user loan list, also creates a file loan list to be used
        private List<int> CalculateLoanListFromUserAndSystemLists()
        {
            //check if any loans where gathered from the system.  Send denial letter if no.
            if (SystemGatheredLoans.Count == 0)
            {
                //if no loans were gathered from the system then send denial letter
                TD22CommentData td22Data = new TD22CommentData("Borrower not eligible for autopay due to not having any loans with an active repayment schedule.", AchRecToAdd.Loans, false);
                string denialReason = "Your account is not in repayment";
                string promptForUser = "Borrower does not have any loans with an active repayment schedule and therefore no loans are eligible for ACH.  Do you want to generate a denial letter?";
                DenialLetter(BrwDemos, td22Data, denialReason, promptForUser, DA);
                return null; //End Script
            }

            //check if all users loans are in system loan list
            List<int> usersExtraLoans = new List<int>();
            foreach (int userLoan in AchRecToAdd.Loans)
            {
                if ((from ln in SystemGatheredLoans
                     where ln == userLoan
                     select ln).Count() == 0)
                {
                    usersExtraLoans.Add(userLoan);
                }
            }
            //check if all system loans are in the users loan list
            List<int> systemExtraLoans = new List<int>();
            foreach (int systemLoan in SystemGatheredLoans)
            {
                if ((from ln in AchRecToAdd.Loans
                     where ln == systemLoan
                     select ln).Count() == 0)
                {
                    systemExtraLoans.Add(systemLoan);
                }
            }

            //give appropriate error based of result
            if (usersExtraLoans.Count > 0 && systemExtraLoans.Count > 0)
            {
                Dialog.Error.Ok("The loan list gathered from TS2X in comparison to the loan list you provided results in major discrepancies.  Please review the information and start the script again.", "Major Loan List Discrepancies");
                return null; //End Script
            }
            else if (systemExtraLoans.Count > 0) //system list had extras
            {
                string userErrorMessage = "TS2X had the following eligible loan sequence number(s) which you didn't provide.";
                string userErrorMessageEnding = "Click Yes to add additional loans or click No to keep your original list.";
                userErrorMessage = $"{userErrorMessage}{Environment.NewLine}{Environment.NewLine}{systemExtraLoans.FormatForComments()}{Environment.NewLine}{Environment.NewLine}{userErrorMessageEnding}";
                //ask the user what they want to do with the extra loans
                if (MessageBox.Show(userErrorMessage, "Add Loans?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    List<int> finalList = new List<int>(AchRecToAdd.Loans.ToArray());
                    foreach (int loan in systemExtraLoans)
                        finalList.Add(loan);
                    return finalList;
                }
                else
                    return AchRecToAdd.Loans;
            }
            else if (usersExtraLoans.Count > 0) //user list had extras
            {
                string denialReason = "Your account is not in repayment";
                TD22CommentData td22Data = new TD22CommentData("Borrower not eligible for autopay due to not having any loans with an active repayment schedule.", AchRecToAdd.Loans, false);
                if (AchRecToAdd.Loans.Count == usersExtraLoans.Count)
                {
                    string userPrompt = "Borrower does not have any loans with an active repayment schedule and therefore no loans are eligible for ACH.  Do you want to generate a denial letter?";
                    DenialLetter(BrwDemos, td22Data, denialReason, userPrompt, DA);
                    return null; //End Script
                }
                else
                {
                    if (MessageBox.Show("At least one selected loan is not eligible for ACH.  If you choose to proceed, only those loans in an eligible status will be added to ACH.  Proceed Yes or No?", "Proceed With Eligible Loans", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        string userPrompt = "Do you want to generate a denial letter.";
                        DenialLetter(BrwDemos, td22Data, denialReason, userPrompt, DA);
                        return null; //End Script
                    }
                    else
                        return SystemGatheredLoans;
                }
            }
            else //lists matched perfectly
                return AchRecToAdd.Loans;
        }

        //makes sure that all loans have the same due day
        private bool CheckForAlignedDueDays()
        {
            string testValue = FirstPayDueDatesDayPortionOnly.First();
            if ((from dd in FirstPayDueDatesDayPortionOnly
                 where dd != testValue
                 select dd).Count() > 0)
            {
                MessageBox.Show("This borrower has multiple due dates.  Please align the repayment and then try again.", "Align Due Dates", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            return true;
        }

        //checks for loan owners, delinquency and certain disqualification statuses
        private bool OwnerAndDelinquentAndDisqualificationStatusCheckOnTS26()
        {
            List<int> ownedByUHEAA = new List<int>();
            List<int> notOwnedByUHEAA = new List<int>();
            string disqualifiedStatus = string.Empty;
            bool borrowerIsDelinquent = false;
            RI.FastPath($"TX3ZITS26{BrwDemos.Ssn}");
            if (RI.ScreenCode == "TSX28") //selection screen
            {
                int row = 8;
                while (RI.MessageCode != "90007")
                {
                    if ((from l in AchRecToAdd.Loans
                         where l == RI.GetText(row, 14, 4).ToInt()
                         select l).Count() > 0)
                    {
                        //take note of loan sequence
                        int loanSeq = RI.GetText(row, 14, 4).ToInt();
                        //target loan
                        RI.PutText(21, 12, RI.GetText(row, 2, 3), Key.Enter, true);
                        //check to see if the lender is affiliated with UHEAA
                        if (DA.IsUheaaAffiliatedLenderId(RI.GetText(7, 48, 8)) || RI.CheckForText(6, 66, "TILP"))
                            ownedByUHEAA.Add(loanSeq);
                        else
                            notOwnedByUHEAA.Add(loanSeq);
                        //check if loan is in a disqualified status on some kind
                        if (RI.CheckForText(3, 10, "VERIFIED DEATH") || RI.CheckForText(3, 10, "VERIFIED DISABILITY") || RI.CheckForText(3, 10, "VERIFIED BANKRUPTCY") ||
                            RI.CheckForText(3, 10, "ALLEGED DEATH") || RI.CheckForText(3, 10, "ALLEGED DISABILITY") || RI.CheckForText(3, 10, "ALLEGED BANKRUPTCY"))
                            disqualifiedStatus = RI.GetText(3, 10, 20);

                        //check if borrower is delinquent
                        RI.Hit(Key.Enter, 2);
                        if (!RI.CheckForText(9, 46, "    "))
                            borrowerIsDelinquent = true;
                        RI.Hit(Key.F12, 3);//exit out of the target screen\
                    }
                    row++;
                    //check if the script needs to forward to the next page
                    if (RI.CheckForText(row, 3, " "))
                    {
                        row = 8;
                        RI.Hit(Key.F8);
                    }
                }
            }
            else //target screen
            {
                if ((from l in AchRecToAdd.Loans
                     where l == RI.GetText(7, 35, 4).ToInt()
                     select l).Count() > 0)
                {
                    //check to see if the lender is affiliated with UHEAA
                    if (DA.IsUheaaAffiliatedLenderId(RI.GetText(7, 48, 8)) || RI.CheckForText(6, 66, "TILP"))
                        ownedByUHEAA.Add(RI.GetText(7, 35, 4).ToInt());
                    else
                        notOwnedByUHEAA.Add(RI.GetText(7, 35, 4).ToInt());
                    //check if loan is in a disqualified status on some kind
                    if (RI.CheckForText(3, 10, "VERIFIED DEATH") || RI.CheckForText(3, 10, "VERIFIED DISABILITY") || RI.CheckForText(3, 10, "VERIFIED BANKRUPTCY") ||
                        RI.CheckForText(3, 10, "ALLEGED DEATH") || RI.CheckForText(3, 10, "ALLEGED DISABILITY") || RI.CheckForText(3, 10, "ALLEGED BANKRUPTCY"))
                        disqualifiedStatus = RI.GetText(3, 10, 20);

                    //check if borrower is delinquent
                    RI.Hit(Key.Enter, 2);
                    if (!RI.CheckForText(9, 46, "    "))
                        borrowerIsDelinquent = true;
                    RI.Hit(Key.F12, 3);//exit out of the target screen\
                }
            }

            //hold on to results for future processing if needed
            AchRecToAdd.Loans = ownedByUHEAA;

            //calculate what to do based off results found in TS26

            //if a disqualification status was found while processing then permit denial letter and end script
            if (disqualifiedStatus.Length > 0)
            {
                string denialReason = "You have no loans eligible for Autopay";
                string userPromptText = $"This borrower is ineligible for ACH because the loans are in a {disqualifiedStatus} status.  Do you want to send a denial letter to the borrower?";
                TD22CommentData td22Data = new TD22CommentData($"Borrower not eligible for Autopay due to loan status = {disqualifiedStatus}.", AchRecToAdd.Loans, false);
                DenialLetter(BrwDemos, td22Data, denialReason, userPromptText, DA);
                return false;
            }

            //based off owner status check decide what to do
            if ((ownedByUHEAA.Count != 0 && notOwnedByUHEAA.Count != 0) || ownedByUHEAA.Count == 0) //some were owned be UHEAA and some weren't or none of the loans where owned by UHEAA
            {
                string denialReason = "You have no loans eligible for Autopay";
                string userPromptText = "Do you want to generate a denial letter?";
                TD22CommentData td22Data = new TD22CommentData("Borrower not eligible for Autopay due to UHEAA not owning the loans.", AchRecToAdd.Loans, false);
                if (ownedByUHEAA.Count != 0 && notOwnedByUHEAA.Count != 0) //some were owned be UHEAA and some weren't
                {
                    if (MessageBox.Show("This borrower has one or more eligible loans not owned by UHEAA that cannot be added to ACH.  Do you want to proceed with the eligible loans?", "Ineligible Loans Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        DenialLetter(BrwDemos, td22Data, denialReason, userPromptText, DA);
                        return false;
                    }
                }
                else if (ownedByUHEAA.Count == 0) //none of the loans were owned by UHEAA
                {
                    Dialog.Error.Ok("Borrower doesn't have any loans owned by UHEAA.", "No Eligible Loans Are Owned By UHEAA");
                    DenialLetter(BrwDemos, td22Data, denialReason, userPromptText, DA);
                    return false;
                }
            }

            //based off delinquency check give appropriate error message
            if (borrowerIsDelinquent)
            {
                MessageBox.Show("This borrower is delinquent.  This must be resolved prior to adding the record.", "Borrower Is Delinquent", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            return true;
        }

        //checks for active ACH records
        private bool Check4ActiveACHRecords()
        {
            RI.FastPath($"TX3ZITS7O{BrwDemos.Ssn}");
            //no results were found
            if (RI.CheckForText(1, 71, "TSX7I"))
                return true; //everything is alright, go ahead and process

            //something was found
            if (RI.ScreenCode == "TSX7J") //selection screen
            {
                int row = 13;
                while (RI.MessageCode != "90007")
                {
                    //if active record
                    if (RI.CheckForText(row, 67, "A"))
                    {
                        MessageBox.Show("There is already an existing active ACH record.  Please review the record to determine how to proceed.  Hit <Insert> when ready to proceed.", "Active ACH", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        RI.PauseForInsert();
                        ActiveACHRecordFoundOption userOptionSelection = new ActiveACHRecordFoundOption();
                        ActiveAchOptionsDialog ActiveACHRecordFoundUI = new ActiveAchOptionsDialog(userOptionSelection);
                        if (ActiveACHRecordFoundUI.ShowDialog() == DialogResult.OK)
                        {
                            if (userOptionSelection.SelectedOption == ActiveACHRecordFoundOption.Option.Add)
                                return true; //add record anyway
                            else if (userOptionSelection.SelectedOption == ActiveACHRecordFoundOption.Option.Change)
                            {
                                //do change processing instead of add processing
                                ChangeBranchProcessor changeProcessor = new ChangeBranchProcessor(RI, BrwDemos, ScriptId, FullName, RecoveryProcessor);
                                changeProcessor.Process();
                                return false; //End script
                            }
                            else
                                return false; //End script
                        }
                        else
                            return false; //End script
                    }
                    row++;
                    if (RI.CheckForText(row, 4, " "))
                    {
                        row = 13;
                        RI.Hit(Key.F8);
                    }
                }
            }
            else if (RI.ScreenCode == "TSX7K") //target screen
            {
                if (RI.CheckForText(10, 18, "A"))
                {
                    MessageBox.Show("There is already an existing active ACH record.  Please review the record to determine how to proceed.  Hit <Insert> when ready to proceed.", "Active ACH", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    RI.PauseForInsert();
                    ActiveACHRecordFoundOption userOptionSelection = new ActiveACHRecordFoundOption();
                    ActiveAchOptionsDialog ActiveACHRecordFoundUI = new ActiveAchOptionsDialog(userOptionSelection);
                    if (ActiveACHRecordFoundUI.ShowDialog() == DialogResult.OK)
                    {
                        if (userOptionSelection.SelectedOption == ActiveACHRecordFoundOption.Option.Add)
                            return true; //add record anyway
                        else if (userOptionSelection.SelectedOption == ActiveACHRecordFoundOption.Option.Change)
                        {
                            //do change processing instead of add processing
                            ChangeBranchProcessor changeProcessor = new ChangeBranchProcessor(RI, BrwDemos, ScriptId, FullName, RecoveryProcessor);
                            changeProcessor.Process();
                            return false; //End script
                        }
                        else
                            return false; //End script
                    }
                    else
                        return false; //End script
                }
            }
            return true;
        }

        //adds ACH record to Compass
        private bool AddACHToCompass()
        {
            RI.FastPath($"TX3ZATS7O{BrwDemos.Ssn};{AchRecToAdd.SSN};{AchRecToAdd.ABARoutingNumber};{AchRecToAdd.BankAccountNumber};{DueDateDayPortion}");
            
            if (RI.CheckForText(1, 71, "TSX7I"))
            {
                //handle half created or pending ACH
                RI.PutText(1, 4, "C", Key.Enter);
                if (RI.GetText(11, 18, 10).ToDate().Date == DateTime.Now.Date && RI.CheckForText(10, 18, "P"))
                    return true;
                else if (RI.GetText(11, 18, 10).ToDate().Date == DateTime.Now.Date && RI.CheckForText(10, 18, "P") == false)
                {
                    RI.PutText(10, 18, "P", Key.Enter);
                    return true;
                }
                else if (RI.GetText(11, 18, 10).ToDate().Date != DateTime.Now.Date && RI.CheckForText(10, 18, "P") == false)
                {
                    if (MessageBox.Show($"The ACH record requested was approved on {RI.GetText(11, 18, 10)}. Do you want to proceed?", "Do You Want To Proceed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DoCheckByPhoneProcessing = false;
                        return true;
                    }
                    else
                        return false; //End script
                }
            }
            else
            {
                //finish off creating record
                RI.PutText(9, 18, AchRecToAdd.GetAccountTypeAsString()); //account type
                RI.PutText(11, 57, AchRecToAdd.AdditionalWithdrawalAmount); //additional amount
                RI.PutText(12, 57, "Y"); // form was signed
                RI.PutText(13, 57, "PPD");

                int loansFoundToBeAdded = 0;
                int row = 17;

                //mark applicable loans
                while (RI.MessageCode != "90007")
                {
                    if (RI.GetText(row, 12, 2).IsNumeric() && (from l in AchRecToAdd.Loans
                                                               where l == RI.GetText(row, 12, 2).ToInt()
                                                               select l).Count() > 0)
                    {
                        //if loan is in the list then add it to the ACH
                        loansFoundToBeAdded++;
                        RI.PutText(row, 3, "A");
                    }
                    row++;
                    if (row > 22)
                    {
                        RI.Hit(Key.F8);
                        row = 17;
                    }
                }

                //check if all loans were added
                if (loansFoundToBeAdded < AchRecToAdd.Loans.Count)
                {
                    //some loans weren't found
                    Dialog.Error.Ok("One or more loans requested for ACH cannot be found.  Please review the account to determine the problem and try again.", "All Loans Not Found To Be Added");
                    return false; //End script
                }

                //all loans added so the record can be completed and put in a pending status
                RI.Hit(Key.Enter);
                if (RI.MessageCode == "90003")
                    RI.PutText(7, 47, "B", Key.Enter);
                RI.PutText(10, 18, "P", Key.Enter);
            }
            return true;
        }

        //Gathers the next payment due date and calculates the next step to take based off the value
        private NextPaymentDueDateAction GatherNextDueDateFromTS26()
        {
            NextPaymentDueDate = string.Empty;

            RI.FastPath($"TX3ZITS26{BrwDemos.Ssn}");

            if (RI.ScreenCode == "TSX28")
            {
                //selection screen
                int row = 8;
                while (RI.MessageCode != "90007")
                {
                    //search for applicable loan
                    if ((from l in AchRecToAdd.Loans
                         where l == RI.GetText(row, 14, 4).ToInt()
                         select l).Count() > 0)
                    {
                        //target loan
                        RI.PutText(21, 12, RI.GetText(row, 2, 3), true);
                        RI.Hit(Key.Enter, 3);
                        NextPaymentDueDate = RI.GetText(8, 45, 8);
                        if (NextPaymentDueDate.IsNullOrEmpty())
                        {
                            Dialog.Def.Ok("Could not determine the next pay due.  Please research this and Hit <Insert> when ready to proceed.");
                            RI.PauseForInsert();
                            using frmCannotDetermineNextPayDue frm = new frmCannotDetermineNextPayDue();
                            if (frm.ShowDialog() == DialogResult.Cancel)
                            {
                                return NextPaymentDueDateAction.EndScript;
                                //Commenting out this part, the BU said they might want it later
                                //RI.Atd22AllLoans(BrwDemos.Ssn, "AHSOT", "", "", ScriptId, false);
                                //double withdrawlAmount;
                                //if (AchRecToAdd.AdditionalWithdrawalAmount.IsPopulated())
                                //    withdrawlAmount = AchRecToAdd.AdditionalWithdrawalAmount.ToDouble();
                                //else
                                //    withdrawlAmount = 0;
                                //if (!GenerateLetterWithoutNextPay(BrwDemos, FullName, "N/A", $"{withdrawlAmount:###,##0.00}", BrwDemos.Ssn, BrwDemos.AccountNumber))
                                //    return NextPaymentDueDateAction.EndScript;
                            }
                            NextPaymentDueDate = frm.Result;
                        }
                        if (NextPaymentDueDate.Length > 0)
                        {
                            if (DoCheckByPhoneProcessing)
                                return NextPaymentDueDateAction.DoTS12Thing;
                            else
                                return NextPaymentDueDateAction.CreateApprovalLetter;
                        }
                        else
                            return NextPaymentDueDateAction.CreateApprovalLetter;
                    }
                    row++;
                    if (RI.CheckForText(row, 3, " "))
                    {
                        row = 8;
                        RI.Hit(Key.F8);
                    }
                }
                return NextPaymentDueDateAction.CreateApprovalLetter; //should never exit through here (this was added to appease the VS gods).
            }
            else
            {
                //target screen
                RI.Hit(Key.Enter, 2);
                NextPaymentDueDate = RI.GetText(8, 45, 8);
                if (NextPaymentDueDate.Length > 0)
                {
                    if (DoCheckByPhoneProcessing)
                        return NextPaymentDueDateAction.DoTS12Thing;
                    else
                        return NextPaymentDueDateAction.CreateApprovalLetter;
                }
                else
                    return NextPaymentDueDateAction.CreateApprovalLetter;
            }
        }

        //does the TS12 screen thing
        private void DoTheTS12Thing()
        {
            bool checkByPhoneProcessed = false;
            double currentAmountDue = 0;

            RI.FastPath($"TX3ZITS12{BrwDemos.Ssn}");
            if (RI.ScreenCode == "TSX14") //selection screen
            {
                int row = 8;
                while (RI.MessageCode != "90007")
                {
                    if (RI.CheckForText(row, 5, NextPaymentDueDate) && RI.CheckForText(row, 24, "A") && RI.CheckForText(row, 28, "5", "7", "N") == false)
                    {
                        if (NextPaymentDueDate.ToDate().Date > DateTime.Today.AddDays(7))
                        {
                            //select line
                            RI.PutText(21, 12, RI.GetText(row, 2, 3), Key.Enter, true);
                            //if total amount due > 0
                            if (RI.GetText(13, 26, 9).ToDouble() > 0)
                                currentAmountDue += RI.GetText(10, 42, 14).ToDouble();
                            RI.Hit(Key.F12);
                        }
                    }

                    row++;

                    if (RI.CheckForText(row, 3, " "))
                    {
                        row = 8;
                        RI.Hit(Key.F8);
                    }
                }

                //search for applicable 1010C transaction
                bool found1010C = false;
                if (NextPaymentDueDate.ToDate().Date > DateTime.Today.AddDays(7))
                {
                    //if payment is due with in 7 days
                    if (currentAmountDue >= 1)
                    {
                        RI.FastPath("TX3Z/ITS2C" + BrwDemos.Ssn);

                        if (RI.CheckForText(2, 27, "LOAN FINANCIAL ACTIVITY SUMMARY"))
                            RI.Hit(Key.Enter);

                        //find a loan that has been added to the ACH
                        if (RI.CheckForText(2, 25, "LOAN FINANCIAL ACTIVITY SELECTION"))
                        {
                            //selection screen
                            while (RI.MessageCode != "90007")
                            {
                                for (int ts2cSelectionScreenRow = 7; ts2cSelectionScreenRow < 20; ts2cSelectionScreenRow++)
                                {
                                    if (RI.GetText(ts2cSelectionScreenRow, 14, 4).Length > 0 && RI.GetText(ts2cSelectionScreenRow, 14, 4).ToInt() == AchRecToAdd.Loans[0])
                                    {
                                        //select option
                                        RI.PutText(21, 18, RI.GetText(ts2cSelectionScreenRow, 2, 2), Key.Enter, true);
                                        //target screen
                                        found1010C = FoundApplicable1010CTransactionOnTS2CTargetScreen(currentAmountDue);
                                        RI.Hit(Key.F12);
                                        if (found1010C)
                                            break;
                                    }
                                }
                                RI.Hit(Key.F8);
                                if (found1010C)
                                    break;
                            }
                        }
                        else if (RI.CheckForText(2, 30, "LOAN FINANCIAL ACTIVITY"))
                            found1010C = FoundApplicable1010CTransactionOnTS2CTargetScreen(currentAmountDue);//target screen

                        if (found1010C == false)
                        {
                            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.AddOptionProcessCheckByPhone;
                            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery log
                            {
                                OPSEntry checkByPhoneEntry = new OPSEntry
                                {
                                    SSN = BrwDemos.Ssn,
                                    FullName = FullName,
                                    DOB = BrwDemos.DateOfBirth,
                                    RoutingNumber = AchRecToAdd.ABARoutingNumber,
                                    BankAccountNumber = AchRecToAdd.BankAccountNumber,
                                    AcctType = AchRecToAdd.AccountType
                                };
                                if (AchRecToAdd.AdditionalWithdrawalAmount.IsNullOrEmpty() || AchRecToAdd.AdditionalWithdrawalAmount.ToDouble() == 0)
                                    checkByPhoneEntry.PaymentAmount = currentAmountDue.ToString("#####0.00");
                                else
                                    checkByPhoneEntry.PaymentAmount = $"{(AchRecToAdd.AdditionalWithdrawalAmount.ToDouble() + currentAmountDue):#####0.00}";
                                //next payment due date needs to be on a week day not a weekend
                                if (NextPaymentDueDate.ToDate().DayOfWeek == DayOfWeek.Saturday || NextPaymentDueDate.ToDate().DayOfWeek == DayOfWeek.Sunday)
                                    while (NextPaymentDueDate.ToDate().DayOfWeek != DayOfWeek.Monday)
                                        NextPaymentDueDate = $"{NextPaymentDueDate.ToDate().AddDays(1):MM/dd/yyyy}";
                                checkByPhoneEntry.EffectiveDate = NextPaymentDueDate;
                                checkByPhoneEntry.AccountHolderName = FullName;
                                DA.AddEntryToDB(checkByPhoneEntry);
                                AddComment($"{currentAmountDue:C} Auto pay payment. Requested payment effective date {NextPaymentDueDate}.", "PHNPN");
                                RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery
                            }
                            checkByPhoneProcessed = true;
                        }
                    }
                }
                else
                    NextPaymentDueDate = $"{NextPaymentDueDate.ToDate().AddMonths(1):MM/dd/yyyy}";

                //if check by phone was not processed and _nextPaymentDueDate < current date + 20 day add one month to _nextPaymentDueDate
                if (!checkByPhoneProcessed && NextPaymentDueDate.ToDate().Date < DateTime.Now.AddDays(20).Date)
                    NextPaymentDueDate = $"{NextPaymentDueDate.ToDate().AddMonths(1):MM/dd/yyyy}";
                CreateApprovalLetters();
            }
            else //target screen
            {
                if (RI.CheckForText(10, 12, NextPaymentDueDate) && RI.CheckForText(6, 54, "A"))
                {
                    //if next pay due date > current date + 7 then
                    if (NextPaymentDueDate.ToDate().Date > DateTime.Today.AddDays(7))
                    {
                        //if total amt due >= 1 then
                        if (RI.GetText(13, 26, 9).ToDouble() >= 1)
                        {
                            currentAmountDue = RI.GetText(10, 42, 14).ToDouble(); //get current amount due
                            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.AddOptionProcessCheckByPhone;
                            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery log
                            {
                                OPSEntry checkByPhoneEntry = new OPSEntry();
                                checkByPhoneEntry.SSN = BrwDemos.Ssn;
                                checkByPhoneEntry.FullName = FullName;
                                checkByPhoneEntry.DOB = BrwDemos.DateOfBirth;
                                checkByPhoneEntry.RoutingNumber = AchRecToAdd.ABARoutingNumber;
                                checkByPhoneEntry.BankAccountNumber = AchRecToAdd.BankAccountNumber;
                                checkByPhoneEntry.AcctType = AchRecToAdd.AccountType;
                                if (AchRecToAdd.AdditionalWithdrawalAmount.IsNullOrEmpty() || AchRecToAdd.AdditionalWithdrawalAmount.ToDouble() == 0)
                                    checkByPhoneEntry.PaymentAmount = $"{currentAmountDue:#####0.00}";
                                else
                                    checkByPhoneEntry.PaymentAmount = $"{(AchRecToAdd.AdditionalWithdrawalAmount.ToDouble() + currentAmountDue):#####0.00}";
                                checkByPhoneEntry.PaymentAmount = currentAmountDue.ToString("#####0.00");
                                //next payment due date needs to be on a week day not a weekend
                                if (NextPaymentDueDate.ToDate().DayOfWeek == DayOfWeek.Saturday || NextPaymentDueDate.ToDate().DayOfWeek == DayOfWeek.Sunday)
                                    while (NextPaymentDueDate.ToDate().DayOfWeek != DayOfWeek.Monday)
                                        NextPaymentDueDate = NextPaymentDueDate.ToDate().AddDays(1).ToString("MM/dd/yyyy");
                                checkByPhoneEntry.EffectiveDate = NextPaymentDueDate;
                                checkByPhoneEntry.AccountHolderName = FullName;
                                DA.AddEntryToDB(checkByPhoneEntry);
                                AddComment($"{currentAmountDue:C} Auto pay payment. Requested payment effective date {NextPaymentDueDate}.", "PHNPN");
                                RecoveryProcessor.UpdateLogWithNewPhase(phase);
                            }
                            checkByPhoneProcessed = true;
                        }
                    }
                    else
                        NextPaymentDueDate = NextPaymentDueDate.ToDate().AddMonths(1).ToString("MM/dd/yyyy"); //add one month to next pay due date

                    //if check by phone was not processed and _nextPaymentDueDate < current date + 20 day add one month to _nextPaymentDueDate
                    if (!checkByPhoneProcessed && NextPaymentDueDate.ToDate().Date < DateTime.Now.AddDays(20).Date)
                        NextPaymentDueDate = $"{NextPaymentDueDate.ToDate().AddMonths(1):MM/dd/yyyy}";
                    CreateApprovalLetters();
                }
            }
        }

        private void AddComment(string comment, string arc)
        {
            ArcData arcData = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = BrwDemos.Ssn,
                Arc = arc,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                ScriptId = ScriptId
            };
            ArcAddResults result = arcData.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding ARC: {arc} for borrower: {BrwDemos.AccountNumber}";
                Dialog.Def.Ok(message);
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
        }

        //searches TS2C Target Screen for applicable 1010C Transaction
        private bool FoundApplicable1010CTransactionOnTS2CTargetScreen(double currentAmountDue)
        {
            while (RI.MessageCode != "90007")
            {
                for (int i = 11; i < 20; i++)
                {
                    if (RI.GetText(i, 12, 8).Length > 0)
                    {
                        DateTime effectiveDate = RI.GetText(i, 12, 8).ToDate();
                        if (RI.CheckForText(i, 33, "1010C") && effectiveDate > NextPaymentDueDate.ToDate().AddMonths(-1).Date && effectiveDate < DateTime.Today)
                        {
                            //Select this transaction to check the Remittance Amount.
                            RI.PutText(22, 18, RI.GetText(i, 2, 2), Key.Enter, true);
                            if (RI.GetText(19, 66, 12).ToDouble() >= currentAmountDue)
                                return true;
                            RI.Hit(Key.F12);
                        }
                    }
                }
                RI.Hit(Key.F8);
            }
            return false;
        }

        //creates approval letters
        private void CreateApprovalLetters()
        {
            //create approval letters
            //approval letter for borrower
            string userPromptTextBrw = "Do you want to send an approval letter to this borrower?";
            TD22CommentData td22DataBrw = new TD22CommentData("Autopay Record Added.", false);
            double withdrawlAmount;
            if (AchRecToAdd.AdditionalWithdrawalAmount != "") 
                withdrawlAmount = AchRecToAdd.AdditionalWithdrawalAmount.ToDouble();
            else 
                withdrawlAmount = 0; 
            ApprovalLetter(BrwDemos, td22DataBrw, NextPaymentDueDate, $"{withdrawlAmount:###,##0.00}", userPromptTextBrw, false, DA);
            //approval letter for bank account holder
            string tempNextPaymentDueDate;
            if (NextPaymentDueDate == "N/A")
                tempNextPaymentDueDate = string.Empty;
            else
                tempNextPaymentDueDate = NextPaymentDueDate;
            string userPromptTextBankAcctHolder = "Do you need to send a separate Account Holder Notification?";
            TD22CommentData td22DataBankAcctHolder = new TD22CommentData("Approval letter sent to account holder - ", false);
            ApprovalLetter(BrwDemos, td22DataBankAcctHolder, tempNextPaymentDueDate, $"{withdrawlAmount:###,##0.00}", userPromptTextBankAcctHolder, true, DA);
        }
    }
}