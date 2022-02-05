using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;


namespace CFUTDTQTSK
{
    public class CreateFutureDatedQueueTasks : ScriptBase
    {
        public ProcessLogRun LogRun { get; set; }

        public CreateFutureDatedQueueTasks(ReflectionInterface ri)
            :base(ri, "CFUTDTQTSK", DataAccessHelper.Region.Uheaa)
        {
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
        }

        public override void Main()
        {
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;

            using (UserInput input = new UserInput(LogRun, RI))
            {
                if (input.ShowDialog() != DialogResult.OK)
                    return;

                //Add the record
                ArcAddResults result = input.UserData.AddArc();
                if(!result.ArcAdded)
                {
                    string message = $"Error adding Arc: {input.UserData.Arc} to Borrower: {input.UserData.AccountNumber}; Error: {string.Join(",", result.Errors)}; Ex: {result.Ex}";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
                    MessageBox.Show($"Error adding data to ArcAdd database, Please contact Systems Support and reference Process Log Id: {ProcessLogData.ProcessLogId}");
                    return;
                }

                Dialog.Info.Ok("Process Complete", "CFUTDTQTSK");
            }
        }
    }
}