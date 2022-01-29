using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace IDRUSERPRO
{

    public class IDRProcessing
    {
        public enum AppResult
        {
            BadData,
            EndScript,
            Success
        }

        public SystemBorrowerDemographics Demo { get; set; }

        private enum InputResult
        {
            AllDataIsValid,
            Cancel,
            BadDataEntered,
            Return
        }

        private DataAccess DA { get; set; }
        private bool IsPending { get; set; }
        private RecoveryLog Recovery { get; set; }
        private ReflectionInterface RI { get; set; }
        private string ScriptId { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private string UserId { get; set; }

        public IDRProcessing(string scriptId, string userId, ReflectionInterface ri)
        {
            Application.EnableVisualStyles();
#if !DEBUG
            Application.SetCompatibleTextRenderingDefault(false);
#endif
            Recovery = new RecoveryLog(string.Format("{0}_{1}_{2}", scriptId, userId, DataAccessHelper.CurrentRegion));
            RI = ri;
            ScriptId = scriptId;
            UserId = userId;
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DA = new DataAccess(LogRun);
        }

        public void Process()
        {
            if (!DatabaseAccessHelper.StandardSprocAccessCheck(Assembly.GetExecutingAssembly()))
            {
                LogRun.LogEnd();
                return;
            }
            ApplicationState currentState = new ApplicationState();

            //If the user selects no then the script will delete the existing recovery.
            if (!Recovery.RecoveryValue.IsNullOrEmpty() && MessageBox.Show("The script has detected you are in recovery.  Do you want to continue processing?", "Continue?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<string> recValues = Recovery.RecoveryValue.SplitAndRemoveQuotes(",");
                currentState.Loans = new BorrowerExistingLoans(new List<Ts26Loans>());
                string ssn = DA.GetSsnFromAppId(recValues[0].ToInt());
                try
                {
                    currentState.AppId = recValues[0].ToInt();
                    currentState.NewApp = false;
                    Demo = RI.GetDemographicsFromTx1j(ssn);
                    currentState.BorData = GetTx1jInfo(Demo.AccountNumber);
                }
                catch (DemographicException)
                {
                    currentState.MisroutedApp = true;
                    currentState.BorData = new BorrowerInfo();
                    currentState.BorData.AccountNumber = ssn;
                    currentState.BorData.Ssn = ssn;
                }
            }
            else
                Recovery.Delete();



            //This will be used to return to the account number entry form if the user select cancel on the IdrInformation form.
            while (!currentState.DoneProcessing)
            {
                //We do not want to show the account entry form when in recovery.
                if (Recovery.RecoveryValue.IsNullOrEmpty())
                    currentState = ShowAccountEntry(currentState);
                if (currentState == null)
                    return;


                BorrowerLoanDebtStatus stat = DA.GetBorrowerLoanDebtStatus(currentState.BorData.Ssn);
                bool borrowerFound = stat.HasAllLoansDeconverted.HasValue && stat.HasReleasedLoans.HasValue;
                if (borrowerFound && !stat.HasAllLoansDeconverted.Value && !stat.HasReleasedLoans.Value) //No released loans, yet they don't have all deconverted loans. Possibly a refresh is needed to bring new loans in.
                {
                    if (Dialog.Info.YesNo("The borrower has no loans on record. Do you want to place this task on hold to wait for upcoming loan information?"))
                    {
                        currentState.DoneProcessing = true;
                        CreateFutureDatedTask(Demo.AccountNumber, "No loans on record yet. Placing the task on hold", null);
                        break;
                    }
                }
                else if (borrowerFound && stat.HasAllLoansDeconverted.Value)
                {
                    if (!Dialog.Info.YesNo("The borrower currently has all deconverted loans. Do you want to continue processing this application?"))
                    {
                        currentState.DoneProcessing = true;
                        break;
                    }
                }

                while (true)
                {
                    currentState = MainFormProcessing(currentState);
                    if (currentState == null)
                        return;
                    if (currentState.DoneProcessing || currentState.Cancel)
                    {
                        if (!Recovery.RecoveryValue.IsNullOrEmpty())
                            currentState.DoneProcessing = true;
                        break;
                    }
                }
            }

            if (IsPending)
                CreateFutureDatedTask(currentState.BorData.AccountNumber, currentState.Comment, currentState.AppInfo.EApplicationId.Trim().IsPopulated() ? currentState.AppInfo.EApplicationId : currentState.AppId.ToString());

            CloseOpenTask();
            ProcessingComplete();
        }

        /// <summary>
        /// Launches the future dated task script.
        /// </summary>
        private void CreateFutureDatedTask(string accountNumber, string comment, string appId)
        {
            using (FutureDatedTasksDays task = new FutureDatedTasksDays())
            {
                task.ShowDialog();

                ArcData data = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = accountNumber,
                    Arc = "IDRPN",
                    ScriptId = ScriptId,
                    ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                    ProcessOn = DateTime.Now.AddDays((int)task.Days),
                    RecipientId = "",
                    Comment = appId == null ? $"{comment}" : $"Application:{appId} {comment}",
                };

                var tryAddArc = new Action(() =>
                { data.AddArc(); });
                var repeaterResult = Repeater.TryRepeatedly(tryAddArc);
                if (!repeaterResult.Successful)
                {
                    LogRun.AddNotification($"Unable to add arc {data.Arc} for account {data.AccountNumber} with comment: {data.Comment}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Dialog.Error.Ok("The Application was unable to enter the future dated ARC IDRPN for this pending applcation in order to create a future task.  Please make sure it is added.");
                }
            }
        }

        /// <summary>
        /// Close the 2A or 2P task
        /// </summary>
        private void CloseOpenTask()
        {
            //Check to see if they have 2 different tasks open.
            bool open2ATask = CheckTask("2A;01");
            bool open2PTask = CheckTask("2P;01");
            if (open2ATask && open2PTask)
                ProcessingComplete("There are multiple queue tasks open.  The tasks will need to be reviewed and close manually.");

            if (!open2ATask && !open2PTask)
            {
                ProcessingComplete("Could not find open 2A or 2P queue task. Review the queues and close the task manually.");
                return;
            }
            RI.CloseCompassQueue(open2ATask ? "2A;01" : "2P;01", "C", "COMPL");
        }


        /// <summary>
        /// Checks to see if a task is open
        /// </summary>
        /// <param name="task">Task to check</param>
        /// <returns>True is a task is open.</returns>
        private bool CheckTask(string task)
        {
            RI.FastPath("TX3Z/ITX6X" + task);
            return RI.CheckForText(8, 75, "W");
        }

        /// <summary>
        /// Goes to TX1J and gathers demographic information for a given account number 
        /// </summary>
        /// <param name="accountNumber">Account Number of the borrower to get the demographic for</param>
        /// <returns>A Borrower Info object with Account NUmber, Ssn, and Name</returns>
        private BorrowerInfo GetTx1jInfo(string accountNumber)
        {
            RI.FastPath("TX3Z/ITX1JB");
            RI.PutText(6, 16, "", true);
            RI.PutText(6, 20, "", true);
            RI.PutText(6, 23, "", true);
            RI.PutText(6, 61, accountNumber, ReflectionInterface.Key.Enter);

            return new BorrowerInfo()
            {
                AccountNumber = accountNumber,
                Ssn = RI.GetText(3, 12, 11).Replace(" ", ""),
                FirstName = RI.GetText(4, 34, 14),
                LastName = RI.GetText(4, 6, 24),
                MiddleName = RI.GetText(4, 53, 1).Contains("_") ? string.Empty : RI.GetText(4, 53, 14)
            };
        }

        /// <summary>
        /// Shows the account entry form that allows the user to enter and account number and select the type of application.
        /// </summary>
        /// <param name="currentState">Object containing properties about the application.</param>
        /// <returns>An ApplicationState object with updated properties about the application.</returns>
        private ApplicationState ShowAccountEntry(ApplicationState currentState)
        {
            using (AccountEntry entry = new AccountEntry(RI, DA))
            {
                //end the script if the user selects cancel on this screen.
                if (entry.ShowDialog() == DialogResult.Cancel)
                    return null;

                Demo = entry.Demo;

                currentState = ApplicationState.UpdateCurrentState(entry.Loans, currentState.BorData, entry.NewApp, false, entry.MisroutedApp, entry.NoBalance, false, false, entry.SelectedApp);
                currentState.FirstTimeApp = entry.FirstTimeApp;
                if (entry.SelectedApp != null)
                    currentState.AppId = entry.SelectedApp.App_ID;

                if (!currentState.MisroutedApp)
                    currentState.BorData = GetTx1jInfo(entry.AccountNumber);
                else
                {
                    currentState.BorData = new BorrowerInfo()
                    {
                        AccountNumber = entry.AccountNumber.IsNullOrEmpty() ? entry.SSN : entry.AccountNumber,
                        Ssn = entry.SSN
                    };
                }
            }

            LogRun.AddNotification(string.Format("ACCOUNT NUMBER:{0}", Demo?.AccountNumber), NotificationType.Other, NotificationSeverityType.Informational);
            return currentState;
        }

        /// <summary>
        /// Calls the method that shows the IdrInformation form and handles the results that come back from the form.
        /// </summary>
        /// <param name="currentState">Object containing properties about the application.</param>
        /// <returns>An ApplicationState object with updated properties about the application.</returns>
        private ApplicationState MainFormProcessing(ApplicationState currentState)
        {
            InputResult result = ShowMainFormAndProcessTheApp(currentState);
            if (result == InputResult.Return)
                return null;
            if (result == InputResult.Cancel)
                currentState.Cancel = true;
            else if (result == InputResult.AllDataIsValid)
                currentState.DoneProcessing = true;
            else if (result == InputResult.BadDataEntered)
            {
                currentState.NewApp = false;
                currentState.UserEnterInvalidData = true;
            }

            return currentState;
        }

        /// <summary>
        /// Shows the IdrInformation form and adds the Application to the System and generates a letter and ARC
        /// </summary>
        /// <param name="currentState">Object containing properties about the application.</param>
        /// <returns>enum with the results of adding the application</returns>
        private InputResult ShowMainFormAndProcessTheApp(ApplicationState currentState)
        {
            //using (IdrInformation info = new IdrInformation(currentState, LogRun.ProcessLogId, DA))
            using (ApplicationEntry appEntry = new ApplicationEntry(currentState, DA, LogRun, RI, UserId, Recovery))
            {
                if (!currentState.NewApp && !appEntry.FoundExistingApp)
                    return InputResult.Cancel;

                if (appEntry.ShowDialog() == DialogResult.Cancel)
                    return InputResult.Cancel;

                IsPending = appEntry.IsPending;

                if (currentState.MisroutedApp || !appEntry.UserInputedData.Active || appEntry.FamilySizeHold)
                    ProcessingComplete();

                CheckLogin();

                List<int> loanSeq = null;

                if (appEntry.Approved)
                {
                    IdrApplicationHelper helper = new IdrApplicationHelper(RI, Recovery, DA, LogRun);
                    loanSeq = helper.CheckTS7C(RI, currentState.BorData.Ssn, currentState.Loans.FilteredLoans, appEntry.Result.EligibilityIndicators.Indicators);

                    AppResult result = new IdrApplicationHelper(RI, Recovery, DA, LogRun).AddTheApp(appEntry, Demo);

                    if (result == AppResult.EndScript)
                        return InputResult.Return;
                    else if (result == AppResult.BadData)
                        return InputResult.BadDataEntered;
                }
                if (Recovery.RecoveryValue.Contains("Database Updated"))
                    Recovery.RecoveryValue = string.Format("{0},IDR Added", appEntry.UserInputedData.ApplicationId);

                CommentLetterData cld = new CommentLetterData(DA, LogRun);

                if (!cld.GenerateArcAndLetter(RI, appEntry, Recovery, Demo, loanSeq, ScriptId, UserId, appEntry.WaitingForNSLDS))
                    return InputResult.Return;

                currentState.Comment = cld.Comment;

                currentState.AppInfo = appEntry.UserInputedData;
                return InputResult.AllDataIsValid;
            }
        }

        /// <summary>
        /// This is not inheriting batchscript so we do not have access to this common method.
        /// </summary>
        private void ProcessingComplete(string message = "Processing Complete")
        {
            MessageBox.Show(message);
            Recovery.Delete();
            if (LogRun != null && LogRun.ProcessLogId > 0)
                ProcessLogger.LogEnd(LogRun.ProcessLogId);
        }

        /// <summary>
        /// Checks to see if the user is still logged into the system.  If the user is not logged it it will allow user to log in and press insert,
        /// the region will then be validated to ensure they are in the correct region.
        /// </summary>
        private void CheckLogin()
        {
            bool isLoggedIn = false;
            while (!isLoggedIn)
            {
                isLoggedIn = true;
                if (!RI.IsLoggedIn)
                {
                    MessageBox.Show(string.Format("You are no longer logged into the {0} region. Please re-login to the {0} region and hit Insert to continue.", DataAccessHelper.CurrentRegion));
                    RI.PauseForInsert();
                    isLoggedIn = false;
                }


                if (!RI.ValidateRegion(DataAccessHelper.CurrentRegion))
                {
                    isLoggedIn = false;
                    RI.PauseForInsert();
                }

            }
        }
    }
}