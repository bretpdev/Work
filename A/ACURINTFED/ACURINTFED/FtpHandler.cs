using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using WinSCP;
using Uheaa.Common.DataAccess;

namespace ACURINTFED
{
    class FtpHandler
    {
        private const int FIFTHEEN_MINUTES = 1000 * 60 * 15; //Milliseconds, to use in Thread.Sleep().
        private readonly DateTime TIME_TO_GIVE_UP; //FTP attempts will end at this time.
        private readonly Credentials _credentials;

        public FtpHandler(Credentials credentials)
        {
            _credentials = credentials;
            TIME_TO_GIVE_UP = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0); //4:00 PM today
        }

        /// <summary>
        /// Downloads files from Accurint that match a given wildcard pattern.
        /// Files are deleted from the FTP server if the download is successful
        /// (i.e., at least one file is downloaded).
        /// </summary>
        /// <param name="filePattern">
        /// The name of the file to download.
        /// Provide the portion of the file name that you know about, plus a wildcard for the unknown part.
        /// </param>
        /// <param name="localPath">The local directory to which the files will be downloaded.</param>
        public IEnumerable<string> DownloadFiles(string filePattern, string localPath)
        {
            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live)
            {
                Thread processingThread = new Thread(new ThreadStart(DisplayProcessing)) { IsBackground = true };
                processingThread.Start();
                List<string> downloadedFiles = new List<string>();

                while (true)
                {
                    using (Session session = new Session())
                    {

                        SessionOptions ss = new SessionOptions();
                        ss.FtpMode = FtpMode.Active;
                        ss.Protocol = Protocol.Sftp;
                        ss.UserName = _credentials.UserName;
                        ss.Password = _credentials.Password;
                        string remotePath = "outgoing/*";

                        ss.HostName = "batchssl2.lexisnexis.com";
                        ss.SshHostKeyFingerprint = "ssh-rsa 2048 d9:4a:bc:39:1a:58:60:4d:18:44:04:58:0e:20:0f:0e";

                        if (!session.Opened)
                            session.Open(ss);

                        TransferOperationResult result = session.GetFiles(remotePath, EnterpriseFileSystem.TempFolder, true);
                        downloadedFiles = Directory.GetFiles(localPath, filePattern).ToList();

                        if (downloadedFiles.Count > 0)
                        {
                            break;
                        }
                        else if (DateTime.Now >= TIME_TO_GIVE_UP)
                        {
                            throw new Exception("No response file on Accurint FTP site");
                        }
                        else
                        {
                            Thread.Sleep(FIFTHEEN_MINUTES);
                        }
                    }
                }

                processingThread.Abort();
                return downloadedFiles;
            }
            else
            {
                return new string[] { string.Format("{0}LN_Output_AccurintFEDRequestFile_Test.pgp", EnterpriseFileSystem.TempFolder) };
            }
        }//DownloadFiles()

        /// <summary>
        /// Uploads a file to Accurint, and verifies that the file was successfully sent.
        /// </summary>
        /// <param name="localFile">The full path and name of the file to upload.</param>
        public bool UploadFile(string localFile)
        {
            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live)
            {
                using (Session session = new Session())
                {
                    SessionOptions ss = new SessionOptions();
                    ss.FtpMode = FtpMode.Active;
                    ss.Protocol = Protocol.Sftp;
                    ss.UserName = _credentials.UserName;
                    ss.Password = _credentials.Password;
                    string remotePath = "incoming/";

                    ss.HostName = "batchtransfer.seisint.com";
                    ss.SshHostKeyFingerprint = "ssh-rsa 2048 d9:4a:bc:39:1a:58:60:4d:18:44:04:58:0e:20:0f:0e";

                    session.Open(ss);
                    TransferOperationResult result = session.PutFiles(localFile, remotePath);
                    return result.IsSuccess;
                }
            }
            else
            {
                return true;
            }
        }//UploadFile()

        private static void DisplayProcessing()
        {
            using (FtpNotifier not = new FtpNotifier())
            {
                not.ShowDialog();
            }
        }
    }//class
}//namespace
