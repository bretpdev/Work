using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace DTX7L
{
    class SessionProcessor
    {
        public ReflectionInterface RI { get; set; }
        private ProcessLogData LogData { get; set; }
        private BatchProcessingHelper LoginInfo { get; set; }
        private ReaderWriterLockSlim ThreadLock { get; set; }

        public SessionProcessor(ProcessLogData logData, ReaderWriterLockSlim threadLock, string scriptId)
        {
            LogData = logData;
            LoginInfo = BatchProcessingHelper.GetNextAvailableId(scriptId, "BatchUheaa");
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
                if (LoginInfo != null)
                {
                    Console.WriteLine(string.Format("Current Time:{1}; {0} id ready for login", LoginInfo.UserName, DateTime.Now));
                    RI = new ReflectionInterface();
                    return RI.Login(LoginInfo.UserName, LoginInfo.Password, DataAccessHelper.Region.Uheaa);
                }
                return false;
            }
            catch (Exception ex)
            {
                string message = "Error creating session";
                Console.WriteLine(message);

                ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning, LogData.ExecutingAssembly, ex);
                return false;
            }
        }

        /// <summary>
        /// Processes TD2A
        /// </summary>
        /// <param name="count">Thread number</param>
        public void ProcessResponseCodes(int count)
        {
            while (Dtx7lErrorLettersRecords.DeletedRecords.Any())
                ProcessTd2a(GetNextDeletedRecord(), count);

            RI.CloseSession();
        }

        private void ProcessTd2a(Dtx7lDeletedRecords record, int count)
        {
            Console.WriteLine("Current Time:{2}; Thread {0} is about to process borrower {1}", count, record.Ssn, DateTime.Now);
            if (!record.Ssn.Contains("999999999"))//Not 100% sure why this is needed but it was in the existing code
            {
                RI.FastPath("TX3Z/CTD2A" + record.Ssn);
                RI.PutText(11, 65, record.Arc); //enter ARC as search criteria
                RI.PutText(21, 16, record.RequestDate.ToString("MMddyy"), ReflectionInterface.Key.Enter); //enter request date as search criteria
                if (!RI.CheckForText(1, 72, "TDX2B"))
                {
                    //if selection screen appears then select first option
                    if (RI.CheckForText(1, 72, "TDX2C"))
                        RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);

                    while (RI.MessageCode != "90007")
                    {
                        if (RI.MessageCode == "01029" || RI.MessageCode == "00000")//Records that are updated by another thread get these errors so we just want to break out of the loop
                        {
                            break;
                        }
                        if (record.IsDueDiligence)
                        {
                            if (RI.CheckForText(15, 2, "__"))
                                RI.PutText(15, 2, "PRNTD", ReflectionInterface.Key.Enter);
                        }
                        else
                            RI.PutText(6, 18, "E", ReflectionInterface.Key.Enter);

                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                }
            }

            record.MarkProcessed();
        }

        /// <summary>
        /// Deletes records from TX7L
        /// </summary>
        /// <param name="count">Thread number</param>
        public void ProcessTX7L(int count)
        {
            while (Dtx7lErrorLettersRecords.DueDilLetters.Any() || Dtx7lErrorLettersRecords.ExpiredLetters.Any())
                DeleteFromTX7L(GetNextRecordToDelete(), count);

            RI.CloseSession();
        }

        private void DeleteFromTX7L(LetterIDAndArcCombo item, int count)
        {
            Console.WriteLine("Current Time:{2}; Thread {0} is about to delete Letter {1}", count, item.LetterID, DateTime.Now);
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
                    AddProcessedRecordToDb(RI.GetText(10, 18, 9), item.ARC, RI.GetText(19, 18, 8).ToDate(), item.LetterID, item.IsDueDil);
                }
            }
            else
                ProcessLogger.AddNotification(Dtx7lErrorLettersRecords.LogData.ProcessLogId,
                    string.Format("Unknown screen found for Letter {0} Screen {1}", item.LetterID, RI.ScreenCode), NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "DTX7LInsertDeletedRecord")]
        private void AddProcessedRecordToDb(string ssn, string arc, DateTime requestDate, string letterId, bool isDueDil)
        {
            if (isDueDil)
            {
                if (RI.CheckForText(4, 18, "TDX24"))
                {
                    DataAccessHelper.Execute("DTX7LInsertDeletedRecord", DataAccessHelper.Database.Uls, SqlParams.Single("Ssn", ssn),
                        SqlParams.Single("Arc", arc), SqlParams.Single("RequestDate", requestDate), SqlParams.Single("LetterId", letterId),
                        SqlParams.Single("IsDueDiligence", isDueDil));

                    RI.PutText(5, 62, "N");//update cancel activity
                    RI.Hit(ReflectionInterface.Key.Enter);
                }
                else
                    DataAccessHelper.Execute("DTX7LInsertDeletedRecord", DataAccessHelper.Database.Uls, SqlParams.Single("Ssn", ssn),
                        SqlParams.Single("Arc", arc), SqlParams.Single("RequestDate", requestDate), SqlParams.Single("LetterId", letterId),
                        SqlParams.Single("IsDueDiligence", false));

            }
            else
                DataAccessHelper.Execute("DTX7LInsertDeletedRecord", DataAccessHelper.Database.Uls, SqlParams.Single("Ssn", ssn),
                    SqlParams.Single("Arc", arc), SqlParams.Single("RequestDate", requestDate), SqlParams.Single("LetterId", letterId),
                    SqlParams.Single("IsDueDiligence", isDueDil));

            RI.Hit(ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F12);
        }

        private LetterIDAndArcCombo GetNextRecordToDelete()
        {
            ThreadLock.EnterWriteLock();
            LetterIDAndArcCombo item = null;
            if (Dtx7lErrorLettersRecords.DueDilLetters.Any())
            {
                item = Dtx7lErrorLettersRecords.DueDilLetters.Dequeue();
                item.IsDueDil = true; //In the response code there is a different flow from non due diligence records
            }
            else
                item = Dtx7lErrorLettersRecords.ExpiredLetters.Dequeue();
            ThreadLock.ExitWriteLock();

            return item;
        }

        private Dtx7lDeletedRecords GetNextDeletedRecord()
        {
            ThreadLock.EnterWriteLock();
            Dtx7lDeletedRecords item = Dtx7lErrorLettersRecords.DeletedRecords.Dequeue();
            ThreadLock.ExitWriteLock();
            return item;
        }
    }
}
