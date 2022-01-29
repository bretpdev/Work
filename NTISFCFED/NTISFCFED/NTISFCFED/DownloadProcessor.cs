using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace NTISFCFED
{
    public class DownloadProcessor
    {
        private string DownloadDirectory { get; set; }
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }

        public DownloadProcessor(ProcessLogRun plr, DataAccess da)
        {
            PLR = plr;
            DA = da;
            DownloadDirectory = Path.Combine(EnterpriseFileSystem.TempFolder, Program.ScriptId);
            if (Directory.Exists(DownloadDirectory) && !DataAccessHelper.TestMode) //Don't delete test directory
            {
                FS.Delete(DownloadDirectory, true);
                Thread.Sleep(2000);//Sleep to allow for our network speed to catch up
            }

            if (!Directory.Exists(DownloadDirectory))
                FS.CreateDirectory(DownloadDirectory);
        }

        /// <summary>
        /// Starts the process to download files from NTIS and will add arc to arcaddprocessing
        /// </summary>
        /// <returns></returns>
        public int StartProcess()
        {
            List<string> files = DownLoadFiles();
            if (files == null)
            {
                string message = "Unable to download response files from NTIS.";
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return 1;
            }

            return ProcessFiles(files);
        }

        private int ProcessFiles(List<string> files)
        {
            List<ResponseFile> records = new List<ResponseFile>();
            foreach (string file in files)
            {
                FS.Copy(file, Path.Combine(EnterpriseFileSystem.GetPath("NTISResponse"), Path.GetFileName(file)));
                if (!file.Contains(".md5"))//The md5 files are just a hash file we do not want to process them
                {
                    using (StreamR sr = new StreamR(file))
                    {
                        sr.ReadLine(); //read header
                        while (!sr.EndOfStream)
                            records.Add(new ResponseFile(sr.ReadLine()));
                    }
                }
            }

            return AddArcs(records);
        }

        private int AddArcs(List<ResponseFile> records)
        {

            int returnCode = 0;
            int tries = 1;
            foreach (ResponseFile borrower in records)
            {
                tries = 1;
                string arc = ParseArc(borrower.StatusCode);

                if (arc == null)
                {
                    string message = string.Format("Unknown response code {0} cannot determine ARC to leave on Borrowers Account {1}", borrower.StatusCode, borrower.AccountNumber);
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    returnCode = 1;
                    continue;
                }
                string letter = DA.GetLetterIdFromLetterPath(Path.GetFileName(borrower.DocumentName), borrower.AccountNumber);
                string comment = string.Format("Letter:{0}; Format:{1}; Status:{2}; Shipping Courier:{3}; Tracking Number:{4}; DocDetailId:{5}", letter, DataAccessHelper.ExecuteSingle<string>("GetFormatNameFromCode", DataAccessHelper.Database.ECorrFed, SqlParams.Single("FormatCode", borrower.AltFormat)), borrower.ParseStatus(), borrower.ShippingCourier ?? "N/A", borrower.TrackingNumber ?? "N/A", DataAccessHelper.ExecuteSingle<int>("GetDocDetailIdFromLetter", DataAccessHelper.Database.ECorrFed, SqlParams.Single("Letter", Path.GetFileName(borrower.DocumentName))));

                ArcAddResults result = null;
                Repeater.TryRepeatedly(() => result = ArcAddSuccessfull(borrower, arc, comment, tries));

            }
            return returnCode;
        }

        public ArcAddResults ArcAddSuccessfull(ResponseFile borrower, string arc, string comment, int tries)
        {
            ArcAddResults result = GetArcData(borrower, arc, comment).AddArc();
            if (result.ArcAdded)
                return result;
            else
            {
                if (tries++ == 5)
                {
                    string message = string.Format("Unable to add Arc: {0}, Comment: {1} to ArcAddProcessing for the following borrower {2}.  Errors returned: {3}", arc, comment, borrower.AccountNumber, string.Join(Environment.NewLine, result.Errors));
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                throw new Exception();
            }
        }

        public string ParseArc(string statusCode)
        {
            switch (statusCode)
            {
                //For def look at Parse Status Method in ResponseFile.cs
                case "R":
                    return "ALTRC";
                case "I":
                    return "ALTIP";
                case "E":
                    return "ALTER";
                case "F":
                    return "ALTPR";
                case "S":
                    return "ALTES";
                case "A":
                    return "ALTAB";
                default:
                    return null;
            }
        }

        private ArcData GetArcData(ResponseFile borrower, string arc, string comment)
        {
            return new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = borrower.AccountNumber,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                Arc = arc,
                ScriptId = Program.ScriptId,
                RecipientId = ""
            };
        }

        private List<string> DownLoadFiles()
        {
            Console.WriteLine("About to download files from NTIS");
            if (DataAccessHelper.TestMode)
            {
                Dialog.Info.Ok($"You are in test mode, so files will be retrieved from {DownloadDirectory} and not from the NTIS server.");
                return Directory.GetFiles(DownloadDirectory).ToList();
            }
            string path = "/download/R_502*";//File pattern to look for

            //Run LFTP, passing it the name of the script file.
            string args = string.Format("O -site NTIS -d {0} -p {1} -delsrc", path, DownloadDirectory);
            using (Process coreFTP = Proc.Start("CoreFtp", args))
            {
                coreFTP.WaitForExit();
                if (coreFTP.ExitCode != 0)
                {
                    string message = string.Format("Unable to download response files from NTIS. CoreFtp exit code: {0}", coreFTP.ExitCode);
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Console.WriteLine(message);
                    return null;
                }
            }

            //See what we got back. If there are any results, delete the file(s) from Accurint.
            return Directory.GetFiles(DownloadDirectory).ToList();
        }
    }
}
