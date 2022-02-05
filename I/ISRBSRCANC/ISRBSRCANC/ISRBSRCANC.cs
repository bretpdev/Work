using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ISRBSRCANC
{
    public class ISRBSRCANC : BatchScript
    {
        public const string EOJ_Total = "Total number of records in the file";
        public const string EOJ_TotalProcessed = "Total number of records processed";
        public const string ERR_ErrorFound = "There was an error in processing record";
        public static List<string> EOJ_Fields = new List<string>() { EOJ_Total, EOJ_TotalProcessed, ERR_ErrorFound };

        public ISRBSRCANC(ReflectionInterface ri)
            : base(ri, "ISRBSRCANC", "ERR_BU19", "EOJ_BU19", EOJ_Fields, DataAccessHelper.Region.Uheaa)
        {
        }

        public override void Main()
        {
            try
            {
                StartupMessage("This starts the ISR / BSR Cancellation application");

                string file = GetFile();
                List<string> data = LoadData(file);

                ProcessFile(data);

                File.Delete(file);

                if (Err.HasErrors)
                    ProcessingComplete("Processing complete with errors. Please see the error report");
                else
                    ProcessingComplete();
            }
            catch (Exception ex)
            {
                Dialog.Error.Ok(ex.Message + "\r\n\r\n" + ex.InnerException);
            }
        }

        /// <summary>
        /// Open the SAS file, check if it exists or is empty
        /// </summary>
        /// <returns>FileName and location if it is not empty or missing</returns>
        private string GetFile()
        {
            string file = ScriptHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, "ULWD18.LWD18R2*", ScriptHelper.FileOptions.None);
            if (file == "")
            {
                Dialog.Error.Ok("The file is missing", "Missing File");
                EndDllScript();
            }
            else if (new FileInfo(file).Length == 0)
            {
                Dialog.Error.Ok("The file is empty", "Emtpy File");
                EndDllScript();
            }

            return file;
        }

        /// <summary>
        /// Open the file and load all the data into a list of strings
        /// </summary>
        /// <returns>List of Strings</returns>
        private List<string> LoadData(string file)
        {
            List<string> data = new List<string>();

            using (StreamReader sr = new StreamReader(file))
                while (!sr.EndOfStream)
                    data.Add(sr.ReadLine());
                
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void ProcessFile(List<string> data)
        {
            int counter = Recovery.RecoveryValue.IsNullOrEmpty() ? 0 : Recovery.RecoveryValue.ToInt();

            foreach (string ssn in data.Skip(counter))
            {
                FastPath("LC34C" + ssn + "01");
                if (!CheckForText(1, 60, "DEFAULT GLOBAL UPDATE"))
                {
                    Err.AddRecord(ERR_ErrorFound, new { SSN = ssn });
                    Eoj.Counts[ERR_ErrorFound].Increment();
                }
                else
                {
                    PutText(2, 56, "Y");
                    PutText(4, 10, "01");
                    PutText(4, 21, GetText(3, 71, 10), ReflectionInterface.Key.Enter, true);

                    if (CheckForText(22, 3, "49233"))
                        Eoj.Counts[EOJ_TotalProcessed].Increment();
                    else
                    {
                        Err.AddRecord(ERR_ErrorFound, new { SSN = ssn });
                        Eoj.Counts[ERR_ErrorFound].Increment();
                    }
                    
                }
                Eoj.Counts[EOJ_Total].Increment();
                Recovery.RecoveryValue = (++counter).ToString();
            }
        }

    }//End Class
}//End Namespace
