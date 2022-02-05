using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using System.Linq.Expressions;

namespace IDRXMLDATA
{
    public class IDRXMLDATA
    {
        public ProcessLogRun LogRun { get; set; }
        public string ScriptId { get { return "IDRXMLDATA"; } }
        static readonly object locker = new object();
        private DataAccess DA { get; set; }

        public IDRXMLDATA(DataAccessHelper.Mode mode)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = mode;
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            DA = new DataAccess(LogRun);
        }
        /// <summary>
        /// Starting point of the application.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            bool showMessage = false;
            if (args.Any() && args.Count() > 2)
                showMessage = true;
            if (!DataAccessHelper.StandardArgsCheck(args, "IDR XML Reader", showMessage) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), showMessage))
                return 1;


            return new IDRXMLDATA(DataAccessHelper.CurrentMode).Process(args);
        }

        /// <summary>
        /// Start up process logger and locate files to process
        /// </summary>
        /// <returns></returns>
        private int Process(string[] args)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationActivityType));
            List<string> files = new List<string>();
            string destination = EnterpriseFileSystem.GetPath("ARCHIVE");
            foreach (string f in Directory.EnumerateFiles(EnterpriseFileSystem.FtpFolder).Where(x => x.Contains("COD_IBR_APPL_XML")))
            {
                files.Add(f);
            }

            ProcessFiles(serializer, files, destination, args);
            LogRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return 0;
        }

        public int GetNumberOfThreads(int xmlNodes, int maxNumberOfThreads)
        {
            return Math.Min(xmlNodes, maxNumberOfThreads);
        }

        /// <summary>
        /// Read XML from files and thread out database comparison
        /// </summary>
        /// <param name="serializer">Serializes the xml into object</param>
        /// <param name="files">List of files to process</param>
        /// <param name="destination">Archive location</param>
        private void ProcessFiles(XmlSerializer serializer, List<string> files, string destination, string[] args)
        {
            foreach (string filename in files)
            {
                Stream reader = new FStream(filename, FileMode.Open);
                ApplicationActivityType xmlData = (ApplicationActivityType)serializer.Deserialize(reader);
                reader.Close();

                if (xmlData.Borrower == null)
                {
                    ArchiveFiles(destination, filename);
                    continue;
                }



                Queue<AppBorrowerType> xmlNodes = new Queue<AppBorrowerType>(xmlData.Borrower.ToList());

                int threads = GetNumberOfThreads(xmlNodes.Count, args.Count() > 1 ? args[1].ToInt() : 5);
                ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

                Parallel.For(0, threads, threadId => //Run 5 threads pulling the borrowers xml and making sql inserts
                {
                    locker.EnterWriteLock();
                    AppBorrowerType borrower = null;
                    borrower = xmlNodes.Dequeue();
                    locker.ExitWriteLock();
                    while (borrower != null)
                    {
                        BorrowerData borrowerToAdd = IdentifyAndUpdateBorrower(borrower, threadId);
                        if (borrowerToAdd != null) //We found the borrower
                        {
                            if (borrowerToAdd.Region == "CDW")
                            {
                                LogRun.AddNotification($"A CDW borrower with account {borrowerToAdd.AccountNumber} was identified in the file {filename}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                return;
                            }

                            if (borrower.RepaymentApplication[0].Spouse != null) //If they have a spouse in the xml, check the database
                                borrowerToAdd.SpouseId = IdentifyAndUpdateSpouse(borrower, threadId);

                            List<ExistingApp> existingAppId = DA.GetExistingApp(borrower.RepaymentApplication[0].ApplicationID);

                            if (existingAppId.Any() && borrowerToAdd.Region == "UDW")
                                LogRun.AddNotification(string.Format("An application with a matching applicationId already exists in the commercial database.  This application will need to be manually reviewed. File: {0}; ApplicationId: {1}.", filename, borrower.RepaymentApplication[0].ApplicationID), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            else if (!existingAppId.Any()) //only process the application if it doesnt already exist.
                            {
                                int? appId = DA.CreateNewApplication(borrowerToAdd, borrower, threadId);
                                if (appId != null) //failed to create new application. Process logged at lower level.
                                {
                                    int? planId = DA.CreateNewSelectedPlan(appId, borrower, borrowerToAdd, threadId);
                                    DA.CreateNewStatusHistory(planId);
                                    foreach (UnderlyingLoansType loan in borrower.RepaymentApplication[0].UnderlyingLoans)
                                    {
                                        DA.CreateOtherLoans(appId, borrower, loan, borrowerToAdd, threadId);
                                        DA.CreateNewLoan(appId, borrowerToAdd.BorrowerID, loan);
                                    }
                                }
                            }
                        }

                        locker.EnterWriteLock();
                        if (xmlNodes.Any())
                            borrower = xmlNodes.Dequeue();
                        else
                            borrower = null;
                        locker.ExitWriteLock();
                    }
                });
                ArchiveFiles(destination, filename);
            }
        }

        private static void ArchiveFiles(string destination, string filename)
        {
            string filePath = filename;
            string fileName = filename.Substring(filename.LastIndexOf(("\\")) + 1);
            Repeater.TryRepeatedly(() => FS.Copy(filename, destination + fileName, true));
            Repeater.TryRepeatedly(() => FS.Delete(filePath));
        }

        /// <summary>
        /// Looks at the spouse node and adds/updates it in our IncomeDrivenRepayment Database
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns>SpouseId</returns>
        private int IdentifyAndUpdateSpouse(AppBorrowerType borrower, int threadId)
        {
            string message = "";
            List<int> spouseId = new List<int>();

            if (!spouseId.Any())
            {
                message = string.Format("ThreadId: {0}; Unable to get a spouse Id for SSN: {1}.  Creating new spouse record.", threadId, borrower.RepaymentApplication[0].Spouse.SSN);
                Console.WriteLine(message);
            }

            spouseId = DA.InsertNewSpouse(borrower);
            DA.UpdateSpouse(spouseId[0], borrower);
            return spouseId[0];
        }

        /// <summary>
        /// Looks at the borrower node and adds/updates it in our IncomeDrivenRepayment Database
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns>BorrowerData object with accountnumber, name, and borrowerId</returns>
        private BorrowerData IdentifyAndUpdateBorrower(AppBorrowerType borrower, int threadId)
        {
            List<int> borrowerId = new List<int>();
            List<BorrowerData> tempBorrowerUDW = DA.GetBorrowerDemos(borrower); //Add new borrower with UDW data

            if (!BorrowerInUhRegion(borrower, tempBorrowerUDW, threadId)) //Borrower in UH region
                return null;

            tempBorrowerUDW[0].Region = "UDW";
            borrowerId = DA.GetBorrowerId(borrower); // We add it in UDW

            if (!borrowerId.Any())
            {
                string message = string.Format("ThreadId: {0}; Unable to get a borrower Id for AccountNumber: {1}.  Creating new borrower record.", threadId, tempBorrowerUDW[0]?.AccountNumber);
                Console.WriteLine(message);
                borrowerId = DA.InsertNewBorrower(tempBorrowerUDW[0]);  //Add borrower to borrowers table
            }

            tempBorrowerUDW[0].BorrowerID = borrowerId[0];
            return tempBorrowerUDW[0];
        }

        /// <summary>
        /// If the borrower exists in only 1 region, we can handle the application.
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns>true if the borrower maps to 1 region only</returns>
        private bool BorrowerInUhRegion(AppBorrowerType borrower, List<BorrowerData> tempBorrowerUDW, int threadId)
        {
            string message = "";
            if (tempBorrowerUDW.Count == 0) //Borrower not found
            {
                message = string.Format("ThreadId: {0}; Borrower not found in UH region. SSN: {1}.", threadId, borrower.SSN);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return false;
            }
            return true;
        }
    }
}