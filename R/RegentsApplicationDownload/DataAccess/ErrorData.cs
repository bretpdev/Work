using System;
using System.IO;
using Q;

namespace RegentsApplicationDownload.DataAccess
{
    class ErrorData
    {
        private static int _recordCount = 0;

        /// <summary>
        /// The full path and firstName of the comma-delimited file that is used as the backing store for error data.
        /// </summary>
        public static string FileName
        {
            get { return WebDataAccess.DATA_DIRECTORY + "RegentsDownloaderErrorData.txt"; }
        }

        /// <summary>
        /// Adds a record to the error data backing store.
        /// </summary>
        /// <param firstName="firstName">The student's first name.</param>
        /// <param firstName="lastName">The student's last name.</param>
        /// <param firstName="studentId">The student's state-assigned ID (SSID).</param>
        /// <param firstName="phoneNumber">The student's phone number.</param>
        /// <param firstName="emailAddress">The student's e-mail address.</param>
        public static void AddRecord(string firstName, string lastName, string studentId, string phoneNumber, string emailAddress)
        {
            _recordCount++;
            string fullName = string.Format("{0} {1}", firstName, lastName);
            bool fileExists = File.Exists(FileName);
            using (StreamWriter fileWriter = new StreamWriter(FileName, true))
            {
                if (!fileExists)
                {
                    fileWriter.WriteCommaDelimitedLine("StaticCurrentDate", "Counter", "Name", "SSID", "Phone", "Email");
                }
                fileWriter.WriteCommaDelimitedLine(DateTime.Now.ToString("M/d/yyyy"), _recordCount.ToString(), fullName, studentId, phoneNumber, emailAddress);
                fileWriter.Close();
            }
        }//AddRecord()

        public static void Delete()
        {
            if (File.Exists(FileName)) { File.Delete(FileName); }
        }//Delete()

        public static bool Exists()
        {
            return File.Exists(FileName);
        }//Exists()
    }//class
}//namespace
