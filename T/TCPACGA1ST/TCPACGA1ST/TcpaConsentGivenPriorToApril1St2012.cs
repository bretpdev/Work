using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;


namespace TCPACGA1ST
{
    public class TcpaConsentGivenPriorToApril1St2012 : BatchScript
    {
        private const string SasFilePattern = "ULWQ37.LWQ37R*";
        private const string EojProcessed = "Total number of records processed:";
        private const string EojTotalRecords = "Total number of records in the file:";
        private const string EojErrors = "Total number of errors:";
        private static string[] EojFields = { EojProcessed, EojErrors, EojTotalRecords };

        private enum ScriptResults
        {
            MConsentN,
            ConsentY,
            UConsentN,
            Error,
            None
        }

        private enum CheckTd22Results
        {
            HasLoans,
            GotEndorser,
            None
        }

        public TcpaConsentGivenPriorToApril1St2012(ReflectionInterface ri)
            : base(ri, "TCPACGA1ST", "ERR_BU35", "TCPA_EOJ", EojFields, DataAccessHelper.Region.Uheaa)
        {

        }

        public override void Main()
        {
            StartupMessage("This is the TCPA Consent Given Prior to April 1st, 2012. Click OK to continue, or Cancel to quit.");

            List<string> filesToProcess = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, SasFilePattern).ToList();

            if (filesToProcess.Count < 1)
                NotifyAndEnd("Sas Files are missing.  Please investigate and try again");

            CheckArcsAccess(new List<string>() { "APRLB", "APRLE", "APRLS", "APRLR", "APRIL" });

