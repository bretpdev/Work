using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;

namespace ACURINTC
{
    abstract class TaskHelper
    {
        public General G { get; private set; }
        public ReflectionInterface RI { get; private set; }
        public DataAccess DA { get; private set; }
        public bool SkipTaskClose { get; private set; }
        public TaskHelper(General g, bool skipTaskClose)
        {
            this.G = g;
            this.RI = g.RI as ReflectionInterface;
            this.DA = g.DA;
            this.SkipTaskClose = skipTaskClose;
        }

        public abstract bool AddComment(QueueInfo data, PendingDemos task, SystemCode code, string arc, string comment);
        public bool CloseTask(QueueInfo queue, PendingDemos task)
        {
            if (!SkipTaskClose)
                return CloseTaskInternal(queue, task);
            return true;
        }
        public bool CloseTaskInternal(QueueInfo queue, PendingDemos task)
        {
            if (task.StatusInfo.TaskIsClosed)
                return true;

            if (!WorkTask(queue, task))
                return false;
            bool closedTask = false;
            //Go to the COMPASS queue screen for the given queue.
            if (task.Queue == "1E")
            {
                RI.FastPath($"TX3Z/ITX6X{task.Queue}{task.SubQueue}{task.TaskControlNumber}");
            }
            else
            {
                RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", task.Queue, task.SubQueue));
            }

            //Select the task that's in Working status.
            bool foundTask = false;

            if (RI.MessageCode == "01020")
                foundTask = true;
            else
            {
                var settings = PageHelper.IterationSettings.Default();
                settings.MinRow = 8;
                settings.RowIncrementValue = 3;
                PageHelper.Iterate(RI, (r, s) =>
                {
                    if (RI.CheckForText(r, 6, task.TaskControlNumber))
                    {
                        if (task.Queue == "1E")
                        {
                            RI.PutText(21, 18, RI.GetText(r, 3, 2), Key.Enter, true);
                            if (!VerifyF4())
                            {
                                foundTask = false;
                                closedTask = false;
                            }
                            else
                            {
                                foundTask = true;
                                closedTask = true;
                            }
                            s.ContinueIterating = false;
                        }
                        else
                        {
                            RI.PutText(21, 18, RI.GetText(r, 3, 2), Key.F2, true);
                            if (RI.CheckForText(1, 72, "TXX6S"))
                            {
                                s.ContinueIterating = false;
                                foundTask = true;
                            }
                        }
                    }
                });
            }
            if (!foundTask)
                closedTask = false;
            else
            {
                if (task.Queue != "1E")
                {
                    //Mark the task as Complete.
                    RI.PutText(8, 19, "C");
                    RI.PutText(9, 19, "COMPL");
                    RI.Hit(Key.Enter);
                    if (RI.CheckForText(23, 2, "01644"))
                        RI.PutText(9, 19, "", Key.Enter, true);
                    closedTask = RI.CheckForText(23, 2, "01005");
                }
            }

            task.StatusInfo.TaskIsClosed = closedTask;
            return closedTask;
        }

        public bool WorkTask(QueueInfo data, PendingDemos task)
        {
            RI.FastPath("TX3Z/CTX6J");
            RI.PutText(7, 42, data.Queue);
            RI.PutText(8, 42, data.SubQueue);
            RI.PutText(9, 42, task.TaskControlNumber, true);
            RI.PutText(9, 76, "A"); //type code
            RI.Hit(Key.Enter);
            if (RI.MessageCode == "01020") //The task was already worked and is no longer open and cannot be re-assigned
                return true;
            RI.PutText(8, 15, G.UserId);
            RI.Hit(Key.Enter);
            if (!RI.CheckForText(23, 2, "01005"))
                return false;
            return true;
        }

        private bool VerifyF4()
        {
            int count = 0;
            while (RI.MessageCode != "01867")
            {
                RI.Hit(Key.F4);
                if (RI.MessageCode != "01867")
                    Thread.Sleep(1000);
                else
                    return true;
                count++;
                if (count == 5)
                    return false;
            }
            return true;
        }

