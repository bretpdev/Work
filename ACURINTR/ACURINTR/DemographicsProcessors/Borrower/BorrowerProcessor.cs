using System;
using ACURINTR.DemographicsParsers;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
	class BorrowerProcessor : ProcessorBase
	{
		public BorrowerProcessor(ReflectionInterface ri, string userId, string scriptId, RecoveryLog recovery, ProcessLogRun logRun)
			: base(ri, userId, scriptId, recovery, logRun)
		{
		}
		/// <summary>
		/// Entry point for BorrowerProcessor Queue processing logic.
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
			if (queueData.System == QueueData.Systems.COMPASS)
				ProcessCompassQueue(queueData, assemblyName, pauseOnQueueClosingError);
			else
				ProcessOneLinkQueue(queueData, assemblyName, pauseOnQueueClosingError);
		}

		protected override void ProcessApplicablePath(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
		{
			if (GRecovery.Path == GeneralRecovery.ProcessingPath.None) 
				GRecovery.Path = GeneralRecovery.ProcessingPath.Address;
			if (task.HasAddress && GRecovery.Path == GeneralRecovery.ProcessingPath.Address)
			{
				BorrowerPathAddress addressProcessor = new BorrowerPathAddress(RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, GRecovery, ErrorReport, LogRun, DA, UserId, ScriptId);
				addressProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			if (GRecovery.Path == GeneralRecovery.ProcessingPath.Address) 
				GRecovery.Path = GeneralRecovery.ProcessingPath.HomePhone;
			//A borrower task may have both home phone and alt phone, but the processor is set up
			//to deal with just one--whichever one is present. So before calling the phone processor,
			//make sure the home phone and alt phone aren't both present.
			string tempPhone = task.Demographics.AlternatePhone;
			task.Demographics.AlternatePhone = "";
			if (task.HasHomePhone && GRecovery.Path == GeneralRecovery.ProcessingPath.HomePhone)
			{
				BorrowerPathPhone phoneProcessor = new BorrowerPathPhone(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, GRecovery, ErrorReport, LogRun, DA, UserId, ScriptId);
				phoneProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			if (GRecovery.Path == GeneralRecovery.ProcessingPath.HomePhone) 
				GRecovery.Path = GeneralRecovery.ProcessingPath.AltPhone;
			task.Demographics.AlternatePhone = tempPhone;
			tempPhone = task.Demographics.PrimaryPhone;
			task.Demographics.PrimaryPhone = "";
			if (task.HasAltPhone && GRecovery.Path == GeneralRecovery.ProcessingPath.AltPhone)
			{
				BorrowerPathPhone phoneProcessor = new BorrowerPathPhone(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, GRecovery, ErrorReport, LogRun, DA, UserId, ScriptId);
				phoneProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			task.Demographics.PrimaryPhone = tempPhone;
			if (GRecovery.Path == GeneralRecovery.ProcessingPath.AltPhone) 
				GRecovery.Path = GeneralRecovery.ProcessingPath.Email;
			if (task.HasEmail && GRecovery.Path == GeneralRecovery.ProcessingPath.Email)
			{
				BorrowerPathEmail emailProcessor = new BorrowerPathEmail(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, GRecovery, ErrorReport, LogRun, DA, UserId, ScriptId);
				emailProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			GRecovery.Queue = queueData.Queue;
		}
	}
}
