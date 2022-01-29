using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHSETUP
{
    class RecoveryProcessor
    {
        public enum RecoveryPhases
        {
            GenerateApprovalLetter,
            GenerateApprovalLetterForBankAccountHolder,
            GenerateDenialLetter,
            EnterApprovalLetterComments,
            EnterApprovalLetterCommentsForBankAccountHolder,
            EnterDenialLetterComments,
            AddOptionNoLoansEligibleComment,
            AddOptionAddACH,
            AddOptionProcessCheckByPhone,
            ChangeOptionAddRemoveTS7OUpdate,
            ChangeOptionChangeExistingDeactivateRecord,
            ChangeOptionChangeExistingCreateNewRecord,
            ChangeOptionChangeExistingCheckByPhone,
            ChangeOptionChangeExistingCommentAdd,
            RemoveOptionDeactivateRecord,
            SuspendOptionSuspendRecord,
            SuspendOptionCommentAdd
        }

        private const string RECOVERY_LOG_FILE_NAME = "Compass ACH Setup Recovery Log (Commercial).txt";
        private readonly string RECOVERY_LOG_LOCATION_AND_NAME;
        private const int RECOVERY_LOG_FILE_HANDLE = 10;
        private bool RecoveryChecked { get; set; } = false;
        private bool InRecovery { get; set; }
        private ProcessLogRun LogRun { get; set; }

        public RecoveryData Data { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RecoveryProcessor(ProcessLogRun logRun)
        {
            RECOVERY_LOG_LOCATION_AND_NAME = string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, RECOVERY_LOG_FILE_NAME);
            //create data object
            Data = new RecoveryData();
            LogRun = logRun;
        }

        //pulls all recovery data into memory
        private void GatherRecoveryLogData()
        {
            using StreamReader sr = new StreamReader(RECOVERY_LOG_LOCATION_AND_NAME);
            List<string> recData = sr.ReadLine().SplitAndRemoveQuotes(",");
            Data.UserSelectedOption = (UserProvidedMainMenuData.UserSelectedACHAction)Enum.Parse(typeof(UserProvidedMainMenuData.UserSelectedACHAction), recData[0]);
            Data.SSNOrAccountNumber = recData[1];
            Data.FirstName = recData[2];
            while (!sr.EndOfStream)
                Data.Phases.Add((RecoveryPhases)Enum.Parse(typeof(RecoveryPhases), sr.ReadLine()));
        }

        /// <summary>
        /// Checks if phase is already in log and if script is in recovery. If not in recovery then it returns false.  If it is in recovery and it's the phase is not in the log then it returns false.  Else, it returns true.
        /// </summary>
        public bool PhaseAlreadyInLog(RecoveryPhases phase)
        {
            if (!InRecovery)
                return false;
            else
            {
                List<RecoveryPhases> results = Data.Phases.Where(p => p == phase).ToList();
                if (results.Count == 0)
                    return false; //phase not found in recovery log
                else
                    return true; //phase found in recovery log
            }
        }

        /// <summary>
        /// Add another phase to the log file.
        /// </summary>
        public void UpdateLogWithNewPhase(RecoveryPhases phase)
        {
            using StreamWriter sw = new StreamWriter(RECOVERY_LOG_LOCATION_AND_NAME, true);
            sw.WriteLine(Enum.GetName(typeof(RecoveryPhases), phase));
        }

        /// <summary>
        /// Updates recovery log with borrower and user selection option data
        /// </summary>
        public void UpdateLogWithUserAndOptionData(UserProvidedMainMenuData.UserSelectedACHAction userSelectedOption, string SSNOrAccountNumber, string FirstName)
        {
            using StreamWriter sw = new StreamWriter(RECOVERY_LOG_LOCATION_AND_NAME);
            sw.WriteLine($"{Enum.GetName(typeof(UserProvidedMainMenuData.UserSelectedACHAction), userSelectedOption)}, {SSNOrAccountNumber}, {FirstName}");
        }

        /// <summary>
        /// Checks if script is in recovery.
        /// </summary>
        /// <returns></returns>
        public bool IsInRecovery()
        {
            if (RecoveryChecked)
                return InRecovery;
            else
            {
                //recovery checked
                RecoveryChecked = true;
                //check if log exists
                if (File.Exists(RECOVERY_LOG_LOCATION_AND_NAME))
                {
                    using StreamReader sr = new StreamReader(RECOVERY_LOG_LOCATION_AND_NAME);
                    List<string> data = sr.ReadLine().SplitAndRemoveQuotes(",");
                    if (data.Count == 3)
                    {
                        string message = $"The script has found a recovery log. Do you wish to recover?{Environment.NewLine}{Environment.NewLine}NOTE: All data will still need to be entered by you. " +
                        $"However, system processing won't be duplicated.{Environment.NewLine}{Environment.NewLine}Log File Specifics:{Environment.NewLine}- User Option Selected: {data[0]}{Environment.NewLine}" +
                        $"- SSN Or Account #:{data[1]}{Environment.NewLine}- First Name:{data[2]}{Environment.NewLine}";
                        if (MessageBox.Show(message, "Recover?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            sr.Dispose();
                            DeleteRecoveryLog();
                            InRecovery = false;
                            return InRecovery;
                        }
                        else
                        {
                            GatherRecoveryLogData();
                            InRecovery = true;
                            return InRecovery;
                        }
                    }
                    else
                    {
                        InRecovery = false;
                        return InRecovery;
                    }
                }
                else
                {
                    InRecovery = false;
                    return InRecovery;
                }
            }
        }

        /// <summary>
        /// Deletes log file if log file exists.
        /// </summary>
        public void DeleteRecoveryLog()
        {
            if (File.Exists(RECOVERY_LOG_LOCATION_AND_NAME))
                FileHelper.DeleteFile(RECOVERY_LOG_LOCATION_AND_NAME, LogRun.ProcessLogId, LogRun.Assembly);
        }
    }
}