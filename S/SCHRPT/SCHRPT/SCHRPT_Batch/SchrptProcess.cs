using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace SCHRPT_Batch
{
    public class SchrptProcess
    {
        static int recordNum = 0;
        private Dictionary<int, string> ReportTypes = null;
        private Dictionary<int, string> Schools = null;
        private Dictionary<int, string> Recipients = null;
        private ConcurrentDictionary<int, Tuple<int,int>> SchoolRecipients = null;

        //Locks
        private static readonly System.Object FileWriterLock = new System.Object();

        private int NumThreads = 4;
        private string Path;
        private int NumberOfSchoolsPerEmail = 200;

        public string ScriptId { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public LogDataAccess LDA { get; set; }
        public DataAccessHelper.Database DB { get; set; }

        public SchrptProcess(ProcessLogRun logRun, DataAccessHelper.Region region, int numberOfSchoolsPerEmail)
        {
            ScriptId = "SCHRPT";
            LogRun = logRun;
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, false);
            DB = DataAccessHelper.Database.Cls;
            DA = new DataAccess(LDA, DB);
            Path = string.Format(EnterpriseFileSystem.GetPath("SCHRPT_Batch") + "schrpt_report{0}.csv", recordNum.ToString());
            NumberOfSchoolsPerEmail = numberOfSchoolsPerEmail;

            ReportTypes = new Dictionary<int, string>();
            Schools = new Dictionary<int, string>();
            Recipients = new Dictionary<int, string>();
            SchoolRecipients = new ConcurrentDictionary<int, Tuple<int,int>>();
        }

        public SchrptProcess(ProcessLogRun logRun, DataAccessHelper.Region region, string path)
        {
            ScriptId = "SCHRPT";
            LogRun = logRun;
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, false);
            DB = DataAccessHelper.Database.Cls;
            DA = new DataAccess(LDA, DB);
            Path = path; //will only be used for the first path

            ReportTypes = new Dictionary<int, string>();
            Schools = new Dictionary<int, string>();
            Recipients = new Dictionary<int, string>();
            SchoolRecipients = new ConcurrentDictionary<int, Tuple<int, int>>();
        }

        public int Process()
        {
            int result = SchrptProcessing();
            LogRun.LogEnd();
            return result;
        }

        private int SchrptProcessing()
        {
            //Delete files from previous run
            DeleteFiles();

            List<SchrptRecord> recipients = DA.GetRecipients();
            List<SchrptRecord> schools = DA.GetSchools();
            List<SchrptRecord> schoolRecipients = DA.GetSchoolRecipients();
            List<SchrptRecord> reportTypes = DA.GetReportTypes();

            foreach (var records in reportTypes.GroupBy(p => p.ReportTypeId))
                ProcessReportTypes(records);
            foreach (var records in recipients.GroupBy(p => p.RecipientId))
                ProcessRecipients(records);
            foreach (var records in schools.GroupBy(p => p.SchoolId))
                ProcessSchools(records);
            foreach (var records in schoolRecipients.GroupBy(p => Tuple.Create<int,int>(p.RecipientId, p.ReportTypeId)))
                ProcessSchoolRecipientsThreaded(records);

            return 0;
        }

        private int SchrptProcessingIndividual()
        {
            List<SchrptRecord> schools = DA.GetSchools();
            List<SchrptRecord> reportTypes = DA.GetReportTypes();

            foreach (var records in reportTypes.GroupBy(p => p.ReportTypeId))
                ProcessReportTypes(records);
            foreach (var records in schools.GroupBy(p => p.SchoolId))
                ProcessSchools(records);

            return 0;
        }

        private void ProcessRecipients(IGrouping<int, SchrptRecord> records)
        {
            List<SchrptRecord> recordsByRecipient = records.ToList();
            if (!recordsByRecipient.Any())
                return;
            foreach (var recipient in recordsByRecipient)
            {
                Recipients.Add(recipient.RecipientId, recipient.Email);
                Console.WriteLine("RecipientId: " + recipient.RecipientId); //DEBUG PRINT
            }
        }
        private void ProcessSchools(IGrouping<int, SchrptRecord> records)
        {
            List<SchrptRecord> recordsBySchool = records.ToList();
            if (!recordsBySchool.Any())
                return;
            foreach (var school in recordsBySchool)
            {
                Schools.Add(school.SchoolId, school.SchoolCode + school.BranchCode);
                Console.WriteLine("SchoolId: " + school.SchoolCode + school.BranchCode); //DEBUG PRINT
            }
        }

        /// <summary>
        /// Runs the query specified by the ReportTypeId and compiles a report
        /// which is emailed if the requester has not received a report in the last
        /// 7 days with the same reportId
        /// </summary>
        /// <param name="records">Schrpt Records with the same RecipientId and ReportTypeId</param>
        private void ProcessSchoolRecipientsThreaded(IGrouping<Tuple<int,int> , SchrptRecord> records)
        {
            List<SchrptRecord> recordsBySchoolRecipient = records.ToList();
            if (!recordsBySchoolRecipient.Any())
                return;

            if(EmailExistsInLastWeek(new List<int>(recordsBySchoolRecipient.Select(r => r.SchoolRecipientId)), recordsBySchoolRecipient[0]))
            {
                string recipientEmail = Recipients[recordsBySchoolRecipient[0].RecipientId];
                Console.WriteLine("Email already delivered for recipient email: " +  recipientEmail + " SKIPPING");
                return;
            }
            if (!Directory.Exists(Directory.GetParent(Path).ToString()))
            {
                DirectoryInfo di = Directory.CreateDirectory(Directory.GetParent(Path).ToString());
            }

            //string filePath = CreateReportFile(recordsBySchoolRecipient);
            //Send multiple files split into groups of 200 schools
            List<string> filePaths = CreateReportFilesPartitionedSchools(recordsBySchoolRecipient, NumberOfSchoolsPerEmail);

            Console.WriteLine("Sending report as attachment to email address at " + Recipients[recordsBySchoolRecipient[0].RecipientId]);
            SendEmail(new List<int>(recordsBySchoolRecipient.Select(r => r.SchoolRecipientId)), recordsBySchoolRecipient[0], filePaths);
        }

        private string CreateReportFile(List<SchrptRecord> recordsBySchoolRecipient)
        {
            bool writeHeader = true;
            using (StreamWriter stream = new StreamWriter(Path))
            {
                using (TextWriter csvFile = TextWriter.Synchronized(stream))
                {
                    var options = new ParallelOptions() { MaxDegreeOfParallelism = NumThreads };
                    Parallel.ForEach(recordsBySchoolRecipient, options, r =>
                    {

                        var t = Tuple.Create<int, int>(r.RecipientId, r.ReportTypeId);
                        SchoolRecipients.AddOrUpdate(r.SchoolRecipientId, t, (k, v) => t);

                        Console.WriteLine("Report for School: " + Schools[r.SchoolId] + " With Recipient: " + r.RecipientId + " And Report Type: " + r.ReportTypeId);
                        //ExecuteReportTypeHelper(Schools[r.SchoolId], ReportTypes[r.ReportTypeId]);
                        DataTable ret = DA.GetStoredProcedure(Schools[r.SchoolId], ReportTypes[r.ReportTypeId]);

                        //Guarantee that the header is the first thing written
                        if (writeHeader)
                        {
                            lock (FileWriterLock)
                            {
                                StringBuilder csv = WriteToCsvFile(ret, writeHeader);
                                if (csv != null)
                                {
                                    char[] csvChars = csv.ToString().ToCharArray();
                                    csvFile.Write(csvChars);
                                    writeHeader = false;
                                }

                            }
                        }
                        else
                        {
                            StringBuilder csv = WriteToCsvFile(ret, writeHeader);
                            if (csv != null)
                            {
                                char[] csvChars = csv.ToString().ToCharArray();
                                csvFile.Write(csvChars);
                            }
                        }

                    });
                    csvFile.Close();
                }
            }
            return Path;
        }

        private List<string> CreateReportFilesPartitionedSchools(List<SchrptRecord> recordsBySchoolRecipient, int numberOfSchoolsPerEmail)
        {
            List<string> filePaths = new List<string>();
            List<List<SchrptRecord>> recordsBySchoolRecipientSplit = SplitList(recordsBySchoolRecipient, numberOfSchoolsPerEmail);
            List<string> FileAttachements = new List<string>();

            foreach(List<SchrptRecord> r in recordsBySchoolRecipientSplit)
            {
                string filePath = CreateReportFile(r);
                filePaths.Add(filePath);

                //Increment The File Number For The Next Batch
                recordNum = recordNum + 1;
                Path = string.Format(EnterpriseFileSystem.GetPath("SCHRPT_Batch") + "schrpt_report{0}.csv", recordNum.ToString());
            }

            return filePaths;
        }

        private List<List<SchrptRecord>> SplitList(List<SchrptRecord> records, int partitionSize)
        {
            List<List<SchrptRecord>> partitionedRecords = new List<List<SchrptRecord>>();
            for(int i = 0; i < records.Count; i+= partitionSize)
            {
                partitionedRecords.Add(records.GetRange(i, Math.Min(partitionSize, records.Count - i)));
            }
            return partitionedRecords;
        }

        private void DeleteFiles()
        {
            if (!Directory.Exists(Directory.GetParent(Path).ToString()))
            {
                return;
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(Directory.GetParent(Path).ToString());
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        private void ProcessReportTypes(IGrouping<int, SchrptRecord> records)
        {
            List<SchrptRecord> recordsByReportType = records.ToList();
            if (!recordsByReportType.Any())
                return;
            foreach (var report in recordsByReportType)
            {
                ReportTypes.Add(report.ReportTypeId, report.StoredProcedureName);
                Console.WriteLine("ReportTypeId: " + report.ReportTypeId + " StoredProcedureName: " + report.StoredProcedureName); //DEBUG PRINT
            }
        }

        private StringBuilder WriteToCsvFile(DataTable t, bool writeHeader)
        {
            if(t != null)
            {
                var csv = new StringBuilder();
                //Create Document Header
                if (writeHeader)
                {
                    foreach (DataColumn col in t.Columns)
                    {
                        csv.Append(col + ",");
                    }
                    csv.Remove(csv.Length - 1, 1);
                    csv.AppendLine();
                    writeHeader = false;
                }
                //Create Document Body
                foreach (DataRow row in t.Rows)
                {
                    foreach (DataColumn col in t.Columns)
                    {
                        if(row[col] == null || row[col].ToString().ToUpper() == "NULL")
                        {
                            csv.Append(",");
                        }
                        else
                        {
                            csv.Append(row[col] + ",");
                        }
                    }
                    csv.Remove(csv.Length - 1, 1);
                    csv.AppendLine();
                }
                return csv;
            }
            return null;
        }

        /// <summary>
        /// Sends the generated CSV report to the requester
        /// if they have not received a report of the same type 
        /// in the last 7 days
        /// </summary>
        /// <param name="schoolReipientIds">Ids from which the report was generated</param>
        /// <param name="recipient">email of the recipient</param>
        /// <returns>true if email sent, false otherwise</returns>
        private bool SendEmail(List<int> schoolReipientIds, SchrptRecord recipientRecord, List<string> attachments)
        {
            foreach(string attachment in attachments)
            {
                string recipient = Recipients[recipientRecord.RecipientId];

                //SEND EMAIL HERE
                string subject = "[SECURE] " + recipientRecord.CompanyName + " from Cornerstone";
                //string body = @"<html lang=""en""><body><p> " + recipientRecord.Name + @" </br></br>Attached are the delinquency reports for the borrowers who attended " + recipientRecord.CompanyName + @" If you have any questions please contact us at CampusContact@mycornerstoneloan.org.</br></br>Thank you,</br></br>Cornerstone</br>Systems Support</br></br>Please do not reply to this email.</p></body></html>";
                string body = recipientRecord.Name + "\n\nAttached are the delinquency reports for the borrowers who attended " + recipientRecord.CompanyName + " If you have any questions please contact us at CampusContact@mycornerstoneloan.org.\n\nThank you,\n\nCornerstone\nSystems Support\n\nPlease do not reply to this email";

                if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev)
                {
                    //***DO NOT CHANGE, WILL NOT REDIRECT EMAIL INTERNALLY***
                    TestEmailHelper.EmailHelper.SendMail(DataAccessHelper.TestMode, /*DO NOT TOUCH recipient*/"rileyjbigelow@gmail.com"/*DO NOT TOUCH*/, "donotreply@mycornerstoneloan.org", subject, body + "You were supposed to recieve this file: " + attachment, "", attachment, TestEmailHelper.EmailHelper.EmailImportance.Normal, true);
                }
                else if(DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live)
                {
                    EmailHelper.SendMail(DataAccessHelper.TestMode, recipient, "donotreply@mycornerstoneloan.org", subject, body, "", attachment, EmailHelper.EmailImportance.Normal, true);
                }
            }

            string formatedAttachments = FormatAttachments(attachments);
            //Add an entry for each schoolRecipientId because if one fails it will return before this
            for (int i = 0; i < schoolReipientIds.Count; i++)
            {
                DA.AddSchoolEmailHistory(schoolReipientIds[i], DateTime.Now, formatedAttachments);
            }

            return true;
        }

        /// <summary>
        /// Format the attachments list into a single string to store in the database
        /// </summary>
        private string FormatAttachments(List<string> attachments)
        {
            string linearizedAttachments = "";
            foreach (string attachment in attachments)
            {
                linearizedAttachments += System.IO.Path.GetFileName(attachment) + ",";
            }
            if (linearizedAttachments.Length > 0)
            {
                linearizedAttachments.Remove(linearizedAttachments.Length - 1, 1);
            }
            return linearizedAttachments;
        }

        private bool EmailExistsInLastWeek(List<int> schoolReipientIds, SchrptRecord recipientRecord)
        {
            string recipient = Recipients[recipientRecord.RecipientId];
            List<SchrptRecord> emailHistory = DA.GetSchoolEmailHistory();
            for (int i = 0; i < schoolReipientIds.Count; i++)
            {
                foreach (var record in emailHistory)
                {
                    if (record.SchoolRecipientId == schoolReipientIds[i] && record.EmailSentAt != null)
                    {
                        //If any report exists in the last 7 days with the same SchoolRecipientId
                        //Date filtering done at the SQL level
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Executes a provided sproc on the specified school and generates a csv report from the results
        /// </summary>
        /// <param name="schoolId">School to run on</param>
        /// <param name="reportId">report to run</param>
        /// <returns>false if empty results from sproc</returns>
        public CsvResult ProcessManualSchoolRequest(int reportId, IEnumerable<int> schoolIds)
        {
            SchrptProcessingIndividual();
            int rowCount = 0;
            string storedProcedure = ReportTypes[reportId];
            string results = "";
            bool writeHeader = true;
            foreach (var schoolId in schoolIds)
            {
                string schoolCode = Schools[schoolId];
                DataTable dt = DA.GetStoredProcedure(schoolCode, storedProcedure);
                var csv = WriteToCsvFile(dt, writeHeader);
                writeHeader = false;
                rowCount += dt.Rows.Count;
                results += csv.ToString(); //+ Environment.NewLine;
            }
            return new CsvResult(rowCount, results.Trim());
        }
    }
}
