using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UsBankImport
{
    /// <summary>
    /// Maintains the visual log of the application, as well as logging notifications in ProcessLogger.
    /// </summary>
    public class Log
    {
        public List<string> LogItems { get; private set; }
        public ProcessLogData ProcessLogData { get; private set; }
        private IRebindable rebind;
        public Log(bool testMode, IRebindable rebind)
        {
            this.rebind = rebind;
            DataAccessHelper.CurrentMode = testMode ? DataAccessHelper.Mode.Test : DataAccessHelper.Mode.Live;
            ProcessLogData = ProcessLogger.RegisterApplication("UsBankImport", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            LogItems = new List<string>();
        }
        /// <summary>
        /// Add an item to the log.
        /// </summary>
        public void LogItem(string item, params string[] args)
        {
            LogItems.Insert(0, DateTime.Now.ToString("hh:mm:ss") + " - " + string.Format(item, args));
            if (rebind != null)
                rebind.Rebind();
        }
        /// <summary>
        /// Add an item to the log, and add a notification to ProcessLogger.
        /// </summary>
        public void LogEojItem(string item, params string[] args)
        {
            item = string.Format(item, args);
            LogItem(item);
            ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, item, NotificationType.EndOfJob, NotificationSeverityType.Informational);
        }
        /// <summary>
        /// Returns the given string with a timestamp at the end.
        /// </summary>
        private string AddTimestamp(string fileName)
        {
            var time = new FileInfo(fileName).CreationTime;
            return fileName + " (" + time.ToString("MM/dd/yy hh:mm") + ")";
        }

        #region Specific Logging Methods
        /// <summary>
        /// Logs that processing has begun.
        /// </summary>
        public void Start()
        {
            LogItem("Start (PL#{0})", ProcessLogData.ProcessLogId.ToString());
        }
        /// <summary>
        /// Logs that a zip file has begun being processed.
        /// </summary>
        /// <param name="zipFile"></param>
        public void BeginZip(string zipFile)
        {
            LogItem("Started Processing {0}", AddTimestamp(zipFile));
        }
        /// <summary>
        /// Logs that the given zip did not contain the necessary csv file.
        /// </summary>
        public void ZipHasNoCsvFile(string zipFile)
        {
            LogEojItem("The DEN-561480.csv file was not located in the zip file ({0})", AddTimestamp(zipFile));
        }
        /// <summary>
        /// Logs that the given zip contained the necessary csv file, but was empty.
        /// </summary>
        public void ZipHasEmptyCsv(string zipFile)
        {
            LogEojItem("The DEN-561480.csv file contains no data to be processed.  ({0})", AddTimestamp(zipFile));
        }
        /// <summary>
        /// Logs that a referenced image was not found in the zip file.
        /// </summary>
        public void ImageNotFound(string zipFile, string imageName)
        {
            LogEojItem("Unable to locate image #{0} in file ({1})", imageName, zipFile);
        }
        /// <summary>
        /// Logs that an image was successfully extracted from the zip file.
        /// </summary>
        public void ExtractedImage(string imageName, string newName)
        {
            LogItem("Extracted image {0} as {1}.", imageName, newName);
        }
        /// <summary>
        /// Logs that a control file has been created.
        /// </summary>
        public void WroteControlFile(string controlFileName)
        {
            LogItem("Created control file {0}", controlFileName);
        }
        /// <summary>
        /// Logs that the given zip file is done being processed.
        /// </summary>
        public void EndZip(string zipFile, string archiveLocation)
        {
            LogItem("Completed processing of ({0}).  File archived as ({1})", zipFile, archiveLocation);
        }
        /// <summary>
        /// Logs the end of processing, and ends the run in ProcessLogger.
        /// </summary>
        public void Finished()
        {
            LogItem("Processing Complete.");
            ProcessLogger.LogEnd(ProcessLogData.ProcessLogId);
        }
        #endregion
    }
}
