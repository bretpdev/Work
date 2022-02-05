using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace AACERRFED
{
    public class AacErrorFixFed : FedBatchScript
    {
        public AacErrorFixFed(ReflectionInterface ri)
            : base(ri, "AACERRFED", "ERR_BU01", "EOJ_BU01", new List<string>())
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = Recovery.RecoveryValue.IsNullOrEmpty() ? "Please select the error file" : "Select the " + Recovery.RecoveryValue + " to recover";
            dialog.InitialDirectory = EnterpriseFileSystem.TempFolder;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ProcessFile(dialog.FileName);
            }

        }

        private void ProcessFile(string fileName)
        {
            if (!ValidateFile(fileName))
                return;
        }

        /// <summary>
        /// Verifies that the file is not empty and has the correct data fields
        /// </summary>
        /// <param name="fileName">Selected file to process</param>
        /// <returns>True if everything is fine, false if there is a problem with the file.</returns>
        private bool ValidateFile(string fileName)
        {
            if (new FileInfo(fileName).Length == 0)
            {
                string message = string.Format("The {0} file is empty", fileName);
                Dialog.Info.Ok(message + ", please check the file and try again", "Empty File");
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.EmptyFile, NotificationSeverityType.Informational);
                return false;
            }

            

            return true;
        }
    }
}
