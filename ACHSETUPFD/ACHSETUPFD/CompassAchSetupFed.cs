using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHSETUPFD
{
	public class CompassAchSetupFed : FedScript
	{
        //private LogDataAccess LDA;
        private ProcessLogRun LogRun;
        private List<DataClasses.EndorserRecord> endorsers;
        private DataAccess DA;

        public CompassAchSetupFed(ReflectionInterface ri)
			: base(ri, "ACHSETUPFD")
		{
            string scriptId = "ACHSETUPFD";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            //LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, false, false);
            DA = new DataAccess(LogRun);
        }

		public override void Main()
		{
            do
            {
                //indicator
                //Recovery Class
                RecoveryProcessor recoveryProc = new RecoveryProcessor(ProcessLogData);
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
                {
                    //end script if user hits cancel
                    return;
                }
                else
                {
                    //continue processing
                    bool isValidSSNOrAcctNumber = true;
                    bool first3CharsOfBothFirstNamesMatch = false;
                    SystemBorrowerDemographics brwDemos = new SystemBorrowerDemographics();
                    try
                    {
                        //get demos, do acct # to ssn translation, and do partial SSN/Acct # validation
                        brwDemos = GetDemographicsFromTx1j(mainMenuResponse.SSN);
                        endorsers = DA.GetEndorsers(mainMenuResponse.SSN); //must return a list
                        if (endorsers.Count > 0)
                        {
                            endorsers = endorsers.OrderBy(e => e.DM_PRS_LST).ThenBy(e => e.DM_PRS_1).ToList();
                        }
                        //check if first three letters of user provided first name and the system gathered first name match
                        if (brwDemos.FirstName.Substring(0, 3).ToUpper() == mainMenuResponse.FirstName.Substring(0, 3).ToUpper())
                        {
                            first3CharsOfBothFirstNamesMatch = true;
                        }
                        else
                        {
                            first3CharsOfBothFirstNamesMatch = false;
                        }
                    }
                    catch (DemographicException)
                    {
                        //borrower wasn't found on TX1J
                        isValidSSNOrAcctNumber = false;
                    }
                    //check if TS24 renders for the borrower
                    string fullName = null;
                    if (isValidSSNOrAcctNumber)
                    {
                        fullName = GetFullNameFromTS24(brwDemos.Ssn);
                        isValidSSNOrAcctNumber = fullName.IsPopulated();
                    }
                    //proceed with processing if account is valid and if first 3 chars of both first names equal
                    if (isValidSSNOrAcctNumber && first3CharsOfBothFirstNamesMatch)
                    {
                        //process according to the option selected by user
                        BaseBranchProcessor processor;
                        if (mainMenuResponse.UserSelectedACHOption == UserProvidedMainMenuData.UserSelectedACHAction.Add)
                        {
                            processor = new AddBranchProcessor(RI, brwDemos, ScriptId, fullName, recoveryProc, ProcessLogData, endorsers, DA, LogRun);
                        }
                        else if (mainMenuResponse.UserSelectedACHOption == UserProvidedMainMenuData.UserSelectedACHAction.Change)
                        {
                            processor = new ChangeBranchProcessor(RI, brwDemos, ScriptId, fullName, recoveryProc, ProcessLogData, DA, LogRun);
                        }
                        else if (mainMenuResponse.UserSelectedACHOption == UserProvidedMainMenuData.UserSelectedACHAction.Remove)
                        {
                            processor = new RemoveBranchProcessor(RI, brwDemos, ScriptId, recoveryProc, ProcessLogData, DA, LogRun); 
                        }
                        else if (mainMenuResponse.UserSelectedACHOption == UserProvidedMainMenuData.UserSelectedACHAction.Suspend)
                        {
                            processor = new SuspendBranchProcessor(RI, brwDemos, ScriptId, recoveryProc, ProcessLogData, DA, LogRun); 
                        }
                        else //missing information
                        {
                            processor = new MissingInformationBranchProcessor(RI, brwDemos, ScriptId, recoveryProc, ProcessLogData, DA, LogRun); 
                        }
                        try
                        {
                            //update recovery log if needed
                            if (!recoveryProc.IsInRecovery())
                                {
                                    //if not in recovery then update recovery log
                                    recoveryProc.UpdateLogWithUserAndOptionData(mainMenuResponse.UserSelectedACHOption, mainMenuResponse.SSN, mainMenuResponse.FirstName);
                                }
                            processor.Process();
                        }
                        catch (EndDLLException)
                        {
                            recoveryProc.DeleteRecoveryLog(); //delete recovery log since the script ended correct
                            return;
                        }
                        catch (InformationForRecoveryNotProvidedException)
                        {
                            //Not all recovery data was provided and/or known.  Allow the user to try and recover again.
                            return;
                        }

                        //If normal return, Success!, delete recovery log to finalize clean up
                        recoveryProc.DeleteRecoveryLog();
                        return;
                    }
                    else if (!isValidSSNOrAcctNumber)
                    {
                        //ssn wasn't valid
                        MessageBox.Show("The ssn/account # provided is not valid on COMPASS.  Please try again.", "Invalid SSN", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (!first3CharsOfBothFirstNamesMatch)
                    {
                        //first names don't match
                        MessageBox.Show("The first name does not match. Please try again.", "First Name Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            } while (MessageBox.Show("Would you like to process another record?", "Process Again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
		}

        /// <summary>
        /// Gets the full name of the borrower from TS24
        /// </summary>
		private string GetFullNameFromTS24(string ssn)
		{
            RI.FastPath(string.Format("TX3Z/ITS24{0}", ssn));
			if (RI.CheckForText(1, 76, "TSX25"))
			{
				return RI.GetText(4, 37, 42);
			}
			else
			{
				return null;
			}
        }

        public bool CheckRegion()
        {
            RI.FastPath("TX3Z/ITX1J");
            if (RI.CheckForText(1, 38, "UHEAAFED")) { return true; }
            else { return false; }
        }

    }
}