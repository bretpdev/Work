using System;
using System.IO;
using WinSCP;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;

namespace SftpCoordinator
{
    public static class GenericFile
    {
        public static DateTime GetLastWriteTime(string path)
        {
            Console.WriteLine($"GetLastWriteTime Called for file: {path}");
            if (FtpHelper.IsFtp(path))
            {
                using (Session session = new Session())
                {
                    session.Open(FtpHelper.GetOptions(path));
                    return session.GetFileInfo(path).LastWriteTime;
                }
            }
            else
                return new FileInfo(path).LastWriteTime;
        }
        public static bool Exists(string path)
        {
            Console.WriteLine($"Checking for file existence for file: {path}");
            if (FtpHelper.IsFtp(path))
            {
                using (Session session = new Session())
                {
                    session.Open(FtpHelper.GetOptions(path));
                    return session.FileExists(FtpHelper.StripHostInfo(path));
                }
            }
            else
            {
                return File.Exists(path);
            }
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite = false)
        {
            //NOTE: FTP TO FTP wont work.
            TransferOperationResult result = null;
            if (FtpHelper.IsFtp(sourceFileName))
            {
                Console.WriteLine($"Source Path determined to be an ftp location: {sourceFileName}");
                using (Session session = new Session())
                {
                    session.Open(FtpHelper.GetOptions(sourceFileName));
                    result = session.GetFiles(FtpHelper.StripHostInfo(sourceFileName), destFileName);
                }
            }
            else if (FtpHelper.IsFtp(destFileName))
            {
                Console.WriteLine($"Destination Path determined to be an ftp location: {destFileName}");
                using (Session session = new Session())
                {
                    session.Open(FtpHelper.GetOptions(destFileName));
                    result = session.PutFiles(sourceFileName, FtpHelper.StripHostInfo(destFileName));
                }
            }
            else
            {
                try
                {
                    Console.WriteLine($"Source and Destination determined to be an local locations: {sourceFileName}, {destFileName}");
                    FS.Copy(sourceFileName.UpdatePath(), destFileName.UpdatePath(), overwrite);
                }
                catch (Exception ex)
                {
                    string message = $"File copy from {sourceFileName} to {destFileName} failed.";
                    Program.PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    throw new Exception(message);
                }
            }

            if (result != null && result.Failures.Count > 0)
            {
                string message = $"FTP Error copying {sourceFileName} to {destFileName}.  Error message: {result.Failures[0].ToString()}";
                throw new Exception(message, result.Failures[0]);
            }
        }

        public static void Delete(string path)
        {
            Console.WriteLine($"File delete called during cleanup on file: {path}");
            if (FtpHelper.IsFtp(path))
            {
                using (Session session = new Session())
                {
                    session.Open(FtpHelper.GetOptions(path));
                    session.RemoveFiles(FtpHelper.StripHostInfo(path));
                }
            }
            else
                Repeater.TryRepeatedly(() => FS.Delete(path.UpdatePath()));
        }

        public static string RemoveExtension(string filename, string ext)
        {
            if (!ext.StartsWith(".")) ext = "." + ext;
            if (filename.ToLower().EndsWith(ext))
                return filename.Substring(0, filename.Length - ext.Length);
            return filename;
        }

        public static string AddExtension(string filename, string ext)
        {
            if (!ext.StartsWith(".")) ext = "." + ext;
            return filename + ext;
        }
    }
}
