using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO.Compression;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using System.Threading;
using WinSCP;

namespace ECORXMLGEN
{
    public class XMLGenerator
    {
        private static ProcessLogRun LogData;
        private const string XmlPrepFolder = "ECORR_XML_PREP";
        private const string XmlFolder = "ECORR_XML";
        private DataAccess DA { get; set; }
        private int ReturnVal { get; set; }

        public XMLGenerator(ProcessLogRun logData)
        {
            LogData = logData;
            DA = new DataAccess(LogData.ProcessLogId);
            ReturnVal = 0;
        }

        /// <summary>
        /// Generates an XML file for all unprocessed records and creates a zip file to send to AES.
        /// </summary>
        public int GenerateXml()
        {
            RunExtractor();
            CleanupDeadDirectories();
            SendUnsentFiles();
            Parallel.For(0, Program.NumberOfThreads, index =>
            {
                Process(index);
            });

            return ReturnVal;
        }

        private void RunExtractor()
        {
            ECorrDocumentDetailsExtractor.Program.Main(new string[] { Program.SkipDays.ToString() });
        }

        private void CleanupDeadDirectories()
        {
            string dir = EnterpriseFileSystem.GetPath(XmlPrepFolder);
            var deadDirs = Directory.GetDirectories(dir).Where(p => new DirectoryInfo(p).LastWriteTime < DateTime.Now.AddDays(-2));
            foreach (string deadDir in deadDirs)
                DeleteDir(deadDir);
        }

        private void SendUnsentFiles()
        {
            List<string> unsentFiles = Directory.GetFiles(EnterpriseFileSystem.GetPath(XmlFolder)).ToList();
            foreach (string file in unsentFiles)
                SendXmlFile(Path.Combine(EnterpriseFileSystem.GetPath(XmlFolder), file));
        }

        private DocumentProperties GetNextUnprocessedRecord()
        {
            try
            {
                return DA.GetNextUnprocessedRecord();
            }
            catch (InvalidOperationException)//Nothing was returned
            {
                return null;
            }
        }

        private void Process(int threadNumber)
        {
            string dir = InitializeDirectories();
            string zipFileName = string.Format("{2}{3}_IN_ZIP_{0:MMddyyhhmmss}_{1}.zip", DateTime.Now, Guid.NewGuid().ToBase64String(), DataAccessHelper.TestMode ? "TEST_" : "",
                DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? "KU" : "UT");
            DocumentProperties document = GetNextUnprocessedRecord();
            List<int> documentDetailIds = new List<int>();
            int fileCount = 1;
            while (document != null)
            {
                WriteToConsole(string.Format("Begining to Process DocumentDetailsId:{0}", document.DocumentDetailsId), threadNumber);
                bool result = WriteXmlDoc(document, dir, zipFileName, threadNumber);
                if (!result)
                {
                    document = GetNextUnprocessedRecord();
                    continue;
                }
                documentDetailIds.Add(document.DocumentDetailsId);
                if (fileCount == 500) //500 PDF 500 XML files
                {
                    ProcessCompleteFile(dir, zipFileName, documentDetailIds, threadNumber);
                    documentDetailIds = new List<int>();
                    dir = EnterpriseFileSystem.GetPath(XmlPrepFolder) + Guid.NewGuid().ToBase64String();
                    Directory.CreateDirectory(dir);
                    zipFileName = string.Format("{2}{3}_IN_ZIP_{0:MMddyyhhmmss}_{1}.zip", DateTime.Now, Guid.NewGuid().ToBase64String(), DataAccessHelper.TestMode ? "TEST_" : "",
                        DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? "KU" : "UT");
                    fileCount = 0;
                }

                fileCount++;
                document = GetNextUnprocessedRecord();
            }

            if (Directory.GetFiles(dir).Count() > 1)//Clean up the files that do not have 500 in them
                ProcessCompleteFile(dir, zipFileName, documentDetailIds, threadNumber);
            DeleteDir(dir);
        }

