using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static System.Console;

namespace AESRCVDIAL
{
    public class FileManagement
    {
        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        List<DialerFiles> Files { get; set; }

        public FileManagement(ProcessLogRun logRun)
        {
            LogRun = logRun;
            DA = new DataAccess(logRun.LDA);
            Files = DA.GetFiles();
        }

        public void Process(DateTime addedAt)
        {
            if (addedAt.Date >= DateTime.Now.Date) //If there is a date provided in the past, don't load any data. It will create the files for that date.
                ReadInData(CheckFilesExists()); //Checks for files that need to be added to the DB and loads them
            CreateFiles(addedAt); //Create the new files for the passed in date, today is default
        }

        /// <summary>
        /// Checks to see if the file is missing or empty and remove it from the list of files to process
        /// </summary>
        private List<string> CheckFilesExists()
        {
            List<string> foundFiles = new List<string>();
            foreach (DialerFiles file in Files)
            {
                int count = DA.GetRecordCountForFile($"{EnterpriseFileSystem.FtpFolder}{file.FileName.Replace("*", "")}");
                if (!Directory.EnumerateFiles(EnterpriseFileSystem.FtpFolder, file.FileName, SearchOption.TopDirectoryOnly).Any() && file.IsRequired && count == 0)
                {
                    string message = $"The {file.FileName} file is missing. Please add the file to the FTP folder and start the script again. The rest of the files have been processed.";
                    LogRun.AddNotification(message, NotificationType.NoFile, NotificationSeverityType.Critical);
                    WriteLine(message);
                    continue;
                }
                if (Directory.EnumerateFiles(EnterpriseFileSystem.FtpFolder, file.FileName, SearchOption.TopDirectoryOnly).Count() > 1)
                {
                    string message = $"There are more than 1 {file.FileName} files in the FTP folder. Please remove all but the current file and start the script up.";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                    continue;
                }
                string fileName = Directory.EnumerateFiles(EnterpriseFileSystem.FtpFolder, file.FileName, SearchOption.TopDirectoryOnly).FirstOrDefault();
                if (fileName != null && new FileInfo(fileName).Length == 0)
                {
                    string message = $"The {file.FileName} file is empty. Please verify that there was no data to process in the {file.FileName} today.";
                    LogRun.AddNotification(message, NotificationType.EmptyFile, NotificationSeverityType.Critical);
                    WriteLine(message);
                    continue;
                }
                if (fileName.IsPopulated())
                    foundFiles.Add(fileName);
            }

            return foundFiles;
        }

