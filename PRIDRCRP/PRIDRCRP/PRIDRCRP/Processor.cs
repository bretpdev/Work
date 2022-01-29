using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static PRIDRCRP.Program;
using Ionic.Zip;

namespace PRIDRCRP
{
    class Processor
    {
        public DataAccess DA { get; set; }
        private ProcessLogRun logRun;
        private FileSelectionMethod fsMethod;
        private int directoryNumber;
        public enum ExtractResult
        {
            NotFound,
            Success,
            Error,
            AlreadyExists
        }

        public Processor(ProcessLogRun logRun, FileSelectionMethod fsMethod, int directoryNumber)
        {
            DA = new DataAccess(logRun);
            this.logRun = logRun;
            this.fsMethod = fsMethod;
            this.directoryNumber = directoryNumber;
        }

        public void Process()
        {
            if (fsMethod == FileSelectionMethod.OpenFileDialog)
            {
                ProcessOpenFileDialog();
                DA.InactivateDuplicateLoanBorrowerInformation();
                //DA.UpdateNullRepaymentPlanChanges();
            }
            else if (fsMethod == FileSelectionMethod.DirectoryWithUnZip)
            {
                ProcessDirectoryWithUnZip();
                DA.InactivateDuplicateLoanBorrowerInformation();
                //DA.UpdateNullRepaymentPlanChanges();
            }
        }

