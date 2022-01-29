using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACCURINT
{
    public class ResponseFileProcessor
    {
        private FileEncryption FileEncrypter { get; set; }
        string ScriptId { get; set; }
        string ArchiveFolder { get; set; }
        private DataAccess DA { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private RunInfo CurrentRun { get; set; }
        private List<UheaaDemosRecord> UheaaDemosRecords { get; set; }
        private List<OneLinkDemosRecord> OneLinkDemosRecords { get; set; }

        public ResponseFileProcessor(FileEncryption fileEncrypter, DataAccess da, ProcessLogRun logRun, RunInfo currentRun, string scriptId, string archiveFolder)
        {
            ScriptId = scriptId;
            ArchiveFolder = archiveFolder;
            FileEncrypter = fileEncrypter;
            DA = da;
            LogRun = logRun;
            CurrentRun = currentRun;
        }

        public bool ReviewResponseFiles()
        {
            string[] responseFiles = Directory.GetFiles(EnterpriseFileSystem.TempFolder, "LN_Output_AccurintRequest*");

            //Check that there is a response file to process.
            if (responseFiles.Length == 0)
            {
                string message = $"The Accurint response file is not in the {EnterpriseFileSystem.TempFolder} folder.";
                LogRun.AddNotification(message, NotificationType.Other, NotificationSeverityType.Warning);
                Dialog.Warning.Ok(message, ScriptId);
                return false;
            }

            //Check for multiple response files.
            if (responseFiles.Length > 1)
            {
                //give the user the chance to delete old files and to confirm that all files should be processed
                string message = "More than one response file was downloaded from Accurint.";
                message += string.Format("  Review the response files in {0} and delete the file or files that aren't needed", EnterpriseFileSystem.TempFolder);
                message += " and then click OK to process all remaining files.";
                message += "  Click Cancel to end the process without processing any files.";
                if (!Dialog.Warning.OkCancel(message, ScriptId))
                    return false;

                responseFiles = Directory.GetFiles(EnterpriseFileSystem.TempFolder, "LN_Output_AccurintRequest*"); // Get files again in case user deleted some
            }

            foreach (string file in responseFiles)
            {
                DA.AddNewResponseFile(CurrentRun.RunId, file); // Sproc won't add dupes
            }


            List<ResponseFile> filesToProcess = DA.GetResponseFiles(CurrentRun.RunId).Where(p => !p.ProcessedAt.HasValue).OrderBy(o => o.ResponseFileId).ToList();
            if (FilesAreMissing(responseFiles, filesToProcess))
                return false;

            UheaaDemosRecords = DA.GetUnprocessedUHRecords(CurrentRun.RunId).Where(p => !p.ResponseAddressArcId.HasValue || !p.ResponsePhoneArcId.HasValue).ToList();
            OneLinkDemosRecords = DA.GetUnprocessedOLRecords(CurrentRun.RunId).Where(p => !p.AddressTaskQueueId.HasValue || !p.PhoneTaskQueueId.HasValue).ToList();

            //Process all files.
            foreach (var responseFile in filesToProcess)
            {
                if (!responseFile.ProcessedAt.HasValue)
                {
                    if (new FileInfo(responseFile.ResponseFileName).Length == 0)
                    {
                        if (Dialog.Info.YesNo($"The response file {responseFile} being processed is empty.  Do you want to process the remaining response files (if any)?", ScriptId))
                        {
                            DA.SetResponseFileProcessed(responseFile.ResponseFileId);
                            continue;
                        }
                        else
                            return false;
                    }
                    else if (!ProcessResponseFile(responseFile))
                        return false;
                }
            }
            return DA.SetResponseFilesProcessed(CurrentRun.RunId);
        }

        private bool FilesAreMissing(string[] foundFiles, List<ResponseFile> filesToProcess)
        {
            string missingFiles = "";
            foreach (string unprocessedFile in filesToProcess.Select(p => p.ResponseFileName).ToList())
            {
                if (!foundFiles.Contains(unprocessedFile)) // A file does not exist on T:\ that the script logged as a file to process
                    missingFiles += $"{unprocessedFile}, ";
            }

            if (!string.IsNullOrEmpty(missingFiles))
            {
                string errorMessage = $"There is one or more files that were received from Accurint for this run which are no longer on the {EnterpriseFileSystem.TempFolder} drive. Please restore the file and re-run the script. Missing files: {missingFiles.Remove(missingFiles.Length - 1, 1)}.";
                LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok(errorMessage);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes a given response file.
        /// Returns true if successful.
        /// </summary>
        /// <param name = "responseFile" > Encrypted response file that is pulled down from Accurint and decrypted</param>
        /// <returns></returns>
        private bool ProcessResponseFile(ResponseFile responseFile)
        {
            string decryptedFile = $"{EnterpriseFileSystem.TempFolder}AccurintResponseFile.txt";

            //Decrypt the response file, convert the line endings to DOS format, and archive a copy.
            if (!DataAccessHelper.TestMode)
            {
                FileEncrypter.DecryptFile(responseFile.ResponseFileName, decryptedFile);
                Unix2DosNewline(decryptedFile);
            }
            else
            {
                decryptedFile = responseFile.ResponseFileName; //don't decrypt in test mode
            }
            string archiveFile = string.Format("{0}AccurintResponseFile_{1:MMddyyyyhhmmss}.txt", ArchiveFolder, DateTime.Now);
#if !DEBUG
            File.Copy(decryptedFile, archiveFile);
#endif
            if (!DA.SetArchivedResponseFileName(responseFile.ResponseFileId, archiveFile))
            {
                LogRun.AddNotification($"Unable to record the archived response file name {archiveFile} for ResponseFileId:{responseFile.ResponseFileId}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }

            //Process the response file.
            int recordCount = 0;
            int line = 2; // Start at line two since line 1 has headers
            bool allRecordsSuccessfullyCommitted = true;
            try
            {

                using (StreamReader responseReader = new StreamReader(decryptedFile))
                {
                    //Get the header row out of the way.
                    responseReader.ReadLine();
                    string responseLine = responseReader.ReadLine();

                    //Process the remaining records.
                    while (responseLine != null)
                    {
                        recordCount++;
                        List<string> responseFields = responseLine.SplitAndRemoveQuotes(",");
                        DemographicInfo demographicInfo = new DemographicInfo(responseFields[0], responseFields[5], responseFields[9], responseFields[10], responseFields[11], responseFields[12], responseFields[13]);

                        bool? taskExists = null;
                        BorrowerTasks tasks = GetTasks(demographicInfo.Ssn, ref taskExists);
                        if (tasks == null && taskExists.HasValue && taskExists.Value == false)
                        {
                            line++;
                            responseLine = responseReader.ReadLine();
                            continue; // Task was already processed
                        }

                        if (HasCompassTasks(tasks))
                            allRecordsSuccessfullyCommitted &= ProcessUheaaResponseRecord(tasks.CompassTasks, demographicInfo);

                        if (HasOneLinkTasks(tasks))
                            allRecordsSuccessfullyCommitted &= ProcessOneLinkResponseRecord(tasks.OneLinkTasks, demographicInfo);

                        responseLine = responseReader.ReadLine();
                        line++;
                    }
                }

                //Note the number of records found in the file.
                string reportFolder = EnterpriseFileSystem.GetPath("EOJ_Accurint");
                string reportFile = string.Format(@"{0}Accurint Response\From Accurint Total.{1:yyyy-MM-dd-hhmm}", reportFolder, DateTime.Now);
                File.WriteAllText(reportFile, recordCount.ToString());
                DA.UpdateRecordsReceivedCount(CurrentRun.RunId, recordCount);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error encountered while trying to parse the response file. Error was found on line {line}. Error: {ex.Message}";
                LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                Dialog.Error.Ok($"The response file is not in the correct format. Please fix the file and re-run. Error found on line {line}. If that doesn't resolve the issue, please reach out to System Support concerning ProcessLogId: {LogRun.ProcessLogId}");
                return false;
            }

            //Clean up.
            File.Delete(responseFile.ResponseFileName);
            File.Delete(decryptedFile);
            DA.SetResponseFileProcessed(responseFile.ResponseFileId);
            return allRecordsSuccessfullyCommitted;
        }

        private bool ProcessUheaaResponseRecord(List<UheaaDemosRecord> tasks, DemographicInfo demographicInfo)
        {
            bool successfullyCommented = true;
            UheaaDemosRecord task = tasks.FirstOrDefault();

            foreach (var tsk in tasks)
            {
                if (!DA.AddReceivedDemos(tsk.DemosId, "UHEAA", demographicInfo.Address, demographicInfo.Address2, demographicInfo.City, demographicInfo.State, demographicInfo.Zip, demographicInfo.Phone))
                    LogRun.AddNotification($"Error adding received demos to DemoLogs table for DemosId:{tsk.DemosId}; Region:UHEAA. Demos received: {demographicInfo}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            //Add a queue task for address information if an address was provided.
            if (!string.IsNullOrEmpty(demographicInfo.Address))
            {
                string comment = string.Join(",", new string[] { demographicInfo.Address, demographicInfo.Address2, demographicInfo.City, demographicInfo.State, demographicInfo.Zip.SafeSubString(0, 5), "", "", "", "", "Accurint" }) + ".";
                int addressArcAddId;
                //Add the system comment.
                if (!task.ResponseAddressArcId.HasValue)
                {
                    if ((addressArcAddId = AddUheaaArc(task, comment)) == 0)
                    {
                        LogRun.AddNotification($"Unable to add ARC: ACRTN to account UH {task.AccountNumber} for address demos. Comment: {comment}; task: {task}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        successfullyCommented = false;
                    }
                    else
                    {
                        foreach (var tsk in tasks) // We only want to submit one ARC per borrower
                        {
                            tsk.ResponseAddressArcId = addressArcAddId;
                            successfullyCommented &= DA.SetUHResponseAddressArcId(tsk.DemosId, addressArcAddId);
                        }
                    }
                }
            }

            //Add a queue task for phone information if a phone was provided.
            if (!string.IsNullOrEmpty(demographicInfo.Phone))
            {
                string comment = string.Join(",", new string[] { "", "", "", "", "", "", demographicInfo.Phone, "", "", "Accurint", }) + ".";
                int phoneArcAddId;

                if (!task.ResponsePhoneArcId.HasValue)
                {
                    if ((phoneArcAddId = AddUheaaArc(task, comment)) == 0)
                    {
                        LogRun.AddNotification($"Error adding ACRTN ARC to UH account {task.AccountNumber} for phone demos. Comment: {comment}; task: {task}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        successfullyCommented = false;
                    }
                    else
                    {
                        foreach (var tsk in tasks) // We only want to submit one ARC per borrower
                        {
                            tsk.ResponsePhoneArcId = phoneArcAddId;
                            successfullyCommented &= DA.SetUHResponsePhoneArcId(tsk.DemosId, phoneArcAddId);
                        }
                    }
                }
            }
            return successfullyCommented;
        }

        private int AddUheaaArc(UheaaDemosRecord uheaaTask, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = uheaaTask.AccountNumber,
                Arc = "ACRTN",
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                ProcessOn = DateTime.Now,
                RecipientId = "",
                ScriptId = ScriptId
            };
            ArcAddResults result = arc.AddArc();
            if (result == null || !result.ArcAdded)
            {
                string message = $"There was an error adding an ACRTN ARC to borrower: {uheaaTask.AccountNumber}. Comment: {comment}; Error(s): {string.Join(",", result.Errors)}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, result.Ex);
                Dialog.Error.Ok(message);
                return 0;
            }
            return result.ArcAddProcessingId;
        }

        private bool ProcessOneLinkResponseRecord(List<OneLinkDemosRecord> tasks, DemographicInfo demographicInfo)
        {
            string workGroup = "ACURINTR";
            bool successfullyCommented = true;
            OneLinkDemosRecord task = tasks.FirstOrDefault();
            foreach (var tsk in tasks)
            {
                if (!DA.AddReceivedDemos(tsk.DemosId, "OneLINK", demographicInfo.Address, demographicInfo.Address2, demographicInfo.City, demographicInfo.State, demographicInfo.Zip, demographicInfo.Phone))
                    LogRun.AddNotification($"Error adding received demos to DemoLogs table for DemosId:{tsk.DemosId}; Region:OneLINK. Demos received: {demographicInfo}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            //Add a queue task for address information if an address was provided.
            if (!string.IsNullOrEmpty(demographicInfo.Address) && !task.AddressTaskQueueId.HasValue)
            {
                string cityStateZip = $"_{demographicInfo.City} {demographicInfo.State} {demographicInfo.Zip}";
                if (!AddLP9OQueueTask(task, tasks, workGroup, $"{demographicInfo.Address}, {cityStateZip}", true))
                    successfullyCommented = false;
            }

            //Add a queue task for phone information if a phone was provided.
            if (!string.IsNullOrEmpty(demographicInfo.Phone) && !task.PhoneTaskQueueId.HasValue)
            {
                if (!AddLP9OQueueTask(task, tasks, workGroup, demographicInfo.Phone, false))
                    successfullyCommented = false;
            }
            return successfullyCommented;
        }

        private bool AddLP9OQueueTask(OneLinkDemosRecord task, List<OneLinkDemosRecord> tasks, string workGroup, string comment, bool isAddressTask)
        {
            QueueData data = new QueueData()
            {
                AccountIdentifier = task.Ssn,
                QueueName = workGroup,
                Comment = comment
            };
            QueueResults result = data.AddQueue();
            if (result == null || !result.QueueAdded)
            {
                string taskType = isAddressTask ? "address demos" : "phone demos";
                string errorMessage = $"There was an error adding a {workGroup} queue task to the OLQTSKBLDR tables for account: {task.AccountNumber}. Task was for incoming {taskType}. Comment: {comment}. Please add the LP9O task manually.";
                LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
                return false;
            }

            if (isAddressTask)
            {
                foreach (var tsk in tasks) // We only want to submit one task per borrower
                {
                    if (!DA.SetOLAddressTaskQueueId(tsk.DemosId, result.QueueId))
                        LogRun.AddNotification($"Error setting the AddressTaskQueueId to {result.QueueId} for OneLINK DemosId {tsk.DemosId}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    else
                        tsk.AddressTaskQueueId = result.QueueId;
                }
                return true; // We still count this as a success because the task was added to the account
            }
            else
            {
                foreach (var tsk in tasks) // We only want to submit one task per borrower
                {
                    if (!DA.SetOLPhoneTaskQueueId(tsk.DemosId, result.QueueId))
                        LogRun.AddNotification($"Error setting the PhoneTaskQueueId to {result.QueueId} for OneLINK DemosId {tsk.DemosId}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    else
                        tsk.PhoneTaskQueueId = result.QueueId;
                }
                return true; // We still count this as a success because the task was added to the account
            }
        }

        private BorrowerTasks GetTasks(string ssn, ref bool? taskExists)
        {
            BorrowerTasks borrowerTasks = new BorrowerTasks();
            borrowerTasks.CompassTasks = UheaaDemosRecords.Where(p => p.Ssn == ssn && (!p.ResponseAddressArcId.HasValue || !p.ResponsePhoneArcId.HasValue)).ToList();
            borrowerTasks.OneLinkTasks = OneLinkDemosRecords.Where(p => p.Ssn == ssn && (!p.AddressTaskQueueId.HasValue || !p.PhoneTaskQueueId.HasValue)).ToList();

            if (!HasCompassTasks(borrowerTasks) && !HasOneLinkTasks(borrowerTasks))
            {
                taskExists = false;
                return null;
            }

            return borrowerTasks;
        }

        private bool HasCompassTasks(BorrowerTasks tasks)
        {
            return tasks != null && tasks.CompassTasks != null && tasks.CompassTasks.Count() > 0;
        }

        private bool HasOneLinkTasks(BorrowerTasks tasks)
        {
            return tasks != null && tasks.OneLinkTasks != null && tasks.OneLinkTasks.Count() > 0;
        }

        private void Unix2DosNewline(string decryptedFile)
        {
            string tempFile = EnterpriseFileSystem.TempFolder + "Unix2Dos";
            try
            {
                using (StreamReader reader = new StreamReader(decryptedFile))
                {
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        char previousChar = '\0';
                        while (!reader.EndOfStream)
                        {
                            char currentChar = Convert.ToChar(reader.Read());
                            if (currentChar == '\n' && previousChar != '\r')
                                writer.Write("\r\n");
                            else
                                writer.Write(currentChar);
                        }
                    }
                }
                File.Copy(tempFile, decryptedFile, true);
            }
            catch (Exception ex)
            {
                Dialog.Error.Ok(ex.Message + "\r\n\r\n" + ex.InnerException);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

    }
}
