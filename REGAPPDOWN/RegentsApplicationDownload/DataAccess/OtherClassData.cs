using System;
using System.IO;
using Q;

namespace RegentsApplicationDownload.DataAccess
{
    class OtherClassData
    {
        private static int _recordCount = 0;

        /// <summary>
        /// The full path and firstName of the comma-delimited file that is used as the backing store for other class data.
        /// </summary>
        public static string FileName
        {
            get { return WebDataAccess.DATA_DIRECTORY + "RegentsDownloaderOtherClassData.txt"; }
        }

        /// <summary>
        /// Adds a record to the other class data backing store.
        /// </summary>
        /// <param firstName="firstName">The student's first name.</param>
        /// <param firstName="lastName">The student's last name.</param>
		/// <param firstName="courseSegment">The category (English, Math, etc.) in which the class was entered.</param>
		/// <param firstName="className">The name of the class in question.</param>
        /// <param firstName="schoolName">The name of the school at which the class was taken.</param>
        /// <param firstName="gradeLevel">The grade in which the class was taken (e.g., 9, 10, 11, 12).</param>
        /// <param firstName="academicYear">The academic year in which the class was taken (e.g., 09/10, 10/11).</param>
        public static void AddRecord(string firstName, string lastName, string courseSegment, string className, string schoolName, string gradeLevel, string academicYear)
        {
            _recordCount++;
            string fullName = string.Format("{0} {1}", firstName, lastName);
            bool fileExists = File.Exists(FileName);
            using (StreamWriter fileWriter = new StreamWriter(FileName, true))
            {
                if (!fileExists)
                {
					fileWriter.WriteCommaDelimitedLine("StaticCurrentDate", "Counter", "Name", "CourseSegment", "ClassName", "HighSchool", "Grade", "AcademicYear");
                }
                fileWriter.WriteCommaDelimitedLine(DateTime.Now.ToString("M/d/yyyy"), _recordCount.ToString(), fullName, courseSegment, className, schoolName, gradeLevel, academicYear);
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
