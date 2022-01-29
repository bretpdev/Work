using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Efs = Uheaa.Common.DataAccess.EnterpriseFileSystem;

namespace TCPAMCGTNM
{
    public class TcpaMobileConsentGivenThroughNewMpnFed : FedBatchScript
    {

        private const string EojTotalsFromSas = "Total number of records in the SAS file";
        private const string EojProcessed = "Number of records successfully processed";
        private const string EojNoUpdate = "No update needed";
        private const string EojErrorQueue = "Number of records sent to error queue";
        private const string EojErrorReport = "Number of records sent to error report";
        private const string FileToProcessPattern = "UNWS23.NWS23*";
        private const string CompleteArc = "PHMPN";
        private const string ErrorArc = "ERMPN";
        private const string ErrorReportKey = "ERR_BU35";
        private static readonly string[] EojFields = { EojTotalsFromSas, EojProcessed, EojNoUpdate, EojErrorQueue, EojErrorReport };

        private enum CommentResults
        {
            BorrowerHasNoOpenLoans,
            CommentAdded,
            ErrorAddingComment
        }

        public TcpaMobileConsentGivenThroughNewMpnFed(ReflectionInterface ri)
            : base(ri, "TcpaMCGTNM", ErrorReportKey, "TCPA_EOJ", EojFields)
        {
        }

        public override void Main()
        {
            StartupMessage("This is the TCPA Mobile Consent Given Through New MPN - FED script. Click OK to continue, or Cancel to quit.");
            bool hasErrors = false;

            if (!CheckArcAccess(CompleteArc)) { NotifyAndEnd("You do not have access to the ARC: {0}", CompleteArc); }

            if (!CheckArcAccess(ErrorArc)) { NotifyAndEnd("You do not have access to the ARC: {0}", ErrorArc); }

            List<string> filesToProcess = Directory.GetFiles(Efs.FtpFolder, FileToProcessPattern).ToList();

            if (filesToProcess.Count < 1)
            {
                NotifyAndEnd("There were no files to process.");
            }

            foreach (string file in filesToProcess)
            {
                hasErrors = false;
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
                            Eoj.Counts[EojTotalsFromSas].Increment();
                        }

                        hasData = true;
                    }

                    if (!hasData)
                    {
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

                //For recovery we want to skip how ever many we completed when it crashed.
                foreach (string accountNumber in fileData.Skip(recoveryCounter))
                {
                    try
                    {
                        FastPath("TX3Z/CTX1J");
                    }
                    catch (Exception)
                    {
                        //Check to see if the user is logged in.
                        NotifyAndEnd("You are not logged in.  Please try again.");
                    }

                    //Clear the screen.  When using the fastpath, sometimes data from the previous account is still populated.
                    PutText(5, 16, "", true);
                    PutText(6, 16, "", true);
                    PutText(6, 20, "", true);
                    PutText(6, 23, "", true);

                    string ssn = DataAccess.GetSsnFromAccountNumber(accountNumber);
                    string results = string.Empty;
                    bool hadError = false;

                    if (Recovery.RecoveryValue.Contains("EojRecordsCountsComplete") || Recovery.RecoveryValue.Contains("CommentAdded"))
                    {
                        PutText(6, 61, accountNumber, ReflectionInterface.Key.Enter);
                        if (!CheckForText(1, 71, "TXX1R"))
                        {
                            if (!Atd22AllLoans(accountNumber, "ERMPN", "Unable to locate account’s demographics", string.Empty, ScriptId, false))
                            {
                                Err.AddRecord(@"unable to add error Arc ERMPN.", new { AccountNumber = accountNumber });
                                Eoj.Counts[EojErrorReport].Increment();
                            }
                            else
                            {
                                Eoj.Counts[EojErrorQueue].Increment();
                            }
                            GetBackToTx1j(accountNumber, string.Empty);
                            hasErrors = true;
                            continue;
                        }

                        string phoneType = GetText(16, 14, 1);

                        //Move down to the phone section of TX1J.
                        Hit(ReflectionInterface.Key.F6);
                        Hit(ReflectionInterface.Key.F6);
                        Hit(ReflectionInterface.Key.F6);

                        while (true)
                        {
                            string phoneValidity = GetText(17, 54, 1);
                            string mbl = GetText(16, 20, 1);
                            string consent = GetText(16, 30, 1);

                            if (phoneValidity.Contains("Y"))
                            {
                                PutText(17, 54, "Y");

                                if (mbl.Contains("M") && consent.Contains("N"))
                                {
                                    PutText(16, 20, "M");
                                    PutText(16, 30, "Y");
                                    if (!UpdateAndSaveChanges())
                                    {
                                        if (!Atd22AllLoans(accountNumber, "ERMPN", "Unable to save mobile indicator changes", string.Empty, ScriptId, false))
                                        {
                                            if (!CheckForText(23, 2, "01764"))
                                            {
                                                Err.AddRecord(@"unable to add error Arc ERMPN.", new { AccountNumber = accountNumber });
                                                Eoj.Counts[EojErrorReport].Increment();
                                            }
                                        }
                                        else
                                        {
                                            Eoj.Counts[EojErrorQueue].Increment();
                                        }
                                        hadError = true;
                                        phoneType = GetBackToTx1j(accountNumber, phoneType);
                                        hasErrors = true;
                                        continue;
                                    }

                                    results += phoneType + ",";
                                }
                                else if (mbl.Contains("U"))
                                {
                                    PutText(16, 20, "M");
                                    PutText(16, 30, "Y");
                                    if (!UpdateAndSaveChanges())
                                    {
                                        if (!Atd22AllLoans(accountNumber, "ERMPN", "Unable to save mobile indicator changes", string.Empty, ScriptId, false))
                                        {
                                            if (!CheckForText(23, 2, "01764"))
                                            {
                                                Err.AddRecord(@"unable to add error Arc ERMPN.", new { AccountNumber = accountNumber });
                                                Eoj.Counts[EojErrorReport].Increment();
                                            }
                                        }
                                        else
                                        {
                                            Eoj.Counts[EojErrorQueue].Increment();
                                        }

                                        hadError = true;
                                        phoneType = GetBackToTx1j(accountNumber, phoneType);
                                        hasErrors = true;
                                        continue;
                                    }
                                    results += phoneType + ",";
                                }
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
                        }

                        Recovery.RecoveryValue = string.Format("{0},UpdatedTX1J", recoveryCounter);
                    }

                    if (Recovery.RecoveryValue.Contains("UpdatedTX1J"))
                    {
                        if (!results.IsNullOrEmpty())
                        {
                            string comment = string.Format("Changed CONSENT field to Y for PHONE TYP(S) {0} because borrower signed their MPN after April 24th, 2009", results.Remove(results.LastIndexOf(",")));

                            CommentResults result = AddComment(ssn, CompleteArc, comment);

                            switch (result)
                            {
                                case CommentResults.BorrowerHasNoOpenLoans:
                                    Err.AddRecord(@"unable to leave comment in borrower's notes, borrower has no open loans.", new { AccountNumber = accountNumber });
                                    Eoj.Counts[EojErrorReport].Increment();
                                    hasErrors = true;
                                    break;
                                case CommentResults.ErrorAddingComment:
                                    Err.AddRecord(@"unable to leave comment in borrower's notes.", new { AccountNumber = accountNumber });
                                    Eoj.Counts[EojErrorReport].Increment();
                                    hasErrors = true;
                                    break;
                                case CommentResults.CommentAdded:
                                    Eoj.Counts[EojProcessed].Increment();
                                    break;
                            }
                        }
                        else
                        {
                            if (!hadError)
                            {
                                Eoj.Counts[EojNoUpdate].Increment();
                            }
                        }
                        recoveryCounter++;
                        Recovery.RecoveryValue = string.Format("{0},CommentAdded", recoveryCounter);
                    }
                }//end foreach

                Recovery.Delete();//Clean up recovery for the file.
                File.Delete(file);
            }

            if (hasErrors)
            {
                //Send an email to Systems Support if the script wrote to the error report.
                string body = string.Format("The script encountered errors and generated an error report located in {0}", Efs.GetPath(ErrorReportKey));
                SendMail("sshelp@utahsbr.edu", "TCPA Mobile Consent Given Through New MPN - FED", "Error Report Created", body, string.Empty, string.Empty, string.Empty, EmailImportance.High, false);
            }

            ProcessingComplete();
        }

