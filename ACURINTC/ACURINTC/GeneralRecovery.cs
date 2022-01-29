using Uheaa.Common.Scripts;

namespace ACURINTC
{
    class GeneralRecovery
    {
        public DataAccess DA { get; set; }

        public enum ProcessingPath
        {
            None,
            Address,
			AltPhone,
            Email,
            HomePhone
        }

        public enum ProcessingPhase
        {
            None,
            ReviewDemographics,
            UpdateOneLink,
            OneLinkComment,
            CloseTask,
            UpdateCompass, 
            CompassComment,
            Locate
        }

        private RecoveryLog RecoveryLog;

        #region Properties
        public string Queue
        {
            get
            {
                return RecoveryLog.RecoveryValue.Split(',')[0];
            }
            set
            {
                RecoveryLog.RecoveryValue = string.Format("{0},{1},{2},{3},{4}", value, "", "", "", "");
				//Make sure the recovery table in BSYS is clear.
				DA.DeleteRecoveryRecords();
            }
        }

        public string AccountNumber
        {
            get
            {
                return RecoveryLog.RecoveryValue.Split(',')[1];
            }
            set
            {
                string[] currentValues = RecoveryLog.RecoveryValue.Split(',');
                RecoveryLog.RecoveryValue = string.Format("{0},{1},{2},{3},{4}", currentValues[0], value, "", "", "");
            }
        }

        public ProcessingPath Path
        {
            get
            {
                switch (RecoveryLog.RecoveryValue.Split(',')[2])
                {
                    case "Address":
                        return ProcessingPath.Address;
					case "AltPhone":
						return ProcessingPath.AltPhone;
                    case "Email":
                        return ProcessingPath.Email;
                    case "HomePhone":
                        return ProcessingPath.HomePhone;
                    default:
                        return ProcessingPath.None;
                }
            }
            set
            {
                string path = "";
                switch (value)
                {
                    case ProcessingPath.Address:
                        path = "Address";
                        break;
					case ProcessingPath.AltPhone:
						path = "AltPhone";
						break;
                    case ProcessingPath.Email:
                        path = "Email";
                        break;
                    case ProcessingPath.HomePhone:
                        path = "HomePhone";
                        break;
                }
                string[] currentValues = RecoveryLog.RecoveryValue.Split(',');
                RecoveryLog.RecoveryValue = string.Format("{0},{1},{2},{3},{4}", currentValues[0], currentValues[1], path, "", "");
            }
        }

        public ProcessingPhase Phase
        {
            get
            {
                switch (RecoveryLog.RecoveryValue.Split(',')[3])
                {
                    case "Review demographic info":
                        return ProcessingPhase.ReviewDemographics;
                    case "Update OneLINK demographics":
                        return ProcessingPhase.UpdateOneLink;
                    case "Add OneLINK comment":
                        return ProcessingPhase.OneLinkComment;
                    case "Update COMPASS demographics":
                        return ProcessingPhase.UpdateCompass;
                    case "Add COMPASS comment":
                        return ProcessingPhase.CompassComment;
                    case "Process locate":
                        return ProcessingPhase.Locate;
                    case "Close task":
                        return ProcessingPhase.CloseTask;
                    default:
                        return ProcessingPhase.None;
                }
            }
            set
            {
                string phase = "";
                switch (value)
                {
                    case ProcessingPhase.ReviewDemographics:
                        phase = "Review demographic info";
                        break;
                    case ProcessingPhase.UpdateOneLink:
                        phase = "Update OneLINK demographics";
                        break;
                    case ProcessingPhase.OneLinkComment:
                        phase = "Add OneLINK comment";
                        break;
                    case ProcessingPhase.UpdateCompass:
                        phase = "Update COMPASS demographics";
                        break;
                    case ProcessingPhase.CompassComment:
                        phase = "Add COMPASS comment";
                        break;
                    case ProcessingPhase.Locate:
                        phase = "Process locate";
                        break;
                    case ProcessingPhase.CloseTask:
                        phase = "Close task";
                        break;
                }
                string[] currentValues = RecoveryLog.RecoveryValue.Split(',');
                RecoveryLog.RecoveryValue = string.Format("{0},{1},{2},{3},{4}", currentValues[0], currentValues[1], currentValues[2], phase, "");
            }
        }

        public string Step
        {
            get
            {
                return RecoveryLog.RecoveryValue.Split(',')[4];
            }
            set
            {
                string[] currentValues = RecoveryLog.RecoveryValue.Split(',');
                RecoveryLog.RecoveryValue = string.Format("{0},{1},{2},{3},{4}", currentValues[0], currentValues[1], currentValues[2], currentValues[3], value);
            }
        }
        #endregion Properties

        /// <summary>
        /// Creates a facade for a RecoveryLog object, customized for the Compare method of processing.
        /// </summary>
        /// <param name="recoveryLog">The RecoveryLog object from the script (normally exposed through the BatchScriptBase.Recovery property).</param>
        public GeneralRecovery(RecoveryLog recoveryLog, DataAccess da)
        {
            RecoveryLog = recoveryLog;
            DA = da;
        }

        /// <summary>
        /// Deletes the log file if it exists, and clears out the recovery table in BSYS.
        /// </summary>
        public virtual void Delete()
        {
			DA.DeleteRecoveryRecords();
            RecoveryLog.Delete();
        }
    }
}