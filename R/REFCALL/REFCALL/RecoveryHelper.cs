using System.Linq;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace REFCALL
{
    public class RecoveryHelper
    {
        public RecoveryLog Log { get; set; }

        public RecoveryHelper(RecoveryLog log)
        {
            Log = log;
            Load();
        }

        public void Reset()
        {
            this.currentStep = RecoveryStep.Startup;
            this.currentSsn = "";
            this.currentReferenceId = "";
            this.Save();
        }

        private void Load()
        {
            var value = Log.RecoveryValue ?? "";
            if (value.Count(o => o == ',') == 2)
            {
                var split = value.Split(',');
                currentStep = (RecoveryStep)(split[0].ToIntNullable() ?? 0);
                currentSsn = split[1];
                currentReferenceId = split[2];
            }
            else
            {
                currentStep = RecoveryStep.Startup;
                currentSsn = "";
                currentReferenceId = "";
            }
        }

        private void Save()
        {
            var compiledString = string.Join(",", ((int)currentStep).ToString(), currentSsn, currentReferenceId);
            this.Log.RecoveryValue = compiledString;
        }

        public enum RecoveryStep
        {
            Startup,
            WroteReferenceData,
            AddBorrowerActionCode,
            AddBorrowerActivityRecord,
            AddReferenceActionCode,
            CloseQueue,
        }

        public bool InRecovery
        {
            get { return currentStep != RecoveryStep.Startup; }
        }

        private RecoveryStep currentStep;
        public RecoveryStep CurrentStep
        {
            get
            {
                Load();
                return currentStep;
            }
            set
            {
                currentStep = value;
                Save();
            }
        }

        private string currentSsn;
        public string CurrentSsn
        {
            get
            {
                Load();
                return currentSsn;
            }
            set
            {
                currentSsn = value;
                Save();
            }
        }

        private string currentReferenceId;
        public string CurrentReferenceId
        {
            get
            {
                Load();
                return currentReferenceId;
            }
            set
            {
                currentReferenceId = value;
                Save();
            }
        }
    }
}