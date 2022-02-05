using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace SOPRMCRD
{
    public class StateOffsetUpdate
    {
        private static ReflectionInterface RI { get; set; }

        public StateOffsetUpdate(ReflectionInterface ri)
        {
            RI = ri;
        }

        public int Process()
        {
            try
            {
                RI.FastPath("LP9ACDSTPARMU");
                int queueCount = CheckForQueue();
                if (queueCount == Program.ERROR)
                    return Program.ERROR;
                else if (queueCount == 2)
                    return Program.SUCCESS;

                ProcessQueues();
                WriteLine("");
                WriteLine("Processing Complete");
                return Program.SUCCESS;
            }
            catch (Exception ex)
            {
                string message = $"There was an error in processing the DSTPARMU Queues. EX: {ex.Message}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                WriteLine(message);
                return Program.ERROR;
            }
        }

        /// <summary>
        /// Check to see if there are tasks ready or if the user is assigned to another task
        /// </summary>
        private int CheckForQueue()
        {
            if (RI.CheckForText(1, 66, "QUEUE SELECTION"))
            {
                WriteLine("There are no tasks in the \"DSTPARMU\" queue.");
                return 2; //Return anything other than 0 or 1 to let it know that there are no queues to process
            }


            if (!RI.CheckForText(3, 24, "DSTPARMU"))
            {
                string message = "This User Id is already assigned to another queue task. Please complete the task and try again.";
                WriteLine(message);
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return Program.ERROR;
            }
            return Program.SUCCESS;
        }

        /// <summary>
        /// Get the borrower demos from the DSTPARMU queue and update the state offset notification in LP4C
        /// </summary>
        private void ProcessQueues()
        {
            do
            {
                WriteLine("");
                List<string> tasks = $"{RI.GetText(12, 11, 61)}{RI.GetText(13, 11, 61)}{RI.GetText(14, 11, 61)}".Replace("_", "").SplitAndRemoveQuotes(",");
                BorrowerRecord bor = LoadBorrower(tasks);
                DoLP4C(bor);
                AddComment(bor);
                RI.FastPath("LP9AC");
                RI.Hit(F6);
                VerifyQueueClosed(bor);
                RI.Hit(F8);
            } while (RI.AltMessageCode != "46004");
        }

        /// <summary>
        /// Loads borrower object with data ready from the screen after removing the UT ID.
        /// </summary>
        private BorrowerRecord LoadBorrower(List<string> tasks)
        {
            tasks[tasks.Count - 1] = tasks.Last().Substring(0, tasks[tasks.Count - 1].Length - 10);
            bool borType = tasks.First().StartsWith("B");
            BorrowerRecord bor = new BorrowerRecord()
            {
                Ssn = RI.GetText(17, 70, 9),
                RecordType = tasks[0],
                Address1 = borType ? tasks[1] : tasks[4],
                Address2 = borType ? tasks[2] : tasks[5],
                City = borType ? tasks[3] : tasks[6],
                State = borType ? tasks[4] : tasks[7],
                Zip = borType ? tasks[5] : tasks[8],
                FirstName = borType ? "" : tasks[1],
                MI = borType ? "" : tasks[2],
                LastName = borType ? "" : tasks[3],
                IsBorrower = borType
            };
            RI.FastPath($"LP22I{bor.Ssn}");
            bor.AccountNumber = RI.GetText(3, 60, 12).Replace(" ", "");
            WriteLine($"Loading borrower account: {bor.AccountNumber} to process.");
            return bor;
        }

        /// <summary>
        /// Determine if the records is a borrower or spouse
        /// </summary>
        private void DoLP4C(BorrowerRecord bor)
        {
            if (bor.IsBorrower)
                ProcessBorrower(bor);
            else
                ProcessSpouse(bor);
        }

        /// <summary>
        /// Add parm card update for borrower
        /// </summary>
        private void ProcessBorrower(BorrowerRecord bor)
        {
            WriteLine($"Adding LCXS2 State Offset for borrower: {bor.AccountNumber}");
            RI.FastPath("LP4CALCXS2;01");
            RI.PutText(7, 4, bor.Ssn);
            RI.PutText(7, 23, bor.Address1);
            RI.PutText(7, 42, bor.Address2, Enter);
            if (RI.AltMessageCode != "48003")
                LogError(bor, "address");

            RI.FastPath("LP4CALCXS2;02");
            RI.PutText(7, 4, bor.Ssn);
            RI.PutText(7, 23, bor.City);
            RI.PutText(7, 42, bor.State);
            RI.PutText(7, 61, bor.Zip, Enter);
            if (RI.AltMessageCode != "48003")
                LogError(bor, "city/state/zip");
        }

        /// <summary>
        /// Add parm card up for spouse
        /// </summary>
        private void ProcessSpouse(BorrowerRecord spouse)
        {
            RI.FastPath("LP4CALCXS2;03");
            RI.PutText(7, 4, spouse.Ssn);
            RI.PutText(7, 23, spouse.FirstName);
            RI.PutText(7, 42, spouse.MI);
            RI.PutText(7, 61, spouse.LastName, Enter);
            if (RI.AltMessageCode != "49004")
                LogError(spouse, "name");

            RI.FastPath("LP4CALCXS2;04");
            RI.PutText(7, 4, spouse.Ssn);
            RI.PutText(7, 23, spouse.Address1);
            RI.PutText(7, 42, spouse.Address2, Enter);
            if (RI.AltMessageCode != "49004")
                LogError(spouse, "address");

            RI.FastPath("LP4CALCXS2;05");
            RI.PutText(7, 4, spouse.Ssn);
            RI.PutText(7, 23, spouse.City);
            RI.PutText(7, 42, spouse.State);
            RI.PutText(7, 61, spouse.Zip, Enter);
            if (RI.AltMessageCode != "49004")
                LogError(spouse, "city/state/zip");
        }

        /// <summary>
        /// Add LP50 comment after adding parm card update
        /// </summary>
        private void AddComment(BorrowerRecord bor)
        {
            WriteLine($"Adding DPMOK ARC for borrower: {bor.AccountNumber}");
            string comment = "Parm card update with address for borrower and or spouse for state offset processed.";
            if (!RI.AddCommentInLP50(bor.Ssn, "AM", "10", "DPMOK", comment, Program.ScriptId))
            {
                string message = $"There was an error adding an LP50 comment for Borrower: {bor.AccountNumber}, ActionCode: DPMOK for Parm Card Update.";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                WriteLine(message);
            }
        }

        private int LogError(BorrowerRecord bor, string updatePart)
        {
            string message = $"Error adding borrower {updatePart} to LCXS2 state offset for borrower: {bor.AccountNumber}";
            WriteLine(message);
            RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            return Program.ERROR;
        }

        private void VerifyQueueClosed(BorrowerRecord bor)
        {
            if (RI.AltMessageCode == "47460")
            {
                string message = $"DSTPARMU queue was not closed for borrower: {bor.AccountNumber} because the activity record was not found for the task.";
                WriteLine(message);
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }
    }
}