            foreach (string file in filesToProcess)
            {
                List<string> fileData = new List<string>();

                using (StreamReader sr = new StreamReader(file))
                {
                    sr.ReadLine();//Read in the Header
                    while (!sr.EndOfStream)
                    {
                        fileData.Add(sr.ReadLine());
                        if (!Recovery.RecoveryValue.IsNullOrEmpty())
                            Eoj.Counts[EojTotalRecords].Increment();
                    }
                }

                if (fileData.Count < 1)
                {
                    MessageBox.Show(string.Format("The SAS file {0} was empty.", file));
                    File.Delete(file);
                    continue;
                }

                int recoveryCounter = Recovery.RecoveryValue.IsNullOrEmpty() ? 0 : int.Parse(Recovery.RecoveryValue);

                foreach (string accountNumber in fileData.Skip(recoveryCounter))
                {
                    if (ProcessTheAccount(accountNumber, file.Substring(file.LastIndexOf(@"\"))))
                        Eoj.Counts[EojProcessed].Increment();

                    Recovery.RecoveryValue = (++recoveryCounter).ToString();
                }

                File.Delete(file);
                Recovery.Delete();//Clean up recovery for the next file.
            }

            Eoj.Publish();
            ProcessingComplete();
        }

        private void CheckArcsAccess(List<string> arcs)
        {
            string noAccessArcs = string.Empty;
            foreach (string arc in arcs)
            {
                FastPath("TX3Z/ITX68");
                PutText(8, 41, UserId, true);
                PutText(10, 41, arc, ReflectionInterface.Key.Enter, true);

                if (!CheckForText(1, 72, "TXX6C"))
                    noAccessArcs += arc + ",";
            }

            if (!noAccessArcs.IsNullOrEmpty())
                NotifyAndEnd("You do not have access to the following ARCS: {0}", noAccessArcs.Remove(noAccessArcs.Length - 1));
        }

        private bool ProcessTheAccount(string accountNumber, string file)
        {
            if (accountNumber.SafeSubString(0, 3).Contains("RF@"))
                return OnelinkReference(accountNumber);

            FastPath("TX3Z/CTX1J");
            PutText(5, 16, "", true);
            PutText(6, 16, "", true);
            PutText(6, 20, "", true);
            PutText(6, 23, "", true);

            if (accountNumber.Length == 10)
                PutText(6, 61, accountNumber, ReflectionInterface.Key.Enter);

            else
                PutText(6, 16, accountNumber, ReflectionInterface.Key.Enter);


            if (!CheckForText(1, 71, "TXX1R"))
            {
                Err.AddRecord("Unable to find the following account: ", new { AccountNumber = accountNumber });
                return false;
            }

            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);

            string phoneType = GetText(16, 14, 1);
            string personType = GetText(1, 9, 1);
            string borrowerSsn = string.Empty;
            string personSsn = string.Empty;//this will the the Endorsers or students Ssn

            if (personType.Contains("R"))
            {
                //Get the Ssn for the borrower this will be used when adding a comment to TD22
                borrowerSsn = GetText(7, 11, 11).Replace(" ", "");
            }
            else if (personType.Contains("S") || personType.Contains("E"))
            {
                //Get the borrowers Ssn and the Endorser's Ssn, this will be used when adding a comment to TD22
                borrowerSsn = GetText(7, 11, 11).Replace(" ", "");
                personSsn = GetText(3, 12, 11).Replace(" ", "");
            }
            else
            {
                //Get the borrowers Ssn
                borrowerSsn = GetText(3, 12, 11).Replace(" ", "");
            }

            Dictionary<ScriptResults, string> results = new Dictionary<ScriptResults, string>();

            while (true)
            {
                string mbl = GetText(16, 20, 1);
                string consent = GetText(16, 30, 1);

                ScriptResults sResults = new ScriptResults();

                if (mbl.Contains("M"))
                {
                    DateTime phoneLastVerified = DateTime.Parse(GetText(16, 45, 8).Replace(" ", "/"));

                    //Process accounts where the verified date is before 04/01/2012
                    if ((phoneLastVerified < new DateTime(2012, 04, 01) && file.Contains("R2")) || personType.Contains("R"))
                        sResults = MblAndConsentProcessing(mbl, consent, borrowerSsn, accountNumber);
                    else
                        sResults = ScriptResults.None;
                }
                else
                    sResults = MblAndConsentProcessing(mbl, consent, borrowerSsn, accountNumber);

                if (!results.Keys.Contains(sResults))
                    results.Add(sResults, phoneType);
                else
                    results[sResults] = results.Where(p => p.Key == sResults).Select(q => q.Value).FirstOrDefault() + "," + phoneType;

                if (phoneType.Contains("H"))
                {
                    PutText(16, 14, "A", ReflectionInterface.Key.Enter);
                    phoneType = "A";
                }
                else if (phoneType.Contains("A"))
                {
                    PutText(16, 14, "W", ReflectionInterface.Key.Enter);
                    phoneType = "W";
                }
                else if (phoneType.Contains("W"))
                {
                    PutText(16, 14, "M", ReflectionInterface.Key.Enter);
                    phoneType = "M";
                }
                else if (phoneType.Contains("M"))
                {
                    phoneType = string.Empty;
                    break;
                }
            }//end while

            if (personType.Contains("B") || personType.Contains("E"))
            {
                CheckTd22Results result = CheckTd22(accountNumber);
                switch (result)
                {
                    case CheckTd22Results.GotEndorser:
                        borrowerSsn = GetText(7, 11, 11).Replace(" ", "");
                        personType = "E";
                        personSsn = GetText(3, 12, 11).Replace(" ", "");
                        break;
                    case CheckTd22Results.None:
                        Err.AddRecord("Unable to leave comment in the following account: ", new { AccountNumber = accountNumber });
                        Eoj.Counts[EojErrors].Increment();
                        return true;
                }
            }

            //Go through Each Key in the dictionary and see 
            bool hadErrors = false;
            string comment = string.Empty;
            foreach (KeyValuePair<ScriptResults, string> item in results)
            {
                bool commentAddedCompass = false;
                string dateObtainedConsent = "April 1st, 2012";

                if (personType.Contains("B"))
                {
                    switch (item.Key)
                    {
                        case ScriptResults.MConsentN:
                            comment = string.Format("Changed CONSENT field to N for phone type(s) {0} because UHEAA obtained consent prior to {1}", item.Value, dateObtainedConsent);
                            break;
                        case ScriptResults.ConsentY:
                            comment = string.Format("Changed CONSENT field to Y for phone type(s) {0} because phone is a landline", item.Value);
                            break;
                        case ScriptResults.UConsentN:
                            comment = string.Format("Changed CONSENT field to N for phone type(s) {0} because phone number was unidentified", item.Value);
                            break;
                        case ScriptResults.Error:
                            hadErrors = true;
                            continue;
                        case ScriptResults.None:
                            continue;
                    }

                    commentAddedCompass = Atd22AllLoans(borrowerSsn, "APRLB", comment, borrowerSsn, ScriptId, false);

                }
                else if (personType.Contains("E"))
                {
                    switch (item.Key)
                    {
                        case ScriptResults.MConsentN:
                            comment = string.Format("Changed CONSENT field to N for endorser’s {0} phone type(s) {1} because UHEAA obtained consent prior to {2}", accountNumber, item.Value, dateObtainedConsent);
                            break;
                        case ScriptResults.ConsentY:
                            comment = string.Format("Changed CONSENT field to Y for endorser’s {0} phone type(s) {1} because phone is a landline", accountNumber, item.Value);
                            break;
                        case ScriptResults.UConsentN:
                            comment = string.Format("Changed CONSENT field to N for endorser’s {0} phone type(s) {1} because phone number was unidentified", accountNumber, item.Value);
                            break;
                        case ScriptResults.Error:
                            hadErrors = true;
                            continue;
                        case ScriptResults.None:
                            continue;
                    }

                    List<int> loanSeqs = GetEndorsersLoanSeqences(personSsn, borrowerSsn);

                    commentAddedCompass = Atd22ByLoan(borrowerSsn, "APRLE", comment, personSsn, loanSeqs, ScriptId, false);

                }
                else if (personType.Contains("R"))
                {
                    switch (item.Key)
                    {
                        case ScriptResults.MConsentN:
                            comment = string.Format("Changed CONSENT field to N for reference’s {0} phone type(s) {1} because UHEAA had not obtained consent", accountNumber, item.Value);
                            break;
                        case ScriptResults.ConsentY:
                            comment = string.Format("Changed CONSENT field to Y for reference’s {0} phone type(s) {1} because phone is a landline", accountNumber, item.Value);
                            break;
                        case ScriptResults.UConsentN:
                            comment = string.Format("Changed CONSENT field to N for reference’s {0} phone type(s) {1} because phone number was unidentified", accountNumber, item.Value);
                            break;
                        case ScriptResults.Error:
                            hadErrors = true;
                            continue;
                        case ScriptResults.None:
                            continue;
                    }

                    commentAddedCompass = Atd22AllLoans(borrowerSsn, "APRLR", comment, accountNumber, ScriptId, false);
                }
                else
                {
                    switch (item.Key)
                    {
                        case ScriptResults.MConsentN:
                            comment = string.Format("Changed CONSENT field to N for student’s {0} phone type(s) {1} because UHEAA obtained consent prior to {2}", accountNumber, item.Value, dateObtainedConsent);
                            break;
                        case ScriptResults.ConsentY:
                            comment = string.Format("Changed CONSENT field to Y for student’s {0} phone type(s) {1} because phone is a landline", accountNumber, item.Value);
                            break;
                        case ScriptResults.UConsentN:
                            comment = string.Format("Changed CONSENT field to N for student’s {0} phone type(s) {1} because phone number was unidentified", accountNumber, item.Value);
                            break;
                        case ScriptResults.Error:
                            hadErrors = true;
                            continue;
                        case ScriptResults.None:
                            continue;
                    }

                    commentAddedCompass = Atd22AllLoans(borrowerSsn, "APRLS", comment, personSsn, ScriptId, false);
                }

                //If the comment was not added successfully then add a record to the error report.
                if (!commentAddedCompass)
                {
                    Err.AddRecord("Unable to leave comment in the following account: ", new { AccountNumber = accountNumber });
                    Eoj.Counts[EojErrors].Increment();
                    return false;
                }
            }

            if (hadErrors)
                return false;
            else
                return true;
        }

        private bool OnelinkReference(string referenceId)
        {
            FastPath(string.Format("LP2CC;{0};", referenceId));
            if (!CheckForText(1, 59, "REFERENCE DEMOGRAPHICS"))
                return false;

            DateTime phoneEffect = CheckForText(15, 71, "MMDDCCYY") ? new DateTime() : DateTime.Parse(GetText(15, 71, 8).Insert(2, "/").Insert(5, "/"));
            bool changedPrimary = false;
            bool changedAlt = false;

            if (CheckForText(13, 36, "Y") && CheckForText(13, 40, "M") && CheckForText(13, 46, "Y"))
            {
                if (phoneEffect < new DateTime(2012, 08, 01))
                {
                    PutText(13, 46, "N");
                    changedPrimary = true;
                }
            }

            if (CheckForText(14, 36, "Y") && CheckForText(14, 40, "M") && CheckForText(14, 46, "Y"))
            {
                if (phoneEffect < new DateTime(2012, 08, 01))
                {
                    PutText(14, 46, "N");
                    changedAlt = true;
                }
            }

            if (changedAlt || changedPrimary)
                PutText(15, 71, DateTime.Now.ToString("MMddyyyy"));

            Hit(ReflectionInterface.Key.F6);

            if (CheckForText(22, 3, "46010"))
                return true;

            string comment = string.Empty;

            if (changedPrimary && changedAlt)
                comment = string.Format("Changed CONSENT field to N for primary and alternate phone number because UHEAA obtained consent prior to {0:MMddyyyy}", new DateTime(2012, 08, 01).ToString("MM/dd/yyyy"));
            else if (changedAlt)
                comment = string.Format("Changed CONSENT field to N for alternate phone number because UHEAA obtained consent prior to {0:MMddyyyy}", new DateTime(2012, 08, 01).ToString("MM/dd/yyyy"));
            else
                comment = string.Format("Changed CONSENT field to N for primary phone number because UHEAA obtained consent prior to {0:MMddyyyy}", new DateTime(2012, 08, 01).ToString("MM/dd/yyyy"));

            if (!AddCommentInLP50(referenceId, "AM", "10", "MPRUL", comment, ScriptId))
            {
                AddQueueTaskInLP9O(referenceId, "APRIL", null, string.Format("{0}, Unable to save activity comment in OneLINK", referenceId), "", "", "");
                return false;
            }

            return true;

        }

        private ScriptResults MblAndConsentProcessing(string mbl, string consent, string bwrSsn, string accountNumber)
        {
            //If the phone number is not valid do nothing and return.
            if (!CheckForText(17, 54, "Y"))
                return ScriptResults.None;

            //Update Moblie phone number with a Consent of Y to a consent of N
            if (mbl.Contains("M") && consent.Contains("Y"))
            {
                PutText(16, 20, "M");
                PutText(16, 30, "N");
                PutText(17, 54, "Y");
                PutText(16, 45, DateTime.Now.ToString("MMddyy"));
                PutText(19, 14, "32", ReflectionInterface.Key.Enter);

                //If account was not updated add to error report and return nothing
                if (!CheckForText(23, 2, "01097"))
                {
                    Atd22AllLoans(bwrSsn, "APRIL", string.Format("Unable to save mobile indicator changes for account {0} for phone type {1}", accountNumber, GetText(16, 14, 1)), string.Empty, ScriptId, false);
                    Eoj.Counts[EojErrors].Increment();
                    ResetTx1j(accountNumber);
                    return ScriptResults.None;
                }

                return ScriptResults.MConsentN;
            }
            else if (mbl.Contains("L") && consent.Contains("N"))//Update any landline to Y
            {
                PutText(16, 20, "L");
                PutText(16, 30, "Y");
                PutText(17, 54, "Y");
                PutText(16, 45, DateTime.Now.ToString("MMddyy"));
                PutText(19, 14, "32", ReflectionInterface.Key.Enter);

                //If account was not updated add to error report and return nothing
                if (!CheckForText(23, 2, "01097"))
                {
                    Atd22AllLoans(bwrSsn, "APRIL", string.Format("Unable to save mobile indicator changes for account {0}", accountNumber), string.Empty, ScriptId, false);
                    Eoj.Counts[EojErrors].Increment();
                    ResetTx1j(accountNumber);
                    return ScriptResults.Error;
                }

                return ScriptResults.ConsentY;
            }
            else if (mbl.Contains("U") && consent.Contains("Y"))//Update any unknown number to N
            {
                PutText(16, 20, "U");
                PutText(16, 30, "N");
                PutText(17, 54, "Y");
                PutText(16, 45, DateTime.Now.ToString("MMddyy"));
                PutText(19, 14, "32", ReflectionInterface.Key.Enter);

                //If account was not updated add to error report and return nothing
                if (!CheckForText(23, 2, "01097"))
                {
                    Atd22AllLoans(bwrSsn, "APRIL", string.Format("Unable to save mobile indicator changes for account {0}", accountNumber), string.Empty, ScriptId, false);
                    Eoj.Counts[EojErrors].Increment();
                    ResetTx1j(accountNumber);
                    return ScriptResults.Error;
                }

                return ScriptResults.UConsentN;
            }

            //Return nothing if none of the above criteria is met.
            return ScriptResults.None;
        }

        private CheckTd22Results CheckTd22(string accountNumber)
        {
            bool checkEnd = false;
            while (true)
            {
                string arcToUse = checkEnd ? "APRLE" : "APRLB";
                string personTypeToCheck = checkEnd ? "S" : "E";
                FastPath(string.Format("TX3Z/ATD22{0};{1}", accountNumber, arcToUse));

                if (!CheckForText(1, 72, "TDX24"))
                {
                    FastPath("TX3Z/ITX1J");
                    PutText(5, 16, personTypeToCheck, true);
                    PutText(6, 16, "", true);
                    PutText(6, 20, "", true);
                    PutText(6, 23, "", true);
                    PutText(6, 61, accountNumber, ReflectionInterface.Key.Enter);

                    if (CheckForText(1, 71, "TXX1R"))
                        return CheckTd22Results.GotEndorser;
                }
                else
                    return CheckTd22Results.HasLoans;

                checkEnd = true;

                if (personTypeToCheck.Contains("S"))
                    break;
            }

            return CheckTd22Results.None;
        }

        private void ResetTx1j(string accountNumber)
        {
            //Get back to TX1j
            FastPath("TX3Z/CTX1J");
            PutText(5, 16, "", true);
            PutText(6, 16, "", true);
            PutText(6, 20, "", true);
            PutText(6, 23, "", true);

            if (accountNumber.Length == 10)
                PutText(6, 61, accountNumber, ReflectionInterface.Key.Enter);

            else
                PutText(6, 16, accountNumber, ReflectionInterface.Key.Enter);

            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);
        }

        private bool AddCommentToOnelink(string ssn, string comment)
        {
            return AddCommentInLP50(ssn, "AM", "10", "MPBUL", comment, ScriptId);
        }
    }
}
