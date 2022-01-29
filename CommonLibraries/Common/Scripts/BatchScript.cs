using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.Scripts
{
    public class BatchScript : ScriptBase
    {
        private RecoveryLog recovery;
        protected override RecoveryLog Recovery
        {
            get
            {
                return recovery;
            }
        }

        private EndOfJobReport EojReport;
        public EndOfJobReport Eoj
        {
            get
            {
                return EojReport;
            }
            set
            {
                EojReport = value;
            }
        }

        private ErrorReport ErrReport;
        public ErrorReport Err
        {
            get
            {
                return ErrReport;
            }
        }
        private string userId;
        private Reflection.Session Rs;

        protected bool CalledByJams
        {
            get
            {
                return RI.CalledByJams;
            }
        }

        protected new string UserId
        {
            get
            {
                return userId;
            }
        }

        protected BatchScript(ReflectionInterface ri, string scriptId, string errorReportFileSystemKey, string eojFileSystemKey, IEnumerable<string> eojFields, DataAccessHelper.Region region)
            : base(ri, scriptId, region)
        {
            DataAccessHelper.CurrentRegion = region;
            Rs = ri.ReflectionSession;
            string scriptName = ScriptHelper.GetScriptName(scriptId).Replace("_", new string[] { @"\", @"/" });

            try
            {
                RI.FastPath("PROF");
                userId = RI.GetText(2, 49, 7);
            }
            catch
            {
                string message = string.Format("The {0} script was unable to access the system to get the user ID and/or validate the region.  Resolve the system access issue and run the script again.", scriptId);
                if (CalledByJams)
                {
                    throw new Exception(message);
                }
                else
                {
                    throw new StupRegionException(message);
                }
            }

            recovery = new RecoveryLog(string.Format("{0}_{1}", scriptId, UserId));
            EojReport = eojFileSystemKey == null ? null : new EndOfJobReport(scriptName, eojFileSystemKey, eojFields, UserId);
            ErrReport = errorReportFileSystemKey == null ? null : new ErrorReport(scriptName, errorReportFileSystemKey, UserId);
        }

        protected void StartupMessage(string message)
        {
            if (!CalledByJams)
            {
                if (MessageBox.Show(message, ScriptId, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                {
                    EndDllScript();
                }
            }
        }

        protected void NotifyAndEnd(string format, params object[] args)
        {
            string reportFolder = EnterpriseFileSystem.GetPath("ERR_BU35");
            string scriptName = ScriptHelper.GetScriptName(ScriptId);
            string errorFile = string.Format("{0}{1} {2:MM-dd-yyyy HH.mm}.txt", reportFolder, scriptName, DateTime.Now);
            string message = string.Format(format, args);

            FS.WriteAllText(errorFile, message);

            if (CalledByJams)
            {
                throw new Exception(message);
            }
            else
            {
                MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndDllScript();
            }
        }

        protected void ProcessingComplete(string message = "Processing Complete")
        {
            if (Err != null) 
                Err.Publish();
            if (Eoj != null)
            {
                if (RI.ProcessLogData != null && RI.ProcessLogData.ProcessLogId > 0)
                    Eoj.PublishProcessLogger(RI.ProcessLogData);

                Eoj.Publish();
            }

            FS.Create(string.Format("{0}MBS{1}_{2}_{3}.TXT", EnterpriseFileSystem.LogsFolder, ScriptId, userId, DateTime.Now.ToString("MMddyyyy_hhmmss")));
            Recovery.Delete();

            if (!CalledByJams)
            {
                MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            EndDllScript();
        }
    }
}
