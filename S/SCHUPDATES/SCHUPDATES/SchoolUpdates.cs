using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace SCHUPDATES
{
    public class SchoolUpdates
    {
        public bool HadError { get; set; }
        public string UserId { get; set; }
        public RecoveryLog Recovery { get; set; }
        public ReflectionInterface RI { get; set; }

        public SchoolUpdates(ReflectionInterface ri, string userId, string scriptId)
        {
            UserId = userId;
            RI = ri;
            Recovery = new RecoveryLog($"{RI.LogRun.ScriptId}_{UserId}_{DataAccessHelper.CurrentRegion}");
        }

        /// <summary>
        /// Start processing will display the form if in user mode
        /// </summary>
        public void Process()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string message = "";

            using (ProcessingSelection sel = new ProcessingSelection())
            {
                DialogResult result = DialogResult.OK;
                sel.ShowDialog();
                if (sel.FileProcessing)
                    result = FileProcessing();
                else
                    result = UserProcessing();
                if (result == DialogResult.OK)
                {
                    message = "Processing Complete." + (HadError ? $" Erorrs occured while processing.  Please Check Process Logger ID: {RI.LogRun.ProcessLogId}" : "");
                    Recovery.Delete();
                    Dialog.Info.Ok(message);
                }
            }

            RI.LogRun.LogEnd();
        }

        /// <summary>
        /// Determines which line was being processed, reads in the unprocessed data and calls the SessionProcessing method
        /// </summary>
        private bool? DoRecovery()
        {
            string file = Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0];
            bool doRecover = Dialog.Info.OkCancel("The application has detected you are in recovery, Select OK to continue processing or Cancel to select a new file.");
            if (doRecover)
            {
                file = ValidateFile(file);
                if (file.IsPopulated())
                {
                    int lineNumber = Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1].ToInt();
                    List<FileData> data = ReadFile(file, lineNumber); //Reads in any data not processed
                    SessionProcessing(data, lineNumber, file);
                    Dialog.Info.Ok("Recovery processing is complete.  The script will resume normal processing now.");
                }
                else
                    return null; //We don't want to return false because it will continue asking for a file
            }
            File.Delete(file);
            Recovery.Delete();
            return doRecover;
        }

        /// <summary>
        /// Opens a file dialog for the user to choose which file to process.
        /// Reads in the file and calls the SessionProcessing method.
        /// </summary>
        private DialogResult FileProcessing()
        {
            bool? doRecover = false;
            if (Recovery.RecoveryValue.IsPopulated())
                doRecover = DoRecovery();
            if (doRecover == null)
                return DialogResult.Cancel;
            if (!doRecover.Value) //Either not in recovery or chose not to recover with prompt.
            {
                string fileToProcess = null;
                while (fileToProcess == null)
                {
                    using (OpenFileDialog file = new OpenFileDialog())
                    {

                        if (file.ShowDialog() != DialogResult.OK)
                            return DialogResult.Cancel;

                        fileToProcess = ValidateFile(file.FileName);
                    }
                }

                if (fileToProcess.IsPopulated())
                {
                    List<FileData> data = ReadFile(fileToProcess);
                    Recovery.RecoveryValue = $"{fileToProcess},0";
                    SessionProcessing(data, 0, fileToProcess);
                    File.Delete(fileToProcess);
                    return DialogResult.OK;
                }
            }
            return DialogResult.Cancel;
        }

        /// <summary>
        /// Processing TX10 and TX13 for each record in the file
        /// </summary>
        private void SessionProcessing(List<FileData> data, int recoveryIndex, string fileName = "")
        {
            bool schoolSuccess = true;
            string school = "";
            foreach (FileData item in data)
            {
                if (schoolSuccess || item.SchoolId != school)
                {
                    bool tx0yProcessing = false;
                    if(item.MergedSchool.IsPopulated())
                    {
                        tx0yProcessing = TX0YProcessing(item);
                    }
                    if (tx0yProcessing || item.MergedSchool.IsNullOrEmpty())
                    {
                        TX10Processing(item);
                        TX13Processing(item);
                        if (fileName.IsPopulated())
                            Recovery.RecoveryValue = $"{fileName},{++recoveryIndex}";
                        schoolSuccess = true;
                        school = "";
                    }
                    else
                    {
                        schoolSuccess = false;
                        school = item.SchoolId;
                    }
                }
            }
        }

        private bool TX0YProcessing(FileData item)
        {
            RI.FastPath($"TX3ZCTX0Y");
            RI.PutText(8, 27, item.SchoolId);
            RI.PutText(10, 27, "000", ReflectionInterface.Key.Enter);
            if (RI.ScreenCode == "TXX05")
                RI.Hit(ReflectionInterface.Key.F10);
            if ((RI.ScreenCode == "TXX0Z" && RI.CheckForText(19, 56, "_")))
            {
                RI.PutText(19, 56, item.MergedSchool);
                RI.PutText(19, 65, item.MergedSchoolDate.Value.ToString("MMddyy"), ReflectionInterface.Key.Enter);
            }

            if (RI.MessageCode == "01214" || !RI.CheckForText(19, 56, item.MergedSchool) || !RI.MessageCode.IsIn("01005") )
            {
                string message = $"There was an error merging the old school code: {item.SchoolId} with the new merged school id: {item.MergedSchool}. Session error message: {RI.Message}";
                var result = LogErrorAndPromptUser(message, item);
                return result; //If the user said they corrected the error return true, otherwise false
            }
            if (RI.MessageCode == "01005")
                return true;
            return true; //UNDO This said true when I got the script but that seems like it could cause issues
        }

        private void TX10Processing(FileData item)
        {
            RI.FastPath($"TX3Z/ATX10");
            RI.PutText(6, 9, item.SchoolId);
            RI.PutText(8, 15, item.LoanProgram, ReflectionInterface.Key.Enter);
            if (RI.MessageCode == "01018") // A record already exists and needs to be updated
                RI.PutText(1, 4, "C", ReflectionInterface.Key.Enter);
            else if (RI.MessageCode == "50510") //School not found
            {
                var result = LogErrorAndPromptUser($"Can't update a school on TX10 until the school is created on TX0Y, current screen is: {RI.GetText(1, 4, 5)}.{RI.ScreenCode} session error message: {RI.Message}", item);
                if(!result)
                {
                    return;
                }
            }

            if(RI.ScreenCode != "TXX12")
            {
                var result = LogErrorAndPromptUser($"Expected to be on screen: TX10.TXX12, but the current screen is: {RI.GetText(1, 4, 5)}.{RI.ScreenCode} session error message: {RI.Message}", item);
                if (!result)
                {
                    return;
                }
            }
            if (RI.ScreenCode == "TXX12")
            {
                if(RI.GetText(11,17,1) != "_")
                {
                    var existingApprovedDate = RI.GetText(11, 17, 8).ToDateNullable();
                    if(existingApprovedDate.HasValue && existingApprovedDate.Value > item.ApprovalDate)
                    {
                        var result = Dialog.Warning.OkCancel("Existing approval date on TX10 is more recent than provided approval date. Please press OK to confirm. Cancelling will skip processing the rest of TX10.");
                        if(!result)
                        {
                            return;
                        }
                    }
                }

                RI.PutText(10, 17, item.TX10Approval);
                RI.PutText(11, 17, item.ApprovalDate.ToString("MMddyy"));
                if (RI.CheckForText(14, 24, "Y"))
                    RI.PutText(15, 17, item.ApprovalDate.AddYears(-1).ToString("MMddyy"));
                RI.Hit(ReflectionInterface.Key.Enter);

                //These fields are optional and were populated back when we were disbursing FFEL loans. 
                if(RI.MessageCode == "04097") //CANNOT BE IN THE FUTURE OR MORE THAN 5 YEARS PRIOR TO CURRENT DATE
                {
                    RI.PutText(14, 24, "", true);
                    RI.PutText(15, 17, "", true);
                    RI.PutText(15, 20, "", true);
                    RI.PutText(15, 23, "", ReflectionInterface.Key.Enter, true);
                }

                if(RI.MessageCode == "01329")
                {
                    var result = LogErrorAndPromptUser($"Status date must be different than what's currently on the system: {RI.Message}", item);
                    if(!result)
                    {
                        return;
                    }
                }

                if (!RI.MessageCode.IsIn("01004", "01005"))
                    LogErrorAndPromptUser($"Unable to update TX10 screen error message: {RI.Message}", item);
            }
        }

        private void TX13Processing(FileData item)
        {
            RI.FastPath($"TX3Z/ATX13");
            RI.PutText(5, 18, item.SchoolId);
            RI.PutText(7, 18, item.LoanProgram);
            RI.PutText(9, 18, item.Guarantor, ReflectionInterface.Key.Enter);
            if (RI.MessageCode == "01018") // A record already exists and needs to be updated
                RI.PutText(1, 4, "C", ReflectionInterface.Key.Enter);
            else if (RI.MessageCode == "50510") //School not found
            {
                var result = LogErrorAndPromptUser($"Can't update a school on TX13 until the school is created on TX0Y, current screen is: {RI.GetText(1, 4, 5)}.{RI.ScreenCode} session error message: {RI.Message}", item);
                if(!result)
                {
                    return;
                }
            }

            if(RI.ScreenCode != "TXX15")
            {
                var result = LogErrorAndPromptUser($"Expected to be on screen: TX13.TXX15, but the current screen is: {RI.GetText(1, 4, 5)}.{RI.ScreenCode} session error message: {RI.Message}", item);
                if(!result)
                {
                    return;
                }
            }
            if (RI.ScreenCode == "TXX15")
            {
                var existingApprovedDate = RI.GetText(11, 15, 8).ToDateNullable();
                if (existingApprovedDate.HasValue && existingApprovedDate.Value > item.ApprovalDate)
                {
                    var result = Dialog.Warning.OkCancel("Existing approval date on TX13 is more recent than provided approval date. Please press OK to confirm. Cancelling will skip processing the rest of TX10.");
                    if (!result)
                    {
                        return;
                    }
                }

                RI.PutText(8, 26, item.SchoolId);
                RI.PutText(10, 12, item.TX13Approval);
                RI.PutText(12, 16, item.TX13Reason);
                RI.PutText(11, 15, item.ApprovalDate.ToString("MMddyy"), ReflectionInterface.Key.Enter);
                //If receiving 01502, reset the approval status reason field
                if(RI.MessageCode == "01502") // If INVALID APPROVAL STATUS, STATUS REASON COMBINATION 
                {
                    //blank out the reason 
                    RI.PutText(12, 16, "", ReflectionInterface.Key.Enter, true);
                }

                if (RI.MessageCode == "01329")
                {
                    var result = LogErrorAndPromptUser($"Status date must be different than what's currently on the system: {RI.Message}", item);
                    if (!result)
                    {
                        return;
                    }
                }

                if (!RI.MessageCode.IsIn("01004", "01005"))
                    LogErrorAndPromptUser($"Unable to update TX13 screen error message: {RI.Message}", item);
            }
        }

        /// <summary>
        /// Adds any error to process logger and updates the HadError bool
        /// </summary>
        private bool LogErrorAndPromptUser(string message, FileData item)
        {
            var result = Dialog.Error.OkCancel($"Please correct the following error and then press OK then insert on the session to continue, or cancel to coninue without correcting the error. {message}, {item.ToString()}");
            if (result)
            {
                RI.PauseForInsert();
                RI.LogRun.AddNotification($"User corrected error: {message}, {item.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
            else
            {
                HadError = true;
                RI.LogRun.AddNotification($"User did NOT correct error: {message}, {item.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            return result;
        }

        /// <summary>
        /// Reads in each record skipping any record below the recovery count
        /// </summary>
        private List<FileData> ReadFile(string file, int recoveryCounter = 0)
        {
            int lineCount = 0;
            List<FileData> data = new List<FileData>();
            using (StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine();//Read out header
                while (lineCount != recoveryCounter)
                {
                    sr.ReadLine();
                    lineCount++;
                }
                while (!sr.EndOfStream)
                {
                    FileData fData = new FileData(sr.ReadLine().SplitAndRemoveQuotes(","), RI.LogRun);
                    if (fData.SchoolId.IsPopulated())
                        data.Add(fData);
                }
            }

            return data;
        }

        /// <summary>
        /// Reads the header row from the file and compares it to the correct header format
        /// </summary>
        private string ValidateFile(string fileToValidate)
        {
            using (StreamReader sr = new StreamReader(fileToValidate))
            {
                string validHeader = "SCHOOL_ID,LOAN_PROGRAM,GUARANTOR,TX10_APPROVAL,TX13_APPROVAL,APPROVAL_DATE,TX13_APPROVAL_REASON,MERGED_SCHOOL,MERGED_SCHOOL_DATE";
                string header = sr.ReadLine();
                if (header.ToUpper() != validHeader)
                {
                    string message = $"Incorrect file format. The file must have the following header: {validHeader}. Please try again.";
                    Dialog.Error.Ok(message);
                    RI.LogRun.AddNotification(message, NotificationType.FileFormatProblem, NotificationSeverityType.Critical);
                    return "";
                }
            }

            return fileToValidate;
        }

        /// <summary>
        /// Creates a csv file adding the data inputs from the SchoolInput form.
        /// Processes the data found in the newly created csv file.
        /// </summary>
        private DialogResult UserProcessing()
        {
            DialogResult result;
            using (SchoolInput input = new SchoolInput(RI.LogRun))
            {
                result = input.ShowDialog();
                if (result != DialogResult.OK)
                    return result;
                List<FileData> data = new List<FileData>();
                foreach (string loanpgm in input.Data.LoanPgms)
                    foreach (string guarantor in input.Data.Guarantors)
                        data.Add(new FileData()
                        {
                            SchoolId = input.Data.SchoolCode,
                            LoanProgram = loanpgm,
                            Guarantor = guarantor,
                            TX10Approval = input.Data.TX10Approval.Substring(0, 1),
                            TX13Approval = input.Data.TX13Approval.Substring(0, 1),
                            ApprovalDate = input.Data.ApprovalDate.Date,
                            MergedSchool = input.Data.MergedSchool,
                            MergedSchoolDate = input.Data.MergedSchoolDate,
                            TX13Reason = input.Data.TX13Reason.IsPopulated() ? input.Data.TX13Reason.Substring(0, 1) : ""
                        });

                SessionProcessing(data, 0, "");
            }
            return result;
        }
    }
}