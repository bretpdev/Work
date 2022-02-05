using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using AUXLTRS.Object_Classes;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;


namespace AUXLTRS
{
    public class CollectorLetters : ScriptBase
    {
        public bool CalledByMauiDude { get; set; }
        public string MauiDudeSsn { get; set; }
        public Q.MDBorrower Bor { get; set; }
        public ProcessLogRun logRun { get; set; }
        public DataAccess DA { get; set; }
        public List<CoBorrowers> coBorrowers { get; set; } // Could be One Link or Compass


        /// <summary>
        /// Use this constructor when launching from a session
        /// </summary>
        public CollectorLetters(Uheaa.Common.Scripts.ReflectionInterface ri)
            : base(ri, "AUXLTRS", DataAccessHelper.Region.Uheaa)
        {
            CalledByMauiDude = false;
        }

        public CollectorLetters(ReflectionInterface ri, Q.MDBorrower borrower, int runNumber)
            : this(ri)
        {
            MauiDudeSsn = borrower.SSN;
            CalledByMauiDude = true;
            Bor = borrower;
        }

        /// <summary>
        /// Use thie constructor when launching from Maui DUDE
        /// Note that for the Maui DUDE .dll, ALL references in the project must be combined
        /// into the single .dll for the test area. (Y:\Codebase\Scripts).
        /// </summary>
        public CollectorLetters(ReflectionInterface ri, string ssn)
            : this(ri)
        {
            CalledByMauiDude = true;
            MauiDudeSsn = ssn;
        }