        /// <summary>
        /// Reads in the records from the file in FTP and adds the data to the CallHistory table
        /// </summary>
        private void ReadInData(List<string> files)
        {
            foreach (string file in files)
            {
                string[] lines = FS.ReadAllLines(file);
                int count = DA.GetRecordCount(file);
                if (count != lines.Count() && count != 0)
                    WriteLine($"There are {lines.Count()} records in the file and {count} records already in the database for the {file} file added today. Recovering to add the rest of the file.");
                if (count == lines.Count())
                {
                    WriteLine($"There are {count} records loaded in the database with FileName {file} and {lines.Count()} in the file. The file has already been loaded for the day.");
                    LogRun.AddNotification($"The {file} file has already been loaded for the day and will be deleted.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                    continue; //The file has been loaded already
                }
                else
                {
                    WriteLine($"Adding data to database for file: {file}; Record Count: {lines.Count()}");
                    Parallel.ForEach(lines, new ParallelOptions() { MaxDegreeOfParallelism = 1 }, line =>
                    {
                        AddRecordToDB(line, file);
                    });
                }
                RepeatResults<Exception> result = Repeater.TryRepeatedly(() => File.Copy(file, $"{EnterpriseFileSystem.GetPath("DialerArchive")}{Path.GetFileName(file)}"));
                if (!result.Successful)
                    LogRun.AddNotification($"There was an error archiving the files. The files will not be deleted so they can be manually moved to {EnterpriseFileSystem.GetPath("DialerArchive")}. EX: {result.CaughtExceptions[0].Message}", NotificationType.ErrorReport, NotificationSeverityType.Critical, result.CaughtExceptions[0]);
#if !DEBUG
                else
                    Repeater.TryRepeatedly(() => FS.Delete(file)); //Delete the file after it adds all the data and copies them to the archive folders
#endif
            }
        }

        /// <summary>
        /// Creates a FileData object with the data from one record in the file and adds it to the database
        /// </summary>
        private void AddRecordToDB(string line, string fileName)
        {
            FileData data = new FileData();
            data.FileName = fileName;
            data.TargetsId = line.SafeSubString(0, 9).Trim();
            data.QueueRegion = line.SafeSubString(9, 3).Trim();
            data.CriticalTaskIndicator = line.SafeSubString(28, 1).Trim();
            data.BorrowersName = line.SafeSubString(75, 50).Trim();
            data.BorrowersPaymentAmount = line.SafeSubString(140, 9).Trim();
            data.BorrowersOutstandingBalance = line.SafeSubString(149, 9).Trim();
            data.BorrowersAccountNumber = line.SafeSubString(202, 10).Trim();
            string attemptDate = $"{line.SafeSubString(223, 2)}{line.SafeSubString(225, 2)}{line.SafeSubString(219, 4)}".Trim();
            data.TargetsDateLastAttempt = attemptDate.IsPopulated() ? attemptDate.Insert(2, "/").Insert(5, "/") : "01/01/1900";
            string contactDate = $"{line.SafeSubString(231, 2)}{line.SafeSubString(233, 2)}{line.SafeSubString(227, 4)}".Trim();
            data.TargetsDateLastContact = contactDate.IsPopulated() ? contactDate.Insert(2, "/").Insert(5, "/") : "01/01/1900";
            data.TargetsRelationshipToBorrower = line.SafeSubString(243, 1).Trim();
            data.TargetsName = line.SafeSubString(244, 50).Trim();
            data.TargetsZip = line.SafeSubString(396, 14).Trim();
            data.TargetsHomePhoneType = line.SafeSubString(440, 1).Trim();
            data.TargetsHomePhone = line.SafeSubString(441, 17).Trim();
            data.TargetsAltPhoneType = line.SafeSubString(458, 1).Trim();
            data.TargetsAltPhone = line.SafeSubString(459, 17).Trim();
            data.TargetsOtherPhoneType = line.SafeSubString(476, 1).Trim();
            data.TargetsOtherPhone = line.SafeSubString(477, 17).Trim();
            data.TargetsTCPAConsentForHomePhone = line.SafeSubString(494, 1).Trim();
            data.TargetsTCPAConsentForAltPhone = line.SafeSubString(495, 1).Trim();
            data.TargetsTCPAConsentForOtherPhone = line.SafeSubString(496, 1).Trim();
            data.RegardsToNumberOfDaysDelinquent = line.SafeSubString(509, 5).Trim();
            data.RegardsToName = line.SafeSubString(524, 50).Trim();
            string skipDate = $"{line.SafeSubString(727, 2)}{line.SafeSubString(729, 2)}{line.SafeSubString(723, 4)}".Trim();
            data.RegardsToSkipStartDate = skipDate.IsPopulated() ? skipDate.Insert(2, "/").Insert(5, "/") : "01/01/1900";
            data.PreviouslyRehabbedIndicator = line.SafeSubString(760, 1).Trim();

            if (ValidateData(data, line))
            {

                int id = DA.InsertSingleRecord(data);
                if (id > 0)
                    WriteLine($"Added record to database for borrower: {data.TargetsId}; DB record: {id}");
            }
        }

        /// <summary>
        /// Checks to make sure the required fields are in the file and PL the record if it is not.
        /// </summary>
        private bool ValidateData(FileData data, string line)
        {
            List<string> missingFields = new List<string>();
            if (data.TargetsId.IsNullOrEmpty())
                missingFields.Add("TargetsId");
            if (data.BorrowersName.IsNullOrEmpty())
                missingFields.Add("BorrowersName");
            if (data.BorrowersAccountNumber.IsNullOrEmpty())
                missingFields.Add("BorrowersAccountNumber");
            if (data.QueueRegion.IsNullOrEmpty())
                missingFields.Add("QueueRegion");
            if (data.TargetsName.IsNullOrEmpty())
                missingFields.Add("TargetsName");

            if (missingFields.Count() > 0)
            {
                string message = $"The following fields are missing in a record: {string.Join(",", missingFields)}. File line: {line}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                WriteLine(message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets all of the data from the table for the date passed in, default is current date. Creates a new csv pipe delimited file.
        /// </summary>
        private void CreateFiles(DateTime addedAt)
        {
            List<FileData> fileData = DA.GetRecordsByDate(addedAt);
            foreach (DialerFiles file in Files)
            {
                List<FileData> fData = fileData.Where(p => p.FileName.Contains(file.FileName.Replace("*", ""))).ToList();
                string fileName = $"{addedAt.ToShortDateString().Replace("/", "_")}.{file.OutputFileName}.csv";
                string csvFileDialer = $"{EnterpriseFileSystem.GetPath("DialerArchive")}{fileName}";
                if (fData.Count > 0)
                {
                    string csvFileDialerNoConsent = $"{EnterpriseFileSystem.GetPath("LGPNOCONSENT")}{addedAt.ToShortDateString().Replace("/", "_")}.{file.OutputFileName}_NoConsent.csv";
                    WriteLine($"Creating {csvFileDialer} file.");
                    using StreamWriter sw = new StreamWriter(csvFileDialer);
                    using StreamWriter swbu = new StreamWriter(csvFileDialerNoConsent);
                    string header = "TargetsId|QueueRegion|CriticalTaskIndicator|BorrowersName|BorrowersPaymentAmount|BorrowersOutstandingBalance|BorrowersAccountNumber|TargetsDateLastAttempt|TargetsDateLastContact|TargetsRelationshipToBorrower|TargetsName|TargetsZip|TargetsHomePhoneType|TargetsHomePhone|TargetsAltPhoneType|TargetsAltPhone|TargetsOtherPhoneType|TargetsOtherPhone|TargetsTCPAConsentForHomePhone|TargetsTCPAConsentForAltPhone|TargetsTCPAConsentForOtherPHone|RegardsToNumberOfDaysDelinquent|RegardsToName|RegardsToSkipStartDate|PreviouslyRehabbedIndicator";
                    sw.WriteLine(header);
                    swbu.WriteLine(header.Replace("|", ","));
                    bool hadNoConsent = false;
                    foreach (FileData data in fData)
                    {
                        string daysDeliq = data.RegardsToNumberOfDaysDelinquent.IsPopulated() ? data.RegardsToNumberOfDaysDelinquent.Trim().ToInt().ToString() : "";

                        if (data.TargetsTCPAConsentForHomePhone.ToUpper() == "N" || data.TargetsTCPAConsentForAltPhone.ToUpper() == "N" || data.TargetsTCPAConsentForOtherPhone.ToUpper() == "N")
                        {
                            hadNoConsent = true;
                            swbu.WriteLine($"{data.TargetsId.Trim()},{data.QueueRegion.Trim()},{data.CriticalTaskIndicator.Trim()},{data.BorrowersName.Trim()},{data.BorrowersPaymentAmount.Trim().FormatAmount()}," +
                                $"{data.BorrowersOutstandingBalance.Trim().FormatAmount()},{data.BorrowersAccountNumber.Trim()},{data.TargetsDateLastAttempt.Trim()},{data.TargetsDateLastContact.Trim()}," +
                                $"{data.TargetsRelationshipToBorrower.Trim()},{data.TargetsName.Trim()},{data.TargetsZip.Trim()},{data.TargetsHomePhoneType.CheckNoConsent(data.TargetsTCPAConsentForHomePhone)}," +
                                $"{data.TargetsHomePhone.CheckNoConsent(data.TargetsTCPAConsentForHomePhone)},{data.TargetsAltPhoneType.CheckNoConsent(data.TargetsTCPAConsentForAltPhone)}," +
                                $"{data.TargetsAltPhone.CheckNoConsent(data.TargetsTCPAConsentForAltPhone)},{data.TargetsOtherPhoneType.CheckNoConsent(data.TargetsTCPAConsentForOtherPhone)}," +
                                $"{data.TargetsOtherPhone.CheckNoConsent(data.TargetsTCPAConsentForOtherPhone)},{data.TargetsTCPAConsentForHomePhone.CheckNoConsent(data.TargetsTCPAConsentForHomePhone)}," +
                                $"{data.TargetsTCPAConsentForAltPhone.CheckNoConsent(data.TargetsTCPAConsentForAltPhone)},{data.TargetsTCPAConsentForOtherPhone.CheckNoConsent(data.TargetsTCPAConsentForOtherPhone)},{daysDeliq},{data.RegardsToName.Trim()}," +
                                $"{data.RegardsToSkipStartDate.Trim()},{data.PreviouslyRehabbedIndicator.Trim()}");
                        }
                        
                        if (data.TargetsTCPAConsentForHomePhone.ToUpper() == "Y" || data.TargetsTCPAConsentForAltPhone.ToUpper() == "Y" || data.TargetsTCPAConsentForOtherPhone.ToUpper() == "Y")
                        {
                            sw.WriteLine($"{data.TargetsId.Trim()}|{data.QueueRegion.Trim()}|{data.CriticalTaskIndicator.Trim()}|{data.BorrowersName.Trim()}|{data.BorrowersPaymentAmount.Trim().FormatAmount()}|" +
                                $"{data.BorrowersOutstandingBalance.Trim().FormatAmount()}|{data.BorrowersAccountNumber.Trim()}|{data.TargetsDateLastAttempt.Trim()}|{data.TargetsDateLastContact.Trim()}|" +
                                $"{data.TargetsRelationshipToBorrower.Trim()}|{data.TargetsName.Trim()}|{data.TargetsZip.Trim()}|{data.TargetsHomePhoneType.CheckConsent(data.TargetsTCPAConsentForHomePhone)}|" +
                                $"{data.TargetsHomePhone.CheckConsent(data.TargetsTCPAConsentForHomePhone)}|{data.TargetsAltPhoneType.CheckConsent(data.TargetsTCPAConsentForAltPhone)}|" +
                                $"{data.TargetsAltPhone.CheckConsent(data.TargetsTCPAConsentForAltPhone)}|{data.TargetsOtherPhoneType.CheckConsent(data.TargetsTCPAConsentForOtherPhone)}|" +
                                $"{data.TargetsOtherPhone.CheckConsent(data.TargetsTCPAConsentForOtherPhone)}|{data.TargetsTCPAConsentForHomePhone.CheckConsent(data.TargetsTCPAConsentForHomePhone)}|" +
                                $"{data.TargetsTCPAConsentForAltPhone.CheckConsent(data.TargetsTCPAConsentForAltPhone)}|{data.TargetsTCPAConsentForOtherPhone.CheckConsent(data.TargetsTCPAConsentForOtherPhone)}|{daysDeliq}|{data.RegardsToName.Trim()}|" +
                                $"{data.RegardsToSkipStartDate.Trim()}|{data.PreviouslyRehabbedIndicator.Trim()}");
                        }
                    }
                    if (!hadNoConsent)
                        swbu.WriteLine("No records for today");
                }
                Repeater.TryRepeatedly(() => File.Copy(csvFileDialer, $"{EnterpriseFileSystem.GetPath("DialerAutoUpload")}{fileName}"));
            }
        }
    }

    public static class Decimals
    {
        public static string FormatAmount(this string field)
        {
            if (field.ToInt() > 0)
                return field.Insert(field.Length - 2, ".").ToDecimal().ToString();
            else
                return "0.00";
        }

        public static string CheckNoConsent(this string field, string consent)
        {
            if (consent == "N")
                return field.Trim();
            if (field == "N")
                return "";
            return "";
        }

        public static string CheckConsent(this string field, string consent)
        {
            if (consent == "Y")
                return field.Trim();
            return "";
        }
    }


}