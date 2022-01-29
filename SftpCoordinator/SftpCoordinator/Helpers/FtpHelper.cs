using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using WinSCP;

namespace SftpCoordinator
{
    public static class FtpHelper
    {
        public static SessionOptions GetOptions(string path)
        {
            SessionOptions ss = new SessionOptions();
            ss.FtpMode = FtpMode.Active;
            ss.Protocol = Protocol.Sftp;

            var credentials = BatchProcessingHelper.GetNextAvailableId("SftpCoordinator", "SFTP");
            ss.UserName = credentials.UserName;
            ss.Password = DataAccess.GetPassword(credentials.UserName);
            BatchProcessingHelper.CloseConnection(credentials);

            string hostName = path.Substring("ftp://".Length);
            hostName = hostName.Remove(hostName.IndexOf('/'));
            ss.HostName = hostName;
            ss.SshHostKeyFingerprint = "ssh-rsa 1024 34:d7:05:da:eb:19:94:99:e2:d5:91:b2:ca:48:84:e5";

            return ss;
        }

        public static bool IsFtp(string path)
        {
            return path.StartsWith("ftp://");
        }

        public static string StripHostInfo(string path)
        {
            if (path.StartsWith("ftp://"))
                path = path.Substring(6);
            path = path.Substring(path.IndexOf('/') + 1);
            return path;
        }
    }
}
