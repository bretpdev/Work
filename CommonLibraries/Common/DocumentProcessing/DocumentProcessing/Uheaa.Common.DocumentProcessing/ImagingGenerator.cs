using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace Uheaa.Common.DocumentProcessing
{
    public class ImagingGenerator
    {
        public enum LetterRecipient
        {
            Borrower = 0,
            Reference = 1,
            Other = 2
        }

        private string ScriptId;
        private string UserId;
        private int NumberInControlFile;
        private string CurrentControlFile { get { return TempControlFiles.Last(); } }
        List<string> TempControlFiles;
        private readonly string date = DateTime.Now.ToString("MM/dd/yyyy");
        private readonly string time = DateTime.Now.ToString("T");
        private readonly string dateTime = DateTime.Now.ToString("G");
        private int CurrentLineCount = 0;
        private bool ShouldImageFile = true;
        public readonly string Folder = "";
        public readonly string ImagingPath = EnterpriseFileSystem.GetPath("Imaging");

        /// <summary>
        /// Creates a new Imaging object
        /// </summary>
        /// <param name="scriptId">Id of script being run</param>
        /// <param name="userId">Id of the user running the application</param>
        /// <param name="numInCtlFile">The number of records to be added to the control file. Default of 100; 0 for Unlimited.</param>
        public ImagingGenerator(string scriptId, string userId, bool imageFile = true, int numInCtlFile = 100)
        {
            ScriptId = scriptId;
            UserId = userId;
            NumberInControlFile = numInCtlFile;
            ShouldImageFile = imageFile;
            Folder = string.Format("{0}{1}\\", EnterpriseFileSystem.TempFolder, scriptId + Guid.NewGuid().ToString().Replace("-", ""));
            if (!Directory.Exists(Folder))
                FS.CreateDirectory(Folder);
            TempControlFiles = new List<string>(Directory.GetFiles(Folder, Path.GetFileName(GenerateControlFileName()) + "*")
                .OrderBy(t => new FileInfo(t).CreationTime));
            CreateControlFile();
        }

        /// <summary>
        /// Creates a new control file.
        /// </summary>
        private void CreateControlFile()
        {
            TempControlFiles.Add(GenerateControlFileName() + Guid.NewGuid() + ".ctl");
            CurrentLineCount = 0;

            using (StreamWriter sw = new StreamW(CurrentControlFile))
            {
                // Header line
                sw.WriteLine(@"~^Folder~{0}, Doc 1^Type~UTCR_TYPE^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~{1}^Attribute~SCAN_TIME~STR~{2}^Attribute~DESCRIPTION~STR~{0}", dateTime, date, time);
            }
        }

        /// <summary>
        /// Generates the name of the control file
        /// </summary>
        /// <returns></returns>
        private string GenerateControlFileName()
        {
            return string.Format("{0}ImagingControlFile_{1}_{2}_", Folder, ScriptId, UserId);
        }

        /// <summary>
        /// Adds the image being added to the control file
        /// </summary>
        /// <param name="destinationAndFileName">The destination of the file to image</param>
        /// <param name="docId">The Document ID</param>
        /// <param name="ssn">Borrower SSN</param>
        public void AddFile(string destinationAndFileName, string docId, string ssn)
        {
            //string fileExtension = Path.GetExtension(destinationAndFileName);
            //double secondsSinceMidnight = DateTime.Now.Subtract(DateTime.Today).TotalMilliseconds / 1000;
            //string timeStamp = secondsSinceMidnight.ToString().Replace(".", "").Substring(0, 7);
            //string imagingFolder = EnterpriseFileSystem.GetPath("IMAGING");
            //Don't create control file if NumberInControlFile is 0 because it was created in the constructor
            if ((CurrentLineCount == NumberInControlFile) && (NumberInControlFile != 0))
                CreateControlFile();
            using (StreamWriter sw = new StreamW(CurrentControlFile, true))
            {
                // Individual file line
                sw.WriteLine(@"DesktopDoc~{3}~{0}, Doc 1^Attribute~SSN~STR~{1}^Attribute~DOC_DATE~STR~{2}^Attribute~DOC_ID~STR~{4}^Attribute~SCAN_DATE~STR~{5:MM/dd/yyyy}^Attribute~SCAN_TIME~STR~{5:HH:mm:ss}", dateTime, ssn, date, destinationAndFileName, docId, DateTime.Now);
            }
            CurrentLineCount++;
        }

        /// <summary>
        /// Publishes the control file to the Imaging folder
        /// </summary>
        public void PublishControlFile()
        {
            if (ShouldImageFile)
            {
                //Move all the image files
                foreach (string file in Directory.GetFiles(Folder).Where(p => Path.GetExtension(p) != ".ctl"))
                {
                    FS.Move(file, Path.Combine(ImagingPath, Path.GetFileName(file)));
                }

                //Move all the control files
                foreach (string file in TempControlFiles)
                {
                    FS.Move(file, Path.Combine(ImagingPath, Path.GetFileName(file)));
                }
            }
        }

        /// <summary>
        /// Delete the folder created for temporary file storage.
        /// </summary>
        /// <param name="Folder"></param>
        public static void DeleteTempFolder(string Folder)
        {
            if (Directory.Exists(Folder))
                FS.Delete(Folder, true);
        }

    }//Class
}//Namespace
