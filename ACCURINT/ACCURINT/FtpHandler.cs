using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Uheaa.Common.DataAccess;

namespace ACCURINT
{
    class FtpHandler
    {
        public const string COREFTP_EXECUTABLE = @"C:\Program Files\CoreFTP\CoreFTP.exe";

        public static bool CoreFtpInstalledCheck()
        {
            if (!File.Exists(COREFTP_EXECUTABLE) && !DataAccessHelper.TestMode)
                return false;
            return true;
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
        public string[] DownloadFiles(string filePattern, string localPath)
        {
            const string ACCURINT_DOWNLOAD_PATH = "outgoing/*";

            // Run CoreFTP if not in test so that it pulls files down from Accurint
            if (!DataAccessHelper.TestMode)
            {
                string lftpUploadArgs = string.Format("O -site Lexis_Nexis -d {0} -p {1} -delsrc", ACCURINT_DOWNLOAD_PATH, localPath);
                using (Process coreFTP = Process.Start(COREFTP_EXECUTABLE, lftpUploadArgs))
                {
                    coreFTP.WaitForExit();
                    if (coreFTP.ExitCode != 0)
                    {
                        return null;
                    }
                }
            }

            //See what we got back. If there are any results, delete the file(s) from Accurint.
            string[] files = Directory.GetFiles((EnterpriseFileSystem.TempFolder + @"AddressPhone\FromL-N\"), filePattern);

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    File.Copy(file, EnterpriseFileSystem.TempFolder + file.Substring(file.LastIndexOf(@"\") + 1, (file.Length - file.LastIndexOf(@"\") - 1)));
                    File.Delete(file);
                }
            }

            string[] downloadedFiles = Directory.GetFiles(localPath, filePattern);

            return downloadedFiles;
        }

        /// <summary>
        /// Uploads a file to Accurint, and verifies that the file was successfully sent.
        /// </summary>
        /// <param name="localFile">The full path and name of the file to upload.</param>
        public bool UploadFile(string localFile)
        {
            const string ACCURINT_UPLOAD_PATH = "incoming/";

            //Run CoreFTP, passing it args, to upload file
            string lftpUploadArgs = string.Format("O -site Lexis_Nexis -u {0} -p {1}", localFile, ACCURINT_UPLOAD_PATH);
            using (Process coreFTP = Process.Start(COREFTP_EXECUTABLE, lftpUploadArgs))
            {
                coreFTP.WaitForExit();
                if (coreFTP.ExitCode != 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
