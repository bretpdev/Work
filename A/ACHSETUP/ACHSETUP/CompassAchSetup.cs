using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACHSETUP
{
    public class CompassAchSetup : ScriptBase
    {
        public enum Status
        {
            Continue,
            End
        }

        private ProcessLogRun LogRun { get; set; }
        public new static string ScriptId { get; } = "ACHSETUP";
        public new ReflectionInterface RI { get; set; }

        public CompassAchSetup(ReflectionInterface ri)
            : base()
        {
            RI = ri;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev);
            RI.LogRun = LogRun;
        }

        public override void Main()
        {
            if (!CheckRegion())
            {
                Dialog.Def.Ok("You are not logged into the correct region", "Error");
                return;
            }
            do
            {
                //indicator
                //Recovery Class               
                RecoveryProcessor recoveryProc = new RecoveryProcessor(LogRun);
                //Main Menu data class
                UserProvidedMainMenuData mainMenuResponse = new UserProvidedMainMenuData();
                //get ready for a dialog result
                DialogResult userResponse = DialogResult.Cancel;
                //check if script is in recovery.  Gather needed data from recovery file or from user depending on recovery status
                if (!recoveryProc.IsInRecovery())
                {
                    //Gather needed information from user
                    MainMenu mainMenuUI = new MainMenu(mainMenuResponse);
                    userResponse = mainMenuUI.ShowDialog();
                }
                else
                {
                    //gather data from recovery log
                    mainMenuResponse.UserSelectedACHOption = recoveryProc.Data.UserSelectedOption;
                    mainMenuResponse.SSN = recoveryProc.Data.SSNOrAccountNumber;
                    mainMenuResponse.FirstName = recoveryProc.Data.FirstName;
                }
                if (userResponse == DialogResult.Cancel && !recoveryProc.IsInRecovery())
                    return;
                else
                {
                    //continue processing
                    bool isValidSSNOrAcctNumber = true;
                    bool first3CharsOfBothFirstNamesMatch = false;
                    SystemBorrowerDemographics brwDemos = new SystemBorrowerDemographics();
                    try
                    {
                        //get demos, do acct # to ssn translation, and do partial SSN/Acct # validation
                        brwDemos = RI.GetDemographicsFromTx1j(mainMenuResponse.SSN);
                        if (brwDemos.Suffix.IsPopulated())
                            brwDemos.LastName = $"{brwDemos.LastName} {brwDemos.Suffix}";
                        //check if first three letters of user provided first name and the system gathered first name match
                        string demoFirst3 = brwDemos.FirstName.Length > 2 ? brwDemos.FirstName.Substring(0, 3) : brwDemos.FirstName;
                        string menuFirst3 = mainMenuResponse.FirstName.Length > 2 ? mainMenuResponse.FirstName.Substring(0, 3) : mainMenuResponse.FirstName;
                        if (demoFirst3.ToUpper() == menuFirst3.ToUpper())
                            first3CharsOfBothFirstNamesMatch = true;
                        else
                            first3CharsOfBothFirstNamesMatch = false;
                    }
                    catch (DemographicException)
                    {
                        isValidSSNOrAcctNumber = false;//borrower wasn't found on TX1J
                    }
                    //check if TS24 renders for the borrower
                    string fullName = null;
                    if (isValidSSNOrAcctNumber)
                    {
                        fullName = GetFullNameFromTS24(brwDemos.Ssn);
                        isValidSSNOrAcctNumber = !string.IsNullOrEmpty(fullName);
                    }
                    //proceed with processing if account is valid and if first 3 chars of both first names equal
                    if (isValidSSNOrAcctNumber && first3CharsOfBothFirstNamesMatch)
                    {
                        //process according to the option selected by user
                        BaseBranchProcessor processor;
                        if (mainMenuResponse.UserSelectedACHOption == UserProvidedMainMenuData.UserSelectedACHAction.Add)
                        {
                            List<Ts26LoanData> data = GetTs26LoanData(brwDemos.Ssn);
                            processor = new AddBranchProcessor(RI, brwDemos, ScriptId, fullName, recoveryProc, data);
                        }
                        else if (mainMenuResponse.UserSelectedACHOption == UserProvidedMainMenuData.UserSelectedACHAction.Change)
                            processor = new ChangeBranchProcessor(RI, brwDemos, ScriptId, fullName, recoveryProc);
                        else if (mainMenuResponse.UserSelectedACHOption == UserProvidedMainMenuData.UserSelectedACHAction.Remove)
                            processor = new RemoveBranchProcessor(RI, brwDemos, ScriptId, recoveryProc);
                        else if (mainMenuResponse.UserSelectedACHOption == UserProvidedMainMenuData.UserSelectedACHAction.Suspend)
                            processor = new SuspendBranchProcessor(RI, brwDemos, ScriptId, recoveryProc);
                        else //missing information
                            processor = new MissingInformationBranchProcessor(RI, brwDemos, ScriptId, recoveryProc);
                        try
                        {
                            //update recovery log if needed
                            if (!recoveryProc.IsInRecovery())//if not in recovery then update recovery log
                                recoveryProc.UpdateLogWithUserAndOptionData(mainMenuResponse.UserSelectedACHOption, mainMenuResponse.SSN, mainMenuResponse.FirstName);
                            processor.Process();
                        }
                        catch (InformationForRecoveryNotProvidedException)
                        {
                            //Not all recovery data was provided and/or known.  Allow the user to try and recover again.
                            return;
                        }

                        //If normal return, Success!, delete recovery log to finalize clean up
                        End(recoveryProc);
                    }
                    else if (!isValidSSNOrAcctNumber)//ssn wasn't valid
                        Dialog.Warning.Ok("The ssn/account # provided isn't valid on COMPASS.  Please try again.", "Invalid SSN");
                    else if (!first3CharsOfBothFirstNamesMatch)//first names don't match
                        Dialog.Warning.Ok("The first name provided doesn't match the first name found on the system.  Please try again.", "First Name Don't Match");
                }
            } while (MessageBox.Show("Would you like to process another record?", "Process Again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void End(RecoveryProcessor recoveryProc)
        {
            LogRun.LogEnd();
            recoveryProc.DeleteRecoveryLog(); //delete recovery log since the script ended correct
            return;
        }

        /// <summary>
        /// Goes to TS26 and gets the lender id, loan add date and loan seq for each loan with a balance.
        /// </summary>
        /// <param name="ssn">Borrowers Ssn</param>
        /// <returns>Ts26Loan Data Object</returns>
        private List<Ts26LoanData> GetTs26LoanData(string ssn)
        {
            List<Ts26LoanData> data = new List<Ts26LoanData>();
            RI.FastPath($"TX3Z/ITS26{ssn}");

            if (RI.ScreenCode =="TSX29")
                data.Add(new Ts26LoanData() { LenderId = RI.GetText(7, 48, 6), LoanSeq = RI.GetText(7, 35, 4), LoanAddDate = RI.GetText(12, 18, 8).ToDate() });
            else
            {
                for (int row = 8; RI.MessageCode != "90007"; row++)
                {
                    if (row > 20 || RI.GetText(row, 2, 2).IsNullOrEmpty())
                    {
                        RI.Hit(Key.F8);
                        row = 7;
                        continue;
                    }
                    if (RI.GetText(row, 48, 11).Replace(",", "").ToDecimal() > 0)
                    {
                        RI.PutText(21, 12, RI.GetText(row, 2, 2), Key.Enter, true);
                        data.Add(new Ts26LoanData() { LenderId = RI.GetText(7, 48, 6), LoanSeq = RI.GetText(7, 35, 4), LoanAddDate = RI.GetText(12, 18, 8).ToDate() });
                        RI.Hit(Key.F12);
                    }
                }
            }
            return data;
        }

        public bool CheckRegion()
        {
            RI.FastPath("TX3Z/ITX1J");
            if (RI.CheckForText(1, 38, "UHEAAFED"))
                return false;
            else
                return true;
        }

        private string GetFullNameFromTS24(string ssn)
        {
            RI.FastPath(string.Format("TX3Z/ITS24{0}", ssn));
            if (RI.CheckForText(1, 76, "TSX25"))
                return RI.GetText(4, 37, 42);
            else
                return null;
        }
    }
}