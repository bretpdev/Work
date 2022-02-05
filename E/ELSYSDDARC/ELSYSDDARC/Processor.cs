using System;
using System.Data.SqlClient;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ELSYSDDARC
{
    class Processor
    {
        private ReflectionInterface RI { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        public BatchProcessingHelper LoginInfo { get; set; }

        public Processor(ProcessLogRun logRun, DataAccess da, string scriptId)
        {
            LogRun = logRun;
            LoginInfo = BatchProcessingHelper.GetNextAvailableId(scriptId, "BatchUheaa");
            DA = da;
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
                    Console.WriteLine(string.Format("{0} id ready for login", LoginInfo.UserName));
                    RI = new ReflectionInterface();
                    return RI.Login(LoginInfo.UserName, LoginInfo.Password);
                }
                return false;
            }
            catch (Exception)
            {
                string message = "Error creating session";
                Console.WriteLine(message);
                ProcessLogger.AddNotification(LogRun.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return false;
            }
        }

        /// <summary>
        /// Goes to CTD2A to find all ARC's on borrowers account
        /// </summary>
        /// <param name="data">list of borrower data objects</param>
        public void ProcessData(int id)
        {
            Borrower bor = null;
            while ((bor = DA.GetNextBorrower()) != null)
            {
                Overwrite(bor);
            }
            BatchProcessingHelper.CloseConnection(LoginInfo);
            Console.WriteLine("Thread " + id + " finished");
            try
            {
                RI.CloseSession();
            }
            catch (Exception)
            {
                //Catching the exception thrown by the session closing. This happens when the session
                //is already closed, It is already logged in the OverwriteCode method if it crashed.
            }
        }

        /// <summary>
        /// Goes to CTD2A to start the overwrite process
        /// </summary>
        /// <param name="bor">The borrower to process</param>
        private void Overwrite(Borrower bor)
        {
            Console.WriteLine(string.Format("Processing Borrower: {0}, ARC: {1}", bor.GroupKey, bor.ARC));
            RI.FastPath("TX3Z/CTD2A" + bor.GroupKey);
            RI.PutText(11, 65, bor.ARC);
            RI.PutText(21, 16, bor.ArcDate.Replace("/", "").Remove(4, 2), ReflectionInterface.Key.Enter);
            OverwriteCode(bor);
        }

        /// <summary>
        /// Overwrites the status code to an E
        /// </summary>
        /// <param name="requestor">The requetor ID from the sas file</param>
        /// <returns>True if overwrite is needed</returns>
        private void OverwriteCode(Borrower bor)
        {
            bool found = false;
            if (RI.CheckForText(1, 72, "TDX2B")) //No ARC's found
            {
                RecordARCError(bor);
                return;
            }
            else if (RI.CheckForText(1, 72, "TDX2C")) //Selection Screen
            {
                RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);
                while (RI.MessageCode != "90007")
                {
                    if (RI.CheckForText(13, 40, bor.Requestor) && RI.GetText(13, 31, 8).ToDate() == bor.ArcDate.ToDate())
                    {
                        found = true;
                        break; //Once the requestor is found, break out
                    }
                    else
                        RI.Hit(ReflectionInterface.Key.F8); ;
                }
                if (!found)
                {
                    RecordARCError(bor);
                    return;
                }
            }
            RI.PutText(6, 18, "E", ReflectionInterface.Key.Enter); //Change ARC status to E to cancel ARC
        }

        /// <summary>
        /// Record any errors finding the ARC
        /// </summary>
        /// <param name="bdata">Borrower Data object for the current borrower</param>
        private void RecordARCError(Borrower bdata)
        {
            string message = string.Format("The ARC was not found for Borrower: {0}, ARC: {1}, ARC Date: {2}, ARC Requestor: {3}", bdata.GroupKey, bdata.ARC, bdata.ArcDate, bdata.Requestor);
            ProcessLogger.AddNotification(LogRun.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            Console.WriteLine(message);
        }

        public void CloseSession()
        {
            RI.CloseSession();
        }
    }
}