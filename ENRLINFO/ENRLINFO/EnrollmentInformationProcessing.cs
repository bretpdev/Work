using ENRLINFO.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ENRLINFO
{
    public class EnrollmentInformationProcessing : ScriptBase
    {
        public ProcessLogRun logRun { get; set; }

        public EnrollmentInformationProcessing(ReflectionInterface ri) : base(ri, "ENRLINFO")
        {
            logRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false);
        }

        public override void Main()
        {
            EnrollmentInformation enrollmentInformation = new EnrollmentInformation();
            while (enrollmentInformation.ShowDialog() != DialogResult.Cancel)
            {
                EnrollmentData data = enrollmentInformation.data;

                //Validate AccountIdentifier
                SystemBorrowerDemographics demos;
                try
                {
                    demos = RI.GetDemographicsFromLP22(data.AccountIdentifier);
                }
                catch (DemographicException ex)
                {
                    Dialog.Error.Ok($"Unable to get demographics from provided account identifier({data.AccountIdentifier}) on onelink.");
                    enrollmentInformation = new EnrollmentInformation();
                    continue;
                }

                //Validate School Code
                RI.FastPath($"LPSCI{data.SchoolCode}");
                if (RI.GetText(22, 3, 5) == "47004")
                {
                    Dialog.Error.Ok($"Unable to access LPSCI using school code({data.SchoolCode}) on onelink.");
                    enrollmentInformation = new EnrollmentInformation();
                    continue;
                }

                //Finish processing and then end
                Process(data, demos);
                //End
                logRun.LogEnd();
                break;
            }

        }

        public void Process(EnrollmentData data, SystemBorrowerDemographics demos)
        {
            OnelinkProcessingHelper helper = new OnelinkProcessingHelper(RI, logRun);
            LP90Data lp90Data = helper.ProcessLP90(demos, data);

            //cancel the EVSNTFLU tasks for the borrower
            bool result = helper.CancelQueuesLP8Y(demos, lp90Data);
            if(!result)
            {
                return;
            }

            //if there was no history
            if(data.EVRHistoryContainsEnrollmentInformation == false)
            {
                //get the enrollment 
                var additionalInfo = OnelinkProcessingHelper.GetAdditionalInfo();
                if (additionalInfo == null)
                {
                    //They pressed cancel
                    logRun.AddNotification("AdditionalEnrollmentInformation form cancelled out of", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                    return;
                }

                helper.HandleLG29(demos, data, additionalInfo, ScriptId);
            }
            //enter enrollment history if it is provided
            else
            {
                DateTime? minDate = helper.HandleLG02(demos);
                if(!minDate.HasValue)
                {
                    logRun.AddNotification($"Unable to get guarentee date from LG02, Account Number {demos.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Dialog.Error.Ok("Unable to get guarentee date from LG02");
                    return;
                }
                helper.HandleExistingLG29(demos, data, minDate.Value, ScriptId);
            }
        }
    }
}
