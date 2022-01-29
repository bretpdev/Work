using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CheckFileExists
{
    public class CheckFiles
    {
        public static ProcessLogData LogData { get; set; }

        public CheckFiles()
        {
        }

        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (!DataAccessHelper.StandardArgsCheck(args, "CheckFileExists"))
                return 1;

            Processs();

            return 0;
        }

        private static void Processs()
        {
            Console.WriteLine("Gathering file names");
            List<FileData> files = GetFilesNames();
            Console.WriteLine("Checking if files exist");
            List<FileData> noFiles = CheckIfFilesExist(files);
            Console.WriteLine("Writing files to file");
            WriteFilesToFile(noFiles);
        }

        private static List<FileData> GetFilesNames()
        {
            SqlConnection conn = new SqlConnection(DataAccessHelper.GetConnectionString(DataAccessHelper.Database.ECorrFed, DataAccessHelper.CurrentMode));
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT [Path], Ssn FROM DocumentDetails WHERE Printed > '2/3/15 1:00' AND Printed < '2/6/15 23:00' AND LetterId IN (1027, 1028, 1029)", conn);
            List<FileData> files = new List<FileData>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    files.Add(DataAccessHelper.Populate<FileData>(reader, false));
            }
            conn.Close();
            conn.Dispose();
            return files;
        }

        private static List<FileData> CheckIfFilesExist(List<FileData> files)
        {
            List<FileData> NoFilesFound = new List<FileData>();
            foreach (FileData item in files)
            {
                string file = item.Path.Substring(item.Path.LastIndexOf(@"\") + 1, item.Path.Length - item.Path.LastIndexOf(@"\") - 1);
                string path = Path.Combine(@"Z:\Centralized Printing\E-Corr\", file);
                if (!File.Exists(path))
                    NoFilesFound.Add(new FileData() { Path = file, Ssn = item.Ssn });
            }
            return NoFilesFound;
        }

        private static void WriteFilesToFile(List<FileData> noFiles)
        {
            using (StreamWriter sw = new StreamWriter(EnterpriseFileSystem.TempFolder + "NoFilesExist.txt", false))
            {
                sw.WriteLine("Path, SSN"); //Write out header
                foreach (FileData file in noFiles)
                {
                    sw.WriteLine(string.Format("{0}, {1}", file.Path, file.Ssn));
                }
            }
        }
    }
}
