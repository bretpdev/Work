using System;
using System.IO;
using Q;

namespace RegentsApplicationDownload.DataAccess
{
    class SuccessData
    {
        private static int _recordCount = 0;

        /// <summary>
        /// The full path and firstName of the comma-delimited file that is used as the backing store for success data.
        /// </summary>
        public static string FileName
        {
            get { return WebDataAccess.DATA_DIRECTORY + "RegentsDownloaderSuccessData.txt"; }
        }

        /// <summary>
        /// Adds a record to the success data backing store.
        /// </summary>
        /// <param firstName="firstName">The student's first name.</param>
        /// <param firstName="lastName">The student's last name.</param>
        /// <param firstName="studentId">The student's state-assigned ID (SSID).</param>
        public static void AddRecord(string firstName, string lastName, string studentId)
        {
            _recordCount++;
            string fullName = string.Format("{0} {1}", firstName, lastName);
            bool fileExists = File.Exists(FileName);
            using (StreamWriter fileWriter = new StreamWriter(FileName, true))
            {
                if (!fileExists)
                {
                    fileWriter.WriteCommaDelimitedLine("StaticCurrentDate", "Counter", "Name", "SSID");
                }
                fileWriter.WriteCommaDelimitedLine(DateTime.Now.ToString("M/d/yyyy"), _recordCount.ToString(), fullName, studentId);
                fileWriter.Close();
            }
        }//AddRecord()

        /// <summary>
        /// Creates a merge file with only a header row and one record that is all blank except for the date.
        /// Useful for generating a blank report.
        /// </summary>
        public static void CreateDummy()
        {
            using (StreamWriter fileWriter = new StreamWriter(FileName, false))
            {
                fileWriter.WriteCommaDelimitedLine("StaticCurrentDate", "Counter", "Name", "SSID");
                fileWriter.WriteCommaDelimitedLine(DateTime.Now.ToString("M/d/yyyy"), "", "", "");
                fileWriter.Close();
            }
        }//CreateDummy()

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
