using System;
using System.IO;
using Q;
using System.Windows.Forms;
using System.Collections.Generic;
using NDesk.Options;

namespace FILEMOVFED
{
    public class SftpFileMoveFed
    {

        public static void Main()
        {
            const string SCRIPT_ID = "FILEMOVFED";
            bool calledByJams = false;
            bool testMode = false;
            
            OptionSet p = new OptionSet() 
            {
				{ "test", "the mode to run the script in.", v => testMode = true },
				{ "jams", "the script is being run by JAMS.", v => calledByJams = true },
            };
            try
            {
                p.Parse(Environment.GetCommandLineArgs());
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
            }

            string scriptName = DataAccess.GetScriptName(SCRIPT_ID);
            ErrorReport errorReport = new ErrorReport(testMode, scriptName, "ERR_BU35", ScriptSessionBase.Region.CornerStone);
            EnterpriseFileSystem efs = new EnterpriseFileSystem(testMode, ScriptSessionBase.Region.CornerStone);

            //verify the user is logged in using the batchscripts account
            if (!Environment.UserName.Equals("batchscripts"))
            {
                MessageBox.Show("This script can only be run by the Batch Scripts user.", "Batch Scripts User Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            //prompt the user if the script is not being run by JAMS
            if (!calledByJams)
            {
                if (!MessageBox.Show("This is the SFTP File Move – FED script which moves files from the SFTP site to their production locations.",
                    scriptName, MessageBoxButtons.OKCancel, MessageBoxIcon.Information).Equals(DialogResult.OK))
                {
                    Environment.Exit(0);
                }
            }

            //move files
            try
            {
                //get a list of files to be moved
                List<FileData> filesToMove = DataAccess.GetFileData(testMode);

                //move each file
                foreach (FileData f in filesToMove)
                {
                    try
                    {
                        //get all instances of the file (files with names that match the search pattern)
                        string[] filesFound = Directory.GetFiles(f.FilePathOriginal, string.Format("{0}*", f.FileNameDescription));

                        //add a record to the error report if no instances of the file are found
                        if (filesFound.Length == 0)
                        {
                            errorReport.AddRecord("File not found", f);
                        }
                        else
                        {
                            //copy and delete each instance of the file found
                            foreach (string fileFound in filesFound)
                            {
                                File.Copy(fileFound, string.Format("{0}{1}", f.FilePathCopyTo, Path.GetFileName(fileFound)), true);
                                File.Copy(fileFound, string.Format("{0}{1}", f.FilePathArchiveTo, Path.GetFileName(fileFound)), true);
                                File.Delete(fileFound);
                            }
                            DataAccess.UpdateLastProcessedTimeStamp(testMode, f.FileNameDescription);
                        }
                    }
                    catch (Exception e)
                    //add a record to the error report if there are problems copying files
                    {
                        errorReport.AddRecord(string.Format("Unable to move/copy file:  {0}", e.Message), f);
                    }
                }
            }
            catch (Exception e)
            //add a record to the error report if there are problems accessing the database to get the files to process
            {
                string message = string.Format("Unable to access database:  {0}", e.Message);
                FileData f = new FileData();

                errorReport.AddRecord(message, f);
                if (calledByJams)
                {
                    Console.WriteLine(message);
                }
                else
                {
                    MessageBox.Show(message, SCRIPT_ID, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            //publish the error report if there were errors
            errorReport.Publish();

            //processing complete
            File.WriteAllText(String.Format("{0}MBS{1}.TXT", efs.LogsFolder, SCRIPT_ID), "");
            if (!calledByJams)
            {
                MessageBox.Show("Processing complete.", scriptName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
