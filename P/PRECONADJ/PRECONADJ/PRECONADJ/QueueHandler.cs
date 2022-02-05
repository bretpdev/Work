using System;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace PRECONADJ
{
    public class QueueHandler
    {
        public ReflectionInterface RI { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public bool FoundLoan { get; set; }

        public QueueHandler(ReflectionInterface ri, ProcessLogRun logRun)
        {
            RI = ri;
            LogRun = logRun;
        }

        /// <summary>
        /// Opens an existing queue task for the borrower selected in the openfile dialog
        /// </summary>
        /// <returns>true: queue found, false: queue not found</returns>
        public bool OpenQueueTask(string ssn, int loanSeq)
        {
            RI.FastPath($"TX3Z/ITX6T{ssn}");
            FoundLoan = false;
            if (RI.MessageCode != "01020")
            {
                var settings = PageHelper.IterationSettings.Default();
                settings.MinRow = 7;
                settings.MaxRow = 17;
                Queues q = GetQueue();
                PageHelper.Iterate(RI, (row, s) =>
                {
                    int seq = RI.GetText(row, 49, 4).ToInt();
                    if (RI.CheckForText(row, 8, q.Queue) && RI.CheckForText(row, 24, q.SubQueue) && loanSeq == seq)
                    {
                        RI.PutText(21, 18, RI.GetText(row, 3, 1)); //Select the row with the matching Queue/Subqueue
                        RI.Hit(ReflectionInterface.Key.Enter);
                        settings.ContinueIterating = false;
                        FoundLoan = true;
                    }
                    settings.RowIncrementValue = 5;
                }, settings);
            }
            if (!FoundLoan)
            {
                if (Dialog.Info.YesNo($"There are no pre-conversion adjustment queue tasks for loan sequence {loanSeq}. Would you like to process adjustments from this spreadsheet anyway?", "Queue Task Not Found"))
                    return true;
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Closes an open existing queue task for the borrower selected in the openfile dialog
        /// </summary>
        /// <returns>true: queue closed, false: queue not closed</returns>
        public bool CloseQueueTask(string ssn)
        {
            Queues q = GetQueue();
            RI.FastPath($"TX3Z/ITX6T{ssn}");
            int row = 8;
            if (RI.CheckForText(row, 76, "W")) //We assume that the first task is the one being worked
            {
                RI.PutText(21, 18, "1"); //Select the row with the matching SSN
                //press f2 to open the screen to complete the task
                RI.Hit(ReflectionInterface.Key.F2);
                if (RI.MessageCode != "01022") //if there is no success code error
                {
                    Dialog.Warning.Ok($"Unable to close queue, please close the assigned queue manually and then press ok to continue. queue: {q.Queue} subqueue: {q.SubQueue}", "Can Not Close Queue");
                    return false;
                }
                RI.PutText(8, 19, "C"); //Mark the task as completed
                RI.Hit(ReflectionInterface.Key.Enter); //hit enter to confirm the task as closed
                if (RI.MessageCode != "01005") //Try adding COMPL message if the session does not close the queue on the first try.
                {
                    RI.PutText(9, 19, "COMPL"); //Add a COMPL comment
                    RI.Hit(ReflectionInterface.Key.Enter); //hit enter to confirm the task as closed
                    if (RI.MessageCode != "01005")
                    {
                        Dialog.Warning.Ok($"Unable to close queue, please close the assigned queue manually and then press ok to continue. queue: {q.Queue} subqueue: {q.SubQueue}", "Can Not Close Queue");
                        return false;
                    }
                }
                return true;
            }
            else
            {
                Dialog.Warning.Ok($"Unable to close queue, please close the assigned queue manually and then press ok to continue. queue: {q.Queue} subqueue: {q.SubQueue}", "Can Not Close Queue");
                return false;
            }
        }

        /// <summary>
        /// Unassigns a queue task from the user if one exists for the current queue
        /// </summary>
        public void UnassignOpenQueueTask()
        {
            RI.FastPath("TX3Z/CTX6J");
            Queues q = GetQueue();
            RI.PutText(7, 42, q.Queue);
            RI.PutText(8, 42, q.SubQueue);
            RI.PutText(9, 42, "", ReflectionInterface.Key.EndKey);
            RI.PutText(12, 42, "W");
            RI.PutText(13, 42, RI.UserId);
            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.MessageCode == "01020") //No records found for search criteria, nothing to unassign
                return;
            if (RI.MessageCode == "01022") //Make desired changes for search criteria, nothing to unassign
            {
                RI.PutText(8, 15, "", ReflectionInterface.Key.EndKey);
                RI.Hit(ReflectionInterface.Key.Enter);
                if (RI.MessageCode == "01005") //Record successfully changed
                    return;
                else
                {
                    Dialog.Warning.Ok($"Unable to unassign queue from user, please unassign queue manually and then press ok to continue. queue: {q.Queue} subqueue: {q.SubQueue}", "Can Not Reassign Queue");
                    return;
                }
            }
        }

        /// <summary>
        /// Determines which queue to use according to the region
        /// </summary>
        public Queues GetQueue()
        {
            Queues q = new Queues();
            q.Queue = "SZ";
            q.SubQueue = "01";
            return q;
        }

        /// <summary>
        /// Enters the queue if one is assinged to the user.
        /// </summary>
        public bool CheckQueueAccess()
        {
            Queues q = GetQueue();
            RI.FastPath($"TX3Z/ITX6X{q.Queue};{q.SubQueue}");

            if (RI.MessageCode == "80014")
            {
                Dialog.Error.Ok($"User Id not assigned to queue: {q.Queue} subqueue: {q.SubQueue}. Please contact Systems Support.", "No Queue Access");
                return false;
            }
            return true;
        }

    }
}