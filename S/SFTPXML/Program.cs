using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSCP;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.IO;
using System.Reflection;

namespace SFTPXML
{
    class Program
    {
        private static List<ErrorFiles> ErrFiles { get; set; }
        static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "SFTPXML"))
                return 1;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogData logData = ProcessLogger.RegisterApplication("SFTPXML", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            ErrFiles = new List<ErrorFiles>();
            BatchProcessingHelper cred = BatchProcessingHelper.GetNextAvailableId("SFTPXML", "EcorrTransfer");

            while (Directory.GetFiles(EnterpriseFileSystem.GetPath("ECORR_XML")).Count() != 0)
            {
                string file = Directory.GetFiles(EnterpriseFileSystem.GetPath("ECORR_XML")).First();
                SendFile(file, cred.UserName, cred.Password, logData);
            }

            ProcessLogger.LogEnd(logData.ProcessLogId);

            return 0;
        }

        private static bool SendFile(string localFile, string userId, string password, ProcessLogData logData)
        {
            using (Session session = new Session())
            {
                SessionOptions ss = GetSessionOptions(userId, password);
                session.Open(ss);
                ProcessLogger.AddNotification(logData.ProcessLogId, string.Format("Current Time {1}, about to send zip file {0}", localFile, DateTime.Now), NotificationType.EndOfJob, NotificationSeverityType.Informational);
                TransferOperationResult result = session.PutFiles(localFile, Path.GetFileName(localFile), true, new TransferOptions() { PreserveTimestamp = false, TransferMode = TransferMode.Binary });
                if (result.IsSuccess)
                    ProcessLogger.AddNotification(logData.ProcessLogId, string.Format("Current Time {1}, file {0} was sent successfully.", localFile, DateTime.Now), NotificationType.EndOfJob, NotificationSeverityType.Informational);
                else
                {
                    try
                    {
                        result.Check();
                    }
                    catch (Exception ex)
                    {
                        ErrorFiles err = GetErrorObject(localFile, ex);
                        ErrFiles.Add(err);
                        ProcessLogger.AddNotification(logData.ProcessLogId, string.Format("Current Time {1}, file {0} was not sent successfully. File has failed {2} times max retry count is 5. See exception for details", Path.GetFileName(localFile), DateTime.Now), NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);
                        if (err.FailCount == 5)
                            MoveErrorFile(localFile, logData);
                    }
                }
                return result.IsSuccess;
            }
        }

        private static ErrorFiles GetErrorObject(string localFile, Exception ex)
        {
            return new ErrorFiles()
            {
                FileName = Path.GetFileName(localFile),
                Ex = ex,
                FailCount = ErrFiles.Where(q => q.FileName == Path.GetFileName(localFile)).Count() + 1
            };
        }

        private static void MoveErrorFile(string localFile, ProcessLogData logData)
        {
            string dir = @"C:\Enterprise Program Files\ecorr_out\ErrorFiles\";
            if (Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.Move(localFile, Path.Combine(dir, Path.GetFileName(localFile)));
            ProcessLogger.AddNotification(logData.ProcessLogId, string.Format("Unable to send file {0}, file has been moved to {1}", Path.GetFileName(localFile), dir), NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }

        private static SessionOptions GetSessionOptions(string userId, string password)
        {
            return new SessionOptions()
                {
                    FtpMode = FtpMode.Active,
                    Protocol = Protocol.Sftp,
                    UserName = userId,
                    Password = password,
                    HostName = "sftp.pheaa.org",
                    
                    SshHostKeyFingerprint = "ssh-rsa 2048 e6:d9:66:d1:fa:a4:37:31:73:4a:01:a4:ba:57:13:a6",
                };
        }
    }
}

