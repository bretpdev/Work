using Q;
using System;
using System.Text;

namespace COSUSPPUT
{
    public class FileProcessor : ScriptSessionBase
    {

        public enum FileType
        {
            R2,
            R3
        }

        public const string ERROR_LOG = "Suspense PUT Error Log.txt";
        public const string MANUAL_REVIEW_LOG_R2 = "Unprocessed Suspense All PUT Loans.txt";
        public const string MANUAL_REVIEW_LOG_R3 = "Unprocessed Suspense Part PUT Loans.txt";
        public const string ED_TRANSMITTAL_LOG = "ED Transmittal Suspense Pmt Deleted.txt";

        private string _scriptID;
        private string _fileToProcess;
        private string _manualReviewLog;
        private string _errorLog;
        private string _edTransmittalLog;
        private string _userID = string.Empty;
        private string UserID 
        { 
            get
            {
                if (_userID == string.Empty)
                {
                    //if user ID not gathered yet then get it
                    _userID = GetUserIDFromLP40();
                }
                return _userID;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ri">ReflectionInterface object</param>
        /// <param name="fileToProcess">File to process</param>
        /// <param name="fileTypeToProcess">File type to process.</param>
        /// <param name="scriptID">Script ID from Sacker</param>
        public FileProcessor(ReflectionInterface ri, string fileToProcess, FileType fileTypeToProcess, string scriptID)
            : base(ri)
        {
            //populate script ID
            _scriptID = scriptID;
            //set up file names
            _fileToProcess = fileToProcess;
            _edTransmittalLog = string.Format("{0}{1}", DataAccessBase.PersonalDataDirectory, ED_TRANSMITTAL_LOG);
            _errorLog = string.Format("{0}{1}", DataAccessBase.PersonalDataDirectory, ERROR_LOG);
            if (fileTypeToProcess == FileType.R2)
            {
                _manualReviewLog = string.Format("{0}{1}", DataAccessBase.PersonalDataDirectory, MANUAL_REVIEW_LOG_R2);
            }
            else
            {
                _manualReviewLog = string.Format("{0}{1}", DataAccessBase.PersonalDataDirectory, MANUAL_REVIEW_LOG_R3);
            }
        }

        /// <summary>
        /// Main starting point for processing.
        /// </summary>
        public void Process()
        {
            VbaStyleFileOpen(_fileToProcess, 1, Common.MSOpenMode.Input);
            string headerRow = VbaStyleFileLineInput(1); //header row
            FileRecord record;
            while (VbaStyleEOF(1) == false)
            {
                record = new FileRecord();
                record.SuspenseAcctNum = VbaStyleFileInput(1);
                record.AccountID = VbaStyleFileInput(1);
                record.BorrowerName = VbaStyleFileInput(1);
                record.LoanType = VbaStyleFileInput(1);
                record.EffectiveDate = VbaStyleFileInput(1);
                record.Amount = VbaStyleFileInput(1);
                record.DealNum = VbaStyleFileInput(1);
                record.BatchNum = VbaStyleFileInput(1);
                record.TransactionType = VbaStyleFileInput(1);
                record.SeqNum = VbaStyleFileInput(1);
                record.AssignedTo = VbaStyleFileInput(1);
                record.Owner = VbaStyleFileInput(1);
                record.LoanSeqNumPmtTargeted = VbaStyleFileInput(1);
                if (TS26BrwTtlBalEquals0(record))
                {
                    ProcessSuspense(record);
                }
                else
                {
                    AddToManualReviewFile(record);
                }
            }
            VbaStyleFileClose(1);
        }

        //checks TS26 for BRWR TTL BAL amount
        private bool TS26BrwTtlBalEquals0(FileRecord record)
        {
            FastPath(string.Format("TX3ZITS26{0}",record.SuspenseAcctNum));
            if (Check4Text(1, 72, "TSX28")) //check for selection screen
            {
                //selection screen found
                PutText(21, 12, "01", ReflectionInterface.Key.Enter);
            }
            //on target screen
            return (double.Parse(GetText(10, 15, 12)) == 0);
        }

        //processes suspense
        private void ProcessSuspense(FileRecord record)
        {
            FastPath(string.Format("TX3ZDTS3I;;;S;;{0}", record.SuspenseAcctNum));
            if (Check4Text(1, 72, "TSX3G"))
            {
                //target screen
                if (DateTime.Parse(record.EffectiveDate) == DateTime.Parse(GetText(15, 25, 8).Replace(" ", @"/")) &&
                    double.Parse(record.Amount) == double.Parse(GetText(15, 12, 12)) &&
                    Check4Text(3, 23, record.BatchNum) && int.Parse(record.SeqNum) == int.Parse(GetText(3, 56, 7)))
                {
                    Hit(ReflectionInterface.Key.Enter); //delete record
                    if (Check4Text(23, 2, "01006 RECORD SUCCESSFULLY DELETED") == false)
                    {
                        //wasn't able to delete
                        UpdateErrorLog(record, "Unable to delete suspense record");
                    }
                    else
                    {
                        //add comments
                        AddComments(record);
                        UpdateEDLog(record);
                    }
                }
                else
                {
                    //if not correct record then add to error log
                    UpdateErrorLog(record, "Unable to find suspense record");
                }
            }
            else
            {
                //selection screen
                int row = 6;
                while (Check4Text(23, 3, "90007 NO MORE DATA TO DISPLAY") == false)
                {
                    if (DateTime.Parse(record.EffectiveDate) == DateTime.Parse(GetText(row, 42, 8)) &&
                    double.Parse(record.Amount) == double.Parse(GetText(row, 30, 12)) &&
                    Check4Text(row, 51, record.BatchNum) && int.Parse(record.SeqNum) == int.Parse(GetText(row, 66, 7)))
                    {
                        PutText(21, 18, GetText(row, 2, 3),ReflectionInterface.Key.Enter, true);
                        Hit(ReflectionInterface.Key.Enter); //delete record
                        if (Check4Text(23, 2, "01006 RECORD SUCCESSFULLY DELETED") == false)
                        {
                            //wasn't able to delete
                            UpdateErrorLog(record, "Unable to delete suspense record");
                        }
                        else
                        {
                            //add comments
                            AddComments(record);
                            UpdateEDLog(record);
                        }
                        return;
                    }
                    row = row + 2;
                    if (Check4Text(row, 3, " "))
                    {
                        Hit(ReflectionInterface.Key.F8);
                        row = 6;
                    }
                }
                //if not correct record then add to error log
                UpdateErrorLog(record, "Unable to find suspense record");
            }
        }

        //adds comments after suspense is successfully deleted
        private void AddComments(FileRecord record)
        {
            StringBuilder comment = new StringBuilder();
            comment.Append("Delete suspense payment ");
            if (record.AssignedTo.StartsWith("U"))
            {
                comment.AppendFormat("Effective {0} for {1}. Forward Pmt To Fedloan Serv {2}.", record.EffectiveDate, record.Amount, DateTime.Today.ToString("MM/dd/yyyy"));
            }
            else
            {
                comment.AppendFormat("{0} - Effective {1} for {2}. Forward Pmt To Fedloan Serv {3}.",record.AssignedTo,record.EffectiveDate,record.Amount,DateTime.Today.ToString("MM/dd/yyyy"));
            }
            if (ATD22FirstLoanOnly(record.SuspenseAcctNum,"SUSDE",comment.ToString()) == false)
            {
                //if comments don't take on TD22 then try again on TD37
                if (ATD37FirstLoan(record.SuspenseAcctNum, "SUSDE", comment.ToString(), _scriptID, false) != Common.CompassCommentScreenResults.CommentAddedSuccessfully)
                {
                    //if comments don't take on TD37 then try on LP50
                    AddCommentInLP50(record.SuspenseAcctNum, "SUSDE", _scriptID, "MS", "16", comment.ToString());
                }
            }
        }

        //adds data to manual review file
        private void AddToManualReviewFile(FileRecord record)
        {
            VbaStyleFileOpen(_manualReviewLog, 2, Common.MSOpenMode.Append);
            VbaStyleFileWriteLine(2, record.SuspenseAcctNum, record.AccountID, record.BorrowerName, record.LoanType, record.EffectiveDate, record.Amount, record.DealNum, record.BatchNum, record.TransactionType, record.SeqNum, record.AssignedTo, record.Owner, record.LoanSeqNumPmtTargeted);
            VbaStyleFileClose(2);
        }

        //updates error log with information and error condition
        private void UpdateErrorLog(FileRecord record, string errorMessage)
        {
            VbaStyleFileOpen(_errorLog, 2, Common.MSOpenMode.Append);
            VbaStyleFileWriteLine(2, record.SuspenseAcctNum, record.AccountID, record.BorrowerName, record.LoanType, record.EffectiveDate, record.Amount, record.DealNum, record.BatchNum, record.TransactionType, record.SeqNum, record.AssignedTo, record.Owner, record.LoanSeqNumPmtTargeted, errorMessage);
            VbaStyleFileClose(2);
        }

        //updates ED log with information
        private void UpdateEDLog(FileRecord record)
        {
            VbaStyleFileOpen(_edTransmittalLog, 2, Common.MSOpenMode.Append);
            VbaStyleFileWriteLine(2, record.SuspenseAcctNum, record.AccountID, record.BorrowerName, record.LoanType, record.EffectiveDate, record.Amount, record.DealNum, record.BatchNum, record.TransactionType, record.SeqNum, record.AssignedTo, record.Owner, record.LoanSeqNumPmtTargeted);
            VbaStyleFileClose(2);
        }

        private bool ATD22FirstLoanOnly(string ssn, string arc, string comment)
        {
            Coordinate coord = null;
            string userID = UserID;

            FastPath("TX3Z/ATD22" + ssn);
            if (Check4Text(1, 72, "TDX23") == false)
            {
                return false;
            }
            //find the ARC
            while (coord == null)
            {
                coord = FindText(arc);
                if (coord == null)
                {
                    Hit(ReflectionInterface.Key.F8);
                    if (Check4Text(23, 2, "90007"))
                    {
                        return false;
                    }
                }
            }
            //select the ARC
            PutText(coord.Row, coord.Column - 5, "01", ReflectionInterface.Key.Enter);
            //exit the function if the selection screen is not displayed
            if (Check4Text(1, 72, "TDX24") == false)
            {
                return false;
            }
            //select first loan
            PutText(11, 3, "X");
            //enter short comments
            if (comment.Length < 132)
            {
                PutText(21, 2, comment + string.Format("  {0}{1}{2} /{3}", "{", _scriptID, "}", userID));
                Hit(ReflectionInterface.Key.Enter);
                if (Check4Text(23, 2, "02860") == false)
                {
                    return false;   
                }
            }
            else //long comments
            {
                //fill the first screen
                PutText(21, 2, comment.SafeSubstring(1,154), ReflectionInterface.Key.Enter);
                if (Check4Text(23, 2, "02860") == false)
                {
                    return false;
                }
                Hit(ReflectionInterface.Key.F4);
                //enter the rest on the expanded comments screen
                for (int k = 155; k < comment.Length; k = k + 260)
                {
                    EnterText(comment.SafeSubstring(k,260));
                }
                EnterText("  {" + _scriptID + "} /" + userID);
                Hit(ReflectionInterface.Key.Enter);
                if (Check4Text(23, 2, "02114") == false)
                {
                    return false;   
                }
            }
            return true;
        }

    }
}
