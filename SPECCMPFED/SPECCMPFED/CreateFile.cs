using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;



namespace SPECCMPFED
{
    class CreateFile
    {
        string[] _header_For_Eoj = { "Total Records Inserted", "Number of records read from original FCOSCUL file", "Number of Error Records" };
        private const string FILE_NAME = "FCOSCUL.prn";
        bool CalledByJams { get; set; }
        EndOfJobReport Eoj;
        ErrorReport Err;

        public CreateFile(bool calledByJams)
        {
            CalledByJams = calledByJams;
            Eoj = new EndOfJobReport("Special Campaign File Upload - Cornerstone", "EOJ_BU03", _header_For_Eoj);
            Err = new ErrorReport("Special Campaign File Upload - Cornerstone Error", "ERR_BU35");
        }

        /// <summary>
        /// Checks the file and write to new file
        /// </summary>
        /// <returns>0 if success 1 if failure</returns>
        public int Run()
        {
            PreRunCleanup();
            if (CopyFile() == 1)
                return 1;

            string fileToProcess = string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, FILE_NAME);

            if (CheckEmptyFile(fileToProcess) == 0)
                return 0;

            IEnumerable<FileData> orgFileData = ReadFile(fileToProcess);

            using (StreamWriter fileWrite = new StreamWriter(string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, FILE_NAME)))
            {
                foreach (FileData fData in orgFileData)
                    WriteToNewFile(fileWrite, fData);
            }

            ProcessingComplete(fileToProcess);

