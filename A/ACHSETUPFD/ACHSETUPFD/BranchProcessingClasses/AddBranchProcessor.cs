using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHSETUPFD
{
    class AddBranchProcessor : BaseBranchProcessor
    {
        //is returned from GatherNextDueDateFromTS26 method to indicate what to do next
        private enum NextPaymentDueDateAction
        {
            CreateApprovalLetter,
            DoTS12Thing
        }

        private List<int> SystemGatheredLoans { get; set; }
        private List<string> FirstPayDueDatesDayPortionOnly { get; set; }
        private bool DoCheckByPhoneProcessing { get; set; }
        private string FullName { get; }
        private int DueDateDayPortion { get; set; }
        private ACHRecord AchRecToAdd { get; set; }
        private string NextPaymentDueDate { get; set; }


        //Used as ref later
        private List<DataClasses.EndorserRecord> Endorsers;

        public AddBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, string fullName, RecoveryProcessor recoveryProc, ProcessLogData processLogData, List<DataClasses.EndorserRecord> endorsers, DataAccess DA, ProcessLogRun logRun)
            : base(ri, brwDemos, scriptID, recoveryProc, processLogData, DA, logRun)
        {
            SystemGatheredLoans = new List<int>();
            FirstPayDueDatesDayPortionOnly = new List<string>();
            FullName = fullName;
            AchRecToAdd = new ACHRecord();
            Endorsers = endorsers;
        }

        public override void Process()
        {
            AchRecToAdd = new ACHRecord();
            AchRecToAdd.IsEndorser = ACHRecord.EndorserStatus.NA;
            AddInfoDialog addACHUI = new AddInfoDialog(AchRecToAdd, Endorsers, DA);
            if (addACHUI.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                //UNSFAAndUNSFXArcCheck(); We do not need to check for this in the federal world. The Fed's don't care how many NSF's a bor has.
                RepaymentScheduleProcessing();
                //check to see if the user provided a list of loans
                if (AchRecToAdd.Loans.Count != 0)
                {
                    AchRecToAdd.Loans = CalculateLoanListFromUserAndSystemLists();
                }
                else
                {
                    //check if any data was collected from the repayment schedule
                    if (SystemGatheredLoans.Count == 0)
                    {
                        TD22CommentData td22Data = new TD22CommentData("Borrower not eligible for autopay due to not having any loans with an active repayment schedule.", AchRecToAdd.Loans, false);
                        string userPromptText = "Borrower does not have any loans with an active repayment schedule and therefore no loans are eligible for ACH.  Do you want to generate a denial letter?";
                        string denialReason = "Your account is not in repayment";
                        DenialLetter(BrwDemos, td22Data, denialReason, userPromptText);
                        return;
                    }
                    //loans collected from system are the loans to use since no loans where provided by the user.
                    AchRecToAdd.Loans = SystemGatheredLoans;
                }
                //check to be sure that all loans have same due day
                CheckForAlignedDueDays();
                //all due dates should be the same at this point so the first one can be used as the Due Day
                DueDateDayPortion = int.Parse(FirstPayDueDatesDayPortionOnly.First());
                //check to be sure all loans are owned by UHEAA also check for disqualification status
                DelinquentAndDisqualificationStatusCheckOnTS26();
                //assume that a check by phone is going to need to be made it will be calculated otherwise later if needed
                DoCheckByPhoneProcessing = true;
                //Check for active ACH records on system.  Handle it if one is fouond
                Check4ActiveACHRecords();
                //Create ACH record on system.
                RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.AddOptionAddACH;
                if (!RecoveryProcessor.PhaseAlreadyInLog(phase))
                {
                    AddACHToCompass();
                    RecoveryProcessor.UpdateLogWithNewPhase(phase);
                }
                //Gather next due date from TS26
                if (GatherNextDueDateFromTS26() == NextPaymentDueDateAction.CreateApprovalLetter)
                {
                    //generate approval letters
                    CreateApprovalLetters();
                }
                else
                {
                    //Do TS12 thing
                    DoTheTS12Thing();
                }
                MessageBox.Show("Processing Complete", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        /// <summary>
        /// Does active repayment schedule checks and gets the initial loan sequence list
        /// </summary>
        private void RepaymentScheduleProcessing()
        {
            //first check for multiple and no repayment schedules
            RI.FastPath(string.Format("TX3ZITS2X{0}", BrwDemos.Ssn));
            if (RI.CheckForText(1, 72, "TSX2Y"))
            {
                //If selection screen is found then check for multiple active repayments.  
                //If a selection screen isn't encountered then there can't be multiple active repayment schedules
                int row = 8;
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    if (RI.CheckForText(row, 7, "A"))
                    {
                        //collect loan information
                        RI.PutText(21, 14, RI.GetText(row, 3, 3), ReflectionInterface.Key.Enter, true);
                        //get day portion of first pay due date
                        FirstPayDueDatesDayPortionOnly.Add(RI.GetText(7, 22, 2));
                        //get loan data if active
                        SystemGatheredLoans.AddRange(GetLoansFromActiveRepaymentSchedule().ToArray());
                        RI.Hit(ReflectionInterface.Key.F12);
                    }
                    row++;
                    if (row == 21)
                    {
                        row = 8;
                        RI.Hit(ReflectionInterface.Key.F8);
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

        /// <summary>
        /// Gets attached loans from Active repayment schedule
        /// </summary>
        private List<int> GetLoansFromActiveRepaymentSchedule()
        {
            RI.Hit(ReflectionInterface.Key.F4);
            List<int> loans = new List<int>();
            while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
            {
                if (!loans.Contains(RI.GetText(7, 44, 3).ToInt()))
                    loans.Add(int.Parse(RI.GetText(7, 44, 3)));
                RI.Hit(ReflectionInterface.Key.F8);
            }
            RI.Hit(ReflectionInterface.Key.F12);
            return loans;
        }

        /// <summary>
        /// Reconciles system loan list and user loan list, also creates a file loan list to be used
        /// </summary>
        private List<int> CalculateLoanListFromUserAndSystemLists()
        {
            //check if any loans where gathered from the system.  Send denial letter if no.
            if (SystemGatheredLoans.Count == 0)
            {
                //if no loans were gathered from the system then send denial letter
                TD22CommentData td22Data = new TD22CommentData("Borrower not eligible for autopay due to not having any loans with an active repayment schedule.", AchRecToAdd.Loans, false);
                string denialReason = "Your account is not in repayment";
                string promptForUser = "Borrower does not have any loans with an active repayment schedule and therefore no loans are eligible for ACH.  Do you want to generate a denial letter?";
                DenialLetter(BrwDemos, td22Data, denialReason, promptForUser);
                EndDllScript();
                return null; //won't ever return here but must be added to appease the VS Gods
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
                MessageBox.Show("The loans found in TS2X do not match the list provided. Please review the information and start the script again.", "Loans do not match", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndDllScript();
                return null;
            }
            else if (systemExtraLoans.Count > 0) //system list had extras
            {
                string userErrorMessage = "TS2X had the following eligible loan sequence number(s) which you did not provide.";
                string userErrorMessageEnding = "Click Yes to add additional loans or click No to keep your original list.";
                userErrorMessage = string.Format("{0}{1}{1}{2}{1}{1}{3}", userErrorMessage, Environment.NewLine, systemExtraLoans.FormatForComments(), userErrorMessageEnding);
                //ask the user what they want to do with the extra loans
                if (MessageBox.Show(userErrorMessage, "Add Loans?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    List<int> finalList = new List<int>(AchRecToAdd.Loans.ToArray());
                    foreach (int loan in systemExtraLoans)
                    {
                        finalList.Add(loan);
                    }
                    return finalList;
                }
                else
                {
                    return AchRecToAdd.Loans;
                }
            }
            else if (usersExtraLoans.Count > 0) //user list had extras
            {
                string denialReason = "Your account is not in repayment";
                TD22CommentData td22Data = new TD22CommentData("Borrower not eligible for autopay due to not having any loans with an active repayment schedule.", AchRecToAdd.Loans, false);
                if (AchRecToAdd.Loans.Count == usersExtraLoans.Count)
                {
                    string userPrompt = "Borrower does not have any loans with an active repayment schedule and therefore no loans are eligible for ACH.  Do you want to generate a denial letter?";
                    DenialLetter(BrwDemos, td22Data, denialReason, userPrompt);
                    EndDllScript();
                    return null;
                }
                else
                {
                    if (MessageBox.Show("At least one selected loan is not eligible for ACH.  If you choose to proceed, only the loans in an eligible status will be added to ACH.  Proceed Yes or No?", "Proceed With Eligible Loans", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        string userPrompt = "Do you want to generate a denial letter.";
                        DenialLetter(BrwDemos, td22Data, denialReason, userPrompt);
                        EndDllScript();
                        return null;
                    }
                    else
                    {
                        return SystemGatheredLoans;
                    }
                }
            }
            else //lists matched perfectly
            {
                return AchRecToAdd.Loans;
            }

        }

        /// <summary>
        /// Makes sure that all loans have the same due day
        /// </summary>
        private void CheckForAlignedDueDays()
        {
            string testValue = FirstPayDueDatesDayPortionOnly.First();
            if ((from dd in FirstPayDueDatesDayPortionOnly
                 where dd != testValue
                 select dd).Count() > 0)
            {
                MessageBox.Show("This borrower has multiple due dates.  Please align the repayment and try again.", "Align Due Dates", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                EndDllScript();
            }
        }

        /// <summary>
        /// Checks for delinquency and certain disqualification statuses
        /// </summary>
        private void DelinquentAndDisqualificationStatusCheckOnTS26()
        {
            string disqualifiedStatus = string.Empty;
            bool borrowerIsDelinquent = false;
            RI.FastPath("TX3ZITS26" + BrwDemos.Ssn);
            if (RI.CheckForText(1, 72, "TSX28")) //selection screen
            {
                int row = 8;
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    if ((from l in AchRecToAdd.Loans
                         where l == int.Parse(RI.GetText(row, 14, 4))
                         select l).Count() > 0)
                    {
                        //take note of loan sequence
                        int loanSeq = int.Parse(RI.GetText(row, 14, 4));
                        //target loan
                        RI.PutText(21, 12, RI.GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);
                        //check if loan is in a disqualified status on some kind
                        if (RI.CheckForText(3, 10, "VERIFIED DEATH") || RI.CheckForText(3, 10, "VERIFIED DISABILITY") || RI.CheckForText(3, 10, "VERIFIED BANKRUPTCY") ||
                            RI.CheckForText(3, 10, "ALLEGED DEATH") || RI.CheckForText(3, 10, "ALLEGED DISABILITY") || RI.CheckForText(3, 10, "ALLEGED BANKRUPTCY"))
                        {
                            disqualifiedStatus = RI.GetText(3, 10, 20);
                        }

                        //check if borrower is delinquent
                        RI.Hit(ReflectionInterface.Key.Enter);
                        RI.Hit(ReflectionInterface.Key.Enter);
                        if (!RI.CheckForText(9, 46, "    "))
                        {
                            borrowerIsDelinquent = true;
                        }
                        RI.Hit(ReflectionInterface.Key.F12);
                        RI.Hit(ReflectionInterface.Key.F12);

                        //exit out of the target screen\
                        RI.Hit(ReflectionInterface.Key.F12);
                    }
                    row++;
                    //check if the script needs to forward to the next page
                    if (RI.CheckForText(row, 3, " "))
                    {
                        row = 8;
                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                }
            }
            else //target screen
            {
                if ((from l in AchRecToAdd.Loans
                     where l == int.Parse(RI.GetText(7, 35, 4))
                     select l).Count() > 0)
                {
                    //check if loan is in a disqualified status on some kind
                    if (RI.CheckForText(3, 10, "VERIFIED DEATH") || RI.CheckForText(3, 10, "VERIFIED DISABILITY") || RI.CheckForText(3, 10, "VERIFIED BANKRUPTCY") ||
                        RI.CheckForText(3, 10, "ALLEGED DEATH") || RI.CheckForText(3, 10, "ALLEGED DISABILITY") || RI.CheckForText(3, 10, "ALLEGED BANKRUPTCY"))
                    {
                        disqualifiedStatus = RI.GetText(3, 10, 20);
                    }

                    //check if borrower is delinquent
                    RI.Hit(ReflectionInterface.Key.Enter);
                    RI.Hit(ReflectionInterface.Key.Enter);
                    if (!RI.CheckForText(9, 46, "    "))
                    {
                        borrowerIsDelinquent = true;
                    }
                    RI.Hit(ReflectionInterface.Key.F12);
                    RI.Hit(ReflectionInterface.Key.F12);

                    //exit out of the target screen\
                    RI.Hit(ReflectionInterface.Key.F12);
                }
            }

            //calculate what to do based off results found in TS26

            //if a disqualification status was found while processing then permit denila letter and end script
            if (disqualifiedStatus.Length > 0)
            {
                string denialReason = "You have no loans eligible for Autopay";
                string userPromptText = string.Format("This borrower is ineligible for ACH because the loans are in a {0} status.  Do you want to send a denial letter to the borrower?", disqualifiedStatus);
                TD22CommentData td22Data = new TD22CommentData(string.Format("Borrower not eligible for Autopay due to loan status = {0}.", disqualifiedStatus), AchRecToAdd.Loans, false);
                DenialLetter(BrwDemos, td22Data, denialReason, userPromptText);
                EndDllScript();
            }

            //based off delinquency check give appropriate error message
            if (borrowerIsDelinquent)
            {
                MessageBox.Show("This borrower is delinquent.  This must be resolved prior to adding the record.", "Borrower Is Delinquent", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                EndDllScript();
            }
        }

        /// <summary>
        /// Checks for active ACH records
        /// </summary>
        private void Check4ActiveACHRecords()
        {
            RI.FastPath("TX3ZITS7O" + BrwDemos.Ssn);
            //no results were found
            if (RI.CheckForText(1, 71, "TSX7I"))
            {
                return; //everything is alright, go ahead and process
            }

            //something was found
            if (RI.CheckForText(1, 72, "TSX7J")) //selection screen
            {
                int row = 13;
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
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
                            {
                                return; //add record anyway
                            }
                            else if (userOptionSelection.SelectedOption == ActiveACHRecordFoundOption.Option.Change)
                            {
                                //do change processing instead of add processing
                                ChangeBranchProcessor changeProcessor = new ChangeBranchProcessor(RI, BrwDemos, ScriptId, FullName, RecoveryProcessor, ProcessLogData, DA, LogRun);
                                changeProcessor.Process();
                                EndDllScript();
                            }
                            else
                            {
                                //end the script
                                EndDllScript();
                            }
                        }
                        else
                        {
                            //end script
                            EndDllScript();
                        }
                    }
                    row++;
                    if (RI.CheckForText(row, 4, " "))
                    {
                        row = 13;
                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                }
            }
            else if (RI.CheckForText(1, 72, "TSX7K")) //target screen
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
                        {
                            return; //add record anyway
                        }
                        else if (userOptionSelection.SelectedOption == ActiveACHRecordFoundOption.Option.Change)
                        {
                            //do change processing instead of add processing
                            ChangeBranchProcessor changeProcessor = new ChangeBranchProcessor(RI, BrwDemos, ScriptId, FullName, RecoveryProcessor, ProcessLogData, DA, LogRun);
                            changeProcessor.Process();
                            EndDllScript();
                        }
                        else
                        {
                            //end the script
                            EndDllScript();
                        }
                    }
                    else
                    {
                        //end script
                        EndDllScript();
                    }
                }
            }
        }

        /// <summary>
        /// Adds ACH record to Compass
        /// </summary>
        private void AddACHToCompass()
        {
            RI.FastPath(string.Format("TX3ZATS7O{0};{1};{2};{3};{4}", BrwDemos.Ssn, AchRecToAdd.SSN, AchRecToAdd.ABARoutingNumber, AchRecToAdd.BankAccountNumber, DueDateDayPortion));

            if (RI.CheckForText(1, 71, "TSX7I"))
            {
                //handle half created or pending ACH
                DateTime screenDate = DateTime.Parse(RI.GetText(11, 18, 10));
                RI.PutText(1, 4, "C", ReflectionInterface.Key.Enter);
                if (screenDate.Date == DateTime.Now.Date && RI.CheckForText(10, 18, "P"))
                {
                    return;
                }
                else if (screenDate.Date == DateTime.Now.Date && RI.CheckForText(10, 18, "P") == false)
                {
                    RI.PutText(10, 18, "P", ReflectionInterface.Key.Enter);
                    return;
                }
                else if (screenDate.Date != DateTime.Now.Date && RI.CheckForText(10, 18, "P") == false)
                {
                    if (MessageBox.Show(string.Format("The ACH record requested was approved on {0}.  Do you want to proceed?", RI.GetText(11, 18, 10)), "Do You Want To Proceed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DoCheckByPhoneProcessing = false;
                        return;
                    }
                    else
                    {
                        EndDllScript();
                    }
                }
            }
            else
            {
                //finish off creating record
                RI.PutText(9, 18, AchRecToAdd.GetAccountTypeAsString()); //account type
                if(AchRecToAdd.IsEndorser == ACHRecord.EndorserStatus.Yes)
                {
                    RI.PutText(7, 47, "E");
                    if(Endorsers.Count == 1)
                    {
                        RI.PutText(7, 51, Endorsers[0].LF_EDS);
                    }
                    else
                    {
                        SelectEndorser selectEndorser = new SelectEndorser(ref Endorsers);
                        if (selectEndorser.ShowDialog() == DialogResult.Cancel)
                        {
                            EndDllScript();
                        }
                        else
                        {
                            RI.PutText(7, 51, selectEndorser.recs[0].LF_EDS);
                            selectEndorser.Dispose();
                        }
                    }
                }
                else if(AchRecToAdd.IsEndorser == ACHRecord.EndorserStatus.No)
                {
                    RI.PutText(7, 47, "B");
                }
                RI.PutText(11, 57, AchRecToAdd.AdditionalWithdrawalAmount); //additional amount
                RI.PutText(12, 57, "Y"); // form was signed
                RI.PutText(13, 57, AchRecToAdd.GetEftAsString());

                int loansFoundToBeAdded = 0;
                int row = 17;

                //mark applicable loans
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    if (RI.GetText(row, 12, 2).IsNumeric() && (from l in AchRecToAdd.Loans
                                                               where l == int.Parse(RI.GetText(row, 12, 2))
                                                               select l).Count() > 0)
                    {
                        //if loan is in the list then add it to the ACH
                        loansFoundToBeAdded++;
                        RI.PutText(row, 3, "A");
                    }
                    row++;
                    if (row > 22)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 17;
                    }
                }

                //check if all loans were added
                if (loansFoundToBeAdded < AchRecToAdd.Loans.Count)
                {
                    //some loans weren't found
                    MessageBox.Show("One or more loans requested for ACH cannot be found.  Please review the account to determine the problem and try again.", "All Loans Not Found To Be Added", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EndDllScript();
                }

                //all loans added so the record can be completed and put in a pending status
                RI.Hit(ReflectionInterface.Key.Enter);
                if (RI.CheckForText(23, 2, "90003"))
                    RI.PutText(7, 47, "B", ReflectionInterface.Key.Enter);
                RI.PutText(10, 18, "P", ReflectionInterface.Key.Enter);
            }
        }

        /// <summary>
        /// Gathers the next payment due date and calculates the next step to take based off the value
        /// </summary>
        private NextPaymentDueDateAction GatherNextDueDateFromTS26()
        {
            NextPaymentDueDate = string.Empty;

            RI.FastPath("TX3ZITS26" + BrwDemos.Ssn);

            if (RI.CheckForText(1, 72, "TSX28"))
            {
                //selection screen
                int row = 8;
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    //search for applicable loan
                    if ((from l in AchRecToAdd.Loans
                         where l == int.Parse(RI.GetText(row, 14, 4))
                         select l).Count() > 0)
                    {
                        //target loan
                        RI.PutText(21, 12, RI.GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);
                        RI.Hit(ReflectionInterface.Key.Enter);
                        RI.Hit(ReflectionInterface.Key.Enter);
                        NextPaymentDueDate = RI.GetText(8, 45, 8); //DateTime.Now.AddDays(10).ToShortDateString();// GetText(8, 45, 8);
                        if (NextPaymentDueDate.Length > 0)
                        {
                            if (DoCheckByPhoneProcessing)
                            {
                                return NextPaymentDueDateAction.DoTS12Thing;
                            }
                            else
                            {
                                return NextPaymentDueDateAction.CreateApprovalLetter;
                            }
                        }
                        else
                        {
                            NextPaymentDueDate payment = new NextPaymentDueDate();
                            PaymentDueDate dueDate = new PaymentDueDate(payment);
                            MessageBox.Show("Could not determine the next payment due date. Please research the account and hit insert.", "Next Payment Date Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RI.PauseForInsert();
                            if (dueDate.ShowDialog() == DialogResult.Cancel)
                            {     
                                Atd22ByBalance(BrwDemos.Ssn, "AHSOT", "", "", ScriptId, false);
                                _option = LetterOption.SaveLetter;
                                NextPaymentDueDate = string.IsNullOrEmpty(payment.PaymentDueDate) ? DateTime.Now.ToShortDateString() : payment.PaymentDueDate;
                            }
                            else
                            {
                                NextPaymentDueDate = payment.PaymentDueDate;
                            }
                            return NextPaymentDueDateAction.CreateApprovalLetter;
                        }
                    }
                    row++;
                    if (RI.CheckForText(row, 3, " "))
                    {
                        row = 8;
                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                }
                return NextPaymentDueDateAction.CreateApprovalLetter; //should never exit through here 
            }
            else
            {
                //target screen
                RI.Hit(ReflectionInterface.Key.Enter);
                RI.Hit(ReflectionInterface.Key.Enter);
                NextPaymentDueDate = RI.GetText(8, 45, 8);
                if (NextPaymentDueDate.Length > 0)
                {
                    if (DoCheckByPhoneProcessing)
                    {
                        return NextPaymentDueDateAction.DoTS12Thing;
                    }
                    else
                    {
                        return NextPaymentDueDateAction.CreateApprovalLetter;
                    }
                }
                else
                {
                    NextPaymentDueDate payment = new NextPaymentDueDate();
                    PaymentDueDate dueDate = new PaymentDueDate(payment);
                    MessageBox.Show("Could not determine the next payment due date. Please research the account and hit insert.", "Next Payment Date Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RI.PauseForInsert();
                    if (dueDate.ShowDialog() == DialogResult.Cancel)
                    {
                        Atd22ByBalance(BrwDemos.Ssn, "AHSOT", "", "", ScriptId, false);
                        _option = LetterOption.SaveLetter;
                        NextPaymentDueDate = string.IsNullOrEmpty(payment.PaymentDueDate) ? DateTime.Now.ToShortDateString() : payment.PaymentDueDate;
                    }
                    else
                    {
                        NextPaymentDueDate = payment.PaymentDueDate;
                    }
                    return NextPaymentDueDateAction.CreateApprovalLetter;
                }
            }
        }

        /// <summary>
        /// Does the TS12 screen thing
        /// </summary>
        private void DoTheTS12Thing()
        {
            DateTime nextPaymentDueDate = DateTime.Parse(NextPaymentDueDate);
            bool checkByPhoneProcessed = false;
            double currentAmountDue = 0;

            RI.FastPath("TX3ZITS12" + BrwDemos.Ssn);
            if (RI.CheckForText(1, 72, "TSX14")) //selection screen
            {
                int row = 8;
                while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    if (RI.CheckForText(row, 5, NextPaymentDueDate) && RI.CheckForText(row, 24, "A") && RI.CheckForText(row, 28, "5", "7", "N") == false)
                    {
                        if (nextPaymentDueDate.Date > DateTime.Today.AddDays(7))
                        {
                            //select line
                            RI.PutText(21, 12, RI.GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);
                            //if total amount due > 0
                            if (double.Parse(RI.GetText(13, 26, 9)) > 0) { currentAmountDue += double.Parse(RI.GetText(10, 42, 14)); }
                            RI.Hit(ReflectionInterface.Key.F12);
                        }
                    }

                    row++;

                    if (RI.CheckForText(row, 3, " "))
                    {
                        row = 8;
                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                }//while

                //search for applicable 1010C transaction
                bool found1010C = false;
                if (nextPaymentDueDate.Date > DateTime.Today.AddDays(7))
                {
                    //if payment is due with in 7 days
                    if (currentAmountDue >= 1)
                    {
                        RI.FastPath("TX3Z/ITS2C" + BrwDemos.Ssn);

                        if (RI.CheckForText(2, 27, "LOAN FINANCIAL ACTIVITY SUMMARY")) { RI.Hit(ReflectionInterface.Key.Enter); }

                        //find a loan that has been added to the ACH
                        if (RI.CheckForText(2, 25, "LOAN FINANCIAL ACTIVITY SELECTION"))
                        {
                            //selection screen
                            while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                            {
                                for (int ts2cSelectionScreenRow = 7; ts2cSelectionScreenRow < 20; ts2cSelectionScreenRow++)
                                {
                                    if (RI.GetText(ts2cSelectionScreenRow, 14, 4).Length > 0 && int.Parse(RI.GetText(ts2cSelectionScreenRow, 14, 4)) == AchRecToAdd.Loans[0])
                                    {
                                        //select option
                                        RI.PutText(21, 18, RI.GetText(ts2cSelectionScreenRow, 2, 2), ReflectionInterface.Key.Enter, true);
                                        //target screen
                                        found1010C = FoundApplicable1010CTransactionOnTS2CTargetScreen(currentAmountDue);
                                        RI.Hit(ReflectionInterface.Key.F12);
                                        if (found1010C) { break; }
                                    }
                                }//for
                                RI.Hit(ReflectionInterface.Key.F8);
                                if (found1010C) { break; }
                            }//while
                        }
                        else if (RI.CheckForText(2, 30, "LOAN FINANCIAL ACTIVITY"))
                        {
                            //target screen
                            found1010C = FoundApplicable1010CTransactionOnTS2CTargetScreen(currentAmountDue);
                        }

                        if (found1010C == false)
                        {
                            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.AddOptionProcessCheckByPhone;
                            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery log
                            {
                                OPSEntry checkByPhoneEntry = new OPSEntry();
                                checkByPhoneEntry.AccountNumber = BrwDemos.AccountNumber.Replace(" ", "");
                                checkByPhoneEntry.SSN = BrwDemos.Ssn;
                                checkByPhoneEntry.FullName = FullName;
                                checkByPhoneEntry.DOB = BrwDemos.DateOfBirth;
                                checkByPhoneEntry.RoutingNumber = AchRecToAdd.ABARoutingNumber;
                                checkByPhoneEntry.BankAccountNumber = AchRecToAdd.BankAccountNumber;
                                checkByPhoneEntry.AcctType = AchRecToAdd.AccountType;
                                if (string.IsNullOrEmpty(AchRecToAdd.AdditionalWithdrawalAmount) || double.Parse(AchRecToAdd.AdditionalWithdrawalAmount) == 0)
                                {
                                    checkByPhoneEntry.PaymentAmount = currentAmountDue.ToString("#####0.00");
                                }
                                else
                                {
                                    checkByPhoneEntry.PaymentAmount = (double.Parse(AchRecToAdd.AdditionalWithdrawalAmount) + currentAmountDue).ToString("#####0.00");
                                }
                                //next payment due date needs to be on a week day not a weekend
                                if (nextPaymentDueDate.DayOfWeek.IsIn(DayOfWeek.Saturday, DayOfWeek.Sunday))
                                {
                                    while (DateTime.Parse(NextPaymentDueDate).DayOfWeek != DayOfWeek.Monday)
                                    {
                                        NextPaymentDueDate = DateTime.Parse(NextPaymentDueDate).AddDays(1).ToString("MM/dd/yyyy");
                                    }
                                }
                                checkByPhoneEntry.EffectiveDate = NextPaymentDueDate;
                                checkByPhoneEntry.AccountHolderName = FullName;
                                DA.AddEntryToDB(checkByPhoneEntry);
                                string comment = string.Format("{0:C} Auto pay payment. Requested payment effective date {1}.", checkByPhoneEntry.PaymentAmount, NextPaymentDueDate);
                                Atd22AllLoans(BrwDemos.Ssn, "PHNPN", comment, "", ScriptId, false);
                                RecoveryProcessor.UpdateLogWithNewPhase(phase); //update recovery
                            }
                            checkByPhoneProcessed = true;
                        }
                    }
                }
                else
                {
                    NextPaymentDueDate = nextPaymentDueDate.AddMonths(1).ToString("MM/dd/yyyy");
                    nextPaymentDueDate = DateTime.Parse(NextPaymentDueDate); 
                }

                //if check by phone was not processed and NextPaymentDueDate < current date + 20 day add one month to NextPaymentDueDate
                if (!checkByPhoneProcessed && DateTime.Parse(NextPaymentDueDate).Date < DateTime.Now.AddDays(20).Date)
                {
                    NextPaymentDueDate = nextPaymentDueDate.AddMonths(1).ToString("MM/dd/yyyy");
                    nextPaymentDueDate = DateTime.Parse(NextPaymentDueDate);
                }
                CreateApprovalLetters();
            }
            else //target screen
            {
                if (RI.CheckForText(10, 12, NextPaymentDueDate) && RI.CheckForText(6, 54, "A"))
                {
                    //if next pay due date > current date + 7 then
                    if (nextPaymentDueDate.Date > DateTime.Today.AddDays(7))
                    {
                        //if total amt due >= 1 then
                        if (double.Parse(RI.GetText(13, 26, 9)) >= 1)
                        {
                            currentAmountDue = double.Parse(RI.GetText(10, 42, 14)); //get current amount due
                            RecoveryProcessor.RecoveryPhases phase = RecoveryProcessor.RecoveryPhases.AddOptionProcessCheckByPhone;
                            if (!RecoveryProcessor.PhaseAlreadyInLog(phase)) //check recovery log
                            {
                                OPSEntry checkByPhoneEntry = new OPSEntry();
                                checkByPhoneEntry.SSN = BrwDemos.Ssn;
                                checkByPhoneEntry.AccountNumber = BrwDemos.AccountNumber.Replace(" ", "");
                                checkByPhoneEntry.FullName = FullName;
                                checkByPhoneEntry.DOB = BrwDemos.DateOfBirth;
                                checkByPhoneEntry.RoutingNumber = AchRecToAdd.ABARoutingNumber;
                                checkByPhoneEntry.BankAccountNumber = AchRecToAdd.BankAccountNumber;
                                checkByPhoneEntry.AcctType = AchRecToAdd.AccountType;
                                if (string.IsNullOrEmpty(AchRecToAdd.AdditionalWithdrawalAmount) || double.Parse(AchRecToAdd.AdditionalWithdrawalAmount) == 0)
                                {
                                    checkByPhoneEntry.PaymentAmount = currentAmountDue.ToString("#####0.00");
                                }
                                else
                                {
                                    checkByPhoneEntry.PaymentAmount = (double.Parse(AchRecToAdd.AdditionalWithdrawalAmount) + currentAmountDue).ToString("#####0.00");
                                }
                                checkByPhoneEntry.PaymentAmount = currentAmountDue.ToString("#####0.00");
                                //next payment due date needs to be on a week day not a weekend
                                if (nextPaymentDueDate.DayOfWeek.IsIn(DayOfWeek.Saturday, DayOfWeek.Sunday))
                                {
                                    while (DateTime.Parse(NextPaymentDueDate).DayOfWeek != DayOfWeek.Monday)
                                    {
                                        NextPaymentDueDate = nextPaymentDueDate.AddDays(1).ToString("MM/dd/yyyy");
                                        nextPaymentDueDate = DateTime.Parse(NextPaymentDueDate);
                                    }
                                }
                                checkByPhoneEntry.EffectiveDate = NextPaymentDueDate;
                                checkByPhoneEntry.AccountHolderName = FullName;
                                DA.AddEntryToDB(checkByPhoneEntry);
                                string comment = string.Format("{0:C} Auto pay payment. Requested payment effective date {1}.", currentAmountDue, NextPaymentDueDate);
                                Atd22AllLoans(BrwDemos.Ssn, "PHNPN", comment, "", ScriptId, false);
                                RecoveryProcessor.UpdateLogWithNewPhase(phase);
                            }
                            checkByPhoneProcessed = true;
                        }
                    }
                    else
                    {
                        NextPaymentDueDate = nextPaymentDueDate.AddMonths(1).ToString("MM/dd/yyyy"); //add one month to next pay due date
                        nextPaymentDueDate = DateTime.Parse(NextPaymentDueDate);
                    }

                    //if check by phone was not processed and NextPaymentDueDate < current date + 20 day add one month to NextPaymentDueDate
                    if (!checkByPhoneProcessed && DateTime.Parse(NextPaymentDueDate).Date < DateTime.Now.AddDays(20).Date)
                    {
                        NextPaymentDueDate = nextPaymentDueDate.AddMonths(1).ToString("MM/dd/yyyy");
                        nextPaymentDueDate = DateTime.Parse(NextPaymentDueDate);
                    }
                    CreateApprovalLetters();
                }
            }
        }

        /// <summary>
        /// Searches TS2C Target Screen for applicable 1010C Transaction
        /// </summary>
        private bool FoundApplicable1010CTransactionOnTS2CTargetScreen(double currentAmountDue)
        {
            while (RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
            {
                for (int i = 11; i < 20; i++)
                {
                    if (RI.GetText(i, 12, 8).Length > 0)
                    {
                        DateTime effectiveDate = DateTime.Parse(RI.GetText(i, 12, 8));
                        if (RI.CheckForText(i, 33, "1010C") && effectiveDate > DateTime.Parse(NextPaymentDueDate).AddMonths(-1).Date && effectiveDate < DateTime.Today)
                        {
                            //Select this transaction to check the Remittance Amount.
                            RI.PutText(22, 18, RI.GetText(i, 2, 2), ReflectionInterface.Key.Enter, true);
                            if (double.Parse(RI.GetText(19, 66, 12)) >= currentAmountDue)
                            {
                                return true;
                            }
                            RI.Hit(ReflectionInterface.Key.F12);
                        }
                    }
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
            return false;
        }

        /// <summary>
        /// Creates approval letters
        /// </summary>
        private void CreateApprovalLetters()
        {
            //create approval letters
            //approval letter for borrower
            string userPromptTextBrw = "Do you want to send an approval letter to this borrower?";
            TD22CommentData td22DataBrw = new TD22CommentData("Autopay Record Added.", false);
            double withdrawlAmount;
            if (AchRecToAdd.AdditionalWithdrawalAmount != "") { withdrawlAmount = double.Parse(AchRecToAdd.AdditionalWithdrawalAmount); }
            else { withdrawlAmount = 0; }
            ApprovalLetter(BrwDemos, td22DataBrw, NextPaymentDueDate, string.Format("{0:###,##0.00}", withdrawlAmount), userPromptTextBrw, false);
            //approval letter for bank account holder
            string tempNextPaymentDueDate;
            if (NextPaymentDueDate == "N/A")
            {
                tempNextPaymentDueDate = string.Empty;
            }
            else
            {
                tempNextPaymentDueDate = NextPaymentDueDate;
            }
            string userPromptTextBankAcctHolder = "Do you need to send a separate Account Holder Notification?";
            TD22CommentData td22DataBankAcctHolder = new TD22CommentData("Approval letter sent to account holder - ", false);
            ApprovalLetter(BrwDemos, td22DataBankAcctHolder, tempNextPaymentDueDate, string.Format("{0:###,##0.00}", withdrawlAmount), userPromptTextBankAcctHolder, true);
        }

    }
}
