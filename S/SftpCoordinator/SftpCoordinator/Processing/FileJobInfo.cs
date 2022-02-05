using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace SftpCoordinator
{
    public class FileJobInfo
    {
        public InvalidFile InvalidFileInfo { get; internal set; }
        public ActivityLog ActivityLogInfo { get; internal set; }
        public List<string> PendingDeletions { get; internal set; }
        protected Project Project { get; set; }
        protected ProjectFile ProjectFile { get; set; }
        protected string SourceFilePath { get; set; }
        protected RunHistory RunHistory { get; set; }
        protected ProcessLogRun PLR { get; set; }
        public FileJobInfo(Project p, ProjectFile pf, RunHistory rh, string file, ProcessLogRun plr)
        {
            Project = p;
            ProjectFile = pf;
            SourceFilePath = file;
            RunHistory = rh;
            PLR = plr;

            ActivityLogInfo = new ActivityLog();
            ActivityLogInfo.RunHistoryId = rh.RunHistoryId;
            ActivityLogInfo.ProjectFileId = pf.ProjectFileId;
            ActivityLogInfo.SourcePath = file;
            //the filename or extension may change during one of the steps, so we will only set the destination folder for now
            ActivityLogInfo.DestinationPath = pf.CalculatedDestinationRoot;
        }

        public bool Process()
        {
            //this is the order these steps are processed in
            List<FileProcessStep> steps = new List<FileProcessStep>
            {
                new CopyFileToTemp(),
                new DecryptFile(),
                new FixLineEndings(),
                new CompressFile(),
                new EncryptFile(),
                new CopyFileFromTemp()
            };
            FileOpResults results = FileOpResults.Initial(ActivityLogInfo.SourcePath);
            foreach (var step in steps)
            {
                results = step.Process(results, ProjectFile, Project, RunHistory);
                step.SetActivityLogProperty(ActivityLogInfo, results.ProcessSuccessful);
                if (!results.ErrorMessage.IsNullOrEmpty())
                    LogError(results.ErrorMessage);
                if (results.ProcessSuccessful == false)
                    return false;
            }
            cleanupResults = results;
            //if everything is successful, results.FilePath will reflect the destination
            ActivityLogInfo.DestinationPath = results.FilePath;
            ActivityLogInfo.PreDecryptionArchiveLocation = results.PreDecryptionArchiveLocation;
            ActivityLogInfo.PreEncryptionArchiveLocation = results.PreEncryptionArchiveLocation;
            return true;
        }
        FileOpResults cleanupResults;
        public bool Cleanup()
        {
            ActivityLogInfo.DeleteSuccessful = CleanupFile(this.ProjectFile.IsArchiveJob);
            if (ActivityLogInfo.DeleteSuccessful == false)
                return false;
            ActivityLogInfo.Successful = true;
            ActivityLog.LogActivity(ActivityLogInfo);
            return true;
        }
        protected bool? CleanupFile(bool isArchiveJob)
        {
            bool? fileDeleted = null;
            foreach (string path in cleanupResults.CleanupPaths)
            {
                if (path.ToLower() == ActivityLogInfo.DestinationPath.ToLower())
                    continue; //don't delete if this is the destination
                try
                {
                    if (!isArchiveJob)
                    {
                        GenericFile.Delete(path);
                        if (fileDeleted == null)
                            fileDeleted = true;
                    }
                }
                catch (Exception ex)
                {
                    LogError($"Error deleting cleanup file: {ex.ToString()}");
                    fileDeleted = false;
                }
            }
            return fileDeleted;
        }

        protected void LogError(string message)
        {
            InvalidFile inv = new InvalidFile();
            inv.FilePath = SourceFilePath;
            try
            {
                inv.FileTimestamp = GenericFile.GetLastWriteTime(SourceFilePath);
            }
            catch (Exception)
            {
                //we might get an exception if the file doesn't exist.  don't worry about it
            }
            inv.ErrorMessage = message;
            inv = InvalidFile.Insert(inv);

            ActivityLogInfo.InvalidFileId = inv.InvalidFileId;
            ActivityLogInfo.Successful = false;
            ActivityLog.LogActivity(ActivityLogInfo);
            PLR.AddNotification($"The following file was not processed successfully: {inv.FilePath} \r\n Error Message: {inv.ErrorMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }
    }
}