            return 0;
        }

        private void ProcessingComplete(string fileToProcess)
        {
            File.Copy(string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, FILE_NAME), string.Format("{0}Final_FCOSCUL_{1}.prn", EnterpriseFileSystem.GetPath("Special_Campaign_File_Upload_Archive"), DateTime.Now.ToShortDateString().Replace("/", "_")), true);
            File.Move(string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, FILE_NAME), string.Format("{0}Batch.autodialer.{1}", EnterpriseFileSystem.GetPath("Special_Campaign_File_Upload"), DateTime.Now.ToString("yyyy-MM-dd")));//move file to location where AES can pick it up
            File.Delete(fileToProcess);//delete temp file
            File.Delete(string.Format("{0}{1}", EnterpriseFileSystem.GetPath("DialerSFTP"), FILE_NAME));//delete file from SFTP server

            Eoj.Publish();//publish the end of job report
            Err.Publish();//publish the error report

            if (!CalledByJams)
                MessageBox.Show("Processing Complete", "Complete", MessageBoxButtons.OK);
        }

        private void WriteToNewFile(StreamWriter fileWrite, FileData fData)
        {
            Result rCode = DataAccess.GetArcAndComment(fData.ResultCode);//Get the Arc and Comment for the given result code
            if (rCode != null)
            {
                int userNameLength = fData.Agent.Length;//determine the length of the user ID.  Some ID's will have a length of 3 or 4 this will be used to pad the spaces

                fData.Agent = string.IsNullOrEmpty(fData.Agent) ? "       " : userNameLength == 3 ? fData.Agent + "    " : fData.Agent + "   ";//if no userName pad with all spaces

                //creating strings to padd the file
                string emptySpace24 = "                                           ";
                string emptySpace73 = "                                                        ";
                string emptySpace = "                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    ";
                string fileLine = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", "T1XG9   ", fData.Ssn, rCode.ResponseCode, rCode.Arc, DateTime.Now.ToString("MM/dd/yyyy"), fData.Time.Replace(":", "."), emptySpace24, fData.Agent, emptySpace73, rCode.ActivityCode, emptySpace);

                fileWrite.WriteLine(fileLine);//write the line to the file
                Eoj.Counts["Total Records Inserted"].Increment();
            }
            else
                Err.AddRecord("The result code was not found in CSYS table SCFU_ResultCodes_Arc_Comment", fData);
        }

        private int CheckEmptyFile(string fileToProcess)
        {
            if (File.ReadAllLines(fileToProcess).Count() < 1)
            {
                if (!CalledByJams)
                    MessageBox.Show("Processing Complete File was Empty", "Complete", MessageBoxButtons.OK);
                Eoj.Publish();
                Err.Publish();

                File.Copy(string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, FILE_NAME), string.Format("{0}Final_FCOSCUL_{1}.prn", EnterpriseFileSystem.GetPath("Special_Campaign_File_Upload_Archive"), DateTime.Now.ToShortDateString().Replace("/", "_")), true);
                File.Delete(string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, FILE_NAME));//delete the empty file from the T drive
                File.Delete(string.Format("{0}{1}", EnterpriseFileSystem.GetPath("DialerSFTP"), FILE_NAME));
                return 0;
            }

            return 1;
        }

        private int CopyFile()
        {
            try
            {
                File.Copy(string.Format("{0}{1}", EnterpriseFileSystem.GetPath("DialerSFTP"), FILE_NAME), string.Format("{0}FCOSCUL.prn", EnterpriseFileSystem.TempFolder));//copy to the T drive
            }
            catch (FileNotFoundException)
            {
                string message = string.Format("File {0} was not found in the {1} directory.  Please investigate and try again", FILE_NAME, EnterpriseFileSystem.GetPath("DialerSFTP"));
                if (!CalledByJams)
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Console.WriteLine(message);
                return 1;
            }

            File.Copy(string.Format("{0}{1}", EnterpriseFileSystem.GetPath("DialerSFTP"), FILE_NAME), string.Format("{0}Original_FCOSCUL_{1}.prn", EnterpriseFileSystem.GetPath("Special_Campaign_File_Upload_Archive"), DateTime.Now.ToShortDateString().Replace("/", "_")), true);//Copy to archive

            return 0;
        }

        private static void PreRunCleanup()
        {
            if (!Directory.Exists(EnterpriseFileSystem.GetPath("Special_Campaign_File_Upload")))
                Directory.CreateDirectory(EnterpriseFileSystem.GetPath("Special_Campaign_File_Upload"));

            if (!Directory.Exists(EnterpriseFileSystem.GetPath("Special_Campaign_File_Upload_Archive")))
                Directory.CreateDirectory(EnterpriseFileSystem.GetPath("Special_Campaign_File_Upload_Archive"));

            if (File.Exists(string.Format("{0}Special Campaign File Upload - Cornerstone.txt", EnterpriseFileSystem.TempFolder)))//delete eoj report if exsist there is no recovery so we want to start fresh each run
                File.Delete(string.Format("{0}Special Campaign File Upload - Cornerstone.txt", EnterpriseFileSystem.TempFolder));

            if (File.Exists(string.Format("{0}Special Campaign File Upload - Cornerstone Error.txt", EnterpriseFileSystem.TempFolder)))//delete err report if exsist there is no recovery so we want to start fresh each run
                File.Delete(string.Format("{0}Special Campaign File Upload - Cornerstone Error.txt", EnterpriseFileSystem.TempFolder));

            if (File.Exists(string.Format("{0}{1}", EnterpriseFileSystem.GetPath("Special_Campaign_File_Upload"), FILE_NAME)))//delete file if it is already there
                File.Delete(string.Format("{0}{1}", EnterpriseFileSystem.GetPath("Special_Campaign_File_Upload"), FILE_NAME));

            if (File.Exists(string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, FILE_NAME)))//delete file if it is already there
                File.Delete(string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, FILE_NAME));
        }

        private IEnumerable<FileData> ReadFile(string fileToProcess)
        {
            DataTable dt =  FileSystemHelper.CreateDataTableFromFile(fileToProcess);
            List<FileData> fData = new List<FileData>();

            foreach (DataRow dr in dt.Rows)
            {
                FileData file = new FileData();

                file.Ssn = dr.Field<string>(0);
                file.Phone = dr.Field<string>(3);
                file.Time = dr.Field<string>(4);
                file.ResultCode = dr.Field<string>(5);
                file.Agent = dr.Field<string>(6);
                fData.Add(file);

                Eoj.Counts["Number of records read from original FCOSCUL file"].Increment();
            }

            return fData;
        }
    }
}