        public bool ReassignTask(QueueInfo queueData, PendingDemos task, string comment)
        {
            string toUserId = DA.LoanServicingManagerId(G.LogRun);
            //Open up the task we're currently working.
            RI.FastPath("TX3Z/CTX6J");
            RI.PutText(7, 42, queueData.Queue);
            RI.PutText(8, 42, queueData.SubQueue);
            RI.PutText(9, 42, task.TaskControlNumber, true);
            RI.PutText(9, 76, "A"); //type code
            RI.Hit(Key.Enter);
            if (RI.MessageCode == "01020") //The task was already worked and is no longer open and cannot be re-assigned
                return true;
            //Assign it to someone else.
            RI.PutText(8, 15, toUserId);
            RI.Hit(Key.Enter);
            if (!RI.CheckForText(23, 2, "01005"))
            {
                string message = string.Format("Error working task in {0}{1}. Re-start the script to finish processing after the error has been resolved.", queueData.Queue, queueData.SubQueue);
                G.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }

            string demographicsSource = G.DSH.GetDemographicsSourceName(task.DemographicsSourceId);
            ArcData ad = new ArcData(DataAccessHelper.CurrentRegion)
            {
                Arc = "KNOTE",
                Comment = comment,
                ScriptId = G.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                AccountNumber = task.AccountNumber
            };
            if (!ad.AddArc().ArcAdded)
            {
                if (!RI.Atd37FirstLoan(task.Ssn, "KNOTE", comment, G.ScriptId, G.UserId, null))
                    G.LogRun.AddNotification(string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.AccountNumber, queueData.DemographicsReviewQueue, task.OriginalAddressText, comment), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            task.StatusInfo.TaskIsClosed = true;
            return true;
        }
        public abstract bool CreateTask(PendingDemos task, string newQueue, string commentIntro);
        public abstract bool BorrowerHasForeignNumber(string ssnOrAccountNumber);
        public abstract bool BorrowerHasForeignAddress(string ssnOrAccountNumber);
        public abstract bool AddressMatches(PendingDemos task);
        public void CloseOrReassignTask(QueueInfo data, PendingDemos task, string comment)
        {
            if (!CloseTask(data, task))
                ReassignTask(data, task, comment);
        }

        public void ProcessLocate(PendingDemos task, SystemCode code)
        {
            //See if the borrower can be a locate based off the current validity indicators and dates.
            SystemBorrowerDemographics lp22Demographics = null;
            try
            {
                lp22Demographics = RI.GetDemographicsFromLP22(task.Ssn);
            }
            catch (DemographicException)
            {
                G.LogRun.AddNotification(string.Format("Unable to locate borrower {0} for ProcessQueueId {1}", task.Ssn, task.ProcessQueueId), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }
            if ((!lp22Demographics.IsValidAddress || lp22Demographics.AddressValidityDate.ToDateNullable() != DateTime.Now.Date) && (!lp22Demographics.IsPrimaryPhoneValid || lp22Demographics.PhoneValidityDate.ToDateNullable() != DateTime.Now.Date))
                return;

            //See what kind of locate we have.
            string skipType = string.Empty;
            //From LP22, go to LP2J and select all historical records.
            if (RI.CheckForText(23, 20, "SET2"))
                RI.Hit(Key.F2);
            RI.Hit(Key.F5);
            RI.PutText(7, 51, "X", Key.Enter);
            //Loop through all the records that have today's date.
            for (int row = 7; RI.CheckForText(row, 34, DateTime.Now.ToString("MMddyyyy")) && !RI.CheckForText(22, 3, "46004"); row++)
            {
                if (row > 18)
                {
                    RI.Hit(Key.F8);
                    row = 7;
                }
                //Select the record.
                RI.PutText(21, 13, RI.GetText(row, 2, 2), Key.Enter);
                //Check the address validity indicator.
                if (lp22Demographics.IsValidAddress && lp22Demographics.AddressValidityDate.ToDateNullable() == DateTime.Now.Date && RI.CheckForText(5, 80, "N"))
                    skipType = (skipType.Length == 0 ? "A" : "B");
                //Check the phone validity indicator.
                if (lp22Demographics.IsPrimaryPhoneValid && lp22Demographics.PhoneValidityDate.ToDateNullable() == DateTime.Now.Date && RI.CheckForText(8, 38, "N"))
                    skipType = (skipType.Length == 0 ? "P" : "B");
                //We can stop looking if we've already found both.
                if (skipType == "B")
                    break;
                //Go back for the next record.
                RI.Hit(Key.F12);
            }

            if (skipType.Length == 0)
                return;

            string comment = string.Format("Prev Skip Type: {0}, Locate Type: {1}", skipType, code.LocateType);
            RI.AddCommentInLP50(task.Ssn, "SFNDM", G.ScriptId, "AM", "36", comment);


            //Cancel eligible queue tasks if the address and phone are both valid,
            //and the task isn't currently being worked.
            if (lp22Demographics.IsValidAddress && lp22Demographics.IsPrimaryPhoneValid)
            {
                RI.FastPath(string.Format("LP8YCSKP;;;{0}", task.Ssn));
                if (!RI.CheckForText(22, 3, "47004"))
                {
                    while (!RI.CheckForText(22, 3, "46004"))
                    {
                        //Mark all the eligible active queue tasks.
                        for (int row = 7; !RI.CheckForText(row, 33, " "); row++)
                        {
                            if (RI.CheckForText(row, 33, "A") && DA.LocateQueues().Contains(RI.GetText(row, 65, 8)))
                            {
                                RI.PutText(row, 33, "X");
                            }
                        }
                        //Post the cancellations.
                        RI.Hit(Key.F6);
                        //Move on to the next page.
                        RI.Hit(Key.F8);
                    }
                }
            }
        }


        /// <summary>
        /// Checks to see if a borrower is on Onelink
        /// </summary>
        public static bool BorrowerExistsOnOnelink(ReflectionInterface RI, string ssn)
        {
            RI.FastPath("LP22I" + ssn);
            return RI.CheckForText(1, 69, "DEMOGRAPHICS");
        }

        public static bool UserHasOneLinkAccess(ReflectionInterface RI)
        {
            RI.FastPath("LP22I");
            if (RI.CheckForText(1, 2, "04011"))
                return false;
            return true;
        }
    }
}