        public void DeleteDir(string dir)
        {
            if (Directory.Exists(dir))
                Repeater.TryRepeatedly(() => Directory.Delete(dir, true));
        }

        private void ProcessCompleteFile(string dir, string zipFileName, List<int> documentDetailIds, int threadNumber)
        {
            WriteToConsole(string.Format("About to create zip file: {0}", zipFileName), threadNumber);
            string moveDir = dir.Substring(0, (dir.LastIndexOf(@"\") + 1));
            Repeater.TryRepeatedly(() => ZipFile.CreateFromDirectory(dir, Path.Combine(EnterpriseFileSystem.GetPath(XmlPrepFolder), zipFileName), CompressionLevel.Optimal, false));
            File.Move(Path.Combine(moveDir, zipFileName), Path.Combine(EnterpriseFileSystem.GetPath(XmlFolder), zipFileName));
            WriteToConsole(string.Format("zip file: {0} Has been created, about to send the file.", zipFileName), threadNumber);
            DeleteDir(dir);
            DA.UpdateZipFIleName(documentDetailIds.Select(p => new { Id = p }).ToList().ToDataTable(), zipFileName);
            if (Program.SendFiles)
                SendXmlFile(Path.Combine(EnterpriseFileSystem.GetPath(XmlFolder), zipFileName));

            WriteToConsole(string.Format("zip file: {0} Has been Sent", zipFileName), threadNumber);
        }

        private static string InitializeDirectories()
        {
            string dir = EnterpriseFileSystem.GetPath(XmlPrepFolder);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!Directory.Exists(EnterpriseFileSystem.GetPath(XmlFolder)))
                Directory.CreateDirectory(EnterpriseFileSystem.GetPath(XmlFolder));

            dir = dir + Guid.NewGuid().ToBase64String();
            Directory.CreateDirectory(dir);
            return dir;
        }

        private bool SendXmlFile(string fileToSend)
        {
            List<ErrorFiles> errFiles = new List<ErrorFiles>();
            string type = DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? "EcorrTransfer" : "UHEAAEcorrTransfer";
            BatchProcessingHelper cred = BatchProcessingHelper.GetNextAvailableId(string.Empty, type);
            using (Session session = new Session())
            {
                SessionOptions ss = GetSessionOptions(cred.UserName, cred.Password);

                session.Open(ss);
                bool success = false;
                LogData.AddNotification(string.Format("Current Time {1}, about to send zip file {0}", fileToSend, DateTime.Now), NotificationType.EndOfJob, NotificationSeverityType.Informational);
                try
                {
                    TransferOperationResult result = session.PutFiles(fileToSend, Path.GetFileName(fileToSend), true, new TransferOptions() { PreserveTimestamp = false, TransferMode = TransferMode.Binary });
                    success = result.IsSuccess;
                    if (result.IsSuccess)
                        LogData.AddNotification(string.Format("Current Time {1}, file {0} was sent successfully.", fileToSend, DateTime.Now), NotificationType.EndOfJob, NotificationSeverityType.Informational);
                    else
                        result.Check();
                }
                catch (Exception ex)
                {
                    ErrorFiles err = GetErrorObject(fileToSend, ex, errFiles);
                    errFiles.Add(err);
                    LogData.AddNotification(string.Format("Current Time {1}, file {0} was not sent successfully. File has failed {2} times max retry count is 5. See exception for details", Path.GetFileName(fileToSend), DateTime.Now, err.FailCount), NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                }

                return success;
            }
        }

        private static ErrorFiles GetErrorObject(string localFile, Exception ex, List<ErrorFiles> errFiles)
        {
            return new ErrorFiles()
            {
                FileName = Path.GetFileName(localFile),
                Ex = ex,
                FailCount = errFiles.Count() + 1
            };
        }

        private void MoveErrorFile(string localFile)
        {
            string dir = EnterpriseFileSystem.GetPath("ECORR_XML_ERRORFILES");
            if (Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.Move(localFile, Path.Combine(dir, Path.GetFileName(localFile)));
            LogData.AddNotification(string.Format("Unable to send file {0}, file has been moved to {1}", Path.GetFileName(localFile), dir), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            ReturnVal = 1;
        }

        private SessionOptions GetSessionOptions(string userId, string password)
        {
            if (DataAccessHelper.TestMode)
            {
                if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
                {
                    userId = "SVUHEAA";
                    password = DA.GetTestPassword(userId);
                    return new SessionOptions()
                    {
                        FtpMode = FtpMode.Active,
                        Protocol = Protocol.Sftp,
                        UserName = userId,
                        Password = password,
                        HostName = "sfwebqa.pheaa.org",

                        SshHostKeyFingerprint = DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ?
                        "ssh-rsa 2048 e6:d9:66:d1:fa:a4:37:31:73:4a:01:a4:ba:57:13:a6" : "ssh-rsa 2048 e6:d9:66:d1:fa:a4:37:31:73:4a:01:a4:ba:57:13:a6",
                    };
                }

                return null;
            }
            else
            {
                return new SessionOptions()
                {
                    FtpMode = FtpMode.Active,
                    Protocol = Protocol.Sftp,
                    UserName = userId,
                    Password = password,
                    HostName = "sftp.pheaa.org",

                    SshHostKeyFingerprint = DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ?
                        "ssh-rsa 2048 e6:d9:66:d1:fa:a4:37:31:73:4a:01:a4:ba:57:13:a6" : "ssh-rsa 2048 e6:d9:66:d1:fa:a4:37:31:73:4a:01:a4:ba:57:13:a6",
                };
            }
        }

        private void WriteToConsole(string message, int threadNumber)
        {
            Console.WriteLine("{0: MM/dd/yyyy hh:mm:ss}; Thread#:{2}; {1}", DateTime.Now, message, threadNumber);
        }

        private bool WriteXmlDoc(DocumentProperties document, string dir, string zipFileName, int threadNumber)
        {
            string file = Path.GetFileName(document.PATH);
            string path = EnterpriseFileSystem.GetPath("ECORRLocation");
            if (!File.Exists(Path.Combine(path, file)))
            {
                string message = string.Format("Unable to move file {0}.  File does not exist", Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), file), DateTime.Now);
                LogData.AddNotification(message, NotificationType.NoFile, NotificationSeverityType.Critical);
                WriteToConsole(message, threadNumber);
                return false;
            }
            string xmlFile = string.Format("INDX_{0}_{1}.xml", Guid.NewGuid(), DataAccessHelper.CurrentRegion);
            WriteToConsole(string.Format("About to create Index file {0}", xmlFile), threadNumber);
            XmlTextWriter writer = new XmlTextWriter(Path.Combine(dir, xmlFile), Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument(); // <?xml version="1.0" encoding="utf-8"?>
            writer.WriteStartElement("InputRequest");
            WriteAttributesAndValues("InputRequest", writer);
            writer.WriteStartElement("RoutingInformation");
            WriteRoutingInformation("RoutingInformation", writer);
            writer.WriteStartElement("Environment");
            writer.WriteString(DataAccessHelper.TestMode ? DataAccessHelper.CurrentMode.ToString().ToUpper() : "PROD");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("Document");
            WriteAttributesAndValues("Document", writer);
            writer.WriteStartElement("DocumentStorageInformation");
            writer.WriteStartElement("ObjectStore");
            writer.WriteString(DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "UHEAA" : "CORNERSTONE");
            writer.WriteEndElement();
            writer.WriteStartElement("DocumentLibrary");
            writer.WriteString(DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "UHEAAeCorr" : "CORNERSTONEeCorr");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("DocumentLocation");
            writer.WriteStartElement("path");
            writer.WriteString(string.Format(document.PATH, Path.GetFileNameWithoutExtension(zipFileName)));
            writer.WriteEndElement();
            writer.WriteEndElement();
            WriteDetailsForEcorrXmlFile(document, writer);
            writer.WriteEndElement();

            try
            {
                string toFile = Path.Combine(dir, file);
                File.Copy(Path.Combine(path, file), toFile, true);

                writer.WriteEndDocument();
                writer.Close();
            }
            catch (FileNotFoundException ex)
            {
                LogData.AddNotification(string.Format("Unable to move file {0}.  File does not exist", Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), file).ToString()), NotificationType.NoFile, NotificationSeverityType.Critical, ex);
                return false;
            }

            WriteToConsole(string.Format("Index file {0} created.", xmlFile), threadNumber);

            return true;
        }

        /// <summary>
        /// Writes the document properties for the XML file.
        /// </summary>
        /// <param name="docData">Object with all of the document properties</param>
        private void WriteDetailsForEcorrXmlFile(DocumentProperties docData, XmlTextWriter writer)
        {
            writer.WriteStartElement("DocumentProperties");
            foreach (PropertyInfo item in docData.GetType().GetProperties().Skip(2)) //Skipping the first 2 properties as one is used before this loop and the other is an ID column.
            {
                if (item.Name == "CorrespondenceFormatId")
                    continue;
                var value = docData.GetType().GetProperty(item.Name).GetValue(docData, null);
                if (value != null)
                {
                    writer.WriteStartElement("DocumentProperty");
                    writer.WriteStartElement("name");
                    writer.WriteString(item.Name);
                    writer.WriteEndElement();
                    writer.WriteStartElement("value");
                    if (docData.GetType().GetProperty(item.Name).PropertyType == typeof(DateTime) || docData.GetType().GetProperty(item.Name).PropertyType == typeof(DateTime?))
                    {
                        DateTime? temp = (DateTime)docData.GetType().GetProperty(item.Name).GetValue(docData, null);
                        if (temp.HasValue)
                        {
                            string tempDate = temp.Value.ToString("MM/dd/yyyy");
                            writer.WriteString(tempDate.ToDate().AddHours(2).ToString("MM/dd/yyyy hh:mm:ss") + " AM");
                        }
                    }
                    else
                    {
                        if (item.Name == "BILL_SEQ")
                        {
                            writer.WriteString(docData.GetType().GetProperty(item.Name).GetValue(docData, null).ToString().Trim().PadLeft(4, '0'));
                        }
                        else if (item.Name == "CORR_METHOD")
                        {
                            if (value.ToString().ToUpper() == "EMAILNOTIFY")
                                writer.WriteString("Email Notify");
                            else if (value.ToString().ToUpper() == "DIRECTDEBIT")
                                writer.WriteString("Direct Debit");
                            else
                                writer.WriteString(docData.GetType().GetProperty(item.Name).GetValue(docData, null).ToString());
                        }
                        else
                            writer.WriteString(docData.GetType().GetProperty(item.Name).GetValue(docData, null).ToString());
                    }

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the attributes for the XML file
        /// </summary>
        /// <param name="tag"></param>
        private void WriteAttributesAndValues(string tag, XmlTextWriter writer)
        {
            List<EcorrXmlAttributeValues> attrValues = DA.GetXMLAttributes(tag);
            foreach (EcorrXmlAttributeValues item in attrValues)
                writer.WriteAttributeString(item.Attribute, item.Value);
        }

        private void WriteRoutingInformation(string tag, XmlTextWriter writer)
        {
            List<EcorrXmlAttributeValues> attrValues = DA.GetXMLAttributes(tag);
            foreach (EcorrXmlAttributeValues item in attrValues)
            {
                //This tag requires a little bit of formatting so that the value is unique and has a value indicating if this is a test.
                if (item.Attribute == "clientUUID")
                    item.Value = string.Format(item.Value, Guid.NewGuid().ToBase64String().Substring(1), DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live ? "TEST" : "");

                writer.WriteStartElement(item.Attribute);
                writer.WriteString(item.Value);
                writer.WriteEndElement();
            }
        }
    }
}
