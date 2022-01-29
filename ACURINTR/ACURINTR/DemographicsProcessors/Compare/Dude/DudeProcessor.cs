using System;
using System.Reflection;
using System.Security.Policy;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using ACURINTR.DemographicsParsers;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
	class DudeProcessor : ProcessorBase
	{
		public DudeProcessor(ReflectionInterface ri, string userId, string scriptId, RecoveryLog recovery, ProcessLogRun logRun)
			: base(ri, userId, scriptId, recovery, logRun)
		{
		}
		/// <summary>
		/// Entry point for DudeProcessor Queue processing logic.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="assemblyName"></param>
		/// <param name="pauseOnQueueClosingError"></param>
		public override void Process(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError)
		{
			ErrorReport = new ErrorReport(queueData.Queue, base.LogRun.ProcessLogId);
			GeneralObj = new General(RI, ErrorReport, UserId, ScriptId, LogRun, DA);
			if (GRecovery.Queue.IsNullOrEmpty()) 
				GRecovery.Queue = queueData.Queue;
			ProcessOneLinkQueue(queueData, assemblyName, pauseOnQueueClosingError);
		}

		protected override void ProcessApplicablePath(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
		{
			//DUDE tasks are set up to only include one demographic type (address, phone, or e-mail),
			//so process the appropriate path based on the task demographics.
			if (task.HasAddress)
			{
				if (GRecovery.Path == GeneralRecovery.ProcessingPath.None) 
					GRecovery.Path = GeneralRecovery.ProcessingPath.Address;
                DudePathAddress addressProcessor = new DudePathAddress(RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, GRecovery, ErrorReport, LogRun, DA, UserId, ScriptId);
				addressProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			else if (task.HasHomePhone || task.HasAltPhone)
			{
				if (GRecovery.Path == GeneralRecovery.ProcessingPath.None) 
					GRecovery.Path = GeneralRecovery.ProcessingPath.HomePhone;
				DudePathPhone phoneProcessor = new DudePathPhone(RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, GRecovery, ErrorReport, LogRun, DA, UserId, ScriptId);
				phoneProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			else if (task.HasEmail)
			{
				if (GRecovery.Path == GeneralRecovery.ProcessingPath.None) 
					GRecovery.Path = GeneralRecovery.ProcessingPath.Email;
				DudePathEmail emailProcessor = new DudePathEmail(RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, GRecovery, ErrorReport, this.LogRun, DA, UserId, ScriptId);
				emailProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			GRecovery.Queue = queueData.Queue;
		}

		public static string ReplaceConsentOnelink(string consentIndicator)
        {
			string newConsentIndicator = consentIndicator;
			if(consentIndicator == "Q")
            {
				newConsentIndicator = "L";
            }
			if(consentIndicator == "X")
            {
				newConsentIndicator = "U";
            }
			return newConsentIndicator;
        }
	}
}
