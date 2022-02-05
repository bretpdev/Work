using Q;
using System.Windows.Forms;
using System.IO;

namespace COSUSPPUT
{
    public class COSUSPPUT : BatchScriptBase
    {

        const string R2_SAS_FILE_NAME_TEMPLATE = "ULWR22.LWR22R2*";
        const string R3_SAS_FILE_NAME_TEMPLATE = "ULWR22.LWR22R3*";

        /// <summary>
        /// Constructor.
        /// </summary>
        public COSUSPPUT(ReflectionInterface ri)
            : base(ri, "COSUSPPUT")
        {
        }

        /// <summary>
        /// Main starting point.
        /// </summary>
        public override void Main()
        {
            if (MessageBox.Show("This is the Compass Suspense PUT Script.  Click OK to continue or Cancel to end the script.","Compass Suspense PUT Script",MessageBoxButtons.OKCancel,MessageBoxIcon.Information) != DialogResult.OK)
            {
                EndDLLScript();
            }
            //figure out file names
            TestModeResults results = TestMode("");
            //check for file existance and handle multiple files
            string r2FileToProcess = DeleteOldFilesReturnMostCurrent(results.FtpFolder, R2_SAS_FILE_NAME_TEMPLATE, Common.FileOptions.None);
            string r3FileToProcess = DeleteOldFilesReturnMostCurrent(results.FtpFolder, R3_SAS_FILE_NAME_TEMPLATE, Common.FileOptions.None);
            if (r2FileToProcess == string.Empty || r3FileToProcess == string.Empty)
            {
                //file didn't exist
                MessageBox.Show("The script couldn't find one of the files to process.  Please contact Systems Support.","Data File Found",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                EndDLLScript();
            }
            if (new FileInfo(r2FileToProcess).Length == 0 && new FileInfo(r3FileToProcess).Length == 0)
            {
                //files were empty
                File.Delete(r2FileToProcess);
                File.Delete(r3FileToProcess);
                MessageBox.Show("Processing Complete!","Processing Complete!",MessageBoxButtons.OK,MessageBoxIcon.Information);
                EndDLLScript();
            }
            if (new FileInfo(r2FileToProcess).Length == 0)
            {
                File.Delete(r2FileToProcess);
                r2FileToProcess = string.Empty; //empty string indicates later to not process file
            }
            if (new FileInfo(r3FileToProcess).Length == 0)
            {
                File.Delete(r3FileToProcess);
                r3FileToProcess = string.Empty; //empty string indicates later to not process file
            }
            //R2 & R3 Files to process identified.  Ready for processing!
            if (r2FileToProcess != string.Empty) //process if not empty
            {
                FileProcessor processor = new FileProcessor(RI, r2FileToProcess, FileProcessor.FileType.R2, ScriptID);
                processor.Process();
                File.Delete(r2FileToProcess); //delete data file
            }
            if (r3FileToProcess != string.Empty) //process if not empty
            {
                FileProcessor processor = new FileProcessor(RI, r3FileToProcess, FileProcessor.FileType.R3, ScriptID);
                processor.Process();
                File.Delete(r3FileToProcess); //delete data file
            }
            //check for error log
            string errorLog = string.Format("{0}{1}", DataAccessBase.PersonalDataDirectory, FileProcessor.ERROR_LOG);
            if (File.Exists(errorLog))
            {
                MessageBox.Show(string.Format("Errors were encountered during processing.  Please check {0} for details.", errorLog),"Error Log Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            MessageBox.Show("Processing Complete!","Processing Complete!",MessageBoxButtons.OK,MessageBoxIcon.Information);
            EndDLLScript();
        }

    }
}