        public override void Main()
        {
            logRun = new ProcessLogRun("AUXLTRS", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DA = new DataAccess(logRun.ProcessLogId);
            coBorrowers = new List<CoBorrowers>();
            MainMenu.SelectionContainer selection;


            do
            {
                selection = new MainMenu.SelectionContainer();
                using (MainMenu mainMenu = new MainMenu(selection))

                {
                    //if (mainMenu.ShowDialog() != DialogResult.OK) { EndDllScript(); }
                    if (mainMenu.ShowDialog() != DialogResult.OK) { selection.Value = MainMenu.Selection.Exit; }
                }

                switch (selection.Value)
                {
                    case MainMenu.Selection.CustomLetter:
                        GeneralProcessing();
                        break;
                    case MainMenu.Selection.DobConflictLetter:
                        ConflictProcessing("DBCON", EnterpriseFileSystem.TempFolder + "DOB Conflict dat.txt", "DOBCFLT");
                        break;
                    case MainMenu.Selection.NameConflictLetter:
                        ConflictProcessing("NMCON", EnterpriseFileSystem.TempFolder + "Name Conflict dat.txt", "NMCFLT");
                        break;
                    case MainMenu.Selection.NoticeOfSatisfaction:
                        NoticeOfSatisfactionProcessing();
                        break;
                    case MainMenu.Selection.NsldsSsn1:
                        NsldsSsnProcessing("N1SSN", "SSN1");
                        break;
                    case MainMenu.Selection.NsldsSsn2:
                        NsldsSsnProcessing("N2SSN", "SSN2");
                        break;
                    case MainMenu.Selection.NsldsSsn3:
                        NsldsSsnProcessing("N3SSN", "SSN3");
                        break;
                    case MainMenu.Selection.PostOfficeLetter:
                        PostOfficeProcessing();
                        break;
                    case MainMenu.Selection.PropertyOwnerInformationRequest:
                        PropertyOwnerProcessing();
                        break;
                    case MainMenu.Selection.RequestForHearing:
                        RequestForHearingProcessing();
                        break;
                    case MainMenu.Selection.SsnConflictLetter:
                        ConflictProcessing("SSCON", EnterpriseFileSystem.TempFolder + "SSN Conflict dat.txt", "SSNCFLT");
                        break;
                    case MainMenu.Selection.ThirdPartyAuthorizationForm:

                        ThrdPrtyAuth.ThirdPartyAuthorization thirdParty = new ThrdPrtyAuth.ThirdPartyAuthorization(RI);
                        thirdParty.Main();
                        break;
                    default:
                        //Debug.Assert(false, "Unrecognized Selection: " + selection.ToString());
                        break;
                }
            } while (selection.Value != MainMenu.Selection.Exit);
        }

        /// <summary>
        /// Process conflicts
        /// </summary>
        private void ConflictProcessing(string arc, string letterDataFile, string letterId)
        {
            string docFolder = EnterpriseFileSystem.GetPath("AuxServicesFolder");

            //Prompt the user for an SSN/Account Number
            for (string ssnOrAccountNumber = GetBorrowerIdFromUser(); ssnOrAccountNumber.Length > 0; ssnOrAccountNumber = GetBorrowerIdFromUser())
            {
                BorrowerDemographicsFromSpecifiedSystem borrower = GetDemographicsFromEitherSystem(ssnOrAccountNumber, BorrowerDemographicsFromSpecifiedSystem.AesSystem.OneLink);
                if (borrower == null)
                {
                    MessageBox.Show("That wasn't a valid SSN or Account Number. Please try again.");
                    //EndDllScript();
                    return;
                }
                Uheaa.Common.Scripts.SystemBorrowerDemographics systemBorrower = borrower.Demographics;
                //Check that the address is valid.
                if (!systemBorrower.IsValidAddress)
                {
                    MessageBox.Show("Borrower's address is invalid, will insert for Ecorr.");
                }

                List<int> LSs = new List<int>();
                List<string> LPs = new List<string>();
                string removeSpaces = systemBorrower.AccountNumber;
                systemBorrower.AccountNumber = removeSpaces.Replace(" ", "");
                string activityType = "";
                string activityContact = "";
                if (borrower.System == BorrowerDemographicsFromSpecifiedSystem.AesSystem.OneLink)
                {
                    ArcData.ArcType arcType = ArcData.ArcType.OneLINK;
                    activityType = "LT";
                    activityContact = "03";


                    // Leave comment and insert record for CFLT file.
                    ArcData arcd = new ArcData(DataAccessHelper.Region.Uheaa)
                    {
                        Arc = arc,
                        AccountNumber = systemBorrower.Ssn,
                        ScriptId = ScriptId,
                        ArcTypeSelected = arcType,
                        Comment = "",
                        ProcessOn = DateTime.Now,
                        ActivityType = activityType,
                        ActivityContact = activityContact,
                        LoanSequences = LSs,
                        LoanPrograms = LPs
                    };

                    ArcAddResults result = arcd.AddArc();
                    if (!result.ArcAdded)
                    {
                        logRun.AddNotification(string.Format("Fail to add ONELink arc account: {0} (A1)", systemBorrower.AccountNumber), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }

                if (borrower.System == BorrowerDemographicsFromSpecifiedSystem.AesSystem.Compass)
                {
                    List<string> privateLoanTypes = DA.GetPrivateLoanTypes();
                    privateLoanTypes.Add("TILP");
                    bool testMode = DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live ? true : false;
                    ArcData.ArcType arcType = ArcData.ArcType.Atd22ByBalance;

                    ArcData arcc = new ArcData(DataAccessHelper.Region.Uheaa)
                    {
                        Arc = arc,
                        AccountNumber = systemBorrower.AccountNumber,
                        ScriptId = ScriptId,
                        ArcTypeSelected = arcType,
                        ActivityType = "",
                        ActivityContact = "",
                        LoanPrograms = privateLoanTypes,
                        LoanSequences = LSs
                    };

                    ArcAddResults result = arcc.AddArc();
                    if (!result.ArcAdded)
                    {
                        logRun.AddNotification(string.Format("Fail to add Compass arc account: {0} (A2)", systemBorrower.AccountNumber), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }
                string keyline = DocumentProcessing.ACSKeyLine(systemBorrower.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
                string lineData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", systemBorrower.LastName, systemBorrower.FirstName, systemBorrower.Address1, systemBorrower.Address2, systemBorrower.City, systemBorrower.State,  systemBorrower.ZipCode, systemBorrower.Country, systemBorrower.AccountNumber, keyline);
                EcorrProcessing.AddRecordToPrintProcessing(this.ScriptId, letterId, lineData, systemBorrower.AccountNumber, "MA2324");
                MessageBox.Show("The CONFLICT letter will be generated by UHECORPRT.", "Letter Scheduled.", MessageBoxButtons.OK);
                //checkForCoBorrowers(systemBorrower.Ssn, systemBorrower.AccountNumber, letterId, "MA2324");
                File.Delete(letterDataFile);
            }
        }


        /// <summary>
        /// General processing, no specific letter type
        /// </summary>
        private void GeneralProcessing()
        {
            string docFolder = EnterpriseFileSystem.GetPath("AuxServicesFolder");
            string dataFile = EnterpriseFileSystem.TempFolder + "customaDat.txt";
            string LetterId = "CUSTOMA";

            //Prompt the user for an SSN/Account Number
            for (string ssnOrAccountNumber = GetBorrowerIdFromUser(); ssnOrAccountNumber.Length > 0; ssnOrAccountNumber = GetBorrowerIdFromUser())
            {
                BorrowerDemographicsFromSpecifiedSystem borrower = GetDemographicsFromEitherSystem(ssnOrAccountNumber, BorrowerDemographicsFromSpecifiedSystem.AesSystem.OneLink);
                if (borrower == null)
                {
                    MessageBox.Show("That wasn't a valid SSN or Account Number. Please try again.");
                    //EndDllScript();
                    return;
                }
                SystemBorrowerDemographics systemBorrower = borrower.Demographics;
                //Write out a CUSTOMA data file.
                string keyline = DocumentProcessing.ACSKeyLine(systemBorrower.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
                string formattedSsn = systemBorrower.Ssn.Insert(5, "-").Insert(3, "-");
                string removeSpaces = borrower.Demographics.AccountNumber;
                borrower.Demographics.AccountNumber = removeSpaces.Replace(" ", "");
                //string lineData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", keyline, systemBorrower.AccountNumber, formattedSsn, systemBorrower.LastName, systemBorrower.FirstName, systemBorrower.Address1, systemBorrower.Address2, systemBorrower.City, systemBorrower.State, systemBorrower.Country, systemBorrower.DateOfBirth, systemBorrower.ZipCode);
                string lineData = $"{systemBorrower.AccountNumber},{systemBorrower.LastName},{systemBorrower.FirstName},{systemBorrower.Address1},{systemBorrower.Address2},{systemBorrower.City},{systemBorrower.State},{systemBorrower.ZipCode},{systemBorrower.Country},{keyline},MA2330";
                EcorrProcessing.AddRecordToPrintProcessing(this.ScriptId, LetterId, lineData, borrower.Demographics.AccountNumber, "MA2330");
                MessageBox.Show("The CUSTOMA letter will be generated by UHECORPRT.", "Letter Scheduled", MessageBoxButtons.OK);
                checkForCoBorrowers(borrower.Demographics.Ssn, borrower.Demographics.AccountNumber, LetterId, "MA2330");
            }
        }

        /// <summary>
        /// Displays form for user to input borrower ssn
        /// </summary>
        private string GetBorrowerIdFromUser()
        {
            return InputBox.ShowDialog("Enter SSN or Account Number", "SSN or Account Number").UserProvidedText;
        }

        /// <summary>
        /// Processes Notice of Satisfaction
        /// </summary>
        private void NoticeOfSatisfactionProcessing()
        {
            string docFolder = EnterpriseFileSystem.GetPath("AwgFolder");
            string dataFile = EnterpriseFileSystem.TempFolder + "cldat.txt";

            do
            {
                Satisfaction satisfaction = new Satisfaction();
                if (CalledByMauiDude) { satisfaction.BorrowerId = MauiDudeSsn; }
                using (SatisfactionDialog satisfactionDialog = new SatisfactionDialog(satisfaction))
                {
                    if (satisfactionDialog.ShowDialog() != DialogResult.OK)
                    {  //EndDllScript(); }
                        satisfactionDialog.Close();
                        return;
                    }
                }

                SystemBorrowerDemographics borrower = null;
                try
                {
                    borrower = GetDemographicsFromLP22(satisfaction.BorrowerId);
                }
                catch (DemographicException)
                {
                    MessageBox.Show("The borrower is not on OneLINK. The script will now end.");
                    EndDllScript();
                }

                string satssn = borrower.Ssn.Replace(" ", "");

                if (!SatisfactionReasonIsValid(satssn, satisfaction.Reason))
                {
                    logRun.AddNotification(string.Format("ssn: {0} reason: {1} (A3)", borrower.Ssn, satisfaction.Reason), NotificationType.ErrorReport, NotificationSeverityType.Informational);
                    return; // EndDllScript(); 
                }

                string employerId = UpdateLC67AndRetrieveEmployerId(borrower.Ssn, satisfaction.Reason);
                if (string.IsNullOrEmpty(employerId)) { break; }
                string comment = "";
                switch (satisfaction.Reason)
                {
                    case SatisfactionDialog.Reason.SmallBalance:
                        comment = string.Format("updated AWG withdrawn date to {0} and withdrawn reason to 26 for small balance, ", DateTime.Now.ToString("MMddyyyy"));
                        break;
                    case SatisfactionDialog.Reason.Bankruptcy:
                        comment = string.Format("updated AWG withdrawn date to {0} and withdrawn reason to 06 for bankruptcy, ", DateTime.Now.ToString("MMddyyyy"));
                        break;
                    case SatisfactionDialog.Reason.Other:
                        comment = string.Format("updated AWG withdrawn date to {0} and withdrawn reason to 99, ", DateTime.Now.ToString("MMddyyyy"));
                        break;
                }

                EmployerDemographics employer = new EmployerDemographics(employerId, RI);
                string varText = "After the date of this release the employer is no longer required to withhold income from the debtor's pay.";
                varText += "  The amount of the order has been paid.  No further deductions should be made against the wages of the debtor.";
                if (satisfaction.Reason == SatisfactionDialog.Reason.Bankruptcy)
                {
                    varText = "The Order of Withholding from Earnings is being released as the borrower has filed for bankruptcy and UHEAA is required to comply with the bankruptcy automatic stay.";
                    varText += "  You may be contacted to resume the wage garnishment when the bankruptcy has ended.";
                }
                if (employer.Zip.Length > 5) { employer.Zip = string.Format("{0}-{1}", employer.Zip.Substring(0, 5), employer.Zip.Substring(5)); }

                string fullLastName = string.Format("{0} {1}", borrower.FirstName, borrower.LastName);
                string obfuscatedSsn = "XXX-XX-" + borrower.Ssn.Substring(5);
                string keyline = DocumentProcessing.ACSKeyLine(borrower.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
                comment += string.Format("mailed NOTSAT to {0} ({1})", employer.Name, employerId);
                string removeSpaces = borrower.AccountNumber;
                borrower.AccountNumber = removeSpaces.Replace(" ", "");

                string lineData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", borrower.AccountNumber, obfuscatedSsn, keyline, fullLastName, employer.Name, employer.Address1, employer.Address2, employer.City, employer.State, employer.Zip, varText);
                EcorrProcessing.AddRecordToPrintProcessing(this.ScriptId, "NOTSAT", lineData, borrower.AccountNumber, "MA2329");

                List<int> LSs = new List<int>();
                List<string> LPs = new List<string>();
                ArcData arcd = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    Arc = "LPAWG",
                    AccountNumber = borrower.Ssn,
                    ScriptId = ScriptId,
                    ArcTypeSelected = ArcData.ArcType.OneLINK,
                    Comment = comment,
                    ProcessOn = DateTime.Now,
                    ActivityType = "LT",
                    ActivityContact = "81",
                    LoanPrograms = LPs,
                    LoanSequences = LSs
                };

                ArcAddResults result = arcd.AddArc();
                if (!result.ArcAdded)
                {
                    logRun.AddNotification(string.Format("Fail to add ONELink arc. Account: {0} (A4)", borrower.AccountNumber), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }

                MessageBox.Show("The NOTSAT letter will be generated by UHECORPRT.", "Letter Scheduled", MessageBoxButtons.OK);
                //checkForCoBorrowers(borrower.Ssn, borrower.AccountNumber, "NOSAT", "MA2329");
                File.Delete(dataFile);
            } while (!CalledByMauiDude);
        }

        /// <summary>
        /// Checks to see if the satisfaction reason is valid
        /// </summary>
        private bool SatisfactionReasonIsValid(string ssn, SatisfactionDialog.Reason satisfactionReason)
        {
            switch (satisfactionReason)
            {
                case SatisfactionDialog.Reason.Bankruptcy:
                    //Check that there's at least one loan in bankruptcy.
                    RI.FastPath("LC05I" + ssn);
                    if (RI.CheckForText(22, 3, "47004"))
                    {
                        string message = "The current letter cannot be printed as the borrower has no loans on LC05.";
                        string caption = "No Defaulted Loans";
                        MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    //Page through each loan on LC05 to see if there's one in bankruptcy status ("07").
                    bool foundBankruptcy = false;
                    RI.PutText(21, 13, "1", ReflectionInterface.Key.Enter);
                    while (!RI.CheckForText(22, 3, "46004"))
                    {
                        if (RI.CheckForText(4, 10, "07"))
                        {
                            foundBankruptcy = true;
                            break;
                        }
                        RI.Hit(ReflectionInterface.Key.F8); //Next loan
                    }
                    if (!foundBankruptcy)
                    {
                        string message = "A bankruptcy Notice of Satisfaction cannot be generated as the borrower does not have any loans in bankruptcy.";
                        string caption = "Invalid Condition for Letter";
                        MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    break;
                case SatisfactionDialog.Reason.SmallBalance:
                    //Check that the borrower has no more than $25 principal + interest
                    //and no more than $100 other costs on LC10.
                    RI.FastPath("LC10I" + ssn);
                    if (RI.CheckForText(22, 3, "47004"))
                    {
                        string message = "The current letter cannot be printed as the borrower has no information displayed on LC10.";
                        string caption = "No LC10 Display";
                        MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    double principalAndInterest = double.Parse(RI.GetText(8, 71, 10).Replace(",", "")) + double.Parse(RI.GetText(9, 71, 10).Replace(",", ""));
                    double otherCosts = double.Parse(RI.GetText(10, 71, 10).Replace(",", "")) + double.Parse(RI.GetText(11, 71, 10).Replace(",", "")) + double.Parse(RI.GetText(12, 71, 10).Replace(",", ""));
                    if (principalAndInterest > 25 || otherCosts > 100)
                    {
                        string message = "A small balance/closed account Notice of Satisfaction cannot be generated as principal and interest is greater than $25 or other costs are greater than $100.";
                        string caption = "Invalid Condition for Letter";
                        MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// Updates LC67 and gets the employee ID
        /// </summary>
        private string UpdateLC67AndRetrieveEmployerId(string ssn, SatisfactionDialog.Reason satisfactionReason)
        {
            RI.FastPath("LC67C" + ssn + "GG");
            if (RI.CheckForText(22, 3, "47004", "00008"))
            {
                MessageBox.Show("No LC67 Record Found or Incorrect SSN");
                return null;
            }
            if (RI.CheckForText(21, 3, "SEL"))
            {
                //Prompt the user to select the record.
                while (!RI.CheckForText(1, 70, "AWG"))
                {
                    string message = "Select the correct record and RI.Hit Insert to continue.";
                    string caption = "Select Record";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK);
                    RI.PauseForInsert();
                }
            }
            string employerId = RI.GetText(9, 45, 8);
            if (satisfactionReason == SatisfactionDialog.Reason.EmployerRequest && RI.CheckForText(8, 19, "__") && RI.CheckForText(16, 63, "__"))
            {
                string message = "A Notice of Satisfaction cannot be re-sent at the employer's request as the AWG record is still open, indicating a notice has not been sent to the employer.";
                string caption = "Invalid Condition for Letter";
                MessageBox.Show(message, caption, MessageBoxButtons.OK);
                return null;
            }
            else if (RI.CheckForText(15, 71, "MMDDCCYY") && RI.CheckForText(7, 35, "MMDDCCYY"))
            {
                //Update inactive date and reason.
                RI.PutText(7, 35, DateTime.Now.ToString("MMddyyyy"));
                switch (satisfactionReason)
                {
                    case SatisfactionDialog.Reason.SmallBalance:
                        RI.PutText(8, 19, "26");
                        break;
                    case SatisfactionDialog.Reason.Bankruptcy:
                        RI.PutText(8, 19, "06");
                        break;
                    case SatisfactionDialog.Reason.Other:
                        RI.PutText(8, 19, "99");
                        break;
                }
                if (satisfactionReason == SatisfactionDialog.Reason.Bankruptcy || satisfactionReason == SatisfactionDialog.Reason.Other)
                {
                    //Update the withdrawn LTR field.
                    RI.PutText(6, 19, "N");
                    //Blank the hold and inactive dates.
                    if (!RI.CheckForText(15, 71, "MM")) { RI.PutText(15, 71, "**"); }
                    if (!RI.CheckForText(17, 71, "MM")) { RI.PutText(17, 71, "**"); }
                }
                RI.Hit(ReflectionInterface.Key.Enter);
            }
            return employerId;
        }

        /// <summary>
        /// Processes Post Office letter
        /// </summary>
        private void PostOfficeProcessing()
        {
            string docPath = EnterpriseFileSystem.GetPath("CollectionsFolder");
            string dataFile = EnterpriseFileSystem.TempFolder + "cldat.txt";

            while (true)
            {
                PostOfficeBorrower postOfficeBorrower = new PostOfficeBorrower();
                using (PostOfficeDialog postOfficeDialog = new PostOfficeDialog(postOfficeBorrower, DA))
                {
                    if (postOfficeDialog.ShowDialog() != DialogResult.OK) { return; } //break; }
                }

                BorrowerDemographicsFromSpecifiedSystem borrower = GetDemographicsFromEitherSystem(postOfficeBorrower.AccountNumber, BorrowerDemographicsFromSpecifiedSystem.AesSystem.OneLink);
                if (borrower == null)
                {
                    MessageBox.Show("That wasn't a valid SSN or Account Number. Please try again.");
                    return; // EndDllScript();
                }
                SystemBorrowerDemographics systemBorrower = borrower.Demographics;
                string removeSpaces = systemBorrower.AccountNumber;
                systemBorrower.AccountNumber = removeSpaces.Replace(" ", "");
                string lineData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", systemBorrower.AccountNumber, systemBorrower.LastName, systemBorrower.FirstName, systemBorrower.Address1, systemBorrower.Address2, systemBorrower.City, systemBorrower.State, systemBorrower.Country, systemBorrower.ZipCode, postOfficeBorrower.City, postOfficeBorrower.State, postOfficeBorrower.Zip);
                EcorrProcessing.AddRecordToPrintProcessing(this.ScriptId, "POFFLOC", lineData, systemBorrower.AccountNumber, "MA2324");
                MessageBox.Show("The POFFLOC letter will be generated by UHECORPRT.", "Letter Scheduled", MessageBoxButtons.OK);
                //checkForCoBorrowers(systemBorrower.Ssn, systemBorrower.AccountNumber, "POFFLOC", "MA2324");
            }
        }

        /// <summary>
        /// Processes Property Owner letter
        /// </summary>
        private void PropertyOwnerProcessing()
        {
            string docPath = EnterpriseFileSystem.GetPath("CollectionsFolder");
            string dataFile = EnterpriseFileSystem.TempFolder + "cldat.txt";

            while (true)
            {
                PropertyOwner propertyOwner = new PropertyOwner();
                using (PropertyOwnerDialog propertyOwnerDialog = new PropertyOwnerDialog(propertyOwner, DA))
                {
                    if (propertyOwnerDialog.ShowDialog() != DialogResult.OK) { break; }
                    propertyOwnerDialog.Close();
                }

                BorrowerDemographicsFromSpecifiedSystem borrower = GetDemographicsFromEitherSystem(propertyOwner.BorrowerSsnOrAccountNumber, BorrowerDemographicsFromSpecifiedSystem.AesSystem.OneLink);
                if (borrower == null)
                {
                    MessageBox.Show("That wasn't a valid SSN or Account Number. Please try again.");
                    return; // EndDllScript();
                }
                SystemBorrowerDemographics systemBorrower = borrower.Demographics;
                string formattedSsn = systemBorrower.Ssn.Insert(5, "-").Insert(3, "-");
                string removeSpaces = systemBorrower.AccountNumber;
                systemBorrower.AccountNumber = removeSpaces.Replace(" ", "");
                string lineData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", systemBorrower.AccountNumber, formattedSsn, systemBorrower.LastName, systemBorrower.FirstName, systemBorrower.Address1, systemBorrower.Address2, systemBorrower.City, systemBorrower.State, systemBorrower.Country, systemBorrower.ZipCode, propertyOwner.Name, propertyOwner.Address1, propertyOwner.Address2, propertyOwner.City, propertyOwner.State, propertyOwner.Zip);
                EcorrProcessing.AddRecordToPrintProcessing(this.ScriptId, "PROP", lineData, systemBorrower.AccountNumber, "MA2324");
                MessageBox.Show("The PROP letter will be generated by UHECORPRT.", "Letter Scheduled", MessageBoxButtons.OK);
                //checkForCoBorrowers(systemBorrower.Ssn, systemBorrower.AccountNumber, "PROP", "MA2324");

            }
        }

        /// <summary>
        /// Processes Request for Hearing letter
        /// </summary>
        private void RequestForHearingProcessing()
        {
            string docFolder = EnterpriseFileSystem.GetPath("AwgFolder");
            string dataFile = EnterpriseFileSystem.TempFolder + "cldat.txt";

            do
            {
                //Get the SSN/AcctNum from the user if it wasn't provided by MauiDUDE.
                string ssnOrAccountNumber = (CalledByMauiDude ? MauiDudeSsn : GetBorrowerIdFromUser());
                if (string.IsNullOrEmpty(ssnOrAccountNumber)) { break; }
                SystemBorrowerDemographics borrower = null;
                try
                {
                    borrower = GetDemographicsFromLP22(ssnOrAccountNumber);
                }
                catch (DemographicException)
                {
                    MessageBox.Show("The borrower is not on OneLINK.");
                    return; // EndDllScript();
                }
                if (!borrower.IsValidAddress)
                {
                    MessageBox.Show("The address is invalid. Unable to send letter.");
                    return;
                }
                string keyline = DocumentProcessing.ACSKeyLine(borrower.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);

                List<int> LSs = new List<int>();
                List<string> LPs = new List<string>();

                ArcData arcd = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    Arc = "LGARR",
                    AccountNumber = borrower.Ssn,
                    ScriptId = ScriptId,
                    ArcTypeSelected = ArcData.ArcType.OneLINK,
                    Comment = "mailed hearing request to borrower",
                    ProcessOn = DateTime.Now,
                    ActivityType = "LT",
                    ActivityContact = "03",
                    LoanPrograms = LPs,
                    LoanSequences = LSs
                };

                ArcAddResults result = arcd.AddArc();
                if (!result.ArcAdded)
                {
                    logRun.AddNotification(string.Format("Fail to add ONELink arc account: {0} (A5)", borrower.AccountNumber), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }

                // RI.AddCommentInLP50(borrower.Ssn, "LT", "03", "LGARR", "mailed hearing request to borrower", ScriptId);
                string removeSpaces = borrower.AccountNumber;
                borrower.AccountNumber = removeSpaces.Replace(" ", "");
                string lineData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", borrower.AccountNumber, keyline, borrower.FirstName, borrower.LastName, borrower.Address1, borrower.Address2, borrower.City, borrower.State, borrower.ZipCode, borrower.Country);
                EcorrProcessing.AddRecordToPrintProcessing(this.ScriptId, "REQHRG", lineData, borrower.AccountNumber, "MA2329");
                MessageBox.Show("The REQHRG letter will be generated by UHECORPRT.", "Letter Scheduled", MessageBoxButtons.OK);
                //checkForCoBorrowers(borrower.Ssn, borrower.AccountNumber, "REQHRG", "MA2329");
            } while (!CalledByMauiDude);
        }

        /// <summary>
        /// Processes NSLDS letter
        /// </summary>
        private void NsldsSsnProcessing(string arc, string letterId)
        {
            string docPath = EnterpriseFileSystem.GetPath("DbqFolder");
            string dataFile = EnterpriseFileSystem.TempFolder + "SsnDat.txt";

            InputBoxResults ssnResults = InputBox.ShowDialog("Enter SSN or Account Number", "SSN or Account Number");
            if (ssnResults.Result != DialogResult.OK) { return; } // EndDllScript(); }
            string ssn = ssnResults.UserProvidedText;
            SystemBorrowerDemographics borrower = null;
            while (borrower == null)
            {
                if (ssn.Length == 0) { EndDllScript(); }
                try
                {
                    borrower = GetDemographicsFromLP22(ssn);
                }
                catch (DemographicException)
                {
                    ssnResults = InputBox.ShowDialog("That SSN or Account Number was not found. Please try again.", "SSN or Account Number");
                    if (ssnResults.Result != DialogResult.OK) { return; } // EndDllScript(); }
                    ssn = ssnResults.UserProvidedText;
                    borrower = null;
                }
            }
            //Check that the address is valid.
            if (!borrower.IsValidAddress)
            {
                MessageBox.Show("Borrower's address is invalid.");
                return; // EndDllScript();
            }
            if (borrower.State == "FC") { borrower.State = ""; }
            string replacement = borrower.AccountNumber.Replace(" ", "");

            // Apparently Replace() doesn't work on the object property, 
            borrower.AccountNumber = replacement;
            List<int> LSs = new List<int>();
            List<string> LPs = new List<string>();

            ArcData arcd = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                Arc = arc,
                AccountNumber = borrower.Ssn,
                ScriptId = ScriptId,
                ArcTypeSelected = ArcData.ArcType.OneLINK ,
                Comment = "",
                ProcessOn = DateTime.Now,
                ActivityType = "LT",
                ActivityContact = "10",
                LoanPrograms = LPs,
                LoanSequences = LSs
            };

            ArcAddResults result = arcd.AddArc();
            if (!result.ArcAdded)
            {
                logRun.AddNotification(string.Format("Fail to add ONELink arc account: {0} (A6)", borrower.AccountNumber), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            string keyline = DocumentProcessing.ACSKeyLine(borrower.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string lineData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", borrower.LastName, borrower.FirstName, borrower.Address1, borrower.Address2, borrower.City, borrower.State,  borrower.ZipCode, borrower.Country, keyline, borrower.AccountNumber);
            EcorrProcessing.AddRecordToPrintProcessing(this.ScriptId, letterId, lineData, borrower.AccountNumber, "MA2230");
            MessageBox.Show("The SSN# letter will be generated by UHECORPRT.", "Letter Scheduled", MessageBoxButtons.OK);
            //checkForCoBorrowers(borrower.Ssn, borrower.AccountNumber, letterId, "MA2330");
        }

        /// <summary>
        /// Gets the borrower demographics from both Compass and OneLink
        /// </summary>
        private BorrowerDemographicsFromSpecifiedSystem GetDemographicsFromEitherSystem(string ssnOrAccountNumber, BorrowerDemographicsFromSpecifiedSystem.AesSystem systemToTryFirst)
        {
            try
            {
                SystemBorrowerDemographics demographics = GetDemographicsFromLP22(ssnOrAccountNumber);
                return new BorrowerDemographicsFromSpecifiedSystem() { Demographics = demographics, System = BorrowerDemographicsFromSpecifiedSystem.AesSystem.OneLink };
            }
            catch (DemographicException)
            {
                try
                {
                    SystemBorrowerDemographics demographics = GetDemographicsFromTx1j(ssnOrAccountNumber);
                    return new BorrowerDemographicsFromSpecifiedSystem() { Demographics = demographics, System = BorrowerDemographicsFromSpecifiedSystem.AesSystem.Compass };
                }
                catch (DemographicException)
                {
                    return null;
                }
            }
        }

        private void checkForCoBorrowers(string borrowerSsn, string borrowerAcct, string currentLetter, string costCenter)
        {
            coBorrowers.Clear();
            coBorrowers = DA.GetCoBorrowers(borrowerSsn);
            if (coBorrowers.Count == 0)
                return;

            foreach (CoBorrowers coborrower in coBorrowers)
            {
                string keyline = DocumentProcessing.ACSKeyLine(coborrower.CoBorSsn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
                string lineData = $"{keyline},{borrowerAcct},{coborrower.LastName.TrimRight(" ")},{coborrower.FirstName.TrimRight(" ")},{coborrower.Address1.TrimRight(" ")},{coborrower.Address2.TrimRight(" ")},{coborrower.City.TrimRight(" ")},{coborrower.State.TrimRight(" ")},{coborrower.Zip.TrimRight(" ")},{coborrower.ForCountry.TrimRight(" ")}";
                EcorrProcessing.AddCoBwrRecordToPrintProcessing(DA.ScriptId, currentLetter, lineData, coborrower.AccountNumber, costCenter, borrowerSsn);
            }

            MessageBox.Show("Coborrower(s) found, letter(s) will be generated by UHECORPRT.", "Letter Scheduled", MessageBoxButtons.OK);

        }


    }
}