using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Security;
using System.IO.Compression;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
// Archival:
// Q:\Systems Support\Batch Scripts\ACS
// Creates new folder each week.

namespace ACS
{
    public class FileProcessor
    {
        private DataAccess DA;
        private ProcessLogRun PLR;
        private string ArchiveLocation;
        private List<string> ProcessedFiles;
        private ACS.Decrypter DecrypterHelper;
        private Parse_Spec ParseSpec;
        private List<AcsOlFileRecord> AcsRecords;
        public int ErrorsAddingRecords { get; private set; }

        public FileProcessor(DataAccess da, ProcessLogRun plr)
        {
            DA = da;
            PLR = plr;
            ArchiveLocation = EnterpriseFileSystem.GetPath("ACS");
            ProcessedFiles = new List<string>();
            DecrypterHelper = new Decrypter();
            ParseSpec = new Parse_Spec();
            AcsRecords = new List<AcsOlFileRecord>();

        }

        /// <summary>
        /// Call methods to unzip, extract, load data into database, and clean up and archive zip files.
        /// </summary>
        /// <param name="targetDate">A starting date if not processing previous weeks data, otherwise today.</param>
        /// <returns>List of borrowers for processing.</returns>
        public int AddRecordsToProcess(DateTime targetDate)
        {
            string subdirectory = IdentifySubdirectory(targetDate);
            if (subdirectory == "") // Subdirectory wasn't found, error was logged in helper method already
                return 1;

            List<string> lines = ReadFiles(subdirectory);
            if (!File.Exists(Path.Combine(subdirectory, "ACSIN.txt"))) // The archive file is the combined ACS file, wherein all records from the other weekly files are stored in it
                CreateArchiveFile(subdirectory, lines);

            LoadRecordsToDb();
            return ErrorsAddingRecords; // This is set inside the ReadFiles() method in the try-catches
        }

        private void LoadRecordsToDb()
        {
            foreach (var record in AcsRecords)
                DA.LoadRecord(record);
        }

        private string IdentifySubdirectory(DateTime targetDate)
        {
            string baseDirectory = Path.Combine(ArchiveLocation, targetDate.Year.ToString());
            List<string> possibleSubDirectoryNames = new List<string>(); // Since this is user-made, we have to see if they formatted the date differently than expected
            possibleSubDirectoryNames.Add(targetDate.ToString("MM-dd-yyyy"));
            possibleSubDirectoryNames.Add(targetDate.ToString("M-dd-yyyy"));
            possibleSubDirectoryNames.Add(targetDate.ToString("MM-d-yyyy"));
            possibleSubDirectoryNames.Add(targetDate.ToString("M-d-yyyy"));

            string subdirectory = "";
            foreach (string fileName in possibleSubDirectoryNames)
            {
                string subcheck = Path.Combine(baseDirectory, fileName);
                if (Directory.Exists(subcheck))
                {
                    subdirectory = subcheck;
                    break;
                }
            }

            if (string.IsNullOrEmpty(subdirectory))
                PLR.AddNotification($"Unable to identify folder that contains the ACS files. Expected to find the folder in {baseDirectory}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            return subdirectory;
        }

        private void CreateArchiveFile(string subdirectory, List<string> lines)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(subdirectory, "ACSIN.txt")))
            {
                foreach (string line in lines)
                {
                    try
                    {
                        sw.WriteLine(line);
                    }
                    catch (Exception ex)
                    {
                        string msg = $"Error trying to create the weekly ACSIN.txt file";
                        PLR.AddNotification(msg, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    }
                }
            }
        }

        private List<string> ReadFiles(string subdirectory)
        {
            List<FileInfo> fileInfos = new List<FileInfo>();
            foreach (string file in Directory.GetFiles(subdirectory).Where(p => Path.GetFileName(p).StartsWith("P103440PT")).ToList())
                fileInfos.Add(new FileInfo(file));
            List<string> linesFromFiles = new List<string>();

            foreach (FileInfo file in fileInfos)
            {
                using (StreamReader currFile = new StreamReader(file.FullName))
                {
                    string line = "";
                    string account = "";
                    string fileId = Path.GetFileName(file.FullName);
                    int lineCount = 0;
                    while ((line = ReadAcsLine(currFile, fileId, lineCount)) != null)
                    {
                        lineCount++;
                        if (line.Length == 1 || string.IsNullOrWhiteSpace(line))
                            break;

                        if (line.Length > 6 && line.SafeSubString(0, 6) == "H10344")
                            continue;

                        linesFromFiles.Add(line);
                        AcsOlFileRecord curr = ParseSpec.ParseData(line);
                        if (string.IsNullOrWhiteSpace(curr.NewAddress.Addr1) || string.IsNullOrWhiteSpace(curr.FirstName))
                        {
                            string msg = string.Format("Data contains a blank address or name. Check file {0}, line {1}.", fileId, lineCount);
                            PLR.AddNotification(msg, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                            continue;
                        }

                        account = line.Substring(17, 9);
                        account = account.Replace("/", "@");
                        if (!account.Contains("@"))
                        {
                            try
                            {
                                account = DecrypterHelper.DecryptSSN(account);
                            }
                            catch (Exception ex)
                            {
                                string msg = string.Format("Data contains invalid SSN characters. Check file {0}, line {1} positions 18-26.", fileId, lineCount);
                                ErrorsAddingRecords++;
                                PLR.AddNotification(msg, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                                continue;
                            }
                        }
                        curr.SSN = account;
                        curr.FileId = fileId;
                        AcsRecords.Add(curr);
                    }
                    currFile.Close();
                }
            }
            return linesFromFiles;
        }

        private string ReadAcsLine(StreamReader currFile, string fileId, int lineCount)
        {
            string line = null;
            try
            {
                line = currFile.ReadLine();
            }
            catch (Exception ex)
            {
                string msg = $"ACS record in the file {fileId}, line {lineCount}, encountered a StreamReader error";
                ErrorsAddingRecords++;
                PLR.AddNotification(msg, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
            return line;
        }
    }
}
