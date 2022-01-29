using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace NTISFCFED
{
    class UploadProcessor
    {
        private string Dir { get; set; }
        private string FileName { get; set; }
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }

        //Note unable to use Uses sporc on ctor
        public UploadProcessor(ProcessLogRun plr, DataAccess da)
        {
            PLR = plr;
            DA = da;
            Console.WriteLine("Starting upload process.");
            int runNumber = DA.GetRunNumber();
            FileName = string.Format("B_502{0:MMddyyyyhhmm}_{1}", DateTime.Now, runNumber.ToString().PadLeft(2, '0'));
            Dir = Path.Combine(EnterpriseFileSystem.TempFolder, FileName);
            CleanUpPreviousRun();
            CreateFile();
        }

        private void CleanUpPreviousRun()
        {
            if (Directory.Exists(Dir) && DataAccessHelper.TestMode)
                FS.Delete(Dir, true);

            Thread.Sleep(2000);
            if (!Directory.Exists(Dir))
                FS.CreateDirectory(Dir);
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Creates a zip file and send it to NTIS
        /// </summary>
        /// <returns></returns>
        public int StartProcess()
        {
            List<DbData> borrowers = DA.Populate();
            for (int index = 0; index < borrowers.Count; index++)
            {
                ProcessBorrower(borrowers[index], (index + 1));//The second parameter is the borrower counter.
            }

            Console.WriteLine("Creating the ZipFile.");
            string zipFileName = Path.Combine(EnterpriseFileSystem.TempFolder, FileName + ".zip");
            Repeater.TryRepeatedly(() => ZipFile.CreateFromDirectory(Dir, zipFileName, CompressionLevel.Optimal, false));
            Console.WriteLine("Creating the Hash file.");
            string hashFile = CreateMd5Hash(zipFileName);

            if (!SendFile(zipFileName))
                return 1;

            if (!SendFile(hashFile))
                return 1;

            Console.WriteLine("Updating records as processed.");
            DA.UpdateRecordsAsProcessed(borrowers);
            DA.InsertRunRecord();

            Console.WriteLine("Adding ARCS to arc add processing.");
            Repeater.TryRepeatedly(() => FS.Delete(Dir, true));
            return AddArcs(borrowers);
        }

        private int AddArcs(List<DbData> borrowers)
        {
            string arc = "ALTST";
            int returnCode = 0;
            int tries = 1;
            foreach (DbData borrower in borrowers)
            {
                tries = 1;
                string letter = DA.GetLetterIdFromLetterPath(Path.GetFileName(borrower.FilePath), borrower.AccountNumber);
                string comment = string.Format("Sent Letter:{0}; Format:{1}; DocDetailId:{2}", letter, borrower.AltFormatDescription, borrower.DocumentDetailsId);
                ArcAddResults result = null;
                Repeater.TryRepeatedly(() => result = GenerateArc(borrower, arc, comment, tries));
            }

            return returnCode;
        }

        private ArcAddResults GenerateArc(DbData borrower, string arc, string comment, int tries)
        {
            ArcAddResults result = GetArcData(borrower, arc, comment).AddArc();
            if (result.ArcAdded)
                return result;
            else
            {
                string message = string.Format("Unable to add Arc: {0}, Comment: {1} to ArcAddProcessing for the following borrower {2}.  Errors returned: {3}", arc, comment, borrower.AccountNumber, string.Join(Environment.NewLine, result.Errors));
                if (tries++ == 5)
                {
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                throw new Exception(message);
            }
        }


        private ArcData GetArcData(DbData borrower, string arc, string comment)
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

        private bool SendFile(string file)
        {
            Console.WriteLine("About to send file {0}", file);
            if (DataAccessHelper.TestMode)
            {
                if (Dialog.Info.YesNo($"Since this is being run in test, files are not actually being uploaded to NTIS.\n Do you want the file {file} to be deleted as it would in a live run?"))
                {
                    if (File.Exists(file))
                        FS.Delete(file);
                }
                    return true;
            }

            string path = "/upload/";
            string args = string.Format("O -site NTIS -u {0} -p {1}", file, path);
            using (Process coreFTP = Process.Start(EnterpriseFileSystem.GetPath("CoreFtp"), args))
            {
                coreFTP.WaitForExit();
                if (coreFTP.ExitCode != 0)
                {
                    string message = string.Format("Unable to send file: {0}. CoreFtp exit code: {1}", file, coreFTP.ExitCode);
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }
            }

            if (File.Exists(file))
                FS.Delete(file);

            return true;
        }

        private string CreateMd5Hash(string fileName)
        {
            string md5Hash = string.Empty;
            using (var md5 = MD5.Create())
            {
                using (var stream = FS.OpenRead(fileName))
                {
                    md5Hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }

            string md5File = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format(@"{1}\{0}.md5", Path.GetFileName(fileName), EnterpriseFileSystem.TempFolder));
            using (StreamW sw = new StreamW(md5File))
            {
                sw.Write(string.Format("{0}  {1}", md5Hash, Path.GetFileName(fileName)));
            }

            return md5File;
        }

        private void ProcessBorrower(DbData borrower, int borrowerNumber)
        {
            Console.WriteLine("Processcessing borrower {0}", borrower.AccountNumber);
            string sourceFileName = CalculateSourceFileName(borrower, borrowerNumber) + ".pdf";
            string line = string.Format("{0},502,{1},{2},P,{3},{4},{5},{6},{7}", FileName.Substring(2), sourceFileName, Path.GetFileNameWithoutExtension(borrower.FilePath),
                GetPageCount(Path.GetFileName(borrower.FilePath)), borrower.AltFormatType, borrower.AltFormatType.ToInt() == 4 ? "T" : "", borrower.GetBorrowerData(), GetReturnContactInfo());

            FS.Copy(Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), Path.GetFileName(borrower.FilePath)), Path.Combine(Dir, sourceFileName));
            AddRecordToFile(line);
        }

        private string GetReturnContactInfo()
        {
            return DA.GetResponseInfo();
        }

        private string CalculateSourceFileName(DbData borrower, int borrowerNumber)
        {
            return string.Format("5{0}.{1}_{2}", borrower.AltFormatType, FileName.Substring(2), borrowerNumber.ToString().PadLeft(6, '0'));
        }

        private int GetPageCount(string file)
        {
            return PdfHelper.GetNumberOfPagesInPdf(Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), file));
        }


        private void AddRecordToFile(string line)
        {
            using (StreamW sw = new StreamW(Path.Combine(Dir, FileName.Substring(2) + ".csv"), true))
            {
                sw.WriteLine(line);
            }
        }

        private void CreateFile()
        {
            using (StreamW sw = new StreamW(Path.Combine(Dir, FileName.Substring(2) + ".csv")))
            {
                sw.WriteLine("FSA DATA FILE,SERVICER CODE,TO FULFILL SOURCE FILE NAME,TO FULFIL SOURCE FILE DOCUMENT NAME,TO FULFILL SOURCE FILE TYPE,TO FULFILL SOURCE LENGTH,REQUESTED ALTERNATIVE FORMAT,DATA CD FORMAT,FIRST NAME,LAST NAME,ADDRESS LINE 1,ADDRESS LINE 2,ADDRESS LINE 3,ADDRESS LINE 4,ADDRESS LINE 5,CITY,STATE,FOREIGN STATE,COUNTRY,ZIP (5 DIGIT),ZIP (4 DIGIT),INTERNATIONAL ZIP,RECIPIENT UNIQUE IDENTIFIER,RETURN NAME,RETURN ADDRESS LINE 1,RETURN ADDRESS LINE 2,RETURN ADDRESS LINE 3,RETURN CITY,RETURN STATE,RETURN ZIP (5 DIGIT),RETURN ZIP (4 DIGIT)");
            }
        }
    }
}
