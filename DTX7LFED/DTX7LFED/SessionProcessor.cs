using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.Baa;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace DTX7LFED
{
    class SessionProcessor
    {
        public IReflectionInterface RI { get; set; }
        public string ScriptId { get; set; }
        private ProcessLogRun LogData { get; set; }
        private BatchProcessingHelper LoginInfo { get; set; }
        private ReaderWriterLockSlim ThreadLock { get; set; }
        private DataAccess DA { get; set; }

        public SessionProcessor(ProcessLogRun logData, DataAccess da, ReaderWriterLockSlim threadLock, string scriptId)
        {
            DA = da;
            LogData = logData;
            ScriptId = scriptId;
            ThreadLock = threadLock;
        }

        /// <summary>
        /// Creates a ReflectionInterface and logs in using the provided batch login info.
        /// </summary>
        /// <returns>True if login successful</returns>
        public bool Login()
        {
            try
            {
                var ri = new ReflectionInterface();
                Thread.Sleep(2000);//Sleep to allow the process to create sessions
                LoginInfo = BatchProcessingLoginHelper.Login(LogData, ri, ScriptId, DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? "BatchCornerStone" : "BatchUheaa");
                Console.WriteLine(string.Format("{0} id ready for login", LoginInfo.UserName));
                if (DataAccessHelper.TestMode)
                    RI = new BaaReflectionInterface(ri, ScriptId);
                else
                    RI = ri;
                return true;
            }
            catch (Exception ex)
            {
                string message = "Error creating session";
                Console.WriteLine(message);

                LogData.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return false;
            }
        }

        /// <summary>
        /// Processes TD2A
        /// </summary>
        /// <param name="count">Thread number</param>
        public void ProcessResponseCodes(int count)
        {
            while (Program.DeletedRecords.Count > 0)
                ProcessTd2a(GetNextDeletedRecord(), count);

            RI.CloseSession();
        }

        private void ProcessTd2a(Dtx7lDeletedRecords record, int count)
        {
            Console.WriteLine("Thread {0} is about to process borrower {1}", count, record.Ssn);
            if (!record.Ssn.Contains("999999999"))//Not 100% sure why this is needed but it was in the existing code
            {
                RI.FastPath("TX3Z/CTD2A" + record.Ssn);
                RI.PutText(11, 65, record.Arc); //enter ARC as search criteria
                RI.PutText(21, 16, record.RequestDate.AddDays(-2).ToString("MMddyy")); //enter request date as search criteria
                RI.PutText(21, 30, record.RequestDate.AddDays(2).ToString("MMddyy"), ReflectionInterface.Key.Enter);

                if (!RI.CheckForText(1, 72, "TDX2B"))
                {
                    //if selection screen appears then select first option
                    if (RI.CheckForText(1, 72, "TDX2C"))
                        RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);

                    while (RI.MessageCode != "90007")
                    {
                        if (RI.MessageCode.IsIn("01029", "00000", "01847", "01796"))//Records that are updated by another thread get these errors so we just want to break out of the loop
                            break;

                        var requestedByUtId = RI.CheckForText(13, 40, "UT");
                        if (RI.CheckForText(15, 2, "__"))
                        {
                            if (requestedByUtId)
                                RI.PutText(15, 2, "PRNTD");
                            else
                                RI.PutText(15, 2, "CANCL");
                        }
                        if (!requestedByUtId)
                            RI.PutText(6, 18, "E");
                        RI.Hit(ReflectionInterface.Key.Enter);
                        if(RI.MessageCode == "03461" && !requestedByUtId)
                        {
                            RI.PutText(15, 2, "", true);
                            RI.Hit(ReflectionInterface.Key.Enter);
                        }
                        RI.Hit(ReflectionInterface.Key.F8);

                    }

                }
            }

            DA.MarkProcessed(record.DTX7LDeletedRecordId);
        }

        /// <summary>
        /// Deletes records from TX7L
        /// </summary>
        /// <param name="count">Thread number</param>
        public void ProcessTX7L(int count)
        {
            while (Program.DueDilLetters.Any() || Program.ExpiredLetters.Any())
            {
                ThreadLock.EnterWriteLock();
                LetterIDAndArcCombo item = null;
                if (Program.DueDilLetters.Any())
                {
                    item = Program.DueDilLetters.Dequeue();
                    item.IsDueDil = true;
                }
                else if (Program.ExpiredLetters.Any())
                    item = Program.ExpiredLetters.Dequeue();

                ThreadLock.ExitWriteLock();

                if (item != null)
                    DeleteFromTX7L(item, count);


            }

            RI.CloseSession();
        }

        private void DeleteFromTX7L(LetterIDAndArcCombo item, int count)
        {
            Console.WriteLine("Thread {0} is about to delete Letter {1}", count, item.LetterID);
            RI.FastPath("TX3Z/DTX7L");
            RI.PutText(8, 46, "", true);
            RI.PutText(11, 46, item.LetterID, true);
            RI.PutText(17, 46, "Y", ReflectionInterface.Key.Enter);

            if (RI.CheckForText(1, 74, "TXX7M"))//There are no records for this record move to the next one.
                return;

            if (RI.CheckForText(1, 72, "TXX7O"))//Target screen
                AddProcessedRecordToDb(RI.GetText(10, 18, 9), item.ARC, RI.GetText(19, 18, 8).ToDate(), item.LetterID, item.IsDueDil);
            else if (RI.CheckForText(1, 72, "TXX7N"))//Selection Screen
            {
                for (int row = 8; RI.MessageCode != "90007"; row++)//If we get a selection screen it will delete all records found
                {
                    if (row > 19 || RI.CheckForText(row, 4, " "))
                    {
                        if (RI.CheckForText(2, 69, "20"))
                        {
                            RI.Hit(ReflectionInterface.Key.F8);
                            RI.Hit(ReflectionInterface.Key.Enter);
                        }
                        else
                            RI.Hit(ReflectionInterface.Key.F8);
                        row = 7;

                        continue;
                    }

                    RI.PutText(21, 14, RI.GetText(row, 3, 2), ReflectionInterface.Key.Enter, true);

                    if (RI.CheckForText(1, 74, "TXX7M"))//There are no records for this record move to the next one.
                        break;
                    AddProcessedRecordToDb(RI.GetText(10, 18, 9), item.ARC, RI.GetText(19, 18, 8).ToDate(), item.LetterID, item.IsDueDil);
                }
            }
            else
                LogData.AddNotification(string.Format("Unknown screen found for Letter {0} Screen {1}", item.LetterID, RI.ScreenCode), NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }


        private HashSet<Tuple<string, string, DateTime, string, bool>> processedRecordsAdded = new HashSet<Tuple<string, string, DateTime, string, bool>>();
        private void AddProcessedRecordToDb(string ssn, string arc, DateTime requestDate, string letterId, bool isDueDil)
        {
            //var onTdx24 = RI.CheckForText(4, 18, "TDX24");
            if (isDueDil)
            {
                RI.PutText(5, 62, "N");//update cancel activity
                RI.Hit(ReflectionInterface.Key.Enter);
            }
            var record = Tuple.Create(ssn, arc, requestDate, letterId, isDueDil);
            if (!processedRecordsAdded.Contains(record))
            {
                DA.InsertDeletedRecord(ssn, arc, requestDate, letterId, isDueDil);
                processedRecordsAdded.Add(record);
            }

            RI.Hit(ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F12);
        }


        private Dtx7lDeletedRecords GetNextDeletedRecord()
        {
            ThreadLock.EnterWriteLock();
            Dtx7lDeletedRecords item = Program.DeletedRecords.Dequeue();
            ThreadLock.ExitWriteLock();
            return item;
        }
    }
}
