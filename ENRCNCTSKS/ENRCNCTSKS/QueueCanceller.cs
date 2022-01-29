using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace ENRCNCTSKS
{
    public class QueueCanceller : BatchScriptBase
    {
        private readonly string CURRENT_USER;
        private readonly string ORS_MANAGER;
        private readonly string ERROR_FILE;
        private readonly string ERROR_FOLDER;
        private readonly bool TEST_MODE;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ri">Instance of Reflection Interface</param>
        public QueueCanceller(ReflectionInterface ri)
            : base(ri, "ENRCNCTSKS")
        {
            CURRENT_USER = GetUserIDFromLP40();
            ORS_MANAGER = DataAccess.GetOrsManagerUserId(ri.TestMode);
            ERROR_FILE = string.Format("Enrollment Cancel Invalid Queue Tasks {0}", DateTime.Now.ToString("MMddyyyy"));
            ERROR_FOLDER = TestMode(@"Q:\Account Services\Daily Reports\").DocFolder;
            TEST_MODE = ri.TestMode;
        }

        /// <summary>
        /// Script entry point
        /// </summary>
        public override void Main()
        {
            //Recover if needed.
            if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
            {
                string[] recoveryValues = Recovery.RecoveryValue.Split(',');
                ReviewLoans(recoveryValues[0], recoveryValues[1]);
            }
            ProcessQueue("LB");
            ProcessQueue("YL");
            ProcessingComplete(true);
        }//Main()

        private void ProcessQueue(string queue)
        {
            //Loop through all tasks in the queue.
            for (string ssn = FindQueueTaskOnTX6X(queue); !string.IsNullOrEmpty(ssn); ssn = FindQueueTaskOnTX6X(queue))
            {
                ReviewLoans(ssn, queue);
            }
        }//ProcessQueue()

        private string FindQueueTaskOnTX6X(string queue)
        {
            //Get the tasks for the selected queue.
            FastPath("TX3Z/ITX6X" + queue);
            if (Check4Text(23, 2, "01020")) { return null; }

            //Find the first task with STATUS = U.
            int row = 8;
            while (!Check4Text(row, 75, "U"))
            {
                row += 3;
                if (row > 17)
                {
                    Hit(Key.F8);
                    if (Check4Text(23, 2, "90007")) { return null; }
                    row = 8;
                }
            }
            //Get the SSN and DATE REQUESTED and select the task.
            string ssn = GetText(row, 6, 9);
            PutText(21, 18, GetText(row, 3, 2), Key.Enter);
            //Now that the task is open, write recovery values for this borrower/queue.
            Recovery.RecoveryValue = string.Format("{0},{1}", ssn, queue);
            return ssn;
        }//FindQueueTaskOnTX6X()

        private void ReviewLoans(string ssn, string queue)
        {
            FastPath("TX3Z/ITS26" + ssn);
            if (Check4Text(23, 2, "01527"))
            {
                //Borrower not found on system.
                if (TEST_MODE)
                {
                    //The test region has a problem assigning tasks, so close it instead.
                    CloseQueueTask(queue);
                }
                else
                {
                    AssignQueueTask(queue);
                }
                return;
            }
            if (AllLoansArePaidOrDeconverted())
            {
                CloseQueueTask(queue);
                return;
            }
            if (Check4Text(1, 72, "TSX28"))
            {
                //Selection screen. Get a list of the last disbursement date
                //for each loan, starting with the last selected loan.
                Hit(Key.Enter);
                List<DateTime> lastDisbursementDates = new List<DateTime>();
                do
                {
                    if (Check4Text(6, 66, "PLUS"))
                    {
                        if (TEST_MODE)
                        {
                            //The test region has a problem assigning tasks, so close it instead.
                            CloseQueueTask(queue);
                        }
                        else
                        {
                            AssignQueueTask(queue);
                        }
                        return;
                    }
                    else
                    {
                        if (!Check4Text(3, 10, "PAID IN FULL", "DECONVERTED"))
                        {
                            lastDisbursementDates.Add(GetLastDisbursementDate());
                        }

                    }
                } while (SelectNextLoan());
                CheckEstimatedGraduationDate(ssn, queue, lastDisbursementDates);
                return;
            }
            else if (Check4Text(1, 72, "TSX29"))
            {
                //Target screen.
                if (Check4Text(6, 66, "PLUS"))
                {
                    if (TEST_MODE)
                    {
                        //The test region has a problem assigning tasks, so close it instead.
                        CloseQueueTask(queue);
                    }
                    else
                    {
                        AssignQueueTask(queue);
                    }
                    return;
                }
                else
                {
                    List<DateTime> lastDisbursementDates = new List<DateTime>();
                    if (!Check4Text(3, 10, "PAID IN FULL", "DECONVERTED"))
                    {
                        lastDisbursementDates.Add(GetLastDisbursementDate());
                    }
                    CheckEstimatedGraduationDate(ssn, queue, lastDisbursementDates);
                    return;
                }
            }
        }//ReviewLoans()

        private bool AllLoansArePaidOrDeconverted()
        {
            //See if we're on a target screen and the loan is paid or deconverted.
            if (Check4Text(1, 72, "TSX29")) { return Check4Text(3, 10, "PAID IN FULL", "DECONVERTED"); }
            //Select each loan on the selection screen until we come across one that's not paid or deconverted.
            int row = 8;
            while (!Check4Text(23, 2, "90007"))
            {
                //Select current row and check the loan status.
                PutText(21, 12, GetText(row, 2, 2), Key.Enter);
                if (!Check4Text(3, 10, "PAID IN FULL", "DECONVERTED"))
                {
                    Hit(Key.F12);
                    return false;
                }
                //Back out to the selection screen and clear the selection entry.
                Hit(Key.F12);
                PutText(21, 12, "", Key.EndKey);
                //Move on to the next row, or the next page if needed.
                row++;
                if (Check4Text(row, 2, "  "))
                {
                    Hit(Key.F8);
                    row = 8;
                }
            }
            return true;
        }//AllLoansArePaidOrDeconverted()

        private bool SelectNextLoan()
        {
            //Make sure we're on the TSX28.
            if (Check4Text(1, 72, "TSX29")) { Hit(Key.F12); }
            if (!Check4Text(1, 72, "TSX28")) { throw new Exception("Failed to find screen TSX28."); }
            //Check the selection line to see what was the previously selected record number.
            int previousRecord = 0;
            if (!Check4Text(21, 12, "__"))
            {
                previousRecord = int.Parse(GetText(21, 12, 2));
            }
            //See if we've already selected every loan on this page.
            if (previousRecord == 12)
            {
                //Go to the next page.
                Hit(Key.F8);
                //Check for "NO MORE DATA TO DISPLAY"
                if (Check4Text(23, 2, "90007")) { return false; }
                previousRecord = 0;
            }
            //See if there is a next record.
            int nextRecord = previousRecord + 1;
            if (Check4Text(nextRecord + 7, 2, "  ")) { return false; }
            //Clear the selection line and select the next record.
            PutText(21, 12, "", Key.EndKey);
            PutText(21, 12, nextRecord.ToString(), Key.Enter);
            return true;
        }//SelectNextLoan()

        private DateTime GetLastDisbursementDate()
        {
            //Go to screen TSX2A.

            Hit(Key.Enter);
            if (!Check4Text(1, 72, "TSX2A")) { throw new Exception("Failed to find screen TSX2A."); }
            //Go to the disbursement page.
            Hit(Key.F7);
            if (!Check4Text(1, 72, "TSX2J")) { throw new Exception("Failed to find screen TSX2J."); }
            //Retrieve the last disbursement date listed.
            DateTime lastDisbursementDate;
            if (!Check4Text(15, 69, "  ")) { lastDisbursementDate = DateTime.Parse(GetText(15, 69, 8)); }
            else if (!Check4Text(15, 52, "  ")) { lastDisbursementDate = DateTime.Parse(GetText(15, 52, 8)); }
            else if (!Check4Text(15, 35, "  ")) { lastDisbursementDate = DateTime.Parse(GetText(15, 35, 8)); }
            else { lastDisbursementDate = DateTime.Parse(GetText(15, 19, 8)); }
            //Back out to TSX29 and return the last disbursement date.
            Hit(Key.F12);
            Hit(Key.F12);
            return lastDisbursementDate;
        }//GetLastDisbursementDate()

        private void CheckEstimatedGraduationDate(string ssn, string queue, List<DateTime> lastDisbursementsDates)
        {
            FastPath("LG29I" + ssn);

            DateTime estimatedGraduationDate;

            if (Check4Text(22, 3, "47004") )
            {
                AssignQueueTask(queue);
                return;
            }


            else if (Check4Text(1, 48, "Student Enrollment Status Display"))
            {
                estimatedGraduationDate = new DateTime
                (
                    int.Parse(GetText(10, 39, 4)),
                    int.Parse(GetText(10, 35, 2)),
                    int.Parse(GetText(10, 37, 2))
                );
            }

            else
            {

                //Find the first record with status A.
                int row = 9;
                while (!Check4Text(row, 10, "A"))
                {
                    row++;
                    if (row > 18)
                    {
                        row = 9;
                        Hit(Key.F8);
                        if (Check4Text(22, 3, "46004")) { throw new Exception("Could not find active status row."); }
                    }
                }
                //Compare the EGD to the disbursement dates.

                estimatedGraduationDate = new DateTime
                (
                    int.Parse(GetText(row, 44, 4)),
                    int.Parse(GetText(row, 40, 2)),
                    int.Parse(GetText(row, 42, 2))
                );
            }



            if (lastDisbursementsDates.Max() > estimatedGraduationDate)
            {
                if (TEST_MODE)
                {
                    //The test region has a problem assigning tasks, so close it instead.
                    CloseQueueTask(queue);
                }
                else
                {
                    AssignQueueTask(queue);
                }
            }
            else
            {
                CloseQueueTask(queue);
            }
        }//CheckEstimatedGraduationDate()

        private void AssignQueueTask(string queue)
        {
            FastPath("TX3Z/CTX6J;");
            PutText(7, 42, queue);
            PutText(13, 42, CURRENT_USER);
            Hit(Key.Enter);
            PutText(8, 15, ORS_MANAGER);
            Hit(Key.Enter);
            //Get out of recovery.
            Recovery.Delete();
        }//AssignQueueTask()

        private void CloseQueueTask(string queue)
        {
            FastPath("TX3Z/ITX6X" + queue);
            //Select the first task with STATUS = W.
            int row = 8;
            while (!Check4Text(row, 75, "W"))
            {
                row += 3;
                if (row > 17)
                {
                    Hit(Key.F8);
                    if (Check4Text(23, 2, "90007")) { return; }
                    row = 8;
                }
            }
            PutText(21, 18, GetText(row, 3, 2), Key.F2);
            //Close the task.
            PutText(8, 19, "C", Key.Enter);
            if (!Check4Text(23, 2, "01005"))
            {
                WriteError(GetText(12, 2, 9), queue);
            }
            //Get out of recovery.
            Recovery.Delete();
        }//CloseQueueTask()

        private void WriteError(string ssn, string queue)
        {
            string[] headers = { "Borrower SSN", "Queue name", "Message" };
            const string MESSAGE = "Error received while closing queue task.";
            bool fileIsNew = !File.Exists(ERROR_FOLDER + ERROR_FILE);
            using (StreamWriter errorWriter = new StreamWriter(ERROR_FOLDER + ERROR_FILE, true))
            {
                if (fileIsNew) { errorWriter.WriteCommaDelimitedLine(headers); }
                errorWriter.WriteCommaDelimitedLine(ssn, queue, MESSAGE);
                errorWriter.Close();
            }
        }//WriteError()
    }//class
}//namespace
