using System;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;


namespace INCARBWRS
{
    public class IncarceratedBorrowers : ScriptBase
    {
        private const string scriptId = "INCARBWRS";

        private readonly string startingSsn = null;

        private const string KJAIL_MESSAGE = "The information is incomplete so the script will create a KJAIL action request on COMPASS for the prison information to be verified.";

        private const string SJAIL_MESSAGE = "The information is incomplete so the script will create a SKIPPRSN queue task and an SJAIL action request on OneLINK for the prison information to be verified.";

        public IncarceratedBorrowers(ReflectionInterface ri, string startingSsn)
            : base(ri, scriptId, DataAccessHelper.Region.Uheaa)
        {
            this.startingSsn = startingSsn;
            PLR = new ProcessLogRun(ProcessLogData.ProcessLogId, scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true);
        }

        public IncarceratedBorrowers(ReflectionInterface ri)
            : this( ri, null)
        {
            this.startingSsn = "Test";
        }

        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }

        [STAThread]
        public override void Main()
        {
            DA = new DataAccess(this.PLR);

            PrisonInfo prison = GetPrisonInfo();
            if (prison == null)
            {
                MessageBox.Show("Prison info could not be gathered. Exiting program.", "Unrecoverable Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (startingSsn != null)
            {
                prison.SSN = startingSsn;
            }

            //Determine processing based on the borrower's loan types.
            Helper.LoanType loanTypes = GetLoanTypes(prison.SSN);
            //bool borrowerHasOnlyNonFfelLoans = ((loanTypes & Helper.LoanType.NonFfel) == Helper.LoanType.NonFfel && (loanTypes & Helper.LoanType.Ffel) != Helper.LoanType.Ffel);
            bool borrowerHasOnlyNonFfelLoans = loanTypes.HasFlag(Helper.LoanType.NonFfel) && !loanTypes.HasFlag(Helper.LoanType.Ffel);
            bool borrowerHasFfelLoans = loanTypes.HasFlag(Helper.LoanType.Ffel);
            if (borrowerHasOnlyNonFfelLoans)
            {
                if (prison.IsComplete)
                {
                    UpdateCompass(prison);
                }
                else
                {
                    MessageBox.Show(KJAIL_MESSAGE, "Information Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AddCompassArc(prison.SSN, "KJAIL", prison.CommentText);
                }
            }

             if (borrowerHasFfelLoans)
            {
                if (prison.IsComplete)
                {
                    UpdateOneLink(prison);
                    if (loanTypes.HasFlag(Helper.LoanType.Compass))
                    {
                        UpdateCompass(prison);
                    }
                }
                else
                {
                    AddQueueTaskInLP9O(prison.SSN, "SKIPPRSN", null, prison.CommentText, null, null, null);
                    AddCommentInLP50(prison.SSN, prison.Contact.ActivityType, prison.Contact.ContactType, "SJAIL", prison.CommentText, scriptId);
                    MessageBox.Show(SJAIL_MESSAGE, "Information Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            MessageBox.Show("Processing is complete.", "Incarcerated Script", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /// <summary>
        /// Adds arc in Compass
        /// </summary>
        private void AddCompassArc(string ssn, string arc, string comment)
        {
            if (!RI.Atd22AllLoans(ssn, arc, comment, null, scriptId, false))
            {
                const string CAPTION = "Action Request Not Added";
                string message = $"The {arc} ARC cannot be added.";
                MessageBox.Show(message, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                string logMessage = $"The {arc} ARC cannot be added for borrower: {ssn}";
                PLR.AddNotification(logMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
        }

        /// <summary>
        /// Get loan types from TS26
        /// </summary>
        private Helper.LoanType GetLoanTypes(string ssn)
        {
            Helper.LoanType loanTypes = Helper.LoanType.Compass;
            RI.FastPath("TX3Z/ITS26" + ssn);
            if (RI.CheckForText(1, 72, "TSX28")) //Selection Screen
            {
                int row = 8;
                while (!RI.CheckForText(23, 2, "90007"))
                {
                    if (this.DA.IsLoanType(RI.GetText(row, 19, 6), "FFEL"))
                        loanTypes |= Helper.LoanType.Ffel;
                    else
                        loanTypes |= Helper.LoanType.NonFfel;
                    if (loanTypes.HasFlag(Helper.LoanType.Ffel) && loanTypes.HasFlag(Helper.LoanType.NonFfel)) { break; }
                    row++;
                    if (RI.CheckForText(row, 3, " "))
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 8;
                    }
                }
            }
            else if (RI.CheckForText(1, 72, "TSX29")) //Target Screen
            {
                if (DA.IsLoanType(RI.GetText(6, 66, 6), "FFEL"))
                    loanTypes |= Helper.LoanType.Ffel;
                else
                    loanTypes |= Helper.LoanType.NonFfel;
            }
            else //No Compass loans
                loanTypes = Helper.LoanType.Ffel;
            return loanTypes;
        }

        /// <summary>
        /// Displays the Prison form to collect the prison data
        /// </summary>
        private PrisonInfo GetPrisonInfo()
        {
            PrisonInfo prison = new PrisonInfo();
            using (PrisonDialog dialog = new PrisonDialog(DA.GetStateCodes(), DA.GetContactSources(), prison))
            {
                bool readyToGo = false;
                while (!readyToGo)
                {
                    switch (dialog.ShowDialog())
                    {
                        case DialogResult.OK:
                            List<string> errors = GetValidationErrors(prison);
                            if (errors.Count == 0)
                                readyToGo = true;
                            else
                            {
                                errors.Insert(0, "Please correct the following problems:");
                                string message = string.Join(Environment.NewLine, errors.ToArray());
                                MessageBox.Show(message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            break;
                        case DialogResult.Cancel:
                            prison = null;
                            return prison;
                    }
                }
            }
            return prison;
        }

        /// <summary>
        /// Checks the entered in data from the form and gets a list of errors
        /// </summary>
        private List<string> GetValidationErrors(PrisonInfo prison)
        {
            List<string> errors = new List<string>();
            //Check that a valid SSN or account number was entered.
            if (string.IsNullOrEmpty(prison.SSN) || prison.SSN.Length < 9 || prison.SSN.Length > 10)
                errors.Add("You must provide a 9-digit SSN or 10-digit account number.");
            else
                GetAccountFromSession(prison, errors);

            //Check the anticipated release date, if provided.
            if (!string.IsNullOrEmpty(prison.AnticipatedReleaseDate))
            {
                DateTime? anticipatedReleaseDate = ConversionHelper.ToDateNullable(prison.AnticipatedReleaseDate);
                if (anticipatedReleaseDate == null)
                    errors.Add("The Anticipated Release Date isn't a valid date.");
            }

            //Check the follow-up date, if provided.
            if (!string.IsNullOrEmpty(prison.FollowUpDate))
            {
                DateTime? followUpDate = ConversionHelper.ToDateNullable(prison.FollowUpDate);

                if ( followUpDate != null)
                {
                    followUpDate += DateTime.Now.TimeOfDay;
                    if (followUpDate > DateTime.Now.AddYears(1) || followUpDate < DateTime.Now)
                        errors.Add("The Follow-Up Date must be in the future, but not by more than one year.");
                }
                else
                    errors.Add("The Follow-Up Date isn't a valid date.");
            }

            //Check that a contact source was selected.
            if (prison.Contact == null || string.IsNullOrEmpty(prison.Contact.Source))
                errors.Add("No source was selected.");

            return errors;
        }

        /// <summary>
        /// Check the session for the account in LP22 then TX1J
        /// </summary>
        /// <param name="prison"></param>
        /// <param name="errors"></param>
        private void GetAccountFromSession(PrisonInfo prison, List<string> errors)
        {
            try
            {
                prison.SSN = GetDemographicsFromLP22(prison.SSN).Ssn;
            }
            catch (DemographicException)
            {
                try
                {
                    prison.SSN = GetDemographicsFromTx1j(prison.SSN).Ssn;
                }
                catch (DemographicException)
                {
                    errors.Add("The SSN or account number could not be found on OneLINK or COMPASS.");
                }
            }
        }

        /// <summary>
        /// Gets loan statues from LC02
        /// </summary>
        private Helper.LoanStatus GetLoanStatuses(string ssn)
        {
            RI.FastPath("LC02I" + ssn);
            if (RI.CheckForText(1, 68, "PRECLAIM MENU")) { return Helper.LoanStatus.None; }

            //Select the first loan if needed.
            if (RI.CheckForText(1, 66, "PRECLAIM SELECT")) { RI.PutText(21, 13, "01", ReflectionInterface.Key.Enter); }

            //Get the status of the first loan.
            Helper.LoanStatus statuses = Helper.LoanStatus.None;
            if (RI.CheckForText(4, 11, "03"))
                statuses |= Helper.LoanStatus.Default;
            else
                statuses |= Helper.LoanStatus.NonDefault;

            //Note identifying info for this loan, so we can recognize when we've looped back to it.
            string firstClaimStatus = RI.GetText(4, 11, 2);
            string firstClaimDate = RI.GetText(2, 54, 8);

            //Loop through the remaining loans until we come back to the first one.
            RI.Hit(ReflectionInterface.Key.F4);
            while (RI.GetText(4, 11, 2) != firstClaimStatus && RI.GetText(2, 54, 8) != firstClaimDate)
            {
                if (RI.CheckForText(4, 11, "03"))
                    statuses |= Helper.LoanStatus.Default;
                else
                    statuses |= Helper.LoanStatus.NonDefault;
                RI.Hit(ReflectionInterface.Key.F4);
            }

            return statuses;
        }

        /// <summary>
        /// Updates Compass demos
        /// </summary>
        /// <param name="prison"></param>
        private void UpdateCompass(PrisonInfo prison)
        {
            string comment = $"Follow up on {prison.FollowUpDate} {prison.CommentText}";
            AddCompassArc(prison.SSN, "KPRIS", comment);
            UpdateTX1J(prison);
        }

        /// <summary>
        /// Update LP22 with the borrowers prison demo info
        /// </summary>
        private bool UpdateLP22(PrisonInfo prison)
        {
            

            RI.FastPath("LP22C" + prison.SSN);
            if (RI.GetText(22, 3, 5) == "47004")
            {
                MessageBox.Show("No account found in LP22.", "LP22: Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            //Invalidate current address and phone.
            RI.PutText(3, 9, "K");
            RI.PutText(10, 57, "N");
            RI.PutText(13, 38, "N");
            RI.Hit(ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6);
            //Enter prison address and phone.
            RI.PutText(3, 9, "K");
            RI.PutText(10, 9, prison.Name, true);
            RI.PutText(10, 57, "Y"); //Valid address
            RI.PutText(11, 9, $"{prison.Address} {prison.InmateNumber}", true);
            RI.PutText(12, 9, prison.City, true);
            RI.PutText(12, 52, prison.State);
            RI.PutText(12, 60, prison.ZIP, true);
            RI.PutText(13, 12, prison.Phone);
            RI.PutText(13, 38, "Y"); //Valid phone
            RI.PutText(13, 68, "T");
            RI.Hit(ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6);
            string eax = RI.GetText(22, 3, 5);
            if ( eax != "40639" && eax != "49000")
            {
                MessageBox.Show("Error in one or more field entries.", "LP22: Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Update OneLink demographic info with the borrowers Prison demos
        /// </summary>
        /// <param name="prison"></param>
        private bool UpdateOneLink(PrisonInfo prison)
        {
            if (!UpdateLP22(prison))
                return false;

            Helper.LoanStatus statuses = GetLoanStatuses(prison.SSN);
            string queue = "";
            if (statuses.HasFlag(Helper.LoanStatus.Default))
            {
                queue = "DPRISON";
                AddQueueTaskInLP9O(prison.SSN, queue, null, null, null, null, null);
            }
            if (statuses.HasFlag(Helper.LoanStatus.NonDefault))
            {
                string nonDefaultQueue = "PRISONRQ";
                AddQueueTaskInLP9O(prison.SSN, nonDefaultQueue, null, null, null, null, null);
                if (!string.IsNullOrEmpty(queue)) { queue += " and "; }
                queue += nonDefaultQueue;
            }
            string comment = $"Borrower incarcerated at {prison.Name} until {prison.AnticipatedReleaseDate}; task in {queue}; updated LP22; follow up on {prison.FollowUpDate}";
            AddCommentInLP50(prison.SSN, prison.Contact.ActivityType, prison.Contact.ContactType, "KPRIS", comment, ScriptId);
            return true;
        }

        /// <summary>
        /// Update TX1J with the borrowers Prison demo info
        /// </summary>
        /// <param name="prison"></param>
        private void UpdateTX1J(PrisonInfo prison)
        {
            //Update address.
            RI.FastPath("TX3Z/CTX1JB" + prison.SSN);
            RI.Hit(ReflectionInterface.Key.F6);
            RI.Hit(ReflectionInterface.Key.F6);
            RI.PutText(8, 18, "44");
            RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"));
            RI.PutText(11, 10, prison.Name, true);
            RI.PutText(11, 55, "Y");
            RI.PutText(12, 10, $"{prison.Address} {prison.InmateNumber}", true);
            RI.PutText(14, 8, prison.City, true);
            RI.PutText(14, 32, prison.State, true);
            RI.PutText(14, 40, prison.ZIP, true);
            RI.Hit(ReflectionInterface.Key.Enter);

            //Update compass phone information.
            RI.Hit(ReflectionInterface.Key.F6);
            UpdateCompassPhone();
            if (RI.CheckForText(23, 2, "06394"))
            {
                InvalidateOtherPhoneNumbers("A");
                InvalidateOtherPhoneNumbers("W");
                InvalidateOtherPhoneNumbers("M");
                RI.PutText(16, 14, "H", ReflectionInterface.Key.Enter);
                UpdateCompassPhone();
            }
        }

        /// <summary>
        /// update compass phone information
        /// </summary>
        private void UpdateCompassPhone()
        {
            //Remove phone.
            RI.PutText(16, 20, "U");
            RI.PutText(16, 30, "N");
            RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"));
            RI.PutText(17, 14, "", true);
            RI.PutText(17, 23, "", true);
            RI.PutText(17, 31, "", true);
            RI.PutText(17, 40, "", true);
            RI.PutText(17, 54, "Y");
            RI.PutText(17, 67, "J");
            RI.PutText(19, 14, "44");
            RI.Hit(ReflectionInterface.Key.Enter);
        }

        /// <summary>
        /// Invalidates the borrowers other phone number
        /// </summary>
        /// <param name="phoneType"></param>
        private void InvalidateOtherPhoneNumbers(string phoneType)
        {
            RI.PutText(16, 14, phoneType, ReflectionInterface.Key.Enter);
            RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"));
            RI.PutText(17, 54, "N");
            RI.PutText(19, 14, "31", ReflectionInterface.Key.Enter);
        }
    }
}