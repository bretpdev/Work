using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ONTIMEPYBB
{
    public class OnTimePayCleanup : BatchScript
    {
        public const string EOJ_TotalProcessed = "Total number processed";
        public const string EOJ_TotalInFile = "Total number in file";
        public const string ERR_ErrorProccessing = "There was an error processing account";
        public const string ERR_EligibleMissingCounter = "The payment counter was missing for borrower";
        public static List<string> EOJ_Fields = new List<string>() { EOJ_TotalInFile, EOJ_TotalProcessed, ERR_ErrorProccessing, ERR_EligibleMissingCounter };

        public string Header = string.Empty;
        public string Addition = "Approved BB Additions.csv";
        public string Correction = "Approved BB Corrections.csv";
        public string Removal = "Approved BB Removals.csv";
        public string ErrorFile = EnterpriseFileSystem.TempFolder + "On Time Payment Error Report.txt";
        public string Changes = "Approved BB Plan Changes.csv";

        public OnTimePayCleanup(ReflectionInterface ri)
            : base(ri, "ONTIMEPYBB", "ERR_BU01", "EOJ_BU01", EOJ_Fields, DataAccessHelper.Region.Uheaa)
        {
        }

        public override void Main()
        {
            string approvedFile = FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, Addition);
            string file = FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, Correction);
            string removalFile = FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, Removal);
            string changesFile = FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, Changes);

            CheckFileExists(approvedFile, file, removalFile, changesFile);

            List<BorrowerData> data = LoadData(file, false, false);
            List<BorrowerData> appData = LoadData(approvedFile, true, false);
            List<BorrowerData> removalData = LoadData(removalFile, false, false);
            //TODO verify that the changes file is of the same format
            List<BorrowerData> changesData = LoadData(changesFile, false, true);


            //Removal
            if (removalFile != null)
            {
                ProcessData(removalData, false, Removal, false);
                File.Delete(removalFile);
            }
            //Add
            if (approvedFile != null)
            {
                ProcessData(appData, true, Addition, false);
                File.Delete(approvedFile);
            }
            //Correct
            if (file != null)
            {
                ProcessData(data, false, Correction, false);
                File.Delete(file);
            }
            //Change
            if(changesFile != null)
            {
                ProcessData(changesData, false, Changes, true);
                File.Delete(changesFile);
            }

            if (Err.HasErrors)
                ProcessingComplete(string.Format("Processing complete with errors. Please see the error report.\r\n\r\nA new error file is located in {0}", ErrorFile));
            else
                ProcessingComplete();
        }

        /// <summary>
        /// Validate file existence for all files.  Process Log any error.
        /// </summary>
        private void CheckFileExists(string approvedFile, string file, string removalFile, string changes)
        {
            List<string> errors = new List<string>();
            if (file.IsNullOrEmpty())
                errors.Add(Correction);
            if (approvedFile.IsNullOrEmpty())
                errors.Add(Addition);
            if (removalFile.IsNullOrEmpty())
                errors.Add(Removal);
            if (changes.IsNullOrEmpty())
                errors.Add(Changes);

            if (errors.Any())
            {
                string errorMessage = "The following files were not found \r\n\r\n" + string.Join(Environment.NewLine, errors);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning, ProcessLogData.ExecutingAssembly);
                Dialog.Error.Ok(errorMessage);
            }
        }

        /// <summary>
        /// Load the data in the file into a List of BorrowerData
        /// </summary>
        private List<BorrowerData> LoadData(string file, bool add, bool changeFile)
        {
            List<BorrowerData> borData = new List<BorrowerData>();
            if (file != null)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string firstLine = sr.ReadLine();
                    if (HasHeader(firstLine))
                    {
                        Header = firstLine;
                        firstLine = "";
                    }
                    while (!sr.EndOfStream)
                    {
                        BorrowerData data = new BorrowerData();
                        List<string> line = firstLine.IsNullOrEmpty() ? sr.ReadLine().SplitAndRemoveQuotes(",") : firstLine.SplitAndRemoveQuotes(",");

                        if (ValidateData(line))
                        {
                            if (line[0].Length == 7)
                                data.SSN = "00" + line[0];
                            else if (line[0].Length == 8)
                                data.SSN = "0" + line[0];
                            else
                                data.SSN = line[0];

                            data.LoanSequence = line[1].ToInt();

                            data.CorrectedStatus = (StatusCodes.CorrectedStatus)Enum.Parse(typeof(StatusCodes.CorrectedStatus), line[3], true);

                            if (add || changeFile)
                                data.BBType = line[2];

                            data.PCVPayments = line[4];
                            data.DisqualDate = line[5];
                            data.DisqualReason = line[6];
                            if (line[7].Length > 140)
                                data.Comment = line[7].Substring(0, 139); //ensure 140 character max length for comment
                            else
                                data.Comment = line[7];
                            borData.Add(data);
                        }
                    }
                }
                return borData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Check the file to determine if a header is present
        /// </summary>
        private bool HasHeader(string firstLine)
        {
            try
            {
                int number = Convert.ToInt32(firstLine.SplitAndRemoveQuotes(",")[0]);
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        /// <summary>
        /// Verifies that the passed in line has the right number of fields
        /// </summary>
        private bool ValidateData(List<string> line)
        {
            if (line.Count() == 0)
                return false;
            if (line[2].ToUpper() == "ELIGIBLE")
            {
                if (line.Count() < 4)
                {
                    Err.AddRecord(ERR_EligibleMissingCounter, new { SSN = line[0], LoanSequence = line[1], BBType = line[2], CorrectedStatus = line[3], PCVPayments = line[4], DisqualDate = line[5], DisqualReason = line[6], Comment = line[7] });
                    Eoj.Counts[ERR_EligibleMissingCounter].Increment();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Navigates to the correct Session Screen and calls ChangeStatus to update the borrowers benefits.
        /// Benefits modifications based on data file.
        /// </summary>
        private void ProcessData(List<BorrowerData> data, bool add, string file, bool changeFile)
        {
            String ErrMessage = "";
            Recovery.RecoveryValue = "";
            int counter = Recovery.RecoveryValue.IsNullOrEmpty() ? 0 : Recovery.RecoveryValue[0].ToString().ToInt();

            //Create a StreamWriter to write all the errors to a new error file that can be run again
            using (StreamWriter sw = new StreamWriter(ErrorFile))
            {
                if (!Header.IsNullOrEmpty())
                    sw.WriteLine(Header);
                if (file.IsPopulated())
                {
                    foreach (BorrowerData bor in data.Skip(counter))
                    {
                        if (add)
                            RI.FastPath("TX3ZATSDS" + bor.SSN);
                        else
                            RI.FastPath("TX3ZCTSDS" + bor.SSN);

                        //Check to make sure it is either on the selection screen or the screen to process
                        if (RI.CheckForText(1, 72, "TSXDV"))
                        {
                            ChangeStatus(bor, counter, sw, add, file, changeFile);
                            continue;
                        }
                        if (!RI.CheckForText(1, 75, "TSXDU"))
                        {
                            ErrMessage = "Unable to find TSXDU";
                            counter = ErrorAndRecovery(bor, counter, sw, file, ErrMessage);
                            continue;
                        }
                        //Choose which loan to change by the sequence number
                        int selection = SelectLoanSequence(bor.LoanSequence);
                        if (selection == 0)
                        {
                            ErrMessage = "No Load Sequence provided.";
                            counter = ErrorAndRecovery(bor, counter, sw, file, ErrMessage);
                            continue;
                        }
                        RI.PutText(21, 18, selection.ToString(), ReflectionInterface.Key.Enter, true);
                        ChangeStatus(bor, counter, sw, add, file, changeFile);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the selection number for the given loan sequence number
        /// </summary>
        private int SelectLoanSequence(int seq)
        {
            while (!RI.CheckForText(23, 2, "90007"))
            {
                for (int i = 8; i < 20; i++)
                {
                    int checkSeq = RI.GetText(i, 8, 3).ToInt();
                    if (checkSeq == seq)
                    {
                        return RI.GetText(i, 4, 3).Trim().ToInt();
                    }
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
            return 0;
        }

        /// <summary>
        /// Reads a borrower object, determines the benefit status, and calls an update function based on that status.
        /// </summary>
        private void ChangeStatus(BorrowerData bor, int counter, StreamWriter sw, bool add, string file, bool changeFile)
        {
            String ErrMessage = "";
            //If it did not find the loan sequence number, add to error log and continue processing
            if (!RI.CheckForText(1, 72, "TSXDV"))
            {
                ErrMessage = "Unable to find TSXDV";
                counter = ErrorAndRecovery(bor, counter, sw, file, ErrMessage);
                return;
            }

            bool processed = true;
            switch (bor.CorrectedStatus)
            {
                case StatusCodes.CorrectedStatus.Disqualified:
                    processed = SetDisqualified(bor);
                    break;
                case StatusCodes.CorrectedStatus.Qualified:
                    processed = SetQualified(bor, add, changeFile);
                    break;
                case StatusCodes.CorrectedStatus.Eligible:
                    processed = SetEligible(bor);
                    break;
                case StatusCodes.CorrectedStatus.Ineligible:
                    processed = SetIneligible(bor);
                    break;
                default:
                    {
                        ErrMessage = "No Corrected Status Provided.";
                        counter = ErrorAndRecovery(bor, counter, sw, file, ErrMessage);
                        return;
                    }
            }
            if (!processed)
            {
                ErrMessage = "Line Not Processed. Status Change failed.";
                counter = ErrorAndRecovery(bor, counter, sw, file, ErrMessage);
                return;
            }
            else
            {
                Eoj.Counts[EOJ_TotalProcessed].Increment();
                Eoj.Counts[EOJ_TotalInFile].Increment();
                Recovery.RecoveryValue = (++counter).ToString();
            }
        }

        /// <summary>
        /// Add the borrower to the error report and increment the recovery counter
        /// </summary>
        private int ErrorAndRecovery(BorrowerData bor, int counter, StreamWriter sw, string file, String Error)
        {
            string errorMessage = string.Format("{0},{1},{2},{3},{4},{5},{6}. \r\n Error Message: {7}", bor.SSN, bor.LoanSequence, bor.CorrectedStatus, bor.PCVPayments, bor.DisqualDate, bor.DisqualReason, bor.Comment, Error);
            counter++;
            Err.AddRecord(ERR_ErrorProccessing, bor);
            Eoj.Counts[ERR_ErrorProccessing].Increment();
            Eoj.Counts[EOJ_TotalInFile].Increment();
            Recovery.RecoveryValue = (counter).ToString();
            sw.WriteLine(errorMessage);
            ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, string.Format("Error in processing file {0}, line {1}", file, errorMessage), NotificationType.ErrorReport, NotificationSeverityType.Critical, ProcessLogData.ExecutingAssembly);
            return counter;
        }

        /// <summary>
        /// Changes eligibility to false, sets the disqualification date and reason from the file, and leaves a comment in the session.
        /// </summary>
        private bool SetDisqualified(BorrowerData bor)
        {
            RI.PutText(11, 16, "X", true);
            RI.PutText(13, 16, bor.DisqualDate, true);
            RI.PutText(13, 44, bor.DisqualReason, true);
            RI.Hit(ReflectionInterface.Key.F6);
            RI.PutText(21, 11, bor.Comment, true);
            RI.Hit(ReflectionInterface.Key.F6);

            if (RI.MessageCode.Contains("01005"))
                return true;

            return false;
        }

        /// <summary>
        /// Changes eligibility to true, sets the benefit type (new benefits only), and leaves a comment in the session.
        /// </summary>
        private bool SetQualified(BorrowerData bor, bool add, bool changeFile)
        {
            //Requires removal and then addition afterwards (should not be in the correction file)
            RI.PutText(11, 16, "Y", true);
            if (add || changeFile)
                RI.PutText(8, 16, bor.BBType, true);
            RI.PutText(12, 16, bor.PCVPayments, true);
            RI.Hit(ReflectionInterface.Key.F6);
            if(RI.CheckForText(23,2,"05774"))
            {
                RI.Hit(ReflectionInterface.Key.F6);
            }
            RI.PutText(21, 11, bor.Comment, true);
            RI.Hit(ReflectionInterface.Key.F6);

            if (RI.MessageCode.Contains("01005") || RI.MessageCode.Contains("01004"))
                return true;

            return false;
        }

        /// <summary>
        /// Changes eligibility to true, clears the disqualification date and reason if populated, and leaves a comment in the session.
        /// </summary>
        private bool SetEligible(BorrowerData bor)
        {
            //Requires removal and then addition afterwards (should not be in the correction file)
            RI.PutText(11, 16, "Y", true);
            RI.PutText(12, 16, bor.PCVPayments, true);

            //blank out the disqual date
            RI.PutText(13, 16, "", true);
            RI.PutText(13, 19, "", true);
            RI.PutText(13, 22, "", true);

            RI.PutText(13, 44, "", true);
            RI.Hit(ReflectionInterface.Key.F6);
            if (RI.CheckForText(23, 2, "05774"))
            {
                RI.Hit(ReflectionInterface.Key.F6);
            }
            RI.PutText(21, 11, bor.Comment, true);
            RI.Hit(ReflectionInterface.Key.F6);

            if (RI.MessageCode.Contains("01005"))
                return true;

            return false;
        }

        /// <summary>
        /// Changes eligibility to false, clears the disqualification date, reason, and benefits type, and leaves a comment in the session.
        /// </summary>
        private bool SetIneligible(BorrowerData bor)
        {
            /* OLD
            RI.PutText(8, 16, "", true); //BBP, Borrower Benefit Type
            RI.PutText(11, 16, "", true); //Eligibility
            RI.PutText(10, 16, "", true); //Effective Date
            RI.PutText(13, 16, "", true); //Disqual Date
            RI.PutText(13, 44, "", true); //Disqual Reason Code
            RI.PutText(15, 16, "", true); //Processing (Y/N)
            RI.Hit(ReflectionInterface.Key.F6);
            RI.PutText(21, 11, bor.Comment, true); //Comment
            RI.Hit(ReflectionInterface.Key.F6);
            */

            RI.PutText(8, 16, bor.BBType, true); //BBP, Borrower Benefit Type
            RI.PutText(11, 16, "X", true); //Eligibility
            //RI.PutText(10, 16, "", true); //Effective Date
            RI.PutText(13, 16, bor.DisqualDate, true); //Disqual Date
            RI.PutText(13, 44, bor.DisqualReason, true); //Disqual Reason Code
            //RI.PutText(15, 16, "", true); //Processing (Y/N)
            RI.Hit(ReflectionInterface.Key.F6);
            RI.PutText(21, 11, bor.Comment, true); //Comment
            RI.Hit(ReflectionInterface.Key.F6);

            if (RI.MessageCode.Contains("01004")) //OLD (RI.MessageCode.Contains("05654"))
                return true;

            return false;
        }
    }
}
