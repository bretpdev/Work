using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace WORKRAQUE
{
    public class WorkRAQueueTask : ScriptBase
    {
        private List<string> ProcessedBorrowers;
        private string Ssn;
        private Borrower Bor;
        private bool Skip;

        public WorkRAQueueTask(ReflectionInterface ri)
            : base(ri, "WORKRAQUE", Uheaa.Common.DataAccess.DataAccessHelper.Region.Uheaa)
        {
            ProcessedBorrowers = new List<string>();
        }

        public override void Main()
        {
            if (DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                ProcessQueues();
        }

        /// <summary>
        /// Loops through each RA queue to process.
        /// </summary>
        private void ProcessQueues()
        {
            while (GetNextRaQueue())
            {
                GatherBorrowerInfo();
                if (Bor.DisbursmentDate != null && Bor.LoanProgram.IsPopulated())
                {
                    RunCTS650();
                    if (!Skip)
                        AddComment();
                }
                if (!Skip)
                {
                    CloseQueue("RA");
                    CloseQueue("R7");
                }
            }
            ProcessComplete();
        }

        /// <summary>
        /// Gets the next RA queue to work
        /// </summary>
        private bool GetNextRaQueue()
        {
            Bor = new Borrower();
            RI.FastPath("TX3ZITX6XRA");
            if (!CheckForText(1, 74, "TXX71"))
                return false;

            while (RI.Message != "90007")
            {
                for (int i = 9; i <= 18; i += 3)
                {
                    if (RI.CheckForText(i, 2, "NEW INFO CONFLICTS WITH RELEASED AND PRE-RELEASED BRWR CODES")
                        || RI.CheckForText(i, 2, "NEW INFORMATION CONFLICTS WITH RELEASED BORROWER CODES"))
                    {
                        string borSsn = RI.GetText(i - 1, 6, 9);
                        if (!borSsn.IsIn(ProcessedBorrowers.ToArray())) //Check to see if the borrower has been processed
                        {
                            Ssn = borSsn;
                            return true;
                        }
                    }
                }

                RI.Hit(ReflectionInterface.Key.F8);
                if (RI.MessageCode == "90007")
                {
                    RI.Hit(ReflectionInterface.Key.Enter); //Go to next page
                    if (RI.MessageCode == "01027")
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Gathers all the borrower information to process queue.
        /// </summary>
        private void GatherBorrowerInfo()
        {
            DateTime oldestDate = DateTime.Now.Date;
            RI.FastPath("LG02I" + Ssn);
            if (RI.CheckForText(22, 3, "47004")) //No valid disbursment date
                InvalidDate();
            else
            {
                if (RI.CheckForText(1, 75, "SELECT")) //Enter the first loan
                    RI.PutText(21, 13, "01", ReflectionInterface.Key.Enter);

                GetOldestDate();
            }
        }

        /// <summary>
        /// Gets the oldest disbursement date from LG02
        /// </summary>
        private void GetOldestDate()
        {
            DateTime compareDate = new DateTime();
            DateTime oldestDate = DateTime.Now.Date;
            string compareUniqueId;
            string compareLoanType;
            DateTime? cDate = new DateTime();
            while (!RI.CheckForText(22, 3, "46004"))
            {
                if (CheckPrivateLoan(""))
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }
                compareDate = RI.GetText(5, 10, 8).Insert(2, "/").Insert(5, "/").ToDate();
                compareUniqueId = RI.GetText(3, 32, 19);
                string tempType = RI.GetText(4, 59, 6);
                compareLoanType = tempType == "ORD" ? "SUB" : tempType;
                bool datePopulated = DatePopulated();
                if (compareDate < oldestDate && (RI.CheckForText(4, 9, "APPROVED") || RI.CheckForText(4, 10, "APPROVED")) && datePopulated)
                {
                    oldestDate = compareDate;
                    Bor.UniqueId = compareUniqueId;
                    Bor.LoanType = compareLoanType;
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
            if (!DatePopulated()) //Process log if date invalid
            {
                InvalidDate();
                return;
            }
            if (Bor.DisbursmentDate >= new DateTime(1993, 7, 1))
                cDate = GetCommonDate(); //Get date from TS26
            else
                cDate = Bor.DisbursmentDate.Value;

            if (cDate < Bor.DisbursmentDate) //Set the earliest date
                Bor.DisbursmentDate = cDate;
        }

        /// <summary>
        /// Checks the loan program to see if it is listed as a private loan
        /// </summary>
        private bool CheckPrivateLoan(string loanPgm)
        {
            bool privateLoan = true;
            string loanType = loanPgm;
            if (loanType.IsNullOrEmpty())//Called from LG02
            {
                if (RI.CheckForText(24, 45, "LG0H"))
                    RI.Hit(ReflectionInterface.Key.F10);
                else
                    RI.Hit(ReflectionInterface.Key.F11);
                loanType = RI.GetText(5, 10, 5);
            }
            if (loanType.IsIn(DataAccess.GetLoanTypes().ToArray()))
                privateLoan = false; //Loan program was in the list of FFEL loans
            if (loanPgm.IsNullOrEmpty())//Called from LG02
                RI.Hit(ReflectionInterface.Key.F12);
            return privateLoan;
        }

        /// <summary>
        /// Process the borrower queue from the CTS5O screen
        /// </summary>
        private bool RunCTS650()
        {
            RI.FastPath("TX3Z/CTS5O");
            if (RI.MessageCode == "02833") //There are no released loans to process
                return false;
            if (RI.ScreenCode == "TSX5N")
            {
                if (RI.GetText(6, 16, 6).IsIn(new List<string>() { "SUBCNS", "UNCNS", "SUBSPC", "UNSPC" }.ToArray()))
                {
                    if (!TargetScreen())
                        return false;
                }
                if (!CheckLoanSequence())
                    return false;
            }
            else
            {
                if (!CheckIfConsolLoans())
                    return false;
                bool overrideCode = GetBorrowerCode() == "921";
                if (!SelectionScreen(overrideCode))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check to see if the date is provided
        /// </summary>
        private bool DatePopulated()
        {
            if (RI.CheckForText(24, 45, "LG0H"))
                RI.Hit(ReflectionInterface.Key.F10);
            else
                RI.Hit(ReflectionInterface.Key.F11);
            if (RI.CheckForText(12, 20, "MMDDCCYY") || RI.CheckForText(12, 20, "    "))
            {
                RI.Hit(ReflectionInterface.Key.F12);
                return false;
            }
            else
            {
                Bor.LoanProgram = GetLoanProgram();
                if (Bor.LoanProgram == "")
                {
                    RI.Hit(ReflectionInterface.Key.F12);
                    return false;
                }
                Bor.DisbursmentDate = RI.GetText(12, 20, 8).Insert(2, "/").Insert(5, "/").ToDate();
                RI.Hit(ReflectionInterface.Key.F12);
                return true;
            }
        }

        /// <summary>
        /// Converts the OneLink loan program to a Compass loan program
        /// </summary>
        private string GetLoanProgram()
        {
            switch (RI.GetText(5, 10, 2))
            {
                case "SF":
                    return "STFFRD";
                case "SU":
                    return "UNSTFD";
                case "PL":
                    return "PLUS";
                case "GB":
                    return "PLUSGB";
                case "SL":
                    return "SLS";
                default:
                    ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, string.Format("Invalid Loan Program for borrower: {0}", Ssn), NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return "";
            }
        }

        /// <summary>
        /// Gets the disbursment date from TS26 if the date is later than 7/1/93
        /// </summary>
        private DateTime? GetCommonDate()
        {
            RI.FastPath("TX3Z/ITS26" + Ssn);
            DateTime compareDate = DateTime.Now.Date;
            DateTime? cDate = new DateTime();
            if (RI.ScreenCode == "TSX29") 
            {
                if (RI.GetText(6, 18, 8).ToDateNullable().Value < compareDate)
                    compareDate = RI.GetText(6, 18, 8).ToDate();
            }
            else
            {
                int row = 8;
                while (RI.MessageCode != "90007")
                {
                    cDate = RI.GetText(row, 5, 8).ToDateNullable();
                    if (cDate.Value < compareDate)
                    {
                        compareDate = cDate.Value;
                        Bor.LoanType = RI.GetText(row, 19, 6);
                    }
                    row++;
                    if (row == 20 || RI.CheckForText(row, 2, "  "))
                    {
                        row = 8;
                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                }
            }
            return compareDate;
        }

        /// <summary>
        /// Adds a message to the process logger indicating the date is not valid
        /// </summary>
        private void InvalidDate()
        {
            string message = string.Format("Invalid disbursement date for SSN: {0}, Loan Type {1}, Unique ID {2}", Ssn, Bor.LoanType, Bor.UniqueId);
            ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
        }

        /// <summary>
        /// Checks if the loan is an open consol and closes the queue if it is not.
        /// </summary>
        private bool TargetScreen()
        {
            if (!OpenNonConsolLoans())
            {
                CloseQueue("RA");
                CloseQueue("R7");
                ProcessedBorrowers.Add(Ssn);
                Skip = true;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks to see if there are any open non-consol loans with a balance
        /// </summary>
        private bool OpenNonConsolLoans()
        {
            string[] loanPrograms = { "PLUS", "PLUSGB", "SLS", "STFFRD", "UNSTFD" };
            RI.FastPath("TX3Z/ITS26");
            int row = 8;
            if (RI.ScreenCode == "TSX28")
            {
                while (RI.MessageCode != "90007")
                {
                    string screenText = RI.GetText(row, 19, 6);
                    if (screenText.IsIn(loanPrograms) && !RI.CheckForText(row, 64, "0.00"))
                        return true;
                    row++;
                    if (row == 21)
                        RI.Hit(ReflectionInterface.Key.F8);
                }
            }
            else
            {
                string screenText = RI.GetText(6, 66, 6);
                if (screenText.IsIn(loanPrograms) && !RI.CheckForText(11, 17, "0.00"))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Closes the queue task
        /// </summary>
        private void CloseQueue(string queue)
        {
            RI.FastPath("TX3Z/ITX6T" + Ssn);
            if (RI.MessageCode == "01020") //No records found in queue
                return;
            int row1 = 7;
            int row2 = 7;
            while (RI.MessageCode != "90007")
            {
                if (RI.MessageCode == "01020")
                    return;
                if (row1 > 15 || RI.CheckForText(row1, 3, " "))
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row1 = 7;
                }
                if (RI.CheckForText(row1, 8, queue))
                {
                    RI.PutText(21, 18, RI.GetText(row1, 3, 1), ReflectionInterface.Key.Enter);
                    RI.FastPath("TX3Z/ITX6T" + Ssn);
                    while (RI.MessageCode != "90007")
                    {
                        if (row2 >= 15 || RI.CheckForText(row2, 3, " "))
                        {
                            RI.Hit(ReflectionInterface.Key.F8);
                            row2 = 7;
                        }
                        if (RI.CheckForText(row2, 8, queue) && RI.CheckForText(row2 + 1, 76, "W"))
                        {
                            RI.PutText(21, 18, RI.GetText(row2, 2, 2), ReflectionInterface.Key.F2);
                            Thread.Sleep(1000);
                            RI.PutText(8, 19, "C");
                            RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter, true);
                            if (RI.MessageCode == "01644")
                                RI.PutText(9, 19, "", ReflectionInterface.Key.Enter, true);
                            Thread.Sleep(1000);
                            RI.Hit(ReflectionInterface.Key.F12);
                            RI.FastPath("TX3Z/ITX6T" + Ssn);
                            break;
                        }
                        row2 += 5;
                    }
                }
                else
                    row1 += 5;
            }
        }

        /// <summary>
        /// Gather the loan sequences and update the borrower code
        /// </summary>
        private bool CheckLoanSequence()
        {
            RI.FastPath("TX3Z/CTS5O");
            string borrowerCode = RI.GetText(12, 21, 3);
            string loanProgram = RI.GetText(6, 16, 6);
            if (CheckPrivateLoan(Bor.LoanProgram))
                return false;
            DateTime? disbDate = RI.GetText(10, 4, 8).Replace(" ", "/").ToDateNullable();
            RI.FastPath("TX3Z/ITS26" + Ssn);
            if (RI.ScreenCode == "TSX29")
                Bor.LoanSequence.Add(RI.GetText(7, 35, 4).ToInt());
            else
            {
                int i = 8;
                while (RI.MessageCode != "90007")
                {
                    if (RI.CheckForText(i, 5, Bor.DisbursmentDate.Value.ToString("MM/dd/yyyy")) && RI.CheckForText(i, 19, Bor.LoanProgram))
                    {
                        Bor.LoanSequence.Add(RI.GetText(i, 14, 4).ToInt());
                        break;
                    }
                    if (i == 20)
                        RI.Hit(ReflectionInterface.Key.F8);
                }
            }
            if (RI.MessageCode == "90007")
            {
                string message = string.Format("The loan sequence number was not found for borrower: {0}", Ssn);
                Dialog.Warning.Ok(message);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return false;
            }
            RI.FastPath("TX3Z/CTS5O");
            string newBorCode = GetBorrowerCode();
            if (newBorCode.IsPopulated() && borrowerCode != newBorCode)
                UpdateBorrowerCode(newBorCode, borrowerCode);
            return true;
        }

        /// <summary>
        /// Get the calculated borrower code depending on the loan program and the disbursement date
        /// </summary>
        private string GetBorrowerCode()
        {
            if (Bor.LoanProgram == "STFFRD" || Bor.LoanProgram == "UNSTFD")
            {
                if (Bor.DisbursmentDate >= new DateTime(1988, 7, 1) && Bor.DisbursmentDate < new DateTime(1992, 7, 23))
                    return "881";
                else if (Bor.DisbursmentDate >= new DateTime(1992, 7, 23) && Bor.DisbursmentDate < new DateTime(1992, 10, 1))
                    return "921";
                else if (Bor.DisbursmentDate >= new DateTime(1992, 10, 1) && Bor.DisbursmentDate < new DateTime(1993, 7, 1))
                    return "922";
                else if (Bor.DisbursmentDate >= new DateTime(1993, 7, 1))
                    return "931";
            }
            else if (Bor.LoanProgram == "PLUSGB")
            {
                if (Bor.DisbursmentDate >= new DateTime(2006, 7, 1))
                    return "206";
            }
            else if (Bor.LoanProgram == "PLUS" || Bor.LoanProgram == "SLS")
            {
                if (Bor.DisbursmentDate >= new DateTime(1987, 7, 1) && Bor.DisbursmentDate < new DateTime(1993, 7, 1))
                    return "871";
                else if (Bor.DisbursmentDate >= new DateTime(1993, 7, 1))
                    return "931";
            }
            return "";
        }

        /// <summary>
        /// Updates the borrower code
        /// </summary>
        private void UpdateBorrowerCode(string newBorCode, string borCode)
        {
            Bor.BorrowerCode.Add(borCode);
            Bor.CorrectedBorrowerCode.Add(newBorCode);
            if (RI.MessageCode == "02834")
                return;

            RI.PutText(12, 21, newBorCode, ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6);
            if (RI.ScreenCode != "TSX9Q")
            {
                string message = string.Format("Invalid disbursement date for borrower: {0}", Ssn);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                RI.Hit(ReflectionInterface.Key.F12);
            }
            string correctedMessage = string.Format("New borrower code corrected based on first disb date of oldest loan. Code changed from {0} to {1}.", borCode, newBorCode);
            RI.PutText(13, 2, correctedMessage, ReflectionInterface.Key.Enter);
        }

        /// <summary>
        /// Check if there are open consol loans
        /// </summary>
        private bool CheckIfConsolLoans()
        {
            string[] consolLoans = { "SUBCNS", "UNCNS", "SUBSPC", "UNSPC" };
            int row = 7;
            while (RI.MessageCode != "90007")
            {
                if (RI.GetText(row, 8, 6).IsIn(consolLoans))
                    if (!TargetScreen())
                        return false;
                row++;
                if (row == 20 || RI.CheckForText(row, 8, "  "))
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 7;
                }
            }
            return true;
        }

        /// <summary>
        /// Process the RA queue from the selection screen
        /// </summary>
        private bool SelectionScreen(bool overrideCode)
        {
            RI.FastPath("TX3Z/CTS5O");
            int row = 7;
            string[] loans = { "SUBCNS", "UNCNS", "SUBSPC", "UNSPC" };
            while (RI.MessageCode != "90007")
            {
                if (!RI.GetText(row, 8, 6).IsIn(loans))
                {
                    string borrowerCode = RI.GetText(row, 62, 3);
                    string loanProgram = RI.GetText(row, 8, 6);
                    if (CheckPrivateLoan(Bor.LoanProgram))
                        return false;
                    string newBorCode = GetNewBorrowerCode(overrideCode, row);
                    CheckPriorDisbursements(borrowerCode);
                    if (newBorCode != "" && borrowerCode != newBorCode) //If the borrower codes don't match, update the code
                    {
                        Bor.BorrowerCode.Add(borrowerCode);
                        Bor.CorrectedBorrowerCode.Add(newBorCode);
                        Bor.LoanSequence.Add(RI.GetText(row, 20, 4).ToInt());
                        RI.PutText(22, 19, RI.GetText(row, 3, 2), ReflectionInterface.Key.Enter, true);
                        UpdateBorrowerCode(newBorCode, borrowerCode);
                        return true;
                    }
                }
                row++;
                if (RI.CheckForText(row, 8, "  ") || row == 20)
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 7;
                }
            }
            return true;
        }

        /// <summary>
        /// Check to see if the borrower had prior disbursements and add to process logger.
        /// </summary>
        private void CheckPriorDisbursements(string borrowerCode)
        {
            if (((Bor.LoanProgram == "STFFRD" || Bor.LoanProgram == "UNSTFD") && Bor.DisbursmentDate < new DateTime(1998, 7, 1)) ||
                ((Bor.LoanProgram == "PLUS" || Bor.LoanProgram == "SLS") && Bor.DisbursmentDate < new DateTime(1987, 7, 1)))
            {
                string message = string.Format("The borrower {0} has a disbursement prior to 7/1/1988 for STFFRD or UNSTFD or prior to 7/1/1978 for PLUS or SLS; Loan Program: {1}, Disbursement Date: {2}, Borrower Code = {3}", Ssn, Bor.LoanProgram, Bor.DisbursmentDate, borrowerCode);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
        }

        /// <summary>
        /// Determines the new borrower code
        /// </summary>
        private string GetNewBorrowerCode(bool overrideCode, int row)
        {
            string newBorCode;
            if (overrideCode)
            {
                if (RI.GetText(row, 49, 8).ToDate() == Bor.DisbursmentDate)
                    newBorCode = "921";
                else
                    newBorCode = "821";
            }
            else
                newBorCode = GetBorrowerCode();
            return newBorCode;
        }

        /// <summary>
        /// Adds a comment to the borrower account updating the corrected borrower code.
        /// </summary>
        private void AddComment()
        {
            if (Bor.LoanSequence.Count > 0 && Bor.BorrowerCode.Count > 0 && Bor.CorrectedBorrowerCode.Count > 0)
            {
                string comment = "New borrower code corrected  based on first disb date of oldest loan for";
                for (int i = 0; i < Bor.LoanSequence.Count; i++)
                {
                    comment += string.Format(", Loan Sequence # {0}. Code changed from {1} to {2}.", Bor.LoanSequence[i], Bor.BorrowerCode[i], Bor.CorrectedBorrowerCode[i]);
                }
                RI.Atd22ByLoan(Ssn, "RVBRC", comment, "", Bor.LoanSequence, ScriptId, false);
            }
        }

        /// <summary>
        /// Close the proecss logger and end script
        /// </summary>
        private void ProcessComplete()
        {
            ProcessLogger.LogEnd(ProcessLogData.ProcessLogId);
            Dialog.Info.Ok("Processing Complete", "Processing Complete");
        }
    }
}