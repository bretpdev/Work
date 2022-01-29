using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CMP30DYSKP
{
    /// <summary>
    /// Processes the associated SAS file by parsing it
    /// and then leaving a work group task in OneLINK
    /// via LP9O.
    /// </summary>
    public class FileProcessor
    {
        private string ScriptId { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private ReflectionInterface RI { get; set; }
        private DataAccess DA { get; set; }
        private string UserId { get; set; }
        private const string R2_FILE = "ULWK17.LWK17R2*";
        private string[] FilesToProcess { get; set; }
        public FileProcessor(string scriptId, ReflectionInterface ri, ProcessLogRun logRun)
        {
            ScriptId = scriptId;
            LogRun = logRun;
            RI = ri;
            UserId = RI.UserId;
            DA = new DataAccess(LogRun);
        }

        /// <summary>
        /// Initiates the file processing.
        /// </summary>
        public int Run()
        {
            Console.WriteLine($"Checking to see if {R2_FILE} file is in {EnterpriseFileSystem.GetPath("FTP")}");
            if (!ProcessingFileExists())
            {
                Console.WriteLine("No records found to process. Ending script run.");
                return 0; // Return success if there are no files to process
            }
            else
                return ProcessFile();
        }

        /// <summary>
        /// Verifies whether there are any SAS files to be processed.
        /// Returns true if so, false otherwise.
        /// </summary>
        private bool ProcessingFileExists()
        {
            FilesToProcess = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, R2_FILE);
            if (FilesToProcess == null || FilesToProcess.Count() == 0)
            {
                LogRun.AddNotification($"No {R2_FILE} file was found.", NotificationType.EndOfJob, NotificationSeverityType.Warning);
                return false;
            }


            if (!FilesToProcess.Any(p => new FileInfo(p).Length > 0)) // Check if all the files are empty
            {
                LogRun.AddNotification($"All {R2_FILE} files are empty.", NotificationType.EndOfJob, NotificationSeverityType.Warning);
                DeleteFiles();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Uses helper methods to read and work the file.
        /// Returns the number of errors encountered.
        /// </summary>
        private int ProcessFile()
        {
            Console.WriteLine($"Processing files with UT ID {UserId}.");
            List<SkipTask> tasksFromFiles = ReadFiles();
            int errorCount = 0;
            foreach(SkipTask task in tasksFromFiles)
            {
                if (!ProcessTask(task))
                    errorCount++;
            }

            Console.WriteLine($"Finished processing tasks. Number of errors encountered: {errorCount}");
            if (DeleteFiles())
                return errorCount;
            else
                return ++errorCount;
        }

        /// <summary>
        /// Deletes the SAS files associated with this script.
        /// Returns false if unable to delete successfully.
        /// </summary>
        /// <returns></returns>
        private bool DeleteFiles()
        {
            try
            {
                foreach (string file in FilesToProcess)
                {
                    var deleteResults = Repeater.TryRepeatedly(() => FS.Delete(file));
                    if (!deleteResults.Successful)
                    {
                        Console.WriteLine($"Error while trying to delete {file} file. See PL # {LogRun.ProcessLogId} for more details.");
                        LogRun.AddNotification($"Error deleting {file} file", NotificationType.FileFormatProblem, NotificationSeverityType.Critical);
                    }
                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"Error deleting {R2_FILE} files", NotificationType.FileFormatProblem, NotificationSeverityType.Critical, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Processes a skip task by adding a work group task
        /// to LP9O. Will only add a task if it does not exist
        /// in CT30. For this functionality to fully prevent 
        /// duplication, the JAMS must have a dependency on the
        /// CT30 refresh.
        /// </summary>
        private bool ProcessTask(SkipTask task)
        {
            if (DA.TaskAlreadyExists(task))
                return true;

            string comment = task.Task == "BRWRCALS" ? "30-Day Compass Skip Borrower Attempt" : "30-Day Compass Skip Accurint Attempt";
            if (!RI.AddQueueTaskInLP9O(task.Ssn, task.Task, null, comment))
            {
                LogRun.AddNotification($"Error adding task {task.Task} to the account of borrower {task.Ssn}. UT ID: {UserId}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets all SkipTask data from the SAS files in FTP.
        /// Returns a distinct list of the records to process.
        /// </summary>
        private List<SkipTask> ReadFiles()
        {
            List<SkipTask> tasksToWork = new List<SkipTask>();
            foreach (string file in FilesToProcess)
            {
                tasksToWork.AddRange(ReadFile(file));
            }
            return tasksToWork.Distinct().ToList();
        }

        /// <summary>
        /// Parses SkipTask data out of text file.
        /// </summary>
        private List<SkipTask> ReadFile(string filePath)
        {
            Console.WriteLine($"Reading file {filePath}.");
            List<SkipTask> tasks = new List<SkipTask>();
            if (File.Exists(filePath))
            {
                int lineCount = 1;
                try
                {
                    using (StreamR sr = new StreamR(filePath))
                    {
                        string line = sr.ReadLine(); // Skip head line
                        while ((line = sr.ReadLine()) != null)
                        {
                            List<string> fieldsInFile = line.SplitAndRemoveQuotes(",");
                            SkipTask st = new SkipTask
                            {
                                Ssn = fieldsInFile[0].Trim(),
                                Task = fieldsInFile[1].Trim()
                            };
                            tasks.Add(st);
                            lineCount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogRun.AddNotification($"Error reading in file: {filePath}. Error occurred at line {lineCount}", NotificationType.FileFormatProblem, NotificationSeverityType.Critical, ex);
                    Console.WriteLine($"Error encountered while trying to process file {filePath}. See PL # {LogRun.ProcessLogId}.");
                    return null;
                }
            }
            return tasks;
        }
    }
}
