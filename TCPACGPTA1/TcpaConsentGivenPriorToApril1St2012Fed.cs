using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using System.Windows.Forms;
using Efs = Uheaa.Common.DataAccess.EnterpriseFileSystem;


namespace TCPACGPTA1
{
    public class TcpaConsentGivenPriorToApril1St2012Fed : FedBatchScript
    {
        private const string EojTotalFromSas = "Total number of records in the SAS file";
        private const string EojProcessed = "Number of records successfully processed";
        private const string EojErrorQueue = "Number of records sent to error queue";
        private const string EojErrorReport = "Number of records sent to error report";
        private const string FilePattern = "UNWQ03.NWQ03R*";
        private static readonly string[] EOJ_FIELDS = { EojTotalFromSas, EojProcessed, EojErrorQueue, EojErrorReport };

        private enum ScriptResults
        {
            MConsentN,
            ConsentY,
            UConsentN,
            None
        }

        private enum CheckTd22Results
        {
            HasLoans,
            GotEndorser,
            None
        }

        public TcpaConsentGivenPriorToApril1St2012Fed(ReflectionInterface ri)
            : base(ri, "TcpaCGPTA1", "ERR_BU35", "EOJ_BU35", EOJ_FIELDS)
        {

        }

        public override void Main()
        {
            StartupMessage("This is the TCPA Mobile Phone Numbers that have Consent before April 1st, 2012 - FED. Click OK to continue, or Cancel to quit.");
            string[] arcsToCheckAccess = new string[] { "APRLE", "APRLS", "APRLR", "APRLB" };

            foreach (string arc in arcsToCheckAccess)
            {
                if (!CheckArcAccess(arc))
                {
                    NotifyAndEnd("You do not have access to the ARC: {0}", arc);
                }
            }

            List<string> filesToProcess = Directory.GetFiles(Efs.FtpFolder, FilePattern).ToList();

            if (filesToProcess.Count < 1)
            {
                NotifyAndEnd("There were no files to process");
            }

            foreach (string file in filesToProcess)
            {
                List<string> fileData = new List<string>();
                using (StreamReader sr = new StreamReader(file))
                {
                    sr.ReadLine();//read in the header
                    bool hasData = false;

                    while (!sr.EndOfStream)
                    {
                        fileData.Add(sr.ReadLine());//Read in the Borrowers account number.  

                        //increment the end of job count if not in recovery.
                        if (Recovery.RecoveryValue.IsNullOrEmpty())
                        {
                            //The first recovery value is set after this is complete.  We do not want to redo this if we are in recovery
                            Eoj.Counts[EojTotalFromSas].Increment();
                        }

                        hasData = true;
                    }

                    if (!hasData)
                    {
                        if (file.Contains("R3"))
                        {
                            MessageBox.Show(string.Format("The SAS file {0} was empty.", file.Substring(file.LastIndexOf(@"\"))));
                            continue;
                        }
                        else
                            NotifyAndEnd("The SAS file {0} was empty.", file.Substring(file.LastIndexOf(@"\")));
                    }
                }

                if (Recovery.RecoveryValue.IsNullOrEmpty())
                {
                    //If the Recovery is already set do not reset it with another value.
                    Recovery.RecoveryValue = "0,EojRecordsCountsComplete";
                }

                //Parse out the recovery index from the recovery file.
                int recoveryCounter = int.Parse(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0]);

                if (!IsLoggedIn())
                {
                    NotifyAndEnd("You are no longer logged into the session.");
                }

                //For recovery we want to skip how ever many we completed when it crashed.
                foreach (string accountNumber in fileData.Skip(recoveryCounter))
                {
                    FastPath("TX3Z/CTX1J");
                    //Clear out all of the populated fields
                    PutText(5, 16, "", true);
                    PutText(6, 16, "", true);
                    PutText(6, 20, "", true);
                    PutText(6, 23, "", true);

                    if (accountNumber.SafeSubString(0, 1).Contains("P"))
                    {
                        PutText(6, 16, accountNumber, ReflectionInterface.Key.Enter);
                    }
                    else
                    {
                        PutText(6, 61, accountNumber, ReflectionInterface.Key.Enter);
                    }

                    if (!CheckForText(1, 71, "TXX1R"))
                    {
                        MessageBox.Show("Script unsuccessfully input account number.");
                        Err.AddRecord("Unable to locate the following account: ", new { AccountNumber = accountNumber });
                        continue;
                    }

                    Hit(ReflectionInterface.Key.F6);
                    Hit(ReflectionInterface.Key.F6);
                    Hit(ReflectionInterface.Key.F6);

                    string phoneType = GetText(16, 14, 1);
                    string personType = GetText(1, 9, 1);
                    string borrowerSsn = string.Empty;
                    string otherSsn = string.Empty;//this will the the Endorsers or students Ssn

                    if (personType.Contains("R"))
                    {
                        //Get the Ssn for the borrower this will be used when adding a comment to TD22
                        borrowerSsn = GetText(7, 11, 11).Replace(" ", "");
                    }
                    else if (personType.Contains("S") || personType.Contains("E"))
                    {
                        //Get the borrowers Ssn and the Endorser's Ssn, this will be used when adding a comment to TD22
                        borrowerSsn = GetText(7, 11, 11).Replace(" ", "");
                        otherSsn = GetText(3, 12, 11).Replace(" ", "");
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

                            //Only process accounts where the verified date is before 04/02/2012
                            if (phoneLastVerified < new DateTime(2012, 04, 01))
                            {
                                sResults = MblAndConsentProcessing(mbl, consent, borrowerSsn, accountNumber);
                            }
                            else
                            {
                                sResults = ScriptResults.None;
                            }
                        }
                        else
                        {
                            sResults = MblAndConsentProcessing(mbl, consent, borrowerSsn, accountNumber);
                        }

                        if (!results.Keys.Contains(sResults))
                        {
                            results.Add(sResults, phoneType);
                        }
                        else
                        {
                            string temp = results.Where(p => p.Key == sResults).Select(q => q.Value).FirstOrDefault();
                            results[sResults] = results.Where(p => p.Key == sResults).Select(q => q.Value).FirstOrDefault() + "," + phoneType;
                        }

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
                        CheckTd22Results result = CheckTd22(accountNumber, personType.Contains("E"));
                        switch (result)
                        {
                            case CheckTd22Results.GotEndorser:
                                borrowerSsn = GetText(7, 11, 11).Replace(" ", "");
                                break;
                            case CheckTd22Results.None:
                                Err.AddRecord("Unable to leave comment in the following account: ", new { AccountNumber = accountNumber });
                                Eoj.Counts[EojErrorReport].Increment();
                                continue;
                        }
                    }

                    //Go through Each Key in the dictionary and see 
                    foreach (KeyValuePair<ScriptResults, string> item in results)
                    {
                        string comment = string.Empty;
                        bool commentAdded = false;

                        if (personType.Contains("B"))
                        {
                            switch (item.Key)
                            {
                                case ScriptResults.MConsentN:
                                    comment = string.Format("Changed CONSENT field to N for phone type(s) {0} because CornerStone obtained consent prior to April 1st, 2012", item.Value);
                                    break;
                                case ScriptResults.ConsentY:
                                    comment = string.Format("Changed CONSENT field to Y for phone type(s) {0} because phone is a landline", item.Value);
                                    break;
                                case ScriptResults.UConsentN:
                                    comment = string.Format("Changed CONSENT field to N for phone type(s) {0} because phone number was unidentified", item.Value);
                                    break;
                                case ScriptResults.None:
                                    continue;
                            }

                            commentAdded = Atd22AllLoans(borrowerSsn, "APRLB", comment, borrowerSsn, ScriptId, false);
                        }
                        else if (personType.Contains("E"))
                        {
                            switch (item.Key)
                            {
                                case ScriptResults.MConsentN:
                                    comment = string.Format("Changed CONSENT field to N for endorser’s {0} phone type(s) {1} because CornerStone obtained consent prior to April 1st, 2012", accountNumber, item.Value);
                                    break;
                                case ScriptResults.ConsentY:
                                    comment = string.Format("Changed CONSENT field to Y for endorser’s {0} phone type(s) {1} because phone is a landline", accountNumber, item.Value);
                                    break;
                                case ScriptResults.UConsentN:
                                    comment = string.Format("Changed CONSENT field to N for endorser’s {0} phone type(s) {1} because phone number was unidentified", accountNumber, item.Value);
                                    break;
                                case ScriptResults.None:
                                    continue;
                            }

                            commentAdded = Atd22AllLoans(borrowerSsn, "APRLE", comment, otherSsn, ScriptId, false);
                        }
                        else if (personType.Contains("R"))
                        {
                            switch (item.Key)
                            {
                                case ScriptResults.MConsentN:
                                    comment = string.Format("Changed CONSENT field to N for reference’s {0} phone type(s) {1} because CornerStone obtained consent prior to April 1st, 2012", accountNumber, item.Value);
                                    break;
                                case ScriptResults.ConsentY:
                                    comment = string.Format("Changed CONSENT field to N for reference’s {0} phone type(s) {1} because phone is a landline", accountNumber, item.Value);
                                    break;
                                case ScriptResults.UConsentN:
                                    comment = string.Format("Changed CONSENT field to N for reference’s {0} phone type(s) {1} because phone number was unidentified", accountNumber, item.Value);
                                    break;
                                case ScriptResults.None:
                                    continue;
                            }

                            commentAdded = Atd22AllLoans(borrowerSsn, "APRLR", comment, accountNumber, ScriptId, false, true);
                        }
                        else
                        {
                            switch (item.Key)
                            {
                                case ScriptResults.MConsentN:
                                    comment = string.Format("Changed CONSENT field to N for student’s {0} phone type(s) {1} because CornerStone obtained consent prior to April 1st, 2012", accountNumber, item.Value);
                                    break;
                                case ScriptResults.ConsentY:
                                    comment = string.Format("Changed CONSENT field to N for student’s {0} phone type(s) {1} because phone is a landline", accountNumber, item.Value);
                                    break;
                                case ScriptResults.UConsentN:
                                    comment = string.Format("Changed CONSENT field to N for student’s {0} phone type(s) {1} because phone number was unidentified", accountNumber, item.Value);
                                    break;
                                case ScriptResults.None:
                                    continue;
                            }

                            commentAdded = Atd22ByBalance(borrowerSsn, "APRLS", comment, otherSsn, ScriptId, false);
                        }

                        //If the comment was not added successfully then add a record to the error report.
                        if (!commentAdded)
                        {
                            Err.AddRecord("Unable to leave comment in the following account: ", new { AccountNumber = accountNumber });
                        }

                    }//end foreach with the dictionary
                }//end foreach accountNumber
                Recovery.Delete();//Clean up recovery for the next file.
                File.Delete(file);
            }//end foreach file

            ProcessingComplete();
        }

        /// <summary>
        /// Checks to see if the current user has access to a given Arc
        /// </summary>
        /// <param name="arc">Arc to check access</param>
        /// <returns>Returns true the user has access and false if the user does not have access</returns>
        private bool CheckArcAccess(string arc)
        {
            FastPath("TX3Z/ITX68");
            PutText(8, 41, UserId, true);
            PutText(10, 41, arc, ReflectionInterface.Key.Enter, true);

            return CheckForText(1, 72, "TXX6C");
        }

        private ScriptResults MblAndConsentProcessing(string mbl, string consent, string bwrSsn, string accountNumber)
        {
            //If the phone number is not valid do nothing and return.
            if (!CheckForText(17, 54, "Y"))
            {
                return ScriptResults.None;
            }

            //Update Moblie phone number with a Consent of Y to a consent of N
            if (mbl.Contains("M") && consent.Contains("Y"))
            {
                PutText(16, 20, "M");
                PutText(16, 30, "N");
                PutText(17, 54, "Y");
                PutText(16, 45, DateTime.Now.ToString("MMddyy"));
                PutText(19, 14, "31", ReflectionInterface.Key.Enter);

                //If account was not updated add to error report and return nothing
                if (!CheckForText(23, 2, "01097"))
                {
                    Atd22AllLoans(bwrSsn, "APRIL", string.Format("Unable to save mobile indicator changes for account {0}", accountNumber), string.Empty, ScriptId, false);
                    return ScriptResults.None;
                }

                return ScriptResults.MConsentN;
            }
            else if (mbl.Contains("L") && consent.Contains("N"))//Update any landline to Y
            {
                PutText(16, 20, "L");
                PutText(16, 30, "Y");
                PutText(16, 45, DateTime.Now.ToString("MMddyy"));
                PutText(19, 14, "21", ReflectionInterface.Key.Enter);

                //If account was not updated add to error report and return nothing
                if (!CheckForText(23, 2, "01097"))
                {
                    Atd22AllLoans(bwrSsn, "APRIL", string.Format("Unable to save mobile indicator changes for account {0}", accountNumber), string.Empty, ScriptId, false);
                    return ScriptResults.None;
                }

                return ScriptResults.ConsentY;
            }
            else if (mbl.Contains("U") && consent.Contains("Y"))//Update any unknown number to N
            {
                PutText(16, 20, "U");
                PutText(16, 30, "N");
                PutText(17, 54, "Y");
                PutText(16, 45, DateTime.Now.ToString("MMddyy"));
                PutText(19, 14, "31", ReflectionInterface.Key.Enter);

                //If account was not updated add to error report and return nothing
                if (!CheckForText(23, 2, "01097"))
                {
                    Atd22AllLoans(bwrSsn, "APRIL", string.Format("Unable to save mobile indicator changes for account {0}", accountNumber), string.Empty, ScriptId, false);
                    return ScriptResults.None;
                }

                return ScriptResults.UConsentN;
            }

            //Return nothing if none of the above criteria is met.
            return ScriptResults.None;
        }

        private CheckTd22Results CheckTd22(string accountNumber, bool checkEnd)
        {
            string arcToUse = checkEnd ? "APRLE" : "APRLB";
            string personTypeToCheck = checkEnd ? "E" : "S";
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
                {
                    return CheckTd22Results.GotEndorser;
                }

                return CheckTd22Results.None;
            }

            return CheckTd22Results.HasLoans;
        }
    }
}
