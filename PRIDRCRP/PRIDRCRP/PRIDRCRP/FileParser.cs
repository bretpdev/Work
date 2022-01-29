using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PRIDRCRP
{
    public class FileParser
    {
        public enum FileSection
        {
            Header, //Title Statement Before Section I
            Section_I, //Borrower Information
            Section_II, //Reference Information
            Section_III, //Financial Transaction Summary
            Section_IV, //Chronological Activity History
            Section_V, //Borrower Preference Activity/History
            PDF //Information is coming from the PDF
        }
        private int SectionBlockCounter { get; set; } = 0;
        private ProcessLogRun logRun { get; set; }

        public FileParser(ProcessLogRun logRun)
        {
            this.logRun = logRun;
        }

        /// <summary>
        /// Processes a single file into the database
        /// </summary>
        /// <param name="file">file should be the fully qualified path to the file</param>
        public List<List<FileInformation>> ProcessFile(string file, DataAccess DA)
        {
            List<List<FileInformation>> fileInformation = new List<List<FileInformation>>();
            fileInformation.Add(GetAllFileInformationObjects(DA));
            FileSection section  = FileSection.Header;
            try
            {
                using (StreamR sr = new StreamR(file))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Update the file section if the line is part of a new section.
                        int counterBeforeUpdate = SectionBlockCounter;
                        FileSection? fs = UpdateFileSection(line);
                        if(fs.HasValue)
                        {
                            section = fs.Value;
                        }

                        if(SectionBlockCounter > counterBeforeUpdate && SectionBlockCounter > 1) //Don't add 1 the first time through
                        {
                            fileInformation.Add(GetAllFileInformationObjects(DA));
                        }

                        //Iterate over the collection of File Informations accumulating data for the valid section
                        foreach(FileInformation fileInfo in fileInformation.Last())
                        {
                            if(fileInfo.Section == section)
                            {
                                fileInfo.GetInformation(line);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("The file:\"{0}\" could not be read:", file));
                Console.WriteLine(e.Message);
            }

            return fileInformation;
        }

        private List<FileInformation> GetAllFileInformationObjects(DataAccess DA)
        {
            List<FileInformation> fileInformation = new List<FileInformation>();
            fileInformation.Add(new BorrowerInformation(logRun)); //Needs to be first in the list
            fileInformation.Add(new DisbursementInformation(logRun));
            fileInformation.Add(new BorrowerActivityInformation(logRun, DA));
            fileInformation.Add(new PaymentHistoryInformation(logRun));
            return fileInformation;
        }

        public FileSection? UpdateFileSection(string line)
        {
            string lineUpper = line.ToUpper();

            if(lineUpper.Contains("SECTION I. BORROWER INFORMATION"))
            {
                SectionBlockCounter++;
                return FileSection.Section_I;
            }
            if (lineUpper.Contains("SECTION II. REFERENCE INFORMATION"))
            {
                return FileSection.Section_II;
            }
            if (lineUpper.Contains("SECTION III. FINANCIAL TRANSACTION SUMMARY"))
            {
                return FileSection.Section_III;
            }
            if (lineUpper.Contains("SECTION IV. CHRONOLOGICAL ACTIVITY HISTORY"))
            {
                return FileSection.Section_IV;
            }
            if (lineUpper.Contains("SECTION V. BORROWER PREFERENCE ACTIVITY/HISTORY"))
            {
                return FileSection.Section_V;
            }
            return null;
        }
    }
}