        private string GetBackToTx1j(string accountNumber, string phoneType)
        {
            FastPath("TX3Z/CTX1J");
            //Clear the screen.  When using the fastpath, sometimes data from the previous account is still populated.
            PutText(5, 16, "", true);
            PutText(6, 16, "", true);
            PutText(6, 20, "", true);
            PutText(6, 23, "", true);
            PutText(6, 61, accountNumber, ReflectionInterface.Key.Enter);
            //Move down to the phone section of TX1J.
            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);

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
            }

            return phoneType;

        }

        /// <summary>
        /// Finishes the update on TX1J
        /// </summary>
        /// <returns>Returns true of changes were added.  Returns false if could not commit the changes.</returns>
        private bool UpdateAndSaveChanges()
        {
            PutText(16, 45, DateTime.Now.ToString("MMddyy"));
            PutText(19, 14, "03", ReflectionInterface.Key.Enter);
            return CheckForText(23, 2, "01097");
        }

        /// <summary>
        /// Adds a comment to TD22.  I am not using the common code here because the spec wants to check to see if there are no open loans.
        /// The common code just returns a boolean if the comment was added or if it was not.
        /// </summary>
        /// <param name="ssn">SSN for the borrower.  Needed for the recipient Id</param>
        /// <param name="arc">Arc to use.</param>
        /// <returns>Returns an Enum indicating what the result was.</returns>
        private CommentResults AddComment(string ssn, string arc, string comment)
        {
            FastPath(string.Format("TX3Z/ATD22{0};{1}", ssn, arc));

            if (CheckForText(23, 2, "50108", "90003"))
            {
                return CommentResults.BorrowerHasNoOpenLoans;
            }

            //select all loans
            for (int row = 11; !CheckForText(23, 2, "90007"); row++)
            {
                if (row > 18)
                {
                    Hit(ReflectionInterface.Key.F8);
                    row = 10;
                }

                if (CheckForText(row, 3, "_"))
                {
                    PutText(row, 3, "X");
                }
            }

            PutText(21, 2, comment, ReflectionInterface.Key.Enter);

            if (CheckForText(23, 2, "02860"))
            {
                return CommentResults.CommentAdded;
            }
            else
            {
                return CommentResults.ErrorAddingComment;
            }
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

            if (CheckForText(1, 72, "TXX6C"))
            {
                return true;
            }

            return false;
        }
    }
}