        public void ProcessDirectoryWithUnZip()
        {
            string directoryPath = EnterpriseFileSystem.GetPath($"PRIDRCRP{directoryNumber}");
            string destinationRoot = EnterpriseFileSystem.GetPath($"PRIDRCRP_DESTINATION{directoryNumber}");

            foreach (string zipFile in GetZipFiles(directoryPath))
            {
                string zipFileWithoutExtention = Path.GetFileNameWithoutExtension(zipFile);
                string directoryDestination = Path.Combine(destinationRoot, zipFileWithoutExtention);
                if (!Directory.Exists(directoryDestination))
                {
                    try
                    {
                        FS.CreateDirectory(directoryDestination);
                    }
                    catch (Exception e)
                    {
                        logRun.AddNotification("Unable to create directory. Directory: " + directoryDestination + " Exception: " + e.Message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                        continue;
                    }
                }
                using (ZipFile archive = ZipFile.Read(zipFile))
                {
                    foreach (var entry in archive.Entries)
                    {
                        //Extract(entry, Path.Combine(destinationRoot, entry.FileName));
                        if (entry.UncompressedSize > 0 && (Path.GetFileName(entry.FileName).ToUpper().EndsWith("_BHAR_ACES.PDF") || Path.GetFileName(entry.FileName).ToUpper().EndsWith("_BHAR_DTS.TXT")))
                        {
                            ExtractResult result = Extract(entry, Path.Combine(destinationRoot, entry.FileName));
                            if (result == ExtractResult.Error)
                            {
                                logRun.AddNotification("Unable to extract file. Continuing. File: " + entry.FileName, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                                continue;
                            }
                        }
                    }
                }
                ProcessDirectoryWithoutUnZip(Path.Combine(destinationRoot, zipFileWithoutExtention), zipFileWithoutExtention);
            }
        }


        private void ProcessDirectoryWithoutUnZip(string directoryPath, string zipFileName)
        {
            string[] files = Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string extension = Path.GetExtension(file);
                if (extension == ".txt")
                {
                    //Make sure the first 9 characters of the filename are numeric(matching our file definition)
                    int? num = fileName.SafeSubString(0, 9).ToIntNullable();
                    if (num.HasValue)
                    {
                        List<string> matchingPdfFiles = files.Where(s => Path.GetFileName(s).StartsWith(fileName.SafeSubString(0, 9)) && Path.GetExtension(s) == ".pdf").ToList();
                        if (matchingPdfFiles.Count == 1)
                        {
                            HandleFile(file, matchingPdfFiles[0], zipFileName);
                        }
                        else if(matchingPdfFiles.Count < 1)
                        {
                            string ErrorMessage = $"1 Corresponding PDF not found, found {matchingPdfFiles.Count} files, please review. txtFile: {file}";

                            new ManualReviewHelper(DA, logRun).FlagForManualReview(new List<Error>() { new Error(null, ErrorMessage)}, fileName.SafeSubString(0, 9));
                            logRun.AddNotification(ErrorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        }
                        else
                        {
                            HandleFile(file, matchingPdfFiles[0], zipFileName);
                        }
                    }
                }
            }
        }

        public void ProcessOpenFileDialog()
        {
            //Get files to process from user
            Dialog.Info.Ok("Please select the text file you wish to process.");
            OpenFileDialog txtFile = new OpenFileDialog();
            txtFile.ShowDialog();

            Dialog.Info.Ok("Please select the pdf file corresponding to the selected text file.");
            OpenFileDialog pdfFile = new OpenFileDialog();
            pdfFile.ShowDialog();

            HandleFile(txtFile.FileName, pdfFile.FileName, null);
        }

        private void HandleFile(string txt, string pdf, string zipFile)
        {
            //reset some internal tracking in DataAccess
            DA.resetLocalVariablesForNewFile();

            //Create a manual review helper so that invalid info can get logged to review
            DA.ZipFileName = zipFile;
            ManualReviewHelper review = new ManualReviewHelper(DA, logRun);

            //Add the ssn parsed from the filename to the DataAccess to error if one is not able to be parsed
            DA.FileNameSsn = Path.GetFileName(txt).SafeSubString(0, 9);
            DA.ReviewHelper = review;

            //Read the .txt file and the .pdf file
            List<List<FileInformation>> fileInfo = new FileParser(logRun).ProcessFile(txt, DA);
            MonetaryPdfParser pdfInfo = new MonetaryPdfParser(DA, logRun);
            pdfInfo.ProcessMonetaryHistory(pdf);

            try
            {
                //Validate All .txt file and .pdf file information
                foreach (List<FileInformation> infoList in fileInfo)
                {
                    foreach (FileInformation info in infoList)
                    {
                        info.ValidateInformation(txt, DA);
                    }
                }

                pdfInfo.ValidateInformation(pdf, review);

                //Write the files to the database
                foreach (List<FileInformation> infoList in fileInfo)
                {
                    foreach (FileInformation info in infoList)
                    {
                        //Note the order matters because it sets the foreign key for the set being processed
                        info.WriteToDatabase(DA);
                    }
                }
                pdfInfo.WriteToDatabase(DA);
            }
            catch (Exception e)
            {
                if (e is FormatException || e.InnerException is FormatException)
                {
                    return;
                }
                throw e;
            }

            //Create review record if necessary
            List<Error> errorLog = new List<Error>();
            foreach (List<FileInformation> infoList in fileInfo)
            {
                foreach (FileInformation info in infoList)
                {
                    //Note the order matters because it sets the foreign key for the set being processed
                    if(info.ExceptionLog != null)
                    {
                        errorLog.AddRange(info.ExceptionLog);
                    }
                }
            }
            //log manual review if there were manual review errors.
            if (errorLog.Count > 0)
            {
                new ManualReviewHelper(DA, logRun).FlagForManualReview(errorLog);
            }
            //PDF currently doesn't use an exception log, mostly for the borrower activty parsing
        }

        private List<string> GetZipFiles(string directoryPath)
        {
            return Directory.GetFiles(directoryPath).Where(
                s => Path.GetFileName(s).ToUpper().StartsWith("COLL")
                && Path.GetExtension(s).ToUpper() == ".ZIP").ToList();
        }

        private ExtractResult Extract(ZipEntry entry, string destination)
        {
            try
            {
                if (File.Exists(destination))
                {
                    return ExtractResult.AlreadyExists;
                }

                using (var writer = FS.OpenWrite(destination))
                    entry.Extract(writer);

            }
            catch (Exception e)
            {
                return ExtractResult.Error;
            }
            return ExtractResult.Success;
        }
    }
}
