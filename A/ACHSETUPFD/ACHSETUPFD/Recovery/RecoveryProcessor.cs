using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;

namespace ACHSETUPFD
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

        private const string RECOVERY_LOG_FILE_NAME = "Compass ACH Setup Recovery Log (Federal).txt";
        private readonly string RECOVERY_LOG_LOCATION_AND_NAME;
        private const int RECOVERY_LOG_FILE_HANDLE = 10;
        private bool _recoveryChecked = false;
        private bool _inRecovery;
        private ProcessLogData _processLogData;

        public RecoveryData Data { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RecoveryProcessor(ProcessLogData processLogData)
            : base()
        {
            RECOVERY_LOG_LOCATION_AND_NAME = string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, RECOVERY_LOG_FILE_NAME);
            //create data object
            Data = new RecoveryData();
            _processLogData = processLogData;
        }

        //pulls all recovery data into memory
        private void GatherRecoveryLogData()
        {
            using (StreamReader sr = new StreamReader(RECOVERY_LOG_LOCATION_AND_NAME))
            {
                List<string> recData = sr.ReadLine().SplitAndRemoveQuotes(",");
                Data.UserSelectedOption = (UserProvidedMainMenuData.UserSelectedACHAction)Enum.Parse(typeof(UserProvidedMainMenuData.UserSelectedACHAction), recData[0]);
                Data.SSNOrAccountNumber = recData[1];
                Data.FirstName = recData[2];
                while (!sr.EndOfStream)
                {
                    Data.Phases.Add((RecoveryPhases)Enum.Parse(typeof(RecoveryPhases), sr.ReadLine()));
                }
            }
        }

        /// <summary>
        /// Checks if phase is already in log and if script is in recovery. If not in recovery then it returns false.  If it is in recovery and it's the phase is not in the log then it returns false.  Else, it returns true.
        /// </summary>
        /// <param name="phase"></param>
        /// <returns></returns>
        public bool PhaseAlreadyInLog(RecoveryPhases phase)
        {
            if (!_inRecovery)
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
        /// <param name="phase"></param>
        public void UpdateLogWithNewPhase(RecoveryPhases phase)
        {
            using (StreamWriter sw = new StreamWriter(RECOVERY_LOG_LOCATION_AND_NAME, true))
                sw.WriteLine(Enum.GetName(typeof(RecoveryPhases), phase));
        }

        /// <summary>
        /// Updates recovery log with borrower and user selection option data
        /// </summary>
        /// <param name="userSelectedOption"></param>
        /// <param name="SSNOrAccountNumber"></param>
        /// <param name="FirstName"></param>
        public void UpdateLogWithUserAndOptionData(UserProvidedMainMenuData.UserSelectedACHAction userSelectedOption, string SSNOrAccountNumber, string FirstName)
        {
            using (StreamWriter sw = new StreamWriter(RECOVERY_LOG_LOCATION_AND_NAME))
                sw.WriteLine(string.Format("{0}, {1}, {2}", Enum.GetName(typeof(UserProvidedMainMenuData.UserSelectedACHAction), userSelectedOption), SSNOrAccountNumber, FirstName));
        }

        /// <summary>
        /// Checks if script is in recovery.
        /// </summary>
        /// <returns></returns>
        public bool IsInRecovery()
        {
            if (_recoveryChecked)
                return _inRecovery;
            else
            {
                //recovery checked
                _recoveryChecked = true;
                //check if log exists
                if (File.Exists(RECOVERY_LOG_LOCATION_AND_NAME))
                {
                    using (StreamReader sr = new StreamReader(RECOVERY_LOG_LOCATION_AND_NAME))
                    {
                        List<string> data = sr.ReadLine().SplitAndRemoveQuotes(",");
                        if (data.Count == 3)
                        {
                            string message = string.Format("The script has found a recovery log. Do you wish to recover?{0}{0}NOTE: All data will still need to be entered by you."
                            + "However, system processing won't be duplicated.{0}{0}Log File Specifics:{0}- User Option Selected: {1}{0}- SSN Or Account #:{2}{0}- First Name:{3}{0}",
                            Environment.NewLine, data[0], data[1], data[2]);
                            if (MessageBox.Show(message, "Recover?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                sr.Dispose();
                                DeleteRecoveryLog();
                                _inRecovery = false;
                                return _inRecovery;
                            }
                            else
                            {
                                GatherRecoveryLogData();
                                _inRecovery = true;
                                return _inRecovery;
                            }
                        }
                        else
                        {
                            _inRecovery = false;
                            return _inRecovery;
                        }
                    }
                }
                else
                {
                    _inRecovery = false;
                    return _inRecovery;
                }
            }
        }

        /// <summary>
        /// Deletes log file if log file exists.
        /// </summary>
        public void DeleteRecoveryLog()
        {
            if (File.Exists(RECOVERY_LOG_LOCATION_AND_NAME))
                FileHelper.DeleteFile(RECOVERY_LOG_LOCATION_AND_NAME, _processLogData.ProcessLogId, _processLogData.ExecutingAssembly);
        }
    }
}
